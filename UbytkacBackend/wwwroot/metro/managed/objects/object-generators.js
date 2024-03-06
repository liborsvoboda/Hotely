



//Generate Global ToolBox Window Browser
async function GenerateGlobalToolBox(){
	let data = Metro.storage.getItem('SolutionToolList', null); let htmlContent = ""; let lastGroup = null;

	data.forEach(tool => {

        if (lastGroup == null || (lastGroup != null && tool.toolType.name != lastGroup)) {
            if (lastGroup != null) { htmlContent += "</ul></div></div>"; }

            htmlContent += "<div id='ToolBoxGroup_" + tool.toolType.id + "'  data-role='panel' data-on-expand='CloseOtherToolBoxList' data-title-caption='" + tool.toolType.name + "' class='panel drop-shadow' style='zoom:0.8;'";
            htmlContent += " data-cls-title-caption='pl-3 text-left' data-collapsed='true' data-collapsible='true'><div class='info-box-content p-2 m-0' title='" + tool.toolType.description + "' ><ul class='skills'>";
        }
        htmlContent += "<li class='c-pointer m-0 p-0' title='" + tool.description + "' onclick='ShowToolBoxCode(\"" + tool.id + "\")' ><span class='icon mif-" + tool.iconName + " mr-2'></span>" + tool.systemName + "</li>";

        lastGroup = tool.toolType.name;
    }); htmlContent += "</ul></div></div>";
    $("#ToolBoxPanel").html(htmlContent);
    $('#ToolBoxSummerEditor').summernote({
        tabsize: 2, height: 600, maxHeight: null,
        toolbar: [['style', ['style']], ['font', ['bold', 'underline', 'clear']], ['fontname', ['fontname']],
        ['fontsize', ['fontsize']], ['color', ['color']], ['para', ['ul', 'ol', 'paragraph']], ['table', ['table']],
        ['insert', ['link', 'picture', 'video']], ['view', ['fullscreen', 'codeview', 'undo', 'redo', 'help']]]
    });
    Metro.infobox.open('#GlobalToolBox');
}






