<%@ Page Language="C#" AutoEventWireup="false" Inherits="BikewaleOpr.Campaign.MapCampaign" %>
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
        display: none;
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
    You are here &raquo; Map Dealer Campaigns
</div>
<div>
    <!-- #Include file="/content/DealerMenu.aspx" -->
</div>
<div>    
    <link rel="stylesheet" href="/css/common.css?V1.2" type="text/css" />
    <script type="text/javascript">
        var contractId = "<%= contractId %>";
        var dealerId = "<%= dealerId%>";
        var contractId = "<%= contractId %>";
        var selectedCampaign = "";
        var dealerName = encodeURIComponent("<%= dealerName %>");
    </script>

    <div>
        <fieldset class="margin-left20">
            <legend class="font14"><b>Map Campaign</b></legend>
            <h3 class="margin-left40">Map with existing campaign(s) or a Create a new Campaign</h3>            
            <% if (rptCampaigns.DataSource != null)
               { %>
            <fieldset style="width: 800px; margin-left: 50px;">
                <legend class="font14"><b>Map Campaign for "<%=dealerName %>"</b></legend>
                <asp:repeater runat="server" id="rptCampaigns">
                    <HeaderTemplate>
                        <div>
                        <table class="margin-top10 margin-bottom10" rules="all" cellspacing="0" cellpadding="5" style="border-width: 1px; border-style: solid; width: 100%; border-collapse: collapse;">
                        <thead>
                            <tr class="dtHeader">
                                <th></th>
                                <th>Campaign Id</th>
                                <th>Email Id</th>
                                <th>Campaign Name</th>                                            
                                <th>Masking Number</th>
                                <th>Serving Radius</th>
                            </tr>
                            </tr>
                        </thead>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr id="trCampaignDetails">
                            <td class="rdbCampaignId">
                                <span id="rdb_<%#DataBinder.Eval(Container.DataItem,"CampaignId") %>">
                                    <input type="radio" name="rdbCampaign" runat="server" id="rdbCampaign" value='<%#DataBinder.Eval(Container.DataItem,"CampaignId") %>' /> 
                                </span>
                            </td>
                            <td><%#DataBinder.Eval(Container.DataItem,"CampaignId") %></td>
                            <td><%#DataBinder.Eval(Container.DataItem,"EmailId") %></td>
                            <td><%#DataBinder.Eval(Container.DataItem,"CampaignName") %></td>
                            <td>                                            
                                <span id="addMaskingNumber_<%#DataBinder.Eval(Container.DataItem,"CampaignId") %>"><%#DataBinder.Eval(Container.DataItem,"MaskingNumber").ToString() == "" ? "" : DataBinder.Eval(Container.DataItem,"MaskingNumber") %></span>
                            </td>
                            <td><%#DataBinder.Eval(Container.DataItem,"ServingRadius") %></td>
                                        
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                    </table>
                    </div>
                    </FooterTemplate>
                </asp:repeater>
            </fieldset>
    <br />
    <br />
    <asp:button id="btnMapCampaign" runat="server" text="Map Existing Campaign" cssclass="margin20 bold margin-left40" />
        <%}
               else
               {%>
    <p class="margin-left40">There are no existing campaigns associated with dealer <%=string.IsNullOrEmpty(dealerName)? "":" '"+dealerName +"' " %>.Click on proceed to create new campaign.</p>
    <% } %>
    <br />
    <br />
    <h3 class="margin-left40">Create a new campaign for <%=dealerName %></h3>
    <b id="rdNewCamp" class="margin-left40">        
        <input type="radio" id="rdbNewCamp" name="rdbCampaign" value="0" />Create New Campaign<br />
    </b>
    <br />
    <br />
    <asp:button id="btnProceed" runat="server" text="Create Campaign" cssclass="margin20 bold margin-left40" />
    <br />
    <br />    
</div>
</fieldset>
</div>

<script type="text/javascript" src="http://st2.aeplcdn.com/bikewale/src/common/chosen.jquery.min.js?v15416"></script>
<script type="text/javascript">
    $(document).ready(function () {
        $('[name$="rdbCampaign"]').attr("name", $('[name$="rdbCampaign"]').attr("name"));
        $('[name$="rdbCampaign"]').click(function () {
            $('[name$="rdbCampaign"]').attr("name", $(this).attr("name"));
            $('#rdbNewCamp').attr('checked', false);
        });
    });
    $('#rdbNewCamp').change(
        function () {
            $("input[name$='rdbCampaign']").each(function () {
                $(this).attr('checked', false);
            });
        }
    );
    $("#btnProceed, #btnMapCampaign").click(function () {
        if ($('#rdbNewCamp').is(':checked')) {
            location.href = "/campaign/ManageDealers.aspx?contractid=" + contractId + "&dealerid=" + dealerId + "&dealername=" + dealerName + "&no=" + <%=dealerNumber %> + "";
    }
    else if ($("input[name$='rdbCampaign']").is(":checked")) {
        var campaignId = '';
        $("input[name$='rdbCampaign']").each(function () {
            if ($(this).is(':checked')) {
                campaignId = $(this).val();
                mapCampaign(campaignId);               
            }
        });
    }
    else {
        alert("Please select existing campaign or create a new campaign");
    }
        return false;
    });

    function mapCampaign(campaignId) {

        if(confirm("Do you want to map the selected campaign?")){
            $.ajax({
                type: "POST",
                url: "/ajaxpro/BikeWaleOpr.Common.AjaxCommon,BikewaleOpr.ashx",
                data: '{"contractId":"' + contractId + '" , "campaignId":"' + campaignId + '"}',
                beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "MapCampaign"); },
                success: function (response) {
                    alert('Campaign has been mapped with contract');
                    //location.href = "/campaign/ManageDealers.aspx?contractid=" + contractId + "&campaignid=" + campaignId + "&dealerid=" + dealerId + "&dealername=" + dealerName + "&no=" + <%=dealerNumber %> + "";
                }
            });
        }    
    }
</script>
<!-- #Include file="/includes/footerNew.aspx" -->
