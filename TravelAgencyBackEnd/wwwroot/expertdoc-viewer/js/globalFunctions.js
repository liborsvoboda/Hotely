// local variables
let ConfigDefaultContent = "### MarkDown Item Template   \r\n```csharp   \r\n\r\n```   \r\n---    \r\n";

//API Part

async function ApiLogin(writeMessage){

	//showPageLoading();
	//var def = $.ajax({
	//	global: false, type: "POST", url: ConfigApiServer.ServerAuthAddress, dataType: 'json',
	//	headers: { "Authorization": "Basic " + btoa(ConfigApiServer.BasicAuthLoginName + ":" + ConfigApiServer.BasicAuthLoginPassword) }
	//});

	//def.fail(function (data) {
	//	var notify = Metro.notify; notify.setup({ width: 300, duration: ConfigSystemMessageShowTime * 1000, animation: 'easeOutBounce' });
	//	notify.create("Login Failed", "Alert", { cls: "alert" }); notify.reset();
	//	hidePageLoading();
	//	return false;
	//});

	//def.done(function (data) {
	//	if (writeMessage) {
	//		var notify = Metro.notify; notify.setup({ width: 300, duration: ConfigSystemMessageShowTime * 1000, animation: 'easeOutBounce' });
	//		notify.create("Login Succesfull", "Info", { cls: "info" }); notify.reset();
	//	} else {
	//		console.log("User Logged");
	//		Metro.storage.setItem('ApiToken', data.Token);
	//		DownloadApiFiles(true);
	//	}
	//});
	DownloadApiFiles(false);
}

async function ReloadFiles() {
	if (ManagerMode == "apiManager") { await DownloadApiFiles(true); }
	else { ReloadFolderFiles() }
}

async function DownloadApiFiles(onlyReload) {
	DocFiles = [];
	showPageLoading();

	var def = $.ajax({
		global: false, type: "GET", url: ConfigApiServer.ServerDataApiAddress , dataType: 'json',
		contentType: 'application/json'
	}).fail(function (data) {
		var notify = Metro.notify; notify.setup({ width: 300, duration: 1000, animation: 'easeOutBounce' });
		notify.create("Failed load Api Documenation List", "Alert", { cls: "alert" }); notify.reset();
		hidePageLoading();
	}).done(function (data) {
		hidePageLoading();
		if (data.length > 0) {
			data.forEach(doc => {
				DocFiles.push({
					groupId: doc.documentationGroupId,
					groupName: doc.documentationGroup.name,
					title: doc.description,
					filename: doc.name, content: (doc.mdContent == null) ? "" : doc.mdContent,
					version: doc.autoVersion, lastUpdate: new Date(doc.timeStamp).toLocaleString()
				});
			});
		}
		
		if (onlyReload) { UpdateFileMenu(); }
		else { startupLoading(); }
	});

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

	} catch(err) { StartupProcessInfo("Configuration setting format problem detected" + err.message,true); }
	await StartupConfiguration();
}

async function StartupConfiguration(){
	try{
		StartupProcessInfo("Configuration setting are ok",false);
	} catch(err) { StartupProcessInfo("Configuration setting format problem detected" + err.message,true); }
	
	await StartupModeSelection();
}


