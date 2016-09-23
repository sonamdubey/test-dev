<%@ Control Language="C#" AutoEventWireup="false"  Inherits="Bikewale.Mobile.Controls.UsedBikes" %>

<div id="makeUsedBikeContent" class="bw-model-tabs-data margin-right10 margin-left10 padding-top20 padding-bottom20 font14">
 
    <%--<h2 class="padding-left10 padding-right10"><%=header %> Recently uploaded Used <%=ModelId > 0 ? String.Format("{0}", modelName) : makeName%> bikes <%=CityId > 0 ? String.Format("in {0}", cityName) : "" %></h2>--%>
  <h2 class="padding-left10 padding-right10"><%=header%></h2>
    <!-- when city is not selected -->
    <div class="grid-12 alpha omega text-black">
    <%if(CityId <= 0) {%>    
        <asp:Repeater runat="server" ID="rptUsedBikeNoCity">
            <ItemTemplate>
                <div class="grid-12 margin-bottom20">
                    <a href="/m<%# Bikewale.Utility.UrlFormatter.UsedBikesUrlNoCity(Convert.ToString(DataBinder.Eval(Container.DataItem,"MakeMaskingName")), Convert.ToString(DataBinder.Eval(Container.DataItem,"ModelMaskingName")),Convert.ToString(DataBinder.Eval(Container.DataItem,"CityMaskingName"))) %>" title="Used <%=makeName %> <% =ModelId>0?String.Format("{0} ",modelName) :"" %>bikes in <%# Convert.ToString(DataBinder.Eval(Container.DataItem,"CityName")) %>">Used <%=makeName %> <% =ModelId>0?String.Format("{0} ",modelName) :"" %>bikes in <%# Convert.ToString(DataBinder.Eval(Container.DataItem,"CityName")) %></a>
                    <p class="margin-top10"><%# Bikewale.Utility.Format.FormatPrice(Convert.ToString(DataBinder.Eval(Container.DataItem,"AvailableBikes"))) %> <%# Convert.ToString(DataBinder.Eval(Container.DataItem,"AvailableBikes")) == "1" ? "bike" : "bikes" %> available</p>
                </div>
            </ItemTemplate>
        </asp:Repeater>     

    <%} else { %>
    <!-- when city is selected -->
         <asp:Repeater runat="server" ID="rptRecentUsedBikes">
            <ItemTemplate>
                <div class="grid-12 margin-bottom20">
                    <a href="/m<%# Bikewale.Utility.UrlFormatter.UsedBikesUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"CityMaskingName")),Convert.ToString(DataBinder.Eval(Container.DataItem,"MakeMaskingName")),Convert.ToString(DataBinder.Eval(Container.DataItem,"ModelMaskingName")),Convert.ToString(DataBinder.Eval(Container.DataItem,"ProfileId"))) %>" title="Used <%=makeName %> <%# Convert.ToString(DataBinder.Eval(Container.DataItem,"ModelName")) %> bikes in <%# Convert.ToString(DataBinder.Eval(Container.DataItem,"CityName")) %>">
                        <%# String.Format("{0}, {1} {2} {3}",Convert.ToString(DataBinder.Eval(Container.DataItem,"MakeYear")), Convert.ToString(DataBinder.Eval(Container.DataItem,"MakeName")), Convert.ToString(DataBinder.Eval(Container.DataItem,"ModelName")), Convert.ToString(DataBinder.Eval(Container.DataItem,"VersionName")))%>
                    </a>
                    <p class="margin-top10">
                       <span class="bwmsprite inr-xsm-icon"></span>
                        <span><%# Bikewale.Utility.Format.FormatPrice(Convert.ToString(DataBinder.Eval(Container.DataItem,"BikePrice"))) %></span>
                    </p>
                </div>
            </ItemTemplate>
        </asp:Repeater>        
     <%} %>
         <div class="padding-left10">
            <a href="/m<%= Bikewale.Utility.UrlFormatter.ViewMoreUsedBikes(Convert.ToUInt32(CityId), cityMaskingName,makeMaskingName,modelMaskingName) %>" title="Used <%=pageHeading %> bikes <%=CityId > 0 ? String.Format("in {0}", cityName) : "" %>">View all used bikes<span class="bwmsprite blue-right-arrow-icon"></span></a>
         </div>
       </div>
    <div class="clear"></div>   
</div>
