<template>
    <div>
        <h1>Order details</h1>
        <div class="row">
            <div class="col">
                <Card style="">
                    <template #title>
                        {{ $t('user.personalDetails') }}
                    </template>
                    <template #content>
                        <ul class="list-group list-group-flush">
                            <li class="list-group-item leftTexAlignt"><span class="fas fa-user"></span> <b>{{ $t('labels.fullName') }}:</b> {{BookingDetail.user.firstName}} {{BookingDetail.user.lastName}}</li>
                            <li class="list-group-item leftTexAlignt"><span class="fas fa-at"></span> <b> {{ $t('labels.email') }}:</b> {{BookingDetail.user.email}}</li>
                            <li class="list-group-item leftTexAlignt"><span class="fas fa-phone"></span> <b> {{ $t('labels.phone') }}:</b> {{BookingDetail.user.phone}}</li>
                            <li class="list-group-item leftTexAlignt"><span class="fas fa-address-book"></span> <b> {{ $t('user.address') }}:</b> {{BookingDetail.user.street}}, {{BookingDetail.user.zipCode}}, {{BookingDetail.user.city}}</li>
                            <li class="list-group-item leftTexAlignt"><span class="icon mif-note"></span> <b> {{ $t('labels.noticeToTenants') }}:</b> {{BookingDetail.message}}</li>
                        </ul>
                    </template>

                </Card>
            </div>
            <div class="col">
                <Card style="">
                    <template #title>
                        {{ $t('user.reservationInfo') }}
                    </template>
                    <template #content>
                        <ul class="list-group list-group-flush">
                            <li class="list-group-item leftTexAlignt"><span class="fas fa-hotel"></span> <b> {{ $t('user.accommodationName') }}:</b> {{BookingDetail.hotelName}}</li>
                            <li class="list-group-item leftTexAlignt"><span class="fas fa-check-square"></span> <b> {{ $t('labels.fromDate') }}:</b> {{new Date(BookingDetail.startDate).toLocaleDateString('cs-CZ')}}</li>
                            <li class="list-group-item leftTexAlignt"><span class="far fa-check-square"></span> <b> {{ $t('labels.toDate') }}:</b> {{new Date(BookingDetail.endDate).toLocaleDateString('cs-CZ')}}</li>
                            <li class="list-group-item leftTexAlignt"><span class="icon mif-cloud2"></span> <b> {{ $t('labels.nightCount') }}:</b> {{ nightCount }}x</li>
                            <li class="list-group-item leftTexAlignt" :class="(checkPersons ? 'error' : '')"><span class="icon mif-users"></span> <b> {{ $t('labels.personCount') }}:</b> {{BookingDetail.adultInput}} {{ $t('labels.adults') }}, {{BookingDetail.childrenInput}} {{ $t('labels.children') }}</li>
                            <li class="list-group-item leftTexAlignt"><span class="fas fa-money-check-alt"></span> <b> {{ $t('labels.totalPrice') }}:</b> {{BookingDetail.totalPrice}} {{BookingDetail.currency}}</li>
                        </ul>
                        <hr />
                        <h4><b>{{ $t('labels.reservationRooms') }}</b></h4>
                        <ul v-for="room in BookingDetail.rooms" class="list-group list-group-flush">
                            <li v-if="room.booked > 0" class="list-group-item leftTexAlignt"><span class="fas fa-hotel"></span> <b> {{ $t('labels.roomName') }}:</b> {{room.name}}</li>
                            <li v-if="room.booked > 0" class="list-group-item leftTexAlignt"><span class="fas fa-check-square"></span> <b> {{ $t('labels.nightPrice') }}:</b> {{ room.price}} {{BookingDetail.currency}}</li>
                            <li v-if="room.booked > 0" class="list-group-item leftTexAlignt">
                                <span class="far fa-check-square"></span> <b> {{ $t('labels.booked') }}:</b> {{ room.booked}}x
                                <span v-if="room.extrabed">
                                    + <span class='mif-hotel mif-2x fg-cyan' style='top:5px;right:5px;' data-role='hint' data-cls-hint='bg-cyan fg-white drop-shadow' :data-hint-text="$t('labels.extraBed')"></span>
                                </span>
                            </li>
                            <li v-if="room.booked > 0" class="list-group-item leftTexAlignt"><span class="far fa-check-square"></span> <b> {{ $t('labels.roomPrice') }}:</b> {{ room.price}} x {{ nightCount}} x {{ room.booked }} = {{ room.price * room.booked * nightCount}} {{BookingDetail.currency}}</li>
                            <br v-if="room.booked > 0" />
                        </ul>
                        <Button class=" shadowed" :label="$t('labels.editLease')" icon="pi pi-user-edit" @click="$router.push('/Hotels/' + BookingDetail.hotelId + '/Rooms')"></Button>
                    </template>
                </Card>
            </div>
        </div>
        <hr/>
    </div>
</template>
<script>
import Card from 'primevue/card';
import Button from 'primevue/button';

export default {
     components:{
         Card,
         Button,
     },
     data(){
        return{
        }
    },
    mounted() {

    },
    computed: {
        hotel() {
            return this.$store.state.hotel;
        },
        BookingDetail() {
            return this.$store.state.bookingDetail;
        },
        checkPersons() {
            if (this.$store.state.bookingDetail.adultInput == 0) { return true; } else { return false; }
        },
        nightCount() {
            return ((new Date(this.$store.state.bookingDetail.endDate) - new Date(this.$store.state.bookingDetail.startDate)) / (1000 * 60 * 60 * 24));
        }
    },
    methods: {
    },
    created(){
    },
}
</script>
<style scoped>

.error {
    color: red;
}


    .leftTexAlignt{
        text-align: left;
    }
    .CardHolder{
        width: 50%;
        display: block;
    }
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