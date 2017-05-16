﻿var swiper, dropdown, emiPopupDiv, offersPopupDiv;
function registerPQAndReload(eledealerId,eleversionId)
{
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
            window.location.href = "/m/pricequote/dealer/?MPQ=" + Base64.encode(rediurl);
        }
    } catch (e) {
        console.warn("Unable to create pricequote : " + e.message);
    }
}
function secondarydealer_Click(dealerID) {
    triggerGA('Dealer_PQ', 'Secondary_Dealer_Card_Clicked', bikeVerLocation);
    registerPQAndReload(dealerID);
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

var offersPopupOpen = function (offersPopupDiv) {
    offersPopupDiv.show();
};

var offersPopupClose = function (offersPopupDiv) {
    offersPopupDiv.hide();
};

var emiPopupOpen = function (emiPopupDiv) {
    emiPopupDiv.show();
};

var emiPopupClose = function (emiPopupDiv) {
    emiPopupDiv.hide();
    $('body, html').removeClass('lock-browser-scroll');
};


docReady(function () {

    emiPopupDiv = $("#emiPopup");
    offersPopupDiv = $("#offersPopup");
    $('#bw-header').addClass('fixed');

    var $window = $(window),
        buttonWrapper = $('#pricequote-floating-button-wrapper'),
        floatingButton = buttonWrapper.find('.float-button'),
        windowHeight,
        body = $('body');

    swiper = new Swiper('.pq-secondary-dealer-swiper', {
        slidesPerView: 'auto',
        spaceBetween: 0
    });

    dropdown = {
        setDropdown: function () {
            var selectDropdown = $('.dropdown-select');

            selectDropdown.each(function () {
                dropdown.setMenu($(this));
            });
        },

        setMenu: function (element) {
            $('<div class="dropdown-menu"></div>').insertAfter(element);
            dropdown.setStructure(element);
        },

        setStructure: function (element) {
            var elementText = element.find('option:selected').text(),
                menu = element.next('.dropdown-menu');

            menu.append('<p class="dropdown-label">' + elementText + '</p><div class="dropdown-list-wrapper"><p class="dropdown-selected-item">' + elementText + '</p><ul class="dropdown-menu-list dropdown-with-select"></ul></div>');

            dropdown.setOption(element);
        },

        setOption: function (element) {
            var selectedIndex = element.find('option:selected').index(),
                menu = element.next('.dropdown-menu'),
                menuList = menu.find('ul'),
                i;

            element.find('option').each(function (index) {
                if (selectedIndex == index) {
                    menuList.append('<li class="active" data-option-value="' + $(this).val() + '">' + $(this).text() + '</li>');
                }
                else {
                    menuList.append('<li data-option-value="' + $(this).val() + '">' + $(this).text() + '</li>');
                }
            });
        },

        active: function (label) {
            $('.dropdown-select-wrapper').find('.dropdown-menu').removeClass('dropdown-active');
            label.closest('.dropdown-menu').addClass('dropdown-active');
        },

        inactive: function () {
            $('.dropdown-select-wrapper').find('.dropdown-menu').removeClass('dropdown-active');
        },

        selectItem: function (element) {
            var elementText = element.text(),
                menu = element.closest('.dropdown-menu'),
                dropdownLabel = menu.find('.dropdown-label'),
                selectedItem = menu.find('.dropdown-selected-item');

            element.siblings('li').removeClass('active');
            element.addClass('active');
            selectedItem.text(elementText);
            dropdownLabel.text(elementText);
        },

        selectOption: function (element) {
            var elementValue = element.attr('data-option-value'),
                wrapper = element.closest('.dropdown-select-wrapper'),
                selectDropdown = wrapper.find('.dropdown-select');

            selectDropdown.val(elementValue).trigger('change');

        }
    };

    dropdown.setDropdown();


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

    $(".leadcapturebtn").click(function (e) {
        ele = $(this);
        try {
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
                "dealersRequired": ele.attr("data-dealersrequired"),
                "gaobject": {
                    cat: ele.attr("c"),
                    act: ele.attr("a"),
                    lab: ele.attr("v")
                }
            };
            if (leadOptions.dealersRequired) {
                generateDealerDropdown();
            }
            dleadvm.setOptions(leadOptions);
        } catch (e) {
            console.warn("Unable to get submit details : " + e.message);
        }

    });

    function generateDealerDropdown() {
        $.ajax({
            type: "GET",
            url: "/api/ManufacturerCampaign/?city=" + cityId,
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
                        $("#ddlMfgDealers").append("<option value selected>Select dealer</option>");
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
                $("#getDealer-select-box").find(".dropdown-menu").remove();
                dropdown.setMenu($("#ddlMfgDealers"));
            },
        });
    };


    $("#ddlVersion").on("change", function () {
        versionName = $(this).children(":selected").text();
        registerPQAndReload(dealerId,$(this).val());
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

    $(".view-offers-target").on("click", function () {
        offersPopupOpen(offersPopupDiv);
        appendHash("offersPopup");
    });

    $('.tnc').on('click', function (e) {
        LoadTerms($(this).attr("id"));
    });
    $(".calculate-emi-target").on("click", function () {
        emiPopupOpen(emiPopupDiv);
        appendHash("emiPopup");
        $('body, html').addClass('lock-browser-scroll');
    });

    $(".emi-popup-close-btn").on("click", function () {
        emiPopupClose(emiPopupDiv);
        window.history.back();
    });


    $('.btn-grey-state').on('click', function () {
        $(this).addClass('button-clicked-state');
        setTimeout(function () { $('.btn-grey-state').removeClass('button-clicked-state'); }, 100);
    });

    $('#getMoreDetails').on('click', function () {
        getMoreDetailsClicked = true;
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

    // tooltip
    $('.bw-tooltip').on('click', '.close-bw-tooltip', function () {
        var tooltipParent = $(this).closest('.bw-tooltip');

        tooltipParent.slideUp();
    });

    $('.tnc').on('click', function (e) {
        LoadTerms($(this).attr("id"));
    });

    $(".termsPopUpCloseBtn").on('mouseup click', function (e) {
        $("div#termsPopUpContainer").hide();
        $(".blackOut-window").hide();
    });

    $('#getDealerDetails,#btnBookBike').click(function () {
        var cookieValue = "CityId=" + cityId + "&AreaId=" + areaId + "&PQId=" + pqId + "&VersionId=" + versionId + "&DealerId=" + dealerId;
        window.location.href = '/m/pricequote/bookingsummary_new.aspx?MPQ=' + Base64.encode(cookieValue);
    });

    // GA Tags
    $("#leadBtnBookNow").on("click", function () {
        leadSourceId = $(this).attr("leadSourceId");
        dataLayer.push({ "event": "Bikewale_all", "cat": "Dealer_PQ", "act": "Get_More_Details_Clicked_Button", "lab": bikeName + "_" + getCityArea });
    });
    $("#leadLink").on("click", function () {
        dataLayer.push({ "event": "Bikewale_all", "cat": "Dealer_PQ", "act": "Get_More_Details_Clicked_Link", "lab": bikeName + "_" + getCityArea });
    });
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
                var calculatedEMI = $.calculateEMI(self.loan(), self.tenure(), self.rateofinterest(), self.processingFees());
                if ($("#spnEMIAmount") && $("#spnEMIAmount").length > 0 && calculatedEMI != "0") $("#spnEMIAmount").text(formatPrice(calculatedEMI)); else { $("#spnEMIAmount").parent().addClass("hide"); }
                return calculatedEMI;
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