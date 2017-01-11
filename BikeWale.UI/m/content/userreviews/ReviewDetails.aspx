<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Content.ReviewDetails" %>
<!DOCTYPE html>
<html>
<head>
    <%
        title = objReview.ReviewEntity.ReviewTitle + " - A Review on " + objReview.BikeEntity.MakeEntity.MakeName + " " + objReview.BikeEntity.ModelEntity.ModelName + " by " + objReview.ReviewEntity.WrittenBy;
        description = objReview.BikeEntity.MakeEntity.MakeName + " User Review - " + "A review/feedback on " + objReview.BikeEntity.MakeEntity.MakeName + " " + objReview.BikeEntity.ModelEntity.ModelName + " by " + objReview.ReviewEntity.WrittenBy + ". Find out what " + objReview.ReviewEntity.WrittenBy + " has to say about " + objReview.BikeEntity.MakeEntity.MakeName + " " + objReview.BikeEntity.ModelEntity.ModelName + ".";
        keywords = objReview.BikeEntity.MakeEntity.MakeName + " " + objReview.BikeEntity.ModelEntity.ModelName + " review, " + objReview.BikeEntity.MakeEntity.MakeName + " " + objReview.BikeEntity.ModelEntity.ModelName + " user review, car review, owner feedback, consumer review";
        canonical = "https://www.bikewale.com/" + objReview.BikeEntity.MakeEntity.MaskingName + "-bikes/" + objReview.BikeEntity.MakeEntity.MaskingName + objReview.BikeEntity.ModelEntity.MaskingName + "/user-reviews/" + objReview.ReviewEntity.ReviewId + ".html";
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
                <div class="box-shadow padding-15-20">
                    <h1 class="margin-bottom10"><%= objReview.ReviewEntity.ReviewTitle %></h1>
                    <div class="grid-6 alpha padding-right5">
                        <span class="bwmsprite calender-grey-sm-icon"></span>
                        <span class="article-stats-content"><%= Bikewale.Common.CommonOpn.GetDisplayDate(objReview.ReviewEntity.ReviewDate.ToString()) %></span>
                    </div>
                    <div class="grid-6 alpha omega">
                        <span class="bwmsprite author-grey-sm-icon"></span>
                        <span class="article-stats-content"><%= objReview.ReviewEntity.WrittenBy %></span>
                    </div>
                    <div class="clear"></div>
                </div>

                <div class="content-inner-block-10">
                    <div class="content-inner-block-10 margin-bottom5">
                        <div class="model-review-image inline-block">
                            <img alt="<%=  objReview.BikeEntity.MakeEntity.MakeName + " " + objReview.BikeEntity.ModelEntity.ModelName %> Reviews" title=" <%=  objReview.BikeEntity.MakeEntity.MakeName + " " + objReview.BikeEntity.ModelEntity.ModelName %> Reviews" src="<%= Bikewale.Common.MakeModelVersion.GetModelImage( objReview.HostUrl, objReview.OriginalImagePath,Bikewale.Utility.ImageSize._110x61) %>">
                        </div>
                        <div class="model-review-details inline-block">
                            <h2 class="font14 margin-bottom5"><%= objReview.BikeEntity.MakeEntity.MakeName  + " " + objReview.BikeEntity.ModelEntity.ModelName %></h2>
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
                        </div><div class="rating-category-list-container content-inner-block-10 star-icon-sm">
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

                <div class="padding-right20 padding-left20 margin-top10 font14 padding-bottom10">
                    <h2 class="review-category-title">Good about this bike</h2>
                    <p class="margin-bottom20"><%=  objReview.ReviewEntity.Pros %></p>

                    <h2 class="review-category-title">Not so good about this bike</h2>
                    <p class="margin-bottom20"><%=  objReview.ReviewEntity.Cons %></p>

                    <h2 class="review-category-title">Full Review</h2>
                    <div class="full-review-content">
                        <%= objReview.ReviewEntity.Comments %>
                    </div>

                    <div class="inline-block margin-right15">
                        <span class="bwmsprite views-grey-sm-icon"></span>
                        <span class="font12 text-light-grey">1,29,651</span>
                    </div>
                    <div class="inline-block">
                        <div class="inline-block margin-right15 cur-pointer">
                            <span class="bwmsprite like-icon"></span>
                            <span class="font12 text-light-grey">4000</span>
                        </div>
                        <div class="inline-block cur-pointer">
                            <span class="bwmsprite dislike-icon"></span>
                            <span class="font12 text-light-grey">300</span>
                        </div>
                    </div>

                    <p class="font14 text-light-grey margin-top10 margin-bottom10">Inappropriate Review? <a href="javascript:void(0)" id="report-abuse-target">Report Abuse</a></p>

                    <div class="border-solid-top padding-top10 text-light-grey"><span class="margin-right10">Was this review helpful to you?</span><a href="" class="btn-transparent-sm margin-right5">Yes</a> <a href="" class="btn-transparent-sm">No</a></div>
                </div>                
            </div>
        </section>

        <section>
            <div class="container box-shadow bg-white section-bottom-margin padding-top15 padding-right20 padding-left20">
                <h2>More reviews for <%= objReview.BikeEntity.ModelEntity.ModelName %></h2>
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
                <div class="padding-bottom15">
                    <a href="" class="font14">Read all <%= objReview.BikeEntity.ModelEntity.ModelName %> user reviews <span class="bwmsprite blue-right-arrow-icon"></span></a>
                </div>
            </div>
        </section>

        <section>
            <div class="container bg-white box-shadow padding-top15 padding-bottom15 section-bottom-margin">
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

        <div id="report-abuse-popup">
            <p class="font14 text-bold text-center margin-bottom10">Why do you want to report it abuse?</p>
            <div class="report-abuse-close-btn position-abt pos-top10 pos-right10 bwmsprite cross-lg-lgt-grey cur-pointer"></div>
            <div class="">
                <p class="margin-bottom10">Comment</p>
                <textarea></textarea>
                <div class="text-center">
                    <button type="button" class="btn btn-orange">Report</button>
                </div>
            </div>
        </div>

        <div id="popup-background"></div>

        <div class="padding5">
            <div id="divModel" class="box1 new-line5 f-12">
	            <div class="normal f-12" style="text-decoration:none;">
		            <table style="width:100%;" cellpadding="0" cellspacing="0">
				        <tr>
					        <td style="width:100px;vertical-align:top;margin-left:5px;">
                                <div class="darkgray"><b><%=!objReview.New && objReview.Used ? "Last Recorded Price: " : "Starts At: " %></b></div>
                                <div class="darkgray"><b>Rs. <%= Bikewale.Common.CommonOpn.FormatNumeric(objReview.BikeEntity.Price.ToString())%></b></div>
                            </td>
					        <td valign="top">
                                <table style="width:100%">
                                       <tr><td class="darkgray" colspan="2"><b> <%=  objReview.ReviewEntity.WrittenBy %>'s Ratings</b></td></tr>
                                        <tr>
                                            <td style="width:105px;" class="darkgray"><span style="position:relative;top:2px;">Overall Average</span></td>
                                            <td style="font-size:0px;"><%= Bikewale.Common.CommonOpn.GetRateImage(Convert.ToDouble(objReview.ReviewRatingEntity.OverAllRating))%></td>
                                        </tr>
                                        <tr>
                                            <td class="darkgray new-line5"><span style="position:relative;top:2px;">Looks</span></td>
                                            <td style="font-size:0px;"><%= Bikewale.Common.CommonOpn.GetRateImage(Convert.ToDouble(objReview.ReviewRatingEntity.StyleRating))%></td>
                                        </tr>
                                            <tr>
                                            <td class="darkgray new-line"><span style="position:relative;top:2px;">Performance</span></td>
                                            <td style="font-size:0px;"><%= Bikewale.Common.CommonOpn.GetRateImage(Convert.ToDouble(objReview.ReviewRatingEntity.PerformanceRating))%></td>
                                        </tr>
                                        <tr>
                                            <td class="darkgray new-line"><span style="position:relative;top:2px;">Space/Comfort</span></td>
                                            <td style="font-size:0px;"><%= Bikewale.Common.CommonOpn.GetRateImage(Convert.ToDouble(    objReview.ReviewRatingEntity.ComfortRating))%></td>
                                        </tr>
                                        <tr>
                                            <td class="darkgray new-line"><span style="position:relative;top:2px;">Fuel Economy</span></td>
                                            <td style="font-size:0px;"><%= Bikewale.Common.CommonOpn.GetRateImage(Convert.ToDouble( objReview.ReviewRatingEntity.FuelEconomyRating))%></td>
                                        </tr>
                                        <tr>
                                            <td class="darkgray new-line"><span style="position:relative;top:2px;">Value For Money</span></td>
                                            <td style="font-size:0px;"><%= Bikewale.Common.CommonOpn.GetRateImage(Convert.ToDouble(objReview.ReviewRatingEntity.ValueRating))%></td>
                                        </tr>
                                </table>
                            </td>
				        </tr>
		            </table>
	            </div>
            </div>
        </div>        

        <script type="text/javascript" src="<%= staticUrl != "" ? "https://st1.aeplcdn.com" + staticUrl : "" %>/m/src/frameworks.js?<%= staticFileVersion %>"></script>

        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <link href="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : "" %>/m/css/bwm-common-btf.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
        <!-- #include file="/includes/footerscript_mobile.aspx" -->
        <script type="text/javascript">
            $('#report-abuse-target').on('click', function () {
                reportAbusePopup.open();
                appendState('reportPopup');
            });

            $('.report-abuse-close-btn').on('click', function () {
                reportAbusePopup.close();
                history.back();
            });

            var reportAbusePopup = {
                open: function () {
                    $('#report-abuse-popup').show();
                    $('body').addClass('lock-browser-scroll');
                    $('#popup-background').show();
                },

                close: function () {
                    $('#report-abuse-popup').hide();
                    $('body').removeClass('lock-browser-scroll');
                    $('#popup-background').hide();
                }
            };

            /* popup state */
            var appendState = function (state) {
                window.history.pushState(state, '', '');
            };

            $(window).on('popstate', function (event) {
                if ($('#report-abuse-popup').is(':visible')) {
                    reportAbusePopup.close();
                }
            });
        </script>
        <!-- #include file="/includes/fontBW_Mobile.aspx" -->
    </form>
</body>
</html>
