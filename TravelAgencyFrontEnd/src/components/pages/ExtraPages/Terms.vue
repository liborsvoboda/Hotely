<template>
    <Card id="card" style="top: 20px;" class="mb-10">
        <template #content>
            <main class="container">
                <div class="row mb-2" v-for="terms in termsList">
                    <div class="col-md-12 d-flex">
                        <div class="col-lg-12 col-md-12" v-html="($store.state.language == 'cz') ? terms.descriptionCz : terms.descriptionEn" />
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
            termsList: []
        }
    },
    created() {
        this.$store
            .dispatch("getTermsList")
            .then(() => {
                this.termsList = this.$store.state.termsList;
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