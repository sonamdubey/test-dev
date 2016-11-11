<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.UpcomingBikesCMS" %>
            <h2>Upcoming bikes</h2>       
              <asp:Repeater ID="rptUpcomingBikes" runat="server" >
              <ItemTemplate>
                            <ul  class="sidebar-bike-list">
                                <li>
                                    <a href="<%# Bikewale.Utility.UrlFormatter.BikePageUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"MakeBase.MaskingName")),Convert.ToString(DataBinder.Eval(Container.DataItem,"ModelBase.MaskingName"))) %>" title="<%# DataBinder.Eval(Container.DataItem, "MakeBase.MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "ModelBase.ModelName").ToString() %>" title="Harley Davison Softail" class="bike-target-link">
                                        <div class="bike-target-image inline-block">
                                            <img src="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImagePath").ToString(),DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._110x61) %>" alt="<%# DataBinder.Eval(Container.DataItem, "MakeBase.MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "ModelBase.ModelName").ToString() %>" border="0">
                                        </div>
                                        <div class="bike-target-content inline-block padding-left10">
                                            <h3><%# DataBinder.Eval(Container.DataItem, "MakeBase.MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "ModelBase.ModelName").ToString() %></h3>
                                            <p class="font11 text-light-grey">Expected price</p>
                                            <span class="bwsprite inr-md"></span><span class="font16 text-bold"><%# Bikewale.Common.CommonOpn.FormatPrice(DataBinder.Eval(Container.DataItem,"EstimatedPriceMin").ToString()) %></span>
                                        </div>
                                    </a>
                                </li>
                                </ul>
                  </ItemTemplate>
                  </asp:Repeater>
                       
  
                                    