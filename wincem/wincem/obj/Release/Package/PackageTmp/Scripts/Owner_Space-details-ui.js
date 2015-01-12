$(document).on("pageinit", "#ownedSpaceDetailsPage", function () {
    spacePicker($('#Space_ID').val(), true);
});

function spacePicker(space_ID, disabled) {
    var pageInit = 1;
    $('#cemeterySelect').change(function () {
        var selectedCemetery = $(this).val();
        if (selectedCemetery != null && selectedCemetery != '') {
            $.getJSON('http://services.arcgis.com/aMgEifFIhCAICARd/ArcGIS/rest/services/Cemetery_Example/FeatureServer/1/query?where=Cemetery_ID%3D' + selectedCemetery +
                        '&text=&objectIds=&time=&geometry=&geometryType=esriGeometryEnvelope&inSR=&spatialRel=esriSpatialRelIntersects&relationParam=&outFields=Division_Number%2C+Division_Name&returnGeometry=false&maxAllowableOffset=&geometryPrecision=&outSR=&returnIdsOnly=false&returnCountOnly=false&orderByFields=Division_Name&groupByFieldsForStatistics=&outStatistics=&returnZ=false&returnM=false&gdbVersion=&returnDistinctValues=false&f=pjson', function (data) {
                            var select = $('#divisionSelect');
                            select.empty();
                            select.append($('<option/>', {
                                value: '00',
                                text: 'Unkown / No Division'
                            }));
                            $.each(data.features, function (index, feature) {
                                if (space_ID.substring(2, 4) == feature.attributes.Division_Number && pageInit == 1) {
                                    select.append($('<option/>', {
                                        value: feature.attributes.Division_Number,
                                        text: feature.attributes.Division_Name,
                                        selected: "selected"
                                    }));
                                }
                                else {
                                    select.append($('<option/>', {
                                        value: feature.attributes.Division_Number,
                                        text: feature.attributes.Division_Name
                                    }));
                                }
                            });
                            $('#divisionSelect').selectmenu('enable');
                            $('#blockSection').selectmenu('disable');
                            $('#lotRow').selectmenu('disable');
                            $('#space').selectmenu('disable');
                            if (pageInit == 1) {
                                $('#divisionSelect').change();
                            }
                            if (disabled) {
                                $('#divisionSelect').selectmenu('disable');
                            }
                            $('#divisionSelect').selectmenu('refresh');
                        });
        }
    });
    $('#divisionSelect').change(function () {
        var selectedDivision = $(this).val();
        var selectedCemetery = $('#cemeterySelect').val();
        var division_ID = selectedCemetery + '-' + selectedDivision
        if (selectedDivision != null && selectedDivision != '') {
            $.getJSON('http://services.arcgis.com/aMgEifFIhCAICARd/ArcGIS/rest/services/Cemetery_Example/FeatureServer/2/query?where=Division_ID%3D\'' + division_ID +
                        '\'&text=&objectIds=&time=&geometry=&geometryType=esriGeometryEnvelope&inSR=&spatialRel=esriSpatialRelIntersects&relationParam=&outFields=Block_Section_ID%2C+Block_Section_Name&returnGeometry=false&maxAllowableOffset=&geometryPrecision=&outSR=&returnIdsOnly=false&returnCountOnly=false&orderByFields=Block_Section_Name&groupByFieldsForStatistics=&outStatistics=&returnZ=false&returnM=false&gdbVersion=&returnDistinctValues=false&f=pjson', function (data) {
                            var select = $('#blockSection');
                            select.empty();
                            select.append($('<option/>', {
                                value: '0',
                                text: 'No Block / Section'
                            }));
                            $.each(data.features, function (index, feature) {
                                if (space_ID.substring(5, 6) == feature.attributes.Block_Section_Name && pageInit == 1) {
                                    select.append($('<option/>', {
                                        value: feature.attributes.Block_Section_Name,
                                        text: feature.attributes.Block_Section_Name,
                                        selected: "selected"
                                    }));
                                }
                                else {
                                    select.append($('<option/>', {
                                        value: feature.attributes.Block_Section_Name,
                                        text: feature.attributes.Block_Section_Name
                                    }));
                                }
                            });
                            if (disabled) {
                                $('#blockSection').selectmenu('disable');
                            }
                            else {
                                $('#blockSection').selectmenu('enable');
                            }
                            $('#blockSection').selectmenu('refresh');
                            if (pageInit == 1) {
                                $('#blockSection').change();
                            }
                        });
        }
    });
    $('#blockSection').change(function () {
        var selectedBlockSection = $(this).val();
        var selectedDivision = $('#divisionSelect').val()
        var selectedCemetery = $('#cemeterySelect').val();
        var Block_Section_ID = selectedCemetery + '-' + selectedDivision + '-' + selectedBlockSection;
        if (selectedDivision != null && selectedDivision != '') {
            $.getJSON('http://services.arcgis.com/aMgEifFIhCAICARd/ArcGIS/rest/services/Cemetery_Example/FeatureServer/3/query?where=Block_Section_ID%3D\'' + Block_Section_ID +
                        '\'&text=&objectIds=&time=&geometry=&geometryType=esriGeometryEnvelope&inSR=&spatialRel=esriSpatialRelIntersects&relationParam=&outFields=Lot_Row_ID%2C+Lot_Row_Name&returnGeometry=false&maxAllowableOffset=&geometryPrecision=&outSR=&returnIdsOnly=false&returnCountOnly=false&orderByFields=Lot_Row_Name&groupByFieldsForStatistics=&outStatistics=&returnZ=false&returnM=false&gdbVersion=&returnDistinctValues=false&f=pjson', function (data) {
                            var select = $('#lotRow');
                            select.empty();
                            $.each(data.features, function (index, feature) {
                                if (space_ID.substring(7, 10) == feature.attributes.Lot_Row_Name && pageInit == 1) {
                                    select.append($('<option/>', {
                                        value: feature.attributes.Lot_Row_Name,
                                        text: feature.attributes.Lot_Row_Name,
                                        selected: "selected"
                                    }));
                                }
                                else {
                                    select.append($('<option/>', {
                                        value: feature.attributes.Lot_Row_Name,
                                        text: feature.attributes.Lot_Row_Name
                                    }));
                                }
                            });
                            if (disabled) {
                                $('#lotRow').selectmenu('disable');
                            }
                            else {
                                $('#lotRow').selectmenu('enable');
                            }
                            $('#lotRow').selectmenu('refresh');
                            if (pageInit == 1) {
                                $('#lotRow').change();
                            }
                        });
        }
    });
    $('#lotRow').change(function () {
        var selectedLot = $(this).val();
        var selectedBlockSection = $('#blockSection').val()
        var selectedDivision = $('#divisionSelect').val()
        var selectedCemetery = $('#cemeterySelect').val();
        var Lot_ID = selectedCemetery + '-' + selectedDivision + '-' + selectedBlockSection + '-' + selectedLot;
        if (selectedDivision != null && selectedDivision != '') {
            $.getJSON('http://services.arcgis.com/aMgEifFIhCAICARd/ArcGIS/rest/services/Cemetery_Example/FeatureServer/5/query?where=Lot_Row_ID%3D\'' + Lot_ID +
                        '\'&text=&objectIds=&time=&geometry=&geometryType=esriGeometryEnvelope&inSR=&spatialRel=esriSpatialRelIntersects&relationParam=&outFields=Space_ID%2C+Space_Number&returnGeometry=false&maxAllowableOffset=&geometryPrecision=&outSR=&returnIdsOnly=false&returnCountOnly=false&orderByFields=Space_Number&groupByFieldsForStatistics=&outStatistics=&returnZ=false&returnM=false&gdbVersion=&returnDistinctValues=false&f=pjson', function (data) {
                            var select = $('#space');
                            select.empty();
                            $.each(data.features, function (index, feature) {
                                if (space_ID == feature.attributes.Space_ID && pageInit == 1) {
                                    select.append($('<option/>', {
                                        value: feature.attributes.Space_ID,
                                        text: feature.attributes.Space_Number,
                                        selected: "selected"
                                    }));
                                }
                                else {
                                    select.append($('<option/>', {
                                        value: feature.attributes.Space_ID,
                                        text: feature.attributes.Space_Number
                                    }));
                                }
                            });
                            if (disabled) {
                                $('#space').selectmenu('disable');
                            }
                            else {
                                $('#space').selectmenu('enable');
                                $('#Space_ID').val($('#space').val());
                            }
                            $('#space').selectmenu('refresh');
                            if (pageInit == 1) {
                                $('#space').change();
                            }
                        });
        }
    });
    $('#space').change(function () {
        $('#Space_ID').val($('#space').val());
        setCookie("space_id", $('#space').val(), 30);
        if (disabled == false) {
            space_ID = $('#space').val();
        }
        if (pageInit == 1) {
            pageInit = 0;
        }
    });
    $.getJSON('http://services.arcgis.com/aMgEifFIhCAICARd/ArcGIS/rest/services/Cemetery_Example/FeatureServer/0/query?where=1%3D1&text=&objectIds=&time=&geometry=&geometryType=esriGeometryEnvelope&inSR=&spatialRel=esriSpatialRelIntersects&relationParam=&outFields=Cemetery_ID%2C+Cemetery_Name&returnGeometry=false&maxAllowableOffset=&geometryPrecision=&outSR=&returnIdsOnly=false&returnCountOnly=false&orderByFields=Cemetery_Name&groupByFieldsForStatistics=&outStatistics=&returnZ=false&returnM=false&gdbVersion=&returnDistinctValues=false&f=pjson', function (data) {
        var select = $('#cemeterySelect');
        select.empty();
        $.each(data.features, function (index, feature) {
            if (space_ID.substring(0, 1) == feature.attributes.Cemetery_ID) {
                select.append($('<option/>', {
                    value: feature.attributes.Cemetery_ID,
                    text: feature.attributes.Cemetery_Name,
                    selected: "selected"
                }));
            }
            else {
                select.append($('<option/>', {
                    value: feature.attributes.Cemetery_ID,
                    text: feature.attributes.Cemetery_Name
                }));
            }
        });
        if (disabled) {
            $('#cemeterySelect').selectmenu('disable');
        }
        $('#cemeterySelect').selectmenu('refresh');
        $('#cemeterySelect').change();
    });

    $('#Space_ID').prop('readonly', 'readonly');

}