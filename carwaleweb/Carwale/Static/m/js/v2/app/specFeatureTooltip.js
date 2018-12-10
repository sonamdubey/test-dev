function positionTooltip($toastMsgField) {

    var $toastMsgDescription = $toastMsgField.children('.description_toast_msg');
    var $activatedToastMessage = $('.description_toast_msg');
    var viewPortWidth = $(window).outerWidth();
    var toastMsgFieldOffsetTop = $toastMsgField.offset().top;
    var toastMsgDesHt = $toastMsgDescription.outerHeight();
    var toastMsgDeswidth = $toastMsgDescription.outerWidth();
    var spaceBtnMsgFieldNNav = (toastMsgFieldOffsetTop - $(window).scrollTop()) - $('.model-main-menu--fixed').outerHeight();
    var toastMsgFieldOffsetLeft = $toastMsgField.offset().left;
    var setToastMsgHorPos = toastMsgFieldOffsetLeft;
    var setToastMsgRelPos = (spaceBtnMsgFieldNNav > toastMsgDesHt) ? (toastMsgFieldOffsetTop - toastMsgDesHt) - 6 : (toastMsgFieldOffsetTop + $toastMsgField.outerHeight()) - 10;

    if ($activatedToastMessage.is(':visible')) {
            $activatedToastMessage.hide();
    }

    if ($toastMsgDescription[0]) {
        $toastMsgDescription.show();
        $toastMsgDescription.offset({ top: setToastMsgRelPos, left: setToastMsgHorPos });
    }
}
$(document).ready(function () {
    var handleDocumentClick = true;
    $('.defination_field').on('click', function () {
        var $toastMsgField = $(this).parent();
        positionTooltip($toastMsgField);
        handleDocumentClick = false;
        $(window).resize(function () {
            var $currentToastMsg = $toastMsgField.children('.description_toast_msg');
            var currentToastMsgIsVisible = $toastMsgField.children('.description_toast_msg').is(':visible');
            if (currentToastMsgIsVisible) {
                positionTooltip($toastMsgField);
                $currentToastMsg.show();
            }
            else {
                $currentToastMsg.hide();
            }
        });
    });

     

        $('.toast_msg_close').on('click', function () {
            var $closeIcon = $(this);
            $closeIcon.parent('.description_toast_msg').hide();
            handleDocumentClick = false;
        });

      

        $(document).click(function () {
            if (handleDocumentClick) {
                $('.description_toast_msg').hide();
            }
            handleDocumentClick = true;
        });
 });