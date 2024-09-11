/*
   Created By  : ANJANI SINGH/Deepak Singh
   Created Date: 2015-12-22
*/

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
var markerCluster;
var _InfoboxContent = [];
var _counter = 0;

var iconvar = 0;
var starticon = 0;
var len;




/*Create Route*/
function CreateRoute(MapControlID, Zoom, CenterLat, CenterLng, InfoboxContent, PathColor, PathOpacity, PathWeight) {
    debugger
    try {


        ClearRoute()
        var ISGSM = 'OFF';
        var prelat;
        var prelong;
        var preISGSM = 'OFF'
        var j = 0;
        var k = 0;
        var goto = 0;
        len = InfoboxContent.length;
        if (InfoboxContent.length != 0) {
            for (var i = 0; i <= InfoboxContent.length - 1; i++) {               

                
                
                if (i > 0) {


                //    if (InfoboxContent[i].ISGSM == 'ON' && preISGSM == 'OFF') {
                //        k = i - 1;
                //        preISGSM = 'ON';
                //        goto = 1;
                //    }
                //    else
                //        if (InfoboxContent[i].ISGSM == 'ON' && preISGSM == 'ON') {
                //            preISGSM = 'ON';
                //            goto = 1;
                //        }
                //        else if (InfoboxContent[i].ISGSM == 'OFF' && preISGSM == 'ON') {
                //            PathColor = '#CC3300';
                //            preISGSM = 'OFF';
                //            goto = 0;

                //        } else if (InfoboxContent[i].ISGSM == 'OFF' && preISGSM == 'OFF') {
                //            k = 0;
                //            PathColor = '#0B3B0B';
                //            preISGSM = 'OFF';
                //            goto = 0;
                //        }

                //    if (goto < 1) {
                //        if (k > 0) {
                //            var _vehTrackCoordinates = [new google.maps.LatLng(InfoboxContent[i].lat,
                //                                InfoboxContent[i].lng),
                //                            new google.maps.LatLng(InfoboxContent[k].lat,
                //                                InfoboxContent[k].lng)
                //            ];

                //        }
                        //else if (InfoboxContent[i].ISGSM == 'OFF') {

                            var _vehTrackCoordinates = [new google.maps.LatLng(InfoboxContent[i].lat,
                                                     InfoboxContent[i].lng),
                                                 new google.maps.LatLng(InfoboxContent[i - 1].lat,
                                                     InfoboxContent[i - 1].lng)
                            ];
                        //}

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


                CreateMarkerWithInfoBox(InfoboxContent[i]);
                iconvar++;
                //}
            }
            //   if (IsfitBounds != false) {
            iconvar = 0;
            starticon = 0;
            
            if (InfoboxContent.length > 0) {
                var endlenth = InfoboxContent.length;
                _markerArray[0].setAnimation(google.maps.Animation.BOUNCE);
                _markerArray[endlenth - 1].setAnimation(google.maps.Animation.BOUNCE);
                _map.setCenter(new google.maps.LatLng(InfoboxContent[InfoboxContent.length - 1].lat, InfoboxContent[InfoboxContent.length - 1].lng));
            }

        } else {
            // alert("Marker list not pass !");
        }
    } catch (Error) {
        // alert("Problem in Create Route :" + Error.message);
    }
}


/*Pause Tracking*/
function Pause() {
    clearTimeout(_SetInterval);
    // ClearMap();
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


/*Create Marker With InfoBox*/
function CreateMarkerWithInfoBox(InfoboxContent, bOpenNewInfoBox, bClosePreviousInfoBox) {

    var iconname = '';
    if (starticon == 0) {
        iconname = '/flag-start.png'

    }
    if (iconvar == len-1)
    {
        iconname = '/flag-end.png'
    }
    starticon++;

    //InfoboxContent.FK_CompanyId


    // var img='http://chart.apis.google.com/chart?chst=d_map_pin_letter&chld='+A|FF0000|000000'
    _marker = new google.maps.Marker({
        map: _map,
        icon: '/App_Images/' + iconname,

        position: new google.maps.LatLng(InfoboxContent.lat, InfoboxContent.lng)
    });


    if (_infowindow && (bClosePreviousInfoBox == "1" || bClosePreviousInfoBox == "")) {
        _infowindow.close();
    }
    var livetrack = "display:none;";
    if (InfoboxContent.Status == 'P') {
        livetrack = "display:inline-block;"
    }
    _infowindow = new google.maps.InfoWindow();

    _content = "<div class='popup'><table class='info-table'><tr align='left'><td colspan='2'>"
    +"<tr><td  align='left'><b>Recorded :</b></td><td align='left'>"
    + InfoboxContent.deviceDateTime + "</td> </tr><tr><td align='left'><b>Speed :</b></td><td align='left'>"
      + InfoboxContent.speed + " Km/Hr</td> </tr><tr><td align='left'><b>Location :</b></td><td align='left'>"
        + InfoboxContent.location + "</td></tr>" //<tr><td align='left'><b>IMEI No. :</b></td><td align='left'>"
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


function setMapOnAll(map) {
    if (typeof _markerArray === 'undefined') { } else {
        for (var i = 0; i < _markerArray.length; i++) {
            _markerArray[i].setMap(map);
        }

        _markerArray = [];
    }
}