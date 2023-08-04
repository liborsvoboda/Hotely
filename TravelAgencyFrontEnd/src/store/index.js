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
                firstName : null,
                lastName : null,
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
        setPropertyGroupList(store, value) {
            store.propertyGroupList = value;
        },
        setBookingList(store, value) {
            store.bookingList = value;
        },
        setHotel(store, value) {
            store.hotel = value
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
        async searchHotels({ commit }, searchString) {
            if (searchString === null || searchString == '') { searchString = "null"; }

            var response = await fetch(
                this.state.apiRootUrl + '/Search/GetSearchInput/' + searchString + '/' + this.state.language, {
                method: 'get',
                headers: {
                    'Content-type': 'application/json'
                }
            });
            var result = await response.json();
            console.log("hotellist", result);

            commit('setHotelSearchResultsList', result.hotelList)
            if (result) {
                router.push({ name: 'result' })
            }
        },
        async getReservedRoomList({ commit }, hotelId) {
            let startDate; let endDate;
            if (this.state.searchString.dates.length) {
                startDate = this.state.searchString.dates[0].toLocaleDateString('sv-SE');
                endDate = this.state.searchString.dates[1].toLocaleDateString('sv-SE');
            }

            var response = await fetch(
                this.state.apiRootUrl + '/ReservedRoomList/' + hotelId + '/' + startDate + '/' + endDate + '/' + this.state.language, {
                method: 'get',
                headers: {
                    'Content-type': 'application/json'
                }
            });
            var result = await response.json();
            commit('setReservedRoomList', result)
        },
        async getTopList({ commit }) {
            var response = await fetch(
                this.state.apiRootUrl + '/Top/GetTopList/' + this.state.language, {
                method: 'get',
                headers: {
                    'Content-type': 'application/json'
                }
            });
            var result = await response.json();
            commit('setTopList', result)
        },
        async getPropertyList({ commit }) {
            var response = await fetch(
                this.state.apiRootUrl + '/Properties/' + this.state.language, {
                method: 'get',
                headers: {
                    'Content-type': 'application/json'
                }
            });
            var result = await response.json();
            //console.log("Property", result);
            commit('setPropertyList', result)
        },
        async getPropertyGroupList({ commit }) {
            var response = await fetch(
                this.state.apiRootUrl + '/Properties/GetPropertyGroupList/' + this.state.language, {
                method: 'get',
                headers: {
                    'Content-type': 'application/json'
                }
            });
            var result = await response.json();
            console.log("PropertyGroup", result);
            commit('setPropertyGroupList', result)
        },
        
        async getRoomTypeList({ commit }) {
            var response = await fetch(
                this.state.apiRootUrl + '/RoomTypes/' + this.state.language, {
                method: 'get',
                headers: {
                    'Content-type': 'application/json'
                }
            });
            var result = await response.json();
            commit('setRoomTypeList', result)
        },
        async getUbytkacInfoList({ commit }) {
            var response = await fetch(
                this.state.apiRootUrl + '/UbytkacInfo/' + this.state.language, {
                method: 'get',
                headers: {
                    'Content-type': 'application/json'
                }
            });
            var result = await response.json();
            commit('setUbytkacInfoList', result)
        },
        async getRegistrationInfoList({ commit }) {
            var response = await fetch(
                this.state.apiRootUrl + '/RegistrationInfo/' + this.state.language, {
                method: 'get',
                headers: {
                    'Content-type': 'application/json'
                }
            });
            var result = await response.json();
            commit('setRegistrationInfoList', result)
        },
        async getOftenQuestionList({ commit }) {
            var response = await fetch(
                this.state.apiRootUrl + '/OftenQuestion/' + this.state.language, {
                method: 'get',
                headers: {
                    'Content-type': 'application/json'
                }
            });
            var result = await response.json();
            commit('setOftenQuestionList', result)
        },
        async getHolidayTipsList({ commit }) {
            var response = await fetch(
                this.state.apiRootUrl + '/HolidayTips/' + this.state.language, {
                method: 'get',
                headers: {
                    'Content-type': 'application/json'
                }
            });
            var result = await response.json();
            commit('setHolidayTipsList', result)
        },
        async setHotel({ commit }, hotel) {
            commit('setHotel', hotel)
        },

        async getSearchDialList({ commit }) {
            var response = await fetch(
                this.state.apiRootUrl + '/Search/GetSearchDialList/' + this.state.language, {
                method: 'get',
                headers: {
                    'Content-type': 'application/json'
                }
            });
            var result = await response.json()
            commit('setSearchDialList', result)
        },
        async getSearchAreaList({ commit }) {
            var response = await fetch(
                this.state.apiRootUrl + '/Search/GetSearchAreaList/' + this.state.language, {
                method: 'get',
                headers: {
                    'Content-type': 'application/json'
                }
            });
            var result = await response.json()
            commit('setSearchAreaList', result)
        },
        async login({ commit }, credentials) {
            if (credentials.Email) {
                let response = await fetch(this.state.apiRootUrl + '/Guest/WebLogin', {
                    method: 'post',
                    headers: {
                        'Authorization': 'Basic ' + encode(credentials.Email + ":" + credentials.Password),
                        'Content-type': 'application/json'
                    },
                    body: JSON.stringify({ language: this.state.language })
                });

                let result = await response.json()

                if (result.message) {
                    this.state.toastErrorMessage = result.message;
                } else {
                    commit('setUser', result)

                    //Load Favorites
                    response = await fetch(this.state.apiRootUrl + '/Guest/GetFavoriteList', {
                        method: 'GET',
                        headers: {
                            'Authorization': 'Bearer ' + this.state.user.Token,
                            'Content-type': 'application/json',
                        }
                    });
                    result = await response.json();
                    commit('setFavoriteList', result);
                    router.push('/profile');
                }

            } else { // remove login on Refresh page
                commit('logOutUser')
                router.push('/')
            }
        },

        async registration({ commit }, regInfo) {
            let response = await fetch(this.state.apiRootUrl + '/Guest/WebRegistration', {
                method: 'post',
                headers: {
                    'Content-type': 'application/json'
                },
                body: JSON.stringify({ User: regInfo, Language: this.state.language })
            });
            this.state.tempVariables.registrationStatus = await response.json();
        },
        setBookedTotalPrice({ commit }) {
            commit('setBookedTotalPrice');
        },
        async logout({ commit }) {
            commit('logOutUser')
            router.push('/')
        },
        async getBookingList({ commit }) {

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
        },
        async getFavoriteHotelList({ commit }) {

            var response = await fetch(
                this.state.apiRootUrl + '/Guest/GetFavoriteList/' + this.state.language, {
                method: 'GET',
                headers: {
                    'Authorization': 'Bearer ' + this.state.user.Token,
                    'Content-type': 'application/json',
                }
            });
            var result = await response.json()
            commit('setFavoriteHotelList', result.hotelList)
        },
        async getTopFiveList({ commit }, type) {

            var response = await fetch(
                this.state.apiRootUrl + '/Guest/GetTopFiveList/' + type +'/' + this.state.language, {
                method: 'GET',
                headers: {
                    'Authorization': 'Bearer ' + this.state.user.Token,
                    'Content-type': 'application/json',
                }
            });
            var result = await response.json()
            commit('setTopFiveList', result.hotelList)
        },
        async getReviews({ commit }, hotelId) {
          var response = await fetch(
            this.state.apiRootUrl + '/Hotel/GetReviews/' + hotelId
          )
          var result = await response.json()
          commit('getReviews', result)
        },

        deleteRegistration({ commit }, Id) {
            fetch(this.state.apiRootUrl + '/Guest/DeleteRegistration', {
                method: 'DELETE',
                headers: {
                    "Authorization": 'Bearer ' + this.state.user.Token,
                    "Content-type": "application/json",
                },

            });
            commit('logOutUser');
            router.push('/');
        },
        setDates({ commit }, date) {
            commit('setDates', date);
        },
        setFavoriteList({ commit }, value) {
            commit('setFavoriteList', value);
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
    },
})

export default store
