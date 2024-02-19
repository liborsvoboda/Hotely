////Modal Forms Library For Using Forms EveryWhere without Page Change

//Generic Modal Login InfoBox Form
function FloatingLoginForm(parentId) {
    Metro.storage.setItem('DiscussionParentId', parentId); 

    let generatedElement = null; let htmlContent = "";
    htmlContent += "<p class='floatingLoginSign' align='center'>" + window.dictionary('user.logIn') + "</p>";
    htmlContent += "<input id='FloatingLoginEmail' class='form-control floatingLoginEmail' type='email' align='center' placeholder='Email' required />";
    htmlContent += "<input id='FloatingLoginPassword' class='form-control floatingLoginPassword' type='password' align='center' placeholder='Password' required minLength='6' />"
    htmlContent += "<button class='button shadowed' onclick=CheckFloatingLoginValid() >" + window.dictionary('user.signIn') + "</button>";

    generatedElement = Metro.infobox.create(htmlContent, "", {
        closeButton: true,
        clsBox: "rounded drop-shadow",
        type: "", removeOnClose: true,
        width: "400", height: "360"
    }); generatedElement.id("FloatingLoginForm");
}

//Generic Discussion Contribution Modal InfoBox Form
function NewDiscussionInfoBox(parentId) {
    AccordionCustomMenuClick('discussionFrame_' + parentId);

    let htmlContent = "";
    htmlContent += "<div class='d-flex row'><div class='h3 col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12' >" + window.dictionary('labels.discussionForum') + "</div>";
    htmlContent += "<div class='h3 col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12'>" + window.dictionary('labels.newContribution') + "</div></div>";
    htmlContent += "<form id='newDiscussionForm' data-role='validator' action='javascript:' data-on-submit='SendDiscussionContribution();InfoBoxOpenClose(\"NewDiscussionInfoBox\");' data-interactive-check='true' >";
    htmlContent += "<div class='d-flex row'><div class='h3 col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12' ><select id='newDiscussionSelectionList' data-role='select' data-clear-button='false' data-validate='required not = -1'></select></div>";
    htmlContent += "<div class='h3 col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12'><div class='form-group'><input data-role='input' id='newDiscussionTitle' data-validate='required' placeholder='" + window.dictionary('labels.writeTitle') + "'></div></div></div>";
    htmlContent += "<div class='d-flex row'><div id='newDiscussionSummernote'>" + window.dictionary('labels.hereWriteNewContribution') + "</div></div>";
    htmlContent += "<div class='d-flex row text-right mt-3'><button class='button success outline shadowed' style='width:200px;' type='submit' >" + window.dictionary('labels.sendContribution') + "</button></div>";
    htmlContent += "</form>";

    let selOptions = [];
    Metro.storage.getItem('DiscussionTemaList', null).forEach(discussion => { selOptions.push({ val: discussion.id, title: discussion.subject, selected: discussion.id == parentId ? true : false }); });

    let generatedElement = Metro.infobox.create(htmlContent, "", {
        closeButton: true,
        clsBox: "rounded drop-shadow",
        type: "", removeOnClose: true,
        width: "800", height: "550"
    }); generatedElement.id("NewDiscussionInfoBox");

    setTimeout(() => {
        let getSelect = Metro.getPlugin("#newDiscussionSelectionList", "select"); getSelect.data(""); getSelect.addOptions(selOptions);
        ElementSummernoteInit('newDiscussionSummernote');
    }, 100);
}






////Support Function List For Modal Forms

//Specific Modal Login Form Validation
async function CheckFloatingLoginValid() {
    if (!$("#FloatingLoginEmail").val().match(/^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/) || $("#FloatingLoginPassword").val().length < 6) {
        ShowNotify('error', (window.dictionary("messages.passwordNotHaveMinimalLengthOrMailNotValid") + " 6"));
        $("#FloatingLoginForm").addClass("ani-ring");
        setTimeout(function () { $("#FloatingLoginForm").removeClass("ani-ring"); }, 1000);
    } else { await FloatingLogin(); }
}


