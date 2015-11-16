// JavaScript Document
var pqCookieObj = {
PQCitySelectedId: 0,
PQAreaSelectedId : 0,
PQCitySelectedName: "",
PQAreaSelectedName:""
};
var temptotalPrice = 0;
var viewModel;
var priceBlock = $('#dvBikePrice');
var mainCity = $("#mainCity");
var cityAreaContainer = $("#city-area-select-container");
var otherBtn = $(cityAreaContainer).find("span.city-other-btn")[0];
var cityList = $("#city-list-container");
var citySelect = $(".city-select");
var areaSelect = $(".area-select"); 
var ctrlSelectCity = $("#ddlCity");
var ctrlSelectArea = $("#ddlArea");
var editBtn = $(cityAreaContainer).find(".city-edit-btn")[0];
var onRoadPriceText = $(cityAreaContainer).find(".city-onRoad-price-container")[0];
//offers section
var offersBlock = $("#offersBlock");
var offerBtnContainer = $(offersBlock).find("div.unveil-offer-btn-container");
var offerBtn = $(offerBtnContainer).find(".unveil-offer-btn");
var offerError = $(".offer-error");

var bikePrice = $("#bike-price");
var showroomPrice = $(".default-showroom-text");
var temptotalPrice = $(bikePrice).text();
var leadBtnBookNow = $("#leadBtnBookNow"),
    leadCapturePopup = $("#leadCapturePopup");




function pqViewModel(modelId, cityId) {
    var self = this;
    self.cities = ko.observableArray([]);
    self.areas = ko.observableArray([]);
    self.selectedCity = ko.observable(cityId);
    self.selectedArea = ko.observable();
    self.selectedModel = ko.observable(modelId);
    self.CustomerVM = ko.observable(new CustomerModel());
    self.priceQuote = ko.observable();
    self.DealerPriceList = ko.observableArray([]);
    self.BWPriceList = ko.observable();
    self.popularCityClicked = ko.observable(false);
    self.isDealerPQAvailable = ko.observable(false);
    self.FormatPricedata = function (item) {
        if (item != undefined)
            return formatPrice(item);
        return "";
    };

    self.DealerOnRoadPrice = ko.computed(function () {
        var total = 0;
        for (i = 0; i < self.DealerPriceList().length; i++) {
            total += self.DealerPriceList()[i].price;
        }
        return total;
    }, this);
    self.LoadCity = function () {
        self.selectedCity(undefined);
        self.cities("");
        loadCity(self);
    };

    self.FetchPriceQuote = function () {
        fetchPriceQuote(self);
    };

    self.selectedCity.subscribe(function () {
        self.areas("");
        self.selectedArea(undefined);
        if (self.selectedCity() != undefined)
        {
            loadArea(self);            
            var selectedCity = $('#ddlCity :selected').text();
            if ($('#ddlCity :selected').index() != 0) {
                dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Model_Page', 'act': 'City_Selected', 'lab': selectedCity });
            }             
        }
        else {
            $(offerBtn).show();
        }
        
    });

    
    self.selectedArea.subscribe(function () {
        if (self.selectedArea() != undefined && self.selectedArea() != 0) {
            fetchPriceQuote(self);
            var selectedArea = $('#ddlArea :selected').text();
            if ($('#ddlArea :selected').index() != 0) {
                dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Model_Page', 'act': 'Area_Selected', 'lab': selectedArea });
            }
        }
        else {
            $(offerBtn).show();
        }
    });

    self.availOfferBtn = function () {
        var city_area = GetGlobalCityArea();
        if (self.priceQuote() && self.priceQuote().IsDealerPriceAvailable && self.priceQuote().dealerPriceQuote.offers.length > 0) {
            dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Model_Page', 'act': 'Avail_Offers_Clicked', 'lab': myBikeName });
        }
        else
        {
            dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Model_Page', 'act': 'Book_Now_Clicked', 'lab': city_area });
        }
        window.location.href = "/pricequote/bookingsummary_new.aspx";
        return false;
    };

    self.notifyAvailable = function () {
        $(".notifyAvailabilityContainer").show();
        $(".blackOut-window").show();
    };

    self.IsValidManufacturer = ko.computed(function () {
        if (self.selectedModel() == 395)
            if (self.selectedCity() != 1 && self.selectedCity() != 12 && self.selectedCity() != 2)
                if (self.priceQuote() && !self.priceQuote().IsDealerPriceAvailable && self.priceQuote().bwPriceQuote.onRoadPrice > 0)
                    return true;
        return false;

    },this);

    self.termsConditions = function (entity) {
        if (entity != null && entity.offerId != 0) {
            LoadTerms(entity.offerId);
        }
    };

    self.captureLead = ko.computed(function () {
        state = false;
        if(self.priceQuote() && self.priceQuote().IsDealerPriceAvailable && (self.priceQuote().dealerPriceQuote.offers.length <= 0))
        {
            var v = self.priceQuote().dealerPriceQuote.varients;
            $.each(v,function () {
                if (this.version.versionId === self.priceQuote().priceQuote.versionId && this.bookingAmount <= 0)
                     state = true;
            });
        }
        return state;
    });

    self.showBookNow = ko.computed(function () {
        state = false;
        if (self.priceQuote() && self.priceQuote().IsDealerPriceAvailable && (self.priceQuote().dealerPriceQuote.offers.length <= 0)) {
            var v = self.priceQuote().dealerPriceQuote.varients;
            $.each(v, function () {
                if (this.version.versionId === self.priceQuote().priceQuote.versionId && this.bookingAmount > 0)
                    state = true;
            });
        }
        return state;
    });

    self.showLeadForm = function () {
            
        $("#leadCapturePopup").show();
        $('body').addClass('lock-browser-scroll');
        $(".blackOut-window-model").show();

        $(".leadCapture-close-btn, .blackOut-window-model").on("click", function () {
            leadCapturePopup.hide();
            $('body').removeClass('lock-browser-scroll');
            $(".blackOut-window-model").hide();
        });
    };

    

}


