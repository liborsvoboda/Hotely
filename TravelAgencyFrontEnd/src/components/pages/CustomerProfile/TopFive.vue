<template>
    <div class="p-4 rounded shadow-sm">
        <div id="testOmega">
            <div class="row">
                <div class="col-md-4">
                    <PhotoSlider :photos="photos" :width="'210px'" :height="'150px'" :key="hotel.id" />
                </div>
                <div class="col-md-8">
                    <div class="row">
                        <div class="col-md-6 pt-0 mt-0 text-start">
                            <b>{{ hotel.name }}</b>
                            <p>{{ hotel.city.city }}, {{ hotel.country.systemName }}</p>
                            <p>
                                {{ $t('labels.ratings') }}:
                                <span class="rounded-pill ">
                                    {{ hotel.averageRating }}
                                </span>
                            </p>
                            <p v-for="property in valueProperties.slice(0, 2)" class="c-help" style="margin-bottom:0px;" @click="createValueInfoBox()"
                               :title="(property.fee) ? (property.feeValue != null) ? $t('labels.fee') + ' ' + property.feeValue + ' ' + hotel.defaultCurrency.name : $t('labels.fee') + ' ' + property.feeRangeMin + ' - ' + property.feeRangeMax + ' ' + hotel.defaultCurrency.name : ''">
                                {{property.name}}:
                                <span class="rounded-pill">
                                    {{ property.value }} {{ property.unit }}
                                </span>
                            </p>
                        </div>

                        <div class="col-md-6 pt-0 mt-0 text-start">
                            <h5>
                                <small>{{ $t('labels.roomPriceFrom') }}:</small> <b>{{ lowestPrice }} {{ hotel.defaultCurrency.name }}</b>
                            </h5>

                            <p v-for="property in bitProperties.slice(0, 4)" class="c-help" style="margin-bottom:0px;" @click="createBitInfoBox()"
                               :title="(property.fee) ? (property.feeValue != null) ? $t('labels.fee') + ' ' + property.feeValue + ' ' + hotel.defaultCurrency.name : $t('labels.fee') + ' ' + property.feeRangeMin + ' - ' + property.feeRangeMax + ' ' + hotel.defaultCurrency.name : ''">
                                <i class="fas fa-check"></i> {{property.name}}
                            </p>
                            <br />
                            <button class="btn btn-primary"
                                    for="btn-check-outlined"
                                    @click="hotelDetailsClick">
                                {{ $t('labels.seeDetail') }}
                            </button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>


<script>
import PhotoSlider from "../HotelViewComponents/RoomsViewComponents/PhotoSlider.vue";

