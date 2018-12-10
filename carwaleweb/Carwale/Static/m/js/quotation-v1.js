window.isWebview = window.platform && (window.platform == 74 || window.platform == 83);

window.onerror = function (messageOrEvent, source, lineno, colno, error) {
    if (source.indexOf('price-quote.js') > 0) {
        var errorObject = { messageOrEvent: messageOrEvent, source: source, lineno: lineno, colno: colno, error: error };
        $.ajax({
            url: '/api/exceptions/',
            type: 'POST',
            data: JSON.stringify(errorObject),
            contentType: "application/json; charset=utf-8",
            success: function (data, textStatus, xhr) {
                console.log(data);
            },
            error: function (xhr, textStatus, errorThrown) {
                console.log('Error in Operation');
            }
        });
    }
}

var CampaignTemplateBind = {
    setAllCTALeadFormAttribute: function () {

        var btnLinkTextCTA = $('.campaignLinkCTA');
        var linkBreakupCTA = $(".price-breakup__offers");
        var pqTemplate = $(".pq-template .dealer-cta__btn");
        var floatingCTA = $('.dealer-cta-container .floating-cta');
        var offersLink = $(".link-campaign-offers-cta");
        var offersTemplate = $(".dealer-cta-container .open-breakup");

        if (window.isSponsoredCar) {
            $.each(pqTemplate, function (index, item) { setCTALeadFormAttribute(item, 325, 311); });
            $.each(btnLinkTextCTA, function (index, item) { setCTALeadFormAttribute(item, 337, 340); });
            $.each(linkBreakupCTA, function (index, item) { setCTALeadFormAttribute(item, 329, 331); });
            $.each(offersLink, function (index, item) { setCTALeadFormAttribute(item, 344, 347); });
            $.each(offersTemplate, function (index, item) { setCTALeadFormAttribute(item, 344, 347); });
        }
        else {
            $.each(pqTemplate, function (index, item) { setCTALeadFormAttribute(item, 309, 311); });
            $.each(btnLinkTextCTA, function (index, item) { setCTALeadFormAttribute(item, 335, 338); });
            $.each(linkBreakupCTA, function (index, item) { setCTALeadFormAttribute(item, 313, 315); });
            $.each(floatingCTA, function (index, item) { setCTALeadFormAttribute(item, 309, 311); });
            $.each(offersLink, function (index, item) { setCTALeadFormAttribute(item, 344, 347); });
            $.each(offersTemplate, function (index, item) { setCTALeadFormAttribute(item, 344, 347); });
        }
    }
}

