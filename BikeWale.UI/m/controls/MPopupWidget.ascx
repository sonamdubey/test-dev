﻿<%@ Control Language="C#" AutoEventWireup="true" Inherits="Bikewale.Mobile.controls.MPopupWidget" %>
<script type="text/javascript">

    var pCityId = <%= CityId %>;
    var pAreaId = <%= AreaId %>;
    var onCookieObj = {};
</script>
 <!-- pricequote widget starts here-->
<div class="bw-city-popup bwm-fullscreen-popup bw-popup-sm text-center hide" id="popupWrapper">
    <div class="city-area-banner"></div>
    <div class="popup-inner-container">
        <div class="bwmsprite onroad-price-close-btn close-btn position-abt pos-top10 pos-right10 cur-pointer"></div>
        <div id="popupHeading" class="content-inner-block-20">
            <p class="font18 margin-bottom5 text-capitalize">Please Tell Us Your Location</p>
            <div class="text-light-grey margin-bottom5"><span class="red">*</span>Get on-road prices by just sharing your location!</div>
            <% if(isOperaBrowser) { %>

            <div class="form-control-box margin-bottom10 ">
                <select id="opCityList" class="form-control" tabindex="2" data-bind="options: BookingCities, value: SelectedCityId, optionsText: 'name', optionsValue: 'id', optionsCaption: '--Select City--', event: { change: function(d,e){selectCity(d,e);} }"></select>
                <span class="fa fa-spinner fa-spin position-abt  text-black btnSpinner"></span>
            </div>
            <div class="form-control-box" data-bind="visible: BookingAreas().length > 0">
                <select id="opAreaList" class="form-control" data-bind="options: BookingAreas, value: SelectedAreaId, optionsText: 'name', optionsValue: 'id', optionsCaption: '--Select Area--', event: { change: function (d,e) { selectArea(d,e); } }"></select>
                <span class="fa fa-spinner fa-spin position-abt  text-black btnSpinner"></span>
            </div>

            <% } else { %>

            <div id="citySelection" class="form-control text-left input-sm position-rel margin-bottom10">
                <div class="selected-city" data-bind="text: (SelectedCity() != null && SelectedCity().name != '') ? SelectedCity().name : 'Select City'"></div>
                <span class="bwmsprite fa-angle-right position-abt pos-top10 pos-right10"></span>
            </div>
            <div id="areaSelection" class="form-control text-left input-sm position-rel margin-bottom10 " data-bind="visible: BookingAreas().length > 0">
                <div class="selected-area" data-bind="text: (SelectedArea() != null && SelectedArea().name != '') ? SelectedArea().name : 'Select Area'">Select Area</div>
                <span class="bwmsprite fa-angle-right position-abt pos-top10 pos-right10"></span>

            </div>

            <div id="btnPriceLoader" class="center-align margin-top20 text-center position-rel">
                <a id="btnDealerPricePopup" class="btn btn-orange btn-full-width font18" data-bind="visible: SelectedCityId() > 0 && IsPersistance() && (!hasAreas() || SelectedAreaId() > 0),click: function(){ IsPersistance(false); InitializePQ();} ">Show on-road price</a>
            </div>

            <div id="popupContent" class="bwm-city-area-popup-wrapper">
                <div class="bw-city-popup-box bwm-city-area-box city-list-container form-control-box text-left">
                    <div class="user-input-box">
                        <span class="back-arrow-box">
                            <span class="bwmsprite back-long-arrow-left"></span>
                            <%--<span class="bwmsprite cross-md-dark-grey"></span>--%>
                        </span>
                        <input type="text" class="form-control padding-right40" placeholder="Type to select city" id="popupCityInput" autocomplete="off" data-bind="textInput: cityFilter,attr: { value: (SelectedCity() != undefined) ? SelectedCity().name : '' }">
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
                        <input type="text" class="form-control padding-right40" placeholder="Type to select area" id="popupAreaInput" autocomplete="off" data-bind="textInput: areaFilter,attr: { value: (SelectedArea() != undefined) ? SelectedArea().name : '' }">
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
            <p data-bind="text : LoadingText()" class="font14"></p> 
        </div>
    </div>
</div>
<!-- pricequote widget ends here-->

<!-- widget script starts here-->
<script type="text/javascript">

    $('#popupWrapper .close-btn').click(function () {        
        $('.getquotation').removeClass('ui-btn-active');
        $("#popupContent").hide();
        $('#popupWrapper').removeClass('loader-active').hide();
    });

    $(document).on("click", ".getquotation", function (e) {
        var ele = $(this); e.stopPropagation();

        checkCookies();
        var options = {
            "modelId": ele.attr('data-modelid') ,
            "cityId": onCookieObj.PQCitySelectedId,
            "areaId": onCookieObj.PQAreaSelectedId,
            "city" : (onCookieObj.PQCitySelectedId > 0)?{ 'id': onCookieObj.PQCitySelectedId, 'name': onCookieObj.PQCitySelectedName }:null,
            "area" : (onCookieObj.PQAreaSelectedId > 0)?{ 'id': onCookieObj.PQAreaSelectedId, 'name': onCookieObj.PQAreaSelectedName }:null,
            "makename" : ele.attr('data-makeName'),
            "modelname" : ele.attr('data-modelName'),
            "pagecatid" : ele.attr('data-pagecatid'),
            "pagesrcid" : ele.attr('data-pqSourceId'),
            "ispersistent" : ele.attr('data-persistent') !=null ? true : false,
            "isreload" : ele.attr("data-reload") !=null ? true : false 

        };
        
        options.cityId = ele.attr('data-preselcity') || options.cityId;

        $('#popupWrapper').addClass('loader-active');
        $('#popupWrapper').show();
        $("#popupContent").show();
        appendHash("onRoadPrice");
        vmquotation.setOptions(options);

    });

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
        self.oBrowser = ko.observable(<%= (isOperaBrowser).ToString().ToLower()%>);
        self.IsPersistance = ko.observable(false);
        self.IsReload = ko.observable(false);
        self.cityFilter =  ko.observable("");
        self.areaFilter = ko.observable("");
        self.hasAreas = ko.observable();
        self.LoadingText = ko.observable("Loading...");

        self.FilterData = function (data,filter)
        {
            filterObj = data;
            if(filter && filter.length > 0)
            {
                var pat = new RegExp(filter,"i");
                filterObj = data.filter(function(place){
                    if(pat.test(place.name)) return place;
                });

            }
            return filterObj;
        };

        self.visibleCities = ko.computed(function(){
            return self.FilterData(self.BookingCities(),self.cityFilter());  		   
        });

        self.visibleAreas = ko.computed(function(){
            return self.FilterData(self.BookingAreas(),self.areaFilter());  		   
        });	

        self.setOptions = function (options,event) {
            if (options != null) {

                if (options.modelId != null)
                    self.SelectedModelId(options.modelId);

                if (options.cityId != null)
                {
                    self.SelectedCityId(options.cityId);
                }else{
                    self.LoadingText('Fetching Cities...');
                }
                   

                if (options.areaId != null)
                    self.SelectedAreaId(options.areaId);

                if (options.city != null)
                    self.SelectedCity(options.city);

                if (options.area != null)
                    self.SelectedArea(options.area);

                if (options.modelname != null)
                    self.ModelName = options.modelname;

                if (options.makename != null)
                    self.MakeName = options.makename;

                if (options.pagecatid != null)
                    self.PageCatId = options.pagecatid;

                if (options.pagesrcid != null)
                    self.PageSourceId = options.pagesrcid;
                        
                if (options.ispersistent != null)
                {
                    self.IsPersistance(options.ispersistent);
                    self.LoadingText("Loading locations...");
                }
                   

                if (options.isreload != null)
                    self.IsReload(options.isreload);


                if(self.SelectedModelId())
                {
                    self.InitializePQ();
                }

                gtmCodeAppender(self.PageCatId, "Get_On_Road_Price_Click", self.MakeName + self.ModelName);
            }
        };

        self.InitializePQ = function(isLocChanged)
        { 
            $('#popupWrapper').addClass('loader-active');

            if (self.SelectedModelId() != null && self.SelectedModelId()  > 0) {

                var objData = {
                    "CityId":self.SelectedCityId(),
                    "AreaId":self.SelectedAreaId(),
                    "ModelId":self.SelectedModelId(),
                    "ClientIP":"<%= ClientIP %>",
                    "SourceType":"2",
                    "VersionId":0,
                    "pQLeadId":self.PageSourceId,
                    'deviceId': getCookie('BWC'),
                    'isPersistance' : self.IsPersistance()  ,
                    'refPQId': typeof pqId != 'undefined' ? pqId : '',
                    'isReload' : self.IsReload()
                        }

                        $.ajax({
                            type: "POST",
                            url: "/api/generatepq/",
                            data : objData, 
                            dataType: 'json',
                            beforeSend: function (xhr) {
                                xhr.setRequestHeader('utma', getCookie('__utma'));
                                xhr.setRequestHeader('utmz', getCookie('_bwutmz'));
                            },
                            success: function (response) {
                                var _responseData = ko.toJS(response);
                                
                                if (_responseData && _responseData.pqCities && _responseData.pqCities.length > 0) {									

                                    var cities = ko.toJS(_responseData.pqCities);
                                    if (cities!=null && cities.length > 0) {
                                        self.BookingCities(cities);
                                        if(self.SelectedCityId() > 0)
                                        {
                                            self.SelectedCity(findCityById(self.SelectedCityId()));
                                        
                                            self.hasAreas((self.SelectedCity() != null && self.SelectedCity().hasAreas) ? true : false);

                                            var areas = ko.toJS(_responseData.pqAreas);
                                            if (areas) {
                                                self.BookingAreas(areas);
                                                if(self.SelectedAreaId() > 0)
                                                {
                                                    self.SelectedArea(findAreaById(self.SelectedAreaId()));
                                                }
                                                else self.SelectedArea(null);
                                            }                                                                         
                                        }
                                    }
                                    $('#popupWrapper').removeClass('loader-active');

                                }
                                else  if(_responseData.priceQuote != null){
                                    
                                    cityArea.close();
                                   
                                    var jsonObj = _responseData.priceQuote;

                                    gaLabel = GetGlobalCityArea() + ', ';

                                    if (self.MakeName || self.ModelName )
                                        gaLabel += self.MakeName + ',' + self.ModelName + ',';
                                    

                                    if (self.SelectedCityId() > 0 ) {                                       
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

                                    if(jsonObj.dealerId > 0)
                                        gtmCodeAppender(self.PageCatId, 'Dealer_PriceQuote_Success_Submit', gaLabel);
                                    else gtmCodeAppender(self.PageCatId, 'BW_PriceQuote_Success_Submit', gaLabel); 
                                    

                                    if(!self.IsReload() && _responseData.qStr!='')
                                    {                                          
                                        $('#popupWrapper .close-btn').click();
                                        window.location.href = "/m/pricequote/dealerpricequote.aspx" + "?MPQ=" + _responseData.qStr; 
                                    }                                        
                                    else   window.location.reload(true);   
                                }
                                else
                                {
                                    if (self.SelectedCityId() > 0 ) {                                       
                                        if (self.SelectedCity() && self.SelectedCity().id > 0) {
                                            lbtext = "Fetching on-road price for " + self.SelectedCity().name;
                                            cookieValue = self.SelectedCity().id + "_" + self.SelectedCity().name; 
                                            if (self.SelectedArea() && self.SelectedArea().id > 0 ) {
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
                };

                
        self.selectCity = function (data, event) {
            
            if (!self.oBrowser()) {
                $(".bwm-city-area-popup-wrapper .back-arrow-box").click();                 
                self.SelectedCity(data);
                self.SelectedCityId(data.id);
            }
            else {
                self.SelectedCity(findCityById(self.SelectedCityId()));
                if(!event.originalEvent) return;
            }

            if(self.SelectedCity()!=null && !self.SelectedCity().hasAreas)
            {
                $('#city-area-content').addClass('city-selected');
                self.LoadingText("Fetching on-road price for " + self.SelectedCity().name);
                self.IsPersistance(false);
            }
            else{
                self.LoadingText("Loading areas for " + self.SelectedCity().name);  
            }
            
            if(data.id != onCookieObj.PQCitySelectedId){ 
                self.SelectedArea(null);
                self.SelectedAreaId(0);
                self.InitializePQ(true);
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

        };

        self.selectArea = function (data, event) {

            if (!self.oBrowser()) {                
                $(".bwm-city-area-popup-wrapper .back-arrow-box").click();
                self.SelectedArea(data);
                self.SelectedAreaId(data.id);
            }
            else {
                self.SelectedArea(findAreaById(self.SelectedAreaId()));
                if(!event.originalEvent) return;
            }

            self.IsPersistance(false);

            if(data.id != onCookieObj.PQAreaSelectedId){
                self.LoadingText("Fetching on-road price for " + self.SelectedArea().name + ", " + self.SelectedCity().name);
                self.InitializePQ(true);
            }

            if (ga_pg_id != null && ga_pg_id == 2 ) {
                dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Model_Page', 'act': 'Area_Selected', 'lab': self.MakeName + ' ' + self.ModelName + '_' + getBikeVersion() + '_' + self.SelectedCity().name + '_' + self.SelectedArea().name });
            }


        };
    }


            function findAreaById(id) {
                return ko.utils.arrayFirst(vmquotation.BookingAreas(), function (child) {
                    return (child.id == id || child.areaId == id);
                });
            }

            function findCityById(id) {
                return ko.utils.arrayFirst(vmquotation.BookingCities(), function (child) {
                    return (child.id == id || child.cityId == id);
                });
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

            function checkCookies() {
                c = document.cookie.split('; ');
                for (i = c.length - 1; i >= 0; i--) {
                    C = c[i].split('=');
                    if (C[0] == "location") {
                        var cData = (String(C[1])).split('_');
                        onCookieObj.PQCitySelectedId = parseInt(cData[0]);
                        onCookieObj.PQCitySelectedName = cData[1];
                        onCookieObj.PQAreaSelectedId = parseInt(cData[2]);
                        onCookieObj.PQAreaSelectedName = cData[3];
                    }
                }
            }

            var vmquotation = new mPopup;
            ko.applyBindings(vmquotation, $("#popupWrapper")[0]);

</script>
<!-- widget script ends here-->
