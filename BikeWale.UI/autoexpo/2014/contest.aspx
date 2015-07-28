<%@ Page trace="false" debug="false" Language="C#" AutoEventWireUp="false" Inherits="AutoExpo.DefaultClass"  %>
<%@ Import NameSpace="Bikewale.Common" %>
<%@ Register TagPrefix="AutoExpo" TagName="RepeaterPager" src="/autoexpo/controls/RepeaterPagerNews.ascx" %>
<%
	// Define all the necessary meta-tags info here.
	// To know what are the available parameters,
	// check page, headerCommon.aspx in common folder.
	
	PageId 			= 9;    
	Title 			= "Bike News - Latest Indian Bike News & Views | BikeWale";
	Description 	= "Latest news updates on Indian Bike industry, expert views and interviews exclusively on BikeWale.";
	Keywords		= "news, Bike news, auto news, latest bike news, indian bike news, bike news of india";
	Revisit 		= "5";
	DocumentState 	= "Static";
    canonical       = "http://www.bikewale.com/autoexpo/";
%>
<!-- #include file="/autoexpo/includes/headNews.aspx" -->
<% if ( Request.QueryString["pn"] != null && Request.QueryString["pn"].Length > 0 ) { %>
<META NAME="ROBOTS" CONTENT="NOINDEX, FOLLOW">
<% } %>

<form runat="server">
<script src="http://cdn.topsy.com/topsy.js?init=topsyWidgetCreator" type="text/javascript"></script>
<script type"text/javascript">
    $(document).ready(function () {
        $("a.cbBox").colorbox({ rel: "nofollow" });
    });
