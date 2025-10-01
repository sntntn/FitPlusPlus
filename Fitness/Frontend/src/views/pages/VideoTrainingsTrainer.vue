<template>

   <div class="buttons-grid">
      <button class="trainerBtn" @click="currentView = 'trainings'">Tvoji treninzi</button>
      <button class="trainerBtn" @click="currentView = 'videos'">Vežbe</button>
      <button class="trainerBtn" @click="currentView = 'addTraining'">Dodaj trening</button>
      <button class="trainerBtn" @click="currentView = 'addVideo'">Dodaj vežbu</button>
    </div>

    <!-- Ovde se prikazuje sadržaj -->
    <div class="content">
      <div v-if="currentView === 'trainings'">
        <h3>Lista treninga</h3>
        <p>Ovde ide sadržaj za treninge...</p>
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
              <!-- ako Path čuva samo ime fajla -->
              <video 
                v-if="exercise.path" 
                :src="`http://localhost:8004/uploads/${exercise.path}`" 
                controls>
              </video>
              <!-- ako nema video, prikaži samo placeholder -->
              <p v-else>Video nije dostupan {{exercise.path}}</p>
            </div>
          </div>
        </div>
      </div>

      <div v-if="currentView === 'addTraining'">
        <h3>Dodaj novi trening</h3>
        <br>

        <form @submit.prevent="submitForm">
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
              <!-- Dugme za dodavanje nove vezbe -->
              <button @click="addNewExercise" :disabled="isFormActive" class="add-button">
                Dodaj novu vežbu
              </button>

              <div class="form-actions">
                <button type="submit">Sačuvaj ceo trening</button>
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

            <!-- Lista vežbi koje su dodane -->
            <div v-if="trainingExercises.length">
              <h3>Dodate vežbe:</h3>
              <div v-for="(exercise, index) in trainingExercises" :key="index" class="exercise-item">
                <div v-if="exercise.isEditing">
                  <div>
                    <label for="exercise">Vežba:</label>
                    <select v-model="exercise.selectedExercise">
                      <option v-for="ex in availableTrainingExercises" :key="ex" :value="ex">{{ ex }}</option>
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
                    <span>{{ exercise.selectedExercise }} - {{ exercise.reps }} ponavljanja, {{ exercise.sets }} setova</span>
                    <button @click="editExercise(index)">Izmeni</button>
                    <button @click="deleteExercise(index)">Obriši</button>
                  </div>
              </div>
            </div>

              <!-- Novi unos za vezbe -->
              <div v-if="isAddingNewExercise">
                <div>
                  <label for="exercise">Vežba:</label>
                  <select v-model="newExercise.selectedExercise">
                    <option v-for="ex in availableTrainingExercises" :key="ex" :value="ex">{{ ex }}</option>
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
                <button @click="saveNewExercise">Sačuvaj</button>
                <button @click="cancelNewExercise">Otkaži</button>
              </div>
          </div>
          
        </form>
       </div>

      <div v-if="currentView === 'addVideo'">
        
        <h3>Dodaj novi video</h3>

        <form @submit="videoSaved">
          <input type="text" v-model="exerciseName" placeholder="Naziv vežbe" />

          <!-- Skriveni input za upload -->
          <input 
            type="file" 
            id="videoUpload" 
            accept="video/*" 
            @change="handleFileUpload" 
            hidden
          />

          <!-- Vidljiva siva kockica -->
          <label for="videoUpload" class="upload-box">
            <div class="upload-content">
              ⬆️
              <p>Upload Video</p>
            </div>
          </label>

          <!-- Prikaz fajla ako je izabran -->
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
import axios from 'axios';

