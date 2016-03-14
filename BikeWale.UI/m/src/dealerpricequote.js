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

var sliderComponentA, sliderComponentB;

$(document).ready(function (e) {

    sliderComponentA = $("#downPaymentSlider").slider({
        range: "min",
        min: 0,
        max: 1000000,
        step: 50000,
        value: 50000,
        slide: function (e, ui) {
            changeComponentBSlider(e, ui);
        },
        change: function (e, ui) {
            changeComponentBSlider(e, ui);
        }
    })

    sliderComponentB = $("#loanAmountSlider").slider({
        range: "min",
        min: 0,
        max: 1000000,
        step: 50000,
        value: 1000000 - $('#downPaymentSlider').slider("option", "value"),
        slide: function (e, ui) {
            changeComponentASlider(e, ui);
        },
        change: function (e, ui) {
            changeComponentASlider(e, ui);
        }
    });

    $("#tenureSlider").slider({
        range: "min",
        min: 12,
        max: 84,
        step: 6,
        value: 36,
        slide: function (e, ui) {
            $("#tenurePeriod").text(ui.value);
        }
    });

    $("#rateOfInterestSlider").slider({
        range: "min",
        min: 0,
        max: 20,
        step: 0.25,
        value: 5,
        slide: function (e, ui) {
            $("#rateOfInterestPercentage").text(ui.value);
        }
    });

    $("#downPaymentAmount").text($("#downPaymentSlider").slider("value"));
    $("#loanAmount").text($("#loanAmountSlider").slider("value"));
    $("#tenurePeriod").text($("#tenureSlider").slider("value"));
    $("#rateOfInterestPercentage").text($("#rateOfInterestSlider").slider("value"));

});

function changeComponentBSlider(e, ui) {
    if (!e.originalEvent) return;
    var totalAmount = 1000000;
    var amountRemaining = totalAmount - ui.value;
    $('#loanAmountSlider').slider("option", "value", amountRemaining);
    $("#loanAmount").text(amountRemaining);
    $("#downPaymentAmount").text(ui.value);
};

function changeComponentASlider(e, ui) {
    if (!e.originalEvent) return;
    var totalAmount = 1000000;
    var amountRemaining = totalAmount - ui.value;
    $('#downPaymentSlider').slider("option", "value", amountRemaining);
    $("#downPaymentAmount").text(amountRemaining);
    $("#loanAmount").text(ui.value);
};