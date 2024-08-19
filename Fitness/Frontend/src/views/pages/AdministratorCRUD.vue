<template>
  <div class="bg-light d-flex flex-row align-items-center">
    <CContainer style="margin-top: 50px;">
      <CRow class="justify-content-center">
        <CCol md="8">
          <CCardGroup>
            <CCard class="p-4">
              <CCardBody class="form">
                <CForm>
                  <h1 style="margin-bottom: 10px">{{isAdd ? 'Add trainer':'Update trainer'}}</h1>

                  <CFormLabel for="name" style="display: block;">Name</CFormLabel>
                  <CInputGroup style="width:70%; margin-bottom: 10px !important">
                    <CFormInput id="name" v-model="name" :disabled="!isAdd" placeholder="Please insert trainer name" />
                  </CInputGroup>
                  <span v-if="errors.name" style="color: red">{{ errors.name }}</span>


                  <CFormLabel for="contactEmail" style="display: block;">Contact Email</CFormLabel>
                  <CInputGroup style="width:70%; margin-bottom: 10px !important">
                    <CFormInput id="contactEmail" :disabled="!isAdd" placeholder="Please insert contact email" v-model="contactEmail" />
                  </CInputGroup>
                  <span v-if="errors.contactEmail" style="color: red">{{ errors.contactEmail }}</span>


                  <CFormLabel for="contactPhone" style="display: block;">Contact Phone</CFormLabel>
                  <CInputGroup style="width:70%; margin-bottom: 10px !important">
                    <CFormInput id="contactPhone" placeholder="Please insert contact phone" v-model="contactPhone" />
                  </CInputGroup>
                  <span v-if="errors.contactPhone" style="color: red">{{ errors.contactPhone }}</span>


                  <CFormLabel for="bio" style="display: block;">Bio</CFormLabel>
                  <CInputGroup style="width:90%; margin-bottom: 10px !important">
                    <CFormInput id="bio" placeholder="Please insert bio" v-model="bio" />
                  </CInputGroup>

                  <div class="d-grid d-md-block" style="text-align: center; margin-top:20px">
                    <CButton color="light" class="px-4" v-on:click="cancel" style="margin: 0 10px">Cancel</CButton>
                    <CButton color="dark" class="px-4" v-on:click="isAdd ? addTrainer() : uptTrainer()" style="margin: 0 10px">{{isAdd? 'Add':'Update'}}</CButton>
                  </div>
                </CForm>
              </CCardBody>
            </CCard>
          </CCardGroup>
        </CCol>
      </CRow>
    </CContainer>
  </div>
</template>

<script>
import dataServices from '../../services/data_services';

export default {
    name: "TrainerCrud",
    data() {
        return {
            isUpt: false,
            isAdd: false,
            id:"",
            name: "",
            contactEmail: "",
            contactPhone: "",
            bio: "",
            trainingType: "",
            rating: null,
            errors: {
              name: null,
              contactEmail: null,
              contactPhone: null
            }
        };
    },
    methods: {
        validateEmail(email) {
          const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
          return emailPattern.test(email);
        },
        validateFullName(name) {
          return name.split(' ').length >= 2;
        },
        validatePhone(phone) {
          const phonePattern = /^\d{3}-\d{4}$/;
          return phonePattern.test(phone);
        },
        validateForm() {
          this.errors.name = !this.validateFullName(this.name) ? 'Name must contain at least first and last name.' : null;
          this.errors.contactEmail = !this.validateEmail(this.contactEmail) ? 'Email must contain "@" and be in a valid format.' : null;
          this.errors.contactPhone = !this.validatePhone(this.contactPhone) ? 'Phone number must be in the format NNN-NNNN.' : null;

          return !this.errors.name && !this.errors.contactEmail && !this.errors.contactPhone;
        },
        addTrainer() {
          if (!this.validateForm()) return;

          var request = {
              Id: this.id,
              FullName: this.name,
              ContactEmail: this.contactEmail,
              ContactPhone: this.contactPhone,
              Bio: this.bio,
              TrainingType: this.trainingType,
              Rating: this.rating
          };
          let loader = this.$loading.show();
          dataServices.methods.add_trainer(request).then(() => {
              loader.hide();
              this.$router.push('/administrator');
          });
        },
        uptTrainer() {
          if (!this.validateForm()) return;

          let id = this.$route.params.id;
          dataServices.methods.get_trainer_by_id(id).then((response) => {
            const trainer = response.data;
            trainer.Id = this.id;
            trainer.FullName = this.name;
            trainer.ContactEmail = this.contactEmail;
            trainer.ContactPhone = this.contactPhone;
            trainer.Bio = this.bio;

            let loader = this.$loading.show();
            dataServices.methods.upt_trainer(id, trainer).then(() => {
              loader.hide();
              this.$router.push('/administrator');
            });
          });
        },
        handleUpdate() {
          const id = this.$route.params.id;
          dataServices.methods.get_trainer_by_id(id).then((response) => {
            this.id = response.data.id,
            this.name = response.data.fullName;
            this.contactEmail = response.data.contactEmail;
            this.contactPhone = response.data.contactPhone;
            this.bio = response.data.bio;
            this.trainingType = response.data.trainingType;
            this.rating = response.data.rating;
          });
        },
        cancel() {
          this.$router.push('/administrator');
        }
    },
    mounted() {
      if (this.$route.params.id == 0)
          this.isAdd = true;
      else {
          this.isUpt = true;
          this.handleUpdate();
      }
    }
}
</script>

<style>
  .container-lg {
    margin-top: 50px !important;
  }

  .select {
    width: 70%;
    margin-bottom: 15px !important;
  }

  .select-red {
    width: 70%;
    margin-bottom: 15px !important;
    border-radius: 1px;
    border-color: red;
  }

  .radioButtons {
    padding-top: 7px;
    margin-top: 50px;
    margin-bottom: 50px;
    justify-content: center;
    align-items: center;
    display: flex;
    width: 30%;
    border: 1px solid black;
    border-radius: 10px;
  }

  .tbl {
    width: 100%;
    border: 1px solid black;
  }

  .test {
    border: 1px solid black;
    text-align: center;
  }
</style>
