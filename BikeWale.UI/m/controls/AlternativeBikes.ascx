<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Controls.AlternativeBikes" %>
<!-- Most Alternative Bikes Starts here-->
<asp:Repeater ID="rptAlternateBikes" runat="server">
    <HeaderTemplate>
        <ul>
    </HeaderTemplate>
    <ItemTemplate>
        <li class="card">
            <div class="front">
                <div class="contentWrapper">
                    <div class="imageWrapper">
                        <a href='/m<%# Bikewale.Utility.UrlFormatter.BikePageUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"MakeBase.MaskingName")),Convert.ToString(DataBinder.Eval(Container.DataItem,"ModelBase.MaskingName"))) %>'>
                            <img class="lazy" data-original="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImagePath").ToString(),DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._310x174) %>" title="<%# DataBinder.Eval(Container.DataItem, "MakeBase.MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "ModelBase.ModelName").ToString() %>" alt="<%# DataBinder.Eval(Container.DataItem, "MakeBase.MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "ModelBase.ModelName").ToString() %>" src="http://img.aeplcdn.com/bikewaleimg/images/circleloader.gif" width="310" height="174">
                        </a>
                    </div>
                    <div class="bikeDescWrapper">
                        <div class="bikeTitle">
                            <h3><a href='/m<%# Bikewale.Utility.UrlFormatter.BikePageUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"MakeBase.MaskingName")),Convert.ToString(DataBinder.Eval(Container.DataItem,"ModelBase.MaskingName"))) %>' title="<%# DataBinder.Eval(Container.DataItem, "MakeBase.MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "ModelBase.ModelName").ToString() %>"><%# DataBinder.Eval(Container.DataItem, "MakeBase.MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "ModelBase.ModelName").ToString() %></a></h3>
                        </div>
                        <div class="font22 text-grey margin-bottom5">
                            <span class="fa fa-rupee"></span>
                            <span class="font24"><%# Bikewale.Utility.Format.FormatPrice(DataBinder.Eval(Container.DataItem, "MinPrice").ToString()) %></span><span class="font16"> onwards</span>
                        </div>
                        <div class="margin-bottom10 font14 text-light-grey">Ex-showroom, <%= ConfigurationManager.AppSettings["defaultName"] %></div>
                        <div class="font13 margin-bottom10">
                            <%# Bikewale.Utility.FormatMinSpecs.GetMinSpecs(Convert.ToString(DataBinder.Eval(Container.DataItem, "Displacement")),Convert.ToString(DataBinder.Eval(Container.DataItem, "FuelEfficiencyOverall")),Convert.ToString(DataBinder.Eval(Container.DataItem, "MaxPower"))) %>
                        </div>
                        <div class="padding-top5 clear">
                            <div class="grid-12 alpha <%# Convert.ToString(DataBinder.Eval(Container.DataItem,"ReviewCount")) == "0" ? "" : "hide" %>">
                                <div class="padding-left5 padding-right5 ">
                                    <div>
                                        <span class="font16 text-light-grey">Not rated yet  </span>
                                    </div>
                                </div>
                            </div>
                            <div class="leftfloat">
                                <div class="padding-left5 padding-right5 <%# Convert.ToString(DataBinder.Eval(Container.DataItem,"ReviewCount")) != "0" ? "" : "hide" %>">
                                    <div>
                                        <span class="margin-bottom10 ">
                                            <%# Bikewale.Utility.ReviewsRating.GetRateImage(Convert.ToDouble(DataBinder.Eval(Container.DataItem,"ReviewRate"))) %>
                                        </span>
                                    </div>
                                </div>
                            </div>
                            <div class="leftfloat border-left1">
                                <div class="padding-left5 padding-right5 <%# Convert.ToString(DataBinder.Eval(Container.DataItem,"ReviewCount")) != "0" ? "" : "hide" %>">
                                    <span class="font16 text-light-grey"><%# DataBinder.Eval(Container.DataItem, "ReviewCount").ToString() %>  Reviews</span>
                                </div>
                            </div>
                            <div class="clear"></div>
                            <a href="javascript:void(0)" modelid="<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "ModelBase.ModelId")) %>" class="<%# (Convert.ToString(DataBinder.Eval(Container.DataItem, "VersionPrice"))!="0")?"":"hide" %> btn btn-sm btn-white margin-top10 fillPopupData">Get on road price</a>
                        </div>
                    </div>
                </div>
            </div>
        </li>
    </ItemTemplate>
    <FooterTemplate></ul></FooterTemplate>
</asp:Repeater>
<!--- Most Alternative Bikes Ends Here-->
