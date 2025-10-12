<template>
  <div class="payment-success">
    <div class="message-container">
      <h1>Payment Successful!</h1>
      <p>Your payment has been processed successfully. We are finalizing your booking...</p>
      <div class="loader"></div>
    </div>
  </div>
</template>

<script>
import dataServices from "@/services/data_services";
import { createChatSession, extendChatSession } from "@/services/ChatService";
import { createIndividualReservation, cancelClientIndividualReservation, cancelTrainerIndividualReservation } from "@/services/ReservationService";

export default {
  created() {
    const urlParams = new URLSearchParams(window.location.search);
    const token = urlParams.get("token");
    const paymentId = urlParams.get("paymentId");

    if (!token || !paymentId) {
      alert("Payment details are missing. Please try again.");
      return;
    }

    const request = { token, paymentId };

    dataServices.methods
      .capture_payment(request)
      .then(() => {
        this.handlePostPaymentActions();
      })
      .catch((error) => {
        console.error("Error capturing payment:", error);
        alert("Failed to finalize your booking. Please try again.");
      });
  },

  methods: {
    handlePostPaymentActions() {
      const bookData = sessionStorage.getItem("bookData");
      const cancelData = sessionStorage.getItem("cancelData");
      const chatData = sessionStorage.getItem("chatData");
      const extendData = sessionStorage.getItem("extendData");
    
      if (bookData) {
        this.processBooking(JSON.parse(bookData));
        sessionStorage.removeItem("bookData");
      } else if (chatData) {
        this.processChat(JSON.parse(chatData));
        sessionStorage.removeItem("chatData");
      } else if (extendData) {
        this.processExtend(JSON.parse(extendData));
        sessionStorage.removeItem("extendData");
      } else if(cancelData) {
        this.processCancel(JSON.parse(cancelData));
        sessionStorage.removeItem("cancelData");
      } else {
        console.log("No data found in session.");
      }
    },

    processBooking(bookData) {
      createIndividualReservation(bookData)
        .then((response) => {
          if (response.status === 201) {
            alert("Training booked successfully!");
          } else {
            alert(`Booking failed. Status: ${response.status}`);
          }
        })
        .catch((error) => {
          console.error("Booking error:", error);
          alert("An error occurred while booking the training.");
        })
        .then(() => {
          this.$router.push(`/client/${bookData.clientId}/individualTrainings`);
        });
    },

    processCancel(cancelData) {
      cancelTrainerIndividualReservation(cancelData.reservationId)
        .then((response) => {
          if (response.status === 200) {
            alert("Training cancelled successfully!");
          } else {
            alert(`Cancellation failed. Status: ${response.status}`);
          }
        })
        .catch((error) => {
          console.error("Cancellation error:", error);
          alert("An error occurred while cancelling the training.");
        })
        .then(() => {
          this.$router.push(`/trainer/${cancelData.trainerId}/individualTrainings`);
        });;
    },

    processChat(chatData) {
      console.log("Creating chat session...");

      createChatSession(chatData.trainerId, chatData.clientId)
        .then((response) => {
          if (response.status === 200) {
            alert("Chat session created successfully!");
          } else {
            alert("Failed to create chat session.");
          }
        })
        .catch((error) => {
          console.error("Chat creation error:", error);
          alert("An error occurred while creating the chat session.");
        })
        .then(() => {
          this.$router.push(`/client/${chatData.clientId}/chat`);
        });
    },

    processExtend(extendData) {
      console.log("Extending chat session...");

      extendChatSession(extendData.trainerId, extendData.clientId)
        .then((response) => {
          if (response.status === 204) {
            alert("Chat session extended successfully!");
          } else {
            alert("Failed to extend chat session.");
          }
        })
        .catch((error) => {
          console.error("Chat extension error:", error);
          alert("An error occurred while extending the chat session.");
        })
        .then(() => {
          this.$router.push(`/client/${extendData.clientId}/chat`);
        });
    },
  },
};
</script>

<style scoped>
  .payment-success {
    display: flex;
    justify-content: center;
    align-items: center;
    height: 100vh;
    background: linear-gradient(135deg, #e9f7ef, #d4edda);
  }

  .message-container {
    text-align: center;
    background: white;
    padding: 2rem;
    border-radius: 10px;
    box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
  }

  h1 {
    color: #28a745;
    font-size: 2rem;
    margin-bottom: 1rem;
  }

  p {
    color: #6c757d;
    font-size: 1.2rem;
  }

  .loader {
    margin-top: 1.5rem;
    width: 30px;
    height: 30px;
    border: 4px solid #f3f3f3;
    border-radius: 50%;
    border-top: 4px solid #28a745;
    animation: spin 2s linear infinite;
  }

  @keyframes spin {
    0% {
      transform: rotate(0deg);
    }

    100% {
      transform: rotate(360deg);
    }
  }
</style>
