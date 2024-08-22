<template>
  <div v-if="enableBooking" style="margin-bottom: 45px; border-bottom: 1px solid black; display: flex; flex-direction: column; align-items: center;">
    <h2>Book a training</h2>
    <h3>Trainer name: {{ trainerData.name }}</h3>
    <h5>Choose training type:</h5>
    <CFormSelect v-model="selectedType" style="width: 20%; margin-bottom: 5px;">
      <option value=""></option>
      <option v-for="type in trainerData.trainingTypes" :key="type" :value="type">
        {{ type }}
      </option>
    </CFormSelect>
  </div>

  <div>
    <div class="date-navigation">
      <button @click="changeWeek(-1)" :disabled="moveCounter == 0">&#9664;</button>
      <span>{{ formattedStartDate }} - {{ formattedEndDate }}</span>
      <button @click="changeWeek(1)" :disabled="moveCounter == 2">&#9654;</button>
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
          <td v-for="day in weekDays"
              :key="day + number"
              class="time-slot"
              :class="{ active: getEvent(day, number) }"
              @click="bookTraining(day, number)">
            <div v-if="getEvent(day, number)">
              {{ getEvent(day, number).name }} - {{ getEvent(day, number).type }}
            </div>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>



<script>
  import dataServices from '../../services/data_services';

  export default {
    data() {
      return {
        currentDate: new Date(),
        events: [],
        moveCounter: 0,
        enableBooking: false,
        selectedType: "",
        trainerData: {
          trainingTypes: [],
          name: ""
        }
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
      async fetchEvents() {
        try {
          const weekId = this.getWeekId(this.currentDate);
          const response = await dataServices.methods.get_client_week_schedule_by_id(weekId, this.$route.params.id);
          const data = response.data;

          if (data.dailySchedules) {
            const events = [];

            for (const [day, slots] of Object.entries(data.dailySchedules)) {
              for (const slot of slots) {
                if (!slot.isAvailable) {
                  const startSlot = this.timeToSlot(slot.startHour, slot.startMinute);
                  const endSlot = this.timeToSlot(slot.endHour, slot.endMinute);

                  const trainerName = slot.trainerName;

                  for (let slotNumber = startSlot; slotNumber < endSlot; slotNumber++) {
                    events.push({
                      day,
                      timeSlot: slotNumber,
                      name: trainerName,
                      type: `${slot.trainingType}`
                    });
                  }
                }
              }
            }

            this.events = events;
          } else {
            console.error('Invalid data structure:', data);
          }
        } catch (error) {
          console.error('Error fetching events:', error);
        }
      },
      getWeekId(date) {
        const startOfYear = new Date(date.getFullYear(), 0, 1);
        const daysBetween = Math.floor((date - startOfYear) / (1000 * 60 * 60 * 24));
        const weekNumber = Math.ceil((daysBetween + startOfYear.getDay() + 1) / 7) + 1;

        return weekNumber - this.getWeekNumber(new Date());
      },
      getWeekNumber(date) {
        const startOfYear = new Date(date.getFullYear(), 0, 1);
        const daysBetween = Math.floor((date - startOfYear) / (1000 * 60 * 60 * 24));
        return Math.ceil((daysBetween + startOfYear.getDay() + 1) / 7);
      },
      timeToSlot(hour, minute) {
        return Math.floor((hour - 8) * 4 + minute / 15 + 1);
      },
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
      async changeWeek(direction) {
        this.moveCounter += direction;
        this.currentDate = new Date(this.currentDate.setDate(this.currentDate.getDate() + direction * 7));
        await this.fetchEvents();
      },
      getEvent(day, timeSlot) {
        return this.events.find(event => event.day === day && event.timeSlot === timeSlot);
      },
      calculateHour(number) {
        const hours = Math.floor((number - 1) / 4) + 8;
        return hours.toString() + ':00';
      },
      async bookTraining(day, timeSlot) {
        if(!this.enableBooking)
          return;

        if (!this.selectedType) {
          alert('Please select a training type before booking.');
          return;
        }

        const startHour = Math.floor((timeSlot - 1) / 4) + 8;
        const startMinute = ((timeSlot - 1) % 4) * 15;
        const duration = this.selectedType.split('(')[1].split(')')[0];
        const trainingType = this.selectedType.split(' (')[0];

        const bookingData = {
          clientId: this.$route.params.id,
          trainerName: this.trainerData.name,
          trainerId: this.$route.params.trainerId,
          trainingType: trainingType,
          duration: duration,
          weekId: this.getWeekId(this.currentDate),
          dayName: day,
          startHour: startHour,
          startMinute: startMinute,
          isBooking: true
        };

        try {
          const response = await dataServices.methods.booking(bookingData);
          if (response.status === 200) {
            this.fetchEvents();
          } else {
            alert('This time slot is already booked. Please choose other time.')
          }
        } catch (error) {
          console.error('Error during booking:', error);
          alert('Failed to book the training.');
        }
      }
    },
    mounted() {
      this.fetchEvents();
      this.$parent.$parent.$parent.setUserData(this.$route.params.id, "client");

      const trainerId = this.$route.params.trainerId;
      if (trainerId) {
        this.enableBooking = true;
        dataServices.methods.get_trainer_by_id(trainerId)
          .then((response) => {
            this.trainerData.name = response.data.fullName;
            this.trainerData.trainingTypes = response.data.trainingTypes.map(type => `${type.name} (${type.duration})`);
          });
      }
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
    width: auto;
    border-collapse: collapse;
    text-align: center;
    margin: 0 auto;
  }

    .schedule-table th {
      border: 1px solid #ddd;
      padding: 0px 20px;
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
    padding: 0px 10px;
    font-size: 6px;
    height: 100%;
    box-sizing: border-box;
    overflow: hidden;
    white-space: nowrap;
    text-overflow: ellipsis;
  }
</style>
