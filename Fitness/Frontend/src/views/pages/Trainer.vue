<template>
  <div class="trainer-page">
    <h1>{{ trainer.fullName }}</h1>
    <div class="editable-field">
      <p><strong>Email:</strong> {{ trainer.contactEmail }}</p>
      <CButton class="edit-btn" color="dark" @click="toggleEditEmail">Edit Email</CButton>
    </div>
    <div v-if="editEmail" class="email-section">
      <CFormInput v-model="newEmail" placeholder="Update your email" />
      <CButton class="spacing_2" color="dark" @click="updateEmail">Save Email</CButton>
    </div>

    <div class="editable-field">
      <p><strong>Phone:</strong> {{ trainer.contactPhone }}</p>
      <CButton class="edit-btn" color="dark" @click="toggleEditPhone">Edit Phone</CButton>
    </div>
    <div v-if="editPhone" class="phone-section">
      <CFormInput v-model="newPhone" placeholder="Update your phone" />
      <CButton class="spacing_2" color="dark" @click="updatePhone">Save Phone</CButton>
    </div>

    <div class="editable-field">
      <p><strong>Bio:</strong> {{ trainer.bio }}</p>
      <CButton class="edit-btn" color="dark" @click="toggleEditBio">Edit Bio</CButton>
    </div>
    <div v-if="editBio" class="bio-section">
      <CFormInput v-model="newBio" placeholder="Update your bio" />
      <CButton class="spacing_2" color="dark" @click="updateBio">Save Bio</CButton>
    </div>

    <CTable caption="top" class="tbl" color="dark" striped>
      <CTableCaption style="font-weight: 600;">LIST OF TRAINING TYPES</CTableCaption>
      <CTableHead>
        <CTableRow>
          <CTableHeaderCell class="test">Name</CTableHeaderCell>
          <CTableHeaderCell class="test">Duration</CTableHeaderCell>
          <CTableHeaderCell class="test">Difficulty</CTableHeaderCell>
          <CTableHeaderCell class="test">Actions</CTableHeaderCell>
        </CTableRow>
      </CTableHead>
      <CTableBody>
        <CTableRow v-for="(type, index) in trainer.trainingTypes" :key="index">
          <CTableDataCell class="test">{{ type.name }}</CTableDataCell>
          <CTableDataCell class="test">{{ type.duration }}</CTableDataCell>
          <CTableDataCell class="test">{{ type.difficulty }}</CTableDataCell>
          <CTableDataCell class="test action-column">
            <CButton color="light" class="px-3" style="margin: 0 10px;" v-on:click="toggleEditTrainingType(type)">
              <CIcon icon="cil-pencil" />
            </CButton>
            <CButton color="light" class="px-3" v-on:click="onDelete(type.id)">
              <CIcon icon="cil-trash" />
            </CButton>
          </CTableDataCell>
        </CTableRow>
      </CTableBody>
    </CTable>
    <GenericModal :modalData="modalData" />

    <div v-if="editTrainingType" class="edit-training-type-section">
      <h3>Edit Training Type</h3>
      <CFormLabel for="editTrainingTypeDuration" style="display: block;">Duration</CFormLabel>
      <CInputGroup style="width:70%; margin-bottom: 10px !important">
        <CFormInput id="editTrainingTypeDuration" placeholder="Update training type duration (HH:MM:SS)" v-model="trainingTypeToEdit.duration" />
      </CInputGroup>
      <CFormLabel for="editTrainingTypeDifficulty" style="display: block;">Difficulty</CFormLabel>
      <CInputGroup style="width:70%; margin-bottom: 10px !important">
        <CFormSelect id="editTrainingTypeDifficulty" v-model="trainingTypeToEdit.difficulty">
          <option value="Beginner">Beginner</option>
          <option value="Intermediate">Intermediate</option>
          <option value="Advanced">Advanced</option>
        </CFormSelect>
      </CInputGroup>
      <CButton class="spacing" color="dark" @click="updateTrainingType">Save Changes</CButton>
    </div>

    <div class="training-type-section">
      <CButton class="spacing" color="dark" @click="toggleAddTrainingType">Add Training Type</CButton>
      <div v-if="addTrainingType">
        <CFormLabel for="trainingTypeName" style="display: block;">Name</CFormLabel>
        <CInputGroup style="width:70%; margin-bottom: 10px !important">
          <CFormInput id="trainingTypeName" placeholder="Please insert training type name" v-model="trainingType.name" />
        </CInputGroup>
        <CFormLabel for="trainingTypeDuration" style="display: block;">Duration</CFormLabel>
        <CInputGroup style="width:70%; margin-bottom: 10px !important">
          <CFormInput id="trainingTypeDuration" placeholder="Please insert training type duration (HH:MM:SS)" v-model="trainingType.duration" />
        </CInputGroup>
        <CFormLabel for="trainingTypeDifficulty" style="display: block;">Difficulty</CFormLabel>
        <CInputGroup style="width:70%; margin-bottom: 10px !important">
          <CFormSelect id="trainingTypeDifficulty" v-model="trainingType.difficulty">
            <option value="Beginner">Beginner</option>
            <option value="Intermediate">Intermediate</option>
            <option value="Advanced">Advanced</option>
          </CFormSelect>
        </CInputGroup>
        <CButton class="spacing" color="dark" @click="addType">Add</CButton>
      </div>
    </div>
  </div>
