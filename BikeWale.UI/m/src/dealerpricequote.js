var versionByDiv = $(".version-div"),
    versionListDiv = $(".version-selection-div"),
    versionListLI = $(".version-selection-div ul li");

versionByDiv.click(function () {
    if (!versionByDiv.hasClass("open"))
        $.versionChangeDown(versionByDiv);
    else
        $.versionChangeUp(versionByDiv);
});

$.versionChangeDown = function (versionByDiv) {
    versionByDiv.addClass("open");
    versionListDiv.show();
};

$.versionChangeUp = function (sortByDiv) {
    versionByDiv.removeClass("open");
    versionListDiv.slideUp();
};

$(document).mouseup(function (e) {
    if (!$(".variantDropDown, .version-div, .version-div #upDownArrow, .version-by-title").is(e.target)) {
        $.versionChangeUp($(".version-div"));
    }
});
//TODO handle version select event

$(document).ready(function () {
    var pqDealerHeader = $('#pqDealerHeader'),
        pqDealerBody = $('#pqDealerBody'),
        pqRemoveHeader = $('#pqRemoveHeader'),
        pqDealerHeaderWrapper = $('#pqDealerDetails'),
        $window = $(window),
        floatButton = $('.float-button'),
        bodHt, footerHt, scrollPosition;
        $window.scroll(function () {
            if ($('#pqDealerHeader')[0] != undefined) {
                if (!pqDealerHeader.hasClass('pq-fixed')) {
                    if ($window.scrollTop() > pqDealerHeader.offset().top && $window.scrollTop() < pqRemoveHeader.offset().top - 40) { //subtract 40px (pq header height)
                        pqDealerHeader.addClass('pq-fixed').find('.dealership-name').addClass('text-truncate padding-bottom5 border-light-bottom');
                        pqDealerBody.addClass('padding-top40');
                    }
                }
                else if (pqDealerHeader.hasClass('pq-fixed')) {
                    if ($window.scrollTop() < pqDealerHeaderWrapper.offset().top || $window.scrollTop() > pqRemoveHeader.offset().top - 40) { //subtract 40px (pq header height)
                        pqDealerHeader.removeClass('pq-fixed').find('.dealership-name').removeClass('text-truncate padding-bottom5 border-light-bottom');
                        pqDealerBody.removeClass('padding-top40');
                    }
                }
            }
            bodHt = $('body').height();
            footerHt = $('footer').height();
            scrollPosition = $(this).scrollTop();
            if (floatButton.offset().top < $('footer').offset().top - 50)
                floatButton.addClass('float-fixed');
            if (floatButton.offset().top > $('footer').offset().top - 50)
                    floatButton.removeClass('float-fixed');
        });
    
});

var offersPopupDiv = $("#offersPopup");

$(".view-offers-target").on("click", function () {
    offersPopupOpen(offersPopupDiv);
    appendHash("offersPopup");
});

$("#ddlVersion").on("change", function () {
    versionName = $(this).children(":selected").text();    
    dataLayer.push({ "event": "Bikewale_all", "cat": "Dealer_PQ", "act": "Version_Changed", "lab": bikeName + "_" + versionName + "_" + getCityArea });
});

$("#calldealer").on("click", function () {
    dataLayer.push({ "event": "Bikewale_all", "cat": "Dealer_PQ", "act": "Call_Dealer_Clicked", "lab": bikeName + "_" + versionName + "_" + getCityArea });
});

$("#leadBtnBookNow").on("click", function () {   
    dataLayer.push({ "event": "Bikewale_all", "cat": "Dealer_PQ", "act": "Get_Offers_Clicked", "lab": bikeName + "_" + versionName + "_" + getCityArea });
    getOffersClicked = true;
    getEMIClicked = false;
});

$("#btnEmiQuote").on("click", function () {
    dataLayer.push({ "event": "Bikewale_all", "cat": "Dealer_PQ", "act": "Get_EMI_Quote_Clicked", "lab": bikeName + "_" + versionName + "_" + getCityArea });
    getOffersClicked = false;
    getEMIClicked = true;
});

$("#aDealerNumber").on("click", function () {
    dataLayer.push({ "event": "Bikewale_all", "cat": "Dealer_PQ", "act": "Dealer_Number_Clicked", "lab": bikeName + "_" + versionName + "_" + getCityArea });
});

$(".offers-popup-close-btn").on("click", function () {
    offersPopupClose(offersPopupDiv);
    window.history.back();
});

    var offersPopupOpen = function (offersPopupDiv) {
        offersPopupDiv.show();
    };

    var offersPopupClose = function (offersPopupDiv) {
        offersPopupDiv.hide();
    };

    var emiPopupDiv = $("#emiPopup");

    $(".calculate-emi-target").on("click", function () {
        emiPopupOpen(emiPopupDiv);
        appendHash("emiPopup");
    });

    $(".emi-popup-close-btn").on("click", function () {
        emiPopupClose(emiPopupDiv);
        window.history.back();
    });

    var emiPopupOpen = function (emiPopupDiv) {
        emiPopupDiv.show();
    };

    var emiPopupClose = function (emiPopupDiv) {
        emiPopupDiv.hide();
    };

$('.btn-grey-state').on('click', function () {
    $(this).addClass('button-clicked-state');
    setTimeout(function () { $('.btn-grey-state').removeClass('button-clicked-state'); }, 100);
});
$('#getMoreDetails').on('click', function () {
    getMoreDetailsClicked = true;
});
