<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.MyBikeWale.MyListing" EnableViewState="false" %>
<%@ Import NameSpace="Bikewale.Common" %>
<%
    AdId = "1395996606542";
    AdPath = "/1017752/BikeWale_MyBikeWale_";
    //Modified By :Sajal Gupta on 03 August 2016
    isAd300x250BtfShown = false;
%>
<!-- #include file="/UI/includes/headmybikewale.aspx" -->
<style>
    .bikeDetails li {display:block;}
</style>
<div class="container_12">
    <div class="grid_12">
        <ul class="breadcrumb">
            <li>You are here: </li>
            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                <a href="/" itemprop="url">
                    <span itemprop="title">Home</span>
                </a>
            </li>
            <li class="fwd-arrow">&rsaquo;</li>
            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                <a href="/mybikewale/" itemprop="url">
                    <span  itemprop="title">My BikeWale</span>
                </a>
            </li>
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
                <% if (listingDetailsList != null && listingDetailsList.Count() > 0)
                   { 
                    foreach (var listingDetails in listingDetailsList)  
                   { %>
                <div id="div_<%= listingDetails.InquiryId %>" class="grey-bg content-block border-light margin-top10">
                    <div class="grid_2 alpha omega">                        
                        <img src="<%= Bikewale.Utility.Image.GetPathToShowImages(listingDetails.Photo.OriginalImagePath, listingDetails.Photo.HostUrl, Bikewale.Utility.ImageSize._110x61)%>" title="<%= listingDetails.BikeName %>" />
                        <div class="margin-top5">Profile : <%= (listingDetails.SellerType == 2 ? "S":"D")%><%= listingDetails.InquiryId %></div>      
                        <% if(listingDetails.StatusId == 1 &&  !isFake) { %>                  
                        <div class="margin-top5"><a href="/used/bikes-in-<%= listingDetails.CityMaskingName %>/<%= listingDetails.MakeMaskingName %>-<%= listingDetails.ModelMaskingName %>-<%= (listingDetails.SellerType == 2 ? "S":"D")%><%= listingDetails.InquiryId%>/">View Bike Details</a></div>
                        <% } %>
                    </div>
                    <div class="grid_4 alpha omega">
                        <h3><%= listingDetails.BikeName %></h3>
                        <div class="margin-top20">
                            <span class="margin-right10 text-highlight">Total Buyers : <%= listingDetails.TotalViews %></span>
                            <% if(listingDetails.TotalViews != 0 && listingDetails.StatusId != 2) { %>
                            <a class="buttons btn-xs" href="/mybikewale/buyerdetails.aspx?id=<%= listingDetails.InquiryId %>">View Buyer Details</a>
                            <% } %>
                            <div class="margin-top10">Bike Listed On : <%= listingDetails.EntryDate.ToString("dd MMMM yyyy") %></div>
                        </div>
                    </div>                    
                    <div class="grid_2 omega">
                        <ul class="bikeDetails">
                            <li><span class="text-highlight">Make Year : </span><span><%= listingDetails.ModelYear.ToString("MMM, yyyy") %></span></li>
                            <li><span class="text-highlight">Kms Done : </span><span><%= CommonOpn.FormatNumeric(listingDetails.KmsDriven.ToString()) %></span></li>
                            <li><span class="text-highlight">Rs : </span><span><%= CommonOpn.FormatNumeric(listingDetails.AskingPrice.ToString()) %></span></li>
                            <li><span class="text-highlight">Color : </span><span><%= listingDetails.Color %></span></li>
                            <li><span class="text-highlight">Registration : </span><span><%= listingDetails.RegisteredAt %></span></li>
                            <li><span class="text-highlight">Owners : </span><span><%= listingDetails.Owner %></span></li>
                        </ul>                        
                    </div>
                    <div class="clear"></div>
                    <% if (!isFake && listingDetails.DaysRemaining < 91)
                       { %>
                    <div class="margin-top10"> 
                        <% if(listingDetails.StatusId == 1) { %>                                       
                        <div class= "left-float"><a target="_blank" rel="noopener" href="/used/sell/default.aspx?id=<%= listingDetails.InquiryId %>">Edit bike details</a> | <a target="_blank" rel="noopener" href="/used/sell/default.aspx?id=<%= listingDetails.InquiryId %>&hash=uploadphoto">Upload bike images</a> | <a class="pointer" title="Remove this listing" href="/used/inquiry/<%= listingDetails.InquiryId %>/remove/">Remove from listing</a></div>                        
                        <% } 
                           if (!isFake) { %>
                        <div id="div_status" class="right-float" style="color:#f00;">
                            <%= GetStatus(listingDetails.StatusId, listingDetails.IsApproved, listingDetails.InquiryId) %>
                        </div>
                        <% } %>
                    <div class="clear"></div>
                    </div> 
                    <% } %>
                    <% if (listingDetails.DaysRemaining > 82 && listingDetails.DaysRemaining < 91 && listingDetails.StatusId == 1 && listingDetails.IsApproved) 
                       { %>
                    <div class="margin-top10">This listing is about to expire. Haven't sold this bike? <a href= <%= string.Format("/used/inquiry/{0}/repost/", listingDetails.InquiryId)%>>Click here</a> to repost.</div>
                    <% }
                       else if (listingDetails.StatusId == 6 && listingDetails.DaysRemaining > 90) 
                       { %>   
                    <div class="margin-top10">This listing has expired. Haven't sold this bike? <a href= <%= string.Format("/used/inquiry/{0}/repost/", listingDetails.InquiryId)%>>Click here</a> to repost.                   
                    <div class="right-float" style="color:#f00;">[ Expired ]</div>  
                    </div>                                                   
                    <% } %>                                    
                </div>
            <%  }
                }  else { %>
        <div id="div_SellYourBike" class="content-block grey-bg border-light margin-top15" runat="server">
            <span class="margin-right10 margin-left10" style="font-size:14px;">You have not listed any bike</span>
            <a href="/used/sell/" class="action-btn">List Your Bike Here</a>
        </div>
        <% } %>
	</div>
    <div class="grid_4">
        <div class="margin-top15">
            <!-- BikeWale_NewBike/BikeWale_NewBike_HP_300x250 -->
            <!-- #include file="/UI/ads/Ad300x250.aspx" -->
        </div>
    </div>
</div>
<!-- #include file="/UI/includes/footerinner.aspx" -->
