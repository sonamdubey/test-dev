<%@ Control Language="C#" AutoEventWireup="false" Inherits="Carwale.UI.Controls.SubNavigation" %>
<%@ Import Namespace="Carwale.UI.Common" %>
<%@ Import Namespace="Carwale.Utility" %>
<%@ Import Namespace="Carwale.BL.CMS" %>
<% string carName = string.Format("{0} {1}", Make, ModelName);%>
<script runat="server">	
    string staticUrl = System.Configuration.ConfigurationManager.AppSettings["staticUrl"];
    string stagingPath = System.Configuration.ConfigurationManager.AppSettings["stagingPath"];
</script>
<link rel="stylesheet" href="/static/css/dropdown-menu.css" type="text/css" >
<script  type="text/javascript"  src="/static/src/dropdown-menu.min.js" ></script>
<script type="text/javascript">
    $(document).ready(function (e) {
        $('#dropdown-nav .dropdown-menu').dropdown_menu({
            sub_indicators: true,
            drop_shadows: true,
            close_delay: 300
        });
    });
</script>
<!-- nav code starts here -->
<% if (!subNavOnCarCompare)
   { %>
<div id="dropdown-nav">
    <ul id="ulSubNavigation" class="dropdown-menu dropdown-menu-skin">
        <li id="liOverview"><a data-role = "click-tracking" data-event = "CWInteractive" data-cat = "Sub Navigation" data-action="<%=ModelName %>" data-label="<%=Category%>-<%=Make%>-<%=ModelName%>-<%=CityName%>" id="overview" class="<%=ModelName.Length>10?"truncate-modelname":"" %>" title="<%=ModelName %>" href="/<%= UrlRewrite.FormatSpecial(Make)%>-cars/<%= MaskingName %>/"><%=ModelName %></a></li>
        <li onclick="GetPriceQuote('','<%=ModelId%>',<%=PQPageId%>, this);"><a data-role = "click-tracking" data-event = "CWInteractive" data-cat = "Sub Navigation" data-action="On Road Price" data-label="<%=Category%>-<%=Make%>-<%=ModelName%>-<%=CityName%>" href="javascript:void(0)">On Road Price</a></li>
        <%if (IsReviewsAvial)
          {%>
        <li id="liReviews">
            <a href="javascript:void(0)">Reviews</a>
            <ul>
                <%if (IsExpertReviewAvial)
                  { %><li hastagname="#expertreviews"><a data-role = "click-tracking" data-event = "CWInteractive" data-cat = "Sub Navigation" data-action="Expert Reviews" data-label="<%=Category%>-<%=Make%>-<%=ModelName%>-<%=CityName%>" href="/<%= UrlRewrite.FormatSpecial(Make)%>-cars/<%= MaskingName %>/expert-reviews/">Expert Reviews</a></li>
                <%} %>
                <%if (IsUserReviewsAvailable)
                  { %>
                <li hastagname="#userreviews"><a data-role = "click-tracking" data-event = "CWInteractive" data-cat = "Sub Navigation" data-action="User Reviews" data-label="<%=Category%>-<%=Make%>-<%=ModelName%>-<%=CityName%>" href="/<%= UrlRewrite.FormatSpecial(Make)%>-cars/<%= MaskingName %>/userreviews/">User Reviews</a></li>
                <%} %>
            </ul>
        </li>
        <%}
          else
          {%>
        <li id="liNews" hastagname="#news"><a data-role = "click-tracking" data-event = "CWInteractive" data-cat = "Sub Navigation" data-action="News" data-label="<%=Category%>-<%=Make%>-<%=ModelName%>-<%=CityName%>" href="/<%= UrlRewrite.FormatSpecial(Make)%>-cars/<%= MaskingName %>/news/">News</a></li>
        <%}%>
            <%if (is360Available || ImageCount > 0 || VideoCount > 0)
        {%>
                            <li id="liPhotos">
                                <a href="javascript:void(0)">Gallery</a>
                                <ul>
                                    <%if (ImageCount > 0)
    {%>
                                        <li ><a data-role = "click-tracking" data-event = "CWInteractive" data-cat = "Sub Navigation" data-action="Images" data-label="<%=Category%>-<%=Make%>-<%=ModelName%>-<%=CityName%>" href="/<%= UrlRewrite.FormatSpecial(Make)%>-cars/<%= MaskingName %>/images/">Images (<%=ImageCount%>)</a></li>
                                    <%}%>
    <%if (is360Available)
    {%>
                                        <li data-role='inview-imp' data-cat='desktop_360_linkages' data-label='<%=carName%>' data-action='<%=TrackingFor360%>_sub_nav_impression'>
                                        <a data-role = "click-tracking" data-event = "CWInteractive" data-cat = "desktop_360_linkages" data-action="<%=TrackingFor360 %>_sub_nav" data-label="<%=carName%>" style="padding: 10px 9px;" href="<%=Carwale.UI.PresentationLogic.EditorialContent.Get360PageUrl(Make, MaskingName, Default360Category, false)%>">360° View</a></li>
                                    <%}%>
    <%if (VideoCount > 0)
    {%>
                                        <li data-role="inview-imp" data-cat="desktop_video_linkages" data-action="<%=TrackingFor360%>_sub_nav_impression" data-label="<%= carName%>" >
                                            <a data-role = "click-tracking" data-event = "CWInteractive" data-cat="desktop_video_linkages" data-action="<%= TrackingFor360%>_sub_nav" data-label="<%=carName%>" href="<%=Carwale.BL.CMS.CMSCommon.GetVideoUrl(Make, MaskingName, null, 0)%>">Videos (<%=VideoCount%>)</a></li>
                                    <%}%>
                                </ul>
                            </li>
                        <%}%>
        <%if(ModelId > 0) {%>
        <li id="liColors" hastagname="#colours"><a data-role = "click-tracking" data-event = "CWInteractive" data-cat = "Sub Navigation" data-action="Colours" data-label="<%=Category%>-<%=Make%>-<%=ModelName%>-<%=CityName%>" href="/<%= UrlRewrite.FormatSpecial(Make)%>-cars/<%= MaskingName %>/<%=ShowColorsLink? "colours/" : "#colours" %>">Colours</a></li>
        <%} %>
        <li id="liVersions" hastagname="#versions"><a data-role = "click-tracking" data-event = "CWInteractive" data-cat = "Sub Navigation" data-action="Versions" data-label="<%=Category%>-<%=Make%>-<%=ModelName%>-<%=CityName%>" href="/<%= UrlRewrite.FormatSpecial(Make)%>-cars/<%= MaskingName %>/#versions">Versions</a></li>
        <li id="liMore">
            <a href="javascript:void(0)">More</a>
            <ul>
                <%if (IsReviewsAvial)
                  {%>
                <li hastagname="#news"><a data-role = "click-tracking" data-event = "CWInteractive" data-cat = "Sub Navigation" data-action="News" data-label="<%=Category%>-<%=Make%>-<%=ModelName%>-<%=CityName%>" href="/<%= UrlRewrite.FormatSpecial(Make)%>-cars/<%= MaskingName %>/news/">News</a></li>
                <%} %>
                <%if (IsMileageAvail && !IsMileagePage)
                  {%>
                <li><a data-role = "click-tracking" data-event = "CWInteractive" data-cat = "Sub Navigation" data-action="Mileage" data-label="<%=Category%>-<%=Make%>-<%=ModelName%>-<%=CityName%>" href="/<%= UrlRewrite.FormatSpecial(Make)%>-cars/<%= MaskingName %>/mileage/">Mileage</a></li>
                <% } %>
                <li><a data-role = "click-tracking" data-event = "CWInteractive" data-cat = "Sub Navigation" data-action="Dealers" data-label="<%=Category%>-<%=Make%>-<%=ModelName%>-<%=CityName%>" href="/new/<%= UrlRewrite.FormatSpecial(Make)%>-dealers/">Dealers</a></li>
                <%if (IsUsedCarAvail)
                  { %>
                <li hastagname="#ucsusedcarsuggestion_divusedcarsuggestion"><a data-role = "click-tracking" data-event = "CWInteractive" data-cat = "Sub Navigation" data-action="Used Model" data-label="<%=Category%>-<%=Make%>-<%=ModelName%>-<%=CityName%>" href="/used/<%= UrlRewrite.FormatSpecial(Make)%>-<%= MaskingName %>-cars/">Used Model</a></li>
                <%} %>
            </ul>
        </li>
    </ul>
</div>
<div class="clear"></div>
<%} %>
<div id="menuareaExpReview" <% if (PageId != "11")
                               { %>class="hide"
    <% } %>>
    <ul class="menu" style="display: inline;">
        <asp:Repeater ID="rptPages" runat="server">
            <ItemTemplate>
                <li style="display: inline; margin-right: 10px;"><a href="#<%# Format.RemoveSpecialCharacters( Container.DataItem.ToString()) %>">
                    <strong>
                        <%# Container.DataItem %>
                    </strong>
                </a>
                </li>
            </ItemTemplate>
        </asp:Repeater>
    </ul>
    <div class="clear"></div>
