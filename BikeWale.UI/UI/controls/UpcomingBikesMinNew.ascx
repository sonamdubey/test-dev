<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.UpcomingBikesMinNew" %>
      <%if (FetchedRecordsCount>0){ %>
                        <div class="content-box-shadow padding-15-20-10 margin-bottom20">            
                     <h2>Upcoming <%=(!String.IsNullOrEmpty(makeName) ? makeName: "")%> bikes</h2>       
              <%foreach(var bike in objBikeList){ %>
                   <ul  class="sidebar-bike-list">
                                <li>
                                    <a href="<%= Bikewale.Utility.UrlFormatter.BikePageUrl(bike.MakeBase.MaskingName,bike.ModelBase.MaskingName) %>" title="<%= String.Format("{0} {1}",bike.MakeBase.MakeName,bike.ModelBase.ModelName) %>" class="bike-target-link">
                                        <div class="bike-target-image inline-block">
                                            <img src="<%= Bikewale.Utility.Image.GetPathToShowImages(bike.OriginalImagePath,bike.HostUrl,Bikewale.Utility.ImageSize._110x61) %>" alt="<%= String.Format("{0} {1}",bike.MakeBase.MakeName,bike.ModelBase.ModelName) %>" border="0">
                                        </div>
                                        <div class="bike-target-content inline-block padding-left10">
                                            <h3><%=String.Format("{0} {1}", bike.MakeBase.MakeName.ToString(), bike.ModelBase.ModelName.ToString())%></h3>
                                            <% if(bike.EstimatedPriceMin > 0) { %>
                                            <p class="font11 text-light-grey">Expected price</p>
                                        <span class="bwsprite inr-md"></span><span class="font16 text-bold">&nbsp;<%= Bikewale.Utility.Format.FormatPrice(Convert.ToString(bike.EstimatedPriceMin)) %> <span class="font14">onwards</span></span>
                                            <% } else { %> 
                                         <span class='font14 text-light-grey'>Price not available</span>
                                            <% } %>
                                         </div>
                                    </a>
                                </li>
                                </ul>
                  <%} %>
                       <div class="view-all-btn-container padding-top10 padding-bottom10">
                                <a href="<%=upcomingBikesLink %>" title="Upcoming bikes in India" class="btn view-all-target-btn">View all bikes<span class="bwsprite teal-right"></span></a>
                            </div>
                            </div>
                      <%} %>
  
                                    