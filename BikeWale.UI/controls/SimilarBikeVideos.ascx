<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.SimilarBikeVideos" EnableViewState="false" %>
<div class="jcarousel-wrapper inner-content-carousel">
    <div class="jcarousel">
        <ul>
            <%foreach (var BikeInfo in SimilarBikeVideoList)
              {
                  string bikeName = string.Format("{0} {1}", BikeInfo.Make.MakeName, BikeInfo.Model.ModelName); %>
            <li>
                <a href="/<%=BikeInfo.Make.MaskingName%>-bikes/<%=BikeInfo.Model.MaskingName%>/videos/" title="<%=bikeName %>" class="jcarousel-card">
                    <div class="model-jcarousel-image-preview">
                        <div class="model-preview-image-container">
                            <div class="card-image-block">
                                <img class="lazy" data-original="<%= Bikewale.Utility.Image.GetPathToShowImages(BikeInfo.OriginalImagePath,BikeInfo.HostUrl,Bikewale.Utility.ImageSize._310x174) %>" src="" border="0">
                                <div class="play-icon-wrapper">
                                    <span class="bwsprite video-play-icon"></span>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="card-desc-block">
                        <h3 class="bikeTitle"><%=bikeName %></h3>
                        <p><%=BikeInfo.VideoCount %> Videos</p>
                    </div>
                </a>
            </li>
            <%} %>
        </ul>
    </div>
   
    <span class="jcarousel-control-left"><a href="#" class="bwsprite jcarousel-control-prev inactive" rel="nofollow" data-jcarouselcontrol="true"></a></span>
    <span class="jcarousel-control-right"> <a href="#" class="bwsprite jcarousel-control-next" rel="nofollow" data-jcarouselcontrol="true"></a></span>
</div>
