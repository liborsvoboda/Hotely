<template>
    <div class="py-4">
        <div class="row">
            <div class="col-md-6 text-left">
                <h1>{{ $t('labels.showNews') }}</h1>
            </div>
            <div class="col-md-6">
                <div class="dropdown text-right" style="margin-top:10px;">
                    <div class="p-button button dropdown-toggle shadowed" type="button" id="dropdownMenuButton1" data-bs-toggle="dropdown" aria-expanded="false">
                        {{ $t('labels.show') }}
                    </div>
                    <ul class="dropdown-menu" aria-labelledby="dropdownMenuButton1">
                        <li><a class="dropdown-item" v-on:click="showNewest">{{ $t('labels.fiveNewest') }}</a></li>
                        <li><a class="dropdown-item" v-on:click="showCheapest">{{ $t('labels.fiveCheapest') }}</a></li>
                        <li><a class="dropdown-item" v-on:click="showFavoritest">{{ $t('labels.fiveFavoritest') }}</a></li>
                    </ul>
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
    },
    async created() {
        await this.$store.dispatch('getTopFiveList','newest');
    },
    computed: {
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
