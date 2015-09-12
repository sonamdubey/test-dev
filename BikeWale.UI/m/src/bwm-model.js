// JavaScript Document

jQuery(function(){
	
	jQuery('.jcarousel-wrapper.model .jcarousel')
    .on('jcarousel:create jcarousel:reload', function() {
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


otherBtn.click(function(){
	cityList.hide();
	$(".city-select-text").removeClass("hide").addClass("show");
	cityAreaContainer.removeClass("hide").addClass("show");
	offerError.removeClass("show").addClass("hide");
});

citySelect.on("change",function(){
	areaSelect.removeClass("hide").addClass("show");
	$(".city-select-text").removeClass("show").addClass("hide");	
	$(".area-select-text").removeClass("hide").addClass("show");
});

areaSelect.on("change",function(){
	$(".area-select-text").removeClass("show").addClass("hide");
	onRoadPriceText.removeClass("hide").addClass("show");
	offerBtnContainer.hide();
	$(".city-area-wrapper").addClass("hide");
	cityareaHide();
	priceChange();
});

editBtn.click(function(){
	$(".city-select-text").removeClass("hide").addClass("show");
	cityAreaContainer.removeClass("hide").addClass("show");
	onRoadPriceText.removeClass("show").addClass("hide");
	$(".city-area-wrapper").removeClass("hide").addClass("show");
	citySelect.removeClass("hide").addClass("show");
});

offerBtn.click(function(){
	offerError.addClass("text-red");	
});

var cityareaHide = function(){
	citySelect.removeClass("show").addClass("hide");
	areaSelect.removeClass("show").addClass("hide");
}

var priceChange = function(){
	var a = $("#bike-price");
	a.html("40,000");
	showroomPrice.html("+ View Breakup");
}

$(".more-features-btn").click(function(){
	$(".more-features").slideToggle();
	var a = $(this).find("span");
	a.text(a.text() === "+" ? "-" : "+");
});

$("#showFullDisc").click(function () {
    $(this).hide()
    $(this).parent().slideUp();
    $("#showSmallDisc").parent().slideDown().show();
    $("#showSmallDisc").show();
});

$("#showSmallDisc").click(function () {
    $(this).hide()
    $(this).parent().slideUp();
    $("#showFullDisc").parent().slideDown().show();
    $("#showFullDisc").show()
});
/* JS for PQ */
//function pqViewModel(modelId,cityId) {
//    var self = this;
//    self.cities = ko.observableArray([]);
//    self.areas = ko.observableArray([]);
//    self.selectedCity = ko.observable(cityId);
//    self.selectedArea = ko.observable();
//    self.selectedModel = ko.observable(modelId);
//    self.priceQuote = ko.observable();
//    self.LoadCity = function () {
//        loadCity(self);
//    }
//    self.LoadArea = function(){
//        loadArea(self);
//    }

//    self.OnAreaChange = function () {
//        fetchPriceQuote(self);
//    }

//    self.FetchPriceQuote = function () {
//        fetchPriceQuote(self);
//    }
//}

//function loadCity(vm) {
//    if (vm.selectedModel()) {
//        $.get("/api/PQCityList/?modelId=" + vm.selectedModel(),
//            function (data) {
//                if (data) {
//                    var city = ko.toJS(data);
//                    vm.cities(city.cities);
//                    $(".city-select-text").removeClass("hide").addClass("show");                                
//                }
//            });
//    }
//}

//function loadArea(vm) {                
//    if (vm.selectedCity()) {
//        $.get("/api/PQAreaList/?modelId=" + vm.selectedModel() + "&cityId=" + vm.selectedCity())
//        .done(function (data) {
//            if (data) {
//                var area = ko.toJS(data);
//                vm.areas(area.areas);
//                $(".city-select-text").removeClass("show").addClass("hide");
//                $(".area-select-text").removeClass("hide").addClass("show");
//            }
//            else {
//                vm.areas([]);
//                $(".area-select-text").removeClass("show").addClass("hide");
//                vm.FetchPriceQuote();
//            }
//        })
//        .fail(function () {
//            vm.areas([]);
//            $(".area-select-text").removeClass("show").addClass("hide");
//            vm.FetchPriceQuote();
//        });
//    }
//    else {
//        vm.areas([]);
//        $(".city-area-wrapper").removeClass("hide").addClass("show");
//        $(".city-select").removeClass("hide").addClass("show");
//        $(".area-select").removeClass("show").addClass("hide");
//        $(".city-select-text").removeClass("hide").addClass("show");
//        $(".area-select-text").removeClass("show").addClass("hide");
//        $(".city-onRoad-price-container").removeClass("show").addClass("hide");
//        $(".unveil-offer-btn-container").attr('style', '');
//        $(".unveil-offer-btn-container").removeClass("hide").addClass("show");
//        $(".default-showroom-text").html("");
//    }                    
//}

//function fetchPriceQuote(vm) {             
//    $("#dvAvailableOffer").empty();
//    if (vm.selectedModel() && vm.selectedCity()) {                    
//        $.get("/api/OnRoadPrice/?cityId=" + vm.selectedCity() + "&modelId=" + vm.selectedModel() + "&clientIP=" + clientIP + "&sourceType=" + 1 + "&areaId=" + (vm.selectedArea() ? vm.selectedArea() : ""))
//        .done(function (data) {
//            if (data) {
//                var pq = ko.toJS(data);
//                vm.priceQuote(pq);
//                if (pq && pq.IsDealerPriceAvailable) {
//                    $(".unveil-offer-btn-container").attr('style', '');
//                    $(".unveil-offer-btn-container").removeClass("show").addClass("hide");
//                    var totalPrice = 0;
//                    var priceBreakText = '';
//                    for (var i = 0; i < pq.dealerPriceQuote.priceList.length; i++) {
//                        totalPrice += pq.dealerPriceQuote.priceList[i].price;
//                        priceBreakText += pq.dealerPriceQuote.priceList[i].categoryName + " + "
//                    }
//                    priceBreakText = priceBreakText.substring(0, priceBreakText.length - 2);
//                    $("#bike-price").html(totalPrice);
//                    $("#breakup").text("(" + priceBreakText + ")");
//                    $("#pqCity").html($("#ddlCity option[value=" + vm.selectedCity() + "]").text())
//                    $("#pqArea").html($("#ddlArea option[value=" + vm.selectedArea() + "]").text())
//                    $(".city-select-text").removeClass("show").addClass("hide");
//                    $(".area-select-text").removeClass("show").addClass("hide");
//                    $(".city-onRoad-price-container").removeClass("hide").addClass("show");
//                    $(".city-area-wrapper").addClass("hide");
//                    if (pq.dealerPriceQuote.offers && pq.dealerPriceQuote.offers.length > 0) {
//                        $('.available-offers-container').removeClass("hide").addClass("show");
//                        $("#dvAvailableOffer").append("<ul id='dpqOffer' data-bind=\"foreach: priceQuote().dealerPriceQuote.offers\"><li data-bind=\"text: offerText\"></li></ul>");
//                        ko.applyBindings(vm, $("#dpqOffer")[0]);
//                    }
//                    else {
//                        $('.available-offers-container').removeClass("hide").addClass("show");
//                        $("#dvAvailableOffer").append("<ul><li>No offers available</li></ul>");
//                    }
//                    $(".default-showroom-text").html("+ View Breakup");
//                }                            
//                else {
//                    if(pq.bwPriceQuote.onRoadPrice > 0) {
//                        totalPrice = pq.bwPriceQuote.onRoadPrice;
//                        priceBreakText = "Ex-showroom + Insurance + RTO";
//                    }                                
//                    $("#bike-price").html(totalPrice);
//                    $("#breakup").text("(" + priceBreakText + ")");
//                    $(".unveil-offer-btn-container").attr('style', '');
//                    $(".unveil-offer-btn-container").removeClass("show").addClass("hide");
//                    $(".city-onRoad-price-container").removeClass("show").addClass("hide");
//                    $(".city-select-text").removeClass("hide").addClass("show");
//                    $(".area-select-text").removeClass("show").addClass("hide");
//                    $(".city-area-wrapper").removeClass("hide").addClass("show");
//                    $(".city-select").removeClass("hide").addClass("show");
//                    $(".area-select").removeClass("show").addClass("hide");
//                    $('.available-offers-container').removeClass("hide").addClass("show");
//                    $("#dvAvailableOffer").empty();
//                    $("#dvAvailableOffer").append("<ul><li>Currently there are no offers in your city. We hope to serve your city soon!</li></ul>");
//                }
//                $(".default-showroom-text").html("+ View Breakup");
//            }
//            else {
//                vm.areas([]);
//                $(".unveil-offer-btn-container").attr('style', '');
//                $(".unveil-offer-btn-container").removeClass("show").addClass("hide");
//                $(".city-onRoad-price-container").removeClass("show").addClass("hide");
//                $(".city-select-text").removeClass("hide").addClass("show");
//                $(".area-select-text").removeClass("show").addClass("hide");
//                $(".city-area-wrapper").removeClass("hide").addClass("show");
//                $(".city-select").removeClass("hide").addClass("show");
//                $(".area-select").removeClass("show").addClass("hide");
//                $('.available-offers-container').removeClass("hide").addClass("show");
//                $("#dvAvailableOffer").empty();
//                $("#dvAvailableOffer").append("<ul><li>Currently there are no offers in your city. We hope to serve your city soon!</li></ul>");
//            }
//        })
//        .fail(function () {
//            vm.areas([]);
//            $(".unveil-offer-btn-container").attr('style', '');
//            $(".unveil-offer-btn-container").removeClass("show").addClass("hide");
//            $(".city-onRoad-price-container").removeClass("show").addClass("hide");
//            $(".city-select-text").removeClass("hide").addClass("show");
//            $(".area-select-text").removeClass("show").addClass("hide");
//            $(".city-area-wrapper").removeClass("hide").addClass("show");
//            $(".city-select").removeClass("hide").addClass("show");
//            $(".area-select").removeClass("show").addClass("hide");
//            $('.available-offers-container').removeClass("hide").addClass("show");
//            $("#dvAvailableOffer").empty();
//            $("#dvAvailableOffer").append("<ul><li>Currently there are no offers in your city. We hope to serve your city soon!</li></ul>");
//        });
//    }
//}

//$("#mainCity li").click(function () {
//    var val = $(this).attr('cityId');                
//    $("#city-list-container").removeClass("show").addClass("hide");
//    $(".city-select-text").removeClass("hide").addClass("show");
//    $("#city-area-select-container").removeClass("hide").addClass("show");
//    $(".offer-error").removeClass("show").addClass("hide");                                        
//    $(".area-select").removeClass("show").addClass("hide");
//    $(".city-select").removeClass("hide").addClass("show");
//    $(".city-area-wrapper").removeClass("hide").addClass("show");
//    $(".city-onRoad-price-container").removeClass("show").addClass("hide");
//    $(".unveil-offer-btn-container").removeClass("hide").addClass("show");
//    if (val) {
//        $("#ddlCity option[value=" + val + "]").attr('selected', 'selected');
//        $('#ddlCity').trigger('change');
//        $(".area-select").removeClass("hide").addClass("show");
//    }
//});

//$(".city-edit-btn").click(function () {
//    if ($("#ddlCity").val() && $("#ddlArea").val()) {
//        $(".city-select-text").removeClass("hide").addClass("show");
//        $(".area-select").addClass("hide");
//        $(".city-onRoad-price-container").removeClass("show").addClass("hide");                    
//    }
//    $(".available-offers-container").removeClass("show").addClass("hide");
//    $(".unveil-offer-btn-container").removeClass("hide").addClass("show");
//});

//function InitVM(cityId) {
//    var viewModel = new pqViewModel(vmModelId, cityId);
//    ko.applyBindings(viewModel, $('#dvBikePrice')[0]);
//    viewModel.LoadCity();
//}

//$(document).ready(function () {
//    InitVM(0);
//    $(".unveil-offer-btn-container").removeClass("hide").addClass("show");
//    $(".unveil-offer-btn-container").attr('style', '');
//});