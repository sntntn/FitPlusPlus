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
  import axios from 'axios';
  import dataServices from '../../services/data_services';

  export default {
    async created() {
      const urlParams = new URLSearchParams(window.location.search);
      const token = urlParams.get('token');
      const paymentId = urlParams.get('paymentId');

      if (token && paymentId) {
        try {
          const request = {
            token: token,
            paymentId: paymentId
          };

          await dataServices.methods.capture_payment(request);

          const bookingData = JSON.parse(sessionStorage.getItem('bookingData'));
          try {
            console.log(bookingData);
            const response = await dataServices.methods.booking(bookingData);
          } catch (error) {
            console.error('Error during booking:', error);
            alert('Failed to book the training.');
          }
          setTimeout(() => {
            this.$router.push(`/client/${bookingData.clientId}/schedule/${bookingData.trainerId}`);
          }, 2000);

        } catch (error) {
          console.error('Error capturing payment:', error);
          alert('Failed to finalize your booking. Please try again.');
        }
      } else {
        alert('Payment details are missing. Please try again.');
      }
    }
  }
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
