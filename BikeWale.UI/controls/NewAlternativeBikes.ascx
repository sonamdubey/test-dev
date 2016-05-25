<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.NewAlternativeBikes" %>

<!-- Alternative Bikes Starts here-->    
<div class="margin-top20 margin-right10 margin-left10 border-solid-top"></div>
<div id="modelAlternateBikeContent" class="bw-model-tabs-data padding-top20 font14">
    <h2 class="padding-left20 padding-right20"><%=WidgetTitle %> Alternate bikes</h2>
    <div class="jcarousel-wrapper">
        <div class="jcarousel">
            <ul>
                <asp:Repeater ID="rptAlternateBikes" runat="server">
                    <ItemTemplate>
                        <li>
                            <div class="model-jcarousel-image-preview margin-bottom15">
                                <a href="<%# Bikewale.Utility.UrlFormatter.BikePageUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"makeBase.MaskingName")),Convert.ToString(DataBinder.Eval(Container.DataItem,"modelBase.MaskingName"))) %>">
                                    <img class="lazy" data-original="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImagePath").ToString(),DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._310x174) %>" title="<%# DataBinder.Eval(Container.DataItem, "makeBase.MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "modelBase.ModelName").ToString() %>" alt="<%# DataBinder.Eval(Container.DataItem, "makeBase.MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "modelBase.ModelName").ToString() %>" src="" border="0">
                                </a>
                            </div>
                            <h3><a href="<%# Bikewale.Utility.UrlFormatter.BikePageUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"makeBase.MaskingName")),Convert.ToString(DataBinder.Eval(Container.DataItem,"modelBase.MaskingName"))) %>" class="text-black">
                                <%# DataBinder.Eval(Container.DataItem, "makeBase.MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "modelBase.ModelName").ToString() %>
                            </a></h3>
                            <p class="text-xt-light-grey margin-bottom12"><%# Bikewale.Utility.FormatMinSpecs.GetMinSpecs(Convert.ToString(DataBinder.Eval(Container.DataItem, "Displacement")),Convert.ToString(DataBinder.Eval(Container.DataItem, "FuelEfficiencyOverall")),Convert.ToString(DataBinder.Eval(Container.DataItem, "MaxPower"))) %></p>
                            <p class="text-light-grey margin-bottom10">Ex-showroom, Mumbai</p>
                            <div class="font20 margin-bottom15">
                                <span class="fa fa-rupee"></span><span class="font22 text-bold"><%# Bikewale.Utility.Format.FormatPrice(DataBinder.Eval(Container.DataItem, "VersionPrice").ToString()) %></span>
                            </div>
                            <a href="javascript:void(0)" pqsourceid="<%= PQSourceId %>" modelid="<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "modelBase.ModelId")) %>" class="<%# (Convert.ToString(DataBinder.Eval(Container.DataItem, "VersionPrice"))!="0")?string.Empty:"hide" %> btn btn-sm btn-grey font14 margin-top10 fillPopupData">Check On-Road Price</a>
                        </li>
                    </ItemTemplate>
                </asp:Repeater>

            </ul>
        </div>
        <span class="jcarousel-control-left"><a href="#" class="bwsprite jcarousel-control-prev inactive" rel="nofollow"></a></span>
        <span class="jcarousel-control-right"><a href="#" class="bwsprite jcarousel-control-next" rel="nofollow"></a></span>
    </div>
</div>
<!--- Alternative Bikes Ends Here-->
