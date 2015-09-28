<%@ Control Language="C#" AutoEventWireup="False" Inherits="Bikewale.Controls.OnRoadPricequote" %>

<script runat="server">
    private string staticUrl = System.Configuration.ConfigurationManager.AppSettings["staticUrl"];
    private string staticFileVersion = System.Configuration.ConfigurationManager.AppSettings["staticFileVersion"];
</script>
<link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/css/chosen.min.css?<%= staticFileVersion %>" rel="stylesheet" />
<div class="container">
    <div class="grid-5 leftfloat">
        <div class="bg-white content-inner-block-15 light-box-shadow rounded-corner2 margin-top70" id="OnRoadContent">
            <h2 class="text-bold margin-bottom20 font28">On road price</h2>

            <!-- On road pricequote control-->
            
            <div class="form-control-box margin-bottom20">
                <input value="" class="form-control ui-autocomplete-input" type="text" placeholder="Search Make and Model" id="makemodelFinalPrice" tabindex="1" autocomplete="off" style="width: 365px;">
                <span class="fa fa-spinner fa-spin position-abt pos-right10 pos-top15 text-black" style="display: none"></span>
                <span class="bwsprite error-icon hide"></span>
                <div class="bw-blackbg-tooltip hide">Please enter make/model name</div>
            </div>
            <div class="form-control-box margin-bottom20 finalPriceCitySelect " >
                <select data-placeholder="--Select City--" class="form-control" id="ddlCitiesOnRoad" tabindex="2" data-bind="options: bookingCities, value: selectedCity, optionsText: 'CityName', optionsValue: 'CityId', optionsCaption: '--Select City--', event: { change: cityChangedOnRoad }"></select>
                <span class="bwsprite error-icon hide"></span>
                <div class="bw-blackbg-tooltip hide">Please Select City</div>
            </div>
            <div class="form-control-box margin-bottom20 finalPriceAreaSelect " data-bind="visible: bookingAreas().length > 0">
                <select data-placeholder="--Select Area--" class="form-control" id="ddlAreaOnRoad" tabindex="3" data-bind="options: bookingAreas, value: selectedArea, optionsText: 'AreaName', optionsValue: 'AreaId', optionsCaption: '--Select Area--'"></select>
                <span class="bwsprite error-icon hide"></span>
                <div class="bw-blackbg-tooltip hide">Please Select Area</div>
            </div>
            <button id="btnDealerPriceOnRoad" tabindex="4" class="btn btn-orange margin-bottom20" type="button" value="Get On-road Price" data-bind="event: { click: getPriceQuoteOnRoad }">Get On-road Price</button>
            <p class="margin-bottom5">Its private, no need to share your number and email</p>    
        </div>
            
               <!-- Onroad price quote ends here-->

            
        </div>
    </div>
    <div class="clear"></div>
</div>

