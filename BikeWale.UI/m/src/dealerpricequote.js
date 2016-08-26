var offersPopupDiv = $("#offersPopup");

$(".view-offers-target").on("click", function () {
    offersPopupOpen(offersPopupDiv);
    appendHash("offersPopup");
});

$('.tnc').on('click', function (e) {
    LoadTerms($(this).attr("id"));
});

function LoadTerms(offerId) {
    $("div#termsPopUpContainer").show();
    $(".blackOut-window").show();
    if (offerId != 0 && offerId != null) {
        $('#termspinner').show();
        $('#terms').empty();
        $.ajax({
            type: "GET",
            url: "/api/Terms/?offerMaskingName=&offerId=" + offerId,
            dataType: 'json',
            success: function (response) {
                if (response != null)
                    $('#terms').html(response);
            },
            error: function (request, status, error) {
                $("div#termsPopUpContainer").hide();
                $(".blackOut-window").hide();
            }
        });
    }
    else {
        $("#terms").load("/statichtml/tnc.html");
    }
    $('#termspinner').hide();
}

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
    $('body, html').addClass('lock-browser-scroll');
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
    $('body, html').removeClass('lock-browser-scroll');
};

$('.btn-grey-state').on('click', function () {
    $(this).addClass('button-clicked-state');
    setTimeout(function () { $('.btn-grey-state').removeClass('button-clicked-state'); }, 100);
});
$('#getMoreDetails').on('click', function () {
    getMoreDetailsClicked = true;
});

/**/

$(document).ready(function () {
    $('#bw-header').addClass('fixed');
    //dropdown.setDropdown();

    var $window = $(window),
        buttonWrapper = $('#pricequote-floating-button-wrapper'),
        floatingButton = buttonWrapper.find('.float-button'),
        windowHeight,
        body = $('body');

    $(window).scroll(function () {
        var windowScrollTop = $(this).scrollTop();
        if (buttonWrapper && buttonWrapper.offset()) {
            buttonWrapperTop = buttonWrapper.offset().top;

            windowHeight = $(this).height() - 63;

            if (windowScrollTop + windowHeight > buttonWrapperTop) {
                floatingButton.removeClass('float-fixed');
                body.addClass('floating-btn-inactive');
            }
            else {
                floatingButton.addClass('float-fixed');
                body.removeClass('floating-btn-inactive');
            }
        }
    });

});

$('.dropdown-select-wrapper').on('click', '.dropdown-label', function () {
    dropdown.active($(this));
});

$('.dropdown-select-wrapper').on('click', '.dropdown-menu-list.dropdown-with-select li', function () {
    var element = $(this);
    if (!element.hasClass('active')) {
        dropdown.selectItem($(this));
        dropdown.selectOption($(this));
    }
});

$(document).on('click', function (event) {
    event.stopPropagation();
    var bodyElement = $('body'),
		dropdownLabel = bodyElement.find('.dropdown-label'),
		dropdownList = bodyElement.find('.dropdown-menu-list'),
		noSelectLabel = bodyElement.find('.dropdown-selected-item');

    if (!$(event.target).is(dropdownLabel) && !$(event.target).is(dropdownList) && !$(event.target).is(noSelectLabel)) {
        dropdown.inactive();
    }


});

$("#getMoreDetailsBtnCampaign").on("click", function () {
    $("#leadCapturePopup").show();
    $('body').addClass('lock-browser-scroll');
    dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Model_Page', 'act': 'Get_More_Details_Clicked', 'lab': bikeName + "_" + getCityArea });
});

var swiper = new Swiper('.pq-secondary-dealer-swiper', {
    slidesPerView: 'auto',
    spaceBetween: 0
});
