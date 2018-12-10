var originId = 0;
var isFromCaptcha = "0", isGSDClick = "0";
var objDetailsCaptcha = { attrId: "", rank: "", pf: "", isPremium: "" };
var isSimilarFetched = false;
var deliveryCity;

var D_carDetailsPage = {
    doc: $(document),
    variables: {
        getBhriguCategory: function () { return "UsedDetailsPage"},
        getChatUnVerifiedText: function () { return "Chat_Unverified_Click" },
        getChatVerifiedText: function () { return "Chat_Verified_Click"}
    },
    window: $(window),
    RECOMMENDLISTING: ko.observableArray([]),
    isLimitExceeded: false, isForbidden: false, kmNumeric: "", priceNumeric: "", bodyTypeId: "", versionSubSegment: "", makeId: "", rootId: "",
    buyerProcessOriginId: {
        sellerDetailsForm: 3,
        photogallerySellerDetailsForm: 4,
        recommendations: 16,
        photogalleryRecommendations: 17,
        certificationReport: 27,
        RightPrice: 38,
        RightPriceRecommendations: 40
    },
    buyerProcessResponseCode: {
        success: 1,
        invalidUser: 2,
        accessForbidden: 4,
        limitReached: 5,
        ipBlocked: 6,
        certificationReportSuccess: 7,
        certificationReportNotAvailable: 8
    },
    sellerDetailForm: {
        openDetailForm: function () {
            $('#expandForm').delay('1000').slideDown('slow', function () {
                $(this).addClass('overflowVisible');
            });
        },
    },
    windowEventHandler: {
        registerEvents: function () {
            $(document).on('keydown', D_carDetailsPage.windowEventHandler.keydownEventHandler);
            $(window).on('popstate', D_carDetailsPage.windowEventHandler.popstateEventHandler);
        },
        keydownEventHandler: function (e) {
            if (e.keyCode == 27) {
                D_carDetailsPage.Finance.closeFinanceForm();
                D_carDetailsPage.Valuation.closePopup();
                D_carDetailsPage.certificate.closeCertificationBox();
            }
        },
        popstateEventHandler: function (e) {
            if ($('#financeIframe').is(':visible')) {
                D_carDetailsPage.Finance.closeFinanceForm();
            }
            else if (e.originalEvent.state && e.originalEvent.state.title == 'detailsValuation') {
                D_carDetailsPage.Valuation.showValuation();
            }
            else {
                D_carDetailsPage.Valuation.closePopup();
            }
        }
    },
    carouselForKnockout: {
        alternateCarousel: function () {
            var target = 3;
            carousel = $('.jcarousel').jcarousel({
                scroll: 1,
                auto: 0,
                animation: 500,
                initCallback: null, buttonNextHTML: null, buttonPrevHTML: null
            });
            $('.jcarousel-control-prev').on('jcarouselcontrol:active', function () {
                $(this).removeClass('inactive');
            }).on('jcarouselcontrol:inactive', function () {
                $(this).addClass('inactive');
            }).jcarouselControl({
                target: '-=' + _target
            });
            $('.jcarousel-control-next').on('jcarouselcontrol:active', function () {
                $(this).removeClass('inactive');
            }).on('jcarouselcontrol:inactive', function () {
                $(this).addClass('inactive');
            }).jcarouselControl({
                target: '+=' + _target
            });
        },

    },
    leadType: {
        getLeadType: function () {
            return chatProcess.isChatLead ? 1 : 0;
        },
    },
    recommendedCars: {
        isRecommended: false,
        accessForbidenMsg: 'Access Forbidden.', limitExceedMsg: 'Oops! You have reached the maximum limit for viewing inquiry details in a day. You can try again tomorrow.',
        registerEvents: function () {
            D_carDetailsPage.doc.on('click', 'a.view-details', function () {
                D_carDetailsPage.recommendedCars.isRecommended = true;
                chatProcess.isChatLead = false;
                var recommendedGsdBtn = $(this);
                D_carDetailsPage.tracking.slot.set("");
                leadData.setGsdNode(this);
                var recommendedCarRank = D_carDetailsPage.recommendedCars.getRankFromRecommendationsId(recommendedGsdBtn.attr('id'));
                D_carDetailsPage.recommendedCars.setOriginId();
                D_carDetailsPage.recommendedCars.hideRecommendation(recommendedCarRank);
                if (D_carDetailsPage.isForbidden || D_carDetailsPage.isLimitExceeded) {
                    D_carDetailsPage.recommendedCars.processRecommendationsErrors(recommendedCarRank, recommendedGsdBtn);
                }
                else {
                    D_carDetailsPage.recommendedCars.getSellerDetailsOrToggle(recommendedCarRank, recommendedGsdBtn);
                }
                D_carDetailsPage.recommendedCars.trackRecommendationAction(recommendedGsdBtn);
            });
            D_carDetailsPage.doc.on('click', 'div.animateBtn', function () {
                $(this).parent().find('a.view-details').trigger('click');
            });
            D_carDetailsPage.doc.on('click', '.popup-close', function () {
                D_carDetailsPage.recommendedCars.closeRecommendedPopup();
            });
        },
        setOriginId: function () {
            if (D_carDetailsPage.Valuation.valuationPopupObj.is(':visible')) {
                originId = D_carDetailsPage.buyerProcessOriginId.RightPriceRecommendations;
            }
            else if (!$('#photoGallery').is(':visible') || $('#photoGallery').css('visibility') == 'hidden')
                originId = D_carDetailsPage.buyerProcessOriginId.recommendations;
            else
                originId = D_carDetailsPage.buyerProcessOriginId.photogalleryRecommendations;

        },
        trackRecommendationAction: function (recommendedGsdBtn) {
            if (!$('#photoGallery').is(':visible') || $('#photoGallery').css('visibility') == 'hidden')
                Common.utils.trackAction('UsedRecommendations', 'Desk_UsedRecommendations', 'Desk_UsedRecoResponses', 'DetailsPage-FormFill-' + recommendedGsdBtn.parents('li:first').attr('id'));
            else
                Common.utils.trackAction('UsedRecommendations', 'Desk_UsedRecommendations', 'Desk_UsedRecoResponses', 'DetailsPhoto-FormFill-' + recommendedGsdBtn.parents('li:first').attr('id'));
        },
        bindRecommendations: function (jsonData) {
            D_carDetailsPage.RECOMMENDLISTING(jsonData);
            $("#blackOut-recommendation").show();
            $("#recommendCars").show("scale", { percent: 100 }, 400);
            setTimeout(function () { $('div.suggestCarsTxt').fadeIn(); }, 4000);
            $('.noRecommendation').animate({ left: '0' }, 1000).css('float', 'left');
            $(".animateRecommendation").delay('500').show('slide', { direction: 'left' }, 1500);
            $('ul.recommendationList li').first().find('div.btnHolder').append("<div class='TwoCirclesPulser animateBtn'><div class='outerCircle pulser'></div><div class='innerCircle pulser'></div></div>");
            if (!$('#photoGallery').is(':visible') || $('#photoGallery').css('visibility') == 'hidden')
                Common.utils.trackAction('UsedRecommendationsImpressions', 'Desk_UsedRecommendations', 'Desk_UsedRecommendations', 'DetailsPage-' + $('.recommendedList').length);
            else
                Common.utils.trackAction('UsedRecommendationsImpressions', 'Desk_UsedRecommendations', 'Desk_UsedRecommendations', 'DetailsPhoto-' + $('.recommendedList').length);
        },

        toggleViewDetailsBtn: function (recommendedCarRank) {
            var viewSellerDetailsBtn = $("#viewSellerDetailsBtn-" + recommendedCarRank);
            var suggestDetailsData = $("#suggestDetailsData-" + recommendedCarRank);
            suggestDetailsData.slideToggle(500);
            D_carDetailsPage.recommendedCars.changeGSDBtnText(viewSellerDetailsBtn);
            viewSellerDetailsBtn.toggleClass('expand add-grey').find('span.fa-angle-down').toggleClass('transform');
        },
        changeGSDBtnText: function (recommendedGsdBtn) {
            if (recommendedGsdBtn.find(".oneClickDetails").hasClass("hideImportant")) {
                recommendedGsdBtn.find(".oneClickDetails").removeClass("hideImportant");
                recommendedGsdBtn.find(".hideSellerDetails").addClass("hideImportant");
            }
            else {
                recommendedGsdBtn.find(".oneClickDetails").addClass("hideImportant");
                recommendedGsdBtn.find(".hideSellerDetails").removeClass("hideImportant");
            }
        },
        processRecommendationsErrors: function(recommendedCarRank, recommendedGsdBtn) {
            var suggestDetailsData = $("#suggestDetailsData-" + recommendedCarRank);
            var $oneClickDetails = recommendedGsdBtn.find(".oneClickDetails");
            if (D_carDetailsPage.recommendedCars.isRecommended) { // recommendation GSD click
                if (($oneClickDetails.hasClass('hideImportant') && suggestDetailsData.is(':visible')) || (!$oneClickDetails.hasClass("hideImportant") && !suggestDetailsData.is(':visible'))) {
                    D_carDetailsPage.recommendedCars.toggleOrShowError(recommendedCarRank, recommendedGsdBtn);
                }
                else {
                    suggestDetailsData.slideToggle(500);
                    D_carDetailsPage.recommendedCars.toggleOrShowError(recommendedCarRank, recommendedGsdBtn);
                }
                setTimeout(function () { recommendedGsdBtn.prop('disabled', false); }, 500);
            }
            else { // Recommendations chat click
                suggestDetailsData.slideToggle(500);
                if (($oneClickDetails.hasClass("hideImportant") && suggestDetailsData.is(':visible'))) { // if 1-click view details is already clicked, first slide up suggestDetails and then show it again.
                    D_carDetailsPage.recommendedCars.toggleViewDetailsBtn(recommendedCarRank);
                }
                D_carDetailsPage.recommendedCars.showErrors(recommendedGsdBtn);
            }
        },
        toggleOrShowError: function (recommendedCarRank, recommendedGsdBtn) {
            D_carDetailsPage.recommendedCars.toggleViewDetailsBtn(recommendedCarRank);
            D_carDetailsPage.recommendedCars.showErrors(recommendedGsdBtn);
        },
        getSellerDetailsOrToggle: function (recommendedCarRank, recommendedGsdBtn) {
            if (!($("#viewSellerDetailsBtn-" + recommendedCarRank + " .oneClickDetails").hasClass("hideImportant"))) {
                $('#loadingIconRecommendations-' + recommendedCarRank).show();
                D_carDetailsPage.recommendedCars.toggleViewDetailsBtn(recommendedCarRank);
                var recommendedCarTrackingRank = D_carDetailsPage.recommendedCars.getRecommendedCarTrackingRank(recommendedCarRank);
                var gsDetailsPageBtn = $('#getsellerDetails');
                processPurchaseInq("recommendation", recommendedCarTrackingRank, gsDetailsPageBtn.attr('data-platform'), gsDetailsPageBtn.attr('data-ispremium'), recommendedGsdBtn);
                setTimeout(function () { recommendedGsdBtn.prop('disabled', false); }, 500);
            }
            else {
                D_carDetailsPage.recommendedCars.toggleViewDetailsBtn(recommendedCarRank);
            }
        },
        getRecommendedCarTrackingRank: function (recommendedCarRank) {
            return "r" + parseInt(parseInt(recommendedCarRank) + 1);
        },
        showErrors: function (recommendedGsdBtn) {
            if (D_carDetailsPage.isForbidden || D_carDetailsPage.isLimitExceeded)
                D_carDetailsPage.recommendedCars.showErrorsForRecommendations(recommendedGsdBtn, D_carDetailsPage.recommendedCars.limitExceedMsg);
        },
        hideRecommendation: function (recommendedCarRank) {
            $('#loadingIconRecommendations-' + recommendedCarRank).hide();
            $('#sellerdetailsData-' + recommendedCarRank).hide();
        },
        hideSellerDetailsScreen: function () {
            if ($('#photoGallery').is(':visible')) {
                $('#pgseller-details').hide();
            }
            else {
                $('#sellerDetails').hide();
            }
        },
        showSellerDetailsScreen: function () {
            if ($('#photoGallery').is(':visible')) {
                $('#pgseller-details').show();
            }
            else {
                $('#sellerDetails').show();
            }
        },
        closeRecommendedPopup: function () {
            $('#recommendCars, .animateRecommendation').hide();
            $('div.suggestCarsTxt').hide();
            $('.recommedCars-left').css({ 'float': 'none', 'left': '324px' })
            $('#blackOut-recommendation').hide();
        },
        showSellerDetailsForRecommendations: function (ds, recommendedGsdBtn) {
            var recommendedCarRank = D_carDetailsPage.recommendedCars.getRankFromRecommendationsId(recommendedGsdBtn.attr('id'));
            $('#loadingIconRecommendations-' + recommendedCarRank).fadeOut('slow', function () {
                $('#sellerdetailsData-' + recommendedCarRank).show();
            });
            $("#seller-Name" + recommendedCarRank).text(ds.seller.name);
            $("#seller-Person" + recommendedCarRank).text(ds.seller.contactPerson ? ds.seller.contactPerson : "");
            $("#seller-Email" + recommendedCarRank).text(ds.seller.email);
            $("#seller-Contact" + recommendedCarRank).text(ds.seller.mobile);
            $("#seller-Address" + recommendedCarRank).text(ds.seller.address);
        },
        showSellerDetails: function (ds) {
            if (ds.seller.dealerShowroomPage) {
                $('.seller-virtual-link').attr('href', ds.seller.dealerShowroomPage).show();
            }
            else {
                $(".seller-virtual-link").hide();
            }
            $("#sellerNameId").text(ds.seller.name);
            $("#contactPersonId").text(ds.seller.contactPerson ? ds.seller.contactPerson : "");
            $("#sellerMobileId").text(ds.seller.mobile);
            $("#sellerEmailId").text(ds.seller.email);
            $("#sellerAddressId").text(ds.seller.address);
        },
        showErrorsForRecommendations: function (recommendedGsdBtn, message) {
            $("#suggestDetailsData-" + D_carDetailsPage.recommendedCars.getRankFromRecommendationsId(recommendedGsdBtn.attr('id'))).html('<li>' + message + '</li>');
        },
        getRankFromRecommendationsId: function (nodeId) {
            //return numeric part in Id present after "-"
            return nodeId.substring(nodeId.lastIndexOf("-") + 1);
        }
    },
    utils: {
        registerEvents: function () {
            D_carDetailsPage.doc.on('click', '#blackOut-recommendation', function () {
                D_carDetailsPage.utils.closePopup();
            });
            D_carDetailsPage.doc.on('click', '.accordion-toggle', function () {
                $(this).next().slideToggle();
                $(this).find("span.up-down-arrow").toggleClass("change-arrow");
            });
        },
        closePopup: function () {
            if ($('#recommendCars').is(':visible'))
                $('.popup-close').trigger('click');

        },
        redirectToUrl: function (url) {
            if (url) {
                location.href = url;
            }
        },
        redirectInNewTab: function (url) {
            if (url) {
                window.open(url, '_blank');
            }
        },
        isNumberChanged: function (newMobileNumber) {
            if ($.cookie('TempCurrentUser')) {
                var currentMobileNumber = $.cookie('TempCurrentUser').split(':')[1];
                return currentMobileNumber != newMobileNumber;
            }
            return false;
        },
        removeParameterFromQS: function (paramName) {
            var qs = window.location.search.slice(1);
            var paramObj = {};
            paramObj[paramName] = true;
            var currQs = commonUtilities.removeFilterFromQS(paramObj, qs);
            if (currQs) {
                currQs = "?" + currQs;
            }
            var currURL = window.location.pathname + currQs;
            history.replaceState(currURL, "", currURL);
        },
        initializeReadMoreCollapsable: function () {
            var detailsReviewReadMoreCollapse = new ReadMoreCollapse('#disclaimerText', {
                expandText: 'Read More',
                collapseText: 'Collapse',
                ellipsis: false
            });
        },
        otpVariables: {
            sourceModule: 1,                        //For UsedcarLead module
            mobileVerificationByType: {
                otp: 1,
                missedCall: 2,
                otpAndMissedCall: 3
            },
            defaultValidityInMins: 30,
            defaultOtpLength: 5
        }
    },
    registerAllEvents: function () {
        D_carDetailsPage.recommendedCars.registerEvents();
        D_carDetailsPage.utils.registerEvents();
        D_carDetailsPage.cityWarning.registerEvents();
        D_carDetailsPage.similarCars.registerEvents();
        D_carDetailsPage.photoGallery.registerEvents();
        D_carDetailsPage.certificate.registerEvents();
        D_carDetailsPage.buyerProcessInvalidResponse.registerEvents();
        D_carDetailsPage.Finance.registerEvents();
        D_carDetailsPage.Valuation.registerEvents();
        D_carDetailsPage.windowEventHandler.registerEvents();
    },
    pageLoad: function () {
        D_carDetailsPage.registerAllEvents();
        D_carDetailsPage.Valuation.sellerDetails.bindBuyerForm();
        D_carDetailsPage.utils.initializeReadMoreCollapsable();
    },
    cityWarning: {
        registerEvents: function () {
            $(document).on('click', 'a[data-user-action="1"]', function (e) {
                e.preventDefault();
                D_carDetailsPage.cityWarning.setUserActionCookie();
                D_carDetailsPage.cityWarning.redirectToSearchPage(this);
            });
        },
        setUserActionCookie: function () {
            SetCookieInDays("_CustCityUserAction", 1);
        },
        redirectToSearchPage: function (actionElement) {
            var redirectUrl = $(actionElement).attr('href');
            var target = $(actionElement).attr('target');

            if (redirectUrl) {
                if (target)
                    D_carDetailsPage.utils.redirectInNewTab(redirectUrl);
                else
                    D_carDetailsPage.utils.redirectToUrl(redirectUrl);
            }
        }

    },
    tracking: {
        trovitTracking: function () {
            if (typeof ta !== 'undefined') {
                ta('send', 'lead');
            }
        },
        triggerFacebookTracking: function (attrId) {
            switch (attrId) {
                case "pg":
                    fbq('track', 'Lead', { content_name: 'Desktop Details PhotoGallery', content_category: 4 });
                    break;
                case "recommendation":
                    fbq('track', 'Lead', { content_name: 'Desktop Details Recommendations', content_category: 12 });
                    break;
                default:
                    fbq('track', 'Lead', { content_name: 'Desktop Details Page', content_category: 3 });
                    break;
            }
        },
        triggerOutbrainTracking: function () {
            obApi('track', 'Used Car Leads');
        },
        conversionTracking: function () {
            var label = 'profileId=' + profileId + '|buyerMobile=' + buyersMobile;
            cwTracking.trackCustomData('UsedDetailsPage', 'Lead', label, true);
        },
        adWordTracking: function () {
            if (typeof dataLayer !== 'undefined') {
                dataLayer.push({ event: 'Desktop_UsedCarLeads' });
            }
        },
        slot: (function () {
            var _slotId = "0";
            var set = function (slotId) {
                _slotId = slotId;
            };
            var get = function () {
                return _slotId;
            }
            return {
                set: set,
                get: get,
            };
        })(),
        gsdClickTracking: function (originId) {
            if (typeof cwUsedTracking !== "undefined" && typeof Common !== "undefined") {
                var action = Common.utils.getCookie("TempCurrentUser")
                    ? cwUsedTracking.eventActions.gsdVerifiedText
                    : cwUsedTracking.eventActions.gsdUnverifiedText;
                var label = "originId=" + originId;

                var slotId = D_carDetailsPage.tracking.slot.get();
                if (slotId) {
                    label += "|slotId=" + slotId;
                }
                var gsdNode = leadData.getGsdNode();
                if (gsdNode) {
                    var ctePackageId = gsdNode.dataset.ctePackageId;
                    if (ctePackageId) {
                        label += "|ctePackageId=" + ctePackageId;
                    }
                }
                var trackingInputObj = {
                    action: action,
                    label: label,
                    sendQs: true,
                };
                cwUsedTracking.track(trackingInputObj);
            }
        }
    },
    sellerForm: {
        checkScrollHeight: function (scrollPosition) {
            if (scrollPosition > 150 && scrollPosition + $(window).height() < $('footer').offset().top) {
                $('.seller-form').addClass('floating-div');
                $('.seller-form').removeClass('floating-div-middle');
            }
            else if (scrollPosition + $(window).height() > $('footer').offset().top) {
                $('.seller-form').removeClass('floating-div');
                $('.seller-form').addClass('floating-div-middle');
            }
            else {
                $('.seller-form').removeClass('floating-div floating-div-middle');
            }
        },

    },
    similarCars: {
        SIMILARLISTING: ko.observableArray([]),
        recommendCarsDiv: document.getElementById("recommendCarsDiv"),
        registerEvents: function () {
            D_carDetailsPage.window.on('scroll', function () {
                scrollPosition = $(this).scrollTop();
                D_carDetailsPage.sellerForm.checkScrollHeight(scrollPosition);
                D_carDetailsPage.similarCars.checkScrollHeight(scrollPosition);
            });
            ko.applyBindings(D_carDetailsPage.similarCars.SIMILARLISTING, D_carDetailsPage.similarCars.recommendCarsDiv)

        },
        checkScrollHeight: function (scrollPosition) {
            if (scrollPosition + $(window).height() > $('div.cw-tabs.cw-tabs-flex.cw-tabs-inner-margin0 >ul>li.active').offset().top + 700) {
                D_carDetailsPage.similarCars.checkIfEligible();
            }
        },

        getSimilarCarsInputParameter: function () {
            return $("#getsellerDetails").attr("stockRecommendationUrl");
        },
        checkIfEligible: function () {
            if (!isSimilarFetched && !$('#buyerForm').is(':visible')) {
                isSimilarFetched = true;
                D_carDetailsPage.similarCars.fetchData(D_carDetailsPage.similarCars.getSimilarCarsInputParameter());
                D_carDetailsPage.tyreData.getTyreData();
            }
        },
        fetchData: function (input) {
            $.fetchSimilarCars(input);
        },
        bindSimilarCars: function (response) {
            D_carDetailsPage.similarCars.SIMILARLISTING(response);
            D_carDetailsPage.carouselForKnockout.alternateCarousel();
        },

    },
    tyreData: {
        getTyreData: function () {
            D_carDetailsPage.tyreData.fetchTyreData($("#getsellerDetails").attr("versionId"));
        },
        fetchTyreData: function (versionId) {
            var url = '/tyrelist/' + versionId + '/tyres?makeyear=' + MakeYear + '&pagesize=6';
            $.when(Common.utils.ajaxCall(url)).done(function (data) {
                if (data != null && $.trim(data) != "") {
                    $('#tyresCarousal').append(data);
                    D_carDetailsPage.carouselForKnockout.alternateCarousel();
                }
            });
        },
    },
    photoGallery: {
        registerEvents: function () {
            $(document).on('mouseout', '.pg-prev', function () {
                D_carDetailsPage.photoGallery.showPrevArrowOnMouseout();
            });
            $(document).on('mouseout', '.pg-next', function () {
                D_carDetailsPage.photoGallery.showNextArrowOnMouseout();
            });
        },
        showNextArrowOnMouseout: function () {
            $('.pg-next-image').css('display', 'block');
        },
        showPrevArrowOnMouseout: function () {
            $('.pg-prev-image').css('display', 'block');
        },
        hidePgArrow: function () {
            if ($('.pg-gallery .pg-thumbs li').length <= 1) {
                $('.pg-prev, .pg-next').hide();
            }
        }
    },
    certificate: {
        registerEvents: function () {
            /* Download tooltip box starts here */
            $('#download').bt({
                contentSelector: "$('#downloadpdf').html()",
                fill: '#ffffff',
                strokeWidth: 1,
                strokeStyle: '#ccc',
                trigger: ['click', 'none'],
                width: '300px',
                height: '600px',
                spikeLength: 8,
                positions: ['top', 'bottom', 'left', 'right'],
                padding: '10px 10px 25px 10px',
                preShow: function (box) {
                    if (typeof Common != 'undefined') {
                        Common.utils.lockPopup();
                    }
                }, clickAnywhereToClose: false,
                showTip: function (box) {

                    if ($.cookie('TempCurrentUser')) {
                        var tempCurrentUser = $.cookie('TempCurrentUser').split(':');
                        $('#pdftxtName').val(tempCurrentUser[0]);
                        $('#pdftxtMobile').val(tempCurrentUser[1]);
                    }
                    boxObj = $(box);
                    boxObj.show();
                    boxObj.find("#pdfcloseBox").click(function () {
                        D_carDetailsPage.certificate.closeCertificationBox();
                    });
                }
            });
            /* Download tooltip box ends here */

            D_carDetailsPage.doc.on('click', '#download_details', function (e) {
                originId = D_carDetailsPage.buyerProcessOriginId.certificationReport;
                $('#pdfnot_auth').hide().html('');
                D_carDetailsPage.recommendedCars.isRecommended = false;
                chatProcess.isChatLead = false;
                profileId = $('#getsellerDetails').attr('profileid');
                D_carDetailsPage.certificate.initiateBuyerProcess($(this), originId);
            });

            D_carDetailsPage.doc.on('focus', '#pdftxtName', function (e) {
                D_carDetailsPage.certificate.hideNameValidation();
            });
            D_carDetailsPage.doc.on('focus', '#pdftxtMobile', function (e) {
                D_carDetailsPage.certificate.hideMobileValidation();
            });
            D_carDetailsPage.doc.on('click', '.blackOut-window', function (e) {
                D_carDetailsPage.certificate.closeCertificationBox();
            });
        },
        missedCallClickVerifyHandler: function (json) {
            $("#missed-call__loading").hide();
            if (json && json.isMobileVerified) {
                D_carDetailsPage.certificate.handleVerificationSuccessResponse();
            }
            else {
                $(".missed-call__info-text").hide();
                $(".missed-call__error-msg").show();
            }
        },
        missedCallPollingVeriHandler: function (json) {
            if (json && json.isMobileVerified) {
                D_carDetailsPage.certificate.handleVerificationSuccessResponse();
            }
        },
        sendOtpApiHandler: function (json) {
            if (json && json.isOtpGenerated) {
                $("#missed-call-number").html('<span class="tel-icon"></span>' + json.tollFreeNumber);
                otpForm.reset();
                otpForm.open();
                otpForm.clearTimerTimeout = setTimer($('#otpTimer'), 'Resend OTP', 30);
                commonUtilities.executeTimely(function () {
                    if ($("#missed-call-number").is(":visible")) {
                        hitIsMobileVerifiedApi(buyersMobile, D_carDetailsPage.certificate.missedCallPollingVeriHandler);
                    }
                    else {
                        return true;
                    }
                }, 5000, 15000, 10);
            }
        },
        hitVerifyOtpApi: function (otp) {
            $.ajax({
                type: 'GET',
                url: '/api/v1/mobile/' + buyersMobile + '/verification/verifyotp/?otpCode=' + otp + '&sourceModule=' + D_carDetailsPage.utils.otpVariables.sourceModule,
                headers: { 'CWK': 'KYpLANI09l53DuSN7UVQ304Xnks=', 'SourceId': '1' },
                dataType: 'Json',
                success: function (json) {
                    if (json.responseCode == 1) {
                        D_carDetailsPage.certificate.handleVerificationSuccessResponse();
                    }
                    else {
                        otpForm.container.find('.otp__error').text('Invalid OTP!');
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    otpForm.container.find('.otp__error').text("something went wrong, please try again later!");
                }
            });
        },
        handleVerificationSuccessResponse: function () {
            $('#otpError').addClass('hide');
            otpForm.container.find('.otp__error').text('');
            otpForm.container.hide();
            $('.modal-bg-window').hide();
            if (chatProcess.isChatLead) {
                otpForm.blackOutWindow.hide();
                chatUIProcess.triggerChat();
            }
            else if ($('#photoGallery').is(':visible') && $('#photoGallery').css('visibility') != 'hidden') {
                $('#pggetsellerDetails').trigger('click');
            }
            else if ($('#valuation').is(':visible')) {
                $('#rpgetsellerDetails').trigger('click');
            }
            else if ($('div.bt-wrapper').is(':visible')) {
                $('#download_details').trigger('click');
            }
            else {
                $('#getsellerDetails').trigger('click');
            }
        },
        initiateBuyerProcess: function (elementObj, originId) {
            var attrId = 'pdf';
            deliveryCity = elementObj.attr("dc");
            $('#pdfprocess_img').show();
            initBuyerProcess(attrId, elementObj.attr('data-rank'), elementObj.attr('data-platform'), elementObj.attr('data-isPremium'), originId);
        },
        closeCertificationBox: function () {
            otpForm.blackOutWindow.hide();
            $('.blackOut-window').hide();
            if (typeof boxObj != 'undefined') {
                boxObj.hide();
            }
            D_carDetailsPage.certificate.hideProcessingIcon();
            D_carDetailsPage.certificate.hideNameValidation();
            D_carDetailsPage.certificate.hideMobileValidation();
            if (typeof Common != 'undefined') {
                Common.utils.unlockPopup();
            }
        },
        hideMobileValidation: function () {
            $('#pdftxtMobileError').hide();
            $('#pdferrorMobileCircle').hide();
        },
        hideNameValidation: function () {
            $('#pdftxtNameError').hide();
            $('#pdferrorNameCircle').hide();
        },
        showServerErrorMessage: function (message) {
            $('#pdfnot_auth').show().html(message);
            D_carDetailsPage.certificate.hideProcessingIcon();
        },
        hideProcessingIcon: function () {
            $('#pdfprocess_img').hide();
        }
    },
    buyerProcessInvalidResponse: {
        registerEvents: function () {
            $(".back-to-gsd-form").on("click", function () {
                var parent = $(this).parent();
                if (parent.attr("id") == "not_auth") {
                    $('.ask-dealer-question,.message-sent').hide();
                    $('.seller-details-on-sms, #buyer_form, #getsellerDetails').show();
                    $('#gettingDetails').hide();
                    buyerProcessReset();
                    $('#txtName').focus();
                } else if (parent.attr("id") == "rpnot_auth") {
                    D_carDetailsPage.Valuation.sellerDetails.showBuyerForm();
                }
                else {
                    $("#pgbuyer_form").show();
                    $("#pggetsellerDetails").show();
                    $('#pggettingDetails').hide();
                }
                parent.hide();
            });
        },
    },
    Finance: {
        iframeUrl: financeIframeUrl,
        registerEvents: function () {
            $(document).on("click", "#getFinance", function (event) {
                D_carDetailsPage.Finance.triggerFinanceClick(this, event);
            });
            $(document).on("click", "#iframe-close", function () {
                D_carDetailsPage.Finance.closeFinanceForm(this);
            });
            $(document).on("click", "#iframeTimeOutClick", function () {
                D_carDetailsPage.Finance.closeFinanceForm(this);
            });
        },
        triggerFinanceClick: function (currentnode, event) {
            event.stopPropagation();
            $('div.popup-loading-pic').show();
            var node = $("#getsellerDetails"); //search page listing
            window.history.pushState("cartrade", "", "");
            Common.utils.lockPopup();
            $('#financeIframe').show(function () {
                try {
                    classifiedFinance.getFinance($(currentnode).data("href"), node, $("#iframecontent")).then(function (response) {
                        $('div.popup-loading-pic').hide();
                        window.history.pushState("cartrade", "", "");
                        $("div.detail-ui-corner-top").css('display', 'none');
                        $("#iframecontent").show();
                    }).catch(function (errResponse) {
                        $("#iframeTimeOut").show();
                        $('div.popup-loading-pic').hide();
                    });
                } catch (err) { }
            });
        },
        closeFinanceForm: function () {
            clearTimeout(classifiedFinance.iframeError);
            $("#iframecontent").empty();
            $("#iframeTimeOut").hide();
            $("#financeIframe").hide();
            Common.utils.unlockPopup();
        }
    },
    Valuation: {
        loadingIconObj: $('.valuation-popup-loading-pic'),
        valuationPopupObj: $('.valuation-popup'),
        popupHeading: $('.valuation-popup__head'),
        valuationOtherDetails: $('.valuation-other-details'),
        similarCars: $('.valuation-popup__similar-cars'),
        rightPriceContent: $('.valuation-popup__right-price'),
        valuationHref: '',
        profileId: '',
        registerEvents: function () {
            $(document).on('click', '.view-market-price', D_carDetailsPage.Valuation.triggerValuationClick);
            $(document).on('click', '#globalPopupBlackOut, #valuation-popup-close', D_carDetailsPage.Valuation.triggerClosePopup);
            D_carDetailsPage.Valuation.recommendations.registerEvents();
        },
        triggerValuationClick: function (event) {
            event.stopPropagation();
            D_carDetailsPage.Valuation.valuationHref = $(this).data('href');
            D_carDetailsPage.Valuation.profileId = $(this).attr('profileid');
            D_carDetailsPage.Valuation.showValuation();
            D_carDetailsPage.Valuation.pushToHistory({ title: 'detailsValuation', href: D_carDetailsPage.Valuation.valuationHref });
            D_carDetailsPage.Valuation.sellerDetails.bindBuyerForm();
        },
        pushToHistory: function (state) {
            window.history.pushState(state, '', '');
        },
        showValuation: function () {
            Common.utils.lockPopup();
            D_carDetailsPage.Valuation.valuationPopupObj.show();
            D_carDetailsPage.Valuation.loadingIconObj.show();
            D_carDetailsPage.Valuation.rightPriceContent.load(D_carDetailsPage.Valuation.valuationHref, function (response, status) {
                D_carDetailsPage.Valuation.rightPriceContent.animate({ scrollTop: 0 });
                D_carDetailsPage.Valuation.valuationOtherDetails.show();
                D_carDetailsPage.Valuation.loadingIconObj.hide();
                D_carDetailsPage.Valuation.trackClick();
                if (status == 'error') {
                    D_carDetailsPage.Valuation.rightPriceContent.html("Something went wrong. Please try again later");
                }
            });
        },
        triggerClosePopup: function () {
            if (D_carDetailsPage.Valuation.valuationPopupObj.is(':visible')) {
                history.back();
            }
        },
        closePopup: function () {
            D_carDetailsPage.Valuation.rightPriceContent.empty();
            D_carDetailsPage.Valuation.valuationPopupObj.hide();
            D_carDetailsPage.Valuation.sellerDetails.showBuyerForm();
            D_carDetailsPage.Valuation.valuationPopupObj.removeClass('similar-cars-active');
            D_carDetailsPage.Valuation.valuationPopupObj.removeClass('list--slide');
            D_carDetailsPage.Valuation.valuationOtherDetails.hide();
            D_carDetailsPage.Valuation.similarCars.hide();
            D_carDetailsPage.Valuation.recommendations.slideBtn.hide();
            D_carDetailsPage.Valuation.recommendations.rprecommendation.empty();
            Common.utils.unlockPopup();
        },
        trackClick: function () {
            var trackingParam = {};
            trackingParam['profileId'] = D_carDetailsPage.Valuation.profileId;
            trackingParam['caseId'] = $(".right-price-box").attr("caseid");
            var rightPriceTrackingData = cwTracking.prepareLabel(trackingParam);
            cwTracking.trackCustomData(D_carDetailsPage.variables.getBhriguCategory(), 'RightPriceClick', rightPriceTrackingData, true);
        },
        setHeading: function (message) {
            D_carDetailsPage.Valuation.popupHeading.text(message);
        },
        sellerDetails: {
            userForm: $('.form__user-details'),
            sellerForm: $('.form__seller-details'),
            errorMessage: $('#rpnot_auth'),
            gettingDetailsBtn: $("#rpgettingDetails"),
            sellerDetailsBtn: $("#rpgetsellerDetails"),
            mobileError: $("#rptxtMobileError"),
            emailError: $("#rptxtEmailError"),

            bindSellerDetails: function (ds) {
                $('#blackOut-recommendation').hide();
                if (ds) {
                    D_carDetailsPage.Valuation.sellerDetails.gettingDetailsBtn.hide();
                    D_carDetailsPage.Valuation.sellerDetails.userForm.hide();
                    $('#sellerName').text(ds.seller.name);
                    $('#sellerContact').text(ds.seller.mobile);
                    $('#sellerEmail').text(ds.seller.email);
                    $('#sellerAddress').text(ds.seller.address);
                    $("#sellerContactPerson").text(ds.seller.contactPerson ? ds.seller.contactPerson : "");
                    if (ds.seller.dealerShowroomPage) {
                        $('.seller-virtual-link').attr('href', ds.seller.dealerShowroomPage).show();
                    }
                    else {
                        $(".seller-virtual-link").hide();
                    }
                    D_carDetailsPage.Valuation.sellerDetails.sellerForm.show();
                    var similarCarsUrl = $("#similarCars a").attr('href');
                    if (similarCarsUrl)
                        $('.bp-SimilarCars').attr('href', similarCarsUrl).show();
                }
            },
            showServerErrorMessage: function (message) {
                D_carDetailsPage.Valuation.sellerDetails.userForm.hide();
                D_carDetailsPage.Valuation.sellerDetails.errorMessage.show().find(".back-to-gsd-form > span").text(message);
            },
            showBuyerForm: function () {
                D_carDetailsPage.Valuation.sellerDetails.userForm.show();
                D_carDetailsPage.Valuation.sellerDetails.sellerDetailsBtn.show();
                D_carDetailsPage.Valuation.sellerDetails.gettingDetailsBtn.hide();
                D_carDetailsPage.Valuation.sellerDetails.sellerForm.hide();
                D_carDetailsPage.Valuation.sellerDetails.errorMessage.hide();
                D_carDetailsPage.Valuation.sellerDetails.mobileError.next().hide();
                D_carDetailsPage.Valuation.sellerDetails.mobileError.hide();
                D_carDetailsPage.Valuation.sellerDetails.emailError.next().hide();
                D_carDetailsPage.Valuation.sellerDetails.emailError.hide();
                D_carDetailsPage.Valuation.sellerDetails.gettingDetailsBtn.addClass('hide');
            },
            bindBuyerForm: function () {
                if ($.cookie('TempCurrentUser')) {
                    var tempCurrentUser = $.cookie('TempCurrentUser').split(':');
                    $('#rptxtName').val(tempCurrentUser[0]);
                    $('#rptxtMobile').val(tempCurrentUser[1]);
                    var email = tempCurrentUser[2];
                    if (email) {
                        $('#rptxtEmail').val(email);
                        $('.email-update-field').trigger('click');
                    }
                }
            }
        },
        recommendations: {
            slideBtn: $('#valuationPopupSlideBtn'),
            tooltipLabel: $('#slideBtnTooltipLabel'),
            list: $('.valuation-popup__similar-cars'),
            rprecommendation: $('#rprecommendedcars'),
            registerEvents: function () {
                $(document).on('click', '#valuationPopupSlideBtn', D_carDetailsPage.Valuation.recommendations.toggleList);
            },
            bindRecommendations: function (jsonData) {
                D_carDetailsPage.Valuation.similarCars.show();
                D_carDetailsPage.RECOMMENDLISTING(jsonData);
                var recommendedListings = $('#recommendations').html();
                D_carDetailsPage.RECOMMENDLISTING([]);
                D_carDetailsPage.Valuation.recommendations.list.show();
                D_carDetailsPage.Valuation.recommendations.rprecommendation.html(recommendedListings);
                $('.bp-SimilarCars').hide();
                D_carDetailsPage.Valuation.recommendations.slideBtn.trigger('click');
            },
            toggleList: function () {
                if (!D_carDetailsPage.Valuation.valuationPopupObj.hasClass('similar-cars-active')) {
                    D_carDetailsPage.Valuation.valuationPopupObj.addClass('similar-cars-active');
                    setTimeout(function () {
                        D_carDetailsPage.Valuation.recommendations.slideBtn.show();
                    }, 500);
                }

                if (!D_carDetailsPage.Valuation.valuationPopupObj.hasClass('list--slide')) {
                    D_carDetailsPage.Valuation.valuationPopupObj.addClass('list--slide');
                    D_carDetailsPage.Valuation.setHeading('People also liked');
                    D_carDetailsPage.Valuation.recommendations.setTooltipLabel('Right price');
                }
                else {
                    D_carDetailsPage.Valuation.valuationPopupObj.removeClass('list--slide');
                    D_carDetailsPage.Valuation.setHeading('Right Price');
                    D_carDetailsPage.Valuation.recommendations.setTooltipLabel('Recommended cars');
                }
            },

            setTooltipLabel: function (message) {
                D_carDetailsPage.Valuation.recommendations.tooltipLabel.text(message);
            }
        }
    }
};

var otpForm = {
    container: $('#otpForm'),
    blackOutWindow: $('#blackOut-recommendation'),
    clearTimerTimeout: '',

    registerEvents: function () {
        $(document).on('click', '#otpClose', function () {
            $(".missed-call__info-text").show();
            $(".missed-call__error-msg").hide();
            otpForm.close();

            if ($('#photoGallery').is(':visible') && $('#photoGallery').css('visibility') != 'hidden') {
                $("#" + "pg" + "gettingDetails").css('display', 'none');
                $("#" + "pg" + "getsellerDetails").show();
            }
            else if ($('#valuation').is(':visible')) {
                $("#" + "rp" + "gettingDetails").css('display', 'none');
                $("#" + "rp" + "getsellerDetails").show();
            }
            else if ($('div.bt-wrapper').is(':visible')) {
                $('#pdfprocess_img').hide();
            }
            else {
                $("#" + "gettingDetails").css('display', 'none');
                $("#" + "getsellerDetails").show();
            }
        });

        $(document).on('keyup', '#getOTP', function () {
            var otp = this.value;
            $(this).val(otp.replace(/[^\d]/, ''));
            if (otp.length == 5) {
                D_carDetailsPage.certificate.hitVerifyOtpApi(otp);
            }

        });

        $(document).on('click', '#verifyOTP', function () {
            var enteredOTP = $("#getOTP").val();
            if (enteredOTP.length == 0) {
                otpForm.container.find('.otp__error').text('Please enter OTP');
            }
            else if (enteredOTP.length < 5) {
                otpForm.container.find('.otp__error').text('Invalid OTP!');
            }
            else {
                D_carDetailsPage.certificate.hitVerifyOtpApi(enteredOTP);
            }
        });

        $(document).on('click', '#otpTimer', function () {
            $.ajax({
                type: "POST",
                url: "/api/v1/resendotp/",
                headers: { 'sourceid': 1 },
                data: JSON.stringify(getMobileVerificationApiData(buyersMobile)),
                contentType: 'application/json',
                dataType: 'json',
                success: function (response) {
                    $('#otpTimer').addClass('otp-status--done').html('OTP sent to Mobile');
                },
                error: function (xhr) {
                    otpForm.container.find('.otp__error').text('Something went wrong!');
                }
            });
        });

        // start of Verification click event
        $(document).on('click', "#missed-call-link", function (e) {
            $("#missed-call__loading").show();
            hitIsMobileVerifiedApi(buyersMobile, D_carDetailsPage.certificate.missedCallClickVerifyHandler);
        });
        // Ends
    },

    open: function () {
        otpForm.container.show();
        otpForm.blackOutWindow.show();
    },

    close: function () {
        otpForm.container.hide();
        otpForm.blackOutWindow.hide();
    },

    validate: {
        setError: function (message) {
            otpForm.container.find('.otp__error').text(message);
        },

        hideError: function () {
            otpForm.container.find('.otp__error').text('');
        }
    },

    reset: function () {
        $('#getOTP').val("");
        otpForm.container.find('.otp__error').text('');
        resetTimer($('#otpTimer'), otpForm.clearTimerTimeout);
    }
};

function resetTimer(container, timerElement) {
    clearTimeout(timerElement);
    container.removeClass('counter--active counter-done otp-status--done');
    container.html('Resend OTP in <span class="time-counter">30</span>s');
}

function setTimer(container, successMessage, count) {
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
}

$(document).ready(function () {
    ko.applyBindings(D_carDetailsPage.RECOMMENDLISTING, document.getElementById("recommendations"));
    D_carDetailsPage.pageLoad();
    D_carDetailsPage.sellerDetailForm.openDetailForm();
    $.coachmarkcookie();
    var prevNextHandleVar = Math.ceil(parseFloat($('#recommendedCars li').length) / 4);
    var prevNextHandleConst = prevNextHandleVar;
    prevNextDisplay(prevNextHandleConst); $('#askexpertsidebanner').remove();

    $('#videoIcon,.play-btn-box').click(function () {
        $('.uc-thumbnail').trigger('click');
        $('#videoTab').trigger('click');
    });
    $('#photoIcon').click(function () {
        $('.uc-thumbnail').trigger('click');
        $('#photoTab').trigger('click');
        $('#pgtxtName').focus();
    });
    $('#photoTab').click(function () {
        $('#galleryList li:first').find('a').trigger('click');
        $('#videoContainer').hide();
        $('#videoContainer').find('iframe').attr('src', '');
        $('#gallery').css({ 'visibility': 'visible', 'height': 'auto' }).show();
        $('#videoTab').removeClass('bold');
        $('#photoTab').addClass('bold');
        $('#pgtxtName').focus();
    });
    $('#videoTab').click(function () {
        $('#gallery').css({ 'visibility': 'hidden', 'height': '0px' });
        $('#videoContainer').find('iframe').attr('src', videoUrl);
        $('#videoContainer').show();
        $('#photoTab').removeClass('bold');
        $('#videoTab').addClass('bold');
        $('#pgtxtName').focus();
    });
    $('.uc-thumbnail').click(function () {
        if ($('#pgbuyer_form').is(':visible') || $('#pgVerifyMobile').is(':visible')) {
            $('#pgseller-details').hide();
        }
        $('#galleryList li:first').find('a').trigger('click');
        $('#photoGallery').css({ 'visibility': 'visible', 'height': 'auto' }).show();
        $('#gallery').css({ 'visibility': 'visible', 'height': 'auto' });
        if (videoUrl.length > 0) {
            $('#gallery').hide();
            $('#videoContainer').find('iframe').attr('src', videoUrl);
            $('#videoTab').addClass('bold');
            $('#photoTab').removeClass('bold');
            $('#videoContainer').show();
        }
        else {
            $('#videoContainer').hide();
            $('#videoContainer').find('iframe').attr('src', '');
            $('#videoTab').removeClass('bold');
            $('#photoTab').addClass('bold');
            $('#gallery').css({ 'visibility': 'visible', 'height': 'auto' }).show();
        }
        $('div.blackout-window-pg').show();
        $('#photoGallery, #pg_closeBtn').show('500');
        D_carDetailsPage.photoGallery.hidePgArrow();
        if ($.cookie('TempCurrentUser')) {
            var tempCurrentUser = $.cookie('TempCurrentUser').split(':');
            $('#pgtxtName').val(tempCurrentUser[0]);
            $('#pgtxtMobile').val(tempCurrentUser[1]);
        }
    });

    $(".topTabs li").click(function () {
        var element = $(this);
        showTopTabsData(element);
    });
    $('.overviewTabs li').click(function () {
        var element = $(this);
        var id = element.attr('data-tabs');
        $('.overviewTabs li').removeClass('active');
        element.addClass('active');
        $('.overviewConatiner').hide();
        $('#' + id).show();
    });
    //for other author carousel
    $('#recommendedCars').jcarousel({
        scroll: 4,
        auto: 0,
        animation: 800,
        wrap: "both",
        initCallback: initCallbackUCR, buttonNextHTML: null, buttonPrevHTML: null
    });

    $('#btnVerifyCaptcha, #pg-btnVerifyCaptcha').click(function () {
        var captchaCode, thisId = this.id;
        if (thisId == "btnVerifyCaptcha")
            captchaCode = $('#txtCaptchaCode').val();
        else
            captchaCode = $('#pg-txtCaptchaCode').val();

        if (captchaCode.length == 0) {
            if (thisId == "btnVerifyCaptcha") {
                ShakeFormView($(".captcha-inputbox"));
                $('#lblCaptcha').html("Code Required").show();
                $('#lblCaptcha').next().show();
            }
            else {
                ShakeFormView($(".captcha-inputbox"));
                $('#pg-lblCaptcha').html("Code Required").show();
                $('#pg-lblCaptcha').next().show();
            }
            return false;
        }
        else {
            VerifyCaptcha(captchaCode, thisId);
        }
    });

    $('input[type="text"]').focus(function () {
        $('.uc-error-circle,.pguc-error-circle,.cw-blackbg-tooltip,.pgcw-blackbg-tooltip').hide();
    });

    function initCallbackUCR(carousel) {
        $('#list_carousel_widget_prev').bind('click', function () {
            if (prevNextHandleVar < prevNextHandleConst || prevNextHandleVar == 0) {
                var arrowElement = $(this);
                arrowElement.css("pointer-events", "none");
                $('#list_carousel_widget_next').removeClass("hide");
                prevNextHandleVar = prevNextHandleVar + 1;
                carousel.prev();
                setTimeout(function () {
                    arrowElement.css("pointer-events", "");
                }, 800);
                if (prevNextHandleVar == prevNextHandleConst)
                    $('#list_carousel_widget_prev').addClass("hide");
            }
            return false;
        });

        $('#list_carousel_widget_next').bind('click', function () {
            if (prevNextHandleVar > 0 || prevNextHandleVar == prevNextHandleConst) {
                var arrowElement = $(this);
                arrowElement.css("pointer-events", "none");
                $('#list_carousel_widget_prev').removeClass("hide");
                prevNextHandleVar = prevNextHandleVar - 1;
                carousel.next();
                setTimeout(function () {
                    arrowElement.css("pointer-events", "");
                }, 800);
                if (prevNextHandleVar == 1)
                    $('#list_carousel_widget_next').addClass("hide");
            }
            return false;
        });
    }

    /* popup */
    $('#shareIcon,.email-friend').click(function (e) {
        $('#blackOut-window').show();
    });
    $('.uc-close-btn, #close-popup').click(function (e) {
        $('#blackOut-window').hide();
        $('.cw-popup').hide();
    });
    $('#shareIcon').click(function (e) {
        $('.share-popup').show();
    });
    $('.email-friend').click(function (e) {
        $('.share-popup').hide();
        $('.send-to-frnd-popup').show();
    });
    $('#sentMail').click(function (e) {
        $('.send-to-frnd-popup').hide();
        $('.thankyou-popup').show();
    });
    $('#shortlistIcon').click(function (e) {
        $(this).toggleClass('rounded-border-orange');
        $(this).find('.used-sprite').toggleClass('favorite');
        if ($('#shortlistIcon').find('.used-sprite').hasClass('favorite')) {
            $(this).attr('title', 'Shortlisted');
        }
        else if ($('#shortlistIcon').find('.used-sprite').hasClass('favorite-grey')) {
            $(this).attr('title', 'Shortlist');
        }
    });

    $('.uc-more-dd').click(function (e) {
        e.stopPropagation();
        $('.more-list').slideToggle();
        $("body").click(function (e) {
            $('.more-list').hide();
        });

    });

    $("#flagIcon").click(function (e) {
        e.preventDefault();
        ShowLoading();
    });

    $("#reqPhotos").click(function () {
        ShowRequestPhotosGrayBox();
    });

    $('#pg_closeBtn').click(function () {
        $('#videoContainer').find('iframe').attr('src', '');
        $('#photoGallery, #pg_closeBtn, div.blackout-window-pg').hide();
        if ($('#pg-captcha').is(':visible')) {
            $('#pg-captcha').hide();
            $('.step-1').show();
        }
        $('.red-border').removeClass('red-border');
        $('.error-icon').hide();
        $('.cw-blackbg-tooltip').hide();
        $('.pg-error').text('');
        $('html, body').css({
            'overflow': 'auto',
            'height': 'auto'
        });
    });

    $('div.blackout-window-pg').click(function () {
        if ($('#photoGallery').is(':visible'))
            $('#pg_closeBtn').trigger('click');
    });
    $(".uc-tabs li:first").trigger('click');
    if ($('.topTabs li').length > 0)
        $('.topTabs').removeClass('hideImportant');

    $('.car-condition-tabs ul li:first').trigger('click');

    $('input[type=text]').focus(function () { //remove error icon when textbox is clicked
        $('.error-icon').hide();
        $('.cw-blackbg-tooltip').hide();
    });

    bindPhotoGallery();

    sellerDetailsBtnTextChange();

    var grayContent = "<div id='gb-overlay'></div>";
    grayContent += "<div id='gb-window'>";
    grayContent += "<div id='gb-head'><span id='gb-title'></span><a id='gb-close' class='gb-close'></a><div class='clear'></div></div>";
    grayContent += "<img id='loading' src='https://imgd.aeplcdn.com/0x0/statics/loader.gif'/><div id='gb-content'></div>";
    grayContent += "</div>";

    $("body").prepend(grayContent);

    //Photo gallery toggle the email fields
    $(document).on('click', '#pgemailTick', function () {
        if ($('#pgemailTick').is(':checked')) {
            $('#pgEmail').show();
            $('#pgtxtEmail').focus();
        }
        else {
            $('#pgEmail').hide();
            $('#pgtxtName').focus();
        }
    });

    /* question box*/
    $(' .uc-question-box-title').click(function (e) {
        $('.uc-question-list').slideToggle();
    });
    $('.uc-question-list li').click(function (e) {
        $(this).find('span').toggleClass('uc-checked');
    });
    $('.email-update').click(function (e) {
        var temp = $(this).find('div.cw-used-sprite');
        temp.toggleClass('uc-checked');
        $('.optional-email').toggle();
        if (temp.hasClass('uc-checked')) {
            $('#txtEmail').focus();
        }
        else {
            $('#txtName').focus();
        }
    });

    //Contact Seller Form Button Click
    $('#getsellerDetails,#pggetsellerDetails,#rpgetsellerDetails').click(function (e) {
        chatProcess.isChatLead = false;
        var $gsdBtn = $(this);
        leadData.setGsdNode(this);
        var attrId = getAttrId($gsdBtn);
        if ((attrId == "" || attrId == "pg" || attrId == "rp")) {
            $("#" + attrId + "gettingDetails").css('display', 'inline-block');
            $("#" + attrId + "getsellerDetails").hide();
        }
        if (typeof commonUtilities !== 'undefined') {
            D_carDetailsPage.tracking.slot.set(commonUtilities.getFilterFromQS("slotId", window.location.search));
        }
        processGSDClick($gsdBtn, attrId);
    });

    //Method for view seller details click
    $('#viewsellerDetails, #pgviewsellerDetails').live('click', function () {

        var attrId = '';
        $(this).hide();
        if ($(this).attr('id') == 'pgviewsellerDetails') {
            attrId = 'pg';
            originId = D_carDetailsPage.buyerProcessOriginId.photogallerySellerDetailsForm;
        }
        else
            originId = D_carDetailsPage.buyerProcessOriginId.sellerDetailsForm;
        $("#" + attrId + "viewingDetails").attr('style', 'display:block');
        var askAttr = $(this).attr('ask');
        if (typeof askAttr !== typeof undefined && askAttr !== false) {
            attrId = 'ask';
        }
        processPurchaseInq(attrId, $(this).attr('data-rank'), $(this).attr('data-platform'), $(this).attr('data-isPremium'));
        dataLayer.push({ event: 'DP_ZipDialVerfication_Stage1', cat: 'UsedCarDetails', act: 'DP_ZipDialVerfication_Stage1' });
    });

    $('.getDetails').click(function (e) {
        $('.ask-dealer-question,.message-sent,#sendingMessage').hide();
        $('.seller-details-on-sms').show();
        $('.askDealer').show();
        buyerProcessReset();
        $('#txtName').focus();
    });
    $('.close-btn').click(function (e) {
        $('.ask-dealer-question,.message-sent').hide();
        $('.seller-details-on-sms, #buyer_form, #getsellerDetails').show();
        buyerProcessReset();
        $('#txtName').focus();
    });

    // otp popup
    otpForm.registerEvents();

    chatUIProcess.registerEvents();
    var isChatSms = commonUtilities.getFilterFromQS("ischatsms", window.location.search);
    if (isChatSms) {
        $('#gsdChat').trigger('click');
    }

    if (typeof cwUsedTracking !== 'undefined') {
        cwUsedTracking.setEventCategory(cwUsedTracking.eventCategory.UsedDetailsPage);
    }
}); // end of document.ready

function VerifyCaptcha(captchaCode, thisId) {
    $.ajax({
        type: "POST",
        url: "/ajaxpro/CarwaleAjax.AjaxClassifiedBuyer,Carwale.ashx",
        data: '{"captchaInput":"' + captchaCode + '", "captchaCookie":"' + $.cookie('CaptchaImageText') + '"}',
        dataType: 'json',
        beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "VerifyCaptcha") },
        success: function (response) {
            var ds = eval('(' + response.value + ')');
            if (ds.captchaStatus == "1") {
                if (thisId == "btnVerifyCaptcha")
                    $('#captcha').hide();
                else
                    $('#pg-captcha').hide();
                isFromCaptcha = "1";
                processPurchaseInq(objDetailsCaptcha.attrId, objDetailsCaptcha.rank, objDetailsCaptcha.pf, objDetailsCaptcha.isPremium);
            }
            else {
                if (thisId == "btnVerifyCaptcha") {
                    ShakeFormView($(".captcha-inputbox"));
                    $('#lblCaptcha').html("Invalid Code").show();
                    $('#lblCaptcha').next().show();
                }
                else {
                    ShakeFormView($(".captcha-inputbox"));
                    $('#pg-lblCaptcha').html("Invalid Code").show();
                    $('#pg-lblCaptcha').next().show();
                }
                return false;
            }
        }
    });
}


