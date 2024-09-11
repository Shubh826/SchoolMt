var map, heuroDrawingManager, shape, temp;
var allPoints, selectedshape;
var fonas;
var polygon;
var centerLat = 0;
var centerLong = 0;
var polygonPoints = [];
var latLng = [];
var radius;
var ck;
var directionsDisplay;
var directionsService;
var map;
var request;
var result;
var start;
var end;
var dr;
var countVia = 0;
$(document).ready(function () {
    var mapOptions = {
        center: new google.maps.LatLng(28.4700, 77.0300),
        zoom: 12,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };
    map = new google.maps.Map(document.getElementById('map_canvas'), mapOptions);
    initializeMap();

    $('#dvRadius').hide();
    $('#dvRoute').hide();
    $('#dvFinal').hide();
    $('input:radio[name="FenceType"]').click(function () {
        $('#dvFinal').hide();
        var type;
        if (this.id == 'rdbRoute') {
            var mapOptions = {
                center: new google.maps.LatLng(28.4700, 77.0300),
                zoom: 12,
                mapTypeId: google.maps.MapTypeId.ROADMAP
            };
            map = new google.maps.Map(document.getElementById('map_canvas'), mapOptions);
            initializeMap();
            type = 'POLYGON';
            $('#radius').val('0');
            $('#dvRadius').hide();
            $('#dvRoute').hide();
        }
        else if (this.id == 'rdbRoutePolygon') {
            type = 'POLYGON';
            $('#dvRoute').show();
            $('#dvRadius').hide();
        }
        else {
            type = 'CIRCLE';
            var mapOptions = {
                center: new google.maps.LatLng(28.4700, 77.0300),
                zoom: 12,
                mapTypeId: google.maps.MapTypeId.ROADMAP
            };
            map = new google.maps.Map(document.getElementById('map_canvas'), mapOptions);
            initializeMap();
            $('#radius').val('0');
            $('#dvRadius').show();
            $('#dvRoute').hide();
        }
        heuroDrawingManager.removeOverlay(shape);
        heuroDrawingManager.setDrawingMode(type);
        if (temp != undefined)
            temp.closeDiv();
    });


});

