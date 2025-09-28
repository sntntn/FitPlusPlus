<template>
  <CDropdown variant="nav-item" placement="bottom-end">
    <CDropdownToggle class="py-0 d-flex align-items-center" :caret="false">
      <CIcon icon="cil-bell" size="lg" class="mx-2" />
    </CDropdownToggle>
    <CDropdownMenu class="pt-0" style="min-width: 250px;">
      <CDropdownItem v-if="notifications.length === 0" disabled>
        No notifications
      </CDropdownItem>
      <CDropdownItem v-for="(notif, index) in notifications" :key="index">
        <CIcon icon="cil-envelope-closed" class="me-2" />
        {{ notif.message }}
      </CDropdownItem>
    </CDropdownMenu>
  </CDropdown>
</template>

<script>
import axios from "axios"

export default {
  name: "AppHeaderDropdownNotif",
  data() {
    return {
      notifications: [],
    }
  },
  mounted() {
    this.fetchNotifications()
  },
  methods: {
    async fetchNotifications() {
      try {
        const res = await axios.get("http://localhost:5000/api/notifications") 
        this.notifications = res.data // očekujem niz objekata tipa { message: "..." }
      } catch (err) {
        console.error("Error fetching notifications:", err)
      }
    }
  }
}
</script>
