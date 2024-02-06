<template>
    <div class="card mb-3" style="margin-left: 20px;margin-right: 20px; top: 20px;width:auto;">
        <div class="row g-0">
            <div class="col-md-4">
                <div class="img-container drop-shadow ">
                    <img :src="imageApi + hotel.hotel.hotelImagesLists.filter(obj =>{ return obj.isPrimary == true })[0].id" class="ml-1 img-fluid" />
                </div>
            </div>
            <div class="col-md-8">
                <div class="card-body">
                    <div class="btn btn-danger pos-absolute btn-sm outline shadowed rounded-0 float-end"
                         type="button"
                         data-toggle="tooltip"
                         data-placement="top"
                         @click="RemoveFavorite" style="top:0px;right:5px;">
                        <i class="fa fa-trash"></i>
                    </div>
                    <h5 class="card-title">{{hotel.hotel.name}} <input v-if="hotel.hotel.averageRating > 0" data-role="rating" data-stared-color="#b59a09" :data-value="hotel.hotel.averageRating" data-static="true"></h5>
                    <div class="textS" v-html="($store.state.language == 'cz') ? hotel.hotel.descriptionCz : hotel.hotel.descriptionEn" />


                </div>
                <div class="" style="position: initial;">
                    <div class="p-button p-component button info outline shadowed mt-2 mr-2" type="button" style="position: absolute; bottom: 5px; right: 10px;width: 125px;" @click="hotelDetailsClick" data-pc-name="button">
                        {{$t('labels.seeDetail')}}
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
//import Button from "primevue/button";

export default {
    components: {
        //Button
    },
    data() {
        return {

        }
    },
    props: {
        hotel: {}
    },
    computed: {
        imageApi() {
            return this.$store.state.apiRootUrl + '/Image/';
        },
    },
    methods: {
       hotelDetailsClick(event) {
            this.$store.state.backRoute = "/Profile/Favorite";
            this.$store.state.backRouteScroll = window.scrollY;

            this.$store.dispatch("setHotel", this.hotel.hotel);
            this.$router.push('/Hotels/' + this.hotel.hotel.id);
       },
       async RemoveFavorite() {

            let response = await fetch(this.$store.state.apiRootUrl + '/Guest/SetFavorite/' + this.hotel.hotel.id, {
                method: "GET",
                headers: {
                    "Authorization": 'Bearer ' + this.$store.state.user.Token,
                    "Content-type": "application/json",
                }
            });

            let result = await response.json();
           this.$store.dispatch('setFavoriteList', result);

            ShowNotify('success', this.$i18n.t("messages.removedFromFavorite"));
            await this.$store.dispatch('getFavoriteHotelList');

        }
    }
}
</script>

<style scoped>
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