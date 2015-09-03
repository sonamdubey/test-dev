<%@ Control Language="C#" AutoEventWireup="False" Inherits="Bikewale.Mobile.controls.MNewLaunchedBikes" %>

<!-- New Launched Bikes Starts here-->
<div class="bw-tabs-data hide" id="mctrlNewLaunchedBikes" >     
    <asp:Repeater ID="rptNewLaunchedBikes" runat="server">
        <HeaderTemplate>
            <div class="jcarousel-wrapper discover-bike-carousel">
                <div class="jcarousel">                    <ul>
        </HeaderTemplate>
        <ItemTemplate>
            <li class="card">
            <div class="front">
                <div class="contentWrapper">
                    <div class="imageWrapper">
                        <a href='/m<%# Bikewale.Utility.UrlFormatter.BikePageUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"MakeBase.MaskingName")),Convert.ToString(DataBinder.Eval(Container.DataItem,"MaskingName"))) %>'>
                            <img class="lazy" src="<%# Bikewale.Utility.Image.GetPathToShowImages(Convert.ToString(DataBinder.Eval(Container.DataItem, "OriginalImagePath")),Convert.ToString(DataBinder.Eval(Container.DataItem, "HostUrl")),Bikewale.Utility.ImageSize._310x174) %>" title="<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "MakeBase.MakeName")) + " " + Convert.ToString(DataBinder.Eval(Container.DataItem, "ModelName")) %>"  alt="<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "MakeBase.MakeName")) + " " +Convert.ToString(DataBinder.Eval(Container.DataItem, "ModelName")) %>">
                        </a>
                    </div>
                    <div class="bikeDescWrapper">
                        <div class="bikeTitle ">
                            <h3><a href='/m<%# Bikewale.Utility.UrlFormatter.BikePageUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"MakeBase.MaskingName")),Convert.ToString(DataBinder.Eval(Container.DataItem,"MaskingName"))) %>' title="<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "MakeBase.MakeName")) + " " +Convert.ToString(DataBinder.Eval(Container.DataItem, "ModelName"))%>"><%# Convert.ToString(DataBinder.Eval(Container.DataItem, "MakeBase.MakeName")) + " " +Convert.ToString(DataBinder.Eval(Container.DataItem, "ModelName"))%></a></h3>
                        </div>
                        <div class="font22 text-grey margin-bottom5">
                            <span class="fa fa-rupee"></span>
                            <span class="font24"><%# Bikewale.Utility.Format.FormatPrice(Convert.ToString(DataBinder.Eval(Container.DataItem, "MinPrice"))) %></span><span class="font16"> onwards</span>
                        </div>
                        <div class="margin-bottom10 font14 text-light-grey">Ex-showroom, Delhi</div>
                        <div class="font13 margin-bottom10">
                            <%# Bikewale.Utility.FormatMinSpecs.GetMinSpecs(Convert.ToString(DataBinder.Eval(Container.DataItem, "Specs.Displacement")),Convert.ToString(DataBinder.Eval(Container.DataItem, "Specs.FuelEfficiencyOverall")),Convert.ToString(DataBinder.Eval(Container.DataItem, "Specs.MaxPower")),Convert.ToString(DataBinder.Eval(Container.DataItem, "Specs.MaximumTorque"))) %>                             
                        </div>
                          <div class="padding-top5 clear">
                            <div class="grid-6 alpha">
                                <div class="padding-left5 padding-right5">                                                                
                                    <div>
                                        <span class="margin-bottom10 <%# Convert.ToString(DataBinder.Eval(Container.DataItem,"ReviewCount")) != "0" ? "" : "hide" %>">
                                           <%# Bikewale.Utility.ReviewsRating.GetRateImage(Convert.ToDouble(DataBinder.Eval(Container.DataItem,"ReviewRate"))) %>
                                        </span>
                                    </div>

                                    <div class="padding-left5 padding-right5 <%# Convert.ToString(DataBinder.Eval(Container.DataItem,"ReviewCount")) == "0" ? "" : "hide" %>">                                                                
                                    <div>
                                        <span class="font14 text-light-grey margin-bottom10">
                                          Not Rated Yet
                                        </span>
                                    </div>
                                </div>

                                </div>
                            </div>
                            <div class="grid-6 omega border-left1">
                                <div class="padding-left5 padding-right5">
                                    <span class="font16 text-light-grey"><%# Bikewale.Utility.FormatDate.GetDDMMYYYY(Convert.ToString(DataBinder.Eval(Container.DataItem, "LaunchDate"))) %></span>
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