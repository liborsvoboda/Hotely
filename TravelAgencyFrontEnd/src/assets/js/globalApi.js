
//Web Login for Users API
async function FloatingLogin() {
    window.showPageLoading();
    let response = await fetch(Metro.storage.getItem('ApiRootUrl', null) + '/Guest/WebLogin', {
        method: 'POST',
        headers: { 'Authorization': 'Basic ' + btoa($("#FloatingLoginEmail").val() + ":" + $("#FloatingLoginPassword").val()), 'Content-type': 'application/json' },
        body: JSON.stringify({ language: Metro.storage.getItem('WebPagesLanguage', null) })
    }); let result = await response.json();

    window.hidePageLoading();
    if (result.message) { ShowNotify('error', result.message); }
    else {
        $("#FloatingLoginForm").hide();
        Metro.storage.setItem('Token', result.Token); Metro.storage.setItem('LoggedUser', result);
        window.watchGlobalVariables.modalLogin = true;
    }
}


//Send Discussion Contribution API
async function SendDiscussionContribution() {
    window.showPageLoading();
    let response = await fetch(
        Metro.storage.getItem('ApiRootUrl', null) + '/MessageModule/SetDiscussionContribution', {
        method: 'POST', headers: { 'Authorization': 'Bearer ' + Metro.storage.getItem('Token', null), 'Content-type': 'application/json' },
            body: JSON.stringify({ ParentId: $("#newDiscussionSelectionList").val(), Subject: $("#newDiscussionTitle").val(), Message: $("#newDiscussionSummernote").summernote('code'), Language: Metro.storage.getItem('WebPagesLanguage', null) })
    }); let result = await response.json();
    if (result.Status == "error") {
        ShowNotify('error', result.ErrorMessage);
    } else { window.watchGlobalVariables.reloadDiscussionForum = true; }
    window.hidePageLoading();
}


//Send Private Message Answer API
async function SendPrivateMessageAnswer(parentId) {
    window.showPageLoading();
    let response = await fetch(
        Metro.storage.getItem('ApiRootUrl', null) + '/MessageModule/SetPrivateMessageAnswer', {
            method: 'POST', headers: { 'Authorization': 'Bearer ' + Metro.storage.getItem('Token', null), 'Content-type': 'application/json' },
            body: JSON.stringify({ ParentId: parentId, Message: $("#messageSummernote_" + parentId).summernote('code'), Language: Metro.storage.getItem('WebPagesLanguage', null) })
    }); let result = await response.json();
    if (result.Status == "error") { ShowNotify('error', result.ErrorMessage);
    } else { window.watchGlobalVariables.reloadPrivateMessage = true; }
    window.hidePageLoading();
}


//Archive Private Message Tree API
async function ArchivePrivateMessage(messageId) {
    window.showPageLoading();
    let response = await fetch(
        Metro.storage.getItem('ApiRootUrl', null) + '/MessageModule/ArchivePrivateMessage/' + messageId + "/" + Metro.storage.getItem('WebPagesLanguage', null), {
        method: 'GET', headers: { 'Authorization': 'Bearer ' + Metro.storage.getItem('Token', null), 'Content-type': 'application/json' }
    }); let result = await response.json();
    if (result.Status == "error") { ShowNotify('error', result.ErrorMessage);
    } else { window.watchGlobalVariables.reloadPrivateMessage = true; }
    window.hidePageLoading();
}


//Set As Readed Specific Private Message API
async function SetShownPrivateMessage(messageId) {
    window.showPageLoading();
    let response = await fetch(
        Metro.storage.getItem('ApiRootUrl', null) + '/MessageModule/SetShownPrivateMessage/' + messageId + "/" + Metro.storage.getItem('WebPagesLanguage', null), {
        method: 'GET', headers: { 'Authorization': 'Bearer ' + Metro.storage.getItem('Token', null), 'Content-type': 'application/json' }
    }); let result = await response.json();
    if (result.Status == "error") {
        ShowNotify('error', result.ErrorMessage);
    } else { window.watchGlobalVariables.reloadPrivateMessage = true; }
    window.hidePageLoading();
}


//Set Comment Status API 
async function setCommentStatus(commentId) {
    window.showPageLoading();
    let response = await fetch(
        Metro.storage.getItem('ApiRootUrl', null) + '/Advertiser/SetCommentStatus/' + commentId + '/' + Metro.storage.getItem('WebPagesLanguage', null), {
        method: 'GET', headers: { 'Authorization': 'Bearer ' + Metro.storage.getItem('Token', null), 'Content-type': 'application/json' }
    }); let result = await response.json();
    if (result.Status == "error") { ShowNotify('error', result.ErrorMessage);
    } else { window.watchAdvertisementVariables.reloadAdvertisement = true; }
    window.hidePageLoading();
};


//Delete Comment Status API 
async function deleteComment(commentId) {
    window.showPageLoading();
    let response = await fetch(
        Metro.storage.getItem('ApiRootUrl', null) + '/Advertiser/DeleteComment/' + commentId + '/' + Metro.storage.getItem('WebPagesLanguage', null), {
        method: 'GET', headers: { 'Authorization': 'Bearer ' + Metro.storage.getItem('Token', null), 'Content-type': 'application/json' }
    }); let result = await response.json();
    if (result.Status == "error") { ShowNotify('error', result.ErrorMessage);
    } else { window.watchAdvertisementVariables.reloadAdvertisement = true; }
    window.hidePageLoading();
};


//Delete Advertiser UnAvailable Room Setting API 
async function deleteUnavailableRoom(recId) {
    window.showPageLoading();
    let response = await fetch(
        Metro.storage.getItem('ApiRootUrl', null) + '/Advertiser/DeleteUnavailableRoom/' + recId + '/' + Metro.storage.getItem('WebPagesLanguage', null), {
        method: 'GET', headers: { 'Authorization': 'Bearer ' + Metro.storage.getItem('Token', null), 'Content-type': 'application/json' }
    }); let result = await response.json();
    if (result.Status == "error") { ShowNotify('error', result.ErrorMessage);
    } else { window.watchAdvertisementVariables.reloadUnavailable = true; }
    window.hidePageLoading();
};




