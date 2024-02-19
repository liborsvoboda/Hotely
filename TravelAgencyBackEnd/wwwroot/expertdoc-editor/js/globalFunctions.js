// local variables
let ConfigDefaultContent = "### MarkDown Item Template   \r\n```csharp   \r\n\r\n```   \r\n---    \r\n";

//API Part

async function ApiLogin(writeMessage){
	showPageLoading();

	var def = $.ajax({
		global: false, type: "POST", url: "/GLOBALNETAuthentication", dataType: 'json',
		headers: { "Authorization": "Basic " + btoa(ConfigApiServer.BasicAuthLoginName + ":" + ConfigApiServer.BasicAuthLoginPassword) }
	});

	def.fail(function (data) {
		var notify = Metro.notify; notify.setup({ width: 300, duration: ConfigSystemMessageShowTime * 1000, animation: 'easeOutBounce' });
		notify.create("Login Failed", "Alert", { cls: "alert" }); notify.reset();
		hidePageLoading();
		return false;
	});

	def.done(function (data) {
		if (writeMessage) {
			var notify = Metro.notify; notify.setup({ width: 300, duration: ConfigSystemMessageShowTime * 1000, animation: 'easeOutBounce' });
			notify.create("Login Succesfull", "Info", { cls: "info" }); notify.reset();
		} else {
			console.log("User Logged");
			Metro.storage.setItem('ApiToken', data.Token);
			DownloadApiFiles(false);
		}
	});
	
}

async function ReloadFiles() {
	if (ManagerMode == "apiManager") { await DownloadApiFiles(true); }
	else { ReloadFolderFiles() }
}

async function DownloadApiFiles(onlyReload) {
	DocFiles = [];
	showPageLoading();

	var def = $.ajax({
		global: false, type: "GET", url: "/GLOBALNETDocSrvDocumentationList/byGroup/" + ConfigApiGroupId, dataType: 'json',
		contentType: 'application/json',
		headers: { "Authorization": 'Bearer ' + Metro.storage.getItem('ApiToken', null) }
	}).fail(function (data) {
		var notify = Metro.notify; notify.setup({ width: 300, duration: 1000, animation: 'easeOutBounce' });
		notify.create("Failed load Api Documenation List", "Alert", { cls: "alert" }); notify.reset();
		hidePageLoading();
	}).done(function (data) {
		hidePageLoading();
		if (data.length > 0) {
			data.forEach(doc => {
				DocFiles.push({
					groupId: doc.DocumentationGroupId,
					filename: doc.Name, content: (doc.Description == null) ? "" : doc.Description,
					version: doc.AutoVersion, lastUpdate: new Date(doc.TimeStamp).toLocaleString()
				});
			});
		}
		
		if (onlyReload) { UpdateFileMenu(); }
		else { startupLoading(); }
	});

}


function GenerateMdBook(){



}

// Functions Part

async function ToMermaid(){
    if ($("#AutoMermaid")[0] != undefined && $("#AutoMermaid")[0].checked) {
		try {
			await mermaid.run({
				nodes: document.querySelectorAll('.lang-mermaid'),
			});
		} catch(err) { }
	}
}

function MermaidConvert(){
	try {
		mermaid.run({
			nodes: document.querySelectorAll('.lang-mermaid'),
		});
		MermaidTransform = false;
	} catch(err) { }
}


function ExportSummaryFile(){
	let summary = "";
	DocFiles.forEach(file =>{ 
		summary += "### " + file.filename + "     \r\n"; 
		summary += "["+ file.filename + "](./" + file.filename + ")    \r\n"; 
		summary += "---     \r\n\r\n"; 
	});
	let UrlSummaryObject = window.URL || window.webkitURL;
	let SummaryBlob = new Blob([summary], {type: 'text/plain'});
	let SummaryDownloadUrl = UrlSummaryObject.createObjectURL(SummaryBlob);
	let SummaryLink = document.createElement('a');
	if (typeof SummaryLink.download === 'undefined') {
		window.location.href = SummaryDownloadUrl;
	} else {
		SummaryLink.download = "Summary.md";SummaryLink.href = SummaryDownloadUrl;
		document.body.appendChild(SummaryLink);SummaryLink.click();
		setTimeout(function () { UrlSummaryObject.revokeObjectURL(SummaryDownloadUrl); }, 100);
	}
}

