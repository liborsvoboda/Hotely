import store from "../../store/index";

//Global Functions Library

window.WizardRequestCityList =function WizardRequestCityList(value) {
    watchGlobalVariables.wizardRequestCityList = value;
}; 






//WIZARD FUNCTIONS
let ValidationWizardStatus = false; window.ValidationWizardStatus = ValidationWizardStatus;
let ActualValidationFormName = "hotelForm"; window.ActualValidationFormName = ActualValidationFormName;
let ActualWizardPage = 1; window.ActualWizardPage = ActualWizardPage;
let WizardImageGallery = []; window.WizardImageGallery = WizardImageGallery;


window.ValidateForm = function ValidateForm() { console.log("validate form");
    window.ValidationWizardStatus = ValidationWizardStatus = true;

    if (ActualWizardPage == 1 && ActualValidationFormName == "hotelForm") {
        window.ActualValidationFormName = ActualValidationFormName = "galleryForm";
        window.ActualWizardPage = ActualWizardPage = ActualWizardPage + 1;
        window.ValidationWizardStatus = ValidationWizardStatus = false;
    } else if (ActualWizardPage == 2 && ctualValidationFormName == "galleryForm") {
        window.ActualValidationFormName = ActualValidationFormName = "roomForm";
        window.ActualWizardPage = ActualWizardPage = ActualWizardPage + 1;
        window.ValidationWizardStatus = ValidationWizardStatus = false;
    } else if (ActualWizardPage == 3 && ctualValidationFormName == "roomForm") {
        window.ActualValidationFormName = ActualValidationFormName = "propertyForm";
        window.ActualWizardPage = ActualWizardPage = ActualWizardPage + 1;
        window.ValidationWizardStatus = ValidationWizardStatus = false;
    } else if (ActualWizardPage == 4 && ctualValidationFormName == "propertyForm") {
        window.ActualValidationFormName = ActualValidationFormName = "previewForm";
        window.ActualWizardPage = ActualWizardPage = ActualWizardPage + 1;
        window.ValidationWizardStatus = ValidationWizardStatus = false;
    }

    $("#AdvertisementWizard")[0]["DATASET:UID:M4Q"].wizard.toPage(ActualWizardPage);
};


window.InvalidForm = function InvalidForm() {
    //When bo back and again to done is validate last Form from other Page
    if (ActualWizardPage > $("#AdvertisementWizard")[0]["DATASET:UID:M4Q"].wizard.current)
    { $("#AdvertisementWizard")[0]["DATASET:UID:M4Q"].wizard.toPage($("#AdvertisementWizard")[0]["DATASET:UID:M4Q"].wizard.current + 1); }

    window.ValidationWizardStatus = ValidationWizardStatus = false;
};



window.WizardGallerySetPrimaryImage = function WizardGallerySetPrimaryImage(filename) {
    WizardImageGallery.forEach(image => {
        if (image.FileName == filename) { image.IsPrimary = true; } else { image.IsPrimary = false; }
    });
    WizardUploadImagesCheck();
}
window.WizardUploadImagesCheck = function WizardUploadImagesCheck() {
    let status = false;
    if (WizardImageGallery.length == 0) { status = false; }
    else {
        let htmlGallery = "<div data-role='lightbox' class='m-2'>";
        WizardImageGallery.forEach(image => {
            if (image.IsPrimary) { status = true; }
            htmlGallery += "<img id='" + image.FileName + "' src='" + image.Attachment + "' :data-original='" + image.Attachment + "' class='c-pointer drop-shadow " + (image.IsPrimary ? " selected " : "") + "' style='max-width:200px;margin:10px;' title='" + window.dictionary('labels.selectDefault') +"' onclick='WizardGallerySetPrimaryImage(\"" + image.FileName + "\")' />";
        }); htmlGallery += "</div>";
        $("#ImageGallery").html(htmlGallery);
    }
    window.ValidationWizardStatus = ValidationWizardStatus = status;
}
window.WizardUploadImages = async function WizardUploadImages(files) {
    if (files.length > 0) {
        window.showPageLoading();

        let setprimary = true;
        for (let i = 0; i < files.length; i++) {
            const reader = new FileReader();
            let fileContent = await new Promise((resolve, reject) => {
                const reader = new FileReader();
                reader.onloadend = () => resolve(reader.result);
                reader.onerror = reject;
                reader.readAsDataURL(files[i]);
            });

            if (fileContent.length < 250 * 1024) {
                WizardImageGallery.push({ hoteId: "", IsPrimary: setprimary ? true : false, File: files[i], FileName: files[i].name, Attachment: fileContent, UserId: store.state.user.UserId, timestamp: new Date() });
                setprimary = false;
            } else {
                var notify = Metro.notify; notify.setup({ width: 300, duration: 3000, animation: 'easeOutBounce' });
                notify.create(window.dictionary('labels.maxFileSize') + files[i].name, "Info"); notify.reset();
            }
        }

        window.WizardImageGallery = WizardImageGallery;
        WizardUploadImagesCheck();
        window.hidePageLoading();
    } else {
        var notify = Metro.notify; notify.setup({ width: 300, duration: 3000, animation: 'easeOutBounce' });
        notify.create(window.dictionary('labels.notInsertedAnyImage'), "Info"); notify.reset();
    }

}






//WIZARD FUNCTIONS

//function UserLoadConfig() {
//    showPageLoading();
//        $.ajax({ url: '/GLOBALNETUserWebConfigList/WebUser', dataType: 'json',
//        type: "GET",
//        headers: {
//            'Content-type': 'application/json',
//            'Authorization': 'Bearer ' + Metro.storage.getItem('ApiToken', null)
//        },
//        success: function (data) {
//            Metro.storage.setItem('UserConfig', JSON.parse(JSON.stringify(data)));
//            hidePageLoading();
//            IsLogged();
//            window.location.href = "/Dashboard";
//        },
//        error: function (error) {
//            var notify = Metro.notify; notify.setup({ width: 300, duration: 1000, animation: 'easeOutBounce' });
//            notify.create("Downloading User Data Failed", "Alert", { cls: "alert" }); notify.reset();
//            hidePageLoading();
//        }
//    });
//}


window.AllowWizardNextPage =function str2bytes(str) {
    var bytes = new Uint8Array(str.length);
    for (var i = 0; i < str.length; i++) {
        bytes[i] = str.charCodeAt(i);
    }
    return bytes;
}