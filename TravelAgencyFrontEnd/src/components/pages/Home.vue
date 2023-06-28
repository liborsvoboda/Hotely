<template>
    <div>
        <Top />
    </div>
</template>
<script>
    import Top from "./TopResult.vue";

export default {
    components:{
        Top
    }
}
</script>
<style scoped>
    /* #homeDiv{
        margin-top: 500px;
    } */
    figure {
    margin: 0;
}


    #welcome{
        font-size: 4em;
        color:rgb(108 193 109);
        padding-top: 20px;
        text-shadow: 2px 2px black;
    }

    a :hover{
        list-style: none;
    }
</style>