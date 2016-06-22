<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.UsedBikes" %> 
   
<%if(showWidget) {%>
<div id="makeUsedBikeContent" class="bw-model-tabs-data margin-right10 margin-left10 padding-top20 padding-bottom20 font14">
    <h2 class="padding-left10 padding-right10">Recently uploaded <%= makeName %> Used bikes</h2>
    <!-- when city is not selected -->
    <div class="grid-12 alpha omega text-black">
    <%if(CityId <= 0) {%>    
        <asp:Repeater runat="server" ID="rptUsedBikeNoCity">
            <ItemTemplate>
                <div class="grid-4 margin-bottom20">
                    <a href="<%# Bikewale.Utility.UrlFormatter.UsedBikesUrlNoCity(Convert.ToString(DataBinder.Eval(Container.DataItem,"MakeMaskingName")), Convert.ToString(DataBinder.Eval(Container.DataItem,"CityMaskingName")), Convert.ToUInt32(DataBinder.Eval(Container.DataItem,"CityId")), MakeId) %>"><%= makeName %> Used bikes in <%# Convert.ToString(DataBinder.Eval(Container.DataItem,"CityName")) %></a>
                    <p class="margin-top10"><%# Bikewale.Utility.Format.FormatPrice(Convert.ToString(DataBinder.Eval(Container.DataItem,"AvailableBikes"))) %> bikes available</p>
                </div>
            </ItemTemplate>
        </asp:Repeater>     

    <%} else if(CityId > 0){ %>
    <!-- when city is selected -->
         <asp:Repeater runat="server" ID="rptRecentUsedBikes">
            <ItemTemplate>
                <div class="grid-4 margin-bottom20">
                    <a href="<%# Bikewale.Utility.UrlFormatter.UsedBikesUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"CityMaskingName")),Convert.ToString(DataBinder.Eval(Container.DataItem,"MakeMaskingName")),Convert.ToString(DataBinder.Eval(Container.DataItem,"ModelMaskingName")),Convert.ToString(DataBinder.Eval(Container.DataItem,"ProfileId"))) %>">
                        <%# String.Format("{0}, {1} {2} {3}",Convert.ToString(DataBinder.Eval(Container.DataItem,"MakeYear")), Convert.ToString(DataBinder.Eval(Container.DataItem,"MakeName")), Convert.ToString(DataBinder.Eval(Container.DataItem,"ModelName")), Convert.ToString(DataBinder.Eval(Container.DataItem,"VersionName")))%>
                    </a>
                    <p class="margin-top10">
                        <span class="fa fa-rupee"></span> 
                        <span><%# Bikewale.Utility.Format.FormatPrice(Convert.ToString(DataBinder.Eval(Container.DataItem,"BikePrice"))) %></span> in 
                        <%# Convert.ToString(DataBinder.Eval(Container.DataItem,"CityName")) %>
                    </p>
                </div>
            </ItemTemplate>
        </asp:Repeater>        
     <%} %>
         <div class="padding-left10">
            <a href="<%= Bikewale.Utility.UrlFormatter.ViewMoreUsedBikes(Convert.ToUInt32(CityId), makeMaskingName, cityMaskingName, MakeId) %>">View all used bikes<span class="bwsprite blue-right-arrow-icon"></span></a>
         </div>
       </div>
    <div class="clear"></div>   
</div>
<%} %>