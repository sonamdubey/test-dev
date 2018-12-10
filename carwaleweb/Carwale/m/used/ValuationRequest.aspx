<%@ Page Language="C#" ContentType="text/html" Inherits="MobileWeb.Used.ValuationRequest" AutoEventWireup="false" Trace="false" EnableEventValidation="false" %>
<%@ Import Namespace="Carwale.BL.Experiments" %>
<%
    Title = "Check value of your car for FREE - CarWale";
    Description = "Check value of your car for absolutely free on carwale.com.";
    Canonical = "https://www.carwale.com/used/carvaluation/";
    MenuIndex = "2";
    bool showExperimentalColor = ProductExperiments.IsShowExperimentalColor(CookiesCustomers.AbTest);
%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <!-- #include file="/m/includes/global/head-script.aspx" -->
    <style type="text/css">
        .valuation-result-section {
            background:#f5f5f5;
            position: fixed;
            top: 0;
            bottom: 0;
            left: 0;
            right: 0;
            overflow-x: hidden;
            overflow-y: scroll;
            z-index: 10;
            width: 100%;
            -webkit-overflow-scrolling: touch;
        }

        .evaluate-back-text {
            display: inline-block;
            vertical-align: middle;
        }

        .similarCarsHeader {
            top: 0;
            left: 0;
            right: 0;
            z-index: 6;
            min-height: 36px;
        }

        .top-corner {
            background: #565a5c;
            color: #fff;
            font-size: 18px;
        }

        .top-corner .back-long-arrow-left-white {
            position: relative;
            top: 3px;
        }
    </style> 
</head>

