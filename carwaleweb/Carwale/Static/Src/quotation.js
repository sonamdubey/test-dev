var tabsCarousel, carouselData;
var focusedPQCity, modelIdFromAddPQ = 0;
var objNewPrice = new Object();
var panel;
var isGoogleApiCalled = false;
var currentURI = location.pathname.substr(1).replace('new/', '');
doNotShowAskTheExpert = false;
var RECOMMENDATIONLEADCLICKSOURCE = null;
var RECOMMENDATIONINQUIRYSOURCE = null;
var PriceBucket = { NoUserCity: 0, HaveUserCity: 1, PriceNotAvailable: 2, CarNotSold: 3 }

var priceQuoteVM = function (priceQuoteObj) {              // Price Quote Object Viewmodel
    var self = this;
    self.carPriceQuote = ko.observable(priceQuoteObj);
    self.tenure = ko.observable(60);
    self.isCrossSellExists = (self.carPriceQuote().IsSponsoredCar == false && self.carPriceQuote().CrossSellCampaignList != null && self.carPriceQuote().CrossSellCampaignList.length > 0) ? true : false;
    // Remove insurance link for Maruti Suzuki Make in Hyderabad, Secunderabad, Bangaluru
    self.showFinanceAdlinksForMakeCity = ko.observable(self.carPriceQuote().carDetails.MakeId == 10 && (self.carPriceQuote().cityDetail.CityId == 105 || self.carPriceQuote().cityDetail.CityId == 394 || self.carPriceQuote().cityDetail.CityId == 2));
    self.selectedVersion = ko.observable(priceQuoteObj.carDetails.VersionId);
    self.interestRate = ko.observable(9.75);

    self.metallicFilters = ko.observableArray([
        { title: ko.observable('Solid Color'), style: ko.observable('active'), filter: false },
        { title: ko.observable('Metallic Color'), style: ko.observable('metalic'), filter: true }
    ]);
    self.enableMetallicFilter = ko.computed(function () {
        return PQ.price.metallicFilterActive(self.carPriceQuote().PriceQuoteList)
    });
    self.activeFilter = ko.observable(self.metallicFilters()[0].filter);
    self.filteredPrices = ko.computed(function () {
        if (self.enableMetallicFilter()) {
            return PQ.price.filteredPrices(self.carPriceQuote().PriceQuoteList, self.activeFilter())
        } else {
            return self.carPriceQuote().PriceQuoteList;
        }
    });
    self.setActiveFilter = function (model, element) {
        $(btObj.btSelector).btOff();
        self.activeFilter(model.filter);
        ko.utils.arrayForEach(self.metallicFilters(), function (metallicFilter) {
            btObj.registerEventsClass();
            if (model.title == metallicFilter.title)
                metallicFilter.style('active');
            else
                metallicFilter.style('metalic');
        });
    };

    self.exshowroom = ko.observable(priceQuoteObj.PriceQuoteList.length < 1 ? 0 : self.filteredPrices()[0].Value);
    self.Loan = ko.observable(Math.round(self.exshowroom() * .85));
    self.onRoadPrice = ko.computed(function () {
        return PQ.price.calculateORP(self.filteredPrices())
    });
    self.minPayableAmount = ko.observable(self.onRoadPrice() - self.exshowroom());
    self.showGstLabel = ko.computed(function () {
        if (self.filteredPrices().length > 0 && self.filteredPrices()[0].Key.indexOf("gst-est-tooltip") >= 0)
            return true;
        else
            return false;

    });

    self.nearByCities = ko.observableArray();

    self.downPayment = ko.pureComputed({
        read: function () {
            return self.onRoadPrice() - self.Loan();
        },
        write: function (value) {
            self.Loan(self.onRoadPrice() - value);
        },
        owner: this
    });
    self.monthlyEMI = ko.pureComputed({
        read: function () {
            return calculateEMI(self.tenure(), self.interestRate(), self.Loan());
        },
        owner: this
    });

    self.savingLinkClick = function () {
        PQ.advertisement.BhartiAxa.openPopUp();
        dataLayer.push({ event: 'PQ-Page-Tracking', cat: "Bhatia-Axa", act: "Amount_Link_Clicked_With_Savings", lab: self.carPriceQuote().carDetails.MakeName + ' ' + self.carPriceQuote().carDetails.ModelName + ' ' + self.carPriceQuote().cityDetail.CityName + ' ' + self.carPriceQuote().InsuranceDiscount.Discount });
        return true;
    }
    self.genericSaveLinkClick = function () {
        PQ.advertisement.BhartiAxa.openPopUp();
        dataLayer.push({ event: 'PQ-Page-Tracking', cat: "Bhatia-Axa", act: "Generic_Link_Clicked_With_Savings", lab: self.carPriceQuote().carDetails.MakeName + ' ' + self.carPriceQuote().carDetails.ModelName + ' ' + self.carPriceQuote().cityDetail.CityName + ' ' + self.carPriceQuote().InsuranceDiscount.Discount });
        return true;
    }
    self.freeQuoteLinkClick = function () {
        PQ.advertisement.BhartiAxa.openPopUp();
        dataLayer.push({ event: 'PQ-Page-Tracking', cat: "Bhatia-Axa", act: "Take_a_free_quote_Link_Clicked", lab: self.carPriceQuote().carDetails.MakeName + ' ' + self.carPriceQuote().carDetails.ModelName + ' ' + self.carPriceQuote().cityDetail.CityName });
        return true;
    }
    self.insuranceLinkClick = function () {
        PQ.advertisement.BhartiAxa.openPopUp();
        dataLayer.push({ event: 'PQ-Page-Tracking', cat: "Bhatia-Axa", act: "A_comprehensive_insurance_BhartiAXA_Clicked", lab: self.carPriceQuote().carDetails.MakeName + ' ' + self.carPriceQuote().carDetails.ModelName + ' ' + self.carPriceQuote().cityDetail.CityName });
        return true;
    }
    self.knowMoreLinkClick = function () {
        PQ.advertisement.BhartiAxa.openPopUp();
        dataLayer.push({ event: 'PQ-Page-Tracking', cat: "Bhatia-Axa", act: "A_comprehensive_insurance_KnowMore_Clicked", lab: self.carPriceQuote().carDetails.MakeName + ' ' + self.carPriceQuote().carDetails.ModelName + ' ' + self.carPriceQuote().cityDetail.CityName });
        return true;
    }

    self.getPQOnCityChange = function (locationData, modelId, versionId) {
        var pqData = { 'modelId': modelId, 'location': locationData, 'versionId': versionId };
        PriceBreakUp.Quotation.setPQCityCookies(pqData);
        _viewModel.oldPQ = this;
        _viewModel.selectedAreaId = locationData.areaId;
        _viewModel.selectedAreaName = locationData.areaName;
        var pqRequest = getPQInputes(pqPageId.PQByChangingCity);
        getNewPQ(pqRequest);
        dataLayer.push({ event: 'PQ-Page-Tracking', cat: "PQ-Panel", act: "CityChange", lab: '' });
    }

    self.getPQOnVersionChange = function (data, obj) {
        var priceQuote = obj.carPriceQuote();
        self.selectedVersion(data.ID);
        var locationData = { cityId: priceQuote.cityDetail.CityId, cityName: priceQuote.cityDetail.CityName, zoneId: priceQuote.cityDetail.ZoneId, areaId: priceQuote.cityDetail.AreaId, areaName: priceQuote.cityDetail.AreaName };
        var pqData = { 'modelId': priceQuote.carDetails.ModelId, 'versionId': self.selectedVersion(), 'location': locationData };
        PriceBreakUp.Quotation.setPQCityCookies(pqData);
        _viewModel.oldPQ = obj;
        _viewModel.selectedAreaId = priceQuote.cityDetail.AreaId;
        _viewModel.selectedAreaName = priceQuote.cityDetail.AreaName;
        var pqRequest = getPQInputes(pqPageId.PQByChangingVersion);
        dataLayer.push({ event: 'PQ-Page-Tracking', cat: "PQ-Panel", act: "VersionChange", lab: '' });
        getNewPQ(pqRequest);
    }

    self.nearByCity = function (locationObj) {
        var scrollTop = parseInt($('html').css('top'));
        $('html').removeClass('lock-browser-scroll');
        $('html,body').scrollTop(-scrollTop);
        Common.isScrollLocked = false;
        var pqRequest = getPQInputes(pqPageId.PQByNearCityClick);
        if (locationObj.cityId > 0) {
            pqRequest.CityId = locationObj.cityId;
            pqRequest.ZoneId = locationObj.zoneId;
            pqRequest.AreaId = locationObj.areaId;
        }
        getNewPQ(pqRequest);
    }

    self.initiateLocationPlugin = function (priceQuoteObj) {
        var div = $('.quotation-box[PQId="' + priceQuoteObj.EnId + '"]').find('#inputCityPriceQuote');
        var location = new LocationSearch((div), {
            showCityPopup: true,
            callback: function (locationObj) {
                var modelId = priceQuoteObj.carDetails.ModelId;
                var versionId = priceQuoteObj.carDetails.VersionId;
                self.getPQOnCityChange(locationObj, modelId, versionId);
                location.closePopup();
                cwTracking.trackCustomData('QuotationPage', 'CityChange', 'OldCity:' + priceQuoteObj.cityDetail.CityName + '|NewCity:' + locationObj.cityName, false);
            },
            ctaText: 'CHECK NOW',
            setGlobalCookie: false
        })
    }

    self.initAlternateCarsLink = function (priceQuoteObj) {
        var div = $('.price-quote-div[PQId="' + priceQuoteObj.EnId + '"]').find('.quotation-alternate-car-link');
        var location = new LocationSearch((div), {
            showCityPopup: true,
            callback: function (locationObj) {
                var pqInput = {};
                pqInput.modelId = location.selector().attr('modelid');
                pqInput.versionId = "";
                pqInput.pageId = pqPageId.PQByAlternateCars;
                pqInput.location = locationObj
                location.closePopup();
                PriceBreakUp.Quotation.RedirectToPQ(pqInput);
            },
            isDirectCallback: true,
            setGlobalCookie: false,
            validationFunction: function () {
                return validateCity();
            }
        });
    }

    self.initNearbyCityLink = function (priceQuoteObjEnId) {
        var div = $('.price-quote-section[PQId="' + priceQuoteObjEnId + '"]').find('.nearbycity');
        var location = new LocationSearch((div), {
            showCityPopup: true,
            callback: function (locationObj) {
                var context = ko.contextFor(location.selector()[0]);
                var obj = context.$parent;
                _viewModel.oldPQ = obj;
                self.nearByCity(locationObj);
                location.closePopup();
            },
            isDirectCallback: true,
            prefillPopup: true,
            setGlobalCookie: false,
            validationFunction: function () {
                var locObj = { cityId: location.selector().attr('data-cityId'), cityName: location.selector().attr('data-cityName'), isComplete: false };
                location.setLocation({ cityId: locObj.cityId, cityName: locObj.cityName });
                return validateNearbyCity(locObj);
            }
        });
    }

    self.toggleVariantCustomDropDown = function (data, event) {
        $customContainer = $(event.target).closest('.selectcustom-container');
        $customContent = $customContainer.find('.selectcustom-content');
        $customContentHolder = $customContainer.find('#selectcustom-input-box-holder');

        $.each($customContent.find('li'), function (key, value) {
            var curOptionText = $(value).find('.pop-version-name').text();
            if (curOptionText == $(event.target).text())
                $(this).addClass("selected");
            else
                $(this).removeClass("selected");
        });

        $customContent.toggle().addClass("selectcustom-active");
        $customContentHolder.addClass("selectcustom-active");

        if ($customContent.find("li.selected").length > 0) {
            var customContentHeight = $customContent.height();
            var selelectedLiPosition = $customContent.find("li.selected").position().top;

            if ($customContentHolder.get(0).getBoundingClientRect().top + customContentHeight > window.innerHeight)
                $customContent.css("top", "-" + customContentHeight + "px");
            else
                $customContent.css("top", "35px");

            if ((selelectedLiPosition + 54) > customContentHeight)
                $customContent.animate({ scrollTop: selelectedLiPosition - customContentHeight + 54 }, 100);
        }
    }

    self.changeDropDownClassOnHover = function (data, event) {
        $(event.target).closest('ul').find('li[class="selected"]').removeClass("selected");
        $(event.target).addClass("selected");
    }

    self.sliderChangeTrack = function (event, ui) {
        if (event.originalEvent) {
            cwTracking.trackCustomData('QuotationPage', 'EMIQuotesChange', event.target.attributes['sliderName'].value + ':' + ui.value, false);
        }
    }
}
ko.bindingHandlers.slider = {
    init: function (element, valueAccessor, allBindingsAccessor, bindingContext) {
        var options = allBindingsAccessor().sliderOptions || {};
        $(element).slider(options);
        ko.utils.registerEventHandler(element, "slide", function (event, ui) {
            var observable = valueAccessor();
            observable(ui.value);
        });
    },
    update: function (element, valueAccessor, allBindingsAccessor, bindingContext) {
        var options = allBindingsAccessor().sliderOptions || {};
        $(element).slider(options);
        var value = ko.utils.unwrapObservable(valueAccessor());
        if (isNaN(value)) value = 0;
        $(element).slider("value", value);
    }
};


