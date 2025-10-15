import axios from "axios";

const GATEWAY_URL = "http://localhost:8005";

const TRAINERS = `${GATEWAY_URL}/trainer`;
const REVIEW = `${GATEWAY_URL}/review`;
const CLIENT = `${GATEWAY_URL}/client`;
const AUTH_URL = `${GATEWAY_URL}/authentication`;
const MSSQL_USERS = `${GATEWAY_URL}/user`;
const PAYMENT = `${GATEWAY_URL}/payment`;
const TRAININGS = `${GATEWAY_URL}/training`;
const UPLOAD = `${GATEWAY_URL}/upload`

//const TRAINERS = "http://localhost:8000/api/v1/Trainer";
//const REVIEW = "http://localhost:8001/api/v1/Review";
//const CLIENT = "http://localhost:8100/api/v1/Client";
//const AUTH_URL = "http://localhost:4000/api/v1/authentication/";
//const MSSQL_USERS = "http://localhost:4000/api/v1/User/";
//const PAYMENT = "http://localhost:8003/api/v1/Payment";
//const TRAININGS = "http://localhost:8007/api/v1/Training";
// const UPLOAD = "http://localhost:8007/api/v1/Upload"

export default {
    methods: {
        // ====================
        // Authentication
        // ====================
        login(user, pw) {
          const data = {
            username: user,
            password: pw
          };

          const headers = {
            'Content-Type': 'application/json'
          };

          return axios.post(AUTH_URL + '/Login', data, { headers });
        },

        register(firstname, lastname, username, password, email, phonenumber, role) {
          const data = {
            firstname: firstname,
            lastname: lastname,
            username: username,
            password: password,
            email: email,
            phonenumber: phonenumber
          };

          const headers = {
            'Content-Type': 'application/json'
          };

          return axios.post(AUTH_URL + '/Register' + role, data, { headers });
        },

        logout(request) {
          axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
          return axios.post(AUTH_URL + '/Logout', request);
        },

        unregister_user(email) {
          axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
          var emailEncoded = encodeURIComponent(email)
          return axios.delete(`${AUTH_URL}/${emailEncoded}`);
        },

        // ====================
        // Access Token Handling
        // ====================
        parse_access_token(access_token) {
          const at_parts = access_token.split('.');
          const payload_string = at_parts[1];
          return JSON.parse(atob(payload_string));
        },

        save_access_token_data(access_token) {
          const at_data = this.parse_access_token(access_token);

          const username = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";
          const role = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";

          sessionStorage.setItem('username', at_data[username]);
          sessionStorage.setItem('role', at_data[role]);

          return at_data[role];
        },

        // ====================
        // Payments
        // ====================
        create_payment(request) {
          axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
          return axios.post(`${PAYMENT}`, request);
        },

        capture_payment(request) {
          axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
          return axios.post(`${PAYMENT}/CapturePayment`, request);
        },

        // ====================
        // Users / Clients / Trainers
        // ====================
        get_user(username) {
          axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
          return axios.get(MSSQL_USERS + '/' + username);
        },

        get_user_id(role, email) {
          if(role == 'Trainer')
            return this.get_trainer_id(email);
          else if(role == 'Client')
            return this.get_client_id(email);
        },

        get_trainer_id(email) {
          axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
          return axios.get(`${TRAINERS}/GetTrainerByEmail/${email}`);
        },

        get_client_id(email) {
          axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
          return axios.get(`${CLIENT}/GetClientByEmail/${email}`);
        },

        get_trainers() {
          axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
          return axios.get(`${TRAINERS}`);
        },

        get_clients() {
          axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
          return axios.get(`${CLIENT}`);
        },

        add_trainer(request) {
          axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
          return axios.post(`${TRAINERS}`, request);
        },

        upt_trainer(tra_id, request) {
          axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
          return axios.put(`${TRAINERS}`, request);
        },

        remove_trainer(tra_id) {
          axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
          return axios.delete(`${TRAINERS}/${tra_id}`);
        },

        get_trainer_by_id(tra_id) {
          axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
          return axios.get(`${TRAINERS}/${tra_id}`);
        },

        get_trainers_by_type(type) {
          axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
          return axios.get(`${TRAINERS}/GetTrainersByTrainingType/${type}`);
        },

        get_trainers_by_rating(rating) {
          axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
          return axios.get(`${TRAINERS}/GetTrainersByRating/${rating}`);
        },

        get_price(tra_id, trainingType) {
          axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
          return axios.get(`${TRAINERS}/GetPrice/${tra_id}/${trainingType}`);
        },

        add_client(request) {
          axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
          return axios.post(`${CLIENT}`, request);
        },

        upt_client(cl_id, request) {
          axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
          return axios.put(`${CLIENT}`, request);
        },

        remove_client(cl_id) {
          axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
          return axios.delete(`${CLIENT}/${cl_id}`);
        },

        get_client_by_id(cli_id) {
          axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
          return axios.get(`${CLIENT}/${cli_id}`);
        },

        // ====================
        // Reviews
        // ====================
        get_reviews_client(cli_id) {
          axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
          return axios.get(`${REVIEW}/client/${cli_id}`);
        },

        get_reviews_trainer(tra_id) {
          axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
          return axios.get(`${REVIEW}/trainer/${tra_id}`);
        },

        submit_review_client(request) {
          axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
          return axios.post(`${REVIEW}/client/${request.clientId}`, request);
        },

        submit_review_trainer(request) {
          axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
          return axios.post(`${REVIEW}/trainer/${request.trainerId}`, request);
        },

        create_exercise(exercise) {
            axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
            return axios.post(`${TRAININGS}/exercise`, exercise);
        },

        delete_exercise(exercise_id){
          axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
          return axios.delete(`${TRAININGS}/exercise/${exercise_id}`);
        },

        get_exercises_by_trainer(trainer_id){
          axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
          return axios.get(`${TRAININGS}/exercises/${trainer_id}`);
        },

        upload_video(file) {
          axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
          const formData = new FormData();
          formData.append("file", file);
          const response = axios.post(`${UPLOAD}/video`, formData, {
            headers: { "Content-Type": "multipart/form-data" },
          });

          return response.data;
        },

        delete_video(fileName){
          axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
          const response = axios.delete(`${UPLOAD}/video/delete/${fileName}`);
        },

        create_training(training){
          axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
          console.log(JSON.stringify(training))
          return axios.post(`${TRAININGS}/training`, training);
        },

        delete_training(training_id){
          axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
          return axios.delete(`${TRAININGS}/training/${training_id}`);
        },

        create_exercises_for_training(trainingExercise){
          axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
          return axios.post(`${TRAININGS}/trainingExercise`, trainingExercise);
        },

        get_trainings_trainer(trainer_id){
          axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
          return axios.get(`${TRAININGS}/training/trainingTrainer/${trainer_id}`);
        },

        get_trainings_client(){
          axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
          return axios.get(`${TRAININGS}/training/trainingClient`);
        },

        get_training_exercises(trainer_id){
          axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
          return axios.get(`${TRAININGS}/trainingExercises/${trainer_id}`);
        },

        delete_training_exercises(training_id){
          axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
          return axios.delete(`${TRAININGS}/trainingExercise/${training_id}`);
        },

        get_purchased_trainings(client_id){
          axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
          return axios.get(`${TRAININGS}/training/byClient/${client_id}`);
        },

        buy_training(training_id, client_id){
          axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
          return axios.post(`${TRAININGS}/training/${training_id}/addClient/${client_id}`);
        }

    }
}
