<template>
    <header id="header">
        <div class="header-top" style="background-color: #0CA9F2 !important">
            <div class="container">

                <div id="toolPanel" data-role="bottom-sheet" class="bottom-sheet pos-fixed list-list grid-style opened" style="top: 0px; left: 90%; z-index:10000;">
                    <div class="w-100 text-left" style="zoom:0.8">
                        <audio id="radio" class="light bg-transparent" data-role="audio-player" data-src="/src/assets/Media/Toto.mp3" data-volume=".5" style="zoom: 0.8"></audio>
                    </div>
                    <div class="w-100 text-left" style="z-index: 1000000;zoom:0.7;">
                        <div id="google_translate_element"></div>
                    </div>

                    <div class="w-100 text-left"><input id="UserAutoTranslate" type="checkbox" data-role="checkbox" data-cls-caption="fg-cyan text-bold" :data-caption="$t('messages.translateAutomatically')" :onchange="userChangeTranslateSetting"></div>
                    <div class="divider"></div>
                    <!-- <div class="d-flex w-100">
                <button class="button w-25 mt-1" style="background-color: #585b5d; width:50px;" onclick="ChangeSchemeTo('darcula.css')"></button>
                <button class="button w-25 mt-1" style="background-color: #AF0015; width:50px;" onclick="ChangeSchemeTo('red-alert.css')"></button>
                <button class="button w-25 mt-1" style="background-color: #690012; width:50px;" onclick="ChangeSchemeTo('red-dark.css')"></button>
                <button class="button w-25 mt-1" style="background-color: #0CA9F2; width:50px;" onclick="ChangeSchemeTo('sky-net.css')"></button>
            </div> -->

                </div>

                <div class="row text-left">
                    <div class="col-lg-9 col-sm-9 col-9 header-top-left">
                        <div class="nav-menu">
                            <div class="d-flex w-100" style="font-size:16px;">
                                <router-link :to="'/profile/'" class="ani-hover-heartbeat">
                                    <!--  <span class="icon mif-star-full " /> -->
                                    {{ $t('labels.topFive') }}
                                </router-link>

                                <a href="#" id="MenuBooking" @click="checkAllowedMenu('MenuBooking')" class="ani-hover-heartbeat">
                                    <!-- <span class="icon mif-list " /> -->
                                    {{ $t('user.bookings') }}
                                </a>

                                <a href="#" id="MenuFavorite" @click="checkAllowedMenu('MenuFavorite')" class="ani-hover-heartbeat">
                                    <!-- <span class="icon mif-favorite " /> -->
                                    {{ $t('user.favorites') }}
                                </a>

                                <a href="#" id="MenuUserSetting" @click="checkAllowedMenu('MenuUserSetting')" class="ani-hover-heartbeat">
                                    <!-- <i class="fas fa-users-cog"></i> -->
                                    {{ $t('user.settings') }}
                                </a>

                                <a href="#" id="MenuAdvertisement" @click="checkAllowedMenu('MenuAdvertisement')" class="ani-hover-heartbeat">
                                    <!-- <span class="icon mif-hotel" :class="(advertisement.length > 0 ? '' : ' ani-shuttle ')"></span> -->
                                    {{ $t('labels.accommodationAdvertisement') }}
                                </a>
                            </div>
                        </div>
                    </div>


                    <div class="col-lg-3 col-sm-3 col-3 header-top-right">
                        <div class="nav-menu">
                            <div v-if="!loggedIn">
                                <a href="#" class="ani-hover-heartbeat"><router-link to="/login">{{ $t('user.login') }}</router-link></a>
                                <a href="#" class="ani-hover-heartbeat"><router-link to="/registration">{{ $t('user.register') }}</router-link></a>
                            </div>
                            <div v-if="loggedIn">
                                <a href="#" @click="logout()">{{ $t('user.logout') }}</a>
                            </div>
                            <div data-role="hint" data-hint-position="bottom" :data-hint-text="$t('labels.translateWeb')" class="c-pointer mif-earth pos-absolute mif-4x fg-brandColor2 ani-hover-heartbeat" style="top:-5px; z-index:100000;" @click="showToolPanel()"></div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="container">
            <div data-role="appbar" data-expand-point="md" style="top:initial;" class="flex-justify-center bg-brandColor1">
                <ul id="helpMenu" class="app-bar-menu" @click="closeHelpMenu">
                    <li @click="home"><router-link to="/">{{ $t('labels.home') }}</router-link></li>
                    <li><router-link to="/UbytkacInfo">{{ $t('labels.ubytkacInfo') }}</router-link></li>
                    <li><router-link to="/RegistrationInfo">{{ $t('labels.registrationInfo') }}</router-link></li>
                    <li><router-link to="/OftenQuestion">{{ $t('labels.oftenQuestion') }}</router-link></li>
                    <li><router-link to="/HolidayTips">{{ $t('labels.holidayTips') }}</router-link></li>
                    <li><router-link to="/Contact">{{ $t('labels.contactus') }}</router-link></li>
                </ul>
            </div>
        </div>
    </header>
