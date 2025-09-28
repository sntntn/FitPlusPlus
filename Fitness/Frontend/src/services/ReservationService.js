import axios from "axios";

const GATEWAY_URL = "http://localhost:8005";
//const RESERVATIONS = `${GATEWAY_URL}/reservation`;                // When Mitreski switch Backend for Reservation to Gateway!!!

const RESERVATIONS = "http://localhost:8025/api/v1/Reservation";    //temporary




// ---------------------- INDIVIDUAL RESERVATIONS ----------------------

// Admin - get all individual
export async function getAllIndividualReservations() {
  const response = await axios.get(`${RESERVATIONS}/individual`);
  return response.data;
}

// Admin - get individual by id
export async function getIndividualReservationById(id) {
  const response = await axios.get(`${RESERVATIONS}/individual/${id}`);
  return response.data;
}

// Client - get individual by clientId
export async function getIndividualReservationsByClient(clientId) {
  const response = await axios.get(`${RESERVATIONS}/individual/client/${clientId}`);
  return response.data;
}

// Trainer - get individual by trainerId
export async function getIndividualReservationsByTrainer(trainerId) {
  const response = await axios.get(`${RESERVATIONS}/individual/trainer/${trainerId}`);
  return response.data;
}

// Client - create individual reservation
export async function createIndividualReservation(reservation) {
  const response = await axios.post(`${RESERVATIONS}/individual`, reservation, {
    headers: { "Content-Type": "application/json" },
  });
  return response.data;
}

// Client/Trainer - update individual reservation
export async function updateIndividualReservation(reservation) {
  const response = await axios.put(`${RESERVATIONS}/individual`, reservation, {
    headers: { "Content-Type": "application/json" },
  });
  return response.data;
}

// Client/Trainer - delete individual reservation
export async function deleteIndividualReservation(id) {
  const response = await axios.delete(`${RESERVATIONS}/individual/${id}`);
  return response.data;
}

// ---------------------- GROUP RESERVATIONS ----------------------

// Admin - get all group
export async function getAllGroupReservations() {
  const response = await axios.get(`${RESERVATIONS}/group`);
  return response.data;
}

// Admin - get group by id
export async function getGroupReservationById(id) {
  const response = await axios.get(`${RESERVATIONS}/group/${id}`);
  return response.data;
}

// Client - get group by clientId
export async function getGroupReservationsByClient(clientId) {
  const response = await axios.get(`${RESERVATIONS}/group/client/${clientId}`);
  return response.data;
}

// Trainer - get group by trainerId
export async function getGroupReservationsByTrainer(trainerId) {
  const response = await axios.get(`${RESERVATIONS}/group/trainer/${trainerId}`);
  return response.data;
}

// Trainer - create group reservation
export async function createGroupReservation(reservation) {
  const response = await axios.post(`${RESERVATIONS}/group`, reservation, {
    headers: { "Content-Type": "application/json" },
  });
  return response.data;
}

// Trainer - update group reservation
export async function updateGroupReservation(reservation) {
  const response = await axios.put(`${RESERVATIONS}/group`, reservation, {
    headers: { "Content-Type": "application/json" },
  });
  return response.data;
}

// Trainer - delete group reservation
export async function deleteGroupReservation(id) {
  const response = await axios.delete(`${RESERVATIONS}/group/${id}`);
  return response.data;
}

// ---------------------- GROUP BOOKING / CANCEL ----------------------

// Client - book group reservation
export async function bookGroupReservation(id, clientId) {
  const response = await axios.post(`${RESERVATIONS}/group/book/${id}`, null, {
    params: { clientId },
  });
  return response.data;
}

// Client - cancel group reservation
export async function cancelGroupReservation(id, clientId) {
  const response = await axios.post(`${RESERVATIONS}/group/cancel/${id}`, null, {
    params: { clientId },
  });
  return response.data;
}
