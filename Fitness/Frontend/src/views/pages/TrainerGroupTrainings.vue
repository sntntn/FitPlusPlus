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
            {{ getTrainingTypeName(training.typeId) }}
          </td>
          <td class="py-2 px-4">{{ training.dateTime }}</td>
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

    // hardcoded trainings
    this.trainings = [
      { id: 1, typeId: "1", dateTime: "2025-10-02T18:00" },
      { id: 2, typeId: "2", dateTime: "2025-10-04T19:00" },
    ];

    // trainers trainingTypes
    const id = this.trainerId;
    dataServices.methods.get_trainer_by_id(id).then((response) => {
      this.trainer = response.data;
      console.log(response.data);
      console.log(this.trainer);
    });

    console.log(reservationService.getAllGroupReservations());

  },
  methods: {
    createTraining() {
      if (!this.newTraining.typeId || !this.newTraining.dateTime) {
        alert("Please select a training type and date/time.");
        return;
      }

      const newItem = {
        id: Date.now(),
        typeId: this.newTraining.typeId,
        dateTime: this.newTraining.dateTime,
      };

      this.trainings.push(newItem);

      // reset form
      this.newTraining = { typeId: "", dateTime: "" };

      // later API (data_services.js)
    },

    getTrainingTypeName(typeId) {
      const type = this.trainer.trainingTypes.find((t) => t.id == typeId);
      return type ? type.name : "Unknown";
    },
  },
};
</script>
