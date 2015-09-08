<%@ Page Language="C#" Inherits="Bikewale.Videos.video" AutoEventWireup="false" %>
<%   
    AdId = "1395986297721";
    AdPath = "/1017752/BikeWale_New_";
%>
<style>
    #divFb {min-height:150px;}
    #divFb .fb-comments iframe { width:600px !important;height:200px !important; border:1px solid red; display:block;}
 </style>
<!-- #include file="/includes/headNew.aspx" -->
<link href="../css/video.css" rel="stylesheet" />
<div class="container_12">
        <div class="grid_12">
            <ul class="breadcrumb">
                <li>You are here: </li>
                <li><a href="/">Home</a></li>
                <li class="fwd-arrow">&rsaquo;</li>
                <li class="current"><strong>Bike Videos</strong></li>
            </ul><div class="clear"></div>
        </div>
        <div class="grid_8 column">
            <div class="content-block-white margin-top10">
                <h1 class="content-block-video">Videos Title</h1><div class="clear"></div>
                <div class="content-block white-shadow">
                   <iframe width="600" height="315" src="//www.youtube.com/embed/bAQiJYW9R38" frameborder="0" allowfullscreen></iframe>
                    <div class="margin-bottom10 yt-link-btn-area">
                        <div class=" margin-left10 left-float" style="font-size: 16px;margin-top:17px;">
                            <span class="video-sprite review-icon-big"></span>
                            <asp:label id="lblViews" runat="server"></asp:label>
                            Views
                        </div>  
                        <div class="right-float margin-right10 set-width" style="margin-top:17px;font-size:14px !important"> 
                            <div class="right-float relative-position">
                                <a href="#" id="spnLike" class="video-sprite like-video margin-left15 margin-right5">Like</a>
                                <span class="relative-position"><span class="count-box-tail"></span>
                                    <asp:label id="lblLikes" cssclass="count-box" runat="server">1234</asp:label>
                                </span>
                            </div>  
                            <div class="rightfloat ytsubscribe-area">         
                                <div class="g-ytsubscribe  margin-right20" data-channelid="UCMDV6J2hWXet7ZCfgrXGgeg" data-layout="default" data-count="default"></div>   
                            </div>     
                        </div>
                        <div class="clear"></div>                    
                    </div>
                    <div>
                        <asp:label id="lblDescription" runat="server">The Honda City has always been one of the more popular sedans in India. With the shift in the demand for Diesel engines, Honda unveiled the 4th Gen City with what is arguably the most interesting diesel engine. So how good is it?
                        We find out..</asp:label>
                    </div>
                    <div class="clear"></div>
                    <div id="divTags" runat="server" class="margin-top10">
                        <span><strong>Tags: </strong></span>
                        <asp:label id="lblTags" runat="server">oyota, Etios Cross, Toyota Etios Cross</asp:label>
                    </div>
                    <div class="clear"></div>
                    <div id="divFb"  class="margin-top10">
                        <div class="fb-comments" data-href="http://www.carwale.com/honda-cars/city/videos/car-review-13451/" data-width="600" data-height="100" data-colorscheme="light"></div>
                    <div class="clear"></div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Left container starts here -->
       <%-- <div class="grid_8  alpha  margin-top10 column">
            <div class="content-block-white">
            <div class="relative">
    	        <!-- data tabs code starts here-->
                <div class="video-tabs">
        	        <ul>
            	        <li class="active" id="mostpopular"  style="font-size : 14px;">Most Popular</li>
                        <li id="expertreviews"  style="font-size : 14px;">Expert Reviews</li>
                        <li id="interior"  style="font-size : 14px;">Interiors Show</li>
                        <li id="miscel"  style="font-size : 14px;">Miscellaneous</li>
                    </ul>
                    <div class="clear"></div>
                </div>
                <!-- data tabs code ends here-->
                <!-- Most Popular data code starts here-->
                <div class="bike-data-list" id="most-popular">
        	        <ul id="ulmostpopular">
                        <div id="divMostPopular"></div>
        	        </ul>
                    <div class="clear"></div>
                </div>
                <!-- Most Popular data code ends here-->
                <!-- Expert Reviews data code starts here-->
                <div class="bike-data-list hide" id="expert-reviews">
        	        <ul id="ulexpertreviews">
                        <div id="divExpertReviews"></div>
        	        </ul>
                    <div class="clear"></div>
                </div>
                <!-- Expert Reviews data code ends here-->
                <!-- Interiors Show data code starts here-->
                <div class="bike-data-list hide" id="interiors-show">
        	        <ul id="ulinterior">
                        <div id="divInterior"></div>
        	        </ul>
                    <div class="clear"></div>
                </div>
                <!-- Interiors Show data code ends here-->
                <!-- Miscellaneous data code starts here-->
                <div class="bike-data-list hide" id="miscellaneous">
        	        <ul id="ulmiscell">
                        <div id="divMiscell"></div>
        	        </ul>
                    <div class="clear"></div>
                </div>
                <!-- Miscellaneous data code ends here-->
                <div id="loadingmsg" class="hide">
                    <div class="loading-popup">
                        <span class="loading-icon"></span>
                        <p style="font-size: 13px; color: blue;">Please wait, we are fetching the results for you...</p>
                        <div class="clear"></div>
                    </div>
                </div>

            </div>
        </div>
        </div><!-- Left Container ends here -->--%>
   
        <!-- Right Container ends here -->
        <div class="grid_4 " style="margin-top:10px !important">
                 <!-- #include file="/ads/Ad300x250BTF.aspx" -->
        </div><!-- Right Container ends here -->
        <div class="clear"></div>
</div>
<!-- #include file="/includes/footerInner.aspx" -->