function CustomerModel() {
    var arr = setuserDetails();
    var self = this;
    if (arr != null && arr.length > 0) {
        self.firstName = ko.observable(arr[0]);
        self.lastName = ko.observable(arr[1]);
        self.emailId = ko.observable(arr[2]);
        self.mobileNo = ko.observable(arr[3]);
    }
    else {
        self.firstName = ko.observable();
        self.lastName = ko.observable();
        self.emailId = ko.observable();
        self.mobileNo = ko.observable();
    }
    self.IsVerified = ko.observable();
    self.NoOfAttempts = ko.observable(0);
    self.IsValid = ko.computed(function () { return self.IsVerified(); }, this);
    self.otpCode = ko.observable();
    self.fullName = ko.computed(function () {
        var _firstName = self.firstName() != undefined ? self.firstName() : "";
        var _lastName = self.lastName() != undefined ? self.lastName() : "";
        return _firstName + ' ' + _lastName;
    }, this);
    self.verifyCustomer = function () {
        if (!self.IsVerified()) {
            var objCust = {
                "dealerId": viewModel.priceQuote().priceQuote.dealerId,
                "pqId": viewModel.priceQuote().priceQuote.quoteId,
                "customerName": viewModel.CustomerVM().fullName,
                "customerMobile": viewModel.CustomerVM().mobileNo,
                "customerEmail": viewModel.CustomerVM().emailId,
                "clientIP": clientIP,
                "pageUrl": pageUrl,
                "versionId": viewModel.priceQuote().priceQuote.versionId,
                "cityId": viewModel.selectedCity()
            }
            $.ajax({
                type: "POST",
                url: "/api/PQCustomerDetail/",
                data: ko.toJSON(objCust),
                async: false,
                contentType: "application/json",
                success: function (response) {
                    var obj = ko.toJS(response);
                    self.IsVerified(obj.isSuccess);
                    if (self.IsVerified()) {                                    
                    }
                    else {
                        self.NoOfAttempts(obj.noOfAttempts);
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    self.IsVerified(false);
                }
            });
        }
    };
    self.generateOTP = function () {
        if (!self.IsVerified()) {
            var objCust = {
                "pqId": viewModel.priceQuote().priceQuote.quoteId,
                "customerMobile": viewModel.CustomerVM().mobileNo,
                "customerEmail": viewModel.CustomerVM().emailId,
                "cwiCode": viewModel.CustomerVM().otpCode,
                "branchId": viewModel.priceQuote().priceQuote.dealerId,
                "customerName": viewModel.CustomerVM().fullName,
                "versionId": viewModel.priceQuote().priceQuote.versionId,
                "cityId": viewModel.selectedCity()
            }
            $.ajax({
                type: "POST",
                url: "/api/PQMobileVerification/",
                data: ko.toJSON(objCust),
                async: false,
                contentType: "application/json",
                success: function (response) {
                    var obj = ko.toJS(response);
                    self.IsVerified(obj.isSuccess);
                    
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    self.IsVerified(false);
                }
            });
        }
    }
    self.regenerateOTP = function () {
        if (self.NoOfAttempts() <= 2 && !self.IsVerified()) {
            var url = '/api/ResendVerificationCode/';
            var objCustomer = {
                "customerName": self.fullName(),
                "customerMobile": self.mobileNo(),
                "customerEmail": self.emailId(),
                "source": 1
            }
            $.ajax({
                type: "POST",
                url: url,
                async: false,
                data: ko.toJSON(objCustomer),
                contentType: "application/json",
                success: function (response) {
                    self.IsVerified(false);
                    self.NoOfAttempts(response.noOfAttempts);
                    alert("You will receive the new OTP via SMS shortly.");
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    self.IsVerified(false);
                }
            });
        }
    }
    self.fullName = ko.computed(function () {
        var _firstName = self.firstName() != undefined ? self.firstName() : "";
        var _lastName = self.lastName() != undefined ? self.lastName() : "";
        return _firstName + ' ' + _lastName;
    }, this);
}

//for jquery chosen 
ko.bindingHandlers.chosen = {
    init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
        var $element = $(element);
        var options = ko.unwrap(valueAccessor());  
        if (typeof options === 'object')
            $element.chosen(options);            

        ['options', 'selectedOptions', 'value'].forEach(function (propName) {
            if (allBindings.has(propName)) {
                var prop = allBindings.get(propName);
                if (ko.isObservable(prop)) {
                    prop.subscribe(function () {
                        $element.trigger('chosen:updated');
                    });
                }
            }
        });
    }
}

function loadCity(vm) {
    $(ctrlSelectCity).prop('disabled', true).prev().show();
    if (vm.selectedModel()) {
        $.get("/api/PQCityList/?modelId=" + vm.selectedModel(),
            function (data) {               
                if (data) {
                    insertModelCitySeparator(data.cities);
                    var city = ko.toJS(data);
                    vm.cities(city.cities);
                    ctrlSelectCity = $("#ddlCity");
                    PQcheckCookies();
                    if (!isNaN(pqCookieObj.PQCitySelectedId) && pqCookieObj.PQCitySelectedId > 0 && vm.cities() && selectElementFromArray(vm.cities(), pqCookieObj.PQCitySelectedId)) {
                        vm.selectedCity(pqCookieObj.PQCitySelectedId);
                        vm.popularCityClicked(true);
                        pqCookieObj.PQCitySelectedId = 0;
                    }
                    $(ctrlSelectCity).prev().hide();
                    ctrlSelectCity.find("option[value='0']").prop('disabled', true);
                    ctrlSelectCity.trigger('chosen:updated');
                }
            });
    }
}

