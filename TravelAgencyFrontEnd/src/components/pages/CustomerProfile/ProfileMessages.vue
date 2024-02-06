<template>
    <div>
        <div class="rounded drop-shadow row">
            <div class="col-md-6 d-flex">

                <div class="mt-2" data-role="hint" :data-hint-text="$t('labels.showArchive')">
                    <input id="showArchivedMessages" type="checkbox" data-role="checkbox" :onchange="ShowArchivedMessages" :checked="userSettings.showArchivedMessages">
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
                <ul data-role="tabs" data-expand="true" data-on-tab="setBackgroundMessagesMenu()">
                    <li id="privateMessagesMenu" class="fg-black text-bold bg-brandColor1" @click="GetPrivateMessageList"><a href="#_privateMessagesMenu">{{ $t('labels.privateMessages') }}</a></li>
                    <li id="reservationMessagesMenu" class="fg-black text-bold" @click="GetReservatinMessageList"><a href="#_reservationMessagesMenu">{{ $t('labels.reservationMessages') }}</a></li>
                </ul>
            </div>

            <div id="_privateMessagesMenu">
                <div id="privateMessageTree" class="p-5"></div>
            </div>

            <div id="_reservationMessagesMenu">
                <div class="d-flex row gutters ml-5 mr-5 mb-5 border">
                    <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12">
                        <h6 class="mt-3 mb-2 text-primary">{{ $t('labels.reservationMessages') }}</h6>
                    </div>

                    <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12">
                        <div class="form-group p-3 pb-0">
                            <label for="form-group FirstName">{{ $t('labels.firstname') }}</label>
                            <input type="text" class="form-control" id="FirstName" autocomplete="off" />
                        </div>
                    </div>
                </div>
            </div>
        </div>


    </div>
</template>

<script>
    import { ref, watch } from 'vue';

export default {
    components: {},
    data() {
        return {
            selectedSet: 'private',
        };
    },
    computed: {
        privateMessageList() {
            return this.$store.state.privateMessageList;
        },
        reservationMessageList() {
            return this.$store.state.reservationMessageList;
        },
        userSettings() {
            return this.$store.state.userSettings;
        },
        loggedIn() {
            return this.$store.state.user.loggedIn;
        },
        user() {
            return this.$store.state.user;
        },
    },
    async mounted() {

    },
    methods: {
        ShowHideMessage(id) {
            console.log("now");

        },
        async ShowArchivedMessages() {
            this.$store.state.userSettings.showArchivedMessages = !this.$store.state.userSettings.showArchivedMessages;
            if (this.selectedSet == 'private') { await this.GetPrivateMessageList(); }
            else if (this.selectedSet == 'reservation') { await this.GetReservatinMessageList(); }
        },
        async GetPrivateMessageList() {
            this.selectedSet = 'private';
            await this.$store.dispatch('getPrivateMessageList');
            this.GeneratePrivateMessageTree();
        },
        async GetReservatinMessageList() {
            this.selectedSet = 'reservation';
            await this.$store.dispatch('getReservationMessageList');
        },
        GeneratePrivateMessageTree() {
            let ignoredMessageIds = [];
            let htmlContent = "<ul data-role='accordion' data-one-frame='true' data-show-active='true' >";
            this.privateMessageList.forEach(message => {

                if (message.messageParentId != null) { ignoredMessageIds.push([message.messageParentId]); }
                console.log("checking", ignoredMessageIds, ignoredMessageIds.includes(message.id))
            
                if (!ignoredMessageIds.includes(message.id)) {
                    htmlContent += "<div class='frame'><div class='heading row d-inline-flex w-100'><div class='h5 fg-black col-xl-3 col-lg-3 col-md-3 col-sm-3 col-12 p-1 m-0 text-left'>" + new Date(message.timeStamp).toLocaleString('cs-CZ') + "</div>";
                    htmlContent += "<div class='h5 fg-black col-xl-3 col-lg-3 col-md-3 col-sm-3 col-12 p-1 m-0 text-left'>" + message.subject + "</div>";
                    htmlContent += "<div class='h5 fg-black col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12 p-1 m-0 text-right'><span onclick=\"ElementSummernoteInit('messageSummernote_" + message.id + "');ElementShowHide('messageSendButton_" + message.id + "') \" data-role='hint' data-hint-position='bottom' data-cls-hint='text-bold drop-shadow' data-hint-text='" + window.dictionary('labels.writeAnswer') + "' class='mif-reply_all mif-1x ani-hover-heartbeat'></span></div></div>";
                    htmlContent += "<div class='content border'>" + message.htmlMessage + "<div id='messageSummernote_" + message.id + "' style='visibility: hidden;'></div>";
                    htmlContent += "<button id='messageSendButton_" + message.id + "' class='pos-absolute button success outline shadowed mr-2' style='right: 0px;bottom: 15px;display:none;' type='button' onclick=SendMessageAnswer('" + message.id + "') > " + window.dictionary('labels.sendAnswer') + "</button></div></div>";
                }

            }); htmlContent += "</ul>";
            $("#privateMessageTree").html(htmlContent);

        },
    
    },
    async created() {
        await this.$store.dispatch('getPrivateMessageList');
        this.GeneratePrivateMessageTree();

        watch(window.watchGlobalVariables, async () => {
            if (window.watchGlobalVariables.reloadPrivateMessage) {
                window.watchGlobalVariables.reloadPrivateMessage = false;
                await this.$store.dispatch('getPrivateMessageList');
                this.GeneratePrivateMessageTree();
            }
        });
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
