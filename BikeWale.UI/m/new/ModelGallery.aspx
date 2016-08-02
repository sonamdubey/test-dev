<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.New.ModelGalleryPage" EnableViewState="false" Trace="false" %>
<%@ Register Src="~/m/controls/ModelGallery.ascx" TagPrefix="BW" TagName="ModelGallery" %>
<!DOCTYPE html>
<html>
<head>
    <%
        title = String.Format("{0} Photos - BikeWale", bikeName);
        keywords = string.Format("{0} photos, {0} pictures, {0} images, {1} {0} photos", modelName, makeName);
        description = String.Format("View pictures of {0} in different colours and angles. Check out {2} photos of {1} on BikeWale", modelName, bikeName, imgCount);
        canonical = String.Format("http://www.bikewale.com/{0}-bikes/{1}/photos/", makeMaskingName,modelMaskingName);
     %>
    <!-- #include file="/includes/headscript_mobile.aspx" -->
    <link href="<%= staticUrl != string.Empty ? "http://st2.aeplcdn.com" + staticUrl : string.Empty %>/m/css/model-gallery.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">

    <BW:ModelGallery ID="ctrlModelGallery" runat="server" />
         
    <!-- #include file="/includes/footerscript_Mobile.aspx" -->
    <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/src/model-gallery.js?<%= staticFileVersion %>"></script>
    </form>
</body>
</html>
