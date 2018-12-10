<%@ Page Language="C#" Inherits="Carwale.UI.Used.Valuation.Valuation" AutoEventWireup="false" Debug="false" Trace="false" EnableEventValidation="false" %>

<!doctype html>
<html itemscope itemtype="http://schema.org/WebPage">
<head>
    <%
        PageId = 5;
        Title = "Check value of your car for FREE";
        Description = "Check value of your car for absolutely free on carwale.com.";
        Revisit = "5";
        DocumentState = "Static";
        canonical = "https://www.carwale.com/used/carvaluation/";
        AdId = "1396440966236";
        AdPath = "/1017752/UsedCarValue_";
        Keywords = "car valuations india, instant valuation, value of your car, my car value, car assessment, used car actual price, used car price";
    %>
    <!-- #include file="/includes/global/head-script.aspx" -->
    <script type='text/javascript'>
        googletag.cmd.push(function () {
            googletag.defineSlot('<%= AdPath %>300x250', [300, 250], 'div-gpt-ad-<%= AdId %>-0').addService(googletag.pubads());
            googletag.defineSlot('<%= AdPath %>970x90', [[220, 90], [728, 90], [950, 90], [960, 90], [970, 66], [970, 90]], 'div-gpt-ad-<%= AdId %>-2').addService(googletag.pubads());
            googletag.pubads().collapseEmptyDivs();
            googletag.pubads().enableSingleRequest();
            googletag.enableServices();
        });
    </script>
    <style>
        .error {
            color: #ef3f30;
        }

        h2.hd2 span, h1.hd1 span {
            font-size: 10px;
            color: #999999;
            font-weight: normal;
        }

        .back-btn {
            float: right;
            top: -65px;
            position: relative;
            cursor: pointer;
        }

        .valuation-result-section {
            background: #f5f5f5;
            z-index: 9;
            width: 100%;
        }

        .content-shadow {
            border: 1px solid #e2e2e2;
            -moz-box-shadow: 0 1px 2px #e5e5e5;
            -webkit-box-shadow: 0 1px 2px #e5e5e5;
            -o-box-shadow: 0 1px 2px #e5e5e5;
            -ms-box-shadow: 0 1px 2px #e5e5e5;
            box-shadow: 0 1px 2px #e5e5e5;
        }

        .valuation-result-section #val-loading-img{
            width:100%;
            height:300px;
        }
         .valuation-result-section #val-loading-img img{
            position: absolute;
            top: 25%;
            right: 50%;
            left: 50%;
         }
    </style>
</head>

