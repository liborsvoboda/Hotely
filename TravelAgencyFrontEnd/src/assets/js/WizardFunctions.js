//import store from "../../store/index";



//WIZARD FUNCTIONS
let ValidationWizardStatus = false;
let ActualValidationFormName = "hotelForm";
let ActualWizardPage = 1;
let WizardRoomPanelCustomButton = [{ html:'<span class=\'mif-cancel\'></span>', cls:'sys-button', onclick:'alert(\'You press rocket button\')' }];

let WizardHotel = {};
let WizardImageGallery = [];
let WizardRooms = [];
let WizardTempRoomPhoto = [];

//Watches
function WizardRequestCityList(value) {
    watchGlobalVariables.wizardRequestCityList = value;
};

//Validation
function ValidateForm() { console.log("validate form");
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


function InvalidForm() { console.log("invalid form");
    //When bo back and again to done is validate last Form from other Page
    if (ActualWizardPage > $("#AdvertisementWizard")[0]["DATASET:UID:M4Q"].wizard.current)
    { $("#AdvertisementWizard")[0]["DATASET:UID:M4Q"].wizard.toPage($("#AdvertisementWizard")[0]["DATASET:UID:M4Q"].wizard.current + 1); }

    ValidationWizardStatus = false;
};


//Gallery Control
function WizardGallerySetPrimaryImage(filename) {
    WizardImageGallery.forEach(image => {
        if (image.FileName == filename) { image.IsPrimary = true; } else { image.IsPrimary = false; }
    });
    WizardUploadImagesCheck();
}
function WizardUploadImagesCheck() {
    let status = false;
    if (WizardImageGallery.length == 0) { status = false; }
    else {

        let htmlGallery = "<div data-role='lightbox' data-role-lightbox='false' class='m-2' data-cls-image='border border-1 bd-white' >"; 
        WizardImageGallery.forEach(image => {
            if (image.IsPrimary) { status = true; }
            htmlGallery += "<img id='" + image.FileName + "' src='" + image.Attachment + "' :data-original='" + image.Attachment + "' class='c-pointer drop-shadow " + (image.IsPrimary ? " selected " : "") + "' style='max-width:150px;margin:10px;' title='" + window.dictionary('labels.selectDefault') + "'/>";
        }); htmlGallery += "</div>";
        $("#HotelImageGallery").html(htmlGallery);

        //remove Gallery Open Function
        WizardImageGallery.forEach(image => {
            var old_element = document.getElementById(image.FileName);
            var new_element = old_element.cloneNode(true);
            new_element.onclick = function () { WizardGallerySetPrimaryImage(image.FileName); }
            old_element.parentNode.replaceChild(new_element, old_element);
        })

    }
    ValidationWizardStatus = status;
}
async function WizardUploadImages(files) {
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

                //repeat insert validation
                let checkGallery = WizardImageGallery; let itsOk = true;
                checkGallery.forEach(image => {
                    setprimary = false;
                    if (image.FileName == files[i].name) { itsOk = false; }
                })

                if (itsOk) {
                    WizardImageGallery.push({ hoteId: 0, IsPrimary: setprimary ? true : false, FileName: files[i].name, Attachment: fileContent, UserId: 0, timestamp: new Date() });
                } else {
                    var notify = Metro.notify; notify.setup({ width: 300, duration: 3000, animation: 'easeOutBounce' });
                    notify.create(window.dictionary('labels.fileExist') + " "+ files[i].name, "Info"); notify.reset();
                }
                
            } else {
                var notify = Metro.notify; notify.setup({ width: 300, duration: 3000, animation: 'easeOutBounce' });
                notify.create(window.dictionary('labels.maxFileSize') + " " + files[i].name, "Info"); notify.reset();
            }
        }

        WizardImageGallery;
        WizardUploadImagesCheck();
        window.hidePageLoading();
    } else {
        var notify = Metro.notify; notify.setup({ width: 300, duration: 3000, animation: 'easeOutBounce' });
        notify.create(window.dictionary('labels.notInsertedAnyImage'), "Info"); notify.reset();
    }

}

//Rooms Control
async function WizardRoomUploadImage(files) {
    console.log("wizRoomUplioad", files);

    if (files.length > 0) {
        window.showPageLoading();

        const reader = new FileReader();
        let fileContent = await new Promise((resolve, reject) => {
            const reader = new FileReader();
            reader.onloadend = () => resolve(reader.result);
            reader.onerror = reject;
            reader.readAsDataURL(files[0]);
        });

        if (fileContent.length < 250 * 1024) {
            WizardTempRoomPhoto = [];
            WizardTempRoomPhoto.push({ FileName: files[i].name, Attachment: fileContent});
        } else {
            var notify = Metro.notify; notify.setup({ width: 300, duration: 3000, animation: 'easeOutBounce' });
            notify.create(window.dictionary('labels.maxFileSize') + files[i].name, "Info"); notify.reset();
        }

        window.WizardTempRoomPhoto = WizardTempRoomPhoto;
        window.hidePageLoading();
    } else {
        var notify = Metro.notify; notify.setup({ width: 300, duration: 3000, animation: 'easeOutBounce' });
        notify.create(window.dictionary('labels.notInsertedAnyImage'), "Info"); notify.reset();
    }
}
function WizardShowRoomPreview() {
    console.log("room Preview");

    let htmlContent = "<ul class='feed-list'><li class='title'>" + window.dictionary('labels.adPreview') + "</li>";
    htmlContent += "<li><img class='avatar dropshadow' src='" + WizardTempRoomPhoto[0].Attachment + "' ><span class='mif-hotel pos-absolute mif-3x fg-blue' style='top:5px;right:5px;' data-role='hint' data-cls-hint='bg-cyan fg-white drop-shadow' data-hint-text='" + window.dictionary('labels.extraBed') +"></span>";
    htmlContent += "<span class='label'>" + $("#RoomName").val() + "</span>";
    htmlContent += "<span class='second-label fg-black bold'>" + $("#RoomsCount").val() + "x " + window.dictionary('labels.maxCapacity') + ": " + $("#RoomMaxCapacity").val() + ", " + $("#RoomPrice").val() + $("#HotelCurrency")[0].selectedOptions[0].text + " <i class='fas fa-user-alt'></i></span>";
    htmlContent += "<span class='second-label' style='zoom: 0.6;margin-left: 210px;top: 5px;'>" + $("#RoomSummernote").summernote('code') + "</span></li>";
    htmlContent += "</ul>";

    Metro.infobox.create(htmlContent, "", {
        closeButton: true,
        type: "info",
        removeOnClose: true,
        height: "auto"
    });

}
function WizardInsertNewRoom() {
}


