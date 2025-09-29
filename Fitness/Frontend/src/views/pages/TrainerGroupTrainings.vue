<template>
  <div class="p-6">
    <h1 class="text-2xl font-bold mb-4">Create Group Training</h1>

    <div class="space-y-4 mb-6">
      <label for="trainingType" class="block font-medium">Training Type</label>
      <select
        v-model="newTraining.typeId"
        id="trainingType"
        class="w-full border rounded px-3 py-2"
      >
        <option value="" disabled>Select training type</option>
        <option
          v-for="type in trainer.trainingTypes"
          :key="type.id"
          :value="type.id"
        >
          {{ type.name }} ({{ type.duration }}, {{ type.difficulty }})
        </option>
      </select>

      <label for="dateTime" class="block font-medium">Date & Time</label>
      <input
        v-model="newTraining.dateTime"
        id="dateTime"
        type="datetime-local"
        class="w-full border rounded px-3 py-2"
      />

      <button
        @click="createTraining"
        style="background-color: #28a745; color: white; padding: 8px 16px; border-radius: 6px; font-weight: 600; cursor: pointer;"
        onmouseover="this.style.backgroundColor='#218838'"
        onmouseout="this.style.backgroundColor='#28a745'"
      >
        ➕ Add Training
      </button>
    </div>

    <h2 class="text-xl font-bold mb-4">My Group Trainings</h2>
    <table class="min-w-full bg-white shadow-md rounded-lg overflow-hidden">
      <thead class="bg-gray-200">
        <tr>
          <th class="py-3 px-4 text-left">Training</th>
          <th class="py-3 px-4 text-left">Date/Time</th>
        </tr>
      </thead>
      <tbody>
        <tr
          v-for="training in trainings"
          :key="training.id"
          class="border-b"
        >
          <td class="py-2 px-4">
            {{ getTrainingTypeName(training.trainingTypeId) }}
          </td>
          <td class="py-2 px-4">{{ training.date }}
            <br>
            {{ training.startTime }} - {{ training.endTime }}
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script>
import dataServices from "../../services/data_services";
import * as reservationService from "../../services/ReservationService";
// import {
//     getAllGroupReservations
// } from "../../services/ReservationService";

export default {
  name: "TrainerGroupTrainings",
  data() {
    return {
      trainer: { trainingTypes: [] },
      trainings: [],
      newTraining: {
        typeId: "",
        dateTime: "",
      },
    };
  },
  created() {
    this.$parent.$parent.$parent.setUserData(this.$route.params.id, "trainer");
    this.trainerId = this.$route.params.id;
    console.log("id trenera: " + this.trainerId);
  },
  mounted() {
    dataServices.methods.get_trainer_by_id(this.trainerId).then((response) => {
      this.trainer = response.data;
      console.log(response.data);
      console.log(this.trainer);
    });

    reservationService.getGroupReservationsByTrainer(this.trainerId).then((response) => {
      this.trainings = response.data
    })
  },
  methods: {
    createTraining() {
      if (!this.newTraining.typeId || !this.newTraining.dateTime) {
        alert("Please select a training type and date/time.");
        return;
      }
      
      const [date, time] = this.newTraining.dateTime.split("T");
      const timePlusOneHour = new Date(this.newTraining.dateTime)
        .toTimeString()
        .split(":")
        .map((v, i) => i < 2 ? String((+v + (i === 0 ? 1 : 0)) % 24).padStart(2, "0") : "00")
        .join(":");

      const newItem = {
        id: "",
        name: "Name",
        about: "About",
        trainerId: this.trainerId,
        trainingTypeId: this.newTraining.typeId,
        capacity: 20,
        clientIds: [],
        date: date,
        startTime: time + ":00",
        endTime: timePlusOneHour,
        dateTime: this.newTraining.dateTime,
      };

      // reset form
      this.newTraining = { typeId: "", dateTime: "" };

      reservationService.createGroupReservation(newItem)
        .then(response => {
          if (response.status === 201) {
            alert("New group training added!")
          } else {
            console.warn("Booking failed with status:", response.status);
            alert("Training not created!")
          }
        })
    },

    getTrainingTypeName(typeId) {
      const type = this.trainer.trainingTypes.find((t) => t.id == typeId);
      return type ? type.name : "Unknown";
    },
  },
};
</script>
