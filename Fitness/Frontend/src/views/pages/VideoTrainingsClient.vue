<template>

  <div class="buttons-grid">
    <button class="clientBtn" @click="currentView = 'available'">Dostupni treninzi</button>
    <button class="clientBtn" @click="currentView = 'bought'">Kupljeni treninzi</button>
  </div>
  
  <div class="content">

    <div v-if="currentView === 'available'">
  
      <figure v-if="trainings && trainings.length > 0" class="trainings" v-for="training in trainings"  :key="training.trainingId" :style="{ backgroundImage: 'url(' + getTrainingImage(training.type) + ')' }">
        <figcaption > {{ training.type }}</figcaption>

        <div class="training-desc">
          <p :style="{ fontSize: '20px', color: 'black' }"> {{ training.description }}
          </p>
        </div>

        <div class="trainer-info">
          <p :style="{ fontSize: '20px', color: 'black' }"> Trener: {{ getTrainerName(training.trainerId) }}
          </p>
          <p :style="{ fontSize: '20px', color: 'black' }"> Ocena: {{ getAverageRating(training.trainerId) }}
          </p>
        </div>

        <button class="overlay" @click="handleTrainingClick(training)"> Buy training </button>
      </figure>

    </div>

    <div v-if="currentView === 'bought'">

      <figure v-if="purchasedTrainings && purchasedTrainings.length > 0" class="trainings" v-for="training in purchasedTrainings"  :key="training.trainingId" :style="{ backgroundImage: 'url(' + getTrainingImage(training.type) + ')' }">
        <figcaption > {{ training.type }}</figcaption>

        <div class="training-desc">
          <p :style="{ fontSize: '20px', color: 'black' }"> {{ training.description }}
          </p>
        </div>

        <div class="trainer-info">
          <p :style="{ fontSize: '20px', color: 'black' }"> Trener: {{ getTrainerName(training.trainerId) }}
          </p>
          <p :style="{ fontSize: '20px', color: 'black' }"> Ocena: {{ getAverageRating(training.trainerId) }}
          </p>
        </div>

        <button class="overlay" @click="handleTrainingClick(training)"> View training </button>
      </figure>
      
    </div>

  </div>


  <div v-if="currentView === 'bought' && expandedTrainingId !== null" class="content" @click.self="closeModal">
    <div class="training-exercise">
      <h3>Trening: {{ expandedTraining.type }}</h3>

      <p :style="{ fontSize: '20px', fontColor: 'black' }"> {{ expandedTraining.description }}
      </p>

      <div v-for="(exercise, index) in getExercisesForTraining(expandedTrainingId)" :key="exercise.exerciseId" class="exercise-item">
        <span>{{ index + 1 }}.{{ getExerciseName(exercise.exerciseId) }} - {{ exercise.exerciseReps }} ponavljanja, {{ exercise.setReps }} {{ exercise.setReps < 5 ? 'seta ' : 'setova'}}</span>
        <button class="video-btn" @click="getExercise(exercise.exerciseId)">Prikaži video</button>
      </div>
      <button @click="expandedTrainingId = null; viewExerciseVideo = null" class="close-btn">Zatvori</button>
    </div>

    <div v-if="viewExerciseVideo !== null" class="video-box">
      <video 
        v-if="viewExerciseVideo.path" 
        :src="`http://localhost:8007/uploads/${viewExerciseVideo.path}`" 
        controls>
      </video>
      <p v-else>Video nije dostupan {{exercise.path}}</p>
      <button class="video-btn" @click="viewExerciseVideo = null">Zatvori video</button>
    </div>

  </div>

</template>

<script>
import dataServices from "@/services/data_services";

