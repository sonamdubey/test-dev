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

cityTabs.on('click', function () {
    cityTabs.removeClass('focused');
    $(this).addClass('focused');
    /*$(this).siblings('li').hide();
	cityAreaContainer.removeClass("hide").addClass("show");
	citySelect.addClass('hide');
	$('.area-select-text').removeClass("hide").addClass("show");
	areaSelect.fadeIn("slow").removeClass("hide");*/
});


otherBtn.click(function () {
    cityList.hide();
    $(".city-select-text").removeClass("hide").addClass("show");
    cityAreaContainer.removeClass("hide").addClass("show");
    offerError.removeClass("show").addClass("hide");
});

citySelect.on("change", function () {
    areaSelect.removeClass("hide").addClass("show");
    $(".city-select-text").removeClass("show").addClass("hide");
    $(".area-select-text").removeClass("hide").addClass("show");
});

areaSelect.on("change", function () {
    $(".area-select-text").removeClass("show").addClass("hide");
    onRoadPriceText.removeClass("hide").addClass("show");
    offerBtnContainer.hide();
    $(".city-area-wrapper").addClass("hide");
    cityareaHide();
    priceChange();
});

editBtn.click(function () {
    $(".city-select-text").removeClass("hide").addClass("show");
    cityAreaContainer.removeClass("hide").addClass("show");
    onRoadPriceText.removeClass("show").addClass("hide");
    $(".city-area-wrapper").removeClass("hide").addClass("show");
    citySelect.removeClass("hide").addClass("show");
});

offerBtn.click(function () {
    offerError.addClass("text-red");
});

var cityareaHide = function () {
    citySelect.removeClass("show").addClass("hide");
    areaSelect.removeClass("show").addClass("hide");
}

var priceChange = function () {
    var a = $("#bike-price");
    a.html("40,000");
    showroomPrice.html("+ View Breakup");
}

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
    self.LoadCity = function () {
        loadCity(self);
    }
    self.LoadArea = function () {
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
                    $(".city-select-text").removeClass("hide");
                    $(".city-select-text").addClass("show");
                }
            });
    }
}

function loadArea(vm) {
    if (vm.selectedCity()) {
        $.get("/api/PQAreaList/?modelId=" + vm.selectedModel() + "&cityId=" + vm.selectedCity())
        .done(function (data) {
            if (data) {
                var area = ko.toJS(data);
                vm.areas(area.areas);
                $(".city-select-text").removeClass("show");
                $(".area-select-text").removeClass("hide");
                $(".city-onRoad-price-container").addClass("hide");
                $(".city-select-text").addClass("hide");
                $(".area-select-text").addClass("show");
                $(".area-select").addClass("show");
            }
            else {
                vm.areas([]);
                $(".area-select-text").removeClass("show");
                $(".city-onRoad-price-container").removeClass("show");
                $(".area-select-text").addClass("hide");
                $(".city-onRoad-price-container").addClass("hide");
                vm.FetchPriceQuote();
            }
        })
        .fail(function () {
            vm.areas([]);
            $(".area-select-text").removeClass("show");
            $(".city-onRoad-price-container").removeClass("show");
            $(".area-select-text").addClass("hide");
            $(".city-onRoad-price-container").addClass("hide");
            vm.FetchPriceQuote();
        });
    }
    else {
        vm.areas([]);
        $(".city-area-wrapper").removeClass("hide");
        $(".city-select").removeClass("hide");
        $(".area-select").removeClass("show");
        $(".city-select-text").removeClass("hide");
        $(".area-select-text").removeClass("show");
        $(".city-onRoad-price-container").removeClass("show");
        $(".unveil-offer-btn-container").attr('style', '');
        $(".unveil-offer-btn-container").removeClass("hide");
        $(".city-area-wrapper").addClass("show");
        $(".city-select").addClass("show");
        $(".area-select").addClass("hide");
        $(".city-select-text").addClass("show");
        $(".area-select-text").addClass("hide");
        $(".city-onRoad-price-container").addClass("hide");
        $(".unveil-offer-btn-container").addClass("show");
        $(".default-showroom-text").html("");

    }
}

