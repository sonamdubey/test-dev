﻿var selectDropdownBox, msg = "";
var getCityArea = GetGlobalCityArea();
var getOfferClick = false, getMoreDetailsClick = false, getEMIClick = false;

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
            "emailRequired": ele.attr("data-isemailrequired"),
            "dealersRequired": ele.attr("data-dealersRequired"),
            "eventcategory":ele.attr("c"),
            "gaobject": {
                cat: ele.attr("c"),
                act: ele.attr("a"),
                lab: bikeVerLocation
            }          
        };
        if (leadOptions.dealersRequired) {
            generateDealerDropdown();
        }
        dleadvm.setOptions(leadOptions);
    });
    
    function generateDealerDropdown() {
        $.ajax({
            type: "GET",
            url: "/api/ManufacturerCampaign/?city="+ cityId,
            contentType: "application/json",
            dataType: 'json',
            success: function (response) {
                var obj = ko.toJS(response);
                var count = obj.length;
                if (count >= 1) {
                    if (count == 1) {
                        $("#ddlMfgDealers").append("<option value='0' data-id='" + obj[0].id + "' >" + obj[0].dealerName + "</option>");
                        $("#ddlMfgDealers").val('0');
                        $("#ddlMfgDealers").closest('.select-box').addClass('done');
                        dleadvm.dealersRequired(false);
                    } else {
                        $("#ddlMfgDealers").html('');
                        $("#ddlMfgDealers").append("<option value></option>");
                        for (i = 0; i < count; i++) {
                            var dt = obj[i];
                            var areaName = '';
                            if (dt.area != null) {
                                areaName = ", " + dt.area;
                            }
                            $("#ddlMfgDealers").append("<option value=" + (i + 1) + " data-id='" + dt.id + "' >" + dt.dealerName + areaName + "</option>");
                        }
                    }
                }
                $("#ddlMfgDealers").trigger("chosen:updated");
            },
        });
    };

    $('.chosen-select').on('change', function () {
        var selectField = $(this);
        if (selectField.val() > 0) {
            selectField.closest('.select-box').addClass('done');
            validate.setError($(this), '');
        }
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

    // version dropdown
    selectDropdownBox = $('.select-box-no-input');

    selectDropdownBox.each(function () {
        var text = $(this).find('.chosen-select').attr('data-title'),
            searchBox = $(this).find('.chosen-search')

        searchBox.empty().append('<p class="no-input-label">' + text + '</p>');
    });
    // emi calculator
    ko.bindingHandlers.slider = {
        init: function (element, valueAccessor, allBindingsAccessor, bindingContext) {
            var options = allBindingsAccessor().sliderOptions || {};
            $("#" + element.id).slider(options);
            ko.utils.registerEventHandler("#" + element.id, "slide", function (event, ui) {
                try {
                    var obj = $("#" + element.id);
                    if (obj.attr('l') !== undefined) {
                        triggerGA(obj.attr("c"), obj.attr("a"), obj.attr("l"));
                    }
                    else if (obj.attr('v') !== undefined) {
                        triggerGA(obj.attr("c"), obj.attr("a"), window[obj.attr("v")]);
                    }
                    else if (obj.attr('f') !== undefined) {
                        triggerGA(obj.attr("c"), obj.attr("a"), eval(obj.attr("f") + '()'));
                    }
                }
                catch (e) {
                    console.warn(e);
                }
                var observable = valueAccessor();
                observable(ui.value);
            });
        },
        update: function (element, valueAccessor, allBindingsAccessor, bindingContext) {
            var options = allBindingsAccessor().sliderOptions || {};
            $("#" + element.id).slider(options);
            var value = ko.utils.unwrapObservable(valueAccessor());
            if (isNaN(value)) value = 0;
            $("#" + element.id).slider("value", value);
        }
    };

    var BikeEMI = function () {
        var self = this;
        self.breakPoints = ko.observable(5);
        self.bikePrice = ko.observable(bikeVersionPrice);
        self.minDnPay = ko.observable(10 * bikeVersionPrice / 100);
        self.maxDnPay = ko.observable(40 * bikeVersionPrice / 100);
        self.minTenure = ko.observable(12);
        self.maxTenure = ko.observable(48);
        self.minROI = ko.observable(10);
        self.maxROI = ko.observable(15);

        self.processingFees = ko.observable(0);
        self.exshowroomprice = ko.observable(bikeVersionPrice);
        self.loan = ko.observable();

        self.tenure = ko.observable((self.maxTenure() - self.minTenure()) / 2 + self.minTenure());
        self.rateofinterest = ko.observable((self.maxROI() - self.minROI()) / 2 + self.minROI());
        self.downPayment = ko.pureComputed({
            read: function () {
                if (self.loan() == undefined || isNaN(self.loan()) || self.loan() == null)
                    self.loan($.LoanAmount(self.exshowroomprice(), 70));
                return (($.LoanAmount(self.exshowroomprice(), 100)) - self.loan());
            },
            write: function (value) {
                self.loan((($.LoanAmount(self.exshowroomprice(), 100))) - value);
            },
            owner: this
        });

        self.monthlyEMI = ko.pureComputed({
            read: function () {
                return $.calculateEMI(self.loan(), self.tenure(), self.rateofinterest(), self.processingFees());
            },
            owner: this
        });

        self.totalPayable = ko.pureComputed({
            read: function () {
                return (self.downPayment() + (self.monthlyEMI() * self.tenure()));
            },
            owner: this
        });
    };


    $.calculateEMI = function (loanAmount, tenure, rateOfInterest, proFees) {
        var interest, totalRepay, finalEmi;
        try {
            interest = (loanAmount * tenure * rateOfInterest) / (12 * 100);
            totalRepay = loanAmount + interest + proFees;
            finalEmi = Math.round((totalRepay / tenure));
        }
        catch (e) {
        }
        return finalEmi;
    };

    $.createSliderPoints = function (index, min, max, breaks, sliderType) {
        var svar = "";
        try {
            switch (sliderType) {
                case 1:
                    svar = $.valueFormatter(Math.round(min + (index * (max - min) / breaks)));
                    break;
                case 2:
                    svar = Math.round(min + (index * (max - min) / breaks));
                    break;
                default:
                    svar = (min + (index * (max - min) / breaks)).toFixed(2);
                    break;
            }
        } catch (e) {

        }
        return svar;
    };

    $.LoanAmount = function (onRoadPrice, percentage) {
        var price;
        try {
            price = (onRoadPrice * percentage) / 100;
            price = Math.round(price);
        }
        catch (e) {
        }
        return price;
    };

    $.valueFormatter = function (num) {
        if (isNaN(num)) {
            if (num >= 100000) {
                return (num / 100000).toFixed(1).replace(/\.0$/, '') + 'L';
            }
            if (num >= 1000) {
                return (num / 1000).toFixed(1).replace(/\.0$/, '') + 'K';
            }
        }

        return num;
    };

    var EMIviewModel = new BikeEMI;
    ko.applyBindings(EMIviewModel, $("#emiContent")[0]);

});
