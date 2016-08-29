<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="versions.aspx.cs" Inherits="Bikewale.New.bikeModel" EnableViewState="false" Trace="false" %>
<%@ Register Src="~/controls/NewAlternativeBikes.ascx" TagName="AlternativeBikes" TagPrefix="BW" %>
<%--<%@ Register Src="~/controls/News_new.ascx" TagName="News" TagPrefix="BW" %>--%>
<%@ Register Src="~/controls/News.ascx" TagName="LatestNews" TagPrefix="BW" %>
<%@ Register Src="~/controls/NewExpertReviews.ascx" TagName="ExpertReviews" TagPrefix="BW" %>
<%@ Register Src="~/controls/NewVideosControl.ascx" TagName="Videos" TagPrefix="BW" %>
<%@ Register Src="~/controls/NewUserReviewsList.ascx" TagPrefix="BW" TagName="UserReviews" %>
<%@ Register Src="~/controls/ModelGallery.ascx" TagPrefix="BW" TagName="ModelGallery" %>
<%@ Register Src="~/controls/PriceInTopCities.ascx" TagPrefix="BW" TagName="TopCityPrice" %>
<!doctype html>
<html>
<head>
    <%
        var modDetails = modelPageEntity.ModelDetails;
        title = String.Format("{0} Price, Reviews, Spec, Photos, Mileage | Bikewale", bikeName);
        description = String.Format("{0} Price in India - Rs. {1}. Find {0} Reviews, Specs, Features, Mileage, On Road Price. See {2} Colors, Images at Bikewale.", bikeName, Bikewale.Utility.Format.FormatPriceLong(price.ToString()), bikeModelName);
        canonical = String.Format("http://www.bikewale.com/{0}-bikes/{1}/", modelPageEntity.ModelDetails.MakeBase.MaskingName, modelPageEntity.ModelDetails.MaskingName);
        AdId = "1442913773076";
        AdPath = "/1017752/Bikewale_NewBike_";
        TargetedModel = modDetails.ModelName;
        fbTitle = title;
        alternate = "http://www.bikewale.com/m/" + modDetails.MakeBase.MaskingName + "-bikes/" + modDetails.MaskingName + "/";
        isAd970x90Shown = true;
        TargetedCity = cityName;
        keywords = string.Format("{0},{0} Bike, bike, {0} Price, {0} Reviews, {0} Photos, {0} Mileage", bikeName);
        ogImage = modelImage; 
        isAd970x90BTFShown = false;
        isHeaderFix = false;
    %>
    <!-- #include file="/includes/headscript_desktop_min.aspx" -->
    <style type="text/css">
        @charset "utf-8";.navigation .carousel-navigation li:hover,.sort-div,.viewBreakupText{cursor:pointer}#moreDealersList a:hover,.btn-inv-grey:hover{text-decoration:none}.header-not-fixed{background:rgba(51,51,51,.8);background:#333\9;padding:10px 20px;z-index:3}.sort-div,.sort-selection-div{background:#fff;border:1px solid #ccc}#modelDetailsContainer .model-details-wrapper{width:522px;margin-left:20px}#expectedPriceContainer{border-bottom:1px solid #ecedee}#variantDetailsContainer .variantText{float:left;font-size:14px;line-height:28px}#variantDetailsContainer .variantDropDown{float:left}#variantDetailsContainer .variantDropDown .form-control{padding:5px 25px 5px 5px}.variantList li{padding-right:20px;padding-left:20px;float:left;border-left:1px solid #ecedee;font-size:14px;text-align:center}.variantList li:first-child{border-left:none;text-align:left;padding-left:0}.city-area-name{margin-left:4px}.model-details-floating-card .city-area-name{width:140px}.exshowroom-area.exshowroom-area-name,.model-details-floating-card .exshowroom-area-name.city-area-name{width:120px}.sort-div{min-width:150px;width:110%;height:32px;padding:5px 9px;color:#555;position:relative}.sort-by-title{display:block;overflow:hidden;text-overflow:ellipsis;white-space:nowrap;text-align:left;width:90%}.sort-selection-div{min-width:150px;width:110%;position:absolute;z-index:2}.sort-selection-div ul li{margin-bottom:5px;margin-top:5px;font-size:14px}.sort-selection-div ul li.selected{font-weight:700}.sort-selection-div ul li input{background:#fff;color:#4d5057;padding:2px 0 2px 8px;font-family:'Open Sans',sans-serif,Arial}.sort-selection-div ul li:hover input{padding:2px 0 2px 8px;cursor:pointer;background:#82888b;color:#fff}#upDownArrow.fa-angle-down{transition:all .5s ease-in-out 0s;font-size:20px}.sort-div .fa-angle-down{transition:transform .3s;-moz-transition:transform .3s;-webkit-transition:transform .3s;-o-transition:transform .3s;-ms-transition:transform .3s}.sort-div.open .fa-angle-down{-moz-transform:rotateZ(180deg);-webkit-transform:rotateZ(180deg);-o-transform:rotateZ(180deg);-ms-transform:rotateZ(180deg);transform:rotateZ(180deg)}.viewBreakupText{color:#0288d1;margin-left:5px}.bike-discontinued-tag,.bike-upcoming-tag{z-index:2}.model-name-review-container:hover .write-review-text{display:block}.connected-carousels .stage{width:385px;margin:0 auto 20px;position:relative}.connected-carousels .navigation{width:385px;position:relative;margin-left:5px}.connected-carousels .carousel{overflow:hidden;position:relative}.connected-carousels .carousel ul{width:20000em;position:relative;list-style:none;margin:0;padding:0}.connected-carousels .carousel li{float:left;width:385px;height:220px}.connected-carousels .carousel li .carousel-img-container{display:table;width:385px;height:220px;text-align:center}.connected-carousels .carousel li .carousel-img-container span{display:table-cell;vertical-align:middle;width:385px;height:220px;background:url(http://imgd4.aeplcdn.com/0x0/bw/static/sprites/d/loader.gif) center center no-repeat}.connected-carousels .carousel li .carousel-img-container span img{width:100%;height:220px}.connected-carousels .carousel-stage{height:220px}.connected-carousels .carousel-navigation{height:52px;width:325px;background:#fff;margin-left:24px}.navigation .carousel-navigation li{float:left;width:90px;height:50px;margin-right:10px}.navigation .carousel-navigation li img{width:100%;border:1px solid #ccc}.navigation .carousel-navigation li.active img{border:1px solid #555}.connected-carousels .carousel-navigation li.active img{border-color:#555}.navigation .carousel-navigation li .carousel-nav-img-container{display:table;width:90px;height:50px;text-align:center}.navigation .carousel-navigation li .carousel-nav-img-container span{display:table-cell;vertical-align:middle;width:90px}.connected-carousels .next-stage,.connected-carousels .prev-stage{display:block;position:absolute;top:40%;color:#fff}.connected-carousels .prev-stage{left:0}.connected-carousels .next-stage{right:0}.connected-carousels .next,.connected-carousels .prev{position:absolute;top:40%;z-index:1;width:26px;height:48px;text-indent:-9999px;border:1px solid #e2e2e2;background-color:#fff;padding:6px 3px}.connected-carousels .navigation .prev-navigation,.connected-carousels .prev-stage{background-position:-125px -355px}.connected-carousels .navigation .next-navigation,.connected-carousels .next-stage{background-position:-150px -355px}.connected-carousels .navigation .prev-navigation:hover,.connected-carousels .prev-stage:hover{background-position:-125px -386px}.connected-carousels .navigation .next-navigation.hover,.connected-carousels .next-stage:hover{background-position:-150px -386px}.connected-carousels .navigation .next-navigation.inactive,.connected-carousels .navigation .prev-navigation.inactive,.connected-carousels .next-stage.inactive,.connected-carousels .prev-stage.inactive{display:block}.dealership-benefit-list .benefit-list-image,.dealership-benefit-list .benefit-list-title,.dealership-benefit-list li{display:inline-block;vertical-align:middle}.connected-carousels .navigation .prev-navigation.inactive,.connected-carousels .prev-stage.inactive{background-position:-125px -325px;cursor:not-allowed}.connected-carousels .navigation .next-navigation.inactive,.connected-carousels .next-stage.inactive{background-position:-150px -325px;cursor:not-allowed}.connected-carousels .navigation .next-navigation,.connected-carousels .navigation .prev-navigation{width:26px;height:45px;text-indent:-9999;border:none;background-color:transparent;padding:6px 3px}.connected-carousels .navigation .prev-navigation{left:2px;top:2px}.connected-carousels .navigation .next-navigation{right:7px;top:2px}.dealership-benefit-list li{width:290px;color:#82888b;margin-top:10px}.dealership-benefit-list.dealer-two-offers li{width:440px}.dealership-benefit-list.f-two-offers .benefit-list-title{width:400px}.dealership-benefit-list .benefit-list-title{width:255px;padding-right:10px;padding-left:10px}.form-control-username{width:270px}.form-control-email-mobile{width:188px}.btn.btn-inv-grey{padding:8px 64px}.btn-inv-grey{background:#4d5057;color:#fff;border:1px solid #4d5057}.btn-inv-grey:hover{background:#82888b;border:1px solid #82888b}#buyingAssistance .errorIcon,#buyingAssistance .errorText,.less-dealers-link{display:none}#moreDealersList{display:none;padding-right:20px;padding-left:20px;overflow:hidden}#moreDealersList li{padding-top:15px;padding-bottom:15px;border-bottom:1px solid #f1f1f1}.model-sprite{background:url(http://imgd2.aeplcdn.com/0x0/bw/static/sprites/d/model-sprite-new.png?v=18Aug2016) no-repeat;display:inline-block}.loc-change-blue-icon{width:12px;height:16px;background-position:-205px -9px;cursor:pointer;position:relative;top:2px}.loc-change-blue-icon:hover{background-position:-205px -35px}.offer-benefit-sprite{background:url(http://imgd1.aeplcdn.com/0x0/bw/static/sprites/d/offer-benefit-sprite.png?v1=30Mar2016v1) no-repeat;display:inline-block}.benifitIcon_1,.benifitIcon_2,.benifitIcon_3,.benifitIcon_4,.benifitIcon_5,.offerIcon_1,.offerIcon_2,.offerIcon_3,.offerIcon_4,.offerIcon_5,.offerIcon_6,.offerIcon_7{width:30px}.benifitIcon_1{height:26px;background-position:0 -181px}.benifitIcon_2{height:30px;background-position:0 -141px}.benifitIcon_3{height:25px;background-position:0 -75px}.benifitIcon_4{height:18px;background-position:0 -293px}.offerIcon_1{height:25px;background-position:0 -40px}.offerIcon_2{height:25px;background-position:0 -321px}.offerIcon_3{height:30px;background-position:0 0}.offerIcon_4{height:26px;background-position:0 -257px}.benifitIcon_5,.offerIcon_5,.offerIcon_7{height:30px;background-position:0 -217px}.offerIcon_6{height:21px;background-position:0 -110px}.edit-blue-icon{width:16px;height:16px;background-position:-115px -249px;cursor:pointer}.margin-top35{margin-top:35px}.text-darker-black{color:#1a1a1a}.text-blue{color:#0288d1}.border-light{border:1px solid #e2e2e2}.border-light-bottom{border-bottom:1px solid #f1f1f1}.pos-top2{top:2px}#leadCapturePopup,#otpPopup{display:none}#modelSpecsTabsContentWrapper{min-height:300px}#modelDetailsFloatingCardContent{display:block;position:fixed;top:-500px;left:5%;right:5%;margin:0 auto;width:996px;z-index:5;-webkit-transition:all .2s ease 0s;-moz-transition:all .2s ease 0s;-o-transition:all .2s ease 0s;-ms-transition:all .2s ease 0s;transition:all .2s ease 0s}#modelDetailsFloatingCardContent .model-details-floating-card{width:976px}#modelDetailsFloatingCardContent.fixed-card{top:0}#modelDetailsFloatingCardContent .overall-specs-tabs-wrapper{display:none}#modelDetailsFloatingCardContent.activate-tabs .overall-specs-tabs-wrapper{display:block}.overall-specs-tabs-wrapper{display:table;background:#fff}.overall-specs-tabs-wrapper a{padding:10px 20px;display:table-cell;font-size:14px;color:#82888b}.overall-specs-tabs-wrapper a:hover{text-decoration:none;color:#4d5057}#modelSpecsTabsContentWrapper .overall-specs-tabs-wrapper a:first-child,.overall-specs-tabs-wrapper a.active{border-bottom:3px solid #ef3f30;font-weight:700;color:#4d5057}.content-inner-block-2010{padding:20px 10px}
    </style>

    <script type="text/javascript">
        <!-- #include file="\includes\gacode_desktop.aspx" -->

        var dealerId = '<%= dealerId%>';
        var pqId = '<%= pqId%>';
        var versionId = '<%= variantId%>';
        var cityId = '<%= cityId%>';
        var bikeVersionLocation = '';
        var bikeVersion = '';
        var isBikeWalePq = "<%= isBikeWalePQ%>";
        var areaId = "<%= areaId %>";
        var isDealerPriceAvailable = "<%= pqOnRoad != null ? pqOnRoad.IsDealerPriceAvailable : false%>";
        var campaignId = "<%= campaignId%>";
        var manufacturerId = "<%= manufacturerId%>";
        var myBikeName = "<%= this.bikeName %>";
        var clientIP = "<%= clientIP%>";
        var pageUrl = "<%= canonical %>";
    </script>
    
