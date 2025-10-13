import axios from "axios";

const GATEWAY_URL = "http://localhost:8005";
// const ANALYTICS_URL = `${GATEWAY_URL}/analytics`;

const ANALYTICS_URL = "http://localhost:8018/api/v1/Analytics";

const analyticsService = {
    async getTrainerIndividualTrainings(trainerId) {
        axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
        let response = await axios.get(`${ANALYTICS_URL}/individual/trainer/${trainerId}`);
        return response;
    },

    async getClientIndividualTrainings(clientId) {
        axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
        let response = await axios.get(`${ANALYTICS_URL}/individual/client/${clientId}`);
        return response;
    },

    async getTrainerGroupTrainings(trainerId) {
        axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
        let response = await axios.get(`${ANALYTICS_URL}/group/trainer/${trainerId}`);
        return response;
    },

    async getClientGroupTrainings(clientId) {
        axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
        let response = await axios.get(`${ANALYTICS_URL}/group/client/${clientId}`);
        return response;
    }
}

export default analyticsService;
