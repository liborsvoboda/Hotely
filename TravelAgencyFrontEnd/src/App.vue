<template>
    <main id="main">

        <Navigator />
        <div class="view pt-4">
            <div class="full-bg-img">
                <div id="AppContainer" class="mask rgba-black-light flex-center">
                    <div class="container" style="margin-top:100px;">
                        <Searcher class="drop-shadow" style="top:20px;height:600px;background-size:cover; background-position: center center;background-attachment:inherit;" />
                        <router-view />
                    </div>
                </div>
            </div>
        </div>
        <Body v-if="showBody" />
        <Footer />
    </main>
</template>

<script>
import Dialog from 'primevue/dialog';
import InputText from 'primevue/InputText';
import Button from 'primevue/button';
import Menu from 'primevue/menu';
import ColorPicker from 'primevue/colorpicker';
import Navigator from '/src/components/Navigator.vue';
import Footer from '/src/components/Footer.vue';
import Body from "/src/components/pages/MainPage/Body.vue";
import Searcher from './components/pages/SearchComponent/Search.vue'

export default {
    name: "App",
    components: {
        Dialog,
        ColorPicker,
        InputText,
        Button,
        Menu,
        Navigator,
        Footer,
        Body,
        Searcher,
    },
    data() {
        return {
        }
    },
    watch: {
        'detectErrorMessage': function (newValue) {
            if (newValue) {
                this.$toast.open({ type: 'error', message: newValue, duration: 5000 });
                this.$store.state.toastErrorMessage = null;
            }
        },
        'detectSuccessMessage': function (newValue) {
            if (newValue) {
                this.$toast.open({ type: 'success', message: newValue, duration: 3000 });
                this.$store.state.toastSuccessMessage = null;
            }
        },
        'detectInfoMessage': function (newValue) {
            if (newValue) {
                this.$toast.open({ type: 'warning', message: newValue, duration: 5000 });
                this.$store.state.toastInfoMessage = null;
            }
        }
    },
    mounted() { 
        if (Metro.storage.getItem('InputBanner', null) != null && Metro.storage.getItem('InputBanner', null).length > 0) { $("#SearchPanel")[0].style.backgroundImage = 'url(' + Metro.storage.getItem('InputBanner', null) + ')'; }
        this.$store.dispatch('startupStorageSetting');
    },
    methods: {
        toggle(event) {
            this.$refs.menu.toggle(event);
        },
        Search(event) {
            this.$store.state.searchButtonLoading = true;
            this.$store.dispatch('searchHotels', this.searchString);
        }
    },
    computed: {
        detectErrorMessage() {
            return this.$store.state.toastErrorMessage;
        },
        detectSuccessMessage() {
            return this.$store.state.toastSuccessMessage;
        },
        detectInfoMessage() {
            return this.$store.state.toastInfoMessage;
        },
        isLoading() {
            return this.$store.state.searchButtonLoading;
        },
        showBody() {
            return this.$route.path == '/' ? true : false;
        }
    },
    created() {

    
        //Hide Google Translate Panel
        // document.addEventListener('click', function() { 
        //     if(document.querySelector("body > div:nth-child(1)").className == "skiptranslate"){
        //         if (document.querySelector("body > div:nth-child(1)").style.display != "none") {
        //             document.querySelector("body > div:nth-child(1)").style.display = "none";
        //         }
        //     }
        // });
        
    },
}


</script>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  /* margin-top: 60px; */
}

.input-group {
  margin-left: auto;
  margin-right: auto;
  display: flex;
  width: fit-content;
  align-content: center;
}

.intro-2 {
    background: url("https://wallpaperaccess.com/full/1198002.jpg")no-repeat center center;
    padding-bottom: 11cm;
    background-size: cover ;
    background-position: top;
    background-attachment: fixed;
    background-repeat: no-repeat;
    background-attachment: fixed;
    border-color: black;
    border-style: solid;
    border-width: 2px 0px 2px 0px;
}

/* button.p-button.p-component{
  background: #53c16e;
  border:#14a04d;
}

.p-button:enabled:hover{
  background:#348047 !important;
  border-color:#14a04d;
} */

.view {
    height: 100%;
    min-height: 900px;
}

audio{
  margin-top: 20px;
}
</style>
