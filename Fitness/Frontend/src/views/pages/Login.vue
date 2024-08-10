<template>
  <div class="bg-light min-vh-100 d-flex flex-row align-items-center">
    <CContainer>
      <CRow class="justify-content-center">
        <CCol :md="4">
          <CCardGroup>
            <CCard class="p-4">
              <CCardBody>
                <CForm>
                  <div style="text-align:center; margin-botom:10px">
                    <img src="" />
                  </div>
                  <h2>Login</h2>
                  <p class="text-medium-emphasis">Sign In to your account</p>
                  <CInputGroup class="mb-3">
                    <CInputGroupText>
                      <CIcon icon="cil-user" />
                    </CInputGroupText>
                    <CFormInput
                      placeholder="Username"
                      autocomplete="username"
                      v-model="username"
                    />
                  </CInputGroup>
                  <CInputGroup class="mb-4">
                    <CInputGroupText>
                      <CIcon icon="cil-lock-locked" />
                    </CInputGroupText>
                    <CFormInput
                      type="password"
                      placeholder="Password"
                      autocomplete="current-password"
                      v-model="password"
                    />
                  </CInputGroup>
                  <CRow>
                    <CCol :xs="6">
                      <CButton color="primary" class="px-4" v-on:click="login"> Login </CButton>
                    </CCol>
                  </CRow>
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
import axios from "axios";

export default {
  name: 'Login',
  data() {
    return {
      username: "",
      password: "",   
    }
  },
  methods: {
    login(event) {
      
      let loader = this.$loading.show();
      dataServices.methods.login(this.username, this.password)
        .then((response) => { 
          sessionStorage.setItem('accessToken', response.data.accessToken);
          sessionStorage.setItem('refreshToken', response.data.refreshToken);
          
          const role = dataServices.methods.save_access_token_data(response.data.accessToken);
          console.log(role);
          if(role == 'Trainer') {
            this.$router.push('/trainer');
          }
          else if(role == 'Admin') {
            console.log('Rola Admin');
            this.$router.push('/administrator');
          }
          else {
            this.$router.push('/client');
          }

          loader.hide();
          
          console.log('Uspeo login');
        })
        .catch( (error) => {
          loader.hide();
          console.log('Neuspeo login!');
        });  
      
    }
    
  },

  computed: {
    
  },
  created() {
  }
}
</script>

