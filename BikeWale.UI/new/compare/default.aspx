<%@ Page Inherits="Bikewale.New.ComparisonChoose" Trace="false" Debug="false" AutoEventWireup="false" Language="C#" EnableEventValidation="false" %>

<%@ Register Src="~/controls/ComparisonMin.ascx" TagName="CompareBikes" TagPrefix="BW" %>
<%@ Register TagPrefix="uc" TagName="InstantBikePrice" Src="/controls/instantbikeprice.ascx" %>
<%@ Register TagPrefix="uc" TagName="BrowseUserReviews" Src="~/controls/BrowseUserReviews.ascx" %>
<%
    title = "Compare Bikes | New Bikes Comparisons in India";
    description = "Comparing Indian bikes was never this easy. BikeWale presents you the easiest way of comparing bikes. Choose two or more bikes to compare them head-to-head.";
    keywords = "bikes compare, compare bikes, compare bikes, bike comparison, bikes comparison india";
    canonical = "http://www.bikewale.com/comparebikes/";
    alternate = "http://www.bikewale.com/m/comparebikes/";
    AdId = "1395986297721";
    AdPath = "/1017752/BikeWale_CompareBikes_";
    //modified by SajalGupta for unfilled impression of ads on 04 Aug 2016.
    isAd300x250Shown = false;
%>
<!-- #include file="/includes/headNew.aspx" -->
<div class="container_12">
    <div class="grid_12">
        <ul class="breadcrumb">
            <li>You are here: </li>
            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                <a itemprop="url" href="/"><span itemprop="title">Home</span></a>
            </li>
            <li class="fwd-arrow">&rsaquo;</li>
            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                <a itemprop="url" href="/new/"><span itemprop="title">New</span></a>
            </li>
            <li class="fwd-arrow">&rsaquo;</li>
            <li class="current"><strong>Compare Bikes in India</strong></li>
        </ul>
        <div class="clear"></div>
    </div>
    <div class="grid_8 margin-top10">
        <!--    Left Container starts here -->
        <h1>Compare Bikes in India</h1>
        <div class="margin-top10">
            <strong>Choose at least two Bikes</strong> of your choice to see how they compare on price, features, and performance. 
        </div>
        <table width="100%" border="0">
            
            <tr>
                <td class="padding-bottom10">
                    <span class="subHeading margin-right5">Bike-1</span>
                    <asp:dropdownlist id="cmbMake" runat="server" tabindex="1" />
                    -
					<asp:dropdownlist id="cmbModel" enabled="false" runat="server" tabindex="2">
							<asp:ListItem Value="0" Text="--Select--" />
					</asp:dropdownlist>
                    -
					<asp:dropdownlist id="cmbVersion" enabled="false" runat="server" tabindex="3">
						<asp:ListItem Value="0" Text="--Select--" />
					</asp:dropdownlist>
                </td>
            </tr>
            <tr>
                <td class="alt padding-bottom10">
                    <span class="subHeading margin-right5">Bike-2</span>
                    <asp:dropdownlist id="cmbMake1" runat="server" tabindex="1" />
                    -
					<asp:dropdownlist id="cmbModel1" enabled="false" runat="server" tabindex="2">
							<asp:ListItem Value="0" Text="--Select--" />
					</asp:dropdownlist>
                    -
					<asp:dropdownlist id="cmbVersion1" enabled="false" runat="server" tabindex="3">
						<asp:ListItem Value="0" Text="--Select--" />
					</asp:dropdownlist>
                </td>
            </tr>
            <tr>
                <td class="padding-bottom10">
                    <span class="subHeading margin-right5">Bike-3</span>
                    <asp:dropdownlist id="cmbMake2" runat="server" tabindex="1" />
                    -
					<asp:dropdownlist id="cmbModel2" enabled="false" runat="server" tabindex="2">
							<asp:ListItem Value="0" Text="--Select--" />
					</asp:dropdownlist>
                    -
					<asp:dropdownlist id="cmbVersion2" enabled="false" runat="server" tabindex="3">
						<asp:ListItem Value="0" Text="--Select--" />
					</asp:dropdownlist>
                </td>
            </tr>
            <tr>
                <td class="alt padding-bottom10">
                    <span class="subHeading margin-right5">Bike-4</span>
                    <asp:dropdownlist id="cmbMake3" runat="server" tabindex="1" />
                    -
					<asp:dropdownlist id="cmbModel3" enabled="false" runat="server" tabindex="2">
							<asp:ListItem Value="0" Text="--Select--" />
					</asp:dropdownlist>
                    -
					<asp:dropdownlist id="cmbVersion3" enabled="false" runat="server" tabindex="3">
						<asp:ListItem Value="0" Text="--Select--" />
					</asp:dropdownlist>
                </td>
            </tr>
        </table>
        <div class="margin-top15">
            <div class="buttons text-center">
                <asp:button id="btnCompare" cssclass="buttons" text="Compare" runat="server" />
            </div>
            <span id="spn" class="error"></span>
        </div>
        <div class="margin-top15">
            <div class="grey-bg content-block">
                <uc:BrowseUserReviews ID="ucUserReviews" runat="server" />
            </div>
            <div class="clear"></div>
        </div>
        <div class="margin-top15">
            <div class="grid_12 alpha " style="border: 1px solid #E2E2E2;">
                <h2 class="text-bold text-center margin-top50 margin-bottom30 font28">Compare Now</h2>
                <BW:CompareBikes ID="ctrlCompareBikes" runat="server" ShowCompButton="false" />
            </div>
            <div class="clear"></div>
        </div>

    </div>
    <!--    Left Container ends here -->
    <div class="grid_4">
        <!--    Right Container starts here -->
        <%--<div>
            <!-- BikeWale_NewBike/BikeWale_NewBike_HP_300x250 -->
            <!-- #include file="/ads/Ad300x250.aspx" -->
        </div>--%>
        <div class="light-grey-bg content-block border-radius5 margin-top10 padding-bottom20 margin-top15">
            <uc:InstantBikePrice runat="server" ID="ucInstantBikePrice" />
        </div>

        <div>
            <!-- BikeWale_NewBike/BikeWale_NewBike_HP_300x250 -->
            <!-- #include file="/ads/Ad300x250BTF.aspx" -->
        </div>
    </div>
    <!--    Right Container ends here -->