function prevNextDisplay(prevNextHandleConst) {
    $('#list_carousel_widget_prev').addClass("hide");
    if (prevNextHandleConst < 2) {
        $('#list_carousel_widget_next').addClass("hide");
    }
}

function ShowLoading() {
    var caption = "Report this listing";
    var url = "/used/reportListing.aspx?car=" + profileId;
    var applyIframe = false;
    var GB_Html = "";

    GB_show(caption, url, 325, 520, applyIframe, GB_Html);
}

//To show the data when tabs below image gallery are clicked
function showTopTabsData(element) {
    var id = element.attr('data-tabs');
    if (id != "more") {
        $('.topTabContainers').hide();
        $(".topTabs li").removeClass('active');
        if (element.parent().parent().hasClass('more-list')) {
            $('.uc-more-dd').addClass('active');
            tabAnimation.tabHrInit();
        }
        else { $('.uc-more-dd').removeClass('active'); }
        element.addClass('active');
        $('#' + id).show();
    }
}

function scrollToElement(ele) {
    $('html,body').animate({
        scrollTop: ele.offset().top - 50
    },
    'slow');
}
var StartIndex = 0;
function bindPhotoGallery() {
    $('.pg-gallery').adGallery({
    });

    $('#galleryList li a').click(function () {
        if ($(this).hasClass('pg-active')) {
            return false;
        }
    });
}
doNotShowAskTheExpert = false;

