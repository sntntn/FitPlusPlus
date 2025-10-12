<template>
  <div class="page">
    <div class="tables">
      <!-- Reserved Trainings -->
      <div class="table-container">
        <h2>Available Group Trainings</h2>
        <table>
          <thead>
            <tr>
              <th>Name</th>
              <th>Trainer</th>
              <th>Date</th>
              <th>Time</th>
              <th>Details</th>
              <th>Action</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="res in availableReservations" :key="res.id">
              <td>{{ res.name }}</td>
              <td>{{ res.trainer }}</td>
              <td>{{ res.date }}</td>
              <td>{{ res.startTime }} - {{ res.endTime }}</td>
              <td>
                <button class="info" @click="showDetails = true; currentReservationId = res.id">Expand</button>
              </td>
              <td>
                <button v-if="!res.clientIds.includes(this.$route.params.id)" class="info" @click="bookReservation(res.id)">Book</button>
                <button v-else class="danger" @click="cancelReservation(res.id)">Cancel</button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>

      <!-- Completed Trainings -->
      <div class="table-container">
        <h2>Completed Group Trainings</h2>
        <table>
          <thead>
            <tr>
              <th>Name</th>
              <th>Trainer</th>
              <th>Date</th>
              <th>Time</th>
              <th>Description</th>
              <th>Action</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="res in completedReservations" :key="res.id">
              <td>{{ res.name }}</td>
              <td>{{ res.trainer }}</td>
              <td>{{ res.date }}</td>
              <td>{{ res.startTime }} - {{ res.endTime }}</td>
              <td>
                <button class="info" @click="showDetails = true; currentReservationId = res.id">Expand</button>
              </td>
              <td>
                <button class="info" @click="showReview = true; currentReservationId = res.id; currentTrainerId = res.trainerId;">Review</button>
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
import data_services from '@/services/data_services';
import {
  getAllGroupReservations,
  bookGroupReservation,
  cancelGroupReservation
} from '@/services/ReservationService'

export default {
  name: "GroupTrainingsPage",

  data() {
    return {
      groupReservations: [],
      trainers: [],
      showDetails: false,
      showReview: false,
      trainingRating: null,
      currentReservationId: "",
      currentTrainerId: ""
    };
  },

  created() {
    this.$parent.$parent.$parent.setUserData(this.$route.params.id, "client");
    console.log("Component created");
  },

  mounted() {
    this.fetchGroupReservations();
  },

  methods: {
    bookReservation(id) {
      if (confirm("Confirm the booking")) {
        bookGroupReservation(id, this.$route.params.id)
          .then(response => {
            if (response.status === 200) {
              location.reload();
            } else {
              console.warn("Booking failed with status:", response.status);
              alert("Training not booked!")
            }
          })
          .catch(error => {
            console.error("Booking API call failed:", error);
            alert("Communication failure!")
          });
      }
    },

    cancelReservation(id) {
      if (confirm("Are you sure you want to cancel this reservation?")) {
        cancelGroupReservation(id, this.$route.params.id)
          .then(response => {
            if (response.status === 204) {
              location.reload();
            } else {
              console.warn("Cancelling failed with status:", response.status);
              alert("Training not cancelled!")
            }
          })
          .catch(error => {
            console.error("Booking API call failed:", error);
            alert("Communication failure!")
          });
      }
    },

    submitReview(reservationId, trainerId, clientId, rating, comment) {
      console.log("Client Group Trainings SUBMIT REVIEW:", trainerId, clientId);
      // let request = {
      //   reservationId: reservationId,
      //   clientId: clientId,
      //   clientComment: comment,
      //   clientRating: rating,
      // }
      // data_services.methods.submit_review_client(request)
      //   .then(response => {
      //       location.reload();
      //     })
      //     .catch(error => {
      //       console.error("Reviewing error:", error);
      //       alert("An error occurred while reviewing a reservation from the trainer side.");
      //     });
    },

    fetchGroupReservations() {
      getAllGroupReservations()
        .then(response => {
          this.groupReservations = response.data;
          console.log(this.groupReservations);
          return data_services.methods.get_trainers();
        })
        .then(response => {
          this.trainers = response.data;
          this.groupReservations.forEach(reservation => {
            const trainer = this.trainers.find(t => t.id === reservation.trainerId);
            reservation.trainer = trainer ? trainer.fullName : "Unknown";
          });
        })
        .catch(error => {
          console.error("Error fetching trainings or trainers:", error);
        });
    }
  },


  computed: {
    availableReservations() {
      return this.groupReservations.filter(r => {
        let rDate = new Date(`${r.date}T${r.endTime}`);
        let now = new Date();
        return now <= rDate && r.capacity > r.clientIds.length;
      });
    },

    completedReservations() {
      return this.groupReservations.filter(r => {
        let rDate = new Date(`${r.date}T${r.endTime}`);
        let now = new Date();
        return rDate < now && r.clientIds.includes(this.$route.params.id);
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