//Generate Global TemplateDocBox Files Browser
async function GenerateTemplateDocBox() {
    let data = Metro.storage.getItem('TemplateDocList', null); let htmlContent = ""; let lastGroup = null;

    data.forEach(template => {

        if (lastGroup == null || (lastGroup != null && template.group.name != lastGroup)) {
            if (lastGroup != null) { htmlContent += "</ul></div></div>"; }

            htmlContent += "<div id='TemplateDocGroup_" + template.group.id + "'  data-role='panel' data-on-expand='CloseOtherTemplateDocList' data-title-caption='" + template.group.name + "' class='panel drop-shadow' style='zoom:0.8;'";
            htmlContent += " data-cls-title-caption='pl-3 text-left' data-collapsed='true' data-collapsible='true'><div class='info-box-content p-2 m-0' title='" + template.group.description + "' ><ul class='skills'>";
        }
        htmlContent += "<li class='c-pointer m-0 p-0' title='" + template.description + "' onclick='ShowTemplateCode(\"" + template.id + "\")' >" + template.name + "</li>";

        lastGroup = template.group.name;
    }); htmlContent += "</ul></div></div>";

    //insert WebCodeLibrary
    if (Metro.storage.getItem('TemplateWebCodeLibraryList', null) == null) { await GetTemplateWebCodeLibraryList(); }
    data = Metro.storage.getItem('TemplateWebCodeLibraryList', null); let startcode = true;
    data.forEach(codelibrary => {
        if (startcode) {
            htmlContent += "<div id='TemplateDocGroup_code' data-role='panel' data-on-expand='CloseOtherTemplateDocList' data-title-caption='Systémová Knihovna WEB Kódů' class='panel drop-shadow' style='zoom:0.8;'";
            htmlContent += " data-cls-title-caption='pl-3 text-left' data-collapsed='true' data-collapsible='true'><div class='info-box-content p-2 m-0' title='Plně Sdílená Knihona pro ještě snažší vývoj' ><ul class='skills'>";
        }
        htmlContent += "<li class='c-pointer m-0 p-0' title='" + codelibrary.description + "' onclick='ShowTemplateCode(\"code" + codelibrary.id + "\")' >" + codelibrary.name + "</li>";
        startcode = false;
    }); htmlContent += "</ul></div></div>";

    //insert webMenuLiList
    if (Metro.storage.getItem('TemplateWebMenuList', null) == null) { await GetTemplateWebMenuList(); }
    data = Metro.storage.getItem('TemplateWebMenuList', null); let startmenu = true;
    data.forEach(menuList => {
        if (startmenu) {
            htmlContent += "<div id='TemplateDocGroup_menu' data-role='panel' data-on-expand='CloseOtherTemplateDocList' data-title-caption='Kompletní Kód WEB Portálu' class='panel drop-shadow' style='zoom:0.8;'";
            htmlContent += " data-cls-title-caption='pl-3 text-left' data-collapsed='true' data-collapsible='true'><div class='info-box-content p-2 m-0' title='Plně Sdílený Kód tohoto Portálu můžete použít jako Inspiraci' ><ul class='skills'>";
        }
        htmlContent += "<li class='c-pointer m-0 p-0' title='" + menuList.description + "' onclick='ShowTemplateCode(\"menu" + menuList.id + "\")' >" + menuList.name + "</li>";
        startmenu = false;
    }); htmlContent += "</ul></div></div>";
    
    
    //insert managedScriptList
    if (Metro.storage.getItem('ManagedJsCssList', null) == null) { await GetManagedJsCssList(); }
    data = Metro.storage.getItem('ManagedJsCssList', null); let startscript = true;
    data.forEach(scriptList => {
        if (startscript) {
        	htmlContent += "<div id='TemplateDocGroup_script' data-role='panel' data-on-expand='CloseOtherTemplateDocList' data-title-caption='Řídící Scripty Portálu' class='panel drop-shadow' style='zoom:0.8;'";
        	htmlContent += " data-cls-title-caption='pl-3 text-left' data-collapsed='true' data-collapsible='true'><div class='info-box-content p-2 m-0' title='Plně Sdílený Kód tohoto Portálu můžete použít jako Inspiraci' ><ul class='skills'>";
        }
        let fileExt = scriptList.fileName.split(".").pop();
        if(scriptList.guestFileContent.length > 0){
        	htmlContent += "<li class='c-pointer m-0 p-0' title='" + scriptList.description + "' onclick='ShowTemplateCode(\"script_" + scriptList.id + "_guest\")' >" + scriptList.fileName + "</li>";
        }
        if(scriptList.userFileContent.length > 0){
        	htmlContent += "<li class='c-pointer m-0 p-0' title='" + scriptList.description + "' onclick='ShowTemplateCode(\"script_" + scriptList.id + "_user\")' >" + scriptList.fileName.replace(fileExt,"user." + fileExt) + "</li>";
		}
        if(scriptList.adminFileContent.length > 0){
        	htmlContent += "<li class='c-pointer m-0 p-0' title='" + scriptList.description + "' onclick='ShowTemplateCode(\"script_" + scriptList.id + "_admin\")' >" + scriptList.fileName.replace(fileExt,"admin." + fileExt) + "</li>";
		}
        if(scriptList.providerContent.length > 0){
        	htmlContent += "<li class='c-pointer m-0 p-0' title='" + scriptList.description + "' onclick='ShowTemplateCode(\"script_" + scriptList.id + "_provider\")' >" + scriptList.fileName.replace(fileExt,"provider." + fileExt) + "</li>";
		}
		
        startscript = false;
    }); htmlContent += "</ul></div></div>";

    $('#TemplateCodeEditorSM').summernote({
        tabsize: 2, height: 600, maxHeight: null,
        toolbar: [['style', ['style']], ['font', ['bold', 'underline', 'clear']], ['fontname', ['fontname']],
        ['fontsize', ['fontsize']], ['color', ['color']], ['para', ['ul', 'ol', 'paragraph']], ['table', ['table']],
        ['insert', ['link', 'picture', 'video']], ['view', ['fullscreen', 'codeview', 'undo', 'redo', 'help']]]
    });
    $("#TemplateDocPanel").html(htmlContent);
    Metro.infobox.open('#GlobalTemplateBox');
}





