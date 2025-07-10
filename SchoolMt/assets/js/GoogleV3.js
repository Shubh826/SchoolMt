/*
   CREATED BY   : MD TARIQUE KHAN
   CREATED DATE : 2020-06-01 11:15 AM 
   PURPOSE      : GOOGLE APIs
*/

/* Global Variable */
var _mapOptions;
var _map;
var _marker;
var _markerArray = [];
var _geopoiArray = [];
var _infowindow;
var _content;
var _InfoboxContent = [];
var _counter = 0;
var _SetInterval;
var _bPlay;
var _bPause;
var _vehPath;
var _RoutePath;
var trHTML = ''
var loc;
var bounds = new google.maps.LatLngBounds();
var chkZoom = false;
var chkZommChange = false;
var lastzoom = 0;
var _zoomDefault = [6, 12, 13, 14, 16, 17, 18, 49, 21, 9, 19, 22]
var markerCluster;

var trafficLayer;
var screenWidth = window.innerWidth;

/**
 * The CenterControl adds a control to the map that recenters the map on
 * Chicago.
 * @constructor
 * @param {!Element} controlDiv
 * @param {!google.maps.Map} map
 * @param {?google.maps.LatLng} center
 */


function CenterControl(MapControlId, controlDiv, map, center, IsGeofence, IsTraffic, IsPOI) {
    // We set up a variable for this since we're adding event listeners
    // later.
   // ////debugger;
    var control = this;
  

    // Set the center property upon construction
    control.center_ = center;
    controlDiv.style.clear = 'both';
    //controlDiv = document.createElement('div');
   
      // Start::created by prince
  
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

    // END::created by prince

    
    // Set CSS for the control border:: Trafic Layer
  if (IsTraffic)
  {
    var goCenterUI = document.createElement('div');
    goCenterUI.id = 'goCenterUI';
    //goCenterUI.title = 'Click to recenter the map';
    controlDiv.appendChild(goCenterUI);

    // Set CSS for the control interior
    var goCenterText = document.createElement('img');
    goCenterText.id = 'goCenterText';
    // goCenterText.src = '/Images/Traffic.png';
    goCenterText.src = '/Images/TrafficHideIcon.png';
    goCenterText.innerHTML = 'Center Map';
    goCenterUI.appendChild(goCenterText);
  }

    // Set CSS for the setCenter control border
    var setCenterUI = document.createElement('div');
    setCenterUI.id = 'setCenterUI';
    setCenterUI.style.display = "none";
    //setCenterUI.title = 'Click to change the center of the map';
    controlDiv.appendChild(setCenterUI);

    // Set CSS for the control interior
    var setCenterText = document.createElement('div');
    setCenterText.id = 'setCenterText';
    setCenterText.innerHTML = 'Set Center';
    setCenterText.style.display = "none";
    setCenterUI.appendChild(setCenterText);

    // created by  shubam sing
    if (IsGeofence)
    {
        var geofenceCenterUI = document.createElement('div');
        geofenceCenterUI.id = 'geofenceCenterUI';
        //geofenceCenterUI.style.display = "none";
        controlDiv.appendChild(geofenceCenterUI);

        var GeofenceCenterText = document.createElement('img');
        GeofenceCenterText.id = 'geofenceCenterText';
        GeofenceCenterText.src = '/Images/GeofenceHideIcon.png';
        GeofenceCenterText.innerHTML = 'Set Center';
        geofenceCenterUI.appendChild(GeofenceCenterText);
    }

    // end by shubham singh


    // created by :: Prince Kumar Srivastva
    if (IsPOI) {
        //IsPOI
        var POICenterUI = document.createElement('div');
        POICenterUI.id = 'POICenterUI';
        //CenterUI.style.display = "none";
        controlDiv.appendChild(POICenterUI);
        var POICenterText = document.createElement('img');
        POICenterText.id = 'POICenterText';
        POICenterText.src = '/Images/POIHideIcon.png';
        POICenterText.innerHTML = 'Set Center';
        POICenterUI.appendChild(POICenterText);
    }

    // end by :: Prince Kumar Srivastva
   

    // Set up the click event listener for 'Center Map': Set the center of
    // the map
    // to the current center of the control.
    if (IsTraffic) {
        goCenterUI.addEventListener('click', function () {
            //var currentCenter = control.getCenter();
            //map.setCenter(currentCenter);

            if (document.getElementById('goCenterText').src.match('/Images/TrafficHideIcon.png')) {
                document.getElementById('goCenterText').src = '/Images/TrafficshowIcon.png';
            }
            else {
                document.getElementById('goCenterText').src = '/Images/TrafficHideIcon.png';
            }

            toggleTraffic();
        });
    }
    if (IsGeofence) {
        geofenceCenterUI.addEventListener('click', function () {

            GeofenceIconOffOn();
        });
    }
    if (IsPOI) {
        POICenterUI.addEventListener('click', function () {

           POIIconOffOn();
        });
    }
    // Set up the click event listener for 'Set Center': Set the center of
    // the control to the current center of the map.
    setCenterUI.addEventListener('click', function () {
        //var newCenter = map.getCenter();
        //control.setCenter(newCenter);
    });
    MapViewCenterUI.addEventListener('click', function () {
        var MapId = MapViewCenterUI.dataset.id;
       // alert(MapId)
       // alert(this.data-id)
       // alert( MapViewCenterUI.dataset.id)
       
      // var x  = document.getElementById(MapViewCenterUI.id).closest("div").parent().parent()[0].id;
       // var x = document.getElementById(MapViewCenterUI.id).parentNode.nodeName;
       // alert(x.id);
       // MapViewCenterUI
       // alert(MapViewCenterUI);
        GoogleMapFullSize(MapId);
        //var newCenter = map.getCenter();
        //control.setCenter(newCenter);
    });

}

/**
 * Define a property to hold the center state.
 * @private
 */
CenterControl.prototype.center_ = null;

/**
 * Gets the map center.
 * @return {?google.maps.LatLng}
 */
CenterControl.prototype.getCenter = function () {
    return this.center_;
};

/**
 * Sets the map center.
 * @param {?google.maps.LatLng} center
 */
CenterControl.prototype.setCenter = function (center) {
    this.center_ = center;
};

/*CREATED BY: TARIQUE - Function to load blank map*/
function initMap(MapControlID, Zoom, CenterLat, CenterLng, IsGeofence, IsTraffic, IsPOI) {

    try {
        _mapOptions = {
            zoom: parseInt(Zoom),
            center: { lat: CenterLat, lng: CenterLng },
            disableDefaultUI: false,
            draggable: true,
            scrollwheel: true,
            mapTypeId: google.maps.MapTypeId.ROADMAP,
            minZoom: 3,
            fullscreenControl: false
        }


        _map = new google.maps.Map(document.getElementById(MapControlID), _mapOptions);


        trafficLayer = new google.maps.TrafficLayer();
        //google.maps.event.addDomListener(document.getElementById('goCenterUI'), 'click', toggleTraffic);



        // Create the DIV to hold the control and call the CenterControl()
        // constructor
        // passing in this DIV.
        var centerControlDiv = document.createElement('div');
        var centerControl = new CenterControl(MapControlID,centerControlDiv, _map, { lat: CenterLat, lng: CenterLng }, IsGeofence,IsTraffic,IsPOI);

        centerControlDiv.index = 1;
        centerControlDiv.style['padding'] = '10px 10px 0 0';
        _map.controls[google.maps.ControlPosition.RIGHT_TOP].push(centerControlDiv);


        google.maps.event.addListenerOnce(_map, 'idle', function () {
            google.maps.event.trigger(_map, 'resize');
        });

        google.maps.event.addListener(_map, 'zoom_changed', function () {
      //  $('.my-label-class').css("display",(_map.getZoom() > 15 ? "block" : "none"));
            if ((chkZommChange == true && (_zoomDefault.indexOf(_map.zoom) != -1))) {
                if (lastzoom == 0) {
                    chkZoom = false;
                }
                else {
                    chkZoom = true;
                }
            }
            else {
                chkZoom = true;
            }
        })
    }
    catch (Error) { }

}

/*START::Created By Prince Kumar Srivastva*/
function initMapForVehicleDashboard(MapControlID, Zoom, CenterLat, CenterLng, IsGeofence, IsTraffic, IsPOI) {

    try {
        _mapOptions = {
            zoom: parseInt(Zoom),
            center: { lat: CenterLat, lng: CenterLng },
            disableDefaultUI: false,
            draggable: true,
            scrollwheel: true,
            mapTypeId: google.maps.MapTypeId.ROADMAP,
            minZoom: 3,
            fullscreenControl: false
        }


        _map = new google.maps.Map(document.getElementById(MapControlID), _mapOptions);


        trafficLayer = new google.maps.TrafficLayer();
        //google.maps.event.addDomListener(document.getElementById('goCenterUI'), 'click', toggleTraffic);



        // Create the DIV to hold the control and call the CenterControl()
        // constructor
        // passing in this DIV.
        var centerControlDiv = document.createElement('div');
        var centerControl = new CenterControl(MapControlID, centerControlDiv, _map, { lat: CenterLat, lng: CenterLng }, IsGeofence, IsTraffic, IsPOI);

        centerControlDiv.index = 1;
        centerControlDiv.style['padding'] = '10px 10px 0 0';
        _map.controls[google.maps.ControlPosition.RIGHT_TOP].push(centerControlDiv);


        google.maps.event.addListenerOnce(_map, 'idle', function () {
            google.maps.event.trigger(_map, 'resize');
        });

        google.maps.event.addListener(_map, 'zoom_changed', function () {
            $('.my-label-class').css("display", (_map.getZoom() > 15 ? "block" : "none"));
            if ((chkZommChange == true && (_zoomDefault.indexOf(_map.zoom) != -1))) {
                if (lastzoom == 0) {
                    chkZoom = false;
                }
                else {
                    chkZoom = true;
                }
            }
            else {
                chkZoom = true;
            }
        })
    }
    catch (Error) { }

}
/*END::Created By Prince Kumar Srivastva*/


/*CREATED BY: TARIQUE - Function to load blank map*/
function initMap1232(MapControlID, Zoom, CenterLat, CenterLng) {

    try {
        _mapOptions = {
            zoom: parseInt(Zoom),
            center: { lat: CenterLat, lng: CenterLng },
            disableDefaultUI: false,
            draggable: true,
            scrollwheel: true,
            mapTypeId: google.maps.MapTypeId.ROADMAP,
            fullscreenControl: false
        }

        _map = new google.maps.Map(document.getElementById(MapControlID), _mapOptions);
        trafficLayer = new google.maps.TrafficLayer();
        google.maps.event.addDomListener(document.getElementById('trafficToggle'), 'click', toggleTraffic);


        _map = new google.maps.Map(document.getElementById(MapControlID),
            _mapOptions);
        google.maps.event.addListenerOnce(_map, 'idle', function () {
            google.maps.event.trigger(_map, 'resize');
        });

        google.maps.event.addListener(_map, 'zoom_changed', function () {
            if ((chkZommChange == true && (_zoomDefault.indexOf(_map.zoom) != -1))) {
                if (lastzoom == 0) {
                    chkZoom = false;
                }
                else {
                    chkZoom = true;
                }
            }
            else {
                chkZoom = true;
            }
        })


    } catch (Error) { }
}

