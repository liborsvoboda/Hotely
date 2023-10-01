/*Metro Storage Using HERE is Global Variable Definitions*/
//Metro.storage.setItem(key, value) - store any data to key
//Metro.storage.getItem(key, default ) - get stored values with key.


/*WebPages Language Variable*/
Metro.storage.setItem('WebPagesLanguage', (navigator.language || navigator.userLanguage).substring(0, 2));

/*WebPages Theme Scheme*/
if (Metro.storage.getItem('WebScheme', null) == null) {
    Metro.storage.setItem('WebScheme', "sky-net.min.css");
    ChangeSchemeTo(Metro.storage.getItem('WebScheme', null));
} else { ChangeSchemeTo(Metro.storage.getItem('WebScheme', null)); }


/*WebPages Automatic Translate*/
if (Metro.storage.getItem('AutomaticTranslate', null) == null) {
    Metro.storage.setItem('AutomaticTranslate', false);
}









