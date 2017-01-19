<%@ Page Language="C#" AutoEventWireup="false" EnableViewState="false" Inherits="Bikewale.Mobile.New.Photos.Default" %>
<%@ Register Src="~/m/controls/ModelGallery.ascx" TagPrefix="BW" TagName="ModelGallery" %>
<%@ Register Src="/m/controls/NewVideosWidget.ascx" TagName="Videos" TagPrefix="BW" %>
<%@ Register TagPrefix="BW" TagName="GenericBikeInfo" Src="~/m/controls/GenericBikeInfoControl.ascx" %>
<%@ Register TagPrefix="BW" TagName="SimilarBikeWithPhotos" Src="~/m/controls/SimilarBikeWithPhotos.ascx" %>
<!DOCTYPE html>
<html>
<head>
   <%  if (vmModelPhotos != null && vmModelPhotos.pageMetas != null) {
            title = vmModelPhotos.pageMetas.Title;
            keywords = vmModelPhotos.pageMetas.Keywords;
            description = vmModelPhotos.pageMetas.Description;
            canonical = vmModelPhotos.pageMetas.CanonicalUrl;
            EnableOG = true;
            OGImage = vmModelPhotos.modelImage; 
        }
       
     %>
    <!-- #include file="/includes/headscript_mobile_min.aspx" -->
    <link rel="stylesheet" type="text/css" href="/m/css/photos.css" />
    <script type="text/javascript">
        <!-- #include file="\includes\gacode_mobile.aspx" -->
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <!-- #include file="/includes/headBW_Mobile.aspx" -->
         <% if (vmModelPhotos != null)
           {
               var objImages = vmModelPhotos.objImageList; %>
        <section>
            <div class="container box-shadow section-bottom-margin">
                <h1 class="section-header bg-white"><%= vmModelPhotos.bikeName %> Photos</h1>
                 <% int i = 0; if (vmModelPhotos.totalPhotosCount > 0)
                   { %>
                <ul class="photos-grid-list">
                     <% while (i < vmModelPhotos.gridPhotosCount - 1 && i < 6) //to handle lazy load for initial images (6 images can vary) 
                       { %>
                    <li>
                        <img src="<%= Bikewale.Utility.Image.GetPathToShowImages(objImages[i].OriginalImgPath,objImages[i].HostUrl,Bikewale.Utility.ImageSize._476x268) %>"  alt="<%= objImages[i].ImageCategory %> Image"  title="<%= objImages[i++].ImageCategory %>"/>
                    </li>
                    <% } %>
                     <% while (i < vmModelPhotos.gridPhotosCount && i < vmModelPhotos.gridSize)
                       { %>
                    <li>
                        <img class="lazy" data-original="<%= Bikewale.Utility.Image.GetPathToShowImages(objImages[i].OriginalImgPath,objImages[i].HostUrl,Bikewale.Utility.ImageSize._476x268) %>" src="" alt="<%= objImages[i].ImageCategory %> Image" title="<%= objImages[i++].ImageCategory %>" />
                    </li>
                <% }  %>

                </ul>
                 <% if (vmModelPhotos.totalPhotosCount < vmModelPhotos.gridSize && vmModelPhotos.nongridPhotosCount > 0) { %>
                <ul class="photos-grid-list photos-remainder-<%= vmModelPhotos.nongridPhotosCount %> remainder-grid-list">
                    <% while (i < vmModelPhotos.totalPhotosCount && i < vmModelPhotos.gridSize)
                         { %>
                    <li>
                         <img class="lazy" data-original="<%= Bikewale.Utility.Image.GetPathToShowImages(objImages[i].OriginalImgPath,objImages[i].HostUrl,Bikewale.Utility.ImageSize._476x268) %>" src="" alt="<%= objImages[i].ImageCategory %> Image" title="<%= objImages[i++].ImageCategory %>" />
                    </li>
                     <% } %>
                </ul>
                <% } } %>
                <div class="clear"></div>
            </div>
        </section>
       <% if(!isUpcoming) { %>
        <section>
            <div class="container bg-white box-shadow padding-15-20 section-bottom-margin">
                <h2 class="margin-bottom15">Know more about this bike</h2>
                   <BW:GenericBikeInfo  ID="ctrlGenericBikeInfo" runat="server" />
            </div>
        </section>
         <% } %>
        <% } %>

    <%if (ctrlVideos.FetchedRecordsCount > 0)
        { %>
        <section>
            <div class="container bg-white box-shadow padding-15-20 section-bottom-margin">
                    <h2 class="margin-bottom15"><%= vmModelPhotos.bikeName %> Videos</h2>
                    <BW:Videos runat="server" ID="ctrlVideos" />
            </div>
        </section>
    <% } %>

         <BW:SimilarBikeWithPhotos  ID="ctrlSimilarBikesWithPhotos" runat="server" />
         <BW:ModelGallery ID="ctrlModelGallery" runat="server" />
        <!-- model-gallery-container ends here -->


        <script type="text/javascript" src="<%= staticUrl != "" ? "https://st1.aeplcdn.com" + staticUrl : "" %>/m/src/frameworks.js?<%= staticFileVersion %>"></script>
        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <link href="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : "" %>/m/css/bwm-common-btf.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
        <!-- #include file="/includes/footerscript_mobile.aspx" -->
        <script type="text/javascript">
            var photoCount = <%= vmModelPhotos!=null ?  vmModelPhotos.totalPhotosCount : 0 %>;
        </script>
        <script type="text/javascript" src="<%= staticUrl != "" ? "https://st1.aeplcdn.com" + staticUrl : "" %>/m/src/photos.js?<%= staticFileVersion %>"></script>
        <!-- #include file="/includes/fontBW_Mobile.aspx" -->
    </form>
</body>
</html>
