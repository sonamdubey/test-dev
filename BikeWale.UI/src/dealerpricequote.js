var detailsSubmitBtn = $("#user-details-submit-btn, #buyingAssistBtn");
var getCityArea = GetGlobalCityArea();
var getOfferClick = false;
var getMoreDetailsClick = false;
var getEMIClick = false;
var msg = "";

$(function () {

    $("#btnEmiQuote").on('click', function () {
        triggerGA('Dealer_PQ', 'Get_EMI_Quote_Clicked', bikeVerLocation);
        getEMIClick = true;
        getOfferClick = false;
    });

});

$('#ddlVersion').on("change", function () {
    $('#hdnVariant').val($(this).val());
    triggerGA('Dealer_PQ', 'Version_Changed', bikeVerLocation);
});



// version dropdown
$('.chosen-select').chosen();

$(document).ready(function () {
    // version dropdown
    var selectDropdownBox = $('.select-box-no-input');

    selectDropdownBox.each(function () {
        var text = $(this).find('.chosen-select').attr('data-title'),
            searchBox = $(this).find('.chosen-search')

        searchBox.empty().append('<p class="no-input-label">' + text + '</p>');
    });
});
var variantsDropdown = $(".variants-dropdown"),
variantSelectionTab = $(".variant-selection-tab"),
variantUL = $(".variants-dropdown-list"),
variantListLI = $(".variants-dropdown-list li");

variantsDropdown.click(function (e) {
    if (!variantsDropdown.hasClass("open"))
        $.variantChangeDown(variantsDropdown);
    else
        $.variantChangeUp(variantsDropdown);
});

$.variantChangeDown = function (variantsDropdown) {
    variantsDropdown.addClass("open");
    variantUL.show();
};

$.variantChangeUp = function (variantsDropdown) {
    variantsDropdown.removeClass("open");
    variantUL.slideUp();
};


$(document).mouseup(function (e) {
    if (!$(".variants-dropdown, .variant-selection-tab, .variant-selection-tab #upDownArrow").is(e.target)) {
        $.variantChangeUp($(".variants-dropdown"));
    }
});

var assistanceGetName = $('#assistanceGetName'),
    assistanceGetEmail = $('#assistanceGetEmail'),
    assistanceGetMobile = $('#assistanceGetMobile');


$(document).ready(function () {
    var sidebarHeight = false;
    if ($('#pqBikeDetails').height() < 400) {
        $('#PQDealerSidebarContainer').css({ 'padding-bottom': '20px' });
        $('#PQDealerSidebarContainer .pqdealer-and-listing-container').css({ 'height': '350px' });
        sidebarHeight = true;
    }
    if (!sidebarHeight) {
        if ($('#pqBikeDetails').height() < 470) {
            $('#PQDealerSidebarContainer').css({ 'padding-bottom': '20px' });
            $('#PQDealerSidebarContainer .pqdealer-and-listing-container').css({ 'height': '400px' });
            sidebarHeight = true;
        }
    }
    if (!sidebarHeight) {
        if ($('#pqBikeDetails').height() < 500) {
            $('#PQDealerSidebarContainer').css({ 'padding-bottom': '20px' });
            $('#PQDealerSidebarContainer .pqdealer-and-listing-container').css({ 'height': '450px' });
        }
    }

    var breadcrumbFlag,
        breadcrumbDiv = $('.breadcrumb');

    var $window = $(window),
        disclaimerText = $('#disclaimerText'),
        PQDealerSidebarContainer = $('#PQDealerSidebarContainer'),
        dealerPriceQuoteContainer = $('#dealerPriceQuoteContainer'),
        PQDealerSidebarHeight;
    $(window).scroll(function () {
        PQDealerSidebarHeight = PQDealerSidebarContainer.height();
        var windowScrollTop = $window.scrollTop(),
            disclaimerTextOffset = disclaimerText.offset(),
            dealerPriceQuoteContainerOffset = dealerPriceQuoteContainer.offset(),
            breadcrumbOffsetTop = breadcrumbDiv.offset().top;

        if (breadcrumbOffsetTop < 100)
            breadcrumbFlag = true;
        else
            breadcrumbFlag = false;

        if ($('#dealerPriceQuoteContainer').height() > 500) {
            if (windowScrollTop < dealerPriceQuoteContainerOffset.top - 50) {
                PQDealerSidebarContainer.css({ 'position': 'relative', 'top': '0', 'right': '0' })
            }
            else if (windowScrollTop > (disclaimerTextOffset.top - PQDealerSidebarHeight - 80)) {
                if (breadcrumbFlag)
                    PQDealerSidebarContainer.css({ 'position': 'relative', 'top': disclaimerTextOffset.top - PQDealerSidebarHeight - 150, 'right': '0' })
                else
                    PQDealerSidebarContainer.css({ 'position': 'relative', 'top': disclaimerTextOffset.top - PQDealerSidebarHeight - 240, 'right': '0' })
            }
            else {
                PQDealerSidebarContainer.css({ 'position': 'fixed', 'top': '50px', 'right': $(window).innerWidth() - (996 + $('#dealerPriceQuoteContainer').offset().left - 11) })
            }
        }
    });
});

function loadDisclaimer(dealerType) {
    $("#read-less").hide();
    if (dealerType == 'Premium') {
        $("#read-more").load("/statichtml/premium.html");
    } else {
        $("#read-more").load("/statichtml/standard.html");
    }
    $("#read-more").show();
}

$('.tnc').on('click', function (e) {
    LoadTerms($(this).attr("id"));
});

$('#termsPopUpCloseBtn').on("click", function () {
    $(".blackOut-window").hide();
    $("div#termsPopUpContainer").hide()
});

$('.blackOut-window').on("click", function () {
    $(".blackOut-window").hide();
    $("div#termsPopUpContainer").hide()
});

function LoadTerms(offerId) {
    $("div#termsPopUpContainer").show();
    $(".blackOut-window").show();
    if (offerId != 0 && offerId != null) {
        $('#termspinner').show();
        $('#terms').empty();
        $.ajax({
            type: "GET",
            url: "/api/Terms/?offerId=" + offerId,
            dataType: 'json',
            success: function (response) {
                if (response != null)
                    $('#terms').html(response);
                $(".termsPopUpContainer").css('height', '500');
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

// tooltip
$('.bw-tooltip').on('click', '.close-bw-tooltip', function () {
    var tooltipParent = $(this).closest('.bw-tooltip');

    if (!tooltipParent.hasClass('slideUp-tooltip')) {
        tooltipParent.fadeOut();
    }
    else {
        tooltipParent.slideUp();
    }
});