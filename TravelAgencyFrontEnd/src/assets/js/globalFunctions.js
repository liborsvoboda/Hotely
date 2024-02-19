

//Set MaterialTab Background in Settings on Change
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


//Set MaterialTab Background in Messages on Change
function setBackgroundMessagesMenu() {
    let menuBackgroundColorName = 'bg-brandColor1';

    setTimeout(function () {
        if (window.$('#privateMessagesMenu').hasClass('active')) { window.$('#privateMessagesMenu')[0].classList.add(menuBackgroundColorName); } else { window.$('#privateMessagesMenu')[0].classList.remove(menuBackgroundColorName); }
        if (window.$('#reservationMessagesMenu').hasClass('active')) { window.$('#reservationMessagesMenu')[0].classList.add(menuBackgroundColorName); } else { window.$('#reservationMessagesMenu')[0].classList.remove(menuBackgroundColorName); }
    }, 10);
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


//InfoBox Screen Center Function on ContentChanged
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


//Generate Newsletter InfoBox
function PreparingNewsletter(dataset) {
    let messageData = "";
    dataset.forEach( record => {
        messageData += "<div id=\"newsletter_" + record.subject + "\" class=\"card image-header\"><div class=\"card-content p-2\"><p class=\"container fg-black\"><b>" + new Date(record.timeStamp).toLocaleDateString() + "</b> <div class=\"h1 w-50 text-left\" style=\"top: -25px;left:100px;\">" + record.subject + "</div></p>" + record.htmlMessage + "</div>";
        messageData += "<span title='" + window.dictionary('labels.print') + "' class=\"c-pointer mif-printer rounded pos-absolute drop-shadow fg-cyan mif-4x shadowed p-1 \" onclick=PrintElement('newsletter_" + record.subject + "') style=\"top: 5px; right: 5px\"></span>";
        messageData += "<span title='" + window.dictionary('labels.downloadHtml') + "' class=\"c-pointer mif-download2 rounded pos-absolute drop-shadow fg-cyan mif-4x shadowed p-1 \" onclick=DownloadHtmlElement('newsletter_" + record.subject + "') style=\"top: 5px; right: 55px\"></span>";
        messageData += "<span title='" + window.dictionary('labels.downloadImage') + "' class=\"c-pointer mif-image rounded pos-absolute drop-shadow fg-cyan mif-4x shadowed p-1 \" onclick=ImageFromElement('newsletter_" + record.subject + "') style=\"top: 5px; right: 105px\"></span>";
        messageData += "<span title='" + window.dictionary('labels.copy') + "' class=\"c-pointer mif-copy rounded pos-absolute drop-shadow fg-cyan mif-4x shadowed p-1 \" onclick=CopyElement('newsletter_" + record.subject + "') style=\"top: 5px; right: 155px\"></span></div>";
    });
    $("#NewsLetterBox").html(messageData);
}


