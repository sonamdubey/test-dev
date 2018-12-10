<%@ Page Language="C#" AutoEventWireup="false" Trace="false" Inherits="Carwale.UI.Authors.Default" %>

<%@ Register TagPrefix="wn" TagName="NewsRightWidget" Src="/Controls/NewsRightWidget.ascx" %>
<%@ Register TagPrefix="wv" TagName="PopularVideoWidget" Src="/Controls/PopularVideoWidget.ascx" %>
<%@ Import Namespace="Carwale.UI.Common" %>
<!doctype html>
<html itemscope itemtype="http://schema.org/WebPage">
<head>
<%
	// Define all the necessary meta-tags info here.
	// To know what are the available parameters,
	// check page, headerCommon.aspx in common folder.
	
    PageId 			= 16;
	Title 			= "Authors";
	Description 	= "List of authors who contribute to CarWale.";
	Keywords		= "news, car news, auto news, latest car news, indian car news, car news of india , author";
	Revisit 		= "5";
	DocumentState 	= "Static";
    altUrl          = altURL;
    AdId            = "1396440332273";
    AdPath          = "/1017752/ReviewsNews_";
%>
<!-- #include file="/includes/global/head-script.aspx" -->
<style>
    .all-author li {
    border-top: 1px solid #e2e2e2;
    display: block;
    margin-top: 10px;
    padding-top: 10px;
}
    .all-author-content h3 { font-size:14px; }
</style>
</head>
<body class="bg-white header-fixed-inner special-page special-skin-body no-bg-color">
    <form runat="server">
        <!-- #include file="/includes/header.aspx" -->
        <input type="hidden" id="hdnIsPageFromCache" runat="server" />
        <section class="container">
            <div class="grid-12">
                <div class="padding-bottom15 text-center">
                    <%= Carwale.UI.HtmlHelpers.Helpers.GetAdBarForAspx(AdId, 0, 90, 0, 0, true, 2) %>
                </div>
            </div>
        </section>
        <div class="clear"></div>
        <section class="bg-light-grey padding-top10 padding-bottom20 no-bg-color">
            <div class="container">
                <div class="grid-12">
                    <div class="breadcrumb margin-bottom15">
                        <!-- breadcrumb code starts here -->
                        <ul class="special-skin-text">
                            <li><a href="/">Home</a></li>
                            <li><span class="fa fa-angle-right margin-right10"></span>Authors</li>
                        </ul>
                        <div class="clear"></div>
                    </div>
                    <h1 class="font30 text-black special-skin-text">All Authors</h1>
                    <div class="border-solid-bottom margin-top10 margin-bottom15"></div>
                </div>
                <div class="clear"></div>
                <div class="grid-8">
					<div>
						 <div class="margin-bottom20">
                            <div class="all-author">
                                <ul>
                                    <asp:Repeater ID="rptAuthorList" runat="server">
                                        <ItemTemplate>
                                        <li class="content-inner-block-10 content-box-shadow rounded-corner2 margin-bottom10">        
                                                <div class="all-author-pic">
                                                    <img src="https://<%#((Carwale.Entity.Author.AuthorList)Container.DataItem).HostUrl %>/<%#((Carwale.Entity.Author.AuthorList)Container.DataItem).ProfileImage %>/<%#((Carwale.Entity.Author.AuthorList)Container.DataItem).ImageName %>">
                                                </div>
                                                <div class="all-author-content">
                                                    <h3><a href="/authors/<%#((Carwale.Entity.Author.AuthorList)Container.DataItem).MaskingName %>"><%#((Carwale.Entity.Author.AuthorList)Container.DataItem).AuthorName %>,</a> <span><%#((Carwale.Entity.Author.AuthorList)Container.DataItem).Designation %></span></h3>
                                                    <p>
                                                        <%#((Carwale.Entity.Author.AuthorList)Container.DataItem).ShortDescription %>
                                                        <a href="<%#((Carwale.Entity.Author.AuthorList)Container.DataItem).MaskingName %>/">More &raquo;</a>
                                                    </p>
                                                </div>
                                                <div class="clear"></div>
                                            </li>    
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ul>
                            </div>
                        </div>
					</div>
                </div>
				<div class="grid-4">
                    <div class="content-box-shadow margin-bottom20">
                        <wn:NewsRightWidget ID="ctrlNewsRightWidget" runat="server" />
                    </div>
                    <div>
                        <wv:PopularVideoWidget ID="ctrlPopularVideoWidget" runat="server" />
                    </div>
                </div>
			    <div class="clear"></div>
            </div>
        </section>
        <div class="clear"></div>
        <!-- #include file="/includes/footer.aspx" -->
        <!-- all other js plugins -->
        <!-- #include file="/includes/global/footer-script.aspx" -->
        <link rel="stylesheet" href="/static/css/author.css" type="text/css" >
    <script type="text/javascript">

        $(document).ready(function (e) {
            //for popular and Upcoming cars tabs
            $("#cars-tabs li h2").click(function () {
                $(".cars-tabs-data").hide();
                $(".cars-tabs-data").eq($(this).parent().index()).show();
                $("#cars-tabs li h2").removeClass("active");
                $(this).addClass("active");
            });
            $("#news-tabs li h2").click(function () {
                $(".news-tab-data").hide();
                $(".news-tab-data").eq($(this).parent().index()).show();
                $("#news-tabs li h2").removeClass("active");
                $(this).addClass("active");
            });
            $("#nlHome-new").jcarousel();

        });

    </script>
</form>
</body>
</html>
