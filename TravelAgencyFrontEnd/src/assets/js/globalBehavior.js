//const { createApp, reactive } = Vue;

// STARTUP Temp Variables Definitions
let pageLoader = null;
let pageLoaderRunningCounter = 0;



/*Definitions  of Global System Behaviors */
function ChangeSchemeTo(n) {
    $("#color-scheme").attr("href", "../src/assets/css/schemes/" + n);
    $("#scheme-name").html(n);
    Metro.storage.setItem('WebScheme', n);
}


/*Start of Global Loading Indicator for All Pages*/
function showPageLoading() {
    pageLoaderRunningCounter++;
    if (pageLoader != undefined) { Metro.activity.close(pageLoader); }
    pageLoader = Metro.activity.open({
        type: 'atom',
        style: 'dark',
        /*overlayColor: '#fff',*/
        overlayClickClose: true,
        /*overlayAlpha: 1*/
    });
}

function hidePageLoading() {
    pageLoaderRunningCounter--;
    if (pageLoaderRunningCounter <= 0) {
        pageLoaderRunningCounter = 0;
        Metro.activity.close(pageLoader);
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
    console.log("checktranslate");
    try {
        new google.translate.TranslateElement({
            pageLanguage: 'cs',
            /*includedLanguages: 'en,cs',*/
            layout: google.translate.TranslateElement.InlineLayout.HORIZONTAL
        }, 'google_translate_element');

        if (Metro.storage.getItem('AutomaticTranslate', null) == true && Metro.storage.getItem('WebPagesLanguage', null) != null && document.querySelector('#google_translate_element select') != null) {

            console.log("translating", Metro.storage.getItem('WebPagesLanguage', null));
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


