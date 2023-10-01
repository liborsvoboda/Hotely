<template>
    <Card id="card" style="top: 20px;" class="mb-10">
        <template #content>
            <main class="container">

                <div class="row mb-2" v-for="oftenQuestion in oftenQuestionList">
                    <div class="col-md-12 d-flex">
                        <div class="col-lg-3 col-md-3 meta-details">
                            <div class="user-details row">
                                <p class="user-name col-lg-12 col-md-12 col-6">
                                    <!-- <a href="#">Random Blogger</a> -->
                                    {{new Date(oftenQuestion.timeStamp).toLocaleDateString('cs-CZ')}}
                                </p>
                                <p class="user-name col-lg-12 col-md-12 col-6">
                                    <!-- <a href="#">Random Blogger</a> -->
                                    Reviews
                                </p>
                                <p class="user-name col-lg-12 col-md-12 col-6">
                                    <a href="#">Comments</a>
                                </p>
                            </div>
                        </div>

                        <div class="col-lg-9 col-md-9" v-html="($store.state.language == 'cz') ? oftenQuestion.descriptionCz : oftenQuestion.descriptionEn" />

                    </div>
                </div>

            </main>
        </template>
    </Card>
</template>


<script>
import Card from "primevue/card";

export default {
    components: {
        Card,
    },
    data() {
        return {
            oftenQuestionList: []
        }
    },
    created() {
        this.$store
            .dispatch("getOftenQuestionList")
            .then(() => {
                this.oftenQuestionList = this.$store.state.oftenQuestionList;
            });
    }
};
</script>

<style scoped>
#card {
  border-radius: 20px;
  margin-top: 30px;
}

.bd-placeholder-img {
  font-size: 1.125rem;
  text-anchor: middle;
  -webkit-user-select: none;
  -moz-user-select: none;
  user-select: none;
}

@media (min-width: 768px) {
  .bd-placeholder-img-lg {
    font-size: 3.5rem;
  }
}
</style>