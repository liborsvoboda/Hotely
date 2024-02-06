
//WIZARD FUNCTIONS
let ActualValidationFormName = "hotelForm";
let ActualWizardPage = 1;
let propertyList = [];
let Router = null;

let HotelRecId = null;
let WizardHotel = {
    Images: [],
    Rooms: [],
    Properties:[]
};
let WizardImageGallery = [];
let WizardRooms = [];
let WizardTempRoomPhoto = [];
let WizardProperties = [];
let WizardSelectedProperty = {};


//Watches
function WizardRequestCityList(value) {
    watchGlobalVariables.wizardRequestCityList = value;
};

async function WizardOpenHelp() {
    let step = Metro.getPlugin($("#AdvertisementWizard"), 'wizard').current;

    if (step == 1) { OpenDocView('WizardBasicSetting'); }
    else if (step == 2) { OpenDocView('WizardImageGallery'); }
    else if (step == 3) { OpenDocView('WizardAdvertisement'); }
    else if (step == 4) { OpenDocView('WizardPropertyList'); }
    else if (step == 5) { OpenDocView('WizardPropertiesSet'); }
    else if (step == 6) { OpenDocView('WizardFinalView'); }
}

//Validation
async function WizardValidateForm() {
    let wizard = Metro.getPlugin($("#AdvertisementWizard"), 'wizard');

    //prepare properties array
    if (wizard.current == 4) {
        propertyList.forEach(property => {
            if ($("#prop_" + property.id) != undefined && $("#prop_" + property.id).val('checked')[0].checked
                && (
                WizardProperties.length == 0 || (WizardProperties.length > 0 && WizardProperties.filter(obj => { return obj.id == property.id; })[0] == undefined)
                )
            ) {
                WizardProperties.push({
                    id: property.id, name: property.systemName, unit: property.propertyOrServiceUnitType.systemName, isAvailable: true, isBit: property.isBit,
                    isFeeInfoRequired: property.isFeeInfoRequired, isFeeRangeAllowed: property.isFeeRangeAllowed, isValueRangeAllowed: property.isValueRangeAllowed,
                    value: null, valueRangeMin: null, valueRangeMax: null,
                    fee: false, feeValue: null, feeRangeMin: null, feeRangeMax: null,
                });
            }
            if ($("#prop_" + property.id) != undefined && !$("#prop_" + property.id).val('checked')[0].checked && WizardProperties.filter(obj => { return obj.id == property.id; })[0] != undefined) {
                WizardProperties.splice(WizardProperties.indexOf(WizardProperties.filter(obj => { return obj.id === property.id; })[0]), 1);
            }
        });
    }


    if (wizard.current == 1 &&
        $("#HotelName").val().length > 0 && $("#LimitGuestCommDays").val().length > 0 && $("#StornoDaysCountBeforeStart").val().length > 0 && 
        $("#HotelCurrency")[0].selectedOptions[0] != undefined && $("#HotelCity")[0].selectedOptions[0] != undefined) {
        window.ActualValidationFormName = ActualValidationFormName = "galleryForm";
        window.ActualWizardPage = ActualWizardPage = 2;

        let htmlContent = "<div class='d-flex' style='width: 250px; height: 150px; top: 0px; left: 0px;'><input id='Images' type = 'file' data-role='file' data-mode='drop' data-on-select='WizardUploadImages' accept='.png,.jpg,.jpeg,.tiff' multiple='multiple' ></div><div id='HotelImageGallery' class='d-flex' />";
        $("#galleryContainer").html(htmlContent);
        WizardUploadImagesCheck();

        wizard.toPage(wizard.current + 1);
    }
    else if (wizard.current == 1 &&
        ($("#HotelName").val().length == 0 || $("#LimitGuestCommDays").val().length == 0 || $("#StornoDaysCountBeforeStart").val().length == 0 ||
            $("#HotelCurrency")[0].selectedOptions[0] == undefined || $("#HotelCity")[0].selectedOptions[0] == undefined)) {
        ShowNotify('info', window.dictionary('labels.missingSetting'));
    }

    else if (wizard.current == 2 && WizardImageGallery.length > 0) {
        window.ActualValidationFormName = ActualValidationFormName = "roomForm";
        window.ActualWizardPage = ActualWizardPage = 3;
        wizard.toPage(wizard.current + 1);
    }
    else if (wizard.current == 2 && WizardImageGallery.length == 0) {
        ShowNotify('info', window.dictionary('labels.missingAnyImage'));
    }

    else if (wizard.current == 3 && WizardRooms.length > 0) {
        window.ActualValidationFormName = ActualValidationFormName = "propertyForm";
        window.ActualWizardPage = ActualWizardPage = 4;
        WizardSetUpdateData2();
        wizard.toPage(wizard.current + 1);
    }
    else if (wizard.current == 3 && WizardRooms.length == 0) {
        ShowNotify('info', window.dictionary('labels.missingAnyRoom'));
    }

    else if (wizard.current == 4 && WizardProperties.length > 0) {
        window.ActualValidationFormName = ActualValidationFormName = "propertySettingsForm";
        window.ActualWizardPage = ActualWizardPage = 5;
        WizardGeneratePropertySettingsPanel();
        window.watchChangeVariables.propertySelected = false;

        wizard.toPage(wizard.current + 1);
    }
    else if (wizard.current == 4 && WizardProperties.length == 0) {
        ShowNotify('info', window.dictionary('labels.missingAnyProperties'));
    }

    else if (wizard.current == 5 && WizardProperties.length > 0) {
        window.ActualValidationFormName = ActualValidationFormName = "previewForm";
        window.ActualWizardPage = ActualWizardPage = 6;
        WizardGeneratePreview();
        wizard.toPage(wizard.current + 1);
    }
};

