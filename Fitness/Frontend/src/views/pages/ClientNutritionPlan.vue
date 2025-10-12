<template>
  <div class="client-container container py-4">
    <div class="row">
      <div class="col-md-6">
        <div class="card p-3 shadow-sm mb-3">
          <h4 class="mb-3 text-center">Goal Calculator</h4>

          <form @submit.prevent="calculateGoal">
            <div class="mb-2">
              <label>Sex</label>
              <select v-model="goal.sex" class="form-select" required>
                <option value="">Select...</option>
                <option value="male">Male</option>
                <option value="female">Female</option>
              </select>
            </div>

            <div class="row mb-2">
              <div class="col">
                <label>Age</label>
                <input v-model.number="goal.age" type="number" class="form-control" required />
              </div>
              <div class="col">
                <label>Height (cm)</label>
                <input v-model.number="goal.height" type="number" class="form-control" required />
              </div>
              <div class="col">
                <label>Weight (kg)</label>
                <input v-model.number="goal.currentWeight" type="number" class="form-control" required />
              </div>
            </div>

            <div class="mb-2">
              <label>Activity Level</label>
              <select v-model="goal.activityLevel" class="form-select" required>
                <option value="">Select...</option>
                <option value="sedentary">Sedentary</option>
                <option value="light">Light</option>
                <option value="moderate">Moderate</option>
                <option value="active">Active</option>
                <option value="veryActive">Very Active</option>
              </select>
            </div>

            <div class="mb-2">
              <label>Goal Type</label>
              <select v-model="goal.goalType" class="form-select" required>
                <option value="">Select...</option>
                <option value="lose">Lose Weight</option>
                <option value="gain">Gain Weight</option>
                <option value="maintain">Maintain Weight</option>
              </select>
            </div>

            <div class="mb-3">
              <label>Intensity</label>
              <select v-model="goal.intensity" class="form-select" required>
                <option value="">Select...</option>
                <option value="low">Low</option>
                <option value="medium">Medium</option>
                <option value="high">High</option>
              </select>
            </div>

            <button type="submit" class="btn btn-success w-100">Calculate</button>
          </form>
        </div>

        <div v-if="calculatedGoal" class="card p-3 shadow-sm text-center">
          <h5>Results</h5>
          <p><strong>BMI:</strong> {{ calculatedGoal.bmi }}</p>
          <p><strong>Target Kcal:</strong> {{ calculatedGoal.targetKcal }}</p>
        </div>
      </div>


      <div class="col-md-6">
        <div class="card p-3 shadow-sm">
          <h4 class="mb-3 text-center">Your Nutrition Plan</h4>

          <div v-if="!calculatedGoal">
            <p class="text-muted text-center">
              Please calculate your goal first so we can suggest a plan.
            </p>
          </div>

          <div v-else-if="!plan">
            <p class="text-muted text-center">
              Your plan for <strong>{{ goal.goalType }}</strong> is still being prepared.
            </p>
          </div>

          <div v-else>
            <h5 class="text-center mb-3 text-capitalize">
              Plan for {{ plan.goalType }}
            </h5>

            <div v-for="mealType in mealTypes" :key="mealType" class="mb-3">
              <h6 class="text-capitalize">{{ mealType }}</h6>
              <ul>
                <li v-for="food in plan[mealType]" :key="food.name">
                  {{ food.name }} — <small>{{ food.calories }} kcal</small>
                </li>
              </ul>
            </div>
          </div>

          <div v-if="plan" class="text-center mt-3">
            <button @click="refreshPlan" class="btn btn-outline-primary btn-sm">
              Refresh Plan
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue'
import axios from 'axios'

const goal = ref({
  clientId: '',
  sex: '',
  age: null,
  height: null,
  currentWeight: null,
  activityLevel: '',
  goalType: '',
  intensity: ''
})

const calculatedGoal = ref(null)
const plan = ref(null)
const mealTypes = ['breakfast', 'lunch', 'dinner', 'snacks']


const calculateGoal = async () => {
  try {
    const res = await axios.post('http://localhost:8103/api/goals', goal.value)
    calculatedGoal.value = res.data
    await fetchPlan()
  } catch (error) {
    console.error(error)
    alert('Error calculating or fetching plan.')
  }
}


const fetchPlan = async () => {
  try {
    const allPlans = await axios.get('http://localhost:8103/api/mealplans')
    const match = allPlans.data.find(
      p => p.goalType === goal.value.goalType
    )
    plan.value = match || null
  } catch (error) {
    console.error(error)
    alert('Error fetching plan.')
  }
}


const refreshPlan = async () => {
  await fetchPlan()
}
</script>

<style scoped>
.client-container {
  max-width: 1000px;
}

ul {
  list-style-type: square;
  padding-left: 1.2rem;
}
</style>



