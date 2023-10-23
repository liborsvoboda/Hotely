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
                                <div v-if="hotel.hotelReservationReviewList != null" class="text-right"><input data-role="rating" :data-value="hotel.hotelReservationReviewList.rating" data-star-color="cyan" data-static="true"></div>
                            </div>
                        </div>
                    </div>
                </div>
                <div v-if="hotel.statusId != 3" class="text-center mb-3" style="bottom: -5px !important; position: absolute; left: 10px;">
                    <div class="p-button p-component button info outline shadowed mr-1" @click="showDetail">
                        {{ $t('labels.reservationDetail') }}
                        <span v-if="newDetailCount > 0" class="badge fg-green mt-1 mr-2">{{ newDetailCount }}</span>
                    </div>
                    <div :id="'EditButton_'+hotel.reservationNumber" :class="(notEdit ? 'disabled' : '')" @click="toggleEdit" class="p-button p-component button info outline shadowed mr-1">
                        {{ $t('labels.editLease') }}
                        <span v-if="newDetailCount > 0" class="badge fg-green mt-1 mr-2">{{ newDetailCount }}</span>
                    </div>
                    <div :id="'CancelButton_'+hotel.reservationNumber" :class="(notEdit ? 'disabled' : '')" @click="cancel(hotel.reservationNumber,hotel.id)" class="p-button p-component button info outline shadowed p-button-danger">{{ $t('labels.cancelBooking') }}</div>
                    <div :id="'ReviewButton_'+hotel.reservationNumber" :class="(notReview ? 'disabled' : '')" class="p-button p-component button warning outline shadowed" @click="addRewiew">{{ $t('labels.ratings') }}</div>
                </div>
                <div v-else class="d-flex mb-3" style="font-weight:bold;color: red;bottom: -5px !important; position: absolute; left: 10px;">
                    <div class="p-button p-component button info outline shadowed mr-1" @click="showDetail">
                        {{ $t('labels.reservationDetail') }}
                        <span v-if="newDetailCount > 0" class="badge fg-green mt-1 mr-2">{{ newDetailCount }}</span>
                    </div>
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
        <div v-if="review">
            <AddReview :hotel="hotel" @closeReviewEdit="closeReviewEdit" />
        </div>
    </div>
</template>

<script>
    import ProfileCustomerDetail from "/src/components/pages/CustomerProfile/ProfileCustomerDetail.vue";
    import ReservationDetail from "/src/components/pages/CustomerProfile/ReservationDetail.vue";
    import AddReview from "/src/components/pages/CustomerProfile/AddReview.vue";
    import Button from "primevue/button";
export default {
    components: {
        ProfileCustomerDetail,
        ReservationDetail,
        AddReview,
        Button,
    },
    props: {
        hotel: {},
    },
    data() {
        return {
            edit: false,
            detail: false,
            review: false
        };
    },
    computed: {
        imageApi() {
            return this.$store.state.apiRootUrl + '/Image/';
        },
        notEdit() {
            return new Date(new Date(this.hotel.startDate).getTime() - this.hotel.hotel.enabledCommDaysBeforeStart * 86400000) < new Date() || this.hotel.statusId != 1;
        },
        notReview() {
            return this.hotel.hotelReservationReviewList != null;
        },
        newDetailCount() {
            if (this.hotel.hotelReservationDetailLists == null) { return 0; }
            return this.hotel.hotelReservationDetailLists.filter(obj => { return !obj.guestSender && !obj.shown; }).length;
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
    mounted() {
        if (new Date(new Date(this.hotel.startDate).getTime() - this.hotel.hotel.enabledCommDaysBeforeStart * 86400000) < new Date() || this.hotel.statusId != 1) {
            $("#EditButton_" + this.hotel.reservationNumber).hide();
            $("#CancelButton_" + this.hotel.reservationNumber).hide();
        }
        if (new Date(this.hotel.endDate) <= new Date() && this.hotel.statusId == 2 && this.hotel.hotelReservationReviewList == null) {
            $("#ReviewButton_" + this.hotel.reservationNumber).show();
        } else { 
            $("#ReviewButton_" + this.hotel.reservationNumber).hide();
        }
    },
    methods: {
        closeReviewEdit(status) {
            this.review = status;
        },
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
            this.review = false;
            this.edit = !this.edit;
        },
        showDetail() {
            this.edit = false;
            this.review = false;
            this.detail = !this.detail;
        },
        addRewiew() {
            this.detail = false;
            this.edit = false;
            this.review = !this.review;
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

                var notify = Metro.notify; notify.setup({ width: 300, timeout: this.$store.state.userSettings.notifyShowTime, duration: 500 });
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