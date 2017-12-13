<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.BrandCityPopUp" %>
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
        <p class="font20 margin-top15 text-center">Looking for a different Dealer?</p>
        <p class="text-light-grey margin-bottom15 margin-top15 text-center">Select the Brand and City to see Dealer details</p>
        <%} %>
        <%else if (requestType.Equals(Bikewale.Entities.BikeData.EnumBikeType.ServiceCenter))
          { %>
        <p class="font20 margin-top15 text-center">Looking for a different Service Center?</p>
        <p class="text-light-grey margin-bottom15 margin-top15 text-center">Select the Brand and City to see Service Center details</p>
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
            <input id="btnSearchBrandCity" class="action-btn margin-top15 margin-left70 " style="display: block;" type="button" value="Show dealers" data-bind="click: searchByBrandCityPopUp, enable: (bookingBrands().length > 0)">
            <%} %>
            <%else if (requestType.Equals(Bikewale.Entities.BikeData.EnumBikeType.ServiceCenter))
              {%>
            <input id="btnSearchBrandCity" class="action-btn margin-top15 margin-left60 " style="display: block;" type="button" value="Show service centers" data-bind="click: searchByBrandCityPopUp, enable: (bookingBrands().length > 0)">
            <%} %>
        </div>
    </div>
</div>