//Gallery Control
function WizardGallerySetPrimaryImage(filename) {
    WizardImageGallery.forEach(image => {
        if (image.FileName == filename) { image.IsPrimary = true; } else { image.IsPrimary = false; }
    });
    WizardUploadImagesCheck();
}

function WizardGalleryRemoveImage(index) {
    WizardImageGallery.splice(index, 1);
    WizardUploadImagesCheck();
}

function WizardUploadImagesCheck() {
    let status = false;
    if (WizardImageGallery.length == 0) {
        let htmlGallery = "<div data-role='lightbox' data-role-lightbox='false' class='m-2' data-cls-image='border border-1 bd-white' ></div>"; 
        $("#HotelImageGallery").html(htmlGallery);
        status = false;
    }
    else {

        let htmlGallery = "<div data-role='lightbox' data-role-lightbox='false' class='m-2' data-cls-image='border border-1 bd-white' >"; 
        WizardImageGallery.forEach((image,index) => {
            if (image.IsPrimary) { status = true; }
            htmlGallery += "<img id='" + image.FileName + "' src='" + image.Attachment + "' :data-original='" + image.Attachment + "' class='c-pointer drop-shadow " + (image.IsPrimary ? " selected " : "") + "' style='max-width:150px;margin:10px;' title='" + window.dictionary('labels.selectDefault') + "'/>";
            htmlGallery += "<span class='mif-cancel c-pointer pos-relative fg-red mif-3x' style='top:-35px;right:25px;' onclick=WizardGalleryRemoveImage(" + index + "); title='" + window.dictionary('labels.delete') + "'></span>";
        }); htmlGallery += "</div>";
        $("#HotelImageGallery").html(htmlGallery);

        //remove Gallery Open Function
        WizardImageGallery.forEach(image => {
            var old_element = document.getElementById(image.FileName);
            var new_element = old_element.cloneNode(true);
            new_element.onclick = function () { WizardGallerySetPrimaryImage(image.FileName); }
            old_element.parentNode.replaceChild(new_element, old_element);
        });

    }
    ValidationWizardStatus = status;
}
async function WizardUploadImages(files) {
    //console.log("uploading images", files);


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
                    WizardImageGallery.push({IsPrimary: setprimary ? true : false, FileName: files[i].name, Attachment: fileContent});
                } else {
                    ShowNotify('info', window.dictionary('labels.fileExist') + " " + files[i].name);
                }
                
            } else {
                ShowNotify('info', window.dictionary('labels.maxFileSize') + " " + files[i].name);
            }
        }

        WizardUploadImagesCheck();
        window.hidePageLoading();
    } else {
        ShowNotify('info', window.dictionary('labels.notInsertedAnyImage'));
    }

}

