<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Content.DefaultF" Trace="false" Async="true" %>

<%@ Import Namespace="Bikewale.Common" %>
<%@ Register TagPrefix="Mms" TagName="MakeModelSearch" Src="/Controls/MakeModelSearch.ascx" %>
<%@ Register TagPrefix="CE" TagName="CalculateEMIMin" Src="/controls/CalculateEMIMin.ascx" %>
<%@ Register TagPrefix="uc" TagName="InstantBikePrice" Src="/controls/instantbikeprice.ascx" %>
<%@ Register TagPrefix="BikeWale" TagName="RepeaterPager" Src="/controls/LinkPagerControl.ascx" %>
<% 
    title = "Features - Stories, Specials & Travelogues | BikeWale";
    description = "Features section of BikeWale brings specials, stories, travelogues and much more.";
    keywords = "features, stories, travelogues, specials, drives.";
    canonical = "http://www.bikewale.com/features/";
    alternate = "http://www.bikewale.com/m/features/";
    prevPageUrl = prevUrl;
    nextPageUrl = nextUrl;
    AdId = "1395986297721";
    AdPath = "/1017752/BikeWale_New_"; 
%>
<!-- #include file="/includes/headNew.aspx" -->
<style type="text/css">
    #content { margin:0; }
    #content h1 { margin-left:10px; margin-right:10px; }
    .sponsored-tag-wrapper { width: 120px;height: 24px;background: #4d5057; color: #fff; font-size: 12px; line-height: 25px; padding: 0 20px; top:-10px; left:-20px; }
    .sponsored-left-tag {width: 0;height: 0;border-top: 13px solid transparent;border-bottom: 15px solid transparent;border-right: 10px solid #fff;position: relative;top: -6px;left: 30px;font-size: 0;line-height: 0;z-index: 1; }
    .sept-dashed { margin:10px 0 15px; }
    .top-breadcrumb { padding-top:20px; margin-right:15px; margin-bottom:10px; margin-left:15px; }
    .article-content { margin-left:10px; padding-top:20px; padding-bottom:20px; border-top:1px solid #e2e2e2; }
    #content > div.article-content:first-of-type { padding-top:0; border-top:0; }
    .article-image-wrapper a { width:100%; height:116px; display:block; }
    .margin-bottom8 { margin-bottom:8px; }
    .calender-grey-icon, .author-grey-icon { width:14px; height:15px; position:relative; top:-2px; margin-right:4px; }
    .calender-grey-icon { background-position:-129px -515px; }
    .author-grey-icon { background-position:-105px -515px; }
    #colorbox { width: 400px !important;height: 400px !important;}
</style>
<div class="container margin-bottom30">
    <div class="grid-12">
        <div class="content-box-shadow padding-bottom20">
            <ul class="breadcrumb top-breadcrumb">
                <li>You are here: </li>
                <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb"><a itemprop="url" href="/"><span itemprop="title">Home</span></a></li>
                <li class="fwd-arrow">&rsaquo;</li>
                <li class="current"><strong>Features</strong></li>
            </ul>
            <div class="clear"></div>
    
            <div id="content" class="grid-8">
                <h1 class="black-text margin-bottom15">Features</h1>
                <asp:repeater id="rptFeatures" runat="server">
			        <itemtemplate>
                        <div id='post-<%# DataBinder.Eval(Container.DataItem,"BasicId") %>' class=" <%# Convert.ToString(DataBinder.Eval(Container.DataItem,"AuthorName")).ToLower().Contains("sponsored") ? "sponsored-content" : "post-content" %> article-content">
					        <%# Convert.ToString(DataBinder.Eval(Container.DataItem,"AuthorName")).ToLower().Contains("sponsored") ? "<div class=\"sponsored-tag-wrapper position-rel\"><span>Sponsored</span><span class=\"sponsored-left-tag\"></span></div>" : "" %>
                            <div class="margin-bottom10">
                                <div class="grid-4 alpha omega article-image-wrapper">
                                    <%# DataBinder.Eval(Container.DataItem,"OriginalImgUrl").ToString() != "" ? "<a href='/features/" + DataBinder.Eval(Container.DataItem,"ArticleUrl") + "-" + DataBinder.Eval(Container.DataItem,"BasicId") + "/'><img src='" + Bikewale.Utility.Image.GetPathToShowImages( DataBinder.Eval( Container.DataItem, "OriginalImgUrl" ).ToString(),DataBinder.Eval( Container.DataItem, "HostURL" ).ToString() ,Bikewale.Utility.ImageSize._210x118) +"' alt='"+ DataBinder.Eval(Container.DataItem,"Title") +"' title='"+ DataBinder.Eval(Container.DataItem,"Title") +"' width='100%' border='0' /></a>" : "" %>
                                </div>
                                <div class="grid-8 padding-left20 omega">
                                    <h2 class="font14 margin-bottom8">
                                        <a href="/features/<%# DataBinder.Eval(Container.DataItem,"ArticleUrl") %>-<%# DataBinder.Eval(Container.DataItem,"BasicId") %>/" rel="bookmark" class="text-black text-bold">
							                <%# DataBinder.Eval(Container.DataItem,"Title") %>
						                </a>
                                    </h2>
                                    <div class="font12 text-light-grey">
                                        <div class="grid-5 alpha">
                                            <span class="bwsprite calender-grey-icon inline-block"></span>
                                            <span class="inline-block">
                                                <%# Bikewale.Utility.FormatDate.GetFormatDate(DataBinder.Eval(Container.DataItem,"DisplayDate").ToString(),"MMMM dd, yyyy") %>
                                            </span>
                                        </div>
                                        <div class="grid-7 alpha">
                                            <span class="bwsprite author-grey-icon inline-block"></span>
                                            <span class="inline-block">
                                                <%# DataBinder.Eval(Container.DataItem,"AuthorName") %>
                                            </span>
                                        </div>
                                        <div class="clear"></div>
                                    </div>
                                </div>
                                <div class="clear"></div>
                            </div>
                            <div class="font14"><%# DataBinder.Eval(Container.DataItem,"Description") %><a href="/features/<%# DataBinder.Eval(Container.DataItem,"ArticleUrl") %>-<%# DataBinder.Eval(Container.DataItem,"BasicId") %>/">Read full story</a></div>
                            <div class="clear"></div>  
				        </div>
			        </itemtemplate>
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
