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
                        <ul class="list-group list-group-flush" >
                            <li class="list-group-item leftTexAlignt"><span class="fas fa-hotel"></span> <b> {{ $t('user.accommodationName') }}:</b> {{BookingDetail.hotelName}}</li>
                            <li class="list-group-item leftTexAlignt"><span class="fas fa-check-square"></span> <b> {{ $t('labels.fromDate') }}:</b> {{new Date(BookingDetail.startDate).toLocaleDateString('cs-CZ')}}</li>
                            <li class="list-group-item leftTexAlignt"><span class="far fa-check-square"></span> <b> {{ $t('labels.toDate') }}:</b> {{new Date(BookingDetail.endDate).toLocaleDateString('cs-CZ')}}</li>

                            <li class="list-group-item leftTexAlignt" :class="(checkPersons ? 'error' : '')"><span class="fas fa-users"></span> <b> {{ $t('labels.personCount') }}:</b> {{BookingDetail.adultInput}} {{ $t('labels.adults') }}, {{BookingDetail.childrenInput}} {{ $t('labels.children') }}</li>
                            <li class="list-group-item leftTexAlignt"><span class="fas fa-money-check-alt"></span> <b> {{ $t('labels.totalPrice') }}:</b> {{BookingDetail.totalPrice}} {{BookingDetail.currency}}</li>
                        </ul>
                        <Button :label="$t('labels.editLease')" icon="pi pi-user-edit" @click="$router.push('/hotels/' + BookingDetail.hotelId + '/rooms')"></Button>
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
    computed: {
        BookingDetail() {
            return this.$store.state.bookingDetail;
        },
        checkPersons() {
            if (this.$store.state.bookingDetail.adultInput == 0) { return true; } else { return false; }
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