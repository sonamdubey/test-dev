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

$('#change-location').on('click', function () {
    cityArea.open();
    appendHash("cityAreaPopup");
    cityArea.openList(tabParent);
    cityArea.closeList(areaMenu);
    areaMenu.hide();
});

$('#city-area-popup .white-back-arrow').on('click', function () {
    cityArea.close();
    window.history.back();
});

$('#city-area-content').on('click', '#city-menu-tab', function () {
    var tab = $(this),
        tabParent = tab.parent('.city-area-menu'),
        cityAreaContent = $('#city-area-content');

    if (cityAreaContent.hasClass('city-selected')) {
        var areaMenu = $('#area-menu');

        if (!tabParent.hasClass('open')) {
            cityArea.openList(tabParent);
            cityArea.closeList(areaMenu);
            areaMenu.hide();
        }
        else {
            cityArea.closeList(tabParent);
            areaMenu.show();
            cityArea.openList(areaMenu);
        }
    }
});

$(".inputbox-list-wrapper").on("click", "li", function () {
    var item = $(this);
    if (!item.hasClass('active')) {
        cityArea.setSelection(item);
    }
});

var cityArea = {
    popup: $('#city-area-popup'),

    open: function () {
        cityArea.popup.show();
        $('body, html').addClass('lock-browser-scroll');
    },

    close: function () {
        cityArea.popup.hide();
        $('body, html').removeClass('lock-browser-scroll');
    },

    openList: function (wrapper) {
        wrapper.find('.inputbox-list-wrapper').slideDown();
        wrapper.addClass('open');
    },

    closeList: function (wrapper) {
        wrapper.find('.inputbox-list-wrapper').slideUp();
        wrapper.removeClass('open');
    },

    setSelection: function (item) {
        var selectionText = item.text(),
            wrapper = item.closest('.city-area-menu');

        wrapper.find('li').removeClass('active');
        item.addClass('active');
        cityArea.setLabel(selectionText, wrapper);
    },

    setLabel: function (itemText, wrapper) {
        var tabLabel = wrapper.find('.city-area-tab-label');

        if (wrapper.attr('id') == 'city-menu') {
            var areaMenu = $('#area-menu');
            $('#city-area-content').addClass('city-selected');
            tabLabel.text('City: ' + itemText);
            cityArea.closeList(wrapper);
            cityArea.resetLabel('Select your area', areaMenu);
            areaMenu.show();
            cityArea.openList(areaMenu);
            areaMenu.find('li').removeClass('active');
        }
        else {
            tabLabel.text('Area: ' + itemText);
            $('#city-area-popup .white-back-arrow').trigger('click');
        }
    },

    resetLabel: function (message, wrapper) {
        wrapper.find('.city-area-tab-label').text(message);
    },
}


$('#city-menu-input').on('focus', function (event) {
    event.stopPropagation();
    $("#city-area-popup").animate({ scrollTop: 147 });
});

$('#area-menu-input').on('focus', function (event) {
    event.stopPropagation();
    $("#city-area-popup").animate({ scrollTop: 190 });
});

$("#city-menu-input, #area-menu-input").on("keyup", function () {
    var inputbox = $(this);
    locationFilter(inputbox);

    if (inputbox.val().length == 0) {
        var wrapper = inputbox.closest('.city-area-menu');

        if (wrapper.attr('id') == 'city-menu') {
            cityArea.resetLabel('Select your city', wrapper);
        }
        else {
            cityArea.resetLabel('Select your area', wrapper);
        }
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
