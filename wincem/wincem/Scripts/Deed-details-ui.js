$(document).on("pageinit", "#ownerDetailsPage", function () {
    $('#ownerDetailsPage :input').attr('disabled', true);
    $('#Deed_Type').slider('disable');
});

$(document).on("click", ".startProcessing", function () {
    $.mobile.showPageLoadingMsg();
});