<body class="m-special-skin-body m-no-bg-color <%= (showExperimentalColor  ? "btn-abtest" : "")%>">
    <!-- #include file="/m/includes/header.aspx" -->
    <!--Outer div starts here-->
    <section class="container">
    	<!--Main container starts here-->
    	<div id="main-container">
			 <div class="grid-12">
			
            <h1 class="pgsubhead margin-top10 margin-bottom10 m-special-skin-text">Used Car Valuation</h1>
            <form runat="server">
            <div class="box content-inner-block-10 content-box-shadow rounded-corner2 margin-bottom10">
                <div class="margin-top15">Manufacturing Year &nbsp;&nbsp; <span id="spnManufacturingYear" class="error"></span></div>
                <div class="margin-top5">
                    <div class="form-control-box margin-top10">
                    <span class="select-box fa fa-angle-down"></span>
                    <asp:DropDownList class="form-control" ID="ddlYear" runat="server"  data-role="none">
                        <asp:ListItem Text="--Select Year--" Value="0" Selected="true" />
                    </asp:DropDownList>
                    </div>
                </div>
                <div id="divMake">
                    <div class="margin-top15">Make &nbsp;&nbsp; <span id="spnMake" class="error"></span></div>
                    <div class="margin-top5">
                        <div class="form-control-box margin-top10">
                        <span class="select-box fa fa-angle-down"></span>
                        <asp:DropDownList class="form-control" ID="ddlMake" runat="server" data-role="none">
                            <asp:ListItem Text="--Select Make--" Value="0" Selected="true" />
                        </asp:DropDownList>
                        <div id="ddlDivMake" style="position:absolute; left:0; right:0; top:0; bottom:0;"></div>
                        </div>
                    </div>
                </div>
                <div id="divModel" runat="server" style="display:none;">
                    <div class="margin-top15">Model &nbsp;&nbsp;<img id="imgLoaderModel" src="/m/images/circleloader.gif" width="16" height="16" style="position:relative;top:3px;display:none;" /> <span id="spnModel" class="error"></span></div>
                    <div class="margin-top5">
                        <div class="form-control-box margin-top10">
                        <span class="select-box fa fa-angle-down"></span>
                        <asp:DropDownList class="form-control" ID="ddlModel" runat="server" data-role="none"  />
                        </div>
                    </div>
                </div>
                <div id="divVersion" runat="server" style="display:none;">
                <div class="margin-top15">Version &nbsp;&nbsp;<img id="imgLoaderVersion" src="/m/images/circleloader.gif" width="16" height="16" style="position:relative;top:3px;display:none;" /> <span id="spnVersion" class="error" style="font-size: 12px;"></span></div>
                 <div class="margin-top5">
                        <div class="form-control-box margin-top10">
                        <span class="select-box fa fa-angle-down"></span>
                        <asp:DropDownList class="form-control" ID="ddlVersion" runat="server" data-role="none" onChange="javascript:versionChanged()" />
                        </div>
                </div>
                </div>               
                <div class="margin-top15">City &nbsp;&nbsp;<img id="imgLoaderCity" src="/m/images/circleloader.gif" width="16" height="16" style="position:relative;top:3px;display:none;" /> <span id="spnCity" class="error"></span></div>
                <div class="margin-top5">
                    <div class="form-control-box margin-top10">
                        <span class="select-box fa fa-angle-down"></span>
                        <asp:DropDownList class="form-control" ID="ddlCity" runat="server" data-role="none"  />
                    </div>
                </div>
                <div id="divOtherCity" runat="server" style="display:none;">
                <div class="margin-top15">State &nbsp;&nbsp; <span id="spnState" class="error"></span></div>
                 <div class="margin-top5">
                    <div class="form-control-box margin-top10">
                        <span class="select-box fa fa-angle-down"></span>
                        <asp:DropDownList class="form-control" ID="ddlState" runat="server" data-role="none"  />
                        </div>
                </div>
                <div id="ddlOtherCitydiv">
                    <div class="margin-top15">Other City &nbsp;&nbsp;<img id="imgLoaderOtherCity" src="/m/images/circleloader.gif" width="16" height="16" style="position:relative;top:3px;display:none;" /> <span id="spnOtherCity" class="error"></span></div>
                    <div class="margin-top5">
                        <div class="form-control-box margin-top10">
                            <span class="select-box fa fa-angle-down"></span>
                        <asp:DropDownList class="form-control" ID="ddlOtherCity" runat="server" data-role="none"  onChange="javascript:otherCityChanged()"/>
                        </div>
                    </div>
                </div>
                </div>
                <div class="margin-top15">Owners &nbsp;&nbsp; <span id="spnOwners" class="error"></span></div>
                <div class="margin-top5">
                    <div class="form-control-box margin-top10">
                    <span class="select-box fa fa-angle-down"></span>
                    <asp:DropDownList class="form-control" ID="ddlOwners" runat="server"  data-role="none" onChange="javascript:ownerChanged()">
                    </asp:DropDownList>
                    </div>
                </div>
                 <div class="margin-top15">Kilometers &nbsp;&nbsp; <span id="spnKms" class="error"></span></div>
                <div class="margin-top5">
                    <div class="form-control-box margin-top10">
                    <span class="select-box fa fa-angle-down"></span>
                     <asp:TextBox ID="txtKms" CssClass="form-control" runat="server" type="tel" placeholder="--Enter kms driven--"></asp:TextBox>
                    </div>
                </div>
                <div class="margin-top15">
                    <a href="javascript:void(0)" class="btn btn-orange btn-sm" id="valuation-result-btn">Check Value</a>
                </div>
            </div>
            </form>
            <div class="valuation-result-section hide">
                <div class="top-corner similarCarsHeader" style="display: block;">
                    <a id="evaluateResultPopupBackArrow" class="btn content-inner-block-10 padding-right30">
                        <span class="cwmsprite back-long-arrow-left-white"></span>
                    </a>
                </div>
                <div class="popup-loading-pic hide" style="position: absolute; top: 40%; right:50%; left:50%">
                    <img src="https://imgd.aeplcdn.com/0x0/statics/loader.gif" width="16" height="16" alt="" title="" />
                </div>
                <div class="valuation-result-content"></div>
            </div>
            <div id="divWindow" style="display:none;">
                <div style="background-image: url('/m/images/loader.gif');background-position:center center;background-repeat:no-repeat;height:40px;filter:alpha(opacity=80);opacity:0.80;">&nbsp;</div>
            </div>
            </div>
        <div class="clear"></div>
        </div>
        <!--Main container ends here-->
    </section>
    <div class="clear"></div>
    <!--Outer div ends here-->

    <!-- #include file="/m/includes/footer.aspx" -->
    <!-- #include file="/m/includes/global/footer-script.aspx" -->
    <script type="text/javascript" src="/static/js/used/valuation/valuationTracking.js"></script>
    <script language="javascript" type="text/javascript">
        var qsParameters = '';
        $(document).ready(function () {            
            $('.valuation-result-section').css('left', $(document).width() + 'px');
            $('.valuation-result-section').show();
            detectedCityId = "-1";

            <%= isQSAvailable ? "getValuation('year=" + year + "&car=" + car + "&city=" + city + "&owner=" + owner + "&kms=" + kms +"');" : "PreFillCity();"%>
            if ($("#ddlYear").val() != $("#ddlYear option:first").val()) {
                 $("#ddlDivMake").hide();
            }
            $("#ddlDivMake").click(function () {
                if ($("#ddlYear").val() == $("#ddlYear option:first").val()) {
                    $("#spnMake").text("Select Year First");
                }
            });
            var txtKms = $("#txtKms");
            txtKms.on("keypress", function (e) {
                var charCode = (e.which) ? e.which : e.keyCode;
                return (charCode > 31 && (charCode < 48 || charCode > 57)) ? false : true;
            });
            txtKms.on('input propertychange', formatValue.withComma);
            txtKms.on('input propertychange', formatValue.handleCommaDelete);

            if (txtKms.val() > 0) {
                txtKms.trigger("propertychange");
            }
            txtKms.focusout(onKmsFocusOut);
            $('#valuation-result-btn').on('click', function(){
                checkValueClick();
            });
            trackValuation(valuationTracking.actionType.valuationFormLoad);
        });

        function PreFillCity() {
            if (isCookieExists('_CustCityMaster') && isCookieExists('_CustCityIdMaster')) {
                detectedCityId = $.cookie("_CustCityIdMaster");
                detectedCityName = $.cookie("_CustCityMaster");
                if ($("#ddlCity option[value='" + detectedCityId + "']").length > 0) {
                    $("#ddlCity").val(detectedCityId);
                }
            }
        }

        function CityChanged() {
            if ($("#ddlCity").val() == -1) {
                $("#divOtherCity").show();
                $('#imgLoaderCity').show();
                LoadState().done(bindStateDropdown);
            }
            else {
                $("#divOtherCity").hide();
            }
            trackValuation(valuationTracking.actionType.citySelect);
        }

        function LoadState() {
            return $.ajax({
                type: 'GET',
                url: '/webapi/geocity/GetStates/',
                dataType: "json"
            });
        }

        function bindStateDropdown(response) {
            $("#ddlState").html("<option value='0'>--Select State--</option>");
            $("#ddlOtherCitydiv").hide();
            $('#imgLoaderCity').hide();
            if (response != null) {
                $.each(response, function (i, stateObj) {
                    $("#ddlState").append('<option value=' + stateObj.StateId + '>' + stateObj.StateName + '</option>');
                });
            }
        }

       function  ValidateYear(){
            var year = $("#ddlYear").val();
            if (year == $("#ddlYear option:first").val()) {
                $("#spnManufacturingYear").text("Select Year");
                $("#ddlDivMake").show();
                return false;
            } else {
                $("#spnManufacturingYear").text("");
                $("#spnMake").text("");
                $("#ddlDivMake").hide();
                return true;
            }
       }
       function YearChanged() {
           trackValuation(valuationTracking.actionType.manufacturingYearSelect);
           var year = $("#ddlYear").val();
           var makeId = $("#ddlMake").val();
           var divModelObj = $("#divModel");
           var divVersionObj = $("#divVersion");
           var dependentArray = [divVersionObj];
           var isValidMakeSelected = makeId > 0;
           if (ValidateYear()) {
               $("#ddlMake").prop("disabled", false);
               if ($("#ddlMake option").length <= 1) {
                   bindMakes(year);
               }
               if (isValidMakeSelected) {
                   bindModel(makeId, year);
               }
           }
           else {
               $("#ddlMake").prop("disabled", true);
               if (!isValidMakeSelected) {
                   dependentArray.push(divModelObj);
               }
           }
           hideDependentElement(dependentArray);
       }

       function bindMakes(year) {
           if (year) {
                $.ajax({
                    type: "GET",
                    url: "/webapi/carmakesdata/getcarmakes/?type=used&year="+year,
                    dataType: "json",
                    success: function (response) {
                        bindDropDownList(response, document.getElementById("ddlMake"), '--Select Make--', 'makeId', 'makeName');
                    }
                });
           }
       }
       
       function MakeChanged() {
           trackValuation(valuationTracking.actionType.makeSelect);
            var makeId = $("#ddlMake").val();
            var year = $("#ddlYear").val();
            $("#divVersion").hide();
            var divModelObj = $("#divModel");
            if (makeId <= 0 || !ValidateYear()) {
                $(divModelObj).hide();
            }
            else {
                bindModel(makeId, year);
            }
        }

        function bindModel(makeId, year) {
            if (makeId && year) {
                var divModelObj = $("#divModel");
                $(divModelObj).show();
                $(divModelObj).slideDown('fast');
                $("#imgLoaderModel").show();
                $.ajax({
                    type: "GET",
                    url: "/webapi/carmodeldata/GetCarModelsByType/?type=used&makeid=" + makeId + "&year=" + year,
                    dataType: "json",
                    success: function (response) {
                        $("#imgLoaderModel").hide();
                        $("#spnModel").text("");
                        bindDropDownList(response, document.getElementById("ddlModel"), '--Select Model--', 'ModelId', 'ModelName');
                    }, error: function (response) {
                        bindDropDownListEmpty(document.getElementById("ddlModel"), '--Select Model--');
                        $("#imgLoaderModel").hide();
                        $("#spnModel").text("No model available for your selection");
                    }
                });
            }
        }

        function ModelChanged() {
            trackValuation(valuationTracking.actionType.modelSelect);
            var modelId = $("#ddlModel").val();
            var year = $("#ddlYear").val();
            if (modelId <= 0 || !ValidateYear()) {
                   $("#divVersion").hide();
            } else {
                $("#divVersion").show();
                $('#divVersion').slideDown('fast');
                $("#imgLoaderVersion").show();
                $.ajax({
                    type: "GET",
                    url: "/webapi/carversionsdata/GetCarVersions/?type=used&modelid=" + modelId + "&year=" + year ,
                    dataType: "json",
                    success: function (response) {
                        $("#imgLoaderVersion").hide();
                        $("#spnVersion").text("");
                        bindDropDownList(response, document.getElementById("ddlVersion"),'--Select Version--','ID','Name');                      
                    }, error: function (response) {
                        bindDropDownListEmpty(document.getElementById("ddlVersion"), '--Select Version--');
                        $("#imgLoaderVersion").hide();
                        var spnVprsionObj = $("#spnVersion");
                        $(spnVprsionObj).text("No Version available for your selection");
                     }    
                });
            }
        }

        function hideDependentElement(ElementObjArray) {
            if (ElementObjArray) {
                for (var i = 0; i < ElementObjArray.length; i++) {
                    ElementObjArray[i].hide();
                }
            }
        }

        function StateChanged() {
            trackValuation(valuationTracking.actionType.stateSelect);
            $("#ddlOtherCitydiv").show();
            var stateId = $("#ddlState").val();
            if (stateId > 0) {
                $("#imgLoaderOtherCity").show();
                $.ajax({
                    type: "GET",
                    url: "/webapi/geocity/GetCitiesByState/?stateId=" + stateId,
                    dataType: "json",
                    success: function (response) {
                       $("#imgLoaderOtherCity").hide();
                       bindDropDownList(response, document.getElementById("ddlOtherCity"),'--Select City--','CityId','CityName');
                       if (detectedCityId != "-1") {
                            if ($("#ddlOtherCity option[value='" + detectedCityId + "']").length > 0) {
                                $("#ddlOtherCity").val(detectedCityId);
                            }
                        }
                    }
                });
            }
        }
        function versionChanged() {
            trackValuation(valuationTracking.actionType.versionSelect);
        }

        function ownerChanged() {
            trackValuation(valuationTracking.actionType.ownersSelect);
        }

        function otherCityChanged() {
            trackValuation(valuationTracking.actionType.otherCitySelect);
        }

        function onKmsFocusOut() {
            trackValuation(valuationTracking.actionType.kilometerDrivenFocusOut);
        }

        function checkValueClick() {
            if (!IsValid()) {
                return false;
            }
            else {
                trackValuation(valuationTracking.actionType.checkValuationButtonClick);
                getValuation();
            }
        }

        function bindDropDownListEmpty(cmbToFill, selectString) {
            $(cmbToFill).empty().append("<option value=\"0\">" + selectString + "</option>");
        }
      function bindDropDownList(response, cmbToFill, selectString, value, text) {
            if (response != null) {
                $(cmbToFill).empty().append("<option value=\"0\">" + selectString + "</option>");
                for (var i = 0; i < response.length; i++) {
                    $(cmbToFill).append("<option value=" + response[i][value] + ">" + response[i][text] + "</option>");
                }
            }
        }


      function IsValid() {          
            retVal = true;
            $("#spnValType").html("");
            $("#spnManufacturingYear").html("");
            $("#spnMake").html("");
            $("#spnModel").html("");
            $("#spnVersion").html("");
            $("#spnCity").html("");
            $("#spnState").html("");
            $("#spnOtherCity").html("");
            $("#spnOwners").html("");
            $("#spnKms").html("");


            if ($("#ddlValType").val() <= 0) {
                retVal = false;
                $("#spnValType").html("Required");
            }

            if (!ValidateYear()) {
                retVal = false;
            }

            if ($("#ddlMake").val() <= 0) {
                retVal = false;
                $("#spnMake").html("Required");
            }

            if ($("#ddlModel").val() <= 0) {
                retVal = false;
                $("#spnModel").html("Required");
            }

            if ($("#ddlVersion").val() <= 0) {
                retVal = false;
                $("#spnVersion").html("Required");
            }

            if ($("#ddlCity").val() == 0) {
                retVal = false;
                $("#spnCity").html("Required");
            }
            else if ($("#ddlCity").val() == -1) {
                if ($("#ddlState").val() <= 0) {
                    retVal = false;
                    $("#spnState").html("Required");
                }
                if ($("#ddlOtherCity").val() <= 0) {
                    retVal = false;
                    $("#spnOtherCity").html("Required");
                }
            }

            if ($("#ddlOwners").val() == 0) {
                retVal = false;
                $("#spnOwners").html("Required");
            }
            var txtKms = $('#txtKms');
            var spnKms = $('#spnKms');
            if (txtKms.attr("data-value") == undefined || txtKms.attr("data-value")=="") {
                retVal = false;
                spnKms.html("Required");
            }
            if (txtKms.attr("data-value") >= 1000000) {
                retVal = false;
                spnKms.html("Should be below 10 lakh");
            }

            return retVal;
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


        $(document).on("citypopped", function () {
            $(".m-city-selection-pop").css('display', 'none');
            $("#m-blackOut-window").css('display', 'none');
            $("html,body").removeClass("lock-browser-scroll");
        });
        changeCityIconShow = false;

        function getValuation(qs) {
            var urlWithOutQs = window.location.origin + window.location.pathname;
            var queryString = qs || getQsparameters();
            $('.valuation-result-section').animate({
                "left": 0
            });
            Common.utils.lockPopup();
            $('.popup-loading-pic').show();
            //replacing the state will clear out history of user coming with qs
            window.history.replaceState('valuationPopUpReplace', "", urlWithOutQs);
            $('.valuation-result-content').load('/used/valuation/v1/report/?' + queryString, function (response, status) {
                window.history.pushState("valuationpopup", "", urlWithOutQs + '?' + queryString);
                $('.popup-loading-pic').hide();
                if (status == 'error') {
                    $('.valuation-result-content').html("<div style='padding: 10px;' class='error'>Something went wrong. Please try again later.</div>");
                }
                else if (status == 'success') {
                    var caseId = $('.right-price-box').attr('caseid');
                    trackVauationResult(caseId)
                }
            });
        }

        //this duplicate with desktop
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
           

        $("#evaluateResultPopupBackArrow").on("click", function () {
            window.history.back();
        });
        $(window).on('popstate', popStateHandler);
        $(window).on('resize', resizeHandler);

        function resizeHandler() {
            if ($('.valuation-result-section').css('left') != '0px') {
                $('.valuation-result-section').css('left', $(document).width() + 'px');
            }
        }

        function popStateHandler() {
            closeValuationPopup();
        }

        function getQsparameters() {
            var cityId = $("#ddlCity").val() == -1 ? $("#ddlOtherCity").val() : $("#ddlCity").val();
            var qs = '';
            qs = 'year=' + $("#ddlYear").val() +
            '&car=' + $("#ddlVersion").val() +
            '&city=' + cityId +
            '&owner=' + $("#ddlOwners").val() +
            '&kms=' + $("#txtKms").attr("data-value");
            return qs;
        }
        function closeValuationPopup() {
            $('.valuation-result-content').empty();
            $('.valuation-result-section').animate({
                "left": $(document).width()
            });
            Common.utils.unlockPopup();
        }

        //Duplicate code used for both desktop and msite tracking
        function getDropDownSelectedText(dropDownSelectorObj) {
            var text;
            if (dropDownSelectorObj.val() > 0) {
                text = dropDownSelectorObj.find(':selected').text();
            }
            else {
                text = "Other";
            }
            return text;
        }

        function getSelectedValuationCityName() {
            var cityName;
            var valuationCityDropDownObj = $("#ddlCity");
            var valuationOtherCityDropDownObj = $("#ddlOtherCity");
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
                valuationTracking.forMobile(actionType, getTrackinglabel(actionType));
            }
        }

        function getValuationButtonClickTrackingLabel() {
            var labelArray = [];
            labelArray.push($("#ddlYear").val());
            labelArray.push(getDropDownSelectedText($('#ddlMake')));
            labelArray.push(getDropDownSelectedText($('#ddlModel')));
            labelArray.push(getDropDownSelectedText($('#ddlVersion')));
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
