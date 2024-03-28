<template>
    <div>
        <div class="rounded drop-shadow row">
            <div class="col-md-6 d-flex">

                <div class="mt-2" data-role="hint" :data-hint-text="$t('labels.showArchive')" data-hint-position='bottom' :data-cls-hint="hintPopupClass + ' drop-shadow'" >
                    <input id="ShowArchivedMessages" data-role="checkbox" :onchange="ShowArchivedMessages" :checked="JSON.parse($store.state.userSettings.showArchivedMessages.toString().toLowerCase())">
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
                <div id="reservationMessageList" class="p-5"></div>
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
        loggedIn() {
            return this.$store.state.user.loggedIn;
        },
        user() {
            return this.$store.state.user;
        },
        hintPopupClass() {
            return Metro.storage.getItem('OnMousePopupClasses', 'bg-cyan fg-white');
        }
    },
    async mounted() {

    },
    methods: {
        async ShowArchivedMessages() {
            this.$store.state.userSettings.showArchivedMessages = JSON.parse(this.$store.state.userSettings.showArchivedMessages.toString().toLowerCase()) ? false : true;
            if (this.selectedSet == 'private') { await this.GetPrivateMessageList(); }
            else if (this.selectedSet == 'reservation') { await this.GetReservatinMessageList(); }
        },
        async GetPrivateMessageList() {
            this.selectedSet = 'private';
            ElementSetCheckBox("ShowArchivedMessages", this.$store.state.userSettings.showArchivedMessages);
            await this.$store.dispatch('getPrivateMessageList');
            this.GeneratePrivateMessageTree();
        },
        async GetReservatinMessageList() {
            this.selectedSet = 'reservation';
            ElementSetCheckBox("ShowArchivedMessages", this.$store.state.userSettings.showArchivedMessages);
            await this.$store.dispatch('getReservationMessageList');
        },
        GeneratePrivateMessageTree() {
            let htmlContent = "<ul data-role='accordion' data-one-frame='true' data-show-active='false' >";
            this.privateMessageList.forEach(message => {

                htmlContent += "<div class='frame'><div id='messageFrame_" + message.id + "' class='heading row d-inline-flex w-100' style='opacity: 0.7;background-color:" + (message.archived ? '#41545e' : !message.shown && message.isSystemMessage ? '#a31212cf' : message.shown && message.isSystemMessage ? '#1DA1F2' : '#86e22a') + "'><div class='h5 fg-black col-xl-3 col-lg-3 col-md-3 col-sm-3 col-12 p-1 m-0 text-left'>" + new Date(message.timeStamp).toLocaleString('cs-CZ') + "</div>";
                htmlContent += "<div class='h5 fg-black col-xl-3 col-lg-3 col-md-3 col-sm-3 col-12 p-1 m-0 text-left'>" + message.subject + "</div>";
                htmlContent += "<div class='h5 fg-black col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12 p-1 m-0 text-right'>";

                if (!message.shown && message.isSystemMessage) { htmlContent += "<span onclick=\"AccordionCustomMenuClick('messageFrame_" + message.id + "').then(()=>{ SetShownPrivateMessage('" + message.id + "');});\" data-role='hint' data-hint-position='bottom' data-cls-hint='" + Metro.storage.getItem('OnMousePopupClasses', 'bg-cyan fg-white') + " drop-shadow' data-hint-text='" + window.dictionary('labels.setAsReaded') + "' class='mif-eye mif-2x mr-2 p-1 rounded bg-white fg-cyan drop-shadow shadowed c-pointer'></span>"; }
                htmlContent += "<span onclick=\"AccordionCustomMenuClick('messageFrame_" + message.id + "').then(()=>{ setTimeout(()=>{ ElementSummernoteInit('messageSummernote_" + message.id + "');ElementShowHide('messageSendButton_" + message.id + "', true);},500);});\" data-role='hint' data-hint-position='bottom' data-cls-hint='" + Metro.storage.getItem('OnMousePopupClasses', 'bg-cyan fg-white') + " drop-shadow' data-hint-text='" + window.dictionary('labels.newPrivateMessage') + "' class='mif-reply_all mif-2x  p-1 rounded bg-white fg-cyan drop-shadow shadowed c-pointer'></span>";
                if (!message.archived) { htmlContent += "<span onclick=\"AccordionCustomMenuClick('messageFrame_" + message.id + "');ArchivePrivateMessage('" + message.id + "');\" data-role='hint' data-hint-position='bottom' data-cls-hint='" + Metro.storage.getItem('OnMousePopupClasses', 'bg-cyan fg-white') + " drop-shadow' data-hint-text='" + window.dictionary('labels.archive') + "' class='mif-shrink mif-2x ml-2 p-1 rounded bg-white fg-cyan drop-shadow shadowed c-pointer'></span>"; }

                htmlContent += "<span title='" + window.dictionary('labels.print') + "' class='c-pointer mif-printer rounded bg-white fg-cyan drop-shadow shadowed c-pointer mif-2x p-1 ml-1 ' onclick=\"AccordionCustomMenuClick('messageFrame_" + message.id + "').then(()=>{ setTimeout(()=>{ PrintElement('messageContent_" + message.id + "');},500);});\" ></span>";
                htmlContent += "<span title='" + window.dictionary('labels.downloadHtml') + "' class='c-pointer mif-download2  rounded bg-white fg-cyan drop-shadow shadowed c-pointer mif-2x p-1 ml-1 ' onclick=\"AccordionCustomMenuClick('messageFrame_" + message.id + "');DownloadHtmlElement('messageContent_" + message.id + "');\" ></span>";
                htmlContent += "<span title='" + window.dictionary('labels.downloadImage') + "' class='c-pointer mif-image  rounded bg-white fg-cyan drop-shadow shadowed c-pointer mif-2x p-1 ml-1 ' onclick=\"AccordionCustomMenuClick('messageFrame_" + message.id + "');ImageFromElement('messageContent_" + message.id + "');\" ></span>";
                htmlContent += "<span title='" + window.dictionary('labels.copy') + "' class='c-pointer mif-copy  rounded bg-white fg-cyan drop-shadow shadowed c-pointer mif-2x p-1 ml-1 ' onclick=\"AccordionCustomMenuClick('messageFrame_" + message.id + "');CopyElement('messageContent_" + message.id + "');\" ></span>";


                htmlContent += "</div></div>";
                htmlContent += "<div class='content border pb-3'><div id='messageContent_" + message.id + "' >" + message.htmlMessage + "</div><div id='messageSummernote_" + message.id + "' style='visibility: hidden;'>" + window.dictionary('labels.hereWriteMessage') + "</div>";
                htmlContent += "<button id='messageSendButton_" + message.id + "' class='pos-relative button success outline shadowed mr-2' style='left: 5px;bottom: 50px;display:none;' type='button' onclick=SendPrivateMessageAnswer('" + message.id + "') > " + window.dictionary('labels.sendAnswer') + "</button>";

                if (message.inverseMessageParent.length > 0) { 
                    let htmlLevel1 = "";
                    message.inverseMessageParent.forEach(level1 => {
                        htmlLevel1 += '<div data-role="accordion" data-one-frame="true" data-show-active="false" class="pl-3">';
                        htmlLevel1 += '<div class="frame"><div id="messageFrame_' + level1.id + '" class="heading row d-inline-flex w-100 pt-0 pb-0 pl-3"  style="opacity: 0.7; background-color:' + (!level1.shown && level1.isSystemMessage ? '#a31212cf' : level1.shown && level1.isSystemMessage ? '#1DA1F2' : '#86e22a') + '">';
                        htmlLevel1 += "<div class='h5 fg-black col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12 pl-3 m-0 text-left'>" + level1.subject + "</div>";
                        htmlLevel1 += "<div class='h5 fg-black col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12 p-1 m-0 text-right'>";
                        
                        if (!level1.shown && level1.isSystemMessage) { htmlLevel1 += "<span onclick=\"SetShownPrivateMessage('" + level1.id + "');\" data-role='hint' data-hint-position='bottom' data-cls-hint='" + Metro.storage.getItem('OnMousePopupClasses', 'bg-cyan fg-white') + " drop-shadow' data-hint-text='" + window.dictionary('labels.setAsReaded') + "' class='mif-eye mif-1x mr-2 p-1 rounded bg-white fg-cyan drop-shadow shadowed c-pointer'></span>"; }
                        htmlLevel1 += "<span title='" + window.dictionary('labels.print') + "' class='c-pointer mif-printer rounded bg-white fg-cyan drop-shadow shadowed c-pointer mif-1x p-1 ml-1 ' onclick=\"AccordionCustomMenuClick('messageFrame_" + level1.id + "').then(()=>{ setTimeout(()=>{ PrintElement('messageContent_" + level1.id + "');},500);});\" ></span>";
                        htmlLevel1 += "<span title='" + window.dictionary('labels.downloadHtml') + "' class='c-pointer mif-download2  rounded bg-white fg-cyan drop-shadow shadowed c-pointer mif-1x p-1 ml-1 ' onclick=\"AccordionCustomMenuClick('messageFrame_" + level1.id + "');DownloadHtmlElement('messageContent_" + level1.id + "');\" ></span>";
                        htmlLevel1 += "<span title='" + window.dictionary('labels.downloadImage') + "' class='c-pointer mif-image  rounded bg-white fg-cyan drop-shadow shadowed c-pointer mif-1x p-1 ml-1 ' onclick=\"AccordionCustomMenuClick('messageFrame_" + level1.id + "');ImageFromElement('messageContent_" + level1.id + "');\" ></span>";
                        htmlLevel1 += "<span title='" + window.dictionary('labels.copy') + "' class='c-pointer mif-copy  rounded bg-white fg-cyan drop-shadow shadowed c-pointer mif-1x p-1 ml-1 ' onclick=\"AccordionCustomMenuClick('messageFrame_" + level1.id + "');CopyElement('messageContent_" + level1.id + "');\" ></span>";

                        htmlLevel1 += "</div></div>";
                        htmlLevel1 += "<div class='content border'><div id='messageContent_" + level1.id + "' >" + level1.htmlMessage + "</div></div>";

                        htmlLevel1 += "</div></div>";
                    });
                    htmlContent += htmlLevel1;
                }

                htmlContent += "</div></div></div></div>";

            }); htmlContent += "</ul>";
            $("#privateMessageTree").html(htmlContent);

        },
    
    },
    async created() {
        await this.GetPrivateMessageList();
        ElementSetCheckBox("ShowArchivedMessages", this.$store.state.userSettings.showArchivedMessages);
        this.GeneratePrivateMessageTree();


        watch(window.watchGlobalVariables, async () => {
            if (window.watchGlobalVariables.reloadPrivateMessage) {
                window.watchGlobalVariables.reloadPrivateMessage = false;
                ElementSetCheckBox("ShowArchivedMessages", this.$store.state.userSettings.showArchivedMessages);
                await this.GetPrivateMessageList();
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
