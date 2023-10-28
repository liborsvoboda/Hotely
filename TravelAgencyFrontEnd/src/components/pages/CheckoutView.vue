<template>
    <div id="test2" class="mt-5">
        <div class="container-fluid">
            <div class="row">
                <div class="col-md-12">
                    <h3>
                        {{ $t('labels.recapitulation') }}
                    </h3>
                </div>
            </div>
            <div class="card p-shadow-1">
                <Steps :model="items" />
            </div>
            <router-view class="" @payment-confirmed="confirmed" />
            <div v-if="notAtStart" @click="prevPage" class="p-button p-component button info outline shadowed mt-3 mr-3">{{ $t('labels.previous') }}</div>
            <div v-if="notAtEnd" @click="nextPage" class="p-button p-component button info outline shadowed mt-3">{{ $t('labels.next') }}</div>
        </div>
    </div>
</template>

<script>
import CustomerDetails from './CheckoutViewComponents/CustomerDetails.vue'
import Steps from 'primevue/steps';
import Button from 'primevue/button';
export default {
    components:{
        CustomerDetails,
        Steps,
        Button
    },
    computed: {
        loggedIn() {
            return this.$store.state.user.loggedIn;
        },
        notAtStart(){
            return this.page > 0 && this.page != this.items.length - 1 ? true : false;
        },
        notAtEnd() {
            return this.page < this.items.length - 1 && (this.page != 1 || this.page == 1 && !this.checkPersons) && (this.loggedIn || this.$store.state.bookingDetail.verified) ? true : false;
        },
        checkPersons() {
            if (this.$store.state.bookingDetail.adultInput == 0) { return true; } else { return false; }
        }
    },
    data() {
		return {
            items: [
                {
                label: this.$i18n.t("labels.customer"),
                to: '/Checkout'
                },
                {
                    label: this.$i18n.t('user.reservationInfo'),
                    to: '/Checkout/OrderDetails'
                },
                {
                    label: this.$i18n.t("labels.sendBookingRequest"),
                    to: '/Checkout/OrderConfirmed'
                },
            ],
            page: 0
		}
	},
    methods:{
        nextPage() {
            if(this.page < this.items.length - 1){
                this.page++;
                this.$router.push(this.items[this.page].to);
            }
        },
        prevPage() {
            if(this.page >= 1){
                this.page--;
                this.$router.push(this.items[this.page].to);
            }
        },
        confirmed(id){
            this.nextPage();
        }
    }
}
</script>

<style scoped>

    .h3, h3{
        padding-top: 20px;
    }
    #test2{
        border-radius: 20px;
        background-color: white;
        padding-left: 25px;
        padding-right: 25px;
        padding-bottom: 20px;
        margin-top: 20px;
        color:rgb(255, 255, 255);
        /* text-shadow: 2px 2px black; */
    }

button.p-button.p-component{
  background: #53c16e;
  border:#14a04d;
  text-decoration: none;
}

.p-button:enabled:hover{
  background:#348047 !important;
  border-color:#14a04d;
}

</style>