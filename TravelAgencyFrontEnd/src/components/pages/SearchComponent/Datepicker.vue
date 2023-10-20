<template>
   <!--  showIcon -->
    <Calendar dateFormat="yy/mm/dd"
              placeholder="Vyberte Období"
              v-model="dates"
              @clear-click="clearDate"
              selectionMode="range"
              :manualInput="false"
              :minDate="minDate"
              :monthNavigator="true"
              :yearNavigator="true"
              yearRange="2023:2050"
              :showButtonBar="true"
              @date-select="getDate" />
</template>

<script>
import Calendar from "primevue/calendar";

export default {
    components: {
        Calendar,
    },
    created() {
        let today = new Date();
        let month = today.getMonth();
        let year = today.getFullYear();
        let prevMonth = month === 0 ? 11 : month - 1;
        let prevYear = prevMonth === 11 ? year - 1 : year;
        let nextMonth = month === 11 ? 0 : month + 1;
        let nextYear = nextMonth === 0 ? year + 1 : year;
        this.minDate = today;
    },
    data() {
        return {
            dates: null,
            minDate: null,
        };
    },
    methods: {
        clearDate() {
            this.$store.dispatch("setDates", []);
        },
        getDate(value) {
            this.$store.dispatch("setDates", this.dates);
        }
    },
    computed: {

    },
};
</script>

<style scoped>
</style>