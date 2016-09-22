<%@ Control Language="C#" AutoEventWireup="False" Inherits="Bikewale.Controls.MostPopularBikes_new" %>

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
                <a href="javascript:void(0);" pagecatid="<%= PageId %>" pqsourceid="<%= PQSourceId%>" makename="<%# Convert.ToString(DataBinder.Eval(Container.DataItem,"objMake.MakeName")) %>" modelname="<%# Convert.ToString(DataBinder.Eval(Container.DataItem,"objModel.ModelName")) %>" modelid="<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "objModel.ModelId")) %>" class="btn btn-grey btn-sm font14 <%# (Convert.ToString(DataBinder.Eval(Container.DataItem, "VersionPrice"))!="0")?"":"hide" %> fillPopupData" rel="nofollow">Check on-road price</a>
            </div>
        </li>
    </ItemTemplate>
</asp:Repeater>
<!--- Most Popular Bikes Ends Here-->
<div class="container margin-bottom10">
    <div class="grid-12">
        <div class="content-box-shadow">
            <h2 class="font18 padding-18-20">Popular Bajaj bikes in Mumbai</h2>

            <div class="jcarousel-wrapper inner-content-carousel padding-bottom20">
                <div class="jcarousel">

                    <ul>
                        <asp:Repeater ID="rptPopoularBikeMake" runat="server">
                            <ItemTemplate>
                                <li>
                                    <a href="" title="Hero Splendor Pro Classic" class="jcarousel-card">
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
                                        <a href="javascript:void(0)" class="btn btn-white font14 btn-size-2" rel="nofollow">View price in Mumbai</a>
                                    </div>
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>

                </div>

          <span class="jcarousel-control-left"><a href="#" class="bwsprite jcarousel-control-prev inactive" rel="nofollow"></a></span>
            <span class="jcarousel-control-right"><a href="#" class="bwsprite jcarousel-control-next" rel="nofollow"></a></span>
            </div>

            
        </div>
    </div>
    </div>
