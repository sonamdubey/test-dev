<%@ Control Language="C#" AutoEventWireup="False" Inherits="Bikewale.Mobile.controls.MMostPopularBikes" %>
<!-- Most Popular Bikes Starts here-->
<asp:Repeater ID="rptMostPopularBikes" runat="server">
        <ItemTemplate>
            <div class="swiper-slide">
                <div class="front">
                <div class="contentWrapper">
                    <div class="imageWrapper">
                        <a href='/m<%# Bikewale.Utility.UrlFormatter.BikePageUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"objMake.MaskingName")),Convert.ToString(DataBinder.Eval(Container.DataItem,"objModel.MaskingName"))) %>'>
                            <img class="swiper-lazy" data-src="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImagePath").ToString(),DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._310x174) %>" title="<%# DataBinder.Eval(Container.DataItem, "objMake.MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "objModel.ModelName").ToString() %>"  alt="<%# DataBinder.Eval(Container.DataItem, "objMake.MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "objModel.ModelName").ToString() %>">
                            <span class="swiper-lazy-preloader"></span>
                        </a>
                    </div>
                    <div class="bikeDescWrapper">
                        <div class="bikeTitle">
                            <h3><a href='/m<%# Bikewale.Utility.UrlFormatter.BikePageUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"objMake.MaskingName")),Convert.ToString(DataBinder.Eval(Container.DataItem,"objModel.MaskingName"))) %>' title="<%# DataBinder.Eval(Container.DataItem, "objMake.MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "objModel.ModelName").ToString() %>"><%# DataBinder.Eval(Container.DataItem, "objMake.MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "objModel.ModelName").ToString() %></a></h3>
                        </div>
                        <div class="margin-bottom5">
                              <%# ShowEstimatedPrice(DataBinder.Eval(Container.DataItem, "VersionPrice")) %>     
                        </div>
                        <div class="margin-bottom10 font14 text-light-grey">Ex-showroom, <%=ConfigurationManager.AppSettings["defaultName"].ToString() %></div>
                        <div class="font13 margin-bottom10">
                            <%# Bikewale.Utility.FormatMinSpecs.GetMinSpecs(Convert.ToString(DataBinder.Eval(Container.DataItem, "Specs.Displacement")),Convert.ToString(DataBinder.Eval(Container.DataItem, "Specs.FuelEfficiencyOverall")),Convert.ToString(DataBinder.Eval(Container.DataItem, "Specs.MaxPower"))) %>                             
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
                                           <%# Bikewale.Utility.ReviewsRating.GetRateImage(Convert.ToDouble(DataBinder.Eval(Container.DataItem,"ModelRating"))) %>
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
                            <a href="javascript:void(0)" pqSourceId="<%= PQSourceId %>" modelName="<%# DataBinder.Eval(Container.DataItem,"objModel.ModelName").ToString() %>" makeName="<%# DataBinder.Eval(Container.DataItem,"objMake.MakeName").ToString() %>" pagecatid="<%=PageId %>"  modelId="<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "objModel.ModelId")) %>" class="<%# (Convert.ToString(DataBinder.Eval(Container.DataItem, "VersionPrice"))!="0")?"":"hide" %> btn btn-sm btn-white margin-top10 fillPopupData">Check On Road Price</a>
                        </div> 
                    </div>
                </div>
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>
<!--- Most Popular Bikes Ends Here-->