//define.amd.jQuery = true;
//dojo.require("esri.map");
//dojo.require("esri.geometry.Extent");
//dojo.require("esri.layers.agstiled");
//dojo.require("esri.toolbars.draw");
//dojo.require("esri.layers.FeatureLayer");
//dojo.require("esri.tasks.query");
//dojo.require("dojo.request");
//dojo.require("esri.graphic");
//dojo.require("esri.symbols.SimpleLineSymbol");
//dojo.require("esri.symbols.SimpleFillSymbol");
//dojo.require("esri.symbols.TextSymbol");
//dojo.require("esri.renderers.SimpleRenderer");

//dojo.require("esri.layers.LabelLayer");

//dojo.require("esri.Color");

var baseURL = "http://wincem.cloudapp.net/wincem/";
var map;
var CemeteryMapURL = "http://services.arcgis.com/aMgEifFIhCAICARd/ArcGIS/rest/services/Cemetery_Example/FeatureServer/0"
var BaseMapURL = "http://tiles.arcgis.com/tiles/aMgEifFIhCAICARd/arcgis/rest/services/Cemetery_Example/MapServer"
var Cemeteries = "http://services.arcgis.com/aMgEifFIhCAICARd/ArcGIS/rest/services/Cemetery_Example/FeatureServer/0/query";
var Division = "http://services.arcgis.com/aMgEifFIhCAICARd/ArcGIS/rest/services/Cemetery_Example/FeatureServer/1/query";
var Block_Section = "http://services.arcgis.com/aMgEifFIhCAICARd/ArcGIS/rest/services/Cemetery_Example/FeatureServer/2/query";
var Lot_Row = "http://services.arcgis.com/aMgEifFIhCAICARd/ArcGIS/rest/services/Cemetery_Example/FeatureServer/3/query";
var Spaces = "http://services.arcgis.com/aMgEifFIhCAICARd/ArcGIS/rest/services/Cemetery_Example/FeatureServer/5/query";
var identify, createDeed, createAssignment;
require([
  "esri/map",
  "esri/geometry/Extent",
  "esri/layers/FeatureLayer",
  "esri/toolbars/draw",
  "esri/tasks/query",
  "dojo/request",
  "esri/graphic",
  "esri/symbols/SimpleLineSymbol",
  "esri/symbols/SimpleFillSymbol",
  "esri/symbols/TextSymbol",
  "esri/renderers/SimpleRenderer",

  "esri/layers/LabelLayer",

  "esri/Color",
  "dojo/domReady!"
], function (
  Map, Extent, FeatureLayer,
  SimpleLineSymbol, SimpleFillSymbol, TextSymbol, SimpleRenderer,
  LabelLayer,
  Color
) {
    cemeteryLayers = new esri.layers.FeatureLayer(CemeteryMapURL, { visible: true, outFields: ["Cemetery_Name"], mode: FeatureLayer.MODE_ONDEMAND });
    cemeteryLabels = new esri.layers.FeatureLayer("http://tiles.arcgis.com/tiles/aMgEifFIhCAICARd/arcgis/rest/services/Cemetery_Example/MapServer/0", { visible: true, outFields: ["Cemetery_Name"], mode: FeatureLayer.MODE_ONDEMAND });
    DivisionLayer = new esri.layers.FeatureLayer("http://services.arcgis.com/aMgEifFIhCAICARd/ArcGIS/rest/services/Cemetery_Example/FeatureServer/1", { "visible": true });
    Block_SectionLayer = new esri.layers.FeatureLayer("http://services.arcgis.com/aMgEifFIhCAICARd/ArcGIS/rest/services/Cemetery_Example/FeatureServer/2", { "visible": true });
    Lot_RowLayer = new esri.layers.FeatureLayer("http://services.arcgis.com/aMgEifFIhCAICARd/ArcGIS/rest/services/Cemetery_Example/FeatureServer/3", { "visible": true});
    BurialLayer = new esri.layers.FeatureLayer("http://services.arcgis.com/aMgEifFIhCAICARd/ArcGIS/rest/services/Cemetery_Example/FeatureServer/4", { "visible": true});
    SpaceLayer = new esri.layers.FeatureLayer("http://services.arcgis.com/aMgEifFIhCAICARd/ArcGIS/rest/services/Cemetery_Example/FeatureServer/5", { "visible": true});
    var bounds = new esri.geometry.Extent({
        "xmin": -16045622,
        "ymin": -811556,
        "xmax": 7297718,
        "ymax": 11142818,
        "spatialReference": { "wkid": 102100 }
    });

    map = new esri.Map("mapDiv", {
        extent: bounds
    });
    //var statesColor = new esri.Color("#666");
    //// create a text symbol to define the style of labels
    //var statesLabel = new esri.symbols.TextSymbol("Test");
    //statesLabel.setColor(statesColor);
    //statesLabel.font.setSize("14pt");
    //statesLabel.font.setFamily("arial");
    var simpleJson = {
        "type": "simple",
        "label": "",
        "description": "",
        "symbol": {
            "color": [210, 105, 30, 191],
            "size": 14,
            "angle": 0,
            "xoffset": 0,
            "yoffset": 0,
            "type": "esriSMS",
            "style": "esriSMSCircle",
            "outline": {
                "color": [0, 0, 128, 255],
                "width": 0,
                "type": "esriSLS",
                "style": "esriSLSSolid"
            }
        }
    }

    statesLabelRenderer = new SimpleRenderer(simpleJson);
    var labels = new esri.layers.LabelLayer({ id: "labels" });
    // tell the label layer to label the countries feature layer 
    // using the field named "admin"
    labels.addFeatureLayer(cemeteryLabels, statesLabelRenderer, "${Cemetery_Name}");
    // add the label layer to the map
    //map.addLayer(labels);


    //var basemap = new esri.layers.ArcGISTiledMapServiceLayer(BaseMapURL);
    //map.addLayer(basemap);
    identify = dojo.connect(map, "onClick", function (evt) {
        identifySpace(evt.mapPoint);
    });
    map.addLayer(cemeteryLayers);
    map.addLayer(DivisionLayer);
    map.addLayer(Block_SectionLayer);
    map.addLayer(Lot_RowLayer);

    map.addLayer(SpaceLayer);
    map.addLayer(BurialLayer);
    map.addLayer(labels);
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
    zoomToCemetery("Highland");
});

