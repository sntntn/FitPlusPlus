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
          <td class="py-2 px-4">{{ training.date }}</td>
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
export default {
  name: "GroupTrainingsPage",

  data() {
    return {
      trainings: []
    };
  },

  created() {
    this.$parent.$parent.$parent.setUserData(this.$route.params.id, "client");
    console.log("Component created");
    this.trainings = [
      { id: 1, name: "Cardio", trainer: "Marko Marković", date: "2025-10-02 18:00", booked: false },
      { id: 2, name: "Strength", trainer: "Jovan Jovanović", date: "2025-10-03 19:00", booked: true },
      { id: 3, name: "Yoga", trainer: "Ana Anić", date: "2025-10-04 17:00", booked: false },
    ];
  },

  mounted() {
    console.log("Component mounted.");
  },

  methods: {
    bookTraining(id) {
      const training = this.trainings.find(t => t.id === id);
      if (training) training.booked = true;
      // later API call for reservation
    },

    cancelBooking(id) {
      const training = this.trainings.find(t => t.id === id);
      if (training) training.booked = false;
      // later API for canceling
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
