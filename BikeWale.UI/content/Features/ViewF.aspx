<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Content.ViewF" Trace="false" Debug="false" Async="true" %>
<%@ Register TagPrefix="CE" TagName="CalculateEMIMin" Src="/controls/CalculateEMIMin.ascx" %>
<%@ Register TagPrefix="uc" TagName="InstantBikePrice" Src="/controls/instantbikeprice.ascx" %>
<%@ Register TagPrefix="PG" TagName="PhotoGallery" Src="/controls/ArticlePhotoGallery.ascx" %>
<% 
    title = articleTitle  + " - Bikewale " ;
    keywords = "features, stories, travelogues, specials, drives";
    description = "";  
    canonical = "http://www.bikewale.com" + canonicalUrl;
    AdId = "1395986297721";
    AdPath = "/1017752/BikeWale_New_";
    alternate = "http://www.bikewale.com/m" + canonicalUrl;
%>
<!-- #include file="/includes/headNew.aspx" -->
<style>
    .block-spacing {margin-top:10px;}
    .block-spacing ul{list-style:none;width:620px;overflow:hidden;}
    .block-spacing li{float:left;padding:5px 5px;border-right : 1px solid #999;background-color:#f3f2f2; } 
    .block-spacing li a{ font-weight:bold; text-decoration:none;   *position:relative; *height:25px; *display:inline-block; *top:-10px;cursor:pointer;}
    .text-bold { font-weight:bold; font-size:14px; color:#000;}
     /* photo gallery classes */
    #image-gallery{white-space:nowrap;overflow:hidden;}
    #image-gallery li {display:inline-block; margin-right:10px; vertical-align: top; white-space: normal;}
    #image-gallery li .thumb {height:110px; background: #f0f0f0;}
    #image-gallery li .thumb img { height:110px; }
    #image-gallery li a:hover {text-decoration:none;color:#034FB6;}
    #image-gallery li:hover h4 {color:#034FB6;}
    #gallery .jcarousel-skin-tango { overflow:hidden; height:110px; }
    #gallery .jcarousel-skin-tango .jcarousel-container-horizontal { width: 628px; *width: 630px; height:auto!important;}
    #gallery .jcarousel-skin-tango .jcarousel-clip {overflow: hidden;}
    #gallery .jcarousel-skin-tango .jcarousel-clip-horizontal {width: 630px;height:auto!important;}
    #gallery .jcarousel-skin-tango .jcarousel-item { width: 165px; overflow:hidden; background:#282626; text-align:center; }
    #gallery .jcarousel-skin-tango .jcarousel-item-horizontal {margin-left: 0;margin-right: 10px;}
    #gallery .jcarousel-skin-tango .jcarousel-next-horizontal, 
    #gallery .jcarousel-skin-tango .jcarousel-prev-horizontal { cursor: pointer; height: 110px; position: absolute; top: 0px; width: 15px; opacity:.8; filter: alpha(opacity=80); }
    #gallery .jcarousel-skin-tango .jcarousel-next-horizontal:hover, 
    #gallery .jcarousel-skin-tango .jcarousel-prev-horizontal:hover { opacity:1; filter: alpha(opacity=100); }
    #gallery .jcarousel-skin-tango .jcarousel-next-horizontal { background: #282626 url("http://img.carwale.com/adgallery/ad_scroll_forward.png") no-repeat center center; right: 10px; }
    #gallery .jcarousel-skin-tango .jcarousel-prev-horizontal { background: #282626 url("http://img.carwale.com/adgallery/ad_scroll_back.png") no-repeat  center center; left: 0px; }

    .article-content img { width:100%; }

 </style>
<script type="text/javascript" src="/src/common/jquery.colorbox-min.js?v=1.0"></script>
    <div class="container_12">
        <div class="grid_12">             			
			<ul class="breadcrumb">
                <li>You are here: </li>
                <li><a href="/">Home</a></li>
                <li class="fwd-arrow">&rsaquo;</li>
                <li><a href="/features/">Features</a></li>
                <li class="fwd-arrow">&rsaquo;</li>
                <li class="current"><strong><%= articleTitle%> Features</strong></li>
            </ul>		    
        </div>
        <div class="grid_8 margin-top10">	
		
		    <h1><%= articleTitle%></h1>        
		    <span><%= authorName + ", " + Bikewale.Utility.FormatDate.GetDaysAgo(displayDate) %> </span> 
            <div class="clear"></div>
		    <div class="block-spacing" id="topNav" runat="server">		
                <div class="text-bold">
                    Read Pages
                </div>	   
			    <div style="padding:5px 0;">   
				    <asp:Repeater ID="rptPages" runat="server">
                        <headertemplate>
                            <ul>
                             <%--   <li style="border:none;" ><a>Read Pages : </a></li>--%>
                        </headertemplate>
					    <itemtemplate>
                            <li>
                                <a href="#<%#Eval("pageId") %>"><%#Eval("PageName") %></a>
                            </li>
						  <%--  <%# CreateNavigationLink(DataBinder.Eval( Container.DataItem, "Priority" ).ToString(), Url ) %>--%>
					    </itemtemplate>
                        <footertemplate>
                            <li>
                                <a href="#divPhotos">Photos</a>
                            </li>
                            </ul>
                        </footertemplate>
					 <%--   <footertemplate>
						    <% if ( ShowGallery )  { %>
						    <%# CreateNavigationLink( Str, Url ) %>
						    <% } %>	
					    </footertemplate>--%>
				    </asp:Repeater>
			    </div>	
		    </div>
            <div class="margin-top10">
                 <asp:Repeater ID="rptPageContent" runat="server">
					    <itemtemplate>
                            <div class="margin-top10 margin-bottom10">
                                <h3 class="content-block grey-bg"><%#Eval("PageName") %></h3>
                                <div id="<%#Eval("pageId") %>" class="margin-top10 article-content">
                                    <%#Eval("content") %>
                                </div>
                            </div>
					    </itemtemplate>             
				    </asp:Repeater>
            </div>
            <div id="divPhotos">
                <PG:PhotoGallery runat="server" ID="ctrPhotoGallery" />
            </div>
		    <%--<div class="margin-top10">
			    <asp:Label ID="lblDetails" runat="server" />
			    <asp:DataList ID="dlstPhoto" runat="server" RepeatDirection="Horizontal" RepeatColumns="3" ItemStyle-VerticalAlign="top">
				    <itemtemplate>
					    <a rel="slidePhoto" target="_blank" href="<%# "http://" + DataBinder.Eval( Container.DataItem, "HostURL" ).ToString() + DataBinder.Eval( Container.DataItem, "ImagePathLarge" ).ToString() %>" title="<b><%# DataBinder.Eval( Container.DataItem, "Caption" ) %></b>" />
						    <img alt="<%# DataBinder.Eval( Container.DataItem, "CategoryName" ) %>" border="0" style="margin:0px 45px 10px 0px;cursor:pointer;" src="<%# "http://" + DataBinder.Eval( Container.DataItem, "HostURL" ).ToString() + DataBinder.Eval( Container.DataItem, "ImagePathThumbNail" ).ToString() %>" title="Click to view larger photo" />
					    </a>
				    </itemtemplate>
			    </asp:DataList>
		    </div>--%>
		    <%--<div class="margin-top10 content-block grey-bg" id="bottomNav" runat="server">
			    <div align="right" style="width:245px;float:right;">
				    <asp:DropDownList ID="drpPages_footer" AutoPostBack="true" CssClass="drpClass" runat="server"></asp:DropDownList>
			    </div>
			    <div style="width:380px; padding:5px 0;">
				    <b>Read Page : </b>
				    <asp:Repeater ID="rptPages_footer" runat="server">
					    <itemtemplate>
						    <%# CreateNavigationLink(DataBinder.Eval( Container.DataItem, "Priority" ).ToString(), Url) %>
					    </itemtemplate>
					    <footertemplate>
						    <% if ( ShowGallery )  { %>
						    <%# CreateNavigationLink( Str, Url ) %>
						    <% } %>	
					    </footertemplate>
				    </asp:Repeater>
			    </div>	
		    </div>--%>
    </div>
        <div class="grid_4"><!--    Right Container starts here -->    
            <%--<div class="margin-top15">
            <!-- BikeWale_NewBike/BikeWale_NewBike_HP_300x250 -->
            <!-- #include file="/ads/Ad300x250.aspx" -->
            </div>--%>	            
            <div class="light-grey-bg content-block border-radius5 margin-top10 padding-bottom20 margin-top15">
                <uc:InstantBikePrice runat="server" ID="ucInstantBikePrice" />
            </div>
            <div class="light-grey-bg content-block border-radius5 margin-top10 padding-bottom20 margin-top15">
                <CE:CalculateEMIMin runat="server" ID="CalculateEMIMin" />
            </div>
            <%--<div class="margin-top15">
            <!-- BikeWale_NewBike/BikeWale_NewBike_HP_300x250 -->
            <!-- #include file="/ads/Ad300x250BTF.aspx" -->
            </div>--%>	
     </div>
   </div>
</form>
<div id="back-to-top" class="back-to-top"><a><span></span></a></div>
<script language="javascript" type="text/javascript">
    $(document).ready(function () {
        //$("a[rel='slidePhoto']").colorbox({
        //});

        var speed = 300;
        //input parameter : id of element, scroll up speed 
        ScrollToTop("back-to-top", speed);
    });
</script>
<!-- #include file="/includes/footerinner.aspx" -->