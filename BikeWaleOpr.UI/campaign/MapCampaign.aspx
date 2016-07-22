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
    You are here &raquo; Manage Dealer Campaigns
</div>
<div>
    <!-- #Include file="/content/DealerMenu.aspx" -->
</div>
    <link rel="stylesheet" href="/css/common.css?V1.2" type="text/css" />
    <script type="text/javascript">
        var contractId = "<%= contractId %>";
        var dealerId = "<%= dealerId%>";
        var selectedCampaign = "";
        var dealerName = encodeURIComponent("<%= dealerName %>");
        var dealerNumber = "<%= dealerNumber %>";
        var maskingNumber = '';
        var userId = '<%= CurrentUser.Id %>';
        var oldMaskingNumber = '<%= oldMaskingNumber %>';
    </script>

    <div>
        <fieldset class="margin-left20">
            <legend><h3>Manage Campaign for "<%=dealerName %>"</h3></legend>
            <% if(contractId > 0){ %>
            <h3 class="margin-left40">Create a new campaign</h3>
            <b id="rdNewCamp" class="margin-left40">
                <input type="radio" id="rdbNewCamp" name="rdbCampaign" value="0" /><label for="rdbNewCamp">Create New Campaign</label><br />
            </b>
            <br />
            <asp:button id="btnProceed" runat="server" text="Create Campaign" cssclass="margin-left40 padding10" />            
            <br />
            <br />
            <hr />
            <% } %>
            <% if (campaignEntity != null) { %>
            <h3 class="margin-left40">Currently Mapped Campaign</h3>
            <div class="margin-left40">
                    <table class="margin-top10 margin-bottom10" rules="all" cellspacing="0" cellpadding="5" style="border-width: 1px; border-style: solid; width: 100%; border-collapse: collapse;">
                                          
                        <thead>
                            <tr class="dtHeader">                                
                                <th>Campaign Id</th>
                                <th>Campaign Email Id</th>
                                <th>Campaign Name</th>                                            
                                <th>Masking Number</th>
                                <th>Serving Radius</th>
                                <th>Edit</th>
                            </tr>
                            </tr>
                        </thead>
                        <tr id="trCampaignDetails">
                            <td><%= campaignEntity.CampaignId %></td>
                            <td><%= campaignEntity.EmailId %></td>
                            <td><%= campaignEntity.CampaignName %></td>
                            <td>                                            
                                <span id="addMaskingNumber_<%= campaignEntity.CampaignId %>"><%= campaignEntity.MaskingNumber == "" ? "" : campaignEntity.MaskingNumber %></span>
                            </td>
                            <td><%= campaignEntity.ServingRadius %></td>
                            <td><a target="_blank" href="/campaign/ManageDealers.aspx?contractid=<%= contractId %>&campaignid=<%= campaignEntity.CampaignId %>&dealerid=<%= dealerId %>&dealername=<%= dealerName %>&no=<%=dealerNumber %>">Edit</a></td>            
                        </tr>
                    </table>
                </div>
            <br />
            <hr />
            <% } %>
            <% if (campaigns != null && campaigns.DealerCampaigns != null && campaigns.DealerCampaigns.Count() > 0) { %>
            <h3 class="margin-left40"><% if(contractId > 0){ %> Map with <% } %>Other Campaign(s)</h3>
            <% if (rptCampaigns.DataSource != null)
               { %>            
                <div class="margin-left40">
                    <table class="margin-top10 margin-bottom10" rules="all" cellspacing="0" cellpadding="5" style="border-width: 1px; border-style: solid; width: 100%; border-collapse: collapse;">
                        <asp:repeater runat="server" id="rptCampaigns">
                    <HeaderTemplate>                        
                        <thead>
                            <tr class="dtHeader">
                                <% if(contractId > 0){ %><th></th><%} %>
                                <th>Campaign Id</th>
                                <th>Campaign Email Id</th>
                                <th>Campaign Name</th>                                            
                                <th>Masking Number</th>
                                <th>Serving Radius</th>
                                <th>Edit</th>
                            </tr>
                            </tr>
                        </thead>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr id="trCampaignDetails">
                            <% if(contractId > 0){ %>
                             <td class="rdbCampaignId">
                                <span id="rdb_<%#DataBinder.Eval(Container.DataItem,"CampaignId") %>">
                                    <input type="radio" name="rdbCampaign" runat="server" id="rdbCampaign" value='<%#DataBinder.Eval(Container.DataItem,"CampaignId") %>' /> 
                                </span>
                            </td>
                            <%} %>
                            <td><%#DataBinder.Eval(Container.DataItem,"CampaignId") %></td>
                            <td><%#DataBinder.Eval(Container.DataItem,"EmailId") %></td>
                            <td><%#DataBinder.Eval(Container.DataItem,"CampaignName") %></td>
                            <td>                                            
                                <span id="addMaskingNumber_<%#DataBinder.Eval(Container.DataItem,"CampaignId") %>"><%#DataBinder.Eval(Container.DataItem,"MaskingNumber").ToString() == "" ? "" : DataBinder.Eval(Container.DataItem,"MaskingNumber") %></span>
                            </td>
                            <td><%#DataBinder.Eval(Container.DataItem,"ServingRadius") %></td>
                            <td><a target="_blank" href="/campaign/ManageDealers.aspx?contractid=<%= contractId %>&campaignid=<%#DataBinder.Eval(Container.DataItem,"CampaignId") %>&dealerid=<%= dealerId %>&dealername=<%= dealerName %>&no=<%=dealerNumber %>">Edit</a></td>            
                        </tr>
                    </ItemTemplate>                    
                </asp:repeater>
                    </table>
                </div>
            
            <% if(contractId > 0){ %>
            <asp:button id="btnMapCampaign" runat="server" text="Map Existing Campaign" cssclass="margin-left40 padding10" />
            <%} %>
            <%}
               else
               {%>
            <p class="margin-left40">There are no existing campaigns associated with dealer <%=string.IsNullOrEmpty(dealerName)? "":" '"+dealerName +"' " %>.Click on proceed to create new campaign.</p>
            <% } %>
            <br />
            <br />            
            <% } %>
        </fieldset>
    </div>

    <script type="text/javascript">

        var _cwWebService = "<%= ConfigurationManager.AppSettings["CwWebServiceHostUrl"] %>" ;

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
        campaignId = $("input[name$='rdbCampaign']:checked").val();
        maskingNumber = $("#addMaskingNumber_" + campaignId).text();
        mapCampaign(campaignId, maskingNumber);
    }
    else {
        alert("Please select existing campaign or create a new campaign");
    }
        return false;
    });

        function mapCampaign(campaignId, maskingNumber) {
            try {
                if (confirm("Do you want to map the selected campaign?")) {
                    //need to verify
                    var objdata = {
                        "ConsumerId" : dealerId,
                        "LeadCampaignId" : campaignId,
                        "LastUpdatedBy" : 1,
                        "ProductTypeId" :3,
                        "DealerType" : 2,
                        "NCDBranchId" : -1,
                        "OldMaskingNumber" : maskingNumber,
                        "MaskingNumber" : maskingNumber,
                        "SellerMobileMaskingId": -1,
                        "Mobile": dealerNumber
                        };

                    $.ajax({
                        type: "POST",
                        url: "/ajaxpro/BikeWaleOpr.Common.AjaxCommon,BikewaleOpr.ashx",
                        //data: '{"contractId":"' + contractId + '" , "campaignId":"' + campaignId + '"}',
                        data: '{"contractId":"' + contractId + '", "dealerId":"' + dealerId + '", "campaignId":"' + campaignId + '", "userId":"' + userId + '", "oldMaskingNumber":"' + oldMaskingNumber + '", "maskingNumber":"' + maskingNumber + '", "dealerMobile":"' + dealerNumber + '"}',
                        beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "MapCampaign"); },
                        success: function (response) {
                            if (JSON.parse(response).value) {
                                alert('Campaign has been mapped with contract. Now please wait for data sync with Carwale');
                                //mapCWCampaignContract(campaignId);
                            }
                            else {
                                alert("There was error occured during mapping. Please contact System Administrator for more details.");
                            }
                        }

                    });
                }
            } catch (e) {
                alert("An error occured. Please contact System Administrator for more details.");
            }
        }

        function mapCWCampaignContract(campaignId) {
            try {
                    $.ajax({
                        type: "POST",
                        url: _cwWebService + "/api/contracts/mapcampaign/?dealerid=" + dealerId + "&contractid=" + contractId + "&campaignid=" + campaignId + "&applicationid=2",
                        success: function (response) {
                            if (JSON.parse(response).value){
                                alert('Contract Campaign Data Syned with CW');
                            }
                            else {
                                alert("There was error occured during mapping. Please contact System Administrator for more details.");
                            }
                        }

                    });
            } catch (e) {
                alert("An error occured. Please contact System Administrator for more details.");
            }

    }
    </script>
    <!-- #Include file="/includes/footerNew.aspx" -->
