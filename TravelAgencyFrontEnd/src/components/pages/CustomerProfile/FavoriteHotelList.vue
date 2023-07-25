<template>
    <div>
        <div v-if="FavoriteHotelList.length < 1 && !errorText">
            <ProgressSpinner />
        </div>
        <div v-if="errorText">
            <p>{{ $t('messages.anyFavoritefound') }}</p>
            <p>{{ $t('messages.anyFavoritefound1') }}</p>
        </div>
         <FavoriteHotel v-for="hotel in FavoriteHotelList" :hotel="hotel" :key="hotel"/>
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