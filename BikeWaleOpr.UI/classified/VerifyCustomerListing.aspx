<%@ Page Language="C#" AutoEventWireup="false" Inherits="BikeWaleOpr.Classified.VerifyCustomerListing" Trace="false" %>
<%@ Import Namespace="BikeWaleOpr.Classified" %>
<%@ Register TagPrefix="Pager" TagName="Pager" Src="/controls/LinkPagerControl.ascx" %>
<!-- #Include file="/includes/headerNew.aspx" -->
<title>Verify Customer Listings</title>
<style>
    #VerifyCustomerListings a {color : #FF9148; font-weight: bold;
    }
</style>
<div class="urh">
		You are here &raquo; Classified &raquo; Verify Customer Listings
</div>
<div >
    <!-- #Include file="classifiedMenu.aspx" -->
</div>
<div class="left"><b>Verify Customer Listings</b>
    <table id="VerifyCustomerListings" class="margin-top10" cellpadding="5" border="1" style="text-align:center;font-size:11px;border-style:solid;border-collapse:collapse;">
        <tbody>
            <tr class="dtHeader" >
                <th>Customer Id</th>
                <th>Email Id</th>
                <th>Total Listings</th>
                <th>Live Listings</th>
                <th>Pending Listings</th>
                <th>Fake Listings</th>
                <th>Mobile Unverified Listings</th>
                <th>Sold Listings</th>
                <th style="max-width:100px">IsFake customer?<input type="button" id="btnFakeCustomer" value="Mark Fake" /></th>                
            </tr>
            <asp:Repeater id="rptCustomerList" runat="server">
                <ItemTemplate>
                    <tr class="dtItem">
                        <td><%# DataBinder.Eval(Container.DataItem,"CustomerId")%></td>
                        <td style="text-align:left; font-size:12px"><%# DataBinder.Eval(Container.DataItem,"CustomerEmail")%></td>
                        <td><a class="<%#DataBinder.Eval(Container.DataItem,"TotalListings").ToString()=="0"? "hide" :"" %>" onclick ='<%# DataBinder.Eval(Container.DataItem,"TotalListings").ToString()=="0"? "" :"hidezero("+ DataBinder.Eval( Container.DataItem, "CustomerId" )+","+ (int)ListingType.TotalListings +")"%>' style="cursor:pointer;"><%# DataBinder.Eval(Container.DataItem,"TotalListings")%></a><span class="<%#DataBinder.Eval(Container.DataItem,"TotalListings").ToString()=="0"? "" :"hide" %>"><%# DataBinder.Eval(Container.DataItem,"TotalListings")%></span></td>
                         <td><a class="<%#DataBinder.Eval(Container.DataItem,"LiveListings").ToString()=="0"? "hide" :"" %>" onclick ='<%# DataBinder.Eval(Container.DataItem,"LiveListings").ToString()=="0"? "" :"hidezero("+ DataBinder.Eval( Container.DataItem, "CustomerId" )+","+ (int)ListingType.LiveListings +")"%>' style="cursor:pointer;"><%# DataBinder.Eval(Container.DataItem,"LiveListings")%></a><span class="<%#DataBinder.Eval(Container.DataItem,"LiveListings").ToString()=="0"? "" :"hide" %>"><%# DataBinder.Eval(Container.DataItem,"LiveListings")%></span></td>
                        <td><a class="<%#DataBinder.Eval(Container.DataItem,"PendingListings").ToString()=="0"? "hide" :"" %>" onclick ='<%# DataBinder.Eval(Container.DataItem,"PendingListings").ToString()=="0"? "" :"hidezero("+ DataBinder.Eval( Container.DataItem, "CustomerId" )+","+ (int)ListingType.PendingListings +")"%>' style="cursor:pointer;"><%# DataBinder.Eval(Container.DataItem,"PendingListings")%></a><span class="<%#DataBinder.Eval(Container.DataItem,"PendingListings").ToString()=="0"? "" :"hide" %>"><%# DataBinder.Eval(Container.DataItem,"PendingListings")%></span></td>
                        <td><a class="<%#DataBinder.Eval(Container.DataItem,"FakeListings").ToString()=="0"? "hide" :"" %>" onclick ='<%# DataBinder.Eval(Container.DataItem,"FakeListings").ToString()=="0"? "" :"hidezero("+ DataBinder.Eval( Container.DataItem, "CustomerId" )+","+ (int)ListingType.FakeListings +")"%>' style="cursor:pointer;"><%# DataBinder.Eval(Container.DataItem,"FakeListings")%></a><span class="<%#DataBinder.Eval(Container.DataItem,"FakeListings").ToString()=="0"? "" :"hide" %>"><%# DataBinder.Eval(Container.DataItem,"FakeListings")%></span></td>
                        <td><a class="<%#DataBinder.Eval(Container.DataItem,"UnVerifiedListings").ToString()=="0"? "hide" :"" %>" onclick ='<%# DataBinder.Eval(Container.DataItem,"UnVerifiedListings").ToString()=="0"? "" :"hidezero("+ DataBinder.Eval( Container.DataItem, "CustomerId" )+","+ (int)ListingType.UnVerifiedListings +")"%>' style="cursor:pointer;"><%# DataBinder.Eval(Container.DataItem,"UnVerifiedListings")%></a><span class="<%#DataBinder.Eval(Container.DataItem,"UnVerifiedListings").ToString()=="0"? "" :"hide" %>"><%# DataBinder.Eval(Container.DataItem,"UnVerifiedListings")%></span></td>
                         <td><a class="<%#DataBinder.Eval(Container.DataItem,"SoldListings").ToString()=="0"? "hide" :"" %>" onclick ='<%# DataBinder.Eval(Container.DataItem,"SoldListings").ToString()=="0"? "" :"hidezero("+ DataBinder.Eval( Container.DataItem, "CustomerId" )+","+ (int)ListingType.SoldListings +")"%>' style="cursor:pointer;"><%# DataBinder.Eval(Container.DataItem,"SoldListings")%></a><span class="<%#DataBinder.Eval(Container.DataItem,"SoldListings").ToString()=="0"? "" :"hide" %>"><%# DataBinder.Eval(Container.DataItem,"SoldListings")%></span></td>
                        <td><input type="checkbox" id="chkCustomer" CustomerId="<%# DataBinder.Eval( Container.DataItem, "CustomerId" ) %>" <%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "IsFake")) ? "checked" : "" %> /></td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </tbody>
    </table>
    <div class="margin-top10">
        <Pager:Pager id="linkPager" runat="server"></Pager:Pager>
    </div>
