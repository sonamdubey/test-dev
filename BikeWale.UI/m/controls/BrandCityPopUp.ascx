<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Controls.BrandCityPopUp" %>
<script type="text/javascript">
    lscache.flushExpired();  //remove expired
    var BrandsKey = "BrandCityPopUp_mk";
    var BrandCityKey = "brandcity_";
   </script>
<link href="<%= !string.IsNullOrEmpty(staticUrl) ? "http://st2.aeplcdn.com" + staticUrl : string.Empty %>/css/chosen.min.css?<%=staticFileVersion %>" rel="stylesheet" />



<!-- pricequote widget starts here-->
<div class="bw-city-popup bwm-fullscreen-popup bw-popup-sm text-center hide" id="popupWrapper">
    <div class="city-area-banner"></div>
    <div class="popup-inner-container">
        <div class="bwmsprite onroad-price-close-btn close-btn position-abt pos-top10 pos-right10 cur-pointer"></div>
        <div id="popupHeading" class="content-inner-block-20">
            <%if (requestType.Equals(Bikewale.Entities.BikeData.EnumBikeType.Dealer))
          { %>
            <p class="font18 margin-bottom5 text-capitalize">Looking for a different dealer?</p>
            <div class="text-light-grey margin-bottom5"><span class="red">*</span>Select the brand and city to see dealer details</div>
            <%} %>
            <%else if (requestType.Equals(Bikewale.Entities.BikeData.EnumBikeType.ServiceCenter))
          { %>
             <p class="font18 margin-bottom5 text-capitalize">Looking for a different service center?</p>
            <div class="text-light-grey margin-bottom5"><span class="red">*</span>Select the brand and city to see service center details</div>
            <%} %>
            <% if(isOperaBrowser) { %>
           <div class="form-control-box margin-bottom10 ">
                <select id="opBrandList" class="form-control" tabindex="2" data-bind="options: bookingBrands(), value: selectedBrand, optionsText: 'makeName', optionsValue: 'makeId', optionsCaption: '--Select Brand--', event: { change: makeChangedPopup }"></select>
                <span class="fa fa-spinner fa-spin position-abt  text-black btnSpinner"></span>
            </div>
            <div class="form-control-box" data-bind="visible: listCities().length > 0">
                <select id="opCitiesList" class="form-control" data-bind="options: listCities(), value: selectCity, optionsText: 'cityName', optionsValue: 'cityId', optionsCaption: '--Select City--', event: { change: isValidInfoPopup }"></select>
                <span class="fa fa-spinner fa-spin position-abt  text-black btnSpinner"></span>
            </div>

            <% } else { %>
            <div id="makeSelection" class="form-control text-left input-sm position-rel margin-bottom10 ">
                <div class="selected-area" data-bind="text: (selectedBrand() != null && selectedBrand().name != '') ? selectedBrand().name : 'Select Brand'">Select Brand</div>
                <span class="bwmsprite fa-angle-right position-abt pos-top10 pos-right10"></span>

            </div>
            <div id="citySelection" class="form-control text-left input-sm position-rel margin-bottom10"  data-bind="visible: listCities().length > 0">
                <div class="selected-city" data-bind="text: (selectCity() != null && selectCity().name != '') ? selectCity().name : 'Select City'"></div>
                <span class="bwmsprite fa-angle-right position-abt pos-top10 pos-right10"></span>
            </div>

            <div id="btnPriceLoader" class="center-align margin-top20 text-center position-rel">
                <a id="btnDealerPricePopup" class="btn btn-orange btn-full-width font18" data-bind="visible: SelectedCityId() > 0 && IsPersistance() && (!hasAreas() || SelectedAreaId() > 0), click: function () { IsPersistance(false); InitializePQ(); } ">Show on-road price</a>
            </div>

            <div id="popupContent" class="bwm-city-area-popup-wrapper">
                <div class="bw-city-popup-box bwm-city-area-box city-list-container form-control-box text-left">
                    <div class="user-input-box">
                        <span class="back-arrow-box">
                            <span class="bwmsprite back-long-arrow-left"></span>
                           

                        </span>
                        <input type="text" class="form-control padding-right40" placeholder="Type to select city" id="popupCityInput" autocomplete="off" data-bind="textInput: cityFilter, attr: { value: (SelectedCity() != undefined) ? SelectedCity().name : '' }">
                    </div>
                    <ul id="popupCityList" data-bind="template: { name: 'bindCityList-template', foreach: visibleCities }"></ul>
                    <script type="text/html" id="bindCityList-template">
                        <li data-bind="text: name, attr: { 'cityId': id }, click: function (d, e) { $parent.selectCity(d, e); }"></li>
                    </script>
                    <div class="margin-top30 font24 text-center margin-top60 "></div>
                </div>

                <div class="bw-area-popup-box bwm-city-area-box area-list-container form-control-box text-left" data-bind="visible: BookingAreas().length > 0">
                    <div class="user-input-box">
                        <span class="back-arrow-box">
                            <span class="bwmsprite back-long-arrow-left"></span>
                        </span>
                        <input type="text" class="form-control padding-right40" placeholder="Type to select area" id="popupAreaInput" autocomplete="off" data-bind="textInput: areaFilter, attr: { value: (SelectedArea() != undefined) ? SelectedArea().name : '' }">
                    </div>
                    <ul id="popupAreaList" data-bind="template: { name: 'bindAreaList-template', foreach: visibleAreas }"></ul>
                    <script type="text/html" id="bindAreaList-template">
                        <li data-bind="text: name, attr: { 'areaId': id }, click: function (d, e) { $parent.selectArea(d, e); }"></li>
                    </script>
                    <div class="margin-top30 font24 text-center margin-top60 "></div>
                </div>
            </div>

            <% } %>

        </div>

    </div>
    <div id="popup-loader-container">
        <div id="popup-loader"></div>
        <div id="popup-loader-text">
            <p data-bind="text: LoadingText()" class="font14"></p> 
        </div>
    </div>
