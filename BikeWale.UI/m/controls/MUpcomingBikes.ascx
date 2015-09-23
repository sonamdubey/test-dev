<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.controls.MUpcomingBikes" %>

<!-- Mobile Upcoming Bikes Starts here-->
    <asp:Repeater ID="rptUpcomingBikes" runat="server">
        <ItemTemplate>
        <li class="card">
            <div class="front">
                <div class="contentWrapper">
                    <!--<div class="position-abt pos-right10 pos-top10 infoBtn bwmsprite alert-circle-icon"></div>-->
                    <div class="imageWrapper">
                            <a href="/m<%# Bikewale.Utility.UrlFormatter.BikePageUrl(Convert.ToString(DataBinder.Eval(Container.DataItem, "MakeBase.MaskingName")),Convert.ToString(DataBinder.Eval(Container.DataItem, "ModelBase.MaskingName"))) %>"></a>
                            <img class="lazy" data-original="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImagePath").ToString(),DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._310x174) %>" title="<%# DataBinder.Eval(Container.DataItem, "MakeBase.MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "ModelBase.ModelName").ToString() %>"  alt="<%# DataBinder.Eval(Container.DataItem, "MakeBase.MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "ModelBase.ModelName").ToString() %>" src="http://img1.aeplcdn.com/grey.gif" width="310" height="174">
                        </a>
                    </div>
                    <div class="bikeDescWrapper">
                        <div class="bikeTitle">
                            <h3><a href="/m<%# Bikewale.Utility.UrlFormatter.BikePageUrl(Convert.ToString(DataBinder.Eval(Container.DataItem, "MakeBase.MaskingName")),Convert.ToString(DataBinder.Eval(Container.DataItem, "ModelBase.MaskingName"))) %>"><%# DataBinder.Eval(Container.DataItem, "MakeBase.MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "ModelBase.ModelName").ToString() %></a></h3>
                        </div>
                        <div class="font22 text-grey margin-bottom5">
                            <span class="fa fa-rupee" style="display: <%# (Convert.ToString(DataBinder.Eval(Container.DataItem, "EstimatedPriceMin"))=="0")?"none":"inline-block"%>"></span>
                            <span class="font24"><%# ShowEstimatedPrice(DataBinder.Eval(Container.DataItem, "EstimatedPriceMin")) %></span>
                        </div>
                        <div class=" <%# (Convert.ToString(DataBinder.Eval(Container.DataItem, "EstimatedPriceMin"))=="0")?"hide":""%> margin-bottom20 font14 text-light-grey">Expected Price</div>
                        <div class="padding-top5 clear border-top1">
                            <div><span class="font16 text-grey"><%# ShowLaunchDate(DataBinder.Eval(Container.DataItem, "ExpectedLaunchDate")) %></span></div>
                        </div>
                    </div>
                </div>
            </div>
        </li> 
        </ItemTemplate>
    </asp:Repeater>
<!-- Ends here-->
