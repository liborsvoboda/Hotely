<template>
    <Card id="test4" class="mb-4">
        <template #content>
            <div class="hotelview">
                <h1>{{hotel.name}}</h1>
                <div class="container-fluid">
                    <!-- <PhotoG :photos="photos"></PhotoG> -->
                    <div class="row">
                        <div class="col-md-12 buttons">
                            <ul class="nav nav-pills">
                                <div class="btn-group" role="group" aria-label="Basic example">
                                    <router-link :to="backRoute">
                                        <button class="btn btn-primary" style="height: 45px;margin-right:50px;"
                                                for="btn-check-outlined">
                                            {{ $t('labels.back') }}
                                        </button>
                                    </router-link>
                                </div>
                                <li class="nav-item">
                                    <Button id="menuButton" :label="$t('labels.info')" icon="pi pi-info-circle" @click="$router.push('/hotels/' + this.$route.params.id + '/')"></Button>
                                </li>
                                <li class="nav-item">
                                    <Button id="menuButton" :label="$t('labels.photos')" icon="pi pi-images" @click="$router.push('/hotels/' + this.$route.params.id + '/photos')"></Button>
                                </li>
                                <li class="nav-item">
                                    <Button id="menuButton" :label="$t('labels.lease')" icon="pi pi-home" @click="$router.push('/hotels/' + this.$route.params.id + '/rooms')"></Button>
                                </li>

                                <li class="nav-item">
                                    <Button id="menuButton" :label="$t('labels.reviews')" icon="pi pi-comments" @click="$router.push('/hotels/' + this.$route.params.id + '/reviews')"></Button>
                                </li>

                                <li class="nav-item">
                                    <button class="btn" @click="SetFavorite" id="starBtn">
                                        <i :class="isFavorite ? 'fas fa-heart fa-2x' : 'far fa-heart fa-2x'" style="color: red;"></i>
                                    </button>
                                </li>
                            </ul>
                        </div>
                    </div>
                    <div> 
                        <router-view></router-view>
                    </div>
                </div>
            </div>
        </template>
    </Card>
</template>

<script>
import Info from './HotelViewComponents/Info.vue'
import Photos from "./HotelViewComponents/Photos.vue"
import Button from 'primevue/button';
import PhotoG from "./HotelViewComponents/RoomsViewComponents/PhotoSlider.vue"
import Card from 'primevue/card';

export default {
    components:{
        Info,
        Photos,
        Button,
        PhotoG,
        Card,    
    },
    data(){
        return{
            hotelInfo:{}
        }
    },
    computed: {
        isFavorite() {
            console.log("HotelChecking Fav store/com store/fav", this.$store.state.hotel, this.hotel.id, this.$store.state.lightFavoriteHotelList);

            return this.$store.state.lightFavoriteHotelList.filter(obj => { return obj.hotelId === this.hotel.id; }).length > 0;
        },
        backRoute() {
            return this.$store.state.backRoute;
        },
        loggedIn() {
            return this.$store.state.user.loggedIn
        },
        photos() {
            var photos = [];
            photos.push({ id: this.hotel.id, hotelPhoto: this.$store.state.apiRootUrl + '/Image/' + this.hotel.id + '/' + this.hotel.hotelImagesLists.filter(obj => { return obj.isPrimary === true; })[0].fileName });

            this.hotel.hotelImagesLists.forEach(image => {
                if (!image.isPrimary) { photos.push({ id: this.hotel.id, hotelPhoto: this.$store.state.apiRootUrl + '/Image/' + this.hotel.id + '/' + image.fileName }) }
            });
            return photos;
        },
        hotel(){ 
            return this.$store.state.hotel;
        }
    },
    methods:{
        async SetFavorite() {
            if (this.loggedIn) {
                let response = await fetch(this.$store.state.apiRootUrl + '/Guest/SetFavorite/' + this.hotel.id, {
                    method: "GET",
                    headers: {
                        "Authorization": 'Bearer ' + this.$store.state.user.Token,
                        "Content-type": "application/json",
                    }
                });

                let result = await response.json();
                this.$store.dispatch('setLightFavoriteHotelList', result);
                var notify = Metro.notify; notify.setup({ width: 300, timeout: this.$store.state.userSettings.notifyShowTime, duration: 500 });
                notify.create(this.isFavorite ? this.$i18n.t("messages.addedToFavorite") : this.$i18n.t("messages.removedFromFavorite"), "Success", { cls: "success" }); notify.reset();
            } else { 
                var notify = Metro.notify; notify.setup({ width: 300, timeout: this.$store.state.userSettings.notifyShowTime, duration: 500 });
                notify.create(this.$i18n.t("messages.forSaveToFavoriteYouMustBeLogged"), "Info"); notify.reset();
            }
        },
    },
    created() {
    }
}
</script>
<style>

    #test4 {
        margin-top: 30px;
        border-radius: 20px;
    }

    .hotelview h1 {
        padding-bottom: 30px;
        font-family: 'Times New Roman', Times, serif;
        font-weight: bold;
        text-decoration: underline;
    }

    #menuButton {
        margin-right: 5px;
        height: 45px;
        background: #53c16e;
        border: #14a04d;
    }

        #menuButton:enabled:hover {
            background: #348047 !important;
            border-color: #14a04d;
        }

    .col-md-12.buttons {
        border-top: 1px solid black;
        border-bottom: 1px solid black;
        padding-top: 15px;
        padding-bottom: 10px;
        color: white;
    }

    .btn-group a {
        color: white !important;
        text-decoration: none !important;
    }


    .nav.nav-pills {
        /* margin-left: 8cm; */
    }

    .nav-link {
        color: rgb(255, 255, 255);
    }

    #starBtn {
        outline: none;
        box-shadow: none;
    }</style>