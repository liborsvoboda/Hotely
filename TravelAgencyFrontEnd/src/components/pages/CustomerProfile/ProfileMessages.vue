<template>
    <div class="profile-content mt-10">
        <div class="rounded row pl-3 pr-3">
            <div class="col-md-6 text-left">
                <h1>{{ $t('labels.messaging') }}</h1>
            </div>
            <div class="col-md-6 text-right">
                <span class="icon mif-info pt-3 mif-3x c-pointer fg-orange" onclick="OpenDocView('Messaging')" />
            </div>
        </div>
        <hr>
        <div class="card-body">

            <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12 pl-5 pr-5 pt-5 mb-0">
                <ul data-role="tabs" data-expand="true" data-on-tab="setBackgroundProfileMenu()">
                    <li id="newsletterMenu" class="fg-black text-bold bg-brandColor1"><a href="#_newsletterMenu">{{ $t('labels.newsletter') }}</a></li>
                    <li id="privateMessagesMenu" class="fg-black text-bold"><a href="#_privateMessagesMenu">{{ $t('user.privateMessages') }}</a></li>
                    <li id="reservationMessagesMenu" class="fg-black text-bold"><a href="#_reservationMessagesMenu">{{ $t('labels.reservationMessages') }}</a></li>
                </ul>
            </div>

            <div id="_newsletterMenu">
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
                </div>
            </div>

            <div id="_privateMessagesMenu">
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
                </div>
            </div>

            <div id="_reservationMessagesMenu">
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
                sendNewMessagesToEmail: null
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
        checkPasswords() {
            if (this.guest.Password.length > 0 && this.guest.Password.length < this.$store.state.system.passwordMin) {

                var notify = Metro.notify; notify.setup({ width: 300, timeout: this.$store.state.userSettings.notifyShowTime, duration: 500 });
                notify.create((window.dictionary("messages.passwordNotHaveMinimalLength") + this.$store.state.system.passwordMin), "Error", { cls: "alert" }); notify.reset();

            } else if (this.guest.Password.length >= this.$store.state.system.passwordMin && this.guest.Password != this.guest.confirmPassword) {

                var notify = Metro.notify; notify.setup({ width: 300, timeout: this.$store.state.userSettings.notifyShowTime, duration: 500 });
                notify.create(window.dictionary("messages.passwordsEmptyOrNotMatch"), "Error", { cls: "alert" }); notify.reset();

            } else if (this.guest.Password === this.guest.confirmPassword) {
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
                that.userSettings.topFiveCount = that.$store.state.userSettings.topFiveCount; $("#topFiveCount").val(that.userSettings.topFiveCount);
                that.userSettings.notifyShowTime = that.$store.state.userSettings.notifyShowTime / 1000; $("#notifyShowTime").val(that.userSettings.notifyShowTime);
                $("#showInactiveAdvertisementAsDefault").val('checked')[0].checked = that.userSettings.showInactiveAdvertisementAsDefault = that.$store.state.userSettings.showInactiveAdvertisementAsDefault;
                that.userSettings.translationLanguage = that.$store.state.userSettings.translationLanguage;
                $("#hideSearchingInPrivateZone").val('checked')[0].checked = that.userSettings.hideSearchingInPrivateZone = that.$store.state.userSettings.hideSearchingInPrivateZone;
                $("#sendNewsletterToEmail").val('checked')[0].checked = that.userSettings.sendNewsletterToEmail = that.$store.state.userSettings.sendNewsletterToEmail;
                $("#sendNewMessagesToEmail").val('checked')[0].checked = that.userSettings.sendNewMessagesToEmail = that.$store.state.userSettings.sendNewMessagesToEmail;
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