</div>
<script type="text/javascript">
    $(document).ready(function () {
        $("select[id^='cmbMake']").change(function () {
            var objbikeMake = $(this);

            getBikeModels(objbikeMake);
        });

        $("select[id^='cmbModel']").change(function () {
            var objBikeMake = $(this).prev();
            var objBikeModel = $(this);
            getBikeVersions(objBikeMake, objBikeModel);
        });
    });

    function getBikeModels(objBikeMake) {
        var bikeMakeId = objBikeMake.val().split('_')[0];

        $.ajax({
            type: "POST",
            url: "/ajaxpro/Bikewale.Ajax.AjaxCompareBikes,Bikewale.ashx",
            data: '{"makeId":"' + bikeMakeId + '", "compareBikes":"New"}',
            beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetModels"); },
            success: function (response) {
                var responseJSON = eval('(' + response + ')');
                var resObj = eval('(' + responseJSON.value + ')');

                var dependentCmbs = new Array();

                bindDropDownList(resObj, objBikeMake.next(), "", dependentCmbs, "--Select Model--");
            }
        });

    }

    function getBikeVersions(objBikeMake, objBikeModel) {
        var bikeMakeId = objBikeMake.val().split('_')[0];
        var bikeModelId = objBikeModel.val().split('_')[0];

        $.ajax({
            type: "POST",
            url: "/ajaxpro/Bikewale.Ajax.AjaxCompareBikes,Bikewale.ashx",
            data: '{"modelId":"' + bikeModelId + '", "compareBikes":"New"}',
            beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetVersions"); },
            success: function (response) {
                var responseJSON = eval('(' + response + ')');
                var resObj = eval('(' + responseJSON.value + ')');

                var dependentCmbs = new Array();

                bindDropDownList(resObj, objBikeModel.next(), "", dependentCmbs, "--Select Version--");
            }
        });
    }
</script>
<script type="text/javascript" language="javascript">
    document.getElementById('btnCompare').onclick = verifyVersions;

    function verifyVersions() {
        var isSame = false;
        var isError = false;

        var selected = 0;
        var ver1 = 0, ver2 = 0, ver3 = 0, ver4 = 0;

        if (document.getElementById('cmbVersion').value > 0)
            selected++;
        if (document.getElementById('cmbVersion1').value > 0)
            selected++;
        if (document.getElementById('cmbVersion2').value > 0)
            selected++;
        if (document.getElementById('cmbVersion3').value > 0)
            selected++;

        if (selected < 2) {
            document.getElementById('spn').innerHTML = "Choose at least two bikes for comparison.";
            isError = true;
        }

        ver1 = document.getElementById('cmbVersion').value;
        ver2 = document.getElementById('cmbVersion1').value;
        ver3 = document.getElementById('cmbVersion2').value;
        ver4 = document.getElementById('cmbVersion3').value;

        if (ver1 > 0 && ver2 > 0)
            if (ver1 == ver2)
                isSame = true;
        if (ver1 > 0 && ver3 > 0)
            if (ver1 == ver3)
                isSame = true;
        if (ver1 > 0 && ver4 > 0)
            if (ver1 == ver4)
                isSame = true;
        if (ver2 > 0 && ver3 > 0)
            if (ver2 == ver3)
                isSame = true;
        if (ver2 > 0 && ver4 > 0)
            if (ver2 == ver4)
                isSame = true;
        if (ver3 > 0 && ver4 > 0)
            if (ver3 == ver4)
                isSame = true;

        if (!isError && isSame) {
            document.getElementById('spn').innerHTML = "Please choose different bikes for comparison.";
        }

        if (isError || isSame)
            return false;
    }
</script>
<!-- #include file="/includes/footerInner.aspx" -->
