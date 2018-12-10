window.priceQuote = {
    pageIds: { PQPAGECITYCHANGE: 66, PQPAGEVERSIONCHANGE: 65 },
    rdbSolid: $(".radio-solid"),
    rdbMetallic: $(".radio-metallic"),
    changeVersionPriceNotAvailable: $(".other-options__by-version"),
    cards: {},
    pqContainer: $(".pq-container"),
    cardNo: "card1", //since only one car is shown on desktop

    registerEvents: function () {
        priceQuote.rdbSolid.on('click', function () {
            //functions will be added later on
            $(".js-solid").show();
            $(".js-metallic").hide();
            priceQuoteCommon.solidMetallicToggle(this, priceQuote.cardNo);
        });

        priceQuote.rdbMetallic.on('click', function () {
            //functions will be added later on
            $(".js-solid").hide();
            $(".js-metallic").show();
            priceQuoteCommon.solidMetallicToggle(this, priceQuote.cardNo);
        });

        $(".near-by-city").on("click", function () {
            priceQuote.getPqUsingNearByCity(this);
        });

        $(".redirect-pqpage__js").on("click", function () {
            var viewBreakUpLink = $(this);
            var pqInput = {
                modelId: viewBreakUpLink.attr("data-modelid"),
                versionId: viewBreakUpLink.attr("data-versionid"),
                location: {
                    cityId: viewBreakUpLink.attr("data-cityid"),
                    areaId: viewBreakUpLink.attr("data-areaid")
                },
                pageId: viewBreakUpLink.attr("data-pageid")
            }
            PriceBreakUp.Quotation.RedirectToPQ(pqInput, true);
        });
    },

    getPqUsingNearByCity: function (self) {
        var versionId = $(self).attr("data-attr-versionid");
        var cityId = $(self).attr("data-attr-cityid");
        var cityName = $(self).attr("data-attr-cityname");
        var isCrossSellPriceQuote = $(self).attr("data-attr-iscrosssellpricequote");
        var pageId = $(self).attr("data-attr-pageid");
        var node = $(self);
        var cardId = $(self).attr("data-attr-cardno");
        var hideCampaign = $(self).attr("data-attr-hidecampaign");

        if ($.inArray(Number(cityId), askingAreaCityId) >= 0) {
            LocationSearch($(self), {
                showCityPopup: true,
                setGlobalCookie: false,
                callback: function (locationObj) {
                    var areaId = locationObj.areaId;
                    if (typeof areaId == 'undefined') {
                        areaId = 0;
                    }
                    priceQuote.getVersionPriceQuote(pageParams.modelId, pageParams.versionId, locationObj.cityId, areaId, priceQuote.pageIds.PQPAGECITYCHANGE, false);
                },
                isDirectCallback: true,
                isAreaOptional: false,
                defaultPopup: false,
                ctaText: 'check now',
                defaultPopupOpen: true,
                validationFunction: function () {
                    return { cityId: cityId, cityName: cityName, isComplete: false };
                }
            });
        }
        else {
            priceQuote.getVersionPriceQuote(0, versionId, cityId, 0, pageId, hideCampaign);
        }
    },

    getVersionPriceQuote: function (modelId, versionId, cityId, areaId, pageId, hideCampaign) {
        var url = priceQuoteCommon.getQuotationUrl(modelId, versionId, cityId, areaId, pageId, hideCampaign);
        location.href = url;
    },

    initCityPopUp: function () {
        LocationSearch($('.js-model-details__city-selection'), {
            showCityPopup: true,
            setGlobalCookie: false,
            callback: function (locationObj) {
                var areaId = locationObj.areaId;
                if (typeof areaId == 'undefined') {
                    areaId = 0;
                }
                priceQuote.getVersionPriceQuote(pageParams.modelId, pageParams.versionId, locationObj.cityId, areaId, priceQuote.pageIds.PQPAGECITYCHANGE, false);
            },
            isDirectCallback: true,
            isAreaOptional: false,
            defaultPopup: false,
            ctaText: 'check now',
            validationFunction: function () {
                return;
            }
        });
    },

    redirectToSellCar: function () {
        window.open("/used/sell/");
    },

    charge: {

        postChargeSelection: function (self) {
            var onRoadPriceElement = $(self).closest(".js-optional-popup").find('.price-breakup__wrapper');

            priceQuoteCommon.chargeGroup.showTotal(self);
            priceQuoteCommon.onRoadPrice.showEffectiveOnRoadPrice(null, self, priceQuote.cardNo, onRoadPriceElement);
            priceQuote.priceBreakUp.show(priceQuote.cardNo, self);
        },

        reset: function (self) {
            var chargeObj = priceQuoteCommon.charge.formChargeObj(self);
            var onRoadPriceElement = $(self).closest(".js-optional-popup").find('.price-breakup__wrapper');

            priceQuoteCommon.resetCharge.triggerResetLink(self);
            priceQuoteCommon.onRoadPrice.showEffectiveOnRoadPrice(null, self, chargeObj.cardNo, onRoadPriceElement);
            priceQuote.priceBreakUp.show(chargeObj.cardNo, self);
        },

        getOptionalPackages: function (self) {
            var optionalPackages = priceQuoteCommon.charge.getOptionalPackages();
            if (optionalPackages.length > 0) {
                $(self).html(optionalPackages);
            }
        }
    },

    priceBreakUp: {

        createPriceBreakupHtml: function (cardNo, charges, onRoadPrice) {
            var tbody = $('<tbody>');

            for (var i = 0; i < charges.length; i++) {
                var currentCharge = charges[i];

                var row = $('<tr>');
                var nameTd = $('<td>').text(currentCharge.chargeGroupName).addClass("float-price-breakup__description price-table__description");
                if (currentCharge.charges != "") {
                    nameTd.append($('<span>').addClass('float-price-breakup__items').text(" (" + currentCharge.charges + ")"));
                }

                var priceTd = $('<td>').text(Common.utils.formatNumeric(currentCharge.totalPrice)).addClass("float-price-breakup__amount price-table__amount");
                row.append(nameTd, priceTd);
                tbody.append(row);
            }

            if (typeof onRoadPrice === "undefined" || onRoadPrice === null) {
                onRoadPrice = priceQuoteCommon.onRoadPrice.calculate(cardNo);
            }
            var totalRow = $('<tr>').addClass("total-col price-table__total");
            var totalNameTd = $('<td>').text("On Road Price").addClass("float-price-breakup__description price-table__description");
            var totalPriceTd = $('<td>').html(priceQuoteCommon.rupeeUnicode + " " + Common.utils.formatNumeric(onRoadPrice))
                                .addClass("float-price-breakup__amount price-table__amount");

            totalRow = totalRow.append(totalNameTd, totalPriceTd);
            tbody = tbody.append(totalRow);

            return tbody;
        },

        show: function (cardNo, self) {
            var selectedCharges = priceQuote.cards[cardNo];
            var charges = priceQuoteCommon.charge.getAllChargesToDisplay(selectedCharges);
            var onRoadPrice = priceQuoteCommon.onRoadPrice.calculate(cardNo);
            var onRoadPriceElement = $(self).closest(".js-optional-popup").find('.price-breakup__wrapper');
            priceQuoteCommon.onRoadPrice.showEffectiveOnRoadPrice(null, self, cardNo, onRoadPriceElement, onRoadPrice);

            var tbody = this.createPriceBreakupHtml(cardNo, charges, onRoadPrice);
            $("." + cardNo + " .price-breakup-table").html(tbody);
        }
    }
}

