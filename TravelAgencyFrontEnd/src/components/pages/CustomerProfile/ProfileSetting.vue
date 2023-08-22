<template>
    <form class="form1" @submit.prevent="checkPasswords">
        <div class="card-body">
            <div class="row gutters">
                <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12">
                    <h6 class="mb-2 text-primary">{{ $t('user.personalDetails') }}</h6>
                </div>
                <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12">
                    <div class="form-group">
                        <label for="FirstName">{{ $t('labels.firstname') }}</label>
                        <input type="text"
                               class="form-control"
                               id="FirstName"
                               :placeholder="user.FirstName"
                               v-model="guest.FirstName" />
                    </div>
                </div>
                <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12">
                    <div class="form-group">
                        <label for="LastName">{{ $t('labels.lastname') }}</label>
                        <input type="text"
                               class="form-control"
                               id="LastName"
                               :placeholder="user.LastName"
                               v-model="guest.LastName" />
                    </div>
                </div>
                <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12">
                    <div class="form-group">
                        <label for="Email">{{ $t('labels.email') }}</label>
                        <input type="email"
                               class="form-control"
                               id="Email"
                               :placeholder="user.Email"
                               v-model="guest.Email" />
                    </div>
                </div>
                <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12">
                    <div class="form-group">
                        <label for="Phone">{{ $t('labels.phone') }}</label>
                        <input type="text"
                               class="form-control"
                               id="Phone"
                               :placeholder="user.Phone"
                               v-model="guest.Phone" />
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
                        <input type="text"
                               class="form-control"
                               id="Street"
                               :placeholder="user.Street"
                               v-model="guest.Street" />
                    </div>
                </div>
                <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12">
                    <div class="form-group">
                        <label for="City">{{ $t('labels.city') }}</label>
                        <input type="text"
                               class="form-control"
                               id="City"
                               :placeholder="user.City"
                               v-model="guest.City" />
                    </div>
                </div>
                <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12">
                    <div class="form-group">
                        <label for="ZipCode">{{ $t('labels.zipCode') }}</label>
                        <input type="text"
                               class="form-control"
                               id="ZipCode"
                               :placeholder="user.ZipCode"
                               v-model="guest.ZipCode" />
                    </div>
                </div>
                <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12">
                    <div class="form-group">
                        <label for="Country">{{ $t('labels.country') }}</label>
                        <input type="text"
                               class="form-control"
                               id="Country"
                               :placeholder="user.Country"
                               v-model="guest.Country" />
                    </div>
                </div>
                <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12">
                    <h6 class="mt-3 mb-2 text-primary">{{ $t('user.actualOrNewPassword') }}</h6>
                </div>
                <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12">
                    <label for="password">{{ $t('labels.password') }}</label>
                    <div class="input-group flex-nowrap form-group">
                        <input type="password" id="password" required minLength=6 class="form-control" v-model="guest.Password">
                    </div>
                </div>
                <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12">
                    <label for="password">{{ $t('user.repeatPassword') }}</label>
                    <div class="input-group flex-nowrap form-group">
                        <input type="password" id="RePassword" required minLength=6 class="form-control" v-model="guest.confirmPassword">
                    </div>
                </div>
            </div>
            <div class="row gutters">
                <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12">
                    <div class="input-group flex-nowrap form-group" style="margin-left:-20px;">
                        <input class="" v-model="guest.UserId" type="checkbox" value="" id="userId" :disabled="user.UserId != ''" />
                        <span style="padding-left:0px;">{{ $t('labels.advertiseAsHost') }}</span>
                    </div>
                </div>
                <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12">
                    <div class="text-right">
                        <button type="button"
                                id="cancel"
                                name="cancel"
                                class="btn btn-secondary mr-1"
                                @click="resetForm()">
                            {{ $t('user.cancelChanges') }}
                        </button>
                        <button type="button"
                                id="update"
                                name="update"
                                class="btn btn-primary mr-1"
                                @click="checkPasswords()">
                            {{ $t('user.saveChanges') }}
                        </button>
                        <button type="button"
                                id="submit"
                                name="submit"
                                class="btn btn-danger"
                                @click="deleteAccout()">
                            {{ $t('user.deleteAccount') }}
                        </button>
                    </div>
                </div>
            </div>
        </div>
    </form>
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
        };
    },
    async mounted() {

    },
    methods: {
        checkPasswords() {
            if (this.guest.Password.length == 0 || this.guest.Password != this.guest.confirmPassword) {
                this.$store.state.toastErrorMessage = this.$i18n.t("messages.passwordsEmptyOrNotMatch");
            } else if (this.guest.Password === this.guest.confirmPassword) {
                this.updateGuest();
            }
        },
        async updateGuest() {
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
                Password: this.guest.Password ? this.guest.Password : this.user.Password,
                UserId: this.guest.UserId && !this.user.UserId ? "0" : this.guest.UserId && this.user.UserId ? this.user.UserId: null,
                Active: true
            }

            await fetch(this.$store.state.apiRootUrl + '/Guest/UpdateRegistration', {
                method: "POST",
                headers: {
                    "Authorization": 'Bearer ' + this.user.Token,
                    "Content-type": "application/json",
                },

                body: JSON.stringify({
                    User: user,
                    Language: this.$store.state.language 
                }),
            });

            this.$store.state.toastSuccessMessage = this.$i18n.t("messages.dataSaved");

            //ReLogin after Update 
            let credentials = {
                Email: this.guest.Email,
                Password: this.guest.Password
                }
            this.$store.dispatch('login', credentials);
        },
        resetForm() {
            document.querySelector(".form1").reset();
            this.guest.UserId = this.user.UserId;
        },
        deleteAccout() {
            if (confirm(this.$i18n.t("user.doYouReallyDeleteAccount"))) {
                this.$store.dispatch("deleteRegistration", this.user.Id);
                this.$store.state.toastSuccessMessage = this.$i18n.t("messages.accountWasDeleted");
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
