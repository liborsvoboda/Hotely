<template>
    <div>
        <input class="form-control"
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
        this.GetTopList();
        this.GetSearchAreaList();
        this.GetSearchDialList();
        this.GetPropertyList();
        //this.GetRoomTypeList();
        this.searchFieldChange();
    },
    methods: {
        async GetTopList() {
            await this.$store.dispatch("getTopList");
        },
        async GetSearchAreaList() {
            await this.$store.dispatch("getSearchAreaList");
        },
        async GetSearchDialList() {
            await this.$store.dispatch("getSearchDialList");
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