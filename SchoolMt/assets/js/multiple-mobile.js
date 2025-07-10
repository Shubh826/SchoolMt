(function ($) {

    $.fn.multiple_mobiles = function (options) {
        

        // Default options
        var defaults = {
            checkDupmobile: true,
            theme: "Bootstrap",
            position: "top",
            mobdata:''
        };

        // Merge send options with defaults
        var settings = $.extend({}, defaults, options);
        var mobile = '';
        var a = new RegExp(/^[6789]\d{9}$/);
     //  var a = new RegExp(/^(\d{10},)*\d{10}$/);
        var deleteIconHTML = "";
        if (settings.theme.toLowerCase() == "Bootstrap".toLowerCase()) {
            //deleteIconHTML = '<a href="#" class="multiple_mobiles-close" title="Remove"><span class="glyphicon glyphicon-remove"></span></a>';
            deleteIconHTML = '<a href="#" class="multiple_emails-close" title="Remove"><img src="/images/remove.png" height="10" style="padding-right:5px;"></span></a>';
        }
        else if (settings.theme.toLowerCase() == "SemanticUI".toLowerCase() || settings.theme.toLowerCase() == "Semantic-UI".toLowerCase() || settings.theme.toLowerCase() == "Semantic UI".toLowerCase()) {
            //deleteIconHTML = '<a href="#" class="multiple_mobiles-close" title="Remove"><i class="remove icon"></i></a>';
            deleteIconHTML = '<a href="#" class="multiple_emails-close" title="Remove"><img src="/images/remove.png" height="10" style="padding-right:5px;"></a>';
        }
        else if (settings.theme.toLowerCase() == "Basic".toLowerCase()) {
            //Default which you should use if you don't use Bootstrap, SemanticUI, or other CSS frameworks
            deleteIconHTML = '<a href="#" class="multiple_mobiles-close" title="Remove"><i class="basicdeleteicon">Remove</i></a>';
        }
        
        return this.each(function () {
           // debugger
            //$orig refers to the input HTML node
            var $orig = $(this);
            var $list = $('<ul class="multiple_mobiles-ul" />'); // create html elements - list of mobile numbers as unordered list
            if (settings.mobdata!='') {
                //debugger
                //Remove space, comma and semi-colon from beginning and end of string
                //Does not remove inside the string as the email will need to be tokenized using space, comma and semi-colon
                var arr = settings.mobdata.trim().replace(/^,|,$/g, '').replace(/^;|;$/g, '');
                //Remove the double quote
                arr = arr.replace(/"/g, "");
                //Split the string into an array, with the space, comma, and semi-colon as the separator
                arr = arr.split(/[\s,;]+/);
                //var errorMobiles = new Array(); //New array to contain the errors
                var Mobile = '';
                for (var i = 0; i < arr.length; i++) {
                    //Check if the email is already added, only if dupEmailCheck is set to true
                    if (a.test(arr[i]) == true && settings.mobdata!='') {
                        $list.append($('<li class="multiple_mobiles-mobile"><span class="mobileno" data-mobile="' + arr[i] + '">' + arr[i] + '</span></li>')
					  .prepend($(deleteIconHTML)
						   .click(function (e) { $(this).parent().remove(); refresh_mobiles(); e.preventDefault(); })
					  )
                        );
                        Mobile += arr[i] + ','
                    
                    }
                   
                }
 
                $(this).val(Mobile.slice(0, -1)).hide();
            }
            
            //if ($(this).val() != '' && IsJsonString($(this).val())) {
            //   debugger
            //    $.each(jQuery.parseJSON($(this).val()), function (index, val) {
            //        $list.append($('<li class="multiple_mobiles-mobile"><span class="mobileno" data-mobile="' + val+ '">' + val + '</span></li>')
			//		  .prepend($(deleteIconHTML)
			//			   .click(function (e) { $(this).parent().remove(); refresh_mobiles(); e.preventDefault(); })
			//		  )
			//		);
            //    });
            //}

            var $input = $('<input type="text" class="multiple_mobiles-input text-center" placeholder="Mobile No." />').on('keyup', function (e) {
               // debugger// input
                $(this).removeClass('multiple_mobiles-error');
                var input_length = $(this).val().length;

                var keynum;
                if (window.event) { // IE					
                    keynum = e.keyCode;
                }
                else if (e.which) { // Netscape/Firefox/Opera					
                    keynum = e.which;
                }

                //if(event.which == 8 && input_length == 0) { $list.find('li').last().remove(); } //Removes last item on backspace with no input

                // Supported key press is tab, enter, space or comma, there is no support for semi-colon since the keyCode differs in various browsers
                if (keynum == 9 || keynum == 32 || keynum == 188) {
                    display_mobile($(this), settings.checkDupmobile);
                }
                else if (keynum == 13) {
                    display_mobile($(this), settings.checkDupmobile);
                    //Prevents enter key default
                    //This is to prevent the form from submitting with  the submit button
                    //when you press enter in the email textbox
                    e.preventDefault();
                }

            }).on('blur', function (event) {
                if ($(this).val() != '') { display_mobile($(this), settings.checkDupmobile); }
            });

            var $container = $('<div class="multiple_mobiles-container" />').click(function () { $input.focus(); }); // container div

            // insert elements into DOM
            if (settings.position.toLowerCase() === "top")
                $container.append($list).append($input).insertAfter($(this));
            else
                $container.append($input).append($list).insertBefore($(this));

            /*
			t is the text input device.
			Value of the input could be a long line of copy-pasted emails, not just a single email.
			As such, the string is tokenized, with each token validated individually.
			
			If the dupmobileCheck variable is set to true, scans for duplicate mobiles, and invalidates input if found.
			Otherwise allows emails to have duplicated values if false.
			*/
            function display_mobile(t, dupmobileCheck) {
              
                //Remove space, comma and semi-colon from beginning and end of string
                //Does not remove inside the string as the email will need to be tokenized using space, comma and semi-colon
                var arr = t.val().trim().replace(/^,|,$/g, '').replace(/^;|;$/g, '');
                //Remove the double quote
                arr = arr.replace(/"/g, "");
                //Split the string into an array, with the space, comma, and semi-colon as the separator
                arr = arr.split(/[\s,;]+/);
               
                var errormobiles = new Array(); //New array to contain the errors
               // var a = new RegExp(/^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$/i);
                // /^(\d{10},)*\d{10}$/
               
             
                for (var i = 0; i < arr.length; i++) {
                    //Check if the mobile no is already added, only if dupmobileCheck is set to true
                    if (dupmobileCheck === true && $orig.val().indexOf(arr[i]) != -1) {
                        if (arr[i] && arr[i].length > 0) {
                            new function () {//
                                var existingElement = $list.find('.mobileno[data-mobile=' + arr[i]+ ']');//.replace('.', '\\.') .replace('@', '\\@');
                                existingElement.css('font-weight', 'bold');
                                setTimeout(function () { existingElement.css('font-weight', ''); }, 1500);
                            }(); // Use a IIFE function to create a new scope so existingElement won't be overriden
                        }
                    }
                    else if (a.test(arr[i]) == true) {
                        $list.append($('<li class="multiple_mobiles-mobile"><span class="mobileno" data-mobile="' + arr[i]+ '">' + arr[i] + '</span></li>')
							  .prepend($(deleteIconHTML)
								   .click(function (e) { $(this).parent().remove(); refresh_mobiles(); e.preventDefault(); })
							  )
						);
                    }
                    else
                        errormobiles.push(arr[i]);
                }
                // If erroneous mobilenumbers found, or if duplicate mobile found
                if (errormobiles.length > 0) {
                    //  t.val(errormobiles.join("; ")).addClass('multiple_mobiles-error');
                    t.val("");
                }
                  
                else
                    t.val("");
                refresh_mobiles();
            }

            function refresh_mobiles() {
                var mobiles = new Array();
                var container = $orig.siblings('.multiple_mobiles-container');
                container.find('.multiple_mobiles-mobile span.mobileno').each(function () { mobiles.push($(this).html()); });
                // $orig.val(JSON.stringify(mobiles)).trigger('change');
                $orig.val(mobiles).trigger('change');
            }

            function IsJsonString(str) {
                try { JSON.parse(str); }
                catch (e) { return false; }
                return true;
            }

            return $(this).hide();

        });

    };

})(jQuery);