</div>





















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
        <%if (requestType.Equals(Bikewale.Entities.BikeData.EnumBikeType.Dealer))
          { %>
        <p class="font20 margin-top15 text-capitalize text-center">Looking for a different dealer?</p>
        <p class="text-light-grey margin-bottom15 margin-top15 text-capitalize text-center">Select the brand and city to see dealer details</p>
        <%} %>
        <%else if (requestType.Equals(Bikewale.Entities.BikeData.EnumBikeType.ServiceCenter))
          { %>
        <p class="font20 margin-top15 text-capitalize text-center">Looking for a different service center?</p>
        <p class="text-light-grey margin-bottom15 margin-top15 text-capitalize text-center">Select the brand and city to see service center details</p>
        <%} %>
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
            <%if (requestType.Equals(Bikewale.Entities.BikeData.EnumBikeType.Dealer))
              { %>
            <input id="btnSearchBrandCity" class="action-btn margin-top15 margin-left70 " style="display: block;" type="button" value="Search dealers" data-bind="click: searchByBrandCityPopUp, enable: (bookingBrands().length > 0)">
            <%} %>
            <%else if (requestType.Equals(Bikewale.Entities.BikeData.EnumBikeType.ServiceCenter))
              {%>
            <input id="btnSearchBrandCity" class="action-btn margin-top15 margin-left60 " style="display: block;" type="button" value="Search service centers" data-bind="click: searchByBrandCityPopUp, enable: (bookingBrands().length > 0)">
            <%} %>
        </div>
    </div>
</div>