function toggleTraffic() {
    if (trafficLayer.getMap() == null) {
        //traffic layer is disabled.. enable it
        trafficLayer.setMap(_map);
    } else {
        //traffic layer is enabled.. disable it
        trafficLayer.setMap(null);
    }


}

/*By Vinish : Function to load markers*/
function DrawMarker(MapControlID, Zoom, CenterLat, CenterLng, InfoboxContent) {

    try {

        var iVar = 0;
        ClearMap();

        if (InfoboxContent.length != 0) {
            for (var i = 0; i <= InfoboxContent.length - 1; i++) {
                CreateMarkerWithInfoBox(InfoboxContent[i])
                loc = new google.maps.LatLng(InfoboxContent[i].Lat, InfoboxContent[i].Long);
                bounds.extend(loc)

            }
            chkZommChange = false;
            //   if (IsfitBounds != false) {
            if (chkZoom == false) {
                chkZommChange = true;
                lastzoom = _map.zoom;
                _map.fitBounds(bounds);
                _map.panToBounds(bounds);
            }
            else {
                chkZommChange = false;
                lastzoom = 0;
            }

        } else {
            // alert("Marker list not pass !");
        }
    } catch (Error) {
        // alert("Problem in Draw Marker :" + Error.message);
    }
}


/*CREATED BY: TARIQUE - Function to load WithMovingDirection*/
function DrawMarkerWithMovingDirection(MapControlID, Zoom, CenterLat, CenterLng, InfoboxContent) {
    try {
     
        var iVar = 0;
        if (_markerArray.length > 0) {
            markerCluster.removeMarkers(_markerArray);
        }

        ClearMap();

        if (InfoboxContent.length != 0) {
            for (var i = 0; i <= InfoboxContent.length - 1; i++) {
                CreateMarkerWithInfoBoxWithDirection(InfoboxContent[i])
                loc = new google.maps.LatLng(InfoboxContent[i].Lat, InfoboxContent[i].Long);
                bounds.extend(loc)
            }

            //  START :: ADD BY : HRITIK GHOSH :: DATE : 04/07/2024 :: PURPOSE : MARKER CLASTER FOR MAP

            //markerCluster = new MarkerClusterer(_map, _markerArray, { imagePath: 'https://developers.google.com/maps/documentation/javascript/examples/markerclusterer/m' });

            markerCluster = new markerClusterer.MarkerClusterer({
                map: _map,
                markers: _markerArray,
            });

            /*markerCluster.addMarkers(_markerArray);*/

             //  END :: ADD BY : HRITIK GHOSH :: DATE : 04/07/2024 :: PURPOSE : MARKER CLASTER FOR MAP


            chkZommChange = false;
            if (chkZoom == false) {
                chkZommChange = true;
                lastzoom = _map.zoom;
                _map.fitBounds(bounds);
                //_map.panToBounds(bounds);
            }
            else {
                chkZommChange = false;
                lastzoom = 0;
                _map.fitBounds(bounds);//Commented By Vinish 07052020
                _map.panToBounds(bounds);
            }

            if (InfoboxContent.length == 1) {
                //Added By Shivam Saluja as Suggested by ayan on 19 May 2020
                var loc1 = new google.maps.LatLng(InfoboxContent[0].Lat, InfoboxContent[0].Long);
                var bounds1 = new google.maps.LatLngBounds();

                bounds1.extend(loc1)
                _map.fitBounds(bounds1);
                _map.panToBounds(bounds1);
                _map.setZoom(16);
                _map.setOptions({ minZoom: 3});
            }
            else if (InfoboxContent.length > 1) {
                   _map.setOptions({ minZoom: 1 });
               if (screenWidth<1920 )  //screen size::1920,1366
                {
                    _map.setZoom(2);
                }
            
            }

        } else {
            // alert("Marker list not pass !");
        }
    } catch (Error) {
        // alert("Problem in Draw Marker :" + Error.message);
    }
}


/*Create Marker With InfoBox With Direction*/ //USED
function CreateMarkerWithInfoBoxWithDirection(InfoboxContent, bOpenNewInfoBox, bClosePreviousInfoBox) {

    //added new
    var latLng = new google.maps.LatLng(49.47805, -123.84716);
    var homeLatLng = new google.maps.LatLng(49.47805, -123.84716);

    var pictureLabel = document.createElement("img");
    pictureLabel.src = "home.jpg";

    var _marker = new MarkerWithLabel({
        position: new google.maps.LatLng(InfoboxContent.Lat, InfoboxContent.Long),
        map: _map,
        draggable: false,
        raiseOnDrag: false,
        labelContent: InfoboxContent.RegistrationNo,
        labelInBackground: true,
        labelAnchor: new google.maps.Point(-18, 52),
        labelClass: "labels",
        //labelStyle: { opacity: 0.75 }
    });

    _marker.setIcon('/Images/' + InfoboxContent.Icon);

    if (_infowindow && (bClosePreviousInfoBox == "1" || bClosePreviousInfoBox == "")) {
        _infowindow.close();
    }
    var livetrack = "display:none;";

    if (InfoboxContent.Status == 'P') {
        livetrack = "display:inline-block;"
    }
    else {
        livetrack = "display:inline-block;"
    }

    _infowindow = new google.maps.InfoWindow();

    var HeaderValue = InfoboxContent.RegistrationNo + "</br>" + InfoboxContent.CustomerName;
    var Navigationurl = "https://www.google.com/maps/dir//" + InfoboxContent.Lat + "," + InfoboxContent.Long;

    _content = "<div class='titleHeader'>" + HeaderValue + "<div style='position: absolute; top: 5px; right: 45px;'><a href='" + Navigationurl + "' target='_blank'><img src='../Images/navigationPopup.png' height='30'></a></div>" + "</div><div class='popup'>"
        + "<table class='info-table' style='width:250px;' width='100%'><tr align='left'><td width='110px' align='left'><b>Driver Name</b></td><td><b>:</b></td><td align='left'>"
        + InfoboxContent.DriverName + "</td></tr><tr><td  align='left'><b>Driver Mobile</b></td><td><b>:</b></td><td align='left'>"
        + InfoboxContent.DriverMobileNo + "</td> </tr><tr><td  align='left'><b>Last GPS Update</b></td><td><b>:</b></td><td align='left'>"
        + InfoboxContent.DeviceDateTime + "</td> </tr><tr><td align='left'><b>Speed</b></td><td><b>:</b></td><td align='left'>"
        + InfoboxContent.Speed + "&nbsp;&nbsp;Km/Hr</td> </tr><tr><td align='left'><b>Device No.</b></td><td><b>:</b></td><td align='left'>"
        + InfoboxContent.DeviceNo + "</td> </tr><tr><td align='left'><b>IMEI No.</b></td><td><b>:</b></td><td align='left'>"
        + InfoboxContent.IMEINo + "</td> </tr><tr><td align='left'><b>Sim No.</b></td><td><b>:</b></td><td align='left'>"
        + InfoboxContent.SIMNo
        //+ "</td> </tr><tr><td align='left'><b>Total Duration</b></td><td><b>:</b></td><td align='left'>"
        //+ InfoboxContent.MovingTime
        + "</td></tr>"
        + '<tr><td align="left"></td><td></td><td align="left">'
        //+ '<tr><td align="left"></td><td align="left"><a style=' + livetrack + '  href=javascript:void(0);" onclick="SendLiveTracking(' + InfoboxContent.FK_VehicleId + ',' + InfoboxContent.AccountId + ',' + InfoboxContent.FK_CustomerId + ');">Live Tracking</a>'
        + "</td></tr> </table></div>";

    google.maps.event.addListener(_marker, 'click', (function (_marker, _content) {
        return function () {
            _infowindow.setContent(_content);
            _infowindow.open(_map, _marker);
        }
    })(_marker, _content));

    if (bOpenNewInfoBox == "1") {
        _infowindow.setContent(_content);
        _infowindow.open(_map, _marker);
    }
    _markerArray.push(_marker);  
}

/*Function to load WithMovingDirection*/
function CreateMarkerWithInfoBox(InfoboxContent, bOpenNewInfoBox, bClosePreviousInfoBox) {

    //////debugger;

    // var img='http://chart.apis.google.com/chart?chst=d_map_pin_letter&chld='+A|FF0000|000000'

    //let icon = (typeof (InfoboxContent.Icon) != "undefined") ? InfoboxContent.Icon : InfoboxContent.icon;

    //let lat = (typeof (InfoboxContent.Lat) != "undefined") ? InfoboxContent.Lat :
    //            (typeof (InfoboxContent.lat) != "undefined") ? InfoboxContent.lat :
    //            (typeof (InfoboxContent.Latitute) != "undefined") ? InfoboxContent.Latitute :
    //            (typeof (InfoboxContent.latitute) != "undefined") ? InfoboxContent.latitute :
    //            (typeof (InfoboxContent.Latitude) != "undefined") ? InfoboxContent.Latitude : InfoboxContent.latitude;

    //let long = (typeof (InfoboxContent.Long) != "undefined") ? InfoboxContent.Long :
    //            (typeof (InfoboxContent.long) != "undefined") ? InfoboxContent.long :
    //            (typeof (InfoboxContent.Longitute) != "undefined") ? InfoboxContent.Longitute :
    //            (typeof (InfoboxContent.longitute) != "undefined") ? InfoboxContent.longitute :
    //            (typeof (InfoboxContent.Longitude) != "undefined") ? InfoboxContent.Longitude : InfoboxContent.longitude;

    _marker = new google.maps.Marker({
        map: _map,
        icon: '/App_Images/' + InfoboxContent.icon,

        position: new google.maps.LatLng(InfoboxContent.latitude, InfoboxContent.longitude)
    });



    if (_infowindow && (bClosePreviousInfoBox == "1" || bClosePreviousInfoBox == "")) {
        _infowindow.close();
    }
    var livetrack = "display:none;";
    //if (InfoboxContent.Status == 'P') {
    //    livetrack = "display:inline-block;"
    //}
    _infowindow = new google.maps.InfoWindow();
    var HeaderValue = InfoboxContent.registrationNo + "</br>" + InfoboxContent.CustomerName;
    _content = "<div class='titleHeader'>" + HeaderValue + "</div><div class='popup'>"
        + "<table class='info-table' style='width:250px;' width='100%'><tr align='left'><td width='110px' align='left'><b>Driver Name</b></td><td><b>:</b></td><td align='left'>"
        + InfoboxContent.chauffeurName + "</td></tr><tr><td  align='left'><b>Driver Mobile</b></td><td><b>:</b></td><td align='left'>"
        + InfoboxContent.chauffeurMobileNo + "</td> </tr><tr><td  align='left'><b>Last GPS Update</b></td><td><b>:</b></td><td align='left'>"
        + InfoboxContent.receivedTime + "</td> </tr><tr><td align='left'><b>Speed</b></td><td><b>:</b></td><td align='left'>"
        + InfoboxContent.speed + "&nbsp;&nbsp;Km/Hr</td> </tr><tr><td align='left'><b>Device No.</b></td><td><b>:</b></td><td align='left'>"
        + InfoboxContent.deviceNo + "</td> </tr><tr><td align='left'><b>IMEI No.</b></td><td><b>:</b></td><td align='left'>"
        + InfoboxContent.imeino + "</td> </tr><tr><td align='left'><b>Sim No.</b></td><td><b>:</b></td><td align='left'>"
        + InfoboxContent.simno
        // + "</td> </tr><tr><td align='left'><b>Total Duration</b></td><td><b>:</b></td><td align='left'>"
        //+ InfoboxContent.MovingTime
        + "</td></tr>"
        + '<tr><td align="left"></td><td></td><td align="left">'
        + "</td></tr> </table></div>";

    google.maps.event.addListener(_marker, 'click', (function (_marker, _content) {
        return function () {
            _infowindow.setContent(_content);
            _infowindow.open(_map, _marker);
        }
    })(_marker, _content));

    if (bOpenNewInfoBox == "1") {
        _infowindow.setContent(_content);
        _infowindow.open(_map, _marker);
    }

    _markerArray.push(_marker);

}

