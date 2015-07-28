<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.PriceQuote.Default" Trace="false" Debug="false" EnableEventValidation="false" Async="true" %>

<%
    title = (modelName == "" ? "New Bike" : modelName) + " On-Road Price Quote";
    description = "Know On-Road Price of any new bike in India. On-road price of a bike includes ex-showroom price of the bike in your city, insurance charges. road-tax, registration charges, handling charges etc. Finance option is also provided so that you can get a fair idea of EMI and down-payment.";
    keywords = "bike price, new bike price, bike prices, bike prices India, new bike price quote, on-road price, on-road prices, on-road prices India, on-road price India";
    AdId = "1395986297721";
    AdPath = "/1017752/Bikewale_PriceQuote_";
    canonical = "http://www.bikewale.com/pricequote/";
    alternate = "http://www.bikewale.com/m/pricequote/";
%>
<!-- #include file="/includes/headNew.aspx" -->

<link href="/css/bw-pq.css?23july2015" rel="stylesheet" />
<link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/css/chosen.min.css" rel="stylesheet" />
<script type="text/javascript" src="/src/pq/price_quote.js?v=1.2"></script>
<script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/src/common/chosen.jquery.min.js"></script>
<script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/src/pq/MetroCities.js?23july2015"></script>
<form id="form1" runat="server">
    <div class="main-container">
        <div class="container_12">
            <div class="grid_12">
                <ul class="breadcrumb">
                    <li>You are here: </li>
                    <li><a class="blue" href="/">Home</a></li>
                    <li class="fwd-arrow">&rsaquo;</li>
                    <li><a class="blue" href="/new/">New</a></li>
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
                                            <asp:dropdownlist id="ddlMake" width="180" cssclass="drpClass"  data-bind=" value: selectedMake, event: { change: ddlMake_Change }, optionsCaption: '--Select Make--'" runat="server"></asp:dropdownlist>
                                            <span id="spnMake" class="error"></span></div>
                                    </div>
                                    <div class="clear"></div>
                                    <div class="input-box input-box-margin">
                                        <div>
                                            <asp:dropdownlist id="ddlModel" width="180" data-bind="options: models, optionsText: 'ModelName', optionsValue: 'ModelId', value: selectedModel, optionsCaption: '--Select Model--', enable: selectedMake, event: { change: bindCities }" cssclass="drpClass" runat="server"><asp:ListItem Text="--Select Model--" Value="0" /></asp:dropdownlist>
                                            <input type="hidden" id="hdn_ddlModel" runat="server" />
                                            <span id="spnModel" class="error"></span></div>
                                    </div>
                                    <div class="clear"></div>
                                    <input type="hidden" id="hdn_ddlVersion" runat="server" />
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
                                        <asp:dropdownlist id="ddlCity" width="180" cssclass="drpClass" data-bind="options: cities, optionsText: 'CityName', optionsValue: 'CityId', value: selectedCity, event: { change: UpdateArea }, optionsCaption: '--Select City--'" runat="server"><asp:ListItem Text="--Select City--" Value="0" /></asp:dropdownlist>
                                        <input type="hidden" id="hdn_ddlCity" runat="server" data-bind=""/><span id="spnCity" class="error" runat="server" /></div>
                                </div>
                                <div class="clear"></div>
                                <div class="input-box input-box-margin hide" id="divAreaChosen">
                                    <select data-placeholder="Search an Area.." class="chosen-select" style="width: 180px" tabindex="2" data-bind="options: areas, optionsText: 'AreaName', optionsValue: 'AreaId', value: selectedArea, optionsCaption: '--Select Area--'" id="ddlArea">
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
                                        <a href="/visitoragreement.aspx" target="_blank" class="blue">visitor agreement</a> and <a href="/privacypolicy.aspx" target="_blank" class="blue">privacy policy</a>.<br />
                                        <span class="error" id="spnAgree" runat="server"></span></p>
                                </div>
                            </div>
                            <div style="margin-left: 180px;" class="mid-box margin-top15">
                                <asp:button id="btnSavePriceQuote" class="action-btn" text="Get Price Quote" runat="server" />
                            </div>
                        </div>
                    </div>
                </div>
                <div id="div_ShowErrorMsg" runat="server" class="grey-bg border-light content-block text-highlight margin-top15"></div>
            </div>
            <div class="grid_4">
                <div class="margin-top15">
                    <!-- BikeWale_NewBike/BikeWale_NewBike_HP_300x250 -->
                    <!-- #include file="/ads/Ad300x250.aspx" -->
                </div>
            </div>
            <!-- Right Container ends here  -->
        </div>
    </div>
    <input type="hidden" id="hdnIsAreaShown" runat="server" />
</form>
<script type="text/javascript">
    var isAreaShown = false;

    var viewModel = {
        selectedCity: ko.observable(),
        cities: ko.observableArray(),
        selectedArea: ko.observable(),
        areas: ko.observableArray(),
        makes:ko.observableArray(),
        selectedMake: ko.observable(),
        selectedModel: ko.observable(),
        models: ko.observableArray()
    };

    $(document).ready(function () {

        ko.applyBindings(viewModel, document.getElementById("pq_car"));

        if (viewModel.selectedMake != undefined) {
                ddlMake_Change();
            }

        

        $('#ddlArea').prop('disabled', true).trigger("chosen:updated");
        $("#ddlArea").chosen({ no_results_text: "No matches found!!" });

        var cityId = '';

        if (window.location.search == '' || window.location.search == null) {
            ddlMake_Change();
        }
        else {
            MetroCities($("#ddlCity"));
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
                return true;
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

        if (!isBuyPrefChecked()) {
            buyTimeMsg.text("Please tell us your buying preferences.");
            isError = true;
        } else {
            buyTimeMsg.text("");
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
        if (makeId != undefined) {
            $.ajax({
                type: "POST",
                url: "/ajaxpro/Bikewale.Ajax.AjaxCommon,Bikewale.ashx",
                data: '{"requestType":"' + requestType + '", "makeId":"' + makeId + '"}',
                beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetModelsNew"); },
                success: function (response) {
                    var responseJSON = eval('(' + response + ')');
                    var resObj = eval('(' + responseJSON.value + ')');

                    viewModel.models(resObj);
                }
            });
        } else {
            viewModel.models([]);
            viewModel.cities([]);
            viewModel.areas([]);
            $('#divAreaChosen').hide();
        }
    }

    function bindCities() {
        var modelId = viewModel.selectedModel();
        if (modelId != undefined) {
            $('#hdn_ddlModel').val(modelId);
            FillCities(modelId);
        }
        else {
            viewModel.cities([]);
            viewModel.areas([]);
            $('#divAreaChosen').hide();
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
                var resObj = eval('(' + responseJSON.value + ')');

                viewModel.cities(resObj);

                MetroCities($("#ddlCity"));
            },
            error: function (response) {
                alert(response);
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
                        $('#divAreaChosen').show();
                        $('.chosen-container').attr('style', 'width:180px');

                        viewModel.areas(resObj);

                        $('.chosen-select').prop('disabled', false);
                        $('.chosen-select').trigger("chosen:updated");
                        isAreaShown = true;
                    } else {
                        $('#divAreaChosen').hide();
                        isAreaShown = false;
                    }

                    $('#hdnIsAreaShown').val(isAreaShown);
                },
                error: function (response) {
                    alert(response);
                }
            });
        }
        else {
            viewModel.areas([]);
        }
    }
</script>
<!-- #include file="/includes/footerInner.aspx" -->
