<template >
    <div>
        <div class="card h-100">
            <div class="card-body">
                <div class="row gutters">
                    <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12">
                        <h6 class="mb-2 text-primary">{{ $t('user.personalDetails') }}</h6>
                    </div>
                    <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12">
                        <ul class="list-group list-group-flush">
                            <li class="list-group-item text-left p-0"><span class="fas fa-user"></span> <b>{{ $t('labels.fullName') }}:</b> {{hotel.firstName}} {{hotel.lastName}}</li>
                            <li class="list-group-item text-left p-0"><span class="fas fa-at"></span> <b> {{ $t('labels.email') }}:</b> {{hotel.email}}</li>
                            <li class="list-group-item text-left p-0"><span class="fas fa-phone"></span> <b> {{ $t('labels.phone') }}:</b> {{hotel.phone}}</li>
                            <li class="list-group-item text-left p-0"><span class="fas fa-address-book"></span> <b> {{ $t('user.address') }}:</b> {{hotel.street}}, {{hotel.zipcode}}, {{hotel.city}}, {{hotel.country}}</li>
                        </ul>
                    </div>
                </div>

                <div class="row gutters">
                    <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12">
                        <h6 class="mt-3 mb-2 text-primary">{{ $t('user.reservationInfo') }}</h6>
                    </div>
                    <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12">
                        <ul class="list-group list-group-flush">
                            <li class="list-group-item text-left p-0"><span class="fas fa-file-invoice"></span> <b> {{ $t('labels.reservationNumber') }}:</b> {{hotel.reservationNumber}}</li>
                            <li class="list-group-item text-left p-0"><span class="fas fa-hotel"></span> <b> {{ $t('user.accommodationName') }}:</b> {{hotel.hotel.name}}</li>
                            <li class="list-group-item text-left p-0"><span class="fas fa-check-square"></span> <b> {{ $t('labels.fromDate') }}:</b> {{new Date(hotel.startDate).toLocaleDateString('cs-CZ')}}</li>
                            <li class="list-group-item text-left p-0"><span class="far fa-check-square"></span> <b> {{ $t('labels.toDate') }}:</b> {{new Date(hotel.endDate).toLocaleDateString('cs-CZ')}}</li>
                            <li class="list-group-item text-left p-0"><span class="fas fa-users"></span> <b> {{ $t('labels.personCount') }}:</b> {{hotel.adult}} {{ $t('labels.adults') }}, {{hotel.children}} {{ $t('labels.children') }}</li>
                            <li class="list-group-item text-left p-0"><span class="fas fa-money-check-alt"></span> <b> {{ $t('labels.totalPrice') }}:</b> {{hotel.totalPrice}} {{hotel.currency.name}}</li>
                        </ul>
                    </div>
                    <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12">
                        <h6 class="mt-3 mb-2 text-primary">{{ $t('labels.lease') }}</h6>
                    </div>
                    <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12">
                        <ul class="list-group list-group-flush">
                            <li v-for="room in hotel.hotelReservedRoomLists" class="list-group-item text-left p-0">
                                <span class="fas fa-door-closed"></span> {{ room.name }} x {{ room.count }}
                            </li>
                        </ul>
                    </div>
                    <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12">
                        <h6 class="mt-3 mb-2 text-primary">{{ $t('labels.messagesHistory') }}</h6>
                    </div>
                    <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12">
                        <ul class="list-group list-group-flush">
                            <li v-for="message in hotel.hotelReservationDetailLists" class="list-group-item text-left p-0 mb-3">
                                <div>
                                    <span class="fas fa-info"></span> {{ new Date(message.timestamp).toLocaleString('cs-CZ') }}
                                    , {{ $t('labels.status') }}: {{ (message.status != null ? message.status.systemName: "")}}
                                </div>
                                <span class="fas fa-envelope-open-text"></span> {{ message.message }}
                            </li>
                        </ul>
                    </div>
                    <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12">
                        <h6 class="mt-3 mb-2 text-primary">{{ $t('labels.ratings') }}</h6>
                    </div>
                    <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12">
                        <ul class="list-group list-group-flush">
                            <li class="list-group-item text-left p-0 mb-3">
                                <div>
                                    <span class="fas fa-info"></span> {{ new Date(hotel.hotelReservationReviewList.timestamp).toLocaleString('cs-CZ') }}
                                    , <input data-role="rating" :data-value="hotel.hotelReservationReviewList.rating" data-star-color="cyan" data-static="true">
                                </div>
                                
                            </li>
                            <li class="list-group-item text-left p-0 mb-3">
                                <div>
                                    <span class="fas fa-envelope-open-text"></span>
                                    {{ hotel.hotelReservationReviewList.description }}
                                </div>
                            </li>
                        </ul>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>

export default {
    components:{

    },
    props:{
        hotel: {}
    },
    data(){
        return{
        }
    },
    computed:{
        info(){
            return this.hotel;
        }
    },
    methods:{
       
    },
    created(){
    },
}
</script>
<style scoped>
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