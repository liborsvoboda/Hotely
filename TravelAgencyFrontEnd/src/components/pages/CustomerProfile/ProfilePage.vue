<template>
    <div class="py-4 mt-4">
        <div class="rounded drop-shadow row">
            <div class="col-md-6 text-left">
                <h1>{{ $t('labels.showNews') }}</h1>
            </div>
            <div class="col-md-6">
                <div class="dropdown text-right" style="margin-top:10px;">
                    <div class="p-button button dropdown-toggle shadowed" type="button" id="dropdownMenuButton1" data-bs-toggle="dropdown" aria-expanded="false">
                        {{ $t('labels.show') }}
                    </div>
                    <ul class="dropdown-menu" aria-labelledby="dropdownMenuButton1">
                        <li><a class="dropdown-item" v-on:click="showNewest">{{ userSettings.topFiveCount }} {{ $t('labels.fiveNewest') }}</a></li>
                        <li><a class="dropdown-item" v-on:click="showCheapest">{{ userSettings.topFiveCount }} {{ $t('labels.fiveCheapest') }}</a></li>
                        <li><a class="dropdown-item" v-on:click="showFavoritest">{{ userSettings.topFiveCount }} {{ $t('labels.fiveFavoritest') }}</a></li>
                        <li><a class="dropdown-item" v-on:click="showBestReviews">{{ userSettings.topFiveCount }} {{ $t('labels.fiveBestScore') }}</a></li>
                    </ul>
                    <span class="icon mif-info pl-3 pt-0 mif-3x c-pointer fg-orange" @click="OpenDocView()" />
                </div>
            </div>
        </div>
        <hr>
        <div v-if="TopFiveList.length == 0">
            <!-- <ProgressSpinner /> -->
        </div>
        <TopFive v-for="result in TopFiveList"
                 :hotel="result.hotel"
                 :key="result.hotel.id" />
    </div>
</template>

<script>
import TopFive from './TopFive.vue';
import ProgressSpinner from 'primevue/progressspinner';
export default {
    data() {
        return {
            
        }
    },
    components: {
        TopFive,
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
        async showNewest() {
            this.$store.state.topFiveList = [];
            await this.$store.dispatch('getTopFiveList', 'newest');
        },
        async showCheapest() {
            this.$store.state.topFiveList = [];
            await this.$store.dispatch('getTopFiveList', 'cheapest');
        },
        async showFavoritest() {
            this.$store.state.topFiveList = [];
            await this.$store.dispatch('getTopFiveList', 'favoritest');
        },
        async showBestReviews() {
            this.$store.state.topFiveList = [];
            await this.$store.dispatch('getTopFiveList', 'bestreview');
        },
        
    },
    async created() {
        await this.$store.dispatch('getTopFiveList','newest');
    },
    computed: {
        userSettings() {
            return this.$store.state.userSettings;
        },
        TopFiveList() {
            return this.$store.state.topFiveList;
        }
    },
    unmounted() {
    }
}
</script>

<style scoped>

img{
    padding-right: 6px;
}

</style>
