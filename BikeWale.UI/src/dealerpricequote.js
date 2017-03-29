var selectDropdownBox, msg = "";
var getCityArea = GetGlobalCityArea();
var getOfferClick = false, getMoreDetailsClick = false, getEMIClick = false;

var variantsDropdown, variantSelectionTab, variantUL, variantListLI;

var variantChangeDown = function (variantsDropdown) {
    variantsDropdown.addClass("open");
    variantUL.show();
};

var variantChangeUp = function (variantsDropdown) {
    variantsDropdown.removeClass("open");
    variantUL.slideUp();
};

function registerPQAndReload(eledealerId, eleversionId) {
    try {
        var isSuccess = false;

        var objData = {
            "dealerId": eledealerId || dealerId,
            "modelId": modelId,
            "versionId": eleversionId || versionId,
            "cityId": cityId,
            "areaId": areaId,
            "clientIP": clientIP,
            "pageUrl": pageUrl,
            "sourceType": 2,
            "pQLeadId": pqSourceId,
            "deviceId": getCookie('BWC')
        };

        isSuccess = dleadvm.registerPQ(objData);

        if (isSuccess) {
            var rediurl = "CityId=" + cityId + "&AreaId=" + areaId + "&PQId=" + dleadvm.pqId() + "&VersionId=" + objData.versionId + "&DealerId=" + objData.dealerId;
            window.location.href = "/pricequote/dealer/?MPQ=" + Base64.encode(rediurl);
        }
    } catch (e) {
        console.warn("Unable to create pricequote : " + e.message);
    }
}

function secondarydealer_Click(dealerID) {
    triggerGA('Dealer_PQ', 'Secondary_Dealer_Card_Clicked', bikeVerLocation);
    registerPQAndReload(dealerID);
}

function openLeadCaptureForm(dealerID) {
    triggerGA('Dealer_PQ', 'Secondary_Dealer_Get_Offers_Clicked', bikeVerLocation);
    event.stopPropagation();
}

function formatPrice(price) {
    price = price.toString();
    var lastThree = price.substring(price.length - 3);
    var otherNumbers = price.substring(0, price.length - 3);
    if (otherNumbers != '')
        lastThree = ',' + lastThree;
    var price = otherNumbers.replace(/\B(?=(\d{2})+(?!\d))/g, ",") + lastThree;
    return price;
}

function loadDisclaimer(dealerType) {
    $("#read-less").hide();
    if (dealerType == 'Premium') {
        $("#read-more").load("/statichtml/premium.html");
    } else {
        $("#read-more").load("/statichtml/standard.html");
    }
    $("#read-more").show();
}

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

docReady(function () {

    // version dropdown
    selectDropdownBox = $('.select-box-no-input');

    variantsDropdown = $(".variants-dropdown"), variantSelectionTab = $(".variant-selection-tab"),
    variantUL = $(".variants-dropdown-list"), variantListLI = $(".variants-dropdown-list li");

    var breadcrumbFlag, breadcrumbDiv = $('.breadcrumb');
    var sidebarHeight = false;
    var $window = $(window),
        disclaimerText = $('#disclaimerText'),
        PQDealerSidebarContainer = $('#PQDealerSidebarContainer'),
        dealerPriceQuoteContainer = $('#dealerPriceQuoteContainer'),
        PQDealerSidebarHeight;

    // version dropdown
    $('.chosen-select').chosen();

    if ($('.pricequote-benefits-list li').length % 2 == 0) {
        $('.pricequote-benefits-list').addClass("pricequote-two-benefits");
    }

    $('#ddlVersion').on("change", function () {
        registerPQAndReload(dealerId, $(this).val());
        triggerGA('Dealer_PQ', 'Version_Changed', bikeVerLocation);
    });

    $("#readmore").on("click", function () {
        loadDisclaimer(dealerType);
    });

    $('.blackOut-window').on("click", function () {
        $("div#termsPopUpContainer").hide();
        $(".blackOut-window").hide();
    });

    $(".leadcapturebtn").click(function (e) {
        ele = $(this);
        var leadOptions = {
            "dealerid": ele.attr('data-item-id'),
            "dealername": ele.attr('data-item-name'),
            "dealerarea": ele.attr('data-item-area'),
            "versionid": versionId,
            "leadsourceid": ele.attr('data-leadsourceid'),
            "pqsourceid": ele.attr('data-pqsourceid'),
            "isleadpopup": ele.attr('data-isleadpopup'),
            "mfgCampid": ele.attr('data-mfgcampid'),
            "pqid": pqId,
            "pageurl": pageUrl,
            "clientip": clientIP,
            "dealerHeading": ele.attr('data-item-heading'),
            "dealerMessage": ele.attr('data-item-message'),
            "dealerDescription": ele.attr('data-item-description'),
            "pinCodeRequired": ele.attr("data-ispincodrequired"),
            "gaobject": {
                cat: ele.attr("c"),
                act: ele.attr("a"),
                lab: bikeVerLocation
            }

        };
        dleadvm.setOptions(leadOptions);
    });

    $("#btnEmiQuote").on('click', function () {
        triggerGA('Dealer_PQ', 'Get_EMI_Quote_Clicked', bikeVerLocation);
        getEMIClick = true;
        getOfferClick = false;
    });

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

    selectDropdownBox.each(function () {
        var text = $(this).find('.chosen-select').attr('data-title'),
            searchBox = $(this).find('.chosen-search')

        searchBox.empty().append('<p class="no-input-label">' + text + '</p>');
    });

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

    $(document).mouseup(function (e) {
        if (!$(".variants-dropdown, .variant-selection-tab, .variant-selection-tab #upDownArrow").is(e.target)) {
            $.variantChangeUp($(".variants-dropdown"));
        }
    });

    variantsDropdown.click(function (e) {
        if (!variantsDropdown.hasClass("open"))
            variantChangeDown(variantsDropdown);
        else
            variantChangeUp(variantsDropdown);
    });
});
