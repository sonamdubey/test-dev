var focusedMakeModel = null, focusedCity = null;
var objBikes = new Object();
var objCity = new Object();
var globalCityId = 0;

$(document).ready(function () {
    $('#newBikeList').val('').focus();
    $('#globalCityPopUp').val('');

	$(".fa-spinner").hide();
	CheckGlobalCookie();
	
	 $('.globalcity-close-btn').click(function () {
        CloseCityPopUp();
    });
	
	function CloseCityPopUp() {
		var globalLocation = $("#globalcity-popup");
		globalLocation.hide();
		globalLocation.removeClass("show").addClass("hide");
		unlockPopup();
		if (!isCookieExists("location"))
		    SetCookieInDays("location", "0", 365);
	}

	$("#globalCity").autocomplete({
		source: function(request, response) {
			dataListDisplay(availableTags,request, response);
		}, minLength: 1
	}).css({ 'width': '179px' });
	

	

    $("#newBikeList").bw_autocomplete({
        recordCount: 5,
        source: 1,
        onClear: function () {
            objBikes = new Object();
        },
        click: function (event, ui, orgTxt) {
            var make = new Object();
            make.maskingName = ui.item.payload.makeMaskingName;
            make.id = ui.item.payload.makeId;
            var model = null;
            if (ui.item.payload.modelId > 0) {
                model = new Object();
                model.maskingName = ui.item.payload.modelMaskingName;
                model.id = ui.item.payload.modelId;
            }
            MakeModelRedirection(make, model);
        },
        open: function (result) {
            objBikes.result = result;
        },
        focusout: function () {
            if ($('li.ui-state-focus a:visible').text() != "") {
                focusedMakeModel = new Object();
                focusedMakeModel = objBikes.result[$('li.ui-state-focus').index()];
            }
        },
        afterfetch: function (result, searchtext) {
            return false;
        }
    }).css({ 'width': '100%' });


    $('#newBikeList').on('keypress', function (e) {
        var id = $('#newBikeList');
        var searchVal = id.val();
        var placeHolder = id.attr('placeholder');
        if (e.keyCode == 13)
            if (btnFindBikeNewNav() || searchVal == placeHolder || searchVal == "") {
                return false;
            }
            else {
                window.location.href = '/new/';
            }
    });


    $('#btnSearch').on('click', function (e) {
        var id = $('#newBikeList');
        var searchVal = id.val();
        var placeHolder = id.attr('placeholder');
        if (btnFindBikeNewNav() || searchVal == placeHolder || (searchVal).trim() == "") {
            window.location.href += '/new/';
            return false;
        } 
    });

    function btnFindBikeNewNav() {
        if (focusedMakeModel == undefined || focusedMakeModel == null)
            return false;
        var splitVal = focusedMakeModel.id.split('|');
        var make = new Object();
        make.maskingName = focusedMakeModel.payload.makeMaskingName;
        make.id = focusedMakeModel.payload.makeId;
        var model = null;
        if (focusedMakeModel.payload.modelId > 0) {
            model = new Object();
            model.maskingName = focusedMakeModel.payload.modelMaskingName;
            model.id = focusedMakeModel.payload.modelId;
        }
        return MakeModelRedirection(make, model);
    }

	// global city popup autocomplete
    $("#globalCityPopUp").bw_autocomplete({
        recordCount: 5,
        source: 3,
        onClear: function () {
            objCity = new Object();
        },
        click: function (event, ui, orgTxt) {
            var city = new Object();
            city.cityId = ui.item.payload.cityId;
            city.maskingName = ui.item.payload.cityMaskingName;
            var CookieValue = city.cityId + "_" + ui.item.label, oneYear = 365;
            SetCookieInDays("location", CookieValue, oneYear);
            CloseCityPopUp();
            showGlobalCity(ui.item.label);
        },
        open: function (result) {
            objCity.result = result;
        },
        focusout: function () {
            if ($('li.ui-state-focus a:visible').text() != "") {
                focusedCity = new Object();
                focusedCity = objCity.result[$('li.ui-state-focus').index()];
            }
        },
        afterfetch: function (result, searchtext) {
            var element = $("#globalCityPopUp");
            if (result != undefined && result.length > 0)
                showHideMatchError(element,false);
            else
                showHideMatchError(element,true);
        }
    }).autocomplete("widget").addClass("globalCity-autocomplete").css({ 'z-index': '11','font-weight':'normal','text-align':'left' }).insertAfter(".ui-autocomplete-input");

	
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

	$(".global-location").click( function(){
	    $("#globalcity-popup").show();
	    lockPopup();
	    CheckGlobalCookie();
	});
	$(".blackOut-window").mouseup(function(e){
		var globalLocation = $("#globalcity-popup"); 
        if(e.target.id !== globalLocation.attr('id') && !globalLocation.has(e.target).length)		
        {
		    globalLocation.hide();
		    unlockPopup();
		    if (!isCookieExists("location"))
		        SetCookieInDays("location", "0", 365);
        }
	});
	
    //making card clickable 
	$('li.card').click(function () {
	    $(this).find('a')[0].click();
	});


	// nav bar code starts
	$(".navbarBtn").click(function(){
		navbarShow();
	});
	function navbarShow() {
		$('body').addClass('lock-browser-scroll');
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
		$('body').addClass('lock-browser-scroll');
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
	//function lockPopup() {
	//	$('body').addClass('lock-browser-scroll');
	//	$(".blackOut-window").show();		
	//}
	//function unlockPopup() {
	//	$('body').removeClass('lock-browser-scroll');
	//	$(".blackOut-window").hide();
	//}	
	
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

	function MakeModelRedirection(make, model) {
	    if (model != null && model != undefined) {
	        window.location.href = "/m/" + make.maskingName + "-bikes/" + model.maskingName + "/";
	        return true;
	    }
	    else if (make != null && make != undefined) {
	        window.location.href = "/m/" + make.maskingName + "-bikes/";
	        return true;
	    }
	}


	$('#btnGlobalCityPopup').on('click', function () {
	    ele = $('#globalCityPopUp');
	    if (globalCityId > 0 && ($(ele).val()) != "") {
	        showHideMatchError(ele, false);
	        CloseCityPopUp();
	    }
	    else {
	        showHideMatchError(ele, true);
	        //unlockPopup();
	    }
	    return false;
	});

});


(function ($) {
    $.fn.hint = function (blurClass) {
        if (!blurClass) {
            blurClass = 'blur';
        }

        return this.each(function () {
            // get jQuery version of 'this'
            var $input = $(this),

            // capture the rest of the variable to allow for reuse
              title = $input.attr('placeholder'),
              $form = $(this.form),
              $win = $(window);

            function remove() {
                if ($input.val() === title && $input.hasClass(blurClass)) {
                    $input.val('').removeClass(blurClass);
                }
            }

            // only apply logic if the element has the attribute
            if (title) {
                // on blur, set value to title attr if text is blank
                $input.blur(function () {
                    if (this.value === '') {
                        $input.val(title).addClass(blurClass);
                    }
                }).focus(remove).blur(); // now change all inputs to title

                // clear the pre-defined text when form is submitted
                $form.submit(remove);
                $win.unload(remove); // handles Firefox's autocomplete
            }
        });
    };

})(jQuery);
(function ($) {
    $.fn.bw_autocomplete = function (options) {
        return this.each(function () {
            if (options == null || options == undefined) {
                console.log("cwsearch: please define options");
                return;
            }
            else if (options.source == null || options.source == undefined || options.source == '') {
                console.log("cwsearch: please define source");
                return;
            }
            var cache = new Object();
            var reqTerm;
            var orgTerm;
            if ($(this).attr('placeholder') != undefined)
                $(this).hint();
            var result;
            $(this).focusout(function () {
                if (options.focusout != undefined)
                    options.focusout();
            })
            $(this).keyup(function () {
                if (options.keyup != undefined)
                    options.keyup();
            })
            $(this).autocomplete({
                autoFocus: true,
                source: function (request, response) {
                    orgTerm = request.term;
                    reqTerm = request.term.replace(/^\s\s*/, '').replace(/\s\s*$/, '').replace(/-/g, ' ').replace(/[^A-Za-z0-9 ]/g, '').toLowerCase();
                    
                    var year = options.year;
                    if (year != null && year != undefined && year != '')
                        year = year.val();
                    else
                        year = '';

                    cacheProp = reqTerm + '_' + year;
                    if (!(cacheProp in cache) && reqTerm.length > 0) {
                        $(".fa-spinner").show();
                        var indexToHit = options.source;
                        var count = options.recordCount;
                        var path = "/api/AutoSuggest?source=" + indexToHit + "&inputText=" + encodeURIComponent(reqTerm) + "&noofrecords=" + count;

                        cache[cacheProp] = new Array();
                        $.ajax({
                            async: true, type: "GET", contentType: "application/json; charset=utf-8", dataType: "json",
                            url: path,
                            success: function (jsonData) {
                                $(".fa-spinner").hide();
                                jsonData = jsonData.suggestionList;
                                cache[reqTerm + '_' + year] = $.map(jsonData, function (item) {
                                    return { label: item.text, payload: item.payload }
                                });
                                result = cache[cacheProp];
                                response(cache[cacheProp]);
                                if (options.afterfetch != null && typeof (options.afterfetch) == "function") options.afterfetch(result, reqTerm);
                            },
                            error: function (error) {
                                result = undefined;
                                options.afterfetch(result, reqTerm);
                                response(cache[cacheProp]);
                            }
                        });
                    }
                    else {
                        result = cache[cacheProp];
                        response(cache[cacheProp]);
                        if (options.afterfetch != null && typeof (options.afterfetch) == "function") options.afterfetch(result, reqTerm);
                    }
                },
                minLength: 1,
                select: function (event, ui) {
                    if (options.click != undefined)
                        options.click(event, ui, $(this).val());
                },
                open: function (event, ui) {
                    if (!isNaN(options.width)) {
                        $('.ui-menu').width(options.width);
                    }
                    if (options.open != undefined)
                        options.open(result);
                }
            }).data("ui-autocomplete")._renderItem = function (ul, item) {
                return $("<li>")
                  .data("ui-autocomplete-item", item)
                  .append('<a OptionName=' + item.label.replace(/\s/g, '').toLowerCase() + '>' + __highlight(item.label, reqTerm) + '</a>')
                  .appendTo(ul);
            };
            function __highlight(s, t) {
                var matcher = new RegExp("(" + $.ui.autocomplete.escapeRegex(t) + ")", "ig");
                return s.replace(matcher, "<strong>$1</strong>");
            }
            function __getValue(key, value) {
                if (value != null && value != undefined && value != '')
                    return key + ':' + value + ';';
                else
                    return '';
            }
            $(this).keyup(function (e) {
                if ($(this).val().replace(/\s/g, '').length == 0 && options.onClear != undefined) {
                    options.onClear();
                }
            });
            $(this).keypress(function (e) {
                if (e.keyCode == 13) {
                    e.preventDefault();
                }
            });
        });
    };
})(jQuery);

function SetCookie(cookieName, cookieValue) {
    document.cookie = cookieName + "=" + cookieValue + '; path =/';
}

function SetCookieInDays(cookieName, cookieValue, nDays) {
    var today = new Date();
    var expire = new Date();
    expire.setTime(today.getTime() + 3600000 * 24 * nDays);
    document.cookie = cookieName + "=" + cookieValue + ";expires=" + expire.toGMTString() + '; path =/';
}

function getCookie(key) {
    var keyValue = document.cookie.match('(^|;) ?' + key + '=([^;]*)(;|$)');
    return keyValue ? keyValue[2] : null;
}

function isCookieExists(cookiename) {
    var coockieVal = $.cookie(cookiename);
    if (coockieVal == undefined || coockieVal == null || coockieVal == "-1" || coockieVal == "")
        return false;
    return true;
}

function showHideMatchError(element, error) {
    if (error) {
        element.parent().find('.error-icon').removeClass('hide');
        element.parent().find('.bw-blackbg-tooltip').removeClass('hide');
        element.addClass('border-red')
    }
    else {
        element.parent().find('.error-icon').addClass('hide');
        element.parent().find('.bw-blackbg-tooltip').addClass('hide');
        element.removeClass('border-red');
    }
}

function showGlobalCity(cityName) {
    $(".gl-default-stage").show();
    $('#cityName').text(cityName);
}

function CloseCityPopUp() {
    var globalLocation = $("#globalcity-popup");
    globalLocation.removeClass("show").addClass("hide");
    unlockPopup();
}

function CheckGlobalCookie() {
    var cookieName = "location";
    if (isCookieExists(cookieName)) {
        var locationCookie = getCookie(cookieName).split("_");
        var cityName = locationCookie[1];
        globalCityId = parseInt(locationCookie[0]);
        showGlobalCity(cityName);
        showHideMatchError($("#globalCityPopUp"), false);
        $(".fa-spinner").hide();
        $("#globalCityPopUp").val(cityName);
    }
    else
    {
        showHideMatchError($("#globalCityPopUp"), false);
        $("#globalcity-popup").show();
        lockPopup();
    }
}


function lockPopup() {
    $('body').addClass('lock-browser-scroll');
    $(".blackOut-window").show();
}
function unlockPopup() {
    $('body').removeClass('lock-browser-scroll');
    $(".blackOut-window").hide();
}
/*function lockPopup() {
    $(".blackOut-window").show();
}

function unlockPopup() {
    $(".blackOut-window").hide();
}*/