function ExportConfiguration(){
	let configuration = "let ConfigUrlMdFile = '" + ConfigUrlMdFile + "'; \r\n"
	configuration += "let ConfigFiles = " + JSON.stringify(ConfigFiles) + "; \r\n"
	configuration += "let ConfigApiServer = { BasicAuthLoginName: '" + ConfigApiServer.BasicAuthLoginName + "', BasicAuthLoginPassword: '" + ConfigApiServer.BasicAuthLoginPassword + "', ServerApiAddress: '" + ConfigApiServer.ServerApiAddress + "'}; \r\n"
	configuration += "let ConfigDefaultTemplate = '" + ConfigDefaultTemplate + "'; \r\n";
	configuration += "let ConfigExportFileOnSaving = " + ConfigExportFileOnSaving.toString() + "; \r\n";
	configuration += "let ConfigMermaidConvertOnExport = " + ConfigMermaidConvertOnExport.toString() + "; \r\n";
	configuration += "let ConfigSystemMessageShowTime = " + ConfigSystemMessageShowTime.toString() + "; \r\n";
	configuration += "let ConfigWaitingTimeInterval = " + ConfigWaitingTimeInterval.toString() + "; \r\n";
	configuration += "let ConfigAutoMultiTranslateEnabled = " + ConfigAutoMultiTranslateEnabled.toString() + "; \r\n";
	configuration += "let ConfigEnableCreateNewDoc = " + ConfigEnableCreateNewDoc.toString() + "; \r\n";
	configuration += "let ConfigSelectedLanguages = " + JSON.stringify(ConfigSelectedLanguages) + "; \r\n";
	configuration += "let ConfigReturnToLanguage = '" + ConfigReturnToLanguage + "'; \r\n";
	configuration += "let ConfigApiGroupId = " + ConfigApiGroupId.toString() + "; \r\n";

	let UrlConfigObject = window.URL || window.webkitURL;
	let ConfigBlob = new Blob([configuration], {type: 'text/plain'});
	let ConfigDownloadUrl = UrlConfigObject.createObjectURL(ConfigBlob);
	let ConfigLink = document.createElement('a');
	if (typeof ConfigLink.download === 'undefined') {
		window.location.href = ConfigDownloadUrl;
	} else {
		ConfigLink.download = "config.js";ConfigLink.href = ConfigDownloadUrl;
		document.body.appendChild(ConfigLink);ConfigLink.click();
		setTimeout(function () { UrlConfigObject.revokeObjectURL(ConfigDownloadUrl); }, 100);
	}

}

async function startupLoading() {
	showPageLoading();

	try{
		if (window.navigator.onLine) {
			StartupProcessInfo("Internet connection is Ok",false);
		} else {StartupProcessInfo("Internet connection failed. Translating is not possible.",true);}

		if (document.querySelector('script[src="./config/config.js"]') != null) {
			StartupProcessInfo("Configuration loaded",false);
		} else {StartupProcessInfo("Configuration [config.js] in [config] folder is missing ",true);}

		await axios.get(window.location.origin + window.location.pathname +"config/" + ConfigDefaultTemplate,{
		headers: { "Access-Control-Allow-Origin": "*", "Access-Control-Allow-Methods": "GET, POST, PUT" }
		}).then((response)=> {
			ConfigDefaultContent = response.data;
			StartupProcessInfo("Default Template loaded",false);
		}).catch(function (error) {
			StartupProcessInfo("Default Template [defaultTemplate.md] file in [config] folder is missing ",true);
			var notify = Metro.notify; notify.setup({ width: 300, duration: ConfigSystemMessageShowTime * 1000, animation: 'easeOutBounce' });
			notify.create("Default Template file '" + ConfigDefaultTemplate + " not Exist in 'config' directory.", "Info",{cls: "supertop"}); notify.reset();
		});

		await axios.get(window.location.origin + window.location.pathname +'config/languages.json',{
		headers: { "Access-Control-Allow-Origin": "*", "Access-Control-Allow-Methods": "GET, POST, PUT" }
		}).then((response)=> {
			let html = "<select data-role='select' id='LanguageMenu' " + ' data-filter-placeholder=\'Search\' data-filter=\'true\' ' +" data-clear-button=\'true\'  multiple >";
			let confightml = "<select data-role='select' id='configSelectedLanguages' " + ' data-filter-placeholder=\'Search\' data-filter=\'true\' ' +" data-clear-button=\'true\'  multiple >";
			let confightmlreturn = "<select data-role='select' id='configReturnToLanguage' " + ' data-filter-placeholder=\'Search\' data-filter=\'true\' ' +" data-clear-button=\'true\' >";
			StartupProcessInfo("Languages List is loaded",false);
			response.data.forEach(language =>{ 
				html += "<option value='" + language.code + "'>" + language.language + "</option>"; 
				confightml += "<option value='" + language.code + "'>" + language.language + "</option>"; 
				confightmlreturn += "<option value='" + language.code + "'>" + language.language + "</option>"; 
			});
			html += "</select>";
			confightml += "</select>";
			confightmlreturn += "</select>";
			$("#LanguageMenuRoot").html(html);
			$("#configSelectedLanguagesRoot").html(confightml);
			$("#configReturnToLanguageRoot").html(confightmlreturn);
		}).catch(function (error) {
			StartupProcessInfo("LanguageList file 'languages.json' not Exist in 'config' directory. ",true);
			var notify = Metro.notify; notify.setup({ width: 300, duration: ConfigSystemMessageShowTime * 1000, animation: 'easeOutBounce' });
			notify.create("LanguageList file 'languages.json' not Exist in 'config' directory.", "Info",{cls: "supertop"}); notify.reset();
		});

	} catch(err) { StartupProcessInfo("Configuration setting format problem detected" + err.message,true); }
	await StartupConfiguration();
}

