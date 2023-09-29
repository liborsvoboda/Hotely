<template >
    <div>
        <h4 class="mb-3">Billing address</h4>
        <div class="card h-100">
            <div class="card-body">
                <div class="row gutters">
                    <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12">
                        <h6 class="mb-2 text-primary">{{ $t('user.personalDetails') }}</h6>
                    </div>
                    <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12">
                        <div class="form-group">
                            <label for="firstName">{{ $t('labels.firstname') }}</label>
                            <input v-model="info.firstName" type="text" class="form-control" id="firstName" placeholder="Enter first name" >
                        </div>
                    </div>
                    <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12">
                        <div class="form-group">
                            <label for="lastName">{{ $t('labels.lastname') }}</label>
                            <input v-model="info.lastName" type="text" class="form-control" id="lastName" placeholder="Enter last Name" >
                        </div>
                    </div>
                    <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12">
                        <div class="form-group">
                            <label for="phone">{{ $t('labels.phone') }}</label>
                            <input v-model="info.phone" type="text" class="form-control" id="phone" placeholder="Enter phone number" >
                        </div>
                    </div>
                    <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12">
                        <div class="form-group">
                            <label for="email">{{ $t('labels.email') }}</label>
                            <input v-model="info.email" type="email" class="form-control" id="email" placeholder="Enter email">
                        </div>
                    </div>
                </div>
                <div class="row gutters">
                    <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12">
                        <h6 class="mt-3 mb-2 text-primary">{{ $t('user.address') }}</h6>
                    </div>
                    <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12">
                        <div class="form-group">
                            <label for="Street">{{ $t('labels.street') }}</label>
                            <input v-model="info.street" type="name" class="form-control" id="Street" placeholder="Enter Street">
                        </div>
                    </div>
                    <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12">
                        <div class="form-group">
                            <label for="ciTy">{{ $t('labels.city') }}</label>
                            <input v-model="info.city" type="name" class="form-control" id="ciTy" placeholder="Enter City">
                        </div>
                    </div>
                    <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12">
                        <div class="form-group">
                            <label for="zIp">{{ $t('labels.zipCode') }}</label>
                            <input v-model="info.zipcode" type="text" class="form-control" id="zIp" placeholder="Zip Code">
                        </div>
                    </div>
                    <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12">
                        <div class="form-group">
                            <label for="country">{{ $t('labels.country') }}</label>
                            <input v-model="info.country" type="text" class="form-control" id="country" placeholder="Country">
                        </div>
                    </div>
                    <BookingMessage @message="updateMessage"></BookingMessage>
                </div>
                <Button class="mt-3" @click="updateReservation" :loading="loading">{{ $t('labels.newMessage') }}</Button>
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
        async updateReservation() {
        
            this.loading = true;
            var response = await fetch(this.$store.state.apiRootUrl + '/Guest/Booking/UpdateBooking', {
                method: 'POST',
                headers: {
                    'Authorization': 'Bearer ' + this.$store.state.user.Token,
                    'Content-type': 'application/json'
                },
                body: JSON.stringify({ Booking: this.info, Language: this.$store.state.language }),
            });
            var result = await response.json();
            this.loading = false;

            //get updated booking
            var notify = Metro.notify; notify.setup({ width: 300, timeout: this.$store.state.userSettings.notifyShowTime, duration: 500 });
            notify.create(this.$i18n.t("messages.updateBookingWasSuccess"), "Success", { cls: "success" }); notify.reset();

            this.$emit('closeEdit', false);
            await this.$store.dispatch('getBookingList');

        },
        updateMessage(msg) {
            this.info.message = msg;
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