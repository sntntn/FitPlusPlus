import { h, resolveComponent } from 'vue'
import { createRouter, createWebHistory } from 'vue-router'

import DefaultLayout from '@/layouts/DefaultLayout'

const routes = [
  {
    path: '/home',
    name: 'Home',
    component: DefaultLayout,
    children: [
      
    ],
  },
  {
    path: '/login',
    name: 'OurLogin',
    component: () => import('@/views/pages/Login.vue')
  },
  {
    path: '/pages',
    redirect: '/pages/404',
    name: 'Pages',
    component: {
      render() {
        return h(resolveComponent('router-view'))
      },
    },
    children: [
      {
        path: 'login',
        name: 'Login',
        component: () => import('@/views/pages/Login'),
      },
    ],
  },
]

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes,
  scrollBehavior() {
    // always scroll to top
    return { top: 0 }
  },
})

router.beforeEach(async (to, from) => {

  var token = sessionStorage.getItem('accessToken');
  if ((token && token != 'null') || to.path == '/login') {
    if(to.path == '/') {
      return false;
    }
    else
      return true;
  }
  else {
    router.push('/login');
    return false;
  }

})

export default router
