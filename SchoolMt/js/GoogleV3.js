/*
   Created By  : ANJANI SINGH/Deepak Singh
   Created Date: 2015-12-22
   Purpose     : Google Map 
*/

/* Global Variable */
var _mapOptions;
var _map;
var _marker;
var _markerArray = [];
var _infowindow;
var _content;
var _InfoboxContent = [];
var _regNo = '';
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

/**
 * The CenterControl adds a control to the map that recenters the map on
 * Chicago.
 * @constructor
 * @param {!Element} controlDiv
 * @param {!google.maps.Map} map
 * @param {?google.maps.LatLng} center
 */
function CenterControl(controlDiv, map, center) {
    // We set up a variable for this since we're adding event listeners
    // later.
    var control = this;

    // Set the center property upon construction
    control.center_ = center;
    controlDiv.style.clear = 'both';

    // Set CSS for the control border
    var goCenterUI = document.createElement('div');
    goCenterUI.id = 'goCenterUI';
    //goCenterUI.title = 'Click to recenter the map';
    controlDiv.appendChild(goCenterUI);

    // Set CSS for the control interior
    var goCenterText = document.createElement('img');
    goCenterText.id = 'goCenterText';
    goCenterText.src = '/images/Traffic1.png';
    goCenterText.innerHTML = 'Center Map';
    goCenterUI.appendChild(goCenterText);

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

    // Set up the click event listener for 'Center Map': Set the center of
    // the map
    // to the current center of the control.
    goCenterUI.addEventListener('click', function () {
        //var currentCenter = control.getCenter();
        //map.setCenter(currentCenter);
        toggleTraffic();
    });

    // Set up the click event listener for 'Set Center': Set the center of
    // the control to the current center of the map.
    setCenterUI.addEventListener('click', function () {
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



//Code by Anjani for icon rotation
var RotateIcon = function (options) {
    this.options = options || {};
    this.rImg = options.img || new Image();
    this.rImg.src = this.rImg.src || this.options.url;
    this.options.width = this.options.width || this.rImg.width || 52;
    this.options.height = this.options.height || this.rImg.height || 60;
    canvas = document.createElement("canvas");
    canvas.width = this.options.width;
    canvas.height = this.options.height;
    this.context = canvas.getContext("2d");
    this.canvas = canvas;
};
RotateIcon.makeIcon = function (url) {
    return new RotateIcon({ url: url });
};
RotateIcon.prototype.setRotation = function (options) {
    var canvas = this.context,
    angle = options.deg
        ? options.deg * Math.PI / 180 :
            options.rad,
        centerX = this.options.width / 2,
        centerY = this.options.height / 2;

    canvas.clearRect(0, 0, this.options.width, this.options.height);
    canvas.save();
    canvas.translate(centerX, centerY);
    canvas.rotate(angle);
    canvas.translate(-centerX, -centerY);
    canvas.drawImage(this.rImg, 0, 0);
    canvas.restore();
    return this;

};
RotateIcon.prototype.getUrl = function () {
    return this.canvas.toDataURL('image/png');
};
//End of code by anjani

/*Function to load blank map*/
function initMap(MapControlID, Zoom, CenterLat, CenterLng, IsTraffic) {
    //debugger
    try {
        _mapOptions = {
            zoom: parseInt(Zoom),
            center: { lat: CenterLat, lng: CenterLng },
            disableDefaultUI: false,
            draggable: true,
            scrollwheel: true,
            mapTypeId: google.maps.MapTypeId.ROADMAP
        }
        _map = new google.maps.Map(document.getElementById(MapControlID),
             _mapOptions);

        if (IsTraffic) {
            // Create the DIV to hold the control and call the CenterControl()
            // constructor
            // passing in this DIV.
            var centerControlDiv = document.createElement('div');
            var centerControl = new CenterControl(centerControlDiv, _map, { lat: CenterLat, lng: CenterLng });

            centerControlDiv.index = 1;
            centerControlDiv.id = 'trafficdiv';
            // ; overflow: hidden; top: 53px; right: 0px;
            centerControlDiv.style['border'] = '0px none';
            centerControlDiv.style['margin'] = '10px';
            centerControlDiv.style['padding'] = '7px';
            centerControlDiv.style['background'] = 'rgb(255,255,255) none repeat scroll 0% 0%';
            centerControlDiv.style['top'] = '53px';
            centerControlDiv.style['right'] = '0px';
            centerControlDiv.style['position'] = 'absolute';
            centerControlDiv.style['cursor'] = 'pointer';
            centerControlDiv.style['user-select'] = 'none';
            centerControlDiv.style['border-radius'] = '2px';
            centerControlDiv.style['height'] = '40px';
            centerControlDiv.style['width'] = '40px';
            centerControlDiv.style['box-shadow'] = 'rgba(0, 0, 0, 0.3) 0px 1px 4px -1px';
            centerControlDiv.style['overflow'] = 'hidden';

            _map.controls[google.maps.ControlPosition.RIGHT_TOP].push(centerControlDiv);

            trafficLayer = new google.maps.TrafficLayer();
            google.maps.event.addDomListener(document.getElementById('trafficToggle'), 'click', toggleTraffic);

        }
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


    } catch (Error) {

        // alert("Problem while map initiaion :" + Error.message);
    }

}

/*Function to load markers*/
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

/*Function to load markers*/
function DrawMarkerWithoutAutofit(MapControlID, Zoom, CenterLat, CenterLng, InfoboxContent) {
    try {

        var iVar = 0;
        ClearMap();
        if (InfoboxContent.length != 0) {
            for (var i = 0; i <= InfoboxContent.length - 1; i++) {
                CreateMarkerWithInfoBox(InfoboxContent[i])
                //  loc = new google.maps.LatLng(InfoboxContent[i].Lat, InfoboxContent[i].Long);
                //   bounds.extend(loc)

            }
            //   if (IsfitBounds != false) {
            //    _map.fitBounds(bounds);
            //  _map.panToBounds(bounds);
            //  }
            //if (InfoboxContent.length > 0) {
            //    _map.setCenter(new google.maps.LatLng(InfoboxContent[0].Lat, InfoboxContent[0].Long));
            //}
            // _map.setZoom(Zoom);

            //_map.panTo(new google.maps.LatLng(InfoboxContent[0].Lat, InfoboxContent[0].Long));
        } else {
            // alert("Marker list not pass !");
        }
    } catch (Error) {
        // alert("Problem in Draw Marker :" + Error.message);
    }
}

/*Create Marker With InfoBox*/
function CreateMarkerWithInfoBox(InfoboxContent, bOpenNewInfoBox, bClosePreviousInfoBox) {



    //  debugger
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

    _content = "<div class='popup'><table class='info-table'><tr align='left'><td colspan='2'><b>"
    + InfoboxContent.RegistrationNo + "</b></td><td><b>"
    + InfoboxContent.ModelName + "</b></td></tr><tr nowrap><td width='90px' align='left'><b>Driver Name:</b></td><td align='left'>"
    + InfoboxContent.ChauffeurName + "</td></tr><tr><td  align='left'><b>Driver Mob:</b></td><td align='left'>"
    + InfoboxContent.ChauffeurMob + "</td> </tr><tr><td  align='left'><b>Recorded :</b></td><td align='left'>"
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

    _markerArray.push(_marker);

}

/*Create Penna Marker With InfoBox*/

function CreatePennaMarkerWithInfoBox(InfoboxContent, bOpenNewInfoBox, bClosePreviousInfoBox) {



    //  debugger
    //InfoboxContent.FK_CompanyId

    if (InfoboxContent.Icon == 'flag-start.png' || InfoboxContent.Icon == 'flag-end.png') {
        // var img='http://chart.apis.google.com/chart?chst=d_map_pin_letter&chld='+A|FF0000|000000'
        _marker = new google.maps.Marker({
            map: _map,
            icon: '/App_Images/' + InfoboxContent.Icon,
            position: new google.maps.LatLng(InfoboxContent.Lat, InfoboxContent.Long)
        });
    } else {
        _marker = new google.maps.Marker({
            map: _map,
            icon: '/App_Images/',// + InfoboxContent.Icon,
            position: new google.maps.LatLng(InfoboxContent.Lat, InfoboxContent.Long)
        });

    }


    if (_infowindow && (bClosePreviousInfoBox == "1" || bClosePreviousInfoBox == "")) {
        _infowindow.close();
    }
    var livetrack = "display:none;";
    if (InfoboxContent.Status == 'P') {
        livetrack = "display:inline-block;"
    }
    _infowindow = new google.maps.InfoWindow();

    _content = "<div class='popup'><table class='info-table'><tr align='left'><td colspan='2'><b>"
    + InfoboxContent.RegistrationNo + "</b></td><td><b>"
    + InfoboxContent.ModelName + "</b></td></tr><tr nowrap><td width='90px' align='left'><b>Driver Name:</b></td><td align='left'>"
    + InfoboxContent.ChauffeurName + "</td></tr><tr><td  align='left'><b>Driver Mob:</b></td><td align='left'>"
    + InfoboxContent.ChauffeurMob + "</td> </tr><tr><td  align='left'><b>Recorded :</b></td><td align='left'>"
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

    _markerArray.push(_marker);

}

/*Create Marker With InfoBox Dynamic case* Created By Shubham Singh On 22/01/2021*/
function CreateMarkerWithInfoBoxDynamic(InfoboxContent, regNo, ATMSImage, bOpenNewInfoBox, bClosePreviousInfoBox) {
   

    // var img='http://chart.apis.google.com/chart?chst=d_map_pin_letter&chld='+A|FF0000|000000'
    _marker = new google.maps.Marker({
        map: _map,
        icon: '/App_Images/' + 'pin_yellow.png',
        ////icon: iconimages,
        //D:\Shubham singh\FTS\FTS\FleetTrackingSystem\App_Images/pin_yellow.png

        position: new google.maps.LatLng(InfoboxContent.latitude, InfoboxContent.longitude)
    });


    if (_infowindow && (bClosePreviousInfoBox == "1" || bClosePreviousInfoBox == "")) {
        _infowindow.close();
    }
    var livetrack = "display:none;";
    //if (InfoboxContent.Status == 'P') {
        livetrack = "display:inline-block;"
    //}
    _infowindow = new google.maps.InfoWindow();

    _content = "<div class='popup'><table class='info-table'><tr align='left'><td colspan='2'><b>"
    + regNo + "</b></td><td><b>"
    + "</b></td></tr><tr nowrap><td width='90px' align='left'><b>IMEI No:</b></td><td align='left'>"
    + InfoboxContent.imeiNo + "</td></tr><tr><td  align='left'><b>Recorded :</b></td><td align='left'>"
    + InfoboxContent.deviceDatetime + "</td> </tr><tr><td align='left'><b>Speed :</b></td><td align='left'>"
    + InfoboxContent.speed + " Km/Hr</td> </tr><tr><td align='left'><b>Device No. :</b></td><td align='left'>"
        + InfoboxContent.deviceNo + "</td></tr>" //<tr><td align='left'><b>IMEI No. :</b></td><td align='left'>"
        //+ InfoboxContent.IMEINo + "</td> </tr><tr><td align='left'><b>Sim No. :</b></td><td align='left'>"
    //+ InfoboxContent.SIMNo
    //+ "</td></tr>"
    + '<tr><td align="left"></td><td align="left"><a style=' + livetrack + '  href="#" onclick="SendLiveTracking(\'' + regNo + '\'' + ',' + InfoboxContent.companyId + ');">Live Tracking</a>'
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

    _markerArray.push(_marker);

}

function SendLiveTracking(RegistrationNo, FK_CompanyId) {

    $.ajax({
        url: '../Tracking/LiveTracking',
        //  url: "@Url.Action("GetMachineList", "Tracking")",
        type: "GET",
        dataType: "JSON",
        data: { CompId: FK_CompanyId, regNo: RegistrationNo },
        success: function (r) {

            var rst = r;
            if (rst == 1) {
                window.location.href = "../Tracking/Tracking";
            }
        }
    });
}

function SendTripLiveTracking(RegistrationNo, tripno, FK_CompanyId) {
    $.ajax({
        url: '../TMS/TripDashboard/TripLiveTrackingData',
        //  url: "@Url.Action("GetMachineList", "Tracking")",
        type: "GET",
        dataType: "JSON",
        data: { CompId: FK_CompanyId, regNo: RegistrationNo, tripNo: tripno },
        success: function (r) {

            var rst = r;
            if (rst == 1) {
                window.location.href = "../TMS/TripDashboard/TripLiveTracking";
            }
        }
    });
}
function CreateTableHistoryBody(InfoboxContent) {
    var tr = "<tr><td style='width:35%'>" + InfoboxContent.DeviceDateTime + "</td>"
                                             + "<td style='width:45%' align='Left'>" + InfoboxContent.DeviceDateTime + "</td>"
                                             + "<td style='width:20%' align='center'>" + InfoboxContent.Speed + "</td> </tr>"
    $('#tbleHistoryBody').html(tr);
    tr = '';
}

/*Create Route*/
function CreateRoute(MapControlID, Zoom, CenterLat, CenterLng, InfoboxContent, PathColor, PathOpacity, PathWeight) {
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



                CreateMarkerWithInfoBox(InfoboxContent[i])

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
                            PathColor = '#CC3300';
                            preISGSM = 'OFF';
                            goto = 0;

                        } else if (InfoboxContent[i].ISGSM == 'OFF' && preISGSM == 'OFF') {
                            k = 0;
                            PathColor = '#0B3B0B';
                            preISGSM = 'OFF';
                            goto = 0;
                        }


                    //if (preISGSM == InfoboxContent[i].ISGSM && preISGSM=='ON')
                    //{

                    //}
                    //if (InfoboxContent[i].ISGSM == 'ON') {
                    //    PathColor = "#CC3300";
                    //    preISGSM='ON'
                    //}
                    //else 
                    //if (InfoboxContent[i].ISGSM == 'OFF' && preISGSM == 'OFF')
                    //{
                    //    k=1;
                    //}
                    //else  if (InfoboxContent[i].ISGSM == 'ON' && preISGSM == 'OFF')
                    //{
                    //    k=k+1;
                    //    preISGSM='ON'
                    //}
                    //else  if (InfoboxContent[i].ISGSM == 'OFF' && preISGSM == 'ON')
                    //{
                    //    k=k+1;
                    //    preISGSM='ON'
                    //}
                    //if (InfoboxContent[i].ISGSM == 'OFF' && preISGSM == 'OFF') {
                    //    PathColor = '#0B3B0B';
                    //    j = i - 1;
                    //    //k = i;
                    //    //if (preISGSM == 'OFF') {
                    //    //    j = j;
                    //    //}
                    //    //else
                    //    //{


                    //    //}
                    //     preISGSM = 'OFF'
                    //}
                    //else
                    //{
                    //    if (preISGSM == 'OFF' && InfoboxContent[i].ISGSM == 'ON')
                    //    {
                    //        k = 0;
                    //        preISGSM = 'ON';

                    //    }
                    //    else if (preISGSM == 'ON' && InfoboxContent[i].ISGSM == 'ON') {
                    //        preISGSM = 'ON';
                    //        k++;

                    //    }
                    //    else if (preISGSM == 'ON' && InfoboxContent[i].ISGSM == 'OFF') {
                    //        k++;
                    //        preISGSM = 'OFF';
                    //        j = i - k;

                    //    }
                    //preISGSM = 'ON'
                    //j = k;
                    // j = j;
                    // }




                    //if(preISGSM=='OFF')
                    //{
                    //    prelat=InfoboxContent[i].Lat;
                    //    prelong = InfoboxContent[i].Long
                    //}
                    //var direction = new google.maps.LatLng(InfoboxContent[i].Lat, InfoboxContent[i].Long);
                    //var heading = google.maps.geometry.spherical.computeHeading(direction, station);
                    //if (InfoboxContent[i].ISGSM == 'OFF') {
                    if (goto < 1) {
                        if (k > 0) {
                            var _vehTrackCoordinates = [new google.maps.LatLng(InfoboxContent[i].Lat,
                                                InfoboxContent[i].Long),
                                            new google.maps.LatLng(InfoboxContent[k].Lat,
                                                InfoboxContent[k].Long)
                            ];

                        } else if (InfoboxContent[i].ISGSM == 'OFF') {

                            var _vehTrackCoordinates = [new google.maps.LatLng(InfoboxContent[i].Lat,
                                                     InfoboxContent[i].Long),
                                                 new google.maps.LatLng(InfoboxContent[i - 1].Lat,
                                                     InfoboxContent[i - 1].Long)
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
                                    //scale: 6,
                                    //rotation: heading,
                                    strokeColor: PathColor,
                                    fillColor: PathColor,
                                    fillOpacity: 1
                                },
                                repeat: '150px',
                                path: _vehTrackCoordinates
                            }]

                        });

                        _RoutePath.setMap(_map);
                    }
                    //}
                    //}
                    //}
                    //else {
                    //    preISGSM = InfoboxContent[i].ISGSM;

                    // }



                }
            }
            //   if (IsfitBounds != false) {

            if (InfoboxContent.length > 0) {
                var endlenth = InfoboxContent.length;
                _markerArray[0].setAnimation(google.maps.Animation.BOUNCE);
                _markerArray[endlenth - 1].setAnimation(google.maps.Animation.BOUNCE);
                _map.setCenter(new google.maps.LatLng(InfoboxContent[InfoboxContent.length - 1].Lat, InfoboxContent[InfoboxContent.length - 1].Long));
            }

        } else {
            // alert("Marker list not pass !");
        }
    } catch (Error) {
        // alert("Problem in Create Route :" + Error.message);
    }
}

/*Create For Penna Route*/

