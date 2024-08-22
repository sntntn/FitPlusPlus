<template>
  <div>
    <h5>Filter by training type</h5>
    <CFormSelect v-model="selectedType" @change="applyTrainingTypeFilter">
      <option value="">All Training Types</option>
      <option v-for="type in trainingTypes" :key="type" :value="type">
        {{ type }}
      </option>
    </CFormSelect>

    <div class="filter-container">
      <h5>Filter by rating</h5>
      <input type="number"
             id="minRating"
             v-model.number="minRating"
             min="0"
             max="10"
             step="0.1" />
      <CButton color="dark" style="margin-left:20px" @click="applyRatingFilter">Apply filter</CButton>
      <CButton color="dark" style="margin-left:10px" @click="resetFilter">Reset filter</CButton>
    </div>

    <CTable caption="top" class="tbl" color="dark" striped>
      <CTableCaption style="font-weight: 600;">LIST OF TRAINERS</CTableCaption>
      <CTableHead>
        <CTableRow>
          <CTableHeaderCell class="test">Name</CTableHeaderCell>
          <CTableHeaderCell class="test">Email</CTableHeaderCell>
          <CTableHeaderCell class="test">Phone</CTableHeaderCell>
          <CTableHeaderCell class="test">Training Types</CTableHeaderCell>
          <CTableHeaderCell class="test">Rating</CTableHeaderCell>
          <CTableHeaderCell class="test">Bio</CTableHeaderCell>
          <CTableHeaderCell class="test">Actions</CTableHeaderCell>
        </CTableRow>
      </CTableHead>
      <CTableBody>
        <CTableRow v-for="(trainer, index) in filteredTrainers" :key="index">
          <CTableDataCell class="test">{{ trainer.fullName }}</CTableDataCell>
          <CTableDataCell class="test">{{ trainer.contactEmail }}</CTableDataCell>
          <CTableDataCell class="test">{{ trainer.contactPhone }}</CTableDataCell>
          <CTableDataCell class="test">{{ getTrainingTypes(trainer.trainingTypes) }}</CTableDataCell>
          <CTableDataCell class="test">{{ trainer.averageRating.toFixed(1) }}</CTableDataCell>
          <CTableDataCell class="test" v-on:click="toggleBio(index)">
            <span v-if="isBioExpanded[index]">{{ trainer.bio }}</span>
            <span v-else @click="toggleBio(index)">{{ trainer.bio.substring(0, 30) }}...</span>
          </CTableDataCell>
          <CTableDataCell class="test">
            <CButton style="margin-right: 10px" color="light" class="px-3" @click="showReviews(trainer.id,trainer.fullName,trainer.reviews)">
              View Reviews
            </CButton>
            <CButton color="light" class="px-3" @click="bookTraining(trainer.id)">
              Book Training
            </CButton>
          </CTableDataCell>
        </CTableRow>
      </CTableBody>
    </CTable>
    <ClientModal :modalData="modalData" @review-added="fetchTrainers" @review-updated="fetchTrainers" @review-deleted="fetchTrainers" />
  </div>
</template>

<script>
import dataServices from '@/services/data_services';
import ClientModal from '@/components/ClientModal.vue';

export default {
  name: "Client",
  components: {
    ClientModal
  },
  data() {
    return {
      trainers: [],
      isBioExpanded: [],
      filteredTrainers: [],
      trainingTypes: [],
      selectedType: '',
      minRating: 0.0,
      modalData: {
        isvisible: false,
        title: "Trainer Reviews",
        trainerId: '',
        trainerName: '',
        reviews: [],
        resolve: null,
        reject: null
      }
    };
  },
  methods: {
    bookTraining(trainerId) {
      this.$router.push(`/client/${this.$route.params.id}/schedule/${trainerId}`);
    },
    getTrainingTypes(trainingTypes) {
      return trainingTypes.map(type => type.name).join(', ');
    },
    toggleBio(index) {
      this.isBioExpanded[index] = !this.isBioExpanded[index];
    },
    applyTrainingTypeFilter() {
      this.$nextTick(() => {
        if (this.selectedType === '') {
          this.filteredTrainers = this.trainers;
        } else {
          dataServices.methods.get_trainers_by_type(this.selectedType).then(response => {
            this.filteredTrainers = response.data;
          });
        }
      });
    },
    fetchTrainers() {
      dataServices.methods.get_trainers().then(response => {
        this.trainers = response.data;
        this.filteredTrainers = this.trainers;
        this.extractTrainingTypes();
      });
    },
    extractTrainingTypes() {
      const types = new Set();
      this.trainers.forEach(trainer => {
        trainer.trainingTypes.forEach(type => types.add(type.name));
      });
      this.trainingTypes = Array.from(types);
    },
    applyRatingFilter() {
      const response = dataServices.methods.get_trainers_by_rating(this.minRating).then(response => {
        this.filteredTrainers = response.data;
      });
    },
    resetFilter() {
      this.minRating = 0.0;
      this.applyTrainingTypeFilter();
    },
    showReviews(id, name, reviews) {
      this.modalData.trainerId = id;
      this.modalData.trainerName = name;
      this.modalData.reviews = reviews;
      this.modalData.isvisible = true;
      return new Promise((resolve, reject) => {
        this.modalData.resolve = resolve;
        this.modalData.reject = reject;
      });
    }
  },
  mounted() {
    this.fetchTrainers();
    this.$parent.$parent.$parent.setUserData(this.$route.params.id, "client");

  }
}
</script>

<style>
  .tbl {
    width: 100%;
    border: 1px solid black;
  }

  .test {
    border: 1px solid black;
    text-align: center;
    cursor: pointer;
  }

  .action-column {
    width: 140px;
  }

  .filter-container {
    margin-top: 20px;
    margin-bottom: 20px;
  }
</style>
