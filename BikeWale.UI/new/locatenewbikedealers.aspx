<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.New.LocateNewBikeDealers" %>

<%@ Register TagPrefix="CE" TagName="CalculateEMIMin" Src="/controls/CalculateEMIMin.ascx" %>
<%@ Register TagPrefix="TIP" TagName="TipsAdvicesMin" Src="/controls/TipsAdvicesMin.ascx" %>
<%@ Register TagPrefix="CM" TagName="ComparisonMin" Src="/controls/comparisonmin.ascx" %>
<%@ Register TagPrefix="BP" TagName="InstantBikePrice" Src="/controls/instantbikeprice.ascx" %>
<%@ Register TagPrefix="NBL" TagName="NewBikeLaunches" Src="/controls/NewBikeLaunches.ascx" %>
<%@ Register TagPrefix="uc" TagName="UpcomingBikes" Src="~/controls/UpcomingBikesMin.ascx" %>
<%@ Import Namespace="Bikewale.Common" %>
<%
    title = "New Bike Dealers in India - Locate Authorized Showrooms - BikeWale";
    keywords = "new bike dealers, new bike showrooms, bike dealers, bike showrooms, showrooms, dealerships, price quote";
    description = "Locate New bike dealers and authorized bike showrooms in India. Find new bike dealer information for more than 200 cities. Authorized company showroom information includes full address, phone numbers, email address, pin code etc.";
    canonical = "http://www.bikewale.com/new/locate-dealers/";
    alternate = "http://www.bikewale.com/m/new/locate-dealers/";
    AdId = "1395986297721";
    AdPath = "/1017752/BikeWale_New_";
%>
<!-- #include file="/includes/headNew.aspx" -->

<div class="container_12">
    <div class="grid_12">
        <ul class="breadcrumb">
            <li>You are here: </li>
            <li><a href="/">Home</a></li>
            <li class="fwd-arrow">&rsaquo;</li>
            <li><a href="/new/">New Bikes</a></li>
            <li class="fwd-arrow">&rsaquo;</li>
            <li class="current"><strong>New Bike Dealers / Showrooms in India</strong></li>
        </ul>
        <div class="clear"></div>
    </div>
    <div class="grid_8 margin-top10">
        <!--    Left Container starts here -->
        <h1>New Bike Dealers / Showrooms in India</h1>
        <p class="padding-top10">Find new bike dealers and authorized showrooms in India. New bike dealer information for more than 200 cities is available. Click on a bike manufacturer name to get the list of its authorized dealers in India.</p>
        <div class="margin-top15">
            <h2>Search Dealers by City & Manufacturer</h2>
            <div class="margin-top10 content-block grey-bg">
                <span>
                    <asp:dropdownlist id="cmbMake" runat="server" />
                </span>
                <span class="margin-left10 margin-right20">
                    <asp:dropdownlist id="cmbCity" enabled="false" runat="server">
					        <asp:ListItem Text="--Select City--" Value="-1" />
				        </asp:dropdownlist>
                </span>
                <input type="hidden" id="hdn_drpCity" runat="server" />
                <span class="action-btn"><a onclick="javascript:locateDealer(this)">Locate Dealers</a></span>
            </div>
        </div>
        <div class="margin-top15">
            <h2>Browse Dealers by Manufacturer</h2>
            <div class="margin-top5">
                <asp:datalist id="dlShowMakes" runat="server" repeatcolumns="3" width="100%" repeatdirection="Horizontal" cellspacing="2" cellpadding="2">
				            <itemtemplate><a href="/new/<%# DataBinder.Eval(Container.DataItem, "MaskingName").ToString()%>-dealers/">					
						            <%# DataBinder.Eval(Container.DataItem, "BikeMake")%> Dealers</a> <span class="text-grey">(<%# DataBinder.Eval(Container.DataItem, "TotalCount")%>)</span><br>						
					            </itemtemplate>					 
			            </asp:datalist>
            </div>
        </div>
        <%--<div class="grid_4 alpha margin-top20">
                    <div class="grey-bg content-block"><TIP:TipsAdvicesMin ID="ctrl_TipsAdvices" runat="server" TopRecords="10"/></div>
                </div>--%>
        <%--<div class="grid_4 grey-bg alpha omega margin-left10 margin-top20">
                    <NBL:NewBikeLaunches ID="ctrl_NewBikeLaunches" runat="server" />
                    <div class="clear"></div>
                </div>--%>
        <div class="grid_4 alpha omega margin-top20">
            <NBL:NewBikeLaunches ID="ctrl_NewBikeLaunches" TopCount="3" runat="server" />
            <div class="clear"></div>
        </div>
        <div class="grid_4 margin-top20">
            <uc:UpcomingBikes ID="ucUpcoming" runat="server" HeaderText="Upcoming Bikes" TopRecords="2" ControlWidth="grid_2" />
        </div>
        <div class="grid_12 comparison-container alpha omega margin-top5" style="border: 1px solid #E2E2E2;">
            <CM:ComparisonMin ID="ctrl_ComparisonMin" runat="server" ShowCompButton="true" />
        </div>
    </div>
    <!--    Left Container ends here -->
    <div class="grid_4">
        <!--    Right Container starts here -->
        <%--<div class="margin-top15">
                    <!-- BikeWale_NewBike/BikeWale_NewBike_HP_300x250 -->
                    <!-- #include file="/ads/Ad300x250.aspx" -->
                </div> --%>
        <div class="margin-top15 light-grey-bg content-block border-radius5 padding-bottom20  margin-top5">
            <BP:InstantBikePrice runat="server" ID="InstantBikePrice" />
        </div>
        <div class="light-grey-bg content-block border-radius5 margin-top10 padding-bottom20 margin-top15">
            <CE:CalculateEMIMin runat="server" ID="CalculateEMIMin" />
        </div>
        <div class="clear"></div>
        <%--<div class="margin-top15">
                    <!-- BikeWale_NewBike/BikeWale_NewBike_HP_300x250 -->
                    <!-- #include file="/ads/Ad300x250BTF.aspx" -->
                </div>--%>
    </div>
    <!--    Right Container ends here -->