function loadArea(vm) {
    $(ctrlSelectArea).prev().show();
    if (vm.selectedCity()!=undefined) {
        $.ajax({
            url: "/api/PQAreaList/?modelId=" + vm.selectedModel() + "&cityId=" + vm.selectedCity(),
            type: "GET",
            contentType: "application/json",
        }).done(function (data) {
            if (data) {
                var area = ko.toJS(data);
                vm.areas(area.areas);
                ctrlSelectArea = $("#ddlArea");
                $(".city-select-text").hide();
                $(offerBtnContainer).show();
                if (!isNaN(pqCookieObj.PQAreaSelectedId) && pqCookieObj.PQAreaSelectedId > 0 && vm.areas() && selectElementFromArray(vm.areas(), pqCookieObj.PQAreaSelectedId)) {
                    vm.selectedArea(pqCookieObj.PQAreaSelectedId);
                    vm.popularCityClicked(true);                    
                    pqCookieObj.PQAreaSelectedId = 0;
                }
                $(ctrlSelectArea).prev().hide();
            }
            else {
                vm.areas([]);
                $(ctrlSelectArea).prev().hide();
                vm.FetchPriceQuote();
            }
        })
        .fail(function () {
            //no areas available;
            vm.areas([]);
            $(ctrlSelectArea).prev().hide();
            vm.FetchPriceQuote();
        });
    }
    else {
        vm.areas([]);
        //$(ctrlSelectArea).trigger("chosen:updated");
        $(".available-offers-container").hide();
        $(offerBtnContainer).show();
        $(ctrlSelectArea).prev().hide();
        $(".city-select-text").removeClass("text-red").show();
    }
}

function fetchPriceQuote(vm) {
    $(priceBlock).find("span.price-loader").show();
    //$("#dvAvailableOffer").empty();
    if (vm.selectedModel()!=undefined && vm.selectedCity()!=undefined) {
        $.ajax({
            url: "/api/OnRoadPrice/?cityId=" + vm.selectedCity() + "&modelId=" + vm.selectedModel() + "&clientIP=" + clientIP + "&sourceType=" + 1 + "&areaId=" + (vm.selectedArea() != undefined ? vm.selectedArea() : 0),
            type: "GET",
            contentType: "application/json",
        }).done(function (data) {
            if (data) {
                var pq = ko.toJS(data);
                var cityName = $(ctrlSelectCity).find("option[value=" + vm.selectedCity() + "]").text();
                vm.priceQuote(pq);
                vm.isDealerPQAvailable(pq.IsDealerPriceAvailable);
                if (pq.IsDealerPriceAvailable) {
                    vm.DealerPriceList(pq.dealerPriceQuote.priceList);
                    $.each(pq.bwPriceQuote.varients, function () {
                        $("#price_" + this.versionId.toString()).text(this.onRoadPrice ? formatPrice(this.onRoadPrice) : "NA");
                        $("#locprice_" + this.versionId.toString()).text("On-road price, " + cityName);
                    });
                    $.each(pq.dealerPriceQuote.varients,function () {
                        $("#price_" + this.version.versionId.toString()).text(this.onRoadPrice ? formatPrice((this.onRoadPrice - pq.insuranceAmount)) : "NA");
                        $("#locprice_" + this.version.versionId.toString()).text("On-road price, " + cityName);
                    });
                }
                else {
                    vm.BWPriceList(pq.bwPriceQuote);
                    $.each(pq.bwPriceQuote.varients, function () {                        
                        $("#price_" + this.versionId.toString()).text(this.onRoadPrice ? formatPrice(this.onRoadPrice) : "NA");
                        $("#locprice_" + this.versionId.toString()).text("On-road price, " + cityName);
                    });
                }
                if (vm.areas().length > 0 && pq && pq.IsDealerPriceAvailable) {
                    var cookieValue = "CityId=" + vm.selectedCity() + "&AreaId=" + vm.selectedArea() + "&PQId=" + pq.priceQuote.quoteId + "&VersionId=" + pq.priceQuote.versionId + "&DealerId=" + pq.priceQuote.dealerId;
                    SetCookie("_MPQ", cookieValue);                              
                    $(offerBtnContainer).hide();
                    temptotalPrice = checkNumeric($(bikePrice).text());
                    var totalPrice = 0;
                    var priceBreakText = '';
                    $.each(pq.dealerPriceQuote.priceList, function () {
                        totalPrice += this.price;
                        priceBreakText += this.categoryName + " + ";
                    });
                    priceBreakText = priceBreakText.substring(0, priceBreakText.length - 3);
                    if (pq.isInsuranceFree && pq.insuranceAmount > 0)
                        totalPrice -= pq.insuranceAmount;

                    if (totalPrice <= 0) {
                        $($(".bike-price-container")[0]).hide();
                        $($(".bike-price-container")[1]).show();
                    }
                    else {
                        $($(".bike-price-container")[1]).hide();
                        $($(".bike-price-container")[0]).show();
                    }
                    if (pq.dealerPriceQuote.offers.length > 0) {
                        dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Model_Page', 'act': 'Avail_Offer_Shown', 'lab': myBikeName });
                    }
                    animatePrice($(bikePrice), temptotalPrice, totalPrice);
                    $("#breakup").text("(" + priceBreakText + ")");
                    $("#pqCity").html($(ctrlSelectCity).find("option[value=" + vm.selectedCity() + "]").text());
                    $("#pqArea").html($(ctrlSelectArea).find("option[value=" + vm.selectedArea() + "]").text() + ', ');
                    $(".area-select-text").hide();                     
                    $(".city-area-wrapper").hide();                                
                    $(".city-select-text").hide();
                    $(".city-onRoad-price-container").show();
                    $('.available-offers-container').show();
                    $(".default-showroom-text").html("View Breakup").addClass('view-breakup-text');

                    setLocationCookie($(ctrlSelectCity).find('option:selected'), $(ctrlSelectArea).find('option:selected'));
                }
                else {
                    temptotalPrice = checkNumeric($(bikePrice).text());
                    totalPrice = pq.bwPriceQuote.onRoadPrice;
                    priceBreakText = "Ex-showroom + Insurance + RTO"; 
                    $("#breakup").text("(" + priceBreakText + ")");

                    if (totalPrice <= 0) {
                        $($(".bike-price-container")[0]).hide();
                        $($(".bike-price-container")[1]).show();
                    }
                    else {
                        $($(".bike-price-container")[1]).hide();
                        $($(".bike-price-container")[0]).show();
                    }
                    animatePrice($(bikePrice), temptotalPrice, totalPrice);
                    $("#pqCity").html($(ctrlSelectCity).find("option[value=" + vm.selectedCity() + "]").text());
                    
                    if (!vm.IsDealerPriceAvailable && vm.selectedArea() == undefined)
                        $("#pqArea").html("");
                    else $("#pqArea").html($(ctrlSelectArea).find("option[value=" + vm.selectedArea() + "]").text() + ', ');
                    
                    $(".city-area-wrapper").hide();
                    $(".city-select-text").hide();
                    $(".area-select-text").hide();
                    $(".city-onRoad-price-container").show();
                    $(offerBtnContainer).hide();
                    $(".available-offers-container").show();                    
                    setLocationCookie($(ctrlSelectCity).find('option:selected'), $(ctrlSelectArea).find('option:selected'));
                }
                $(".default-showroom-text").html("View Breakup").addClass('view-breakup-text');
                $(priceBlock).find("span.price-loader").hide();
            }
            else {
                vm.areas([]);
                $(priceBlock).find("span.price-loader").hide();
                pqAreaFailStatus();
            }
        })
        .fail(function () {
            vm.areas([]);
            $(priceBlock).find("span.price-loader").hide();
            pqAreaFailStatus();
        });
    }
}

