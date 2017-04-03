var ga_pg_id = "16";

var dropdown;
var overallSpecsTabsContainer, modelSpecsTabsContentWrapper, modelSpecsFooter, topNavBarHeight;

function formatPrice(price) {
    if (price != null) {
        price = price.toString();
        var lastThree = price.substring(price.length - 3);
        var otherNumbers = price.substring(0, price.length - 3);
        if (otherNumbers != '')
            lastThree = ',' + lastThree;
        var price = otherNumbers.replace(/\B(?=(\d{2})+(?!\d))/g, ",") + lastThree;
    }

    return price;
}

docReady(function () {
    try {
        // activate first tab
        $('.overall-specs-tabs-wrapper li').first().addClass('active');

        // dropdown
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
                    menu = element.next('.dropdown-menu'),
                    menuTitle = element.attr('data-title');

                menu.append('<p class="dropdown-label">' + elementText + '</p><div class="dropdown-list-wrapper"><p class="dropdown-menu-title">' + menuTitle + '</p><ul class="dropdown-menu-list dropdown-with-select"></ul></div>');

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
                    dropdownLabel = menu.find('.dropdown-label');

                element.siblings('li').removeClass('active');
                element.addClass('active');
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

        var bikeVersions = [
            {
                "PriceQuoteId": 0,
                "ManufacturerName": null,
                "MaskingNumber": null,
                "ExShowroomPrice": 50000,
                "RTO": 1500,
                "Insurance": 2000,
                "OnRoadPrice": 53500,
                "MakeName": "Honda",
                "MakeMaskingName": "honda",
                "ModelName": "CB Shine",
                "ModelMaskingName": "shine",
                "VersionName": "2017",
                "CityId": 1,
                "CityMaskingName": "mumbai",
                "City": "Mumbai",
                "Area": null,
                "HasArea": false,
                "VersionId": 4514,
                "CampaignId": 0,
                "ManufacturerId": 0,
                "Varients": null,
                "OriginalImage": "",
                "HostUrl": "",
                "MakeId": 7,
                "IsModelNew": true,
                "IsVersionNew": true,
                "State": null,
                "ManufacturerAd": null,
                "LeadCapturePopupHeading": null,
                "LeadCapturePopupDescription": null,
                "LeadCapturePopupMessage": null,
                "PinCodeRequired": false
            },
            {
                "PriceQuoteId": 0,
                "ManufacturerName": null,
                "MaskingNumber": null,
                "ExShowroomPrice": 50615,
                "RTO": 5043,
                "Insurance": 1315,
                "OnRoadPrice": 56973,
                "MakeName": "Honda",
                "MakeMaskingName": "honda",
                "ModelName": "CB Shine",
                "ModelMaskingName": "shine",
                "VersionName": "Kick/Drum/Spokes",
                "CityId": 1,
                "CityMaskingName": "mumbai",
                "City": "Mumbai",
                "Area": null,
                "HasArea": false,
                "VersionId": 111,
                "CampaignId": 0,
                "ManufacturerId": 0,
                "Varients": null,
                "OriginalImage": "/bw/models/honda-cb-shine-kick/drum/spokes-111.jpg?20151209184344",
                "HostUrl": "https://imgd1.aeplcdn.com/",
                "MakeId": 7,
                "IsModelNew": true,
                "IsVersionNew": true,
                "State": null,
                "ManufacturerAd": null,
                "LeadCapturePopupHeading": null,
                "LeadCapturePopupDescription": null,
                "LeadCapturePopupMessage": null,
                "PinCodeRequired": false
            },
            {
                "PriceQuoteId": 0,
                "ManufacturerName": null,
                "MaskingNumber": null,
                "ExShowroomPrice": 57300,
                "RTO": 5511,
                "Insurance": 1773,
                "OnRoadPrice": 64584,
                "MakeName": "Honda",
                "MakeMaskingName": "honda",
                "ModelName": "CB Shine",
                "ModelMaskingName": "shine",
                "VersionName": "Electric Start/Drum/Alloy",
                "CityId": 1,
                "CityMaskingName": "mumbai",
                "City": "Mumbai",
                "Area": null,
                "HasArea": false,
                "VersionId": 112,
                "CampaignId": 0,
                "ManufacturerId": 0,
                "Varients": null,
                "OriginalImage": "/bw/models/honda-cb-shine-electric-start/drum/alloy-112.jpg?20151209184344",
                "HostUrl": "https://imgd1.aeplcdn.com/",
                "MakeId": 7,
                "IsModelNew": true,
                "IsVersionNew": true,
                "State": null,
                "ManufacturerAd": null,
                "LeadCapturePopupHeading": null,
                "LeadCapturePopupDescription": null,
                "LeadCapturePopupMessage": null,
                "PinCodeRequired": false
            }
        ];

        var versionTable = function () {
            var self = this;

            self.defaultVersion = bikeVersions[0];
            self.exshowroomPrice = ko.observable();
            self.rtoPrice = ko.observable();
            self.insurancePrice = ko.observable();
            self.onRoadPrice = ko.observable();

            self.setVersionDetails = function (version) {
                self.exshowroomPrice(formatPrice(version.ExShowroomPrice));
                self.rtoPrice(formatPrice(version.RTO));
                self.insurancePrice(formatPrice(version.Insurance));
                self.onRoadPrice(formatPrice(version.OnRoadPrice));
            };

            self.getVersionObject = function (verId) {
                verId = parseInt(verId);
                for (var i = 0; i < bikeVersions.length; i++) {
                    var versionObj = bikeVersions[i];
                    if (versionObj.VersionId == verId) {
                        self.setVersionDetails(versionObj);
                        break;
                    }
                }
            }

            self.setVersionDetails(self.defaultVersion);
        };

        var vmVersionTable = new versionTable();
        ko.applyBindings(vmVersionTable, $("#orpContent")[0]);

        $window = $(window),
        overallSpecsTabsContainer = $('.overall-specs-tabs-container'),
        modelSpecsTabsContentWrapper = $('#modelSpecsTabsContentWrapper'),
        modelSpecsFooter = $('#modelSpecsFooter'),
        topNavBarHeight = overallSpecsTabsContainer.height();

        var tabsLength = $('.overall-specs-tabs-wrapper li').length - 1;
        if (tabsLength < 2) {
            $('.overall-specs-tabs-wrapper li').css({ 'display': 'inline-block', 'width': 'auto' });
        }

        $(window).scroll(function () {
            var windowScrollTop = $window.scrollTop(),
                modelSpecsTabsOffsetTop = modelSpecsTabsContentWrapper.offset().top,
                modelSpecsFooterOffsetTop = modelSpecsFooter.offset().top;

            if (windowScrollTop > modelSpecsTabsOffsetTop) {
                overallSpecsTabsContainer.addClass('fixed-tab-nav');
            }

            else if (windowScrollTop < modelSpecsTabsOffsetTop) {
                overallSpecsTabsContainer.removeClass('fixed-tab-nav');
            }

            if (overallSpecsTabsContainer.hasClass('fixed-tab-nav')) {
                if (windowScrollTop > modelSpecsFooterOffsetTop - topNavBarHeight) {
                    overallSpecsTabsContainer.removeClass('fixed-tab-nav');
                }
            }

            $('#modelSpecsTabsContentWrapper .bw-model-tabs-data').each(function () {
                var top = $(this).offset().top - overallSpecsTabsContainer.height(),
                    bottom = top + $(this).outerHeight();
                if (windowScrollTop >= top && windowScrollTop <= bottom) {
                    overallSpecsTabsContainer.find('li').removeClass('active');
                    $('#modelSpecsTabsContentWrapper .bw-mode-tabs-data').removeClass('active');

                    $(this).addClass('active');

                    var currentActiveTab = overallSpecsTabsContainer.find('li[data-tabs="#' + $(this).attr('id') + '"]');
                    overallSpecsTabsContainer.find(currentActiveTab).addClass('active');
                }
            });

        });

        $('.overall-specs-tabs-wrapper li').click(function () {
            var target = $(this).attr('data-tabs');
            $('html, body').animate({ scrollTop: $(target).offset().top - overallSpecsTabsContainer.height() }, 1000);
            centerItVariableWidth($(this), '.overall-specs-tabs-container');
            return false;
        });

        // more cities
        $('.view-cities-link').on('click', function () {
            $('#more-cities-list').slideDown();
            $(this).closest('div').hide();
        });

        // dropdown events
        $('.dropdown-select-wrapper').on('click', '.dropdown-label', function () {
            dropdown.active($(this));
        });

        $('.dropdown-select-wrapper').on('click', '.dropdown-menu-list.dropdown-with-select li', function () {
            var element = $(this);
            if (!element.hasClass('active')) {
                dropdown.selectItem($(this));
                dropdown.selectOption($(this));
                dropdown.inactive();

                vmVersionTable.getVersionObject(element.attr('data-option-value'));
            }
        });

        $(document).on('click', function (event) {
            var dropdownSelect = $('.dropdown-select-wrapper');

            if (dropdownSelect.find('.dropdown-menu').hasClass('dropdown-active') && dropdownSelect.find('.dropdown-list-wrapper').is(':visible')) {
                if (!dropdownSelect.is(event.target) && dropdownSelect.has(event.target).length === 0) {
                    dropdown.inactive();
                }
            }
        });

        // emi calculator
        ko.bindingHandlers.slider = {
            init: function (element, valueAccessor, allBindingsAccessor, bindingContext) {
                var options = allBindingsAccessor().sliderOptions || {};
                $("#" + element.id).slider(options);
                ko.utils.registerEventHandler("#" + element.id, "slide", function (event, ui) {
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
                    return calculatedEMI;
                },
                owner: this
            });
        };

        $.calculateEMI = function (loanAmount, tenure, rateOfInterest, proFees) {
            var interest, totalRepay, finalEmi;
            try {
                interest = (loanAmount * tenure * rateOfInterest) / (12 * 100);
                totalRepay = loanAmount + interest + proFees;
                finalEmi = Math.ceil((totalRepay / tenure));
            }
            catch (e) {
            }
            return formatPrice(finalEmi);
        };

        $.LoanAmount = function (onRoadPrice, percentage) {
            var price;
            try {
                price = (onRoadPrice * percentage) / 100;
                price = Math.ceil(price / 100.0) * 100;
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

    } catch (e) {
        console.warn(e.message);
    }
});