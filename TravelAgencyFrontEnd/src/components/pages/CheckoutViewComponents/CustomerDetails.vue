<template >
    <div class="herp">
        <h4 class="mb-3">Billing address</h4>
        <div class="card h-100">
            <div class="card-body">
                <div class="row gutters">
                    <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12">
                        <h6 class="mb-2 text-primary">{{ $t('user.personalDetails') }} </h6>
                    </div>
                    <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12">
                        <div class="form-group">
                            <label for="email">{{ $t('labels.email') }}</label>
                            <div class=" d-flex">
                                <input v-model="bookingUser.email" :disabled="emailChecked || loggedIn" type="email" class="form-control w-75" id="email" :placeholder="user.Email" @input="save">
                                <button v-if="!loggedIn && !emailChecked" class="submit" :onclick="checkEmail" align="center">{{ $t('labels.verify') }}</button>
                            </div>
                        </div>
                    </div>
                    <div v-if="!verifySent && !loggedIn && !emailChecked" class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12">
                        <div class="form-group">
                            <label>{{ $t('user.logIn') }}</label>
                            <div class=" d-flex">
                                <input v-model="password" type="password" class="form-control w-75" id="password" :placeholder="$t('labels.password')">
                                <button class="submit" :onclick="login" align="center">{{ $t('user.login') }}</button>
                            </div>
                        </div>
                    </div>
                    <div v-if="verifySent" class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12">
                        <div class="form-group">
                            <label>{{ $t('labels.verifyCode') }}</label>
                            <div class=" d-flex">
                                <input v-model="verifyCodeInput" type="text" class="form-control w-50" id="verifyCodeInput">
                                <button class="submit" :onclick="verify" align="center">{{ $t('labels.verify') }}</button>
                                <button class="login" :onclick="showLogin" align="center">{{ $t('user.login') }}</button>
                            </div>
                        </div>
                    </div>
                    <hr class="col-xl-11 col-lg-11 col-md-11 col-sm-11 col-11 pr-5">

                    <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12">
                        <div class="form-group">
                            <label for="firstName">{{ $t('labels.firstname') }}</label>
                            <input v-model="bookingUser.firstName" :disabled="!emailChecked && !loggedIn" type="text" class="form-control" id="firstName" :placeholder="user.FirstName" @input="save">
                        </div>
                    </div>
                    <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12">
                        <div class="form-group">
                            <label for="lastName">{{ $t('labels.lastname') }}</label>
                            <input v-model="bookingUser.lastName" :disabled="!emailChecked && !loggedIn" type="text" class="form-control" id="lastName" :placeholder="user.LastName" @input="save">
                        </div>
                    </div>
                    <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12">
                        <div class="form-group">
                            <label for="phone">{{ $t('labels.phone') }}</label>
                            <input v-model="bookingUser.phone" :disabled="!emailChecked && !loggedIn" type="text" class="form-control" id="phone" :placeholder="user.Phone" @input="save">
                        </div>
                    </div>
                </div>
                <div class="row gutters">
                    <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12">
                        <h6 class="mt-3 mb-2 text-primary">{{ $t('user.address') }}</h6>
                    </div>
                    <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12">
                        <div class="form-group">
                            <label for="Street">{{ $t('labels.street') }}</label>
                            <input v-model="bookingUser.street" :disabled="!emailChecked && !loggedIn" type="name" class="form-control" id="Street" :placeholder="user.Street" @input="save">
                        </div>
                    </div>
                    <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12">
                        <div class="form-group">
                            <label for="city">{{ $t('labels.city') }}</label>
                            <input v-model="bookingUser.city" :disabled="!emailChecked && !loggedIn" type="name" class="form-control" id="city" :placeholder="user.City" @input="save">
                        </div>
                    </div>
                    <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12">
                        <div class="form-group">
                            <label for="zipCode">{{ $t('labels.zipCode') }}</label>
                            <input v-model="bookingUser.zipCode" :disabled="!emailChecked && !loggedIn" type="text" class="form-control" id="zipCode" :placeholder="user.ZipCode" @input="save">
                        </div>
                    </div>
                    <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12">
                        <div class="form-group">
                            <label for="country">{{ $t('labels.country') }}</label>
                            <input v-model="bookingUser.country" :disabled="!emailChecked && !loggedIn" type="text" class="form-control" id="country" :placeholder="user.Country" @input="save">
                        </div>
                    </div>
                </div>
                <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12 mt-5">
                    <h6 class="mb-2 text-primary">{{ $t('labels.noticeToTenants') }}</h6>
                </div>
                    <BookingMessage @message="updateMessage"></BookingMessage>
            </div>
        </div>
    </div>
</template>

<script>
    import BookingMessage from './BookingMessage.vue';
    import { encode } from "base-64";
