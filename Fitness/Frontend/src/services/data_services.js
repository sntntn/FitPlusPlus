import axios from "axios";

const TRAINERS = "http://localhost:8000";
const LOGIN_URL = "http://localhost:4000/api/v1/authentication/Login";


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

        register(firstname, lastname, username, password, email, phonenumber) {
            
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

        get_trainers(inf_id) {
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

        remove_trainer(tra_id) {
          axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
          return axios.delete(`${TRAINERS}/api/v1/Trainer/${tra_id}`);
        }
    }
}
