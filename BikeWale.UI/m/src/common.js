$(document).ready(function() {
	var availableTags = [
		"Bajaj Suzuki 800",
		"Bajaj Suzuki 500",
		"Bajaj 1",
		"Bajaj 2",
		"Bajaj 3",
		"Bajaj 4",
		"Bajaj 5",
		"Bajaj 6",
		"Bajaj 7",
	];	
	
	 $('.globalcity-close-btn').click(function () {
        CloseCityPopUp();
    });
	
	function CloseCityPopUp() {
		var globalLocation = $("#globalcity-popup");
		/*
		if (!isCookieExists('_CustCityMaster')) {
			SetCookieInDays('_CustCityMaster', 'Select City');
			SetCookieInDays('_CustCityIdMaster', '-1');
		}
		
		function isCookieExists(cookiename) {
		var coockieVal = $.cookie(cookiename);
		if (coockieVal == undefined || coockieVal == null)
			return false;
		return true;
		}
		*/
		globalLocation.hide();
		globalLocation.removeClass("show").addClass("hide");
		unlockPopup();
	}
	$("#globalCity").autocomplete({
		source: function(request, response) {
			dataListDisplay(availableTags,request, response);
		},minLength: 1
	}).css({'width':'179px'});
	$("#newBikeList").autocomplete({
		source: function(request, response) {
			dataListDisplay(availableTags,request, response);
		},minLength: 1
	}).css({'width':'100%'});
	// global city popup autocomplete
	$("#globalCityPopUp").autocomplete({
		source: function(request, response) {
			dataListDisplay(availableTags,request, response);
		},minLength: 1
	}).autocomplete("widget").addClass("globalCity-autocomplete").css({'z-index':'11'});
	
	$("#getFinalPrice").autocomplete({
		source: function(request, response) {
			dataListDisplay(availableTags,request, response);
		},minLength: 1
	});
	
	$("#citySelectionFinalPrice").autocomplete({
		source: function(request, response) {
			dataListDisplay(availableTags,request, response);
		},minLength: 1
	}).css({'width':'365px'});
	
	
	
	$(".blackOut-window").mouseup(function(e){
		var globalSearchPopup = $(".global-search-popup"); 
        if(e.target.id !== globalSearchPopup.attr('id') && !globalSearchPopup.has(e.target).length)		
        {
		    globalSearchPopup.hide();
			unlockPopup();
        }
    });
	/*$("#gs-close").click(function(){
		$(".global-search-popup").hide(); 
		unlockPopup();        
    });
	$("#gs-text-clear").click( function(){
		$("#globalSearchPopup").focus();
	});*/
	// globalcity-popup code 
	$(".global-location").click( function(){
		$("#globalcity-popup").show();
		lockPopup();
	});
	$(".blackOut-window").mouseup(function(e){
		var globalLocation = $("#globalcity-popup"); 
        if(e.target.id !== globalLocation.attr('id') && !globalLocation.has(e.target).length)		
        {
		    globalLocation.hide();
			unlockPopup();
        }
    });
	
	/*$(".card").flip({
		axis: 'y',
		trigger: 'manual',
		reverse: true
	});
	
	$(".infoBtn").click(function(){
		$(this).parents("li").flip(true).siblings().flip(false);	
	});
	
	$(".closeBtn").click(function(){
		$(this).parents("li").flip(false);	
	});*/
	
	// nav bar code starts
	$(".navbarBtn").click(function(){
		navbarShow();
	});
	function navbarShow() {
		//$('body').addClass('lock-browser-scroll');
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
	$(".navUL > li > a").click(function(e){
		
		if(!$(this).hasClass("open")) {
			var a = $(".navUL li a");
			a.removeClass("open").next("ul").slideUp(350);
			$(this).addClass("open").next("ul").slideDown(350);
			
			if($(this).siblings().size() == 0) {
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
		//$('body').addClass('lock-browser-scroll');
		$("#nav").removeClass('open').animate({'left':'-300px'});
		$(".blackOut-window").hide();
	}
	// login code starts 
	$("#firstLogin").click(function(){
		lockPopup();
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
	$(".loginCloseBtn").click(function(){
		unlockPopup();
		$(".loginPopUpWrapper").animate({right:'-400px'});
		loginSignupSwitch();
	});	
	$("#forgotpass").click(function(){
		$("#forgotpassbox").toggleClass("hide show");
	});
	$(".loginBtnSignUp").click(function(){
		$(".loginStage").hide();
		$(".signUpStage").show();
	});
	$(".signupBtnLogin").click(function(){
		loginSignupSwitch();
	});
	function loginSignupSwitch(){
		$(".loginStage").show();
		$(".signUpStage").hide();
	}
	function lockPopup() {
		//$('body').addClass('lock-browser-scroll');
		$(".blackOut-window").show();		
	}
	function unlockPopup() {
		//$('body').removeClass('lock-browser-scroll');
		$(".blackOut-window").hide();
	}	
	
	// lang changer code
    $(".changer-default").click( function(){
		$(".lang-changer-option").show();
	});
	$(".lang-changer-option li a").click( function(){
		var langTxt = $(this).text();
		$("#LangName").text(langTxt);
		$(".lang-changer-option").hide();
	}); // ends	
	// Common BW tabs code
	/*$(".bw-tabs li").live('click',function() {
		var panel = $(this).closest(".bw-tabs-panel");
		panel.find(".bw-tabs li").removeClass("active");
		$(this).addClass("active");
		var panelId = $(this).attr("data-tabs");
		panel.find(".bw-tabs-data").hide();
		$("#" + panelId).show();
	});*/ // ends
	// Common CW select box tabs code
	$(".bw-tabs select").change( function (){
		var panel = $(this).closest(".bw-tabs-panel");
		var panelId = $(this).val();
		panel.find(".bw-tabs-data").hide();
		$('#' + panelId).show();
	}); // ends
	/* jCarousel custom methods */
	$(function () {
		var jcarousel = $('.jcarousel').jcarousel();
		$('.jcarousel-control-prev').on('jcarouselcontrol:active', function () {
			$(this).removeClass('inactive');
		}).on('jcarouselcontrol:inactive', function () {
			$(this).addClass('inactive');
		}).jcarouselControl({
			target: '-=1'
		});
		$('.jcarousel-control-next').on('jcarouselcontrol:active', function () {
			$(this).removeClass('inactive');
		}).on('jcarouselcontrol:inactive', function () {
			$(this).addClass('inactive');
		}).jcarouselControl({
			target: '+=1'
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




