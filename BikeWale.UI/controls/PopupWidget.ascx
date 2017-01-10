﻿<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.PopupWidget" %>
<script runat="server">
    private string staticUrl1 = System.Configuration.ConfigurationManager.AppSettings["staticUrl"];
    private string staticFileVersion1 = System.Configuration.ConfigurationManager.AppSettings["staticFileVersion"];
</script>

<!--bw popup code starts here-->
<div id="priceQuoteWidget" class="hide">     
    <div class="bw-popup bw-popup-sm" data-bind="css: IsLoading() ? 'location-loader-active' : ''">
        <!-- ko if : IsLoading() -->
        <div class="content-inner-block-20">
            <div class="location-loader-container">
                <div class="loader-flipper">
                    <div class="loader-front">
                        <span class="bwsprite red-marker"></span>
                    </div>
                    <div class="loader-back">
                        <span class="bwsprite black-marker"></span>
                    </div>
                </div>
            </div>
            <p data-bind="text : LoadingText()" class="font14"></p> 
        </div>
        <!-- /ko -->
        <!-- ko ifnot : IsLoading() -->
        <div class="popup-inner-container">
            <div class="bwsprite popup-close-btn close-btn position-abt pos-top10 pos-right10 cur-pointer"></div>
            <div class="cityPop-icon-container">
                <div class="icon-outer-container rounded-corner50 margin-bottom20">
                    <div class="icon-inner-container rounded-corner50">
                        <span class="bwsprite orp-location-icon margin-top20"></span>
                    </div>
                </div>
            </div>
            <p class="font20 margin-top15 text-capitalize text-center">Please Tell Us Your Location</p>
            <p class="text-light-grey margin-bottom15 margin-top15 text-capitalize text-center">Get on-road prices by just sharing your location!</p>
         
            <div class="padding-top10" id="popupContent">
                <div class="select-box margin-top10">
                   <p class="select-label">City</p>
                    <select class="chosen-select" data-placeholder="Select city" id="ddlCitiesPopup" tabindex="2"
                            data-bind="options: BookingCities(),value: SelectedCityId, optionsText: 'name', optionsValue: 'id',chosen: { width: '100%',search_contains: true },event : { change : selectCity }">
                        </select>
                        <span class="boundary"></span>
                    <span class="error-text" data-bind="validationMessage: SelectedCity"></span>
                </div> 
                <div id="area-dropdown-field" class="select-box margin-top10" data-bind="visible: SelectedCityId() > 0 && BookingAreas().length > 1 ">
                   <p class="select-label">Area</p>
                    <select class="chosen-select" data-placeholder="Select area" id="ddlAreaPopup" tabindex="3"
                         data-bind="options: BookingAreas(), value: SelectedAreaId, optionsText: 'name', optionsValue: 'id',chosen: { width: '100%',search_contains: true }, event: { change:selectArea }">
                    </select>
                    <span class="boundary"></span>
                    <span class="error-text" data-bind="validationMessage: SelectedCity"></span>
                    <p class="position-abt progress-bar"></p>
                    <p class="position-abt progress-bar-label"></p>
                </div>  
            </div>
       
        </div>
         <!-- /ko -->
    </div>
</div> 
<!--bw popup code ends here-->