//Rooms Control
async function WizardRoomUploadImage(files) {
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
            WizardTempRoomPhoto.push({ FileName: files[0].name, Attachment: fileContent});
        } else {
            WizardTempRoomPhoto = [];
            ShowNotify('info', window.dictionary('labels.maxFileSize') + files[0].name);
            Metro.getPlugin($("#roomImage"), 'file').clear();
        }

        window.WizardTempRoomPhoto = WizardTempRoomPhoto;
        window.hidePageLoading();
    } else {
        WizardTempRoomPhoto = [];
        ShowNotify('info', window.dictionary('labels.notInsertedAnyImage'));
        Metro.getPlugin($("#roomImage"), 'file').clear();
    }

    window.watchChangeVariables.roomShowPreviewEnabled = window.WizardTempRoomPhoto != undefined && window.WizardTempRoomPhoto.length > 0;
};
function WizardRemoveRoom(index) {
    WizardRooms.splice(index, 1);
    WizardGenerateRooms();
}
function WizardGenerateRooms() {
    let htmlContent = "";

    let currency = $("#HotelCurrency option:selected").text();
    WizardRooms.forEach((room, index) => {

        htmlContent += " <div id='" + room.RoomName + "' data-role='panel' class='p-1' data-title-caption='" + room.RoomName + "' data-cls-panel='shadow-3' data-cls-content='fg-white' data-collapsible='true' data-collapsed='true' >"
        htmlContent += "<ul class='feed-list fg-black text-left'>";
        htmlContent += "<li><img class='avatar dropshadow' src='" + room.Attachment + "'>";
        htmlContent += "<span class='c-pointer mif-cancel mif-3x pos-absolute fg-red' style='top:5px;left:0px;' title='" + window.dictionary('labels.remove') + "' onclick='WizardRemoveRoom(" + index + ")'></span>";
        htmlContent += "<span class='second-label fg-black bold'>" + room.RoomsCount + "x " + window.dictionary('labels.maxCapacity') + ": " + room.MaxCapacity + ", " + room.Price + " " + currency + " <i class='fas fa-user-alt'></i>" + (room.ExtraBed ? " + <span class='mif-hotel mif-3x ' style = 'top:5px;' data-role='hint' data-cls hint='bg-cyan fg-white drop-shadow' data-hint-text='" + window.dictionary('labels.extraBed') + "' > </span>" : "") + "</span>";
        htmlContent += "<span class='second-label' style='zoom: 1;'>" + room.Description + "</span></li>";
        htmlContent += "</ul>";
        htmlContent += "</div>";
    });

    $("#roomContainer").html(htmlContent);

}

