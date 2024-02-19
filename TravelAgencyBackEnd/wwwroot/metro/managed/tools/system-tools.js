
//Function for  Mermaid Data to Graphics Conversion
async function Mermaid() { try { await mermaid.run({ nodes: document.querySelectorAll('.class-mermaid'), }); } catch (err) { } }

//Function for Highlighting Code Segments
function HighlightCode() { document.querySelectorAll('div.code').forEach(el => { hljs.highlightElement(el); }); }


//Globální funkce Otevření Okna Konzole
function OpenGlobalConsole(){
	if($("#GlobalConsoleWindow")[0] == undefined){
		ScrollToTop();
		Metro.window.create({id:'GlobalConsoleWindow',title:"Console",shadow:true,draggable:true,modal:true,clsWindow:"supertop",icon:"<span class=\"mif-info\"</span>",btnClose:true,width:'40%',height:600,place:"center",content:"<iframe id=\"GlobalConsole\" src=\"../metro/tools/console/index.html\" style=\"width:100%;height:550px\"></iframe>"});
	}
}



//Globální funkce Otevření HTML Editoru nebo Obnovení Obsahu
function OpenGlobalSummernote(){
	if($("#GlobalSummernoteWindow")[0] == undefined){
		ScrollToTop();
		Metro.window.create({id:'GlobalSummernoteWindow',title:"HTML Editor",onWindowCreate:'ActivateGlobalSummernote()',shadow:true,draggable:true,modal:true,clsWindow:"supertop",icon:"<span class=\"mif-info\"</span>",btnClose:true,width:'50%',height:600,place:"center",content:"<div id='GlobalSummernoteEditor' ></div>"});
	} else { $('#GlobalSummernoteEditor').summernote('code',window.HtmlForGlobalSummernote); }
}


//Globální funkce Aktivace HTML Editoru
//use this Variable globally, can be used from more sources to One target 
window.HtmlForGlobalSummernote = null;
function ActivateGlobalSummernote(){
    $('#GlobalSummernoteEditor').summernote({
	    tabsize: 2, height: 600, maxHeight:null,
	    toolbar: [['style', ['style']], ['font', ['bold', 'underline', 'clear']], ['fontname', ['fontname']],
	    ['fontsize', ['fontsize']], ['color', ['color']], ['para', ['ul', 'ol', 'paragraph']], ['table', ['table']],
	    ['insert', ['link', 'picture', 'video']], ['view', ['fullscreen', 'codeview', 'undo', 'redo', 'help']]]
	});
	$('#GlobalSummernoteEditor').summernote('code',window.HtmlForGlobalSummernote);
}