<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.NewAlternativeBikes" %>
<style type="text/css">
    #modelAlternateBikeContent h2{font-size:18px; color:#1a1a1a; font-weight:bold; margin-bottom:15px;}#modelAlternateBikeContent h3 { font-size:14px; color:#4d5057; margin-bottom:12px; }.margin-bottom12 { margin-bottom:12px; }#modelAlternateBikeContent .jcarousel { width:934px; left:20px; padding:0; }#modelAlternateBikeContent .jcarousel li { width:292px; height:auto; margin-right:28px; }#modelAlternateBikeContent h3 a.text-black { display:block; } #modelAlternateBikeContent .jcarousel-control-left { left:0; top:13%; }#modelAlternateBikeContent .jcarousel-control-right { right:0; top:13%; }#modelAlternateBikeContent .jcarousel-control-prev, #modelAlternateBikeContent .jcarousel-control-next {width: 36px;height: 68px;} #modelAlternateBikeContent .jcarousel-control-prev:hover { background-position: -36px -134px}#modelAlternateBikeContent .jcarousel-control-next:hover {background-position: -63px -134px}#modelAlternateBikeContent .jcarousel-control-prev {background-position: -36px -79px;}#modelAlternateBikeContent .jcarousel-control-next {background-position: -63px -79px;}#modelAlternateBikeContent .jcarousel-control-prev.inactive {background-position: -36px -24px;}#modelAlternateBikeContent .jcarousel-control-next.inactive {background-position: -63px -24px;}.model-jcarousel-image-preview { width:292px; height:164px; display:table; text-align:center;}.model-jcarousel-image-preview a { width:292px; height:164px; display:block; background: #fff url('http://imgd1.aeplcdn.com/0x0/bw/static/sprites/d/loader.gif') no-repeat center center;}.model-jcarousel-image-preview a img { width:100%; height:164px; }.inr-md-lg { width:12px; height:17px; background-position:-64px -515px; }
</style>
<!-- Alternative Bikes Starts here-->    
<div id="modelAlternateBikeContent" class="bw-model-tabs-data padding-top20 padding-bottom20 font14">
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
                            <div class="margin-bottom15">
                                <span class="bwsprite inr-md-lg"></span>&nbsp;<span class="font22 text-bold"><%# Bikewale.Utility.Format.FormatPrice(DataBinder.Eval(Container.DataItem, "VersionPrice").ToString()) %></span>
                            </div>
                            <a href="javascript:void(0)" pqsourceid="<%= PQSourceId %>" modelid="<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "modelBase.ModelId")) %>" class="<%# (Convert.ToString(DataBinder.Eval(Container.DataItem, "VersionPrice"))!="0")?string.Empty:"hide" %> btn btn-sm btn-grey font14 fillPopupData" rel="nofollow">Check on-road price</a>
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

