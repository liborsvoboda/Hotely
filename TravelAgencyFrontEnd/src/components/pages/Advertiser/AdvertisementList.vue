<template>
    <div class="p-4 rounded drop-shadow" :style="getAdvertisementStyle" >
        <div id="testOmega" style="opacity:1.0;">
            <div class="row">
                <div class="col-md-12 pt-0 mt-0">
                    <ul class="h-menu open large pos-static bg-cyan fg-cyan" style="background-color:transparent !important;">
                        <li :title="$t('labels.editLease')" @click="editAdvertisement(hotel.id)"><a href="#" class="p-2"><span class="mif-description icon mif-2"></span></a></li>
                        <li :title="$t('labels.requestOfApproval')" @click="setApprovalRequest(hotel.id)" :class="(hotel.approved ? 'disabled' : '')"><a href="#" class="p-2"><span class="mif-traff icon"></span></a></li>
                        <li :title="$t('labels.actDeactivation')" @click="setActivation(hotel.id)"><a href="#" class="p-2"><span class="mif-traffic-cone icon"></span></a></li>
                        <li :title="$t('labels.deleteUnused')" @click="deleteAdvertisement(hotel.id)" :class="(hotel.hotelReservationLists.length > 0 ? 'disabled' : '')"><a href="#" class="p-2"><span class="mif-cancel icon"></span></a></li>
                        <li :title="$t('labels.tasks')" @click="newCommentHotelId = hotel.id;generateMessageBox();" onclick="Metro.infobox.open('#CommentBox');">
                            <a href="#" class="p-2">
                                <span class="mif-clipboard icon"></span>
                                <span class="badge bg-orange fg-white mt-2" style="font-size: 10px;" :style="(getOpenedCommentsCount == 0 ? ' display: none ': ' display: inline ')">{{ getOpenedCommentsCount }}</span>
                                <span class="badge bg-brandColor1 fg-white mt-8" style="font-size: 10px;" :style="(hotel.guestAdvertiserNoteLists.length == 0 ? ' display: none ': ' display: inline ')">{{ hotel.guestAdvertiserNoteLists.length }}</span>
                            </a>
                        </li>
                        <!--  <li :title="$t('labels.showOnMap')"><a href="#" class="p-2"><span class="mif-my-location icon"></span></a></li> -->
                        <!-- <li :title="$t('labels.publish')"><a href="#" class="p-2"><span class="mif-feed icon"></span></a></li> -->
                        <li :title="$t('labels.reservation')" @click="openReservation();" onclick="Metro.infobox.open('#ReservationBox');" :class="(hotel.hotelReservationLists.filter(obj => {return new Date(obj.endDate) >= new Date();}).length == 0 ? 'disabled' : '')">
                            <a href="#" class="p-2">
                                <span class="mif-shop icon"></span>
                                <span class="badge bg-orange fg-white mt-2" style="font-size: 10px;" :style="(getUnshownDetailCount == 0 ? ' display: none ': ' display: inline ')">{{ getUnshownDetailCount }}</span>
                                <span class="badge bg-brandColor1 fg-white mt-8" style="font-size: 10px;" :style="(hotel.hotelReservationLists.filter(obj => {return new Date(obj.endDate) >= new Date();}).length == 0 ? ' display: none ': ' display: inline ')">{{ hotel.hotelReservationLists.filter(obj => {return new Date(obj.endDate) >= new Date();}).length }}</span>
                            </a>
                        </li>
                        <li :title="$t('labels.reservationsHistoryOverview')" @click="openHistory();" onclick="Metro.infobox.open('#HistoryBox');" :class="(hotel.hotelReservationLists.filter(obj => {return new Date(obj.endDate) < new Date();}).length == 0 ? 'disabled' : '')">
                            <a href="#" class="p-2">
                                <span class="mif-history icon"></span>
                                <span class="badge bg-brandColor1 fg-white mt-8" style="font-size: 10px;" :style="(hotel.hotelReservationLists.filter(obj => {return new Date(obj.endDate) < new Date();}).length == 0 ? ' display: none ': ' display: inline ')">{{ hotel.hotelReservationLists.filter(obj => {return new Date(obj.endDate) < new Date();}).length }}</span>
                            </a>
                        </li>
                        <li :title="$t('labels.unavailableSettings')" @click="openUnavailableSettings()" onclick="Metro.infobox.open('#UnavailableSettingsBox');"><a href="#" class="p-2"><span class="mif-blocked icon"></span></a></li>
                        <li :title="$t('labels.showBookedCalendar')" @click="openCalendar()" onclick="Metro.infobox.open('#CalendarBox');"><a href="#" class="p-2"><span class="mif-calendar icon"></span></a></li>
                        <li :title="$t('labels.reviews')" @click="openReview()" onclick="Metro.infobox.open('#ReviewBox');" :class="(hotel.hotelReservationReviewLists.length == 0 ? 'disabled' : '')">
                            <a href="#" class="p-2">
                                <span class="mif-blogger icon"></span>
                                <span class="badge bg-orange fg-white mt-2" style="font-size: 10px;" :style="(getUnshownReviewCount == 0 ? ' display: none ': ' display: inline ')">{{ getUnshownReviewCount }}</span>
                                <span class="badge bg-brandColor1 fg-white mt-8" style="font-size: 10px;" :style="(hotel.hotelReservationReviewLists.length == 0 ? ' display: none ': ' display: inline ')">{{ hotel.hotelReservationReviewLists.length }}</span>
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
                                <small class="fg-black">{{ $t('labels.roomPriceFrom') }}:</small> <b>{{ lowestPrice }} {{ hotel.defaultCurrency.name }}</b>
                            </h5>
                            <p class="c-help ani-hover-heartbeat" style="margin-bottom:0px;" @click="createPriceInfoBox()">
                                {{ $t('labels.servicesAndFacilitiesAvailable') }}
                            </p>
