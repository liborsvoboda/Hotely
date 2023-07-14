<template>
  <div class="p-4 rounded shadow-sm">
    <div class="row">
        <div class="col-lg-4 col-md-4">
            <img :src="imageApi + room.id" alt="" width="250" height="150" />
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

                <div v-if=" $store.state.searchString.dates.length && $store.state.searchResults.length "
                style="font-weight: bold;">{{ $t('labels.availableRoomsCount') }}: {{room.roomsCount}}</div>
                <div v-else style="font-weight: bold;">{{ $t('labels.totalRoomsCount') }}: {{room.roomsCount}}</div>

                <div v-if=" $store.state.searchString.dates.length && $store.state.searchResults.length ">
                    <InputNumber v-model="room.roomType"
                                 showButtons
                                 :min="0"
                                 :max="room.roomsCount"
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
            array: [],
            arrayIndex: null,
            singleRooms: null,
            doubleRooms: null,
            familyRooms: null,
        };
    },
    props: {
        room: {},
        hotel: {},
    },
    mounted() {
        if (this.$store.state.searchResults.length) {
            this.array = this.$store.state.searchResults;
            this.arrayIndex = this.array.findIndex((i) => i.hotel.id == this.$route.params.id);

            if (this.array[this.arrayIndex].roomList) {
                this.singleRooms = this.array[this.arrayIndex].roomList.singleRooms;
                this.doubleRooms = this.array[this.arrayIndex].roomList.doubleRooms;
                this.familyRooms = this.array[this.arrayIndex].roomList.familyRooms;
            }
        }
    },
    computed: {
        imageApi() {
            return this.$store.state.apiRootUrl + '/RoomImage/';
        },
        inputSingleRooms: {
            get() {
                return this.$store.state.bookingDetails.noOfSingleRooms;
            },
            set(noOfUnit) {
                if (this.room.type == "Single") {
                    let unitPrice = this.room.price;
                    let roomId = this.room.id;
                    this.$store.dispatch("setSingleRooms", {
                    noOfUnit,
                    unitPrice,
                    roomId,
                    });
                }
            },
        },
        inputDoubleRooms: {
            get() {
                return this.$store.state.bookingDetails.noOfDoubleRooms;
            },
            set(noOfUnit) {
                if (this.room.type == "Double") {
                    let unitPrice = this.room.price;
                    let roomId = this.room.id;
                    this.$store.dispatch("setDoubleRooms", {
                    noOfUnit,
                    unitPrice,
                    roomId,
                    });
                }
            },
        },
        inputFamilyRooms: {
            get() {
            return this.$store.state.bookingDetails.noOfFamilyRooms;
            },
            set(noOfUnit) {
                if (this.room.type == "Family") {
                    let unitPrice = this.room.price;
                    let roomId = this.room.id;
                    this.$store.dispatch("setFamilyRooms", {
                    noOfUnit,
                    unitPrice,
                    roomId,
                    });
                }
            },
        },
    },
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