<template>
  <div>
    <h3>Client Analytics</h3>
    <div class="analytics-container">
      <div class="analytics-item">
        <h5>Number of Attended Trainings</h5>
        <p>{{ analytics.attendedTrainings }}</p>
      </div>
      <div class="analytics-item">
        <h5>Number of Cancelled Trainings</h5>
        <p>{{ analytics.cancelledTrainings }}</p>
      </div>
      <div class="analytics-item">
        <h5>Average Rating of Trainings</h5>
        <p>{{ analytics.averageRating.toFixed(1) }}</p>
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
      <CTableBody>
        <CTableRow v-for="(trainer, index) in analytics.trainersWorkedWith" :key="index">
          <CTableDataCell>{{ trainer.fullName }}</CTableDataCell>
          <CTableDataCell>{{ trainer.contactEmail }}</CTableDataCell>
          <CTableDataCell>{{ trainer.contactPhone }}</CTableDataCell>
          <CTableDataCell>{{ getTrainingTypes(trainer.trainingTypes) }}</CTableDataCell>
          <CTableDataCell>{{ trainer.averageRating.toFixed(1) }}</CTableDataCell>
        </CTableRow>
      </CTableBody>
    </CTable>
  </div>
</template>

<script>
import dataServices from '@/services/data_services';

export default {
  name: "ClientAnalytics",
  data() {
    return {
      analytics: {
        attendedTrainings: 0,
        cancelledTrainings: 0,
        averageRating: 0,
        trainersWorkedWith: []
      }
    };
  },
  methods: {
    getTrainingTypes(trainingTypes) {
      return trainingTypes.map(type => type.name).join(', ');
    },
    fetchAnalytics() {
      const clientId = this.$route.params.id;
      dataServices.methods.get_client_analytics(clientId).then(response => {
        // console.log(response);
        this.analytics = response.data;
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
  display: flex;
  justify-content: space-around;
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