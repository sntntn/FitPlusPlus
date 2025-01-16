import axios from "axios";

const BASE_URL = "http://localhost:8082/api/Chat";

export async function getBasicInfoForTrainerSessions(trainerId) {
    try {
      const response = await axios.get(`${BASE_URL}/sessions/trainer/${trainerId}/basic-info`);
      return response.data;
    } catch (error) {
        console.error("Error fetching trainer sessions basic info:", error);
        throw error; 
    }
}
