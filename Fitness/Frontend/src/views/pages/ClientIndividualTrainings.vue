<template>
  <div class="p-6 max-w-lg mx-auto">
    <h1 class="text-2xl font-bold mb-6">Book Individual Training</h1>

    <!-- Choose trainer -->
    <div class="mb-4">
      <label for="trainer" class="block mb-2 font-semibold">Choose trainer:</label>
      <select
        v-model="selectedTrainer"
        id="trainer"
        class="w-full border rounded p-2"
      >
        <option disabled value="">-- Select trainer --</option>
        <option v-for="trainer in trainers" :key="trainer.id" :value="trainer.id">
          {{ trainer.name }}
        </option>
      </select>
    </div>

    <!-- Check availability -->
    <div class="mb-6">
      <button
        class="check-btn"
        @click="checkAvailability"
        :disabled="!selectedTrainer"
      >
        Check trainer availability
      </button>
      <div v-if="unavailableSlots.length > 0" class="mt-3 text-sm text-red-600">
        <p>Trainer is unavailable at:</p>
        <ul class="list-disc ml-5">
          <li v-for="(slot, index) in unavailableSlots" :key="index">
            {{ slot.date }} ({{ slot.start }} - {{ slot.end }})
          </li>
        </ul>
      </div>
    </div>

    <br>
    <!-- Date & time inputs -->
    <div class="mb-4">
      <label for="date" class="block mb-2 font-semibold">Date:</label>
      <input
        type="date"
        id="date"
        v-model="date"
        class="w-full border rounded p-2"
      />
    </div>

    <div class="mb-4">
      <label for="start" class="block mb-2 font-semibold">Start time:</label>
      <input
        type="time"
        id="start"
        v-model="startTime"
        class="w-full border rounded p-2"
      />
    </div>

    <div class="mb-6">
      <label for="end" class="block mb-2 font-semibold">End time:</label>
      <input
        type="time"
        id="end"
        v-model="endTime"
        class="w-full border rounded p-2"
      />
    </div>

    <br>
    <!-- Book training -->
    <button class="book-btn w-full py-3 text-lg" @click="bookTraining">
      Book Training
    </button>
  </div>
</template>

<script>
export default {
  name: "IndividualTrainingPage",

  data() {
    return {
      trainers: [
        { id: 1, name: "Marko Marković" },
        { id: 2, name: "Ana Anić" },
        { id: 3, name: "Jovan Jovanović" }
      ],
      selectedTrainer: "",
      unavailableSlots: [],
      date: "",
      startTime: "",
      endTime: ""
    };
  },

  methods: {
    checkAvailability() {
      console.log("Checking availability for trainer:", this.selectedTrainer);
      // MOCK: vraćamo zauzete termine
      this.unavailableSlots = [
        { date: "2025-10-03", start: "18:00", end: "19:00" },
        { date: "2025-10-04", start: "16:00", end: "17:00" }
      ];
      // ovde ide API call -> ReservationService.getUnavailableSlots(trainerId)
    },

    bookTraining() {
      if (!this.selectedTrainer || !this.date || !this.startTime || !this.endTime) {
        alert("Please fill in all fields before booking.");
        return;
      }

      const payload = {
        trainerId: this.selectedTrainer,
        clientId: this.$route.params.id, // pretpostavka kao kod group page-a
        date: this.date,
        start: this.startTime,
        end: this.endTime
      };

      console.log("Booking training with payload:", payload);
      // ovde ide API call -> ReservationService.bookIndividualTraining(payload)
      alert("Training booked successfully!");
    }
  }
};
</script>

<style scoped>
.check-btn {
  background-color: #3b82f6; /* plavo */
  color: white;
  padding: 0.5rem 1rem;
  border-radius: 8px;
  border: none;
  cursor: pointer;
  font-weight: 600;
}
.check-btn:hover {
  background-color: #2563eb;
}

.book-btn {
  background-color: #22c55e; /* zeleno */
  color: white;
  border: none;
  border-radius: 10px;
  cursor: pointer;
  font-weight: 700;
  font-size: 1.2rem;
}
.book-btn:hover {
  background-color: #16a34a;
}
</style>
