<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.PriceQuote.Default" Trace="false" Debug="false" EnableEventValidation="false" Async="true" %>

<%
    title = (modelName == "" ? "New Bike" : modelName) + " On-Road Price Quote";
    description = "Know On-Road Price of any new bike in India. On-road price of a bike includes ex-showroom price of the bike in your city, insurance charges. road-tax, registration charges, handling charges etc. Finance option is also provided so that you can get a fair idea of EMI and down-payment.";
    keywords = "bike price, new bike price, bike prices, bike prices India, new bike price quote, on-road price, on-road prices, on-road prices India, on-road price India";
    AdId = "1395986297721";
    AdPath = "/1017752/Bikewale_PQ_";
    canonical = "https://www.bikewale.com/pricequote/";
    alternate = "https://www.bikewale.com/m/pricequote/";
    //modified by SajalGupta for unfilled impression of ads on 04 Aug 2016.
    isAd300x250_BTFShown = false;
%>
<!-- #include file="/UI/includes/headNew.aspx" -->

<link href="<%= staticUrl  %>/UI/css/bw-pq.css?<%= staticFileVersion %>" rel="stylesheet" />
<link href="<%= staticUrl  %>/UI/css/chosen.min.css?<%= staticFileVersion %>" rel="stylesheet" />
<script type="text/javascript" src="<%= staticUrl  %>/UI/src/pq/price_quote.js?<%= staticFileVersion %>"></script>
<script type="text/javascript" src="<%= staticUrl  %>/UI/src/common/chosen.jquery.min.js"></script>
<div class="main-container">
    <div class="container_12  container-min-height">
        <div class="grid_12">
            <ul class="breadcrumb">
                <li>You are here: </li>
                <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                    <a class="blue" href="/" itemprop="url">
                        <span itemprop="title">Home</span>
                    </a>
                </li>
                <li class="fwd-arrow">&rsaquo;</li>
                <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                    <a class="blue" href="/new-bikes-in-india/" itemprop="url">
                        <span itemprop="title">New</span>
                    </a>
                </li>
                <li class="fwd-arrow">&rsaquo;</li>
                <li class="current"><strong>On-Road Price Quote</strong></li>
            </ul>
            <div class="clear"></div>
        </div>
        <div class="grid_8 margin-top10">
            <h1 class="margin-bottom5">On Road Price Quote</h1>
            <div id="div_GetPQ" runat="server">
                <div id="get-pq-new" class="inner-content">
                    <div class="mid-box" id="pq_car">
                        <div id="divBikeDetails">
                            <div>
                                <div>
                                    <div class="input-box">
                                        <h2>Select Bike</h2>
                                    </div>
                                    <div class="input-box">
                                        <asp:dropdownlist id="ddlMake" width="200" cssclass="drpClass" tabindex="0" data-bind=" value: selectedMake, event: { change: ddlMake_Change }, optionsCaption: '--Select Make--'" runat="server"></asp:dropdownlist>
                                        <span id="spnMake" class="error"></span>
                                    </div>
                                </div>
                                <div class="clear"></div>
                                <div class="input-box input-box-margin">
                                    <div>
                                        <asp:dropdownlist id="ddlModel" tabindex="1" width="200" data-bind="options: models, optionsText: 'modelName', optionsValue: 'modelId', value: selectedModel, optionsCaption: '--Select Model--', enable: selectedMake, event: { change: bindCities }" cssclass="drpClass" runat="server"><asp:ListItem Text="--Select Model--" Value="0" /></asp:dropdownlist>
                                        <input type="hidden" id="hdn_ddlModel" runat="server" />
                                        <span id="spnModel" class="error"></span>
                                    </div>
                                </div>
                                <div class="clear"></div>
                                <div class="dotted-hr margin-top10 margin-bottom10"></div>
                            </div>
                        </div>
                        <div id="divLocation">
                            <div>
                                <div class="input-box">
                                    <h2>Select Location</h2>
                                </div>
                                <div class="input-box">
                                    <asp:dropdownlist data-placeholder="Search an Area.." class="chosen-select" style="width: 200px" tabindex="2" width="200" cssclass="drpClass" data-bind="options: cities, optionsText: 'CityName', optionsValue: 'CityId', value: selectedCity, event: { change: UpdateArea }, optionsCaption: '--Select City--', chosen: { width: '200px' }" id="ddlCity" runat="server"></asp:dropdownlist>
                                    <input type="hidden" id="hdn_ddlCity" runat="server" data-bind="" /><span id="spnCity" class="error" runat="server" />
                                </div>
                            </div>
                            <div class="clear"></div>
                            <div class="input-box input-box-margin " data-bind="visible: selectedCity() && areas() && areas().length > 0">
                                <select data-placeholder="Search an Area.." class="chosen-select" style="width: 200px" tabindex="3" data-bind="options: areas, optionsText: 'AreaName', optionsValue: 'AreaId', value: selectedArea, optionsCaption: '--Select Area--', chosen: { width: '200px' }" id="ddlArea">
                                    <option value=""></option>
                                </select>
                                <input type="hidden" id="hdn_ddlArea" runat="server" /><span id="spnArea" class="error" runat="server" />
                            </div>
                            <div class="clear"></div>
                        </div>
                        <div class="clear"></div>
                        <div style="margin-top: 10px; margin-left: 180px;">
                            <div>
                                <p class="margin-top10">
                                    <input type="checkbox" checked="checked" id="userAgreement" runat="server"><label for="userAgreement"> I agree with BikeWale</label>
                                    <a href="/visitoragreement.aspx" target="_blank" rel="noopener" class="blue">visitor agreement</a> and <a href="/privacypolicy.aspx" target="_blank" rel="noopener" class="blue">privacy policy</a>.<br />
                                    <span class="error" id="spnAgree" runat="server"></span>
                                </p>
                            </div>
                        </div>
                        <div style="margin-left: 180px;" class="mid-box margin-top15">
                            <asp:button id="btnSavePriceQuote" class="action-btn text_white" text="Check On-Road Price" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div id="div_ShowErrorMsg" runat="server" class="grey-bg border-light content-block text-highlight margin-top15"></div>
        </div>
        <div class="grid_4">
                <div class="margin-top15">
                    <!-- BikeWale_NewBike/BikeWale_NewBike_HP_300x250 -->
                    <!-- #include file="/UI/ads/Ad300x250.aspx" -->
                </div>
        </div>
        <!-- Right Container ends here  -->
    </div>
