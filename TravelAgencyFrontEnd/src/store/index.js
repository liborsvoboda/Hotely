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
            registrationStatus: null
        },

        system: {
            passwordMin: 6,
        },

        userSettings: {
            notifyShowTime: 1000,
            showInactiveAdvertisementAsDefault: false,
            translationLanguage: 'cz'
        },

        apiRootUrl: 'http://localhost:5000/WebApi',
        language: 'cz',
        hotel: [],
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
        setAdvertisementList(store, value) {
            store.advertisementList = value;
            console.log("setAdvertisementList", store.advertisementList);
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
        },
        setPropertyList(store, value) {
            store.propertyList = value;
            console.log("Set PropertyList", store.propertyList);
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
        setTopList(store, value) {
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
        setDates(state, date) {
            state.searchString.dates = date;
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
        },
    },
    actions: {
        setDates({ commit }, date) {
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
                router.push({ name: 'result' });
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
        async getTopList({ commit }) {
            console.log("topList", this.state.searchResults);
        
            let mainloader; if (!this.state.searchResults == [] || !this.state.searchResults.hotelList.length) { mainloader = true; window.showPageLoading(); } else { mainloader = false; window.showPartPageLoading(); }
            let response = await fetch(
                this.state.apiRootUrl + '/Top/GetTopList/' + this.state.language, {
                method: 'GET',
                headers: {
                    'Content-type': 'application/json'
                }
            });
            let result = await response.json();
            commit('setTopList', result);
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
            let mainloader; if (!this.state.advertisementList.length) { mainloader = true; window.showPageLoading(); } else { mainloader = false; window.showPartPageLoading(); }

            let response = await fetch(
                this.state.apiRootUrl + '/Advertiser/GetAdvertisementList/' + this.state.language, {
                method: 'GET',
                headers: {
                    'Authorization': 'Bearer ' + this.state.user.Token,
                    'Content-type': 'application/json',
                }
            });
            let result = await response.json();
            console.log("setAdvertisementList", result);
            commit('setAdvertisementList', result);
            if (mainloader) { if (mainloader) { window.hidePageLoading(); } else {window.hidePartPageLoading(); } } else {window.hidePartPageLoading(); }
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
                    var notify = Metro.notify; notify.setup({ width: 300, duration: this.state.userSettings.notifyShowTime });
                    notify.create(result.message, "Error", { cls: "alert" }); notify.reset();

                } else {
                    commit('setUser', result);

                    //Load Light Favorites for hearts
                    await this.dispatch("getLightFavoriteHotelList");


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
            let response = await fetch(
                this.state.apiRootUrl + '/Guest/GetTopFiveList/' + type + '/' + this.state.language, {
                method: 'GET',
                headers: {
                    'Authorization': 'Bearer ' + this.state.user.Token,
                    'Content-type': 'application/json',
                }
            });
            let result = await response.json();
            commit('setTopFiveList', result.hotelList);
            if (mainloader) { window.hidePageLoading(); } else {window.hidePartPageLoading(); }
        },
        //TODO 
        async getReviews({ commit }, hotelId) {
            //let mainloader; if (!this.state.topFiveList.length) { mainloader = true; window.showPageLoading(); } else { mainloader = false; window.showPartPageLoading(); }
            let response = await fetch(
                this.state.apiRootUrl + '/Hotel/GetReviews/' + hotelId
            )
            let result = await response.json();
            commit('getReviews', result);
            //if (mainloader) { window.hidePageLoading(); } else {window.hidePartPageLoading(); }
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

                var notify = Metro.notify; notify.setup({ width: 300, duration: this.state.userSettings.notifyShowTime });
                notify.create(window.dictionary("messages.dataSaved"), "Success", { cls: "success" }); notify.reset();
            }
            else {
                var notify = Metro.notify; notify.setup({ width: 300, duration: this.state.userSettings.notifyShowTime });
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
        
    },
})

export default store