function WizardPropertySelect(id) {
    WizardProperties.forEach((property) => {

        if (property.id == id) {
            WizardSelectedProperty = property;

            //value part
            if (!property.isBit) {
                $("#PropValueRoot").removeClass("disabled");
                if (property.value != null) {
                    $("#PropValueRoot").removeClass("disabled"); $("#PropValue").val(property.value);

                    if (property.isValueRangeAllowed) {
                        $("#PropValueRangeRoot").removeClass("disabled"); $("#PropValueRange").val('checked')[0].checked = false;
                    } else { $("#PropValueRangeRoot").addClass("disabled"); $("#PropValueRange").val('checked')[0].checked = false; }

                    $("#PropMinValueRoot").addClass("disabled"); $("#PropValueRangeMin").val(null);
                    $("#PropMaxValueRoot").addClass("disabled"); $("#PropValueRangeMax").val(null);
                } else if (property.valueRangeMin != null || property.valueRangeMax != null) {
                    $("#PropValueRoot").addClass("disabled"); $("#PropValue").val(null);
                    $("#PropValueRangeRoot").removeClass("disabled"); $("#PropValueRange").val('checked')[0].checked = true;

                    $("#PropMinValueRoot").removeClass("disabled"); $("#PropValueRangeMin").val(property.valueRangeMin);
                    $("#PropMaxValueRoot").removeClass("disabled"); $("#PropValueRangeMax").val(property.valueRangeMax);
                }
            } else {
                $("#PropValueRoot").addClass("disabled"); $("#PropValue").val(null);
                $("#PropValueRangeRoot").addClass("disabled"); $("#PropValueRange").val('checked')[0].checked = false;
                $("#PropMinValueRoot").addClass("disabled"); $("#PropValueRangeMin").val(null);
                $("#PropMaxValueRoot").addClass("disabled"); $("#PropValueRangeMax").val(null);
            }

            //fee part
            if (property.fee) {
                $('#PropFee').val('checked')[0].checked = true;
                if (property.feeValue != null && property.feeValue != "") {
                    $("#PropFeeRoot").removeClass("disabled"); $("#PropFeeValueRoot").removeClass("disabled");  $("#PropFeeValue").val(property.feeValue);
                    $('#PropFeeRange').val('checked')[0].checked = false;

                    if (property.isFeeRangeAllowed) { $("#PropFeeRangeRoot").removeClass("disabled"); } else { $("#PropFeeRangeRoot").addClass("disabled"); }
                    $("#PropFeeRangeMinRoot").addClass("disabled"); $("#PropFeeRangeMin").val(null);
                    $("#PropFeeRangeMaxRoot").addClass("disabled"); $("#PropFeeRangeMax").val(null);
                } else {
                    $('#PropFeeRange').val('checked')[0].checked = true;

                    $("#PropFeeValueRoot").addClass("disabled"); $("#PropFeeValue").val(null);
                    $("#PropFeeRangeMinRoot").removeClass("disabled"); $("#PropFeeRangeMin").val(property.feeRangeMin);
                    $("#PropFeeRangeMaxRoot").removeClass("disabled"); $("#PropFeeRangeMax").val(property.feeRangeMax);
                }
            }
            else {
                $('#PropFee').val('checked')[0].checked = false; $('#PropFeeRange').val('checked')[0].checked = false;
                $("#PropFeeRoot").removeClass("disabled"); $("#PropFeeRangeRoot").addClass("disabled"); $("#PropFeeValue").val(null);
                $("#PropFeeRangeMinRoot").addClass("disabled"); $("#PropFeeRangeMaxRoot").addClass("disabled"); $("#PropFeeRangeMin").val(null); $("#PropFeeRangeMax").val(null);
            }
        }
    });
    window.watchChangeVariables.propertySelected = true;
}
function WizardGeneratePropertySettingsPanel() {
    let htmlContent = "";
    let lineSeparator = true;
    WizardProperties.forEach((property) => {
        htmlContent += lineSeparator ? "<div class='d-flex w-100'>" : "";
        if (property.isBit) { htmlContent += "<input name='propRadio' type='radio' data-role='radio' onclick='WizardPropertySelect(" + property.id + ")'><div class='d-flex w-50 mt-2'><span>" + property.name + "</span>"; }
        else { htmlContent += "<input name='propRadio' type='radio' data-role='radio' onclick='WizardPropertySelect(" + property.id + ")'><div class='d-flex w-50 mt-2'><span>" + property.name + ": <span class='rounded-pill'>" + property.value + " " + property.unit + "</span>" + "</span>"; }

        htmlContent += ((property.fee) ? (property.feeValue != null && property.feeValue != "") ?
            '<span title=\'' + window.dictionary('labels.fee') + '\' class=\'badge bg-green mt-2 fg-white\' style=\'right: 50px;position: absolute;\'>' + property.feeValue + ' ' + $("#HotelCurrency")[0].selectedOptions[0].text + ' </span>'
            : '<span title=\'' + window.dictionary('labels.fee') + '\' class=\'badge bg-green mt-2 fg-white\' style=\'right: 50px;position: absolute;\'>' + property.feeRangeMin + " - " + property.feeRangeMax + ' ' + $("#HotelCurrency")[0].selectedOptions[0].text + ' </span>'
            : '');
        htmlContent += "</div>";

        htmlContent += !lineSeparator ? "</div>" : "";
        lineSeparator = !lineSeparator;
    });
    $("#propSettingsContainer").html(htmlContent);
}

