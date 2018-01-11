<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.GenericBikeInfoControl" EnableViewState="false" %>

<% if (bikeInfo != null)
   { %>
<section>
    <div class="container section-bottom-margin">
        <div class="grid-12">
            <div class="content-box-shadow padding-top20 padding-right20 padding-bottom15 padding-left20 model-grid-12-slug">
                <%if (IsUpcoming)
                  { %>
                <p class="model-ribbon-tag upcoming-ribbon">Upcoming</p>
                <%}
                  else if (IsDiscontinued)
                  { %>
                <p class="model-ribbon-tag discontinued-ribbon">Discontinued</p>
                <%} %>
                <div class="clear"></div>
                <a href="<%= bikeUrl%>" title="<%=bikeName%>" class="leftfloat margin-bottom15 text-default">
                    <h2><%=bikeName%></h2>
                </a>     
                <%if (RatingCount > 0)
                        { %>
                <div id="reviewRatingsDiv" class="inline-block">            
                        <span class="rate-count-<%=Math.Round(Rating) %>">
                            <span class="bwsprite star-icon star-size-16"></span>
                            <span class="font14 text-bold inline-block"><%= Rating.ToString("0.0").TrimEnd('0', '.') %></span>
                        </span>
                        <span class='font11 text-xt-light-grey inline-block padding-left3'>(<%=string.Format("{0} {1}", RatingCount, RatingCount > 1 ? "ratings" : "rating") %>)</span>
                        <%if (UserReviewCount > 0)
                        {  %>
                            <a class='text-xt-light review-left-divider inline-block' href="<%=string.Format("{0}reviews/", bikeUrl)%>" title="<%=bikeName%> user reviews"><%=string.Format("{0} {1}", UserReviewCount, UserReviewCount > 1 ? "reviews" : "review") %></a>
                       <%  } %>   
                    </div> 
                <%} %>               
                <div class="clear"></div>
                <div class="grid-8 alpha padding-right20 border-solid-right">
                    <a href="<%= bikeUrl%>" title="<%=bikeName%>" class="model-image-target vertical-top">
                        <img class="lazy" data-original="<%= Bikewale.Utility.Image.GetPathToShowImages(bikeInfo.OriginalImagePath,bikeInfo.HostUrl,Bikewale.Utility.ImageSize._160x89) %>" src="" alt="<%=bikeName%>" />
                    </a>                   
                    <div class="model-details-block vertical-top">
                        <% if (bikeInfo.MinSpecs != null)
                           { %>
                        <ul class="key-specs-list margin-bottom15 text-light-grey">
                            <%if (bikeInfo.MinSpecs.Displacement > 0)
                              { %>
                            <li>
                                <span class="bwsprite capacity-sm"></span>
                                <span><%=Bikewale.Utility.FormatMinSpecs.ShowAvailable(Convert.ToString(bikeInfo.MinSpecs.Displacement))%> cc</span>
                            </li>
                            <%} if (bikeInfo.MinSpecs.FuelEfficiencyOverall > 0)
                              { %>
                            <li>
                                <span class="bwsprite mileage-sm"></span>
                                <span><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(Convert.ToString(bikeInfo.MinSpecs.FuelEfficiencyOverall),"kmpl") %></span>
                            </li>
                            <%} if (bikeInfo.MinSpecs.MaxPower > 0)
                              { %>
                            <li>
                                <span class="bwsprite power-sm"></span>
                                <span><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(Convert.ToString(bikeInfo.MinSpecs.MaxPower)) %> bhp</span>
                            </li>
                            <%} if (bikeInfo.MinSpecs.KerbWeight > 0)
                              { %>
                            <li>
                                <span class="bwsprite weight-sm"></span>
                                <span><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(Convert.ToString(bikeInfo.MinSpecs.KerbWeight)) %> kgs</span>
                            </li>
                            <%} %>
                        </ul>
                        <%} %>                        
                        <ul class="item-more-details-list margin-bottom5 inline-block">
                            <%if (bikeInfo.Tabs != null)
                              {
                                  foreach (var Tabsdetails in bikeInfo.Tabs)
                                  { %>
                            <li>
                                <a href="<%= Tabsdetails.URL%>" title="<%= String.Format("{0} {1}",bikeName,Tabsdetails.Title)%>">
                                    <span class="bwsprite <%=Tabsdetails.IconText%>-sm"></span>
                                    <span class="icon-label"><%=Tabsdetails.TabText%></span>
                                </a>
                            </li>
                            <%}
                              } %>
                        </ul>
                        <%if (bikeInfo.UsedBikeCount > 0)
                          { %>
                        <div class="border-solid-bottom margin-bottom10"></div>
                        <a href="<%=Bikewale.Utility.UrlFormatter.UsedBikesUrlNoCity(bikeInfo.Make.MaskingName,bikeInfo.Model.MaskingName,(cityDetails!=null)?cityDetails.CityMaskingName:"india") %>" title="<%=bikeInfo.UsedBikeCount %> Used <%=bikeName%> bikes" class="block text-default hover-no-underline">
                            <span class="used-target-label inline-block">
                                <span class="font14 text-bold"><%=bikeInfo.UsedBikeCount %> Used <%=bikeName%> bikes</span>
                                <span class="font12 text-light-grey">starting at <span class="bwsprite inr-xsm-grey"></span> <%=Bikewale.Utility.Format.FormatNumeric(Convert.ToString(bikeInfo.UsedBikeMinPrice))%></span>
                            </span>
                            <span class="bwsprite next-grey-icon"></span>
                        </a>
                        <%} %>
                    </div>
                </div>
                <div class="grid-4 omega padding-left20">
                    <% if (IsDiscontinued)
                       {%>
                    <p class="font14 text-light-grey margin-bottom5" title="Last known Ex-showroom price">Last known Ex-showroom price</p>
                    <div class="margin-bottom10">
                        <span class="bwsprite inr-lg"></span>
                        <span class="font18 text-bold"><%= Bikewale.Utility.Format.FormatPrice(Convert.ToString(bikeInfo.BikePrice))%></span>
                    </div>
                    <%}
                       else if (IsUpcoming)
                       {%>
                    <p class="font14 text-light-grey margin-bottom5" title="Expected price">Expected price</p>
                    <div class="margin-bottom10">
                        <span class="bwsprite inr-lg"></span>
                        <span class="font18 text-bold"><%= Bikewale.Utility.Format.FormatNumeric(Convert.ToString(bikeInfo.EstimatedPriceMin)) %> - <%= Bikewale.Utility.Format.FormatNumeric(Convert.ToString(bikeInfo.EstimatedPriceMax)) %></span>
                    </div>
                    <%}
                       else
                       {
                           if (bikeInfo.PriceInCity > 0 && cityDetails != null)
                           { %>
                    <p class="font14 text-light-grey margin-bottom5" title="<%=String.Format("On-road price, {0}",cityDetails.CityName)%>"><%=String.Format("On-road price, {0}",cityDetails.CityName)%></p>
                    <div class="margin-bottom10">
                        <span class="bwsprite inr-lg"></span>
                        <span class="font18 text-bold"><%= Bikewale.Utility.Format.FormatPrice(Convert.ToString(bikeInfo.PriceInCity)) %></span>
                    </div>
                    <% }
                           else
                           { %>
                    <p class="font14 text-light-grey margin-bottom5" title="<%=String.Format("Ex-showroom, {0}",Bikewale.Utility.BWConfiguration.Instance.DefaultName)%>"><%=String.Format("Ex-showroom, {0}",Bikewale.Utility.BWConfiguration.Instance.DefaultName)%></p>
                    <div class="margin-bottom10">
                        <span class="bwsprite inr-lg"></span>
                        <span class="font18 text-bold"><%= Bikewale.Utility.Format.FormatPrice(Convert.ToString(bikeInfo.BikePrice)) %></span>
                    </div>
                    <%}
                       } %>
                    <a href="<%=Bikewale.Utility.UrlFormatter.BikePageUrl(bikeInfo.Make.MaskingName,bikeInfo.Model.MaskingName)%>" title="<%=bikeName%>" class="btn btn-white btn-180-34">View model details <span class="bwsprite btn-red-arrow"></span></a>
                </div>
                <div class="clear"></div>
            </div>
        </div>
        <div class="clear"></div>
    </div>
</section>
<% } %>
