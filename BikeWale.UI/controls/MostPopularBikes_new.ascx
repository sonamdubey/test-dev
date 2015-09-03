<%@ Control Language="C#" AutoEventWireup="False"  Inherits="Bikewale.controls.MostPopularBikes_new" %>

<!-- Most Popular Bikes Starts here-->
<div class="bw-tabs-data" id="ctrlMostPopularBikes" >     
    <asp:Repeater ID="rptMostPopularBikes" runat="server">
        <HeaderTemplate>
            <div class="jcarousel-wrapper discover-bike-carousel">
             <div class="jcarousel" data-jcarousel="true">
                <ul style="left: 0px; top: 0px;">
        </HeaderTemplate>
        <ItemTemplate>
            <li class="front">
                <div class="contentWrapper">
                    <div class="imageWrapper">
                        <a href='<%# Bikewale.Utility.UrlFormatter.BikePageUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"objMake.MaskingName")),Convert.ToString(DataBinder.Eval(Container.DataItem,"objModel.MaskingName"))) %>'>
                            <img class="lazy" src="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImagePath").ToString(),DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._310x174) %>" title="<%# DataBinder.Eval(Container.DataItem, "objMake.MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "objModel.ModelName").ToString() %>"  alt="<%# DataBinder.Eval(Container.DataItem, "objMake.MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "objModel.ModelName").ToString() %>">
                        </a>
                    </div>
                    <div class="bikeDescWrapper">
                        <div class="bikeTitle margin-bottom10">
                            <h3><a href='<%# Bikewale.Utility.UrlFormatter.BikePageUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"objMake.MaskingName")),Convert.ToString(DataBinder.Eval(Container.DataItem,"objModel.MaskingName"))) %>' title="<%# DataBinder.Eval(Container.DataItem, "objMake.MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "objModel.ModelName").ToString() %>"><%# DataBinder.Eval(Container.DataItem, "objMake.MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "objModel.ModelName").ToString() %></a></h3>
                        </div>
                        <div class="margin-bottom10 font20">
                            <span class="fa fa-rupee"></span>
                            <span class="font22"><%# Bikewale.Utility.Format.FormatPrice(DataBinder.Eval(Container.DataItem, "VersionPrice").ToString()) %></span><span class="font16"> onwards</span>
                        </div>
                        <div class="font12 text-light-grey margin-bottom10">Ex-showroom, Delhi</div>
                        <div class="font14 margin-bottom10">
                            <%# Bikewale.Utility.FormatMinSpecs.GetMinSpecs(Convert.ToString(DataBinder.Eval(Container.DataItem, "Specs.Displacement")),Convert.ToString(DataBinder.Eval(Container.DataItem, "Specs.FuelEfficiencyOverall")),Convert.ToString(DataBinder.Eval(Container.DataItem, "Specs.MaxPower")),Convert.ToString(DataBinder.Eval(Container.DataItem, "Specs.MaximumTorque"))) %>                             
                        </div>
                        <div class="leftfloat">
                            <p class=" inline-block border-solid-right padding-right10 <%# Convert.ToString(DataBinder.Eval(Container.DataItem,"ReviewCount")) != "0" ? "" : "hide" %>">
                                <%# Bikewale.Utility.ReviewsRating.GetRateImage(Convert.ToDouble(DataBinder.Eval(Container.DataItem,"ModelRating"))) %>
                            </p>
                        </div>
                       <div class="leftfloat margin-left10 font16 text-light-grey <%# Convert.ToString(DataBinder.Eval(Container.DataItem,"ReviewCount")) != "0" ? "" : "hide" %>">
                          <span><%# DataBinder.Eval(Container.DataItem, "ReviewCount").ToString() %> Reviews</span>
                        </div>

                        <div class="leftfloat font14 text-light-grey <%# Convert.ToString(DataBinder.Eval(Container.DataItem,"ReviewCount")) == "0" ? "" : "hide" %>">
                         <span class="border-solid-right">Not rated yet  </span><a href="/content/userreviews/writereviews.aspx?bikem=<%# DataBinder.Eval(Container.DataItem,"objModel.ModelId") %>"><span class="margin-left10">Write a review</span></a>
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
            <span class="jcarousel-control-left"><a href="#" class="bwsprite jcarousel-control-prev inactive" data-jcarouselcontrol="true"></a></span>
            <span class="jcarousel-control-right"><a href="#" class="bwsprite jcarousel-control-next " data-jcarouselcontrol="true"></a></span>
        </div> 
        </FooterTemplate>
    </asp:Repeater>
</div> <!--- New Launched Bikes Ends Here-->