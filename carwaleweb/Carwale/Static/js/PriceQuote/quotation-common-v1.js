window.priceQuoteCommon = {
    getQuotationUrl: function (modelId, versionId, cityId, areaId, pageId, hideCampaign) {
        var url = "/quotation/?m=" + modelId + "&v=" + versionId + "&c=" + cityId + "&a=" + areaId + "&p=" + pageId;
        if (hideCampaign == true || hideCampaign == "true") {
            url = url + "&hc=" + hideCampaign;
        }
        return url;
    },

    openQuotationPage: function (self) {
        var element = $(self).closest(".js-recommendcar-carousel");
        var data = $(element).data();
        var url = priceQuoteCommon.getQuotationUrl(data.modelid, data.versionid, data.cityid, data.areaid, data.pqpageid, false);
        window.open(url);
    },

    selectionOption: { SINGLESELECT: 1, MULTISELECT: 2, ALLSELECT: 3 },
    chargeGroupType: { COMPULSORY: 1, OPTIONAL: 2 },
    
    rupeeUnicode: '&#8377;',

    charge: {
        insuranceHeader: "Insurance",
        formChargeObj: function (self) {
            var chargeObj = {
                chargeGroupId: self.getAttribute("chargegroup-id"),
                chargeGroupName: self.getAttribute("chargegroup-name"),
                chargeGroupSortOrder: self.getAttribute("chargegroup-sortorder"),
                chargeId: self.getAttribute("charge-id"),
                chargeName: self.getAttribute("charge-name"),
                chargeSortOrder: self.getAttribute("charge-sortorder"),
                selectionOption: self.getAttribute("selection-option"),
                price: self.getAttribute("price"),
                cardNo: self.getAttribute("card-no"),
                chargeType: null
            };
            return chargeObj;
        },
        distinctOptionalChargeGroupNames: "",

        addToArray: function (chargeObj) {
            priceQuote.cards[chargeObj.cardNo].push(chargeObj);
        },

        deleteFromArray: function (index, chargeObj) {
            priceQuote.cards[chargeObj.cardNo].splice(index, 1);
        },

        getIndex: function (chargeObj) {
            var chargeIndex = -1;
            $.each(priceQuote.cards[chargeObj.cardNo], function (index, value) {
                if (value.chargeGroupId === chargeObj.chargeGroupId && value.chargeId === chargeObj.chargeId) {
                    chargeIndex = index;
                    return false;
                }
            });
            return chargeIndex;
        },

        addCompulsoryChargesToArray: function (cardNo) {
            var compulsoryCharges = $('.compulsory-charges:visible[card-no=' + cardNo + ']');
            $.each(compulsoryCharges, function (index, value) {
                var chargeObj = priceQuoteCommon.charge.formChargeObj(value);
                chargeObj.chargeType = priceQuoteCommon.chargeGroupType.COMPULSORY;
                priceQuoteCommon.charge.addToArray(chargeObj);
            });
        },

        getAllChargesToDisplay: function (selectedCharges) {
            var finalChargesToDisplay = [];

            selectedCharges = selectedCharges.sort(priceQuoteCommon.utility.customSort("chargeGroupSortOrder"));
            var distinctChargeGroups = priceQuoteCommon.chargeGroup.getDistinctChargeGroupIds(selectedCharges);
            distinctOptionalChargeGroupNames = priceQuoteCommon.chargeGroup.getDistinctOptionalChargeGroupNames(selectedCharges);

            for (var i = 0; i < distinctChargeGroups.length; i++) {
                var currentCharges = priceQuoteCommon.chargeGroup.filterChargesOnChargeGroupId(selectedCharges, distinctChargeGroups[i]);

                var currentSelectionOption = Number(currentCharges[0].selectionOption);
                var currentChargeGroupId = Number(currentCharges[0].chargeGroupId);

                if (currentChargeGroupId === priceQuoteCommon.chargeGroup.types.COMPULSORYINSURANCE) {
                    var optionalInsuranceCharges = priceQuoteCommon.chargeGroup.filterChargesOnChargeGroupId(selectedCharges, priceQuoteCommon.chargeGroup.types.OPTIONALINSURANCE);
                    finalChargesToDisplay = finalChargesToDisplay.concat(this.getInsuranceCharges(currentCharges, optionalInsuranceCharges));
                }
                else if (currentChargeGroupId === priceQuoteCommon.chargeGroup.types.OPTIONALINSURANCE) {
                    continue;
                }
                else if (currentSelectionOption === priceQuoteCommon.selectionOption.SINGLESELECT) {
                    finalChargesToDisplay = finalChargesToDisplay.concat(this.getSingleSelectCharges(currentCharges));
                }
                else if (currentSelectionOption === priceQuoteCommon.selectionOption.MULTISELECT) {
                    finalChargesToDisplay = finalChargesToDisplay.concat(this.getMultipleSelectCharges(currentCharges));
                }
                else if (currentSelectionOption === priceQuoteCommon.selectionOption.ALLSELECT) {
                    finalChargesToDisplay = finalChargesToDisplay.concat(this.getAllSelectCharges(currentCharges));
                }
            }

            return finalChargesToDisplay;
        },

        getOptionalPackages: function () {
            var optionalPackages = "";
            chargeGroupCount = distinctOptionalChargeGroupNames.length;
            if (chargeGroupCount > 0) {
                optionalPackages += "Optional packages include ";
                for (var i = 0; i < 2 && i < chargeGroupCount; i++) {
                    if (i != 0) {
                        optionalPackages += ", ";
                    }
                    optionalPackages += distinctOptionalChargeGroupNames[i];
                }
                if (chargeGroupCount > 2) {
                    optionalPackages += ", etc.";
                }
            }
            return optionalPackages;
        },

        getSingleSelectCharges: function (currentCharges) {
            var charge = currentCharges[0];

            return [this.createPriceBreakUpItem(charge.chargeGroupName, charge.chargeName, charge.price)];
        },

        getMultipleSelectCharges: function (currentCharges) {
            var charges = "";
            var totalPrice = 0;

            currentCharges = currentCharges.sort(priceQuoteCommon.utility.customSort("chargeSortOrder"));

            for (var i = 0; i < currentCharges.length; i++) {
                var currentCharge = currentCharges[i];
                charges = charges + currentCharge.chargeName + ", ";
                totalPrice = totalPrice + Number(currentCharge.price);
            }

            return [this.createPriceBreakUpItem(currentCharges[0].chargeGroupName, charges.slice(0, -2), totalPrice)];
        },

        getAllSelectCharges: function (currentCharges) {
            var priceBreakUpItems = [];

            currentCharges = currentCharges.sort(priceQuoteCommon.utility.customSort("chargeSortOrder"));
            for (var i = 0; i < currentCharges.length; i++) {
                var charge = currentCharges[i];
                priceBreakUpItems.push(this.createPriceBreakUpItem(charge.chargeName, "", charge.price));
            }

            return priceBreakUpItems;
        },

        getInsuranceCharges: function (currentCharges, optionalInsuranceCharges) {
            var charge = currentCharges[0];
            var optionalInsuranceCharge = null;

            if (optionalInsuranceCharges != null && optionalInsuranceCharges.length > 0) {
                optionalInsuranceCharge = optionalInsuranceCharges[0];
                return [this.createPriceBreakUpItem(this.insuranceHeader, optionalInsuranceCharge.chargeName, Number(charge.price) + Number(optionalInsuranceCharge.price))];
            }
            else {
                return [this.createPriceBreakUpItem(charge.chargeName, "", charge.price)];
            }
        },

        createPriceBreakUpItem: function (chargeGroupName, charges, totalPrice) {
            return {
                chargeGroupName: chargeGroupName,
                charges: charges,
                totalPrice: totalPrice
            }
        },
    },

    chargeGroup: {

        types: { OPTIONALINSURANCE: 9, COMPULSORYINSURANCE: 17 },

        showTotal: function (self) {
            var chargeObj = priceQuoteCommon.charge.formChargeObj(self);
            var totalPrice = priceQuoteCommon.chargeGroup.getTotal(chargeObj);
            priceQuoteCommon.chargeGroup.showHideTotal(totalPrice, self.getAttribute("parent-id"));
        },

        deleteChargeOfSameGroup: function (chargeObj) {
            for (var index = priceQuote.cards[chargeObj.cardNo].length - 1; index >= 0; index--) {
                if (priceQuote.cards[chargeObj.cardNo][index].chargeGroupId === chargeObj.chargeGroupId) {
                    priceQuoteCommon.charge.deleteFromArray(index, chargeObj);
                }
            }
        },

        calculateTotal: function (chargeObj) {
            var total = 0;
            $.grep(priceQuote.cards[chargeObj.cardNo], function (element, index) {
                if (typeof element !== "undefined" && element.chargeGroupId === chargeObj.chargeGroupId) {
                    total = total + parseInt(element.price);
                }
            });
            return total;
        },

        calculateSingleSelectTotal: function (chargeObj) {
            priceQuoteCommon.chargeGroup.deleteChargeOfSameGroup(chargeObj);
            priceQuoteCommon.charge.addToArray(chargeObj);
            chargeObj.chargeType = priceQuoteCommon.chargeGroupType.OPTIONAL;
            return priceQuoteCommon.chargeGroup.calculateTotal(chargeObj);
        },

        calculateMultiSelectTotal: function (chargeObj) {
            var chargeIndex = priceQuoteCommon.charge.getIndex(chargeObj);

            if (chargeIndex >= 0) {
                priceQuoteCommon.charge.deleteFromArray(chargeIndex, chargeObj);
            } else {
                chargeObj.chargeType = priceQuoteCommon.chargeGroupType.OPTIONAL;
                priceQuoteCommon.charge.addToArray(chargeObj);
            }

            return priceQuoteCommon.chargeGroup.calculateTotal(chargeObj);
        },

        getDistinctChargeGroupIds: function (selectedCharges) {
            var distinctChargeGroupIds = [];

            for (var i = 0; i < selectedCharges.length; i++) {
                var currentCharge = selectedCharges[i];
                if (distinctChargeGroupIds.indexOf(currentCharge.chargeGroupId) < 0) {
                    distinctChargeGroupIds.push(currentCharge.chargeGroupId);
                }
            }

            return distinctChargeGroupIds;
        },

        getDistinctOptionalChargeGroupNames: function (selectedCharges) {
            var distinctChargeGroupNames = [];
            for (var i = 0; i < selectedCharges.length; i++) {
                var currentCharge = selectedCharges[i];
                if (distinctChargeGroupNames.indexOf(currentCharge.chargeGroupName) < 0 && currentCharge.chargeType == priceQuoteCommon.chargeGroupType.OPTIONAL) {
                    distinctChargeGroupNames.push(currentCharge.chargeGroupName);
                }
            }
            return distinctChargeGroupNames;
        },


        filterChargesOnChargeGroupId: function (selectedCharges, chargeGroupId) {
            return $.grep(selectedCharges, function (obj) {
                return Number(obj.chargeGroupId) === Number(chargeGroupId);
            });
        },

        getTotal: function (chargeObj) {
            switch (parseInt(chargeObj.selectionOption)) {
                case priceQuoteCommon.selectionOption.SINGLESELECT:
                    return priceQuoteCommon.chargeGroup.calculateSingleSelectTotal(chargeObj);
                case priceQuoteCommon.selectionOption.MULTISELECT:
                    return priceQuoteCommon.chargeGroup.calculateMultiSelectTotal(chargeObj);
                default: return 0;
            }
        },

        showHideTotal: function (price, selector) {
            var parentElement = $("#" + selector);
            var oemTitleContainer = $(parentElement).parent();
            var resetPackageLink = $(oemTitleContainer).find('.reset-package');

            if (price > 0) {
                parentElement.find(".charge_total").html(Common.utils.formatNumeric(price));
                parentElement.not(":visible").removeClass("hide");
                resetPackageLink.not(":visible").removeClass("hide");
            } else {
                parentElement.addClass("hide");
                resetPackageLink.addClass("hide");
            }
        },
    },

    resetCharge: {

        resetAllSelection: function (resetElement) {
            if (typeof event !== "undefined") {
                event.stopPropagation();
            }
            var ulInputRadio = resetElement.find('input[type="radio"]'),
                ulInputCkbx = resetElement.find('input[type="checkbox"]');

            $(ulInputRadio).prop('checked', false);
            $(ulInputCkbx).prop('checked', false);
        },

        hideAllResetButtons: function (cardNo) {
            $("." + cardNo).find('.reset-package').addClass("hide");
        },

        resetChargeSelections: function (cardNo) {
            $("." + cardNo).find(".chargegroup_total").addClass("hide");
            priceQuote.cards[cardNo] = [];
        },

        triggerResetLink: function (self) {
            if (typeof event !== "undefined") {
                event.preventDefault();
            }
            var chargeObj = priceQuoteCommon.charge.formChargeObj(self);
            var resetElement = $(self).closest('.js-oem-package-item');

            priceQuoteCommon.chargeGroup.deleteChargeOfSameGroup(chargeObj);
            priceQuoteCommon.chargeGroup.showHideTotal(0, self.getAttribute("parent-id"));
            priceQuoteCommon.resetCharge.resetAllSelection(resetElement);
        }
    },

    cardArrays: {

        declare: function () {
            for (var i = 1; i <= $(".pq-container").length; i++) {
                priceQuote.cards["card" + i] = [];
            }
        },

        initialize: function () {
            priceQuoteCommon.cardArrays.declare();

            $.each(priceQuote.cards, function (index) {
                priceQuoteCommon.charge.addCompulsoryChargesToArray(index);
            });
        },
    },

    solidMetallicToggle: function (self, cardNo) {
        priceQuoteCommon.resetCharge.resetChargeSelections(cardNo);
        priceQuoteCommon.resetCharge.hideAllResetButtons(cardNo);

        var resetElement = $("." + cardNo + '.js-oem-package-item');
        priceQuoteCommon.resetCharge.resetAllSelection(resetElement);
        priceQuoteCommon.charge.addCompulsoryChargesToArray(cardNo);
    },

    utility: {

        customSort: function (property) {
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
    },

    onRoadPrice: {

        calculate: function (cardNo) {
            var charges = priceQuote.cards[cardNo];
            var onRoadPrice = 0;

            $.each(charges, function (index, value) {
                onRoadPrice = onRoadPrice + parseInt(value.price);
            });

            return onRoadPrice;
        },

        updateFinalPrice: function (self, onRoadPrice) {
            var formatedValue = Common.utils.formatNumeric(onRoadPrice);
            priceQuoteCommon.animation.highlightUpdatedPrice(self);
            return formatedValue;
        },

        showEffectiveOnRoadPrice: function (viewBreakUpAmountElement, self, cardNo, onRoadPriceElement, onRoadPrice) {
            if (typeof cardNo === "undefined" || cardNo === null) {
                cardNo = $(self).attr("card-no");
            }

            if (viewBreakUpAmountElement === null) {
                viewBreakUpAmountElement = $(".price-breakup__amount.onroad-price[card-no=" + cardNo + "]");
            }

            if (typeof onRoadPrice === "undefined") {
                onRoadPrice = this.calculate(cardNo);
            }

            if (onRoadPrice > 0) {
                viewBreakUpAmountElement.html(this.updateFinalPrice(onRoadPriceElement, onRoadPrice));
                viewBreakUpAmountElement.not(":visible").show();
            } else {
                viewBreakUpAmountElement.hide();
            }
        }
    },

    animation: {

        createRipple: function (event) {
            var targetElement = event.currentTarget;

            // create .ink element if it doesn't exist
            if (!targetElement.querySelectorAll(".ink").length) {
                var span = document.createElement("span")

                span.classList.add("ink")
                targetElement.appendChild(span)
            }

            var ink = targetElement.querySelectorAll(".ink")[0]

            // incase of quick double clicks stop the previous animation
            ink.classList.remove("animate")

            // set size of .ink
            if (!ink.offsetHeight && !ink.offsetWidth) {
                // use parent's width or height whichever is larger for the diameter to make a circle which can cover the entire element.
                var dimension = Math.max(targetElement.offsetHeight, targetElement.offsetWidth)

                ink.style.width = dimension + 'px'
                ink.style.height = dimension + 'px'
            }

            var rect = targetElement.getBoundingClientRect()

            var offset = {
                top: rect.top + (window.pageYOffset || document.documentElement.scrollTop),
                left: rect.left + (window.pageXOffset || document.documentElement.scrollLeft)
            }

            // get click coordinates
            // logic = click coordinates relative to page - parent's position relative to page - half of self height/width to make it controllable from the center;
            var clickPositionX = event.pageX - offset.left - ink.offsetWidth / 2
            var clickPositionY = event.pageY - offset.top - ink.offsetHeight / 2

            ink.style.top = clickPositionY + 'px'
            ink.style.left = clickPositionX + 'px'
            ink.classList.add("animate")

            // remove .ink element on animation end
            var detachRipple = function () {
                targetElement.removeEventListener("animationend", detachRipple)
                if (targetElement.querySelector('.ink'))
                    targetElement && targetElement.removeChild(ink)
            }

            targetElement.addEventListener("animationend", detachRipple)
            //targetElement && targetElement.removeChild(ink)
        },

        highlightUpdatedPrice: function (self) {
            var $activePriceBreakUp = self;
            $activePriceBreakUp.addClass('highlight-price');
            setTimeout(function () {
                $activePriceBreakUp.removeClass('highlight-price');
            }, 1000);
        }
    }
}