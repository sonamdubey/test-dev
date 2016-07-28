<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Content.DefaultRT" Trace="false" Async="true" %>

<%@ Import Namespace="Bikewale.Common" %>
<%@ Register TagPrefix="BikeWale" TagName="RepeaterPager" Src="/controls/LinkPagerControl.ascx" %>
<%@ Register TagPrefix="Mms" TagName="MakeModelSearch" Src="/Controls/MakeModelSearch.ascx" %>
<%@ Register TagPrefix="CE" TagName="CalculateEMIMin" Src="/controls/CalculateEMIMin.ascx" %>
<%@ Register TagPrefix="uc" TagName="InstantBikePrice" Src="/controls/instantbikeprice.ascx" %>
<%
    title = "Road tests, First drives of New Bikes in India";
    description = "Road testing a bike is the only way to know true capabilities of a bike. Read our road tests to know how bikes perform on various aspects.";
    keywords = "road test, road tests, roadtests, roadtest, bike reviews, expert bike reviews, detailed bike reviews, test-drives, comprehensive bike tests, bike preview, first drives";
    canonical = "http://www.bikewale.com" + "/road-tests/";
    alternate = "http://www.bikewale.com" + "/m/road-tests/";
    prevPageUrl = prevUrl;
    nextPageUrl = nextUrl;
    AdId = "1395986297721";
    AdPath = "/1017752/Bikewale_Reviews_";
