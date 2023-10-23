<template >
    <div>
        <div class="card h-100">
            <div class="card-body">
                <div class="row gutters">
                    <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12">
                        <h6 class="mb-2 text-primary">{{ $t('labels.addReview') }}</h6>
                    </div>

                    <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12 text-left">
                        <input id="Rating" class="mt-2" data-role="rating" data-stared-color="cyan" :data-message="$t('labels.ratings')" >
                    </div>
                </div>

                <div class="row gutters">
                    <BookingMessage @message="updateMessage"></BookingMessage>
                </div>
                <Button class="mt-3" @click="addReview" :loading="loading">{{ $t('labels.addReview') }}</Button>
            </div>
        </div>
    </div>
</template>

<script>
import BookingMessage from '/src/components/pages/CheckoutViewComponents/BookingMessage.vue'
import Button from 'primevue/button';
export default {
    components:{
        BookingMessage,
        Button,
    },
    props:{
        hotel: {}
    },
    data(){
        return{
            loading: false,
            message: null
        }
    },
    computed:{
        info(){
            return this.hotel;
        }
    },
    mounted() {
        this.info.message = '';
    },
    methods:{
        async addReview() {
        
            this.loading = true;
            var response = await fetch(this.$store.state.apiRootUrl + '/Guest/Review/AddReview', {
                method: 'POST',
                headers: {
                    'Authorization': 'Bearer ' + this.$store.state.user.Token,
                    'Content-type': 'application/json'
                },
                body: JSON.stringify({
                    HotelId: this.hotel.hotelId, ReservationId: this.hotel.id,
                    Rating: $("#Rating").val(), Message: this.message, Language: this.$store.state.language }),
            });
            var result = await response.json();
            this.loading = false;

            var notify = Metro.notify; notify.setup({ width: 300, timeout: this.$store.state.userSettings.notifyShowTime, duration: 500 });
            notify.create(this.$i18n.t("messages.addReviewWasSuccess"), "Success", { cls: "success" }); notify.reset();

            //get updated booking
            this.$emit('closeReviewEdit', false);
            await this.$store.dispatch('getBookingList');

        },
        updateMessage(msg) {
            this.message = msg;
        }
    },
    created(){
    },
}
</script>
<style scoped>
    label{
        color:black;
    }

    button.p-button.p-component{
  background: #53c16e;
  border:#14a04d;
  text-decoration: none;
}

.p-button:enabled:hover{
  background:#348047 !important;
  border-color:#14a04d;
  text-decoration: none;
}
</style>