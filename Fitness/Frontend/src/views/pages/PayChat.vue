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
        <button @click="payChatSession" class="paypal-button">
          Pay with PayPal
        </button>
        <!-- TODO: Resolve chat extension logic -->
      </div>
    </div>
</template>
  
<script>
import data_services from "@/services/data_services";
import { 
  createChatSession,
  extendChatSession,
  getChatSession
} from "@/services/ChatService";

export default {
  name: "PayChat",
  mounted() {
    this.$parent.$parent.$parent.setUserData(this.$route.params.id, "client");
    const pathParts = this.$route.path.split("/"); 
    this.clientId = pathParts[2];
    this.trainerId = pathParts[4];
  },
  methods: {    
    initiatePayment(chatData, price) {
      return data_services.methods
        .get_trainer_by_id(chatData.trainerId)
        .then((trainerResponse) => {
          const request = {
            id: "",
            userId: chatData.clientId,
            amount: price,
            currency: "USD",
            trainerPayPalEmail: trainerResponse.data.contactEmail,
          };

          return data_services.methods.create_payment(request);
        })
        .then((response) => {
          const paymentId = response.data.payment.id;
          const approvalUrl = response.data.paymentLink;

          console.log("Chat payment initiated with ID:", paymentId);

          window.location.href = approvalUrl;
        })
        .catch((error) => {
          console.error("Error initiating chat payment:", error);
          alert("Failed to initiate payment.");
          return false;
        });
    },

    payChatSession() {
      if (!this.trainerId || !this.clientId) {
        alert("Trainer ID or Client ID is missing.");
        console.log("Client ID:", this.clientId, "Trainer ID:", this.trainerId);
        return;
      }

      const chatData = {
        clientId: this.clientId,
        trainerId: this.trainerId,
      };
      const price = 30; // TODO: Consider making dynamic

      getChatSession(this.trainerId, this.clientId)
        .then((response) => {
          let storageKey;
          console.log(response);
          if (response.status === 200) {
            console.log("Chat session already exists! Extending chat session");
            storageKey = "extendData";
            // TODO: Optionally ask user with confirm() before extending
          } else if (response.status === 404) {
            console.log("Chat session not found. Creating a new one...");
            storageKey = "chatData";
          }

          sessionStorage.setItem(storageKey, JSON.stringify(chatData));

          return this.initiatePayment(chatData, price);
        })
        .then(() => {
          console.log("Payment process initiated successfully.");
        })
        .catch((error) => {
          console.error("Error processing chat session:", error);
          alert("Failed to process chat session. Please try again.");
        });
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

  .apply-button {
    background-color: #28a745; /* Zelena boja */
    color: white;
    padding: 10px 20px;
    font-size: 1.2em;
    border: none;
    border-radius: 5px;
    cursor: pointer;
    margin-top: 10px;
  }

  .apply-button:hover {
    background-color: #218838;
  }

</style>
  