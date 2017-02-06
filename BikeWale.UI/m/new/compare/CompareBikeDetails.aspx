﻿<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.New.CompareBikeDetails" Trace="false" %>
<%@ Register Src="~/m/controls/SimilarCompareBikes.ascx" TagPrefix="BW" TagName="SimilarBikes" %>
<!DOCTYPE html>
<html>
<head>
    <%
        if(pageMetas!=null)
        {
            title = pageMetas.Title;
            keywords = pageMetas.Keywords;
            description = pageMetas.Description;
            canonical = pageMetas.CanonicalUrl;
            AdPath = "/1017752/Bikewale_Mobile_NewBikes";
            AdId = "1398766302464";
            TargetedModels = targetedModels;  
        }
    %>
    <!-- #include file="/includes/headscript_mobile_min.aspx" -->
    <link rel="stylesheet" type="text/css" href="/m/css/compare/details.css" />
    <script type="text/javascript">
        <!-- #include file="\includes\gacode_mobile.aspx" -->
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <!-- #include file="/includes/headBW_Mobile.aspx" -->

        <% if(vmCompare!=null) { %>
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
                        <div class="bike-details-block <%= (bike.VersionId != sponsoredVersionId ) ? "" : "sponsored-bike-details-block" %>" data-masking="<%= string.Format("{0}-{1}",bike.MakeMaskingName,bike.ModelMaskingName)%>" data-changed="" data-versionid="<%= bike.VersionId %>">
                            <% if (bike.VersionId == sponsoredVersionId)
                               {%>
                            <span class="position-abt pos-top5 label-text">Sponsored</span>
                            <% } %>
                            <span <%= (bike.VersionId != sponsoredVersionId ) ? "" : "id='close-sponsored-bike'" %> class="close-selected-bike position-abt pos-right5 bwmsprite cross-sm-dark-grey"></span>
                            <a href="<%= string.Format("/m/{0}-bikes/{1}/",bike.MakeMaskingName,bike.ModelMaskingName) %>" title="<%= bikeName %>" class="block margin-top10">
                                <h2 class="font14"><%= bikeName  %></h2>
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
                            <% if (bike.VersionId != sponsoredVersionId)
                               { %>
                            <div>
                                <a href="javascript:void(0)" class="btn btn-white bike-orp-btn getquotation" data-modelid="<%= bike.ModelId %>" rel="nofollow">On-road price</a>
                            </div>
                            <% }
                               else
                               { %>

                            <div class="padding-top5 padding-bottom5">
                                <a href="" class="font14">Know more <span class="bwmsprite know-more-icon"></span></a>
                            </div>
                            <% } %>
                        </div>
                        <% } %>
                        <div class="clear"></div>
                    </div>
                    <div id="comparison-floating-card" class="box-shadow slideIn-transition">
                        <% foreach (var bike in vmCompare.BasicInfo)
                           {   %>
                        <div class="bike-details-block">
                            <% if (bike.VersionId == sponsoredVersionId)
                               {%>
                            <span class="position-abt pos-top5 label-text">Sponsored</span>
                            <% } %>
                            <a href="<%= string.Format("/m/{0}-bikes/{1}/",bike.MakeMaskingName,bike.ModelMaskingName) %>" class="bike-title-target"><%= string.Format("{0} {1}",bike.Make,bike.Model) %></a>
                            <% if (bike.VersionId != sponsoredVersionId)
                               { %>
                            <div>
                                <a href="javascript:void(0)" class="btn btn-white bike-orp-btn getquotation" data-modelid="<%= bike.ModelId %>" rel="nofollow">On-road price</a>
                            </div>
                            <% }
                               else
                               { %>
                            <div class="padding-top5 padding-bottom5">
                                <a href="" class="font14">Know more <span class="bwmsprite know-more-icon"></span></a>
                            </div>
                            <% } %>
                        </div>
                        <% } %>
                        <div class="clear"></div>
                        <div class="overall-specs-tabs-container">
                            <ul class="overall-specs-tabs-wrapper">
                                <li data-tabs="specsTabContent" class="active">Specifications</li>
                                <li data-tabs="featuresTabContent">Features</li>
                                <li data-tabs="coloursTabContent">Colours</li>
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
                        </ul>
                        <div class="clear"></div>
                    </div>
                    <div id="specsTabContent" class="bw-tabs-data active">
                        <% if (vmCompare.CompareSpecifications != null)
                           { %>
                        <% foreach (var spec in vmCompare.CompareSpecifications.Spec)
                           { %>
                        <div class="model-accordion-tab active">
                            <span class="offers-sprite engine-sm-icon"></span>
                            <span class="accordion-tab-label"><%= spec.Text %></span>
                            <span class="bwmsprite fa-angle-down"></span>
                        </div>
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
                    <div id="featuresTabContent" class="bw-tabs-data">
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
                    <div id="coloursTabContent" class="bw-tabs-data">
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
                                           { %>
                                        <div class="color-box color-count-<%= color.HexCodes.Count %>">
                                            <% foreach (var hexCode in color.HexCodes)
                                               { %>
                                            <span style="background-color: #<%= hexCode %>"></span>
                                            <% } %>
                                        </div>
                                        <p><%= color.Color %></p>
                                        <% } %>
                                    </td>
                                    <% } %>
                                </tr>


                            </tbody>
                        </table>
                        <% } %>
                    </div>

                    <div id="toggle-float-button" class="grid-12 float-button float-fixed clearfix slideIn-transition">
                        <button type="button" id="toggle-features-btn" class="btn btn-teal btn-full-width">Hide common features</button>
                    </div>
                    <div class="clear"></div>
                    <div id="comparison-footer"></div>
                </div>

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
                                <td><a href="<%= Bikewale.Utility.UrlFormatter.UsedBikesUrlNoCity(bike.MakeMaskingName,bike.ModelMaskingName,bike.UsedBikeCount.CityMaskingName) %>" class="block"><%= string.Format("{0} Used {1} {2}",bike.UsedBikeCount.BikeCount,bike.Make,bike.Model) %></a><p class="text-light-grey text-unbold">starting at<br />
                                    <span class="bwmsprite inr-grey-xxsm-icon"></span><%= Bikewale.Utility.Format.FormatPrice(bike.UsedBikeCount.StartingPrice.ToString()) %></p>
                                </td>
                                <% } %>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
            <% } %>

            <% if(objMakes!=null) { %>
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
        </section>
        <% } %>

        <% if (ctrlSimilarBikes!=null && ctrlSimilarBikes.fetchedCount > 0)
           { %>
        <div>
            <section class="container related-comparison-container margin-bottom20 ">
                <h2 class="font14 padding-left10 margin-top5 margin-bottom15">Related comparisons</h2>
                <div class="swiper-container">
                    <div class="swiper-wrapper">
                        <BW:SimilarBikes ID="ctrlSimilarBikes" runat="server" />
                    </div>
                </div>
            </section>
        </div>
        <% } %>

        <script type="text/javascript" src="<%= staticUrl != "" ? "https://st1.aeplcdn.com" + staticUrl : "" %>/m/src/frameworks.js?<%= staticFileVersion %>"></script>
        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <link href="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : "" %>/m/css/bwm-common-btf.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
        <!-- #include file="/includes/footerscript_mobile.aspx" -->
        <script type="text/javascript" src="<%= staticUrl != "" ? "https://st1.aeplcdn.com" + staticUrl : ""%>/m/src/compare/details.js?<%= staticFileVersion %>"></script>
        <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,600,700' rel='stylesheet' type='text/css' />

    </form>
</body>
</html>



