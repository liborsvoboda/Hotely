<template>
    <div class="card h-100">

        <div id="AdvertisementWizard" data-role="wizard" data-button-outline="false" class="wizard-wide-fs"
             data-button-mode="square" data-on-before-next="return ValidationWizardStatus;" data-on-next-click="$('#'+ActualValidationFormName).submit();">
            <section>
                <div class="page-content p-0 h-100">
                    

                    <form id="hotelForm" class="login-form bg-white p-1 mx-auto" data-role="validator" action="javascript:" data-clear-invalid="2000" data-on-error-form="InvalidForm" data-on-validate-form="ValidateForm">
                        <h2 class="fg-brandColor2">{{ $t('labels.mainSettings') }}</h2>

                        <div class="row flex-align-center">
                            <div class="form-group w-50">
                                <input id="HotelName" type="text" data-role="input" :placeholder="$t('labels.insertAdvertisementName')"
                                       data-validate="required maxlength=50" data-clear-button="true">
                            </div>

                            <div class="form-group w-50">
                                <div id="currencyGroup" class="form-group">
                                    <select class="select" id="HotelCurrency" :data-filter-placeholder="$t('labels.selectCurrency')"
                                            data-empty-value="" data-validate="required not=-1" data-clear-button="true">
                                        <!-- <option v-for="country in countryList" :value="country.id">{{country.systemName}}</option> -->
                                    </select>
                                </div>
                            </div>
                        </div>

                        <div class="row flex-align-center">
                            <div id="countryGroup" class="form-group w-50">
                                <select class="select" id="HotelCountry" :data-filter-placeholder="$t('labels.selectCountry')"
                                        data-empty-value="" data-validate="required not=-1" data-clear-button="true">
                                    <!-- <option v-for="country in countryList" :value="country.id">{{country.systemName}}</option> -->
                                </select>
                            </div>

                            <div id="cityGroup" class="form-group w-50 mt-0" data-role="hint" data-cls-hint="bg-cyan fg-white drop-shadow" :data-hint-text="$t('labels.selectCity')">
                                <select class="select" id="HotelCity" :data-filter-placeholder="$t('labels.selectCity')"
                                        data-empty-value="" data-validate="required not=-1" data-clear-button="true">
                                    <!-- <option v-for="country in countryList" :value="country.id">{{country.systemName}}</option> -->
                                </select>
                            </div>
                        </div>

                        <div class="form-group mb-5">
                            <div id="HotelSummernote">
                                {{$t('labels.insertDescription')}}
                            </div>
                        </div>
                    </form>
                </div>
            </section>
            <section>
                <div class="page-content p-0 h-100">
                    <form id="galleryForm" class="login-form bg-white p-1 mx-auto" data-role="validator" action="javascript:" data-clear-invalid="2000" data-on-error-form="InvalidForm" data-on-validate-form="ValidateForm">
                        <h2 class="fg-brandColor2">{{ $t('labels.photoGallery') }}</h2>

                        <div class="d-flex flex-justify-start" style="height:400px;overflow-y:auto;">
                            <div id="galleryContainer" class="d-flex w-100">

                                <div class="d-flex" style="width:250px;height:150px;top:0px;left:0px;">
                                    <input id="Images" type="file" data-role="file" data-mode="drop" data-on-select="WizardUploadImages" data-validate="custom=WizardUploadImagesCheck" accept=".png,.jpg,.jpeg,.tiff" multiple="multiple">
                                </div>

                                <div id="ImageGallery" class="d-flex" />

                            </div>
                        </div>


                    </form>
                </div>
            </section>
            <section>
                <div class="page-content p-0 h-100">
                    <div class="page-content p-0">

                        <span class="mif-eye pos-absolute mif-4x fg-blue c-pointer" style="left:5px;top:5px" data-role="hint" data-cls-hint="bg-cyan fg-white drop-shadow" :data-hint-text="$t('labels.showPreview')" onclick="WizardShowRoomPreview" />
                        <span class="mif-stack3 pos-absolute mif-4x fg-blue c-pointer" style="left:55px;top:5px" data-role="hint" data-cls-hint="bg-cyan fg-white drop-shadow" :data-hint-text="$t('labels.saveNew')" onclick="WizardInsertNewRoom" />
                        
                        <h2 class="fg-brandColor2">{{ $t('labels.equipmentForRent') }}</h2>
                        <div class="d-block" style="height:400px;overflow-y:auto;">

                            <div class="container bg-grayBlue" style="height:200px;">
                                <div id="roomContainer" class="d-flex w-100 ">

                                    <div data-role="panel" data-title-caption="Panel title" data-cls-panel="shadow-3" data-cls-content="bg-cyan fg-white"
                                         data-collapsible="true" data-custom-buttons="WizardRoomPanelCustomButton">
                                        ...
                                    </div>

                                </div>
                            </div>

                            <form id="roomForm" class="login-form bg-white p-1 mx-auto" data-role="validator" action="javascript:"
                                  data-clear-invalid="2000" data-on-error-form="InvalidForm" data-on-validate-form="ValidateForm">

                                <div class="row flex-align-center">
                                    <div class="form-group w-50">
                                        <input id="RoomName" type="text" data-role="input" :placeholder="$t('labels.equipmentForRentName')" data-validate="required maxlength=50" data-clear-button="true">
                                    </div>

                                    <div id="roomTypeGroup" class="form-group w-50 mt-0">
                                        <select class="select" id="RoomType" :data-filter-placeholder="$t('labels.selectType')" data-empty-value="" data-validate="required not=-1" data-clear-button="true">
                                            <!-- <option v-for="country in countryList" :value="country.id">{{country.systemName}}</option> -->
                                        </select>
                                    </div>
                                </div>


                                <div class="row flex-align-center">
                                    <div class="form-group w-25" data-role="hint" data-cls-hint="bg-cyan fg-white drop-shadow" :data-hint-text="$t('labels.insertPrice')">
                                        <input id="RoomPrice" type="text" data-role="spinner" data-min-value="1" data-max-value="99999" data-validate="required"
                                               data-cls-spinner-value="text-bold bg-cyan fg-white" data-cls-spinner-button="info" data-step="100"
                                               data-plus-icon="<span class='mif-plus fg-white'></span>" data-minus-icon="<span class='mif-minus fg-white'></span>">
                                    </div>

                                    <div class="form-group w-25 text-right">
                                        <input id="RoomExtraBed" type="checkbox" data-role="checkbox" :data-caption="$t('labels.extraBed')" data-caption-position="left">
                                    </div>

                                    <div class="form-group w-25" data-role="hint" data-cls-hint="bg-cyan fg-white drop-shadow" :data-hint-text="$t('labels.insertMaxCapacity')">
                                        <input id="RoomMaxCapacity" type="text" data-role="spinner" data-min-value="1" data-max-value="999" data-validate="required"
                                               data-cls-spinner-value="text-bold bg-cyan fg-white" data-cls-spinner-button="info"
                                               data-plus-icon="<span class='mif-plus fg-white'></span>" data-minus-icon="<span class='mif-minus fg-white'></span>">
                                    </div>

                                    <div class="form-group w-25" data-role="hint" data-cls-hint="bg-cyan fg-white drop-shadow" :data-hint-text="$t('labels.insertRoomsSameCount')">
                                        <input id="RoomsCount" type="text" data-role="spinner" data-min-value="1" data-max-value="999" data-validate="required"
                                               data-cls-spinner-value="text-bold bg-cyan fg-white" data-cls-spinner-button="info"
                                               data-plus-icon="<span class='mif-plus fg-white'></span>" data-minus-icon="<span class='mif-minus fg-white'></span>">
                                    </div>
                                </div>


                                <div class="row flex-align-center">
                                    <div class="form-group w-50">
                                    </div>

                                    <div class="form-group w-50" data-role="hint" data-cls-hint="bg-cyan fg-white drop-shadow" :data-hint-text="$t('labels.insertRoomImage')">
                                        <input id="roomImage" type="file" data-role="file" data-on-select="WizardUploadRoomImage"
                                               data-button-title="<span class='mif-folder'></span>" accept=".png,.jpg,.jpeg,.tiff">
                                    </div>
                                </div>


                                <div class="row flex-align-center mb-5">
                                    <div class="form-group w-100">
                                        <div id="RoomSummernote">
                                            {{$t('labels.insertDescription')}}
                                        </div>
                                    </div>
                                </div>


                            </form>
                        </div>
                    </div>
                    <!-- <span class="mif-done_all pos-absolute mif-5x fg-blue" style="right:20px;bottom:5px"></span> -->
                </div>

            </section>
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
        let WizardScript = document.createElement('script');
        WizardScript.setAttribute('src', window.location.origin + '/src/assets/js/wizardFunctions.js');
        document.head.appendChild(WizardScript);

        this.getCountryList();
        this.getCurrencyList();
        this.getRoomTypeList();
        
        $('#HotelSummernote').summernote({tabsize: 2,height: 150, maxHeight: 150,
            toolbar: [['style', ['style']],['font', ['bold', 'underline', 'clear']],['fontname', ['fontname']],
                ['fontsize', ['fontsize']],['color', ['color']],['para', ['ul', 'ol', 'paragraph']],['table', ['table']],
                ['insert', ['link', 'picture', 'video']],['view', ['fullscreen', 'codeview', 'undo', 'redo', 'help']]]
        });
        $('#RoomSummernote').summernote({tabsize: 2, height: 150, maxHeight: 150,
            toolbar: [['style', ['style']], ['font', ['bold', 'underline', 'clear']], ['fontname', ['fontname']],
            ['fontsize', ['fontsize']], ['color', ['color']], ['para', ['ul', 'ol', 'paragraph']], ['table', ['table']],
            ['insert', ['link', 'picture', 'video']], ['view', ['fullscreen', 'codeview', 'undo', 'redo', 'help']]]
        });
        
    },
    methods: {
        async getCountryList() {
            window.showPageLoading();
            var response = await fetch(
                this.$store.state.apiRootUrl + '/Advertiser/GetCountryList',
                { method: 'GET', headers: { 'Authorization': 'Bearer ' + this.$store.state.user.Token, 'Content-type': 'application/json' } }
            ); let result = await response.json();

            let html = "<select data-role='select' id='HotelCountry' data-filter-placeholder='" + this.$i18n.t('labels.selectCountry') + "' data-on-change='WizardRequestCityList' data-empty-value='' data-validate='required' data-clear-button='true' >";
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
            
            let html = "<select data-role='select' id='HotelCurrency' data-filter-placeholder='" + this.$i18n.t('labels.selectCurrency') + "' data-empty-value='' data-validate='required' data-clear-button='true' >";
            result.forEach(currency => { html += "<option value='" + currency.Id + "'>" + currency.Name + "</option>"; });
            html += "</select>";
            $("#currencyGroup").html(html);

            window.hidePageLoading();
        },
        async getRoomTypeList() {
            window.showPageLoading();
            var response = await fetch(
                this.$store.state.apiRootUrl + '/Advertiser/GetRoomTypeList/' + this.$store.state.language,
                { method: 'GET', headers: { 'Authorization': 'Bearer ' + this.$store.state.user.Token, 'Content-type': 'application/json' } }
            ); let result = await response.json();

            let html = "<select data-role='select' id='RoomType' data-filter-placeholder='" + this.$i18n.t('labels.selectType') + "' data-empty-value='' data-validate='required' data-clear-button='true' >";
            result.forEach(room => { html += "<option value='" + room.Id + "'>" + room.SystemName + "</option>"; });
            html += "</select>";
            $("#roomTypeGroup").html(html);

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

                let html = "<select data-role='select' id='HotelCity' data-filter-placeholder='" + window.dictionary('labels.selectCity') + "' data-empty-value='' data-validate='required' data-clear-button='true' >";
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