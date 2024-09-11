

var map, heuroDrawingManager, shape, temp, drawingManager;
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
var selectedShape;
var DrawnShape;
var markers = [];
var MaxGeoRadius;
var ErrorMsg;
$(document).ready(function () {

    var mapOptions = {
        center: new google.maps.LatLng(28.4700, 77.0300),
        zoom: 14,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };

    map = new google.maps.Map(document.getElementById('map_canvas'), mapOptions);

    $('#dvRadius').hide();
    $('#dvRoute').hide();
    $('#dvFinal').hide();

    $('#chkFenceType').on('change', function () {
        if ($('#chkFenceType').is(":checked")) {
            $('#radius').val('0');
            $('#dvRoute').show();
            $('#LocationDiv').hide();
            $('#txtAutocomplete').val('');
            clearMarkers();
            deleteSelectedShape(true);
        }
        else if (!$('#chkFenceType').is(":checked")) {

            $('#address1').val('');
            $('#address2').val('');
            Start_Lat = '';
            End_Lat = '';
            Start_Lon = '';
            End_Lon = '';

            $('#LocationDiv').show();
            $('#dvRoute').hide();
        }
    });
});

//To Load a Blank MAP
function ResetMap() {
    var mapOptions = {
        center: new google.maps.LatLng(28.4700, 77.0300),
        zoom: 14,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };

    map = new google.maps.Map(document.getElementById('map_canvas'), mapOptions);

    $('#dvRadius').hide();
    $('#dvRoute').hide();
    $('#dvFinal').hide();
}



//Geofence radius will be dynamicaly changed when radius is changed in radius text box with MAX_VALUE '1000' .
function SetRadius(Exceeded) {
    if (Exceeded) {
        $('#radius').val(MaxGeoRadius);
        shape.setRadius(MaxGeoRadius);
    }
    else {
        var rad = parseInt($('#radius').val());
        shape.setRadius(rad);
    }
}

//Added by Tarique 21 FEB :: Adds marker at given Lat-Long
function AddMarker(Lat, Long) {
    if ((Lat != '' || Lat != undefined) && (Long != '' || Long != undefined)) {
        var myLatlng = new google.maps.LatLng(Lat, Long);
        var marker = new google.maps.Marker({
            position: myLatlng,
        });
    }
    if (marker != null) {
        marker.setMap(map);
    }
}

//First three methods below are used to remove a drawn geofence when either user deletes the geofence by clicking right or or searches a new location to draw a new geofence
function clearSelection() {
    if (selectedShape) {
        selectedShape.setEditable(false);
        selectedShape = null;
    }
}

function setSelection(shape) {
    clearSelection();
    selectedShape = shape;
    shape.setEditable(true);
}

function deleteSelectedShape(NewSearchedLocation) {
    if (NewSearchedLocation == true) {
        if (DrawnShape) {
            DrawnShape.setMap(null);
        }
        DrawnShape = false;
    }
    else {
        if (selectedShape) {
            selectedShape.setMap(null);
        }
    }
}

////%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% CODE TO INITIALIZE MAP FOR FIRST TIME TO ADD NEW GEOFENCE %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%

