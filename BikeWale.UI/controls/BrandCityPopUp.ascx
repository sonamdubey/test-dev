<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.BrandCityPopUp" %>
<script runat="server">
    private string staticUrl1 = System.Configuration.ConfigurationManager.AppSettings["staticUrl"];
    private string staticFileVersion1 = System.Configuration.ConfigurationManager.AppSettings["staticFileVersion"];
</script>
<script type="text/javascript">
    lscache.flushExpired();  //remove expired
    var BrandsKey = "all";
    var BrandCityKey = "brandcity_";
    var sourceHref = '0';
    var brandClicked = false;
    var cityClicked = false;
</script>
<style>
    div#brandcityPopup{
      height: 310px;
    }
</style>
<link href="<%= !string.IsNullOrEmpty(staticUrl1) ? "http://st2.aeplcdn.com" + staticUrl1 : string.Empty %>/css/chosen.min.css?<%=staticFileVersion1 %>" rel="stylesheet" />
<div class="bw-popup hide bw-popup-sm" id="brandcityPopup">
    <div class="popup-inner-container">
        <div class="bwsprite popup-close-btn close-btn position-abt pos-top10 pos-right10 cur-pointer"></div>
        <div class="cityPop-icon-container">
            <div class="icon-outer-container rounded-corner50 margin-bottom20">
                <div class="icon-inner-container rounded-corner50">
                    <span class="bwsprite cityPopup-icon margin-top20"></span>
                </div>
            </div>
        </div>
        <p class="font20 margin-top15 text-capitalize text-center">Search dealers</p>
         <div class="padding-top10" id="brandCityPopUpContent">
            <div id="divBrandLoader" class="margin-top10 form-control-box">
                <div class="placeholder-loading-text form-control">Loading Brands..<span class="fa fa-spinner fa-spin position-abt text-black btnSpinner"></span></div>
                <div data-bind="visible: bookingBrands().length > 0">
                    <select data-placeholder="--Select Brands--" class="chosen-select" id="ddlBrandPopup" data-bind="options: bookingBrands(), value: selectedBrand, optionsText: 'makeName', optionsValue: 'makeId', optionsCaption: '--Select Brand--', event: { change: makeChangedPopup }">

                    </select>
                    <span class="bwsprite error-icon error-tooltip-siblings"></span>
                    <div class="bw-blackbg-tooltip error-tooltip-siblings"></div>
                </div>
                <span class="position-abt progress-bar"></span>
            </div>


            <div id="divCitiesLoader" class="margin-top10 form-control-box">
                <div class="placeholder-loading-text form-control">Loading Cities..<span class="fa fa-spinner fa-spin position-abt text-black btnSpinner"></span></div>
                <div data-bind="visible: listCities().length > 0">
                    <select data-placeholder="--Select City--" class="chosen-select" id="ddlCityPopup" tabindex="2" data-bind="options: listCities(), value: selectCity, optionsText: 'cityName', optionsValue: 'cityId', optionsCaption: '--Select City--', event: { change: isValidInfoPopup }"></select>
                    <span class="bwsprite error-icon error-tooltip-siblings"></span>
                    <div class="bw-blackbg-tooltip error-tooltip-siblings"></div>
                </div>
                <span class="position-abt progress-bar"></span>
            </div>
         <input id="btnSearchBrandCity" class="action-btn margin-top15 margin-left100 " style="display: block;position:absolute" type="button" value="Search" data-bind="click: searchByBrandCityPopUp, enable: (listCities().length > 0)">
        </div>
    </div>
</div>

