import { createStore } from 'vuex';
import router from '../router/index';
import { encode } from "base-64";

/*
APiRoots Urls
http://nomad.ubytkac.cz:5000/WebApi
http://localhost:5000/WebApi
*/
const store = createStore({
    state: {
        toastErrorMessage: null,
        toastSuccessMessage: null,
        toastInfoMessage: null,

        tempVariables: {
            registrationStatus: null,
            goBackToRoomsPage: false
        },

        system: {
            passwordMin: 6,
        },

        userSettings: {
            topFiveCount: 5,
            notifyShowTime: 2000,
            showInactiveAdvertisementAsDefault: true,
            translationLanguage: '',
            hideSearchingInPrivateZone : false
        },

        apiRootUrl: 'http://localhost:5000/WebApi',
        language: 'cz',
        hotel: [],
        unavailableRoomList: [],
        webMottoList: [],
        roomBookingList: [],
        reviewList: [],
        statusList: [],
        privacyPolicyList: [],
        termsList: [],
        advertisementList: [],
        propertyGroupList: [],
        bookingList: [],
        propertyList: [],
        roomTypeList: [],
        ubytkacInfoList: [],
        registrationInfoList: [],
        oftenQuestionList: [],
        holidayTipsList: [],
        searchDialList: [],
        searchAreaList: [],
        lightFavoriteHotelList: [],
        favoriteHotelList: [],
        topFiveList: [],
        searchResults: [],
        filteredResults: [],
        backRoute: "/",
        backRouteScroll: 0,
        reservedRoomList: [],
        searchButtonLoading: false,
        searchString: {
            string: '',
            inputAdult: 0,
            inputChild: 0,
            inputRooms: 0,
            dates: [],
        },
        bookingDetail: {
            /*cleared by clearBooking*/
            hotelId: null,
            hotelName: null,
            currencyId: null,
            currency: null,
            startDate: null,
            endDate: null,
            adultInput: 0,
            childrenInput: 0,
            message: null,
            verified: false,
            user: {
                firstName: null,
                lastName: null,
                email: null,
                street: null,
                phone: null,
                city: null,
                country: null,
                zipCode: null
            },
            rooms: [],
            totalPrice: 0
        },

        user: {
            loggedIn: false,
        },

    },
    mutations: {
        setWebMottoList(store, value) {
            store.webMottoList = value;
            console.log("setWebMottoList", store.webMottoList);
        },
        setReviewList(store, value) {
            store.reviewList = value;
            console.log("setReviewList", store.reviewList);
        },
        setStatusList(store, value) {
            store.statusList = value;
        },
        setPrivacyPolicyList(store, value) {
            store.privacyPolicyList = value;
        },
        setTermsList(store, value) {
            store.termsList = value;
        },
        setAdvertisementList(store, value) {
            store.advertisementList = value;
            console.log("setAdvertisementList", store.advertisementList);
        },
        setUnavailableRoomList(store, value) {
            store.unavailableRoomList = value;
            console.log("setUnavailableRoomList  ", store.unavailableRoomList);
        },
        setRoomBookingList(store, value) {
            store.roomBookingList = value;
            console.log("setRoomBookingList", store.roomBookingList);
        },
        setPropertyGroupList(store, value) {
            store.propertyGroupList = value;
        },
        setBookingList(store, value) {
            store.bookingList = value;
            console.log("setBookingList", store.bookingList);
        },
        setHotel(store, value) {
            store.hotel = value;
            console.log("settedHotel", store.hotel);
        },
        setTopFiveList(store, value) {
            store.topFiveList = value;
        },
        setLightFavoriteHotelList(store, value) {
            store.lightFavoriteHotelList = value;
        },
        setFavoriteHotelList(store, value) {
            store.favoriteHotelList = value;
        },
        setReservedRoomList(store, value) {
            store.reservedRoomList = value;

            //Prepare Room List for Booking
            store.bookingDetail.rooms = [];
            store.hotel.hotelRoomLists.forEach(room => {
                store.bookingDetail.rooms.push({
                    id: room.id,
                    typeId: room.roomTypeId,
                    name: room.name,
                    price: room.price,
                    booked: 0,
                    extrabed: false
                });
            });
            store.bookingDetail.totalPrice = 0;

            // get to info and next to Rooms Back On dates change in Rooms for Full Refresh
            if (router.currentRoute.value.name == "Rooms") {
                router.push({ name: 'Info' });
                store.tempVariables.goBackToRoomsPage = true;
            }
        },
        setPropertyList(store, value) {
            store.propertyList = value;
        },
        setRoomTypeList(store, value) {
            store.roomTypeList = value;
        },
        setSearchDialList(store, value) {
            store.searchDialList = value;
        },
        setSearchAreaList(store, value) {
            store.searchAreaList = value;
        },
        setUbytkacInfoList(store, value) {
            store.ubytkacInfoList = value;
        },
        setRegistrationInfoList(store, value) {
            store.registrationInfoList = value;
        },
        setOftenQuestionList(store, value) {
            store.oftenQuestionList = value;
        },
        setHolidayTipsList(store, value) {
            store.holidayTipsList = value;
        },
        setMainTopList(store, value) {
            store.searchResults = value;
            store.searchButtonLoading = false;
        },
        setHotelSearchResultsList(store, value) {
            store.searchResults = value;
            store.filteredResults = value;
            store.searchButtonLoading = false;
        },
        setSearchButtonLoadingTrue(store, value) {
            store.searchButtonLoading = true;
        },
        async setDates(state, date) {
            state.searchString.dates = date;

            // Updating BookedCalendar in Hotel Detail
            if (state.searchString.dates.length && state.searchString.dates[1] != null &&
                (router.currentRoute.value.name == "Info" ||
                    router.currentRoute.value.name == "Hotels" ||
                    router.currentRoute.value.name == "Rooms" ||
                    router.currentRoute.value.name == "Photos" ||
                    router.currentRoute.value.name == "Reviews")
                ) { await this.dispatch("getReservedRoomList", state.hotel.id); }
        },
        setSearchString(state, value) {
            state.searchString.string = value;
        },
        setBookedTotalPrice(state) {
            var days = Math.floor(
                (Date.parse(state.searchString.dates[1].toLocaleDateString('sv-SE')) -
                    Date.parse(state.searchString.dates[0].toLocaleDateString('sv-SE'))) /
                86400000
            )
            state.bookingDetail.totalPrice = 0;
            state.bookingDetail.rooms.forEach(room => {
                if (room.booked > 0) { state.bookingDetail.totalPrice += days * room.price * room.booked; }
            });

        },
        setUser(state, data) {
            state.user = data;
            state.user.loggedIn = true;
        },
        logOutUser(state) {
            store.advertisementList = [];
            store.lightFavoriteHotelList = [];
            store.favoriteHotelList = [];
            store.bookingList = [];
            
            state.user = {
                loggedIn: false,
            }
            Metro.storage.storage.clear();
        },
    },
    actions: {
        setDates({ commit }, date) {
            console.log("setDates", date);
            commit('setDates', date);
        },
        setLightFavoriteHotelList({ commit }, value) {
            commit('setLightFavoriteHotelList', value);
        },
        setBookedTotalPrice({ commit }) {
            commit('setBookedTotalPrice');
        },
        async setHotel({ commit }, hotel) {
            commit('setHotel', hotel)
        },
        async logout({ commit }) {
            commit('logOutUser')

            router.push('/')
        },
        clearBooking() {
            this.state.bookingDetail = {
                hotelId: null,
                hotelName: null,
                currencyId: null,
                currency: null,
                startDate: null,
                endDate: null,
                adultInput: 0,
                childrenInput: 0,
                message: null,
                verified: false,
                user: {
                    firstName: null,
                    lastName: null,
                    email: null,
                    street: null,
                    phone: null,
                    city: null,
                    country: null,
                    zipCode: null
                },
                rooms: [],
                totalPrice: 0
            }
        },
        async searchHotels({ commit }, searchString) {
            let mainloader; if (!this.state.searchResults == [] || !this.state.searchResults.hotelList.length) { mainloader = true; window.showPageLoading(); } else { mainloader = false; window.showPartPageLoading(); }
            if (searchString === null || searchString == '') { searchString = "null"; }

            let response = await fetch(
                this.state.apiRootUrl + '/Search/GetSearchInput/' + searchString + '/' + this.state.language, {
                method: 'GET',
                headers: {
                    'Content-type': 'application/json'
                }
            });
            let result = await response.json();
            console.log("hotellist", result);

            commit('setHotelSearchResultsList', result.hotelList);
            if (mainloader) { window.hidePageLoading(); } else {window.hidePartPageLoading(); }

            if (result) {
                router.push({ name: 'Result' });
            }
            
        },
        async getReservedRoomList({ commit }, hotelId) {
            let mainloader; if (!this.state.reservedRoomList.length) { mainloader = true; window.showPageLoading(); } else { mainloader = false; window.showPartPageLoading(); }
            let startDate; let endDate;
            
            if (this.state.searchString.dates.length) {
                startDate = this.state.searchString.dates[0].toLocaleDateString('sv-SE');
                endDate = this.state.searchString.dates[1].toLocaleDateString('sv-SE');
            }

            let response = await fetch(
                this.state.apiRootUrl + '/ReservedRoomList/' + hotelId + '/' + startDate + '/' + endDate + '/' + this.state.language, {
                method: 'GET',
                headers: {
                    'Content-type': 'application/json'
                }
            });
            let result = await response.json();
            commit('setReservedRoomList', result);
            if (mainloader) { window.hidePageLoading(); } else {window.hidePartPageLoading(); }
        },
        async getMainTopList({ commit }) {
            let mainloader; if (!this.state.searchResults == [] || !this.state.searchResults.hotelList.length) { mainloader = true; window.showPageLoading(); } else { mainloader = false; window.showPartPageLoading(); }
            let response = await fetch(
                this.state.apiRootUrl + '/Top/GetMainTopList/' + this.state.language, {
                method: 'GET',
                headers: {
                    'Content-type': 'application/json'
                }
            });
            let result = await response.json();
            commit('setMainTopList', result);
            if (mainloader) { window.hidePageLoading(); } else {window.hidePartPageLoading(); }

        },
        async getPropertyList({ commit }) {
            if (!this.state.propertyList.length) {
                let mainloader; if (!this.state.propertyList.length) { mainloader = true; window.showPageLoading(); } else { mainloader = false; window.showPartPageLoading(); }
                let response = await fetch(
                    this.state.apiRootUrl + '/Properties/' + this.state.language, {
                    method: 'GET',
                    headers: {
                        'Content-type': 'application/json'
                    }
                });
                let result = await response.json();
                //console.log("Property", result);
                commit('setPropertyList', result);
                if (mainloader) { window.hidePageLoading(); } else { window.hidePartPageLoading(); }
            }
        },
        async getPropertyGroupList({ commit }) {
            if (!this.state.propertyGroupList.length) {
                let mainloader; if (!this.state.propertyGroupList.length) { mainloader = true; window.showPageLoading(); } else { mainloader = false; window.showPartPageLoading(); }
                let response = await fetch(
                    this.state.apiRootUrl + '/Properties/GetPropertyGroupList/' + this.state.language, {
                    method: 'GET',
                    headers: {
                        'Content-type': 'application/json'
                    }
                });
                let result = await response.json();
                console.log("PropertyGroup", result);
                commit('setPropertyGroupList', result);
                if (mainloader) { window.hidePageLoading(); } else { window.hidePartPageLoading(); }
            }
        },
        async getAdvertisementList({ commit }) {
            window.showPageLoading();
            let response = await fetch(
                this.state.apiRootUrl + '/Advertiser/GetAdvertisementList/' + this.state.language, {
                method: 'GET',
                headers: {
                    'Authorization': 'Bearer ' + this.state.user.Token,
                    'Content-type': 'application/json',
                }
            });
            let result = await response.json();
            commit('setAdvertisementList', result);
            window.hidePageLoading();
        },
        async getUnavailableRoomList({ commit }, hotelId) {
            window.showPageLoading();
            let response = await fetch(
                this.state.apiRootUrl + '/Advertiser/GetUnavailableRooms/' + hotelId, {
                method: 'GET',
                headers: {
                    'Authorization': 'Bearer ' + this.state.user.Token,
                    'Content-type': 'application/json',
                }
            });
            let result = await response.json();
            commit('setUnavailableRoomList', result);
            window.hidePageLoading();
        },

        async getRoomBookingList({ commit }, hotelRoomId) {
            window.showPageLoading();
            let response = await fetch(
                this.state.apiRootUrl + '/Advertiser/GetRoomBookingList/' + hotelRoomId + "/" + this.state.language, {
                method: 'GET',
                headers: {
                    'Authorization': 'Bearer ' + this.state.user.Token,
                    'Content-type': 'application/json',
                }
            });
            let result = await response.json();
            commit('setRoomBookingList', result);
            window.hidePageLoading();
        },
        async getReviewList({ commit }) {
            window.showPageLoading();
            let response = await fetch(
                this.state.apiRootUrl + '/Advertiser/GetReviewList', {
                method: 'GET',
                headers: {
                    'Authorization': 'Bearer ' + this.state.user.Token,
                    'Content-type': 'application/json',
                }
            });
            let result = await response.json();
            commit('setReviewList', result);
            window.hidePageLoading();
        },
        async getStatusList({ commit }) {
            window.showPageLoading();
            let response = await fetch(
                this.state.apiRootUrl + '/Advertiser/GetStatusList/' + this.state.language, {
                method: 'GET',
                headers: {
                    'Authorization': 'Bearer ' + this.state.user.Token,
                    'Content-type': 'application/json',
                }
            });
            let result = await response.json();
            commit('setStatusList', result);
            window.hidePageLoading();
        },
        async getWebMottoList({ commit }) {
            let response = await fetch(
                this.state.apiRootUrl + '/WebPages/GetWebMottoList', {
                method: 'GET',
                headers: { 'Content-type': 'application/json' }
            });
            let result = await response.json();
            commit('setWebMottoList', result);
        },
        async getRoomTypeList({ commit }) {
            let mainloader; if (!this.state.roomTypeList.length) { mainloader = true; window.showPageLoading(); } else { mainloader = false; window.showPartPageLoading(); }
            let response = await fetch(
                this.state.apiRootUrl + '/RoomTypes/' + this.state.language, {
                method: 'GET',
                headers: {
                    'Content-type': 'application/json'
                }
            });
            let result = await response.json();
            commit('setRoomTypeList', result);
            if (mainloader) { window.hidePageLoading(); } else {window.hidePartPageLoading(); }
        },
        async getUbytkacInfoList({ commit }) {
            let mainloader; if (!this.state.ubytkacInfoList.length) { mainloader = true; window.showPageLoading(); } else { mainloader = false; window.showPartPageLoading(); }
            let response = await fetch(
                this.state.apiRootUrl + '/UbytkacInfo/' + this.state.language, {
                method: 'GET',
                headers: {
                    'Content-type': 'application/json'
                }
            });
            let result = await response.json();
            commit('setUbytkacInfoList', result);
            if (mainloader) { window.hidePageLoading(); } else {window.hidePartPageLoading(); }
        },
        async getRegistrationInfoList({ commit }) {
            let mainloader; if (!this.state.registrationInfoList.length) { mainloader = true; window.showPageLoading(); } else { mainloader = false; window.showPartPageLoading(); }
            let response = await fetch(
                this.state.apiRootUrl + '/RegistrationInfo/' + this.state.language, {
                method: 'GET',
                headers: {
                    'Content-type': 'application/json'
                }
            });
            let result = await response.json();
            commit('setRegistrationInfoList', result);
            if (mainloader) { window.hidePageLoading(); } else {window.hidePartPageLoading(); }
        },
        async getOftenQuestionList({ commit }) {
            let mainloader; if (!this.state.oftenQuestionList.length) { mainloader = true; window.showPageLoading(); } else { mainloader = false; window.showPartPageLoading(); }
            let response = await fetch(
                this.state.apiRootUrl + '/OftenQuestion/' + this.state.language, {
                method: 'GET',
                headers: {
                    'Content-type': 'application/json'
                }
            });
            let result = await response.json();
            commit('setOftenQuestionList', result);
            if (mainloader) { window.hidePageLoading(); } else {window.hidePartPageLoading(); }
        },
        async getHolidayTipsList({ commit }) {
            let mainloader; if (!this.state.holidayTipsList.length) { mainloader = true; window.showPageLoading(); } else { mainloader = false; window.showPartPageLoading(); }
            let response = await fetch(
                this.state.apiRootUrl + '/HolidayTips/' + this.state.language, {
                method: 'GET',
                headers: {
                    'Content-type': 'application/json'
                }
            });
            let result = await response.json();
            commit('setHolidayTipsList', result);
            if (mainloader) { window.hidePageLoading(); } else {window.hidePartPageLoading(); }
        },
        async getPrivacyPolicyList({ commit }) {
            let mainloader; if (!this.state.privacyPolicyList.length) { mainloader = true; window.showPageLoading(); } else { mainloader = false; window.showPartPageLoading(); }
            let response = await fetch(
                this.state.apiRootUrl + '/PrivacyPolicy/' + this.state.language, {
                method: 'GET',
                headers: {
                    'Content-type': 'application/json'
                }
            });
            let result = await response.json();
            commit('setPrivacyPolicyList', result);
            if (mainloader) { window.hidePageLoading(); } else { window.hidePartPageLoading(); }
        },
        async getTermsList({ commit }) {
            let mainloader; if (!this.state.termsList.length) { mainloader = true; window.showPageLoading(); } else { mainloader = false; window.showPartPageLoading(); }
            let response = await fetch(
                this.state.apiRootUrl + '/Terms/' + this.state.language, {
                method: 'GET',
                headers: {
                    'Content-type': 'application/json'
                }
            });
            let result = await response.json();
            commit('setTermsList', result);
            if (mainloader) { window.hidePageLoading(); } else { window.hidePartPageLoading(); }
        },
        async getSearchDialList({ commit }) {
            if (!this.state.searchDialList.length) {
                let mainloader; if (!this.state.searchDialList.length) { mainloader = true; window.showPageLoading(); } else { mainloader = false; window.showPartPageLoading(); }
                let response = await fetch(
                    this.state.apiRootUrl + '/Search/GetSearchDialList/' + this.state.language, {
                    method: 'GET',
                    headers: {
                        'Content-type': 'application/json'
                    }
                });
                let result = await response.json();
                commit('setSearchDialList', result);
                if (mainloader) { window.hidePageLoading(); } else { window.hidePartPageLoading(); }
            }
        },
        async getSearchAreaList({ commit }) {
            if (!this.state.searchAreaList.length) {
                let mainloader; if (!this.state.searchAreaList.length) { mainloader = true; window.showPageLoading(); } else { mainloader = false; window.showPartPageLoading(); }
                let response = await fetch(
                    this.state.apiRootUrl + '/Search/GetSearchAreaList/' + this.state.language, {
                    method: 'GET',
                    headers: {
                        'Content-type': 'application/json'
                    }
                });
                let result = await response.json();
                commit('setSearchAreaList', result);
                if (mainloader) { window.hidePageLoading(); } else { window.hidePartPageLoading(); }
            }
        },
        async login({ commit }, credentials) {
            window.showPageLoading();
            if (credentials.Email) {
                let response = await fetch(this.state.apiRootUrl + '/Guest/WebLogin', {
                    method: 'POST',
                    headers: {
                        'Authorization': 'Basic ' + encode(credentials.Email + ":" + credentials.Password),
                        'Content-type': 'application/json'
                    },
                    body: JSON.stringify({ language: this.state.language })
                });

                let result = await response.json()

                window.hidePageLoading();
                if (result.message) {
                    var notify = Metro.notify; notify.setup({ width: 300, timeout: this.state.userSettings.notifyShowTime, duration: 500 });
                    notify.create(result.message, "Error", { cls: "alert" }); notify.reset();

                } else {
                    commit('setUser', result);
                    Metro.storage.setItem('Token', result.Token);

                    //Load Light Favorites for hearts and user settings
                    await this.dispatch("getLightFavoriteHotelList");
                    await this.dispatch("getGuestSettingList");

                    //path after login
                    router.push('/');
                }

            } else { // remove login on Refresh page
                window.hidePageLoading();
                commit('logOutUser');
                router.push('/')
            }
            //window.hidePageLoading();
        },

        async registration({ commit }, regInfo) {
            window.showPageLoading();
            let response = await fetch(this.state.apiRootUrl + '/Guest/WebRegistration', {
                method: 'POST',
                headers: {
                    'Content-type': 'application/json'
                },
                body: JSON.stringify({ User: regInfo, Language: this.state.language })
            });
            this.state.tempVariables.registrationStatus = await response.json();
            window.hidePageLoading();
        },
        async getBookingList({ commit }) {
            let mainloader; if (!this.state.bookingList.length) { mainloader = true; window.showPageLoading(); } else { mainloader = false; window.showPartPageLoading(); }
            let response = await fetch(
                this.state.apiRootUrl + '/Guest/Booking/GetBookingList/' + this.state.language, {
                method: 'GET',
                headers: {
                    'Authorization': 'Bearer ' + this.state.user.Token,
                    'Content-type': 'application/json',
                }
            });
            let result = await response.json();
            commit('setBookingList', result);
            if (mainloader) { window.hidePageLoading(); } else {window.hidePartPageLoading(); }
        },
        async getLightFavoriteHotelList({ commit }) {
            let mainloader; if (!this.state.lightFavoriteHotelList.length) { mainloader = true; window.showPageLoading(); } else { mainloader = false; window.showPartPageLoading(); }
            let response = await fetch(
                this.state.apiRootUrl + '/Guest/GetLightFavoriteList', {
                method: 'GET',
                headers: {
                    'Authorization': 'Bearer ' + this.state.user.Token,
                    'Content-type': 'application/json',
                }
            });
            let result = await response.json();
            commit('setLightFavoriteHotelList', result);
            if (mainloader) { window.hidePageLoading(); } else { window.hidePartPageLoading(); }
        },
        async getFavoriteHotelList({ commit }) {
            let mainloader; if (!this.state.favoriteHotelList.length) { mainloader = true; window.showPageLoading(); } else { mainloader = false; window.showPartPageLoading(); }

            //Load Updated Light Favorites for hearts
            await this.dispatch("getLightFavoriteHotelList");

            let response = await fetch(
                this.state.apiRootUrl + '/Guest/GetFavoriteList/' + this.state.language, {
                method: 'GET',
                headers: {
                    'Authorization': 'Bearer ' + this.state.user.Token,
                    'Content-type': 'application/json',
                }
            });
            let result = await response.json();
            commit('setFavoriteHotelList', result.hotelList);
            if (mainloader) { window.hidePageLoading(); } else {window.hidePartPageLoading(); }
        },
        async getTopFiveList({ commit }, type) {
            let mainloader; if (!this.state.topFiveList.length) { mainloader = true; window.showPageLoading(); } else { mainloader = false; window.showPartPageLoading(); }
            let response;
            if (this.state.user.loggedIn == true) {
                response = await fetch(
                    this.state.apiRootUrl + '/Guest/GetAuthTopFiveList/' + type + '/' + this.state.userSettings.topFiveCount + '/' + this.state.language, {
                        method: 'GET', headers: { 'Authorization': 'Bearer ' + this.state.user.Token, 'Content-type': 'application/json' }
                });
            } else {
                response = await fetch(
                    this.state.apiRootUrl + '/Guest/GetTopFiveList/' + type + '/' + this.state.language, {
                    method: 'GET', headers: { 'Content-type': 'application/json' }
                });
            }
            let result = await response.json();
            commit('setTopFiveList', result.hotelList);
            if (mainloader) { window.hidePageLoading(); } else {window.hidePartPageLoading(); }
        },

        async setAdvertiserShown({ commit }, reservationId) {
            let response = await fetch(
                this.state.apiRootUrl + '/Advertiser/SetAdvertiserShown/' + reservationId, {
                method: 'GET',
                headers: {
                    'Authorization': 'Bearer ' + this.state.user.Token,
                    'Content-type': 'application/json',
                }
            });
            let result = await response.json();
        },

        async setReviewShown({ commit }, reservationId) {
            let response = await fetch(
                this.state.apiRootUrl + '/Advertiser/SetReviewShown/' + reservationId, {
                method: 'GET',
                headers: {
                    'Authorization': 'Bearer ' + this.state.user.Token,
                    'Content-type': 'application/json',
                }
            });
            let result = await response.json();
        },
        async setGuestShown({ commit }, reservationId) {
            let response = await fetch(
                this.state.apiRootUrl + '/Guest/SetGuestShown/' + reservationId, {
                method: 'GET',
                headers: {
                    'Authorization': 'Bearer ' + this.state.user.Token,
                    'Content-type': 'application/json',
                }
            });
            let result = await response.json();
        },

        async getWebSettings({ commit }) {
            let response = await fetch(
                this.state.apiRootUrl + '/WebPages/GetSettingList', {
                method: 'GET',
                headers: { 'Content-type': 'application/json', }
            });
            let result = await response.json();
            if (result.Status == "error") {
                var notify = Metro.notify; notify.setup({ width: 300, timeout: this.state.userSettings.notifyShowTime, duration: 500 });
                notify.create(result.ErrorMessage, "Error", { cls: "alert" }); notify.reset();
            } else {
                Metro.storage.setItem('WebSettings', result);
                result.forEach(setting => {
                    switch (setting.key) {
                        case "NotifyShowTime": Metro.storage.setItem('NotifyShowTime', setting.value);
                            this.state.userSettings.notifyShowTime = setting.value;
                            break;
                        case "AutomaticDetectedLanguageTranslate": Metro.storage.setItem('AutomaticDetectedLanguageTranslate', JSON.parse(setting.value.toLowerCase())); break;
                        case "BackgroundOpacitySetting": Metro.storage.setItem('BackgroundOpacitySetting', setting.value); break;
                        case "BackgroundColorSetting": Metro.storage.setItem('BackgroundColorSetting', setting.value); break;
                        case "BackgroundVideoSetting": Metro.storage.setItem('BackgroundVideoSetting', setting.value); break;
                        case "BackgroundImageSetting": Metro.storage.setItem('BackgroundImageSetting', setting.value); break;
                        case "BackgroundColor": Metro.storage.setItem('BackgroundColor', setting.value); break;
                        case "InputBanner": Metro.storage.setItem('InputBanner', setting.value); break;
                        case "ReviewInsertDaysLimit": Metro.storage.setItem('ReviewInsertDaysLimit', setting.value); break;
                        case "BookCalendarViewMonthsBefore": Metro.storage.setItem('BookCalendarViewMonthsBefore', setting.value); break;
                        case "BookCalendarViewMonthsAfter": Metro.storage.setItem('BookCalendarViewMonthsAfter', setting.value); break;
                            
                    }
                });
                ApplyLoadedWebSetting();
            }
        },

        async updateRegistration({ commit }, updatedUser) {
            window.showPageLoading();
            let response = await fetch(this.state.apiRootUrl + '/Guest/UpdateRegistration', {
                method: 'POST',
                headers: {
                    "Authorization": 'Bearer ' + this.state.user.Token,
                    "Content-type": "application/json",
                },
                 body: JSON.stringify({ User: updatedUser, Language: this.state.language }),
            });

            let result = await response.json();
            if (result.Status != "error") {
                updatedUser.loggedIn = true;
                updatedUser.Token = this.state.user.Token;
                this.state.user = updatedUser;

                var notify = Metro.notify; notify.setup({ width: 300, timeout: this.state.userSettings.notifyShowTime, duration: 500 });
                notify.create(window.dictionary("messages.dataSaved"), "Success", { cls: "success" }); notify.reset();
            }
            else {
                var notify = Metro.notify; notify.setup({ width: 300, timeout: this.state.userSettings.notifyShowTime, duration: 500 });
                notify.create(result.ErrorMessage, "Error", { cls: "alert" }); notify.reset();
            }

            window.hidePageLoading();
        },

        async deleteRegistration({ commit }, Id) {
            window.showPageLoading();
            await fetch(this.state.apiRootUrl + '/Guest/DeleteRegistration', {
                method: 'DELETE',
                headers: {
                    "Authorization": 'Bearer ' + this.state.user.Token,
                    "Content-type": "application/json",
                },

            });
            commit('logOutUser');
            window.hidePageLoading();
            router.push('/');
        },
        async getGuestSettingList({ commit }) {
            window.showPageLoading();
            let response = await fetch(
                this.state.apiRootUrl + '/Guest/GetGuestSettingList/' + this.state.user.Id, {
                method: 'GET',
                    headers: {
                        "Authorization": 'Bearer ' + this.state.user.Token,
                        'Content-type': 'application/json'
                    }
            });
            let result = await response.json();
            if (result.Status == undefined) {
                result.forEach(setting => {
                    switch (setting.key) {
                        case "topFiveCount":
                            this.state.userSettings.topFiveCount = setting.value;
                            break;
                        case "notifyShowTime":
                            this.state.userSettings.notifyShowTime = setting.value;
                            break;
                        case "showInactiveAdvertisementAsDefault":
                            this.state.userSettings.showInactiveAdvertisementAsDefault = setting.value;
                            break;
                        case "translationLanguage":
                            this.state.userSettings.translationLanguage = setting.value;
                            break;
                        case "hideSearchingInPrivateZone":
                            this.state.userSettings.hideSearchingInPrivateZone = setting.value;
                            break;
                            
                    }
                });
            }
            window.hidePageLoading();
        },
        async updateGuestSetting({ commit }, userSettings) {
            window.showPageLoading();
            let response = await fetch(this.state.apiRootUrl + '/Guest/SetGuestSettingList', {
                method: 'POST',
                headers: {
                    "Authorization": 'Bearer ' + this.state.user.Token,
                    'Content-type': 'application/json'
                },
                body: JSON.stringify({ Settings: userSettings, Language: this.state.language })
            });
            let result = await response.json();

            if (result.Status == "error") {
                var notify = Metro.notify; notify.setup({ width: 300, timeout: this.state.userSettings.notifyShowTime, duration: 500 });
                notify.create(result.ErrorMessage, "Error", { cls: "alert" }); notify.reset();
            } else {
                userSettings.forEach(setting => {
                    switch (setting.Key) {
                        case "topFiveCount":
                            this.state.userSettings.topFiveCount = setting.Value;
                            break;
                        case "notifyShowTime":
                            this.state.userSettings.notifyShowTime = setting.Value;
                            break;
                        case "showInactiveAdvertisementAsDefault":
                            this.state.userSettings.showInactiveAdvertisementAsDefault = setting.Value;
                            break;
                        case "translationLanguage":
                            this.state.userSettings.translationLanguage = setting.Value;
                            break;
                        case "hideSearchingInPrivateZone":
                            this.state.userSettings.hideSearchingInPrivateZone = setting.Value;
                            break;
                            
                    }
                });
            }
            window.hidePageLoading();
        },
    },
})

export default store