//function init() {

//    cemeteryLayers = new esri.layers.FeatureLayer(CemeteryMapURL, { "visible": true, "outFields": ["Cemetery_Name"] });
//    DivisionLayer = new esri.layers.FeatureLayer("http://services.arcgis.com/aMgEifFIhCAICARd/ArcGIS/rest/services/Cemetery_Example/FeatureServer/1", { "visible": true});
//    Block_SectionLayer = new esri.layers.FeatureLayer("http://services.arcgis.com/aMgEifFIhCAICARd/ArcGIS/rest/services/Cemetery_Example/FeatureServer/2", { "visible": true });
//    Lot_RowLayer = new esri.layers.FeatureLayer("http://services.arcgis.com/aMgEifFIhCAICARd/ArcGIS/rest/services/Cemetery_Example/FeatureServer/3", { "visible": true, "opacity": 0.5 });
//    BurialLayer = new esri.layers.FeatureLayer("http://services.arcgis.com/aMgEifFIhCAICARd/ArcGIS/rest/services/Cemetery_Example/FeatureServer/4", { "visible": true, "opacity": 0.5 });
//    SpaceLayer = new esri.layers.FeatureLayer("http://services.arcgis.com/aMgEifFIhCAICARd/ArcGIS/rest/services/Cemetery_Example/FeatureServer/5", { "visible": true, "opacity": 0.5 });
//    var bounds = new esri.geometry.Extent({
//        "xmin": -16045622,
//        "ymin": -811556,
//        "xmax": 7297718,
//        "ymax": 11142818,
//        "spatialReference": { "wkid": 102100 }
//    });

