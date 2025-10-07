<template>
  <div class="container mt-5">
    <h2 class="mb-4 text-center">Calorie Goal Calculator</h2>

    <form @submit.prevent="submitGoal" class="mb-4">
      <div class="form-group mb-3">
        <label>Gender</label>
        <select v-model="goal.sex" class="form-control">
          <option value="female">Female</option>
          <option value="male">Male</option>
        </select>
      </div>

      <div class="form-group mb-3">
        <label>Age</label>
        <input v-model.number="goal.age" type="number" class="form-control" />
      </div>

      <div class="form-group mb-3">
        <label>Height (cm)</label>
        <input v-model.number="goal.height" type="number" class="form-control" />
      </div>

      <div class="form-group mb-3">
        <label>Current Weight (kg)</label>
        <input v-model.number="goal.currentWeight" type="number" class="form-control" />
      </div>

      <div class="form-group mb-3">
        <label>Activity Level</label>
        <select v-model="goal.activityLevel" class="form-control">
          <option value="sedentary">Sedentary (little or no exercise)</option>
          <option value="light">Light (1–3 days/week)</option>
          <option value="moderate">Moderate (3–5 days/week)</option>
          <option value="active">Active (6–7 days/week)</option>
          <option value="veryActive">Very Active (physical job or intense training)</option>
        </select>
      </div>

      <div class="form-group mb-3">
        <label>Goal</label>
        <select v-model="goal.goalType" class="form-control">
          <option value="lose">Lose weight</option>
          <option value="gain">Gain weight</option>
          <option value="maintain">Maintain weight</option>
        </select>
      </div>

      <div class="form-group mb-3">
        <label>Intensity</label>
        <select v-model="goal.intensity" class="form-control">
          <option value="low">Low</option>
          <option value="medium">Medium</option>
          <option value="high">High</option>
        </select>
      </div>

      <button type="submit" class="btn btn-primary w-100">
        Calculate Calories
      </button>
    </form>


    <div v-if="result" class="alert alert-success mt-4">
      <h4>Results</h4>
      <p><strong>BMI:</strong> {{ result.bmi }}</p>
      <p><strong>Recommended Calories:</strong> {{ result.targetKcal }} kcal/day</p>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue'
import axios from 'axios'
import jwtDecode from 'jwt-decode'

const goal = ref({
  clientId: '',
  sex: 'null',
  age: null,
  height: null,
  currentWeight: null,
  activityLevel: 'null',
  goalType: 'null',
  intensity: 'null'
})


const result = ref(null)

//token to get user id
const token = localStorage.getItem('token')
if (token) {
  try {
    const decoded = jwtDecode(token)
    goal.value.clientId = decoded.sub
  } catch (err) {
    console.error('Failed to decode token:', err)
  }
}

const submitGoal = async () => {
  try {
    const res = await axios.post('http://localhost:8103/api/goals', goal.value)
    result.value = res.data
  } catch (err) {
    console.error('Failed to submit goal:', err)
  }
}
</script>

<style scoped>
.container {
  max-width: 600px;
}
</style>

