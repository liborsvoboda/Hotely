

function ScrollToTop() { window.scrollTo(0, 0); }
function enableScroll() { window.onscroll = function () { }; }
function disableScroll() {
    scrollTop = window.pageYOffset || document.documentElement.scrollTop;
    scrollLeft = window.pageXOffset || document.documentElement.scrollLeft,
        window.onscroll = function () { window.scrollTo(scrollLeft, scrollTop); };
}

function hidePageLoading() { Metro.activity.close(pageLoader); }
function showPageLoading() {
    if (pageLoader != undefined) {
        if (pageLoader[0]["DATASET:UID:M4Q"] == undefined) { pageLoader = null; }
        else {
            try { Metro.activity.close(pageLoader); } catch {
                try { pageLoader.close(); } catch { pageLoader = pageLoader[0]["DATASET:UID:M4Q"].dialog; pageLoader.close(); }; pageLoader = null;
            }
        }
    }
    pageLoader = Metro.activity.open({ type: 'atom', style: 'dark', overlayClickClose: true, /*overlayColor: '#fff', overlayAlpha: 1*/ });
}

//<a href="javascript:void(window.open('view-source:file:///'))">
//    use view-source to traverse and peruse Splashtop system files</a>

function showSource() {
    var source = "<html>";
    source += document.getElementsByTagName('html')[0].innerHTML;
    source += "</html>";
    source = source.replace(/</g, "&lt;").replace(/>/g, "&gt;");
    source = "<pre>" + source + "</pre>";
    sourceWindow = window.open('', 'Source of page', 'height=800,width=800,scrollbars=1,resizable=1');
    sourceWindow.document.write(source);
    sourceWindow.document.close();
    if (window.focus) sourceWindow.focus();
}

/*
(function () {
    "use strict";

    var d = document.createElement("div");
    d.style.cssText = "max-width:500px; max-height:200px; background-color:rgba(223,223,223,.7); border:3px solid rgba(0,0,0,.5); padding:5px; margin:10px; overflow-x:hidden; overflow-y:auto; word-break:break-word; font-family:'Courier New',Consolas,Lucida Console,monospace,sans-serif; text-shadow:.3px .3px rgba(0,0,0,.2),-0.3px -0.3px rgba(0,0,0,.2); border-radius:5px; box-shadow:1px 1px 5px rgba(0,0,0,.3),-1px -1px 5px rgba(0,0,0,.3);position: fixed;top: 10px;right: 10px; ";
    d.appendChild(document.createTextNode(document.querySelector('html').innerHTML));
    document.querySelector('body').appendChild(d);
}());
*/

function UserChangeTranslateSetting() {
    Metro.storage.setItem('UserAutomaticTranslate', $("#UserAutomaticTranslate").val('checked')[0].checked);
}

function htmlDecode(input) {
    var doc = new DOMParser().parseFromString(input, "text/html");
    return doc.documentElement.textContent;
}

function str2bytes(str) {
    var bytes = new Uint8Array(str.length);
    for (var i = 0; i < str.length; i++) {
        bytes[i] = str.charCodeAt(i);
    }
    return bytes;
}

function googleTranslateElementInit() {
    $(document).ready(function () {
        new google.translate.TranslateElement({
            pageLanguage: 'cs', //includedLanguages: 'en,cs',
            layout: google.translate.TranslateElement.InlineLayout.HORIZONTAL,
            autoDisplay: false
        }, 'google_translate_element');

        let autoTranslateSetting = Metro.storage.getItem('UserAutomaticTranslate', null) == null || Metro.storage.getItem('UserAutomaticTranslate', null) == false ? false : true;
        if (autoTranslateSetting && document.querySelector('#google_translate_element select') != null) {
            setTimeout(function () {
                let selectElement = document.querySelector('#google_translate_element select');
                selectElement.value = Metro.storage.getItem('DetectedLanguage', null);
                selectElement.dispatchEvent(new Event('change'));
            }, 1000);
        }

        /*
        let userTranslateSetting = Metro.storage.getItem('UserAutomaticTranslate', null) != null && Metro.storage.getItem('UserAutomaticTranslate', null) ? true : false;
        if (userTranslateSetting && document.querySelector('#google_translate_element select') != null) {
            setTimeout(function () {
                let selectElement = document.querySelector('#google_translate_element select');
                selectElement.value = "nemam";
                selectElement.dispatchEvent(new Event('change'));
            }, 1000);
        }*/


    });  
}

function CancelTranslation() {
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
}

