<%@ Control Language="C#" AutoEventWireup="true" Inherits="Bikewale.Mobile.controls.MPopupWidget" %>
<script type="text/javascript">
    lscache.flushExpired(); 
    var modelCityKey = "mc_";
    var cityAreaKey = "ca_";
    var pCityId = <%= CityId %>;
    var pAreaId = <%= AreaId %>;
    var onCookieObj = {};
</script>

<!-- pricequote widget starts here-->
<div id="popupWrapper">
    <div id="city-area-popup" class="bwm-fullscreen-popup">
        <div class="header-fixed fixed">
            <div class="leftfloat header-back-btn">
                <a href="javascript:void(0)" rel="nofollow"><span class="bwmsprite white-back-arrow"></span></a>
            </div>
            <div class="leftfloat header-title text-bold text-white font18">Select location</div>
            <div class="clear"></div>
        </div>
        <div class="city-area-banner"></div>
        <div id="city-area-content">
             <% if(!isOperaBrowser) { %>
            <div id="city-menu" class="city-area-menu open">
                <div id="city-menu-tab" class="city-area-tab cursor-pointer">
                    <span class="city-area-tab-label" data-bind="text: (SelectedCity() != undefined && SelectedCity().name != '') ? 'City : ' + SelectedCity().name : 'Select your city'"></span>
                    <span class="chevron bwmsprite chevron-down"></span>
                </div>
                <div class="inputbox-list-wrapper">
                    <div class="form-control-box user-input-box">
                        <span class="bwmsprite search-icon-grey"></span>
                        <input type="text" class="form-control padding-right40" placeholder="Type to select city" id="city-menu-input" autocomplete="off"><!-- data-bind="textInput: cityFilter" -->
                        <span class="fa fa-spinner fa-spin position-abt text-black"></span>
                    </div>
                    <ul id="city-menu-list" data-bind="template: { name: 'bindCityList-template', foreach: visibleCities }" ></ul>
                    <script type="text/html" id="bindCityList-template">
                         <li data-bind="text: name, attr: { 'cityId': id }, click: function (d, e) { $parent.selectCity(d, e); }"></li>
                    </script>
                </div>
            </div>  
            <div id="area-menu" class="city-area-menu">
                <div id="area-menu-tab" class="city-area-tab">
                    <span class="city-area-tab-label" data-bind="text: (SelectedArea() != undefined && SelectedArea().name != '') ? 'Area : ' + SelectedArea().name : 'Select your area'"></span>
                </div>
                <div class="inputbox-list-wrapper">
                    <div class="form-control-box user-input-box">
                        <span class="bwmsprite search-icon-grey"></span>
                        <input type="text" class="form-control padding-right40" placeholder="Type to select area" id="area-menu-input" autocomplete="off"><!-- data-bind="textInput: areaFilter" -->
                        <span class="fa fa-spinner fa-spin position-abt text-black"></span>
                    </div> 
                     <ul id="area-menu-list" data-bind="template: { name: 'bindAreaList-template', foreach: visibleAreas }" ></ul>
                    <script type="text/html" id="bindAreaList-template">
                         <li data-bind="text: name, attr: { 'areaId': id }, click: function (d, e) { $parent.selectArea(d, e); }"></li>
                    </script>
                </div>
            </div>
            <% } else {%>

             <div class="form-control-box margin-bottom10 ">
                <select class="form-control" tabindex="2" data-bind="options: BookingCities, value: SelectedCityId, optionsText: 'name', optionsValue: 'id', optionsCaption: '--Select City--', event: { change: selectCity }"></select>
                <span class="fa fa-spinner fa-spin position-abt  text-black btnSpinner"></span>
            </div>
            <div class="form-control-box" data-bind="visible: BookingAreas().length > 0">
                <select class="form-control" data-bind="options: BookingAreas, value: SelectedAreaId, optionsText: 'name', optionsValue: 'id', optionsCaption: '--Select Area--', event: { change: function (data, event) { selectArea(data, event); } }"></select>
                <span class="fa fa-spinner fa-spin position-abt  text-black btnSpinner"></span>
            </div>

            <% } %>
        </div>
    </div>
</div>
<!-- pricequote widget ends here-->

