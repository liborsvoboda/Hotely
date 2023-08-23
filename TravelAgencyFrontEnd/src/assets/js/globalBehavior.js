//const { createApp, reactive } = Vue;

// STARTUP Temp Variables Definitions
let pageLoader;
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
    }

}

function googleTranslateElementInit() {
    new google.translate.TranslateElement({
        pageLanguage: 'en',
        includedLanguages: 'en,cs',
        layout: google.translate.TranslateElement.InlineLayout.HORIZONTAL
    }, 'google_translate_element');

    let userTranslateSetting = Metro.storage.getItem('UserConfig', null) != null && Metro.storage.getItem('UserConfig', null).UserAutoTranslate ? true : false;
    if (userTranslateSetting) {
        setTimeout(function () {
            let selectElement = document.querySelector('#google_translate_element select');
            selectElement.value = 'cs';
            selectElement.dispatchEvent(new Event('change'));
        }, 1000);
    }
}

