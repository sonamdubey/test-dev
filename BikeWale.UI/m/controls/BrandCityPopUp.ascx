<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Controls.BrandCityPopUp" %>
<script type="text/javascript">
    //lscache.flushExpired();  //remove expired
    var BrandsKey = "BrandCityPopUp_mk";
    var BrandCityKey = "brandcity_";
   </script>
<link href="<%= !string.IsNullOrEmpty(staticUrl) ? "http://st2.aeplcdn.com" + staticUrl : string.Empty %>/css/chosen.min.css?<%=staticFileVersion %>" rel="stylesheet" />



<!-- pricequote widget starts here-->
<div class="bw-city-popup bwm-fullscreen-popup bw-popup-sm text-center hide" id="brandcitypopupWrapper">
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
                <div class="selected-area" id="ddlBrandsPopup" data-bind="text: (selectedBrand() != null && selectedBrand().makeName != '') ? selectedBrand().makeName : 'Select Brand'">Select Brand</div>
                <span class="bwmsprite fa-angle-right position-abt pos-top10 pos-right10"></span>

            </div>
            <div id="citiesSelection" class="form-control text-left input-sm position-rel margin-bottom10"  data-bind="visible: listCities().length > 0">
                <div class="selected-city" id="ddlCityPopup" data-bind="text: (selectCity() != null && selectCity().cityName != '') ? selectCity().cityName : 'Select City'"></div>
                <span class="bwmsprite fa-angle-right position-abt pos-top10 pos-right10"></span>
            </div>

            <div id="btnSearchBrandCity" class="center-align margin-top20 text-center position-rel">
                <%if (requestType.Equals(Bikewale.Entities.BikeData.EnumBikeType.Dealer))
              { %>
                <a id="btnSearchBrandCityPopup" class="btn btn-orange btn-full-width font18" data-bind="click: searchByBrandCityPopUp, enable: (bookingBrands().length > 0) ">Search dealers</a>
            <%} %>
                <%else if (requestType.Equals(Bikewale.Entities.BikeData.EnumBikeType.ServiceCenter))
              {%>
                <a id="btnSearchBrandCityPopup" class="btn btn-orange btn-full-width font18" data-bind="click: searchByBrandCityPopUp, enable: (bookingBrands().length > 0) ">Search service centers</a>
            <%} %>
            </div>

            <div id="brandcitypopupContent" class="bwm-city-area-popup-wrapper">
                <div class="bw-city-popup-box bwm-city-area-box city-list-container form-control-box text-left">
                    <div class="user-input-box">
                        <span class="back-arrow-box">
                            <span class="bwmsprite back-long-arrow-left"></span>
                           

                        </span>
                        <input type="text" class="form-control padding-right40" placeholder="Type to select brand" id="popupCityInput" autocomplete="off" data-bind="textInput: brandFilter, attr: { value: (selectedBrand() != undefined) ? selectedBrand().makeName : '' }">
                    </div>
                    <ul id="popupMakeList" data-bind="template: { name: 'bindBrandList-template', foreach: visibleBrands }"></ul>
                    <script type="text/html" id="bindBrandList-template">
                        <li data-bind="text: makeName, attr: { 'makeId': id }, click: function (d, e) { $parent.chooseBrand(d, e); }"></li>
                    </script>
                    <div class="margin-top30 font24 text-center margin-top60 "></div>
                </div>

                <div class="bw-area-popup-box bwm-city-area-box area-list-container form-control-box text-left" data-bind="visible: listCities().length > 0">
                    <div class="user-input-box">
                        <span class="back-arrow-box">
                            <span class="bwmsprite back-long-arrow-left"></span>
                        </span>
                        <input type="text" class="form-control padding-right40" placeholder="Type to select city" id="popupAreaInput" autocomplete="off" data-bind="textInput: cityFilter, attr: { value: (selectCity() != undefined) ? selectCity().cityName : '' }">
                    </div>
                    <ul id="popupCitiesList" data-bind="template: { name: 'bindCitiesList-template', foreach: visibleCities }"></ul>
                    <script type="text/html" id="bindCitiesList-template">
                        <li data-bind="text: cityName, attr: { 'cityId': id }, click: function (d, e) { $parent.chooseCity(d, e); }"></li>
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

