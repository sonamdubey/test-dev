rightPrice = function () {
        var _trackingCategory = "UsedSearchPage";
        var $valuationResultSectionObj = $('.valuation-result-section');
        var $resultContentObj = $('.valuation-result-content');
        var triggerValuationClick = function (event) {
            event.stopPropagation();
            event.preventDefault();
            var valuationLink = $(this);
            history.pushState('valuation Pop Up', '', '');
            $resultContentObj.empty();
            $valuationResultSectionObj.show('slide', { direction: 'right' }, 500);
            Common.utils.showLoading();
            $resultContentObj.load($(this).data('href'), function (response, status) {
                Common.utils.hideLoading();
                var $gsdButton = $(valuationLink).parents('li').find('.getSellerDetails');
                var tempOriginId = $gsdButton.attr('profileId') ? m_bp_process.originId.searchPageRightPrice : m_bp_process.originId.detailsPageRightPrice;
                $gsdButton = $gsdButton.attr('profileId') ? $gsdButton : $("#getSellerDetailsBtn .getSellerDetails");
                Common.utils.lockPopup();
                _trackClick(valuationLink);
                if (status == 'error') {
                    $resultContentObj.html("Something went wrong. Please try again later");
                }
                // transfer gsd button attributes to valution popup
                var $gsdButtonValuation = $("#valuation-seller-details #getsellerDetails");
                if ($gsdButton && $gsdButtonValuation) {
                    $gsdButton.each(function () {
                        $.each(this.attributes, function () {
                            var name = this.name;
                            if (name != 'class' && name != 'data-bind') {                               
                                $gsdButtonValuation.attr(name, this.value);
                            }
                        });
                    });
                    $gsdButtonValuation.attr('oid', tempOriginId);
                    // change text of button if user is verified
                    if (typeof m_bp_additonalFn != 'undefined')
                        m_bp_additonalFn.sellerDetailsBtnTextChange();
                } else {
                    $gsdButtonValuation.hide();
                }
            });
        },
        triggerPopUpBackArrow = function () {
            $("#getSellerDetailsBtn").hide();
            history.back();
        },        
        _trackClick = function (valuationLink) {
            var trackingParam = {};
            trackingParam['profileId'] = valuationLink.attr('profileid');
            trackingParam['caseId'] = $(".right-price-box").attr("caseid");
            var rightPriceTrackingData = cwTracking.prepareLabel(trackingParam);
            var trackingPage = $('#carDetails, div.detail-ui-corner-top').is(':visible') ? "UsedDetailsPage" : _trackingCategory;
            cwTracking.trackCustomData(trackingPage, 'RightPriceClick', rightPriceTrackingData, true);
        }
    
        return { triggerValuationClick: triggerValuationClick, triggerPopUpBackArrow: triggerPopUpBackArrow };
}();
$(document).ready(function () {
    $(document).on('click', '.view-market-price',rightPrice.triggerValuationClick);
    $(document).on('click', '#evaluateResultPopupBackArrow', rightPrice.triggerPopUpBackArrow);
});