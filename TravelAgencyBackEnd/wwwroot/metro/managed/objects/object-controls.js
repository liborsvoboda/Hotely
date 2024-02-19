
//Control Translation Panel
function ShowToolPanel(close) {
    $("#UserAutomaticTranslate").val('checked')[0].checked = Metro.storage.getItem('UserAutomaticTranslate', null);
    if (close) { { Metro.bottomsheet.close($('#toolPanel')); } } else {
        if (Metro.bottomsheet.isOpen($('#toolPanel'))) { Metro.bottomsheet.close($('#toolPanel')); }
        else { Metro.bottomsheet.open($('#toolPanel')); }
    }
}


//Control Global Documentation Window
function ShowGlobalDocBox() {
    if (Metro.infobox.isOpen('#GlobalDocBox')) { Metro.infobox.close('#GlobalDocBox'); }
    else { Metro.infobox.open('#GlobalDocBox'); }
}


//Control Global News Window
function ShowNewsInfoBox() {
    if (Metro.infobox.isOpen('#NewsInfoBox')) { Metro.infobox.close('#NewsInfoBox'); }
    else { GetNewsList(); }
};


//Control Global Templates Manager Window
async function ShowGlobalTemplateBox() {
    if (Metro.infobox.isOpen('#GlobalTemplateBox')) {$("#GTBhtml")[0].style.visibility = "hidden"; Metro.infobox.close('#GlobalTemplateBox'); }
    else {$("#GTBhtml")[0].style.visibility = "visible"; await GetTemplateDocList(); }
}

//Control Global Tool Window 
function ShowGlobalToolBox() {
    if (Metro.infobox.isOpen('#GlobalToolBox')) { Metro.infobox.close('#GlobalToolBox'); }
    else { GetSolutionToolList(); }
}


//Control Show Message Panel
function ShowMessagePanel(close) {
    charms = Metro.getPlugin($("#charmPanel"), 'charms');
    if (close) {
        charms.close();
    } else { charms.toggle(); }
}

//Control Close All Global Window Before Open New
function CloseAllGlobalWindow(val) {
	if (val !="GlobalDocBox") { Metro.infobox.close('#GlobalDocBox'); }
	if (val !="GlobalTemplateBox") { Metro.infobox.close('#GlobalTemplateBox'); }
	if (val !="NewsInfoBox") { Metro.infobox.close('#NewsInfoBox');}
	if (val !="GlobalToolBox") { Metro.infobox.close('#GlobalToolBox');}
}

//Control Header Menu WebPage Mottos Cycling
function ShowWebPageMottos(){
	if(Metro.storage.getItem('CycleWebPageMottosEnabled', null) != null && Metro.storage.getItem('CycleWebPageMottosEnabled', null)){
		setTimeout(function () { 
		try {
            let data = Metro.storage.getItem('MottoList', null); let setNext = false; let setted = false;

            var BreakException = {};
            try {
                data.forEach((motto, index) => {
                    if (Metro.storage.getItem('LastMottoId', null) == null || setNext) { Metro.storage.setItem('LastMottoId', motto.id); $("#WebPageMottos").html(motto.systemName); setted = true; throw BreakException; }
                    if (Metro.storage.getItem('LastMottoId', motto.id) == motto.id) { setNext = true; }
                    if (data.length - 1 == index && !setted) { Metro.storage.setItem('LastMottoId', data[0].id); $("#WebPageMottos").html(data[0].systemName); }
                });
            } catch (e) { if (e !== BreakException) throw e; }
            setTimeout(function () { ShowWebPageMottos(); }, (Metro.storage.getItem('CycleWebPageMottosSecTime', null) != null ? Metro.storage.getItem('CycleWebPageMottosSecTime', null) * 1000 : 10000));
		} catch { } }, (Metro.storage.getItem('CycleWebPageMottosSecTime', null) != null ? Metro.storage.getItem('CycleWebPageMottosSecTime', null) * 1000 : 10000 ));
	}
}



//-----------------------------------------------------------------------------------


//Control Close Other TamplateBox Group panel menu 
function CloseOtherTemplateDocList(element) {

    if (element.id != "TemplateDocGroup_code") { Metro.getPlugin('#TemplateDocGroup_code', 'panel').collapse(); }
    if (element.id != "TemplateDocGroup_menu") { Metro.getPlugin('#TemplateDocGroup_menu', 'panel').collapse(); }
    if (element.id != "TemplateDocGroup_script") { Metro.getPlugin('#TemplateDocGroup_script', 'panel').collapse(); }

    let data = Metro.storage.getItem('TemplateDocList', null);
    data.forEach(template => {
        if (template.group.id != element.id.split("_")[1]) { Metro.getPlugin('#TemplateDocGroup_' + template.group.id, 'panel').collapse(); }
    });
}