export default {
  name: "VideoTrainings",

  data(){
    return {
      currentView: null,

      isAvailable: false,
      trainers: [],
      trainings: [], 
      trainingPics: [ { name: 'Kardio', type: require('@/assets/images/running.jpeg')}, 
        { name: 'Snaga', type: require('@/assets/images/strength.jpg')}, 
        { name: 'Pilates', type: require('@/assets/images/yoga.jpg')}, 
        {name: 'Mix', type: require('@/assets/images/mix.jpg')} ],

      purchasedTrainings: [],
      expandedTrainingId: null,
      expandedTraining: null,
      trainingExercises: [],
      exercises: [], 

      viewExerciseVideo: null
    }
  },

  methods: {
    async loadTrainings(){
      const clientId = this.$route.params.id;

      try {

        const response = await dataServices.methods.get_trainings_client();
        if (response.data) {
          this.trainings = response.data;
        }
      } catch (error) {
        console.error("Greška pri dohvatanju treninga:", error);
      }

      try{
        const response2 = await dataServices.methods.get_trainers();
        if( response2 != null){
          this.trainers = response2.data;
        }
      } catch (error) {
        console.error("Greška pri dohvatanju treninga:", error);
      }

      this.trainings = this.trainings.filter(t => !t.clientIds || !t.clientIds.includes(clientId));

      for(const trainer of this.trainers){
        try {
          dataServices.methods.get_exercises_by_trainer(trainer.id).then(response => {
              if(response.data != null){
                 this.exercises.push(...response.data);
              }
            }
          );
        } catch (error) {
          console.error("Greška pri dodavanju vežbi:", error);
        }
      }
    
    },

    async loadPurchased(){
      const clientId = this.$route.params.id;
      try {
        const response = await dataServices.methods.get_purchased_trainings(clientId);
        if (response.data) {
          this.purchasedTrainings = response.data;
          this.purchasedTrainings.forEach(async training => {
            try {
              const exercisesResponse = await dataServices.methods.get_training_exercises(training.trainingId);
              if (exercisesResponse.data) {
                this.trainingExercises.push(...exercisesResponse.data);
              }
            } catch (error) {
              console.error("Greška pri dohvatanju vezbi iz treninga: ", training.trainingId , " ", error);
            }
          })
        }
      } catch (error) {
        console.error("Greška pri dohvatanju treninga za klijenta:", error);
      }
    },

    getTrainingImage(trainingType) {
      const pic = this.trainingPics.find(p => p.name === trainingType);
      return pic ? pic.type : '';
    },

    getTrainerName(trainerId){
      if (!this.trainers || this.trainers.length === 0) {
        return "Učitavanje...";
      }

      const trainer = this.trainers.find(p => p.id === trainerId);
      return trainer.fullName;
    },

    getAverageRating(trainerId){
      if (!this.trainers || this.trainers.length === 0) {
        return "Učitavanje...";
      }

      const trainer = this.trainers.find(p => p.id === trainerId);
      return trainer.averageRating;
    },

    async initiatePayment(videoData, price) {
      try {
        let trainer = await dataServices.methods.get_trainer_by_id(videoData.trainerId)
        const request = {
          id: "",
          userId: videoData.clientId,
          amount: price,
          currency: "USD",
          trainerPayPalEmail: trainer.data.contactEmail
        };
        let response = await dataServices.methods.create_payment(request);

        const paymentId = response.data.payment.id;
        const approvalUrl = response.data.paymentLink;

        console.log("Payment initiated with ID:", paymentId);
        window.location.href = approvalUrl;
      } catch(error) {
        console.error("Error initiating payment:", error);
        alert("Failed to initiate payment.");
        return false;
      }
    },

    async handleTrainingClick(training) {

      const clientId = this.$route.params.id;

      if (!this.isPurchased(training)) {
        if (confirm(`Da li želiš da kupiš trening?`)) {
          const videoData = {
            trainingId: training.trainingId,
            trainerId: training.trainerId,
            clientId: clientId
          };
          const price = 60; // USD
          
          sessionStorage.setItem("videoData", JSON.stringify(videoData));

          return this.initiatePayment(videoData, price);
        }
      } else {
        this.expandedTrainingId = training.trainingId;
        this.expandedTraining = this.purchasedTrainings.find(e => e.trainingId === training.trainingId);
      }

    },

    isPurchased(training) {
      return this.purchasedTrainings.some(t => t.trainingId === training.trainingId);
    },

    getExercisesForTraining(trainingId) {
      console.log(this.trainingExercises);
      return this.trainingExercises.filter(
        (exercise) => exercise.trainingId === trainingId);
    },

    getExerciseName(exerciseId) {
      const ex = this.exercises.find(e => e.id === exerciseId);
      return ex ? ex.name : '';
    },

    getExercise(exerciseId){
      const ex = this.exercises.find(e => e.id === exerciseId);
      this.viewExerciseVideo = ex;
    }

  },

  beforeMount() {
  },

  mounted() {
    const id = this.$route.params.id;
    this.$parent.$parent.$parent.setUserData(id, "client");
    this.loadTrainings();
    this.loadPurchased();
  },

  

}

