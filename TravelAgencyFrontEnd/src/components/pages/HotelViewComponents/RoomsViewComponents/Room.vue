<template>
  <div class="p-4 rounded shadow-sm">
      <div class="row">
          <div class="col-lg-4 col-md-4">
              <div class="img-container drop-shadow" width="250" height="150">
                  <img :src="imageApi + room.id" width="250" height="150" />
              </div>
              </div>
              <div class="col-lg-8 col-md-8">
                  <div style="font-size:x-large;font-weight: bold;text-align:left;">{{ room.name }}</div>
                  <div style="text-align:left;" v-html="($store.state.language == 'cz') ? room.descriptionCz : room.descriptionEn" />
              </div>

              <div class="row">
                  <div class="col-lg-4 col-md-4">
                      <p>
                          {{ $t('labels.maxCapacity') }}: {{ room.maxCapacity }}
                          <i class="fas fa-user-alt"></i>
                          <br />
                          {{ $t('labels.price') }}: <span style="font-weight: bold;">{{ room.price }} {{hotel.defaultCurrency.name}}</span>
                      </p>
                  </div>
                  <div class="col-lg-8 col-md-8">

                      <div v-if=" $store.state.searchString.dates.length"
                           style="font-weight: bold;">
                          {{ $t('labels.availableRoomsCount') }}: {{getAvailableRoomsCount(room.roomTypeId)}} {{ $t('labels.fromTotalCount')}}: {{room.roomsCount}}
                      </div>
                      <div v-else style="font-weight: bold;">{{ $t('labels.totalRoomsCount') }}: {{room.roomsCount}}</div>

                      <div v-if=" $store.state.searchString.dates.length">
                          {{ $t('labels.reserv') }}:
                          <InputNumber v-model="room.roomType"
                                       @input="calculateValues(room.id,room.roomType)"
                                       showButtons
                                       :min="0"
                                       :max="getAvailableRoomsCount(room.roomTypeId)"
                                       :value="room.roomType" />
                      </div>
                      <span v-else><span style="color: red">{{ $t('labels.fillDateMessage') }}</span></span>
                  </div>
              </div>
              <div class="row"></div>
          </div>
  </div>
</template>

<script>
import InputNumber from "primevue/inputnumber";

export default {
    components: {
        InputNumber,
    },
    data() {
        return {
        };
    },
    props: {
        room: {},
        hotel: {},
    },
    mounted() {
    },
    computed: {
        imageApi() {
            return this.$store.state.apiRootUrl + '/RoomImage/';
        },
    },
    methods: {
        calculateValues(id, value) {
            this.$store.state.bookingDetail.rooms.forEach(room => {
                if (room.id == id) { room.booked = value; }
            });
            this.$store.dispatch('setBookedTotalPrice');
        },
        getAvailableRoomsCount(typeId) {
            let availableRooms = 0;
            this.hotel.hotelRoomLists.forEach(room => {
                if (room.roomTypeId == typeId) { availableRooms = room.roomsCount; }
            });

            this.$store.state.reservedRoomList.forEach(resRoom => { 
                if (resRoom.roomTypeId == typeId) { availableRooms = availableRooms - resRoom.count; }
            });

            return availableRooms;
        }
    }
};
</script>

<style >
input {
  width: 70px;
}
</style>

<style scoped>
p {
  padding-top: 5px;
}

.p-4 {
  margin-bottom: 15px;
  background-color: rgb(241, 241, 241);
}
</style>