export default {
    components:{
        BookingMessage,
    },
    data(){
        return {
            emailChecked: false,
            password: null,
            verifyCodeInput: null,
            verifyCode: null,
            verifySent: false
        }
    },
    mounted() {
        this.bookingUser = this.BookingUser;
    },
    computed: {
        loggedIn() {
            return this.$store.state.user.loggedIn;
        },
        user() {
            if (this.loggedIn) {
                this.bookingUser.firstName = this.bookingUser.firstName ? this.bookingUser.firstName : this.$store.state.user.FirstName;
                this.bookingUser.lastName = this.bookingUser.lastName ? this.bookingUser.lastName : this.$store.state.user.LastName;
                this.bookingUser.email = this.bookingUser.email ? this.bookingUser.email : this.$store.state.user.Email;
                this.bookingUser.street = this.bookingUser.street ? this.bookingUser.street : this.$store.state.user.Street;
                this.bookingUser.phone = this.bookingUser.phone ? this.bookingUser.phone : this.$store.state.user.Phone;
                this.bookingUser.city = this.bookingUser.city ? this.bookingUser.city : this.$store.state.user.City;
                this.bookingUser.country = this.bookingUser.country ? this.bookingUser.country : this.$store.state.user.Country;
                this.bookingUser.zipCode = this.bookingUser.zipCode ? this.bookingUser.zipCode : this.$store.state.user.ZipCode;
            }
            return this.$store.state.user;
        },
        bookingUser() {
            return this.$store.state.bookingDetail.user;
        }
    },
    methods: {
        verify() {
            if (this.verifyCode != null && this.verifyCode == this.verifyCodeInput) { 
                this.emailChecked = true;
                this.verifySent = false;
                this.$store.state.bookingDetail.verified = true;

                var notify = Metro.notify; notify.setup({ width: 300, duration: this.$store.state.userSettings.notifyShowTime });
                notify.create(this.$i18n.t("messages.emailVerified"), "Success", { cls: "success" }); notify.reset();
            } else { 
                var notify = Metro.notify; notify.setup({ width: 300, duration: this.$store.state.userSettings.notifyShowTime });
                notify.create(this.$i18n.t("messages.verifyNotMatch"), "Info", { cls: "info" }); notify.reset();
            }
        },
        showLogin() {
            this.verifySent = false;
        },
        async login() {
            let response = await fetch(this.$store.state.apiRootUrl + '/Guest/WebLogin', {
                method: 'post',
                headers: {
                    'Authorization': 'Basic ' + encode(this.bookingUser.email + ":" + this.password),
                    'Content-type': 'application/json'
                },
                body: JSON.stringify({ language: this.$store.state.language })
            });
            let result = await response.json()
            if (result.message) {

                var notify = Metro.notify; notify.setup({ width: 300, duration: this.$store.state.userSettings.notifyShowTime });
                notify.create(result.message, "Error", { cls: "alert" }); notify.reset();

            } else {
                this.$store.state.user = result;
                this.$store.state.user.loggedIn = true;
                this.$store.state.bookingDetail.verified = true;

                var notify = Metro.notify; notify.setup({ width: 300, duration: this.$store.state.userSettings.notifyShowTime });
                notify.create(this.$i18n.t("messages.loginSuccess"), "Success", { cls: "success" }); notify.reset();

                this.bookingUser.firstName = this.bookingUser.firstName ? this.bookingUser.firstName : this.$store.state.user.FirstName;
                this.bookingUser.lastName = this.bookingUser.lastName ? this.bookingUser.lastName : this.$store.state.user.LastName;
                this.bookingUser.email = this.bookingUser.email ? this.bookingUser.email : this.$store.state.user.Email;
                this.bookingUser.street = this.bookingUser.street ? this.bookingUser.street : this.$store.state.user.Street;
                this.bookingUser.phone = this.bookingUser.phone ? this.bookingUser.phone : this.$store.state.user.Phone;
                this.bookingUser.city = this.bookingUser.city ? this.bookingUser.city : this.$store.state.user.City;
                this.bookingUser.country = this.bookingUser.country ? this.bookingUser.country : this.$store.state.user.Country;
                this.bookingUser.zipCode = this.bookingUser.zipCode ? this.bookingUser.zipCode : this.$store.state.user.ZipCode;
            }

        },
        checkEmail() {
            if (!this.loggedIn && !this.emailChecked && this.bookingUser.email.match(/^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/)) {

                var def = $.ajax({
                    global: false, type: "POST",
                    url: this.$store.state.apiRootUrl + "/Guest/Reservation/CheckEmail",
                    dataType: 'json', contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ EmailAddress: this.bookingUser.email, Language: this.$store.state.language })
                });

                var that = this;

                def.fail(function (data) {
                    var notify = Metro.notify; notify.setup({ width: 300, duration: that.$store.state.userSettings.notifyShowTime });
                    notify.create(data.responseJSON.ErrorMessage, "Error", { cls: "alert" }); notify.reset();
                });

                def.done(function (data) {
                    console.log("rteturbn", data);
                    that.verifyCode = data.ErrorMessage;
                    that.verifySent = true;
                    var notify = Metro.notify; notify.setup({ width: 300, duration: that.$store.state.userSettings.notifyShowTime });
                    notify.create(that.$i18n.t("user.verifyEmailWasSent"), "Info", { cls: "info" }); notify.reset();
                });


            }
        },
        save() {
        },
        updateMessage(msg){
            this.$store.state.bookingDetail.message = msg;
            this.save();
        }
    },
    created() {
    },
}
</script>
<style scoped>
    label{
        color:black;
    }

.submit {
    cursor: pointer;
    border-radius: 5em;
    color: #fff;
    background: linear-gradient(to right, #478151, #45705e);
    border: 0;
    padding-left: 40px;
    padding-right: 40px;
    padding-bottom: 10px;
    padding-top: 10px;
    font-family: 'Ubuntu', sans-serif;
    font-size: 13px;
    box-shadow: 0 0 20px 1px rgba(0, 0, 0, 0.04);
}

.login {
    cursor: pointer;
    border-radius: 5em;
    color: #fff;
    background: linear-gradient(to right, #ffac10, #45705e);
    border: 0;
    padding-left: 40px;
    padding-right: 40px;
    padding-bottom: 10px;
    padding-top: 10px;
    font-family: 'Ubuntu', sans-serif;
    font-size: 13px;
    box-shadow: 0 0 20px 1px rgba(0, 0, 0, 0.04);
}

</style>