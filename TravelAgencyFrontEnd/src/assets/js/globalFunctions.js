
function disableScroll() {
    scrollTop = window.pageYOffset || document.documentElement.scrollTop;
    scrollLeft = window.pageXOffset || document.documentElement.scrollLeft,

    window.onscroll = function () {
        window.scrollTo(scrollLeft, scrollTop);
    };
}

function enableScroll() {
    window.onscroll = function () { };
}

function setBackgroundProfileMenu() {
    let menuBackgroundColorName = 'bg-brandColor1';

    setTimeout(function () {
        if (window.$('#personalDetailsMenu').hasClass('active')) { window.$('#personalDetailsMenu')[0].classList.add(menuBackgroundColorName); } else { window.$('#personalDetailsMenu')[0].classList.remove(menuBackgroundColorName); }
        if (window.$('#addressMenu').hasClass('active')) { window.$('#addressMenu')[0].classList.add(menuBackgroundColorName); } else { window.$('#addressMenu')[0].classList.remove(menuBackgroundColorName); }
        if (window.$('#actualOrNewPasswordMenu').hasClass('active')) { window.$('#actualOrNewPasswordMenu')[0].classList.add(menuBackgroundColorName); } else { window.$('#actualOrNewPasswordMenu')[0].classList.remove(menuBackgroundColorName); }
        if (window.$('#advertisingMenu').hasClass('active')) { window.$('#advertisingMenu')[0].classList.add(menuBackgroundColorName); } else { window.$('#advertisingMenu')[0].classList.remove(menuBackgroundColorName); }
        if (window.$('#settingsMenu').hasClass('active')) { window.$('#settingsMenu')[0].classList.add(menuBackgroundColorName); } else { window.$('#settingsMenu')[0].classList.remove(menuBackgroundColorName); }
    }, 10);
};

async function setCommentStatus(commentId, apiUrl) {
    window.showPageLoading();
    let response = await fetch(
        apiUrl + '/Advertiser/SetCommentStatus/' + commentId + '/' + Metro.storage.getItem('WebPagesLanguage', null), {
            method: 'GET', headers: { 'Authorization': 'Bearer ' + Metro.storage.getItem('Token', null), 'Content-type': 'application/json' }
    }); let result = await response.json();
    if (result.Status == "error") {
        var notify = Metro.notify; notify.setup({ width: 300, timeout: 2000, duration: 500 });
        notify.create(result.ErrorMessage, "Error", { cls: "alert" }); notify.reset();
    } else {
        window.watchAdvertisementVariables.reloadAdvertisement = true;
    }
    window.hidePageLoading();
};

async function deleteComment(commentId, apiUrl) {
    window.showPageLoading();
    let response = await fetch(
        apiUrl + '/Advertiser/DeleteComment/' + commentId + '/' + Metro.storage.getItem('WebPagesLanguage', null), {
        method: 'GET', headers: { 'Authorization': 'Bearer ' + Metro.storage.getItem('Token', null), 'Content-type': 'application/json' }
    }); let result = await response.json();
    if (result.Status == "error") {
        var notify = Metro.notify; notify.setup({ width: 300, timeout: 2000, duration: 500 });
        notify.create(result.ErrorMessage, "Error", { cls: "alert" }); notify.reset();
    } else {
        window.watchAdvertisementVariables.reloadAdvertisement = true;
    }
    window.hidePageLoading();
};


window.str2bytes =function str2bytes(str) {
    var bytes = new Uint8Array(str.length);
    for (var i = 0; i < str.length; i++) {
        bytes[i] = str.charCodeAt(i);
    }
    return bytes;
}
