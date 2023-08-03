import { createStore } from 'vuex';
import router from '../router/index';
import { encode } from "base-64";
//import AppVue from '../App.vue';

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


        //guestId: 33, // hard coded
        //home: { title: 'store name' },
        //name: 'Vue',
        //addReview: {
        //  name: '',
        //  email: '',
        //  message: '',
        //},
        //getReviews: [],
        //savedHotel: [],
        //bookedHotels: [],
        //reservation: {
        //  fullName: 'sadas',
        //  hotelName: 'sds',
        //  startDate: '',
        //  endDate: '',
        //  adults: '',
        //  children: '',
        //  customerMessage: '',
        //  type: '',
        //  totalPrice: '',
        //  extraBed: null,
        //  hotelRoomsViewModel: {
        //    singleRooms: '2',
        //    doubleRooms: '2',
        //    familyRooms: '2',
        //  },
        //},
        //orderId: '',
    },
    mutations: {
        setBookingList(store, value) {
            store.bookingList = value;
            console.log("bookingList", store.bookingList);
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


        //  setEmail(store, value) {
        //  store.addReview.email = value
        //  },
        //setMessage(store, value) {
        //  store.addReview.message = value
        //},
        //setReservationDetails(state, data) {
        //  state.reservation = data
        //},
        //getReviews(state, data) {
        //  state.getReviews = data
        //},

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

    //setEmail(state, value) {
    //  state.user.Email = value
    //},
    //setPassword(state, value) {
    //  state.user.Password = value
    //},
    //setBookedHotels(state, value) {
    //  state.bookedHotels = value
    //},
    //setOrderId(state, value) {
    //  state.orderId = value
    //}
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
            console.log("Property", result);
            commit('setPropertyList', result)
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


        //async searchHotelByName({ commit }, searchString) {
        //    var response = await fetch(
        //        this.state.apiRootUrl + '/Search/search?input=' + searchString
        //    ) // Default is GET
        //    var result = await response.json()
        //    if (result) {
        //        commit('setHotelSearchResultsList', result)
        //        router.push({ name: 'result' })
        //    }
        //},
        //async getHotelById({ commit }, hotelId) {
        //    var response = await fetch(
        //        this.state.apiRootUrl + '/Hotel/GetById/' + hotelId
        //    ) // Default is GET
        //    var result = await response.json()
        //    commit('setHotel', result)
        //},
        async setHotel({ commit }, hotel) {
            commit('setHotel', hotel)
        },
        //async getReservationById({ commit }, reservationId) {
        //    var response = await fetch(
        //        this.state.apiRootUrl + '/Booking/' + reservationId
        //    )
        //    var result = await response.json()
        //    commit('setReservationDetails', result)
        //},
        //async searchHotelByCity({ commit }, searchString) {
        //    var response = await fetch(
        //        this.state.apiRootUrl + '/Search/GetHotelByCity?input=' +
        //        searchString
        //    ) // Default is GET
        //    var result = await response.json()
        //    if (result) {
        //        commit('setHotelSearchResultsList', result)
        //        router.push({ name: 'result' })
        //    }
        //},
        //async searchSpecific({ commit }, payload) {
        //    window.scrollTo(0, 0)
        //    commit('setSearchString', payload.searchString)
        //    commit('setSearchButtonLoadingTrue', null)
        //    setTimeout(
        //        function (that) {
        //            // Timeout resolves inconsistent scroll behaviour between scrollTo and router.push
        //            if (payload.type === 'city') {
        //                that.dispatch('searchHotelByCity', payload.searchString)
        //            } else {
        //                that.dispatch('searchHotelByName', payload.searchString)
        //            }
        //        },
        //        500,
        //        this
        //    )
        //},

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



    //async getBookings({ commit }) {
    //  var response = await fetch(
    //    this.state.apiRootUrl + '/Booking/guest/' + this.state.user.id
    //  )
    //  var result = await response.json()
    //  commit('setBookedHotels', result)
    //},
    //async setOrderId({ commit }, value) {
    //  commit('setOrderId', value)
    //},

    //setHotelId({ commit }, value) {
    //  commit('setHotelId', value)
    //},
    },
})

export default store
