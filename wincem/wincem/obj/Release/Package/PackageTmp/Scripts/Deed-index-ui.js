
$(document).on("keyup", "#searchString_deed", function () {
    deedSearch();
});

$(document).on("change", "#number_of_records_deed", function () {
    deedSearch();
});

$(document).on("click", ".startProcessing", function () {
    $.mobile.showPageLoadingMsg();
});

$(document).on("contextmenu", ".deedRecord", function (e) {
    $('#deleteLink').attr('href', '/wincem/Owner/Delete/' + $(this).attr("id"));
    $('#editLink').attr('href', '/wincem/Owner/Edit/' + $(this).attr("id"));
    $('#deedPopup').popup('open', { positionTo: "#" + $(this).attr("id") });
    $('#deedPopup').popup("reposition", { positionTo: "#" + $(this).attr("id") });
    return false;
});

function deedSearch() {
    if ($('#searchString_deed').val().length > 2 || $('#searchString_deed').val() == '') {
        $.ajax({
            url: 'http://wincem.cloudapp.net/Wincem/Owner/SearchDeeds?SearchString=' + $('#searchString_deed').val() + '&number_of_records=' + $('#number_of_records_deed').val() + "&deed_number=" + $('#deed_number').is(':checked') + "&Owner_Name=" + $('#Owner_Name').is(':checked')
                    + "&Deed_Name=" + $('#Deed_Name').is(':checked') + "&Date_of_Purchase=" + $('#Date_of_Purchase').is(':checked'),
            beforeSend: function (xhr) {
            },
            success: function (data) {
                $('#deedListDiv').html(data);
            },
            complete: function () {
                $('#deedList').listview();
                //$(".deedRecord").on("contextmenu", function (e) {
                //    $('#deleteLink').attr('href', '/wincem/Owner/Delete/' + $(this).attr("id"));
                //    $('#editLink').attr('href', '/wincem/Owner/Edit/' + $(this).attr("id"));
                //    $('#deedPopup').popup('open', { positionTo: "#" + $(this).attr("id") });
                //    $('#deedPopup').popup("reposition", { positionTo: "#" + $(this).attr("id") });
                //    return false;
                //});
            }
        });
    }
}