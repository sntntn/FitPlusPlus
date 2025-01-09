import { h, resolveComponent } from 'vue'
import { createRouter, createWebHistory } from 'vue-router'

import DefaultLayout from '@/layouts/DefaultLayout'

const routes = [
  {
    path: '/home',
    name: 'Home',
    component: DefaultLayout,
    children: [
      {
        path: '/administrator/trainers',
        name: 'TrainersList',
        component: () => import('@/views/pages/AdministratorTrainers.vue'),
      },

      {
        path: '/administrator/trainers/:id',
        name: 'Administrator Operations - Trainers',
        component: () =>
          import(/* webpackChunkName: "dashboard" */ '@/views/pages/AdministratorTrainersCRUD.vue')
      },

      {
        path: '/administrator/clients',
        name: 'ClientsList',
        component: () => import('@/views/pages/AdministratorClients.vue')
      },

      {
        path: '/administrator/clients/:id',
        name: 'Administrator Operations - Clients',
        component: () => import('@/views/pages/AdministratorClientsCRUD.vue')
      },

      {
        path: '/trainer/:id',
        name: 'Trainer',
        component: () => import('@/views/pages/Trainer.vue'),
      },

      {
        path: '/client/:id',
        name: 'Client',
        component: () => import('@/views/pages/Client.vue'),
      },


      {
        path: '/client/:id/schedule',
        name: 'Client Schedule',
        component: () => import('@/views/pages/ClientSchedule.vue')
      },

      {
        path: '/client/:id/schedule/:trainerId',
        name: 'Book Training',
        component: () => import('@/views/pages/ClientSchedule.vue')
      },

      {
        path: '/trainer/:id/schedule',
        name: 'Trainer Schedule',
        component: () => import('@/views/pages/TrainerSchedule.vue')
      },

      {
        path: '/payment-success',
        name: 'Payment Success',
        component: () => import('@/views/pages/PaymentSuccess.vue')
      },

      {
        path: '/payment-cancel',
        name: 'Payment Cancel',
        component: () => import('@/views/pages/PaymentCancel.vue')
      },

      {
        path: '/trainer/:id/chat',
        name: 'Trainer Chat',
        component: () => import('@/views/pages/TrainerChat.vue')
      },

      // {
      //   path: '/client/:id/chat',
      //   name: 'ClientChat',
      //   component: () => import('@/views/pages/ClientChat.vue'),
      // },

    ],
  },
  {
    path: '/login',
    name: 'OurLogin',
    component: () => import('@/views/pages/Login.vue')
  },
  {
    path: '/registration',
    name: 'Registration',
    component: () => import('@/views/pages/Registration.vue')
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

/* router.beforeEach(async(to,from)=>{
   var token = sessionStorage.getItem('accessToken');
    if (to.path == '/') {
     router.push('/trainer');
     return false;
    }
   return true;
 })
*/

router.beforeEach(async (to, from) => {

  var token = sessionStorage.getItem('accessToken');
  if ((token && token != 'null') || to.path == '/login' || to.path == '/registration') {
    
    return true;
  }
  else {
    router.push('/login');
    return false;
  }

})

export default router
