//define.amd.jQuery = true;
dojo.require("esri.map");
dojo.require("esri.layers.agstiled");
dojo.require("esri.toolbars.draw");
dojo.require("esri.layers.FeatureLayer");
dojo.require("esri.tasks.query");
dojo.require("dojo.request");
dojo.require("esri.graphic");
var map;
var identify, createDeed, createAssignment;
function init() {
   
    cemeteryLayers = new esri.layers.ArcGISDynamicMapServiceLayer(config.cemeteryMapURL, { "visible": true, "opacity": 0.5 });
    map = new esri.Map("mapDiv", {
    });


    var basemap = new esri.layers.ArcGISTiledMapServiceLayer(config.baseMapURL);
    map.addLayer(basemap);
    identify = dojo.connect(map, "onClick", function (evt) {
        identifySpace(evt.mapPoint);
    });
    map.addLayer(cemeteryLayers);
    map.resize();
    if (getParameterByName("Space_ID") != "") {
        zoomToSpace(getParameterByName("Space_ID"));
    }
    if (getParameterByName("SpaceList") != "") {
        zoomToSpaces(getParameterByName("SpaceList"));
    }
    $("#createDeedCollapse").bind("collapse", function (e) {
        $('#createDeedList > li').each(function (index) {
            if ($(this).attr("role") != "heading")
                $(this).remove();
        });
        map.graphics.clear();
        $('#createDeedList').listview('refresh');
        dojo.disconnect(createDeed);
        dojo.disconnect(identify);
        var collapsed = $("#createAssignmentCollapse").collapsible("option", "collapsed");
        $("#createDeedCollapse").collapsible("option", "collapsed", true);
        if (collapsed != false) {
            identify = dojo.connect(map, "onClick", function (evt) {
                identifySpace(evt.mapPoint);
            });
        }
    });

    $("#createAssignmentCollapse").bind("collapse", function (e) {
        $('#createAssignmentList > li').each(function (index) {
            if ($(this).attr("role") != "heading")
                $(this).remove();
        });
        map.graphics.clear();
        $('#createAssignmentList').listview('refresh');
        dojo.disconnect(createAssignment);
        dojo.disconnect(identify);
        var collapsed = $("#createDeedCollapse").collapsible("option", "collapsed");
        $("#createAssignmentCollapse").collapsible("option", "collapsed", true);
        if (collapsed != false) {
            identify = dojo.connect(map, "onClick", function (evt) {
                identifySpace(evt.mapPoint);
            });
        }
    });
}
$(document).on("keyup", "#searchString", function () {
    var selected = $("input[type='radio'][name='radio-Search-Options']:checked");
    if (selected.length > 0)
        selectedValue = selected.val();
    if (selectedValue == "Burials")
        burialSearch();
    else
        deedSearch();
});

$(document).on("click", ".brClose", function () {
    $("#burialReportMenu").popup("close");
});

$(document).on("click", "#burialTotalsCollapseClose", function () {
    $('#burialTotalsCollapse').trigger('collapse');
});

$(document).on("click", "#deedTotalsCollapseClose", function () {
    $('#deedTotalsCollapse').trigger('collapse');
});

$(document).on("click", "#burialReportMenuClose", function () {
    $('#burialReportMenu').trigger('collapse');
});

$(document).on("click", "#burialTotals", function () {
    $("#identifyPopup").popup("close");
    $("#deedTotalPopup").popup("close");
    $("#bookmarkMenu").popup("close");
    $.ajax({
        url: config.baseURL + 'Burial/burialTotals?' + "startDate=" + $("#bt_start_date").val() + "&endDate=" + $("#bt_end_date").val(),
        beforeSend: function () {
            $.mobile.showPageLoadingMsg();
        },
        success: function (data) {
            $('#burialTotalPopupContent').html(data);
            $('#burialTotalsList').listview();
            
            setTimeout(function () {
                $("#burialTotalPopup").popup("open", { positionTo: "#burialTotals", transition: "pop" });
                $.mobile.hidePageLoadingMsg();
            }, 500);
        }
    });
});

$(document).on("click", "#deedTotals", function () {
    $("#identifyPopup").popup("close");
    $("#burialTotalPopup").popup("close");
    $("#bookmarkMenu").popup("close");
    $.ajax({
        url: config.baseURL + 'Owner/deedTotals?' + "startDate=" + $("#dt_start_date").val() + "&endDate=" + $("#dt_end_date").val(),
        beforeSend: function () {
            $.mobile.showPageLoadingMsg();
        },
        success: function (data) {
            $('#deedTotalPopupContent').html(data);
            $('#deedTotalsList').listview();
            setTimeout(function () {
                $("#deedTotalPopup").popup("open", { positionTo: "#deedTotals", transition: "pop" });
                $.mobile.hidePageLoadingMsg();
            }, 500);
        }
    });
});