async function StartupConfiguration(){
	try{
		$("#configUrlMdFile").val(ConfigUrlMdFile);
		$("#configFiles").data('taginput').val(ConfigFiles.toLocaleString());
		$("#basicAuthLoginName").val(ConfigApiServer.BasicAuthLoginName);
		$("#basicAuthLoginPassword").val(ConfigApiServer.BasicAuthLoginPassword);
		$("#serverApiAddress").val(ConfigApiServer.ServerApiAddress);
		$("#configDefaultTemplate").val(ConfigDefaultTemplate);
		$("#configMermaidConvertOnExport").val('checked')[0].checked = ConfigMermaidConvertOnExport;
		$("#configExportFileOnSaving").val('checked')[0].checked = ConfigExportFileOnSaving;
		$("#configSystemMessageShowTime").val(ConfigSystemMessageShowTime);
		$("#configWaitingTimeInterval").val(ConfigWaitingTimeInterval);
		$("#configAutoMultiTranslateEnabled").val('checked')[0].checked = ConfigAutoMultiTranslateEnabled;
		$("#AutoTranslateSelected").val('checked')[0].checked = ConfigAutoMultiTranslateEnabled;
		$("#configEnableCreateNewDoc").val('checked')[0].checked = ConfigEnableCreateNewDoc;
		$("#configApiGroupId").val(ConfigApiGroupId);
		

		//Main Settings
		$(document).ready(async function () {
			Metro.getPlugin($("#configSelectedLanguages"),'select').val(ConfigSelectedLanguages);
			Metro.getPlugin($("#LanguageMenu"),'select').val(ConfigSelectedLanguages);
			Metro.getPlugin($("#configReturnToLanguage"),'select').val(ConfigReturnToLanguage);
		});

		StartupProcessInfo("Configuration setting are ok",false);
	} catch(err) { StartupProcessInfo("Configuration setting format problem detected" + err.message,true); }
	
	await StartupModeSelection();
}


async function StartupModeSelection(){

	try {

		if (Metro.storage.getItem('ApiToken', null) != null) {
			ManagerMode = "apiManager";
			UpdateFileMenu();
		 	console.log("Manager is in api server mode");
		 } else if(ConfigFiles.length){
		 	ManagerMode = "filesEditor";
			ReloadFolderFiles()

		 } else if(ConfigUrlMdFile.length){
		 	ManagerMode = "editor";
			ReloadFolderFiles();

		 	await axios.get(ConfigUrlMdFile,{
		 	headers: { "Access-Control-Allow-Origin": "*", "Access-Control-Allow-Methods": "GET, POST, PUT" }
		 	}).then((response)=> {
		 		$("#Mavon")[0].__vue__.d_value=response.data;
		 	}).catch(function (error) {
		 		var notify = Metro.notify; notify.setup({ width: 300, duration: ConfigSystemMessageShowTime * 1000, animation: 'easeOutBounce' });
		 		notify.create("Cors Error. For Url Loading must Run App as WebServer or you can Use Copy/Paste/Export", "Info",{cls: "supertop"}); notify.reset();
		 	});
		 	$("#Mavon")[0].__vue__.d_value = ConfigDefaultContent; 
		 } else { 
		 	ManagerMode = "editor";
			ReloadFolderFiles();
		 	$("#Mavon")[0].__vue__.d_value = ConfigDefaultContent; 
			
		 }
			
	} catch(err) { StartupProcessInfo("Mode selection problem detected. Check config.js" + err.message,true);
		console.log(err);
		ManagerMode = "editor";
		$("#Mavon")[0].__vue__.d_value = ConfigDefaultContent;
	}
	ConfigSetProgramMenu();
	hidePageLoading();
}


function StartupProcessInfo(addMessage,isError){
	console.log(addMessage);
	//$("#startupProcess").html($("#startupProcess").html() + "<p class='" + (isError? 'fc-darkRed' : 'fc-darkBlue') + "'>" + addMessage + "...</p>");
}


