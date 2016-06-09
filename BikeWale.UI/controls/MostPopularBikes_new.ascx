<%@ Control Language="C#" AutoEventWireup="False"  Inherits="Bikewale.controls.MostPopularBikes_new" %>

<!-- Most Popular Bikes Starts here--> 
    <asp:Repeater ID="rptMostPopularBikes" runat="server">
        <ItemTemplate>
            <li class="front">
                <div class="contentWrapper">
                    <div class="imageWrapper">
                        <a href='<%# Bikewale.Utility.UrlFormatter.BikePageUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"objMake.MaskingName")),Convert.ToString(DataBinder.Eval(Container.DataItem,"objModel.MaskingName"))) %>'>
                            <img class="lazy" src="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImagePath").ToString(),DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._310x174) %>" title="<%# DataBinder.Eval(Container.DataItem, "objMake.MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "objModel.ModelName").ToString() %>"  alt="<%# DataBinder.Eval(Container.DataItem, "objMake.MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "objModel.ModelName").ToString() %>">
                        </a>
                    </div>
                    <div class="bikeDescWrapper">
                        <div class="bikeTitle margin-bottom10">
                            <h3><a href='<%# Bikewale.Utility.UrlFormatter.BikePageUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"objMake.MaskingName")),Convert.ToString(DataBinder.Eval(Container.DataItem,"objModel.MaskingName"))) %>' title="<%# DataBinder.Eval(Container.DataItem, "objMake.MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "objModel.ModelName").ToString() %>"><%# DataBinder.Eval(Container.DataItem, "objMake.MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "objModel.ModelName").ToString() %></a></h3>
                        </div>
                        <div class="font20">
                            <%# ShowEstimatedPrice(DataBinder.Eval(Container.DataItem, "VersionPrice")) %>                             
                        </div>
                        <div class="font12 text-light-grey margin-bottom10">Ex-showroom, <%=ConfigurationManager.AppSettings["defaultName"].ToString() %></div>
                        <div class="font14 margin-bottom10">
                            <%# Bikewale.Utility.FormatMinSpecs.GetMinSpecs(Convert.ToString(DataBinder.Eval(Container.DataItem, "Specs.Displacement")),Convert.ToString(DataBinder.Eval(Container.DataItem, "Specs.FuelEfficiencyOverall")),Convert.ToString(DataBinder.Eval(Container.DataItem, "Specs.MaxPower"))) %>                             
                        </div>
                        <div class="leftfloat">
                            <p class=" inline-block border-solid-right padding-right10 <%# Convert.ToString(DataBinder.Eval(Container.DataItem,"ReviewCount")) != "0" ? "" : "hide" %>">
                                <%# Bikewale.Utility.ReviewsRating.GetRateImage(Convert.ToDouble(DataBinder.Eval(Container.DataItem,"ModelRating"))) %>
                            </p>
                        </div>
                       <div class="leftfloat margin-left10 font16 text-light-grey <%# Convert.ToString(DataBinder.Eval(Container.DataItem,"ReviewCount")) != "0" ? "" : "hide" %>">
                          <a href="/<%# Convert.ToString(DataBinder.Eval(Container.DataItem,"objMake.MaskingName"))%>-bikes/<%#Convert.ToString(DataBinder.Eval(Container.DataItem,"objModel.MaskingName")) %>/user-reviews/" ><span><%# Convert.ToString(DataBinder.Eval(Container.DataItem, "ReviewCount")) %> Reviews</span></a>
                        </div>

                        <div class="leftfloat font16 text-light-grey <%# Convert.ToString(DataBinder.Eval(Container.DataItem,"ReviewCount")) == "0" ? "" : "hide" %>">
                         <span class="border-solid-right padding-right10">Not rated yet  </span><a href="/content/userreviews/writereviews.aspx?bikem=<%# DataBinder.Eval(Container.DataItem,"objModel.ModelId") %>"><span class="margin-left10">Write a review</span></a>
                        </div>

                        <div class="clear"></div>
                        <a href="#" pageCatId="<%= PageId %>" pqSourceId="<%= PQSourceId%>" makeName="<%# Convert.ToString(DataBinder.Eval(Container.DataItem,"objMake.MakeName")) %>" modelName="<%# Convert.ToString(DataBinder.Eval(Container.DataItem,"objModel.ModelName")) %>" modelId="<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "objModel.ModelId")) %>" class="btn btn-grey margin-top10 <%# (Convert.ToString(DataBinder.Eval(Container.DataItem, "VersionPrice"))!="0")?"":"hide" %> fillPopupData"">Check on-road price</a>
                    </div>
                </div>
            </li>
        </ItemTemplate>
    </asp:Repeater>
 <!--- Most Popular Bikes Ends Here-->