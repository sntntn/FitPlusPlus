<template>
  <CDropdown variant="nav-item" placement="bottom-end">
    <CDropdownToggle class="py-0 d-flex align-items-center" :caret="false">
      <CIcon icon="cil-bell" size="lg" class="mx-2" />
    </CDropdownToggle>
    <CDropdownMenu class="pt-0" style="min-width: 300px;">
      <CDropdownItem v-if="notifications.length === 0" disabled>
        No notifications
      </CDropdownItem>

      <CDropdownItem
        v-for="(notif, index) in notifications"
        :key="index"
        class="d-flex align-items-start"
      >
        <CIcon icon="cil-envelope-closed" class="me-2 mt-1" />
        <div>
          <div :class="{'fw-bold': !notif.notificationRead}">
            {{ notif.title }}
          </div>
          <small class="text-muted">{{ formatDate(notif.creationDate) }}</small>
        </div>
      </CDropdownItem>
    </CDropdownMenu>
  </CDropdown>
</template>

<script>
import { getNotificationsByUserId } from "@/services/NotificationService"

export default {
  name: "AppHeaderDropdownNotif",
  data() {
    return {
      notifications: [],
      userId: null,
    }
  },
  created() {
    this.userId = this.$route.params.id
    console.log("Ovde stampamo userID:", this.userId)
    this.fetchNotifications()
  },
  methods: {
    async fetchNotifications() {
      try {
        console.log("userId ovde:", this.userId)

        if (!this.userId) {
          console.warn("UserId not found in route params")
          return
        }

        const res = await getNotificationsByUserId(this.userId)
        console.log("ovo je res:", res)
        this.notifications = res
      } catch (err) {
        console.error("Error fetching notifications:", err)
      }
    },
    formatDate(dateStr) {
      const date = new Date(dateStr)
      return date.toLocaleString("sr-RS", {
        day: "2-digit",
        month: "2-digit",
        year: "numeric",
        hour: "2-digit",
        minute: "2-digit",
      })
    },
  },
}
</script>
