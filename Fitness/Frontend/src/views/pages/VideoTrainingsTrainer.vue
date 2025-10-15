<template>

  <div class="buttons-grid">
    <button class="trainerBtn" @click="currentView = 'trainings'">Tvoji treninzi</button>
    <button class="trainerBtn" @click="currentView = 'videos'">Vežbe</button>
    <button class="trainerBtn" @click="currentView = 'addTraining'">Dodaj trening</button>
    <button class="trainerBtn" @click="currentView = 'addVideo'">Dodaj vežbu</button>
  </div>

  <div class="content">
    <div v-if="currentView === 'trainings'">
      <h3>Lista treninga</h3>
      
      <figure v-if="trainingExercises && trainingExercises.length > 0" class="trainings" v-for="training in trainings">
        <figcaption> {{ training.type }}</figcaption>
        <p :style="{ fontSize: '20px', fontColor: 'black' }"> {{ training.description }}
        </p>

        <div v-for="(exercise, index) in trainingExercises.filter(e => e.trainingId === training.trainingId)" :key="index" class="exercise-item">
          <span>{{ index + 1 }}.{{ getExerciseName(exercise.exerciseId) }} - {{ exercise.exerciseReps }} ponavljanja, {{ exercise.setReps }} {{ exercise.setReps < 5 ? 'seta ' : 'setova'}}</span>
        </div>

        <button class="delete-btn" @click="deleteTraining(training.trainingId)">
          Obriši trening
        </button>
      </figure>

    </div>

    <div v-if="currentView === 'videos'" class="videos-container">
      <h3>Lista tvojih video vežbi</h3>
      <br></br>

      <div v-if="exercises.length === 0">
        <p>Nema unetih vežbi.</p>
      </div>

      <div v-else class="exercise-list">
        <div v-for="exercise in exercises" :key="exercise.Id" class="exercise-card">
          <h5>{{ exercise.name }}</h5>
          <div class="video-box">

            <video 
              v-if="exercise.path" 
              :src="`http://localhost:8007/uploads/${exercise.path}`" 
              controls>
            </video>

            <p v-else>Video nije dostupan {{exercise.path}}</p>
          </div>

          <div class="exercise-actions">
            <button @click="deleteVideoExercise(exercise.id)" class="delete-btn">Obriši</button>
          </div>
        </div>
      </div>
    </div>

    <div v-if="currentView === 'addTraining'">
      <h3>Dodaj novi trening</h3>
      <br>

      <form @submit.prevent="saveTraining">
        <div class="training-form">

          <div class="header">
            <div>
              <label for="training-type">Tip treninga:</label>
              <input
                id="training-type"
                v-model="trainingType"
                type="text"
                placeholder="Unesite tip treninga"
              />
            </div>

            <button @click="addNewExercise" :disabled="isFormActive" class="add-button">
              Dodaj novu vežbu
            </button>

            <div class="form-actions">
              <button type="submit" :disabled="!trainingExercises.length || isFormActive">Sačuvaj ceo trening</button>
            </div>
          </div>

          <div class="note-container">
            <label for="note">Dodaj opis treninga:</label>
            <textarea
              id="note"
              v-model="note"
              placeholder="Opis..."
              class="note-textarea"
            ></textarea>
          </div>

          <div v-if="trainingExercises.length">
            <h3>Dodate vežbe:</h3>
            <div v-for="(exercise, index) in trainingExercises" :key="index" class="exercise-item">
              <div v-if="exercise.isEditing">

                <div>
                  <label for="exercise">Vežba:</label>
                  <select v-model="exercise.selectedExercise">
                    <option v-for="ex in exercises" :key="ex" :value="ex.id">{{ ex.name }}</option>
                  </select>
                </div>

                <div>
                  <label for="reps">Broj ponavljanja:</label>
                  <select v-model="exercise.reps">
                    <option v-for="n in repsOptions" :key="n" :value="n">{{ n }}</option>
                  </select>
                </div>

                <div>
                  <label for="sets">Broj setova:</label>
                  <select v-model="exercise.sets">
                    <option v-for="n in setsOptions" :key="n" :value="n">{{ n }}</option>
                  </select>
                </div>

                <button @click="saveExercise(index)">Sačuvaj</button>
                <button @click="cancelEdit(index)">Otkaži</button>
              </div>
                
              <div v-else>
                <span>{{ getExerciseName(exercise.selectedExercise) }} - {{ exercise.reps }} ponavljanja, {{ exercise.sets }} {{ exercise.sets < 5 ? 'seta ' : 'setova'}} </span>
                <button @click="editExercise(index)" type="button">Izmeni</button>
                <button @click="deleteExercise(index)" type="button">Obriši</button>
              </div>
            </div>
          </div>

          <div v-if="isAddingNewExercise">

            <div>
              <label for="exercise">Vežba:</label>
              <select v-model="newExercise.selectedExercise"> 
                <option v-for="ex in exercises" :key="ex" :value="ex.id">{{ ex.name }}</option>
              </select>
            </div>

            <div>
              <label for="reps">Broj ponavljanja:</label>
              <select v-model="newExercise.reps">
                <option v-for="n in repsOptions" :key="n" :value="n">{{ n }}</option>
              </select>
            </div>

            <div>
              <label for="sets">Broj setova:</label>
              <select v-model="newExercise.sets">
                <option v-for="n in setsOptions" :key="n" :value="n">{{ n }}</option>
              </select>
            </div>
            <button @click="saveNewExercise" type="button" 
                :disabled="!newExercise.selectedExercise || !newExercise.reps || !newExercise.sets">Sačuvaj</button>
            <button @click="cancelNewExercise" type="button">Otkaži</button>
          </div>
        </div>
      </form>
    </div>

    <div v-if="currentView === 'addVideo'">
      <h3>Dodaj novi video</h3>

      <form @submit="videoSaved" class="videoForma">
        <input type="text" v-model="exerciseName" placeholder="Naziv vežbe" />

        <input 
          type="file" 
          id="videoUpload" 
          accept="video/*" 
          @change="handleFileUpload" 
          hidden
        />

        <label for="videoUpload" class="upload-box">
          <div class="upload-content">
            ⬆️
            <p>Upload Video</p>
          </div>
        </label>

        <div v-if="selectedFile" class="file-info">
            <p>Izabran file: {{ selectedFile.name }}</p>
        </div>

        <button type="submit" >Sačuvaj</button>
      </form>
    </div>
      
  </div>

