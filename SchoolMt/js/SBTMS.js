
var AddressControllID = '';
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

    var shapes = [];

    drawingManager = new google.maps.drawing.DrawingManager({
        drawingControl: true,
        drawingControlOptions: {
            position: google.maps.ControlPosition.TOP_CENTER,
            drawingModes: [google.maps.drawing.OverlayType.CIRCLE]//, google.maps.drawing.OverlayType.POLYGON]
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

        if (shape.radius > MaxGeoRadius) {
            $('#radius').val(MaxGeoRadius);
            shape.setRadius(MaxGeoRadius);            
            alert('Radius should not be greater than ' + MaxGeoRadius + ' Mtr. ');
        }        

        radius = parseInt(shape.getRadius());
        centerLat = shape.getCenter().lat();
        centerLong = shape.getCenter().lng();
        $('#radius').val(radius);        

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

    google.maps.event.addListener(shapeObj, 'rightclick', function (e) {

        if (IsView == true) {
            var r = confirm("Do you want to delete!");
            if (r == true) {
                var newShape = shapeObj;
                setSelection(newShape);
                $('#radius').val('');
                deleteSelectedShape(false);
            }
        }
    });

    //Fires when shape is dragged on map
    google.maps.event.addListener(shapeObj, 'dragend', function (evt) {

        if (type == "CIRCLE") {
            if (shapeObj.radius > MaxGeoRadius) {

                $('#radius').val(MaxGeoRadius);
                shapeObj.setRadius(MaxGeoRadius);
                alert('Radius should not be greater than ' + MaxGeoRadius + ' Mtr. ');
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
