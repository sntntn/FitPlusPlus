<template>
  <div class="chat-container">

    <aside class="chat-list">
      <h3>Clients</h3>
      <ul>
        <li
          v-for="client in clients"
          :key="client.id"
          :class="{ active: client.id === selectedClient.id }"
          @click="selectClient(client)"
        >
          <p>{{ client.name }}</p>
        </li>
      </ul>
    </aside>

    <main class="chat-main" v-if="selectedClient">
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
    </main>
  </div>
</template>

<script>
import { getBasicInfoForTrainerSessions } from "../../services/ChatService";

export default {
  data() {
    return {
      clients: [
        {
          id: 1,
          name: "Client 1",  
          isUnlocked: true,
          expirationDate: "2025-02-20T12:00:00",
          messages: [
            { id: 1, text: "Hello, Trainer!", sender: "client" },
            { id: 2, text: "Hi, how can I help you?", sender: "trainer" },
          ],
        },
        {
          id: 2,
          name: "Client 2",
          isUnlocked: false,  
          expirationDate: "2025-01-30T10:00:00",
          messages: [
            { id: 3, text: "When is my next session?", sender: "client" },
            { id: 4, text: "It's on Monday at 10 AM.", sender: "trainer" },
          ],
        },
      ],
      selectedClient: null,
      newMessage: "",
    };
  },
  created() {
    this.selectedClient = this.clients[0];
  },
  mounted() {
    this.$parent.$parent.$parent.setUserData(this.$route.params.id, "trainer");
    this.trainerId = this.$route.params.id; // Ako trainerId dolazi iz rute
    this.fetchTrainerSessionsBasicInfo();
   
  },
  methods: {
    selectClient(client) {
      this.selectedClient = client;
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
            console.log("Trainer Sessions Basic Info:", basicInfo);
        } catch (error) {
            console.error("Failed to fetch trainer sessions basic info:", error);
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
</style>