async function ConfigApplySettings(){

	try{
		ShowToolPanel();
		ConfigUrlMdFile = $("#configUrlMdFile").val();
		ConfigFiles = $("#configFiles").data('taginput').val();
		ConfigApiServer.BasicAuthLoginName = $("#basicAuthLoginName").val();
		ConfigApiServer.BasicAuthLoginPassword = $("#basicAuthLoginPassword").val();
		ConfigApiServer.ServerApiAddress = $("#serverApiAddress").val();
		ConfigDefaultTemplate = $("#configDefaultTemplate").val();
		ConfigMermaidConvertOnExport = $("#configMermaidConvertOnExport").val('checked')[0].checked;
		ConfigExportFileOnSaving = $("#configExportFileOnSaving").val('checked')[0].checked;
		ConfigSystemMessageShowTime = $("#configSystemMessageShowTime").val();
		ConfigWaitingTimeInterval = $("#configWaitingTimeInterval").val();
		ConfigAutoMultiTranslateEnabled = $("#configAutoMultiTranslateEnabled").val('checked')[0].checked;
		ConfigAutoMultiTranslateEnabled = $("#AutoTranslateSelected").val('checked')[0].checked;
		ConfigEnableCreateNewDoc = $("#configEnableCreateNewDoc").val('checked')[0].checked;
		ConfigApiGroupId = $("#ConfigApiGroupId").val();
		

		//Main Settings
		ConfigSelectedLanguages = Metro.getPlugin($("#configSelectedLanguages"),'select').val();
		ConfigReturnToLanguage = Metro.getPlugin($("#configReturnToLanguage"),'select').val();

		await StartupModeSelection();
	} catch(err) { StartupProcessInfo("Mode selection problem detected. Check config.js" + err.message,true); }


}

function ShowToolPanel() {
    if (Metro.bottomsheet.isOpen($('#toolPanel'))) { Metro.bottomsheet.close($('#toolPanel')); }
    else {
        Metro.bottomsheet.open($('#toolPanel'));
    }
}

function NewFileAddToList(){
	try{
		if($("#NewFilename").val().length > 0 ){
			let NewDocFile = {
				filename: $("#NewFilename").val(), 
				content: (
					$("#CopyDocumentTemplate").val('checked')[0].checked 
					? ConfigDefaultContent 
					: $("#CopyFileMenu")[0].selectedOptions[0].text != '' 
					? DocFiles.filter(obj => { return obj.filename == $("#CopyFileMenu")[0].selectedOptions[0].text })[0].content 
					: '' 
				), version: 0, groupId: ConfigApiGroupId, lastUpdate: new Date().toLocaleString()	

				};
			DocFiles.push(NewDocFile);
			UpdateFileMenu();
			Metro.infobox.close($('#NewFilePanel'));

			var notify = Metro.notify; notify.setup({ width: 300, duration: ConfigSystemMessageShowTime * 1000, animation: 'easeOutBounce' });
				notify.create("New Document " + $("#NewFilename").val() + " was succesfully inserted.", "Info",{cls: "supertop"}); notify.reset();
		}
	} catch {}
}

function UpdateFileMenu(){
	let html = "<select data-role='select' id='FileMenu' data-empty-value='' data-on-change='FileMenuChange' data-add-empty-value='true' data-cls-option='m-0 p-0' data-filter-placeholder='Search' data-filter='true' >";
	let copyhtml = "<select data-role='select' id='CopyFileMenu' data-empty-value='' data-add-empty-value='true' data-cls-option='m-0 p-0' data-clear-button='true' data-filter-placeholder='Search' data-filter='true' >";
	let index = 0;
	$("#MemoryDocFilesCounter").html("Files Loaded in Memory: " + DocFiles.length);
	DocFiles.forEach(file =>{ 
		html += "<option value='" + file.filename + "'>" + file.filename + "</option>"; 
		copyhtml += "<option value='" + file.filename + "'>" + file.filename + "</option>"; 
	});
	html += "</select>";
	copyhtml += "</select>";
	$("#FileMenuRoot").html(html);
	$("#CopyFileMenuRoot").html(copyhtml);
}