<script type="text/javascript">

    var BrandsKey = "BrandCityPopUp_",BrandCityKey = "brandcity_",brandcityPopUp,makeid = '<%=makeId%>',cityId = '<%=cityId%>';
    var brandcityPopUp,popupcity,popupBrand,vmCityBrandPopup;

    var viewModelCityBrandPopup = function () {
        var self = this;
        self.selectCity = ko.observable(),
        self.listCities = ko.observableArray([]),
        self.selectedBrand = ko.observable(),
        self.bookingBrands = ko.observableArray([]),
        self.hasCities = ko.observable(),
        self.SelectedCityId = ko.observable(cityId),
        self.searchByBrandCityBtnClicked = ko.observable(false),
        self.makeMasking = ko.pureComputed(function () {
            return ko.utils.arrayFirst(self.bookingBrands(), function (child) {
                return child.makeId === self.selectedBrand();
            }).maskingName;
        });
        self.cityMasking = ko.pureComputed(function () {

            return ko.utils.arrayFirst(self.listCities(), function (child) {
                return child.cityId === self.selectCity();
            }).cityMaskingName;
        });
        self.cityApiUrl = ko.pureComputed(function () {
            if (<%=(requestType.Equals(Bikewale.Entities.BikeData.EnumBikeType.Dealer)).ToString().ToLower()%>)
                return "/api/v2/DealerCity/?makeId=" + self.selectedBrand();
            else if (<%=(requestType.Equals(Bikewale.Entities.BikeData.EnumBikeType.ServiceCenter)).ToString().ToLower()%>)
                return "/api/servicecenter/cities/make/" + self.selectedBrand() + "/";
        });
        self.makeApiUrl = "/api/makelist/?requesttype=" + '<%=(int)requestType%>';

        self.FillBrandsPopup = function () {
            var isAborted = false;
            
            if (self.bookingBrands().length < 1 || self.bookingBrands().length == undefined) {
                $.ajax({
                    type: "GET",
                    url: self.makeApiUrl,
                    dataType: 'json',
                    beforeSend: function (xhr) {
                        self.bookingBrands([]);
                        self.listCities([]);
                        progressBar.startLoading($("#divBrandLoader"));
                        $("#divBrandLoader .placeholder-loading-text").show();
                        BrandsKey="BrandCityPopUp_"+'<%=requestType%>';
                        if (data = lscache.get(BrandsKey)) {
                            var brands = ko.toJS(data);
                            if (brands) {
                                progressBar.stopLoading($("#divBrandLoader"));
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
                            progressBar.stopLoading($("#divBrandLoader"));
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
        };

        self.completeBrandPopup = function () {
            
            if (makeid>0) {
                self.selectedBrand(parseInt(makeid));
            }
            popupcity.find("option[value='0']").prop('disabled', true);
            popupcity.trigger('chosen:updated');

            popupBrand.find("option[value='0']").prop('disabled', true);
            popupBrand.trigger('chosen:updated');
            self.makeChangedPopup();
        };

        self.makeChangedPopup = function () {
            var isAborted = false;
            self.searchByBrandCityBtnClicked(false);
           
            if (self.selectedBrand() != undefined) {
                BrandCityKey = "brandcity_" +'<%=requestType%>'+'_' + self.selectedBrand().toString();
                if (self.selectCity() != null) {
                    self.SelectedCityId(self.selectCity());
                }
                $.ajax({
                    type: "GET",
                    url: self.cityApiUrl(),
                    dataType: 'json',
                    beforeSend: function (xhr) {
                        self.listCities([]);
                        $("#divCitiesLoader").show();
                        $("#divCitiesLoader .placeholder-loading-text").show();
                        progressBar.startLoading($("#divCitiesLoader"));
                        if (data = lscache.get(BrandCityKey)) {
                            var cities = ko.toJS(data);
                            if (cities) {
                                progressBar.stopLoading($("#divCitiesLoader"));
                                $("#divCitiesLoader .placeholder-loading-text").hide();
                                insertCitySeparatorNew(cities);
                                self.listCities(data);
                                isAborted = true;
                                xhr.abort();
                            }
                        }
                    },
                    success: function (response) {
                        var cities;
                        if (<%=(requestType.Equals(Bikewale.Entities.BikeData.EnumBikeType.Dealer)).ToString().ToLower()%>)
                            cities = ko.toJS(response.City);
                        else if (<%=(requestType.Equals(Bikewale.Entities.BikeData.EnumBikeType.ServiceCenter)).ToString().ToLower()%>)
                            cities = ko.toJS(response);
                        if (cities != null)
                            lscache.set(BrandCityKey, cities, 60);
                        if (cities.length) {
                            progressBar.stopLoading($("#divCitiesLoader"));
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
                    complete: function (xhr) {
                        if(xhr.status!=200)
                        {
                            self.listCities([]);
                            $('#ddlCityPopup').trigger("chosen:updated");
                        }
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
        };


        self.completeCityPopup = function () {
            
            if (self.SelectedCityId() != null) {
                self.preselectCity();
            }
            else self.selectCity(null);
            $('#ddlCityPopup').trigger("chosen:updated");           
            
        };

        self.preselectCity = function () {

            if (self.listCities().length > 0) {
                self.selectCity(self.SelectedCityId());
            }
        };

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
        };
        self.showBrandCityError = function (errMsgParent, errMsg) {
            errMsgParent.find('.error-tooltip-siblings').show();
            errMsgParent.css({ 'border-color': 'red' });
            errMsg = errMsg.substring(0, errMsg.length - 1);
            errMsgParent.find('.bw-blackbg-tooltip').text("Please select " + errMsg);
        };

        self.removeBrandCityError = function (errMsgParent) {
            errMsgParent.css({ 'border-color': '#ccc' });
            errMsgParent.find('.error-tooltip-siblings').hide();
            errMsgParent.find('.bw-blackbg-tooltip').text("");
        };

        self.searchByBrandCityPopUp = function () {

            self.searchByBrandCityBtnClicked(true);
            isvalid = self.isValidInfoPopup();
            if (isvalid) {
                if (<%=(requestType.Equals(Bikewale.Entities.BikeData.EnumBikeType.Dealer)).ToString().ToLower()%>) 
                {
                    window.location.href =  "/dealer-showrooms/" + self.makeMasking() + "/" + self.cityMasking() + "/";
            }
            else if (<%=(requestType.Equals(Bikewale.Entities.BikeData.EnumBikeType.ServiceCenter)).ToString().ToLower()%>) {
            window.location.href = + "/service-centers/" +self.makeMasking() + "/" + self.cityMasking() + "/" ;
        };
    }
    }

    };

    docReady(function () {

        brandcityPopUp = $('#brandcityPopUp'),popupcity = $('#ddlCityPopup'),popupBrand = $('#ddlBrandPopup');

        lscache.flushExpired(); 
        lscache.setBucket('BCPopup');


        $('#brandcityPopup .close-btn, .blackOut-window').mouseup(function () {
            popup.unlock();
            $('#brandcityPopup').fadeOut(100);
        });

        $("#ddlCityPopup").chosen({ no_results_text: "No matches found!!" });
        $("#ddlBrandPopup").chosen({ no_results_text: "No matches found!!" });
        $('.chosen-container').attr('style', 'width:100%;');
        vmCityBrandPopup = new viewModelCityBrandPopup;
        ko.applyBindings(vmCityBrandPopup, $("#brandCityPopUpContent")[0]);
        $('body').on('click', "#brandSelect", function (e) {
            $('#brandcityPopup').fadeIn(100);
            popup.lock();
            e.preventDefault();
            $("#errMsgPopUp").empty();
            vmCityBrandPopup.FillBrandsPopup();
        });
    });

</script>
