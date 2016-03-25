<%@ Page Language="C#" AutoEventWireup="false" Inherits="BikewaleOpr.Campaign.MapCampaign" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.7.2/jquery.min.js"></script>
    <link rel="stylesheet" href="/css/common.css?V1.2" type="text/css" />
    <script type="text/javascript">
        var contractId = "<%= contractId %>";
        var dealerId = "<%= dealerId%>";
        var contractId = "<%= contractId %>";
        var selectedCampaign = "";
        var dealerName = encodeURIComponent("<%= dealerName %>");
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <fieldset class="margin-left20">
              <h3 class="margin-left40">Map with existing campaign(s) or a Create a new Campaign</h3>
                <legend class="font14"><b>Map Campaign</b></legend>
                <div>
                    <fieldset style="width: 800px; margin-left: 250px;">
                        <legend class="font14"><b>Map Campaign for "<%=dealerName %>"</b></legend>
                        <asp:Panel ID="pnlExisting" runat="server">
<%--                            <b>
                               <asp:RadioButton runat="server" GroupName="ExistingOrNew" ID="rdbExistingCamp" Text="Map With Existing Campaign" Checked="true" /><%=string.IsNullOrEmpty(dealerName)?"":" for Dealer '"+dealerName +"'" %>
                                </b>--%>
                            <asp:Repeater runat="server" ID="rptCampaigns">
                                <HeaderTemplate>
                                    <table cellpadding="5" class="lstcamptable">
                                        <tr>
                                            <th></th>
                                            <th>CampaignId</th>
                                            <th width="150">Email Id</th>
                                            <th>Dealer Name</th>
                                            <th>Status</th>
                                            <th>Masking Number</th>
                                        </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr id="trCampaignDetails">
                                        <td class="rdbCampaignId">
                                            <span id="rdb_<%#Eval("CampaignId") %>">
                                                <input type="radio" name="rdbCampaign" runat="server" id="rdbCampaign" value='<%#Eval("CampaignId") %>' checked='<%#Eval("IsMapped")%>' /> 
                                            </span>
                                        </td>
                                        <td><%#Eval("CampaignId") %></td>
                                        <td width="150"><%#Eval("DealerEmailId") %></td>
                                        <td><%#Eval("DealerName") %></td>
                                        <td><%#(bool)Eval("IsActive")== true? "Active" : "Inactive" %></td>
                                        <td>
                                            <a target="_blank" onclick="mapCampaign.showMapMaskingNumberPopup(<%#Eval("CampaignId") %>)" id="addMaskingNumberLink_<%#Eval("CampaignId") %>">Add Masking Number</a>
                                            <span id="addMaskingNumber_<%#Eval("CampaignId") %>"><%#Eval("Number").ToString() == "" ? "" : Eval("Number") %></span>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                        </asp:Panel>
                    </fieldset>
                    <br />
                    <p class="margin-left40"><strong>Create a new campaign for <%=dealerName %></strong></p>
                    <b id="rdNewCamp" class="margin-left40">
                        <asp:RadioButton runat="server" GroupName="ExistingOrNew" ID="rdbNewCamp" Text="Create New Campaign" />
                    </b>
                    <br />
                    <br />
<%--                    <asp:Panel Visible="false" ID="pnlNew" runat="server" class="pnlnew margin-left40" >
                        <p><b>There are no existing campaigns associated with dealer <%=string.IsNullOrEmpty(dealerName)? "":" '"+dealerName +"' " %>.Click on proceed to create new campaign.</b></p>
                    </asp:Panel>--%>

                    <asp:Button ID="btnProceed" runat="server"  Text="Proceed" CssClass="margin20 bold margin-left40" />
                    <br /><br />
                    <!-- OnClientClick="javascript:if(!mapCampaign.btnProceedClick()){return false;};" -->
                </div>
            </fieldset>
        </div>
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
            $("#btnProceed").click(function () {
                if ($('#rdbNewCamp').is(':checked')) {
                    location.href = "/campaign/ManageDealers.aspx?contractid=" + contractId + "&dealerid=" + dealerId + "&dealername=" + dealerName;
                }
                else if ($("input[name$='rdbCampaign']").is(":checked")) {
                    var campaignId = '';
                    $("input[name$='rdbCampaign']").each(function () {
                        if ($(this).is(':checked')) {
                            campaignId = $(this).val();
                            mapCampaign(campaignId);
                            location.href = "/campaign/ManageDealers.aspx?contractid=" + contractId + "&campaignid="+ campaignId +"&dealerid=" + dealerId + "&dealername=" + dealerName;
                        }
                    });
                }
                else {
                    alert("Please select existing campaign or create a new campaign");
                }
                return false;
            });

            function mapCampaign(campaignId) {
                $.ajax({
                    type: "POST",
                    url: "/ajaxpro/BikeWaleOpr.Common.AjaxCommon,BikewaleOpr.ashx",
                    data: '{"contractId":"' + contractId + '" , "campaignId":"' + campaignId + '"}',
                    beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "MapCampaign"); },
                    success: function (response) {
                        alert('Campaign has been mapped with contract')
                    }
                });
            }
        </script>
</form>
</body>
</html>