<!-- widget script starts here-->
<script type="text/javascript">
    var onCookieObj = {};

    $(document).ready(function(){
        $(document).on("click",".blackOut-window, #priceQuoteWidget .close-btn",function(){
            pqPopupContent.close();
        });

        var chosenSelectBox = $('.chosen-select');

        chosenSelectBox.each(function () {
            var text = $(this).attr('data-placeholder');
            $(this).siblings('.chosen-container').find('input[type=text]').attr('placeholder', text);
        });
 
    });

    $(document).keydown(function(event){
        if(event.keyCode == 27){
            if($('#priceQuoteWidget').is(':visible')){
                pqPopupContent.close();
            }
        }
    });

    $(document).on("click", ".getquotation", function (e) {
        var ele = $(this); e.stopPropagation(); e.preventDefault();

        vmquotation.CheckCookies();
        var options = {
            "modelId": ele.attr('data-modelid'),
            "cityId": onCookieObj.PQCitySelectedId,
            "areaId": onCookieObj.PQAreaSelectedId,
            "city": (onCookieObj.PQCitySelectedId > 0) ? { 'id': onCookieObj.PQCitySelectedId, 'name': onCookieObj.PQCitySelectedName } : null,
            "area": (onCookieObj.PQAreaSelectedId > 0) ? { 'id': onCookieObj.PQAreaSelectedId, 'name': onCookieObj.PQAreaSelectedName } : null,
            "makename": ele.attr('data-makeName'),
            "modelname": ele.attr('data-modelName'),
            "pagecatid": ele.attr('data-pagecatid'),
            "pagesrcid": ele.attr('data-pqsourceid'),
            "ispersistent": ele.attr('data-persistent') != null ? true : false,
            "isreload": ele.attr("data-reload") != null ? true : false

        };

        options.cityId = ele.attr('data-preselcity') || options.cityId;

        if(options.modelId > 0)
        {
            $('#priceQuoteWidget,#popupContent,.blackOut-window').show();
            vmquotation.IsLoading(true);
            vmquotation.setOptions(options);
           
        }

    });

    ko.bindingHandlers.chosen = {
        init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
            var $element = $(element);
            var options = ko.unwrap(valueAccessor());
            if (typeof options === 'object')
                $element.chosen(options);

            ['options', 'selectedOptions', 'value'].forEach(function (propName) {
                if (allBindings.has(propName)) {
                    var prop = allBindings.get(propName);
                    if (ko.isObservable(prop)) {
                        prop.subscribe(function () {
                            $element.trigger('chosen:updated');
                        });
                    }
                }
            });
        }
    }

    var mPopup = function () {
        var self = this;
        self.MakeName = "";
        self.ModelName = "";
        self.PageCatId = "";
        self.PageSourceId = "";
        self.SelectedModelId = ko.observable();
        self.SelectedCity = ko.observable();
        self.SelectedArea = ko.observable();
        self.SelectedCityId = ko.observable(<%= CityId %>);
        self.SelectedAreaId = ko.observable(<%= AreaId %>);
        self.BookingCities = ko.observableArray([]);
        self.BookingAreas = ko.observableArray([]);
        self.IsPersistance = ko.observable(false);
        self.IsReload = ko.observable(false);
        self.cityFilter = ko.observable("");
        self.areaFilter = ko.observable("");
        self.hasAreas = ko.observable();
        self.LoadingText = ko.observable("Loading...");
        self.validate = ko.observable(false);
        self.IsLoading = ko.observable(false);

        var dummyOption = { id: 0, name: '', isPopular: false, hasAreas: false };

        self.setOptions = function (options, event) {
            if (options != null) {

                self.IsLoading(true);

                self.SelectedModelId(options.modelId || 0);
                self.SelectedCityId(options.cityId || 0);

                if (self.SelectedCityId() == 0)  self.LoadingText('Fetching Cities...');
                
                self.SelectedAreaId(options.areaId || 0);
                self.SelectedCity(options.city || null);
                self.SelectedArea(options.area || null);
                self.ModelName = options.modelname || "";
                self.MakeName = options.makename || "";
                self.PageCatId = options.pagecatid || "";
                self.PageSourceId = options.pagesrcid || "";
                self.IsPersistance(options.ispersistent || false);
                self.IsReload(options.isreload || false);

                if (self.IsPersistance())  self.LoadingText("Loading locations...");

                if (self.SelectedModelId()) {
                    self.InitializePQ();
                }

                gtmCodeAppender(self.PageCatId, "Get_On_Road_Price_Click", self.MakeName + self.ModelName);
            }
        };

        self.InitializePQ = function () {

            if (self.SelectedModelId() != null && self.SelectedModelId() > 0) {

                var objData = {
                    "CityId": self.SelectedCityId(),
                    "AreaId": self.SelectedAreaId(),
                    "ModelId": self.SelectedModelId(),
                    "ClientIP": "<%= ClientIP %>",
                    "SourceType": "1",
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
                                cities.splice(0, 0, dummyOption);
                                self.BookingCities(cities);
                                if (self.SelectedCityId() > 0) {
                                    self.SelectedCity(self.findItemById(self.BookingCities(),"id",self.SelectedCityId()));
                                    self.hasAreas((self.SelectedCity() != null && self.SelectedCity().hasAreas) ? true : false);
                                    var areas = ko.toJS(_responseData.pqAreas);
                                    progressBar.stopLoading('#area-dropdown-field');
                                    if (areas != null && areas.length > 0) {
                                        areas.splice(0, 0, dummyOption);
                                        self.BookingAreas(areas);
                                        if (self.SelectedAreaId() > 0) {
                                            self.SelectedArea(self.findItemById(self.BookingAreas(),"id",self.SelectedAreaId()));
                                        }
                                        else self.SelectedArea(null);
                                    }
                                    else{
                                        self.BookingAreas([]);
                                        self.SelectedArea(null);
                                        self.SelectedAreaId(0);
                                    }
                                }
                            }
                            self.IsLoading(false);
                            pqPopupContent.active();
                        }
                        else if (_responseData.priceQuote != null) {

                            var jsonObj = _responseData.priceQuote; 
                            gaLabel = GetGlobalCityArea() + ', ';

                            if (self.MakeName || self.ModelName)
                                gaLabel += self.MakeName + ',' + self.ModelName + ','; 

                            if (self.SelectedCityId() > 0) {
                                if (self.SelectedCity() && self.SelectedCity().id > 0) {
                                    lbtext  = "Fetching on-road price for " + self.SelectedCity().name;
                                    cookieValue = self.SelectedCity().id + "_" + self.SelectedCity().name;
                                    if (self.SelectedArea() && jsonObj.isDealerAvailable) {
                                        cookieValue += ("_" + self.SelectedArea().id + "_" + self.SelectedArea().name);
                                        lbtext = "Fetching on-road price for " + (self.SelectedArea().name + ", " + self.SelectedCity().name);
                                    }
                                    if (self.SelectedCityId() != onCookieObj.PQCitySelectedId || (self.SelectedAreaId() > 0 && jsonObj.isDealerAvailable))
                                        SetCookieInDays("location", cookieValue, 365);
                                    self.LoadingText(lbtext);
                                }
                            }

                            if (jsonObj.dealerId > 0)
                                gtmCodeAppender(self.PageCatId, 'Dealer_PriceQuote_Success_Submit', gaLabel);
                            else gtmCodeAppender(self.PageCatId, 'BW_PriceQuote_Success_Submit', gaLabel);


                            if (!self.IsReload() && _responseData.qStr != '') {
                                window.location = "/pricequote/dealerpricequote.aspx" + "?MPQ=" + _responseData.qStr;
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
                                        lbtext = "Fetching on-road price for " + (self.SelectedArea().name + ", " + self.SelectedCity().name);
                                    }
                                    if (self.SelectedCityId() != onCookieObj.PQCitySelectedId || self.SelectedAreaId() > 0)
                                        SetCookieInDays("location", cookieValue, 365);

                                    self.LoadingText(lbtext);
                                }

                            }
                            window.location.reload(true);
                        }
                    },
                    complete : function()
                    {                       
                        if(self.SelectedCityId() > 0)
                        {
                            $('#ddlCitiesPopup').parent().addClass('done');
                            if(self.SelectedAreaId() > 0)
                            {
                                $('#ddlAreaPopup').parent().addClass('done');
                            }
                        }
                        pqPopupContent.updateChosen( $('#ddlCitiesPopup'));
                        pqPopupContent.updateChosen( $('#ddlAreaPopup'));
                        
                    }
                });
            }
        };


        self.selectCity = function (nVal) {

            try {
                self.SelectedCity(self.findItemById(self.BookingCities(),"id",self.SelectedCityId()));

                if (self.SelectedCity() != null &&  self.SelectedCity().id > 0) {
                    $('#ddlCitiesPopup').parent().addClass('done');

                    self.SelectedArea(null);
                    self.SelectedAreaId(0);
                    self.BookingAreas([]);

                    pqPopupContent.updateChosen( $('#ddlAreaPopup'));

                    if (!self.SelectedCity().hasAreas) {                       
                        self.LoadingText("Fetching on-road price for " + self.SelectedCity().name);
                        self.IsLoading(true);
                        self.IsPersistance(false);
                    }
                    else {
                        self.BookingAreas.push(dummyOption);
                        progressBar.startLoading('#area-dropdown-field', 'Loading areas...');
                        self.LoadingText("Loading areas for " + self.SelectedCity().name);
                    }

                    if (self.SelectedCity().hasAreas || self.SelectedCity().id != onCookieObj.PQCitySelectedId) {
                        self.InitializePQ(); 
                    }
                    else{
                        self.IsLoading(false);
                    }

                    if (ga_pg_id != null && ga_pg_id == 2) {
                        var actText = '';
                        if (self.SelectedCity().hasAreas) {
                            actText = 'City_Selected_Has_Area';
                        }
                        else {
                            actText = 'City_Selected_Doesnt_Have_Area';
                        }
                        dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Model_Page', 'act': actText, 'lab': getBikeVersion() + '_' + self.SelectedCity().name });
                    }

                    
                }
            } catch (e) {
                console.warn(e);
            }

        };

        self.selectArea = function (data, event) {

            try {

                self.SelectedArea(self.findItemById(self.BookingAreas(),"id",self.SelectedAreaId()));

                if (self.SelectedArea()!=null && self.SelectedArea().id > 0) {
                    $('#ddlAreaPopup').trigger("chosen:updated").parent().addClass('done');
                    self.IsPersistance(false);

                    if (self.SelectedArea().id != onCookieObj.PQAreaSelectedId) {
                        self.LoadingText("Fetching on-road price for " + self.SelectedArea().name + ", " + self.SelectedCity().name);
                        self.IsLoading(true);
                        self.InitializePQ();
                    }

                    if (ga_pg_id != null && ga_pg_id == 2) {
                        dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Model_Page', 'act': 'Area_Selected', 'lab': self.MakeName + ' ' + self.ModelName + '_' + getBikeVersion() + '_' + self.SelectedCity().name + '_' + self.SelectedArea().name });
                    }
                }
            } catch (e) {
                console.warn(e);
            }

        };

        self.CheckCookies = function()
        {
            try {
                c = document.cookie.split('; ');
                for (i = c.length - 1; i >= 0; i--) {
                    C = c[i].split('=');
                    if (C[0] == "location") {
                        var cData = (String(C[1])).split('_');
                        onCookieObj.PQCitySelectedId = parseInt(cData[0]) || 0;
                        onCookieObj.PQCitySelectedName = cData[1] || "";
                        onCookieObj.PQAreaSelectedId = parseInt(cData[2]) || 0;
                        onCookieObj.PQAreaSelectedName = cData[3] || "";
                        break;
                    }
                }
            } catch (e) {
                console.warn(e);
            }
        };

        self.findItemById = function(items,attr,item)
        {
            return ko.utils.arrayFirst(items, function (child) {
                return (child[attr]==item);
            });
        };
    }


    function gtmCodeAppender(pageId, action, label) {
        var category = '';
        if (pageId != null) {
            switch (pageId) {
                case "1":
                    category = 'Make_Page';
                    break;
                case "2":
                    category = "CheckPQ_Series";
                    action = "CheckPQ_Series_" + action;
                    break;
                case "3":
                    category = "Model_Page";
                    action = "CheckPQ_Model_" + action;
                    break;
                case '4':
                    category = 'New_Bikes_Page';
                    break;
                case '5':
                    category = 'HP';
                    break;
                case '6':
                    category = 'Search_Page';
                    break;
            }
            if (label) {
                dataLayer.push({ 'event': 'Bikewale_all', 'cat': category, 'act': action, 'lab': label });
            }
            else {
                dataLayer.push({ 'event': 'Bikewale_all', 'cat': category, 'act': action });
            }
        }

    }

    var timeOutVariable;

    var pqPopupContent = {
        
        active: function(){
            timeOutVariable = setTimeout(function(){
                    $('#priceQuoteWidget').addClass('activate-popup-content');
            }, 200);
        },

        inactive: function(){
            $('#priceQuoteWidget').removeClass('activate-popup-content');
            clearTimeout(timeOutVariable);
        },

        updateChosen: function(element){
            $(element).trigger("chosen:updated");
        },

        close: function(){
            $('.getquotation').removeClass('ui-btn-active');
            $("#popupContent,#priceQuoteWidget,.blackOut-window").hide();
            pqPopupContent.inactive();
        }
    };

    var progressBar = {
        startLoading: function (element, message) {    
            try {
                var _self = $(element).find(".progress-bar").css({'width':'0'}).show();
                _self.animate({ width: '100%' }, 7000);
                $(element).find(".progress-bar-label").text(message);
            }
            catch (e) { return };
        },

        stopLoading: function(element){
            try {
                var _self = $(element).find(".progress-bar");
                _self.stop(true, true).css({'width':'100%'}).fadeOut(1000);
                $(element).find(".progress-bar-label").empty();
            }
            catch (e) { return };
        }
    }

    var vmquotation = new mPopup;
    ko.applyBindings(vmquotation, $("#priceQuoteWidget")[0]);
    $('#ddlCitiesPopup,#ddlAreaPopup').chosen();


</script>
