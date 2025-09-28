// navigacija u side meniju
export function generateTrainerNav(id) {
  return [
    {
      component: 'CNavItem',
      name: 'Home Page',
      to: `/trainer/${id}`,
      icon: 'cil-user'
    },
    {
      component: 'CNavItem',
      name: 'Schedule',
      to: `/trainer/${id}/schedule`,
      icon: 'cil-calendar'
    },
    {
      component: 'CNavItem',
      name: 'Chat',
      to: `/trainer/${id}/chat`,
      icon: 'cil-speech',
    },
  ];
}

export function generateClientNav(id) {
  return [
    {
      component: 'CNavItem',
      name: 'Home Page',
      to: `/client/${id}`,
      icon: 'cil-user'
    },
    {
      component: 'CNavItem',
      name: 'Schedule',
      to: `/client/${id}/schedule`,
      icon: 'cil-calendar'
    },
    {
      component: 'CNavItem',
      name: 'Group training',
      to: `/client/${id}/groupTrainings`,
      icon: 'cil-people',
    },
    {
      component: 'CNavItem',
      name: 'Individual training',
      to: `/client/${id}/individualTrainings`,
      icon: 'cil-user-follow',
    },
    {
      component: 'CNavItem',
      name: 'Chat',
      to: `/client/${id}/chat`,
      icon: 'cil-speech'
    },
  ];
}

export function generateAdministratorNav() {
  return [
    {
      component: 'CNavItem',
      name: 'Trainers',
      to: `/administrator/trainers`,
      icon: 'cil-user'
    },
    {
      component: 'CNavItem',
      name: 'Clients',
      to: `/administrator/clients`,
      icon: 'cil-user'
    }
  ]
}
