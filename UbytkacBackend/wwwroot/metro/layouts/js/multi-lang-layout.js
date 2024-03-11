//START DEFINE PAGE GLOBAL VARIABLES
let siteLoader;
let charms;

//END OF DEFINE PAGE GLOBAL VARIABLES
//START DEFINE STORAGE FOR TOOL
Metro.storage.setItem('BackendServerAddress', window.location.origin); /* You Can Set External Backend Server */
Metro.storage.setItem('ApiOriginSuffix', Metro.storage.getItem('BackendServerAddress', null) + "/WebApi");
Metro.storage.setItem('DetectedLanguage', (navigator.language || navigator.userLanguage).substring(0, 2));

/*WebPages Theme Scheme*/
if (Metro.storage.getItem('WebScheme', null) == null) {
    Metro.storage.setItem('WebScheme', "sky-net.css");
    ChangeSchemeTo(Metro.storage.getItem('WebScheme', null));
} else { ChangeSchemeTo(Metro.storage.getItem('WebScheme', null)); }

//Start Set User Default Setting
if (Metro.storage.getItem('UserAutomaticTranslate', null) == null) { Metro.storage.setItem('UserAutomaticTranslate', true); }


//END OF STORAGE FOR TOOL
//START PAGE BEHAVIORS CONTROL DEFINITONS


/*Definitions  of Global System Behaviors */
function ChangeSchemeTo(n) {
    $("#portal-color-scheme").attr("href", Metro.storage.getItem('BackendServerAddress', null) + "/metro/css/schemes/" + n);
    $("#scheme-name").html(n);
    Metro.storage.setItem('WebScheme', n);
}

/*Start of Global Loading Indicator for All Pages*/
function hidePageLoading() { Metro.activity.close(siteLoader); }
function showPageLoading() {
    if (siteLoader != undefined) {
        if (siteLoader[0]["DATASET:UID:M4Q"] == undefined) { siteLoader = null; }
        else { try { Metro.activity.close(siteLoader); } catch {
                try { siteLoader.close(); } catch { siteLoader = siteLoader[0]["DATASET:UID:M4Q"].dialog; siteLoader.close(); }; siteLoader = null; } }
    }
    siteLoader = Metro.activity.open({ type: 'atom', style: 'dark', overlayClickClose: true, /*overlayColor: '#fff', overlayAlpha: 1*/ });
}


