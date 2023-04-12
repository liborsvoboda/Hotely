import { createApp } from 'vue'
import App from './App.vue'
import router from "./router/index"

import store from "./store/index"

import PrimeVue from 'primevue/config';
import VueCookies from 'vue3-cookies'

import 'primevue/resources/themes/saga-blue/theme.css'
import 'primevue/resources/primevue.min.css'
import 'primeicons/primeicons.css'
import 'primeflex/primeflex.css';

import ConfirmationService from 'primevue/confirmationservice';
import { createI18n } from 'vue-i18n';


import en from './translation/en.json';
import cz from './translation/cz.json';

store.state.apiUrls
const i18n = createI18n({
    locale: 'cz',
    messages: { en, cz },
    fallbackLocale: 'cz'
});

createApp(App)
    .use(router)
    .use(store)
    .use(PrimeVue)
    .use(ConfirmationService)
    .use(VueCookies)
    .use(i18n)
    .mount('#app')