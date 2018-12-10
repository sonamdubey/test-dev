<script language="c#" runat="server"></script>>

<div id="blackOut-recommendation" class="hide"></div>
<!-- recommendation popup starts here -->
<div id="recommendCars" class="searchRecommendation hide">
    <!-- left side seller details starts here -->
    <div id="contactSeller" class="grid-4 alpha omega recommedCars-left position-rel noRecommendation">
        <span id="popup-close-icon" class="position-abt pos-top10 pos-right10 verification-close-btn popup-close-esc-key">
            <span class="cwsprite cross-lg-dark-grey "></span>
        </span>
        <div id="sellerDetailsScreen" class="seller-detail-screen3 hide">
                <div class="loading-icon-recommendation hide" id="loadingIconSellerDetails"><img src="https://img.aeplcdn.com/adgallery/loader.gif"></div>
                <p class="content-inner-block-10 font18 text-bold padding-bottom20">Seller details</p>
                <div class="detailList font14">
                    <p class="buyerprocess-sprite seller-name-ic leftfloat"></p>
                    <div class="leftfloat inner-details">
                        <h4 id="contactPersonId" class="seller-details-right-name-field contactPersonClass"></h4>
                        <div class="dealer-rating-container position-rel inline-block">
                            <span id="topRatedSellerTag" class="top-rated-seller-tag">Top Rated Seller</span>
                            <span class="dealer-rating-tooltip dealer-rating-tooltip--bottom">This seller has consistently been rated well by his customers</span>
                        </div>
                        <p id="sellerNameId" class="margin-top5"></p>

                        <a href="#" class="font14 seller-virtual-link" target="_blank">Check other cars from this seller</a>

                    </div>
                    <div class="clear"></div>
                    <ul>
                        <li>
                            <span class="buyerprocess-sprite seller-call-ic"></span>
                            <div class="inner-details" id="sellerMobileId">
                                <p class="text-black"></p>
                            </div>
                            <div class="clear"></div>
                        </li>
                        <li>
                            <span class="buyerprocess-sprite seller-email-ic"></span>
                            <div class="inner-details" id="sellerEmailId">
                                <p class="text-black"></p>
                            </div>
                            <div class="clear"></div>
                        </li>
                        <li>
                            <span class="buyerprocess-sprite seller-location-ic"></span>
                            <div class="inner-details" id="sellerAddressId">
                                <p class="text-black"></p>
                            </div>
                            <div class="clear"></div>
                        </li>
                    </ul>
                    <div class="font11 text-light-grey margin-top20 margin-bottom10">Your contact details have been shared with the seller.</div>
                    <a class="font14 text-link bp-SimilarCars" target="_blank" onClick="dataLayer.push({ event: 'ViewSimilarCarsClick', cat: 'UsedCarSearch', act: 'ViewSimilarCarsClick_SearchPage'});">View similar cars >></a>

                </div>
                
                <div class="position-rel text-bold font18 suggestCarsTxt  hide">
                    <p class="animateArrowIcon">People also liked <span class="fa fa-angle-double-right"></span></p>
                </div>
            </div>
         <!-- Buyer Process captcha starts here -->
        <div id="captcha" class="captcha content-inner-block-10 hide">
            <h3 class="margin-bottom20 text-black">Security check</h3>
            <iframe id="captchaCode" src="" frameborder="0" scrolling="no" width="200" height="55"></iframe>
            <div>
	            <p class="margin-bottom10">Enter the code shown above</p>
                <div class="position-rel captcha-inputbox">
                    <input type="text" id="txtCaptchaCode" class="form-control"/>
                    <span id="lblCaptcha" class="hide cw-blackbg-tooltip"></span>
                    <span class="hide cwsprite error-icon"></span>
                </div>
                <p>(If you can't read it: <a class="text-link" onclick="javascript:regenerateCode(this,1)">Regenerate Code</a>)</p>
	            <input id="btnVerifyCaptcha" type="button" class="btnVerifyCaptcha btn btn-orange margin-top10" value="Verify" />
            </div>
        </div>
    <!-- Buyer Process captcha ends here -->
    <div id="not_auth" class="alert hide content-inner-block-20 margin-top15">Oops! You have reached the maximum limit for viewing inquiry details in a day.</div>
        </div>
       
    <!-- right side recommendation list code starts here -->
    <div id="recommendedCarsSection" class="grid-8 alpha omega animateRecommendation bg-white hide">
        <span class="popup-close position-abt pos-top10 pos-right10 popup-close-esc-key">
            <span class="cwsprite cross-lg-white"></span>
        </span>
        <div class="text-white content-inner-block-10 bg-brandColor">
            <h3 class="">View seller details in <strong>one click</strong></h3>
            <p id="txtRecommendationHeading" data-bind="visible: D_buyerProcess.recommendedCars.viewSellerBtnVisible" class="font11 hide">View seller details in <strong>one click</strong></p>
        </div>
        <div class="content-inner-block-10-20">
            <ul class="recommendationList margin-top10"  id="recommendations" data-bind="foreach: RECOMMENDLISTING">
            <li class="recommendedList" data-bind="attr: { id: 'RecommendedListing-' + $index(), rankabs: Rank }">
                <a class="recommendcarThumb" target="_blank" data-bind="attr: { href: Url, profileId: ProfileId, cityName: CityName, cityId: CityId, makeName: MakeName, modelName: ModelName, price: Price, seller: Seller }">
                    <img data-bind="attr: { profileId: ProfileId, alt: CarName, title: CarName, 'src': OriginalImgPath == '' || OriginalImgPath == null ? 'https://img.carwale.com/used/no-cars.jpg' : HostUrl + '370x208' + OriginalImgPath }">
  
                </a>
                <div class="recommendcarDetails font14">
                    <a class="carDetails text-default block" target="_blank" data-bind="attr: { href: Url, profileId: ProfileId, cityName: CityName, cityId: CityId, makeName: MakeName, modelName: ModelName, price: Price, seller: Seller }">
                        <p class="font14 margin-bottom5 text-link" data-bind="text: CarName"></p>
                        <p><span class="text-grey text-bold" data-bind="text: Price">1.40 lakhs</span></p>
                        <p class="margin-bottom5"><span data-bind="text: Km + ' km'">9,000 km</span><span class="margin-left5 margin-right5">|</span><span data-bind="    text: Fuel">Diesel</span><span class="margin-left5 margin-right5">|</span><span data-bind="    text: MakeYear">2014</span></p>
                        <div class="position-rel margin-bottom10">
                            <p class="recommendationList__cityName inline-block margin-bottom5" data-bind="text: AreaName + ', ' + CityName">Ashok Vihar, New Dehli</p>
                            <span class="dealer-rating-container rightfloat inline-block margin-right10">
                                <span data-bind="attr: { 'class': DealerRatingText == null || DealerRatingText == '' ? 'hide' : 'top-rated-seller-tag' }, text: DealerRatingText" class="top-rated-seller-tag">Top Rated Seller</span>
                                <span class="dealer-rating-tooltip dealer-rating-tooltip--right">This seller has consistently been rated well by his customers</span>
                            </span>
                        </div>
                    </a>
                    <div class="btnHolder">
                        <span class="inline-block position-rel grid-8" data-bind="visible: D_buyerProcess.recommendedCars.viewSellerBtnVisible">
                            <a data-bind="attr: { id: 'viewDetailsBtn-' + $index(), profileId: ProfileId, modelName: ModelName, makeYear: MakeYear, makeName: MakeName, makeId: MakeId, rootId: RootId, cityId: CityId, cityName: CityName, price: Price, bodyStyleId: BodyStyleId, versionsubsegmentID: VersionSubSegmentID, priceNumeric: PriceNumeric, kmNumeric: KmNumeric, 'data-cte-package-id': CtePackageId }" class="btn btn-orange btn-xs view-details seller-btn-container popup-seller-btn"><span class="oneClickDetails font18 toggleText">1-Click <span class="font15">VIEW DETAILS</span></span><span class="hideSellerDetails hideImportant ">HIDE SELLER DETAILS</span> 
                                <span class="fa fa-angle-down margin-left5"></span>
                            </a>
                        </span>
                    </div>
                </div>
                <div class="clear"></div>
                <!-- recommendation list seller details form code starts here -->
                <div class="othersellerDetails hide position-rel" data-bind="attr: { id: 'suggestDetailsData-' + $index() }">
                    <div class="content-inner-block-10">
                            <div class="recommendpreLoading text-white hide" data-bind="attr: { id: 'loadingIconRecommendations-' + $index() }">
                            <div class="loading-icon-recommendation"><img src="https://img.aeplcdn.com/adgallery/loader.gif"></div>
                                <div class="grid-6 alpha omega">
                                    <ul>
                                        <li>
                                            <span class="buyerprocess-sprite seller-name-ic opacity50"></span>
                                            <div class="inner-box">
                                                <h4 class="font14 seller-Name"></h4>
                                                <p>a</p>
                                            </div>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <span class="buyerprocess-sprite seller-call-ic opacity50"></span>
                                            <div class="inner-box seller-Contact">
                                                <p>a</p>
                                            </div>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <span class="buyerprocess-sprite seller-email-ic opacity50"></span>
                                            <div class="inner-box seller-Email">
                                                <p>a</p>
                                            </div>
                                            <div class="clear"></div>
                                        </li>
                                    </ul>
                            </div>
                            <div class="grid-6 alpha omega">
                                <ul>
                                    <li>
                                        <span class="buyerprocess-sprite seller-location-ic opacity50"></span>
                                        <div class="inner-box seller-Address">
                                            <p>a</p>
                                        </div>
                                        <div class="clear"></div>
                                    </li>
                                </ul>
                            </div>
                        <div class="clear"></div>
                        </div>
                        <div data-bind="attr: { id: 'sellerdetailsData-' + $index() }">
                            <div class="grid-6 alpha omega">
                                <ul>
                                    <li>
                                        <span class="buyerprocess-sprite seller-name-ic opacity50"></span>
                                        <div class="inner-box">
                                            <h4 class="text-black font14 seller-Name" data-bind="attr: { id: 'seller-Person' + $index() }"></h4>
                                            <p data-bind="attr: { id: 'seller-Name' + $index() }"></p>
                                        </div>
                                        <div class="clear"></div>
                                    </li>
                                    <li>
                                        <span class="buyerprocess-sprite seller-call-ic opacity50"></span>
                                        <div class="inner-box seller-Contact" data-bind="attr: { id: 'seller-Contact' + $index() }">
                                            <p></p>
                                        </div>
                                        <div class="clear"></div>
                                    </li>
                                    <li>
                                        <span class="buyerprocess-sprite seller-email-ic opacity50"></span>
                                        <div class="inner-box seller-Email" data-bind="attr: { id: 'seller-Email' + $index() }">
                                            <p></p>
                                        </div>
                                        <div class="clear"></div>
                                    </li>
                                </ul>
                        </div>
                        <div class="grid-6 alpha omega">
                            <ul>
                                <li>
                                    <span class="buyerprocess-sprite seller-location-ic opacity50"></span>
                                    <div class="inner-box seller-Address" data-bind="attr: { id: 'seller-Address' + $index() }">
                                        <p></p>
                                    </div>
                                    <div class="clear"></div>
                                </li>
                            </ul>
                        </div>
                            <div class="clear"></div>
                        </div>
                        <div class="font11 text-light-grey rightfloat" data-bind="attr: { id: 'details-msg' + $index() }">Your contact details have been shared with the seller.</div>
                        <div class="clear"></div>
                    </div>
                    <div class="clear"></div>
                </div>
            </li>                       
        </ul>
        </div>
    </div>
    <div class="clear"></div>
</div>
<!-- similar cars poupup ends here -->
<div id="similarCarPopup" class="searchRecommendation hide">
    <div class="grid-12 alpha omega animateRecommendation bg-white">
        <span class="similarcar-close position-abt pos-top10 pos-right10 cur-pointer popup-close-esc-key">
            <span class="cwsprite cross-lg-white" id="similar-popup-close"></span>
        </span>
        <div class="text-white content-inner-block-10 bg-brandColor">
            <h3 class="">Similar cars</h3>
        </div>
        <div class="content-inner-block-10-20">
            <ul class="recommendationList margin-top10"  id="similarCarsList" data-bind="foreach: RECOMMENDLISTING">
            <li class="recommendedList" data-bind="attr: { id: 'RecommendedListing-' + $index(), rankabs: Rank }">
                <a class="recommendcarThumb" target="_blank" data-bind="attr: { href: Url, profileId: ProfileId, cityName: CityName, cityId: CityId, makeName: MakeName, modelName: ModelName, price: Price, seller: Seller }">
                    <img data-bind="attr: { profileId: ProfileId, alt: CarName, title: CarName, 'src': OriginalImgPath == '' || OriginalImgPath == null ? 'https://img.carwale.com/used/no-cars.jpg' : HostUrl + '370x208' + OriginalImgPath }">
  
                </a>
                <div class="recommendcarDetails font14">
                    <a class="carDetails text-default block" target="_blank" data-bind="attr: { href: Url, profileId: ProfileId, cityName: CityName, cityId: CityId, makeName: MakeName, modelName: ModelName, price: Price, seller: Seller }">
                        <p class="font14 margin-bottom5 text-link" data-bind="text: CarName"></p>
                        <p><span class="text-grey text-bold" data-bind="text: Price">1.40 lakhs</span></p>
                        <p class="margin-bottom5"><span data-bind="text: Km + ' km'">9,000 km</span><span class="margin-left5 margin-right5">|</span><span data-bind="    text: Fuel">Diesel</span><span class="margin-left5 margin-right5">|</span><span data-bind="    text: MakeYear">2014</span></p>
                        <div class="position-rel margin-bottom10">
                            <p class="recommendationList__cityName inline-block margin-bottom5" data-bind="text: AreaName + ', ' + CityName">Ashok Vihar, New Dehli</p>
                            <span class="dealer-rating-container rightfloat inline-block margin-right10">
                                <span data-bind="attr: { 'class': DealerRatingText == null || DealerRatingText == '' ? 'hide' : 'top-rated-seller-tag' }, text: DealerRatingText" class="top-rated-seller-tag">Top Rated Seller</span>
                                <span class="dealer-rating-tooltip dealer-rating-tooltip--right">This seller has consistently been rated well by his customers</span>
                            </span>
                        </div>
                    </a>
                    <span class="cityName text-black hide" data-bind="text:CityName"></span>
                    <div class="listViewScreen1">
                           <div class="position-rel btn-container btnHolder">
                                <span class="inline-block preVerification">
                                    <a class="redirect-rt btn btn-orange btn-xs gridBtn contact-seller clickSearchTracking seller-btn-container grid-12 popup-seller-btn" data-bind="attr: { id: 'similarCarsDetailsBtn-' + $index(), profileId: ProfileId, modelName: ModelName, makeYear: MakeYear, makeName: MakeName, makeId: MakeId, rootId: RootId, cityId: CityId, cityName: CityName, price: Price, bodyStyleId: BodyStyleId, versionsubsegmentID: VersionSubSegmentID, priceNumeric: PriceNumeric, seller: Seller, kmNumeric: KmNumeric, 'data-cte-package-id': CtePackageId }">
                                        <span class="gsdTxt font18">Get Seller Details</span>
                                        <span class="oneClickDetails hide font18 text-bold">1-Click <span class="font11">View Details</span></span>
                                    </a>
                                </span>
                            </div>
                        <p class="text-grey seller-note"><i>Seller details will be sent on mobile number</i></p>
                        <div class="clear"></div>
                    </div>
                </div>
                <div class="clear"></div>
            </li>                       
        </ul>
        </div>
    </div>
    <div class="clear"></div>
</div>
<div id="verificationPopUp" class="verificationDetails text-right">
    <span class="position-abt pos-top5 pos-left5 cwsprite cross-sm-lgt-grey cur-pointer closeVerificationBtn"></span>
    <p class="font14 margin-bottom5 margin-top5 text-black">
        <span>Saved: </span>
        <span class="margin-bottom5 text-bold font13"><span class="verifiedNumber border-dotted-bottom">9867272258</span> <span class="margin-left5 fa fa-check text-green"></span></span>
    </p>
    <p class="font11">View seller details in <strong>One Click</strong></p>
</div>

<div id="sellerDetailsPopup">
	<span id="sellerDetailsClose" class="cur-pointer cwmsprite cross-lg-dark-grey popup-close-esc-key"></span>
	<div id="sellerDetailsForm" class="seller-details-form">
		<p class="popup-box__title">Fill in your details</p>
		<div class="form-control-box margin-bottom30">
			<input type="text" id="getName" class="form-control padding-left40" placeholder="Name" maxlength="50" tabindex="1">
			<span class="cw-used-sprite uc-uname"></span>
			<span class="cw-blackbg-tooltip hide"></span>
			<span class="cwsprite error-icon hide"></span>
		</div>
		<div class="form-control-box margin-bottom30">
			<input name="txtMobile" placeholder="Mobile Number" type="text" id="txtMobile" tabindex="2" maxlength="10" size="27" class="mobile-text text padding-left60 form-control inline-block verify-number getDetailsInput">
			<span class="cw-used-sprite uc-mobile"></span>
			<span id="txtMobileError" class="cw-blackbg-tooltip mobileError hide"></span>
			<span class="cwsprite error-icon mobileError hide"></span>
			<span class="form-mobile-prefix">+91</span>
		</div>
		<button type="button" id="submitSellerDetailsForm" class="btn btn-orange btn-full-width margin-bottom5 text-uppercase" tabindex="3">Submit</button>
       
        <p style="text-align:left; font-size:11px; margin-top:20px">By submitting this form you agree to our <a href="/visitoragreement.aspx" target="_blank">terms and conditions</a></p>
	</div>
</div>
<div class="modal-bg-window seller-details-modal-bg"></div>

<div id="otpForm">
	<span id="otpClose" class="cur-pointer cwmsprite cross-lg-dark-grey popup-close-esc-key"></span>
	<p class="popup-box__title">Enter OTP Here</p>
	<div class="form-control-box margin-bottom40">
		<input type="tel" id="getOTP" class="form-control" placeholder="Enter 5 digit OTP" maxlength="5">
		<span id="otpError" class="otp__error"></span>
		<p id="otpTimer" class="resend-otp-label">Resend OTP in <span class="time-counter">30</span>s</p>
	</div>
	<a href="javascript:void(0)" class="btn btn-orange btn-full-width text-uppercase margin-bottom10" id="verifyOTP" rel="nofollow">Verify</a>
    <img id="imgLoadingBtnVerify" class="position-abt hide" src="https://imgd.aeplcdn.com/0x0/statics/loader.gif" width="16" height="16">

     <p class="or-divider"><span>OR</span></p>
     <%-- Missed Call DIV starts here --%>
        <div class="missed-call-wrapper width80 " id="missed-call-wrapper">
           
            <a id="missed-call-number" class="btn btn-teal-ghost btn-full-width margin-top20 margin-bottom20 cur-default">
                <span class="tel-icon"></span>
            </a>
            <p class="missed-call__info-text">Simply give a MISSED CALL and</p>
            <p class="missed-call__error-msg hide">Error occured. Give a MISSED CALL and</p>
            <button class="missed-call__verify-link position-rel" type="button">
                Click here to verify
                <img id="missed-call__loading" class="hide position-abt" src="https://imgd.aeplcdn.com/0x0/statics/loader.gif" width="16" height="16">
            </button>
            
        </div>
        <%-- Ends --%>
</div>


<div class="modal-bg-window otp-modal-bg"></div>