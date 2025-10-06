<template>
  <div id="wrapper">
    <h3> Dostupni treninzi: </h3>

    <figure v-if="trainings && trainings.length > 0" class="trainings" v-for="training in trainings" :style="{ backgroundImage: 'url(' + getTrainingImage(training.type) + ')' }">
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

        <button class="overlay" @click="buyProgram">See more</button>
    </figure>

  </div>
</template>

<script>
import dataServices from "@/services/data_services";

export default {
  name: "VideoTrainings",

  data(){
    return {
      isAvailable: false,
      trainers: [],
      trainings: [], 
      trainingPics: [ { name: 'Kardio', type: require('@/assets/images/running.jpeg')}, 
        { name: 'Snaga', type: require('@/assets/images/strength.jpg')}, 
        { name: 'Pilates', type: require('@/assets/images/yoga.jpg')}, 
        {name: 'Mix', type: require('@/assets/images/mix.jpg')} ],
      }
  },

  methods: {

    buyProgram(){
        alert("Kupili ste program!");
    },

    async loadTrainings(){
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
    }

  },

  beforeMount() {
  },

  mounted() {

    const id = this.$route.params.id;

    this.$parent.$parent.$parent.setUserData(id, "client");

    this.loadTrainings();

  }
}

</script>

<style scoped>

.trainings {
  position: relative;
  width: 45%; 
  height: 350px;
  background-size: cover;
  background-position: center;
  border-radius: 10px;
  display: inline-block;
  vertical-align: top; 
  margin: 15px; 
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

</style>