</template>

<script>
    import { ref, watch } from 'vue';

export default {
    components: {},
    data() {
        return {

        };
    },
    computed: {
        loggedIn() {
            return this.$store.state.user.loggedIn;
        },
        user() {
            return this.$store.state.user;
        },
        advertisement() {
            return this.$store.state.advertisementList;
        },
        actualRoutePath() {
            return this.$router.fullpath;
        },
    },
    methods: {
        closeHelpMenu() {
            document.querySelector("#helpMenu").classList.remove("opened");
            document.querySelector("#helpMenu").classList.add("collapsed");
            document.querySelector("#helpMenu").style.display = "none";
        },
        userChangeTranslateSetting() {
            Metro.storage.setItem('AutomaticTranslate', $("#UserAutoTranslate").val('checked')[0].checked);
        },
        showToolPanel(close) {
            $("#UserAutoTranslate").val('checked')[0].checked = Metro.storage.getItem('AutomaticTranslate', null);

            if (close) { { Metro.bottomsheet.close($('#toolPanel')); } } else {
                if (Metro.bottomsheet.isOpen($('#toolPanel'))) { Metro.bottomsheet.close($('#toolPanel')); }
                else {
                    Metro.bottomsheet.open($('#toolPanel'));
                }
            }
        },
        checkAllowedMenu(menuName) {
            if (this.loggedIn) {
                switch (menuName) {
                    case "MenuBooking":
                        this.$router.push('/profile/bookings');
                        break;
                    case "MenuFavorite":
                        this.$router.push('/profile/favorite');
                        break;
                    case "MenuUserSetting":
                        this.$router.push('/profile/profileSetting');
                        break;
                    case "MenuAdvertisement":
                        if (this.user.UserId != '') { this.$router.push('/profile/advertisement'); }
                        else {
                            document.getElementById(menuName).classList.add("ani-flash");
                            setTimeout(function () { document.getElementById(menuName).classList.remove("ani-flash"); }, 5000);

                            var notify = Metro.notify; notify.setup({ width: 300, timeout: this.$store.state.userSettings.notifyShowTime, duration: 500 });
                            notify.create(this.$i18n.t("messages.youMustBeAdvertiser"), "Info"); notify.reset();
                        }
                        break;
                }
            } else {
                document.getElementById(menuName).classList.add("ani-flash");
                document.getElementById(menuName).classList.add("bg-red");
                setTimeout(function () {
                    document.getElementById(menuName).classList.remove("ani-flash");
                    document.getElementById(menuName).classList.remove("bg-red");
                }, 3000);

                var notify = Metro.notify; notify.setup({ width: 300, timeout: this.$store.state.userSettings.notifyShowTime, duration: 500 });
                notify.create(this.$i18n.t("messages.menuIsAvailableAfterLogin"), "Info"); notify.reset();
            }

        },
        async home() {
            await this.$store.dispatch("getTopList");
            this.$router.push('/');
        },
        logout() {
            this.$store.dispatch("logout");
        },
    },

    created() {
    },
}
</script>


<style scoped>

