import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import store from './store'

import CoreuiVue from '@coreui/vue'
import CIcon from '@coreui/icons-vue'
import { iconsSet as icons } from '@/assets/icons'

import {LoadingPlugin} from 'vue-loading-overlay';
import 'vue-loading-overlay/dist/css/index.css';

import axios from 'axios'

axios.interceptors.response.use(response => {
    return response;
 }, error => {
   if (error.response.status === 401) {
    router.push('/login');
   }
   return error;
 });

const app = createApp(App)
app.use(store)
app.use(router)
app.use(CoreuiVue)
app.use(LoadingPlugin, {
  loader: 'dots'
});
app.provide('icons', icons)
app.component('CIcon', CIcon)

app.mount('#app')
