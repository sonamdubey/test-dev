<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.NewLaunchedBikes_new" %>
<!-- New Launched Bikes Starts here-->
<div class="jcarousel-wrapper inner-content-carousel carousel-height-360">
    <div class="jcarousel">
        <ul>
            <asp:Repeater ID="rptNewLaunchedBikes" runat="server">
                <ItemTemplate>
                    <li>
                        <a href="<%# Bikewale.Utility.UrlFormatter.BikePageUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"MakeBase.MaskingName")),Convert.ToString(DataBinder.Eval(Container.DataItem,"MaskingName"))) %>" title="<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "MakeBase.MakeName")) + " " + Convert.ToString(DataBinder.Eval(Container.DataItem, "ModelName")) %>" class="jcarousel-card">
                            <div class="model-jcarousel-image-preview">
                                <span class="card-image-block">
                                    <img class="lazy" data-original="<%# Bikewale.Utility.Image.GetPathToShowImages(Convert.ToString(DataBinder.Eval(Container.DataItem, "OriginalImagePath")),Convert.ToString(DataBinder.Eval(Container.DataItem, "HostUrl")),Bikewale.Utility.ImageSize._310x174) %>" alt="<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "MakeBase.MakeName")) + " " + Convert.ToString(DataBinder.Eval(Container.DataItem, "ModelName")) %>" src="" border="0">
                                </span>
                            </div>
                            <div class="card-desc-block">
                                <h3 class="bikeTitle"><%# Convert.ToString(DataBinder.Eval(Container.DataItem, "MakeBase.MakeName")) + " " +Convert.ToString(DataBinder.Eval(Container.DataItem, "ModelName"))%></h3>
                                <p class="text-xt-light-grey font14 margin-bottom10">
                                    <%# Bikewale.Utility.FormatMinSpecs.GetMinSpecs(Convert.ToString(DataBinder.Eval(Container.DataItem, "Specs.Displacement")),Convert.ToString(DataBinder.Eval(Container.DataItem, "Specs.FuelEfficiencyOverall")),Convert.ToString(DataBinder.Eval(Container.DataItem, "Specs.MaxPower")),Convert.ToString(DataBinder.Eval(Container.DataItem, "Specs.KerbWeight"))) %>
                                </p>
                                <p class="font14 text-light-grey margin-bottom5">Ex-showroom, <%=ConfigurationManager.AppSettings["defaultName"].ToString() %></p>
                                <div class="text-default text-bold">
                                    <%# ShowEstimatedPrice(DataBinder.Eval(Container.DataItem, "MinPrice")) %>
                                </div>
                            </div>
                        </a>
                        <div>
                            <a href="#" data-makename="<%# DataBinder.Eval(Container.DataItem,"MakeBase.MakeName").ToString() %>" data-modelname="<%# DataBinder.Eval(Container.DataItem,"ModelName") %>" data-pagecatid="<%= PageId %>" data-pqsourceid="<%= PQSourceId %>" data-modelid="<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "ModelId")) %>" class="btn btn-grey btn-sm font14 margin-left20 margin-bottom20 <%# (Convert.ToString(DataBinder.Eval(Container.DataItem, "MinPrice"))!="0")?"":"hide" %> getquotation">Check on-road price</a>
                        </div>
                    </li>
                </ItemTemplate>
            </asp:Repeater>
        </ul>
    </div>
    <span class="jcarousel-control-left"><a href="#" class="bwsprite jcarousel-control-prev inactive" rel="nofollow"></a></span>
    <span class="jcarousel-control-right"><a href="#" class="bwsprite jcarousel-control-next" rel="nofollow"></a></span>
</div>
<!--- New Launched Bikes Ends Here-->