//    map = new esri.Map("mapDiv", {
//        extent: bounds
//    });
//    var statesColor = new esri.Color("#666");
//    // create a text symbol to define the style of labels
//    var statesLabel = new esri.TextSymbol();
//    statesLabel.font.setSize("14pt");
//    statesLabel.font.setFamily("arial");
//    statesLabelRenderer = new esri.renderers.SimpleRenderer(statesLabel);
//    var labels = new esri.layers.LabelLayer({ id: "labels" });
//    // tell the label layer to label the countries feature layer 
//    // using the field named "admin"
//    labels.addFeatureLayer(cemeteryLayers, statesLabelRenderer, "${Cemetery_Name}");
//    // add the label layer to the map
//    //map.addLayer(labels);


//    //var basemap = new esri.layers.ArcGISTiledMapServiceLayer(BaseMapURL);
//    //map.addLayer(basemap);
//    identify = dojo.connect(map, "onClick", function (evt) {
//        identifySpace(evt.mapPoint);
//    });
//    map.addLayer(cemeteryLayers);
//    map.addLayer(DivisionLayer);
//    map.addLayer(Block_SectionLayer);
//    map.addLayer(Lot_RowLayer);
//    map.addLayer(BurialLayer);
//    map.addLayer(SpaceLayer);
//    map.addLayer(labels);
//    map.resize();
//    if (getParameterByName("Space_ID") != "") {
//        zoomToSpace(getParameterByName("Space_ID"));
//    }
//    if (getParameterByName("SpaceList") != "") {
//        zoomToSpaces(getParameterByName("SpaceList"));
//    }
//    $("#createDeedCollapse").bind("collapse", function (e) {
//        $('#createDeedList > li').each(function (index) {
//            if ($(this).attr("role") != "heading")
//                $(this).remove();
//        });
//        map.graphics.clear();
//        $('#createDeedList').listview('refresh');
//        dojo.disconnect(createDeed);
//        dojo.disconnect(identify);
//        var collapsed = $("#createAssignmentCollapse").collapsible("option", "collapsed");
//        $("#createDeedCollapse").collapsible("option", "collapsed", true);
//        if (collapsed != false) {
//            identify = dojo.connect(map, "onClick", function (evt) {
//                identifySpace(evt.mapPoint);
//            });
//        }
//    });

//    $("#createAssignmentCollapse").bind("collapse", function (e) {
//        $('#createAssignmentList > li').each(function (index) {
//            if ($(this).attr("role") != "heading")
//                $(this).remove();
//        });
//        map.graphics.clear();
//        $('#createAssignmentList').listview('refresh');
//        dojo.disconnect(createAssignment);
//        dojo.disconnect(identify);
//        var collapsed = $("#createDeedCollapse").collapsible("option", "collapsed");
//        $("#createAssignmentCollapse").collapsible("option", "collapsed", true);
//        if (collapsed != false) {
//            identify = dojo.connect(map, "onClick", function (evt) {
//                identifySpace(evt.mapPoint);
//            });
//        }
//    });
//    zoomToCemetery("Highland");
//}
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
        url: 'http://wincem.cloudapp.net/wincem/Burial/burialTotals?' + "startDate=" + $("#bt_start_date").val() + "&endDate=" + $("#bt_end_date").val(),
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
        url: 'http://wincem.cloudapp.net/wincem/Owner/deedTotals?' + "startDate=" + $("#dt_start_date").val() + "&endDate=" + $("#dt_end_date").val(),
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
    window.location.href = baseURL + "Owner/Create?spaceList=" + spaceList.slice(0, -1) + "&deedType=D";
    //window.location.replace(baseURL + "Owner/Create?spaceList=" + spaceList.slice(0, -1)) + "&deedType=D";
});

