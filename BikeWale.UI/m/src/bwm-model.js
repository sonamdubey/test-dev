// JavaScript Document

jQuery(function () {

    jQuery('.jcarousel-wrapper.model .jcarousel')
    .on('jcarousel:create jcarousel:reload', function () {
        var element = $(this),
            width = element.innerWidth();
        element.jcarousel('items').css('width', width + 'px');
    })
    .jcarousel({
        // Your configurations options

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
        if(item!=undefined)
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
    }
    self.LoadArea = function () {
        if (self.selectedArea())
            self.areas([]);
        loadArea(self);
    }

    self.OnAreaChange = function () {
        if (self.areas()) {
            fetchPriceQuote(self);
        }        
    }

    self.FetchPriceQuote = function () {
        fetchPriceQuote(self);
    }
}

function loadCity(vm) {
    if (vm.selectedModel()) {
        $.get("/api/PQCityList/?modelId=" + vm.selectedModel(),
            function (data) {
                if (data) {
                    var city = ko.toJS(data);
                    vm.cities(city.cities);
                    //PQcheckCookies();
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
                    SetCookieInDays("location", vm.selectedCity() + '_' + pq.bwPriceQuote.city);
                    $(".unveil-offer-btn-container").attr('style', '');
                    $(".unveil-offer-btn-container").removeClass("show");
                    $(".unveil-offer-btn-container").addClass("hide");
                    temptotalPrice = totalPrice;
                    var totalPrice = 0;
                    var priceBreakText = '';
                    for (var i = 0; i < pq.dealerPriceQuote.priceList.length; i++) {
                        totalPrice += pq.dealerPriceQuote.priceList[i].price;
                        priceBreakText += pq.dealerPriceQuote.priceList[i].categoryName + " + "
                    }
                    priceBreakText = priceBreakText.substring(0, priceBreakText.length - 2);

                    //animatePrice($("#bike-price"), temptotalPrice, totalPrice);
                    $("#btnBookNow").show();
                    $("#bike-price").html(formatPrice(totalPrice));
                    $("#breakup").text("(" + priceBreakText + ")");
                    $("#pqCity").html($("#ddlCity option[value=" + vm.selectedCity() + "]").text())
                    $("#pqArea").html($("#ddlArea option[value=" + vm.selectedArea() + "]").text())

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
                    temptotalPrice = totalPrice;
                    totalPrice = pq.bwPriceQuote.onRoadPrice;
                    priceBreakText = "Ex-showroom + Insurance + RTO";
                    $("#bike-price").html(formatPrice(totalPrice));
                    $("#breakup").text("(" + priceBreakText + ")");
                    $("#btnBookNow").hide();
                    //animatePrice($("#bike-price"), temptotalPrice, totalPrice);
                    $(".city-onRoad-price-container").removeClass("hide").addClass("show");
                    $("#pqCity").html($("#ddlCity option[value=" + vm.selectedCity() + "]").text());
                    $("#pqArea").html("");
                    $(".city-select-text").removeClass("show").addClass("hide");
                    $(".city-area-wrapper").removeClass("show").addClass("hide");
                    $(".city-select").removeClass("hide").addClass("hide");
                    $(".unveil-offer-btn-container").attr('style', '');
                    $(".unveil-offer-btn-container").removeClass("show").addClass("hide");
                    $("#dvAvailableOffer").empty();
                    $("#dvAvailableOffer").html("<ul><li>Currently there are no offers in your city. We hope to serve your city soon!</li></ul>");
                    $(".available-offers-container").removeClass("hide").addClass("show");

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
    $("#city-list-container").removeClass("show");
    $(".city-select-text").removeClass("hide");
    $("#city-area-select-container").removeClass("hide");
    $(".offer-error").removeClass("show");
    $(".area-select").removeClass("show");
    $(".city-select").removeClass("hide");
    $(".city-area-wrapper").removeClass("hide");
    $(".city-onRoad-price-container").removeClass("show");
    $(".unveil-offer-btn-container").removeClass("hide");

    $("#city-list-container").addClass("hide");
    $(".city-select-text").addClass("show");
    $("#city-area-select-container").addClass("show");
    $(".offer-error").addClass("hide");
    $(".area-select").addClass("hide");
    $(".city-select").addClass("show");
    $(".city-area-wrapper").addClass("show");
    $(".city-onRoad-price-container").addClass("hide");
    $(".unveil-offer-btn-container").addClass("show");

    if (val) {
        $("#ddlCity option[value=" + val + "]").attr('selected', 'selected');
        $('#ddlCity').trigger('change');
        $(".area-select").removeClass("hide");
        $(".area-select").addClass("show");
    }
});


$(".city-edit-btn").click(function () {
    if ($("#ddlCity").val()) {
        $(".city-select-text").removeClass("hide").addClass("show");
        if ($("#ddlArea").val()) {
            $(".area-select").removeClass("hide").addClass("show");
            $('.area-select-text').removeClass("hide").addClass("show");
            $(".city-select-text").removeClass("show").addClass("hide");
        }

        $(".city-select").removeClass("hide").addClass("show");
        $(".city-onRoad-price-container").removeClass("show").addClass("hide");
        $(".city-area-wrapper").removeClass("hide").addClass("show");
    }
    $(".available-offers-container").removeClass("show").addClass("hide");
    $(".unveil-offer-btn-container").removeClass("hide").addClass("show");
});


function InitVM(cityId) {
    var viewModel = new pqViewModel(vmModelId, cityId);
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
        easing: "easeOutExpo",
        step: function () {
            $(ele).text(commaSeparateNumber(Math.round(this.someValue)));
        }
    }).promise().done(function () {
        $(ele).text(end);
    });
}


function commaSeparateNumber(val) {
    while (/(\d+)(\d{3})/.test(val.toString())) {
        val = val.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
    }
    return val;
}