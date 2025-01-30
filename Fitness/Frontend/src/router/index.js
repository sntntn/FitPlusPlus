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
        path: '/client/:id/groupTrainings',
        name: 'Client Group Trainings',
        component: () => import('@/views/pages/ClientGroupTrainings.vue')
      },

      {
        path: '/trainer/:id/groupTrainings',
        name: 'Trainer Group Trainings',
        component: () => import('@/views/pages/TrainerGroupTrainings.vue')
      },

      {
        path: '/client/:id/individualTrainings',
        name: 'Client Individual Trainings',
        component: () => import('@/views/pages/ClientIndividualTrainings.vue')
      },

      {
        path: '/trainer/:id/individualTrainings',
        name: 'Trainer Individual Trainings',
        component: () => import('@/views/pages/TrainerIndividualTrainings.vue')
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

      {
        path: '/client/:id/chat',
        name: 'Client Chat',
        component: () => import('@/views/pages/ClientChat.vue'),
      },

      {
        path: '/client/:id/videotrainings',
        name: 'Video Trainings',
        component: () => import('@/views/pages/VideoTrainings.vue')
      },

      {
        path: '/trainer/:id/videotrainings',
        name: 'Video Trainings',
        component: () => import('@/views/pages/VideoTrainings.vue')
      }

      {
        path: '/client/:id/pay-chat/:trainerId',
        name: 'PayChat',
        component: () => import('@/views/pages/PayChat.vue'),
      },
      {
        path: '/client/:id/analytics',
        name: 'ClientAnalytics',
        component: () => import('@/views/pages/ClientAnalytics.vue')
      },
      {
        path: '/trainer/:id/analytics',
        name: 'TrainerAnalytics',
        component: () => import('@/views/pages/TrainerAnalytics.vue')
      }
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
