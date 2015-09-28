// JavaScript Document

jQuery(function () {

    jQuery('.jcarousel-wrapper.model .jcarousel')
    .on('jcarousel:create jcarousel:reload', function () {
        var element = $(this),
            width = element.innerWidth();
        element.jcarousel('items').css('width', width + 'px');
    })
    .on('jcarousel:targetin', 'li', function () {
        $("img.lazy").lazyload({
            threshold: 200
        });
    });
    $(".jcarousel-pagination").click(function () {
        $("img.lazy").lazyload({
            threshold: 200
        });
    });
    
    $(".alternatives-carousel").on('jcarousel:visiblein', 'li', function (event, carousel) {
        $(this).find("img.lazy").trigger("imgLazyLoad");
    });

});

var otherBtn = $(".city-other-btn");
var cityAreaContainer = $("#city-area-select-container");
var cityList = $("#city-list-container");
var cityTabs = $("#city-list-container ul li");
var citySelect = $(".city-select");
var areaSelect = $(".area-select");
var offerBtnContainer = $(".unveil-offer-btn-container");
var offerBtn = $(".unveil-offer-btn");
var offerError = $(".offer-error");
var editBtn = $(".city-edit-btn")
var bikePrice = $("#bike-price");
var onRoadPriceText = $(".city-onRoad-price-container")
var showroomPrice = $(".default-showroom-text");
var PQCitySelectedId = 0;
var PQCitySelectedName = "";
var temptotalPrice = $(bikePrice).text();
var modelViewModel;

cityTabs.on('click', function () {
    cityTabs.removeClass('focused');
    $(this).addClass('focused');
});


otherBtn.click(function () {
    cityList.hide();
    $(".city-select-text").removeClass("hide").addClass("show");
    cityAreaContainer.removeClass("hide").addClass("show");
    offerError.removeClass("show").addClass("hide");
});


$(".more-features-btn").click(function () {
    $(".more-features").slideToggle();
    var a = $(this).find("span");
    a.text(a.text() === "+" ? "-" : "+");
});

$("a.read-more-btn").click(function () {
    $(".model-about-more-desc").slideToggle();
    $(".model-about-main").toggle();
    var a = $(this).find("span");
    a.text(a.text() === "more" ? "less" : "more");
});
/* JS for PQ */
function pqViewModel(modelId, cityId) {
    var self = this;
    self.cities = ko.observableArray([]);
    self.areas = ko.observableArray([]);
    self.selectedCity = ko.observable(cityId);
    self.selectedArea = ko.observable();
    self.selectedModel = ko.observable(modelId);
    self.priceQuote = ko.observable();
    self.DealerPriceList = ko.observableArray([]);
    self.BWPriceList = ko.observable();
    self.FormatPricedata = function (item) {
        if (item != undefined)
            return formatPrice(item);
        return "";
    };
    self.isDealerPQAvailable = ko.observable(false);
    self.DealerOnRoadPrice = ko.computed(function () {
        var total = 0;
        for (i = 0; i < self.DealerPriceList().length; i++) {
            total += self.DealerPriceList()[i].price;
        }
        return total;
    }, this);
    self.LoadCity = function () {
        loadCity(self);
    };
    self.LoadArea = function () {
        if (self.selectedArea())
            self.areas([]);
        loadArea(self);
    };

    self.OnAreaChange = function () {
        if (self.areas()) {
            fetchPriceQuote(self);
        }
    };

    self.FetchPriceQuote = function () {
        fetchPriceQuote(self);
    };

    //self.EditButton = function () {
    //    if (self.selectedArea()) {
    //        self.selectedArea(undefined);
    //    }
    //    editButton();
    //};
}

