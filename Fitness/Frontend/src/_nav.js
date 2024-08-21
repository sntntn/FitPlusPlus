// navigacija u side meniju
export function generateTrainerNav(id) {
  return [
    {
      component: 'CNavItem',
      name: 'Home Page',
      to: `/trainer/${id}`,
      icon: 'cil-menu'
    },
    {
      component: 'CNavItem',
      name: 'Schedule',
      to: `/trainer/${id}/schedule`,
      icon: 'cil-calendar'
    }
  ];
}

export function generateClientNav(id) {
  return [
    {
      component: 'CNavItem',
      name: 'Home Page',
      to: `/client/${id}`,
      icon: 'cil-menu'
    },
    {
      component: 'CNavItem',
      name: 'Schedule',
      to: `/client/${id}/schedule`,
      icon: 'cil-calendar'
    }
  ];
}