function sellerDetailsBtnTextChange() {
    if ($.cookie('TempCurrentUser') != null) {
        $("span.gsdTxt").hide();
        $("span.oneClickDetails").show();
    }
}

$("#pgbackToBuyerForm").live("click", function (e) {
    $(this).hide();
    $('#pgverifying, #pgviewingDetails, .pgmobile-verification, pgseller-details-call, #pggettingDetails,#pgtollFreeBox').hide();
    $('#pgverifyNumber, #pgviewsellerDetails, #pgbuyer_form, #pggetsellerDetails').show();
    $('#pgmissedCallVerification').html('');
    $('#pgtxtCwiCode').val("");
    $('#pgerrorVerifyCode').text("");
    $('#pgerrorVerifyCode,#pgerrorVerifyCodeCircle').attr('style', 'display:none');
    $('.pgtollFree').removeClass('error');
    $("#pgnot_auth").hide()
    $('#pgtxtName').focus();
});

function buyerProcessReset() {
    $('#verifying, #viewingDetails,.seller-details,.mobile-verification,.seller-details-call').hide()
    $('#verifyNumber, #viewsellerDetails').show();
    $('#missedCallVerification').html('');
    $('#txtCwiCode').val("");
    $('#errorVerifyCode').text("");
    $('#errorVerifyCode,#errorVerifyCodeCircle').attr('style', 'display:none');
}
var ltsrc = "",   //Lead Tracking Source 
           cwc = "",     //Carwale cookie which stores the unique identity of evry user
           buyersName = "", buyersEmail = "", buyersMobile = "", askQuestion = "", // Buyers Details
        carModel = "", makeYear = "", GB_DONE = false, re = /^[0-9]*$/, dealerQuestion = "", askform = "",
        transToken = "", reEmail = /^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,6}$/, clientId = "",
        requestCount = 1, isDisabled = false, UTMA = '', UTMZ = '';

