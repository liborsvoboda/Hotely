<template>
    <Card id="card" style="top: 20px;" class=" mb-10">
        <template #content>
            <main class="container">
                <div class="row">
                    <div class="col-md-6 d-flex">
                        <div class="mt-2" data-role="hint" :data-hint-text="$t('labels.showArchivedDiscussion')" data-hint-position='bottom' data-cls-hint='text-bold drop-shadow'>
                            <input id="ShowArchivedDiscussion" data-role="checkbox" :onchange="ShowArchivedDiscussion" :checked="JSON.parse($store.state.tempVariables.showArchivedDiscussion.toString().toLowerCase())">
                        </div>
                        <h1>{{ $t('labels.discussionForum') }}</h1>
                    </div>
                    <div class="col-md-6 text-right">
                        <span class="icon mif-info pt-3 mif-3x c-pointer fg-orange" onclick="OpenDocView('Discussion')" />
                    </div>
                </div>

                <div class="row p-0 m-0">
                    <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12 pl-5 pr-5 pt-5 mb-0 bg-lightGray drop-shadow">
                        <div id="discussionForum" class=""></div>
                    </div>
                </div>
            </main>
        </template>
    </Card>
</template>


<script>
    import Card from "primevue/card";
    import { ref, watch } from 'vue';