function DrawTripMarker(MapControlID, Zoom, CenterLat, CenterLng, InfoboxContent) {
    try {

        var iVar = 0;
        ClearMap();

        if (InfoboxContent.length != 0) {
            for (var i = 0; i <= InfoboxContent.length - 1; i++) {
                CreateTripMarkerWithInfoBox(InfoboxContent[i])
                loc = new google.maps.LatLng(InfoboxContent[i].Lat, InfoboxContent[i].Long);
                bounds.extend(loc)

            }
            //chkZommChange = false;
            //   if (IsfitBounds != false) {
            //if (chkZoom == false) {
            //    chkZommChange = true;
            //    lastzoom = _map.zoom;
            _map.fitBounds(bounds);
            _map.panToBounds(bounds);
            //}
            //else {
            //    chkZommChange = false;
            //    lastzoom = 0;
            //}

        } else {
            // alert("Marker list not pass !");
        }
    } catch (Error) {
        // alert("Problem in Draw Marker :" + Error.message);
    }
}

function CreateTripMarkerWithInfoBoxforLiveTracking(InfoboxContent, bOpenNewInfoBox, bClosePreviousInfoBox, id) {

    _marker = new google.maps.Marker({
        map: _map,
        position: new google.maps.LatLng(InfoboxContent.Lat, InfoboxContent.Long)
    });
    //  _marker.id = InfoboxContent.length;
    var str = RotateIcon.makeIcon('/App_Images/' + InfoboxContent.Icon).setRotation({ deg: InfoboxContent.Heading }).getUrl();
    // alert(str);


    //By Tarique, Suggested by Deepak SIR, on 9June_14:40PM  :: Previously just below code block was implemented
    if (endsWith(str, "AD12TscoAAAAAElFTkSuQmCC")) {
        _marker.setIcon('/App_Images/' + InfoboxContent.Icon);
    }
    else {
        _marker.setIcon(RotateIcon.makeIcon('/App_Images/' + InfoboxContent.Icon).setRotation({ deg: InfoboxContent.Heading }).getUrl());
    }


    //Commented by Tarique, Suggested by Deepak SIR, on 9June_14:40PM
    //if (str.endsWith("AD12TscoAAAAAElFTkSuQmCC"))
    //    _marker.setIcon('../../App_Images/' + InfoboxContent.Icon);
    //else
    //    _marker.setIcon(RotateIcon.makeIcon('/App_Images/' + InfoboxContent.Icon).setRotation({ deg: InfoboxContent.Heading }).getUrl());





    if (_infowindow && (bClosePreviousInfoBox == "1" || bClosePreviousInfoBox == "")) {
        _infowindow.close();
    }
    var livetrack = "display:none;";
    if (InfoboxContent.Status == 'P') {
        livetrack = "display:inline-block;"
    }
    _infowindow = new google.maps.InfoWindow();

    _content =
        "<div class='titleHeader'></div><div class='popup' style='width:450px;'><table class='info-table' width='100%'><tr align='left'><td colspan='4'><b style='margin-bottom:5px;display:block;font-size:14px;color:#e90a0b'>" + InfoboxContent.Route_Name + "</b></td></tr><tr align='left'><td width='90px' align='left'><b>Vehicle No.:</b></td><td  colspan='3'><b>"
        + InfoboxContent.Registration_No + "</b></td></tr><tr nowrap><td width='90px' align='left'><b>Driver Name:</b></td><td align='left'>"
        + InfoboxContent.DriverName + "</td><td  align='left'><b>Driver Mob:</b></td><td align='left'>"
        + InfoboxContent.DriverMobileNo + "</td> </tr><tr><td  align='left'><b>Recorded :</b></td><td align='left'>"
        + InfoboxContent.DeviceDateTime + "</td>"//<tr><td align='left'><b>Device No. :</b></td><td align='left'>"
        + "<td align='left'><b>Trip No. :</b></td><td align='left'>"
        + InfoboxContent.Trip_No
        + "</td></tr><tr><td align='left'><b>ETD :</b></td><td align='left'>"
        + InfoboxContent.ETD
        + "</td><td align='left'><b>ETA :</b></td><td align='left'>"
        + InfoboxContent.ExpectedDtofArrival
        + "</td></tr><tr><td align='left'><b>RTA :</b></td><td align='left'>"
        + InfoboxContent.RTA
        + "</td><td align='left'><b>Travel Date :</b></td><td align='left'>"
        + InfoboxContent.Travel_Date
        + "</td></tr><tr><td align='left'><b>ATD :</b></td><td align='left'>"
        + InfoboxContent.ATD
        + "</td></tr>"//<tr><td align='left'><b>ATA:</b></td><td align='left'>"
        //+ InfoboxContent.ATA
        //+ "</td></tr>"
        + "</td></tr><tr><td align='left'><b>Alarm :</b></td><td colspan='3' align='left'>"
        + InfoboxContent.AlarmDesc
        + "</td></tr>"
        //comment by anjani
        + '<tr><td align="left"></td><td align="left"><a href="#" onclick="ShowTrafficOnMap(\'' + InfoboxContent.Trip_No + '\');">Live Traffic</a>'
        + "</td></tr>"
        + "</table></div>";
    google.maps.event.addListener(_marker, 'click', (function (_marker, _content) {
        return function () {
            _infowindow.setContent(_content);
            _infowindow.open(_map, _marker);
        }
    })(_marker, _content));

    if (bOpenNewInfoBox == "1") {
        _infowindow.setContent(_content);
        _infowindow.open(_map, _marker);
    }
    _map.setCenter(new google.maps.LatLng(InfoboxContent.Lat, InfoboxContent.Long));


    _markerArray.push(_marker);


}

function CreateRouteWithoutMarker(MapControlID, Zoom, CenterLat, CenterLng, InfoboxContent, PathColor, PathOpacity, PathWeight) {
    

    try {

        ClearRoute()

        var prelat;
        var prelong;
        var preISGSM = 'OFF'
        var j = 0;
        var k = 0;
        var goto = 0;
        if (InfoboxContent.length != 0) {
            for (var i = 0; i <= InfoboxContent.length - 1; i++) {
                PathColor = '#006fe6'
                if (i == 0 || i == InfoboxContent.length - 1) {
                    CreateMarkerWithInfoBoxLiveTracking(InfoboxContent[i])
                }
                if (i > 0) {
                    if (InfoboxContent[i].ISGSM == 'ON' && preISGSM == 'OFF') {
                        k = i - 1;
                        preISGSM = 'ON';
                        goto = 1;
                    }
                    else
                        if (InfoboxContent[i].ISGSM == 'ON' && preISGSM == 'ON') {
                            preISGSM = 'ON';
                            goto = 1;
                        }
                        else if (InfoboxContent[i].ISGSM == 'OFF' && preISGSM == 'ON') {
                            preISGSM = 'OFF';
                            goto = 0;

                        } else if (InfoboxContent[i].ISGSM == 'OFF' && preISGSM == 'OFF') {
                            k = 0;
                            preISGSM = 'OFF';
                            goto = 0;
                        }
                    if (InfoboxContent[i].icon == 'pin_yellow.png') {
                        PathColor = '#F3971E'
                    }
                    if (InfoboxContent[i].icon == 'pin_green.png') {
                        PathColor = '#65D873'
                    }
                    if (InfoboxContent[i].icon == 'pin_blue.png') {
                        PathColor = '#2191ED'
                    }
                    if (InfoboxContent[i].icon == 'pin_red.png') {
                        PathColor = '#EA0000'
                    }

                    if (goto < 1) {
                        if (k > 0) {
                            var _vehTrackCoordinates = [new google.maps.LatLng(InfoboxContent[i].latitude,
                                InfoboxContent[i].longitude),
                            new google.maps.LatLng(InfoboxContent[k].latitude,
                                InfoboxContent[k].longitude)
                            ];

                        }
                        //else if (InfoboxContent[i].ISGSM == 'OFF') {
                        else {

                            var _vehTrackCoordinates = [new google.maps.LatLng(InfoboxContent[i].latitude,
                                InfoboxContent[i].longitude),
                            new google.maps.LatLng(InfoboxContent[i - 1].latitude,
                                InfoboxContent[i - 1].longitude)
                            ];
                        }

                        _RoutePath = new google.maps.Polyline({
                            path: _vehTrackCoordinates,
                            strokeColor: PathColor,//"#CC3300",
                            strokeOpacity: PathOpacity,//80,
                            strokeWeight: PathWeight,//2,
                            icons: [{

                                icon: {
                                    path: google.maps.SymbolPath.BACKWARD_CLOSED_ARROW,
                                    scale: 2,
                                    //rotation: heading,
                                    strokeColor: PathColor,
                                    fillColor: PathColor,
                                    fillOpacity: 3
                                },
                                repeat: '150px',
                                path: _vehTrackCoordinates
                            }]
                        });
                        _RoutePath.setMap(_map);
                    }
                }
            }

            if (InfoboxContent.length > 0) {
                var endlenth = InfoboxContent.length;
                _markerArray[0].setAnimation(google.maps.Animation.BOUNCE);
                //_markerArray[endlenth - 1].setAnimation(google.maps.Animation.BOUNCE);
                _map.setCenter(new google.maps.LatLng(InfoboxContent[InfoboxContent.length - 1].latitude, InfoboxContent[InfoboxContent.length - 1].longitude));
            }

        } else {
            // alert("Marker list not pass !");
        }
    } catch (Error) {
        // alert("Problem in Create Route :" + Error.message);
    }
}

