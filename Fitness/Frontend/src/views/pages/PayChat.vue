<template>
    <div class="pay-chat">
      <div class="header">
        <h1>Pay Online Chat Mentorship for 30 Days</h1>
        <p>
          Get personalized online mentorship with a professional trainer, available for a full 30-day
          period. This mentorship will help you improve your skills, get advice, and receive feedback on
          your progress.
        </p>
      </div>
  
      <div class="why-choose">
        <h2>Why You Should Buy Online Mentorship</h2>
        <p>
          Our online mentorship provides you with access to a professional trainer who will guide you
          step-by-step. Whether you want to improve your performance, get personalized advice, or receive
          feedback on your training, this 30-day mentorship will help you achieve your goals faster and
          more effectively.
        </p>
        <p>
          With flexible communication through chat, you’ll be able to ask questions, receive tips, and
          get direct support at any time.
        </p>
      </div>
  
      <div class="payment">
        <h2>Price: $30 for 30 Days</h2>
        <p>
          Secure your 30-day chat mentorship now!
        </p>
        <button @click="redirectToPayPal" class="paypal-button">
          Pay with PayPal
        </button>
      </div>
    </div>
</template>
  
<script>
  export default {
    name: "PayChat",
    mounted() {
      this.$parent.$parent.$parent.setUserData(this.$route.params.id, "client");
    },
    methods: {
      redirectToPayPal() {
        const paypalUrl = `https://www.paypal.com/cgi-bin/webscr?cmd=_xclick&business=STAVITI_PAYPAL_MAIL&item_name=Chat Mentorship&amount=30.00&currency_code=USD`;
        
        window.location.href = paypalUrl;
      }
    }

  };

</script>
  
<style scoped>
  .pay-chat {
    font-family: Arial, sans-serif;
    max-width: 800px;
    margin: 0 auto;
    padding: 20px;
    background-color: #f4f4f4;
    border-radius: 10px;
  }
  
  .header {
    background-color: #ffffff;
    padding: 20px;
    border-radius: 8px;
    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
  }
  
  .header h1 {
    color: #333;
    font-size: 2.2em;
    font-weight: bold;
    margin-bottom: 10px;
  }
  
  .header p {
    font-size: 1.1em;
    color: #666;
    line-height: 1.6;
  }
  
  .why-choose {
    margin-top: 30px;
  }
  
  .why-choose h2 {
    font-size: 1.7em;
    color: #333;
    margin-bottom: 10px;
  }
  
  .why-choose p {
    font-size: 1.1em;
    color: #666;
    line-height: 1.6;
    margin-bottom: 20px;
  }
  
  .payment {
    background-color: #ffffff;
    padding: 20px;
    border-radius: 8px;
    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
    margin-top: 30px;
  }
  
  .payment h2 {
    font-size: 1.7em;
    color: #333;
  }
  
  .payment p {
    font-size: 1.2em;
    color: #555;
    margin-bottom: 20px;
  }
  .header h1 {
    color: #b90505;
  }
  .payment h2 {
    color: #b90505;
  }

  .paypal-button {
    background-color: #0070ba;
    color: white;
    padding: 10px 20px;
    font-size: 1.2em;
    border: none;
    border-radius: 5px;
    cursor: pointer;
  }

  .paypal-button:hover {
    background-color: #005ea6;
  }


</style>
  