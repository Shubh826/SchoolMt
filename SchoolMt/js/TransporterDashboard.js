/*
   Created By  : Md. Tarique Khan
   Created Date: 2018-09-06
   Purpose     : Transporter Dashboard 
*/

/* Global Variable */
var polyline = '';
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

/*Function to load blank map*/    //USED
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

function ClearMap() {//USED    
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

/*Function to load WithMovingDirection*/
function DrawMarkerWithMovingDirection(MapControlID, Zoom, CenterLat, CenterLng, InfoboxContent, companyID, selectType) {
    try {
        
        var iVar = 0;
        if (_markerArray.length > 0) {
            markerCluster.removeMarkers(_markerArray);
        }
        
        if (!$('#ModalforTodayactivity').hasClass('in')) { //Ref: https://stackoverflow.com/questions/19506672/how-to-check-if-bootstrap-modal-is-open-so-i-can-use-jquery-validate, Added By Tarik on 18 SEPT 18 :: Alt Code => $('#myModal').is(':visible');
            ClearMap();
        }
        
        if (InfoboxContent.length != 0) {
            for (var i = 0; i <= InfoboxContent.length - 1; i++) {
                CreateMarkerWithInfoBoxWithDirection(InfoboxContent[i], companyID, selectType)
                loc = new google.maps.LatLng(InfoboxContent[i].Lat, InfoboxContent[i].Long);
                bounds.extend(loc)
            }

            markerCluster = new MarkerClusterer(_map, _markerArray, { imagePath: 'https://developers.google.com/maps/documentation/javascript/examples/markerclusterer/m' });
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
                _map.fitBounds(bounds);
            }

           // _map.setCenter(new google.maps.LatLng(InfoboxContent[InfoboxContent.length - 1].Lat, InfoboxContent[InfoboxContent.length - 1].Long))

        } else {
            // alert("Marker list not pass !");
        }
    } catch (Error) {
        // alert("Problem in Draw Marker :" + Error.message);
    }
}