/*Function to load WithMovingDirection*/
function CreateMarkerWithInfoBoxLiveTracking(InfoboxContent, bOpenNewInfoBox, bClosePreviousInfoBox) {
    // var img='http://chart.apis.google.com/chart?chst=d_map_pin_letter&chld='+A|FF0000|000000'
    
    _marker = new google.maps.Marker({
        map: _map,
        icon: '/App_Images/' + InfoboxContent.icon,

        position: new google.maps.LatLng(InfoboxContent.latitude, InfoboxContent.longitude)
    });


    if (_infowindow && (bClosePreviousInfoBox == "1" || bClosePreviousInfoBox == "")) {
        _infowindow.close();
    }
    var livetrack = "display:none;";
    if (InfoboxContent.Status == 'P') {
        livetrack = "display:inline-block;"
    }
    _infowindow = new google.maps.InfoWindow();
    var HeaderValue = InfoboxContent.registrationNo + "</br>" + InfoboxContent.CustomerName

    _content = "<div class='titleHeader'>" + HeaderValue + "<div style='position: absolute; top: 5px; right: 45px;'></div>" + '<div style="position: absolute;top: 0px;right: 5px;"><button draggable="false" aria-label="Close" title="Close" type="button" class="gm-ui-hover-effect" style="background: none;border: 0;margin: 0;padding:0;cursor:pointer;user-select: none;width: 28px;height: 28px;position: absolute;top: 10px;right: 10px;" onClick="closeInfoWindow();"><span style="mask-image: url(&quot;data:image/svg+xml,%3Csvg%20xmlns%3D%22http%3A//www.w3.org/2000/svg%22%20viewBox%3D%220%200%2024%2024%22%3E%3Cpath%20d%3D%22M19%206.41L17.59%205%2012%2010.59%206.41%205%205%206.41%2010.59%2012%205%2017.59%206.41%2019%2012%2013.41%2017.59%2019%2019%2017.59%2013.41%2012z%22/%3E%3Cpath%20d%3D%22M0%200h24v24H0z%22%20fill%3D%22none%22/%3E%3C/svg%3E&quot;);pointer-events: none;display: block;width: 22px;height: 22px;margin: 3px;top: 2px;"></span></button></div>' + "</div><div class='popup'>"
        + "<table class='info-table' style='width:100%' width='100%'><tr align='left'><td width='110px' align='left'><b>Driver Name</b></td><td><b>:</b></td><td align='left'>"
        + InfoboxContent.chauffeurName + "</td></tr><tr><td  align='left'><b>Driver Mobile</b></td><td><b>:</b></td><td align='left'>"
        + InfoboxContent.chauffeurMobileNo + "</td> </tr><tr><td id='gpsUpdate' align='left'><b>Last GPS Update</b></td><td><b>:</b></td><td align='left'>"
        + InfoboxContent.receivedTime + "</td> </tr><tr><td align='left'><b>Speed</b></td><td><b>:</b></td><td align='left'>"
        + InfoboxContent.speed + "&nbsp;&nbsp;Km/Hr</td> </tr><tr><td align='left'><b>Device No.</b></td><td><b>:</b></td><td align='left'>"
        + InfoboxContent.deviceNo + "</td> </tr><tr><td align='left'><b>IMEI No.</b></td><td><b>:</b></td><td align='left'>"
        + InfoboxContent.imeino + "</td> </tr><tr><td align='left'><b>Sim No.</b></td><td><b>:</b></td><td align='left'>"
        + InfoboxContent.simno
        + "</td></tr>"
        + '<tr><td align="left"></td><td></td><td align="left">'
        + "</td></tr> </table></div>";

    google.maps.event.addListener(_marker, 'click', (function (_marker, _content) {
        return function () {
            _infowindow.setContent(_content);
            _infowindow.open(_map, _marker);
        }
    })(_marker, _content));

    if (bOpenNewInfoBox == "1") {
        _infowindow.setContent(_content);
        _infowindow.open(_map, _marker);
    }

    _markerArray.push(_marker);

}



/*CREATED BY: TARIQUE - Creates Marker & Polyline On History Tracking Page*/
function CreateRouteWithoutMarkerForHistoryTracking(MapControlID, Zoom, CenterLat, CenterLng, InfoboxContent, PathColor, PathOpacity, PathWeight) {
    
    try {

        ClearRoute()

        var prelat;
        var prelong;
        var preISGSM = 'OFF'
        var j = 0;
        var k = 0;
        var goto = 0;

        if (InfoboxContent.length != 0) {

            for (var i = 0; i <= InfoboxContent.length - 1; i++) {

                PathColor = '#006fe6'

                if (i == 0 || i == InfoboxContent.length - 1) {
                    CreateMarkerWithInfoBoxForHistoryTracking(InfoboxContent[i])
                }

                if (i > 0) {

                    //if (InfoboxContent[i].ISGSM == 'ON' && preISGSM == 'OFF') {
                    //    k = i - 1;
                    //    preISGSM = 'ON';
                    //    goto = 1;
                    //}
                    //else
                    //    if (InfoboxContent[i].ISGSM == 'ON' && preISGSM == 'ON') {
                    //        preISGSM = 'ON';
                    //        goto = 1;
                    //    }
                    //    else if (InfoboxContent[i].ISGSM == 'OFF' && preISGSM == 'ON') {
                    //        preISGSM = 'OFF';
                    //        goto = 0;
                    //    } else if (InfoboxContent[i].ISGSM == 'OFF' && preISGSM == 'OFF') {
                    //        k = 0;
                    //        preISGSM = 'OFF';
                    //        goto = 0;
                    //    }

                    if (InfoboxContent[i].icon == 'pin_yellow.png') {
                        PathColor = '#F3971E'
                    }
                    if (InfoboxContent[i].icon == 'pin_green.png') {
                        PathColor = '#65D873'
                    }
                    if (InfoboxContent[i].icon == 'pin_blue.png') {
                        PathColor = '#2191ED'
                    }
                    if (InfoboxContent[i].icon == 'pin_red.png') {
                        PathColor = '#EA0000'
                    }

                    if (goto < 1) {
                        if (k > 0) {
                            var _vehTrackCoordinates = [
                                new google.maps.LatLng(InfoboxContent[i].latitude, InfoboxContent[i].longitude),
                                new google.maps.LatLng(InfoboxContent[k].latitude, InfoboxContent[k].longitude)
                            ];

                        }
                        //else if (InfoboxContent[i].ISGSM == 'OFF') {
                        else {

                            var _vehTrackCoordinates = [
                                new google.maps.LatLng(InfoboxContent[i].latitude, InfoboxContent[i].longitude),
                                new google.maps.LatLng(InfoboxContent[i - 1].latitude, InfoboxContent[i - 1].longitude)
                            ];
                        }

                        _RoutePath = new google.maps.Polyline({
                            path: _vehTrackCoordinates,
                            strokeColor: PathColor,//"#CC3300",
                            strokeOpacity: PathOpacity,//80,
                            strokeWeight: PathWeight,//2,
                            icons: [{

                                icon: {
                                    path: google.maps.SymbolPath.BACKWARD_CLOSED_ARROW,
                                    scale: 2,
                                    //rotation: heading,
                                    strokeColor: PathColor,
                                    fillColor: PathColor,
                                    fillOpacity: 3
                                },
                                repeat: '150px',
                                path: _vehTrackCoordinates
                            }]
                        });
                        _RoutePath.setMap(_map);
                    }

                }

            }

            if (InfoboxContent.length > 0) {
                var endlenth = InfoboxContent.length;
                _markerArray[0].setAnimation(google.maps.Animation.BOUNCE);
                //_markerArray[endlenth - 1].setAnimation(google.maps.Animation.BOUNCE);
                _map.setCenter(new google.maps.LatLng(InfoboxContent[InfoboxContent.length - 1].latitude, InfoboxContent[InfoboxContent.length - 1].longitude));
            }

        }
        else {
            // alert("Marker list not pass !");
        }
    }
    catch (Error) {
        // alert("Problem in Create Route :" + Error.message);
    }
}

/*CREATED BY: TARIQUE - Creates Marker On History Tracking Page*/
function CreateMarkerWithInfoBoxForHistoryTracking(InfoboxContent, bOpenNewInfoBox, bClosePreviousInfoBox) {
    //////debugger

    _marker = new google.maps.Marker({
        map: _map,
        icon: '/App_Images/' + InfoboxContent.icon,

        position: new google.maps.LatLng(InfoboxContent.latitude, InfoboxContent.longitude)
    });


    if (_infowindow && (bClosePreviousInfoBox == "1" || bClosePreviousInfoBox == "")) {
        _infowindow.close();
    }
    var livetrack = "display:none;";
    //if (InfoboxContent.Status == 'P') {
    //    livetrack = "display:inline-block;"
    //}
    _infowindow = new google.maps.InfoWindow();
    var HeaderValue = InfoboxContent.registrationNo + "</br>" + InfoboxContent.CustomerName
    _content = "<div class='titleHeader'>" + HeaderValue + "<div style='position: absolute; top: 5px; right: 45px;'></div>" + '<div style="position: absolute;top: 0px;right: 5px;"><button draggable="false" aria-label="Close" title="Close" type="button" class="gm-ui-hover-effect" style="background: none;border: 0;margin: 0;padding:0;cursor:pointer;user-select: none;width: 28px;height: 28px;position: absolute;top: 10px;right: 10px;" onClick="closeInfoWindow();"><span style="mask-image: url(&quot;data:image/svg+xml,%3Csvg%20xmlns%3D%22http%3A//www.w3.org/2000/svg%22%20viewBox%3D%220%200%2024%2024%22%3E%3Cpath%20d%3D%22M19%206.41L17.59%205%2012%2010.59%206.41%205%205%206.41%2010.59%2012%205%2017.59%206.41%2019%2012%2013.41%2017.59%2019%2019%2017.59%2013.41%2012z%22/%3E%3Cpath%20d%3D%22M0%200h24v24H0z%22%20fill%3D%22none%22/%3E%3C/svg%3E&quot;);pointer-events: none;display: block;width: 22px;height: 22px;margin: 3px;top: 2px;"></span></button></div>' + "</div><div class='popup'>"
        + "<table class='info-table' style='width:250px;' width='100%'><tr align='left'><td width='110px' align='left'><b>Driver Name</b></td><td><b>:</b></td><td align='left'>"
        + InfoboxContent.chauffeurName + "</td></tr><tr><td  align='left'><b>Driver Mobile</b></td><td><b>:</b></td><td align='left'>"
        + InfoboxContent.chauffeurMobileNo + "</td> </tr><tr><td  align='left'><b>Last GPS Update</b></td><td><b>:</b></td><td align='left'>"
        + InfoboxContent.receivedTime + "</td> </tr><tr><td align='left'><b>Speed</b></td><td><b>:</b></td><td align='left'>"
        + InfoboxContent.speed + "&nbsp;&nbsp;Km/Hr</td> </tr><tr><td align='left'><b>Device No.</b></td><td><b>:</b></td><td align='left'>"
        + InfoboxContent.deviceNo + "</td> </tr><tr><td align='left'><b>IMEI No.</b></td><td><b>:</b></td><td align='left'>"
        + InfoboxContent.imeino + "</td> </tr><tr><td align='left'><b>Sim No.</b></td><td><b>:</b></td><td align='left'>"
        + InfoboxContent.simno + "</td> </tr>"
      //  +"<tr><td align='left'>"<b>Total Duration</b></td><td><b>:</b></td><td align='left'>"
       // + InfoboxContent.MovingTime
       // + "</td></tr>"
        + '<tr><td align="left"></td><td></td><td align="left">'
        + "</td></tr> </table></div>";

    //_content = "<div class='titleHeader'>" + HeaderValue + "</div><div class='popup'><table class='info-table' width='100%'><tr align='left'><td><b>"
    //+ "" + "</b></td></tr><tr nowrap><td width='90px' align='left'><b>Driver Name:</b></td><td align='left'>"
    //+ InfoboxContent.chauffeurName + "</td></tr><tr><td  align='left'><b>Driver Mobile:</b></td><td align='left'>"
    //+ InfoboxContent.chauffeurMobileNo + "</td> </tr><tr><td  align='left'><b>Recorded :</b></td><td align='left'>"
    //+ InfoboxContent.receivedTime + "</td> </tr><tr><td align='left'><b>Speed :</b></td><td align='left'>"
    //  + InfoboxContent.speed + " Km/Hr</td> </tr><tr><td align='left'><b>Device No. :</b></td><td align='left'>"
    //    + InfoboxContent.deviceNo + "</td></tr>" //<tr><td align='left'><b>IMEI No. :</b></td><td align='left'>"
    //    //+ InfoboxContent.imeino + "</td> </tr><tr><td align='left'><b>Sim No. :</b></td><td align='left'>"
    ////+ InfoboxContent.SIMNo
    ////+ "</td></tr>"
    //+ '<tr><td align="left"></td><td align="left"><a style=' + livetrack + '  href="#" onclick="SendLiveTracking(\'' + InfoboxContent.registrationNo + '\'' + ',' + "InfoboxContent.FK_CompanyId" + ');">Live Tracking</a>'
    //+ "</td></tr> </table></div>";
    ////  + "<tr><td align='left'></td><td align='left'><a href='#' onclick='SendLiveTracking(''+RegNo,CompId)'>Live Tracking</a>"
    google.maps.event.addListener(_marker, 'click', (function (_marker, _content) {
        return function () {
            _infowindow.setContent(_content);
            _infowindow.open(_map, _marker);
        }
    })(_marker, _content));

    if (bOpenNewInfoBox == "1") {
        _infowindow.setContent(_content);
        _infowindow.open(_map, _marker);
    }

    _markerArray.push(_marker);

}