async function StartupModeSelection(){

	try {

		if (ConfigApiServer.ServerDataApiAddress != null && ConfigApiServer.ServerDataApiAddress.length > 0) {
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

function UpdateFileMenu() {
	let html = "<ul id='FileMenu' class='sidebar-menu'>";
	let index = 0; firstMenuItem = null;
	$("#MemoryDocFilesCounter").html("Files Loaded in Memory: " + DocFiles.length);
	let groupMenuId = null;
	DocFiles.forEach(file => {
		firstMenuItem = firstMenuItem == null ? file.filename : firstMenuItem;
		if (groupMenuId != file.groupId) {
			if (groupMenuId != null) { html += "<li class='divider'</li>"; }
			html += "<li class='group-title fg-cyan' title='" + file.title + "' >" + file.groupName + "</li>";
		}
		html += "<li id='" + file.filename + "' class='pt-2 pb-2 pl-3' style='font-size: 12px;' onclick='FileMenuChange(this.id)'  title='" + file.title + "'  >" + file.filename + "</li>"; 

		groupMenuId = file.groupId;
	});
	html += "</ul>";
	$("#FileMenuRoot").html(html);
	FileMenuChange(firstMenuItem);
}


async function ReloadFolderFiles(){
	DocFiles = [];
	let html = "<ul id='FileMenu' class='sidebar-menu'>";
	let index = 0; firstMenuItem = null;
	$("#MemoryDocFilesCounter").html("Files Loaded in Memory: " + DocFiles.length);
	ConfigFiles.forEach(async file =>{ 
		await axios.get(window.location.origin + window.location.pathname +'data/' + file,{
			headers: { "Access-Control-Allow-Origin": "*", "Access-Control-Allow-Methods": "GET, POST, PUT" }
			}).then((response) => {
				firstMenuItem = firstMenuItem == null ? file : firstMenuItem;
				DocFiles.push({ filename: file, content: response.data, version: "none", lastUpdate: "none" });
				html += "<li id='" + file + "' class='pt-2 pb-2 text-bold pl-3' onclick='FileMenuChange(this.id)' >" + file + "</li>"; 
			}).catch(function (error) {
				var notify = Metro.notify; notify.setup({ width: 300, duration: ConfigSystemMessageShowTime * 1000, animation: 'easeOutBounce' });
				notify.create("File " + file + " could not be loaded. Check existing.", "Info",{cls: "supertop"}); notify.reset();
			});
		index++;
		if(index === ConfigFiles.length) { 
			$("#MemoryDocFilesCounter").html("Files Loaded in Memory: " + DocFiles.length);
			html += "</ul>";
			$("#FileMenuRoot").html(html);
			FileMenuChange(firstMenuItem);
		}
	});
}

function ConfigSetProgramMenu(){
	if (ManagerMode == "editor") { 
		$("#ReloadFilesMenu")[0].setAttribute('disabled', '');
	} else if (ManagerMode == "filesEditor") { 
		$("#ReloadFilesMenu")[0].removeAttribute('disabled');
	} else if (ManagerMode == "apiManager") { 
		$("#ReloadFilesMenu")[0].removeAttribute('disabled');

		
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

async function FileMenuChange(fileName) {
	DocFiles.forEach(async file =>{
		if (file.filename == fileName) {
			$("#Mavon")[0].__vue__.d_value = file.content;
			if (ManagerMode == "apiManager") {
				$("#MemoryDocFilesCounter").html("<div>Files Loaded in Memory: " + DocFiles.length + " </div><div style='font-size:10px;'> File Version: " + file.version + " LastUpdate: " + file.lastUpdate + "</div>");
			}
			await mermaid.run({
				nodes: document.querySelectorAll('.lang-mermaid'),
			});

			setTimeout(async function () {
				await mermaid.run({
					nodes: document.querySelectorAll('.lang-mermaid'),
				});
			}, 5000);
		}
	});
	DocContentChanged = false;
}

async function OutputAction(action){
showPageLoading();
	try{
		if (ConfigMermaidConvertOnExport){try{await mermaid.run({nodes: document.querySelectorAll('.lang-mermaid'),});} catch(err) {console.log(err);} }

		switch (action[0]) {
			case "html": 
				let UrlHtmlObject = window.URL || window.webkitURL;
				let HtmlBlob = new Blob([DocHeaderContent + document.querySelector("#Mavon > div.v-note-panel > div.v-note-show > div.v-show-content.scroll-style.scroll-style-border-radius").innerHTML +"</body></html>"], {type: 'text/plain'});
				let HtmlDownloadUrl = UrlHtmlObject.createObjectURL(HtmlBlob);
				let HtmlLink = document.createElement('a');
				if (typeof HtmlLink.download === 'undefined') {
					window.location.href = HtmlDownloadUrl;
				} else {
					let HtmlFileName;
					HtmlFileName = 'Export.html';
					HtmlLink.download = HtmlFileName;
					HtmlLink.href = HtmlDownloadUrl;
					document.body.appendChild(HtmlLink);
					HtmlLink.click();
					setTimeout(function () { UrlHtmlObject.revokeObjectURL(HtmlDownloadUrl); }, 100);
				}
			break;
			case "pdf":
					let PdfFileName;
					PdfFileName = 'Export.pdf';
					await makePDF(PdfFileName);
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
					MdFileName = 'Export.md';
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