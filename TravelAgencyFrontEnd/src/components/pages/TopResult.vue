<template>
    <Card id="test3" v-if="searchResults != null">
        <template #content>
            <div class="row">
                <div class="col-md-12">
                    <Top v-for="result in searchResults"
                         :hotel="result.hotel"
                         :key="result.hotel.id" />
                </div>
            </div>
        </template>
    </Card>
    <div v-else style="border-radius: 20px; margin-top: 20px;">
        <div class="row">
            <ProgressSpinner />
        </div>
    </div>
</template>

<script>
    import Top from './SearchResultComponents/Top.vue'
    import ProgressSpinner from 'primevue/progressspinner';
    import Card from 'primevue/card';

export default ({
    components:{
        Top,
        Card,
        ProgressSpinner
    },
    data(){
        return{
        }
    },
    computed: {
        searchResults() {
            return this.$store.state.searchResults.hotelList;
        },
    },
    methods: {
    },
})
</script>

<style scoped>

#test3 {
    border-radius: 20px;
    margin-top: 20px;
}

.dropdown {
    margin-top: 35px;
}

div.dropdown {
    margin-left: 7cm;
}

a.dropdown-item {
    color: black !important;
}

</style>