async function ReloadFolderFiles(){
	DocFiles = [];
	let html = "<select data-role='select' id='FileMenu' data-empty-value='' data-on-change='FileMenuChange' data-add-empty-value='true' data-cls-option='m-0 p-0' data-filter-placeholder='Search' data-filter='true' >";
	let copyhtml = "<select data-role='select' id='CopyFileMenu' data-empty-value='' data-add-empty-value='true' data-cls-option='m-0 p-0' data-clear-button='true' data-filter-placeholder='Search' data-filter='true' >";
	let index = 0;
	$("#MemoryDocFilesCounter").html("Files Loaded in Memory: " + DocFiles.length);
	ConfigFiles.forEach(async file =>{ 
		await axios.get(window.location.origin + window.location.pathname +'data/' + file,{
			headers: { "Access-Control-Allow-Origin": "*", "Access-Control-Allow-Methods": "GET, POST, PUT" }
			}).then((response)=> {
				DocFiles.push({ filename: file, content: response.data, version: "none", lastUpdate: "none" });
				html += "<option value='" + file + "'>" + file + "</option>"; 
				copyhtml += "<option value='" + file + "'>" + file + "</option>"; 
			}).catch(function (error) {
				var notify = Metro.notify; notify.setup({ width: 300, duration: ConfigSystemMessageShowTime * 1000, animation: 'easeOutBounce' });
				notify.create("File " + file + " could not be loaded. Check existing.", "Info",{cls: "supertop"}); notify.reset();
			});
		index++;
		if (index === ConfigFiles.length) {
			$("#MemoryDocFilesCounter").html("Files Loaded in Memory: " + DocFiles.length);
			html += "</select>";
			copyhtml += "</select>";
			$("#FileMenuRoot").html(html);
			$("#CopyFileMenuRoot").html(copyhtml);
		}
	});
	
}

function ConfigSetProgramMenu(){
	if (ManagerMode == "editor") { 
		$("#FileMenuRoot")[0].setAttribute('disabled', '');
		$("#Mavon")[0].__vue__.$root.toolbar.save = false;
		$("#SummaryGenerator")[0].setAttribute('disabled', '');
		$("#ReloadFilesMenu")[0].setAttribute('disabled', '');
		$("#NewMemFileMenu")[0].setAttribute('disabled', '');
		$("#InfoEditorMode")[0].style.display = "inline-flex";
		$("#InfoFilesManagerMode")[0].style.display = "none";
		$("#InfoApiManagerMode")[0].style.display = "none";
		$("#mdbookGenerator")[0].setAttribute('disabled', '');
	} else if (ManagerMode == "filesEditor") { 
		$("#FileMenuRoot")[0].style.display = "inline-flex";
		$("#Mavon")[0].__vue__.$root.toolbar.save = true;
		$("#SummaryGenerator")[0].setAttribute('disabled', '');
		$("#ReloadFilesMenu")[0].removeAttribute('disabled');
		$("#NewMemFileMenu")[0].removeAttribute('disabled');
		$("#InfoEditorMode")[0].style.display = "none";
		$("#InfoFilesManagerMode")[0].style.display = "inline-flex";
		$("#InfoApiManagerMode")[0].style.display = "none";
		$("#mdbookGenerator")[0].setAttribute('disabled', '');
	} else if (ManagerMode == "apiManager") { 
		$("#FileMenuRoot")[0].style.display = "inline-flex";
		$("#Mavon")[0].__vue__.$root.toolbar.save = true;
		$("#SummaryGenerator")[0].removeAttribute('disabled');
		$("#ReloadFilesMenu")[0].removeAttribute('disabled');
		$("#NewMemFileMenu")[0].removeAttribute('disabled');
		$("#InfoEditorMode")[0].style.display = "none";
		$("#InfoFilesManagerMode")[0].style.display = "none";
		$("#InfoApiManagerMode")[0].style.display = "inline-flex";
		$("#mdbookGenerator")[0].removeAttribute('disabled');
		
	}
}

function googleTranslateElementInit() {
    new google.translate.TranslateElement({
        pageLanguage: 'en',
        //includedLanguages: 'en,cs',
        layout: google.translate.TranslateElement.InlineLayout.HORIZONTAL
    }, 'google_translate_element');

	/*
    if ($("#AutoTranslate")[0] != undefined && $("#AutoTranslate")[0].checked) {
		let WebPagesLanguage = (navigator.language || navigator.userLanguage).substring(0, 1); 
         setTimeout(function () {
                 let selectElement = document.querySelector('#google_translate_element select');
                 selectElement.value = WebPagesLanguage;
                 selectElement.dispatchEvent(new Event('change'));
         }, 1000);
    }
	*/
}

function ConfigMenuChange(tab,el){
	if (tab.id == "configModes" || tab.id == "configSystem") { $("#configButtons")[0].setAttribute('style', 'display:flex !important');} else {$("#configButtons")[0].setAttribute('style', 'display:none !important');}
}

