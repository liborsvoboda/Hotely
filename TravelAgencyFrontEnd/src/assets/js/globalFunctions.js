
function disableScroll() {
    scrollTop = window.pageYOffset || document.documentElement.scrollTop;
    scrollLeft = window.pageXOffset || document.documentElement.scrollLeft,

    window.onscroll = function () {
        window.scrollTo(scrollLeft, scrollTop);
    };
}

function enableScroll() {
    window.onscroll = function () { };
}

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


window.str2bytes =function str2bytes(str) {
    var bytes = new Uint8Array(str.length);
    for (var i = 0; i < str.length; i++) {
        bytes[i] = str.charCodeAt(i);
    }
    return bytes;
}