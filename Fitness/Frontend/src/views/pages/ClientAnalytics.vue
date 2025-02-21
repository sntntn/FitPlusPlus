<template>
  <div>
    <!-- Section 1: Statistics About Trainings -->
    <div class="statistics-section">
      <h3>Training Statistics</h3>
      <div class="statistics-grid">
        <div class="statistic-card">
          <h5>Total Trainings</h5>
          <p>{{ numOfHeldTrainings + numOfCancelledTrainings }}</p>
        </div>
        <div class="statistic-card">
          <h5>Held Trainings</h5>
          <p>{{ numOfHeldTrainings }}</p>
        </div>
        <div class="statistic-card">
          <h5>Cancelled Trainings</h5>
          <p>{{ numOfCancelledTrainings }}</p>
        </div>
      </div>

      <h4>Trainings by Type</h4>
      <CTable caption="top" class="tbl" color="dark" striped>
        <CTableHead>
          <CTableRow>
            <CTableHeaderCell>Training Type</CTableHeaderCell>
            <CTableHeaderCell>Count</CTableHeaderCell>
          </CTableRow>
        </CTableHead>
        <CTableBody>
          <!-- <CTableRow v-for="(count, type) in statistics.trainingsByType" :key="type">
            <CTableDataCell>{{ type }}</CTableDataCell>
            <CTableDataCell>{{ count }}</CTableDataCell>
          </CTableRow> -->
        </CTableBody>
      </CTable>
    </div>

    <h3>Your Analytics</h3>
    <div class="analytics-container">
      <div class="analytics-item">
        <h5>Total Number Trainings</h5>
        <p>{{ numOfHeldTrainings + numOfCancelledTrainings }}</p>
      </div>
      <div class="analytics-item">
        <h5>Number of Attended Trainings</h5>
        <p>{{ numOfHeldTrainings }}</p>
      </div>
      <div class="analytics-item">
        <h5>Number of Cancelled Trainings</h5>
        <p>{{ numOfCancelledTrainings }}</p>
      </div>
      <div class="analytics-item">
        <h5>Average Rating of Trainings</h5>
        <p>{{ 0 }}</p>
      </div>
    </div>

    <h4>Trainers Worked With</h4>
    <CTable caption="top" class="tbl" color="dark" striped>
      <CTableHead>
        <CTableRow>
          <CTableHeaderCell>Name</CTableHeaderCell>
          <CTableHeaderCell>Email</CTableHeaderCell>
          <CTableHeaderCell>Phone</CTableHeaderCell>
          <CTableHeaderCell>Training Types</CTableHeaderCell>
          <CTableHeaderCell>Rating</CTableHeaderCell>
        </CTableRow>
      </CTableHead>
      <!-- <CTableBody>
        <CTableRow v-for="(trainer, index) in analytics.trainersWorkedWith" :key="index">
          <CTableDataCell>{{ trainer.fullName }}</CTableDataCell>
          <CTableDataCell>{{ trainer.contactEmail }}</CTableDataCell>
          <CTableDataCell>{{ trainer.contactPhone }}</CTableDataCell>
          <CTableDataCell>{{ getTrainingTypes(trainer.trainingTypes) }}</CTableDataCell>
          <CTableDataCell>{{ trainer.averageRating.toFixed(1) }}</CTableDataCell>
        </CTableRow>
      </CTableBody> -->
    </CTable>
  </div>
</template>

<script>
// import dataServices from '@/services/data_services';
import analyticsService from '@/services/AnalyticsService'

export default {
  name: "ClientAnalytics",
  data() {
    return {
      clientTrainings: [],
      numOfHeldTrainings: 0,
      numOfCancelledTrainings: 0
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
        this.calculateStatistics();
      });
    },
    calculateStatistics() {
      this.clientTrainings.forEach(training => {
        if (training.status == 0) {
          this.numOfHeldTrainings++;
        }
        else {
          this.numOfCancelledTrainings++;
        }
      });
    }
  },
  mounted() {
    this.fetchAnalytics();
    this.$parent.$parent.$parent.setUserData(this.$route.params.id, "client");
  }
}
</script>

<style>
.analytics-container {
  /* display: flex;
  justify-content: space-around; */
  margin-bottom: 20px;
}

.analytics-item {
  text-align: center;
}

.analytics-item h5 {
  margin-bottom: 10px;
}

.analytics-item p {
  font-size: 1.5em;
  font-weight: bold;
}

.tbl {
  width: 100%;
  border: 1px solid black;
}

.CTableHeaderCell, .CTableDataCell {
  border: 1px solid black;
  text-align: center;
}
</style>