function initializeMap() {
    
    // directionsDisplay.setMap(map);
    heuroDrawingManager = new HeuroGeofence(map);
    heuroDrawingManager.initdrawingManager();
    heuroDrawingManager.enableListener(true);
    heuroDrawingManager.setDialogDiv($('#dialog-message'));
    if ($('#ddlFencetype > option').length == 0) {
        $('#ddlFencetype').append("<option value='SELECT'>Select</option>");
        $('#ddlFencetype').append("<option value='POLYGON'>Polygon</option>");
        $('#ddlFencetype').append("<option value='CIRCLE'>Circle</option>");
        // $('#ddlFencetype').append("<option value='RECTANGLE'>Rectangle</option>");
    }
    google.maps.event.addListener(heuroDrawingManager.getDrawingManager(), 'overlaycomplete', function (e) {
        $('#dvFinal').show();
        latLng = [];
        shape = e.overlay;

        if (e.type == "polygon") {
            polygon = shape.getPath().getArray();

            for (var i = 0; i < polygon.length; i++) {

                latLng.push({ Latitude: polygon[i].lat(), Longtude: polygon[i].lng() });
            }
            radius = '0';
            centerLat = '0';
            centerLong = '0';
            radius.slice(1, '.');
            $('#txtRadius').val(radius);
        }
        else {
            polygon = null;
            radius = parseInt(shape.getRadius());
            centerLat = shape.getCenter().lat();
            centerLong = shape.getCenter().lng();
            //radius.slice(1,radius.indexOf( "."));
            $('#radius').val(radius);
        }
        //  var latLng = [];
        $('#dvFinal').show();
        heuroDrawingManager.enableDrawingManager(false);
        google.maps.event.addListener(shape, 'rightclick', function () {

            var r = confirm("Do you want to delete!");
            if (r == true) {

                $('#radius').val('0');
                $('#FenceType').val('');
                $('input[id="rdbArea"]').prop('checked', false);
                $('input[id="rdbRoute"]').prop('checked', false);
                $('#dvRadius').hide();

                var mapOptions = {
                    center: new google.maps.LatLng(28.4700, 77.0300),
                    zoom: 12,
                    mapTypeId: google.maps.MapTypeId.ROADMAP
                };
                map = new google.maps.Map(document.getElementById('map_canvas'), mapOptions);
                initializeMap();

            }           
        });
    });
    //When your first group of radio buttons has changed

}
function DrawGeofence(type, radius, centerLat, centerLong, colorCode, polyPoint, order) {
    heuroDrawingManager.removeOverlay(shape);
    var mapOptions = {
        //center: new google.maps.LatLng(28.4700, 77.0300),
        zoom: 12,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };
    map = new google.maps.Map(document.getElementById('map_canvas'), mapOptions);
    if (type == "CIRCLE") {
        //var centerPoint = centerPoint.split(',');
        var radius = radius
        var options = {
            strokeColor: colorCode,
            strokeOpacity: 1.0,
            strokeWeight: 3,
            zoom: 16,
            center: new google.maps.LatLng(centerLat, centerLong),
            //radius: Math.sqrt(radius) * 100,
            radius: parseInt(radius),
            map: map
        };
        //options.map.center.latLng = centerLong;
        //options.map.center.lat = centerLat;
        options.map.zoom = 16;
        shapeObj = new google.maps.Circle(options);
        customShapeBounds = shapeObj.getBounds();
        map.setCenter(new google.maps.LatLng(centerLat, centerLong));

        r = shapeObj.getRadius();
        lt = shapeObj.getCenter().lat();
        lg = shapeObj.getCenter().lng();
        shape = shapeObj;
    }
    else if (type == "POLYGON") {

        customShapeBounds = new google.maps.LatLngBounds();
        var l = polyPoint.split(';');

        var polygonCoords = [];

        for (var i = 0; i < l.length - 1 ; i++) {

            var pointLatlng = l[i].split(',');
            polygonCoords.push(new google.maps.LatLng(pointLatlng[0], pointLatlng[1]));
            latLng.push({ Latitude: pointLatlng[0], Longtude: pointLatlng[1] });
            if (i != 0) {
                customShapeBounds.extend(polygonCoords[i]);
            }
        }
        var options = {
            paths: polygonCoords,
            strokeColor: colorCode,
            strokeOpacity: 1.0,
            strokeWeight: 3,
            zoom: 16,
            center: new google.maps.LatLng(polygonCoords[0], polygonCoords[1]),
            map: map
        };

        map.setCenter(new google.maps.LatLng(pointLatlng[0], pointLatlng[1]));
        options.map.center.D = polygonCoords[1];
        options.map.center.k = polygonCoords[0];
        options.map.zoom = 16;
        shapeObj = new google.maps.Polygon(options);
        polygonCoords = null;
        shape = shapeObj;
    }
    google.maps.event.addListener(shapeObj, 'rightclick', function () {

        var r = confirm("Do you want to delete!");
        if (r == true) {

            $('#radius').val('0');
            $('#radius').val('0');
            $('#FenceType').val('');
            $('input[id="rdbArea"]').prop('checked', false);
            $('input[id="rdbRoute"]').prop('checked', false);
            var mapOptions = {
                center: new google.maps.LatLng(28.4700, 77.0300),
                zoom: 12,
                mapTypeId: google.maps.MapTypeId.ROADMAP
            };
            map = new google.maps.Map(document.getElementById('map_canvas'), mapOptions);
            initializeMap();

        }
    });
}

function saveGeofence() {

    var stringObj = heuroDrawingManager.geofenceJSON($('#txtAreaObj').val(), heuroDrawingManager.getDrawingMode(), shape);
    insertShape(stringObj);
}
function updateGeofence() {
    var stringObj = heuroDrawingManager.geofenceJSON($('#txtAreaObj').val(), heuroDrawingManager.getCustomShape().GeofenceType, shape);
    editShape(stringObj);
}
function deleteGeofence() {
    var stringObj = heuroDrawingManager.geofenceJSON($('#txtAreaObj').val(), heuroDrawingManager.getCustomShape().GeofenceType, shape);
    deleteShape(stringObj);
}