var addNewPQ = function (data, event) {                          // function for Adding New PQ
    var priceQuote =
        ko.computed(function () {
            return ko.utils.arrayFilter(data.priceQuoteCollection(), function (res) {
                return res.carPriceQuote().EnId == $('#pqTabs').find('.quo-active').attr('pqid');
            });
        });
    callToSuggestionCar(priceQuote()[0].carPriceQuote());

    var caption = "SELECT CAR TO CHECK PRICE";
    var applyIframe = false;
    var url = "";
    modelIdFromAddPQ = 0;
    GB_show(caption, url, 210, 510, applyIframe, $('#pqPopup').html());
    initLocationPluginForAnotherCarPopUp();
    $("#gb-window").addClass("area-shown");
    dataLayer.push({ event: 'PQ-Page-Tracking', cat: "PQ-Panel", act: "AddAnotherCarClick", lab: "Tab-No-" + getNoOfTabs() })
}
function initLocationPluginForAnotherCarPopUp() {
    var div = document.getElementById('addCarPopUp');
    locationPluginObj = new LocationSearch((div), {
        showCityPopup: false,
        customPopUpSelector: $("#addCarPopUp"),
        setGlobalCookie: false
    })
}

function callToSuggestionCar(obj) {
    var refModelId = obj.carDetails.ModelId;
    var refCityId = obj.cityDetail.CityId;
    var refZoneId = obj.cityDetail.ZoneId;

    $.ajax({
        type: 'GET',
        url: '/api/pq/recommendedCars/?userHistory=' + userHistory.getUserModelHistory() + '&noOfReco=3&refModelId=' + refModelId + '&cityId=' + refCityId + '&zoneId=' + refZoneId + '&platformId=1',
        success: function (json) {
            ko.cleanNode(document.getElementById('suggestCars'));
            ko.applyBindings(json, document.getElementById('suggestCars'));
            bindAdPQAutocomplete();
            $("#addPQAutocomplete").focus();
        }
    });
}
//check below method 
function fillAutoComplete(data, event) {
    modelIdFromAddPQ = data.modelId;
    $.cookie('_PQModelId', data.modelId, { path: '/' });
    $.cookie('_PQVersionId', '', { path: '/' });
    $('#addPQAutocomplete').val(data.makeName + ' ' + data.modelName);
    $('#addCarError').text('');
    prefillPQObject();
}

var pqPageId = {
    PQOnPageLoad: 1,
    PQByNewTab: 15,
    PQByChangingCity: 16,
    PQByChangingVersion: 17,
    PQByChangingSubRegion: 20,
    PQByAlternateCars: 36,
    PQByCrossSell: 59,
    PQByNearCityClick: 132
}

function showHideDrpError(element, error) {
    if (error) {
        $(element[0]).removeClass('hide');
        $(element[1]).removeClass('hide');
    }
    else {
        $(element[0]).addClass('hide');
        $(element[1]).addClass('hide');
    }
}
function hideModelErrorPQ() {
    $("#Make").find('.cityErrIcon').hide();
    $("#Make").find('.cityErrorMsg').hide();
    $("#Make").find('.cityErrorMsg').text('');
}

function prefillPQObject() {
    $("#addCarPopUp .city-search").val("");
    $("#addCarPopUp .pqlocation-plugin-area").val("");
    hideModelErrorPQ();
    var pqLocationObj = PriceBreakUp.Quotation.getPQLocation();
    pqLocationObj.areaId = _viewModel.selectedAreaId;
    pqLocationObj.areaName = _viewModel.selectedAreaName;
    locationPluginObj.prefill(pqLocationObj, "#addCarPopUp");
    locationPluginObj.setLocation(pqLocationObj);
}

function bindAdPQAutocomplete() {
    $('#addPQAutocomplete').cw_autocomplete({
        isPriceExists: 1,
        resultCount: 10,
        textType: ac_textTypeEnum.model,//+ ',' + ac_textTypeEnum.version,
        source: ac_Source.generic,
        onClear: function () {
            objNewPrice = new Object();
        },
        click: function (event, ui, orgTxt) {
            objNewPrice.Name = ui.item.label;
            objNewPrice.Id = ui.item.id;
            modelIdFromAddPQ = ui.item.id.split(':')[2].split("|")[0];
            $('#addPQAutocomplete').val(objNewPrice.Name);
            prefillPQObject();
            hideModelErrorPQ();
            $.cookie('_PQModelId', modelIdFromAddPQ, { path: '/' });
            $.cookie('_PQPageId', "15", { path: '/' });
            $.cookie('_PQVersionId', "0", { path: '/' });
        },
        open: function (result) {
            objNewPrice.result = result;
        },
        afterfetch: function (result, searchtext) {
            if (result != undefined && result.length > 0) {
                showHideDrpError($('#addPQAutocomplete').siblings(), false);
            }
            else {
                modelIdFromAddPQ = null;
                showHideDrpError($('#addPQAutocomplete').siblings(), true);
                ShowDropDownDisabled('#drpCity');
            }
        },
        focusout: function () {
            if ($('li.ui-state-focus a:visible').text() != "") {
                focusedPQCity = objNewPrice.result[$('li.ui-state-focus').index()];
                $('#addPQAutocomplete').val(focusedPQCity.label);
                modelIdFromAddPQ = focusedPQCity.id.split(':')[2];
                prefillPQObject();
                hideModelErrorPQ();
                $.cookie('_PQModelId', modelIdFromAddPQ, { path: '/' });
                $.cookie('_PQVersionId', "0", { path: '/' });
            }
        }
    });
}

function onViewMapTrack(tabNo) {
    var tab = $(tabNo).closest(".cw-tabs-panel").parent().parent().parent().parent().parent();
    var latitude = tab.find(".latitude").text();
    var longitude = tab.find(".longitude").text();
    var dealerName = tab.find(".dealername").text();
    tab.find('#googleMap').attr('href', "https://maps.google.com/maps?z=12&t=m&q=loc:" + latitude + '+' + longitude);
}

var FeaturedTab = undefined;
var isActivePresent = 0;

var isHashHaveModel = false;
var IsSponsoredCarShowed = false;

function showImageLoading() {             // function for showing image Loading
    $('div.blackOut-window').show();
    $('#loadingCarImg').show();
}

function hideImageLoading() {             // function for hiding image Loading
    $('div.blackOut-window').hide();
    $('#loadingCarImg').hide();
    $("#pq-container").show();

    Location.globalSearch.bindGlobalCityLabel();

    if (!isCookieExists('_CoachmarkStatus')) {
        showCoachmarks();
    }

    tabAnimation.tabHrInit();
}

var viewModel = function () {                     // Viewmodel
    var self = this;
    self.priceQuoteCollection = ko.observableArray();
    self.oldPQ = ko.observable();
    self.selectedAreaId = ko.observable();
    self.selectedAreaName = ko.observable();

    self.removeTab = function (place) {
        quoCarousel.registerEvents();
        wrapCarousel.noAdCarousel();
        setTimeout(function () {
            wrapCarousel.adCarousel();
        }, 10);

        ShowNextActiveTab(place.carPriceQuote().EnId);

        $('#leaderBoard div[lbSlotId="' + place.carPriceQuote().EnId + '"]').remove();

        self.priceQuoteCollection.remove(place);
        countliLength();
        HideShowCloseButton();
        refreshCarousel();
        //when sponsored car is removed set IsSponsoredCarShowed to false
        if (place.carPriceQuote().IsSponsoredCar == true)
            IsSponsoredCarShowed = false;

        var tabCount = $("#pqTabs li").length;
        if (tabCount == 1) {
            $(".stPrev, .stNext").addClass("hide");
            $("#pq-jcarousel").css({ 'width': '292px' });
            $(".quotation-tabs li").css({ 'width': '291px' });
            $(".quotation-tabs ul").css({ 'width': '292px', 'margin': '0', 'border-right': '0' });
            $(".orp-add-btn .add-text").removeClass("hide");
            //$(".orp-add-btn").css({ 'right': '540px', 'border-right': '1px solid #e2e2e2' });
            $(".ui-icon-close").hide();
        }
        if (tabCount == 2) {
            $(".stPrev, .stNext").addClass("hide");
            $("#pq-jcarousel").css({ 'width': '590px' });
            $(".quotation-tabs li").css({ 'width': '291px' });
            $(".quotation-tabs ul").css({ 'width': '590px', 'margin': '0' });
            $(".orp-add-btn .add-text").removeClass("hide");
            focusLi(nextPQId);
            //$(".orp-add-btn").css({ 'right': '315px', 'border-right': '1px solid #e2e2e2' });
        }

        dataLayer.push({ event: 'PQ-Page-Tracking', cat: "PQ-Panel", act: "TabRemoved", lab: "Tab-No-" + getNoOfTabs() });
    }


}

function getNoOfTabs() {//function for getting the no of tabs
    try {
        var pqIdArr = getParameterByName('pqid').split(',');
        var length = pqIdArr.length;
        return length;
    }
    catch (e) {
        console.log("getNoOfTabs:" + e.message);
    }
}

function cityVersionCookieChange(pqId) {        // function for setting City and version Cookie
    var priceQuote = lookup(_viewModel.priceQuoteCollection(), pqId);
    locationData = { cityId: priceQuote.cityDetail.CityId, cityName: priceQuote.cityDetail.CityName, zoneId: priceQuote.cityDetail.ZoneId };
    var pqData = { 'modelId': priceQuote.carDetails.ModelId, 'versionId': priceQuote.carDetails.VersionId, 'location': locationData };
    PriceBreakUp.Quotation.setPQCityCookies(pqData);
    _viewModel.selectedAreaId = priceQuote.cityDetail.AreaId;
    _viewModel.selectedAreaName = priceQuote.cityDetail.AreaName;
}

function lookup(array, value) {
    for (var i = 0, len = array.length; i < len; i++)
        if (array[i] && array[i].carPriceQuote().EnId === value)
            return array[i].carPriceQuote();
}

var _viewModel = new viewModel();

function getPQInputes(pageId) {              // function for providing Input to the ajax function
    var isPqOrDealerPageId = ModelCar.PQ.pageId.isValidPageId(pageId);
    var modelId = $.cookie('_PQModelId');
    var cityId = $.cookie('_CustCityId');
    var zoneId = $.cookie('_PQZoneId');
    var areaId = _viewModel.selectedAreaId;
    var pqInputes = new Object();
    pqInputes.Name = $.cookie('_CustomerName');
    pqInputes.Email = $.cookie('_CustEmail');
    pqInputes.Mobile = $.cookie('_CustMobile');
    pqInputes.CarVersionId = $.cookie('_PQVersionId');
    pqInputes.CarModelId = $.cookie('_PQModelId');
    pqInputes.CityId = cityId;
    pqInputes.AreaId = areaId;
    pqInputes.ZoneId = Number(zoneId) > 0 ? zoneId : "";
    pqInputes.CarModelId = modelId;
    pqInputes.SourceId = '1';
    pqInputes.PageId = Number(pageId) > 0 ? pageId : '0';
    pqInputes.CampaignId = getPersistentDealerCampaign(modelId, cityId, zoneId);
    pqInputes.IsSponsoredCarShowed = IsSponsoredCarShowed;
    pqInputes.UtmaCookie = isCookieExists('__utma') ? $.cookie('__utma') : '';
    pqInputes.UtmzCookie = isCookieExists('_cwutmz') ? $.cookie('_cwutmz') : '';
    return pqInputes;
}

function validatePQCookies() {
    var modelId = $.cookie('_PQModelId');
    var cityId = $.cookie('_CustCityId');
    var zoneId = $.cookie('_PQZoneId');
    var versionId = $.cookie('_PQVersionId')

    if (Number(cityId) > 0 && (Number(modelId) > 0 || Number(versionId) > 0))
        return true;
    else
        return false;
}

function applyKOBindings() {
    ko.applyBindings(_viewModel, document.getElementById('pq-container'));
}

function btToolTip() {                             // function for beauty tooltip
    $("div.question-box").each(function (index, btdiv) {
        bindBT($(btdiv));
    })
}
function bindBT(btdiv) {                    // function to bind Beauty tooltip
    btdiv.bt({
        contentSelector: "$('#" + (btdiv.hasClass("thane") ? "thane" : "pune") + "ZoneContent').html()",
        fill: '#fff',
        strokeWidth: 1,
        strokeStyle: '#cacaca',
        trigger: ['hover', ''],
        width: '300',
        spikeLength: 8,
        positions: ['right'],
        padding: 10,
        shadow: true,
        shadowOffsetX: 3,
        shadowOffsetY: 3,
        shadowBlur: 8,
        shadowColor: '#ccc',
        shadowOverlap: false
    });
}

