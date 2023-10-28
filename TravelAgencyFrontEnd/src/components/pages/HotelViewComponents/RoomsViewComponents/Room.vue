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
                <div class="col-lg-4 col-md-4 fg-cyan">
                    <p>
                        {{ $t('labels.maxCapacity') }}: {{ room.maxCapacity }}
                        <i class="fas fa-user-alt fg-cyan"></i>
                        <span v-if="room.extraBed">
                            + <span class='mif-hotel mif-2x fg-cyan' style='top:5px;right:5px;' data-role='hint' data-cls-hint='bg-cyan fg-white drop-shadow' :data-hint-text="$t('labels.extraBed')"></span>
                        </span>

                        <br />
                        {{ $t('labels.price') }}: <span style="font-weight: bold;">{{ room.price }} {{hotel.defaultCurrency.name}}</span>
                    </p>
                </div>
                <div class="col-lg-6 col-md-6">

                    <div v-if="$store.state.searchString.dates.length && $store.state.searchString.dates[1] != null"
                         style="font-weight: bold;">
                        {{ $t('labels.availableRoomsCount') }}: {{ availableCount }} {{ $t('labels.fromTotalCount')}}: {{room.roomsCount}}
                    </div>
                    <div v-else style="font-weight: bold;">{{ $t('labels.totalRoomsCount') }}: {{room.roomsCount}}</div>

                    <div v-if=" $store.state.searchString.dates.length && $store.state.searchString.dates[1] != null">
                        {{ $t('labels.reserv') }}:
                        <InputNumber v-model="room.roomType"
                                     @input="calculateValues(room.id,room.roomType)"
                                     showButtons
                                     :min="0"
                                     :max="availableCount"
                                     :value="room.roomType" />
                    </div>
                    <span v-else><span style="color: red">{{ $t('labels.fillDateMessage') }}</span></span>
                </div>

                <div v-if="$store.state.searchString.dates.length && $store.state.searchString.dates[1] != null && room.extraBed" class="col-lg-1 col-md-1" data-role='hint' data-cls-hint='bg-cyan fg-white drop-shadow' :data-hint-text="$t('labels.iwantExtraBed')">
                    <input :id="'ExtraBed_'+room.id" @change="setExtraBed(room.id)" type="checkbox" data-role="checkbox" class="zoom2 pt-2" data-title="Checkbox" :disabled="availableCount == 0">
                </div>

                <div v-if="$store.state.searchString.dates.length && $store.state.searchString.dates[1] != null && room.roomsCount == 1" class="col-lg-1 col-md-1" data-role='hint' data-cls-hint='bg-cyan fg-white drop-shadow' :data-hint-text="$t('labels.showBookedCalendar')">
                    <span :id="'BookedCalendar_'+room.id" class="icon c-pointer mif-calendar mif-4x pos-absolute fg-blue" style="bottom:15px;left:0px;"
                          @click="setBookedCalendar(room.name);" onclick="Metro.infobox.open('#BookedCalendarBox');" />
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
                availableCount: 0,
                bookedCalendar: []
            };
        },
        props: {
            room: {},
            hotel: {},
        },
    mounted() {

        //START OF setAvailableCount
        let maxBookedRooms = 0;
        this.$store.state.reservedRoomList.forEach((resRoom, index) => {
            if (resRoom.hotelRoomId == this.room.id && resRoom.roomTypeId == this.room.roomTypeId) {
                let startDate = new Date(resRoom.startDate);
                while (new Date(startDate).toLocaleDateString('sv-SE') <= new Date(resRoom.endDate).toLocaleDateString('sv-SE')) {
                    if (this.bookedCalendar.filter(obj => { return obj.date == new Date(startDate).toLocaleDateString('sv-SE') }).length > 0) {
                        this.bookedCalendar.forEach(book => {
                            if (book.roomId == resRoom.hotelRoomId && book.date == new Date(startDate).toLocaleDateString('sv-SE')) {
                                book.booked = book.booked + resRoom.count;
                                if (maxBookedRooms < book.booked ) { maxBookedRooms = book.booked; }
                            }
                        });
                    }
                    if (this.bookedCalendar.filter(obj => { return obj.date == new Date(startDate).toLocaleDateString('sv-SE') }).length == 0) {
                        this.bookedCalendar.push({ roomId: this.room.id, date: new Date(startDate).toLocaleDateString('sv-SE'), availableRooms: this.room.roomsCount, booked: resRoom.count });
                        if (maxBookedRooms < resRoom.count) { maxBookedRooms = resRoom.count; }
                    }
                    startDate = new Date(startDate.getFullYear(), startDate.getMonth(), startDate.getDate() + 1);
                }
            }
        });
        this.availableCount = this.room.roomsCount - maxBookedRooms;
        // End of set setAvailableCount

        //SetExtrabed value on Again edit booking
        this.$store.state.bookingDetail.rooms.forEach(bookedRoom => { $("#ExtraBed_" + bookedRoom.id).val('checked')[0].checked = bookedRoom.extrabed; });
        if (this.$store.state.bookingDetail.rooms.length > 0) { this.$store.dispatch('setBookedTotalPrice'); }
    
        },
        computed: {
            imageApi() {
                return this.$store.state.apiRootUrl + '/RoomImage/';
            },
            BookingDetail() {
                console.log("roomBooking", this.$store.state.bookingDetail);
                return this.$store.state.bookingDetail;
            },

        },
    methods: {
        setBookedCalendar(roomname) {
            $("#roomName").html(roomname);
            let bookedList = []; let minDate = null; let maxDate = null;
            this.bookedCalendar.forEach(booking => {
                if (minDate == null) {
                    minDate = new Date(new Date(booking.date).getFullYear(), new Date(booking.date).getMonth(), 1);
                }
                if (booking.booked > 0) { bookedList.push(booking.date); }
                maxDate = new Date(new Date(booking.date).getFullYear(), new Date(booking.date).getMonth() + 1, 1);
            });

            let calendar = Metro.getPlugin("#BookedCalendar", "calendar"); calendar.selected = []; calendar.exclude = []; calendar.clearSelected();
            calendar.setExclude(bookedList);
            if (minDate != null) {
                if (Metro.storage.getItem('BookCalendarViewMonthsBefore', null) != 1) { minDate = new Date(maxDate.getFullYear(), maxDate.getMonth() - parseInt(Metro.storage.getItem('BookCalendarViewMonthsBefore', null)), 1); }
                calendar.min = new Date();
            }
            if (maxDate != null) {
                if (Metro.storage.getItem('BookCalendarViewMonthsAfter', null) != 1) { maxDate = new Date(maxDate.getFullYear(), maxDate.getMonth() + parseInt(Metro.storage.getItem('BookCalendarViewMonthsAfter', null)) - 1, 31); }
                calendar.max = maxDate;
            }
            calendar.setShow(minDate);
        },
        calculateValues(id, value) {
            this.$store.state.bookingDetail.rooms.forEach(room => {
                if (room.id == id) { room.booked = value; room.extrabed = false; }
            });
            this.$store.dispatch('setBookedTotalPrice');
        },
        setExtraBed(id) {
            this.$store.state.bookingDetail.rooms.forEach(room => {
                if (room.id == id && $("#ExtraBed_" + id).val('checked')[0] != undefined) { room.extrabed = $("#ExtraBed_" + id).val('checked')[0].checked; }
            });
            console.log("booking", this.$store.state.bookingDetail.rooms);
        },

    }
};
</script>

<style>
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