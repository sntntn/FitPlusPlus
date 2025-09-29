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
          {{ trainer.fullName }}
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
import dataServices from '@/services/data_services';
import {
  getGroupReservationsByTrainer,
  getIndividualReservationsByTrainer,
  createIndividualReservation
} from '@/services/ReservationService'

export default {
  data() {
    return {
      trainers: [],
      selectedTrainer: "",
      unavailableSlots: [],
      date: "",
      startTime: "",
      endTime: ""
    };
  },

  methods: {
    checkAvailability() {
      this.unavailableSlots = [];

      getIndividualReservationsByTrainer(this.selectedTrainer)
        .then(list => {
          list.data.forEach(element => {
            this.unavailableSlots.push({
              date: element.date,
              start: element.startTime,
              end: element.endTime
            });
          });
        })
        .catch(error => {
          console.error("Error fetching individual reservations:", error);
        });

      getGroupReservationsByTrainer(this.selectedTrainer)
        .then(list => {
          list.data.forEach(element => {
            this.unavailableSlots.push({
              date: element.date,
              start: element.startTime,
              end: element.endTime
            });
          });
        })
        .catch(error => {
          console.error("Error fetching group reservations:", error);
        });
    },

    bookTraining() {
      if (!this.selectedTrainer || !this.date || !this.startTime || !this.endTime) {
        alert("Please fill in all fields before booking.");
        return;
      }

      const payload = {
        id: "",
        trainerId: this.selectedTrainer,
        clientId: this.$route.params.id,
        trainingTypeId: "",
        date: this.date,
        startTime: this.startTime + ":00",
        endTime: this.endTime + ":00"
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
    }
  },

  mounted() {
    this.fetchTrainers();
    this.$parent.$parent.$parent.setUserData(this.$route.params.id, "client");
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
