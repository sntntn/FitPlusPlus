<template>
  <div class="page">
    <!-- Booking Form -->
    <div class="booking-form">
      <h2>Create a Group Training</h2>

      <div class="button-row">
        <button @click="showSchedule = true">Your Schedule</button>
      </div>

      <div v-if="showSchedule" class="modal-overlay" @click.self="showSchedule = false">
        <div class="modal-content">
          <button class="close-btn" @click="showSchedule = false">×</button>
          <FullCalendar :options="calendarOptions"/>
        </div>
      </div>

      <label>Training Name</label>
      <input type="text" v-model="trainingName" placeholder="Enter training name"/>

      <label>Training Description</label>
      <textarea rows="4" v-model="trainingDescription" placeholder="Enter training description"></textarea>

      <label>Capacity</label>
      <input type="number" min="1" step="1" v-model="trainingCapacity"/>

      <label>Training Date</label>
      <input type="date" v-model="trainingDate" :min="today"/>

      <label>Start Time</label>
      <input type="time" v-model="startTime" />

      <label>End Time</label>
      <input type="time" v-model="endTime" />

      <button class="book-btn" @click="createTraining">Create Training</button>
    </div>

    <!-- Tables -->
    <div class="tables">
      <!-- Reserved Trainings -->
      <div class="table-container">
        <h2>Reserved Trainings</h2>
        <table>
          <thead>
            <tr>
              <th>Name</th>
              <th>Date</th>
              <th>Time</th>
              <th>Details</th>
              <th>Action</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="res in availableReservations" :key="res.id">
              <td>{{ res.name }}</td>
              <td>{{ res.date }}</td>
              <td>{{ res.startTime }} - {{ res.endTime }}</td>
              <td>
                <button class="info" @click="showDetails = true; currentReservationId = res.id">Expand</button>
              </td>
              <td>
                <button class="danger" @click="cancelTraining(res.id)">Cancel</button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>

      <!-- Completed Trainings -->
      <div class="table-container">
        <h2>Completed or Cancelled Trainings</h2>
        <table>
          <thead>
            <tr>
              <th>Name</th>
              <th>Date</th>
              <th>Time</th>
              <th>Details</th>
              <th>Action</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="res in completedReservations" :key="res.id">
              <td>{{ res.name }}</td>
              <td>{{ res.date }}</td>
              <td>{{ res.startTime }} - {{ res.endTime }}</td>
              <td>
                <button class="info" @click="showDetails = true; currentReservationId = res.id">Expand</button>
              </td>
              <td>
                <button class="info" @click="showReview = true; currentReservationId = res.id;">Review</button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>

    <div v-if="showDetails" class="modal-overlay" @click.self="showDetails = false">
      <div class="modal-content">
        <button class="close-btn" @click="showDetails = false">×</button>
        <h2>Training details</h2>
        <div class="review-window">

          <div class="comment">
            <h5>Description:</h5>
            <label>{{ this.groupReservations.find(r => r.id == this.currentReservationId).about }}</label>
          </div>
          <div class="comment">
            <h5>Capacity:</h5>
            <label>
              {{ this.groupReservations.find(r => r.id == this.currentReservationId).capacity }}
              ({{ this.groupReservations.find(r => r.id == this.currentReservationId).clientIds.length }} taken)
            </label>
          </div>

          <div class="actions">
            <button type="button" @click="showDetails = false">Cancel</button>
          </div>
        </div>
      </div>
    </div>

    <div v-if="showReview" class="modal-overlay" @click.self="showReview = false">
      <div class="modal-content">
        <button class="close-btn" @click="showReview = false">×</button>
        <h2>Submit your review</h2>
        <div class="review-window">
          <!-- Rating (1-10) -->
          <div class="rating">
            <h5>Rating:</h5>
            <label v-for="n in 10" :key="n">
              <input type="radio" :value="n" v-model="trainingRating" />
              {{ n }}
            </label>
          </div>

          <div class="comment">
            <h5>Comment:</h5>
            <textarea v-model="comment" placeholder="Leave a comment..." rows="4"></textarea>
          </div>

          <div class="actions">
            <button type="button" @click="showReview = false">Cancel</button>
            <button :disabled="!trainingRating" class="info"
              @click="submitReview(currentReservationId, this.$route.params.id, trainingRating, comment)">
              Submit Review
            </button>
          </div>
        </div>
      </div>
    </div>

  </div>