$(document).on("change", "input[type='radio'][name='radio-Search-Options']", function () {
    var selected = $("input[type='radio'][name='radio-Search-Options']:checked");
    if (selected.length > 0)
        selectedValue = selected.val();
    if (selectedValue == "Burials")
        burialSearch();
    else
        deedSearch();
});

$("#createAssignmentCollapse").bind("expand", function (e) {
    map.graphics.clear();
    $("#identifyPopup").popup("close");
    var collapsed = $("#createDeedCollapse").collapsible("option", "collapsed");
    $("#createAssignmentCollapse").collapsible("option", "collapsed", false);
    if (collapsed == false) {
        //$('#createDeedCollapse').trigger('collapse');
        $('#createDeedList > li').each(function (index) {
            if ($(this).attr("role") != "heading")
                $(this).remove();
        });
        $('#createDeedList').listview('refresh');
    }
    dojo.disconnect(identify);
    dojo.disconnect(createDeed);
    createAssignment = dojo.connect(map, "onClick", function (evt) {
        $("#selectionErrorPopup").popup("close");
        selectSpaceAssignment(evt.mapPoint);
    });
});

$("#createDeedCollapse").bind("expand", function (e) {
    map.graphics.clear();
    $("#identifyPopup").popup("close");
    var collapsed = $("#createAssignmentCollapse").collapsible("option", "collapsed");
    $("#createDeedCollapse").collapsible("option", "collapsed", false);
    if (collapsed == false) {
        //$('#createAssignmentCollapse').trigger('collapse');
        $('#createAssignmentList > li').each(function (index) {
            if ($(this).attr("role") != "heading")
                $(this).remove();
        });
        $('#createAssignmentList').listview('refresh');
    }
    dojo.disconnect(identify);
    dojo.disconnect(createAssignment);
    createDeed = dojo.connect(map, "onClick", function (evt) {
        $("#selectionErrorPopup").popup("close");
        selectSpaceDeed(evt.mapPoint);
    });
});

$(document).on("click", ".deed_li", function () {
    var children = $(this).find("a").html();
    $(this).remove();
    $('#createDeedList').listview('refresh');
    $.each(map.graphics.graphics, function (index, value) {
        if ((value.attributes) == undefined) {
        }
        else {
            if (value.attributes.Space_ID == children)
                map.graphics.remove(value);
        }
    });
});

$(document).on("click", ".assignment_li", function () {
    var children = $(this).find("a").html();
    $(this).remove();
    $('#createAssignmentList').listview('refresh');
    $.each(map.graphics.graphics, function (index, value) {
        if ((value.attributes) == undefined) {
        }
        else {
            if (value.attributes.Space_ID == children)
                map.graphics.remove(value);
        }
    });
});
$(document).on("click", "#createDeedCancel", function () {
    $('#createDeedList > li').each(function (index) {
        if ($(this).attr("role") != "heading")
            $(this).remove();
    });
    map.graphics.clear();
    $('#createDeedList').listview('refresh');
    $('#createDeedList').trigger('collapse');
    dojo.disconnect(createDeed);
    
});

$(document).on("click", ".startProcessing", function () {
    $.mobile.showPageLoadingMsg();
});

$(document).on("click", "#createAssignmentCancel", function () {
    $('#createAssignmentList > li').each(function (index) {
        if ($(this).attr("role") != "heading")
            $(this).remove();
    });
    map.graphics.clear();
    $('#createAssignmentList').listview('refresh');
    $('#createAssignmentList').trigger('collapse');
    dojo.disconnect(createAssignment);
});

$(document).on("click", "#createDeedOK", function () {
    $.mobile.showPageLoadingMsg();
    var spaceList = "";
    $('#createDeedList > li').each(function (index) {
        if ($(this).attr("role") != "heading")
            spaceList = spaceList + $(this).attr("data-space_id") + ",";
    });
    window.location.href = config.baseURL + "Owner/Create?spaceList=" + spaceList.slice(0, -1) + "&deedType=D";
});

