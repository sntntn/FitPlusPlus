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
export default {
  data() {
    return {
      trainers: [
        {
          id: 1,
          name: "Trainer 1",
          messages: [
            { id: 1, text: "Hello, Client!", sender: "trainer" },
            { id: 2, text: "Hi, Trainer!", sender: "client" },
          ],
        },
        {
          id: 2,
          name: "Trainer 2",
          messages: [
            { id: 3, text: "How can I help you?", sender: "trainer" },
            { id: 4, text: "I have a question about my program.", sender: "client" },
          ],
        },
      ],
      selectedTrainer: null,
      newMessage: "",
    };
  },
  created() {
    //this.selectedTrainer = this.trainers[0];
  },
  methods: {
    selectTrainer(trainer) {
      this.selectedTrainer = trainer;
    },
    sendMessage() {
      if (this.newMessage.trim() !== "") {
        this.selectedTrainer.messages.push({
          id: Date.now(),
          text: this.newMessage,
          sender: "client",
        });
        this.newMessage = "";
      }
    },
  },
  mounted() {
      this.$parent.$parent.$parent.setUserData(this.$route.params.id, "client");
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
