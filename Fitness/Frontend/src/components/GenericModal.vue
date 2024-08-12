<template>
  <CModal :visible="modalData.isVisible" @close="onClose">
    <CModalHeader>
      <CModalTitle>{{ this.modalData.title }}</CModalTitle>
    </CModalHeader>
    <CModalBody>{{ this.modalData.body }}</CModalBody>
    <CModalFooter>
      <CButton color="light" @click="onClose">No</CButton>
      <CButton color="dark" @click="onDelete">
        Yes
      </CButton>
    </CModalFooter>
  </CModal>
</template>

<script>export default {
    name: 'Test',
    props: ['modalData'],
    methods: {
        onClose() {
            this.modalData.resolve(false);
        },

        onDelete() {
            this.modalData.resolve(true);
        }
    }
}</script>
