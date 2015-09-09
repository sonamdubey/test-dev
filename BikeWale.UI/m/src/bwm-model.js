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