function loadCity(vm) {
    if (vm.selectedModel()) {
        $.get("/api/PQCityList/?modelId=" + vm.selectedModel(),
            function (data) {
                if (data) {
                    var city = ko.toJS(data);
                    vm.cities(city.cities);
                    //PQcheckCookies();
                    //var citySelectedNow = null;
                    //for (i = 0; i < city.cities.length; i++) {
                    //    c = city.cities[i].cityId;
                    //    if (PQCitySelectedId == c) {
                    //        citySelectedNow = city.cities[i];
                    //        break;
                    //    }
                    //}  
                    //if (citySelectedNow != null) {
                    //    vm.selectedCity(citySelectedNow.cityId);
                    //    loadArea(vm);
                    //    if (parseInt($('#mainCity li[cityId' + citySelectedNow.cityId + ']').attr('cityId')) > 0) {
                    //        $('#mainCity li[cityId' + citySelectedNow.cityId + ']').click();
                    //    }
                    //    else {
                    //        $('#mainCity li:last-child').click();
                    //    }
                    //    //get pricequote
                    //    fetchPriceQuote(vm);
                    //}
                    //else {
                    //    $(".city-select-text").removeClass("hide").addClass("show");
                    //}  
                    $(".city-select-text").removeClass("hide");
                    $(".city-select-text").addClass("show");
                }
            });
    }
}

function PQcheckCookies() {
    c = document.cookie.split('; ');
    for (i = c.length - 1; i >= 0; i--) {
        C = c[i].split('=');
        if (C[0] == "location") {
            var cData = (String(C[1])).split('_');
            PQCitySelectedId = parseInt(cData[0]);
            PQCitySelectedName = cData[1];
        }
    }
}

function loadArea(vm) {
    if (vm.selectedCity()) {
        $.ajax({
            url: "/api/PQAreaList/?modelId=" + vm.selectedModel() + "&cityId=" + vm.selectedCity(),
            type: "GET",
            contentType: "application/json",
            async: false
        }).done(function (data) {
            if (data) {
                var area = ko.toJS(data);
                vm.areas(area.areas);
                $(".city-select-text").removeClass("show");
                $(".area-select-text").removeClass("hide");
                $(".city-onRoad-price-container").addClass("hide");
                $(".city-select-text").addClass("hide");
                $(".area-select-text").addClass("show");
                $(".area-select").addClass("show");
                $("#btnBookNow").hide();
            }
            else {
                vm.areas([]);
                vm.FetchPriceQuote();
            }
        })
        .fail(function () {
            vm.areas([]);
            vm.FetchPriceQuote();
        });
    }
    else {
        vm.areas([]);
    }
}

