<%@ Page Language="C#" AutoEventWireup="false" EnableViewState="false" Inherits="Bikewale.New.Photos.Default" %>

<%@ Register TagPrefix="BW" TagName="SimilarBikeWithPhotos" Src="~/controls/SimilarBikeWithPhotos.ascx" %>
<%@ Register TagPrefix="BW" TagName="Videos" Src="~/controls/NewVideosControl.ascx" %>
<!DOCTYPE html>
<html>
<head>
    <%  if (vmModelPhotos != null && vmModelPhotos.pageMetas != null)
        {
            title = vmModelPhotos.pageMetas.Title;
            keywords = vmModelPhotos.pageMetas.Keywords;
            description = vmModelPhotos.pageMetas.Description;
            canonical = vmModelPhotos.pageMetas.CanonicalUrl;
            alternate = vmModelPhotos.pageMetas.AlternateUrl;
            enableOG = true;
            ogImage = vmModelPhotos.modelImage;
        }       
    %>
    <!-- #include file="/includes/headscript_desktop_min.aspx" -->
    <link rel="stylesheet" type="text/css" href="https://cdnjs.cloudflare.com/ajax/libs/Swiper/3.4.1/css/swiper.min.css" />
    <link rel="stylesheet" type="text/css" href="/css/photos.css" />
    <script type="text/javascript">
        <!-- #include file="\includes\gacode_desktop.aspx" -->
    </script>
