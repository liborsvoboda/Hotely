import { createRouter, createWebHistory } from "vue-router";
import store from "../store/index"

import Info from "../components/pages/HotelViewComponents/Info.vue";
import HotelView from "/src/components/pages/HotelView.vue";
import Rooms from "../components/pages/HotelViewComponents/Rooms.vue";
import Photos from "../components/pages/HotelViewComponents/Photos.vue";
import PhotoGallery from "../components/pages/HotelViewComponents/RoomsViewComponents/PhotoGallery.vue";
import Reviews from "../components/pages/HotelViewComponents/Reviews.vue";
import SearchResult from "../components/pages/SearchResult.vue";
import AddReview from "../components/pages/HotelViewComponents/ReviewComponents/AddReview.vue";
import CheckoutView from "../components/pages/CheckoutView.vue";
import { registerRuntimeCompiler } from "@vue/runtime-core";
import Home from "/src/components/pages/Home.vue";
import CustomerDetails from '/src/components/pages/CheckoutViewComponents/CustomerDetails.vue'
import OrderDetails from '/src/components/pages/CheckoutViewComponents/OrderDetails.vue'
import OrderConfirmed from '/src/components/pages/CheckoutViewComponents/OrderConfirmed.vue'
import ProfileMain from "../components/pages/CustomerProfile/ProfileMain.vue";
import Login from "../components/pages/Login/Login.vue"
import Registration from "../components/pages/Login/Registration.vue";
import Forgot from "../components/pages/Login/Forgot.vue";
import TopFive from '../components/pages/CustomerProfile/ProfilePage.vue'
import Bookings from '../components/pages/CustomerProfile/Bookings.vue'
import FavoriteHotelList from '/src/components/pages/CustomerProfile/FavoriteHotelList.vue';
import ProfileSetting from '../components/pages/CustomerProfile/ProfileSetting.vue'
import Contact from '../components/pages/Extra pages/Contact.vue';
import About from '../components/pages/Extra pages/About.vue';
import UbytkacInfo from '../components/pages/Extra pages/UbytkacInfo.vue';
import RegistrationInfo from '../components/pages/Extra pages/RegistrationInfo.vue';
import OftenQuestion from '../components/pages/Extra pages/OftenQuestion.vue';
import HolidayTips from '../components/pages/Extra pages/HolidayTips.vue';
import Advertisement from '../components/pages/Advertiser/Advertisement.vue';
import AdvertisementWizard from '../components/pages/Advertiser/AdvertisementWizard.vue';

const routes = [
    {
        path: "/",
        name: "Home",
        component: Home,
        meta: {
            requiresAuth: false, title: "Home"
        },
    },
    {
        path: "/profile",
        name: "profile",
        component: ProfileMain,
        meta: {
            requiresAuth: true, title: "Advertisement News"
        },
        children: [
            {
                path: "",
                name: "TopFive",
                component: TopFive,
                meta: {
                    requiresAuth: false, title: "Top Five"
                },
            },
            {
                path: "bookings",
                name: "Bookings",
                component: Bookings,
                meta: {
                    requiresAuth: true, title: "Bookings"
                },
            },
            {
                path: "favorite",
                name: "favorite",
                component: FavoriteHotelList,
                meta: {
                    requiresAuth: true, title: "Favorite List"
                },
            },
            {
                path: "profileSetting",
                name: "profileSetting",
                component: ProfileSetting,
                meta: {
                    requiresAuth: true, title: "Profile Setting"
                },
            },
            {
                path: "advertisement",
                name: "advertisement",
                component: Advertisement,
                meta: {
                    requiresAuth: true, title: "Advertisement"
                },
            },
            {
                path: "advertisementWizard",
                name: "advertisementWizard",
                component: AdvertisementWizard,
                meta: {
                    requiresAuth: true, title: "Advertisement Wizard"
                },
            },
            
        ],
    },
    {
        path: "/result",
        name: "result",
        component: SearchResult,
        meta: {
            requiresAuth: false, title: "Search Result"
        },
    },
    {
        path: "/login",
        name: "login",
        component: Login,
        meta: {
            requiresAuth: false, title: "Login"
        },
    },
    {
        path: "/registration",
        name: "registration",
        component: Registration,
        meta: {
            requiresAuth: false, title: "Registration"
        },
    },
    {
        path: "/forgot",
        name: "forgot",
        component: Forgot,
        meta: {
            requiresAuth: false, title: "Forgot"
        },
    },
    {
        path: "/Contact",
        name: "contact",
        component: Contact,
        meta: {
            requiresAuth: false, title: "Contact"
        },
    },
    {
        path: "/About",
        name: "about",
        component: About,
        meta: {
            requiresAuth: false, title: "About"
        },
    },
    {
        path: "/UbytkacInfo",
        name: "ubytkacInfo",
        component: UbytkacInfo,
        meta: {
            requiresAuth: false, title: "Ubytkac Info"
        },
    },
    {
        path: "/RegistrationInfo",
        name: "registrationInfo",
        component: RegistrationInfo,
        meta: {
            requiresAuth: false, title: "Registration Info"
        },
    },
    {
        path: "/OftenQuestion",
        name: "oftenQuestion",
        component: OftenQuestion,
        meta: {
            requiresAuth: false, title: "Often Question"
        },
    },
    {
        path: "/HolidayTips",
        name: "holidayTips",
        component: HolidayTips,
        meta: {
            requiresAuth: false, title: "Holiday Tips"
        },
    },

    // {
    //   path: "/addReview",
    //   name: "addReview",
    //   component: AddReview,
    // },
    {
        path: "/hotels/:id",
        name: "hotels",
        component: HotelView,
        meta: {
            requiresAuth: false, title: "Accommodation"
        },
        children: [
            {
                path: "",
                name: "Info",
                component: Info,
                meta: {
                    requiresAuth: false, title: "Accommodation"
                },
            },
            {
                path: "rooms",
                name: "hotels",
                component: Rooms,
                meta: {
                    requiresAuth: false, title: "Accommodation Units"
                },
            },
            {
                path: "Photos",
                name: "photos",
                component: PhotoGallery,
                meta: {
                    requiresAuth: false, title: "Photo Gallery"
                },
            },
            {
                path: "Reviews",
                name: "reviews",
                component: Reviews,
                meta: {
                    requiresAuth: true, title: "Reviews"
                },

            },
            {
                path: "AddReview",
                name: "addReview",
                component: AddReview,
                meta: {
                    requiresAuth: true, title: "New Review"
                },
            },
        ],
    },
    {
        path: "/checkout",
        name: "Checkout",
        component: CheckoutView,
        meta: {
            requiresAuth: false, title: "Accommodation Reservation"
        },
        children: [
            {
                path: "",
                name: "customerDetails",
                component: CustomerDetails,
                meta: {
                    requiresAuth: false, title: "Accommodation Reservation Customer"
                },

            },
            {
                path: "orderDetails",
                name: "OrderDetails",
                component: OrderDetails,
                meta: {
                    requiresAuth: false, title: "Accommodation Reservation Detail"
                },

            },
            {
                path: "orderConfirmed",
                name: "OrderConfirmed",
                component: OrderConfirmed,
                meta: {
                    requiresAuth: false, title: "Accommodation Reservation Confirm"
                },

            }
        ]
    },
];
const router = createRouter({
    history: createWebHistory(),
    routes,
});

