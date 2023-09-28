<template>
    <html>
    <head>
        <link href="https://fonts.googleapis.com/css?family=Ubuntu" rel="stylesheet">
        <meta name="viewport" content="width=device-width, initial-scale=1" />
        <link rel="stylesheet" href="/src/assets/css/font-awesome.min.css">
        <title>Log in</title>
    </head>


    <div class="main">
        <p class="sign" align="center">{{ $t('labels.registration')}}</p>

        <form class="form1" @submit.prevent="checkPasswords">

            <input v-if="verified" class="un" type="text" align="center" :placeholder="$t('labels.firstname')" required v-model="guest.Firstname">
            <input v-if="verified" class="un" type="text" align="center" :placeholder="$t('labels.lastname')" required v-model="guest.Lastname">
            <input v-if="verified" class="un" type="text" align="center" :placeholder="$t('labels.street')" required v-model="guest.Street">
            <input v-if="verified" class="un" type="text" align="center" :placeholder="$t('labels.zipCode')" required v-model="guest.Zipcode">
            <input v-if="verified" class="un" type="text" align="center" :placeholder="$t('labels.city')" required v-model="guest.City">
            <input v-if="verified" class="un" type="text" align="center" :placeholder="$t('labels.country')" required v-model="guest.Country">
            <input v-if="verified" class="un" type="text" align="center" :placeholder="$t('labels.phone')" required v-model="guest.Phone">

            <input class="un" type="email" align="center" :placeholder="$t('labels.email')" required v-model="guest.Email">
            <ul v-if="!verified" class="ul">
                <li>
                    <button class="submit" :onclick="sendVerifyEmail" align="center">{{ $t('user.sendVerifyEmail') }}</button>
                </li>
            </ul>
            <input v-if="verifySent && !verified" class="un" type="text" align="center" :placeholder="$t('labels.verifyCode')" required v-model="Verifycode" @input="checkVerify()">

            <input v-if="verified" class="password" type="password" align="center" :placeholder="$t('labels.password')" required minLength=6 v-model="guest.Password">
            <input v-if="verified" class="confirmPassword" type="password" align="center" :placeholder="$t('labels.retypePassword')" required minLength=6 v-model="guest.ConfirmPassword">

            <ul v-if="verified" class="ul">
                <li>
                    <button class="submit" align="center">{{ $t('user.register') }}</button>
                </li>
            </ul>
        </form>

    </div>

</html>
</template>





<script>

export default {

    components: {
    },
    data() {
        return {
            verifySent: false,
            Verifycode: null,
            ApiVerificationCode: null,
            verified: false,
            guest: {
                Firstname: "",
                Lastname: "",
                Street: "",
                Zipcode: "",
                City: "",
                Country: "",
                Phone: "",
                Email: "",
                Password: "",
                ConfirmPassword: ""
            }
        }
    },
    async mounted() {

    },
    methods: {
        sendVerifyEmail() {
            if (this.guest.Email.match(/^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/)) {

                var def = $.ajax({
                    global: false, type: "POST",
                    url: this.$store.state.apiRootUrl + "/Guest/SendVerifyCode",
                    dataType: 'json', contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ EmailAddress: this.guest.Email, Language: this.$store.state.language })
                });

                var that = this;

                def.fail(function (data) {
                    var notify = Metro.notify; notify.setup({ width: 300, duration: that.$store.state.userSettings.notifyShowTime });
                    notify.create(data.responseJSON.ErrorMessage, "Error", { cls: "alert" }); notify.reset();
                });

                def.done(function (data) {
                    that.ApiVerificationCode = data.verifyCode;
                    that.verifySent = true;

                    var notify = Metro.notify; notify.setup({ width: 300, duration: that.$store.state.userSettings.notifyShowTime });
                    notify.create(that.$i18n.t('user.verifyEmailWasSent'), "Info", { cls: "info" }); notify.reset();
                });
            } else {
                document.querySelector('.main').classList.add("ani-ring");
                setTimeout(function () {
                    document.querySelector('.main').classList.remove("ani-ring");
                }, 1000);
            }
        },
        checkVerify() {
            if (this.Verifycode == this.ApiVerificationCode) {
                document.querySelector('.main').style.height = "950px";
                this.verified = true;
            }
        },
        checkPasswords() {
            if (this.guest.ConfirmPassword.length > 0) {
                if (this.guest.ConfirmPassword != this.guest.Password) {

                    var notify = Metro.notify; notify.setup({ width: 300, duration: this.$store.state.userSettings.notifyShowTime });
                    notify.create(this.$i18n.t("messages.passwordsNotMatch"), "Error", { cls: "alert" }); notify.reset();

                } else if (this.guest.ConfirmPassword == this.guest.Password) {
                    this.guestRegistration();
                }
            }
        },

        async guestRegistration() {
            let regInfo = {
                Firstname: this.guest.Firstname,
                Lastname: this.guest.Lastname,
                Street: this.guest.Street,
                Zipcode: this.guest.Zipcode,
                City: this.guest.City,
                Country: this.guest.Country,
                Phone: this.guest.Phone,
                Email: this.guest.Email,
                Password: this.guest.Password,
                Active: true
            }

            await this.$store.dispatch('registration', regInfo).then(() => {
                if (this.$store.state.tempVariables.registrationStatus.Status == "emailExist") {

                    var notify = Metro.notify; notify.setup({ width: 300, duration: this.$store.state.userSettings.notifyShowTime });
                    notify.create(this.$store.state.tempVariables.registrationStatus.ErrorMessage, "Error", { cls: "alert" }); notify.reset();

                } else if (this.$store.state.tempVariables.registrationStatus.Status == "loginInfoSentToEmail") {
                    this.resetForm();
                    document.querySelector('.form1').innerHTML = '<p class="text"></p>';
                    document.querySelector('.main').style.height = "150px";

                    var notify = Metro.notify; notify.setup({ width: 300, duration: this.$store.state.userSettings.notifyShowTime });
                    notify.create(this.$store.state.tempVariables.registrationStatus.ErrorMessage, "Success", { cls: "success" }); notify.reset();


                    //Login after registration 
                    let credentials = {
                        Email: this.guest.Email,
                        Password: this.guest.Password
                    }
                    this.$store.dispatch('login', credentials)

                }
            });

        },
        resetForm() {
            document.querySelector('.form1').reset()
        }
    }
}
</script>