//Templates Global Help Load Selected Code Item to Editor
function ShowTemplateCode(id) {
    if (id.toString().indexOf("code") > -1) {
        let data = Metro.storage.getItem('TemplateWebCodeLibraryList', null);
        data.forEach(codelibrary => {
            if ("code" + codelibrary.id == id) {
                window.HtmlForGlobalSummernote = codelibrary.htmlContent;
                if ($("#VueTemplateSection")[0].style.visibility != "hidden") { $("#TemplateCodeEditor")[0].__vnode.ctx.data.content = codelibrary.htmlContent; }
                else { $("#TemplateCodeEditorSM").summernote('code', codelibrary.htmlContent); }
            }
        });
    } else if (id.toString().indexOf("menu") > -1) {
        let data = Metro.storage.getItem('TemplateWebMenuList', null);
        data.forEach(menuList => {
            if ("menu" + menuList.id == id) {
                window.HtmlForGlobalSummernote = menuList.htmlContent;
                if ($("#VueTemplateSection")[0].style.visibility != "hidden") { $("#TemplateCodeEditor")[0].__vnode.ctx.data.content = menuList.htmlContent; }
                else { $("#TemplateCodeEditorSM").summernote('code', menuList.htmlContent); }
            }
        });
    } else if (id.toString().indexOf("script") > -1) {
        let data = Metro.storage.getItem('ManagedJsCssList', null);
        data.forEach(scriptList => {
            if ("script_" + scriptList.id + "_guest" == id) {
                window.HtmlForGlobalSummernote = scriptList.guestFileContent;
                if ($("#VueTemplateSection")[0].style.visibility != "hidden") { $("#TemplateCodeEditor")[0].__vnode.ctx.data.content = scriptList.guestFileContent; }
                else { $("#TemplateCodeEditorSM").summernote('code', scriptList.guestFileContent); }
            }
            if ("script_" + scriptList.id + "_user" == id) {
                window.HtmlForGlobalSummernote = scriptList.userFileContent;
                if ($("#VueTemplateSection")[0].style.visibility != "hidden") { $("#TemplateCodeEditor")[0].__vnode.ctx.data.content = scriptList.userFileContent; }
                else { $("#TemplateCodeEditorSM").summernote('code', scriptList.userFileContent); }
            }
            if ("script_" + scriptList.id + "_admin" == id) {
                window.HtmlForGlobalSummernote = scriptList.adminFileContent;
                if ($("#VueTemplateSection")[0].style.visibility != "hidden") { $("#TemplateCodeEditor")[0].__vnode.ctx.data.content = scriptList.adminFileContent; }
                else { $("#TemplateCodeEditorSM").summernote('code', scriptList.adminFileContent); }
            }
            if ("script_" + scriptList.id + "_provider" == id) {
                window.HtmlForGlobalSummernote = scriptList.providerContent;
                if ($("#VueTemplateSection")[0].style.visibility != "hidden") { $("#TemplateCodeEditor")[0].__vnode.ctx.data.content = scriptList.providerContent; }
                else { $("#TemplateCodeEditorSM").summernote('code', scriptList.providerContent); }
            }
        });
    } else {
        let data = Metro.storage.getItem('TemplateDocList', null);
        data.forEach(template => {
            if (template.id == id) {
                window.HtmlForGlobalSummernote = template.template;
                if ($("#VueTemplateSection")[0].style.visibility != "hidden") { $("#TemplateCodeEditor")[0].__vnode.ctx.data.content = template.template; }
                else { $("#TemplateCodeEditorSM").summernote('code', template.template); }
            }
        });
    }
}

//GlobalTemplateDocsChanger Changing Html/Js Editor
function GlobalTemlateDocChangeRunner() {
    if ($("#VueTemplateSection")[0].style.visibility != "hidden") {
        $("#TemplateCodeEditorSM").summernote('code', window.HtmlForGlobalSummernote);
        $("#VueTemplateSection")[0].style.visibility = "hidden"; $("#VueTemplateSection")[0].style.height = "0px";
        $("#HtmlTemplateSection")[0].style.visibility = "visible"; $("#HtmlTemplateSection")[0].style.height = "600px";
        $("#GTBjs")[0].style.visibility = "hidden"; $("#GTBhtml")[0].style.visibility = "visible";
    } else {
        $("#TemplateCodeEditor")[0].__vnode.ctx.data.content = window.HtmlForGlobalSummernote;
        $("#VueTemplateSection")[0].style.visibility = "visible"; $("#VueTemplateSection")[0].style.height = "600px";
        $("#HtmlTemplateSection")[0].style.visibility = "hidden"; $("#HtmlTemplateSection")[0].style.height = "0px";
        $("#GTBjs")[0].style.visibility = "visible"; $("#GTBhtml")[0].style.visibility = "hidden";
    }
}


//-----------------------------------------------------------------------------------


//Global ToolBoxOtherGroupClosing Control
function CloseOtherToolBoxList(element) {
    let data = Metro.storage.getItem('SolutionToolList', null);
    data.forEach(tool => { if (tool.toolType.id != element.id.split("_")[1]) { Metro.getPlugin('#ToolBoxGroup_' + tool.toolType.id, 'panel').collapse(); } });
}


//Show Control ToolBoxSelected Item in Editor/Viewer
window.ToolBoxTargetLink = null;
window.ToolBoxTargetContent = null;
function ShowToolBoxCode(id) {
    let data = Metro.storage.getItem('SolutionToolList', null);
    data.forEach(tool => {
        if (tool.id == id) {
            window.ToolBoxTargetLink = tool.command;
            $('#ToolBoxSummerEditor').summernote('code', '<iframe id="ToolBoxTarget" src="' + tool.command + '" style="width:100%;height:calc(100% - 60px);"></iframe>');
        }
    });
 }