//before change route
router.beforeEach((to, from, next) => {
    $("#SearchPanel").show();

    //check forgoten routes for unauthorized user
    if (!store.state.user.loggedIn && to.meta.requiresAuth) { router.push('/'); }

    if (to.fullPath.indexOf("/UbytkacInfo") > -1
        || to.fullPath.indexOf("/RegistrationInfo") > -1
        || to.fullPath.indexOf("/OftenQuestion") > -1
        || to.fullPath.indexOf("/HolidayTips") > -1
        || to.fullPath.indexOf("/Contact") > -1
    ) { $("#SearchPanel").hide(); }

    //autopage reset scroling without backroute from advertisement
    if (store.state.backRoute != to.fullPath
        && to.fullPath.indexOf("/hotels/") == -1 && to.fullPath.indexOf("/profile") == -1
        && to.fullPath.indexOf("/login") == -1 && to.fullPath.indexOf("/registration") == -1
        && to.fullPath.indexOf("/forgot") == -1 && to.fullPath.indexOf("/profile/advertisement") == -1 
    ) {
        window.scrollTo(0, 0);
    } else if (to.fullPath.indexOf("/profile/advertisement") > -1) {
        if (store.state.userSettings.hideSearchingInPrivateZone) { $("#SearchPanel").hide(); } else { $("#SearchPanel").show(); }
        window.scrollTo(0, window.scrollY);
    } else if (to.fullPath.indexOf("/hotels/") > -1) {
        window.scrollTo(0, 220);
    } else if (to.fullPath.indexOf("/profile") > -1 && to.fullPath.indexOf("/profile/advertisement") == -1) {
        //hide searching in private zone
        if (store.state.userSettings.hideSearchingInPrivateZone) {
            $("#SearchPanel").hide(); window.scrollTo(0, 0);
        } else { $("#SearchPanel").show(); window.scrollTo(0, 220); }
            
    } else if (to.fullPath.indexOf("/login") > -1 || to.fullPath.indexOf("/registration") > -1 || to.fullPath.indexOf("/forgot") > -1) {
        $("#SearchPanel").hide(); window.scrollTo(0, 0);
        //window.scrollTo(0, 220);
    } else if (store.state.backRoute == to.fullPath) {
        window.scrollTo(0, store.state.backRouteScroll);
    }

    document.title = `Ubytkac: ${to.meta.title}`;
    next();
});

//check translate after route
router.afterEach((to, from, next) => {

    $(document).ready(function () {
        try {
            if (store.state.userSettings.translationLanguage != "" && document.querySelector('#google_translate_element select') != null) {

                setTimeout(function () {
                    let selectElement = document.querySelector('#google_translate_element select');
                    selectElement.value = store.state.userSettings.translationLanguage;
                    selectElement.dispatchEvent(new Event('change'));
                }, 1000);
                setTimeout(function () {
                    document.querySelector("body > div:nth-child(1)").style.display = "none";
                }, 1000);
            }
        } catch (err) { }

    });

});



//Go to root page on hard reload
if (router.options.history.state.back == null) { router.push("/"); }

export default router;
