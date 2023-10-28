<template >
    <div>
        <Card class="cardStyle">
            <template #title>
                {{ $t('messages.reservationWasSent') }}
            </template>
            <template #content>
                <img src="https://raw.githubusercontent.com/PKief/vscode-markdown-checkbox/master/logo.png" class="rounded-circle" id="orderPic"/>
                <div> {{ $t('messages.reservationInfo') }} </div>
            </template>
        </Card>
    </div>
</template>
<script>
    import Card from 'primevue/card';
    import router from '../../../router/index';
    import { encode } from "base-64";
export default {
    components:{
        Card,
    },
    computed: {
        loggedIn() {
            return this.$store.state.user.loggedIn;
        },
    },
    async mounted() {
        //check unauthorized open url
        if (this.$store.state.bookingDetail.hotelId == null) { this.$router.push('/'); }

        if (this.loggedIn) {
            //send auth booking 
            let response = await fetch(this.$store.state.apiRootUrl + '/Guest/Reservation/AuthSetBooking', {
                method: 'POST',
                headers: {
                    'Authorization': 'Bearer ' + this.$store.state.user.Token,
                    'Content-type': 'application/json'
                },
                body: JSON.stringify({ Booking: this.$store.state.bookingDetail, Language: this.$store.state.language })
            });

            let result = await response.json();

            if (result.ErrorMessage) {

                var notify = Metro.notify; notify.setup({ width: 300, timeout: this.$store.state.userSettings.notifyShowTime, duration: 500 });
                notify.create(result.ErrorMessage, "Error", { cls: "alert" }); notify.reset();
            } else {
                this.$store.dispatch('clearBooking');
            }
        } else {
            //send unauth booking
            let response = await fetch(this.$store.state.apiRootUrl + '/Guest/Reservation/UnauthSetBooking', {
                method: 'POST',
                headers: {
                    'Content-type': 'application/json'
                },
                body: JSON.stringify({ Booking: this.$store.state.bookingDetail, Language: this.$store.state.language })
            });

            let result = await response.json();

            if (result.ErrorMessage) {

                var notify = Metro.notify; notify.setup({ width: 300, timeout: this.$store.state.userSettings.notifyShowTime, duration: 500 });
                notify.create(result.ErrorMessage, "Error", { cls: "alert" }); notify.reset();

            } else {
                let password = result.Status;

                let response = await fetch(this.$store.state.apiRootUrl + '/Guest/WebLogin', {
                    method: 'post',
                    headers: {
                        'Authorization': 'Basic ' + encode(this.$store.state.bookingDetail.user.email + ":" + password),
                        'Content-type': 'application/json'
                    },
                    body: JSON.stringify({ language: this.$store.state.language })
                });
                let loginres = await response.json()
                if (loginres.message) {
                    var notify = Metro.notify; notify.setup({ width: 300, timeout: this.$store.state.userSettings.notifyShowTime, duration: 500 });
                    notify.create(loginres.message, "Error", { cls: "alert" }); notify.reset();
                } else {
                    this.$store.state.user = loginres;
                    this.$store.state.user.loggedIn = true;
                    var notify = Metro.notify; notify.setup({ width: 300, timeout: this.$store.state.userSettings.notifyShowTime, duration: 500 });
                    notify.create(this.$i18n.t("messages.loginSuccess"), "Info"); notify.reset();
                }

                //clear booking
                this.$store.dispatch('clearBooking');
            }
        }
    }
}
</script>
<style scoped>
    #orderPic {
        width: 100px;
        height: 100px;
    }
    .cardStyle{
        margin-top: 30px;
    }
</style>