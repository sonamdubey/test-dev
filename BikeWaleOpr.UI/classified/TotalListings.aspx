<%@ Page Language="C#" AutoEventWireup="false" Inherits="BikeWaleOpr.Classified.TotalListings" Trace="false" %>
<%@ Import Namespace="BikeWaleOpr.Common" %>
<%@ Import Namespace="BikeWaleOpr.Classified" %>
<%--<title>Total Listings By Customer</title>--%>
<link href="/css/Common.css" rel="stylesheet" />
<script src="/src/jquery-1.6.min.js"></script>
<style>
    .tdStyle { background-color:#d4d4d4; text-align:left;height:30px;}
    #totalListing th {color:black;}
</style>
<div style="padding:5px;">
    <table id=totalListing border="1" class="tableViw"  style="border-width:1px;text-align:center;border-style:solid;border-collapse:collapse;border-spacing:0;">
        <tbody>
            <tr class="dtHeader">
                <th style="font-size:13px">Profile Id</th>
                <th style="font-size:13px">Customer Name</th>
                <th style="font-size:13px">Customer Mobile</th>
                <th style="font-size:13px">Bike Name</th>
                <th style="font-size:13px">Price</th>
                <th style="font-size:13px">City</th>
                <th style="font-size:13px">State</th>   
                <th style="font-size:13px">Make Year</th>
                <th style="font-size:13px">Kilometers</th>
                <th style="font-size:13px">Color</th>
                <th style="font-size:13px">Owner</th>
                <th style="font-size:13px">Insurance Type</th>
                <th style="font-size:13px">Insurance Expiry Date</th>
                <th style="font-size:13px">Lifetime Tax</th>
                <th style="font-size:13px;">Entry Date</th>
                <th style="font-size:13px;">Photos</th>
                <th colspan ="2" style="font-size:13px">Listing Status</th>
            </tr>
             <tr class="<%= ((listType == (int)ListingType.TotalListings) )? "" :"hide"  %>"><td id="tdLiveListing" colspan="19" class="tdStyle"><b>Live Listings</b></td></tr>
            <asp:Repeater id="rptCustomerLiveList" runat="server">
                <ItemTemplate>
                    <tr class="dtItem">
                        <td id ="Lprofile_id"><%# DataBinder.Eval(Container.DataItem,"ProfileId")%></td>
                        <td><%# DataBinder.Eval(Container.DataItem,"CustomerName")%></td>
                        <td><%# DataBinder.Eval(Container.DataItem,"CustomerMobile")%></td>
                        <td><%# DataBinder.Eval(Container.DataItem,"BikeName")%></td>
                        <td><%# CommonOpn.FormatNumeric(DataBinder.Eval(Container.DataItem,"Price").ToString())%></td>
                        <td><%# DataBinder.Eval(Container.DataItem,"City")%></td>
                        <td><%# DataBinder.Eval(Container.DataItem,"State")%></td>
                        <td><%# DataBinder.Eval(Container.DataItem,"MakeYear","{0:MMM-yyyy}")%></td>
                        <td><%# CommonOpn.FormatNumeric(DataBinder.Eval(Container.DataItem,"Kilometers").ToString())%></td>
                        <td><%# DataBinder.Eval(Container.DataItem,"Color")%></td>
                        <td><%# Convert.ToInt32(DataBinder.Eval(Container.DataItem,"Owner")) > 4 ? "> 4" : DataBinder.Eval(Container.DataItem,"Owner")%> </td>
                        <td><%# DataBinder.Eval(Container.DataItem,"InsuranceType")%></td>
                        <td><%# DataBinder.Eval(Container.DataItem,"InsuranceExpiryDate","{0:MMM-yyyy}")%></td>
                        <td><%# DataBinder.Eval(Container.DataItem,"LifetimeTax")%></td>
                        <td><%# DataBinder.Eval(Container.DataItem,"EntryDate","{0:dd-MMM-yyyy}")%></td>
                        <td><input id="btnLView" onclick ="<%# string.Format("javascript:window.open('/classified/listingphotos.aspx?profileid={0},left=0,top=0,width=1400,height=660,resizable=0,scrollbars=yes')",DataBinder.Eval(Container.DataItem,"ProfileId").ToString()) %>"  <%# (Convert.ToInt32(DataBinder.Eval(Container.DataItem, "PhotoCount")) > 0) ? "" : "style='display:none;'" %> type="button" value ="View Photos"  /></td>
                        <td><input class="discardList" id="btnLDiscard" type="button" value ="Discard" profileId="<%# DataBinder.Eval( Container.DataItem, "ProfileId" ) %>" /></td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
            <tr class="<%= ((listType == (int)ListingType.TotalListings) )? "" :"hide"  %>"><td id="tdPendingListing" colspan="19" class="tdStyle"><b>Pending Listings</b></td></tr>
            <asp:Repeater id="rptCustomerPendingList" runat="server">
                <ItemTemplate>
                    <tr class="dtItem">
                        <td id ="Pprofile_id"><%# DataBinder.Eval(Container.DataItem,"ProfileId")%></td>
                        <td><%# DataBinder.Eval(Container.DataItem,"CustomerName")%></td>
                        <td><%# DataBinder.Eval(Container.DataItem,"CustomerMobile")%></td>
                        <td><%# DataBinder.Eval(Container.DataItem,"BikeName")%></td>
                        <td><%# CommonOpn.FormatNumeric(DataBinder.Eval(Container.DataItem,"Price").ToString())%></td>
                        <td><%# DataBinder.Eval(Container.DataItem,"City")%></td>
                        <td><%# DataBinder.Eval(Container.DataItem,"State")%></td>
                        <td><%# DataBinder.Eval(Container.DataItem,"MakeYear","{0:MMM-yyyy}")%></td>
                        <td><%# CommonOpn.FormatNumeric(DataBinder.Eval(Container.DataItem,"Kilometers").ToString())%></td>
                        <td><%# DataBinder.Eval(Container.DataItem,"Color")%></td>
                        <td><%# Convert.ToInt32(DataBinder.Eval(Container.DataItem,"Owner")) > 4 ? "> 4" : DataBinder.Eval(Container.DataItem,"Owner")%> </td>
                        <td><%# DataBinder.Eval(Container.DataItem,"InsuranceType")%></td>
                        <td><%# DataBinder.Eval(Container.DataItem,"InsuranceExpiryDate","{0:MMM-yyyy}")%></td>
                        <td><%# DataBinder.Eval(Container.DataItem,"LifetimeTax")%></td>
                        <td><%# DataBinder.Eval(Container.DataItem,"EntryDate","{0:dd-MMM-yyyy}")%></td>
                        <td><input id="btnPView" onclick ="javascript:window.open('/classified/listingphotos.aspx?profileid=<%# DataBinder.Eval(Container.DataItem,"ProfileId")%>    ','','left=0,top=0,width=1400,height=660,resizable=0,scrollbars=yes')" type="button" value ="View Photos" <%# (Convert.ToInt32(DataBinder.Eval(Container.DataItem, "PhotoCount")) > 0) ? "" : "style='display:none;'" %> /></td>
                        <td><input class="approveList" id="btnPApprove" type="button" value ="Approve" profileId="<%# DataBinder.Eval( Container.DataItem, "ProfileId" ) %>" />
                       <input class="discardList" id="btnPDiscard" type="button" value ="Discard" profileId="<%# DataBinder.Eval( Container.DataItem, "ProfileId" ) %>" /></td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
             <tr class="<%= ((listType == (int)ListingType.TotalListings)) ? "" :"hide"  %>"><td id="tdFakeListing" colspan="19" class="tdStyle"><b>Fake Listings</b></td></tr>
              <asp:Repeater id="rptCustomerFakeList" runat="server">
                <ItemTemplate>
                    <tr class="dtItem">
                        <td id ="Fprofile_id"><%# DataBinder.Eval(Container.DataItem,"ProfileId")%></td>
                        <td><%# DataBinder.Eval(Container.DataItem,"CustomerName")%></td>
                        <td><%# DataBinder.Eval(Container.DataItem,"CustomerMobile")%></td>
                        <td><%# DataBinder.Eval(Container.DataItem,"BikeName")%></td>
                        <td><%# CommonOpn.FormatNumeric(DataBinder.Eval(Container.DataItem,"Price").ToString())%></td>
                        <td><%# DataBinder.Eval(Container.DataItem,"City")%></td>
                        <td><%# DataBinder.Eval(Container.DataItem,"State")%></td>
                        <td><%# DataBinder.Eval(Container.DataItem,"MakeYear","{0:MMM-yyyy}")%></td>
                        <td><%# CommonOpn.FormatNumeric(DataBinder.Eval(Container.DataItem,"Kilometers").ToString())%></td>
                        <td><%# DataBinder.Eval(Container.DataItem,"Color")%></td>
                        <td><%# DataBinder.Eval(Container.DataItem,"Owner")%></td>
                        <td><%# DataBinder.Eval(Container.DataItem,"InsuranceType")%></td>
                        <td><%# DataBinder.Eval(Container.DataItem,"InsuranceExpiryDate","{0:MMM-yyyy}")%></td>
                        <td><%# DataBinder.Eval(Container.DataItem,"LifetimeTax")%></td>
                        <td><%# DataBinder.Eval(Container.DataItem,"EntryDate","{0:dd-MMM-yyyy}")%></td>
                        <td><input id="btnFView" type="button" onclick ="javascript:window.open('/classified/listingphotos.aspx?profileid=<%# DataBinder.Eval(Container.DataItem,"ProfileId")%>    ','','left=0,top=0,width=1350,height=660,resizable=0,scrollbars=yes')" value ="View Photos" <%# (Convert.ToInt32(DataBinder.Eval(Container.DataItem, "PhotoCount")) > 0) ? "" : "style='display:none;'" %> /></td>
                        <td><input class="approveList" id="btnFApprove" type="button" value ="Approve" profileId="<%# DataBinder.Eval( Container.DataItem, "ProfileId" ) %>" /></td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
            <tr class="<%= ((listType == (int)ListingType.TotalListings)) ? "" :"hide"  %>"><td id="tdUnVerifiedListing" colspan="19" class="tdStyle"><b>Mobile Unverified Listings</b></td></tr>
              <asp:Repeater id="rptCustomerUnVerifiedList" runat="server">
                <ItemTemplate>
                    <tr class="dtItem">
                        <td id ="Uprofile_id"><%# DataBinder.Eval(Container.DataItem,"ProfileId")%></td>
                        <td><%# DataBinder.Eval(Container.DataItem,"CustomerName")%></td>
                        <td><%# DataBinder.Eval(Container.DataItem,"CustomerMobile")%></td>
                        <td><%# DataBinder.Eval(Container.DataItem,"BikeName")%></td>
                        <td><%# CommonOpn.FormatNumeric(DataBinder.Eval(Container.DataItem,"Price").ToString())%></td>
                        <td><%# DataBinder.Eval(Container.DataItem,"City")%></td>
                        <td><%# DataBinder.Eval(Container.DataItem,"State")%></td>
                        <td><%# DataBinder.Eval(Container.DataItem,"MakeYear","{0:MMM-yyyy}")%></td>
                        <td><%# CommonOpn.FormatNumeric(DataBinder.Eval(Container.DataItem,"Kilometers").ToString())%></td>
                        <td><%# DataBinder.Eval(Container.DataItem,"Color")%></td>
                        <td><%# DataBinder.Eval(Container.DataItem,"Owner")%></td>
                        <td><%# DataBinder.Eval(Container.DataItem,"InsuranceType")%></td>
                        <td><%# DataBinder.Eval(Container.DataItem,"InsuranceExpiryDate","{0:MMM-yyyy}")%></td>
                        <td><%# DataBinder.Eval(Container.DataItem,"LifetimeTax")%></td>
                        <td><%# DataBinder.Eval(Container.DataItem,"EntryDate","{0:dd-MMM-yyyy}")%></td>
                        <td><input id="btnUView" type="button" onclick ="javascript:window.open('/classified/listingphotos.aspx?profileid=<%# DataBinder.Eval(Container.DataItem,"ProfileId")%>    ','','left=0,top=0,width=1400,height=660,resizable=0,scrollbars=yes')" value ="View Photos" <%# (Convert.ToInt32(DataBinder.Eval(Container.DataItem, "PhotoCount")) > 0) ? "" : "style='display:none;'" %> /></td>
                        <td><input class="approveList" id="btnUApprove" type="button" value ="Approve" profileId="<%# DataBinder.Eval( Container.DataItem, "ProfileId" ) %>" />
                        <input class="discardList" id="btnUDiscard" type="button" value ="Discard" profileId="<%# DataBinder.Eval( Container.DataItem, "ProfileId" ) %>" /></td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
            <tr class="<%= ((listType == (int)ListingType.TotalListings)) ? "" :"hide"  %>"><td id="tdSoldListing" colspan="19" class="tdStyle"><b>Sold Listings</b></td></tr>
              <asp:Repeater id="rptCustomerSoldList" runat="server">
                <ItemTemplate>
                    <tr class="dtItem">
                        <td id ="Sprofile_id"><%# DataBinder.Eval(Container.DataItem,"ProfileId")%></td>
                        <td><%# DataBinder.Eval(Container.DataItem,"CustomerName")%></td>
                        <td><%# DataBinder.Eval(Container.DataItem,"CustomerMobile")%></td>
                        <td><%# DataBinder.Eval(Container.DataItem,"BikeName")%></td>
                        <td><%# CommonOpn.FormatNumeric(DataBinder.Eval(Container.DataItem,"Price").ToString())%></td>
                        <td><%# DataBinder.Eval(Container.DataItem,"City")%></td>
                        <td><%# DataBinder.Eval(Container.DataItem,"State")%></td>
                        <td><%# DataBinder.Eval(Container.DataItem,"MakeYear","{0:MMM-yyyy}")%></td>
                        <td><%# CommonOpn.FormatNumeric(DataBinder.Eval(Container.DataItem,"Kilometers").ToString())%></td>
                        <td><%# DataBinder.Eval(Container.DataItem,"Color")%></td>
                        <td><%# DataBinder.Eval(Container.DataItem,"Owner")%></td>
                        <td><%# DataBinder.Eval(Container.DataItem,"InsuranceType")%></td>
                        <td><%# DataBinder.Eval(Container.DataItem,"InsuranceExpiryDate","{0:MMM-yyyy}")%></td>
                        <td><%# DataBinder.Eval(Container.DataItem,"LifetimeTax")%></td>
                        <td><%# DataBinder.Eval(Container.DataItem,"EntryDate","{0:dd-MMM-yyyy}")%></td>
                        <td><input id="btnSView" type="button" onclick ="javascript:window.open('/classified/listingphotos.aspx?profileid=<%# DataBinder.Eval(Container.DataItem,"ProfileId")%>    ','','left=0,top=0,width=1400,height=660,resizable=0,scrollbars=yes')" value ="View Photos"<%# (Convert.ToInt32(DataBinder.Eval(Container.DataItem, "PhotoCount")) > 0) ? "" : "style='display:none;'" %> /></td>
                        <td></td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </tbody>
    </table>
</div>
<script>
    $(document).ready(function () {
        var liveListings = $("#Lprofile_id").length;
        var pendingListings = $("#Pprofile_id").length;
        var fakeListings = $("#Fprofile_id").length;
        var unVerifiedListings = $("#Uprofile_id").length;
        var soldListings = $("#Sprofile_id").length;
        if (liveListings <= 0) {
            $("#tdLiveListing").addClass("hide");
        }
        if (pendingListings <= 0) {
            $("#tdPendingListing").addClass("hide");
        }
        if (fakeListings <= 0) {
            $("#tdFakeListing").addClass("hide");
        }
        if (unVerifiedListings <= 0) {
            $("#tdUnVerifiedListing").addClass("hide");
        }
        if (soldListings <= 0) {
            $("#tdSoldListing").addClass("hide");
        }
    });

    $(".approveList").click(function(){
        var profileId = $(this).attr("profileId");
        if (confirm("Are you sure you want to approve Inquiry?") == true) {
            $.ajax({
                type: "POST",
                url: "/ajaxpro/BikeWaleOpr.Common.AjaxCommon,BikewaleOpr.ashx",
                data: '{"profileId":"' + profileId + '"}',
                beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "ApproveListing"); },
                success: function (response) {
                    alert("Enquiry Approved Successfully!");
                    window.location.reload(true);
                }
            });
        }
    });

    $(".discardList").click(function(){
        var profileId = $(this).attr("profileId");
        if (confirm("Are you sure you want to discard Inquiry?") == true) {
            $.ajax({
                type: "POST",
                url: "/ajaxpro/BikeWaleOpr.Common.AjaxCommon,BikewaleOpr.ashx",
                data: '{"profileId":"' + profileId + '"}',
                beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "DiscardListing"); },
                success: function (response) {
                    alert("Inquiry Discarded Successfully!");
                    window.location.reload(true);
                }
            });
        }
    });
    </script>
<!-- #Include file="/includes/footerNew.aspx" -->
