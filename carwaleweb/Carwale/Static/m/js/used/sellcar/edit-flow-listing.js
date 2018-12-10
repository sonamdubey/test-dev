var EditCarListing = {
    editSellcarDocReady: function () {
        EditCarListing.setSelectors();
        EditCarListing.registerEvents();
    },
    cachedInquiryId: '',// used for caching inquiries recieved
    s3fetchUrl: '',
    //Variables declared for selectors
    setSelectors: function () {    
        EditCarListing.stopAdPopup = '.stop-showing-ad-popup-container';
        EditCarListing.reasonsInputBox = '#reasonsInputBox';
        EditCarListing.reasonsSelectBox = '#getReason';
        EditCarListing.modalBg = '#modalBg';
        EditCarListing.adBottomSlidePopup = '.ad-bottom-slide-popup';
        EditCarListing.stopAdContainer = '.stop-showing-ad-popup-container';
        EditCarListing.acknwoledgementSlider = '.acknowledgement-msg-card';
        EditCarListing.modalBgLayout = '.modal-bg-layout';
    },

    //All events for the selectors
    registerEvents: function () {      
        $(document).on('click', '#remove-car', function () {
            var inqId = $(this).attr('data-listingid');
            if (EditCarListing.reasonSelectionVal()) {
                editCarCommon.setLoadingScreen();
                var selectedVal = $("#getReason option:selected").val();
                var comments = $("#comment").val();
                var settings = {
                    url: '/used/mylistings/' + inqId + '/?authToken=' + encodeURIComponent($.cookie('encryptedAuthToken')),
                    type: "PATCH",
                    data: {
                        statusid: selectedVal,
                        deletecomments: comments ? comments.trim() : ""
                    }
                }
                $.ajax(settings).done(function (response) {
                    $('.mylistingContainer').empty();
                    EditCarListing.resetStopAdPopUp();
                    //DO NOT BIND html before cloing pop up
                    //It will break the pop up container
                    $('.mylistingContainer').html(response);
                    editCarCommon.removeLoadingScreen();
                }).fail(function (xhr) {
                    EditCarListing.resetStopAdPopUp();
                    editCarCommon.removeLoadingScreen();
                    editCarCommon.showModal(xhr.responseText);
                });
                iphoneInputFocus.OutFocus();

            }
            else {
                field.setError($(EditCarListing.reasonsSelectBox), 'Please choose a reason to continue');
            }
        });
        $(document).on('click', '.mylistingContainer .modal-box .modal__close', function () {
            history.back();
            $('body').removeClass('lock-browser-scroll');
        });
        $(window).on('popstate', function () {
            if ($(EditCarListing.acknwoledgementSlider).hasClass('ack-slider-active')) {
                EditCarListing.hideAcknowledgementSlider();
            }
            else if ($('#modalPopUp').children().hasClass('stop-showing-ad-popup-container')) {
                EditCarListing.resetStopAdPopUp();
            }
            else if ($(EditCarListing.adBottomSlidePopup).hasClass("expandedBottom"))
            {
                EditCarListing.hideAdDetailList();
            }
            else if (editCarCommon.isVisible()) {
                editCarCommon.hideModal();
            }
            else if(location.pathname=="/used/mylistings/search/")
            {
                window.location.href = "/used/mylistings/search/";
            }
            if (Common.utils.getValueFromQS("editable")) {
                EditCarListing.removeDisabledEditPopup();
            }
        });

        $(document).on('click', '.stop-ad-link', function () {
            var inqId = $(this).attr('data-listingid');
            $('#modalPopUp').attr("data-current", "stop-showing-ad-popup-container");
            $('#remove-car').attr("data-listingid", inqId);
            editCarTracking.trackForMobile(editCarTracking.actionType.stopAdd, inqId);
            popUp.showPopUp(setTimeout(function () {
                if ($('.form-control-box').find('.chosen-container').length > 0) {
                    $(".chosen-select").next('').remove();
                }
                ChosenInit($('.stop-showing-ad-popup-container'));
            }, 0));

            iphoneInputFocus.OnFocus();
            history.pushState('stopAdPopup','');
        });

        $(document).on('click', '.js-delete-my-ad', function () {
            var inqId = $(this).attr('data-listingid');
            editCarCommon.setLoadingScreen();
            editCarTracking.trackForMobile(editCarTracking.actionType.deleteAdd, inqId);
            var settings = {
                url: '/used/mylistings/' + inqId + '/?authToken=' + encodeURIComponent($.cookie('encryptedAuthToken')),
                type: "PATCH",
                data: {
                    isarchived: true
                }
            }
            
            $.ajax(settings).done(function (response) {
                $('.mylistingContainer').empty();
                $('.mylistingContainer').html(response);
                editCarCommon.removeLoadingScreen();
            }).fail(function (xhr) {
                editCarCommon.removeLoadingScreen();
                editCarCommon.showModal(xhr.responseText);
            });
            iphoneInputFocus.OutFocus();
        });

        $(document).on('click', '.close-icon', function () {
            iphoneInputFocus.OutFocus();
            if ($(EditCarListing.acknwoledgementSlider).hasClass('ack-slider-active')) {
                EditCarListing.hideAcknowledgementSlider();
            }
            else if ($(EditCarListing.adBottomSlidePopup).hasClass("expandedBottom")) {
                EditCarListing.hideAdDetailList();
            }
            else
            {
                EditCarListing.resetStopAdPopUp();
            }
            if (Common.utils.getValueFromQS("editable")){
                EditCarListing.removeDisabledEditPopup();
            }
            $('body').removeClass('lock-browser-scroll');
        });
        $(document).on('click', '#view-ad-detail', function () {
           EditCarListing.viewAdDetailList($(this).attr('data-listingid'));
        });
        $(document).on('click', '.modal-bg-layout, #ad-inq-close', function () {
            history.back();
        });
        $(document).on('change', EditCarListing.reasonsSelectBox, function () {
            EditCarListing.removeReasonValidationError();
        });
        $(document).on('click', '.edit-listing-link', function () {
            var inquiryId = $(this).attr('data-listingid');
            var url = "/used/mylistings/" + inquiryId + "?authToken=" + $.cookie('encryptedAuthToken');
            editCarTracking.trackForMobile(editCarTracking.actionType.editCar, "EditCar");
            window.location.assign(url);
        });

           
        $(document).on('click', '#cancel-reason', function () {
            iphoneInputFocus.OutFocus();
            EditCarListing.resetStopAdPopUp();
        });

        // event listeners for Ackwolegdement slider
        $(document).on('click', '.cross-btn', function () {
            EditCarListing.hideAcknowledgementSlider();
        });
        // end

        $(document).on('click', '#downloadReceipt', function () {
            var apiUrl = this.data("url");
            var inquiryId = this.data("listingid");
            var key = this.data("pdfkey");
            var expiredInMins = 1000;
            var receipturl = fetchReceiptAwsUrl(apiUrl, key, expiredInMins);
            if (receipturl) {
                window.open(receipturl);
            }
            
        });

        if ($('.sellcar-edit-list-container').length)
        {
            editCarTracking.trackForMobile(editCarTracking.actionType.listingViewLoad, "ListingViewLoad");
        }
        $('.success-msg__btn-close').on('click', function () {
            $('.success-msg__block').hide();
            if (typeof (clientCache) != undefined) {
                clientCache.remove('congratsSlug', true);
            }
        });
        // take user to plans screen on click of upgrade link or button on congrats slider
        $(document).on('click', '#upgrade', function (e) {
            e.preventDefault();
            var inquiryId = $(this).attr('data-inquiryid');
            var qs = 'upgrade=true' + ((inquiryId) ? ('&inquiryId=' + inquiryId) : '');
            location = '/used/sell/plans/?'+qs+'&isPartial=false&type=2&value=' + inquiryId;
        });

        if (typeof (clientCache) != undefined) {
            var returnValue = clientCache.get('congratsSlug', true);
            if (returnValue && returnValue.id) {
                $('.success-msg__block').show();
                setTimeout(function () {
                    $('.success-msg__block').fadeOut("slow", function () {
                        clientCache.remove('congratsSlug', true);
                    });
                }, 3000);
            }
        }
        if (Common.utils.getValueFromQS("editable")) {
            editCarCommon.setLoadingScreen();
            setTimeout(function () {
                $(EditCarListing.modalBg).show();
                $('#modalPopUp').attr("data-current", "disableEditListing");
                $("#modalPopUp").append('<div class="cross-default-15x16 cross-lg-dark-grey rightfloat close-icon"></div><div style="position:relative;top:1em;font-weight:700">Editing Disabled</div><span style="position:relative;top:2em">Since this car has been inspected, editing is disabled. To edit, please call on <a style="color: #00afa0" href="tel:+918530482263"</a>+91-8530482263</span>').show();
                history.pushState('disableEditListing', '');
                editCarCommon.removeLoadingScreen();
            }, 1000);
        }
    },

    // show acknowledgement slider
    showAcknowledgementSlider : function(){
        var paymentStatus = Common.utils.getValueFromQS("payst");
        if (paymentStatus && $("[data-payst='" + paymentStatus + "']").length) {
            setTimeout(function () { $(EditCarListing.modalBgLayout).show(); }, 0);
            $("[data-payst='" + paymentStatus + "']").removeClass('closed').addClass('ack-slider-active');
            history.pushState("ackslidershown", null, null);
        }
    },
    // hide acknowledgement sreen, replace url to avoid browser back
    hideAcknowledgementSlider: function () {
        var url = EditCarListing.utils.removeFromQS("payst", window.location.href);
        history.replaceState(null, document.title, url);
        location.reload();
    },
    resetStopAdPopUp: function(){
        formField.emptySelect($(EditCarListing.reasonsSelectBox));
        $('.js-comment-box').val('');
        EditCarListing.removeReasonValidationError();
        $(".select-box").removeClass('done');
        popUp.hidePopUp();
    },

    //Function for validating Reason selection
    reasonSelectionVal: function () {
        if ($(EditCarListing.reasonsSelectBox).val() !== "0") {
            return true;
        }
        
        return false;
    },

    removeReasonValidationError: function () {
        field.hideError($(EditCarListing.reasonsSelectBox));
    },
    removeDisabledEditPopup: function () {
        var url = EditCarListing.utils.removeFromQS("editable", window.location.href);
        $('#modalPopUp').empty();
        window.history.replaceState(null, document.title, url);
        if (!document.referrer){
            history.pushState("editFromAutoLogin", null, null);           
        }
        history.back();
    },


    //Function to view Ad Details popup
    viewAdDetailList: function (inquiryId) {
      setTimeout(function () {
            if (inquiryId != EditCarListing.cachedInquiryId) {
                $(EditCarListing.adBottomSlidePopup).empty();
                $(EditCarListing.modalBg).show();
                var platform = $('.mylistingContainer').attr('data-platform').trim();
                var url = "/used/mylistings/" + inquiryId + "/inquiries/?platform=" + platform + "&authtoken=" + $.cookie('encryptedAuthToken');
                editCarCommon.setLoadingScreen();
                $(EditCarListing.adBottomSlidePopup).load(url, function (response, status) {
                    editCarCommon.removeLoadingScreen();
                    if(status == 'error')
                    {
                        EditCarListing.hideAdDetailList();
                        editCarTracking.trackForMobile(editCarTracking.actionType.viewInq, inquiryId + "|" + "N");
                        editCarCommon.showModal(response);
                        return;
                    }
                    $(EditCarListing.adBottomSlidePopup).addClass('expandedBottom');
                    editCarTracking.trackForMobile(editCarTracking.actionType.viewInq, inquiryId + "|" + "Y");
                    EditCarListing.cachedInquiryId = inquiryId;
                });
            }
            else
            {
                $(EditCarListing.adBottomSlidePopup).addClass('expandedBottom');
                $(EditCarListing.modalBg).show();
            }
        },100)       
        scrollLockFunc.lockScroll();
        history.pushState('viewInquiries', '');
    },

    removeCachedInquiry: function()
    {
        EditCarListing.cachedInquiryId = "";
    },
    //Function to hide Ad Details popup
    hideAdDetailList: function () {
        $(EditCarListing.adBottomSlidePopup).removeClass('expandedBottom');
        $(EditCarListing.modalBg).hide();
       scrollLockFunc.unlockScroll();
    },
    utils:{
        removeFromQS : function(name, url) {           
                var prefix = name + '=';
                var pars = url.split(/[&;]/g);
                for (var i = pars.length; i-- > 0;) {
                    if (pars[i].indexOf(prefix) > -1) {
                        pars.splice(i, 1);
                    }
                }
                url = pars.join('&');
                return url;
        }
    },
    fetchReceiptAwsUrl: function (url, key, expiredInMins) {
        if (key != "" && url) {
            return $.ajax({
                type: "GET", url: url, dataType: 'json',
                async: false, headers: { 'key': key, expiredInMins: expiredInMins }
            }).done(function (response) {
                if (response != null && response != "") {
                    return response;
                }
            });
        }
    }
}

$(document).ready(function () {
    var authCookie = document.cookie.match(/encryptedAuthToken/g);
    if (authCookie && authCookie.length > 1) {
        document.cookie = 'encryptedAuthToken' + '=; expires=Thu, 01-Jan-70 00:00:01 GMT;path=/used/mylistings;domain=' + document.domain;
        document.cookie = 'encryptedAuthToken' + '=; expires=Thu, 01-Jan-70 00:00:01 GMT;path=/used/mylistings;domain=' + document.domain.substring(document.domain.indexOf('.'));
    }
    EditCarListing.editSellcarDocReady();
    EditCarListing.showAcknowledgementSlider();    
});