<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.PriceQuote.Default" Trace="false" EnableEventValidation="false" Async="true" %>

<%
    title = (modelName == "" ? "New Bike" : makeName + " " + modelName) + " On-Road Price Quote";
    description = "Know On-Road Price of any new bike in India. On-road price of a bike includes ex-showroom price of the bike in your city, insurance charges. road-tax, registration charges, handling charges etc. Finance option is also provided so that you can get a fair idea of EMI and down-payment.";
    keywords = "bike price, new bike price, bike prices, bike prices India, new bike price quote, on-road price, on-road prices, on-road prices India, on-road price India";
    canonical = "https://www.bikewale.com/pricequote/";
    AdPath = "/1017752/Bikewale_Mobile_OnRoadPrice";
    AdId = "1398839030772";
    Ad_320x50 = true;
    Ad_Bot_320x50 = true;
    menu = "3";
%>
<!-- #include file="/UI/includes/headermobile_noad.aspx" -->
<script type="text/javascript" src="<%= staticUrl%>/UI/m/src/placeholder.js?v=1.0"></script>
<script type="text/javascript" src="https://stb.aeplcdn.com/bikewale/src/BikeWaleCommon.js?v=3.2"></script>
<link href="<%= staticUrl  %>/UI/css/chosen.min.css?<%= staticFileVersion %>" type="text/css" rel="stylesheet" />
<style type="text/css">
    .ui-filterable div input {
        height: 40px;
    }

    .chosen-container.chosen-container-single span {
        font-weight: normal;
    }
</style>
<asp:textbox id="txtMake" runat="server" style="display: none;" text="" data-role="none" />
<asp:textbox id="txtModel" runat="server" style="display: none;" text="" data-role="none" />
<asp:textbox id="txtCity" runat="server" style="display: none;" text="" data-role="none" />
<asp:textbox id="txtArea" runat="server" style="display: none;" text="" data-role="none" />
<asp:hiddenfield id="hdnmodel" runat="Server" value="" />
<div class="padding5" id="pq_car">
    <div id="br-cr" itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb"><a href="/m/new-bikes-in-india/" class="normal" itemprop="url"><span itemprop="title">New Bikes</span></a> &rsaquo; <span class="lightgray">On-Road Price Quote</span></div>
    <h1><%=(modelName == "" ? "New Bike" : makeName + " " + modelName) %> On-Road Price</h1>
    <div class="box1 new-line5">
        <%if (modelId == "")
            {%>
        <div class="new-line5">
            <asp:dropdownlist id="ddlMake" class="textAlignLeft" data-bind=" value: selectedMake, optionsCaption: '--Select Make--'" runat="server"></asp:dropdownlist>
        </div>
        <div id="divModel" style="display: none;">
            <div class="new-line15">
                <img id="imgLoaderModel" src="https://imgd.aeplcdn.com/0x0/bw/static/sprites/m/circleloader.gif" width="16" height="16" style="position: relative; top: 3px; display: none;" />
                <asp:dropdownlist id="ddlModel" class="textAlignLeft" data-bind="options: models, optionsText: 'modelName', optionsValue: 'modelId', value: selectedModel, optionsCaption: '--Select Model--', enable: selectedMake" runat="server"><asp:ListItem Text="--Select Model--" Value="" /></asp:dropdownlist>
            </div>
        </div>
        <%} %>
        <div class="new-line15" data-bind="visible: selectedModel() > 0">
            <img id="imgLoaderCity" src="https://imgd.aeplcdn.com/0x0/bw/static/sprites/m/circleloader.gif" width="16" height="16" style="position: relative; top: 3px; display: none;" />
            <div class="new-line15">
                <asp:dropdownlist id="ddlCity" class="textAlignLeft" data-bind="options: cities, optionsText: 'CityName', optionsValue: 'CityId', value: selectedCity, optionsCaption: '--Select City--',chosen: { width: '100%' }" runat="server"><asp:ListItem Text="--Select City--" Value="" /></asp:dropdownlist>
            </div>
        </div>
        <div class="new-line5" id="divArea" style="display: none;">
            <img id="imgLoaderArea" src="https://imgd.aeplcdn.com/0x0/bw/static/sprites/m/circleloader.gif" width="16" height="16" style="position: relative; top: 3px; display: none;" />
            <div class="ui-select ui-corner-all ui-shadow new-line15" id="divAreaPopup" style="font-size: 14px;" onclick="OpenPopup(this)"><a href="#" class="ui-btn">--Select an Area--</a></div>
            <div class="divAutoSuggest" style="min-height: 100% !important; background-color: #f8f8f8; display: none">
                <div data-role="header" data-theme="b" class="ui-corner-top" data-icon="delete">
                    <a href="#" onclick="CloseWindow()" data-role="button" data-theme="b" data-icon="delete" data-iconpos="notext" class="ui-btn-right">Close</a>
                    <h1>Select an Area</h1>
                </div>
                <input id="inputSearch" type="text" placeholder="Search an Area.." onkeyup="autosearch(this);" class="searchBox" />
                <ul data-role="listview" data-inset="true" id="ddlAreaTest" data-theme="a" class="filtered-list">
                </ul>
            </div>
        </div>
        <div class="new-line15">
            <div>
                <input type="checkbox" style="margin-top: 3px;" id="userAgreement" checked="checked" /></div>
            <div style="margin-left: 35px; !important;">
                I agree with BikeWale <a href="/visitoragreement.aspx" target="_blank" rel="noopener">Visitor Agreement</a> and <a href="/privacypolicy.aspx" target="_blank" rel="noopener">Privacy Policy</a>.
            </div>
        </div>
        <p class="lightgray f-12 new-line10">
            We respect your privacy and will never publicly display, share or use your contact details without your authorization.  By providing your contact details to us you agree that 
				        we (and/or any of our partners including dealers, bike manufacturers,banks like ICICI bank etc) may call you on the phone number mentioned, in order to provide information or assist you in any transactions, 
				        and that we may share your details with these partners.
        </p>
        <div class="new-line15">
            <asp:linkbutton id="btnSubmit" runat="server" data-theme="b" cssclass="getPrice" style="color: #fff !important;" text="Check On-Road Price" data-rel="popup" data-role="button" data-transition="pop" data-position-to="window" />
        </div>
        <div class="new-line5">&nbsp;</div>
    </div>