</template>

<script>

import dataServices from "@/services/data_services";

export default {
  name: "VideoTrainings",

  data(){
    return{
      isAvailable: false,
      currentView: null,

      selectedFile: "",
      exerciseName: "",
      
      trainings: [],
      exercises: [],

      trainingType: '',
      trainingExercises: [],
      isAddingNewExercise: false, 
      isFormActive: false, 
      originalExerciseData: {} ,
      note: '',
      repsOptions: [1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20], 
      setsOptions: [1,2,3,4,5,6,7,8,9,10], 
      newExercise: {
        selectedExercise: '',
        reps: 0,
        sets: 0
      }

    }
  },

  methods: {

    async loadExercises() {

      try {
        const id = this.$route.params.id;
        dataServices.methods.get_exercises_by_trainer(id).then(response => {
            if(response.data != null){
              this.exercises = response.data;
            }
          }
        );
      } catch (error) {
        console.error("Greška pri dohvatanju vežbi:", error);
      }
    },

    handleFileUpload(event) {
      this.selectedFile = event.target.files[0];
      console.log(this.selectedFile);
    },

    async videoSaved(e){
      e.preventDefault();

      const uploadResult =  dataServices.methods.upload_video(this.selectedFile);

      const id = this.$route.params.id;

      const exercise = {
        Id: "",
        Name: this.exerciseName,
        TrainerId: id,
        Path: this.selectedFile.name
      };

      try {
        const response = dataServices.methods.create_exercise(exercise);  
        alert("Vežba sačuvana!");
      } catch (error) {
        console.error("Greška prilikom čuvanja vežbe:", error);
      }

      this.exerciseName = "";
      this.selectedFile = null;

      const fileInput = document.getElementById("videoUpload");
      if (fileInput) {
        fileInput.value = "";
      }

      this.loadExercises();
    },

    addNewExercise() {
      if (!this.isFormActive) {
        this.isAddingNewExercise = true;
        this.isFormActive = true;
      }
    },

    saveNewExercise() {
      this.trainingExercises.push({
        ...this.newExercise,
        isEditing: false
      });
      this.isAddingNewExercise = false;
      this.isFormActive = false;
      this.resetNewExercise();
    },

    cancelNewExercise() {
      this.isAddingNewExercise = false;
      this.isFormActive = false;
      this.resetNewExercise();
    },

    editExercise(index) {
      if (!this.isFormActive) {
        this.originalExerciseData = { ...this.trainingExercises[index] };
        this.trainingExercises[index].isEditing = true;
        this.isFormActive = true;
      }
    },

    saveExercise(index) {
      this.trainingExercises[index].isEditing = false;
      this.isFormActive = false;
    },

    cancelEdit(index) {
      this.trainingExercises[index] = { ...this.originalExerciseData };
      this.trainingExercises[index].isEditing = false;
      this.isFormActive = false;
    },

    deleteExercise(index) {
      this.trainingExercises.splice(index, 1);
    },

    resetNewExercise() {
      this.newExercise = {
        selectedExercise: '',
        reps: 0,
        sets: 0
      };
    },

    async saveTraining() {
      
      const trainerId = this.$route.params.id;

      const training = {
        TrainingId: "",
        TrainerId: trainerId,
        Type: this.trainingType,
        Description: this.note,
        ClientIds: []
      }

      const response = await dataServices.methods.create_training(training);
      const trainingId = response.data.trainingId;
      
      try {
        for (let i = 0; i < this.trainingExercises.length; i++) {
          const ex = this.trainingExercises[i];

          const trainingExercise = {
            Id: "",
            TrainerId: trainerId,
            TrainingId: trainingId,
            ExerciseId: ex.selectedExercise,
            ExerciseReps: Number(ex.reps),
            SetReps: Number(ex.sets),
            Set: Number(i + 1)
          };

          const response = dataServices.methods.create_exercises_for_training(trainingExercise).then(res => {
              console.log("Sačuvano:", res.data);
            })
            .catch(err => {
              console.error("Greška pri čuvanju vežbe:", err);
            });
        }
      } catch (err) {
        console.error("Greška pri čuvanju vezbe:", err);
      }

      console.log(this.trainingExercises);
      alert("Trening sačuvan!");

      this.resetForm();
    },

    resetForm() {
      this.trainingType = '';
      this.note = '';
      this.trainingExercises = [];
      this.resetNewExercise();
    },

    async loadTrainings(){
       try {
        const trainerId = this.$route.params.id; 

        const response = await dataServices.methods.get_trainings_trainer(trainerId);
        if (response.data) {
          this.trainings = response.data;
        }
      } catch (error) {
        console.error("Greška pri dohvatanju treninga:", error);
      }

      this.trainingExercises = [];
      
      for (const training of this.trainings) {
        try {
          const exercisesResponse = await dataServices.methods.get_training_exercises(training.trainingId);
          if (exercisesResponse.data) {
            this.trainingExercises.push(...exercisesResponse.data);
          }
        } catch (error) {
          console.error("Greška pri dohvatanju vezbi iz treninga: ", training.trainingId , " ", error);
        }
      }
    },

    async deleteTraining(trainingId){

      try{
        const response = await dataServices.methods.delete_training(trainingId);
      } catch {
        console.error("Greška pri brisanju treninga: ", trainingId);
      }

      try{
        const response = await dataServices.methods.delete_training_exercises(trainingId);
      } catch {
        console.error("Greška pri brisanju vezbi treninga: ", trainingId);
      }

      alert('Obrisali ste trening!');
      this.trainingExercises = this.trainingExercises.filter(tr => tr.Id !== trainingId);
      this.loadTrainings();
    },

    getExerciseName(id) {
      const ex = this.exercises.find(e => e.id === id);
      return ex ? ex.name : '';
    },

    deleteVideoExercise(exerciseId) {
      const exPath = this.exercises.find(e => e.id === exerciseId).path;

      try{
        const response = dataServices.methods.delete_exercise(exerciseId);
      }catch{
        console.error("Greška pri brisanju vezbe: ", exerciseId);
      }

      try{
        const response = dataServices.methods.delete_video(exPath);
        this.exercises = this.exercises.filter(ex => ex.path !== exPath)
      }catch{
        console.error("Greška pri brisanju vezbe: ", exerciseId);
      }

    },

    getTrainingImage(trainingType) {
      const pic = this.trainingPics.find(p => p.name === trainingType);
      return pic ? pic.type : '';
    }
  },

  watch: {
    currentView(newVal) {
      
      this.trainingExercises = [];

      if(newVal === "trainings"){
        this.loadTrainings();
      }
    }
  },

  beforeMount() {
  },

  mounted() {
    this.$parent.$parent.$parent.setUserData(this.$route.params.id, "trainer");
    this.loadExercises();
  }
}

