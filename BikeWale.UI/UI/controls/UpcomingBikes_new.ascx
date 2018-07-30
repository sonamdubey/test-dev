<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.UpcomingBikes_new" %>
    <!-- Upcoming Bikes Starts here-->
    <asp:Repeater ID="rptUpcomingBikes" runat="server">
        <ItemTemplate>
            <li>
                <a href="<%# Bikewale.Utility.UrlFormatter.BikePageUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"MakeBase.MaskingName")),Convert.ToString(DataBinder.Eval(Container.DataItem,"ModelBase.MaskingName"))) %>" title="<%# DataBinder.Eval(Container.DataItem, "MakeBase.MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "ModelBase.ModelName").ToString() %>" class="jcarousel-card">
                    <div class="model-jcarousel-image-preview">
                        <span class="card-image-block">
                            <img class="lazy" data-original="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImagePath").ToString(),DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._310x174) %>" alt="<%# DataBinder.Eval(Container.DataItem, "MakeBase.MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "ModelBase.ModelName").ToString() %>" src="" border="0">
                        </span>
                    </div>
                    <div class="card-desc-block">
                        <h3 class="bikeTitle">
                            <span><%# DataBinder.Eval(Container.DataItem, "MakeBase.MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "ModelBase.ModelName").ToString() %></span>
                        </h3>
                        <p class="font14 text-light-grey margin-bottom5">Expected launch</p>
                        <p class="font16 text-default text-bold margin-bottom15"><%# DataBinder.Eval(Container.DataItem, "ExpectedLaunchDate") %></p>
                        <p class="font14 text-light-grey margin-bottom5">Expected price</p>
                        <span class="bwsprite inr-lg"></span>
                        <span class="font18 text-default text-bold"><%# Bikewale.Common.CommonOpn.FormatPrice(DataBinder.Eval(Container.DataItem,"EstimatedPriceMin").ToString()) %> onwards</span>
                    </div>
                </a>
            </li>
        </ItemTemplate>
    </asp:Repeater>
    <!-- Ends here-->
                                    