</div>
<input type="hidden" id="hdnIsAreaShown" runat="server" />
<div data-role="popup" id="popupDialog" data-overlay-theme="a" data-theme="c" data-dismissible="false" class="ui-corner-all">
    <div data-role="header" data-theme="a" class="ui-corner-top" style="background-color: #000">
        <h1 style="color: #fff;">Error !!</h1>
    </div>
    <div data-role="content" data-theme="d" class="ui-corner-bottom ui-content" style="background-color: #fff;">
        <span id="spnError" style="font-size: 14px; line-height: 20px;" class="error"><%= errMsg %></span>
        <a href="#" data-role="button" data-rel="back" data-theme="c" data-mini="true">OK</a>
    </div>
</div>

<script type="text/javascript" src="<%= staticUrl  %>/UI/m/src/chosen-jquery-min-mobile.js?<%= staticFileVersion %>"></script>
<script type="text/javascript">

    var areaDataSet = [];
    var pqAreaId = 0;
    var pqAreaName = "";
    var isAreaShown = false;
    var onCookieObj = {};
    var metroCitiesIds = [40, 12, 13, 10, 224, 1, 198, 105, 246, 176, 2, 128];
    var viewModelPQ = {
        selectedCity: ko.observable(),
        cities: ko.observableArray(),
        makes: ko.observableArray(),
        selectedMake: ko.observable(),
        selectedModel: ko.observable(),
        models: ko.observableArray()
    };
    var preSelectedCityId = 0;
    var preSelectedCityName = "";


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

    function autosearch(event) {
        var matchingAreas = [];
        var term = $(event).val().trim();

        var invalid = /[°"§%()\[\]{}=\\?´`'#<>|,;]+/g;
        term = term.replace(invalid, "-");

        var re = /^([A-Za-z0-9_&-\.\s]*)$/;

        if (re.test(term) && (term.indexOf('.') !== 0)) {
            matchingAreas = filter(term, areaDataSet);
        }

        var searchStr = '';
        if (matchingAreas.length > 0) {
            for (var i = 0; i < matchingAreas.length; i++) {
                searchStr += "<li class='ui-last-child'><a class='ui-btn' href='#'  areaId='" + matchingAreas[i].value + "'  areaName='" + matchingAreas[i].text + "' onClick='selectArea(this);'>" + matchingAreas[i].text.replace(new RegExp("(" + term + ")", "i"), '<u>$1</u>') + "</a></li>";
            }

            $('.filtered-list').html(searchStr).trigger('create');
        }
        else {
            searchStr = '<li>No matches found!!</li>';
            $('.filtered-list').html(searchStr).trigger('create');
        }
    }

    function selectArea(event) {

        var self = $(event);
        var areaId = self.attr('areaId');
        var areaName = self.attr('areaname');
        pqAreaName = areaName;
        pqAreaId = areaId;
        var cityId = $("#ddlCity").val();
        if (isAreaShown == true) {
            $("#txtArea").val(areaId);
            var str = " <a href='#' class='ui-btn'>" + areaName + "</a>";
            $("#divAreaPopup").html(str);
        }
        CloseWindow();
    }

    function filter(letter, items) {

        var x = [];
        var filtered = [];
        x = letter.match(/\.{2}/g);
        if (x !== null && x.length > 1) {

        }
        else {

            var searchTerm = letter.replace(/\.{1,}/g, '_');
            searchTerm = searchTerm.replace(/\s{2,}/g, ' ');
            var letterMatch = new RegExp('^' + searchTerm, 'i');

            for (var i = 0; i < items.length; i++) {
                var item = items[i];

                var resultTerm = item.text.replace(/\.{1,}/g, '_');
                resultTerm = resultTerm.replace(/\s{2,}/g, ' ');

                var invalid = /[°"§%()\[\]{}=\\?´`'#<>|,;]+/g;
                resultTerm = resultTerm.replace(invalid, "-");

                var splitResult = resultTerm.split(' ');
                for (var j = 0; j < splitResult.length; j++) {
                    if (letterMatch.test(splitResult[j]) || letterMatch.test(resultTerm)) {
                        filtered.push(item);
                        break;
                    }
                }
            }
        }

        return filtered;
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


    function OpenPopup(divAreaddl) {
        $("#divParentPageContainer").hide();
        $("#divForPopup").attr("style", "z-index:1002;width:100%;height:100%;position:absolute;");
        $(divAreaddl).next().show();
        $(".divAutoSuggest").show();

        $("#divForPopup").html($(divAreaddl).next().html());

        $('.searchBox').focus();
    }

    function CloseWindow() {
        $(".divAutoSuggest").hide();
        $("#divForPopup").hide();
        $("#divParentPageContainer").show();
    }

    $(document).ready(function () {
        ko.applyBindings(viewModelPQ, document.getElementById("pq_car"));

        $(".divAutoSuggest").hide();
        <% if (!String.IsNullOrEmpty(errMsg))
    {%>
        $("#popupDialog").popup("open");
        return false;
        <%}%>

        if ($("#txtModel").val() > 0) {
            FillCitiesPQ($("#txtMake").val());
        }

        $("#numMobile").val($("#txtMobile").val());

        $("#ddlMake").change(function () {
            var makeId = $(this).val();
            $("#ddlModel").val(0);
            $("#ddlModel").selectmenu("refresh", true);
            if (makeId == 0) {
                $("#divModel").hide();
            }
            if (makeId != 0) {
                $("#txtMake").val(makeId);
                LoadModels(makeId);
            } else {
                $("#ddlModel").val("0").attr("disabled", true);
                FillCitiesPQ(0);
            }
        });

        $("#ddlModel").change(function () {
            var modelId = $(this).val();
            if (modelId != 0) {
                $("#imgLoaderVersion").show();
                $("#txtModel").val(modelId);
                FillCitiesPQ(modelId);
            }
            else {
                FillCitiesPQ(0);
            }
        });

        $("#ddlCity").change(function () {
            areaDataSet = [];
            var resetAreas = '';
            $("#txtCity").val($(this).val());
            $('.filtered-list').html(resetAreas).trigger('create');
            var modelId = $("#txtModel").val();
            $(".ui-filterable input[data-type='search']").val('');
            $("#txtArea").val(0);

            var str = " <a href='#' class='ui-btn'>" + '--Select an Area--' + "</a>";
            $("#divAreaPopup").html(str);

            var cityId = $(this).val();
            $("#divArea").hide();
            $("#imgLoaderArea").show();
            $("#ddlCity-button").find("span.textAlignLeft").hide();
            $.ajax({
                type: "POST",
                url: "/ajaxpro/Bikewale.Ajax.AjaxPriceQuote,Bikewale.ashx",
                data: '{"modelId":"' + modelId + '","cityId":"' + cityId + '"}',
                beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetPriceQuoteArea"); },
                success: function (response) {
                    $("#imgLoaderArea").hide();
                    var responseJSON = eval('(' + response + ')');
                    var resObj = eval('(' + responseJSON.value + ')');
                    if (resObj && resObj.length > 0) {
                        isAreaShown = true;
                        $("#hdnIsAreaShown").val(isAreaShown);

                        for (var i = 0; i < resObj.length; i++) {
                            var objAreas = { value: resObj[i].AreaId, text: resObj[i].AreaName };
                            areaDataSet.push(objAreas);

                            $('#ddlAreaTest').append("<li class='ui-last-child'><a class='ui-btn' href='#' areaId='" + resObj[i].AreaId + "'  areaName='" + resObj[i].AreaName + "' onClick='selectArea(this);'>" + resObj[i].AreaName + "</a></li>");
                            $("#divArea").show();
                        }
                        $('#ddlAreaTest li').find('a').last().css('border-bottom', '1px solid #ccc');
                        if (!isNaN(onCookieObj.PQAreaSelectedId) && onCookieObj.PQAreaSelectedId != 0 && selectElementFromArray(resObj, onCookieObj.PQAreaSelectedId)) {
                            selectArea($('#ddlAreaTest li a.ui-btn[areaId="' + onCookieObj.PQAreaSelectedId + '"]')[0]);
                        }
                    }
                    else {
                        isAreaShown = false;
                        $("#imgLoaderArea").hide();
                        $("#hdnIsAreaShown").val(isAreaShown);
                    }
                }
            });

        });

        function LoadModels(makeId) {
            var requestType = "PRICEQUOTE";
            $('#divModel').slideDown('fast');
            $("#imgLoaderModel").show();
            $.ajax({
                type: "GET",
                url: "/api/PQModelList/?makeId=" + makeId,
                success: function (response) {
                    if (response) {
                        viewModelPQ.models(response.models);
                        $("#imgLoaderModel").hide();
                    }

                }
            });
            $('#divModel').removeClass("hide");
            $('#divModel').slideDown('fast', function () {
            });
        }

        function FillCitiesPQ(modelId) {
            if (modelId) {
                $.ajax({
                    type: "POST",
                    url: "/ajaxpro/Bikewale.Ajax.AjaxPriceQuote,Bikewale.ashx",
                    data: '{"modelId":"' + modelId + '"}',
                    beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetPriceQuoteCitiesNew"); },
                    success: function (response) {
                        var responseJSON = eval('(' + response + ')');
                        var resObj = eval('(' + responseJSON.value + ')');
                        if (resObj != null && resObj.length > 0) {
                            var initIndex = 0;
                            checkCookies();
                            insertCitySeparator(resObj);
                            viewModelPQ.cities(resObj);
                            if (!isNaN(onCookieObj.PQCitySelectedId) && onCookieObj.PQCitySelectedId > 0 && viewModelPQ.cities() && selectElementFromArray(viewModelPQ.cities(), onCookieObj.PQCitySelectedId)) {
                                viewModelPQ.selectedCity(onCookieObj.PQCitySelectedId);
                                $("#ddlCity option[value=" + onCookieObj.PQCitySelectedId + "]").attr("selected", "selected");
                            }
                            $("#ddlCity").find("option[value='0']").prop('disabled', true);
                            $("#ddlCity").trigger('chosen:updated');
                            $("#ddlCity-button").removeClass().find("span.textAlignLeft").hide();
                            $("#txtCity").val($("#ddlCity").val());

                            if ($("#ddlCity").val() != "0") $("#ddlCity").trigger("change");
                        }
                        else viewModelPQ.cities([]);
                    },
                    error: function (response) {
                    }
                });
            }
            else {
                viewModelPQ.cities([]);
            }
        }

        $('#btnSubmit').click(function () {
            if (isValid()) {
                pqSetLocationCookie();
            }
            else return false;
        });

        function isValid() {

            var retVal = true;
            var errorMsg = "";

            var reEmail = /^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,6}$/;
            var reMobile = /^[0-9]*$/;
            var name = /^[a-zA-Z& ]+$/;

            $("#txtMobile").val($("#numMobile").val());

            if ($("#ddlMake").val() == 0) {
                retVal = false;
                errorMsg = "Please select make<br>";
            }
            else if ($("#ddlModel").val() == 0) {
                retVal = false;
                errorMsg += "Please select model<br>";
            }

            if ($("#ddlCity").val() <= 0) {
                retVal = false;
                errorMsg += "Please select city<br>";
            }
            else
                if (isAreaShown == true) {
                    if ($("#txtArea").val() <= 0) {

                        retVal = false;
                        errorMsg += "Please select area<br>";
                    }
                }


            if (document.getElementById("userAgreement").checked == false) {
                retVal = false;
                errorMsg += "You must agree with the BikeWale Visitor Agreement and Privacy Policy to continue.<br>";
            }


            if (retVal == false) {
                $("#spnError").html(errorMsg);
                $("#popupDialog").popup("open");
            }

            return retVal;
        }
    });

    function UpdateArea() {

        var cityId = $("#txtCity").val();
        var modelId = $("#txtModel").val();
        if (cityId != undefined) {
            $('#txtCity').val(cityId);
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

                        viewModelPQ.areas(resObj);
                        $('.chosen-select').prop('disabled', false);
                        if (!isNaN(onCookieObj.PQAreaSelectedId) && onCookieObj.PQAreaSelectedId > 0 && selectElementFromArray(resObj, onCookieObj.PQAreaSelectedId)) {
                            $('#ddlAreaTest li a[areaId="' + onCookieObj.PQAreaSelectedId + '"]').click();
                            onCookieObj.PQAreaSelectedId = 0;
                        }
                        $('.chosen-select').trigger("chosen:updated");
                        isAreaShown = true;
                    } else {
                        $('#divAreaChosen').hide();
                        isAreaShown = false;
                    }

                    $('#hdnIsAreaShown').val(isAreaShown);
                }
            });
        }
        else {
            viewModelPQ.areas([]);
        }
    }

    function pqSetLocationCookie() {
        var selectedCityId = parseInt($("#ddlCity").val());
        if (selectedCityId > 0) {
            cookieValue = selectedCityId + "_" + $("#ddlCity option:selected").text();
            var selectedAreaId = parseInt($("#txtArea").val());
            if (selectedAreaId > 0)
                cookieValue += "_" + selectedAreaId + "_" + pqAreaName;

            if (selectedCityId != onCookieObj.PQCitySelectedId && selectedAreaId > 0)
                SetCookieInDays("location", cookieValue, 365);
        }
    }
</script>
<!-- #include file="/UI/includes/footermobile_noad.aspx" -->
