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
          <span v-if="!trainer.isUnlocked" class="locked-icon">{{ trainer.name }}🔒</span>
          <span v-if="trainer.isUnlocked" class="unlocked-icon">{{ trainer.name }}</span> </li>
        <li v-if="trainers.length === 0">
          <p class="warning-text1">No available trainer</p>
        </li>
      </ul>
    </aside>

    <main class="chat-main">
      <template v-if="selectedTrainer">
        <h3>Chat with {{ selectedTrainer.name }}</h3>
        <div class="chat-messages" ref="messagesContainer">
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
import { 
  getBasicInfoForClientSessions,
  getMessagesFromSession,
  sendMessageToSession,
  getTrainerById
} from "../../services/ChatService";

export default {
  data() {
    return {
      trainers: [],
      selectedTrainer: null,
      newMessage: "",
      socket: null,
    };
  },
  created() {
    this.$parent.$parent.$parent.setUserData(this.$route.params.id, "client");
    this.clientId = this.$route.params.id;
    this.fetchClientSessionsBasicInfo();
  },
  mounted() {
  },
  beforeDestroy() {
    if (this.socket) {
      console.log("Closing WebSocket before component is destroyed.");
      this.socket.close();
    }
  },
  methods: {
    selectTrainer(trainer) {
      if(this.selectedTrainer && this.socket){
        this.socket.close();
        console.log("Closed previous WebSocket");
      }

      this.selectedTrainer = trainer;
      console.log(trainer.id);
      this.fetchMessages(trainer.id);
      this.openWebSocket(trainer.id);
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
            this.scrollToBottom();
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
            
            for (const session of basicInfo){
              try {
                const trainerInfo = await getTrainerById(session.trainerId);

                this.trainers.push({
                  id: session.trainerId,
                  name: `${trainerInfo.fullName}`,
                  isUnlocked: session.isUnlocked,
                  expirationDate: session.expirationDate,
                  messages: [],
                });
              } catch (trainerError) {
                  console.error(`Failed to fetch trainer info for TrainerId: ${session.trainerId}`, trainerError);
              }
            }
        } catch (error) {
            console.error("Failed to fetch client sessions basic info:", error);
        }
    },
    async fetchMessages(trainerId) {
      const clientId = this.clientId;
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

        this.$nextTick(() => this.scrollToBottom());

      } catch (error) {
        console.error("Failed to fetch messages:", error);
        this.selectedTrainer.messages = [];

      }
    },

    openWebSocket(trainerId) {
      const clientId = this.clientId;
      const wsUrl = `ws://localhost:8082/ws/chat?trainerId=${trainerId}&clientId=${clientId}`;

      this.socket = new WebSocket(wsUrl);

      this.socket.onopen = () => {
        console.log("WebSocket connected for trainer:", trainerId);
      };

      this.socket.onmessage = (event) => {
        const messageData = JSON.parse(event.data);
        console.log("New message received:", messageData);

        let wasAtBottom = this.isScrolledToBottom();

        if (this.selectedTrainer && this.selectedTrainer.id === trainerId && messageData.SenderType=="trainer") {
          this.selectedTrainer.messages.push({
            id: messageData.Id || Date.now(),
            text: messageData.Content,
            sender: messageData.SenderType.toLowerCase(),
          });
        }
        
        if (wasAtBottom) {
            this.scrollToBottom();
        }

      };

      this.socket.onclose = () => {
        console.log("WebSocket closed for trainer:", trainerId);
      };

      this.socket.onerror = (error) => {
        console.error("WebSocket error:", error);
      };
    },

    scrollToBottom() {
      this.$nextTick(() => {
        const container = this.$refs.messagesContainer;
        if (container) {
          container.scrollTop = container.scrollHeight;
        }
      });
    },
    isScrolledToBottom() {
      const container = this.$refs.messagesContainer;
      if (!container) return false;
      return container.scrollHeight - container.scrollTop <= container.clientHeight + 5;
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
