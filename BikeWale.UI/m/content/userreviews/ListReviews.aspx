<%@ Page Language="C#" AutoEventWireup="false"  Inherits="Bikewale.Mobile.Content.ListReviews" %>
<%@ Register TagPrefix="Pager" TagName="Pager" Src="/m/controls/ListPagerControl.ascx" %>
<!DOCTYPE html>
<html>
<head>
    <%  title = "User Reviews: " + objModelEntity.MakeBase.MakeName + " " + objModelEntity.ModelName;
        description = objModelEntity.MakeBase.MakeName + " " + objModelEntity.ModelName + " User Reviews - Read first-hand reviews of actual " + objModelEntity.MakeBase.MakeName + " " + objModelEntity.ModelName + " owners. Find out what buyers of " + objModelEntity.MakeBase.MakeName + " " + objModelEntity.ModelName + " have to say about the bike.";
        keywords = objModelEntity.MakeBase.MakeName + " " + objModelEntity.ModelName + " reviews, " + objModelEntity.MakeBase.MakeName + " " + objModelEntity.ModelName + " Users Reviews, " + objModelEntity.MakeBase.MakeName + " " + objModelEntity.ModelName + " customer reviews, " + objModelEntity.MakeBase.MakeName + " " + objModelEntity.ModelName + " customer feedback, " + objModelEntity.MakeBase.MakeName + " " + objModelEntity.ModelName + " owner feedback, user bike reviews, owner feedback, consumer feedback, buyer reviews";
        canonical = "https://www.bikewale.com/" + objModelEntity.MakeBase.MaskingName + "-bikes/" + objModelEntity.MaskingName + "/user-reviews";
        relPrevPageUrl = String.IsNullOrEmpty(prevPageUrl) ? "" : "https://www.bikewale.com" + prevPageUrl;
        relNextPageUrl = String.IsNullOrEmpty(nextPageUrl) ? "" : "https://www.bikewale.com" + nextPageUrl;
        AdPath = "/1017752/Bikewale_Mobile_Model";
        AdId = "1398837216327";
        //menu = "9";
      %>
    <!-- #include file="/includes/headscript_mobile_min.aspx" -->
    <link rel="stylesheet" type="text/css" href="/m/css/user-review/listing.css" />
    <script type="text/javascript">
        <!-- #include file="\includes\gacode_mobile.aspx" -->
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <!-- #include file="/includes/headBW_Mobile.aspx" -->

        <section>
            <div class="container box-shadow bg-white section-bottom-margin">
                <h1 class="box-shadow padding-15-20"><%= objModelEntity.MakeBase.MakeName  + " " + objModelEntity.ModelName%>  User reviews</h1>
                <div class="content-inner-block-10">
                    <div class="content-inner-block-10 margin-bottom5">
                        <div class="model-review-image inline-block">
                            <a href="" title="">
                                <img alt="<%= objModelEntity.MakeBase.MaskingName + " " + objModelEntity.ModelName %> Reviews" title="<%= objModelEntity.MakeBase.MaskingName + " " + objModelEntity.ModelName %> Reviews" src="<%= Bikewale.Common.MakeModelVersion.GetModelImage( objModelEntity.HostUrl, objModelEntity.OriginalImagePath,Bikewale.Utility.ImageSize._110x61) %>">
                            </a>
                        </div>
                        <div class="model-review-details inline-block">
                            <h2 class="font14 margin-bottom5"><a href="" title="" class="text-default"><%= objModelEntity.MakeBase.MakeName  + " " + objModelEntity.ModelName%></a></h2>
                            <p class="font11 text-light-grey">Ex-showroom, New Delhi</p>
                            <span class="bwmsprite inr-xsm-icon"></span>
                            <span class="font16 text-bold">87,000</span>
                        </div>
                    </div>
                    <div class="border-solid rating-box-container display-table">
                        <div class="rating-box overall text-center">
                            <p class="text-bold font14 margin-bottom10">Overall Rating</p>
                            <div class="margin-bottom10">
                                <span class="star-one-icon"></span>
                                <div class="inline-block">
                                    <span class="font20 text-bold">4</span>
                                    <span class="padding-left2 font12 text-light-grey">/ 5</span>
                                </div>
                            </div>
                            <p class="font14 text-light-grey"><%= totalReviews %> Reviews</p>
                        </div>
                        <div class="rating-category-list-container content-inner-block-10 star-icon-sm">
                            <ul class="rating-category-list">
                                <li>
                                    <span class="rating-category-label">Looks</span><span>
                                        <span class="star-one-icon"></span>
                                        <span class="text-bold">4</span>
                                        <span class="font12"> / 5</span>
                                    </span>
                                </li>
                                <li>
                                    <span class="rating-category-label">Fuel Economy</span><span>
                                        <span class="star-one-icon"></span>
                                        <span class="text-bold">4</span>
                                        <span class="font12"> / 5</span>
                                    </span>
                                </li>
                                <li>
                                    <span class="rating-category-label">Performance</span><span>
                                        <span class="star-one-icon"></span>
                                        <span class="text-bold">4</span>
                                        <span class="font12"> / 5</span>
                                    </span>
                                </li>
                                <li>
                                    <span class="rating-category-label">Value for Money</span><span>
                                        <span class="star-one-icon"></span>
                                        <span class="text-bold">4.5</span>
                                        <span class="font12"> / 5</span>
                                    </span>
                                </li>
                                <li>
                                    <span class="rating-category-label">Space/Comfort</span><span>
                                        <span class="star-one-icon"></span>
                                        <span class="text-bold">4</span>
                                        <span class="font12"> / 5</span>
                                    </span>
                                </li>
                            </ul>
                            <div class="clear"></div>
                        </div>
                    </div>
                </div>
            </div>
        </section>

        <%if(totalReviews > 0) { %>
        <section>
            <div class="container box-shadow bg-white section-bottom-margin padding-top15 padding-right20 padding-left20">
                <h2><%= totalReviews + " " + objModelEntity.ModelName %> User reviews</h2>
                <ul class="model-user-review-list">
                    <li>
                        <div class="model-user-review-rating-container">
                            <p class="font16 text-bold">4</p>
                            <p class="inline-block margin-bottom5 margin-top5">
                                <span class="star-one-icon"></span><span class="star-one-icon"></span><span class="star-one-icon"></span><span class="star-one-icon"></span><span class="star-zero-icon"></span>
                            </p>
                        </div>
                        <div class="model-user-review-title-container">
                            <h3>
                                <a class="target-link margin-bottom7" href="" title="Black Beast">Black Beast</a>
                            </h3>
                            <div class="grid-7 alpha padding-right5">
                                <span class="bwmsprite calender-grey-sm-icon"></span>
                                <span class="article-stats-content">Mar 11, 2016</span>
                            </div>
                            <div class="grid-5 alpha omega">
                                <span class="bwmsprite author-grey-sm-icon"></span>
                                <span class="article-stats-content">Santhosh Adiga</span>
                            </div>
                            <div class="clear"></div>
                        </div>
                        <p class="font14 margin-top10">Style Many say its overdone. But when I look at my black RS its not overdone. Each curves have its own functionality and also black looks awesome. Coming to front look it...</p>
                    </li>
                    <li>
                        <div class="model-user-review-rating-container">
                            <p class="font16 text-bold">4</p>
                            <p class="inline-block margin-bottom5 margin-top5">
                                <span class="star-one-icon"></span><span class="star-one-icon"></span><span class="star-one-icon"></span><span class="star-one-icon"></span><span class="star-zero-icon"></span>
                            </p>
                        </div>
                        <div class="model-user-review-title-container">
                            <h3>
                                <a class="target-link margin-bottom7" href="" title="Black Beast">Black Beast</a>
                            </h3>
                            <div class="grid-7 alpha padding-right5">
                                <span class="bwmsprite calender-grey-sm-icon"></span>
                                <span class="article-stats-content">Mar 11, 2016</span>
                            </div>
                            <div class="grid-5 alpha omega">
                                <span class="bwmsprite author-grey-sm-icon"></span>
                                <span class="article-stats-content">Santhosh Adiga</span>
                            </div>
                            <div class="clear"></div>
                        </div>
                        <p class="font14 margin-top10">Style Many say its overdone. But when I look at my black RS its not overdone. Each curves have its own functionality and also black looks awesome. Coming to front look it...</p>
                    </li>
                    <li>
                        <div class="model-user-review-rating-container">
                            <p class="font16 text-bold">4</p>
                            <p class="inline-block margin-bottom5 margin-top5">
                                <span class="star-one-icon"></span><span class="star-one-icon"></span><span class="star-one-icon"></span><span class="star-one-icon"></span><span class="star-zero-icon"></span>
                            </p>
                        </div>
                        <div class="model-user-review-title-container">
                            <h3>
                                <a class="target-link margin-bottom7" href="" title="Black Beast">Black Beast</a>
                            </h3>
                            <div class="grid-7 alpha padding-right5">
                                <span class="bwmsprite calender-grey-sm-icon"></span>
                                <span class="article-stats-content">Mar 11, 2016</span>
                            </div>
                            <div class="grid-5 alpha omega">
                                <span class="bwmsprite author-grey-sm-icon"></span>
                                <span class="article-stats-content">Santhosh Adiga</span>
                            </div>
                            <div class="clear"></div>
                        </div>
                        <p class="font14 margin-top10">Style Many say its overdone. But when I look at my black RS its not overdone. Each curves have its own functionality and also black looks awesome. Coming to front look it...</p>
                    </li>
                </ul>
                <div class="padding-top15 padding-bottom15 border-solid-top font14">
                    <div class="grid-5 alpha omega text-light-grey font13">
                        <span class="text-bold text-default">1-10</span> of <span class="text-bold text-default">100</span> reviews
                    </div>
                    <div class="clear"></div>
                </div>
            </div>
        </section>
        <% } %>

        <section>
            <div class="container bg-white box-shadow padding-top15 padding-bottom15">
                <h2 class="padding-right20 padding-bottom15 padding-left20">User reviews of similar bikes</h2>
                <div class="swiper-container padding-top5 padding-bottom5 user-reviews-type-carousel">
                    <div class="swiper-wrapper">
                        <div class="swiper-slide">
                            <div class="swiper-card">
                                <a href="" title="" class="">
                                    <div class="swiper-image-preview">
                                        <img class="swiper-lazy" data-src="http://imgd1.aeplcdn.com//174x98//bw/models/honda-cb-shine-kick/drum/spokes-111.jpg" alt="" src="" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-details-block">
                                        <h3 class="font12 text-black margin-bottom10 text-truncate">Bajaj Kratos VS400</h3>
                                        <ul class="bike-review-features">
                                            <li>
                                                <span class="star-one-icon"></span>
                                                <span class="font16 text-bold text-default">2</span><span class="font12 text-default"> / 5</span>
                                            </li>
                                            <li class="font14">53 Reviews</li>
                                        </ul>
                                    </div>
                                </a>
                            </div>
                        </div>
                        <div class="swiper-slide">
                            <div class="swiper-card">
                                <a href="" title="" class="">
                                    <div class="swiper-image-preview">
                                        <img class="swiper-lazy" data-src="http://imgd1.aeplcdn.com//174x98//bw/models/honda-cb-shine-kick/drum/spokes-111.jpg" alt="" src="" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-details-block">
                                        <h3 class="font12 text-black margin-bottom10 text-truncate">Bajaj Kratos VS400</h3>
                                        <ul class="bike-review-features">
                                            <li>
                                                <span class="star-one-icon"></span>
                                                <span class="font16 text-bold text-default">2</span><span class="font12 text-default"> / 5</span>
                                            </li>
                                            <li class="font14">53 Reviews</li>
                                        </ul>
                                    </div>
                                </a>
                            </div>
                        </div>
                        <div class="swiper-slide">
                            <div class="swiper-card">
                                <a href="" title="" class="">
                                    <div class="swiper-image-preview">
                                        <img class="swiper-lazy" data-src="http://imgd1.aeplcdn.com//174x98//bw/models/honda-cb-shine-kick/drum/spokes-111.jpg" alt="" src="" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-details-block">
                                        <h3 class="font12 text-black margin-bottom10 text-truncate">Bajaj Kratos VS400</h3>
                                        <ul class="bike-review-features">
                                            <li>
                                                <span class="star-one-icon"></span>
                                                <span class="font16 text-bold text-default">2</span><span class="font12 text-default"> / 5</span>
                                            </li>
                                            <li class="font14">53 Reviews</li>
                                        </ul>
                                    </div>
                                </a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>

        <div class="padding5">
            <div id="divModel" class="box1 new-line5">
	            <div class="normal f-12" style="text-decoration:none;">
		            <table style="width:100%;" cellpadding="0" cellspacing="0">
				        <tr>
					        <td style="width:100px;vertical-align:top;margin-left:5px;">
                                <div class="darkgray"><b><%=!objModelEntity.New && objModelEntity.Used ? "Last Recorded Price: " : "Starts At: " %> </b></div>
                                <div class="darkgray"><b>Rs. <%=Bikewale.Common.CommonOpn.FormatPrice(objModelEntity.MinPrice.ToString()) %></b></div>
                            </td>
					        <td valign="top">
                                <table style="width:100%">
                                        <tr>
                                            <td style="width:105px;" class="darkgray"><span style="position:relative;top:2px;"><b>Overall Average</b></span></td>
                                            <td style="font-size:0px;"> 
                                                <%= Bikewale.Common.CommonOpn.GetRateImage(Convert.ToDouble(objRating.OverAllRating))%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="darkgray new-line5"><span style="position:relative;top:2px;">Looks</span></td>
                                            <td style="font-size:0px;">
                                                <%= Bikewale.Common.CommonOpn.GetRateImage(Convert.ToDouble(objRating.StyleRating))%>
                                            </td>
                                        </tr>
                                            <tr>
                                            <td class="darkgray new-line"><span style="position:relative;top:2px;">Performance</span></td>
                                            <td style="font-size:0px;">
                                                <%= Bikewale.Common.CommonOpn.GetRateImage(Convert.ToDouble(objRating.PerformanceRating))%>     
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="darkgray new-line"><span style="position:relative;top:2px;">Space/Comfort</span></td>
                                            <td style="font-size:0px;">
                                                <%= Bikewale.Common.CommonOpn.GetRateImage(Convert.ToDouble(objRating.ComfortRating))%>    
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="darkgray new-line"><span style="position:relative;top:2px;">Fuel Economy</span></td>
                                            <td style="font-size:0px;">
                                                <%= Bikewale.Common.CommonOpn.GetRateImage(Convert.ToDouble(objRating.FuelEconomyRating))%>   
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="darkgray new-line"><span style="position:relative;top:2px;">Value For Money</span></td>
                                            <td style="font-size:0px;">
                                                <%= Bikewale.Common.CommonOpn.GetRateImage(Convert.ToDouble(objRating.ValueRating))%>   
                                            </td>
                                       </tr>
                                </table>
                            </td>
				        </tr>
		            </table>
	            </div>
            </div>
        <%if(totalReviews > 0) { %>   
            <div>
                <div>
                    <h2 class="margin-top30 margin-bottom20">All Reviews (<%= totalReviews %>)</h2>
                </div>
                <div id="allReviews" class="box new-line5" style="padding:0px 5px;">
                    <asp:Repeater id="rptUserReviews" runat="server">
                        <itemtemplate>
                            <a href='/m/<%= objModelEntity.MakeBase.MaskingName %>-bikes/<%= objModelEntity.MaskingName %>/user-reviews/<%#DataBinder.Eval(Container.DataItem,"ReviewId")%>.html' class="normal f-12">   
                                <div class="container">
                                    <div class="sub-heading">
				                        <b><%#DataBinder.Eval(Container.DataItem,"ReviewTitle") %></b> &nbsp;&nbsp;<span class="arr-small">&raquo;</span>
			                        </div>    
	                                <div class="darkgray new-line5">
		                                <%# Bikewale.Common.CommonOpn.GetDisplayDate(DataBinder.Eval(Container.DataItem,"ReviewDate").ToString()) %> | By <%# DataBinder.Eval(Container.DataItem,"WrittenBy") %>
	                                </div>
                                    <div class="new-line5" style="font-size:0px;">
                                        <%# Bikewale.Common.CommonOpn.GetRateImage(Convert.ToDouble(DataBinder.Eval(Container.DataItem,"OverAllRating.OverAllRating"))) %>            
                                    </div>
                                </div>
                            </a>                
                        </itemtemplate>
                    </asp:Repeater>
                </div>
            </div>
        <%} %>
        <Pager:Pager id="listPager" runat="server"></Pager:Pager>
        </div>

        <script type="text/javascript" src="<%= staticUrl != "" ? "https://st1.aeplcdn.com" + staticUrl : "" %>/m/src/frameworks.js?<%= staticFileVersion %>"></script>

        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <link href="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : "" %>/m/css/bwm-common-btf.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
        <!-- #include file="/includes/footerscript_mobile.aspx" -->
        <!-- #include file="/includes/fontBW_Mobile.aspx" -->
    </form>
</body>
</html>