$(document).on('ready', function (event) {
    if (pageParams.isSolidPricePresent) {
        $(".js-solid").show();
        $(".js-metallic").hide();
    }
    else if (pageParams.isMetallicPricePresent) {
        //atf css hides js-metallic class on page load
        $(".js-metallic").show();
    }

    // for emi popup
    if (typeof EmiCalculator !== "undefined") {
        EmiCalculator.EmiCalculatorDocReady();
    }
    if (typeof EmiCalculatorExtended !== "undefined") {
        EmiCalculatorExtended.setInitialEMIModelResult();
    }

    //version dropdown
    var dropdown = new Dropdown('#versionDropDown', {
        selectionBoxClass: 'version-dropdown'
    });

    //optional packages popup
    var optionalPackagesPopupSolid = new Popup('.js-customize-car-link.js-solid', {
        onPopupOpen: function () {
            priceQuote.priceBreakUp.show(priceQuote.cardNo, $('.js-customize-car-link.js-solid'));
        },

        onCloseClick: function () {
            priceQuote.charge.getOptionalPackages('.content-card__optional-packages .content-card-details__description.js-solid');
        }
    });
    var optionalPackagesPopupMetallic = new Popup('.js-customize-car-link.js-metallic', {
        onPopupOpen: function () {
            priceQuote.priceBreakUp.show(priceQuote.cardNo, $('.js-customize-car-link.js-metallic'));
        },

        onCloseClick: function () {
            priceQuote.charge.getOptionalPackages('.content-card__optional-packages .content-card-details__description.js-metallic');
        }
    });

    // edit model popup
    var editModelPopup = new Popup('.js-edit-model-link', {
        onPopupOpen: function () {
            $('.lazy').lazyload();
        }
    });

    $('.versiondrp__list_js .versiondrp__item_js').on('click', function () {
        var versionId = $(this).attr("data-versionid");
        priceQuote.getVersionPriceQuote(pageParams.modelId, versionId, pageParams.cityId, pageParams.areaId, priceQuote.pageIds.PQPAGEVERSIONCHANGE, false);
    });

    $('.jcarousel-wrapper').on('jcarousel:animateend', function (event, carousel) {
        $(this).find('.lazy').lazyload();
    });

    //$(".other-oem-packages__li, .other-oem-packages__li-accessories").on("click", function (e) {
    //    priceQuoteCommon.animation.createRipple(e);
    //});

    //location popup
    priceQuote.registerEvents();
    priceQuote.initCityPopUp();
    priceQuoteCommon.cardArrays.initialize();
})