$(document).on("click", "#createAssignmentOK", function () {
    $.mobile.showPageLoadingMsg();
    var spaceList = "";
    $('#createAssignmentList > li').each(function (index) {
        if ($(this).attr("role") != "heading")
            spaceList = spaceList + $(this).attr("data-space_id") + ",";
    });
    window.location.href = baseURL + "Owner/Create?spaceList=" + spaceList.slice(0, -1) + "&deedType=A";
    //window.location.replace(baseURL + "Owner/Create?spaceList=" + spaceList.slice(0, -1)) + "&deedType=D";
});

$(document).on("change", ".burialReport", function () {
    $("#burialReportOK").attr("href", "http://wincem.cloudapp.net/wincem/burial/BurialReport?cemetery=" + ($("#burialReportCemetery").val().length == 0 ? 0 : $("#burialReportCemetery").val()) + "&startDate=" + $("#start_date").val() + "&endDate=" + $("#end_date").val())
});

$(document).on("click", ".deedRecord", function (e) {
    //$('#deleteLink').attr('href', '/wincem/Burial/Delete/' + $(this).attr("id"));
    //$('#editLink').attr('href', '/wincem/Burial/Edit/' + $(this).attr("id"));
    //$('#burialPopup').popup('open', { positionTo: "#" + $(this).attr("id") });
    //$('#burialPopup').popup("reposition", { positionTo: "#" + $(this).attr("id") });
    zoomToSpaces($(this).attr("data-spaceList"));
    return false;
});

$(document).on("click", ".burialRecord", function (e) {
    //$('#deleteLink').attr('href', '/wincem/Burial/Delete/' + $(this).attr("id"));
    //$('#editLink').attr('href', '/wincem/Burial/Edit/' + $(this).attr("id"));
    //$('#burialPopup').popup('open', { positionTo: "#" + $(this).attr("id") });
    //$('#burialPopup').popup("reposition", { positionTo: "#" + $(this).attr("id") });
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
    //$('#identifyClose:Event(!click)').each(function () {
    $("#identifyClose").unbind("click");
    $("#identifyClose").on("click", function () {
        $('#identifyPopup').popup('close');
    });
    //});

    $("#toolsPanelClose").on("click", function () {
        $('#toolsPanel').panel('close');
    });
});

//dojo.ready(init);

function zoomToCemetery(cemeteryName) {
    $.ajax({
        url: Cemeteries,
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
            cemetery.setSpatialReference(new esri.SpatialReference(102100));
            map.setExtent(cemetery.getExtent(), true);
            setTimeout(function () { $.mobile.hidePageLoadingMsg(); }, 500);
        }
    });

}

function zoomToSpace(spaceID) {
    $.ajax({
        url: Spaces,
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
            space.setSpatialReference(new esri.SpatialReference(102100));
            map.graphics.add(new esri.Graphic(space, new esri.symbol.SimpleFillSymbol()));
            map.setExtent(space.getExtent(), true);
            //content = 'Space ID: ' + data.features[0].attributes.Space_ID
            //$.ajax({
            //    url: 'Owner/DetailsMap/?Space_ID=' + data.features[0].attributes.Space_ID,
            //    success: function (data) {
            //        $('#identifyContent').append(data);
            //        $("[id^='ownerIdentifyButton']").button();
            //        $("[id^='ownerCreateButton']").button();
            //        $("[id^='addBurialButton']").button();
            //        $("[id^='spaceInfoButton']").button();
            //    }
            //});

            //$('#identifyContent').html(content);
            //$('#identifyPopup').popup('open');
            identifySpace(space.getExtent().getCenter());
        }
    });
}

