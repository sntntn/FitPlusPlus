<template>
  <div>
    <table class="schedule-table">
      <thead>
        <tr>
          <th></th>
          <th v-for="day in days" :key="day">{{ day }}</th>
        </tr>
      </thead>
      <tbody style="height: 400px">
        <tr v-for="number in 48" :key="number">
          <!-- Only add the hour cell for every 4th row -->
          <td v-if="number % 4 === 1" :rowspan="4" style="font-size:8px; vertical-align: top">{{ calculateHour(number) }}</td>
          <td v-for="day in days" :key="day + number" @click="addEvent(day, number)" class="time-slot" :class="{ 'active': isEvent(day, number) }">
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
        days: ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday', 'Sunday'],
        hours: ['10:00', '11:00', '12:00', '13:00', '14:00', '15:00', '16:00', '17:00', '18:00', '19:00', '20:00', '21:00'],
        events: [], // Events: { day: 'Monday', timeSlot: 12, duration: 6, name: 'Training', trainer: 'Trainer' }
        trainingType: 'Yoga', // Example training type
        trainerName: 'John Doe' // Example trainer name
      };
    },
    methods: {
      // Check if there's an event in the specific time slot
      isEvent(day, timeSlot) {
        return this.events.some(event => event.day === day && event.timeSlot === timeSlot);
      },
      // Get the event in the specific time slot
      getEvent(day, timeSlot) {
        return this.events.find(event => event.day === day && event.timeSlot === timeSlot);
      },
      // Add a new event
      addEvent(day, startSlot) {
        const duration = 4; // Duration in 15-minute intervals (4 intervals = 1 hour)
        for (let i = 0; i < duration; i++) {
          this.events.push({
            day: day,
            timeSlot: startSlot + i,
            name: this.trainingType,
            trainer: this.trainerName
          });
        }
      },
      calculateHour(number) {
        const hours = Math.floor((number - 1) / 4) + 10;
        return hours.toString() + ':00';
      }
    }
  };
</script>

<style scoped>
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