</script>
<div id="content" class="left-grid">
	<div style="background-color:#fff;padding:2px 0 2px 10px;width:100%;">
        <h1 class="hd1"><%= NewsTitle %></h1>
    </div>
	<AutoExpo:RepeaterPager id="rpgNews" PageSize="10" PagerPageSize="10" runat="server">
		<asp:Repeater ID="rptNews" runat="server" EnableViewState="false">
			<itemtemplate>
				<div class="entry">	
					<div id="post-<%# DataBinder.Eval(Container.DataItem,"BasicId") %>">	
                        <div class="alignleft">
                        <%--<%# DataBinder.Eval(Container.DataItem, "MainImageSet").ToString() == "True" ? "<a class='cbBox' href='" + AutoExpo.Common.ImagingFunctions.GetImagePath("/ec/", DataBinder.Eval(Container.DataItem, "HostURL").ToString()) + DataBinder.Eval(Container.DataItem, "BasicId") + "/img/m/" + DataBinder.Eval(Container.DataItem, "BasicId") + "_l.jpg'><img class='alignleft size-thumbnail img-border-news' src='" + AutoExpo.Common.ImagingFunctions.GetImagePath("/ec/", DataBinder.Eval(Container.DataItem, "HostURL").ToString()) + DataBinder.Eval(Container.DataItem, "BasicId") + "/img/m/" + DataBinder.Eval(Container.DataItem, "BasicId") + "_m.jpg' align='right' border='0' /></a>" : ""%>--%>
                            <img class='size-thumbnail img-border-news' src='/autoexpo/images/8467_m.jpg' align='right' border='0' />
                        </div>													
						<div style="float:right;width:450px;">                                
                            <div style="float:left;">
                                <h2 class="splh2">
								    <a href="/autoexpo/<%# DataBinder.Eval(Container.DataItem,"BasicId") %>-<%# DataBinder.Eval(Container.DataItem,"Url") %>.html" 
									    rel="bookmark" title="Permanent Link to <%# DataBinder.Eval(Container.DataItem,"Title") %>">
									    <%# DataBinder.Eval(Container.DataItem,"Title") %>
								    </a>&nbsp;<%# DataBinder.Eval(Container.DataItem,"AuthorName") %>
							    </h2>
                            </div>
                            <div style="float:right;width:52px;height:43px;background-color:#e3e3e3;text-align:center;">
                                <%# Convert.ToDateTime(DataBinder.Eval(Container.DataItem, "DisplayDate")).ToString("d MMM yyyy") %>
                            </div>
                        <%--<div class="f-small" style="padding:4px 0 0 0;">
							<abbr><%# DataBinder.Eval(Container.DataItem,"DisplayDate", "{0:f}") %></abbr> by 
							<%# DataBinder.Eval(Container.DataItem,"AuthorName") %>
						</div>--%>
                        <ul class="social">
							<li><iframe src="http://www.facebook.com/plugins/like.php?href=http://www.bikewale.com/autoexpo/<%# DataBinder.Eval(Container.DataItem,"BasicId") %>-<%# DataBinder.Eval(Container.DataItem,"Url") %>.html&amp;layout=button_count&amp;show_faces=false&amp;width=80;&amp;action=like&amp;font&amp;colorscheme=light&amp;height=25" scrolling="no" frameborder="0" style="border:none; overflow:hidden; width:80px; height:25px;" allowTransparency="true"></iframe></li>
							<li><iframe allowtransparency="true" frameborder="0" scrolling="no" src="http://platform.twitter.com/widgets/tweet_button.html?text=<%# DataBinder.Eval(Container.DataItem,"Title")  %>&via=BikeWale&url=http://www.bikewale.com/autoexpo/<%# DataBinder.Eval(Container.DataItem,"BasicId") %>-<%# DataBinder.Eval(Container.DataItem,"Url") %>.html&counturl=http://www.bikewale.com/autoexpo/<%# DataBinder.Eval(Container.DataItem,"BasicId") %>-<%# DataBinder.Eval(Container.DataItem,"Url") %>.html" style="width:110px; height:40px;"></iframe></li>
                            <li>
                                <div class="g_plus">
                                    <!-- Place this tag where you want the +1 button to render -->
                                <g:plusone size="medium" href="http://www.bikewale.com/autoexpo/<%# DataBinder.Eval(Container.DataItem,"BasicId") %>-<%# DataBinder.Eval(Container.DataItem,"Url") %>.html" count="true"></g:plusone>
                                <!-- Place this tag after the last plusone tag -->
                                <script type="text/javascript">
                                    (function () {
                                        var po = document.createElement('script'); po.type = 'text/javascript'; po.async = true;
                                        po.src = 'https://apis.google.com/js/plusone.js';
                                        var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(po, s);
                                    })();
                                </script>
                                </div>
                            </li>
						</ul>  <div style="clear:both"></div>
						<%# DataBinder.Eval(Container.DataItem,"Description") %>&nbsp;<a href="/autoexpo/<%# DataBinder.Eval(Container.DataItem,"BasicId") %>-<%# DataBinder.Eval(Container.DataItem,"Url") %>.html">More &raquo;</a>                                                          
						<div style="clear:both;"></div>
								
						</div>
					</div>
				</div><div style="clear:both"></div> 
			</itemtemplate>  
            <AlternatingItemTemplate>
                <div class="entryAlter">	
					<div id="post-<%# DataBinder.Eval(Container.DataItem,"BasicId") %>">	
                        <div class="alignleft">
                        <%--<%# DataBinder.Eval(Container.DataItem, "MainImageSet").ToString() == "True" ? "<a class='cbBox' href='" + AutoExpo.Common.ImagingFunctions.GetImagePath("/ec/", DataBinder.Eval(Container.DataItem, "HostURL").ToString()) + DataBinder.Eval(Container.DataItem, "BasicId") + "/img/m/" + DataBinder.Eval(Container.DataItem, "BasicId") + "_l.jpg'><img class='alignleft size-thumbnail img-border-news' src='" + AutoExpo.Common.ImagingFunctions.GetImagePath("/ec/", DataBinder.Eval(Container.DataItem, "HostURL").ToString()) + DataBinder.Eval(Container.DataItem, "BasicId") + "/img/m/" + DataBinder.Eval(Container.DataItem, "BasicId") + "_m.jpg' align='right' border='0' /></a>" : ""%>--%>
                            <img class='size-thumbnail img-border-news' src='/autoexpo/images/8467_m.jpg' align='right' border='0' />
                        </div>													
						<div style="float:right;width:450px;">                                
                            <div style="float:left;">
                                <h2 class="splh2">
								    <a href="/autoexpo/<%# DataBinder.Eval(Container.DataItem,"BasicId") %>-<%# DataBinder.Eval(Container.DataItem,"Url") %>.html" 
									    rel="bookmark" title="Permanent Link to <%# DataBinder.Eval(Container.DataItem,"Title") %>">
									    <%# DataBinder.Eval(Container.DataItem,"Title") %>
								    </a>&nbsp;<%# DataBinder.Eval(Container.DataItem,"AuthorName") %>
							    </h2>
                            </div>
                            <div style="float:right;width:52px;height:43px;background-color:#fff;text-align:center;">
                                <%# Convert.ToDateTime(DataBinder.Eval(Container.DataItem, "DisplayDate")).ToString("d MMM yyyy") %>
                            </div>
                        <%-- <div class="f-small" style="padding:4px 0 0 0;">
							<abbr><%# DataBinder.Eval(Container.DataItem,"DisplayDate", "{0:f}") %></abbr> by 
							<%# DataBinder.Eval(Container.DataItem,"AuthorName") %>
						</div>--%>
                        <ul class="social">
							<li><iframe src="http://www.facebook.com/plugins/like.php?href=http://www.bikewale.com/autoexpo/<%# DataBinder.Eval(Container.DataItem,"BasicId") %>-<%# DataBinder.Eval(Container.DataItem,"Url") %>.html&amp;layout=button_count&amp;show_faces=false&amp;width=80;&amp;action=like&amp;font&amp;colorscheme=light&amp;height=25" scrolling="no" frameborder="0" style="border:none; overflow:hidden; width:80px; height:25px;" allowTransparency="true"></iframe></li>
							<li><iframe allowtransparency="true" frameborder="0" scrolling="no" src="http://platform.twitter.com/widgets/tweet_button.html?text=<%# DataBinder.Eval(Container.DataItem,"Title")  %>&via=BikeWale&url=http://www.bikewale.com/autoexpo/<%# DataBinder.Eval(Container.DataItem,"BasicId") %>-<%# DataBinder.Eval(Container.DataItem,"Url") %>.html&counturl=http://www.bikewale.com/autoexpo/<%# DataBinder.Eval(Container.DataItem,"BasicId") %>-<%# DataBinder.Eval(Container.DataItem,"Url") %>.html" style="width:110px; height:40px;"></iframe></li>
                            <li>
                                <div class="g_plus">
                                    <!-- Place this tag where you want the +1 button to render -->
                                <g:plusone size="medium" href="http://www.bikewale.com/autoexpo/<%# DataBinder.Eval(Container.DataItem,"BasicId") %>-<%# DataBinder.Eval(Container.DataItem,"Url") %>.html" count="true"></g:plusone>
                                <!-- Place this tag after the last plusone tag -->
                                <script type="text/javascript">
                                    (function () {
                                        var po = document.createElement('script'); po.type = 'text/javascript'; po.async = true;
                                        po.src = 'https://apis.google.com/js/plusone.js';
                                        var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(po, s);
                                    })();
                                </script>
                                </div>
                            </li>
						</ul>  <div style="clear:both"></div>
						<%# DataBinder.Eval(Container.DataItem,"Description") %>&nbsp;<a href="/autoexpo/<%# DataBinder.Eval(Container.DataItem,"BasicId") %>-<%# DataBinder.Eval(Container.DataItem,"Url") %>.html">More &raquo;</a>                                                          
						<div style="clear:both;"></div>								
						</div>
					</div>
				</div><div style="clear:both"></div>
            </AlternatingItemTemplate>              
		</asp:Repeater>		
	</AutoExpo:RepeaterPager>
    <!-- #include file="/autoexpo/includes/slider.aspx" -->
</div>
</form>
<script language="javascript">
	$("a[rel='slide']").colorbox({width:"700px", height:"500px"});
</script>

<!-- #include file="/autoexpo/includes/LogoFooter.aspx" -->
<!-- #include file="/autoexpo/includes/footer.aspx" -->	  