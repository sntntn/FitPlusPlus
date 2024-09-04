<template>
  <div class="bg-light d-flex flex-row align-items-center">
    <CContainer style="margin-top: 50px;">
      <CRow class="justify-content-center">
        <CCol md="8">
          <CCardGroup>
            <CCard class="p-4">
              <CCardBody class="form">
                <CForm>
                  <h1 style="margin-bottom: 10px">{{isAdd ? 'Add client':'Update client'}}</h1>

                  <CFormLabel for="name" style="display: block;">Name</CFormLabel>
                  <CInputGroup style="width:70%; margin-bottom: 10px !important">
                    <CFormInput id="name" v-model="name" :disabled="!isAdd" placeholder="Please insert client name" />
                  </CInputGroup>
                  <span v-if="errors.name" style="color: red">{{ errors.name }}</span>

                  <CFormLabel for="surname" style="display: block;">Surname</CFormLabel>
                  <CInputGroup style="width:70%; margin-bottom: 10px !important">
                    <CFormInput id="surname" v-model="surname" :disabled="!isAdd" placeholder="Please insert client surname" />
                  </CInputGroup>
                  <span v-if="errors.surname" style="color: red">{{ errors.surname }}</span>


                  <CFormLabel for="contactEmail" style="display: block;">Contact Email</CFormLabel>
                  <CInputGroup style="width:70%; margin-bottom: 10px !important">
                    <CFormInput id="contactEmail" :disabled="!isAdd" placeholder="Please insert contact email" v-model="contactEmail" />
                  </CInputGroup>
                  <span v-if="errors.contactEmail" style="color: red">{{ errors.contactEmail }}</span>


                  

                  <div class="d-grid d-md-block" style="text-align: center; margin-top:20px">
                    <CButton color="light" class="px-4" v-on:click="cancel" style="margin: 0 10px">Cancel</CButton>
                    <CButton color="dark" class="px-4" v-on:click="isAdd ? addClient() : uptClient()" style="margin: 0 10px">{{isAdd? 'Add':'Update'}}</CButton>
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
    name: "ClientCrud",
    data() {
        return {
            isUpt: false,
            isAdd: false,
            id:"",
            name: "",
            surname: "",
            contactEmail: "",
            errors: {
              name: null,
              contactEmail: null,
              surname: null
            }
        };
    },
    methods: {
        validateFullName(name) {
          return name.split(' ').length >= 2;
        },
        validateForm() {
          this.errors.name = !this.validateFullName(this.name + ' ' + this.surname) ? 'Name must contain at least first and last name.' : null;

          return !this.errors.name && !this.errors.contactPhone;
        },
        addClient() {
          if (!this.validateForm()) return;

          var request = {
              Id: this.id,
              Name: this.name,
              Surname: this.surname,
              Email: this.contactEmail,
          };

          console.log(request);
          let loader = this.$loading.show();
          dataServices.methods.add_client(request).then(() => {
                dataServices.methods.register(this.name, this.surname, this.name, this.surname + '123RS2', this.contactEmail, "", 'Client')
                    .then(() => {
                        loader.hide();
                        this.$router.push('/administrator/clients');
                    })
          });
        },
        uptClient() {
          if (!this.validateForm()) return;

          let id = this.$route.params.id;
          dataServices.methods.get_client_by_id(id).then((response) => {
            const client = response.data;
            client.Id = this.id;
            client.Name = this.name;
            client.Surname = this.surname;
            client.Email = this.contactEmail;
            

            let loader = this.$loading.show();
            dataServices.methods.upt_client(id, client).then(() => {
              loader.hide();
              this.$router.push('/administrator/clients');
            });
          });
        },
        handleUpdate() {
          const id = this.$route.params.id;
          dataServices.methods.get_client_by_id(id).then((response) => {
            this.id = response.data.id,
            this.name = response.data.name;
            this.surname = response.data.surname;
            this.contactEmail = response.data.email;
          });
        },
        cancel() {
          this.$router.push('/administrator/clients');
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