// function to bind ajax functions
function bindAdDivs(pqId) {
    var indexOfPQObject = getPQObjectByPQId(pqId);

    if (Number(indexOfPQObject) >= 0) {

        var pqObject = getPQObjectAtIndex(indexOfPQObject);
        var _cityName = Number(pqObject.cityDetail.ZoneId) > 0 ? pqObject.cityDetail.ZoneName.replace(/"/g, "").replace(/'/g, "").replace(/\(|\)/g, "") : pqObject.cityDetail.CityName;
        var _modelName = pqObject.carDetails.ModelName;

        var ad1 = "<div class=\"text-center margin-top10\"><div class=\"adunit sponsored\" data-adunit=\"PriceQuote_970x90\" data-dimensions=\"970x90\" data-targeting='{\"City\":\"" + _cityName + "\",\"Models\":\"" + _modelName + "\"}'></div>";
        var addiv = $('.pq-below-container[pqid="' + pqId + '"]').find(".dfp_pq_ad");
        addiv.html(ad1);

        if ($('#leaderBoard div[lbSlotId="' + pqId + '"]').length == 0) {
            var leaderBoard = "<div lbSlotId='" + pqId + "'class=\"adunit sponsored\" data-adunit=\"PriceQuote_ATF_970x90\" data-dimensions=\"970x90,960x60,970x66,960x66,970x60,728x90\" data-targeting='{\"City\":\"" + _cityName + "\",\"Models\":\"" + _modelName + "\"}'></div>";
            $("#leaderBoard").append(leaderBoard);
        }
    }
}

function callDFP() {
    try {
        $.dfp({
            dfpID: '1017752',
            enableSingleRequest: false,
            collapseEmptyDivs: true,
            afterEachAdLoaded: function (adunit) {
                var parentAdUnit = $(adunit).parent();
                $("div.dfp_pq_ad").parent().show();
                if ($(adunit).hasClass('display-block')) { // ad present
                    parentAdUnit.removeClass("hide");
                } else {
                    parentAdUnit.addClass("hide"); // no ad present
                }
            }
        });
    }
    catch (err) {
        console.log('dfp:' + err.message);
    }
}

/* Ad Carousel Code Starts Here */
var quoCarousel = {
    registerEvents: function () {
        $('.jcarousel').jcarousel();
        $('.jcarousel-control-prev')
            .on('jcarouselcontrol:active', function () {
                $(this).removeClass('inactive');
            })
            .on('jcarouselcontrol:inactive', function () {
                $(this).addClass('inactive');
            })
            .jcarouselControl({
                target: '-=1'
            });

        $('.jcarousel-control-next')
            .on('jcarouselcontrol:active', function () {
                $(this).removeClass('inactive');
            })
            .on('jcarouselcontrol:inactive', function () {
                $(this).addClass('inactive');
            })
            .jcarouselControl({
                target: '+=1'
            });

        $('.jcarousel-pagination')
            .on('jcarouselpagination:active', 'a', function () {
                $(this).addClass('active');
            })
            .on('jcarouselpagination:inactive', 'a', function () {
                $(this).removeClass('active');
            })
            .jcarouselPagination();
    }
}
var wrapCarousel = {
    adCarousel: function () {
        $(".multiple-ad-container .jcarousel").jcarousel({
            wrap: 'circular',
            vertical: false,
        });
    },
    noAdCarousel: function () {
        $(".multiple-ad-container .jcarousel").jcarousel({
            wrap: null,
            vertical: false
        });
    },
    registerEvents: function () {
        $('#pq-jcarousel li').on('click', function () {
            wrapCarousel.noAdCarousel();
            setTimeout(function () {
                wrapCarousel.adCarousel();
            }, 10);
        });
    }
}
/* Ad Carousel Code Ends Here */

/* Ad Slug Animation Code on load of Page */
$(document).live('ready', '#ctl00', function () {
    $('body').addClass('animate-ad', function () {
        if ($('.animate-ad').is(':visible')) {
            ModelCar.PQ.animateSlug(),
                setTimeout(function () { $('body').addClass("show-animate").removeClass("animate-ad"); });
        }
        Quotation.registerEvents();
        quoCarousel.registerEvents();
        wrapCarousel.adCarousel();
        wrapCarousel.registerEvents();
    });
});

function getNewPQ(pqRequest) {
    // function for getting new price Quote
    var addAnotherSbtBtn = $("#btnAddCarSubmit");
    if (pqRequest.PageId == pqPageId.PQByNewTab) {
        if (IsValidate()) {
            var addAnotherSbtBtn = $("#btnAddCarSubmit");
            if (addAnotherSbtBtn.length > 0) addAnotherSbtBtn.attr('value', 'Loading...');
            GB_hide();
        }
        else {
            if (addAnotherSbtBtn.length > 0) addAnotherSbtBtn.attr('value', 'CHECK NOW');
            return;
        }
    }
    showImageLoading();

    $.ajax({
        type: 'POST',
        url: '/v1/api/pq/',
        dataType: 'Json',
        contentType: "application/x-www-form-urlencoded",
        data: pqRequest,
        statusCode: {
            206: function (response) {
                if (response != null) {
                    window.location.href = response.responseText;
                }
            }
        },
        success: function (response) {
            try {
                if (response == null || response.length == 0) {
                    window.location = '/new/prices.aspx';
                }
                if (response.length > 1)
                    IsSponsoredCarShowed = true;

                pqResponse(response, pqRequest.PageId);

                initFunctionsOnPqResponse(response);

                pqTrackingEvents(response);
                if (response[0].SponsoredDealer == null || response[0].SponsoredDealer.DealerId == 0) {
                    dataLayer.push({ event: 'PQ-Page-Tracking', cat: "Finance_Desktop", act: "BB_PQpage_EMIbox_Finance_Shown", lab: response[0].carDetails.MakeName + ',' + response[0].carDetails.ModelName + ',' + response[0].carDetails.VersionName + "," + response[0].cityDetail.CityName });
                    dataLayer.push({ event: 'PQ-Page-Tracking', cat: "PQ_Desktop_PlaceholderAd", act: "Block Shown", lab: response[0].carDetails.MakeName + ',' + response[0].carDetails.ModelName + ',' + response[0].cityDetail.CityName });
                }
                /* Ad Animation Code Starts Here */
                if (ModelCar.PQ.isAnimateSlugShown(pqRequest.PageId)) {
                    ModelCar.PQ.animateSlug();
                }

                if (ModelCar.PQ.isAnimateSlugStop(pqRequest.PageId)) {
                    ModelCar.PQ.noAnimateSlug();
                }
                /* Ad Animation Code Ends Here */
                quoCarousel.registerEvents();
                btObj.registerEventsClass();
                wrapCarousel.adCarousel();
                wrapCarousel.registerEvents();
                if (pqRequest.PageId == pqPageId.PQByNewTab)
                    cwTracking.trackCustomData('QuotationPage', 'CarAdd', 'make:' + response[0].carDetails.MakeName + '|model:' + response[0].carDetails.ModelName + '|version:' + response[0].carDetails.VersionName + '|city:' + response[0].cityDetail.CityName, false);
                if (!ModelCar.PQ.pageId.isValidPQPageId(pqRequest.PageId))
                    cwTracking.trackCustomData('QuotationPage', 'PQPageload', 'make:' + response[0].carDetails.MakeName + '|model:' + response[0].carDetails.ModelName + '|version:' + response[0].carDetails.VersionName + '|city:' + response[0].cityDetail.CityName, false);

                fillNearbyCities(response, true);
            } catch (err) {
                console.log('postpq' + err.message);
            }
        },
        failure: function (response) {
            hideImageLoading();
            alert("Failure, check connection.");
        }
    });
}

function validateCity() {
    var locObj = PriceBreakUp.Quotation.getPQLocation();
    if (Number(locObj.cityId) > 0) {
        if ($.inArray(Number(locObj.cityId), askingAreaCityId) >= 0 && Number(_viewModel.selectedAreaId) > 0) {
            locObj.areaId = _viewModel.selectedAreaId;
            locObj.areaName = _viewModel.selectedAreaName;
            locObj.isComplete = true;
            return locObj;
        } else if ($.inArray(Number(locObj.cityId), askingAreaCityId) < 0) {
            locObj.isComplete = true;
            return locObj;
        } else {
            return locObj;
        }
    } else {
        return null;
    }
}

function validateNearbyCity(locObj) {
    if (Number(locObj.cityId) >= 0) {
        if ($.inArray(Number(locObj.cityId), askingAreaCityId) >= 0) {
            return locObj;
        }
        else {
            locObj.isComplete = true;
            return locObj;
        }
    }
    else {
        return null;
    }
}


function pqTrackingEvents(responseObj) {
    dataLayer.push({ event: "quotationEvent" });
    dataLayer.push({ event: 'PQ-Page-Tracking', cat: "PQ-Panel", act: "PQTaken", lab: '' });
    switch (responseObj[0].InsuranceDiscount.Type) {
        case 1:
            dataLayer.push({ event: 'PQ-Page-Tracking', cat: "Bhatia-Axa", act: "Savings_communication_shown ", lab: responseObj[0].carDetails.MakeName + ' ' + responseObj[0].carDetails.ModelName + ' ' + responseObj[0].cityDetail.CityName + ' ' + responseObj[0].InsuranceDiscount.Discount });
            break;
        case 2:
            dataLayer.push({ event: 'PQ-Page-Tracking', cat: "Bhatia-Axa", act: "A_comprehensive_insurance_communication_shown", lab: responseObj[0].carDetails.MakeName + ' ' + responseObj[0].carDetails.ModelName + ' ' + responseObj[0].cityDetail.CityName });
            break;
        case 3:
            dataLayer.push({ event: 'PQ-Page-Tracking', cat: "Bhatia-Axa", act: "Take_a_free_quote_communication_shown", lab: responseObj[0].carDetails.MakeName + ' ' + responseObj[0].carDetails.ModelName + ' ' + responseObj[0].cityDetail.CityName });
            break;
    }
}

function alternateCars() {                      // function for binding alteranate cars corousel click
    bindCarousel();
    $(".alternate-cars-carousel .jcarousel-control-prev").live('click', function () {
        alternateCarousel.jcarousel('scroll', '-=1')
    });
    $(".alternate-cars-carousel .jcarousel-control-next").live('click', function () {
        $(".alternate-cars-carousel .jcarousel-control-prev").removeClass('inactive');
        alternateCarousel.jcarousel('scroll', '+=1')
    });
}

function fillNearbyCities(response, isAsync) {
    for (var index = 0; index < response.length; index++) {
        if (response[index].PriceQuoteList.length == 0) {
            getNearbyCities(response[index], isAsync);
        }
    }
}

function getNearbyCities(currPq, isAsync) {
    var index = getPQObjectByPQId(currPq.EnId);
    $.ajax({
        type: 'GET',
        url: '/api/v1/pq/nearbycities/?versionid=' + currPq.carDetails.VersionId + '&cityid=' + currPq.cityDetail.CityId + '&count=' + nearByCityCount,
        dataType: 'Json',
        async: isAsync,
        contentType: "application/x-www-form-urlencoded",
        success: function (response) {
            if (response != null) {
                _viewModel.priceQuoteCollection()[index].nearByCities(response.cities);
                _viewModel.priceQuoteCollection()[index].initNearbyCityLink(currPq.EnId);
            }
        }
    });
}

function pqResponse(response, pageId) {

    if (typeof response !== 'undefined') {
        var isCarPresent = false;
        var pqArray = [];

        for (var car = 0; car < _viewModel.priceQuoteCollection().length; car++) {

            var currentPqObject = _viewModel.priceQuoteCollection()[car].carPriceQuote();

            if (PQ.price.isCarPresentOnPqPage(currentPqObject, response)) {
                isCarPresent = true;
                pqArray.push(currentPqObject);
                _viewModel.oldPQ = _viewModel.priceQuoteCollection()[car];
                replacePQToViewModel(pqArray, pageId);
                response = pqArray;
                break;
            }
        }
        PQ.price.addOrReplacePqToViewModel(isCarPresent, pageId, response);
    }

    // for specific IE browser version 8
    if ($.browser.msie && parseInt($.browser.version, 10) === 8) {
        $('.quotation-box[PQId="' + response[0].EnId + '"]').find("#drpPqCity").val(response[0].cityDetail.ZoneId == "" ? response[0].cityDetail.CityId : (response[0].cityDetail.CityId + '.' + response[0].cityDetail.ZoneId));
    }

    showHideContainers(response[0].EnId);
    HideShowCloseButton();
}

function hideShowCurrentTabPrice(pqId) {
    $('.pq-car-thumb').show();
    $('.pq-price').show();
    $('#pqTabs [pqid="' + pqId.toString() + '"]').find('.pq-car-thumb').hide();
    $('#pqTabs [pqid="' + pqId.toString() + '"]').find('.pq-price').hide();

}

// function tio replace object on version or city change in price Quote page
function replacePQToViewModel(pqCarsArray, pageId) {
    try {
        pqCarsArray[0].EnId = decodeURIComponent(pqCarsArray[0].EnId);
        var indexOfCurrentPQ = $.inArray(_viewModel.oldPQ, _viewModel.priceQuoteCollection());
        if (_viewModel.oldPQ.carPriceQuote().IsSponsoredCar == true) {
            IsSponsoredCarShowed = false;
        }
        updateUrl(_viewModel.oldPQ.carPriceQuote().EnId, pqCarsArray[0].EnId);
        if (pqCarsArray[0].DealerShowroom != null) {
            if (pqCarsArray[0].DealerShowroom.objImageList != null && pqCarsArray[0].DealerShowroom.objImageList.length > 8) {
                pqCarsArray[0].DealerShowroom.objImageList = pqCarsArray[0].DealerShowroom.objImageList.slice(0, 8);
            }
        }

        $('#leaderBoard div[lbSlotId="' + _viewModel.oldPQ.carPriceQuote().EnId + '"]').remove();

        var pq = new priceQuoteVM(pqCarsArray[0]);
        _viewModel.priceQuoteCollection.replace(_viewModel.priceQuoteCollection()[indexOfCurrentPQ], pq);
        pq.initiateLocationPlugin(pqCarsArray[0]);
        pq.initAlternateCarsLink(pqCarsArray[0]);

        setAllCTALeadFormAttribute(pqCarsArray[0]);

        if (pqCarsArray[0].SponsoredDealer != null && pqCarsArray[0].SponsoredDealer.DealerId != null && pqCarsArray[0].SponsoredDealer.DealerId != 0)
            saveDealerCookie(pqCarsArray[0].SponsoredDealer.DealerId, pqCarsArray[0].cityDetail.CityId.toString(), pqCarsArray[0].cityDetail.ZoneId, pqCarsArray[0].carDetails.ModelId)
        hideShowCurrentTabPrice(pqCarsArray[0].EnId.toString());
        if (pqCarsArray.length > 1) {
            _viewModel.priceQuoteCollection.splice(indexOfCurrentPQ + 1, 0, new priceQuoteVM(pqCarsArray[1]));
            addToUrl(pqCarsArray[1].EnId.toString(), undefined, indexOfCurrentPQ, pqCarsArray[0].EnId.toString(), pqCarsArray[0].EnId.toString());
            if (pqCarsArray[1].SponsoredDealer != null && pqCarsArray[1].SponsoredDealer.DealerId != null && pqCarsArray[1].SponsoredDealer.DealerId != 0)
                saveDealerCookie(pqCarsArray[1].SponsoredDealer.DealerId, pqCarsArray[1].cityDetail.CityId.toString(), pqCarsArray[1].cityDetail.ZoneId, pqCarsArray[1].carDetails.ModelId);
            if (pqCarsArray[1].DealerShowroom != null) {
                if (pqCarsArray[1].DealerShowroom.objImageList != null && pqCarsArray[1].DealerShowroom.objImageList.length > 8) {
                    pqCarsArray[1].DealerShowroom.objImageList = pqCarsArray[1].DealerShowroom.objImageList.slice(0, 8);
                }
            }
            try {
                bindAdDivs(pqCarsArray[1].EnId);
                if (pqCarsArray[1].CrossSellCampaignList != null && pqCarsArray[1].CrossSellCampaignList.length > 0)
                    dataLayer.push({ event: 'PQ-Page-Tracking', cat: 'Cross-Sell-Unit', act: 'Cross-sell-unit-shown', lab: pqCarsArray[1].cityDetail.CityName + ' - ' + pqCarsArray[1].carDetails.ModelName + ' - ' + pqCarsArray[1].CrossSellCampaignList[0].ModelName + ' - ' + pqCarsArray[1].CrossSellCampaignList[0].DealerName + ' - ' + pqCarsArray[1].CrossSellCampaignList[0].CampaignId });
            }
            catch (e) {
                console.log('replacePQToViewModel:' + e.message);
            }

        }
        btToolTip();

        if (pageId != pqPageId.PQByNearCityClick) {
            _viewModel.selectedAreaId = pqCarsArray[0].cityDetail.AreaId;
            _viewModel.selectedAreaName = pqCarsArray[0].cityDetail.AreaName;
        }

        try {
            bindAdDivs(pqCarsArray[0].EnId);
            if (pqCarsArray[0].CrossSellCampaignList != null && pqCarsArray[0].CrossSellCampaignList.length > 0)
                dataLayer.push({ event: 'PQ-Page-Tracking', cat: 'Cross-Sell-Unit', act: 'Cross-sell-unit-shown', lab: pqCarsArray[0].cityDetail.CityName + ' - ' + pqCarsArray[0].carDetails.ModelName + ' - ' + pqCarsArray[0].CrossSellCampaignList[0].ModelName + ' - ' + pqCarsArray[0].CrossSellCampaignList[0].DealerName + ' - ' + pqCarsArray[0].CrossSellCampaignList[0].CampaignId });
        }
        catch (e) {
            console.log('replacePQToViewModel:' + e.message);
        }

        callDFP();
    } catch (err) {
        console.log('replacePQToViewModel:' + err.message);
    }
}

function setQuotationPageCTALeadFormAttribute(ctaSource, pqObj, leadClickSource, inquirySource, recoLeadSource, recoInquirySource) {
    if (typeof ctaSource != "undefined") {
        ctaSource.removeAttribute('onclick');
        ctaSource.setAttribute('emiadavailable', 'true');
        ctaSource.setAttribute('leadclicksource', leadClickSource);
        ctaSource.setAttribute('recoleadsource', recoLeadSource);
        ctaSource.setAttribute('inquirysourceid', inquirySource);
        ctaSource.setAttribute('recoinquirysource', recoInquirySource);
        ctaSource.setAttribute('data-predictionScore', pqObj.SponsoredDealer.PredictionData != null ? pqObj.SponsoredDealer.PredictionData.Score : 0);
        ctaSource.setAttribute('data-predictionLabel', pqObj.SponsoredDealer.PredictionData != null ? pqObj.SponsoredDealer.PredictionData.Label : "");

        var CarDetails =
                {
                    carMakeId: pqObj.carDetails.MakeId,
                    carMakeName: pqObj.carDetails.MakeName,
                    carMaskingName: pqObj.carDetails.MaskingName,
                    carModelName: pqObj.carDetails.ModelName,
                    carModelId: pqObj.carDetails.ModelId
                };

        var CityDetail = {
            cityId: pqObj.cityDetail.CityId,
            cityName: pqObj.cityDetail.CityName,
            areaId: pqObj.cityDetail.AreaId,
            zoneId: pqObj.cityDetail.ZoneId
        };
        ctaSource.setAttribute('data-cardetails', JSON.stringify(CarDetails));
        ctaSource.setAttribute('data-citydetails', JSON.stringify(CityDetail));
        ctaSource.setAttribute('data-versionid', pqObj.carDetails.VersionId);
        ctaSource.setAttribute('data-versionname', pqObj.carDetails.VersionName);

        ctaSource.setAttribute("data-leadpanel", pqObj.SponsoredDealer.LeadPanel);
        ctaSource.setAttribute("data-sponsordlrname", pqObj.SponsoredDealer.DealerName);
        ctaSource.setAttribute("data-campaignid", pqObj.SponsoredDealer.DealerId);
        ctaSource.setAttribute("data-showemail", pqObj.SponsoredDealer.ShowEmail);
        ctaSource.setAttribute("data-mutualleads", pqObj.SponsoredDealer.MutualLeads);
        ctaSource.setAttribute("data-dealeradminid", pqObj.SponsoredDealer.DealerAdminId);  // Identifies dealers group id
        ctaSource.setAttribute("page", "QuotationPage");
    }
}

function setAllCTALeadFormforcrosSellAttribute(ctaSource, pqObj, index, leadClickSource, inquirySource, recoLeadSource, recoInquirySource) {
    if (typeof ctaSource != "undefined") {
        ctaSource.removeAttribute('onclick');
        ctaSource.setAttribute('emiadavailable', 'true');
        ctaSource.setAttribute('leadclicksource', leadClickSource);
        ctaSource.setAttribute('recoleadsource', recoLeadSource);
        ctaSource.setAttribute('inquirysourceid', inquirySource);
        ctaSource.setAttribute('recoinquirysource', recoInquirySource);

        var CarDetails =
                {
                    carMakeId: pqObj.CrossSellCampaignList[index].MakeId,
                    carMakeName: pqObj.CrossSellCampaignList[index].MakeName,
                    carMaskingName: pqObj.CrossSellCampaignList[index].MaskingName,
                    carModelName: pqObj.CrossSellCampaignList[index].ModelName,
                    carModelId: pqObj.CrossSellCampaignList[index].ModelId,
                    hostURL: pqObj.CrossSellCampaignList[index].HostURL,
                    originalImgPath: pqObj.CrossSellCampaignList[index].OriginalImgPath
                };
        ctaSource.setAttribute('data-cardetails', JSON.stringify(CarDetails));
        ctaSource.setAttribute('data-versionid', pqObj.CrossSellCampaignList[index].VersionId);
        ctaSource.setAttribute('data-versionname', pqObj.CrossSellCampaignList[index].VersionName);

        ctaSource.setAttribute("data-cityid", pqObj.cityDetail.CityId);
        ctaSource.setAttribute("data-leadpanel", pqObj.CrossSellCampaignList[index].LeadPanel);
        ctaSource.setAttribute("data-sponsordlrname", pqObj.CrossSellCampaignList[index].DealerName);
        ctaSource.setAttribute("data-campaignid", pqObj.CrossSellCampaignList[index].CampaignId);
        ctaSource.setAttribute("data-showemail", pqObj.CrossSellCampaignList[index].ShowEmail);
        ctaSource.setAttribute("page", "QuotationPage");
    }
}


function setAllCTALeadFormAttribute(obj) {

    var sponsoredDealerAd = $('.quotation-box[pqid="' + obj.EnId + '"]').find(".btn-dealer-ad")[0];


    var crossSellCTA = $('.quotation-box[pqid="' + obj.EnId + '"]').find(".crossSellCTA");

    $.each(crossSellCTA, function (index, item) {
        setAllCTALeadFormforcrosSellAttribute(item, obj, index, 130, 123, 135, 128);
    });

    var btnOffetLink = $('.quotation-box[pqid="' + obj.EnId + '"]').find(".campaignLinkCTA ")[0];
    var btnEmiDealerQuote = $('.pq-below-container[pqid="' + obj.EnId + '"]').find("#dlp-cw-navigate")[0];
    var getOffersLink = $('.pq-below-container[pqid="' + obj.EnId + '"]').find(".dealer-assist-link")[0];

    setQuotationPageCTALeadFormAttribute(sponsoredDealerAd, obj, 100, 34, 135, 128);
    setQuotationPageCTALeadFormAttribute(btnOffetLink, obj, 101, 34, 135, 128);
    setQuotationPageCTALeadFormAttribute(btnEmiDealerQuote, obj, 104, 100, 135, 128);
    setQuotationPageCTALeadFormAttribute(getOffersLink, obj, 360, 34, 135, 128);
}

// function to9 add new price quote object to Viewmodel
function addPQToViewModel(pqCarsArray, addtoUrl) {
    try {
        for (var i = 0; i < pqCarsArray.length; i++) {
            pqCarsArray[i].EnId = decodeURIComponent(pqCarsArray[i].EnId).replace(/ /g, '+');

            if (pqCarsArray[i].DealerShowroom != null) {
                if (pqCarsArray[i].DealerShowroom.objImageList != null && pqCarsArray[i].DealerShowroom.objImageList.length > 8) {
                    pqCarsArray[i].DealerShowroom.objImageList = pqCarsArray[i].DealerShowroom.objImageList.slice(0, 8);
                }
            }
            if (pqCarsArray[i].SponsoredDealer != null && pqCarsArray[i].SponsoredDealer.DealerId != null && pqCarsArray[i].SponsoredDealer.DealerId != 0)
                saveDealerCookie(pqCarsArray[i].SponsoredDealer.DealerId, pqCarsArray[i].cityDetail.CityId.toString(), pqCarsArray[i].cityDetail.ZoneId, pqCarsArray[i].carDetails.ModelId)

            var pq = new priceQuoteVM(pqCarsArray[i]);
            _viewModel.priceQuoteCollection.push(pq);

            setAllCTALeadFormAttribute(pqCarsArray[i]);

            pq.initiateLocationPlugin(pqCarsArray[i]);
            pq.initAlternateCarsLink(pqCarsArray[i]);

            if (addtoUrl === undefined) {
                if (pqCarsArray[i].IsSponsoredCar == true)
                    addToUrl(pqCarsArray[i].EnId, undefined, undefined, "", pqCarsArray[0].EnId);
                else {
                    addToUrl(pqCarsArray[i].EnId, undefined, undefined, "", pqCarsArray[i].EnId)

                }

                if (pqCarsArray.length < 2)
                    focusLi(pqCarsArray[i].EnId.toString());
                else {
                    if (i == 1)
                        focusLi(pqCarsArray[i].EnId.toString());
                }


                if (pqCarsArray[i].SponsoredDealer != null && pqCarsArray[i].SponsoredDealer.DealerId != null && pqCarsArray[i].SponsoredDealer.DealerId != 0) {
                    dataLayer.push({ event: 'PQ-Page-Tracking', cat: "PQ-Panel", act: "PlaceholderBlockAppearance", lab: '' });
                } else {
                    dataLayer.push({ event: 'PQ-Page-Tracking', cat: "PQ-Panel", act: "PQAdAppearance", lab: '' });
                }

            }
            if (pqCarsArray[i].IsSponsoredCar == true)
                IsSponsoredCarShowed = true;

            alternateCars();
            bindAdDivs(pqCarsArray[i].EnId);

            _viewModel.selectedAreaId = pqCarsArray[i].cityDetail.AreaId;
            _viewModel.selectedAreaName = pqCarsArray[i].cityDetail.AreaName;
            $.cookie("_CustCity", pqCarsArray[i].cityDetail.CityName, { path: '/' }); //To be removed when all places migrate from city dropdown to popup

            try {
                if (pqCarsArray[i].CrossSellCampaignList != null && pqCarsArray[i].CrossSellCampaignList.length > 0)
                    dataLayer.push({ event: 'PQ-Page-Tracking', cat: 'Cross-Sell-Unit', act: 'Cross-sell-unit-shown', lab: pqCarsArray[i].cityDetail.CityName + ' - ' + pqCarsArray[i].carDetails.ModelName + ' - ' + pqCarsArray[i].CrossSellCampaignList[0].ModelName + ' - ' + pqCarsArray[i].CrossSellCampaignList[0].DealerName + ' - ' + pqCarsArray[i].CrossSellCampaignList[0].CampaignId });
            }
            catch (e) {
                console.log('addPQToViewModel:' + e.message);
            }
        }
        var tempwidth = 0;
        callDFP();
        btToolTip();
        $.cookie("_PQVersionId", pqCarsArray[0].carDetails.VersionId, { path: '/' });
    } catch (err) {
        console.log('addPQToViewModel:' + err.message);
    }
}

$('div.quo-car-place').live('click', function () {
    dataLayer.push({ event: 'PQ-Page-Tracking', cat: "PQ-Panel", act: "ModelImageClick", lab: '' });
});

$('div#modelLink').live('click', function () {
    dataLayer.push({ event: 'PQ-Page-Tracking', cat: "PQ-Panel", act: "ModelVersionLinkClick", lab: '' });
});

$('table.tblDefault .btnOfferLink').live('click', function () {
    dataLayer.push({ event: 'PQ-Page-Tracking', cat: "PQ-Panel", act: "PQAdLinkClick", lab: '' });
});


function replacePQInObservableArray() {                               // function for replacing the model in observable array
    _viewModel.priceQuoteCollection.replace(_viewModel.priceQuoteCollection()[indexOfCurrentPQ], pqCarsArray[i]);
    _viewModel.isDropDownChanged = 0;
}

function showHideContainers(pqId) {      // function for hiding Container
    $('.quotation-box').addClass('hide');
    hideShowCurrentTabPrice(pqId);
    $('.quotation-box[pqid="' + pqId + '"]').removeClass('hide');
    $('.pq-below-container').addClass('hide');
    $('.pq-below-container[pqid="' + pqId + '"]').removeClass('hide');
    $('.tabs li').removeClass("quo-active");
    $('.tabs li[pqid="' + pqId + '"]').addClass("quo-active");
    $('#leaderBoard div.adunit').addClass('hide');
    $('#leaderBoard div[lbSlotId="' + pqId + '"]').removeClass('hide');
}

function getPersistentDealerCampaign(currModelId, currCityId, currZoneId) {  // function for persistent dealer campaign
    try {
        if (typeof (currZoneId) === 'undefined')
            currZoneId = "";
        var campaignDetails = isCookieExists('_dealerCityModel') ? $.cookie('_dealerCityModel') : '';
        if (campaignDetails && campaignDetails != '') {
            var arrOfDealerInfo = campaignDetails.split('!');
            for (var i = 0; i < arrOfDealerInfo.length - 1; i++) {
                var cookieCity = arrOfDealerInfo[i].split('~')[1];
                var cookieModel = arrOfDealerInfo[i].split('~')[2];
                var cookieZone = arrOfDealerInfo[i].split('~')[3];
                if (currCityId == cookieCity && currModelId == cookieModel && currZoneId == cookieZone)
                    return parseInt(arrOfDealerInfo[i].split('~')[0]);
            }
        }
        return 0;
    }
    catch (e) {
        console.log("getPersisitentCookie:" + e.message);
    }
}

function formatNumeric(inputPrice) {
    var inputPrice = inputPrice.toString();
    var formattedPrice = "";
    var breakPoint = 3;
    for (var i = inputPrice.length - 1; i >= 0; i--) {

        formattedPrice = inputPrice.charAt(i) + formattedPrice;
        if ((inputPrice.length - i) == breakPoint && inputPrice.length > breakPoint) {
            formattedPrice = "," + formattedPrice;
            breakPoint = breakPoint + 2;
        }
    }
    return formattedPrice;
}

function validatePQRequest(pqRequest) {
    if (pqRequest.CarVersionId == "")
        alert("invalid request");
}

function formatPrice(price, percentage, additiveAmount) {
    var amount = parseInt(price * percentage);
    if (additiveAmount != undefined && Number(additiveAmount) > 0) {
        amount = amount + additiveAmount;
    }
    return FormatFullPrice(amount.toString(), amount.toString()).replace("lakhs", "L").replace("crores", "Cr");
}

function calculateEMI(tenure, interestRate, loanAmount) {  // function to calculate EMI
    var emi;
    if (interestRate == 0) {
        emi = parseInt(loanAmount / tenure);
    } else {
        interestRate = (interestRate * 0.01) / 12;
        emi = (loanAmount * interestRate * Math.pow((1 + interestRate), tenure)) / (Math.pow((1 + interestRate), tenure) - 1);
    }
    return Math.round(emi);
}

function assignSliderLabels(inputElement, exShowroomPrice) {        // function to assign dealer labels

    var partitionVariable = 0;

    $(inputElement).next().find('li > div').each(function () {
        if (partitionVariable == 0) {
            $(this).text("0");
        } else if (exShowroomPrice == 0) {
            $(this).text("");
        } else {
            $(this).text(Math.round(((exShowroomPrice * partitionVariable) / 100000) * 10) / 10 + "L");
        }
        partitionVariable = partitionVariable + 0.25;
    });
}

function SubmitLead(clickedButton, emailSubmitted) {
    // function for submitting Lead
    var currentPQObject = getCurrentPQObject();
    var comments = "";
    var element = $(clickedButton);
    var buttonDiv = element.closest(".cw-tabs-panel").parent().parent().parent();
    onBlurValidation(buttonDiv);
    var dealerLeadObject = new Object();
    dealerLeadObject.Name = $.trim(buttonDiv.find(".customername").val());
    dealerLeadObject.Email = $.trim(buttonDiv.find(".customeremail").val());
    dealerLeadObject.Mobile = $.trim(buttonDiv.find(".customermobile").val());
    dealerLeadObject.MakeId = currentPQObject.carDetails.MakeId;
    dealerLeadObject.modelId = currentPQObject.carDetails.ModelId;
    dealerLeadObject.modelName = currentPQObject.carDetails.ModelName;
    dealerLeadObject.VersionId = currentPQObject.carDetails.VersionId;
    dealerLeadObject.InquirySourceId = 98;
    dealerLeadObject.LeadClickSource = 115;
    dealerLeadObject.DealerId = currentPQObject.DealerShowroom.objDealerDetails.CampaignId;
    dealerLeadObject.CityId = currentPQObject.cityDetail.CityId;
    dealerLeadObject.cwtccat = element.attr("data-df-cwtccat"); //df stands for deffered
    dealerLeadObject.cwtcact = element.attr("data-df-cwtcact");
    dealerLeadObject.cwtclbl = element.attr("data-df-cwtclbl");
    dealerLeadObject.score = currentPQObject.SponsoredDealer.PredictionData != null ? currentPQObject.SponsoredDealer.PredictionData.Score : 0;
    dealerLeadObject.label = currentPQObject.SponsoredDealer.PredictionData != null ? currentPQObject.SponsoredDealer.PredictionData.Label : '';

    if (isValid(buttonDiv)) {
        $(buttonDiv).find('.tyHeading').empty();
        setCookies(buttonDiv);
        SendNewCarRequestDealer(dealerLeadObject, buttonDiv, emailSubmitted);
    }
}

function processRequest(btn) {

    var data = getCurrentPQObject();

    var tabNo = -1;

    if (tabNo == 4) {
        GetPQIdAndCallBack(activeCarDiv, SetCookieAndRedirect(activeCarDiv));
    }
    else {
        inquirySourceId = "34";
        SetCookieAndRedirect(data);
    }
}

function SetCookieAndRedirect(data) {      // function to set cookie and redirect to offers page
    var pqId = data.PQId;
    var makeName = data.carDetails.MakeName;
    var modelId = data.carDetails.ModelId;
    var modelName = data.carDetails.ModelName;
    var cityId = data.cityDetail.CityId;
    var cityName = data.cityDetail.CityName;
    var zoneId = data.cityDetail.ZoneId;
    var zoneName = data.cityDetail.ZoneName;
    var versionid = data.carDetails.VersionId;
    var versionName = data.carDetails.VersionName;
    var carImage = data.carDetails.LargePic;
    var dealerId = data.Offers.DealerId;
    var dealerName = data.Offers.DealerName;
    var offerId = data.Offers.OfferId;
    var availabilityCount = data.Offers.AvailabilityCount;
    var offerDescription = data.Offers.Description;
    var offerTAndC = data.Offers.TermsAndCondition;
    var carName = makeName + " " + modelName + " " + versionName;

    $.cookie('_DealerName', dealerName, { path: '/' });
    $.cookie('_OfferAvailCount', availabilityCount, { path: '/' });

    $.cookie('_PQOfferTAndC', offerTAndC, { path: '/' });
    $.cookie('_PQOfferDesc', offerDescription, { path: '/' });
    $.cookie('_PQCarName', carName, { path: '/' })
    $.cookie('_PQCarImg', carImage, { path: '/' });
    $.cookie('_PQMOdelName', modelName, { path: '/' });
    $.cookie('_PQId', pqId, { path: '/' });
    $.cookie('_DealerId', dealerId, { path: '/' });
    $.cookie('_OfferId', offerId, { path: '/' });
    $.cookie('_PQMOdelId', modelId, { path: '/' });
    document.cookie = '_CustCityId=' + cityId + '; expires = ' + permanentCookieTime() + "; domain=" + defaultCookieDomain + '; path =/';
    $.cookie('_PQVersionId', versionid, { path: '/' });
    document.cookie = '_PQZoneId=' + zoneId + '; expires = ' + permanentCookieTime() + "; domain=" + defaultCookieDomain + '; path =/';
    if (zoneId != null && zoneId != "") {
        document.cookie = '_CustCity=' + zoneName + '; expires = ' + permanentCookieTime() + "; domain=" + defaultCookieDomain + '; path =/';
    } else {
        document.cookie = '_CustCity=' + cityName + '; expires = ' + permanentCookieTime() + "; domain=" + defaultCookieDomain + '; path =/';
    }

    if (modelName && versionName && cityName && zoneName && offerId) {
        dataLayer.push({ event: 'CWOffersBehaviour', cat: 'CWOffers', act: 'GenerateCouponBtnDesktopPQ', lab: modelName + '-' + versionName + '-' + cityName + '-' + zoneName + '-' + offerId });
    }

    window.location.href = '/new/offercustomerinfo.aspx';
}

function getPQObjectByPQId(PQId) {            // function to get PQ Object By Id
    var vm = ko.dataFor(document.getElementById("pqCarContainer"));
    return findInArray(PQId, vm.priceQuoteCollection(), "carPriceQuote().EnId");
}

function getCurrentPQObjIndex() {             //function to get Current Object Index
    var vm = ko.dataFor(document.getElementById("pqCarContainer"));
    var currentPQId = $("div.quotation-box:visible").attr("pqid");
    return findInArray(currentPQId, vm.priceQuoteCollection(), "carPriceQuote().EnId");
}

function getCurrentPQObject() {
    var currentIndex = getCurrentPQObjIndex();
    return getPQObjectAtIndex(currentIndex);
}

function getPQObjectAtIndex(index) {
    var vm = ko.dataFor(document.getElementById("pqCarContainer"));
    return vm.priceQuoteCollection()[index].carPriceQuote();
}

function findInArray(value, array, parameter) {
    var item;
    for (var i = 0; i < array.length; i++) {
        item = array[i];
        if (value == eval('(item.' + parameter + ')')) return i;
    }
    return -1;
}
function IsValidate() {
    var retVal = true;

    if (typeof (modelIdFromAddPQ) == "undefined" || (typeof (modelIdFromAddPQ) != "undefined" && Number(modelIdFromAddPQ) < 1) || ($.trim($("#addPQAutocomplete").val()) == "" || $.trim($("#addPQAutocomplete").val()) == "Type to select car name, e.g. Renault Duster")) {
        retVal = false;
        $("#Make").find('.cityErrIcon').show();
        $("#Make").find('.cityErrorMsg').text("Please Select Model");
        $("#Make").find('.cityErrorMsg').show();
    }
    else {
        if (!locationPluginObj.validateLocation()) {
            retVal = false;
        }
    }

    return retVal;
}

function refreshCarousel() {
    try {
        tabsCarousel.jcarousel('reload');
    } catch (e) {
        bindCarousel();
        tabsCarousel.jcarousel('reload');
        console.log('refreshCarousel:' + e.message)
    }
    countliLength();
}

///*
function countliLength() {            // function to set ccss based on the number of tabs
    liCount = $("#pqTabs li").length;
    if (liCount == 1) {
        $(".stPrev, .stNext").addClass("hide");
        $("#pq-jcarousel").css({ 'width': '291px' });
        $(".quotation-tabs li").css({ 'width': '291px' });
        $(".quotation-tabs ul").css({ 'width': '291px', 'margin': '0', 'border-right': '0' });
        $(".add-text").removeClass("hide");
        $(".ui-icon-close").hide();
    }
    if (liCount == 2) {
        $(".stPrev, .stNext").addClass("hide");
        $("#pq-jcarousel").css({ 'width': '590px' });
        $(".quotation-tabs li").css({ 'width': '291px' });
        $(".quotation-tabs ul").css({ 'width': '590px', 'margin': '0' });
        $(".add-text").removeClass("hide");
    }
    if (liCount == 3) {
        $(".stPrev, .stNext").addClass("hide");
        $("#pq-jcarousel").css({ 'width': '882px' });
        $(".quotation-tabs li").css({ 'width': '291px' });
        $(".quotation-tabs ul").css({ 'width': '882px', 'margin': '0' });
        $(".add-text").addClass("hide");
        $(".step4-box").css({ 'right': '78px' });
    }
    if (liCount > 3) {
        $(".stPrev, .stNext").removeClass("hide").parent().show();
        //$(".quotation-tabs ul").css({ 'width': '813px', 'margin': '0 40px' });
        $("#pq-jcarousel").css({ 'width': '808px' });
        $(".quotation-tabs li").css({ 'width': '270px' });
        $(".quotation-tabs ul").css({ 'width': '813px' });
        $(".add-text").addClass("hide");
        $(".tabs").css({ 'border-top': '1px solid #e2e2e2' });
        $(".quotation-tabs").css({ 'border-top': '0' });
        $(".stNav").css({ 'top': '0px' });
        $(".step4-box").css({ 'right': '78px' });
    }
}

var nextPQId;

function ShowNextActiveTab(tabPQId) {        // function for showing Next active tab
    nextPQId = getNextPQId(tabPQId);
    removeUrl(tabPQId, nextPQId);
    if (!($('.quotation-box[pqid="' + tabPQId.toString() + '"]').hasClass('hide'))) {
        hideShowCurrentTabPrice(nextPQId);
        cityVersionCookieChange(nextPQId);
        showHideContainers(nextPQId);

    }
}

//Hide the close button if only one tab exists
function HideShowCloseButton() {
    if (_viewModel.priceQuoteCollection().length <= 1) {
        $(' .close-btn').hide();
    } else {
        $(' .close-btn').show();
    }
}

//get next PQId from the pqIds list
function getNextPQId(pqId) {
    var priceQuoteCollection = _viewModel.priceQuoteCollection();
    var noOfPQIds = priceQuoteCollection.length;
    for (var i = 0 ; i < noOfPQIds; i++) {
        if (pqId == priceQuoteCollection[i].carPriceQuote().EnId) {
            if (i < noOfPQIds - 1)    // to identify whether last tab was closed
                return priceQuoteCollection[i + 1].carPriceQuote().EnId;
            else
                return priceQuoteCollection[i - 1].carPriceQuote().EnId;
        }
    }
}

var showroomPostRender = function () {

    // To get the value from cookie if it is not null
    if ($.cookie('_CustomerName') != null && $.cookie('_CustEmail') != null && $.cookie('_CustMobile') != null) {
        $('.customername').val($.cookie('_CustomerName'));
        $('.customeremail').val($.cookie('_CustEmail'));
        $('.customermobile').val($.cookie('_CustMobile'));
    }


    $(".cw-tabs-panel .cw-tabs li").click(function () {     // function for dealer locator in PQ Page
        var category = "price_quote";
        tabAnimation.$thisLI = $(this);
        panel = $(this).closest(".cw-tabs-panel");
        tabAnimation.panel = $(this).closest(".cw-tabs-panel");
        var pqId = this.getAttribute('pqid');
        panel.find(".cw-tabs li").removeClass("active");
        $(this).addClass("active").append('<span class="dealer-sprite dlparrow"></span>');

        var panelId = $(this).attr("data-tabs");
        panel.find(".cw-tabs-data").hide();
        panel.find("#" + panelId).show();

        var dealerName = panel.parent().parent().find("#divDealerName").text();
        action = $(this).attr('data-tabs') + "_tab";
        dataLayer.push({ event: 'locate_dealer_section', cat: category, act: action, lab: dealerName });

        var addActiveDivId = panel.parent().parent().parent().attr('pqid');

        panel.find('#gallery a').each(function () {
            $(this).attr('rel', addActiveDivId);
        });
        // if map in not intialized
        if (panelId == "contact_details" && panel.find('#map-canvas').html().length < 1)
            GoogleMapsLoaded();

        //for colorbox pop up window code starts here
        try {
            panel.find('a[rel="' + addActiveDivId + '"]').colorbox({ width: "600px", height: "450px" }); // end here
        }
        catch (err) {
            console.log('showroomPostRender:' + err.message);
        }

        tabAnimation.liClickTabAnimation();
    });

    $('.cboxElement').click(function () {
        var activeDiv = $(this).closest(".dlp-panel-group").parent().parent().parent().parent().parent();
        var dealerName = activeDiv.find("#divDealerName").text();
        dataLayer.push({ event: 'locate_dealer_section', cat: "price_quote", act: 'image_popup', lab: dealerName });
    });

    $('#cboxNext').click(function () {
        var activeDiv = $(this).closest(".dlp-panel-group").parent().parent().parent().parent().parent();
        var dealerName = activeDiv.find("#divDealerName").text();
        dataLayer.push({ event: 'locate_dealer_section', cat: "price_quote", act: 'image_popup_next', lab: dealerName });
    });

    $('#cboxPrevious').click(function () {
        var activeDiv = $(this).closest(".dlp-panel-group").parent().parent().parent().parent().parent();
        var dealerName = activeDiv.find("#divDealerName").text();
        dataLayer.push({ event: 'locate_dealer_section', cat: "price_quote", act: 'image_popup_previous', lab: dealerName });
    });
}

function GoogleMapsLoaded() {
    Common.utils.loadGoogleApi(bindMap, panel);
}


function bindMap(container) {      // function for binding Map

    var latitude = $(container).find(".latitude").text();
    var longitude = $(container).find(".longitude").text();

    if (isLatLongValid(latitude, longitude)) {
        try {
            $(container).find(".map-hide").show();
            $(container).find(".map-text").show();
            var elems = $(container).find("#map-canvas")[0];
            var dealerNames = $(container).find(".dealername").text();
            var dealerAddress = $(container).find(".address").text();
            var dealerMobileNumbers = $(container).find(".mobileno").text();

            var myLatlng = new google.maps.LatLng(latitude, longitude);

            var mapOptions = {
                zoom: 14,
                center: myLatlng,
                mapTypeId: google.maps.MapTypeId.ROADMAP
            };

            var map = new google.maps.Map(elems, mapOptions);
            var contentString = '<div id="content" style="width:240px;">' +
             '<strong>' + dealerNames + '</strong>' + '<br/>' + dealerAddress + '<br/>' + '<strong>' + dealerMobileNumbers + '</strong>' + '<br/>'
             + '<a onclick="javascript:onViewMapTrack(this)" id="googleMap" style="text-decorations:none; color:inherit;" target="_blank" >View on Google Map' + '</a>'
            '</div>';

            var infowindow = new google.maps.InfoWindow({
                content: contentString
            });

            var marker = new google.maps.Marker({
                position: myLatlng,
                map: map,

            });
            marker.setMap(map);

            google.maps.event.addListener(marker, 'click', function () {
                infowindow.open(map, marker);
            });
        }
        catch (e) {
            console.debug("bindMap:CustomEror" + e);
        }
    }
    else {
        $(container).find(".map-hide").hide();
    }
}

function toggleThisCheckbox(checkboxli) {
    $(checkboxli).toggleClass('checked');
}

function newPqByCrossSell(data) {
    var pqInput = {};
    pqInput.modelId = $(data).attr('modelId');
    pqInput.versionId = $(data).attr('versionId');
    pqInput.pageId = pqPageId.PQByCrossSell;
    pqInput.location = PriceBreakUp.Quotation.getPQLocation();
    addNewCarPQ(pqInput);
}

function addNewCarPQ(pqInput) {
    PriceBreakUp.Quotation.setPQCityCookies(pqInput);
    _viewModel.selectedAreaId = pqInput.location.areaId;
    _viewModel.selectedAreaName = pqInput.location.areaName;
    var pqRequest = getPQInputes(pqInput.pageId);
    $("html, body").animate({ scrollTop: 0 }, "slow");
    getNewPQ(pqRequest);

}

function FormatSpecial(url) {
    reg = /[^/\-0-9a-zA-Z\s]*/g; // everything except a-z, 0-9, / and - 
    url = url.replace(reg, '');
    var formattedUrl = url.toLowerCase().replace(/ /g, "").replace(/-/g, "").replace("/", "");
    return formattedUrl;
}

function bindCarOnRefresh() {            // function to bind all the object on Page refresh
    try {
        var pqIdsList = getParameterByName('pqid');
        var activeTab = getParameterByName('t');
        var pqIds = pqIdsList.split(',');

        for (var i = 0; i < pqIds.length; i++) {
            if (pqIds[i] == activeTab)
                isActivePresent = 1;
        }
        if (isActivePresent == 0) {
            activeTab = pqIds[0];

        }
        else {
            isActivePresent = 0;
        }
    } catch (err) {
        console.log('bindCarOnRefresh():' + err.message);
    }
    $.ajax({
        type: 'GET',
        url: '/v1/api/pq/?pqids=' + pqIdsList + '&sourceid=' + 1,
        dataType: 'Json',
        contentType: "application/x-www-form-urlencoded",
        success: function (response) {
            try {
                if (response == null || response.length != pqIds.length) {
                    window.location = '/new/prices.aspx';
                }

                ko.applyBindings(_viewModel, document.getElementById('pq-container'));
                addPQToViewModel(response, 1);

                if (activeTab == "")
                    activeTab = _viewModel.priceQuoteCollection()[0].carPriceQuote().EnId;
                cityVersionCookieChange(activeTab);
                showHideContainers(activeTab);
                HideShowCloseButton();
                if (!isCookieExists('_CustCity')) {
                    $.each(response, function (index, val) {
                        if (response[index].EnId == activeTab) {
                            PQ.cookie.setCustCityCookie(response[index].cityDetail.ZoneId > 0 ? response[index].cityDetail.ZoneName : response[index].cityDetail.CityName, response[index].cityDetail.CityId);
                            _viewModel.selectedAreaId = response[index].cityDetail.AreaId;
                        }
                    });
                }
                initFunctionsOnPqResponse(response);
                focusLi(activeTab);
                Quotation.registerEvents();
                btObj.registerEventsClass();
                fillNearbyCities(response, true);
            } catch (err) {
                console.log("GetById:" + err.message);
            }

        }
    });
}

function addToUrl(pqId, title, indexOfCurrentPQ, previousPQId, activeid) {    // function to add new PQ Id to the URL
    try {
        pqId = decodeURIComponent(pqId);
        title = getTitle(title);
        var pqIdsInQs = Common.utils.getValueFromQS('pqid');

        if (pqIdsInQs == "") {
            hideShowCurrentTabPrice(pqId);
            activeid = pqId;
            History.replaceState({ state: pqId }, title, currentURI + '?pqid=' + pqId + '&t=' + activeid);
        }
        else {
            if (indexOfCurrentPQ != undefined) {                               // code will execute only when feature car would come on version change
                var pqidArr = getParameterByName('pqid').split(',');
                pqidArr.splice(indexOfCurrentPQ + 1, 0, pqId);
                var pqids = pqidArr.join();
                pqId = previousPQId;
                activeid = pqId;
            }
            else {

                hideShowCurrentTabPrice(pqId);
                var pqids = getParameterByName('pqid') + ',' + pqId;
            }

            History.replaceState({ state: pqId }, title, currentURI + '?pqid=' + pqids + '&t=' + activeid);
        }
    }
    catch (err) {
        console.log("addToUrl:" + err.message);
    }
}

function updateUrl(old_pqId, new_pqId, title) {  // function to update URL
    title = getTitle(title);
    var queryStr = location.search.replace(new RegExp(old_pqId.replace(new RegExp("\\+", 'g'), '\\+'), 'g'), new_pqId);
    History.replaceState({ state: new_pqId }, title, currentURI + queryStr);
}

function removeUrl(pqId, toShow_pqId, title) {
    title = getTitle(title);
    var pqids = getParameterByName('pqid').replace(decodeURIComponent(pqId).replace(/ /g, '+'), '').replace(/(^,)|(,$)/g, "").replace(',,', ',');
    History.replaceState({ state: toShow_pqId }, title, currentURI + '?pqid=' + pqids + '&t=' + toShow_pqId);
}

function UpdateActivetab(activ_pqId, title) {
    title = getTitle(title);
    var pqids = getParameterByName('pqid');
    tabAnimation.tabHrInit();
    History.replaceState({ state: activ_pqId }, title, currentURI + '?pqid=' + pqids + '&t=' + activ_pqId);
}

function getTitle(title) {
    return title = (typeof title === 'undefined') ? 'Instant Free New Car Price Quote-CarWale' : title;
}

function getParameterByName(name, str) {
    str = typeof str === 'undefined' ? location.search : str;
    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(str);
    return results === null ? "" : results[1];
}

function focusLi(pqId) {
    var li = tabsCarousel.find($('li[pqid="' + pqId.toString() + '"]'));
    tabsCarousel.jcarousel('scroll', li);
}

function bindCarousel() {
    // pqTabs carousel controls starts here
    tabsCarousel = $('#pq-jcarousel').jcarousel({
        scroll: 1,
        auto: 0,
        animation: 800,
        // wrap: "circular",
        initCallback: null, buttonNextHTML: null, buttonPrevHTML: null
    });
    carouselData = $('#pq-jcarousel').data('jcarousel');

    alternateCarousel = $('.alternate-cars-carousel .jcarousel').jcarousel({
        scroll: 1,
        auto: 0,
        animation: 800,
        // wrap: "circular",
        initCallback: null, buttonNextHTML: null, buttonPrevHTML: null
    });
    carouselData = $('.alternate-cars-carousel .jcarousel').data('jcarousel');
}
function redirectToDealerpage(e) {
    var activeCarDiv = $('.quo-active').attr('id');
    var makeName = $(e).attr('makename');
    var cityId = $(e).attr('cityId');
    var cityName = $(e).attr('cityname');
    window.location.href = '/' + FormatSpecial(makeName) + '-dealer-showrooms/' + FormatSpecial(cityName) + '-' + cityId;
}
function FormatFullPrice(minPrice, maxPrice) {
    if (minPrice < 100000) {
        return formatNumeric(minPrice);
    }
    else {
        var priceRange = "", tempMinPrice = "", tempMaxPrice = "";

        if (minPrice.indexOf(",") != -1) {
            minPrice = minPrice.Replace(",", "");
        }

        if (maxPrice.indexOf(",") != -1) {
            maxPrice = maxPrice.Replace(",", "");
        }
        if (parseFloat(minPrice) == parseFloat(maxPrice)) {
            if (minPrice.length >= 8) //when price in crore.
            {
                tempMinPrice = ((parseFloat(minPrice) / 10000000)).toFixed(2);
                priceRange = tempMinPrice.toString() + " Crores";
            } else if ((minPrice.length >= 6) && (minPrice.length < 8)) //when price in lakhs.
            {
                tempMinPrice = ((parseFloat(minPrice) / 100000)).toFixed(2);
                priceRange = tempMinPrice.toString() + " Lakhs";
            } else //when price in thousands.
            {
                priceRange = minPrice.toString();
            }
        } else {
            if (minPrice.length >= 8)  //when both min and max prices are in crores
            {
                tempMinPrice = ((parseFloat(minPrice) / 10000000)).toFixed(2);
                tempMaxPrice = ((parseFloat(maxPrice) / 10000000)).toFixed(2);

                priceRange = tempMinPrice.toString() + " - " + tempMaxPrice.toString() + " Crores";
            } else if ((minPrice.length < 8) && (minPrice.length >= 6) && (maxPrice.length >= 8)) //when min price in lakhs and max price is in crores
            {
                tempMinPrice = ((parseFloat(minPrice) / 100000)).toFixed(2);
                tempMaxPrice = ((parseFloat(maxPrice) / 10000000)).toFixed(2);

                priceRange = tempMinPrice.toString() + " Lakhs - " + tempMaxPrice.toString() + " Crores";
            } else if ((minPrice.length >= 6) && (maxPrice.length < 8) && (minPrice.length < 8)) //when min ans max prices are in lakhs
            {
                tempMinPrice = ((parseFloat(minPrice) / 100000)).toFixed(2);
                tempMaxPrice = ((parseFloat(maxPrice) / 100000)).toFixed(2);

                priceRange = tempMinPrice.toString() + " - " + tempMaxPrice.toString() + " Lakhs";
            } else //when min and max prices are in thousands
            {
                priceRange = minPrice.toString() + "-" + maxPrice.toString();
            }
        }

        return priceRange.replace(/.00/g, '');
    }
}


function discountedMoney(insuranceAmount, Discount) { // function to determine discounted money for Bharti AXA
    var discounted = (Discount / 100) * insuranceAmount;
    return discounted;
}

/***************************** INLINE LEAD FORM START*************************/
// function to initialize lead form inside template
function initInlineLeadForm(PQList) {
    //assign and extra unique class to PQ Ad div
    var currentFormDiv;
    for (var pqObject in PQList) {
        try {
            if (PQList[pqObject].SponsoredDealer != null) {
                if (PQList[pqObject].SponsoredDealer.TemplateHtml != null) {
                    var templateHtml = PQList[pqObject].SponsoredDealer.TemplateHtml;
                    var currentPQId = PQList[pqObject].EnId;
                    if (templateHtml.indexOf('pq-template-form') > 0) {
                        var currentQuotationBox;
                        $('.quotation-box').each(function () {
                            if ($(this).attr('pqid') == currentPQId) {
                                currentQuotationBox = this;
                            }
                        });
                        if (typeof currentQuotationBox !== 'undefined') {
                            $(currentQuotationBox).find('.pq-template-form').addClass('pq-template-form-' + currentPQId);
                            currentFormDiv = '.pq-template-form-' + currentPQId;
                        }
                    }
                }
            }
        }
        catch (err) {
            console.log('initInlineLeadForm:' + err.message);
        }
        $('.pq-template-form').find('#custMobile').attr('placeholder', 'Mobile Number (10 digits)');
        $('.pq-template-form').find('#custName').attr('placeholder', 'Name');
        dataLayer.push({ event: 'DealerLeadPopUpBehaviour', cat: 'OpenTemplateForm', act: 'OpenTemplateShown' });
    }
}

// function to submit lead form inside template
function SubmitInlineLeadForm(btn) {


    var currentPQId = $(btn).closest('.quotation-box').attr('pqid');;
    var preDeterminedObject = new Object();
    var currentPQObject = getCurrentPQObject();

    preDeterminedObject.LeadClickSource = currentPQObject.IsSponsoredCar == true ? "127" : "126";
    preDeterminedObject.inquirySourceId = currentPQObject.IsSponsoredCar == true ? "40" : "34";
    preDeterminedObject.AdType = "OpenTemplateForm";
    preDeterminedObject.GACat = "OpenTemplateForm";
    preDeterminedObject.Caption = "Get Offers on " + currentPQObject.carDetails.MakeName + " " + currentPQObject.carDetails.ModelName;

    popupData = getDealerObject(currentPQObject, preDeterminedObject);
    popupData.PopupID = '.pq-template-form-' + currentPQId;

    processPQLead(true);
}

// function to initialize lead form inside crossSellCampaign
function initInlineCrossSellLeadForm(PQList) {
    //assign and extra unique class to PQ Ad div
    var currentFormDiv;
    for (var pqObject in PQList) {
        try {
            var crossSellList = new Object();
            if (PQList[pqObject].CrossSellCampaignList != null) {
                crossSellList = PQList[pqObject].CrossSellCampaignList
            }
            if (crossSellList.length > 0) {
                for (var i = 0; i < crossSellList.length; i++) {
                    if (crossSellList[i].TemplateHtml != null) {
                        var templateHtml = crossSellList[i].TemplateHtml;
                        var currentPQId = PQList[pqObject].EnId;
                        if (templateHtml.indexOf('pq-template-form') > 0) {
                            var currentQuotationBox;
                            $('.pq-below-container').each(function () {
                                if ($(this).attr('pqid') == currentPQId) {
                                    currentQuotationBox = this;
                                }
                            });
                            if (typeof currentQuotationBox !== 'undefined') {
                                $(currentQuotationBox).find('.pq-template-form').addClass('pq-template-form-' + currentPQId);
                                currentFormDiv = '.pq-template-form-' + currentPQId;
                            }

                        }
                    }
                }
            }
        }
        catch (err) {
            console.log('initInlineCrossSellLeadForm:' + err.message);
        }
        $('.pq-template-form').find('#custMobile').attr('placeholder', 'Mobile Number (10 digits)');
        $('.pq-template-form').find('#custName').attr('placeholder', 'Name');
        dataLayer.push({ event: 'DealerLeadPopUpBehaviour', cat: 'OpenTemplateForm', act: 'OpenTemplateShown' });
    }
}

//function to initialize lead form inside crossSellCampaign
function InitCrossSellLeadForm(btn, json) {
    var currentPQId = $(btn).closest('.quotation-box').attr('PQId');
    var preDeterminedObject = new Object();
    var currentPQObject = getCurrentPQObject();

    for (var i = 0; i < json.LeadSource.length; i++) {
        if (json.LeadSource[i].LeadClickSourceDesc == "Button") {
            preDeterminedObject.LeadClickSource = json.LeadSource[i].LeadClickSourceId;
            preDeterminedObject.inquirySourceId = json.LeadSource[i].InquirySourceId;
        }
        if (json.LeadSource[i].LeadClickSourceDesc == "Recommendation") {
            RECOMMENDATIONLEADCLICKSOURCE = json.LeadSource[i].LeadClickSourceId;
            RECOMMENDATIONINQUIRYSOURCE = json.LeadSource[i].InquirySourceId;
        }
    }
    preDeterminedObject.AdType = "popupbuttonclick";
    preDeterminedObject.GACat = "Cross-Sell-Unit";
    preDeterminedObject.Caption = "Get Offers on " + currentPQObject.carDetails.MakeName + " " + currentPQObject.carDetails.ModelName;

    var data = getDealerObjectForCrossSell(currentPQObject, preDeterminedObject, json);
    data.cwtccat = "QuotationPage";
    data.cwtcact = "CrossSellLeadSubmit";
    data.cwtclbl = 'make:' + data.makeName + '|model:' + data.modelName + '|version:' + data.versionName + '|city:' + currentPQObject.cityDetail.CityName + '|CrossSellMake:' + currentPQObject.carDetails.MakeName + '|CrossSellModel:' + currentPQObject.carDetails.ModelName + '|CrossSellVersion:' + currentPQObject.carDetails.VersionName;

    $('.get-offer-icon-group').show();
    data.isSponsoredCar = true;

    cwTracking.trackCustomData(data.cwtccat, "CrossSellClick", data.cwtclbl, false);
}



/***************************** INLINE LEAD FORM- END *************************/
function showCoachmarks() {
    $(".coachmark-box.step1-box").fadeIn(500);
    Location.globalSearch.setCoachMarkCookie();
    $('#stepOnebtn').live('click', function () {
        $('.coachmark-box.step1-box').hide();
        $('.coachmark-box.step2-box').show();
    });
    $('#stepTwobtn').live('click', function () {
        $('.coachmark-box.step2-box').hide();
        $('.coachmark-box.step3-box').show();
        if (!$('.step3-box').is(':visible')) {
            $('.coachmark-box.step4-box').show();
        }
    });
    $('#stepThreebtn').live('click', function () {
        $('.coachmark-box.step3-box').hide();
        $('.coachmark-box.step4-box').show();
    });
    $('#stepFourbtn').live('click', function () {
        $('.coachmark-box.step4-box').hide();
    });
}

function hideCoachmarks() {
    $('.coachmark-box').hide();
}



function hideCoachmarksOnElementClick() {
    //hide the coachmarks on click on these active elements
    $('.close-btn').live('click', function () { hideCoachmarks(); Location.globalSearch.setCoachMarkCookie(); });
    $('#pagetop').find('select').live('change', function () { hideCoachmarks(); Location.globalSearch.setCoachMarkCookie(); });
    $('#pagetop').find('select').live('click', function () { hideCoachmarks(); Location.globalSearch.setCoachMarkCookie(); });
    $('.zone-tabs').find('li').live('click', function () { hideCoachmarks(); Location.globalSearch.setCoachMarkCookie(); });
    $('#addNewPQ').live('click', function () { hideCoachmarks(); Location.globalSearch.setCoachMarkCookie(); });
    $('#btnDealerOffer').live('click', function () { hideCoachmarks(); Location.globalSearch.setCoachMarkCookie(); });
    $('.btnOfferLink').live('click', function () { hideCoachmarks(); Location.globalSearch.setCoachMarkCookie(); });
}

function AddUserHistoryToCookie(response) {
    if (response.length > 1) {
        for (var responseItem = 0; responseItem < response.length; responseItem++) {
            if (response[responseItem].IsSponsoredCar == false)
                userHistory.trackUserHistory([response[responseItem].carDetails.ModelId]);
        }
    } else {
        userHistory.trackUserHistory([response[0].carDetails.ModelId]);
    }
}

function lnkEmiBtnClick(e) {
    dataLayer.push({ event: 'PQ-Page-Tracking', cat: "PQ_Desktop_PlaceholderAd", act: "Button Clicked", lab: e.carPriceQuote().carDetails.MakeName + ',' + e.carPriceQuote().carDetails.ModelName + ',' + e.carPriceQuote().cityDetail.CityName });
    PQ.advertisement.BhartiAxa.openPopUp();
}

function lnkEmiQuotesTopBanksClick(e) {
    dataLayer.push({ event: 'PQ-Page-Tracking', cat: "Finance_Desktop", act: "BB_PQpage_EMIbox_Finance_Clicked", lab: e.carPriceQuote().carDetails.MakeName + ',' + e.carPriceQuote().carDetails.ModelName + ',' + e.carPriceQuote().carDetails.VersionName + "," + e.carPriceQuote().cityDetail.CityName });
    return true;
}

var PQ = {
    doc: $(document),
    categoryItem: {
        Exshowroom: 2,
        Rto: 3,
        Insurance: 5
    },
    pageProperties: {
        Overview: 1,
        Insurance: 3,
        EmiCalculator: 5,
        TurboWidget: 6
    },
    advantage: {
        advantageTracking: function (event, action) {
            Common.utils.trackAction(event, 'deals_desktop', action, 'PQpagedesktop');
            return true;
        },
        advantageAdTracking: function (event, action) {
            Common.utils.trackAction(event, 'deals_desktop', action, 'PQ_pageAddesktop');
            return true;
        }
    },
    price: {
        metallicFilterActive: function (pqList) {
            return ko.utils.arrayFilter(pqList, function (item) { return item.CategoryItemId == PQ.categoryItem.Exshowroom; }).length > 1;
        },
        filteredPrices: function (pqList, filerCondition) {
            return ko.utils.arrayFilter(pqList, function (item) { return item.IsMetallic == filerCondition; })
        },
        calculateORP: function (pqList) {
            var total = 0;
            ko.utils.arrayForEach(pqList, function (price) {
                total += price.Value;
            });
            return total;
        },
        getDefaultMettalicFilter: function (pqList) {
            if (PQ.price.metallicFilterActive(pqList))
                return false;
            else
                return pqList[0].IsMetallic;
        },
        getMinimumORP: function (pqList) {
            var defaultFilter = PQ.price.getDefaultMettalicFilter(pqList);
            var filteredPriceList = PQ.price.filteredPrices(pqList, defaultFilter);
            return PQ.price.calculateORP(filteredPriceList);
        },
        isCarPresentOnPqPage: function (currentPqObject, response) {
            return (currentPqObject.carDetails.ModelId === response[0].carDetails.ModelId && currentPqObject.carDetails.VersionId === response[0].carDetails.VersionId
                 && currentPqObject.cityDetail.CityId === response[0].cityDetail.CityId && currentPqObject.cityDetail.AreaId === response[0].cityDetail.AreaId)
        },
        isPqReplacable: function (pageId) {
            return (pageId == pqPageId.PQByChangingVersion || pageId == pqPageId.PQByChangingCity ||
                pageId == pqPageId.PQByChangingSubRegion || pageId == pqPageId.PQByNearCityClick)
        },
        addOrReplacePqToViewModel: function (isCarPresent, pageId, response) {
            if (!isCarPresent) {
                if (PQ.price.isPqReplacable(pageId)) {
                    replacePQToViewModel(response, pageId);
                }
                else {
                    addPQToViewModel(response);
                }
            }
        }
    },

    advertisement: {
        modelName: "",
        cityName: "",

        BankBazaarLink: {
            advertisementLinkTracking: function (cityName, modelName) {
                Common.utils.trackAction("PQ-Page-Tracking", "BB_PQ_Link", "Link_Shown", modelName + ',' + cityName);
            },
            openPopUp: function () {
                PQ.slider.category = 'BB_PQ_Link';
                PQ.slider.open();
            },
            closePopUp: function () {
                PQ.slider.close();
            },
            registerEvents: function () {
                PQ.doc.on('click', 'div#divBtnBBLnkBtn', function () {
                    Common.utils.trackAction('PQ-Page-Tracking', "BB_PQ_Link", "BB_Button_Clicked", PQ.advertisement.modelName + ',' + PQ.advertisement.cityName);
                    window.open('http://www.bankbazaar.com/car-loan.html?WT.mc_id=bb01|CL|PQpageSliderButton_desktop&utm_source=bb01&utm_medium=display&utm_campaign=bb01&variant=slide&variantOptions=mobileRequired', '_blank');
                    PQ.slider.close();
                });

            },
            openBankBazzarSlider: function (cityName, modelName) {
                PQ.advertisement.modelName = modelName;
                PQ.advertisement.cityName = cityName;
                PQ.advertisement.BankBazaarLink.openPopUp();
                Common.utils.trackAction("PQ-Page-Tracking", "BB_PQ_Link", "Link_Clicked", modelName + ',' + cityName);
            },

        },
        BhartiAxa: {
            openPopUp: function () {
                PQ.slider.category = 'BhartiAxa_PQ_Ad';
                PQ.slider.open();
                preFillCustomerDetails("#divBhartiAxa");
                $('#divBhartiAxaLeadForm').show();
                hideCustomerFormErrors();
                PQ.advertisement.BhartiAxa.hideThankYouScreen();
            },
            getPopupData: function () {
                popupData.showEmail = true;
                popupData.PopupID = "#divBhartiAxa";
                popupData.GACat = PQ.slider.category;
                return popupData;
            },
            registerEvents: function () {
                PQ.doc.on('click', '#btnBhartiaxaSubmit', function () {
                    if (PQ.advertisement.BhartiAxa.validateFields("#divBhartiAxa")) {
                        PQ.advertisement.BhartiAxa.submitLead();
                    }
                });
                PQ.doc.on('click', 'div#divViewQuote', function () {
                    var bhartiAxaUrl = $(this).attr('href');
                    window.open(bhartiAxaUrl);
                    PQ.slider.close();
                    Common.utils.trackAction('PQ-Page-Tracking', PQ.slider.category, "BA_ViewQuote_Clicked");
                });
            },
            getApiInput: function () {
                var currentPQObject = getCurrentPQObject();
                var apiInput = new Object();
                apiInput.VersionId = currentPQObject.carDetails.VersionId,
                apiInput.CityId = currentPQObject.cityDetail.CityId,
                apiInput.Name = "Anonymous",
                apiInput.Email = $(popupData.PopupID + ' #custEmail').val(),
                apiInput.Mobile = $(popupData.PopupID + ' #custMobile').val(),
                apiInput.InsuranceNew = true
                return apiInput;
            },
            submitLead: function () {
                var apiInput = PQ.advertisement.BhartiAxa.getApiInput();
                $(popupData.PopupID + " .btnBASubmit").val("Processing...");

                $.ajax({
                    type: 'POST',
                    url: '/api/insurance/bhartiaxa/quote/',
                    data: apiInput,
                    contentType: "application/x-www-form-urlencoded",
                    dataType: 'Json',
                    success: function (response) {
                        PQ.advertisement.BhartiAxa.processPostSubmitMsg(response);
                    },
                    failure: function (response) {
                        Common.utils.trackAction('PQ-Page-Tracking', PQ.slider.category, "BA_Unsuccessful_Submit");
                        console.log("Failure, check connection");
                    }
                });
            },
            showThankYouScreen: function () {
                $('#bhartiaxa-thank-you').show();
                $('#divBhartiAxaLeadForm').hide();
            },
            showViewQuoteBtn: function (redirectUrl) {
                $('div#divViewQuote').attr('href', redirectUrl).show();
                $('div.view-quote').show();
            },
            hideThankYouScreen: function () {
                $('#bhartiaxa-thank-you').hide();
            },
            validateFields: function (targetFormDiv) {
                popupData = PQ.advertisement.BhartiAxa.getPopupData();
                var retVal = true;
                if (!validateMobile(targetFormDiv))
                    retVal = false;
                if (!validateEmail(targetFormDiv))
                    retVal = false;
                return retVal;
            },
            processPostSubmitMsg: function (response) {
                Common.utils.trackAction('PQ-Page-Tracking', PQ.slider.category, "BA_Successful_Submit");
                PQ.advertisement.BhartiAxa.showThankYouScreen();
                Common.utils.setEachCookie('_CustEmail', $(popupData.PopupID + ' #custEmail').val());
                Common.utils.setEachCookie('_CustMobile', $(popupData.PopupID + ' #custMobile').val());

                $(popupData.PopupID + " .btnBASubmit").val("Submit");

                if (response.IsQuoteAvailable) {
                    PQ.advertisement.BhartiAxa.showViewQuoteBtn(response.RedirectUrl);
                    Common.utils.trackAction('PQ-Page-Tracking', PQ.slider.category, "BA_Successful_ThankYou_Shown");
                } else {
                    Common.utils.trackAction('PQ-Page-Tracking', PQ.slider.category, "BA_Unsuccessful_ThankYou_Shown");
                }
            }
        },
        TrackImpressions: function (act, cat, lab) {
            Common.utils.trackAction("CWNonInteractive", act, cat, lab);
        }
    },

    slider: {
        category: '',
        open: function () {
            if (PQ.slider.category == '') {
                return;
            }
            else if (PQ.slider.category == 'BB_PQ_Link') {
                $("#divBankBazaar").show();
            }
            else if (PQ.slider.category == 'BhartiAxa_PQ_Ad') {
                $("#divBhartiAxa").show();
            }
            Common.utils.lockPopup();
            $("#divPqSliderWrapper").animate({ right: '0' }, '600');
        },
        close: function () {
            Common.utils.unlockPopup();
            $("#divPqSliderWrapper").animate({ right: '-500px' }, '600');
            PQ.slider.category = '';
            $(".slider-div").hide();
        },
        emptyGlobalRecoVariables: function () {
            RECOMMENDATIONLEADCLICKSOURCE = null;
            RECOMMENDATIONINQUIRYSOURCE = null;
        },
        initGlobalRecoVariables: function (recoLeadClickSource, recoInquirySource) {
            RECOMMENDATIONLEADCLICKSOURCE = recoLeadClickSource;
            RECOMMENDATIONINQUIRYSOURCE = recoInquirySource;
        },
        registerEvents: function () {
            PQ.doc.on('click', 'div.divLoginCloseBtn', function () {
                if ($('#divPqSliderWrapper').length >= 1) {
                    PQ.slider.close();
                    if (PQ.slider.category != '') {
                        Common.utils.trackAction('PQ-Page-Tracking', PQ.slider.category, "Side_Panel_Closed_Close_Btn");
                    }
                }
                PQ.slider.emptyGlobalRecoVariables();
            });
            PQ.doc.on('click', 'div#globalPopupBlackOut', function () {
                if ($("div#divPqSliderWrapper").is(':visible')) {
                    PQ.slider.close();
                    if (PQ.slider.category != '') {
                        Common.utils.trackAction("PQ-Page-Tracking", PQ.slider.category, "Side_Panel_Closed_Outside_Click");
                    }
                }
                PQ.slider.emptyGlobalRecoVariables();
            });
            PQ.doc.on('keyup', function (e) {
                if (($("div#divPqSliderWrapper").is(':visible')) && e.keyCode == 27) {   // esc btn click
                    PQ.slider.close();
                    if (PQ.slider.category != '') {
                        Common.utils.trackAction("PQ-Page-Tracking", PQ.slider.category, "Side_Panel_Closed_Esc");
                    }
                }
                PQ.slider.emptyGlobalRecoVariables();
            });
        },

    },

    cookie: {
        setCustCityCookie: function (cityName, cityId) {
            switch (cityId) {
                case 645: {
                    cityName = "Thane";
                    break;
                }
                case 646: {
                    cityName = "Pune";
                    break;
                }
                case 647: {
                    cityName = "Pune";
                    break;
                }
            }
            $.cookie("_CustCity", cityName, { path: '/' });
        }
    }
}

function initFunctionsOnPqResponse(response) {
    alternateCars();
    hideImageLoading();
    initInlineLeadForm(response);
    initInlineCrossSellLeadForm(response);
    AddUserHistoryToCookie(response);
}

var Quotation = {
    registerEvents: function () {
        $('.ad-tooltip').on('mouseover', function () {
            if (!$('#addNewPQ .add-text').is(":visible")) {
                $(btObj.btSelector).btOff();
            }
        });
        $('.ad-tooltip').on('mouseout', function () {
            $(btObj.btSelector).btOff();
        });
        $('.ad-tooltip.stable-tooltip').on('mouseover', function () {
            $(btObj.btSelector).btOn();
        });
        $('.ad-tooltip.stable-tooltip').on('mouseout', function () {
            $(btObj.btSelector).btOn();
        });
        $(document).on('click', '.bt-wrapper a', function (e) {
            e.preventDefault();
            $(btObj.btSelector).btOff();
        });

    }
}

function saveDealerCookie(campaignId, cityId, zoneId, modelId) {
    if (cityId == null) cityId = -1;
    var dealerDetail = $.cookie("_dealerCityModel");
    if (dealerDetail == null || dealerDetail == undefined)
        $.cookie("_dealerCityModel", campaignId + "~" + cityId + "~" + modelId + "~" + zoneId + "!", { path: '/' });
    else {
        var temp = dealerDetail.split('!');
        var len = temp.length;
        var flag = 0;
        for (var i = 0 ; i < len - 1 ; i++) {
            var curr = temp[i].split('~');
            var existingcity = curr[1];
            var existingmodel = curr[2];
            var existingzone = curr[3];
            if (existingcity == cityId && existingmodel == modelId && existingzone == zoneId) {
                dealerDetail = dealerDetail.replace(curr[0], campaignId);
                $.cookie("_dealerCityModel", dealerDetail, { path: '/' });
                break;
            } flag++;
        }
        if (flag == (len - 1))
            $.cookie("_dealerCityModel", dealerDetail + campaignId + "~" + cityId + "~" + modelId + "~" + zoneId + "!", { path: '/' });
    }
}