$(document).ready(function () {

    $.fn.shake = function (options) {
        // defaults
        var settings = {
            'shakes': 2,
            'distance': 10,
            'duration': 400
        };
        // merge options
        if (options) {
            $.extend(settings, options);
        }
        // make it so
        var pos;
        return this.each(function () {
            $this = $(this);
            // position if necessary
            pos = $this.css('position');
            if (!pos || pos === 'static') {
                $this.css('position', 'relative');
            }
            // shake it
            for (var x = 1; x <= settings.shakes; x++) {
                $this.animate({ left: settings.distance * -1 }, (settings.duration / settings.shakes) / 4)
                    .animate({ left: settings.distance }, (settings.duration / settings.shakes) / 2)
                    .animate({ left: 0 }, (settings.duration / settings.shakes) / 4);
            }
        });
    };

    $(offerBtnContainer).show();
    $(offerBtn).click(function () {
        if ( viewModel.cities() && viewModel.selectedCity() === undefined ) {
            $('.offer-error').addClass("text-red").shake();
            $('.city-select-text').addClass("text-red").shake();
        }
        else if(viewModel.selectedCity != undefined && viewModel.areas() || viewModel.selectedArea() === undefined)
        {
            $('.area-select-text').addClass("text-red").shake();
        }
    });


    function transferEffect(target)
    {
        $(this).effect('transfer', { to: $(target) }, 1000);
    }

    $(priceBlock).delegate('#mainCity li', 'click', function () {
        var val = $(this).attr('cityId');
        $(".offer-error").hide();
        $(".city-onRoad-price-container").hide();
        $(offerBtnContainer).show();
        $(priceBlock).find("#city-list-container").hide();
        $(cityAreaContainer).show();        
        $('.offer-error').removeClass("text-red");
        viewModel.popularCityClicked(true);
        if (val) {
            $(ctrlSelectCity).find(" option[value=" + val + "]").prop('selected', 'selected');
            $(ctrlSelectCity).trigger('change');
        }
    });

    $(editBtn).on('click',function (e) {
        $(".city-onRoad-price-container").hide();
        $(".city-area-select-container").show();
        $(priceBlock).find("#ddlCity_chosen").show();
        $(".city-area-wrapper").show();

        if (viewModel.areas() && viewModel.selectedArea())
        {
            $(".available-offers-container").removeClass("text-red").show();
            $('.area-select-text').removeClass("text-red").show();
            $(priceBlock).find("#ddlArea_chosen").trigger('chosen-updated').show();
        }
        else
        {
            $(".available-offers-container").hide();
            $(offerBtnContainer).show();
            $(".city-select-text").removeClass("text-red").show(); 
        }        
            
               
    });

    $("a.read-more-btn").click(function () {
        if(!$(this).hasClass("open")) {
            $(".model-about-main").hide();
            $(".model-about-more-desc").show();
            var a = $(this).find("span");
            a.text(a.text() === "more" ? "less" : "more");
            $(this).addClass("open");
        }
        else if($(this).hasClass("open")) {
            $(".model-about-main").show();
            $(".model-about-more-desc").hide();
            var a = $(this).find("span");
            a.text(a.text() === "more" ? "less" : "more");
            $(this).removeClass("open");
        }
        
    });

   

    $(".more-features-btn").click(function () {
        $(".more-features").slideToggle();
        $("html, body").animate({scrollTop: $("#features").offset().top }, 1000);
        var a = $(this).find("span");
        a.text(a.text() === "+" ? "-" : "+");
    });

    $(priceBlock).on('click', 'span.view-breakup-text', function () {
        $("div#breakupPopUpContainer").show();
        $(".blackOut-window").show();
    });

    $(".breakupCloseBtn,.blackOut-window").on('mouseup click',function (e) {         
        $("div#breakupPopUpContainer").hide();
        $(".blackOut-window").hide();        
    });

    $(".termsPopUpCloseBtn,.blackOut-window").on('mouseup click', function (e) {
        $("div#termsPopUpContainer").hide();
        $(".blackOut-window").hide();
    });


    $(document).on('keydown', function (e) {
        if (e.keyCode === 27) {
            $("div.breakupCloseBtn").click();
            $("div.termsPopUpCloseBtn").click();
            $("div.leadCapture-close-btn").click();
        }
    });

});

