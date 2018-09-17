<%@ Page Language="C#" Inherits="Bikewale.Used.ViewAlbum" AutoEventWireUp="false"  Trace="false" Debug="false" %>
<%@ Import Namespace="Bikewale.Common" %>
<html>
<head>
    <link rel="stylesheet" type="text/css" href="/UI/css/style.css" />
    <style type="text/css">
        .ad-gallery{width:675px; background-color:#3c3c3c;}
        .ad-gallery,.ad-gallery *{margin:0;padding:0}
        .ad-gallery .ad-image-wrapper{width:100%;height:430px;margin-bottom:10px;position:relative;overflow:hidden;}
        .ad-gallery .ad-image-wrapper .ad-loader{position:absolute;z-index:10;top:48%;left:48%;border:1px solid #CCC}
        .ad-gallery .ad-image-wrapper .ad-next{position:absolute;right:0;top:0;width:25%;height:100%;cursor:pointer;display:block;z-index:100}
        .ad-gallery .ad-image-wrapper .ad-prev{position:absolute;left:0;top:0;width:25%;height:100%;cursor:pointer;display:block;z-index:100}
        .ad-gallery .ad-image-wrapper .ad-prev,.ad-gallery .ad-image-wrapper .ad-next{background:url(https://img.aeplcdn.com/adgallery/non-existing.jpg)\9}
        .ad-gallery .ad-image-wrapper .ad-prev .ad-prev-image,.ad-gallery .ad-image-wrapper .ad-next .ad-next-image{background:url(https://imgd.aeplcdn.com/0x0/bw/static/design15/old-images/d/ad-prev.png);width:30px;height:30px;display:none;position:absolute;top:47%;left:0;z-index:101}
        .ad-gallery .ad-image-wrapper .ad-next .ad-next-image{background:url(https://imgd.aeplcdn.com/0x0/bw/static/design15/old-images/d/ad-next.png);width:30px;height:30px;right:0;left:auto}
        .ad-gallery .ad-image-wrapper .ad-image{position:absolute;overflow:hidden;top:0;left:0;z-index:9}
        .ad-gallery .ad-image-wrapper .ad-image a img{border:0}
        .ad-gallery .ad-image-wrapper .ad-image .ad-image-description{position:absolute;bottom:0;left:0;text-align:left;width:100%;z-index:2;background:url(https://imgd.aeplcdn.com/0x0/bw/static/design15/old-images/d/opa75.png);color:#000;padding:7px; display:none;}
        * html .ad-gallery .ad-image-wrapper .ad-image .ad-image-description{background:none;filter:progid:DXImageTransform.Microsoft.AlphaImageLoader (enabled=true,sizingMethod=scale,src='https://imgd.aeplcdn.com/0x0/bw/static/design15/old-images/d/opa75.png')}
        .ad-gallery .ad-controls{height:20px}
        .ad-gallery .ad-info{float:left; color:#fff;}
        .ad-gallery .ad-slideshow-controls{float:right; color:#fff;}
        .ad-gallery .ad-slideshow-controls .ad-slideshow-start,.ad-gallery .ad-slideshow-controls .ad-slideshow-stop{padding-left:5px;cursor:pointer}
        .ad-gallery .ad-slideshow-controls .ad-slideshow-countdown{padding-left:5px;font-size:.9em}
        .ad-gallery .ad-slideshow-running .ad-slideshow-start{cursor:default;font-style:italic}
        .ad-gallery .ad-nav{width:100%;position:relative}
        .ad-gallery .ad-forward,.ad-gallery .ad-back{position:absolute;top:0;height:100%;z-index:10}
        * html .ad-gallery .ad-forward,.ad-gallery .ad-back{height:100px}
        .ad-gallery .ad-back{cursor:pointer;left:-20px;width:13px;display:block;background:url(https://imgd.aeplcdn.com/0x0/bw/static/design15/old-images/d/ad-scroll-back.png) 0 22px no-repeat}
        .ad-gallery .ad-forward{cursor:pointer;display:block;right:-20px;width:13px;background:url(https://imgd.aeplcdn.com/0x0/bw/static/design15/old-images/d/ad-scroll-forward.png) 0 22px no-repeat}
        .ad-gallery .ad-nav .ad-thumbs{overflow:hidden;width:100%;}
        .ad-gallery .ad-thumbs .ad-thumb-list{float:left;width:9000px;list-style:none}
        .ad-gallery .ad-thumbs li{float:left;padding-right:5px}
        .ad-gallery .ad-thumbs li a img{border:3px solid #CCC;display:block}
        .ad-gallery .ad-thumbs li a.ad-active img{border:3px solid #ffff33;}
        .ad-preloads{position:absolute;left:-9000px;top:-9000px}
        .ad-gallery .ad-image-wrapper .ad-image .ad-image-description .ad-description-title,.ad-gallery .ad-thumbs li a{display:block}
    </style>  
    <script type="text/javascript" src="https://st.carwale.com/jquery-1.7.2.min.js?v=1.0" ></script>
    <script src="<%= staticUrl  %>/UI/src/common/ad-gallery.js?v=1.0" type="text/javascript"></script>    
    <title>View Photographs for Bike Profile #<%= profileNo%></title>	
</head>
<body style="background:none;">
<div id="cw-body">    
    <div class="content-block">
        <h2 class="left-float">Photographs for Bike Profile #<%= profileNo %></h2>
        <span class="readmore pointer"><a onClick="javascript:window.close()" class="right-float" style="margin-right:20px;">Close</a><div class="clear"></div></span>
    </div>
    <div style="padding-left:20px;">
        <div id="gallery" class="ad-gallery">
            <div class="ad-image-wrapper"></div>      
            <div class="ad-controls"></div>
            <div class="ad-nav">
                <div class="ad-thumbs">
                    <ul class="ad-thumb-list">
                        <asp:Repeater ID="rptPhotos" runat="server">
	                        <itemtemplate>
                                <li>
                                    <a rel="slide" href='<%# Bikewale.Utility.Image.GetPathToShowImages( DataBinder.Eval(Container.DataItem, "OriginalImagePath").ToString(), DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._110x61) %>'>
				                        <img alt="Loading..." src='<%# ImagingFunctions.GetPathToShowImages( DataBinder.Eval(Container.DataItem, "OriginalImagePath").ToString(), DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._310x174 ) %>' border="0" />
			                        </a>
                                </li>
	                        </itemtemplate>
                        </asp:Repeater>
                    </ul>
                </div>
            </div>
        </div> 
    </div>  
</div>
<script type="text/javascript">
    $('.ad-gallery').adGallery({ slideshow: {enable: false }});    
</script>	
</body>
</html>