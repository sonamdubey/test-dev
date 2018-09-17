<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Controls.BrandCityPopUp" %>

<link href="<%= staticUrl  %>/UI/css/chosen.min.css?<%=staticFileVersion %>" rel="stylesheet" />
<div class="bw-city-popup bwm-fullscreen-popup bw-popup-sm text-center hide" id="brandcitypopupWrapper">
    <div class="city-area-banner"></div>
    <div class="popup-inner-container">
        <div class="bwmsprite onroad-price-close-btn close-btn position-abt pos-top10 pos-right10 cur-pointer"></div>
        <div id="popupHeading" class="content-inner-block-20">
            <%if (requestType.Equals(Bikewale.Entities.BikeData.EnumBikeType.Dealer))
              { %>
            <p class="font18 margin-bottom5">Looking for a different Dealer?</p>
            <div class="text-light-grey margin-bottom5"><span class="red">*</span>Select the brand and city to see dealer details</div>
            <%} %>
            <%else if (requestType.Equals(Bikewale.Entities.BikeData.EnumBikeType.ServiceCenter))
              { %>
            <p class="font18 margin-bottom5">Looking for a different Service Center?</p>
            <div class="text-light-grey margin-bottom5"><span class="red">*</span>Select the brand and city to see service center details</div>
            <%} %>
            <% if (isOperaBrowser)
               { %>
            <div class="form-control-box margin-bottom10 ">
                <select id="opBrandList" class="form-control" tabindex="2" data-bind="options: bookingBrands(), value: selectedBrand, optionsText: 'makeName', optionsValue: 'makeId', optionsCaption: '--Select Brand--', event: { change: choosebrand }"></select>
                <span class="fa fa-spinner fa-spin position-abt  text-black btnSpinner"></span>
            </div>
            <div class="form-control-box" data-bind="visible: listCities().length > 0">
                <select id="opCitiesList" class="form-control" data-bind="options: listCities(), value: selectCity, optionsText: 'cityName', optionsValue: 'cityId', optionsCaption: '--Select City--', event: { change: chooseCity }"></select>
                <span class="fa fa-spinner fa-spin position-abt  text-black btnSpinner"></span>
            </div>

            <% }
               else
               { %>
            <div id="makeSelection" class="form-control text-left input-sm position-rel margin-bottom10 " data-bind="click: function (d,e) { hideError($(e.target)) }, visible: listCities().length > 0">
                <div class="selected-area" id="ddlBrandsPopup" data-bind="click: function (d, e) { hideError($(e.target).parent()) }, text: (selectedBrand() != null && selectedBrand().makeName != '') ? selectedBrand().makeName : 'Select Brand'">Select Brand</div>
                <span class="bwmsprite error-icon errorIcon hide"></span>
                <div class="bw-blackbg-tooltip errorText hide"></div>
                <span class="bwmsprite fa-angle-right position-abt pos-top10 pos-right10"></span>

            </div>
            <div id="citiesSelection" class="form-control text-left input-sm position-rel margin-bottom10" data-bind="visible: listCities().length > 0, click: function(d,e) {hideError($(e.target))}">
                <div class="selected-city" id="ddlCityPopup" data-bind="click:function (d,e) { hideError($(e.target).parent()) },text: (selectCity() != null && selectCity().cityName != '') ? selectCity().cityName : 'Select City'"></div>
                <span class="bwmsprite error-icon errorIcon hide"></span>
                <div class="bw-blackbg-tooltip errorText hide"></div>
                <span class="bwmsprite fa-angle-right position-abt pos-top10 pos-right10"></span>
            </div>

            <div id="btnSearchBrandCity" class="center-align margin-top20 text-center position-rel">
                <%if (requestType.Equals(Bikewale.Entities.BikeData.EnumBikeType.Dealer))
                  { %>
                <a id="btnSearchBrandCityPopup" class="btn btn-orange btn-full-width font18" rel="nofollow" data-bind="click: searchByBrandCityPopUp, enable: (bookingBrands().length > 0),visible:(listCities().length > 0)">Show dealers</a>
                <%} %>
                <%else if (requestType.Equals(Bikewale.Entities.BikeData.EnumBikeType.ServiceCenter))
                  {%>
                <a id="btnSearchBrandCityPopup" class="btn btn-orange btn-full-width font18" rel="nofollow" data-bind="click: searchByBrandCityPopUp, enable: (bookingBrands().length > 0),visible:(listCities().length > 0)">Show service centers</a>
                <%} %>
            </div>

            <div id="brandcitypopupContent" class="bwm-city-area-popup-wrapper">
                <div class="bw-city-popup-box bwm-city-area-box city-list-container form-control-box text-left">
                    <div class="user-input-box">
                        <span class="back-arrow-box">
                            <span class="bwmsprite back-long-arrow-left"></span>


                        </span>
                        <input type="text" class="form-control padding-right40" placeholder="Type to select brand" id="popupMakeInput" autocomplete="off" data-bind="textInput: brandFilter, attr: { value: (selectedBrand() != undefined) ? selectedBrand().makeName : '' }">
                    </div>
                    <ul id="popupMakeList" data-bind="template: { name: 'bindBrandList-template', foreach: visibleBrands }"></ul>
                    <script type="text/html" id="bindBrandList-template">
                        <li data-bind="text: makeName, attr: { 'makeId': makeId }, click: function (d, e) { $parent.chooseBrand(d, e); }"></li>
                    </script>
                    <div class="margin-top30 font24 text-center margin-top60 "></div>
                </div>

                <div class="bw-area-popup-box bwm-city-area-box area-list-container form-control-box text-left" data-bind="visible: listCities().length > 0">
                    <div class="user-input-box">
                        <span class="back-arrow-box">
                            <span class="bwmsprite back-long-arrow-left"></span>
                        </span>
                        <%--                        --%>
                        <input type="text" class="form-control padding-right40" placeholder="Type to select city" id="popupCityNameInput" autocomplete="off" data-bind="textInput: cityFilter, attr: { value: (selectCity() != undefined) ? selectCity().cityName : '' }">
                    </div>
                    <ul id="popupCitiesList" data-bind="template: { name: 'bindCitiesList-template', foreach: visibleCities }"></ul>
                    <script type="text/html" id="bindCitiesList-template">
                        <li data-bind="text: cityName, attr: { 'cityId': cityId }, click: function (d, e) { $parent.chooseCity(d, e); }"></li>
                    </script>
                    <div class="margin-top30 font24 text-center margin-top60 "></div>
                </div>
            </div>

            <% } %>
        </div>

    </div>
    <div id="popup-loader-container">
        <div class="cover-popup-loader"></div>
        <div class="cover-popup-loader-text">
            <p data-bind="text: LoadingText()" class="font14"></p>
        </div>
    </div>
