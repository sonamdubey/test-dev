<div id="m-blackOut-window" class="hide"></div>
<style type="text/css">
    .width80 { overflow: inherit; }
    div.leftfloat.seller-Name-div{width:85%;word-wrap:break-word;border-bottom:1px solid #ccc;padding:0 0 10px}
</style> 
<div id="buyer-blackout-window" class="hide"></div>    
<div id="buyerForm" class="buyerProcess padding-top10 text-center hide">
<span class="cur-pointer cwmsprite cross-lg-dark-grey seller-close"></span>
    <div class="certificationPdfScreen hide">
        <div class="upeer-box">
            <h2 class="text-black text-bold padding-bottom5">Download Report</h2>
			<p class="font14 padding-bottom10">Your mobile no. will be shared with the seller.</p>
			<div class="position-rel width80">
				<input type="text" class="form-control padding-right20 padding-left20 margin-bottom20" placeholder="Name" id="pdftxtUserName" maxlength="50">
			</div>
            <div class="position-rel width80">
                <input type="tel" class="form-control padding-right60 padding-left40 margin-bottom20" placeholder="Mobile number" id="pdftxtMobileNo" maxlength="10">
                <span class="position-abt pos-top10 pos-left10">+91</span>
                <img id="pdfimgLoadingBtnSend" class="position-abt pos-right-25 pos-top10 hide" src="https://imgd.aeplcdn.com/0x0/statics/loader.gif" width="16" height="16">
			</div>
			<div class="width80">
				<a class="btn btn-orange btn-full-width text-uppercase buyer-pro-btn" id="pdfbtnSend">DOWNLOAD</a>
			</div>
        </div>
    </div>
    <div class="screen1 hide">
        <div id="cityWarningGSD" class="border-solid-bottom padding-bottom10 padding-left10 text-left">
            <div id="locationWarning" class="inline-block valign-middle padding-right10">
                <span class="cwmsprite car-location-ic-sm"></span>
            </div>
            <div class="inline-block valign-middle locationDetails">
                <p class="margin-bottom5">Car location: <span id="carLocationText" class="carLocation text-bold"></span></p>
                <p id="changeCity">Not your city? <span class="text-link changeCityLocation">Search cars in your city</span></p>
                <p id="deliveryText" class="hide margin-bottom5 carDelivery"></p>
            </div>
        </div>
        <div id="getSellerDetailsForm" class="upeer-box">
			<h2 class="text-black text-bold padding-bottom10">Fill in your details</h2>
			<div class="position-rel width80">
				<input type="text" class="form-control padding-right20 padding-left20 margin-bottom20" placeholder="Name" id="txtUserName" maxlength="50">
			</div>
            <div class="position-rel width80">
                <input type="tel" class="form-control padding-right60 padding-left40 margin-bottom20" placeholder="Mobile number" id="txtMobileNo" maxlength="10">
                <span class="position-abt pos-top10 pos-left10">+91</span>
                <img id="imgLoadingBtnSend" class="position-abt pos-right-25 pos-top10 hide" src="https://imgd.aeplcdn.com/0x0/statics/loader.gif" width="16" height="16">
			</div>
			<div class="width80">
				<a class="btn btn-orange btn-full-width text-uppercase buyer-pro-btn" id="btnSend">Submit</a>
			</div>
            <p class="width80" style="font-size:11px; padding-top:20px">By submitting this form you agree to our <a href="/visitoragreement.aspx" target="_blank">terms and conditions</a></p>
		</div>
    </div>
    <div id="otpForm" class="upeer-box hide">
        <p class="upper-box__title">Enter OTP Here</p>
        <div class="position-rel width80">
            <input type="tel" id="getOTP" class="form-control" placeholder="Enter 5 digit OTP" maxlength="5">
            <a href="javascript:void(0)" class="btn btn-orange btn-xs text-uppercase buyer-pro-btn" id="verifyOTP" rel="nofollow">Verify</a>
            <span class="otp__error"></span>
        </div>
        <div class="position-rel width80">
            <p id="otpTimer" class="resend-otp-label">Resend OTP in <span class="time-counter">30</span>s</p>
            <div class="clear"></div>
        </div>
        <div class="missed-call-wrapper width80">
            <p class="or-divider"><span>OR</span></p>
            <a id="missed-call-number" class="btn btn-teal-ghost btn-full-width margin-top20 margin-bottom20">
                <span class="tel-icon"></span>
            </a>
            <p class="missed-call__info-text">Simply give a MISSED CALL and</p>
            <p class="missed-call__error-msg hide">Error occured. Give a MISSED CALL and</p>
            <button class="missed-call__verify-link position-rel">
                Click here to verify
                <img id="missed-call__loading" class="hide" src="https://imgd.aeplcdn.com/0x0/statics/loader.gif" width="16" height="16">
            </button>
        </div>
    </div>
    <div class="screen3 hide" style="overflow:scroll !important;">
        <h2 class="text-black text-bold">Seller details</h2>
        <div class="position-rel">
            <ul class="sellerDetails margin-bottom10 content-inner-block-10 showToastMessage">
                <li >
                    <span class="buyerprocess-sprite seller-name"></span>
                    <div id="seller-Name-div" class="leftfloat">
                        <p class="seller-Person top-rated-seller-name-field"></p>
                        <span id="seller-details-rating-text" class="top-rated-seller-tag" style="display:none"></span>
                        <div class="seller-rating__toast"><span class="seller-rating-toast__close"></span><p>This seller has consistently been rated well by his customers</p></div>
                        <p class="seller-Name font13 text-light-grey"> Loading...</p>
                    </div>
                    <div class="clear"></div>
                </li>
                <li >
                    <span class="buyerprocess-sprite seller-masking-no"></span>
                    <p class="leftfloat seller-Contact"></p>
                    <div class="clear"></div>
                </li>
                <li >
                    <span class="buyerprocess-sprite seller-email"></span>
                    <p class="leftfloat"><a class="seller-Email"></a></p>
                    <div class="clear"></div>
                </li>
            
                <li >
                    <span class="buyerprocess-sprite seller-address"></span>
                    <p class="leftfloat"><span class="seller-Address"></span></p>
                    <div class="clear"></div>
                    <p><span class="font10 text-light-grey margin-left10">Your contact details have been shared with the seller.</span></p>
                </li>
            </ul>
            <div class="moveRecommendation hide">
                <span class="fa fa-angle-double-up"></span>
            </div>
        </div>
        <!-- recommended cars code starts here -->
        <div class="suggestion-box hide">
            <h2 class="text-black text-bold padding-top5 padding-bottom5 recommendationTitle">Recommendations for you</h2>
            <ul id="suggetions" class="suggestion-list" data-bind="foreach: m_bp_recommendedCars.LISTING, attr: {count:m_bp_recommendedCars.LISTING().length}">
                <li class="recommendedList" data-bind="attr: { id: 'RecommendedListing-' + $index(),profileid: ProfileId,rankabs: Rank}">
                    
                    <h4 class="margin-bottom10 text-unbold">
                        <a target="_blank" class="carNameClass" data-bind="attr: { href: '/m' + Url +'?rk=r'+($index()+1)+'&scid=0'}, text: CarName">Mercedes-Benz C-ClassC 200 Ava</a>
                    </h4>
                    <div class="carThumb">
                        <a class="imgholder" target="_blank"  data-bind="attr: { href: '/m' + Url + '?rk=r' + ($index() + 1) + '&scid=0' }">
                            <%--<img src="<%= Carwale.Utility.CWConfiguration._imgHostUrl %>370x208/cw/tc/CE/1B/708174/ol/2015-Mercedes-Benz-M-Class-2006-2012-350-CDI-3434571.JPG">--%>
                             <img data-bind="attr: { profileId: ProfileId, alt: CarName, title: CarName, 'src': OriginalImgPath == '' || OriginalImgPath == null ? 'https://img.carwale.com/used/no-cars.jpg' : HostUrl + '370x208' + OriginalImgPath }">
                        </a>
                    </div>
                    <a class="carDetails text-default" target="_blank" data-bind="attr: { href: '/m' + Url + '?rk=r' + ($index() + 1) + '&scid=0', title: CarName, profileid: ProfileId }">
                        <p class="margin-bottom5">₹<span class="margin-left5 text-grey text-bold" data-bind="text: Price">7.85 lakhs</span></p>
                      <%--  <h3 class="margin-bottom5">₹1.75 lakhs</h3>--%>
                        <p class="font13 margin-bottom5" data-bind="attr: { href: Url }"><span data-bind="    text: Km + ' km'">16,186 Km</span> | <span data-bind="    text: Fuel">Diesel</span> | <span data-bind="    text: MakeYear">2015</span></p>
                        <p data-bind="text: AreaName + ', ' + CityName">Santacruz(E), Mumbai</p>
                    </a>
                    <div class="clear"></div>
                    <p class="padding-top10 buttonMain">
                        <a data-bind="attr: { id: 'viewDetailsBtn-' + $index(), profileId: ProfileId, modelName: ModelName, makeYear: MakeYear, makeName: MakeName, makeId: MakeId, rootId: RootId, cityId: CityId, cityName: CityName, price: Price, bodyStyleId: BodyStyleId, versionsubsegmentID: VersionSubSegmentID, priceNumeric: PriceNumeric, kmNumeric: KmNumeric }" class="btn btn-white btn-xs text-uppercase view-details" oid="-1"><span class="toggleText">View seller details</span><span class="fa fa-angle-down margin-left5"></span></a>
                    </p>
                    <!-- recommended car seller details div starts here -->
                    <div class="suggestDetails hide" data-bind="attr: { id: 'suggestDetails-' + $index()}">
                       <div class="border-solid content-box-shadow margin-top5 content-inner-block-5 position-rel">
                            <div class="preloadingBox hide" data-bind="attr: { id: 'preloadingBox-' + $index()}">
                                <img class="recommendloadIcon centerAlign" src="https://imgd.aeplcdn.com/0x0/statics/loader.gif" width="16" height="16">
                                <ul class="sellerDetails margin-bottom10 content-inner-block-10 text-white">
                                    <li >
                                        <span class="buyerprocess-sprite seller-name"></span>
                                        <p class="leftfloat"><span>a</span><br /><span> </span></p>
                                        <div class="clear"></div>
                                    </li>
                                    <li >
                                        <span class="buyerprocess-sprite seller-masking-no"></span>
                                        <p class="leftfloat"><span>a</span></p>
                                        <div class="clear"></div>
                                    </li>
                                    <li >
                                        <span class="buyerprocess-sprite seller-email"></span>
                                        <p class="leftfloat"><span>a</span></p>
                                        <div class="clear"></div>
                                    </li>
            
                                    <li >
                                        <span class="buyerprocess-sprite seller-address"></span>
                                        <p class="leftfloat"><span>a</span></p>
                                        <div class="clear"></div>
                                        <p><span class="font10 text-light-grey margin-left10">Your contact details have been shared with the seller</span></p>
                                    </li>
                            </ul> 
                            </div>    
                            <!-- append seller detail ul li here -->
                             <ul class="sellerDetails sellerdetailsData content-inner-block-10 hide" data-bind="attr: { id: 'sellerdetailsData-' + $index()}">
                                    <li >
                                        <span class="buyerprocess-sprite seller-name"></span>
                                        <div class="leftfloat seller-Name-div"><p class="seller-Person" data-bind="attr: { id: 'seller-Person' + $index()}"></p><p class="seller-Name font13 text-light-grey" data-bind="attr: { id: 'seller-Name' + $index()}"></p></div>
                                        <div class="clear"></div>
                                    </li>
                                    <li >
                                        <span class="buyerprocess-sprite seller-masking-no"></span>
                                        <p class="leftfloat seller-Contact" data-bind="attr: { id: 'seller-Contact' + $index()}"></p>
                                        <div class="clear"></div>
                                    </li>
                                    <li >
                                        <span class="buyerprocess-sprite seller-email"></span>
                                        <p class="leftfloat"><a data-bind="attr: { id: 'seller-Email' + $index()}" class="seller-Email"></a></p>
                                        <div class="clear"></div>
                                    </li>
            
                                    <li >
                                        <span class="buyerprocess-sprite seller-address"></span>
                                        <p class="leftfloat"><span data-bind="attr: { id: 'seller-Address' + $index()}" class="seller-Address"></span></p>
                                        <div class="clear"></div>
                                        <p><span class="font10 text-light-grey margin-left10">Your contact details have been shared with the seller</span></p>
                                    </li>
                            </ul>
                            
                        </div>
                    </div>
                </li>
            </ul>
        </div>
    </div>
    </div>
    <div data-role="popup" id="verfiyError" data-overlay-theme="a" data-theme="c" data-dismissible="false"  class="text-center buyer-popup rounded-corner2 content-box-shadow hide">
        <div data-role="header" data-theme="a" class="ui-corner-top content-inner-block-10">
            <span class="error-image"></span>
            <h3 id="errorHeading">Wrong OTP Entered</h3>
        </div>
        <div data-role="content" data-theme="d" class="ui-corner-bottom ui-content ">
            <span id="verifyErrorMssg" class="error padding-left10 padding-right10 block">Please re-enter valid OTP.</span>
            <a href="javascript:void(0)" data-role="button" class="btn btn-xs btn-white btn-full-width margin-top15" id="btnErrorOk">OK</a>
        </div>
    </div>


