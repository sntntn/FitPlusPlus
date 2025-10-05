<template>
  <div class="page">
    <!-- Tables -->
    <div class="tables">
      <!-- Reserved Trainings -->
      <div class="table-container">
        <h2>Reserved Trainings</h2>
        <table>
          <thead>
            <tr>
              <th>Client</th>
              <th>Training Type</th>
              <th>Date</th>
              <th>Time</th>
              <th>Action</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="res in reservedTrainings" :key="res.id">
              <td>{{ res.client }}</td>
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
              <th>Training Type</th>
              <th>Date</th>
              <th>Time</th>
              <th>Status</th>
              <th>Action</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="res in completedOrCancelled" :key="res.id">
              <td>{{ res.client }}</td>
              <td>{{ res.date }}</td>
              <td>{{ res.time }}</td>
              <td>{{ res.status }}</td>
              <td>
                <button v-if="res.status == 'Active'" class="info" @click="showReview = true; currentReservationId = res.id">
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
            <button :disabled="!trainingRating" class="info" @click="submitReview(currentReservationId, trainingRating, comment)">
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
  cancelTrainerIndividualReservation,
  getIndividualReservationsByTrainer
} from '@/services/ReservationService'

export default {
  data() {
    return {
      trainers: [],
      clients: [],
      reservations: [],
      showReview: false,
      trainingRating: null,
      currentReservationId: ""
    };
  },

  methods: {
    cancelTraining(res_id) {
      if (confirm("Are you sure you want to cancel the training?")) {
        cancelTrainerIndividualReservation(res_id)
          .then(response => {
            location.reload();
          })
          .catch(error => {
            console.error("Cancelling error:", error);
            alert("An error occurred while cancelling a reservation from the trainer side.");
          });
      }
    },

    submitReview(reservationId, rating, comment) {
      // TODO: Connect review submission with the backend
    },

    findClientName(cli_id) {
      let client = this.clients.find(t => t.id == cli_id);
      return client !== undefined ? client.name + " " + client.surname : "";
    },

    findTrainingType(type_id) {
      let trainingTypes = this.trainers.map(t => t.trainingTypes).flat();
      let trainingType = trainingTypes.find(t => t.id == type_id);
      return trainingType !== undefined ? trainingType.name : "";
    },

    fetchTrainers() {
      dataServices.methods.get_trainers()
        .then(response => {
          this.trainers = response.data;
        })
        .catch(error => {
          console.error("Failed to fetch trainers:", error);
        });
    },

    fetchClients() {
      dataServices.methods.get_clients()
        .then(response => {
          this.clients = response.data;
        })
        .catch(error => {
          console.error("Failed to fetch clients:", error);
        });
    },

    fetchReservations() {
      let trainerId = this.$route.params.id;
      getIndividualReservationsByTrainer(trainerId)
        .then(response => {
          this.reservations = response.data;
        })
        .catch(error => {
          console.error("Failed to fetch reservations:", error);
        })
    }
  },

  computed: {
    reservedTrainings() {
      return this.reservations
        .filter(r => {
          let rDate = new Date(`${r.date}T${r.endTime}`);
          let now = new Date();
          return now <= rDate && r.status == 0;
        })
        .map(r => ({
          id: r.id,
          client: this.findClientName(r.clientId),
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
          client: this.findClientName(r.clientId),
          trainingType: this.findTrainingType(r.trainingTypeId),
          date: r.date,
          time: `${r.startTime} - ${r.endTime}`,
          status: r.status == 0 ? "Active" : (r.status == 1 ? "Cancelled by client" : "Cancelled by me")
        }));
    }
  },

  created() {
    this.$parent.$parent.$parent.setUserData(this.$route.params.id, "trainer");
  },

  mounted() {
    this.fetchTrainers();
    this.fetchClients();
    this.fetchReservations();
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
