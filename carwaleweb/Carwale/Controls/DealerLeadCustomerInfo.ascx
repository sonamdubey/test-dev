<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DealerLeadCustomerInfo.ascx.cs" Inherits="Carwale.UI.Controls.DealerLeadCustomerInfo" %>

<link rel="stylesheet" href="/static/css/nc-gallery-style.css" type="text/css" >
<div class="divCustForm step-1 leadForm">
    <div class="nc-pg-fields ng-div-name">
        <div class="nc-pg-fields-input-text">
            <input type="text" name="Name" placeholder="Name" id="custName" class="custName" />
            <span class="cw-nc-pg-sprite cw-nc-error-red-icon err-name-icon hide"></span>
            <div class="cw-blackbg-tooltip err-name-msg hide">Please enter your name</div>
        </div>
    </div>
    <div class="nc-pg-fields customerEmail ng-div-email">
        <div class="nc-pg-fields-input-text">
            <input name="Email" type="text" placeholder="Email" id="custEmail" class="custEmail">
            <span class="cw-nc-pg-sprite cw-nc-error-red-icon err-email-icon hide"></span>
            <div class="cw-blackbg-tooltip hide err-email-msg">Please enter your email</div>
        </div>
    </div>
    <div class="nc-pg-fields ng-div-mobile">
        <div class="nc-pg-fields-input-text">
            <input type="text" name="mobile" placeholder="Mobile Number (10 Digits)" id="custMobile" class="custMobile" />
            <span class="cw-nc-pg-sprite cw-nc-error-red-icon err-mobile-icon hide"></span>
            <div class="cw-blackbg-tooltip hide err-mobile-msg">Please enter your mobile number</div>
        </div>
    </div>
    <div id="cityDealerPopup">
        <!--ko if:pqCities().length > 0 -->
        <div class="nc-pg-fields divCity ng-div-city">
            <select id="drpCityDealerPopup" class="drpCityDealerPopup" data-role="none" data-bind='options: pqCities, optionsText: "name", optionsValue: "id", event: { change: dealerPopupCityChange }'></select>
            <span class="cw-nc-pg-sprite cw-nc-error-red-icon err-city-icon hide"></span>
            <div class="cw-blackbg-tooltip hide err-city-msg">Please select your city</div>
        </div>
        <!-- /ko -->
    </div>
    <div class="nc-pg-fields dealerDiv ng-div-dealer">
        <select id="dealerList" class="dealerList" data-role="none" data-bind='options: dealer, optionsCaption: "Select Your Nearest Dealership", optionsText: "name", optionsValue: "id", event: { change: drpDealerChange }'></select>
        <span class="cw-nc-pg-sprite cw-nc-error-red-icon err-dealer-icon hide"></span>
        <div class="cw-blackbg-tooltip hide err-dealer-msg">Please select a dealer</div>
    </div>
    <div class="book-test-drive text-left font14 text-green text-bold">
        <label><input type="checkbox" class="chkBoxBookTestDrive__js" name="" /> I'm also interested in taking a test drive</label>
    </div>
    <div class="book-test-drive-disclaimer testDriveDisclaimer__js text-left font12 margin-left15">
        <span>A CarWale executive will call you and assist you with scheduling your test drive</span>
    </div>
    <div class="margin-top15">
        <input type="button" id="btnSubmit" class="btnSubmit btn-orange btn-xlg text-white btn-full-width font16 cur-pointer" value="<%=btnText %>" data-role="none" />
    </div>

</div>

<div id="thank-you" class="thank-you-msg hide">
    <h2 class="nc-thank-you-text text-black">Thank you <span id="tycaption-cust-name"></span></h2>