/*%%%%%%%%%%%%%%%%%%%%%%%%%% Getdirection Created By Deepak %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%*/
function PlayPlotRoute(Trackoption) {

    itrate = true;
    var dateFrom = document.getElementById('datetimepickerFrom').value;
    var dateTo = document.getElementById('datetimepickerTo').value;

    var carId = document.getElementById('txtreg').value;

    if (dateFrom.length > 0 && dateTo.length > 0 && carId != '') {
        loading_div();
        FleetRoboApp.FleetRoboAutoFillService.PlotRoute(carId, dateFrom, dateTo, OnComplete, OnTimeOut, OnError);

        function OnComplete(arg) {

            VehTrackingData = arg;
            if (VehTrackingData.length == 0) {
                hide_div();
                alert('No Record Found');
                // Initialise();
                return false;
            }

            itrate = true;
            if (Trackoption == 'play') {
                mapFocusLatLon = new google.maps.LatLng(VehTrackingData[0].Lat, VehTrackingData[0].Lon);

                mapOptions = {
                    zoom: 16,
                    center: mapFocusLatLon,
                    mapTypeId: google.maps.MapTypeId.ROADMAP,
                    scrollwheel: false
                };
                Map = new google.maps.Map(document.getElementById("map_canvas"), mapOptions);
                HandleTrackButtonsVisibility(false, true, true, false, false, false);

                //Remove Trace Car Setting Object from the top
                VehTrackingData.pop();
                hide_div();
                vehTrackTimer = setTimeout('ItrateTrack()', 600);
            }

            else {
                $('.collapse').slideUp("fast");
                $('#flip').slideDown("fast");

                //var traceZoom = parseInt(VehTrackingData[VehTrackingData.length - 1].Lat)
                var traceZoom = 16;
                VehTrackingData.pop()
                var traceLatLon = new google.maps.LatLng(VehTrackingData[0].Lat, VehTrackingData[0].Lon);
                mapOptions = {
                    zoom: traceZoom,
                    center: traceLatLon,
                    mapTypeId: google.maps.MapTypeId.ROADMAP,
                    scrollwheel: false
                };

                Map = new google.maps.Map(document.getElementById("map-canvas"), mapOptions);

                //   HandleTrackButtonsVisibility(true, false, false, false, true, true);


                PlotRoute();
                hide_div();
            }

        }

        function OnTimeOut(arg) {

        }

        function OnError(arg) {

        }

    }
    else {
        alert('Please Insert Valid date & time and valid Registration No.');
    }
}
/*%%%%%%%%%%%%%%%%%%%%%%%%%%  Created By Deepak %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%*/
function PlotRoute() {
    itrate = false;
    // clearTimeout(trackPlayTimeOut);
    var marker;
    var content = null;
    var infowindow;
    var r = [];
    var vehpath = [];
    for (var i = 0; i < VehTrackingData.length; i++) {
        if (i > 0) {
            var lat = parseFloat(VehTrackingData[i].Lat);
            var lng = parseFloat(VehTrackingData[i].Lon);
            vehpath.push(new google.maps.LatLng(lat, lng));
        }
    }
    DrawPolyLine(vehpath)
}
/*%%%%%%%%%%%%%%%%%%%%%%%%%%  Created By Deepak %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%*/
function DrawPolyGon(r) {
    fonas = new google.maps.Polygon({
        paths: r,
        strokeColor: "#FF0000",
        strokeOpacity: 0.8,
        strokeWeight: 2,
        fillColor: "#FF0000",
        fillOpacity: 0.35
    });

    fonas.setMap(Map);
    google.maps.event.addListener(fonas, 'rightclick', function (e) {

        saveGeofence('POLYGON', fonas);
        ShowModalPopup();
    });
}

function ShowModalPopup() {
    $find("mpe").show();

    return false;
}
/*%%%%%%%%%%%%%%%%%%%%%%%%%%  Created By Deepak %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%*/
function DrawPolyLine(r) {
    var kelias = new google.maps.Polyline({
        path: r,
        strokeColor: "#FFFFF",
        strokeOpacity: 1.0,
        strokeWeight: 4,
        icon: ""
    });
    kelias.setMap(Map);
    var z = 10 / 10000;
    if (document.getElementById('txtdisroute').value.length > 0) {

        z = document.getElementById('txtdisroute').value / 10000;
    }
    var poly = [];
    var way2 = r;

    for (i in r) {
        poly.push(new google.maps.LatLng(r[i].lat() + z, r[i].lng() - z));

    }
    r.reverse();
    for (x in r) {
        poly.push(new google.maps.LatLng(r[x].lat() - z, r[x].lng() + z));
    }

    DrawPolyGon(poly)
}



/*%%%%%%%%%%%%%%%%%%%%%%%%%%  Created By Rashmi %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%*/
function DrawUserPathPolyLine(r) {
    var kelias = new google.maps.Polyline({
        path: r,
        strokeColor: "#FFFFF",
        strokeOpacity: 1.0,
        strokeWeight: 4,
        icon: ""
    });
    kelias.setMap(map);
    var z = 10 / 10000;
    if (document.getElementById('txtdisroute').value.length > 0) {

        z = document.getElementById('txtdisroute').value / 10000;
    }
    var poly = [];
    var way2 = r;

    for (i in r) {
        poly.push(new google.maps.LatLng(r[i].lat() + z, r[i].lng() - z));

    }
    r.reverse();
    for (x in r) {
        poly.push(new google.maps.LatLng(r[x].lat() - z, r[x].lng() + z));
    }

    DrawUserPathPolyGon(poly)
}

/*%%%%%%%%%%%%%%%%%%%%%%%%%%  Created By Deepak %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%*/
function DrawUserPathPolyGon(r) {
    fonas = new google.maps.Polygon({
        paths: r,
        strokeColor: "#FF0000",
        strokeOpacity: 0.8,
        strokeWeight: 2,
        fillColor: "#FF0000",
        fillOpacity: 0.35
    });

    fonas.setMap(map);
    google.maps.event.addListener(fonas, 'rightclick', function (e) {
        saveGeofence('POLYGON', fonas);
        ShowModalPopup();
    });
}