function PlayRouteWithoutMarker(MapControlID, Zoom, CenterLat, CenterLng, InfoboxContent, PathColor, PathOpacity, PathWeight, TimerIntervalMS, bAlwayOpenInfoBox, IsPanEnable) {

    try {

        ClearMap();
        _counter = !_counter ? 0 : _counter;
        _InfoboxContent = InfoboxContent;
        trHTML = '';

        _SetInterval = setInterval('PlayWithoutMarker(' + '"' + PathColor + '",' + PathOpacity + ',' + PathWeight + ',' + bAlwayOpenInfoBox + ',' + IsPanEnable + ')', TimerIntervalMS);
    } catch (Error) {
        // alert("Problem in Create Route :" + Error.message);
    }
}

var _mycounter = 0;
function PlayWithoutMarker(PathColor, PathOpacity, PathWeight, bAlwayOpenInfoBox, IsPanEnable) {
    //////debugger
    if (_counter <= _InfoboxContent.length - 1) {
        _mycounter++;
        if (($("#myplaybutton" + slotId + "").find('i').attr('class'))) {
            if (($("#myplaybutton" + slotId + "").find('i').attr('class')).indexOf('play') > -1) {
                return false;
            }
        }

        if ($("#myplaybutton" + slotId + "").attr('myval') && !$("#fast" + slotId + "").attr('myval')) {
            return false;
        }
        if (_InfoboxContent.length == _mycounter) {


            $("#myplaybutton" + slotId + "").removeAttr('val');
            $("#myplaybutton" + slotId + "").attr('val', 'play');
            $("#myplaybutton" + slotId + "").find('i').removeAttr('class');
            $("#myplaybutton" + slotId + "").find('i').attr('class', 'la la-play');

            clearInterval(_SetInterval);

        }
        else {

        }

        var mydata = _InfoboxContent[_mycounter - 1];

        if (mydata) {
            $("#datadiv" + slotId + "").show();
            $("#ANDCriteria" + slotId + "").show();
            $("#iconsdata" + slotId + "").show();
            $("#datadiv" + slotId + "").find('#from_to').text(mydata.receivedTime);
            $("#datadiv" + slotId + "").find('#engineon').text(mydata.gpstime);
            $("#datadiv" + slotId + "").find('#speed').text(mydata.speed);
            $("#datadiv" + slotId + "").find('#distance').text(mydata.Distance);
            $("#datadiv" + slotId + "").find('#add').text(mydata.location + " ," + mydata.latitude + " ," + mydata.longitude);

        }
        else {
            //            $("#datadiv").hide();
        }

        ///Progress Bar Incremental
        if ($("#btnIncrementBar" + slotId + "").attr('aria-valuemax')) {
            $("#btnIncrementBar" + slotId + "").removeAttr('aria-valuemax');
            $("#btnIncrementBar" + slotId + "").attr('aria-valuemax', _InfoboxContent.length);
        }

        var t = (parseInt(_counter) + 1);

        var oneperce = 100 / (_InfoboxContent.length);
        t = oneperce * t;

        t = t + '%';

        $("#btnIncrementBar" + slotId + "").css({ 'width': t, 'background-color': '#393b4a' });
        $("#btnIncrementBar" + slotId + "").removeAttr('aria-valuenow');
        $("#btnIncrementBar" + slotId + "").attr('aria-valuenow', (_counter + 1));
        //$("#btnIncrementBar").text(t);

        var prelat;
        var prelong;
        var preISGSM = 'OFF'
        var j = 0;
        var k = 0;
        var goto = 0;


        if (_counter == 0 || _counter == _InfoboxContent.length - 1) {

            CreateMarkerWithInfoBox(_InfoboxContent[_counter]);
            if (_InfoboxContent.length > 1) {
                CreateMarkerWithInfoBoxForLast(_InfoboxContent[_InfoboxContent.length - 1]);
            }

        }

        //CreateTableHistoryBody(_InfoboxContent[_counter]);

        PathColor = '#006fe6'

        if (_counter > 0) {
            if (_InfoboxContent[_counter].ISGSM == 'ON' && preISGSM == 'OFF') {
                k = _counter - 1;
                preISGSM = 'ON';
                goto = 1;
            }
            else
                if (_InfoboxContent[_counter].ISGSM == 'ON' && preISGSM == 'ON') {
                    preISGSM = 'ON';
                    goto = 1;
                }
                else if (_InfoboxContent[_counter].ISGSM == 'OFF' && preISGSM == 'ON') {
                    preISGSM = 'OFF';
                    goto = 0;

                } else if (_InfoboxContent[_counter].ISGSM == 'OFF' && preISGSM == 'OFF') {
                    k = 0;
                    preISGSM = 'OFF';
                    goto = 0;
                }
            //if (_InfoboxContent[_counter].Icon == 'pin_yellow.png') {
            //    PathColor = '#F3971E'
            //}
            //if (_InfoboxContent[_counter].Icon == 'pin_green.png') {
            //    PathColor = '#65D873'
            //}
            //if (_InfoboxContent[_counter].Icon == 'pin_blue.png') {
            //    PathColor = '#2191ED'
            //}
            //if (_InfoboxContent[_counter].Icon == 'pin_red.png') {
            //    PathColor = '#EA0000'
            //}




            /*
            
            if (goto < 1) {
                        if (k > 0) {
                            var _vehTrackCoordinates =  [
                                                            new google.maps.LatLng(InfoboxContent[i].latitude, InfoboxContent[i].longitude),
                                                            new google.maps.LatLng(InfoboxContent[k].latitude, InfoboxContent[k].longitude) 
                                                        ];

                        }
                        //else if (InfoboxContent[i].ISGSM == 'OFF') {
                        else{

                            var _vehTrackCoordinates =  [
                                                            new google.maps.LatLng(InfoboxContent[i].latitude, InfoboxContent[i].longitude),
                                                            new google.maps.LatLng(InfoboxContent[i - 1].latitude, InfoboxContent[i - 1].longitude)
                                                        ];
                        }

                        _RoutePath = new google.maps.Polyline({
                            path: _vehTrackCoordinates,
                            strokeColor: PathColor,//"#CC3300",
                            strokeOpacity: PathOpacity,//80,
                            strokeWeight: PathWeight,//2,
                            icons: [{

                                icon: {
                                    path: google.maps.SymbolPath.BACKWARD_CLOSED_ARROW,
                                    scale: 2,
                                    //rotation: heading,
                                    strokeColor: PathColor,
                                    fillColor: PathColor,
                                    fillOpacity: 3
                                },
                                repeat: '150px',
                                path: _vehTrackCoordinates
                            }]
                        });
                        _RoutePath.setMap(_map);
                    }
            
            */









            if (goto < 1) {

                if (k > 0) {
                    var _vehTrackCoordinates = [
                        new google.maps.LatLng(_InfoboxContent[_counter].latitude, _InfoboxContent[_counter].longitude),
                        new google.maps.LatLng(_InfoboxContent[k].latitude, _InfoboxContent[k].longitude)
                    ];


                }
                //else if (InfoboxContent[i].ISGSM == 'OFF') {
                else {

                    var _vehTrackCoordinates = [
                        new google.maps.LatLng(_InfoboxContent[_counter].latitude, _InfoboxContent[_counter].longitude),
                        new google.maps.LatLng(_InfoboxContent[_counter - 1].latitude, _InfoboxContent[_counter - 1].longitude)
                    ];
                }

                _RoutePath = new google.maps.Polyline({
                    path: _vehTrackCoordinates,
                    strokeColor: PathColor,//"#CC3300",
                    strokeOpacity: PathOpacity,//80,
                    strokeWeight: PathWeight,//2,
                    icons: [{

                        icon: {
                            path: google.maps.SymbolPath.BACKWARD_CLOSED_ARROW,
                            scale: 2,
                            //rotation: heading,
                            strokeColor: PathColor,
                            fillColor: PathColor,
                            fillOpacity: 3
                        },
                        repeat: '150px',
                        path: _vehTrackCoordinates
                    }]
                });

                if (IsPanEnable = 1) {
                    _map.panTo(new google.maps.LatLng(_InfoboxContent[_counter].latitude,
                        _InfoboxContent[_counter].longitude));
                }

                _RoutePath.setMap(_map);
            }
        }
    }///////////////////////////////

    //if (InfoboxContent.length > 0) {
    //    var endlenth = InfoboxContent.length;
    //    _markerArray[0].setAnimation(google.maps.Animation.BOUNCE);
    //    _markerArray[endlenth - 1].setAnimation(google.maps.Animation.BOUNCE);
    //    _map.setCenter(new google.maps.LatLng(InfoboxContent[InfoboxContent.length - 1].Lat, InfoboxContent[InfoboxContent.length - 1].Long));
    //}

    //} else {
    // alert("Marker list not pass !");
    //}


    _counter++;

}

