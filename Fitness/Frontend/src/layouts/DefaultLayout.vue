<template>
  <div>
    <AppSidebar :id="userData.id" :type="userData.type"/>
    <div class="wrapper d-flex flex-column min-vh-100 bg-light">
      <AppHeader />
      <div class="body flex-grow-1 px-3">
        <CContainer lg>
          <router-view />
        </CContainer>
      </div>
      <AppFooter />
    </div>
  </div>
</template>
<script>
import { CContainer } from '@coreui/vue'
import AppFooter from '@/components/AppFooter.vue'
import AppHeader from '@/components/AppHeader.vue'
import AppSidebar from '@/components/AppSidebar.vue'

export default {
  name: 'DefaultLayout',
  components: {
    AppFooter,
    AppHeader,
    AppSidebar,
    CContainer,
    },
    data() {
      return {
        userData: {
          id: null,
          type: null
        },
      };
    },
    methods: {
      setUserData(id,type) {
        this.userData.id = id;
        this.userData.type = type;
      }
    }
}
</script>
