<template>
  <div class="page">
    <!-- Booking Form -->
    <div class="booking-form">
      <h2>Book a Training</h2>

      <label>Choose Trainer</label>
      <select v-model="selectedTrainer">
        <option disabled value="">-- select trainer --</option>
        <option v-for="trainer in this.trainers" :key="trainer.id" :value="trainer">
          {{ trainer.fullName }}
        </option>
      </select>

      <div class="button-row">
        <button :disabled="!selectedTrainer" @click="getAvailability = true">Get Availability</button>
        <button :disabled="!selectedTrainer" @click="showTrainerInfo = true">Trainer Info</button>
      </div>

      <div v-if="getAvailability" class="modal-overlay" @click.self="getAvailability = false">
        <div class="modal-content">
          <button class="close-btn" @click="getAvailability = false">×</button>
          <FullCalendar :options="calendarOptions"/>
        </div>
      </div>

      <div v-if="showTrainerInfo" class="modal-overlay" @click.self="showTrainerInfo = false">
        <div class="modal-content">
          <button class="close-btn" @click="showTrainerInfo = false">×</button>
          <h2>{{ selectedTrainer.fullName }}</h2>
          <p><strong>Bio:</strong> {{ selectedTrainer.bio }}</p>
          <p><strong>Email:</strong> <a :href="`mailto:${selectedTrainer.contactEmail}`">{{ selectedTrainer.contactEmail }}</a></p>
          <p><strong>Contact phone:</strong> {{ selectedTrainer.contactPhone }}</p>
          <p><strong>Average rating:</strong> {{ selectedTrainer.averageRating }} / 10</p>
          <p><strong>Trainings offered:</strong> {{ selectedTrainer.trainingTypes.map(t => t.name).join(', ') }}</p>
        </div>
      </div>

      <label>Training Date</label>
      <input type="date" v-model="trainingDate" :min="today"/>

      <label>Training Type</label>
      <select v-model="selectedTrainingType">
        <option disabled value="">-- select type --</option>
        <option v-for="type in availableTrainingTypes" :key="type" :value="type">
          {{ `${type.name} (${type.price} USD)` }}
        </option>
      </select>

      <label>Start Time</label>
      <input type="time" v-model="startTime" />

      <label>End Time</label>
      <label>{{ endTime }}</label>

      <button class="book-btn" @click="bookTraining">Book Training</button>
    </div>

    <!-- Tables -->
    <div class="tables">
      <!-- Reserved Trainings -->
      <div class="table-container">
        <h2>Reserved Trainings</h2>
        <table>
          <thead>
            <tr>
              <th>Trainer</th>
              <th>Training Type</th>
              <th>Date</th>
              <th>Time</th>
              <th>Action</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="res in reservedTrainings" :key="res.id">
              <td>{{ this.findTrainerName(res.trainerId) }}</td>
              <td>{{ res.trainingType }}</td>
              <td>{{ res.date }}</td>
              <td>{{ res.time }}</td>
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
              <th>Trainer</th>
              <th>Training Type</th>
              <th>Date</th>
              <th>Time</th>
              <th>Status</th>
              <th>Action</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="res in completedOrCancelled" :key="res.id">
              <td>{{ this.findTrainerName(res.trainerId) }}</td>
              <td>{{ res.trainingType }}</td>
              <td>{{ res.date }}</td>
              <td>{{ res.time }}</td>
              <td>{{ res.status }}</td>
              <td>
                <button v-if="res.status == 'Active'" class="info" @click="showReview = true; currentReservationId = res.id; currentTrainerId = res.trainerId">
                  Review
                </button>
                <button v-else :disabled="true" class="info">Review</button>
              </td>
            </tr>
          </tbody>
        </table>
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
              @click="submitReview(currentReservationId, currentTrainerId, this.$route.params.id, trainingRating, comment)">
              Submit Review
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import dataServices from '@/services/data_services';
import {
  getIndividualReservationsByClient,
  cancelClientIndividualReservation,
  getGroupReservationsByTrainer,
  getIndividualReservationsByTrainer,
} from '@/services/ReservationService'
import FullCalendar from '@fullcalendar/vue3'
import timeGridPlugin from '@fullcalendar/timegrid'
import interactionPlugin from '@fullcalendar/interaction'