</div>
<div class="hide margin-minus30" id="thank-you-message-recommend">
    <span class="background-adjust"></span>
    <div class="text-center bg-light-grey thanks-section-new padding-left10 padding-right10 padding-bottom40">
        <div class="text-green padding-top40 font22 text-bold">Thank You !</div>
        <p class="font14 padding-top10 ">A car consultant would get in touch with you shortly with assistance on your purchase.</p>
    </div>
    <div class="margin-bottom10 text-center suggestTitle position-rel">
        <h2 class="text-grey font20">Other Suggested Cars</h2>
        <span class="font13">(Based on your research)</span>
    </div>
</div>
<div class="thank-you-message-without-email hide">
    <div class="margin-top15 font14 text-grey">
        <p class="text-grey">
            A car consultant would get back to you shortly with assistance on your car purchase.
        </p>
    </div>
    </div>
<div class="thank-you-message-with-email hide" id="thank-you-message-with-email" style="display:none">
    <div class="margin-top15 font14 text-grey">
        <p class="text-grey">
            You can provide your email id below to receive the price list, brochure and other collaterals over email.
        </p>
    </div>
    <div class="nc-pg-fields customerEmail margin-top20">
        <div class="nc-pg-fields-input-text">
            <input name="Email" type="text" placeholder="Email" id="custEmailOptional" class="custEmailOptional">
            <span class="cw-nc-pg-sprite cw-nc-error-red-icon err-email-icon hide"></span>
            <div class="cw-blackbg-tooltip hide err-email-msg">Please enter a valid email</div>
        </div>
    </div>
</div>

<div id="reqAssist" class="suggestion-box hide">
     <!--ko 'if':CampaignRecommendation().length > 1 -->
     <div id="selectRecommendation" class="inline-block margin-top15">
            <p class="text-link font14 select-all toggle-selection-view">
                <span class="inline-block valign-middle text-bold toggle-selection-txt">Select All</span>
                <span class="select-bar inline-block valign-middle">
                    <span class="circle-icon"><img src="https://imgd.aeplcdn.com/0x0/cw/design15/toggle-slider-circle.png"></span>
                </span>
            </p>
        </div>
    <!-- /ko -->
     <ul id="recommedationList" class="listTable padding-top20" data-bind="foreach: CampaignRecommendation">
        <li>
            <div class="listCell"><span class="unchecked" data-bind="attr: { 'ModelId': $data.carData.carModel.modelId, 'ModelName': $data.carData.carModel.modelName, 'CampaignId': $data.campaign.id, 'MakeName': $data.carData.carMake.makeName, 'City': $data.carPricesOverview.city.name }"></span></div>
            <div class="listCell">
                <img data-bind="attr: { 'src': $data.carData.carImageBase.hostUrl + '110X61' + $data.carData.carImageBase.originalImgPath }" width="120" />
            </div>
            <div class="listCell">
                <div class="font14 text-bold" data-bind="text: $data.carData.carMake.makeName + ' ' + $data.carData.carModel.modelName"></div>
                <div class="font14">₹ <strong data-bind="text: $data.carPricesOverview.price"></strong></div>
                <div class="font13 text-light-grey" data-bind="text: $data.carPricesOverview.city.name != null ? '(On-Road Price ' + $data.carPricesOverview.city.name + ')' : '(' + $data.carPricesOverview.priceLabel + ')'"></div>
                <div class="font13" data-bind="text: $data.campaign.contactName"></div>
            </div>
        </li>
        
    </ul>

    <div class="clear margin-top10 req-assist-button">
        <p id="selectOption" class="font12 text-red text-bold hide">(Please select atleast one option)</p>
        <div class="clearfix">
            <input type="button" id="reqAsstncBtn" value="Request Assistance" class="btn btn-disable leftfloat btn-full-width text-uppercase padLR16" />
            <%--<input type="button" id="noThanks" class="btn btn-white rightfloat text-uppercase padLR17" value="No Thanks" />--%>
        </div>
    </div>
</div>
<!-- /suggestion-box -->


