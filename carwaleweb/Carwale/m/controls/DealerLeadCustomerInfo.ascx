<%@ Control Language="C#" AutoEventWireup="true" Inherits="MobileWeb.Controls.DealerLeadCustomerInfo" %>
<style>
    .suggestion-box { /*max-width: 320px;*/ margin: 0 auto; }

    .center-div { display: table; margin: 0 auto; }
</style>
<div class="customer-info margin-left10 margin-right10">
    <div class="margin-bottom20 position-rel custName">
        <input class="form-control" type="text" id="custName" name="" data-role="none" placeholder="Name" />
        <span class="cwmsprite error-icon hide"></span>
        <div class="cw-blackbg-tooltip hide"></div>
    </div>
    <div class="margin-bottom20 position-rel custMobile">
        <input type="tel" id="custMobile" name="" data-role="none" placeholder="Mobile +91" class="form-control" />
        <span class="cwmsprite error-icon hide"></span>
        <div class="cw-blackbg-tooltip hide"></div>
    </div>
    <div class="margin-bottom20 position-rel custEmail hide">
        <input class="form-control" type="email" id="custEmail" name="" data-role="none" placeholder="Email" />
        <span class="cwmsprite error-icon hide"></span>
        <div class="cw-blackbg-tooltip hide"></div>
    </div>
    <div id="cityDealerPopup">
        <!--ko if:pqCities().length > 0 -->
        <div class="margin-bottom20 position-rel cityDiv">
            <select class="form-control" id="drpCityDealerPopup" data-role="none" data-bind='options: pqCities, optionsText: "name", optionsValue: "id", event: { change: dealerPopupCityChange }'>
            </select>
            <span class="cwmsprite error-icon hide"></span>
            <div class="cw-blackbg-tooltip hide"></div>
        </div>
        <!-- /ko -->
    </div>
    <div class="margin-bottom20 position-rel dealerDiv">
        <select id="dealerList" class="form-control" data-role="none" data-bind='options: dealer, optionsCaption: "Select Your Nearest Dealership", optionsText: "name", optionsValue: "id", event: { change: drpDealerChange }'>
        </select>
        <span class="cwmsprite error-icon hide"></span>
        <div class="cw-blackbg-tooltip hide"></div>
    </div>
    <div class="margin-bottom20 margin-top20">
        <input type="button" id="btnSubmit" value="Submit" data-role="none" class="text-uppercase btn btn-orange width100" />
    </div>
    <input type="hidden" id="hdnDealerId">
    <input type="hidden" id="hdnDealerName">
    <div class="error-red agreement"></div>

    <div class="text-center margin-top10 font11">
        <span>By proceeding ahead you agree to CarWale <a href="/visitoragreement.aspx">visitor agreement</a> , <a href="/privacypolicy.aspx">privacy policy</a>  and <a href="/offerTermsAndConditions.aspx" target="_blank">offer terms and conditions</a>.</span>
    </div>
</div>
<div class="thank-you-msg text-left hide">
    <div id="postsubmitmsg" class="margin-bottom10">
        <h2>Thank you <span id="inquirerName"></span></h2>
        <p class="margin-top10">A car consultant from <span id="tycaption"></span>&nbsp;would call you soon with more information.<span class="hide msg-with-email"> Meanwhile you can provide your email id to receive the price-list, broucher and other collaterals.</span></p>
    </div>
    <div class="margin-bottom20 position-rel custEmailOptional hide">
        <input class="form-control" type="email" id="custEmailOptional" name="" data-role="none" placeholder="Email" />
        <span class="cwmsprite error-icon hide"></span>
        <div class="cw-blackbg-tooltip hide"></div>
    </div>
</div>



<%--    </div>--%>

<div id="thanksText" class="thank-box hide">
    <h2 class="margin-bottom20 popup_title">Thank You</h2>
    <p class="font16">
        You would soon get a call from the recommended dealers with assistance on your car purchase.
    </p>

    <div class="advt-recomentdation margin-top20" id="dealsRecommendationDiv">
        <h2 class="font18">Similiar cars with Great Savings</h2>
        <ul class="advt-rec-list padding-top20 text-center" data-bind="foreach: RecommendedDeals">
            <li class="margin-bottom20">
                <a data-cwtccat="QuotationPage" data-bind="attr: { 'data-cwtcact': $data.cwtcact, 'data-cwtclbl': 'make:' + $data.carMake.makeName + '|model:' + $data.carModel.modelName + '|city:' + $data.cityName, 'href': '/m/' + formatSpecial($data.carMake.makeName) + '-cars/' + $data.carModel.maskingName + '/advantage/?cityid=' + $data.cityId }" target="_blank">
                    <div class="advt-rec-car-image inline-block">
                        <img data-bind="attr: { 'src': $data.carImgDetails.hostUrl + '110X61' + $data.carImgDetails.originalImgPath }" alt="" title="" />
                    </div>
                    <div class="advt-rec-car-details inline-block margin-left15">
                        <div class="font14 text-bold margin-bottom5" data-bind="text: $data.carMake.makeName + ' ' + Common.utils.filterModelName($data.carModel.modelName)"></div>
                        <div class="font12 margin-top5 margin-bottom5">
                            <span class="cancel-price text-grey position-rel">₹<span data-bind="text: formatNumeric($data.onRoadPrice)"></span></span>
                            <span class="off-price text-green margin-left5">(₹ <span data-bind="text: formatNumeric($data.savings)"></span>off)</span>
                        </div>
                        <div class="font13 text-light-grey">
                            <span class="car-cost-price font16 text-grey text-bold">₹<span data-bind="text: formatNumeric($data.offerPrice)"></span></span>
                            <!-- ko 'if':$data.stockCount == 1 -->
                            <span class="text-red car-left-count font12">Only <span data-bind="text: $data.stockCount"></span>car left</span>
                            <!-- /ko -->
                            <!-- ko 'if':$data.stockCount > 1 && $data.stockCount < 6 -->
                            <span class="text-red car-left-count font12">Only <span data-bind="text: $data.stockCount"></span>cars left</span>
                            <!-- /ko -->
                            <!-- ko 'if':$data.stockCount > 5 -->
                            <span class="text-red car-left-count font12">Only <span>few</span> cars left</span>
                            <!-- /ko -->
                        </div>
                        <div class="font12 text-grey margin-top5">Final On-Road Price in <span data-bind="text: $data.cityName"></span></div>
                    </div>
                </a>
            </li>
        </ul>
    </div>
