// STARTUP Temp Variables Definitions
let pageLoader = null;
let pageLoaderRunningCounter = 0;
let partPageLoader = null;
let partPageLoaderRunningCounter = 0;

//background video & Startup Init
let bgVideo = function () { $('.player').mb_YTPlayer(); };
$(document).ready(function () { googleTranslateElementInit(); });


//Global Control For Show Page MD Help
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
                ShowNotify('error', result.ErrorMessage);
            } else {
                document.getElementById("DocView").srcdoc = result;
            }
            window.hidePageLoading();
        }, 100);
    }
}


/*Definitions  of Global System Behaviors */
function ChangeSchemeTo(n) {
    $("#color-scheme").attr("href", window.location.origin + "/src/assets/css/schemes/" + n);
    $("#scheme-name").html(n);
    Metro.storage.setItem('WebScheme', n);
}


//Hide Google Translate Browser Panel
document.addEventListener('click', function () {
    try {
        if (document.querySelector("body > div:nth-child(1)").className == "skiptranslate") {
            if (document.querySelector("body > div:nth-child(1)").style.display != "none") {
                document.querySelector("body > div:nth-child(1)").style.display = "none";
            }
        }
    } catch {}
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


//Global Hide Loading Indicator
function hidePageLoading() {
    pageLoaderRunningCounter--;
    if (pageLoaderRunningCounter <= 0) {
        pageLoaderRunningCounter = 0;

        if (pageLoader != undefined) {
            if (pageLoader[0]["DATASET:UID:M4Q"] == undefined) { pageLoader = null; }
            else { pageLoader = pageLoader[0]["DATASET:UID:M4Q"].dialog; pageLoader.close(); pageLoader = null; }
        }
        googleTranslateElementInit();
    }

}


//Global Show PartLoading Indicator
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


//Global Hide PartLoading Indicator
function hidePartPageLoading() {
    partPageLoaderRunningCounter--;
    if (partPageLoaderRunningCounter <= 0) {
        partPageLoaderRunningCounter = 0;
        if (partPageLoader != undefined) {
            if (partPageLoader[0]["DATASET:UID:M4Q"] == undefined) { partPageLoader = null; }
            else { partPageLoader = partPageLoader[0]["DATASET:UID:M4Q"].dialog; partPageLoader.close(); partPageLoader = null; }
        } 
        googleTranslateElementInit();
    }
}


//Global Implementation of Web Translation
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


//Global Cancel Translation to Default Webpage content
function CancelTranslation() {
    try {
        setTimeout(function () {
            let selectElement = document.querySelector('#google_translate_element select');
            selectElement.selectedIndex = 1;
            selectElement.dispatchEvent(new Event('change'));
            if (selectElement.value != '') {
                setTimeout(function () {
                    let selectElement = document.querySelector('#google_translate_element select');
                    selectElement.selectedIndex = 0;
                    selectElement.dispatchEvent(new Event('change'));
                    if (selectElement.value != '') {
                        setTimeout(function () {
                            let selectElement = document.querySelector('#google_translate_element select');
                            selectElement.selectedIndex = 0;
                            selectElement.dispatchEvent(new Event('change'));
                        }, 2000);
                    }
                }, 2000);
            }
        }, 1000);
    } catch { }
}


//Apply WebPages Main Settings
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
            if (secondstart) { setTimeout(function () { $('.player').YTPPlay(); }, 2000); }
        }, 2000);
    }
}


//Fuction Open Print Dialog for selected Element by Element Id
//Using jquery.printElement.js
function PrintElement(elementId) {
    try {
        $("#" + elementId).printElement({ pageTitle: elementId.split("_")[1] + '.html', printMode: 'popup' });
    } catch (err) { }
}


//Fuction Download Html Page selected Element by Element Id
function DownloadHtmlElement(elementId) {
    try {
        var a = document.body.appendChild(document.createElement("a"));
        a.download = elementId + ".html";
        a.href = "data:text/html;charset=utf-8," + encodeURIComponent(document.getElementById(elementId).innerHTML);
        a.click();
    } catch (err) { }
}


//Fuction Copy to Clipboard Html code selected Element by Element Id
async function CopyElement(elementId) {
    try {
        let text = document.getElementById(elementId).innerHTML;
        await navigator.clipboard.writeText(text);
    } catch (err) { }
}