function LoadTerms(offerId) {
    $(".termsPopUpContainer").css('height', '150')
    $('#termspinner').show();
    $('#terms').empty();
    $("div#termsPopUpContainer").show();
    $(".blackOut-window").show();

    var url = abHostUrl + "/api/DealerPriceQuote/GetOfferTerms?offerMaskingName=&offerId=" + offerId;
    if (offerId != '' && offerId != null) {
        $.ajax({
            type: "GET",
            url: abHostUrl + "/api/DealerPriceQuote/GetOfferTerms?offerMaskingName=&offerId=" + offerId,
            dataType: 'json',
            success: function (response) {
                $(".termsPopUpContainer").css('height', '500')
                $('#termspinner').hide();
                if (response.html != null)
                    $('#terms').html(response.html);
            },
            error: function (request, status, error) {
                $("div#termsPopUpContainer").hide();
                $(".blackOut-window").hide();
            }
        });      
    }
    else {
        setTimeout(LoadTerms, 2000); // check again in a second
    }
}

//function to check pqCookies
function PQcheckCookies() {
    c = document.cookie.split('; ');
    for (i = c.length - 1; i >= 0; i--) {
        C = c[i].split('=');
        if (C[0] == "location") {
            var cData = (String(C[1])).split('_');
            pqCookieObj.PQCitySelectedId = parseInt(cData[0]);
            pqCookieObj.PQCitySelectedName = cData[1];
            pqCookieObj.PQAreaSelectedId = parseInt(cData[2]);
            pqCookieObj.PQAreaSelectedName = cData[3];
            
        }
    }
}

//photos corousel function
(function ($) {

    var connector = function (itemNavigation, carouselStage) {
        return carouselStage.jcarousel('items').eq(itemNavigation.index());
    };
    var connector2 = function (itemNavigation2, carouselStage2) {
        return carouselStage2.jcarousel('items').eq(itemNavigation2.index());
    };
    var connector3 = function (itemNavigation3, carouselStage3) {
        return carouselStage3.jcarousel('items').eq(itemNavigation3.index());
    };
    $(function () {
        var carouselStage = $('.carousel-stage').jcarousel();
        var carouselNavigation = $('.carousel-navigation').jcarousel();

        var carouselStage2 = $('.carousel-stage-photos').jcarousel();
        var carouselNavigation2 = $('.carousel-navigation-photos').jcarousel();

        var carouselStage3 = $('.carousel-stage-videos').jcarousel();
        var carouselNavigation3 = $('.carousel-navigation-videos').jcarousel();        

        carouselNavigation.jcarousel('items').each(function () {
            var item = $(this);
            var target = connector(item, carouselStage);
            item
                .on('jcarouselcontrol:active', function () {
                    carouselNavigation.jcarousel('scrollIntoView', this);
                    item.addClass('active');
                })
                .on('jcarouselcontrol:inactive', function () {
                    item.removeClass('active');
                })
                .jcarouselControl({
                    target: target,
                    carousel: carouselStage
                });
        });

        carouselNavigation2.jcarousel('items').each(function () {
            var item2 = $(this);
            var target = connector2(item2, carouselStage2);
            item2
				.on('jcarouselcontrol:active', function () {
				    carouselNavigation2.jcarousel('scrollIntoView', this);
				    item2.addClass('active');
				})
				.on('jcarouselcontrol:inactive', function () {
				    item2.removeClass('active');
				})
				.jcarouselControl({
				    target: target,
				    carousel: carouselStage2
				});
        });

        carouselNavigation3.jcarousel('items').each(function () {
            var item3 = $(this);
            var target = connector3(item3, carouselStage3);
            item3
				.on('jcarouselcontrol:active', function () {
				    carouselNavigation3.jcarousel('scrollIntoView', this);
				    item3.addClass('active');
				})
				.on('jcarouselcontrol:inactive', function () {
				    item3.removeClass('active');
				})
				.jcarouselControl({
				    target: target,
				    carousel: carouselStage3
				});
        });

        $('.prev-stage, .photos-prev-stage, .videos-prev-stage')
            .on('jcarouselcontrol:inactive', function () {
                $(this).addClass('inactive');
            })
            .on('jcarouselcontrol:active', function () {
                $(this).removeClass('inactive');
            })
            .jcarouselControl({
                target: '-=1'
            });
        $('.next-stage, .photos-next-stage, .videos-next-stage')
            .on('jcarouselcontrol:inactive', function () {
                $(this).addClass('inactive');
            })
            .on('jcarouselcontrol:active', function () {
                $(this).removeClass('inactive');
            })
            .jcarouselControl({
                target: '+=1'
            });
        $('.prev-navigation, .photos-prev-navigation, .videos-prev-navigation')
            .on('jcarouselcontrol:inactive', function () {
                $(this).addClass('inactive');
            })
            .on('jcarouselcontrol:active', function () {                
                $(this).removeClass('inactive');
            })
            .jcarouselControl({
                target: '-=4'
            });
        $('.next-navigation, .photos-next-navigation, .videos-next-navigation')
            .on('jcarouselcontrol:inactive', function () {
                $(this).addClass('inactive');
            })
            .on('jcarouselcontrol:active', function () {                
                $(this).removeClass('inactive');
            })
            .jcarouselControl({
                target: '+=4'
            });


        $(".carousel-navigation, .carousel-stage, .carousel-stage-photos, .carousel-navigation-photos").on('jcarousel:visiblein', 'li', function (event, carousel) {
            $(this).find("img.lazy").trigger("imgLazyLoad");
        });
        //$(".carousel-stage-photos").on('jcarousel:visiblein', 'li', function (event, carousel) {
        //    getImageIndex();
        //});       
    });
})(jQuery);

