<%@ Control Language="C#" AutoEventWireup="False" Inherits="Bikewale.Mobile.Controls.MMostPopularBikes" %>
<!-- Most Popular Bikes Starts here-->
<asp:Repeater ID="rptMostPopularBikes" runat="server">
        <ItemTemplate>
            <div class="swiper-slide">
                <div class="front">
                <div class="contentWrapper">
                    <div class="imageWrapper">
                        <a href='/m<%# Bikewale.Utility.UrlFormatter.BikePageUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"objMake.MaskingName")),Convert.ToString(DataBinder.Eval(Container.DataItem,"objModel.MaskingName"))) %>' >
                            <img class="swiper-lazy" data-src="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImagePath").ToString(),DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._310x174) %>" title="<%# DataBinder.Eval(Container.DataItem, "objMake.MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "objModel.ModelName").ToString() %>"  alt="<%# DataBinder.Eval(Container.DataItem, "objMake.MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "objModel.ModelName").ToString() %>">
                            <span class="swiper-lazy-preloader"></span>
                        </a>
                    </div>
                    <div class="bikeDescWrapper">
                        <div class="bikeTitle margin-bottom10">
                            <h3><a href='/m<%# Bikewale.Utility.UrlFormatter.BikePageUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"objMake.MaskingName")),Convert.ToString(DataBinder.Eval(Container.DataItem,"objModel.MaskingName"))) %>' title="<%# DataBinder.Eval(Container.DataItem, "objMake.MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "objModel.ModelName").ToString() %>"><%# DataBinder.Eval(Container.DataItem, "objMake.MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "objModel.ModelName").ToString() %></a></h3>
                        </div>
                        <div class="font14 text-x-light margin-bottom10">
                            <%# Bikewale.Utility.FormatMinSpecs.GetMinSpecs(Convert.ToString(DataBinder.Eval(Container.DataItem, "Specs.Displacement")),Convert.ToString(DataBinder.Eval(Container.DataItem, "Specs.FuelEfficiencyOverall")),Convert.ToString(DataBinder.Eval(Container.DataItem, "Specs.MaxPower"))) %>                             
                        </div>
                        <div class="margin-bottom5 font14 text-light-grey">Ex-showroom, <%=ConfigurationManager.AppSettings["defaultName"].ToString() %></div>
                        <div class="margin-bottom5">
                              <%# ShowEstimatedPrice(DataBinder.Eval(Container.DataItem, "VersionPrice")) %>     
                        </div>
                        <a href="javascript:void(0)" data-pqSourceId="<%= PQSourceId %>" data-modelName="<%# DataBinder.Eval(Container.DataItem,"objModel.ModelName").ToString() %>" data-makeName="<%# DataBinder.Eval(Container.DataItem,"objMake.MakeName").ToString() %>" data-pagecatid="<%=PageId %>"  data-modelid="<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "objModel.ModelId")) %>" class="<%# (Convert.ToString(DataBinder.Eval(Container.DataItem, "VersionPrice"))!="0")?"":"hide" %> btn btn-sm btn-white margin-top10 getquotation">Check on-road price</a>
                    </div>
                </div>
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>
<!--- Most Popular Bikes Ends Here-->

                <h2 class="padding-15-20">Popular <%=makeName %> bikes in <%=cityname %></h2>
                <div class="swiper-container card-container">
                    <div class="swiper-wrapper">
                         <asp:Repeater ID="rptPopoularBikeMake" runat="server">
                            <ItemTemplate>
                        <div class="swiper-slide">
                            <div class="swiper-card">
                                <a href='/m<%# Bikewale.Utility.UrlFormatter.BikePageUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"objMake.MaskingName")),Convert.ToString(DataBinder.Eval(Container.DataItem,"objModel.MaskingName"))) %>' title="<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "objMake.MakeName"))+" "+Convert.ToString( DataBinder.Eval(Container.DataItem, "objModel.ModelName"))%>">
                             <div class="swiper-image-preview position-rel">
                                       <img class="swiper-lazy" data-src="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImagePath").ToString(),DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._310x174) %>" title="<%# DataBinder.Eval(Container.DataItem, "objMake.MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "objModel.ModelName").ToString() %>"  alt="<%# DataBinder.Eval(Container.DataItem, "objMake.MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "objModel.ModelName").ToString() %>">
                            <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-details-block padding-right15 padding-left15">
                                        <p class="target-link font12 text-truncate margin-bottom5"><%# Convert.ToString(DataBinder.Eval(Container.DataItem, "objMake.MakeName"))+" "+Convert.ToString( DataBinder.Eval(Container.DataItem, "objModel.ModelName"))%></p>
                                        <p class="text-truncate text-light-grey font11">Ex-showroom, <%=cityname %></p>
                                        <p>
                                           
                                            <span class="font16 text-default text-bold"><%# ShowEstimatedPrice(DataBinder.Eval(Container.DataItem, "VersionPrice")) %></span>
                                        </p>
                                    </div>
                                </a>
                                <div class="padding-10-15">
                                    <a href="/m<%# Bikewale.Utility.UrlFormatter.PriceInCityUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"objMake.MaskingName")),Convert.ToString(DataBinder.Eval(Container.DataItem,"objModel.MaskingName")),cityMaskingName) %>" class="btn btn-card btn-full-width btn-white font14 text-truncate" >On-road price in <%=cityname %></a>
                                </div>
                            </div>
                        </div>
                                      </ItemTemplate>
                        </asp:Repeater>
                        </div>
                        </div>
   