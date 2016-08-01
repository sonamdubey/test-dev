<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.m.New.ModelGalleryPage" EnableViewState="false" Trace="false" %>
<%@ Register Src="~/m/controls/ModelGallery.ascx" TagPrefix="BW" TagName="ModelGallery" %>
<!DOCTYPE html>
<html>
<head>
    <%
        title = String.Format("{0} Photos - Bikewale", bikeName );
        keywords = string.Format("{0} 3G photos, {0} 3G pictures, {0} 3G pics, {1} 3G photos, {1} 3G pictures, {1} 3G pics,", bikeName, modelName);
        description = String.Format("View pictures of {0}. This {0} picture clearly shows you how {1} looks like.",bikeName, modelName );
        canonical = String.Format("http://www.bikewale.com/{0}-bikes/{1}/photos/", makeMaskingName,modelMaskingName);
     %>
    <!-- #include file="/includes/headscript_mobile.aspx" -->
    <link href="<%= staticUrl != string.Empty ? "http://st2.aeplcdn.com" + staticUrl : string.Empty %>/m/css/model-gallery.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <section class="model-gallery-container">
        <h1 class="font16 text-white"><%=bikeName %> Photos</h1>
        <div class="gallery-close-btn position-abt pos-top15 pos-right15 bwmsprite cross-md-white cur-pointer"></div>

        <div class="bw-tabs-panel">
            <ul class="bw-tabs horizontal-tabs-wrapper">
                <li class="active" data-tabs="photos" id="photos-tab">Photos</li>
                <li data-tabs="videos" id="videos-tab">Videos</li>
            </ul>
            <BW:ModelGallery ID="ctrlModelGallery" runat="server" />
        </div>

    </section>    
    <!-- #include file="/includes/footerscript_Mobile.aspx" -->
    <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/src/model-gallery.js?<%= staticFileVersion %>"></script>
    </form>
</body>
</html>