</script>

<style scoped>

  .content {
    width: 100%;
    max-width: 100%;
    padding: 20px;
    border: 1px solid #ddd;
    border-radius: 8px;
    background: #f9f9f9;

    display: grid;
    grid-template-columns: repeat(2, 1fr); 
    justify-items: center;
    align-items: start; 
    gap: 25px; 
  }
  
  h3 {
    text-align: left;
  }

  .trainings {
    position: relative;
    width: 100%; 
    height: 350px;
    background-size: cover;
    background-position: center;
    border-radius: 10px;
    display: inline-block;
    vertical-align: top; 
    margin: 5px; 
    flex-direction: column;
    justify-content: space-between; 
    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
    overflow: hidden;
    opacity: 0.9;
    transition: transform 0.3s ease, box-shadow 0.3s ease;
  }

  .trainings:hover {
    transform: scale(1.02);
    box-shadow: 0 6px 12px rgba(0, 0, 0, 0.3);
  }

  .trainings::before {
    content: "";
    position: absolute;
    inset: 0;
    background-color: rgba(255, 255, 255, 0.5); 
    z-index: 0;
  }

  .trainings * {
    position: relative;
    z-index: 1;
  }

  figcaption {
    text-align: center;
    font-size: 22px;
    font-weight: bold;
    padding: 10px;
    margin-top: 10px;
    border-radius: 6px;
    align-self: center;
    width: fit-content;
  }

  .training-desc {
    background: rgba(255, 255, 255, 0.6);
    border-radius: 6px;
    padding: 10px;
    margin: 0 10px;
  }

  .trainer-info {
    background: rgba(255, 255, 255, 0.6);
    border-radius: 6px;
    padding: 10px;
    margin: 0 10px 10px 10px;
  }

  .overlay {
    position: absolute;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -50%);
    padding: 12px 22px;
    border: none;
    border-radius: 8px;
    background-color: rgba(0, 0, 0, 0.7);
    color: white;
    font-size: 18px;
    cursor: pointer;
    opacity: 0;
    pointer-events: none;
    transition: opacity 0.3s ease, transform 0.3s ease;
  }

  .trainings:hover .overlay {
    opacity: 1;
    pointer-events: all;
  }

  .exercises-section {
    background: rgba(255, 255, 255, 0.9);
    padding: 15px;
    border-radius: 8px;
    margin-top: 10px;
  }

  .training-exercise {
      position: relative;
      width: 95%;
      padding: 15px;
      margin: 15px;
      display: block;
      vertical-align: top;
      background-size: cover;
      opacity: 0.9;
      border-radius: 10px;
      border: 2px solid #d35400        
  }

  .exercise-item {
      font-size: 18px;
      color: #222;
      margin-left: 10px;
      padding: 3px 0;
      border-bottom: 1px solid #ccc; 
  }

  .exercise-item:last-child {
      border-bottom: none;
  }

  .close-btn, .video-btn{
    display: block;        
    margin-left: auto;     
    right: 10px;   
    padding: 6px 12px;
    border-radius: 8px;
    font-size: 16px;
    font-weight: 500;
    cursor: pointer;
    background-color: #fff;
    color: #333;
    border: 2px solid #e67e22;
    box-shadow: 0 2px 4px rgba(0,0,0,0.1);
    transition: background 0.3s, color 0.3s, transform 0.2s;
  }

  .buttons-grid {
    display: grid;
    grid-template-columns: 1fr 1fr;
    gap: 20px;
    margin-bottom: 30px;
  }

  .clientBtn {
    margin: 5px;
    padding: 10px 18px;
    border-radius: 8px;
    font-size: 16px;
    font-weight: 500;
    cursor: pointer;
    background-color: #fff;
    color: #333;
    border: 2px solid #e67e22;
    box-shadow: 0 2px 4px rgba(0,0,0,0.1);
    transition: background 0.3s, color 0.3s, transform 0.2s;
  }

  .video-box, video{
    position: relative;
    padding: 15px;
    width: 100%;
    height: 350px;
    border-radius: 10px;
  }

</style>