function SendMessage() {
    showPageLoading();
    var def = $.ajax({
        global: false, type: "POST", url: Metro.storage.getItem('ApiOriginSuffix', null) + "/WebPages/InsertMessage", dataType: 'json',
        headers: {
            'Content-type': 'application/json'
        },
        data: JSON.stringify({ Message: $("#NewMessage").val() })
    });

    def.fail(function (err) {
        var notify = Metro.notify; notify.setup({ width: 300, duration: 1000, animation: 'easeOutBounce' });
        notify.create("Saving Message Failed", "Alert", { cls: "alert" }); notify.reset();
        hidePageLoading();
    });

    def.done(function (data) {
        var notify = Metro.notify; notify.setup({ width: 300, duration: 1000, animation: 'easeOutBounce' });
        notify.create("Saving Message Sucessfully", "Info", { cls: "success" }); notify.reset();
        $("#NewMessage").val(null);
        hidePageLoading();
        GetMessages();
    });
}

function PrintElement(elementId) {
    try {
        $("#" + elementId).printElement({ pageTitle: elementId.split("_")[1] + ".html", printMode: "popup" })
    } catch (t) { }
}

function DownloadHtmlElement(elementId) {
    try {
        var t = document.body.appendChild(document.createElement("a")); t.download = elementId + ".html"; t.href = "data:text/html;charset=utf-8," + encodeURIComponent(document.getElementById(elementId).innerHTML); t.click()
    } catch (i) { }
}

async function CopyElement(elementId) {
    try {
        let t = document.getElementById(elementId).innerHTML; await navigator.clipboard.writeText(t)
    } catch (t) { }
}

//Function for  Mermaid Data to Graphics Conversion
async function Mermaid() { try { await mermaid.run({ nodes: document.querySelectorAll('.class-mermaid'), }); } catch (err) { } }

//Function for Highlighting Code Segments
function HighlightCode() { document.querySelectorAll('div.code').forEach(el => { hljs.highlightElement(el); }); }


function ImageFromElement(elementId) {
    try {
        $("document").ready(function () {
            html2canvas($("#" + elementId), {
                onrendered: function (t) {
                    $("#previewImage").append(t);
                    var r = t.toDataURL("image/png"), u = r.replace(/^data:image\/png/, "data:application/octet-stream"), i = document.body.appendChild(document.createElement("a")); i.download = elementId + ".png"; i.href = u; i.click()
                }
            })
        })
    } catch (t) { }
}


async function FileReaderToImageData(n) {
    const t = new FileReader; return await new Promise((t, i) => {
        const r = new FileReader; r.onloadend = () => t(r.result); r.onerror = i;
        console.log("files", JSON.parse(JSON.stringify(files)));
        r.readAsDataURL(n[0])
    })
}


//Modal Window CREATE Function
function CreateLinkWindow(title, url) {
    var blankButton = [
        {
            html: "<span class='mif-open-book' title='Otevřít v Novém Okně'></span>",
            cls: "sys-button",
            onclick: "window.open('" + url + "','_blank')"
        }, {
            html: "<span class='mif-backward' title='Zpět do Výchozí Složky '></span>",
            cls: "warning",
            onclick: "$(\"#toolWindow\").attr(\"src\",\"" + url + "\")"
        }
    ];
    Metro.window.create({
        resizeable: true,
        draggable: true,
        width: '90%',
        height: 800,
        clsWindow: "pos-absolute",
        icon: "<span class='mif-eye'></span>",
        customButtons: blankButton,
        title: title,
        content: "<iframe id='toolWindow' src='" + url + "' allowfullscreen frameborder='0' width='100%' height='100%' style='overflow: hidden; height: 800px; width: 100 %'></iframe>",
        //overlayColor: "transparent",
        btnClose: true,
        shadow: true,
        modal: false,
        place: "center",
        onShow: function () { window.scrollTo(0, 0); }
    });
}

function ChangeSource(url) {
    $("#contentWindow").html('<iframe type="text/html" src="' + url + '" style="width:1024px;min-width:1024px !important;max-width:1024px !important; min-height:100vh;"  frameborder="0" style="height:100%;" height="100%"></iframe>');
    ScrollToTop();
}

//Control Translation Panel
function ShowToolPanel(close) {
    UserChangeTranslateSetting();
   // $("#UserAutomaticTranslate").val('checked')[0].checked = Metro.storage.getItem('UserAutomaticTranslate', null);
    if (close) { { hideMetroCharm("#toolPanel"); } } else {
        if (metroCharm.isOpened("#toolPanel")) { hideMetroCharm($('#toolPanel')); }
        else { showMetroCharm($('#toolPanel')); document.getElementById("toolPanel").style.left = "40%" }
    }
}


//Control Global News Window
function ShowNewsInfoBox() {
    if (Metro.infobox.isOpen('#NewsInfoBox')) { Metro.infobox.close('#NewsInfoBox'); }
    else { GetNewsList(); }
};

