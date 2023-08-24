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
        favoriteList: [],
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
        setFavoriteHotelList(store, value) {
            store.favoriteHotelList = value;
        },
        setFavoriteList(store, value) {
            store.favoriteList = value;
        },
        setReservedRoomList(store, value) {
            store.reservedRoomList = value;
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
            state.user = data
            state.user.loggedIn = true
        },
        logOutUser(state) {
            state.user = {
                loggedIn: false,
            }
        },
    },
    actions: {
        setDates({ commit }, date) {
            commit('setDates', date);
        },
        setFavoriteList({ commit }, value) {
            commit('setFavoriteList', value);
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
            window.showPageLoading();
            if (searchString === null || searchString == '') { searchString = "null"; }

            var response = await fetch(
                this.state.apiRootUrl + '/Search/GetSearchInput/' + searchString + '/' + this.state.language, {
                method: 'GET',
                headers: {
                    'Content-type': 'application/json'
                }
            });
            var result = await response.json();
            console.log("hotellist", result);

            commit('setHotelSearchResultsList', result.hotelList);
            window.hidePageLoading();

            if (result) {
                router.push({ name: 'result' });
            }
            
        },
        async getReservedRoomList({ commit }, hotelId) {
            window.showPageLoading();
            let startDate; let endDate;
            if (this.state.searchString.dates.length) {
                startDate = this.state.searchString.dates[0].toLocaleDateString('sv-SE');
                endDate = this.state.searchString.dates[1].toLocaleDateString('sv-SE');
            }

            var response = await fetch(
                this.state.apiRootUrl + '/ReservedRoomList/' + hotelId + '/' + startDate + '/' + endDate + '/' + this.state.language, {
                method: 'GET',
                headers: {
                    'Content-type': 'application/json'
                }
            });
            var result = await response.json();
            commit('setReservedRoomList', result);
            window.hidePageLoading();
        },
        async getTopList({ commit }) {
            window.showPageLoading();
            var response = await fetch(
                this.state.apiRootUrl + '/Top/GetTopList/' + this.state.language, {
                method: 'GET',
                headers: {
                    'Content-type': 'application/json'
                }
            });
            var result = await response.json();
            commit('setTopList', result);
            window.hidePageLoading();

        },
        async getPropertyList({ commit }) {
            window.showPageLoading();
            var response = await fetch(
                this.state.apiRootUrl + '/Properties/' + this.state.language, {
                method: 'GET',
                headers: {
                    'Content-type': 'application/json'
                }
            });
            var result = await response.json();
            //console.log("Property", result);
            commit('setPropertyList', result);
            window.hidePageLoading();
        },
        async getPropertyGroupList({ commit }) {
            window.showPageLoading();
            var response = await fetch(
                this.state.apiRootUrl + '/Properties/GetPropertyGroupList/' + this.state.language, {
                method: 'GET',
                headers: {
                    'Content-type': 'application/json'
                }
            });
            var result = await response.json();
            console.log("PropertyGroup", result);
            commit('setPropertyGroupList', result);
            window.hidePageLoading();
        },
        async getAdvertisementList({ commit }) {
            window.showPageLoading();
            var response = await fetch(
                this.state.apiRootUrl + '/Advertiser/GetAdvertisementList/' + this.state.language, {
                method: 'GET',
                headers: {
                    'Authorization': 'Bearer ' + this.state.user.Token,
                    'Content-type': 'application/json',
                }
            });
            var result = await response.json();
            console.log("setAdvertisementList", result);
            commit('setAdvertisementList', result);
            window.hidePageLoading();
        },

        async getRoomTypeList({ commit }) {
            window.showPageLoading();
            var response = await fetch(
                this.state.apiRootUrl + '/RoomTypes/' + this.state.language, {
                method: 'GET',
                headers: {
                    'Content-type': 'application/json'
                }
            });
            var result = await response.json();
            commit('setRoomTypeList', result);
            window.hidePageLoading();
        },
        async getUbytkacInfoList({ commit }) {
            window.showPageLoading();
            var response = await fetch(
                this.state.apiRootUrl + '/UbytkacInfo/' + this.state.language, {
                method: 'GET',
                headers: {
                    'Content-type': 'application/json'
                }
            });
            var result = await response.json();
            commit('setUbytkacInfoList', result);
            window.hidePageLoading();
        },
        async getRegistrationInfoList({ commit }) {
            window.showPageLoading();
            var response = await fetch(
                this.state.apiRootUrl + '/RegistrationInfo/' + this.state.language, {
                method: 'GET',
                headers: {
                    'Content-type': 'application/json'
                }
            });
            var result = await response.json();
            commit('setRegistrationInfoList', result);
            window.hidePageLoading();
        },
        async getOftenQuestionList({ commit }) {
            window.showPageLoading();
            var response = await fetch(
                this.state.apiRootUrl + '/OftenQuestion/' + this.state.language, {
                method: 'GET',
                headers: {
                    'Content-type': 'application/json'
                }
            });
            var result = await response.json();
            commit('setOftenQuestionList', result);
            window.hidePageLoading();
        },
        async getHolidayTipsList({ commit }) {
            window.showPageLoading();
            var response = await fetch(
                this.state.apiRootUrl + '/HolidayTips/' + this.state.language, {
                method: 'GET',
                headers: {
                    'Content-type': 'application/json'
                }
            });
            var result = await response.json();
            commit('setHolidayTipsList', result);
            window.hidePageLoading();
        },
        async getSearchDialList({ commit }) {
            window.showPageLoading();
            var response = await fetch(
                this.state.apiRootUrl + '/Search/GetSearchDialList/' + this.state.language, {
                method: 'GET',
                headers: {
                    'Content-type': 'application/json'
                }
            });
            var result = await response.json();
            commit('setSearchDialList', result);
            window.hidePageLoading();
        },
        async getSearchAreaList({ commit }) {
            window.showPageLoading();
            var response = await fetch(
                this.state.apiRootUrl + '/Search/GetSearchAreaList/' + this.state.language, {
                method: 'GET',
                headers: {
                    'Content-type': 'application/json'
                }
            });
            var result = await response.json();
            commit('setSearchAreaList', result);
            window.hidePageLoading();
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
                    this.state.toastErrorMessage = result.message;
                } else {
                    commit('setUser', result);

                    //Load Advertisement if PowerUser
                    if (this.state.user.UserId != '') {
                        await this.dispatch("getAdvertisementList");
                    }

                    //Load Favorites
                    await this.dispatch("getFavoriteHotelList");
                    

                    //path after login
                    router.push('/profile');
                }

            } else { // remove login on Refresh page
                window.hidePageLoading();
                commit('logOutUser');
                router.push('/')
            }
            window.hidePageLoading();
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
            window.showPageLoading();
            var response = await fetch(
                this.state.apiRootUrl + '/Guest/Booking/GetBookingList/' + this.state.language, {
                method: 'GET',
                headers: {
                    'Authorization': 'Bearer ' + this.state.user.Token,
                    'Content-type': 'application/json',
                }
            });
            var result = await response.json();
            commit('setBookingList', result);
            window.hidePageLoading();
        },
        async getFavoriteHotelList({ commit }) {
            window.showPageLoading();
            var response = await fetch(
                this.state.apiRootUrl + '/Guest/GetFavoriteList/' + this.state.language, {
                method: 'GET',
                headers: {
                    'Authorization': 'Bearer ' + this.state.user.Token,
                    'Content-type': 'application/json',
                }
            });
            var result = await response.json();
            commit('setFavoriteHotelList', result.hotelList);
            window.hidePageLoading();
        },
        async getTopFiveList({ commit }, type) {
            window.showPageLoading();
            var response = await fetch(
                this.state.apiRootUrl + '/Guest/GetTopFiveList/' + type + '/' + this.state.language, {
                method: 'GET',
                headers: {
                    'Authorization': 'Bearer ' + this.state.user.Token,
                    'Content-type': 'application/json',
                }
            });
            var result = await response.json();
            commit('setTopFiveList', result.hotelList);
            window.hidePageLoading();
        },
        //TODO 
        async getReviews({ commit }, hotelId) {
            //window.showPageLoading();
            var response = await fetch(
                this.state.apiRootUrl + '/Hotel/GetReviews/' + hotelId
            )
            var result = await response.json();
            commit('getReviews', result);
            //window.hidePageLoading();
        },

        deleteRegistration({ commit }, Id) {
            window.showPageLoading();
            fetch(this.state.apiRootUrl + '/Guest/DeleteRegistration', {
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
