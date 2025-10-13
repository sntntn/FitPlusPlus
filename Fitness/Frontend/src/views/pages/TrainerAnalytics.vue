<template>
  <h2>Analytics</h2>

  <div class="page">
    <div class="training-statistics">
      <!-- Individual Trainings -->
      <div class="training-statistics-container">
        <h2>Individual trainings</h2>
        <hr>
        <div class="stats-section">
          <div class="num-stat">
            <span class="label">Total number of individual trainings:</span>
            <span class="value">{{ numberOfIndividualTrainings }}</span>
          </div>

          <div class="stat">
            <span class="label">Individual training status:</span>
            <div class="pie-chart">
              <Pie
                id="trainers-worked-with"
                :options="{ responsive: true }"
                :data="{
                  labels: ['Active', 'Cancelled by Trainer', 'Cancelled by Client'],
                  datasets: [{
                    backgroundColor: ['#eba834', '#eb3d34', '#a5e339'],
                    data: individualTrainingStatus
                  }]
                }"
              />
            </div>
          </div>

          <div class="num-stat">
            <span class="label">My average rating:</span>
            <span class="value">{{ averageIndividualRating }}</span>
          </div>

          <div class="num-stat">
            <span class="label">Average client rating:</span>
            <span class="value">{{ clientAverageIndividualRating }}</span>
          </div>

          <div class="stat">
            <span class="label">Monthy income:</span>
            <div class="histogram">
              <Line
                id="trainings-per-month"
                :options="{
                  responsive: true,
                  animation: { duration: 1000 }
                }"
                :data="{
                  labels: this.months,
                  datasets: [{
                    label: 'Income ($)',
                    data: individualIncomePerMonth,
                    backgroundColor: 'lightgreen'
                  }]
                }"
              />
            </div>
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
                    data: individualTrainingsPerMonth,
                    backgroundColor: '#31872e'
                  }]
                }"
              />
            </div>
          </div>

          <div class="stat">
            <span class="label">Number of trainings per client:</span>
            <div class="histogram">
              <Bar
                id="trainings-per-month"
                :options="{
                  responsive: true,
                  animation: { duration: 1000 }
                }"
                :data="{
                  labels: individualClientNames,
                  datasets: [{
                    label: 'Trainings',
                    data: individualTrainingsPerClient,
                    backgroundColor: '#11322e'
                  }]
                }"
              />
            </div>
          </div>

          <div class="stat">
            <span class="label">Number of trainings per training types:</span>
            <div class="histogram">
              <Bar
                id="trainings-per-month"
                :options="{
                  responsive: true,
                  animation: { duration: 1000 }
                }"
                :data="{
                  labels: individualTrainingTypeNames,
                  datasets: [{
                    label: 'Trainings',
                    data: individualTrainingsPerTrainingType,
                    backgroundColor: trainingsPerTrainingTypeBackgroundColors
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
          <div class="num-stat">
            <span class="label">Total number of group trainings:</span>
            <span class="value">{{ numberOfGroupTrainings }}</span>
          </div>

          <div class="stat">
            <span class="label">Group training status:</span>
            <div class="pie-chart">
              <Pie
                id="trainers-worked-with"
                :options="{ responsive: true }"
                :data="{
                  labels: ['Active', 'Cancelled by Trainer'],
                  datasets: [{
                    backgroundColor: ['#eba834', '#eb3d34'],
                    data: groupTrainingStatus
                  }]
                }"
              />
            </div>
          </div>

          <div class="num-stat">
            <span class="label">My average rating:</span>
            <span class="value">{{ averageMyGroupRating }}</span>
          </div>

          <div class="num-stat">
            <span class="label">Average group rating:</span>
            <span class="value">{{ averageGroupRating }}</span>
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
                    data: groupTrainingsPerMonth,
                    backgroundColor: '#31872e'
                  }]
                }"
              />
            </div>
          </div>

          <div class="stat">
            <span class="label">Number of trainings per client:</span>
            <div class="histogram">
              <Bar
                id="trainings-per-month"
                :options="{
                  responsive: true,
                  animation: { duration: 1000 }
                }"
                :data="{
                  labels: groupClientNames,
                  datasets: [{
                    label: 'Trainings',
                    data: groupTrainingsPerClient,
                    backgroundColor: '#11322e'
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
import dataServices from '@/services/data_services';
import analyticsService from '@/services/AnalyticsService'

import { Bar, Pie, Line } from 'vue-chartjs'
import {
  Chart as ChartJS,
  ArcElement,
  Title,
  Tooltip,
  Legend,
  BarElement,
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement
} from 'chart.js'

ChartJS.register(
  ArcElement,
  Title,
  Tooltip,
  Legend,
  BarElement,
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement
)

export default {
  name: "ClientAnalytics",
  components: { Bar, Pie, Line },
  data() {
    return {
      individualTrainings: [],
      groupTrainings: [],
      trainers: [],
      clients: [],
      months: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"]
    };
  },

  methods: {
    fetchIndividualTrainings() {
      const trainerId = this.$route.params.id;
      analyticsService.getTrainerIndividualTrainings(trainerId)
        .then(response => {
          this.individualTrainings = response.data;
        });
    },

    fetchGroupTrainings() {
      const trainerId = this.$route.params.id;
      analyticsService.getTrainerGroupTrainings(trainerId)
        .then(response => {
          this.groupTrainings = response.data;
        });
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

    randomColor(i, total) {
      return `hsl(${(i * 360) / total}, 70%, 60%)`;
    }
  },

  mounted() {
    this.fetchIndividualTrainings();
    this.fetchGroupTrainings();
    this.fetchTrainers();
    this.fetchClients();
    this.$parent.$parent.$parent.setUserData(this.$route.params.id, "trainer");
  },

  computed: {
    // Individual Trainings
    numberOfIndividualTrainings() {
      return this.individualTrainings.filter(it => it.status == 0).length;
    },

    individualTrainingStatus() {
      const trainingStatus = ['Active', 'Cancelled by Trainer', 'Cancelled by Client'];
      return trainingStatus.map((_, i) => this.individualTrainings.filter(it => it.status == i).length);
    },

    averageIndividualRating() {
      const reviews = this.individualTrainings.map(it => it.trainerReview ?? []).flat();
      return reviews.length == 0 ? "--" : reviews.reduce((a, r) => a + r.rating, 0) / reviews.length;
    },

    clientAverageIndividualRating() {
      const reviews = this.individualTrainings.map(it => it.clientReview ?? []).flat();
      return reviews.length == 0 ? "--" : reviews.reduce((a, r) => a + r.rating, 0) / reviews.length;
    },

    individualIncomePerMonth() {
      const trainerId = this.$route.params.id;
      let trainer = this.trainers.find(t => t.id == trainerId);
      let trainingTypes = trainer !== undefined ? trainer.trainingTypes : [];

      let months = new Array(12).fill(0);
      let trainingTypesPerMonth = months.map((_, i) =>
        this.individualTrainings
          .filter(it => {
            let date = new Date(it.date);
            return it.status == 0 && date.getMonth() == i && date.getFullYear() == (new Date).getFullYear();
          })
          .map(it => trainingTypes.find(tt => tt.id == it.trainingTypeId))
        );
      return trainingTypesPerMonth.map(m => m.reduce((a, tt) => a + (tt !== undefined ? tt.price : 0), 0));
    },

    individualTrainingsPerMonth() {
      let trainingsPerMonth = new Array(12).fill(0);
      return trainingsPerMonth.map((_, i) =>
        this.individualTrainings
          .filter(it => {
            let date = new Date(it.date);
            return it.status == 0 && date.getMonth() == i && date.getFullYear() == (new Date).getFullYear();
          })
          .length
        );
    },

    individualClients() {
      return this.clients
        .filter(c => this.individualTrainings.filter(it => it.status == 0).map(it => it.clientId).includes(c.id));
    },

    individualClientNames() {
      return this.individualClients.map(c => c.name + " " + c.surname);
    },

    individualTrainingsPerClient() {
      return this.individualClients.map(c => this.individualTrainings.filter(it => it.status == 0 && it.clientId == c.id).length)
    },

    individualTrainingTypes() {
      const trainerId = this.$route.params.id;
      let trainer = this.trainers.find(t => t.id == trainerId);
      let trainingTypes = trainer !== undefined ? trainer.trainingTypes : [];
      return trainingTypes.filter(tt => this.individualTrainings
        .filter(it => it.status == 0)
        .map(it => it.trainingTypeId).includes(tt.id));
    },

    individualTrainingTypeNames() {
      return this.individualTrainingTypes.map(t => t.name);
    },

    individualTrainingsPerTrainingType() {
      return this.individualTrainingTypes.map(t => this.individualTrainings.filter(it => it.status == 0 && it.trainingTypeId == t.id).length);
    },

    trainingsPerTrainingTypeBackgroundColors() {
      return this.individualTrainingsPerTrainingType.map((_, i) => this.randomColor(i, this.individualTrainingsPerTrainingType.length))
    },

    // Group Trainings
    numberOfGroupTrainings() {
      return this.groupTrainings.filter(gt => gt.status == 0).length;
    },

    groupTrainingStatus() {
      const trainingStatus = ['Active', 'Cancelled by Trainer'];
      return trainingStatus.map((_, i) => this.groupTrainings.filter(gt => gt.status == i).length);
    },

    averageMyGroupRating() {
      const myReviews =
        this.groupTrainings.map(gt => gt.trainerReview ?? []).flat();
      return myReviews.length == 0 ? "--" : myReviews.reduce((a, r) => a + r.rating, 0) / myReviews.length;
    },

    averageGroupRating() {
      const reviews = this.groupTrainings.map(gt => gt.clientReviews ?? []).flat();
      return reviews.length == 0 ? "--" : reviews.reduce((a, r) => a + r.rating, 0) / reviews.length;
    },

    groupTrainingsPerMonth() {
      let trainingsPerMonth = new Array(12).fill(0);
      return trainingsPerMonth.map((_, i) =>
        this.groupTrainings
          .filter(it => {
            let date = new Date(it.date);
            return it.status == 0 && date.getMonth() == i && date.getFullYear() == (new Date).getFullYear();
          })
          .length
        );
    },

    groupClients() {
      return this.clients
        .filter(c => this.groupTrainings.filter(gt => gt.status == 0).map(gt => gt.clientIds).flat().includes(c.id));
    },

    groupClientNames() {
      return this.groupClients.map(c => c.name + " " + c.surname);
    },

    groupTrainingsPerClient() {
      return this.groupClients
        .map(c => this.groupTrainings.filter(gt => gt.status == 0 && gt.clientIds.includes(c.id)).length)
    },
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

.num-stat {
  width: 100%;
  display: flex;
  flex-direction: row;
}

.label {
  font-weight: bold;
}

.value {
  font-size: 1.2rem;
  color: #333;
  margin-top: -2px;
  margin-left: 5px;
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
