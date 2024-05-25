<template>
    <header id="header">
        <div class="header-top" style="background-color: white !important;max-height: 45px;">
            <div class="container">

                <!-- Language Tool Panel -->
                <div id="toolPanel" data-role="bottom-sheet" class="bottom-sheet pos-fixed list-list grid-style opened" style="top: 0px; left: 85%; z-index:10000;min-width:470px;">
                    <div class="w-100 text-left">
                        <audio id="radio" class="light bg-tra;nsparent" data-role="audio-player" data-src="/src/assets/Media/Toto.mp3" data-volume=".5"></audio>
                    </div>
                    <div class="w-100 text-left" style="z-index: 1000000;">
                        <div id="google_translate_element"></div>
                    </div>

                    <div class="w-100 d-inline-flex">
                        <div class="w-75 text-left">
                            <input id="UserAutoTranslate" type="checkbox" data-role="checkbox" data-cls-caption="fg-cyan text-bold" :data-caption="$t('messages.translateAutomatically')" :onchange="userChangeTranslateSetting">
                        </div><div class="w-25 mt-1 text-right"><button class="button secondary mini" onclick="CancelTranslation()">{{ $t('labels.cancelTranslation') }}</button></div>
                    </div>

                    <div class="divider"></div>

                    <div class="d-flex w-100">
                        <button class="button w-25 mt-1" style="background-color: #585b5d; width:50px;" onclick="ChangeSchemeTo('darcula.css')"></button>
                        <button class="button w-25 mt-1" style="background-color: #AF0015; width:50px;" onclick="ChangeSchemeTo('red-alert.css')"></button>
                        <button class="button w-25 mt-1" style="background-color: #690012; width:50px;" onclick="ChangeSchemeTo('red-dark.css')"></button>
                        <button class="button w-25 mt-1" style="background-color: #0CA9F2; width:50px;" onclick="ChangeSchemeTo('sky-net.css')"></button>
                    </div>

                    <div class="c-pointer mif-cancel icon pos-absolute fg-red" style="top:5px;right:5px;" @click="showToolPanel()"></div>
                </div>


                <!-- NewsLetter Panel -->
                <div id="NewsLetterInfoBox" class="info-box rounded drop-shadow" data-role="infobox" data-type="default" data-width="800" data-height="600" style="visibility:hidden;" data-close-button="false">
                    <span id="CloseNewsletterInfoBox" class="button square closer" onclick="InfoBoxOpenClose('NewsLetterInfoBox')"></span>
                    <div class="info-box-content" style="overflow-y:auto;">
                        <div class="d-flex row ">
                            <div id="NewsLetterBox" class="d-block m-0 p-0" style="overflow-y: scroll;width: calc(100% - 32px);height:550px;max-height: 550px;"></div>
                        </div>
                    </div>
                </div>

                <div class="row text-left">
                    <div class="col-lg-7 col-sm-7 col-7 header-top-left">
                        <div class="nav-menu">
                            <div class="d-flex w-100" style="font-size:16px;">
                                <router-link :to="'/Profile/'">
                                    <span data-role="hint" data-hint-position="bottom" :data-cls-hint="hintPopupClass + ' drop-shadow'" :data-hint-text="window.dictionary('labels.top') + ' ' + userSettings.topFiveCount" class="c-pointer mif-broadcast mif-3x fg-brandColor2 ani-hover-heartbeat"
                                          style="top:-6px; left:5px;"></span>
                                </router-link>

                                <span id="MenuBooking" data-role="hint" data-hint-position="bottom" :data-cls-hint="hintPopupClass + ' drop-shadow'" :data-hint-text="window.dictionary('user.bookings')" class="c-pointer mif-open-book mif-3x fg-brandColor2 ani-hover-heartbeat"
                                      style="top:-9px; left:15px;" @click="checkAllowedMenu('MenuBooking')"></span>

                                <span id="MenuFavorite" data-role="hint" data-hint-position="bottom" :data-cls-hint="hintPopupClass + ' drop-shadow'" :data-hint-text="window.dictionary('user.favorites')" class="c-pointer mif-favorite mif-3x fg-brandColor2 ani-hover-heartbeat"
                                      style="top: -10px;left: 35px;" @click="checkAllowedMenu('MenuFavorite')"></span>

                                <span id="MenuProfileMessages" data-role="hint" data-hint-position="bottom" :data-cls-hint="hintPopupClass + ' drop-shadow'" :data-hint-text="window.dictionary('labels.messages')" class="c-pointer mif-mail-read mif-3x fg-brandColor2 ani-hover-heartbeat"
                                      style="top: -10px;left: 55px;" @click="checkAllowedMenu('MenuProfileMessages')">
                                    <span class="badge bg-orange fg-white p-1 pt-1 mt-2 mr-1" style="font-size: 12px;" :style="((unreadPrivateMessageCount == null || unreadPrivateMessageCount == 0 )? ' display: none ': ' display: inline ')">{{ unreadPrivateMessageCount }}</span>
                                </span>

                                <span id="MenuUserSetting" data-role="hint" data-hint-position="bottom" :data-cls-hint="hintPopupClass + ' drop-shadow'" :data-hint-text="window.dictionary('user.settings')" class="c-pointer mif-server mif-3x fg-brandColor2 ani-hover-heartbeat"
                                      style="top: -9px;left: 85px;" @click="checkAllowedMenu('MenuUserSetting')"></span>


                                <span id="MenuAdvertisement" data-role="hint" data-hint-position="bottom" :data-cls-hint="hintPopupClass + ' drop-shadow'" :data-hint-text="window.dictionary('labels.accommodationAdvertisement')" class="c-pointer mif-location-city mif-3x fg-brandColor2 ani-hover-heartbeat"
                                      style="top: -10px; left: 105px;" @click="checkAllowedMenu('MenuAdvertisement')"></span>

                                <span id="MenuCreditPackages" data-role="hint" data-hint-position="bottom" :data-cls-hint="hintPopupClass + ' drop-shadow'" :data-hint-text="window.dictionary('labels.buyCredit')" class="c-pointer mif-credit-card mif-3x fg-brandColor2 ani-hover-heartbeat"
                                      style="top: -8px; left: 120px;" @click="checkAllowedMenu('MenuCreditPackages')"></span>
                            </div>
                        </div>
                    </div>


                    <div class="col-lg-5 col-sm-5 col-5 header-top-right">
                        <div class="nav-menu">
                            <div v-if="!loggedIn">
                                <a href="#" style="right:70px;"><router-link to="/Login">{{ $t('user.login') }}</router-link></a>
                                <a href="#" style="right:70px;"><router-link to="/Registration">{{ $t('user.register') }}</router-link></a>
                            </div>
                            <div v-if="loggedIn">
                                <a href="#" style="right:80px;" @click="logout()">{{ $t('user.logout') }}</a>
                            </div>

                            <div data-role="hint" data-hint-position="bottom" :data-cls-hint="hintPopupClass + ' drop-shadow'" :data-hint-text="$t('labels.newsletter') + '\r\n' + newsletterLastTimestamp " class="c-pointer mif-news pos-absolute mif-4x fg-brandColor2 ani-hover-heartbeat"
                                 style="top: -8px;right: 45px;" onclick="InfoBoxOpenClose('NewsLetterInfoBox')">
                                <span class="badge bg-orange fg-white p-1 pt-1 mt-2 mr-1" style="font-size: 12px;" :style="(newsletterCount == 0 ? ' display: none ': ' display: inline ')">{{ newsletterCount }}</span>
                            </div>

                            <div data-role="hint" data-hint-position="bottom" :data-cls-hint="hintPopupClass + ' drop-shadow'" :data-hint-text="$t('labels.discussionForum')" class="c-pointer mif-blogger pos-absolute mif-3x fg-brandColor2 ani-hover-heartbeat" style="top:-5px;right:11px;" @click="checkAllowedMenu('DiscussionForum')"></div>

                            <div data-role="hint" data-hint-position="bottom" :data-cls-hint="hintPopupClass + ' drop-shadow'" :data-hint-text="$t('labels.translateWeb')" class="c-pointer mif-earth pos-absolute mif-3x fg-brandColor2 ani-hover-heartbeat" style="top:-5px; z-index:100000;" @click="showToolPanel()"></div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="container">
            <div data-role="appbar" data-expand-point="md" style="top:initial;background-color:#53c16e !important;color: white !important;" class="flex-justify-center bg-brandColor1">
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
        unreadPrivateMessageCount() {
            return this.$store.state.unreadPrivateMessageCount;
        },
        newsletterLastTimestamp() {
            return this.$store.state.newsletterList[0] != null ? "<br />" + this.$i18n.t("labels.lastUpdate") + "<br />" + new Date(this.$store.state.newsletterList[0].timeStamp).toLocaleString('cs-CZ') : "";
        },
        newsletterCount() {
            return this.$store.state.newsletterList.length;
        },
        loggedIn() {
            return this.$store.state.user.loggedIn;
        },
        user() {
            return this.$store.state.user;
        },
        userSettings() {
            return this.$store.state.userSettings;
        },
        advertisement() {
            return this.$store.state.advertisementList;
        },
        actualRoutePath() {
            return this.$router.fullpath;
        },
        hintPopupClass() {
            return Metro.storage.getItem('OnMousePopupClasses', 'bg-cyan fg-white');
        }
    
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
            if (menuName == "DiscussionForum") { this.$router.push('/DiscussionForum'); }
            else if (this.loggedIn) {
                switch (menuName) {
                    case "MenuProfileMessages":
                        this.$router.push('/Profile/ProfileMessages');
                        break;
                    case "MenuBooking":
                        this.$router.push('/Profile/Bookings');
                        break;
                    case "MenuFavorite":
                        this.$router.push('/Profile/Favorite');
                        break;
                    case "MenuUserSetting":
                        this.$router.push('/Profile/ProfileSetting');
                        break;
                    case "MenuCreditPackages":
                        this.$router.push('/Profile/CreditPackages');
                        break;
                    case "MenuAdvertisement":
                        if (this.user.UserId != '') { this.$router.push('/Profile/Advertisement'); }
                        else {
                            document.getElementById(menuName).classList.add("ani-flash");
                            setTimeout(function () { document.getElementById(menuName).classList.remove("ani-flash"); }, 5000);

                            ShowNotify('info', this.$i18n.t("messages.youMustBeAdvertiser"));
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

                ShowNotify('info', this.$i18n.t("messages.menuIsAvailableAfterLogin"));
            }

        },
        async home() {
            await this.$store.dispatch("getMainTopList");
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

            color: #495057 !important;
            -webkit-transition: all 0.3s ease 0s;
            -moz-transition: all 0.3s ease 0s;
            -o-transition: all 0.3s ease 0s;
            transition: all 0.3s ease 0s;
        }

            .header-top a:hover {
                font-weight:bold;

                color: #14a04d !important;
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
        font-weight: bold;
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
