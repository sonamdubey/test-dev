<%@ Control Language="C#" AutoEventWireup="false"  Inherits="Bikewale.Mobile.Controls.UsedBikes" %>

<div id="makeUsedBikeContent" class="bw-model-tabs-data margin-right10 margin-left10 padding-top20 padding-bottom20 font14">
 
    <%--<h2 class="padding-left10 padding-right10"><%=header %> Recently uploaded Used <%=ModelId > 0 ? String.Format("{0}", modelName) : makeName%> bikes <%=CityId > 0 ? String.Format("in {0}", cityName) : "" %></h2>--%>
    <h2 class="padding-left10 padding-right10"><%=header%></h2>
    <!-- when city is not selected -->
    <ul class="padding-right10 padding-left10">
    <%if(CityId <= 0) {%>    
        <asp:Repeater runat="server" ID="rptUsedBikeNoCity">
            <ItemTemplate>
                <li class="margin-bottom20">
                    <a href="/m<%# Bikewale.Utility.UrlFormatter.UsedBikesUrlNoCity(Convert.ToString(DataBinder.Eval(Container.DataItem,"MakeMaskingName")), Convert.ToString(DataBinder.Eval(Container.DataItem,"ModelMaskingName")),Convert.ToString(DataBinder.Eval(Container.DataItem,"CityMaskingName"))) %>" title="<%=makeName %><% =ModelId>0?String.Format(" {0} ",modelName) :"" %> Used Bikes in <%# Convert.ToString(DataBinder.Eval(Container.DataItem,"CityName")) %>">Used <%=makeName %><% =ModelId>0?String.Format(" {0} ",modelName) :"" %>bikes in <%# Convert.ToString(DataBinder.Eval(Container.DataItem,"CityName")) %></a>
                    <p class="margin-top5 text-black"><%# Bikewale.Utility.Format.FormatPrice(Convert.ToString(DataBinder.Eval(Container.DataItem,"AvailableBikes"))) %><%# Convert.ToString(DataBinder.Eval(Container.DataItem,"AvailableBikes")) == "1" ? " bike" : " bikes" %> available</p>
                </li>
            </ItemTemplate>
        </asp:Repeater>     

    <%} else { %>
    <!-- when city is selected -->
         <asp:Repeater runat="server" ID="rptRecentUsedBikes">
            <ItemTemplate>
                <li class="margin-bottom20">
                    <a href="/m<%# Bikewale.Utility.UrlFormatter.UsedBikesUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"CityMaskingName")),Convert.ToString(DataBinder.Eval(Container.DataItem,"MakeMaskingName")),Convert.ToString(DataBinder.Eval(Container.DataItem,"ModelMaskingName")),Convert.ToString(DataBinder.Eval(Container.DataItem,"ProfileId"))) %>" title="Used <%# String.Format("{0} {1}",makeName,Convert.ToString(DataBinder.Eval(Container.DataItem,"ModelName"))) %> bikes in <%# Convert.ToString(DataBinder.Eval(Container.DataItem,"CityName")) %>">
                        <%# String.Format("{0}, {1} {2} {3}",Convert.ToString(DataBinder.Eval(Container.DataItem,"MakeYear")), Convert.ToString(DataBinder.Eval(Container.DataItem,"MakeName")), Convert.ToString(DataBinder.Eval(Container.DataItem,"ModelName")), Convert.ToString(DataBinder.Eval(Container.DataItem,"VersionName")))%>
                    </a>
                    <p class="margin-top5 text-black">
                       <span class="bwmsprite inr-xxsm-icon"></span><span><%# Bikewale.Utility.Format.FormatPrice(Convert.ToString(DataBinder.Eval(Container.DataItem,"BikePrice"))) %></span>
                    </p>
                </li>
            </ItemTemplate>
        </asp:Repeater>        
     <%} %>
    </ul>
    <div class="padding-left10 view-all-btn-container">
        <a href="/m<%= Bikewale.Utility.UrlFormatter.ViewMoreUsedBikes(Convert.ToUInt32(CityId), cityMaskingName,makeMaskingName,modelMaskingName) %>" title="<%=pageHeading %> Used Bikes in <%=CityId > 0 ? cityName : "India" %>" class="btn view-all-target-btn">View all used bikes<span class="bwmsprite teal-right"></span></a>
    </div>
    <div class="clear"></div>   
</div>
