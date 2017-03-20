<%@ Control Language="C#" AutoEventWireup="False" Inherits="Bikewale.Mobile.Controls.MMostPopularBikes" %>
<!-- Most Popular Bikes Starts here-->
<asp:Repeater ID="rptMostPopularBikes" runat="server">
        <ItemTemplate>
            <div class="swiper-slide">
                <div class="swiper-card">
                    <a href="/m<%# Bikewale.Utility.UrlFormatter.BikePageUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"objMake.MaskingName")),Convert.ToString(DataBinder.Eval(Container.DataItem,"objModel.MaskingName"))) %>" title="<%# DataBinder.Eval(Container.DataItem, "objMake.MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "objModel.ModelName").ToString() %>">
                        <div class="swiper-image-preview position-rel">
                            <img class="swiper-lazy" data-src="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImagePath").ToString(),DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._174x98) %>" title=""  alt="<%# DataBinder.Eval(Container.DataItem, "objMake.MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "objModel.ModelName").ToString() %>">
                            <span class="swiper-lazy-preloader"></span>
                        </div>
                        <div class="swiper-details-block">
                            <h3 class="target-link font12 text-truncate margin-bottom5"><%# DataBinder.Eval(Container.DataItem, "objMake.MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "objModel.ModelName").ToString() %></h3>
                            <p class="text-truncate text-light-grey font11">Ex-showroom, <%=ConfigurationManager.AppSettings["defaultName"].ToString() %></p>
                            <p class="text-default">
                                <%# ShowEstimatedPrice(DataBinder.Eval(Container.DataItem, "VersionPrice")) %>     
                            </p>
                        </div>
                    </a>
                    <div class="swiper-btn-block">
                        <a href="javascript:void(0)" data-pqSourceId="<%= PQSourceId %>" data-modelName="<%# DataBinder.Eval(Container.DataItem,"objModel.ModelName").ToString() %>" data-makeName="<%# DataBinder.Eval(Container.DataItem,"objMake.MakeName").ToString() %>" data-pagecatid="<%=PageId %>"  data-modelid="<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "objModel.ModelId")) %>" class="<%# (Convert.ToString(DataBinder.Eval(Container.DataItem, "VersionPrice"))!="0")?"":"hide" %> btn btn-card btn-full-width btn-white getquotation">Check on-road price</a>
                    </div>
                
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>
<!--- Most Popular Bikes Ends Here-->
    <% if (mostPopularByMake)
                           { %>
<div class="carousel-heading-content padding-top15">
<div class="swiper-heading-left-grid inline-block">
	 <h2>Popular <%=makeName %> bikes in <%=cityname %></h2>
    </div>
<div class="swiper-heading-right-grid inline-block text-right">
            <a href="/m/<%=makeMaskingName %>-bikes/" title="<%=makeName %> Bikes" class="btn view-all-target-btn">View all</a>
        </div>
    <div class="clear"></div>
    </div>
                <div class="swiper-container card-container">
                    <div class="swiper-wrapper">
                         <asp:Repeater ID="rptPopoularBikeMake" runat="server">
                            <ItemTemplate>
                        <div class="swiper-slide">
                            <div class="swiper-card">
                                <a href='/m<%# Bikewale.Utility.UrlFormatter.BikePageUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"objMake.MaskingName")),Convert.ToString(DataBinder.Eval(Container.DataItem,"objModel.MaskingName"))) %>' title="<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "objMake.MakeName"))+" "+Convert.ToString( DataBinder.Eval(Container.DataItem, "objModel.ModelName"))%>">
                             <div class="swiper-image-preview position-rel">
                                       <img class="swiper-lazy" data-src="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImagePath").ToString(),DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._174x98) %>" title="<%# DataBinder.Eval(Container.DataItem, "objMake.MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "objModel.ModelName").ToString() %>"  alt="<%# DataBinder.Eval(Container.DataItem, "objMake.MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "objModel.ModelName").ToString() %>">
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
                                <div class="padding-top10 padding-right15 padding-bottom10 padding-left15">
                                    <a href="/m<%# Bikewale.Utility.UrlFormatter.PriceInCityUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"objMake.MaskingName")),Convert.ToString(DataBinder.Eval(Container.DataItem,"objModel.MaskingName")),cityMaskingName) %>" class="btn btn-card btn-full-width btn-white font14 text-truncate" title="<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "objMake.MakeName"))+" "+Convert.ToString( DataBinder.Eval(Container.DataItem, "objModel.ModelName"))%> On-road price in <%=cityname %>" >On-road price in <%=cityname %></a>
                                </div>
                            </div>
                        </div>
                                      </ItemTemplate>
                        </asp:Repeater>
                        </div>
                        </div>

            <%} %>