/*Plays Route Without Markers END*//*Function to load from play WithoutMarker*/
function CreateMarkerWithInfoBoxForLast(InfoboxContent, bOpenNewInfoBox, bClosePreviousInfoBox) {

    let icon = (typeof (InfoboxContent.Icon) != "undefined") ? InfoboxContent.Icon : InfoboxContent.icon;

    let lat = (typeof (InfoboxContent.Lat) != "undefined") ? InfoboxContent.Lat :
        (typeof (InfoboxContent.lat) != "undefined") ? InfoboxContent.lat :
            (typeof (InfoboxContent.Latitute) != "undefined") ? InfoboxContent.Latitute :
                (typeof (InfoboxContent.latitute) != "undefined") ? InfoboxContent.latitute :
                    (typeof (InfoboxContent.Latitude) != "undefined") ? InfoboxContent.Latitude : InfoboxContent.latitude;

    let long = (typeof (InfoboxContent.Long) != "undefined") ? InfoboxContent.Long :
        (typeof (InfoboxContent.long) != "undefined") ? InfoboxContent.long :
            (typeof (InfoboxContent.Longitute) != "undefined") ? InfoboxContent.Longitute :
                (typeof (InfoboxContent.longitute) != "undefined") ? InfoboxContent.longitute :
                    (typeof (InfoboxContent.Longitude) != "undefined") ? InfoboxContent.Longitude : InfoboxContent.longitude;


    //////debugger
    _marker = new google.maps.Marker({
        map: _map,
        icon: '/App_Images/' + icon,//InfoboxContent.Icon,
        position: new google.maps.LatLng(lat, long)
        //position: new google.maps.LatLng(InfoboxContent.Lat, InfoboxContent.Long)
    });


    if (_infowindow && (bClosePreviousInfoBox == "1" || bClosePreviousInfoBox == "")) {
        _infowindow.close();
    }
    var livetrack = "display:none;";
    if (InfoboxContent.Status == 'P') {
        livetrack = "display:inline-block;"
    }
    _infowindow = new google.maps.InfoWindow();

    _content = "<div class='titleHeader'></div><div class='popup'><table class='info-table' width='100%'><tr align='left'><td colspan='2'><b>"
        + InfoboxContent.RegistrationNo + "</b></td><td><b>"
        + InfoboxContent.ModelName + "</b></td></tr><tr nowrap><td width='90px' align='left'><b>Driver Name:</b></td><td align='left'>"
        + InfoboxContent.DriverName + "</td></tr><tr><td  align='left'><b>Driver Mob:</b></td><td align='left'>"
        + InfoboxContent.DriverMobileNo + "</td> </tr><tr><td  align='left'><b>Recorded :</b></td><td align='left'>"
        + InfoboxContent.DeviceDateTime + "</td> </tr><tr><td align='left'><b>Speed :</b></td><td align='left'>"
        + InfoboxContent.Speed + " Km/Hr</td> </tr><tr><td align='left'><b>Device No. :</b></td><td align='left'>"
        + InfoboxContent.DeviceNo + "</td></tr>" //<tr><td align='left'><b>IMEI No. :</b></td><td align='left'>"
        //+ InfoboxContent.IMEINo + "</td> </tr><tr><td align='left'><b>Sim No. :</b></td><td align='left'>"
        //+ InfoboxContent.SIMNo
        //+ "</td></tr>"
        + '<tr><td align="left"></td><td align="left"><a style=' + livetrack + '  href="#" onclick="SendLiveTracking(\'' + InfoboxContent.RegistrationNo + '\'' + ',' + InfoboxContent.FK_CompanyId + ');">Live Tracking</a>'
        + "</td></tr> </table></div>";
    //  + "<tr><td align='left'></td><td align='left'><a href='#' onclick='SendLiveTracking(''+RegNo,CompId)'>Live Tracking</a>"
    google.maps.event.addListener(_marker, 'click', (function (_marker, _content) {
        return function () {
            _infowindow.setContent(_content);
            _infowindow.open(_map, _marker);
        }
    })(_marker, _content));

    if (bOpenNewInfoBox == "1") {
        _infowindow.setContent(_content);
        _infowindow.open(_map, _marker);
    }
}


/****************************GOOG MAP UTILITY FINCTIONS STARTS****************************/

/*CREATED BY: TARIQUE - Removes All Object On Map*/
function ClearMap() {
    removeLine();
    setMapOnAll(null);
}

/*CREATED BY: TARIQUE - Called From ClearMap() Function */
function removeLine() {
    if (typeof _vehPath === 'undefined') { } else {
        _vehPath.setMap(null);
    }
}

/*CREATED BY: TARIQUE - Called From ClearMap() Function */
function setMapOnAll(map) {

    if (typeof _markerArray === 'undefined') { } else {
        for (var i = 0; i < _markerArray.length; i++) {
            _markerArray[i].setMap(map);
        }

        _markerArray = [];
    }
}
/*CREATED BY: TARIQUE - Called From ClearMap() Function */
function ClearRoute() {

    if (typeof _RoutePath != 'undefined') {
        _RoutePath.setMap(null);
    }
    setMapOnAll(null);
    //if (typeof _vehPath === 'undefined') { } else {
    //    _vehPath.setMap(null);
    //}
}

/****************************GOOG MAP UTILITY FINCTIONS ENDS****************************/

/*CREATED BY: TARIQUE - Called From Link Vehicle Marker's Infobox */
function SendLiveTracking(_FK_VehicleId, _AccountId, _CustomerId) {

    $.ajax({
        url: '../TrackOnMap/LiveTracking',
        type: "GET",
        dataType: "JSON",
        data: { AccountId: _AccountId, CustomerId: _CustomerId, FK_VehicleId: _FK_VehicleId },
        success: function (r) {
            //alert(r);
            if (r == 1) {
                window.location.href = "../TrackOnMap/Index";
            }
        }
    });
}


/*CREATED BY: TARIQUE - CREARS THE INTERVAL */
function Pause() {
    clearTimeout(_SetInterval);
    // ClearMap();
}



/*CREATE MARKER WITH ICON*/
//This method is called inside "initMap()" method to add new markers.

function Addmarkers(latLongArray, MarkerTitle, imagePath) {
    //////debugger
    if (latLongArray != null && latLongArray.length > 0) {

        for (let i = 0; i < latLongArray.length; i++) {
            var marker = new google.maps.Marker({
                position: new google.maps.LatLng(latLongArray[i].latitude, latLongArray[i].longitude),
                title: MarkerTitle,
                draggable: false,
                map: _map,
                icon: imagePath
            });
        }
    }


}

function AddSingleMarker(Lat, Long, ImagePath, GeofenceName, GeoAddress, LatLongCoordinate, Radius) {
//  ////debugger
        var latlng = new google.maps.LatLng(Lat, Long);
        bounds.extend(latlng);
        var image = {
         //   url: '/Images/POI/' + ImageName, //imagePath,
            url: ImagePath, //imagePath,
            scaledSize: new google.maps.Size(32, 30), // scaled icon size :: Added By Sarfaraz on   //32, 30
        };
        var marker = new google.maps.Marker({
            position: new google.maps.LatLng(latlng),
            //  title: MarkerTitle,
            draggable: false,
            map: _map,
            icon: image
           
        });
        _infowindow = new google.maps.InfoWindow();
        var Radiusstr = "";
        if (Radius != "") //Circle
          
        {
            _content = "<div class='popup' style='width:50px, height:50px'>"
                   //+ "<div class='titleHeaderFor'></div>"
                   + "<div class='popup'>"// +"<div class='popup'>"
                   + "<table class='info-table' width='100%'>"
                   + "<tr align='left'>" // ---------------------------------------//1ST TABLE ROW START
                   + "<td><image height=22px width=22px src=" + ImagePath + "></image><b>" + GeofenceName + "</b></td>"
                   + "</tr>"// ---------------------------------------//1ST TABLE ROW END

                   + "<tr align='left'>"  // ---------------------------------------//2ND TABLE ROW START
                   + "<td>" + GeoAddress + "<br>" + LatLongCoordinate + "<br>"
                   + Radius + " Mtr" + "<br>" + "Radius" + "</td>"  //"<td width='90px' align='left'><b>Geofence Name:</b></td>"
                   + "</tr>"; // ---------------------------------------//2ND TABLE ROW END

            +"</table>"
            + "</div>"
            + "</div>";

        }
        else {   // Polygon
            _content = "<div class='popup' style='width:50px, height:50px'>"
                // + "<div class='titleHeaderFor'></div>"
                 + "<div class='popup'>"// +"<div class='popup'>"
                 + "<table class='info-table' width='100%'>"
                 + "<tr align='left'>" // ---------------------------------------//1ST TABLE ROW START
                 + "<td><image height=22px width=22px src=" + ImagePath + "></image><b>" + GeofenceName + "</b></td>"
                 + "</tr>"// ---------------------------------------//1ST TABLE ROW END

                 + "<tr align='left'>"  // ---------------------------------------//2ND TABLE ROW START
                 + "<td>" + GeoAddress + "<br>" + LatLongCoordinate + "<br>"
                 //+ (Radius != "" ? +Radius + " km" : "") + "<br>" + "Radius"
                 + "</td>"  //"<td width='90px' align='left'><b>Geofence Name:</b></td>"
                 + "</tr>"; // ---------------------------------------//2ND TABLE ROW END

            +"</table>"
            + "</div>"
            + "</div>";
        }
        
 
        google.maps.event.addListener(marker, 'click', (function (marker, _content) {
            return function () {
                _infowindow.setContent("<b>" + _content + "</b>"); // set content
                _infowindow.open(_map, marker); // open at marker's location
            }
        })(marker, _content));
        google.maps.event.addListener(_map, 'click', function () {
            _infowindow.close(_map, marker);
        });
        //google.maps.event.addListener(marker, '', function () {
        //    _infowindow.close();
        //});
        _geopoiArray.push(marker);

        //var loc;
        //loc = new google.maps.LatLng(5.451060, 100.305520);
        //bounds.extend(loc)
        //_map.fitBounds(bounds);
        //_map.panToBounds(bounds);
        //_map.setCenter(loc);
}

