import axios from "axios";

const NOTIFICATIONS = "http://localhost:8004/api/v1/Notification";

// TO DO when Mitreski switch Notification to Gateway 

//const GATEWAY_URL = "http://localhost:8005";
//const NOTIFICATIONS = `${GATEWAY_URL}/api/v1/Notification`;

// Admin
export async function getNotifications() {
  axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
  try {
    axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
    const response = await axios.get(`${NOTIFICATIONS}`);
    return response.data;
  } catch (error) {
    console.error("Error fetching notifications:", error);
    throw error;
  }
}

export async function getNotificationsByUserId(userId) {
  try {
    axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
    const response = await axios.get(`${NOTIFICATIONS}/user/${userId}`);
    return response.data;
  } catch (error) {
    console.error("Error fetching notifications by userId:", error);
    throw error;
  }
}

export async function getNotificationById(id) {
  try {
    axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
    const response = await axios.get(`${NOTIFICATIONS}/${id}`);
    return response.data;
  } catch (error) {
    console.error("Error fetching notification by id:", error);
    throw error;
  }
}

export async function updateNotification(notification) {
  try {
    axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
    const response = await axios.put(`${NOTIFICATIONS}`, notification, {
      headers: { "Content-Type": "application/json" }
    });
    console.log(response.data);
    return response.data;
  } catch (error) {
    console.error("Error updating notification:", error);
    throw error;
  }
}

export async function markNotificationAsRead(notificationId) {
  axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
  return axios.put(
    `${NOTIFICATIONS}/${notificationId}/read`, 
    {}, 
    { headers: { "Content-Type": "application/json" } }
  )
  .then(res => res.data)
  .catch(err => {
    console.error("Error marking notification as read:", err);
    throw err;
  });
}


export async function deleteAllNotifications() {
  try {
    axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
    const response = await axios.delete(`${NOTIFICATIONS}`);
    return response.data;
  } catch (error) {
    console.error("Error deleting all notifications:", error);
    throw error;
  }
}

export async function deleteNotificationsByUserId(userId) {
  try {
    axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
    const response = await axios.delete(`/user/${userId}`);
    return response.data;
  } catch (error) {
    console.error("Error deleting notifications by userId:", error);
    throw error;
  }
}

export async function deleteNotificationById(id) {
  try {
    axios.defaults.headers.common = { 'Authorization': `Bearer ${sessionStorage.getItem('accessToken')}` };
    const response = await axios.delete(`${NOTIFICATIONS}/${id}`);
    return response.data;
  } catch (error) {
    console.error("Error deleting notification by id:", error);
    throw error;
  }
}
