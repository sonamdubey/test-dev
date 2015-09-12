<%@ Control Language="C#" AutoEventWireup="False" Inherits="Bikewale.Controls.OnRoadPricequote" %>

<link href="/css/chosen.min.css" rel="stylesheet" />
<style>
    
/*PopupWidget Styling*/
#OnRoadContent .bw-popup, .bw-contact-popup { background:#fff; width:454px; position:fixed; left:50%; top:50%; z-index:999; margin-left:-220px; margin-top:-150px;  border-radius:2px; }
#OnRoadContent .bw-contact-popup { max-width:454px;}
#OnRoadContent .bw-popup-sm { width:300px; position:fixed; top:50%; left:52%;}
#OnRoadContent .popup-inner-container { padding:20px 20px 30px;}
#OnRoadContent .popup-inner-container h2{ padding-bottom:10px; border-bottom:2px solid #c62000;}
#OnRoadContent .bw-popup-sm select{top: 0;width: 100%;z-index: 2;margin: 5px;} 

/*#OnRoadContent div.chosenError { border:1px solid #F00;} 
#OnRoadContent div.chosenError div b {background-image:none;}*/
</style>

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
                <select data-placeholder="--Select Area--" class="form-control" id="ddlAreaOnRoad" tabindex="3" data-bind="options: bookingAreas, value: selectedArea, optionsText: 'AreaName', optionsValue: 'AreaId', optionsCaption: '--Select Area--', event: { change: areaChangedOnRoad }"></select>
                <span class="bwsprite error-icon hide"></span>
                <div class="bw-blackbg-tooltip hide">Please Select Area</div>
            </div>
            <button id="btnDealerPriceOnRoad" tabindex="4" class="btn btn-orange margin-bottom20" type="button" value="Get Price Quote" data-bind="event: { click: getPriceQuoteOnRoad }">Get price quote</button>
            </div>
            
               <!-- Onroad price quote ends here-->

            <p>Its private, no need to share your number and email</p>
        </div>
    </div>
    <div class="clear"></div>
</div>

<script type="text/javascript" src="/src/common/chosen.jquery.min.js"></script>
<script type="text/javascript">

    var preSelectedCityId = 0;
    var preSelectedCityName = "";
    var selectedModel = 0;
    var abHostUrl = '<%= ConfigurationManager.AppSettings["ABApiHostUrl"]%>';
    var metroCitiesIds = [40, 12, 13, 10, 224, 1, 198, 105, 246, 176, 2, 128];
    var pageId;
    $onRoadContent = $('#OnRoadContent');
    onRoadcity  = $('#ddlCitiesOnRoad');
    onRoadArea = $('#ddlAreaOnRoad');
    onRoadMakeModel = $('#makemodelFinalPrice');
    mname = "";

   
    
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
                        if (metroCitiesIds.indexOf(cities[i].CityId) > -1) {
                            var currentCity = cities[i];
                            cities.splice(cities.indexOf(currentCity), 1);
                            cities.splice(initIndex++, 0, currentCity);
                        }

                        if(preSelectedCityId == cities[i].CityId )
                        {
                            citySelected = cities[i];
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
        gtmCodeAppender(pageId, "City Selected", null);
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

    function areaChangedOnRoad() {
        gtmCodeAppender(pageId, "Area Selected", null);
    }



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
            $.ajax({
                type: 'POST',
                url: "/ajaxpro/Bikewale.Ajax.AjaxBikeBooking,Bikewale.ashx",
                data: '{"cityId":"' + cityId + '", "areaId":"' + areaId + '", "modelId":"' + selectedModel + '", "isMobileSource":false}',
                dataType: 'json',
                beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "ProcessPQ"); },
                success: function (json) {
                    var jsonObj = $.parseJSON(json.value);
                    if (jsonObj != undefined && jsonObj.quoteId > 0 && jsonObj.dealerId > 0) {
                        gtmCodeAppender(pageId, "Successful submission - DealerPQ", "Model : " + selectedModel + ', City : ' + viewModelOnRoad.selectedCity() + ', Area : ' + viewModelOnRoad.selectedArea());
                        window.location = "/pricequote/dealerpricequote.aspx";
                    }
                    else if (jsonObj != undefined && jsonObj.quoteId > 0) {
                        gtmCodeAppender(pageId, "Successful submission - BikeWalePQ", "Model : " + selectedModel + ', City : ' + viewModelOnRoad.selectedCity() + ', Area : ' + viewModelOnRoad.selectedArea());
                        window.location = "/pricequote/quotation.aspx";
                    } else {
                        gtmCodeAppender(pageId, "Error in submission", null);
                        $("#errMsgOnRoad").text("Oops. We do not seem to have pricing for given details.").show();
                    }
                },
                error: function (e) {
                    gtmCodeAppender(pageId, "Error in submission", null);
                    $("#errMsg").text("Oops. Some error occured. Please try again.").show();
                }
            });
        } else {
            gtmCodeAppender(pageId, "Error in submission", null);
            $("#errMsgOnRoad").text("Please select all the details").show();
        }
    }

    function gtmCodeAppender(pageId, action, label) {
        if (pageId != null) {
            switch (pageId) {
                case "1":
                    category = 'CheckPQ_Make';
                    action = "CheckPQ_Make_" + action;
                    break;
                case "2":
                    category = "CheckPQ_Series";
                    action = "CheckPQ_Series_" + action;
                    break;
                case "3":
                    category = "CheckPQ_Model";
                    action = "CheckPQ_Model_" + action;
                    break;
            }
            if (label) {
                dataLayer.push({ 'event': 'product_bw_gtm', 'cat': category, 'act': action, 'lab': label });
            }
            else {
                dataLayer.push({ 'event': 'product_bw_gtm', 'cat': category, 'act': action });
            }
        }

    }

    function checkCookies()
    {
        c = document.cookie.split('; ');
        for(i=c.length-1; i>=0; i--)
        {
            C = c[i].split('=');
            if(C[0]=="location")
            {
                var cData = (String(C[1])).split('_');
                preSelectedCityId = parseInt(cData[0]);
                preSelectedCityName = cData[1];
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
                    pageId = $(this).attr('pageCatId');
                    gtmCodeAppender(pageId, "Button Clicked", null);
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