function zoomToSpaces(spaceList) {
    $.ajax({
        url: Spaces,
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
                //initialExtent.setSpatialReference(new esri.SpatialReference(102678));
                initialExtent.setSpatialReference(new esri.SpatialReference(102100));
                var extent = new esri.geometry.Extent(initialExtent.getExtent().toJson());

                $.each(data.features, function (index, value) {
                    space = new esri.geometry.Polygon(data.features[index].geometry);
                    //space.setSpatialReference(new esri.SpatialReference(102678));
                    space.setSpatialReference(new esri.SpatialReference(102100));
                    map.graphics.add(new esri.Graphic(space, new esri.symbol.SimpleFillSymbol()));
                    extent.union(space.getExtent());
                });
                map.setExtent(extent, true);
            }
            else {
                alert("No Spaces defined for this deed!");
            }
            //content = 'Space ID: ' + data.features[0].attributes.Space_ID
            //$.ajax({
            //    url: 'Owner/DetailsMap/?Space_ID=' + data.features[0].attributes.Space_ID,
            //    success: function (data) {
            //        $('#identifyContent').append(data);
            //        $("[id^='ownerIdentifyButton']").button();
            //        $("[id^='ownerCreateButton']").button();
            //        $("[id^='addBurialButton']").button();
            //        $("[id^='spaceInfoButton']").button();
            //    }
            //});

            //$('#identifyContent').html(content);
            //$('#identifyPopup').popup('open');
        }
    });
}