//Fuction Generate PNG Image From selected Element by Element Id
function ImageFromElement(elementId) {
    try {
        $('document').ready(function () {
            html2canvas($("#" + elementId), {
                onrendered: function (canvas) {
                    $("#previewImage").append(canvas);
                    var imageData = canvas.toDataURL("image/png");
                    var newData = imageData.replace(/^data:image\/png/, "data:application/octet-stream");
                    var a = document.body.appendChild(document.createElement("a"));
                    a.download = elementId + ".png";
                    a.href = newData;
                    a.click();
                }
            });
        });
    } catch (err) { }
}


//Global Control Disable Window Scrolling
function disableScroll() {
    try {
        scrollTop = window.pageYOffset || document.documentElement.scrollTop;
        scrollLeft = window.pageXOffset || document.documentElement.scrollLeft,
        window.onscroll = function () { window.scrollTo(scrollLeft, scrollTop); };
    } catch { }
}


//Global Control Enable Window Scrolling
function enableScroll() {
    window.onscroll = function () { };
}


//Global Function string to Bytes array
window.str2bytes = function str2bytes(str) {
    var bytes = new Uint8Array(str.length);
    for (var i = 0; i < str.length; i++) {
        bytes[i] = str.charCodeAt(i);
    }
    return bytes;
}


//Global Controller For Expand/Collapse of All Collapse Elements
function ElementExpand(elementId) {
    try {
        let el = Metro.getPlugin('#' + elementId, 'collapse');
        let elStatus = el.isCollapsed();
        if (elStatus) { el.expand(); } else { el.collapsed(); }
    } catch { }
}


//Global Controller For Show/Hide Elements
function ElementShowHide(elementId,showOnly = false) {
    try {
        let el = Metro.get$el('#' + elementId);
        if (showOnly) { el.show(); }
        else if (el.style("display") == "none") { el.show(); } else { el.hide(); }
    } catch { }
}


//Global Init Standard SummernoteElement Init by Element Id
function ElementSummernoteInit(elementId) {
    try {
        $('#' + elementId).summernote({
            tabsize: 2, height: 250, maxHeight: 250,
            toolbar: [['style', ['style']], ['font', ['bold', 'underline', 'clear']], ['fontname', ['fontname']],
            ['fontsize', ['fontsize']], ['color', ['color']], ['para', ['ul', 'ol', 'paragraph']], ['table', ['table']],
            ['insert', ['link', 'picture', 'video']], ['view', ['fullscreen', 'undo', 'redo', 'help']]]
        });
        var newCss = {};
        newCss.backgroundColor = '#d6caba';
        $('.note-editable').css(newCss);
    } catch (err) { console.log("ElementSummernoteInit:",err) }
}


//Global Controller For Set Checkbox with true/false Value by Element Id
function ElementSetCheckBox(elementId, val) {
    try {
        $('#' + elementId).val('checked')[0].checked = JSON.parse((val.toString().toLowerCase()));
    } catch { }
}


//Global Controller For Set Active Class to Element by Element Id
function ElementSetActive(elementId) {
    try {
        $('#' + elementId).addClass(" active ");
    } catch { }
}


//Accordion Control Active CustomMenu In Header 
async function AccordionCustomMenuClick(elementId) {
    const result = await setTimeout(() => {
        if (!$('#' + elementId).parent()[0].classList.contains("active")) {
            $('#' + elementId).parent().addClass(' active ');
            setTimeout(() => { $('#' + elementId).parent().children()[1].style.display = "block"; }, 50);
        }
    }, 100);
}


//Global Control for Open/Close InfoBox By Element Id
function InfoBoxOpenClose(elementId) {
    try {
        if (Metro.infobox.isOpen('#' + elementId)) { Metro.infobox.close('#' + elementId); }
        else { Metro.infobox.open('#' + elementId); }
    } catch { }
}


//Global Control for Show Configured Notification 
function ShowNotify(type, message) {
    try {
        var notify = Metro.notify; notify.setup({ width: 300, timeout: Metro.storage.getItem('NotifyShowTime', 2000), duration: 500 });
        if (type == 'error') { notify.create(message, "Error", { cls: "alert" }); }
        else if (type == 'success') { notify.create(message, "Success", { cls: "success" }); }
        else if (type == 'info') { notify.create(message, "Info"); }
        notify.reset();
    } catch { }
}