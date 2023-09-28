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
                    <button class="btn btn-danger btn-sm rounded-0 float-end"
                            type="button"
                            data-toggle="tooltip"
                            data-placement="top"
                            @click="RemoveFavorite">
                        <i class="fa fa-trash"></i>
                    </button>
                    <h5 class="card-title">{{hotel.hotel.name}}</h5>
                    <div class="textS" v-html="($store.state.language == 'cz') ? hotel.hotel.descriptionCz : hotel.hotel.descriptionEn" />

                    <Button class="p-button-info mt-2" style="position: absolute; bottom: 5px; right: 20px;width: 125px;" @click="hotelDetailsClick">
                        {{$t('labels.seeDetail')}}
                    </Button>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import Button from "primevue/button";

export default {
    components: {
        Button
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
            this.$store.state.backRoute = "/profile/favorite";
            this.$store.state.backRouteScroll = window.scrollY;

            this.$store.dispatch("setHotel", this.hotel.hotel);
            this.$router.push('/hotels/' + this.hotel.hotel.id);
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

            var notify = Metro.notify; notify.setup({ width: 300, duration: this.$store.state.userSettings.notifyShowTime });
            notify.create(this.$i18n.t("messages.removedFromFavorite"), "Success", { cls: "success" }); notify.reset();
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