$(document).on("click", "#createAssignmentOK", function () {
    $.mobile.showPageLoadingMsg();
    var spaceList = "";
    $('#createAssignmentList > li').each(function (index) {
        if ($(this).attr("role") != "heading")
            spaceList = spaceList + $(this).attr("data-space_id") + ",";
    });
    window.location.href = config.baseURL + "Owner/Create?spaceList=" + spaceList.slice(0, -1) + "&deedType=A";
});

$(document).on("change", ".burialReport", function () {
    $("#burialReportOK").attr("href", config.baseURL + "burial/BurialReport?cemetery=" + ($("#burialReportCemetery").val().length == 0 ? 0 : $("#burialReportCemetery").val()) + "&startDate=" + $("#start_date").val() + "&endDate=" + $("#end_date").val())
});

$(document).on("click", ".deedRecord", function (e) {
    zoomToSpaces($(this).attr("data-spaceList"));
    return false;
});

$(document).on("click", ".burialRecord", function (e) {
    zoomToSpace($(this).attr("data-space_id"));
    return false;
});

$(document).on("pageinit", "#mapPage", function () {

    $("#Highland").on("click", function () {
        zoomToCemetery('Highland');
        $('#bookmarkMenu').popup("close");

    });
    $("#Graham").on("click", function () {
        zoomToCemetery('Graham');
        $('#bookmarkMenu').popup("close");

    });
    $("#St_Marys").on("click", function () {
        zoomToCemetery("St Mary''s");
        $('#bookmarkMenu').popup("close");

    });
    $("#Union").on("click", function () {
        zoomToCemetery('Union');
        $('#bookmarkMenu').popup("close");
        
    });
    $("#identifyPopup").on({
        popupbeforeposition: function () {
            var h = $(window).height();

            $("#identifyPopup").css("height", h - 150);
        }
    });

    $("#identifyClose").unbind("click");
    $("#identifyClose").on("click", function () {
        $('#identifyPopup').popup('close');
    });


    $("#toolsPanelClose").on("click", function () {
        $('#toolsPanel').panel('close');
    });
});

dojo.ready(init);

function zoomToCemetery(cemeteryName) {
    $.ajax({
        url: config.cemeteries,
        dataType: "jsonp",
        data: {
            where: "Cemetery_Name = '" + cemeteryName + "'",
            text: "",
            objectIds: "",
            time: "",
            geometry: "",
            geometryType: "esriGeometryEnvelope",
            inSR: "",
            spatialRel: "esriSpatialRelIntersects",
            relationParam: "",
            outFields: "*",
            returnGeometry: true,
            maxAllowableOffset: "",
            geometryPrecision: "",
            outSR: "",
            returnIdsOnly: false,
            returnCountOnly: false,
            orderByFields: "",
            groupByFieldsForStatistics: "",
            outStatistics: "",
            returnZ: false,
            returnM: false,
            gdbVersion: "",
            returnDistinctValues: false,
            f: "pjson"
        },
        success: function (data) {
            cemetery = new esri.geometry.Polygon(data.features[0].geometry);
            cemetery.setSpatialReference(new esri.SpatialReference(config.spatialReference));
            map.setExtent(cemetery.getExtent(), true);
            setTimeout(function () { $.mobile.hidePageLoadingMsg(); }, 500);
        }
    });

}

function zoomToSpace(spaceID) {
    $.ajax({
        url: config.spaces,
        dataType: "jsonp",
        data: {
            where: "Space_ID = '" + spaceID + "'",
            text: "",
            objectIds: "",
            time: "",
            geometry: "",
            geometryType: "esriGeometryEnvelope",
            inSR: "",
            spatialRel: "esriSpatialRelIntersects",
            relationParam: "",
            outFields: "*",
            returnGeometry: true,
            maxAllowableOffset: "",
            geometryPrecision: "",
            outSR: "",
            returnIdsOnly: false,
            returnCountOnly: false,
            orderByFields: "",
            groupByFieldsForStatistics: "",
            outStatistics: "",
            returnZ: false,
            returnM: false,
            gdbVersion: "",
            returnDistinctValues: false,
            f: "pjson"
        },
        success: function (data) {
            space = new esri.geometry.Polygon(data.features[0].geometry);
            space.setSpatialReference(new esri.SpatialReference(config.spatialReference));
            map.graphics.add(new esri.Graphic(space, new esri.symbol.SimpleFillSymbol()));
            map.setExtent(space.getExtent(), true);
            identifySpace(space.getExtent().getCenter());
        }
    });
}