$(".photos-next-stage").click(function () {
    getImageNextIndex();
});

$(".photos-prev-stage").click(function () {
    getImagePrevIndex();
});

$(".carousel-navigation-photos").click(function () {
    getImageIndex();
});

$(".stage-photos").hover(function () {
    $(".photos-next-stage, .photos-prev-stage, .photos-prev-stage.inactive, .photos-next-stage.inactive").toggleClass("hide show");
});

$(".navigation-photos").hover(function () {
    $(".photos-prev-navigation, .photos-next-navigation, .photos-prev-navigation.inactive, .photos-next-navigation.inactive").toggleClass("hide show");
});

$(".stage-videos").hover(function () {
    $(".videos-next-stage, .videos-prev-stage, .videos-prev-stage.inactive, .videos-next-stage.inactive").toggleClass("hide show");
});

$(".navigation-videos").hover(function () {
    $(".videos-prev-navigation, .videos-next-navigation, .videos-prev-navigation.inactive, .videos-next-navigation.inactive").toggleClass("hide show");
});
function animatePrice(ele,start,end)
{
    $({ someValue: start }).stop(true).animate({ someValue: end }, {
        duration: 500,
        easing: 'easeInOutBounce',
        step: function () { 
            $(ele).text(formatPrice(Math.round(this.someValue)));
        }
    }).promise().done(function () {
        $(ele).text(formatPrice(end));
    });
}

//priceFormatter
function formatPrice(price) {
    var price = price.toString();
    var thMatch = /(\d+)(\d{3})$/;
    var thRest = thMatch.exec(price);
    if (!thRest) return price;
    return (thRest[1].replace(/\B(?=(\d{2})+(?!\d))/g, ",") + "," + thRest[2]);
}

function checkNumeric(str) {
    return parseInt(str.replace(/\,/g, ''));
}

//sections display properties for the area fail status
function pqAreaFailStatus() {
    $(offerBtnContainer).hide();
    $(".city-onRoad-price-container").hide();
    $(".city-select-text").show();
    $(".area-select-text").hide();
    $(".city-area-wrapper").show();
    $(".city-select").show();
    $(".area-select").hide();
    $('.available-offers-container').show();
    $(priceBlock).find("span.price-loader").hide();
}

$("#btnShowOffers").on("click", function () {
    dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Model_Page', 'act': 'Show_Offers_Clicked', 'lab': myBikeName });
});

$("#bikeBannerImageCarousel .stage li").click(function () {
    dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Model_Page', 'act': 'Photo_Clicked', 'lab': myBikeName });
    if (imgTotalCount > 0) {
        $('body').addClass('lock-browser-scroll');
        $(".blackOut-window-model").show();
        $(".bike-gallery-popup").removeClass("hide").addClass("show");
        $(".modelgallery-close-btn").removeClass("hide").addClass("show");
        $(".carousel-stage-photos ul li").slice(0, 3).find("img.lazy").trigger("imgLazyLoad");
        $(".carousel-navigation-photos ul li").slice(0, 5).find("img.lazy").trigger("imgLazyLoad");
        $(document).on("keydown", function (e) {
            var $blackModel = $(".blackOut-window-model");
            var $bikegallerypopup = $(".bike-gallery-popup");
            if ($bikegallerypopup.hasClass("show") && e.keyCode === 27) {
                $(".modelgallery-close-btn").click();
            }
            if ($bikegallerypopup.hasClass("show") && e.keyCode == 39 && $("#photos-tab").hasClass("active")) {
                $(".photos-next-stage").click();
            }
            if ($bikegallerypopup.hasClass("show") && e.keyCode == 37 && $("#photos-tab").hasClass("active")) {
                $(".photos-prev-stage").click();
            }
        });
    }    
});

$(".modelgallery-close-btn, .blackOut-window-model").click(function () {
    $('body').removeClass('lock-browser-scroll');
    $(".blackOut-window-model").hide();
    $(".bike-gallery-popup").removeClass("show").addClass("hide");
    $(".modelgallery-close-btn").removeClass("show").addClass("hide");
    videoiFrame.setAttribute("src", "");
    var galleryThumbIndex = $(".carousel-navigation-photos ul li.active").index();
    $(".carousel-stage").jcarousel('scroll', galleryThumbIndex);
});

$(document).ready(function () {
    getImageDetails();
});

var mainImgIndexA;

$(".carousel-stage ul li").click(function () {
    mainImgIndexA = $(".carousel-navigation ul li.active").index();
    setGalleryImage(mainImgIndexA);
});

var setGalleryImage = function (currentImgIndex) {
    $(".carousel-stage-photos").jcarousel('scroll', currentImgIndex);
    getImageDetails();
};

var getImageDetails = function () {
    imgTotalCount = $(".carousel-stage-photos ul li").length;
    var imgIndexA = $(".carousel-navigation-photos ul li.active");
    var imgIndex = imgIndexA.index() + 1;
    var imgTitle = imgIndexA.find("img").attr("title");
    setImageDetails(imgTitle, imgIndex);
};