function fetchPriceQuote(vm) {
    $("#dvAvailableOffer").empty();
    if (vm.selectedModel() && vm.selectedCity()) {
        $.get("/api/OnRoadPrice/?cityId=" + vm.selectedCity() + "&modelId=" + vm.selectedModel() + "&clientIP=" + clientIP + "&sourceType=" + 2 + "&areaId=" + (vm.selectedArea() ? vm.selectedArea() : ""))
        .done(function (data) {
            if (data) {
                var pq = ko.toJS(data);
                vm.priceQuote(pq);
                if (pq && pq.IsDealerPriceAvailable) {
                    var cookieValue = "CityId=" + vm.selectedCity() + "&AreaId=" + vm.selectedArea() + "&PQId=" + pq.priceQuote.quoteId + "&VersionId=" + pq.priceQuote.versionId + "&DealerId=" + pq.priceQuote.dealerId;
                    SetCookie("_MPQ", cookieValue);
                    SetCookieInDays("location", vm.selectedCity() + '_' + pq.bwPriceQuote.city);
                    $(".unveil-offer-btn-container").attr('style', '');
                    $(".unveil-offer-btn-container").removeClass("show");
                    $(".unveil-offer-btn-container").addClass("hide");
                    var totalPrice = 0;
                    var priceBreakText = '';
                    for (var i = 0; i < pq.dealerPriceQuote.priceList.length; i++) {
                        totalPrice += pq.dealerPriceQuote.priceList[i].price;
                        priceBreakText += pq.dealerPriceQuote.priceList[i].categoryName + " + "
                    }
                    priceBreakText = priceBreakText.substring(0, priceBreakText.length - 2);
                    $("#bike-price").html(totalPrice);
                    $("#breakup").text("(" + priceBreakText + ")");
                    $("#pqCity").html($("#ddlCity option[value=" + vm.selectedCity() + "]").text())
                    $("#pqArea").html($("#ddlArea option[value=" + vm.selectedArea() + "]").text())

                    $(".city-onRoad-price-container").removeClass("hide");
                    $(".city-select-text").removeClass("show");
                    $(".area-select-text").removeClass("show");
                    $('.available-offers-container').removeClass("hide");

                    $(".city-select-text").addClass("hide");
                    $(".area-select-text").addClass("hide");
                    $(".city-area-wrapper").addClass("hide");
                    $(".city-onRoad-price-container").addClass("show");
                    $('.available-offers-container').addClass("show");
                    if (pq.dealerPriceQuote.offers && pq.dealerPriceQuote.offers.length > 0) {
                        $("#dvAvailableOffer").append("<ul id='dpqOffer' data-bind=\"foreach: priceQuote().dealerPriceQuote.offers\"><li data-bind=\"text: offerText\"></li></ul>");
                        ko.applyBindings(vm, $("#dpqOffer")[0]);
                    }
                    else {
                        $("#dvAvailableOffer").append("<ul><li>No offers available</li></ul>");
                    }
                    $(".default-showroom-text").html("+ View Breakup");
                }
                else {
                    if (pq.bwPriceQuote.onRoadPrice > 0) {
                        totalPrice = pq.bwPriceQuote.onRoadPrice;
                        priceBreakText = "Ex-showroom + Insurance + RTO";
                    }
                    $("#bike-price").html(totalPrice);
                    $("#breakup").text("(" + priceBreakText + ")");
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
                $(".default-showroom-text").html("+ View Breakup");
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
    if ($("#ddlCity").val() && $("#ddlArea").val()) {
        $(".city-select-text").removeClass("hide");
        $(".city-select-text").addClass("show");
        $(".area-select").addClass("hide");
        $(".city-onRoad-price-container").removeClass("show");
        $(".city-onRoad-price-container").addClass("hide");
    }
    $(".available-offers-container").removeClass("show");
    $(".unveil-offer-btn-container").removeClass("hide");

    $(".available-offers-container").addClass("hide");
    $(".unveil-offer-btn-container").addClass("show");
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
});