function GB_show(caption, url, height, width, applyIframe, GB_Html) {
    try {
        GB_HEIGHT = height || 400;
        GB_WIDTH = width || 400;

        // show loading gif image if taking time in loading
        $("#loading").show();
        $("#gb-content").hide();

        if (!GB_DONE) {// append only once			
            $("#gb-close").click(GB_hide);
            $("#gb-overlay").click(GB_hide);

            if (applyIframe) { // Apply iframe on demand				
                $("#gb-overlay").bgiframe();
            }

            GB_DONE = true;
        }

        $("#gb-title").html(caption);
        $("#gb-overlay").show().css({ height: $(document).height() + "px", opacity: "0.9", width: "100%" });
        $("#gb-window").show();
        GB_position();


        if (url != "#" && GB_Html == "") { // url available to load external page.
            $("#gb-content").load(url, loadingDone);
        } else { // 
            $("#gb-content").html(GB_Html);
            loadingDone();
        }

    } catch (e) {
        alert(e);
    }
}
function getCookie(visited) {
    var Isvisited = visited + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1);
        if (c.indexOf(Isvisited) != -1)
            return c.substring(Isvisited.length, c.length);
    }
    return "";
}
function GB_hide() {
    $("#gb-window,#gb-overlay").hide();
}
function GB_position() {
    var de = document.documentElement;
    var w = self.innerWidth || (de && de.clientWidth) || document.body.clientWidth;

    var gbTop = getTopPos();

    if (GB_HEIGHT >= 450) {
        gbTop += 30;
    } else {
        gbTop += 100;
    }

    $("#gb-window").css({ width: GB_WIDTH + "px", height: 'auto', left: ((w - GB_WIDTH) / 2) + "px", top: gbTop });
    $("#gb-content").css({ height: GB_HEIGHT + "px" });
}
function getTopPos() {
    return getTopResults(window.pageYOffset ? window.pageYOffset : 0, document.documentElement ? document.documentElement.scrollTop : 0, document.body ? document.body.scrollTop : 0);
}
function getTopResults(n_win, n_docel, n_body) {
    var n_result = n_win ? n_win : 0;
    if (n_docel && (!n_result || (n_result > n_docel)))
        n_result = n_docel;
    return n_body && (!n_result || (n_result > n_body)) ? n_body : n_result;
}
function loadingDone() {
    $("#loading").hide();
    $("#gb-content").fadeIn(300);
}

