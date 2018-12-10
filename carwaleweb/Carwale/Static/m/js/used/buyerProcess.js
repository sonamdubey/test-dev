var M_buyerProcess = function () {

    var m_buyerProcessSelf = this;

    m_buyerProcessSelf.getSellerDetails = function () {
        var _self = this;
        _self.registerEvents = function () {
            m_bp_DOC.on('click', 'a.getSellerDetails', function (event) {
                tracker.setSlotId(this);
                $m_bp_GSDBUTTON = $(this);
                leadType.setCurrLeadType(leadType.getSellerDetailLead().Value);
                _self.commonClickActions();
                $("#m-blackOut-window").show();
                m_bp_recommendedCars.ISRECOMMENDATION = false;
                $m_bp_GSDBUTTON.addClass("selected");
                if ($m_bp_GSDBUTTON.parent('.seller-btn-detail').length)
                    search.showSimilarCarsLink($m_bp_GSDBUTTON.closest(".stockDetailsBlock"), $m_bp_GSDBUTTON.attr("profileId"));
                _self.gsdClick($m_bp_GSDBUTTON, leadType.getSellerDetailLead().Name);
                if (!$('#deliveryCity').is(':visible'))
                    $('#changeCityDetailsPage').show();
                if ($(this).parent('.seller-btn-detail').length)
                    $('#buyerForm').removeClass('changeCityPosition');
            });
            m_bp_DOC.on('click', "span.seller-close", function (e) {      //Close Icon Click
                history.back();
            });
            m_bp_DOC.on('click', '#certificatePdfLink', function () {
                tracker.setSlotId();
                $("#m-blackOut-window").show();
                leadType.setCurrLeadType(leadType.getCertificationReportLead().Value);
                $m_bp_GSDBUTTON = $('#getsellerDetails');
                window.history.pushState(leadType.getSellerDetailLead().Name, "", "");
                var isExists = _self.checkCookieExists('TempCurrentUser');
                if (isExists) {
                    var tempCurrentUser = _self.getCookie('TempCurrentUser').split(":");
                    $('#pdftxtUserName').val(tempCurrentUser[0]);
                    $('#pdftxtMobileNo').val(tempCurrentUser[1]);
                }
                m_bp_pdfScreen.showScreen();
            });

        };

        _self.commonClickActions = function () {
            Common.utils.lockPopup();
            if (navigator.userAgent.match(/(iPod|iPhone|iPad)/)) {
                scrollTopPosition = $(window).scrollTop();
            }
            $("img.imgLoadingSPage").addClass('hide');
        }
        _self.resetCommonClickActions = function () {
            if (navigator.userAgent.match(/(iPod|iPhone|iPad)/)) {
                $(window).scrollTop(scrollTopPosition);
            }
        }

        _self.gsdClick = function ($node, leadType) {
            _self.selectScreen($node);
            window.history.pushState(leadType, "", "");
        };

        _self.selectScreen = function ($node) {
            var isExists;
            var isChatSms = commonUtilities.getFilterFromQS("ischatsms", window.location.search);
            if (isChatSms) {
                M_buyerProcess_Utils.removeParameterFromQS('ischatsms');
                _self.setLeadFormTitle("Please enter your details to view chat.");
            }
            var originId = m_bp_process.getOriginId($node[0]); // taking out the javascript object from jquery object.
            var inputParams = {
                originId: originId,
                isNumberVerified: false,
                slotId: tracker.getSlotId()
            };
            if ($node[0]) {
                inputParams.ctePackageId = $node[0].dataset.ctePackageId;
            }
            isExists = _self.checkCookieExists('TempCurrentUser');
            if (isExists) {
                var tempCurrentUser = _self.getCookie('TempCurrentUser').split(":");
                $('#txtUserName').val(tempCurrentUser[0]);
                $('#txtMobileNo').val(tempCurrentUser[1]);
                m_bp_process.submitRequestForInquiry(m_bp_process.getStockDetails($node), $node, isChatSms);
                inputParams.isNumberVerified = true;
                tracker.trackGsdClick(inputParams);
            }
            else {
                m_bp_screen1.showScreen();
                if (leadType.getCurrLeadType().Value === leadType.getWhatsAppLead().Value) {
                    var action = M_tracking.Tracking.variables.getWhatsAppChatUnVerifiedText();
                    cwTracking.trackCustomData(M_tracking.Tracking.variables.getBhriguCategory(), action, 'profileId=' + $node.attr('profileid'), true);
                }
                tracker.trackGsdClick(inputParams);
            }
        };

        _self.checkCookieExists = function (cookieName) {
            try {
                var cookieList, currentCookie;
                cookieList = document.cookie.split(';');
                for (var i = 0; i < cookieList.length; i++) {
                    currentCookie = cookieList[i];
                    if (currentCookie.indexOf(cookieName) != -1)
                        return true;
                }
            }
            catch (e) {
                logError(e.message, e.stack, "M_buyerProcess - m_buyerProcessSelf.getSellerDetails.checkCookieExists", location.pathname);
            }
            return false;
        };

        _self.getCookie = function (visited) {   // function to get the value of cookie
            try {
                var Isvisited = visited + "=";
                var ca = document.cookie.split(';');
                for (var i = 0; i < ca.length; i++) {
                    var c = ca[i];
                    while (c.charAt(0) == ' ') c = c.substring(1);
                    if (c.indexOf(Isvisited) != -1)
                        return c.substring(Isvisited.length, c.length);
                }
            }
            catch (e) {
                logError(e.message, e.stack, "M_buyerProcess - m_buyerProcessSelf.getSellerDetails.getCookie", location.pathname);
            }
            return "";
        };

        _self.closeClick = function () {
            try {
                $('#buyerForm').slideUp();
                $("#imgLoadingBtnMissCall").hide();
                $('#m-blackOut-window').hide();
                $('#buyerForm .screen1').hide();
                $('#buyerForm .screen3').hide();
                $('#buyerForm .certificationPdfScreen').hide();
                $('.suggestion-box').hide();
                $('div.moveRecommendation').hide();
                $('#deliveryText').hide();
                $('#otpForm').hide();
                $('.seller-rating__toast').hide();                      //hide top rated tool tip
                m_bp_pdfScreen.ISCERTIFICATION = false;
                clearInterval(m_bp_TIMER);
                clearInterval(m_bp_ZIPTIMEOUT);
                $m_bp_GSDBUTTON.removeClass('no-events selected');
                Common.utils.unlockPopup(true);
                _self.setLeadFormTitle("Fill in your details");
                _self.resetCommonClickActions();
            }
            catch (e) {
                logError(e.message, e.stack, "M_buyerProcess - m_buyerProcessSelf.getSellerDetails.closeClick", location.pathname);
            }
        };

        _self.closeFormPopup = function () {
            try {
                _self.closeClick();
            }
            catch (e) {
                logError(e.message, e.stack, "M_buyerProcess - m_buyerProcessSelf.getSellerDetails.closeFormPopup", location.pathname);
            }
        };

        _self.setLeadFormTitle = function (text) {
            $("#leadformTitle").text(text);
        }
    };

    m_buyerProcessSelf.process = function () {
        var _self = this;
        _self.isForbidden = false;
        _self.isLimitExceeded = false;

        _self.originId = {
            searchPage: 5,
            detailsPage: 6,
            photoGallery: 11,
            searchRecommendation: 12,
            detailsRecommendation: 13,
            photoGalleryRecommendation: 14,
            searchNearByCity: 21,
            certificationReport: 28,
            searchPageRightPrice: 34,
            detailsPageRightPrice: 35,
            rightPriceRecommendation: 36,
            searchSimilarCars: 19
        };

        _self.showReportInputScreenAfterError = function () {
            m_bp_pdfScreen.showScreen();
            m_bp_pdfScreen.ISCERTIFICATION = false;
        };

        _self.showInputScreenAfterError = function () {
            if (m_bp_pdfScreen.ISCERTIFICATION) {
                _self.showReportInputScreenAfterError();
            }
            else {
                if ($('.screen3').css('display') == 'none') {
                    m_bp_screen1.showScreen();
                }
            }
        };

        _self.showPdfDownloadError = function () {
            try {
                m_bp_additonalFn.showErrorMessage("Report Download Error", "Report not available.");
                if (m_bp_pdfScreen.ISCERTIFICATION) {
                    _self.showReportInputScreenAfterError();
                }
            }
            catch (e) {
                logError(e.message, e.stack, "M_buyerProcess - m_buyerProcessSelf.process.showPdfDownloadError", location.pathname);
            }
        };

        _self.getUserDetails = function () {
            try {
                var tempMobileNo, user, userName;
                if (m_bp_pdfScreen.ISCERTIFICATION) {
                    if ($('#pdftxtMobileNo')) {
                        tempMobileNo = $('#pdftxtMobileNo').val().trim();
                        userName = $('#pdftxtUserName').val();
                    }
                }
                else {
                    if ($('#txtMobileNo')) {
                        tempMobileNo = $('#txtMobileNo').val().trim();
                        userName = $('#txtUserName').val();
                    }
                }
                user = { name: userName, email: '', mobileNo: tempMobileNo };
                return user;
            }
            catch (e) {
                logError(e.message, e.stack, "M_buyerProcess - m_buyerProcessSelf.process.getUserDetails", location.pathname);
            }
            return { name: '', email: '', mobileNo: '' };
        };

        _self.getOriginId = function (gsdNode) {
            if (gsdNode) {
                var originId = gsdNode.getAttribute('oid');
                if (originId && originId !== '-1') {
                    originId = parseInt(originId);
                }
                else {
                    originId = _self.getOriginIdForRecommendationsAndSimilarCars(msite_ORIGIN_ID);
                }
                return originId;
            }
            else {
                return -1;
            }
        };

        _self.getOriginIdForRecommendationsAndSimilarCars = function (mSiteOriginId) {
            var originId;
            switch (mSiteOriginId) {
                case _self.originId.searchPage: originId = _self.originId.searchRecommendation;
                    break;
                case _self.originId.detailsPage: originId = _self.originId.detailsRecommendation;
                    break;
                case _self.originId.photoGallery: originId = _self.originId.photoGalleryRecommendation;
                    break;
                case _self.originId.searchNearByCity: originId = _self.originId.searchRecommendation;
                    break;
                case _self.originId.searchPageRightPrice: originId = _self.originId.rightPriceRecommendation;
                    break;
                case _self.originId.detailsPageRightPrice: originId = _self.originId.rightPriceRecommendation;
                    break;
                case _self.originId.searchSimilarCars: originId = _self.originId.searchSimilarCars;
                    break;
                default: originId = -1;
            }
            return originId;
        };
        _self.getStockDetails = function ($node) {
            try {
                var tempProfileId, tempRootId, tempIsDealer, tempCityName, stock, tempOriginId, tempRank, tempDeliveryCity;
                tempProfileId = $node.attr('profileid');
                tempRootId = $node.attr('rootId');
                tempCityName = $node.attr('cityName');
                tempIsDealer = tempProfileId.substring(0, 1).toLowerCase() == 'd' ? 1 : 0;
                tempDeliveryCity = $node.attr('dc');
                tempOriginId = $node.attr('oid');
                if (tempOriginId === '-1') {
                    tempOriginId = _self.getOriginIdForRecommendationsAndSimilarCars(msite_ORIGIN_ID);
                }
                else {
                    tempOriginId = parseInt($node.attr('oid'));
                    if (tempOriginId) {
                        if ($node.attr('isnearbycitylisting')) {
                            tempOriginId = _self.originId.searchNearByCity;
                        }
                        if (m_bp_pdfScreen.ISCERTIFICATION) {
                            tempOriginId = _self.originId.certificationReport;//28 m site certification pdf download
                        }
                    }
                    else {
                        tempOriginId = -1;
                    }
                    msite_ORIGIN_ID = tempOriginId;

                }
                tempRank = $node.closest('li').attr('rankabs');
                if (!tempRank) {
                    tempRank = _self.getRankFromQS();
                }
                stock = { profileId: tempProfileId, rootId: tempRootId, isDealer: tempIsDealer, cityName: tempCityName, deliveryCity: tempDeliveryCity, originId: tempOriginId, rank: tempRank };
                return stock;
            }
            catch (e) {
                logError(e.message, e.stack, "M_buyerProcess - m_buyerProcessSelf.process.getStockDetails", location.pathname);
            }
            return { profileId: '', rootId: '', isDealer: '', cityName: '', originId: '', rank: '' };
        };
        _self.getRankFromQS = function () {
            var qs = location.search;
            var params = qs.replace('?', '').split('&');
            var rank = params.find(function (element) {
                if (element.split('=')[0] === 'rk')
                    return element;
            });
            return rank ? rank.split('=')[1] : undefined;
        };
        _self.handleVerificationSuccessResponse = function () {
            var otpForm = m_bp_screen1.otpForm;
            otpForm.find('.otp__error').text('');
            otpForm.hide();
            m_bp_process.submitRequestForInquiry(m_bp_process.getStockDetails($m_bp_GSDBUTTON), $m_bp_GSDBUTTON);
        };
        _self.GetLeadType = function () {
            var _leadType = leadType.getCurrLeadType();
            return _leadType ? _leadType.Value : undefined;
        }
        _self.IsValidLatLong = function (latitude, longitude) {
            return (latitude >= -90.0 && latitude <= 90.0 && longitude >= -180.0 && longitude <= 180);
        }
        _self.GetLatLong = function (leadTrackingParams) {
            var latitude = $.cookie("_CustLatitude");
            var longitude = $.cookie("_CustLongitude");
            if (_self.IsValidLatLong(latitude, longitude)) {
                leadTrackingParams["latitude"] = latitude;
                leadTrackingParams["longitude"] = longitude;
            }
        }
        _self.submitRequestForInquiry = function (stock, $node, isChatSms) {
            var user = _self.getUserDetails();
            var leadData = {
                "profileId": stock.profileId,
                "buyer": {
                    "name": user.name,
                    "mobile": user.mobileNo,
                    "email": user.email
                },
                "leadTrackingParams": {
                    "originId": stock.originId,
                    "rank": stock.rank,
                    "deliveryCity": stock.deliveryCity,
                    "leadType": _self.GetLeadType(),
                    "slotId": tracker.getSlotId()
                }
            };
            if (
                leadData.leadTrackingParams.originId === _self.originId.searchPage ||
                leadData.leadTrackingParams.originId === _self.originId.searchNearByCity ||
                leadData.leadTrackingParams.originId === _self.originId.searchPageRightPrice
            ) {
                leadData.leadTrackingParams["queryString"] = window.location.search.substr(1);
            }
            if (isChatSms) {
                leadData.ischatsms = isChatSms;
            }
            _self.GetLatLong(leadData.leadTrackingParams);
            if (stock.rootId) {
                m_bp_additonalFn.addToLocalStorage(m_bp_additonalFn.rootResponsivekey, stock.rootId);
            }

            $("#cw_loading_icon").removeClass('hide');      //we are using 'display: flex', so cann't do .show()
            _self.processPurchaseInquiry(leadData, $node, stock);
        };

        _self.processPurchaseInquiry = function (leadData, $node, stock) {
            try {
                var stockleadsApi = $.ajax({
                    type: "POST",
                    url: "/api/v1/stockleads/",
                    headers: { 'sourceid': 43 },
                    data: JSON.stringify(leadData),
                    contentType: 'application/json',
                    dataType: 'json',
                    error: function (xhr) {
                        _self.handleErrorResponse(xhr, $node, leadData.buyer.mobile);
                    }
                });



                if (!m_bp_pdfScreen.ISCERTIFICATION && !(m_bp_recommendedCars.ISRECOMMENDATION) && leadType.getCurrLeadType().Value !== leadType.getChatLead().Value) {//no need to send request for recommendations in case of chat.
                    if (!$('#recommendedCarsPopup').is(':visible')) {
                        var similarCarsUrl = $node.attr('popupurl');
                        var fetchSimilarCars = $.ajax({
                            url: similarCarsUrl
                        });
                    }
                }

                $.when(stockleadsApi, fetchSimilarCars)
                    .done(function (stockLeadsResponse, fetchSimilarCarsResponse) {
                        var exotelObj = { pId: leadData.profileId, dealer: stock.isDealer.toString(), userName: leadData.buyer.name, userEmail: leadData.buyer.email, userMobileNo: leadData.buyer.mobile };

                        _self.handleSuccessResponse(stockLeadsResponse[0], exotelObj, $node, stock);
                        m_bp_screen1.hideScreen();
                        Common.utils.unlockPopup();
                        if (leadType.getCurrLeadType().Value === leadType.getWhatsAppLead().Value) {
                            $('#m-blackOut-window').hide();
                        }
                        else if (fetchSimilarCarsResponse && !(m_bp_recommendedCars.ISRECOMMENDATION) && leadType.getCurrLeadType().Value !== leadType.getChatLead().Value) {
                            m_bp_detailsScreen.showScreen();
                            _self.bindRecommenedCarsInSellerDetails(fetchSimilarCarsResponse[0]);
                        }
                    });
            }
            catch (e) {
                logError(e.message, e.stack, "M_buyerProcess - m_buyerProcessSelf.process.processPurchaseInquiry", location.pathname);
            }
        };

        _self.triggerFacebookTracking = function (stock) {
            switch (stock.originId) {
                case 5:
                    fbq('track', 'Lead', { content_name: 'msite search page', content_category: 6 });
                    break;
                case 6:
                case 28:
                    fbq('track', 'Lead', { content_name: 'msite details page', content_category: 7 });
                    break;
                case 11:
                    fbq('track', 'Lead', { content_name: 'msite PhotoGallery', content_category: 8 });
                    break;
                case 12:
                    fbq('track', 'Lead', { content_name: 'msite search recommendations', content_category: 9 });
                    break;
                case 13:
                    fbq('track', 'Lead', { content_name: 'msite details recommendations', content_category: 10 });
                    break;
                case 14:
                    fbq('track', 'Lead', { content_name: 'msite PhotoGallery recommendations', content_category: 11 });
                    break;
                case 19:
                    fbq('track', 'Lead', { content_name: 'msite search similar cars', content_category: 16 });
                    break;
                case 21:
                    fbq('track', 'Lead', { content_name: 'msite search nearby city cars', content_category: 21 });
                    break;
                default:
                    fbq('track', 'Lead', { content_name: 'msite page', content_category: 22 });
            }
        };
        _self.triggerOutbrainTracking = function () {
            obApi('track', 'Used Car Leads');
        }
        _self.triggerTrovitTracking = function () {
            if (typeof ta !== 'undefined') {
                ta('send', 'lead');
            }
        }
        _self.triggerAdWordTracking = function () {
            //this tracking is for tag "MSite Search and Details Page Conversion for Google Adwords" in tag manager
            if (typeof dataLayer !== 'undefined') {
                dataLayer.push({ event: 'MSite_UsedCarLeads'});
            }
        }
        _self.bindRecommenedCarsInSellerDetails = function (response) {
            if (response) {
                $('.suggestion-list').html(response);
                $('.suggestion-box').removeClass('hideImportant').delay('1000').slideDown('slow');
                $('div.moveRecommendation').removeClass('hideImportant').show();
            }
            else {
                $('div.suggestion-box, div.moveRecommendation').hide();
            }
            m_bp_additonalFn.sellerDetailsBtnTextChange();
            $('#cwmLoadingIcon').hide();
        }
        _self.publishRecommendedCarsClick = function (element, callback) {
            var $btnSimilarCars = $(element);
            var similarCarsUrl = $btnSimilarCars.attr('popupurl');
            $.ajax({
                url: similarCarsUrl,
                success: function (response) {
                    callback(response, $btnSimilarCars);
                }

            });
        }
        _self.setOneClickOnRecommendedCars = function () {
            if (!$m_bp_GSDBUTTON.find(".getSimilarCarSellerDetails").hasClass("hideImportant")) {
                $(".getSimilarCarSellerDetails").addClass('hideImportant');
                $(".oneClickDetails").removeClass('hideImportant');
            }
        }
        _self.handleSuccessResponse = function (response, exotelObj, $node, stock) {
            try {
                $node.parent().find("img").hide();
                _self.isForbidden = false;
                _self.isLimitExceeded = false;
                _self.storeTempCookie(exotelObj);
                chatProcess.processChatRegistration(response.appId, response.buyer, chatUIProcess.setChatIconVisibilty, chatProcess.source.mobileBrowser);
                if (leadType.getCurrLeadType().Value === leadType.getWhatsAppLead().Value) {
                    cwTracking.trackCustomData(M_tracking.Tracking.variables.getBhriguCategory(), M_tracking.Tracking.variables.getWhatsAppChatVerifiedText(), 'profileId=' + $node.attr('profileid'), true);
                    whatsAppProcess.startWhatsAppChat(response.seller.mobile, response.whatsAppMessage);
                    $("#cw_loading_icon").addClass('hide');
                }
                else {
                    $("#cw_loading_icon").addClass('hide');     //we are using 'display: flex', so cann't do .hide()
                    if (!m_bp_pdfScreen.ISCERTIFICATION) {
                        if (m_bp_recommendedCars.ISRECOMMENDATION) {
                            var similarCarId = m_bp_recommendedCars.getRankFromRecommendationId($node.attr('id'));
                            $('#suggestDetails-' + similarCarId + ' div').html($('#sellerDetailsDiv').html());
                            m_bp_recommendedCars.toggleViewDetailsBtn($node, similarCarId);
                        }
                        m_bp_detailsScreen.bindSellerDetails(response.seller, $node);
                    }
                    else {
                        if (response && response.certificationReportUrl) {
                            m_bp_pdfScreen.downloadPdf(response.certificationReportUrl);
                            m_bp_pdfScreen.clear();
                        }
                        else {
                            m_bp_process.showPdfDownloadError();
                        }
                    }
                }
                _self.setOneClickOnRecommendedCars();
                m_bp_process.triggerFacebookTracking(stock);
                m_bp_process.triggerOutbrainTracking();
                m_bp_process.triggerAdWordTracking();
                m_bp_process.triggerTrovitTracking();
            }
            catch (e) {
                logError(e.message, e.stack, "M_buyerProcess - m_buyerProcessSelf.process.handleSuccessResponse", location.pathname);
            }
        };

        _self.storeTempCookie = function (exotelObj) {
            var date = new Date();
            date.setTime(date.getTime() + (90 * 24 * 60 * 60 * 1000)); //90 days
            var cookieExpiry = date.toGMTString();
            document.cookie = "TempCurrentUser=" + exotelObj.userName + ":" + exotelObj.userMobileNo + ":" + exotelObj.userEmail + ":0; expires=" + cookieExpiry + "; path=/";
        }

        _self.isNumberChanged = function (newMobileNumber) {
            if (m_bp_getSellerDetails.checkCookieExists('TempCurrentUser')) {
                var currentMobileNumber = m_bp_getSellerDetails.getCookie('TempCurrentUser').split(":")[1];
                return currentMobileNumber != newMobileNumber;
            }
            return false;
        };

        _self.handleErrorResponse = function (xhr, $node, buyerMobile) {
            $("#cw_loading_icon").addClass('hide');                    //we are using 'display: flex', so cann't do .hide() 
            var response = JSON.parse(xhr.responseText);
            $node.parent().find("img").hide();
            switch (parseInt(xhr.status)) {
                case 403:
                    if (_self.isMobileUnverifiedResponse(response)) {
                        if (!$('#buyerForm').is(':visible')) {
                            _self.showInputScreenAfterError();
                        }
                        else {
                            _self.hitMobileVerificationApi(buyerMobile, _self.sendOtpApiHandler);
                        }
                    }
                    else {
                        m_bp_additonalFn.showErrorMessage("Error!", response.Message);
                        _self.showInputScreenAfterError();
                        _self.isForbidden = true;
                        _self.isLimitExceeded = true;
                    }
                    break;
                default:
                    if (!_self.isMobileMismatchedResponse(response)) {
                        m_bp_additonalFn.showErrorMessage("Error!", response.Message);
                    }
                    else {
                        $.cookie("TempCurrentUser", null, { path: '/' });         //in case of mis matched numer removing tempcurrcookie 
                    }
                    m_bp_additonalFn.hideLoading();
                    m_bp_process.showInputScreenAfterError();
                    break;
            }
        };

        _self.isMobileUnverifiedResponse = function (response) {
            return response.ModelState && response.ModelState.hasOwnProperty("MobileUnverified");
        };

        _self.isMobileMismatchedResponse = function (response) {
            return response.ModelState && response.ModelState.hasOwnProperty("MobileMismatched");
        };

        _self.checkOtpValidity = function (otp) {
            var user = m_bp_process.getUserDetails();
            otpVerification.verifyOtp(user.mobileNo, otp, 43).done(function (json) {
                if (json.responseCode == 1) {
                    m_bp_process.handleVerificationSuccessResponse();
                }
                else {
                    m_bp_screen1.otpForm.find('.otp__error').text('Invalid OTP!');
                }
            }).fail(function () {
                m_bp_screen1.otpForm.find('.otp__error').text('Something went wrong!');
            });
        };
        _self.hitMobileVerificationApi = function (mobileNumber, responseHandler) {
            otpVerification.sendOtp(mobileNumber, 43).done(function (json) {
                if (responseHandler) {
                    responseHandler(json);
                }
            });
        };
        _self.hitIsMobileVerifiedApi = function (mobileNumber, responseHandler) {
            otpVerification.verifyMobile(mobileNumber, 43).done(function (json) {
                if (responseHandler) {
                    responseHandler(json);
                }
            });
        }
        _self.sendOtpApiHandler = function (json) {
            $("#imgLoadingBtnSend").hide();
            $("#pdfimgLoadingBtnSend").hide();
            if (json != null && json.isOtpGenerated) {
                $("#missed-call-number").attr("href", "tel:" + json.tollFreeNumber).html('<span class="tel-icon"></span>' + json.tollFreeNumber);
                $(".missed-call__info-text").show();
                $(".missed-call__error-msg").hide();
                $(".certificationPdfScreen").hide();
                $('#getSellerDetailsForm').hide();
                $('#getOTP').val("");
                m_bp_screen1.otpForm.find('.otp__error').text('');
                m_bp_screen1.resetTimer($('#otpTimer'), m_bp_screen1.clearTimerTimeout);
                m_bp_screen1.otpForm.show();
                m_bp_screen1.clearTimerTimeout = m_bp_screen1.setTimer($('#otpTimer'), 'Resend OTP', 30);
            }
        };
        _self.missedCallClickVeriHandler = function (veriResponse) {
            $("#missed-call__loading").hide();
            if (veriResponse != null && veriResponse.isMobileVerified) {
                m_bp_process.handleVerificationSuccessResponse();
            }
            else {
                $(".missed-call__info-text").hide();
                $(".missed-call__error-msg").show();
            }
        }
        _self.missedCallPollingVeriHandler = function (veriResponse) {
            if (veriResponse != null && veriResponse.isMobileVerified) {
                m_bp_process.handleVerificationSuccessResponse();
            }
        }
    };

    m_buyerProcessSelf.additonalFn = function () {
        var _self = this;
        _self.isLocalStorage = (window.localStorage) ? true : false;
        _self.rootResponsivekey = "userPreferredRootId";
        _self.registerEvents = function () {
            m_bp_DOC.on('click', '#btnErrorOk', function () {
                _self.closeErrorPopup();
            });
            if (navigator.userAgent.match(/(iPod|iPhone|iPad)/)) {
                $("#seller-contact-no").attr('target', '_blank');
            }
        };

        _self.closeErrorPopup = function () {
            $("#divOverlay").hide();
            $("div.popup").addClass("hide");
            $("#verfiyError").hide();
            $(".imgLoadingSPage").hide();
        }

        _self.scrollPrevForm = function (offset, windHt, windHtp, scrollBottom, scrollTop) {
            if (offset > windHt)
                $('html, body').animate({ scrollTop: offset - (($(window).height() * scrollBottom)) }, 1000);
            else if (offset < windHtp)
                $('html, body').animate({ scrollTop: offset - (($(window).height() * scrollTop)) }, 1000);
        };
        _self.showLoading = function (element) {  // Function to Show Loading Icon 
            element.after(LOADINGHTML);
            element.next("#img-loader").show();
        };
        _self.hideLoading = function () { // Function to Hide Loading Icon 
            $m_bp_GSDBUTTON.parents().eq(3).find("#img-loader").hide().remove();
        };
        _self.showErrorMessage = function (errorHeadingMsg, message)  // function for showing error 
        {
            try {
                $("#verfiyError").show();
                $("#errorHeading").text(errorHeadingMsg);
                $("#verifyErrorMssg").html(message);
                $("#imgLoadingBtnVerify").hide();
                $("#imgLoadingBtnSend").hide();
            }
            catch (e) {
                logError(e.message, e.stack, "M_buyerProcess - m_buyerProcessSelf.additonalFn.showErrorMessage", location.pathname);
            }
        };
        _self.sellerDetailsBtnTextChange = function () {
            try {
                if (m_bp_getSellerDetails.getCookie('TempCurrentUser') != null && m_bp_getSellerDetails.getCookie('TempCurrentUser').length > 0) {
                    $(".getSimilarCarSellerDetails").addClass('hideImportant');
                    $(".oneClickDetails").removeClass('hideImportant');
                }
            }
            catch (e) {
                logError(e.message, e.stack, "M_buyerProcess - m_buyerProcessSelf.additonalFn.sellerDetailsBtnTextChange", location.pathname);
            }
        };
        _self.trackAction = function (actionEvent, actionCat, actionAct) {
            try {
                dataLayer.push({ event: actionEvent, cat: actionCat, act: actionAct });
            }
            catch (e) {
                logError(e.message, e.stack, "M_buyerProcess - m_buyerProcessSelf.additonalFn.trackAction", location.pathname);
            }
        };
        _self.addToLocalStorage = function (key, value) {
            try {
                if (_self.isLocalStorage) {
                    window.localStorage.setItem(key, value);
                }
            } catch (e) {
                logError(e.message, e.stack, "M_buyerProcess - m_buyerProcessSelf.addToLocalStorage", location.pathname);
            }
        }
        _self.getFromLocalStorage = function (key) {
            try {
                if (_self.isLocalStorage) {
                    return window.localStorage.getItem(key);
                }
            } catch (e) {
                logError(e.message, e.stack, "M_buyerProcess - m_buyerProcessSelf.addToLocalStorage", location.pathname);
            }
        }
    };

    m_buyerProcessSelf.detailsScreen = function () {
        var _self = this;
        _self.bindSellerDetails = function (seller, node) {
            try {
                var firstNo = seller.mobile.split(",")[0];
                var clickableMobileHTML = _self.getSeparateClickableNo(seller.mobile);
                if (m_bp_recommendedCars.ISRECOMMENDATION) {
                    var similarCarId = m_bp_recommendedCars.getRankFromRecommendationId(node.attr('id'));
                    if ($('#suggetions').is(":visible"))
                        $('#popup-suggetions').empty();
                    else
                        $('#suggetions').empty();
                    $('#suggestDetails-' + similarCarId + ' .recommendloadIcon').hide();
                    $('#suggestDetails-' + similarCarId + ' .sellerDetails .seller-Name').text(seller.name);
                    $('#suggestDetails-' + similarCarId + ' .sellerDetails .seller-Person').text(seller.contactPerson ? seller.contactPerson : "");
                    $('#suggestDetails-' + similarCarId + ' .sellerDetails .seller-Email').attr("href", "mailto:" + seller.email).text(seller.email);
                    $('#suggestDetails-' + similarCarId + ' .sellerDetails .seller-Contact').html(clickableMobileHTML);
                    $('#suggestDetails-' + similarCarId + ' .sellerDetails .seller-Address').text(seller.address);
                    if (!$('#suggetions').is(":visible"))
                        $('#buyerForm').hide('slide', { direction: 'down' }, 500);
                    $('#suggestDetails-' + similarCarId).show();
                    if (!($('.screen3').is(":visible")))
                        $('#m-blackOut-window').hide();
                }
                else {
                    $('.screen3').find(".seller-Name").text(seller.name);
                    $('.screen3').find(".seller-Person").text(seller.contactPerson ? seller.contactPerson : "");
                    $('.screen3').find(".seller-Email").attr("href", "mailto:" + seller.email).text(seller.email);
                    $('.screen3').find(".seller-Contact").html(clickableMobileHTML);
                    $('.screen3').find(".seller-Address").text(seller.address);
                    if (seller.ratingText) {
                        $('.screen3').find(".top-rated-seller-tag").text(seller.ratingText).show();
                    }
                    else {
                        $('.screen3').find(".top-rated-seller-tag").hide();
                    }
                }

                $("#contactNo").text(firstNo);
                firstNo = "tel:" + '+91' + firstNo;
                $("#seller-contact-no").attr("href", firstNo);
                //hiding the loading icon.
                $(".recommendloadIcon").hide();
                //Removing the text-white class
                $('.sellerDetails').removeClass('text-white');
            }
            catch (e) {
                logError(e.message, e.stack, "M_buyerProcess - m_buyerProcessSelf.detailsScreen.bindSellerDetails", location.pathname);
            }
        };

        _self.getCityId = function () {
            var cityId = commonUtilities.getFilterFromQS("city", window.location.search); //search page: this would fetch city if lead generated from search page
            if (!cityId) {
                if (typeof (_cityId) !== 'undefined') {//details page: this would fetch city if lead generated from details page
                    return _cityId;
                }
            }
            return cityId;
        }

        _self.getSeparateClickableNo = function (number) {
            return number.replace(/\d{10}/g, ' <a href="tel:+91$&">$&</a>');
        };

        _self.showScreen = function () {
            try {
                $('.screen1').hide();
                if ($('#recommendedCarsPopup').is(':visible')) {
                    $('.suggestion-box,.moveRecommendation').addClass('hideImportant');
                }
                $(".imgLoadingSPage").addClass('hide');
                $('#buyerForm').show();
                $('#buyerForm .screen3').slideDown(1000);
                $("#m-blackOut-window").show();
                Common.utils.lockPopup();
            }
            catch (e) {
                logError(e.message, e.stack, "M_buyerProcess - m_buyerProcessSelf.detailsScreen.showScreen", location.pathname);
            }

        };
    };

    m_buyerProcessSelf.pdfScreen = function () {
        var _self = this;
        _self.ISCERTIFICATION = false;
        _self.registerEvents = function () {
            m_bp_DOC.on('click', '#pdfbtnSend', function () {
                _self.ISCERTIFICATION = true;
                $("#pdfimgLoadingBtnSend").show();

                var isValid = m_bp_screen1.validateForm($('#pdftxtUserName'), $('#pdftxtMobileNo'));

                if (isValid) {
                    m_bp_process.submitRequestForInquiry(m_bp_process.getStockDetails($m_bp_GSDBUTTON), $m_bp_GSDBUTTON);
                }
                else {
                    $("#pdfimgLoadingBtnSend").hide();
                }
            });
        };

        _self.showScreen = function () {
            $('#m-blackOut-window').show();
            $('#buyerForm').show();
            $('#buyerForm .certificationPdfScreen').slideDown();
            $("#imgLoading").hide();
            $("#pdfimgLoadingBtnSend").hide();
        };

        _self.downloadPdf = function (Url) {
            //for browser compatibility,
            //setting location.href after timeout, instead of directly setting it.
            setTimeout(function () {
                location.href = Url;
            }, 0);
        };

        _self.clear = function () {
            $("#pdfimgLoadingBtnSend").hide();
            m_bp_pdfScreen.ISCERTIFICATION = false;
            history.back();
        }
    }

    m_buyerProcessSelf.screen1 = function () {
        var _self = this;
        _self.otpForm = $('#otpForm');
        _self.clearTimerTimeout = '';

        _self.registerEvents = function () {

            m_bp_DOC.on('click', "#btnSend", function () {     // ArrowButton Click
                $("#imgLoadingBtnSend").show();

                var isValid = _self.validateForm($('#txtUserName'), $('#txtMobileNo'));

                if (isValid) {
                    m_bp_process.submitRequestForInquiry(m_bp_process.getStockDetails($m_bp_GSDBUTTON), $m_bp_GSDBUTTON); // need to pass $m_bp_GSDBUTTON parameter to track conversion for profileId

                }
                else {
                    $("#imgLoadingBtnSend").hide();
                }

            });
            m_bp_DOC.on('click', "#changeCityDetailsPage", function () {
                $("div.global-location").trigger('click');
                setTimeout(function () {
                    window.history.pushState("areapopup", null, null);
                }, 200);
            });
            m_bp_DOC.on('click', 'div.screen1 span.changeCityLocation', function () {
                _self.changeCityFromBuyerForm();

            });
            m_bp_DOC.on('click', '#gl-close, #globalPopupBlackOut, #btnConfirmCity', function () {
                $('div.detail-ui-corner-top').css('z-index', '9');
                _self.closeGLCity();
            });
            m_bp_DOC.on('click', '#verifyOTP', function () {
                var otp = $('#getOTP').val();
                if (otp.length == 0) {
                    _self.otpForm.find('.otp__error').text('Please enter OTP');
                }
                else if (otp.length < 5) {
                    _self.otpForm.find('.otp__error').text('Invalid OTP!');
                }
                else {
                    m_bp_process.checkOtpValidity(otp);
                }
            });
            m_bp_DOC.on('keyup', '#getOTP', function () {
                var otp = this.value;
                $(this).val(otp.replace(/[^\d]/, ''));
                if (otp.length == 5) {
                    m_bp_process.checkOtpValidity(otp);
                }
            });
            m_bp_DOC.on('click', '#otpTimer', function () {
                var user = m_bp_process.getUserDetails();
                otpVerification.resendOtp(user.mobileNo, 43).done(function () {
                    $('#otpTimer').addClass('otp-status--done').html('OTP sent to Mobile');
                }).fail(function () {
                    _self.otpForm.find('.otp__error').text('Something went wrong!');
                });
            });
            m_bp_DOC.on('click', ".missed-call__verify-link", function (e) {
                $("#missed-call__loading").show();
                var user = m_bp_process.getUserDetails();
                m_bp_process.hitIsMobileVerifiedApi(user.mobileNo, m_bp_process.missedCallClickVeriHandler);
            });
            m_bp_DOC.one('click', "#missed-call-number", function (e) {
                commonUtilities.executeTimely(function () {
                    if ($("#missed-call-number").is(":visible")) {
                        var user = m_bp_process.getUserDetails();
                        m_bp_process.hitIsMobileVerifiedApi(user.mobileNo, m_bp_process.missedCallPollingVeriHandler);
                    }
                    else {
                        return true;
                    }
                }, 5000, 10000, 10);
            });
        };
        _self.resetTimer = function (container, timeoutElement) {
            clearTimeout(timeoutElement);
            container.removeClass('counter--active counter-done otp-status--done');
            container.html('Resend OTP in <span class="time-counter">30</span>s');
        };

        _self.setTimer = function (container, successMessage, count) {
            container.addClass('counter--active');
            count -= 1;

            var clearCounter = setInterval(function () {
                if (!count) {
                    container.removeClass('counter--active').text(successMessage);
                    clearTimeout(clearCounter);
                }
                else {
                    container.find('.time-counter').text(count);
                    count -= 1;
                }
            }, 1000);

            return clearCounter;
        };

        _self.closeGLCity = function () {
            $('#m-blackOut-window').removeClass('changeCityWindow');
            $('#buyerForm').removeClass('changeCityPosition');
            $('#carDetails').removeClass('cardetailsPosition');
            $('#globalPopupBlackOut').hide();
            unlockPopup();
        };

        _self.changeCityFromBuyerForm = function () {
            window.history.pushState('globalLocation', 'globalLocation', window.location.search);
            $('div.detail-ui-corner-top').css('z-index', '7');
            $("div.global-location").trigger('click');
        };

        _self.validateForm = function (nameField, mobileField) {
            var errorHeading = "",
                errorMessage = "";

            var validateName = _self.validateNameField(nameField)
            if (!validateName.isValid) {
                errorHeading = "Invalid Name";
                errorMessage += '<span class="message-item">' + validateName.message + '</span>';
            }

            var isMobileValid = _self.validateInputField(mobileField, 10);
            if (!isMobileValid) {
                errorHeading = "Invalid Number";
                errorMessage += '<span class="message-item">Enter valid 10 digit mobile number.</span>'
            }

            if (!validateName.isValid && !isMobileValid) {
                errorHeading = "Invalid Details";
            }

            if (!validateName.isValid || !isMobileValid) {
                m_bp_additonalFn.showErrorMessage(errorHeading, errorMessage);
            }

            return validateName.isValid && isMobileValid
        };

        _self.validateNameField = function (field) {
            try {
                var isValid = false,
                    message = '';

                var fieldVal = field.val(),
                    reTest = /^[a-zA-Z_'. ]{3,50}$/,
                    fieldLength = fieldVal.length;

                if (!fieldLength) {
                    message = "Please enter your name."
                }
                else if (fieldLength < 3) {
                    message = "Name should be atleast 3 characters."
                }
                else if (!reTest.test(fieldVal)) {
                    message = "Invalid name."
                }
                else {
                    isValid = true;
                }

                return {
                    isValid: isValid,
                    message: message
                };
            }
            catch (e) {

            }
        };

        _self.validateInputField = function (field, limit) {
            try {
                var fieldVal, fieldLength, reTest;
                reTest = /^[0-9]*$/;
                fieldVal = field.val()
                fieldLength = fieldVal.length;
                if (!(isNaN(fieldVal)) && fieldLength === limit && reTest.test(fieldVal))
                    return true;
                else
                    return false;
            }
            catch (e) {
                logError(e.message, e.stack, "M_buyerProcess - m_buyerProcessSelf.screen1.validateInputField", location.pathname);
            }
            return false;
        };

        _self.showScreen = function () {
            var deliveryCity = _self.getFilterFromQSWithURL('deliverycity');
            $('#m-blackOut-window').show();
            $('#getSellerDetailsForm').show();
            $('#buyerForm').show();
            $('#buyerForm .screen1').slideDown();
            $("#imgLoading").hide();
            $(".imgLoadingSPage").addClass('hide');
            $("#imgLoadingBtnSend").hide();
            $('#carLocationText').text($m_bp_GSDBUTTON.attr('cityname'));
            $('#locationWarning, #changeCity').show();
            if ($m_bp_GSDBUTTON.attr('dc') && $m_bp_GSDBUTTON.attr('dc') != "0" && $m_bp_GSDBUTTON.attr('dctext')) {
                _self.showDeliveryText($m_bp_GSDBUTTON.attr('dctext'));
            }
            if ($m_bp_GSDBUTTON.attr("oid") === "11") {
                $("#cityWarningGSD").hide();
            }
            else {
                $("#cityWarningGSD").show();
            }
            if (deliveryCity) {
                $('#changeCity,#locationWarning').hide();
                $('#deliveryText').show().text('Delivery available in ' + deliveryCity.replace('%20', ' '));
            }
        };

        _self.getFilterFromQSWithURL = function (name) {
            var qs = location.search.replace('?', '');
            var params = qs.split('&');
            var result = {};
            var propval, filterName, value;
            var isFound = false;
            for (var i = 0; i < params.length; i++) {
                var propval = params[i].split('=');
                filterName = propval[0];
                if (filterName == name) {
                    value = propval[1];
                    isFound = true;
                    break;
                }
            };
            if (isFound && value.length > 0) {
                if (value.indexOf('+') > 0)
                    return value.replace(/\+/g, " ");
                else
                    return value;
            }
            else
                return "";
        }
        _self.hideScreen = function () {
            $('#buyerForm .screen1').slideUp();
        };
        _self.showDeliveryText = function (deliveryText) {
            var deliveryTextElement = $('#deliveryText');
            deliveryTextElement.text(deliveryText);
            deliveryTextElement.show();
            $('#locationWarning, #changeCity').hide();
            $("#cityWarningGSD").show();
        };
    };

    m_buyerProcessSelf.pageLoad = function () {
        try {
            m_bp_getSellerDetails.registerEvents();
            m_bp_screen1.registerEvents();
            m_bp_additonalFn.registerEvents();
            m_bp_recommendedCars.registerEvents();
            m_bp_pdfScreen.registerEvents();
        }
        catch (e) {
            logError(e.message, e.stack, "M_buyerProcess - m_buyerProcessSelf.pageLoad", location.pathname);
        }
    };

    m_buyerProcessSelf.recommendedCars = function () {
        var _self = this; _self.LISTING = ko.observableArray([]); _self.ISRECOMMENDATION;
        _self.registerEvents = function () {
            m_bp_DOC.on('click', '.similarCarsViewDetails', function (e) {
                _self.ISRECOMMENDATION = true;
                tracker.setSlotId();
                leadType.setCurrLeadType(leadType.getSellerDetailLead().Value);
                var recommendedGsdBtn = $(this);
                $m_bp_GSDBUTTON = recommendedGsdBtn;
                var recommendedCarRank = _self.getRankFromRecommendationId(recommendedGsdBtn.attr('id'));
                //To avoid flicker on 2nd click on vsd for same car. first click remove hide which is never applied back.
                _self.hideClasses(recommendedGsdBtn, recommendedCarRank);
                _self.getSellerDetailsOrToggle(recommendedGsdBtn, recommendedCarRank);
            });

            m_bp_DOC.on('click', '.suggestmaskingNo', function (e) {
                if (!m_bp_REGEX.test(location.href))
                    dataLayer.push({ event: 'UsedRecommendations', Cat: 'MSite_UsedRecommendations', Act: 'Mite_UsedRecoResponses', Label: 'SearchPage-MaskingNoClick-' + $(this).parents('li:first').attr('id') });
                else
                    dataLayer.push({ event: 'UsedRecommendations', Cat: 'MSite_UsedRecommendations', Act: 'MSite_UsedRecoResponses', Label: 'DetailsPage-MaskingNoClick-' + $(this).parents('li:first').attr('id') });
            });
        };
        _self.getSellerDetailsOrToggle = function (recommendedGsdBtn, recommendedCarRank) {
            if (recommendedGsdBtn.find(".hideSellerDetails").hasClass("hideImportant")) {
                $('#preloadingBox-' + recommendedCarRank).show();
                m_bp_getSellerDetails.gsdClick(recommendedGsdBtn, leadType.getSellerDetailLead().Name);
            }
            else {
                _self.toggleViewDetailsBtn(recommendedGsdBtn, recommendedCarRank);
                _self.hideClasses(recommendedGsdBtn, recommendedCarRank);
            }
        };
        _self.toggleHideBtnOrShowError = function (recommendedGsdBtn, recommendedCarRank) {
            if (!recommendedGsdBtn.find('.hideSellerDetails').hasClass("hideImportant")) {
                _self.toggleViewDetailsBtn(recommendedGsdBtn, recommendedCarRank);
            }
            else {
                _self.showErrors();
            }
        };
        _self.showErrors = function () {
            if (m_bp_process.isForbidden) {
                m_bp_process.showAccessForbiddenError();
            }
            else {
                m_bp_process.showLimitExceededError();
            }
        };
        _self.hideClasses = function (recommendedGsdBtn, recommendedCarRank) {
            $('#preloadingBox-' + recommendedCarRank).hide();
            $('#sellerdetailsData-' + recommendedCarRank).hide();
        };
        _self.toggleViewDetailsBtn = function (recommendedGsdBtn, recommendedCarRank) {
            $('#buyerForm').animate({ scrollTop: recommendedGsdBtn.offset().top - $('.screen3').offset().top });
            $('#suggestDetails-' + recommendedCarRank).slideToggle();
            _self.changeGSDBtnText(recommendedGsdBtn);
            _self.changeGSDBtnColor(recommendedGsdBtn);
        };
        _self.changeGSDBtnText = function (recommendedGsdBtn) {
            if (recommendedGsdBtn.find(".oneClickDetails").hasClass("hideImportant")) {
                recommendedGsdBtn.find(".oneClickDetails").removeClass("hideImportant");
                recommendedGsdBtn.find(".hideSellerDetails").addClass("hideImportant");
            }
            else {
                recommendedGsdBtn.find(".oneClickDetails").addClass("hideImportant");
                recommendedGsdBtn.find(".hideSellerDetails").removeClass("hideImportant");
            }

        };
        _self.changeGSDBtnColor = function (recommendedGsdBtn) {
            recommendedGsdBtn.toggleClass('expand add-grey').find('span.fa-angle-down').toggleClass('transform');
        };
        _self.getRankFromRecommendationId = function (nodeId) {
            //return numeric part in Id present after "-"
            return nodeId.substring(nodeId.lastIndexOf("-") + 1);
        };
    };

};

var leadType = (function () {
    var _currLeadType;
    var _leadType = [
        { Name: "GetSellerDetailLead", Value: 0 },
        { Name: "ChatLead", Value: 1 },
        { Name: "WhatsAppLead", Value: 2 },
        { Name: "CertificationReportLead", Value: 3 },
    ]
    var getCurrLeadType = function () {
        return _currLeadType;
    }
    var setCurrLeadType = function (leadTypeValue) {
        _currLeadType = leadTypeValue > -1 && leadTypeValue < _leadType.length ? _leadType[leadTypeValue] : undefined;
        chatProcess.isChatLead = leadTypeValue === getChatLead().Value;     //remove this when we reomve chatProcess.js
    }
    var getSellerDetailLead = function () {
        return _leadType[0];
    }
    var getChatLead = function () {
        return _leadType[1];
    }
    var getWhatsAppLead = function () {
        return _leadType[2];
    }
    var getCertificationReportLead = function () {
        return _leadType[3];
    }
    return {
        getSellerDetailLead: getSellerDetailLead,
        getChatLead: getChatLead,
        getWhatsAppLead: getWhatsAppLead,
        setCurrLeadType: setCurrLeadType,
        getCurrLeadType: getCurrLeadType,
        getCertificationReportLead: getCertificationReportLead,
    }
})();

var M_buyerProcess_Utils = (function () {
    var removeParameterFromQS = function (paramName) {
        var qs = window.location.search.slice(1);
        var paramObj = {};
        paramObj[paramName] = true;
        var currQs = commonUtilities.removeFilterFromQS(paramObj, qs);
        if (currQs) {
            currQs = "?" + currQs;
        }
        var currURL = window.location.pathname + currQs;
        history.replaceState(currURL, "", currURL);
    }

    return {
        removeParameterFromQS: removeParameterFromQS
    }
})();

var M_tracking = {
    Tracking: {
        variables: {
            getBhriguCategory: function () {
                return $('#carDetails, div.detail-ui-corner-top').is(':visible') ? "UsedDetailsPage" :
                    $('#body').length <= 0 ? "UsedPhotoGalleryPage" : "UsedSearchPage";
            },
            getChatUnVerifiedText: function () { return "Chat_Unverified_Click" },
            getChatVerifiedText: function () { return "Chat_Verified_Click" },
            getWhatsAppChatUnVerifiedText: function () { return "Whatsapp_Unverified_Click" },
            getWhatsAppChatVerifiedText: function () { return "Whatsapp_Verified_Click" }
        },
        listingsTrackingCategoryEnum: {
            Impression: 1,
            DetailView: 2,
            PhotoView: 3,
            BtnSellerView: 4,
            Response: 5,
            SimilarCarsView: 6,
        },
        action: "",
        prepareLabel: function (trackingparam) {
            var label = '';
            if (trackingparam && typeof (trackingparam) == 'object') {
                for (var property in trackingparam) {
                    label = label + property + '=' + trackingparam[property] + '|';
                }
            }
            return label;
        },
        triggerTracking: function (trackingparam, act, isTrackQs) {
            var label = M_tracking.Tracking.prepareLabel(trackingparam);
            if (label && cwTracking) {
                //cwTracking.trackCustomData('UsedCars', act, label, isTrackQs);
            }
        },
        performTrackingOperations: function (node, cid, recommendedCount, buyerMobile) {
            var trackingParam = {};
            trackingParam = M_tracking.Tracking.getTrackingParameters(node, cid);
            trackingParam['cntRm'] = (recommendedCount >= 0) ? recommendedCount : -1;
            trackingParam['buyerMobile'] = buyerMobile ? buyerMobile : null;
            M_tracking.Tracking.triggerTracking(trackingParam, M_tracking.Tracking.action, true);
        },
        clickTracking: function (node, cid) {
            $(document).on("click", node, function (event) {
                M_tracking.Tracking.performTrackingOperations(this, cid);
            });
        },
        detailviewTracking: function () {
            var cid = M_tracking.Tracking.listingsTrackingCategoryEnum.DetailView;
            M_tracking.Tracking.clickTracking(".usedsearch-img-holder", cid);
            M_tracking.Tracking.clickTracking(".usedsearch-car-item #listingTxt", cid);
            M_tracking.Tracking.clickTracking(".recommendedList .imgholder,.recommendedList .carDetails", cid);
            M_tracking.Tracking.clickTracking(".similar-cars a", cid);
            M_tracking.Tracking.clickTracking(".carDescWrapper", cid);
            M_tracking.Tracking.clickTracking(".popup-recommendedList .imgholder,.popup-recommendedList .carDetails", cid);
            M_tracking.Tracking.clickTracking('.detailsPageSimilarCars a,.detailsPageSimilarCars .carDescWrapper', cid);
        },
        selletBtnClickTracking: function () {
            M_tracking.Tracking.clickTracking('.getSellerDetails', M_tracking.Tracking.listingsTrackingCategoryEnum.BtnSellerView);
            M_tracking.Tracking.clickTracking(".buttonMain a", M_tracking.Tracking.listingsTrackingCategoryEnum.BtnSellerView);
        },
        bindClickTracking: function () {
            M_tracking.Tracking.detailviewTracking();
            M_tracking.Tracking.selletBtnClickTracking();
            M_tracking.Tracking.clickTracking("#carDetailsImg", M_tracking.Tracking.listingsTrackingCategoryEnum.PhotoView);
            M_tracking.Tracking.clickTracking('.recommendedCarsPopupBtn', M_tracking.Tracking.listingsTrackingCategoryEnum.SimilarCarsView);
        },
        removeUndefinedParameters: function (trackingParam) {
            for (var property in trackingParam) {
                if (typeof trackingParam[property] == 'undefined')
                    delete trackingParam[property];
            }
            return trackingParam;
        },
        getTrackingParameters: function (node, cid) {
            var trackingParam = {};
            var currentNodeId = $(node).attr('id');
            trackingParam['cid'] = cid;
            var currentListing = $(node).parents('li');
            trackingParam['pid'] = currentListing.attr('profileid');
            trackingParam['rkAbs'] = currentListing.attr('rankAbs');
            trackingParam['scid'] = currentListing.attr('ispremium') == 'true' ? 1 : 0;
            trackingParam['srcid'] = 1;
            trackingParam['pf'] = $.listingsTrackingPlatform;
            M_tracking.Tracking.action = currentListing.attr('data-action-page');

            // to handle cases from details popup and details page,photogallery
            if (currentNodeId == 'getsellerDetails' || currentNodeId == 'oc_getSellerDetails' || currentNodeId == 'carDetailsImg') {
                currentNodeId = $("#carDetailsImg");
                trackingParam['pid'] = $(currentNodeId).attr('profileid');
                trackingParam['rkAbs'] = $(currentNodeId).attr('rankAbs');
                trackingParam['scid'] = $(currentNodeId).attr('ispremium');
            }
            if (typeof trackingParam['pid'] == 'undefined')
                trackingParam['pid'] = $(node).attr('profileid');
            if (typeof ipDetectedCityId != 'undefined')
                trackingParam['ipdc'] = ipDetectedCityId;
            if (!M_tracking.Tracking.action)
                M_tracking.Tracking.action = $(node).attr('data-action-page');
            //-------------------------
            return M_tracking.Tracking.removeUndefinedParameters(trackingParam);
        }
    }
}


var topRatedSelllerToolTip = (function () {

    function _positionTooltip($dealerRatingLnk) {
        $('.seller-rating__toast').hide();
        var $sellerToastMessage = $dealerRatingLnk.siblings('.seller-rating__toast');
        if ($sellerToastMessage) {
            var viewPortWidth = $(window).outerWidth();
            var dealerRatingOffsetTop = $dealerRatingLnk.offset().top;
            var midPosDealerRatingLnk = ($dealerRatingLnk.width() / 2) + $dealerRatingLnk.offset().left;
            var setToastMsgHorPos = midPosDealerRatingLnk - ($sellerToastMessage.outerWidth() / 2);
            var ToastMsgLeftPos = $dealerRatingLnk.offset().left;
            var isSellerDetails = $dealerRatingLnk.closest('.showToastMessage').length != 0
            var isCarDetails = $dealerRatingLnk.closest('.top-rated-car-detail').length != 0
            var setToastMsgRelPos;
            var diff;
            var setToastLeftPos;
            if (isSellerDetails) {
                var parentOffset = $dealerRatingLnk.closest('.sellerDetails').offset().top;
                var childOffset = $dealerRatingLnk.offset().top;
                diff = childOffset - parentOffset;
            }
            else {
                diff = dealerRatingOffsetTop - $(window).scrollTop() - $('.js-fix-tab-nav').outerHeight();
            }
            if (isCarDetails) {
                setToastLeftPos = ToastMsgLeftPos;
            }
            else {
                var sellerToastMessagePosRt = setToastMsgHorPos + $sellerToastMessage.outerWidth();
                var resetLeftPos = sellerToastMessagePosRt - viewPortWidth + 10;
                setToastLeftPos = viewPortWidth > sellerToastMessagePosRt ? setToastMsgHorPos : (setToastMsgHorPos - resetLeftPos);
            }

            if (diff < $sellerToastMessage.outerHeight()) {
                $sellerToastMessage.addClass('arrow-bottom');
                setToastMsgRelPos = dealerRatingOffsetTop + $dealerRatingLnk.outerHeight() + 5;
            } else {
                $sellerToastMessage.removeClass('arrow-bottom');
                setToastMsgRelPos = dealerRatingOffsetTop - $sellerToastMessage.outerHeight() - 10;
            }




            $sellerToastMessage.show();
            $sellerToastMessage.offset({ top: setToastMsgRelPos, left: setToastLeftPos });
        }
    }

    function registerEvents() {
        m_bp_DOC.on('click', '.top-rated-seller-tag', function (event) {
            event.stopPropagation();
            _positionTooltip($(this));
        });
        $(window).resize(function () {
            $('.seller-rating__toast').hide();
        });
        m_bp_DOC.on('click', '.seller-rating-toast__close', function (event) {
            event.stopPropagation();
            $(this).closest('.seller-rating__toast').hide();
        });
    }

    return {
        registerEvents: registerEvents
    }
})();

var tooltipPosition = (function () {
    function registerEvents() {
        var listPageCoachMarkContainer, listPageChatBtnContainer, listPageCoachMarkArrow;

        setTimeout(function () {

            listPageChatBtnContainer = $('.chat-btn-container')[0], listPageCoachMarkContainer = $('.seller-btn-detail .details-coachmark');

            if (listPageChatBtnContainer && listPageChatBtnContainer.length != 0) {
                listPageCoachMarkContainer.addClass('listingpage__coachmark-container--position');
            }

        }, 500);
    }
    return {
        registerEvents: registerEvents
    }
})();

var m_buyerProcess = new M_buyerProcess();
var m_bp_process = new m_buyerProcess.process(); var m_bp_getSellerDetails = new m_buyerProcess.getSellerDetails();
var m_bp_detailsScreen = new m_buyerProcess.detailsScreen(); var m_bp_screen1 = new m_buyerProcess.screen1(); var m_bp_additonalFn = new m_buyerProcess.additonalFn(); var m_bp_pdfScreen = new m_buyerProcess.pdfScreen();
var m_bp_DOC = $(document); var msite_ORIGIN_ID = -1; var $m_bp_GSDBUTTON, m_bp_REGEX = new RegExp(".*(d|D|s|S)[0-9]+|(cd_buyerProcess)"), m_bp_TIMER, m_bp_ZIPTIMEOUT; var m_bp_recommendedCars = new m_buyerProcess.recommendedCars();
$(document).ready(function () {
    var m_bp_PageLoad = new m_buyerProcess.pageLoad();
    M_tracking.Tracking.bindClickTracking();
    topRatedSelllerToolTip.registerEvents();
    tooltipPosition.registerEvents();

    chatUIProcess.registerEvents();
    chatProcess.getChatHtml(function (isMyChatsVisible, chatHtml) {
        $("#chatPopup").html(chatHtml);
        if (!isMyChatsVisible) {
            $('.chat-launcher-icon').hide();
        }
    }, chatUIProcess.setChatIconVisibilty, chatProcess.source.mobileBrowser);
    m_bp_DOC.on('click', '.chat-btn', function (event) {
        var $element = $(this);
        leadType.setCurrLeadType(chatCommonProcess.getLeadTypeValue($element));
        $m_bp_GSDBUTTON = $element.parents('.seller-btn-detail, .detail-btn-container').find('a.getSellerDetails, a.similarCarsViewDetails');
        m_bp_recommendedCars.ISRECOMMENDATION = false;
        m_bp_getSellerDetails.commonClickActions();
        m_bp_getSellerDetails.gsdClick($m_bp_GSDBUTTON, leadType.getCurrLeadType().Name);
    });
    $(window).on('popstate', function (e) {
        if ($('#mck-message-cell').is(':visible')) {
            if (history.state === "MyChats") {                                      // if state is 'MyChats' then trigger back button of popup so listing_page of chat will appear
                //jQuery not working here for pg, so used pure javascript 
                document.querySelector('#mck-icon-backward-icon').click();
            }
            else if ($("#mck-loc-box").is(':visible')) {
                $("#mck-loc-box").hide();
                history.pushState(leadType.getChatLead().Name, "", "");
            }
            else if ($('.fancybox-image').is(':visible')) {
                document.querySelector('.fancybox-close').click();
                history.pushState(leadType.getChatLead().Name, "", "");
            }
            else {          // if state is not 'MyChats' then close popup
                $('#sidebox-close').trigger('click');
            }
        }
        else if ($('#globalcity-popup').is(':visible')) {
            m_bp_screen1.closeGLCity();
        }
        else if ($('#verfiyError').is(':visible')) {
            m_bp_additonalFn.closeErrorPopup();
        }
        else if ($('#buyerForm').is(':visible')) {
            m_bp_getSellerDetails.closeFormPopup();
        }
    });
});

var chatCommonProcess = (function () {
    var getLeadTypeValue = function ($element) {
        if ($element && $element.length > 0) {
            return $element.find("#chatIcon").data("chatLeadType");
        }
        return leadType.getChatLead().Value; //by deafault, it will consider as it as chat lead
    }

    return {
        getLeadTypeValue: getLeadTypeValue,
    }
})();

var scrollTopPosition = 0;

var chatUIProcess = (function () {
    var setChatIconVisibilty = function (responseMsg, count) {
        if (responseMsg === chatProcess.chatRegistrationResponse.Success) {
            if (count > 0) {
                $('.chat-icon-lg').addClass('global-chat');
            }
            else if (count === 0) {
                $('.chat-icon-lg').removeClass('global-chat');
            }
        }
        else {
            $('.chat-icon-lg').hide();
        }
    };

    var registerEvents = function () {
        var $doc = $(document);
        $doc.on('click', '.mck-button-launcher', function (e) {
            Common.utils.lockPopup();
            history.pushState("MyChats", "", "");
        });
        $doc.on('click', '#mck-contact-list li', function (e) {
            history.pushState(leadType.getChatLead().Name, "", "");
        });
        $doc.on('click', '#mck-icon-backward', function (e) {
            if (history.state === leadType.getChatLead().Name) {             //if state is 'ChatLead' then do history back as that will handle whether to hide popup or show listing_page of chat 
                history.back();
                $('#m-blackOut-window').hide();
            }
        });
        $doc.on('click', '#sidebox-close', function (e) {
            if (history.state === 'MyChats') {
                history.back();
            }
        });
        $('.mck-button-launcher').on('click', function (e) {
            Common.utils.lockPopup();
        });
        $doc.on("click", ".mck-close-sidebox", function (e) {
            $('#cw_loading_icon').addClass('hide');
            if (history.state !== leadType.getSellerDetailLead().Name) {       // if GSD popup is open than don't hide blackout window
                $('#globalPopupBlackOut, #m-blackOut-window').hide();
                Common.utils.unlockPopup();
            }
            m_bp_getSellerDetails.resetCommonClickActions();
        });
    };

    return {
        setChatIconVisibilty: setChatIconVisibilty,
        registerEvents: registerEvents
    }
})();

$('#mck-attachmenu-box').on('click', function () { //added code for scenario where user is typing i.e. 4 to 5 lines and then click on attach so, attach menu shouldn't overlap with the text
    var element = $(this);
    var containerHeight = element.closest('.mck-textbox-container').outerHeight()
    if (containerHeight > 97) {
        containerHeight = containerHeight + 20
    }
    element.find('#mck-upload-menu-list').css('margin-top', (containerHeight / 2) + 'px')
})

var whatsAppProcess = (function () {
    var _getUrlForWhatsApp = function (mobile, whatsAppMessage) {
        return "https://api.whatsapp.com/send?phone=91" + mobile + "&text=" + (whatsAppMessage || "");
    };
    var startWhatsAppChat = function (mobile, whatsAppMessage) {         //In case of multiple mobile number, first one will be chosen
        var _mobile = (mobile) ? mobile.split(',') : undefined;
        if (_mobile) {
            var _url = _getUrlForWhatsApp(_mobile[0], whatsAppMessage);
            window.open(_url, '_self');
        }
    };

    return {
        startWhatsAppChat: startWhatsAppChat,
    };
})();

var tracker = (function () {
    var _slotId = 0;
    var setSlotId = function (element) {      //if element is empty, null or undefined then setting _slotId value to null
        if (!element) {
            _slotId = 0;
        }
        else {
            _slotId = element.dataset.slotId;
            if (!_slotId) {
                _slotId = commonUtilities.getFilterFromQS("slot", window.location.search) || 0;
            }
        }
    };
    var getSlotId = function () {
        return _slotId;
    };
    var trackGsdClick = function (inputParams) {
        if (typeof cwUsedTracking !== 'undefined') {

            var label = "originId=" + inputParams.originId + "|slotId=" + inputParams.slotId + "|ctePackageId=" + inputParams.ctePackageId;

            var trackingParams = {
                action: inputParams.isNumberVerified ? cwUsedTracking.eventActions.gsdVerifiedText : cwUsedTracking.eventActions.gsdUnverifiedText,
                label: label,
                sendQs: true,
            };
            cwUsedTracking.track(trackingParams);
        }
    };
    return {
        setSlotId: setSlotId,
        getSlotId: getSlotId,
        trackGsdClick: trackGsdClick,
    };
})();