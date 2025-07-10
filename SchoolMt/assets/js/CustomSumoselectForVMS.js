/*!
 * jquery.sumoselect - v3.0.2
 * http://hemantnegi.github.io/jquery.sumoselect
 * 2014-04-08
 *
 * Copyright 2015 Hemant Negi
 * Email : hemant.frnz@gmail.com
 * Compressor http://refresh-sf.com/
 */

(function ($) {
    //  debugger;
    'namespace sumo';
    $.fn.SumoSelect = function (options) {
        // This is the easiest way to have default options.
        var settings = $.extend({


            placeholder: 'Select Here',   // Dont change it here.
            /*****START::Change By Prince*********************/
            // captionFormatAllSelected: '{0} Selected !', // format of caption text when all elements are selected. set null to use captionFormat. It will not work if there are disabled elements in select.::Prev Code
            captionFormatAllSelected: 'All', // format of caption text when all elements are selected. set null to use captionFormat. It will not work if there are disabled elements in select.::New Code By Prince
            /*****END::Change By Prince*********************/
            csvDispCount: 3,              // display no. of items in multiselect. 0 to display all.
            captionFormat: '{0} Selected', // format of caption text. you can set your locale.
            floatWidth: 300,              // Screen width of device at which the list is rendered in floating popup fashion.
            forceCustomRendering: false,  // force the custom modal on all devices below floatWidth resolution.
            nativeOnDevice: ['Android', 'BlackBerry', 'iPhone', 'iPad', 'iPod', 'Opera Mini', 'IEMobile', 'Silk'], //
            outputAsCSV: false,           // true to POST data as csv ( false for Html control array ie. default select )
            csvSepChar: ',',              // separation char in csv mode
            okCancelInMulti: false,       // display ok cancel buttons in desktop mode multiselect also.
            triggerChangeCombined: true,  // im multi select mode wether to trigger change event on individual selection or combined selection.
            selectAll: true,             // to display select all button in multiselect mode.|| also select all will not be available on mobile devices.
            search: true,                // to display input for filtering content. selectAlltext will be input text placeholder
            searchText: 'Search...',      // placeholder for search input
            noMatch: 'No matches for "{0}"',
            prefix: '',                   // some prefix usually the field name. eg. '<b>Hello</b>'
            locale: ['OK', 'Cancel', 'All'],  // all text that is used. don't change the index.
            up: false, // set true to open upside.
            Sumofor: ''
        }, options);


        var ret = this.each(function () {
            var selObj = this; // the original select object.
            if (this.sumo || !$(this).is('select')) return; //already initialized

            this.sumo = {
                E: $(selObj),   //the jquery object of original select element.
                is_multi: $(selObj).attr('multiple'), //if its a multiple select
                select: '',
                caption: '',
                placeholder: '',
                optDiv: '',
                CaptionCont: '',
                ul: '',
                is_floating: false,
                is_opened: false,
                //backdrop: '',
                mob: false, // if to open device default select
                Pstate: [],

                createElems: function () {
                    /// debugger;
                    var O = this;
                    O.E.wrap('<div class="SumoSelect" tabindex="0">');
                    O.select = O.E.parent();
                    O.caption = $('<span>');
                    if (settings.Sumofor == 'Vehicles' || settings.Sumofor == 'Vehicle') {
                        O.CaptionCont = $('<p id="VehicleCaptionCont" class="CaptionCont"><label><i></i></label></p>').addClass('SelectBox').attr('style', O.E.attr('style')).prepend(O.caption);

                    }
                    else if (settings.Sumofor == 'Customer') {
                        O.CaptionCont = $('<p id="CustomerCaptionCont" class="CaptionCont"><label><i></i></label></p>').addClass('SelectBox').attr('style', O.E.attr('style')).prepend(O.caption);

                    }
                    else if (settings.Sumofor == 'AccountIdAllArr') {
                        O.CaptionCont = $('<p id="AccountIdAllArrCaptionCont" class="CaptionCont"><label><i></i></label></p>').addClass('SelectBox').attr('style', O.E.attr('style')).prepend(O.caption);

                    }
                    else if (settings.Sumofor == 'VehicleType') {
                        O.CaptionCont = $('<p id="VehicleTypeCaptionCont" class="CaptionCont"><label><i></i></label></p>').addClass('SelectBox').attr('style', O.E.attr('style')).prepend(O.caption);
                    }


                    else if (settings.Sumofor == 'DeviceType') {
                        O.CaptionCont = $('<p id="DeviceTypeCaptionCont" class="CaptionCont"><label><i></i></label></p>').addClass('SelectBox').attr('style', O.E.attr('style')).prepend(O.caption);
                    }
                    else if (settings.Sumofor == 'GetStatusType') {
                        O.CaptionCont = $('<p id="GetStatusTypeCaptionCont" class="CaptionCont"><label><i></i></label></p>').addClass('SelectBox').attr('style', O.E.attr('style')).prepend(O.caption);
                    }


                    else if (settings.Sumofor == 'GetDeviceID') {
                        O.CaptionCont = $('<p id="GetDeviceIDCaptionCont" class="CaptionCont"><label><i></i></label></p>').addClass('SelectBox').attr('style', O.E.attr('style')).prepend(O.caption);
                    }


                    else if (settings.Sumofor == 'GetDeviceIMEIID') {
                        O.CaptionCont = $('<p id="GetDeviceIMEIIDCaptionCont" class="CaptionCont"><label><i></i></label></p>').addClass('SelectBox').attr('style', O.E.attr('style')).prepend(O.caption);
                    }

                    else if (settings.Sumofor == 'GetSimMobileID') {
                        O.CaptionCont = $('<p id="GetSimMobileIDCaptionCont" class="CaptionCont"><label><i></i></label></p>').addClass('SelectBox').attr('style', O.E.attr('style')).prepend(O.caption);
                    }

                    else if (settings.Sumofor == 'GetSimSNID') {
                        O.CaptionCont = $('<p id="GetSimSNIDCaptionCont" class="CaptionCont"><label><i></i></label></p>').addClass('SelectBox').attr('style', O.E.attr('style')).prepend(O.caption);
                    }
                    else if (settings.Sumofor == 'SearchValue') {
                        O.CaptionCont = $('<p id="SearchValueCaptionCont" class="CaptionCont"><label><i></i></label></p>').addClass('SelectBox').attr('style', O.E.attr('style')).prepend(O.caption);
                    }
                    else if (settings.Sumofor == 'VehicleDetail') {
                        O.CaptionCont = $('<p id="VehicleDetailCaptionCont" class="CaptionCont"><label><i></i></label></p>').addClass('SelectBox').attr('style', O.E.attr('style')).prepend(O.caption);
                    }
                    else if (settings.Sumofor == 'LicenseKey') {
                        O.CaptionCont = $('<p id="LicenseKeyCaptionCont" class="CaptionCont"><label><i></i></label></p>').addClass('SelectBox').attr('style', O.E.attr('style')).prepend(O.caption);
                    }
                    else if (settings.Sumofor == 'LicenseValidty') {
                        O.CaptionCont = $('<p id="LicenseValidtyCaptionCont" class="CaptionCont"><label><i></i></label></p>').addClass('SelectBox').attr('style', O.E.attr('style')).prepend(O.caption);
                    }
                    else if (settings.Sumofor == 'Subscriber') {
                        O.CaptionCont = $('<p id="SubscriberCaptionCont" class="CaptionCont"><label><i></i></label></p>').addClass('SelectBox').attr('style', O.E.attr('style')).prepend(O.caption);
                    }
                    else if (settings.Sumofor == 'AdditionalAcc') {
                        O.CaptionCont = $('<p id="AdditionalAccCaptionCont" class="CaptionCont"><label><i></i></label></p>').addClass('SelectBox').attr('style', O.E.attr('style')).prepend(O.caption);
                    }
                    else if (settings.Sumofor == 'MappingStatus') {
                        O.CaptionCont = $('<p id="MappingStatusCaptionCont" class="CaptionCont"><label><i></i></label></p>').addClass('SelectBox').attr('style', O.E.attr('style')).prepend(O.caption);
                    }
                    else if (settings.Sumofor == 'SubscriptionStatus') {
                        O.CaptionCont = $('<p id="SubscriptionStatusCaptionCont" class="CaptionCont"><label><i></i></label></p>').addClass('SelectBox').attr('style', O.E.attr('style')).prepend(O.caption);
                    }
                    else if (settings.Sumofor == 'LicenseStatus') {
                        O.CaptionCont = $('<p id="LicenseStatusCaptionCont" class="CaptionCont"><label><i></i></label></p>').addClass('SelectBox').attr('style', O.E.attr('style')).prepend(O.caption);
                    }
                    else {
                        O.CaptionCont = $('<p id="CaptionCont" class="CaptionCont"><label><i></i></label></p>').addClass('SelectBox').attr('style', O.E.attr('style')).prepend(O.caption);

                    }
                    O.select.append(O.CaptionCont);

                    // default turn off if no multiselect
                    if (!O.is_multi) settings.okCancelInMulti = false

                    if (O.E.attr('disabled'))
                        O.select.addClass('disabled').removeAttr('tabindex');

                    //if output as csv and is a multiselect.
                    if (settings.outputAsCSV && O.is_multi && O.E.attr('name')) {
                        //create a hidden field to store csv value.
                        O.select.append($('<input class="HEMANT123" type="hidden" />').attr('name', O.E.attr('name')).val(O.getSelStr()));

                        // so it can not post the original select.
                        O.E.removeAttr('name');
                    }

                    //break for mobile rendring.. if forceCustomRendering is false
                    if (O.isMobile() && !settings.forceCustomRendering) {
                       // O.setNativeMobile();
                       // return;
                    }

                    // if there is a name attr in select add a class to container div
                    if (O.E.attr('name')) O.select.addClass('sumo_' + O.E.attr('name'))

                    //hide original select
                    O.E.addClass('SumoUnder').attr('tabindex', '-1');

                    //## Creating the list...
                    if (settings.Sumofor == 'Vehicles' || settings.Sumofor == 'Vehicle') {
                        O.optDiv = $('<div id="optWrapperVehicle" class="optWrapper ' + (settings.up ? 'up' : '') + '">');
                    }
                    else if (settings.Sumofor == 'Customer') {
                        O.optDiv = $('<div id="optWrapperCustomer" class="optWrapper ' + (settings.up ? 'up' : '') + '">');
                    }
                    else if (settings.Sumofor == 'AccountIdAllArr') {
                        O.optDiv = $('<div id="optWrapperAccountIdAllArr" class="optWrapper ' + (settings.up ? 'up' : '') + '">');
                    }
                    else if (settings.Sumofor == 'VehicleType') {
                        O.optDiv = $('<div id="optWrapperVehicleType" class="optWrapper ' + (settings.up ? 'up' : '') + '">');
                    }

                    else if (settings.Sumofor == 'DeviceType') {
                        O.optDiv = $('<div id="optWrapperDeviceType" class="optWrapper ' + (settings.up ? 'up' : '') + '">');
                    }

                    else if (settings.Sumofor == 'GetStatusType') {
                        O.optDiv = $('<div id="optWrapperGetStatusType" class="optWrapper ' + (settings.up ? 'up' : '') + '">');
                    }

                    else if (settings.Sumofor == 'GetDeviceID') {
                        O.optDiv = $('<div id="optWrapperGetDeviceID" class="optWrapper ' + (settings.up ? 'up' : '') + '">');
                    }

                    else if (settings.Sumofor == 'GetDeviceIMEIID') {
                        O.optDiv = $('<div id="optWrapperGetDeviceIMEIID" class="optWrapper ' + (settings.up ? 'up' : '') + '">');
                    }

                    else if (settings.Sumofor == 'GetSimMobileID') {
                        O.optDiv = $('<div id="optWrapperGetSimMobileID" class="optWrapper ' + (settings.up ? 'up' : '') + '">');
                    }
                    else if (settings.Sumofor == 'GetSimSNID') {
                        O.optDiv = $('<div id="optWrapperGetSimSNID" class="optWrapper ' + (settings.up ? 'up' : '') + '">');
                    }


                    else if (settings.Sumofor == 'ExpensesCategory') {
                        O.optDiv = $('<div id="optWrapperExpensesCategory" class="optWrapper ' + (settings.up ? 'up' : '') + '">');
                    }
                    else if (settings.Sumofor == 'ExpensesType') {
                        O.optDiv = $('<div id="optWrapperExpensesType" class="optWrapper ' + (settings.up ? 'up' : '') + '">');
                    }
                    else if (settings.Sumofor == 'SearchValue') {
                        O.optDiv = $('<div id="optWrapperSearchValue" class="optWrapper ' + (settings.up ? 'up' : '') + '">');
                    }
                    else if (settings.Sumofor == 'VehicleDetail') {
                        O.optDiv = $('<div id="optWrapperVehicleDetail" class="optWrapper ' + (settings.up ? 'up' : '') + '">');
                    }
                    else if (settings.Sumofor == 'LicenseKey') {
                        O.optDiv = $('<div id="optWrapperLicenseKey" class="optWrapper ' + (settings.up ? 'up' : '') + '">');
                    }
                    else if (settings.Sumofor == 'LicenseValidty') {
                        O.optDiv = $('<div id="optWrapperLicenseValidty" class="optWrapper ' + (settings.up ? 'up' : '') + '">');
                    }
                    else if (settings.Sumofor == 'Subscriber') {
                        O.optDiv = $('<div id="optWrapperSubscriber" class="optWrapper ' + (settings.up ? 'up' : '') + '">');
                    }
                    else if (settings.Sumofor == 'AdditionalAcc') {
                        O.optDiv = $('<div id="optWrapperAdditionalAcc" class="optWrapper ' + (settings.up ? 'up' : '') + '">');
                    }
                    else if (settings.Sumofor == 'MappingStatus') {
                        O.optDiv = $('<div id="optWrapperMappingStatus" class="optWrapper ' + (settings.up ? 'up' : '') + '">');
                    }
                    else if (settings.Sumofor == 'SubscriptionStatus') {
                        O.optDiv = $('<div id="optWrapperSubscriptionStatus" class="optWrapper ' + (settings.up ? 'up' : '') + '">');
                    }
                    else if (settings.Sumofor == 'LicenseStatus') {
                        O.optDiv = $('<div id="optWrapperLicenseStatus" class="optWrapper ' + (settings.up ? 'up' : '') + '">');
                    }
                    else {
                        O.optDiv = $('<div id="optWrapper" class="optWrapper ' + (settings.up ? 'up' : '') + '">');
                    }


                    //branch for floating list in low res devices.
                    O.floatingList();

                    //Creating the markup for the available options
                    O.ul = $('<ul class="options">');
                    O.optDiv.append(O.ul);

                    // Select all functionality
                    if (settings.selectAll) O.SelAll();

                    // search functionality
                    if (settings.search) O.Search();

                    O.ul.append(O.prepItems(O.E.children()));

                    //if multiple then add the class multiple and add OK / CANCEL button
                    if (O.is_multi) O.multiSelelect();

                    O.select.append(O.optDiv);
                    O.basicEvents();
                    O.selAllState();



                    if (O.selAll.hasClass('selected')) {
                        O.selAll.removeClass('selected');
                    }
                },


                prepItems: function (opts, d, attribute) {
                    /////  debugger;
                    var lis = [], O = this;
                    $(opts).each(function (i, opt) {       // parsing options to li
                        opt = $(opt);
                        lis.push(opt.is('optgroup') ?
                            $('<li class="group ' + (opt[0].disabled ? 'disabled' : '') + '"><label>' + opt.attr('label') + '</label><ul></ul><li>')
                            .find('ul')
                            .append(O.prepItems(opt.children(), opt[0].disabled))
                            .end()
                            :
                            O.createLi(opt, d, attribute)
                            );
                    });
                    return lis;
                },

                //## Creates a LI element from a given option and binds events to it
                //## returns the jquery instance of li (not inserted in dom)

                createLi: function (opt, d, attribute) {
                    /// debugger;
                    var O = this;
                    attribute = (attribute == undefined) ? "" : attribute
                    if (!opt.attr('value')) opt.attr('value', opt.val());

                    if (!opt.attr('data-id')) opt.attr('data-id', attribute);

                    // todo: remove this data val 
                    //li = $('<li class=opt ' + opt.attr('data-id', attribute) + '><label>' + opt.text() + '</label></li>');//.data('val',opt.val());
                    li = $('<li class=opt ' + attribute + '><label>' + opt.text() + '</label></li>');//.data('val',opt.val());

                    li.data('opt', opt);    // store a direct reference to option.
                    // opt.attr("data-id", attr);// custom attribute
                    opt.data('li', li);    // store a direct reference to list item.

                    if (O.is_multi) li.prepend('<span><i></i></span>');

                    if (opt[0].disabled || d)
                        li = li.addClass('disabled');

                    O.onOptClick(li);

                    if (opt[0].selected)
                        li.addClass('selected');

                    if (opt.attr('class'))
                        li.addClass(opt.attr('class'));

                    if (opt.attr('data-id'))
                        li.addClass(opt.attr('data-id'));

                    return li;
                },

                //## Returns the selected items as string in a Multiselect.
                getSelStr: function () {
                    ///  debugger;
                    // get the pre selected items.
                    sopt = [];
                    this.E.find('option:selected').each(function () { sopt.push($(this).val()); });
                    return sopt.join(settings.csvSepChar);
                },

                //## THOSE OK/CANCEL BUTTONS ON MULTIPLE SELECT.
                multiSelelect: function () {
                    //   debugger;
                    var O = this;
                    O.optDiv.addClass('multiple');
                    O.okbtn = $('<p class="btnOk">' + settings.locale[0] + '</p>').click(function () {
                        //  debugger;
                        //if combined change event is set.
                        if (settings.triggerChangeCombined) {

                            //check for a change in the selection.
                            changed = false;
                            if (O.E.find('option:selected').length != O.Pstate.length) {
                                changed = true;
                            }
                            else {
                                O.E.find('option').each(function (i, e) {
                                    if (e.selected && O.Pstate.indexOf(i) < 0) changed = true;
                                });
                            }
                            if (changed) {
                                O.callChange();
                                O.setText();
                            }
                        }
                        O.hideOpts();
                    });
                    O.cancelBtn = $('<p class="btnCancel">' + settings.locale[1] + '</p>').click(function () {
                        O._cnbtn();
                        O.hideOpts();
                    });
                    O.optDiv.append($('<div class="MultiControls">').append(O.okbtn).append(O.cancelBtn));
                },

                //_cnbtn: function () {
                //    debugger
                //    var O = this;
                //    //remove all selections
                //    O.E.find('option:selected').each(function () { this.selected = false; });
                //    O.optDiv.find('li.selected').removeClass('selected')

                //    //restore selections from saved state.
                //    for (var i = 0; i < O.Pstate.length; i++) {
                //        O.E.find('option')[O.Pstate[i]].selected = true;
                //        O.ul.find('li.opt').eq(O.Pstate[i]).addClass('selected');
                //    }
                //    O.selAllState();
                //},

                /*   START :: Add By Hritik Ghosh :: Date : 02/07/2024   */

                _cnbtn: function () {
                    var O = this;
                    // Cache the selectors to avoid multiple DOM queries
                    var $options = O.E.find('option');
                    var $selectedOptions = O.E.find('option:selected');
                    var $optDivSelected = O.optDiv.find('li.selected');
                    var $ulOptions = O.ul.find('li.opt');

                    // Remove all selections
                    $selectedOptions.each(function () { this.selected = false; });
                    $optDivSelected.removeClass('selected');

                    // Restore selections from saved state
                    var PstateLength = O.Pstate.length;
                    for (var i = 0; i < PstateLength; i++) {
                        var index = O.Pstate[i];
                        $options[index].selected = true;
                        $ulOptions.eq(index).addClass('selected');
                    }
                    // Update the select all state
                    O.selAllState();
                },
                /*   END :: Add By Hritik Ghosh :: Date : 02/07/2024   */

                SelAll: function () {
                    //  debugger;
                    var O = this;
                    O.placeholder = '';
                    if (!O.is_multi) return;

                    var Pid = settings.Sumofor == "VehicleDetail" ? "pVehicleDetail" : settings.Sumofor == "Customer" ? "pCustomer" : (settings.Sumofor == "Vehicles" || settings.Sumofor == "Vehicle") ? "pVehicle" : settings.Sumofor == "VehicleType" ? "PVehicleType" : settings.Sumofor == "ExpensesCategory" ? "pExpensesCategory" : settings.Sumofor == "ExpensesType" ? "pExpensesType" : settings.Sumofor == "DeviceType" ? "pDeviceType" : settings.Sumofor == "GetStatusType" ? "pGetStatusType" : settings.Sumofor == "GetDeviceID" ? "pGetDeviceID" : settings.Sumofor == "GetDeviceIMEIID" ? "" : settings.Sumofor == "pGetDeviceIMEIID" ? "" : settings.Sumofor == "GetSimMobileID" ? "pGetSimMobileID" : settings.Sumofor == "GetSimSNID" ? "pGetSimSNID" : settings.Sumofor == "SearchValue" ? "pSearchValue" : settings.Sumofor == "AccountIdAllArr" ? "pAccountIdAllArr" : settings.Sumofor == "LicenseKey" ? "pLicenseKey" : settings.Sumofor == "LicenseValidty" ? "pLicenseValidty" : settings.Sumofor == "Subscriber" ? "pSubscriber" : settings.Sumofor == "AdditionalAcc" ? "pAdditionalAcc" : settings.Sumofor == "MappingStatus" ? "pMappingStatus" : settings.Sumofor == "SubscriptionStatus" ? "pSubscriptionStatus" : settings.Sumofor == "LicenseStatus" ? "pLicenseStatus" : "Pid";    

               
                    Pid = '<p class="select-all" id="' + Pid + '"' + '><span><i></i></span><label>' + settings.locale[2] + '</label></p>';
                    O.selAll = $(Pid);//dynamic
                    //  O.selAll = $('<p class="select-all"><span><i></i></span><label>' + settings.locale[2] + '</label></p>');//static

                    O.selAll.on('click', function () {
                        //O.toggSelAll(!); 
                        //debugger;
                        /*****START:: Find Wrapper,Paragraph and Sumo Dropdown Id***********/
                        var paragraphId = settings.Sumofor == "VehicleDetail" ? "pVehicleDetail" : settings.Sumofor == "Customer" ? "pCustomer" : (settings.Sumofor == "Vehicles" || settings.Sumofor == "Vehicle") ? "pVehicle" : settings.Sumofor == "VehicleType" ? "pVehicleType" : settings.Sumofor == "ExpensesCategory" ? "pExpensesCategory" : settings.Sumofor == "ExpensesType" ? "pExpensesType" : settings.Sumofor == "DeviceType" ? "pDeviceType" : settings.Sumofor == "GetStatusType" ? "pGetStatusType" : settings.Sumofor == "GetDeviceID" ? "pGetDeviceID" : settings.Sumofor == "GetDeviceIMEIID" ? "pGetDeviceIMEIID" : settings.Sumofor == "GetSimMobileID" ? "pGetSimMobileID" : settings.Sumofor == "GetSimSNID" ? "pGetSimSNID" : settings.Sumofor == "SearchValue" ? "pSearchValue" : settings.Sumofor == "AccountIdAllArr" ? "pAccountIdAllArr" : settings.Sumofor == "LicenseKey" ? "pLicenseKey" : settings.Sumofor == "LicenseValidty" ? "pLicenseValidty" : settings.Sumofor == "Subscriber" ? "pSubscriber" : settings.Sumofor == "AdditionalAcc" ? "pAdditionalAcc" : settings.Sumofor == "MappingStatus" ? "pMappingStatus" : settings.Sumofor == "SubscriptionStatus" ? "pSubscriptionStatus" : settings.Sumofor == "LicenseStatus" ? "pLicenseStatus" : "Pid";   
                        paragraphId = "#" + paragraphId;

                        var optWrapperId = settings.Sumofor == "VehicleDetail" ? "optWrapperVehicleDetail" : settings.Sumofor == "Customer" ? "optWrapperCustomer" : (settings.Sumofor == "Vehicles" || settings.Sumofor == "Vehicle") ? "optWrapperVehicle" : settings.Sumofor == "VehicleType" ? "optWrapperVehicleType" : settings.Sumofor == "ExpensesCategory" ? "optWrapperExpensesCategory" : settings.Sumofor == "ExpensesType" ? "optWrapperExpensesType" : settings.Sumofor == "DeviceType" ? "optWrapperDeviceType" : settings.Sumofor == "GetStatusType" ? "optWrapperGetStatusType" : settings.Sumofor == "GetDeviceID" ? "optWrapperGetDeviceID" : settings.Sumofor == "GetDeviceIMEIID" ? "optWrapperGetDeviceIMEIID" : settings.Sumofor == "GetSimMobileID" ? "optWrapperGetSimMobileID" : settings.Sumofor == "GetSimSNID" ? "optWrapperGetSimSNID" : settings.Sumofor == "SearchValue" ? "optWrapperSearchValue" : settings.Sumofor == "AccountIdAllArr" ? "optWrapperAccountIdAllArr" : settings.Sumofor == "LicenseKey" ? "optWrapperLicenseKey" : settings.Sumofor == "LicenseValidty" ? "optWrapperLicenseValidty" : settings.Sumofor == "Subscriber" ? "optWrapperSubscriber" : settings.Sumofor == "AdditionalAcc" ? "optWrapperAdditionalAcc" : settings.Sumofor == "MappingStatus" ? "optWrapperMappingStatus" : settings.Sumofor == "SubscriptionStatus" ? "optWrapperSubscriptionStatus" : settings.Sumofor == "LicenseStatus" ? "optWrapperLicenseStatus" : "optWrapper";    
                        optWrapperId = optWrapperId == "" ? "#optWrapper > ul > li.opt" : "#" + optWrapperId + "> ul > li.opt";

                        var div = $(this).closest("div").parent().parent()[0].className;
                        var DropdownId = div.replace("form-group", "").replace("Div", "").replace("row", "").trim();
                        // className	"optWrapper okCancelInMulti multiple"	

                        // var child = $(this).closest("div")[0].className;

                        DropdownId = "#" + DropdownId + "> option";
                        //EX:-className = "form-group FK_CustomerIdDiv"

                        /*****END:: Find Wrapper,Paragraph and Sumo Dropdown Id***********/

                        var Alloption = $(optWrapperId).length;
                        var Visibleoption = $(optWrapperId).not('.hidden').length;

                        if (O.selAll.hasClass('partial selected')) {

                            // $(".select-all").removeClass("partial selected");//static
                            $(paragraphId).removeClass("partial selected");//dynamic :: make paragraph id with select all option
                        }
                        if (O.selAll.hasClass('partial')) {

                            // $(".select-all").removeClass("partial selected");//static
                            $(paragraphId).removeClass("partial");//dynamic :: make paragraph id with select all option
                        }
                        O.selAll.toggleClass('selected');

                        if (Visibleoption > 500) {
                            Alloption = Visibleoption;
                        }

                        if (Alloption == Visibleoption) {
                            if (O.selAll.hasClass('selected')) { /****if select all then code excecute this block********/

                                // $(".select-all").addClass("selected");static
                                $(paragraphId).addClass("selected");//dynamic :: make paragraph id with select all option

                                //  $("#FK_CustomerId > option").attr("selected", "selected"); static
                                $(DropdownId).attr("selected", "selected");//dynamic

                                //  $(".optWrapper > ul > li.opt").addClass("selected");//static
                                $(optWrapperId).addClass("selected");//dynamic

                                O.E.find('option').prop("selected", true);//when select All then Dropdown All option Selected
                                // var length = O.E.find('option:selected').length;//lengh of selected dropdown
                            }
                            else /****if not select all then code excecute this block********/ {
                                // $(".optWrapper > ul > li.opt").removeClass('selected');// static
                                $(optWrapperId).removeClass('selected');//dynamic

                                //  $(".select-all").removeClass("partial");//static
                                // $(".select-all").removeClass("selected");//static
                                $(paragraphId).removeClass("partial");//dynamic
                                $(paragraphId).removeClass("selected");//dynamic

                                //$("#FK_CustomerId > option").removeAttr("selected");
                                $(DropdownId).removeAttr("selected");//dynamic

                                //$("#FK_CustomerId > option").prop("selected", false);
                                $(DropdownId).prop("selected", false);//dynamic

                                O.E.find(':selected').prop("selected", false);
                                // var lengths = O.E.find('option:selected').length;
                                //   var list= O.optDiv.find('li.opt');

                            }
                        }
                        else {
                            O.optDiv.prepend(O.selAll);
                            O.optDiv.find('li.opt').not('.hidden').each(function (ix, e) {
                                /// debugger;
                                e = $(e);
                                if (O.selAll.hasClass('selected')) {
                                    if (!e.hasClass('selected')) e.trigger('click');
                                }
                                else
                                    if (e.hasClass('selected')) e.trigger('click');
                            });

                        }
                        /**START::Prev Code::Commented By Prince becasuse Loading isssue in large data to select/unselect*/
                        //  O.optDiv.prepend(O.selAll);

                        //O.optDiv.find('li.opt').not('.hidden').each(function (ix, e) {
                        //    debugger;
                        //    e = $(e);
                        //    if (O.selAll.hasClass('selected')) {
                        //        if (!e.hasClass('selected')) e.trigger('click');
                        //    }
                        //    else
                        //        if (e.hasClass('selected')) e.trigger('click');
                        //});
                        /**END::Prev Code::Commented By Prince becasuse Loading isssue in large data to select/unselect*/
                    });

                    if (O.selAll.hasClass('selected')) {
                        O.selAll.removeClass('selected');
                    }
                    if (O.selAll.hasClass('partial selected')) {
                        O.selAll.removeClass('partial selected');
                    }
                    if (O.selAll.hasClass('partial')) {
                        O.selAll.removeClass('partial');
                    }


                    //   O.optDiv.prepend(O.selAll);
                    O.optDiv.prepend(O.selAll);
                },

                // search module (can be removed if not required.)
                Search: function () {
                    ///debugger;
                    var O = this,
                        cc = O.CaptionCont.addClass('search'),
                        P = $('<p class="no-match">');

                    O.ftxt = $('<input type="text" class="search-txt" value="" placeholder="' + settings.searchText + '">')
                        .on('click', function (e) {
                            e.stopPropagation();
                        });
                    cc.append(O.ftxt);
                    O.optDiv.children('ul').after(P);

                    O.ftxt.on('keyup.sumo', function () {
                        ///debugger;
                        var hid = O.optDiv.find('ul.options li.opt').each(function (ix, e) {
                            e = $(e);
                            if (e.text().toLowerCase().indexOf(O.ftxt.val().toLowerCase()) > -1)
                                e.removeClass('hidden');
                            else
                                e.addClass('hidden');
                        }).not('.hidden');

                        P.html(settings.noMatch.replace(/\{0\}/g, O.ftxt.val())).toggle(!hid.length);

                        O.selAllState();
                    });
                },

                selAllState: function () {
                    var O = this;
                    if (settings.selectAll) {
                        var sc = 0, vc = 0;
                        O.optDiv.find('li.opt').not('.hidden').each(function (ix, e) {
                            if ($(e).hasClass('selected')) sc++;
                            if (!$(e).hasClass('disabled')) vc++;
                        });
                        //select all checkbox state change.
                        if (sc == vc) O.selAll.removeClass('partial').addClass('selected');
                        else if (sc == 0) O.selAll.removeClass('selected partial');
                        else O.selAll.addClass('partial')//.removeClass('selected');
                    }
                },

                showOpts: function () {
                    var O = this;
                    if (O.E.attr('disabled')) return; // if select is disabled then retrun
                    O.is_opened = true;
                    O.select.addClass('open');

                    if (O.ftxt) O.ftxt.focus();
                    else O.select.focus();

                    // hide options on click outside.
                    $(document).on('click.sumo', function (e) {
                         //   debugger;
                        //  alert('outside : ' + O.select.has(e.target).length);

                        /*****START:: Find Caption Id***********/

                        var CaptionCountId = settings.Sumofor == "VehicleDetail" ? "VehicleDetailCaptionCont" : settings.Sumofor == "Customer" ? "CustomerCaptionCont" : (settings.Sumofor == "Vehicles" || settings.Sumofor == "Vehicle") ? "VehicleCaptionCont" : settings.Sumofor == "VehicleType" ? "VehicleTypeCaptionCont" : settings.Sumofor == "ExpensesCategory" ? "ExpensesCategoryCaptionCont" : settings.Sumofor == "ExpensesType" ? "optWrapperExpensesType" : settings.Sumofor == "DeviceType" ? "DeviceTypeCaptionCont" : settings.Sumofor == "GetStatusType" ? "GetStatusTypeCaptionCont" : settings.Sumofor == "GetDeviceID" ? "GetDeviceIDCaptionCont" : settings.Sumofor == "GetDeviceIMEIID" ? "GetDeviceIMEIIDCaptionCont" : settings.Sumofor == "GetSimMobileID" ? "GetSimMobileIDCaptionCont" : settings.Sumofor == "GetSimSNID" ? "GetSimSNIDCaptionCont" : settings.Sumofor == "SearchValue" ? "SearchValueCaptionCont" : settings.Sumofor == "AccountIdAllArr" ? "AccountIdAllArrCaptionCont" : settings.Sumofor == "LicenseKey" ? "LicenseKeyCaptionCont" : settings.Sumofor == "LicenseValidty" ? "LicenseValidtyCaptionCont" : settings.Sumofor == "Subscriber" ? "SubscriberCaptionCont" : settings.Sumofor == "AdditionalAcc" ? "AdditionalAccCaptionCont" : settings.Sumofor == "MappingStatus" ? "MappingStatusCaptionCont" : settings.Sumofor == "SubscriptionStatus" ? "SubscriptionStatusCaptionCont" : settings.Sumofor == "LicenseStatus" ? "LicenseStatusCaptionCont" : "CaptionCont";  

                        /*****END:: Find Caption Id***********/

                        CaptionCountId = "#" + CaptionCountId + "> span";

                        if (O.select.has(e.target).length == 0) // when click out side of sumo select container ::By Prince on 10-02-2022
                        {
                            if (O.E.find('option:selected').length == O.E.find('option').length) // when no of dropdown option length is equals to no of dropdown selected option length
                            {
                                if (O.E.find('option:selected').length > 0 && O.E.find('option').length > 0) {
                                    $(CaptionCountId).text("All");
                                }
                                if (!O.is_opened) return;
                                O.hideOpts();

                            }
                            else  // when no of dropdown option length is not equals to no of dropdown selected option length
                            {
                                if (O.E.find('option:selected').length > 0) //when select atleast one option
                                {
                                    $(CaptionCountId).text(O.E.find('option:selected').length + " Selected");
                                    O.hideOpts();
                                }
                                else   //when select none option
                                {
                                    $(CaptionCountId).text("--Select--");
                                    O.hideOpts();
                                }

                                //  if (!O.is_opened) return;


                            }

                        }
                        else {
                            if (!O.select.is(e.target)                  // if the target of the click isn't the container...
                            && O.select.has(e.target).length === 0) { // ... nor a descendant of the container
                                if (!O.is_opened) return;
                                O.hideOpts();
                                if (settings.okCancelInMulti) O._cnbtn();
                            }
                        }

                    });

                    if (O.is_floating) {
                        H = O.optDiv.children('ul').outerHeight() + 2;  // +2 is clear fix
                        if (O.is_multi) H = H + parseInt(O.optDiv.css('padding-bottom'));
                        O.optDiv.css('height', H);
                        $('body').addClass('sumoStopScroll');
                    }

                    O.setPstate();
                },

                //maintain state when ok/cancel buttons are available storing the indexes.
                setPstate: function () {
                    var O = this;
                    if (O.is_multi && (O.is_floating || settings.okCancelInMulti)) {
                        O.Pstate = [];
                        // assuming that find returns elements in tree order
                        O.E.find('option').each(function (i, e) { if (e.selected) O.Pstate.push(i); });
                    }
                },

                callChange: function () {
                    this.E.trigger('change').trigger('click');
                },

                hideOpts: function () {
                    var O = this;
                    if (O.is_opened) {
                        O.is_opened = false;
                        O.select.removeClass('open').find('ul li.sel').removeClass('sel');
                        $(document).off('click.sumo');
                        O.select.focus();
                        $('body').removeClass('sumoStopScroll');

                        // clear the search
                        if (settings.search) {
                            O.ftxt.val('');
                            O.optDiv.find('ul.options li').removeClass('hidden');
                            O.optDiv.find('.no-match').toggle(false);
                        }
                    }
                },
                setOnOpen: function () {
                    var O = this,
                        li = O.optDiv.find('li.opt:not(.hidden)').eq(settings.search ? 0 : O.E[0].selectedIndex);

                    O.optDiv.find('li.sel').removeClass('sel');
                    li.addClass('sel');
                    O.showOpts();
                },
                nav: function (up) {
                    var O = this, c,
                    s = O.ul.find('li.opt:not(.disabled, .hidden)'),
                    sel = O.ul.find('li.opt.sel:not(.hidden)'),
                    idx = s.index(sel);
                    if (O.is_opened && sel.length) {

                        if (up && idx > 0)
                            c = s.eq(idx - 1);
                        else if (!up && idx < s.length - 1 && idx > -1)
                            c = s.eq(idx + 1);
                        else return; // if no items before or after

                        sel.removeClass('sel');
                        sel = c.addClass('sel');

                        // setting sel item to visible view.
                        var ul = O.ul,
                            st = ul.scrollTop(),
                            t = sel.position().top + st;
                        if (t >= st + ul.height() - sel.outerHeight())
                            ul.scrollTop(t - ul.height() + sel.outerHeight());
                        if (t < st)
                            ul.scrollTop(t);

                    }
                    else
                        O.setOnOpen();
                },

                basicEvents: function () {
                    var O = this;
                    O.CaptionCont.click(function (evt) {
                        O.E.trigger('click');
                        if (O.is_opened) O.hideOpts(); else O.showOpts();
                        evt.stopPropagation();
                    });

                    O.select.on('keydown.sumo', function (e) {
                        switch (e.which) {
                            case 38: // up
                                O.nav(true);
                                break;

                            case 40: // down
                                O.nav(false);
                                break;

                            case 32: // space
                                if (settings.search && O.ftxt.is(e.target)) return;
                            case 13: // enter
                                if (O.is_opened)
                                    O.optDiv.find('ul li.sel').trigger('click');
                                else
                                    O.setOnOpen();
                                break;
                            case 9:	 //tab
                            case 27: // esc
                                if (settings.okCancelInMulti) O._cnbtn();
                                O.hideOpts();
                                return;

                            default:
                                return; // exit this handler for other keys
                        }
                        e.preventDefault(); // prevent the default action (scroll / move caret)
                    });

                    $(window).on('resize.sumo', function () {
                        O.floatingList();
                    });
                },

                onOptClick: function (li) {
                    ///debugger;
                    var O = this;
                    li.click(function () {
                        var li = $(this);
                        if (li.hasClass('disabled')) return;
                        txt = "";
                        if (O.is_multi) {
                            li.toggleClass('selected');
                            li.data('opt')[0].selected = li.hasClass('selected');
                            O.selAllState();
                        }
                        else {
                            li.parent().find('li.selected').removeClass('selected'); //if not multiselect then remove all selections from this list
                            li.toggleClass('selected');
                            li.data('opt')[0].selected = true;
                        }

                        //branch for combined change event.
                        if (!(O.is_multi && settings.triggerChangeCombined && (O.is_floating || settings.okCancelInMulti))) {
                            O.setText();
                            O.callChange();
                        }

                        if (!O.is_multi) O.hideOpts(); //if its not a multiselect then hide on single select.
                    });
                },
                setText: function () {
                    //  debugger;
                    var O = this;
                    O.placeholder = "";
                    if (O.is_multi) {
                        sels = O.E.find(':selected').not(':disabled'); //selected options.

                        for (i = 0; i < sels.length; i++) {

                            if (i + 1 >= settings.csvDispCount && settings.csvDispCount) {
                                if (sels.length == O.E.find('option').length && settings.captionFormatAllSelected) {
                                    O.placeholder = settings.captionFormatAllSelected.replace(/\{0\}/g, sels.length) + ',';
                                } else {
                                    O.placeholder = settings.captionFormat.replace(/\{0\}/g, sels.length) + ',';
                                }

                                break;
                            }
                            else O.placeholder += $(sels[i]).text() + ", ";
                        }

                        O.placeholder = O.placeholder.replace(/,([^,]*)$/, '$1'); //remove unexpected "," from last.
                    }
                    else {
                        O.placeholder = O.E.find(':selected').not(':disabled').text();
                    }

                    is_placeholder = false;

                    if (!O.placeholder) {

                        is_placeholder = true;

                        O.placeholder = O.E.attr('placeholder');
                        if (!O.placeholder)                  //if placeholder is there then set it
                            O.placeholder = O.E.find('option:disabled:selected').text();
                    }

                    O.placeholder = O.placeholder ? (settings.prefix + ' ' + O.placeholder) : settings.placeholder

                    //set display text
                    O.caption.html(O.placeholder);
                    O.CaptionCont.attr('title', O.placeholder);

                    //set the hidden field if post as csv is true.
                    csvField = O.select.find('input.HEMANT123');
                    if (csvField.length) csvField.val(O.getSelStr());

                    //add class placeholder if its a placeholder text.
                    if (is_placeholder) O.caption.addClass('placeholder'); else O.caption.removeClass('placeholder');
                    return O.placeholder;
                },

                isMobile: function () {

                    // Adapted from http://www.detectmobilebrowsers.com
                    var ua = navigator.userAgent || navigator.vendor || window.opera;

                    // Checks for iOs, Android, Blackberry, Opera Mini, and Windows mobile devices
                    for (var i = 0; i < settings.nativeOnDevice.length; i++) if (ua.toString().toLowerCase().indexOf(settings.nativeOnDevice[i].toLowerCase()) > 0) return settings.nativeOnDevice[i];
                    return false;
                },

                setNativeMobile: function () {
                    var O = this;
                    O.E.addClass('SelectClass')//.css('height', O.select.outerHeight());
                    O.mob = true;
                    O.E.change(function () {
                        O.setText();
                    });
                },

                floatingList: function () {
                    var O = this;
                    //called on init and also on resize.
                    //O.is_floating = true if window width is < specified float width
                    O.is_floating = $(window).width() <= settings.floatWidth;

                    //set class isFloating
                    O.optDiv.toggleClass('isFloating', O.is_floating);

                    //remove height if not floating
                    if (!O.is_floating) O.optDiv.css('height', '');

                    //toggle class according to okCancelInMulti flag only when it is not floating
                    O.optDiv.toggleClass('okCancelInMulti', settings.okCancelInMulti && !O.is_floating);
                },

                //HELPERS FOR OUTSIDERS
                // validates range of given item operations
                vRange: function (i) {
                    var O = this;
                    opts = O.E.find('option');
                    if (opts.length <= i || i < 0) throw "index out of bounds"
                    return O;
                },

                //toggles selection on c as boolean.
                toggSel: function (c, i) {
                    // debugger;
                    var O = this;
                    if (typeof (i) === "number") {
                        O.vRange(i);
                        opt = O.E.find('option')[i];
                    }
                    else {
                        opt = O.E.find('option[value="' + i + '"]')[0] || 0;
                    }
                    if (!opt || opt.disabled)
                        return;

                    if (opt.selected != c) {
                        opt.selected = c;
                        if (!O.mob) $(opt).data('li').toggleClass('selected', c);

                        O.callChange();
                        O.setPstate();
                        O.setText();
                        O.selAllState();
                    }
                },

                //toggles disabled on c as boolean.
                toggDis: function (c, i) {
                    var O = this.vRange(i);
                    O.E.find('option')[i].disabled = c;
                    if (c) O.E.find('option')[i].selected = false;
                    if (!O.mob) O.optDiv.find('ul.options li').eq(i).toggleClass('disabled', c).removeClass('selected');
                    O.setText();
                },

                // toggle disable/enable on complete select control
                toggSumo: function (val) {
                    var O = this;
                    O.enabled = val;
                    O.select.toggleClass('disabled', val);

                    if (val) {
                        O.E.attr('disabled', 'disabled');
                        O.select.removeAttr('tabindex');
                    }
                    else {
                        O.E.removeAttr('disabled');
                        O.select.attr('tabindex', '0');
                    }

                    return O;
                },

                //toggles alloption on c as boolean.
                toggSelAll: function (c) {
                    var O = this;
                    O.E.find('option').each(function (ix, el) {
                        if (O.E.find('option')[$(this).index()].disabled) return;
                        O.E.find('option')[$(this).index()].selected = c;
                        if (!O.mob)
                            O.optDiv.find('ul.options li').eq($(this).index()).toggleClass('selected', c);
                        O.setText();
                    });
                    if (!O.mob && O.selAll) O.selAll.removeClass('partial').toggleClass('selected', c);
                    O.callChange();
                    O.setPstate();
                },

                /* outside accessibility options
                   which can be accessed from the element instance.
                */
                reload: function () {
                    var elm = this.unload();
                    return $(elm).SumoSelect(settings);
                },

                unload: function () {
                    var O = this;
                    O.select.before(O.E);
                    O.E.show();

                    if (settings.outputAsCSV && O.is_multi && O.select.find('input.HEMANT123').length) {
                        O.E.attr('name', O.select.find('input.HEMANT123').attr('name')); // restore the name;
                    }
                    O.select.remove();
                    delete selObj.sumo;
                    return selObj;
                },

                //## add a new option to select at a given index.
                add: function (val, txt, attribute, i) {
                    //  debugger;
                    var d = undefined;
                    attribute = (typeof attribute == "undefined") ? "" : attribute;
                    if (typeof val == "undefined") throw "No value to add"

                    var O = this;
                    opts = O.E.find('option')
                    if (typeof txt == "number") { i = txt; txt = val; }
                    if (typeof txt == "undefined") { txt = val; }

                    opt = $("<option></option>").attr("data-id", attribute).val(val).html(txt);


                    if (opts.length < i) throw "index out of bounds"

                    if (typeof i == "undefined" || opts.length == i) { // add it to the last if given index is last no or no index provides.
                        O.E.append(opt);
                        if (!O.mob) O.ul.append(O.createLi(opt, d, attribute));
                    }
                    else {
                        opts.eq(i).before(opt);
                        if (!O.mob) O.ul.find('li.opt').eq(i).before(O.createLi(opt, d, attribute));
                    }

                    return selObj;
                },

                //## removes an item at a given index.
                remove: function (i) {
                    var O = this.vRange(i);
                    O.E.find('option').eq(i).remove();
                    if (!O.mob) O.optDiv.find('ul.options li').eq(i).remove();
                    O.setText();
                },

                //## Select an item at a given index.
                selectItem: function (i) {
                    /// debugger;
                    this.toggSel(true, i
                        );
                },

                //## UnSelect an iten at a given index.
                unSelectItem: function (i) { this.toggSel(false, i); },

                //## Select all items  of the select.
                selectAll: function () {
                    this.toggSelAll(true);
                },

                //## UnSelect all items of the select.
                unSelectAll: function () { this.toggSelAll(false); },

                //## Disable an iten at a given index.
                disableItem: function (i) { this.toggDis(true, i) },

                //## Removes disabled an iten at a given index.
                enableItem: function (i) { this.toggDis(false, i) },

                //## New simple methods as getter and setter are not working fine in ie8-
                //## variable to check state of control if enabled or disabled.
                enabled: true,
                //## Enables the control
                enable: function () { return this.toggSumo(false) },

                //## Disables the control
                disable: function () { return this.toggSumo(true) },


                init: function () {
                    var O = this;
                    O.createElems();
                    O.setText();
                    return O
                }

            };

            selObj.sumo.init();
        });

        return ret.length == 1 ? ret[0] : ret;
    };


}(jQuery));