function getAttrId(gsdButton) {
    var attrId;
    if (gsdButton.attr('id') === "pggetsellerDetails") {
        attrId = "pg";
    } else if (gsdButton.attr('id') === "rpgetsellerDetails") {
        attrId = "rp";
    } else {
        attrId = "";
    }
    return attrId;
}

function processGSDClick(gsdButton, attrId) {
    var attrId = attrId ? attrId : "";
    var dataRank = gsdButton.attr('data-rank');
    var dataPlatform = gsdButton.attr('data-platform');
    var dataPremium = gsdButton.attr('data-isPremium');

    profileId = $('#getsellerDetails').attr('profileid');
    deliveryCity = $('#getsellerDetails').attr('dc');

    if (attrId === "pg") {
        originId = D_carDetailsPage.buyerProcessOriginId.photogallerySellerDetailsForm;
    } else if (attrId === "rp") {
        originId = D_carDetailsPage.buyerProcessOriginId.RightPrice;
        var gsd = $("#getsellerDetails");
        dataRank = gsd.attr('data-rank');
        dataPlatform = gsd.attr('data-platform');
        dataPremium = gsd.attr('data-ispremium');
    }
    else
        originId = D_carDetailsPage.buyerProcessOriginId.sellerDetailsForm;
    $('#viewsellerDetails').removeAttr("ask");
    isGSDClick = "1";
    D_carDetailsPage.recommendedCars.isRecommended = false;
    initBuyerProcess(attrId, dataRank, dataPlatform, dataPremium, originId);
    dataLayer.push({ event: 'buyUsedCarDetails', cat: 'UsedCarDetails', act: 'ContactSeller' });
}



