import axios from "axios";

const TRAINERS = "http://localhost:8000";
const REVIEW = "http://localhost:8001";
const CLIENT = "http://localhost:8100";
const LOGIN_URL = "http://localhost:4000/api/v1/authentication/Login";
const REGISTER_URL = "http://localhost:4000/api/v1/authentication/Register";
const MSSQL_USERS = "http://localhost:4000/api/v1/User/";

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

          return axios.post(LOGIN_URL, data, { headers });                  
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

          return axios.post(REGISTER_URL + role, data, { headers });
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
          return axios.get(MSSQL_USERS + username);
        },

        get_user_id(role, email) {
          if(role == 'Trainer')
            return this.get_trainer_id(email);
          else if(role == 'Client')
            return this.get_client_id(email);
        },

        get_trainer_id(email) {
          axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
          return axios.get(`${TRAINERS}/api/v1/Trainer/GetTrainerByEmail/${email}`);
        },

        get_client_id(email) {
          axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
          return axios.get(`${CLIENT}/api/v1/Client/GetClientByEmail/${email}`);
        },

        get_trainers() {
          axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
          return axios.get(`${TRAINERS}/api/v1/Trainer`);
        },

        add_trainer(request) {
          axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
          return axios.post(`${TRAINERS}/api/v1/Trainer`, request);
        },

        upt_trainer(tra_id, request) {
          axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
          return axios.put(`${TRAINERS}/api/v1/Trainer`, request);
        },

        get_trainer_by_id(tra_id) {
          axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
          return axios.get(`${TRAINERS}/api/v1/Trainer/${tra_id}`);
        },

        get_trainers_by_type(type) {
          axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
          return axios.get(`${TRAINERS}/api/v1/Trainer/GetTrainersByTrainingType/${type}`);
        },

        get_trainers_by_rating(rating) {
          axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
          return axios.get(`${TRAINERS}/api/v1/Trainer/GetTrainersByRating/${rating}`);
        },

        remove_trainer(tra_id) {
          axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
          return axios.delete(`${TRAINERS}/api/v1/Trainer/${tra_id}`);
        },

        add_review(request) {
          axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
          return axios.post(`${REVIEW}/api/v1/Review`, request);
        },

        get_reviews(tra_id) {
          axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
          return axios.get(`${REVIEW}/api/v1/Review/${tra_id}`);
        },

        delete_review(rev_id) {
          axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
          return axios.delete(`${REVIEW}/api/v1/Review/${rev_id}`);
        },

        update_review(request) {
          axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
          return axios.put(`${REVIEW}/api/v1/Review`, request);
        },

        get_trainer_week_schedule_by_id(week_id, tra_id) {
          axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
          return axios.get(`${TRAINERS}/api/v1/Trainer/GetTrainerWeekSchedule/${tra_id}/${week_id}`);
        },

        get_client_week_schedule_by_id(week_id, cl_id) {
          axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
          return axios.get(`${CLIENT}/api/v1/Client/GetClientWeekSchedule/${cl_id}/${week_id}`);
        },

        get_client_by_id(cli_id) {
          axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
          return axios.get(`${CLIENT}/api/v1/Client/${cli_id}`);
        },
    }
}
