<template>
    <div>
        <div v-if="!advertisementList.length && !errorText">
            <ProgressSpinner />
        </div>
        <div v-if="errorText" class="h2 pt-5">
            <div class="pos-absolute button" style="top:10px;right:10px;">{{ $t('labels.newAccommodationAdvertisement') }}</div>
            <p>{{ $t('messages.anyAdvertisementExist') }}</p>
        </div>

        <div v-else class="h2 pt-5">
            <div class="pos-absolute button" style="top:10px;right:10px;">{{ $t('labels.newAccommodationAdvertisement') }}</div>
            <AdvertisementList v-for="hotel in advertisementList" :hotel="hotel" :key="hotel.id" />
        </div>
        
    </div>
</template>
<script>
    import AdvertisementList from './AdvertisementList.vue'
    import ProgressSpinner from 'primevue/progressspinner';
export default {
    data(){
        return{
            errorText: false
        }
    },
    components:{
        AdvertisementList,
        ProgressSpinner
    },
    async created(){
        
        if (!this.advertisementList.length){
            this.errorText = true;
        }
        
    },
    computed:{
        advertisementList(){
            return this.$store.state.advertisementList;
        }
    }
}
</script>
<style lang=""> 
    
</style>