function ShowModalPopup() {
    $find("mpe").show();

    return false;
}

function loading_div() {

    $("#lightbox, #lightbox-panel").fadeIn(200);
};

function hide_div() {

    $("#lightbox, #lightbox-panel").fadeOut(200);
}
function savePathGeofence() {
    var stringObj = heuroDrawingManager.geofenceJSON($('#txtfencename').val(), 'POLYGON', fonas);
    var _hdnfinalval = document.getElementById("<%=hdnLatLon.ClientID%>");
    _hdnfinalval.value = JSON.stringify(stringObj);

}
function selectDefault() {
    $('#ddGeometryType').val("SELECT");
    $('#ddGeometryType').change();
    if (heuroDrawingManager != undefined)
        heuroDrawingManager.enableDrawingManager(false);
}

function selectShape() {
    $.ajax({
        type: "POST",
        url: "GeofenceBeta.aspx/selectRoute",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            allPoints = msg.d;
            $('#InformationDiv').html('');
            for (var i = 0; i < msg.d.length; i++) {
                if (i % 2 == 0)
                    $('#InformationDiv').append("<div class='even'>" + msg.d[i].GeofenceName + "</div>");
                else
                    $('#InformationDiv').append("<div class='odd'>" + msg.d[i].GeofenceName + "</div>");
            }
        },
        error: function (error) {
            console.log('Error: ' + error.responseText);
        }
    });
}
function initAutocomplete() {

    var map = new google.maps.Map(document.getElementById('map_canvas'), {
        center: { lat: -33.8688, lng: 151.2195 },
        zoom: 13,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    });

    // Create the search box and link it to the UI element.
    var input = document.getElementById('pac-input');
    var searchBox = new google.maps.places.SearchBox(input);
    map.controls[google.maps.ControlPosition.TOP_LEFT].push(input);

    // Bias the SearchBox results towards current map's viewport.
    map.addListener('bounds_changed', function () {
        searchBox.setBounds(map.getBounds());
    });

    var markers = [];
    // Listen for the event fired when the user selects a prediction and retrieve
    // more details for that place.
    searchBox.addListener('places_changed', function () {
        var places = searchBox.getPlaces();

        if (places.length == 0) {
            return;
        }

        // Clear out the old markers.
        markers.forEach(function (marker) {
            marker.setMap(null);
        });
        markers = [];

        // For each place, get the icon, name and location.
        var bounds = new google.maps.LatLngBounds();
        places.forEach(function (place) {
            var icon = {
                url: place.icon,
                size: new google.maps.Size(71, 71),
                origin: new google.maps.Point(0, 0),
                anchor: new google.maps.Point(17, 34),
                scaledSize: new google.maps.Size(25, 25)
            };

            // Create a marker for each place.
            markers.push(new google.maps.Marker({
                map: map,
                icon: icon,
                title: place.name,
                position: place.geometry.location
            }));

            if (place.geometry.viewport) {
                // Only geocodes have viewport.
                bounds.union(place.geometry.viewport);
            } else {
                bounds.extend(place.geometry.location);
            }
        });
        map.fitBounds(bounds);
    });
}

function insertShape(obj) {
    var parameter = { ValueObj: obj };
    $.ajax({
        type: "POST",
        url: "GeofenceBeta.aspx/insertRoute",
        data: JSON.stringify({ ValueObj: JSON.stringify(obj) }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            if (msg.d == 1) {
                temp.hideDiv();
                selectShape();
                heuroDrawingManager.removeOverlay(shape);
                selectDefault();
            }
        },
        error: function (error) {
            console.log('Error: ' + error.responseText);
        }
    });
}

function editShape(obj) {
    var parameter = { ValueObj: obj };
    $.ajax({
        type: "POST",
        url: "GeofenceBeta.aspx/updateGeofence",
        data: JSON.stringify({ ValueObj: JSON.stringify(obj), oldname: heuroDrawingManager.getCustomShape().GeofenceName }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            if (msg.d == 1) {
                temp.hideDiv();
                selectShape();
                heuroDrawingManager.removeOverlay(shape);
                selectDefault();
            }
        },
        error: function (error) {
            console.log('Error: ' + error.responseText);
        }
    });
}
function deleteShape(obj) {
    var parameter = { ValueObj: obj };
    $.ajax({
        type: "POST",
        url: "GeofenceBeta.aspx/deleteGeofence",
        data: JSON.stringify({ ValueObj: JSON.stringify(obj) }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            if (msg.d == 1) {
                temp.hideDiv();
                selectShape();
                heuroDrawingManager.removeOverlay(shape);
                selectDefault();
            }
        },
        error: function (error) {
            console.log('Error: ' + error.responseText);
        }
    });
}