</div>
<!-- /thanks text box -->


<div class="done margin-bottom10 margin-top20 hide">
    <input type="button" value="Done" data-role="none" class="text-uppercase btn btn-orange width100" id="btnDone" />
</div>



<div id="reqAssist" class="suggestion-box lead-recommend-cars text-left hide">

    <div class="text-center bg-light-grey thanks-section padding-left20 padding-right20">
        <div class="text-green padding-top20 font22 text-bold">Thank You !</div>
        <p class="font14 padding-top10 ">A car consultant would get in touch with you shortly with assistance on your purchase.</p>
    </div>

    <div class="recommendation-section position-rel">
        <div class="margin-bottom10 text-center suggestTitle position-rel">
            <h2>Other Suggested Cars</h2>
            <span class="font13">(Based on your research)</span>
        </div>
        <!--ko 'if':CampaignRecommendation().length > 1 -->
        <div id="divSelectRecommendation" class="margin-top15 margin-bottom10 padding-top5 padding-bottom5 padding-left15 padding-right10">
            <p class="text-link font14 select-all toggle-selection-view">
                <span class="inline-block margin-right10 valign-middle text-bold toggle-selection-txt">Select All</span>
                <span class="select-bar inline-block valign-middle">
                    <span class="circle-icon">
                        <img src="<%= Carwale.Utility.CWConfiguration._imgHostUrl %>0x0/cw/design15/toggle-slider-circle.png"></span>
                </span>
            </p>
        </div>
        <!-- /ko -->
        <ul id="recommedationList" class="listTable" data-bind="foreach: CampaignRecommendation">
            <li class="rounded-corner2">
                <a href="javascript:void(0)">
                    <div class="listCell"><span class="unchecked" data-bind="attr: { 'ModelId': $data.carModel.modelId, 'ModelName': $data.carModel.modelName, 'CampaignId': $data.campaignId, 'MakeName': $data.carMake.makeName, 'CityName': $data.custLocation.cityName }"></span></div>
                    <div class="listCell">
                        <img data-bind="attr: { 'src': $data.carImageBase.hostUrl + '110X61' + $data.carImageBase.originalImgPath }" width="100" />
                    </div>
                    <div class="listCell">
                        <div class="font14 text-bold" data-bind="text: $data.carMake.makeName + ' ' + $data.carModel.modelName"></div>
                        <div class="font14">₹ <strong data-bind="text: formatNumeric($data.carPrices.baseVersionOnRoadPrice)"></strong></div>
                        <div class="font13 text-light-grey" data-bind="text: '(On-Road ' + $data.custLocation.cityName + ')'"></div>
                        <div class="font13" data-bind="text: $data.dealerSummary.name"></div>
                    </div>
                </a>
            </li>

        </ul>
        <div class="clear margin-bottom20 margin-left10 margin-right10">
            <p id="selectOption" class="font12 text-red text-bold hide">(Please select atleast one option)</p>
            <div class="block margin-auto">
                <input type="button" id="reqAsstncBtn" onclick="reqAsstncFun(this)" value="Request Assistance" class="btn btn-orange btn-xs btn-full-width" />
                <%--<input type="button" class="btn btn-white btn-xs rightfloat margin-left10" id="noThanks" onclick="closeDealerPopup(this)" value="No Thanks" />--%>
            </div>
        </div>
    </div>
</div>
<!-- /suggestion-box -->

<script type="text/javascript">

    function closeDealerPopup(e) {
        //$("div.black-window").hide();
        $("div.dealerPopup").hide();
        $("#popup-title").show();
        $(".thank-you-msg,.done").hide();
        $("#thanksText").hide();
        $("#reqAssist").hide();
        $('#selectOption').hide();
        if ($(e).val() == "No Thanks") {
            dataLayer.push({ event: 'DealerLeadPopUpBehaviour', cat: "Recommended Cars with Tie Up - MSite", act: 'No Thanks Button Clicked' });
        }
    }
</script>

<style>
    .advt-rec-car-image { max-width: 100px; max-height: 92px; vertical-align: middle; }

        .advt-rec-car-image img { width: 100%; }

    .advt-rec-car-details { width: 62%; text-align: left; vertical-align: middle; }

    .cancel-price:before { border-top: 1px solid #565a5c; content: ""; position: absolute; top: 8px; left: 0; width: 100%; }

    .car-left-count { margin: 2px 0 0 8px; display: inline-block; vertical-align: top; }

    @media only screen and (max-width: 320px) {
        .advt-rec-car-details { width: 59%; text-align: left; }
    }
</style>
