// Add a variable to track whether the function is in progress
var ischeckMaintenanceFlagInProgress = false;
var frequencycountTimer = 10000;  // 10 sec
var MaintenanceTimer = 5000;   // 5 sec
var IsClearmaintenanceInterval = false;
var frequencycount=0;
var Ispopup;
var maintenanceIntervalId;

$(document).ready(function () {
    // debuggerssssss
    // Set the initial interval and store the interval ID

    checkMaintenanceFlag();
    // Check maintenance flag every 60 seconds (adjust as needed)
     maintenanceIntervalId = setInterval(function () {
        checkMaintenanceFlag();
    }, MaintenanceTimer);

    // For example, if maintenance loop not required, clear the interval
    if (IsClearmaintenanceInterval) {
        clearInterval(maintenanceIntervalId);
    }
});



function checkMaintenanceFlag() {
    //alert(ischeckMaintenanceFlagInProgress);
    if (ischeckMaintenanceFlagInProgress==false) {
      
    // Set the flag to indicate that the function is in progress
    ischeckMaintenanceFlagInProgress = true;

    
    $.ajax({
        url: '/Account/CheckMaintenanceFlag', // Update with your controller/action
        type: 'GET',
        //data: {
        //    'ischeckMaintenanceFlagInProgress': ischeckMaintenanceFlagInProgress,
        //},
        success: function (result) {
            if (result != null && result.length > 0) {
                //debugger
                // console.log(result);
                if (result[0].ID==-1) {// popup can not close

                    // Show maintenance modal
                    var html = '';
                    if (result[0].Value != null) {
                        var ServerMaintenanceSMGArr = result[0].Value.split(";");
                    }

                    html += '<div>'
                    html += '<h2 style="text-align:center"><b>' + ServerMaintenanceSMGArr[0] + '</b></h4>'
                    // html +='<h2 style="text-align:center"><b>'+ ServerMaintenanceSMGArr[1]+'</b></h2>'
                    html += '</div>'
                    html += '<div class="modal-body" style="padding-top:0px;>'
                    html += '<div class="row">'
                    html += '<div class="col-md-12">'
                    //html += '<div class="table-responsive" style="max-height:500px">'
                    //html += '<div>'
                    html += '<h5 style="text-align:center"><b>' + ServerMaintenanceSMGArr[1] + '</b></h5>'
                    html += '<h5 style="text-align:center"><b>' + ServerMaintenanceSMGArr[2] + '</b></h5>'
                    html += '</div>'
                    html += '<div>'
                    html += '<h6 style="text-align:center">' + ServerMaintenanceSMGArr[3] + '</h6>'
                    html += '</div>'
                    html += '</div>'
                    html += '</div>'
                    //html += '</div>'
                    //html += '</div>'
                    $("#MaintenanceStatusId").html(html);
                    $('#ServerMaintenanceAnnouncement').modal('hide');
                    $('#maintenanceModal').modal('show');
                }
                else if (result[0].ID == -2) {   // popup can close
                    // frequencycount
                   // alert(result[0].IsMaintenanceAnnouncement);
                   // frequencycount = frequencycount <= 1 ? frequencycount + 1 : frequencycount;
                    // Show maintenance announcement modal
                    if (result[0].IsMaintenanceAnnouncement ==1)
                    {
                    var html = '';
                    if (result[0].Value != null) {
                        var ServerMaintenanceSMGArr = result[0].Value.split(";");
                    }
                    html += '<div>'
                    html += '<h4 style="text-align:center"><b>' + ServerMaintenanceSMGArr[0] + '</b></h4>'
                    html +='<h2 style="text-align:center"><b>'+ ServerMaintenanceSMGArr[1]+'</b></h2>'
                    html += '</div>'
                    html += '<div class="modal-body" style="padding-top:0px;">'
                    html += '<div class="row">'
                    html += '<div class="col-md-12">'
                    //html += '<div class="table-responsive" style="max-height:500px">'
                    //html += '<div>'
                    html += '<h5 style="text-align:center"><b>' + ServerMaintenanceSMGArr[2] + '</b></h5>'
                    html += '<h5 style="text-align:center"><b>' + ServerMaintenanceSMGArr[3] + '</b></h5>'
                    html += '</div>'
                    html += '<div>'
                    html += '<h6 style="text-align:center">' + ServerMaintenanceSMGArr[4] + '</h6>'
                    html += '</div>'
                    html += '</div>'
                    html += '</div>'
                    //html += '</div>'
                    //html += '</div>'
                    $("#MaintenanceAnnouncementId").html(html);
                        //alert('count:' +frequencycount);
                    
                        setTimeout(function () {
                            $('#ServerMaintenanceAnnouncement').modal('show');
                            $('#maintenanceModal').modal('hide');
                        }, frequencycountTimer);
                    
                    }
                  
                }
                else {
                    // Hide maintenance modal
                    $('#maintenanceModal').modal('hide');
                    $('#ServerMaintenanceAnnouncement').modal('hide');

                }
                // Reset the flag after the AJAX call is completed
            }
        },
        error: function (xhr, status, error) {
            // Handle errors
            console.error('Error:', status, error);

            // Reset the flag after the AJAX call is completed
           // ischeckMaintenanceFlagInProgress = false;
            //console.log(xhr);
           // console.error(xhr);
        }
    });
    ischeckMaintenanceFlagInProgress = false;
    }
}


//setTimeout(function () { if (xhr && xhr.readyState !== 4) { xhr.abort(); console.log('Request aborted due to timeout'); } }, 5000);



function SetPopupdata()
{
      $.ajax({
        url: '/Account/CheckMaintenanceFlag', // Update with your controller/action
        type: 'GET',
        //data: {
        //    'categoryid': $('#FK_CategoryId').val(),
        //},
        success: function (result) {
         // debugger
           // console.log(result);
            if (result[0].ID) {

                // Show maintenance modal
                var html = '';
                if (result[0].Value != null)
                {
                    var ServerMaintenanceSMGArr = result[0].Value.split(";");
                }

                html += '<div>'
                html += '<h4 style="text-align:center"><b>' + ServerMaintenanceSMGArr[0] + '</b></h4>'
                // html +='<h2 style="text-align:center"><b>'+ ServerMaintenanceSMGArr[1]+'</b></h2>'
                html += '</div>'
                html += '<div class="modal-body">'
                html += '<div class="row">'
                html += '<div class="col-md-12">'
                html += '<div class="table-responsive" style="max-height:500px">'
                html += '<div>'
                html += '<p class="highest-alar-title text-blue" style="text-align:center"><span>' + ServerMaintenanceSMGArr[1] + '</span></p>'
                html += '<p class="highest-alar-title text-blue" style="text-align:center"><span>' + ServerMaintenanceSMGArr[2] + '</span></p>'
                html += '</div>'
                html += '<div>'
                html += '<h6 style="text-align:center">' + ServerMaintenanceSMGArr[3] + '</h6>'
                html += '</div>'
                html += '</div>'
                html += '</div>'
                html += '</div>'
                html += '</div>'
                $("#MaintenanceStatusId").html(html);
                $('#maintenanceModal').modal('show');
            } else {
                // Hide maintenance modal
                $('#maintenanceModal').modal('hide');
            }
            // Reset the flag after the AJAX call is completed
            ischeckMaintenanceFlagInProgress = false;
        },
        error: function () {
            // Reset the flag after the AJAX call is completed
            ischeckMaintenanceFlagInProgress = false;
            console.error('Error checking maintenance flag');
        }
    });
  
}


