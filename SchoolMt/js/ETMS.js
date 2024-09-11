/*
   Created By  :Deepak Singh
   Created Date: 2017-09-08
   Purpose     : Google Map For Employee transport management system
*/

/* Global Variable */
/* Global Variable */
var _mapOptions;
var _map;
var _marker;
var _markerArray = [];
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

/*Function to load blank map*/
function initMap(MapControlID, Zoom, CenterLat, CenterLng) {
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
function DrawMarker(MapControlID, Zoom, CenterLat, CenterLng, InfoboxContent, IsNewRoute) {
    //debugger;
    try {

        var iVar = 0;
        ClearMap();

        if (InfoboxContent.length != 0) {
            for (var i = 0; i <= InfoboxContent.length - 1; i++) {
                CreateMarkerWithInfoBox(InfoboxContent[i], "1", "1", IsNewRoute)
                loc = new google.maps.LatLng(InfoboxContent[i].Address_Lat, InfoboxContent[i].Address_Long);
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

function ClearMap() {
    removeLine();
    setMapOnAll(null);
}

// Sets the map on all markers in the array.
function setMapOnAll(map) {
    if (typeof _markerArray === 'undefined') { } else {
        for (var i = 0; i < _markerArray.length; i++) {
            _markerArray[i].setMap(map);
        }

        _markerArray = [];
    }
}

function removeLine() {

    if (typeof _vehPath === 'undefined') { } else {
        _vehPath.setMap(null);
    }
}

/*Create Marker With InfoBox*/
function CreateMarkerWithInfoBox(InfoboxContent, bOpenNewInfoBox, bClosePreviousInfoBox, IsNewRoute) {
    //debugger;
    //InfoboxContent.FK_CompanyId
    // var img='http://chart.apis.google.com/chart?chst=d_map_pin_letter&chld='+A|FF0000|000000'
    alert('/App_Images/' + InfoboxContent.Icon)
    _marker = new google.maps.Marker({
        map: _map,
        icon: '/App_Images/' + InfoboxContent.Icon,
        //label: InfoboxContent.Employee_Name,
        position: new google.maps.LatLng(InfoboxContent.Address_Lat, InfoboxContent.Address_Long)
    });
    if (_infowindow && (bClosePreviousInfoBox == "1" || bClosePreviousInfoBox == "")) {
        _infowindow.close();
    }
    //debugger;
    var livetrack = "display:none;";

    if (InfoboxContent.Icon.indexOf('green') != '-1' && IsNewRoute) {
        livetrack = "display:inline-block;"
    }

    if (!IsNewRoute) {
        livetrack = "display:inline-block;"
    }

    _infowindow = new google.maps.InfoWindow();
    debugger;
    _content = "<div class='popup'><table class='info-table'><tr align='left'><td colspan='2'><b>"
    + InfoboxContent.Employee_Code + "</b></td><td><b>"
    + InfoboxContent.Gender + "</b></td></tr><tr nowrap><td width='90px' align='left'><b>Employee Name:</b></td><td align='left'>"
    + InfoboxContent.Employee_Name + "</td></tr><tr><td  align='left'><b>Mob.No.:</b></td><td align='left'>"
    + InfoboxContent.Mobile_No + "</td> </tr><tr><td  align='left'><b>Area Name :</b></td><td align='left'>"
    + InfoboxContent.Area_Name + "</td> </tr><tr><td align='left'><b>Landmark :</b></td><td align='left'>"
      + InfoboxContent.Landmark + " </td> </tr><tr><td align='left'><b>Address :</b></td><td align='left'>"
        + InfoboxContent.Address + "</td> </tr><tr><td align='left'><b>Shift :</b></td><td align='left'>"
        + InfoboxContent.Shift_Name +
    "</td></tr>"
    //+ '<tr><td align="left"></td><td align="left"><a style=' + livetrack + ' id=' + InfoboxContent.Employee_Code + '   href="#" onclick="Addmore(\'' + InfoboxContent.Employee_Code + '\'' + ',\'' + '' + InfoboxContent.Employee_Name + '\'' + ',' + InfoboxContent.Address_Lat + ',' + InfoboxContent.Address_Long + ',' + InfoboxContent.FK_Geofence_ID + ',' + false + ');">Add Employee in Route</a>'
    + '<tr><td align="left"></td><td align="left"><a style=' + livetrack + ' id=' + InfoboxContent.Employee_Code + '   href="#" onclick="Addmore(\'' + InfoboxContent.Employee_Code + '\'' + ',\'' + '' + InfoboxContent.Employee_Name + '\'' + ',' + InfoboxContent.Address_Lat + ',' + InfoboxContent.Address_Long + ',' + InfoboxContent.PK_Employee_ID + ',' + false + ');">Add Employee in Route</a>'
    + "</td></tr> </table></div>";
    //+ "<tr><td align='left'></td><td align='left'><a href='#' onclick='SendLiveTracking(''+RegNo,CompId)'>Live Tracking</a>"
    // (EmpCode,EmpName,EmpLat,Emplon)style=' + livetrack + '
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
    _infowindow.close();
}

/*Function to load Moving Markers*/
function DrawMarkerWithMovingDirection(MapControlID, Zoom, CenterLat, CenterLng, InfoboxContent) {
    try {
        var iVar = 0;
        ClearMap();

        if (InfoboxContent.length != 0) {
            for (var i = 0; i <= InfoboxContent.length - 1; i++) {
                CreateMarkerWithInfoBoxWithDirection(InfoboxContent[i])
                loc = new google.maps.LatLng(InfoboxContent[i].Lat, InfoboxContent[i].Long);
                bounds.extend(loc)
            }

            chkZommChange = false;

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

        }
    } catch (Error) {

    }
}

/*Creates Marker With InfoBox With Direction*/
function CreateMarkerWithInfoBoxWithDirection(InfoboxContent, bOpenNewInfoBox, bClosePreviousInfoBox) {
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
        //if (true) {
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
        + InfoboxContent.IMEINo + "</td> </tr><tr><td align='left'><b>Sim No. :</b></td><td align='left'>"                                                              //
    + InfoboxContent.SIMNo
    + "</td></tr>"
    //+ '<tr><td align="left"></td><td align="left"><a style=' + livetrack + '  href="javascript:void(0)" onclick="ETMSLiveTracking(\'' + InfoboxContent.RegistrationNo + '\'' + ',\'' + '' + InfoboxContent.FK_CompanyId + '\'' + ',' + InfoboxContent.Trip_No + ',' + InfoboxContent.TripType + ');">Live Tracking</a>'
    + '<tr><td align="left"></td><td align="left"> <a style=' + livetrack + '  href="javascript:void(0)"  onclick="ETMSLiveTracking(\'' + InfoboxContent.RegistrationNo + '\'' + ',\'' + '' + InfoboxContent.FK_CompanyId + '\'' + ',\'' + '' + InfoboxContent.Trip_No + '\'' + ',\'' + '' + InfoboxContent.TripType + '\'' + ');">Live Tracking    </a>'
    + "</td></tr> </table></div>";
    //debugger;
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

//Checks string (Used to rotate icon)
function endsWith(str, suffix) {
    return str.indexOf(suffix, str.length - suffix.length) !== -1;
}

function ETMSLiveTracking(RegNo, FK_CompanyId, Trip_No, TripType) {
    //debugger
    $.ajax({
        url: '../ETMSTripDashboard/ETMSLiveTracking',
        //  url: "@Url.Action("ETMSLiveTracking", "ETMSTripDashboard")",
        type: "GET",
        dataType: "JSON",
        data: { CompId: FK_CompanyId, regNo: RegNo, Trip_Typ: TripType },
        success: function (r) {

            var rst = r;
            if (rst == 1) {
                window.location.href = "../ETMS/ETMSTripDashboard/TrackLive?CompID=" + FK_CompanyId + "&RegNo=" + RegNo + "&TripNo=" + Trip_No + "&TripType=" + TripType;
            }
        }
    });
}

/*Create Route CURRENTLY USED ON ETMS LIVE TRACKING PAGE*/
function CreateETMSRoute(MapControlID, Zoom, CenterLat, CenterLng, InfoboxContent, PathColor, PathOpacity, PathWeight, DataLength) {
    try {
        CreateMarkerWithInfoBoxWithDirectionETMSDashboard(InfoboxContent[0], 1, 1);
        //CreateMarkerWithInfoBoxForLiveTracking(InfoboxContent[0], 1, 1, CenterLat, CenterLng, DataLength);
        var prelat;
        var prelong;
        var preISGSM = 'OFF'
        var j = 0;
        var k = 0;
        var goto = 0;
        if (InfoboxContent.length != 0) {
            for (var i = 0; i <= (InfoboxContent.length - 1) ; i++) {

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

                    if (goto < 1) {

                        var _vehTrackCoordinates = [new google.maps.LatLng(InfoboxContent[i].Lat,
                                            InfoboxContent[i].Long),
                                        new google.maps.LatLng(InfoboxContent[i - 1].Lat,
                                            InfoboxContent[i - 1].Long)
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
                _map.setCenter(new google.maps.LatLng(InfoboxContent[InfoboxContent.length - 1].Lat, InfoboxContent[InfoboxContent.length - 1].Long));
            }

        } else {
        }
    } catch (Error) {
    }
}

/*Create Marker With InfoBox FOR LIVE Tracking*/
function CreateMarkerWithInfoBoxForLiveTracking(InfoboxContentLT, bOpenNewInfoBox, bClosePreviousInfoBox, CenterLat, CenterLng, DataLength) {
    //debugger;
    //alert(DataLength);
    _markerArray = [];
    if (DataLength == '1') {
        _marker = new google.maps.Marker({
            map: _map,
            icon: '/App_Images/' + InfoboxContentLT.Icon,

            position: new google.maps.LatLng(CenterLat, CenterLng)
        });
    }
    else {
        _marker.setIcon('/App_Images/' + InfoboxContentLT.Icon);
        _marker.setPosition(new google.maps.LatLng(CenterLat, CenterLng));
    }

    if (_infowindow && (bClosePreviousInfoBox == "1" || bClosePreviousInfoBox == "")) {
        _infowindow.close();
    }

    _infowindow = new google.maps.InfoWindow();

    _content = "<div class='popup'><table class='info-table'><tr align='left'><td colspan='2'><b style='margin-bottom:5px;display:block;font-size:14px;color:#e90a0b'>"
    + InfoboxContentLT.Route_Name + "</b></td><td><b>" + "</td> </tr><tr><td align='left'><b>Start Location :</b></td><td align='left'>"
    + InfoboxContentLT.Start_Location + "</td></tr><tr><td align='left'><b>End Location :</b></td><td align='left'>"
    + InfoboxContentLT.End_Location + "</b></td></tr><tr><td align='left'><b>Registration No. :</b></td><td align='left'>"
    + InfoboxContentLT.RegistrationNo + "</td></tr><tr><td  align='left'><b>Chauffeur Name :</b></td><td align='left'>"
    + InfoboxContentLT.ChauffeurName + "</td></tr><tr><td  align='left'><b>Chauffeur Mobile No.:</b></td><td align='left'>"
    + InfoboxContentLT.ChauffeurMob + "</td></tr><tr><td align='left'><b>Recorded :</b></td><td align='left'>"
    + InfoboxContentLT.DeviceDateTime + "</td></tr><tr><td align='left'><b>Trip No.:</b></td><td align='left'>"
    + InfoboxContentLT.Trip_No + "</td></tr><tr><td align='left'><b>Trip Type :</b></td><td align='left'>"
    + InfoboxContentLT.TripType
    "</td></tr>"
     + "</td></tr> </table></div>";
    _infowindow.setContent(_content);
    _infowindow.open(_map, _marker);

    if (bOpenNewInfoBox == "1") {
        _infowindow.setContent(_content);
        _infowindow.open(_map, _marker);
    }

}

/*Function to load Moving Markers For ETMS Dashboard*/
function DrawMarkerWithMovingDirectionETMSTripDashboard(MapControlID, Zoom, CenterLat, CenterLng, InfoboxContent) {
    //debugger;
    try {
        var iVar = 0;
        ClearMap();

        if (InfoboxContent.length != 0) {
            for (var i = 0; i <= InfoboxContent.length - 1; i++) {
                CreateMarkerWithInfoBoxWithDirectionETMSTripDashboard(InfoboxContent[i])
                loc = new google.maps.LatLng(InfoboxContent[i].Lat, InfoboxContent[i].Long);
                bounds.extend(loc)
            }

            chkZommChange = false;

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

        }
    } catch (Error) {

    }
}



/*Function to load Moving Markers For ETMS Dashboard*/
function DrawMarkerWithMovingDirectionETMSDashboard(MapControlID, Zoom, CenterLat, CenterLng, InfoboxContent) {
    //debugger;
    try {
        var iVar = 0;
        ClearMap();

        if (InfoboxContent.length != 0) {
            for (var i = 0; i <= InfoboxContent.length - 1; i++) {
                CreateMarkerWithInfoBoxWithDirectionETMSDashboard(InfoboxContent[i])
                loc = new google.maps.LatLng(InfoboxContent[i].Lat, InfoboxContent[i].Long);
                bounds.extend(loc)
            }

            chkZommChange = false;

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

        }
    } catch (Error) {

    }
}

/*Creates Marker With InfoBox With Direction For ETMS Dashboard*/
function CreateMarkerWithInfoBoxWithDirectionETMSDashboard(InfoboxContent, bOpenNewInfoBox, bClosePreviousInfoBox) {
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


    if (InfoboxContent.PK_Trip_ID == '0') {// IF Vehicle is not in trip, infobox will contain info of vehicle only

        _content = "<div class='popup'><table class='info-table'><tr align='left'><td colspan='2'><b>"
    + InfoboxContent.RegistrationNo + "</b></td><td><b>"
    + InfoboxContent.ModelName + "</b></td></tr><tr nowrap><td width='90px' align='left'><b>Driver Name:</b></td><td align='left'>"
    + InfoboxContent.ChauffeurName + "</td></tr><tr><td  align='left'><b>Driver Mob:</b></td><td align='left'>"
    + InfoboxContent.ChauffeurMob + "</td> </tr><tr><td  align='left'><b>Recorded :</b></td><td align='left'>"
    + InfoboxContent.DeviceDateTime + "</td> </tr><tr><td align='left'><b>Speed :</b></td><td align='left'>"
      + InfoboxContent.Speed + " Km/Hr</td> </tr><tr><td align='left'><b>Device No. :</b></td><td align='left'>"
        + InfoboxContent.DeviceNo + "</td> </tr><tr><td align='left'><b>IMEI No. :</b></td><td align='left'>"
        + InfoboxContent.IMEINo + "</td> </tr><tr><td align='left'><b>Sim No. :</b></td><td align='left'>"                                                              //
    + InfoboxContent.SIMNo
    + '</td></tr>'
    + '<tr><td align="left"></td><td align="left"> <a style=' + livetrack + '  href="javascript:void(0)"  onclick="ETMSLiveTrackingETMSDashboard(\'' + InfoboxContent.RegistrationNo + '\'' + ',\'' + '' + InfoboxContent.FK_CompanyId + '\'' + ',\'' + '' + InfoboxContent.Trip_No + '\'' + ',\'' + '' + InfoboxContent.TripType + '\'' + ');">Live Tracking    </a>'
    + '</td></tr> </table></div>';

    }
    else {// IF Vehicle is in trip, infobox will contain info of vehicle & trip both

        _content = "<div class='popup'><table class='info-table'><tr align='left'><td colspan='2'><b style='margin-bottom:5px;display:block;font-size:14px;color:#e90a0b'>"
                    + InfoboxContent.Route_Name + "</b></td><td><b>" + "</td> </tr><tr><td align='left'><b>Start Location :</b></td><td align='left'>"
                    + InfoboxContent.Start_Location + "</td></tr><tr><td align='left'><b>End Location :</b></td><td align='left'>"
                    + InfoboxContent.End_Location + "</b></td></tr><tr><td align='left'><b>Registration No. :</b></td><td align='left'>"
                    + InfoboxContent.RegistrationNo + "</td></tr><tr><td  align='left'><b>Chauffeur Name :</b></td><td align='left'>"
                    + InfoboxContent.ChauffeurName + "</td></tr><tr><td  align='left'><b>Chauffeur Mobile No.:</b></td><td align='left'>"
                    + InfoboxContent.ChauffeurMob + "</td></tr><tr><td align='left'><b>Recorded :</b></td><td align='left'>"
                    + InfoboxContent.DeviceDateTime + "</td></tr><tr><td align='left'><b>Trip No.:</b></td><td align='left'>"
                    + InfoboxContent.Trip_No + "</td></tr><tr><td align='left'><b>Service Type :</b></td><td align='left'>"
                    + InfoboxContent.TripType
                    + '<tr><td align="left"></td><td align="left"> <a style=' + livetrack + '  href="javascript:void(0)"  onclick="ETMSLiveTrackingETMSDashboard(\'' + InfoboxContent.RegistrationNo + '\'' + ',\'' + '' + InfoboxContent.FK_CompanyId + '\'' + ',\'' + '' + InfoboxContent.Trip_No + '\'' + ',\'' + '' + InfoboxContent.TripType + '\'' + ');">Live Tracking    </a>'
                         + "</td></tr> </table></div>";
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

/*Creates Marker With InfoBox With Direction For ETMS Dashboard*/
function CreateMarkerWithInfoBoxWithDirectionETMSTripDashboard(InfoboxContent, bOpenNewInfoBox, bClosePreviousInfoBox) {
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


    if (InfoboxContent.PK_Trip_ID == '0') {// IF Vehicle is not in trip, infobox will contain info of vehicle only

        _content = "<div class='popup'><table class='info-table'><tr align='left'><td colspan='2'><b>"
    + InfoboxContent.RegistrationNo + "</b></td><td><b>"
    + InfoboxContent.ModelName + "</b></td></tr><tr nowrap><td width='90px' align='left'><b>Driver Name:</b></td><td align='left'>"
    + InfoboxContent.ChauffeurName + "</td></tr><tr><td  align='left'><b>Driver Mob:</b></td><td align='left'>"
    + InfoboxContent.ChauffeurMob + "</td> </tr><tr><td  align='left'><b>Recorded :</b></td><td align='left'>"
    + InfoboxContent.DeviceDateTime + "</td> </tr><tr><td align='left'><b>Speed :</b></td><td align='left'>"
      + InfoboxContent.Speed + " Km/Hr</td> </tr><tr><td align='left'><b>Device No. :</b></td><td align='left'>"
        + InfoboxContent.DeviceNo + "</td> </tr><tr><td align='left'><b>IMEI No. :</b></td><td align='left'>"
        + InfoboxContent.IMEINo + "</td> </tr><tr><td align='left'><b>Sim No. :</b></td><td align='left'>"                                                              //
    + InfoboxContent.SIMNo
    + '</td></tr>'
    + '<tr><td align="left"></td><td align="left"> <a style=' + livetrack + '  href="javascript:void(0)"  onclick="ETMSLiveTrackingETMSTripDashboard(\'' + InfoboxContent.RegistrationNo + '\'' + ',\'' + '' + InfoboxContent.FK_CompanyId + '\'' + ',\'' + '' + InfoboxContent.Trip_No + '\'' + ',\'' + '' + InfoboxContent.TripType + '\'' + ');">Live Tracking    </a>'
    + '</td></tr> </table></div>';

    }
    else {// IF Vehicle is in trip, infobox will contain info of vehicle & trip both

        _content = "<div class='popup'><table class='info-table'><tr align='left'><td colspan='2'><b style='margin-bottom:5px;display:block;font-size:14px;color:#e90a0b'>"
                    + InfoboxContent.Route_Name + "</b></td><td><b>" + "</td> </tr><tr><td align='left'><b>Start Location :</b></td><td align='left'>"
                    + InfoboxContent.Start_Location + "</td></tr><tr><td align='left'><b>End Location :</b></td><td align='left'>"
                    + InfoboxContent.End_Location + "</b></td></tr><tr><td align='left'><b>Registration No. :</b></td><td align='left'>"
                    + InfoboxContent.RegistrationNo + "</td></tr><tr><td  align='left'><b>Chauffeur Name :</b></td><td align='left'>"
                    + InfoboxContent.ChauffeurName + "</td></tr><tr><td  align='left'><b>Chauffeur Mobile No.:</b></td><td align='left'>"
                    + InfoboxContent.ChauffeurMob + "</td></tr><tr><td align='left'><b>Recorded :</b></td><td align='left'>"
                    + InfoboxContent.DeviceDateTime + "</td></tr><tr><td align='left'><b>Trip No.:</b></td><td align='left'>"
                    + InfoboxContent.Trip_No + "</td></tr><tr><td align='left'><b>Service Type :</b></td><td align='left'>"
                    + InfoboxContent.TripType
                    + '<tr><td align="left"></td><td align="left"> <a style=' + livetrack + '  href="javascript:void(0)"  onclick="ETMSLiveTrackingETMSTripDashboard(\'' + InfoboxContent.RegistrationNo + '\'' + ',\'' + '' + InfoboxContent.FK_CompanyId + '\'' + ',\'' + '' + InfoboxContent.Trip_No + '\'' + ',\'' + '' + InfoboxContent.TripType + '\'' + ');">Live Tracking    </a>'
                         + "</td></tr> </table></div>";
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

/*Creates Marker With InfoBox With Direction For ETMS Dashboard*/
function CreateMarkerWithInfoBoxWithDirectionETMSDashboardLive(InfoboxContent, bOpenNewInfoBox, bClosePreviousInfoBox) {
    _marker = new google.maps.Marker({
        map: _map,
        position: new google.maps.LatLng(InfoboxContent.Lat, InfoboxContent.Long)
    });

   // _marker.setIcon('/App_Images/' + InfoboxContent.Icon);
        //var str = RotateIcon.makeIcon('/App_Images/' + InfoboxContent.Icon).setRotation({ deg: InfoboxContent.Heading }).getUrl();
        //var a = endsWith(str, "AD12TscoAAAAAElFTkSuQmCC");
        //if (endsWith(str, "AD12TscoAAAAAElFTkSuQmCC"))
        //    _marker.setIcon('/App_Images/' + InfoboxContent.Icon);
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


    if (InfoboxContent.PK_Trip_ID == '0') {// IF Vehicle is not in trip, infobox will contain info of vehicle only

        _content = "<div class='popup'><table class='info-table'><tr align='left'><td colspan='2'><b>"
    + InfoboxContent.RegistrationNo + "</b></td><td><b>"
    + InfoboxContent.ModelName + "</b></td></tr><tr nowrap><td width='90px' align='left'><b>Driver Name:</b></td><td align='left'>"
    + InfoboxContent.ChauffeurName + "</td></tr><tr><td  align='left'><b>Driver Mob:</b></td><td align='left'>"
    + InfoboxContent.ChauffeurMob + "</td> </tr><tr><td  align='left'><b>Recorded :</b></td><td align='left'>"
    + InfoboxContent.DeviceDateTime + "</td> </tr><tr><td align='left'><b>Speed :</b></td><td align='left'>"
      + InfoboxContent.Speed + " Km/Hr</td> </tr><tr><td align='left'><b>Device No. :</b></td><td align='left'>"
        + InfoboxContent.DeviceNo + "</td> </tr><tr><td align='left'><b>IMEI No. :</b></td><td align='left'>"
        + InfoboxContent.IMEINo + "</td> </tr><tr><td align='left'><b>Sim No. :</b></td><td align='left'>"                                                              //
    + InfoboxContent.SIMNo
    + '</td></tr>'
    + '<tr><td align="left"></td><td align="left"> <a style=' + livetrack + '  href="javascript:void(0)"  onclick="ETMSLiveTrackingETMSDashboard(\'' + InfoboxContent.RegistrationNo + '\'' + ',\'' + '' + InfoboxContent.FK_CompanyId + '\'' + ',\'' + '' + InfoboxContent.Trip_No + '\'' + ',\'' + '' + InfoboxContent.TripType + '\'' + ');">Live Tracking    </a>'
    + '</td></tr> </table></div>';

    }
    else {// IF Vehicle is in trip, infobox will contain info of vehicle & trip both

        _content = "<div class='popup'><table class='info-table'><tr align='left'><td colspan='2'><b style='margin-bottom:5px;display:block;font-size:14px;color:#e90a0b'>"
                    + InfoboxContent.Route_Name + "</b></td><td><b>" + "</td> </tr><tr><td align='left'><b>Start Location :</b></td><td align='left'>"
                    + InfoboxContent.Start_Location + "</td></tr><tr><td align='left'><b>End Location :</b></td><td align='left'>"
                    + InfoboxContent.End_Location + "</b></td></tr><tr><td align='left'><b>Registration No. :</b></td><td align='left'>"
                    + InfoboxContent.RegistrationNo + "</td></tr><tr><td  align='left'><b>Chauffeur Name :</b></td><td align='left'>"
                    + InfoboxContent.ChauffeurName + "</td></tr><tr><td  align='left'><b>Chauffeur Mobile No.:</b></td><td align='left'>"
                    + InfoboxContent.ChauffeurMob + "</td></tr><tr><td align='left'><b>Recorded :</b></td><td align='left'>"
                    + InfoboxContent.DeviceDateTime + "</td></tr><tr><td align='left'><b>Trip No.:</b></td><td align='left'>"
                    + InfoboxContent.Trip_No + "</td></tr><tr><td align='left'><b>Service Type :</b></td><td align='left'>"
                    + InfoboxContent.TripType
                    + '<tr><td align="left"></td><td align="left"> <a style=' + livetrack + '  href="javascript:void(0)"  onclick="ETMSLiveTrackingETMSDashboard(\'' + InfoboxContent.RegistrationNo + '\'' + ',\'' + '' + InfoboxContent.FK_CompanyId + '\'' + ',\'' + '' + InfoboxContent.Trip_No + '\'' + ',\'' + '' + InfoboxContent.TripType + '\'' + ');">Live Tracking    </a>'
                         + "</td></tr> </table></div>";
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


// LIVE TRACKING FOR ETMS DASHBOARD
function ETMSLiveTrackingETMSTripDashboard(RegNo, FK_CompanyId, Trip_No, TripType) {
    //debugger
    $.ajax({
        url: '../ETMSDashboard/ETMSDashboardLiveTracking',
        //  url: "@Url.Action("ETMSDashboardLiveTracking", "ETMSDashboard")",
        type: "GET",
        dataType: "JSON",
        data: { CompId: FK_CompanyId, regNo: RegNo, Trip_Typ: TripType },
        success: function (r) {

            var rst = r;
            if (rst == 1) {
                window.location.href = "../ETMS/ETMSTripDashboard/TrackLive?CompID=" + FK_CompanyId + "&RegNo=" + RegNo + "&TripNo=" + Trip_No + "&TripType=" + TripType;
            }
        }
    });
}
// LIVE TRACKING FOR ETMS DASHBOARD
function ETMSLiveTrackingETMSDashboard(RegNo, FK_CompanyId, Trip_No, TripType) {
    //debugger
    $.ajax({
        url: '../ETMSDashboard/ETMSDashboardLiveTracking',
        //  url: "@Url.Action("ETMSDashboardLiveTracking", "ETMSDashboard")",
        type: "GET",
        dataType: "JSON",
        data: { CompId: FK_CompanyId, regNo: RegNo, Trip_Typ: TripType },
        success: function (r) {

            var rst = r;
            if (rst == 1) {
                window.location.href = "../ETMS/ETMSDashboard/TrackLive?CompID=" + FK_CompanyId + "&RegNo=" + RegNo + "&TripNo=" + Trip_No + "&TripType=" + TripType;
            }
        }
    });
}