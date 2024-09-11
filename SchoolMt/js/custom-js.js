// JavaScript Document
$(document).ready(function() {
	var headerH = $(".header").height();
	var navbarH = $(".navbar").height() + 10;
	var footerH = $(".footer").height();
	var sectionH = $(window).height() - ( headerH + navbarH + footerH + 12);
	$('.nanoTable').css('height', (sectionH - 100));
	$('.nanoTableMin').css('height', (sectionH - 200));
	var autoRefreshH = $(".autoRefresh").height();
	var windowHeight = $(window).height() - ( headerH + navbarH + autoRefreshH + 10);
	var searchH = $(".stylish-input-group").height();
	var loginBgH = $(window).height();
	$('.loginBg').css('height', loginBgH);
    //alert(loginBgH);
	$('.section-body .contentHeightShow').css('min-height', sectionH);
	$('#mapLiveTracking').css('height', (sectionH - 30));
	$('.section-body .heightShow').css('min-height', windowHeight);
	$('#mapTracking').css('min-height', (windowHeight - (searchH +3)));
	
	//-----Pin Menu Start-----//
	$(".pinMnu").click(function () {
        if (!$(this).parents('.section-body').hasClass('full-right')) {
            $(this).parents('.section-body').addClass('full-right');
            $(this).find('i').addClass('inSide');
        } else {
			$(this).find('i').removeClass('inSide');
            $(this).find('i').addClass('outSide');
            $(this).parents('.section-body').removeClass('full-right');
        };
        return false;
    });
	//-----Pin Menu End-----//
	
	//-----dashLeftpinMnu Menu Start-----//
	$(".dashLeftpinMnu").click(function () {
        if (!$(this).parents('.section-body').hasClass('full-weight-right')) {
            $(this).parents('.section-body').addClass('full-weight-right');
        } else {
            $(this).parents('.section-body').removeClass('full-weight-right');
        };
        return false;
    });
	//-----dashLeftpinMnu Menu End-----//
	
	//-----dashRightpinMnu Menu Start-----//
	$(".dashRightpinMnu").click(function () {
        if (!$(this).parents('.section-body').hasClass('full-weight-left')) {
            $(this).parents('.section-body').addClass('full-weight-left');
        } else {
            $(this).parents('.section-body').removeClass('full-weight-left');
        };
        return false;
    });
	//-----dashRightpinMnu Menu End-----//
	
	$(".hide-btn").click(function () {
        if (!$(this).parents('.panel').hasClass('collapsible')) {
            $(this).parents('.panel').addClass('collapsible');
			$(this).find('i.fa').removeClass('fa-minus');
            $(this).find('i.fa').addClass('fa-plus');
            $(this).parents('.trackingData').addClass('trackingNone');
            $(this).find('i').removeClass('inSide');
            $(this).find('i').addClass('outSide');
            $(this).parents('.section-body').removeClass('full-right');
        } else {
			$(this).find('i.fa').removeClass('fa-plus');
            $(this).find('i.fa').addClass('fa-minus');
            $(this).parents('.panel').removeClass('collapsible');			
			$(this).parents('.trackingData').removeClass('trackingNone');
			$(this).parents('.section-body').addClass('full-right');
            $(this).find('i').addClass('inSide');
			$(this).parents().find('.trackingData').show();
        };
        return false;
    });
	
	$(".play-btn").click(function(){
		//alert("hello");
		if (!$(this).parents('.section-body').hasClass('full-right')) {
            $(this).parents('.section-body').addClass('full-right');
            $(this).find('i').addClass('inSide');
			$(this).parents().find('.trackingData').show();
        } else {
			$(this).find('i').removeClass('inSide');
            $(this).find('i').addClass('outSide');
            $(this).parents('.section-body').removeClass('full-right');
        };
        return false;
	});
	
	$(".close-btn").click(function () {
		$(this).parents('.trackingData').hide();
	});
});

$(function(){
    $(".dropdown").hover(            
            function() {
                $('.dropdown-menu', this).stop( true, true ).fadeIn("fast");
                $(this).toggleClass('open');
                $('b', this).toggleClass("caret caret-up");                
            },
            function() {
                $('.dropdown-menu', this).stop( true, true ).fadeOut("fast");
                $(this).toggleClass('open');
                $('b', this).toggleClass("caret caret-up");                
            });
    });