function initializeMapCustom() {
    //debugger
    var shapes = [];

    drawingManager = new google.maps.drawing.DrawingManager({
        drawingControl: true,
        drawingControlOptions: {
            position: google.maps.ControlPosition.TOP_CENTER,
            drawingModes: [google.maps.drawing.OverlayType.POLYGON, google.maps.drawing.OverlayType.CIRCLE]
        },
        polygonOptions: {
            editable: true,
            draggable: true
        },
        circleOptions: {
            editable: true,
            draggable: true
        }
    });
    drawingManager.setMap(map);


    // FIRES WHEN USER CHANGES THE DRAWING TOOL FROM TOP OF THE MAP
    google.maps.event.addListener(drawingManager, "drawingmode_changed", function () {
        //debugger
        $('#radius').val('');

        $('#dvFinal').hide();

        if (drawingManager.getDrawingMode() != null) {

            $('#hdnType').val(drawingManager.getDrawingMode().toUpperCase());

            if (drawingManager.getDrawingMode().toUpperCase() == 'CIRCLE') {
                //debugger
                $('#radius').val('');
                $('#dvRadius').show();
                IsGeoTypeCircle = true;
            }
            else { $('#dvRadius').hide(); IsGeoTypeCircle = false; }

            for (var i = 0; i < shapes.length; i++) {
                shapes[i].setMap(null);
            }
            shapes = [];
        }
    });

    // Add a listener for creating new shape event. FIRES WHEN DRAWING IS COMPLETED
    google.maps.event.addListener(drawingManager, "overlaycomplete", function (e) {

        if (drawingManager.getDrawingMode()) {
            drawingManager.setDrawingMode(null);
        }

        if (e.type.toUpperCase() == 'CIRCLE') {
            $('#radius').val('');
            $('#radius').val(Math.round(e.overlay.radius));
            if (e.overlay.radius > MaxGeoRadius) {

                $('#radius').val(MaxGeoRadius);
                e.overlay.setRadius(MaxGeoRadius);
                alert(ErrorMsg);
            }
        }

        $('#dvFinal').show();

        latLng = [];
        shape = e.overlay;
        DrawnShape = shape;

        if (e.type == "polygon") {

            polygon = shape.getPath().getArray();

            for (var i = 0; i < polygon.length; i++) {
                //debugger
                latLng.push({ Latitude: polygon[i].lat(), Longtude: polygon[i].lng() });
            }
            radius = '0';
            centerLat = '0';
            centerLong = '0';
            radius.slice(1, '.');
            $('#txtRadius').val(radius);
        }

        else {

            if (shape.radius > MaxGeoRadius) {

                $('#radius').val(MaxGeoRadius);
                shape.setRadius(MaxGeoRadius);
                //shape.radius = 1000;
                alert(ErrorMsg);
            }
            polygon = null;

            radius = parseInt(shape.getRadius());
            centerLat = shape.getCenter().lat();
            centerLong = shape.getCenter().lng();
            $('#radius').val(radius);
        }

        //Fires when shape is dragged on map
        google.maps.event.addListener(shape, 'dragend', function (evt) {
            //debugger
            if (shape.radius > MaxGeoRadius) {

                $('#radius').val(MaxGeoRadius);
                shape.setRadius(MaxGeoRadius);
                alert(ErrorMsg);
            }
            centerLat = evt.latLng.lat();
            centerLong = evt.latLng.lng();
        });

        //Fires when 'right clicked' on shape
        google.maps.event.addListener(shape, 'rightclick', function (e) {

            var r = confirm("Do you want to delete!");
            if (r == true) {
                var newShape = shape;
                setSelection(newShape);
                $('#radius').val('');
                deleteSelectedShape(false);
            }
        });

        //Fires when Radius of CIRCLE is changed
        google.maps.event.addListener(shape, 'radius_changed', function () {
            //init's event

            $('#radius').val(Math.round(shape.radius));
            if (shape.radius > MaxGeoRadius) {

                $('#radius').val(MaxGeoRadius);
                shape.setRadius(MaxGeoRadius);
                alert(ErrorMsg);
            }
        });

        //Fires when CENTER of the CIRCLE is changed
        //google.maps.event.addListener(shape, 'center_changed', function () {  
        //  alert('center_changed');
        //    $('#newLat').val(shape.map.center.lat());
        //    $('#newLong').val(shape.map.center.lng());
        //});

        var newShape = e.overlay;
        newShape.type = e.type;
        shapes.push(newShape);
        $('#dvFinal').show();
    });

    ////%%%%%%%%%%%%%%%%%%%%%%%%%% Code to show geofence drawing tool ends here %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
}

////%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% CODE TO SHOW DRAWN GEOFENCE FETCHED FROM DataBase. Values Reset To Update OLD Geofence %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%

