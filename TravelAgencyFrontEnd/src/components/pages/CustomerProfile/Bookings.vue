<template>
    <div>
        <div v-if="!bookingList.length && !errorText">
            <ProgressSpinner />
        </div>
        <div v-if="errorText">
            <p>{{ $t('messages.anyReservationExist') }}</p>
        </div>
        <BookedHotel v-for="hotel in bookingList" :hotel="hotel" :key="hotel.id"></BookedHotel>
        <!-- <ConfirmDialog></ConfirmDialog> -->
    </div>
</template>
<script>
import BookedHotel from './BookedHotel.vue'
import ConfirmDialog from "primevue/confirmdialog";
import ProgressSpinner from 'primevue/progressspinner';
export default {
    data(){
        return{
            errorText: false
        }
    },
    components:{
        BookedHotel,
        ConfirmDialog,
        ProgressSpinner
    },
    async created(){
        await this.$store.dispatch('getBookingList');
        
        if (!this.bookingList.length){
            this.errorText = true;
        }
        
    },
    computed:{
        bookingList(){
            return this.$store.state.bookingList;
        }
    }
}
</script>
<style lang=""> 
    
</style>