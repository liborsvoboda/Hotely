<template>
    <Card id="test4">
        <template #content>
            <div class="hotelview">

                <h1>{{hotel.name}}</h1>
                <div class="container-fluid">
                    <PhotoG :photos="photos"></PhotoG>
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

                                    <Button id="menuButton" :label="$t('labels.info')" icon="pi pi-info-circle" @click="$router.push('/hotels/' + this.$route.params.id + '/')"></Button>
                                    <Button id="menuButton" :label="$t('labels.photos')" icon="pi pi-images" @click="$router.push('/hotels/' + this.$route.params.id + '/photos')"></Button>
                                    <Button id="menuButton" :label="$t('labels.rooms')" icon="pi pi-home" @click="$router.push('/hotels/' + this.$route.params.id + '/rooms')"></Button>
                                    <Button id="menuButton" :label="$t('labels.reviews')" icon="pi pi-comments" @click="$router.push('/hotels/' + this.$route.params.id + '/reviews')"></Button>
                                </div>

                                <li class="nav-item">
                                    <button class="btn" @click="ToggleStar" id="starBtn">
                                        <i :class="star ? 'fas fa-heart fa-2x' : 'far fa-heart fa-2x'" style="color: red;"></i>
                                    </button>
                                </li>
                            </ul>
                        </div>
                    </div>
                    <div> <!-- tab view -->
                        <router-view></router-view>
                    </div>
                </div>
            </div>
        </template>
    </Card>
</template>
<style>
        #test4{
            margin-top: 30px;
            border-radius: 20px;
         }

        .hotelview h1{
            padding-bottom: 30px;
            font-family:'Times New Roman', Times, serif;
            font-weight: bold;
            text-decoration:underline;
        }

        #menuButton{
           margin-right:5px;
            height:45px;
            background: #53c16e;
            border:#14a04d;
        }

        #menuButton:enabled:hover{
            background:#348047 !important;
            border-color:#14a04d;
        }

        .col-md-12.buttons{
            border-top: 1px solid black;
            border-bottom: 1px solid black;
            padding-top:15px;
            padding-bottom:10px;
            margin-left:px;
            color:white;
        }

        .btn-group a{ 
            color:white !important;
            text-decoration: none !important;
        }
      

        .nav.nav-pills{
            margin-left: 8cm;
        }

        .nav-link{
            color:rgb(255, 255, 255);
        }

        #starBtn{
            outline: none;
            box-shadow: none;
        }

</style>


<script>
import Info from './HotelViewComponents/Info.vue'
import Photos from "./HotelViewComponents/Photos.vue"
import Button from 'primevue/button';
import PhotoG from "./HotelViewComponents/RoomsViewComponents/PhotoSlider.vue"
import Card from 'primevue/card';

export default ({
    components:{
        Info,
        Photos,
        Button,
        PhotoG,
        Card,    
    },
    data(){
        return{
            hotelInfo:{},
            star: false
        }
    },
    computed: {
        backRoute() {
            return this.$store.state.backRoute;
        },
        photos() {
            var photos = [];
            photos.push({ id: this.hotel.id, hotelPhoto: this.$store.state.apiRootUrl + '/Image/' + this.hotel.id + '/' + this.hotel.hotelImagesLists.filter(obj => { return obj.isPrimary === true; })[0].fileName });

            this.hotel.hotelImagesLists.forEach(image => {
                if (!image.isPrimary) { photos.push({ id: this.hotel.id, hotelPhoto: this.$store.state.apiRootUrl + '/Image/' + this.hotel.id + '/' + image.fileName }) }
            });
            console.log("PHOTOS", photos);
            return photos;
        },
        hotel(){ 
            return this.$store.state.hotel;
        }
    },
    methods:{
        ToggleStar(){
            if(this.star === false){
                // Set fav
                this.$store.dispatch('addFavouriteHotel', this.hotel.id)
                this.star = !this.star;
            }
            else{
                // Remove fav
                this.$store.dispatch('removeFavouriteHotel', this.hotel.id)
                this.star = !this.star;
            }
        },
        getSavedHotels(){ 
            fetch(this.state.apiRootUrl + '/Guest/GetSavedHotels/' + this.$store.state.user.id)
                .then(response => response.json())
                .then(result => {
                    if(result){
                        for (let i = 0; i < result.length; i++) {
                            if(result[i].hotelId === this.hotel.id){
                                this.star = true;
                            }
                        }
                    }
            });
        }
    },
    watch:{
        hotelInfo: {
            handler: function(oldVal, newVal){
                setTimeout(function(that){ 
                    that.getSavedHotels();
                }, 500, this);
            },
        }
    },
    created() {
        if(!this.$store.state.searchResults.length){
            this.$store.dispatch('getHotelById', this.$route.params.id)
        }
        
  
        this.$store.dispatch('getHotelById', this.$route.params.id)
        .then(() =>{
            this.getSavedHotels();
        });

        // setTimeout(function(that){ 
        //     that.getSavedHotels();
        // }, 1500, this);
    }
})
</script>