var getImageNextIndex = function () {
    var imgIndexA = $(".carousel-navigation-photos ul li.active").next();
    var imgIndex = imgIndexA.index() + 1;
    var imgTitle = imgIndexA.find("img").attr("title");
    setImageDetails(imgTitle, imgIndex);
}

var getImagePrevIndex = function () {
    var imgIndexA = $(".carousel-navigation-photos ul li.active").prev();
    var imgIndex = imgIndexA.index() + 1;
    var imgTitle = imgIndexA.find("img").attr("title");    
    setImageDetails(imgTitle, imgIndex);
}

var getImageIndex = function () {
    var imgIndexA = $(".carousel-navigation-photos ul li.active");
    var imgIndex = imgIndexA.index() + 1;
    var imgTitle = imgIndexA.find("img").attr("title");
    setImageDetails(imgTitle, imgIndex);
}

var setImageDetails = function (imgTitle,imgIndex) {            
    $(".leftfloatbike-gallery-details").text(imgTitle);
    if (imgIndex > 0) {
        $(".bike-gallery-count").text(imgIndex.toString() + "/" + imgTotalCount.toString());
    }
}

var videoiFrame = document.getElementById("video-iframe");

/* first video src */
$("#photos-tab, #videos-tab").click(function () {
    firstVideo();
});

$("#videos-tab").click(function () {
    dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Model_Page', 'act': 'Video_Tab_Clicked', 'lab': myBikeName });
});

var firstVideo = function () {
    var a = $(".carousel-navigation-videos ul").first("li");
    var newSrc = a.find("img").attr("iframe-data");
    videoiFrame.setAttribute("src", newSrc);
};

var navigationVideosLI = $(".carousel-navigation-videos ul li");
navigationVideosLI.click(function () {
    navigationVideosLI.removeClass("active");
    $(this).addClass("active");
    var newSrc = $(this).find("img").attr("iframe-data");
    videoiFrame.setAttribute("src", newSrc);
});

function insertModelCitySeparator(response) {
    l = (response != null) ? response.length : 0;
    if (l > 0) {
        for (i = 0; i < l; i++) {
            if (!response[i].isPopular) {
                if (i > 0)
                    response.splice(i, 0, { cityId: 0, cityName: "--------------------",cityMaskingName:"",isPopular: false });
                break;
            }
        }
    }
}

//800x600
if ($(window).width() < 996 && $(window).width() > 790) {
    $(".bikeModel-user-ratings").removeClass("margin-left50").addClass("margin-left20");
    $(".modelgallery-close-btn").removeClass("pos-right20");
    $(".modelgallery-close-btn").css("right", "185px");
    $(".photos-videos-tabs").removeClass("margin-top20");
    if ($("#bike-gallery-popup .home-tabs").hasClass("hide"))
        $("#bike-gallery-popup").find("div.bike-gallery-heading").removeClass("margin-top90").addClass("margin-top40");
    $("#alternative-bikes-section .bikeTitle, #alternative-bikes-section .bikeStartPrice, #alternative-bikes-section .bikeShowroomName, #alternative-bikes-section .bikeSpecs, #alternative-bikes-section .fillPopupData").removeClass("margin-bottom10").addClass("margin-bottom5");
}

//lead
// JavaScript Document

var firstname = $("#getFirstName");
var lastname = $("#getLastName");
var emailid = $("#getEmailID");
var mobile = $("#getMobile");
var otpContainer = $(".mobile-verification-container");

var detailsSubmitBtn = $("#user-details-submit-btn");
var otpText = $("#getOTP");
var otpBtn = $("#otp-submit-btn");

var prevEmail = "";
var prevMobile = "";

detailsSubmitBtn.click(function () {
    if (ValidateUserDetail()) {
        viewModel.CustomerVM().verifyCustomer();
        if (viewModel.CustomerVM().IsValid()) {
            $("#personalInfo").hide();
            $(".call-for-queries").hide();

            window.location.href = "/pricequote/bookingsummary_new.aspx";
        }
        else {
            otpContainer.removeClass("hide").addClass("show");
            $(this).hide();
            nameValTrue();
            hideError(mobile);
            otpText.val('').removeClass("border-red");
            otpText.siblings("span, div").css("display", "none");
        }
        setPQUserCookie();
        var getCityArea = GetGlobalCityArea();
        dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Model_Page', 'act': 'Step_1_Successful_Submit', 'lab': getCityArea });
    }
});

var ValidateUserDetail = function () {

    var isValid = true;
    var getCityArea = GetGlobalCityArea();

    isValid = validateEmail(getCityArea);
    isValid &= validateMobile(getCityArea);
    isValid &= validateName(getCityArea);
    isValid &= validateLastName(getCityArea);
    if (!isValid) {
        $('#customize-tab').addClass('disabled-tab').removeClass('active-tab  text-bold');
        $('#confirmation-tab').addClass('disabled-tab').removeClass('active-tab text-bold');
    }
    return isValid;
};

