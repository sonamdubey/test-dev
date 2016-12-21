<%@ Control Language="C#" AutoEventWireup="False" Inherits="Bikewale.Controls.MostPopularBikes_new" %>
<% if (mostPopular==true)
                           { %>
<!-- Most Popular Bikes Starts here-->
<asp:Repeater ID="rptMostPopularBikes" runat="server">
    <ItemTemplate>
        <li>
            <a href="<%# Bikewale.Utility.UrlFormatter.BikePageUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"objMake.MaskingName")),Convert.ToString(DataBinder.Eval(Container.DataItem,"objModel.MaskingName"))) %>" title="<%# DataBinder.Eval(Container.DataItem, "objMake.MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "objModel.ModelName").ToString() %>" class="jcarousel-card">
                <div class="model-jcarousel-image-preview">
                    <span class="card-image-block">
                        <img class="lazy" data-original="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImagePath").ToString(),DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._310x174) %>" alt="<%# DataBinder.Eval(Container.DataItem, "objMake.MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "objModel.ModelName").ToString() %>" src="" border="0">
                    </span>
                </div>
                <div class="card-desc-block">
                    <h3 class="bikeTitle"><%# DataBinder.Eval(Container.DataItem, "objMake.MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "objModel.ModelName").ToString() %></h3>
                    <p class="text-xt-light-grey font14 margin-bottom10">
                        <%# Bikewale.Utility.FormatMinSpecs.GetMinSpecs(Convert.ToString(DataBinder.Eval(Container.DataItem, "Specs.Displacement")),Convert.ToString(DataBinder.Eval(Container.DataItem, "Specs.FuelEfficiencyOverall")),Convert.ToString(DataBinder.Eval(Container.DataItem, "Specs.MaxPower"))) %>
                    </p>
                    <p class="font14 text-light-grey margin-bottom5">Ex-showroom, <%=ConfigurationManager.AppSettings["defaultName"].ToString() %></p>
                    <p class="text-bold text-default">
                        <%# ShowEstimatedPrice(DataBinder.Eval(Container.DataItem, "VersionPrice")) %>
                    </p>
                </div>
            </a>
            <div class="margin-left20 margin-bottom20">
                <a href="javascript:void(0);" data-pagecatid="<%= PageId %>" data-pqsourceid="<%= PQSourceId%>" data-makename="<%# Convert.ToString(DataBinder.Eval(Container.DataItem,"objMake.MakeName")) %>" data-modelname="<%# Convert.ToString(DataBinder.Eval(Container.DataItem,"objModel.ModelName")) %>" data-modelid="<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "objModel.ModelId")) %>" class="btn btn-grey btn-sm font14 <%# (Convert.ToString(DataBinder.Eval(Container.DataItem, "VersionPrice"))!="0")?"":"hide" %> getquotation" rel="nofollow">Check on-road price</a>
            </div>
        </li>
    </ItemTemplate>
</asp:Repeater>
<%} %>
<!--- Most Popular Bikes Ends Here-->

          <% if (mostPopularByMake)
                           { %>
            <div class="jcarousel-wrapper inner-content-carousel padding-bottom20">
                <div class="jcarousel">

                    <ul>
                        <asp:Repeater ID="rptPopoularBikeMake" runat="server">
                            <ItemTemplate>
                                <li>
                                    <a href="<%# Bikewale.Utility.UrlFormatter.BikePageUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"objMake.MaskingName")),Convert.ToString(DataBinder.Eval(Container.DataItem,"objModel.MaskingName"))) %>" title="<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "objMake.MakeName"))+" "+Convert.ToString( DataBinder.Eval(Container.DataItem, "objModel.ModelName"))%>" class="jcarousel-card">
                                        <div class="model-jcarousel-image-preview">
                                            <span class="card-image-block">
                                                <img class="lazy" data-original="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImagePath").ToString(),DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._310x174) %>" alt="<%# DataBinder.Eval(Container.DataItem, "objMake.MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "objModel.ModelName").ToString() %>" src="" border="0">
                                            </span>
                                        </div>
                                        <div class="card-desc-block">
                                            <h3 class="bikeTitle"><%# Convert.ToString(DataBinder.Eval(Container.DataItem, "objMake.MakeName"))+" "+Convert.ToString( DataBinder.Eval(Container.DataItem, "objModel.ModelName"))%></h3>
                                            <p class="text-xt-light-grey font14 margin-bottom10">
                                                <%# Bikewale.Utility.FormatMinSpecs.GetMinSpecs(Convert.ToString(DataBinder.Eval(Container.DataItem, "Specs.Displacement")),Convert.ToString(DataBinder.Eval(Container.DataItem, "Specs.FuelEfficiencyOverall")),Convert.ToString(DataBinder.Eval(Container.DataItem, "Specs.MaxPower")),Convert.ToString(DataBinder.Eval(Container.DataItem, "Specs.KerbWeight"))) %>
                                            </p>
                                            <p class="font14 text-light-grey margin-bottom5">Ex-showroom, <%# Convert.ToString(DataBinder.Eval(Container.DataItem, "CityName"))%></p>
                                            <p class="text-bold text-default">
                                                <%# ShowEstimatedPrice(DataBinder.Eval(Container.DataItem, "VersionPrice")) %>
                                            </p>
                                        </div>
                                    </a>
                                    <div class="margin-left20 margin-bottom20">
                                        <a href="<%# Bikewale.Utility.UrlFormatter.PriceInCityUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"objMake.MaskingName")),Convert.ToString(DataBinder.Eval(Container.DataItem,"objModel.MaskingName")), Convert.ToString(DataBinder.Eval(Container.DataItem, "CityMaskingName"))) %>" class="btn btn-white btn-truncate font14 btn-size-2" title="<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "objMake.MakeName"))+" "+Convert.ToString( DataBinder.Eval(Container.DataItem, "objModel.ModelName"))%> On-road price in <%#Convert.ToString(DataBinder.Eval(Container.DataItem,"CityName")) %>" >On-road price in <%#Convert.ToString(DataBinder.Eval(Container.DataItem,"CityName")) %></a>
                                    </div>
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>

                </div>

          <span class="jcarousel-control-left"><a href="#" class="bwsprite jcarousel-control-prev inactive" rel="nofollow"></a></span>
            <span class="jcarousel-control-right"><a href="#" class="bwsprite jcarousel-control-next" rel="nofollow"></a></span>
            </div>

            <%} %>
       