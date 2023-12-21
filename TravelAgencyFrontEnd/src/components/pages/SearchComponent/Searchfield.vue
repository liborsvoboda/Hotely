<template>
    <div>
        <input type="text"
                class="form-control"
               id="searchInput"
               :placeholder="$t('labels.insertSearchValue')"
               @input="searchFieldChange"
               v-model="searchString"
               minlength="1" />
        <datalist id="datalistOptions">
            <option :value="item" v-for="item in searchList" :key="item.id" />
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
        searchList() {
            if (this.searchString.length == 0) {
                return this.$store.state.searchAreaList;
            } else {
                return this.$store.state.searchDialList;
            }
        },
        loggedIn() {
            return this.$store.state.user.loggedIn;
        },
    },
    async mounted() {

        // For searching and correct show Loaded only OneTime if []
        this.GetPropertyList();
        this.GetPropertyGroupList();
        this.GetSearchAreaList();
        this.GetSearchDialList();

        //Load Top If nothing loaded
        if (this.$route.fullPath == '/' &&
            (this.$store.state.searchResults == [] || this.$store.state.searchResults.hotelList == undefined || !this.$store.state.searchResults.hotelList.length))
        { this.$store.dispatch("getMainTopList"); }

        this.searchFieldChange();
    },
    methods: {
        async GetMainTopList() {
            await this.$store.dispatch("getMainTopList");
        },
        async GetSearchAreaList() {
            await this.$store.dispatch("getSearchAreaList");
        },
        async GetSearchDialList() {
            await this.$store.dispatch("getSearchDialList");
        },
        async GetPropertyGroupList() {
            await this.$store.dispatch("getPropertyGroupList");
        },
        async GetPropertyList() {
            await this.$store.dispatch("getPropertyList");
        },
        async GetRoomTypeList() {
            await this.$store.dispatch("getRoomTypeList");
        },
        searchFieldChange() {
            this.$emit("input-changed", this.searchString);
            // if(this.searchString.length >= 2){
            var input = document.querySelector("#searchInput")
            input.setAttribute("list", "datalistOptions")
            // } else{
            //     var input = document.querySelector("#searchInput")
            //     input.setAttribute("list", "")
            // }
        }
    },
};
</script>

<style scoped>

</style>