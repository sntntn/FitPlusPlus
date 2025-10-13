import axios from "axios";

const BASE_URL = "http://localhost:8157/api/v1";


export async function getAllFoods() {
  try {
    const res = await axios.get(`${BASE_URL}/food`);
    return res.data;
  } catch (err) {
    console.error("Error fetching foods:", err);
    throw err;
  }
}


//trener

export async function getMealPlansForTrainer(trainerId) {
  try {
    const res = await axios.get(`${BASE_URL}/mealplans/trainer/${trainerId}`);
    return res.data || [];
  } catch (err) {
    console.error("Error fetching trainer meal plans:", err);
    throw err;
  }
}

export async function saveMealPlan(plan) {
  try {
    const res = await axios.post(`${BASE_URL}/mealplans`, plan);
    return res.data;
  } catch (err) {
    console.error("Error saving meal plan:", err);
    throw err;
  }
}

export async function deleteMealPlan(trainerId, goalType) {
  try {
    const res = await axios.delete(`${BASE_URL}/MealPlans/trainer/${trainerId}/goal/${goalType}`);
    return res.data;
  } catch (err) {
    console.error("Error deleting meal plan:", err);
    throw err;
  }
}

//klijent

export async function loadTrainers() {
  try {
    const res = await axios.get(`${BASE_URL}/mealplans`);
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
    const res = await axios.post(`${BASE_URL}/goals`, goal);
    return res.data;
  } catch (error) {
    console.error("Error calculating goal:", error);
    throw error;
  }
}

export async function fetchPlan(trainerId, goalType) {
  try {
    const res = await axios.get(`${BASE_URL}/MealPlans/trainer/${trainerId}/goal/${goalType}`);
    return res.data;
  } catch (error) {
    console.error("Error fetching plan:", error);
    throw error;
  }
}
