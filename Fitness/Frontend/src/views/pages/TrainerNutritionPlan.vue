<template>
  <div class="trainer-container container py-4">
    <h2 class="mb-3 text-center">Nutrition Plans</h2>

    <div class="card p-3 mb-4 shadow-sm">
      <h4 class="mb-3">Create or Update Plan</h4>

      <div class="form-group mb-3">
        <label>Select Goal Type</label>
        <select v-model="goalType" class="form-control">
          <option value="" disabled>Select...</option>
          <option value="lose">Lose Weight</option>
          <option value="gain">Gain Weight</option>
          <option value="maintain">Maintain Weight</option>
        </select>
      </div>

      <div v-for="mealType in mealTypes" :key="mealType" class="mb-3">
        <label class="form-label text-capitalize">{{ mealType }}</label>
        <select v-model="mealPlan[mealType]" multiple class="form-select">
          <option v-for="food in foods" :key="food.id || food.name" :value="{ name: food.name, calories: food.calories }">
            {{ food.name }} ({{ food.calories }} kcal)
          </option>
        </select>
      </div>

      <button @click="savePlan" class="btn btn-success w-100">
        Save / Update Plan
      </button>
    </div>

    <div class="card p-3 shadow-sm">
      <h4 class="mb-3 text-center">Existing Plans Overview</h4>

      <div v-if="plans.length === 0" class="text-muted text-center">
        No nutrition plans created yet.
      </div>

      <table v-else class="table table-bordered text-center align-middle">
        <thead>
          <tr>
            <th>Goal Type</th>
            <th>Created At</th>
            <th>Preview</th>
            <th>Delete</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="plan in plans" :key="plan.id">
            <td class="text-capitalize">{{ plan.goalType }}</td>
            <td>{{ new Date(plan.createdAt).toLocaleString() }}</td>
            <td>
              <button class="btn btn-outline-primary btn-sm" @click="selectPlan(plan)">
                View Details
              </button>
            </td>
            <td>
              <button class="btn btn-outline-danger btn-sm" @click="deletePlan(plan.goalType)">
                Delete
              </button>
            </td>
          </tr>
        </tbody>
      </table>

      <div v-if="selectedPlan" class="mt-3">
        <h5>Plan for: <strong>{{ selectedPlan.goalType }}</strong></h5>
        <div class="row">
          <div v-for="mealType in mealTypes" :key="mealType" class="col-md-6">
            <h6 class="mt-3 text-capitalize">{{ mealType }}</h6>
            <ul>
              <li v-for="item in selectedPlan[mealType]" :key="item.name">
                {{ item.name }} — <small>{{ item.calories }} kcal</small>
              </li>
            </ul>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import axios from 'axios'

const foods = ref([])
const plans = ref([])
const goalType = ref('')
const mealPlan = ref({
  breakfast: [],
  lunch: [],
  dinner: [],
  snacks: []
})
const selectedPlan = ref(null)
const mealTypes = ['breakfast', 'lunch', 'dinner', 'snacks']

//Load 
onMounted(async () => {
  try {
    const foodRes = await axios.get('http://localhost:8103/api/food')
    foods.value = foodRes.data

    const plansRes = await axios.get('http://localhost:8103/api/mealplans')
    plans.value = plansRes.data
  } catch (err) {
    console.error('Error loading data:', err)
  }
})

//Create plan
const savePlan = async () => {
  if (!goalType.value) {
    alert('Please select a goal type.')
    return
  }

  const plan = {
    goalType: goalType.value,
    breakfast: mealPlan.value.breakfast,
    lunch: mealPlan.value.lunch,
    dinner: mealPlan.value.dinner,
    snacks: mealPlan.value.snacks
  }

  try {

    const existing = plans.value.find(p => p.goalType === goalType.value)

    if (existing) {
      const confirmUpdate = confirm(
        `A plan for "${goalType.value}" already exists. Do you want to replace it?`
      )
      if (!confirmUpdate) return


      await axios.delete(`http://localhost:8103/api/mealplans/${goalType.value}`)
      await axios.post('http://localhost:8103/api/mealplans', plan)
      alert(`Plan for "${goalType.value}" updated successfully!`)
    } else {
      await axios.post('http://localhost:8103/api/mealplans', plan)
      alert(`Plan for "${goalType.value}" created successfully!`)
    }


    const plansRes = await axios.get('http://localhost:8103/api/mealplans')
    plans.value = plansRes.data
    mealPlan.value = { breakfast: [], lunch: [], dinner: [], snacks: [] }
    goalType.value = ''
  } catch (err) {
    console.error(err)
    alert('Error saving or updating plan.')
  }
}


const selectPlan = (plan) => {
  selectedPlan.value = plan
}


const deletePlan = async (goalType) => {
  if (!confirm(`Are you sure you want to delete the "${goalType}" plan?`)) return

  try {
    await axios.delete(`http://localhost:8103/api/mealplans/${goalType}`)
    alert(`Plan for "${goalType}" deleted successfully!`)
    const res = await axios.get('http://localhost:8103/api/mealplans')
    plans.value = res.data
  } catch (err) {
    console.error(err)
    alert('Error deleting plan.')
  }
}
</script>

<style scoped>
.trainer-container {
  max-width: 850px;
}

select.form-select[multiple] {
  height: 120px;
}
</style>

