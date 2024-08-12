import axios from "axios";

const TRAINERS = "http://localhost:8000";


export default {
    methods: {
        login(user, pw) {
                          
        },

        register(firstname, lastname, username, password, email, phonenumber) {
            
        },

        get_trainers(inf_id) {
          axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('pharmacyToken')}` };
          return axios.get(`${TRAINERS}/api/v1/Trainer`);
        },

        add_trainer(request) {
          axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('pharmacyToken')}` };
          return axios.post(`${TRAINERS}/api/v1/Trainer`, request);
        },

        upt_trainer(tra_id, request) {
          axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('pharmacyToken')}` };
          return axios.put(`${TRAINERS}/api/v1/Trainer`, request);
        },

        get_trainer_by_id(tra_id) {
          axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('pharmacyToken')}` };
          return axios.get(`${TRAINERS}/api/v1/Trainer/${tra_id}`);
        },

        remove_trainer(tra_id) {
          axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('pharmacyToken')}` };
          return axios.delete(`${TRAINERS}/api/v1/Trainer/${tra_id}`);
        }
    }
}
