<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.UsedBikeWidget" %>
    <% 
    %>
<% if (FetchedRecordsCount > 0)
   {        %>
<div id="used-bikes-content" class="grid-12 padding-top20 padding-bottom20">
    <div class="grid-<%=masterGrid %> font14">
        <h2 class="font18 margin-bottom15"><%= pageHeading %></h2>
        <%foreach (Bikewale.Entities.UsedBikes.MostRecentBikes bike in objUsedBikes)
          { %>
        <div class="grid-<%=childGrid %> alpha margin-bottom20">
            <a title="<%= String.Format("{0}, {1} {2} {3}", bike.MakeYear,bike.MakeName,bike.ModelName,bike.VersionName)%>" href="/used/bikes-in-<%= bike.CityMaskingName %>/<%= bike.MakeMaskingName %>-<%= bike.ModelMaskingName %>-<%= bike.ProfileId %>/"><%=String.Format("{0}, {1} {2} {3}", bike.MakeYear,bike.MakeName,bike.ModelName,bike.VersionName)%> </a>
            <p class="margin-top10"><span class="bwsprite inr-sm-dark"></span> <%= Bikewale.Utility.Format.FormatPrice(bike.BikePrice.ToString()) %></p>
        </div>
        <% } %>
        <a title="View all <%=makeName%> used bikes in <%=cityName %>" href="<%= Bikewale.Utility.UrlFormatter.ViewMoreUsedBikes(Convert.ToUInt32(CityId), cityMaskingName, makeMaskingName, modelMaskingName) %>">View all used bikes in <%= cityName%><span class="bwsprite blue-right-arrow-icon"></span></a>        
    </div>
    <% if (isAd)
       { %>
    <div class="grid-4 alpha">
        <div class="rightfloat">
            <script type='text/javascript' src='https://www.googletagservices.com/tag/js/gpt.js'>
                              googletag.pubads().definePassback('/1017752/Bikewale_PQ_300x250', [300, 250]).display();
            </script>
        </div>
    </div>
    <% } %>
    <div class="clear"></div>
</div>
<% } %>
