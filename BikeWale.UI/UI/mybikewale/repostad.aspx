<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.MyBikewale.RepostSellBikeAd"  EnableViewState="false" %>
<%
    title = "My BikeWale - Repost Sell Bike Ad";
%>
<!-- #include file="/includes/headMyBikeWale.aspx" -->
    <div class="container_12 container-min-height">
        <div class="grid_12">
            <ul class="breadcrumb">
                <li>You are here: </li>
                <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                    <a href="/" itemprop="url">
                        <span itemprop="title">Home</span>
                    </a>
                </li>
                <li class="fwd-arrow">&rsaquo;</li>
                <li class="current"><strong>My BikeWale</strong></li>
            </ul><div class="clear"></div>
        </div>
        <div class="grid_12 margin-top10">
            <div class="content-block grey-bg border-light margin-top15">
                <h2>Repost Your Listing</h2>
                <% if (isAuthorized)
                   {
                       if (isReposted)
                       {%>

                        <div class="margin-top20 text-bold">Congratulations! Your listing has been reposted successfully.</div>
                        <div class="margin-top10">You can view, edit and delete your listing from <span class="margin-left5"><a href="/mybikewale/default.aspx">My BikeWale</a></span></div>
                        <%}
                       else
                       { %>

                        <div class="margin-top10">If you repost, the listing will be available on BikeWale for 90 more days.</div>
                        <div class="margin-top10 text-bold">Do you want to repost your listing?</div>

                        <div class="margin-top10 text-bold">
                            <asp:button id="btnRepost" cssclass="action-btn text_white" runat="server" text="Yes" />
                        </div>
                        <% } %>
                <% }
                   else
                   { %>
                    <div class="margin-top20 text-bold">You don't have access to repost this listing because this listing was posted using a different login id.</div>
                    <div class="margin-top10">You should use the same login credentials (email and password) that you had used while posting this listing on BikeWale.</div>

                <% } %>
            </div>
        </div>
        <div class="grid_4">
            <div class="margin-top15">
                <!-- BikeWale_NewBike/BikeWale_NewBike_HP_300x250 -->
                <!-- #include file="/ads/Ad300x250.aspx" -->
            </div>
        </div>
    </div>
<!-- #include file="/includes/footerInner.aspx" -->
