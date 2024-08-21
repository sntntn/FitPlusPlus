<template>
  <div>
    <div class="date-navigation">
      <button @click="changeWeek(-1)">&#9664;</button>
      <span>{{ formattedStartDate }} - {{ formattedEndDate }}</span>
      <button @click="changeWeek(1)">&#9654;</button>
    </div>

    <table class="schedule-table">
      <thead>
        <tr>
          <th></th>
          <th v-for="day in weekDays" :key="day">{{ day }}</th>
        </tr>
      </thead>
      <tbody style="height: 400px">
        <tr v-for="number in 48" :key="number">
          <!-- Only add the hour cell for every 4th row -->
          <td v-if="number % 4 === 1" :rowspan="4" style="font-size:8px; vertical-align: top">{{ calculateHour(number) }}</td>
          <td v-for="day in weekDays" :key="day + number" @click="addEvent(day, number)" class="time-slot" :class="{ 'active': isEvent(day, number) }">
            <div v-if="getEvent(day, number)">{{ getEvent(day, number).name }} - {{ getEvent(day, number).trainer }}</div>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>


<script>
  export default {
    data() {
      return {
        currentDate: new Date(),
        events: [],
        trainingType: 'Yoga',
        trainerName: 'John Doe',
      };
    },
    computed: {
      weekDays() {
        const startOfWeek = this.getStartOfWeek(this.currentDate);
        return Array.from({ length: 7 }, (_, i) => {
          const date = new Date(startOfWeek);
          date.setDate(date.getDate() + i);
          return date.toLocaleDateString('en-US', { weekday: 'long' });
        });
      },
      formattedStartDate() {
        console.log("Usao u formattedStartDate");
        const startOfWeek = this.getStartOfWeek(this.currentDate);
        return this.formatDate(startOfWeek);
      },
      formattedEndDate() {
        const startOfWeek = this.getStartOfWeek(this.currentDate);
        const endOfWeek = new Date(startOfWeek);
        endOfWeek.setDate(startOfWeek.getDate() + 6);
        return this.formatDate(endOfWeek);
      },
    },
    methods: {
      getStartOfWeek(date) {
        const day = date.getDay();
        const diff = date.getDate() - day + (day === 0 ? -6 : 1); 
        return new Date(date.setDate(diff));
      },
      formatDate(date) {
        return date.toLocaleDateString('en-US', {
          month: 'short',
          day: 'numeric',
          year: 'numeric',
        });
      },
      changeWeek(direction) {
        this.currentDate = new Date(this.currentDate.setDate(this.currentDate.getDate() + direction * 7));
      },
      isEvent(day, timeSlot) {
        const startOfWeek = this.getStartOfWeek(this.currentDate);
        const dayIndex = this.weekDays.indexOf(day);
        const date = new Date(startOfWeek);
        date.setDate(date.getDate() + dayIndex);
        return this.events.some(event => new Date(event.day).getTime() === date.getTime() && event.timeSlot === timeSlot);
      },
      getEvent(day, timeSlot) {
        const startOfWeek = this.getStartOfWeek(this.currentDate);
        const dayIndex = this.weekDays.indexOf(day);
        const date = new Date(startOfWeek);
        date.setDate(date.getDate() + dayIndex);
        return this.events.find(event => new Date(event.day).getTime() === date.getTime() && event.timeSlot === timeSlot);
      },
      addEvent(day, startSlot) {
        const duration = 4;
        const startOfWeek = this.getStartOfWeek(this.currentDate);
        const dayIndex = this.weekDays.indexOf(day);
        const date = new Date(startOfWeek);
        date.setDate(date.getDate() + dayIndex);
        for (let i = 0; i < duration; i++) {
          this.events.push({
            day: date.toString().split('T')[0], 
            timeSlot: startSlot + i,
            name: this.trainingType,
            trainer: this.trainerName
          });
        }
      },
      calculateHour(number) {
        const hours = Math.floor((number - 1) / 4) + 8;
        return hours.toString() + ':00';
      }
    },
    mounted(){
      this.$parent.$parent.$parent.setUserData(this.$route.params.id,"trainer");
    }
  };
</script>


<style scoped>
  .date-navigation {
    display: flex;
    justify-content: center;
    margin-bottom: 10px;
  }

    .date-navigation button {
      font-size: 16px;
      border: none;
      background-color: transparent;
      cursor: pointer;
    }

    .date-navigation span {
      font-size: 16px;
      margin: 0 10px;
    }

  .schedule-table {
    width: 100%;
    border-collapse: collapse;
    text-align: center;
  }

    .schedule-table th {
      border: 1px solid #ddd;
      padding: 0px;
      font-size: 14px;
    }

    .schedule-table td {
      border: 1px solid #ddd;
      padding: 0px;
      font-size: 7px;
      cursor: pointer;
      height: 10px;
    }

  .time-slot.active {
    background-color: #4CAF50;
    color: white;
  }

  .time-slot:hover {
    background-color: #e9e9e9;
  }

  .time-slot div {
    padding: 2px 5px;
    font-size: 8px;
    height: 100%;
    box-sizing: border-box;
  }
</style>
