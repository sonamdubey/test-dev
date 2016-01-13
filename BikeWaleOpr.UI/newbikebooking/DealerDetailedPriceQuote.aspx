<%@ Page Language="C#" AutoEventWireup="false" Inherits="BikewaleOpr.newbikebooking.DealerDetailedPriceQuote" trace="false" %>
<!-- #Include file="/includes/headerNew.aspx" -->
<%--<script type="text/javascript" src="/src/common/common.js?V1.1"></script>--%>
<script type="text/ecmascript" src="/src/AjaxFunctions.js"></script>
<script src="/src/knockout.js" type="text/javascript"></script>
<style>
    .dtItem{border-bottom:1px solid #808080;}
    select { padding:10px; cursor:pointer;}
    .footer {margin-top:20px;}
    .top_info_left { text-transform:capitalize; }
    .dtItem {font-size:larger;}
</style>
<div>
    <div>
        <div class="margin-top10">
            <asp:DropDownList ID="ddlMake" runat="server"><asp:ListItem Value="0" Text="--Selected Make--" ></asp:ListItem></asp:DropDownList>
            <select id="ddlModel"><option value="0">--Selected Model--</option></select>
            <select id="ddlVersion"><option value="0">--Selected Version--</option></select>
            <select id="ddlCity"><option value="0">--Selected City--</option></select>
            <select id="ddlArea"><option value="0">--Selected Area--</option></select>
            <input type="button" id="btnGetPriceQuote" value="Get Dealer Price Quote" style="padding:10px; margin-left:20px; cursor:pointer;"/>
        </div>       
    </div>
    <div id="DealerDetailsList">
        <table class="margin-top10" rules="all" cellspacing="0" cellpadding="5" style="border-width:1px;border-style:solid;width:100%;border-collapse:collapse;">
            <thead>
                <tr class="dtHeader">
                    <td>
                        DealerId
                    </td>
                    <td>
                        Price List
                    </td>
                    <td>
                        Offer List
                    </td>
                    <td style="width:400px;">
                        Dealer Details
                    </td>
                    <td>
                        Booking Amount
                    </td>
                    <td>
                        Availability
                    </td>
                    <td>
                        Availability By Color
                    </td>
                </tr>
            </thead>
            <tbody data-bind="template: { name: 'DealerList', foreach: DealersDetails }">
                
            </tbody>
        </table>
    </div>

    <script type="text/html" id="DealerList">
        <tr class="dtItem">
            <td>
                <div data-bind="text : dealerDetails.dealerId"></div>
            </td>
            <td>
                <div data-bind="template:{foreach : priceList}">
                    <div>
                        <span data-bind="text: CategoryName"></span><span> : </span><span data-bind="    text: FormatPrice(Price())"></span>
                    </div>
                </div>
                <hr />
                <div>
                    <span>Total Price</span><span> : </span><span data-bind="text : GetTotalPrice(priceList())"></span>
                </div>
            </td>
            <td>
                <div data-bind="template: { foreach: offerList }">
                    <ul >
                        <li style="list-style:circle;list-style-position:inside;" data-bind="text: OfferText"></li>
                    </ul>
                </div>
            </td>
            <td>
                <div>
                    <span><b>Organization</b></span><span> : </span><span data-bind="text: dealerDetails.organization"></span>
                </div>
                <div>
                    <span><b>Working Time</b></span><span> : </span><span data-bind="text: dealerDetails.workingTime"></span>
                </div>
                <div>
                    <span><b>Address</b></span><span> : </span><span data-bind="text: dealerDetails.address"></span>
                </div>
                <div>
                    <span><b>Dealer Name</b></span><span> : </span><span data-bind="text: dealerDetails.dealerName"></span>
                </div>
                <div>
                    <span><b>Email-Id</b></span><span> : </span><span data-bind="text: dealerDetails.emailId"></span>
                </div>
                <div>
                    <span><b>Mobile No.</b></span><span> : </span><span data-bind="text: dealerDetails.mobileNo"></span>
                </div>
            </td>
            <td>
                <span>Rs. </span><span data-bind="text: bookingAmount"></span>
            </td>
            <td>
                <div data-bind="text: GetAvailabilty(availability())"></div>
            </td>
            <td>
                <div  data-bind="template: { foreach: availabilityByColor }">
                    <div style="border-bottom:1px #808080 dashed;margin:10px;" data-bind="attr: {colorId : ColorId}">
                        <div><span><b>Color : </b></span><span data-bind="text:ColorName"></span></div>
                        <div><span><b>Availability : </b></span><span data-bind="text: GetAvailabilty(NoOfDays())"></span></div>
                        <div style="display:inline-flex;margin-bottom:10px;" data-bind="html: GetColors(HexCode()) "></div>
                    </div>
                </div>
            </td>
        </tr>
    </script>
</div>
<script type="text/javascript">
    var ddlModel = $("#ddlModel"),
            ddlVersion = $("#ddlVersion"),
            ddlCity = $("#ddlCity"),
            ddlArea = $("#ddlArea"),
            ddlMake = $("#ddlMake"),
            btnGetPriceQuote = $("#btnGetPriceQuote");
    var abApiHostUrl = '<%= System.Configuration.ConfigurationManager.AppSettings["ABApiHostUrl"]%>';
    var dealerList = '';

    var DealerViewModel = function (model) {
        ko.mapping.fromJS(model, {}, this);
    };

    $(document).ready(function () {
        $('#DealerDetailsList').hide();
    });
    
    ddlMake.change(function () {
        var makeId = $(this).val()
        fillmodels(makeId);
    });

    ddlModel.change(function () {
        fillVersions($(this).val())
        fillCities($(this).val());
    });

    ddlCity.change(function () {
        fillAreas($(this).val());
    });

    btnGetPriceQuote.click(function () {
        var versionId = ddlVersion.val(), areaId = ddlArea.val(), cityId = ddlCity.val();
        var element = document.getElementById('DealerDetailsList');

        if (versionId > 0 && areaId > 0) {
            $.ajax({
                type: "GET",
                url: abApiHostUrl + "/api/dealerpricequote/GetAllDealerPriceQuotes/?versionId=" + versionId + "&cityid=" + cityId + "&areaid=" + areaId,
                datatype: "json",
                success: function (response) {

                    ko.cleanNode(element);
                    if (response != null) {
                        ko.applyBindings(new DealerViewModel(response), element);
                        $('#DealerDetailsList').show();
                    }
                    else {
                        $('#DealerDetailsList').hide();
                        alert("No Dealer Present For perticular Area");
                    }
                }
            });
        }
        else {
            alert("Please select all fields.");
        }
    });

    function fillmodels(makeid) {
        var requestType = "PRICEQUOTE";
        if (makeid > 0) {
            ddlModel.val("0").attr("disabled", true);
            ddlVersion.val("0").attr("disabled", true);
            ddlCity.val("0").attr("disabled", true);
            ddlArea.val("0").attr("disabled", true);

            $.ajax({
                type: "POST",
                url: "/ajaxpro/BikeWaleOpr.Common.AjaxCommon,BikewaleOpr.ashx",
                data: '{"makeId":"' + makeid + '","requestType":"'+requestType+'"}',
                beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetModels"); },
                success: function (response) {
                    var responseJSON = eval('(' + response + ')');
                    var resObj = eval('(' + responseJSON.value + ')');
                    bindDropDownList(resObj, ddlModel, "", "--Select Model--");
                }
            });
        }
        else {
            ddlModel.val("0").attr("disabled", true);
            ddlVersion.val("0").attr("disabled", true);
            ddlCity.val("0").attr("disabled", true);
            ddlArea.val("0").attr("disabled", true);
        }
    }

    function fillVersions(modelId) {
        var requestType = "PRICEQUOTE";
        if (modelId > 0) {
            ddlVersion.val("0").attr("disabled", true);
            ddlArea.val("0").attr("disabled", true);

            $.ajax({
                type: "POST",
                url: "/ajaxpro/BikeWaleOpr.Common.AjaxCommon,BikewaleOpr.ashx",
                data: '{"modelId":"' + modelId + '","requestType":"' + requestType + '"}',
                beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetVersions"); },
                success: function (response) {
                    var responseJSON = eval('(' + response + ')');
                    var resObj = eval('(' + responseJSON.value + ')');
                    bindDropDownList(resObj, ddlVersion, "", "--Select Version--");
                }
            });
        }
        else {
            ddlVersion.val("0").attr("disabled", true);
            ddlCity.val("0").attr("disabled", true);
            ddlArea.val("0").attr("disabled", true);
        }
    }

    function fillCities(modelId) {
        if (modelId > 0) {
            ddlCity.val("0").attr("disabled", true);
            ddlArea.val("0").attr("disabled", true);
            $.ajax({
                type: "POST",
                url: "/ajaxpro/BikeWaleOpr.Common.AjaxCommon,BikewaleOpr.ashx",
                data: '{"modelId":"' + modelId + '"}',
                beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetPriceQuoteCities"); },
                success: function (response) {
                    if (response.length > 1) {
                        var responseJSON = eval('(' + response + ')');
                        var resObj = eval('(' + responseJSON.value + ')');
                        bindDropDownList(resObj, ddlCity, "", "--Select City--");
                    }
                    else {
                        $('#DealerDetailsList').hide();
                        alert("No Dealer Present For perticular Area");
                    }
                }
            });
        }
        else {
            ddlCity.val("0").attr("disabled", true);
            ddlArea.val("0").attr("disabled", true);
        }
    }

    function fillAreas(cityId) {
        if (cityId > 0) {
            ddlArea.val("0").attr("disabled", true);
            $.ajax({
                type: "POST",
                url: "/ajaxpro/BikeWaleOpr.Common.AjaxCommon,BikewaleOpr.ashx",
                data: '{"cityId":"' + cityId + '"}',
                beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetAreas"); },
                success: function (response) {

                    if (response.length > 1) {
                        var responseJSON = eval('(' + response + ')');
                        var resObj = eval('(' + responseJSON.value + ')');
                        bindDropDownList(resObj, ddlArea, "", "--Select Area--");
                    }
                    else {
                        $('#DealerDetailsList').hide();
                        alert("No Dealer Present For perticular Area");
                    }
                }
            });
        }
        else {
            ddlArea.val("0").attr("disabled", true);
        }
    }

    function GetTotalPrice(priceList)
    {
        var totalPrice = 0;
        for (var i = 0; i < priceList.length; i++) {
            totalPrice += parseInt(priceList[i].Price());
        }
        return FormatPrice(totalPrice);
    }

    function GetAvailabilty(noOfDays)
    {
        var availability = '';
        if (noOfDays > 0)
            availability = noOfDays + ' days.';
        else if(noOfDays < 0)
            availability ='Not Available';
        else
            availability = 'In Stock.';

        return availability;
    }

    function FormatPrice(price) {
        var thMatch = /(\d+)(\d{3})$/;
        var thRest = thMatch.exec(price);
        if (!thRest) return price;
        return (thRest[1].replace(/\B(?=(\d{2})+(?!\d))/g, ",") + "," + thRest[2]);
    }
    function GetColors(objColors)
    {
        var htmlTemp = '';
        for (var index = 0; index < objColors.length; index++) {
            htmlTemp += "<div style='margin-right: 10px;border: 1px solid #AAAAAA;width:20px;height:20px;background-color:#" + objColors[index] + "'></div>";
        }
        return htmlTemp;
    }
</script>
<!-- #Include file="/includes/footerNew.aspx" -->
