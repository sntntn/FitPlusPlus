import axios from "axios";

const GATEWAY_URL = "http://localhost:8005";

//const CHAT = "http://localhost:8082/api/Chat";
//const CLIENT = "http://localhost:8100/api/v1/Client";
//const TRAINERS = "http://localhost:8000/api/v1/Trainer";

const CHAT = `${GATEWAY_URL}/chat`;
const CLIENT = `${GATEWAY_URL}/client`;
const TRAINERS = `${GATEWAY_URL}/trainer`;


export async function getBasicInfoForTrainerSessions(trainerId) {
    try {
      const response = await axios.get(`${CHAT}/sessions/${trainerId}/my-sessions-summary`);
      return response.data;
    } catch (error) {
        console.error("Error fetching trainer sessions basic info:", error);
        throw error; 
    }
}

export async function getBasicInfoForClientSessions(clientId) {
  try {
    const response = await axios.get(`${CHAT}/sessions/${clientId}/my-sessions-summary`);
    return response.data;
  } catch (error) {
      console.error("Error fetching client sessions basic info:", error);
      throw error; 
  }
}

export async function getMessagesFromSession(trainerId, clientId) {
  try {
    const response = await axios.get(`${CHAT}/sessions/messages?trainerId=${trainerId}&clientId=${clientId}`);
    return response.data;
  } catch (error) {
    console.error("Error fetching messages:", error);
    throw error;
  }
}

export async function sendMessageToSession(trainerId, clientId, content, senderType) {
  try {
    const response = await axios.post(
      `${CHAT}/sessions/messages`,
      content,
      {
        params: {
          trainerId,
          clientId,
          senderType,
        },
        headers: {
          "Content-Type": "application/json",
        },
      }
    );
    return response.data;
  } catch (error) {
    console.error("Error sending message:", error);
    throw error;
  }
}

export async function createChatSession(trainerId, clientId) {
  try {
    const response = await axios.post(
      `${CHAT}/sessions`, 
      null,
      {
        params: {
          trainerId,
          clientId,
        },
        headers: {
          "Content-Type": "application/json",
        },
      }
    );
    return response.data;

  } catch (error) {
    console.error("Error creating chat session:", error);
    alert("Failed to create chat session. Please try again.");
    throw error;
  }
}

export async function getClientById(clientId) {
  try {
    const response = await axios.get(`${CLIENT}/${clientId}`);
    return response.data;
  } catch (error) {
    console.error("Error fetching client info based on id:", error);
    throw error;
  }
}

export async function getTrainerById(trainerId) {
  try {
    const response = await axios.get(`${TRAINERS}/${trainerId}`);
    return response.data;
  } catch (error) {
    console.error("Error fetching trainer info based on id:", error);
    throw error;
  }
}
