//Start Declaration After Page Load
window.addEventListener('load', function () { var editor; });

//define object for bind and edit propertiesto edit -- required
//ContentTools.StylePalette.add([ new ContentTools.Style('*', '*', ['*'])]);
ContentTools.StylePalette.add([new ContentTools.Style('*', '*', ['*']), new ContentTools.Style('*', '*', ['span'])]);

//Cannot be Defined All - Editor is Content Also odefine bindable object <div data-editable data-name="main-content">
editor = ContentTools.EditorApp.get();editor.init('*[data-editable]');//editor.init('*', '*');


//Load Translation 
xhr = new XMLHttpRequest(); xhr.open('GET', '../../translations/cs.json', true);
function onStateChange(ev) {var translations;if (ev.target.readyState == 4) {translations = JSON.parse(ev.target.responseText);ContentEdit.addTranslations('cs', translations);ContentEdit.LANGUAGE = 'cs';}}
xhr.addEventListener('readystatechange', onStateChange);xhr.send(null);


//Insert All Object to Editable Collection
editor.myToolsState = 'all-tools';
ContentEdit.Root.get().bind('focus', function (element) {
    let NOT_EDITABLE_TOOLS = [[]];
    if (element.domElement().classList.contains('not-editable')) {
        if (editor.myToolsState != 'text-only') { editor.myToolsState = 'text-only'; editor.toolbox().tools(NOT_EDITABLE_TOOLS); }
    } else { if (editor.myToolsState != 'all-tools') { editor.myToolsState = 'all-tools'; editor.toolbox().tools(ContentTools.DEFAULT_TOOLS); } }
});


//Image Uploader Functionality
function rotateImage(direction) {
    var formData;
    xhrComplete = function (ev) { var response;
        if (ev.target.readyState != 4) { return; }
        xhr = null; xhrComplete = null; dialog.busy(false);
        if (parseInt(ev.target.status) == 200) {
            response = JSON.parse(ev.target.responseText);
            image = { size: response.size, url: response.url + '?_ignore=' + Date.now() };
            dialog.populate(image.url, image.size);
        } else { new ContentTools.FlashUI('no'); }
    }; dialog.busy(true);
    formData = new FormData(); formData.append('url', image.url); formData.append('direction', direction);
    xhr = new XMLHttpRequest(); xhr.addEventListener('readystatechange', xhrComplete);
    xhr.open('POST', '/rotate-image', true); xhr.send(formData);
}

function getImages() {
    var descendants, i, images; images = {};
    for (imageName in editor.regions()) {
        descendants = editor.regions()[imageName].descendants();
        for (i = 0; i < descendants.length; i++) {
            if (descendants[i].type() !== 'Image') { continue; }; images[descendants[i].attr('src')] = descendants[i].size()[0]; }
    }; return images;
}

function imageUploader(dialog) {
    var image, xhr, xhrComplete, xhrProgress;
    console.log("imageupploader", JSON.parse(JSON.stringify(dialog)));
    dialog.addEventListener('imageuploader.cancelupload', function () {
        if (xhr) { xhr.upload.removeEventListener('progress', xhrProgress); xhr.removeEventListener('readystatechange', xhrComplete); xhr.abort(); }
        dialog.state('empty');
    });
    dialog.addEventListener('imageuploader.clear', function () { dialog.clear(); image = null; });
    dialog.addEventListener('imageuploader.rotateccw', function () { rotateImage('CCW'); });
    dialog.addEventListener('imageuploader.rotatecw', function () { rotateImage('CW'); });

    dialog.addEventListener('imageuploader.fileready', function (ev) { var formData; var file = ev.detail().file;
        xhrProgress = function (ev) { dialog.progress((ev.loaded / ev.total) * 100); }
        xhrComplete = function (ev) {
            var response; 

            if (ev.target.readyState != 4) { return; }
            xhr = null; xhrProgress = null; xhrComplete = null;
            if (parseInt(ev.target.status) == 200) { response = JSON.parse(ev.target.responseText);
                image = { size: response.size, url: response.url };
                dialog.populate(image.url, image.size);
            } else { new ContentTools.FlashUI('no'); }
        }
        
        dialog.state('uploading'); dialog.progress(0);
        formData = new FormData(); formData.append('image', file);
        xhr = new XMLHttpRequest(); xhr.upload.addEventListener('progress', xhrProgress);
        xhr.addEventListener('readystatechange', xhrComplete); xhr.open('POST', window.location.origin + '/WebApi/PostApiFileRotator', true);
        xhr.send(formData);
    });


    dialog.addEventListener('imageuploader.save', function () {
        var crop, cropRegion, formData;
        xhrComplete = function (ev) {
            if (ev.target.readyState !== 4) { return; }
            xhr = null; xhrComplete = null; dialog.busy(false);
            if (parseInt(ev.target.status) === 200) {
                var response = JSON.parse(ev.target.responseText);
                dialog.save( response.url, response.size,
                    { 'alt': response.alt, 'data-ce-max-width': response.size[0] }); } else { new ContentTools.FlashUI('no'); }
        }
        dialog.busy(true);
        formData = new FormData();
        formData.append('url', image.url);
        formData.append('width', 600);
        if (dialog.cropRegion()) { formData.append('crop', dialog.cropRegion()); }
        xhr = new XMLHttpRequest();
        xhr.addEventListener('readystatechange', xhrComplete);
        xhr.open('POST', window.location.origin + '/WebApi/PutApiFileRotator', true);
        xhr.send(formData);
    });


}
ContentTools.IMAGE_UPLOADER = imageUploader;


editor.addEventListener('save', function (ev) {
    var regions = ev.detail().regions;

    // Collect the contents of each region into a FormData instance
    payload = new FormData();
    payload.append('page', window.location.pathname);
    payload.append('images', JSON.stringify(getImages()));
    payload.append('regions', JSON.stringify(regions));

    // Send the updated content to the server to be saved
    function onStateChange(ev) {
        // Check if the request is finished
        if (ev.target.readyState == 4) {
            editor.busy(false);
            if (status == '200') {
                // Save was successful, notify the user with a flash
                new ContentTools.FlashUI('ok');
            } else {
                // Save failed, notify the user with a flash
                new ContentTools.FlashUI('no');
            }
        }
    };

    xhr = new XMLHttpRequest();
    xhr.addEventListener('readystatechange', onStateChange);
    xhr.open('POST', '/x/save-page');
    xhr.send(payload);
});


// AutoSave every 30 seconds
//editor.addEventListener('start', function (ev) { var that = this; function autoSave() { this.save(true); }; this.autoSaveTimer = setInterval(autoSave, 30 * 1000); });
//editor.addEventListener('stop', function (ev) { clearInterval(this.autoSaveTimer); });

// Manually start/stop the editor
//editor.start();
//editor.stop(save);