</div>
<input type="hidden" id="hdnIsAreaShown" runat="server" />
<script type="text/javascript">
    var metroCitiesIds = [40, 12, 13, 10, 224, 1, 198, 105, 246, 176, 2, 128];
    var isAreaShown = false;
    var preSelectedCityId = 0;
    var preSelectedCityName = "";
    var onCookieObj = {};
    var viewModel = {
        selectedCity: ko.observable(),
        cities: ko.observableArray(),
        selectedArea: ko.observable(),
        areas: ko.observableArray(),
        makes: ko.observableArray(),
        selectedMake: ko.observable(),
        selectedModel: ko.observable(),
        models: ko.observableArray()
    };
    var gaObj = { 'id': '<%= (int)Bikewale.Entities.Pages.GAPages.DealerPriceQuote_Landing%>', 'name': '<%= Bikewale.Entities.Pages.GAPages.DealerPriceQuote_Landing%>' };

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
    };

    $(document).ready(function () {

        ko.applyBindings(viewModel, document.getElementById("pq_car"));

        if (viewModel.selectedMake != undefined) {
            ddlMake_Change();
        }

        var cityId = '';

        if (window.location.search == '' || window.location.search == null) {
            ddlMake_Change();
        }
        else {
            //MetroCities($("#ddlCity"));
        }
        if ($("#ddlCity").val() > 0) {
            $("#hdn_ddlCity").val($("#ddlCity").val());
        }

        $("#ddlArea").change(function () {
            $("#hdn_ddlArea").val($("#ddlArea").chosen().val());
        });

        if ($("#ddlArea").chosen().val() > 0) {
            if (isAreaShown == true) {
                $("#hdn_ddlArea").val($("#ddlArea").val());
            }
        }

        $("#btnSavePriceQuote").click(function () {
            if (!isValidated()) {
                return false;
            }
            else {
                cityId = viewModel.selectedCity();
                areaId = viewModel.selectedArea();
                //set global cookie
                if (cityId != onCookieObj.PQCitySelectedId || areaId > 0)
                    setLocationCookie($('#ddlCity option:selected'), $('#ddlArea option:selected'));
            }
        });

    });

    function isValidated() {
        var re = /^[0-9]*$/;
        var reEmail = /^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,6}$/;

        var isError = false;
        var stdCode = $.trim($("#txtStdCode").val());
        var phone = $.trim($("#txtLandline").val());
        var city = $("#ddlCity");
        var area = $("#ddlArea").chosen().val();

        var rdoResearch = $("#rdoResearching");

        var emailMsg = $("#errEmail");
        var nameMsg = $("#errName");
        var contMsg = $("#errMobile");
        var cityMsg = $("#spnCity");
        var areaMsg = $("#spnArea");
        var buyTimeMsg = $("#spnBuyTime");
        var spnPhone = $("#landline");

        if (city.val() <= 0 || city.val() == "" || city.prop("disabled")) {
            cityMsg.text("Required");
            isError = true;
        } else {
            cityMsg.text("");
        }
        if (isAreaShown == true) {
            if (area <= 0 || area == "") {
                areaMsg.text("Required");
                isError = true;
            } else {
                areaMsg.text("");
            }
        }

        if (!isError) {
            if (!$("#userAgreement").prop("checked")) {
                alert("You must be agree with BikeWale visitor agreement and privacy policy.");
                isError = true;
            }
        }

        if (isError == true) {
            return false;
        } else {
            return true;
        }
    }


    function ddlMake_Change() {
        var requestType = "PRICEQUOTE";
        var makeId = viewModel.selectedMake();
        if (makeId) {
            $.ajax({
                type: "GET",
                url: "/api/PQModelList/?makeId=" + makeId,
                success: function (response) {
                    if (response) {
                        viewModel.models(response.models);
                      
                    }
                }
            });
        } else {
            viewModel.models([]);
            viewModel.cities([]);
            viewModel.areas([]);
        }
    }

    function bindCities() {
        var modelId = viewModel.selectedModel();
        if (modelId != undefined) {
            $('#hdn_ddlModel').val(modelId);
            FillCities(modelId);
            var obj = GetGlobalLocationObject();
            if (obj != null) {
                $('#ddlCity').val(obj.CityId).trigger('chosen:updated');
            }
        }
        else {
            viewModel.cities([]);
            viewModel.areas([]);
        }
    }

    function FillCities(modelId) {
        $.ajax({
            type: "POST",
            url: "/ajaxpro/Bikewale.Ajax.AjaxPriceQuote,Bikewale.ashx",
            data: '{"modelId":"' + modelId + '"}',
            beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetPriceQuoteCitiesNew"); },
            success: function (response) {

                var responseJSON = eval('(' + response + ')');
                var cities = eval('(' + responseJSON.value + ')');
                var citySelected = null;
                if (cities && cities.length > 0) {
                    insertCitySeparator(cities);
                    checkCookies();
                    viewModel.cities(cities);
                    if (!isNaN(onCookieObj.PQCitySelectedId) && onCookieObj.PQCitySelectedId > 0 && viewModel.cities() && selectElementFromArray(viewModel.cities(), onCookieObj.PQCitySelectedId)) {
                        viewModel.selectedCity(onCookieObj.PQCitySelectedId);
                    }
                    $("#ddlCity").find("option[value='0']").prop('disabled', true);
                    $("#ddlCity").trigger('chosen:updated');
                    UpdateArea();
                }
                else {
                    viewModel.cities([]);
                    viewModel.areas([]);
                }
            }
        });
    }

    function UpdateArea() {
        var cityId = viewModel.selectedCity();
        var modelId = viewModel.selectedModel();
        if (cityId != undefined) {
            $('#hdn_ddlCity').val(cityId);
            $.ajax({
                type: "POST",
                url: "/ajaxpro/Bikewale.Ajax.AjaxPriceQuote,Bikewale.ashx",
                data: '{"modelId":"' + modelId + '","cityId":"' + cityId + '"}',
                beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetPriceQuoteArea"); },
                success: function (response) {

                    var responseJSON = eval('(' + response + ')');
                    var resObj = eval('(' + responseJSON.value + ')');
                    if (responseJSON.value != '[]') {

                        viewModel.areas(resObj);
                        if (!isNaN(onCookieObj.PQAreaSelectedId) && onCookieObj.PQAreaSelectedId > 0 && viewModel.areas() && selectElementFromArray(viewModel.areas(), onCookieObj.PQAreaSelectedId)) {
                            viewModel.selectedArea(onCookieObj.PQAreaSelectedId);
                        }
                        $("#hdn_ddlArea").val($("#ddlArea").chosen().val());
                        isAreaShown = true;
                    } else {
                        isAreaShown = false;
                        viewModel.areas([]);
                        viewModel.selectedArea(undefined);
                    }

                    $('#hdnIsAreaShown').val(isAreaShown);
                },
                error: function () {
                    viewModel.areas([]);
                    viewModel.selectedArea(undefined);
                }
            });
        }
        else {
            viewModel.areas([]);
            viewModel.selectedArea(undefined);
        }
    }


    function checkCookies() {
        c = document.cookie.split('; ');
        for (i = c.length - 1; i >= 0; i--) {
            C = c[i].split('=');
            if (C[0] == "location") {
                var cData = (String(C[1])).split('_');
                onCookieObj.PQCitySelectedId = parseInt(cData[0]);
                onCookieObj.PQCitySelectedName = cData[1] ? cData[1].replace(/-/g, ' ') : "";
                onCookieObj.PQAreaSelectedId = parseInt(cData[2]);
                onCookieObj.PQAreaSelectedName = cData[3] ? cData[3].replace(/-/g, ' ') : "";

            }
        }
    }
</script>
<style type="text/css">
    .container-min-height {
        min-height: 530px;
    }
</style>
<!-- #include file="/UI/includes/footerInner.aspx" -->
