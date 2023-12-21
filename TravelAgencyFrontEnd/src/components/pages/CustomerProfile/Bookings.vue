<template>
    <div>
        <div class="rounded drop-shadow row">
            <div class="col-md-6 text-left">
                <h1>{{ $t('user.bookings') }}</h1>
            </div>
            <div class="col-md-6 text-right">
                <span class="icon mif-info pt-3 mif-3x c-pointer fg-orange" onclick="OpenDocView('Bookings')" />
            </div>
        </div>
        <div v-if="!bookingList.length && !errorText">
        </div>
        <div v-if="errorText">
            <p>{{ $t('messages.anyReservationExist') }}</p>
        </div>
        <BookedHotel v-for="hotel in bookingList" :hotel="hotel" :key="hotel.id"></BookedHotel>
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
    methods: {
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