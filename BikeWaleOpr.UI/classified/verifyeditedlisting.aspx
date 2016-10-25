<%@ Page Language="C#" AutoEventWireup="false" Inherits="BikewaleOpr.classified.VerifyEditedListing" %>
<%@ Import Namespace="BikeWaleOpr.Common" %>

<!-- #Include file="/includes/headerNew.aspx" -->
<div>
        <!-- #Include file="classifiedMenu.aspx" -->
</div>
<div class="left min-height600" id="divManagePrices">
    <h1>Verify Edited Listing</h1>

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
                <% foreach(var listing in sellListing) { %>
                <tr class="dtItem" id="detailed_edit_row">
                    <td><%=listing.InquiryId %></td>
                    <td><%=listing.Version.VersionName %></td>
                    <td><%=listing.KiloMeters %></td>
                    <td><%=listing.Expectedprice %></td>
                    <td><%=listing.ManufacturingYear.Year %></td>
                    <td><input id="btnLView" onclick ="<%= string.Format("javascript:window.open('/classified/listingphotos.aspx?profileid={0}','','left=0,top=0,width=1400,height=660,resizable=0,scrollbars=yes')", listing.InquiryId) %>" <%= listing.PhotoCount > 0 ? "" : "style='display:none;'" %> type="button" value ="View Photos"  /></td>
                    <td>
                        <input data-attr-id="<%=listing.InquiryId %>" data-attr-profileid="<%=listing.ProfileId %>" data-attr-bikename="<%=listing.Version.VersionName %>" id="btnApprove" type="button" value="Approve" /><input data-attr-id="<%=listing.InquiryId %>" data-attr-profileid="<%=listing.ProfileId %>" data-attr-bikename="<%=listing.Version.VersionName %>" id="btnDiscard" type="button" value="Discard" />
                    </td>
                </tr>
                   <% } %>
            </tbody>
        </table>
    </div>
</div>
<script type="text/javascript">
var userid = '<%= CurrentUser.Id %>';
 var BwOprHostUrl = '<%= ConfigurationManager.AppSettings["BwOprHostUrlForJs"]%>';
    $('td #btnApprove').click(function () {
      acceptReject($(this), 1);
      $('#detailed_edit_row').html('<td colspan=7 class="greenMsg">This listing has been approved</td>').animate({ left: '250px' });
  });
    $('td #btnDiscard').click(function () {
        //debugger;
      acceptReject($(this), 0);
      $('#detailed_edit_row').html('<td colspan=7 class="redMsg">This listing has been discarded </td>').animate({ left: '250px' });
  });

  function acceptReject(btn, status) {
      var selInquiry = (btn).attr('data-attr-id');
      var profileId = (btn).attr('data-attr-profileid');
      var bikename = (btn).attr('data-attr-bikename');
      var uri = BwOprHostUrl + "/api/used/sell/pendinginquiries/" + selInquiry + "/?isApproved=" + status + "&approvedBy=" + userid + "&profileId=" + profileId + "&bikeName=" + bikename;
      console.log(uri);
        $.ajax({
            type: "POST",
            url: uri,
            success: function (response) {
            },
            complete: function (xhr) {
                if (xhr.status != 200) {
                    alert("Something went wrong .Please try again !!");
                }
            }
        });
}
</script>
<!-- #Include file="/includes/footerNew.aspx" -->