﻿
$(document).on("keyup", "#searchString_burial", function () {
    burialSearch();
});

$(document).on("change", "#number_of_records_burial", function () {
    burialSearch();
});

$(document).on("click", ".startProcessing", function () {
    $.mobile.showPageLoadingMsg();
});

$(document).on("contextmenu", ".burialRecord", function (e) {
    $('#deleteLink').attr('href', config.baseURL + 'Burial/Delete/' + $(this).attr("id"));
    $('#editLink').attr('href', config.baseURL + 'Burial/Edit/' + $(this).attr("id"));
    $('#burialPopup').popup('open', { positionTo: "#" + $(this).attr("id") });
    $('#burialPopup').popup("reposition", { positionTo: "#" + $(this).attr("id") });
    return false;
});

function burialSearch() {
    if ($('#searchString_burial').val().length > 2 || $('#searchString_burial').val() == '') {
        $.ajax({
            url: config.baseURL + 'Burial/SearchBurials?SearchString=' + $('#searchString_burial').val() + '&number_of_records=' + $('#number_of_records_deed').val() + "&First_Name=" + $('#First_Name').is(':checked') + "&Last_Name=" + $('#Last_Name').is(':checked')
                    + "&Permit_Number=" + $('#Permit_Number').is(':checked') + "&Burial_Date=" + $('#Burial_Date').is(':checked') + "&Work_Order_Date=" + $('#Work_Order_Date').is(':checked'),
            beforeSend: function (xhr) {
            },
            success: function (data) {
                $('#burialListDiv').html(data);
            },
            complete: function () {
                $('#burialList').listview();
            }
        });
    }
}