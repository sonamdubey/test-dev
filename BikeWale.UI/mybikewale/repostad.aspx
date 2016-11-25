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
        <% if (isAuthorized)
           {
               if (isReposted)
               {%>
                <div class="grid_12 margin-top10">
                    <div class="content-block grey-bg border-light margin-top15">
                        <h2>Repost your ad</h2>
                        <div class="margin-top20 text-bold">Congratulations, Your ad has been resposted.</div>
                        <div class="margin-top10">To access your ads, please visit<span class="margin-left5"><a href="/mybikewale/default.aspx">My BikeWale</a></span></div>
                    </div>
                </div>
                <%}
                 else
                 { %>
                <div class="grid_12 margin-top10">
                    <div class="content-block grey-bg border-light margin-top15">
                        <h2>Repost your ad</h2>
                        <div class="margin-top20">Dear user, please click on the following button to repost your ad.</div>
                        <div class="margin-top20 text-bold">
                            <asp:button id="btnRepost" cssclass="action-btn text_white" runat="server" text="Repost" />
                        </div>
                    </div>
                </div>
                <% } %>
        <% }
           else
           { %>
        <div class="grid_12 margin-top10">
            <div class="content-block grey-bg border-light margin-top15">
                <h2>Unauthorized</h2>
                <div class="margin-top5">You are not authorized to access this page.</div>
                <div class="margin-top10">To access your ads, please visit<span class="margin-left5"><a href="/mybikewale/default.aspx">My BikeWale</a></span></div>
            </div>
        </div>
        <% } %>
        <div class="grid_4">
            <div class="margin-top15">
                <!-- BikeWale_NewBike/BikeWale_NewBike_HP_300x250 -->
                <!-- #include file="/ads/Ad300x250.aspx" -->
            </div>
        </div>
    </div>
<!-- #include file="/includes/footerInner.aspx" -->
