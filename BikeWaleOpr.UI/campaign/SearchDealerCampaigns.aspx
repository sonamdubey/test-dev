<%@ Page Language="C#" AutoEventWireup="false" Inherits="BikewaleOpr.campaign.SearchDealerCampaigns" Async="true" AsyncTimeout="60" %>

<!-- #Include file="/includes/headerNew.aspx" -->
<script src="/src/jquery-1.6.min.js" type="text/javascript"></script>
<script src="/src/AjaxFunctions.js" type="text/javascript"></script>
<script type="text/ecmascript" src="/src/AjaxFunctions.js"></script>
<script src="/src/knockout.js" type="text/javascript"></script>
<link rel="stylesheet" href="/css/common.css?V1.2" type="text/css" />
<link href="http://st2.aeplcdn.com/bikewale/css/chosen.min.css?v15416" rel="stylesheet" />
<style type="text/css">
    .greenMessage {
        color: #6B8E23;
        font-size: 11px;
    }

    .redmsg {
        color: #FFCECE;
    }

    .errMessage {
        color: #FF4A4A;
    }

    .valign {
        vertical-align: top;
    }
    .progress-bar {
    width: 0;
    display:none;
    height: 2px;
    background: #16A085;
    bottom: 0px;
    left: 0;
    border-radius: 2px;
}

    .position-abt {
    position: absolute;
}

    .position-rel {
    position: relative;
}
</style>
<div>
    You are here &raquo; Search Dealer Campaigns
</div>
<div>
    <!-- #Include file="/content/DealerMenu.aspx" -->
</div>
<div>
    <div id="inputSection" class="position-rel" style="margin-left:200px">
    <div style="border: 1px solid #777;" class="padding10">
        <span>Dealer's City : <font color="red">* &nbsp</font>
            <asp:dropdownlist id="drpCity" enabled="True" cssclass="drpClass" runat="server">
					<asp:ListItem Text="--Select City--" Value="-1"/>
					</asp:dropdownlist>
            <span style="font-weight: bold; color: red;" id="spndrpCity" class="error" />&nbsp&nbsp
        </span>
        <span>Dealer Name : <font color="red">* &nbsp</font>
            <asp:dropdownlist id="drpDealer" enabled="True" cssclass="drpClass" runat="server">
				<asp:ListItem Text="--Select Dealer--" Value="-1" />
				</asp:dropdownlist>
        </span>
        <span>
            <input id="getCampaigns" type="button" class="padding10" value="Get Campaigns" />
        </span>
    </div>
    <span class="position-abt progress-bar" style="width: 100%;overflow: hidden;/* display: none; */"></span>
    </div>
    <div style="margin-left: 200px;display:none;overflow-x:auto;overflow-y:hidden" id="DealerCampaignsList">
        <div class="padding10" >
            <h4 id="selDealerHeading"></h4>
        </div>
        <table class="margin-top10" rules="all" cellspacing="0" cellpadding="5" style="border-width: 1px; border-style: solid; width: 100%; border-collapse: collapse;">
            <thead>
                <tr class="dtHeader">
                    <td>Campaign Name</td>
                    <td>Campaign EmailId</td>
                    <td>Campaign LeadServingRadius</td>
                    <td>IsActive Campaign</td>
                    <td>Masking Number</td>
                    <td>Mobile No</td>
                    <td>Mapped Masking No</td>
                    <td>Mapped Mobile No</td>
                    <td>Organization</td>
                    <td>Contract StartDate</td>
                    <td>Contract EndDate</td>
                    <td>Contract Status</td>
                    <td>NoOfRules</td>
                </tr>
            </thead>
            <tbody data-bind="template: { name: 'DealerList', foreach: Table }">
            </tbody>
        </table>
      </div>
        <script type="text/html" id="DealerList">
            <tr class="dtItem">
                <td data-bind="text : CampaignName"></td>
                <td data-bind="text : CampaignEmailId"> </td>
                <td data-bind="text : CampaignLeadServingRadius"></td>
                <td data-bind="text: (IsActiveCampaign)?'Yes':'No'"></td>
                <td data-bind="text : MaskingNumber"></td>
                <td data-bind="text : MobileNo"></td>
                <td data-bind="text : MappedMaskingNo"></td>
                <td data-bind="text : MappedMobileNo"></td>
                <td data-bind="text : Organization"></td>
                <td data-bind="text : ContractStartDate" ></td>
                <td data-bind="text : ContractEndDate"></td>
                <td data-bind="text: (ContractStatus)?'Active':'Inactive'"></td>  
                <td data-bind="text: NoOfRules"></td>
            </tr>
        </script>
    