</div>
<script type="text/javascript">
    $(document).ready(function () {
        if ($("#cmbMake").val().split('_')[0] > 0) {
            FillCity();
        }
    });

    $("#cmbMake").change(function () {
        FillCity();
    });

    function FillCity() {
        var makeId = $("#cmbMake").val().split('_')[0];
        //alert(cityId);
        if (makeId != "0") {
            //alert(cityId);
            $.ajax({
                type: "POST",
                url: "/ajaxpro/Bikewale.Ajax.AjaxCommon,Bikewale.ashx",
                data: '{"makeId":"' + makeId + '"}',
                beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetDealersCitiesListByMakeId"); },
                success: function (response) {
                    var responseJSON = eval('(' + response + ')');
                    var resObj = eval('(' + responseJSON.value + ')');

                    var dependentCmbs = new Array();
                    bindDropDownList(resObj, $("#cmbCity"), "hdn_drpCity", dependentCmbs, "--Select City--");
                }
            });
        } else {
            $("#cmbCity").val(0).attr("disabled", true);
        }
    }

    function locateDealer(e) {
        var cityId = $("#cmbCity").val().split('_')[0];
        var makeValueArray = $("#cmbMake").val();
        var makeId = makeValueArray.split('_')[0];
        var city = $("#cmbCity option:selected").val().split('_')[1];
        //var make = replaceAll(replaceAll($("#cmbMake option:selected").text(), " ", ""), "-", "");
        var make = makeValueArray.split('_')[1];
        if (Number(cityId) <= 0 && Number(makeId) <= 0) {
            alert("Please select city and make to locate dealers.");    //change the message
            return false;
        }
        else if (Number(cityId) <= 0) {
            alert("Please select city to locate dealers.");
            return false;
        }
        else if (Number(makeId) <= 0) {
            alert("Please select make to locate dealers.");
            return false;
        }
        //alert(Bikewale.Common.UrlRewrite.FormatSpecial(city));
        //FormatSpecial(city);
        //alert(city);
        //alert(city.toLowerCase());
        location.href = "/new/" + make.toLowerCase() + "-dealers/" + cityId + "-" + city + ".html";
    }

    function replaceAll(str, rep, repWith) {
        var occurrence = str.indexOf(rep);

        while (occurrence != -1) {
            str = str.replace(rep, repWith);
            occurrence = str.indexOf(rep);
        }
        return str;
    }
</script>
<style type="text/css">
    .grid_8.comparison-container .container {
        width: 620px;
        padding-bottom: 10px;
    }
</style>
<!-- #include file="/includes/footerInner.aspx" -->