function fetchPriceQuote(vm) {
    $("#dvAvailableOffer").empty();
    if (vm.selectedModel() && vm.selectedCity()) {
        $.ajax({
            url: "/api/OnRoadPrice/?cityId=" + vm.selectedCity() + "&modelId=" + vm.selectedModel() + "&clientIP=" + clientIP + "&sourceType=" + 1 + "&areaId=" + (vm.selectedArea() != undefined ? vm.selectedArea() : 0),
            type: "GET",
            contentType: "application/json",
            async: false
        }).done(function (data) {
            if (data) {
                var pq = ko.toJS(data);
                vm.priceQuote(pq);
                vm.isDealerPQAvailable(pq.IsDealerPriceAvailable);
                if (pq.IsDealerPriceAvailable) {
                    vm.DealerPriceList(pq.dealerPriceQuote.priceList);
                }
                else {
                    vm.BWPriceList(pq.bwPriceQuote);
                }
                if (vm.areas().length > 0 && pq && pq.IsDealerPriceAvailable) {
                    var cookieValue = "CityId=" + vm.selectedCity() + "&AreaId=" + vm.selectedArea() + "&PQId=" + pq.priceQuote.quoteId + "&VersionId=" + pq.priceQuote.versionId + "&DealerId=" + pq.priceQuote.dealerId;
                    SetCookie("_MPQ", cookieValue);
                    //if (pq.bwPriceQuote.city!=null)
                    //    SetCookieInDays("location", vm.selectedCity() + '_' + pq.bwPriceQuote.city);
                    $(".unveil-offer-btn-container").attr('style', '');
                    $(".unveil-offer-btn-container").removeClass("show");
                    $(".unveil-offer-btn-container").addClass("hide");
                    temptotalPrice = checkNumeric($(bikePrice).text().trim());
                    var totalPrice = 0;
                    var priceBreakText = '';
                    for (var i = 0; i < pq.dealerPriceQuote.priceList.length; i++) {
                        totalPrice += pq.dealerPriceQuote.priceList[i].price;
                        priceBreakText += pq.dealerPriceQuote.priceList[i].categoryName + " + "
                    }
                    priceBreakText = priceBreakText.substring(0, priceBreakText.length - 2);
                    if (pq.isInsuranceFree && pq.insuranceAmount > 0)
                        totalPrice = totalPrice - pq.insuranceAmount;

                    if (totalPrice <= 0) {
                        $($(".bike-price-container")[0]).hide();
                        $($(".bike-price-container")[1]).show();
                    }
                    else {
                        $($(".bike-price-container")[1]).hide();
                        $($(".bike-price-container")[0]).show();
                    }

                    //set global cookie
                    cityId = vm.selectedCity();
                    if (cityId > 0) {
                        cityName = $("#ddlCity option:selected").text();
                        cookieValue = cityId + "_" + cityName;
                        SetCookieInDays("location", cookieValue, 365);
                    }

                    animatePrice($("#bike-price"), temptotalPrice, totalPrice);
                    $("#btnBookNow").show();
                    //$("#bike-price").html(formatPrice(totalPrice));
                    $("#breakup").text("(" + priceBreakText + ")");
                    $("#pqCity").html($("#ddlCity option[value=" + vm.selectedCity() + "]").text());
                    $("#pqArea").html($("#ddlArea option[value=" + vm.selectedArea() + "]").text() + ', ');

                    $(".city-onRoad-price-container").removeClass("hide");
                    $(".city-select-text").removeClass("show");
                    $(".area-select-text").removeClass("show");
                    $('.available-offers-container').removeClass("hide");

                    $(".city-select-text").addClass("hide");
                    $(".area-select-text").addClass("hide");
                    $(".city-area-wrapper").removeClass("show").addClass("hide");
                    $(".city-onRoad-price-container").addClass("show");
                    $('.available-offers-container').addClass("show");

                    if (pq.dealerPriceQuote.offers && pq.dealerPriceQuote.offers.length > 0) {
                        $("#dvAvailableOffer").append("<ul id='dpqOffer' data-bind=\"foreach: priceQuote().dealerPriceQuote.offers\"><li data-bind=\"text: offerText\"></li></ul>");
                        ko.applyBindings(vm, $("#dpqOffer")[0]);
                    }
                    else {
                        $("#dvAvailableOffer").append("<ul><li>No offers available</li></ul>");
                    }
                    $(".default-showroom-text").html("View Breakup").addClass('view-breakup-text');
                }
                else {
                    temptotalPrice = checkNumeric($(bikePrice).text().trim());
                    totalPrice = pq.bwPriceQuote.onRoadPrice;
                    priceBreakText = "Ex-showroom + Insurance + RTO";
                    //$("#bike-price").html(formatPrice(totalPrice));
                    $("#breakup").text("(" + priceBreakText + ")");
                    $("#btnBookNow").hide();
                    if (totalPrice <= 0) {
                        $($(".bike-price-container")[0]).hide();
                        $($(".bike-price-container")[1]).show();
                    }
                    else {
                        $($(".bike-price-container")[1]).hide();
                        $($(".bike-price-container")[0]).show();
                    }
                    animatePrice($("#bike-price"), temptotalPrice, totalPrice);
                    $(".city-onRoad-price-container").removeClass("hide").addClass("show");
                    $("#pqCity").html($("#ddlCity option[value=" + vm.selectedCity() + "]").text());
                    if (vm.selectedArea() == undefined)
                        $("#pqArea").html("");
                    else $("#pqArea").html($("#ddlArea option[value=" + vm.selectedArea() + "]").text() + ', ');
                    $(".city-select-text").removeClass("show").addClass("hide");
                    $(".city-area-wrapper").removeClass("show").addClass("hide");
                    $(".city-select").removeClass("hide").addClass("hide");
                    $(".unveil-offer-btn-container").attr('style', '');
                    $(".unveil-offer-btn-container").removeClass("show").addClass("hide");
                    $("#dvAvailableOffer").empty();
                    $("#dvAvailableOffer").html("<ul><li>Currently there are no offers in your city. We hope to serve your city soon!</li></ul>");
                    $(".available-offers-container").removeClass("hide").addClass("show");

                    //set global cookie
                    cityId = vm.selectedCity();
                    if (cityId > 0) {
                        cityName = $("#ddlCity option:selected").text();
                        cookieValue = cityId + "_" + cityName;
                        SetCookieInDays("location", cookieValue, 365);
                    }

                }
                $(".default-showroom-text").html("View Breakup").addClass('view-breakup-text');
            }
            else {
                vm.areas([]);
                $(".unveil-offer-btn-container").attr('style', '');
                $(".unveil-offer-btn-container").removeClass("show");
                $(".city-onRoad-price-container").removeClass("show");
                $(".city-select-text").removeClass("hide");
                $(".area-select-text").removeClass("show");
                $(".city-area-wrapper").removeClass("hide");
                $(".city-select").removeClass("hide");
                $(".area-select").removeClass("show");
                $('.available-offers-container').removeClass("hide");

                $(".unveil-offer-btn-container").addClass("hide");
                $(".city-onRoad-price-container").addClass("hide");
                $(".city-select-text").addClass("show");
                $(".area-select-text").addClass("hide");
                $(".city-area-wrapper").addClass("show");
                $(".city-select").addClass("show");
                $(".area-select").addClass("hide");
                $('.available-offers-container').addClass("show");

                $("#dvAvailableOffer").empty();
                $("#dvAvailableOffer").append("<ul><li>Currently there are no offers in your city. We hope to serve your city soon!</li></ul>");
            }
        })
        .fail(function () {
            vm.areas([]);
            $(".unveil-offer-btn-container").attr('style', '');
            $(".unveil-offer-btn-container").removeClass("show");
            $(".city-onRoad-price-container").removeClass("show");
            $(".city-select-text").removeClass("hide");
            $(".area-select-text").removeClass("show");
            $(".city-area-wrapper").removeClass("hide");
            $(".city-select").removeClass("hide");
            $(".area-select").removeClass("show");
            $('.available-offers-container').removeClass("hide");

            $(".unveil-offer-btn-container").addClass("hide");
            $(".city-onRoad-price-container").addClass("hide");
            $(".city-select-text").addClass("show");
            $(".area-select-text").addClass("hide");
            $(".city-area-wrapper").addClass("show");
            $(".city-select").addClass("show");
            $(".area-select").addClass("hide");
            $('.available-offers-container').addClass("show");

            $("#dvAvailableOffer").empty();
            $("#dvAvailableOffer").append("<ul><li>Currently there are no offers in your city. We hope to serve your city soon!</li></ul>");
        });
    }
}