function initBuyerProcess(attrId, rank, pf, isPremium, originId) {
    if (attrId == undefined || attrId == null) {
        attrId = "";
    }

    ltsrc = getCookie("CWLTS").split(':')[0];
    cwc = getCookie("CWC");
    UTMA = getCookie("__utma");
    if (UTMA) {
        UTMA = '"' + UTMA + '"';
    }
    UTMZ = getCookie("__utmz");
    if (UTMZ) {
        UTMZ = '"' + UTMZ + '"';
    }

    buyersName = $("#" + attrId + "txtName").val();
    buyersMobile = $("#" + attrId + "txtMobile").val();

    if (attrId == "pg") {
        if ($('#' + attrId + 'emailTick').is(':checked'))
            buyersEmail = $("#" + attrId + "txtEmail").val();
        else
            buyersEmail = "";
    }
    else {
        if ($("#" + attrId + "emailTick").hasClass("uc-checked"))
            buyersEmail = $("#" + attrId + "txtEmail").val();
        else
            buyersEmail = "";
    }

    if (validateForm(attrId, originId)) {
        $(this).hide();
        processPurchaseInq(attrId, rank, pf, isPremium);
        dataLayer.push({ event: 'DP_Verification_Stage1', cat: 'UsedCarDetails', act: 'DP_Verification_Stage1' });
    }
}

// Method for Initial get seller details request in the Buyer Process Form  
function validateForm(attrId, originId) {
    clearErrorBox(attrId);

    var isValid = false;
    isValid = validateBuyerName(attrId);
    isValid &= validateBuyerMobile(attrId);

    if (isValid) {
        if (validateBuyerEmail(attrId)) {
            return true;
        }
    }
    else {
        if(chatProcess.isChatLead) {
            chatUIProcess.loader.hide();
            cwTracking.trackCustomData(D_carDetailsPage.variables.getBhriguCategory(), D_carDetailsPage.variables.getChatUnVerifiedText(), 'profileId=' + $('#getsellerDetails').attr('profileid'), false);
        }
        else {
            $("#" + attrId + "gettingDetails").css('display', 'none');
            $("#" + attrId + "getsellerDetails").show();
        }
    }
    D_carDetailsPage.tracking.gsdClickTracking(originId);
    return false;
}

//clear the error messages
function clearErrorBox(attrId) {
    $("#" + attrId + "txtNameError").hide().text("");
    $("#" + attrId + "errorNameCircle").hide();

    $("#" + attrId + "txtMobileError").hide().text("");
    $("#" + attrId + "errorMobileCircle").hide();

    $("#" + attrId + "txtEmailError").hide().text("");
    $("#" + attrId + "errorEmailCircle").hide();
}

