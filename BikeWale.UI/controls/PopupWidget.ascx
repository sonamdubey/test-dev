<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.PopupWidget" %>
<script runat="server">
    private string staticUrl1 = System.Configuration.ConfigurationManager.AppSettings["staticUrl"];
    private string staticFileVersion1 = System.Configuration.ConfigurationManager.AppSettings["staticFileVersion"];
</script>

<!--bw popup code starts here-->
<script type="text/javascript">

    var pCityId = <%= CityId %>;
    var pAreaId = <%= AreaId %>;
    var onCookieObj = {};
</script>

<style type="text/css" >
    
.select-box {
  height: 60px;
  position: relative; }
  .select-box .select-label {
    position: absolute;
    top: 0;
    color: #82888b;
    font-size: 16px; }
  .select-box .chosen-container {
    height: 30px;
    position: relative;
    top: 0;
    padding: 0;
    border: 0;
    cursor: pointer;
    border-bottom: 1px solid #82888b;
    background: transparent url(https://imgd1.aeplcdn.com/0x0/bw/static/sprites/m/dropdown-icon.png) 100% 50% no-repeat;
    -webkit-border-radius: 0;
    -moz-border-radius: 0;
    -ms-border-radius: 0;
    border-radius: 0; }
    .select-box .chosen-container .chosen-single {
      color: #4d5057;
      font-size: 16px;
      font-weight: 700;
      background: transparent; }
    .select-box .chosen-container .chosen-default {
      display: none; }
    .select-box .chosen-container .chosen-search {
      padding: 0;
      border-bottom: 1px solid #41b4c4; }
      .select-box .chosen-container .chosen-search input[type=text] {
        border: 0;
        padding: 12px 10px;
        font-size: 16px; }
    .select-box .chosen-container .chosen-results {
      margin: 0;
      padding: 0;
      font-size: 14px; }
      .select-box .chosen-container .chosen-results::-webkit-scrollbar {
        width: 5px; }
      .select-box .chosen-container .chosen-results::-webkit-scrollbar {
        width: 5px; }
      .select-box .chosen-container .chosen-results::-webkit-scrollbar-track {
        background: #fff; }
      .select-box .chosen-container .chosen-results::-webkit-scrollbar-track {
        background: #fff; }
      .select-box .chosen-container .chosen-results::-webkit-scrollbar-thumb {
        background: #ddd; }
      .select-box .chosen-container .chosen-results::-webkit-scrollbar-thumb {
        background: #ddd; }
      .select-box .chosen-container .chosen-results li {
        padding: 8px 10px;
        transition: background .1s linear; }
      .select-box .chosen-container .chosen-results .highlighted {
        background-color: #f5f5f5; }
      .select-box .chosen-container .chosen-results .result-selected {
        font-weight: 700; }
    .select-box .chosen-container .chosen-drop {
      width: 350px;
      z-index: 1;
      top: -15px;
      border: 1px solid #e2e2e2;
      -webkit-box-shadow: 0 2px 10px 1px #ccc;
      -moz-box-shadow: 0 2px 10px 1px #ccc;
      -ms-box-shadow: 0 2px 10px 1px #ccc;
      -o-box-shadow: 0 2px 10px 1px #ccc;
      box-shadow: 0 2px 10px 1px #ccc;
      -webkit-border-radius: 2px;
      -moz-border-radius: 2px;
      -ms-border-radius: 2px;
      border-radius: 2px; }

.select-box.select-box-no-input .chosen-search {
  border-bottom: 1px solid #e2e2e2; }

.select-box.select-box-no-input .chosen-results li {
  padding-right: 20px;
  padding-left: 20px; }

.select-box.select-box-no-input .no-input-label {
  font-size: 16px;
  color: #4d5057;
  padding: 11px 20px;
  cursor: text; }

.done .select-label {
  font-size: 12px;
  top: -20px; }

.select-box .chosen-single:focus {
  outline: none; }

.chosen-disabled.single-version {
  opacity: 1 !important;
  background: none; }

input[type="text"]:focus,
input[type="number"]:focus {
  outline: none;
  box-shadow: none; }

.input-box {
  height: 60px;
  text-align: left; }
  .input-box input {
    width: 100%;
    display: block;
    padding: 7px 0;
    border-bottom: 1px solid #82888b;
    font-size: 16px;
    font-weight: 700;
    color: #4d5057; }
  .input-box label {
    position: absolute;
    top: 4px;
    left: 0;
    user-select: none;
    font-size: 16px;
    color: #82888b;
    cursor: text;
    -webkit-transition: 0.2s ease all;
    -moz-transition: 0.2s ease all;
    -o-transition: 0.2s ease all;
    transition: 0.2s ease all; }

    #popup-loader-container {
        position: fixed;
        border: 1px solid #ccc;
        width: 200px;
        min-height: 100px;
        top: 40%;
        left: 50%;
        background-color: white;
    }

  #popup-loader, .cover-popup-loader {
    height: 50px;
    background: url(https://imgd2.aeplcdn.com/0x0/bw/static/sprites/m/loader-red.gif) no-repeat center center;
    position: absolute;
    top: 45%;
    left: 5%;
    right: 5%;
    margin: 0 auto;
}

  #popup-loader-text, .cover-popup-loader-text {
    position: absolute;
    top: 60%;
    margin: 0 auto;
    left: 5%;
    right: 5%;
    text-align: center;
}

</style>

<link href="<%= !string.IsNullOrEmpty(staticUrl1) ? "https://st2.aeplcdn.com" + staticUrl1 : string.Empty %>/css/chosen.min.css?<%=staticFileVersion1 %>" rel="stylesheet" />
<script type="text/javascript" src="<%= !string.IsNullOrEmpty(staticUrl1) ? "https://st2.aeplcdn.com" + staticUrl1 : string.Empty %>/src/common/chosen.jquery.min.js?<%= staticFileVersion1 %>"></script>
<script type="text/javascript" src="<%= staticUrl1 != "" ? "https://st2.aeplcdn.com" + staticUrl1 : "" %>/src/knockout.validation.js?<%= staticFileVersion1 %>"></script>
<div id="priceQuoteWidget" class="hide">
           <!-- ko if : IsLoading() -->
        <div id="popup-loader-container">
            <div id="popup-loader"></div>
            <div id="popup-loader-text">
                <p data-bind="text : LoadingText()" class="font14"></p> 
            </div>
        </div>
        <!-- /ko -->
    <!-- ko ifnot : IsLoading() -->
    <div class="bw-popup bw-popup-sm">
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
                <div class="select-box margin-top10" data-bind="visible: BookingAreas().length > 0">
                   <p class="select-label">Area</p>
                    <select class="chosen-select" data-placeholder="Select area" id="ddlAreaPopup" tabindex="3"
                            data-bind="options: BookingAreas(), value: SelectedAreaId, optionsText: 'name', optionsValue: 'id',chosen: { width: '100%',search_contains: true }, event: { change:selectArea }">
                        </select>
                     <span class="boundary"></span>
                    <span class="error-text" data-bind="validationMessage: SelectedCity"></span>
                </div>  
                <%--<input id="btnDealerPricePopup" class ="action-btn margin-top15 margin-left70" style="display: block;" type="button" value="Show on-road price" data-bind="visible: SelectedCityId() > 0 && IsPersistance() && (!hasAreas() || SelectedAreaId() > 0),click: function(){ IsPersistance(false); InitializePQ();} "/>--%>
            </div>
       
    </div>
</div>
     <!-- /ko -->
</div>

<!--bw popup code ends here-->

<!-- widget script starts here-->
<script type="text/javascript">

    $(document).ready(function(){
        $(document).on("click",".blackOut-window,#priceQuoteWidget div.bw-popup div.close-btn",function(){ 
            $('.getquotation').removeClass('ui-btn-active');
            $("#popupContent,#priceQuoteWidget,.blackOut-window").hide();
        });

        var chosenSelectBox = $('.chosen-select');

        chosenSelectBox.each(function () {
            var text = $(this).attr('data-placeholder');
            $(this).siblings('.chosen-container').find('input[type=text]').attr('placeholder', text);
        });
 
    });

    $(document).on("click", ".getquotation", function (e) {
        var ele = $(this); e.stopPropagation();

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

    ko.validation.init({
        errorelementclass: 'invalid',
        insertmessages: false
    }, true);

    //for jquery chosen
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

        self.SelectedCity = ko.observable().extend({
            required: {
                params: true,
                message: 'Please select city',
                onlyIf: function () {
                    return self.validate();
                }
            }
        });

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

        self.InitializePQ = function (isLocChanged) {

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

                        }
                        else if (_responseData.priceQuote != null) {

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
                                window.location.href = "/pricequote/dealerpricequote.aspx" + "?MPQ=" + _responseData.qStr;
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
                        
                    }
                });
            }
        };


        self.selectCity = function (nVal) {

            try {
                self.SelectedCity(self.findItemById(self.BookingCities(),"id",self.SelectedCityId()));

                if (self.SelectedCity() != null &&  self.SelectedCity().id > 0) {
                    $('#ddlCitiesPopup').trigger("chosen:updated").parent().addClass('done');
                    if (!self.SelectedCity().hasAreas) {                       
                        self.LoadingText("Fetching on-road price for " + self.SelectedCity().name);
                        self.IsLoading(true);
                        self.IsPersistance(false);
                    }
                    else {
                        self.LoadingText("Loading areas for " + self.SelectedCity().name);
                    }

                    if (self.SelectedCity().id != onCookieObj.PQCitySelectedId) {
                        self.SelectedArea(null);
                        self.SelectedAreaId(0);
                        self.InitializePQ(true);
                        self.BookingAreas([]);
                    }
                    else{
                        self.IsLoading(false);
                        self.SelectedArea(null);
                        self.SelectedAreaId(0);
                        self.BookingAreas([]);
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
                        self.InitializePQ(true);
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

    var vmquotation = new mPopup;
    ko.applyBindings(vmquotation, $("#priceQuoteWidget")[0]);
    $('#ddlCitiesPopup,#ddlAreaPopup').chosen();


</script>
