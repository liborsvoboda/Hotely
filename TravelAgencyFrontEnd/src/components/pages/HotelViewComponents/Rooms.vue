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
                          <router-link to="/checkout" class="btn btn-primary" @click="Book"><span class="far fa-bookmark"></span> {{ $t('labels.demandReservation')}} </router-link>
                      </div>
                      <div v-else>
                          <b>{{ $t('labels.insertRoomsCount')}}</b>
                      </div>
                  </div>
              </div>
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
            console.log("hotel", this.$store.state.hotel);
            console.log("rooms", this.$store.state.hotel.hotelRoomLists);
        
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
        //Cleared Booked Rooms
        this.$store.state.bookingDetail.rooms = [];
        this.$store.state.hotel.hotelRoomLists.forEach(room => {
            this.$store.state.bookingDetail.rooms.push({
                id: room.id, 
                typeId: room.roomTypeId,
                name: room.name, 
                price: room.price,
                booked: 0
            });
        });
        this.$store.state.bookingDetail.totalPrice = 0;
    },
    methods: {
        Book() {
            this.$store.state.bookingDetail.hotelId = this.$store.state.hotel.id;
            this.$store.state.bookingDetail.hotelName = this.$store.state.hotel.name;
            this.$store.state.bookingDetail.startDate = this.$store.state.searchString.dates[0];
            this.$store.state.bookingDetail.endDate = this.$store.state.searchString.dates[1];

            this.$store.state.bookingDetail.adultInput = this.$store.state.searchString.inputAdult;
            this.$store.state.bookingDetail.childrenInput = this.$store.state.searchString.inputChild;
            this.$store.state.bookingDetail.roomsInput = this.$store.state.searchString.inputRooms;

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