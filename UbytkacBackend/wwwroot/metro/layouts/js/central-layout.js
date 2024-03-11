
// Behaviors

// STARTUP Temp Variables Definitions
let pageLoader;
let charms;

/*Definitions  of Global System Behaviors */
function ChangeSchemeTo(n) {
    $("#portal-color-scheme").attr("href", Metro.storage.getItem('BackendServerAddress', null) + "/metro/css/schemes/" + n);
    $("#scheme-name").html(n);
    Metro.storage.setItem('WebScheme', n);
}

/*Start of Global Loading Indicator for All Pages*/
function hidePageLoading() { Metro.activity.close(pageLoader); }
function showPageLoading() {
    if (pageLoader != undefined) {
        if (pageLoader[0]["DATASET:UID:M4Q"] == undefined) { pageLoader = null; }
        else { try { Metro.activity.close(pageLoader); } catch {
                try { pageLoader.close(); } catch { pageLoader = pageLoader[0]["DATASET:UID:M4Q"].dialog; pageLoader.close(); }; pageLoader = null; } }
    }
    pageLoader = Metro.activity.open({ type: 'atom', style: 'dark', overlayClickClose: true, /*overlayColor: '#fff', overlayAlpha: 1*/ });
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

function ApplyLoadedWebSetting(val) {
    if (Metro.storage.getItem('WebPagesName', null) != null) {
        $("#WebPagesName").html(Metro.storage.getItem('WebPagesName', null) + "<small id='WebPageMottos'>" + (Metro.storage.getItem('CycleWebPageMottosEnabled', null) == null ? Metro.storage.getItem('WebPagesNameSmall', null) : "" ) + "</small>");
    } else { $("#WebPagesName").html("EDC+ESB IT GroupWare-Solution.Eu <small id='WebPageMottos'>Celé IT v 1 Řešení v každém jazyce</small>"); }


    if (document.getElementsByClassName("YTPOverlay")[0] == undefined) {
        document.getElementsByClassName("overlay")[0].style.opacity = (Metro.storage.getItem('BackgroundOpacitySetting', null) /100);
        document.getElementsByClassName("overlay")[0].style.backgroundColor = Metro.storage.getItem('BackgroundColorSetting', null);
    } else {
        document.getElementsByClassName("YTPOverlay")[0].style.opacity = (Metro.storage.getItem('BackgroundOpacitySetting', null) / 100);
        document.getElementsByClassName("YTPOverlay")[0].style.backgroundColor = Metro.storage.getItem('BackgroundColorSetting', null);
    }
    $("#body")[0].style.backgroundImage = 'url("' + Metro.storage.getItem('BackgroundImageSetting', null) + '")';
    try { $("#backgroundPlayer")[0].dataset.property = "{ videoURL:'" + Metro.storage.getItem('BackgroundVideoSetting', null) + "', containment: '#body', showControls: false, autoPlay: true, loop: true, mute: true, startAt: 0, opacity: 1, quality: 'default', optimizeDisplay: true }";} catch { }
    try { $('.player').YTPChangeMovie({ videoURL: Metro.storage.getItem('BackgroundVideoSetting', null), containment: '#body', showControls: false, autoPlay: true, loop: true, mute: true, startAt: 0, opacity: 1, quality: 'default', optimizeDisplay: true });} catch { }
    setTimeout(function () { $('.player').YTPPlay(); }, 1000);
}

//Afte Api Load 
function SetWebMenuPanel() {
    let menuContent = Metro.storage.getItem('WebMenuList', null);
    if (menuContent.length > 0) {
        let html = ""; let groupId = null;
        menuContent.forEach((menuItem, index) => {
            if (groupId != menuItem.groupId) {
                html += (html.length > 0 ? "</ul></li>" : "");
                html += "<li ><a href='#' onclick=\'" + (menuItem.group.onclick.toString().replace('"', '\"')) + "\' class='dropdown-toggle' data-role='ripple' data-ripple-color='#CE352C' data-ripple-alpha='.2' >" + menuItem.group.name + "</a>";
                html += "<ul class='d-menu' data-role='dropdown'>";
            }
            if(Metro.storage.getItem('ReloadContentOnly', null) == null || !Metro.storage.getItem('ReloadContentOnly', null)){
	            html += 
	            "<li onclick=window.location.href='" + (window.location.protocol == "file:" ? window.location.pathname.replace(window.location.pathname.split("/").pop(), "") : "/") + menuItem.id.toString() + "-" + menuItem.name.replace(/\s+/g, '') + (window.location.protocol == "file:" ? ".html" : "") + "' class='" + (menuItem.menuClass != null ? menuItem.menuClass : "") + "' " + (menuItem.description != null && menuItem.description.length > 0 ?
		            "data-role='hint' data-cls-hint='supertop' data-hint-position='bottom' data-hint-text='" + menuItem.description + "'" : "") + " ><a href='#' >" + menuItem.name + 
		        "</a></li>";
	        } else {
	            html += 
	            "<li onclick=SetWebMenuContent('" + menuItem.id.toString() + "') class='" + 
		            (menuItem.menuClass != null ? menuItem.menuClass : "") + "' " + 
		            (menuItem.description != null && menuItem.description.length > 0 ?
		            " data-role='hint' data-cls-hint='supertop' data-hint-position='bottom' data-hint-text='" + menuItem.description + "'" : "") + 
		            " ><a href='#" + menuItem.id.toString() + "-" + menuItem.name.replace(/\s+/g, '') + "' >" + menuItem.name + 
		        "</a></li>";
	        }
            groupId = menuItem.groupId;
        });
        html += "</ul></li>";
        html += "<li><div data-role='hint' data-hint-position='bottom' data-hint-text='Přeložit do libovolného jazyku' class='c-pointer mif-language mif-2x fg-white p-3 ani-hover-heartbeat' onclick='ShowToolPanel()'></div></li>";
        $("#WebMainMenuPanel").html(html);

    }
}

async function SetWebMenuContent(id){
	window.showPageLoading();
	let response = await fetch(Metro.storage.getItem('ApiOriginSuffix', null) + (Metro.storage.getItem('ApiToken', null) == null ? '/WebPages/GetWebMenuListById/' + id : '/WebPages/GetAuthWebMenuListById/' + id), {
        method: 'GET', headers: { 'Content-type': 'application/json', 'Authorization': 'Bearer ' + Metro.storage.getItem('ApiToken', null) }
    }); let result = await response.json();
    if (result.Status == "error") { ShowBlockedMessage(); } 
    else {
        $("#WebPageHTMLContent").html(result.htmlContent);
        document.querySelector("#WebPageHTMLContent").style.position = "absolute";
        document.querySelector("#WebPageHTMLContent").style.position = "relative";
        document.querySelector("#WebPageHTMLContent").style.top = "70px";
    }
    setTimeout(function () { try { $('.player').YTPPlay(); } catch { } }, 1000);

    // refresh Menu   
    await GetWebMenuList();
    window.hidePageLoading();
}

async function SetWebMenuHtml() {
    setTimeout(async function () {
        window.showPageLoading(); let id = 0;
        if (window.location.pathname != "/") { id = window.location.pathname.split("/").pop().split("-")[0]; } else { id = 0; }
        let response = await fetch(Metro.storage.getItem('ApiOriginSuffix', null) + (Metro.storage.getItem('ApiToken', null) == null ? '/WebPages/GetWebMenuListById/' + id : '/WebPages/GetAuthWebMenuListById/' + id), {
            method: 'GET', headers: { 'Content-type': 'application/json', 'Authorization': 'Bearer ' + Metro.storage.getItem('ApiToken', null) }
        }); let result = await response.json();

        if (result.Status == "error") { ShowBlockedMessage(); } else {
            $("#WebPageHTMLContent").html(result.htmlContent);
            document.querySelector("#WebPageHTMLContent").style.position = "absolute";
            document.querySelector("#WebPageHTMLContent").style.position = "relative";
            document.querySelector("#WebPageHTMLContent").style.top = "70px";
        }
        setTimeout(function () { try { $('.player').YTPPlay(); } catch { } }, 1000);

        // refresh Menu   
        await GetWebMenuList();
        window.hidePageLoading();
    }, 100);
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

// Functions 
//Global Functions Library

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




function RestrictionControls() {
    //Enable-Disable Logged Controls
    //$("#UserSettingBtn").prop('disabled', !isLogged);
}



//Save Token on Success Login
async function AdminLogin(data) {
    Cookies.set('ApiToken', data.Token);
    Metro.storage.setItem('ApiToken', data.Token);
    Metro.storage.setItem('User', data);

    window.location.href = '/' + Metro.storage.getItem('WebMenuList', null)[0].id + "-" + Metro.storage.getItem('WebMenuList', data.Token)[0].name.replace(/\s+/g, '');
}


//Do Logout
function Logout() {
    Cookies.remove('ApiToken');
    Metro.storage.storage.clear();
    window.location.href ='/';
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


//Generated Objects
<!-- TOOL Panel -->
    <div id="toolPanel" data-role="bottom-sheet" class="bottom-sheet pos-fixed list-list grid-style opened" style="top: 0px; left: 90%; z-index:10000;min-width: 400px;">
        <div class="w-100 text-left" > <audio id="radio" class="light bg-transparent" data-role="audio-player" data-src="./metro/media/hotel_california.mp3" data-volume=".5" ></audio> </div>
        <div class="w-100 text-left" style="z-index: 1000000;"><div id="google_translate_element"></div></div>
        <div class="w-100 d-inline-flex">
            <div class="w-75 text-left">
                <input id="UserAutomaticTranslate" type="checkbox" data-role="checkbox" data-cls-caption="fg-cyan text-bold" data-caption="Překládat Automaticky" onchange="UserChangeTranslateSetting">
            </div><div class="w-25 mt-1 text-right"><button class="button secondary mini" onclick="CancelTranslation()">Zrušit Překlad</button></div>
            </div>
        <div class="d-flex w-100">
            <button class="button shadowed w-25 mt-1" style="background-color: #585b5d; width:50px;" onclick="ChangeSchemeTo('darcula.css')"></button>
            <button class="button shadowed w-25 mt-1" style="background-color: #AF0015; width:50px;" onclick="ChangeSchemeTo('red-alert.css')"></button>
            <button class="button shadowed w-25 mt-1" style="background-color: #690012; width:50px;" onclick="ChangeSchemeTo('red-dark.css')"></button>
            <button class="button shadowed w-25 mt-1" style="background-color: #0CA9F2; width:50px;" onclick="ChangeSchemeTo('sky-net.css')"></button>
        </div>
        <div class="c-pointer mif-cancel icon pos-absolute fg-red" style="top:5px;right:5px;" onclick="ShowToolPanel()"></div>
    </div>

//Blocked IP Info Panel
function ShowBlockedMessage() {
    var html_content =
        "<h3>Blokovaná IP Adresa</h3>" +
        "<p>Vaše adrese je blokována, protože byla zjištěna podezřelá činnost...</p>" +
        "<p>Pro Odblokování nás kontaktujte Telefonicky.</p>";
    Metro.infobox.create(html_content, "alert");
}

//Unauthorized Access Info Panel
function ShowUnAuthMessage() {
    Logout();
    var html_content =
        "<h3>Neautorizovaný Přístup</h3>" +
        "<p>Pokoušíte se provést autorizovanou operaci neoprávněně,</p>" +
        "<p>nebo platnost vašeho tokenu vypršela.</p>";
    Metro.infobox.create(html_content, "alert");
}
