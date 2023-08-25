//const { createApp, reactive } = Vue;

// STARTUP Temp Variables Definitions
let pageLoader = null;
let pageLoaderRunningCounter = 0;
let partPageLoader = null;
let partPageLoaderRunningCounter = 0;


/*Definitions  of Global System Behaviors */
function ChangeSchemeTo(n) {
    $("#color-scheme").attr("href", "../src/assets/css/schemes/" + n);
    $("#scheme-name").html(n);
    Metro.storage.setItem('WebScheme', n);
}


//Hide Google Translate Panel
document.addEventListener('click', function () {
    if (document.querySelector("body > div:nth-child(1)").className == "skiptranslate") {
        if (document.querySelector("body > div:nth-child(1)").style.display != "none") {
            document.querySelector("body > div:nth-child(1)").style.display = "none";
        }
    }
});

/*Start of Global Loading Indicator for All Pages*/
function showPageLoading() {
    pageLoaderRunningCounter++;
    if (pageLoader != undefined) {
        if (pageLoader[0]["DATASET:UID:M4Q"] == undefined) { pageLoader = null; }
        else { pageLoader = pageLoader[0]["DATASET:UID:M4Q"].dialog; pageLoader.close(); pageLoader = null;}
    }
    pageLoader = Metro.activity.open({
        role: 'dialog',
        type: 'atom',
        style: 'dark',
        /*overlayColor: '#fff',*/
        //overlayClickClose: true,
        /*overlayAlpha: 1*/
        text: '<div class=\'mt-2 text-small\'>' + window.dictionary('labels.loadingData') + '</div>',
    });
}

function hidePageLoading() {
    pageLoaderRunningCounter--;
    if (pageLoaderRunningCounter <= 0) {
        pageLoaderRunningCounter = 0;

        if (pageLoader != undefined) {
            if (pageLoader[0]["DATASET:UID:M4Q"] == undefined) { pageLoader = null; }
            else { pageLoader = pageLoader[0]["DATASET:UID:M4Q"].dialog; pageLoader.close(); pageLoader = null; }
        }//Metro.activity.close(pageLoader);

        googleTranslateElementInit();
    }

}

function showPartPageLoading() {
    partPageLoaderRunningCounter++;
    if (partPageLoader != undefined) {
        if (partPageLoader[0]["DATASET:UID:M4Q"] == undefined) { partPageLoader = null; }
        else { partPageLoader = partPageLoader[0]["DATASET:UID:M4Q"].dialog; partPageLoader.close(); partPageLoader = null; }
    }
    partPageLoader = Metro.activity.open({
        role: 'dialog',
        type: 'bars',
        style: 'dark',
        overlayColor: 'transparent',
        overlayClickClose: true,
        overlayAlpha: 0,
        text: '<div class=\'mt-2 text-small\'>' + window.dictionary('labels.loadingPartData') + '</div>',
    });
}
function hidePartPageLoading() {
    partPageLoaderRunningCounter--;
    if (partPageLoaderRunningCounter <= 0) {
        partPageLoaderRunningCounter = 0;
        if (partPageLoader != undefined) {
            if (partPageLoader[0]["DATASET:UID:M4Q"] == undefined) { partPageLoader = null; }
            else { partPageLoader = partPageLoader[0]["DATASET:UID:M4Q"].dialog; partPageLoader.close(); partPageLoader = null; }
        } //Metro.activity.close(partPageLoader);

        googleTranslateElementInit();
    }

}

$(document).ready(function () {
    googleTranslateElementInit();
});

document.addEventListener("load", function () {
    googleTranslateElementInit();
});

function googleTranslateElementInit() {
    
    try {
        new google.translate.TranslateElement({
            pageLanguage: 'cs',
            /*includedLanguages: 'en,cs',*/
            layout: google.translate.TranslateElement.InlineLayout.HORIZONTAL
        }, 'google_translate_element');

        if (Metro.storage.getItem('AutomaticTranslate', null) == true && Metro.storage.getItem('WebPagesLanguage', null) != null && document.querySelector('#google_translate_element select') != null) {

            setTimeout(function () {
                let selectElement = document.querySelector('#google_translate_element select');
                selectElement.value = Metro.storage.getItem('WebPagesLanguage', null);
                selectElement.dispatchEvent(new Event('change'));
            }, 1000);
            setTimeout(function () {
                document.querySelector("body > div:nth-child(1)").style.display = "none";
            }, 1000);
        }
    } catch (err) { }
}