function zoomToSpaces(spaceList) {
    $.ajax({
        url: config.spaces,
        dataType: "jsonp",
        data: {
            where: "Space_ID IN (" + spaceList + ")",
            text: "",
            objectIds: "",
            time: "",
            geometry: "",
            geometryType: "esriGeometryEnvelope",
            inSR: "",
            spatialRel: "esriSpatialRelIntersects",
            relationParam: "",
            outFields: "*",
            returnGeometry: true,
            maxAllowableOffset: "",
            geometryPrecision: "",
            outSR: "",
            returnIdsOnly: false,
            returnCountOnly: false,
            orderByFields: "",
            groupByFieldsForStatistics: "",
            outStatistics: "",
            returnZ: false,
            returnM: false,
            gdbVersion: "",
            returnDistinctValues: false,
            f: "pjson"
        },
        success: function (data) {
            
            map.graphics.clear();
            if (typeof (data.error) == 'undefined') {
                initialExtent = new esri.geometry.Polygon(data.features[0].geometry);
                initialExtent.setSpatialReference(new esri.SpatialReference(config.spatialReference));
                var extent = new esri.geometry.Extent(initialExtent.getExtent().toJson());

                $.each(data.features, function (index, value) {
                    space = new esri.geometry.Polygon(data.features[index].geometry);
                    space.setSpatialReference(new esri.SpatialReference(config.spatialReference));
                    map.graphics.add(new esri.Graphic(space, new esri.symbol.SimpleFillSymbol()));
                    extent.union(space.getExtent());
                });
                map.setExtent(extent, true);
            }
            else {
                alert("No Spaces defined for this deed!");
            }
        }
    });
}


function identifySpace(mapPoint) {
    $("#deedTotalPopup").popup("close");
    $("#burialTotalPopup").popup("close");
    $("#bookmarkMenu").popup("close");
    $.ajax({
        url: config.spaces,
        dataType: "jsonp",
        data: {
            where: "",
            text: "",
            objectIds: "",
            time: "",
            geometry: JSON.stringify(mapPoint.toJson()),
            geometryType: "esriGeometryPoint",
            inSR: "",
            spatialRel: "esriSpatialRelIntersects",
            relationParam: "",
            outFields: "*",
            returnGeometry: true,
            maxAllowableOffset: "",
            geometryPrecision: "",
            outSR: "",
            returnIdsOnly: false,
            returnCountOnly: false,
            orderByFields: "",
            groupByFieldsForStatistics: "",
            outStatistics: "",
            returnZ: false,
            returnM: false,
            gdbVersion: "",
            returnDistinctValues: false,
            f: "pjson"
        },
        beforeSend: function(){
            $.mobile.showPageLoadingMsg();
        },
        success: function (data) {
            if (data.features.length > 0) {
                map.graphics.clear();
                content = 'Space ID: ' + data.features[0].attributes.Space_ID
                space = new esri.geometry.Polygon(data.features[0].geometry);
                space.setSpatialReference(new esri.SpatialReference(config.spatialReference));
                map.graphics.add(new esri.Graphic(space, new esri.symbol.SimpleFillSymbol()));
                $.ajax({
                    url: config.baseURL + 'Owner/DetailsMap/?Space_ID=' + data.features[0].attributes.Space_ID,
                    success: function (data) {
                        $('#identifyContent').append(data);
                        $("[id^='ownerIdentifyButton']").button();
                        $("[id^='ownerCreateButton']").button();
                        $("[id^='addBurialButton']").button();
                        $("[id^='spaceInfoButton']").button();
                        $("[id^='ClosephotoPopup']").button();
                        $("[id^='photoPopup']").popup();
                        $.mobile.hidePageLoadingMsg();
                    }
                });
                $('#identifyContent').html(content);
                setTimeout(function () {
                    $('#identifyPopup').popup('open');
                }, 500);
            }
            else {
                $.mobile.hidePageLoadingMsg();
            }
        }
    });
}

