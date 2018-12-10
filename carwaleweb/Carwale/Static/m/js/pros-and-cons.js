var UserReviewScroll = (function () {
    var modelReviewContainer, showModelReviewContainer, fullReviewBtn, fixHideBtn, showFullReview, shortReviewPosition;
    function _setSelectors() {
        modelReviewContainer = document.getElementById("model-review-container").classList;
        showModelReviewContainer = document.getElementsByClassName('show-full-review')[0];
        fullReviewBtn = document.getElementById("full-review-btn").classList;
        fixHideBtn = "fix-hide-button";
        showFullReview = "show-full-review";
    }
    function hideReviewScroll() {
        _setSelectors();
        if (ViewPortDetect.isInViewport(showModelReviewContainer)) {
            fullReviewBtn.add(fixHideBtn);
        }
        else if (ViewPortDetect.TopInViewport(showModelReviewContainer, 100)) {
            fullReviewBtn.add(fixHideBtn);
        }
        else if (ViewPortDetect.BottomInViewport(showModelReviewContainer, 60)) {
            fullReviewBtn.remove(fixHideBtn);
        }
        else if (ViewPortDetect.HeightOutOfViewport(showModelReviewContainer, 30)) {
            fullReviewBtn.add(fixHideBtn);
        }
        else {
            fullReviewBtn.remove(fixHideBtn);
        }
    }
    function registerEvents() {
        if (document.getElementById("full-review-btn") != null && document.getElementById("model-review-container") != null) {
            _setSelectors();
            document.getElementById("full-review-btn").onclick = function () {
                if (!modelReviewContainer.contains(showFullReview)) {
                    var showFullReviewScrollTop = $('#model-review-container')[0].offsetTop;
                    shortReviewPosition = showFullReviewScrollTop;
                    modelReviewContainer.add(showFullReview);
                    hideReviewScroll();
                    window.addEventListener('scroll', hideReviewScroll);
                }
                else {
                    modelReviewContainer.remove(showFullReview);
                    fullReviewBtn.remove('fix-hide-button');
                    window.removeEventListener('scroll', hideReviewScroll);
                    if (shortReviewPosition != null && shortReviewPosition != undefined) {
                        $(window).scrollTop(shortReviewPosition);
                    }
                }
            }
        }
    }
    return {
        registerEvents: registerEvents,
    }
})();

document.addEventListener("load", UserReviewScroll.registerEvents());

var change = true;
var previousPosition = 15;
var element = document.getElementById('pros-cons__card-list__container');
jQuery('.pros-cons-card-container').bind('touchend', function (event) {
    var position = element.getBoundingClientRect();
    if (position.left < 15 && position.left >= -170) {
        if (previousPosition > position.left && change == true) {
            Common.utils.trackAction("CWInteractive", "CarWale_Opinion_pros_cons_m", "Impression_cons_swiped", ModelName);
            cwTracking.trackCustomData("CarWaleOpinionProsCons", "ImpressionConsSwiped", ("modelid=" + Modelid + "|source=43"), false);
            change = false;
        }
        if (previousPosition < position.left && change == false) {
            change = true;
        }
    }
    else if (position.left >= 15 || position.left < previousPosition) {
        change = true;
    }
    previousPosition = position.left;
});

if ((document.getElementById('verdictDescription') == null)) {
    $(".pros-and-cons-wrapper").addClass('review-unavailable');
}