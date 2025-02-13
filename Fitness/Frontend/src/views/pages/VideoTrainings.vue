<template>
  <div>
    <p v-if="isAvailable"> {{ message }} </p>
    <p v-else> {{ message }} </p>
  </div>

  <div id="wrapper">
    <h1> Available programs: </h1>
    <figure class="trainings" v-for="training in trainings" :style="{ backgroundImage: `url(${training.type})`}">
      <figcaption> {{ training.name }}</figcaption>
      <p :style="{ fontSize: '20px', fontColor: 'black' }">Svi imaju za sad isti opis.<br>
          Kao nesto pise.<br>
      </p>
      <button class="overlay" @click="buyProgram">See more</button>
    </figure>
  </div>
</template>

<script>
import dataServices from "@/services/data_services";

export default {
  name: "VideoTrainings",

  data(){
    return{
      isAvailable: false,
      message: 'Probna verzija!',
      trainings: [
        { name: 'Prvi', type: require('@/assets/images/running.jpeg')},
        { name: 'Drugi', type: require('@/assets/images/strength.jpg')},
        { name: 'Treci', type: require('@/assets/images/yoga.jpg')},
        { name: 'Cetvrti', type: require('@/assets/images/home.jpeg')},
        { name: 'Peti', type: require('@/assets/images/strength.jpg')},
        { name: 'Sesti', type: require('@/assets/images/running.jpeg')},
      ]
    }
  },

  methods: {
    async getTraining(){
      this.message = 'Radim!';

      /*await dataServices.methods.get_trainings_for_client(this.$route.params.id).then(
        response => {
          console.log(this.$route.params.id);
          this.trainings = response.data;
        }
      );*/


    },

    buyProgram(){
        alert("Kupili ste program!");
    }

  },

  beforeMount() {
    this.getTraining();
  },

  mounted() {
    this.$parent.$parent.$parent.setUserData(this.$route.params.id, "client");
  }
}

</script>

<style scoped>
  body {
    //background-image: url('../../assets/images/fitplusplus.jpeg');
    //background-size: cover;
  }

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

</style>