<style scoped>

a:link {
    text-decoration: none;
}

a:visited {
    text-decoration: none;
}

a:hover {
    text-decoration: none;
}

a:active {
    text-decoration: none;
}

.main {
    background-color: #ffffff;
    width: 400px;
    height: 300px;
    margin: 7em auto;
    border-radius: 1.5em;
    box-shadow: 0px 11px 35px 2px rgba(0, 0, 0, 0.14);
}

.sign {
    padding-top: 40px;
    color: #548358;
    font-family: 'Ubuntu', sans-serif;
    font-weight: bold;
    font-size: 23px;
}

.un {
    width: 76%;
    color: rgb(38, 50, 56);
    font-weight: 700;
    font-size: 14px;
    letter-spacing: 1px;
    background: rgba(136, 126, 126, 0.04);
    padding: 10px 20px;
    border: none;
    border-radius: 20px;
    outline: none;
    box-sizing: border-box;
    border: 2px solid rgba(0, 0, 0, 0.02);
    margin-bottom: 50px;
    text-align: center;
    margin-bottom: 27px;
    font-family: 'Ubuntu', sans-serif;
}

.password {
    width: 76%;
    color: rgb(38, 50, 56);
    font-weight: 700;
    font-size: 14px;
    letter-spacing: 1px;
    background: rgba(136, 126, 126, 0.04);
    padding: 10px 20px;
    border: none;
    border-radius: 20px;
    outline: none;
    box-sizing: border-box;
    border: 2px solid rgba(0, 0, 0, 0.02);
    margin-bottom: 50px;
    text-align: center;
    margin-bottom: 27px;
    font-family: 'Ubuntu', sans-serif;
}

.confirmPassword {
    width: 76%;
    color: rgb(38, 50, 56);
    font-weight: 700;
    font-size: 14px;
    letter-spacing: 1px;
    background: rgba(136, 126, 126, 0.04);
    /* padding: 10px 20px; */
    border: none;
    border-radius: 20px;
    outline: none;
    box-sizing: border-box;
    border: 2px solid rgba(0, 0, 0, 0.02);
    margin-bottom: 50px;
    text-align: center;
    margin-bottom: 27px;
    font-family: 'Ubuntu', sans-serif;
}

form.form1 {
    padding-top: 40px;
}

.pass {
    width: 76%;
    color: rgb(38, 50, 56);
    font-weight: 700;
    font-size: 14px;
    letter-spacing: 1px;
    background: rgba(136, 126, 126, 0.04);
    padding: 10px 20px;
    border: none;
    border-radius: 20px;
    outline: none;
    box-sizing: border-box;
    border: 2px solid rgba(0, 0, 0, 0.02);
    margin-bottom: 50px;
    text-align: center;
    margin-bottom: 27px;
    font-family: 'Ubuntu', sans-serif;
}

.text {
    color: red;
    font-size: 12px;
}



.un:focus, .pass:focus {
    border: 2px solid rgba(0, 0, 0, 0.18) !important;
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

.ul {
    list-style: none;
    padding-right: 40px;
}

.forgot {
    text-shadow: 0px 0px 3px rgba(117, 117, 117, 0.12);
    color: #E1BEE7;
    padding-top: 15px;
}

a {
    text-shadow: 0px 0px 3px rgba(117, 117, 117, 0.12);
    color: #507050;
    text-decoration: none
}

@media (max-width: 600px) {
    .main {
        border-radius: 0px;
    }
}
</style>