export default {
    components: {
        Card,
    },
    data() {
        return {
            
        }
    },
    computed: {
        discussionForumList() {
            return this.$store.state.discussionForumList;
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
        async ShowArchivedDiscussion() {
            this.$store.state.tempVariables.showArchivedDiscussion = JSON.parse(this.$store.state.tempVariables.showArchivedDiscussion.toString().toLowerCase()) ? false : true;
            await this.GetDiscussionForumList();
        },
        async GetDiscussionForumList() {
            ElementSetCheckBox("ShowArchivedDiscussion", this.$store.state.tempVariables.showArchivedDiscussion);
            await this.$store.dispatch('getDiscussionForumList');
            this.GenerateDiscussionForumTree();
        },
        GenerateDiscussionForumTree() {
            let htmlContent = "<ul data-role='accordion' data-one-frame='true' data-show-active='false' >";

            let discussionList = [];this.$store.state.discussionForumList.forEach(discussion => { discussionList.push({ id: discussion.id, subject: discussion.subject }); });
            Metro.storage.setItem('DiscussionTemaList', discussionList);

            this.discussionForumList.forEach(discussion => {

                htmlContent += "<div class='frame'><div id='discussionFrame_" + discussion.id + "' class='heading row d-inline-flex w-100 bg-gray drop-shadow pl-4 pt-1 pr-1 pb-0 mb-2' style='opacity: 0.7;'><div class='h5 fg-black col-xl-3 col-lg-3 col-md-3 col-sm-3 col-12 p-1 m-0 text-left'>" + window.dictionary('labels.established') + ": " + new Date(discussion.timeStamp).toLocaleDateString('cs-CZ') + "</div>";
                htmlContent += "<div class='h5 fg-black col-xl-5 col-lg-5 col-md-5 col-sm-5 col-12 p-1 m-0 text-left'>" + discussion.subject + "</div>";
                htmlContent += "<div class='h5 fg-black col-xl-4 col-lg-4 col-md-4 col-sm-4 col-12 p-1 m-0 text-right'><span class='pos-absolute' style='left:0px;top:5px'>" + discussion.inverseMessageParent.length +"x " + "</span>";

                if (this.loggedIn) { htmlContent += "<span onclick=NewDiscussionInfoBox('" + discussion.id + "'); data-role='hint' data-hint-position='bottom' data-cls-hint='text-bold drop-shadow' data-hint-text='" + window.dictionary('labels.newDiscussionContribution') + "' class='mif-reply_all mif-2x  p-1 rounded bg-white fg-cyan drop-shadow shadowed c-pointer' ></span>"; }
                else { htmlContent += "<span onclick=FloatingLoginForm('" + discussion.id + "'); data-role='hint' data-hint-position='bottom' data-cls-hint='text-bold drop-shadow bg-red' data-hint-text='" + window.dictionary('labels.newDiscussionContribution') + (!this.loggedIn ? "\r\n" + window.dictionary('labels.youMustBeLogged') : "") + "' class='mif-reply_all mif-2x  p-1 rounded bg-white fg-cyan drop-shadow shadowed c-pointer' ></span>"; }

                htmlContent += "<span title='" + window.dictionary('labels.print') + "' class='c-pointer mif-printer rounded bg-white fg-cyan drop-shadow shadowed c-pointer mif-2x p-1 ml-1 ' onclick=\"AccordionCustomMenuClick('discussionFrame_" + discussion.id + "').then(()=>{ setTimeout(()=>{ PrintElement('discussionContent_" + discussion.id + "');},500);});\" ></span>";
                htmlContent += "<span title='" + window.dictionary('labels.downloadHtml') + "' class='c-pointer mif-download2  rounded bg-white fg-cyan drop-shadow shadowed c-pointer mif-2x p-1 ml-1 ' onclick=\"AccordionCustomMenuClick('discussionFrame_" + discussion.id + "');DownloadHtmlElement('discussionContent_" + discussion.id + "');\" ></span>";
                htmlContent += "<span title='" + window.dictionary('labels.downloadImage') + "' class='c-pointer mif-image  rounded bg-white fg-cyan drop-shadow shadowed c-pointer mif-2x p-1 ml-1 ' onclick=\"AccordionCustomMenuClick('discussionFrame_" + discussion.id + "');ImageFromElement('discussionContent_" + discussion.id + "');\" ></span>";
                htmlContent += "<span title='" + window.dictionary('labels.copy') + "' class='c-pointer mif-copy  rounded bg-white fg-cyan drop-shadow shadowed c-pointer mif-2x p-1 ml-1 ' onclick=\"AccordionCustomMenuClick('discussionFrame_" + discussion.id + "');CopyElement('discussionContent_" + discussion.id + "');\" ></span>";

                htmlContent += "</div></div>";
                htmlContent += "<div class='content border mb-5'><div id='discussionContent_" + discussion.id + "' >" + discussion.htmlMessage + "</div>";

                if (discussion.inverseMessageParent.length > 0) {
                    let htmlLevel1 = "";
                    discussion.inverseMessageParent.forEach(level1 => {
                        htmlLevel1 += '<div data-role="accordion" data-one-frame="true" data-show-active="false" class="pl-3">';
                        htmlLevel1 += '<div class="frame"><div id="discussionFrame_' + level1.id + '" class="heading row d-inline-flex w-100 pt-0 pb-0 pl-3 pr-1 mb-2 drop-shadow shadowed" style="opacity: 0.3; background-color:' + (level1.isSystemMessage ? '#1DA1F2' : '#86e22a') + '">';
                        htmlLevel1 += "<div class='fg-black col-xl-4 col-lg-4 col-md-4 col-sm-4 col-12 pl-3 m-0 text-left'>" + new Date(level1.timeStamp).toLocaleDateString('cs-CZ') + ": " + window.dictionary('labels.from') + ": " + level1.guest.firstName + " " + level1.guest.lastName + "</div>";
                        htmlLevel1 += "<div class='h5 fg-black col-xl-4 col-lg-4 col-md-4 col-sm-4 col-12 pl-3 m-0 text-left'>" + level1.subject + "</div>";
                        htmlLevel1 += "<div class='h5 fg-black col-xl-4 col-lg-4 col-md-4 col-sm-4 col-12 p-1 m-0 text-right'>";

                        htmlLevel1 += "<span title='" + window.dictionary('labels.print') + "' class='c-pointer mif-printer rounded bg-white fg-cyan drop-shadow shadowed c-pointer mif-1x p-1 ml-1 ' onclick=\"AccordionCustomMenuClick('discussionFrame_" + level1.id + "').then(()=>{ setTimeout(()=>{ PrintElement('discussionContent_" + level1.id + "');},500);});\" ></span>";
                        htmlLevel1 += "<span title='" + window.dictionary('labels.downloadHtml') + "' class='c-pointer mif-download2  rounded bg-white fg-cyan drop-shadow shadowed c-pointer mif-1x p-1 ml-1 ' onclick=\"AccordionCustomMenuClick('discussionFrame_" + level1.id + "');DownloadHtmlElement('discussionContent_" + level1.id + "');\" ></span>";
                        htmlLevel1 += "<span title='" + window.dictionary('labels.downloadImage') + "' class='c-pointer mif-image  rounded bg-white fg-cyan drop-shadow shadowed c-pointer mif-1x p-1 ml-1 ' onclick=\"AccordionCustomMenuClick('discussionFrame_" + level1.id + "');ImageFromElement('discussionContent_" + level1.id + "');\" ></span>";
                        htmlLevel1 += "<span title='" + window.dictionary('labels.copy') + "' class='c-pointer mif-copy  rounded bg-white fg-cyan drop-shadow shadowed c-pointer mif-1x p-1 ml-1 ' onclick=\"AccordionCustomMenuClick('discussionFrame_" + level1.id + "');CopyElement('discussionContent_" + level1.id + "');\" ></span>";

                        htmlLevel1 += "</div></div>";
                        htmlLevel1 += "<div class='content border mb-5'><div id='discussionContent_" + level1.id + "' >" + level1.htmlMessage + "</div></div>";

                        htmlLevel1 += "</div></div>";
                    });
                    htmlContent += htmlLevel1;
                }

                htmlContent += "</div></div></div></div>";

            }); htmlContent += "</ul>";
            $("#discussionForum").html(htmlContent);

        },
    },
    async created() {
        await this.GetDiscussionForumList();
        ElementSetCheckBox("ShowArchivedDiscussion", this.$store.state.tempVariables.showArchivedDiscussion);
        this.GenerateDiscussionForumTree();

        watch(window.watchGlobalVariables, async () => {
            if (window.watchGlobalVariables.modalLogin) {
                window.watchGlobalVariables.modalLogin = false;
                this.$store.state.user = Metro.storage.getItem('LoggedUser', null); Metro.storage.setItem('LoggedUser', null); this.$store.state.user.loggedIn = true;
                await this.$store.dispatch("getLightFavoriteHotelList");
                await this.$store.dispatch("getGuestSettingList");
                await this.$store.dispatch('getUnreadPrivateMessageCount');
                this.GenerateDiscussionForumTree();
                NewDiscussionInfoBox(Metro.storage.getItem('DiscussionParentId', null));
            }
            if (window.watchGlobalVariables.reloadDiscussionForum) {
                window.watchGlobalVariables.reloadDiscussionForum = false;
                ElementSetCheckBox("ShowArchivedDiscussion", this.$store.state.tempVariables.showArchivedDiscussion);
                await this.GetDiscussionForumList();
                this.GenerateDiscussionForumTree();
            }
        });

    }
};
</script>

<style scoped>
#card {
  border-radius: 20px;
  margin-top: 30px;
}

.bd-placeholder-img {
  font-size: 1.125rem;
  text-anchor: middle;
  -webkit-user-select: none;
  -moz-user-select: none;
  user-select: none;
}

@media (min-width: 768px) {
  .bd-placeholder-img-lg {
    font-size: 3.5rem;
  }
}
</style>