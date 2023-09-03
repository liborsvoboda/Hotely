<template>
    <div class="py-4">
        <div class="row">
            <div class="col-md-6 d-flex">
                <span :title ="$t('labels.showAlsoInactive')" style="zoom:1.5;">
                    <input id="showAlsoInactive" type="checkbox" data-role="checkbox" class="" data-title="Checkbox" :onchange="showAlsoInactive">
                </span>
                
                <h1>{{ $t('labels.accommodationAdvertisement') }}</h1>
            </div>
            <div class="col-md-6">
                <div class="pos-absolute p-button p-component button info" style="top:10px;right:10px;" @click="$router.push('/profile/advertisementWizard')">{{ $t('labels.newAccommodationAdvertisement') }}</div>
            </div>
        </div>
        <hr>
        <!--<div v-if="advertisementList.length && !errorText">
                <ProgressSpinner />
        </div> -->
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
    methods: {

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
            //return this.$store.state.advertisementList;
        }
    },
    methods: {
        async showAlsoInactive() {
            this.ShowAlsoInactive = !this.ShowAlsoInactive;
        }
    },
    async mounted() {
        if (this.$store.state.user.UserId != '') {
            await this.$store.dispatch("getAdvertisementList");
            if (!this.$store.state.advertisementList.length) { this.errorText = true; }
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
