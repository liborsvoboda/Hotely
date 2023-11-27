<template>
    <div class="py-4">
        <div class="rounded drop-shadow row">
            <div class="row pr-0">
                <div class="col-md-6 d-flex">
                    <div class="mt-2" data-role="hint" :data-hint-text="$t('labels.showAlsoInactive')">
                        <input id="showAlsoInactive" type="checkbox" data-role="checkbox" class="" data-title="Checkbox" :onchange="showAlsoInactive" :checked="$store.state.userSettings.showInactiveAdvertisementAsDefault">
                    </div>

                    <h1>{{ $t('labels.accommodationAdvertisement') }}</h1>
                </div>
                <div class="col-md-6 text-right pt-3 pr-0 mr-0">
                    <button class="p-button p-component button info shadowed" @click="startAdvertisement()">{{ $t('labels.newAccommodationAdvertisement') }}</button>
                    <span class="icon mif-info pl-3 mif-3x c-pointer fg-orange" @click="OpenDocView()" />
                </div>
            </div>
        </div>
        <hr>

        <div v-if="errorText" class="h2 pt-5">
            <p>{{ $t('messages.anyAdvertisementExist') }}</p>
        </div>
        <div v-if="advertisementList.length" class="">
            <AdvertisementList v-for="hotel in advertisementList" :hotel="hotel" :key="hotel.id" />
        </div>
    </div>
</template>

<script>
    import AdvertisementList from './AdvertisementList.vue'
    import ProgressSpinner from 'primevue/progressspinner';
export default {
    data() {
        return {
            errorText: false,
            ShowAlsoInactive: false
        }
    },
    components: {
        AdvertisementList,
        ProgressSpinner,
    },
    async mounted() {
        this.ShowAlsoInactive = this.$store.state.userSettings.showInactiveAdvertisementAsDefault;

        if (this.$store.state.user.UserId != '') {
            await this.$store.dispatch("getAdvertisementList");
        }
    
        if (this.$store.state.statusList.length == 0) {
            await this.$store.dispatch("getStatusList");
        }
    },
    async created() {
        if (!this.advertisementList.length) {
            this.errorText = true;
        }
    },
    computed: {
        advertisementList() {
            if (this.ShowAlsoInactive) { return this.$store.state.advertisementList; }
            else { return this.$store.state.advertisementList.filter(obj => { return obj.deactivated == false; }); }
        }
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
                    that.$store.state.apiRootUrl + '/WebPages/GetWebDocumentationList/Advertisements', {
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
        startAdvertisement() {
            ActualValidationFormName = "hotelForm";
            ActualWizardPage = 1;
            propertyList = [];
            Router = this.$router;
            ApiRootUrl = null;
            HotelRecId = null;
            WizardImageGallery = [];
            WizardRooms = [];
            WizardTempRoomPhoto = [];
            WizardProperties = [];
            WizardSelectedProperty = {};
            WizardHotel = {
                Images: [],
                Rooms: [],
                Properties: []
            };
            this.$router.push('/Profile/AdvertisementWizard');
        },
        async showAlsoInactive() {
            this.ShowAlsoInactive = !this.ShowAlsoInactive;
        }
    },
    watch: {
        advertisementList(newVal) {
            if (this.advertisementList.length) { this.errorText = false; } else { this.errorText = true; }
        }
    }
}
</script>

<style scoped>

img{
    padding-right: 6px;
}

button.p-button.p-component {
    background: #53c16e;
    border: #14a04d;
    text-decoration: none;
}

.p-button:enabled:hover {
    background: #348047 !important;
    border-color: #14a04d;
    text-decoration: none;
}


</style>
