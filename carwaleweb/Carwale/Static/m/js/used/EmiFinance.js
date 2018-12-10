var emiFinance = (function () {
    var financeUrl = '';
    var iframeTimeout;
    function financeClickHandler(event) {
        event.stopPropagation();
        event.preventDefault();
        $('div.popup-loading-pic').removeClass('hideImportant');
        var $node = $(this).parents('li').find('a.getSellerDetails');
        try {
            if (!isIphone) {
                Common.utils.lockPopup();
                financeUrl = $(this).data("href");
                var IFrameDiv = $("#iframecontent");
                $('#financeIframe').show('slide', { direction: 'right' }, 500, function () {
                    if (IFrameDiv.find($("iframe")).length > 0) {
                        $("iframe").remove();
                    }
                    var iframe = $("<iframe></iframe>").attr({
                        "src": financeUrl,
                        "width": "100%",
                        "height": "100%"
                    });
                    iframe.appendTo(IFrameDiv);
                    iframeTimeout = setTimeout(function () {
                        $("#iframeTimeOut").show();
                        $('div.popup-loading-pic').removeClass('hideImportant');
                    }, 20000);
                    iframe.load(function () {
                        clearTimeout(iframeTimeout);
                        window.addEventListener('message', receiveMessage, false);
                        $('div.popup-loading-pic').addClass('hideImportant');
                        window.history.pushState("cartrade", "", "");
                        $("div.detail-ui-corner-top").css('display', 'none');
                        Common.utils.unlockPopup();
                        $("#iframecontent").show();
                    });
                });
            } else {
                var qs = $(this).data("href").split('?')[1] + "&profileId=" + $node.attr("profileid");
                window.open("/m/used/finance.aspx?" + qs, "_blank");
            }
        } catch (err) { console.log("Some error occured while opening iframe :" + err.message); }
    };

    function receiveMessage() {
        var origin = event.origin || event.originalEvent.origin;
        if (financeUrl.indexOf(origin) >= 0 && event.data.toLowerCase() === 'true') {
            closeFinanceForm();
            window.removeEventListener('message', receiveMessage, false);
        }
    }

    function closeFinanceForm() {
        clearTimeout(iframeTimeout);
        $("#iframecontent").empty();
        $("#iframeTimeOut").hide();
        $('#financeIframe').hide(slider.effect, slider.options, slider.duration);
        Common.utils.unlockPopup();
        if ($('#carDetails, div.detail-ui-corner-top').is(':visible'))
            $("div.detail-ui-corner-top").css('display', 'block');
    };

    return { closeFinanceForm: closeFinanceForm, financeClickHandler: financeClickHandler };
})();
$(document).ready(function () {
    $(document).on("click", '.getFinance', emiFinance.financeClickHandler);
    $(document).on("click", "#FinanceFormBack,#iframeTimeOutClick", emiFinance.closeFinanceForm);
});