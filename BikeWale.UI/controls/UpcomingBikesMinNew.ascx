<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.UpcomingBikesMinNew" %>
      <%if (FetchedRecordsCount>0){ %>
                        <div class="content-box-shadow padding-15-20-10 margin-bottom20">            
                     <h2>Upcoming bikes</h2>       
              <%foreach(var bike in objBikeList){ %>
                   <ul  class="sidebar-bike-list">
                                <li>
                                    <a href="<%= Bikewale.Utility.UrlFormatter.BikePageUrl(Convert.ToString(bike.MakeBase.MakeName),Convert.ToString(bike.ModelBase.MaskingName)) %>" title="<%= String.Format("{0} {1}",bike.MakeBase.MakeName.ToString(),bike.ModelBase.ModelName.ToString()) %>" class="bike-target-link">
                                        <div class="bike-target-image inline-block">
                                            <img src="<%= Bikewale.Utility.Image.GetPathToShowImages(bike.OriginalImagePath.ToString(),bike.HostUrl.ToString(),Bikewale.Utility.ImageSize._110x61) %>" alt="<%= String.Format("{0} {1}",bike.MakeBase.MakeName.ToString(),bike.ModelBase.ModelName.ToString()) %>" border="0">
                                        </div>
                                        <div class="bike-target-content inline-block padding-left10">
                                            <h3><%=String.Format("{0} {1}", bike.MakeBase.MakeName.ToString(), bike.ModelBase.ModelName.ToString())%></h3>
                                            <p class="font11 text-light-grey">Expected price</p>
                                            <span class="bwsprite inr-md"></span><span class="font16 text-bold"><%= Bikewale.Common.CommonOpn.FormatPrice(bike.EstimatedPriceMin.ToString()) %></span>
                                        </div>
                                    </a>
                                </li>
                                </ul>
                  <%} %>
                       <div class="margin-top10 margin-bottom10">
                                <a href="<%=upcomingBikesLink %>" class="font14">View all upcoming bikes<span class="bwsprite blue-right-arrow-icon"></span></a>
                            </div>
                            </div>
                      <%} %>
  
                                    