export default {
  components: { FullCalendar },

  data() {
    return {
      trainers: [],
      reservations: [],
      selectedTrainer: "",
      getAvailability: false,
      showTrainerInfo: false,
      showReview: false,
      selectedTrainingType: "",
      trainingDate: "",
      trainingRating: null,
      currentReservationId: "",
      currentTrainerId: "",
      today: new Date().toISOString().split("T")[0], // e.g. "2025-09-30"
      startTime: "",
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

  methods: {
    initiatePayment(bookData, price) {
      return dataServices.methods
        .get_trainer_by_id(bookData.trainerId)
        .then((trainerResponse) => {
          const request = {
            id: "",
            userId: bookData.clientId,
            amount: price,
            currency: "USD",
            trainerPayPalEmail: trainerResponse.data.contactEmail,
          };

          return dataServices.methods.create_payment(request);
        })
        .then((response) => {
          const paymentId = response.data.payment.id;
          const approvalUrl = response.data.paymentLink;

          console.log("Payment initiated with ID:", paymentId);
          window.location.href = approvalUrl;
        })
        .catch((error) => {
          console.error("Error initiating payment:", error);
          alert("Failed to initiate payment.");
          return false;
        });
    },

    bookTraining() {
      if (
        !this.selectedTrainer ||
        !this.trainingDate ||
        !this.selectedTrainingType ||
        !this.startTime
      ) {
        alert("Please fill in all fields before booking.");
        return;
      }

      // TODO: Check if the trainer is free (maybe client has overseen the trainer availabilty)

      const bookData = {
        id: "",
        trainerId: this.selectedTrainer.id,
        clientId: this.$route.params.id,
        trainingTypeId: this.selectedTrainingType.id,
        date: this.trainingDate,
        startTime: this.startTime + ":00",
        endTime: this.endTime,
      };

      const price = this.selectedTrainingType.price;

      sessionStorage.setItem("bookData", JSON.stringify(bookData));

      this.initiatePayment(bookData, price)
        .then(() => {
          console.log("Payment process initiated.");
        })
        .catch((error) => {
          console.error("Payment initiation error:", error);
          alert("Could not start payment process. Please try again.");
        });
    },

    cancelTraining(res_id) {
      if (confirm("Are you sure you want to cancel the training?")) {
        cancelClientIndividualReservation(res_id)
          .then(response => {
            location.reload();
          })
          .catch(error => {
            console.error("Cancelling error:", error);
            alert("An error occurred while cancelling a reservation from the client side.");
          });
      }
    },

    submitReview(reservationId, trainerId, clientId, rating, comment) {
      let request = {
        reservationId: reservationId,
        clientId: this.$route.params.id,
        clientComment: comment,
        clientRating: rating,
      }
      dataServices.methods.submit_review_client(request)
        .then(response => {
            location.reload();
          })
          .catch(error => {
            console.error("Reviewing error:", error);
            alert("An error occurred while reviewing a reservation from the client side.");
          });
    },

    findTrainerName(tra_id) {
      let trainer = this.trainers.find(t => t.id == tra_id);
      return trainer !== undefined ? trainer.fullName : "";
    },

    findTrainingType(type_id) {
      let trainingTypes = this.trainers.map(t => t.trainingTypes).flat();
      let trainingType = trainingTypes.find(t => t.id == type_id);
      return trainingType !== undefined ? trainingType.name : "";
    },

    // Fetch list of trainers
    fetchTrainers() {
      dataServices.methods.get_trainers()
        .then(response => {
          this.trainers = response.data;
        })
        .catch(error => {
          console.error("Failed to fetch trainers:", error);
        });
    },

    fetchReservations() {
      let clientId = this.$route.params.id;
      getIndividualReservationsByClient(clientId)
        .then(response => {
          this.reservations = response.data;
        })
        .catch(error => {
          console.error("Failed to fetch reservations:", error);
        })
    }
  },

  computed: {
    availableTrainingTypes() {
      return this.selectedTrainer ? this.selectedTrainer.trainingTypes : [];
    },

    endTime() {
      let start = this.startTime;
      let duration = this.selectedTrainingType.duration; 

      if (!start || !duration) return null;

      // Split the duration string
      const [h1, m1] = start.split(':').map(Number)
      const [h2, m2, s2] = duration.split(':').map(Number);

      let numMinutes = (60 * (h1 + h2) + m1 + m2) % (24 * 60);
      const hours = Math.floor(numMinutes / 60).toString().padStart(2, '0');
      const minutes = (numMinutes % 60).toString().padStart(2, '0');
      const seconds = s2.toString().padStart(2, '0');
      return `${hours}:${minutes}:${seconds}`;
    },

    reservedTrainings() {
      return this.reservations
        .filter(r => {
          let rDate = new Date(`${r.date}T${r.endTime}`);
          let now = new Date();
          return now <= rDate && r.status == 0;
        })
        .map(r => ({
          id: r.id,
          trainerId: r.trainerId,
          trainingType: this.findTrainingType(r.trainingTypeId),
          date: r.date,
          time: `${r.startTime} - ${r.endTime}`
        }));
    },

    completedOrCancelled() {
      return this.reservations
        .filter(r => {
          let rDate = new Date(`${r.date}T${r.endTime}`);
          let now = new Date();
          return rDate < now || r.status != 0;
        })
        .map(r => ({
          id: r.id,
          trainerId: r.trainerId,
          trainingType: this.findTrainingType(r.trainingTypeId),
          date: r.date,
          time: `${r.startTime} - ${r.endTime}`,
          status: r.status == 0 ? "Active" : (r.status == 1 ? "Cancelled by me" : "Cancelled by trainer")
        }));
    }
  },

  watch: {
    selectedTrainer() {
      let unavailableSlots = []

      getIndividualReservationsByTrainer(this.selectedTrainer.id)
        .then(response => {
          response.data.forEach(element => {
            console.log(element);
            if(element.status == 0) {
              unavailableSlots.push({
                title: "individual",
                date: element.date,
                start: element.startTime,
                end: element.endTime
              });
            }
          });

          getGroupReservationsByTrainer(this.selectedTrainer.id)
            .then(response => {
              response.data.forEach(element => {
                unavailableSlots.push({
                  title: "group",
                  date: element.date,
                  start: element.startTime,
                  end: element.endTime
                });
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
              console.error("Error fetching group reservations:", error);
            });
        })
        .catch(error => {
          console.error("Error fetching individual reservations:", error);
        });
    }
  },

  mounted() {
    this.fetchTrainers();
    this.fetchReservations();
    this.$parent.$parent.$parent.setUserData(this.$route.params.id, "client");
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
