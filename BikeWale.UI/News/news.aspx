<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.News.news" Trace="false"  Debug="false" Async="true"%>
<%@ Import NameSpace="Bikewale.Common" %>
<%@ Register TagPrefix="BP" TagName="InstantBikePrice" Src="/controls/instantbikeprice.ascx" %>
<%@ Register TagPrefix="LD" TagName="LocateDealer" Src="/controls/locatedealer.ascx" %>
<%
    title = articleTitle + " - BikeWale News";
    description = "BikeWale coverage on " + articleTitle + ". Get the latest reviews and photos for " + articleTitle + " on BikeWale coverage."; 
    canonical= "http://www.bikewale.com/news/" + basicId + "-" + articleUrl + ".html";
    fbTitle 		= title;
    fbImage = GetMainImagePath();//fbLogoUrl; 
    alternate = "http://www.bikewale.com/m/news/" + basicId + "-" + articleUrl + ".html";
    AdId="1395995626568";
    AdPath="/1017752/BikeWale_News_";
%>
<!-- #include file="/includes/headNews.aspx" -->
<style type="text/css" >
    .align-right {text-align: right;}
    .leftfloat {float: left;}
    .rightfloat {float: right;}
    .next-prev-link{width:260px;}
    .next-prev-link p:first-child a { color:#666;}
    .next-prev-link a:hover { color:#000; }
    .next-prev-link a span.next-arrow { background:url(http://img.aeplcdn.com/newtemplate/cw-sprite.png) 0 -589px no-repeat; width:10px; height:15px; display:inline-block;  }
    .next-prev-link:hover a span.next-arrow { background-position:0 -610px; }
    .next-prev-link a span.prev-arrow { background:url(http://img.aeplcdn.com/newtemplate/cw-sprite.png) -13px -589px no-repeat; width:10px; height:15px; display:inline-block; margin-right:5px; }
    .next-prev-link:hover a span.prev-arrow { background-position:-13px -610px; }
    .next-prev-link p:last-child a { text-decoration:underline;font-size: 11px;display:block; color:#666}
    .next-prev-link p:first-child {padding-bottom:5px; }

    .article-content img { width:100%; }
    .article-content p iframe { width:620px !important; }
	
</style>
<div class="container_12">
    <div class="grid_12"><ul class="breadcrumb"><li>You are here: </li><li><a href="/">Home</a></li><li>&rsaquo; <a title="Indian Bike News" href="/news/">Bike News</a></li><li class="current">&rsaquo; <strong><%= articleTitle%></strong></li></ul><div class="clear"></div></div>
	<div class="grid_8 margin-top10">    
        <h1><%= articleTitle %></h1>
		<div id="post-<%= basicId%>">			
            <div class="grid_6 alpha margin-top5"><div class="margin-bottom5"><abbr><%= Bikewale.Utility.FormatDate.GetFormatDate(displayDate, "MMMM dd, yyyy hh:mm tt") %></abbr> by <%= authorName%></div></div>
            <ul class="social">
                <li><fb:like href="http://www.bikewale.com/news/<%= basicId%>-<%= articleUrl %>.html" send="false" layout="button_count" width="80" show_faces="false"></fb:like></li>
                <li><a href="https://twitter.com/share" class="twitter-share-button" data-url="http://www.bikewale.com/news/<%= basicId%>-<%= articleUrl %>.html" data-via='<%= articleUrl %>' data-lang="en">Tweet</a></li>
                <li><div class="g-plusone" data-size="medium" data-href="http://www.bikewale.com/news/<%= basicId%>-<%=articleUrl %>.html"></div></li>
            </ul>
			<div class="clear"></div>
            <div class="margin-top15 article-content">
			    <%if(!String.IsNullOrEmpty(GetMainImagePath())) %>
                    <div style="text-align:center;"><img alt='<%= articleTitle%>' title='<%= articleTitle%>' src='<%= GetMainImagePath() %>'></div>
                <%= content %>
				<div style="clear:both"></div>
			</div>
		</div>
        <div>&nbsp;</div> 
        <!--start-->
        <div class="clear"></div>
         <div class="leftfloat next-prev-link">
            <% if (!String.IsNullOrEmpty(objArticle.PrevArticle.ArticleUrl))
               { %>
            <p class="align-left"><a href="<%= "/news/" + objArticle.PrevArticle.BasicId + "-" + objArticle.PrevArticle.ArticleUrl + ".html"%>" style="text-decoration:none;"><span class="prev-arrow"></span><b>Previous Article</b></a></p>
            <p class="align-left"><a href="<%= "/news/" + objArticle.PrevArticle.BasicId + "-" + objArticle.PrevArticle.ArticleUrl + ".html"%>" style="color:#666;text-decoration:underline;"><%=objArticle.PrevArticle.Title %></a></p>
            <%} %>
        </div>
        <div class="rightfloat next-prev-link">
            <% if (!String.IsNullOrEmpty(objArticle.NextArticle.ArticleUrl))
               { %>
            <p class="align-right"><a href="<%=  "/news/" + objArticle.NextArticle.BasicId + "-" + objArticle.NextArticle.ArticleUrl + ".html"%>" style="text-decoration:none;"><b>Next Article</b> <span class="next-arrow"></span></a></p>
            <p class="align-right"><a href="<%=  "/news/" + objArticle.NextArticle.BasicId + "-" + objArticle.NextArticle.ArticleUrl + ".html"%>" style="color:#666;text-decoration:underline;"><%=objArticle.NextArticle.Title %></a></p>
            <% } %>
        </div> 
        <!--end-->
    </div>  
    <div class="grid_4">
        <%--<div class="margin-top15">
            <!-- BikeWale_News/BikeWale_News_300x250 -->
            <!-- #include file="/ads/Ad300x250.aspx" -->
        </div>--%>
        <div class="light-grey-bg content-block border-radius5 padding-bottom20 margin-top15">
            <BP:InstantBikePrice runat="server" ID="InstantBikePrice" />
        </div>        
        <div class="light-grey-bg content-block border-radius5 margin-top10 padding-bottom20">
            <LD:LocateDealer runat="server" id="LocateDealer" />
        </div>
        <%--<div class="margin-top15">
            <!-- BikeWale_News/BikeWale_News_300x250 -->
            <!-- #include file="/ads/Ad300x250BTF.aspx" -->
        </div>--%>
    </div>    
</div>




<script type="text/javascript" type="text/javascript">
    $(document).ready(function () {
        $("#btnSubmit").click(function () {
            if (Validate()) {
                return false;
            }
            //return false;
        });
        $("#lnkRegCaptcha").click(function () { RegenerateCaptcha(); return false; });
        $("#txtUserName").click(function () {
            if ($("#txtUserName").val() == $("#txtUserName").attr("placeholder")) {
                $("#txtUserName").val("");
            }
        }).focus(function () {
            if ($("#txtUserName").val() == $("#txtUserName").attr("placeholder")) {
                $("#txtUserName").val("");
            }
        }).blur(function () {
            if ($("#txtUserName").val() == "") {
                $("#txtUserName").val($("#txtUserName").attr("placeholder"));
            }
        });
        $("#txtUserEmail").click(function () {
            if ($("#txtUserEmail").val() == $("#txtUserEmail").attr("placeholder")) {
                $("#txtUserEmail").val("");
            }
        }).focus(function () {
            if ($("#txtUserEmail").val() == $("#txtUserEmail").attr("placeholder")) {
                $("#txtUserEmail").val("");
            }
        }).blur(function () {
            if ($("#txtUserEmail").val() == "") {
                $("#txtUserEmail").val($("#txtUserEmail").attr("placeholder"));
            }
        });
        $("#txtComment").click(function () {
            if ($("#txtComment").val() == $("#txtComment").attr("placeholder")) {
                $("#txtComment").val("");
            }
        }).focus(function () {
            if ($("#txtComment").val() == $("#txtComment").attr("placeholder")) {
                $("#txtComment").val("");
            }
        }).keydown(function () {
            showCharactersLeft();
        }).blur(function () {
            if ($("#txtComment").val() == "") {
                $("#txtComment").val($("#txtComment").attr("placeholder"));
            }
            else {
                showCharactersLeft();
            }
        });
    });

    function Validate() {
        var isError = false;
        var emailPattern = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;
        var regExp = /^[A-Za-z ]$/;

        var name = $("#txtUserName").val();
        var email = $("#txtUserEmail").val();
        var comment = $("#txtComment").val();
        var captcha = $("#txtCaptcha").val();

        $("#spnName").text("");
        $("#spnEmail").text("");
        $("#spnError").text("");
        $("#spnCaptcha").text("");

        if (name != null && name != "") {
            for (var i = 0; i < name.length; i++) {
                if (!name.charAt(i).match(regExp)) {
                    $("#spnName").text("Please enter your name");
                    isError = true;
                }
            }
        } else {
            $("#spnName").text("Please enter your name");
            isError = true;
        }

        if (email != null && email != "") {
            if (email != $("#txtUserEmail").attr("placeholder")) {
                if (!emailPattern.test(email)) {
                    $("#spnEmail").text("Please enter a valid email");
                    isError = true;
                }
            } else {
                $("#spnEmail").text("Please enter your email");
                isError = true;
            }
        } else {
            $("#spnEmail").text("Please enter your email");
            isError = true;
        }

        if (comment == $("#txtComment").attr("placeholder")) {
            $("#spnError").text("Please enter the comment");
            isError = true;
        } else if (comment == "") {
            $("#spnError").text("Please enter the comment");
            isError = true;
        }

        if (captcha == "") {
            $("#spnCaptcha").text("Please enter the code shown above");
            isError = true;
        }

        return isError;
    }

    function RegenerateCaptcha() {
        $("#ifrmCaptcha").attr("src", "/Common/CaptchaImage/JpegImage.aspx");
    }

    function showCharactersLeft() {
        var maxSize = 500;
        var comment = $("#txtComment").val();
        var size = comment.length;

        if (size >= maxSize) {
            $("#txtComment").val(comment.substring(0, maxSize - 1));
            size = maxSize;
        }

        $("#spnDesc").html("Characters Left : " + (maxSize - size));
    }

</script>
<!-- #include file="/includes/footerInner.aspx" -->