<script type="text/javascript">

    var $ddlCities = $("#ddlCities"), $ddlMakes = $("#ddlMakes");
    var bikeCityId = $("#ddlCities").val();
    var brandcityPopUp = $('#brandcityPopUp');
    lscache.flushExpired();
    lscache.setBucket('DLPage');
    popupcity = $('#ddlCityPopup');
    popupBrand = $('#ddlBrandPopup');
    var viewModelCityBrandPopup = new function (){
        var self = this;
    self.selectCity = ko.observable(),
       self.listCities = ko.observableArray([]),
    self.selectedBrand = ko.observable(),
    self.bookingBrands = ko.observableArray([]),
    self.hasCities = ko.observable(),
    self.searchByBrandCityBtnClicked = ko.observable(false),
    self.makeMasking = ko.pureComputed(function () {

            return ko.utils.arrayFirst(self.bookingBrands(), function(child) {
                return child.makeId === self.selectedBrand();
            }).maskingName;
    })
    self.cityMasking = ko.pureComputed(function () {

        return ko.utils.arrayFirst(self.listCities(), function (child) {
            return child.cityId === self.selectCity();
        }).cityMaskingName;
    })
    };

    function FillBrandsPopup() {
        var isAborted = false;
        if (viewModelCityBrandPopup.bookingBrands().length < 1) {
            debugger;
            $.ajax({
                type: "GET",
                url: "/api/makelist/?requesttype=" + requestType,
                dataType: 'json',
                beforeSend: function (xhr) {
                    viewModelCityBrandPopup.bookingBrands([]);
                    viewModelCityBrandPopup.listCities([]);
                    startLoading($("#divBrandLoader"));
                    $("#divBrandLoader .placeholder-loading-text").show();
                    if (data = lscache.get(BrandsKey)) {
                        var brands = ko.toJS(data);
                        if (brands) {
                            stopLoading($("#divBrandLoader"));
                            $("#divBrandLoader .placeholder-loading-text").hide();
                            viewModelCityBrandPopup.bookingBrands(data);
                            isAborted = true;
                            xhr.abort();
                        }
                        else {
                            viewModelCityBrandPopup.bookingBrands([]);
                        }
                    }
                },
                success: function (response) {
                    var _gZippedBrandsParse = ko.toJS(response);
                    lscache.set(BrandsKey, _gZippedBrandsParse.makes, 60);
                    var brands = _gZippedBrandsParse;
                    var brands = ko.toJS(response);
                    if (brands) {
                        stopLoading($("#divBrandLoader"));
                        $("#divBrandLoader .placeholder-loading-text").hide();
                        viewModelCityBrandPopup.bookingBrands(brands);
                        
                    }
                    else {
                        viewModelCityBrandPopup.bookingBrands([]);
                        $('#ddlBrandsPopup').trigger("chosen:updated");
                    }
                },
                complete: function () {
                    completeBrandPopup();
                }
            });

            if (isAborted) {
                completeBrandPopup();
            }
        }
    }



    function completeBrandPopup() {
        popupBrand.find("option[value='0']").prop('disabled', true);
        popupBrand.trigger('chosen:updated');
        makeChangedPopup();
    }

    function makeChangedPopup() {
        var isAborted = false;
        if (viewModelCityBrandPopup.selectedBrand() != undefined) {
            debugger;
            BrandCityKey = "brandcity_" + viewModelCityBrandPopup.selectedBrand().toString();
            $.ajax({
                type: "GET",
                url: "/api/v2/DealerCity/?makeId=" + viewModelCityBrandPopup.selectedBrand(),
                dataType: 'json',
                beforeSend: function (xhr) {
                    viewModelCityBrandPopup.listCities([]);
                    $("#divCitiesLoader").show();
                    $("#divCitiesLoader .placeholder-loading-text").show();
                    startLoading($("#divCitiesLoader"));
                    if (data = lscache.get(BrandCityKey)) {
                        var cities = ko.toJS(data);
                        if (cities) {
                            stopLoading($("#divCitiesLoader"));
                            $("#divCitiesLoader .placeholder-loading-text").hide();
                            insertCitySeparatorNew(cities);
                            viewModelCityBrandPopup.listCities(data);
                            isAborted = true;
                            xhr.abort();
                        }
                    }
                },
                success: function (response) {
                    var cities = ko.toJS(response.City);
                    lscache.set(BrandCityKey, cities, 60);
                    if (cities.length) {
                        stopLoading($("#divCitiesLoader"));
                        $("#divCitiesLoader .placeholder-loading-text").hide();
                        insertCitySeparatorNew(cities);
                        viewModelCityBrandPopup.listCities(cities);
                    }
                    else {
                        viewModelCityBrandPopup.listCities([]);
                        $("#divCitiesLoader").hide();
                        $('#ddlCityPopup').trigger("chosen:updated");
                    }
                },
                error: function (e) {
                    viewModelCityBrandPopup.listCities([]);
                    $('#ddlCityPopup').trigger("chosen:updated");
                },
                complete: function () {
                    completeCityPopup();
                }
            });

        } else {
            viewModelCityBrandPopup.listCities([]);
            $("#divCitiesLoader").hide();
        }

        if (isAborted) {
            completeCityPopup();
        }
        isValidInfoPopup();
    }

    function completeCityPopup() {
        $('#ddlCityPopup').trigger("chosen:updated");

        ev = $._data($('ul.chosen-results')[1], 'events');
        if (!(ev && ev.click)) {
            $($('ul.chosen-results')[1]).on('click', 'li', function (e) {
                if (cityClicked == false) {
                    if (ga_pg_id != null && ga_pg_id == 2) {
                           cityClicked = true;
                    }
                }
            });
        }
    }

    function isValidInfoPopup() {
        isValid = true;
        var errMsg = "",
            errMsgParent;

        if (viewModelCityBrandPopup.selectedBrand() == undefined && viewModelCityBrandPopup.searchByBrandCityBtnClicked()) {
            errMsgParent = $('#divBrandLoader.form-control-box');
            errMsg += "Brand,";
            isValid = false;
            showBrandCityError(errMsgParent, errMsg);
        }

        else {
            removeBrandCityError($('#divBrandLoader.form-control-box'));
        }

        if (viewModelCityBrandPopup.bookingBrands().length > 0 && viewModelCityBrandPopup.selectedBrand() == undefined && viewModelCityBrandPopup.searchByBrandCityBtnClicked()) {
            errMsgParent = $('#divCitiesLoader.form-control-box');
            errMsg += "City,";
            isValid = false;
            showBrandCityError(errMsgParent, errMsg);
        }

        else {
            removeBrandCityError($('#divCitiesLoader.form-control-box'));
        }

        return isValid;
    }

    function showBrandCityError(errMsgParent, errMsg) {
        errMsgParent.find('.error-tooltip-siblings').show();
        errMsgParent.css({ 'border-color': 'red' });
        errMsg = errMsg.substring(0, errMsg.length - 1);
        errMsgParent.find('.bw-blackbg-tooltip').text("Please select " + errMsg);
    }

    function removeBrandCityError(errMsgParent) {
        errMsgParent.css({ 'border-color': '#ccc' });
        errMsgParent.find('.error-tooltip-siblings').hide();
        errMsgParent.find('.bw-blackbg-tooltip').text("");
    }


    function searchByBrandCityPopUp() {
        
        viewModelCityBrandPopup.searchByBrandCityBtnClicked(true);
                if (requestType == 12) {
            window.location.href = "/" + viewModelCityBrandPopup.makeMasking() + "-dealer-showrooms-in-" + viewModelCityBrandPopup.cityMasking() + "/";
        }
        else if (requestType == 13) {
            window.location.href = "/" + viewModelCityBrandPopup.makeMasking() + "-service-center-in-" + viewModelCityBrandPopup.cityMasking() + "/";
        }
    }


    $(document).ready(function () {
        $('body').on('click', "#brandSelect", function (e) {
             $('#brandcityPopup').fadeIn(100);
             popup.lock();
            e.preventDefault();
            $("#errMsgPopUp").empty();
            FillBrandsPopup();
             });

        $('#brandcityPopup .close-btn, .blackOut-window').mouseup(function () {
            popup.unlock();
            $('#brandcityPopup').fadeOut(100);
        });

        $("#ddlCityPopup").chosen({ no_results_text: "No matches found!!" });
        $("#ddlBrandPopup").chosen({ no_results_text: "No matches found!!" });
        $('.chosen-container').attr('style', 'width:100%;');

        ko.applyBindings(viewModelCityBrandPopup, $("#brandCityPopUpContent")[0]);
    });
    
</script>
