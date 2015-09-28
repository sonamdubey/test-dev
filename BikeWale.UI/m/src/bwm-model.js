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

/*
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
*/
// JavaScript Document
var pqCookieObj = {
    PQCitySelectedId: 0,
    PQAreaSelectedId: 0,
    PQCitySelectedName: "",
    PQAreaSelectedName: ""
};
var temptotalPrice = 0;
var modelViewModel;
var priceBlock = $('#dvBikePrice');
var cityAreaContainer = $("#city-area-select-container");
var otherBtn = $('#mainCity').find("span.city-other-btn")[0];
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
        loadCity(self);
    };

    self.FetchPriceQuote = function () {
        fetchPriceQuote(self);
    };

    self.selectedCity.subscribe(function () {
        self.areas("");
        self.selectedArea(undefined);
        loadArea(self);
    });

    self.selectedArea.subscribe(function () {
        if (self.selectedArea() != undefined && self.selectedArea() != 0)
            fetchPriceQuote(self);
    });

    self.availOfferBtn = function () {
        if (self.priceQuote() && self.priceQuote().IsDealerPriceAvailable && self.priceQuote().dealerPriceQuote.offers.length > 0)
            window.location.href = "/pricequote/bookingsummary_new.aspx";
        return false;
    };
}

function InitVM(cityId) {
    var viewModel = new pqViewModel(vmModelId, cityId);
    modelViewModel = viewModel;
    ko.applyBindings(viewModel, $('#dvBikePrice')[0]);
    viewModel.LoadCity();
}

function loadCity(vm) {
    $(ctrlSelectCity).prop('disabled', true).next().show();
    if (vm.selectedModel()) {
        $.get("/api/PQCityList/?modelId=" + vm.selectedModel(),
            function (data) {
                $(ctrlSelectCity).next().hide();
                if (data) {
                    var city = ko.toJS(data);
                    vm.cities(city.cities);
                    ctrlSelectCity = $("#ddlCity");
                    //$(ctrlSelectCity).trigger("chosen:updated");
                    PQcheckCookies();
                    if (selectElementFromArray(vm.cities(), pqCookieObj.PQCitySelectedId)) {
                        vm.selectedCity(pqCookieObj.PQCitySelectedId);
                        pqCookieObj.PQCitySelectedId = 0;
                    }
                }
            });
    }
}

function loadArea(vm) {
    $(ctrlSelectArea).prop('disabled', true).next().show();
    if (vm.selectedCity()) {
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
                $(ctrlSelectArea).next().hide();
                //$(ctrlSelectArea).trigger("chosen:updated");
                if (selectElementFromArray(vm.areas(), pqCookieObj.PQAreaSelectedId)) {
                    vm.selectedArea(pqCookieObj.PQAreaSelectedId);
                    pqCookieObj.PQAreaSelectedId = 0;
                }
            }
            else {
                vm.areas([]);
               // $(ctrlSelectArea).trigger("chosen:updated");
                vm.FetchPriceQuote();
            }
        })
        .fail(function () {
            //no areas available;
            vm.areas([]);
           // $(ctrlSelectArea).trigger("chosen:updated");
            vm.FetchPriceQuote();
        });
    }
    else {
        vm.areas([]);
       // $(ctrlSelectArea).trigger("chosen:updated");
        $(".available-offers-container").hide();
        $(offerBtnContainer).show();
        $(".city-select-text").removeClass("text-red").show();
    }
}

