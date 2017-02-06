<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.UsedPopularModels" %>
<% if(FetchedRecordsCount > 0){ %>
<div id="makeUsedBikeContent" class="bw-model-tabs-data margin-right10 margin-left10 padding-top20 padding-bottom20 font14">

    <h2 class="padding-left10 padding-right10">Recently uploaded Used <%= MakeName %> bikes</h2>
    <!-- when city is not selected -->
    <div class="grid-12 alpha omega text-black">
        <%
         foreach (var bike in UsedBikeModelInCityList)
           { %>
                <div class="grid-4 margin-bottom20">
                    <a title="Used <%= bike.MakeName %> <%= bike.ModelName %> bikes In <%= CityName %>" href="<%= Bikewale.Utility.UrlFormatter.ViewMoreUsedBikes(CityId,CityMaskingName,bike.MakeMaskingName,bike.ModelMaskingName) %>"><%= bike.AvailableBikes %> Used <%= bike.MakeName %> <%= bike.ModelName %> bikes</a>
                    <p class="margin-top10">Starting at <span class="bwsprite inr-sm-dark"></span> <%= Bikewale.Utility.Format.FormatPrice(bike.MinimumPrice) %> </p>
                </div>
        <%  }
           %>
    </div>
    <div class="clear"></div>
    <div class="padding-left10">
        <a title="Used <%=MakeName %> bikes in <%= CityName %>" href="<%= Bikewale.Utility.UrlFormatter.ViewMoreUsedBikes(CityId, CityMaskingName, MakeMaskingName) %>">View all used <%=MakeName %> <%= pageHeading %> bikes<span class="bwsprite blue-right-arrow-icon"></span></a>
    </div>

</div>
<% } %>
</div>