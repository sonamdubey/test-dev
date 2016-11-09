<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Content.DefaultF" Trace="false" Async="true" %>

<%@ Import Namespace="Bikewale.Common" %>
<%@ Register TagPrefix="BikeWale" TagName="RepeaterPager" Src="~/m/controls/LinkPagerControl.ascx" %>
<%@ Register TagPrefix="BW" TagName="MostPopularBikesMin" Src="~/controls/MostPopularBikesMin.ascx" %>
<!Doctype html>
<html>
<head>
    <% 
	    title = "Features - Stories, Specials & Travelogues | BikeWale";
	    description = "Features section of BikeWale brings specials, stories, travelogues and much more.";
	    keywords = "features, stories, travelogues, specials, drives.";
	    canonical = "http://www.bikewale.com/features/";
	    alternate = "http://www.bikewale.com/m/features/";
        relPrevPageUrl = prevUrl;
        relNextPageUrl = nextUrl;
	    AdId = "1395986297721";
	    AdPath = "/1017752/BikeWale_New_";
        //modified by SajalGupta for unfilled impression of ads on 04 Aug 2016.
        isAd300x250Shown = false;
    %>
    <!-- #include file="/includes/headscript_desktop_min.aspx" -->
    <link href="/css/content/listing.css" rel="stylesheet" type="text/css" />

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
                        <li><span class="bwsprite fa-angle-right margin-right10"></span>Features</li>
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
                            <h1 class="section-header">Features</h1>
                            <div class="section-inner-padding">
                                <asp:repeater id="rptFeatures" runat="server">
					                <itemtemplate>
						                <div id='post-<%# DataBinder.Eval(Container.DataItem,"BasicId") %>' class="<%# Regex.Match(Convert.ToString(DataBinder.Eval(Container.DataItem,"AuthorName")), @"\b(sponsored)\b",RegexOptions.IgnoreCase).Success ? "sponsored-content" : "post-content" %> article-content">
								                <%# Regex.Match(Convert.ToString(DataBinder.Eval(Container.DataItem,"AuthorName")), @"\b(sponsored)\b",RegexOptions.IgnoreCase).Success ? "<div class=\"sponsored-tag-wrapper position-rel\"><span>Sponsored</span><span class=\"sponsored-left-tag\"></span></div>" : "" %>
								            <div class="article-image-wrapper">
									            <%# string.Format("<a href='{0}'><img src='{1}' alt='{2}' title='{2}' width='100%' border='0' /></a>",Bikewale.Utility.UrlFormatter.GetArticleUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"BasicId")),Convert.ToString(DataBinder.Eval(Container.DataItem,"ArticleUrl")),Bikewale.Entities.CMS.EnumCMSContentType.Features.ToString()),Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem,"OriginalImgUrl").ToString(),DataBinder.Eval(Container.DataItem,"HostURL").ToString(),Bikewale.Utility.ImageSize._210x118),DataBinder.Eval(Container.DataItem,"Title")) %>
								            </div>
								            <div class="article-desc-wrapper">
									            <h2 class="font14 margin-bottom10">
										            <a href="<%# Bikewale.Utility.UrlFormatter.GetArticleUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"BasicId")),Convert.ToString(DataBinder.Eval(Container.DataItem,"ArticleUrl")),Bikewale.Entities.CMS.EnumCMSContentType.Features.ToString()) %>" rel="bookmark" class="text-black text-bold">
											            <%# DataBinder.Eval(Container.DataItem,"Title") %>
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
									            <div class="font14 line-height"><%# DataBinder.Eval(Container.DataItem,"Description") %><a href="<%# Bikewale.Utility.UrlFormatter.GetArticleUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"BasicId")),Convert.ToString(DataBinder.Eval(Container.DataItem,"ArticleUrl")),Bikewale.Entities.CMS.EnumCMSContentType.Features.ToString()) %>">Read full story</a></div>
								            </div>
                                            <div class="clear"></div>
						                </div>
					                </itemtemplate>
				                </asp:repeater>

                                <div id="footer-pagination" class="font14 padding-top10">
                                    <div class="grid-5 alpha omega text-light-grey">
                                        <p>Showing <span class="text-default text-bold">1-10</span> of <span class="text-default text-bold">26,398</span> articles</p>                                        
                                    </div>
				                    <BikeWale:RepeaterPager ID="ctrlPager" runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>

                    <script type="text/javascript" src="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/src/frameworks.js?<%=staticFileVersion %>"></script>

                    <div class="grid-4 omega">
                        
                        <BW:MostPopularBikesMin ID="ctrlPopularBikes" runat="server" />

                        <div class="content-box-shadow padding-15-20-10 margin-bottom20">
                            <h2>Upcoming bikes</h2>
                            <ul class="sidebar-bike-list">
                                <li>
                                    <a href="" title="Harley Davison Softail" class="bike-target-link">
                                        <div class="bike-target-image inline-block">
                                            <img src="http://imgd1.aeplcdn.com//110x61//bw/models/tvs-apache-rtr-200-4v.jpg" />
                                        </div>
                                        <div class="bike-target-content inline-block padding-left10">
                                            <h3>Harley Davison Softail</h3>
                                            <p class="font11 text-light-grey">Ex-showroom New Delhi</p>
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
                                            <p class="font11 text-light-grey">Ex-showroom New Delhi</p>
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
                                            <p class="font11 text-light-grey">Ex-showroom New Delhi</p>
                                            <span class="bwsprite inr-md"></span><span class="font16 text-bold">&nbsp;1,12,000</span>
                                        </div>
                                    </a>
                                </li>
                                <li>
                                    <a href="" title="Royal Enfield Thunderbird 350" class="bike-target-link">
                                        <div class="bike-target-image inline-block">
                                            <img src="http://imgd1.aeplcdn.com//110x61//bw/models/tvs-apache-rtr-200-4v.jpg" />
                                        </div>
                                        <div class="bike-target-content inline-block padding-left10">
                                            <h3>Royal Enfield Thunderbird 350</h3>
                                            <p class="font11 text-light-grey">Ex-showroom New Delhi</p>
                                            <span class="bwsprite inr-md"></span><span class="font16 text-bold">&nbsp;1,32,000</span>
                                        </div>
                                    </a>
                                </li>
                            </ul>
                            <div class="margin-top10 margin-bottom10">
                                <a href="" class="font14">View all upcoming bikes<span class="bwsprite blue-right-arrow-icon"></span></a>
                            </div>
                        </div>
                        <div class="margin-bottom20">
                            <!-- Ad -->
                        </div>
                        
                        <a href="" id="on-road-price-widget" class="content-box-shadow content-inner-block-20">
                            <span class="inline-block">
                                <img src="https://imgd1.aeplcdn.com/0x0/bw/static/design15/on-road-price.png" alt="Rupee" border="0" />
                            </span><h2 class="text-default inline-block">Get accurate on-road price for bikes</h2>
                            <span class="bwsprite right-arrow"></span>
                        </a>
                        <%--<div>
				        <!-- BikeWale_NewBike/BikeWale_NewBike_HP_300x250 -->
				        <!-- #include file="/ads/Ad300x250.aspx" -->
				        </div>--%>
			        
			            <%--<div>
				            <!-- BikeWale_NewBike/BikeWale_NewBike_HP_300x250 -->
				            <!-- #include file="/ads/Ad300x250BTF.aspx" -->
			            </div>--%>
                    </div>
                    <div class="clear"></div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <!-- #include file="/includes/footerBW.aspx" -->
        
        <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/css/bw-common-btf.css?<%=staticFileVersion %>" rel="stylesheet" type="text/css" />
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/src/common.min.js?<%= staticFileVersion %>"></script>
        <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,700' rel='stylesheet' type='text/css' />
        <!--[if lt IE 9]>
            <script src="/src/html5.js"></script>
        <![endif]-->
    </form>
</body>
</html>
