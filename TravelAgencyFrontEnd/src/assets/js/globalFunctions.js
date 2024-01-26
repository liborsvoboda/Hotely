
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


async function deleteUnavailableRoom(recId, apiUrl) {
    window.showPageLoading();
    let response = await fetch(
        apiUrl + '/Advertiser/DeleteUnavailableRoom/' + recId + '/' + Metro.storage.getItem('WebPagesLanguage', null), {
        method: 'GET', headers: { 'Authorization': 'Bearer ' + Metro.storage.getItem('Token', null), 'Content-type': 'application/json' }
    }); let result = await response.json();
    if (result.Status == "error") {
        var notify = Metro.notify; notify.setup({ width: 300, timeout: 2000, duration: 500 });
        notify.create(result.ErrorMessage, "Error", { cls: "alert" }); notify.reset();
    } else {
        window.watchAdvertisementVariables.reloadUnavailable = true;
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


//Closing PriceList Other Groups on Panel Expand
//filled on loaded groups 
//window.propertyGroupList = []; 
function CloseOtherPriceListGroups(element) {
    let pricePanel = null;
    window.propertyGroupList.forEach(group => {
        if (group.sequence != element.id.split("_")[1]) {
            pricePanel = Metro.getPlugin('#priceGroup_' + group.sequence, 'panel');
            if (pricePanel != undefined) { pricePanel.collapse(); }
        }
    });
    //more filters
    if (1000000 != element.id.split("_")[1]) {
        pricePanel = Metro.getPlugin('#priceGroup_1000000', 'panel'); if (pricePanel != undefined) { pricePanel.collapse(); }
    }
}


function PriceInfoBoxSwitchView() {
    let el = Metro.getPlugin($("#PriceInfoBox"), 'info-box');
    if (el != undefined) {
        if ($("#PriceInfoBoxSwitch").html() == window.dictionary('labels.list')) {
            $("#PriceInfoBoxSwitch").html(window.dictionary('labels.groups'));
            el.element.width("60%");
            $("#PriceInfoGroups")[0].style.display = 'none'; $("#PriceInfoList")[0].style.display = 'inline';
        } else {
            $("#PriceInfoBoxSwitch").html(window.dictionary('labels.list'));
            el.element.width("400");
            $("#PriceInfoList")[0].style.display = 'none'; $("#PriceInfoGroups")[0].style.display = 'inline';
        }
        el.reposition();
    }
}


function PreparingNewsletter(dataset) {
    console.log("pripravuji", dataset);
    let messageData = "";
    dataset.forEach((record, index) => {
        messageData += "<div id=\"" + record.subject + "\" class=\"card image-header\"><div class=\"card-content p-2\"><p class=\"container fg-black\"><b>" + new Date(record.timeStamp).toLocaleDateString() + "</b> <div class=\"h1 text-center\" style=\"top: -25px;\">" + record.subject + "</div></p>" + record.htmlMessage + "</div>";
        messageData += "<span title='" + window.dictionary('labels.print') + "' class=\"c-pointer mif-printer rounded pos-absolute drop-shadow fg-cyan mif-4x drop-shadow shadowed p-1 \" onclick=PrintElement('" + record.subject + "') style=\"top: 5px; right: 5px\"></span>";
        messageData += "<span title='" + window.dictionary('labels.download') + "' class=\"c-pointer mif-download2 rounded pos-absolute drop-shadow fg-cyan mif-4x drop-shadow shadowed p-1 \" onclick=DownloadElement('" + record.subject + "') style=\"top: 5px; right: 55px\"></span>";
        messageData += "<span title='" + window.dictionary('labels.copy') + "' class=\"c-pointer mif-copy rounded pos-absolute drop-shadow fg-cyan mif-4x drop-shadow shadowed p-1 \" onclick=CopyElement('" + record.subject + "') style=\"top: 5px; right: 105px\"></span></div>";
    });
    $("#NewsLetterBox").html(messageData);
}