export default {
    components: {
        PhotoSlider,
    },
    data() {
        return {
            infoBox: null,
        };
    },
    props: {
        hotel: {},
    },
    computed: {
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
            photos.push({ id: this.hotel.id, hotelPhoto: this.$store.state.apiRootUrl + '/Image/' + this.hotel.id + '/' + this.hotel.hotelImagesLists.filter(obj => { return obj.isPrimary === true; })[0].fileName });

            this.hotel.hotelImagesLists.forEach(image => {
                if (!image.isPrimary) { photos.push({ id: this.hotel.id, hotelPhoto: this.$store.state.apiRootUrl + '/Image/' + this.hotel.id + '/' + image.fileName }) }
            });
            return photos;
        },
        valueProperties() {
            var valueProperties = [];
            this.hotel.hotelPropertyAndServiceLists.forEach(property => {
                var valueProperty = this.$store.state.propertyList.filter(obj => { return obj.id === property.propertyOrServiceId; })[0];
                if (property.isAvailable && valueProperty.isValue) { valueProperties.push({ name: valueProperty.systemName, value: property.value, unit: valueProperty.propertyOrServiceUnitType.systemName, fee: property.fee, feeValue: property.feeValue, feeRangeMin: property.feeRangeMin, feeRangeMax: property.feeRangeMax }); }
            });
            return valueProperties;
        },
        bitProperties() {
            var bitProperties = [];
            this.hotel.hotelPropertyAndServiceLists.forEach(property => {
                var valueProperty = this.$store.state.propertyList.filter(obj => { return obj.id === property.propertyOrServiceId; })[0];
                if (property.isAvailable && valueProperty.isBit) { bitProperties.push({ name: valueProperty.systemName, fee: property.fee, feeValue: property.feeValue, feeRangeMin: property.feeRangeMin, feeRangeMax: property.feeRangeMax }); }
            });
            return bitProperties;
        }

    },
    methods: {
        createBitInfoBox() {
            if (this.infoBox != null) { Metro.infobox.close(this.infoBox); }

            let htmlContent = "<div class='skill-box bg-transparent'><h5>" + this.$i18n.t('labels.servicesAndFacilitiesAvailable') + "</h5><ul class='skills'>";
            let lineSeparator = true;
            this.bitProperties.forEach((property) => {
                htmlContent += lineSeparator ? "<li><div class='d-flex w-100'>" : "";
                htmlContent += "<div class='d-flex w-50'><span>" + property.name + "</span>" +
                    ((property.fee) ? (property.feeValue != null) ?
                        '<span title=\'' + this.$i18n.t('labels.fee') + '\' class=\'badge bg-green fg-white\' style=\'right: 10px;position: absolute;\'>' + property.feeValue + ' ' + this.hotel.defaultCurrency.name + ' </span>'
                        : '<span title=\'' + this.$i18n.t('labels.fee') + '\' class=\'badge bg-green fg-white\' style=\'right: 10px;position: absolute;\'>' + property.feeRangeMin + " - " + property.feeRangeMax + ' ' + this.hotel.defaultCurrency.name + ' </span>'
                        : '')
                    + "</div>";

                htmlContent += !lineSeparator ? "</div></li>" : "";
                lineSeparator = !lineSeparator;
            });
            console.log("content", htmlContent);

            this.infoBox = Metro.infobox.create(htmlContent + "</ul></div>", "", {
                closeButton: true,
                type: "info",
                removeOnClose: true,
                height: "auto"
            });
        },
        createValueInfoBox() {
            if (this.infoBox != null) { Metro.infobox.close(this.infoBox); }

            let htmlContent = "<div class='skill-box bg-transparent'><h5>" + this.$i18n.t('labels.servicesAndFacilitiesAvailable') + "</h5><ul class='skills'>";
            let lineSeparator = true;
            this.valueProperties.forEach((property) => {
                htmlContent += lineSeparator ? "<li><div class='d-flex w-100'>" : "";
                htmlContent += "<div class='d-flex w-50'><span>" + property.name + ": <span class='rounded-pill'>" + property.value + " " + property.unit + "</span>" + "</span>" +
                    ((property.fee) ? (property.feeValue != null) ?
                        '<span title=\'' + this.$i18n.t('labels.fee') + '\' class=\'badge bg-green fg-white\' style=\'right: 10px;position: absolute;\'>' + property.feeValue + ' ' + this.hotel.defaultCurrency.name + ' </span>'
                        : '<span title=\'' + this.$i18n.t('labels.fee') + '\' class=\'badge bg-green fg-white\' style=\'right: 10px;position: absolute;\'>' + property.feeRangeMin + " - " + property.feeRangeMax + ' ' + this.hotel.defaultCurrency.name + ' </span>'
                        : '')
                    + "</div>";

                htmlContent += !lineSeparator ? "</div></li>" : "";
                lineSeparator = !lineSeparator;
            });
            console.log("content", htmlContent);

            this.infoBox = Metro.infobox.create(htmlContent + "</ul></div>", "", {
                closeButton: true,
                type: "info",
                removeOnClose: true,
                height: "auto"
            });
        },
        hotelDetailsClick(event) {
            this.$store.state.backRoute = "/profile";
            this.$store.dispatch("setHotel", this.hotel);
            this.$router.push('/hotels/' + this.hotel.id);
        },
    },
};
</script>

<style scoped>
    #testOmega {
        opacity: 100% !important;
    }

    .btn.btn-primary {
        background-color: #53c16e;
        border-color: #1bc541;
    }

    a.nav-link {
        margin-left: 35mm;
    }

    .p-4 {
        margin-top: 15px;
        background-color: rgb(241, 241, 241);
    }
</style>