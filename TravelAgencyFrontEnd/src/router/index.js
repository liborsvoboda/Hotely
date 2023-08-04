import { createRouter, createWebHistory } from "vue-router";
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
import Profile from '../components/pages/CustomerProfile/ProfilePage.vue'
import Bookings from '../components/pages/CustomerProfile/Bookings.vue'
import FavoriteHotelList from '/src/components/pages/CustomerProfile/FavoriteHotelList.vue';
import ProfileSetting from '../components/pages/CustomerProfile/ProfileSetting.vue'
import Contact from '../components/pages/Extra pages/Contact.vue';
import About from '../components/pages/Extra pages/About.vue';
import UbytkacInfo from '../components/pages/Extra pages/UbytkacInfo.vue';
import RegistrationInfo from '../components/pages/Extra pages/RegistrationInfo.vue';
import OftenQuestion from '../components/pages/Extra pages/OftenQuestion.vue';
import HolidayTips from '../components/pages/Extra pages/HolidayTips.vue';

const routes = [
    {
        path: "/",
        name: "Home",
        component: Home,
    },
    {
        path: "/Profile",
        name: "Profile",
        component: ProfileMain,
        children: [
            {
                path: "",
                component: Profile,
            },
            {
                path: "bookings",
                name: "Bookings",
                component: Bookings,
            },
            {
                path: "favorite",
                name: "favorite",
                component: FavoriteHotelList,
            },
            {
                path: "profileSetting",
                name: "profileSetting",
                component: ProfileSetting,
            },
        ],
    },
    {
        path: "/result",
        name: "result",
        component: SearchResult,
    },
    {
        path: "/login",
        name: "login",
        component: Login,
    },
    {
        path: "/registration",
        name: "registration",
        component: Registration,
    },
    {
        path: "/forgot",
        name: "forgot",
        component: Forgot,
    },
    {
        path: "/Contact",
        name: "contact",
        component: Contact,
    },
    {
        path: "/About",
        name: "about",
        component: About,
    },
    {
        path: "/UbytkacInfo",
        name: "ubytkacInfo",
        component: UbytkacInfo,
    },
    {
        path: "/RegistrationInfo",
        name: "registrationInfo",
        component: RegistrationInfo,
    },
    {
        path: "/OftenQuestion",
        name: "oftenQuestion",
        component: OftenQuestion,
    },
    {
        path: "/HolidayTips",
        name: "holidayTips",
        component: HolidayTips,
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
        children: [
            {
                path: "",
                component: Info,
            },
            {
                path: "rooms",
                name: "hotels",
                component: Rooms,
            },
            {
                path: "Photos",
                name: "photos",
                component: PhotoGallery,
            },
            {
                path: "Reviews",
                name: "reviews",
                component: Reviews,
            },
            {
                path: "AddReview",
                name: "addReview",
                component: AddReview,
            },
        ],
    },
    {
        path: "/checkout",
        name: "Checkout",
        component: CheckoutView,
        children: [
            {
                path: "",
                name: "customerDetails",
                component: CustomerDetails,
            },
            {
                path: "orderDetails",
                name: "OrderDetails",
                component: OrderDetails,
            },
            {
                path: "orderConfirmed",
                name: "OrderConfirmed",
                component: OrderConfirmed,
            }
        ]
    },
    //{
    //    path: "/reservationdetails/:id",
    //    name: "reservationdetails",
    //    component: ReservationDetails,
    //},
];
const router = createRouter({
    history: createWebHistory(),
    routes,
});
export default router;
