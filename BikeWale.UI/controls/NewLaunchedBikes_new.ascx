<%@ Control Language="C#" AutoEventWireup="true" Inherits="Bikewale.controls.NewLaunchedBikes_new" %>
<!-- New Launched Bikes Starts here-->
<div class="bw-tabs-data hide" id="ctrlNewLaunchedBikes" >     
    <asp:Repeater ID="rptNewLaunchedBikes" runat="server">
        <HeaderTemplate>
            <div class="jcarousel-wrapper discover-bike-carousel">
             <div class="jcarousel" data-jcarousel="true">
                <ul style="left: -954px; top: 0px;">
        </HeaderTemplate>
        <ItemTemplate>
            <li class="front">
                <div class="contentWrapper">
                    <div class="imageWrapper">
                        <a href='<%# Bikewale.Utility.UrlFormatter.BikePageUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"MakeBase.MaskingName")),Convert.ToString(DataBinder.Eval(Container.DataItem,"MaskingName"))) %>'>
                            <img class="lazy" src="<%# Bikewale.Utility.Image.GetPathToShowImages(Convert.ToString(DataBinder.Eval(Container.DataItem, "OriginalImagePath")),Convert.ToString(DataBinder.Eval(Container.DataItem, "HostUrl")),Bikewale.Utility.ImageSize._310x174) %>" title="<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "MakeBase.MakeName")) + " " + Convert.ToString(DataBinder.Eval(Container.DataItem, "ModelName")) %>"  alt="<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "MakeBase.MakeName")) + " " +Convert.ToString(DataBinder.Eval(Container.DataItem, "ModelName")) %>">
                        </a>
                    </div>
                    <div class="bikeDescWrapper">
                        <div class="bikeTitle margin-bottom10">
                            <h3><a href='<%# Bikewale.Utility.UrlFormatter.BikePageUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"MakeBase.MaskingName")),Convert.ToString(DataBinder.Eval(Container.DataItem,"MaskingName"))) %>' title="<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "MakeBase.MakeName")) + " " +Convert.ToString(DataBinder.Eval(Container.DataItem, "ModelName"))%>"><%# Convert.ToString(DataBinder.Eval(Container.DataItem, "MakeBase.MakeName")) + " " +Convert.ToString(DataBinder.Eval(Container.DataItem, "ModelName"))%></a></h3>
                        </div>
                        <div class="margin-bottom10 font20">
                            <span class="fa fa-rupee"></span>
                            <span class="font22"><%# Bikewale.Utility.Format.FormatPrice(Convert.ToString(DataBinder.Eval(Container.DataItem, "MinPrice"))) %></span><span class="font16"> onwards</span>
                        </div>
                        <div class="font12 text-light-grey margin-bottom10">Ex-showroom, Delhi</div>
                        <div class="font14 margin-bottom10">
                            <%# Bikewale.Utility.FormatMinSpecs.GetMinSpecs(Convert.ToString(DataBinder.Eval(Container.DataItem, "Specs.Displacement")),Convert.ToString(DataBinder.Eval(Container.DataItem, "Specs.FuelEfficiencyOverall")),Convert.ToString(DataBinder.Eval(Container.DataItem, "Specs.MaxPower")),Convert.ToString(DataBinder.Eval(Container.DataItem, "Specs.MaximumTorque"))) %>                             
                        </div>
                        <div class="leftfloat">
                            <p class=" inline-block border-solid-right padding-right10  <%# Convert.ToString(DataBinder.Eval(Container.DataItem,"ReviewCount")) != "0" ? "" : "hide" %>"">
                                <%# Bikewale.Utility.ReviewsRating.GetRateImage(Convert.ToDouble(DataBinder.Eval(Container.DataItem,"ReviewRate"))) %>
                            </p>
                        </div>
                        <div class=" leftfloat font16 text-light-grey <%# Convert.ToString(DataBinder.Eval(Container.DataItem,"ReviewCount")) == "0" ? "" : "hide" %>">
                         <p class="inline-block border-solid-right padding-right10">
                            Not rated yet
                         </p>
                        </div>

                        <div class="leftfloat margin-left10 font16 text-light-grey">
                            <span><%# Bikewale.Utility.FormatDate.GetDDMMYYYY(Convert.ToString(DataBinder.Eval(Container.DataItem, "LaunchDate"))) %></span>
                        </div>
                        <div class="clear"></div>
                        <a href="#" class="btn btn-grey margin-top10">Get on road price</a>
                    </div>
                </div>
            </li>
        </ItemTemplate>
        <FooterTemplate>
            </ul>
            </div>
            <span class="jcarousel-control-left"><a href="#" class="bwsprite jcarousel-control-prev" data-jcarouselcontrol="true"></a></span>
            <span class="jcarousel-control-right"><a href="#" class="bwsprite jcarousel-control-next inactive" data-jcarouselcontrol="true"></a></span>
        </div> 
        </FooterTemplate>
    </asp:Repeater>
</div> <!--- New Launched Bikes Ends Here-->