function WizardGeneratePreview() {
    let htmlContent = "";

    //generate hotel preview
    htmlContent += " <div data-role='panel' class='p-1' data-title-caption='" + $("#HotelName").val() + "' data-cls-panel='shadow-3' data-cls-content='fg-white' data-collapsible='true' >"
    htmlContent += "<ul class='feed-list fg-black text-left'>";
    htmlContent += "<li><img class='avatar dropshadow' src='" + WizardImageGallery.filter(obj => { return obj.IsPrimary === true; })[0].Attachment + "'>";
    htmlContent += "<span class='second-label fg-black bold'>" + $("#HotelCountry")[0].selectedOptions[0].text + ", " + $("#HotelCity")[0].selectedOptions[0].text + ", " + $("#HotelCurrency")[0].selectedOptions[0].text + "</span>";
    htmlContent += "<span class='second-label' style='zoom: 1;'>" + $('#HotelSummernote').summernote('code') + "</span></li>";
    htmlContent += "</ul>";
    htmlContent += "</div>";

    let index = 0;
    WizardRooms.forEach(room => {

        htmlContent += " <div id='" + room.RoomName + "' data-role='panel' class='p-1' data-title-caption='" + room.RoomName + "' data-cls-panel='shadow-3' data-cls-content='fg-white' data-collapsible='true' data-collapsed='true' >"
        htmlContent += "<ul class='feed-list fg-black text-left'>";
        htmlContent += "<li><img class='avatar dropshadow' src='" + room.Attachment + "'><span class='c-pointer mif-cancel mif-2x pos-absolute' style='top:5px;left:5px;' title='" + window.dictionary('labels.remove') + "' onclick='WizardRemoveRoom(" + index + ")'></span>";
        htmlContent += "<span class='second-label fg-black bold'>" + room.RoomsCount + "x " + window.dictionary('labels.maxCapacity') + ": " + room.MaxCapacity + ", " + room.Price + " " + $("#HotelCurrency")[0].selectedOptions[0].text + " <i class='fas fa-user-alt'></i>" + (room.ExtraBed ? " + <span class='mif-hotel mif-3x ' style = 'top:5px;' data-role='hint' data-cls hint='bg-cyan fg-white drop-shadow' data-hint-text='" + window.dictionary('labels.extraBed') + "' > </span>" : "") + "</span>";
        htmlContent += "<span class='second-label' style='zoom: 1;'>" + room.Description + "</span></li>";
        htmlContent += "</ul>";
        htmlContent += "</div>";

        index++;
    });

    $("#previewContainer").html(htmlContent);
}
async function SaveHotel() {
    window.showPageLoading();
    console.log("Saving hotel");

    //Save Hotel
    var response = await fetch(
        Metro.storage.getItem('ApiRootUrl', null) + '/Advertiser/SetHotel', {
            method: 'POST', headers: { 'Authorization': 'Bearer ' + Metro.storage.getItem('Token', null), 'Content-type': 'application/json' },
        body: JSON.stringify({
            HotelRecId: HotelRecId,
            HotelName: $("#HotelName").val(), CurrencyId: $("#HotelCurrency")[0].selectedOptions[0].value,
            CountryId: $("#HotelCountry")[0].selectedOptions[0].value, CityId: $("#HotelCity")[0].selectedOptions[0].value,
            Description: $('#HotelSummernote').summernote('code'), LimitGuestCommDays: $("#LimitGuestCommDays").val(),
            StornoDaysCountBeforeStart: $("#StornoDaysCountBeforeStart").val(), GuestStornoEnabled : $("#GuestStornoEnabled").val('checked')[0].checked
        })
    }

    ); let result = await response.json(); HotelRecId = result.InsertedId;

    //Save HotelImages
    var response = await fetch(
        Metro.storage.getItem('ApiRootUrl', null) + '/Advertiser/SetHotelImages', {
            method: 'POST', headers: { 'Authorization': 'Bearer ' + Metro.storage.getItem('Token', null), 'Content-type': 'application/json' },
            body: JSON.stringify({ HotelRecId: HotelRecId, Images: WizardImageGallery })
        }
    ); result = await response.json();

    //Save HotelRooms
    var response = await fetch(
        Metro.storage.getItem('ApiRootUrl', null) + '/Advertiser/SetHotelRooms', {
            method: 'POST', headers: { 'Authorization': 'Bearer ' + Metro.storage.getItem('Token', null), 'Content-type': 'application/json' },
            body: JSON.stringify({ HotelRecId: HotelRecId, Rooms: WizardRooms })
        }
    ); result = await response.json();

    //Update HotelProperties
    var response = await fetch(
        Metro.storage.getItem('ApiRootUrl', null) + '/Advertiser/SetHotelProperties', {
            method: 'POST', headers: { 'Authorization': 'Bearer ' + Metro.storage.getItem('Token', null), 'Content-type': 'application/json' },
            body: JSON.stringify({ HotelRecId: HotelRecId, Properties: WizardProperties })
        }
    ); result = await response.json();

    ShowNotify('info', window.dictionary('labels.advertisementHasBeenSaved'));
    window.hidePageLoading();
    Router.push('/Profile/Advertisement');
}

