<%@ Language="C#" ContentType="text/html" Inherits="MobileWeb.Authors.AuthorDetails" AutoEventWireup="false" trace="false" %>
<%@ Import Namespace="Carwale.UI.Common" %>
<%@ Import Namespace="Carwale.BL.Experiments" %>
<%
    bool showExperimentalColor = ProductExperiments.IsShowExperimentalColor(CookiesCustomers.AbTest);
%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" itemscope itemtype="http://schema.org/WebPage">
   <head>
<!-- #include file="/m/includes/global/head-script.aspx" -->
<style>
    .author-imgWidth > img, .other-author-img img {max-width:100%}
    .line-border {
    border-top: 1px solid #ccc;
    height: 1px;
}
    .cw-m-sprite {
    background: url(https://img.aeplcdn.com/pq/m-icons.png?16042015-v9999) no-repeat;
    display: inline-block;
}
    
.author-fb { background-position:0px -235px; width:23px; height:24px; }
.author-gplus { background-position:-28px -235px; width:24px; height:23px; }
.author-linkedin { background-position:-57px -235px; width:23px; height:24px; }
.author-tw { background-position:-85px -235px; width:24px; height:24px; }
.list-bullet-icon { background-position:-45px -55px; width:10px; height:8px; }
.article-list li {
    color: #034fb6;
    padding: 3px 0px;
    list-style: url(https://img2.aeplcdn.com/m/images/list-bullet.png) outside;
    margin-left: 13px;
}
</style>
</head>

<body class="m-special-skin-body m-no-bg-color <%= (showExperimentalColor? "btn-abtest" : "")%>">
	<!-- #include file="/m/includes/header.aspx" -->
	<!--Outer div starts here-->
	<section class="container">
    	<!--Main container starts here-->
    	<div id="main-container">
			<div class="grid-12">
            <div class="inner-container light-shadow darkgray">
                <h1 class="margin-top10 margin-bottom10 m-special-skin-text"><%= authorName %></h1>
                <div class="content-inner-block-10 content-box-shadow rounded-corner2 margin-bottom10">
                <div class="author-imgWidth"><img src="https://<%=hostUrl%>/<%=profileImage%>/<%=imageName%>" alt="Author" title="Author" border="0" /></div>
                <div class="margin-top10">
                    <span class="bold-text">Email :</span>
                    <span><a style="text-decoration:none" class="text-black" href="mailto:<%=emailId%>"><%=emailId%></a></span>
                </div>
                <div class="margin-top5">
                    <div class="leftfloat margin-right5">Catch me on</div>
                    <div class="leftfloat">
                        <%if(facebookProfile != ""){ %><a href="<%=facebookProfile%>" class="cw-m-sprite author-fb margin-right5" target="_blank"></a><%} %>
                        <%if(googlePlusProfile != ""){ %><a href="<%=googlePlusProfile %>" class="cw-m-sprite author-gplus margin-right5" target="_blank"></a><%} %>
                        <%if(linkedInProfile != ""){ %><a href="<%=linkedInProfile %>" class="cw-m-sprite author-linkedin margin-right5" target="_blank"></a><%} %>
                        <%if(twitterProfile != ""){ %><a href="<%=twitterProfile %>" class="cw-m-sprite author-tw" target="_blank"></a><%} %>
                    </div>
                    <div class="clear"></div>
                </div>
                <div class="line-border margin-top10"></div>
                <div class="margin-top10"><%=fullDescription %></div>
                <%
                if(rptExptReviews.Items.Count>0)
                {
                %>
                <div class="line-border margin-top10"></div>
                <h2 class="bold-text margin-top10">Expert Reviews</h2>
                <div class="article-list margin-top10">
                    <ul>
                        <asp:repeater id="rptExptReviews" runat="server">
                            <itemtemplate>
                                <%--<li><a href='/m/expert-reviews/<%#Eval("MakeName") %>-cars/<%#Eval("MaskingName") %>/expert-reviews-'><%#Eval("Title") %></a></li>--%>
                                <li><a href='/m/<%# FormURL(Convert.ToInt32(DataBinder.Eval(Container.DataItem,"CategoryId")),DataBinder.Eval(Container.DataItem,"MakeName").ToString(),DataBinder.Eval(Container.DataItem,"MaskingName").ToString(),DataBinder.Eval(Container.DataItem,"URL").ToString(),Convert.ToInt32(DataBinder.Eval(Container.DataItem,"BasicID"))) %>'><%#Eval("Title") %></a></li>
                            </itemtemplate>
                        </asp:repeater>
                    </ul>
                    <div class="clear"></div>
                </div>
                
                <%} %>
                <%
                if(rptArticles.Items.Count>0)
                {
                %>
                <div class="line-border margin-top10"></div>
                <h2 class="bold-text margin-top10 margin-bottom10">Articles</h2>
                <div class="article-list margin-bottom10">
                    <ul>
                        <asp:repeater id="rptArticles" runat="server">
                            <itemtemplate>
                                <li><a style="text-decoration:none" href='<%# string.Format("/{0}{1}", "m", Eval("Url"))%>'><%#Eval("Title") %></a></li>
                             </itemtemplate>
                        </asp:repeater>
                    </ul>
                    <div class="clear"></div>
                </div>
                <%} %>
                    </div>
            </div>
            <div class="inner-container light-shadow">
                <h2 class="bold-text darkgray margin-bottom10 m-special-skin-text">Other Authors</h2>
                <div class="other-authors margin-bottom10">
                    <ul>
                        <asp:repeater id="rptOtherAuthors" runat="server">
                            <itemtemplate>
                                <li>
                                    <div class="author-details content-inner-block-10 content-box-shadow rounded-corner2 margin-bottom10">
                                        <div class="other-author-img"><a href='/m/authors/<%#Eval("MaskingName") %>'><img src='https://<%#Eval("HostUrl") %>/<%#Eval("ProfileImage") %>/<%#Eval("ImageName") %>' alt="" title="<%#Eval("AuthorName") %>" border="0" /></a></div>
                                        <div class="blue margin-top10"><a href='/m/authors/<%#Eval("MaskingName") %>'><%#Eval("AuthorName") %></a></div>
                                        <div class="lightgray"><%#Eval("Designation") %></div>
                                    </div>
                                </li>
                            </itemtemplate>
                        </asp:repeater>
                    </ul>
                    <div class="clear"></div>
                </div>
                <div class="more-authors margin-bottom10 hide">
                    <ul>
                        <asp:repeater id="rptAllOtherAuthors" runat="server">
                            <itemtemplate>
                                <li>
                                    <div class="author-details content-inner-block-10 content-box-shadow rounded-corner2 margin-bottom10">
                                        <div class="other-author-img"><a href='/m/authors/<%#Eval("MaskingName") %>/' title=""><img src='https://<%#Eval("HostUrl") %>/<%#Eval("ProfileImage") %>/<%#Eval("ImageName") %>' alt="" title="" border="0" /></a></div>
                                        <div class="blue margin-top10"><a href='/m/authors/<%#Eval("MaskingName") %>/' title=""><%#Eval("AuthorName") %></a></div>
                                        <div class="lightgray"><%#Eval("Designation") %></div>
                                    </div>
                                </li>
                            </itemtemplate>
                        </asp:repeater>
                    </ul>
                    <div class="clear"></div>
                </div>
                <button class="btn btn-xs btn-orange btn-full-width margin-bottom15" id="viewAuthors">View All Authors</button>
            </div>
                              
            <!--fb code starts here-->
            <div id="fb-root"></div>
            
         </div>
            <div class="clear"></div>
        </div>
        <!--Main container ends here-->
    </section>
    <div class="clear"></div>
    <!--Outer div ends here-->

    <!-- #include file="/m/includes/footer.aspx" -->
	<!-- #include file="/m/includes/global/footer-script.aspx" -->

    <script>(function (d, s, id) {
    var js, fjs = d.getElementsByTagName(s)[0];
    if (d.getElementById(id)) return;
    js = d.createElement(s); js.id = id;
    js.src = "//connect.facebook.net/en_US/all.js#xfbml=1";
    fjs.parentNode.insertBefore(js, fjs);
}(document, 'script', 'facebook-jssdk'));

            </script>
            <!--fb code ends here-->
            <script type="text/javascript">

                $(document).ready(function () {

                    //authors
                    $("#viewAuthors").click(function () {
                        if ($(".more-authors").is(':hidden')) {
                            $(".more-authors").slideDown();
                            $("html, body").animate({ scrollTop: $(".more-authors").offset().top }, 1500);
                            $("#viewAuthors").html("View Less Authors");
                        }
                        else if ($(".more-authors").is(':visible')) {
                            $(".more-authors").slideUp();
                            $("html, body").animate({ scrollTop: $(".other-authors").offset().top }, 500);
                            $("#viewAuthors").html("View All Authors");
                        }

                    });

                    //More Tab navigation
                    $("#more-tab").click(function () {
                        $("#more-tab").find("#f-nav-icon").toggleClass("more-arrow, more-arrow-down");
                        $("#more-tab").find("#f-nav-icon").toggleClass("more-arrow-down, more-arrow");
                        $("#more-nav").slideToggle()

                    });
                });
            </script>

</body>
</html>