function CreatePennaRoute(MapControlID, Zoom, CenterLat, CenterLng, InfoboxContent, PathColor, PathOpacity, PathWeight) {
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



                //CreateMarkerWithInfoBox(InfoboxContent[i])
                CreatePennaMarkerWithInfoBox(InfoboxContent[i])

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
                            PathColor = '#CC3300';
                            preISGSM = 'OFF';
                            goto = 0;

                        } else if (InfoboxContent[i].ISGSM == 'OFF' && preISGSM == 'OFF') {
                            k = 0;
                            PathColor = '#0B3B0B';
                            preISGSM = 'OFF';
                            goto = 0;
                        }


                    //if (preISGSM == InfoboxContent[i].ISGSM && preISGSM=='ON')
                    //{

                    //}
                    //if (InfoboxContent[i].ISGSM == 'ON') {
                    //    PathColor = "#CC3300";
                    //    preISGSM='ON'
                    //}
                    //else 
                    //if (InfoboxContent[i].ISGSM == 'OFF' && preISGSM == 'OFF')
                    //{
                    //    k=1;
                    //}
                    //else  if (InfoboxContent[i].ISGSM == 'ON' && preISGSM == 'OFF')
                    //{
                    //    k=k+1;
                    //    preISGSM='ON'
                    //}
                    //else  if (InfoboxContent[i].ISGSM == 'OFF' && preISGSM == 'ON')
                    //{
                    //    k=k+1;
                    //    preISGSM='ON'
                    //}
                    //if (InfoboxContent[i].ISGSM == 'OFF' && preISGSM == 'OFF') {
                    //    PathColor = '#0B3B0B';
                    //    j = i - 1;
                    //    //k = i;
                    //    //if (preISGSM == 'OFF') {
                    //    //    j = j;
                    //    //}
                    //    //else
                    //    //{


                    //    //}
                    //     preISGSM = 'OFF'
                    //}
                    //else
                    //{
                    //    if (preISGSM == 'OFF' && InfoboxContent[i].ISGSM == 'ON')
                    //    {
                    //        k = 0;
                    //        preISGSM = 'ON';

                    //    }
                    //    else if (preISGSM == 'ON' && InfoboxContent[i].ISGSM == 'ON') {
                    //        preISGSM = 'ON';
                    //        k++;

                    //    }
                    //    else if (preISGSM == 'ON' && InfoboxContent[i].ISGSM == 'OFF') {
                    //        k++;
                    //        preISGSM = 'OFF';
                    //        j = i - k;

                    //    }
                    //preISGSM = 'ON'
                    //j = k;
                    // j = j;
                    // }




                    //if(preISGSM=='OFF')
                    //{
                    //    prelat=InfoboxContent[i].Lat;
                    //    prelong = InfoboxContent[i].Long
                    //}
                    //var direction = new google.maps.LatLng(InfoboxContent[i].Lat, InfoboxContent[i].Long);
                    //var heading = google.maps.geometry.spherical.computeHeading(direction, station);
                    //if (InfoboxContent[i].ISGSM == 'OFF') {
                    if (goto < 1) {
                        if (k > 0) {
                            var _vehTrackCoordinates = [new google.maps.LatLng(InfoboxContent[i].Lat,
                                                InfoboxContent[i].Long),
                                            new google.maps.LatLng(InfoboxContent[k].Lat,
                                                InfoboxContent[k].Long)
                            ];

                        } else if (InfoboxContent[i].ISGSM == 'OFF') {

                            var _vehTrackCoordinates = [new google.maps.LatLng(InfoboxContent[i].Lat,
                                                     InfoboxContent[i].Long),
                                                 new google.maps.LatLng(InfoboxContent[i - 1].Lat,
                                                     InfoboxContent[i - 1].Long)
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
                                    //scale: 6,
                                    //rotation: heading,
                                    strokeColor: PathColor,
                                    fillColor: PathColor,
                                    fillOpacity: 1
                                },
                                repeat: '150px',
                                path: _vehTrackCoordinates
                            }]

                        });

                        _RoutePath.setMap(_map);
                    }
                    //}
                    //}
                    //}
                    //else {
                    //    preISGSM = InfoboxContent[i].ISGSM;

                    // }



                }
            }
            //   if (IsfitBounds != false) {

            if (InfoboxContent.length > 0) {
                var endlenth = InfoboxContent.length;
                _markerArray[0].setAnimation(google.maps.Animation.BOUNCE);
                _markerArray[endlenth - 1].setAnimation(google.maps.Animation.BOUNCE);
                _map.setCenter(new google.maps.LatLng(InfoboxContent[InfoboxContent.length - 1].Lat, InfoboxContent[InfoboxContent.length - 1].Long));
            }

        } else {
            // alert("Marker list not pass !");
        }
    } catch (Error) {
        // alert("Problem in Create Route :" + Error.message);
    }
}


/*Create Route*/
function CreateMARKERLBS(MapControlID, Zoom, CenterLat, CenterLng, InfoboxContent, PathColor, PathOpacity, PathWeight) {
    try {



        //ClearRoute()
        if (InfoboxContent.length != 0) {
            for (var i = 0; i <= InfoboxContent.length - 1; i++) {
                CreateMarkerWithInfoBox(InfoboxContent[i])
            }
            //   if (IsfitBounds != false) {

            //if (InfoboxContent.length > 0) {
            //    var endlenth = InfoboxContent.length;
            //    _markerArray[0].setAnimation(google.maps.Animation.BOUNCE);
            //    _markerArray[endlenth - 1].setAnimation(google.maps.Animation.BOUNCE);
            //    _map.setCenter(new google.maps.LatLng(InfoboxContent[0].Lat, InfoboxContent[0].Long));
            //}

        } else {
            // alert("Marker list not pass !");
        }
    } catch (Error) {
        // alert("Problem in Create Route :" + Error.message);
    }
}

/*Plays Route With Marker*/
function PlayRoute(MapControlID, Zoom, CenterLat, CenterLng, InfoboxContent, PathColor, PathOpacity, PathWeight, TimerIntervalMS, bAlwayOpenInfoBox, IsPanEnable) {

    try {

        ClearMap();
        _counter = 0;
        _InfoboxContent = InfoboxContent;
        trHTML = '';
        _SetInterval = setInterval('Play(' + '"' + PathColor + '",' + PathOpacity + ',' + PathWeight + ',' + bAlwayOpenInfoBox + ',' + IsPanEnable + ')', TimerIntervalMS);
    } catch (Error) {
        // alert("Problem in Create Route :" + Error.message);
    }
}

/*Pause Tracking*/
function Pause() {
    clearTimeout(_SetInterval);
    // ClearMap();
}
function CreateTableHistoryBody(InfoboxContent) {

    var tr = "<tr><td style='width:35%'>" + InfoboxContent.DeviceDateTime + "</td>"
                                             + "<td style='width:45%' align='Left'>" + InfoboxContent.Location + "</td>"
                                             + "<td style='width:20%' align='center'>" + InfoboxContent.Speed + "</td>"
                                             + "<td style='width:20%' align='center'>" + InfoboxContent.Lat + "</td>"
                                             + "<td style='width:20%' align='center'>" + InfoboxContent.Long + "</td></tr>"
    trHTML += tr;
    $('#tbleHistoryBody').html(trHTML);
    tr = '';
}

var prelat;
var prelong;
var preISGSM = 'OFF'
var j = 0;
var k = 0;
var goto = 0;
function Play(PathColor, PathOpacity, PathWeight, bAlwayOpenInfoBox, IsPanEnable) {

    if (_counter <= _InfoboxContent.length - 1) {

        CreateMarkerWithInfoBox(_InfoboxContent[_counter], bAlwayOpenInfoBox, 1);
        CreateTableHistoryBody(_InfoboxContent[_counter]);
        if (_counter > 0) {
            //var _vehTrackCoordinates = [new google.maps.LatLng(_InfoboxContent[_counter].Lat,
            //                               _InfoboxContent[_counter].Long),
            //                           new google.maps.LatLng(_InfoboxContent[_counter - 1].Lat,
            //                               _InfoboxContent[_counter - 1].Long)
            //];
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
                    PathColor = '#CC3300';
                    preISGSM = 'OFF';
                    goto = 0;

                } else if (_InfoboxContent[_counter].ISGSM == 'OFF' && preISGSM == 'OFF') {
                    k = 0;
                    PathColor = '#0B3B0B';
                    preISGSM = 'OFF';
                    goto = 0;
                }


            if (goto < 1) {
                if (k > 0) {
                    var _vehTrackCoordinates = [new google.maps.LatLng(_InfoboxContent[_counter].Lat,
                                        _InfoboxContent[_counter].Long),
                                    new google.maps.LatLng(_InfoboxContent[k].Lat,
                                        _InfoboxContent[k].Long)
                    ];

                } else if (_InfoboxContent[_counter].ISGSM == 'OFF') {

                    var _vehTrackCoordinates = [new google.maps.LatLng(_InfoboxContent[_counter].Lat,
                                             _InfoboxContent[_counter].Long),
                                         new google.maps.LatLng(_InfoboxContent[_counter - 1].Lat,
                                             _InfoboxContent[_counter - 1].Long)
                    ];
                }


                _vehPath = new google.maps.Polyline({
                    path: _vehTrackCoordinates,
                    ////strokeColor: PathColor,
                    ////strokeOpacity: PathOpacity,
                    ////strokeWeight: PathWeight
                    strokeColor: PathColor,// PathColor,//"#CC3300",
                    strokeOpacity: PathOpacity,//80,
                    strokeWeight: PathWeight,//2,
                    icons: [{

                        icon: {
                            path: google.maps.SymbolPath.BACKWARD_CLOSED_ARROW,
                            //scale: 6,
                            //rotation: heading,
                            strokeColor: PathColor,
                            fillColor: PathColor,
                            fillOpacity: 1
                        },
                        repeat: '150px',
                        path: _vehTrackCoordinates
                    }]
                });

                if (IsPanEnable = 1) {
                    _map.panTo(new google.maps.LatLng(_InfoboxContent[_counter].Lat,
                                                   _InfoboxContent[_counter].Long));
                }

                _vehPath.setMap(_map);
            }

        }
        _counter++;
    }
}

function ClearMap() {
    //debugger
    removeLine();
    setMapOnAll(null);
}

function removeLine() {

    if (typeof _vehPath === 'undefined') { } else {
        _vehPath.setMap(null);
    }
}

function ClearRoute() {

    if (typeof _RoutePath != 'undefined') {
        _RoutePath.setMap(null);
    }
    setMapOnAll(null);
    //if (typeof _vehPath === 'undefined') { } else {
    //    _vehPath.setMap(null);
    //}
}
// Sets the map on all markers in the array.
function setMapOnAll(map) {
    // debugger
    if (typeof _markerArray === 'undefined') { } else {
        for (var i = 0; i < _markerArray.length; i++) {
            _markerArray[i].setMap(map);
        }

        _markerArray = [];
    }
}
//Trip Dashboard
/*Function to load markers*/
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

/*Create Trip Marker With InfoBox*/
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
        "<div class='popup' style='width:450px;'><table class='info-table' width='100%'><tr align='left'><td colspan='4'><b style='margin-bottom:5px;display:block;font-size:14px;color:#e90a0b'>" + InfoboxContent.Route_Name + "</b></td></tr><tr align='left'><td width='90px' align='left'><b>Vehicle No.:</b></td><td  colspan='3'><b>"
    + InfoboxContent.Registration_No + "</b></td></tr><tr nowrap><td width='90px' align='left'><b>Driver Name:</b></td><td align='left'>"
    + InfoboxContent.ChauffeurName + "</td><td  align='left'><b>Driver Mob:</b></td><td align='left'>"
    + InfoboxContent.ChauffeurMob + "</td> </tr><tr><td  align='left'><b>Recorded :</b></td><td align='left'>"
    + InfoboxContent.Device_DateTime + "</td>"//<tr><td align='left'><b>Device No. :</b></td><td align='left'>"
    //    + InfoboxContent.Device_No + "</td> </tr><tr><td align='left'><b>IMEI No. :</b></td><td align='left'>"
    //    + InfoboxContent.IMEINo + "</td> </tr><tr><td align='left'><b>Sim No. :</b></td><td align='left'>"
   // + InfoboxContent.SimNo
   // + "</td></tr>
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

    //      "<div class='popup'><table class='info-table'><tr align='left'><td colspan='2'><b>" + InfoboxContent.Route_Name + "</b></td></tr><tr align='left'><td width='90px' align='left'><b>Vehicle No.:</b></td><td  ><b>"
    //  + InfoboxContent.Registration_No + "</b></td></tr><tr nowrap><td width='90px' align='left'><b>Driver Name:</b></td><td align='left'>"
    //  + InfoboxContent.ChauffeurName + "</td></tr><tr><td  align='left'><b>Driver Mob:</b></td><td align='left'>"
    //  + InfoboxContent.ChauffeurMob + "</td> </tr><tr><td  align='left'><b>Recorded :</b></td><td align='left'>"
    //  + InfoboxContent.Device_DateTime + "</td> </tr>"//<tr><td align='left'><b>Device No. :</b></td><td align='left'>"
    //     // + InfoboxContent.Device_No + "</td> </tr><tr><td align='left'><b>IMEI No. :</b></td><td align='left'>"
    //     // + InfoboxContent.IMEINo + "</td> </tr><tr><td align='left'><b>Sim No. :</b></td><td align='left'>"
    ////  + InfoboxContent.SimNo
    ////  + "</td></tr>
    // + "<tr><td align='left'><b>Trip No. :</b></td><td align='left'>"
    //  + InfoboxContent.Trip_No
    //  + "</td></tr><tr><td align='left'><b>ETD :</b></td><td align='left'>"
    //  + InfoboxContent.ETD
    //  + "</td></tr><tr><td align='left'><b>ETA :</b></td><td align='left'>"
    //  + InfoboxContent.ExpectedDtofArrival
    //  + "</td></tr><tr><td align='left'><b>RTA :</b></td><td align='left'>"
    //  + InfoboxContent.RTA
    //  + "</td></tr><tr><td align='left'><b>Travel Date :</b></td><td align='left'>"
    //  + InfoboxContent.Travel_Date
    //  + "</td></tr><tr><td align='left'><b>ATD :</b></td><td align='left'>"
    //  + InfoboxContent.ATD
    //  + "</td></tr>"//<tr><td align='left'><b>ATA:</b></td><td align='left'>"
    //  //+ InfoboxContent.ATA
    //  //+ "</td></tr>"
    //    + "</td></tr><tr><td align='left'><b>Alarm :</b></td><td align='left'>"
    //  + InfoboxContent.AlarmDesc
    //  + "</td></tr>"
    //  //comment by anjani
    //    + '<tr><td align="left"></td><td align="left"><a href="#" onclick="ShowTrafficOnMap(\'' + InfoboxContent.Trip_No + '\');">Live Traffic</a>'
    //    + "</td></tr>"
    //  + "</table></div>";
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

function DeleteMarker(id) {

    for (var i = 0; i < _markerArray.length; i++) {
        if (_markerArray[i].id == id) {
            _markerArray[i].setMap(null);
            _markerArray.splice(i, 1);
            return;
        }
    }
};

function DeleteMarkerLastMarker() {

    for (var i = 0; i < _markerArray.length; i++) {

        _markerArray[i].setMap(null);
        // _markerArray.splice(i, 1);

    }
};

/*Create Trip Marker With InfoBox*/
function CreateTripMarkerWithInfoBox(InfoboxContent, bOpenNewInfoBox, bClosePreviousInfoBox) {
    // ;
    _marker = new google.maps.Marker({
        map: _map,
        //   icon: '../App_Images/' + InfoboxContent.Icon,

        position: new google.maps.LatLng(InfoboxContent.Lat, InfoboxContent.Long)
    });

    //  _marker.id = InfoboxContent.length;
    var str = RotateIcon.makeIcon('/App_Images/' + InfoboxContent.Icon).setRotation({ deg: InfoboxContent.Heading }).getUrl();
    // alert(InfoboxContent.Registration_No + '|||||||||||||' + str);
    var a = endsWith(str, "AD12TscoAAAAAElFTkSuQmCC");
    if (endsWith(str, "AD12TscoAAAAAElFTkSuQmCC") || endsWith(str, "AAAAAAAAAAAAAAAAH4NMPwAAZOMoNsAAAAASUVORK5CYII="))
        _marker.setIcon('/App_Images/' + InfoboxContent.Icon);
    else
        _marker.setIcon(RotateIcon.makeIcon('/App_Images/' + InfoboxContent.Icon).setRotation({ deg: InfoboxContent.Heading }).getUrl());



    if (_infowindow && (bClosePreviousInfoBox == "1" || bClosePreviousInfoBox == "")) {
        _infowindow.close();
    }
    var livetrack = "display:none;";
    if (InfoboxContent.Status == 'P') {
        livetrack = "display:inline-block;"
    }

    _infowindow = new google.maps.InfoWindow();

    _content = "<div class='popup' style='width:450px;'><table class='info-table' width='100%'><tr align='left'><td colspan='4'><b style='margin-bottom:5px;display:block;font-size:14px;color:#e90a0b'>" + InfoboxContent.Route_Name + "</b></td></tr><tr align='left'><td width='90px' align='left'><b>Vehicle No.:</b></td><td  colspan='3'><b>"
    + InfoboxContent.Registration_No + "</b></td></tr><tr nowrap><td width='90px' align='left'><b>Driver Name:</b></td><td align='left'>"
    + InfoboxContent.ChauffeurName + "</td><td  align='left'><b>Driver Mob:</b></td><td align='left'>"
    + InfoboxContent.ChauffeurMob + "</td> </tr><tr><td  align='left'><b>Recorded :</b></td><td align='left'>"
    + InfoboxContent.Device_DateTime + "</td>"//<tr><td align='left'><b>Device No. :</b></td><td align='left'>"
    //    + InfoboxContent.Device_No + "</td> </tr><tr><td align='left'><b>IMEI No. :</b></td><td align='left'>"
    //    + InfoboxContent.IMEINo + "</td> </tr><tr><td align='left'><b>Sim No. :</b></td><td align='left'>"
   // + InfoboxContent.SimNo
   // + "</td></tr>
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
      + '<tr><td align="left"></td><td align="left"><a href="#" onclick="SendTripLiveTracking(\'' + InfoboxContent.Registration_No + '\'' + ',\'' + InfoboxContent.Trip_No + '\'' + ',\'' + 0 + '\');">Live Tracking</a>'
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

    _markerArray.push(_marker);
}