%>
<!-- #include file="/includes/headNew.aspx" -->
<style type="text/css">
    #content { margin:0; }
    #content h1 { margin-left:10px; margin-right:10px; }
    .top-breadcrumb { padding-top:20px; margin-right:15px; margin-bottom:10px; margin-left:15px; }
    .article-image-wrapper { width: 206px; margin-right: 20px; float: left; }
    .article-desc-wrapper { width: auto; }
    .article-content { margin-left:10px; padding-top:20px; padding-bottom:20px; border-top:1px solid #e2e2e2; }
    #content > div.article-content:first-of-type { padding-top:0; border-top:0; }
    .article-image-wrapper a { width:100%; height:116px; display:block; }
    .margin-bottom8 { margin-bottom:8px; }
    .calender-grey-icon, .author-grey-icon { width:14px; height:15px; position:relative; top:-2px; margin-right:4px; }
    .calender-grey-icon { background-position:-129px -515px; }
    .author-grey-icon { background-position:-105px -515px; }
    #colorbox { width: 400px !important;height: 400px !important;}
    .article-date { min-width: 164px; padding-right: 10px; }
    .article-author { min-width: 220px; }
    .article-date , .article-author { display: inline-block; vertical-align: middle; }
</style>
<script type="text/javascript" src="<%= staticUrlPath != "" ? "http://st1.aeplcdn.com" + staticUrlPath : "" %>/src/common/jquery.colorbox-min.js?v=1.0"></script>
<div class="container margin-bottom30">
    <div class="grid-12">
        <div class="content-box-shadow padding-bottom20">
            <ul class="breadcrumb top-breadcrumb">
                <li>You are here: </li>
                <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb"><a href="/" itemprop="url"><span itemprop="title">Home</span></a></li>
                <li class="fwd-arrow">&rsaquo;</li>
                <li class="current"><strong>Road Tests</strong></li>
            </ul>
            <div class="clear"></div>
    
            <div id="content" class="grid-8">
                <h1 class="black-text margin-bottom15">Road Tests</h1>
                <Mms:MakeModelSearch ID="MakeModelSearch" RequestType="RoadTest" runat="server" Visible="false"></Mms:MakeModelSearch>
                <div class="alert moz-round" id="alertObj" runat="server" visible="false"></div>
                <asp:repeater id="rptRoadTest" runat="server" enableviewstate="false">
				    <Itemtemplate>					
					    <div id='post-<%# DataBinder.Eval(Container.DataItem,"BasicId") %>' class="article-content">
                            <div class="margin-bottom10">
                                <div class="article-image-wrapper">
                                    <%# !String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem,"OriginalImgUrl").ToString()) ? "<a href='/road-tests/" + DataBinder.Eval(Container.DataItem,"ArticleUrl") + "-" + DataBinder.Eval(Container.DataItem,"BasicId") + ".html'><img src='" + Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem,"OriginalImgUrl").ToString(),DataBinder.Eval(Container.DataItem,"HostURL").ToString(),Bikewale.Utility.ImageSize._210x118) +"' alt='"+ DataBinder.Eval(Container.DataItem,"Title").ToString() +"' title='"+ DataBinder.Eval(Container.DataItem,"Title").ToString() +"' width='100%' border='0' /></a>" : "" %>
                                </div>
                                <div class="article-desc-wrapper">
                                    <h2 class="font14 margin-bottom8">
                                        <a href='/road-tests/<%# DataBinder.Eval(Container.DataItem,"ArticleUrl") %>-<%# DataBinder.Eval(Container.DataItem,"BasicId") %>.html' rel="bookmark" class="text-black text-bold">
                                            <%# DataBinder.Eval(Container.DataItem,"Title").ToString() %>
                                        </a>
                                    </h2>
                                    <div class="font12 text-light-grey margin-bottom25">
                                        <div class="article-date">
                                            <span class="bwsprite calender-grey-icon inline-block"></span>
                                            <span class="inline-block">
                                                <%# Bikewale.Utility.FormatDate.GetFormatDate(DataBinder.Eval(Container.DataItem,"DisplayDate").ToString(),"MMMM dd, yyyy") %>
                                            </span>
                                        </div>
                                        <div class="article-author">
                                            <span class="bwsprite author-grey-icon inline-block"></span>
                                            <span class="inline-block">
                                                <%# DataBinder.Eval(Container.DataItem,"AuthorName") %>
                                            </span>
                                        </div>
                                    </div>
                                    <div class="font14"><%# DataBinder.Eval(Container.DataItem,"Description") %><a href="/road-tests/<%# DataBinder.Eval(Container.DataItem,"ArticleUrl") %>-<%# DataBinder.Eval(Container.DataItem,"BasicId") %>.html">Read full review</a></div>
                                </div>
                                <div class="clear"></div>
                            </div>
                        </div>
			        </Itemtemplate>
		        </asp:repeater>
                <BikeWale:RepeaterPager ID="linkPager" runat="server" />
            </div>
            <div class="grid-4">
        <!--    Right Container starts here -->
        <%--<div class="margin-top15">
            <!-- BikeWale_NewBike/BikeWale_NewBike_HP_300x250 -->
            <!-- #include file="/ads/Ad300x250.aspx" -->
        </div>--%>
        <div class="light-grey-bg content-block border-radius5 padding-bottom20 margin-top15">
            <uc:InstantBikePrice runat="server" ID="ucInstantBikePrice" />
        </div>
        <div class="light-grey-bg content-block border-radius5 padding-bottom20 margin-top10">
            <CE:CalculateEMIMin runat="server" ID="CalculateEMIMin" />
            <div class="clear"></div>
        </div>
        <div>
            <!-- BikeWale_NewBike/BikeWale_NewBike_HP_300x250 -->
            <!-- #include file="/ads/Ad300x250BTF.aspx" -->
        </div>        
    </div>
            <div class="clear"></div>
        </div>
    </div>
    <div class="clear"></div>
</div>
<%--<script type="text/javascript" language="javascript">
    $("a[rel='slide']").colorbox({ width: "700px", height: "500px" });
</script>--%>
<script type="text/javascript" src="<%= staticUrlPath != "" ? "http://st1.aeplcdn.com" + staticUrlPath : "" %>/src/common/jquery.colorbox-min.js?v=1.0"></script>
<script type="text/javascript">
    $(document).ready(function () {
        $(".cboxElement").colorbox({
            rel: 'cboxElement'
        });
    });
</script>
<!-- #include file="/includes/footerInner.aspx" -->
