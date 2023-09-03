<template>
    <div class="card h-100">

        <div id="AdvertisementWizard" data-role="wizard" data-button-outline="false" class="wizard-wide-fs"
             data-button-mode="square" data-on-before-next="return false;" data-on-next-click="$('#'+ActualValidationFormName).submit();" data-on-finish-click="SaveHotel">
            <section>
                <div class="page-content p-0 h-100">


                    <form id="hotelForm" class="login-form bg-white p-1 mx-auto" data-role="validator" action="javascript:" data-clear-invalid="2000" data-on-error-form="WizardValidateForm" data-on-validate-form="WizardValidateForm">
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
                    <form id="galleryForm" class="login-form bg-white p-1 mx-auto" data-role="validator" action="javascript:" data-clear-invalid="2000" data-on-error-form="WizardValidateForm" data-on-validate-form="WizardValidateForm">
                        <h2 class="fg-brandColor2">{{ $t('labels.photoGallery') }}</h2>

                        <div class="d-flex flex-justify-start" style="height:400px;overflow-y:auto;">
                            <div id="galleryContainer" class="d-flex w-100">

                                <div class="d-flex" style="width:250px;height:150px;top:0px;left:0px;">
                                    <input id="Images" type="file" data-role="file" data-mode="drop" data-on-select="WizardUploadImages" data-validate="custom=WizardUploadImagesCheck" accept=".png,.jpg,.jpeg,.tiff" multiple="multiple">
                                </div>

                                <div id="HotelImageGallery" class="d-flex" />

                            </div>
                        </div>
                    </form>
                </div>
            </section>
            <section>
                <div class="page-content p-0 h-100">
                    <div class="page-content p-0">

                        <span class="mif-eye pos-absolute mif-4x fg-blue c-pointer" style="left:5px;top:5px" :onclick="WizardShowRoomPreview" data-role="hint"
                              data-cls-hint="bg-cyan fg-white drop-shadow" :data-hint-text="$t('labels.showPreview')" :class="(!window.watchChangeVariables.roomShowPreviewEnabled ? 'disabled' : '')" />

                        <span class="mif-stack3 pos-absolute mif-4x fg-blue c-pointer" style="left:55px;top:5px" :onclick="WizardInsertNewRoom" data-role="hint"
                              data-cls-hint="bg-cyan fg-white drop-shadow" :data-hint-text="$t('labels.saveNew')" :class="(!window.watchChangeVariables.roomShowPreviewEnabled ? 'disabled' : '')" />

                        <h2 class="fg-brandColor2">{{ $t('labels.equipmentForRent') }}</h2>
                        <div class="d-block" style="height:400px;overflow-y:auto;">

                            <div class="container bg-grayBlue p-1" style="height:200px;overflow-y:auto;">
                                <div id="roomContainer" class="d-flex w-100 ">
                                </div>
                            </div>

                            <form id="roomForm" class="login-form bg-white p-1 mx-auto" data-role="validator" action="javascript:"
                                  data-clear-invalid="2000" data-on-error-form="WizardValidateForm" data-on-validate-form="WizardValidateForm">

                                <div class="row flex-align-center">
                                    <div class="form-group w-50">
                                        <input id="RoomName" type="text" data-role="input" :placeholder="$t('labels.equipmentForRentName')" data-clear-button="true">
                                    </div>

                                    <div id="roomTypeGroup" class="form-group w-50 mt-0">
                                        <select class="select" id="RoomType" :data-filter-placeholder="$t('labels.selectType')" data-empty-value="" data-clear-button="true">
                                            <!-- <option v-for="country in countryList" :value="country.id">{{country.systemName}}</option> -->
                                        </select>
                                    </div>
                                </div>

                                <div class="row flex-align-center">
                                    <div class="form-group w-25" data-role="hint" data-cls-hint="bg-cyan fg-white drop-shadow" :data-hint-text="$t('labels.insertPrice')">
                                        <input id="RoomPrice" type="text" data-role="spinner" data-min-value="1" data-max-value="99999" data-cls-spinner-value="text-bold bg-cyan fg-white"
                                               data-cls-spinner-button="info" data-step="100" data-plus-icon="<span class='mif-plus fg-white'></span>" data-minus-icon="<span class='mif-minus fg-white'></span>">
                                    </div>

                                    <div class="form-group w-25 text-right">
                                        <input id="RoomExtraBed" type="checkbox" data-role="checkbox" :data-caption="$t('labels.extraBed')" data-caption-position="left">
                                    </div>

                                    <div class="form-group w-25" data-role="hint" data-cls-hint="bg-cyan fg-white drop-shadow" :data-hint-text="$t('labels.insertMaxCapacity')">
                                        <input id="RoomMaxCapacity" type="text" data-role="spinner" data-min-value="1" data-max-value="999" data-cls-spinner-value="text-bold bg-cyan fg-white"
                                               data-cls-spinner-button="info" data-plus-icon="<span class='mif-plus fg-white'></span>" data-minus-icon="<span class='mif-minus fg-white'></span>">
                                    </div>

                                    <div class="form-group w-25" data-role="hint" data-cls-hint="bg-cyan fg-white drop-shadow" :data-hint-text="$t('labels.insertRoomsSameCount')">
                                        <input id="RoomsCount" type="text" data-role="spinner" data-min-value="1" data-max-value="999" data-cls-spinner-value="text-bold bg-cyan fg-white"
                                               data-cls-spinner-button="info" data-plus-icon="<span class='mif-plus fg-white'></span>" data-minus-icon="<span class='mif-minus fg-white'></span>">
                                    </div>
                                </div>

                                <div class="row flex-align-center">
                                    <div class="form-group w-50">
                                    </div>

                                    <div class="form-group w-50" data-role="hint" data-cls-hint="bg-cyan fg-white drop-shadow" :data-hint-text="$t('labels.insertRoomImage')">
                                        <input id="roomImage" type="file" data-role="file" data-on-select="WizardRoomUploadImage"
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
                </div>

            </section>
            <section>
                <div class="page-content p-0 h-100">
                    <div class="page-content p-0">
                        <h2 class="fg-brandColor2">{{ $t('labels.servicesAndProperties') }}</h2>
                        <div class="d-block" style="height:400px;overflow-y:auto;">

                            <form id="propertyForm" class="login-form bg-white p-1 mx-auto" data-role="validator" action="javascript:"
                                  data-clear-invalid="2000" data-on-error-form="WizardValidateForm" data-on-validate-form="WizardValidateForm">
                            </form>
                            <div class="mb-5"> </div>
                        </div>
                    </div>
                </div>

            </section>
            <section>
                <div class="page-content p-0 h-100">
                    <div class="page-content p-0">

                        <span class="mif-done_all pos-absolute mif-4x fg-blue c-pointer" style="left:5px;top:5px" :onclick="WizardUpdateProperty" data-role="hint"
                              data-cls-hint="bg-cyan fg-white drop-shadow" :data-hint-text="$t('labels.saveChanges')" :class="(!window.watchChangeVariables.propertySelected ? 'disabled' : '')" />

                        <h2 class="fg-brandColor2">{{ $t('labels.servicesAndPropertiesSettings') }}</h2>
                        <div class="d-block" style="height:400px;overflow-y:auto;">

                            <div class="container bg-grayBlue p-1 text-center" style="height:200px;overflow-y:auto;">
                                <div id="propSettingsContainer" class="d-block w-100 ">
                                </div>
                            </div>

                            <form id="propertySettingsForm" class="login-form bg-white p-1 mx-auto" data-role="validator" action="javascript:"
                                  data-clear-invalid="2000" data-on-error-form="WizardValidateForm" data-on-validate-form="WizardValidateForm">

                                <div class="row flex-align-center">

                                    <div id="PropValueRoot" class="form-group mt-3 w-25 disabled" data-role="hint" data-cls-hint="bg-cyan fg-white drop-shadow" :data-hint-text="$t('labels.insertValue')">
                                        <input id="PropValue" type="text" data-role="spinner" data-min-value="0" data-max-value="99999" data-cls-spinner-value="text-bold bg-cyan fg-white"
                                               data-cls-spinner-button="info" data-step="1" data-plus-icon="<span class='mif-plus fg-white'></span>" data-minus-icon="<span class='mif-minus fg-white'></span>">
                                    </div>

                                    <div id="PropValueRangeRoot" class="form-group w-25 text-right disabled">
                                        <input id="PropValueRange" type="checkbox" data-role="checkbox" :data-caption="$t('labels.range')" data-caption-position="left" :onclick="WizardPropValueRangeChange">
                                    </div>

                                    <div id="PropValueRangeMinRoot" class="form-group w-25 disabled" data-role="hint" data-cls-hint="bg-cyan fg-white drop-shadow" :data-hint-text="$t('labels.insertMinValue')">
                                        <input id="PropValueRangeMin" type="text" data-role="spinner" data-min-value="0" data-max-value="99999" data-cls-spinner-value="text-bold bg-cyan fg-white"
                                               data-cls-spinner-button="info" data-step="100" data-plus-icon="<span class='mif-plus fg-white'></span>" data-minus-icon="<span class='mif-minus fg-white'></span>">
                                    </div>

                                    <div id="PropValueRangeMaxRoot" class="form-group w-25 disabled" data-role="hint" data-cls-hint="bg-cyan fg-white drop-shadow" :data-hint-text="$t('labels.insertMaxValue')">
                                        <input id="PropValueRangeMax" type="text" data-role="spinner" data-min-value="0" data-max-value="99999" data-cls-spinner-value="text-bold bg-cyan fg-white"
                                               data-cls-spinner-button="info" data-step="100" data-plus-icon="<span class='mif-plus fg-white'></span>" data-minus-icon="<span class='mif-minus fg-white'></span>">
                                    </div>
                                </div>

                                <div class="row flex-align-center">
                                    <div class="form-group w-25 mt-3 text-left">
                                        <div id="PropFeeRoot" class="w-50 text-left disabled">
                                            <input id="PropFee" type="checkbox" data-role="checkbox" :data-caption="$t('labels.fee')" data-caption-position="right" :onclick="WizardPropFeeChange">
                                        </div>
                                        <div id="PropFeeRangeRoot" class="w-50 text-left disabled">
                                            <input id="PropFeeRange" type="checkbox" data-role="checkbox" :data-caption="$t('labels.range')" data-caption-position="right" :onclick="WizardPropFeeRangeChange">
                                        </div>
                                    </div>

                                    <div id="PropFeeValueRoot" class="form-group w-25 disabled" data-role="hint" data-cls-hint="bg-cyan fg-white drop-shadow" :data-hint-text="$t('labels.insertFee')">
                                        <input id="PropFeeValue" type="text" data-role="spinner" data-min-value="0" data-max-value="99999" data-cls-spinner-value="text-bold bg-cyan fg-white"
                                               data-cls-spinner-button="info" data-step="100" data-plus-icon="<span class='mif-plus fg-white'></span>" data-minus-icon="<span class='mif-minus fg-white'></span>">
                                    </div>

                                    <div id="PropMinFeeRoot" class="form-group w-25 disabled" data-role="hint" data-cls-hint="bg-cyan fg-white drop-shadow" :data-hint-text="$t('labels.insertMinFeeValue')">
                                        <input id="PropFeeRangeMin" type="text" data-role="spinner" data-min-value="0" data-max-value="99999" data-cls-spinner-value="text-bold bg-cyan fg-white"
                                               data-cls-spinner-button="info" data-step="100" data-plus-icon="<span class='mif-plus fg-white'></span>" data-minus-icon="<span class='mif-minus fg-white'></span>">
                                    </div>

                                    <div id="PropMaxFeeRoot" class="form-group w-25 disabled" data-role="hint" data-cls-hint="bg-cyan fg-white drop-shadow" :data-hint-text="$t('labels.insertMaxFeeValue')">
                                        <input id="PropFeeRangeMax" type="text" data-role="spinner" data-min-value="0" data-max-value="99999" data-cls-spinner-value="text-bold bg-cyan fg-white"
                                               data-cls-spinner-button="info" data-step="100" data-plus-icon="<span class='mif-plus fg-white'></span>" data-minus-icon="<span class='mif-minus fg-white'></span>">
                                    </div>
                                </div>


                            </form>

                            <div class="mb-5"> </div>
                        </div>
                    </div>
                </div>
            </section>
            <section>
                <div class="page-content p-0 h-100">
                    <div class="page-content p-0">
                        <h2 class="fg-brandColor2">{{ $t('labels.previewAndSave') }}</h2>
                        <div class="d-block" style="height:400px;overflow-y:auto;">

                            <div class="container d-block bg-grayBlue p-1" style="height:400px;overflow-y:auto;">
                                <div id="previewContainer" class="d-flow ">
                                </div>
                            </div>

                            <form id="previewForm" class="login-form bg-white p-1 mx-auto" data-role="validator" action="javascript:"
                                  data-clear-invalid="2000" data-on-error-form="WizardValidateForm" data-on-validate-form="WizardValidateForm">
                            </form>
                            <div class="mb-5"> </div>
                        </div>
                    </div>
                </div>
            </section>
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
    import * as WizardFunctions from '../../../assets/js/WizardFunctions';

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
        },
        WizardTempRoomPhoto: function () {
            console.log(window.WizardTempRoomPhoto != undefined && window.WizardTempRoomPhoto.length > 0);
            return window.WizardTempRoomPhoto != undefined && window.WizardTempRoomPhoto.length > 0;
        },
    
    },
    mounted() {
        propertyList = store.state.propertyList;
        ApiRootUrl = store.state.apiRootUrl;
        Token = store.state.user.Token;

        this.getCountryList();
        this.getCurrencyList();
        this.getRoomTypeList();
        this.GenerateProperties();
        
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

        if (WizardHotel != {}) { WizardSetUpdateData();}
        
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

            if (WizardHotel.HotelCountry != undefined) { 
                $("#HotelCountry").val(WizardHotel.HotelCountry);
                WizardRequestCityList([WizardHotel.HotelCountry]);
            }
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

            if (WizardHotel.HotelCurrency != undefined) { $("#HotelCurrency").val(WizardHotel.HotelCurrency); WizardSetUpdateData1(); }
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
        WizardShowRoomPreview() {
            let htmlContent = "<ul class='feed-list'><li class='title'>" + window.dictionary('labels.adPreview') + "</li>";
            htmlContent += "<li><img class='avatar dropshadow' src='" + WizardTempRoomPhoto[0].Attachment + "'>";
            htmlContent += "<span class='label'>" + $("#RoomName").val() + "</span>";
            htmlContent += "<span class='second-label fg-black bold'>" + $("#RoomsCount").val() + "x " + this.$i18n.t('labels.maxCapacity') + ": " + $("#RoomMaxCapacity").val() + ", " + $("#RoomPrice").val() + $("#HotelCurrency")[0].selectedOptions[0].text + " <i class='fas fa-user-alt'></i>" + ($("#RoomExtraBed").val('checked')[0].checked ? " + <span class='mif-hotel mif-3x ' style = 'top:5px;' data-role='hint' data-cls hint='bg-cyan fg-white drop-shadow' data-hint-text='" + this.$i18n.t('labels.extraBed') + "' > </span>" : "") + "</span>";
            htmlContent += "<span class='second-label' style='zoom: 1;'>" + $("#RoomSummernote").summernote('code') + "</span></li>";
            htmlContent += "</ul>";

            Metro.infobox.create(htmlContent, "", {
                closeButton: true,
                type: "info",
                removeOnClose: true,
                height: "auto"
            });
        },
        WizardInsertNewRoom() {
            WizardRooms.push({
                RoomName: $("#RoomName").val(), RoomTypeId: $("#RoomType")[0].selectedOptions[0].value,
                Price: $("#RoomPrice").val(), ExtraBed: $("#RoomExtraBed").val('checked')[0].checked,
                MaxCapacity: $("#RoomMaxCapacity").val(), RoomsCount: $("#RoomsCount").val(),
                Description: $("#RoomSummernote").summernote('code'), FileName: WizardTempRoomPhoto[0].FileName, Attachment: WizardTempRoomPhoto[0].Attachment
            });
            WizardGenerateRooms();
        },
        GenerateProperties() {
            let htmlContent = "";

            let second = false; let prevGroup = "";
            this.$store.state.propertyList.forEach(property => {

                if (prevGroup != property.propertyGroupId && property.propertyGroupId != null) {
                    if (second) { htmlContent += "</div>"; }
                    htmlContent += "<div class='row p-3 flex-align-center text-bold'>" + property.propertyGroup.systemName + "</div>";
                    second = false;
                }
                if (prevGroup != property.propertyGroupId && property.propertyGroupId == null) {
                    if (second) { htmlContent += "</div>"; }
                    htmlContent += "<div class='row p-3 flex-align-center text-bold'>" + window.dictionary('labels.moreFilters') + "</div>";
                    second = false;
                }

                if (!second) { htmlContent += "<div class='row flex-align-center'>"; }

                if (!second) { htmlContent += "<div class='w-50 text-left'><input id='prop_" + property.id + "' type='checkbox' data-role='checkbox' data-caption='" + property.systemName + "' data-caption-position='right'></div>"; }
                else { htmlContent += "<div class='w-50 text-right'><input id='prop_" + property.id + "' type='checkbox' data-role='checkbox' data-caption='" + property.systemName + "' data-caption-position='left'></div>"; }

                if (second) { htmlContent += "</div>"; }
                second = !second; prevGroup = property.propertyGroupId;
            });
            $("#propertyForm").html(htmlContent);
        },
        WizardUpdateProperty() {
            console.log("save changes click");

            WizardSelectedProperty.value = $("#PropValue").val(); WizardSelectedProperty.valueRangeMin = $("#PropValueRangeMin").val(); WizardSelectedProperty.valueRangeMax = $("#PropValueRangeMax").val();
            WizardSelectedProperty.fee = $('#PropFee').val('checked')[0].checked; WizardSelectedProperty.feeValue = $("#PropFeeValue").val();
            WizardSelectedProperty.feeRangeMin = $("#PropFeeRangeMin").val(); WizardSelectedProperty.feeRangeMax = $("#PropFeeRangeMax").val();

            WizardProperties.forEach((property) => { if (property.id == WizardSelectedProperty.id) { property = WizardSelectedProperty; } });
            WizardGeneratePropertySettingsPanel();
            window.watchChangeVariables.propertySelected = false;
        },
        WizardPropFeeChange() {
            if (WizardSelectedProperty.isFeeRangeAllowed) { $("#PropFeeRangeRoot").removeClass("disabled"); } else { $("#PropFeeRangeRoot").addClass("disabled"); }
            if ($('#PropFee').val('checked')[0].checked) { $("#PropFeeValueRoot").removeClass("disabled"); } else { $("#PropFeeValueRoot").addClass("disabled"); }
        },
        WizardPropValueRangeChange() {
            console.log("value range");
            if ($('#PropValueRange').val('checked')[0].checked) {
                $("#PropValueRoot").addClass("disabled"); $("#PropMinValueRoot").removeClass("disabled"); $("#PropMaxValueRoot").removeClass("disabled");
                $("#PropValue").val(null); 
            }
            else { 
                $("#PropValueRoot").removeClass("disabled"); $("#PropMinValueRoot").addClass("disabled"); $("#PropMaxValueRoot").addClass("disabled");
                $("#PropValueRangeMin").val(null);
                $("#PropValueRangeMax").val(null);
            }
        },
        WizardPropFeeRangeChange() {
            console.log("fee range");
            if ($('#PropFeeRange').val('checked')[0].checked) {
                $("#PropFeeValueRoot").addClass("disabled"); $("#PropMinFeeRoot").removeClass("disabled"); $("#PropMaxFeeRoot").removeClass("disabled");
                $("#PropFeeValue").val(null);
            }
            else {
                $("#PropFeeValueRoot").removeClass("disabled"); $("#PropMinFeeRoot").addClass("disabled"); $("#PropMaxFeeRoot").addClass("disabled");
                $("#PropFeeRangeMin").val(null);
                $("#PropFeeRangeMax").val(null);
            }
        }
    
  
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

                if (WizardHotel.HotelCity != undefined) {
                    $("#HotelCity").val(WizardHotel.HotelCity);
                }
                window.hidePageLoading();
            }
        });
    },
    beforeDestroy() {
    }
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