<script type="text/javascript">
    var requestType = '<%=requestType%>'; 
    debugger;
    $(document).on("click", "#changeOptions", function (e) {
        $('#brandcitypopupWrapper').addClass('loader-active');
        $('#brandcitypopupWrapper').show();
        $("#brandcitypopupContent").show();
        appendHash("searchbybrandcity");
        vmbrandcity.FillBrandsPopup();
       
    });

    $('#brandcitypopupWrapper .close-btn').click(function () {
        $('.getquotation').removeClass('ui-btn-active');
        $("#brandcitypopupContent").hide();
        $('#brandcitypopupWrapper').removeClass('loader-active').hide();
    });
   

    var mBrandCityPopup = function () {
        var self = this;
        self.selectCity = ko.observable(),
        self.SelectedCityId = ko.observable(),
        self.SelectedMakeId = ko.observable(),
        self.listCities = ko.observableArray([]),
        self.selectedBrand = ko.observable(),
        self.bookingBrands = ko.observableArray([]),
        self.hasCities = ko.observable(),
        self.oBrowser = ko.observable(<%= (isOperaBrowser).ToString().ToLower()%>),
        self.IsPersistance = ko.observable(false),
        self.IsReload = ko.observable(false),
        self.brandFilter = ko.observable(""),
        self.cityFilter = ko.observable(""),
        self.LoadingText = ko.observable("Loading..."),
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
       
        self.FilterData = function (data, filter) {
            filterObj = data;
            if (filter && filter.length > 0) {
                var pat = new RegExp(filter, "i");
                filterObj = data.filter(function (place) {
                    if (pat.test(place.name)) return place;
                });

            }
            return filterObj;
        };

        self.visibleCities = ko.computed(function () {
            return self.FilterData(self.listCities(), self.cityFilter());
        });

        self.visibleBrands = ko.computed(function () {
            return self.FilterData(self.bookingBrands(), self.brandFilter());
        });

       
        self.FillBrandsPopup = function () {
            $('#brandcitypopupWrapper').addClass('loader-active');
            debugger;
            if (self.bookingBrands().length < 1 || self.bookingBrands().length == undefined) {
                $.ajax({
                    type: "GET",
                    url: "/api/makelist/?requesttype=" + '<%=(int)requestType%>',
                    dataType: 'json',
                    beforeSend: function (xhr) {
                        self.bookingBrands([]);
                        self.listCities([]);
                        startLoading($("#makeSelection"));
                        $("#makeSelection .placeholder-loading-text").show();
                        //if (data = lscache.get(BrandsKey)) {
                        //    var brands = ko.toJS(data);
                        //    if (brands) {
                        //        stopLoading($("#makeSelection"));
                        //        $("#makeSelection .placeholder-loading-text").hide();
                        //        self.bookingBrands(brands);
                        //        isAborted = true;
                        //        xhr.abort();
                        //    }
                        //    else {
                        //        self.bookingBrands([]);
                        //    }
                        //}
                    },
                    success: function (response) {
                        var brands = ko.toJS(response);
                        if (brands) {
                            //lscache.set(BrandsKey, brands.makes, 60);
                            stopLoading($("#makeSelection"));
                            $("#makeSelection .placeholder-loading-text").hide();
                            self.bookingBrands(brands.makes);

                        }
                        else {
                            self.bookingBrands([]);
                            $('#ddlBrandsPopup').trigger("chosen:updated");
                        }
                        $('#brandcitypopupWrapper').removeClass('loader-active');
                    },
                    complete: function () {
                        self.completeBrandPopup();
                    }
                });

            }
        };

        self.completeBrandPopup = function () {
            $('#ddlBrandPopup').find("option[value='0']").prop('disabled', true);
            $('#ddlBrandPopup').trigger('chosen:updated');
            self.makeChangedPopup();
        };

        self.makeChangedPopup = function () {
             self.searchByBrandCityBtnClicked(false)
            if (self.selectedBrand() != undefined) {
                BrandCityKey = "brandcity_" + self.selectedBrand().toString();
                $.ajax({
                    type: "GET",
                    url: "/api/v2/DealerCity/?makeId=" + self.selectedBrand(),
                    dataType: 'json',
                    beforeSend: function (xhr) {
                        self.listCities([]);
                        $("#citySelection").show();
                        $("#citySelection .placeholder-loading-text").show();
                        startLoading($("#citySelection"));
                    //    if (data = lscache.get(BrandCityKey)) {
                    //        var cities = ko.toJS(data);
                    //        if (cities) {
                    //            stopLoading($("#citySelection"));
                    //            $("#citySelection .placeholder-loading-text").hide();
                    //            insertCitySeparatorNew(cities);
                    //            self.listCities(data);
                    //            isAborted = true;
                    //            xhr.abort();
                    //        }
                    //    }
                    },
                    success: function (response) {
                        var cities = ko.toJS(response.City);
                        if (cities != null)
                            if (cities.length) {
                                stopLoading($("#citySelection"));
                                $("#citySelection .placeholder-loading-text").hide();
                                insertCitySeparatorNew(cities);
                                self.listCities(cities);
                            }
                            else {
                                self.listCities([]);
                                $("#citySelection").hide();
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
                $("#citySelection").hide();
            }

            self.isValidInfoPopup();
        }


        self.completeCityPopup = function () {
            $('#ddlCityPopup').trigger("chosen:updated");
        };


        self.isValidInfoPopup = function () {
            isValid = true;
            var errMsg = "",
                errMsgParent;

            if (self.selectedBrand() == undefined && self.searchByBrandCityBtnClicked()) {
                errMsgParent = $('#makeSelection.form-control-box');
                errMsg += "Brand,";
                isValid = false;
                self.showBrandCityError(errMsgParent, errMsg);
            }

            else {
                self.removeBrandCityError($('#makeSelection.form-control-box'));
            }

            if (self.bookingBrands().length > 0 && self.selectCity() == undefined && self.searchByBrandCityBtnClicked()) {
                errMsgParent = $('#citySelection.form-control-box');
                errMsg += "City,";
                isValid = false;
                self.showBrandCityError(errMsgParent, errMsg);
            }

            else {
                self.removeBrandCityError($('#citySelection.form-control-box'));
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
                if (requestType == '<%=Bikewale.Entities.BikeData.EnumBikeType.Dealer%>') {
                    window.location.href = "/m/" + self.makeMasking() + "-dealer-showrooms-in-" + self.cityMasking() + "/";
                }
                else if (requestType == '<%=Bikewale.Entities.BikeData.EnumBikeType.ServiceCenter%>') {
                    window.location.href = "/m/" + self.makeMasking() + "-service-center-in-" + self.cityMasking() + "/";
                }
            }
        }


        self.chooseBrand = function (data, event) {

            if (!self.oBrowser()) {
                $(".bwm-city-area-popup-wrapper .back-arrow-box").click();
                self.selectedBrand(data);
                self.SelectedMakeId(data.id);
            }
            else {
                self.selectedBrand(findMakeById(self.SelectedMakeId()));
                if (!event.originalEvent) return;
            }

            if (self.selectedBrand() != null) {
                $('#city-area-content').addClass('city-selected');
                self.LoadingText("Loading cities for " + self.selectedBrand().name);
            }

        };

        self.chooseCity = function (data, event) {

            if (!self.oBrowser()) {
                $(".bwm-city-area-popup-wrapper .back-arrow-box").click();
                self.selectCity(data);
                self.SelectedCityId(data.id);
            }
            else {
                self.selectCity(findCityById(self.SelectedCityId()));
                if (!event.originalEvent) return;
            }

            self.IsPersistance(false);
       };
        self.findMakeById = function (id) {
            return ko.utils.arrayFirst(self.bookingBrands(), function (child) {
                return (child.id == id || child.makeId == id);
            });
        }

        self.findCityById = function (id) {
            return ko.utils.arrayFirst(self.listCities(), function (child) {
                return (child.id == id || child.cityId == id);
            });
        }


    };

        <%--self.InitializePQ = function (isLocChanged) {
            $('#popupWrapper').addClass('loader-active');

            if (self.SelectedModelId() != null && self.SelectedModelId() > 0) {

                var objData = {
                    "CityId": self.SelectedCityId(),
                    "AreaId": self.SelectedAreaId(),
                    "ModelId": self.SelectedModelId(),
                    "ClientIP": "<%= ClientIP %>",
                    "SourceType": "2",
                    "VersionId": 0,
                    "pQLeadId": self.PageSourceId,
                    'deviceId': getCookie('BWC'),
                    'isPersistance': self.IsPersistance(),
                    'refPQId': typeof pqId != 'undefined' ? pqId : '',
                    'isReload': self.IsReload()
                }

                $.ajax({
                    type: "POST",
                    url: "/api/generatepq/",
                    data: objData,
                    dataType: 'json',
                    beforeSend: function (xhr) {
                        xhr.setRequestHeader('utma', getCookie('__utma'));
                        xhr.setRequestHeader('utmz', getCookie('_bwutmz'));
                    },
                    success: function (response) {
                        var _responseData = ko.toJS(response);

                        if (_responseData && _responseData.pqCities && _responseData.pqCities.length > 0) {

                            var cities = ko.toJS(_responseData.pqCities);
                            if (cities != null && cities.length > 0) {
                                self.BookingCities(cities);
                                if (self.SelectedCityId() > 0) {
                                    self.SelectedCity(findCityById(self.SelectedCityId()));

                                    self.hasAreas((self.SelectedCity() != null && self.SelectedCity().hasAreas) ? true : false);

                                    var areas = ko.toJS(_responseData.pqAreas);
                                    if (areas) {
                                        self.BookingAreas(areas);
                                        if (self.SelectedAreaId() > 0) {
                                            self.SelectedArea(findAreaById(self.SelectedAreaId()));
                                        }
                                        else self.SelectedArea(null);
                                    }
                                }
                            }
                            $('#popupWrapper').removeClass('loader-active');

                        }
                        else if (_responseData.priceQuote != null) {

                            cityArea.close();

                            var jsonObj = _responseData.priceQuote;

                            gaLabel = GetGlobalCityArea() + ', ';

                            if (self.MakeName || self.ModelName)
                                gaLabel += self.MakeName + ',' + self.ModelName + ',';


                            if (self.SelectedCityId() > 0) {
                                if (self.SelectedCity() && self.SelectedCity().id > 0) {
                                    lbtext = "Fetching on-road price for " + self.SelectedCity().name;
                                    cookieValue = self.SelectedCity().id + "_" + self.SelectedCity().name;
                                    if (self.SelectedArea() && jsonObj.isDealerAvailable) {
                                        cookieValue += ("_" + self.SelectedArea().id + "_" + self.SelectedArea().name);
                                        lbtext = "Fetching on-road price for " + self.SelectedArea().name + ", " + self.SelectedCity().name;
                                    }
                                    SetCookieInDays("location", cookieValue, 365);

                                    self.LoadingText(lbtext);
                                }

                            }

                            if (jsonObj.dealerId > 0)
                                gtmCodeAppender(self.PageCatId, 'Dealer_PriceQuote_Success_Submit', gaLabel);
                            else gtmCodeAppender(self.PageCatId, 'BW_PriceQuote_Success_Submit', gaLabel);


                            if (!self.IsReload() && _responseData.qStr != '') {
                                //$('#popupWrapper').hide();
                                window.location.href = "/m/pricequote/dealerpricequote.aspx" + "?MPQ=" + _responseData.qStr;
                            }
                            else window.location.reload(true);
                        }
                        else {
                            if (self.SelectedCityId() > 0) {
                                if (self.SelectedCity() && self.SelectedCity().id > 0) {
                                    lbtext = "Fetching on-road price for " + self.SelectedCity().name;
                                    cookieValue = self.SelectedCity().id + "_" + self.SelectedCity().name;
                                    if (self.SelectedArea() && self.SelectedArea().id > 0) {
                                        cookieValue += ("_" + self.SelectedArea().id + "_" + self.SelectedArea().name);
                                        lbtext = "Fetching on-road price for " + self.SelectedArea().name + ", " + self.SelectedCity().name;
                                    }
                                    SetCookieInDays("location", cookieValue, 365);

                                    self.LoadingText(lbtext);
                                }

                            }
                            window.location.reload(true);
                        }
                    }
                });
            }
        };--%>





        

   

    //function gtmCodeAppender(pageId, action, label) {
    //    var category = '';
    //    if (pageId != null) {
    //        switch (pageId) {
    //            case "1":
    //                category = 'Make_Page';
    //                break;
    //            case "2":
    //                category = "CheckPQ_Series";
    //                action = "CheckPQ_Series_" + action;
    //                break;
    //            case "3":
    //                category = "Model_Page";
    //                action = "CheckPQ_Model_" + action;
    //                break;
    //            case '4':
    //                category = 'New_Bikes_Page';
    //                break;
    //            case '5':
    //                category = 'HP';
    //                break;
    //            case '6':
    //                category = 'Search_Page';
    //                break;
    //        }
    //        if (label) {
    //            dataLayer.push({ 'event': 'Bikewale_all', 'cat': category, 'act': action, 'lab': label });
    //        }
    //        else {
    //            dataLayer.push({ 'event': 'Bikewale_all', 'cat': category, 'act': action });
    //        }
    //    }

    //}

    //function checkCookies() {
    //    c = document.cookie.split('; ');
    //    for (i = c.length - 1; i >= 0; i--) {
    //        C = c[i].split('=');
    //        if (C[0] == "location") {
    //            var cData = (String(C[1])).split('_');
    //            onCookieObj.PQCitySelectedId = parseInt(cData[0]);
    //            onCookieObj.PQCitySelectedName = cData[1];
    //            onCookieObj.PQAreaSelectedId = parseInt(cData[2]);
    //            onCookieObj.PQAreaSelectedName = cData[3];
    //        }
    //    }
    //}

   
   
   // lscache.setBucket('BCPopup');
    //popupcity = $('#ddlCityPopup');
   // popupBrand = $('#ddlBrandPopup');
    <%--var viewModelCityBrandPopup = new function () {
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
                    //beforeSend: function (xhr) {
                    //    self.bookingBrands([]);
                    //    self.listCities([]);
                    //    startLoading($("#makeSelection"));
                    //    $("#makeSelection .placeholder-loading-text").show();
                    //    if (data = lscache.get(BrandsKey)) {
                    //        var brands = ko.toJS(data);
                    //        if (brands) {
                    //            stopLoading($("#makeSelection"));
                    //            $("#makeSelection .placeholder-loading-text").hide();
                    //            self.bookingBrands(brands);
                    //            isAborted = true;
                    //            xhr.abort();
                    //        }
                    //        else {
                    //            self.bookingBrands([]);
                    //        }
                    //    }
                    //},
                    success: function (response) {
                        var brands = ko.toJS(response);
                        if (brands) {
                            //lscache.set(BrandsKey, brands.makes, 60);
                            stopLoading($("#makeSelection"));
                            $("#makeSelection .placeholder-loading-text").hide();
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
            $('#ddlBrandPopup').find("option[value='0']").prop('disabled', true);
            $('#ddlBrandPopup').trigger('chosen:updated');
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
                    //beforeSend: function (xhr) {
                    //    self.listCities([]);
                    //    $("#citySelection").show();
                    //    $("#citySelection .placeholder-loading-text").show();
                    //    startLoading($("#citySelection"));
                    //    if (data = lscache.get(BrandCityKey)) {
                    //        var cities = ko.toJS(data);
                    //        if (cities) {
                    //            stopLoading($("#citySelection"));
                    //            $("#citySelection .placeholder-loading-text").hide();
                    //            insertCitySeparatorNew(cities);
                    //            self.listCities(data);
                    //            isAborted = true;
                    //            xhr.abort();
                    //        }
                    //    }
                    //},
                    success: function (response) {
                        var cities = ko.toJS(response.City);
                        if (cities != null)
                           // lscache.set(BrandCityKey, cities, 60);
                        if (cities.length) {
                            stopLoading($("#citySelection"));
                            $("#citySelection .placeholder-loading-text").hide();
                            insertCitySeparatorNew(cities);
                            self.listCities(cities);
                        }
                        else {
                            self.listCities([]);
                            $("#citySelection").hide();
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
                $("#citySelection").hide();
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
                errMsgParent = $('#makeSelection.form-control-box');
                errMsg += "Brand,";
                isValid = false;
                self.showBrandCityError(errMsgParent, errMsg);
            }

            else {
                self.removeBrandCityError($('#makeSelection.form-control-box'));
            }

            if (self.bookingBrands().length > 0 && self.selectCity() == undefined && self.searchByBrandCityBtnClicked()) {
                errMsgParent = $('#citySelection.form-control-box');
                errMsg += "City,";
                isValid = false;
                self.showBrandCityError(errMsgParent, errMsg);
            }

            else {
                self.removeBrandCityError($('#citySelection.form-control-box'));
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

    };--%>

    //$(document).ready(function () {
    //    $('body').on('click', "#changeOptions", function (e) {
    //        $('#popupWrapper').fadeIn(100);
    //        popup.lock();
    //        e.preventDefault();
    //        $("#errMsgPopUp").empty();
    //        viewModelCityBrandPopup.FillBrandsPopup();
    //    });

    //    $('#popupWrapper .close-btn, .blackOut-window').mouseup(function () {
    //        popup.unlock();
    //        $('#popupWrapper').fadeOut(100);
    //    });

    //    $("#ddlCityPopup").chosen({ no_results_text: "No matches found!!" });
    //    $("#ddlBrandPopup").chosen({ no_results_text: "No matches found!!" });
    //    $('.chosen-container').attr('style', 'width:100%;');

    //    ko.applyBindings(viewModelCityBrandPopup, $("#brandCityPopUpContent")[0]);
    //});
    var vmbrandcity = new mBrandCityPopup;
    ko.applyBindings(vmbrandcity, $("#brandcitypopupWrapper")[0]);

</script>