</div>
<!-- nav code ends here -->

<script type="text/javascript">
    $(window).load(function () {
        var hashTag = window.location.hash;
        if (hashTag.length > 0) {
            ScrollPage(hashTag);
        }
    });

    $("#overview").click(function () {
        $("html, body").animate({ scrollTop: 0 }, 500);
        $('#ulSubNavigation li').find('.active').removeClass("active");
        $('#liOverview').find('a').addClass("active");
        window.location.hash = '';
        window.history.replaceState('', document.title, window.location.pathname);
    });

    var priceQuoteClicked = false;
    $(document).ready(function () {
        <%if (Convert.ToBoolean(IsOverviewPage))
          {%>
        $('#overview').removeAttr("href");
        <%}%>
        selectSubNaviTab(window.location.hash.toLowerCase());
    });

    function ScrollPage(hashTag) {
        hashTag = hashTag.replace('#', '');
        $('html, body').animate({ scrollTop: $('#' + hashTag).offset().top - 50 });
    }

    $("#ulSubNavigation li").click(function () {
        var hasTagName = $(this).attr("hasTagName");
        if (hasTagName) {
            selectSubNaviTab(hasTagName.toLowerCase());
            ScrollPage(hasTagName);
        }
    });

    function selectSubNaviTab(subTabUrl) {
        $('#ulSubNavigation li').find('.active').removeClass("active");
        <%if (!IsOverviewPage)
          {%>
        $('#ulSubNavigation li').find('a').removeClass("active");
        <%}%>

        if ((subTabUrl.indexOf("#expertreviews") > -1) || (subTabUrl.indexOf("#userreviews") > -1) || ($("#hdnTest").val() == "43")) {
            $('#liReviews a').first().addClass("active");
        }
        else if (subTabUrl.indexOf("#colours") > -1) {
            $('#liColors').find('a').addClass("active");
        }
            // for photo tab active
        <% if (PageId == "0" && !is360Available)
           { %>
        else if (subTabUrl > -1) {
            $('#liPhotos').find('a').addClass("active");
        }
        <% } %>

        else if (subTabUrl.indexOf("#versions") != -1) {
            $('#liVersions').find('a').addClass("active");
        }
        else if ((subTabUrl.indexOf("#news") != -1) || (subTabUrl.indexOf("#video") != -1) || (subTabUrl.indexOf("#ucsusedcarsuggestion_divusedcarsuggestion") != -1)) {
            $('#liMore a').first().addClass("active");
        }
        else {
             <%if (Convert.ToBoolean(IsOverviewPage))
               {%>
            $('#liOverview').find('a').addClass("active");
            <%}%>
        }

    }
    function formatURL(str) {
        str = str.toLowerCase();
        str = str.replace(/[^-0-9a-zA-Z]/g, '');
        return str;
    }
</script>