.app-bar-menu li {
    list-style: none !important;
}



    #user {
        color: rgb(131, 255, 93);
        padding-right: 25px;
    }

    nav#nav-menu-container {
        display: inline-block;
    }

    ol,
    ul {
        margin: 0;
        padding: 0;
        list-style: none;
    }

    select {
        display: block;
    }

    figure {
        margin: 0;
    }

    a {
        -webkit-transition: all 0.3s ease 0s;
        -moz-transition: all 0.3s ease 0s;
        -o-transition: all 0.3s ease 0s;
        transition: all 0.3s ease 0s;
    }

    /*Header*/

    .header-top {
        font-size: 12px;
        padding: 6px 0px;
        background-color: rgb(83 193 110);
    }

        .header-top a {
            color: rgb(255, 255, 255);
            -webkit-transition: all 0.3s ease 0s;
            -moz-transition: all 0.3s ease 0s;
            -o-transition: all 0.3s ease 0s;
            transition: all 0.3s ease 0s;
        }

            .header-top a:hover {
                /* color:darkblue ; */
                font-weight:bold;
            }

        .header-top ul li {
            display: inline-block;
            margin-right: 15px;
        }

    @media (max-width: 414px) {
        .header-top ul li {
            margin-right: 0px;
        }
    }

    .header-top .header-top-left a {
        margin-right: 8px;
    }

    .header-top .header-top-right {
        text-align: right;
    }

        .header-top .header-top-right .header-social a {
            color: #fff;
            margin-left: 15px;
            -webkit-transition: all 0.3s ease 0s;
            -moz-transition: all 0.3s ease 0s;
            -o-transition: all 0.3s ease 0s;
            transition: all 0.3s ease 0s;
        }

            .header-top .header-top-right .header-social a:hover {
                color: #11b82d;
            }

    .main-menu {
        padding-top: 10px;
        background: #33393a;
        padding-left: 15px;
        padding-right: 15px;
        border-bottom: solid 2px;
        border-color: black;
    }

    #header {
        position: fixed;
        /* position: absolute; */
        left: 0;
        top: 0;
        right: 0;
        transition: all 0.5s;
        z-index: 997;
    }

    /* #header.header-scrolled {
      transition: all 0.5s;
      background-color: rgba(34, 34, 34, 0.9);
    }

    #header.header-scrolled .header-top {
      display: none;
    }

     #header.header-scrolled .main-menu {
      background: transparent;
    } */

    /*--------------------------------------------------------------
    # Navigation Menu
    --------------------------------------------------------------*/
    /* Nav Menu Essentials */
    .nav-menu,
    .nav-menu * {
        margin: 0;
        padding: 0;
        list-style: none;
    }

        .nav-menu ul {
            position: absolute;
            display: none;
            top: 100%;
            right: 0;
            z-index: 99;
        }

        .nav-menu li {
            position: relative;
            white-space: nowrap;
        }

        .nav-menu > li {
            float: left;
        }

        .nav-menu li:hover > ul,
        .nav-menu li.sfHover > ul {
            display: block;
        }

        .nav-menu ul ul {
            top: 0;
            right: 100%;
        }

        .nav-menu ul li {
            min-width: 180px;
        }

    /* Nav Menu Arrows */
    /* .sf-arrows .sf-with-ul {
      padding-right: 30px;
    }

    .sf-arrows .sf-with-ul:after {
      content: "\f107";
      position: absolute;
      right: 15px;
      font-family: FontAwesome;
      font-style: normal;
      font-weight: normal;
    } */

    /* .sf-arrows ul .sf-with-ul:after {
      content: "\f105";
    } */

    /* Nav Meu Container */
    @media (max-width: 768px) {
        #nav-menu-container {
            display: none;
        }
    }

    /* Nav Meu Styling */
    .nav-menu a {
        padding: 0 8px 0px 8px;
        text-decoration: none;
        display: inline-block;
        color: #fff;
        font-weight: 500;
        font-size: 12px;
        text-transform: uppercase;
        outline: none;
    }

    .nav-menu li:hover > a {
        color: #309c4b;
    }

    .nav-menu > li {
        margin-left: 4px;
    }

    .nav-menu ul {
        margin: 22px 0 0 0;
        padding: 10px;
        box-shadow: 0px 0px 30px rgba(127, 137, 161, 0.25);
        background: #fff;
    }

        .nav-menu ul li {
            transition: 0.3s;
        }

            .nav-menu ul li a {
                padding: 5px 10px;
                color: #333;
                transition: 0.3s;
                display: block;
                font-size: 12px;
                text-transform: none;
            }

            .nav-menu ul li:hover > a {
                color: #19df23;
            }

        .nav-menu ul ul {
            margin-right: 10px;
            margin-top: 0;
        }
</style>