$("#mainCity li").click(function () {
    var val = $(this).attr('cityId');
    $("#city-list-container").removeClass("show").addClass("hide");
    $(".city-select-text").removeClass("hide").addClass("show");
    $("#city-area-select-container").removeClass("hide").addClass("show");
    $(".offer-error").removeClass("show").addClass("hide");
    $(".area-select").removeClass("show").addClass("hide");
    $(".city-select").removeClass("hide").addClass("show");
    $(".city-area-wrapper").removeClass("hide").addClass("show");
    $(".city-onRoad-price-container").removeClass("show").addClass("hide");
    $(".unveil-offer-btn-container").removeClass("hide").addClass("show");
    if (val) {
        $("#ddlCity option[value=" + val + "]").prop('selected', 'selected');
        $('#ddlCity').trigger('change');

    }
});


$(".city-edit-btn").click(function () {
    if ($("#ddlCity").val()) {

        if ($("#ddlArea").val()) {
            $(".area-select").removeClass("hide").addClass("show");
            $('.area-select-text').removeClass("hide").addClass("show");
            $(".available-offers-container").removeClass("hide").addClass("show").removeClass("text-red");
            $(".city-select-text").removeClass("show").addClass("hide").removeClass("text-red");
            modelViewModel.selectedArea(undefined);

        }
        else {
            $(".city-select-text").removeClass("hide").addClass("show");
            $(".city-select").removeClass("hide").addClass("show").removeClass("text-red");
            $('.area-select-text').removeClass("show").addClass("hide").removeClass("text-red");
            $(".available-offers-container").removeClass("show").addClass("hide");
            $(".unveil-offer-btn-container").removeClass("hide").addClass("show");
        }

        $(".city-area-wrapper").removeClass("hide").addClass("show");
        $(".city-onRoad-price-container").removeClass("show").addClass("hide");

    }
});


