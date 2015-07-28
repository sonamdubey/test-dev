<%@ Page trace="false" debug="false" Language="C#" AutoEventWireUp="false" Inherits="AutoExpo.details"  %>
<%@ Import NameSpace="Bikewale.Common" %>
<%@ Register TagPrefix="AutoExpo" TagName="RepeaterPager" src="/autoexpo/controls/RepeaterPagerNews.ascx" %>
<%@ Register TagPrefix="AutoExpo" TagName="spContent" src="/autoexpo/controls/sponsoredContent.ascx" %>
<%
	// Define all the necessary meta-tags info here.
	// To know what are the available parameters,
	// check page, headerCommon.aspx in common folder.

    PageId          = 1;
	Title 			= NewsTitle;
	Description 	= "Auto Expo 2014 - " + NewsTitle;
	Keywords		= "auto expo, auto expo 2014, auto show india, auto expo delhi";
	Revisit 		= "5";
	DocumentState 	= "Static";   
%>
<!-- #include file="/autoexpo/includes/headNews.aspx" -->
<% if ( Request.QueryString["pn"] != null && Request.QueryString["pn"].Length > 0 ) { %>
<META NAME="ROBOTS" CONTENT="NOINDEX, FOLLOW">
<% } %>
<script type="text/javascript">!function (d, s, id) { var js, fjs = d.getElementsByTagName(s)[0]; if (!d.getElementById(id)) { js = d.createElement(s); js.id = id; js.src = "https://platform.twitter.com/widgets.js"; fjs.parentNode.insertBefore(js, fjs); } }(document, "script", "twitter-wjs");</script>
<script language="javascript" src="/autoexpo/js/jquery.slideshow.js" type="text/javascript"></script>
<form runat="server">
<script type="text/javascript">
    $(document).ready(function () {       
        $("a.cbBox").colorbox({ rel: "nofollow" });
    });
</script>
<ul class="breadcrumb"><li>You are here: </li><li><a href="/autoexpo/2014/">Home</a></li><li class="current">&rsaquo; <strong><%= NewsTitle %></strong></li></ul>
<div class="pthead">
    <h1><%=NewsTitle %></h1>    
    <div class="clear"></div>
    <div class="font11">
        <span><%= Convert.ToDateTime(DisplayDate).ToString("yyyy-MM-ddThh:mm:ss+5:30") %></span>
        <span>by <%= AuthorName %></span>
    </div>
</div>

<div id="content" class="left-grid">	
    <div class="content-box">
        <div class="data-box">
            <div class="social">
                <div class="g-plus-btn">
                    <!-- Place this tag where you want the +1 button to render -->
                    <g:plusone size="medium" href="http://bikewale.com/news/<%= BasicId %>-<%= Url %>.html" count="true"></g:plusone>
                    <!-- Place this tag after the last plusone tag -->
                    <script type="text/javascript">
                        (function () {
                            var po = document.createElement('script'); po.type = 'text/javascript'; po.async = true;
                            po.src = 'https://apis.google.com/js/plusone.js';
                            var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(po, s);
                        })();
                    </script>
                </div>
                <div class="tw-btn">
                    <a href="https://twitter.com/share?text=<%= NewsTitle  %>&via=BikeWale&url=http://bikewale.com/news/<%= BasicId %>-<%= Url %>.html&counturl=http://bikewale.com/news/<%= BasicId %>-<%= Url %>.html" class="twitter-share-button" data-lang="en">Tweet</a>
                    <%--<iframe allowtransparency="true" frameborder="0" scrolling="no" src="http://platform.twitter.com/widgets/tweet_button.html?text=<%= NewsTitle  %>&via=BikeWale&url=http://bikewale.com/news/<%= BasicId %>-<%= Url %>.html&counturl=http://bikewale.com/news/<%= BasicId %>-<%= Url %>.html" style="width:110px; height:40px;"></iframe>--%>
                </div>
                <div class="fb-btn">
                    <iframe src="//www.facebook.com/plugins/like.php?href=http://bikewale.com/news/<%= BasicId %>-<%= Url %>.html&amp;width=10&amp;layout=button_count&amp;action=like&amp;show_faces=false&amp;height=80" scrolling="no" frameborder="0" style="border:none; overflow:hidden; height:20px;" allowTransparency="true"></iframe>
		        </div>
                <div class="clear"></div>
            </div>
            <div>
                <%= Details %>
            </div>
        </div>
    </div>	
</div>
<div class="clear"></div>
</form>
<script language="javascript">
	$("a[rel='slide']").colorbox({width:"700px", height:"500px"});
</script>
<!-- #include file="/autoexpo/includes/footer.aspx" -->


	  