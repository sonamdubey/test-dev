<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.MyBikeWale.MyListing" %>
<%@ Import NameSpace="Bikewale.Common" %>
<%
    AdId = "1395996606542";
    AdPath = "/1017752/BikeWale_MyBikeWale_";
%>
<!-- #include file="/includes/headmybikewale.aspx" -->
<style>
    .bikeDetails li {display:block;}
</style>
<div class="container_12">
    <div class="grid_12">
        <ul class="breadcrumb">
            <li>You are here: </li>
            <li><a href="/">Home</a></li>
            <li class="fwd-arrow">&rsaquo;</li>
            <li><a href="/mybikewale/">My BikeWale</a></li>
            <li class="fwd-arrow">&rsaquo;</li>
            <li class="current"><strong>My Inquiries</strong></li>
        </ul><div class="clear"></div>
    </div>    
    
    <div class="grid_8 margin-top10">
        <div id="div_FakeCustomer" class="grid_8 alpha omega" style="width:614px;" runat="server">
            <h1>Sell Your Bike - Easy & Fast</h1>
            <h3 class="grey-bg border-light padding5 margin-top10 margin-bottom10 isfake">You are not authorized to add any listing. Please contact us on <u>contact@bikewale.com</u></h3>
        </div>
        <h2>My Bike(s) Listed For Sale</h2>
        <asp:DataList runat="server" ID="rptListings" RepeatColumns="1" RepeatDirection="Horizontal">
            <ItemTemplate>
                <div id="div_<%# DataBinder.Eval(Container.DataItem, "InquiryId") %>" class="grey-bg content-block border-light margin-top10">
                    <div class="grid_2 alpha omega">
                        <%--<img src="<%# GetImagePath(DataBinder.Eval(Container.DataItem, "ImageUrlThumbSmall").ToString(), DataBinder.Eval(Container.DataItem, "DirectoryPath").ToString(), DataBinder.Eval(Container.DataItem, "HostURL").ToString())%>" title="<%# DataBinder.Eval(Container.DataItem, "Bike") %>" />--%>
                        <img src="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImagePath").ToString(), DataBinder.Eval(Container.DataItem, "HostURL").ToString(),Bikewale.Utility.ImageSize._110x61)%>" title="<%# DataBinder.Eval(Container.DataItem, "Bike") %>" />
                        <div class="margin-top5">Profile : S<%# DataBinder.Eval(Container.DataItem, "InquiryId") %></div>
                        <%--<div class="margin-top5"><a href="/used/bikedetails.aspx?bike=S<%# DataBinder.Eval(Container.DataItem, "InquiryId") %>">View Bike Details</a></div>--%>
                        <div class="<%# DataBinder.Eval(Container.DataItem, "StatusId").ToString() == "1" ?  "margin-top5" : "hide" %> <%= isFake ? "hide" : "" %>"><a href="/used/bikes-in-<%# DataBinder.Eval( Container.DataItem, "CityMaskingName" ).ToString() %>/<%# DataBinder.Eval( Container.DataItem, "MakeMaskingName" ).ToString() %>-<%# DataBinder.Eval( Container.DataItem, "ModelMaskingName" ).ToString() %>-S<%# DataBinder.Eval( Container.DataItem, "InquiryId" ) %>/">View Bike Details</a></div>
                    </div>
                    <div class="grid_4 alpha omega">
                        <h3><%# DataBinder.Eval(Container.DataItem, "Bike") %></h3>
                        <div class="margin-top20">
                            <span class="margin-right10 text-highlight">Total Buyers : <%# DataBinder.Eval(Container.DataItem, "TotalViews") %></span>
                            <a class="buttons btn-xs <%# Convert.ToInt32(DataBinder.Eval(Container.DataItem, "TotalViews")) == 0 ? "hide" : "show" %> <%# DataBinder.Eval(Container.DataItem,"StatusId").ToString() == "2" ? "hide" : "" %>" href="/mybikewale/buyerdetails.aspx?id=<%# DataBinder.Eval(Container.DataItem, "InquiryId") %>">View Buyer Details</a>
                            <div class="margin-top10">Bike Listed On : <%# Convert.ToDateTime(DataBinder.Eval(Container.DataItem, "EntryDate")).ToString("MMMM dd, yyyy") %></div>
                        </div>
                    </div>                    
                    <div class="grid_2 omega">
                        <ul class="bikeDetails">
                            <li><span class="text-highlight">Make Year : </span><span><%# Convert.ToDateTime(DataBinder.Eval(Container.DataItem, "MakeYear")).ToString("MMM, yyyy") %></span></li>
                            <li><span class="text-highlight">Kms Done : </span><span><%# CommonOpn.FormatNumeric(DataBinder.Eval(Container.DataItem, "Kilometers").ToString()) %></span></li>
                            <li><span class="text-highlight">Rs : </span><span><%# CommonOpn.FormatNumeric(DataBinder.Eval(Container.DataItem, "Price").ToString()) %></span></li>
                            <li><span class="text-highlight">Color : </span><span><%# DataBinder.Eval(Container.DataItem, "Color") %></span></li>
                            <li><span class="text-highlight">Registration : </span><span><%# DataBinder.Eval(Container.DataItem, "RegistrationPlace") %></span></li>
                            <li><span class="text-highlight">Owners : </span><span><%# DataBinder.Eval(Container.DataItem, "Owner") %></span></li>
                        </ul>                        
                    </div>
                    <div class="clear"></div>
                    <% if(!isFake) { %>
                    <div class="margin-top10">                                        
                        <div class="<%# DataBinder.Eval(Container.DataItem, "StatusId").ToString() == "1" ?  "left-float" : "hide" %>"><a target="_blank" href="/used/sell/default.aspx?id=<%# DataBinder.Eval(Container.DataItem, "InquiryId") %>">Edit bike details</a> | <a target="_blank" href="/used/sell/uploadbasic.aspx?id=<%# DataBinder.Eval(Container.DataItem, "InquiryId") %>">Upload bike photos</a> | <a class="pointer" onclick="removeBike('<%# DataBinder.Eval(Container.DataItem, "InquiryId") %>')">Remove from listing</a></div>                        
                        
                        <div id="div_status <%= isFake ? "hide" : "" %>"" class="right-float" style="color:#f00;">
                            <%# GetStatus(DataBinder.Eval(Container.DataItem, "StatusId").ToString(),Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"IsApproved")),DataBinder.Eval(Container.DataItem, "InquiryId").ToString()) %>
                        </div>
                    <div class="clear"></div>
                    </div> 
                    <% } %>                                       
                </div>
            </ItemTemplate>
        </asp:DataList>
        <div id="div_SellYourBike" class="content-block action-btn grey-bg border-light margin-top15" runat="server">
            <span class="margin-right10 margin-left10" style="font-size:14px;">You have not listed any bike</span><a href="/used/sell/">List Your Bike Here</a>
        </div>
	</div>
    <div class="grid_4">
        <div class="margin-top15">
            <!-- BikeWale_NewBike/BikeWale_NewBike_HP_300x250 -->
            <!-- #include file="/ads/Ad300x250.aspx" -->
        </div>
    </div>
</div>
<script type="text/javascript">
    var isRemoved = false;

    function removeBike(bikeId) {
        var left = (screen.width - 300) / 2;
        var top = (screen.height - 250) / 2;
        var type = "1";	//for sell inquiry
        window.open("/mybikewale/removeFromListing.aspx?type=" + type + "&id=" + bikeId, "remove", "menu=no,address=no,scrollbars=no,resizable=no,width=310,height=290,left=" + left + ",top=" + top);
    }
</script>
<!-- #include file="/includes/footerinner.aspx" -->