<%@ Page Language="C#" AutoEventWireup="false"  Inherits="Bikewale.Mobile.New.CompareBike" Trace="false" %>
<%@ Register TagPrefix="CB" TagName="CompareBike" Src="/m/controls/CompareBikeMin.ascx" %>
<%
    title = "Compare Bikes | New Bike Comparisons in India - BikeWale";
    keywords = "bike compare, compare bike, compare bikes, bike comparison, bike comparison india";
    description = "Comparing Indian bikes was never this easy. CarWale presents you the easiest way of comparing bikes. Choose two or more bikes to compare them head-to-head.";
    canonical = "http://www.bikewale.com/comparebikes/";
    AdPath = "/1017752/Bikewale_Mobile_NewBikes";
    AdId = "1398766302464";
    menu = "11";
    Ad_320x50 = true;
    Ad_Bot_320x50 = true;
%>
<!-- #include file="/includes/headermobile.aspx" -->
<link href="/m/css/compare/landing.css" type="text/css" rel="stylesheet" />
<script type="text/javascript" src="http://st2.aeplcdn.com/bikewale/src/BikeWaleCommon.js?v=3.2"></script>
<style>
    
</style>
    <input type="hidden" id="hdnMake1" runat="server" />
<input type="hidden" id="hdnMake2" runat="server" />
<input type="hidden" id="hdnModel1" runat="server" />
<input type="hidden" id="hdnModel2" runat="server" />
<input type="hidden" id="hdnBikeName1" runat="server" />
<input type="hidden" id="hdnBikeName2" runat="server" />
<input type="hidden" id="hdnVersionId1" runat="server" value="-1" />
<input type="hidden" id="hdnVersionId2" runat="server" value="-1"/>


