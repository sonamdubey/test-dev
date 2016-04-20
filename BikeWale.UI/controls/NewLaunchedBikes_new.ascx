<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.controls.NewLaunchedBikes_new" %>
<!-- New Launched Bikes Starts here-->    
    <asp:Repeater ID="rptNewLaunchedBikes" runat="server">
        <ItemTemplate>
            <li class="front">
                <div class="contentWrapper">
                    <div class="imageWrapper">
                        <a href='<%# Bikewale.Utility.UrlFormatter.BikePageUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"MakeBase.MaskingName")),Convert.ToString(DataBinder.Eval(Container.DataItem,"MaskingName"))) %>'>
                            <img class="lazy" src="<%# Bikewale.Utility.Image.GetPathToShowImages(Convert.ToString(DataBinder.Eval(Container.DataItem, "OriginalImagePath")),Convert.ToString(DataBinder.Eval(Container.DataItem, "HostUrl")),Bikewale.Utility.ImageSize._310x174) %>" title="<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "MakeBase.MakeName")) + " " + Convert.ToString(DataBinder.Eval(Container.DataItem, "ModelName")) %>"  alt="<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "MakeBase.MakeName")) + " " +Convert.ToString(DataBinder.Eval(Container.DataItem, "ModelName")) %>">
                        </a>
                    </div>
                    <div class="bikeDescWrapper">
                        <div class="bikeTitle margin-bottom10">
                            <h3><a href='<%# Bikewale.Utility.UrlFormatter.BikePageUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"MakeBase.MaskingName")),Convert.ToString(DataBinder.Eval(Container.DataItem,"MaskingName"))) %>' title="<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "MakeBase.MakeName")) + " " +Convert.ToString(DataBinder.Eval(Container.DataItem, "ModelName"))%>"><%# Convert.ToString(DataBinder.Eval(Container.DataItem, "MakeBase.MakeName")) + " " +Convert.ToString(DataBinder.Eval(Container.DataItem, "ModelName"))%></a></h3>
                        </div>
                        <div class="font20 ">
                            <%# ShowEstimatedPrice(DataBinder.Eval(Container.DataItem, "MinPrice")) %>  
                        </div>
                        <div class="font12 text-light-grey margin-bottom10">Ex-showroom, <%=ConfigurationManager.AppSettings["defaultName"].ToString() %></div>
                        <div class="font14 margin-bottom10">
                            <%# Bikewale.Utility.FormatMinSpecs.GetMinSpecs(Convert.ToString(DataBinder.Eval(Container.DataItem, "Specs.Displacement")),Convert.ToString(DataBinder.Eval(Container.DataItem, "Specs.FuelEfficiencyOverall")),Convert.ToString(DataBinder.Eval(Container.DataItem, "Specs.MaxPower"))) %>                             
                        </div>
                        <div class="leftfloat">
                            <p class=" inline-block border-solid-right padding-right10  <%# Convert.ToString(DataBinder.Eval(Container.DataItem,"ReviewCount")) != "0" ? "" : "hide" %>"">
                                <%# Bikewale.Utility.ReviewsRating.GetRateImage(Convert.ToDouble(DataBinder.Eval(Container.DataItem,"ReviewRate"))) %>
                            </p>
                        </div>
                        <div class=" leftfloat font16 text-light-grey <%# Convert.ToString(DataBinder.Eval(Container.DataItem,"ReviewCount")) == "0" ? "" : "hide" %>">
                         <span class="border-solid-right padding-right10">
                            Not rated yet
                         </span>
                        </div>

                        <div class="leftfloat margin-left10 font16 text-light-grey">
                            <span><%# Bikewale.Utility.FormatDate.GetDDMMYYYY(Convert.ToString(DataBinder.Eval(Container.DataItem, "LaunchDate"))) %></span>
                        </div>
                        <div class="clear"></div>
                        <a href="#" makeName="<%# DataBinder.Eval(Container.DataItem,"MakeBase.MakeName").ToString() %>" modelName="<%# DataBinder.Eval(Container.DataItem,"ModelName") %>" pagecatid="<%= PageId %>" pqSourceId="<%= PQSourceId %>" modelId="<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "ModelId")) %>" class="btn btn-grey margin-top10 <%# (Convert.ToString(DataBinder.Eval(Container.DataItem, "MinPrice"))!="0")?"":"hide" %> fillPopupData">Check On Road Price</a>
                    </div>
                </div>
            </li>
        </ItemTemplate>
    </asp:Repeater>
<!--- New Launched Bikes Ends Here-->

