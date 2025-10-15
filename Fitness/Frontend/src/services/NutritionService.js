import axios from "axios";

const GATEWAY_URL = "http://localhost:8005";
const FOOD_URL = `${GATEWAY_URL}/food`
const MEAL_PLANS_URL = `${GATEWAY_URL}/mealplans`
const GOALS_URL = `${GATEWAY_URL}/goals`

// const BASE_URL = "http://localhost:8157/api/v1";

export async function addFood(food) {
  try {
    const res = await axios.post(`${FOOD_URL}`, food);
    return res.data;
  } catch (err) {
    console.error("Error adding food:", err);
    throw err;
  }
}

export async function getAllFoods() {
  try {
    const res = await axios.get(`${FOOD_URL}`);
    return res.data;
  } catch (err) {
    console.error("Error fetching foods:", err);
    throw err;
  }
}


//trener

export async function getMealPlansForTrainer(trainerId) {
  try {
    const res = await axios.get(`${MEAL_PLANS_URL}/trainer/${trainerId}`);
    return res.data || [];
  } catch (err) {
    console.error("Error fetching trainer meal plans:", err);
    throw err;
  }
}

export async function saveMealPlan(plan) {
  try {
    const res = await axios.post(`${MEAL_PLANS_URL}`, plan);
    return res.data;
  } catch (err) {
    console.error("Error saving meal plan:", err);
    throw err;
  }
}

export async function deleteMealPlan(trainerId, goalType) {
  try {
    const res = await axios.delete(`${MEAL_PLANS_URL}/trainer/${trainerId}/goal/${goalType}`);
    return res.data;
  } catch (err) {
    console.error("Error deleting meal plan:", err);
    throw err;
  }
}

//klijent

export async function loadTrainers() {
  try {
    const res = await axios.get(`${MEAL_PLANS_URL}`);
    const seen = new Map();
    for (const p of res.data) {
      if (!seen.has(p.trainerId)) {
        seen.set(p.trainerId, { id: p.trainerId, name: p.trainerName });
      }
    }
    return Array.from(seen.values());
  } catch (err) {
    console.error("Error loading trainers:", err);
    throw err;
  }
}

export async function calculateGoal(goal) {
  try {
    const res = await axios.post(`${GOALS_URL}`, goal);
    return res.data;
  } catch (error) {
    console.error("Error calculating goal:", error);
    throw error;
  }
}

export async function fetchPlan(trainerId, goalType) {
  try {
    const res = await axios.get(`${MEAL_PLANS_URL}/trainer/${trainerId}/goal/${goalType}`);
    return res.data;
  } catch (error) {
    console.error("Error fetching plan:", error);
    throw error;
  }
}