function DrawHubEdit(type, radius, centerLat, centerLong, colorCode, polyPoint, order, IsView) {

    //IsView is TRUE if user clicked on 'Edit' button [Not Updatable], and FALSE if user clicked on 'View' button [Updatable]

    if (IsView == false) {
        drawingManager.setDrawingMode(null);
        drawingManager.drawingControl = false;
    }
    else {
        drawingManager.drawingControl = true;
    }

    if (type == "ROUTE_GEOFENCE_POLYGON") {
        $('#chkFenceType').trigger('click');
    }

    var mapOptions = {
        zoom: 15,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };

    map = new google.maps.Map(document.getElementById('map_canvas'), mapOptions);
    if (type == "CIRCLE") {
        var radius = radius
        var options = {
            strokeColor: colorCode,
            strokeOpacity: 1.0,
            strokeWeight: 3,
            zoom: 16,
            center: new google.maps.LatLng(centerLat, centerLong),
            radius: parseInt(radius),
            draggable: IsView,
            editable: IsView,
            map: map
        };
        options.map.zoom = 16;
        shapeObj = new google.maps.Circle(options);
        customShapeBounds = shapeObj.getBounds();
        map.setCenter(new google.maps.LatLng(centerLat, centerLong));

        r = shapeObj.getRadius();
        lt = shapeObj.getCenter().lat();
        lg = shapeObj.getCenter().lng();
        shape = shapeObj;
    }

    else if (type == "POLYGON" || type == "ROUTE_GEOFENCE_POLYGON") {

        customShapeBounds = new google.maps.LatLngBounds();
        var l = polyPoint.split(';');
        var polygonCoords = [];

        for (var i = 0; i < l.length - 1; i++) {
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
            draggable: IsView,
            editable: IsView,
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

    if (type == "POLYGON") {
        google.maps.event.addListener(shapeObj.getPath(), 'set_at', markerCoords);
    }

    //google.maps.event.addListener(shapeObj.getPath(), 'insert_at', markerCoords);
    //google.maps.event.addListener(shapeObj.getPath(), 'remove_at', markerCoords);



    google.maps.event.addListener(shapeObj, 'rightclick', function (e) {
        // Drawfence        
        if (IsView == true) {
            var r = confirm("Do you want to delete!");
            if (r == true) {
                var newShape = shapeObj;
                setSelection(newShape);
                $('#radius').val('');
                deleteSelectedShape(false);
            }


            //var mapOptions = {
            //    center: new google.maps.LatLng(shapeObj.map.center.lat(), shapeObj.map.center.lng()),
            //    zoom: 12,
            //    mapTypeId: google.maps.MapTypeId.ROADMAP
            //};
            //map = new google.maps.Map(document.getElementById('map_canvas'), mapOptions);

            //drawingManager = new google.maps.drawing.DrawingManager({
            //    drawingControl: true,
            //    drawingControlOptions: {
            //        position: google.maps.ControlPosition.TOP_CENTER,
            //        drawingModes: [google.maps.drawing.OverlayType.POLYGON, google.maps.drawing.OverlayType.CIRCLE]
            //    },
            //    polygonOptions: {
            //        editable: true,
            //        draggable: true
            //    },
            //    circleOptions: {
            //        editable: true,
            //        draggable: true
            //    }
            //});

            //drawingManager.setDrawingMode(null);
            //drawingManager.setMap(map);
        }
    });

    //Fires when shape is dragged on map
    google.maps.event.addListener(shapeObj, 'dragend', function (evt) {

        if (type == "CIRCLE") {
            if (shapeObj.radius > MaxGeoRadius) {

                $('#radius').val(MaxGeoRadius);
                shapeObj.setRadius(MaxGeoRadius);
                alert(ErrorMsg);
            }
            $('#CenterLat').val(shapeObj.getCenter().lat());
            $('#CenterLong').val(shapeObj.getCenter().lng());
        }

        else if (type == "POLYGON") {
            latLng = [];
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
    });

    //Fires when Radius of CIRCLE is changed
    google.maps.event.addListener(shapeObj, 'radius_changed', function () {

        $('#radius').val(Math.round(shapeObj.radius));
        if (shapeObj.radius > MaxGeoRadius) {

            $('#radius').val(MaxGeoRadius);
            shapeObj.setRadius(MaxGeoRadius);
            alert(ErrorMsg);
        }
    });

    google.maps.event.addListener(drawingManager, "drawingmode_changed", function () {

        $('#radius').val('0');
        var newShape = shapeObj;
        setSelection(newShape);
        if (drawingManager.drawingMode != null) {
            deleteSelectedShape(false);
        }
    });
    //drawingManager.drawingMode = null;
    drawingManager.setMap(map);
}

////%%%%%%%%%%%%%%%%%%%%%%%%%% CODE TO UPDATE OLD POLYGON ON MAP SO THAT POLYGON CAN BE UPDATED WITH NEW Lat-Longs and new Points %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
function markerCoords() {

    latLng = [];
    polygon = [];
    polygon = shapeObj.getPath().getArray();
    for (var i = 0; i < polygon.length; i++) {

        latLng.push({ Latitude: polygon[i].lat(), Longtude: polygon[i].lng() });
    }
    radius = '0';
    centerLat = '0';
    centerLong = '0';
    radius.slice(1, '.');
    $('#txtRadius').val(radius);
}


//Under Development Till: "15 MAR". CALLED IN CASE OF ROUTE GEOFENCE
function RouteDrawn(mapObj) {

    var shapes = [];
    drawingManager = new google.maps.drawing.DrawingManager({
        drawingControl: true,
        drawingControlOptions: {
            position: google.maps.ControlPosition.TOP_CENTER,
            drawingModes: [google.maps.drawing.OverlayType.POLYGON]
        },
        polygonOptions: {
            editable: true,
            draggable: true
        },
    });
    drawingManager.setMap(mapObj);

    // FIRES WHEN USER CHANGES THE DRAWING TOOL FROM TOP OF THE MAP
    google.maps.event.addListener(drawingManager, "drawingmode_changed", function () {

        $('#radius').val('');

        $('#dvFinal').hide();

        if (drawingManager.getDrawingMode() != null) {

            $('#hdnType').val(drawingManager.getDrawingMode().toUpperCase());

            if (drawingManager.getDrawingMode().toUpperCase() == 'CIRCLE') {
                $('#radius').val('');
                $('#dvRadius').show();
            }
            else { $('#dvRadius').hide(); }

            for (var i = 0; i < shapes.length; i++) {
                shapes[i].setMap(null);
            }
            shapes = [];
        }
    });

    // Add a listener for creating new shape event. FIRES WHEN DRAWING IS COMPLETED
    google.maps.event.addListener(drawingManager, "overlaycomplete", function (e) {

        if (drawingManager.getDrawingMode()) {
            drawingManager.setDrawingMode(null);
        }

        if (e.type.toUpperCase() == 'CIRCLE') {
            $('#radius').val('');
            $('#radius').val(Math.round(e.overlay.radius));
            if (e.overlay.radius > MaxGeoRadius) {

                $('#radius').val(MaxGeoRadius);
                e.overlay.setRadius(MaxGeoRadius);
                alert(ErrorMsg);
            }
        }

        $('#dvFinal').show();
        latLng = [];
        shape = e.overlay;
        DrawnShape = shape;

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
            if (shape.radius > MaxGeoRadius) {

                $('#radius').val(MaxGeoRadius);
                shape.setRadius(MaxGeoRadius);
                alert(ErrorMsg);
            }
            polygon = null;
            radius = parseInt(shape.getRadius());
            centerLat = shape.getCenter().lat();
            centerLong = shape.getCenter().lng();
            $('#radius').val(radius);
        }

        //Fires when shape is dragged on map
        google.maps.event.addListener(shape, 'dragend', function (evt) {

            if (shape.radius > MaxGeoRadius) {
                $('#radius').val(MaxGeoRadius);
                shape.setRadius(MaxGeoRadius);
                alert(ErrorMsg);
            }
            centerLat = evt.latLng.lat();
            centerLong = evt.latLng.lng();
        });

        //Fires when 'right clicked' on shape
        google.maps.event.addListener(shape, 'rightclick', function (e) {

            var r = confirm("Do you want to delete!");
            if (r == true) {
                var newShape = shape;
                setSelection(newShape);
                $('#radius').val('');
                deleteSelectedShape(false);
            }
        });

        //Fires when Radius of CIRCLE is changed
        google.maps.event.addListener(shape, 'radius_changed', function () {

            $('#radius').val(Math.round(shape.radius));
            if (shape.radius > MaxGeoRadius) {
                $('#radius').val(MaxGeoRadius);
                shape.setRadius(MaxGeoRadius);
                alert(ErrorMsg);
            }
        });

        //Fires when CENTER of the CIRCLE is changed
        //google.maps.event.addListener(shape, 'center_changed', function () {  
        //  alert('center_changed');
        //    $('#newLat').val(shape.map.center.lat());
        //    $('#newLong').val(shape.map.center.lng());
        //});

        var newShape = e.overlay;
        newShape.type = e.type;
        shapes.push(newShape);
        $('#dvFinal').show();
    });

}
