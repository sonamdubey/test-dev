<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Controls.UploadPhotoRequestPopup" %>
<!-- request for image popup start -->
<div id="request-media-popup" class="bw-popup bwm-fullscreen-popup size-small">
    <div class="popup-inner-container text-center size-small">
        <div class="bwsprite bwmsprite close-btn request-media-close cross-lg-lgt-grey position-abt pos-top20 pos-right20 cur-pointer"></div>
        <div id="requester-details-section">
            <div class="icon-outer-container rounded-corner50percent rounded-corner50 margin-bottom10">
                <div class="icon-inner-container rounded-corner50percent rounded-corner50">
                    <span class="bwsprite bwmsprite request-media-icon margin-top15"></span>
                </div>
            </div>
            <p class="font18 text-bold margin-bottom10">Request Images</p>
            <p class="font14 text-light-grey margin-bottom25">Request the seller to upload images of this bike</p>

            <div class="input-box form-control-box margin-bottom10">
                <input type="text" id="requesterName" />
                <label for="requesterName">Name<sup>*</sup></label>
                <span class="boundary"></span>
                <span class="error-text"></span>
            </div>
            <div class="input-box form-control-box margin-bottom10">
                <input type="email" id="requesterEmail" />
                <label for="requesterEmail">Email<sup>*</sup></label>
                <span class="boundary"></span>
                <span class="error-text"></span>
            </div>
            <div class="input-box input-number-box form-control-box margin-bottom15">
                <input type="tel" id="requesterMobile" maxlength="10" />
                <label for="requesterMobile">Mobile number<sup>*</sup></label>
                <span class="input-number-prefix">+91</span>
                <span class="boundary"></span>
                <span class="error-text"></span>
            </div>
            <a class="btn btn-orange btn-fixed-width" id="submit-requester-details-btn">Request</a>
        </div>

        <div id="request-sent-section">
            <div class="icon-outer-container rounded-corner50percent rounded-corner50 margin-bottom10">
                <div class="icon-inner-container rounded-corner50percent rounded-corner50">
                    <span class="bwsprite bwmsprite thankyou-icon margin-top15"></span>
                </div>
            </div>
            <p class="font18 text-bold margin-bottom10">Request sent!</p>
            <p class="font14 text-light-grey margin-bottom25">
                We have requested seller to upload images.<br />
                We will let you know as soon as the seller uploads it.
            </p>

            <a class="btn btn-orange" id="submit-request-sent-btn">Done</a>
        </div>
    </div>
</div>
<!-- request for image popup ends -->

<script type="text/javascript">
    var requesterName = $('#requesterName'),
    requesterEmail = $('#requesterEmail'),
    requesterMobile = $('#requesterMobile');

    $('#request-media-btn').on('click', function () {
        requestMediaPopup.open();
        appendHash("requestMedia");
        $('body, html').addClass('lock-browser-scroll');
        shownInterestInThisBike("<%= ProfileId %>");
    });

    $('#submit-requester-details-btn').on('click', function () {
        if (ValidateUserDetail(requesterName, requesterEmail, requesterMobile)) {            
            submitPhotoRequest();            
        }
    });

    $('#submit-request-sent-btn, .request-media-close').on('click', function () {
        requestMediaPopup.close();
        window.history.back();
        $('body, html').removeClass('lock-browser-scroll');
    });

    var requestMediaPopup = {
        popup: $('#request-media-popup'),

        userDetails: $('#requester-details-section'),

        acknowledgment: $('#request-sent-section'),

        open: function () {
            requestMediaPopup.popup.show();
        },

        close: function () {
            requestMediaPopup.popup.hide();
            requestMediaPopup.userDetailsSection();
        },

        userDetailsSection: function () {
            requestMediaPopup.acknowledgment.hide();
            requestMediaPopup.userDetails.show();
        },

        acknowledgmentSection: function () {
            requestMediaPopup.userDetails.hide();
            requestMediaPopup.acknowledgment.show();
        },
    };

    function shownInterestInThisBike(profileId) {
        $.ajax({
            type: "POST",
            url: "/api/bikebuyer/showninterest/?profileId=" + profileId + "&isDealer=false",
            contentType: "application/json",
            dataType: 'json',            
            success: function (response) {
                var ds = response;
                // buyer already shown interest; Show seller information;        
                if (ds.shownInterest) {                             
                    $('#request-media-btn').hide();                    
                } else { // first time showing interest
                    if (ds.buyer && ds.buyer.customerName && ds.buyer.customerEmail && ds.buyer.customerMobile) {
                        if (ds.buyer.customerName.length > 0)
                            requesterName.val(ds.buyer.customerName).closest(".input-box").addClass("not-empty"); //Prefill buyer information
                        if (ds.buyer.customerEmail.length > 0 && ds.buyer.customerEmail !="undefined")
                            requesterEmail.val(ds.buyer.customerEmail).closest(".input-box").addClass("not-empty");
                        if (ds.buyer.customerMobile.length > 0)
                            requesterMobile.val(ds.buyer.customerMobile).closest(".input-box").addClass("not-empty");                        
                    }
                }
            },
            complete: function (xhr, ajaxOptions, thrownError) {
                if (xhr.status != 200){

                }
             }        
        });
    }

    
    function submitPhotoRequest(){
        var buyerMessage = "";		
        var objData = {
            "buyer":
		            {
		                "customerId": 0,
		                "customerName": requesterName.val(),
		                "customerEmail": requesterEmail.val(),
		                "customerMobile": requesterMobile.val()
		            },
            "profileId": "<%= ProfileId %>",
            "bikeName": "<%= BikeName %>"
        };
        $.ajax({
            type: "POST",
            url: "/api/bikebuyer/requestphotos/",
            data: ko.toJSON(objData),
            contentType: "application/json",
            dataType: 'json',            
            success: function(response) {                
                if (response) {
                    $('#request-media-btn').hide();                                        
                }
                requestMediaPopup.acknowledgmentSection();
            },
            complete: function (xhr, ajaxOptions, thrownError) {                
                if (xhr.status != 200) {

                }
            }
        });        
    }
    /* input focus */
    requesterName.on("focus", function () {
        validate.onFocus(requesterName);
    });

    requesterEmail.on("focus", function () {
        validate.onFocus(requesterEmail);
    });

    requesterMobile.on("focus", function () {
        validate.onFocus(requesterMobile);
    });

    /* input blur */
    requesterName.on("blur", function () {
        validate.onBlur(requesterName);
    });

    requesterEmail.on("blur", function () {
        validate.onBlur(requesterEmail);
    });

    requesterMobile.on("blur", function () {
        validate.onBlur(requesterMobile);
    });
</script>
