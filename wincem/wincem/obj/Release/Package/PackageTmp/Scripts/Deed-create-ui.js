$(document).on("pageinit", "#ownerCreatePage", function () {
    $("[type='date']").on("keyup", function (event) {
        if (event.which == 32) {
            var d = new Date();
            $(this).val(d.getFullYear() + "-" + pad(d.getMonth() + 1, 2, '0') + "-" + pad(d.getDate(), 2, '0'));
        }
    });
    if(getParameterByName('spaceList').length > 0 )
        $('#Deed_Type').slider('disable');
});

$(document).on("click", ".startProcessing", function () {
    $.mobile.showPageLoadingMsg();
});

$("[type='text']").keyup(function () {
    this.value = this.value.toUpperCase();
});
$("[type='text']").change(function () {
    this.value = this.value.toUpperCase();
});