function WizardSetUpdateData() {
    HotelRecId = WizardHotel.HotelId;
    $("#HotelName").val(WizardHotel.HotelName);
    $("#LimitGuestCommDays").val(WizardHotel.LimitGuestCommDays);
    $("#StornoDaysCountBeforeStart").val(WizardHotel.StornoDaysCountBeforeStart);
    $("#GuestStornoEnabled").val('checked')[0].checked = WizardHotel.GuestStornoEnabled;

    WizardImageGallery = [];
    WizardHotel.Images.forEach(image => {
        WizardImageGallery.push({
            IsPrimary: image.isPrimary, FileName: image.fileName, Attachment: "data:image/png;base64," + image.attachment });
    }); WizardUploadImagesCheck();
}

function WizardSetUpdateData1() {
    WizardRooms = [];
    WizardHotel.Rooms.forEach(room => {
        WizardRooms.push({
            RoomName: room.name, RoomTypeId: room.roomTypeId, Price: room.price, ExtraBed: room.extraBed,
            MaxCapacity: room.maxCapacity, RoomsCount: room.roomsCount,
            Description: room.descriptionCz, FileName: room.imageName, Attachment: "data:image/png;base64," + room.image
        });
    }); WizardGenerateRooms();
}

function WizardSetUpdateData2() {
    WizardProperties = [];
    WizardHotel.Properties.forEach(property => {
        $('#prop_' + property.propertyOrService.id).val('checked')[0].checked = true;
        WizardProperties.push({
            id: property.propertyOrService.id, name: property.propertyOrService.systemName, unit: property.propertyOrService.propertyOrServiceUnitType.systemName, isAvailable: property.isAvailable, isBit: property.propertyOrService.isBit,
            isFeeInfoRequired: property.propertyOrService.isFeeInfoRequired, isFeeRangeAllowed: property.propertyOrService.isFeeRangeAllowed, isValueRangeAllowed: property.propertyOrService.isValueRangeAllowed,
            value: property.value, valueRangeMin: property.valueRangeMin, valueRangeMax: property.valueRangeMax,
            fee: property.fee, feeValue: property.feeValue, feeRangeMin: property.feeRangeMin, feeRangeMax: property.feeRangeMax
        });
    }); WizardGeneratePropertySettingsPanel();
}
