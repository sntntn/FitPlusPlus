import axios from "axios";

const BASE_URL = "http://localhost:8082/api/Chat";

export async function getBasicInfoForTrainerSessions(trainerId) {
    try {
      const response = await axios.get(`${BASE_URL}/sessions/${trainerId}/basic-info`);
      return response.data;
    } catch (error) {
        console.error("Error fetching trainer sessions basic info:", error);
        throw error; 
    }
}

export async function getMessagesFromSession(trainerId, clientId) {
  try {
    const response = await axios.get(`${BASE_URL}/sessions/messages?trainerId=${trainerId}&clientId=${clientId}`);
    return response.data;
  } catch (error) {
    console.error("Error fetching messages:", error);
    throw error;
  }
}

