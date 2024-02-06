import { createApp, reactive } from 'vue'
import App from './App.vue'
import router from "./router/index"
import store from "./store/index"

import PrimeVue from 'primevue/config';
import VueCookies from 'vue3-cookies'

import ToastPlugin from 'vue-toast-notification';
import 'vue-toast-notification/dist/theme-bootstrap.css';

import 'primevue/resources/themes/saga-blue/theme.css'
import './index.css'
import 'primevue/resources/primevue.min.css'
import 'primeicons/primeicons.css'
import 'primeflex/primeflex.css';
import 'vite/modulepreload-polyfill'

/*import ConfirmationService from 'primevue/confirmationservice';*/
import { createI18n, useI18n } from 'vue-i18n';

import en from './translation/en.json';
import cz from './translation/cz.json';


if (process.env.NODE_ENV === 'production') {
    store.state.apiRootUrl = 'http://nomad.ubytkac.cz:5000/WebApi'
} else { store.state.apiRootUrl = 'http://localhost:5000/WebApi' }

const i18n = createI18n({
    locale: 'cz',
    messages: { en, cz },
    fallbackLocale: 'cz'
});

let app = createApp(App)
    /*.prototype()*/
    .use(router)
    .use(store)
    .use(PrimeVue)
    .use(VueCookies)
    .use(i18n)
    .use(ToastPlugin)
    ;


//Global Variables For Watch Vue + Metro + other.js
app.config.globalProperties.window = window; 
app.config.globalProperties.window.dictionary = i18n.global.t;
app.config.globalProperties.window.watchGlobalVariables = reactive({
    wizardRequestCityList: null,
    reloadPrivateMessage: false
});

app.config.globalProperties.window.watchChangeVariables = reactive({
    roomShowPreviewEnabled: false,
    propertySelected: false
});

app.config.globalProperties.window.watchAdvertisementVariables = reactive({
    reloadAdvertisement: false,
    reloadUnavailable: false
});



app.mount('#app');