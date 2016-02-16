<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Content.DefaultF" Trace="true" Async="true" %>

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
<div class="container_12">
    <div class="grid_12">
        <ul class="breadcrumb">
            <li>You are here: </li>
            <li><a href="/">Home</a></li>
            <li class="fwd-arrow">&rsaquo;</li>
            <li class="current"><strong>Features</strong></li>
        </ul>
        <div class="clear"></div>
    </div>
    <div id="content" class="grid_8 margin-top10">
        <h1>Features &nbsp;&nbsp;&nbsp;</h1>
        <asp:repeater id="rptFeatures" runat="server">
					<itemtemplate>
                        <div id='post-<%# DataBinder.Eval(Container.DataItem,"BasicId") %>' class="margin-bottom15">
						    <div class="anchor-title">
							    <a href="/features/<%# DataBinder.Eval(Container.DataItem,"ArticleUrl") %>-<%# DataBinder.Eval(Container.DataItem,"BasicId") %>/" rel="bookmark" title="Permanent Link to <%# DataBinder.Eval(Container.DataItem,"Title") %>">
								    <%# DataBinder.Eval(Container.DataItem,"Title") %>
							    </a>
						    </div>	
                            <div class="grid_5 alpha">
							    <abbr><%# Bikewale.Utility.FormatDate.GetFormatDate(DataBinder.Eval(Container.DataItem,"DisplayDate").ToString(),"MMMM dd, yyyy") %></abbr> by 
							    <%# DataBinder.Eval(Container.DataItem,"AuthorName") %>
						    </div><div class="clear"></div>
                            <div class="margin-top10">
							    <%--<%# DataBinder.Eval(Container.DataItem,"SmallPicUrl").ToString() != "" ? "<a class='cbBox' href='" + "http://" + DataBinder.Eval(Container.DataItem, "HostURL").ToString() + DataBinder.Eval(Container.DataItem,"LargePicUrl") + "'><img class='alignright size-thumbnail img-border-news' src='" + "http://" + DataBinder.Eval(Container.DataItem, "HostURL").ToString() + DataBinder.Eval(Container.DataItem,"SmallPicUrl") +"' align='right' border='0' /></a>" : "" %>--%>
                                <%# DataBinder.Eval(Container.DataItem,"OriginalImgUrl").ToString() != "" ? "<a class='cbBox cboxElement' href='" + Bikewale.Utility.Image.GetPathToShowImages( DataBinder.Eval( Container.DataItem, "OriginalImgUrl" ).ToString(),DataBinder.Eval( Container.DataItem, "HostURL" ).ToString() ,Bikewale.Utility.ImageSize._210x118) + "'><img class='alignright size-thumbnail img-border-news' src='" + Bikewale.Utility.Image.GetPathToShowImages( DataBinder.Eval( Container.DataItem, "OriginalImgUrl" ).ToString(),DataBinder.Eval( Container.DataItem, "HostURL" ).ToString() ,Bikewale.Utility.ImageSize._144x81) +"' align='right' border='0' /></a>" : "" %>
							    <%# DataBinder.Eval(Container.DataItem,"Description") %>							
						    </div><div class="clear"></div>
                            <div class="margin-top10 item-footer">
						        <div class="grid_5 alpha">
                                    <a href="/features/<%# DataBinder.Eval(Container.DataItem,"ArticleUrl") %>-<%# DataBinder.Eval(Container.DataItem,"BasicId") %>/">Read full article &raquo;</a>
						        </div>                                
                                <div class="clear"></div>
                            </div>                           						
					    </div>
                        <div class="sept-dashed"></div>
				    </itemtemplate>
			    </asp:repeater>
        <BikeWale:RepeaterPager ID="linkPager" runat="server" />
    </div>
    <div class="grid_4">
        <!--    Right Container starts here -->
        <%--<div class="margin-top15">
            <!-- BikeWale_NewBike/BikeWale_NewBike_HP_300x250 -->
            <!-- #include file="/ads/Ad300x250.aspx" -->
            </div>--%>
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