// validate buyer's Name
function validateBuyerName(attrId) {
    var isValid = false,
		message = "";

    var reTest = /^[a-zA-Z_'. ]{3,50}$/,
		fieldLength = buyersName.length;

    if (!fieldLength) {
        message = "Please enter your name."
    }
    else if (fieldLength < 3) {
        message = "Name should be atleast 3 characters."
    }
    else if (!reTest.test(buyersName)) {
        message = "Invalid name."
    }
    else {
        isValid = true;
    }

    if (!isValid) {
        ShakeFormView($("#" + attrId + "txtName").closest(".form-control-box"));
        $("#" + attrId + "txtNameError").text(message).show();
        $("#" + attrId + "errorNameCircle").show();
    }

    return isValid;
}

//validate buyer's Mobile
function validateBuyerMobile(attrId) {
    if (buyersMobile == "") {
        ShakeFormView($(".mobile-box, .mobile-ug-field"));
        $("#" + attrId + "txtMobileError").show();
        $("#" + attrId + "txtMobileError").text("Please enter your mobile number");
        $("#" + attrId + "errorMobileCircle").show();
        return false;
    } else if (buyersMobile != "" && re.test(buyersMobile) == false) {
        ShakeFormView($(".mobile-box, .mobile-ug-field"));
        $("#" + attrId + "txtMobileError").show();
        $("#" + attrId + "txtMobileError").text("Invalid mobile number");
        $("#" + attrId + "errorMobileCircle").show();
        return false;
    } else if (buyersMobile != "" && (!re.test(buyersMobile) || buyersMobile.length < 10 || buyersMobile.length > 10)) {
        ShakeFormView($(".mobile-box, .mobile-ug-field"));
        $("#" + attrId + "txtMobileError").show();
        $("#" + attrId + "txtMobileError").text("Your mobile number should be of 10 digits only");
        $("#" + attrId + "errorMobileCircle").show();
        return false;
    } else {
        $("#" + attrId + "txtMobileError").hide();
        $("#" + attrId + "txtMobileError").text("");
        $("#" + attrId + "errorMobileCircle").hide();
    }
    return true;
}

//validate buyer's email
function validateBuyerEmail(attrId) {
    if (document.all && !window.atob) {
        if (buyersEmail == "Email(Optional)") {
            buyersEmail = "";
        }
    }
    if (buyersEmail == "") {
        if (attrId == "" || attrId == "rp") {
            ShakeFormView($(".optional-email, .email-ug-field"));
            if ($("#" + attrId + "emailTick").hasClass("uc-checked")) {
                $("#" + attrId + "txtEmailError").show();
                $("#" + attrId + "txtEmailError").text("Please enter your email address");
                $("#" + attrId + "errorEmailCircle").show();
                return false;
            }
        }
        else if (attrId == "pg") {
            ShakeFormView($(".optional-email, .email-ug-field"));
            if ($('#' + attrId + 'emailTick').is(':checked')) {
                $("#" + attrId + "txtEmailError").show();
                $("#" + attrId + "txtEmailError").text("Please enter your email address");
                $("#" + attrId + "errorEmailCircle").show();
                return false;
            }
        }

    } else if (!reEmail.test(buyersEmail.toLowerCase())) {
        ShakeFormView($(".optional-email, .email-ug-field"));
        $("#" + attrId + "txtEmailError").show();
        $("#" + attrId + "txtEmailError").text("Invalid email address");
        $("#" + attrId + "errorEmailCircle").show();
        return false;
    } else {
        $("#" + attrId + "txtEmailError").hide();
        $("#" + attrId + "txtEmailError").text("");
        $("#" + attrId + "errorEmailCircle").hide();
    }
    return true;
}

// Method for processing get seller details buyer request in the Buyer Process Form  
function processPurchaseInq(attrId, rank, pf, isPremium, recommendedGsdBtn) {
    abTestcookie = $.cookie("_abtest");
    if (D_carDetailsPage.recommendedCars.isRecommended) {
        if (recommendedGsdBtn) {
            profileId = recommendedGsdBtn.attr('ProfileId');
            rank = recommendedGsdBtn.closest('li').attr('rankabs');
        }
    }
    if (attrId != "ask") {
        askQuestion = "";
    }

    var slotId = D_carDetailsPage.tracking.slot.get();
    var leadData = {
        "profileId": profileId,
        "buyer": {
            "name": buyersName,
            "mobile": buyersMobile,
            "email": buyersEmail
        },
        "leadTrackingParams": {
            "originId": originId,
            "rank": rank,
            "deliveryCity": deliveryCity ? deliveryCity : undefined,
            "leadType": D_carDetailsPage.leadType.getLeadType(),
            "slotId": slotId ? slotId : "0",
        }
    };
    var isChatSms = commonUtilities.getFilterFromQS("ischatsms", window.location.search);
    if (isChatSms) {
        leadData.isChatSms = isChatSms;
        D_carDetailsPage.utils.removeParameterFromQS('ischatsms');
    }

    deliveryCity = 0;

    $.ajax({
        type: "POST",
        url: "/api/v1/stockleads/",
        headers: { 'sourceid': 1 },
        data: JSON.stringify(leadData),
        contentType: 'application/json',
        dataType: 'json',
        success: function (response) {
            if (!chatProcess.isChatLead) { // After otp-verification, hide only if it was not a chat lead.
                $('.' + attrId + 'seller-details-on-sms').hide();
                $('#' + attrId + 'buyer_form').hide();
            }
            D_carDetailsPage.isForbidden = false;
            D_carDetailsPage.isLimitExceeded = false;
            D_carDetailsPage.tracking.triggerFacebookTracking(attrId);
            D_carDetailsPage.tracking.triggerOutbrainTracking();
            D_carDetailsPage.tracking.trovitTracking();
            D_carDetailsPage.tracking.conversionTracking();
            D_carDetailsPage.tracking.adWordTracking();
            successfulResponse(response, attrId, profileId, rank, pf, isPremium, recommendedGsdBtn);
            D_carDetailsPage.tracking.gsdClickTracking(leadData.leadTrackingParams.originId);
        },
        error: function (xhr) {
            var response = JSON.parse(xhr.responseText);
            if(chatProcess.isChatLead) {
                chatUIProcess.loader.hide();
                cwTracking.trackCustomData(D_carDetailsPage.variables.getBhriguCategory(), D_carDetailsPage.variables.getChatUnVerifiedText(), 'profileId=' + $('#getsellerDetails').attr('profileid'), false);
            }
            switch (parseInt(xhr.status)) {
                case 403:
                    if (isMobileUnverified(response)) {
                        hitMobileVerificationApi(buyersMobile, D_carDetailsPage.certificate.sendOtpApiHandler);
                    }
                    else {
                        D_carDetailsPage.isForbidden = true; //PUT CONDITION FOR LIMIT EXCEED ETC. TOO
                        if (D_carDetailsPage.recommendedCars.isRecommended) {
                            D_carDetailsPage.recommendedCars.showErrorsForRecommendations(recommendedGsdBtn, response.Message);
                        } else if (attrId === 'recommendation') {
                            $("#suggestDetailsData-" + D_carDetailsPage.recommendedCars.getRankFromRecommendationsId(recommendedGsdBtn.attr('id'))).slideToggle(500);
                            D_carDetailsPage.recommendedCars.showErrors(recommendedGsdBtn);
                        }
                        else {
                            showError(attrId, response.Message);
                        }
                    }
                    break;
                default:
                    if(isMobileMismatchedResponse(response)) {
                        $.cookie("TempCurrentUser", null, { path: '/' });
                        $("#buyerform-title").removeClass("font18").addClass("font16").text("Please enter your details to view chat.");
                    }
                    else {
                        showError(attrId, response.Message);
                    }
                    break;
            }
            D_carDetailsPage.tracking.gsdClickTracking(leadData.leadTrackingParams.originId);
        }
    });
}

function isMobileUnverified(response) {
    return response.ModelState && response.ModelState.hasOwnProperty("MobileUnverified");
}

function isMobileMismatchedResponse (response) {
    return response.ModelState && response.ModelState.hasOwnProperty("MobileMismatched");
};

function showError(attrId, message) {
    if (attrId == "pg") {
        $('.' + attrId + 'seller-details-on-sms').hide();
        $('#' + attrId + 'buyer_form').hide();
        $("#pgnot_auth").show().find(".back-to-gsd-form > span").text(message);
    }
    else if (attrId == "rp") {
        $('.' + attrId + 'seller-details-on-sms').hide();
        $('#' + attrId + 'buyer_form').hide();
        D_carDetailsPage.Valuation.sellerDetails.showServerErrorMessage(message);
    }
    else if (attrId == "") {
        $('.seller-details-on-sms').hide();
        $('#buyer_form').hide();
        $("#not_auth").show().find(".back-to-gsd-form > span").text(message);
    }
    if (attrId == 'pdf') {
        D_carDetailsPage.certificate.showServerErrorMessage(message);
    }
}

function getMobileVerificationApiData(mobile) {
    var mobileVerificationApiData = {
        "sourceModule": D_carDetailsPage.utils.otpVariables.sourceModule,
        "mobileVerificationByType": D_carDetailsPage.utils.otpVariables.mobileVerificationByType.otpAndMissedCall,
        "validityInMins": D_carDetailsPage.utils.otpVariables.defaultValidityInMins,
        "otpLength": D_carDetailsPage.utils.otpVariables.defaultOtpLength
    };
    if (mobile) {
        mobileVerificationApiData["mobile"] = mobile;
    }
    return mobileVerificationApiData;
}

function hitMobileVerificationApi(mobileNumber, responseHandler) {//checks if mobile is verified and sends otp if not.
    $.ajax({
        type: 'POST',
        headers: { "sourceid": "1" },
        url: '/api/v1/mobile/' + mobileNumber + '/verification/start/',
        data: JSON.stringify(getMobileVerificationApiData()),
        contentType: 'application/json',
        dataType: 'json',
        success: function (json) {
            if (responseHandler) {
                responseHandler(json);
            }
        }
    });
}

function hitIsMobileVerifiedApi(mobileNumber, responseHandler) {//checks if mobile is verified.
    $.ajax({
        type: 'GET',
        headers: { "sourceid": "1" },
        url: '/api/v1/mobile/' + mobileNumber + '/verification/status/',
        dataType: 'Json',
        success: function (json) {
            if (responseHandler)
                responseHandler(json);
        }
    });
}

//Method for handling the Get seller details response in the Buyer Process Form  
function processPurchaseInqResponse(ds, attrId, profileId, rank, pf, isPremium, recommendedGsdBtn) {
    $('.' + attrId + 'seller-details-on-sms').hide();
    $('#' + attrId + 'buyer_form').hide();
    switch (parseInt(ds.Status)) {
        case D_carDetailsPage.buyerProcessResponseCode.success:
            D_carDetailsPage.isForbidden = false;
            D_carDetailsPage.isLimitExceeded = false;
            D_carDetailsPage.tracking.triggerFacebookTracking(attrId);
            D_carDetailsPage.tracking.trovitTracking();
            D_carDetailsPage.tracking.conversionTracking();
            D_carDetailsPage.tracking.adWordTracking();
            successfulResponse(ds, attrId, profileId, rank, pf, isPremium, recommendedGsdBtn);
            break;
        case D_carDetailsPage.buyerProcessResponseCode.invalidUser:
            $('.ask-dealer-question').hide();
            if (attrId == "pg")
                $("#pgnot_auth").show().find(".back-to-gsd-form > span").text(ds.Message);
            else if (attrId == "rp") {
                D_carDetailsPage.Valuation.sellerDetails.showServerErrorMessage(ds.Message);
            }
            else if (attrId == "")
                $("#not_auth").show().find(".back-to-gsd-form > span").text(ds.Message);
            if (attrId == 'pdf')
                D_carDetailsPage.certificate.showServerErrorMessage(ds.Message);
            break;
        case D_carDetailsPage.buyerProcessResponseCode.accessForbidden:
            D_carDetailsPage.isForbidden = true;
            if (D_carDetailsPage.recommendedCars.isRecommended) {
                D_carDetailsPage.recommendedCars.showErrorsForRecommendations(recommendedGsdBtn, ds.Message);
            }
            else {
                $('.ask-dealer-question').hide();
                if (attrId == "pg")
                    $("#pgnot_auth").show().find(".back-to-gsd-form > span").text(ds.Message);
                else if (attrId == "rp") {
                    D_carDetailsPage.Valuation.sellerDetails.showServerErrorMessage(ds.Message);
                }
                else if (attrId == "")
                    $("#not_auth").show().find(".back-to-gsd-form > span").text(ds.Message);
                if (attrId == 'pdf')
                    D_carDetailsPage.certificate.showServerErrorMessage(ds.Message);
            }
            break;
        case D_carDetailsPage.buyerProcessResponseCode.limitReached:
            D_carDetailsPage.isLimitExceeded = true;
            if (attrId == "ask") {
                $('.ask-dealer-question').hide();
                $("#not_auth_ask").show().find(".back-to-gsd-form > span").text(ds.Message);;
            } else if (attrId == "rp") {
                D_carDetailsPage.Valuation.sellerDetails.showServerErrorMessage(ds.Message);
            }
            else {
                if (D_carDetailsPage.recommendedCars.isRecommended) {
                    D_carDetailsPage.recommendedCars.showErrorsForRecommendations(recommendedGsdBtn, ds.Message);
                }
                else {
                    $("#pgbuyer_form,#pgseller-details,.pgmobile-verification,#pgtollFreeBox,#pg-captcha").hide();
                    $('#pgbackToBuyerForm').show();
                    $("#pgnot_auth").show().find(".back-to-gsd-form > span").text(ds.Message);
                    $("#" + attrId + "not_auth").show().find(".back-to-gsd-form > span").text(ds.Message);
                    D_carDetailsPage.recommendedCars.hideSellerDetailsScreen();
                    if (attrId == 'pdf')
                        D_carDetailsPage.certificate.showServerErrorMessage(ds.Message);
                }
            }
            break;

        case D_carDetailsPage.buyerProcessResponseCode.ipBlocked:
            if (attrId == "" || attrId == "ask") {
                $('#captchaCode').attr('src', '/Common/CaptchaImage/JpegImage.aspx').show();
                $('#captcha').show();
                $('.ask-dealer-question').hide();
            }
            else if (attrId == "pg") {
                $('#pg-captchaCode').attr('src', '/Common/CaptchaImage/JpegImage.aspx').show();
                $('#pg-captcha').show();
            }
            else if (attrId == 'pdf') {
                D_carDetailsPage.certificate.showServerErrorMessage('Number blocked due abnormally high usage.');
            }
            else if (attrId == "rp") {
                D_carDetailsPage.Valuation.sellerDetails.showServerErrorMessage('Number blocked due abnormally high usage');
            }
            objDetailsCaptcha.attrId = attrId
            objDetailsCaptcha.rank = rank;
            objDetailsCaptcha.pf = pf;
            objDetailsCaptcha.isPremium = isPremium;
            break;

        case D_carDetailsPage.buyerProcessResponseCode.certificationReportSuccess:
            D_carDetailsPage.isForbidden = false;
            D_carDetailsPage.isLimitExceeded = false;
            D_carDetailsPage.tracking.triggerFacebookTracking(attrId);
            D_carDetailsPage.tracking.trovitTracking();
            D_carDetailsPage.tracking.adWordTracking();
            successfulResponse(ds, attrId, profileId, rank, pf, isPremium, recommendedGsdBtn);
            break;

        case D_carDetailsPage.buyerProcessResponseCode.certificationReportNotAvailable:
            D_carDetailsPage.certificate.showServerErrorMessage(ds.Message);
    }
}

//Method to handle the response when the verification is successull
function successfulResponse(ds, attrId, profileId, rank, pf, isPremium, recommendedGsdBtn) {
    setTempCurrentUserCookie();
    sellerDetailsBtnTextChange();
    chatProcess.processChatRegistration(ds.appId, ds.buyer, chatUIProcess.setChatIconVisibilty, chatProcess.source.desktopBrowser, chatUIProcess.loader.hide);
    if (chatProcess.isChatLead) {
        chatProcess.startChat(ds.appId, $('#chat-btn-vdp'), ds.seller, ds.buyer, ds.stock, chatUIProcess.chatRegistration);
        cwTracking.trackCustomData(D_carDetailsPage.variables.getBhriguCategory(), D_carDetailsPage.variables.getChatVerifiedText(), 'profileId=' + $('#getsellerDetails').attr('profileid'), false);
    }
    else {
        if (attrId == "ask") {
            $("#" + attrId + "verifying").hide();
            $("#" + attrId + "verifyNumber").show();
            $('.mobile-verification,.ask-dealer-question,#sendingMessage,.seller-details-call').hide();
            $('.message-sent,#sendMessage').show();
        }
        else if (attrId == "pdf") {
            //hit the pdf link
            if (ds.certificationReportUrl) {
                location.href = ds.certificationReportUrl;
            }
            D_carDetailsPage.certificate.closeCertificationBox();
        }
        else {
            if (attrId == "pg") {
                $("#" + attrId + "buyer_form").hide();
            }
            $("#" + attrId + "gettingDetails").hide();
            $("#" + attrId + "getsellerDetails").show();
            $('#pgbackToBuyerForm').hide();
            prepareSellInfo(ds, attrId, profileId, rank, pf, isPremium, recommendedGsdBtn);
        }
    }
    dataLayer.push({ event: 'buyUsedCarDetails', cat: 'UsedCarDetails', act: 'VerifyZipDial' });
}

function setTempCurrentUserCookie() {
    var date = new Date();
    date.setTime(date.getTime() + (90 * 24 * 60 * 60 * 1000)); //90 days
    var cookieExpiry = date.toGMTString();
    document.cookie = "TempCurrentUser=" + buyersName + ":" + buyersMobile + ":" + buyersEmail + ":0; expires=" + cookieExpiry + "; path=/";
}

//Method to show the details when the verfication is successfull 
function prepareSellInfo(ds, attrId, profileId, rank, pf, isPremium, recommendedGsdBtn) {
    if (attrId == undefined || attrId == null) {
        attrId = "";
    }
    if (attrId != "rp") {
        D_carDetailsPage.recommendedCars.showSellerDetailsScreen();
        var seller = ds.seller;
        $("#" + attrId + "seller_name").text(seller.name);
        $("#" + attrId + "seller_email").text(seller.email);
        $("#" + attrId + "seller_mobile").text(seller.mobile);
        $("#" + attrId + "seller_address").text(seller.address);
        if (seller.dealerShowroomPage) {
            $('.seller-virtual-link').attr('href', seller.dealerShowroomPage).show();
        }
        else {
            $(".seller-virtual-link").hide();
        }
        if (_isDealer == "1") {
            $("#" + attrId + "contact_person").text(seller.contactPerson ? seller.contactPerson : "");
        } else {
            $("#" + attrId + "contact_person").closest('li').hide();
        }
        $('.' + attrId + 'seller-details-call').hide();
        $('.' + attrId + 'mobile-verification').hide();
        $('.' + attrId + 'seller-details').show();
        $('#pgbackToBuyerForm').hide();
    } else {
        D_carDetailsPage.Valuation.sellerDetails.bindSellerDetails(ds);
    }


    var node = $("#getsellerDetails");
    D_carDetailsPage.kmNumeric = node.attr("kmNumeric");
    D_carDetailsPage.priceNumeric = node.attr("priceNumeric");
    D_carDetailsPage.bodyTypeId = node.attr("bodyStyleId");
    D_carDetailsPage.versionSubSegment = node.attr("versionSubsegmentId");
    D_carDetailsPage.makeId = node.attr("makeid");
    D_carDetailsPage.rootId = node.attr("rootid");
    var stockRecommendationUrl = node.attr("stockRecommendationUrl");

    if (!D_carDetailsPage.recommendedCars.isRecommended) {
        $.ajax({
            type: 'GET',
            url: stockRecommendationUrl,
            headers: { "sourceid": "1" },
            dataType: 'json',
            success: function (jsonData) {
                var numRecommendations = jsonData.length;
                if (numRecommendations > 0) {
                    if (attrId == 'rp') {
                        D_carDetailsPage.Valuation.recommendations.bindRecommendations(jsonData);
                    } else {
                        D_carDetailsPage.recommendedCars.bindRecommendations(jsonData);
                    }
                }
                else {
                    $('#blackOut-recommendation').hide();
                }
                D_carDetailsPage.recommendedCars.showSellerDetails(ds);
            }
        });
    }
    else {
        D_carDetailsPage.recommendedCars.showSellerDetailsForRecommendations(ds, recommendedGsdBtn);
    }

    triggerRemarketingCode(attrId);
}

function triggerRemarketingCode(attrId) {
    //Google double click
    var axel = Math.random() + "";
    var a = axel * 10000000000000000;
    var spotpix = new Image();
    var idx = ModelName.indexOf('[');
    var strModel = idx == -1 ? ModelName : ModelName.substring(0, idx - 1);
    if (attrId == 'pg')
        spotpix.src = "https://ad.doubleclick.net/ddm/activity/src=4948701;type=usedc0;cat=deskt0;u1=" + MakeName.replace(/ /g, '') + ";u2=" + strModel.replace(/ /g, '') + ";u3=" + price.replace(/ /g, '') + ";u4=" + cityName.replace(/ /g, '') + ";u5=" + dealerText + ";u6=" + individualText + ";ord=" + a + "?";
    else
        spotpix.src = "https://ad.doubleclick.net/ddm/activity/src=4948701;type=usedc0;cat=deskt000;u1=" + MakeName.replace(/ /g, '') + ";u2=" + strModel.replace(/ /g, '') + ";u3=" + price.replace(/ /g, '') + ";u4=" + cityName.replace(/ /g, '') + ";u5=" + dealerText + ";u6=" + individualText + ";ord=" + a + "?";

}


function ValidateRLForm() {
    var reason = $("#ddlReason").val();
    var desc = $("#txtDescription").val();
    var email = $("#txtEmail").val();
    var emailPattern = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;
    var isError = false;

    if (reason == -1) {
        alert("Please select the reason why you want to report this listing");
        return false;
    }

    if (desc == $("#txtDescription").attr("placeholder")) {
        alert("Please enter the description");
        return false;
    } else if (desc == "") {
        alert("Please enter the description");
        return false;
    }



    if (email != null && email != "") {
        if (!emailPattern.test(email)) {
            alert("Please enter a valid email");
            return false;
        }
    }
    return true;
}

function showCharactersLeft() {
    var maxSize = 6000;
    var desc = $("#txtDescription").val();
    var size = desc.length;

    if (size >= maxSize) {
        $("#txtDescription").val(desc.substring(0, maxSize - 1));
        size = maxSize;
    }

    $("#spnDesc").html("Characters Left : " + (maxSize - size));
}



$("#btnReport").live('click', function (e) {
    e.preventDefault();
    var reason = $("#ddlReason").val();
    var desc = $("#txtDescription").val();
    var email = $("#txtEmail").val();
    if (ValidateRLForm()) {
        $("#process_img").css({ "display": "inline-block" });
        var inqType;
        if (_isDealer == "1") {
            inqType = 1;
        } else {
            inqType = 2;
        }
        $.ajax({
            type: "POST",
            url: "/ajaxpro/CarwaleAjax.AjaxClassifiedBuyer,Carwale.ashx",
            data: '{"inquiryId":"' + inquiryId + '", "inquiryType":"' + inqType + '", "reasonId":"' + reason + '", "description":"' + desc + '", "email":"' + email + '"}',
            beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "ReportListing"); },
            success: function (response) {
                var id = eval('(' + response + ')');
                var intRegex = /^\d+$/;
                if (id.value != -1) {
                    $("#reportListingfrm").hide();
                    $("#showTY").html("<span>Your complaint has been successfully recorded.</span>");
                } else {
                    $("#reportListingfrm").hide();
                    $("#showTY").html("<span>Your complaint could not be recorded. Please try again later.</span>");
                }
            }
        });
    }
});
$("#txtDescription").live('focus', function () {
    if ($("#txtDescription").val() == $("#txtDescription").attr("placeholder")) {
        $("#txtDescription").val("");
    }
}).live('keyup', function () {
    showCharactersLeft();
}).live('blur', function () {
    if ($("#txtDescription").val() == "") {
        $("#txtDescription").val($("#txtDescription").attr("placeholder"));
    } else {
        showCharactersLeft();
    }
});

