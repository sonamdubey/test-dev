<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.UsedBikes" %>

<div id="makeUsedBikeContent" class="bw-model-tabs-data margin-right10 margin-left10 padding-top20 padding-bottom20 font14">

    <h2 class="padding-left10 padding-right10">Used <%= pageHeading %> bikes <%=CityId > 0 ? String.Format("in {0}", cityName) : " in India" %></h2>
    <!-- when city is not selected -->
    <div class="grid-12 alpha omega text-black">
        <%if (CityId <= 0)
          {%>
        <asp:Repeater runat="server" ID="rptUsedBikeNoCity">
            <ItemTemplate>
                <div class="grid-4 margin-bottom20">
                    <a title="Used <%=pageHeading%> bikes in <%# Convert.ToString(DataBinder.Eval(Container.DataItem,"CityName"))%>" href="<%#Bikewale.Utility.UrlFormatter.UsedBikesUrlNoCity(Convert.ToString(DataBinder.Eval(Container.DataItem,"MakeMaskingName")), Convert.ToString(DataBinder.Eval(Container.DataItem,"ModelMaskingName")), Convert.ToString(DataBinder.Eval(Container.DataItem,"CityMaskingName")))%>">Used <%=pageHeading%> bikes in <%#Convert.ToString(DataBinder.Eval(Container.DataItem,"CityName"))%></a>
                    <p class="margin-top10"><%# Bikewale.Utility.Format.FormatPrice(Convert.ToString(DataBinder.Eval(Container.DataItem,"AvailableBikes")))%><%#Convert.ToString(DataBinder.Eval(Container.DataItem,"AvailableBikes")) == "1" ? " bike" : " bikes" %> available</p>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
    <div class="clear"></div>
    <div class="padding-left10 view-all-btn-container">
        <a title="<%=pageHeading %> Used Bikes in India" href="<%= Bikewale.Utility.UrlFormatter.ViewMoreUsedBikes(Convert.ToUInt32(CityId), cityMaskingName, makeMaskingName, modelMaskingName) %>" class="btn view-all-target-btn">View all used bikes<span class="bwsprite teal-right"></span></a>
    </div>
    <%}
          else
          { %>
    <!-- when city is selected -->
    <asp:Repeater runat="server" ID="rptRecentUsedBikes">
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
    </asp:Repeater>
</div>
<div class="clear"></div>
<div class="padding-left10 view-all-btn-container">
    <a title="<%=pageHeading%> Used Bikes in <%=cityName%>" href="<%=Bikewale.Utility.UrlFormatter.ViewMoreUsedBikes(Convert.ToUInt32(CityId), cityMaskingName, makeMaskingName, modelMaskingName)%>" class="btn view-all-target-btn">View all used bikes<span class="bwsprite teal-right"></span></a>
</div>
<%} %>

</div>