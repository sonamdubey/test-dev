<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.NewAlternativeBikes" %>
<!-- Alternative Bikes Starts here-->    

<div id="modelAlternateBikeContent" class="bw-model-tabs-data padding-top20 padding-bottom20 font14">
    <h2 class="padding-left20 padding-right20 margin-bottom15"><%=heading %></h2>
    <div class="jcarousel-wrapper inner-content-carousel">
        <div class="jcarousel">
            <ul>
                <asp:Repeater ID="rptAlternateBikes" runat="server">
                    <ItemTemplate>
                        <li>
                            <%if(priceincitypage){ %>
                             <a href="<%# Bikewale.Utility.UrlFormatter.BikePageUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"MakeBase.MaskingName")),Convert.ToString(DataBinder.Eval(Container.DataItem,"ModelBase.MaskingName"))) %>" title="<%# DataBinder.Eval(Container.DataItem, "MakeBase.MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "ModelBase.ModelName").ToString() %>" class="jcarousel-card">
                                <div class="model-jcarousel-image-preview">
                                    <img class="lazy" data-original="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImagePath").ToString(),DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._310x174) %>" alt="<%# DataBinder.Eval(Container.DataItem, "MakeBase.MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "ModelBase.ModelName").ToString() %>" src="" border="0">
                                </div>
                                <div class="card-desc-block">
                                    <p class="bikeTitle"><%# DataBinder.Eval(Container.DataItem, "MakeBase.MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "ModelBase.ModelName").ToString() %></p>
                                    <p class="text-xt-light-grey margin-bottom10"><%# Bikewale.Utility.FormatMinSpecs.GetMinSpecs(Convert.ToString(DataBinder.Eval(Container.DataItem, "Displacement")),Convert.ToString(DataBinder.Eval(Container.DataItem, "FuelEfficiencyOverall")),Convert.ToString(DataBinder.Eval(Container.DataItem, "MaxPower")),Convert.ToString(DataBinder.Eval(Container.DataItem, "Kerbweight"))) %></p>
                                
                                        <p class="text-light-grey margin-bottom5">
                                     
                                        <%#"Ex Showroom,"+DataBinder.Eval(Container.DataItem, "CityName") %>
                                            </p>
                                              </div>
                            </a>   
                                    <span class="bwsprite inr-lg"></span>&nbsp;<span class="font18 text-default text-bold"><%# Bikewale.Utility.Format.FormatPrice(DataBinder.Eval(Container.DataItem, "MinPrice").ToString())%></span>
                                  <div class="margin-left20 margin-bottom20">
                                        <a href="<%# Bikewale.Utility.UrlFormatter.PriceInCityUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"MakeBase.MaskingName")),Convert.ToString(DataBinder.Eval(Container.DataItem,"ModelBase.MaskingName")),Convert.ToString(DataBinder.Eval(Container.DataItem,"CityMaskingName"))) %>" class="btn btn-white btn-truncate font14 btn-size-2" title="<%# DataBinder.Eval(Container.DataItem, "MakeBase.MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "ModelBase.ModelName").ToString() %> On-road price in <%#Convert.ToString(DataBinder.Eval(Container.DataItem, "CityName")) %>" >On-road price in <%#Convert.ToString(DataBinder.Eval(Container.DataItem, "CityName")) %></a>
                                    </div>

                                <% }else { %>
                             <a href="<%# Bikewale.Utility.UrlFormatter.BikePageUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"MakeBase.MaskingName")),Convert.ToString(DataBinder.Eval(Container.DataItem,"ModelBase.MaskingName"))) %>" title="<%# DataBinder.Eval(Container.DataItem, "MakeBase.MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "ModelBase.ModelName").ToString() %>" class="jcarousel-card">
                                <div class="model-jcarousel-image-preview">
                                    <img class="lazy" data-original="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImagePath").ToString(),DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._310x174) %>" alt="<%# DataBinder.Eval(Container.DataItem, "MakeBase.MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "ModelBase.ModelName").ToString() %>" src="" border="0">
                                </div>
                                <div class="card-desc-block">
                                    <p class="bikeTitle"><%# DataBinder.Eval(Container.DataItem, "MakeBase.MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "ModelBase.ModelName").ToString() %></p>
                                    <p class="text-xt-light-grey margin-bottom10"><%# Bikewale.Utility.FormatMinSpecs.GetMinSpecs(Convert.ToString(DataBinder.Eval(Container.DataItem, "Displacement")),Convert.ToString(DataBinder.Eval(Container.DataItem, "FuelEfficiencyOverall")),Convert.ToString(DataBinder.Eval(Container.DataItem, "MaxPower")),Convert.ToString(DataBinder.Eval(Container.DataItem, "Kerbweight"))) %></p>
                                

                                           <p class="text-light-grey margin-bottom5">
                                    Ex-Showroom , Mumbai

                                    </p>
                                    <span class="bwsprite inr-lg"></span>&nbsp;<span class="font18 text-default text-bold"><%# Bikewale.Utility.Format.FormatPrice(DataBinder.Eval(Container.DataItem, "MinPrice").ToString())%></span>
                                </div>
                            </a>
                            <div class="margin-left20 margin-bottom20">
                                <a href="javascript:void(0)" pqsourceid="<%= PQSourceId %>" modelid="<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "ModelBase.ModelId")) %>" class="<%# (Convert.ToString(DataBinder.Eval(Container.DataItem, "MinPrice"))!="0")?string.Empty:"hide" %> btn btn-sm btn-grey font14 fillPopupData" rel="nofollow">Check on-road price</a>
                            </div>

                                    <%} %>
                        </li>
                    </ItemTemplate>
                </asp:Repeater>

            </ul>
        </div>
        <span class="jcarousel-control-left"><a href="#" class="bwsprite jcarousel-control-prev inactive" rel="nofollow"></a></span>
        <span class="jcarousel-control-right"><a href="#" class="bwsprite jcarousel-control-next" rel="nofollow"></a></span>
    </div>
</div>
