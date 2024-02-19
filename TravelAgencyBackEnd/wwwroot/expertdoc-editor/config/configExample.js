// Set Configuration
// Without setting run by index.html directly as Builder
// Configuration Standalone is for Manage/Show Saved files 
// Configuration Api is for Manage/Show Api files with Automatic Versioning

//configuration class definition { Name,AutoVersion,Description,TimeStamp}

// !!! Select One Of Settings Option !!! Run Only First setted Variant !!!

// Its Manage Files in ProgramFolder/data !! Run only If its running from WebServer
// default []
// example ['test.md','test1.md']
let ConfigFiles = ['helpFullEn.md','help_en.md','MerMaid.md'];

// Insert Documentation Server Api Url !! Run only If its running from WebServer
// default null
// http://127.0.0.1:3000/GetDocSrvDocumentationListApi
let ConfigApiServer = {
    BasicAuthLoginName: 'tester',
    BasicAuthLoginPassword: 'tester',
    ServerApiAddress: 'http:127.0.0.1:5000'
}

// its only Load MarkDown file from URL for Modify and Export
// Cors need running Https
// default null
// example "https://some.com/makdown.md"
let ConfigUrlMdFile = "https://kliknetezde.cz:5000/tools/EDC_ESB_InteliHelp/src/README.md";


//END MODES



// Default Template file from "config" Folder
let ConfigDefaultTemplate = "defaultTemplate.md";

// Automatic Export file on saving to memory
let ConfigExportFileOnSaving = true;
//TODO not used
let ConfigEnableCreateNewDoc = true;
// System message Show Time
let ConfigSystemMessageShowTime = 5;

//Its needed for multiple export
let ConfigMermaidConvertOnExport = true;

//TODO not used
let ConfigSelectedLanguages = ['cs','en','de','fr'];

// Preparing Time for Multiple Export
let ConfigWaitingTimeInterval = 10;

// Enablwe Automatic Translation on Startup
let ConfigAutoMultiTranslateEnabled = true;

// Return to Language After Multi Translation
let ConfigReturnToLanguage = 'en';