export default {
  name: "VideoTrainings",

  data(){
    return{
      isAvailable: false,
      currentView: null,

      selectedFile: "",
      exerciseName: "",
      
      trainings: [],
      exercises: [], // Lista svih vežbi koje su dodate
      availableExercises: [
        { Id: 1, Name: 'Sklekovi', Path: 'file_example_MP4_480_1_5MG.mp4'},
        { Id: 2, Name: 'Čučnjevi', Path: 'file_example_MP4_480_1_5MG.mp4'},
        { Id: 3, Name: 'Mrtvo dizanje', Path: 'file_example_MP4_480_1_5MG.mp4'},
        { Id: 4, Name: 'Benč pres', Path: 'file_example_MP4_480_1_5MG.mp4'},
      ],

      trainingType: '',
      trainingExercises: [], // Lista dodanih vežbi
      isAddingNewExercise: false, // Da li je forma za novu vežbu otvorena
      isFormActive: false, // Da li je neka forma aktivna za editovanje
      originalExerciseData: {} ,
      note: '',
      availableTrainingExercises: ["Čučanj", "Sklek", "Mrtvo dizanje", "Benč pres", "Iskorak"], // Primer dostupnih vežbi
      repsOptions: [1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20], // Opcije za broj ponavljanja
      setsOptions: [1,2,3,4,5,6,7,8,9,10], // Opcije za broj setova
      newExercise: {
        selectedExercise: '',
        reps: 0,
        sets: 0
      }

    }
  },

  methods: {

    buyProgram(){
        alert("Kupili ste program!");
    },

    handleFileUpload(event) {
      this.selectedFile = event.target.files[0];
      console.log(this.selectedFile)
    },

    async videoSaved(e){
      e.preventDefault();

      const uploadResult =  dataServices.methods.upload_video(this.selectedFile);

      const id = this.$route.params.id;

      const exercise = {
        Id: "",
        Name: this.exerciseName,
        TrainerId: id,
        Path: this.selectedFile.name // za sad šalješ samo ime fajla
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
      
    },

    async loadExercises() {

      try {
        const id = this.$route.params.id; // ako želiš samo vežbe jednog trenera
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
        this.originalExerciseData = { ...this.exercises[index] };
        this.trainingExercises[index].isEditing = true;
        this.isFormActive = true;
      }
    },
    saveExercise(index) {
      this.trainingExercises[index].isEditing = false;
      this.isFormActive = false;
    },
    cancelEdit(index) {
      this.exercises[index] = { ...this.originalExerciseData };
      this.trainingExercises[index].isEditing = false;
      this.isFormActive = false;
    },
    deleteExercise(index) {
      this.trainingExercises.splice(index, 1);
    },
    resetNewExercise() {
      this.trainingExercises = {};
      this.note = '';
      this.newExercise = {
        selectedExercise: '',
        reps: 10,
        sets: 3
      };
    },

    submitForm() {
      // Ovde možete poslati podatke na server ili ih obraditi lokalno
      alert("Trening sačuvan!");

      // Resetovanje forme nakon submit-a
      this.resetForm();
    },

    resetForm() {
      this.trainingType = '';
      this.note = '';
      this.exercises = [];
      this.resetNewExercise();
    }
  },

  watch: {
    currentView(newVal) {
      if (newVal === "videos") {
        this.loadExercises();
      }
    }
  },

  beforeMount() {
  },

  mounted() {
    this.$parent.$parent.$parent.setUserData(this.$route.params.id, "trainer");
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
  
  /* Pozadina i boje */
  background-color: #fff;
  color: #333;

  /* Ivice i senka */
  border: 2px solid #e67e22; /* narandžasta ivica */
  box-shadow: 0 2px 4px rgba(0,0,0,0.1);

  /* Glatke tranzicije */
  transition: background 0.3s, color 0.3s, transform 0.2s;
}

button:hover {
  background-color: #e67e22;       /* unutrašnjost postaje narandžasta */
  color: #fff;                           /* tekst se okreće u belo */
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
  border: 2px dashed #e67e22;      /* narandžasta ivica */
  border-radius: 12px;
  background-color: #fff;                /* bela unutrašnjost */
  display: flex;
  justify-content: center;
  align-items: center;
  cursor: pointer;
  transition: border-color 0.3s, background 0.3s, transform 0.2s;
  box-shadow: 0 2px 6px rgba(0,0,0,0.1);
}

.upload-box:hover {
  border-color: #d35400;     /* tamnija narandžasta */
  background: #fafafa;
  transform: translateY(-3px);           /* blagi lift */
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
  max-width: 600px;
}

.header {
  display: flex;
  align-items: center;   /* poravnaj sve po visini */
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
  flex-wrap: wrap;        /* da prelazi u novi red */
  justify-content: center; /* centriraj po ekranu */
  gap: 20px;               /* razmak između kartica */
}

.exercise-card {
  flex: 0 1 45%;           /* svaka zauzima ~45% širine ekrana */
  background: #f8f8f8;
  padding: 15px;
  border-radius: 10px;

  box-shadow: 0 2px 6px rgba(0,0,0,0.1);
  text-align: center;
  
  /* Ivice i senka */
  border: 2px solid #e67e22;     /* naglašena ivica u narandžastoj */
  box-shadow: 0 2px 6px rgba(0,0,0,0.1);
  
  text-align: center;
  transition: transform 0.2s, box-shadow 0.3s, border-color 0.3s;
}

.exercise-card:hover {
  transform: translateY(-4px);                           /* blagi lift efekat */
  box-shadow: 0 6px 12px rgba(0,0,0,0.15);               /* jača senka */
  border-color: #d35400;                     /* tamnija narandžasta ivica */
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
  width: 100%;
  max-width: 400px;
  padding: 10px 14px;
  margin: 10px 0;
  font-size: 16px;
  color: #333;
  background: #fff;
  
  border: 2px solid #ccc;          /* osnovna siva ivica */
  border-radius: 8px;              /* blago zaobljene ivice */
  box-shadow: 0 2px 4px rgba(0,0,0,0.05);

  transition: border-color 0.3s, box-shadow 0.3s;
}

/* Efekat kad je u fokusu */
input[type="text"]:focus, .note-textarea{
  outline: none;
  border-color: #d35400;     /* narandžasta ivica */
  box-shadow: 0 0 6px rgba(230, 126, 34, 0.4); /* narandžasti glow */
}

</style>
