<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.UsedPopularModels" %>

<div id="makeUsedBikeContent" class="bw-model-tabs-data margin-right10 margin-left10 padding-top20 padding-bottom20 font14">

    <h2 class="padding-left10 padding-right10">Recently uploaded Used <%= pageHeading %> bikes <%=CityId > 0 ? String.Format("in {0}", cityName) : "" %></h2>
    <!-- when city is not selected -->
    <div class="grid-12 alpha omega text-black">
        <%if (CityId <= 0)
          {%>
        <%--<asp:Repeater runat="server" ID="rptUsedBikeNoCity">
            <ItemTemplate> --%>
        <% foreach (var bike in UsedBikeModelInCityList)
           { %>
                <div class="grid-4 margin-bottom20">
                    <a title="Used <%= bike.MakeName %> <%= bike.ModelName %> bikes In India" href="">Used <%=pageHeading%> bikes in India</a>
                    <p class="margin-top10"><%= bike.AvailableBikes %>bikes available</p>
                </div>
        <% } %>
            <%--</ItemTemplate>
        </asp:Repeater>--%>
    </div>
    <div class="clear"></div>
    <div class="padding-left10">
        <a title="Used <%=pageHeading %> bikes in India" href="<%= Bikewale.Utility.UrlFormatter.ViewMoreUsedBikes(Convert.ToUInt32(CityId), cityMaskingName, makeMaskingName, modelMaskingName) %>">View all used <%= pageHeading %> bikes<span class="bwsprite blue-right-arrow-icon"></span></a>
    </div>
    <%}
          else
          { %>
    <!-- when city is selected -->
    <%--<asp:Repeater runat="server" ID="rptRecentUsedBikes">
        <ItemTemplate>
            <div class="grid-4 margin-bottom20">
                <a href="<%#Bikewale.Utility.UrlFormatter.UsedBikesUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"CityMaskingName")),Convert.ToString(DataBinder.Eval(Container.DataItem,"MakeMaskingName")),Convert.ToString(DataBinder.Eval(Container.DataItem,"ModelMaskingName")),Convert.ToString(DataBinder.Eval(Container.DataItem,"ProfileId")))%>"
                    title="<%#String.Format("{0}, {1} {2} {3}",Convert.ToString(DataBinder.Eval(Container.DataItem,"MakeYear")), Convert.ToString(DataBinder.Eval(Container.DataItem,"MakeName")), Convert.ToString(DataBinder.Eval(Container.DataItem,"ModelName")), Convert.ToString(DataBinder.Eval(Container.DataItem,"VersionName")))%>">
                    <%# String.Format("{0}, {1} {2} {3}",Convert.ToString(DataBinder.Eval(Container.DataItem,"MakeYear")), Convert.ToString(DataBinder.Eval(Container.DataItem,"MakeName")), Convert.ToString(DataBinder.Eval(Container.DataItem,"ModelName")), Convert.ToString(DataBinder.Eval(Container.DataItem,"VersionName")))%>   
                </a>
                <p class="margin-top10">
                    <span class="bwsprite inr-sm-dark"></span>
                    <span><%# Bikewale.Utility.Format.FormatPrice(Convert.ToString(DataBinder.Eval(Container.DataItem,"BikePrice"))) %></span>
                </p>
            </div>
        </ItemTemplate>
    </asp:Repeater>--%>
</div>
<div class="clear"></div>
<div class="padding-left10">
    <a title="Used <%=pageHeading%> bikes in <%=cityName%>" href="<%=Bikewale.Utility.UrlFormatter.ViewMoreUsedBikes(Convert.ToUInt32(CityId), cityMaskingName, makeMaskingName, modelMaskingName)%>">View all used <%= pageHeading %> bikes<span class="bwsprite blue-right-arrow-icon"></span></a>
</div>
<%} %>

</div>