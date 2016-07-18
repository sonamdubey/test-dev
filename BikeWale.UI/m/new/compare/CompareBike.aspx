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
<script type="text/javascript" src="http://st2.aeplcdn.com/bikewale/src/BikeWaleCommon.js?v=3.2"></script>
<style>
    a {text-decoration:none !important;}
</style>
    <input type="hidden" id="hdnMake1" runat="server" />
<input type="hidden" id="hdnMake2" runat="server" />
<input type="hidden" id="hdnModel1" runat="server" />
<input type="hidden" id="hdnModel2" runat="server" />
<input type="hidden" id="hdnBikeName1" runat="server" />
<input type="hidden" id="hdnBikeName2" runat="server" />
<input type="hidden" id="hdnVersionId1" runat="server" value="-1" />
<input type="hidden" id="hdnVersionId2" runat="server" value="-1"/>


    <div class="padding5">
        <div id="br-cr"  itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
            <a href="/m/new/" class="normal" itemprop="url"><span itemprop="title">New Bikes</span></a> &rsaquo; 
            <span class="lightgray">Compare Bikes</span>
        </div>
        <div style="position:relative;">
	        <h1>Compare Bikes</h1>
        </div>

    <div class="new-line5">
        <div id="divMakeddl1" style="border:1px solid #b3b4c6; background-color:white; padding:5px 10px;font-size:14px;text-align:center;" onclick="OpenPopup(this)">Select Make Model Version</div>
        <%--<div data-role="popup" id="popupDdlBike" data-overlay-theme="a" data-theme="c" data-dismissible="false"  class="ui-corner-all">--%>
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
        <div class="new-line5" style="text-align:center;">Vs</div>
        <div class="new-line5" id="divMakeddl2" style="border:1px solid #b3b4c6; background-color:white; padding:5px 10px;font-size:14px;text-align:center;" onclick="OpenPopup(this)">Select Make Model Version</div>
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
    </div>
    <%--<div class="new-line10" style="text-align:center;"><span class="linkButtonBig" onclick="VerifyVersion();">&nbsp;&nbsp;Compare&nbsp;&nbsp;</span></div>--%>
    <div class="new-line10 " style="text-align:center;">
        <a data-theme="b"  style="color : #fff !important;" data-rel="popup" data-role="button" data-transition="pop" data-position-to="window" onclick="VerifyVersion();">Compare</a>
    </div>

    <div data-role="popup" id="popupDialog" data-overlay-theme="a" data-theme="c" data-dismissible="false"  class="ui-corner-all">
        <div data-role="header" data-theme="a" class="ui-corner-top" style="background-color:#000;">
            <h1 class="ui-title" style="color:#fff;" role="heading" aria-level="1">Error !!</h1>
        </div>
        <div data-role="content" data-theme="d" class="ui-corner-bottom ui-content" style="background-color:#fff;">
            <span id="spnError" style="font-size:14px;line-height:20px;" class="error" ></span>
            <a href="#" data-role="button" data-rel="back" data-theme="c" data-mini="true">OK</a>
        </div>
    </div>
    <div style="padding-bottom:5px;" class="new-line15">
        <div class="new-line10">
                <CB:CompareBike ID="NewBikeDealers1" runat="server" ></CB:CompareBike>
        </div>
    </div>
    </div>


<script type="text/javascript">
    
    $(document).ready(function () {
        var bikeName = "";
        if ($("#hdnVersionId1").val() != "-1" && $("#hdnVersionId2").val() != "-1") {
            formatedMake1 = $("#hdnMake1").val();
            formatedMake2 = $("#hdnMake2").val();
            formatedModel1 = $("#hdnModel1").val();
            formatedModel2 = $("#hdnModel2").val();
            $("#divMakeddl1").html($("#hdnBikeName1").val());
            $("#divMakeddl2").html($("#hdnBikeName2").val());
        }
    });

    function OpenPopup(divMakeddl) {
        $("#divParentPageContainer").hide();
        $("#divForPopup").attr("style", "z-index:1002;width:100%;height:100%;position:absolute;");
        $(divMakeddl).next().show();
        //$("#divListContainer").show();
        $(".divMake").show();
        //$("#divForPopup").html($("#divListContainer1").html());
        $("#divForPopup").html($(divMakeddl).next().html());
        $(".divModel").hide();
        $(".divVersion").hide();
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
        if (type == "1") {
            formatedMake1 = formatURL(makeName1);
            formatedModel1 = modelName1;
            $("#hdnVersionId1").val($(a).attr("id"));
            $("#divMakeddl1").html(bikeName);
        }
        else if (type == "2") {
            formatedMake2 = formatURL(makeName2);
            formatedModel2 = modelName2;
            $("#hdnVersionId2").val($(a).attr("id"));
            $("#divMakeddl2").html(bikeName);
        }
    }

    function VerifyVersion() {
        var isError = false;
        if (typeof formatedMake1 != "undefined" && typeof formatedMake2 != "undefined" && typeof formatedModel1 != "undefined" && typeof formatedModel2 != "undefined") {
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
        $("#hdnBikeName1").val($("#divMakeddl1").html());
        $("#hdnBikeName2").val($("#divMakeddl2").html());
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
    }

</script>
<!-- #include file="/includes/footermobile.aspx" -->
