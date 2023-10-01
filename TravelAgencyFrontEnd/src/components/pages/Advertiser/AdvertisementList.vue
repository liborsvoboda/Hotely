<template>
    <div class="p-4 rounded drop-shadow" :style="(hotel.deactivated ? 'background-color: rgb(242 214 161 / 70%);' : '')">
        <div id="testOmega" style="opacity:1.0;">
            <div class="row">
                <div class="col-md-12 pt-0 mt-0">
                    <ul class="h-menu open large pos-static bg-cyan fg-cyan" style="background-color:transparent !important;">
                        <li :title="$t('labels.editLease')" @click="editAdvertisement(hotel.id)"><a href="#" class="p-2"><span class="mif-description icon mif-2"></span></a></li>
                        <li :title="$t('labels.requestOfApproval')" @click="setApprovalRequest(hotel.id)" :class="(hotel.approved ? 'disabled' : '')"><a href="#" class="p-2"><span class="mif-traff icon"></span></a></li>
                        <!--  <li :title="$t('labels.setTimeline')"><a href="#" class="p-2"><span class="mif-calendar icon"></span></a></li> -->
                        <li :title="$t('labels.actDeactivation')" @click="setActivation(hotel.id)"><a href="#" class="p-2"><span class="mif-traffic-cone icon"></span></a></li>
                        <li :title="$t('labels.deleteUnused')" @click="deleteAdvertisement(hotel.id)" :class="(hotel.hotelReservationLists.length > 0 ? 'disabled' : '')"><a href="#" class="p-2"><span class="mif-cancel icon"></span></a></li>
                        <li :title="$t('labels.comments')" @click="newCommentHotelId = hotel.id;generateMessageBox();" onclick="Metro.infobox.open('#CommentBox');">
                            <a href="#" class="p-2">
                                <span class="mif-comment icon"></span>
                                <span class="badge bg-orange fg-white mt-2" style="font-size: 10px;" :style="(getOpenedCommentsCount == 0 ? ' display: none ': ' display: inline ')">{{ getOpenedCommentsCount }}</span>
                                <span class="badge bg-brandColor1 fg-white mt-8" style="font-size: 10px;" :style="(hotel.guestAdvertiserNoteLists.length == 0 ? ' display: none ': ' display: inline ')">{{ hotel.guestAdvertiserNoteLists.length }}</span>
                            </a>
                        </li>
                        <!--  <li :title="$t('labels.showOnMap')"><a href="#" class="p-2"><span class="mif-my-location icon"></span></a></li> -->
                        <!-- <li :title="$t('labels.publish')"><a href="#" class="p-2"><span class="mif-feed icon"></span></a></li> -->
                        <li :title="$t('labels.reservation')" @click="openedReservation(hotel.id)" :class="(getUnshownDetailCount == 0 ? 'disabled' : '')">
                            <a href="#" class="p-2">
                                <span class="mif-shop icon"></span>
                                <span class="badge bg-orange fg-white mt-2" style="font-size: 10px;" :style="(getUnshownDetailCount == 0 ? ' display: none ': ' display: inline ')">{{ getUnshownDetailCount }}</span>
                                <span class="badge bg-brandColor1 fg-white mt-8" style="font-size: 10px;" :style="(hotel.hotelReservationLists.filter(obj => {return new Date(obj.startDate) > new Date();}).length == 0 ? ' display: none ': ' display: inline ')">{{ hotel.hotelReservationLists.filter(obj => {return new Date(obj.startDate) > new Date();}).length }}</span>
                            </a>
                        </li>
                    </ul>
                </div>
            </div>

            <div class="row">
                <div class="col-md-4">
                    <PhotoSlider :photos="photos" :width="'210px'" :height="'150px'" :key="hotel.id" class="drop-shadow" />
                </div>
                <div class="col-md-8">
                    <div class="row">
                        <div class="col-md-4 pt-0 mt-0 text-start">
                            <b class="c-help" :title="$t('labels.advertisementName')">{{ hotel.name }}</b>
                            <div>
                                {{ $t('labels.currency') }}:
                                <span class="rounded-pill ">
                                    {{ hotel.defaultCurrency.name }}
                                </span>
                            </div>
                            <div>{{ hotel.city.city }}, {{ hotel.country.systemName }}</div>
                            <div>
                                {{ $t('labels.totalCapacity') }}:
                                <span class="rounded-pill ">
                                    {{ hotel.totalCapacity }}
                                </span>
                            </div>

                            <div>
                                {{ $t('labels.imagesCount') }}:
                                <span class="rounded-pill ">
                                    {{ hotel.hotelImagesLists.length }}
                                </span>
                            </div>
                            <div>
                                {{ $t('labels.advertisementCount') }}:
                                <span class="rounded-pill ">
                                    {{ hotel.hotelRoomLists.length }}
                                </span>
                            </div>
                        </div>

                        <div class="col-md-4 pt-0 mt-0 text-start">
                            <h5 class="c-help ani-hover-heartbeat" style="margin-bottom:0px;" @click="createRoomListBox()">
                                <small>{{ $t('labels.roomPriceFrom') }}:</small> <b>{{ lowestPrice }} {{ hotel.defaultCurrency.name }}</b>
                            </h5>
                            <p class="c-help ani-hover-heartbeat" style="margin-bottom:0px;" @click="createValueInfoBox()">
                                {{ $t('labels.valueInformations') }}
                            </p>
                            <p class="c-help ani-hover-heartbeat" style="margin-bottom:0px;" @click="createBitInfoBox()">
                                {{ $t('labels.availableInformations') }}
                            </p>
                            <div class="c-help ani-hover-heartbeat">
                                {{ $t('labels.reservationCount') }}:
                                <span class="rounded-pill ">
                                    {{ hotel.hotelReservationLists.length }}
                                </span>
                            </div>
                            <div class="c-help ani-hover-heartbeat">
                                {{ $t('labels.popularCount') }}:
                                <span class="rounded-pill ">
                                    {{ hotel.popularCount }}
                                </span>
                            </div>
                            <div class="">
                                {{ $t('labels.searchCount') }}:
                                <span class="rounded-pill ">
                                    {{ hotel.shown }}
                                </span>
                            </div>
                        </div>

                        <div class="col-md-4 pt-0 mt-0 text-start">
                            <div>
                                {{ $t('labels.state') }}:
                                <span class="rounded-pill ">
                                    {{ ( hotel.approveRequest ? $t('labels.approvalRequest') : hotel.approved ? $t('labels.approved') : $t('labels.processed') ) }}
                                </span>
                            </div>

                            <div>
                                {{ $t('labels.status') }}:
                                <span class="rounded-pill ">
                                    {{ ( hotel.deactivated ? $t('labels.inactive') : $t('labels.active') ) }}
                                </span>
                            </div>

                            <div>
                                {{ $t('labels.advertisementStatus') }}:
                                <span class="rounded-pill ">
                                    {{ ( hotel.advertised ? $t('labels.advertised') : $t('labels.unadvertised') ) }}
                                </span>
                            </div>
                            <div>
                                {{ $t('labels.ratings') }}:
                                <span class="rounded-pill ">
                                    {{ hotel.averageRating }}
                                </span>
                            </div>
                            <div>
                                {{ $t('labels.topStatus') }}:
                                <span class="rounded-pill ">
                                    {{ (hotel.top ? $t('labels.enabled') : $t('labels.disabled') ) }}
                                </span>
                            </div>
                            <div>
                                {{ $t('labels.topShownCount') }}:
                                <span class="rounded-pill ">
                                    {{ hotel.topShown }}
                                </span>
                            </div>

                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div id="CommentBox" class="info-box" data-role="infobox" data-type="default" data-width="600" data-height="600">
            <span class="button square closer"></span>
            <div class="info-box-content" style="overflow-y:auto;">
                <h3>{{ $t('labels.advertisementNotepad') }}</h3>
                <form id="CommentForm" data-role="validator" action="javascript:" data-on-submit="newCommentIsValid = true;" data-interactive-check="true" autocomplete="off" data-on-error="newCommentIsValid = false;">
                    <div class="d-flex row ">
                        <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12">
                            <input id="CommentTitle" type="text" class="input" data-role="input" :placeholder="$t('labels.title')" data-validate="required" maxlength="50" />
                        </div>
                        <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12 text-right">
                            <div class="form-actions p-0 m-0 pl-2">
                                <button class="button success outline shadowed mr-2" type="submit" @click="setNewComment()" style="top: 0px; position: absolute; right: 70px;"> {{ $t('labels.saveNew') }} </button>
                                <button class="button alert outline shadowed" type="reset">Reset</button>
                            </div>
                        </div>
                    </div>

                    <textarea id="CommentNote" data-role="textarea" data-auto-size="true" data-max-height="150" maxlength="4000" class="primary" data-validate="required" :placeholder="$t('labels.writeComment')"></textarea>
                </form>
                <hr />
                <div id="MessageBox" class="d-block m-0 p-0" style="overflow-y: scroll;height:330px;max-height: 330px;"></div>
            </div>
        </div>


    </div>
