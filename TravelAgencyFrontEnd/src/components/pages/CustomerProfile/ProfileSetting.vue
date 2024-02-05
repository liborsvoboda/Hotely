<template>
    <div class="">
        <div class="rounded drop-shadow row">
            <div class="col-md-6 text-left">
                <h1>{{ $t('user.settings') }}</h1>
            </div>
            <div class="col-md-6 text-right">
                <span class="icon mif-info pt-3 mif-3x c-pointer fg-orange" onclick="OpenDocView('Settings')" />
            </div>
        </div>
        <hr>
        <form class="form1" @submit.prevent="checkPasswords" autocomplete="off">
            <div class="card-body">

                <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12 pl-5 pr-5 pt-5 mb-0">
                    <ul data-role="tabs" data-expand="true" data-on-tab="setBackgroundProfileMenu()">
                        <!-- <ul data-app-bar="true" data-role="materialtabs" data-fixed-tabs="true" data-deep="true"> -->
                        <li id="personalDetailsMenu" class="fg-black text-bold bg-brandColor1"><a href="#_personalDetails">{{ $t('user.personalDetails') }}</a></li>
                        <li id="addressMenu" class="fg-black text-bold"><a href="#_address">{{ $t('user.address') }}</a></li>
                        <li id="actualOrNewPasswordMenu" class="fg-black text-bold"><a href="#_actualOrNewPassword">{{ $t('labels.password') }}</a></li>
                        <li id="advertisingMenu" class="fg-black text-bold"><a href="#_advertising">{{ $t('labels.advertising') }}</a></li>
                        <li id="settingsMenu" class="fg-black text-bold"><a href="#_settings">{{ $t('user.settings') }}</a></li>
                    </ul>
                </div>

                <div id="_personalDetails">
                    <div class="d-flex row gutters ml-5 mr-5 mb-5 border">
                        <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12">
                            <h6 class="mt-3 mb-2 text-primary">{{ $t('user.personalDetails') }}</h6>
                        </div>

                        <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12">
                            <div class="form-group p-3 pb-0">
                                <label for="form-group FirstName">{{ $t('labels.firstname') }}</label>
                                <input type="text" class="form-control" id="FirstName" :placeholder="user.FirstName" v-model="guest.FirstName" autocomplete="off" />
                            </div>
                        </div>
                        <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12">
                            <div class="form-group p-3 pb-0">
                                <label for="LastName">{{ $t('labels.lastname') }}</label>
                                <input type="text" class="form-control" id="LastName" :placeholder="user.LastName" v-model="guest.LastName" autocomplete="off" />
                            </div>
                        </div>
                        <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12">
                            <div class="form-group p-3 pb-0">
                                <label for="Email">{{ $t('labels.email') }}</label>
                                <input type="email" class="form-control" id="Email" :placeholder="user.Email" v-model="guest.Email" autocomplete="off" readonly />
                            </div>
                        </div>
                        <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12">
                            <div class="form-group p-3 pb-0">
                                <label for="Phone">{{ $t('labels.phone') }}</label>
                                <input type="text" class="form-control" id="Phone" :placeholder="user.Phone" v-model="guest.Phone" autocomplete="off" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>


            <div id="_address">
                <div class="d-flex row gutters ml-5 mr-5 mb-5 border">
                    <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12">
                        <h6 class="mt-3 mb-2 text-primary">{{ $t('user.address') }}</h6>
                    </div>

                    <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12">
                        <div class="form-group p-3 pb-0">
                            <label for="Street">{{ $t('labels.street') }}</label>
                            <input type="text" class="form-control" id="Street" :placeholder="user.Street" v-model="guest.Street" autocomplete="off" />
                        </div>
                    </div>
                    <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12">
                        <div class="form-group p-3 pb-0">
                            <label for="City">{{ $t('labels.city') }}</label>
                            <input type="text" class="form-control" id="City" :placeholder="user.City" v-model="guest.City" autocomplete="off" />
                        </div>
                    </div>
                    <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12">
                        <div class="form-group p-3 pb-0">
                            <label for="ZipCode">{{ $t('labels.zipCode') }}</label>
                            <input type="text" class="form-control" id="ZipCode" :placeholder="user.ZipCode" v-model="guest.ZipCode" autocomplete="off" />
                        </div>
                    </div>
                    <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12">
                        <div class="form-group p-3 pb-0">
                            <label for="Country">{{ $t('labels.country') }}</label>
                            <input type="text" class="form-control" id="Country" :placeholder="user.Country" v-model="guest.Country" autocomplete="off" />
                        </div>
                    </div>
                </div>
            </div>

            <div id="_actualOrNewPassword">
                <div class="d-flex row gutters ml-5 mr-5 mb-5 border">
                    <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12">
                        <h6 class="mt-3 mb-2 text-primary">{{ $t('user.newPassword') }}</h6>
                    </div>
                    <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12">
                        <label for="password">{{ $t('labels.password') }}</label>
                        <div class="input-group flex-nowrap form-group p-3 pb-0">
                            <input type="password" id="password" required :minLength="$store.state.system.passwordMin" class="form-control" v-model="guest.Password" autocomplete="new-password">
                        </div>
                    </div>
                    <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12">
                        <label for="password">{{ $t('user.repeatPassword') }}</label>
                        <div class="input-group flex-nowrap form-group p-3 pb-0">
                            <input type="password" id="RePassword" required :minLength="$store.state.system.passwordMin" class="form-control" v-model="guest.confirmPassword" autocomplete="off">
                        </div>
                    </div>
                </div>
            </div>
        </form>

        <div id="_advertising">
            <div class="d-flex row gutters ml-5 mr-5 mb-5 border">
                <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12">
                    <h6 class="mt-3 mb-2 text-primary">{{ $t('labels.advertising') }}</h6>
                </div>
                <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12">
                    <div class="input-group flex-nowrap form-group p-3 pb-0" style="margin-left:-20px;">
                        <input id="userId" class="" v-model="guest.UserId" type="checkbox" value="" :disabled="user.UserId != ''" onclick="AdvertiserSetingChangedEvent" />
                        <span style="padding-left:0px;">{{ $t('labels.advertiseAsHost') }}</span>
                    </div>
                </div>

                <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12">
                    <div class="input-group flex-nowrap form-group p-3 pb-0" style="margin-left:-20px;">
                        <input id="showInactiveAdvertisementAsDefault" class="" v-model="userSettings.showInactiveAdvertisementAsDefault" type="checkbox" value="" :disabled="user.UserId == ''" />
                        <span style="padding-left:0px;">{{ $t('labels.showInactiveAdvertisementAsDefault') }}</span>
                    </div>
                </div>
            </div>
        </div>

        <div id="_settings">
            <div class="d-flex row gutters ml-5 mr-5 mb-5 border">
                <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12">
                    <h6 class="mt-3 mb-2 text-primary">{{ $t('user.settings') }}</h6>
                </div>

                <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12">
                    <div class="form-group p-3 pb-0">
                        <label for="Street">{{ $t('labels.notifyShowTime') }}</label>
                        <input id="notifyShowTime" type="text" v-model="userSettings.notifyShowTime" data-role="spinner" data-min-value="1" data-max-value="30">
                    </div>
                </div>
                <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12">
                    <div class="form-group p-3 pb-0">
                        <label>{{ $t('labels.autoTranslationLanguage') }}</label>
                        <span id="languageSelector"></span>
                    </div>
                </div>
                <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12 pt-5">
                    <div class="input-group flex-nowrap form-group p-3 pb-0" style="margin-left:-20px;">
                        <input id="hideSearchingInPrivateZone" class="" v-model="userSettings.hideSearchingInPrivateZone" type="checkbox" value="" />
                        <span style="padding-left:0px;">{{ $t('labels.hideSearchingInPrivateZone') }}</span>
                    </div>
                </div>
                <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12">
                    <div class="form-group p-3 pb-0">
                        <label for="Street">{{ $t('labels.setCustomTopFiveCount') }}</label>
                        <input id="topFiveCount" type="text" v-model="userSettings.topFiveCount" data-role="spinner" data-min-value="5" data-step="10" data-max-value="250">
                    </div>
                </div>

                <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12 pt-5">
                    <div class="input-group flex-nowrap form-group p-3 pb-0" style="margin-left:-20px;">
                        <input id="sendNewsletterToEmail" class="" v-model="userSettings.sendNewsletterToEmail" type="checkbox" value="" />
                        <span style="padding-left:0px;">{{ $t('labels.sendNewsletterToEmail') }}</span>
                    </div>
                </div>

                <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12 pt-5">
                    <div class="input-group flex-nowrap form-group p-3 pb-0" style="margin-left:-20px;">
                        <input id="sendNewMessagesToEmail" class="" v-model="userSettings.sendNewMessagesToEmail" type="checkbox" value="" />
                        <span style="padding-left:0px;">{{ $t('labels.sendNewMessagesToEmail') }}</span>
                    </div>
                </div>

                <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12 pt-5">
                    <div class="input-group flex-nowrap form-group p-3 pb-0" style="margin-left:-20px;">
                        <input id="showArchivedMessages" class="" v-model="userSettings.showArchivedMessages" type="checkbox" value="" />
                        <span style="padding-left:0px;">{{ $t('labels.showArchivedMessages') }}</span>
                    </div>
                </div>

            </div>
        </div>


        <div class="row gutters pr-5">
            <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12">
            </div>
            <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12">
                <div class="text-right">
                    <button type="button" class="button secondary outline shadowed mb-1" @click="resetForm();">
                        {{ $t('user.cancelChanges') }}
                    </button>
                    <button type="button" class="button success outline shadowed ml-1 mb-1" @click="checkPasswords()">
                        {{ $t('user.saveChanges') }}
                    </button>
                    <button type="button" class="button alert outline shadowed ml-1 mb-1" @click="deleteAccout()">
                        {{ $t('user.deleteAccount') }}
                    </button>
                </div>
            </div>
        </div>

    </div>
