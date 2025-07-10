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
var centerControlDiv = document.createElement('div');
var image = {
   url: '/Images/' + 'GeofenceshowIcon.png',
  //  url: 'http://maps.google.com/mapfiles/ms/icons/black-dot.png' // Custom icon
    scaledSize: new google.maps.Size(25, 25), // scaled icon size :: Added By Sarfaraz on   //32, 30
};
var GoogleMapFor='VFLEET';
var GoogleMapLat=GoogleMapFor=='TG'?28.23:5.451060;
var GoogleMapLong=GoogleMapFor=='TG'?78.25:100.305520;

function initMapForGeofence(MapControlID, Zoom, CenterLat, CenterLng) {

    try {
        var mapOptions = {
            center: new google.maps.LatLng(5.451060, 100.305520),
            zoom: 14,
            mapTypeId: google.maps.MapTypeId.ROADMAP,
            fullscreenControl: false
        };


        map = new google.maps.Map(document.getElementById(MapControlID), mapOptions);


       // trafficLayer = new google.maps.TrafficLayer();
        //google.maps.event.addDomListener(document.getElementById('goCenterUI'), 'click', toggleTraffic);



        // Create the DIV to hold the control and call the CenterControl()
        // constructor
        // passing in this DIV.
        var centerControlDiv = document.createElement('div');
        var centerControl = new CreateFullScreenControl(MapControlID, centerControlDiv, map, { lat: CenterLat, lng: CenterLng });

        centerControlDiv.index = 1;
        centerControlDiv.style['padding'] = '10px 10px 0 0';
        map.controls[google.maps.ControlPosition.RIGHT_TOP].push(centerControlDiv);


        //google.maps.event.addListenerOnce(map, 'idle', function () {
        //    google.maps.event.trigger(map, 'resize');
        //});

        //google.maps.event.addListener(map, 'zoom_changed', function () {
        //    if ((chkZommChange == true && (_zoomDefault.indexOf(map.zoom) != -1))) {
        //        if (lastzoom == 0) {
        //            chkZoom = false;
        //        }
        //        else {
        //            chkZoom = true;
        //        }
        //    }
        //    else {
        //        chkZoom = true;
        //    }
        //})
    }
    catch (Error) { }

}