</template>


<script>
    import PhotoSlider from "../HotelViewComponents/RoomsViewComponents/PhotoSlider.vue";
    import { ref, watch } from 'vue';

export default {
    components: {
        PhotoSlider,
    },
    data() {
        return {
            infoBox: null,
            newCommentHotelId : null,
        };
    },
    props: {
        hotel: {},
    },
    computed: {
        getOpenedCommentsCount() {
            return this.hotel.guestAdvertiserNoteLists.filter(obj => { return obj.solved == false; }).length;
        },
        getUnshownDetailCount() {
            let count = 0; let recNo = 0;
            this.hotel.hotelReservationLists.filter(obj => { return new Date(obj.startDate) > new Date(); }).forEach(reservation => {
                recNo++; let subrecNo = 0;
                reservation.hotelReservationDetailLists.forEach(detail => {
                    subrecNo++;
                    if (detail.shown == false) { count++; }
                    if (recNo == this.hotel.hotelReservationLists.filter(obj => { return new Date(obj.startDate) > new Date(); }).length && subrecNo == reservation.hotelReservationDetailLists.length) {
                        return count;
                    }
                });
            });
            return count;
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
            photos.push({ id: this.hotel.id, hotelPhoto: this.$store.state.apiRootUrl + '/Image/' + this.hotel.id + '/' + this.hotel.hotelImagesLists.filter(obj => { return obj.isPrimary === true; })[0].fileName });

            this.hotel.hotelImagesLists.forEach(image => {
                if (!image.isPrimary) { photos.push({ id: this.hotel.id, hotelPhoto: this.$store.state.apiRootUrl + '/Image/' + this.hotel.id + '/' + image.fileName }) }
            });
            this.hotel.hotelRoomLists.forEach(room => {
                photos.push({ id: this.hotel.id, hotelPhoto: this.$store.state.apiRootUrl + '/RoomImage/' + room.id })
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
    mounted() {


    },
    methods: {
        async generateMessageBox() {
            if (this.hotel.id == this.newCommentHotelId) {
                let messageData = "";
                this.hotel.guestAdvertiserNoteLists.forEach(message => {
                    messageData += "<div class=\"card image - header\"><div class=\"d-block card-content p-0 " + (!message.solved ? " bg-brandColor1 " : " bg-lightOlive ") + "\"><div class=\"d-flex\"><div class=\"h5 fg-black col-xl-8 col-lg-8 col-md-8 col-sm-8 col-12 p-1 m-0\">" + message.title + "</div><div class=\"h8 fg-black col-xl-4 col-lg-4 col-md-4 col-sm-4 col-12 text-right p-1 m-0\">" + new Date(message.timeStamp).toLocaleString('cs-CZ') + "</div></div>"; 
                    messageData += "<div class=\"d-flex\"><div class=\"fg-white col-xl-10 col-lg-10 col-md-10 col-sm-10 col-12 p-1\">" + message.note + "</div>";
                    messageData += "<div class=\"col-xl-2 col-lg-2 col-md-2 col-sm-2 col-12 p-1 text-right\"><div class=\"d-block \">";
                    messageData += "<div class=\"mb-1 p-button p-component button shadowed small info " + (!message.solved ? " disabled " : "") + " \" onclick=\"setCommentStatus('" + message.id + "','" + this.$store.state.apiRootUrl + "');\">" + window.dictionary("labels.setOpened") + "</div>";
                    messageData += "<div class=\"mb-1 p-button p-component button shadowed small success " + (message.solved ? " disabled " : "") + " \" onclick=\"setCommentStatus('" + message.id + "','" + this.$store.state.apiRootUrl + "');\">" + window.dictionary("labels.setSolved") + "</div>";
                    messageData += "<div class=\"p-button p-component button shadowed small alert \" onclick=\"deleteComment('" + message.id + "','" + this.$store.state.apiRootUrl + "');\">" + window.dictionary("labels.delete") + "</div>";
                    messageData += "</div></div></div></div></div>";
                });
                $("#MessageBox").html(messageData);
            }
        },
        async setNewComment() {
            $("#CommentForm").submit();

            var that = this;
            setTimeout(async function () {
                if (newCommentIsValid) {
                    window.showPageLoading();
                    let response = await fetch(
                        that.$store.state.apiRootUrl + '/Guest/SetGuestComment', {
                        method: 'POST', headers: { 'Authorization': 'Bearer ' + that.$store.state.user.Token, 'Content-type': 'application/json' },
                        body: JSON.stringify({ Title: $("#CommentTitle").val(), Note: $("#CommentNote").val(), HotelId: that.newCommentHotelId, Language: that.$store.state.language })
                    }); let result = await response.json();

                    if (result.Status == "error") {
                        var notify = Metro.notify; notify.setup({ width: 300, timeout: that.$store.state.userSettings.notifyShowTime, duration: 500 });
                        notify.create(result.ErrorMessage, "Error", { cls: "alert" }); notify.reset();
                    } else {
                        if (document.getElementById("CommentForm") != null) { document.getElementById("CommentForm").reset(); }
                        let result = await that.$store.dispatch("getAdvertisementList");
                        that.generateMessageBox();
                    }
                    window.hidePageLoading();
                }
            }, 1000);
        
        },
        openedReservation(hotelId) {
            this.$router.push('/profile/advertisementWizard');
        },
        editAdvertisement(hotelId) {
            ActualValidationFormName = "hotelForm";
            ActualWizardPage = 1;
            propertyList = [];
            Router = this.$router;
            ApiRootUrl = null;
            HotelRecId = null;
            WizardImageGallery = [];
            WizardRooms = [];
            WizardTempRoomPhoto = [];
            WizardProperties = [];
            WizardSelectedProperty = {};
            WizardHotel = {
                HotelId: this.hotel.id, HotelName: this.hotel.name, HotelCurrency: this.hotel.defaultCurrencyId,
                HotelCountry: this.hotel.countryId, HotelCity: this.hotel.cityId, Description: this.hotel.descriptionCz,
                Images: this.hotel.hotelImagesLists, 
                Rooms: this.hotel.hotelRoomLists,
                Properties: this.hotel.hotelPropertyAndServiceLists
            };
            this.$router.push('/profile/advertisementWizard');
        },
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

            this.infoBox = Metro.infobox.create(htmlContent, "", {
                closeButton: true,
                type: "info",
                removeOnClose: true,
                height: "auto"
            });
        },
        async setApprovalRequest(hotelId) {
            disableScroll();
            var response = await fetch(
                this.$store.state.apiRootUrl + '/Advertiser/ApprovalRequest/' + hotelId, {
                method: 'GET', headers: { 'Authorization': 'Bearer ' + this.$store.state.user.Token, 'Content-type': 'application/json' }
            }
            ); let result = await response.json();
            if (this.$store.state.user.UserId != '') {
                await this.$store.dispatch("getAdvertisementList");
                if (!this.$store.state.advertisementList.length) { this.errorText = true; }
            }
            enableScroll();
        },
        async deleteAdvertisement(hotelId) {
            window.showPartPageLoading();
            disableScroll();
            var response = await fetch(
                this.$store.state.apiRootUrl + '/Advertiser/DeleteAdvertisement/' + hotelId, {
                method: 'GET', headers: { 'Authorization': 'Bearer ' + this.$store.state.user.Token, 'Content-type': 'application/json' }
            }
            ); let result = await response.json();
            if (result.Status == "error") {
                var notify = Metro.notify; notify.setup({ width: 300, timeout: this.$store.state.userSettings.notifyShowTime, duration: 500 });
                notify.create(window.dictionary('labels.cannotDeleteBecauseIsUsed'), "Info"); notify.reset();
                window.hidePartPageLoading();
            } else {
                if (this.$store.state.user.UserId != '') {
                    await this.$store.dispatch("getAdvertisementList");
                    if (!this.$store.state.advertisementList.length) { this.errorText = true; }
                }
                window.hidePartPageLoading();
            }
            enableScroll();
        },
        async setActivation(hotelId) {
            window.showPartPageLoading();
            disableScroll();
            var response = await fetch(
                this.$store.state.apiRootUrl + '/Advertiser/ActivationHotel/' + hotelId, {
                method: 'GET', headers: { 'Authorization': 'Bearer ' + this.$store.state.user.Token, 'Content-type': 'application/json' }
            }
            ); let result = await response.json();
            if (this.$store.state.user.UserId != '') {
                await this.$store.dispatch("getAdvertisementList");
                if (!this.$store.state.advertisementList.length) { this.errorText = true; }
            }
            window.hidePartPageLoading();
            enableScroll();
        },
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

            this.infoBox = Metro.infobox.create(htmlContent + "</ul></div>", "", {
                closeButton: true,
                type: "info",
                removeOnClose: true,
                height: "auto"
            });
        },
        hotelDetailsClick(event) {
            this.$store.state.backRoute = "/profile/advertisement";
            this.$store.state.backRouteScroll = window.scrollY;

            this.$store.dispatch("setHotel", this.hotel);
            this.$router.push('/hotels/' + this.hotel.id);
        },
    },
    created() {
        $('[scroll="disable"]').on('click', function () {
            var oldWidth = $body.innerWidth();
            scrollPosition = window.pageYOffset;
            $body.css('overflow', 'hidden');
            $body.css('position', 'fixed');
            $body.css('top', `-${scrollPosition}px`);
            $body.width(oldWidth);
        });

        watch(window.watchAdvertisementVariables, async () => {
            if (window.watchAdvertisementVariables.reloadAdvertisement) {
                window.watchAdvertisementVariables.reloadAdvertisement = false;
                let result = await this.$store.dispatch("getAdvertisementList");
                this.generateMessageBox();
            }
        });
    }
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