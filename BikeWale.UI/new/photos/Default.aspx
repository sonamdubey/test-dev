<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.New.PhotoGallery.BikePhotos" Debug="false" EnableViewState="false" %>

<%@ Register TagPrefix="PG" TagName="PhotoGallary" Src="/controls/PhotoGallaryMin.ascx" %>
<%
    title = String.Format("{0} {1} Images | {1} Photos - BikeWale", makename, modelName);
    keywords = string.Format("{0} photos, {0} pictures, {0} images, {1} {0} photos", modelName, makename);
    description = String.Format("View images of {0} in different colors and angles. Check out {2} photos of {1} on BikeWale", modelName, bikeName, photoGallary.FetchedCount);
    canonical = String.Format("https://www.bikewale.com/{0}-bikes/{1}/images/", objModelEntity.MakeBase.MaskingName, objModelEntity.MaskingName); 
    alternate = string.Format("https://www.bikewale.com/m/{0}-bikes/{1}/images/", objModelEntity.MakeBase.MaskingName, objModelEntity.MaskingName);
    AdId = "1395986297721";
    AdPath = "/1017752/BikeWale_New_";
    ShowTargeting = "1";
    TargetedModel = objModelEntity.ModelName;
    isAd300x250_BTFShown = false;
%>
<!-- #include file="/includes/headNew.aspx" -->

<script runat="server">	
    string staticUrl1 = System.Configuration.ConfigurationManager.AppSettings["staticUrl"];
    
</script>
<link rel="stylesheet" type="text/css" href="<%= !String.IsNullOrEmpty(staticUrl1) ? "https://st2.aeplcdn.com" + staticUrl1 : "" %>/css/jquery.ad-gallery.css" />
<link type="text/css" href="<%= !String.IsNullOrEmpty(staticUrl1) ? "https://st2.aeplcdn.com" + staticUrl1 : "" %>/css/css-research-photos.css?<%= staticFileVersion %>" rel="Stylesheet" />
<style type="text/css">
    .font26 {
        font-size: 26px !important;
        font-weight: normal !important;
        color: #333 !important;
    }
</style>


<!--[if IE 7]><style type="text/css">#galleryHolder{width:600px;height:400px;*height:475px;margin-bottom:10px;}.ad-gallery{margin: 0;*margin: 10px 0 0 0;padding:0;}</style><![endif]-->
<script type="text/javascript" src="<%= !String.IsNullOrEmpty(staticUrl1) ? "https://st2.aeplcdn.com" + staticUrl1 : "" %>/src/new/photogallery/jquery.ad-gallery.js"></script>
<script type="text/javascript">
    var MakeId = '<%= objModelEntity.MakeBase.MakeId%>';
    var ModelId = '<%= objModelEntity.ModelId%>';
    var MakeName = '<%=objModelEntity.MakeBase.MakeName %>';
    var ModelName = '<%=objModelEntity.ModelName %>';
    var MainCategory = '0';
</script>
<script type="text/javascript" src="<%= !String.IsNullOrEmpty(staticUrl1) ? "https://st2.aeplcdn.com" + staticUrl1 : "" %>/src/new/photogallery/image-gallery.js?<%= staticFileVersion %>"></script>
<div class="container_12">
    <div class="grid_12 margin-bottom15">
        <ul class="breadcrumb">
            <li>You are here: </li>
            <li><a href="/">Home</a></li>
            <li class="fwd-arrow">&rsaquo;</li>
            <li><a href="/new/">New Bikes</a></li>
            <li class="fwd-arrow">&rsaquo;</li>
            <li><a href="/<%= objModelEntity.MakeBase.MaskingName%>-bikes/"><%=objModelEntity.MakeBase.MakeName %> Bikes</a></li>
            <li class="fwd-arrow">&rsaquo;</li>
            <li><a href="/<%= objModelEntity.MakeBase.MaskingName%>-bikes/<%= objModelEntity.MaskingName%>/"><%=objModelEntity.MakeBase.MakeName + " " + objModelEntity.ModelName %></a></li>
            <li class="fwd-arrow">&rsaquo;</li>
            <li class="current"><strong>Images</strong></li>
        </ul>
        <div class="clear"></div>
    </div>
    <div class="grid_12">
        <PG:PhotoGallary ID="photoGallary" runat="server"></PG:PhotoGallary>
    </div>
    <div class="grid_4">
        <div class="top-spacing">&nbsp;</div>
        <!-- BikeWale_NewBike/BikeWale_NewBike_HP_300x250 -->
        <!-- #include file="/ads/Ad300x250.aspx" -->
    </div>
</div>
<!-- #include file="/includes/footerInner.aspx" -->
