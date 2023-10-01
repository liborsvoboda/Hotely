<template>
    <div id="checkForm" class="main" style="top: 30px;">
        <p class="sign" align="center">{{ $t('user.forgotPassword')}}</p>

        <form class="form1" @submit.prevent="sendNewPassword">
            <input class="un" type="email" align="center" :placeholder="$t('labels.email')" required v-model="guest.Email">
            <ul v-if="!verified" class="ul">
                <li>
                    <button class="submit shadowed" :onclick="checkValid" align="center">{{ $t('user.sendNewPassword') }}</button>
                </li>
            </ul>
        </form>

        <div class="forgot" align="center"><router-link to="/login">{{ $t('user.logIn') }}</router-link></div>
        <div class="forgot p-0" align="center"><router-link to="/registration">{{ $t('labels.registration') }}</router-link></div>

    </div>
</template>





<script>

export default {

    components: {
    },
    data() {
        return {
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
        checkValid() {
            if (!this.guest.Email.match(/^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/)) {
                document.getElementById("checkForm").classList.add("ani-ring");
                setTimeout(function () {
                    document.getElementById("checkForm").classList.remove("ani-ring");
                }, 1000);
            }

        },
        sendNewPassword() {
            var def = $.ajax({
                global: false, type: "POST",
                url: this.$store.state.apiRootUrl + "/Guest/ResetPassword",
                dataType: 'json', contentType: "application/json; charset=utf-8",
                data: JSON.stringify({ EmailAddress: this.guest.Email, Language: this.$store.state.language })
            });

            var that = this;

            def.fail(function (data) {
                var notify = Metro.notify; notify.setup({ width: 300, timeout: that.$store.state.userSettings.notifyShowTime, duration: 500 });
                notify.create(data.responseJSON.ErrorMessage, "Error", { cls: "alert" }); notify.reset();
            });

            def.done(function (data) {
                var notify = Metro.notify; notify.setup({ width: 300, timeout: that.$store.state.userSettings.notifyShowTime, duration: 500 });
                notify.create(window.dictionary('user.resetPasswordEmailWasSent'), "Info"); notify.reset();
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
    height: 340px;
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