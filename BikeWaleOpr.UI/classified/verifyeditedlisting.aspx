<%@ Page Language="C#" AutoEventWireup="false" Inherits="BikewaleOpr.classified.VerifyEditedListing" %>
<%@ Import Namespace="BikeWaleOpr.Common" %>

<!-- #Include file="/includes/headerNew.aspx" -->
<div>
        <!-- #Include file="classifiedMenu.aspx" -->
</div>
<div class="left min-height600" id="divManagePrices">
    <h1>Manage Showroom Prices</h1>

    <div class="margin-top10 floatLeft" style="width: 850px; display: inline-block;">
        <table class="table-bordered" cellspacing="0" cellpadding="5">
            <tbody>
                <tr class="dtHeader">
                    <th style="font-size: 13px">Profile Id</th>
                    <th style="font-size: 13px">Version</th>
                    <th style="font-size: 13px">Kms ridden</th>
                    <th style="font-size: 13px">Price</th>
                    <th style="font-size: 13px">Manufacturing Year</th>
                    <th style="font-size: 13px;">Photos</th>
                    <th colspan="2" style="font-size: 13px">Listing Status</th>
                </tr>
                <tr class="dtItem" id="detailed_edit_row">
                    <td>1</td>
                    <td>sss</td>
                    <td>105644</td>
                    <td>19545</td>
                    <td>sss</td>
                    <td>sss</td>
                    <td>
                        <input data-attr-id="22" id="btnApprove" type="button" value="Approve" /><input id="btnDiscard" type="button" value="Discard" /></td>
                </tr>
                <tr class="dtItem">
                    <td>1</td>
                    <td>sss</td>
                    <td>105644</td>
                    <td>19545</td>
                    <td>sss</td>
                    <td>sss</td>
                    <td>
                        <input data-attr-id="33" id="btnApprove" type="button" value="Approve" /><input id="btnDiscard" type="button" value="Discard" /></td>
                 </tr>
                <%--<tr class="<%= ((listType == (int)ListingType.TotalListings) )? "" :"hide"  %>"><td id="tdLiveListing" colspan="19" class="tdStyle"><b>Live Listings</b></td></tr>--%>
                <asp:repeater id="rptPendingEditedListing" runat="server">
                <ItemTemplate>
                    <tr class="dtItem">
                        <td><%# DataBinder.Eval(Container.DataItem,"ProfileId")%></td>
                        <td><%# DataBinder.Eval(Container.DataItem,"version")%></td>
                        <td><%# CommonOpn.FormatNumeric(DataBinder.Eval(Container.DataItem,"Kilometers").ToString())%></td>
                        <td><%# CommonOpn.FormatNumeric(DataBinder.Eval(Container.DataItem,"Price").ToString())%></td>
                        <td><%# DataBinder.Eval(Container.DataItem,"MakeYear","{0:MMM-yyyy}")%></td>
                        <td><input id="btnLView" onclick ="<%# string.Format("javascript:window.open('/classified/listingphotos.aspx?profileid={0}','','left=0,top=0,width=1400,height=660,resizable=0,scrollbars=yes')",DataBinder.Eval(Container.DataItem,"InquiryId").ToString()) %>"  <%# (Convert.ToInt32(DataBinder.Eval(Container.DataItem, "PhotoCount")) > 0) ? "" : "style='display:none;'" %> type="button" value ="View Photos"  /></td>
                        <td data-attr-id="5" ><input data-attr-id="" class="discardList" id="btnLDiscard" type="button" value ="Discard" bikeName="<%# DataBinder.Eval( Container.DataItem, "BikeName" ) %>" profileId="<%# DataBinder.Eval( Container.DataItem, "ProfileId" ) %>" inquiryId="<%# DataBinder.Eval( Container.DataItem, "InquiryId")%>"/></td>
                    </tr>
                </ItemTemplate>
            </asp:repeater>
            </tbody>
        </table>
    </div>
</div>
<script type="text/javascript">
  $('td #btnApprove').click(function () {
      var selInquiry = $(this).attr('data-attr-id');
  });
  $('td #btnDiscard').click(function () {
      var selInquiry = $(this).attr('data-attr-id');
  });
</script>
<!-- #Include file="/includes/footerNew.aspx" -->