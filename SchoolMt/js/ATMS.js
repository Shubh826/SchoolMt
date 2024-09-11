/*
   Created By  : Tarique
   Created Date: 2018-04-04
   Purpose     : Google Map 
*/


/********************CODE FOR ATMS STARTS HERE****************************/

//Trip Dashboard
var _mapOptions;
var _map;
var _SetInterval;
var _RoutePath;
var _infowindow;
var _marker;
var _markerArray = [];
var _vehPath;
var _RoutePath;
var trHTML = ''
var loc;
var bounds = new google.maps.LatLngBounds();
var chkZoom = false;
var chkZommChange = false;
var lastzoom = 0;
var _zoomDefault = [6, 12, 13, 14, 16, 17, 18, 49, 21, 9, 19, 22]
var _content;
var _InfoboxContent = [];
var _counter = 0;

/*Function to load markers*/
function DrawATMSTripMarker(MapControlID, Zoom, CenterLat, CenterLng, InfoboxContent,companyID) {
    
    try {
        
        var iVar = 0;
        ClearMap();
        if (InfoboxContent.length != 0) {
            for (var i = 0; i <= InfoboxContent.length - 1; i++) {

                CreateATMSTripMarkerWithInfoBox(InfoboxContent[i],companyID)
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
function CreateATMSTripMarkerWithInfoBox(InfoboxContent,companyID, bOpenNewInfoBox, bClosePreviousInfoBox) {
    
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
   
    if (companyID == '0') {
        _content = "<div class='popup' style='width:450px;'><table class='info-table' width='100%'><tr align='left'><td colspan='4'><b style='margin-bottom:5px;display:block;font-size:14px;color:#e90a0b'>" + InfoboxContent.Route_Name + "</b></td></tr><tr align='left'><td width='90px' align='left'><b>Vehicle No.:</b></td><td  colspan='3'><b>"
        + InfoboxContent.Registration_No + "</b></td></tr><tr nowrap><td width='90px' align='left'><b>Driver Name:</b></td><td align='left'>"
        + InfoboxContent.ChauffeurName + "</td><td  align='left'><b>Driver Mob:</b></td><td align='left'>"
        + InfoboxContent.ChauffeurMob + "</td> </tr><tr><td  align='left'><b>Recorded :</b></td><td align='left'>"
        + InfoboxContent.Device_DateTime + "</td>"
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


        + "</td></tr><tr><td align='left'><b>Device No.:</b></td><td align='left'>"
        + InfoboxContent.DeviceNo
        + "</td><td align='left'><b>Client Name</b></td><td align='left'>"
        + InfoboxContent.ClientName

        + "</td></tr><tr><td align='left'><b>Branch Name:</b></td><td align='left'>"
        + InfoboxContent.BranchName
        + "</td>"

        + "</td></tr>"
          + "</td></tr><tr><td align='left'><b>Alarm :</b></td><td colspan='3' align='left'>"
        + InfoboxContent.AlarmDesc
        + "</td></tr>"
            + '<tr><td align="left"></td><td align="left"><a href="javascript:void(0)" onclick="SendATMSTripLiveTracking(\'' + InfoboxContent.FK_CompanyID + '\'' + ',\'' + InfoboxContent.DeviceNo + '\'' + ',\'' + InfoboxContent.Trip_No + '\'' + ',\'' + InfoboxContent.ClientID + '\'' + ',\'' + InfoboxContent.BranchID + '\');">Live Tracking</a>'
          + "</td></tr>"
        + "</table></div>";
    }
    else
    {
        _content = "<div class='popup' style='width:450px;'><table class='info-table' width='100%'><tr align='left'><td colspan='4'><b style='margin-bottom:5px;display:block;font-size:14px;color:#e90a0b'>" + InfoboxContent.Route_Name
        + "</b></td></tr><tr align='left'><td width='90px' align='left'><b>Vehicle No.:</b></td><td  colspan='3'><b>"
       + InfoboxContent.Registration_No + "</b></td></tr><tr nowrap><td width='90px' align='left'><b>Driver Name:</b></td><td align='left'>"
       + InfoboxContent.ChauffeurName + "</td><td  align='left'><b>Driver Mob:</b></td><td align='left'>"
       + InfoboxContent.ChauffeurMob + "</td> </tr><tr><td  align='left'><b>Recorded :</b></td><td align='left'>"
       + InfoboxContent.Device_DateTime + "</td>"
       + "<td align='left'><b>Trip No. :</b></td><td align='left'>"
       + InfoboxContent.Trip_No
       + "</td></tr><tr><td align='left'><b>Customer :</b></td><td align='left'>"
       + InfoboxContent.CustomerName
       + "</td><td align='left'><b>LR No. :</b></td><td align='left'>"
       + InfoboxContent.LRNo
       + "</td></tr><tr><td align='left'><b>Invoice No. :</b></td><td align='left'>"
       + InfoboxContent.InvoiceNo
       + "</td><td align='left'><b>Invoice Date :</b></td><td align='left'>"
       + InfoboxContent.InvoiceDate
       + "</td></tr><tr><td align='left'><b>Origin :</b></td><td align='left'>"
       + InfoboxContent.Origin

        + "</td></tr><tr><td align='left'><b>Destination :</b></td><td align='left'>"
       + InfoboxContent.Destination
        

       + "</td></tr><tr><td align='left'><b>Device No.:</b></td><td align='left'>"
       + InfoboxContent.DeviceNo
       + "</td><td align='left'><b>Client Name</b></td><td align='left'>"
       + InfoboxContent.ClientName

       + "</td></tr><tr><td align='left'><b>Branch Name:</b></td><td align='left'>"
       + InfoboxContent.BranchName
       + "</td>"
       + "</td><td align='left'><b>DO :</b></td><td align='left'>"
       + InfoboxContent.DeliveryOrder
       + "</td>"
       + "</td></tr><tr><td align='left'><b>Net Weight:</b></td><td align='left'>"
       + InfoboxContent.NetWeight
       + "</td>"

       + "</td></tr>"
         + "</td></tr><tr><td align='left'><b>Alarm :</b></td><td colspan='3' align='left'>"
       + InfoboxContent.AlarmDesc
       + "</td></tr>"
           + '<tr><td align="left"></td><td align="left"><a href="javascript:void(0)" onclick="SendATMSTripLiveTracking(\'' + InfoboxContent.FK_CompanyID + '\'' + ',\'' + InfoboxContent.DeviceNo + '\'' + ',\'' + InfoboxContent.Trip_No + '\'' + ',\'' + InfoboxContent.ClientID + '\'' + ',\'' + InfoboxContent.BranchID + '\');">Live Tracking</a>'
         + "</td></tr>"
       + "</table></div>";
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

function SendATMSTripLiveTracking(FK_CompanyId, Device_No, TripNo, Client_ID, Branch_ID) {
    
    $.ajax({
        url: '../ATMS/ATMSTripDashboard/TripLiveTrackingData',       
        type: "GET",
        dataType: "JSON",
        data: { CompId: FK_CompanyId, DeviceNo: Device_No, tripNo: TripNo, ClientID: Client_ID, BranchID: Branch_ID },
        success: function (r) {
            var rst = r;
            if (rst == 1) {
                window.location.href = "../ATMS/ATMSTripDashboard/TripLiveTracking";
            }
        }
    });
}

/*Create Trip Marker With InfoBox*/
function CreateATMSTripMarkerWithInfoBoxforLiveTracking(InfoboxContent, bOpenNewInfoBox, bClosePreviousInfoBox, id, companyID) {
    
    _marker = new google.maps.Marker({
        map: _map,
        position: new google.maps.LatLng(InfoboxContent.Lat, InfoboxContent.Long)
    });

    var str = RotateIcon.makeIcon('/App_Images/' + InfoboxContent.Icon).setRotation({ deg: InfoboxContent.Heading }).getUrl();

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
    if (companyID == '0') {
        _content =
            "<div class='popup' style='width:450px;'><table class='info-table' width='100%'><tr align='left'><td colspan='4'><b style='margin-bottom:5px;display:block;font-size:14px;color:#e90a0b'>" + InfoboxContent.Route_Name + "</b></td></tr><tr align='left'><td width='90px' align='left'><b>Vehicle No.:</b></td><td  colspan='3'><b>"
        + InfoboxContent.Registration_No + "</b></td></tr><tr nowrap><td width='90px' align='left'><b>Driver Name:</b></td><td align='left'>"
        + InfoboxContent.ChauffeurName + "</td><td  align='left'><b>Driver Mob:</b></td><td align='left'>"
        + InfoboxContent.ChauffeurMob + "</td> </tr><tr><td  align='left'><b>Recorded :</b></td><td align='left'>"
        + InfoboxContent.Device_DateTime + "</td>"
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


        + "</td></tr><tr><td align='left'><b>Device No.:</b></td><td align='left'>"
        + InfoboxContent.DeviceNo
        + "</td><td align='left'><b>Client Name</b></td><td align='left'>"
        + InfoboxContent.ClientName

        + "</td></tr><tr><td align='left'><b>Branch Name:</b></td><td align='left'>"
        + InfoboxContent.BranchName
        + "</td>"


        + "</td></tr>"
          + "</td></tr><tr><td align='left'><b>Alarm :</b></td><td colspan='3' align='left'>"
        + InfoboxContent.AlarmDesc
        + "</td></tr>"

          //+ '<tr><td align="left"></td><td align="left"><a href="#" onclick="ShowTrafficOnMap(\'' + InfoboxContent.Trip_No + '\');">Live Traffic</a>'
        + "</td></tr>"
        + "</table></div>";
    }
    else
    {
        _content = "<div class='popup' style='width:450px;'><table class='info-table' width='100%'><tr align='left'><td colspan='4'><b style='margin-bottom:5px;display:block;font-size:14px;color:#e90a0b'>" + InfoboxContent.Route_Name
      + "</b></td></tr><tr align='left'><td width='90px' align='left'><b>Vehicle No.:</b></td><td  colspan='3'><b>"
     + InfoboxContent.Registration_No + "</b></td></tr><tr nowrap><td width='90px' align='left'><b>Driver Name:</b></td><td align='left'>"
     + InfoboxContent.ChauffeurName + "</td><td  align='left'><b>Driver Mob:</b></td><td align='left'>"
     + InfoboxContent.ChauffeurMob + "</td> </tr><tr><td  align='left'><b>Recorded :</b></td><td align='left'>"
     + InfoboxContent.Device_DateTime + "</td>"
     + "<td align='left'><b>Trip No. :</b></td><td align='left'>"
     + InfoboxContent.Trip_No
     + "</td></tr><tr><td align='left'><b>Customer :</b></td><td align='left'>"
     + InfoboxContent.CustomerName
     + "</td><td align='left'><b>LR No. :</b></td><td align='left'>"
     + InfoboxContent.LRNo
     + "</td></tr><tr><td align='left'><b>Invoice No. :</b></td><td align='left'>"
     + InfoboxContent.InvoiceNo
     + "</td><td align='left'><b>Invoice Date :</b></td><td align='left'>"
     + InfoboxContent.InvoiceDate
     + "</td></tr><tr><td align='left'><b>Origin :</b></td><td align='left'>"
     + InfoboxContent.Origin

      + "</td></tr><tr><td align='left'><b>Destination :</b></td><td align='left'>"
     + InfoboxContent.Destination


     + "</td></tr><tr><td align='left'><b>Device No.:</b></td><td align='left'>"
     + InfoboxContent.DeviceNo
     + "</td><td align='left'><b>Client Name</b></td><td align='left'>"
     + InfoboxContent.ClientName

     + "</td></tr><tr><td align='left'><b>Branch Name:</b></td><td align='left'>"
     + InfoboxContent.BranchName
     + "</td>"
     + "</td><td align='left'><b>DO :</b></td><td align='left'>"
     + InfoboxContent.DeliveryOrder
     + "</td>"
     + "</td></tr><tr><td align='left'><b>Net Weight:</b></td><td align='left'>"
     + InfoboxContent.NetWeight
     + "</td>"

     + "</td></tr>"
       + "</td></tr><tr><td align='left'><b>Alarm :</b></td><td colspan='3' align='left'>"
     + InfoboxContent.AlarmDesc
     + "</td></tr>"
        // + '<tr><td align="left"></td><td align="left"><a href="javascript:void(0)" onclick="SendATMSTripLiveTracking(\'' + InfoboxContent.FK_CompanyID + '\'' + ',\'' + InfoboxContent.DeviceNo + '\'' + ',\'' + InfoboxContent.Trip_No + '\'' + ',\'' + InfoboxContent.ClientID + '\'' + ',\'' + InfoboxContent.BranchID + '\');">Live Tracking</a>'
       + "</td></tr>"
     + "</table></div>";
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

/**CREATE ROUTE ****/
function CreateRoute(MapControlID, Zoom, CenterLat, CenterLng, InfoboxContent, PathColor, PathOpacity, PathWeight, IsMarkerNotRequired) {
    try {


        ClearRoute()
        var ISGSM = 'OFF';
        var prelat;
        var prelong;
        var preISGSM = 'OFF'
        var j = 0;
        var k = 0;
        var goto = 0;
        if (InfoboxContent.length != 0) {
            for (var i = 0; i <= InfoboxContent.length - 1; i++) {
               
               // CreateMarkerWithInfoBox(InfoboxContent[i])

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
                        if (k > 0) {
                            var _vehTrackCoordinates = [new google.maps.LatLng(InfoboxContent[i].latitude,
                                                InfoboxContent[i].longitude),
                                            new google.maps.LatLng(InfoboxContent[k].latitude,
                                                InfoboxContent[k].longitude)
                            ];

                        } else if (ISGSM == 'OFF') {
                            var _vehTrackCoordinates = [new google.maps.LatLng(InfoboxContent[i].latitude,
                                                     InfoboxContent[i].longitude),
                                                 new google.maps.LatLng(InfoboxContent[i - 1].latitude,
                                                     InfoboxContent[i - 1].longitude)
                            ];
                        }



                        if (IsMarkerNotRequired) {
                            _RoutePath = new google.maps.Polyline({
                                path: _vehTrackCoordinates,
                                strokeColor: PathColor,//"#CC3300",
                                strokeOpacity: PathOpacity,//80,
                                strokeWeight: PathWeight,//2,
                                icons: [{

                                    icon: {
                                        //path: google.maps.SymbolPath.BACKWARD_CLOSED_ARROW,
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
                        }
                        else {
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
                        }

                        

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

/*Create Marker With InfoBox*/
function CreateMarkerWithInfoBox(InfoboxContent, bOpenNewInfoBox, bClosePreviousInfoBox) {


    // var img='http://chart.apis.google.com/chart?chst=d_map_pin_letter&chld='+A|FF0000|000000'
    _marker = new google.maps.Marker({
        map: _map,
        icon: '/App_Images/' + 'pin_yellow.png',
        position: new google.maps.LatLng(InfoboxContent.latitude, InfoboxContent.longitude)
    });


    if (_infowindow && (bClosePreviousInfoBox == "1" || bClosePreviousInfoBox == "")) {
        _infowindow.close();
    }
   // var livetrack = "display:none;";
   // if (InfoboxContent.Status == 'P') {
   //     livetrack = "display:inline-block;"
   // }
    _infowindow = new google.maps.InfoWindow();

    _content = "<div class='popup'><table class='info-table'><tr nowrap><td width='90px' align='left'><b>Device No:</b></td><td align='left'>"
    + InfoboxContent.deviceNo + "</b></td></tr><tr nowrap><td width='90px' align='left'><b>Device Datetime:</b></td><td align='left'>"
    + InfoboxContent.deviceDatetime + "</b></td></tr><tr nowrap><td width='90px' align='left'><b>Latitude:</b></td><td align='left'>"
    + InfoboxContent.latitude + "</b></td></tr><tr nowrap><td width='90px' align='left'><b>Longitude:</b></td><td align='left'>"
    + InfoboxContent.longitude
    + "</td></tr> </table></div>";

   // _content = "<div class='popup'><table class='info-table'><tr align='left'><td colspan='2'><b>"
    //+ InfoboxContent.deviceNo + "</b></td><td><b>"
    //+ "" + "</b></td></tr><tr nowrap><td width='90px' align='left'><b>Device Datetime:</b></td><td align='left'>"
    //+ InfoboxContent.deviceDatetime
   // + InfoboxContent.ModelName + "</b></td></tr><tr nowrap><td width='90px' align='left'><b>Driver Name:</b></td><td align='left'>"
    //+ InfoboxContent.ChauffeurName + "</td></tr><tr><td  align='left'><b>Driver Mob:</b></td><td align='left'>"
   // + InfoboxContent.ChauffeurMob + "</td> </tr><tr><td  align='left'><b>Recorded :</b></td><td align='left'>"
   // + InfoboxContent.deviceDatetime + "</td> </tr><tr><td align='left'><b>Speed :</b></td><td align='left'>"
     // + InfoboxContent.speed + " Km/Hr</td> </tr><tr><td align='left'><b>Speed :</b></td><td align='left'>"
       // + InfoboxContent.deviceNo + "</td> </tr><tr><td align='left'><b>IMEI No. :</b></td><td align='left'>"
       // + InfoboxContent.imeiNo + "</td> </tr><tr><td align='left'><b>Sim No. :</b></td><td align='left'>"
    //+ InfoboxContent.batteryPercantage
   // + "</td></tr>"
   // + '<tr><td align="left"></td><td align="left"><a style=' + livetrack + '  href="#" onclick="SendLiveTracking(\'' + InfoboxContent.RegistrationNo + '\'' + ',' + InfoboxContent.FK_CompanyId + ');">Live Tracking</a>'
    //+ "</td></tr> </table></div>";
    //+ "<tr><td align='left'></td><td align='left'><a href='#' onclick='SendLiveTracking(''+RegNo,CompId)'>Live Tracking</a>"
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

/*Play Route*/
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

function Pause() {
    clearTimeout(_SetInterval);
    // ClearMap();
}

function ClearMap() {
    removeLine();
    setMapOnAll(null);
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
    if (typeof _markerArray === 'undefined') { } else {
        for (var i = 0; i < _markerArray.length; i++) {
            _markerArray[i].setMap(map);
        }

        _markerArray = [];
    }
}

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

/***function create route for fixed type route ****/

function CreateRouteForFixed(MapControlID, Zoom, CenterLat, CenterLng, InfoboxContent, PathColor, PathOpacity, PathWeight, IsMarkerNotRequired) {
    try {

        //debugger
        ClearRoute()
        var ISGSM = 'OFF';
        var prelat;
        var prelong;
        var preISGSM = 'OFF'
        var j = 0;
        var k = 0;
        var goto = 0;
        if (InfoboxContent.length != 0) {
            for (var i = 0; i <= InfoboxContent.length - 1; i++) {

                // CreateMarkerWithInfoBox(InfoboxContent[i])

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
                        if (k > 0) {
                            var _vehTrackCoordinates = [new google.maps.LatLng(InfoboxContent[i].Lat,
                                                InfoboxContent[i].Long),
                                            new google.maps.LatLng(InfoboxContent[k].Lat,
                                                InfoboxContent[k].Long)
                            ];

                        } else if (ISGSM == 'OFF') {
                            var _vehTrackCoordinates = [new google.maps.LatLng(InfoboxContent[i].Lat,
                                                     InfoboxContent[i].Long),
                                                 new google.maps.LatLng(InfoboxContent[i - 1].Lat,
                                                     InfoboxContent[i - 1].Long)
                            ];
                        }


                        if (IsMarkerNotRequired) {
                            _RoutePath = new google.maps.Polyline({
                                path: _vehTrackCoordinates,
                                strokeColor: PathColor,//"#CC3300",
                                strokeOpacity: PathOpacity,//80,
                                strokeWeight: PathWeight,//2,
                                icons: [{

                                    icon: {
                                        //path: google.maps.SymbolPath.BACKWARD_CLOSED_ARROW,
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
                        }
                        else {
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
                        }

                       

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

/********************CODE FOR ATMS ENDS   HERE****************************/

