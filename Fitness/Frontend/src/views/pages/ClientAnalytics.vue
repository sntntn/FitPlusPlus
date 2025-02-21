<template>
  <div>
    <!-- Section 1: Statistics About Trainings -->
    <h2>Your Analytics</h2>

    <div class="section">
      <h3>Trainings</h3>
      <div class="analytics-container">
        <div class="analytics-item">
          <h5>Total Number Trainings: <b>{{ numOfHeldTrainings + numOfCancelledTrainings }}</b></h5>
        </div>
        <div class="analytics-item">
          <h5>Number of Attended Trainings:
            <b>{{ numOfHeldTrainings }} ({{ (numOfHeldTrainings / (numOfHeldTrainings + numOfCancelledTrainings) * 100).toFixed(2) }} %)</b>
          </h5>
        </div>
        <div class="analytics-item">
          <h5> Number of Cancelled Trainings:
            <b>{{ numOfCancelledTrainings }} ({{ (numOfCancelledTrainings / (numOfHeldTrainings + numOfCancelledTrainings) * 100).toFixed(2) }} %)</b>
          </h5>
        </div>
      </div>
    </div>

    <div class="section">
      <h3>Trainers Worked With</h3>
      <CTable caption="top" class="tbl" color="dark" striped>
        <CTableHead>
          <CTableRow>
            <CTableHeaderCell>Name</CTableHeaderCell>
            <CTableHeaderCell>Email</CTableHeaderCell>
            <CTableHeaderCell>Phone</CTableHeaderCell>
            <CTableHeaderCell>Num of Trainings</CTableHeaderCell>
          </CTableRow>
        </CTableHead>
        <CTableBody>
          <CTableRow v-for="(trainer, index) in trainersWorkedWith" :key="index">
            <CTableDataCell>{{ trainer.fullName }}</CTableDataCell>
            <CTableDataCell>{{ trainer.contactEmail }}</CTableDataCell>
            <CTableDataCell>{{ trainer.contactPhone }}</CTableDataCell>
            <CTableDataCell>{{ trainer.numOfTrainings }}</CTableDataCell>
            <!-- <CTableDataCell>{{ getTrainingTypes(trainer.trainingTypes) }}</CTableDataCell>
            <CTableDataCell>{{ trainer.averageRating.toFixed(1) }}</CTableDataCell> -->
          </CTableRow>
        </CTableBody>
      </CTable>
    </div>
  </div>
</template>

<script>
// import dataServices from '@/services/data_services';
import analyticsService from '@/services/AnalyticsService'
import dataServices from '@/services/data_services';

export default {
  name: "ClientAnalytics",
  data() {
    return {
      clientTrainings: [],
      numOfHeldTrainings: 0,
      numOfCancelledTrainings: 0,
      trainersWorkedWith: []
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

<style>
.section {
  background-color: rgb(144, 161, 168);
  border-radius: 6px;
  padding: 1em;
  padding-bottom: 3px;
  margin-bottom: 1em;
}

.analytics-container {
  /* display: flex;
  justify-content: space-around; */
  margin-bottom: 20px;
}

.analytics-item {
  display: flex;
  /* text-align: center; */
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