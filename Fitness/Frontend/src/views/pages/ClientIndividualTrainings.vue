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
              <th>Date/Time</th>
              <th>Action</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="res in reservedTrainings" :key="res.id">
              <td>{{ res.trainer }}</td>
              <td>{{ res.type }}</td>
              <td>{{ res.date }} {{ res.time }}</td>
              <td>
                <button class="danger" @click="cancelTraining(res.id)">Cancel</button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>

      <!-- Completed Trainings -->
      <div class="table-container">
        <h2>Completed Trainings</h2>
        <table>
          <thead>
            <tr>
              <th>Trainer</th>
              <th>Training Type</th>
              <th>Date/Time</th>
              <th>Review</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="done in completedTrainings" :key="done.id">
              <td>{{ done.trainer }}</td>
              <td>{{ done.type }}</td>
              <td>{{ done.date }} {{ done.time }}</td>
              <td>
                <button class="info" @click="reviewTraining(done.id)">Review</button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</template>

<script>
import dataServices from '@/services/data_services';
import {
  getIndividualReservationsByClient,
  getGroupReservationsByTrainer,
  getIndividualReservationsByTrainer,
  createIndividualReservation
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
      selectedTrainingType: "",
      trainingDate: "",
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
    bookTraining() {
      if (!this.selectedTrainer || !this.trainingDate || !this.selectedTrainingType || !this.startTime) {
        alert("Please fill in all fields before booking.");
        return;
      }

      const payload = {
        id: "",
        trainerId: this.selectedTrainer.id,
        clientId: this.$route.params.id,
        trainingTypeId: this.selectedTrainingType.id,
        date: this.trainingDate,
        startTime: this.startTime + ":00",
        endTime: this.endTime
      };

      console.log("Booking training with payload:", payload);

      createIndividualReservation(payload)
        .then(response => {
          if (response.status === 201) {
            alert("Training booked successfully!");
          } else {
            alert(`Booking failed. Status: ${response.status}`);
          }
        })
        .catch(error => {
          console.error("Booking error:", error);
          alert("An error occurred while booking the training.");
        });
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
          console.log(this.reservations);
        })
        .catch(error => {
          console.error("Failed to fetch trainers:", error);
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
    }
  },

  watch: {
    selectedTrainer() {
      let unavailableSlots = []

      getIndividualReservationsByTrainer(this.selectedTrainer.id)
        .then(list => {
          list.data.forEach(element => {
            unavailableSlots.push({
              date: element.date,
              start: element.startTime,
              end: element.endTime
            });
          });

          getGroupReservationsByTrainer(this.selectedTrainer.id)
            .then(list => {
              list.data.forEach(element => {
                unavailableSlots.push({
                  date: element.date,
                  start: element.startTime,
                  end: element.endTime
                });
              });

              let unavailableEvents = unavailableSlots.map(t => ({
                title: 'booked',
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

.booking-form button {
  padding: 8px 12px;
  border: none;
  border-radius: 6px;
  cursor: pointer;
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

.fc-non-business {
  background-color: #e0e0e0 !important;
  opacity: 0.6;
}
</style>
