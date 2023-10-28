<template>
    <div class="py-4">
        <div class="rounded drop-shadow row">
            <div class="row">
                <div class="col-md-6 d-flex">
                    <div class="mt-2" data-role="hint" :data-hint-text="$t('labels.showAlsoInactive')">
                        <input id="showAlsoInactive" type="checkbox" data-role="checkbox" class="" data-title="Checkbox" :onchange="showAlsoInactive" :checked="$store.state.userSettings.showInactiveAdvertisementAsDefault">
                    </div>

                    <h1>{{ $t('labels.accommodationAdvertisement') }}</h1>
                </div>
                <div class="col-md-6">
                    <div class="pos-absolute p-button p-component button info shadowed" style="top:17px;right:0px;" @click="startAdvertisement()">{{ $t('labels.newAccommodationAdvertisement') }}</div>
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

</style>
