<template>
    <div class="profile-content mt-10">
        <div class="rounded row pl-3 pr-3">
            <div class="col-md-6 text-left">

                <div class="mt-2" data-role="hint" :data-hint-text="$t('labels.showArchive')">
                    <input id="showArchive" type="checkbox" data-role="checkbox" class="" data-title="Checkbox" :onchange="showAlsoInactive" :checked="$store.state.userSettings.showInactiveAdvertisementAsDefault">
                </div>

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
            ShowArchived: false

        };
    },
    async mounted() {
      
    },
    methods: {

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