<script type="text/javascript" src="/src/common/chosen.jquery.min.js?14sept2015"></script>
<script type="text/javascript">

    var preSelectedCityId = 0;
    var preSelectedCityName = "", selectedMakeName = '', selectedCityName = '', gaLabel = '', selectedAreaName = '';
    var selectedModel = 0;
    var abHostUrl = '<%= ConfigurationManager.AppSettings["ABApiHostUrl"]%>';
    var metroCitiesIds = [40, 12, 13, 10, 224, 1, 198, 105, 246, 176, 2, 128];
    var pageId;
    $onRoadContent = $('#OnRoadContent');
    onRoadcity  = $('#ddlCitiesOnRoad');
    onRoadArea = $('#ddlAreaOnRoad');
    onRoadMakeModel = $('#makemodelFinalPrice');
    mname = "";
    var onCookieObj = {};

   
    
    // knockout OnRoadData binding
    var viewModelOnRoad = {
        selectedCity: ko.observable(),
        bookingCities: ko.observableArray([]),
        selectedArea: ko.observable(),
        bookingAreas: ko.observableArray([])
    };


    function FillCitiesOnRoad(modelId) {
        $.ajax({
            type: "POST",
            url: "/ajaxpro/Bikewale.Ajax.AjaxPriceQuote,Bikewale.ashx",
            data: '{"modelId":"' + modelId + '"}',
            beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetPriceQuoteCitiesNew"); },
            success: function (response) {
                var obj = JSON.parse(response);
                var cities = JSON.parse(obj.value);
                var citySelected = null; 
                if (cities) {
                    checkCookies();
                    var initIndex = 0;
                    for (var i = 0; i < cities.length; i++) { 

                        if (!isNaN(onCookieObj.PQCitySelectedId) && onCookieObj.PQCitySelectedId > 0 && onCookieObj.PQCitySelectedId == cities[i].CityId) {
                            citySelected = cities[i];
                        }

                        if (metroCitiesIds.indexOf(cities[i].CityId) > -1) {
                            var currentCity = cities[i];
                            cities.splice(cities.indexOf(currentCity), 1);
                            cities.splice(initIndex++, 0, currentCity);
                        }                            
                    }

                    cities.splice(initIndex, 0, { CityId: 0, CityName: "---------------", CityMaskingName: null }); 

                    viewModelOnRoad.bookingCities(cities);

                    if(citySelected!=null)
                    {
                        viewModelOnRoad.selectedCity(citySelected.CityId);
                    }                        

                    $("#ddlCitiesOnRoad option[value=0]").prop("disabled", "disabled");
                    if ($("#ddlCitiesOnRoad option:last-child").val() == "0") {
                        $("#ddlCitiesOnRoad option:last-child").remove();
                    }
                    if ($("#ddlCitiesOnRoad option:first-child").next().val() == "0") {
                        $("#ddlCitiesOnRoad option[value=0]").remove();
                    }
                    $('#ddlCitiesOnRoad').trigger("chosen:updated");                        

                    cityChangedOnRoad();
                }
                else {
                    viewModelOnRoad.bookingCities([]);
                    $('#ddlCitiesOnRoad').trigger("chosen:updated");
                }
            }
        });
    }

    function cityChangedOnRoad() {
        //gtmCodeAppender(pageId, "City Selected", null);
        if (viewModelOnRoad.selectedCity() != undefined) {
            $.ajax({
                type: "POST",
                url: "/ajaxpro/Bikewale.Ajax.AjaxPriceQuote,Bikewale.ashx",
                data: '{"cityId":"' + viewModelOnRoad.selectedCity() + '","modelId":"' + selectedModel + '"}',
                dataType: 'json',
                beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetPriceQuoteArea"); },
                success: function (response) {
                    areas = $.parseJSON(response.value);
                    if (areas.length) {
                        viewModelOnRoad.bookingAreas(areas);
                        if (!isNaN(onCookieObj.PQAreaSelectedId) && onCookieObj.PQAreaSelectedId > 0 && selectElementFromArray(areas, onCookieObj.PQAreaSelectedId)) {
                            viewModelOnRoad.selectedArea(onCookieObj.PQAreaSelectedId);
                            onCookieObj.PQAreaSelectedId = 0;
                        }
                        $('#ddlAreaOnRoad').trigger("chosen:updated");
                    }
                    else {
                        viewModelOnRoad.selectedArea(0);
                        viewModelOnRoad.bookingAreas([]);
                        $('#ddlAreaOnRoad').trigger("chosen:updated");
                    }
                }
            });
        } else {
            viewModelOnRoad.bookingAreas([]);
        }
    }

    //function areaChangedOnRoad() {
    //    gtmCodeAppender(pageId, "Area Selected", null);
    //}



    function isValidInfoOnRoad() {
        isValid = true;
        var errMsg = "Missing fields:";
        showHideMatchError(onRoadMakeModel, false);
        showHideMatchError(onRoadcity, false);
        showHideMatchError(onRoadArea, false);
        $(onRoadcity).next().removeClass("chosenError");
        mname = onRoadMakeModel.val();

        if (selectedModel <= 0 || (mname == "" || mname.length < 2 || mname == "Search Make and Model"))
        {
            showHideMatchError(onRoadMakeModel, true);
            errMsg += "Make/Model,";
            isValid = false;
            $(onRoadcity).next().addClass("chosenError");
        }

        if (viewModelOnRoad.selectedCity() == undefined ) {
            showHideMatchError(onRoadcity, true);              
            errMsg += "City,";
            isValid = false;
        }
        

        if (viewModelOnRoad.bookingAreas().length > 0 && viewModelOnRoad.selectedArea() == undefined) {
            showHideMatchError(onRoadArea, true);
            errMsg += "Area,";
            isValid = false;
        }

        if (!isValid) {
            errMsg = errMsg.substring(0, errMsg.length - 1);
            gtmCodeAppender(pageId, "Error in submission", errMsg);
        }

        return isValid;
    }

    function getPriceQuoteOnRoad() {
        var cityId = viewModelOnRoad.selectedCity(), areaId = viewModelOnRoad.selectedArea() ? viewModelOnRoad.selectedArea() : 0;
        if (isValidInfoOnRoad()) {

            //set global cookie
            setLocationCookie($('#ddlCitiesOnRoad option:selected'), $('#ddlAreaOnRoad option:selected'));
            

            $.ajax({
                type: 'POST',
                url: "/ajaxpro/Bikewale.Ajax.AjaxBikeBooking,Bikewale.ashx",
                data: '{"cityId":"' + cityId + '", "areaId":"' + areaId + '", "modelId":"' + selectedModel + '", "isMobileSource":false}',
                dataType: 'json',
                beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "ProcessPQ"); },
                success: function (json) {
                    var jsonObj = $.parseJSON(json.value);
                    selectedCityName = $("#ddlCitiesOnRoad option:selected").text();

                    if (areaId > 0)
                        selectedAreaName = $("#ddlAreaOnRoad option:selected").text();

                    if (selectedMakeName!="" && selectedCityName != "") {
                        gaLabel = selectedMakeName + ',' + selectedCityName;

                        if (selectedAreaName != '')
                            gaLabel += ',' + selectedAreaName;
                    }

                    if (jsonObj != undefined && jsonObj.quoteId > 0 && jsonObj.dealerId > 0) {
                        gtmCodeAppender(pageId, 'Dealer_PriceQuote_Success_Submit', gaLabel);
                        window.location = "/pricequote/dealerpricequote.aspx";
                    }
                    else if (jsonObj != undefined && jsonObj.quoteId > 0) {
                        gtmCodeAppender(pageId, 'BW_PriceQuote_Success_Submit', gaLabel);
                        window.location = "/pricequote/quotation.aspx";
                    } else {
                        gtmCodeAppender(pageId, 'BW_PriceQuote_Error_Submit', gaLabel);
                        $("#errMsgOnRoad").text("Oops. We do not seem to have pricing for given details.").show();
                    }
                },
                error: function (e) {
                    gtmCodeAppender(pageId, 'BW_PriceQuote_Error_Submit', gaLabel);
                    $("#errMsg").text("Oops. Some error occured. Please try again.").show();
                }
            });
        } else {
            gtmCodeAppender(pageId, 'BW_PriceQuote_Error_Submit', gaLabel);
            $("#errMsgOnRoad").text("Please select all the details").show();
        }
    }

    function gtmCodeAppender(pageId, action, label) {
        if (pageId != null) {
            switch (pageId) {
                case "1":
                    category = 'HP';
                    break;
                case "2":
                    category = "New_Bikes_Page";
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

    function checkCookies()
    {
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

    $(function () {

      $("#makemodelFinalPrice").bw_autocomplete({
            width: 365,
            source: 2,
            recordCount: 10,
            onClear: function () {
                objBikes = new Object();
            },
            click: function (event, ui, orgTxt) {
                var model = null;
                if (ui.item.payload.modelId > 0) {
                    model = new Object();
                    model.maskingName = ui.item.payload.modelMaskingName;
                    model.id = ui.item.payload.modelId;
                    pageId = '<%= PageId %>';
                    selectedMakeName = ui.item.label;
                    gtmCodeAppender(pageId, "Get_On_Road_Price_Click", null);
                    $("#errMsgOnRoad").empty();
                    selectedModel = model.id;
                    FillCitiesOnRoad(selectedModel);
                    
                }
            },
            open: function (result) {
                objBikes.result = result;
            },
            focusout: function () {
                if ($('li.ui-state-focus a:visible').text() != "") {
                    focusedMakeModel = new Object();
                    focusedMakeModel = objBikes.result[$('li.ui-state-focus').index()];                      
                }
            },
            afterfetch: function (result, searchtext) {
                return false;
            }
      });
        
      $("#ddlCitiesOnRoad").chosen({ no_results_text: "No matches found!!" });
      $("#ddlAreaOnRoad").chosen({ no_results_text: "No matches found!!" });

      ko.applyBindings(viewModelOnRoad, $("#OnRoadContent")[0]);

    });
</script>