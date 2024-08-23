<template>
  <CModal :visible="modalData.isvisible" @close="onClose">
    <CModalHeader>
      <CModalTitle>{{ modalData.title }} - {{ modalData.trainerName }}</CModalTitle>
    </CModalHeader>
    <CModalBody>
      <div v-if="modalData.reviews && modalData.reviews.length">
        <div v-for="(review, index) in modalData.reviews" :key="index" class="review-item">
          <div class="review-content">
            <p class="review-comment">{{ review.comment }}</p>
            <p class="review-rating">Rating: {{ review.rating }} / 10</p>
          </div>
          <div class="review-actions">
            <CButton color="light" class="px-3" @click="startUpdateReview(review)">
              <CIcon icon="cil-pencil" />
            </CButton>
            <CButton color="light" class="px-3" style="margin-left:10px" @click="onDelete(review.id)">
              <CIcon icon="cil-trash" />
            </CButton>
          </div>
        </div>
      </div>
      <div v-else>
        <p>No reviews available.</p>
      </div>

      <div v-if="isUpdateMode">
        <h5>Update Review</h5>
        <CFormGroup>
          <CInputGroup class="mb-3">
            <CFormInput v-model="updateReview.comment" placeholder="Comment" />
          </CInputGroup>
          <CInputGroup class="mb-3">
            <CFormInput type="number" v-model.number="updateReview.rating" placeholder="Rating" min="0" max="10" step="0.1" />
          </CInputGroup>
          <CButton color="dark" style="margin-bottom:10px" @click="updateReviewHandler">Save Changes</CButton>
          <CButton color="dark" style="margin-bottom:10px; margin-left:10px" @click="cancelUpdate">Cancel</CButton>
        </CFormGroup>
      </div>

      <div>
        <CButton color="dark" style="margin-bottom:10px" @click="toggleAddReview">Add Review</CButton>
        <div v-if="showAddReviewForm">
          <CFormGroup>
            <CInputGroup class="mb-3">
              <CFormInput v-model="newReview.comment" placeholder="Comment" />
            </CInputGroup>
            <CInputGroup class="mb-3">
              <CFormInput type="number" v-model.number="newReview.rating" placeholder="Rating" min="0" max="10" step="1" />
            </CInputGroup>
            <CButton color="dark" @click="addReview(modalData.trainerId)">Submit</CButton>
            <CButton color="dark" style="margin-left:10px" @click="cancelAddReview">Cancel</CButton>
          </CFormGroup>
        </div>
      </div>
    </CModalBody>
    <CModalFooter>
      <CButton color="light" @click="onClose">Close</CButton>
    </CModalFooter>
  </CModal>
  <GenericModal :modalData="genericModalData" />
</template>

<script>
  import dataServices from '@/services/data_services';
  import GenericModal from '@/components/GenericModal.vue';

  export default {
    name: 'ClientModal',
    components: {
      GenericModal,
    },
    props: ['modalData'],
    data() {
      return {
        deleteReviewId: null,
        deleteConfirmVisible: false,
        isUpdateMode: false,
        showAddReviewForm: false,
        updateReview: {
          id: '',
          trainerId: '',
          clientId: '',
          comment: '',
          rating: 0
        },
        newReview: {
          trainerId: '',
          clientId: '',
          comment: '',
          rating: 0
        },
        genericModalData: {
          isVisible: false,
          title: "Confirm delete",
          body: "Are you sure that you want to delete this trainer review?",
          resolve: null,
          reject: null
        }
      };
    },
    methods: {
      onClose() {
        this.modalData.isvisible = false;
        if (this.modalData.resolve) {
          this.modalData.resolve();
          this.modalData.resolve = null;
          this.modalData.reject = null;
        }
      },
      startUpdateReview(review) {
        if (review.clientId !== this.$route.params.id) {
          alert("You can only update your own reviews.");
          return;
        }
        this.isUpdateMode = true;
        this.updateReview = { ...review };
      },
      updateReviewHandler() {
        this.updateReview.trainerId = this.modalData.trainerId;
        this.updateReview.clientId = this.$route.params.id;
        console.log(this.updateReview);
        dataServices.methods.update_review(this.updateReview).then(() => {
          this.fetchReviews();
          this.updateReview = { id: '', trainerId: '', clientId: '', comment: '', rating: 0 };
          this.isUpdateMode = false;
          this.$emit('review-updated');
        });
      },
      cancelUpdate() {
        this.isUpdateMode = false;
        this.updateReview = { id: '', trainerId: '', clientId: '', comment: '', rating: 0 };
      },
      deletceReview(id) {
        dataServices.methods.delete_review(id).then(() => {
          this.fetchReviews();
          this.$emit('review-deleted');
          this.modalData.isvisible = true;
        });
      },
      onDelete(id) {
        const review = this.modalData.reviews.find(review => review.id === id);
        if(review.clientId !== this.$route.params.id){
          alert("You can only delete your own reviews.");
          return;
        }

        this.deleteReviewId = id;
        this.openModal().then((result) => {
          if (result) {
            this.deleteReview(this.deleteReviewId);
          }
          this.genericModalData.isVisible = false;
          this.genericModalData.resolve = null;
          this.genericModalData.reject = null;
        });
      },
      openModal() {
        return new Promise((resolve, reject) => {
          this.genericModalData.isVisible = true;
          this.genericModalData.resolve = resolve;
          this.genericModalData.reject = reject;
        });
      },
      toggleAddReview() {
        this.showAddReviewForm = !this.showAddReviewForm;
      },
      addReview(trainerId) {
        const clientId = this.$route.params.id;
        dataServices.methods.get_schedule_trainers_by_client_id(clientId).
        then((response) => {
          const trainerIds = response.data;
          const trainerExists = trainerIds.includes(trainerId);

          if (!trainerExists) {
            alert("You cannot add a review for a trainer that is not in your schedule.");
            return;
          }
          this.newReview.trainerId = trainerId;
          this.newReview.clientId = clientId;
        
          dataServices.methods.add_review(this.newReview).then(() => {
            this.fetchReviews();
            this.newReview = { comment: '', rating: 0 };
            this.showAddReviewForm = false;
            this.$emit('review-added');
          }).catch(error => {
            console.error("Error adding review: ", error);
            alert("An error occurred while adding the review.");
          });
        }).catch(error => {
          console.error("Error fetching client trainers: ", error);
          alert("An error occurred while checking the trainer.");
        });
      },
      cancelAddReview() {
        this.showAddReviewForm = false;
        this.newReview = { comment: '', rating: 0 };
      },
      fetchReviews() {
        dataServices.methods.get_reviews(this.modalData.trainerId).then((reviews) => {
          this.modalData.reviews = reviews.data;
        });
      }
    }
  }
</script>

<style scoped>
  .review-item {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 5px;
    padding: 5px;
    border-radius: 8px;
    background-color: #dbd5d5;
  }

  .review-content {
    flex: 1;
    padding: 5px;
    border-radius: 8px;
    background-color: #ffffff;
    margin-right: 10px;
  }

  .review-comment {
    margin: 0;
    font-size: 12px;
    line-height: 1.5;
  }

  .review-rating {
    margin: 5px 0 0;
    font-size: 12px;
    color: #666;
  }

  .review-actions {
    display: flex;
    align-items: center;
  }
</style>
