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
              Please calculate your goal first to choose a trainer and view a plan.
            </p>
          </div>

          <div v-else>
            <div class="mb-3">
              <label>Select Trainer</label>
              <select v-model="selectedTrainerName" class="form-select" @change="fetchPlan">
                <option value="">Select trainer...</option>
                <option v-for="t in trainers" :key="t.name" :value="t.name">
                  {{ t.name }}
                </option>
              </select>
            </div>

            <div v-if="!plan">
              <p class="text-muted text-center">
                No plan found for this trainer and goal type.
              </p>
            </div>

            <div v-else>
              <h5 class="text-center mb-3 text-capitalize">
                Plan for {{ plan.goalType }} ({{ selectedTrainerName }})
              </h5>

              <div v-for="mealType in mealTypes" :key="mealType" class="mb-3">
                <h6 class="text-capitalize">{{ mealType }}</h6>
                <ul>
                  <li v-for="food in plan[mealType]" :key="food.name">
                    {{ food.name }} — <small>{{ food.calories }} kcal</small>
                  </li>
                </ul>
              </div>

              <div class="mt-4">
                <h5 class="text-center mb-3">Calorie Intake Tracker</h5>
                <p class="text-center text-muted">
                  Enter how much (in grams) of each food you consumed:
                </p>

                <div v-for="mealType in mealTypes" :key="mealType + '-input'" class="mb-3">
                  <h6 class="text-capitalize">{{ mealType }}</h6>
                  <div v-for="food in plan[mealType]" :key="food.name + '-input'" class="d-flex align-items-center mb-2">
                    <div class="flex-grow-1">
                      {{ food.name }} — <small>{{ food.calories }} kcal/100g</small>
                    </div>
                    <input type="number" class="form-control ms-2" style="width: 90px"
                      v-model.number="foodQuantities[food.name]" @input="calculateConsumed" min="0" />
                    <span class="ms-2 text-muted small">
                      ≈ {{ ((foodQuantities[food.name] || 0) * (food.calories || 0) / 100).toFixed(0) }} kcal
                    </span>
                  </div>
                </div>

                <div class="text-center mt-3 border-top pt-3">
                  <p><strong>Total Consumed:</strong> {{ consumedCalories }} kcal</p>
                  <p>
                    <strong>Remaining:</strong>
                    {{ Math.max(0, calculatedGoal.targetKcal - consumedCalories) }} kcal left
                  </p>
                </div>
              </div>
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

<script>
import axios from "axios";

export default {
  data() {
    return {
      clientId: null,
      goal: {
        clientId: "",
        sex: "",
        age: null,
        height: null,
        currentWeight: null,
        activityLevel: "",
        goalType: "",
        intensity: "",
      },
      calculatedGoal: null,
      plan: null,
      trainers: [],
      selectedTrainerName: "",
      mealTypes: ["breakfast", "lunch", "dinner", "snacks"],
      consumedCalories: 0,
      foodQuantities: {},
    };
  },

  created() {
    const routeId = this.$route.params.id || "";
    this.$parent.$parent.$parent.setUserData(routeId, "client");
    this.clientId = routeId;
    this.goal.clientId = routeId;
    this.loadTrainers();

    this.$watch(
      () => this.$route.params.id,
      (newId, oldId) => {
        if (newId !== oldId) {
          const safeId = newId || "";
          console.log(`Client ID changed from ${oldId} to ${safeId}`);
          this.$parent.$parent.$parent.setUserData(safeId, "client");
          this.clientId = safeId;
          this.goal.clientId = safeId;
          this.loadTrainers();
        }
      }
    );
  },

  mounted() {
    console.log("ClientNutritionPlan mounted");
  },

  beforeDestroy() {
    console.log("ClientNutritionPlan destroyed");
  },

  methods: {
    async loadTrainers() {
      try {
        const res = await axios.get("http://localhost:8157/api/mealplans");
        const seen = new Map();
        for (const p of res.data) {
          if (!seen.has(p.trainerId)) {
            seen.set(p.trainerId, { id: p.trainerId, name: p.trainerName });
          }
        }
        this.trainers = Array.from(seen.values());
      } catch (err) {
        console.error("Error loading trainers:", err);
      }
    },

    async calculateGoal() {
      try {
        console.log("Calculating goal for client:", this.goal);
        const res = await axios.post("http://localhost:8157/api/goals", this.goal);
        this.calculatedGoal = res.data;
      } catch (error) {
        console.error("Error calculating goal:", error);
        alert("Error calculating goal.");
      }
    },

    async fetchPlan() {
      try {
        const trainer = this.trainers.find((t) => t.name === this.selectedTrainerName);
        if (!trainer) return;

        console.log("Fetching plan for:", trainer.id, this.goal.goalType);
        const res = await axios.get(
          `http://localhost:8157/api/MealPlans/trainer/${trainer.id}/goal/${this.goal.goalType}`
        );
        this.plan = res.data;
      } catch (error) {
        console.error("Error fetching plan:", error);
        this.plan = null;
      }
    },

    async refreshPlan() {
      await this.fetchPlan();
    },

    calculateConsumed() {
      if (!this.plan) return;
      let total = 0;
      for (const mealType of this.mealTypes) {
        for (const food of this.plan[mealType]) {
          const qty = parseFloat(this.foodQuantities[food.name]) || 0;
          total += (food.calories || 0) * (qty / 100);
        }
      }
      this.consumedCalories = Math.round(total);
    },
  },
};
</script>

<style scoped>
.client-container {
  max-width: 1000px;
}

ul {
  list-style-type: square;
  padding-left: 1.2rem;
}

input[type="number"] {
  text-align: center;
}
</style>
