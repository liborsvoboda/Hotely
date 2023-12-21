//const { createApp, reactive } = Vue;

// STARTUP Temp Variables Definitions
let pageLoader = null;
let pageLoaderRunningCounter = 0;
let partPageLoader = null;
let partPageLoaderRunningCounter = 0;

//background video & Startup Init
let bgVideo = function () { $('.player').mb_YTPlayer(); };
$(document).ready(function () { googleTranslateElementInit(); });


async function OpenDocView(docname) {
    if (docname != null && docname.length > 0 && Metro.get$el($("#DocView"))[0] == undefined) {
        Metro.window.create({
            title: "Nápověda", shadow: true, draggable: true, modal: false, icon: "<span class=\"mif-info\"</span>",
            btnClose: true, width: 1000, height: 680, place: "top-center", btnMin: false, btnMax: false, clsWindow: "", dragArea: "#AppContainer",
            content: "<iframe id=\"DocView\" height=\"650\" style=\"width:100%;height:650px;\"></iframe>"
        });

        setTimeout(async function () {
            window.showPageLoading();
            let response = await fetch(
                Metro.storage.getItem('ApiRootUrl', null) + '/WebPages/GetWebDocumentationList/' + docname, {
                method: 'GET', headers: { 'Content-type': 'application/json' }
            }); let result = await response.json();

            if (result.Status == "error") {
                var notify = Metro.notify; notify.setup({ width: 300, timeout: Metro.storage.getItem('NotifyShowTime', null), duration: 500 });
                notify.create(result.ErrorMessage, "Error", { cls: "alert" }); notify.reset();
            } else {
                document.getElementById("DocView").srcdoc = result;
            }
            window.hidePageLoading();
        }, 100);
    }
}


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
        role: 'dialog', type: 'atom', style: 'dark',
        /*overlayColor: '#fff',overlayClickClose: true, overlayAlpha: 1*/
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


function googleTranslateElementInit() {
    $(document).ready(function () {
        try {
            new google.translate.TranslateElement({
                pageLanguage: 'cs', /*includedLanguages: 'en,cs',*/
                layout: google.translate.TranslateElement.InlineLayout.HORIZONTAL
            }, 'google_translate_element');

            // Anonymous User AutoTranslate
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
    });
}


function ApplyLoadedWebSetting() {
    if (Metro.storage.getItem('InputBanner', null) != null && Metro.storage.getItem('InputBanner', null).length > 0 && $("#SearchPanel")[0] != undefined ) { $("#SearchPanel")[0].style.backgroundImage = 'url(' + Metro.storage.getItem('InputBanner', null) + ')'; }
    if (Metro.storage.getItem('BackgroundImageSetting', null) != null && Metro.storage.getItem('BackgroundImageSetting', null).length > 0) { $("#app")[0].style.backgroundImage = 'url("' + Metro.storage.getItem('BackgroundImageSetting', null) + '")'; }
    if (Metro.storage.getItem('BackgroundVideoSetting', null) != null && Metro.storage.getItem('BackgroundVideoSetting', null).length > 0) {
        let secondstart = false;
        if (document.getElementsByClassName("YTPOverlay")[0] == undefined) { $(function () { bgVideo(); }); secondstart = true; } else {
            if (Metro.storage.getItem('BackgroundVideoSetting', null).indexOf($('.player')[0].videoID) == -1) {
                try { $('.player').YTPChangeMovie({ videoURL: Metro.storage.getItem('BackgroundVideoSetting', null), containment: '#body', showControls: false, autoPlay: true, loop: true, mute: true, startAt: 0, opacity: 1, quality: 'default', optimizeDisplay: true }); }
                catch { $("#backgroundPlayer")[0].dataset.property = "{ videoURL:'" + Metro.storage.getItem('BackgroundVideoSetting', null) + "', containment: '#body', showControls: false, autoPlay: true, loop: true, mute: true, startAt: 0, opacity: 1, quality: 'default', optimizeDisplay: true }"; }
            }
        }
        
        setTimeout(function () {
            if (secondstart) {
                if (Metro.storage.getItem('BackgroundVideoSetting', null).indexOf($('.player')[0].videoID) == -1) {
                    try { $('.player').YTPChangeMovie({ videoURL: Metro.storage.getItem('BackgroundVideoSetting', null), containment: '#body', showControls: false, autoPlay: true, loop: true, mute: true, startAt: 0, opacity: 1, quality: 'default', optimizeDisplay: true }); }
                    catch { $("#backgroundPlayer")[0].dataset.property = "{ videoURL:'" + Metro.storage.getItem('BackgroundVideoSetting', null) + "', containment: '#body', showControls: false, autoPlay: true, loop: true, mute: true, startAt: 0, opacity: 1, quality: 'default', optimizeDisplay: true }"; }
                }
            }
            try {
                document.getElementsByClassName("YTPOverlay")[0].style.opacity = (Metro.storage.getItem('BackgroundOpacitySetting', null) / 100);
                document.getElementsByClassName("YTPOverlay")[0].style.setProperty("background-color", Metro.storage.getItem('BackgroundColorSetting', null), "important");
            } catch { }
            $('.player').YTPPlay();
            if (secondstart) {
                console.log("secondstart");
                setTimeout(function () { $('.player').YTPPlay(); }, 2000);
            }
        }, 2000);
    }
}

