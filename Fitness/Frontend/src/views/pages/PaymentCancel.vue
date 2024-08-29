<template>
  <div class="payment-cancel">
    <div class="message-container">
      <h1>Payment Canceled</h1>
      <p>Your payment was canceled. You will be redirected back shortly.</p>
      <div class="loader"></div>
    </div>
  </div>
</template>

<script>
  export default {
    async created() {
      setTimeout(() => {
        this.$router.go(-3);
      }, 2000);
    }
  }
</script>

<style scoped>
  .payment-cancel {
    display: flex;
    justify-content: center;
    align-items: center;
    height: 100vh;
    background: linear-gradient(135deg, #f8f9fa, #e9ecef);
  }

  .message-container {
    text-align: center;
    background: white;
    padding: 2rem;
    border-radius: 10px;
    box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
  }

  h1 {
    color: #dc3545;
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
    border-top: 4px solid #dc3545;
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