</head>
<body class="header-fixed-inner">
    <form id="form1" runat="server">
        <!-- #include file="/includes/headBW.aspx" -->

        <section class="container padding-top10">
            <div class="grid-12">
                <div class="breadcrumb margin-bottom15">
                    <ul>
                        <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                            <a href="/" itemprop="url" title="Home">
                                <span itemprop="title">Home</span>
                            </a>
                        </li>
                        <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                            <span class="bwsprite fa-angle-right margin-right10"></span>
                            <a href="/new-bikes-in-india/" itemprop="url" title="New Bikes">
                                <span itemprop="title">New Bikes</span>
                            </a>
                        </li>
                        <% if (vmModelPhotos != null && vmModelPhotos.objMake != null && vmModelPhotos.objModel != null)
                           { %>
                        <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                            <span class="bwsprite fa-angle-right margin-right10"></span>
                            <a href="/<%=vmModelPhotos.objMake.MaskingName%>-bikes/" itemprop="url" title="<%=vmModelPhotos.objMake.MakeName%> Bikes">
                                <span itemprop="title"><%=vmModelPhotos.objMake.MakeName%> Bikes</span>
                            </a>
                        </li>
                        <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                            <span class="bwsprite fa-angle-right margin-right10"></span>
                            <a href="/<%=vmModelPhotos.objMake.MaskingName%>-bikes/<%=vmModelPhotos.objModel.MaskingName%>/" title="<%=bikeName%>">
                                <span itemprop="title"><%=bikeName%></span>
                            </a>
                        </li>
                        <%} %>
                        <li><span class="bwsprite fa-angle-right margin-right10"></span>Images</li>
                    </ul>
                    <div class="clear"></div>
                </div>
            </div>
            <div class="clear"></div>
        </section>

        <section>
            <div class="container margin-bottom20">
                <div class="grid-12">
                    <div class="content-box-shadow bg-listing">
                        <h1 class="content-box-shadow padding-14-20"><%=bikeName%> Images</h1>
                        <% if (vmModelPhotos != null)
                           {
                               var objImages = vmModelPhotos.objImageList;                                
                        %>
                        <ul class="photos-grid-list model-main-image">
                            <li>
                                <%if (vmModelPhotos.firstImage != null)
                                  { %>
                                <div class="main-image-container">
                                    <img src="<%= Bikewale.Utility.Image.GetPathToShowImages(vmModelPhotos.firstImage.OriginalImgPath,vmModelPhotos.firstImage.HostUrl,Bikewale.Utility.ImageSize._640x348)%>" alt="<%= vmModelPhotos.firstImage.ImageCategory %> Image" title="<%= vmModelPhotos.firstImage.ImageCategory %>" />
                                </div>
                                <% } %>
                            </li>
                        </ul>
                        <% int i = 0; if (vmModelPhotos.totalPhotosCount > 0)
                           { %>
                        <ul class="photos-grid-list model-grid-images">
                            <% while (i < vmModelPhotos.gridPhotosCount - 1 && i < 13) //to handle lazy load for initial images (12 images can vary) 
                               { %>
                            <li>
                                <img src="<%= Bikewale.Utility.Image.GetPathToShowImages(objImages[i].OriginalImgPath,objImages[i].HostUrl,Bikewale.Utility.ImageSize._476x268) %>" alt="<%= objImages[i].ImageCategory %> Image" title="<%= objImages[i++].ImageCategory %>" />
                            </li>
                            <% } %>
                            <% while (i < vmModelPhotos.gridPhotosCount && i < vmModelPhotos.GridSize)
                               { %>
                            <li>
                                <img class="lazy" data-original="<%= Bikewale.Utility.Image.GetPathToShowImages(objImages[i].OriginalImgPath,objImages[i].HostUrl,Bikewale.Utility.ImageSize._476x268) %>" src="" alt="<%= objImages[i].ImageCategory %> Image" title="<%= objImages[i++].ImageCategory %>" />
                            </li>
                            <% }  %>
                        </ul>
                        <% if (vmModelPhotos.totalPhotosCount < vmModelPhotos.GridSize && vmModelPhotos.nongridPhotosCount > 1)
                           { %>
                        <ul class="photos-grid-list photos-remainder-<%= vmModelPhotos.nongridPhotosCount %> remainder-grid-list model-grid-images">
                            <% while (i < vmModelPhotos.totalPhotosCount && i < vmModelPhotos.GridSize)
                               { %>
                            <li>
                                <img class="lazy" data-original="<%= Bikewale.Utility.Image.GetPathToShowImages(objImages[i].OriginalImgPath,objImages[i].HostUrl,Bikewale.Utility.ImageSize._476x268) %>" src="" alt="<%= objImages[i].ImageCategory %> Image" title="<%= objImages[i++].ImageCategory %>" />
                            </li>
                            <% }
                           }
                           else if (vmModelPhotos.totalPhotosCount == 1)
                           { %>
                            <li>
                                <img src="<%= Bikewale.Utility.Image.GetPathToShowImages(objImages[0].OriginalImgPath,objImages[0].HostUrl,Bikewale.Utility.ImageSize._476x268) %>" alt="<%= objImages[0].ImageCategory %> Image" title="<%= objImages[0].ImageCategory %>" />
                            </li>
                            <% } %>
                        </ul>
                        <% }
                           }%>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>


        <% if (ctrlVideos.FetchedRecordsCount > 0)
           { %>
        <div class="container margin-bottom20">
            <div class="grid-12">
                <div id="modelVideosContent" class="content-box-shadow font14 padding-top20 padding-right10 padding-bottom20 padding-left10 margin-bottom20 border-solid-bottom">
                    <BW:Videos runat="server" ID="ctrlVideos" />
                </div>
            </div>
        </div>
        <% } %>

        <BW:SimilarBikeWithPhotos ID="ctrlSimilarBikesWithPhotos" runat="server" />

        
        <script type="text/javascript" src="<%= staticUrl != "" ? "https://st1.aeplcdn.com" + staticUrl : "" %>/src/frameworks.js?<%=staticFileVersion %>"></script>
        <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/Swiper/3.4.1/js/swiper.min.js"></script>

        <div id="gallery-root">
            <div class="gallery-container">
                <div class="gallery-header">
                    <h2 class="text-white gallery-title"><%= bikeName %> Images</h2>
                    <span id="gallery-close-btn" class="bwsprite cross-md-white cur-pointer"></span>
                    <ul class="horizontal-tabs-wrapper">
                        <li data-bind="click: togglePhotoTab, css: photosTabActive() ? 'active' : ''">Images</li>
                        <li data-bind="click: togglePhotoTab, css: !photosTabActive() ? 'active' : ''">Videos</li>
                    </ul>
                    <div class="clear"></div>
                </div>

                <div class="gallery-body">
                    <div id="main-photo-swiper" class="gallery-swiper" data-bind="visible: photosTabActive()">
                        <div class="swiper-container">
                            <div class="swiper-wrapper">
                                <div class="swiper-slide"><img class="swiper-lazy" data-src="https://imgd.aeplcdn.com//958x539//bw/ec/22012/Honda-CB-Shine-Side-66791.jpg?1" alt="1" border="0" /><div class="swiper-lazy-preloader swiper-lazy-preloader-white"></div></div>
                                <div class="swiper-slide"><img class="swiper-lazy" data-src="https://imgd.aeplcdn.com//958x539//bikewaleimg/ec/15504/img/l/Bajaj-Pulsar-RS200-Front-three-quarter-50256.jpg?2" alt="2" border="0" /><div class="swiper-lazy-preloader swiper-lazy-preloader-white"></div></div>
                                <div class="swiper-slide"><img class="swiper-lazy" data-src="https://imgd.aeplcdn.com//958x539//bw/ec/19963/Honda-Activa-3G-Side-64339.jpg?3" alt="3" border="0" /><div class="swiper-lazy-preloader swiper-lazy-preloader-white"></div></div>
                                <div class="swiper-slide"><img class="swiper-lazy" data-src="https://imgd.aeplcdn.com//958x539//bw/ec/22012/Honda-CB-Shine-Front-threequarter-66794.jpg?4" alt="4" border="0" /><div class="swiper-lazy-preloader swiper-lazy-preloader-white"></div></div>
                                <div class="swiper-slide"><img class="swiper-lazy" data-src="https://imgd.aeplcdn.com//958x539//bw/ec/19963/Honda-Activa-3G-Front-threequarter-64338.jpg?5" alt="5" border="0" /><div class="swiper-lazy-preloader swiper-lazy-preloader-white"></div></div>
                            </div>
                            <!-- Add Pagination -->
                            <div class="swiper-pagination"></div>
                            <div class="swiper-button-next swiper-button-white"></div>
                            <div class="swiper-button-prev swiper-button-white"></div>
                        </div>
                        <%--<div id="wrapper">
                            <div id="myCarousel" data-carousel-3d>
                                <img src="https://imgd.aeplcdn.com//958x539//bw/ec/22012/Honda-CB-Shine-Side-66791.jpg?1" alt="1" />
                                <img src="https://imgd.aeplcdn.com//958x539//bikewaleimg/ec/15504/img/l/Bajaj-Pulsar-RS200-Front-three-quarter-50256.jpg?2" alt="2" />
                                <img src="https://imgd.aeplcdn.com//958x539//bw/ec/19963/Honda-Activa-3G-Side-64339.jpg?3" alt="3" />
                                <img src="https://imgd.aeplcdn.com//958x539//bw/ec/22012/Honda-CB-Shine-Front-threequarter-66794.jpg?4" alt="4" />
                                <img src="https://imgd.aeplcdn.com//958x539//bw/ec/19963/Honda-Activa-3G-Front-threequarter-64338.jpg?5" alt="5" />
                            </div>
                        </div>--%>
                        <%--<div class="jcarousel-wrapper">
                            <div class="jcarousel">
                                <ul>
                                    <li>
                                        <div>
                                            <img src="https://imgd.aeplcdn.com//958x539//bw/ec/22012/Honda-CB-Shine-Side-66791.jpg?1" alt="1" />
                                        </div>
                                    </li>
                                    <li>
                                        <div>
                                            <img src="https://imgd.aeplcdn.com//958x539//bikewaleimg/ec/15504/img/l/Bajaj-Pulsar-RS200-Front-three-quarter-50256.jpg?2" alt="2" />
                                        </div>
                                    </li>
                                    <li>
                                        <div>
                                            <img class="lazy" data-original="https://imgd.aeplcdn.com//958x539//bw/ec/19963/Honda-Activa-3G-Side-64339.jpg?3" alt="3" src="" />
                                        </div>
                                    </li>
                                    <li>
                                        <div>
                                            <img class="lazy" data-original="https://imgd.aeplcdn.com//958x539//bw/ec/22012/Honda-CB-Shine-Front-threequarter-66794.jpg?4" alt="4" src="" />
                                        </div>
                                    </li>
                                    <li>
                                        <div>
                                            <img class="lazy" data-original="https://imgd.aeplcdn.com//958x539//bw/ec/19963/Honda-Activa-3G-Front-threequarter-64338.jpg?5" alt="5" src="" />
                                        </div>
                                    </li>
                                </ul>
                            </div>
                            <a href="#" class="bwsprite jcarousel-control-prev inactive" rel="nofollow"></a>
                            <a href="#" class="bwsprite jcarousel-control-next" rel="nofollow"></a>
                        </div>--%>
                        <%--<div id="imageFlow">
                            <div class="text">
                                <div class="title">Loading</div>
                                <div class="legend">Please wait...</div>
                            </div>
                            <div class="scrollbar">
                                <img class="track" src="" alt="">
                                <img class="arrow-left" src="/images/arrow-left-test.png" alt="">
                                <img class="arrow-right" src="/images/arrow-right-test.png" alt="">
                                <img class="bar" src="" alt="">
                            </div>
                        </div>--%>
                    </div>

                    <div id="main-video-content" data-bind="visible: !photosTabActive()">
                        Videos
                    </div>
                </div>

                <div class="gallery-footer">
                    <div class="footer-tabs-wrapper">
                        <div class="footer-tab">
                            <span class="bwsprite info-icon"></span>
                            <span class="inline-block font14">Know more about the bike</span>
                        </div>

                        <div class="footer-tab">
                            <span class="bwsprite color-palette"></span>
                            <span class="inline-block font14">Colours</span>
                        </div>

                        <div data-bind="visible: photosTabActive()" class="footer-tab">
                            <span class="bwsprite grid-icon"></span>
                            <span class="inline-block font14">All photos</span>
                        </div>                        

                        <div data-bind="visible: !photosTabActive()" class="footer-tab">
                            <span class="bwsprite grid-icon"></span>
                           <span class="inline-block font14">All videos</span>
                        </div>

                        <div class="clear"></div>
                    </div>
                </div>
            </div>
        </div>

        <!-- #include file="/includes/footerBW.aspx" -->

        <link href="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : "" %>/css/bw-common-btf.css?<%=staticFileVersion %>" rel="stylesheet" type="text/css" />
        <!-- #include file="/includes/footerscript.aspx" -->
        <script type="text/javascript">
            var photoCount = <%= vmModelPhotos.totalPhotosCount + 1%>;
        </script>
        <script type="text/javascript" src="<%= staticUrl != "" ? "https://st1.aeplcdn.com" + staticUrl : "" %>/src/photos.js?<%=staticFileVersion %>"></script>
        <!-- #include file="/includes/fontBW.aspx" -->
    </form>
</body>
</html>
