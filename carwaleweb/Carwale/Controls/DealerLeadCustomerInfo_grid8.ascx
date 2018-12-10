<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DealerLeadCustomerInfo_grid8.ascx.cs" Inherits="Carwale.UI.Controls.DealerLeadCustomerInfo_grid8" %>

<link rel="stylesheet" href="/static/css/nc-gallery-style.css" type="text/css" >
<div class="leadForm">
    <div class="margin-bottom15 grid-6">
        <div class="nc-pg-fields-input-text position-rel">
            <input type="text" name="Name" placeholder="Name" id="custName" class="custName form-control" />
            <span class="cw-nc-pg-sprite cw-nc-error-red-icon err-name-icon hide" style="display: none;"></span>
            <div class="cw-blackbg-tooltip err-name-msg hide">Please enter your name</div>
        </div>
    </div>
    <div class="margin-bottom15 grid-6 customerEmail" id="custEmailVisible">
        <div class="nc-pg-fields-input-text position-rel">
            <input name="Email" type="text" placeholder="Email" id="custEmail" class="custEmail form-control">
            <span class="cw-nc-pg-sprite cw-nc-error-red-icon err-email-icon hide" style="display: none;"></span>
            <div class="cw-blackbg-tooltip hide err-email-msg">Please enter your email</div>
        </div>
    </div>
    <div class="margin-bottom15 grid-6">
        <div class="nc-pg-fields-input-text position-rel">
            <input type="text" name="mobile" placeholder="Mobile" id="custMobile" class="custMobile form-control" />
            <span class="cw-nc-pg-sprite cw-nc-error-red-icon err-mobile-icon hide" style="display: none;"></span>
            <div class="cw-blackbg-tooltip hide err-mobile-msg">Please enter your mobile number</div>
        </div>
    </div>
    <div class="margin-bottom15 divCity grid-6" id="clearBindAfterCity">
        <div class="position-rel">
            <select id="drpCityDealerPopup" class="drpCityDealerPopup form-control" data-role="none" data-bind='options: pqCities, optionsText: "CityName", optionsValue: "CityId", event: { change: dealerPopupCityChange }'></select>
            <span class="cw-nc-pg-sprite cw-nc-error-red-icon err-city-icon hide" style="display: none;"></span>
            <div class="cw-blackbg-tooltip hide err-city-msg">Please select your city</div>
        </div>
    </div>
    <div class="margin-bottom15 dealerDiv grid-6">
        <div class="position-rel">
            <select id="dealerList" class="dealerList form-control" data-role="none" data-bind='options: dealer, optionsCaption: "Select Your Nearest Dealership", optionsText: "name", optionsValue: "id", event: { change: drpDealerChange }'></select>
            <span class="cw-nc-pg-sprite cw-nc-error-red-icon err-dealer-icon hide" style="display: none;"></span>
            <div class="cw-blackbg-tooltip hide err-dealer-msg">Please select a dealer</div>
        </div>
    </div>
    <div class="clear"></div>
    <div class="margin-left10">
        <input type="button" id="btnSubmit" class="btnSubmit btn btn-orange" value="<%=btnText %>" data-role="none" />
    </div>

</div>
<div id="thank-you" class="thank-you-msg thank-you hide">
    <h2 class="text-black">Thank you <span id="tycaption-cust-name"></span></h2>
</div>
<div class="thank-you-message-without-email thank-you hide">
    <div class="margin-top10 font14 text-grey">
        <p class="text-grey">
            A car consultant would get back to you shortly with assistance on your car purchase.
        </p>
    </div>
</div>
<div class="thank-you-message-with-email thank-you hide">
    <div class="margin-top15 font14 text-grey">
        <p class="text-grey">
            You can provide your email id below to receive the price list, brochure and other collaterals over email.
        </p>
    </div>
    <div class="nc-pg-fields customerEmail margin-top20">
        <div class="nc-pg-fields-input-text grid-5 alpha omega ">
            <input name="Email" type="text" placeholder="Email" id="custEmailOptional" class="custEmailOptional">
            <span class="cw-nc-pg-sprite cw-nc-error-red-icon err-email-icon hide"></span>
            <div class="cw-blackbg-tooltip hide err-email-msg">Please enter a valid email</div>
        </div>
    </div>
</div>
<div class="clear"></div>
<div class="margin-top15 done hide">
    <input type="button" id="divDone" class="divDone btn-orange btn-xlg text-white text-uppercase font18 cur-pointer" value="Done" />
</div>
<input type="hidden" id="hdnDealerId">
<input type="hidden" id="hdnDealerName">
