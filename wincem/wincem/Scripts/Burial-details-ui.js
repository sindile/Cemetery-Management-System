$(document).on("pageinit", "#burialDetialsPage", function () {
    $('#burialDetialsPage :input').attr('disabled', true);
    $('#Burial_Position').selectmenu("disable");
    $('#Burial_Record_Type').selectmenu("disable");
    $('#Gender').slider("disable");
    $('#Cremation').slider("disable");
    $('#Garden').slider("disable");
});

$(document).on("click", ".startProcessing", function () {
    $.mobile.showPageLoadingMsg();
});