/*Create Marker With InfoBox With Direction*/ //USED
function CreateMarkerWithInfoBoxWithDirection(InfoboxContent, companyID, selectType, bOpenNewInfoBox, bClosePreviousInfoBox) {

    // var img='http://chart.apis.google.com/chart?chst=d_map_pin_letter&chld='+A|FF0000|000000'
    //_marker = new google.maps.Marker({
    //    map: _map,
    //    //   icon: '../App_Images/' + InfoboxContent.Icon,

    //    position: new google.maps.LatLng(InfoboxContent.Lat, InfoboxContent.Long)
    //});
    var latLng = new google.maps.LatLng(49.47805, -123.84716);
    var homeLatLng = new google.maps.LatLng(49.47805, -123.84716);

    

    var pictureLabel = document.createElement("img");
    pictureLabel.src = "home.jpg";

    var _marker = new MarkerWithLabel({
        position: new google.maps.LatLng(InfoboxContent.Lat, InfoboxContent.Long),
        map: _map,
        draggable: false,
        raiseOnDrag: true,
        labelContent: InfoboxContent.RegistrationNo,
        //labelAnchor: new google.maps.Point(3, 30),
        //labelClass: "labels", // the CSS class for the label
        labelInBackground: true,
       
        labelAnchor: new google.maps.Point(22, 12),
        labelClass: "labels", // the CSS class for the label
        labelStyle: { opacity: 0.75 }
    });


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
    if (_infowindow && (bClosePreviousInfoBox == "1" || bClosePreviousInfoBox == "")) {
        _infowindow.close();
    }
    var livetrack = "display:none;";

    //BELOW IF ELSE ARE ACTUALLY USELESS CODE IS LEFT HERE AS THEY COULD BE USED LATER
    if (InfoboxContent.Status == 'P') {
        livetrack = "display:inline-block;"
    }
    else {
        livetrack = "display:inline-block;"
    }
    _infowindow = new google.maps.InfoWindow();
    if (companyID == '0') {
        if (selectType == "Dedicated") {
            _content = "<div class='popup'><table class='info-table'><tr align='left'><td colspan='2'><b>"
            + InfoboxContent.RegistrationNo + "</b></td><td><b>"
            + InfoboxContent.ModelName + "</b></td></tr><tr nowrap><td width='90px' align='left'><b>Driver Name:</b></td><td align='left'>"
            + InfoboxContent.ChauffeurName + "</td></tr><tr><td  align='left'><b>Driver Mob:</b></td><td align='left'>"
            + InfoboxContent.ChauffeurMob + "</td> </tr><tr><td  align='left'><b>Recorded :</b></td><td align='left'>"
            + InfoboxContent.DeviceDateTime + "</td> </tr><tr><td align='left'><b>Speed :</b></td><td align='left'>"
              + InfoboxContent.Speed + " Km/Hr</td> </tr><tr><td align='left'><b>Device No. :</b></td><td align='left'>"
                + InfoboxContent.DeviceNo + "</td> </tr>"
            + "</td></tr>"
            + '<tr><td align="left"></td><td align="left"><a style=' + livetrack + '  href="javascript:void(0)" onclick="TodayActivity(\'' + InfoboxContent.RegistrationNo + '\'' + ',\'' + InfoboxContent.DeviceNo + '\'' + ',' + InfoboxContent.FK_CompanyId + ');">Today Activity</a>'
            + "</td></tr> </table></div>";
        }else
            if (selectType == "Market") {
                _content = "<div class='popup'><table class='info-table'><tr align='left'><td colspan='2'><b>"
            + InfoboxContent.RegistrationNo + "</b></td><td><b>"
            + InfoboxContent.ModelName + "</b></td></tr><tr nowrap><td width='90px' align='left'><b>Driver Name:</b></td><td align='left'>"
            + InfoboxContent.ChauffeurName + "</td></tr><tr><td  align='left'><b>Driver Mob:</b></td><td align='left'>"
            + InfoboxContent.ChauffeurMob + "</td> </tr><tr><td  align='left'><b>Recorded :</b></td><td align='left'>"
            + InfoboxContent.DeviceDateTime + "</td> </tr><tr><td align='left'><b>Speed :</b></td><td align='left'>"
              + InfoboxContent.Speed + " Km/Hr</td> </tr><tr><td align='left'><b>Device No. :</b></td><td align='left'>"
                + InfoboxContent.DeviceNo + "</td> </tr>"
            + "</td></tr>"
            //+ '<tr><td align="left"></td><td align="left"><a style=' + livetrack + '  href="javascript:void(0)" onclick="TodayActivity(\'' + InfoboxContent.RegistrationNo + '\'' + ',\'' + InfoboxContent.DeviceNo + '\'' + ',' + InfoboxContent.FK_CompanyId + ');">Today Activity</a>'
            //+ "</td></tr>"
            +"</table></div>";
            }
    }
    else
    {
        if (selectType == "Dedicated") {
            _content = "<div class='popup' style='width:450px;'><table class='info-table' width='100%'><tr align='left'><td colspan='4'><b style='margin-bottom:5px;display:block;font-size:14px;color:#e90a0b'>" + InfoboxContent.RouteName
            + "</b></td></tr><tr align='left'><td width='90px' align='left'><b>Vehicle No.:</b></td><td  colspan='3'><b>"
           + InfoboxContent.RegistrationNo + "</b></td></tr><tr nowrap><td width='90px' align='left'><b>Driver Name:</b></td><td align='left'>"
           + InfoboxContent.ChauffeurName + "</td><td  align='left'><b>Driver Mob:</b></td><td align='left'>"
           + InfoboxContent.ChauffeurMob + "</td> </tr><tr><td  align='left'><b>Recorded :</b></td><td align='left'>"
           + InfoboxContent.DeviceDateTime + "</td>"
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
           + "</td><td align='left'><b>Speed:</b></td><td align='left'>"
           + InfoboxContent.Speed
           + " Km/Hr </td>"

           + "</td></tr>"
             + "</td></tr><tr><td align='left'><b>Alarm :</b></td><td colspan='3' align='left'>"
           + InfoboxContent.AlarmDesc
           + "</td></tr>"
               + '<tr><td align="left"></td><td align="left"><a style=' + livetrack + '  href="javascript:void(0)" onclick="TodayActivity(\'' + InfoboxContent.RegistrationNo + '\'' + ',\'' + InfoboxContent.DeviceNo + '\'' + ',' + InfoboxContent.FK_CompanyId + ');">Today Activity</a>'
             + "</td></tr>"
           + "</table></div>";
        }
        else
            if (selectType == "Market")
            {
            _content = "<div class='popup' style='width:450px;'><table class='info-table' width='100%'><tr align='left'><td colspan='4'><b style='margin-bottom:5px;display:block;font-size:14px;color:#e90a0b'>" + InfoboxContent.RouteName
           + "</b></td></tr><tr align='left'><td width='90px' align='left'><b>Vehicle No.:</b></td><td  colspan='3'><b>"
          + InfoboxContent.RegistrationNo + "</b></td></tr><tr nowrap><td width='90px' align='left'><b>Driver Name:</b></td><td align='left'>"
          + InfoboxContent.ChauffeurName + "</td><td  align='left'><b>Driver Mob:</b></td><td align='left'>"
          + InfoboxContent.ChauffeurMob + "</td> </tr><tr><td  align='left'><b>Recorded :</b></td><td align='left'>"
          + InfoboxContent.DeviceDateTime + "</td>"
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
          + "</td><td align='left'><b>Speed:</b></td><td align='left'>"
          + InfoboxContent.Speed
          + " Km/Hr </td>"

          + "</td></tr>"
            + "</td></tr><tr><td align='left'><b>Alarm :</b></td><td colspan='3' align='left'>"
          + InfoboxContent.AlarmDesc
          + "</td></tr>"
            //  + '<tr><td align="left"></td><td align="left"><a style=' + livetrack + '  href="javascript:void(0)" onclick="TodayActivity(\'' + InfoboxContent.RegistrationNo + '\'' + ',\'' + InfoboxContent.DeviceNo + '\'' + ',' + InfoboxContent.FK_CompanyId + ');">Today Activity</a>'
            //+ "</td></tr>"
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

function endsWith(str, suffix) {
    return str.indexOf(suffix, str.length - suffix.length) !== -1;
}

function TodayActivity(RegistrationNo,_DeviceNo, CompanyID) {
    //debugger
    var CurrentDatetime = new Date();
    var DD = CurrentDatetime.getDate();
    var MM = parseInt(CurrentDatetime.getMonth()) + 1;
    var YY = CurrentDatetime.getFullYear();

    if (parseInt(DD) < 10) {
        DD = "0" + DD;
    }
    if (parseInt(MM) < 10) {
        MM = "0" + MM;
    }

    var Hr = CurrentDatetime.getHours();

    var Min = CurrentDatetime.getMinutes();

    var Sec = CurrentDatetime.getSeconds();

    if (parseInt(Hr) < 10) {
        Hr = "0" + Hr;
    }
    if (parseInt(Min) < 10) {
        Min = "0" + Min;
    }
    if (parseInt(Sec) < 10) {
        Sec = "0" + Sec;
    }
    var From = DD + "-" + MM + "-" + YY + " 00:00:00";
    var To = DD + "-" + MM + "-" + YY + " " + Hr + ":" + Min + ":" + Sec;

    $.ajax({
        url: '../TransporterDashboard/GetTodayDetailByVehicle',
        //  url: "@Url.Action("GetTodayDetailByVehicle", "TransporterDashboard")",
        type: "GET",
        //dataType: "JSON",
        data: { FK_CompanyID: CompanyID, RegNo: RegistrationNo, From: From, To: To },
        cache: false,
        beforeSend: function () {
            $(".modalBgLoader, .modalLoaderCenter").show();
        },
        //complete: function () {
        //    $(".modalBgLoader, .modalLoaderCenter").hide();
        //},
        success: function (data) {
            //alert('SUCCESS')
            SetTodayActivityData(data, CompanyID, From, To, RegistrationNo, _DeviceNo);
        },
        error: function (res) {
            //alert('ERROR')
            //debugger
           
        },
        failure: function (res) {
            //alert('FAIL')           
        },
    });
}

/*CREATES ROUTE WITH START END MARKERS AND ROUTE PATH AS POLY LINE IN BLUE*/
function CreateRouteWithOutWayPointMarkers(MapControlID, Zoom, CenterLat, CenterLng, InfoboxContent, PathColor, PathOpacity, PathWeight) {
    try {

        ClearRoute()

        if (InfoboxContent.length != 0) {
            var _vehTrackCoordinates = [];
            for (var i = 0; i <= InfoboxContent.length - 1; i++) {

                _vehTrackCoordinates.push({ lat: InfoboxContent[i].Lat, lng: InfoboxContent[i].Long });


                //FIRST MARKER: START POINT
                if (i == 0) {
                    _marker = new google.maps.Marker({
                        map: _map,
                        icon: '/App_Images/' + 'flag-start.png',

                        position: new google.maps.LatLng(InfoboxContent[i].Lat, InfoboxContent[i].Long)
                    });
                    _markerArray.push(_marker);
                }

                //LAST MARKER: END POINT
                if (i == InfoboxContent.length - 1) {
                    //debugger
                    _marker = new google.maps.Marker({
                        map: _map,
                        icon: '/App_Images/' + 'flag-end.png',

                        position: new google.maps.LatLng(InfoboxContent[i].Lat, InfoboxContent[i].Long)
                    });
                    _markerArray.push(_marker);
                }

                var PolyPath = new google.maps.Polyline({
                    path: _vehTrackCoordinates,
                    geodesic: true,
                    strokeColor: '#56baf8',
                    strokeOpacity: 1.0,
                    strokeWeight: 5,
                });

                PolyPath.setMap(_map);
            }

            polyline = new google.maps.Polyline({
                path: google.maps.geometry.encoding.decodePath(_RoutePath),
                strokeColor: '#56baf8',
                strokeOpacity: 1.0,
                strokeWeight: 5,
                map: _map
            });
            polyline.setMap(_map);

            if (InfoboxContent.length > 0) {
                var endlenth = InfoboxContent.length;
                _markerArray[0].setAnimation(google.maps.Animation.BOUNCE);
                _markerArray[_markerArray.length - 1].setAnimation(google.maps.Animation.BOUNCE);
                _map.setCenter(new google.maps.LatLng(InfoboxContent[InfoboxContent.length - 1].Lat, InfoboxContent[InfoboxContent.length - 1].Long));
            }

        } else {
            // alert("Marker list not pass !");
        }
    } catch (Error) {

        // alert("Problem in Create Route :" + Error.message);
    }
}


/*************************CUSTOM MARKERS CODES START*********************************/

//Simple Code Just With Title And Custom Image
function AddCustomeMarker(latLong, MarkerTitle, imagePath) {     
    var marker = new google.maps.Marker({
        position: latLong,
        title: MarkerTitle,
        draggable: false,
        map: _map,
        icon: imagePath
    });   
}

//Marker With Infobox with customized content
function DrawMarkerWithCustomContent(InfoboxContent) {
    try {
        //debugger
      
        //if (_markerArray.length > 0) {
        //    markerCluster.removeMarkers(_markerArray);
        //}        

        if (InfoboxContent.length != 0) {
            for (var i = 0; i <= InfoboxContent.length - 1; i++) {
                CreateMarkerWithCustomInfoBoxContent(InfoboxContent[i])
                //loc = new google.maps.LatLng(InfoboxContent[i].Lat, InfoboxContent[i].Long);
                //bounds.extend(loc)
            }

            //markerCluster = new MarkerClusterer(_map, _markerArray, { imagePath: 'https://developers.google.com/maps/documentation/javascript/examples/markerclusterer/m' });

            //_map.fitBounds(bounds);

        } else {
            // alert("Marker list not pass !");
        }
    } catch (Error) {
        // alert("Problem in Draw Marker :" + Error.message);
    }
}

/*Creates Marker With InfoBox With Custom Content*/ //USED
function CreateMarkerWithCustomInfoBoxContent(InfoboxContent) {
    //debugger
    // var img='http://chart.apis.google.com/chart?chst=d_map_pin_letter&chld='+A|FF0000|000000'
    _marker = new google.maps.Marker({
        map: _map,
        position: new google.maps.LatLng(InfoboxContent._lat, InfoboxContent._lng)
    });
   
    _marker.setIcon('/App_Images/Stopping.png');   
    
    _infowindow.close();   
    
    _infowindow = new google.maps.InfoWindow();
    _content = "<div class='popup'><table class='info-table'>"
      +"  <tr nowrap><td width='200px' align='left'><b>Ignition OFF Time:</b></td><td align='left'>"
    + InfoboxContent._From + "</td></tr><tr><td  align='left'><b>Ignition ON Time:</b></td><td align='left'>"
    + InfoboxContent._To + "</td> </tr><tr><td  align='left'><b>Ignition OFF Duration (Mins):</b></td><td align='left'>"
    + InfoboxContent._TotalIgnitionOffMinutes + "</td> </tr>"
    + "</td></tr>"
    +"</table></div>";
    google.maps.event.addListener(_marker, 'click', (function (_marker, _content) {
        return function () {
            _infowindow.setContent(_content);
            _infowindow.open(_map, _marker);
        }
    })(_marker, _content));

    _infowindow.setContent(_content);
    //_infowindow.open(_map, _marker);
   
    //_markerArray.push(_marker);
}

/*************************CUSTOM MARKERS CODES END*********************************/