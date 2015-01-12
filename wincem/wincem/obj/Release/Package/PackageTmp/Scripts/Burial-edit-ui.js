﻿$("[type='date']").on("keyup", function (event) {
    if (event.which == 32) {
        var d = new Date();
        $(this).val(d.getFullYear() + "-" + pad(d.getMonth() + 1, 2, '0') + "-" + pad(d.getDate(), 2, '0'));
    }
});
//
$("#Burial_Position").on("change", function () {
    $("#burialImage").attr("src", "../../Images/" + $(this).val() + ".png");
    $("#burialImage").attr("alt", $(this).val());
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

