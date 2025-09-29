<template>
  <div class="p-6">
    <h1 class="text-2xl font-bold mb-4">Group Trainings</h1>

    <table class="min-w-full bg-white shadow-md rounded-lg overflow-hidden">
      <thead class="bg-gray-200">
        <tr>
          <th class="py-3 px-4 text-left">Training</th>
          <th class="py-3 px-4 text-left">Trainer</th>
          <th class="py-3 px-4 text-left">Date/Time</th>
          <th class="py-3 px-4 text-center">Action</th>
        </tr>
      </thead>
      <tbody>
        <tr
          v-for="training in trainings"
          :key="training.id"
          class="border-b"
        >
          <td class="py-2 px-4">{{ training.name }}</td>
          <td class="py-2 px-4">{{ training.trainer }}</td>
          <td class="py-2 px-4">{{ training.date }}
            <br>
            {{  training.startTime }} - {{ training.endTime }}
          </td>
          <td class="py-2 px-4 text-center">
            <button
              v-if="!training.booked"
              class="book-btn"
              @click="bookTraining(training.id)"
            >
              Book
            </button>
            <button
              v-else
              class="cancel-btn"
              @click="cancelBooking(training.id)"
            >
              Cancel
            </button>
          </td>
        </tr>
      </tbody>
    </table>
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
      trainings: [],
      trainers: []
    };
  },

  created() {
    this.$parent.$parent.$parent.setUserData(this.$route.params.id, "client");
    console.log("Component created");
  },

  mounted() {
    this.fetchTrainings()
  },

  methods: {
    bookTraining(id) {
      const training = this.trainings.find(t => t.id === id);

      bookGroupReservation(id, this.$route.params.id)
        .then(response => {
          if (response.status === 200) {
            training.booked = true;
          } else {
            console.warn("Booking failed with status:", response.status);
            alert("Training not booked!")
          }
        })
        .catch(error => {
          console.error("Booking API call failed:", error);
          alert("Communication failure!")
        });
    },

    cancelBooking(id) {
      const training = this.trainings.find(t => t.id === id);

      cancelGroupReservation(id, this.$route.params.id)
        .then(response => {
          if (response.status === 204) {
            training.booked = false;
          } else {
            console.warn("Cancelling failed with status:", response.status);
            alert("Training not cancelled!")
          }
        })
        .catch(error => {
          console.error("Booking API call failed:", error);
          alert("Communication failure!")
        });
    },

    fetchTrainings() {
      getAllGroupReservations()
        .then(response => {
          this.trainings = response.data;
          return data_services.methods.get_trainers();
        })
        .then(response => {
          this.trainers = response.data;
          this.trainings.forEach(training => {
            const trainer = this.trainers.find(t => t.id === training.trainerId);
            training.trainer = trainer ? trainer.fullName : "Unknown";
            training.booked = training.clientIds.includes(this.$route.params.id)
          });
        })
        .catch(error => {
          console.error("Error fetching trainings or trainers:", error);
        });
    }
  }
};
</script>

<style scoped>
.book-btn {
  background-color: #22c55e; /* zeleno */
  color: white;
  padding: 0.5rem 1rem;
  border-radius: 8px;
  border: none;
  cursor: pointer;
  font-weight: 600;
}
.book-btn:hover {
  background-color: #378955;
}

.cancel-btn {
  background-color: #ef4444; /* crveno */
  color: white;
  padding: 0.5rem 1rem;
  border-radius: 8px;
  border: none;
  cursor: pointer;
  font-weight: 600;
}
.cancel-btn:hover {
  background-color: #dc2626;
}
</style>
