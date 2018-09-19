<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.New.CompareBikeDetails" Trace="false" %>

<%@ Register Src="~/UI/m/controls/SimilarCompareBikes.ascx" TagPrefix="BW" TagName="SimilarBikes" %>
<!DOCTYPE html>
<html>
<head>
    <%
        if (pageMetas != null)
        {
            title = pageMetas.Title;
            keywords = pageMetas.Keywords;
            description = pageMetas.Description;
            canonical = pageMetas.CanonicalUrl;
            AdPath = "/1017752/Bikewale_Mobile_NewBikes_";
            AdId = "1398766302464";
            TargetedModels = targetedModels;
        }
    %>
    <!-- #include file="/UI/includes/headscript_mobile_min.aspx" -->
    <link rel="stylesheet" type="text/css" href="/UI/m/css/compare/details.css" />
    <script type="text/javascript">
        <!-- #include file="\UI\includes\gacode_mobile.aspx" -->
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <!-- #include file="/UI/includes/headBW_Mobile.aspx" -->

        <% if (vmCompare != null)
            { %>
        <section>
            <% if (vmCompare.BasicInfo != null)
                { %>
            <div <%= !isSponsoredBike ? "" : "id='sponsored-column-active'"  %>>
                <%-- add sponsored-column-active for sponsored bike--%>
                <div class="container box-shadow bg-white card-bottom-margin bw-tabs-panel">
                    <h1 class="box-shadow padding-15-20 margin-bottom3 text-bold"><%= comparisionText %></h1>
                    <div class="comparison-main-card">
                        <% foreach (var bike in vmCompare.BasicInfo)
                            {
                                string bikeName = string.Format("{0} {1}", bike.Make, bike.Model); %>
                        <div class="bike-details-block <%= (bike.VersionId != sponsoredVersionId ) ? "" : "sponsored-bike-details-block" %>" data-masking="<%= string.Format("{0}-{1}",bike.MakeMaskingName,bike.ModelMaskingName)%>" data-changed="" data-modelid="<%= bike.ModelId %>" data-versionid="<%= bike.VersionId %>" data-bikename="<%= String.Format("{0}_{1}_{2}",bike.Make,bike.Model,bike.Version) %>">
                            <% if (bike.VersionId == sponsoredVersionId)
                                {%>
                            <span class="position-abt pos-top5 label-text">Sponsored</span>
                            <% } %>
                            <span <%= (bike.VersionId != sponsoredVersionId ) ? "" : "id='close-sponsored-bike'" %> class="close-selected-bike position-abt pos-right5 bwmsprite cross-sm-dark-grey"></span>
                            <a href="<%= string.Format("/m/{0}-bikes/{1}/",bike.MakeMaskingName,bike.ModelMaskingName) %>" title="<%= bikeName %>" class="block margin-top10 <%= !String.IsNullOrEmpty(sponsoredBikeImpressionTracker) ? "bw-ga" : "" %>" <%= sponsoredBikeImpressionTracker %>>
                                <span class="font10 text-light-grey text-truncate"><%= bike.Make  %></span>
                                <h2 class="font12 text-truncate margin-bottom5"><%= bike.Model  %></h2>
                                <img class="bike-image-block" src="<%= Bikewale.Utility.Image.GetPathToShowImages(bike.ImagePath,bike.HostUrl,Bikewale.Utility.ImageSize._110x61) %>" alt="<%= bikeName %>" />
                            </a>
                            <p class="label-text">Version:</p>


                            <% if (bike.Versions.Count() > 1)
                                { %>
                            <div class="dropdown-select-wrapper">
                                <select class="dropdown-select" data-title="Version">
                                    <% foreach (var v in bike.Versions)
                                        { %>
                                    <option value="<%= v.VersionId %>" <%= (bike.VersionId!=v.VersionId)?"":" selected" %>><%= v.VersionName %></option>
                                    <% } %>
                                </select>
                            </div>
                            <% }
                                else
                                { %>
                            <p class="dropdown-selected-item option-count-one dropdown-width"><%= bike.Version %></p>
                            <% } %>

                            <p class="text-truncate label-text">Ex-showroom, <%= Bikewale.Utility.BWConfiguration.Instance.DefaultName %></p>
                            <p class="margin-bottom10">
                                <span class="bwmsprite inr-xsm-icon"></span><span class="font16 text-bold"><%= Bikewale.Utility.Format.FormatPrice(bike.Price.ToString()) %></span>
                            </p>
                            <% if ((bike.VersionId != sponsoredVersionId) || string.IsNullOrEmpty(featuredBike))
                                { %>
                            <div>
                                <a href="javascript:void(0)" class="btn btn-white bike-orp-btn getquotation" data-modelid="<%= bike.ModelId %>" rel="nofollow">On-road price</a>
                            </div>
                            <% }
                                else
                                { %>
                            <% if (!String.IsNullOrEmpty(featuredBike))
                                { %>
                            <div class="padding-top5 padding-bottom5">
                                <span onclick="<%= String.Format("window.open('{0}', '_blank')",featuredBike)  %>" title="View <%= bike.Model  %> details on <%=bike.Make %>'s site" data-cat="Comparison_Page" data-act="Sponsored_Comparison_Know_more_clicked" data-lab="<%= String.Format("{0}_Top Card",bike.Name) %>" class="font14 bw-ga text-link know-more-btn-shown"><%= knowMoreLinkText %><span class="bwmsprite know-more-icon margin-left5"></span></span>
                            </div>
                            <% } %>
                            <% } %>
                        </div>
                        <% } %>
                        <div class="clear"></div>
                    </div>
                    <div id="comparison-floating-card" class="box-shadow slideIn-transition">
                        <% foreach (var bike in vmCompare.BasicInfo)
                            {   %>
                        <div class="bike-details-block <%= (bike.VersionId != sponsoredVersionId ) ? "" : "sponsored-bike-details-block" %>">
                            <% if (bike.VersionId == sponsoredVersionId)
                                {%>
                            <span class="position-abt pos-top5 label-text">Sponsored</span>
                            <% } %>
                            <span class="font10 text-light-grey text-truncate"><%= bike.Make  %></span>
                            <a href="<%= string.Format("/m/{0}-bikes/{1}/",bike.MakeMaskingName,bike.ModelMaskingName) %>" class="font12 text-truncate bike-title-target"><%= bike.Model %></a>
                            <% if ((bike.VersionId != sponsoredVersionId) || string.IsNullOrEmpty(featuredBike))
                                { %>
                            <div>
                                <a href="javascript:void(0)" class="btn btn-white bike-orp-btn getquotation" data-pqsourceid="<%= (int)Bikewale.Entities.PriceQuote.PQSourceEnum.Mobile_CompareBike %>" data-modelid="<%= bike.ModelId %>" rel="nofollow">On-road price</a>
                            </div>
                            <% }
                                else
                                { %>
                            <% if (!String.IsNullOrEmpty(featuredBike))
                                { %>
                            <div class="padding-top5 padding-bottom5">
                                <span onclick="<%= String.Format("window.open('{0}', '_blank')",featuredBike)  %>" title="View <%= bike.Model  %> details on <%=bike.Make %>'s site" data-cat="Comparison_Page" data-act="Sponsored_Comparison_Know_more_clicked" data-lab="<%= String.Format("{0}_Floating Card",bike.Name) %>" class="font14 bw-ga  text-link"><%= knowMoreLinkText %><span class="bwmsprite know-more-icon margin-left5"></span></span>
                            </div>
                            <% } %>
                            <% } %>
                        </div>
                        <% } %>
                        <div class="clear"></div>
                        <div class="overall-specs-tabs-container">
                            <ul class="overall-specs-tabs-wrapper">
                                <li data-tabs="specsTabContent" class="active">Specifications</li>
                                <li data-tabs="featuresTabContent">Features</li>
                                <li data-tabs="coloursTabContent">Colours</li>
                                <%if (vmCompare.UserReviewData != null)
                                    { %>
                                <li id="li-reviewtab" data-tabs="reviewTabContent" class="bw-ga" data-cat="Compare_Bikes" data-act="Floating_UserReview_Section" data-lab="<%=TargetedModels.Replace(",", "_")%>">Reviews</li>
                                <%} %>
                            </ul>
                            <div class="clear"></div>
                        </div>
                    </div>

                    <div id="overall-specs-tabs" class="overall-specs-tabs-container">
                        <ul class="overall-specs-tabs-wrapper">
                            <li data-tabs="specsTabContent" class="active">
                                <h3>Specifications</h3>
                            </li>
                            <li data-tabs="featuresTabContent">
                                <h3>Features</h3>
                            </li>
                            <li data-tabs="coloursTabContent">
                                <h3>Colours</h3>
                            </li>
                            <%if (vmCompare.UserReviewData != null)
                                { %>
                            <li data-tabs="reviewTabContent" class="bw-ga" data-cat="Compare_Bikes" data-act="Floating_UserReview_Section" data-lab="<%=TargetedModels.Replace(",", "_")%>">
                                <h3>Reviews</h3>
                                <%} %>
                            </li>
                        </ul>
                        <div class="clear"></div>
                    </div>
                    <div id="specsTabContent" class="bw-tabs-data active hide-features">
                        <% if (vmCompare.CompareSpecifications != null)
                            { %>
                        <% bool isFirstActiveTab = true; foreach (var spec in vmCompare.CompareSpecifications.Spec)
                            {  %>
                        <div class="model-accordion-tab <%= (isFirstActiveTab) ? "active" :""  %>">
                            <span class="offers-sprite specs-<%= spec.Value%>-sm-icon"></span>
                            <span class="accordion-tab-label"><%= spec.Text%></span>
                            <span class="bwmsprite fa-angle-down"></span>
                        </div>
                        <table class="table-content" width="100%" cellspacing="0" cellpadding="0" border="0">
                            <tbody>
                                <% isFirstActiveTab = false; foreach (var specCat in spec.SpecCategory)
                                    {  %>
                                <tr class="row-type-heading">
                                    <td colspan="2"><%= specCat.Text%></td>
                                    <td></td>
                                </tr>
                                <tr class="row-type-data">
                                    <% foreach (var compSpec in specCat.CompareSpec)
                                        { %>
                                    <td><%= compSpec.Value%></td>
                                    <% } %>
                                    <%if (!isSponsoredBike)
                                        { %>
                                    <td></td>
                                    <% } %>
                                </tr>
                                <% } %>
                            </tbody>
                        </table>

                        <% } %>

                        <% } %>
                    </div>
                    <div id="featuresTabContent" class="bw-tabs-data hide-features">
                        <% if (vmCompare.CompareFeatures != null)
                            { %>
                        <% foreach (var spec in vmCompare.CompareFeatures.Spec)
                            { %>
                        <table class="table-content" width="100%" cellspacing="0" cellpadding="0" border="0">
                            <tbody>
                                <% foreach (var specCat in spec.SpecCategory)
                                    {  %>
                                <tr class="row-type-heading">
                                    <td colspan="2"><%= specCat.Text %></td>
                                    <td></td>
                                </tr>
                                <tr class="row-type-data">
                                    <% foreach (var compSpec in specCat.CompareSpec)
                                        { %>
                                    <td><%= compSpec.Value %></td>
                                    <% } %>
                                    <%if (!isSponsoredBike)
                                        { %>
                                    <td></td>
                                    <% } %>
                                </tr>
                                <% } %>
                            </tbody>
                        </table>
                        <% } %>
                        <% } %>
                    </div>
                    <div id="coloursTabContent" class="bw-tabs-data hide-features">
                        <% if (vmCompare.CompareColors != null)
                            { %>
                        <table class="table-content" width="100%" cellspacing="0" cellpadding="0" border="0">
                            <tbody>
                                <tr class="row-type-heading">
                                    <td colspan="2"></td>
                                    <td></td>
                                </tr>
                                <tr class="row-type-data">
                                    <% foreach (var bike in vmCompare.CompareColors.bikes)
                                        { %>
                                    <td>
                                        <% foreach (var color in bike.bikeColors)
                                            {
                                                if (color.HexCodes != null)
                                                { %>
                                        <div class="color-box color-count-<%= color.HexCodes.Count()   %>">
                                            <% foreach (var hexCode in color.HexCodes)
                                                { %>
                                            <span style="background-color: #<%= hexCode %>"></span>
                                            <% } %>
                                        </div>
                                        <p><%= color.Color %></p>
                                        <% }
                                            } %>
                                    </td>
                                    <% } %>
                                </tr>


                            </tbody>
                        </table>
                        <% } %>
                    </div>
                    <% if (vmCompare.UserReviewData != null)
                        { %>
                    <div id="reviewTabContent" class="review-tab bw-tabs-data">
                        <table class="table-content" width="100%" cellspacing="0" cellpadding="0" border="0">
                            <tbody>
                                <tr class="row-type-heading">
                                    <td colspan="2">Overall rating</td>
                                    <td></td>
                                </tr>
                                <tr class="row-type-data">
                                    <% foreach (var compSpec in vmCompare.UserReviewData.OverallRating)
                                        { %>
                                    <td>
                                        <% if (!compSpec.ReviewRate.Equals("--"))
                                            {
                                        %>
                                        <div class="rate-count-<%=Math.Floor(Convert.ToDouble(compSpec.ReviewRate))%> rating">
                                            <span class="bwmsprite star-icon"></span>
                                            <span class="font18 text-bold"><%=compSpec.ReviewRate%></span>
                                        </div>
                                        <div class="rating-count">(<%=compSpec.RatingCount%>&nbsp;ratings)</div>
                                        <a href="/m<%=compSpec.ReviewListUrl%>" target="_blank" class="font12"><%=compSpec.ReviewCount%>&nbsp;reviews</a>
                                        <% }
                                            else
                                            { %>
                                        <span class="text-bold">--</span>
                                        <%} %>
                                    </td>
                                    <% } %>
                                    <td></td>
                                </tr>
                                <% var spec1 = vmCompare.UserReviewData.CompareReviews.Spec.FirstOrDefault().SpecCategory.FirstOrDefault(); %>
                                <tr class="row-type-heading">
                                    <td colspan="2"><%=spec1.Text%></td>
                                    <td></td>
                                </tr>
                                <tr class="row-type-data">
                                    <% foreach (var compSpec in spec1.CompareSpec)
                                        { %>
                                    <td><%=compSpec.Value%>&nbsp
                                        <%if (!compSpec.Value.Equals("--"))
                                            {%>
                                        <span class="font12 text-xt-light">kmpl</span>
                                        <%}%>
                                    </td>
                                    <%} %>
                                    <td></td>
                                </tr>

                                <%if (vmCompare.UserReviewData.CompareReviews != null && vmCompare.UserReviewData.CompareReviews.Spec != null && vmCompare.UserReviewData.CompareReviews.Spec.Count() > 1)
                                    {
                                        foreach (var spec in vmCompare.UserReviewData.CompareReviews.Spec.Skip(1))
                                        {
                                %>

                                <tr class="row-type-heading">
                                    <td colspan="2"><%=spec.Text%></td>
                                    <td></td>
                                </tr>

                                <%foreach (var specCat in spec.SpecCategory)
                                    {%>

                                <tr class="data-head">
                                    <td colspan="2"><%=specCat.Text%></td>
                                    <td></td>
                                </tr>
                                <tr class="row-type-data">
                                    <%foreach (var compSpec in specCat.CompareSpec)
                                        {%>
                                    <td class="border-bottom-grey margin-bottom5">
                                        <%if (!compSpec.Value.Equals("--"))
                                            { %>
                                        <div class='rating-bar margin-top5'>
                                            <span class='rate-<%=Math.Floor(Convert.ToDouble(compSpec.Value))%>'></span>
                                        </div>
                                        &nbsp
                                        <%=Math.Floor(Convert.ToDouble(compSpec.Value))%>
                                        <% }
                                            else
                                            { %>
                                        <%=compSpec.Value%>
                                        <% } %>
                                    </td>
                                    <%} %>
                                    <td></td>
                                </tr>

                                <%}
                                        }
                                    }%>

                                <% if (vmCompare.UserReviewData.MostHelpfulReviewList != null)
                                    { %>
                                <tr class="row-type-heading ">
                                    <td colspan="2">Reviews</td>
                                    <td></td>
                                </tr>
                                <tr class="row-type-data font14">
                                    <% foreach (var review in vmCompare.UserReviewData.MostHelpfulReviewList)
                                        { %>
                                    <td>
                                        <span class="rating-badge margin-bottom10" data-rate-bg="<%=review.RatingValue%>">
                                            <span class="bwmsprite star-white"></span><%=review.RatingValue%>
                                        </span>
                                        <p class="font14 text-default margin-bottom10">
                                            <a href="/m<%=review.ReviewDetailUrl%>" target="_blank" class="text-default"><%=review.ReviewTitle%></a>
                                        </p>
                                        <p class="review-decription">
                                            <%=review.ReviewDescription%>
                                        </p>
                                        <a href="/m<%=review.ReviewListUrl%>" class="review-title review-link bw-ga" data-cat="Compare_Bikes" data-act="Read_Reviews_Clicked" data-lab="<%=targetedModels.Replace(",", "_")%>">All reviews</a>
                                    </td>
                                    <%} %>
                                    <td></td>
                                </tr>
                                <%} %>
                            </tbody>
                        </table>
                    </div>
                    <%} %>

                    <div id="toggle-float-button" class="grid-12 float-button float-fixed clearfix slideIn-transition">
                        <button type="button" id="toggle-features-btn" class="btn btn-teal btn-full-width">Hide common features</button>
                    </div>
                    <div class="clear"></div>
                    <div id="comparison-footer"></div>
                </div>
                <section>
                    <div class="container bg-white box-shadow margin-bottom15 content-inner-block-20">
                        <p class="text-bold font16 line-height17 inline"><%= templateSummaryTitle %></p>
                        <span class="model-preview-main-content">
                            <p class="font14 text-light-grey line-height17 inline padding-top10"><%= Bikewale.Utility.StringExtention.StringHelper.Truncate(compareSummaryText,200) %>  ... </p>
                        </span>
                        <span class="model-preview-more-content hide">
                            <p class="font14 text-light-grey line-height17 inline padding-top10"><%= compareSummaryText %></p>
                        </span>
                        <a href="javascript:void(0)" class="font14 read-more-model-preview" rel="nofollow">Read more</a>
                    </div>
                </section>
                <% if (isUsedBikePresent)
                    { %>
                <div id="used-bikes-container" class="container box-shadow bg-white card-bottom-margin">
                    <h2 class="content-inner-block-15">Used bikes you may like</h2>
                    <table class="table-content" width="100%" cellspacing="0" cellpadding="0" border="0">
                        <tbody>
                            <tr class="row-type-heading">
                                <td colspan="2"></td>
                                <td></td>
                            </tr>
                            <tr class="row-type-data">
                                <% foreach (var bike in vmCompare.BasicInfo)
                                    { %>
                                <td>
                                    <% if (bike.UsedBikeCount.BikeCount > 0)
                                        {  %>
                                    <a href="/m<%= Bikewale.Utility.UrlFormatter.UsedBikesUrlNoCity(bike.MakeMaskingName,bike.ModelMaskingName,!string.IsNullOrEmpty(bike.UsedBikeCount.CityMaskingName)?bike.UsedBikeCount.CityMaskingName:"india" ) %>" class="block"><%= string.Format("{0} Used {1} {2}",bike.UsedBikeCount.BikeCount,bike.Make,bike.Model) %></a><p class="text-light-grey text-unbold">
                                        starting at<br />
                                        <span class="bwmsprite inr-grey-xxsm-icon"></span><%= Bikewale.Utility.Format.FormatPrice(bike.UsedBikeCount.StartingPrice.ToString()) %>
                                    </p>
                                    <% }
                                        else
                                        { %>
                                    <div class="font14 text-bold text-center">--</div>
                                    <%} %>
                                </td>
                                <% } %>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
            <% } %>

            <% if (objMakes != null)
                { %>
            <!-- select bike starts here -->
            <div id="select-bike-cover-popup" class="cover-window-popup">
                <div class="ui-corner-top">
                    <div id="close-bike-popup" class="cover-popup-back cur-pointer leftfloat" data-bind="click: closeBikePopup">
                        <span class="bwmsprite fa-angle-left"></span>
                    </div>
                    <div class="cover-popup-header leftfloat">Select bikes</div>
                    <div class="clear"></div>
                </div>
                <div class="bike-banner"></div>
                <div id="select-make-wrapper" class="cover-popup-body">
                    <div class="cover-popup-body-head">
                        <p class="no-back-btn-label head-label inline-block">Select Make</p>
                    </div>
                    <ul class="cover-popup-list with-arrow">
                        <% foreach (var make in objMakes)
                            { %>
                        <li data-bind="click: makeChanged" data-masking="<%= make.MaskingName %>" data-id="<%= make.MakeId %>"><span><%= make.MakeName %></span></li>
                        <% } %>
                    </ul>
                </div>

                <div id="select-model-wrapper" class="cover-popup-body">
                    <div class="cover-popup-body-head">
                        <div data-bind="click: modelBackBtn" class="body-popup-back cur-pointer inline-block">
                            <span class="bwmsprite back-long-arrow-left"></span>
                        </div>
                        <p class="head-label inline-block">Select Model</p>
                    </div>
                    <ul class="cover-popup-list with-arrow" data-bind="foreach: modelArray">
                        <li data-bind="click: $parent.modelChanged, attr: { 'data-id': modelId, 'data-masking': maskingName }">
                            <span data-bind="text: modelName, attr: { 'data-id': modelId, 'data-masking': maskingName }"></span>
                        </li>
                    </ul>
                </div>

                <div id="select-version-wrapper" class="cover-popup-body">
                    <div class="cover-popup-body-head">
                        <div data-bind="click: versionBackBtn" class="body-popup-back cur-pointer inline-block">
                            <span id="arrow-version-back" class="bwmsprite back-long-arrow-left"></span>
                        </div>
                        <p class="head-label inline-block">Select Version</p>
                    </div>
                    <ul class="cover-popup-list" data-bind="foreach: versionArray">
                        <li data-bind="click: $parent.versionChanged">
                            <span data-bind="text: versionName, attr: { 'data-id': versionId }"></span>
                        </li>
                    </ul>
                </div>

                <div class="cover-popup-loader-body">
                    <div class="cover-popup-loader"></div>
                    <div class="cover-popup-loader-text font14">Loading...</div>
                </div>
            </div>
            <!-- select bike ends here -->
            <% } %>
            <% } %>

            <div class="same-version-toast">
                <p>Please select different bike for comparision.</p>
            </div>

        </section>
        <% } %>

        <% if (ctrlSimilarBikes != null && ctrlSimilarBikes.fetchedCount > 0)
            { %>
        <section class="container bg-white box-shadow margin-bottom15">
            <h2 class="padding-15-20">Similar comparisons</h2>
            <BW:SimilarBikes ID="ctrlSimilarBikes" runat="server" />
        </section>
        <% } %>

          <% if (breadcrumb != null && breadcrumb.BreadcrumListItem != null && breadcrumb.BreadcrumListItem.Any())
            {%>
        <section>
            <div class="breadcrumb">
                <span class="breadcrumb-title">You are here:</span>
                <ul>
                    <%foreach (var item in breadcrumb.BreadcrumListItem)
                        {%>
                    <%if (!string.IsNullOrEmpty(item.Item.Url))
                        {%>
                    <li>
                        <a class="breadcrumb-link" href="<%= item.Item.Url %>" title="<%= item.Item.Name %>">
                            <span class="breadcrumb-link__label" itemprop="name"><%= item.Item.Name %></span>
                        </a>
                    </li>
                    <%}
                        else
                        {%>
                    <li>
                        <span class="breadcrumb-link__label"><%= item.Item.Name %></span>
                    </li>
                    <%}%>
                    <%}%>
                </ul>
                <div class="clear"></div>
            </div>
            <div class="clear"></div>
        </section>
        <% } %>

        <script type="text/javascript">
            var compareSource = <%=  (int)Bikewale.Entities.Compare.CompareSources.Mobile_CompareBike_Page %>;
        </script>
        <script type="text/javascript" src="<%= staticUrl  %>/UI/m/src/frameworks.js?<%= staticFileVersion %>"></script>
        <!-- #include file="/UI/includes/footerBW_Mobile.aspx" -->
        <link href="<%= staticUrl  %>/UI/m/css/bwm-common-btf.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
        <!-- #include file="/UI/includes/footerscript_mobile.aspx" -->
        <script type="text/javascript" src="<%= staticUrl %>/UI/m/src/compare/details.js?<%= staticFileVersion %>"></script>
        <%
            string fontFile = "/UI/css/fonts/OpenSans/open-sans-v15-latin-regular.woff",
            fontUrl = String.Format("{0}{1}?{2}", Bikewale.Utility.BWConfiguration.Instance.StaticUrl, fontFile, Bikewale.Utility.BWConfiguration.Instance.StaticCommonFileVersion);
        %>
        <style>
            @font-face { 
                font-family: 'Open Sans';
                font-style: normal;
                font-weight: 400;
                src: local('Open Sans Regular'), local('OpenSans-Regular'), url('<%= fontUrl%>') format('woff');
             }
        </style>
        
        
        <%  
            fontFile = "/UI/css/fonts/OpenSans/open-sans-v15-latin-600.woff";
            fontUrl = String.Format("{0}{1}?{2}", Bikewale.Utility.BWConfiguration.Instance.StaticUrl, fontFile, Bikewale.Utility.BWConfiguration.Instance.StaticCommonFileVersion); 
        %>
        <style>
             @font-face {
                font-family: 'Open Sans';
                font-style: normal;
                font-weight: 600;
                src: local('Open Sans SemiBold'), local('OpenSans-SemiBold'), url('<%= fontUrl%>') format('woff');
             } 
        </style>
        <%  
            fontFile = "/UI/css/fonts/OpenSans/open-sans-v15-latin-700.woff";
            fontUrl  = String.Format("{0}{1}?{2}", Bikewale.Utility.BWConfiguration.Instance.StaticUrl, fontFile, Bikewale.Utility.BWConfiguration.Instance.StaticCommonFileVersion); 
        %>
        <style>
             @font-face { 
	            font-family: 'Open Sans';
	            font-style: normal;
	            font-weight: 700;
	            src: local('Open Sans Bold'), local('OpenSans-Bold'), url('<% =fontUrl %>') format('woff');
             }
        </style>

    </form>


    

</body>
</html>