var validateName = function (cityArea) {
    var isValid = true;
    var a = firstname.val().length;
    if (firstname.val().indexOf('&') != -1) {
        isValid = false;
        setError(firstname, 'Invalid name');
    }
    else if (a == 0) {
        isValid = false;
        setError(firstname, 'Please enter your first name');
    }
    else if (a >= 1) {
        isValid = true;
        nameValTrue()
    }
    if (!isValid) { dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Model Page', 'act': 'Step_1_Submit_Error_Name', 'lab': cityArea }); }
    return isValid;
}

var lastnameValTrue = function () {
    hideError(lastname)
    lastname.siblings("div").text('');
};
var nameValTrue = function () {
    hideError(firstname)
    firstname.siblings("div").text('');
};

firstname.on("focus", function () {
    hideError(firstname);
});

emailid.on("focus", function () {
    hideError(emailid);
    prevEmail = emailid.val().trim();
});

mobile.on("focus", function () {
    hideError(mobile)
    prevMobile = mobile.val().trim();

});

emailid.on("blur", function () {
    if (prevEmail != emailid.val().trim()) {
        var getCityArea = GetGlobalCityArea();
        if (validateEmail(getCityArea)) {
            viewModel.CustomerVM().IsVerified(false);
            detailsSubmitBtn.show();
            otpText.val('');
            otpContainer.removeClass("show").addClass("hide");
            hideError(emailid);
        }
        $('#confirmation-tab').addClass('disabled-tab').removeClass('active-tab text-bold');
        $('#customize-tab').addClass('disabled-tab').removeClass('active-tab text-bold');
    }
    else
        viewModel.CustomerVM().IsVerified(true);
});

mobile.on("blur", function () {
    if (prevMobile != mobile.val().trim()) {
        var getCityArea = GetGlobalCityArea();
        if (validateMobile(getCityArea)) {
            viewModel.CustomerVM().IsVerified(false);
            detailsSubmitBtn.show();
            otpText.val('');
            otpContainer.removeClass("show").addClass("hide");
            hideError(mobile);
        }
        $('#confirmation-tab').addClass('disabled-tab').removeClass('active-tab text-bold');
        $('#customize-tab').addClass('disabled-tab').removeClass('active-tab text-bold');
    }
    else
        viewModel.CustomerVM().IsVerified(true);

});

var mobileValTrue = function () {
    mobile.removeClass("border-red");
    mobile.siblings("span, div").hide();
};


otpText.on("focus", function () {
    otpText.val('');
    otpText.siblings("span, div").css("display", "none");
});

var setError = function (ele, msg) {
    ele.addClass("border-red");
    ele.siblings("span, div").show();
    ele.siblings("div").text(msg);
}

function hideError(ele) {
    ele.removeClass("border-red");
    ele.siblings("span, div").hide();
}
/* Email validation */
function validateEmail(cityArea) {
    var isValid = true;
    var emailID = emailid.val();
    var reEmail = /^[A-z0-9._+-]+@[A-z0-9.-]+\.[A-z]{2,6}$/;

    if (emailID == "") {
        setError(emailid, 'Please enter email address');
        isValid = false;
    }
    else if (!reEmail.test(emailID)) {
        setError(emailid, 'Invalid Email');
        isValid = false;
    }
    if (!isValid) { dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Model Page', 'act': 'Step_1_Submit_Error_Email', 'lab': cityArea }); }
    return isValid;
}

function validateMobile(cityArea) {
    var isValid = true;
    var reMobile = /^[0-9]{10}$/;
    var mobileNo = mobile.val();
    if (mobileNo == "") {
        isValid = false;
        setError(mobile, "Please enter your Mobile Number");
    }
    else if (!reMobile.test(mobileNo) && isValid) {
        isValid = false;
        setError(mobile, "Mobile Number should be 10 digits");
    }
    else {
        hideError(mobile)
    }
    if (!isValid) { dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Model Page', 'act': 'Step_1_Submit_Error_Mobile', 'lab': cityArea }); }
    return isValid;
}

var otpVal = function (msg) {
    otpText.addClass("border-red");
    otpText.siblings("span, div").css("display", "block");
    otpText.siblings("div").text(msg);
};

function validateOTP() {
    var retVal = true;
    var isNumber = /^[0-9]{5}$/;
    var cwiCode = otpText.val();
    viewModel.CustomerVM().IsVerified(false);
    if (cwiCode == "") {
        retVal = false;
        otpVal("Please enter your Verification Code");
    }
    else {
        if (isNaN(cwiCode)) {
            retVal = false;
            otpVal("Verification Code should be numeric");
        }
        else if (cwiCode.length != 5) {
            retVal = false;
            otpVal("Verification Code should be of 5 digits");
        }
    }
    return retVal;

}

otpBtn.click(function () {
    var isValid = true;
    var getCityArea = GetGlobalCityArea();
    isValid = validateEmail(getCityArea);
    isValid &= validateMobile(getCityArea);
    isValid &= validateName(getCityArea);
    isValid &= validateLastName(getCityArea);
    $('#processing').show();
    if (!validateOTP())
        $('#processing').hide();

    if (validateOTP() && isValid) {
        viewModel.CustomerVM().generateOTP();
        var getCityArea = GetGlobalCityArea();

        if (viewModel.CustomerVM().IsVerified()) {
           // $.customizeState();
            $("#personalInfo").hide();
            $(".booking-dealer-details").removeClass("hide").addClass("show");
            $('#processing').hide();

            detailsSubmitBtn.show();
            otpText.val('');
            otpContainer.removeClass("show").addClass("hide");

            // OTP Success
            dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Model_Page', 'act': 'Step_1_OTP_Successful_Submit', 'lab': getCityArea });

            window.location.href = "/pricequote/bookingsummary_new.aspx";

        }
        else {
            $('#processing').hide();
            otpVal("Please enter a valid OTP.");
            // push OTP invalid
            dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Model_Page', 'act': 'Step_1_OTP_Submit_Error', 'lab': getCityArea });
        }
    }
});

function setuserDetails() {
    var cookieName = "_PQUser";
    if (isCookieExists(cookieName)) {
        var arr = getCookie(cookieName).split("&");
        return arr;
    }
}

var validateLastName = function (cityArea) {
    var isValid = true;
    if (lastname.val().indexOf('&') != -1) {
        isValid = false;
        setError(lastname, 'Invalid name');
    }
    else {
        isValid = true;
        lastnameValTrue();
    }
    return isValid;
}

function setPQUserCookie() {
    var val = firstname.val() + '&' + lastname.val() + '&' + emailid.val() + '&' + mobile.val();
    SetCookie("_PQUser", val);
}