function identifySpace(mapPoint) {
    $("#deedTotalPopup").popup("close");
    $("#burialTotalPopup").popup("close");
    $("#bookmarkMenu").popup("close");
    $.ajax({
        url: Spaces,
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
                space.setSpatialReference(new esri.SpatialReference(102100));
                map.graphics.add(new esri.Graphic(space, new esri.symbol.SimpleFillSymbol()));
                $.ajax({
                    url: 'http://wincem.cloudapp.net/wincem/Owner/DetailsMap/?Space_ID=' + data.features[0].attributes.Space_ID,
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
        url: Spaces,
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
                    space.setSpatialReference(new esri.SpatialReference(102100));
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
        url: Spaces,
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
                    space.setSpatialReference(new esri.SpatialReference(102100));
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


function addBurial(space_ID, burial_type) {
    $.ajax({
        url: Spaces,
        dataType: "jsonp",
        data: {
            where: "Space_ID = '" + space_ID + "'",
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
            centroid_x = 0;
            centroid_y = 0;
            for(var x = 0; x < space.rings[0].length - 1; x ++){
                centroid_x = centroid_x + space.getPoint(0, x).x;
                centroid_y = centroid_y + space.getPoint(0, x).y;
            };
            centroid_y = centroid_y / (space.rings[0].length - 1);
            centroid_x = centroid_x / (space.rings[0].length - 1);

            if (burial_type == "Cremation1" || burial_type == "Cremation2" || burial_type == "Cremation3") {
                BurialsFeatureLayer.applyEdits([new esri.Graphic(drawCremation(centroid_x, centroid_y, burial_type, data.features[0].attributes.Space_ID))], null, null);
            }
            else if (burial_type == "Full_Burial"){
                BurialsFeatureLayer.applyEdits([new esri.Graphic(drawCasket(centroid_x, centroid_y, data.features[0].attributes.Space_ID))], null, null);
            }
            else if (burial_type == "Infant1" || burial_type == "Infant2" || burial_type == "Infant3") {
                BurialsFeatureLayer.applyEdits([new esri.Graphic(drawInfantCasket(centroid_x, centroid_y, burial_type, data.features[0].attributes.Space_ID))], null, null);
            }

        }
    });
}

function drawCasket(centroid_x, centroid_y, space_ID) {
    width1 = 8;
    width2 = 4;
    height1 = 1.5;
    height2 = 3;
    oposite = Math.abs(space.getPoint(0, 0).x - space.getPoint(0, 1).x);
    adjacent = Math.abs(space.getPoint(0, 0).y - space.getPoint(0, 1).y);
    var angle = 0;
    if (oposite > adjacent) {
        angle = Math.atan(-adjacent / oposite);
    }
    else {
        angle = Math.atan(-oposite / adjacent);
    }

    //*************************************************
    //**********p2*************************************
    //****p1******************p3***********************
    //****p0******************p4***********************
    //**********p5*************************************
    //*************************************************

    p0_x = centroid_x - (((width1 / 2) * Math.cos(angle)) + ((height1 / 2) * Math.sin(angle)))
    p0_y = centroid_y - (((height1 / 2) * Math.cos(angle)) - ((width1 / 2) * Math.sin(angle)))

    p1_x = centroid_x - (((width1 / 2) * Math.cos(angle)) - ((height1 / 2) * Math.sin(angle)))
    p1_y = centroid_y + (((height1 / 2) * Math.cos(angle)) + ((width1 / 2) * Math.sin(angle)))

    p2_x = centroid_x - (((width2 / 2) * Math.cos(angle)) - ((height2 / 2) * Math.sin(angle)))
    p2_y = centroid_y + (((height2 / 2) * Math.cos(angle)) + ((width2 / 2) * Math.sin(angle)))

    p3_x = centroid_x + (((width1 / 2) * Math.cos(angle)) + ((height1 / 2) * Math.sin(angle)))
    p3_y = centroid_y + (((height1 / 2) * Math.cos(angle)) - ((width1 / 2) * Math.sin(angle)))

    p4_x = centroid_x + (((width1 / 2) * Math.cos(angle)) - ((height1 / 2) * Math.sin(angle)))
    p4_y = centroid_y - (((height1 / 2) * Math.cos(angle)) + ((width1 / 2) * Math.sin(angle)))

    p5_x = centroid_x - (((width2 / 2) * Math.cos(angle)) + ((height2 / 2) * Math.sin(angle)))
    p5_y = centroid_y - (((height2 / 2) * Math.cos(angle)) - ((width2 / 2) * Math.sin(angle)))

    var casket = {
        "geometry": {
            "rings": [[[p0_x, p0_y], [p1_x, p1_y],
    [p2_x, p2_y], [p3_x, p3_y], [p4_x, p4_y],
    [p5_x, p5_y], [p0_x, p0_y]]], "spatialReference": { "wkid": 3420 }
        },
        "attributes": {
            "Burial_ID": null,
            "Space_ID": space_ID
        },
        "symbol": {
            "color": [0, 0, 0, 64], "outline": {
                "color": [0, 0, 0, 255],
                "width": 1, "type": "esriSLS", "style": "esriSLSSolid"
            },
            "type": "esriSFS", "style": "esriSFSSolid"
        }
    };

    return casket;

}

function drawInfantCasket(centroid_x, centroid_y, position, space_ID) {
    width1 = 4;
    width2 = 2;
    height1 = 0.75;
    height2 = 1.5;
    oposite = Math.abs(space.getPoint(0, 0).x - space.getPoint(0, 1).x);
    adjacent = Math.abs(space.getPoint(0, 0).y - space.getPoint(0, 1).y);
    var angle = 0;
    if (oposite > adjacent) {
        angle = Math.atan(-adjacent / oposite);
    }
    else {
        angle = Math.atan(-oposite / adjacent);
    }

    if (position == "Infant2") {
        centroid_x = centroid_x - (((2.5) * Math.cos(angle)) - ((0) * Math.sin(angle)));
        centroid_y = centroid_y + (((0) * Math.cos(angle)) + ((2.5) * Math.sin(angle)));
    }
    else if (position == "Infant3") {
        centroid_x = centroid_x + (((2.5) * Math.cos(angle)) - ((0) * Math.sin(angle)));
        centroid_y = centroid_y - (((0) * Math.cos(angle)) + ((2.5) * Math.sin(angle)));
    }

    //*************************************************
    //**********p2*************************************
    //****p1******************p3***********************
    //****p0******************p4***********************
    //**********p5*************************************
    //*************************************************

    p0_x = centroid_x - (((width1 / 2) * Math.cos(angle)) + ((height1 / 2) * Math.sin(angle)))
    p0_y = centroid_y - (((height1 / 2) * Math.cos(angle)) - ((width1 / 2) * Math.sin(angle)))

    p1_x = centroid_x - (((width1 / 2) * Math.cos(angle)) - ((height1 / 2) * Math.sin(angle)))
    p1_y = centroid_y + (((height1 / 2) * Math.cos(angle)) + ((width1 / 2) * Math.sin(angle)))

    p2_x = centroid_x - (((width2 / 2) * Math.cos(angle)) - ((height2 / 2) * Math.sin(angle)))
    p2_y = centroid_y + (((height2 / 2) * Math.cos(angle)) + ((width2 / 2) * Math.sin(angle)))

    p3_x = centroid_x + (((width1 / 2) * Math.cos(angle)) + ((height1 / 2) * Math.sin(angle)))
    p3_y = centroid_y + (((height1 / 2) * Math.cos(angle)) - ((width1 / 2) * Math.sin(angle)))

    p4_x = centroid_x + (((width1 / 2) * Math.cos(angle)) - ((height1 / 2) * Math.sin(angle)))
    p4_y = centroid_y - (((height1 / 2) * Math.cos(angle)) + ((width1 / 2) * Math.sin(angle)))

    p5_x = centroid_x - (((width2 / 2) * Math.cos(angle)) + ((height2 / 2) * Math.sin(angle)))
    p5_y = centroid_y - (((height2 / 2) * Math.cos(angle)) - ((width2 / 2) * Math.sin(angle)))

    var casket = {
        "geometry": {
            "rings": [[[p0_x, p0_y], [p1_x, p1_y],
    [p2_x, p2_y], [p3_x, p3_y], [p4_x, p4_y],
    [p5_x, p5_y], [p0_x, p0_y]]], "spatialReference": { "wkid": 3420 }
        },
        "attributes": {
            "Burial_ID": null,
            "Space_ID": space_ID
        },
        "symbol": {
            "color": [0, 0, 0, 64], "outline": {
                "color": [0, 0, 0, 255],
                "width": 1, "type": "esriSLS", "style": "esriSLSSolid"
            },
            "type": "esriSFS", "style": "esriSFSSolid"
        }
    };

    return casket;

}

function drawCremation(centroid_x, centroid_y, position, space_ID) {
    width1 = 1.75;
    width2 = 1;
    height1 = 1;
    height2 = 1.75;
    oposite = Math.abs(space.getPoint(0, 0).x - space.getPoint(0, 1).x);
    adjacent = Math.abs(space.getPoint(0, 0).y - space.getPoint(0, 1).y);
    var angle = 0;
    if (oposite > adjacent) {
        angle = Math.atan(-adjacent / oposite);
    }
    else {
        angle = Math.atan(-oposite / adjacent);
    }
    if (position == "Cremation1") {
        centroid_x = centroid_x + (((1.5) * Math.cos(angle)) - ((0) * Math.sin(angle)));
        centroid_y = centroid_y - (((0) * Math.cos(angle)) + ((1.5) * Math.sin(angle)));
    }
    else if (position == "Cremation2") {
        centroid_x = centroid_x + (((3.5) * Math.cos(angle)) - ((1) * Math.sin(angle)));
        centroid_y = centroid_y - (((1) * Math.cos(angle)) + ((3.5) * Math.sin(angle)));
    }
    else if (position == "Cremation3") {
        centroid_x = centroid_x + (((3.5) * Math.cos(angle)) + ((1) * Math.sin(angle)));
        centroid_y = centroid_y + (((1) * Math.cos(angle)) - ((3.5) * Math.sin(angle)));
    }

    //************************************************
    //********p2*p3***********************************
    //****p1*********p4*******************************
    //****p0*********p5*******************************
    //********p7*p6***********************************
    //************************************************

    p0_x = centroid_x - (((width1 / 2) * Math.cos(angle)) + ((height1 / 2) * Math.sin(angle)))
    p0_y = centroid_y - (((height1 / 2) * Math.cos(angle)) - ((width1 / 2) * Math.sin(angle)))

    p1_x = centroid_x - (((width1 / 2) * Math.cos(angle)) - ((height1 / 2) * Math.sin(angle)))
    p1_y = centroid_y + (((height1 / 2) * Math.cos(angle)) + ((width1 / 2) * Math.sin(angle)))

    p2_x = centroid_x - (((width2 / 2) * Math.cos(angle)) - ((height2 / 2) * Math.sin(angle)))
    p2_y = centroid_y + (((height2 / 2) * Math.cos(angle)) + ((width2 / 2) * Math.sin(angle)))

    p3_x = centroid_x + (((width2 / 2) * Math.cos(angle)) + ((height2 / 2) * Math.sin(angle)))
    p3_y = centroid_y + (((height2 / 2) * Math.cos(angle)) - ((width2 / 2) * Math.sin(angle)))

    p4_x = centroid_x + (((width1 / 2) * Math.cos(angle)) + ((height1 / 2) * Math.sin(angle)))
    p4_y = centroid_y + (((height1 / 2) * Math.cos(angle)) - ((width1 / 2) * Math.sin(angle)))

    p5_x = centroid_x + (((width1 / 2) * Math.cos(angle)) - ((height1 / 2) * Math.sin(angle)))
    p5_y = centroid_y - (((height1 / 2) * Math.cos(angle)) + ((width1 / 2) * Math.sin(angle)))

    p6_x = centroid_x + (((width2 / 2) * Math.cos(angle)) - ((height2 / 2) * Math.sin(angle)))
    p6_y = centroid_y - (((height2 / 2) * Math.cos(angle)) + ((width2 / 2) * Math.sin(angle)))

    p7_x = centroid_x - (((width2 / 2) * Math.cos(angle)) + ((height2 / 2) * Math.sin(angle)))
    p7_y = centroid_y - (((height2 / 2) * Math.cos(angle)) - ((width2 / 2) * Math.sin(angle)))

    var cremation = {
        "geometry": {
            "rings": [[[p0_x, p0_y], [p1_x, p1_y],
    [p2_x, p2_y], [p3_x, p3_y], [p4_x, p4_y],
    [p5_x, p5_y], [p6_x, p6_y], [p7_x, p7_y], [p0_x, p0_y]]], "spatialReference": { "wkid": 3420 }
        },
        "attributes": {
            "Burial_ID": null,
            "Space_ID": space_ID
        },
        "symbol": {
            "color": [0, 0, 0, 64], "outline": {
                "color": [0, 0, 0, 255],
                "width": 1, "type": "esriSLS", "style": "esriSLSSolid"
            },
            "type": "esriSFS", "style": "esriSFSSolid"
        }
    };

    return cremation;

}

function burialSearch() {
    if ($('#searchString').val().length > 2 || $('#searchString').val() == '') {
        $.ajax({
            url: 'http://wincem.cloudapp.net/wincem/Burial/SearchBurialsMap?SearchString=' + $('#searchString').val() + '&number_of_records=' + 50 + "&First_Name=" + true + "&Last_Name=" + true
                    + "&Permit_Number=" + true + "&Burial_Date=" + true + "&Work_Order_Date=" + true,
            beforeSend: function (xhr) {
            },
            success: function (data) {
                $('#mapSearchResults').html(data);
            },
            complete: function () {
                $('#burialList').listview();
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

function deedSearch() {
    if ($('#searchString').val().length > 2 || $('#searchString').val() == '') {
        $.ajax({
            url: 'http://wincem.cloudapp.net/wincem/Owner/SearchDeedsMap?SearchString=' + $('#searchString').val() + '&number_of_records=' + true + "&deed_number=" + true + "&Owner_Name=" + true
                    + "&Deed_Name=" + true + "&Date_of_Purchase=" + true,
            beforeSend: function (xhr) {
            },
            success: function (data) {
                $('#mapSearchResults').html(data);
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