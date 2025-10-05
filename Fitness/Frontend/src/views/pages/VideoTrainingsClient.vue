<template>
  <div id="wrapper">
    <h3> Dostupni treninzi: </h3>

    <figure v-if="trainers && trainers.length > 0" class="trainings" v-for="training in trainings" :style="{ backgroundImage: 'url(' + getTrainingImage(training.type) + ')' }">
        <figcaption :style="{background: 'rgba(255, 255, 255, 0.5)', 'border-radius': '5px'}"> {{ training.type }}</figcaption>

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

  figure {
    position: relative;
    height: 300px;
    width: 45%;
    padding: 10px;
    margin: 10px;
    display: inline-block;
    background-size: cover;
    opacity: 0.75;
    border-radius: 5px;
  }

  figure:hover .overlay{
    opacity: 0.5;
  }
  .overlay {
    position: absolute;
    top: 0;
    left: 0;
    right: 0;
    bottom: 0;
    opacity: 0;
    transition: 0.5s ease;
    background-color: lightcoral;
    border-color: black;
    align-items: center;
    font-size: 20px;
    color: black;
  }

  figcaption {
    font-size: 30px;
    color: black;
  }

  .trainer-info {
    position: absolute;
    bottom: 10px;     
    left: 10px;     
    right: 10px;
    background: rgba(255, 255, 255, 0.7); 
    border-radius: 5px;
    padding: 5px;
  }
  
  .training-desc {
    bottom: 10px;  
    left: 10px;        
    right: 10px;
    background: rgba(255, 255, 255, 0.5); 
    border-radius: 5px;
    padding: 5px;
  }

</style>