<!-- widget script starts here-->
<script type="text/javascript">

    $('#popupWrapper .close-btn,.blackOut-window').click(function () {
        $('.bw-city-popup').fadeOut(100);
        $('body').removeClass('lock-browser-scroll');
        $(".blackOut-window").hide();
        $('a.fillPopupData').removeClass('ui-btn-active');
        stopLoading($('#top-progress-bar'));
    });

    $(document).on("click", ".getquotation", function (e) {
        var ele = $(this); e.stopPropagation();
        startLoading($('#top-progress-bar'));
        checkCookies();
        var options = {
            "modelId": ele.attr('modelId') ,
            "cityId": onCookieObj.PQCitySelectedId,
            "areaId": onCookieObj.PQAreaSelectedId,
            "makename" : ele.attr('makeName'),
            "modelname" : ele.attr('modelName'),
            "pagecatid" : ele.attr('pagecatid'),
            "pagesrcid" : ele.attr('pqSourceId'),
            "ispersistent" : ele.attr('ismodel') !=null ? true : false,
            "isreload" : ele.attr("data-reload") !=null ? true : false 

        }; 
				
        vmquotation.setOptions(options);
        appendHash("onRoadPrice");

    });

    var showPQPopup = function(isCitySelected) {
        var tab = $('#city-area-content #city-menu-tab'),
        tabParent = tab.parent('.city-area-menu'),
        cityAreaContent = $('#city-area-content');
        var areaMenu = $('#area-menu');

        if (cityAreaContent.hasClass('city-selected')) {
            var areaMenu = $('#area-menu');
        }

        cityArea.open();
        cityArea.openList(tabParent);
        cityArea.closeList(areaMenu);
        areaMenu.hide();
        if(isCitySelected)
        {
            $('#city-area-content #city-menu-tab').click();
        }			    
    };

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
        self.oBrowser = ko.observable(<%= isOperaBrowser.ToString().ToLower()%>);
		self.IsPersistance = ko.observable(false);
		self.IsReload = ko.observable(false);
		self.cityFilter =  ko.observable("");
		self.areaFilter = ko.observable("");

		self.FilterData = function (data,filter)
		{
		    filterObj = data;
		    if(filter && filter.length > 0)
		    {
		        var pat = new RegExp(filter,"i");
		        filterObj = data.filter(function(place){
		            if(pat.test(place.name)) return place;
		            //filterObj = ko.utils.arrayFilter(data, function(item) {
		            //     if(pat.test(item.name)) return item;

		        });

		    }
		    return filterObj;
		}

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
		            self.SelectedCityId(options.cityId);

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
		            self.IsPersistance(options.ispersistent);

		        if (options.isreload != null)
		            self.IsReload(options.isreload);


		        if(self.SelectedModelId())
		        {
		            self.InitializePQ();
		        }

		        gtmCodeAppender(self.PageCatId, "Get_On_Road_Price_Click", self.MakeName + self.ModelName);
		    }
		};

		self.InitializePQ = function(data,event)
		{
		    var isAborted = false;

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
                                        $('#city-menu-input').fastLiveFilter('#city-menu-list');
                                    }									
                                    if(self.SelectedCityId() > 0)
                                    {
                                        self.SelectedCity(findCityById(self.SelectedCityId()));
                                        
                                        $('#city-area-content').addClass('city-selected');
                                        var hasAreas = (self.SelectedCity() != null && self.SelectedCity().hasAreas) ? true : false;
                                        if(!hasAreas) {
                                            $('#city-area-content').removeClass('city-selected'); 									        
                                        }
                                        if(self.SelectedAreaId() > 0)
                                        {
                                            self.SelectedArea(findAreaById(self.SelectedAreaId()));
                                        }                                 
                                    }

                                    var areas = ko.toJS(_responseData.pqAreas);
                                    if (areas) {
                                        self.BookingAreas(areas);
                                        $('#area-menu-input').fastLiveFilter('#area-menu-list');
                                    }

                                    showPQPopup(self.SelectedCityId() > 0 );

                                }
                                else  if(_responseData.priceQuote != null){
                                    
                                    cityArea.close();
                                   
                                    var jsonObj = _responseData.priceQuote;

                                    gaLabel = GetGlobalCityArea() + ', ';

                                    if (self.MakeName != undefined || self.ModelName != undefined)
                                        gaLabel = self.MakeName + ',' + self.ModelName + ',';				                    

                                    var queryStr = "CityId=" + self.SelectedCityId() + "&AreaId=" + (!isNaN(self.SelectedAreaId()) ? self.SelectedAreaId() : 0) + "&PQId=" + jsonObj.quoteId + "&VersionId=" + jsonObj.versionId + "&DealerId=" + jsonObj.dealerId;

                                    if(jsonObj.dealerId > 0)
                                        gtmCodeAppender(self.PageCatId, 'Dealer_PriceQuote_Success_Submit', gaLabel);
                                    else gtmCodeAppender(self.PageCatId, 'BW_PriceQuote_Success_Submit', gaLabel);

                                    if(!self.IsReload())
                                        window.location = "/m/pricequote/dealerpricequote.aspx" + "?MPQ=" + Base64.encode(queryStr);
                                    else   window.location.reload(); 
                                }
                                else
                                {
                                    window.location.reload();
                                }
                                
                            },
                            complete: function (xhr) {
                                if(xhr.status == 200 && !xhr.priceQuote) {
                                    if(self.SelectedCity())
                                    {
                                        cookieValue = self.SelectedCity().id + "_" + self.SelectedCity().name;

                                        self.SelectedArea(findAreaById(self.SelectedAreaId()));

                                        if (self.SelectedArea() != null) {
                                            cookieValue += ("_" + self.SelectedArea().id + "_" + self.SelectedArea().name);
                                        }

                                        SetCookieInDays("location", cookieValue, 365);
                                    }

                                }
                                stopLoading($('#top-progress-bar'));
                            }
                        });
                    }
				};

				
        self.selectCity = function (data, event) {
            startLoading($('#top-progress-bar'));
            var isAborted = false;
            if (!self.oBrowser()) {
                self.SelectedCity(data);
                self.SelectedCityId(data.id);
            }
            else {
                self.SelectedCity(findCityById(self.SelectedCityId()));
            }

            if(self.SelectedCity()!=null && !self.SelectedCity().hasAreas)
            {
                $('#city-area-content').addClass('city-selected');
                self.IsPersistance(false); 						
            }
					

            self.InitializePQ(data,event);

				
            //-------------------------------------------------------------------------------------------
            //ev = $._data($('ul#popupCityList')[0], 'events');
            //if (!(ev && ev.click)) {
            //	$('ul#popupCityList').on('click', 'li', function (e) {
            //		if (ga_pg_id != null && ga_pg_id == 2 && cityClicked == false) {
            //			var actText = '';
            //			if (self.SelectedCity().hasAreas) {
            //				actText = 'City_Selected_Has_Area';
            //			}
            //			else {
            //				actText = 'City_Selected_Doesnt_Have_Area';
            //			}
            //			dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Model_Page', 'act': actText, 'lab': getBikeVersion() + '_' + self.SelectedCity().name });
            //			cityClicked = true;
            //		}
            //	});
            //}

        };

        self.selectArea = function (data, event) {
            if (!self.oBrowser()) {
                self.SelectedArea(data);
                self.SelectedAreaId(data.id);
            }
            else {
                self.SelectedArea(findAreaById(self.SelectedAreaId()));
            }
            //if (ga_pg_id != null && ga_pg_id == 2 && areaClicked == false) {
            //	dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Model_Page', 'act': 'Area_Selected', 'lab': myBikeName + '_' + getBikeVersion() + '_' + self.SelectedCity().name + '_' + self.SelectedArea().name });
            //	areaClicked = true;
            //}

            self.IsPersistance(false);
            self.InitializePQ(data,event);

        };

				

    }


            function findAreaById(id) {
                return ko.utils.arrayFirst(vmquotation.BookingAreas(), function (child) {
                    return (child.id === id || child.areaId === id);
                });
            }

            function findCityById(id) {
                return ko.utils.arrayFirst(vmquotation.BookingCities(), function (child) {
                    return (child.id === id || child.cityId === id);
                });
            }

            function startLoading(ele) {
                try {
                    var _self = $(ele).find(".progress-bar").css({ 'width': '0' }).show();
                    _self.animate({ width: '100%' }, 7000);
                }
                catch (e) { return };
            }

            function stopLoading(ele) {
                try {
                    var _self = $(ele).find(".progress-bar");
                    _self.stop(true, true).css({ 'width': '100%' }).fadeOut(1000);
                }
                catch (e) { return };
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