</div>
<div class="margin-top10"> </div>

<!-- #Include file="/includes/footerNew.aspx" -->
<script>
    $("#btnFakeCustomer").click(function () {
        var CustIdList = "";
        $("#VerifyCustomerListings").find("input:checked").each(function () {
            CustIdList += $(this).attr("CustomerId") + ',';
        });
        if (CustIdList != "") {
            if (confirm("Are you sure you want to mark customer fake?") == true) {
                $.ajax({
                    type: "POST",
                    url: "/ajaxpro/BikeWaleOpr.Common.AjaxCommon,BikewaleOpr.ashx",
                    data: '{"CustIdList":"' + CustIdList + '"}',
                    beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "DiscardCustomers"); },
                    success: function (response) {
                        alert("Customers Marked Fake Successfully!");
                    }
                });
            }
        }
        else { alert("Please select a customer!")}
    });

    ////function hidezero(customerId, listingType) {
    ////    javascript: window.open('totallistings.aspx?custid=' + customerId + '&listtype=' + listingType + '',
    ////        '', 'left=0,top=0,resizable=0,width=1400,height=660,scrollbars=yes');

    //function hidezero(customerId, listingType) {
    //    var win = window.open('totallistings.aspx?custid=' + customerId + '&listtype=' + listingType + '',
    //      '', 'left=0,top=0,resizable=0,width=1400,height=660,scrollbars=yes');
    //}

    var timerObj, newWindow;

    function hidezero(customerId, listingType) {

        var url = 'totallistings.aspx?custid=' + customerId + '&listtype=' + listingType + '';
        switch (listingType) {
            case 1:
                newWindow = window.open(url, '', 'left=0,top=0,height=660,width=1400,menubar=no,resizable=yes,scrollbars=yes');
                timerObj = window.setInterval("fun_To_ReTitle(" + "'Total Listings'" + ")", 500);
                break;
            case 2: newWindow = window.open(url, '', 'left=0,top=0,height=660,width=1400,menubar=no,resizable=yes,scrollbars=yes');
                timerObj = window.setInterval("fun_To_ReTitle(" + "'Live Listings'" + ")", 500);
                break;
            case 3: newWindow = window.open(url, '', 'left=0,top=0,height=660,width=1400,menubar=no,resizable=yes,scrollbars=yes');
                timerObj = window.setInterval("fun_To_ReTitle(" + "'Pending Listings'" + ")", 500);
                break;
            case 4: newWindow = window.open(url, '', 'left=0,top=0,height=660,width=1400,menubar=no,resizable=yes,scrollbars=yes');
                timerObj = window.setInterval("fun_To_ReTitle(" + "'Fake Listings'" + ")", 500);
                break;
            case 5: newWindow = window.open(url, '', 'left=0,top=0,height=660,width=1400,menubar=no,resizable=yes,scrollbars=yes');
                timerObj = window.setInterval("fun_To_ReTitle(" + "'Mobile Unverified Listings'" + ")", 500);
                break;
            case 6: newWindow = window.open(url, '', 'left=0,top=0,height=660,width=1400,menubar=no,resizable=yes,scrollbars=yes');
                timerObj = window.setInterval("fun_To_ReTitle(" + "'Sold Listings'" + ")", 500);
                break;
        }
    }


    //function openDetailsPopUpWindow(url) {
    //    newWindow = window.open(url, '', 'height=500,width=700,menubar=no,resizable=yes,scrollbars=yes');
    //    timerObj = window.setInterval("fun_To_ReTitle('~~newTitle~~ ')", 10);
    //}


    function fun_To_ReTitle(newTitle) {
        if (newWindow.document.readyState == 'complete') {
            newWindow.document.title = newTitle;
            window.clearInterval(timerObj);
        }
    }
</script>