</template>

<script>
import dataServices from "../../services/data_services";
import {
  getIndividualReservationsByTrainer,
  getGroupReservationsByTrainer,
  createGroupReservation,
  deleteGroupReservation
} from "../../services/ReservationService";
import FullCalendar from '@fullcalendar/vue3'
import timeGridPlugin from '@fullcalendar/timegrid'
import interactionPlugin from '@fullcalendar/interaction'

export default {
  name: "TrainerGroupTrainings",

  components: { FullCalendar },

  data() {
    return {
      trainer: "",
      groupReservations: [],
      showSchedule: false,
      trainingName: "",
      trainingDescription: "",
      trainingCapacity: 1,
      trainingDate: "",
      startTime: "",
      endTime: "",
      showDetails: false,
      showReview: false,
      trainingRating: null,
      currentReservationId: "",
      calendarOptions: {
        plugins: [ timeGridPlugin, interactionPlugin ],
        initialView: 'timeGridWeek',
        headerToolbar: {
          left: 'prev,next today',
          center: 'title',
          right: ''
        },
        editable: false,
        selectible: false,
        allDaySlot: false,
        firstDay: 1,
        businessHours: {
          daysOfWeek: [1,2,3,4,5], 
          startTime: "08:00",
          endTime: "23:00"
        },
        validRange: {
          start: new Date() // blocks past *days* automatically
        },
        eventConstraint: {
          start: new Date() // blocks past *times* as well
        },
        events: []
      }
    };
  },

  created() {
    this.$parent.$parent.$parent.setUserData(this.$route.params.id, "trainer");
  },

  mounted() {
    dataServices.methods.get_trainer_by_id(this.$route.params.id)
      .then(response => {
        this.trainer = response.data;

        let unavailableSlots = []
        getGroupReservationsByTrainer(this.trainer.id)
          .then(response => {
            this.groupReservations = response.data;
            this.groupReservations.forEach(element => {
              unavailableSlots.push({
                title: "group",
                date: element.date,
                start: element.startTime,
                end: element.endTime
              });
            });

          getIndividualReservationsByTrainer(this.trainer.id)
            .then(response => {
              response.data.forEach(element => {
                if(element.status == 0) {
                  unavailableSlots.push({
                    title: "individual",
                    date: element.date,
                    start: element.startTime,
                    end: element.endTime
                  });
                }
              });

              let unavailableEvents = unavailableSlots.map(t => ({
                title: t.title,
                start: `${t.date}T${t.start}`,
                end: `${t.date}T${t.end}`,
                display: 'background'
              }));

              this.calendarOptions.events = unavailableEvents;
            })
            .catch(error => {
              console.error("Error fetching individual reservations:", error);
            });
        })
        .catch(error => {
          console.error("Error fetching group reservations:", error);
        });
      });
  },

  methods: {
    createTraining() {
      if (!this.trainingName || !this.trainingDescription || !this.trainingDate || !this.startTime || !this.endTime) {
        alert("Please fill in all fields before creating a training.");
        return;
      }

      if (!/^[1-9][0-9]*$/.test(this.trainingCapacity)) {
        alert("Capacity has to be a natural number!");
        return;
      }

      const payload = {
        id: "",
        name: this.trainingName,
        about: this.trainingDescription,
        trainerId: this.trainer.id,
        capacity: this.trainingCapacity,
        clientIds: [],
        date: this.trainingDate,
        startTime: this.startTime + ":00",
        endTime: this.endTime + ":00"
      };

      createGroupReservation(payload)
        .then(response => {
          if (response.status === 201) {
            alert("New group training added!")
            location.reload();
          } else {
            console.warn("Booking failed with status:", response.status);
            alert("Training not created!")
          }
        })
    },

    cancelTraining(id) {
      deleteGroupReservation(id)
        .then(response => {
          alert("Training cancelled!");
          location.reload();
        })
        .catch(error => {
          console.error("Error occurred while cancelling a group reservation:", error);
        });
    },

    submitReview(reservationId, trainerId, rating, comment) {
      let request = {
        reservationId: reservationId,
        trainerId: trainerId,
        trainerComment: comment,
        trainerRating: rating
      }
      dataServices.methods.submit_review_trainer(request)
        .then(response => {
            location.reload();
          })
          .catch(error => {
            console.error("Reviewing error:", error);
            alert("An error occurred while reviewing a reservation from the trainer side.");
          });
    }
  },

  computed: {
    availableReservations() {
      return this.groupReservations.filter(r => {
        let rDate = new Date(`${r.date}T${r.endTime}`);
        let now = new Date();
        return now <= rDate;
      });
    },

    completedReservations() {
      return this.groupReservations.filter(r => {
        let rDate = new Date(`${r.date}T${r.endTime}`);
        let now = new Date();
        return rDate < now;
      });
    }
  }
};
</script>

