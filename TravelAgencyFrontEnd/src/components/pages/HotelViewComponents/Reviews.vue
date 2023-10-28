<template>
    <div class="pt-4 mt-0">
        <h5>{{ $t('labels.averageRating') }}:</h5>
        <input data-role="rating" :data-value="hotel.averageRating" data-static="true">
        <div class="row mb-2" v-for="review in hotel.hotelReservationReviewLists">
            <div class="col-md-12 d-flex">
                <div class="col-lg-3 col-md-3 meta-details">
                    <div class="user-details row">
                        <p class="user-name col-lg-12 col-md-12 col-6">
                            {{new Date(review.timestamp).toLocaleString('cs-CZ')}}
                        </p>
                        <p class="user-name col-lg-12 col-md-12 col-6">
                            <input data-role="rating" :data-value="review.rating" data-stared-color="cyan" data-static="true">
                        </p>
                    </div>
                </div>
                <div class="col-lg-9 col-md-9 text-left">
                    <div class="user-details row">
                        <div class="user-name col-lg-12 col-md-12 col-6">
                            <span class="h5">{{ $t('labels.comment') }}: </span>
                            {{review.description}}
                        </div>
                        <div v-if="review.answer != null && review.answer.length > 0" class="user-name col-lg-12 col-md-12 col-6">
                            <span class="h6">{{ $t('labels.answer') }}:</span>
                            {{review.answer}}
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>

    export default {
        components: {

        },
        mounted() {

        },
        data() {
            return {

            };
        },
        computed: {
            hotel() {
                return this.$store.state.hotel;
            },
        },
        methods: {

        },
    };
</script>

<style scoped>
    .btn.btn-primary {
        background-color: #53c16e;
        border-color: #1bc541;
    }
</style>