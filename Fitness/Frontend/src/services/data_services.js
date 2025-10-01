import axios from "axios";

const GATEWAY_URL = "http://localhost:8005";

const TRAINERS = `${GATEWAY_URL}/trainer`;
const REVIEW = `${GATEWAY_URL}/review`;
const CLIENT = `${GATEWAY_URL}/client`;
const AUTH_URL = `${GATEWAY_URL}/authentication`;
const MSSQL_USERS = `${GATEWAY_URL}/user`;
const PAYMENT = `${GATEWAY_URL}/payment`;
const TRAININGS = "http://localhost:8004/api/v1/Trainings";


//const TRAINERS = "http://localhost:8000/api/v1/Trainer";
//const REVIEW = "http://localhost:8001/api/v1/Review";
//const CLIENT = "http://localhost:8100/api/v1/Client";
//const AUTH_URL = "http://localhost:4000/api/v1/authentication/";
//const MSSQL_USERS = "http://localhost:4000/api/v1/User/";
//const PAYMENT = "http://localhost:8003/api/v1/Payment";

export default {
    methods: {
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

        create_payment(request) {
          axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
          return axios.post(`${PAYMENT}`, request);
        },

        capture_payment(request) {
          axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
          return axios.post(`${PAYMENT}/CapturePayment`, request);
        },

        add_client(request) {
          axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
          return axios.post(`${CLIENT}`, request);
        },

        unregister_user(email) {
          axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
          return axios.delete(`${AUTH_URL}${email}`);
        },

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

        upt_client(cl_id, request) {
          axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
          return axios.put(`${CLIENT}`, request);
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

        remove_trainer(tra_id) {
          axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
          return axios.delete(`${TRAINERS}/${tra_id}`);
        },

        remove_client(cl_id) {
          axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
          return axios.delete(`${CLIENT}/${cl_id}`);
        },

        get_price(tra_id, trainingType) {
          axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
          return axios.get(`${TRAINERS}/GetPrice/${tra_id}/${trainingType}`);
        },

        booking(request) {
          axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
          return axios.put(`${CLIENT}/BookTraining`, request);
        },

        cancelBooking(request) {
          axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
          return axios.put(`${TRAINERS}/CancelTraining`, request);
        },

        add_review(request) {
          axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
          return axios.post(`${REVIEW}`, request);
        },

        get_reviews(tra_id) {
          axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
          return axios.get(`${REVIEW}/${tra_id}`);
        },

        delete_review(rev_id) {
          axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
          return axios.delete(`${REVIEW}/${rev_id}`);
        },

        update_review(request) {
          axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
          return axios.put(`${REVIEW}`, request);
        },

        get_trainer_week_schedule_by_id(week_id, tra_id) {
          axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
          return axios.get(`${TRAINERS}/GetTrainerWeekSchedule/${tra_id}/${week_id}`);
        },

        get_client_week_schedule_by_id(week_id, cl_id) {
          axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
          return axios.get(`${CLIENT}/GetClientWeekSchedule/${cl_id}/${week_id}`);
        },

        get_client_by_id(cli_id) {
          axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
          return axios.get(`${CLIENT}/${cli_id}`);
        },
        get_schedule_trainers_by_client_id(cli_id) {
          axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
          return axios.get(`${CLIENT}/GetTrainerIdsFromClientSchedule/${cli_id}`);
        },

        async get_trainings_for_client(cli_id){
          axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
          return await axios.get(`${TRAININGS}/training/trainingClient/${cli_id}`);
        },

        async get_trainings_for_trainer(trainer_id){
        //axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
        try{
          const data = await axios.get(`${TRAININGS}/training/trainingTrainer/${trainer_id}`);
        } catch (error) {
          console.error("Error fetching trainings!", error);
          throw error;
        }

      }
    }
}