</head>
<body class="bg-light-grey" itemscope itemtype="http://schema.org/Product">
    <form runat="server">
         <% if (modelPageEntity != null && modelPageEntity.ModelDesc != null)
                                       { %>
        <meta itemprop="description" itemtype="https://schema.org/description" content= "<%=modelPageEntity.ModelDesc.SmallDescription %>" />
        <% } %>
        <meta itemprop="name" content="<%= bikeName %>" />
        <meta itemprop="image" content="<%= modelImage %>" />
        
        <!-- #include file="/includes/headBW.aspx" -->
        <section class="bg-light-grey padding-top10" id="breadcrumb">
            <div class="container">
                <div class="grid-12">
                    <div class="breadcrumb margin-bottom15">
                        <!-- breadcrumb code starts here -->
                        <ul>
                            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb"><a href="/" itemprop="url">
                                <span itemprop="title">Home</span></a>
                            </li>
                            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                                <span class="bwsprite fa-angle-right margin-right10"></span>
                                <a href="/<%= modelPageEntity.ModelDetails.MakeBase.MaskingName %>-bikes/" itemprop="url">
                                    <span itemprop="title"><%= modelPageEntity.ModelDetails.MakeBase.MakeName %> Bikes</span>
                                </a></li>
                            <li><span class="bwsprite fa-angle-right margin-right10"></span>
                              <span><%= modelPageEntity.ModelDetails.MakeBase.MakeName %> <%= modelPageEntity.ModelDetails.ModelName %></span>
                            </li>
                        </ul>
                        <div class="clear"></div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        <section>
            <div class="container" id="modelDetailsContainer">
                <div class="grid-12 margin-bottom20">
                    <div class="content-inner-block-20 content-box-shadow">
                        <div class="grid-5 alpha">
                            <div class="position-rel <%= modelPageEntity.ModelDetails.Futuristic ? string.Empty : "hide" %>">
                                <%--<span class="model-sprite bw-upcoming-bike-ico bike-upcoming-tag position-abt"></span>--%>
                                <span class="upcoming-text-label font16 position-abt text-white text-center">Upcoming</span>
                            </div>
                            <div class="position-rel <%= !modelPageEntity.ModelDetails.Futuristic && !modelPageEntity.ModelDetails.New ? string.Empty : "hide" %>">
                                <span class="discontinued-text-label font16 position-abt text-center">Discontinued</span>
                            </div>
                            <div class="connected-carousels" id="bikeBannerImageCarousel">
                                <div class="stage">
                                    <div class="carousel carousel-stage">
                                        <ul>
                                            <asp:Repeater ID="rptModelPhotos" runat="server">
                                                <ItemTemplate>
                                                    <li>
                                                        <div class="carousel-img-container">
                                                            <span>                                                                
                                                                <img class='<%# Container.ItemIndex > 2 ? "lazy" : "" %>' data-original='<%# Container.ItemIndex > 2 ? Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImgPath").ToString(),DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._476x268) : "" %>' title="<%# bikeName + ' ' + DataBinder.Eval(Container.DataItem, "ImageCategory").ToString() %>" alt="<%# bikeName + ' ' + DataBinder.Eval(Container.DataItem, "ImageCategory").ToString() %>" src='<%# Container.ItemIndex > 2 ? "" : Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImgPath").ToString(),DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._476x268) %>' border="0" />
                                                            </span>
                                                        </div>
                                                    </li>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </ul>
                                    </div>
                                    <a href="#" class="prev prev-stage bwsprite" rel="nofollow"></a>
                                    <a href="#" class="next next-stage bwsprite" rel="nofollow"></a>
                                </div>

                                <div class="navigation">
                                    <a href="#" class="prev prev-navigation bwsprite" rel="nofollow"></a>
                                    <a href="#" class="next next-navigation bwsprite" rel="nofollow"></a>
                                    <div class="carousel carousel-navigation">
                                        <ul>
                                            <asp:Repeater ID="rptNavigationPhoto" runat="server">
                                                <ItemTemplate>
                                                    <li>
                                                        <div class="carousel-nav-img-container">
                                                            <span>
                                                                <img class="<%# Container.ItemIndex > 7 ? "lazy" : "" %>" data-original="<%# Container.ItemIndex > 7 ? Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImgPath").ToString(),DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._110x61) : "" %>" title="<%# bikeName + ' ' + DataBinder.Eval(Container.DataItem, "ImageCategory").ToString() %>" alt="<%# bikeName + ' ' + DataBinder.Eval(Container.DataItem, "ImageCategory").ToString() %>" src="<%# Container.ItemIndex <= 7 ? Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImgPath").ToString(),DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._110x61) : "http://imgd1.aeplcdn.com/0x0/bw/static/sprites/d/loader.gif"%>" border="0" />
                                                            </span>
                                                        </div>
                                                    </li>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </ul>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="grid-7 model-details-wrapper omega">
                            <div class="model-name-review-container">
                                <h1><%= bikeName %></h1>
                                <% if (!modelPageEntity.ModelDetails.Futuristic || modelPageEntity.ModelDetails.New)
                                   { %>
                                <!-- Review & ratings -->
                                <div id="modelRatingsContainer" class="margin-top5 margin-bottom20 <%= modelPageEntity.ModelDetails.Futuristic ? "hide " : string.Empty %>">
                                    <% if (Convert.ToDouble(modelPageEntity.ModelDetails.ReviewRate) > 0)
                                       { %>
                                    <p class="bikeModel-user-ratings leftfloat margin-right10">
                                        <%= Bikewale.Utility.ReviewsRating.GetRateImage(Convert.ToDouble(modelPageEntity.ModelDetails.ReviewRate)) %>
                                    </p>

                                    <span itemprop="aggregateRating" itemscope="" itemtype="http://schema.org/AggregateRating">
                                        
                                        <meta itemprop="ratingValue" content="<%=modelPageEntity.ModelDetails.ReviewRate %>">
                                        <meta itemprop="worstRating" content="1">
                                        <meta itemprop="bestRating" content="5">
                                        <a href="<%= FormatShowReview(modelPageEntity.ModelDetails.MakeBase.MaskingName,modelPageEntity.ModelDetails.MaskingName) %>" class="review-count-box font14 border-solid-left leftfloat margin-right20 padding-left10 ">
                                            <span itemprop="reviewCount">
                                                <%= modelPageEntity.ModelDetails.ReviewCount %>
                                            </span>Reviews
                                        </a>
                                    </span>
                                    <% }
                                       else
                                       { %>
                                    <p class="leftfloat margin-right20 font14">Not rated yet</p>
                                    <% } %>
                                    <a href="<%= FormatWriteReviewLink() %>" class="hide border-solid-left leftfloat margin-right10 padding-left10 font14 write-review-text">Write a review</a>
                                    <div class="clear"></div>
                                </div>
                                <!-- Review & ratings -->
                                <% } %>
                            </div>
                            <!-- Variants -->
                            <div id="variantDetailsContainer" class="variants-dropDown margin-top20 <%= modelPageEntity.ModelDetails.Futuristic ? "hide": string.Empty%>">
                                <div>
                                    <p class="variantText text-light-grey margin-right10">Version: </p>

                                    <% if (modelPageEntity.ModelVersions != null && modelPageEntity.ModelVersions.Count > 1)
                                       { %>
                                    <div class="form-control-box variantDropDown">
                                        <div class="sort-div rounded-corner2">
                                            <div class="sort-by-title" id="sort-by-container">
                                                <span class="leftfloat sort-select-btn">
                                                    <asp:Label runat="server" ID="defaultVariant"></asp:Label>
                                                </span>
                                                <span class="clear"></span>
                                            </div>
                                            <span id="upDownArrow" class="rightfloat fa fa-angle-down position-abt pos-top13 pos-right10"></span>
                                        </div>
                                        <div class="sort-selection-div sort-list-items hide">
                                            <ul id="sortbike">
                                                <asp:Repeater ID="rptVariants" runat="server">
                                                    <ItemTemplate>
                                                        <li>
                                                            <asp:Button Style="width: 100%; text-align: left" ID="btnVariant" ToolTip='<%#Eval("VersionName") %>' OnCommand="btnVariant_Command" versionid='<%#Eval("VersionId") %>' CommandName='<%#Eval("VersionId") %>' CommandArgument='<%#Eval("VersionName") %>' runat="server" Text='<%#Eval("VersionName") %>'></asp:Button>                                                            
                                                        </li>
                                                        <asp:HiddenField ID="hdn" Value='<%#Eval("VersionId") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </ul>
                                            <asp:HiddenField ID="hdnVariant" Value="0" runat="server" />
                                        </div>
                                    </div>
                                    <% }
                                       else
                                       { %>
                                    <p id='versText' class="variantText margin-right20"><%= variantText %></p>
                                    <% } %>
                                    <div class="clear"></div>
                                </div>


                                <%if (modelPageEntity.ModelVersionSpecs != null)
                                  { %>
                                <ul class="variantList margin-top10 text-xt-light-grey">
                                    <%if (modelPageEntity.ModelVersionSpecs.Displacement != 0)
                                      { %>
                                    <li>
                                        <span><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.Displacement) %></span>
                                        <span>cc</span>
                                    </li>
                                    <% } %>
                                    <%if (modelPageEntity.ModelVersionSpecs.FuelEfficiencyOverall != 0)
                                      { %>
                                    <li>
                                        <span><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.FuelEfficiencyOverall) %></span>
                                        <span>kmpl</span>
                                    </li>
                                    <% } %>
                                    <%if (modelPageEntity.ModelVersionSpecs.MaxPower != 0)
                                      { %>
                                    <li>
                                        <span><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.MaxPower) %></span>
                                        <span>bhp</span>
                                    </li>
                                    <%} %>
                                    <%if (modelPageEntity.ModelVersionSpecs.KerbWeight != 0)
                                      { %>
                                    <li>
                                        <span><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.KerbWeight) %></span>
                                        <span>kg</span>
                                    </li>
                                    <%} %>
                                </ul>
                                <div class="clear"></div>
                                <%} %>
                            </div>
                            <div id="scrollFloatingButton"></div>
                            <!-- Variant div ends -->
                            <% if (!modelPageEntity.ModelDetails.Futuristic)
                               { %>
                            <div id="modelPriceContainer" class="padding-top15">
                                <% if (isDiscontinued)
                                   { %>
                                <p class="font14 text-light-grey">Last known Ex-showroom price</p>
                                <% } %>
                                <% else if (!isCitySelected)
                                   {%>
                                <p class="font14 text-light-grey">Ex-showroom price in <span class="font14 text-default"><%= Bikewale.Utility.BWConfiguration.Instance.DefaultName %></span><a ismodel="true" modelid="<%=modelId %>" class="margin-left5 fillPopupData changeCity"><span class="bwsprite loc-change-blue-icon"></span></a></p>
                                <% } %>
                                <% else if (!isOnRoadPrice)
                                   {%>
                                <p class="font14 text-light-grey">Ex-showroom price in <span><span class="font14 text-default city-area-name"><%= !string.IsNullOrEmpty(areaName) ? string.Format("{0}, {1}", areaName, cityName) : cityName %></span></span><a ismodel="true" modelid="<%=modelId %>" class="margin-left5 fillPopupData changeCity"><span class="bwsprite loc-change-blue-icon"></span></a></p>
                                <% } %>
                                <% else
                                   {%>
                                <p class="font14 text-light-grey">On-road price in<span><span class="city-area-name"><%= !string.IsNullOrEmpty(areaName) ? string.Format("{0}, {1}", areaName, cityName) : cityName %></span></span><a ismodel="true" modelid="<%=modelId %>" class="margin-left5 fillPopupData changeCity"><span class="bwsprite loc-change-blue-icon"></span></a></p>

                                <% } %>
                                <%  if (price == 0)
                                    { %>
                                <span class="font20">Price not available</span>
                                <%  }
                                    else
                                    { %>
                                <div class="leftfloat margin-top5 margin-right15 <%= (isBookingAvailable && isDealerAssitance) ? "model-price-book-now-wrapper" : string.Empty %> " itemprop="offers" itemscope itemtype="http://schema.org/Offer">
                                    <span itemprop="priceCurrency" content="INR">
                                        <span class="bwsprite inr-md-lg"></span>
                                    </span>
                                    <span id="new-bike-price" class="font22" itemprop="price" content="<%=price %>"><%= Bikewale.Utility.Format.FormatPrice(price.ToString()) %></span>
                                    <%if (isOnRoadPrice)
                                      {%>
                                    <span id="viewBreakupText" class="font14 text-bold viewBreakupText">View detailed price</span>
                                    <br>
                                    <% } %>
                                </div>
                                <%  } %>
                                <%if (isBookingAvailable && isDealerAssitance) { %>
                                <a href="/pricequote/bookingsummary_new.aspx?MPQ=<%= mpqQueryString %>" class="btn btn-grey leftfloat margin-top20" id="bookNowBtn">Book now </a>
                                <%}%>
                                <div class="clear"></div>
                                <% if (isDiscontinued)
                                   { %>
                                <p class="default-showroom-text font14 text-light-grey margin-top5"><%= bikeName %> is now discontinued in India.</p>
                                <% } %>
                                <% 
                                   else
                                       if (toShowOnRoadPriceButton)
                                       { %>
                                <a id="btnGetOnRoadPrice" href="javascript:void(0)" ismodel="true" modelid="<%=modelId %>" class="btn btn-orange margin-top10 fillPopupData">Check on-road price</a>
                                <div class="clear"></div>
                                
                                <% } %>
                            </div>

                            <% if (viewModel != null && viewModel.IsPremiumDealer && !isBikeWalePQ)
                               { %>
                            <a href="javascript:void(0)" id="getassistance" leadSourceId="12" class="btn btn-orange margin-top10 margin-right10 leftfloat">Get offers from dealer</a>
                            <div class="leftfloat margin-top10">
                                <span class="font12 text-light-grey">Powered by</span><br />
                                <span class="font14"><%= viewModel.Organization %></span>
                            </div>
                            <div class="clear"></div>
                             <%  }
                                } %>
                            <% if (!toShowOnRoadPriceButton && isBikeWalePQ && dealerId == 0)
                               { %>
                            <%--<div class="insurance-breakup-text text-bold padding-top10" >
                                <a target="_blank" id="insuranceLink" href="/insurance/">Save up to 60% on insurance - PolicyBoss</a>
                            </div>--%>
                            <% } %>
                            <!-- upcoming start -->
                            <% if (modelPageEntity.ModelDetails.Futuristic && modelPageEntity.UpcomingBike != null)
                               { %>
                            <div id="upcoming">
                                <% if (modelPageEntity.UpcomingBike.EstimatedPriceMin != 0 && modelPageEntity.UpcomingBike.EstimatedPriceMax != 0)
                                   { %>
                                <div id="expectedPriceContainer" class="padding-top15">
                                    <p class="font14 default-showroom-text text-light-grey">Expected Price</p>
                                    <div class="modelExpectedPrice margin-bottom15">
                                        <span class="bwsprite inr-md-lg"></span>
                                        <span id="bike-price" class="font22">
                                            <span><%= Bikewale.Utility.Format.FormatNumeric(Convert.ToString(modelPageEntity.UpcomingBike.EstimatedPriceMin)) %></span>
                                            <span>- </span>
                                            <span class="bwsprite inr-md-lg"></span>
                                            <span><%= Bikewale.Utility.Format.FormatNumeric(Convert.ToString(modelPageEntity.UpcomingBike.EstimatedPriceMax)) %></span>
                                        </span>
                                    </div>
                                </div>
                                <%}
                                   else
                                   { %>
                                <p class="font26 default-showroom-text text-light-grey margin-bottom5">Price Unavailable</p>
                                <% } %>
                                <% if (!string.IsNullOrEmpty(modelPageEntity.UpcomingBike.ExpectedLaunchDate))
                                   { %>
                                <div id="expectedDateContainer" class="padding-top15 font14">
                                    <p class="default-showroom-text text-light-grey margin-bottom10">Expected launch date</p>
                                    <p class="modelLaunchDate text-bold font18 margin-bottom10"><%= modelPageEntity.UpcomingBike.ExpectedLaunchDate %></p>
                                    <p class="default-showroom-text text-light-grey"><%= bikeName %> is not launched in India yet.</p>
                                    <p class="text-light-grey">Information on this page is tentative.</p>
                                </div>
                                <%} %>
                            </div>
                            <% } %>
                            <!-- upcoming end -->
                        </div>
                        <div class="clear"></div>
                        <% if (viewModel!= null && viewModel.IsPremiumDealer && !isBikeWalePQ)
                           { %>
                        <div id="dealerDetailsWrapper" class="border-light margin-top20">
                            <div class="padding-top20 padding-right20 padding-left20">
                                <div class="border-light-bottom padding-bottom20">
                                    <h3 class="font18 text-darker-black leftfloat margin-right20"><%= viewModel.Organization %>, <%=viewModel.AreaName %></h3>
                                    <p class="leftfloat text-bold font16 position-rel pos-top2"><span class="bwsprite phone-black-icon"></span><%=viewModel.MaskingNumber %></p>
                                    <div class="clear"></div>
                                </div>
                            </div>
                            <% if (viewModel.Offers != null && viewModel.OfferCount > 0)
                               { %>
                            <div class="font14 content-inner-block-20">
                                <p class="text-bold margin-bottom10">Exclusive offers on this bike from <%=viewModel.Organization %>, <%=viewModel.AreaName %>:</p>
                                <ul class="dealership-benefit-list">
                                    <asp:Repeater ID="rptOffers" runat="server">
                                        <ItemTemplate>
                                            <li>
                                                <span class="benefit-list-image offer-benefit-sprite offerIcon_<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "OfferCategoryId"))%>"></span>
                                                <span class="benefit-list-title"><%# Convert.ToString(DataBinder.Eval(Container.DataItem, "offerText"))%></span>
                                                <span class="tnc font9 <%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "IsOfferTerms"))? string.Empty: "hide" %>" id="<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "OfferId")) %>">View terms</span>
                                            </li>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ul>
                                <div class="clear"></div>
                            </div>
                            <% } %>
                            <div id="dealerAssistance">
                            <div id="buyingAssistance" class="bg-light-grey font14 content-inner-block-20">
                                <p class="text-bold margin-bottom20">Get assistance on buying this bike:</p>
                                <div>
                                    <div class="form-control-box form-control-username leftfloat margin-right20">
                                        <input type="text" class="form-control" placeholder="Name" id="assistGetName" data-bind="textInput: fullName" />
                                        <span class="bwsprite error-icon errorIcon"></span>
                                        <div class="bw-blackbg-tooltip errorText"></div>
                                    </div>
                                    <div class="form-control-box form-control-email-mobile leftfloat margin-right20">
                                        <input type="text" class="form-control" placeholder="Email id" id="assistGetEmail" data-bind="textInput: emailId" />
                                        <span class="bwsprite error-icon errorIcon"></span>
                                        <div class="bw-blackbg-tooltip errorText"></div>
                                    </div>
                                    <div class="form-control-box form-control-email-mobile leftfloat margin-right20">
                                        <p class="mobile-prefix">+91</p>
                                        <input type="text" class="form-control padding-left40" maxlength="10" placeholder="Number" id="assistGetMobile" data-bind="textInput: mobileNo" />
                                        <span class="bwsprite error-icon errorIcon"></span>
                                        <div class="bw-blackbg-tooltip errorText"></div>
                                    </div>
                                    <a class="btn btn-inv-grey leftfloat" leadSourceId="13" id="assistFormSubmit" data-bind="event: { click: submitLead }">Submit</a>
                                    <div class="clear"></div>
                                </div>
                            </div>
                                        <!-- lead capture popup start-->
                                <div id="leadCapturePopup" class="text-center rounded-corner2">
                                    <div class="leadCapture-close-btn position-abt pos-top10 pos-right10 bwsprite cross-lg-lgt-grey cur-pointer"></div>
                                    <!-- contact details starts here -->
                                    <div id="contactDetailsPopup">
                                        <div class="icon-outer-container rounded-corner50">
                                            <div class="icon-inner-container rounded-corner50">
                                                <span class="bwsprite user-contact-details-icon margin-top25"></span>
                                            </div>
                                        </div>
                                        <p class="font20 margin-top25 margin-bottom10">Provide contact details</p>
                                        <p class="text-light-grey margin-bottom20">Dealership will get back to you with offers, EMI quotes, exchange benefits and much more!</p>
                                        <div class="personal-info-form-container">
                                            <div class="form-control-box personal-info-list">
                                                <input type="text" class="form-control get-first-name" placeholder="Name (mandatory)"
                                                    id="getFullName" data-bind="textInput: fullName">
                                                <span class="bwsprite error-icon errorIcon"></span>
                                                <div class="bw-blackbg-tooltip errorText"></div>
                                            </div>
                                            <div class="form-control-box personal-info-list">
                                                <input type="text" class="form-control get-email-id" placeholder="Email address (mandatory)"
                                                    id="getEmailID" data-bind="textInput: emailId">
                                                <span class="bwsprite error-icon errorIcon"></span>
                                                <div class="bw-blackbg-tooltip errorText"></div>
                                            </div>
                                            <div class="form-control-box personal-info-list">
                                                <p class="mobile-prefix">+91</p>
                                                <input type="text" class="form-control padding-left40 get-mobile-no" placeholder="Mobile no. (mandatory)"
                                                    id="getMobile" maxlength="10" data-bind="textInput: mobileNo">
                                                <span class="bwsprite error-icon errorIcon"></span>
                                                <div class="bw-blackbg-tooltip errorText"></div>
                                            </div>
                                            <div class="clear"></div>
                                            <a class="btn btn-orange margin-top10" id="user-details-submit-btn" data-bind="event: { click: submitLead }">Submit</a>
                                        </div>                   
                                    </div>
                                    <!-- contact details ends here -->
                                    <!-- thank you message starts here -->
                                    <div id="notify-response" class="hide margin-top10 content-inner-block-20 text-center">
                                        <div class="icon-outer-container rounded-corner50">
                                            <div class="icon-inner-container rounded-corner50">
                                                <span class="bwsprite user-contact-details-icon margin-top25"></span>
                                            </div>
                                        </div>
                                        <p class="font18 text-bold margin-bottom20">Thank you <span class="notify-leadUser"></span></p>
                                        <% if(viewModel!=null){ %>
                                        <p class="font16 margin-bottom40"><%=viewModel.Organization %>, <%=viewModel.AreaName %> will get in touch with you soon</p>
                                        <% } %>
                                        <input type="button" id="notifyOkayBtn" class="btn btn-orange" value="Okay" />
                                    </div>
                                    <!-- thank you message ends here -->

                                    <!-- otp starts here -->
                                    <div id="otpPopup">
                                        <div class="icon-outer-container rounded-corner50">
                                            <div class="icon-inner-container rounded-corner50">
                                                <span class="bwsprite otp-icon margin-top25"></span>
                                            </div>
                                        </div>
                                        <p class="font18 margin-top25 margin-bottom20">Verify your mobile number</p>
                                        <p class="font14 text-light-grey margin-bottom20">We have sent OTP on your mobile. Please enter that OTP in the box provided below:</p>
                                        <div>
                                            <div class="lead-mobile-box lead-otp-box-container font22">
                                                <span class="bwsprite phone-black-icon"></span>
                                                <span class="text-light-grey">+91</span>
                                                <span class="lead-mobile"></span>
                                                <span class="bwsprite edit-blue-icon edit-mobile-btn"></span>
                                            </div>
                                            <div class="otp-box lead-otp-box-container">
                                                <div class="form-control-box margin-bottom10">
                                                    <input type="text" class="form-control" maxlength="5" placeholder="Enter your OTP" id="getOTP" data-bind="value: otpCode">
                                                    <span class="bwsprite error-icon errorIcon"></span>
                                                    <div class="bw-blackbg-tooltip errorText"></div>
                                                </div>
                                                <a class="resend-otp-btn margin-left10 blue rightfloat resend-otp-btn" id="resendCwiCode" data-bind="visible: (NoOfAttempts() < 2), click: function () { regenerateOTP() }">Resend OTP
                                                </a>
                                                <p class="otp-alert-text margin-left10 otp-notify-text text-light-grey font12 margin-top10" data-bind="visible: (NoOfAttempts() >= 2)">
                                                    OTP has been already sent to your mobile
                                                </p>
                                                <div class="clear"></div>
                                                <%--<p class="resend-otp-btn margin-bottom20" id="resendCwiCode">Resend OTP</p>--%>
                                                <input type="button" class="btn btn-orange margin-top20" value="Confirm OTP" id="otp-submit-btn">
                                            </div>
                                            <div class="update-mobile-box">
                                                <div class="form-control-box text-left">
                                                    <p class="mobile-prefix">+91</p>
                                                    <input type="text" class="form-control padding-left40" placeholder="Mobile no." maxlength="10" id="getUpdatedMobile" data-bind="value: mobileNo" />
                                                    <span class="bwsprite error-icon errorIcon"></span>
                                                    <div class="bw-blackbg-tooltip errorText"></div>
                                                </div>
                                                <input type="button" class="btn btn-orange" value="Send OTP" id="generateNewOTP" data-bind="event: { click: submitLead }" />
                                            </div>
                                        </div>
                                    </div>
                                    <!-- otp ends here -->
                                </div>
                                <!-- lead capture popup End-->
                                </div>
                            <% if(isBookingAvailable && bookingAmt > 0){ %>
                            <div class="font14 text-light-grey content-inner-block-20">
                                <p>The booking amount of <span class="bwsprite inr-sm-grey"></span><%=bookingAmt %> has to be paid online and balance amount of <span class="bwsprite inr-sm-grey"></span><%= price-bookingAmt  %> has to be paid at the dealership. <a href="/pricequote/bookingsummary_new.aspx?MPQ=<%= mpqQueryString %>">Book now</a></p>
                            </div>
                            <% } %>
                        </div>
                        <% } %>
                        <% if (viewModel != null && viewModel.SecondaryDealerCount > 0)
                           { %>
                        <ul id="moreDealersList">
                            <asp:Repeater ID="rptSecondaryDealers" runat="server">
                                <ItemTemplate>
                                    <li>
                                        <a href="javascript:void(0);" onclick="secondarydealer_Click(
                                            <%# Convert.ToString(DataBinder.Eval(Container.DataItem, "DealerId")) %>)" class="font18 text-bold text-darker-black margin-right20 secondary"><%# Convert.ToString(DataBinder.Eval(Container.DataItem, "Name")) %>, <%# Convert.ToString(DataBinder.Eval(Container.DataItem, "Area")) %></a>
                                        <span class="font16 text-bold"><span class="bwsprite phone-black-icon"></span><%# Convert.ToString(DataBinder.Eval(Container.DataItem, "MaskingNumber")) %></span>
                                    </li>
                                </ItemTemplate>
                            </asp:Repeater>
                        <% } %>
                        </ul>
                        <% if (viewModel!=null && viewModel.SecondaryDealerCount > 0)
                           { %>
                        <div class="text-center margin-top20">
                            <a href="javascript:void(0)" class="font14 more-dealers-link">Check price from <%=viewModel.SecondaryDealerCount %> more dealers <span class="font12"><span class="bwsprite chevron-down"></span></span></a>
                            <a href="javascript:void(0)" class="font14 less-dealers-link">Show less dealers <span class="font12"><span class="bwsprite chevron-up"></span></span></a>
                        </div>
                        <%} %>
                    </div>

                </div>
                <div class="clear"></div>
            </div>
           
            <!-- Terms and condition Popup start -->
            <div class="termsPopUpContainer content-inner-block-20 hide" id="termsPopUpContainer">
                <div class="fixed-close-btn-wrapper">
                    <div class="termsPopUpCloseBtn fixed-close-btn bwsprite cross-lg-lgt-grey cur-pointer"></div>
                </div>
                <h3>Terms and conditions</h3>
                <div class="hide" style="vertical-align: middle; text-align: center;" id="termspinner">
                    <img class="lazy" data-original="http://imgd1.aeplcdn.com/0x0/bw/static/sprites/d/loader.gif"  src="" />
                </div>
                <div id="terms" class="breakup-text-container padding-bottom10 font14">
                </div>
                <div id='orig-terms' class='hide'>
                </div>
            </div>
            <!-- Terms and condition Popup Ends -->
        </section>
        

        <meta itemprop="manufacturer" name="manufacturer" content="<%= modelPageEntity.ModelDetails.MakeBase.MakeName %>">  
        <meta itemprop="model" content="<%= TargetedModel %>"/>
        <section id="modelDetailsFloatingCardContent" class="container">
            <div class="grid-12">
                <div class="model-details-floating-card">
                    <div class="content-box-shadow content-inner-block-1020">
                        <div class="grid-5 alpha omega">
                            <div class="model-card-image-content inline-block-top margin-right20">
                                <img  src="<%= modelImage %>" ></img>
                            </div>
                            <div class="model-card-title-content inline-block-top">
                                <p class="font16 text-bold margin-bottom5"><%= bikeName %></p>
                                <p class="font14 text-light-grey"><%= variantText %></p>
                            </div>
                        </div>
                        <div class="grid-4 floating-card-area-details">
                              <% if (!modelPageEntity.ModelDetails.Futuristic)
                               { %>
                                <%if(isDiscontinued){ %>
                                <p class="font14 text-light-grey leftfloat">Last known Ex-showroom price</p>
                                <%} else
                                 if (!isCitySelected)
                                   {%>
                                <p class="font14 text-light-grey exshowroom-area"><span>Ex-showroom price in</span>&nbsp;<span class="font14 exshowroom-area-name text-truncate"><%= Bikewale.Utility.BWConfiguration.Instance.DefaultName %></span></p>
                                <% } %>
                                <% else if (!isOnRoadPrice)
                                   {%>
                                <p class="font14 text-light-grey leftfloat exshowroom-area"><span class="leftfloat">Ex-showroom price in</span><span class="leftfloat text-truncate exshowroom-area-name city-area-name margin-right5"><%= areaName %> <%= cityName %></span></p>
                                <% } %>
                                <% else
                                   {%>
                                <p class="font14 text-light-grey leftfloat"><span class="leftfloat">On-road price in</span><span class="leftfloat text-truncate city-area-name margin-right5"><%= areaName %> <%= cityName %></span></p>

                                <% } %>                          
                            <div class="clear"></div>

                            <div>
                                 <% if (price == 0)
                                    { %>
                                <span class="font18">Price not available</span>
                                <%  } else{ %>
                                <span class="bwsprite inr-lg"></span>&nbsp;<span class="font18 text-bold"><%= Bikewale.Utility.Format.FormatPrice(price.ToString()) %></span>
                                <%} %>
                            </div>
                            <%} 
                    //<!-- upcoming start Floating -->
                             else if (modelPageEntity.UpcomingBike != null)
                               { %>                           
                                <% if (modelPageEntity.UpcomingBike.EstimatedPriceMin != 0 && modelPageEntity.UpcomingBike.EstimatedPriceMax != 0)
                                   { %>                                
                                    <p class="font14 text-light-grey margin-bottom5 leftfloat">Expected Price</p> 
                                    <div class="clear"></div>                                                                     
                                    <span class="bwsprite inr-lg"></span>
                                    <span id="bike-priceFloating" class="font18 text-bold">
                                        <span><%= Bikewale.Utility.Format.FormatNumeric(Convert.ToString(modelPageEntity.UpcomingBike.EstimatedPriceMin)) %></span>
                                        <span>- </span>
                                        <span class="bwsprite inr-lg"></span>
                                        <span><%= Bikewale.Utility.Format.FormatNumeric(Convert.ToString(modelPageEntity.UpcomingBike.EstimatedPriceMax)) %></span>
                                    </span>                                                            
                                    <%}
                                    else
                                    { %>
                                    <p class="font20">Price Unavailable</p>
                                <% } %>                                                                                              
                            <% } %>
                    <!-- upcoming end Floating-->
                        </div>
                        <div class="grid-3 model-orp-btn alpha omega">
                             <% if (toShowOnRoadPriceButton && !isDiscontinued)
                               { %>                            
                             <a href="javascript:void(0)" id="btnCheckOnRoadPriceFloating" ismodel="true" modelid="<%=modelId %>" class="btn btn-orange font14 <%=(viewModel != null && viewModel.IsPremiumDealer && !isBikeWalePQ) ? "margin-top5" : "margin-top20" %> fillPopupData bw-ga" rel="nofollow" c="Model_Page" a="Floating_Card_Check_On_Road_Price_Button_Clicked" v="myBikeName">Check on-road price</a>
                            <%} else
                                    if (viewModel != null && viewModel.IsPremiumDealer && !isBikeWalePQ && !isDiscontinued)
                                    {%>									 
                                     <a href="javascript:void(0)" id="getOffersFromDealerFloating" leadSourceId="24" class="btn btn-orange font14 <%=(viewModel != null && viewModel.IsPremiumDealer && !isBikeWalePQ) ? "margin-top5" : "margin-top20" %> bw-ga" rel="nofollow" c="Model_Page" a="Floating_Card_Get_Offers_Clicked" v="myBikeName">Get offers from dealer</a>
                                    <%} %>
                            
                            <!-- if no 'powered by' text is present remove margin-top5 add margin-top20 in offers button -->
                            <%if (viewModel != null && viewModel.IsPremiumDealer && !isBikeWalePQ)
                              { %>
                            <p class="model-powered-by-text font12 margin-top10 text-truncate"><span class="text-light-grey">Powered by </span><%= viewModel.Organization %></p>
                            <%} %>
                        </div>
                        <div class="clear"></div>
                    </div>
                    <div class="overall-specs-tabs-wrapper content-box-shadow">
                        <% if ((modelPageEntity.ModelDesc != null && !string.IsNullOrEmpty(modelPageEntity.ModelDesc.SmallDescription)) || modelPageEntity.ModelVersionSpecs != null)
                           { %>
                        <a href="#modelSummaryContent" rel="nofollow">About</a>
                        <%} %>
                        <% if(modelPageEntity.ModelVersions!= null && modelPageEntity.ModelVersions.Count > 0) { %>
                        <a href="#modelPricesContent" rel="nofollow">Prices</a>
                         <% } %>
                         <% if(modelPageEntity.ModelVersionSpecs != null ){ %>
                        <a href="#modelSpecsFeaturesContent" rel="nofollow">Specs & Features</a>
                        <% } %>
                        <%if (modelPageEntity.ModelColors != null && modelPageEntity.ModelColors.Count() > 0)
                        { %>
                        <a href="#modelColorsContent" rel="nofollow">Colours</a>
                        <%} %>
                        <% if (ctrlExpertReviews.FetchedRecordsCount > 0 || ctrlUserReviews.FetchedRecordsCount > 0)
                           { %>
                        <a href="#modelReviewsContent" rel="nofollow">Reviews</a>
                        <%} %>
                        <% if (ctrlVideos.FetchedRecordsCount > 0)
                            { %>
                            <a href="#modelVideosContent" rel="nofollow">Videos</a>
                        <%} %>
                         <% if (ctrlNews.FetchedRecordsCount > 0)
                             { %>
                        <a href="#modelNewsContent" rel="nofollow">News</a><%} %>
                          <% if (ctrlAlternativeBikes.FetchedRecordsCount > 0) { %>
                        <a href="#modelAlternateBikeContent" rel="nofollow">Alternatives</a>  
                        <%} %>                   
                    </div>
                </div>
            </div>
        </section>

        <section class="container">
            <div id="modelSpecsTabsContentWrapper" class="grid-12 margin-bottom20">
                <div class="content-box-shadow">
                    <div class="overall-specs-tabs-wrapper">
                        <% if ((modelPageEntity.ModelDesc != null && !string.IsNullOrEmpty(modelPageEntity.ModelDesc.SmallDescription)) || modelPageEntity.ModelVersionSpecs != null)
                           { %>
                        <a class="active" href="#modelSummaryContent" rel="nofollow">About</a>
                        <%} %>
                         <% if(modelPageEntity.ModelVersions!= null && modelPageEntity.ModelVersions.Count > 0) { %>
                        <a href="#modelPricesContent" rel="nofollow">Prices</a>
                         <% } %>
                        <% if(modelPageEntity.ModelVersionSpecs != null ){ %>
                        <a href="#modelSpecsFeaturesContent" rel="nofollow">Specs & Features</a>
                         <% } %>
                        <%if (modelPageEntity.ModelColors != null && modelPageEntity.ModelColors.Count() > 0)
                        { %>
                        <a href="#modelColorsContent" rel="nofollow">Colours</a>
                        <%} %>
                        <% if (ctrlExpertReviews.FetchedRecordsCount > 0 || ctrlUserReviews.FetchedRecordsCount > 0)
                           { %>
                        <a href="#modelReviewsContent" rel="nofollow">Reviews</a>
                        <%} %>
                        <% if (ctrlVideos.FetchedRecordsCount > 0)
                            { %>
                           <a href="#modelVideosContent" rel="nofollow">Videos</a>
                        <%} %>
                          <% if (ctrlNews.FetchedRecordsCount > 0)
                             { %>
                        <a href="#modelNewsContent" rel="nofollow">News</a>
                        <%} %>

                        <% if (ctrlAlternativeBikes.FetchedRecordsCount > 0) { %>
                        <a href="#modelAlternateBikeContent" rel="nofollow">Alternatives</a> 
                        <%} %>                      
                    </div>
                    <div class="border-divider"></div>

                    <% if ((modelPageEntity.ModelDesc != null && !string.IsNullOrEmpty(modelPageEntity.ModelDesc.SmallDescription)) || modelPageEntity.ModelVersionSpecs !=null)
                       { %>
                    <div id="modelSummaryContent" class="bw-model-tabs-data margin-right10 margin-left10 content-inner-block-2010 border-solid-bottom">
                        <%if(modelPageEntity.ModelDesc != null && !string.IsNullOrEmpty(modelPageEntity.ModelDesc.SmallDescription)){ %>
                        <div class="grid-8 alpha margin-bottom20">
                            <h2>About <%=bikeName %></h2>
                            <h3>Preview</h3>
                            <p class="font14 text-light-grey line-height17">
                                <span class="model-preview-main-content">
                                    <%= modelPageEntity.ModelDesc.SmallDescription %>
                                </span>
                                <span class="model-preview-more-content hide" style="display: none;">
                                    <%= modelPageEntity.ModelDesc.FullDescription %>
                                </span>
                                <%if (!string.IsNullOrEmpty(modelPageEntity.ModelDesc.SmallDescription)) { %>
                                <a href="javascript:void(0)" class="read-more-model-preview" rel="nofollow">Read more</a>
                                <% } %>
                            </p>
                        </div>
                        <div class="grid-4 text-center alpha omega margin-bottom20">
                            <!-- #include file="/ads/Ad300x250.aspx" -->
                        </div>
                        <%} %>
                        <div class="clear"></div>
                         <% if(modelPageEntity.ModelVersionSpecs != null){ %>
                        <h3>Specification summary</h3>
                        <div class="grid-3 border-light-right omega">
                            <span class="inline-block model-sprite specs-capacity-icon margin-right30" title="<%=bikeName %> Engine Capacity"></span>
                            <div class="inline-block">
                                <p class="font22 text-bold margin-bottom5"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.Displacement) %><span> cc</span></p>
                                <p class="font16 text-light-grey">Capacity</p>
                            </div>
                        </div>
                        <div class="grid-3 padding-left40 border-light-right omega">
                            <span class="inline-block model-sprite specs-mileage-icon margin-right30" title="<%=bikeName %> Mileage"></span>
                            <div class="inline-block">
                                <p class="font22 text-bold margin-bottom5"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.FuelEfficiencyOverall) %><span> kmpl</span></p>
                                <p class="font16 text-light-grey">Mileage</p>
                            </div>
                        </div>
                        <div class="grid-3 padding-left60 border-light-right omega">
                            <span class="inline-block model-sprite specs-maxpower-icon margin-right30" title="<%=bikeName %> Max Power"></span>
                            <div class="inline-block">
                                <p class="font22 text-bold margin-bottom5"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.MaxPower) %><span> bhp</span></p>
                                <p class="font16 text-light-grey">Max power</p>
                            </div>
                        </div>
                        <div class="grid-3 padding-left50 omega">
                            <span class="inline-block model-sprite specs-weight-icon margin-right30" title="<%=bikeName %> Kerb Weight"></span>
                            <div class="inline-block">
                                <p class="font22 text-bold margin-bottom5"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.KerbWeight) %><span> kgs</span></p>
                                <p class="font16 text-light-grey">Weight</p>
                            </div>
                        </div>
                        <div class="clear"></div>
                          <% } %>
                    </div>
                    
                    <%} %>

                    <script type="text/javascript" src="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/src/frameworks.js?<%=staticFileVersion %>"></script>

                    <% if(modelPageEntity.ModelVersions!= null && modelPageEntity.ModelVersions.Count > 0) { %>
                    <div id="modelPricesContent" class="bw-model-tabs-data margin-right10 margin-left10 padding-top20 padding-right10 padding-bottom15 padding-left10 border-solid-bottom">
                        <h2><%=bikeName %> Prices</h2>
                        <div id="prices-by-version-content" class="grid-8 alpha padding-right20">
                            <h3 class="margin-bottom20">Prices by versions</h3>
                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                <thead>
                                    <tr>
                                        <th align="left" width="65%" class="font12 text-unbold text-xt-light-grey padding-bottom5 border-solid-bottom">Version</th>
                                        <th align="left" width="35%" class="font12 text-unbold text-xt-light-grey padding-bottom5 border-solid-bottom">Price</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:Repeater runat="server" ID="rptVarients" OnItemDataBound="rptVarients_ItemDataBound">
                                        <ItemTemplate>
                                            <tr class="version-prices-tr">
                                                <td width="70%" class="padding-bottom15 padding-top10 padding-right10 font14" valign="top">
                                                    <p class="margin-bottom5"><%# Convert.ToString(DataBinder.Eval(Container.DataItem, "VersionName")) %></p>
                                                    <p class="font12 text-xt-light-grey"><%# FormatVarientMinSpec(Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "AlloyWheels")),Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "ElectricStart")),Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "AntilockBrakingSystem")),Convert.ToString(DataBinder.Eval(Container.DataItem, "BrakeType"))) %></p>
                                                </td>
                                                <td width="30%" class="padding-bottom10 padding-top10 padding-right10 font14 divider-bottom" valign="top">
                                                    <p class="font16 text-bold text-black">
                                                        <span class="bwsprite inr-md"></span>
                                                        <span><asp:Label Text='<%# Bikewale.Utility.Format.FormatPrice(Convert.ToString(DataBinder.Eval(Container.DataItem, "Price"))) %>' ID="txtComment" runat="server"></asp:Label></span>
                                                    </p>
                                                    <p class="font12 text-xt-dark-grey">
                                                        <asp:Label ID="lblExOn" Text="Ex-showroom" runat="server"></asp:Label>, 
                                                        <% if (cityId != 0)
                                                            { %>
                                                        <span><%= cityName %></span>
                                                        <% }
                                                            else
                                                            { %>
                                                        <span><%= Bikewale.Common.Configuration.GetDefaultCityName %></span>
                                                        <% } %>                                                   
                                                    </p>
                                                </td>
                                                <asp:HiddenField ID="hdnVariant" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "VersionId") %>' />
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tbody>
                            </table>
                        </div>
                       <BW:TopCityPrice ID="ctrlTopCityPrices" runat="server" />
                        <div class="clear"></div>
                    </div>
                     <% } %>

                    <% if(modelPageEntity.ModelVersionSpecs != null ){ %>
                    <div id="modelSpecsFeaturesContent" class="bw-model-tabs-data padding-top20 font14">
                        <h2 class="padding-left20 padding-right20"><%=bikeModelName%> Specifications & Features</h2>
                        <h3 class="padding-left20">Specifications</h3>

                        <ul id="model-specs-list">
                            <li>
                                <div class="model-accordion-tab active">
                                    <span class="model-sprite engine-sm-icon margin-right10"></span>
                                    <span class="inline-block">Engine & transmission</span>
                                    <span class="bwsprite accordion-angle-icon"></span>
                                </div>
                                <div class="specs-features-list">
                                    <div class="grid-6">
                                        <div class="grid-6 text-light-grey">
                                            <p>Displacement</p>
                                            <p>Cylinders</p>
                                            <p>Max Power</p>
                                            <p>Maximum Torque</p>
                                            <p>Bore</p>
                                            <p>Stroke</p>
                                            <p>Valves Per Cylinder</p>
                                            <p>Fuel Delivery System</p>
                                        </div>
                                        <div class="grid-6 text-bold">
                                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.Displacement) %> <span>cc</span></p>
                                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.Cylinders) %></p>
                                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.MaxPower, "bhp", modelPageEntity.ModelVersionSpecs.MaxPowerRPM, "rpm") %></p>
                                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.MaximumTorque, "Nm", modelPageEntity.ModelVersionSpecs.MaximumTorqueRPM,"rpm") %></p>
                                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.Bore,"mm") %></p>
                                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.Stroke,"mm") %></p>
                                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.ValvesPerCylinder) %></p>
                                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.FuelDeliverySystem) %></p>
                                        </div>
                                        <div class="clear"></div>
                                    </div>
                                    <div class="grid-6">
                                        <div class="grid-6 text-light-grey">
                                            <p>Fuel Type</p>
                                            <p>Ignition</p>
                                            <p>Spark Plugs</p>
                                            <p>Cooling System</p>
                                            <p>Gearbox Type</p>
                                            <p>No. of Gears</p>
                                            <p>Transmission Type</p>
                                            <p>Clutch</p>
                                        </div>
                                        <div class="grid-6 text-bold">
                                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.FuelType) %></p>
                                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.Ignition) %></p>
                                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.SparkPlugsPerCylinder, "Per Cylinder") %></p>
                                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.CoolingSystem) %></p>
                                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.GearboxType) %></p>
                                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.NoOfGears) %></p>
                                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.TransmissionType) %></p>
                                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.Clutch) %></p>
                                        </div>
                                        <div class="clear"></div>
                                    </div>
                                    <div class="clear"></div>
                                </div>
                            </li>
                            <li>
                                <div class="model-accordion-tab">
                                    <span class="model-sprite brakes-sm-icon margin-right10"></span>
                                    <span class="inline-block">Brakes, wheels & suspension</span>
                                    <span class="bwsprite accordion-angle-icon"></span>
                                </div>
                                <div class="specs-features-list">
                                    <div class="grid-6">
                                        <div class="grid-6 text-light-grey">
                                            <p>Brake Type</p>
                                            <p>Front Disc</p>
                                            <p>Front Disc/Drum Size</p>
                                            <p>Rear Disc</p>
                                            <p>Rear Disc/Drum Size</p>
                                            <p>Calliper Type</p>
                                            <p>Wheel Size</p>
                                        </div>
                                        <div class="grid-6 text-bold">
                                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.BrakeType) %></p>
                                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.FrontDisc) %></p>
                                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.FrontDisc_DrumSize,"mm") %></p>
                                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.RearDisc) %></p>
                                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.RearDisc_DrumSize,"mm") %></p>
                                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.CalliperType) %></p>
                                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.WheelSize,"inches") %></p>
                                        </div>
                                        <div class="clear"></div>
                                    </div>
                                    <div class="grid-6">
                                        <div class="grid-6 text-light-grey">
                                            <p>Front Tyre</p>
                                            <p>Rear Tyre</p>
                                            <p>Tubeless Tyres</p>
                                            <p>Radial Tyres</p>
                                            <p>Alloy Wheels</p>
                                            <p>Front Suspension</p>
                                            <p>Rear Suspension</p>
                                        </div>
                                        <div class="grid-6 text-bold">
                                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.FrontTyre) %></p>
                                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.RearTyre) %></p>
                                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.TubelessTyres) %></p>
                                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.RadialTyres) %></p>
                                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.AlloyWheels) %></p>
                                            <p title="<%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.FrontSuspension) %>"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.FrontSuspension) %></p>
                                            <p title="<%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.RearSuspension) %>"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.RearSuspension) %></p>
                                        </div>
                                        <div class="clear"></div>
                                    </div>
                                    <div class="clear"></div>
                                </div>
                            </li>
                            <li>
                                <div class="model-accordion-tab">
                                    <span class="model-sprite dimension-sm-icon margin-right10"></span>
                                    <span class="inline-block">Dimensions & chasis</span>
                                    <span class="bwsprite accordion-angle-icon"></span>
                                </div>
                                <div class="specs-features-list">
                                    <div class="grid-6">
                                        <div class="grid-6 text-light-grey">
                                            <p>Kerb Weight</p>
                                            <p>Overall Length</p>
                                            <p>Overall Width</p>
                                            <p>Overall Height</p>
                                        </div>
                                        <div class="grid-6 text-bold">
                                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.KerbWeight,"kg") %></p>
                                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.OverallLength,"mm") %></p>
                                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.OverallWidth,"mm") %></p>
                                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.OverallHeight,"mm") %></p>
                                        </div>
                                        <div class="clear"></div>
                                    </div>
                                    <div class="grid-6">
                                        <div class="grid-6 text-light-grey">
                                            <p>Wheelbase</p>
                                            <p>Ground Clearance</p>
                                            <p>Seat Height</p>
                                            <p>Chassis Type</p>
                                        </div>
                                        <div class="grid-6 text-bold">
                                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.Wheelbase,"mm") %></p>
                                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.GroundClearance, "mm") %></p>
                                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.SeatHeight,"mm") %></p>
                                            <p title="<%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.ChassisType) %>"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.ChassisType) %></p>
                                        </div>
                                        <div class="clear"></div>
                                    </div>
                                    <div class="clear"></div>
                                </div>
                            </li>
                            <li>
                                <div class="model-accordion-tab">
                                    <span class="model-sprite fuel-sm-icon margin-right10"></span>
                                    <span class="inline-block">Fuel effeciency & performance</span>
                                    <span class="bwsprite accordion-angle-icon"></span>
                                </div>
                                <div class="specs-features-list">
                                    <div class="grid-6">
                                        <div class="grid-6 text-light-grey">
                                            <p>Fuel Tank Capacity</p>
                                            <p>Reserve Fuel Capacity</p>
                                            <p>Fuel Efficiency Overall</p>
                                            <p>Fuel Efficiency Range</p>
                                            <p>Top Speed</p>
                                        </div>
                                        <div class="grid-6 text-bold">
                                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.FuelTankCapacity,"litres") %></p>
                                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.ReserveFuelCapacity,"litres") %></p>
                                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.FuelEfficiencyOverall,"kmpl") %></p>
                                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.FuelEfficiencyRange,"km") %></p>
                                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.TopSpeed,"kmph") %></p>
                                        </div>
                                        <div class="clear"></div>
                                    </div>
                                    <div class="grid-6">
                                        <div class="grid-6 text-light-grey">
                                            <p>0 to 60 kmph</p>
                                            <p>0 to 80 kmph</p>
                                            <p>0 to 40 kmph</p>
                                            <p>60 to 0 kmph</p>
                                            <p>80 to 0 kmph</p>
                                        </div>
                                        <div class="grid-6 text-bold">
                                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.Performance_0_60_kmph,"seconds") %></p>
                                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.Performance_0_80_kmph,"seconds") %></p>
                                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.Performance_0_40_m,"seconds") %></p>
                                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.Performance_60_0_kmph) %></p>
                                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.Performance_80_0_kmph) %></p>
                                        </div>
                                        <div class="clear"></div>
                                    </div>
                                    <div class="clear"></div>
                                </div>
                            </li>
                        </ul>
                        
                        <div id="model-features-content" class="margin-top20 margin-bottom20">
                            <h3 class="padding-left20">Features</h3>
                            <div class="specs-features-list model-features-list">
                                <div class="grid-6 alpha">
                                    <div class="grid-6 padding-left20 text-light-grey">
                                        <p>Speedometer</p>
                                        <p>Fuel Guage</p>
                                        <p>Tachometer Type</p>
                                        <p>Tachometer</p>
                                    </div>
                                    <div class="grid-6 omega text-bold">
                                        <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.Speedometer) %></p>
                                        <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.FuelGauge) %></p>
                                        <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.TachometerType) %></p>
                                        <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.Tachometer) %></p>
                                    </div>
                                    <div class="clear"></div>
                                </div>
                                <div class="grid-6">
                                    <div class="grid-6 padding-left20 text-light-grey">
                                        <p>Digital Fuel Guage</p>
                                        <p>Tripmeter</p>
                                        <p>Electric Start</p>
                                        <p>Shift Light</p>
                                    </div>
                                    <div class="grid-6 omega text-bold">
                                        <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.DigitalFuelGauge) %></p>
                                        <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.Tripmeter) %></p>
                                        <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.ElectricStart) %></p>
                                        <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.ShiftLight) %></p>
                                    </div>
                                    <div class="clear"></div>
                                </div>
                                <div class="clear"></div>
                            </div>

                            <div id="model-more-features-list" class="specs-features-list model-features-list">
                                <div class="grid-6 alpha">
                                    <div class="grid-6 padding-left20 text-light-grey">
                                        <p>Stand Alarm</p>
                                        <p>Stepped Seat</p>
                                        <p>No. of Tripmeters</p>
                                        <p>Tripmeter Type</p>
                                        <p>Low Fuel Indicator</p>
                                        <p>Low Oil Indicator</p>
                                        <p>Low Battery Indicator</p>
                                        <p>Pillion Backrest</p>
                                        <p>Pillion Grabrail</p>
                                        <p>Pillion Seat</p>
                                        <p>Pillion Footrest</p>
                                    </div>
                                    <div class="grid-6 omega text-bold">
                                        <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.StandAlarm) %></p>
                                        <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.SteppedSeat) %></p>
                                        <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.NoOfTripmeters) %></p>
                                        <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.TripmeterType) %></p>
                                        <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.LowFuelIndicator) %></p>
                                        <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.LowOilIndicator) %></p>
                                        <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.LowBatteryIndicator) %></p>
                                        <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.PillionBackrest) %></p>
                                        <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.PillionGrabrail) %></p>
                                        <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.PillionSeat) %></p>
                                        <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.PillionFootrest) %></p>
                                    </div>
                                    <div class="clear"></div>
                                </div>
                                <div class="grid-6">
                                    <div class="grid-6 padding-left20 text-light-grey">
                                        <p>Antilock Braking System</p>
                                        <p>Killswitch</p>
                                        <p>Clock</p>
                                        <p>Electric System</p>
                                        <p>Battery</p>
                                        <p>Headlight Type</p>
                                        <p>Headlight Bulb Type</p>
                                        <p>Brake/Tail Light</p>
                                        <p>Turn Signal</p>
                                        <p>Pass Light</p>
                                    </div>
                                    <div class="grid-6 omega text-bold">
                                        <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.AntilockBrakingSystem) %></p>
                                        <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.Killswitch) %></p>
                                        <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.Clock) %></p>
                                        <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.ElectricSystem) %></p>
                                        <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.Battery) %></p>
                                        <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.HeadlightType) %></p>
                                        <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.HeadlightBulbType) %></p>
                                        <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.Brake_Tail_Light) %></p>
                                        <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.TurnSignal) %></p>
                                        <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.PassLight) %></p>
                                    </div>
                                    <div class="clear"></div>
                                </div>
                                <div class="clear"></div>
                            </div>

                            <div class="padding-right20 padding-left20">
                                <a href="javascript:void(0)" class="view-features-link bw-ga" c="Model_Page" a="View_all_features_link_cliked" v="myBikeName" title="<%=bikeName %> Features" rel="nofollow">View all features</a>
                            </div>
                        </div>

                        <div class="margin-right10 margin-left10 border-solid-top"></div>
                    </div>
                    <% } %>
                     
                    <%if (modelPageEntity.ModelColors != null && modelPageEntity.ModelColors.Count() > 0)
                        { %>
                    <div id="modelColorsContent" class="bw-model-tabs-data padding-top20 font14">
                        <h2 class="padding-left20 padding-right20"><%=bikeName %> Colours</h2>
                        <ul id="modelColorsList">
                            <asp:Repeater ID="rptColor" runat="server">
                                <ItemTemplate>
                                    <li>                                                                                                                                     
                                        <div class="color-box inline-block <%# (((IList)(DataBinder.Eval(Container.DataItem, "HexCodes"))).Count == 1 )?"color-count-one": (((IList)(DataBinder.Eval(Container.DataItem, "HexCodes"))).Count >= 3 )?"color-count-three":"color-count-two" %>">
                                            <asp:Repeater runat="server" DataSource='<%# DataBinder.Eval(Container.DataItem, "HexCodes") %>'>
                                                <ItemTemplate>
                                                    <span <%# String.Format("style='background-color: #{0}'",Convert.ToString(Container.DataItem)) %>></span>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </div>
                                        <p class="font16 inline-block text-truncate"><%# Convert.ToString(DataBinder.Eval(Container.DataItem, "ColorName")) %></p>
                                    </li>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ul>
                        <div class="margin-right10 margin-left10 border-solid-top"></div>
                    </div>
                    <%} %>                    
                      
                    <%if (ctrlExpertReviews.FetchedRecordsCount > 0 || ctrlUserReviews.FetchedRecordsCount > 0)
                      { %>
                    <div id="modelReviewsContent" class="bw-model-tabs-data margin-right10 margin-left10 padding-top20 padding-bottom20 border-solid-bottom font14">
                        <h2 class="padding-right10 padding-left10"><%= bikeName %> Reviews</h2>
                        <% if(ctrlExpertReviews.FetchedRecordsCount > 0){ %>
                        <!-- expert review starts-->
                        <BW:ExpertReviews runat="server" ID="ctrlExpertReviews" />
                        <!-- expert review ends-->
                        <% if (ctrlExpertReviews.FetchedRecordsCount > 0 && ctrlUserReviews.FetchedRecordsCount > 0){ %>
                            <div class="margin-bottom20"></div>
                        <% } %>
                        <% } %>

                        <% if (ctrlUserReviews.FetchedRecordsCount > 0){ %>
                        <!-- user reviews -->
                        <BW:UserReviews runat="server" ID="ctrlUserReviews" />
                        <!-- user reviews ends -->
                         <% } %>
                    </div>
                    <%} %>

                    <% if (ctrlVideos.FetchedRecordsCount > 0)
                    { %>
                    <div id="modelVideosContent" class="bw-model-tabs-data margin-right10 margin-left10 padding-top20 padding-bottom20 border-solid-bottom font14">
                        
                        <!-- Video reviews -->
                        <BW:Videos runat="server" ID="ctrlVideos" />
                        <!-- Video reviews ends -->
                    </div>
                    <% } %>

                    <% if (ctrlNews.FetchedRecordsCount > 0)
                       { %>
                    <!-- News widget starts -->                    
                    <BW:LatestNews runat="server" ID="ctrlNews" />
                    <!-- News widget ends -->
                    <% } %>  

                    <% if (ctrlAlternativeBikes.FetchedRecordsCount > 0) { %>
                    <!-- Alternative reviews ends -->
                    <BW:AlternativeBikes ID="ctrlAlternativeBikes" runat="server" />
                    <!-- Alternative reviews ends -->
                    <% } %>
                    <div id="overallSpecsDetailsFooter"></div>
                </div>
            </div>
            <div class="clear"></div>
        </section>   
          
        <%            
            if (ctrlUserReviews.FetchedRecordsCount > 0)
            {
                reviewTabsCnt++;
                isUserReviewZero = false;
                isUserReviewActive = true;
            }
            if (ctrlExpertReviews.FetchedRecordsCount > 0)
            {
                reviewTabsCnt++;
                isExpertReviewZero = false;
                if (!isUserReviewActive)
                {
                    isExpertReviewActive = true;
                }
            }
            if (ctrlNews.FetchedRecordsCount > 0)
            {
                reviewTabsCnt++;
                isNewsZero = false;
                if (!isUserReviewActive && !isExpertReviewActive)
                {
                    isNewsActive = true;
                }
            }
            if (ctrlVideos.FetchedRecordsCount > 0)
            {
                reviewTabsCnt++;
                isVideoZero = false;
                if (!isUserReviewActive && !isExpertReviewActive && !isNewsActive)
                {
                    isVideoActive = true;
                }
            } 
        %>       

        <!-- Check On-Road Price popup -->
        <div id="onRoadPricePopup" class="rounded-corner2 content-inner-block-20 text-center hide">
            <div class="onroadPriceCloseBtn position-abt pos-top20 pos-right20 bwsprite cross-lg-lgt-grey cur-pointer"></div>
            <div class="form-control-box padding-top30">
                <select id="ddlCity" data-bind="options: cities, optionsText: 'cityName', optionsValue: 'cityId', value: selectedCity, optionsCaption: 'Select City', chosen: { width: '190px' }"></select>
                <%--<input type="text" class="form-control" placeholder="Type to select city" id="orpCity">--%>
            </div>
            <div class="form-control-box padding-top30">
                <select id="ddlArea" data-bind="options: areas, optionsText: 'areaName', optionsValue: 'areaId', value: selectedArea, optionsCaption: 'Select Area', chosen: { width: '190px' }"></select>
                <%-- <input type="text" class="form-control" placeholder="Type to select area" id="orpArea">--%>
            </div>
            <input type="button" value="Confirm" class="btn btn-orange margin-top40" id="onroadPriceConfirmBtn">
        </div>

        <BW:ModelGallery ID="ctrlModelGallery" runat="server" />
        <!-- #include file="/includes/footerBW.aspx" -->
        <!--[if lt IE 9]>
            <script src="/src/html5.js"></script>
        <![endif]-->
        <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/css/bw-common-btf.css?<%=staticFileVersion %>" rel="stylesheet" type="text/css" />
        <link href="<%= !string.IsNullOrEmpty(staticUrl) ? "http://st2.aeplcdn.com" + staticUrl : string.Empty %>/css/model-btf.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
        <script type="text/javascript" src="<%= staticUrl != string.Empty ? "http://st2.aeplcdn.com" + staticUrl : string.Empty %>/src/common.min.js?<%= staticFileVersion %>"></script>
        <script type="text/javascript" src="<%= staticUrl != string.Empty ? "http://st2.aeplcdn.com" + staticUrl : string.Empty %>/src/model.js?<%= staticFileVersion %>"></script>
        <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,700' rel='stylesheet' type='text/css' />

        <script type="text/javascript">
            ga_pg_id = '2';
            
            // Cache selectors outside callback for performance.
            var leadSourceId;
           
            var getCityArea = GetGlobalCityArea();
            if (bikeVersionLocation == '') {
                bikeVersionLocation = getBikeVersionLocation();
            }
            if (bikeVersion == '') {
                bikeVersion = getBikeVersion();
            }
            function secondarydealer_Click(dealerID) {
                var rediurl = "CityId=" + cityId + "&AreaId=" + areaId + "&PQId=" + pqId + "&VersionId=" + versionId + "&DealerId=" + dealerID;
                window.location.href = "/pricequote/dealerpricequote.aspx?MPQ=" + Base64.encode(rediurl);
            }
            $(function () {
                if ($('.dealership-benefit-list li').length % 2 == 0) {
                    $('.dealership-benefit-list').addClass("dealer-two-offers");
                }
            });
        </script>
    </form>
</body>
</html>