
//Modal Window CREATE Function
function CreateLinkWindow(title, url) {
    var blankButton = [
        {
            html: "<span class='mif-open-book' title='Otevřít v Novém Okně'></span>",
            cls: "sys-button",
            onclick: "window.open('" + url + "','_blank')"
        }, {
            html: "<span class='mif-backward' title='Zpět do Výchozí Složky '></span>",
            cls: "warning",
            onclick: "$(\"#toolWindow\").attr(\"src\",\"" + url + "\")"
        }
    ];
        Metro.window.create({
        resizeable: true,
        draggable: true,
        width: '90%',
        height: 800,
        clsWindow: "pos-absolute",
        icon: "<span class='mif-eye'></span>",
        customButtons: blankButton,
        title: title,
        content: "<iframe id='toolWindow' src='" + url + "' allowfullscreen frameborder='0' width='100%' height='100%' style='overflow: hidden; height: 800px; width: 100 %'></iframe>",
        //overlayColor: "transparent",
        btnClose: true,
        shadow: true,
        modal: false,
        place: "center",
        onShow: function () { window.scrollTo(0, 0); }
    });
}