</script>

<style scoped>

  .buttons-grid {
    display: grid;
    grid-template-columns: 1fr 1fr;
    gap: 20px;
    margin-bottom: 30px;
  }

  button {
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

  button:hover {
    background-color: #e67e22;       
    color: #fff;                           
    transform: translateY(-2px);
  }

  .content {
    width: 100%;
    max-width: 100%;
    padding: 20px;
    border: 1px solid #ddd;
    border-radius: 8px;
    background: #f9f9f9;
  }

  .upload-wrapper {
    display: flex;
    flex-direction: column;
    align-items: center;
  }

  .upload-box {
    width: 220px;
    height: 220px;
    border: 2px dashed #e67e22;     
    border-radius: 12px;
    background-color: #fff;              
    display: flex;
    justify-content: center;
    align-items: center;
    cursor: pointer;
    transition: border-color 0.3s, background 0.3s, transform 0.2s;
    box-shadow: 0 2px 6px rgba(0,0,0,0.1);
  }

  .upload-box:hover {
    border-color: #d35400;     
    background: #fafafa;
    transform: translateY(-3px);           
  }

  .upload-content {
    text-align: center;
    color: #555;
    font-size: 16px;
  }
  .upload-content p {
    margin-top: 5px;
    font-weight: 500;
  }

  .file-info {
    margin-top: 10px;
    padding: 10px 15px;
    border: 1px solid #ddd;
    border-radius: 8px;
    background: #f9f9f9;
    color: #333;
    font-size: 14px;
    box-shadow: 0 1px 4px rgba(0,0,0,0.05);
  }
  .file-info p {
    margin: 0;
  }

  .training-form {
    width: 80%;
  }

  .header {
    width: 80%;
    display: flex;
    align-items: center;   
    gap: 20px;
    flex-wrap: nowrap
  }

  button {
    margin: 5px;
  }

  input, select {
    margin: 5px;
  }

  .exercise-list {
    display: flex;
    flex-wrap: wrap;       
    justify-content: center; 
    gap: 20px;               
  }

  .exercise-card {
    flex: 0 1 45%;           
    background: #f8f8f8;
    padding: 15px;
    border-radius: 10px;
    box-shadow: 0 2px 6px rgba(0,0,0,0.1);
    text-align: center;
    border: 2px solid #e67e22;     
    box-shadow: 0 2px 6px rgba(0,0,0,0.1);  
    text-align: center;
    transition: transform 0.2s, box-shadow 0.3s, border-color 0.3s;
  }

  .exercise-card:hover {
    transform: translateY(-4px);                          
    box-shadow: 0 6px 12px rgba(0,0,0,0.15);             
    border-color: #d35400;                     
  }

  .video-box video {
    width: 100%;
    height: 350px;
    border-radius: 6px;
  }

  .note-container {
    margin-top: 20px;
  }

  .note-container label {
    display: block;
    margin-bottom: 5px;
  }

  input[type="text"], .note-textarea {
    width: 80%;
    padding: 10px 14px;
    margin: 10px 0;
    font-size: 16px;
    color: #333;
    background: #fff;
    
    border: 2px solid #ccc;          
    border-radius: 8px;              
    box-shadow: 0 2px 4px rgba(0,0,0,0.05);

    transition: border-color 0.3s, box-shadow 0.3s;
  }

  input[type="text"]:focus, .note-textarea{
    width: 80%;
    outline: none;
    border-color: #d35400;     
    box-shadow: 0 0 6px rgba(230, 126, 34, 0.4); 
  }

  .videoForma {
    width: 50%;
  }

  figure.trainings {
      position: relative;
      width: 80%;
      padding: 15px;
      margin: 15px;
      display: block;
      vertical-align: top;
      background-size: cover;
      opacity: 0.9;
      border-radius: 10px;
      border: 2px solid #d35400        
  }

  figure.trainings figcaption {
      font-size: 22px;
      font-weight: bold;
      color: black;
      margin-bottom: 5px;
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

  .delete-btn {
    display: block;        
    margin-left: auto;     
    right: 10px;           
    padding: 6px 12px;
    border-radius: 6px;
    cursor: pointer;
    transition: background-color 0.2s;
  }

  .delete-btn:hover {
    background-color: #c1121f;
  }

</style>
