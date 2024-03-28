<template>
    <div class="p-2 rounded drop-shadow shadowed shadow-sm mb-4">

        <div id="testOmega">
            <div class="row">
                <div class="col-md-6">
                    <PhotoSlider :photos="photos" :width="'210px'" :height="'150px'" :key="hotel.id" class="drop-shadow" />
                </div>
                <div class="col-md-6 mb-5">
                    <div class="row">
                        <div class="col-md-6 pt-0 mt-0 text-start">
                            <b data-role="popover" :data-popover-text="'<div v-html='+hotel.descriptionCz+'' " data-popover-hide="3000" data-popover-position="top" data-close-button="true" data-cls-popover=" width-600 drop-shadow">{{ hotel.name }}</b>
                            <div>{{ hotel.city.city }}, {{ hotel.country.systemName }}</div>
                            <p v-if="hotel.hotelReservationReviewLists.length">
                                <input data-role="rating" :data-value="hotel.averageRating" data-stared-color="#b59a09" data-static="true">
                                <!--  {{ $t('labels.averageRating') }}: <span class="rounded-pill"> {{ hotel.averageRating }} z 5 </span> -->
                            </p>
                        </div>

                        <div class=" col-md-6 pt-0 mt-0 text-start">
                            <h5 class="c-pointer" @click="createRoomListBox()" data-role="hint" :data-cls-hint="hintPopupClass + ' drop-shadow'" :data-hint-text="$t('labels.clickForAccommodationSpace')">
                                <small>{{ $t('labels.roomPriceFrom') }}:</small> <b>{{ lowestPrice }} {{ hotel.defaultCurrency.name }}</b>
                            </h5>

                            <p v-for="property in getUsedPropertyGroups" class="c-help" style="margin-bottom:0px;" @click="createPriceInfoBox()" data-role="hint" :data-cls-hint="hintPopupClass + ' drop-shadow'" :data-hint-text="$t('labels.clickForShowPriceList')">
                                <i class="fas fa-check"></i> {{property.name}}
                            </p>
                        </div>
                    </div>
                    <div class="p-button pos-absolute p-component button info outline shadowed" for="btn-check-outlined" @click="hotelDetailsClick" style="bottom:-52px; right:10px;">
                        {{ $t('labels.seeDetail') }}
                    </div>
                </div>
            </div>
        </div>
        <!--  <hr /> -->
    </div>
</template>


<script>
import Info from "../HotelViewComponents/Info.vue";
import PhotoSlider from "../HotelViewComponents/RoomsViewComponents/PhotoSlider.vue";

