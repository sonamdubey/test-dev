﻿<%@ Control Language="C#" AutoEventWireup="false" EnableViewState="false"  Inherits="Bikewale.Mobile.Controls.UsedPopularModels" %>
<% if(FetchedRecordsCount>0){ %>
<div id="makeUsedBikeContent" class="bw-model-tabs-data margin-right10 margin-left10 padding-top20 padding-bottom20 font14">
 <h2 class="padding-left10 padding-right10">Recently uploaded Used <%= MakeName %> bikes</h2>
    <div class="grid-12 alpha omega text-black">
    <%
         foreach (var bike in UsedBikeModelInCityList)
           { %>   

                <div class="grid-12 margin-bottom20">
                    <a title="Used <%= bike.MakeName %> <%= bike.ModelName %> bikes in <%= CityName %>" href="/m<%= Bikewale.Utility.UrlFormatter.ViewMoreUsedBikes(CityId,CityMaskingName,bike.MakeMaskingName,bike.ModelMaskingName) %>"><%= bike.AvailableBikes %> Used <%= bike.MakeName %> <%= bike.ModelName %>  <%= bike.AvailableBikes > 1 ? "bikes":"bike" %></a>
                    <p class="margin-top10">Starting at <span class="bwmsprite inr-xxsm-icon"></span><%= Bikewale.Utility.Format.FormatPrice(bike.MinimumPrice) %> </p>
                </div>
     <%       
      } %>
         <div class="padding-left10">
            <a href="/m<%= Bikewale.Utility.UrlFormatter.ViewMoreUsedBikes(CityId, CityMaskingName,MakeMaskingName) %>" title="Used <%=MakeName %> bikes in <%=CityName %>">View all used <%=MakeName %> bikes<span class="bwmsprite blue-right-arrow-icon"></span></a>
         </div>
       </div>
    <div class="clear"></div>   
</div>
<% } %>