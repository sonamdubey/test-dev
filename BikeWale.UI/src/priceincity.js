var bikeName;
var selectDropdownBox;
var bikeVersions = [];

var $window, overallSpecsTabsContainer, modelSpecsTabsContentWrapper, modelSpecsFooter;

docReady(function () {

    $('.overall-specs-tabs-wrapper a').first().addClass('active');

    // version dropdown
    $('.chosen-select').chosen();

    selectDropdownBox = $('.select-box-no-input');
    selectDropdownBox.each(function () {
        var text = $(this).find('.chosen-select').attr('data-title'),
            searchBox = $(this).find('.chosen-search')

        searchBox.empty().append('<p class="no-input-label">' + text + '</p>');
    });

    bikeVersions = JSON.parse(Base64.decode(encodedBikeVersions));
       
        //    "PriceQuoteId": 0,
        //    "ManufacturerName": null,
        //    "MaskingNumber": null,
        //    "ExShowroomPrice": 50615,
        //    "RTO": 5043,
        //    "Insurance": 1315,
        //    "OnRoadPrice": 56973,
        //    "MakeName": "Honda",
        //    "MakeMaskingName": "honda",
        //    "ModelName": "CB Shine",
        //    "ModelMaskingName": "shine",
        //    "VersionName": "Kick/Drum/Spokes",
        //    "CityId": 1,
        //    "CityMaskingName": "mumbai",
        //    "City": "Mumbai",
        //    "Area": null,
        //    "HasArea": false,
        //    "VersionId": 111,
        //    "CampaignId": 0,
        //    "ManufacturerId": 0,
        //    "Varients": null,
        //    "OriginalImage": "/bw/models/honda-cb-shine-kick/drum/spokes-111.jpg?20151209184344",
        //    "HostUrl": "https://imgd1.aeplcdn.com/",
        //    "MakeId": 7,
        //    "IsModelNew": true,
        //    "IsVersionNew": true,
        //    "State": null,
        //    "ManufacturerAd": null,
        //    "LeadCapturePopupHeading": null,
        //    "LeadCapturePopupDescription": null,
        //    "LeadCapturePopupMessage": null,
        //    "PinCodeRequired": false
        

    var versionTable = function () {
        var self = this;

        self.defaultVersion = bikeVersions[0];
        self.exshowroomPrice = ko.observable();
        self.rtoPrice = ko.observable();
        self.insurancePrice = ko.observable();
        self.onRoadPrice = ko.observable();
        self.selectedVersion = ko.observable(self.defaultVersion);
        self.bikeVersions = ko.observableArray(bikeVersions);
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

    $('#version-dropdown').chosen().change(function () {
        vmVersionTable.getVersionObject($(this).val());
    });

    $window = $(window),
    overallSpecsTabsContainer = $('.overall-specs-tabs-container'),
    modelSpecsTabsContentWrapper = $('#modelSpecsTabsContentWrapper'),
    modelSpecsFooter = $('#modelSpecsFooter');

    $(window).scroll(function () {
        var windowScrollTop = $window.scrollTop(),
            modelSpecsTabsOffsetTop = modelSpecsTabsContentWrapper.offset().top,
            modelSpecsFooterOffsetTop = modelSpecsFooter.offset().top;

        if (windowScrollTop > modelSpecsTabsOffsetTop) {
            overallSpecsTabsContainer.addClass('fixed-tab');
        }

        else if (windowScrollTop < modelSpecsTabsOffsetTop) {
            overallSpecsTabsContainer.removeClass('fixed-tab');
        }

        if (windowScrollTop > modelSpecsFooterOffsetTop - 44) { //44 height of top nav bar
            overallSpecsTabsContainer.removeClass('fixed-tab');
        }

        $('#modelSpecsTabsContentWrapper .bw-model-tabs-data').each(function () {
            var top = $(this).offset().top - overallSpecsTabsContainer.height(),
                bottom = top + $(this).outerHeight();
            if (windowScrollTop >= top && windowScrollTop <= bottom) {
                overallSpecsTabsContainer.find('a').removeClass('active');
                $('#modelSpecsTabsContentWrapper .bw-mode-tabs-data').removeClass('active');

                $(this).addClass('active');

                overallSpecsTabsContainer.find('a[href="#' + $(this).attr('id') + '"]').addClass('active');
            }
        });

    });

    $('.overall-specs-tabs-wrapper a[href^="#"]').click(function () {
        var target = $(this.hash);
        if (target.length == 0) target = $('a[name="' + this.hash.substr(1) + '"]');
        if (target.length == 0) target = $('html');
        $('html, body').animate({ scrollTop: target.offset().top - overallSpecsTabsContainer.height() }, 1000);
        return false;
    });

    // add divider between version prices table and prices in nearby cities
    addDivider($('#version-prices-grid'), $('#nearby-prices-grid'));

    var bikeVersionPrice = 93750;

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
        self.minDnPay = ko.observable(10 * bikeVersionPrice/100);
        self.maxDnPay = ko.observable(40 * bikeVersionPrice/100);
        self.minTenure = ko.observable(12);
        self.maxTenure = ko.observable(48);
        self.minROI = ko.observable(10);
        self.maxROI = ko.observable(15);

        self.processingFees = ko.observable(0);
        self.exshowroomprice = ko.observable(bikeVersionPrice);
        self.loan = ko.observable();

        self.tenure = ko.observable((self.maxTenure() - self.minTenure())/2 + self.minTenure());
        self.rateofinterest = ko.observable((self.maxROI() - self.minROI())/2 + self.minROI());
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
                return $.calculateEMI(self.loan(), self.tenure(), self.rateofinterest(),self.processingFees());
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


    $.calculateEMI = function (loanAmount, tenure, rateOfInterest,proFees) {
        var interest, totalRepay, finalEmi;
        try {
            interest = (loanAmount * tenure * rateOfInterest) / (12 * 100);
            totalRepay = loanAmount + interest + proFees;
            finalEmi = Math.ceil((totalRepay / tenure));
        }
        catch (e) {
        }
        return finalEmi;
    };

    $.createSliderPoints = function(index,min,max,breaks,sliderType)
    {   var svar = "";
        try {
            switch(sliderType)
            {
                case 1: 
                    svar =  $.valueFormatter(Math.round(min + (index * (max - min)/breaks)));
                    break;
                case 2:
                    svar =  Math.round(min + (index * (max - min)/breaks));
                    break;
                default:
                    svar =  (min + (index * (max - min)/breaks)).toFixed(2);
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
            price = Math.ceil(price / 100.0) * 100;
        }
        catch (e) {
        }
        return price;
    };

    $.valueFormatter = function (num) {
        if(isNaN(num))
        {
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

    var $dvPgVar = $("#dvPgVar");
    bikeName = $dvPgVar.data("bikename");
    var modelId = $dvPgVar.data("modelid");

    $("#btnDealerPricePopup").click(function () {
        var selArea = '';
        if ($('#ddlAreaPopup option:selected').index() > 0) {
            selArea = '_' + $('#ddlAreaPopup option:selected').text();
        }
        triggerGA('Price_in_City_Page', 'Show_On_Road_Price_Clicked', bikeName + "_" + $('#versions .active').text() + '_' + $('#ddlCitiesPopup option:selected').text() + selArea);

    });

    $("#dealerDetails").click(function (e) {
        try {
            ele = $(this);
            vmquotation.CheckCookies();
            vmquotation.IsLoading(true);
            $('#priceQuoteWidget,#popupContent,.blackOut-window').show();
            var options = {
                "modelId": modelId,
                "cityId": onCookieObj.PQCitySelectedId,
                "areaId": onCookieObj.PQAreaSelectedId,
                "city": (onCookieObj.PQCitySelectedId > 0) ? { 'id': onCookieObj.PQCitySelectedId, 'name': onCookieObj.PQCitySelectedName.replace(/-/g, ' ') } : null,
                "area": (onCookieObj.PQAreaSelectedId > 0) ? { 'id': onCookieObj.PQAreaSelectedId, 'name': onCookieObj.PQAreaSelectedName.replace(/-/g, ' ') } : null,
            };
            vmquotation.IsOnRoadPriceClicked(false);
            vmquotation.setOptions(options);
        } catch (e) {
            console.warn(e.message);
        }
    });

    $('.model-versions-tabs-wrapper a').on('click', function () {
        var verid = $(this).attr('id');
        showTab(verid);
    });

    $('.model-versions-tabs-wrapper a').first().trigger("click");

});

// add divider
function addDivider(grid1, grid2) {
    if (grid1.height() > grid2.height()) {
        grid1.addClass('border-solid-right');
    }
    else {
        grid2.addClass('border-solid-left');
    }
}

function showTab(version) {
    $('.model-versions-tabs-wrapper a').removeClass('active');
    $('.model-versions-tabs-wrapper a[id="' + version + '"]').addClass('active');
    $('.priceTable').hide();
    $('.priceTable[id="' + version + '"]').show();
}

function getBikeVersionName() {
    var bikeVersion = $('#versions .active').text();
    var bikeNameVersion = bikeName + '_' + bikeVersion;
    return bikeNameVersion;
}