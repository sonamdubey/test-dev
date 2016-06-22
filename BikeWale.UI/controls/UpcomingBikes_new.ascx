<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.controls.UpcomingBikes_new" %>
    <!-- Upcoming Bikes Starts here-->
    <asp:Repeater ID="rptUpcomingBikes" runat="server">
        <ItemTemplate>
            <li>
                <div class="model-preview-image-container margin-bottom15">
                    <a href='<%# Bikewale.Utility.UrlFormatter.BikePageUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"MakeBase.MaskingName")),Convert.ToString(DataBinder.Eval(Container.DataItem,"ModelBase.MaskingName"))) %>'>
                        <img class="lazy" data-original="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImagePath").ToString(),DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._310x174) %>" title="<%# DataBinder.Eval(Container.DataItem, "MakeBase.MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "ModelBase.ModelName").ToString() %>" alt="<%# DataBinder.Eval(Container.DataItem, "MakeBase.MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "ModelBase.ModelName").ToString() %>" src="" border="0">
                    </a>
                </div>
                <a class="block font16 text-bold margin-bottom10 text-black" href='<%# Bikewale.Utility.UrlFormatter.BikePageUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"MakeBase.MaskingName")),Convert.ToString(DataBinder.Eval(Container.DataItem,"ModelBase.MaskingName"))) %>'">
                    <%# DataBinder.Eval(Container.DataItem, "MakeBase.MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "ModelBase.ModelName").ToString() %>
                </a>
                <p class="font14 text-light-grey margin-bottom5">Expected launch</p>
                <p class="font16 text-bold margin-bottom15"><%# DataBinder.Eval(Container.DataItem, "ExpectedLaunchDate") %></p>
                <p class="font14 text-light-grey margin-bottom5">Expected price</p>
                <div class="font16">
                    <span class="bwsprite inr-lg"></span>
                    <span class="text-bold">
                        <%# Bikewale.Common.CommonOpn.FormatPrice(DataBinder.Eval(Container.DataItem,"EstimatedPriceMin").ToString()) %>
                     </span>
                </div>
            </li>
        </ItemTemplate>
    </asp:Repeater>
<!-- Ends here-->

                                    