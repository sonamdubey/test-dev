<%@ Page Language="C#" AutoEventWireup="false" EnableViewState="false" Inherits="Bikewale.Mobile.New.Photos.Default" %>
<%@ Register Src="~/m/controls/ModelGallery.ascx" TagPrefix="BW" TagName="ModelGallery" %>
<!DOCTYPE html>
<html>
<head>
    <%
        title = String.Format("{0} Photos - BikeWale", bikeName);
        keywords = string.Format("{0} photos, {0} pictures, {0} images, {1} {0} photos", modelName, makeName);
        description = String.Format("View pictures of {0} in different colors and angles. Check out {2} photos of {1} on BikeWale", modelName, bikeName, imgCount);
        canonical = String.Format("https://www.bikewale.com/{0}-bikes/{1}/photos/", makeMaskingName,modelMaskingName);
        EnableOG = true;
        OGImage = modelImage;
     %>
    
    <!-- #include file="/includes/headscript_mobile.aspx" -->
    <link rel="stylesheet" type="text/css" href="/m/css/photos.css" />
</head>
<body>
    <form id="form1" runat="server">
        <!-- #include file="/includes/headBW_Mobile.aspx" -->
        <section>
            <div class="container box-shadow section-bottom-margin bg-grid">
                <h1 class="section-header bg-white">Bajaj Pulsar RS200 Photos(<%= imgCount %>)(<%= totalImages %>)(<%= imgCount %>)</h1>
                <% if(totalImages > 0) { %>
                <ul class="photos-grid-list">
                    <% for (int i = 0; (i < totalImages - 1) && i < 30; )
                       { %>
                    <li>
                        <img class="lazy" data-original="<%= Bikewale.Utility.Image.GetPathToShowImages(objImageList[i].OriginalImgPath,objImageList[i++].HostUrl,Bikewale.Utility.ImageSize._476x268) %>" src="" alt="Model Image" />
                    </li>
                    <li>
                        <img class="lazy" data-original="<%= Bikewale.Utility.Image.GetPathToShowImages(objImageList[i].OriginalImgPath,objImageList[i++].HostUrl,Bikewale.Utility.ImageSize._476x268) %>" src="" alt="Model Image" />
                    </li>
                    <li>
                        <img class="lazy" data-original="<%= Bikewale.Utility.Image.GetPathToShowImages(objImageList[i].OriginalImgPath,objImageList[i++].HostUrl,Bikewale.Utility.ImageSize._476x268) %>" src="" alt="Model Image" />
                    </li>
                    <li>
                        <img class="lazy" data-original="<%= Bikewale.Utility.Image.GetPathToShowImages(objImageList[i].OriginalImgPath,objImageList[i++].HostUrl,Bikewale.Utility.ImageSize._476x268) %>" src="" alt="Model Image" />
                    </li>
                    <li>
                        <img class="lazy" data-original="<%= Bikewale.Utility.Image.GetPathToShowImages(objImageList[i].OriginalImgPath,objImageList[i++].HostUrl,Bikewale.Utility.ImageSize._476x268) %>" src="" alt="Model Image" />
                    </li>
                    <li>
                        <img class="lazy" data-original="<%= Bikewale.Utility.Image.GetPathToShowImages(objImageList[i].OriginalImgPath,objImageList[i++].HostUrl,Bikewale.Utility.ImageSize._476x268) %>" src="" alt="Model Image" />
                    </li>
                    <% } %>
                </ul>
                <% } if(imgCount < 30 && remainingImages > 0) { %>
                <ul class="photos-grid-list photos-remainder-<%= remainingImages %>">
                      <% for (int i = totalImages; i < imgCount && i < 30; i++)
                         { %>
                   <li>
                        <img class="lazy" data-original="<%= Bikewale.Utility.Image.GetPathToShowImages(objImageList[i].OriginalImgPath,objImageList[i].HostUrl,Bikewale.Utility.ImageSize._476x268) %>" src="" alt="Model Image" />
                    </li>
                    <% } %>
                </ul>
                <% } %>
                <div class="clear"></div>
            </div>
        </section>

        <section>
            <div class="container bg-white box-shadow section-bottom-margin">
                <h2 class="padding-15-20">Photos for alternate bikes</h2>
                <div class="swiper-container card-container alternate-bikes-photo-swiper">
                    <div class="swiper-wrapper">
                        <div class="swiper-slide">
                            <div class="swiper-card">
                                <a href="" title="Bajaj Pulsar RS200 Photos" class="block swiper-card-target">
                                    <div class="swiper-image-preview">
                                        <img class="swiper-lazy" data-src="https://imgd3.aeplcdn.com/160x89/bw/models/bajaj-pulsar-rs200.jpg" alt="Bajaj Pulsar RS200 Photos" src="" />
                                        <span class="swiper-lazy-preloader"></span>
                                        <span class="black-overlay">
                                            <span class="bwmsprite photos-white-icon"></span>
                                        </span>
                                    </div>
                                    <div class="swiper-details-block">
                                        <h3 class="margin-bottom5 text-truncate">Bajaj Pulsar RS200</h3>
                                        <p class="font14 text-default text-truncate">128 photos</p>
                                    </div>
                                </a>
                            </div>
                        </div>
                        <div class="swiper-slide">
                            <div class="swiper-card">
                                <a href="" title="Bajaj Pulsar RS200 Photos" class="block swiper-card-target">
                                    <div class="swiper-image-preview">
                                        <img class="swiper-lazy" data-src="https://imgd3.aeplcdn.com/160x89/bw/models/bajaj-pulsar-rs200.jpg" alt="Bajaj Pulsar RS200 Photos" src="" />
                                        <span class="swiper-lazy-preloader"></span>
                                        <span class="black-overlay">
                                            <span class="bwmsprite photos-white-icon"></span>
                                        </span>
                                    </div>
                                    <div class="swiper-details-block">
                                        <h3 class="margin-bottom5 text-truncate">Bajaj Pulsar RS200</h3>
                                        <p class="font14 text-default text-truncate">128 photos</p>
                                    </div>
                                </a>
                            </div>
                        </div>
                        <div class="swiper-slide">
                            <div class="swiper-card">
                                <a href="" title="Bajaj Pulsar RS200 Photos" class="block swiper-card-target">
                                    <div class="swiper-image-preview">
                                        <img class="swiper-lazy" data-src="https://imgd3.aeplcdn.com/160x89/bw/models/bajaj-pulsar-rs200.jpg" alt="Bajaj Pulsar RS200 Photos" src="" />
                                        <span class="swiper-lazy-preloader"></span>
                                        <span class="black-overlay">
                                            <span class="bwmsprite photos-white-icon"></span>
                                        </span>
                                    </div>
                                    <div class="swiper-details-block">
                                        <h3 class="margin-bottom5 text-truncate">Bajaj Pulsar RS200</h3>
                                        <p class="font14 text-default text-truncate">128 photos</p>
                                    </div>
                                </a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>

 <script type="text/javascript" src="<%= staticUrl != "" ? "https://st1.aeplcdn.com" + staticUrl : "" %>/m/src/frameworks.js?<%= staticFileVersion %>"></script>
        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <link href="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : "" %>/m/css/bwm-common-btf.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
        <!-- #include file="/includes/footerscript_mobile.aspx" -->
        <script type="text/javascript">
            var photoCount = 100;
        </script>
        <script type="text/javascript" src="<%= staticUrl != "" ? "https://st1.aeplcdn.com" + staticUrl : "" %>/m/src/photos.js?<%= staticFileVersion %>"></script>
        <!-- #include file="/includes/fontBW_Mobile.aspx" -->
    </form>
</body>
</html>
