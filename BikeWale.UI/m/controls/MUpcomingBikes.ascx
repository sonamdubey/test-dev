<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.controls.MUpcomingBikes" %>

<!-- Mobile Upcoming Bikes Starts here-->
<div class="bw-tabs-data hide" id="mctrlUpcomingBikes">
    <asp:Repeater ID="rptUpcomingBikes" runat="server">
        <HeaderTemplate>
         <div class="jcarousel-wrapper upComingBikes">
                            <div class="jcarousel">
                                <ul>
        </HeaderTemplate>
        <ItemTemplate>
        <li class="card">
            <div class="front">
                <div class="contentWrapper">
                    <!--<div class="position-abt pos-right10 pos-top10 infoBtn bwmsprite alert-circle-icon"></div>-->
                    <div class="imageWrapper">
                            <a href="/m<%# Bikewale.Utility.UrlFormatter.BikePageUrl(Convert.ToString(DataBinder.Eval(Container.DataItem, "MakeBase.MaskingName")),Convert.ToString(DataBinder.Eval(Container.DataItem, "ModelBase.MaskingName"))) %>"></a>
                            <img class="lazy" src="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImagePath").ToString(),DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._310x174) %>" title="<%# DataBinder.Eval(Container.DataItem, "MakeBase.MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "ModelBase.ModelName").ToString() %>"  alt="<%# DataBinder.Eval(Container.DataItem, "MakeBase.MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "ModelBase.ModelName").ToString() %>">                                                       
                        </a>
                    </div>
                    <div class="bikeDescWrapper">
                        <div class="bikeTitle">
                            <h3><a href="/m<%# Bikewale.Utility.UrlFormatter.BikePageUrl(Convert.ToString(DataBinder.Eval(Container.DataItem, "MakeBase.MaskingName")),Convert.ToString(DataBinder.Eval(Container.DataItem, "ModelBase.MaskingName"))) %>"><%# DataBinder.Eval(Container.DataItem, "MakeBase.MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "ModelBase.ModelName").ToString() %></a></h3>
                        </div>
                        <div class="font22 text-grey margin-bottom5">
                            <span class="fa fa-rupee"></span>
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
        <FooterTemplate>
            </div>
                            <span class="jcarousel-control-left"><a href="javascript:void(0)" class="bwmsprite jcarousel-control-prev"></a></span>
                            <span class="jcarousel-control-right"><a href="javascript:void(0)" class="bwmsprite jcarousel-control-next"></a></span>
                            <p class="text-center jcarousel-pagination"></p>
                        </div>
        </FooterTemplate>
    </asp:Repeater>
</div><!-- Ends here-->