<style scoped>
.page {
  display: flex;
  gap: 20px;
  padding: 20px;
  background: #f4f4f4;
  min-height: 100vh;
  box-sizing: border-box;
}

/* Booking form */
.booking-form {
  flex: 1;
  background: #fff;
  padding: 20px;
  border-radius: 12px;
  box-shadow: 0 2px 6px rgba(0, 0, 0, 0.1);
  display: flex;
  flex-direction: column;
  gap: 12px;
}

.booking-form h2 {
  margin: 0 0 10px;
  font-size: 20px;
  font-weight: bold;
}

.booking-form label {
  font-size: 14px;
  font-weight: 500;
}

.booking-form select,
.booking-form input {
  padding: 6px 8px;
  border: 1px solid #ccc;
  border-radius: 6px;
  width: 100%;
}

.button-row {
  display: flex;
  gap: 10px;
}

.book-btn {
  background: #28a745;
  color: white;
  align-self: flex-end;
}

/* Tables */
.tables {
  flex: 2;
  display: flex;
  gap: 20px;
}

.table-container {
  flex: 1;
  background: #fff;
  padding: 16px;
  border-radius: 12px;
  box-shadow: 0 2px 6px rgba(0, 0, 0, 0.1);
}

.table-container h2 {
  margin-bottom: 10px;
  font-size: 18px;
  font-weight: bold;
}

table {
  width: 100%;
  border-collapse: collapse;
  font-size: 14px;
}

th,
td {
  padding: 8px;
  border-bottom: 1px solid #ddd;
}

th {
  background: #f9f9f9;
  text-align: left;
}

button {
  padding: 8px 12px;
  border: none;
  border-radius: 6px;
  cursor: pointer;
}

button.danger {
  background: #dc3545;
  color: white;
}

button.info {
  background: #007bff;
  color: white;
}

/* Basic modal styles */
.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0,0,0,0.4);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 999;
}

.modal-content {
  background: #fff;
  padding: 10px;
  width: 90%;
  max-width: 900px;
  max-height: 90vh;
  overflow-y: auto;
  border-radius: 8px;
  position: relative;
}

.close-btn {
  position: absolute;
  top: 4px;
  right: 8px;
  font-size: 1.5rem;
  border: none;
  background: transparent;
  cursor: pointer;
}

button:disabled {
  background-color: #cccccc; /* Gray background */
  color: #666666; /* Darker gray text */
  cursor: not-allowed; /* Change cursor to indicate no interaction */
  opacity: 0.7; /* Optional: reduce opacity for a more subdued look */
  cursor: not-allowed;
}

.fc-non-business {
  background-color: #e0e0e0 !important;
  opacity: 0.6;
}

.rating label {
  margin-right: 0.5rem;
}

.comment textarea {
  width: 100%;
  margin-top: 0.5rem;
}

.review-window {
  margin: 10px;
}

.actions {
  display: flex;
  justify-content: flex-end;
  gap: 0.5rem;
  margin-top: 1rem;
}
</style>

