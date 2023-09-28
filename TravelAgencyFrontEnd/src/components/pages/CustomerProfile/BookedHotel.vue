<template>
    <div class="card mb-3" style="margin-left: 20px;margin-right: 20px; top: 20px;width:auto;">
        <div class="row g-0">
            <div class="col-md-4 d-flex flex-align-center">
                <div class="img-container drop-shadow ">
                    <img :src="imageApi + hotel.hotel.hotelImagesLists.filter(obj =>{ return obj.isPrimary == true })[0].id" class="img-fluid ml-1"
                         style="cursor:pointer" :title="$t('labels.searchAccomodation')" @click="openAccommodation(hotel.hotel.id)" />
                </div>
            </div>
            <div class="col-md-8">
                <div class="card-body">
                    <div class="row g-0">
                        <div class="col-md-6">
                            <h5 class="card-title text-left">{{ hotel.reservationNumber }}</h5>
                            <div class="textB">
                                <div class="text-left">{{ hotel.firstName }} {{ hotel.lastName }}</div>
                                <div class="text-left">{{ startDate }} - {{ endDate }}</div>
                                <div class="text-left">{{ hotel.adult }} {{ $t('labels.adults') }}, {{ hotel.children }} {{ $t('labels.children') }}</div>
                                <div class="text-left">{{ $t('labels.requestDate') }} {{new Date(hotel.timestamp).toLocaleDateString('cs-CZ') }}</div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <h5 class="card-title text-right">{{ hotel.hotel.name }}</h5>
                            <div class="textB">
                                <div class="text-right">{{ hotel.status.systemName }}</div>
                                <div v-for="room in hotel.hotelReservedRoomLists" class="text-right">
                                    {{ room.name }} x {{ room.count }}
                                </div>
                                <div class="text-right">{{ hotel.totalPrice }} {{ hotel.currency.name }}</div>
                            </div>
                        </div>
                    </div>
                </div>
                <div v-if="hotel.statusId != 3" class="text-center mb-3" style="bottom: 0px !important; position: absolute; right: 10px;">
                    <div class="p-button p-component button info mr-1" @click="showDetail">{{ $t('labels.reservationDetail') }}</div>
                    <div :disabled="notEdit" @click="toggleEdit" class="p-button p-component button info mr-1">{{ $t('labels.editLease') }}</div>
                    <div :disabled="notEdit" @click="cancel(hotel.reservationNumber,hotel.id)" class="p-button p-component button info p-button-danger">{{ $t('labels.cancelBooking') }}</div>
                </div>
                <div v-else class="d-flex mb-3" style="font-weight:bold;color: red;bottom: 0px !important; position: absolute; right: 10px;">
                    <div class="p-button p-component button info mr-1" @click="showDetail">{{ $t('labels.reservationDetail') }}</div>
                    <div class="text-center pt-2 ml-5">{{ $t('messages.thisBookingIsCancelled') }}</div>
                </div>
            </div>
        </div>
        <div v-if="edit">
            <ProfileCustomerDetail :hotel="hotel" @closeEdit="closeEdit" />
        </div>
        <div v-if="detail">
            <ReservationDetail :hotel="hotel" />
        </div>
    </div>
</template>

<script>
    import ProfileCustomerDetail from "/src/components/pages/CustomerProfile/ProfileCustomerDetail.vue";
    import ReservationDetail from "/src/components/pages/CustomerProfile/ReservationDetail.vue";
    import Button from "primevue/button";
export default {
    components: {
        ProfileCustomerDetail,
        ReservationDetail,
        Button,
    },
    props: {
        hotel: {},
    },
    data() {
        return {
            edit: false,
            detail: false
        };
    },
    computed: {
        imageApi() {
            return this.$store.state.apiRootUrl + '/Image/';
        },
        notEdit() {
            return new Date(this.hotel.startDate) < new Date() || this.hotel.statusId != 1;
        },
        startDate() {
            if (this.hotel.startDate === undefined) {
                return 'Not available'
            }
            return new Date(this.hotel.startDate).toLocaleDateString('cs-CZ');
        },
        endDate() {
            if (this.hotel.endDate === undefined) {
                return 'Not available'
            }
            return new Date(this.hotel.endDate).toLocaleDateString('cs-CZ');
        },
    },
    methods: {
        closeEdit(status) {
            this.edit = status;
        },
        openAccommodation() {
            this.$store.dispatch('clearBooking');
            this.$router.push('/');
            this.$store.state.searchButtonLoading = true;
            this.$store.dispatch('searchHotels', this.hotel.hotel.name);
        },
        toggleEdit() {
            this.detail = false;
            this.edit = !this.edit;
        },
        showDetail() {
            this.edit = false;
            this.detail = !this.detail;
        },
    
        async cancel(bookingNr ,id) {
            let message = prompt(this.$i18n.t("messages.doYouReallyCancelBooking") + ' ' + bookingNr + this.$i18n.t("messages.writeCancelReason"));
            
            if (message == null || message == "") {

            } else {
                var response = await fetch(
                    this.$store.state.apiRootUrl + '/Guest/Booking/CancelBooking', {
                    method: 'POST',
                    headers: {
                        'Authorization': 'Bearer ' + this.$store.state.user.Token,
                        'Content-type': 'application/json',
                    },
                    body: JSON.stringify({ ReservationId: id, Message: message, Language: this.$store.state.language })
                });
                var result = await response.json()
                await this.$store.dispatch('getBookingList');

                var notify = Metro.notify; notify.setup({ width: 300, duration: this.$store.state.userSettings.notifyShowTime });
                notify.create(this.$i18n.t("messages.bookingWasCancelled"), "Success", { cls: "success" }); notify.reset();
            }
        },
    },
};
</script>

<style scoped>

#buttonG{
  background: #53c16e;
  border:#14a04d;
}

#buttonG:enabled:hover{
  background:#348047 !important;
  border-color:#14a04d;
}


</style>