var ga_pg_id = "16";

var dropdown;
var overallSpecsTabsContainer, modelSpecsTabsContentWrapper, modelSpecsFooter, topNavBarHeight;
var bikeVersionPrice, versionCount, bikeVersions;
var processingFees = 0;

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

    if (document.getElementById('dealerProcessingFees')) {
        processingFees = parseInt($('#dealerProcessingFees').val());
    }

    // ad blocker active than fallback method
    if (window.canRunAds === undefined) {
        callFallBackWriteReview();
    }

    function callFallBackWriteReview() {
        $('#adBlocker').show();
        $('.sponsored-card').hide();
    };


    try {
        // activate first tab
        $('.overall-specs-tabs-wrapper li').first().addClass('active');

		//floating button
		var floatingButton = document.querySelectorAll('.floating-btn')[0];

		if (floatingButton) {
			attachListener('scroll', window, toggleFloatingBtn);
		}

		function attachListener(event, element, functionName) {
			if (element.addEventListener) {
				element.addEventListener(event, functionName, false);
			}
			else if (element.attachEvent) {
				element.attachEvent('on' + event, functionName);
			}
		};
		
		var	docWindowHeight = $(window).height();

		function toggleFloatingBtn() {
			var bodyHeight = $('body').height(),
				footerHeight = $('footer').height(),
				scrollPosition = $(window).scrollTop();

			if (scrollPosition + docWindowHeight > (bodyHeight - footerHeight))
				$(floatingButton).hide();
			else
				$(floatingButton).show();
		}

        
		
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

        bikeVersions = JSON.parse(Base64.decode($("#dvVersionPrice").text())), isDiscontinued = $("#dvVersionPrice").data("isdiscontinued");
        bikeVersionPrice = JSON.parse(Base64.decode($("#versionPrice").text()));
        versionCount = JSON.parse(Base64.decode($("#versionCount").text()));
        var versionTable = function () {
            var self = this;

            self.defaultVersion = bikeVersions[0];
            self.exshowroomPrice = ko.observable();
            self.rtoPrice = ko.observable();
            self.insurancePrice = ko.observable();
            self.onRoadPrice = ko.observable();
            self.isDiscontinued = ko.observable(isDiscontinued.toLowerCase() == "true");
            self.setVersionDetails = function (version) {
                $("#priceincity").attr("data-versionid",version.VersionId);
                self.exshowroomPrice(formatPrice(version.ExShowroomPrice));
                self.rtoPrice(formatPrice(version.RTO));
                self.insurancePrice(formatPrice(version.Insurance));
                self.onRoadPrice(formatPrice(version.OnRoadPrice));
                $(".version-price").text(formatPrice(!self.isDiscontinued() ? version.OnRoadPrice : version.ExShowroomPrice));
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

        var tabElementThird = modelSpecsTabsContentWrapper.find('.bw-model-tabs-data:eq(3)'),
            tabElementSixth = modelSpecsTabsContentWrapper.find('.bw-model-tabs-data:eq(6)'),
            tabElementNinth = modelSpecsTabsContentWrapper.find('.bw-model-tabs-data:eq(9)');

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

            if (tabElementThird.length != 0) {
                focusFloatingTab(tabElementThird, 250, 0);
            }

            if (tabElementSixth.length != 0) {
                focusFloatingTab(tabElementSixth, 500, 250);
            }

            if (tabElementNinth.length != 0) {
                focusFloatingTab(tabElementNinth, 750, 500);
            }

            function focusFloatingTab(element, startPosition, endPosition) {
                if (windowScrollTop > element.offset().top - 45) {
                    if (!$('#overallSpecsTab').hasClass('scrolled-left-' + startPosition)) {
                        $('.overall-specs-tabs-container').addClass('scrolled-left-' + startPosition);
                        scrollHorizontal(startPosition);
                    }
                }

                else if (windowScrollTop < element.offset().top) {
                    if ($('#overallSpecsTab').hasClass('scrolled-left-' + startPosition)) {
                        $('.overall-specs-tabs-container').removeClass('scrolled-left-' + startPosition);
                        scrollHorizontal(endPosition);
                    }
                }
            };

        });

        $('.overall-specs-tabs-wrapper li').click(function () {
            var target = $(this).attr('data-tabs');
            $('html, body').animate({ scrollTop: $(target).offset().top - overallSpecsTabsContainer.height() }, 1000);
            centerItVariableWidth($(this), '.overall-specs-tabs-container');
            return false;
        });

        function scrollHorizontal(pos) {
            $('#overallSpecsTab').animate({ scrollLeft: pos - 15 + 'px' }, 500);
        }

        // more cities
        $('.view-cities-link').on('click', function () {
            $('#more-cities-list').show();
            $(this).closest('div').hide();
        });

        // version dropdown
        function handleVersionMenuClick(dropdown) {
            var offsetTop = $(dropdown.container).offset().top - $('.overall-specs-tabs-container').height();

            $('html, body').animate({ scrollTop: offsetTop }, 500);
        }

        function handleVersionChange(dropdown) {
            var optionValue = dropdown.activeOption.value;
            vmVersionTable.getVersionObject(optionValue);
        }

        var versionDropdown = new DropdownMenu('#versionDropdown', {
            onMenuClick: handleVersionMenuClick,
            onChange: handleVersionChange
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

        $('.dropdown-select-wrapper').on('change', '.dropdown-select',function () {
            try {
                var obj = $(this);
                if (obj.attr("data-lab") !== undefined) {
                    triggerGA(obj.attr("data-cat"), obj.attr("data-act"), obj.attr("data-lab"));
                }
                else if (obj.attr("data-var") !== undefined) {
                    triggerGA(obj.attr("data-cat"), obj.attr("data-act"), window[obj.attr("data-var")]);
                }
                else if (obj.attr("data-func") !== undefined) {
                    triggerGA(obj.attr("data-cat"), obj.attr("data-act"), eval(obj.attr("data-func") + '()'));
                }
            }
            catch (e) {
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
                    try {
                        var obj = $("#" + element.id);
                        if (obj.attr("data-lab") !== undefined) {
                            triggerGA(obj.attr("data-cat"), obj.attr("data-act"), obj.attr("data-lab"));
                        }
                        else if (obj.attr("data-var") !== undefined) {
                            triggerGA(obj.attr("data-cat"), obj.attr("data-act"), window[obj.attr("data-var")]);
                        }
                        else if (obj.attr("data-func") !== undefined) {
                            triggerGA(obj.attr("data-cat"), obj.attr("data-act"), eval(obj.attr("data-func") + '()'));
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
            
            self.processingFees = ko.observable(processingFees);
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
                    self.loan(($.LoanAmount(self.exshowroomprice(), 100)) - value);
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
            self.totalPayable = ko.pureComputed({
                read: function () {
                    return (self.downPayment() + (self.monthlyEMI() * self.tenure()) + self.processingFees());
                },
                owner: this
            });
        };

        $.calculateEMI = function (loanAmount, tenure, rateOfInterest, proFees) {
            var finalEmi;
            try {
                finalEmi = Math.round((loanAmount * rateOfInterest / 1200) / (1 - Math.pow((1 + rateOfInterest / 1200), (-1.0 * tenure))));
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

    } catch (e) {
        console.warn(e.message);
    }
    var $dvPgVar = $("#dvPgVar");
    bikeName = $dvPgVar.data("bikename");
  
    var cityName = $dvPgVar.data("cityarea");
    if (isCoverfoxShown) {
      triggerNonInteractiveGA('Price_in_City_Page', isNewCoverfoxShown ? 'PriceBreakUpAdShown' : 'BankbazaarLink_Shown', bikeName + '_' + cityName);
    }

    $(".leadcapturebtn").click(function (e) {
        ele = $(this);
        try {
            var leadOptions = {
                "dealerid": ele.attr('data-item-id'),
                "dealername": ele.attr('data-item-name'),
                "dealerarea": ele.attr('data-item-area'),
                "versionid": $("#priceincity").data("versionid"),
                "leadsourceid": ele.attr('data-leadsourceid'),
                "pqsourceid": ele.attr('data-pqsourceid'),
                "isleadpopup": ele.attr('data-isleadpopup'),
                "mfgCampid": ele.attr('data-mfgcampid'),
                "pqguid": $("#priceincity").data("pqguid") || 0,
                "pageurl": window.location.href,
                "clientip": '',
                "dealerHeading": ele.attr('data-item-heading'),
                "dealerMessage": ele.attr('data-item-message'),
                "dealerDescription": ele.attr('data-item-description'),
                "pinCodeRequired": ele.attr("data-ispincodrequired"),
                "emailRequired": ele.attr("data-isemailrequired"),
                "dealersRequired": ele.attr("data-dealersrequired"),
                "gaobject": {
                    cat: ele.attr("data-cat"),
                    act: ele.attr("data-act"),
                    lab: ele.attr("data-var")
                },
                "sendLeadSMSCustomer": ele.attr('data-issendleadsmscustomer'),
                "organizationName": ele.attr('data-item-organization'),
                "campaignId": ele.attr("data-campaignid")
            };
            dleadvm.setOptions(leadOptions);
        } catch (e) {
            console.warn("Unable to get submit details : " + e.message);
        }

    });
    AnimateCTA.registerEvents();
});