<%@ Control Language="C#" AutoEventWireup="False" Inherits="Bikewale.Mobile.Controls.MNewLaunchedBikes" %>

<!-- New Launched Bikes Starts here-->  
    <asp:Repeater ID="rptNewLaunchedBikes" runat="server">
        <ItemTemplate>
            <div class="swiper-slide">
                  <div class="swiper-card">
                    <a href="/m<%# Bikewale.Utility.UrlFormatter.BikePageUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"MakeBase.MaskingName")),Convert.ToString(DataBinder.Eval(Container.DataItem,"MaskingName"))) %>" title="<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "MakeBase.MakeName")) + " " + Convert.ToString(DataBinder.Eval(Container.DataItem, "ModelName")) %>">
                        <div class="swiper-image-preview position-rel">
                            <img class="swiper-lazy" data-src="<%# Bikewale.Utility.Image.GetPathToShowImages(Convert.ToString(DataBinder.Eval(Container.DataItem, "OriginalImagePath")),Convert.ToString(DataBinder.Eval(Container.DataItem, "HostUrl")),Bikewale.Utility.ImageSize._174x98) %>" alt="<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "MakeBase.MakeName")) + " " +Convert.ToString(DataBinder.Eval(Container.DataItem, "ModelName")) %>">
                            <span class="swiper-lazy-preloader"></span>
                        </div>
                        <div class="swiper-details-block">
                            <h3 class="target-link font12 text-truncate margin-bottom5"><%# Convert.ToString(DataBinder.Eval(Container.DataItem, "MakeBase.MakeName")) + " " +Convert.ToString(DataBinder.Eval(Container.DataItem, "ModelName"))%></h3>
                            <p class="text-truncate text-light-grey font11">Ex-showroom, <%=ConfigurationManager.AppSettings["defaultName"].ToString() %></p>
                            <p>
                                <span class="text-default text-bold"><%# ShowEstimatedPrice(DataBinder.Eval(Container.DataItem, "MinPrice")) %></span>
                            </p>
                        </div>
                    </a>
                    <div class="swiper-btn-block">
                        <a href="javascript:void(0)" data-modelName="<%# DataBinder.Eval(Container.DataItem,"ModelName").ToString() %>" data-makeName="<%# DataBinder.Eval(Container.DataItem,"MakeBase.MakeName").ToString() %>" data-pqSourceId="<%= PQSourceId%>" data-pagecatid="<%=PageId %>" data-modelid="<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "ModelId")) %>" class="btn btn-card btn-full-width btn-white getquotation <%# (Convert.ToString(DataBinder.Eval(Container.DataItem, "MinPrice"))!="0")?"":"hide" %> ">Check on-road price</a>
                    </div>
                </div>
          </div>
        </ItemTemplate>          
    </asp:Repeater>
 <!--- New Launched Bikes Ends Here-->