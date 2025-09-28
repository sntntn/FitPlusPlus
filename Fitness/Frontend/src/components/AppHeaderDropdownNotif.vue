<template>
  <div>
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
          @click="openNotification(notif.id)"
        >
          <CIcon icon="cil-envelope-closed" class="me-2 mt-1" />
          <div>
            <div :class="{ 'fw-bold': !notif.notificationRead }">
              {{ notif.title }}
            </div>
            <small class="text-muted">{{ formatDate(notif.creationDate) }}</small>
          </div>
        </CDropdownItem>
      </CDropdownMenu>
    </CDropdown>

    <CModal :visible="showModal" @close="showModal = false">
      <CModalHeader>
        <CModalTitle>{{ selectedNotification?.title }}</CModalTitle>
      </CModalHeader>
      <CModalBody v-if="selectedNotification">
        <p><strong>Datum:</strong> {{ formatDate(selectedNotification.creationDate) }}</p>
        <p><strong>Tip:</strong> {{ selectedNotification.type }}</p>
        <p><strong>Sadržaj:</strong> {{ selectedNotification.content }}</p>
        <p><strong>Email poslat:</strong> {{ selectedNotification.email ? "Da" : "Ne" }}</p>
        <p><strong>Status:</strong> 
          <span v-if="selectedNotification.notificationRead">Pročitano</span>
          <span v-else>Nepročitano</span>
        </p>
      </CModalBody>
      <CModalFooter>
        <CButton color="secondary" @click="showModal = false">Close</CButton>
      </CModalFooter>
    </CModal>
  </div>
</template>

<script>
import {
  getNotificationsByUserId,
  getNotificationById,
} from "@/services/NotificationService"

export default {
  name: "AppHeaderDropdownNotif",
  data() {
    return {
      notifications: [],
      userId: null,
      showModal: false,
      selectedNotification: null,
    }
  },
  created() {
    this.userId = this.$route.params.id
    this.fetchNotifications()
  },
  methods: {
    async fetchNotifications() {
      try {
        if (!this.userId) {
          console.warn("UserId not found in route params")
          return
        }
        const res = await getNotificationsByUserId(this.userId)
        this.notifications = res
      } catch (err) {
        console.error("Error fetching notifications:", err)
      }
    },
    async openNotification(id) {
      try {
        const notif = await getNotificationById(id)
        this.selectedNotification = notif
        this.showModal = true
      } catch (err) {
        console.error("Error fetching notification details:", err)
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
