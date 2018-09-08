/*
 * notifIt! by @naoxink
 */
function defaultConfirm(eventObject) {
    console.log("confirm");
    return;
}


function defaultCancel(eventObject) {
    console.log("cancel");
    return;
}

function notif(config) {
    var to = null;
    var defaults = {
        id: 1,
        type: "info",
        width: 400,
        height: 60,
        position: "right",
        autohide: 1,
        msg: "This is my default message",
        confirm: false,
        cancelText: "Cancelar",
        confirmText: "Confirmar",
        cancelFunction: defaultCancel,
        cancelFunctionParameters: {},
        confirmFunction: defaultConfirm,
        confirmFunctionParameters: {},
        opacity: 1,
        multiline: 0,
        fade: 0,
        bgcolor: "",
        color: "",
        timeout: 5000
    };
    $.extend(defaults, config);
    
    var confirmed = false;

    position = defaults.position;

    if (defaults.width > 0) {
        defaults.width = defaults.width;
    } else if (defaults.width === "all") {
        defaults.width = screen.width - 60;
    }

    if (defaults.height < 100 && defaults.height > 0) {
        height = defaults.height;
    }

    var id = 'ui_notifIt_' + defaults.id;
    var div;
    if (defaults.confirm) {
        var confirmDiv = "<div class='ui_notifIt_confirmDiv'><a class='ui_notifIt_cancelAnchor btNotifitn' href='#' class='btNotifit'>" + defaults.cancelText + "</a> &nbsp; <a href='#' class='ui_notifIt_confirmAnchor btNotifitn'>"+ defaults.confirmText + "</a></div>"
        div = "<div id='" + id + "'  ><p>" + defaults.msg + "</p>" + "<br/>" + confirmDiv + "</div>";

    }
    else {
        div = "<div id='" + id + "'  ><p>" + defaults.msg + "</p></div>";
    }


    $("#"+id).remove();
    clearInterval(to);
    $("body").append(div);
    if (defaults.confirm) {

        $("#" + id + "  .ui_notifIt_cancelAnchor.btNotifitn").click(defaults.cancelFunctionParameters, defaults.cancelFunction);
        $("#" + id + "  .ui_notifIt_confirmAnchor.btNotifitn").click(defaults.confirmFunctionParameters, defaults.confirmFunction);
    }

    if (defaults.multiline) {
        $("#" + id).css("padding", 15);
    } else {
        $("#" + id).css("height", height);
        $("#" + id).css("line-height", height + "px");
    }

    $("#" + id).css("width", defaults.width);

    $("#" + id).css("opacity", defaults.opacity);

    switch (defaults.type) {
        case "error":
            $("#" + id).addClass("notifIt_error");
            break;
        case "success":
            $("#" + id).addClass("notifIt_success");
            break;
        case "info":
            $("#" + id).addClass("notifIt_info");
            break;
        case "warning":
            $("#" + id).addClass("notifIt_warning");
            break;
        default:
            $("#" + id).addClass("notifIt_default");
            break;
    }

    $("#" + id).css("background-color", defaults.bgcolor);
    
    $("#" + id).css("color", defaults.color);
    
   
    switch (defaults.position) {
        case "left":
            $("#" + id).css("left", parseInt(0 - (defaults.width + 10)));
            $("#" + id).css("left", parseInt(0 - (defaults.width * 2)));
            $("#" + id).animate({ left: 10 });
            break;
        case "right":
            $("#" + id).css("right", parseInt(0 - (defaults.width + 10)));
            $("#" + id).css("right", parseInt(0 - (defaults.width * 2)));
            $("#" + id).animate({ right: 10 });
            break;
        case "center":
            var mid = (window.innerWidth || document.documentElement.clientWidth || document.body.clientWidth) / 2;
            $("#" + id).css("top", parseInt(0 - (defaults.height + 10)));
            $("#" + id).css("left", mid - parseInt(defaults.width / 2));
            $("#" + id).animate({ top: 10 });
            break;
        default:
            var mid = (window.innerWidth || document.documentElement.clientWidth || document.body.clientWidth) / 2;
            $("#" + id).css("right", parseInt(0 - (defaults.width + 10)));
            $("#" + id).css("left", mid - parseInt(defaults.width / 2));
            $("#" + id).animate({ right: 10 });
            break;
    }
    
        $("#" + id).click(function () {
            notifit_dismiss(this, to, defaults);
        });

        if (defaults.autohide) {
            if (!isNaN(defaults.timeout)) { // Take the timeout if is a number
                to = setTimeout(function() {
                    $("#" + id).click();
                }, defaults.timeout);
            }
        }
}

function notifit_dismiss(notif, to, config) {
    clearInterval(to);
    if (!config.fade) {
        switch(config.position){
            case "center":
                $(notif).animate({
                    top: parseInt(config.height - (config.height / 2))
                }, 100, function() {
                    $(notif).animate({
                        top: parseInt(0 - (config.height * 2))
                    }, 100, function() {
                        $(notif).remove();
                    });
                });
            break;
            case "right":
                $(notif).animate({
                    right: parseFloat(config.width - (config.width * 0.9))
                }, 100, function() {
                    $(notif).animate({
                        right: parseInt(0 - (config.width * 2))
                    }, 100, function() {
                        $(notif).remove();
                    });
                });
            break;
            case "left":
                $(notif).animate({
                    left: parseFloat(config.width - (config.width * 0.9))
                }, 100, function() {
                    $(notif).animate({
                        left: parseInt(0 - (config.width * 2))
                    }, 100, function() {
                        $(notif).remove();
                    });
                });
            break;
        }
    } else {
        $(notif).fadeOut("slow", function () {
            $(notif).remove();
        });
    }
}