/*start*/
function DrawMarkerForRefillvsDrain(MapControlID, Zoom, CenterLat, CenterLng, InfoboxContent) {
    ////debugger
    try {

        var iVar = 0;
        if (_markerArray.length > 0) {
            markerCluster.removeMarkers(_markerArray);
        }

        ClearMap();

        if (InfoboxContent.length != 0) {
            for (var i = 0; i <= InfoboxContent.length - 1; i++) {
                CreateMarkerWithInfoBoxForRefillvsDrain(InfoboxContent[i])
                loc = new google.maps.LatLng(InfoboxContent[i].latitude, InfoboxContent[i].longitude);
                bounds.extend(loc)
            }

            //  START :: ADD BY : HRITIK GHOSH :: DATE : 04/07/2024 :: PURPOSE : MARKER CLASTER FOR MAP

            //markerCluster = new MarkerClusterer(_map, _markerArray, { imagePath: 'https://developers.google.com/maps/documentation/javascript/examples/markerclusterer/m' });
            
            markerCluster = new markerClusterer.MarkerClusterer({
                map: _map,
                markers: _markerArray,
            });

            /*markerCluster.addMarkers(_markerArray);*/

             //  END :: ADD BY : HRITIK GHOSH :: DATE : 04/07/2024 :: PURPOSE : MARKER CLASTER FOR MAP


            chkZommChange = false;
            chkZoom = false;
            if (chkZoom == false) {
                chkZommChange = true;
                lastzoom = _map.zoom;
                _map.fitBounds(bounds);
                //_map.panToBounds(bounds);
            }
            else {
                chkZommChange = false;
                lastzoom = 0;
                _map.fitBounds(bounds);//Commented By Vinish 07052020
                _map.panToBounds(bounds);
            }

            if (InfoboxContent.length == 1) {
                //Added By Shivam Saluja as Suggested by ayan on 19 May 2020
                var loc1 = new google.maps.LatLng(InfoboxContent[0].latitude, InfoboxContent[0].longitude);
                var bounds1 = new google.maps.LatLngBounds();

                bounds1.extend(loc1)
                _map.fitBounds(bounds1);
                _map.panToBounds(bounds1);
                _map.setZoom(18);
            }

        } else {
            // alert("Marker list not pass !");
        }
    } catch (Error) {
        // alert("Problem in Draw Marker :" + Error.message);
    }
}

/*end*/


/* START :: Created By: Hritik Ghosh  */

function DrawMarkerForFuelLog(MapControlID, Zoom, CenterLat, CenterLng, InfoboxContent) {
    //debugger;
    try {

        var iVar = 0;
        if (_markerArray.length > 0) {
            markerCluster.removeMarkers(_markerArray);
        }

        ClearMap();

        if (InfoboxContent.length != 0) {
            for (var i = 0; i <= InfoboxContent.length - 1; i++) {
                CreateMarkerWithInfoBoxForFuelLog(InfoboxContent[i])
                loc = new google.maps.LatLng(InfoboxContent[i].lattitude, InfoboxContent[i].longitude);
                bounds.extend(loc)
            }

            //  START :: ADD BY : HRITIK GHOSH :: DATE : 04/07/2024 :: PURPOSE : MARKER CLASTER FOR MAP

            //markerCluster = new MarkerClusterer(_map, _markerArray, { imagePath: 'https://developers.google.com/maps/documentation/javascript/examples/markerclusterer/m' });

            markerCluster = new markerClusterer.MarkerClusterer({
                map: _map,
                markers: _markerArray,
            });

            /*markerCluster.addMarkers(_markerArray);*/

             //  END :: ADD BY : HRITIK GHOSH :: DATE : 04/07/2024 :: PURPOSE : MARKER CLASTER FOR MAP


            chkZommChange = false;
            chkZoom = false;
            if (chkZoom == false) {
                chkZommChange = true;
                lastzoom = _map.zoom;
                _map.fitBounds(bounds);
                //_map.panToBounds(bounds);
            }
            else {
                chkZommChange = false;
                lastzoom = 0;
                _map.fitBounds(bounds);//Commented By Vinish 07052020
                _map.panToBounds(bounds);
            }

            if (InfoboxContent.length == 1) {
                //Added By Shivam Saluja as Suggested by ayan on 19 May 2020
                var loc1 = new google.maps.LatLng(InfoboxContent[0].lattitude, InfoboxContent[0].longitude);
                var bounds1 = new google.maps.LatLngBounds();

                bounds1.extend(loc1)
                _map.fitBounds(bounds1);
                _map.panToBounds(bounds1);
                _map.setZoom(18);
            }

        } else {
            // alert("Marker list not pass !");
        }
    } catch (Error) {
        // alert("Problem in Draw Marker :" + Error.message);
    }
}


function DrawMarkerForDailyRefuelAndDrain(MapControlID, Zoom, CenterLat, CenterLng, InfoboxContent) {
    //debugger;
    try {

        var iVar = 0;
        if (_markerArray.length > 0) {
            markerCluster.removeMarkers(_markerArray);
        }

        ClearMap();

        if (InfoboxContent.length != 0) {
            for (var i = 0; i <= InfoboxContent.length - 1; i++) {
                CreateMarkerWithInfoBoxForDailyRefuelAndDrain(InfoboxContent[i])
                loc = new google.maps.LatLng(InfoboxContent[i].lattitude, InfoboxContent[i].longitude);
                bounds.extend(loc)
            }

            //  START :: ADD BY : HRITIK GHOSH :: DATE : 04/07/2024 :: PURPOSE : MARKER CLASTER FOR MAP

            //markerCluster = new MarkerClusterer(_map, _markerArray, { imagePath: 'https://developers.google.com/maps/documentation/javascript/examples/markerclusterer/m' });

            markerCluster = new markerClusterer.MarkerClusterer({
                map: _map,
                markers: _markerArray,
            });

            /*markerCluster.addMarkers(_markerArray);*/

             //  END :: ADD BY : HRITIK GHOSH :: DATE : 04/07/2024 :: PURPOSE : MARKER CLASTER FOR MAP


            chkZommChange = false;
            chkZoom = false;
            if (chkZoom == false) {
                chkZommChange = true;
                lastzoom = _map.zoom;
                _map.fitBounds(bounds);
                //_map.panToBounds(bounds);
            }
            else {
                chkZommChange = false;
                lastzoom = 0;
                _map.fitBounds(bounds);//Commented By Vinish 07052020
                _map.panToBounds(bounds);
            }

            if (InfoboxContent.length == 1) {
                //Added By Shivam Saluja as Suggested by ayan on 19 May 2020
                var loc1 = new google.maps.LatLng(InfoboxContent[0].lattitude, InfoboxContent[0].longitude);
                var bounds1 = new google.maps.LatLngBounds();

                bounds1.extend(loc1)
                _map.fitBounds(bounds1);
                _map.panToBounds(bounds1);
                _map.setZoom(18);
            }

        } else {
            // alert("Marker list not pass !");
        }
    } catch (Error) {
        // alert("Problem in Draw Marker :" + Error.message);
    }
}

/* END :: Created By: Hritik Ghosh  */


/* START :: Created By: Hritik Ghosh  */


function CreateMarkerWithInfoBoxForFuelLog(InfoboxContent, bOpenNewInfoBox, bClosePreviousInfoBox) {
    //debugger
    //added new
    // var latLng = new google.maps.LatLng(49.47805, -123.84716);
    // var homeLatLng = new google.maps.LatLng(49.47805, -123.84716);

    //  var pictureLabel = document.createElement("img");
    //  pictureLabel.src = "home.jpg";

    var _marker = new MarkerWithLabel({
        position: new google.maps.LatLng(InfoboxContent.lattitude, InfoboxContent.longitude),
        map: _map,
        draggable: false,
        raiseOnDrag: false,
        // labelContent: InfoboxContent.RegistrationNo,
        labelInBackground: true,
        labelAnchor: new google.maps.Point(-18, 52),
        labelClass: "labels",
        //labelStyle: { opacity: 0.75 }
    });
    if (InfoboxContent.event == 'Refuel') {
        _marker.setIcon('/App_Images/' + 'Refill.png');
    }
    else { _marker.setIcon('/App_Images/' + 'Drain.png'); }
    //  _marker.setIcon('/Images/' + 'flag-start.png');  

    if (_infowindow && (bClosePreviousInfoBox == "1" || bClosePreviousInfoBox == "")) {
        _infowindow.close();
    }
    var livetrack = "display:none;";

    if (InfoboxContent.Status == 'P') {
        livetrack = "display:inline-block;"
    }
    else {
        livetrack = "display:inline-block;"
    }

    _infowindow = new google.maps.InfoWindow();

    var HeaderValue = InfoboxContent.vehicleNo + "</br>" + InfoboxContent.accountName;
    var Navigationurl = "https://www.google.com/maps/dir//" + InfoboxContent.lattitude + "," + InfoboxContent.longitude;

    _content = "<div class='titleHeader'>" + HeaderValue + "<div style='position: absolute; top: 5px; right: 45px;'><a href='" + Navigationurl + "' target='_blank'><img src='../Images/navigationPopup.png' height='30'></a></div>" +'<div style="position: absolute;top: 0px;right: 5px;"><button draggable="false" aria-label="Close" title="Close" type="button" class="gm-ui-hover-effect" style="background: none;border: 0;margin: 0;padding:0;cursor:pointer;user-select: none;width: 28px;height: 28px;position: absolute;top: 10px;right: 10px;" onClick="closeInfoWindow();"><span style="mask-image: url(&quot;data:image/svg+xml,%3Csvg%20xmlns%3D%22http%3A//www.w3.org/2000/svg%22%20viewBox%3D%220%200%2024%2024%22%3E%3Cpath%20d%3D%22M19%206.41L17.59%205%2012%2010.59%206.41%205%205%206.41%2010.59%2012%205%2017.59%206.41%2019%2012%2013.41%2017.59%2019%2019%2017.59%2013.41%2012z%22/%3E%3Cpath%20d%3D%22M0%200h24v24H0z%22%20fill%3D%22none%22/%3E%3C/svg%3E&quot;);pointer-events: none;display: block;width: 22px;height: 22px;margin: 3px;top: 2px;"></span></button></div>'+ "</div><div class='popup'>"
        + "<table class='info-table' style='width:250px;' width='100%'><tr align='left'><td  align='left'><b>DateTime</b></td><td><b>:</b></td><td align='left'>"
        + InfoboxContent.dateTime + "</td> </tr><tr><td  align='left'><b>Event</b></td><td><b>:</b></td><td align='left'>"
        + InfoboxContent.event + "</td> </tr><tr><td align='left'><b>Volume</b></td><td><b>:</b></td><td align='left'>"
        + InfoboxContent.fuelVolume + ' (Current Tank ' + InfoboxContent.fuelAfter + ')' + "</td> </tr><tr><td  align='left'><b>Address</b></td><td><b>:</b></td><td align='left'>"
        + InfoboxContent.location

        + "</td></tr>"
        + '<tr><td align="left"></td><td></td><td align="left">'
        + "</td></tr> </table></div>";

    google.maps.event.addListener(_marker, 'click', (function (_marker, _content) {
        return function () {
            _infowindow.setContent(_content);
            _infowindow.open(_map, _marker);
        }
    })(_marker, _content));

    if (bOpenNewInfoBox == "1") {
        _infowindow.setContent(_content);
        _infowindow.open(_map, _marker);
    }
    _markerArray.push(_marker);

}

/* START :: Add By Hritik : Use for Close Info Window In maps : Date: 30/05/2024 */

function closeInfoWindow() {
    //debugger
    _infowindow.close();
}

/* END :: Add By Hritik : Use for Close Info Window In maps : Date: 30/05/2024 */

