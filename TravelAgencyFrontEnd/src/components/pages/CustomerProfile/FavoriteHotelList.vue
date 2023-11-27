<template>
    <div>
        <div class="rounded drop-shadow row">
            <div class="col-md-6 text-left">
                <h1>{{ $t('user.favorites') }}</h1>
            </div>
            <div class="col-md-6 text-right">
                <span class="icon mif-info pt-3 mif-3x c-pointer fg-orange" @click="OpenDocView()" />
            </div>
        </div>
        <div v-if="FavoriteHotelList.length < 1 && !errorText">
            <!-- <ProgressSpinner /> -->
        </div>
        <div v-if="errorText">
            <p>{{ $t('messages.anyFavoritefound') }}</p>
            <p>{{ $t('messages.anyFavoritefound1') }}</p>
        </div>
        <FavoriteHotel v-for="hotel in FavoriteHotelList" :hotel="hotel" :key="hotel" />
    </div>
</template>
<script>
import FavoriteHotel from './FavoriteHotel.vue';
import ProgressSpinner from 'primevue/progressspinner';
export default {
    data(){
        return{
            errorText: false,
        }
    },
    components:{
        FavoriteHotel,
        ProgressSpinner,
    },
    methods: {
        OpenDocView() {
            Metro.window.create({
                title: "Nápověda", shadow: true, draggable: true, modal: false, icon: "<span class=\"mif-info\"</span>",
                btnClose: true, width: 1000, height: 680, place: "top-center", btnMin: false, btnMax: false, clsWindow: "", dragArea: "#AppContainer",
                content: "<iframe id=\"DocView\" height=\"650\" style=\"width:100%;height:650px;\"></iframe>"
            });

            var that = this;
            setTimeout(async function () {
                window.showPageLoading();
                let response = await fetch(
                    that.$store.state.apiRootUrl + '/WebPages/GetWebDocumentationList/Favorites', {
                    method: 'GET', headers: { 'Content-type': 'application/json' }
                }); let result = await response.json();

                if (result.Status == "error") {
                    var notify = Metro.notify; notify.setup({ width: 300, timeout: that.$store.state.userSettings.notifyShowTime, duration: 500 });
                    notify.create(result.ErrorMessage, "Error", { cls: "alert" }); notify.reset();
                } else {
                    document.getElementById("DocView").srcdoc = result;
                }
                window.hidePageLoading();
            }, 1000);

        },
    },
    async created(){
        await this.$store.dispatch('getFavoriteHotelList');

        if(!this.FavoriteHotelList.length){
            this.errorText = true;
        }
        
    },
    computed:{
        FavoriteHotelList(){
            return this.$store.state.favoriteHotelList;
        }
    }, 
    unmounted(){
    }
}
</script>
<style lang="">
    
</style>