$(document).ready(function () {
   
    
    initMapForGeofence('map_canvas', 14, GoogleMapLat, GoogleMapLong);

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
    initMapForGeofence('map_canvas', 3, GoogleMapLat, GoogleMapLong);
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
                alert('Radius should not be greater than ' + MaxGeoRadius + ' Mtr. ');
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
                //shape.radius = 1000;
                alert('Radius should not be greater than ' + MaxGeoRadius + ' Mtr. ');
            }
            polygon = null;

            radius = parseInt(shape.getRadius());
            centerLat = shape.getCenter().lat();
            centerLong = shape.getCenter().lng();
            $('#radius').val(radius);
        }

        //Fires when shape is dragged on map
        google.maps.event.addListener(shape, 'dragend', function (evt) {
           // debugger
            if (shape.radius > MaxGeoRadius) {

                $('#radius').val(MaxGeoRadius);
                shape.setRadius(MaxGeoRadius);
                alert('Radius should not be greater than ' + MaxGeoRadius + ' Mtr. ');
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
                alert('Radius should not be greater than ' + MaxGeoRadius + ' Mtr. ');
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

function DrawGeofenceEdit(type, radius, centerLat, centerLong, colorCode, polyPoint, order, IsView) {

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

  
     initMapForGeofence('map_canvas', 14, GoogleMapLat, GoogleMapLong);

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
        //debugger
        if (type == "CIRCLE") {
            if (shapeObj.radius > MaxGeoRadius) {

                $('#radius').val(MaxGeoRadius);
                shapeObj.setRadius(MaxGeoRadius);
                alert('Radius should not be greater than ' + MaxGeoRadius + ' Mtr. ');
            }
            $('#Lat').val(shapeObj.getCenter().lat());
            $('#Long').val(shapeObj.getCenter().lng());
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
            alert('Radius should not be greater than ' + MaxGeoRadius + ' Mtr. ');
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
                alert('Radius should not be greater than ' + MaxGeoRadius + ' Mtr. ');
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
                alert('Radius should not be greater than ' + MaxGeoRadius + ' Mtr. ');
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
                alert('Radius should not be greater than ' + MaxGeoRadius + ' Mtr. ');
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
                alert('Radius should not be greater than ' + MaxGeoRadius + ' Mtr. ');
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

function initializeMapCustomWithPOIIcon() {

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
                alert('Radius should not be greater than ' + MaxGeoRadius + ' Mtr. ');
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
                //shape.radius = 1000;
                alert('Radius should not be greater than ' + MaxGeoRadius + ' Mtr. ');
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
                alert('Radius should not be greater than ' + MaxGeoRadius + ' Mtr. ');
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
                alert('Radius should not be greater than ' + MaxGeoRadius + ' Mtr. ');
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
        DrawPOIOverGeofence();
    });

    ////%%%%%%%%%%%%%%%%%%%%%%%%%% Code to show geofence drawing tool ends here %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
}
function DrawGeofence(type, radius, centerLat, centerLong, colorCode, polyPoint, order, name,IsdeleteGeofence) {
  ////  debugger
    var latLng = [];
    var heuroDrawingManager, shape, temp;
    if (IsdeleteGeofence)
    {
        if (CircleArray.length > 0)
        {
            for (var i = 0; i < CircleArray.length; i++)
            {
                CircleArray[i].setMap(null);
            }
        }
       
        if (PolygonArray.length > 0)
        {
            for (var i = 0; i < PolygonArray.length; i++) {
                PolygonArray[i].setMap(null);
            }
        }   

    }
    else {

        if (type == "CIRCLE") {

            var options = {
                strokeColor: colorCode,
                strokeOpacity: 1.0,
                strokeWeight: 3,
                zoom: 16,
                center: new google.maps.LatLng(centerLat, centerLong),
                radius: parseInt(radius),
                map: _map
            };

        geoshapeCirle = new google.maps.Circle(options);
        customShapeBounds = geoshapeCirle.getBounds();
        CircleArray.push(geoshapeCirle);
        //CircleArray.push({
        //    CircularShape: geoshapeCirle
        //});

        loc = new google.maps.LatLng(centerLat, centerLong);
        bounds.extend(loc);
        }

    else if (type == "POLYGON") {
        
        if (polyPoint != null) {
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

                loc = new google.maps.LatLng(pointLatlng[0], pointLatlng[1]);
                bounds.extend(loc);
            }

            var options = {
                paths: polygonCoords,
                strokeColor: colorCode,
                strokeOpacity: 1.0,
                strokeWeight: 3,
                zoom: 16,
                center: new google.maps.LatLng(polygonCoords[0], polygonCoords[1]),
                map: _map,
                icon: {
                    strokeColor: '#FFD700',
                    strokeOpacity: 1.0,
                    strokeWeight: 2,
                    fillColor: '#FFD700',
                    fillOpacity: 1.0,
                    path: google.maps.SymbolPath.CIRCLE,
                    scale: 20,
                    anchor: new google.maps.Point(0, 0)
                }
            };
            geoshapePolygon = new google.maps.Polygon(options);
            PolygonArray.push(geoshapePolygon);

            //PolygonArray.push({
            //    PolygonalShape: geoshapePolygon
            //});
        }
        polygonCoords = null;
              }


    var marker = new google.maps.Marker({
        map: _map
    });

    var infowindow = new google.maps.InfoWindow();

    if (type == "CIRCLE")
    {
        google.maps.event.addListener(geoshapeCirle, 'mouseover', function () {
            if (type == "CIRCLE") {
                marker.setPosition(this.getCenter()); // get circle's center
            }
            else {
                marker.setPosition(new google.maps.LatLng(pointLatlng[0], pointLatlng[1]));
            }
            infowindow.setContent("<b>" + name + "</b>"); // set content
            infowindow.open(_map, marker); // open at marker's location
            marker.setVisible(false); // hide the marker
        });

        google.maps.event.addListener(geoshapeCirle, 'mouseout', function () {
            infowindow.close();
        });
    }

    else if (type == "POLYGON") {

        google.maps.event.addListener(geoshapePolygon, 'mouseover', function () {

            if (type == "CIRCLE") {
                marker.setPosition(this.getCenter()); // get circle's center
            }
            else {
                marker.setPosition(new google.maps.LatLng(pointLatlng[0], pointLatlng[1]));
            }
            infowindow.setContent("<b>" + name + "</b>"); // set content
            infowindow.open(_map, marker); // open at marker's location
            marker.setVisible(false); // hide the marker

        });

        google.maps.event.addListener(geoshapePolygon, 'mouseout', function () {
            infowindow.close();
        });
    }
}
}


function DrawGeofenceForVehicleDashboard(type, radius, centerLat, centerLong, colorCode, polyPoint, order, name, IsdeleteGeofence) {
    // debugger
    var labelContentText = _map.getZoom() > 15 ? '<div class="my-label-class" style="display:block;">' + name + '</div>' : '<div class="my-label-class" style="display:none;">' + name + '</div>';
    var latLng = [];
    var heuroDrawingManager, shape, temp;
    if (IsdeleteGeofence) {
        if (CircleArray.length > 0) {
            for (var i = 0; i < CircleArray.length; i++) {
                CircleArray[i].setMap(null);
            }
        }

        if (PolygonArray.length > 0) {
            for (var i = 0; i < PolygonArray.length; i++) {
                PolygonArray[i].setMap(null);
            }
        }

    }
    else {

        if (type == "CIRCLE") {

            var options = {
                title: name,
                strokeColor: colorCode,
                strokeOpacity: 1.0,
                strokeWeight: 3,
                zoom: 16,
                center: new google.maps.LatLng(centerLat, centerLong),
                radius: parseInt(radius),
                fillColor:colorCode,   //"#FF0000", // Fill color (red in this example)
                fillOpacity: 0.25,
                map: _map
            };

            geoshapeCirle = new google.maps.Circle(options);
            customShapeBounds = geoshapeCirle.getBounds();
            CircleArray.push(geoshapeCirle);
           
            var CircleM =
              // new google.maps.Marker({
                    new MarkerWithLabel({
                        map: _map,
                   position: new google.maps.LatLng(centerLat, centerLong),
                   labelContent: labelContentText, // Your label content with a custom class
                   labelAnchor: new google.maps.Point(20,20), // Adjust the anchor position as needed
                   icon: {
                       strokeColor: '#FFFFFF',
                       strokeOpacity: 1.0,
                       strokeWeight: 2,
                       fillColor:colorCode, //'#000000',  //   //#FFD700
                       fillOpacity: 1.0,
                       path: google.maps.SymbolPath.CIRCLE,
                       scale: 5,
                       labelOrigin: new google.maps.Point(0,3),
                   },
                   // zIndex: Math.round(centerLat * -100000) << 5

               });


            //var infowindow = new google.maps.InfoWindow();

            //google.maps.event.addListener(geoshapeCirle, 'mouseover', function () {
            //    //if (type == "CIRCLE") {
            //    //    marker.setPosition(this.getCenter()); // get circle's center
            //    //}
            //    //else {
            //    //    marker.setPosition(new google.maps.LatLng(pointLatlng[0], pointLatlng[1]));
            //    //}
            //    infowindow.setContent("<b>" + name + "</b>"); // set content
            //    infowindow.open(_map, geoshapeCirle); // open at marker's location
            //   // marker.setVisible(false); // hide the marker
            //});

            //google.maps.event.addListener(geoshapeCirle, 'mouseout', function () {
            //    infowindow.close();
            //});
            
            CircleArray.push(CircleM);

            loc = new google.maps.LatLng(centerLat, centerLong);
            bounds.extend(loc);
            
        }
        
        else if (type == "POLYGON") {

            if (polyPoint != null) {
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

                    loc = new google.maps.LatLng(pointLatlng[0], pointLatlng[1]);
                    bounds.extend(loc);
                }

                var options = {
                    paths: polygonCoords,
                    strokeColor: colorCode,
                    strokeOpacity: 1.0,
                    strokeWeight: 3,
                    zoom: 16,
                    center: new google.maps.LatLng(polygonCoords[0], polygonCoords[1]),
                    fillColor: colorCode, //'#FFD700',
                    fillOpacity: 0.25,
                    map: _map,
                    icon: {
                        strokeColor:colorCode, //'#FFD700',
                        strokeOpacity: 1.0,
                        strokeWeight: 2,
                        fillColor:colorCode, //'#FFD700',
                        fillOpacity: 0.35,
                        path: google.maps.SymbolPath.CIRCLE,
                        scale: 5,
                        anchor: new google.maps.Point(0, 0)
                    }
                };
                geoshapePolygon = new google.maps.Polygon(options);


                PolygonArray.push(geoshapePolygon);

                var PolygonM =
              //new google.maps.Marker({
                   new MarkerWithLabel({
                       map: _map,
                       position: new google.maps.LatLng(centerLat, centerLong),
                       labelContent: labelContentText, // Your label content with a custom class
                       labelAnchor: new google.maps.Point(20, 20), // Adjust the anchor position as needed
                       icon: {
                           strokeColor: '#FFFFFF',
                           strokeOpacity: 1.0,
                           strokeWeight: 2,
                           fillColor:colorCode, //'#000000',  //   //#FFD700
                           fillOpacity: 1.0,
                           path: google.maps.SymbolPath.CIRCLE,
                           scale: 5,
                           labelOrigin: new google.maps.Point(0, 3),
                       },
                       // zIndex: Math.round(centerLat * -100000) << 5

                   });
                PolygonArray.push(PolygonM);

                //PolygonArray.push({
                //    PolygonalShape: geoshapePolygon
                //});
            }
            polygonCoords = null;
        }

      

       // if (type == "CIRCLE") {

            //google.maps.event.addListener(geoshapeCirle, 'mouseover', function () {
            //    if (type == "CIRCLE") {
            //        marker.setPosition(this.getCenter()); // get circle's center
            //    }
            //    else {
            //        marker.setPosition(new google.maps.LatLng(pointLatlng[0], pointLatlng[1]));
            //    }
            //    infowindow.setContent("<b>" + name + "</b>"); // set content
        //    infowindow.open(_map, marker); // open at marker's location
            //    marker.setVisible(false); // hide the marker
            //});

            //google.maps.event.addListener(geoshapeCirle, 'mouseout', function () {
            //    infowindow.close();
            //});
       // }

        //else if (type == "POLYGON") {

            //google.maps.event.addListener(geoshapePolygon, 'mouseover', function () {

            //    if (type == "CIRCLE") {
            //        marker.setPosition(this.getCenter()); // get circle's center
            //    }
            //    else {
            //        marker.setPosition(new google.maps.LatLng(pointLatlng[0], pointLatlng[1]));
            //    }
            //    infowindow.setContent("<b>" + name + "</b>"); // set content
        //    infowindow.open(_map, marker); // open at marker's location
        //    marker.setVisible(false); // hide the marker

            //});

            //google.maps.event.addListener(geoshapePolygon, 'mouseout', function () {
            //    infowindow.close();
            //});
        //}
    }
}



function CreateFullScreenControl(MapControlId, controlDiv, map, center) {
    // We set up a variable for this since we're adding event listeners
    // later.
    var control = this;


    // Set the center property upon construction
    //control.center_ = center;
    controlDiv.style.clear = 'both';
    //controlDiv = document.createElement('div');

    // Start::created by prince
    // if (IsGeofence)
    // {
    var MapViewCenterUI = document.createElement('div');
    MapViewCenterUI.id = 'MapViewCenterUI';
    MapViewCenterUI.setAttribute("data-id", MapControlId);
    // MapViewCenterUI.data-id = 'MapViewCenterUI';
    //geofenceCenterUI.style.display = "none";
    controlDiv.appendChild(MapViewCenterUI);


    var MapViewCenterText = document.createElement('img');
    MapViewCenterText.id = 'MapViewCenterText';
    MapViewCenterText.src = '/Images/FullScreen.png';
    MapViewCenterText.innerHTML = 'Set Map';
    MapViewCenterUI.appendChild(MapViewCenterText);
    // }

    // END::created by prince

    MapViewCenterUI.addEventListener('click', function () {
        var MapId = MapViewCenterUI.dataset.id;
        //alert(MapId)
        GoogleMapFullSize(MapId);
        
    });

}

function GoogleMapFullSize(MapControlId) {
    if (document.getElementById('MapViewCenterText').src.match('/Images/FullScreen.png')) {
        document.getElementById('MapViewCenterText').src = '/Images/DefaultScreen.png';
        var elem = document.getElementById(MapControlId);
        req = elem.requestFullScreen || elem.webkitRequestFullScreen || elem.mozRequestFullScreen;
        req.call(elem);
    }
    else {
        document.getElementById('MapViewCenterText').src = '/Images/FullScreen.png';
        if (document.exitFullscreen) {
            document.exitFullscreen();
        } else if (document.webkitExitFullscreen) { /* Safari */
            document.webkitExitFullscreen();
        } else if (document.msExitFullscreen) { /* IE11 */
            document.msExitFullscreen();
        }
    }
}

