<template>
    <div class="container">
        <div class="row">
            <!-- left column -->
            <div class="col-md-10">
                <Room v-for="room in hotelInfo.hotelRoomLists" :room="room" :hotel="hotelInfo" :key="room.id" />
            </div>


            <!-- right column -->
            <div class="col-md-2 rounded shadow-sm" id="rightCard">
                <div class="row pt-5">
                    <div class="col-md-12">
                        <b>
                            {{ $t('labels.totalPrice')}}
                            <br />
                            {{ totalprice }} {{hotelInfo.defaultCurrency.name}}
                        </b>
                    </div>

                    <div class="col-md-12 pt-5">
                        <div v-if="CanBook">
                            <router-link to="/Checkout" class="btn btn-primary shadowed" @click="Book"><span class="far fa-bookmark"></span> {{ $t('labels.demandReservation')}} </router-link>
                        </div>
                       <!--  <div v-else>
                            <b>{{ $t('labels.insertRoomsCount')}}</b>
                        </div> -->
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div id="BookedCalendarBox" class="info-box" data-role="infobox" data-type="default" data-width="630" data-height="450">
        <span class="button square closer"></span>
        <div class="info-box-content" style="overflow-y:auto;">
            <div class="d-flex row ">
                <div class="h3 col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12 pt-0 mt-0 pb-0 mb-0">
                    {{ $t('labels.availableCalendar') }}
                </div>
                <div id="roomName" class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12 pt-0 mt-0 pb-0 mb-0"></div>
            </div>
            <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12">
                <div id="BookedCalendar" data-role="calendar" data-wide-point="sm" data-week-start="1" data-input-format="yyyy-mm-dd" data-buttons="today" data-ripple="true"></div>
            </div>
        </div>
    </div>
</template>

<script>
import Room from "./RoomsViewComponents/Room.vue";
import Options from "./RoomsViewComponents/Option.vue";
import SelectButton from 'primevue/selectbutton';
export default {
    components: {
        Room,
        Options,
        SelectButton
    },
    data(){
        return{
            
        }
    },
    computed: {
        hotelInfo() {
            return this.$store.state.hotel;
        },
        totalprice() {
            return this.$store.state.bookingDetail.totalPrice;
        },
        CanBook(){
            if (this.$store.state.bookingDetail.totalPrice > 0) {
                return true;
            }
            return false;
        },
    },
    mounted() {
        
    },
    methods: {
        Book() {
            this.$store.state.bookingDetail.hotelId = this.$store.state.hotel.id;
            this.$store.state.bookingDetail.hotelName = this.$store.state.hotel.name;
            this.$store.state.bookingDetail.currencyId = this.$store.state.hotel.defaultCurrency.id;
            this.$store.state.bookingDetail.currency = this.$store.state.hotel.defaultCurrency.name;
            this.$store.state.bookingDetail.startDate = this.$store.state.searchString.dates[0].toLocaleDateString('sv-SE');
            this.$store.state.bookingDetail.endDate = this.$store.state.searchString.dates[1].toLocaleDateString('sv-SE');
            window.scrollTo(0, 0);
        },
    },
    created(){
    }
};
</script>


<style scoped>

.p-buttonset .p-button.p-highlight {
    background: #53c16e !important;
    border-color: #1bc541 !important;
}

    .p-buttonset .p-button.p-highlight:hover {
        background: #53c16e !important;
        border-color: #1bc541 !important;
    }

.btn.btn-primary {
    background-color: #53c16e;
    border-color: #1bc541;
}

.container {
    padding-top: 20px;
}

#rightCard {
    margin-bottom: 15px;
}
</style>