<div>
    <div class="grid-12 margin-top10">
        <div class="content-box-shadow padding-top20 padding-right10 padding-left10 padding-bottom20">
            <h1 class="margin-bottom15 text-center">Compare bikes</h1>
            <div id="divMakeddl1" class="grid-6 compare-box">
                <div class="compare-box-placeholder text-center" onclick="OpenPopup(this)">
                    <span class="grey-bike"></span>
                    <p>Select bike 1</p>
                </div>
            </div>
            <div id="divListContainer" style="display:none;">
                <div class="divMake" style="min-height:100% !important;background-color:#f8f8f8;">
                    <div data-role="header" data-theme="b"  class="ui-corner-top" data-icon="delete">
                        <a href="#" onclick="CloseWindow()" data-role="button" data-theme="b" data-icon="delete" data-iconpos="notext" class="ui-btn-right">Close</a>
                        <h1>Select Make</h1>
                    </div>
            
                    <div data-role="content" data-theme="d" class="ui-corner-bottom ui-content">
                        <ul data-role="listview" id="ddlMake1" runat="server"></ul>
                    </div>
                </div>
                <div class="divModel" style="display:none;min-height:100% !important;background-color:#f8f8f8;">
                    <div data-role="header" data-theme="b" class="ui-corner-top" >
                        <a href="#" onclick="CloseWindow()" data-role="button" data-theme="b" data-icon="delete" data-iconpos="notext" class="ui-btn-right">Close</a>
                        <h1>Select Model</h1>
                    </div>
                    <div data-role="content" data-theme="d" class="ui-corner-bottom ui-content">
                        <ul id="Ul1" data-role="listview" type="ddlModel1">
                   
                        </ul>
                    </div>
                </div>
                <div class="divVersion" style="display:none;min-height:100% !important;background-color:#f8f8f8;">
                    <div data-role="header" data-theme="b" class="ui-corner-top" >
                        <a href="#" onclick="CloseWindow()" data-role="button" data-theme="b" data-icon="delete" data-iconpos="notext" class="ui-btn-right">Close</a>
                        <h1>Select Version</h1>
                    </div>
                    <ul data-role="listview" type="ddlVersion1">
                    </ul>
                </div>
            </div>

            <div id="divMakeddl2" class="grid-6 compare-box">
                <div class="compare-box-placeholder text-center" onclick="OpenPopup(this)">
                    <span class="grey-bike"></span>
                    <p>Select bike 2</p>
                </div>
            </div>

            <div id="divListContainer2" style="display:none;">
                <div class="divMake" style="min-height:100% !important;background-color:#f8f8f8;">
                    <div data-role="header" data-theme="b"  class="ui-corner-top">
                        <a href="#" onclick="CloseWindow()" data-role="button" data-theme="b" data-icon="delete" data-iconpos="notext" class="ui-btn-right">Close</a>
                        <h1>Select Make</h1>
                    </div>
                    <div data-role="content" data-theme="d" class="ui-corner-bottom ui-content">
                        <ul data-role="listview" id="ddlMake2" runat="server"></ul>
                    </div>
                </div>
                <div class="divModel" style="display:none;min-height:100% !important;background-color:#f8f8f8;">
                    <img id="imgLoaderModel" src="/m/images/circleloader.gif" width="16" height="16" style="position:relative;top:3px;display:none;" /> 
                    <div data-role="header" data-theme="b" class="ui-corner-top" >
                        <a href="#" onclick="CloseWindow()" data-role="button" data-theme="b" data-icon="delete" data-iconpos="notext" class="ui-btn-right">Close</a>
                        <h1>Select Model</h1>
                    </div>
                    <ul data-role="listview" type="ddlModel2">
                
                    </ul>
                </div>
                <div class="divVersion" style="display:none;min-height:100% !important;background-color:#f8f8f8;">
                    <div data-role="header" data-theme="b" class="ui-corner-top" >
                        <a href="#" onclick="CloseWindow()" data-role="button" data-theme="b" data-icon="delete" data-iconpos="notext" class="ui-btn-right">Close</a>
                        <h1>Select Version</h1>
                    </div>
                    <ul data-role="listview" type="ddlVersion2">
                    </ul>
                </div>
            </div>
            <div class="clear"></div>

            <div class="margin-top20 text-center">
                <a data-theme="b" data-rel="popup" data-role="button" data-transition="pop" data-position-to="window" onclick="VerifyVersion();" id="compare-button">Compare now</a>
            </div>
        </div>
    </div>
    <div class="clear"></div>

    <div data-role="popup" id="popupDialog" data-overlay-theme="a" data-theme="c" data-dismissible="false"  class="ui-corner-all">
        <div data-role="header" data-theme="a" class="ui-corner-top" style="background-color:#000;">
            <h1 class="ui-title" style="color:#fff;" role="heading" aria-level="1">Error !!</h1>
        </div>
        <div data-role="content" data-theme="d" class="ui-corner-bottom ui-content" style="background-color:#fff;">
            <span id="spnError" style="font-size:14px;line-height:20px;" class="error" ></span>
            <a href="#" data-role="button" data-rel="back" data-theme="c" data-mini="true">OK</a>
        </div>
    </div>
    
    </div>

    <div class="container margin-bottom20">
        <div class="grid-12 alpha omega">
            <CB:CompareBike ID="NewBikeDealers1" runat="server" ></CB:CompareBike>
        </div>
        <div class="clear"></div>
    </div>


