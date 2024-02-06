

//Send Private Message Answer API
async function SendMessageAnswer(parentId) {
    window.showPageLoading();
    let response = await fetch(
        Metro.storage.getItem('ApiRootUrl', null) + '/MessageModule/SetPrivateMessageAnswer', {
            method: 'POST', headers: { 'Authorization': 'Bearer ' + Metro.storage.getItem('Token', null), 'Content-type': 'application/json' },
            body: JSON.stringify({ ParentId: parentId, Message: $("#messageSummernote_" + parentId).summernote('code'), Language: Metro.storage.getItem('WebPagesLanguage', null) })
    }); let result = await response.json();
    if (result.Status == "error") {
        ShowNotify('error', result.ErrorMessage);
    } else {
        window.watchGlobalVariables.reloadPrivateMessage = true;
    }
    window.hidePageLoading();
}


//Set Comment Status API 
async function setCommentStatus(commentId) {
    window.showPageLoading();
    let response = await fetch(
        Metro.storage.getItem('ApiRootUrl', null) + '/Advertiser/SetCommentStatus/' + commentId + '/' + Metro.storage.getItem('WebPagesLanguage', null), {
        method: 'GET', headers: { 'Authorization': 'Bearer ' + Metro.storage.getItem('Token', null), 'Content-type': 'application/json' }
    }); let result = await response.json();
    if (result.Status == "error") {
        ShowNotify('error', result.ErrorMessage);
    } else {
        window.watchAdvertisementVariables.reloadAdvertisement = true;
    }
    window.hidePageLoading();
};

//Delete Comment Status API 
async function deleteComment(commentId) {
    window.showPageLoading();
    let response = await fetch(
        Metro.storage.getItem('ApiRootUrl', null) + '/Advertiser/DeleteComment/' + commentId + '/' + Metro.storage.getItem('WebPagesLanguage', null), {
        method: 'GET', headers: { 'Authorization': 'Bearer ' + Metro.storage.getItem('Token', null), 'Content-type': 'application/json' }
    }); let result = await response.json();
    if (result.Status == "error") {
        ShowNotify('error', result.ErrorMessage);
    } else {
        window.watchAdvertisementVariables.reloadAdvertisement = true;
    }
    window.hidePageLoading();
};

//Delete Advertiser UnAvailable Room Setting API 
async function deleteUnavailableRoom(recId) {
    window.showPageLoading();
    let response = await fetch(
        Metro.storage.getItem('ApiRootUrl', null) + '/Advertiser/DeleteUnavailableRoom/' + recId + '/' + Metro.storage.getItem('WebPagesLanguage', null), {
        method: 'GET', headers: { 'Authorization': 'Bearer ' + Metro.storage.getItem('Token', null), 'Content-type': 'application/json' }
    }); let result = await response.json();
    if (result.Status == "error") {
        ShowNotify('error', result.ErrorMessage);
    } else {
        window.watchAdvertisementVariables.reloadUnavailable = true;
    }
    window.hidePageLoading();
};




