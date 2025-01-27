<template>
  <div class="chat-container">

    <aside class="chat-list">
      <h3>Trainers</h3>
      <ul>
        <li
          v-for="trainer in trainers"
          :key="trainer.id"
          :class="{ active: trainer.id === selectedTrainer?.id }"
          @click="selectTrainer(trainer)"
        >
          <p>{{ trainer.name }}</p>
        </li>
        <li v-if="trainers.length === 0">
          <p class="warning-text1">No available trainer</p>
        </li>
      </ul>
    </aside>

    <main class="chat-main">
      <template v-if="selectedTrainer">
        <h3>Chat with {{ selectedTrainer.name }}</h3>
        <div class="chat-messages">
          <div
            v-for="message in selectedTrainer.messages"
            :key="message.id"
            :class="['message', message.sender === 'trainer' ? 'trainer' : 'client']"
          >
            <p>{{ message.text }}</p>
          </div>
        </div>

        <div class="message-input">
          <input
            type="text"
            v-model="newMessage"
            placeholder="Type your message..."
          />
          <button @click="sendMessage">Send</button>
        </div>
      </template>
      <template v-else>
        <h3 class="warning-text">No trainer selected</h3>
        <div class="chat-messages">
          <!-- TO DO dodati Loader-->
          <span class="warning-text1">Select a trainer from the chat list on the left</span>
        </div>
        <div class="message-input">
          <input
            type="text"
            v-model="newMessage"
            placeholder="Type your message..."
            :disabled="!selectedTrainer"
          />
          <button @click="sendMessage" :disabled="!selectedTrainer">Send</button>
        </div>
      </template>

    </main>
  </div>
</template>

<script>
import { getBasicInfoForClientSessions } from "../../services/ChatService";
import { getMessagesFromSession } from "../../services/ChatService";
import { sendMessageToSession } from "../../services/ChatService";


export default {
  data() {
    return {
      trainers: [],
      selectedTrainer: null,
      newMessage: "",
    };
  },
  created() {
    this.$parent.$parent.$parent.setUserData(this.$route.params.id, "client");
    this.clientId = this.$route.params.id;
    this.fetchClientSessionsBasicInfo();
  },
  mounted() {
  },
  methods: {
    selectTrainer(trainer) {
      this.selectedTrainer = trainer;
      console.log(trainer.id);
      this.fetchMessages(trainer.id);

    },
    async sendMessage() {
      if (this.newMessage.trim() !== "") {
        try{
            const response = await sendMessageToSession(
              this.selectedTrainer.id,
              this.clientId,
              this.newMessage,
              "client"
            );

            this.selectedTrainer.messages.push({
              id: Date.now(),
              text: this.newMessage,
              sender: "client",
            });
            this.newMessage = "";
          }
          catch (error) {
          console.error("Error sending message:", error);
          alert("Failed to send message. Please try again.");
        }
      }
    },
    async fetchClientSessionsBasicInfo() {
        const clientId = this.clientId;
        try {
            const basicInfo = await getBasicInfoForClientSessions(clientId);
            
            basicInfo.forEach(session => {
            console.log("TrainerId:", session.trainerId);
            console.log("ClientId:", session.clientId);
            console.log("Is Unlocked:", session.isUnlocked);
            console.log("Expiration Date:", session.expirationDate);
            console.log("--------------");

            this.trainers.push({
            id: session.trainerId,
            name: session.trainerId, // TrainerId kao name
            isUnlocked: session.isUnlocked,
            expirationDate: session.expirationDate,
            messages: [
              { id: Date.now(), text: "Session started", sender: "client" }, //Primer poruke
            ],
          });
        });
        } catch (error) {
            console.error("Failed to fetch client sessions basic info:", error);
        }
    },
    async fetchMessages(trainerId) {
      const clientId = this.clientId;
      console.log(clientId);
      try {
        const response = await getMessagesFromSession(trainerId, clientId);
        console.log(response);
        const transformedMessages = response.map((msg) => ({
          id: msg.id?.id || Date.now(), 
          text: msg.content,
          sender: msg.senderType.toLowerCase(), // "Client" u "client", "Trainer" u "trainer"
        }));

        console.log("Transformed Messages:", transformedMessages);
        this.selectedTrainer.messages = transformedMessages;

        
      } catch (error) {
        console.error("Failed to fetch messages:", error);
        this.selectedTrainer.messages = [];

      }
    },
  }
};
</script>

<style scoped>
.chat-container {
  display: flex;
  height: 100vh;
}
.chat-list {
  width: 30%;
  border-right: 1px solid #ccc;
  padding: 1rem;
  background-color: #f9f9f9;
}
.chat-list h3 {
  margin-bottom: 1rem;
}
.chat-list ul {
  list-style: none;
  padding: 0;
}
.chat-list li {
  padding: 0.5rem;
  cursor: pointer;
  border-bottom: 1px solid #ddd;
}
.chat-list li.active {
  background-color: #e0f7fa;
}
.chat-main {
  flex: 1;
  padding: 1rem;
}
.chat-main h3 {
  margin-bottom: 1rem;
}
.chat-messages {
  height: 70%;
  overflow-y: auto;
  margin-bottom: 1rem;
  border: 1px solid #ccc;
  padding: 1rem;
  background-color: #fff;
}
.message {
  margin-bottom: 0.5rem;
  padding: 0.5rem 1rem;
  border-radius: 10px;
  max-width: 60%;
  word-wrap: break-word;
}
.message.trainer {
  text-align: left;
  background-color: #f1f1f1;
  align-self: flex-start;
}
.message.client {
  text-align: right;
  background-color: #d1f7d6;
  align-self: flex-end;
}
.chat-messages {
  display: flex;
  flex-direction: column;
}
.message-input {
  display: flex;
  align-items: center;
  gap: 1rem;
}
.message-input input {
  flex: 1;
  padding: 0.5rem;
  border: 1px solid #ccc;
}
.message-input button {
  padding: 0.5rem 1rem;
  background-color: #4caf50;
  color: white;
  border: none;
  cursor: pointer;
}
.warning-text {
  color: rgb(163, 30, 30);
  font-weight: bold;
  text-align: center;
}
.warning-text1 {
  color: rgb(163, 30, 30);
  font-weight: normal;
  text-align: center;
}
</style>