export default {
    components: {
        Info,
        PhotoSlider,
    },
    data() {
        return {
            infoBox: null,
            roomListBox: null,
        };
    },
    computed: {
        getUsedPropertyGroups() {
            let usedGroups = []; let lastGroup = null;
            this.getPriceListProperties.forEach(property => {
                if (lastGroup == null || (lastGroup != null && property.group != lastGroup)) { usedGroups.push({ name: property.group }); }
                lastGroup = property.group;
            });
            return usedGroups;
        },
        lowestPrice() {
            var rooms = this.hotel.hotelRoomLists;
            rooms.sort((a, b) => {
            return a.price - b.price;
            });
            var min = rooms[0];
            return min.price;
        },
        photos() {
            var photos = [];
            photos.push({ id: this.hotel.id, hotelPhoto:this.$store.state.apiRootUrl + '/Image/' + this.hotel.id + '/' + this.hotel.hotelImagesLists.filter(obj => { return obj.isPrimary === true; })[0].fileName });

            this.hotel.hotelImagesLists.forEach(image => {
                if (!image.isPrimary) { photos.push({ id: this.hotel.id, hotelPhoto: this.$store.state.apiRootUrl + '/Image/' + this.hotel.id + '/' + image.fileName }) }
            });
            // this.hotel.hotelRoomLists.forEach(room => {
            //     photos.push({ id: this.hotel.id, hotelPhoto: this.$store.state.apiRootUrl + '/RoomImage/' + room.id })
            // });
            return photos;
        },
        getPriceListProperties() {
            var priceListProperties = [];
            this.hotel.hotelPropertyAndServiceLists.forEach(property => {
                var valueProperty = this.$store.state.propertyList.filter(obj => { return obj.id === property.propertyOrServiceId; })[0];
                if (property.isAvailable && valueProperty.isBit) {
                    priceListProperties.push({
                        isBit: true,
                        groupSequence: valueProperty.propertyGroup != null ? valueProperty.propertyGroup.sequence : 1000000,
                        group: valueProperty.propertyGroup != null ? valueProperty.propertyGroup.systemName : this.$i18n.t('labels.moreFilters'),
                        name: valueProperty.systemName, fee: property.fee, feeValue: property.feeValue, feeRangeMin: property.feeRangeMin, feeRangeMax: property.feeRangeMax
                    });
                }
                if (property.isAvailable && valueProperty.isValue) {
                    priceListProperties.push({
                        isBit: false,
                        groupSequence: valueProperty.propertyGroup != null ? valueProperty.propertyGroup.sequence : 1000000,
                        group: valueProperty.propertyGroup != null ? valueProperty.propertyGroup.systemName : this.$i18n.t('labels.moreFilters'),
                        name: valueProperty.systemName, value: property.value, unit: valueProperty.propertyOrServiceUnitType.systemName, fee: property.fee, feeValue: property.feeValue, feeRangeMin: property.feeRangeMin, feeRangeMax: property.feeRangeMax
                    });
                }
            });

            priceListProperties = priceListProperties.sort((a, b) => a.groupSequence - b.groupSequence);
            return priceListProperties;
        },
        hintPopupClass() {
            return Metro.storage.getItem('OnMousePopupClasses', 'bg-cyan fg-white');
        }


    },
    props: {
        hotel: {},
    },
    methods: {
        createRoomListBox() {
            if (this.roomListBox != null) { Metro.infobox.close(this.roomListBox); }
            let htmlContent = "<ul class='feed-list'><li class='title'>" + this.$i18n.t('labels.equipmentForRent') + "</li>";
            let lineSeparator = true;
            this.hotel.hotelRoomLists.forEach((room) => { 
                htmlContent += "<li><img class='avatar' src='" + this.$store.state.apiRootUrl + "/RoomImage/" + room.id + "' >";
                htmlContent += "<span class='label'>" + room.name + "</span>";
                htmlContent += "<span class='second-label fg-black bold'>" + room.roomsCount + "x " + this.$i18n.t('labels.maxCapacity') + ": " + room.maxCapacity + " <i class='fas fa-user-alt'></i></span>";
                htmlContent += "<span class='second-label fg-black bold'>" + room.price + " " + this.hotel.defaultCurrency.name + "</span></li>";
            }); htmlContent += "</ul>";

            this.infoBox = Metro.infobox.create(htmlContent , "", {
                closeButton: true,
                type: "",
                removeOnClose: true,
                width:"600",
                height: "auto"
            });
        },
        createPriceInfoBox() {
            if (this.infoBox != null) { Metro.infobox.close(this.infoBox); }
            let htmlContent = "<div class='skill-box bg-transparent'>";
            htmlContent += "<div class='d-flex' ><span class='h5 w-75' > " + this.$i18n.t('labels.servicesAndFacilitiesAvailable') + "</span>";
            htmlContent += "<span id='PriceInfoBoxSwitch' onclick='PriceInfoBoxSwitchView()' class='c-pointer fg-blue w-25 pr-4 text-right'>" + window.dictionary('labels.list') + "</span></div>";

            htmlContent += "<div id='PriceInfoList' style='display: none;'>";
            let lastGroup = null; let newLineSplitter = 0;
            this.getPriceListProperties.forEach((property) => {

                if (lastGroup == null || (lastGroup != null && property.group != lastGroup)) {
                    if (newLineSplitter != 0) { htmlContent += "</div></li>"; } newLineSplitter = 0;

                    if (lastGroup != null) { htmlContent += "</ul></div>"; }


                    htmlContent += "<div  class='label drop-shadow' ><b>" + property.group + "</b><ul class='skills'>";
                }
                if (newLineSplitter == 0 || newLineSplitter == 3) {
                    if (newLineSplitter != 0) { htmlContent += "</div></li>"; }
                    htmlContent += "<li class='d-flex'><div class='d-flex w-100'>";
                }

                htmlContent += "<div class='d-flex w-33'><span class='w-100'>" + property.name + (!property.isBit ? " <span class='rounded-pill'>" + property.value + " " + property.unit : "") + "</span>";
                htmlContent += ((property.fee) ? (property.feeValue != null) ?
                    '<span title=\'' + this.$i18n.t('labels.fee') + '\' class=\'badge bg-green fg-white\' style=\'top:0px;right: 10px;\'>' + property.feeValue + ' ' + this.hotel.defaultCurrency.name + ' </span>'
                    : '<span title=\'' + this.$i18n.t('labels.fee') + '\' class=\'badge bg-green fg-white\' style=\'top:0px;right: 10px;\'>' + property.feeRangeMin + " - " + property.feeRangeMax + ' ' + this.hotel.defaultCurrency.name + ' </span>'
                    : '')
                    + "</div>";

                newLineSplitter += 1;
                lastGroup = property.group;
            });
            htmlContent += "</ul></div></div>";

            htmlContent += "<div id='PriceInfoGroups' style='display: inline;'>";
            lastGroup = null;
            this.getPriceListProperties.forEach((property) => {

                if (lastGroup == null || (lastGroup != null && property.group != lastGroup)) {
                    if (lastGroup != null) { htmlContent += "</ul></div></div>"; }
                    htmlContent += "<div id='priceGroup_" + (property.groupSequence != null ? property.groupSequence : 1000000) + "'  data-role='panel' data-on-expand='CloseOtherPriceListGroups' data-title-caption='" + property.group + "' class='panel drop-shadow' data-cls-title-caption='h5 pl-3 mt-2 text-left' data-collapsed='" + (lastGroup == null ? 'false' : 'true') + "' data-collapsible='true'><div class='info-box-content p-0 m-0' ><ul class='skills'>";
                }

                htmlContent += "<li><div class='d-flex w-100'><span>" + property.name + (!property.isBit ? " <span class='rounded-pill'>" + property.value + " " + property.unit : "") + "</span>" +

                    ((property.fee) ? (property.feeValue != null) ?
                        '<span title=\'' + this.$i18n.t('labels.fee') + '\' class=\'badge bg-green fg-white\' style=\'right: 0px;position: absolute;\'>' + property.feeValue + ' ' + this.hotel.defaultCurrency.name + ' </span>'
                        : '<span title=\'' + this.$i18n.t('labels.fee') + '\' class=\'badge bg-green fg-white\' style=\'right: 0px;position: absolute;\'>' + property.feeRangeMin + " - " + property.feeRangeMax + ' ' + this.hotel.defaultCurrency.name + ' </span>'
                        : '')
                    + "</div></li>";

                lastGroup = property.group;
            });
            htmlContent += "</div>";

            this.infoBox = Metro.infobox.create(htmlContent + "</div>", "", {
                closeButton: true,
                type: "",
                removeOnClose: true,
                width: "400",
                height: "auto",
                tag: window.dictionary('labels.groups')
            }); this.infoBox.id("PriceInfoBox");

        },
        hotelDetailsClick(event) {
            this.$store.state.backRoute = "/Result";
            this.$store.state.backRouteScroll = window.scrollY;

            this.$store.dispatch("setHotel", this.hotel);
            this.$router.push('/Hotels/' + this.hotel.id);
        },
    },
};
</script>

<style scoped>
#testOmega {
  opacity: 100% !important;
}

.btn.btn-primary{
    background-color:#53c16e;
    border-color:#1bc541;
  }

a.nav-link {
  margin-left: 35mm;
}

.p-4 {
  margin-top: 15px;
  background-color: rgb(241, 241, 241);
}
</style>