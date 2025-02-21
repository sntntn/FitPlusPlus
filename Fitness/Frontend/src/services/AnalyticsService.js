import axios from "axios";

const ANALYTICS_URL = "http://localhost:8018/api/v1/Analytics";

const analyticsService = {
    async getClientAnalytics(clientId) {
        axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
        let response = await axios.get(`${ANALYTICS_URL}/ClientTrainings?clientId=${clientId}`);
        return response;
    }
}

export default analyticsService;
