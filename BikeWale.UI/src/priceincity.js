var bikeName, isDiscontinued, versionCount, bikeVersionPrice;
var selectDropdownBox;
var bikeVersions = [];
var processingFees = 0;

var $window, overallSpecsTabsContainer, modelSpecsTabsContentWrapper, modelSpecsFooter;

docReady(function () {

    if (document.getElementById('dealerProcessingFees')) {
        processingFees = parseInt($('#dealerProcessingFees').val());
    }

    $('.overall-specs-tabs-wrapper span').first().addClass('active');
   
    // ad blocker active than fallback method
    if (window.canRunAds === undefined) {
        callFallBackWriteReview();
    }
    function callFallBackWriteReview() {
        $('#adBlocker').show();
        $('.sponsored-card').hide();
    };

    
    // version dropdown
    $('.chosen-select').chosen();

    selectDropdownBox = $('.select-box-no-input');
    selectDropdownBox.each(function () {
        var text = $(this).find('.chosen-select').attr('data-title'),
            searchBox = $(this).find('.chosen-search')

        searchBox.empty().append('<p class="no-input-label">' + text + '</p>');
    });

    bikeVersions = JSON.parse(Base64.decode($("#dvVersionPrice").text()));
    isDiscontinued = $("#dvVersionPrice").data("isdiscontinued");
    bikeVersionPrice = JSON.parse(Base64.decode($("#versionPrice").text()));
    versionCount = JSON.parse(Base64.decode($("#versionCount").text()));

    var versionTable = function () {
        var self = this;

        self.defaultVersion = bikeVersions[0];
        self.exshowroomPrice = ko.observable();
        self.rtoPrice = ko.observable();
        self.insurancePrice = ko.observable();
        self.onRoadPrice = ko.observable();
        self.selectedVersion = ko.observable(self.defaultVersion);
        self.bikeVersions = ko.observableArray(bikeVersions);
        self.isDiscontinued = ko.observable(isDiscontinued.toLowerCase() == "true");
        self.setVersionDetails = function (version) {
            $("#priceincity").attr("data-versionid", version.VersionId);
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

    $('.chosen-select').on('change', function () {
        var selectField = $(this);
        if (selectField.val() > 0) {
            selectField.closest('.select-box').addClass('done');
        }
    });

    $('#version-dropdown').chosen().change(function () {
        var obj = $(this);
        vmVersionTable.getVersionObject(obj.val());
        triggerGA(obj.attr("data-cat"), obj.attr("data-act"), obj.attr("data-lab"));
    });

    $window = $(window),
    overallSpecsTabsContainer = $('.overall-specs-tabs-container'),
    modelSpecsTabsContentWrapper = $('#modelSpecsTabsContentWrapper'),
    modelSpecsFooter = $('#modelSpecsFooter');

    $('#modelAlternateBikeContent')

    var alternateBikeContent = $('#modelAlternateBikeContent');

    if (alternateBikeContent.length != 0) {
        alternateBikeContent.removeClass('bw-model-tabs-data');
    }

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
                overallSpecsTabsContainer.find('span').removeClass('active');
                $('#modelSpecsTabsContentWrapper .bw-mode-tabs-data').removeClass('active');

                $(this).addClass('active');

                overallSpecsTabsContainer.find('span[data-href="#' + $(this).attr('id') + '"]').addClass('active');
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

    // emi calculator
    ko.bindingHandlers.slider = {
        init: function (element, valueAccessor, allBindingsAccessor, bindingContext) {
            var options = allBindingsAccessor().sliderOptions || {};
            $("#" + element.id).slider(options);
            ko.utils.registerEventHandler("#" + element.id, "slide", function (event, ui) {
                try{
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
                catch(e){
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
        self.minDnPay = ko.observable(10 * bikeVersionPrice/100);
        self.maxDnPay = ko.observable(40 * bikeVersionPrice/100);
        self.minTenure = ko.observable(12);
        self.maxTenure = ko.observable(48);
        self.minROI = ko.observable(10);
        self.maxROI = ko.observable(15);

        self.processingFees = ko.observable(processingFees);
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
                self.loan(($.LoanAmount(self.exshowroomprice(), 100)) - value);
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
                return (self.downPayment() + (self.monthlyEMI() * self.tenure()) + self.processingFees());
            },
            owner: this
        });
    };          


    $.calculateEMI = function (loanAmount, tenure, rateOfInterest,proFees) {
        var finalEmi;
        try {
            finalEmi = Math.round((loanAmount * rateOfInterest / 1200) / (1 - Math.pow((1 + rateOfInterest / 1200), (-1.0 * tenure))));
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
            price = Math.round(price);
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
    var cityName = $dvPgVar.data("cityarea");

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

    triggerNonInteractiveGA('Price_in_City_Page', 'CoverFox_Link_Shown', bikeName + '_' + cityName);

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
                "pqid": $("#priceincity").data("pqid") || 0,
                "pageurl": document.referrer,
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
                }
            };
            
            dleadvm.setOptions(leadOptions);
        } catch (e) {
            console.warn("Unable to get submit details : " + e.message);
        }

    });
   
});

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

$(".navtab").click(function () {

    try {
        var scrollSectionId = $(this).data('href');
        $('html,body').animate({
            scrollTop: $(scrollSectionId).offset().top - 40
        },
      'slow');

    }
    catch (e) {
        console.warn(e);
    }
});

