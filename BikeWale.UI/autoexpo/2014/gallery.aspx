<%@ Page trace="false" debug="false" Language="C#" AutoEventWireUp="false" Inherits="AutoExpo.gallery"  %>
<%@ Import NameSpace="Bikewale.Common" %>
<%@ Register TagPrefix="AutoExpo" TagName="RepeaterPager" src="/autoexpo/controls/RepeaterPagerGallery.ascx" %>
<%@ Register TagPrefix="AutoExpo" TagName="spContent" src="/autoexpo/controls/sponsoredContent.ascx" %>
<%
	// Define all the necessary meta-tags info here.
	// To know what are the available parameters,
	// check page, headerCommon.aspx in common folder.
	
	PageId          = 2;
	Title 			= "Auto Expo 2014 - BikeWale";
	Description 	= "BikeWale's coverage on Auto Expo 2014, the largest auto show in India.";
	Keywords		= "auto expo, auto expo 2014, auto show india, auto expo delhi";
	Revisit 		= "5";
	DocumentState 	= "Static";
    canonical       = "http://bikewale.com/autoexpo/2014/";
%>
<!-- #include file="/autoexpo/includes/headNews.aspx" -->
<% if ( Request.QueryString["pn"] != null && Request.QueryString["pn"].Length > 0 ) { %>
<META NAME="ROBOTS" CONTENT="NOINDEX, FOLLOW">
<% } %>
<script type="text/javascript" src="/autoexpo/js/jsCarousel-2.0.0.js"></script>
    <style type="text/css">
        
    </style>

<script language="javascript" src="/autoexpo/js/jquery.slideshow.js" type="text/javascript"></script>
<script type="text/javascript">
    $(document).ready(function (e) {
        $('ul.autoexpo-carousel').jcarousel({
            scroll: 2
        });        
    });
</script>
<div class="pthead">
    <h1>Photo Gallery</h1>    
    <div class="clear"></div>
</div>
<div class="left-grid">
<form id="form1" runat="server">
	<AutoExpo:RepeaterPager id="rpgNews" PageSize="10" PagerPageSize="10" runat="server">
		<asp:Repeater ID="rptNews" runat="server" EnableViewState="false">
			<itemtemplate>
				<div class="entry">
                    <script type="text/javascript">
                        $(document).ready(function () {
                            $('a.gallery<%# DataBinder.Eval(Container.DataItem, "BasicId") %>').colorbox({ rel: 'gallery<%# DataBinder.Eval(Container.DataItem, "BasicId") %>' });
                        });
                    </script>
                    <div id="post-<%# DataBinder.Eval(Container.DataItem,"BasicId") %>" class="content-box">
                        <h3><%# DataBinder.Eval(Container.DataItem,"Title")%></h3>
                        <div class="data-box"> 
                            <div id="gallery">                                    
                                <ul id="image-gallery" class="autoexpo-carousel jcarousel-skin-tango" style="width:10000px !important;">
                                    <asp:Repeater id="rptThumbnail" datasource='<%# GetImages(DataBinder.Eval(Container.DataItem,"BasicId").ToString()) %>'  runat="server">
                                        <ItemTemplate>
                                            <li>                                           	                                
                                                <%--<%# "<a class='cbBox"+DataBinder.Eval(Container.DataItem,"BasicId")+"' rel='slide"+DataBinder.Eval(Container.DataItem,"BasicId")+"' href='" + AutoExpo.Common.ImagingFunctions.GetImagePath("/ec/", DataBinder.Eval(Container.DataItem, "HostUrl").ToString()) + DataBinder.Eval(Container.DataItem, "BasicId") + "/img/l/" + DataBinder.Eval(Container.DataItem, "Id") + ".jpg'><img class='alignleft size-thumbnail img-border-news' src='" + AutoExpo.Common.ImagingFunctions.GetImagePath("/ec/", DataBinder.Eval(Container.DataItem, "HostURL").ToString()) + DataBinder.Eval(Container.DataItem, "BasicId") + "/img/t/" + DataBinder.Eval(Container.DataItem, "Id") + ".jpg' align='right' border='0' /></a>"%>--%>
                                                <%# "<a class='gallery"+DataBinder.Eval(Container.DataItem,"BasicId")+"' rel='gallery"+DataBinder.Eval(Container.DataItem,"BasicId")+"' href='http://"+Eval("HostUrl").ToString() + DataBinder.Eval(Container.DataItem, "ImagePathLarge").ToString()+"'><img class='thumb' src= 'http://"+Eval("HostUrl").ToString() + DataBinder.Eval(Container.DataItem, "ImagePathThumbnail").ToString()+"'  border='0' /></a>"%>
                                            </li>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ul>
                            </div>
                            <div class="clear"></div>
                        </div>
		            </div>
                    <div class="clear"></div>
	            </div>
                <div class="clear"></div>            
			</itemtemplate>
		</asp:Repeater>		
	</AutoExpo:RepeaterPager></form>  
</div>
<div class="right-grid">
    <div class="content-box">
        <h3>Know More About Brands</h3>
        <div class="brands-list">                	
            <ul>
                <li><a href="/autoexpo/2014/brand.aspx?mid=1">Bajaj</a></li>
                <li><a href="/autoexpo/2014/brand.aspx?mid=8">Hyosung</a></li>
                <li><a href="/autoexpo/2014/brand.aspx?mid=5">Harley Davidson</a></li>
                <li><a href="/autoexpo/2014/brand.aspx?mid=6">Hero</a></li>
                <li><a href="/autoexpo/2014/brand.aspx?mid=7">Honda</a></li>
                <li><a href="/autoexpo/2014/brand.aspx?mid=13">Yamaha</a></li>
                <li><a href="/autoexpo/2014/brand.aspx?mid=10">Mahindra</a></li>
                <li><a href="/autoexpo/2014/brand.aspx?mid=12">Suzuki</a></li>
                <li><a href="/autoexpo/2014/brand.aspx?mid=22">Triumph</a></li>
                <li><a href="/autoexpo/2014/brand.aspx?mid=15">TVS</a></li>
            </ul>
            <div class="clear"></div>
                    
        </div>
    </div>
</div>
<div style="clear:both;"></div>
<!-- #include file="/autoexpo/includes/footer.aspx" -->