/*Function to load WithMovingDirection*/
function DrawMarkerWithMovingDirection(MapControlID, Zoom, CenterLat, CenterLng, InfoboxContent) {
    //debugger
    try {
        // ;
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
            _map.fitBounds(bounds);
            _map.panToBounds(bounds);
            markerCluster = new MarkerClusterer(_map, _markerArray, { imagePath: 'https://developers.google.com/maps/documentation/javascript/examples/markerclusterer/m' });
            //  _map.setCenter(new google.maps.LatLng(InfoboxContent[0].Lat, InfoboxContent[0].Long));
            chkZommChange = false;
            //   if (IsfitBounds != false) {
            if (chkZoom == false) {
                chkZommChange = true;
                lastzoom = _map.zoom;

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
/*Create Marker With InfoBox With Direction*/
function CreateMarkerWithInfoBoxWithDirection(InfoboxContent, bOpenNewInfoBox, bClosePreviousInfoBox) {

    // debugger

    //InfoboxContent.FK_CompanyId



    // var img='http://chart.apis.google.com/chart?chst=d_map_pin_letter&chld='+A|FF0000|000000'
    _marker = new google.maps.Marker({
        map: _map,
        //   icon: '../App_Images/' + InfoboxContent.Icon,

        position: new google.maps.LatLng(InfoboxContent.Lat, InfoboxContent.Long)
    });

    //  _marker.id = InfoboxContent.length;

    if (InfoboxContent.FK_CompanyId == '20') {
        var str = RotateIcon.makeIcon('/App_Images/' + InfoboxContent.Icon).setRotation({ deg: InfoboxContent.Heading }).getUrl();
        var a = endsWith(str, "AD12TscoAAAAAElFTkSuQmCC");
        if (endsWith(str, "AD12TscoAAAAAElFTkSuQmCC"))
            _marker.setIcon('/App_Images/' + InfoboxContent.Icon);
        else
            _marker.setIcon(RotateIcon.makeIcon('/App_Images/' + InfoboxContent.Icon).setRotation({ deg: InfoboxContent.Heading }).getUrl());
    }
    else {
        _marker.setIcon('/App_Images/' + InfoboxContent.Icon);
    }

    if (_infowindow && (bClosePreviousInfoBox == "1" || bClosePreviousInfoBox == "")) {
        _infowindow.close();
    }
    var livetrack = "display:none;";
    if (InfoboxContent.Status == 'P') {
        livetrack = "display:inline-block;"
    }
    var nearBanks = "";

    if (InfoboxContent.GoogleApiNearByType) {
        nearBanks = '</br><a href="javascript:void(0)"  onclick="ViewPlacesOnMap_MyNearby(\'' + InfoboxContent.Lat + '\'' + ',\'' + InfoboxContent.Long + '\'' + ',\'' + InfoboxContent.GoogleApiNearByType + '\'' + ',\'' + InfoboxContent.GoogleApiNearByName + '\');">View Nearby ' + InfoboxContent.NearBy + '</a>'
    }
    _infowindow = new google.maps.InfoWindow();

    _content = "<div class='popup'><table class='info-table'><tr align='left'><td colspan='2'><b>"
    + InfoboxContent.RegistrationNo + "</b></td><td><b>"
    + InfoboxContent.ModelName + "</b></td></tr><tr nowrap><td width='90px' align='left'><b>Driver Name:</b></td><td align='left'>"
    + InfoboxContent.ChauffeurName + "</td></tr><tr><td  align='left'><b>Driver Mob:</b></td><td align='left'>"
    + InfoboxContent.ChauffeurMob + "</td> </tr><tr><td  align='left'><b>Recorded :</b></td><td align='left'>"
    + InfoboxContent.DeviceDateTime + "</td> </tr><tr><td align='left'><b>Speed :</b></td><td align='left'>"
    + InfoboxContent.Speed + " Km/Hr</td> </tr><tr><td align='left'><b>Device No. :</b></td><td align='left'>"
    + InfoboxContent.DeviceNo + "</td> </tr><tr><td align='left'><b>IMEI No. :</b></td><td align='left'>"
    + InfoboxContent.IMEINo + "</td> </tr><tr><td align='left'><b>Sim No. :</b></td><td align='left'>"
    + InfoboxContent.SIMNo
    + "</td></tr>"
    + '<tr><td align="left"></td><td align="left"><a style=' + livetrack + '  href="#" onclick="SendLiveTracking(\'' + InfoboxContent.RegistrationNo + '\'' + ',' + InfoboxContent.FK_CompanyId + ');">Live Tracking</a></br><a href="javascript:void(0)"  onclick="ViewPlacesOnMap(\'' + InfoboxContent.Lat + '\'' + ',\'' + InfoboxContent.Long + '\');">View Police Stations</a>'
    + nearBanks
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
    _markerArray.push(_marker);
}

function endsWith(str, suffix) {
    return str.indexOf(suffix, str.length - suffix.length) !== -1;
}


/*Code By Tarique Starts :: Being Used to play Completed Trip Routes*/
/*Sets time interval & calls 'PlayActualRouteSteps' to draw actual route step by step*/
function ActualRoutePlay(MapControlID, CenterLat, CenterLng, InfoboxContent, PathColor, PathOpacity, PathWeight, TimerIntervalMS, bAlwayOpenInfoBox, IsPanEnable) {

    try {
        Pause();
        ClearMap();
        _counter = 0;
        _InfoboxContent = InfoboxContent;
        trHTML = '';
        _SetInterval = setInterval('PlayActualRouteSteps(' + '"' + PathColor + '",' + PathOpacity + ',' + PathWeight + ',' + bAlwayOpenInfoBox + ',' + IsPanEnable + ')', TimerIntervalMS);
    } catch (Error) {
        // alert("Problem in Create Route :" + Error.message);
    }
}

/*Draws actual route step by step*/
function PlayActualRouteSteps(PathColor, PathOpacity, PathWeight, bAlwayOpenInfoBox, IsPanEnable) {
    //;
    // Pause();
    //alert(_InfoboxContent.length);
    if (_counter <= _InfoboxContent.length - 1) {
        CreateMarkerWithInfoBox(_InfoboxContent[_counter], bAlwayOpenInfoBox, 1);
        if (_counter > 0) {


            var _vehTrackCoordinates = [new google.maps.LatLng(_InfoboxContent[_counter].Lat,
                                             _InfoboxContent[_counter].Long),
                                         new google.maps.LatLng(_InfoboxContent[_counter - 1].Lat,
                                             _InfoboxContent[_counter - 1].Long)
            ];

            _vehPath = new google.maps.Polyline({
                path: _vehTrackCoordinates,
                strokeColor: PathColor,
                strokeOpacity: PathOpacity,
                strokeWeight: PathWeight,
                icons: [{

                    icon: {
                        path: google.maps.SymbolPath.BACKWARD_CLOSED_ARROW,
                        strokeColor: PathColor,
                        fillColor: PathColor,
                        fillOpacity: 1
                    },
                    repeat: '150px',
                    path: _vehTrackCoordinates
                }]
            });
            _vehPath.setMap(_map);
        }


        _counter++;
    }

}

/*Code By Tarique Ends*/

/*Draws actual route step by step*/
function PlayActualRouteStepsDynamic(PathColor, PathOpacity, PathWeight, bAlwayOpenInfoBox, IsPanEnable) {
    //;
    // Pause();
    //alert(_InfoboxContent.length);
    if (_counter <= _InfoboxContent.length - 1) {
        CreateMarkerWithInfoBoxDynamic(_InfoboxContent[_counter], _regNo,_ATMSIMage, bAlwayOpenInfoBox, 1);
        if (_counter > 0) {


            var _vehTrackCoordinates = [new google.maps.LatLng(_InfoboxContent[_counter].latitude,
                                             _InfoboxContent[_counter].longitude),
                                         new google.maps.LatLng(_InfoboxContent[_counter - 1].latitude,
                                             _InfoboxContent[_counter - 1].longitude)
            ];

            _vehPath = new google.maps.Polyline({
                path: _vehTrackCoordinates,
                strokeColor: PathColor,
                strokeOpacity: PathOpacity,
                strokeWeight: PathWeight,
                icons: [{

                    icon: {
                        path: google.maps.SymbolPath.BACKWARD_CLOSED_ARROW,
                        strokeColor: PathColor,
                        fillColor: PathColor,
                        fillOpacity: 1
                    },
                    repeat: '150px',
                    path: _vehTrackCoordinates
                }]
            });
            _vehPath.setMap(_map);
        }


        _counter++;
    }

}

/*Code By Shubham Ends*/

/*CODE FOR IBOUND TRIP MANAGEMENT SYSTEM STARTS*/

/*CREATES INBOUND TRIP MARKER*/
function DrawInboundTripMarker(MapControlID, Zoom, CenterLat, CenterLng, InfoboxContent) {

    //alert('DrawInboundTripMarker');
    try {

        var iVar = 0;
        ClearMap();

        if (InfoboxContent.length != 0) {
            for (var i = 0; i <= InfoboxContent.length - 1; i++) {
                CreateInboundTripMarkerWithInfoBox(InfoboxContent[i])
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

/*Create INBOUND Trip Marker With InfoBox*/
function CreateInboundTripMarkerWithInfoBox(InfoboxContent, bOpenNewInfoBox, bClosePreviousInfoBox) {
    ;
    //debugger
    //alert('CreateInboundTripMarkerWithInfoBox');
    _marker = new google.maps.Marker({
        map: _map,
        //   icon: '../App_Images/' + InfoboxContent.Icon,

        position: new google.maps.LatLng(InfoboxContent.Lat, InfoboxContent.Long)
    });

    //  _marker.id = InfoboxContent.length;
    var str = RotateIcon.makeIcon('/App_Images/' + InfoboxContent.Icon).setRotation({ deg: InfoboxContent.Heading }).getUrl();
    // alert(InfoboxContent.Registration_No + '|||||||||||||' + str);
    var a = endsWith(str, "AD12TscoAAAAAElFTkSuQmCC");
    if (endsWith(str, "AD12TscoAAAAAElFTkSuQmCC") || endsWith(str, "AAAAAAAAAAAAAAAAH4NMPwAAZOMoNsAAAAASUVORK5CYII="))
        _marker.setIcon('/App_Images/' + InfoboxContent.Icon);
    else
        _marker.setIcon(RotateIcon.makeIcon('/App_Images/' + InfoboxContent.Icon).setRotation({ deg: InfoboxContent.Heading }).getUrl());



    if (_infowindow && (bClosePreviousInfoBox == "1" || bClosePreviousInfoBox == "")) {
        _infowindow.close();
    }
    var livetrack = "display:none;";
    if (InfoboxContent.Status == 'P') {
        livetrack = "display:inline-block;"
    }

    _infowindow = new google.maps.InfoWindow();

   // debugger
    if (InfoboxContent.EndAddress != "" || InfoboxContent.WayPtAddress != "") {


        if (!InfoboxContent.IsSimTrip) {
            _content = "<div class='popup' style='width:450px;'><table class='info-table' width='100%'><tr align='left'><td colspan='4'><b style='margin-bottom:5px;display:block;font-size:14px;color:#e90a0b'>" + InfoboxContent.Route_Name + "</b></td></tr><tr align='left'><td width='90px' align='left'><b>Vehicle No.:</b></td><td  colspan='3'><b>"
       + InfoboxContent.Registration_No + "</b></td></tr><tr nowrap><td width='90px' align='left'><b>Driver Name:</b></td><td align='left'>"
       + InfoboxContent.ChauffeurName + "</td><td  align='left'><b>Driver Mob:</b></td><td align='left'>"
       + InfoboxContent.ChauffeurMob + "</td> </tr><tr><td  align='left'><b>Recorded :</b></td><td align='left'>"
       + InfoboxContent.Device_DateTime + "</td>"//<tr><td align='left'><b>Device No. :</b></td><td align='left'>"
       //    + InfoboxContent.Device_No + "</td> </tr><tr><td align='left'><b>IMEI No. :</b></td><td align='left'>"
       //    + InfoboxContent.IMEINo + "</td> </tr><tr><td align='left'><b>Sim No. :</b></td><td align='left'>"
      // + InfoboxContent.SimNo
      // + "</td></tr>
       + "<td align='left'><b>Trip No. :</b></td><td align='left'>"
       + InfoboxContent.Trip_No
       + "</td></tr><tr><td align='left'><b>Travel Date :</b></td><td align='left'>"
       + InfoboxContent.Travel_Date
       + "</td></tr><tr><td align='left'><b>ATD :</b></td><td align='left'>"
       + InfoboxContent.ATD
       + "</td></tr>"//<tr><td align='left'><b>ATA:</b></td><td align='left'>"
       //+ InfoboxContent.ATA
       //+ "</td></tr>"

        + "</td></tr><tr><td align='left'><b>Location :</b></td><td align='left'>"
       + InfoboxContent.Location

       + "</td></tr><tr><td align='left'><b>Alarm :</b></td><td colspan='3' align='left'>"
       + InfoboxContent.AlarmDesc
       + "</td></tr><tr><td align='left'><b>End Address :</b></td><td align='left'>"
       + InfoboxContent.EndAddress

       + "</td><td align='left'><b>Way Point Address :</b></td><td align='left'>"
       + InfoboxContent.WayPtAddress

       + "</td></tr><tr><td align='left'><b>ShipmentNo. :</b></td><td align='left'>"
       + InfoboxContent.ShipmentNo

       + "</td></tr>"
       //comment by anjani
         + '<tr><td align="left"></td><td align="left"><a href="javascript:void(0)" onclick="SendInboundTripLiveTracking(\'' + InfoboxContent.Registration_No + '\'' + ',\'' + InfoboxContent.Trip_No + '\'' + ',\'' + InfoboxContent.FK_CompanyId + '\'' + ',\'' + InfoboxContent.FK_Start_Geo_ID + '\'' + ',\'' + InfoboxContent.FK_End_Geo_ID + '\'' + ',\'' + InfoboxContent.Trans_Trip_ID + '\'' + ',\'' + 0 + '\'' + ',\'' + InfoboxContent.TSInDT + '\');">Live Tracking</a>'
         + "</td></tr>"
       + "</table></div>";
        } else {
            _content = "<div class='popup' style='width:450px;'><table class='info-table' width='100%'><tr align='left'><td width='90px' align='left'><b>Vehicle No.:</b></td><td  colspan='3'><b>"
       + InfoboxContent.Registration_No + "</b></td></tr><tr nowrap><td width='90px' align='left'><b>Driver Name:</b></td><td align='left'>"
       + InfoboxContent.ChauffeurName + "</td><td  align='left'><b>Driver Mob:</b></td><td align='left'>"
       + InfoboxContent.ChauffeurMob + "</td> </tr><tr><td  align='left'><b>Recorded :</b></td><td align='left'>"
       + InfoboxContent.Device_DateTime + "</td>"//<tr><td align='left'><b>Device No. :</b></td><td align='left'>"
       //    + InfoboxContent.Device_No + "</td> </tr><tr><td align='left'><b>IMEI No. :</b></td><td align='left'>"
       //    + InfoboxContent.IMEINo + "</td> </tr><tr><td align='left'><b>Sim No. :</b></td><td align='left'>"
      // + InfoboxContent.SimNo
      // + "</td></tr>
       //+ "<td align='left'><b>Trip No. :</b></td><td align='left'>"
       //+ InfoboxContent.Trip_No
       //+ "</td></tr><tr><td align='left'><b>ATD :</b></td><td align='left'>"
       //+ InfoboxContent.ATD
       //+ "</td><td align='left'><b>Travel Date :</b></td><td align='left'>"
       //+ InfoboxContent.Travel_Date
       //+ "</td></tr><tr><td align='left'><b>ATD :</b></td><td align='left'>"
       //+ InfoboxContent.ATD
      // + "</td></tr>"//<tr><td align='left'><b>ATA:</b></td><td align='left'>"
       //+ InfoboxContent.ATA
       //+ "</td></tr>"
        // + "</td></tr><tr><td align='left'><b>Alarm :</b></td><td colspan='3' align='left'>"
       //+ InfoboxContent.AlarmDesc

        + "</td></tr><tr><td align='left'><b>ATD :</b></td><td align='left'>"
       + InfoboxContent.ATD

        + "</td></tr><tr><td align='left'><b>Location :</b></td><td align='left'>"
       + InfoboxContent.Location


       + "</td></tr><tr><td align='left'><b>End Address :</b></td><td align='left'>"
       + InfoboxContent.EndAddress

       + "</td><td align='left'><b>Way Point Address :</b></td><td align='left'>"
       + InfoboxContent.WayPtAddress

       + "</td></tr><tr><td align='left'><b>ShipmentNo. :</b></td><td align='left'>"
       + InfoboxContent.ShipmentNo

       + "</td></tr"
       //comment by anjani
       + '<tr><td align="left"></td><td align="left"><a href="javascript:void(0)" onclick="SendInboundTripLiveTracking(\'' + InfoboxContent.Registration_No + '\'' + ',\'' + InfoboxContent.Trip_No + '\'' + ',\'' + InfoboxContent.FK_CompanyId + '\'' + ',\'' + InfoboxContent.FK_Start_Geo_ID + '\'' + ',\'' + InfoboxContent.FK_End_Geo_ID + '\'' + ',\'' + InfoboxContent.Trans_Trip_ID + '\'' + ',\'' + 1 + '\'' + ',\'' + InfoboxContent.TSInDT + '\');">Live Tracking</a>'
         //+ '<tr><td align="left"></td><td align="left"><a href="javascript:void(0)" onclick="SendInboundTripLiveTracking(\'' + InfoboxContent.Registration_No + '\'' + ',\'' + InfoboxContent.Trip_No + '\'' + ',\'' + InfoboxContent.FK_CompanyId + '\'' + ',\'' + 1 + '\'' + ',\'' + InfoboxContent.TSInDT + '\');">Live Tracking</a>'
         + "</td></tr>"
       + "</table></div>";
        }
    } else {


        if (!InfoboxContent.IsSimTrip) {
            _content = "<div class='popup' style='width:450px;'><table class='info-table' width='100%'><tr align='left'><td colspan='4'><b style='margin-bottom:5px;display:block;font-size:14px;color:#e90a0b'>" + InfoboxContent.Route_Name + "</b></td></tr><tr align='left'><td width='90px' align='left'><b>Vehicle No.:</b></td><td  colspan='3'><b>"
       + InfoboxContent.Registration_No + "</b></td></tr><tr nowrap><td width='90px' align='left'><b>Driver Name:</b></td><td align='left'>"
       + InfoboxContent.ChauffeurName + "</td><td  align='left'><b>Driver Mob:</b></td><td align='left'>"
       + InfoboxContent.ChauffeurMob + "</td> </tr><tr><td  align='left'><b>Recorded :</b></td><td align='left'>"
       + InfoboxContent.Device_DateTime + "</td>"//<tr><td align='left'><b>Device No. :</b></td><td align='left'>"
       //    + InfoboxContent.Device_No + "</td> </tr><tr><td align='left'><b>IMEI No. :</b></td><td align='left'>"
       //    + InfoboxContent.IMEINo + "</td> </tr><tr><td align='left'><b>Sim No. :</b></td><td align='left'>"
      // + InfoboxContent.SimNo
      // + "</td></tr>
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

        + "</td></tr><tr><td align='left'><b>Location :</b></td><td align='left'>"
       + InfoboxContent.Location

       + "</td></tr><tr><td align='left'><b>Alarm :</b></td><td colspan='3' align='left'>"
       + InfoboxContent.AlarmDesc
       + "</td></tr>"
       //comment by anjani
         + '<tr><td align="left"></td><td align="left"><a href="javascript:void(0)" onclick="SendInboundTripLiveTracking(\'' + InfoboxContent.Registration_No + '\'' + ',\'' + InfoboxContent.Trip_No + '\'' + ',\'' + InfoboxContent.FK_CompanyId + '\'' + ',\'' + InfoboxContent.FK_Start_Geo_ID + '\'' + ',\'' + InfoboxContent.FK_End_Geo_ID + '\'' + ',\'' + InfoboxContent.Trans_Trip_ID + '\'' + ',\'' + 0 + '\'' + ',\'' + InfoboxContent.TSInDT + '\');">Live Tracking</a>'
         + "</td></tr>"
       + "</table></div>";
        } else {
            _content = "<div class='popup' style='width:450px;'><table class='info-table' width='100%'><tr align='left'><td width='90px' align='left'><b>Vehicle No.:</b></td><td  colspan='3'><b>"
       + InfoboxContent.Registration_No + "</b></td></tr><tr nowrap><td width='90px' align='left'><b>Driver Name:</b></td><td align='left'>"
       + InfoboxContent.ChauffeurName + "</td><td  align='left'><b>Driver Mob:</b></td><td align='left'>"
       + InfoboxContent.ChauffeurMob + "</td> </tr><tr><td  align='left'><b>Recorded :</b></td><td align='left'>"
       + InfoboxContent.Device_DateTime + "</td>"//<tr><td align='left'><b>Device No. :</b></td><td align='left'>"
       //    + InfoboxContent.Device_No + "</td> </tr><tr><td align='left'><b>IMEI No. :</b></td><td align='left'>"
       //    + InfoboxContent.IMEINo + "</td> </tr><tr><td align='left'><b>Sim No. :</b></td><td align='left'>"
      // + InfoboxContent.SimNo
      // + "</td></tr>
       //+ "<td align='left'><b>Trip No. :</b></td><td align='left'>"
       //+ InfoboxContent.Trip_No
       //+ "</td></tr><tr><td align='left'><b>ATD :</b></td><td align='left'>"
       //+ InfoboxContent.ATD
       //+ "</td><td align='left'><b>Travel Date :</b></td><td align='left'>"
       //+ InfoboxContent.Travel_Date
       //+ "</td></tr><tr><td align='left'><b>ATD :</b></td><td align='left'>"
       //+ InfoboxContent.ATD
      // + "</td></tr>"//<tr><td align='left'><b>ATA:</b></td><td align='left'>"
       //+ InfoboxContent.ATA
       //+ "</td></tr>"
        // + "</td></tr><tr><td align='left'><b>Alarm :</b></td><td colspan='3' align='left'>"
       //+ InfoboxContent.AlarmDesc

        + "</td></tr><tr><td align='left'><b>ATD :</b></td><td align='left'>"
       + InfoboxContent.ATD

        + "</td></tr><tr><td align='left'><b>Location :</b></td><td align='left'>"
       + InfoboxContent.Location


       + "</td></tr>"
       //comment by anjani
       + '<tr><td align="left"></td><td align="left"><a href="javascript:void(0)" onclick="SendInboundTripLiveTracking(\'' + InfoboxContent.Registration_No + '\'' + ',\'' + InfoboxContent.Trip_No + '\'' + ',\'' + InfoboxContent.FK_CompanyId + '\'' + ',\'' + InfoboxContent.FK_Start_Geo_ID + '\'' + ',\'' + InfoboxContent.FK_End_Geo_ID + '\'' + ',\'' + InfoboxContent.Trans_Trip_ID + '\'' + ',\'' + 1 + '\'' + ',\'' + InfoboxContent.TSInDT + '\');">Live Tracking</a>'
         //+ '<tr><td align="left"></td><td align="left"><a href="javascript:void(0)" onclick="SendInboundTripLiveTracking(\'' + InfoboxContent.Registration_No + '\'' + ',\'' + InfoboxContent.Trip_No + '\'' + ',\'' + InfoboxContent.FK_CompanyId + '\'' + ',\'' + 1 + '\'' + ',\'' + InfoboxContent.TSInDT + '\');">Live Tracking</a>'
         + "</td></tr>"
       + "</table></div>";
        }
    }
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

/*Calls Live Tracking Function For Live Tracking Of Inbound TIRPS*/
function SendInboundTripLiveTracking(RegistrationNo, tripno, FK_CompanyId, _StartGeoId, _EndGeoId, _TripId, IsSimtrip, atd) {

    var Simtrip = 'false';
    if (IsSimtrip == 1) {
        Simtrip = 'true'
    }
    //alert('SendInboundTripLiveTracking');
    $.ajax({
        url: '../TMS/InboundTripDashboard/InboundTripLiveTrackingData',
        //  url: "@Url.Action("GetMachineList", "Tracking")",
        type: "GET",
        dataType: "JSON",
        data: { CompId: FK_CompanyId, regNo: RegistrationNo, tripNo: tripno, IsSimTrip: Simtrip, ATD: atd, StartGeoId: _StartGeoId, EndGeoId: _EndGeoId, TripId: _TripId },
        success: function (r) {

            var rst = r;
            if (rst == 1) {
                window.location.href = "../TMS/InboundTripDashboard/InboundTripLiveTracking";
            }
        }
    });
}

/*Create Trip Marker With InfoBox*/
/*Created By : Vinish*/
/*Created Datetime : 2021-03-23 11:07:31.030*/
function CTSCreateInboundTripMarkerWithInfoBoxforLiveTracking(InfoboxContent, bOpenNewInfoBox, bClosePreviousInfoBox, id) {
    _marker = new google.maps.Marker({
        map: _map,
        position: new google.maps.LatLng(InfoboxContent.Current_Lat, InfoboxContent.Current_Long)
    });
    ClearMap();
    if (InfoboxContent.FK_CompanyId == '20' || InfoboxContent.FK_CompanyId == '268' || InfoboxContent.FK_CompanyId == '1009') {
        var str = RotateIcon.makeIcon('/App_Images/' + InfoboxContent.Icon).setRotation({ deg: InfoboxContent.Heading }).getUrl();
        var a = endsWith(str, "AD12TscoAAAAAElFTkSuQmCC");
        if (endsWith(str, "AD12TscoAAAAAElFTkSuQmCC"))
            _marker.setIcon('/App_Images/' + InfoboxContent.Icon);
        else
            _marker.setIcon(RotateIcon.makeIcon('/App_Images/' + InfoboxContent.Icon).setRotation({ deg: InfoboxContent.Heading }).getUrl());
    }
    else {
        _marker.setIcon('/App_Images/' + InfoboxContent.Icon);
    }
    //  _marker.id = InfoboxContent.length;      
    //var str = RotateIcon.makeIcon('/App_Images/' + InfoboxContent.Icon).setRotation({ deg: InfoboxContent.Heading }).getUrl();
    //// alert(str);


    ////By Tarique, Suggested by Deepak SIR, on 9June_14:40PM  :: Previously just below code block was implemented
    //if (endsWith(str, "AD12TscoAAAAAElFTkSuQmCC")) {
    //    _marker.setIcon('/App_Images/' + InfoboxContent.Icon);
    //}
    //else {
    //    _marker.setIcon(RotateIcon.makeIcon('/App_Images/' + InfoboxContent.Icon).setRotation({ deg: InfoboxContent.Heading }).getUrl());
    //}

    if (_infowindow && (bClosePreviousInfoBox == "1" || bClosePreviousInfoBox == "")) {
        _infowindow.close();
    }
    var livetrack = "display:none;";
    if (InfoboxContent.Status == 'P') {
        livetrack = "display:inline-block;"
    }
    _infowindow = new google.maps.InfoWindow();
    _content = "<div class='popup'><table class='info-table'><tr align='left'><td colspan='2'><b>"

    + InfoboxContent.VehicleNo + "</b></td><td><b>"
    + InfoboxContent.ModelName + "</b></td></tr><tr nowrap><td width='90px' align='left'><b>Driver Name:</b></td><td align='left'>"
    + InfoboxContent.ChauffeurName + "</td></tr><tr><td  align='left'><b>Driver Mob:</b></td><td align='left'>"
    + InfoboxContent.ChauffeurMob + "</td> </tr><tr><td  align='left'><b>Recorded :</b></td><td align='left'>"
    + InfoboxContent.DeviceDateTime + "</td> </tr><tr><td align='left'><b>Speed :</b></td><td align='left'>"
    + InfoboxContent.Speed + " Km/Hr</td> </tr><tr><td align='left'><b>Device No. :</b></td><td align='left'>"
    + InfoboxContent.DeviceNo + "</td></tr><tr><td align='left'><b>IMEI No. :</b></td><td align='left'>"
    + InfoboxContent.IMEINo + "</td> </tr><tr><td align='left'><b>Sim No. :</b></td><td align='left'>"
    + InfoboxContent.SIMNo
    //+ "</td></tr>"
    //+ '<tr><td align="left"></td><td align="left"><a style=' + livetrack + '  href="#" onclick="SendLiveTracking(\'' + InfoboxContent.RegistrationNo + '\'' + ',' + InfoboxContent.FK_CompanyId + ');">Live Tracking</a>'
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
    _map.setCenter(new google.maps.LatLng(InfoboxContent.Current_Lat, InfoboxContent.Current_Long));
    _markerArray.push(_marker);
}



/*Create Trip Marker With InfoBox*/
function FlipkartCreateInboundTripMarkerWithInfoBoxforLiveTracking(InfoboxContent, bOpenNewInfoBox, bClosePreviousInfoBox, id) {
    //debugger
    //alert('CreateInboundTripMarkerWithInfoBoxforLiveTracking')
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

    if (_infowindow && (bClosePreviousInfoBox == "1" || bClosePreviousInfoBox == "")) {
        _infowindow.close();
    }
    var livetrack = "display:none;";
    if (InfoboxContent.Status == 'P') {
        livetrack = "display:inline-block;"
    }
    _infowindow = new google.maps.InfoWindow();
    if (InfoboxContent.EndAddress != "" || InfoboxContent.WayPtAddress!="") {
        //if (!InfoboxContent.IsSimTrip && !InfoboxContent.IsElockTrip) {
        if (!InfoboxContent.IsElockTrip) {
            _content =
                "<div class='popup' style='width:450px;'><table class='info-table' width='100%'><tr align='left'><td colspan='4'><b style='margin-bottom:5px;display:block;font-size:14px;color:#e90a0b'>" + InfoboxContent.Route_Name + "</b></td></tr><tr align='left'><td width='90px' align='left'><b>Vehicle No.:</b></td><td  colspan='3'><b>"
            + InfoboxContent.Registration_No + "</b></td></tr><tr nowrap><td width='90px' align='left'><b>Driver Name:</b></td><td align='left'>"
            + InfoboxContent.ChauffeurName + "</td><td  align='left'><b>Driver Mob:</b></td><td align='left'>"
            + InfoboxContent.ChauffeurMob + "</td> </tr><tr><td  align='left'><b>Recorded :</b></td><td align='left'>"
            + InfoboxContent.Device_DateTime + "</td>"//<tr><td align='left'><b>Device No. :</b></td><td align='left'>"
            //    + InfoboxContent.Device_No + "</td> </tr><tr><td align='left'><b>IMEI No. :</b></td><td align='left'>"
            //    + InfoboxContent.IMEINo + "</td> </tr><tr><td align='left'><b>Sim No. :</b></td><td align='left'>"
           // + InfoboxContent.SimNo
           // + "</td></tr>
            + "<td align='left'><b>Trip No. :</b></td><td align='left'>"
            + InfoboxContent.Trip_No
            + "</td></tr><tr><td align='left'><b>Travel Date :</b></td><td align='left'>"
            + InfoboxContent.Travel_Date
            + "</td></tr><tr><td align='left'><b>ATD :</b></td><td align='left'>"
            + InfoboxContent.ATD
            + "</td></tr>"//<tr><td align='left'><b>ATA:</b></td><td align='left'>"
            //+ InfoboxContent.ATA
            //+ "</td></tr>"

              + "</td></tr><tr><td align='left'><b>Location :</b></td><td align='left'>"
            + InfoboxContent.Location


              + "</td></tr><tr><td align='left'><b>Alarm :</b></td><td colspan='3' align='left'>"
            + InfoboxContent.AlarmDesc
            + "</td></tr>"
            //comment by anjani
              //+ '<tr><td align="left"></td><td align="left"><a href="javascript:void(0)" onclick="ShowTrafficOnMapForInbound(\'' + InfoboxContent.Trip_No + '\');">Live Traffic</a>'
       //     + "</td></tr><tr><td align='left'><b>End Address :</b></td><td align='left'>"
       //     + InfoboxContent.EndAddress


       //     + "</td><td align='left'><b>Way Point Address :</b></td><td align='left'>"
       //     + InfoboxContent.WayPtAddress
       //      + "</td></tr><tr><td align='left'><b>ShipmentNo. :</b></td><td align='left'>"
       //+ InfoboxContent.ShipmentNo

       //+ "</td></tr>"
            + "</table></div>";
        } else if (InfoboxContent.IsElockTrip) {
            _content = "<div class='popup' style='width:450px;'><table class='info-table' width='100%'><tr align='left'><td width='90px' align='left'><b>Vehicle No.:</b></td><td  colspan='3'><b>"
       + InfoboxContent.Registration_No + "</b></td></tr><tr nowrap><td width='90px' align='left'><b>Driver Name:</b></td><td align='left'>"
       + InfoboxContent.ChauffeurName + "</td><td  align='left'><b>Driver Mob:</b></td><td align='left'>"
       + InfoboxContent.ChauffeurMob + "</td> </tr><tr><td  align='left'><b>Recorded :</b></td><td align='left'>"
       + ((InfoboxContent.Device_DateTime == null || InfoboxContent.Device_DateTime == "null") ? "NA" : InfoboxContent.Device_DateTime) + "</td>"//<tr><td align='left'><b>Device No. :</b></td><td align='left'>"
       //    + InfoboxContent.Device_No + "</td> </tr><tr><td align='left'><b>IMEI No. :</b></td><td align='left'>"
       //    + InfoboxContent.IMEINo + "</td> </tr><tr><td align='left'><b>Sim No. :</b></td><td align='left'>"
      // + InfoboxContent.SimNo
      // + "</td></tr>
       //+ "<td align='left'><b>Trip No. :</b></td><td align='left'>"
       //+ InfoboxContent.Trip_No
       //+ "</td></tr><tr><td align='left'><b>ATD :</b></td><td align='left'>"
       //+ InfoboxContent.ATD
       //+ "</td><td align='left'><b>Travel Date :</b></td><td align='left'>"
       //+ InfoboxContent.Travel_Date
       //+ "</td></tr><tr><td align='left'><b>ATD :</b></td><td align='left'>"
       //+ InfoboxContent.ATD
      // + "</td></tr>"//<tr><td align='left'><b>ATA:</b></td><td align='left'>"
       //+ InfoboxContent.ATA
       //+ "</td></tr>"
        // + "</td></tr><tr><td align='left'><b>Alarm :</b></td><td colspan='3' align='left'>"
       //+ InfoboxContent.AlarmDesc
       + "</td></tr>"

       + "</td></tr><tr><td align='left'><b>ATD :</b></td><td align='left'>"
            + InfoboxContent.ATD


            + "</td></tr><tr><td align='left'><b>Location :</b></td><td align='left'>"
            + InfoboxContent.Location

       //comment by anjani
        //+ '<tr><td align="left"></td><td align="left"><a href="javascript:void(0)" onclick="ShowTrafficOnMapForInbound(\'' + InfoboxContent.Trip_No + '\');">Live Traffic</a>'
       //  + "</td></tr><tr><td align='left'><b>End Address :</b></td><td align='left'>"
       //     + InfoboxContent.EndAddress


       //     + "</td><td align='left'><b>Way Point Address :</b></td><td align='left'>"
       //     + InfoboxContent.WayPtAddress
       //      + "</td></tr><tr><td align='left'><b>ShipmentNo. :</b></td><td align='left'>"
       //+ InfoboxContent.ShipmentNo

       //+ "</td></tr>"
       + "</table></div>";
        } else {

        }
    } else {
        if (!InfoboxContent.IsSimTrip && !InfoboxContent.IsElockTrip) {
            _content =
                "<div class='popup' style='width:450px;'><table class='info-table' width='100%'><tr align='left'><td colspan='4'><b style='margin-bottom:5px;display:block;font-size:14px;color:#e90a0b'>" + InfoboxContent.Route_Name + "</b></td></tr><tr align='left'><td width='90px' align='left'><b>Vehicle No.:</b></td><td  colspan='3'><b>"
            + InfoboxContent.Registration_No + "</b></td></tr><tr nowrap><td width='90px' align='left'><b>Driver Name:</b></td><td align='left'>"
            + InfoboxContent.ChauffeurName + "</td><td  align='left'><b>Driver Mob:</b></td><td align='left'>"
            + InfoboxContent.ChauffeurMob + "</td> </tr><tr><td  align='left'><b>Recorded :</b></td><td align='left'>"
            + InfoboxContent.Device_DateTime + "</td>"//<tr><td align='left'><b>Device No. :</b></td><td align='left'>"
            //    + InfoboxContent.Device_No + "</td> </tr><tr><td align='left'><b>IMEI No. :</b></td><td align='left'>"
            //    + InfoboxContent.IMEINo + "</td> </tr><tr><td align='left'><b>Sim No. :</b></td><td align='left'>"
           // + InfoboxContent.SimNo
           // + "</td></tr>
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

              + "</td></tr><tr><td align='left'><b>Location :</b></td><td align='left'>"
            + InfoboxContent.Location


              + "</td></tr><tr><td align='left'><b>Alarm :</b></td><td colspan='3' align='left'>"
            + InfoboxContent.AlarmDesc
            + "</td></tr>"
            //comment by anjani
              //+ '<tr><td align="left"></td><td align="left"><a href="javascript:void(0)" onclick="ShowTrafficOnMapForInbound(\'' + InfoboxContent.Trip_No + '\');">Live Traffic</a>'
            + "</td></tr>"
            + "</table></div>";
        } else if (InfoboxContent.IsElockTrip) {
            _content = "<div class='popup' style='width:450px;'><table class='info-table' width='100%'><tr align='left'><td width='90px' align='left'><b>Vehicle No.:</b></td><td  colspan='3'><b>"
       + InfoboxContent.Registration_No + "</b></td></tr><tr nowrap><td width='90px' align='left'><b>Driver Name:</b></td><td align='left'>"
       + InfoboxContent.ChauffeurName + "</td><td  align='left'><b>Driver Mob:</b></td><td align='left'>"
       + InfoboxContent.ChauffeurMob + "</td> </tr><tr><td  align='left'><b>Recorded :</b></td><td align='left'>"
       + ((InfoboxContent.Device_DateTime == null || InfoboxContent.Device_DateTime == "null") ? "NA" : InfoboxContent.Device_DateTime) + "</td>"//<tr><td align='left'><b>Device No. :</b></td><td align='left'>"
       //    + InfoboxContent.Device_No + "</td> </tr><tr><td align='left'><b>IMEI No. :</b></td><td align='left'>"
       //    + InfoboxContent.IMEINo + "</td> </tr><tr><td align='left'><b>Sim No. :</b></td><td align='left'>"
      // + InfoboxContent.SimNo
      // + "</td></tr>
       //+ "<td align='left'><b>Trip No. :</b></td><td align='left'>"
       //+ InfoboxContent.Trip_No
       //+ "</td></tr><tr><td align='left'><b>ATD :</b></td><td align='left'>"
       //+ InfoboxContent.ATD
       //+ "</td><td align='left'><b>Travel Date :</b></td><td align='left'>"
       //+ InfoboxContent.Travel_Date
       //+ "</td></tr><tr><td align='left'><b>ATD :</b></td><td align='left'>"
       //+ InfoboxContent.ATD
      // + "</td></tr>"//<tr><td align='left'><b>ATA:</b></td><td align='left'>"
       //+ InfoboxContent.ATA
       //+ "</td></tr>"
        // + "</td></tr><tr><td align='left'><b>Alarm :</b></td><td colspan='3' align='left'>"
       //+ InfoboxContent.AlarmDesc
       + "</td></tr>"

       + "</td></tr><tr><td align='left'><b>ATD :</b></td><td align='left'>"
            + InfoboxContent.ATD


            + "</td></tr><tr><td align='left'><b>Location :</b></td><td align='left'>"
            + InfoboxContent.Location

       //comment by anjani
        //+ '<tr><td align="left"></td><td align="left"><a href="javascript:void(0)" onclick="ShowTrafficOnMapForInbound(\'' + InfoboxContent.Trip_No + '\');">Live Traffic</a>'
         + "</td></tr>"
       + "</table></div>";
        } else {

        }
    }
    

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

/*Create Trip Marker With InfoBox*/
function CreateInboundTripMarkerWithInfoBoxforLiveTracking(InfoboxContent, bOpenNewInfoBox, bClosePreviousInfoBox, id) {
    //debugger
    //alert('CreateInboundTripMarkerWithInfoBoxforLiveTracking')
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

    if (_infowindow && (bClosePreviousInfoBox == "1" || bClosePreviousInfoBox == "")) {
        _infowindow.close();
    }
    var livetrack = "display:none;";
    if (InfoboxContent.Status == 'P') {
        livetrack = "display:inline-block;"
    }
    _infowindow = new google.maps.InfoWindow();
    if (InfoboxContent.EndAddress != "" || InfoboxContent.WayPtAddress != "") {
        //if (!InfoboxContent.IsSimTrip && !InfoboxContent.IsElockTrip) {
        if (!InfoboxContent.IsElockTrip) {
            _content =
                "<div class='popup' style='width:450px;'><table class='info-table' width='100%'><tr align='left'><td colspan='4'><b style='margin-bottom:5px;display:block;font-size:14px;color:#e90a0b'>" + InfoboxContent.Route_Name + "</b></td></tr><tr align='left'><td width='90px' align='left'><b>Vehicle No.:</b></td><td  colspan='3'><b>"
            + InfoboxContent.Registration_No + "</b></td></tr><tr nowrap><td width='90px' align='left'><b>Driver Name:</b></td><td align='left'>"
            + InfoboxContent.ChauffeurName + "</td><td  align='left'><b>Driver Mob:</b></td><td align='left'>"
            + InfoboxContent.ChauffeurMob + "</td> </tr><tr><td  align='left'><b>Recorded :</b></td><td align='left'>"
            + InfoboxContent.Device_DateTime + "</td>"//<tr><td align='left'><b>Device No. :</b></td><td align='left'>"
            //    + InfoboxContent.Device_No + "</td> </tr><tr><td align='left'><b>IMEI No. :</b></td><td align='left'>"
            //    + InfoboxContent.IMEINo + "</td> </tr><tr><td align='left'><b>Sim No. :</b></td><td align='left'>"
           // + InfoboxContent.SimNo
           // + "</td></tr>
            + "<td align='left'><b>Trip No. :</b></td><td align='left'>"
            + InfoboxContent.Trip_No
            + "</td></tr><tr><td align='left'><b>Travel Date :</b></td><td align='left'>"
            + InfoboxContent.Travel_Date
            + "</td></tr><tr><td align='left'><b>ATD :</b></td><td align='left'>"
            + InfoboxContent.ATD
            + "</td></tr>"//<tr><td align='left'><b>ATA:</b></td><td align='left'>"
            //+ InfoboxContent.ATA
            //+ "</td></tr>"

              + "</td></tr><tr><td align='left'><b>Location :</b></td><td align='left'>"
            + InfoboxContent.Location


              + "</td></tr><tr><td align='left'><b>Alarm :</b></td><td colspan='3' align='left'>"
            + InfoboxContent.AlarmDesc
            + "</td></tr>"
            //comment by anjani
              //+ '<tr><td align="left"></td><td align="left"><a href="javascript:void(0)" onclick="ShowTrafficOnMapForInbound(\'' + InfoboxContent.Trip_No + '\');">Live Traffic</a>'
            + "</td></tr><tr><td align='left'><b>End Address :</b></td><td align='left'>"
            + InfoboxContent.EndAddress


            + "</td><td align='left'><b>Way Point Address :</b></td><td align='left'>"
            + InfoboxContent.WayPtAddress
             + "</td></tr><tr><td align='left'><b>ShipmentNo. :</b></td><td align='left'>"
       + InfoboxContent.ShipmentNo

       + "</td></tr>"
            + "</table></div>";
        } else if (InfoboxContent.IsElockTrip) {
            _content = "<div class='popup' style='width:450px;'><table class='info-table' width='100%'><tr align='left'><td width='90px' align='left'><b>Vehicle No.:</b></td><td  colspan='3'><b>"
       + InfoboxContent.Registration_No + "</b></td></tr><tr nowrap><td width='90px' align='left'><b>Driver Name:</b></td><td align='left'>"
       + InfoboxContent.ChauffeurName + "</td><td  align='left'><b>Driver Mob:</b></td><td align='left'>"
       + InfoboxContent.ChauffeurMob + "</td> </tr><tr><td  align='left'><b>Recorded :</b></td><td align='left'>"
       + ((InfoboxContent.Device_DateTime == null || InfoboxContent.Device_DateTime == "null") ? "NA" : InfoboxContent.Device_DateTime) + "</td>"//<tr><td align='left'><b>Device No. :</b></td><td align='left'>"
       //    + InfoboxContent.Device_No + "</td> </tr><tr><td align='left'><b>IMEI No. :</b></td><td align='left'>"
       //    + InfoboxContent.IMEINo + "</td> </tr><tr><td align='left'><b>Sim No. :</b></td><td align='left'>"
      // + InfoboxContent.SimNo
      // + "</td></tr>
       //+ "<td align='left'><b>Trip No. :</b></td><td align='left'>"
       //+ InfoboxContent.Trip_No
       //+ "</td></tr><tr><td align='left'><b>ATD :</b></td><td align='left'>"
       //+ InfoboxContent.ATD
       //+ "</td><td align='left'><b>Travel Date :</b></td><td align='left'>"
       //+ InfoboxContent.Travel_Date
       //+ "</td></tr><tr><td align='left'><b>ATD :</b></td><td align='left'>"
       //+ InfoboxContent.ATD
      // + "</td></tr>"//<tr><td align='left'><b>ATA:</b></td><td align='left'>"
       //+ InfoboxContent.ATA
       //+ "</td></tr>"
        // + "</td></tr><tr><td align='left'><b>Alarm :</b></td><td colspan='3' align='left'>"
       //+ InfoboxContent.AlarmDesc
       + "</td></tr>"

       + "</td></tr><tr><td align='left'><b>ATD :</b></td><td align='left'>"
            + InfoboxContent.ATD


            + "</td></tr><tr><td align='left'><b>Location :</b></td><td align='left'>"
            + InfoboxContent.Location

       //comment by anjani
        //+ '<tr><td align="left"></td><td align="left"><a href="javascript:void(0)" onclick="ShowTrafficOnMapForInbound(\'' + InfoboxContent.Trip_No + '\');">Live Traffic</a>'
         + "</td></tr><tr><td align='left'><b>End Address :</b></td><td align='left'>"
            + InfoboxContent.EndAddress


            + "</td><td align='left'><b>Way Point Address :</b></td><td align='left'>"
            + InfoboxContent.WayPtAddress
             + "</td></tr><tr><td align='left'><b>ShipmentNo. :</b></td><td align='left'>"
       + InfoboxContent.ShipmentNo

       + "</td></tr>"
       + "</table></div>";
        } else {

        }
    } else {
        if (!InfoboxContent.IsSimTrip && !InfoboxContent.IsElockTrip) {
            _content =
                "<div class='popup' style='width:450px;'><table class='info-table' width='100%'><tr align='left'><td colspan='4'><b style='margin-bottom:5px;display:block;font-size:14px;color:#e90a0b'>" + InfoboxContent.Route_Name + "</b></td></tr><tr align='left'><td width='90px' align='left'><b>Vehicle No.:</b></td><td  colspan='3'><b>"
            + InfoboxContent.Registration_No + "</b></td></tr><tr nowrap><td width='90px' align='left'><b>Driver Name:</b></td><td align='left'>"
            + InfoboxContent.ChauffeurName + "</td><td  align='left'><b>Driver Mob:</b></td><td align='left'>"
            + InfoboxContent.ChauffeurMob + "</td> </tr><tr><td  align='left'><b>Recorded :</b></td><td align='left'>"
            + InfoboxContent.Device_DateTime + "</td>"//<tr><td align='left'><b>Device No. :</b></td><td align='left'>"
            //    + InfoboxContent.Device_No + "</td> </tr><tr><td align='left'><b>IMEI No. :</b></td><td align='left'>"
            //    + InfoboxContent.IMEINo + "</td> </tr><tr><td align='left'><b>Sim No. :</b></td><td align='left'>"
           // + InfoboxContent.SimNo
           // + "</td></tr>
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

              + "</td></tr><tr><td align='left'><b>Location :</b></td><td align='left'>"
            + InfoboxContent.Location


              + "</td></tr><tr><td align='left'><b>Alarm :</b></td><td colspan='3' align='left'>"
            + InfoboxContent.AlarmDesc
            + "</td></tr>"
            //comment by anjani
              //+ '<tr><td align="left"></td><td align="left"><a href="javascript:void(0)" onclick="ShowTrafficOnMapForInbound(\'' + InfoboxContent.Trip_No + '\');">Live Traffic</a>'
            + "</td></tr>"
            + "</table></div>";
        } else if (InfoboxContent.IsElockTrip) {
            _content = "<div class='popup' style='width:450px;'><table class='info-table' width='100%'><tr align='left'><td width='90px' align='left'><b>Vehicle No.:</b></td><td  colspan='3'><b>"
       + InfoboxContent.Registration_No + "</b></td></tr><tr nowrap><td width='90px' align='left'><b>Driver Name:</b></td><td align='left'>"
       + InfoboxContent.ChauffeurName + "</td><td  align='left'><b>Driver Mob:</b></td><td align='left'>"
       + InfoboxContent.ChauffeurMob + "</td> </tr><tr><td  align='left'><b>Recorded :</b></td><td align='left'>"
       + ((InfoboxContent.Device_DateTime == null || InfoboxContent.Device_DateTime == "null") ? "NA" : InfoboxContent.Device_DateTime) + "</td>"//<tr><td align='left'><b>Device No. :</b></td><td align='left'>"
       //    + InfoboxContent.Device_No + "</td> </tr><tr><td align='left'><b>IMEI No. :</b></td><td align='left'>"
       //    + InfoboxContent.IMEINo + "</td> </tr><tr><td align='left'><b>Sim No. :</b></td><td align='left'>"
      // + InfoboxContent.SimNo
      // + "</td></tr>
       //+ "<td align='left'><b>Trip No. :</b></td><td align='left'>"
       //+ InfoboxContent.Trip_No
       //+ "</td></tr><tr><td align='left'><b>ATD :</b></td><td align='left'>"
       //+ InfoboxContent.ATD
       //+ "</td><td align='left'><b>Travel Date :</b></td><td align='left'>"
       //+ InfoboxContent.Travel_Date
       //+ "</td></tr><tr><td align='left'><b>ATD :</b></td><td align='left'>"
       //+ InfoboxContent.ATD
      // + "</td></tr>"//<tr><td align='left'><b>ATA:</b></td><td align='left'>"
       //+ InfoboxContent.ATA
       //+ "</td></tr>"
        // + "</td></tr><tr><td align='left'><b>Alarm :</b></td><td colspan='3' align='left'>"
       //+ InfoboxContent.AlarmDesc
       + "</td></tr>"

       + "</td></tr><tr><td align='left'><b>ATD :</b></td><td align='left'>"
            + InfoboxContent.ATD


            + "</td></tr><tr><td align='left'><b>Location :</b></td><td align='left'>"
            + InfoboxContent.Location

       //comment by anjani
        //+ '<tr><td align="left"></td><td align="left"><a href="javascript:void(0)" onclick="ShowTrafficOnMapForInbound(\'' + InfoboxContent.Trip_No + '\');">Live Traffic</a>'
         + "</td></tr>"
       + "</table></div>";
        } else {

        }
    }


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

/*CODE FOR INBOUND TRIP MANAGEMENT SYSTEM ENDS*/

/*Create A Route With Infobox But Without Marker*/
function CreateRouteWithoutMarker(MapControlID, Zoom, CenterLat, CenterLng, InfoboxContent, PathColor, PathOpacity, PathWeight) {
    //alert(1)
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
                PathColor = '#65D873'
                if (i == 0 || i == InfoboxContent.length - 1) {
                    CreateMarkerWithInfoBox(InfoboxContent[i])
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
                    if (InfoboxContent[i].Icon == 'pin_yellow.png') {
                        PathColor = '#F3971E'
                    }
                    if (InfoboxContent[i].Icon == 'pin_green.png') {
                        PathColor = '#65D873'
                    }
                    if (InfoboxContent[i].Icon == 'pin_blue.png') {
                        PathColor = '#2191ED'
                    }
                    if (InfoboxContent[i].Icon == 'pin_red.png') {
                        PathColor = '#EA0000'
                    }

                    if (goto < 1) {
                        if (k > 0) {
                            var _vehTrackCoordinates = [new google.maps.LatLng(InfoboxContent[i].Lat,
                                                InfoboxContent[i].Long),
                                            new google.maps.LatLng(InfoboxContent[k].Lat,
                                                InfoboxContent[k].Long)
                            ];

                        } else if (InfoboxContent[i].ISGSM == 'OFF') {

                            var _vehTrackCoordinates = [new google.maps.LatLng(InfoboxContent[i].Lat,
                                                     InfoboxContent[i].Long),
                                                 new google.maps.LatLng(InfoboxContent[i - 1].Lat,
                                                     InfoboxContent[i - 1].Long)
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
                _markerArray[endlenth - 1].setAnimation(google.maps.Animation.BOUNCE);
                _map.setCenter(new google.maps.LatLng(InfoboxContent[InfoboxContent.length - 1].Lat, InfoboxContent[InfoboxContent.length - 1].Long));
            }

        } else {
            // alert("Marker list not pass !");
        }
    } catch (Error) {
        // alert("Problem in Create Route :" + Error.message);
    }
}

/*Plays Route Without Markers STARTS*/
function PlayRouteWithoutMarker(MapControlID, Zoom, CenterLat, CenterLng, InfoboxContent, PathColor, PathOpacity, PathWeight, TimerIntervalMS, bAlwayOpenInfoBox, IsPanEnable) {
    //alert('PlayRouteWithoutMarker')
    try {

        ClearMap();
        _counter = 0;
        _InfoboxContent = InfoboxContent;
        trHTML = '';
        _SetInterval = setInterval('PlayWithoutMarker(' + '"' + PathColor + '",' + PathOpacity + ',' + PathWeight + ',' + bAlwayOpenInfoBox + ',' + IsPanEnable + ')', TimerIntervalMS);
    } catch (Error) {
        // alert("Problem in Create Route :" + Error.message);
    }
}
function PlayWithoutMarker(PathColor, PathOpacity, PathWeight, bAlwayOpenInfoBox, IsPanEnable) {
    if (_counter <= _InfoboxContent.length - 1) {

        var prelat;
        var prelong;
        var preISGSM = 'OFF'
        var j = 0;
        var k = 0;
        var goto = 0;

        if (_counter == 0 || _counter == _InfoboxContent.length - 1) {
            CreateMarkerWithInfoBox(_InfoboxContent[_counter])
        }

        CreateTableHistoryBody(_InfoboxContent[_counter]);

        PathColor = '#65D873'

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
            if (_InfoboxContent[_counter].Icon == 'pin_yellow.png') {
                PathColor = '#F3971E'
            }
            if (_InfoboxContent[_counter].Icon == 'pin_green.png') {
                PathColor = '#65D873'
            }
            if (_InfoboxContent[_counter].Icon == 'pin_blue.png') {
                PathColor = '#2191ED'
            }
            if (_InfoboxContent[_counter].Icon == 'pin_red.png') {
                PathColor = '#EA0000'
            }

            if (goto < 1) {

                if (k > 0) {
                    var _vehTrackCoordinates = [new google.maps.LatLng(_InfoboxContent[_counter].Lat,
                                        _InfoboxContent[_counter].Long),
                                    new google.maps.LatLng(_InfoboxContent[k].Lat,
                                        _InfoboxContent[k].Long)
                    ];

                } else if (_InfoboxContent[_counter].ISGSM == 'OFF') {

                    var _vehTrackCoordinates = [new google.maps.LatLng(_InfoboxContent[_counter].Lat,
                                             _InfoboxContent[_counter].Long),
                                         new google.maps.LatLng(_InfoboxContent[_counter - 1].Lat,
                                             _InfoboxContent[_counter - 1].Long)
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
                    _map.panTo(new google.maps.LatLng(_InfoboxContent[_counter].Lat,
                                                   _InfoboxContent[_counter].Long));
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
function PlayRouteWithoutMarkerforplaypause(MapControlID, Zoom, CenterLat, CenterLng, InfoboxContent, PathColor, PathOpacity, PathWeight, TimerIntervalMS, bAlwayOpenInfoBox, IsPanEnable) {
    //alert('PlayRouteWithoutMarker')
    try {

        ClearMap();
        _counter = 0;
        _InfoboxContent = InfoboxContent;
        trHTML = '';
        _SetInterval = setInterval('PlayWithoutMarkerforplaypause(' + '"' + PathColor + '",' + PathOpacity + ',' + PathWeight + ',' + bAlwayOpenInfoBox + ',' + IsPanEnable + ')', TimerIntervalMS);
    } catch (Error) {
        // alert("Problem in Create Route :" + Error.message);
    }
}

function PlayWithoutMarkerforplaypause(PathColor, PathOpacity, PathWeight, bAlwayOpenInfoBox, IsPanEnable) {
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
            //$("#datadiv" + slotId + "").show();
            //$("#ANDCriteria" + slotId + "").show();
            //$("#iconsdata" + slotId + "").show();
            //$("#datadiv" + slotId + "").find('#from_to').text(mydata.receivedTime);
            //$("#datadiv" + slotId + "").find('#engineon').text(mydata.gpstime);
            //$("#datadiv" + slotId + "").find('#speed').text(mydata.speed);
            //$("#datadiv" + slotId + "").find('#distance').text(mydata.Distance);
            //$("#datadiv" + slotId + "").find('#add').text(mydata.location + " ," + mydata.latitude + " ," + mydata.longitude);

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
        var prelat;
        var prelong;
        var preISGSM = 'OFF'
        var j = 0;
        var k = 0;
        var goto = 0;

        if (_counter == 0 || _counter == _InfoboxContent.length - 1) {
            CreateMarkerWithInfoBox(_InfoboxContent[_counter])
        }

        CreateTableHistoryBody(_InfoboxContent[_counter]);

        PathColor = '#65D873'

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
            if (_InfoboxContent[_counter].Icon == 'pin_yellow.png') {
                PathColor = '#F3971E'
            }
            if (_InfoboxContent[_counter].Icon == 'pin_green.png') {
                PathColor = '#65D873'
            }
            if (_InfoboxContent[_counter].Icon == 'pin_blue.png') {
                PathColor = '#2191ED'
            }
            if (_InfoboxContent[_counter].Icon == 'pin_red.png') {
                PathColor = '#EA0000'
            }

            if (goto < 1) {

                if (k > 0) {
                    var _vehTrackCoordinates = [new google.maps.LatLng(_InfoboxContent[_counter].Lat,
                                        _InfoboxContent[_counter].Long),
                                    new google.maps.LatLng(_InfoboxContent[k].Lat,
                                        _InfoboxContent[k].Long)
                    ];

                } else if (_InfoboxContent[_counter].ISGSM == 'OFF') {

                    var _vehTrackCoordinates = [new google.maps.LatLng(_InfoboxContent[_counter].Lat,
                                             _InfoboxContent[_counter].Long),
                                         new google.maps.LatLng(_InfoboxContent[_counter - 1].Lat,
                                             _InfoboxContent[_counter - 1].Long)
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
                    _map.panTo(new google.maps.LatLng(_InfoboxContent[_counter].Lat,
                                                   _InfoboxContent[_counter].Long));
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

/*Plays Route Without Markers END*/

//%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% Added By Abhishek [Code START] %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%//

function DrawCTMSTripMarker(MapControlID, Zoom, CenterLat, CenterLng, InfoboxContent) {
    try {
        var iVar = 0;
        ClearMap();

        if (InfoboxContent.length != 0) {
            for (var i = 0; i <= InfoboxContent.length - 1; i++) {
                CreateCTMSTripMarkerWithInfoBox(InfoboxContent[i])
                loc = new google.maps.LatLng(InfoboxContent[i].Lat, InfoboxContent[i].Long);
                bounds.extend(loc)
            }
            _map.fitBounds(bounds);
            _map.panToBounds(bounds);
        } else {
        }
    } catch (Error) {
    }
}

function CreateCTMSTripMarkerWithInfoBox(InfoboxContent, bOpenNewInfoBox, bClosePreviousInfoBox) {
    _marker = new google.maps.Marker({
        map: _map,
        position: new google.maps.LatLng(InfoboxContent.Lat, InfoboxContent.Long)
    });
    var str = RotateIcon.makeIcon('/App_Images/' + InfoboxContent.Icon).setRotation({ deg: InfoboxContent.Heading }).getUrl();
    var a = endsWith(str, "AD12TscoAAAAAElFTkSuQmCC");
    if (endsWith(str, "AD12TscoAAAAAElFTkSuQmCC") || endsWith(str, "AAAAAAAAAAAAAAAAH4NMPwAAZOMoNsAAAAASUVORK5CYII="))
        _marker.setIcon('/App_Images/' + InfoboxContent.Icon);
    else
        _marker.setIcon(RotateIcon.makeIcon('/App_Images/' + InfoboxContent.Icon).setRotation({ deg: InfoboxContent.Heading }).getUrl());

    if (_infowindow && (bClosePreviousInfoBox == "1" || bClosePreviousInfoBox == "")) {
        _infowindow.close();
    }
    var livetrack = "display:none;";
    if (InfoboxContent.Status == 'P') {
        livetrack = "display:inline-block;"
    }
    _infowindow = new google.maps.InfoWindow();

    _content = "<div class='popup' style='width:450px;'><table class='info-table' width='100%'><tr align='left'><td colspan='4'><b style='margin-bottom:5px;display:block;font-size:14px;color:#e90a0b'>" + InfoboxContent.Trip_No + "</b></td></tr>"
    + "<tr align='left'><td width='90px' align='left'><b>Vehicle No.<td align='center'>:</td></b></td><td  colspan='3'><b>"
    + InfoboxContent.Registration_No + "</b></td></tr>"
    + "</td></tr>"
    + "<tr><td align='left'><b>Location&nbsp <td align='center'>:&nbsp</td></b></td>&nbsp<td align='left'>&nbsp"
    + InfoboxContent.Location
    + "</td></tr>"
    + "<tr><td align='left'><b>Alert&nbsp <td align='center'>:&nbsp</td></b></td>&nbsp<td align='left'>&nbsp"
    + InfoboxContent.Alert + "</td></tr>"
    + "<tr><td align='left'><b>AC Status&nbsp <td align='center'>:&nbsp</td></b></td>&nbsp<td align='left'>&nbsp"
    + InfoboxContent.ACStatus
    + "</td></tr>"
    + "<tr><td align='left'><b>Door Status&nbsp <td align='center'>:&nbsp</td></b></td>&nbsp<td align='left'>&nbsp"
    + InfoboxContent.DoorStatus
    + "</td></tr>"
    + "<tr><td align='left'><b>Chamber Wise Temperature&nbsp <td align='center'>:&nbsp</td></b></td>&nbsp<td align='left'>&nbsp"
    + InfoboxContent.ChamberWiseTemp
    + "</td></tr>"
    + '<tr><td align="left"></td><td align="left"></td><td align="left"><a href="javascript:void(0)" onclick="SendCTMSTripLiveTracking(\'' + InfoboxContent.Registration_No + '\'' + ',\'' + InfoboxContent.Trip_No + '\'' + ',\'' + 0 + '\');">Live Tracking</a>'
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
    _markerArray.push(_marker);
}
var ICON = '';

function CreateCTMSTripMarkerWithInfoBoxforLiveTracking(InfoboxContent, counter, _StartIconList, id, bOpenNewInfoBox, bClosePreviousInfoBox) {
    if (counter == 0) {
        ICON = '/App_Images/flag-start.png';
    }
    else {
        ICON = '/App_Images/pin_green.png'
    }

    if (_StartIconList != null) {
        if (id == 0) {
            _marker.setMap(null);
            ICON = '/App_Images/flag-start.png';
            _marker = new google.maps.Marker({
                map: _map,
                position: new google.maps.LatLng(_StartIconList.Lat, _StartIconList.Long),
                icon: ICON
            });
        }
        else {
            ICON = '/App_Images/pin_green.png'
            _marker = new google.maps.Marker({
                map: _map,
                position: new google.maps.LatLng(InfoboxContent.Lat, InfoboxContent.Long),
                icon: ICON
            });
        }
    } else {
        _marker = new google.maps.Marker({
            map: _map,
            position: new google.maps.LatLng(InfoboxContent.Lat, InfoboxContent.Long),
            icon: ICON
        });
    }

    _infowindow.close();
    if (_infowindow && (bClosePreviousInfoBox == "1" || bClosePreviousInfoBox == "")) {
        _infowindow.close();
    }
    var livetrack = "display:none;";
    if (InfoboxContent.Status == 'P') {
        livetrack = "display:inline-block;"
    }
    _infowindow = new google.maps.InfoWindow();

    if (id == 0) {

        _content = "<div class='popup' style='width:450px;'><table class='info-table' width='100%'><tr align='left'><td colspan='5'><b style='margin-bottom:5px;display:block;font-size:14px;color:#e90a0b'>" + InfoboxContent.Trip_No + "</b></td></tr>"
            + "<tr align='left'><td width='90px' align='left' ><b>Vehicle No.&nbsp<td align='center'>:&nbsp</td></b></td><td colspan='3'><b>&nbsp"
            + _StartIconList.Registration_No + "</b></td></tr>"
            + "</td></tr>"
            + "<tr><td align='left'><b>Location&nbsp <td align='center'>:&nbsp</td></b></td>&nbsp<td align='left'>&nbsp"
            + _StartIconList.Location
            + "</td></tr>"
            + "<tr><td align='left'><b>Alert&nbsp <td align='center'>:&nbsp</td></b></td>&nbsp<td align='left'>&nbsp"
            + _StartIconList.Alert
            + "</td></tr>"
            + "<tr><td align='left'><b>AC Status&nbsp <td align='center'>:&nbsp</td></b></td>&nbsp<td align='left'>&nbsp"
            + _StartIconList.ACStatus
            + "</td></tr>"
            + "<tr><td align='left'><b>Door Status&nbsp <td align='center'>:&nbsp</td></b></td>&nbsp<td align='left'>&nbsp"
            + _StartIconList.DoorStatus
            + "</td></tr>"
            + "<tr><td align='left'><b>Chamber Wise Temperature&nbsp <td align='center'>:&nbsp</td></b></td>&nbsp<td align='left'>&nbsp"
            + _StartIconList.ChamberWiseTemp
            + "</td></tr>"
            + "</table></div>";
    }
    else {
        _content = "<div class='popup' style='width:450px;'><table class='info-table' width='100%'><tr align='left'><td colspan='5'><b style='margin-bottom:5px;display:block;font-size:14px;color:#e90a0b'>" + InfoboxContent.Trip_No + "</b></td></tr>"
    + "<tr align='left'><td width='90px' align='left' ><b>Vehicle No.&nbsp<td align='center'>:&nbsp</td></b></td><td colspan='3'><b>&nbsp"
    + InfoboxContent.Registration_No + "</b></td></tr>"
    + "</td></tr>"
    + "<tr><td align='left'><b>Location&nbsp <td align='center'>:&nbsp</td></b></td>&nbsp<td align='left'>&nbsp"
    + InfoboxContent.Location
    + "</td></tr>"
    + "<tr><td align='left'><b>Alert&nbsp <td align='center'>:&nbsp</td></b></td>&nbsp<td align='left'>&nbsp"
    + InfoboxContent.Alert
    + "</td></tr>"
    + "<tr><td align='left'><b>AC Status&nbsp <td align='center'>:&nbsp</td></b></td>&nbsp<td align='left'>&nbsp"
    + InfoboxContent.ACStatus
    + "</td></tr>"
    + "<tr><td align='left'><b>Door Status&nbsp <td align='center'>:&nbsp</td></b></td>&nbsp<td align='left'>&nbsp"
    + InfoboxContent.DoorStatus
    + "</td></tr>"
    + "<tr><td align='left'><b>Chamber Wise Temperature&nbsp <td align='center'>:&nbsp</td></b></td>&nbsp<td align='left'>&nbsp"
    + InfoboxContent.ChamberWiseTemp
    + "</td></tr>"
    + "</table></div>";
    }

    google.maps.event.addListener(_marker, 'click', (function (_marker, _content) {
        return function () {
            _infowindow.close();
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
//%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% Added By Abhishek [Code END] %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%//


/*************************** CTMS VEHICLE SCORE DASHBOARD ***********************************/

/*Function to load WithMovingDirection*/
function DrawMarkerWithMovingDirectionForVehicleDashboard(MapControlID, Zoom, CenterLat, CenterLng, InfoboxContent) {
    //debugger
    try {

        var iVar = 0;
        if (_markerArray.length > 0) {
            markerCluster.removeMarkers(_markerArray);
        }

        if (!$('#ModalforTodayactivity').hasClass('in')) { //Ref: https://stackoverflow.com/questions/19506672/how-to-check-if-bootstrap-modal-is-open-so-i-can-use-jquery-validate, Added By Tarik on 18 SEPT 18 :: Alt Code => $('#myModal').is(':visible');
            //debugger
            ClearMap();
        }

        if (InfoboxContent.length != 0) {
            for (var i = 0; i <= InfoboxContent.length - 1; i++) {
                //debugger;
                VehicleScoreDashboard(InfoboxContent[i]);

                _markerArray[i].setMap(_map);

                loc = new google.maps.LatLng(InfoboxContent[i].Lat, InfoboxContent[i].Long);
                bounds.extend(loc)
            }

            markerCluster = new MarkerClusterer(_map, _markerArray, { imagePath: 'https://developers.google.com/maps/documentation/javascript/examples/markerclusterer/m' });
            chkZommChange = false;
            if (chkZoom == false) {
                chkZommChange = true;
                lastzoom = _map.zoom;
                _map.fitBounds(bounds);
            }
            else {
                chkZommChange = false;
                lastzoom = 0;
                _map.fitBounds(bounds);
            }

        } else {
        }
    } catch (Error) {
    }
}

/***FUNCTION TO CREATE MARKER ON MAP ON SEARCH CLICK****/
function VehicleScoreDashboard(InfoboxContent, bOpenNewInfoBox, bClosePreviousInfoBox) {

    //debugger
    //// var img='http://chart.apis.google.com/chart?chst=d_map_pin_letter&chld='+A|FF0000|000000'

    _marker = new google.maps.Marker({
        map: _map,
        position: new google.maps.LatLng(InfoboxContent.Lat, InfoboxContent.Long)
    });

    ////_marker.id = InfoboxContent.length;

    _marker.setIcon('/App_Images/' + InfoboxContent.Icon);
    if (InfoboxContent.FK_CompanyId == '20') {
        var str = RotateIcon.makeIcon('/App_Images/' + InfoboxContent.Icon).setRotation({ deg: InfoboxContent.Heading }).getUrl();
        var a = endsWith(str, "AD12TscoAAAAAElFTkSuQmCC");
        if (endsWith(str, "AD12TscoAAAAAElFTkSuQmCC"))
            _marker.setIcon('/App_Images/' + InfoboxContent.Icon);
        else
            _marker.setIcon(RotateIcon.makeIcon('/App_Images/' + InfoboxContent.Icon).setRotation({ deg: InfoboxContent.Heading }).getUrl());
    }
    else {
        _marker.setIcon('/App_Images/' + InfoboxContent.Icon);
    }

    if (_infowindow && (bClosePreviousInfoBox == "1" || bClosePreviousInfoBox == "")) {
        _infowindow.close();
    }
    var livetrack = "display:none;";
    if (InfoboxContent.Status == 'P') {
        livetrack = "display:inline-block;"
    }

    _infowindow = new google.maps.InfoWindow();

    _content = "<div class='popup'><table class='info-table'><tr align='left'><td colspan='2'><b>"
    + InfoboxContent.RegistrationNo + "</b></td><td><b>"
    + InfoboxContent.TypeDimension + "</b></td></tr><tr nowrap><td width='90px' align='left'><b>Driver Name:</b></td><td align='left'>"
    + InfoboxContent.ChauffeurName + "</td></tr><tr><td  align='left'><b>Driver Mob:</b></td><td align='left'>"
    + InfoboxContent.ChauffeurMob + "</td> </tr><tr><td  align='left'><b>Supervisor Name :</b></td><td align='left'>"
    + InfoboxContent.SupervisorName + "</td> </tr><tr><td align='left'><b>Supervisor Mob. :</b></td><td align='left'>"
    + InfoboxContent.SupervisorMob + "</td> </tr><tr><td align='left'><b>Speed :</b></td><td align='left'>"
    + InfoboxContent.Speed + "Km / Hr </td> </tr><tr><td align='left'><b>Route :</b></td><td align='left'>"
    + InfoboxContent.RouteName + "</td> </tr></td></tr>"
    + '<tr><td align="left"></td></tr> </table></div>';
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

    _markerArray.push(_marker);


}

/*************************************************************************************************/


/*Function to load WithMovingDirection  for Tracking On Map*/
function DrawMarkerWithMovingDirectionForTrackingOnMap(MapControlID, Zoom, CenterLat, CenterLng, InfoboxContent) {

    try {
        // ;
        var iVar = 0;

        if (_markerArray.length > 0) {
            try {
                markerCluster.removeMarkers(_markerArray);
            } catch (e) {

            }

        }
        ClearMap();
        if (InfoboxContent.length != 0) {
            for (var i = 0; i <= InfoboxContent.length - 1; i++) {
                CreateMarkerWithInfoBoxWithDirectionForTrackingOnMap(InfoboxContent[i])
                loc = new google.maps.LatLng(InfoboxContent[i].Lat, InfoboxContent[i].Long);
                bounds.extend(loc)

            }
            markerCluster = new MarkerClusterer(_map, _markerArray, { imagePath: 'https://developers.google.com/maps/documentation/javascript/examples/markerclusterer/m' });
            //  _map.setCenter(new google.maps.LatLng(InfoboxContent[0].Lat, InfoboxContent[0].Long));
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
/*Create Marker With InfoBox With Direction  for Tracking On Map*/
function CreateMarkerWithInfoBoxWithDirectionForTrackingOnMap(InfoboxContent, bOpenNewInfoBox, bClosePreviousInfoBox) {



    //InfoboxContent.FK_CompanyId



    // var img='http://chart.apis.google.com/chart?chst=d_map_pin_letter&chld='+A|FF0000|000000'
    _marker = new google.maps.Marker({
        map: _map,
        //   icon: '../App_Images/' + InfoboxContent.Icon,

        position: new google.maps.LatLng(InfoboxContent.Lat, InfoboxContent.Long)
    });

    //  _marker.id = InfoboxContent.length;

    if (InfoboxContent.FK_CompanyId == '20') {
        var str = RotateIcon.makeIcon('/App_Images/' + InfoboxContent.Icon).setRotation({ deg: InfoboxContent.Heading }).getUrl();
        var a = endsWith(str, "AD12TscoAAAAAElFTkSuQmCC");
        if (endsWith(str, "AD12TscoAAAAAElFTkSuQmCC"))
            _marker.setIcon('/App_Images/' + InfoboxContent.Icon);
        else
            _marker.setIcon(RotateIcon.makeIcon('/App_Images/' + InfoboxContent.Icon).setRotation({ deg: InfoboxContent.Heading }).getUrl());
    }
    else {
        _marker.setIcon('/App_Images/' + InfoboxContent.Icon);
    }

    if (_infowindow && (bClosePreviousInfoBox == "1" || bClosePreviousInfoBox == "")) {
        _infowindow.close();
    }
    var livetrack = "display:none;";
    if (InfoboxContent.Status == 'P') {
        livetrack = "display:inline-block;"
    }

    _infowindow = new google.maps.InfoWindow();

    _content = "<div class='popup'><table class='info-table'><tr align='left'><td colspan='2'><b>"
    + InfoboxContent.RegistrationNo + "</b></td><td><b>"
    + InfoboxContent.ModelName + "</b></td></tr><tr nowrap><td width='90px' align='left'><b>Driver Name:</b></td><td align='left'>"
    + InfoboxContent.ChauffeurName + "</td></tr><tr><td  align='left'><b>Driver Mob:</b></td><td align='left'>"
    + InfoboxContent.ChauffeurMob + "</td> </tr><tr><td  align='left'><b>Recorded :</b></td><td align='left'>"
    + InfoboxContent.DeviceDateTime + "</td> </tr><tr><td align='left'><b>Speed :</b></td><td align='left'>"
    + InfoboxContent.Speed + " Km/Hr</td> </tr><tr><td align='left'><b>Device No. :</b></td><td align='left'>"
    + InfoboxContent.DeviceNo + "</td> </tr><tr><td align='left'><b>IMEI No. :</b></td><td align='left'>"
    + InfoboxContent.IMEINo + "</td> </tr><tr><td align='left'><b>Sim No. :</b></td><td align='left'>"
    + InfoboxContent.SIMNo
    + "</td></tr>"
    + '<tr><td align="left"></td><td align="left"><a style=' + livetrack + '  href="#" onclick="SendLiveTrackingforTrackingOnMap(\'' + InfoboxContent.RegistrationNo + '\'' + ',' + InfoboxContent.FK_CompanyId + ');">Live Tracking</a>'
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
    _markerArray.push(_marker);
}

function SendLiveTrackingforTrackingOnMap(RegistrationNo, FK_CompanyId) {

    $.ajax({
        url: '../TrackingOnMap/LiveTracking',
        //  url: "@Url.Action("GetMachineList", "Tracking")",
        type: "GET",
        dataType: "JSON",
        data: { CompId: FK_CompanyId, regNo: RegistrationNo },
        success: function (r) {

            var rst = r;
            if (rst == 1) {
                window.location.href = "../TrackingOnMap/Index";
            }
        }
    });
}

/*Create A Route With Infobox But Without Marker for Tracking On Map*/
function CreateRouteWithoutMarkerForTrackingOnMap(MapControlID, Zoom, CenterLat, CenterLng, InfoboxContent, PathColor, PathOpacity, PathWeight) {
    //alert(1)
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
                PathColor = '#65D873'
                if (i == 0 || i == InfoboxContent.length - 1) {
                    CreateMarkerWithInfoBoxforTrackingOnMap(InfoboxContent[i])
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
                    if (InfoboxContent[i].Icon == 'pin_yellow.png') {
                        PathColor = '#F3971E'
                    }
                    if (InfoboxContent[i].Icon == 'pin_green.png') {
                        PathColor = '#65D873'
                    }
                    if (InfoboxContent[i].Icon == 'pin_blue.png') {
                        PathColor = '#2191ED'
                    }
                    if (InfoboxContent[i].Icon == 'pin_red.png') {
                        PathColor = '#EA0000'
                    }

                    if (goto < 1) {
                        if (k > 0) {
                            var _vehTrackCoordinates = [new google.maps.LatLng(InfoboxContent[i].Lat,
                                                InfoboxContent[i].Long),
                                            new google.maps.LatLng(InfoboxContent[k].Lat,
                                                InfoboxContent[k].Long)
                            ];

                        } else if (InfoboxContent[i].ISGSM == 'OFF') {

                            var _vehTrackCoordinates = [new google.maps.LatLng(InfoboxContent[i].Lat,
                                                     InfoboxContent[i].Long),
                                                 new google.maps.LatLng(InfoboxContent[i - 1].Lat,
                                                     InfoboxContent[i - 1].Long)
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
                _markerArray[endlenth - 1].setAnimation(google.maps.Animation.BOUNCE);
                _map.setCenter(new google.maps.LatLng(InfoboxContent[InfoboxContent.length - 1].Lat, InfoboxContent[InfoboxContent.length - 1].Long));
            }

        } else {
            // alert("Marker list not pass !");
        }
    } catch (Error) {
        // alert("Problem in Create Route :" + Error.message);
    }
}

/*Create Marker With InfoBox*/
function CreateMarkerWithInfoBoxforTrackingOnMap(InfoboxContent, bOpenNewInfoBox, bClosePreviousInfoBox) {




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

    _content = "<div class='popup'><table class='info-table'><tr align='left'><td colspan='2'><b>"
    + InfoboxContent.RegistrationNo + "</b></td><td><b>"
    + InfoboxContent.ModelName + "</b></td></tr><tr nowrap><td width='90px' align='left'><b>Driver Name:</b></td><td align='left'>"
    + InfoboxContent.ChauffeurName + "</td></tr><tr><td  align='left'><b>Driver Mob:</b></td><td align='left'>"
    + InfoboxContent.ChauffeurMob + "</td> </tr><tr><td  align='left'><b>Recorded :</b></td><td align='left'>"
    + InfoboxContent.DeviceDateTime + "</td> </tr><tr><td align='left'><b>Speed :</b></td><td align='left'>"
      + InfoboxContent.Speed + " Km/Hr</td> </tr><tr><td align='left'><b>Device No. :</b></td><td align='left'>"
        + InfoboxContent.DeviceNo + "</td></tr>" //<tr><td align='left'><b>IMEI No. :</b></td><td align='left'>"
        //+ InfoboxContent.IMEINo + "</td> </tr><tr><td align='left'><b>Sim No. :</b></td><td align='left'>"
    //+ InfoboxContent.SIMNo
    //+ "</td></tr>"
    + '<tr><td align="left"></td><td align="left"><a style=' + livetrack + '  href="#" onclick="SendLiveTrackingforTrackingOnMap(\'' + InfoboxContent.RegistrationNo + '\'' + ',' + InfoboxContent.FK_CompanyId + ');">Live Tracking</a>'
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

    _markerArray.push(_marker);

}


/*Plays Route Without Markers for Tracking On Map STARTS*/
function PlayRouteWithoutMarkerForTrackingOnMap(MapControlID, Zoom, CenterLat, CenterLng, InfoboxContent, PathColor, PathOpacity, PathWeight, TimerIntervalMS, bAlwayOpenInfoBox, IsPanEnable) {
    //
    //alert('PlayRouteWithoutMarker')
    try {

        ClearMap();
        _counter = 0;
        _InfoboxContent = InfoboxContent;
        trHTML = '';
        _SetInterval = setInterval('PlayWithoutMarkerForTrackingOnMap(' + '"' + PathColor + '",' + PathOpacity + ',' + PathWeight + ',' + bAlwayOpenInfoBox + ',' + IsPanEnable + ')', TimerIntervalMS);
    } catch (Error) {
        // alert("Problem in Create Route :" + Error.message);
    }
}

function PlayWithoutMarkerForTrackingOnMap(PathColor, PathOpacity, PathWeight, bAlwayOpenInfoBox, IsPanEnable) {

    if (_counter <= _InfoboxContent.length - 1) {

        var prelat;
        var prelong;
        var preISGSM = 'OFF'
        var j = 0;
        var k = 0;
        var goto = 0;

        if (_counter == 0 || _counter == _InfoboxContent.length - 1) {
            CreateMarkerWithInfoBoxforTrackingOnMap(_InfoboxContent[_counter])
        }

        CreateTableHistoryBody(_InfoboxContent[_counter]);

        PathColor = '#65D873'

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
            if (_InfoboxContent[_counter].Icon == 'pin_yellow.png') {
                PathColor = '#F3971E'
            }
            if (_InfoboxContent[_counter].Icon == 'pin_green.png') {
                PathColor = '#65D873'
            }
            if (_InfoboxContent[_counter].Icon == 'pin_blue.png') {
                PathColor = '#2191ED'
            }
            if (_InfoboxContent[_counter].Icon == 'pin_red.png') {
                PathColor = '#EA0000'
            }

            if (goto < 1) {

                if (k > 0) {
                    var _vehTrackCoordinates = [new google.maps.LatLng(_InfoboxContent[_counter].Lat,
                                        _InfoboxContent[_counter].Long),
                                    new google.maps.LatLng(_InfoboxContent[k].Lat,
                                        _InfoboxContent[k].Long)
                    ];

                } else if (_InfoboxContent[_counter].ISGSM == 'OFF') {

                    var _vehTrackCoordinates = [new google.maps.LatLng(_InfoboxContent[_counter].Lat,
                                             _InfoboxContent[_counter].Long),
                                         new google.maps.LatLng(_InfoboxContent[_counter - 1].Lat,
                                             _InfoboxContent[_counter - 1].Long)
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
                    _map.panTo(new google.maps.LatLng(_InfoboxContent[_counter].Lat,
                                                   _InfoboxContent[_counter].Long));
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

/*Plays Route Without Markers for Tracking On Map END*/


/*DRAW MARKER AND LIVE TRACKING CALL FOR NEW DASHBOARD PAGE :: BY TARIQUE KHAN :: 22 JULY*/

/*Function to load WithMovingDirection*/
function DrawMarkerWithMovingDirectionForVehicleDahboardLOC(MapControlID, Zoom, CenterLat, CenterLng, InfoboxContent) {
    //debugger
    try {
        // ;
        var iVar = 0;

        if (_markerArray.length > 0) {
            markerCluster.removeMarkers(_markerArray);
        }
        ClearMap();
        if (InfoboxContent.length != 0) {
            for (var i = 0; i <= InfoboxContent.length - 1; i++) {
                CreateMarkerWithInfoBoxWithDirectionForVehicleDahboardLOC(InfoboxContent[i])
                loc = new google.maps.LatLng(InfoboxContent[i].Lat, InfoboxContent[i].Long);
                bounds.extend(loc)

            }
            _map.fitBounds(bounds);
            _map.panToBounds(bounds);
            markerCluster = new MarkerClusterer(_map, _markerArray, { imagePath: 'https://developers.google.com/maps/documentation/javascript/examples/markerclusterer/m' });
            chkZommChange = false;
            if (chkZoom == false) {
                chkZommChange = true;
                lastzoom = _map.zoom;

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

/*Create Marker With InfoBox With Direction*/
function CreateMarkerWithInfoBoxWithDirectionForVehicleDahboardLOC(InfoboxContent, bOpenNewInfoBox, bClosePreviousInfoBox) {

    _marker = new google.maps.Marker({
        map: _map,
        position: new google.maps.LatLng(InfoboxContent.Lat, InfoboxContent.Long)
    });

    if (InfoboxContent.FK_CompanyId == '20') {
        var str = RotateIcon.makeIcon('/App_Images/' + InfoboxContent.Icon).setRotation({ deg: InfoboxContent.Heading }).getUrl();
        var a = endsWith(str, "AD12TscoAAAAAElFTkSuQmCC");
        if (endsWith(str, "AD12TscoAAAAAElFTkSuQmCC"))
            _marker.setIcon('/App_Images/' + InfoboxContent.Icon);
        else
            _marker.setIcon(RotateIcon.makeIcon('/App_Images/' + InfoboxContent.Icon).setRotation({ deg: InfoboxContent.Heading }).getUrl());
    }

    else {
        _marker.setIcon('/App_Images/' + InfoboxContent.Icon);
    }

    if (_infowindow && (bClosePreviousInfoBox == "1" || bClosePreviousInfoBox == "")) {
        _infowindow.close();
    }

    var livetrack = "display:none;";

    if (InfoboxContent.Status == 'P') {
        livetrack = "display:inline-block;"
    }

    _infowindow = new google.maps.InfoWindow();

    _content = "<div class='popup'><table class='info-table'><tr align='left'><td colspan='2'><b>"
    + InfoboxContent.RegistrationNo + "</b></td><td><b>"
    + InfoboxContent.ModelName + "</b></td></tr><tr nowrap><td width='90px' align='left'><b>Driver Name:</b></td><td align='left'>"
    + InfoboxContent.ChauffeurName + "</td></tr><tr><td  align='left'><b>Driver Mob:</b></td><td align='left'>"
    + InfoboxContent.ChauffeurMob + "</td> </tr><tr><td  align='left'><b>Recorded :</b></td><td align='left'>"
    + InfoboxContent.DeviceDateTime + "</td> </tr><tr><td align='left'><b>Speed :</b></td><td align='left'>"
    + InfoboxContent.Speed + " Km/Hr</td> </tr><tr><td align='left'><b>Device No. :</b></td><td align='left'>"
    + InfoboxContent.DeviceNo + "</td> </tr><tr><td align='left'><b>IMEI No. :</b></td><td align='left'>"
    + InfoboxContent.IMEINo + "</td> </tr><tr><td align='left'><b>Sim No. :</b></td><td align='left'>"
    + InfoboxContent.SIMNo
    + "</td></tr>"
    + '<tr><td align="left"></td><td align="left"><a style=' + livetrack + '  href="#" onclick="SendLiveTrackingForVehicleDahboardLOC(\'' + InfoboxContent.RegistrationNo + '\'' + ',' + InfoboxContent.FK_CompanyId + ');">Live Tracking</a>'
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

function SendLiveTrackingForVehicleDahboardLOC(RegistrationNo, FK_CompanyId) {

    $.ajax({
        url: '../TrackingOnMap/LiveTracking',
        type: "GET",
        dataType: "JSON",
        data: { CompId: FK_CompanyId, regNo: RegistrationNo },
        success: function (r) {

            var rst = r;
            if (rst == 1) {
                window.location.href = "../TrackingOnMap/Index";
            }
        }
    });
}

/*DRAW MARKER AND LIVE TRACKING CALL FOR NEW DASHBOARD PAGE :: BY TARIQUE KHAN :: 22 JULY*/

/*Function to load Employee Marker With MovingDirection For FSTS  BY: Shivam Saluja On 10 sept 2019*/
function DrawEmployeeMarkerWithMovingDirection(MapControlID, Zoom, CenterLat, CenterLng, InfoboxContent) {
    // debugger
    try {
        // ;
        var iVar = 0;

        if (_markerArray.length > 0) {
            markerCluster.removeMarkers(_markerArray);
        }
        ClearMap();
        if (InfoboxContent.length != 0) {
            for (var i = 0; i <= InfoboxContent.length - 1; i++) {
                CreateEmployeeMarkerWithInfoBoxWithDirection(InfoboxContent[i])
                loc = new google.maps.LatLng(InfoboxContent[i].Lat, InfoboxContent[i].Long);
                bounds.extend(loc)

            }
            _map.fitBounds(bounds);
            _map.panToBounds(bounds);
            markerCluster = new MarkerClusterer(_map, _markerArray, { imagePath: 'https://developers.google.com/maps/documentation/javascript/examples/markerclusterer/m' });
            //  _map.setCenter(new google.maps.LatLng(InfoboxContent[0].Lat, InfoboxContent[0].Long));
            chkZommChange = false;
            //   if (IsfitBounds != false) {
            if (chkZoom == false) {
                chkZommChange = true;
                lastzoom = _map.zoom;

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
/*Create Employee Marker With InfoBox With Direction For FSTS  BY: Shivam Saluja On 10 sept 2019*/
function CreateEmployeeMarkerWithInfoBoxWithDirection(InfoboxContent, bOpenNewInfoBox, bClosePreviousInfoBox) {

    // debugger

    //InfoboxContent.FK_CompanyId



    // var img='http://chart.apis.google.com/chart?chst=d_map_pin_letter&chld='+A|FF0000|000000'
    _marker = new google.maps.Marker({
        map: _map,
        //   icon: '../App_Images/' + InfoboxContent.Icon,

        position: new google.maps.LatLng(InfoboxContent.Lat, InfoboxContent.Long)
    });

    //  _marker.id = InfoboxContent.length;

    if (InfoboxContent.FK_CompanyId == '20') {
        var str = RotateIcon.makeIcon('/App_Images/' + InfoboxContent.Icon).setRotation({ deg: InfoboxContent.Heading }).getUrl();
        var a = endsWith(str, "AD12TscoAAAAAElFTkSuQmCC");
        if (endsWith(str, "AD12TscoAAAAAElFTkSuQmCC"))
            _marker.setIcon('/App_Images/' + InfoboxContent.Icon);
        else
            _marker.setIcon(RotateIcon.makeIcon('/App_Images/' + InfoboxContent.Icon).setRotation({ deg: InfoboxContent.Heading }).getUrl());
    }
    else {
        _marker.setIcon('/App_Images/' + InfoboxContent.Icon);
    }

    if (_infowindow && (bClosePreviousInfoBox == "1" || bClosePreviousInfoBox == "")) {
        _infowindow.close();
    }
    var livetrack = "display:none;";
    if (InfoboxContent.Status == 'P') {
        livetrack = "display:inline-block;"
    }

    _infowindow = new google.maps.InfoWindow();
    if (InfoboxContent.RoleNames == 'FSE') {
        _content = "<div class='popup'><table class='info-table'><tr align='left'><td colspan='2' style=''><b>"
       + InfoboxContent.EmpName + "</b></td></tr><tr nowrap><td width='90px' align='left'><b>Employee ID:</b></td><td align='left'>"
       + InfoboxContent.FK_EmployeeId + "</td></tr><tr nowrap><td width='90px' align='left'><b>POD Name:</b></td><td align='left'>"
       + InfoboxContent.PODName + "</td></tr><tr><td  align='left'><b>Team Name:</b></td><td align='left'>"
       + InfoboxContent.TeamName + "</td> </tr><tr><td  align='left'><b>Mobile No. :</b></td><td align='left'>"
       + InfoboxContent.Mobile_No + "</td> </tr><tr><td align='left'><b>Battery :</b></td><td align='left'>"
       + InfoboxContent.BatteryLevel + " %</td> </tr><tr nowrap><td  width='90px' align='left'><b>Geofence Name :</b></td><td align='left'>"
       + InfoboxContent.GeofenceName + "</td> </tr><tr><td align='left'><b>Onboarding :</b></td><td align='left'>"
       + InfoboxContent.Onboardingcount + "/" + InfoboxContent.OnboardingTarget + "</td> </tr><tr><td align='left'><b>Total Visit :</b></td><td align='left'>"
       + InfoboxContent.Visitcount + "/" + InfoboxContent.VisitTarget + "</td> </tr><tr><td align='left'><b>Last Updated Time :</b></td><td align='left'>"
       + InfoboxContent.DeviceDateTime
       + "</td></tr>"
       + '<tr><td align="left"></td><td align="left"><a style=' + livetrack + '  href="#" onclick="SendLiveTracking(\'' + InfoboxContent.RegistrationNo + '\'' + ',' + InfoboxContent.FK_CompanyId + ');">Live Tracking</a>'
       + "</td></tr> </table></div>";
    } else {
        _content = "<div class='popup'><table class='info-table'><tr align='left'><td colspan='2' style=''><b>"
   + InfoboxContent.EmpName + "</b></td></tr><tr nowrap><td width='90px' align='left'><b>Employee ID:</b></td><td align='left'>"
   + InfoboxContent.FK_EmployeeId + "</td></tr><tr><td  align='left'><b>Mobile No. :</b></td><td align='left'>"
   + InfoboxContent.Mobile_No + "</td> </tr><tr><td align='left'><b>Battery :</b></td><td align='left'>"
   + InfoboxContent.BatteryLevel + " %</td> </tr><tr nowrap><td  width='90px' align='left'><b>Geofence Name :</b></td><td align='left'>"
   + InfoboxContent.GeofenceName + "</td> </tr><tr><td align='left'><b>Onboarding :</b></td><td align='left'>"
   + InfoboxContent.Onboardingcount + "/" + InfoboxContent.OnboardingTarget + "</td> </tr><tr><td align='left'><b>Total Visit :</b></td><td align='left'>"
   + InfoboxContent.Visitcount + "/" + InfoboxContent.VisitTarget + "</td> </tr><tr><td align='left'><b>Last Updated Time :</b></td><td align='left'>"
   + InfoboxContent.DeviceDateTime
   + "</td></tr>"
   + '<tr><td align="left"></td><td align="left"><a style=' + livetrack + '  href="#" onclick="SendLiveTracking(\'' + InfoboxContent.RegistrationNo + '\'' + ',' + InfoboxContent.FK_CompanyId + ');">Live Tracking</a>'
   + "</td></tr> </table></div>";
    }

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
    _markerArray.push(_marker);
}

/*Function to load With MovingDirection For Geofence Dashboard*/
function DrawMarkerWithMovingDirectionForGeofenceDashboard(MapControlID, Zoom, CenterLat, CenterLng, InfoboxContent) {
    //debugger
    try {
        //// debugger;
        var iVar = 0;

        ClearMap();

        if (_markerArray.length > 0) {
            markerCluster.removeMarkers(_markerArray);
        }

        if (InfoboxContent.length != 0) {
            for (var i = 0; i <= InfoboxContent.length - 1; i++) {
                //debugger
                CreateMarkerWithInfoBoxWithDirectionForGeofenceDashboard(InfoboxContent[i])
                loc = new google.maps.LatLng(InfoboxContent[i].Lat, InfoboxContent[i].Long);
                bounds.extend(loc);
            }

            //debugger

            markerCluster = new MarkerClusterer(_map, _markerArray, { imagePath: 'https://developers.google.com/maps/documentation/javascript/examples/markerclusterer/m' });
            //  _map.setCenter(new google.maps.LatLng(InfoboxContent[0].Lat, InfoboxContent[0].Long));
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

        }
    }
    catch (Error) {
        // alert("Problem in Draw Marker :" + Error.message);
    }
}

/*Create Marker With InfoBox With Direction  For Geofence Dashboard*/
function CreateMarkerWithInfoBoxWithDirectionForGeofenceDashboard(InfoboxContent, bOpenNewInfoBox, bClosePreviousInfoBox) {
    //debugger
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
    });

    if (InfoboxContent.FK_CompanyId == '20') {
        var str = RotateIcon.makeIcon('/App_Images/' + InfoboxContent.Icon).setRotation({ deg: InfoboxContent.Heading }).getUrl();
        var a = endsWith(str, "AD12TscoAAAAAElFTkSuQmCC");
        if (endsWith(str, "AD12TscoAAAAAElFTkSuQmCC"))
            _marker.setIcon('/App_Images/' + InfoboxContent.Icon);
        else
            _marker.setIcon(RotateIcon.makeIcon('/App_Images/' + InfoboxContent.Icon).setRotation({ deg: InfoboxContent.Heading }).getUrl());
    }
    else {
        _marker.setIcon('/App_Images/' + InfoboxContent.Icon);
    }

    if (_infowindow && (bClosePreviousInfoBox == "1" || bClosePreviousInfoBox == "")) {
        _infowindow.close();
    }
    var livetrack = "display:none;";
    if (InfoboxContent.Status == 'P') {
        livetrack = "display:inline-block;"
    }

    _infowindow = new google.maps.InfoWindow();

    _content = "<div class='popup'><table class='info-table'><tr align='left'><td colspan='2'><b>"
               + InfoboxContent.RegistrationNo + "</b></td><td><b>"
               + InfoboxContent.ModelName + "</b></td></tr><tr nowrap><td width='90px' align='left'><b>Driver Name:</b></td><td align='left'>"
               + InfoboxContent.ChauffeurName + "</td></tr><tr><td  align='left'><b>Driver Mob:</b></td><td align='left'>"
               + InfoboxContent.ChauffeurMob + "</td> </tr><tr><td  align='left'><b>Recorded :</b></td><td align='left'>"
               + InfoboxContent.DeviceDateTime + "</td> </tr><tr><td align='left'><b>Speed :</b></td><td align='left'>"
               + InfoboxContent.Speed + " Km/Hr</td> </tr><tr><td align='left'><b>Device No. :</b></td><td align='left'>"
               + InfoboxContent.DeviceNo + "</td> </tr><tr><td align='left'><b>IMEI No. :</b></td><td align='left'>"
               + InfoboxContent.IMEINo + "</td> </tr><tr><td align='left'><b>Sim No. :</b></td><td align='left'>"
               + InfoboxContent.SIMNo
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
    _markerArray.push(_marker);
}





/*Calls Live Tracking Function For SBTMS Trip Dashboard*/
function SendSBTMSTripLiveTracking(RegistrationNo, tripno) {
    // debugger

    //alert('SendInboundTripLiveTracking');
    $.ajax({
        url: '../SBTMS/SBTMSTripDashboard/SBTMSripLiveTrackingData',
        //  url: "@Url.Action("GetMachineList", "Tracking")",
        type: "GET",
        dataType: "JSON",
        data: { regNo: RegistrationNo, tripNo: tripno },
        success: function (r) {

            var rst = r;
            if (rst == 1) {
                window.location.href = "../SBTMS/SBTMSTripDashboard/SBTMSTripLiveTracking";
            }
        }
    });
}

/*Create Trip Marker With InfoBox For SBTMS Trip Dashboard*/
function CreateSBTMSTripMarkerWithInfoBoxforLiveTracking(InfoboxContent, bOpenNewInfoBox, bClosePreviousInfoBox, id) {
    //debugger
    //alert('CreateInboundTripMarkerWithInfoBoxforLiveTracking')
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

    if (_infowindow && (bClosePreviousInfoBox == "1" || bClosePreviousInfoBox == "")) {
        _infowindow.close();
    }
    var livetrack = "display:none;";
    if (InfoboxContent.Status == 'P') {
        livetrack = "display:inline-block;"
    }
    _infowindow = new google.maps.InfoWindow();

    _content = "<div class='popup'><table class='info-table'><tr align='left'><td colspan='2'><b>"
           + InfoboxContent.RegistrationNo + "</b></td><td><b>"
           + InfoboxContent.ModelName + "</b></td></tr><tr nowrap><td width='90px' align='left'><b>Driver Name:</b></td><td align='left'>"
           + InfoboxContent.ChauffeurName + "</td></tr><tr><td  align='left'><b>Driver Mob:</b></td><td align='left'>"
           + InfoboxContent.ChauffeurMob + "</td> </tr><tr><td  align='left'><b>Recorded :</b></td><td align='left'>"
           + InfoboxContent.DeviceDateTime + "</td> </tr><tr><td align='left'><b>Speed :</b></td><td align='left'>"
           + InfoboxContent.Speed + " Km/Hr</td> </tr><tr><td align='left'><b>Device No. :</b></td><td align='left'>"
           + InfoboxContent.DeviceNo + "</td> </tr><tr><td align='left'><b>IMEI No. :</b></td><td align='left'>"
           + InfoboxContent.IMEINo + "</td> </tr><tr><td align='left'><b>Sim No. :</b></td><td align='left'>"
           + InfoboxContent.SIMNo
           + "</td></tr>"
           + "</table></div>"



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
function ViewPlacesOnMap(lat, long) {

    //  debugger

    // //  var url = "https://www.google.com/maps/search/?api=1&query="  +lat + ',' + long;
    //   var url = 'https://www.google.com/maps/search/police+stations/@' + lat + ',' + long 
    ////   var url = 'https://www.google.com/maps/search/police+stations+near+me/@' + lat + ',' + long + ',13z/data=!3m1!4b1'

    //  window.open(url, '_blank', 'location=no,scrollbars=yes,status=yes');
    //   window.open(url, '_blank');

    var radius = 5000;
    var type = 'police';
    var cur_location = new google.maps.LatLng(lat, long);

    var request = {
        location: cur_location,
        radius: radius,
        types: [type]

    };
    for (var i = 0; i < _markerArray.length; i++) {
        if (_markerArray[i].title) {
            _markerArray[i].setMap(null);
        }
    }
    service = new google.maps.places.PlacesService(_map);
    service.search(request, createMarkers);
}
var nearByQuery = "";
function ViewPlacesOnMap_MyNearby(lat, long, GoogleApiNearByType, GoogleApiNearByName) {

    //  debugger

    // //  var url = "https://www.google.com/maps/search/?api=1&query="  +lat + ',' + long;
    //   var url = 'https://www.google.com/maps/search/police+stations/@' + lat + ',' + long 
    ////   var url = 'https://www.google.com/maps/search/police+stations+near+me/@' + lat + ',' + long + ',13z/data=!3m1!4b1'

    //  window.open(url, '_blank', 'location=no,scrollbars=yes,status=yes');
    //   window.open(url, '_blank');

    var radius = 5000;
    var type = GoogleApiNearByType.split(',');
    var searchName = GoogleApiNearByName.split(',');
    var cur_location = new google.maps.LatLng(lat, long);
    for (var i = 0; i < _markerArray.length; i++) {
        if (_markerArray[i].title) {
            _markerArray[i].setMap(null);
        }
    }

    for (var i = 0; i < searchName.length; i++) {
        nearByQuery = $.trim(searchName[i]);
        var request = {
            location: cur_location,
            radius: radius,
            types: type,
            query: nearByQuery

        };

        service = new google.maps.places.PlacesService(_map);
        service.textSearch(request, createMarkersWithValidation);

    }


}


function createMarkersWithValidation(results, status) {
    //debugger
    if (status == google.maps.places.PlacesServiceStatus.OK) {
        // and create new markers by search result
        for (var i = 0; i < results.length; i++) {

            createMarkerValid(results[i]);

        }
    } else if (status == google.maps.places.PlacesServiceStatus.ZERO_RESULTS) {
        alert('Sorry, nothing is found');
    }

}



function createMarkerValid(obj) {
    //debugger;
    var bool = false;
    console.log("nearByQuery : " + nearByQuery);
    console.log("obj.name : " + obj.name);
    var searchName = (obj.name).toLocaleLowerCase();
    nearByQuery = nearByQuery.toLocaleLowerCase();
    if (searchName.indexOf(nearByQuery) >= 0) {
        bool = true;
    }

    if (bool) {
        var mark = new google.maps.Marker({
            position: obj.geometry.location,
            map: _map,
            title: obj.name + '\n' + obj.formatted_address
        });

        _markerArray.push(mark);
    }

}

// create markers (from 'findPlaces' function)

function createMarkers(results, status) {

    if (status == google.maps.places.PlacesServiceStatus.OK) {
        // and create new markers by search result
        for (var i = 0; i < results.length; i++) {
            createMarker(results[i]);
        }
    } else if (status == google.maps.places.PlacesServiceStatus.ZERO_RESULTS) {
        alert('Sorry, nothing is found');
    }

}

// creare single marker function

function createMarker(obj) {
    //  debugger
    // prepare new Marker object
    var mark = new google.maps.Marker({
        position: obj.geometry.location,
        map: _map,
        title: obj.name + '\n' + obj.formatted_address
    });

    _markerArray.push(mark);


    // add event handler to current marker

    //google.maps.event.addListener(mark, 'click', function () {

    //    clearInfos();

    //    infowindow.open(map, mark);

    //});

    //infos.push(infowindow);

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

/*Code By Tarique Starts :: Being Used to play Completed Trip Routes*/
/*Sets time interval & calls 'PlayActualRouteSteps' to draw actual route step by step*/
function ActualRoutePlayForATMS(MapControlID, CenterLat, CenterLng, InfoboxContent, PathColor, PathOpacity, PathWeight, TimerIntervalMS, bAlwayOpenInfoBox, IsPanEnable) {

    try {
        Pause();
        ClearMap();
        _counter = 0;
        _InfoboxContent = InfoboxContent;
        trHTML = '';
        _SetInterval = setInterval('PlayActualRouteSteps(' + '"' + PathColor + '",' + PathOpacity + ',' + PathWeight + ',' + bAlwayOpenInfoBox + ',' + IsPanEnable + ')', TimerIntervalMS);
    } catch (Error) {
        // alert("Problem in Create Route :" + Error.message);
    }
}
function ActualRoutePlayForATMSDynamic(MapControlID, CenterLat, CenterLng, InfoboxContent, PathColor, PathOpacity, PathWeight, TimerIntervalMS, bAlwayOpenInfoBox, IsPanEnable, regNo, ATMSImage) {

    try {
        Pause();
        ClearMap();
        _counter = 0;
        _InfoboxContent = InfoboxContent;
        _regNo = regNo;
        _ATMSIMage = ATMSImage;
        trHTML = '';
        _SetInterval = setInterval('PlayActualRouteStepsDynamic(' + '"' + PathColor + '",' + PathOpacity + ',' + PathWeight + ',' + bAlwayOpenInfoBox + ',' + IsPanEnable + ')', TimerIntervalMS);
    } catch (Error) {
        // alert("Problem in Create Route :" + Error.message);
    }
}

function CreateRouteDynamic(MapControlID, Zoom, CenterLat, CenterLng, InfoboxContent, PathColor, PathOpacity, PathWeight, regNo, ATMSImage) {
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



                CreateMarkerWithInfoBoxDynamic(InfoboxContent[i], regNo, ATMSImage)

                if (i > 0) {

                            PathColor = '#0B3B0B';
                            preISGSM = 'OFF';
                            goto = 0;

                    if (goto < 1) {

                            var _vehTrackCoordinates = [new google.maps.LatLng(InfoboxContent[i].latitude,
                                                     InfoboxContent[i].longitude),
                                                 new google.maps.LatLng(InfoboxContent[i - 1].latitude,
                                                     InfoboxContent[i - 1].longitude)
                            ];

                        _RoutePath = new google.maps.Polyline({
                            path: _vehTrackCoordinates,
                            strokeColor: PathColor,//"#CC3300",
                            strokeOpacity: PathOpacity,//80,
                            strokeWeight: PathWeight,//2,
                            icons: [{

                                icon: {
                                    path: google.maps.SymbolPath.BACKWARD_CLOSED_ARROW,
                                    //scale: 6,
                                    //rotation: heading,
                                    strokeColor: PathColor,
                                    fillColor: PathColor,
                                    fillOpacity: 1
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
                _markerArray[endlenth - 1].setAnimation(google.maps.Animation.BOUNCE);
                _map.setCenter(new google.maps.LatLng(InfoboxContent[InfoboxContent.length - 1].latitude, InfoboxContent[InfoboxContent.length - 1].longitude));
            }

        } else {
            // alert("Marker list not pass !");
        }
    } catch (Error) {
        // alert("Problem in Create Route :" + Error.message);
    }

}

/*Function to load Merchant Marker With MovingDirection For FSTS  BY: Prince Kumar Srivastava On 28-12-2020*/
function DrawMerchantMarkerWithMovingDirection(MapControlID, Zoom, CenterLat, CenterLng, InfoboxContent) {
    /////  debugger
    try {
        var iVar = 0;
        if (_markerArray.length > 0) {
            markerCluster.removeMarkers(_markerArray);
        }
        if (InfoboxContent.length != 0) {
            for (var i = 0; i <= InfoboxContent.length - 1; i++) {
                CreateMerchantMarkerWithInfoBoxWithDirection(InfoboxContent[i])
                loc = new google.maps.LatLng(InfoboxContent[i].Addr_Lat, InfoboxContent[i].Addr_Long);
                bounds.extend(loc);

            }
            map.fitBounds(bounds);
            map.panToBounds(bounds);
            markerCluster = new MarkerClusterer(map, _markerArray, { imagePath: 'https://developers.google.com/maps/documentation/javascript/examples/markerclusterer/m' });
            //  _map.setCenter(new google.maps.LatLng(InfoboxContent[0].Lat, InfoboxContent[0].Long));
            chkZommChange = false;

            //   if (IsfitBounds != false) {
            if (chkZoom == false) {
                chkZommChange = true;
                lastzoom = map.zoom;

            }
            else {
                chkZommChange = false;
                lastzoom = 0;
            }
        }

    } catch (Error) {
        // alert("Problem in Draw Marker :" + Error.message);
    }
}

/*Create Merchant With InfoBox With Direction For FSTS  BY: Prince Kumar Srivastava On 28-12-2020*/
function CreateMerchantMarkerWithInfoBoxWithDirection(InfoboxContent, bOpenNewInfoBox, bClosePreviousInfoBox) {
    // var img='http://chart.apis.google.com/chart?chst=d_map_pin_letter&chld='+A|FF0000|000000'
    ///// debugger;
    _marker = new google.maps.Marker({
        map: map,
        //   icon: '../App_Images/' + InfoboxContent.Icon,

        position: new google.maps.LatLng(InfoboxContent.Addr_Lat, InfoboxContent.Addr_Long)
    });

    //  _marker.id = InfoboxContent.length;

    if (InfoboxContent.FK_CompanyId == '20') {
        var str = RotateIcon.makeIcon('/App_Images/' + InfoboxContent.Icon).setRotation({ deg: InfoboxContent.Heading }).getUrl();
        var a = endsWith(str, "AD12TscoAAAAAElFTkSuQmCC");
        if (endsWith(str, "AD12TscoAAAAAElFTkSuQmCC"))
            _marker.setIcon('/App_Images/' + InfoboxContent.Icon);
        else
            _marker.setIcon(RotateIcon.makeIcon('/App_Images/' + InfoboxContent.Icon).setRotation({ deg: InfoboxContent.Heading }).getUrl());
    }
    else {
        _marker.setIcon('/App_Images/' + InfoboxContent.Icon);
    }

    if (_infowindow && (bClosePreviousInfoBox == "1" || bClosePreviousInfoBox == "")) {
        _infowindow.close();
    }
    var livetrack = "display:none;";
    //if (InfoboxContent.Status == 'P') {
    //    livetrack = "display:inline-block;"
    //}

    _infowindow = new google.maps.InfoWindow();
    _content = "<div class='popup'><table class='info-table'><tr align='left'>"
    + "<td width='90px' align='left'><b>Merchant ID:</b></td><td align='left'>"
    + InfoboxContent.PK_MerchantID + "</td></tr><tr nowrap><td width='90px' align='left'><b>Merchant Name:</b></td><td align='left'>"
    + InfoboxContent.MerchantName + "</td></tr><tr><td  align='left'><b>Area Name:</b></td><td align='left'>"
    + InfoboxContent.AreaName + "</td> </tr><tr><td  align='left'><b>City Name:</b></td><td align='left'>"
    + InfoboxContent.CityName + "</td> </tr><tr><td align='left'><b>Mobile No. :</b></td><td align='left'>"
    + InfoboxContent.OwnerMobileNo
    + "</td></tr> </table></div>";

    google.maps.event.addListener(_marker, 'click', (function (_marker, _content) {
        return function () {
            _infowindow.setContent(_content);
            _infowindow.open(map, _marker);
        }
    })(_marker, _content));

    if (bOpenNewInfoBox == "1") {
        _infowindow.setContent(_content);
        _infowindow.open(map, _marker);
    }
    _markerArray.push(_marker);
}