</div>
<script type="text/javascript" src="http://st2.aeplcdn.com/bikewale/src/common/chosen.jquery.min.js?v15416"></script>
<script>
    var ABApiHostUrl = '<%= cwHostUrl%>';
    var ddlDealer = $("#drpDealer");
    var selectString = "--Select Dealer--";
    var onInitCity = $("#drpCity option:selected").val();
    if (onInitCity > 0) {
        $.ajax({
            type: "GET",
            url: ABApiHostUrl + "/api/Dealers/GetAllDealers/?cityId=" + onInitCity,
            success: function (response) {
                ddlDealer.empty().append("<option value=\"0\">" + selectString + "</option>").removeAttr("disabled");
                for (var i = 0; i < response.length; i++) {
                    ddlDealer.append("<option value=" + response[i].Value + " makeId=" + response[i].MakeId + ">" + response[i].Text + "</option>");
                }
            }
        });
    }

    $("#drpCity").change(function () {
        var cityId = $(this).val();
        $("#hdnCityId").val(cityId);  
        if (cityId > 0) {
            $.ajax({
                type: "GET",
                url: ABApiHostUrl + "/api/Dealers/GetAllDealers/?cityId=" + cityId,
                success: function (response) {
                    ddlDealer.empty().append("<option value=\"0\">" + selectString + "</option>").removeAttr("disabled");
                    for (var i = 0; i < response.length; i++) {
                        ddlDealer.append("<option value=" + response[i].Value + " makeId=" + response[i].MakeId + ">" + response[i].Text + "</option>");
                    }
                    ddlDealer.trigger('chosen:updated');
                }
            });
        }
        else {
            ddlDealer.empty().append("<option value=\"0\">" + selectString + "</option>").removeAttr("enabled");
            ddlDealer.trigger('chosen:updated');
        }
    });

    var DealerViewModel = function (model) {
        ko.mapping.fromJS(model, {}, this);
    };

    $("#getCampaigns").click(function () {
        cId = $("#drpCity option:selected");
        dId = $("#drpDealer option:selected"); 
        cityId = cId.val();
        dealerId = dId.val();
       dealerHeading = cId.text() + " : " + dId.text() + "  Campaigns ";
        if (!isNaN(cityId) && cityId != "0") {
            if (!isNaN(dealerId) && dealerId != "0") {
                var element = document.getElementById('DealerCampaignsList');
                $.ajax({
                    type: "POST",
                    url: "/ajaxpro/BikeWaleOpr.Common.AjaxCommon,BikewaleOpr.ashx",
                    data: '{"dealerId":"' + dealerId + '"}',
                    beforeSend: function (xhr) {
                        startLoading($("#inputSection"));
                        xhr.setRequestHeader("X-AjaxPro-Method", "GetDealerCampaigns");
                    },
                    datatype: "json",
                    success: function (response) {
                        ko.cleanNode(element);                         
                        var responseJSON = eval('(' + response + ')');
                        if (responseJSON.value != "")
                        {
                            response = eval('(' + responseJSON.value + ')');
                            if (response != null && response.Table != null) {
                                ko.applyBindings(new DealerViewModel(response), element);
                                $('#DealerCampaignsList').show();
                            }
                            else {
                                $('#DealerCampaignsList').hide();
                                alert(dealerHeading + "not available");
                            }
                        }
                        else {
                            $('#DealerCampaignsList').hide();
                            alert(dealerHeading + "not available");
                        }
                       
                    },
                    complete: function (xhr) {
                        if(xhr.status != 200)
                        {
                            alert("Something went wrong .Please try again !!")
                        }
                        stopLoading($("#inputSection"));
                    }
                });
            } else {
                alert("Please select Dealer");
            }

        } else {
            alert("Please select City ");
        }
    });

    $("#drpCity").chosen({ width: "200px", no_results_text: "No matches found!!", search_contains: true });
    $("#drpDealer").chosen({ width: "200px", no_results_text: "No matches found!!", search_contains: true });    function startLoading(ele) {
        try {
            var _self = $(ele).find(".progress-bar").css({ 'width': '0' }).show();
            _self.animate({ width: '100%' }, 5000);
        }
        catch (e) { return };
    }

    function stopLoading(ele) {
        try {
            var _self = $(ele).find(".progress-bar");
            _self.stop(true, true).css({ 'width': '100%' }).fadeOut(1000);
        }
        catch (e) { return };
    }

</script>
<!-- #Include file="/includes/footerNew.aspx" -->
