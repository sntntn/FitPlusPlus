<template>
  <div class="trainer-container container py-4">
    <h2 class="mb-3 text-center">Nutrition Plans</h2>

    <div class="card p-3 mb-4 shadow-sm">
      <h4 class="mb-3">Create or Update Plan</h4>

      <div class="form-group mb-3">
        <label>Trainer Name</label>
        <input v-model="trainerName" type="text" class="form-control" placeholder="Enter your name" required />
      </div>

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
        No nutrition plans created yet for this trainer.
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

<script>
import { 
  getAllFoods,
  getMealPlansForTrainer,
  saveMealPlan,
  deleteMealPlan
} from "../../services/NutritionService";

export default {
  data() {
    return {
      trainerId: null,
      trainerName: "",
      foods: [],
      plans: [],
      goalType: "",
      mealPlan: { breakfast: [], lunch: [], dinner: [], snacks: [] },
      selectedPlan: null,
      mealTypes: ["breakfast", "lunch", "dinner", "snacks"],
    };
  },

  created() {
    const routeId = this.$route.params.id || '';
    this.$parent.$parent.$parent.setUserData(routeId, "trainer");
    this.trainerId = routeId;
    this.loadData();
  },

  methods: {
    async loadData() {
      this.foods = await getAllFoods();
      if (this.trainerId) this.plans = await getMealPlansForTrainer(this.trainerId);
    },

    async savePlan() {
      if (!this.goalType || !this.trainerName) {
        alert("Please fill in trainer name and goal type.");
        return;
      }

      const plan = {
        trainerId: this.trainerId,
        trainerName: this.trainerName,
        goalType: this.goalType,
        ...this.mealPlan,
      };

      const existing = this.plans.find((p) => p.goalType === this.goalType);
      if (existing) {
        const confirmUpdate = confirm(
          `A plan for "${this.goalType}" already exists. Replace it?`
        );
        if (!confirmUpdate) return;
        await deleteMealPlan(this.trainerId, this.goalType);
      }

      await saveMealPlan(plan);
      alert(`Plan for "${this.goalType}" saved successfully!`);
      await this.loadData();

      this.mealPlan = { breakfast: [], lunch: [], dinner: [], snacks: [] };
      this.goalType = "";
      this.selectedPlan = null;
    },

    async deletePlan(goalType) {
      if (!confirm(`Are you sure you want to delete the "${goalType}" plan?`)) return;
      await deleteMealPlan(this.trainerId, goalType);
      alert(`Plan for "${goalType}" deleted successfully!`);
      await this.loadData();
    },
  },
};
</script>


<style scoped>
.trainer-container {
  max-width: 850px;
}

select.form-select[multiple] {
  height: 120px;
}
</style>
