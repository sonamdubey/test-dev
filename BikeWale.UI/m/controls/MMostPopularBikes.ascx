<%@ Control Language="C#" AutoEventWireup="False" Inherits="Bikewale.Mobile.controls.MMostPopularBikes" %>


<!-- Most Popular Bikes Starts here-->
<div class="bw-tabs-data " id="mctrlMostPopularBikes" >     
    <asp:Repeater ID="rptMostPopularBikes" runat="server">
        <HeaderTemplate>
            <div class="jcarousel-wrapper discover-bike-carousel">
                <div class="jcarousel">
                    <ul>
        </HeaderTemplate>
        <ItemTemplate>
            <li class="card">
                <div class="front">
                <div class="contentWrapper">
                    <div class="imageWrapper">
                        <a href='/m<%# Bikewale.Utility.UrlFormatter.BikePageUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"objMake.MaskingName")),Convert.ToString(DataBinder.Eval(Container.DataItem,"objModel.MaskingName"))) %>'>
                            <img class="lazy" src="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImagePath").ToString(),DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._310x174) %>" title="<%# DataBinder.Eval(Container.DataItem, "objMake.MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "objModel.ModelName").ToString() %>"  alt="<%# DataBinder.Eval(Container.DataItem, "objMake.MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "objModel.ModelName").ToString() %>">
                        </a>
                    </div>
                    <div class="bikeDescWrapper">
                        <div class="bikeTitle margin-bottom10">
                            <h3><a href='/m<%# Bikewale.Utility.UrlFormatter.BikePageUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"objMake.MaskingName")),Convert.ToString(DataBinder.Eval(Container.DataItem,"objModel.MaskingName"))) %>' title="<%# DataBinder.Eval(Container.DataItem, "objMake.MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "objModel.ModelName").ToString() %>"><%# DataBinder.Eval(Container.DataItem, "objMake.MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "objModel.ModelName").ToString() %></a></h3>
                        </div>
                        <div class="margin-bottom10 font20">
                            <span class="fa fa-rupee"></span>
                            <span class="font22"><%# Bikewale.Utility.Format.FormatPrice(DataBinder.Eval(Container.DataItem, "VersionPrice").ToString()) %></span><span class="font16"> onwards</span>
                        </div>
                        <div class="font12 text-light-grey margin-bottom10">Ex-showroom, Delhi</div>
                        <div class="font14 margin-bottom10">
                            <%# Bikewale.Utility.FormatMinSpecs.GetMinSpecs(Convert.ToString(DataBinder.Eval(Container.DataItem, "Specs.Displacement")),Convert.ToString(DataBinder.Eval(Container.DataItem, "Specs.FuelEfficiencyOverall")),Convert.ToString(DataBinder.Eval(Container.DataItem, "Specs.MaxPower")),Convert.ToString(DataBinder.Eval(Container.DataItem, "Specs.MaximumTorque"))) %>                             
                        </div>
                        <div class="padding-top5 clear">
                            <div class="grid-12 alpha <%# Convert.ToString(DataBinder.Eval(Container.DataItem,"ReviewCount")) == "0" ? "" : "hide" %>">
                                <div class="padding-left5 padding-right5 ">                                                                
                                    <div>
                                         <span class="font16 text-light-grey">Not rated yet  </span>
                                    </div>
                                </div>
                            </div>
                            <div class="grid-6 alpha">
                                <div class="padding-left5 padding-right5 <%# Convert.ToString(DataBinder.Eval(Container.DataItem,"ReviewCount")) != "0" ? "" : "hide" %>">                                                                
                                    <div>
                                        <span class="margin-bottom10 ">
                                           <%# Bikewale.Utility.ReviewsRating.GetRateImage(Convert.ToDouble(DataBinder.Eval(Container.DataItem,"ModelRating"))) %>
                                        </span>
                                    </div>
                                </div>
                            </div>
                            <div class="grid-6 omega border-left1">
                                <div class="padding-left5 padding-right5 <%# Convert.ToString(DataBinder.Eval(Container.DataItem,"ReviewCount")) != "0" ? "" : "hide" %>">
                                    <span class="font16 text-light-grey"><%# DataBinder.Eval(Container.DataItem, "ReviewCount").ToString() %>  Reviews</span>
                                </div>
                            </div>
                            <div class="clear"></div>
                            <a href="javascript:void(0)" class="btn btn-sm btn-white margin-top10">Get on road price</a>
                        </div> 
                    </div>
                </div>
                </div>
            </li>
        </ItemTemplate>
        <FooterTemplate>
            </ul>
              </div>
                <span class="jcarousel-control-left"><a href="javascript:void(0)" class="bwmsprite jcarousel-control-prev"></a></span>
                <span class="jcarousel-control-right"><a href="javascript:void(0)" class="bwmsprite jcarousel-control-next"></a></span>
                <p class="text-center jcarousel-pagination"></p>
             </div>  
        </FooterTemplate>
    </asp:Repeater>
</div> <!--- New Launched Bikes Ends Here-->