function fetchPriceQuote(vm) {
    $(priceBlock).find("span.price-loader").show();
    //$("#dvAvailableOffer").empty();
    if (vm.selectedModel() != undefined && vm.selectedCity() != undefined) {
        $.ajax({
            url: "/api/OnRoadPrice/?cityId=" + vm.selectedCity() + "&modelId=" + vm.selectedModel() + "&clientIP=" + clientIP + "&sourceType=" + 1 + "&areaId=" + (vm.selectedArea() != undefined ? vm.selectedArea() : 0),
            type: "GET",
            contentType: "application/json",
        }).done(function (data) {
            if (data) {
                var pq = ko.toJS(data);
                vm.priceQuote(pq);

                $(priceBlock).find("span.price-loader").hide();
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

                    animatePrice($(bikePrice), 1000, totalPrice);
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
            }
            else {
                vm.areas([]);
                pqAreaFailStatus();
            }
        })
        .fail(function () {
            vm.areas([]);
            pqAreaFailStatus();
        });
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


$(document).ready(function () {

    if (isUsed == "False") {
        InitVM(cityId);
    }

    $(offerBtnContainer).show();
    $(offerBtn).click(function () {
        if (modelViewModel.selectedCity() == undefined || modelViewModel.selectedArea() == undefined) {
            $('.offer-error').addClass("text-red");
            $('.city-select-text').addClass("text-red");
            if (modelViewModel.selectedCity() != undefined && modelViewModel.selectedArea() == undefined) {
                $('.area-select-text').addClass("text-red");
            }
        }
    });


    $("#mainCity li").click(function () {
        var val = $(this).attr('cityId');
        $(".offer-error").removeClass("show").addClass("hide");
        $(".city-onRoad-price-container").hide();
        $(offerBtnContainer).show();
        $('.offer-error').removeClass("text-red");
        if (val) {
            $(ctrlSelectCity).find(" option[value=" + val + "]").prop('selected', 'selected');
            $(ctrlSelectCity).trigger('change');
        }
    });

    $(editBtn).on('click', function (e) {
        if (modelViewModel.areas() && modelViewModel.selectedArea()) {
            $(".available-offers-container").removeClass("text-red").show();
            $('.area-select-text').removeClass("text-red").show();
        }
        else {
            $(".available-offers-container").hide();
            $(offerBtnContainer).show();
            $(".city-select-text").removeClass("text-red").show();
        }

        $(".city-area-select-container").show();
        $(".city-area-wrapper").show();
        $(".city-onRoad-price-container").hide();

    });

    $("a.read-more-btn").click(function () {
        $(".model-about-more-desc").slideToggle();
        var a = $(this).find("span");
        a.text(a.text() === "more" ? "less" : "more");
    });
    $(".more-features-btn").click(function () {
        $(".more-features").slideToggle();
        var a = $(this).find("span");
        a.text(a.text() === "+" ? "-" : "+");
    });

    $(priceBlock).on('click', 'span.view-breakup-text', function () {
        $("div#breakupPopUpContainer").show();
        $(".blackOut-window").show();
    });

    $(".breakupCloseBtn,.blackOut-window").on('mouseup click', function (e) {
        $("div#breakupPopUpContainer").hide();
        $(".blackOut-window").hide();
    });

    $(document).on('keydown', function (e) {
        if (e.keyCode === 27) $("div.breakupCloseBtn").click();
    });

    //$(ctrlSelectCity).chosen({ no_results_text: "No matches found!!" });
    //$(ctrlSelectArea).chosen({ no_results_text: "No matches found!!" });

});

/*
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
*/

//photos corousel function
(function ($) {

    var connector = function (itemNavigation, carouselStage) {
        return carouselStage.jcarousel('items').eq(itemNavigation.index());
    };

    $(function () {

        var carouselStage = $('.carousel-stage').jcarousel();
        var carouselNavigation = $('.carousel-navigation').jcarousel();


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


        $('.prev-stage')
            .on('jcarouselcontrol:inactive', function () {
                $(this).addClass('inactive');
            })
            .on('jcarouselcontrol:active', function () {
                $(this).removeClass('inactive');
            })
            .jcarouselControl({
                target: '-=1'
            });

        $('.next-stage')
            .on('jcarouselcontrol:inactive', function () {
                $(this).addClass('inactive');
            })
            .on('jcarouselcontrol:active', function () {
                $(this).removeClass('inactive');
            })
            .jcarouselControl({
                target: '+=1'
            });


        $('.prev-navigation')
            .on('jcarouselcontrol:inactive', function () {
                $(this).addClass('inactive');
            })
            .on('jcarouselcontrol:active', function () {
                $(this).removeClass('inactive');
            })
            .jcarouselControl({
                target: '-=1'
            });

        $('.next-navigation')
            .on('jcarouselcontrol:inactive', function () {
                $(this).addClass('inactive');
            })
            .on('jcarouselcontrol:active', function () {
                $(this).removeClass('inactive');
            })
            .jcarouselControl({
                target: '+=1'
            });
    });
})(jQuery);

//Animate the element's value from start to end:
function animatePrice(ele, start, end) {
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
}