function googleTranslateElementInit() {
    $(document).ready(function () {
        new google.translate.TranslateElement({
            pageLanguage: 'cs', //includedLanguages: 'en,cs',
            layout: google.translate.TranslateElement.InlineLayout.HORIZONTAL,
            autoDisplay: false
        }, 'google_translate_element');

        let autoTranslateSetting = Metro.storage.getItem('AutomaticDetectedLanguageTranslate', null) == null || Metro.storage.getItem('AutomaticDetectedLanguageTranslate', null) == false ? false : true;
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


function PrintElement(elementId){
	try{$("#"+elementId).printElement({pageTitle:elementId.split("_")[1]+".html",printMode:"popup"})
	}catch(t){}
}

function DownloadHtmlElement(elementId){
	try{var t=document.body.appendChild(document.createElement("a"));t.download=elementId+".html";t.href="data:text/html;charset=utf-8,"+encodeURIComponent(document.getElementById(elementId).innerHTML);t.click()
	}catch(i){}
}

async function CopyElement(elementId){
	try{let t=document.getElementById(elementId).innerHTML;await navigator.clipboard.writeText(t)
	}catch(t){}
}

function ImageFromElement(elementId){
	try{$("document").ready(function(){html2canvas($("#"+elementId),{onrendered:function(t){$("#previewImage").append(t);
	var r=t.toDataURL("image/png"),u=r.replace(/^data:image\/png/,"data:application/octet-stream"),i=document.body.appendChild(document.createElement("a"));i.download=elementId+".png";i.href=u;i.click()}})})
	}catch(t){}
}


//Control Translation Panel
function ShowToolPanel(close) {
    $("#UserAutomaticTranslate").val('checked')[0].checked = Metro.storage.getItem('UserAutomaticTranslate', null);
    if (close) { { Metro.bottomsheet.close($('#ToolPanel')); } } else {
        if (Metro.bottomsheet.isOpen($('#ToolPanel'))) { Metro.bottomsheet.close($('#ToolPanel')); }
        else { Metro.bottomsheet.open($('#ToolPanel')); }
    }
}

//END PAGE BEHAVIORS CONTROL DEFINITONS
//START GLOBAL FUNCTIONS DECLARATIONS


function ScrollToTop() { window.scrollTo(0, 0); }
function enableScroll() { window.onscroll = function () { }; }
function disableScroll() {
    scrollTop = window.pageYOffset || document.documentElement.scrollTop;
    scrollLeft = window.pageXOffset || document.documentElement.scrollLeft,
    window.onscroll = function () { window.scrollTo(scrollLeft, scrollTop); };
}

function str2bytes(str) {
    var bytes = new Uint8Array(str.length);
    for (var i = 0; i < str.length; i++) {
        bytes[i] = str.charCodeAt(i);
    }
    return bytes;
}

function htmlDecode(input) {
    var doc = new DOMParser().parseFromString(input, "text/html");
    return doc.documentElement.textContent;
}

function UserChangeTranslateSetting() {
    Metro.storage.setItem('UserAutomaticTranslate', $("#UserAutomaticTranslate").val('checked')[0].checked);
}


//Save Token on Success Login
async function Login(data) {
    Cookies.set('ApiToken', data.Token);
    Metro.storage.setItem('ApiToken', data.Token);
    Metro.storage.setItem('User', data);
}


//Do Logout
function Logout() {
    Cookies.remove('ApiToken');
    Metro.storage.storage.clear();
}


//Check is logged
function IsLogged() {
    return Metro.storage.getItem('ApiToken', null) != null;
}




//Load Metro
async function LoadMetro() {
    const pathCss = './metro/css/metro-all.min.css';
    const pathThemeCss = './metro/css/schemes/sky-net.css';
    const pathJs = './metro/js/metro.4.5.2.min.js?v=4.5.2';

    const dataCss = await fetch(pathCss).then((r) => r.text());
    const dataThemeCss = await fetch(pathThemeCss).then((r) => r.text());

    const myFont = new FontFace('metro', "url('./metro/mif/metro.svg') format('svg'), url('./metro/mif/metro.woff') format('woff'), url('./metro/mif/metro.ttf') format('truetype')");
    await myFont.load();document.fonts.add(myFont);

    const dataJs = await fetch(pathJs).then((r) => r.text())

    const style = document.createElement("style")
    style.textContent = dataCss
    document.querySelector("head").appendChild(style)

    style.textContent = dataThemeCss
    document.querySelector("head").appendChild(style)

    new Function(dataJs)();
    Metro.init();
    Metro.toast.create("Metro 4 did loaded successful!", {showTop: true,clsToast: "success"});
    $("#a1").accordion()
}

//Unload Metro 
function UnloadMetro() {
    delete Metro;
}

async function FileReaderToImageData(n){
	const t=new FileReader;return await new Promise((t,i)=>{
		const r=new FileReader;r.onloadend=()=>t(r.result);r.onerror=i;
		console.log("files",JSON.parse(JSON.stringify(files)));
		r.readAsDataURL(n[0])
	})
}

//END GLOBAL FUNCTIONS DECLARATIONS
//START OBJECT GENERATORS DEFINITIONS

<!-- TOOL Panel -->

function GenerateToolPanel(){
	let htmlTool ='<div id="ToolPanel" data-role="bottom-sheet" class="draggable bottom-sheet pos-fixed list-list grid-style opened drag-element" data-drag-element="ToolPanel" style="top: 160px; left: 90%; z-index:10000;min-width: 400px;">';
	htmlTool +='<div class="w-100 text-left" > <audio id="radio" class="light bg-transparent" data-role="audio-player" data-src="./metro/media/hotel_california.mp3" data-volume=".5" ></audio> </div>';
	htmlTool +='<div class="w-100 text-left" style="z-index: 1000000;"><div id="google_translate_element"></div></div>';
	htmlTool +='<div class="w-100 d-inline-flex"><div class="w-75 text-left">';
	htmlTool +='<input id="UserAutomaticTranslate" type="checkbox" data-role="checkbox" data-cls-caption="fg-cyan text-bold" data-caption="Překládat Automaticky" onchange="UserChangeTranslateSetting" checked>';
	htmlTool +='</div><div class="w-25 mt-1 text-right"><button class="button secondary mini" onclick="CancelTranslation()">Zrušit Překlad</button></div>';
	htmlTool +='</div><div class="d-flex w-100">';
	htmlTool +='<button class="button shadowed w-25 mt-1" style="background-color: #585b5d; width:50px;" onclick="ChangeSchemeTo(\'darcula.css\')"></button>';
	htmlTool +='<button class="button shadowed w-25 mt-1" style="background-color: #AF0015; width:50px;" onclick="ChangeSchemeTo(\'red-alert.css\')"></button>';
	htmlTool +='<button class="button shadowed w-25 mt-1" style="background-color: #690012; width:50px;" onclick="ChangeSchemeTo(\'red-dark.css\')"></button>';
	htmlTool +='<button class="button shadowed w-25 mt-1" style="background-color: #0CA9F2; width:50px;" onclick="ChangeSchemeTo(\'sky-net.css\')"></button>';
	htmlTool +='</div><div class="c-pointer mif-cancel icon pos-absolute fg-red" style="top:5px;right:5px;display:none;" onclick="ShowToolPanel()"></div></div>';
	
	let origin = htmlTool + $('#WebPageHTMLContent').html();$('#WebPageHTMLContent').html(origin);
	//Metro.infobox.open('#ToolPanel');
}

//Blocked IP Info Panel
function ShowBlockedMessage() {
    var html_content =
        "<h3>Blokovaná IP Adresa</h3>" +
        "<p>Vaše adrese je blokována, protože byla zjištěna podezřelá činnost...</p>" +
        "<p>Pro Odblokování nás kontaktujte Telefonicky.</p>";
    Metro.infobox.create(html_content, "alert", {clsBox: " drop-shadow shadowed "});
}

//Unauthorized Access Info Panel
function ShowUnAuthMessage() {
    Logout();
    var html_content =
        "<h3>Neautorizovaný Přístup</h3>" +
        "<p>Pokoušíte se provést autorizovanou operaci neoprávněně,</p>" +
        "<p>nebo platnost vašeho tokenu vypršela.</p>";
    Metro.infobox.create(html_content, "alert", {clsBox: " drop-shadow shadowed "});
}


//END OBJECT GENERATORS DEFINITIONS
//STARTUP COMMANDS 
try { showPageLoading();} catch {}





$(document).ready(function () { hidePageLoading();GenerateToolPanel(); });
//END OF STARTUP COMMANDS


