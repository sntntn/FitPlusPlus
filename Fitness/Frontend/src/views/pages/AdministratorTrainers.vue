<template>
  <div>
    <CForm>
      <CButton color="dark" class="px-4" v-on:click="addTrainer" style="margin-right: 973px;">Add Trainer</CButton>
    </CForm>
    <CTable caption="top" class="tbl" color="dark" striped>
      <CTableCaption style="font-weight: 600;">LIST OF TRAINERS</CTableCaption>
      <CTableHead>
        <CTableRow>
          <CTableHeaderCell class="test">{{ headers.id }}</CTableHeaderCell>
          <CTableHeaderCell class="test">{{ headers.name }}</CTableHeaderCell>
          <CTableHeaderCell class="test">{{ headers.contactEmail }}</CTableHeaderCell>
          <CTableHeaderCell class="test">{{ headers.contactPhone }}</CTableHeaderCell>
          <CTableHeaderCell class="test">{{ headers.trainingType }}</CTableHeaderCell>
          <CTableHeaderCell class="test">{{ headers.rating }}</CTableHeaderCell>
          <CTableHeaderCell class="test">{{ headers.bio }}</CTableHeaderCell>
          <CTableHeaderCell class="test action-column">Actions</CTableHeaderCell>
        </CTableRow>
      </CTableHead>
      <CTableBody>
        <CTableRow v-for="(trainer, index) in trainers" :key="index">
          <CTableDataCell class="test">{{ trainer.id }}</CTableDataCell>
          <CTableDataCell class="test">{{ trainer.fullName }}</CTableDataCell>
          <CTableDataCell class="test">{{ trainer.contactEmail }}</CTableDataCell>
          <CTableDataCell class="test">{{ trainer.contactPhone }}</CTableDataCell>
          <CTableDataCell class="test">{{ getTrainingTypes(trainer.trainingTypes) }}</CTableDataCell>
          <CTableDataCell class="test">
            <span v-if="trainer.averageRating === 0.0">/</span>
            <span v-else>{{ trainer.averageRating.toFixed(1) }}</span>
          </CTableDataCell>
          <CTableDataCell class="test" v-on:click="toggleBio(index)">
            <span v-if="isBioExpanded[index]">{{ trainer.bio }}</span>
            <span v-else @click="toggleBio(index)">{{ trainer.bio.substring(0, 30) }}...</span>
          </CTableDataCell>
          <CTableDataCell class="test">
            <CButton color="light" class="px-3" style="margin: 0 10px;" v-on:click="updateTrainer(trainer.id)">
              <CIcon icon="cil-pencil" />
            </CButton>
            <CButton color="light" class="px-3" v-on:click="onDelete(trainer.id)">
              <CIcon icon="cil-trash" />
            </CButton>
          </CTableDataCell>
        </CTableRow>
      </CTableBody>
    </CTable>
    <GenericModal :modalData="modalData" />
  </div>
</template>

<script>
import dataServices from '@/services/data_services';
import GenericModal from '@/components/GenericModal.vue';

export default {
  name: "Trainers",
  components: {
    GenericModal
  },
  data() {
    return {
      headers: {
        id: "Id",
        name: "Name",
        contactEmail: "Email",
        contactPhone: "Phone",
        trainingType: "Training Type",
        rating: "Rating",
        bio: "Bio"
      },
      trainers: [],
      isBioExpanded: [],
      modalData: {
        isVisible: false,
        title: "Confirm delete",
        body: "Are you sure that you want to delete this trainer?",
        resolve: null,
        reject: null
      }
    };
  },
  methods: {
    getTrainingTypes(trainingTypes) {
      return trainingTypes.map(type => type.name).join(', ');
    },
    toggleBio(index) {
      this.isBioExpanded[index] = !this.isBioExpanded[index];
    },
    addTrainer() {
      this.$router.push('/administrator/trainers/0');
    },
    removeTrainer(id) {
      let loader = this.$loading.show();
      dataServices.methods.remove_trainer(id).then(() => {
        this.fetchTrainers();
        loader.hide();
      });
    },
    updateTrainer(id) {
      this.$router.push('/administrator/trainers/' + id);
    },
    onDelete(id) {
      this.openModal().then((result) => {
        if(result) {
          dataServices.methods.get_trainer_by_id(id).then((response) => {
            var trainer = response.data;
            dataServices.methods.unregister_user(trainer.contactEmail).then((response) => {
              this.removeTrainer(id);
            })
          });
        }
        this.modalData.isVisible = false;
        this.modalData.resolve = null;
        this.modalData.reject = null;
      });
    },
    openModal() {
      return new Promise((resolve, reject) => {
        this.modalData.isVisible = true;
        this.modalData.resolve = resolve;
        this.modalData.reject = reject;
      });
    },
    fetchTrainers() {
      let loader = this.$loading.show();
      dataServices.methods.get_trainers().then(response => {
        this.trainers = response.data;
        this.isBioExpanded = Array(this.trainers.length).fill(false);
        loader.hide();
      }).catch(error => {
        console.error('Error fetching trainers:', error);
        loader.hide();
      });
    }
  },
  mounted() {
    this.fetchTrainers();
  }
};
</script>

<style>
  #txt {
    font-size: 14px;
    color: red;
    text-align: center;
    font-family: Verdana, Geneva, Tahoma, sans-serif;
  }

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
</style>
