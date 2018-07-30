<%@ Control Language="C#" EnableViewState="false" AutoEventWireup="false" Inherits="Bikewale.Controls.UsedPopularModelsInCity" %>
<% if (FetchedRecordsCount > 0)
   {        %>
<div id="used-bikes-content" class="grid-12 padding-top20 padding-bottom5">
    <div class="grid-<%=masterGrid %> font14">
        <h2 class="font18 margin-bottom15">Used <%= MakeName %> bikes in <%= CityName %></h2>
        <%foreach (var bike in UsedBikeModelInCityList)
          { %>
        <div class="grid-<%=childGrid %> alpha margin-bottom20">
            <a title="Used <%= String.Format("{0} {1}", bike.MakeName,bike.ModelName) %> bikes in <%= CityName %>" href="<%= Bikewale.Utility.UrlFormatter.ViewMoreUsedBikes(CityId,CityMaskingName,bike.MakeMaskingName,bike.ModelMaskingName) %>"><%= bike.AvailableBikes %> Used <%= String.Format("{0} {1} {2}",bike.MakeName,bike.ModelName,(bike.AvailableBikes > 1 ? "bikes":"bike")) %></a>
            <p class="margin-top10">Starting at <span class="bwsprite inr-sm-dark"></span> <%= Bikewale.Utility.Format.FormatPrice(bike.MinimumPrice) %> </p>
         </div>
        <% } %>
        <a title="Used <%=MakeName%> bikes in <%=CityName %>" href="<%= Bikewale.Utility.UrlFormatter.ViewMoreUsedBikes(Convert.ToUInt32(CityId), CityMaskingName, MakeMaskingName) %>">View all used <%=MakeName %> bikes in <%= CityName%><span class="bwsprite blue-right-arrow-icon"></span></a>
    </div>
    <% if (IsAd)
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
