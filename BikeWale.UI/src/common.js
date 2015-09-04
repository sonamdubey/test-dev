// JavaScript Document
$(document).ready(function() {
	var availableTags = [
		"Bajaj 1",
		"Bajaj 2",
		"Bajaj 3",
		"Bajaj 4",
		"Bajaj 5"
	];
	
	$("#globalCity").autocomplete({
		source: function(request, response) {
			dataListDisplay(availableTags,request, response);
		},minLength: 1
	}).css({'width':'179px'});
	
	$("#globalCityPopUp").autocomplete({
		source: function(request, response) {
			dataListDisplay(availableTags,request, response);
		},minLength: 1
	}).css({'width':'350px'});
	
	$("#newBikeList").autocomplete({
		source: function(request, response) {
			dataListDisplay(availableTags,request, response);
		},minLength: 1
	}).css({'width':'469px'});
	
	$("#makemodelFinalPrice").autocomplete({
		source: function(request, response) {
			dataListDisplay(availableTags,request, response);
		},minLength: 1
	}).css({'width':'365px'});
	
	// nav bar code starts
	$(".navbarBtn").click(function(){
		navbarShow();
	});
	
	function navbarShow() {
		$("#nav").addClass('open').animate({'left':'0px'});
		$(".blackOut-window").show();
	}	
	$(".blackOut-window").mouseup(function(e){
		var nav = $("#nav"); 
        if(e.target.id !== nav.attr('id') && !nav.has(e.target).length)		
        {
		    nav.animate({'left':'-300px'});
			unlockPopup();
        }
    }); 
	$(".navUL > li > a").click(function(){		
		if(!$(this).hasClass("open")) {
			var a = $(".navUL li a");
			a.removeClass("open").next("ul").slideUp(350);
			$(this).addClass("open").next("ul").slideDown(350);
			
			if($(this).siblings().size() === 0) {
				navbarHide();
			}
			
			$(".nestedUL > li > a").click(function(){
				$(".nestedUL li a").removeClass("open");
				$(this).addClass("open");
				navbarHide();
			});
			
		}
		else if($(this).hasClass("open")) {
			$(this).removeClass("open").next("ul").slideUp(350);
		}
	}); // nav bar code ends here
	
	function navbarHide(){
		$("#nav").removeClass('open').animate({'left':'-300px'});
		$(".blackOut-window").hide();
	}
	function navbarHideOnESC() {
		$("#nav").removeClass('open').animate({ 'left': '-300px' });
		$(".blackOut-window").hide();
	}
	
	// login code starts 
	$("#firstLogin").click(function(){
		$(".blackOut-window").show();
		$(".loginPopUpWrapper").animate({right:'0'});
	});
	$(".blackOut-window").mouseup(function(e){
		var loginPopUp = $(".loginPopUpWrapper");
        if(e.target.id !== loginPopUp.attr('id') && !loginPopUp.has(e.target).length)
        {
            loginPopUp.animate({'right':'-400px'});
			unlockPopup();
        }
    });
	
	$(".loginCloseBtn").click(function () {
        unlockPopup();
        $(".loginPopUpWrapper").animate({ right: '-400px' });
        $(".loggedinProfileWrapper").animate({ right: '-280px' });
        loginSignupSwitch();
    });
    $("#forgotpass").click(function () {
        $("#forgotpassbox").toggleClass("hide show");
    });
    $("button.loginBtnSignUp").click(function () {
        $("div.loginStage").hide();
        $("div.signUpStage").show();
    });
    $("#btnSignUpBack").click(function () {
        $("div.signUpStage").hide();
        $("div.loginStage").show();
    });
    $("#btnSignUpBack").click(function () {
        loginSignupSwitch();
    });
    function loginSignupSwitch() {
        $(".loginStage").show();
        $(".signUpStage").hide();
    }
	
	//user logged in code
	$("#userLoggedin").click(function(){
		$(".blackOut-window").show();
		$(".loggedinProfileWrapper").animate({right:'0'});
	});
	$(".afterLoginCloseBtn").click(function(){
		unlockPopup();
		$(".loggedinProfileWrapper").animate({right:'-280px'});
		loginSignupSwitch();
	});
	$(".blackOut-window").mouseup(function(e){
		var loggedIn = $(".loggedinProfileWrapper");
        if(e.target.id !== loggedIn.attr('id') && !loggedIn.has(e.target).length)
        {
            loggedIn.animate({'right':'-280px'});
			unlockPopup();
        }
    });
	
	function headerOnScroll() {
		if ($(window).scrollTop() > 40) {
			$('#header').addClass('header-fixed-with-bg');
		} else {
			$('#header').removeClass('header-fixed-with-bg');
		}
	}

	// for landing pages header scroll with bg effect
    if (typeof (landingPage) != "undefined" && landingPage == true) {
        $('#header').removeClass('header-fixed').addClass('header-landing');
        headerOnScroll();
        $(window).scroll(headerOnScroll);
    }

	
	//global city popup
	$(".gl-default-stage").click( function(){
		$(".blackOut-window").show();
		$(".globalcity-popup").removeClass("hide").addClass("show");
	});
	
	$(".blackOut-window").mouseup(function(e){
		var globalLocation = $("#globalcity-popup"); 
        if(e.target.id !== globalLocation.attr('id') && !globalLocation.has(e.target).length)		
        {
		    globalLocation.removeClass("show").addClass("hide");
			unlockPopup();
        }
    });
	$(".globalcity-close-btn").click(function(){
		$(".globalcity-popup").removeClass("show").addClass("hide");
		unlockPopup();
	});
	
	function CloseCityPopUp() {
		var globalLocation = $("#globalcity-popup");
		globalLocation.removeClass("show").addClass("hide");
		unlockPopup();
	}
	
	$(document).keydown(function (e) {
		// ESCAPE key pressed
		if (e.keyCode == 27) {
			CloseCityPopUp();
			navbarHideOnESC();
			loginHideOnESC();
		}
	});
	
	function loginHideOnESC() {
		$(".loginPopUpWrapper").animate({ right: '-400px' });
		$(".loggedinProfileWrapper").animate({
			right: '-280px'
		});
	}
		
	function lockPopup() {
		$('body').addClass('lock-browser-scroll');
		$(".blackOut-window").show();
	}
	
	function unlockPopup() {
		$('body').removeClass('lock-browser-scroll');
		$(".blackOut-window").hide();
	}
	
	// Common BW tabs code
	$(".bw-tabs li").live('click', function () {
		var panel = $(this).closest(".bw-tabs-panel");
		panel.find(".bw-tabs li").removeClass("active");
		$(this).addClass("active");
		var panelId = $(this).attr("data-tabs");
		panel.find(".bw-tabs-data").hide();
		$("#" + panelId).show();
	}); // ends
	/* jCarousel custom methods */
	$(function () {
		var jcarousel = $('.jcarousel').jcarousel();
		$('.jcarousel-control-prev').on('jcarouselcontrol:active', function () {
			$(this).removeClass('inactive');
		}).on('jcarouselcontrol:inactive', function () {
			$(this).addClass('inactive');
		}).jcarouselControl({
			target: '-=3'
		});
		$('.jcarousel-control-next').on('jcarouselcontrol:active', function () {
			$(this).removeClass('inactive');
		}).on('jcarouselcontrol:inactive', function () {
			$(this).addClass('inactive');
		}).jcarouselControl({
			target: '+=3'
		});
		$('.jcarousel-pagination').on('jcarouselpagination:active', 'a', function () {
			$(this).addClass('active');
		}).on('jcarouselpagination:inactive', 'a', function () {
			$(this).removeClass('active');
		}).on('click', function (e) {
			e.preventDefault();
		}).jcarouselPagination({
			item: function (page) {
				return '<a href="#' + page + '">' + page + '</a>';
			}
		});
		// Swipe handlers for mobile
		$(".jcarousel").swipe({ fingers: 'all', swipeLeft: swipe1, swipeRight: swipe1, allowPageScroll: "auto" });
		function swipe1(event, direction, distance, duration, fingerCount) {
			if (direction == "left") {
				$(this).closest('.jcarousel-wrapper').find("a.jcarousel-control-next").click();
			}
			else if (direction == "right") {
				$(this).closest('.jcarousel-wrapper').find("a.jcarousel-control-prev").click();
			}
		}
	});
	// common autocomplete data call function
	function dataListDisplay(availableTags,request,response){
		var results = $.ui.autocomplete.filter(availableTags, request.term);
		response(results.slice(0, 5));
	}		
	
});