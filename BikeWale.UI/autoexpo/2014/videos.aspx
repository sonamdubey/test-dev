<%@ Page Language="C#" AutoEventWireup="false" Inherits="AutoExpo.Videos" Trace="false" %>
<%@ Import NameSpace="Bikewale.Common" %>
<%@ Register TagPrefix="AutoExpo" TagName="RepeaterPager" src="/autoexpo/controls/RepeaterPagerGallery.ascx" %>
<%@ Register TagPrefix="AutoExpo" TagName="spContent" src="/autoexpo/controls/sponsoredContent.ascx" %>
<%
	// Define all the necessary meta-tags info here.
	// To know what are the available parameters,
	// check page, headerCommon.aspx in common folder.

    PageId          = 3;
	Title 			= "Auto Expo 2014 Videos";
	Description 	= "Auto Expo 2014 - Videos";
	Keywords		= "auto expo, auto expo 2014, auto show india, auto expo delhi";
	Revisit 		= "5";
	DocumentState 	= "Static";   
%>
<!-- #include file="/autoexpo/includes/headNews.aspx" -->
<% if ( Request.QueryString["pn"] != null && Request.QueryString["pn"].Length > 0 ) { %>
<META NAME="ROBOTS" CONTENT="NOINDEX, FOLLOW">
<% } %>
<script type="text/javascript" src="/autoexpo/js/jsCarousel-2.0.0.js"></script>
    <style type="text/css">
        
    </style>
<script type="text/javascript">
    $().ready(function () {
        $('ul.autoexpo-carousel').jcarousel({
            scroll: 2
        });
        $(".videos-grp").colorbox({
            rel: 'videos-grp',
            iframe: true,
            innerWidth: 640,
            innerHeight: 390,
            current:""
        });
    });
</script>

<script language="javascript" src="/autoexpo/js/jquery.slideshow.js" type="text/javascript"></script>
<div class="pthead">
    <h1>Videos</h1>    
    <div class="clear"></div>
</div>
<div class="left-grid">
<form id="form1" runat="server">
	<AutoExpo:RepeaterPager id="rpgNews" PageSize="10" PagerPageSize="10" runat="server">
		<asp:Repeater ID="rptNews" runat="server" EnableViewState="false">
			<itemtemplate>
				<div class="entry" > 
                    <script type="text/javascript">
                        $(document).ready(function () {
                            $('a.videos-grp<%# DataBinder.Eval(Container.DataItem, "BasicId") %>').colorbox({ rel: 'videos-grp<%# DataBinder.Eval(Container.DataItem, "BasicId") %>' });
                        });
                    </script> 
                    <div id='post-<%# DataBinder.Eval(Container.DataItem,"BasicId") %>' class="content-box">
                        <h3><%#DataBinder.Eval(Container.DataItem,"Title")%></h3>
                        <div class="data-box">
                            <div id="gallery">                                            
                                <ul id="image-gallery" class="autoexpo-carousel jcarousel-skin-tango" style="width:10000px !important;">
                                    <asp:Repeater id="rptThumbnail" datasource='<%# GetVideos(DataBinder.Eval(Container.DataItem,"BasicId").ToString()) %>'  runat="server">
                                        <ItemTemplate>
                                            <li>
                                                <%--<%# "<a class='cbBox"+DataBinder.Eval(Container.DataItem,"BasicId")+"' rel='slide"+DataBinder.Eval(Container.DataItem,"BasicId")+"' href='" + AutoExpo.Common.ImagingFunctions.GetImagePath("/ec/", DataBinder.Eval(Container.DataItem, "HostUrl").ToString()) + DataBinder.Eval(Container.DataItem, "BasicId") + "/img/l/" + DataBinder.Eval(Container.DataItem, "Id") + ".jpg'><img class='alignleft size-thumbnail img-border-news' src='" + AutoExpo.Common.ImagingFunctions.GetImagePath("/ec/", DataBinder.Eval(Container.DataItem, "HostURL").ToString()) + DataBinder.Eval(Container.DataItem, "BasicId") + "/img/t/" + DataBinder.Eval(Container.DataItem, "Id") + ".jpg' align='right' border='0' /></a>"%>--%>
                                                <a class='videos-grp' rel='videos-grp' href='http://www.youtube.com/embed/<%# Eval("VideoId").ToString() %>?rel=0&amp;wmode=transparent'> <img class='alignleft size-thumbnail img-border-news' src= 'http://img.youtube.com/vi/<%# Eval("VideoId").ToString() %>/1.jpg' border='0' /></a>
                                            </li>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ul>                                       
                            </div><div style="clear:both;"></div>
                        </div>
		            </div>
	            </div><div style="clear:both;"></div>            
			</itemtemplate>
		</asp:Repeater>		
	</AutoExpo:RepeaterPager></form>  
</div>
<div style="clear:both;"></div>
<!-- #include file="/autoexpo/includes/footer.aspx" -->
