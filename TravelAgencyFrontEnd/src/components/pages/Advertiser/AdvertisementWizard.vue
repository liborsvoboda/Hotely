<template>
    <div class="card h-100">

        <div class="wizard-wide-fs" data-role="wizard" data-button-mode="square">
            <section>
                <div class="page-content">
                    <form id="hotelForm" method="post"
                          class="login-form bg-white p-1 mx-auto"
                          data-role="validator" action="javascript:" data-clear-invalid="2000" data-on-error-form="invalidForm" data-on-validate-form="validateForm">
                        <h2 class="fg-brandColor2">{{ $t('labels.mainSettings') }}</h2>

                        <div class="form-group">
                            <input id="Name" type="text" data-role="input" :placeholder="$t('labels.insertAdvertisementName')" data-validate="required maxlength=50" data-clear-button="true">
                        </div>

                        <div id="countryGroup" class="form-group">
                            <select class="select" id="Country" :data-filter-placeholder="$t('labels.selectCountry')" data-empty-value="" data-validate="required not=-1" data-clear-button="true">
                                <!-- <option v-for="country in countryList" :value="country.id">{{country.systemName}}</option> -->
                            </select>
                        </div>

                        <div id="cityGroup" class="form-group">
                            <select class="select" id="City" :data-filter-placeholder="$t('labels.selectCity')" data-empty-value="" data-validate="required not=-1" data-clear-button="true">
                                <!-- <option v-for="country in countryList" :value="country.id">{{country.systemName}}</option> -->
                            </select>
                        </div>

                        <div id="currencyGroup" class="form-group">
                            <select class="select" id="Currency" :data-filter-placeholder="$t('labels.selectCurrency')" data-empty-value="" data-validate="required not=-1" data-clear-button="true">
                                <!-- <option v-for="country in countryList" :value="country.id">{{country.systemName}}</option> -->
                            </select>
                        </div>

                        <div class="form-group">
                            <div id="summernote">
                                {{$t('labels.insertDescription')}}
                            </div>
                        </div>
                    </form>
                </div>
            </section>
            <section><div class="page-content">Page 2</div></section>
            <section><div class="page-content">Page 3</div></section>
            <section><div class="page-content">Page 4</div></section>
            <section><div class="page-content">Page 5</div></section>
        </div>
    </div>

    <!--  <BookingMessage @message="updateMessage"></BookingMessage>
 <Button class="mt-3" @click="updateReservation" :loading="loading">{{ $t('labels.newMessage') }}</Button> -->
   
</template>

<script>
    import BookingMessage from '/src/components/pages/CheckoutViewComponents/BookingMessage.vue';
    import Button from 'primevue/button';
    import { ref, watch } from 'vue';
    import store from "../../../store/index";

export default {
    components: {
        BookingMessage,
        Button,
    },
    props: {
        hotel: {}
    },
    data() {
        return {

        }
    },
    computed: {
        info() {
            return this.hotel;
        },
        watchGlobalVariables: function () {
            return window.watchGlobalVariables;
        }
    },
    mounted() {
        this.getCountryList();
        this.getCurrencyList();
        $(document).ready(function () {
            $('#summernote').summernote(this.$store.state.summernoteCfg);
        });
        

    },
    methods: {
        async getCountryList() {
            window.showPageLoading();
            var response = await fetch(
                this.$store.state.apiRootUrl + '/Advertiser/GetCountryList',
                { method: 'GET', headers: { 'Authorization': 'Bearer ' + this.$store.state.user.Token, 'Content-type': 'application/json' } }
            ); let result = await response.json();

            let html = "<select data-role='select' id='Country' data-filter-placeholder='" + this.$i18n.t('labels.selectCountry') + "' data-on-change='WizardRequestCityList' data-empty-value='' data-validate='required' data-clear-button='true' >";
            result.forEach(country => { html += "<option value='" + country.Id + "'>" + country.SystemName + "</option>"; });
            html += "</select>";
            $("#countryGroup").html(html);

            window.hidePageLoading();
        },
        async getCurrencyList() {
            window.showPageLoading();
            var response = await fetch(
                this.$store.state.apiRootUrl + '/Advertiser/GetCurrencyList',
                { method: 'GET', headers: { 'Authorization': 'Bearer ' + this.$store.state.user.Token, 'Content-type': 'application/json' } }
            ); let result = await response.json();
            
            let html = "<select data-role='select' id='Currency' data-filter-placeholder='" + this.$i18n.t('labels.selectCurrency') + "' data-empty-value='' data-validate='required' data-clear-button='true' >";
            result.forEach(currency => { html += "<option value='" + currency.Id + "'>" + currency.Name + "</option>"; });
            html += "</select>";
            $("#currencyGroup").html(html);

            window.hidePageLoading();
        },

    },
    created() {

        //Watcher Initiate for Country selection changed dinaical generate selectbox
        watch(window.watchGlobalVariables, async () => {
            if (window.watchGlobalVariables.wizardRequestCityList.length) {
                window.showPageLoading();
                var response = await fetch(
                    store.state.apiRootUrl + '/Advertiser/GetCityList/' + JSON.parse(JSON.stringify(window.watchGlobalVariables.wizardRequestCityList))[0],
                    { method: 'GET', headers: { 'Authorization': 'Bearer ' + store.state.user.Token, 'Content-type': 'application/json' } }
                ); let result = await response.json();

                let html = "<select data-role='select' id='City' data-filter-placeholder='" + window.dictionary('labels.selectCity') + "' data-empty-value='' data-validate='required' data-clear-button='true' >";
                result.forEach(city => { html += "<option value='" + city.Id + "'>" + city.City + "</option>"; });
                html += "</select>";
                $("#cityGroup").html(html);

                window.hidePageLoading();
            }
        });
    },
};


    
</script>
<style scoped>
    label{
        color:black;
    }

    button.p-button.p-component{
  background: #53c16e;
  border:#14a04d;
  text-decoration: none;
}

.p-button:enabled:hover{
  background:#348047 !important;
  border-color:#14a04d;
  text-decoration: none;
}
</style>