document.addEventListener('DOMContentLoaded', CampaignTemplateBind.setAllCTALeadFormAttribute);
var MODELLIST = [];
var lastIndex;
window.priceQuote = {
    cards: {},
    insuranceHeader: "Insurance",
    chargeGroups: { OPTIONALINSURANCE: 9, COMPULSORYINSURANCE: 17 },
    rupeeUnicode: '&#8377;',

    pageIds: { MSITEPQPAGEVERSIONCHANGE: 65, PRICENOTAVAILABLEVERSIONCHANGE: 133 },

    campaignType: { PQ: 1, PAIDCROSSSELL: 2, HOUSECROSSSELL: 3 },

    registerEvents: function () {
        $("a.more-packages").click(function (e) {
            e.preventDefault();
            var self = this;
            var solidOrMetallic = $(this).hasClass("solid-compulsory") ? "solid-compulsory" : "metallic-compulsory";
            var hiddenElem = $(this).closest(".optional-packages").find(".hidden-package." + solidOrMetallic);
            if (!hiddenElem.is(":visible")) {
                hiddenElem.slideDown(200, function () {
                    hiddenElem.removeClass("hide").find('img.lazy').lazyload();
                    $(self).hide();
                });
            }
        });

        $(".oem-section__container").on("click", function (e) {
            event.stopPropagation();
            if ($(this).closest(".oem-swiper").length) {  // For Multiple Packages
                priceQuote.resetAllPackage($(this).siblings());
            }
            var toggleStatus = priceQuote.toggleanimation(this);
            if (toggleStatus === "checked") {
                priceQuote.selectCustomPackages(this);
                priceQuote.showPriceBreakup(this);
            }
            priceQuoteCommon.animation.createRipple(e);
        });

        $(".instruction-icon").on("click", function (event) {
            $(".pq-tool-tip").text('');
            var clickedToolTipAttr = $(this).attr("spnIdentifier");
            var domWithDescription = $('.' + clickedToolTipAttr)
            if (domWithDescription[0] != undefined) {
                $('.pq-tool-tip').append($(domWithDescription.html()));
                priceQuote.showTooltip(this, event);
            }
        });

        $(".car-filters__version").on("click", function () {
            priceQuote.openVersionPopUp(this);
        });
        $(".other-options__by-version").on("click", function () {
            priceQuote.openVersionPopUp(this);
        });

        $(".pq-city-change").on("click", function () {
            priceQuote.changeCity(this);
        });

        $(".select-version-popup__close").on("click", function () {
            $(".select-version-popup").hide();
            priceQuote.removeState();
        });

        $(".other-oem-packages input").on("change", function () {
            //priceQuote.showPriceBreakup(this);
            priceQuote.changeCustomPacakge();
        });

        $(".near-by-city").on("click", function () {
            priceQuote.getPqUsingNearByCity(this);
        });

        $("input[name=color-switcher]").on("change", function () {
            if ($(this).attr("checked")) {
                $(this).closest(".color-switcher__body").removeClass("solid-color metallic-color").addClass("metallic-color");
            } else {
                $(this).closest(".color-switcher__body").removeClass("solid-color metallic-color").addClass("solid-color");
            }
        });

        $(".other-oem-packages__li, .ripple-bontianer, .other-oem-packages__li-accessories, .pq-btn, .car-filters__version, .car-filters__city").on("click", function (e) {
            priceQuoteCommon.animation.createRipple(e);
        });

        $(".price-breakup-wrapper").on("click", function () {
            priceQuote.animateUpPriceBreakup(this);
        });

        $(".close-float-price-breakup").on("click", function () {
            priceQuote.animateDownPriceBreakup(this);
            $('.floating-model-header').removeClass('translate-fixed-model-name');
            priceQuote.removeState();
            event.preventDefault();
            event.stopPropagation();
        });

        $(".grey-patch").on("click", function () {
            priceQuote.animateDownPriceBreakup($(".close-float-price-breakup"));
            $('.floating-model-header').removeClass('translate-fixed-model-name');
            priceQuote.removeState();
            event.stopPropagation();
        });

        $(".customize-car__btn").on('click', function () {
            $(this).hide().closest(".pq-container").removeClass("hide-package");
        });

        $(".radio-solid").on('click', function () {
            var cardNo = $(this).attr('card-no');
            priceQuote.showSolidHideMetallic(cardNo);
            priceQuote.solidMetallicToggle(this, cardNo);
        });

        $(".radio-metallic").on('click', function () {
            var cardNo = $(this).attr('card-no');
            priceQuote.showMetallicHideSolid(cardNo);
            priceQuote.solidMetallicToggle(this, cardNo);
        });

        priceQuote.setGlobalModelList();

        if (typeof EmiCalculator !== "undefined") {
            EmiCalculator.EmiCalculatorDocReady();
        }
    },

    setGlobalModelList: function () {
        MODELLIST = [];
        var cards = $(".pq-container");
        for (var i = 0; i < cards.length; i++) {
            MODELLIST.push(Number(cards.eq(i).find($(".open-modelchange-popup")).attr("data-model")));
        }
    },

    showDefaultPriceQuoteOnLoad: function () {
        var cards = $(".pq-container");
        for (var i = 1; i < cards.length + 1; i++) {
            var cardId = "card" + i;
            this.showPriceQuoteForCard(cardId);
        }
    },

    showPriceQuoteForCard: function (cardId) {
        if ($("." + cardId + " .solid-compulsory").length > 0) {
            priceQuote.showSolidHideMetallic(cardId);
        }
        else {
            priceQuote.showMetallic(cardId);
        }
    },

    showMetallicHideSolid: function (card) {
        $("." + card + " .metallic-compulsory:not(.hidden-package)").show();
        $("." + card + " .solid-compulsory").hide();
        $("." + card + " .other-oem-packages input").attr('checked', false);
    },

    showSolidHideMetallic: function (card) {
        $("." + card + " .solid-compulsory:not(.hidden-package)").show();
        $("." + card + " .metallic-compulsory").hide();
        $("." + card + " .other-oem-packages input").attr('checked', false);
    },

    showMetallic: function (card) {
        $("." + card + " .metallic-compulsory:not(.hidden-package)").show();
    },

    putPriceBreakupBottom: function (pqContainer) {
        var priceBreakupContainer = $(pqContainer).find('.price-breakup-container');
        $("html, body").animate({
            scrollTop: $(priceBreakupContainer).offset().top + $(priceBreakupContainer).outerHeight(true) - $(window).height()
        });
    },

    addDealerCtaBottom: function (pqContainer) {
        var priceBreakupContainer = $(pqContainer).find(".price-breakup-container"),
            dealerCtaContainer = $(".dealer-cta-container"),
            hasDealerCtaContainer = priceBreakupContainer.closest(pqContainer).find($(".dealer-cta-container")).length;
        if (hasDealerCtaContainer != 0) {
            $(priceBreakupContainer).css("bottom", $(dealerCtaContainer).outerHeight());
            $(dealerCtaContainer).addClass("floating-dealer");
        }
        $(dealerCtaContainer).find(".campaign-template").addClass("open-breakup");
    },

    removeDealerCtaBottom: function (pqContainer) {
        var priceBreakupContainer = $(pqContainer).find(".price-breakup-container"),
            dealerCtaContainer = $(".dealer-cta-container");
        $(priceBreakupContainer).css("bottom", 0);
        $(dealerCtaContainer).removeClass("floating-dealer");
        $(dealerCtaContainer).find(".campaign-template").removeClass("open-breakup");
    },

    animateUpPriceBreakup: function (self) {
        var pqContainer = $(self).closest(".pq-container"),
            priceBreakupContainer = $(self).closest(".price-breakup-container"),
            priceBreakDetails = $(priceBreakupContainer).find(".float-price-breakup-details")

        pqContainer.addClass('breakUpActive');

        this.bindPriceBreakup(pqContainer);

        var priceBreakDetailsHeight = $(priceBreakDetails).outerHeight();
        if (!$(priceBreakupContainer).hasClass("open-breakup")) {
            this.trackSummaryCardPullUpDown("shown", pqContainer.attr("make-name"));
            priceQuote.appendState("price-breakup");
            $(priceBreakupContainer).addClass("open-breakup");

            var openBreakUp = $(priceBreakupContainer).hasClass("open-breakup");
            if (openBreakUp) {
                $('.floating-model-header').addClass('translate-fixed-model-name');
            }

            //scroll pricebreakup to bottom
            priceQuote.putPriceBreakupBottom(pqContainer);
            priceBreakupContainer.find(".campaign-link").addClass("open-breakup");
            $(priceBreakupContainer).addClass("floating-dealer");

            $(priceBreakDetails).addClass("collapsed-overflow").css("height", "0px").show().animate({
                height: priceBreakDetailsHeight
            }).scrollTop(0);


            priceQuote.lockPopup();
            priceQuote.addDealerCtaBottom(pqContainer);
        }
    },

    animateDownPriceBreakup: function (self) {
        var pqContainer = $(self).closest(".pq-container"),
            priceBreakupContainer = $(self).closest(".price-breakup-container"),
            priceBreakDetails = $(priceBreakupContainer).find(".float-price-breakup-details"),
            pqCardWithBreakUpOpen = $(pqContainer).parent().find('.breakUpActive');

        pqContainer.removeClass('breakUpActive');

        if ($(priceBreakupContainer).hasClass("open-breakup")) {
            var trackMakeName = "",
                trackedMakeName = $(pqCardWithBreakUpOpen).attr('make-name');

            this.trackSummaryCardPullUpDown("hide", trackedMakeName);
            $(priceBreakDetails).animate({
                height: 0
            }, function () {
                $(this).hide().css("height", "auto").removeClass("collapsed-overflow");
                $(priceBreakupContainer).removeClass("open-breakup");
            });
            priceQuote.removeDealerCtaBottom(pqContainer);
            priceQuote.unlockPopup();
            priceBreakupContainer.find(".campaign-link").removeClass("open-breakup");
            $(priceBreakupContainer).removeClass("floating-dealer");
            $(document).trigger("scroll");
        }
    },

    lockPopup: function () {
        $(".grey-patch").show();
        Common.utils.lockPopup();
        $(".blackOut-window").hide();
        $(document).off("scroll");
        //$('body').css('position', 'fixed');
    },

    unlockPopup: function () {
        $(".grey-patch").hide();
        $('body').css('position', '');
        var html = document.getElementsByTagName('html')[0];
        var scrollTop = parseInt(html.style.top);
        $('html').removeClass('lock-browser-scroll');
        $('html,body').scrollTop(-scrollTop);
        $(document).on("scroll", function () {
            priceQuote.bindScroll();
        });
    },

    showTooltip: function (self, event) {
        event.stopPropagation();
        var leftOffset = $(self).offset().left + 8,
            topOffset = $(self).offset().top - 5;
        if ($(".pq-tool-tip").is(":visible")) {
            previousToolTip = $(".pq-tool-tip");
            previousToolTip.hide();
        }
        if (!$(".pq-tool-tip").is(":visible")) {
            $(".pq-tool-tip").show().css({ top: topOffset, left: leftOffset });

            // check for left position of tooltip relative to current ViewPort
            var tooltipX = $(".pq-tool-tip").offset().left - (window.scrollX ||
                                    window.pageXOffset || document.body.scrollLeft);
            var toolTipY = tooltipX + $(".pq-tool-tip").outerWidth();
            if (tooltipX < 0) {
                tooltipX = 10 - tooltipX;
                $(".pq-tool-tip").css({ left: leftOffset + tooltipX });
            }
            if (toolTipY > $(window).width()) {
                tooltipX = toolTipY - $(window).width() + 10
                $(".pq-tool-tip").css({ left: leftOffset - tooltipX });
            }
        } else {
            $(".pq-tool-tip").hide();
        }
    },

    selectCustomPackages: function (self) {
        var listArray = $(self).closest(".pq-container").find(".oem-package-list__li");
        for (var i = 0; i < listArray.length; i++) {
            var cat = $(listArray[i]).attr("data-category"),
                val = $(listArray[i]).attr("data-value");
            if ($("input[name=" + cat + "]").attr("type") === "checkbox") {
                $("input[name=" + cat + "]").attr('checked', false);
            }
            $("input[name=" + cat + "][value=" + val + "]").attr('checked', 'checked');
        }
    },

    changeCustomPacakge: function () {
        priceQuote.resetAllPackage($(".oem-section__container"));
        //$("input[name=insurance], input[name=accessories], input[name=assistance]").off("change");
    },

    toggleanimation: function (self) {
        var animateCheckmark = $(self).find(".checkmark")[0],
            animateCircle = $(self).find(".checkmark__circle")[0],
            animateCheck = $(self).find(".checkmark__check")[0];

        if (animateCheckmark.classList.contains("checkmarkAnimate") && animateCircle.classList.contains("checkmark__circle-grey") && (animateCheck.classList.contains("checkmark__check-grey"))) {

            animateCheckmark.classList.remove("checkmarkAnimate");
            animateCircle.classList.remove("checkmark__circle-grey");
            animateCheck.classList.remove("checkmark__check-grey");
            return "checked";

        } else {
            animateCheckmark.classList.add("checkmarkAnimate");
            animateCircle.classList.add("checkmark__circle-grey");
            animateCheck.classList.add("checkmark__check-grey");
            return "unchecked";
        }

    },

    resetAllPackage: function (elemArr) {
        for (var i = 0; i < elemArr.length; i++) {
            var animateCheckmark = $(elemArr[i]).find(".checkmark")[0],
            animateCircle = $(elemArr[i]).find(".checkmark__circle")[0],
            animateCheck = $(elemArr[i]).find(".checkmark__check")[0];
            if (!(animateCheckmark.classList.contains("checkmarkAnimate")) && !(animateCircle.classList.contains("checkmark__circle-grey")) && !(animateCheck.classList.contains("checkmark__check-grey"))) {
                animateCheckmark.classList.add("checkmarkAnimate");
                animateCircle.classList.add("checkmark__circle-grey");
                animateCheck.classList.add("checkmark__check-grey");

            }
        }
    },

    showPriceBreakup: function (self) {
        var pqContainer = $(self).closest(".pq-container");
        var priceBreakupElem = $(pqContainer).find(".price-breakup");
        var onRoadPriceElement = $(pqContainer).find('.price-breakup__wrapper');

        priceQuoteCommon.onRoadPrice.showEffectiveOnRoadPrice(priceBreakupElem.find(".price-breakup__amount.onroad-price"), self, null, onRoadPriceElement);

        if (!$(priceBreakupElem).is(":visible")) {
            $(priceBreakupElem).show();
            $(pqContainer).find(".price-breakup-container").addClass("fixed-dealer");;
            $(".dealer-cta-container").removeClass("fixed-dealer");
            this.trackSummaryCardShow(pqContainer.attr("make-name"));
        }
        var cardNo = $(pqContainer).attr("card-no");
    },

    isScrolledIntoView: function (elem, percent) {
        var docViewTop = $(window).scrollTop(),
            docViewBottom = docViewTop + $(window).height(),
            elemTop = $(elem).offset().top + $(elem).find(".floating-container").height(),
            elemBottom = elemTop + $(elem).height();
        if (percent) {
            var value = (100 / percent);
            docViewBottom = docViewTop + $(window).height() / value;
        }
        return ((elemTop <= docViewBottom));
    },

    bindScroll: function () {
        var viewArr = [], headArr = [];
        for (var i = 0; i < $(".pq-container").length; i++) {
            var pqContainerElement = $(".pq-container")[i];
            viewArr.push(priceQuote.isScrolledIntoView($(pqContainerElement), 50));
            headArr.push(priceQuote.isScrolledIntoView($(pqContainerElement), 50));
        }
        var pqContainer = $(".pq-container")[viewArr.lastIndexOf(true)];
        var priceBreakupElem = $(pqContainer).find(".price-breakup");
        var priceBreakupContainerElem = $(pqContainer).find(".price-breakup-container");
        var dealerCTAElem = $(pqContainer).find(".dealer-cta");
        var dealerCTAContainerElem = $(pqContainer).find(".dealer-cta-container");


        if (!$(priceBreakupElem).is(":visible")) {
            if (dealerCTAElem.length) {
                var status = priceQuote.isScrolledIntoView($(dealerCTAElem));
                priceQuote.floatPriceBreakUp(status, dealerCTAContainerElem);
            }
        } else {
            $(priceBreakupContainerElem).removeClass("fixed-dealer");
            var status = priceQuote.isScrolledIntoView($(priceBreakupElem));
            priceQuote.floatPriceBreakUp(status, priceBreakupContainerElem);
        }

        if (lastIndex !== viewArr.lastIndexOf(true)) {
            lastIndex = viewArr.lastIndexOf(true);
            $(".price-breakup-container, .dealer-cta-container").removeClass("fixed-dealer");
        }


        if ($(window).scrollTop() >= $(".car-model-details").offset().top) {
            $(".floating-model-header").hide();
            var pqContainerHead = $(".pq-container")[headArr.lastIndexOf(true)];
            var cardNo = $(pqContainerHead).attr("card-no");
            $("." + cardNo + ".floating-model-header").show();
        } else {
            $(".floating-model-header").hide();
        }
    },

    floatPriceBreakUp: function (status, containerElem) {
        if (status) {
            $(containerElem).removeClass("fixed-dealer");
        } else {
            $(".price-breakup-container, .dealer-cta-container").removeClass("fixed-dealer");
            $(containerElem).addClass("fixed-dealer");
        }
    },

    appendState: function (state) {
        window.history.pushState(state, '', '');
    },

    removeState: function () {
        window.history.back();
    },

    openVersionPopUp: function (self) {
        var pageId = $(self).attr("Pageid");
        $(self).closest(".pq-container").find(".select-version-popup").show();
        $(".pqVersionList").attr("pageId", pageId);
        priceQuote.appendState("version-popup");
    },

    getVersionPriceQuote: function (modelId, versionId, cityId, areaId, isCrossSellPriceQuote, pageId, node, cardId, hideCampaign, cityName) {
        var url = priceQuoteCommon.getQuotationUrl(modelId, versionId, cityId, areaId, pageId, hideCampaign);
        if (isCrossSellPriceQuote) {
            priceQuote.getCrossSellPriceQuote(url, node, cardId);
            priceQuote.setCookies(cityId, cityName, versionId);

            if (typeof campaignId !== "undefined") {
                saveDealerCookie(campaignId, userCityId, userZoneId, CarDetails.carModelId);
            }
        } else {
            location.href = url;
        }
    },

    getCrossSellPriceQuote: function (url, node, cardId) {
        $.ajax({
            url: url + "&cs=true",
            type: 'GET',
            complete: function (response) {
                $(node).closest('.pq-container').replaceWith(response.responseText);
                priceQuote.registerEvents();
                priceQuote.showPriceQuoteForCard(cardId);
                priceQuoteCommon.resetCharge.resetChargeSelections(cardId);
                priceQuoteCommon.charge.addCompulsoryChargesToArray(cardId);

                CampaignTemplateBind.setAllCTALeadFormAttribute();
                var onRoadPriceElement = $(node).closest(".pq-container").find('.price-breakup__wrapper');
                priceQuoteCommon.onRoadPrice.showEffectiveOnRoadPrice(null, self, cardId, onRoadPriceElement);

                var data = EmiCalculator.createEmiStateObject($('.card2 .customize-emi-container'));
                var modelName = data[0].data.vehicleData.modelName;
                dataLayer.push({ event: 'CWNonInteractive', cat: "MSite_PQ_ModelChange", act: "PQ_ModelChange_shown", lab: modelName });
                EMI_PRICE_STORE.dispatch(EMI_PRICES_EVENTS.updateEmiModel(data));
                EmiCalculatorExtended.setEMIModelResult();
                OpenOffers.registerEvents();
                ShowToolTip.registerEvents();
                AnimateIcons.registerEvents();
            }
        });
    },

    changeCity: function (self) {
        var versionId = $(self).attr("data-attr-versionid");
        var cityId = $(self).attr("data-attr-cityid");
        var cityName = $(self).attr("data-attr-cityname");
        var areaId = $(self).attr("data-attr-areaid");
        var isCrossSellPriceQuote = $(self).attr("data-attr-iscrosssellpricequote") == "true";
        var pageId = $(self).attr("data-attr-pageid");
        var node = $(self);
        var cardId = $(self).attr("data-attr-cardno");
        var hideCampaign = $(self).attr("data-attr-hidecampaign");

        globalLocation.toChangeGlobalCity = false;
        new globalLocation.BL().openLocHint({ cityId: cityId, cityName: cityName }, globalLocation.expectedUserInput.MandatoryArea, function (payload) {
            var areaId = payload.areaId;
            if (typeof areaId === 'undefined') {
                areaId = 0;
            }
            priceQuote.getVersionPriceQuote(0, versionId, payload.cityId, areaId, isCrossSellPriceQuote, pageId, node, cardId, hideCampaign, payload.cityName);
            globalLocation.toChangeGlobalCity = true;
        }, null, false);
    },

    getPqUsingNearByCity: function (self) {
        var cityId = $(self).attr("data-attr-cityid");
        if ($.inArray(Number(cityId), askingAreaCityId) >= 0) {
            priceQuote.changeCity(self);
        }
        else {
            var versionId = $(self).attr("data-attr-versionid");
            var cityName = $(self).attr("data-attr-cityname");
            var isCrossSellPriceQuote = $(self).attr("data-attr-iscrosssellpricequote") == "true";
            var pageId = $(self).attr("data-attr-pageid");
            var node = $(self);
            var cardId = $(self).attr("data-attr-cardno");
            var hideCampaign = $(self).attr("data-attr-hidecampaign");
            priceQuote.getVersionPriceQuote(0, versionId, cityId, 0, isCrossSellPriceQuote, pageId, node, cardId, hideCampaign, cityName);
        }
    },

    getPqByVerionClick: function (modelId, versionId, cityId, areaId, isCrossSellPriceQuote, node, cardId, cityName) {
        var hideCampaign = false;
        var pageId = $(node).attr("pageid");
        priceQuote.getVersionPriceQuote(0, versionId, cityId, areaId, isCrossSellPriceQuote, pageId, node, cardId, hideCampaign, cityName);
    },
   
    charge: {

        reset: function (self) {
            var chargeObj = priceQuoteCommon.charge.formChargeObj(self);
            var onRoadPriceElement = $(self).closest(".pq-container").find('.price-breakup__wrapper');

            priceQuoteCommon.resetCharge.triggerResetLink(self);
            priceQuoteCommon.onRoadPrice.showEffectiveOnRoadPrice(null, self, chargeObj.cardNo, onRoadPriceElement);
        }
    },

    solidMetallicToggle: function (self, cardNo) {
        priceQuoteCommon.resetCharge.resetChargeSelections(cardNo);
        priceQuoteCommon.resetCharge.hideAllResetButtons(cardNo);
        priceQuoteCommon.charge.addCompulsoryChargesToArray(cardNo);

        var onRoadPriceElement = $(self).closest(".pq-container").find('.price-breakup__wrapper');
        priceQuoteCommon.onRoadPrice.showEffectiveOnRoadPrice(null, self, cardNo, onRoadPriceElement);
    },

    bindPriceBreakup: function (pqContainer) {
        if (pqContainer != null && pqContainer.length > 0 && pqContainer[0] != null) {
            var cardNo = pqContainer.attr("card-no");

            var selectedCharges = this.cards[cardNo];
            var charges = priceQuoteCommon.charge.getAllChargesToDisplay(selectedCharges);
            var tbody = this.createPriceBreakupHtml(cardNo, charges);

            $("." + cardNo + " .price-breakup-table").html(tbody);
        }
    },

    createPriceBreakupHtml: function (cardNo, charges) {
        var tbody = $('<tbody>');

        for (var i = 0; i < charges.length; i++) {
            var currentCharge = charges[i];

            var row = $('<tr>');
            var nameTd = $('<td>').text(currentCharge.chargeGroupName).addClass("float-price-breakup__description");
            if (currentCharge.charges != "") {
                nameTd.append($('<span>').addClass('float-price-breakup__items').text(" (" + currentCharge.charges + ")"));
            }

            var priceTd = $('<td>').text(formatNumeric(currentCharge.totalPrice)).addClass("float-price-breakup__amount");

            row.append(nameTd, priceTd);
            tbody.append(row);
        }

        var hrRow1 = $('<tr>');
        hrRow1 = hrRow1.append($('<td colspan="2">').addClass("hr-line"));

        var hrRow2 = $('<tr>');
        hrRow2 = hrRow2.append($('<td colspan="2">').addClass("hr-line"));

        var totalRow = $('<tr>').addClass("total-col");
        var totalNameTd = $('<td>').text("On Road Price").addClass("float-price-breakup__description");
        var totalPriceTd = $('<td>').html(priceQuoteCommon.rupeeUnicode + " " + formatNumeric(priceQuoteCommon.onRoadPrice.calculate(cardNo))).addClass("float-price-breakup__amount");

        totalRow = totalRow.append(totalNameTd, totalPriceTd);

        tbody = tbody.append(hrRow1);
        tbody = tbody.append(totalRow);
        tbody = tbody.append(hrRow2);

        return tbody;
    },

    createPriceBreakUpItem: function (chargeGroupName, charges, totalPrice) {
        return {
            chargeGroupName: chargeGroupName,
            charges: charges,
            totalPrice: totalPrice
        }
    },

    trackSummaryCardShow: function (makeName) {
        dataLayer.push({ event: 'CWInteractive', cat: 'MSite_PQ_Optional_floating_card', act: 'floating_card_shown', lab: makeName });
    },

    trackSummaryCardPullUpDown: function (action, makeName) {
        dataLayer.push({ event: 'CWInteractive', cat: 'MSite_PQ_Optional_summary_card_interaction', act: action, lab: makeName });
    },

    getCampaignType: function (campaignType) {
        switch (parseInt(campaignType)) {
            case priceQuote.campaignType.PQ:
                return "Pq";
            case priceQuote.campaignType.PAIDCROSSSELL:
                return "PaidCS";
            case priceQuote.campaignType.HOUSECROSSSELL:
                return "HouseCS";
        }
    },

    bindLeadSources: function (self) {
        var leadSourcePrefix = "NewPq-";
        var campaignTypeId = $(self).attr("campaign-ad-type");
        if (typeof campaignTypeId === "undefined" || campaignTypeId === null) { //template
            campaignTypeId = $(self).closest(".dealer-cta").attr("campaign-ad-type");
        }

        var source = priceQuote.getTemplateSource(self);
        var campaignType = priceQuote.getCampaignType(campaignTypeId);

        $.grep(leadSources, function (value, index) {
            if (value.adType == campaignTypeId && value.name === leadSourcePrefix + campaignType + source) {
                $(self).attr({ "sourceclick": value.leadClickSource, "leadinquirysource": value.inquirySource });
            }
            if (value.adType == campaignTypeId && value.name === leadSourcePrefix + campaignType + source + "Reco") {
                $(self).attr({ "recoleadsource": value.leadClickSource, "recoinquirysource": value.inquirySource });
            }
        });
    },

    getTemplateSource: function (self) {
        var source = $(self).attr("source"); //Link or Template

        if ($(self).hasClass("open-breakup")) {
            return source + "SummaryCard";
        }
        else if ($(self).hasClass("emi-calculator")) {
            return source + "EmiCalculator";
        }
        else {
            return source;
        }
    },

    setCookies: function (cityId, cityName, versionId) {
        document.cookie = '_CustCityId=' + cityId + '; expires = ' + permanentCookieTime() + "; domain=" + defaultCookieDomain + '; path =/';
        document.cookie = '_CustCity=' + cityName + '; expires = ' + permanentCookieTime() + "; domain=" + defaultCookieDomain + '; path =/';
        document.cookie = '_PQVersionId=' + versionId + '; expires = ' + permanentCookieTime() + "; domain=" + defaultCookieDomain + '; path =/';
    },

    getModelChangeCard: function (modelId, versionId, cityId, areaId) {
        Loader.showFullPageLoader();
        $.ajax({
            url: "/quotation/modelChange/?modelId=" + modelId + "&versionId=" + versionId + "&cityId=" + cityId + "&areaId=" + areaId + "&excludingModels=" + String(MODELLIST),
            type: 'GET',
            complete: function (response) {
                if (response != null && response.responseText != null) {
                    $("#modelChangeDiv").html(response.responseText);
                    priceQuote.initSwiper();
                    Loader.hideFullPageLoader();
                }
            },
            error: function () {
                Loader.hideFullPageLoader();
            }
        });
    },

    initSwiper: function () {
        $('#divModelSwiper.swiper-container:not(".noSwiper")').each(function (index, element) {
            var currentSwiper = $(this);
            currentSwiper.addClass('sw-' + index).swiper({
                slidesPerView: 'auto',
                spaceBetween: 10,
                preloadImages: false,
                lazyLoading: true,
                lazyLoadingInPrevNext: true
            });
        });
    }
}