<body class="bg-white header-fixed-inner special-page special-skin-body no-bg-color">
    <!-- #include file="/includes/header.aspx" -->
    <section class="container">
        <div class="grid-12">
            <div class="bg-white padding-bottom10 padding-top10 text-center">
                <%= Carwale.UI.HtmlHelpers.Helpers.GetAdBarForAspx(AdId, 0, 90, 0, 0, true, 2) %>
            </div>
        </div>
        <div class="clear"></div>
    </section>
    <div class="clear"></div>
    <section class="bg-light-grey padding-top10 padding-bottom20 no-bg-color">
        <div class="container">
            <div class="grid-12">
                <div class="breadcrumb margin-bottom15">
                    <!-- breadcrumb code starts here -->
                    <ul class="special-skin-text">
                        <li><a href="/">Home</a></li>
                        <li><span class="fa fa-angle-right margin-right10"></span><a href="/used/">Used Cars</a></li>
                        <li><span class="fa fa-angle-right margin-right10"></span>Car Valuation</li>
                    </ul>
                    <div class="clear"></div>
                </div>
                <h1 class="font30 text-black special-skin-text">Used Car Valuation</h1>
                <div class="border-solid-bottom margin-top10 margin-bottom15"></div>
            </div>
            <div class="clear"></div>

            <div class="grid-8">
                <div class="search-right-price content-box-shadow content-inner-block-10">
                    <div class="left-grid">
                        <div class="gray-block-top">
                            <h2 class="hd2">Car Information <span><font color="red">*</font>All Fields are mandatory</span></h2>
                            <div class="text-grey"></div>
                            <div class="mid-box">
                                <form runat="server">
                                    <table cellspacing="0" border="0" cellpadding="5" width="100%">
                                        <tr>
                                            <td width="140">Manufacturing Year </td>
                                            <td>
                                                <div class="form-control-box inline-block year-selection">
                                                    <span class="select-box fa fa-angle-down"></span>
                                                    <asp:DropDownList ID="cmbYear" runat="server" class="form-control" Width="200">
                                                        <asp:ListItem Text="--Select Year--" Value="0" Selected="true" />
                                                    </asp:DropDownList>
                                                </div>
                                                <span id="spnYear" class="error"></span>
                                            </td>
                                        </tr>
                                         <tr>
                                            <td width="140" style="padding-right: 2px;">Manufacturing Month </td>
                                            <td>
                                                <div class="form-control-box inline-block month-selection">
                                                    <span class="select-box fa fa-angle-down"></span>
                                                    <asp:DropDownList ID="cmbMonth" runat="server" class="form-control" Width="200">
                                                        <asp:ListItem Text="--Select Month--" Value="0" Selected="true" />
                                                        <asp:ListItem Value="1" Text="Jan" />
                                                        <asp:ListItem Value="2" Text="Feb" />
                                                        <asp:ListItem Value="3" Text="Mar" />
                                                        <asp:ListItem Value="4" Text="Apr" />
                                                        <asp:ListItem Value="5" Text="May" />
                                                        <asp:ListItem Value="6" Text="Jun" />
                                                        <asp:ListItem Value="7" Text="Jul" />
                                                        <asp:ListItem Value="8" Text="Aug" />
                                                        <asp:ListItem Value="9" Text="Sep" />
                                                        <asp:ListItem Value="10" Text="Oct" />
                                                        <asp:ListItem Value="11" Text="Nov" />
                                                        <asp:ListItem Value="12" Text="Dec" />
                                                    </asp:DropDownList>
                                                </div>
                                                <span id="spnMonth" class="error"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Make</td>
                                            <td>
                                                <div class="form-control-box inline-block make-selection">
                                                    <span class="select-box fa fa-angle-down"></span>
                                                    <asp:DropDownList ID="cmbMake"  CssClass="form-control drpClass" runat="server" Width="200">
                                                        <asp:ListItem Value="0" Text="--Select Make--" Selected="true" />
                                                    </asp:DropDownList>
                                                    <div id="ddlMakeDiv" style="position:absolute; left:0; right:0; top:0; bottom:0;"></div>
                                                </div>
                                                <span id="spnMake" class="error"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Model</td>
                                            <td>
                                                <div class="form-control-box inline-block model-selection">
                                                    <span class="select-box fa fa-angle-down"></span>
                                                    <asp:DropDownList ID="cmbModel"  CssClass="form-control drpClass" runat="server" Width="200">
                                                        <asp:ListItem Value="0" Text="--Select Model--" Selected="true" />
                                                    </asp:DropDownList>
                                                </div>
                                                <span id="spnModel" class="error"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Version</td>
                                            <td>
                                                <div class="form-control-box inline-block version-selection">
                                                    <span class="select-box fa fa-angle-down"></span>
                                                    <asp:DropDownList ID="cmbVersion" CssClass="form-control drpClass" runat="server" Width="200">
                                                        <asp:ListItem Value="0" Text="--Select Version--" Selected="true" />
                                                    </asp:DropDownList>
                                                </div>
                                                <span id="spnVersion" class="error" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>City</td>
                                            <td>
                                                <div class="form-control-box inline-block city-selection">
                                                    <span class="select-box fa fa-angle-down"></span>
                                                    <asp:DropDownList ID="cmbValuationCity" CssClass="form-control drpClass" runat="server" Width="200">
                                                        <asp:ListItem Value="0" Text="--Select City--" Selected="true" />
                                                    </asp:DropDownList>
                                                </div>
                                                <span id="spnValueCity" class="error" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <table cellspacing="0" border="0" cellpadding="3" width="100%" style="display: none;" id="tblStateCity" runat="server">
                                                    <tr>
                                                        <td width="140">State</td>
                                                        <td>
                                                            <div class="form-control-box inline-block other-state-selection">
                                                                <asp:DropDownList ID="cmbState" CssClass="form-control drpClass" runat="server" Width="200">
                                                                    <asp:ListItem Text="--Select State--" Value="0" Selected="true" />
                                                                </asp:DropDownList>
                                                            </div>
                                                            <span id="spnState" class="error"></span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>City</td>
                                                        <td>
                                                            <div class="form-control-box inline-block other-city-selection">
                                                                <asp:DropDownList ID="cmbCity" CssClass="form-control drpClass" Enabled="false" runat="server" Width="200">
                                                                    <asp:ListItem Text="--Select City--" Value="0" Selected="true" />
                                                                </asp:DropDownList>
                                                            </div>
                                                            <span id="spnCity" class="error"></span>
                                                        </td>
                                                    </tr>                                                    
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="140">No Of Owners</td>
                                            <td>
                                                <div class="form-control-box inline-block owner-selection">
                                                    <span class="select-box fa fa-angle-down"></span>
                                                    <asp:DropDownList ID="ddlOwners" runat="server" class="form-control" Width="200">
                                                    </asp:DropDownList>
                                                </div>
                                                <span id="spnOwners" class="error"></span>
                                            </td>
                                        </tr>
                                             <tr>
                                            <td>Kilometers</td>
                                            <td>
                                                <div class="form-control-box inline-block city-selection">
                                                    <span class="select-box fa fa-angle-down"></span>
                                                    <asp:TextBox ID="txtKms" CssClass="form-control" Width="200" runat="server" placeholder="--Enter Kms driven--"></asp:TextBox>
                                                </div>
                                                <span id="spnKms" class="error" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                            <td>
                                                <a href="javascript:void(0)" class="btn btn-orange btn-sm" id="valuation-result-btn" onclick="onCheckValueClick()">Check Value</a>
                                            </td>
                                        </tr>
                                    </table>
                                </form>
                                <br />
                                <div class="mid-box">
                                    <ul class="ul-arrow2">
                                        <li>Year of Manufacture e.g. 2005</li>
                                        <li>Make, Model and Variant e.g. Hyundai Accent GLS</li>
                                        <li>The city in which the car was registered</li>
                                    </ul>
                                    <br />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="valuation-result-section content-shadow hide">
                    <div id="valuation-result"></div>
                    <div id="val-loading-img">
                        <img src='https://imgd.aeplcdn.com/0x0/statics/loader.gif' alt='Loading...' />
                    </div>
                    <p class="rightfloat alpha omega font12 margin-bottom10 margin-right10">
                        <a href="javascript:void(0)" class="btn btn-xs btn-white" id="evaluate-again-btn">Evaluate Again</a>
                        <a class="btn btn-sm btn-orange" onClick="sellCarButtonClick()">Sell Your Car</a>
                    </p>
                    <div class="clear"></div>
                </div>
            </div>
            <div class="grid-4">
                <%= Carwale.UI.HtmlHelpers.Helpers.GetAdBarForAspx(AdId, 300, 250, 20, 20, false, 0) %>
            </div>
            <div class="clear"></div>
        </div>
    </section>
    <div class="clear"></div>
    <!-- #include file="/includes/footer.aspx" -->
    <!-- all other js plugins -->
    <!-- #include file="/includes/global/footer-script.aspx" -->
    <script type="text/javascript" src="/static/js/used/valuation/valuationTracking.js"></script>
    <script type="text/javascript">

        $(document).ready(function () {
            document.getElementById('cmbYear').onchange = cmbYear_Change;
            document.getElementById("cmbMake").onchange = cmbMake_Change;
            document.getElementById("cmbModel").onchange = cmbModel_Change;
            document.getElementById("cmbVersion").onchange = onVersionChange;
            document.getElementById("cmbValuationCity").onchange = cmbValuationCity_Change;
            document.getElementById("cmbState").onchange = cmbState_Change;
            document.getElementById("cmbCity").onchange = onOtherCityChange;
            document.getElementById("ddlOwners").onchange = onOwnerChange;
            document.getElementById("txtKms").onfocusout = onKmsFocusOut;
            document.getElementById("cmbMonth").onchange = verifyMonth;

            $("#evaluate-again-btn").on("click", function () {
                closeValuationPopup();
                window.history.pushState(null, "Car Valuation", location.href.split("?")[0]);
                trackValuation(valuationTracking.actionType.evaluateAgainButtonClick);
                trackValuation(valuationTracking.actionType.valuationFormLoad);
                
            });
            if ($("#cmbYear").val() != $("#cmbYear option:first").val()) {
                $("#ddlMakeDiv").hide();
            }
            $("#ddlMakeDiv").click(function () {
                if ($("#cmbYear").val() == $("#cmbYear option:first").val()) {
                    $("#spnMake").text(" Select Year First");
                    }                
            });
            var txtKms = $("#txtKms");
            txtKms.on("keypress", function (e) {
                var charCode = (e.which) ? e.which : e.keyCode;
                return (charCode > 31 && (charCode < 48 || charCode > 57)) ? false : true;
            });
            txtKms.on('input propertychange', formatValue.withComma);
            if (txtKms.val() > 0) {
                txtKms.trigger("propertychange");
            }
            <%= isQSAvailable ? "showValuation(" + year + "," + car + "," + city + "," + owner + "," + kms + "," + "); window.history.replaceState({year : " + year + ", car: " + car + ", city : " + city + ", owner : " + owner + ", kms : " + kms + "}, 'Valuation Result');" : null%>
            trackValuation(valuationTracking.actionType.valuationFormLoad);
        });

        function sellCarButtonClick() {
            trackValuation(valuationTracking.actionType.sellCarButtonClick);
            var qsParams = document.URL.split("?")[1];
            window.location.href = "/used/sell/" + (qsParams != 'undefined' ? "?" + (qsParams) : "");
        }

        function verifyYear(year) {
            if (year == $("#cmbYear option:first").val()) {
                ShakeFormView($(".year-selection"));
                document.getElementById('spnYear').innerHTML = "Select car year";
                $("#ddlMakeDiv").show();
                return false;
            } else {
                document.getElementById('spnYear').innerHTML = "";
                $("#spnMake").text("");
                $("#ddlMakeDiv").hide();
            }
            return true;
        }

        function verifyMonth() {
            if ($('#cmbMonth').find(":selected").val()== 0) {
                ShakeFormView($(".month-selection"));
                $('#spnMonth').text("Select car month");
                return false;
            } else {
                $('#spnMonth').text("");
            }
            return true;
        }

        function cmbYear_Change() {
            trackValuation(valuationTracking.actionType.manufacturingYearSelect);
            var year = $('#cmbYear').find(":selected").val();
            var makeId = document.getElementById("cmbMake").value;
            disableDependentCombos(new Array("cmbModel", "cmbVersion"));
            bindMake(year);
            bindModel(year, makeId);
        }

        function createOptionGroup(resp, grpLabel, valueKey, TextKey) {
            var groupHtmlObj = $('<optgroup>', { label: grpLabel });
            if (resp) {
                for (var i = 0; i < resp.length; i++) {
                    groupHtmlObj.append("<option value=" + resp[i][valueKey] + ">" + resp[i][TextKey] + "</option>");
                }
            }
            return groupHtmlObj;
        }

        function bindDropDownWithGroup(responseLabelMapping, element, selectString, valueKey, TextKey) {
            if (responseLabelMapping) {
                if (!selectString){
                    selectString = "--Select--";
                }
                $(element).empty().append("<option value=\"0\">" + selectString + "</option>").attr("disabled", false);
                var groupArr = responseLabelMapping.map(function (mapping) {
                    return createOptionGroup(mapping.data, mapping.label, valueKey, TextKey);
                });
                for (var i = 0; i < groupArr.length; i++) {
                    $(element).append(groupArr[i]);
                }
                
            }
        }

        function bindMake(year) {
            if (verifyYear(year)) {
                $("#cmbMake").prop("disabled", false);
                if ($("#cmbMake > option").length <= 1) {
                    showLoading('cmbMake');
                    $.ajax({
                        type: "GET",
                        url: "/webapi/carmakesdata/getcarmakes/?type=used&module=2&year=" + year,
                        dataType: "json",
                        success: function (response) {
                            $('#spnMake').text("");
                            var groupLabelDataMap = [
                                { data: response.popularMakes, label: "Popular Makes" },
                                { data: response.otherMakes, label: "Other Makes" }];
                            bindDropDownWithGroup(groupLabelDataMap, document.getElementById("cmbMake"), '--Select make--', "makeId", "makeName");
                        }, error: function (response) {
                            $('#spnMake').text("No makes available for your selection");
                            hideLoading('cmbMake', '--Select Make--');
                        }
                    });
                }

            } else {
                $("#cmbMake").prop("disabled", true);
            }
        }

        function onVersionChange(e) {
            trackValuation(valuationTracking.actionType.versionSelect);
            verifyVersion();
        }

        function onOtherCityChange(e) {
            trackValuation(valuationTracking.actionType.otherCitySelect);
            verifyCity();
        }

        function onOwnerChange(e) {
            trackValuation(valuationTracking.actionType.ownersSelect);
            verifyOwners();
        }

        function onKmsFocusOut() {
            trackValuation(valuationTracking.actionType.kilometerDrivenFocusOut);
        }

        function verifyVersion() {
            var x = document.getElementById('cmbVersion');
            if (x.value == "") {
                ShakeFormView($(".version-selection"));
                document.getElementById('spnVersion').innerHTML = "Select Version";
                return true;
            }
            else if (document.getElementById('cmbVersion').options[0].selected) {
                ShakeFormView($(".version-selection"));
                document.getElementById('spnVersion').innerHTML = "Select Version";
                return true;
            }
            else document.getElementById('spnVersion').innerHTML = "";
        }

        function verifyCity() {
            if (document.getElementById('cmbValuationCity').options[0].selected) {
                ShakeFormView($(".city-selection"));
                document.getElementById('spnCity').innerHTML = "Select City";
                document.getElementById('spnValueCity').innerHTML = "Select City";
                return true;
            }
            else if (document.getElementById('cmbValuationCity').value == "-1" && document.getElementById('cmbCity').options[0].selected) {
                ShakeFormView($(".other-city-selection"));
                document.getElementById('spnCity').innerHTML = "Select City";
                return true;
            }
            else {
                document.getElementById('spnCity').innerHTML = "";
                document.getElementById('spnValueCity').innerHTML = "";
            }
        }
        function verifyKms() {
            var txtKms = $('#txtKms');
            var spnKms = $('#spnKms');
            if (txtKms.attr("data-value") == undefined || txtKms.attr("data-value")=="") {
                spnKms.html("Enter Kms");
                return false;
            }
            if (txtKms.attr("data-value") >= 1000000) {
                spnKms.html("Should be below 10 lakh");
                return false;
            }
            spnKms.html("");
            return true;
        }
        function verifyOwners() {
            if (document.getElementById('ddlOwners').options[0].selected) {
                ShakeFormView($(".owner-selection"));
                document.getElementById('spnOwners').innerHTML = "Select Owner";
                return true;
            }
            else {
                document.getElementById('spnOwners').innerHTML = "";
            }
        }
        function cmbState_Change(e) {
            trackValuation(valuationTracking.actionType.stateSelect);
            var stateId = document.getElementById("cmbState").value;
            disableDependentCombos(new Array("cmbCity"));
            if (stateId > 0) {
                showLoading('cmbCity');
                $.ajax({
                    type: "GET",
                    url: "/webapi/geocity/GetCitiesByState/?stateId=" + stateId,
                    dataType: "json",
                    success: function (response) {
                        bindDropDownList(response, document.getElementById("cmbCity"), '--Select City--', "CityId", "CityName");
                    }
                });
            }
        }

        function cmbMake_Change(e) {
            trackValuation(valuationTracking.actionType.makeSelect);
            var year = $('#cmbYear').find(":selected").val();
            var makeId = document.getElementById("cmbMake").value;
            disableDependentCombos(new Array("cmbModel", "cmbVersion"));
            bindModel(year, makeId);
        }

        function bindModel(year, makeId) {
            if (makeId > 0 && verifyYear(year)) {
                showLoading('cmbModel');
                $.ajax({
                    type: "GET",
                    url: "/webapi/carmodeldata/GetCarModelsByType/?type=used&makeid=" + makeId + "&year=" + year,
                    dataType: "json",
                    success: function (response) {
                        $('#spnModel').text("");
                        bindDropDownList(response, document.getElementById("cmbModel"), '--Select Model--', "ModelId", "ModelName");
                    }, error: function (response) {
                        $('#spnModel').text("No model available for your selection");
                        hideLoading('cmbModel', '--Select Model--');
                    }
                });
            }
        }

        function cmbModel_Change(e) {
            trackValuation(valuationTracking.actionType.modelSelect);
            var year = $('#cmbYear').find(":selected").val();
            var modelId = document.getElementById("cmbModel").value;
            disableDependentCombos(new Array("cmbVersion"));
            if (modelId > 0 && verifyYear(year)) {
                showLoading('cmbVersion');
                $.ajax({
                    type: "GET",
                    url: "/webapi/carversionsdata/GetCarVersions/?type=used&modelid=" + modelId+"&year="+year, 
                    dataType: "json",
                    success: function (response) {
                        $('#spnVersion').text("");
                        bindDropDownList(response, document.getElementById("cmbVersion"), '--Select Version--', "ID", "Name");
                    }, error: function (response) {
                        $('#spnVersion').text("No Version available for your selection");
                        hideLoading('cmbVersion', '--Select Version--');
                    }
                });
            }
        }

        function cmbValuationCity_Change(e) {
            trackValuation(valuationTracking.actionType.citySelect);
            var cityId = document.getElementById('cmbValuationCity').value;
            if (cityId >= 0) {
                document.getElementById("tblStateCity").style.display = "none";
                verifyCity();
            }
            else if (cityId == -1) {
                document.getElementById("tblStateCity").style.display = "";
                document.getElementById('spnCity').innerHTML = "";
                document.getElementById('spnValueCity').innerHTML = "";
                if ($('#cmbState option').length <= 1) {
                    $.ajax({
                        type: 'GET',
                        url: '/webapi/geocity/GetStates/',
                        dataType: "json",
                        success: function (response) {
                            bindDropDownList(response, document.getElementById("cmbState"), '--Select State--', "StateId", "StateName");
                        }
                    });
                }
            }
        }

        function showValuation(year, car, city, owner, kms) {
            $('#val-loading-img').show();
            $(".search-right-price").hide();
            $('.valuation-result-section').show();
            var valuationUrl = '/used/valuation/v1/report/?year=' + year + '&car=' + car + '&city=' + city + '&kms=' + kms + '&owner=' + owner ;
            $('#valuation-result').load(valuationUrl, function (response, status) {
                $('#val-loading-img').hide();
                if (status == 'error') {
                    $('#valuation-result').html("<div style='padding: 10px;' class='error'>Something went wrong. Please try again later.</div>");
                }
                else if (status == 'success') {
                    var caseId = $('.right-price-box').attr('caseid');
                    trackVauationResult(caseId)
                }
            });
        }

        function trackVauationResult(caseId) {
            if (caseId) {
                if (caseId == 1) {
                    trackValuation(valuationTracking.actionType.valuationExactMatchLoad);
                }
                else if (caseId > 1 && caseId < 9) {
                    trackValuation(valuationTracking.actionType.valuationApproximateMatchLoad);
                }
                else {
                    trackValuation(valuationTracking.actionType.valuationNotAvailableLoad);
                }
            }
        }

        function onCheckValueClick() {
            trackValuation(valuationTracking.actionType.checkValuationButtonClick);
            getValuation();
        }

        function getValuation() {
            if (verifyYear($('#cmbYear').find(":selected").val()) && !verifyVersion() && !verifyCity() && !verifyOwners() && verifyKms() && verifyMonth()) {
                var year = $("#cmbYear").val();
                var car = $("#cmbVersion").val();
                var city = $("#cmbValuationCity").val() > 0 ? $("#cmbValuationCity").val() : $("#cmbCity").val();
                var owner = $("#ddlOwners").val();
                var month = $("#cmbMonth").val();
                var kms = $("#txtKms").attr("data-value");
                showValuation(year, car, city, owner, kms);
                var qs = '?year=' + year + '&month=' + month + '&car=' + car + '&city=' + city + '&kms=' + kms + '&owner=' + owner;
                window.history.pushState({ year: year, month: month, car: car, city: city, kms: kms, owner: owner }, "Valuation Result", qs);
            }
        }

        $(window).on('popstate', function () {
            if (history && history.state && !isNaN(history.state.year) && !isNaN(history.state.car) && !isNaN(history.state.city) && !isNaN(history.state.owner) && !isNaN(history.state.kms) && !isNaN(history.state.month)) {
                showValuation(history.state.year, history.state.car, history.state.city, history.state.owner, history.state.kms);
            }
            else {
                closeValuationPopup();
            }
        });

        function closeValuationPopup() {
            $('#valuation-result').empty();
            $('.valuation-result-section').hide();
            $(".search-right-price").show();
        }

        function showLoading(objId) {
            $("#" + objId).find("option:first").text("--Loading--");
        }
        function  hideLoading(objId, value){
            $("#" + objId).find("option:first").text(value);
        }

        function bindDropDownList(response, cmbToFill, selectString, value, text) {
            if (response != null) {
                if (!selectString || selectString == '') selectString = "--Select--";
                $(cmbToFill).empty().append("<option value=\"0\">" + selectString + "</option>").attr("disabled", false);

                for (var i = 0; i < response.length; i++) {
                    $(cmbToFill).append("<option value=" + response[i][value] + ">" + response[i][text] + "</option>");
                }
            }
        }

        function disableDependentCombos(dependentCmbs) {
            if (dependentCmbs) {
                for (var i = 0; i < dependentCmbs.length; i++) {
                    $("#" + dependentCmbs[i]).attr("disabled", true);
                    $("#" + dependentCmbs[i]).val(0);
                }
            }
        }
        var formatValue = (function () {
            function withComma() {
                var fieldValue = this.value,
                    caretPos = this.selectionStart,
                    lenBefore = fieldValue.length;

                fieldValue = fieldValue.replace(/[^\d]/g, "").replace(/^0+/, "");
                this.setAttribute('data-value', fieldValue);
                this.value = Common.utils.formatNumeric(fieldValue);

                var selEnd = caretPos + this.value.length - lenBefore;
                if (this.value[selEnd - 1] == ',') {
                    selEnd--;
                }
                this.selectionEnd = selEnd > 0 ? selEnd : 0;
            }

            function handleCommaDelete(event) {
                var fieldValue = this.value;
                if (event.keyCode == 8) {             //backspace
                    if (fieldValue[this.selectionEnd - 1] == ',') {
                        this.selectionEnd--;
                    }
                }
                else if (event.keyCode == 46) {       //delete
                    if (fieldValue[this.selectionEnd] == ',') {
                        this.selectionStart++;
                    }
                }
            }

            return {              
                withComma: withComma,
                handleCommaDelete: handleCommaDelete                
            }
        })();

        function getDropDownSelectedText(dropDownSelectorObj) {
            var text;
            if (dropDownSelectorObj.val() > 0) {
                text = dropDownSelectorObj.find(':selected').text();
            } else {
                text = "Other";
            }
            return text;
        }

        function getSelectedValuationCityName() {
            var cityName;
            var valuationCityDropDownObj = $("#cmbValuationCity");
            var valuationOtherCityDropDownObj = $("#cmbCity");
            if (valuationCityDropDownObj.val() == -1) {
                cityName = getDropDownSelectedText(valuationOtherCityDropDownObj);
            }
            else {
                cityName = getDropDownSelectedText(valuationCityDropDownObj);
            }
            return cityName;
        }

        function trackValuation(actionType) {
            if (actionType) {
                valuationTracking.forDesktop(actionType, getTrackinglabel(actionType));
            }
        }

        function getValuationButtonClickTrackingLabel() {
            var labelArray = [];
            labelArray.push($("#cmbYear").val());
            labelArray.push(getDropDownSelectedText($('#cmbMake')));
            labelArray.push(getDropDownSelectedText($('#cmbModel')));
            labelArray.push(getDropDownSelectedText($('#cmbVersion')));
            labelArray.push(getSelectedValuationCityName());
            return labelArray.filter(function (item) { if (item) { return true; } }).join('|');
        }

        function getTrackinglabel(actionType) {
            switch (actionType) {
                case valuationTracking.actionType.checkValuationButtonClick:
                    return getValuationButtonClickTrackingLabel();
                    break;
                default:
                    return getSelectedValuationCityName();
            }
        }
    </script>

</body>
</html>