function ChangeScheme() {
    //$("#color-scheme").attr("href", "/css/schemes/" + (Scheme =="light" ? "red-dark.min.css" : "sky-net.min.css"));
	if (Scheme == "light") {$("#body").removeClass("editor-dark");$("#body").addClass("editor-light");} else {$("#body").removeClass("editor-light");$("#body").addClass("editor-dark");}
	Scheme = Scheme == "light" ? "dark" : "light";
}

function showPageLoading() {
    if (pageLoader != undefined) { Metro.activity.close(pageLoader); }
    pageLoader = Metro.activity.open({
        role: 'dialog',
        type: 'bars',
        style: 'dark',
        overlayColor: 'transparent',
        overlayClickClose: true,
        overlayAlpha: 0,
    });
}

function hidePageLoading() {
    Metro.activity.close(pageLoader);
}

function FileMenuChange(fileName){
	DocFiles.forEach(file =>{
		if (file.filename == fileName) {
			$("#Mavon")[0].__vue__.d_value = file.content;
			MermaidConvert(); setTimeout(function () { MermaidConvert(); }, 1000);
			if (ManagerMode == "apiManager") {
				$("#MemoryDocFilesCounter").html("<div>Files Loaded in Memory: " + DocFiles.length + " </div><div style='font-size:10px;'> File Version: " + file.version + " LastUpdate: " + file.lastUpdate + "</div>");
			
			}
		}
	});
	DocContentChanged = false;
	document.querySelector("#Mavon > div.v-note-op > div.v-left-item.transition > button.op-icon.fa.fa-mavon-floppy-o").style.backgroundColor = "transparent";
	
}

function ShowHelps(helpType){
	switch (helpType[0]) {
			case "basics": 
			// var MdRenderer = MavonEditor.mavonEditor.getMarkdownIt();
			// var MdContent = MdRenderer.render(BasicHelp);
			
			$("#UpanelTitle").html("Basic Documentation Types Help");$("#UpanelContent").html(BasicHelp);
			Metro.infobox.open('#UniversalPanel');break;
			case "advanced": 
			$("#UpanelTitle").html("Advanced Documentation Types Help");$("#UpanelContent").html(AdvancedHelp);
			Metro.infobox.open('#UniversalPanel');break;
			case "mermaid": 
			$("#UpanelTitle").html("Advanced Documentation Types Help");$("#UpanelContent").html(MerMaidHelp);
			Metro.infobox.open('#UniversalPanel');break;
			case "modules": Metro.infobox.open('#ThirdPartyAddOns');break;
			case "codes": Metro.infobox.open('#ProgramLanguages');break;
			case "project": window.open("https://KlikneteZde.Cz:5000","_blabk");break;
			
	}
	if (helpType[0].length){ Metro.getPlugin($("#HelpMenu"),'select').reset();}
}