<script type="text/javascript">
    var requestType='<%=requestType%>';
    var brandcityPopUp = $('#brandcityPopUp');
   
    lscache.setBucket('BCPopup');
    popupcity = $('#ddlCityPopup');
    popupBrand = $('#ddlBrandPopup');
    var viewModelCityBrandPopup = new function () {
        var self = this;
        self.selectCity = ko.observable(),
        self.listCities = ko.observableArray([]),
        self.selectedBrand = ko.observable(),
        self.bookingBrands = ko.observableArray([]),
        self.hasCities = ko.observable(),
        self.searchByBrandCityBtnClicked = ko.observable(false),
        self.makeMasking = ko.pureComputed(function () {

            return ko.utils.arrayFirst(self.bookingBrands(), function (child) {
                return child.makeId === self.selectedBrand();
            }).maskingName;
        })
        self.cityMasking = ko.pureComputed(function () {

            return ko.utils.arrayFirst(self.listCities(), function (child) {
                return child.cityId === self.selectCity();
            }).cityMaskingName;
        })

        self.FillBrandsPopup = function () {
            var isAborted = false;
            if (self.bookingBrands().length < 1 || self.bookingBrands().length == undefined) {
                $.ajax({
                    type: "GET",
                    url: "/api/makelist/?requesttype=" + '<%=(int)requestType%>',
                    dataType: 'json',
                    beforeSend: function (xhr) {
                        self.bookingBrands([]);
                        self.listCities([]);
                        startLoading($("#divBrandLoader"));
                        $("#divBrandLoader .placeholder-loading-text").show();
                        if (data = lscache.get(BrandsKey)) {
                            var brands = ko.toJS(data);
                            if (brands) {
                                stopLoading($("#divBrandLoader"));
                                $("#divBrandLoader .placeholder-loading-text").hide();
                                self.bookingBrands(brands);
                                isAborted = true;
                                xhr.abort();
                            }
                            else {
                                self.bookingBrands([]);
                            }
                        }
                    },
                    success: function (response) {
                        var brands = ko.toJS(response);
                        if (brands) {
                            lscache.set(BrandsKey, brands.makes, 60);
                            stopLoading($("#divBrandLoader"));
                            $("#divBrandLoader .placeholder-loading-text").hide();
                            self.bookingBrands(brands.makes);

                        }
                        else {
                            self.bookingBrands([]);
                            $('#ddlBrandsPopup').trigger("chosen:updated");
                        }
                    },
                    complete: function () {
                        self.completeBrandPopup();
                    }
                });

                if (isAborted) {
                    self.completeBrandPopup();
                }
            }
        }

        self.completeBrandPopup = function () {
            popupBrand.find("option[value='0']").prop('disabled', true);
            popupBrand.trigger('chosen:updated');
            self.makeChangedPopup();
        }

        self.makeChangedPopup = function () {
            var isAborted = false;
            self.searchByBrandCityBtnClicked(false)
            if (self.selectedBrand() != undefined) {
                BrandCityKey = "brandcity_" + self.selectedBrand().toString();
                $.ajax({
                    type: "GET",
                    url: "/api/v2/DealerCity/?makeId=" + self.selectedBrand(),
                    dataType: 'json',
                    beforeSend: function (xhr) {
                        self.listCities([]);
                        $("#divCitiesLoader").show();
                        $("#divCitiesLoader .placeholder-loading-text").show();
                        startLoading($("#divCitiesLoader"));
                        if (data = lscache.get(BrandCityKey)) {
                            var cities = ko.toJS(data);
                            if (cities) {
                                stopLoading($("#divCitiesLoader"));
                                $("#divCitiesLoader .placeholder-loading-text").hide();
                                insertCitySeparatorNew(cities);
                                self.listCities(data);
                                isAborted = true;
                                xhr.abort();
                            }
                        }
                    },
                    success: function (response) {
                        var cities = ko.toJS(response.City);
                        if (cities != null)
                            lscache.set(BrandCityKey, cities, 60);
                        if (cities.length) {
                            stopLoading($("#divCitiesLoader"));
                            $("#divCitiesLoader .placeholder-loading-text").hide();
                            insertCitySeparatorNew(cities);
                            self.listCities(cities);
                        }
                        else {
                            self.listCities([]);
                            $("#divCitiesLoader").hide();
                            $('#ddlCityPopup').trigger("chosen:updated");
                        }
                    },
                    error: function (e) {
                        self.listCities([]);
                        $('#ddlCityPopup').trigger("chosen:updated");
                    },
                    complete: function () {
                        self.completeCityPopup();
                    }
                });

            } else {
                self.listCities([]);
                $("#divCitiesLoader").hide();
            }

            if (isAborted) {
                self.completeCityPopup();
            }
            self.isValidInfoPopup();
        }


        self.completeCityPopup = function () {
            $('#ddlCityPopup').trigger("chosen:updated");
        }


        self.isValidInfoPopup = function () {
            isValid = true;
            var errMsg = "",
                errMsgParent;

            if (self.selectedBrand() == undefined && self.searchByBrandCityBtnClicked()) {
                errMsgParent = $('#divBrandLoader.form-control-box');
                errMsg += "Brand,";
                isValid = false;
                self.showBrandCityError(errMsgParent, errMsg);
            }

            else {
                self.removeBrandCityError($('#divBrandLoader.form-control-box'));
            }

            if (self.bookingBrands().length > 0 && self.selectCity() == undefined && self.searchByBrandCityBtnClicked()) {
                errMsgParent = $('#divCitiesLoader.form-control-box');
                errMsg += "City,";
                isValid = false;
                self.showBrandCityError(errMsgParent, errMsg);
            }

            else {
                self.removeBrandCityError($('#divCitiesLoader.form-control-box'));
            }

            return isValid;
        }
        self.showBrandCityError = function (errMsgParent, errMsg) {
            errMsgParent.find('.error-tooltip-siblings').show();
            errMsgParent.css({ 'border-color': 'red' });
            errMsg = errMsg.substring(0, errMsg.length - 1);
            errMsgParent.find('.bw-blackbg-tooltip').text("Please select " + errMsg);
        }

        self.removeBrandCityError = function (errMsgParent) {
            errMsgParent.css({ 'border-color': '#ccc' });
            errMsgParent.find('.error-tooltip-siblings').hide();
            errMsgParent.find('.bw-blackbg-tooltip').text("");
        }

        self.searchByBrandCityPopUp = function () {

            self.searchByBrandCityBtnClicked(true);
            isvalid = self.isValidInfoPopup();
            if (isvalid) {
                if (requestType == '<%=Bikewale.Entities.BikeData.EnumBikeType.Dealer%>') {
                    window.location.href = "/" + self.makeMasking() + "-dealer-showrooms-in-" + self.cityMasking() + "/";
                }
                else if (requestType == '<%=Bikewale.Entities.BikeData.EnumBikeType.ServiceCenter%>') {
                    window.location.href = "/" + self.makeMasking() + "-service-center-in-" + self.cityMasking() + "/";
                }
            }
        }

    };

    $(document).ready(function () {
        $('body').on('click', "#brandSelect", function (e) {
            $('#brandcityPopup').fadeIn(100);
            popup.lock();
            e.preventDefault();
            $("#errMsgPopUp").empty();
            viewModelCityBrandPopup.FillBrandsPopup();
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