<script type="text/javascript">
    
    var formatedMake1, formatedMake2, formatedModel1, formatedModel2;

    $(document).ready(function () {
        var bikeName = "",
            divMakeddl1 = "#divMakeddl1",
            divMakeddl2 = "#divMakeddl2";
 
        if ($("#hdnVersionId1").val() != "-1" && $("#hdnVersionId2").val() != "-1") {
            formatedMake1 = $("#hdnMake1").val();
            formatedMake2 = $("#hdnMake2").val();
            formatedModel1 = $("#hdnModel1").val();
            formatedModel2 = $("#hdnModel2").val();
        }
    });

    function OpenPopup(divMakeddl) {
        var compareBox = $(divMakeddl).closest('.compare-box');

        $("#divParentPageContainer").hide();
        $("#divForPopup").attr("style", "z-index:10;width:100%;height:100%;position:absolute;");        
        $(compareBox).next().show();
        $(".divMake").show();
        $("#divForPopup").html($(compareBox).next().html());
        $(".divModel, .divVersion").hide();
    }

    function ShowModel(a) {
        bikeName = $(a).text();
        makeId = $(a).attr("id");
        var type = $(a).attr("type").toString();
        $("#imgLoaderModel").show();
        $.ajax({
            type: "POST",
            url: "/ajaxpro/Bikewale.Ajax.AjaxCompareBikes,Bikewale.ashx",
            data: '{"makeId":"' + makeId + '", "compareBikes":"new", "type" : "' + type + '"}',
            beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetNewBikeModels"); },
            success: function (response) {
                $("#imgLoaderModel").hide();
                var response = eval('(' + response + ')');
                if (response != "") {
                    if (type == "1") {
                        makeName1 = $(a).attr("MaskingName");
                        $("#divForPopup ul").attr("type", "ddlModel1").html(response.value);
                        $("#divForPopup ul").attr("type", "ddlModel1").listview().listview('refresh');
                    }
                    else if (type == "2") {
                        makeName2 = $(a).attr("MaskingName");
                        $("#divForPopup ul").attr("type", "ddlModel2").html(response.value);
                        $("#divForPopup ul").attr("type", "ddlModel2").listview().listview('refresh');
                    }
                }
                    $(".divModel").show();
                    window.scrollTo(0, 1);
                    $(".divMake").hide();
                    $(".divVersion").hide();
                //}
            }
        });
    }

    function ShowVersion(a) {
        var retVal = "";
        bikeName += " " + $(a).text();
        modelId = $(a).attr("id").split('_')[0];
        var type = $(a).attr("type");
        $.ajax({
            type: "POST",
            url: "/ajaxpro/Bikewale.Ajax.AjaxCompareBikes,Bikewale.ashx",
            data: '{"modelId":"' + modelId + '", "compareBikes":"new"}',
            beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetVersions"); },
            success: function (response) {
                var jsonString = eval('(' + response + ')');
                var resObj = eval('(' + jsonString.value + ')');
                if (resObj.Table.length > 0);
                {
                    for (var i = 0; i < resObj.Table.length; i++) {
                        if (type == "1")
                            retVal += "<li><a onclick=\"ShowBikeName(this);\" id ='" + resObj.Table[i].Value + "' type='1'>" + resObj.Table[i].Text + "</a></li>";
                        else if (type == "2")
                            retVal += "<li><a onclick=\"ShowBikeName(this);\" id ='" + resObj.Table[i].Value + "' type='2'>" + resObj.Table[i].Text + "</a></li>";
                    }
                }
                if (retVal != "") {
                    if (type == "1") {
                        modelName1 = $(a).attr('id').split('_')[1];
                        $("#divForPopup ul").attr("type", "ddlVersion1").html(retVal);
                        $("#divForPopup ul").attr("type", "ddlVersion1").listview().listview('refresh');
                    }
                    else if (type == "2") {
                        modelName2 = $(a).attr('id').split('_')[1];
                        $("#divForPopup ul").attr("type", "ddlVersion2").html(retVal);
                        $("#divForPopup ul").attr("type", "ddlVersion2").listview().listview('refresh');
                    }
                    $(".divVersion").show();
                    window.scrollTo(0, 1);
                    $(".divMake").hide();
                    $(".divModel").hide();
                }
            }
        });
    }

    function ShowBikeName(a) {
        var type = "";

        $(".divMake").hide();
        $(".divModel").hide();
        $(".divVersion").hide();
        $("#divForPopup").hide();
        $("#divParentPageContainer").show();
        bikeName += " " + $(a).text();
        type = $(a).attr("type");

        var element = $('<div class="selected-bike position-rel"><span class="bwmsprite cancel-select cross-sm-dark-grey cur-pointer position-abt pos-top5 pos-right5"></span><img src="http://imgd4.aeplcdn.com//210x118//bw/models/benelli-tnt25.jpg" border="0" /><p class="selected-bike-label text-bold text-truncate font12 margin-bottom5">' + bikeName + '</p><p class="text-truncate text-light-grey font11">Ex-showroom, Mumbai</p><p class="text-default"><span class="bwmsprite inr-xsm-icon"></span>&nbsp;<span class="text-bold font16">81,618</span></p></div>');
        
        if (type == "1") {
            formatedMake1 = formatURL(makeName1);
            formatedModel1 = modelName1;
            $("#hdnVersionId1").val($(a).attr("id"));

            $(divMakeddl1).find('.compare-box-placeholder').hide();
            $(divMakeddl1).append(element);
        }
        else if (type == "2") {
            formatedMake2 = formatURL(makeName2);
            formatedModel2 = modelName2;
            $("#hdnVersionId2").val($(a).attr("id"));

            $(divMakeddl2).find('.compare-box-placeholder').hide();
            $(divMakeddl2).append(element);
        }
        $('#divListContainer, #divListContainer2').hide();
    }

    function VerifyVersion() {
        var isError = false;
        if (formatedMake1 && formatedMake2 && formatedModel1 && formatedModel2) {
            var ver1 = $("#hdnVersionId1").val();
            var ver2 = $("#hdnVersionId2").val();
            if (ver1 == "-1" && ver2 == "-1") {
                $("#spnError").html("Please select bikes to compare <br>");
                $("#popupDialog").popup("open");
                isError = true;
            }
            else if (ver1 == ver2) {
                $("#spnError").html("Please choose different bikes for comparison <br>");
                $("#popupDialog").popup("open");
                isError = true;
            }
        }
        else {
            $("#spnError").html("Please select bikes to compare <br>");
            $("#popupDialog").popup("open");
            isError = true;
        }

        if (isError)
            return false;
        else
            GenerateURL();
    }

    function GenerateURL() {
        var makeArr = new Array;
        var modelArr = new Array;
        var versionArr = new Array;
        if ($("#hdnVersionId1").val() > 0) {
            makeArr.push(formatedMake1);
            modelArr.push(formatedModel1);
            versionArr.push($("#hdnVersionId1").val());
        }
        if ($("#hdnVersionId2").val() > 0) {
            makeArr.push(formatedMake2);
            modelArr.push(formatedModel2);
            versionArr.push($("#hdnVersionId2").val());
        }

        var url = '';
        var qs = '';
        for (var i = 0; i < makeArr.length; i++) {
            if (i != makeArr.length - 1) {
                url += makeArr[i] + '-' + modelArr[i] + '-vs-';
                qs += 'bike' + (i + 1).toString() + '=' + versionArr[i] + '&';
            }
            else {
                url += makeArr[i] + '-' + modelArr[i];
                qs += 'bike' + (i + 1).toString() + '=' + versionArr[i];
            }
        }
        $("#hdnMake1").val(formatedMake1);
        $("#hdnMake2").val(formatedMake2);
        $("#hdnModel1").val(formatedModel1);
        $("#hdnModel2").val(formatedModel2);
        $("#hdnBikeName1").val($("#divMakeddl1 p").html());
        $("#hdnBikeName2").val($("#divMakeddl2 p").html());
        location.href = "/m/comparebikes/" + url + '/?' + qs;
    }

    function formatURL(str) {
        str = str.toLowerCase();
        str = str.replace(/[^0-9a-zA-Z]/g, '');
        return str;
    }

    function CloseWindow() {
        $(".divMake").hide();
        $(".divModel").hide();
        $(".divVersion").hide();
        $("#divForPopup").hide();
        $("#divParentPageContainer").show();
        $('#divListContainer, #divListContainer2').hide();
    }

    $('.compare-box').on('click', '.cancel-select', function (event) {
        var selectedBike = $(this).closest('.selected-bike'),
            compareBox = $(this).closest('.compare-box');

        selectedBike.siblings('.compare-box-placeholder').show();
        selectedBike.remove();
        if ($(compareBox).attr('id') == 'divMakeddl1') {
            formatedMake1 = null;
            formatedModel1 = null;
        }
        else {
            formatedMake2 = null;
            formatedModel2 = null;
        }
    });

</script>
<!-- #include file="/includes/footermobile.aspx" -->