<div id="thanksText" class="thank-box hide">
    <h2 class="margin-bottom10 popup_title">Thank You</h2>
    <p class="font14">
        You would soon get a call from the recommended dealers with assistance on your car purchase. 
    </p>
 
    <div class="advt-recomentdation margin-top20" id="recommendedDealsDiv"  >
        <h2 class="font18">Similiar cars with Great Savings</h2>
        <ul class="advt-rec-list padding-top20" data-bind="foreach: RecommendedDeals" >
        <li class="margin-bottom20">
            <a data-role="click-tracking" data-event="CWInteractive" data-action="dealsaccess_desktop" data-cat="deals_desktop" data-label="PQrecommendDesk" data-cwtccat="QuotationPage" data-bind="attr: { 'data-cwtcact': $data.cwtcact,'data-cwtclbl': 'make:' + $data.carMake.makeName + '|model:' + $data.carModel.modelName + '|city:' + $data.cityName, 'href': '/' + Common.utils.formatSpecial($data.carMake.makeName) + '-cars/' + $data.carModel.maskingName + '/advantage/?cityid=' + $data.cityId }" target="_blank">
            <div class="advt-rec-car-image inline-block">
                <img  data-bind="attr: { 'src': $data.carImgDetails.hostUrl + '110X61' + $data.carImgDetails.originalImgPath }" alt="" title="" />
            </div>
            <div class="advt-rec-car-details inline-block margin-left15">
                <div class="font16 text-bold margin-bottom5" data-bind="text: $data.carMake.makeName + ' ' + Common.utils.filterModelName($data.carModel.modelName)"></div>
                <div class="font12 margin-top5 margin-bottom5 text-grey">
                    <span class="cancel-price position-rel">₹ <span data-bind="text: Common.utils.formatNumeric($data.onRoadPrice)"></span></span>
                    <span class="off-price text-green margin-left5">(₹ <span data-bind="text: Common.utils.formatNumeric($data.savings)"> </span> Off)</span>
                </div>
                <div class="font13 text-light-grey">
                    <span class="car-cost-price font16 text-grey text-bold">₹ <span data-bind="text: Common.utils.formatNumeric($data.offerPrice)"></span></span>
                    <%--<span class="text-red car-left-count font12">Only <span data-bind="text: $data.stockCount"></span> car left</span>--%>
                     <!-- ko 'if':$data.stockCount == 1 -->
                    <span class="text-red car-left-count font12">Only <span data-bind="text: $data.stockCount"></span> car left</span>
                     <!-- /ko -->
                     <!-- ko 'if':$data.stockCount > 1 && $data.stockCount < 6 -->
                     <span class="text-red car-left-count font12">Only <span data-bind="text: $data.stockCount"></span> cars left</span>
                     <!-- /ko -->
                     <!-- ko 'if':$data.stockCount > 5 -->
                     <span class="text-red car-left-count font12">Only <span>few</span> cars left</span>
                     <!-- /ko -->
                </div>
                <div class="font13 margin-top5 text-grey">Final On-Road Price in <span data-bind="text: $data.cityName"></span></div>
            </div>
           </a>
        </li>
    </ul>
  </div>
</div>
<!-- thanks text box ends here-->
<!-- thanks you box and DealsRecommendation ends here -->
<div class="margin-top30 done hide">
    <input type="button" id="divDone" class="divDone btn-orange btn-xlg text-white text-uppercase font18 cur-pointer" value="Done" />
</div>
<input type="hidden" id="hdnDealerId">
<input type="hidden" id="hdnDealerName">
<script>

    function validCheckbox() {
        if ($(this).closest("#reqAssist").find("li .checked").length) {
            return true;
        }
        else false;
    }

</script>
<style>
.advt-rec-car-image{max-width:125px;max-height:92px;}
.advt-rec-car-image img{width:100%;}
.advt-rec-car-details{width:58%;}
.cancel-price:before {border-top:1px solid #565a5c; content:""; position:absolute; top:8px; left:0; width:100%;}
.car-left-count{margin: 2px 0 0 8px;display: inline-block;vertical-align: top;}
</style>
