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
<script type="text/javascript" src="<%= staticUrlPath != "" ? "http://st1.aeplcdn.com" + staticUrlPath : "" %>/src/common/jquery.colorbox-min.js?v=1.0"></script>
<div class="container_12 margin-bottom20 padding-bottom20">
    <div class="grid_12">
        <ul class="breadcrumb">
            <li>You are here: </li>
            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                <a href="/" itemprop="url"><span itemprop="title">Home</span></a>
            </li>
            <li class="fwd-arrow">&rsaquo;</li>
            <li class="current"><strong>Road Tests</strong></li>
        </ul>
        <div class="clear"></div>
    </div>
    <div id="content" class="grid_8 margin-top10">
        <h1>Road Tests &nbsp;&nbsp;&nbsp;</h1>
        <Mms:MakeModelSearch ID="MakeModelSearch" RequestType="RoadTest" runat="server" Visible="false"></Mms:MakeModelSearch>
        <div class="clear"></div>
        <div class="alert moz-round" id="alertObj" runat="server" visible="false"></div>
        <asp:repeater id="rptRoadTest" runat="server" enableviewstate="false">
				<Itemtemplate>					
					<div id='post-<%# DataBinder.Eval(Container.DataItem,"BasicId") %>' class="margin-bottom15">
						<div class="anchor-title">
							<a href='/road-tests/<%# DataBinder.Eval(Container.DataItem,"ArticleUrl") %>-<%# DataBinder.Eval(Container.DataItem,"BasicId") %>.html' rel="bookmark" title="Permanent Link to <%# DataBinder.Eval(Container.DataItem,"Title") %>">
								<%--<%# ( DataBinder.Eval(Container.DataItem, "SubCategory").ToString() == string.Empty ? "Road Test" : DataBinder.Eval(Container.DataItem, "SubCategory").ToString())  + ": " + DataBinder.Eval(Container.DataItem,"MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "ModelName").ToString() %>--%>
							    <%#  DataBinder.Eval(Container.DataItem,"Title").ToString()%>
                            </a>
						</div>	
                        <div class="grid_5 alpha">
							<abbr><%# Bikewale.Utility.FormatDate.GetFormatDate(DataBinder.Eval(Container.DataItem,"DisplayDate").ToString(),"MMMM dd, yyyy") %></abbr> by 
							<%# DataBinder.Eval(Container.DataItem,"AuthorName") %>
						</div><div class="clear"></div>
                        <div class="margin-top10">
							<%--<%# DataBinder.Eval(Container.DataItem,"ImagePathThumbNail").ToString() == "True" ? "<a class='cbBox' href='" + Bikewale.Common.ImagingFunctions.GetImagePath("/ec/", DataBinder.Eval(Container.DataItem,"HostURL").ToString()) + DataBinder.Eval(Container.DataItem,"BasicId") + "/img/m/" + DataBinder.Eval(Container.DataItem,"BasicId") + "_l.jpg'><img class='alignright size-thumbnail img-border-news' src='" + Bikewale.Common.ImagingFunctions.GetImagePath("/ec/", DataBinder.Eval(Container.DataItem, "HostURL").ToString()) + DataBinder.Eval(Container.DataItem,"BasicId") + "/img/m/"+ DataBinder.Eval(Container.DataItem,"BasicId") +"_m.jpg' align='right' border='0' /></a>" : "" %>--%>
                            <div class="grid_5 alpha"><%# DataBinder.Eval(Container.DataItem,"Description") %></div>
                            <%--<div class="grid_3 omega"><%# !String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem,"SmallPicUrl").ToString()) ? "<a class='cbBox' href='" + Bikewale.Common.ImagingFunctions.GetPathToShowImages(DataBinder.Eval(Container.DataItem,"LargePicUrl").ToString(),DataBinder.Eval(Container.DataItem,"HostURL").ToString()) + "'><img class='alignright size-thumbnail border-light' src='" + Bikewale.Common.ImagingFunctions.GetPathToShowImages(DataBinder.Eval(Container.DataItem,"SmallPicUrl").ToString(),DataBinder.Eval(Container.DataItem,"HostURL").ToString()) +"' align='right' border='0' style='padding:2px;' /></a>" : "" %></div>--%>
                            <div class="grid_3 omega"><%# !String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem,"OriginalImgUrl").ToString()) ? "<a class='cbBox cboxElement' href='" + Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem,"OriginalImgUrl").ToString(),DataBinder.Eval(Container.DataItem,"HostURL").ToString(),Bikewale.Utility.ImageSize._210x118) + "'><img class='alignright size-thumbnail border-light' src='" + Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem,"OriginalImgUrl").ToString(),DataBinder.Eval(Container.DataItem,"HostURL").ToString(),Bikewale.Utility.ImageSize._144x81) +"' align='right' border='0' style='padding:2px;' /></a>" : "" %></div>
						</div><div class="clear"></div>
                        <div class="margin-top10 item-footer">
						    <div class="grid_5 alpha">
                                <a href="/road-tests/<%# DataBinder.Eval(Container.DataItem,"ArticleUrl") %>-<%# DataBinder.Eval(Container.DataItem,"BasicId") %>.html">Read full article &raquo;</a>
						    </div>                                
                            <div class="clear"></div>
                        </div>                           						
					</div>
                    <div class="sept-dashed"></div>
			</Itemtemplate>
		</asp:repeater>
        <BikeWale:RepeaterPager ID="linkPager" runat="server" />
    </div>
    <div class="grid_4">
        <!--    Right Container starts here -->
        <div class="margin-top15">
            <!-- BikeWale_NewBike/BikeWale_NewBike_HP_300x250 -->
            <!-- #include file="/ads/Ad300x250.aspx" -->
        </div>
        <div class="light-grey-bg content-block border-radius5 margin-top10 padding-bottom20 margin-top15">
            <uc:InstantBikePrice runat="server" ID="ucInstantBikePrice" />
        </div>
        <div class="light-grey-bg content-block border-radius5 margin-top10 padding-bottom20 margin-top15">
            <CE:CalculateEMIMin runat="server" ID="CalculateEMIMin" />
            <div class="clear"></div>
        </div>
        <div>
            <!-- BikeWale_NewBike/BikeWale_NewBike_HP_300x250 -->
            <!-- #include file="/ads/Ad300x250BTF.aspx" -->
        </div>        
    </div>
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
<style type="text/css">
    #colorbox {
        width: 400px !important;
        height: 400px !important;
    }
</style>
<!-- #include file="/includes/footerInner.aspx" -->