</template>

<script>


export default {
    components: {},
    data() {
        return {
            AdvertiserSetingChanged: false,

            guest: {
                FirstName: "",
                LastName: "",
                Street: "",
                ZipCode: "",
                City: "",
                Country: "",
                Phone: "",
                Email: "",
                Password: "",
                confirmPassword: "",
                id: '',
                UserId: false
            },
            userSettings: {
                notifyShowTime: null,
                showInactiveAdvertisementAsDefault: null,
                translationLanguage: null,
                hideSearchingInPrivateZone: null,
                topFiveCount: null,
                sendNewsletterToEmail: null,
                sendNewMessagesToEmail: null,
                showArchivedMessages: null,
            },

        };
    },
    async mounted() {
        this.userSettings.topFiveCount = this.$store.state.userSettings.topFiveCount;
        this.userSettings.notifyShowTime = this.$store.state.userSettings.notifyShowTime / 1000;
        this.userSettings.showInactiveAdvertisementAsDefault = this.$store.state.userSettings.showInactiveAdvertisementAsDefault;
        this.userSettings.translationLanguage = this.$store.state.userSettings.translationLanguage;
        this.userSettings.hideSearchingInPrivateZone = this.$store.state.userSettings.hideSearchingInPrivateZone;
        this.userSettings.sendNewsletterToEmail = this.$store.state.userSettings.sendNewsletterToEmail;
        this.userSettings.sendNewMessagesToEmail = this.$store.state.userSettings.sendNewMessagesToEmail;
        this.userSettings.showArchivedMessages = this.$store.state.userSettings.showArchivedMessages;

        
        //get google languagelist
        try { 
            if (document.querySelector("#\\:0\\.targetLanguage > select") != null) {
                let googleLanguagelist = [].slice.call(document.querySelector("#\\:0\\.targetLanguage > select").options);
                let html = "<select data-role='select' id='languageList' data-filter-placeholder='" + window.dictionary('labels.selectLanguage') + "' data-empty-value='' data-validate='required' data-clear-button='true' >";
                let jumpFirst = true;
                googleLanguagelist.forEach(lang => { if (!jumpFirst) { html += "<option value='" + lang.value + "'>" + lang.text + "</option>"; }; jumpFirst = false; });
                html += "</select>";
                $("#languageSelector").html(html);
                $("#languageList").val(this.userSettings.translationLanguage);
            }
        } catch { }

    },
    methods: {
        AdvertiserSetingChangedEvent() {
            this.AdvertiserSetingChanged = true;
        },
        checkPasswords() {

            if (this.guest.Password.length > 0 && this.guest.Password.length < this.$store.state.system.passwordMin) {
                var notify = Metro.notify; notify.setup({ width: 300, timeout: this.$store.state.userSettings.notifyShowTime, duration: 500 });
                notify.create((window.dictionary("messages.passwordNotHaveMinimalLength") + this.$store.state.system.passwordMin), "Error", { cls: "alert" }); notify.reset();
            } else if (this.guest.Password.length >= this.$store.state.system.passwordMin && this.guest.Password != this.guest.confirmPassword) {
                var notify = Metro.notify; notify.setup({ width: 300, timeout: this.$store.state.userSettings.notifyShowTime, duration: 500 });
                notify.create(window.dictionary("messages.passwordsEmptyOrNotMatch"), "Error", { cls: "alert" }); notify.reset();
            } else if (this.guest.Password == "" && this.guest.Password === this.guest.confirmPassword && this.guest.UserId == false) {
                this.updateGuest();
            } else if (this.AdvertiserSetingChanged && this.guest.Password == "" && this.guest.Password === this.guest.confirmPassword && this.guest.UserId == true) {
                var notify = Metro.notify; notify.setup({ width: 300, timeout: this.$store.state.userSettings.notifyShowTime, duration: 500 });
                notify.create(window.dictionary("messages.forAdvertiserActivationYouMustInserttPassword"), "Error", { cls: "alert" }); notify.reset();
            } else if (this.AdvertiserSetingChanged && this.guest.Password.length >= this.$store.state.system.passwordMin && this.guest.Password === this.guest.confirmPassword && this.guest.UserId == true) {
                this.updateGuest();
            } else if (!this.AdvertiserSetingChanged && this.guest.Password == "" && this.guest.Password === this.guest.confirmPassword && isNaN(this.guest.UserId) == false) {
                this.updateGuest();
            }
        },
        async updateGuest() {
            let guestSettings = [];
            guestSettings.push({ Key: 'topFiveCount', Value: $("#topFiveCount").val() });
            guestSettings.push({ Key: 'notifyShowTime', Value: $("#notifyShowTime").val() * 1000 });
            guestSettings.push({ Key: 'showInactiveAdvertisementAsDefault', Value: this.userSettings.showInactiveAdvertisementAsDefault });
            guestSettings.push({ Key: 'translationLanguage', Value: ($("#languageList")[0].selectedOptions[0] != undefined ? $("#languageList")[0].selectedOptions[0].value : "") });
            guestSettings.push({ Key: 'hideSearchingInPrivateZone', Value: this.userSettings.hideSearchingInPrivateZone });
            guestSettings.push({ Key: 'sendNewsletterToEmail', Value: this.userSettings.sendNewsletterToEmail });
            guestSettings.push({ Key: 'sendNewMessagesToEmail', Value: this.userSettings.sendNewMessagesToEmail });
            guestSettings.push({ Key: 'showArchivedMessages', Value: this.userSettings.showArchivedMessages });

            
            await this.$store.dispatch('updateGuestSetting', guestSettings);

            let user = {
                Id: this.user.Id,
                FirstName: this.guest.FirstName ? this.guest.FirstName : this.user.FirstName,
                LastName: this.guest.LastName ? this.guest.LastName : this.user.LastName,
                Street: this.guest.Street ? this.guest.Street : this.user.Street,
                ZipCode: this.guest.ZipCode ? this.guest.ZipCode : this.user.ZipCode,
                City: this.guest.City ? this.guest.City : this.user.City,
                Country: this.guest.Country ? this.guest.Country : this.user.Country,
                Phone: this.guest.Phone ? this.guest.Phone : this.user.Phone,
                Email: this.guest.Email ? this.guest.Email : this.user.Email,
                Password: this.guest.Password.length > 0 ? this.guest.Password : this.user.Password,
                UserId: this.guest.UserId && !this.user.UserId ? "0" : this.guest.UserId && this.user.UserId ? this.user.UserId : null,
                Active: true
            }

            await this.$store.dispatch('updateRegistration', user);
            this.resetForm();
        },
        resetForm() {
            document.querySelector(".form1").reset();
            this.guest.UserId = this.user.UserId;
           

            //refil local setting
            var that = this;
            setTimeout(function () {
                that.AdvertiserSetingChanged = false;

                that.userSettings.topFiveCount = that.$store.state.userSettings.topFiveCount; $("#topFiveCount").val(that.userSettings.topFiveCount);
                that.userSettings.notifyShowTime = that.$store.state.userSettings.notifyShowTime / 1000; $("#notifyShowTime").val(that.userSettings.notifyShowTime);
                $("#showInactiveAdvertisementAsDefault").val('checked')[0].checked = that.userSettings.showInactiveAdvertisementAsDefault = that.$store.state.userSettings.showInactiveAdvertisementAsDefault;
                that.userSettings.translationLanguage = that.$store.state.userSettings.translationLanguage;
                $("#hideSearchingInPrivateZone").val('checked')[0].checked = that.userSettings.hideSearchingInPrivateZone = that.$store.state.userSettings.hideSearchingInPrivateZone;
                $("#sendNewsletterToEmail").val('checked')[0].checked = that.userSettings.sendNewsletterToEmail = that.$store.state.userSettings.sendNewsletterToEmail;
                $("#sendNewMessagesToEmail").val('checked')[0].checked = that.userSettings.sendNewMessagesToEmail = that.$store.state.userSettings.sendNewMessagesToEmail;
                $("#showArchivedMessages").val('checked')[0].checked = that.userSettings.showArchivedMessages = that.$store.state.userSettings.showArchivedMessages;
                $("#userId").val('checked')[0].checked = that.guest.UserId != '' ? true : false;
            }, 100);
        },
        deleteAccout() {
            if (confirm(window.dictionary("user.doYouReallyDeleteAccount"))) {
                this.$store.dispatch("deleteRegistration", this.user.Id);

                var notify = Metro.notify; notify.setup({ width: 300, timeout: this.$store.state.userSettings.notifyShowTime, duration: 500 });
                notify.create(window.dictionary("messages.accountWasDeleted"), "Success", { cls: "success" }); notify.reset();
            }
        },
    },
    computed: {
        loggedIn() {
            return this.$store.state.user.loggedIn;
        },
        user() {
            this.guest.UserId = this.$store.state.user.UserId == "" ? false : true;
            return this.$store.state.user;
        },
    },
    created() {
        
    }
};
</script>

<style scoped>
label {
  color: black;
}

.text {
  color: red;
}

#update{
  background-color: rgb(83 193 110);
  border: #0e833d
}
</style>
