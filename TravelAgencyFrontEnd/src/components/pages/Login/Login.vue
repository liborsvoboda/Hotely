<template>
    <div id="loginForm" class="main" style="top: 30px;">
        <p class="sign" align="center">{{ $t('user.logIn') }}</p>
        <!--   <form class="form1"> -->
            <input class="un" type="email" align="center" placeholder="Email" required v-model="Email" />
            <input class="pass" type="password" align="center" placeholder="Password" required :minLength="$store.state.system.passwordMin" v-model="Password" />
            <button class="submit shadowed" :onclick="checkValid">{{ $t('user.signIn') }}</button>

            <div class="forgot" align="center"><router-link to="/Forgot">{{ $t('user.forgotPassword') }}</router-link></div>
            <div class="forgot p-0" align="center"><router-link to="/Registration">{{ $t('labels.registration') }}</router-link></div>
        <!--  </form> -->
    </div>
</template>

<script>

export default {
    data() {
        return {
            Email: '',
            Password: '',
        }
    },
    computed: {
        loggedIn() {
            return this.$store.state.user.loggedIn;
        },
        user() {
            return this.$store.state.user;
        },
    },
    methods: {
        async checkValid() {
            if (!this.Email.match(/^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/)
                || this.Password.length < this.$store.state.system.passwordMin) {

                ShowNotify('error', (window.dictionary("messages.passwordNotHaveMinimalLengthOrMailNotValid") + this.$store.state.system.passwordMin));
                document.getElementById("loginForm").classList.add("ani-ring");
                setTimeout(function () {
                    document.getElementById("loginForm").classList.remove("ani-ring");
                }, 1000);
            } else {
                await this.guestLogin();
            }
        },
        async guestLogin() {
            let credentials = {
                Email: this.Email,
                Password: this.Password
            }
            await this.$store.dispatch('login', credentials);
        },
        async logout() {
            await this.$store.dispatch('logout');
        },
  },
}
</script>

<style scoped>
body {
  background-color: #f3ebf6;
  font-family: 'Ubuntu', sans-serif;
}

.main {
  background-color: #ffffff;
  width: 400px;
  height: 360px;
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

.un:focus,
.pass:focus {
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
  margin-bottom: 10px;
  font-family: 'Ubuntu', sans-serif;
  font-size: 13px;
  box-shadow: 0 0 20px 1px rgba(0, 0, 0, 0.04);
}

.forgot {
  text-shadow: 0px 0px 3px rgba(117, 117, 117, 0.12);
  color: #e1bee7;
  padding-top: 15px;
}

a {
  text-shadow: 0px 0px 3px rgba(117, 117, 117, 0.12);
  color: #507050;
  text-decoration: none;
}
.text {
  color: red;
}
@media (max-width: 600px) {
  .main {
    border-radius: 0px;
  }
}
</style>
