<template>
  <div class="chat-container">

    <aside class="chat-list">
      <h3>Clients</h3>
      <ul>
        <li
          v-for="client in clients"
          :key="client.id"
          :class="{ active: client.id === selectedClient?.id }"
          @click="selectClient(client)"
        >
          <p>{{ client.name }}</p>
        </li>
        <li v-if="clients.length === 0">
          <p class="warning-text1">No available clients</p>
        </li>
      </ul>
    </aside>

    <main class="chat-main">
      <template v-if="selectedClient">
        <h3>Chat with {{ selectedClient.name }}</h3>
        <div class="chat-messages">
          <div
            v-for="message in selectedClient.messages"
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
        <h3 class="warning-text">No client selected</h3>
        <div class="chat-messages">
          <!-- TO DO dodati Loader-->
          <span class="warning-text1">Select a client from the chat list on the left</span>

        </div>

        <div class="message-input">
          <input
            type="text"
            v-model="newMessage"
            placeholder="Type your message..."
            :disabled="!selectedClient"
          />
          <button @click="sendMessage" :disabled="!selectedClient">Send</button>
        </div>
      </template>

    </main>
  </div>
</template>

<script>
import { getBasicInfoForTrainerSessions } from "../../services/ChatService";
import { getMessagesFromSession } from "../../services/ChatService";


export default {
  data() {
    return {
      clients: [],
      selectedClient: null,
      newMessage: "",
    };
  },
  created() {
    this.$parent.$parent.$parent.setUserData(this.$route.params.id, "trainer");
    this.trainerId = this.$route.params.id;
    this.fetchTrainerSessionsBasicInfo();
  },
  mounted() {
   
  },
  methods: {
    selectClient(client) {
      this.selectedClient = client;
      console.log(client.id);
      this.fetchMessages(client.id);
    },
    sendMessage() {
      if (this.newMessage.trim() !== "") {
        this.selectedClient.messages.push({
          id: Date.now(),
          text: this.newMessage,
          sender: "trainer",
        });
        this.newMessage = "";
      }
    },
    async fetchTrainerSessionsBasicInfo() {
        const trainerId = this.trainerId;

        try {
            const basicInfo = await getBasicInfoForTrainerSessions(trainerId);
            
            basicInfo.forEach(session => {
            console.log("TrainerId:", session.trainerId);
            console.log("ClientId:", session.clientId);
            console.log("Is Unlocked:", session.isUnlocked);
            console.log("Expiration Date:", session.expirationDate);
            console.log("--------------");

            this.clients.push({
            id: session.clientId,
            name: session.clientId, // ClientId kao name
            isUnlocked: session.isUnlocked,
            expirationDate: session.expirationDate,
            messages: [
              { id: Date.now(), text: "Session started", sender: "trainer" }, // Primer poruke
            ],
          });
        });
        } catch (error) {
            console.error("Failed to fetch trainer sessions basic info:", error);
        }
    },
    async fetchMessages(clientId) {
      const trainerId = this.trainerId;
      console.log(trainerId);
      try {
        const response = await getMessagesFromSession(trainerId, clientId);
        console.log(response);
        const transformedMessages = response.map((msg) => ({
          id: msg.id?.id || Date.now(), 
          text: msg.content,
          sender: msg.senderType.toLowerCase(), // "Trainer" u "trainer"
        }));

        console.log("Transformed Messages:", transformedMessages);
        this.selectedClient.messages = transformedMessages;

        
      } catch (error) {
        console.error("Failed to fetch messages:", error);
        this.selectedClient.messages = [];

      }
    },
  },
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
  display: flex;
  flex-direction: column;
}
.message {
  margin-bottom: 0.5rem;
  padding: 0.5rem 1rem;
  border-radius: 10px;
  max-width: 60%;
  word-wrap: break-word;
}
.message.trainer {
  text-align: right;
  background-color: #d1f7d6;
  align-self: flex-end;
}
.message.client {
  text-align: left;
  background-color: #f1f1f1;
  align-self: flex-start;
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