</template>

<script>
  import dataServices from '../../services/data_services';
  import GenericModal from '@/components/GenericModal.vue';

  export default {
    name: "Trainer",
    components: {
      GenericModal
    },
    data() {
      return {
        trainer: {},
        editBio: false,
        editEmail: false,
        editPhone: false,
        addTrainingType: false,
        newBio: '',
        newEmail: '',
        newPhone: '',
        trainingType: {
          id: '',
          name: '',
          duration: '',
          difficulty: 'Beginner'
        },
        editTrainingType: false,
        trainingTypeToEdit: {
          id: '',
          name: '',
          duration: '',
          difficulty: ''
        },
        modalData: {
          isVisible: false,
          title: "Confirm delete",
          body: "Are you sure that you want to delete this training type?",
          resolve: null,
          reject: null
        }
      };
    },
    methods: {
      fetchTrainer() {
        const id = this.$route.params.id;
        dataServices.methods.get_trainer_by_id(id).then((response) => {
          this.trainer = response.data;
        });
      },
      toggleEditBio() {
        this.editBio = !this.editBio;
        this.newBio = this.trainer.bio;
      },
      updateBio() {
        const id = this.$route.params.id;
        dataServices.methods.get_trainer_by_id(id).then((response) => {
          const trainer = response.data;
          trainer.Bio = this.newBio;

          dataServices.methods.upt_trainer(id, trainer).then(() => {
            this.trainer.bio = this.newBio;
            this.editBio = false;
          });
        });
      },
      toggleEditEmail() {
        this.editEmail = !this.editEmail;
        this.newEmail = this.trainer.contactEmail;
      },
      updateEmail() {
        const id = this.$route.params.id;
        dataServices.methods.get_trainer_by_id(id).then((response) => {
          const trainer = response.data;
          trainer.ContactEmail = this.newEmail;

          dataServices.methods.upt_trainer(id, trainer).then(() => {
            this.trainer.contactEmail = this.newEmail;
            this.editEmail = false;
          });
        });
      },
      toggleEditPhone() {
        this.editPhone = !this.editPhone;
        this.newPhone = this.trainer.contactPhone;
      },
      updatePhone() {
        const id = this.$route.params.id;
        dataServices.methods.get_trainer_by_id(id).then((response) => {
          const trainer = response.data;
          trainer.ContactPhone = this.newPhone;

          dataServices.methods.upt_trainer(id, trainer).then(() => {
            this.trainer.contactPhone = this.newPhone;
            this.editPhone = false;
          });
        });
      },
      toggleEditTrainingType(type) {
        this.editTrainingType = true;
        this.trainingTypeToEdit = { ...type };
      },
      updateTrainingType() {
        const updatedTrainingTypes = this.trainer.trainingTypes.map(type =>
          type.id === this.trainingTypeToEdit.id ? this.trainingTypeToEdit : type
        );
        const trainer = { ...this.trainer, trainingTypes: updatedTrainingTypes };

        dataServices.methods.upt_trainer(trainer.id, trainer).then(() => {
          this.editTrainingType = false;
          this.trainingTypeToEdit = { id: '', name: '', duration: '', difficulty: '' };
          this.fetchTrainer();
         });

      },
      toggleAddTrainingType() {
        this.addTrainingType = !this.addTrainingType;
      },
      addType() {
        const updatedTrainingTypes = [...this.trainer.trainingTypes, this.trainingType];
        const trainer = { ...this.trainer, trainingTypes: updatedTrainingTypes };

        dataServices.methods.upt_trainer(trainer.id, trainer).then(() => {
          this.addTrainingType = false;
          this.trainingType = {id: '', name: '', duration: '', difficulty: '' };
          this.fetchTrainer();
        });
      },
      deleteTrainingType(typeId) {
        const updatedTrainingTypes = this.trainer.trainingTypes.filter(type => type.id !== typeId);
        const trainer = { ...this.trainer, trainingTypes: updatedTrainingTypes };

        dataServices.methods.upt_trainer(trainer.id, trainer).then(() => {
          this.fetchTrainer();
        });
      },
      onDelete(id) {
        this.openModal().then((result) => {
          if (result) {
            this.deleteTrainingType(id);
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
      }
    },
    mounted() {
      this.fetchTrainer();
    }
  };
</script>

<style scoped>
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

  .spacing {
    margin-bottom: 15px; /* Add space below the element */
  }

  .spacing_2 {
    margin-bottom: 15px; /* Add space below the element */
    margin-top: 15px;
  }

  .edit-btn {
    display: inline-block;
    margin-left: 10px;
  }

  .editable-field {
    display: flex;
    align-items: center;
    margin-bottom: 15px;
  }

    .editable-field p {
      margin: 0;
    }

    .editable-field .edit-btn {
      margin-left: 10px;
    }

  .action-column {
    width: 140px;
  }
</style>

