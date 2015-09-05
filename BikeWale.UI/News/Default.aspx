<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.News.Default" Trace="false" EnableViewState="false" Async="true" %>
<%@ Import NameSpace="Bikewale.Common" %>
<%@ Register TagPrefix="RP" TagName="RepeaterPager" src="/News/RepeaterPagerNews.ascx" %>
<%@ Register TagPrefix="BP" TagName="InstantBikePrice" Src="/controls/instantbikeprice.ascx" %>
<%@ Register TagPrefix="CE" TagName="CalculateEMIMin" Src="/controls/CalculateEMIMin.ascx" %>
<%@ Register TagPrefix="BikeWale" TagName="RepeaterPager" Src="/controls/LinkPagerControl.ascx" %>
<%
    title 			= "Bike News - Latest Indian Bike News & Views | BikeWale";
	description 	= "Latest news updates on Indian bikes industry, expert views and interviews exclusively on BikeWale.";
	keywords		= "news, bike news, auto news, latest bike news, indian bike news, bike news of india";
    canonical       = "http://www.bikewale.com/news/";
    prevPageUrl     = prevUrl;
    nextPageUrl     = nextUrl;
    alternate       =  "http://www.bikewale.com/m/news/";
    AdId="1395995626568";
    AdPath="/1017752/BikeWale_News_HomePage_";
%>
<!-- #include file="/includes/headnews.aspx" -->
<script type="text/javascript">
    $(document).ready(function () {
        $("a.cbBox").colorbox({ rel: "nofollow" });
    });
</script>
<form id="form1" runat="server">
<div class="container_12">
    <div class="grid_12">
        <ul class="breadcrumb">
            <li>You are here: </li>
            <li><a href="/">Home</a></li>
            <li class="fwd-arrow">&rsaquo;</li>
            <li class="current"><strong>News</strong></li>
        </ul><div class="clear"></div>
    </div>
    <div id="content" class="grid_8 margin-top10">        
        <h1 class="black-text">Bike News <span>Latest Indian Bikes News and Views</span></h1>        
        <asp:Repeater ID="rptNews" runat="server">
				<itemtemplate>
					<div id='post-<%# DataBinder.Eval(Container.DataItem,"BasicId") %>' class="margin-bottom15">
						<div class="anchor-title">
                            <h2 class="font18"><a href="/news/<%# DataBinder.Eval(Container.DataItem,"BasicId") %>-<%# DataBinder.Eval(Container.DataItem,"ArticleUrl") %>.html" rel="bookmark" title="Permanent Link to <%# DataBinder.Eval(Container.DataItem,"Title") %>">									    
								<%# DataBinder.Eval(Container.DataItem,"Title") %>
							</a></h2>
                        </div>		
                        <div class="grid_5 alpha">
							<abbr><%# DataBinder.Eval(Container.DataItem,"DisplayDate", "{0:f}") %></abbr> by 
							<%# DataBinder.Eval(Container.DataItem,"AuthorName") %>                                                        
						</div>                        
                        <ul class="social">
                            <li><fb:like href="http://www.bikewale.com/news/<%# DataBinder.Eval(Container.DataItem,"BasicId") %>-<%# DataBinder.Eval(Container.DataItem,"ArticleUrl") %>.html" send="false" layout="button_count" width="80" show_faces="false"></fb:like></li>
                            <li><a href="https://twitter.com/share" class="twitter-share-button" data-url="http://www.bikewale.com/news/<%# DataBinder.Eval(Container.DataItem,"BasicId") %>-<%# DataBinder.Eval(Container.DataItem,"ArticleUrl") %>.html" data-via='<%# DataBinder.Eval(Container.DataItem,"Title") %>' data-lang="en">Tweet</a></li>
                            <li><div class="g-plusone" data-size="medium" data-href="http://www.bikewale.com/news/<%# DataBinder.Eval(Container.DataItem,"BasicId") %>-<%# DataBinder.Eval(Container.DataItem,"ArticleUrl") %>.html"></div></li>
                        </ul><div class="clear"></div>
                        <div style="border-top: 1px solid #f0f0f0;"></div>
                        <div class="margin-top10">
						<div class="grid_5 alpha"><%# DataBinder.Eval(Container.DataItem,"Description") %></div>
              			<%--<div class="grid_3 omega"><%#"<a href='/news/" + DataBinder.Eval(Container.DataItem,"BasicId") + "-" + DataBinder.Eval(Container.DataItem,"ArticleUrl") + ".html'><img class='alignright size-thumbnail border-light' style='padding:2px;' src='" + Bikewale.Common.ImagingFunctions.GetPathToShowImages(DataBinder.Eval(Container.DataItem,"SmallPicUrl").ToString(), DataBinder.Eval(Container.DataItem,"HostUrl").ToString()) + "' align='right' border='0' /></a>" %></div>--%>
                            <div class="grid_3 omega"><%#"<a href='/news/" + DataBinder.Eval(Container.DataItem,"BasicId") + "-" + DataBinder.Eval(Container.DataItem,"ArticleUrl") + ".html'><img class='alignright size-thumbnail border-light' style='padding:2px;' src='" + Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem,"OriginalImgUrl").ToString(), DataBinder.Eval(Container.DataItem,"HostUrl").ToString(),Bikewale.Utility.ImageSize._144x81) + "' align='right' border='0' /></a>" %></div>
                        </div><div class="clear"></div>
                        <div class="margin-top10">
						    <div class="grid_7 alpha readmore">
                                <a href="/news/<%# DataBinder.Eval(Container.DataItem,"BasicId") %>-<%# DataBinder.Eval(Container.DataItem,"ArticleUrl") %>.html">Read the rest of this entry &raquo;</a>                            
						    </div>                            
						    <div class="grid_1 omega black-text font11"><%# DataBinder.Eval(Container.DataItem,"Views") %> views</div>
                            <div class="clear"></div>
                        </div>
					</div>
                    <div class="sept-dashed margin-bottom15"></div>
				</itemtemplate>
			</asp:Repeater>		  		
        <BikeWale:RepeaterPager id="linkPager" runat="server"/>       
	</div>
    <div class="grid_4">
        <div class="margin-top15">
            <!-- BikeWale_News/BikeWale_News_300x250 -->
            <!-- #include file="/ads/Ad300x250.aspx" -->
        </div>
        <div class="light-grey-bg content-block border-radius5 padding-bottom20 margin-top15">
            <BP:InstantBikePrice runat="server" ID="InstantBikePrice" />
        </div>        
        <div class="light-grey-bg content-block border-radius5 margin-top10 padding-bottom20">
            <CE:CalculateEMIMin runat="server" ID="CalculateEMIMin" />
        </div>  
        <div class="margin-top15">
            <!-- BikeWale_News/BikeWale_News_300x250 -->
            <!-- #include file="/ads/Ad300x250BTF.aspx" -->
        </div>
    </div>  
</div>
</form>
<script language="javascript">
    //$("a[rel='slide']").colorbox({ width: "700px", height: "500px" });
</script>
<!-- #include file="/includes/footerInner.aspx" -->