function CreateMarkerWithInfoBoxForDailyRefuelAndDrain(InfoboxContent, bOpenNewInfoBox, bClosePreviousInfoBox) {
    //debugger
    //added new
    // var latLng = new google.maps.LatLng(49.47805, -123.84716);
    // var homeLatLng = new google.maps.LatLng(49.47805, -123.84716);

    //  var pictureLabel = document.createElement("img");
    //  pictureLabel.src = "home.jpg";

    var _marker = new MarkerWithLabel({
        position: new google.maps.LatLng(InfoboxContent.lattitude, InfoboxContent.longitude),
        map: _map,
        draggable: false,
        raiseOnDrag: false,
        // labelContent: InfoboxContent.RegistrationNo,
        labelInBackground: true,
        labelAnchor: new google.maps.Point(-18, 52),
        labelClass: "labels",
        //labelStyle: { opacity: 0.75 }
    });
    if (InfoboxContent.event == 'Refuel') {
        _marker.setIcon('/App_Images/' + 'Refill.png');
    }
    else { _marker.setIcon('/App_Images/' + 'Drain.png'); }
    //  _marker.setIcon('/Images/' + 'flag-start.png');  

    if (_infowindow && (bClosePreviousInfoBox == "1" || bClosePreviousInfoBox == "")) {
        _infowindow.close();
    }
    var livetrack = "display:none;";

    if (InfoboxContent.Status == 'P') {
        livetrack = "display:inline-block;"
    }
    else {
        livetrack = "display:inline-block;"
    }

    _infowindow = new google.maps.InfoWindow();

    var HeaderValue = InfoboxContent.vehicleNo + "</br>" + InfoboxContent.accountName;
    var Navigationurl = "https://www.google.com/maps/dir//" + InfoboxContent.lattitude + "," + InfoboxContent.longitude;

    _content = "<div class='titleHeader'>" + HeaderValue + "<div style='position: absolute; top: 5px; right: 45px;'><a href='" + Navigationurl + "' target='_blank'><img src='../Images/navigationPopup.png' height='30'></a></div>" + '<div style="position: absolute;top: 0px;right: 5px;"><button draggable="false" aria-label="Close" title="Close" type="button" class="gm-ui-hover-effect" style="background: none;border: 0;margin: 0;padding:0;cursor:pointer;user-select: none;width: 28px;height: 28px;position: absolute;top: 10px;right: 10px;" onClick="closeInfoWindow();"><span style="mask-image: url(&quot;data:image/svg+xml,%3Csvg%20xmlns%3D%22http%3A//www.w3.org/2000/svg%22%20viewBox%3D%220%200%2024%2024%22%3E%3Cpath%20d%3D%22M19%206.41L17.59%205%2012%2010.59%206.41%205%205%206.41%2010.59%2012%205%2017.59%206.41%2019%2012%2013.41%2017.59%2019%2019%2017.59%2013.41%2012z%22/%3E%3Cpath%20d%3D%22M0%200h24v24H0z%22%20fill%3D%22none%22/%3E%3C/svg%3E&quot;);pointer-events: none;display: block;width: 22px;height: 22px;margin: 3px;top: 2px;"></span></button></div>' + "</div><div class='popup'>"
        + "<table class='info-table' style='width:250px;' width='100%'><tr align='left'><td  align='left'><b>DateTime</b></td><td><b>:</b></td><td align='left'>"
        + InfoboxContent.fuelLevelEnd + "</td> </tr><tr><td  align='left'><b>Event</b></td><td><b>:</b></td><td align='left'>"
        + InfoboxContent.event + "</td> </tr><tr><td align='left'><b>Volume</b></td><td><b>:</b></td><td align='left'>"
        + InfoboxContent.fuelVolume + ' (Current Tank ' + InfoboxContent.fuelAfter + ')' + "</td> </tr><tr><td  align='left'><b>Address</b></td><td><b>:</b></td><td align='left'>"
        + InfoboxContent.location

        + "</td></tr>"
        + '<tr><td align="left"></td><td></td><td align="left">'
        + "</td></tr> </table></div>";

    google.maps.event.addListener(_marker, 'click', (function (_marker, _content) {
        return function () {
            _infowindow.setContent(_content);
            _infowindow.open(_map, _marker);
        }
    })(_marker, _content));

    if (bOpenNewInfoBox == "1") {
        _infowindow.setContent(_content);
        _infowindow.open(_map, _marker);
    }
    _markerArray.push(_marker);

}

/* END :: Created By: Hritik Ghosh  */





function CreateMarkerWithInfoBoxFUEL(InfoboxContent, bOpenNewInfoBox, bClosePreviousInfoBox) {



    //  ////debugger
    //InfoboxContent.FK_CompanyId


    // var img='http://chart.apis.google.com/chart?chst=d_map_pin_letter&chld='+A|FF0000|000000'
    _marker = new google.maps.Marker({
        map: _map,
        icon: '/App_Images/' + InfoboxContent.Icon,

        position: new google.maps.LatLng(InfoboxContent.Lat, InfoboxContent.Long)
    });


    if (_infowindow && (bClosePreviousInfoBox == "1" || bClosePreviousInfoBox == "")) {
        _infowindow.close();
    }
    var livetrack = "display:none;";
    if (InfoboxContent.Status == 'P') {
        livetrack = "display:inline-block;"
    }

    _infowindow = new google.maps.InfoWindow();

    //_content = "<div class='popup'><table class='info-table'>"
    //    + "<tr align='left'>"
    //    + "<td colspan='2'><b>" + InfoboxContent.RegistrationNo + "</b></td>"
    //    //+ "<td><b>" + InfoboxContent.ModelName + "</b></td>"
    //    +"</tr>"
    //    +"<tr nowrap><td width='90px' align='left'><b>Driver:</b></td><td align='left'>"+ InfoboxContent.ChauffeurName + "</td></tr>"
    //    +"<tr><td  align='left'><b>Driver Mobile:</b></td><td align='left'>"+ InfoboxContent.ChauffeurMob + "</td> </tr>"
    //    +"<tr><td  align='left'><b>Refill :</b></td><td align='left'>"+ InfoboxContent.DeviceDateTime + "</td> </tr>"
    //    +"<tr><td align='left'><b>Refill Start :</b></td><td align='left'>" + InfoboxContent.Speed + " </td> </tr>"
    //    +"<tr><td align='left'><b>Refill End :</b></td><td align='left'>"+ InfoboxContent.Location + " </td> </tr>"
    //    + "<tr><td align='left'><b>Location</b></td><td align='left'>" + InfoboxContent.Location + " </td> </tr>"
    //    + "<tr><td align='left'><b>Price</b></td><td align='left'>" + InfoboxContent.Price + "</td></tr>"
    //    + "</table></div>";

    _content = "<div class='popup'><table class='info-table'>"
        + "<tr align='left'>"
        + "<td colspan='2'><b>" + InfoboxContent.RegistrationNo + "</b></td>"
        //+ "<td><b>" + InfoboxContent.ModelName + "</b></td>"
        + "</tr>"
        + "<tr nowrap><td width='90px' align='left'><b>Driver:</b></td><td align='left'>" + InfoboxContent.ChauffeurName + "</td></tr>"
        + "<tr><td  align='left'><b>Driver Mobile:</b></td><td align='left'>" + InfoboxContent.ChauffeurMob + "</td> </tr>"
        + "<tr><td  align='left'><b>Device No. :</b></td><td align='left'>" + InfoboxContent.DeviceNo + "</td> </tr>"
        + "<tr><td align='left'><b>Device Date Time :</b></td><td align='left'>" + InfoboxContent.DeviceDateTime + " </td> </tr>"
        + "<tr><td align='left'><b>Location :</b></td><td align='left'>" + InfoboxContent.Location + " </td> </tr>"
        + "<tr><td align='left'><b>Speed</b></td><td align='left'>" + InfoboxContent.Speed + " </td> </tr>"
        + "<tr><td align='left'><b>Price</b></td><td align='left'>" + InfoboxContent.Fuel_price + "</td></tr>"
        + "</table></div>";

    google.maps.event.addListener(_marker, 'click', (function (_marker, _content) {
        return function () {
            _infowindow.setContent(_content);
            _infowindow.open(_map, _marker);
        }
    })(_marker, _content));

    if (bOpenNewInfoBox == "1") {
        _infowindow.setContent(_content);
        _infowindow.open(_map, _marker);
    }

    _markerArray.push(_marker);

}
function CreateMarkerWithInfoBoxForRefillvsDrain(InfoboxContent, bOpenNewInfoBox, bClosePreviousInfoBox) {
    //////debugger
    //added new
    // var latLng = new google.maps.LatLng(49.47805, -123.84716);
    // var homeLatLng = new google.maps.LatLng(49.47805, -123.84716);

    //  var pictureLabel = document.createElement("img");
    //  pictureLabel.src = "home.jpg";

    var _marker = new MarkerWithLabel({
        position: new google.maps.LatLng(InfoboxContent.latitude, InfoboxContent.longitude),
        map: _map,
        draggable: false,
        raiseOnDrag: false,
        // labelContent: InfoboxContent.RegistrationNo,
        labelInBackground: true,
        labelAnchor: new google.maps.Point(-18, 52),
        labelClass: "labels",
        //labelStyle: { opacity: 0.75 }
    });
    if (InfoboxContent.event == 'Refill') {
        _marker.setIcon('/App_Images/' + 'Refill.png');
    }
    else { _marker.setIcon('/App_Images/' + 'Drain.png'); }
    //  _marker.setIcon('/Images/' + 'flag-start.png');  

    if (_infowindow && (bClosePreviousInfoBox == "1" || bClosePreviousInfoBox == "")) {
        _infowindow.close();
    }
    var livetrack = "display:none;";

    if (InfoboxContent.Status == 'P') {
        livetrack = "display:inline-block;"
    }
    else {
        livetrack = "display:inline-block;"
    }

    _infowindow = new google.maps.InfoWindow();

    var HeaderValue = InfoboxContent.regNO + "</br>" + InfoboxContent.companyName;
    var Navigationurl = "https://www.google.com/maps/dir//" + InfoboxContent.latitude + "," + InfoboxContent.longitude;

    _content = "<div class='titleHeader'>" + HeaderValue + "<div style='position: absolute; top: 5px; right: 45px;'><a href='" + Navigationurl + "' target='_blank'><img src='../Images/navigationPopup.png' height='30'></a></div>" + "</div><div class='popup'>"
        + "<table class='info-table' style='width:250px;' width='100%'><tr align='left'><td  align='left'><b>DateTime</b></td><td><b>:</b></td><td align='left'>"
        + InfoboxContent.time + "</td> </tr><tr><td  align='left'><b>Event</b></td><td><b>:</b></td><td align='left'>"
        + InfoboxContent.event + "</td> </tr><tr><td align='left'><b>Volume</b></td><td><b>:</b></td><td align='left'>"
        + InfoboxContent.value + ' (Current Tank ' + InfoboxContent.after + 'L)' + "</td> </tr><tr><td  align='left'><b>Address</b></td><td><b>:</b></td><td align='left'>"
        + InfoboxContent.address

        + "</td></tr>"
        + '<tr><td align="left"></td><td></td><td align="left">'
        + "</td></tr> </table></div>";

    google.maps.event.addListener(_marker, 'click', (function (_marker, _content) {
        return function () {
            _infowindow.setContent(_content);
            _infowindow.open(_map, _marker);
        }
    })(_marker, _content));

    if (bOpenNewInfoBox == "1") {
        _infowindow.setContent(_content);
        _infowindow.open(_map, _marker);
    }
    _markerArray.push(_marker);

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


