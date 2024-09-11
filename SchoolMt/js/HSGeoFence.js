/// <reference path="GoogleMapShowAll.js" />
function HeuroGeofence(obj) {
  
    var drawingManager;
    var dialogDiv;
    var customShape;
    var customShapeBounds;

    var map = obj || function() { };
    this.setMap = function(obj) {
        map = obj;
    }
    this.getMap = function() {
        return map;
    }
    this.initdrawingManager = function() {
        var polyOptions = {
            strokeColor: '#000000',
            strokeOpacity: 1.0,
            strokeWeight: 3,
            editable: false,
            draggable: false
        }
        drawingManager = new google.maps.drawing.DrawingManager({
            drawingControl: false,
            polylineOptions: polyOptions,
            rectangleOptions: polyOptions,
            circleOptions: polyOptions, 
            polygonOptions: polyOptions,
            map: map
        });
    }
    this.getDrawingManager = function() {
        return drawingManager;
    }
    this.enableDrawingManager = function(obj) {
        localDrawingManager(obj);
    }
    function localDrawingManager(obj) {
        if (obj == false) {
            if (drawingManager != undefined)
                drawingManager.setMap(null);
        }
        else {
            drawingManager.setMap(map);
        }
    }
    this.enableListener = function(obj) {
        if (obj == true) {
            localDrawingManager(false);
        }
        else {
            localDrawingManager(true);
        }
    }
    this.removeOverlay = function(shape) {
        if (shape != undefined)
            shape.setMap(null);
        shape = undefined;
        localDrawingManager(true);
    }
    this.setDrawingMode = function(obj) {
        if (drawingManager != undefined) {
            switch (obj) {
                case 'CIRCLE':
                    drawingManager.setDrawingMode(google.maps.drawing.OverlayType.CIRCLE);                  
                    break;
                case 'POLYGON':
                    drawingManager.setDrawingMode(google.maps.drawing.OverlayType.POLYGON);                  
                    break;
                case 'RECTANGLE':
                    drawingManager.setDrawingMode(google.maps.drawing.OverlayType.RECTANGLE);                  
                    break;
                case 'POLYLINE':
                    drawingManager.setDrawingMode(google.maps.drawing.OverlayType.POLYLINE);
                    break;
                default:
                    break;
            }
        }
    }
    this.getDrawingMode = function() {
        return localDrawingMode();
    }
    function localDrawingMode() {
        var drawingMode;
        if (drawingManager != undefined) {
            switch (drawingManager.getDrawingMode()) {
                case google.maps.drawing.OverlayType.CIRCLE:
                    drawingMode = 'CIRCLE';
                    break;
                case google.maps.drawing.OverlayType.POLYGON:
                    drawingMode = 'POLYGON';
                    break;
                case google.maps.drawing.OverlayType.RECTANGLE:
                    drawingMode = 'RECTANGLE';
                    break;
                case google.maps.drawing.OverlayType.POLYLINE:
                    drawingMode = 'POLYLINE';
                    break;
                default:
                    break;
            }
        }
        return drawingMode;
    }
    this.setDialogDiv = function(obj) {
        dialogDiv = obj;
    }
    this.getDialogDiv = function() {
        return dialogDiv;
    }

    function getCenterOfPolygon(polygon) {
        var PI = 22 / 7
        var X = 0;
        var Y = 0;
        var Z = 0;
        polygon.getPath().forEach(function(vertex, inex) {
            lat1 = vertex.lat();
            lon1 = vertex.lng();
            lat1 = lat1 * PI / 180
            lon1 = lon1 * PI / 180
            X += Math.cos(lat1) * Math.cos(lon1)
            Y += Math.cos(lat1) * Math.sin(lon1)
            Z += Math.sin(lat1)
        })
        Lon = Math.atan2(Y, X)
        Hyp = Math.sqrt(X * X + Y * Y)
        Lat = Math.atan2(Z, Hyp)
        Lat = Lat * 180 / PI
        Lon = Lon * 180 / PI
        return new google.maps.LatLng(Lat, Lon);
    }

    this.geofenceJSON = function(geofenceNameObj, typeObj, shapeObj) {
        var returnObj;
        if (typeObj == "CIRCLE") {
            var type = 'HeuroGeofence.CIRCLE, HeuroGeofence';
            var center = shapeObj.getCenter().lat() + "," + shapeObj.getCenter().lng();
            var radius = shapeObj.getRadius();
            returnObj = { GeofenceType: typeObj, CenterPoint: center, Radius: radius };
        }

        else if (typeObj == "POLYGON") {
            var type = 'HeuroGeofence.POLYGON, HeuroGeofence';
            var polygon = shapeObj.getPath().getArray();
            var latLng = [];
            for (var i = 0; i < polygon.length; i++) {            
               
                latLng.push({ Latitude: polygon[i].lat(), Longitude: polygon[i].lng() });
            }
            var centerLatLng = getCenterOfPolygon(shapeObj);
            latLng.push({ Latitude: polygon[0].lat(), Longitude: polygon[0].lng() });
            returnObj = {GeofenceType: typeObj, PolygonPoints: latLng, Latitude: centerLatLng.lat(), Longitude: centerLatLng.lng() };
        }

        else if (typeObj == "RECTANGLE") {
            var type = 'HeuroGeofence.RECTANGLE, HeuroGeofence';
            var center = shapeObj.getBounds().getCenter().lat() + "," + shapeObj.getBounds().getCenter().lng();
            var NE = shapeObj.getBounds().getNorthEast().lat() + "," + shapeObj.getBounds().getNorthEast().lng();
            var SW = shapeObj.getBounds().getSouthWest().lat() + "," + shapeObj.getBounds().getSouthWest().lng();
            returnObj = { GeofenceType: typeObj, NorthEast: NE, SouthWest: SW, CenterPoint: center };
        }
        else if (typeObj == "POLYLINE") {
            var type = 'HeuroGeofence.POLYLINE, HeuroGeofence';
            var polygon = shapeObj.getPath().getArray();
            var latLng = [];
            for (var i = 0; i < polygon.length; i++) {
                latLng.push({ Latitude: polygon[i].lat(), Longitude: polygon[i].lng() });
            }
            returnObj = { GeofenceName: geofenceNameObj, GeofenceType: typeObj, Defination: { $type: type, Points: latLng} };
        }
        return returnObj;
    }
    this.getCustomShape = function() {
        return customShape;
    }
    this.setCustomShape = function setCustomShape(type,centerPoint, radius, polyPoints, nePoints, swPoints,colorCode) {
        var shapeObj;
        customShape = obj;      
        if (type == "CIRCLE") {
            var centerPoint = centerPoint.split(',');
            var radius = radius
            var options = {
                strokeColor: colorCode,
                strokeOpacity: 1.0,
                strokeWeight: 3,
                zoom:16,
                center: new google.maps.LatLng(centerPoint[0], centerPoint[1]),
                //radius: Math.sqrt(radius) * 100,
                radius:parseInt(radius),
                map: map
            };
            options.map.center.D = centerPoint[1];
            options.map.center.k = centerPoint[0];
            options.map.zoom=16;
            shapeObj = new google.maps.Circle(options);
            customShapeBounds = shapeObj.getBounds();
            map.setCenter(new google.maps.LatLng(centerPoint[0], centerPoint[1]));
            
            r = shapeObj.getRadius();
            lt = shapeObj.getCenter().lat();
            lg = shapeObj.getCenter().lng();
        }
        else if (type == "POLYGON") {
            customShapeBounds = new google.maps.LatLngBounds();
            var l = polyPoints.split(';');
            
            var polygonCoords = [];
           
            for (var i = 0; i < l.length - 1; i++) {
                if (i != 0) {
                    var pointLatlng = l[i].split(',');
                    polygonCoords.push(new google.maps.LatLng(pointLatlng[0], pointLatlng[1]));
                    customShapeBounds.extend(polygonCoords[i-1]);
                }
            }
            var options = {
                paths: polygonCoords,
                strokeColor: colorCode,
                strokeOpacity: 1.0,
                strokeWeight: 3,
                zoom: 16,
                center: new google.maps.LatLng(polygonCoords[0].k, polygonCoords[1].D),
                map: map
            };

            map.setCenter(new google.maps.LatLng(pointLatlng[0], pointLatlng[1]));
            options.map.center.D = polygonCoords[1].D;
            options.map.center.k = polygonCoords[0].k;
            options.map.zoom = 16;
            shapeObj = new google.maps.Polygon(options);
            polygonCoords = null;
        }
        else if (type == "RECTANGLE") {
            var l = obj.Defination;           
            var nePointlatlng = nePoints.split(',')
            var swPointlatlng = swPoints.split(',')
            var traceLatLon = new google.maps.LatLng(nePointlatlng[0], swPointlatlng[1]);
            var bounds = new google.maps.LatLngBounds(new google.maps.LatLng(nePointlatlng[0],swPointlatlng[1]),
            new google.maps.LatLng(swPointlatlng[0], nePointlatlng[1]));
            var options = {
                bounds: bounds,
                center: traceLatLon,
                zoom: 16,
                strokeColor: colorCode,
                strokeOpacity: 1.0,
                strokeWeight: 3,
                map: map
            };
            options.map.center.D = nePointlatlng[1];
            options.map.center.k = nePointlatlng[0];
            options.map.zoom = 16;
            shapeObj = new google.maps.Rectangle(options);
            customShapeBounds = bounds;
        }
        else if (type == "POLYLINE") {
            customShapeBounds = new google.maps.LatLngBounds();
            var l = obj.Defination.Points;
            var polylineCoords = [];
            for (var i = 0; i < l.length; i++) {
                polylineCoords.push(new google.maps.LatLng(l[i].Latitude, l[i].Longitude));
                customShapeBounds.extend(polylineCoords[i]);
            }
            var options = {
                path: polylineCoords,
                strokeColor: '#000000',
                strokeOpacity: 1.0,
                strokeWeight: 3,
                map: map
            };
            shapeObj = new google.maps.Polyline(options);
        }
        return shapeObj;
    }
    this.getCustomShapeBounds = function() {
        return customShapeBounds;
    }
}

function saveDialog(dialog) {
    var dialogdiv = dialog || function() { };
    var values = "<div id='popupContent'><label>Geofence Name :</label><textarea id='txtAreaObj' rows='4' col='300'></textarea><input type='button' value='save' ID='btnGeofencesave'></div>";
    var callBackFunction;
    var uodateCallFunction;
    this.callBackFunction = function(obj) {
        callBackFunction = obj;
    }
    this.updateCallBackFunction = function(obj) {
        uodateCallFunction = obj;
    }
    this.setDivContent = function(obj) {
        values = obj;
    }
    this.showDiv = function() {
        dialogdiv.html('');
        var options = {};
        dialogdiv.append(values).dialog();
        dialog.parent().show();
        dialogdiv.on('click', '#btnGeofencesave', function() { callBackFunction(); });
        dialogdiv.on('click', '#btnGeofenceupdate', function() { uodateCallFunction(); });
    }
    this.closeDiv = function() {
        dialogdiv.dialog('close');
        dialogdiv.unbind('click');
    }
    this.hideDiv = function() {
        var options = {};
        dialogdiv.parent().hide('shake', options, 1000);
        dialogdiv.unbind('click');
    }
}