<!--                             <p class="c-help ani-hover-heartbeat" style="margin-bottom:0px;" @click="createBitInfoBox()">
                                {{ $t('labels.availableInformations') }}
                            </p> -->
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
                                    {{ ( hotel.approveRequest != null && !hotel.approveRequest && !hotel.approved  ? $t('labels.denied') : hotel.approveRequest ? $t('labels.approvalRequest') : hotel.approved ? $t('labels.approved') : $t('labels.processed') ) }}
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

        <div id="ReservationBox" class="info-box" data-role="infobox" data-type="default" data-width="800" data-height="600">
            <span class="button square closer"></span>
            <div class="info-box-content" style="overflow-y:auto;">
                <div class="d-flex row ">
                    <div class="h3 col-xl-5 col-lg-5 col-md-5 col-sm-5 col-12">
                        {{ $t('labels.reservationsOverview') }}
                    </div>
                    <div class="col-xl-7 col-lg-7 col-md-7 col-sm-7 col-12 text-right pr-5">
                        <select id="reservations" data-role="select" data-add-empty-value="true" data-clear-button="false" @change='changeSelectedReservation()'></select>
                    </div>
                </div>
                <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12">
                    <ul data-role="tabs" data-expand="true">
                        <li id="reservationInfoMenu" class="fg-black"><a href="#_reservationInfo">{{ $t('labels.reservationInfo') }}</a></li>
                        <li id="reservationRoomsMenu" class="fg-black"><a href="#_reservationRooms">{{ $t('labels.reservationRooms') }}</a></li>
                        <li id="reservationMessagesMenu" class="fg-black"><a href="#_reservationMessages">{{ $t('labels.reservationMessages') }}</a></li>
                        <li id="reservationNewMessageMenu" class="fg-black"><a href="#_reservationNewMessage">{{ $t('labels.reservationNewMessage') }}</a></li>
                    </ul>
                </div>
                <div id="_reservationInfo">
                    <div class="d-flex row ">
                        <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12">
                            <h5>{{ (reservationId > 0 ? getSelectedReservationByReservationId.reservationNumber : "")}}</h5>
                        </div>
                        <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12 text-right">
                            {{ (reservationId > 0 ? new Date(getSelectedReservationByReservationId.startDate).toLocaleDateString('cs-CZ') : "" ) + " - " + (reservationId > 0 ? new Date(getSelectedReservationByReservationId.endDate).toLocaleDateString('cs-CZ') : "") }}
                        </div>

                        <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12">
                            {{ (reservationId > 0 ? getSelectedReservationByReservationId.firstName : "") + " " + (reservationId > 0 ? getSelectedReservationByReservationId.lastName : "") }}
                        </div>
                        <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12 text-right">{{ $t('labels.email') }}: {{ (reservationId > 0 ? getSelectedReservationByReservationId.email : "") }} </div>
                        <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12"> {{ (reservationId > 0 ? getSelectedReservationByReservationId.street : "") }} </div>
                        <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12 text-right">{{ $t('labels.phone') }}:  {{ (reservationId > 0 ? getSelectedReservationByReservationId.phone : "") }} </div>

                        <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12"> {{ (reservationId > 0 ? getSelectedReservationByReservationId.city : "") }} {{ (reservationId > 0 ? getSelectedReservationByReservationId.zipcode : "") }}</div>
                        <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12 text-right">{{ $t('labels.adults') }}/{{ $t('labels.children') }}:  {{ (reservationId > 0 ? getSelectedReservationByReservationId.adult : "") }} / {{ (reservationId > 0 ? getSelectedReservationByReservationId.children : "") }} </div>

                        <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12"> {{ (reservationId > 0 ? getSelectedReservationByReservationId.country : "") }}</div>
                        <div class="h5 col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12 text-right">{{ $t('labels.totalPrice') }}: {{ (reservationId > 0 ? getSelectedReservationByReservationId.totalPrice : "") }} {{ hotel.defaultCurrency.name }} </div>

                    </div>
                </div>

                <div id="_reservationRooms">
                    <div id="ReservationRoomsBox" class="d-block m-0 p-0" style="overflow-y: scroll;height:460px;max-height: 460px;"></div>
                </div>
                <div id="_reservationMessages">
                    <div id="ReservationMessageBox" class="d-block m-0 p-0" style="overflow-y: scroll;height:460px;max-height: 460px;"></div>
                </div>
                <div id="_reservationNewMessage">
                    <div class="d-block m-0 p-0">
                        <form id="ReservationForm" data-role="validator" action="javascript:" data-on-submit="newMessageIsValid = true;" data-interactive-check="true" autocomplete="off" data-on-error="newMessageIsValid = false;">
                            <div class="d-flex row ">
                                <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12">
                                    <select id="hotelStatus" data-role="select" data-clear-button="false"></select>
                                </div>

                                <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12">
                                    <textarea id="ReservationNote" data-role="textarea" data-auto-size="true" data-max-height="150" maxlength="4000" class="primary" data-validate="required" :placeholder="$t('labels.writeMessage')"></textarea>
                                </div>
                                <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12 text-right">
                                    <button class="button success outline shadowed mr-2" type="submit" @click="setNewReservationDetail()" :disabled="reservationId == null"> {{ $t('labels.saveNew') }} </button>
                                </div>
                            </div>
                        </form>
                    </div>
                </div>
            </div>
        </div>

        <div id="HistoryBox" class="info-box" data-role="infobox" data-type="default" data-width="800" data-height="600">
            <span class="button square closer"></span>
            <div class="info-box-content" style="overflow-y:auto;">
                <div class="d-flex row ">
                    <div class="h3 col-xl-5 col-lg-5 col-md-5 col-sm-5 col-12">
                        {{ $t('labels.reservationsHistoryOverview') }}
                    </div>
                    <div class="col-xl-7 col-lg-7 col-md-7 col-sm-7 col-12 text-right pr-5">
                        <select id="history" data-role="select" data-add-empty-value="true" data-clear-button="false" @change='changeSelectedHistory()'></select>
                    </div>
                </div>
                <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12">
                    <ul data-role="tabs" data-expand="true">
                        <li class="fg-black"><a href="#_historyInfo">{{ $t('labels.reservationInfo') }}</a></li>
                        <li class="fg-black"><a href="#_historyRooms">{{ $t('labels.reservationRooms') }}</a></li>
                        <li class="fg-black"><a href="#_historyMessages">{{ $t('labels.reservationMessages') }}</a></li>
                    </ul>
                </div>
                <div id="_historyInfo">
                    <div class="d-flex row ">
                        <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12">
                            <h5>{{ (historyId > 0 ? hotel.hotelReservationLists.filter(obj => {return obj.id == historyId;})[0].reservationNumber : "")}}</h5>
                        </div>
                        <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12 text-right">
                            {{ (historyId > 0 ? new Date(hotel.hotelReservationLists.filter(obj => {return obj.id == historyId;})[0].startDate).toLocaleDateString('cs-CZ') : "" ) + " - " + (historyId > 0 ? new Date(hotel.hotelReservationLists.filter(obj => {return obj.id == historyId;})[0].endDate).toLocaleDateString('cs-CZ') : "") }}
                        </div>

                        <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12">
                            {{ (historyId > 0 ? hotel.hotelReservationLists.filter(obj => {return obj.id == historyId;})[0].firstName : "") + " " + (historyId > 0 ? hotel.hotelReservationLists.filter(obj => {return obj.id == historyId;})[0].lastName : "") }}
                        </div>
                        <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12 text-right">{{ $t('labels.email') }}: {{ (historyId > 0 ? hotel.hotelReservationLists.filter(obj => {return obj.id == historyId;})[0].email : "") }} </div>
                        <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12"> {{ (historyId > 0 ? hotel.hotelReservationLists.filter(obj => {return obj.id == historyId;})[0].street : "") }} </div>
                        <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12 text-right">{{ $t('labels.phone') }}:  {{ (historyId > 0 ? hotel.hotelReservationLists.filter(obj => {return obj.id == historyId;})[0].phone : "") }} </div>

                        <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12"> {{ (historyId > 0 ? hotel.hotelReservationLists.filter(obj => {return obj.id == historyId;})[0].city : "") }} {{ (historyId > 0 ? hotel.hotelReservationLists.filter(obj => {return obj.id == historyId;})[0].zipcode : "") }}</div>
                        <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12 text-right">{{ $t('labels.adults') }}/{{ $t('labels.children') }}:  {{ (historyId > 0 ? hotel.hotelReservationLists.filter(obj => {return obj.id == historyId;})[0].adult : "") }} / {{ (historyId > 0 ? hotel.hotelReservationLists.filter(obj => {return obj.id == historyId;})[0].children : "") }} </div>

                        <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12"> {{ (historyId > 0 ? hotel.hotelReservationLists.filter(obj => {return obj.id == historyId;})[0].country : "") }}</div>
                        <div class="h5 col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12 text-right">{{ $t('labels.totalPrice') }}: {{ (historyId > 0 ? hotel.hotelReservationLists.filter(obj => {return obj.id == historyId;})[0].totalPrice : "") }} {{ hotel.defaultCurrency.name }} </div>

                    </div>
                </div>

                <div id="_historyRooms">
                    <div id="HistoryRoomsBox" class="d-block m-0 p-0" style="overflow-y: scroll;height:460px;max-height: 460px;"></div>
                </div>
                <div id="_historyMessages">
                    <div id="HistoryMessageBox" class="d-block m-0 p-0" style="overflow-y: scroll;height:460px;max-height: 460px;"></div>
                </div>
            </div>
        </div>

        <div id="CalendarBox" class="info-box" data-role="infobox" data-type="default" data-width="800" data-height="600">
            <span class="button square closer"></span>
            <div class="info-box-content" style="overflow-y:auto;">
                <div class="d-flex row ">
                    <div class="h3 col-xl-5 col-lg-5 col-md-5 col-sm-5 col-12">
                        {{ $t('labels.availableCalendar') }}
                    </div>
                    <div class="col-xl-7 col-lg-7 col-md-7 col-sm-7 col-12 text-right pr-5">
                        <select id="roomSelection" data-role="select" data-add-empty-value="true" data-clear-button="false" @change='changeSelectedRoom()'></select>
                    </div>
                </div>
                <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12">
                    <ul data-role="tabs" data-expand="true">
                        <li class="fg-black"><a href="#_calendarRoom">{{ $t('labels.selectedRoom') }}</a></li>
                        <li class="fg-black"><a href="#_calendar">{{ $t('labels.availableCalendar') }}</a></li>
                        <li class="fg-black"><a href="#_calendarDataTable">{{ $t('labels.calendarDataTable') }}</a></li>
                        <li class="fg-black"><a href="#_bookingData">{{ $t('labels.bookingData') }}</a></li>
                    </ul>
                </div>

                <div id="_calendarRoom">
                    <div id="CalendarRoomBox" class="d-block m-0 p-0" style="overflow-y: scroll;height:460px;max-height: 460px;"></div>
                </div>
                <div id="_calendar">
                    <div class="d-flex row ">
                        <div id="BookedRoomCalendar" data-role="calendar" data-wide-point="sm" data-week-start="1" data-input-format="yyyy-mm-dd" data-buttons="today" data-multi-select="true"></div>
                    </div>
                </div>
                <div id="_calendarDataTable">
                    <div id="CalendarDataTableBox" class="d-block m-0 p-0" style="overflow-y: scroll;height:460px;max-height: 460px;">
                        <table id="CalendarDataTable" class="table striped table-border mt-4" data-role="table" data-cls-component="mt-10" data-show-activity="true" data-rows="30" data-pagination="true" data-show-all-pages="false">
                            <thead>
                                <tr>
                                    <th data-sortable="true">{{ $t('labels.date') }}</th>
                                    <th data-sortable="true">{{ $t('labels.totalRoomsCount') }}</th>
                                    <!-- <th data-sortable="true">Poptáno</th> -->
                                    <th data-sortable="true">{{ $t('labels.booked') }}</th>
                                </tr>
                            </thead>
                            <tbody></tbody>
                        </table>
                    </div>
                </div>
                <div id="_bookingData">
                    <div id="BookingDataBox" class="d-block m-0 p-0" style="overflow-y: scroll;height:460px;max-height: 460px;"></div>
                </div>

            </div>
        </div>

        <div id="ReviewBox" class="info-box" data-role="infobox" data-type="default" data-width="800" data-height="600">
            <span class="button square closer"></span>
            <div class="info-box-content" style="overflow-y:auto;">
                <div class="d-flex row ">
                    <div class="h3 col-xl-5 col-lg-5 col-md-5 col-sm-5 col-12">
                        {{ $t('labels.reviewOverview') }}
                    </div>
                    <div class="col-xl-7 col-lg-7 col-md-7 col-sm-7 col-12 text-right pr-5">
                        <select id="reviews" data-role="select" data-add-empty-value="true" data-clear-button="false" @change='changeSelectedReview()'></select>
                    </div>
                </div>
                <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12">
                    <ul data-role="tabs" data-expand="true">
                        <li class="fg-black"><a href="#_reviewAllMessages">{{ $t('labels.allReviews') }}</a></li>
                        <li class="fg-black"><a href="#_reviewInfo">{{ $t('labels.selectedReview') }}</a></li>
                        <li class="fg-black"><a href="#_reviewNewAnswer">{{ $t('labels.insertAnswer') }}</a></li>
                    </ul>
                </div>
                <div id="_reviewAllMessages">
                    <div id="reviewAllMessagesBox" class="d-block m-0 p-0" style="overflow-y: scroll;height:460px;max-height: 460px;"></div>
                </div>
                <div id="_reviewInfo">
                    <div id="ReviewInfoBox" class="d-block m-0 p-0" style="overflow-y: scroll;height:460px;max-height: 460px;"></div>
                </div>
                <div id="_reviewNewAnswer">
                    <div class="d-block m-0 p-0">
                        <form id="ReviewForm" data-role="validator" action="javascript:" data-on-submit="newReviewIsValid = true;" data-interactive-check="true" autocomplete="off" data-on-error="newReviewIsValid = false;">
                            <div class="d-flex row ">
                                <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12">
                                    <textarea id="ReviewNote" data-role="textarea" data-auto-size="true" data-max-height="150" maxlength="2048" class="primary" data-validate="required" :placeholder="$t('labels.writeAnswer')"></textarea>
                                </div>
                                <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12 text-right">
                                    <button class="button success outline shadowed mr-2" type="submit" @click="setNewReviewAnswer()" :disabled="reviewId == null"> {{ $t('labels.saveAnswer') }} </button>
                                </div>
                            </div>
                        </form>
                    </div>
                </div>
            </div>
        </div>

        <div id="UnavailableSettingsBox" class="info-box" data-role="infobox" data-type="default" data-width="800" data-height="600">
            <span class="button square closer"></span>
            <div class="info-box-content" style="overflow-y:auto;">
                <div class="d-flex row ">
                    <div class="h3 col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12">
                        {{ $t('labels.unavailableSettings') }}
                    </div>
                </div>
                <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12">
                    <ul data-role="tabs" data-expand="true">
                        <li class="fg-black"><a href="#_setUnavailable">{{ $t('labels.setUnavailable') }}</a></li>
                        <li class="fg-black"><a href="#_unavailableRoomList">{{ $t('labels.unavailableRoomList') }}</a></li>
                    </ul>
                </div>

                <div id="_setUnavailable">
                    <div class="d-block m-0 p-0" style="overflow-y: scroll;height:460px;max-height: 460px;">
                        <form id="UnavailableForm" data-role="validator" action="javascript:" data-on-submit="newUnavailableIsValid = true;" data-interactive-check="true" autocomplete="off" data-on-error="newUnavailableIsValid = false;">
                            <div class="row ">
                                <div class="w-50 text-left"> {{ $t('labels.unavailableFromDate') }} </div>
                                <div class="w-50"> <input id="UnavailableFromDate" type="text" data-role="calendarpicker" data-cls-calendar="compact" data-validate="required" data-format="DD.MM.YYYY" style="height: auto"> </div>

                                <div class="w-50 text-left"> {{ $t('labels.unavailableToDate') }} </div>
                                <div class="w-50"> <input id="UnavailableToDate" type="text" data-role="calendarpicker" data-cls-calendar="compact" data-validate="required" data-format="DD.MM.YYYY" style="height: auto"> </div>

                                <div class="w-100 text-left"> {{ $t('labels.forEquipmentForRent') }} </div>
                                <div class="w-100">
                                    <select id="RoomSelectionUnv" data-role="select" data-clear-button="true" multiple data-validate="required" data-cls-selected-item="bg-teal fg-white"
                                            data-cls-selected-item-remover="bg-darkTeal fg-white"></select>
                                </div>
                                <p></p>
                                <div class="w-100 text-right">
                                    <button class="button success outline shadowed mr-2" type="submit" @click="setNewUnavailable()"> {{ $t('labels.saveUnavailable') }} </button>
                                </div>
                            </div>
                        </form>
                    </div>
                </div>
                <div id="_unavailableRoomList">
                    <div id="UnavailableRoomListBox" class="d-block m-0 p-0" style="overflow-y: scroll;height:460px;max-height: 460px;">
                    </div>
                </div>
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
            newCommentHotelId: null,
            reservationId: null,
            historyId: null,
            reviewId: null,
            roomId: null
        };
    },
    props: {
        hotel: {},
    },
    computed: {
        getAdvertisementStyle() {
            let backgroundStyle = "";
           /*deactivated*/ if (this.hotel.deactivated) { backgroundStyle += "background-color: #c1c5c9;"; backgroundStyle += "opacity: 0.7;"; } 
           /*rozpracovano*/ else if (!this.hotel.deactivated && this.hotel.approveRequest == null) { backgroundStyle += "background-color: #9ecfff;"; backgroundStyle += "opacity: 0.7;"; }
           /*ceka na schvaleni*/ else if (!this.hotel.deactivated && this.hotel.approveRequest && !this.hotel.approved) { backgroundStyle += "background-color: #f1ce22;"; backgroundStyle += "opacity: 0.7;"; }
           /*zamitnuto*/ else if (!this.hotel.deactivated && this.hotel.approveRequest != null && !this.hotel.approveRequest && !this.hotel.approved) { backgroundStyle += "background-color: #a31212cf;"; backgroundStyle += "opacity: 0.7;"; }
           /*schvaleno*/ else if (!this.hotel.deactivated && this.hotel.approveRequest != null && !this.hotel.approveRequest && this.hotel.approved) { backgroundStyle += "background-color: #098b01cf;"; backgroundStyle += "opacity: 0.7;"; }
            return backgroundStyle;
        },
        getSelectedReservationByReservationId() {
            if (this.reservationId > 0) {
                return this.hotel.hotelReservationLists.filter(obj => { return obj.id == this.reservationId; })[0];
            } else return "";
        },
        getOpenedCommentsCount() {
            return this.hotel.guestAdvertiserNoteLists.filter(obj => { return obj.solved == false; }).length;
        },
        getUnshownDetailCount() {
            let count = 0; let recNo = 0;
            this.hotel.hotelReservationLists.filter(obj => { return new Date(obj.endDate) >= new Date(); }).forEach(reservation => {
                recNo++; let subrecNo = 0;
                reservation.hotelReservationDetailLists.forEach(detail => {
                    subrecNo++;
                    if (detail.shown == false && detail.guestSender) { count++; }
                    if (recNo == this.hotel.hotelReservationLists.filter(obj => { return new Date(obj.endDate) >= new Date(); }).length && subrecNo == reservation.hotelReservationDetailLists.length) {
                        return count;
                    }
                });
            });
            return count;
        },
        getUnshownReviewCount() {
            return this.hotel.hotelReservationReviewLists.filter(obj => { return obj.advertiserShown == false; }).length;
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
        getUsedPropertyGroups() {
            let usedGroups = []; let lastGroup = null;
            this.getPriceListProperties.forEach(property => {
                if (lastGroup == null || (lastGroup != null && property.group != lastGroup)) { usedGroups.push({ name: property.group }); }
                lastGroup = property.group;
            });
            return usedGroups;
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
        }
    },
    mounted() {


    },
    methods: {
        async changeSelectedRoom() {
            if ($("#roomSelection")[0] != undefined && $("#roomSelection")[0].selectedOptions[0] != undefined && $("#roomSelection")[0].selectedOptions[0].value != "") {
                this.roomId = $("#roomSelection")[0].selectedOptions[0].value;

                //fill room info Card
                let messageData = ""; let availableCount = 0;
                this.hotel.hotelRoomLists.forEach(room => {
                    if (this.roomId == room.id) { availableCount = room.roomsCount;

                        messageData = "<ul class='feed-list'>";
                        messageData += "<li><img class='avatar' src='" + this.$store.state.apiRootUrl + "/RoomImage/" + room.id + "' >";
                        messageData += "<span class='label'>" + room.name + "</span>";
                        messageData += "<span class='second-label fg-black bold' style='font-size: 14px;'>" + room.roomsCount + "x" + (!room.extraBed ? "" : "+ <span class='mif-hotel mif-2x fg-cyan' style='top:3px;' data-role='hint' data-cls-hint='bg-cyan fg-white drop-shadow' : data-hint-text='" + window.dictionary('labels.extraBed') + "'></span>") + "</span>";
                        messageData += "<span class='second-label fg-black bold''>" + room.price + " " + this.hotel.defaultCurrency.name + "</span>";
                        messageData += "</li>";
                        messageData += "</ul>";
                        messageData += "<div>" + room.descriptionCz + "</div>";
                        $("#CalendarRoomBox").html(messageData);
                    }
                });

                //prepare calendar Array
                await this.$store.dispatch("getRoomBookingList", this.roomId).then(() => {
                    let bookingCalendar = [];
                    this.$store.state.roomBookingList.forEach(booking => {
                        if (booking.statusId == 2 || booking.statusId == 5) {
                            let startDate = new Date(booking.startDate);
                            while (new Date(startDate).toLocaleDateString('sv-SE') <= new Date(booking.endDate).toLocaleDateString('sv-SE')) {
                                if (bookingCalendar.filter(obj => { return obj.date == new Date(startDate).toLocaleDateString('sv-SE') }).length > 0) {
                                    bookingCalendar.forEach(book => {
                                        if (book.date == new Date(startDate).toLocaleDateString('sv-SE')) { book.booked = book.booked + booking.count; }
                                    });
                                }
                                if (bookingCalendar.filter(obj => { return obj.date == new Date(startDate).toLocaleDateString('sv-SE') }).length == 0) {
                                    bookingCalendar.push({ date: new Date(startDate).toLocaleDateString('sv-SE'), availableRooms: availableCount, booked: booking.count });
                                }
                                startDate = new Date(startDate.getFullYear(), startDate.getMonth(), startDate.getDate() + 1);
                            }
                        }
                    });

                    //Convert to Exclude calendar Array
                    let bookedList = []; bookingCalendar.forEach(booking => { if (booking.availableRooms <= booking.booked) { bookedList.push(booking.date); } });
                    let selectedList = []; bookingCalendar.forEach(booking => { if (booking.booked > 0 && booking.availableRooms > booking.booked) { selectedList.push(booking.date); } });
                    let calendar = Metro.getPlugin("#BookedRoomCalendar", "calendar");
                    calendar.selected = []; calendar.exclude = []; calendar.clearSelected();
                    calendar.setExclude(bookedList); calendar.setPreset(selectedList); calendar.setShow(new Date());

                    //Fill Table Data
                    let tableData = []; bookingCalendar.forEach(booking => { tableData.push([booking.date, booking.availableRooms , booking.booked  ]) });
                    var table = Metro.getPlugin("#CalendarDataTable", "table"); table.setItems(tableData); table.reload();

                    // Fill All Status Data
                    messageData = "<ul data-role='treeview'>"; 
                    this.$store.state.roomBookingList.forEach(booking => {
                        if (booking.reservation != null) {
                            messageData += "<li data-caption='" + booking.reservation.reservationNumber + " <b>" + booking.reservation.totalPrice + this.hotel.defaultCurrency.name + "</b> " + new Date(booking.reservation.startDate).toLocaleDateString() + " - " + new Date(booking.reservation.endDate).toLocaleDateString() + " " + booking.status.systemName + " <b>" + booking.count + "x</b> " + " ' >";
                            messageData += "<ul><li data-caption='" + booking.count + "x " + booking.name + " " + (!booking.extraBed ? "" : " + <span class=\"mif-hotel mif-2x fg-cyan\" style=\"top:3px;\" data-role=\"hint\" data-cls-hint=\"bg-cyan fg-white drop-shadow\" :data-hint-text=\"" + window.dictionary('labels.extraBed') + "\" ></span>") + "' ></li>";
                            messageData += "<li data-caption='" + new Date(booking.reservation.startDate).toLocaleDateString() + " - " + new Date(booking.reservation.endDate).toLocaleDateString() + " " + window.dictionary("labels.nightCount") + ": <b>" + parseInt(((new Date(booking.endDate) - new Date(booking.startDate)) / (1000 * 60 * 60 * 24))) + "x</b> " + "' ></li>";
                            messageData += "<li data-caption='" + window.dictionary('labels.adults') + ": " + booking.reservation.adult + " " + window.dictionary('labels.children') + ": " + booking.reservation.children + "' ></li>";
                            messageData += "</ul></li>";
                        }
                    }); messageData += "</ul>";
                    $("#BookingDataBox").html(messageData);
                });
                
                
            }
        },
        async changeSelectedHistory() {
            if ($("#history")[0] != undefined && $("#history")[0].selectedOptions[0] != undefined && $("#history")[0].selectedOptions[0].value != "") {
                this.historyId = $("#history")[0].selectedOptions[0].value;
                this.hotel.hotelReservationLists.forEach(async reservation => {
                    if (reservation.id == this.historyId) {

                        let messageData = "";
                        messageData = "<ul class='feed-list'>";
                        reservation.hotelReservedRoomLists.forEach((room) => {
                            messageData += "<li><img class='avatar' src='" + this.$store.state.apiRootUrl + "/RoomImage/" + room.hotelRoomId + "' >";
                            messageData += "<span class='label'>" + room.name + "</span>";
                            messageData += "<span class='second-label fg-black bold' style='font-size: 14px;'>" + room.count + "x" + (!room.extraBed ? "" : "+ <span class='mif-hotel mif-2x fg-cyan' style='top:3px;' data-role='hint' data-cls-hint='bg-cyan fg-white drop-shadow' : data-hint-text='" + window.dictionary('labels.extraBed') + "'></span>") + "</span>";
                            messageData += "</li>";
                        }); messageData += "</ul>";
                        $("#HistoryRoomsBox").html(messageData);
                        messageData = "";

                        reservation.hotelReservationDetailLists.forEach(async (detail, index) => {
                            messageData += "<div class=\"card image-header\"><div class=\"d-block card-content p-0 " + (!detail.shown && detail.guestSender ? " bg-gitlab " : !detail.guestSender ? " bg-brandColor1 " : " bg-lightOlive ") + "\">";
                            messageData += "<div class=\"d-flex\"><div class=\"h5 fg-black col-xl-3 col-lg-3 col-md-3 col-sm-3 col-12 p-1 m-0\">" + this.$store.state.statusList.filter(obj => { return obj.Id == detail.statusId })[0].TranslationName + "</div>";
                            messageData += "<div class=\"h5 fg-black col-xl-4 col-lg-4 col-md-4 col-sm-4 col-12 text-center p-1 m-0\">" + new Date(detail.startDate).toLocaleDateString('cs-CZ') + " - " + new Date(detail.endDate).toLocaleDateString('cs-CZ') + "</div>";
                            messageData += "<div class=\"h5 fg-black col-xl-2 col-lg-2 col-md-2 col-sm-2 col-12 p-1 text-right m-0\">" + detail.adult + "/" + detail.children + " | " + detail.totalPrice + "</div>";
                            messageData += "<div class=\"h8 fg-black col-xl-3 col-lg-3 col-md-3 col-sm-3 col-12 text-right p-1 m-0\">" + new Date(detail.timestamp).toLocaleString('cs-CZ') + "</div></div>";
                            messageData += "<div class=\"d-flex\"><div class=\"fg-white col-xl-10 col-lg-10 col-md-10 col-sm-10 col-12 p-1\">" + detail.message + "</div></div>";
                            messageData += "</div></div>";
                        });
                        $("#HistoryMessageBox").html(messageData);
                    }
                });
            } else {
                this.historyId = null;
                $("#HistoryRoomsBox").html('');
                $("#HistoryMessageBox").html('');
            }
        },
        async changeSelectedReservation() {
            if ($("#reservations")[0] != undefined && $("#reservations")[0].selectedOptions[0] != undefined && $("#reservations")[0].selectedOptions[0].value != "") {
                this.reservationId = $("#reservations")[0].selectedOptions[0].value;

                let setShown = false;
                await this.hotel.hotelReservationLists.forEach(async reservation => {
                    if (reservation.id == this.reservationId) {

                        let select = Metro.getPlugin("#hotelStatus", "select");
                        select.val(reservation.statusId);

                        let messageData = "";
                        messageData = "<ul class='feed-list'>";
                        reservation.hotelReservedRoomLists.forEach((room) => {
                            messageData += "<li><img class='avatar' src='" + this.$store.state.apiRootUrl + "/RoomImage/" + room.hotelRoomId + "' >";
                            messageData += "<span class='label'>" + room.name + "</span>";
                            messageData += "<span class='second-label fg-black bold' style='font-size: 14px;'>" + room.count + "x" + (!room.extraBed ? "" : "+ <span class='mif-hotel mif-2x fg-cyan' style='top:3px;' data-role='hint' data-cls-hint='bg-cyan fg-white drop-shadow' : data-hint-text='" + window.dictionary('labels.extraBed') + "'></span>") + "</span>";
                            messageData += "</li>";
                        }); messageData += "</ul>";
                        $("#ReservationRoomsBox").html(messageData);
                        messageData = "";

                        reservation.hotelReservationDetailLists.forEach(async (detail, index) => {
                            messageData += "<div class=\"card image-header\"><div class=\"d-block card-content p-0 " + (!detail.shown && detail.guestSender ? " bg-gitlab " : !detail.guestSender ? " bg-brandColor1 " : " bg-lightOlive ") + "\">";
                            messageData += "<div class=\"d-flex\"><div class=\"h5 fg-black col-xl-3 col-lg-3 col-md-3 col-sm-3 col-12 p-1 m-0\">" + this.$store.state.statusList.filter(obj => { return obj.Id == detail.statusId })[0].TranslationName + "</div>";
                            messageData += "<div class=\"h5 fg-black col-xl-4 col-lg-4 col-md-4 col-sm-4 col-12 text-center p-1 m-0\">" + new Date(detail.startDate).toLocaleDateString('cs-CZ') + " - " + new Date(detail.endDate).toLocaleDateString('cs-CZ') + "</div>";
                            messageData += "<div class=\"h5 fg-black col-xl-2 col-lg-2 col-md-2 col-sm-2 col-12 p-1 text-right m-0\">" + detail.adult + "/" + detail.children + " | " + detail.totalPrice + "</div>";
                            messageData += "<div class=\"h8 fg-black col-xl-3 col-lg-3 col-md-3 col-sm-3 col-12 text-right p-1 m-0\">" + new Date(detail.timestamp).toLocaleString('cs-CZ') + "</div></div>";
                            messageData += "<div class=\"d-flex\"><div class=\"fg-white col-xl-10 col-lg-10 col-md-10 col-sm-10 col-12 p-1\">" + detail.message + "</div></div>";
                            messageData += "</div></div>";

                            //Set Shown
                            if (!detail.shown && detail.guestSender) { setShown = true; detail.shown = true; }
                            if (reservation.hotelReservationDetailLists.length == index + 1 && setShown) {
                                await this.$store.dispatch("setAdvertiserShown", this.reservationId);
                            }
                        });
                        $("#ReservationMessageBox").html(messageData);
                    }
                });
            } else {
                this.reservationId = null;
                $("#ReservationRoomsBox").html('');
                $("#ReservationMessageBox").html('');
            }
        },
        async generateMessageBox() {
            if (this.hotel.id == this.newCommentHotelId) {
                let messageData = "";
                this.hotel.guestAdvertiserNoteLists.forEach(message => {
                    messageData += "<div class=\"card image-header\"><div class=\"d-block card-content p-0 " + (!message.solved ? " bg-brandColor1 " : " bg-lightOlive ") + "\"><div class=\"d-flex\"><div class=\"h5 fg-black col-xl-8 col-lg-8 col-md-8 col-sm-8 col-12 p-1 m-0\">" + message.title + "</div><div class=\"h8 fg-black col-xl-4 col-lg-4 col-md-4 col-sm-4 col-12 text-right p-1 m-0\">" + new Date(message.timeStamp).toLocaleString('cs-CZ') + "</div></div>";
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
                        that.$store.state.apiRootUrl + '/Advertiser/SetGuestComment', {
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
        async setNewReservationDetail() {
            $("#ReservationForm").submit();

            var that = this;
            setTimeout(async function () {
                if (newMessageIsValid) {
                    window.showPageLoading();
                    let response = await fetch(
                        that.$store.state.apiRootUrl + '/Advertiser/SetAdvertiserDetail', {
                        method: 'POST', headers: { 'Authorization': 'Bearer ' + that.$store.state.user.Token, 'Content-type': 'application/json' },
                        body: JSON.stringify({ ReservationId: that.reservationId, StatusId: $("#hotelStatus")[0].selectedOptions[0].value, DetailMessage: $("#ReservationNote").val(), Language: that.$store.state.language })
                    }); let result = await response.json();

                    if (result.Status == "error") {
                        var notify = Metro.notify; notify.setup({ width: 300, timeout: that.$store.state.userSettings.notifyShowTime, duration: 500 });
                        notify.create(result.ErrorMessage, "Error", { cls: "alert" }); notify.reset();
                    } else {
                        if (document.getElementById("ReservationForm") != null) { document.getElementById("ReservationForm").reset(); }
                        $("#ReservationNote").val('');
                        let result = await that.$store.dispatch("getAdvertisementList");
                        that.openReservation();
                    }
                    window.hidePageLoading();
                }
            }, 1000);
        },
        openReservation() {
            let select = Metro.getPlugin("#reservations", "select"); let options = [];
            this.hotel.hotelReservationLists.forEach(reservation => {
                let newMessage = false;
                if (new Date(reservation.endDate) >= new Date()) {
                    reservation.hotelReservationDetailLists.forEach((detail, index) => {
                        if (!detail.shown && detail.guestSender) { newMessage = true; }
                        if (reservation.hotelReservationDetailLists.length == index + 1) {
                            options.push({ val: reservation.id, title: (newMessage ? "!!! " : "") + " " + this.$store.state.statusList.filter(obj => { return obj.Id == reservation.statusId })[0].TranslationName + " " + reservation.reservationNumber + " " + new Date(reservation.startDate).toLocaleDateString('cs-CZ'), selected: false });
                        }
                    });
                }
            });
            select.data(""); select.addOptions(options);

            select = Metro.getPlugin("#hotelStatus", "select"); options = [];
            this.$store.state.statusList.forEach(status => {
                options.push({ val: status.Id, title: status.TranslationName, selected: false });
            });
            select.data(""); select.addOptions(options);

            this.reservationId = null;
            $("#ReservationRoomsBox").html('');
            $("#ReservationMessageBox").html('');
        },
        async openUnavailableSettings() {
            let select = Metro.getPlugin("#RoomSelectionUnv", "select"); let options = [];
            this.hotel.hotelRoomLists.forEach(room => {
                options.push({ val: room.id, title: room.roomsCount + "x " + room.name + " ", selected: false });
            });
            select.data(""); select.addOptions(options);
            await this.generateUnavailableRoomListBox();
        },
        async generateUnavailableRoomListBox() {
            let result = await this.$store.dispatch("getUnavailableRoomList", this.hotel.id);
            let messageData = "<ul>";

            this.$store.state.unavailableRoomList.forEach(unavailable => {
                messageData += "<li><span class='mif-cancel fg-red c-pointer' onclick=\"deleteUnavailableRoom('" + unavailable.Id + "','" + this.$store.state.apiRootUrl + "' )\"></span> " + unavailable.Count + "x " + unavailable.Name + " <b>" + new Date(unavailable.StartDate).toLocaleDateString() + " - " + new Date(unavailable.EndDate).toLocaleDateString() + "</b></li>";
            });

            messageData += "</ul>";
            $("#UnavailableRoomListBox").html(messageData);
        },
        async setNewUnavailable() {
            $("#UnavailableForm").submit();

            var that = this;
            setTimeout(async function () {
                if (newUnavailableIsValid) {
                    window.showPageLoading();
                    let response = await fetch(
                        that.$store.state.apiRootUrl + '/Advertiser/SetUnavailableRooms', {
                        method: 'POST', headers: { 'Authorization': 'Bearer ' + that.$store.state.user.Token, 'Content-type': 'application/json' },
                            body: JSON.stringify({
                                HotelId: that.hotel.id,
                                RoomsId: $("#RoomSelectionUnv").val(),
                                StartDate: $("#UnavailableFromDate").val().split('.')[2] + "-" + $("#UnavailableFromDate").val().split('.')[1] + "-" + $("#UnavailableFromDate").val().split('.')[0],
                                EndDate: $("#UnavailableToDate").val().split('.')[2] + "-" + $("#UnavailableToDate").val().split('.')[1] + "-" + $("#UnavailableToDate").val().split('.')[0],
                                Language: that.$store.state.language
                            })
                    }); let result = await response.json();

                    if (result.Status == "error") {
                        var notify = Metro.notify; notify.setup({ width: 300, timeout: that.$store.state.userSettings.notifyShowTime, duration: 500 });
                        notify.create(result.ErrorMessage, "Error", { cls: "alert" }); notify.reset();
                    } else {
                        if (document.getElementById("UnavailableForm") != null) {
                            document.getElementById("UnavailableForm").reset(); let select = Metro.getPlugin("#RoomSelectionUnv", "select"); select.reset();
                        }
                    }
                    window.hidePageLoading();
                }
            }, 1000);
        },
        openCalendar() {
            let select = Metro.getPlugin("#roomSelection", "select"); let options = [];
            this.hotel.hotelRoomLists.forEach(room => {
                options.push({ val: room.id, title: room.roomsCount + "x " + room.name + " ", selected: false });
            });
            select.data(""); select.addOptions(options);

            this.roomId = null;
            $("#CalendarRoomBox").html('');
            $("#BookingDataBox").html('');
        },
        openHistory() {
            let select = Metro.getPlugin("#history", "select"); let options = [];
            this.hotel.hotelReservationLists.forEach(reservation => {
                let newMessage = false;
                if (new Date(reservation.endDate) < new Date()) {
                    reservation.hotelReservationDetailLists.forEach((detail, index) => {
                        if (!detail.shown && detail.guestSender) { newMessage = true; }
                        if (reservation.hotelReservationDetailLists.length == index + 1) {
                            options.push({ val: reservation.id, title: (newMessage ? "!!! " : "") + " " + this.$store.state.statusList.filter(obj => { return obj.Id == reservation.statusId })[0].TranslationName + " " + reservation.reservationNumber + " " + new Date(reservation.startDate).toLocaleDateString('cs-CZ'), selected: false });
                        }
                    });
                }

            });
            select.data(""); select.addOptions(options);

            this.historyId = null;
            $("#HistoryRoomsBox").html('');
            $("#HistoryMessageBox").html('');
        },
        openReview() {
            let select = Metro.getPlugin("#reviews", "select"); let options = [];
            let messageData = ""; let setShown = false;
            this.hotel.hotelReservationReviewLists.forEach(async (review, index) => {

                messageData += "<div class=\"card image-header\"><div class=\"d-block card-content p-0 " + (!review.advertiserShown ? " bg-gitlab " : " bg-lightOlive ") + "\">";
                messageData += "<div class=\"d-flex\"><div class=\"h5 fg-black col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12 p-1 m-0\"><input data-role=\"rating\" data-value=\"" + review.rating + "\" data-stared-color=\"cyan\" data-static=\"true\" ></div>";
                messageData += "<div class=\"h5 fg-black col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12 text-right p-1 m-0\">" + new Date(review.timestamp).toLocaleString('cs-CZ') + "</div></div>";
                messageData += "<div class=\"d-flex\"><div class=\"fg-white col-xl-10 col-lg-10 col-md-10 col-sm-10 col-12 p-1\">" + window.dictionary('labels.comment') + ": " + review.description + "</div></div>";
                if (review.answer != null && review.answer.length > 0) { messageData += "<div class=\"d-flex\"><div class=\"fg-white col-xl-10 col-lg-10 col-md-10 col-sm-10 col-12 p-1\">" + window.dictionary('labels.answer') + ": " + review.answer + "</div></div>"; }
                messageData += "</div></div>";

                if (review.answer == null || review.answer.length == 0) {
                    options.push({ val: review.id, title: (review.advertiserShown == false ? "!!!" : "") + " " + new Date(review.timestamp).toLocaleString('cs-CZ') });
                }

                //Set Shown
                if (!review.advertiserShown) { setShown = true; review.advertiserShown = true; }
                if (this.hotel.hotelReservationReviewLists.length == index + 1 && setShown) {
                    await this.$store.dispatch("setReviewShown", this.hotel.id);
                }

            });
            select.data(""); select.addOptions(options);
            $("#reviewAllMessagesBox").html(messageData);
            this.reviewId = null;
        },
        async changeSelectedReview() {
            if ($("#reviews")[0] != undefined && $("#reviews")[0].selectedOptions[0] != undefined) {
                this.reviewId = $("#reviews")[0].selectedOptions[0].value;
                let messageData = "";
                this.hotel.hotelReservationReviewLists.forEach(async review => {
                    if (review.id == this.reviewId) {
                        messageData += "<div class=\"card image-header\"><div class=\"d-block card-content p-0 " + (!review.advertiserShown ? " bg-gitlab " : " bg-lightOlive ") + "\">";
                        messageData += "<div class=\"d-flex\"><div class=\"h5 fg-black col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12 p-1 m-0\"><input data-role=\"rating\" data-value=\"" + review.rating + "\" data-stared-color=\"cyan\" data-static=\"true\" ></div>";
                        messageData += "<div class=\"h5 fg-black col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12 text-right p-1 m-0\">" + new Date(review.timestamp).toLocaleString('cs-CZ') + "</div></div>";
                        messageData += "<div class=\"d-flex\"><div class=\"fg-white col-xl-10 col-lg-10 col-md-10 col-sm-10 col-12 p-1\">" + window.dictionary('labels.comment') + ": " + review.description + "</div></div>";
                        messageData += "</div></div>";
                    }
                });
                $("#ReviewInfoBox").html(messageData);
            }
        },
        async setNewReviewAnswer() {
            $("#ReviewForm").submit();

            var that = this;
            setTimeout(async function () {
                if (newReviewIsValid) {
                    window.showPageLoading();
                    let response = await fetch(
                        that.$store.state.apiRootUrl + '/Advertiser/SetAdvertiserReviewAnswer', {
                        method: 'POST', headers: { 'Authorization': 'Bearer ' + that.$store.state.user.Token, 'Content-type': 'application/json' },
                        body: JSON.stringify({ ReviewId: that.reviewId, Answer: $("#ReviewNote").val(), Language: that.$store.state.language })
                    }); let result = await response.json();

                    if (result.Status == "error") {
                        var notify = Metro.notify; notify.setup({ width: 300, timeout: that.$store.state.userSettings.notifyShowTime, duration: 500 });
                        notify.create(result.ErrorMessage, "Error", { cls: "alert" }); notify.reset();
                    } else {
                        if (document.getElementById("ReviewForm") != null) { document.getElementById("ReviewForm").reset(); }
                        $("#ReviewNote").val('');
                        let result = await that.$store.dispatch("getAdvertisementList");
                        that.openReview();
                    }
                    window.hidePageLoading();
                }
            }, 1000);

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
                Properties: this.hotel.hotelPropertyAndServiceLists,
                LimitGuestCommDays: this.hotel.enabledCommDaysBeforeStart,
                StornoDaysCountBeforeStart: this.hotel.stornoDaysCountBeforeStart,
                GuestStornoEnabled: this.hotel.guestStornoEnabled
            };
            this.$router.push('/Profile/AdvertisementWizard');
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
                type: "",
                removeOnClose: true,
                width: "600",
                height: "auto"
            });
        },
        async setApprovalRequest(hotelId) {
            disableScroll();
            var response = await fetch(
                this.$store.state.apiRootUrl + '/Advertiser/ApprovalRequest/' + hotelId, {
                method: 'GET', headers: { 'Authorization': 'Bearer ' + this.$store.state.user.Token, 'Content-type': 'application/json' }
            }); let result = await response.json();
            if (this.$store.state.user.UserId != '') {
                await this.$store.dispatch("getAdvertisementList");
                if (!this.$store.state.advertisementList.length) { this.errorText = true; }
            }
            enableScroll();
        },
        async deleteAdvertisement(hotelId) {
            if (confirm(window.dictionary("messages.doYouReallyWantDeleteAdvertisement"))) {
                window.showPartPageLoading();
                disableScroll();
                var response = await fetch(
                    this.$store.state.apiRootUrl + '/Advertiser/DeleteAdvertisement/' + hotelId, {
                    method: 'GET', headers: { 'Authorization': 'Bearer ' + this.$store.state.user.Token, 'Content-type': 'application/json' }
                }); let result = await response.json();
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
            }
        },
        async setActivation(hotelId) {
            window.showPartPageLoading();
            disableScroll();
            var response = await fetch(
                this.$store.state.apiRootUrl + '/Advertiser/ActivationHotel/' + hotelId, {
                method: 'GET', headers: { 'Authorization': 'Bearer ' + this.$store.state.user.Token, 'Content-type': 'application/json' }
            }); let result = await response.json();
            if (this.$store.state.user.UserId != '') {
                await this.$store.dispatch("getAdvertisementList");
                if (!this.$store.state.advertisementList.length) { this.errorText = true; }
            }
            window.hidePartPageLoading();
            enableScroll();
        },
        createPriceInfoBox() {
            if (this.infoBox != null) { Metro.infobox.close(this.infoBox); }
            let htmlContent = "<div class='skill-box bg-transparent'><h5>" + this.$i18n.t('labels.servicesAndFacilitiesAvailable') + "</h5>";
            let lastGroup = null;
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

            this.infoBox = Metro.infobox.create(htmlContent + "</div>", "", {
                closeButton: true,
                type: "",
                removeOnClose: true,
                width: "400",
                height: "auto"
            });
        },
        hotelDetailsClick(event) {
            this.$store.state.backRoute = "/Profile/Advertisement";
            this.$store.state.backRouteScroll = window.scrollY;

            this.$store.dispatch("setHotel", this.hotel);
            this.$router.push('/Hotels/' + this.hotel.id);
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
            if (window.watchAdvertisementVariables.reloadUnavailable) {
                window.watchAdvertisementVariables.reloadUnavailable = false;
                await this.generateUnavailableRoomListBox();
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