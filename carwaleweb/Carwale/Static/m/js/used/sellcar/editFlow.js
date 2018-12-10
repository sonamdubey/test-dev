var EditCarFlow = {
    searchValue: '',
    listingContainer: '',
    userSearchType: {
        mobile: 1,
        profileId: 2
    },
    editSellcarDocReady: function () {
        EditCarFlow.setSelectors();
        EditCarFlow.registerEvents();
        editCarTracking.trackForMobile(editCarTracking.actionType.searchPageLoad, "SearchPageLoad");
    },

    //Variables declared for selectors
    setSelectors: function () {
        EditCarFlow.MobSearch = '#MobSearch';
        EditCarFlow.ProIdSearch = '#ProIdSearch';
        EditCarFlow.submitEditSearch = '#submitEditSearch';
        EditCarFlow.resendText = '.resend-text';
        EditCarFlow.counterText = '.counter-text';
        EditCarFlow.reqOtp = '#req-otp';
        EditCarFlow.getOtp = '.get-otp';
        EditCarFlow.searchResult = '.searchresult';
        EditCarFlow.editSearchForm = '.edit-search-form';
        EditCarFlow.listingContainer = '.sellcar-edit-list-container';
    },

    //All events for the selectors
    registerEvents: function () {
        $(document).on("click", EditCarFlow.submitEditSearch, function () {
            EditCarFlow.submitEditSearchfunc();
        });
        $('.input-box input').blur(function () {
            tmpval = $(this).val();
            if (tmpval.length > 0) {
                $(this).parent().addClass('done');
            }
            else {
                $(this).parent().removeClass('done');
            }
        });
        $(document).on('click', '.button-tab li', function () {
            $(EditCarFlow.MobSearch).focus();
            $(EditCarFlow.ProIdSearch).focus();
        });

        $(document).on("click", EditCarFlow.resendText, function () {
            if(!$('.timeBox').hasClass('active'))                
             EditCarFlow.onResendTextClick();
        
        });

        $(document).on("click", "#verify-otp", function () {
            var otpText = $(EditCarFlow.getOtp).val();
            if (userMobNo.userOTP($(EditCarFlow.getOtp))) {
                editCarCommon.setLoadingScreen();
                EditCarFlow.fetchMyListings(EditCarFlow.searchType, EditCarFlow.searchValue, otpText)
                    .done(function (response) {
                        $(EditCarFlow.searchResult).html(response);
                        $(EditCarFlow.searchResult).removeClass('edit-otp-form');
                        if (EditCarFlow.searchType === EditCarFlow.userSearchType.profileId && $(EditCarFlow.listingContainer).length) {
                            EditCarFlow.bringSearchedListingToTop(EditCarFlow.searchValue);
                        }
                        if ($(EditCarFlow.listingContainer).length)
                        {
                            $(EditCarFlow.searchResult).removeClass('edit-otp-form');
                            editCarTracking.trackForMobile(editCarTracking.actionType.listingViewLoad, "ListingViewLoad");
                        }
                        else
                        {
                            $(EditCarFlow.searchResult).addClass('edit-otp-form');
                        }
                        var url = $(EditCarFlow.listingContainer).length ? "/used/mylistings/?type=" + EditCarFlow.searchType + "&value=" + EditCarFlow.searchValue + "&isredirect=true" + "&authtoken=" + $.cookie("encryptedAuthToken") : null;
                        if (!history.state)
                            historyObj.addToHistory("landingPage", "", url);
                        else
                            historyObj.replaceHistory("landingPage", "", url);
                        editCarCommon.removeLoadingScreen();
                        editCarTracking.trackForMobile(editCarTracking.actionType.otpVerified, EditCarFlow.searchValue);
                    })
                    .fail(function (xhr) {
                        editCarCommon.removeLoadingScreen();
                        if (xhr.status === 403) {
                            field.setError($(EditCarFlow.getOtp), "Invalid OTP");
                        }
                        else
                        {
                            editCarCommon.showModal(xhr.responseText);
                        }
                    });
            }
            else {
                $(EditCarFlow.getOtp).focus();
            }

        });
        $(document).on("click", ".back-arrow-unit", function () {
            history.back();
        });
        $(document).on('click', '.modal-box .modal__close', function () {
            history.back();
        });
    },
    //Function on submit of form
    submitEditSearchfunc: function () {

        if ($('#EditMobile').is(':visible')) {
            EditCarFlow.searchType = EditCarFlow.userSearchType.mobile;
            EditCarFlow.searchValue = $(MobSearch).val();
            if (!userMobNo.userMobile($(MobSearch))) {
                $(EditCarFlow.MobSearch).focus();
                return;
            }
            
        }
        else {
            EditCarFlow.searchType = EditCarFlow.userSearchType.profileId;
            EditCarFlow.searchValue = $(EditCarFlow.ProIdSearch).val();
            $(EditCarFlow.ProIdSearch).focus();
            if (!userMobNo.userProfileId($(EditCarFlow.ProIdSearch))) {
                return;
            }
        }
        editCarTracking.trackForMobile(EditCarFlow.searchType === EditCarFlow.userSearchType.mobile ? editCarTracking.actionType.mobileSearch : editCarTracking.actionType.profileSearch, EditCarFlow.searchValue);
        editCarCommon.setLoadingScreen();
        EditCarFlow.fetchMyListings(EditCarFlow.searchType, EditCarFlow.searchValue, '', $.cookie("encryptedAuthToken")).done(function (response) {
            EditCarFlow.bindListings(response);
            editCarCommon.removeLoadingScreen();
        }).fail(function (xhr) {
            editCarCommon.removeLoadingScreen();
            if (xhr.status === 404) {
                field.setError($(EditCarFlow.ProIdSearch), "Profile Id not found or deleted");
            }
            else
            {
                editCarCommon.showModal(xhr.responseText);
            }
        });
    },
    bindListings:function(response){
        $(EditCarFlow.searchResult).html(response);
        $(EditCarFlow.editSearchForm).hide();
        if (EditCarFlow.searchType === EditCarFlow.userSearchType.profileId && $(EditCarFlow.listingContainer).length) {
            EditCarFlow.bringSearchedListingToTop(EditCarFlow.searchValue);
        }
        if ($(EditCarFlow.listingContainer).length)
        {
            $(EditCarFlow.searchResult).removeClass('edit-otp-form');
            editCarTracking.trackForMobile(editCarTracking.actionType.listingViewLoad, "ListingViewLoad");
        }
        else
        {
            $(EditCarFlow.searchResult).addClass('edit-otp-form');
            editCarTracking.trackForMobile(editCarTracking.actionType.otpScreenLoad, EditCarFlow.searchValue);
        }
        $(EditCarFlow.searchResult).show();
        $(EditCarFlow.getOtp).focus();
        if (secTimer.interval != undefined) {
            clearInterval(secTimer.interval);
        }
        secTimer.counterOn(30);
        var url = $(EditCarFlow.listingContainer).length ? "/used/mylistings/?type=" + EditCarFlow.searchType + "&value=" + EditCarFlow.searchValue + "&isredirect=true" + "&authtoken=" + $.cookie("encryptedAuthToken") : null;
        if (!history.state)
            historyObj.addToHistory("landingPage","",url);
        else
            historyObj.replaceHistory("landingPage","",url);
    },
    bringSearchedListingToTop: function (profileId) {
        var listingContainer = $(EditCarFlow.listingContainer);
        var listing = listingContainer.children("#" + profileId.toUpperCase());// profile or listing to be moved to top
        listingContainer.prepend(listing); // move listing to top
    },
    fetchMyListings: function (type, value, otpCode, authToken, platformsrc) {
        var platform = platformsrc || $('.edit-search-form').attr('data-platform');
        var param = {
            type: type,
            value: value,
            otpCode: otpCode || '',
            authToken: authToken || '',
            platform: platform || '',
        }
        var settings = {
            url: '/used/mylistings/',
            type: "GET",
            data: param
        }
        return $.ajax(settings);
    },

    //Function of resend otp
    onResendTextClick: function () {
        clearInterval(secTimer.interval);
        secTimer.counterOn(30);
        $(EditCarFlow.getOtp).val('').focus();
        if ($(EditCarFlow.resendText).hasClass('counter-hidden')) {
            $(EditCarFlow.resendText).removeClass('counter-hidden');
            $(EditCarFlow.counterText).show();
            $(EditCarFlow.getOtp).val('').focus();
            if (secTimer.interval != undefined) {
                clearInterval(secTimer.interval);
            }
            secTimer.counterOn(30);
            EditCarFlow.fetchMyListings(EditCarFlow.searchType, EditCarFlow.searchValue).fail(function (xhr) {
                editCarCommon.showModal(xhr.resendText);
            });
        }
        else {
            event.preventDefault();
        }
        editCarTracking.trackForMobile(editCarTracking.actionType.otpResent, EditCarFlow.searchValue);
    },
}
$(window).on('popstate', function () {
    //this listing page pop need a way to move to edit-flow-listing.js
    if (history.state && history.state.currentState == 'landingPage' && $('#modalPopUp').children().hasClass('stop-showing-ad-popup-container')) {
        EditCarListing.resetStopAdPopUp();
    }
    else if (history.state && history.state.currentState == 'landingPage' && $(EditCarListing.adBottomSlidePopup).hasClass("expandedBottom"))
    {
        EditCarListing.hideAdDetailList();
    }
    else if (history.state && history.state.currentState == 'landingPage' && editCarCommon.isVisible())
    {
        editCarCommon.hideModal();
    }
    else if (!history.state) {
        $(EditCarFlow.editSearchForm).show();
        $(EditCarFlow.searchResult).hide();
        EditCarListing.removeCachedInquiry();
    }
});

$(document).ready(function () {
    var authCookie = document.cookie.match(/encryptedAuthToken/g);
    if (authCookie && authCookie.length > 1) {
        document.cookie = 'encryptedAuthToken' + '=; expires=Thu, 01-Jan-70 00:00:01 GMT;path=/used/mylistings;domain=' + document.domain;
        document.cookie = 'encryptedAuthToken' + '=; expires=Thu, 01-Jan-70 00:00:01 GMT;path=/used/mylistings;domain=' + document.domain.substring(document.domain.indexOf('.'));
    }
    EditCarFlow.editSellcarDocReady();
});

