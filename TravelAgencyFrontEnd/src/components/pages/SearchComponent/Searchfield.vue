<template>
  <div>
    <input
      class="form-control"
      id="searchInput"
      placeholder="Type to search..."
      @input="searchFieldChange"
      v-model="searchString"
      minlength="2"
    />
    <datalist id="datalistOptions">
        <option :value="item" v-for="item in searchDialList" :key="item.id" />
    </datalist>
  </div>
</template>

<script>

export default {
    data() {
        return {
            searchString: "",
        };
    },
    computed: {
        searchDialList() {
            return this.$store.state.searchDialList;
        },
    },
    mounted() {
        this.GetSearchDialList();
        this.GetPropertyList();
    },
    methods: {
        GetSearchDialList() {
            this.$store.dispatch("getSearchDialList");
        },
         GetPropertyList() {
            this.$store.dispatch("getPropertyList");
        },
        searchFieldChange() {
            this.$emit("input-changed", this.searchString);
                if(this.searchString.length >= 2){
                    var input = document.querySelector("#searchInput")
                input.setAttribute("list", "datalistOptions")
            } else{
                var input = document.querySelector("#searchInput")
                input.setAttribute("list", "")
            }
        }
    },
};
</script>

<style scoped>

</style>