$(document).ready(function () {
    priceQuote.registerEvents();
    priceQuote.showDefaultPriceQuoteOnLoad();
    priceQuoteCommon.cardArrays.initialize();
    priceQuote.setCookies(cityId, cityName, versionId);
    EmiCalculatorExtended.setEMIModelResult();
});

$("ul.news-body-UL li").click(function () {
    $("ul.news-body-list").slideUp();
    $('.view-more-btn').find("span").text("more");
});

var lastStatus,
    previousToolTip;

$(document).on("scroll", function () {
    priceQuote.bindScroll();
});

$(document).on("click", function () {
    if ($(".pq-tool-tip").is(":visible")) {
        $(".pq-tool-tip").hide();
    }
});

$(window).on('popstate', function (event) {
    if ($(".pq-container").hasClass("breakUpActive")) {
        priceQuote.animateDownPriceBreakup($(".close-float-price-breakup"));
    }
    if ($(".select-version-popup").is(':visible')) {
        $(".select-version-popup").hide();
    }
});

function customSort(property) {
    var sortOrder = 1;
    if (property[0] === "-") {
        sortOrder = -1;
        property = property.substr(1);
    }
    return function (a, b) {
        var resultPart = (Number(a[property]) > Number(b[property])) ? 1 : 0;
        var result = (Number(a[property]) < Number(b[property])) ? -1 : resultPart;
        return result * sortOrder;
    }
}

$(document).on("click", "#pq-popup-close-spn, #btnDone", function () {
    $('body').removeClass('iphone-optional');
});

$(document).on('click', ".open-modelchange-popup", function () {
    var data = $(this).data();
    priceQuote.getModelChangeCard(data.model, data.version, data.city, data.area);
});