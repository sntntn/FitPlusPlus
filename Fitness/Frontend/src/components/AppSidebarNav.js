import { defineComponent, h, onMounted, ref, resolveComponent, watch } from 'vue'
import { RouterLink, useRoute } from 'vue-router'

import {
  CBadge,
  CSidebarNav,
  CNavItem,
  CNavGroup,
  CNavTitle,
} from '@coreui/vue'
import { generateTrainerNav, generateClientNav, generateAdministratorNav } from '@/_nav.js'

const normalizePath = (path) =>
  decodeURI(path)
    .replace(/#.*$/, '')
    .replace(/(index)?\.(html)$/, '')

const isActiveLink = (route, link) => {
  if (link === undefined) {
    return false
  }

  if (route.hash === link) {
    return true
  }

  const currentPath = normalizePath(route.path)
  const targetPath = normalizePath(link)

  return currentPath === targetPath
}

const isActiveItem = (route, item) => {
  if (isActiveLink(route, item.to)) {
    return true
  }

  if (item.items) {
    return item.items.some((child) => isActiveItem(route, child))
  }

  return false
}

const AppSidebarNav = defineComponent({
  name: 'AppSidebarNav',
  components: {
    CNavItem,
    CNavGroup,
    CNavTitle,
  },
  props: {
    id: {
      type: String,
      required: true
    },
    type: {
      type: String,
      required: true
    }
  },
  setup(props) {
    const route = useRoute()
    const nav = ref([])
    const updateNav = () => {
      console.log("updateNav called");
      console.log('props.type:', props.type);
      console.trace();
      debugger;

      if (props.type === 'trainer') {
        nav.value = generateTrainerNav(props.id)
      } else if (props.type === 'client') {
        nav.value = generateClientNav(props.id)
      } else {
        nav.value = generateAdministratorNav()
      }
    }

    watch(() => [props.id, props.type], updateNav, { immediate: true })


    const renderItem = (item) => {
      if (item.items) {
        return h(
          CNavGroup,
          {
            visible: item.items.some((child) => isActiveItem(route, child)),
          },
          {
            togglerContent: () => [
              h(resolveComponent('CIcon'), {
                customClassName: 'nav-icon text-danger',
                name: item.icon,
              }),
              item.name,
            ],
            default: () => item.items.map((child) => renderItem(child)),
          },
        )
      }

      return item.to
        ? h(
          RouterLink,
          {
            to: item.to,
            custom: true,
          },
          {
            default: (props) =>
              h(
                resolveComponent(item.component),
                {
                  active: props.isActive,
                  href: props.href,
                  onClick: () => props.navigate(),
                },
                {
                  default: () => [
                    item.icon &&
                    h(resolveComponent('CIcon'), {
                      customClassName: 'nav-icon text-danger',
                      name: item.icon,
                    }),
                    item.name,
                    item.badge &&
                    h(
                      CBadge,
                      {
                        class: 'ms-auto',
                        color: item.badge.color,
                      },
                      {
                        default: () => item.badge.text,
                      },
                    ),
                  ],
                },
              ),
          },
        )
        : h(
          resolveComponent(item.component),
          {},
          {
            default: () => item.name,
          },
        )
    }

    return () =>
      h(
        CSidebarNav,
        {},
        {
          default: () => nav.value.map((item) => renderItem(item)),
        },
      )
  }
})

export { AppSidebarNav }
