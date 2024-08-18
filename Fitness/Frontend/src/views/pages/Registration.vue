<template>
  <div class="bg-light d-flex flex-row align-items-center">
    <CContainer style="margin-top: 50px;">
      <CRow class="justify-content-center">
        <CCol md="8">
          <CCardGroup>
            <CCard class="p-4">
              <CCardBody class="form">
                <CForm>
                    
                  <h1 style="margin-bottom: 20px">Register client</h1>                 
                  <CFormLabel for="firstname" style="display: block;">First Name</CFormLabel>
                  <CInputGroup style="width:70%; margin-bottom: 15px !important" class="mb-4">
                    <CFormInput id="firstname" placeholder="Please insert first name" v-model="firstname" />
                  </CInputGroup>

                  <CFormLabel for="lastname">Last Name</CFormLabel>
                  <CInputGroup style="width:70%; margin-bottom: 15px !important" class="mb-4">
                    <CFormInput id="lastname" placeholder="Please insert last name" v-model="lastname"/>
                  </CInputGroup>

                  <CFormLabel for="username">Username</CFormLabel>
                  <CInputGroup style="width:70%; margin-bottom: 15px !important" class="mb-4">
                    <CFormInput id="username" placeholder="Please insert username" v-model="username"/>
                  </CInputGroup>

                  <CFormLabel for="pw">Password</CFormLabel>
                  <CInputGroup style="width:70%; margin-bottom: 15px !important" class="mb-4">
                    <CFormInput :type="showPassword ? 'text' : 'password'" id="pw" placeholder="Please insert password" v-model="password"/>
                    <CInputGroupAppend>
                      <CButton color="light" @click="togglePasswordVisibility">
                        <CIcon name="cilToggleOn" v-if="!showPassword"/>
                        <CIcon name="cilToggleOff" v-else/>
                      </CButton>
                    </CInputGroupAppend>
                  </CInputGroup>

                  <CFormLabel for="email">Email</CFormLabel>
                  <CInputGroup style="width:70%; margin-bottom: 15px !important" class="mb-4">
                    <CFormInput id="email" placeholder="Please insert email" v-model="email"/>
                  </CInputGroup>

                  <CFormLabel for="phnum">Phone number</CFormLabel>
                  <CInputGroup style="width:70%; margin-bottom: 15px !important" class="mb-4">
                    <CFormInput id="phnum" placeholder="Please insert phone number" v-model="phonenumber"/>
                  </CInputGroup>
                  
                  <div class="d-grid d-md-block" style="text-align: center; margin-top:40px">
                    <CButton color="light" class="px-4" @click="cancel" style="margin: 0 10px">Cancel</CButton>  
                    <CButton color="dark" class="px-4" @click="register" style="margin: 0 10px">Register</CButton>
                  </div>
                </CForm>
              
              </CCardBody>
            </CCard>
          </CCardGroup>
        </CCol>
      </CRow>
    </CContainer>

    <CModal :visible="showSuccessModal" @close="closeSuccessModal">
      <CModalHeader>
        <CModalTitle>Registration Successful</CModalTitle>
      </CModalHeader>
      <CModalBody>
        Your registration was successful!
      </CModalBody>
      <CModalFooter>
        <CButton color="primary" @click="closeSuccessModal">OK</CButton>
      </CModalFooter>
    </CModal>

     <CModal :visible="showFailModal" @close="closeFailModal">
      <CModalHeader>
        <CModalTitle>Registration Failed</CModalTitle>
      </CModalHeader>
      <CModalBody>
        Your registration has failed!
      </CModalBody>
      <CModalFooter>
        <CButton color="primary" @click="closeFailModal">OK</CButton>
      </CModalFooter>
    </CModal>
  </div>
</template>

<script>
import dataServices from '../../services/data_services'

export default {
    data() {
        return {
            firstname: "",
            lastname: "",
            username: "",
            password: "",
            email: "",
            phonenumber: "",
            showPassword: false,
            showSuccessModal: false,
            showFailModal: false
        }
    },

    methods: {

      register() {
        dataServices.methods.register(this.firstname, this.lastname, this.username, this.password, this.email, this.phonenumber, "Client")
          .then( (response) => {
              if(response.status >= 200 && response.status < 300)
                this.showSuccessModal = true;
              else
                this.showFailModal = true;  
          })
          .catch( (error) => {
              console.log(error);
          });  
      },

      closeSuccessModal() {
        this.$router.push('/login');
      },

      closeFailModal() {
        this.showFailModal = false;
      },

      cancel() {
        this.$router.push('/login'); 
      },

      togglePasswordVisibility() {
        this.showPassword = !this.showPassword;
      }
    },

    created() {
    }
    
}
</script>
