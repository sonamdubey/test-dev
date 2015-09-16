// JavaScript Document

(function($) {
    
    var connector = function(itemNavigation, carouselStage) {
        return carouselStage.jcarousel('items').eq(itemNavigation.index());
    };

    $(function() {
        
        var carouselStage      = $('.carousel-stage').jcarousel();
        var carouselNavigation = $('.carousel-navigation').jcarousel();

        
        carouselNavigation.jcarousel('items').each(function() {
            var item = $(this);

            
            var target = connector(item, carouselStage);

            item
                .on('jcarouselcontrol:active', function() {
                    carouselNavigation.jcarousel('scrollIntoView', this);
                    item.addClass('active');
                })
                .on('jcarouselcontrol:inactive', function() {
                    item.removeClass('active');
                })
                .jcarouselControl({
                    target: target,
                    carousel: carouselStage
                });
        });

        
        $('.prev-stage')
            .on('jcarouselcontrol:inactive', function() {
                $(this).addClass('inactive');
            })
            .on('jcarouselcontrol:active', function() {
                $(this).removeClass('inactive');
            })
            .jcarouselControl({
                target: '-=1'
            });

        $('.next-stage')
            .on('jcarouselcontrol:inactive', function() {
                $(this).addClass('inactive');
            })
            .on('jcarouselcontrol:active', function() {
                $(this).removeClass('inactive');
            })
            .jcarouselControl({
                target: '+=1'
            });

        
        $('.prev-navigation')
            .on('jcarouselcontrol:inactive', function() {
                $(this).addClass('inactive');
            })
            .on('jcarouselcontrol:active', function() {
                $(this).removeClass('inactive');
            })
            .jcarouselControl({
                target: '-=1'
            });

        $('.next-navigation')
            .on('jcarouselcontrol:inactive', function() {
                $(this).addClass('inactive');
            })
            .on('jcarouselcontrol:active', function() {
                $(this).removeClass('inactive');
            })
            .jcarouselControl({
                target: '+=1'
            });
    });
})(jQuery);




var otherBtn = $(".city-other-btn");
var cityAreaContainer = $("#city-area-select-container");
var cityList = $("#city-list-container");
var citySelect = $(".city-select");
var areaSelect = $(".area-select");
var offerBtnContainer = $(".unveil-offer-btn-container");
var offerBtn = $(".unveil-offer-btn");
var offerError = $(".offer-error");
var editBtn = $(".city-edit-btn")
var bikePrice = $("#bike-price");
var onRoadPriceText = $(".city-onRoad-price-container")
var showroomPrice = $(".default-showroom-text");
var temptotalPrice = $(bikePrice).text();

var cityareaHide = function(){
	citySelect.removeClass("show").addClass("hide");
	areaSelect.removeClass("show").addClass("hide");
}

var priceChange = function(){
	var a = $("#bike-price");
	a.html("40,000");
	showroomPrice.html("View Breakup");
}

function formatPrice(price){
    price = price.toString();
    var lastThree = price.substring(price.length - 3);
    var otherNumbers = price.substring(0, price.length - 3);
    if (otherNumbers != '')
        lastThree = ',' + lastThree;
    var price = otherNumbers.replace(/\B(?=(\d{2})+(?!\d))/g, ",") + lastThree;

    return price;
}

//Animate the element's value from x to y:
function animatePrice(ele,start,end)
{
    $({ someValue: start }).stop(true).animate({ someValue: end }, {
        duration: 500,
        easing: 'easeInOutBounce',
        step: function () { 
            $(ele).text(commaSeparateNumber(Math.round(this.someValue)));
        }
    }).promise().done(function () {
        $(ele).text(commaSeparateNumber(end));
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
