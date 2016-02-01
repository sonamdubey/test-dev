<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageDealerCampaigns.aspx.cs" Inherits="BikewaleOpr.newbikebooking.ManageDealerCampaigns" %>
<!-- #Include file="/includes/headerNew.aspx" -->
<%--<script type="text/javascript" src="/src/common/common.js?V1.1"></script>--%>
<script type="text/ecmascript" src="/src/AjaxFunctions.js"></script>
<script src="/src/knockout.js" type="text/javascript"></script>
<style>
    .dtItem{border-bottom:1px solid #808080;}
    select { padding:10px; cursor:pointer;vertical-align:top}
    .footer {margin-top:20px;}
    .top_info_left { text-transform:capitalize; }
    .dtItem {font-size:larger;}
</style>
<div>
    <div>
        <h3> Manage Manufacturer's Campaigns</h3>
        <hr />
        <input name="action" type="radio" value="1"/> Add Campaign
        <input name="action" type="radio" value="2" /> Edit Campaign
        <hr />
        <div class="margin-top10">
            <asp:DropDownList ID="ddlMake" runat="server"><asp:ListItem Value="0" Text="--Selected Make--" ></asp:ListItem></asp:DropDownList>
            <select id="ddlModel" multiple><option value="0">--Selected Model--</option></select>
             <select id="ddlManufacturers" ><option value="0">--Select Dealer(Manufacturer)--</option></select>
            <input type="button" id="btnGetPriceQuote" value="Go" style="padding:10px; margin-left:20px; cursor:pointer;"/>
           
        </div> 
        <hr />      
    </div>
    </div>
<!-- #Include file="/includes/footerNew.aspx" -->
<script>

    var ddlModel = $("#ddlModel"), ddlMake = $("#ddlMake"), btnGetPriceQuote = $("#btnGetPriceQuote"), ddlManufacturers = $("#ddlManufacturers");
    ddlMake.change(function () {
        var makeId = $(this).val()
        fillmodels(makeId);
    });

    ddlModel.change(function () {
        fillManuFacturers(modelIds);
    });


    function fillmodels(makeid) {
        var requestType = "PRICEQUOTE";
        if (makeid > 0) {
            ddlModel.val("0").attr("disabled", true);
            $.ajax({
                type: "POST",
                url: "/ajaxpro/BikeWaleOpr.Common.AjaxCommon,BikewaleOpr.ashx",
                data: '{"makeId":"' + makeid + '","requestType":"' + requestType + '"}',
                beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetModels"); },
                success: function (response) {
                    var responseJSON = eval('(' + response + ')');
                    var resObj = eval('(' + responseJSON.value + ')');
                    bindDropDownList(resObj, ddlModel, "", "--Select Model--");
                },
                complete : function(xhr)
                {
                    if(xhr.status==404 || shr.status==204)
                    {
                      alert("")
                    }
                }
            });
        }
        else {
            ddlModel.val("0").attr("disabled", true);
        }
    }

    function fillManuFacturers(modelIds)
    {
        var modelIds = "";
        var arr = $.grep(arr, function (value) {
            return value != "0";
        });

        if (arr.length > 0)
        {
            modelIds = arr.toString();
            if (makeid > 0 && modelIds!="0") {
                $.ajax({
                    type: "POST",
                    url: "/ajaxpro/BikeWaleOpr.Common.AjaxCommon,BikewaleOpr.ashx",
                    data: '{"makeId":"' + makeid + '","requestType":"' + requestType + '"}',
                    beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetModels"); },
                    success: function (response) {
                        var responseJSON = eval('(' + response + ')');
                        var resObj = eval('(' + responseJSON.value + ')');
                        bindDropDownList(resObj, ddlManufacturers, "", "--Select Amanufacturer--");
                    },
                    complete: function (xhr) {
                        if (xhr.status == 404 || xhr.status == 204) {
                            alert("Error Occurred");
                        }
                    }
                });
            }
        }
       
        else {
            ddlModel.val("0").attr("disabled", true);
        }
    }
</script>