</div>

<script type="text/javascript">
    lscache.flushExpired(); 
    var BrandsKey = "BrandCityPopUp_";
    var BrandCityKey = "brandcity_";
    var cityId = '<%=cityId%>';
    var makeId = '<%=makeId%>';
    lscache.setBucket('BrandCitypopup');
    $("#changeOptions").click(function (e) {
        $('#brandcitypopupWrapper').addClass('loader-active');
        $('#brandcitypopupWrapper').show();
        $("#brandcitypopupContent").show();
        appendHash("searchbybrandcity");
        vmbrandcity.FillBrandsPopup();
       
    });

    $('#brandcitypopupWrapper .close-btn').click(function () {
        $("#brandcitypopupContent").hide();
        $('#brandcitypopupWrapper').removeClass('loader-active').hide();
    });


    var BrandCityPopup = function () {
        var self = this;
        self.selectCity = ko.observable(),
        self.SelectedMakeId = ko.observable(),
        self.listCities = ko.observableArray([]),
        self.SelectedCityId = ko.observable(cityId);
        self.selectedBrand = ko.observable(),
        self.bookingBrands = ko.observableArray([]),
        self.oBrowser = ko.observable(<%= (isOperaBrowser).ToString().ToLower()%>),
        self.makeApiUrl = "/api/makelist/?requesttype=" + '<%=(int)requestType%>',
        self.brandFilter = ko.observable(""),
        self.cityFilter = ko.observable(""),
        self.LoadingText = ko.observable("Loading..."),
        self.searchByBrandCityBtnClicked = ko.observable(false),
        self.makeMasking = ko.pureComputed(function () {
            return ko.utils.arrayFirst(self.bookingBrands(), function (child) {
                return child.makeId === self.selectedBrand().makeId;
            }).maskingName;
        });
        self.cityMasking = ko.pureComputed(function () {
            return ko.utils.arrayFirst(self.listCities(), function (child) {
                return child.cityId === self.selectCity().cityId;
            }).cityMaskingName;
        });
        self.cityApiUrl = ko.pureComputed(function () {
            if (<%=(requestType.Equals(Bikewale.Entities.BikeData.EnumBikeType.Dealer)).ToString().ToLower()%>)
                return ("/api/v2/DealerCity/?makeId=" + self.selectedBrand().makeId);
            else if (<%=(requestType.Equals(Bikewale.Entities.BikeData.EnumBikeType.ServiceCenter)).ToString().ToLower()%>)
                return ("/api/servicecenter/cities/make/" + self.selectedBrand().makeId + "/");
        });
        
        self.visibleCities = ko.computed(function () {
            filter = self.cityFilter();
            filterObj = self.listCities();
            if (filter && filter.length > 0) {
                var pat = new RegExp(filter, "i");
                filterObj = self.listCities().filter(function (i) {
                    if (pat.test(i.cityName)) return i;
                });
            }
            return filterObj;
        });


        self.visibleBrands = ko.computed(function () {
            filter = self.brandFilter();
            filterObj = self.bookingBrands();
            if (filter && filter.length > 0) {
                var pat = new RegExp(filter, "i");
                filterObj = self.bookingBrands().filter(function (i) {
                    if (pat.test(i.makeName)) return i;
                });
            }
            return filterObj;
        });

        self.FillBrandsPopup = function () {
            $('#brandcitypopupWrapper').addClass('loader-active');
            var isAborted = false;
            BrandsKey="BrandCityPopUp_"+'<%=requestType%>';
            if (data = lscache.get(BrandsKey)) {
                var brands = ko.toJS(data);
                if (brands) {
                    self.bookingBrands(brands);
                    if (self.bookingBrands() != null) {
                        isAborted = true;
                    }
                }
                else {
                    self.bookingBrands([]);
                }
                self.preselectMake();
            }
            if (!isAborted) {

                $.ajax({
                    type: "GET",
                    url: self.makeApiUrl,
                    dataType: 'json',
                    beforeSend: function (xhr) {
                        self.bookingBrands([]);
                        self.listCities([]);
                    },
                    success: function (response) {
                        var brands = ko.toJS(response);
                        if (brands) {
                            lscache.set(BrandsKey, brands.makes, 60);
                            self.bookingBrands(brands.makes);
                        }
                        else {
                            self.bookingBrands([]);
                        }
                    },
                    complete: function () {
                        self.preselectMake();
                    }
                });
            }
        };


        self.makeChangedPopup = function () {
            self.searchByBrandCityBtnClicked(false);
            var isAborted = false;
            if (self.selectedBrand() != undefined) {
                BrandCityKey = "brandcity_"+'<%=requestType%>'+'_' + self.selectedBrand().makeId;
                if (data = lscache.get(BrandCityKey)) {
                    var cities = ko.toJS(data);
                    if (cities) {
                        self.listCities(cities);
                       
                        isAborted = true;
                        if(self.SelectedCityId()!=null)
                        {
                            self.preselectCity();
                        }
                        else self.selectCity(null);
                    }
                    $('#brandcitypopupWrapper').removeClass('loader-active');
                    
                }
                
                if (!isAborted) {
                    $.ajax({
                        type: "GET",
                        url: self.cityApiUrl(),
                        dataType: 'json',
                        beforeSend: function (xhr) {
                            self.listCities([]);
                        },
                        success: function (response) {
                            var cities;
                            if (<%=(requestType.Equals(Bikewale.Entities.BikeData.EnumBikeType.Dealer)).ToString().ToLower()%>)
                            cities = ko.toJS(response.City);
                            else if (<%=(requestType.Equals(Bikewale.Entities.BikeData.EnumBikeType.ServiceCenter)).ToString().ToLower()%>)
                                cities = ko.toJS(response);
                            if (cities) {
                                lscache.set(BrandCityKey, cities, 60);
                                self.listCities(cities);
                                if(self.SelectedCityId()!=null )
                                {
                                    self.preselectCity();
                                }
                                else self.selectCity(null);
                            }         
                            
                            else {
                                self.listCities([]);
                            }
                            $('#brandcitypopupWrapper').removeClass('loader-active');
                           

                        },
                        error: function (e) {
                            self.listCities([]);
                        }
                    });
                }
            }
            else {
                self.listCities([]);
            }
        };

        self.isValidInfoPopup=function() {
            isValid = true;
            if (self.selectedBrand() == undefined && self.searchByBrandCityBtnClicked()) {
                isValid = false;
                self.setError($("#makeSelection"), 'Please select a brand');
                
            }
            if (self.bookingBrands().length > 0 && self.selectCity() == undefined && self.searchByBrandCityBtnClicked()) {
                isValid = false;
                self.setError($("#citiesSelection"), 'Please select a city');
             
            }
            if (isValid) {
                self.hideError($("#makeSelection"));
                self.hideError($("#citiesSelection"));
            }
            return isValid;
        };

        self.searchByBrandCityPopUp = function () {
            self.searchByBrandCityBtnClicked(true);
            isvalid = self.isValidInfoPopup();
            if (isvalid) {
                if (<%=(requestType.Equals(Bikewale.Entities.BikeData.EnumBikeType.Dealer)).ToString().ToLower()%>) {
                    window.location.href = "/m/"  + "dealer-showrooms/"+ self.makeMasking() +"/"+ self.cityMasking() + "/";
            }
            else if (<%=(requestType.Equals(Bikewale.Entities.BikeData.EnumBikeType.ServiceCenter)).ToString().ToLower()%>) {
                window.location.href = "/m/service-centers/" +self.makeMasking() + "/" + self.cityMasking() + "/" ;
        };
    }
    };

    self.preselectMake = function () {
        if (makeId > 0 && self.bookingBrands().length > 0) {
            self.selectedBrand(self.findMakeById(parseInt(makeId)));
            self.makeChangedPopup();
        }
    };
        self.preselectCity = function () {

            if (self.listCities().length > 0) {
                self.selectCity(self.findCityById(parseInt(self.SelectedCityId())));
            }
        };
        self.chooseBrand = function (data, event) {
           
            if (!self.oBrowser()) {
                $(".bwm-city-area-popup-wrapper .back-arrow-box").click();
                self.selectedBrand(data);
                self.makeChangedPopup();
                self.SelectedMakeId(data.id);
            }
            else {
                self.selectedBrand(findMakeById(self.SelectedMakeId()));
                if (!event.originalEvent) return;
            }

            if (self.selectedBrand() != null) {
                $('#brandcitypopupWrapper').addClass('loader-active');
                self.LoadingText("Loading cities for " + self.selectedBrand().makeName);
                self.makeChangedPopup();
            }
            
            
        };

        self.chooseCity = function (data, event) {
          if (!self.oBrowser()) {
                $(".bwm-city-area-popup-wrapper .back-arrow-box").click();
                self.selectCity(data);
                
                self.SelectedCityId(data.cityId);
            }
            else {
                self.selectCity(findCityById(self.SelectedCityId()));
                if (!event.originalEvent) return;
            }
          
        };

        self.findMakeById = function (id) {
            return ko.utils.arrayFirst(self.bookingBrands(), function (child) {
                if (child.id == id || child.makeId == id)
                    return child;
            });
        };
        
        self.findCityById = function (id) {
            return ko.utils.arrayFirst(self.listCities(), function (child) {
                if (child.id == id || child.cityId == id)
                    return child;
            });
        };

        self.setError = function (element, msg) {
            element.addClass("border-red");
            element.find("span.errorIcon, div.errorText").removeClass("hide");
            element.find("div.errorText").text(msg);
        };

        self.hideError = function (element) {
            element.removeClass("border-red");
            element.find("span.errorIcon, div.errorText").addClass("hide");

        };
             
    };
   

    $("#popupCitiesList").on("click", "li", function () {
        var self = $(this);
        self.addClass('activeBrand').siblings().removeClass('activeBrand');
    });
    $("#popupMakeList").on("click", "li", function () {
        var self = $(this);
        self.addClass('activeBrand').siblings().removeClass('activeBrand');
    });
    var vmbrandcity = new BrandCityPopup;
    ko.applyBindings(vmbrandcity, $("#brandcitypopupWrapper")[0]);

</script>
