import axios from "axios";

const ANALYTICS_URL = "http://localhost:8005/api/v1/Analytics";

const analyticsService = {
    async getClientAnalytics(clientId) {
        let harcodedObject = Promise.resolve({
            data: {
            attendedTrainings: 10,
            cancelledTrainings: 2,
            averageRating: 7.4,
            trainersWorkedWith: [
                {
                fullName: "Vukasin Markovic",
                contactEmail: "vmark@fitness.com",
                contactPhone: "+38160123456",
                trainingTypes: [
                    { name: "yoga" },
                    { name: "pilates "}
                ],
                averageRating: 10.0
                }
            ]
            }
        });

        axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
        let response = await axios.get(`${ANALYTICS_URL}/GetClientNumOfTrainingForClient?clientId=${clientId}`);
        return response;
    }
}

export default analyticsService;
