<template>
  <h2>Your Analytics</h2>

  <div class="page">
    <div class="training-statistics">
      <!-- Individual Trainings -->
      <div class="training-statistics-container">
        <h2>Individual trainings</h2>
        <hr>
        <div class="stats-section">
          <div class="stat">
            <span class="label">Total number of individual trainings:</span>
            <span class="value">{{ totalTrainings }}</span>
          </div>

          <div class="stat">
            <span class="label">My average rating:</span>
            <span class="value">{{ myAverageRating }}</span>
          </div>

          <div class="stat">
            <span class="label">Trainer average rating:</span>
            <span class="value">{{ trainerAverageRating }}</span>
          </div>

          <div class="stat">
            <span class="label">Number of individual trainings per month:</span>
            <div class="histogram">
              <Bar
                id="trainings-per-month"
                :options="{
                  responsive: true,
                  animation: { duration: 1000 }
                }"
                :data="{
                  labels: this.months,
                  datasets: [{
                    label: 'Trainings',
                    data: this.trainingsPerMonth,
                    backgroundColor: '#31872e'
                  }]
                }"
              />
            </div>
          </div>

          <div class="stat">
            <span class="label">Number of trainings per trainer:</span>
            <div class="pie-chart">
              <Pie
                id="trainers-worked-with"
                :options="{ responsive: true }"
                :data="{
                  labels: this.trainersWorkedWith,
                  datasets: [{
                    backgroundColor: ['#41B883', '#E46651', '#00D8FF', '#DD1B16'],
                    data: this.numTrainingsByTrainer
                  }]
                }"
              />
            </div>
          </div>

          <div class="stat">
            <span class="label">Number of trainings per training types:</span>
            <div class="pie-chart">
              <Pie
                id="trainers-worked-with"
                :options="{ responsive: true }"
                :data="{
                  labels: this.trainersWorkedWith,
                  datasets: [{
                    backgroundColor: ['#41B883', '#E46651', '#00D8FF', '#DD1B16'],
                    data: this.numTrainingsByTrainer
                  }]
                }"
              />
            </div>
          </div>
        </div>

      </div>

      <!-- Group Trainings -->
      <div class="training-statistics-container">
        <h2>Group trainings</h2>
        <hr>

        <div class="stats-section">
          <div class="stat">
            <span class="label">Total number of group trainings:</span>
            <span class="value">{{ totalTrainings }}</span>
          </div>

          <div class="stat">
            <span class="label">My average rating:</span>
            <span class="value">{{ myAverageRating }}</span>
          </div>

          <div class="stat">
            <span class="label">Average group rating:</span>
            <span class="value">{{ trainerAverageRating }}</span>
          </div>

          <div class="stat">
            <span class="label">Trainer average rating:</span>
            <span class="value">{{ trainerAverageRating }}</span>
          </div>

          <div class="stat">
            <span class="label">Number of group trainings per month:</span>
            <div class="histogram">
              <Bar
                id="trainings-per-month"
                :options="{
                  responsive: true,
                  animation: { duration: 1000 }
                }"
                :data="{
                  labels: this.months,
                  datasets: [{
                    label: 'Trainings',
                    data: this.trainingsPerMonth,
                    backgroundColor: '#31872e'
                  }]
                }"
              />
            </div>
          </div>

          <div class="stat">
            <span class="label">Number of trainings per trainer:</span>
            <div class="pie-chart">
              <Pie
                id="trainers-worked-with"
                :options="{ responsive: true }"
                :data="{
                  labels: this.trainersWorkedWith,
                  datasets: [{
                    backgroundColor: ['#41B883', '#E46651', '#00D8FF', '#DD1B16'],
                    data: this.numTrainingsByTrainer
                  }]
                }"
              />
            </div>
          </div>

        </div>
      </div>
    </div>

  </div>
</template>

<script>
// import dataServices from '@/services/data_services';
import analyticsService from '@/services/AnalyticsService'
import dataServices from '@/services/data_services';

import { Bar, Pie } from 'vue-chartjs'
import { Chart as ChartJS, ArcElement, Title, Tooltip, Legend, BarElement, CategoryScale, LinearScale } from 'chart.js'

ChartJS.register(ArcElement, Title, Tooltip, Legend, BarElement, CategoryScale, LinearScale)

export default {
  name: "ClientAnalytics",
  components: { Bar, Pie },
  data() {
    return {
      clientTrainings: [],
      numOfHeldTrainings: 0,
      numOfCancelledTrainings: 0,
      trainersWorkedWith: ["Vukasin", "Natalija"],
      numTrainingsByTrainer: [16, 29],
      months: ["Jan","Feb","Mar","Apr","May","Jun","Jul","Aug","Sep","Oct","Nov","Dec"],
      trainingsPerMonth: [0, 0, 0, 0, 12, 7, 9, 11, 6, 8, 0, 13]
    };
  },
  methods: {
    getTrainingTypes(trainingTypes) {
      return trainingTypes.map(type => type.name).join(', ');
    },
    fetchAnalytics() {
      const clientId = this.$route.params.id;
      analyticsService.getClientAnalytics(clientId).then(response => {
        this.clientTrainings = response.data;
        this.calculateTrainingStatistics();
        this.calculateTrainersStatistics();
      });
    },
    calculateTrainingStatistics() {
      this.clientTrainings.forEach(training => {
        if (training.status == 0) {
          this.numOfHeldTrainings++;
        }
        else {
          this.numOfCancelledTrainings++;
        }
      });
    },
    calculateTrainersStatistics() {
      let trainers = new Map();
      this.clientTrainings.forEach(training => {
        let id = training.trainerId;
        if (trainers.has(id)) {
          trainers.set(id, trainers.get(id) + 1);
        }
        else {
          trainers.set(id, 1);
        }
      });

      dataServices.methods.get_trainers().then(response => {
        response.data.forEach(trainer => {
          if (trainers.has(trainer.id)) {
            this.trainersWorkedWith.push({
              "id": trainer.id,
              "fullName": trainer.fullName,
              "contactEmail": trainer.contactEmail,
              "contactPhone": trainer.contactPhone,
              "numOfTrainings": trainers.get(trainer.id)
            });
          }
        });
      });
    }
  },
  mounted() {
    this.fetchAnalytics();
    this.$parent.$parent.$parent.setUserData(this.$route.params.id, "client");
  }
}
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
.training-statistics {
  flex: 2;
  display: flex;
  gap: 20px;
}

.training-statistics-container {
  flex: 1;
  background: #fff;
  padding: 16px;
  border-radius: 12px;
  box-shadow: 0 2px 6px rgba(0, 0, 0, 0.1);
}

.training-statistics-container h2 {
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

.stats-section {
  width: 100%;
  display: flex;
  flex-direction: column; /* makes them vertical */
  align-items: flex-start;
  gap: 1rem; /* space between rows */
}

.stat {
  width: 100%;
  display: flex;
  flex-direction: column;
}

.label {
  font-weight: bold;
}

.value {
  font-size: 1.2rem;
  color: #333;
}

.histogram {
  width: 80%;
  margin: auto;
}

.pie-chart {
  width: 40%;
  margin: auto;
}
</style>
