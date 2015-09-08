<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.controls.UpcomingBikes_new" %>
    <!-- Upcoming Bikes Starts here-->
    <asp:Repeater ID="rptUpcomingBikes" runat="server">
        <ItemTemplate>
            <li class="front">
                <div class="contentWrapper">
                    <div class="imageWrapper">
                        <a href='<%# Bikewale.Utility.UrlFormatter.BikePageUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"MakeBase.MaskingName")),Convert.ToString(DataBinder.Eval(Container.DataItem,"ModelBase.MaskingName"))) %>'>
                            <img class="lazy" src="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImagePath").ToString(),DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._310x174) %>" title="<%# DataBinder.Eval(Container.DataItem, "MakeBase.MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "ModelBase.ModelName").ToString() %>"  alt="<%# DataBinder.Eval(Container.DataItem, "MakeBase.MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "ModelBase.ModelName").ToString() %>">
                        </a>
                    </div>
                    <div class="bikeDescWrapper">
                        <div class="bikeTitle margin-bottom10">
                            <h3><a href='<%# Bikewale.Utility.UrlFormatter.BikePageUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"MakeBase.MaskingName")),Convert.ToString(DataBinder.Eval(Container.DataItem,"ModelBase.MaskingName"))) %>'">
                                <%# DataBinder.Eval(Container.DataItem, "MakeBase.MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "ModelBase.ModelName").ToString() %>
                                </a></h3>
                        </div>
                        <div class="margin-bottom10 font20 ">
                            <span class="fa fa-rupee" style="display:<%# (Convert.ToString(DataBinder.Eval(Container.DataItem, "EstimatedPriceMin"))=="0")?"none":"inline-block"%>"></span>
                            <span class="font22"><%# ShowEstimatedPrice(DataBinder.Eval(Container.DataItem, "EstimatedPriceMin")) %> </span>
                        </div>
                        <div class="font12 text-light-grey margin-bottom10 <%# (Convert.ToString(DataBinder.Eval(Container.DataItem, "EstimatedPriceMin"))=="0")?"hide":""%>">Expected Price</div>
                        <div class="border-solid-top margin-top10 margin-bottom10"></div>
                        <p class="font16"><%# ShowLaunchDate(DataBinder.Eval(Container.DataItem, "ExpectedLaunchDate")) %>
                    </div>
                </div>
            </li>
        </ItemTemplate>
    </asp:Repeater>
<!-- Ends here-->