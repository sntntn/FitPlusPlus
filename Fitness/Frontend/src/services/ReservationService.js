import axios from "axios";

const GATEWAY_URL = "http://localhost:8005";
const RESERVATIONS = `${GATEWAY_URL}/reservation`;

// const RESERVATIONS = "http://localhost:8103/api/v1/Reservation";

// ---------------------- INDIVIDUAL RESERVATIONS ----------------------

// Admin - get all individual
export async function getAllIndividualReservations() {
  axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
  const response = await axios.get(`${RESERVATIONS}/individual`);
  return response;
}

// Admin - get individual by id
export async function getIndividualReservationById(id) {
  axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
  const response = await axios.get(`${RESERVATIONS}/individual/${id}`);
  return response;
}

// Client - get individual by clientId
export async function getIndividualReservationsByClient(clientId) {
  axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
  const response = await axios.get(`${RESERVATIONS}/individual/client/${clientId}`);
  return response;
}

// Trainer - get individual by trainerId
export async function getIndividualReservationsByTrainer(trainerId) {
  axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
  const response = await axios.get(`${RESERVATIONS}/individual/trainer/${trainerId}`);
  return response;
}

// Client - create individual reservation
export async function createIndividualReservation(reservation) {
  axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
  const response = await axios.post(`${RESERVATIONS}/individual`, reservation, {
    headers: { "Content-Type": "application/json" },
  });
  return response;
}

// ---------------------- GROUP RESERVATIONS ----------------------

// Admin - get all group
export async function getAllGroupReservations() {
  axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
  const response = await axios.get(`${RESERVATIONS}/group`);
  return response;
}

// Admin - get group by id
export async function getGroupReservationById(id) {
  axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
  const response = await axios.get(`${RESERVATIONS}/group/${id}`);
  return response;
}

// Client - get group by clientId
export async function getGroupReservationsByClient(clientId) {
  axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
  const response = await axios.get(`${RESERVATIONS}/group/client/${clientId}`);
  return response;
}

// Trainer - get group by trainerId
export async function getGroupReservationsByTrainer(trainerId) {
  axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
  const response = await axios.get(`${RESERVATIONS}/group/trainer/${trainerId}`);
  return response;
}

// Trainer - create group reservation
export async function createGroupReservation(reservation) {
  axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
  const response = await axios.post(`${RESERVATIONS}/group`, reservation, {
    headers: { "Content-Type": "application/json" },
  });
  return response;
}

// Trainer - delete group reservation
export async function deleteGroupReservation(id) {
  axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
  const response = await axios.delete(`${RESERVATIONS}/group/${id}`);
  return response;
}

// ---------------------- GROUP BOOKING / CANCEL ----------------------

// Client - book group reservation
export async function bookGroupReservation(id, clientId) {
  axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
  const response = await axios.post(`${RESERVATIONS}/group/book/${id}`, null, {
    params: { clientId },
  });
  return response;
}

// Client - cancel individual reservation
export async function cancelClientIndividualReservation(id) {
  axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
  const response = await axios.put(`${RESERVATIONS}/individual/client/cancel/${id}`);
  return response;
}

// Client - cancel group reservation
export async function cancelGroupReservation(id, clientId) {
  axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
  const response = await axios.post(`${RESERVATIONS}/group/cancel/${id}`, null, {
    params: { clientId },
  });
  return response;
}

// Trainer - cancel individual reservation
export async function cancelTrainerIndividualReservation(id) {
  axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
  const response = await axios.put(`${RESERVATIONS}/individual/trainer/cancel/${id}`);
  return response;
}