function InitVM(cityId) {
    var viewModel = new pqViewModel(vmModelId, cityId);
    modelViewModel = viewModel;
    ko.applyBindings(viewModel, $('#dvBikePrice')[0]);
    viewModel.LoadCity();
}

$(document).ready(function () {
    if (isUsed == "False") {
        InitVM(cityId);
    }
    $(".unveil-offer-btn-container").removeClass("hide");
    $(".unveil-offer-btn-container").addClass("show");
    $(".unveil-offer-btn-container").attr('style', '');

    $("#btnBookNow").on("click", function () {
        window.location.href = "/m/pricequote/bookingsummary_new.aspx";
    });

    $(".blackOut-window").mouseup(function (e) {
        var breakup = $("#breakupPopUpContainer");
        if (e.target.id !== breakup.attr('id') && !breakup.has(e.target).length) {
            breakup.removeClass("show").addClass("hide");
            unlockPopup();
        }
    });

    $("div#dvBikePrice").on('click', 'span.view-breakup-text', function () {
        faqPopupShow();
    });

    $(".breakupCloseBtn").click(function () {
        $(".breakupPopUpContainer").removeClass("show").addClass("hide");
        unlockPopup();
    });

    $(offerBtn).click(function () {
        if (modelViewModel.selectedCity() == undefined || modelViewModel.selectedArea() == undefined) {
            $('.offer-error').addClass("text-red");
            $('.city-select-text').addClass("text-red");
            if (modelViewModel.selectedCity() != undefined && modelViewModel.selectedArea() == undefined) {
                $('.area-select-text').addClass("text-red");
            }
        }
    });

});


var priceChange = function () {
    var a = $("#bike-price");
    a.html("40,000");
    showroomPrice.css({ "display": "none" });
    $(".view-breakup-text").css({ "display": "inline" });
}

function faqPopupShow() {
    $(".breakupPopUpContainer").removeClass("hide").addClass("show");
    $(".blackOut-window").show();
};

function unlockPopup() {
    $('body').removeClass('lock-browser-scroll');
    $(".blackOut-window").hide();
}

function formatPrice(price) {
    price = price.toString();
    var lastThree = price.substring(price.length - 3);
    var otherNumbers = price.substring(0, price.length - 3);
    if (otherNumbers != '')
        lastThree = ',' + lastThree;
    var price = otherNumbers.replace(/\B(?=(\d{2})+(?!\d))/g, ",") + lastThree;

    return price;
}

//Animate the element's value from x to y:
function animatePrice(ele, start, end) {
    $({ someValue: start }).stop(true).animate({ someValue: end }, {
        duration: 500,
        easing: 'easeInOutBounce',
        step: function () {
            $(ele).text(commaSeparateNumber(Math.round(this.someValue)));
        }
    }).promise().done(function () {
        $(ele).text(formatPrice(end));
    });
}


function commaSeparateNumber(val) {
    while (/(\d+)(\d{3})/.test(val.toString())) {
        val = val.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
    }
    return val;
}

function checkNumeric(str) {
    return parseInt(str.replace(/\,/g, ''));
}

// GA codes
$('#ddlCity').change(function () {
    var cityClicked = $('#ddlCity option:selected').text();
    dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Model_Page', 'act': 'City_Selected', 'lab': cityClicked });

});

$('#ddlArea').change(function () {
    var areaClicked = $('#ddlArea option:selected').text();
    dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Model_Page', 'act': 'Area_Selected', 'lab': areaClicked });

});

$("#btnShowOffers").on("click", function () {
    dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Model_Page', 'act': 'Show_Offers_Clicked', 'lab': myBikeName });
});

$("#btnBookNow").on("click", function () {
    
    var city_area = getCookie('location');
    dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Model_Page', 'act': 'Show_Offers_Clicked', 'lab': myBikeName + '_' + city_area });
});