function selectSpaceDeed(mapPoint) {
    $.ajax({
        url: config.spaces,
        dataType: "jsonp",
        data: {
            where: "Available = 'Yes'",
            text: "",
            objectIds: "",
            time: "",
            geometry: JSON.stringify(mapPoint.toJson()),
            geometryType: "esriGeometryPoint",
            inSR: "",
            spatialRel: "esriSpatialRelIntersects",
            relationParam: "",
            outFields: "*",
            returnGeometry: true,
            maxAllowableOffset: "",
            geometryPrecision: "",
            outSR: "",
            returnIdsOnly: false,
            returnCountOnly: false,
            orderByFields: "",
            groupByFieldsForStatistics: "",
            outStatistics: "",
            returnZ: false,
            returnM: false,
            gdbVersion: "",
            returnDistinctValues: false,
            f: "pjson"
        },
        beforeSend: function () {
            //$.mobile.showPageLoadingMsg();
        },
        success: function (data) {
            var notSelected = true;
            if (data.features.length > 0) {
                $('#createDeedList > li').each(function (index) {
                    if ($(this).attr("data-space_id") == data.features[0].attributes.Space_ID)
                        notSelected = false;
                });
                if (notSelected) {
                    space = new esri.geometry.Polygon(data.features[0].geometry);
                    space.setSpatialReference(new esri.SpatialReference(config.spatialReference));
                    map.graphics.add(new esri.Graphic(space, new esri.symbol.SimpleFillSymbol(), data.features[0].attributes));
                    $('#createDeedList').append("<li class='deed_li' data-space_id='" + data.features[0].attributes.Space_ID + "'><a href='#'>" + data.features[0].attributes.Space_ID + '</a></li>');
                    $('#createDeedList').listview('refresh');
                }
            }
            else {
                $("#selectionErrorPopup").popup("open", {positionTo: "window", transition: "pop"});
            }
        }
    });
}

function selectSpaceAssignment(mapPoint) {
    $.ajax({
        url: config.spaces,
        dataType: "jsonp",
        data: {
            where: "Available = 'No'",
            text: "",
            objectIds: "",
            time: "",
            geometry: JSON.stringify(mapPoint.toJson()),
            geometryType: "esriGeometryPoint",
            inSR: "",
            spatialRel: "esriSpatialRelIntersects",
            relationParam: "",
            outFields: "*",
            returnGeometry: true,
            maxAllowableOffset: "",
            geometryPrecision: "",
            outSR: "",
            returnIdsOnly: false,
            returnCountOnly: false,
            orderByFields: "",
            groupByFieldsForStatistics: "",
            outStatistics: "",
            returnZ: false,
            returnM: false,
            gdbVersion: "",
            returnDistinctValues: false,
            f: "pjson"
        },
        beforeSend: function () {
            //$.mobile.showPageLoadingMsg();
        },
        success: function (data) {
            var notSelected = true;
            if (data.features.length > 0) {
                $('#createAssignmentList > li').each(function (index) {
                    if ($(this).attr("data-space_id") == data.features[0].attributes.Space_ID)
                        notSelected = false;
                });
                if (notSelected) {
                    space = new esri.geometry.Polygon(data.features[0].geometry);
                    space.setSpatialReference(new esri.SpatialReference(config.spatialReference));
                    map.graphics.add(new esri.Graphic(space, new esri.symbol.SimpleFillSymbol(), data.features[0].attributes));
                    $('#createAssignmentList').append("<li class='assignment_li' data-space_id='" + data.features[0].attributes.Space_ID + "'><a href='#'>" + data.features[0].attributes.Space_ID + '</a></li>');
                    $('#createAssignmentList').listview('refresh');
                }
            }
            else {
                $("#selectionErrorPopup").popup("open", { positionTo: "window", transition: "pop" });
            }
        }
    });
}

function getParameterByName(name) {
    name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.search);
    return results == null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
}

function burialSearch() {
    if ($('#searchString').val().length > 2 || $('#searchString').val() == '') {
        $.ajax({
            url: config.baseURL + 'Burial/SearchBurialsMap?SearchString=' + $('#searchString').val() + '&number_of_records=' + 50 + "&First_Name=" + true + "&Last_Name=" + true
                    + "&Permit_Number=" + true + "&Burial_Date=" + true + "&Work_Order_Date=" + true,
            beforeSend: function (xhr) {
            },
            success: function (data) {
                $('#mapSearchResults').html(data);
            },
            complete: function () {
                $('#burialList').listview();
            }
        });
    }
}

function deedSearch() {
    if ($('#searchString').val().length > 2 || $('#searchString').val() == '') {
        $.ajax({
            url: config.baseURL + 'Owner/SearchDeedsMap?SearchString=' + $('#searchString').val() + '&number_of_records=' + true + "&deed_number=" + true + "&Owner_Name=" + true
                    + "&Deed_Name=" + true + "&Date_of_Purchase=" + true,
            beforeSend: function (xhr) {
            },
            success: function (data) {
                $('#mapSearchResults').html(data);
            },
            complete: function () {
                $('#deedList').listview();
            }
        });
    }
}