<template>
  <div>
    <h1>Payment Successful!</h1>
    <p>Your payment has been processed successfully. We are finalizing your booking...</p>
  </div>
</template>

<script>
import axios from 'axios';
import dataServices from '../../services/data_services';


export default {
  async created() {
    // Uzmite token sa URL-a
    const urlParams = new URLSearchParams(window.location.search);
    const token = urlParams.get('token');
    const paymentId = urlParams.get('paymentId');


    if (token && paymentId) {
      console.log("Token:",token);
      console.log("PaymentId:",paymentId);
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

        this.$router.go(-2);
        
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