async function OutputAction(action){
showPageLoading();
	try{
		if (ConfigMermaidConvertOnExport){try{await mermaid.run({nodes: document.querySelectorAll('.lang-mermaid'),});} catch(err) {console.log(err);} }

		switch (action[0]) {
			case "html": 
				if ($("#AutoTranslateSelected").val('checked')[0].checked && Array.from($("#LanguageMenu")[0].selectedOptions).length > 1) {

					let UrlHtmlObject = window.URL || window.webkitURL;
						let HtmlBlob = new Blob([DocHeaderContent + document.querySelector("#Mavon > div.v-note-panel > div.v-note-show > div.v-show-content.scroll-style.scroll-style-border-radius").innerHTML +"</body></html>"], {type: 'text/plain'});
						let HtmlDownloadUrl = UrlHtmlObject.createObjectURL(HtmlBlob);
						let HtmlLink = document.createElement('a');
						if (typeof HtmlLink.download === 'undefined') {
							window.location.href = HtmlDownloadUrl;
						} else {
							let HtmlFileName;
							if ($("#FileMenu")[0].selectedOptions[0].value.length) {HtmlFileName = $("#FileMenu")[0].selectedOptions[0].value.split(".")[0] + '.html';} else { HtmlFileName = 'Export.html';}
							HtmlLink.download = HtmlFileName;
							HtmlLink.href = HtmlDownloadUrl;
							document.body.appendChild(HtmlLink);
							HtmlLink.click();
							setTimeout(function () { UrlHtmlObject.revokeObjectURL(HtmlDownloadUrl); }, 100);
						}

						let index = 0;
						Array.from($("#LanguageMenu")[0].selectedOptions).forEach(option => { 
							setTimeout(async function () {
								showPageLoading();
								let selectElement = document.querySelector('#google_translate_element select');
								selectElement.value = option.value;
								selectElement.dispatchEvent(new Event('change'));

								var notify = Metro.notify; notify.setup({ width: 300, duration: ConfigSystemMessageShowTime * 1000, animation: 'easeOutBounce' });
								notify.create("Waiting" + ConfigWaitingTimeInterval + " sec Interval for Translating", "Info",{cls: "supertop"}); notify.reset();

								setTimeout(async function () {
									if (ConfigMermaidConvertOnExport){try{await mermaid.run({nodes: document.querySelectorAll('.lang-mermaid'),});} catch(err) {console.log(err);} }

									 window.scrollTo(0, document.body.scrollHeight);
									  setTimeout(async function () {
										 window.scrollTo(0, 0);
										  setTimeout(async function () {
											let UrlHtmlObject = window.URL || window.webkitURL;
											let HtmlBlob = new Blob([DocHeaderContent + document.querySelector("#Mavon > div.v-note-panel > div.v-note-show > div.v-show-content.scroll-style.scroll-style-border-radius").innerHTML +"</body></html>"], {type: 'text/plain'});
											let HtmlDownloadUrl = UrlHtmlObject.createObjectURL(HtmlBlob);
											let HtmlLink = document.createElement('a');
											if (typeof HtmlLink.download === 'undefined') {
												window.location.href = HtmlDownloadUrl;
											} else {
												let HtmlFileName;
												if ($("#FileMenu")[0].selectedOptions[0].value.length) {HtmlFileName = $("#FileMenu")[0].selectedOptions[0].value.split(".")[0] + "_" + option.value + '.html';} else { HtmlFileName = 'Export_' + option.value + '.html';}

												HtmlLink.download = 'Export_' + option.value + '.html';
												HtmlLink.href = HtmlDownloadUrl;
												document.body.appendChild(HtmlLink);
												HtmlLink.click();
												setTimeout(async function () { UrlHtmlObject.revokeObjectURL(HtmlDownloadUrl); }, 100);
											}
										}, 3000);
									 }, 2000);
								 }, 2000);
							 }, (ConfigWaitingTimeInterval * 1000 * index) + 1000);
							index++;
							if(Array.from($("#LanguageMenu")[0].selectedOptions).length === index) { 
								console.log("reset time");
								setTimeout(async function () {
									let selectElement = document.querySelector('#google_translate_element select');selectElement.value = ConfigReturnToLanguage;selectElement.dispatchEvent(new Event('change'));
									hidePageLoading();
								},(ConfigWaitingTimeInterval * 1000 * Array.from($("#LanguageMenu")[0].selectedOptions).length) + 2000);
							}
						});
				} else {

					let UrlHtmlObject = window.URL || window.webkitURL;
					let HtmlBlob = new Blob([DocHeaderContent + document.querySelector("#Mavon > div.v-note-panel > div.v-note-show > div.v-show-content.scroll-style.scroll-style-border-radius").innerHTML +"</body></html>"], {type: 'text/plain'});
					let HtmlDownloadUrl = UrlHtmlObject.createObjectURL(HtmlBlob);
					let HtmlLink = document.createElement('a');
					if (typeof HtmlLink.download === 'undefined') {
						window.location.href = HtmlDownloadUrl;
					} else {
						let HtmlFileName;
						if ($("#FileMenu")[0].selectedOptions[0].value.length) {HtmlFileName = $("#FileMenu")[0].selectedOptions[0].value.split(".")[0] + '.html';} else { HtmlFileName = 'Export.html';}
						HtmlLink.download = HtmlFileName;
						HtmlLink.href = HtmlDownloadUrl;
						document.body.appendChild(HtmlLink);
						HtmlLink.click();
						setTimeout(function () { UrlHtmlObject.revokeObjectURL(HtmlDownloadUrl); }, 100);
					}

				}
			break;
			case "pdf":
				if ($("#AutoTranslateSelected").val('checked')[0].checked && Array.from($("#LanguageMenu")[0].selectedOptions).length > 1) {

					let PdfFileName;
					if ($("#FileMenu")[0].selectedOptions[0].value.length) {PdfFileName = $("#FileMenu")[0].selectedOptions[0].value.split(".")[0] + '.pdf';} else { PdfFileName = 'Export.pdf';}
					await makePDF(PdfFileName);

					let index = 0;
					Array.from($("#LanguageMenu")[0].selectedOptions).forEach(option => { 
							setTimeout(async function () {
								showPageLoading();
								let selectElement = document.querySelector('#google_translate_element select');
								selectElement.value = option.value;
								selectElement.dispatchEvent(new Event('change'));

								var notify = Metro.notify; notify.setup({ width: 300, duration: ConfigSystemMessageShowTime * 1000, animation: 'easeOutBounce' });
								notify.create("Waiting" + ConfigWaitingTimeInterval + " sec Interval for Translating", "Info",{cls: "supertop"}); notify.reset();

								setTimeout(async function () {
									if (ConfigMermaidConvertOnExport){try{await mermaid.run({nodes: document.querySelectorAll('.lang-mermaid'),});} catch(err) {console.log(err);} }

									 window.scrollTo(0, document.body.scrollHeight);
									  setTimeout(async function () {
										 window.scrollTo(0, 0);
										  setTimeout(async function () {

											let PdfFileName;
											if ($("#FileMenu")[0].selectedOptions[0].value.length) {PdfFileName = $("#FileMenu")[0].selectedOptions[0].value.split(".")[0] + "_" + option.value + '.pdf';} else { PdfFileName = 'Export_' + option.value + '.pdf';}
											await makePDF(PdfFileName);
										  }, 3000);
									 }, 2000);
								 }, 2000);
							 }, (ConfigWaitingTimeInterval * 1000 * index) + 1000);

							index++;
							if(Array.from($("#LanguageMenu")[0].selectedOptions).length === index) { 
								console.log("reset time");
								setTimeout(async function () {
									let selectElement = document.querySelector('#google_translate_element select');selectElement.value = ConfigReturnToLanguage;selectElement.dispatchEvent(new Event('change'));
									hidePageLoading();
								},(ConfigWaitingTimeInterval * 1000 * Array.from($("#LanguageMenu")[0].selectedOptions).length) + 2000);
							}
						});
				} else {
						
					let PdfFileName;
					if ($("#FileMenu")[0].selectedOptions[0].value.length) {PdfFileName = $("#FileMenu")[0].selectedOptions[0].value.split(".")[0] + '.pdf';} else { PdfFileName = 'Export.pdf';}
					await makePDF(PdfFileName);
				}
			break;
			case "md":
				
				let UrlMdObject = window.URL || window.webkitURL;
				let MdBlob = new Blob([DocContent], {type: 'text/plain'});
				let MdDownloadUrl = UrlMdObject.createObjectURL(MdBlob);
				let MdLink = document.createElement('a');
				if (typeof MdLink.download === 'undefined') {
					window.location.href = MdDownloadUrl;
				} else {
					let MdFileName;
					if ($("#FileMenu")[0].selectedOptions[0].value.length) {MdFileName = $("#FileMenu")[0].selectedOptions[0].value.split(".")[0] + '.md';} else { MdFileName = 'Export.md';}
					MdLink.download = MdFileName;
					MdLink.href = MdDownloadUrl;
					document.body.appendChild(MdLink);
					MdLink.click();
					setTimeout(function () { UrlMdObject.revokeObjectURL(MdDownloadUrl); }, 100);
				}

			break;
			default:
				break;
		}
	} catch(err) { console.log(err);
		var notify = Metro.notify; notify.setup({ width: 300, duration: ConfigSystemMessageShowTime * 1000, animation: 'easeOutBounce' });
			notify.create("Test Export Again, Mermaid Convert has problem", "Info",{cls: "supertop"}); notify.reset();
			hidePageLoading();
	}
	if (action[0].length){ Metro.getPlugin($("#OutputMenu"),'select').reset();}
	hidePageLoading();

}

