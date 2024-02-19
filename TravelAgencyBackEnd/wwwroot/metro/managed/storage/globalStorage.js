//Web Portal Default Configuration

/* You Can Set External Backend Server */
Metro.storage.setItem('BackendServerAddress', window.location.origin);

/* Example */
// Metro.storage.setItem('BackendServerAddress', "https://kliknetezde.cz");


/*Start Set Global Constants*/
Metro.storage.setItem('ApiOriginSuffix', Metro.storage.getItem('BackendServerAddress', null) + "/WebApi");
Metro.storage.setItem('DetectedLanguage', (navigator.language || navigator.userLanguage).substring(0, 2));
/*End Global Constants*/


/*WebPages Theme Scheme*/
if (Metro.storage.getItem('WebScheme', null) == null) {
    Metro.storage.setItem('WebScheme', "sky-net.css");
    ChangeSchemeTo(Metro.storage.getItem('WebScheme', null));
} else { ChangeSchemeTo(Metro.storage.getItem('WebScheme', null)); }

/* End Global Default Setting*/


//Start Set User Default Setting
if (Metro.storage.getItem('UserAutomaticTranslate', null) == null) { Metro.storage.setItem('UserAutomaticTranslate', false); }








