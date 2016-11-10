<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.News.news"  %>
<%@ Register TagPrefix="BW" TagName="MostPopularBikesMin" Src="~/controls/MostPopularBikesMin.ascx" %>
<%@ Import NameSpace="Bikewale.Common" %>
<!Doctype html>
<html>
<head>
	<%
        if(metas!=null)
        {
            title = metas.Title;
            description = metas.Description;
            canonical = metas.CanonicalUrl;
            fbTitle = metas.Title;
            fbImage = metas.ShareImage;
            alternate = metas.AlternateUrl;  
        }
		
		AdId="1395995626568";
		AdPath="/1017752/BikeWale_News_";
	%>

	<!-- #include file="/includes/headscript_desktop_min.aspx" -->
    <link href="/css/content/details.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">
        <!-- #include file="\includes\gacode_desktop.aspx" -->
    </script>
</head>
<body class="bg-light-grey header-fixed-inner">
    <form runat="server">
        <!-- #include file="/includes/headBW.aspx" -->

        <section class="container padding-top10">
            <div class="grid-12">
                <div class="breadcrumb margin-bottom15">
                    <ul>
                        <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                            <a href="/" itemprop="url">
                                <span itemprop="title">Home</span>
                            </a>
                        </li>
						<li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
							<span class="bwsprite fa-angle-right margin-right10"></span>
                            <a href="/news/" itemprop="url">
                                <span itemprop="title">Bike News</span>
                            </a>
                        </li>
                        <% if(objArticle!=null) { %>
                        <li><span class="bwsprite fa-angle-right margin-right10"></span><%= objArticle.Title  %></li> 
                         <% } %>
                    </ul>
                    <div class="clear"></div>
                </div>
            </div>
            <div class="clear"></div>
        </section>
        
		<section>
            <div class="container margin-bottom20">
                <div class="grid-12">
                    <div id="content" class="grid-8 alpha">
                        <div class="bg-white">
                            <% if(objArticle!=null) { %>
                            <div class="section-header">
                                <h1 class="margin-bottom5"><%= objArticle.Title %></h1>
								<div>
									<span class="bwsprite calender-grey-sm-icon"></span>
									<span class="article-stats-content margin-right20"><%= Bikewale.Utility.FormatDate.GetFormatDate(Convert.ToString(objArticle.DisplayDate), "MMMM dd, yyyy hh:mm tt") %></span>
									<span class="bwsprite author-grey-sm-icon"></span>
									<span class="article-stats-content"><%= objArticle.AuthorName %></span>
								</div>
                            </div>
                            <div class="section-inner-padding">
								<div id="post-<%= objArticle.BasicId %>">
									<div class="article-content padding-top5">
										<%if(!String.IsNullOrEmpty(metas.ShareImage)) %>
											<div style="text-align:center;"><img alt='<%= objArticle.Title %>' title='<%= objArticle.Title %>' src='<%= metas.ShareImage %>'></div>
										<%= objArticle.Content %>
										<div class="clear"></div>
									</div>
								</div>
								<div class="grid-6 alpha">
									<% if (!String.IsNullOrEmpty(objArticle.PrevArticle.ArticleUrl))
									   { %>
										<a href="<%= "/news/" + objArticle.PrevArticle.BasicId + "-" + objArticle.PrevArticle.ArticleUrl + ".html"%>" class="text-default next-prev-article-target">
											<span class="bwsprite prev-arrow"></span>
											<span class="text-bold">Previous Article</span><br />
											<span class="next-prev-article-title"><%=objArticle.PrevArticle.Title %></span>
										</a>
									<%} %>
								</div>
								<div class="grid-6 omega text-right">
									<% if (!String.IsNullOrEmpty(objArticle.NextArticle.ArticleUrl))
									   { %>
										<a href="<%=  "/news/" + objArticle.NextArticle.BasicId + "-" + objArticle.NextArticle.ArticleUrl + ".html"%>" class="text-default next-prev-article-target">
											<span class="text-bold">Next Article</span>
											<span class="bwsprite next-arrow"></span><br />
											<span class="next-prev-article-title"><%=objArticle.NextArticle.Title %></span>
										</a>
									<% } %>
								</div>
								<div class="clear"></div>
							</div>
                            <% } %>
						</div>
					</div>

					<script type="text/javascript" src="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/src/frameworks.js?<%=staticFileVersion %>"></script>

					<div class="grid-4 omega">
					     <BW:MostPopularBikesMin ID="ctrlPopularBikes" runat="server" />
						<div class="margin-bottom20">
                            <!-- BikeWale_News/BikeWale_News_300x250 -->
                        </div>
                        <div class="content-box-shadow padding-15-20-10 margin-bottom20">
                            <h2>Upcoming Royal Enfield bikes</h2>
                            <ul class="sidebar-bike-list">
                                <li>
                                    <a href="" title="Harley Davison Softail" class="bike-target-link">
                                        <div class="bike-target-image inline-block">
                                            <img src="http://imgd1.aeplcdn.com//110x61//bw/models/tvs-apache-rtr-200-4v.jpg" />
                                        </div>
                                        <div class="bike-target-content inline-block padding-left10">
                                            <h3>Harley Davison Softail</h3>
                                            <p class="font11 text-light-grey">Expected price</p>
                                            <span class="bwsprite inr-md"></span><span class="font16 text-bold">&nbsp;87,000</span>
                                        </div>
                                    </a>
                                </li>
                                <li>
                                    <a href="" title="Bajaj Pulsar AS200" class="bike-target-link">
                                        <div class="bike-target-image inline-block">
                                            <img src="http://imgd1.aeplcdn.com//110x61//bw/models/tvs-apache-rtr-200-4v.jpg" />
                                        </div>
                                        <div class="bike-target-content inline-block padding-left10">
                                            <h3>Bajaj Pulsar AS200</h3>
                                            <p class="font11 text-light-grey">Expected price</p>
                                            <span class="bwsprite inr-md"></span><span class="font16 text-bold">&nbsp;92,000</span>
                                        </div>
                                    </a>
                                </li>
                                <li>
                                    <a href="" title="Honda Unicorn 150" class="bike-target-link">
                                        <div class="bike-target-image inline-block">
                                            <img src="http://imgd1.aeplcdn.com//110x61//bw/models/tvs-apache-rtr-200-4v.jpg" />
                                        </div>
                                        <div class="bike-target-content inline-block padding-left10">
                                            <h3>Honda Unicorn 150</h3>
                                            <p class="font11 text-light-grey">Expected price</p>
                                            <span class="bwsprite inr-md"></span><span class="font16 text-bold">&nbsp;1,12,000</span>
                                        </div>
                                    </a>
                                </li>
                            </ul>
                            <div class="margin-top10 margin-bottom10">
                                <a href="" class="font14">View all upcoming Royal Enfield bikes<span class="bwsprite blue-right-arrow-icon"></span></a>
                            </div>
                        </div>
					</div>
					<div class="clear"></div>
				</div>
				<div class="clear"></div>
			</div>
		</section>

        

		<!-- #include file="/includes/footerBW.aspx" -->

        <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/css/bw-common-btf.css?<%=staticFileVersion %>" rel="stylesheet" type="text/css" />
		<link href="<%= !string.IsNullOrEmpty(staticUrl) ? "http://st2.aeplcdn.com" + staticUrl : string.Empty %>/css/jquery.floating-social-share.min.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css">
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/src/common.min.js?<%= staticFileVersion %>"></script>
		<script type="text/javascript" src="<%= staticUrl != string.Empty ? "http://st2.aeplcdn.com" + staticUrl : string.Empty %>/src/jquery.floating-social-share.min.js?<%= staticFileVersion %>">"></script>
		<script type="text/javascript" type="text/javascript">
			$(document).ready(function () {
				$("body").floatingSocialShare();
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
        <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,700' rel='stylesheet' type='text/css' />
        <!--[if lt IE 9]>
            <script src="/src/html5.js"></script>
        <![endif]-->
    </form>
</body>
</html>