async function makePDF(filename) {
	try{
		const htmlWidth = PrintArea.width();
		const htmlHeight = PrintArea.height();
		const topLeftMargin = 15;
		let pdfWidth = htmlWidth + (topLeftMargin * 2);
		let pdfHeight = (pdfWidth * 1.5) + (topLeftMargin * 2);
		const canvasImageWidth = htmlWidth;
		const canvasImageHeight = htmlHeight;
		const totalPDFPages = Math.ceil(htmlHeight / pdfHeight) - 1;

		const data = PrintArea;
		html2canvas(data.get(0), { allowTaint: true }).then(canvas => {

		  canvas.getContext('2d');
		  const imgData = canvas.toDataURL("image/jpeg", 1.0);
		  let pdf = new jsPDF('p', 'pt', [pdfWidth, pdfHeight]);
		  pdf.addImage(imgData, 'png', topLeftMargin, topLeftMargin, canvasImageWidth, canvasImageHeight);

		  for (let i = 1; i <= totalPDFPages; i++) {
			pdf.addPage([pdfWidth, pdfHeight], 'p');
			pdf.addImage(imgData, 'png', topLeftMargin, - (pdfHeight * i) + (topLeftMargin * 4), canvasImageWidth, canvasImageHeight);
		  }

		  pdf.save(filename);
		});
	} catch(err) { console.log(err);
		var notify = Metro.notify; notify.setup({ width: 300, duration: ConfigSystemMessageShowTime * 1000, animation: 'easeOutBounce' });
			notify.create("Pdf printing Failure. Please Inform supplier.", "Error",{cls: "supertop"}); notify.reset();
	}
}