function ShowRequestPhotosGrayBox() {
    var caption = "Request Photos";
    var url = "/used/RequestPhotos.aspx";
    var applyIframe = false;
    var GB_Html = "";
    GB_show(caption, url, 270, 350, applyIframe, GB_Html);
}

function submitPhotoRequest() {
    var buyerMessage = "";
    $.ajax({
        type: "POST",
        url: "/ajaxpro/CarwaleAjax.AjaxClassifiedBuyer,Carwale.ashx",
        data: '{"sellInquiryId":"' + inquiryId + '", "consumerType":"' + consumerType + '", "buyerMessage":"' + buyerMessage + '", "buyerName":"' + buyersName + '", "buyerEmail":"' + buyersEmail + '", "buyerMobile":"' + buyersMobile + '", "carName":"' + carName + '"}',
        dataType: 'json',
        beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "UploadPhotosRequest"); },
        success: function (response) {
            //boxObj.find("#buyer_form,#verifiy_mobile,#initWait").hide();
            $("#requester_form").removeClass("opacity4");
            $("#requester_form").hide();
            $("#process_img1").hide();
            $("#gb-title").text("It's Done");
            $("#photos_req_msg").fadeIn(500);
            if (response.value) {
                isPhotoReqDone = response.value;
                $("#reqPhotos").fadeOut(500);
            } else {
                $("#gb-title").text(" ");
                $("#photos_req_confirm").html("Error in submitting request to the seller. Please try again.");
            }
        }
    });
}

function regenerateCode(captchaFlag) {
    if (captchaFlag == 1) {
        $('#captchaCode').attr("src", "/Common/CaptchaImage/JpegImage.aspx");
        $('#txtCaptchaCode').val("");
        $('#lblCaptcha').hide();
        $('#lblCaptcha').next().hide();
        if (objDetailsCaptcha.attrId != "ask")
            objDetailsCaptcha.attrId = "";
    }
    else if (captchaFlag == 2) {
        $('#pg-captchaCode').attr("src", "/Common/CaptchaImage/JpegImage.aspx");
        $('#pg-txtCaptchaCode').val("");
        $('#pg-lblCaptcha').hide();
        $('#pg-lblCaptcha').next().hide();
        objDetailsCaptcha.attrId = "pg";
    }
}

$.coachmarkcookie = function () {
    if (isCookieExists('UsedCarsCoachmark1')) {
        var cookie = $.cookie('UsedCarsCoachmark1');
        if (cookie.indexOf('details') == -1) {
            SetCookieInDays('UsedCarsCoachmark1', $.cookie('UsedCarsCoachmark1') + "details", 30);
            $.showCoachMark();
        }
    }
    else {
        SetCookieInDays('UsedCarsCoachmark1', "details" + '|', 30);
        $.showCoachMark();
    }
};

$.showCoachMark = function () {
    var detailsCoachmark = $('div .details-coachmark');
    $(window).load(function () {
        detailsCoachmark.delay(2000).fadeIn(1000);
    });
    $('#detCoachmark').click(function () {
        detailsCoachmark.hide();
    });
};

$.thumbnail = function () {
    var imgHolder = $('.uc-thumbnail');
    var img = $('.uc-thumbnail img');
    var imgHeight = $('.uc-thumbnail img').height();
    var applyPosition = imgHeight - 348;
    if (imgHeight > 348) {
        img.css({ 'position': 'relative', 'top': -applyPosition / 2 });
        imgHolder.addClass('block');
    }

};
$.fetchSimilarCars = function (input) {
    $.ajax({
        url: input,
        type: 'GET',
        contentType: 'application/json; charset=utf-8',
        beforeSend: function (xhr) { xhr.setRequestHeader('sourceid', '1'); },
        success: function (response) {
            D_carDetailsPage.similarCars.bindSimilarCars(response);
        }
    });
};
$(".blackOut-window").on("click", function () {
    D_carDetailsPage.Finance.closeFinanceForm();
});

$('.email-update-field').click(function (e) {
    var checkbox = $(this).find('span.cw-used-sprite');

    checkbox.toggleClass('uc-checked');
    $(this).closest('.form__user-details').find('.optional-email').toggle();

    if (checkbox.hasClass('uc-checked')) {
        $('#txtEmail').focus();
    }
    else {
        $('#txtName').focus();
    }
});

var chatUIProcess = function () {
    var registerEvents = function () {
        D_carDetailsPage.doc.on('click', '.view-details-chat', function () {
            chatProcess.isChatLead = true;
            D_carDetailsPage.recommendedCars.isRecommended = false;
            var recommendedGsdBtn = $(this).parent().find('a.view-details');
            var recommendedCarRank = D_carDetailsPage.recommendedCars.getRankFromRecommendationsId(recommendedGsdBtn.attr('id'));
            var recommendedCarTrackingRank = D_carDetailsPage.recommendedCars.getRecommendedCarTrackingRank(recommendedCarRank);
            D_carDetailsPage.recommendedCars.setOriginId();

            if (D_carDetailsPage.isForbidden || D_carDetailsPage.isLimitExceeded) {
                D_carDetailsPage.recommendedCars.processRecommendationsErrors(recommendedCarRank, recommendedGsdBtn)
            }
            else {
                chatUIProcess.loader.show($(this).find('.chat-btn'));
                var gsDetailsPageBtn = $('#getsellerDetails');
                processPurchaseInq("recommendation", recommendedCarTrackingRank, gsDetailsPageBtn.attr('data-platform'), gsDetailsPageBtn.attr('data-ispremium'), recommendedGsdBtn);
            }
            D_carDetailsPage.recommendedCars.trackRecommendationAction(recommendedGsdBtn);
        });

        $(document).on('ready', function () {
            chatProcess.getChatHtml(function (isMyChatsVisible, chatHtml) {
                $("#chatPopup").html(chatHtml);
                if (isMyChatsVisible) {
                    $('.global-chat-icon').removeClass('hide');
                }
            }, chatUIProcess.setChatIconVisibilty, chatProcess.source.desktopBrowser, chatUIProcess.loader.hide);
        });

        $('#gsdChat').on('click', function () {
            var $chatBtn = $(this);
            _processCommonChatAction($chatBtn, $chatBtn.parent().find('#getsellerDetails'));
        });

        $('#rpGSDChat').on('click', function () {
            var $chatBtn = $(this);
            _processCommonChatAction($chatBtn, $chatBtn.parent().find('#rpgetsellerDetails'));
        });

        $('#pgGSDChat').on('click', function () {
            var $chatBtn = $(this);
            _processCommonChatAction($chatBtn, $chatBtn.parent().find('#pggetsellerDetails'));
        });
    }

    var _processCommonChatAction = function ($chatBtn, $gsdBtn) {
        chatProcess.isChatLead = true;
        chatUIProcess.loader.show($chatBtn.find('.chat-btn'));
        var attrId = getAttrId($gsdBtn)
        processGSDClick($gsdBtn, attrId);
    }

    var triggerChat = function () {
        if ($('#photoGallery').is(':visible') && $('#photoGallery').css('visibility') != 'hidden') {
            $('#pgGSDChat').trigger('click');
        }
        else if ($('#valuation').is(':visible')) {
            $('#rpGSDChat').trigger('click');
        }
        else {
            $('#gsdChat').trigger('click');
        }
    }

    var setChatIconVisibilty = function (responseMsg, count) {
        if (responseMsg === chatProcess.chatRegistrationResponse.Success) {
            $('.global-chat-icon').removeClass('hide');
            if (count > 0) {
                $('.global-chat-btn .chat-icon').addClass('global-chat');
                var countText = count > 99 ? '99+' : count;
                $('#chat-count').text(countText);
            }
            else if (count === 0) {
                $('#chat-count').empty();
                $('.global-chat-btn .chat-icon').removeClass('global-chat');
            }
        }
        else {
            $('.global-chat-icon').addClass('hide');
        }
    };

    var loader = (function () {
        var _container;
        var show = function ($container) {
            _container = $container;
            $container.find('.js-threedot-loader').hide();
            $container.find('.oxygenLoaderContainer__js').show();
        }
        var hide = function () {
            if(_container) {
                _container.find('.js-threedot-loader').show();
                _container.find('.oxygenLoaderContainer__js').hide();
                _container = undefined;
            }
        }
        return {show: show, hide: hide};
    })();

    return { setChatIconVisibilty: setChatIconVisibilty, registerEvents: registerEvents, triggerChat: triggerChat, loader: loader};
}();

var leadData = (function () {
    var _gsdNode;
    var setGsdNode = function (gsdNode) {
        if (gsdNode) {
            _gsdNode = gsdNode;
        }
    };
    var getGsdNode = function () {
        return _gsdNode;
    };
    return {
        setGsdNode: setGsdNode,
        getGsdNode: getGsdNode,
    }
})();
