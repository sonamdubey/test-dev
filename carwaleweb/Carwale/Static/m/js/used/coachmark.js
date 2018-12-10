var coachMark = (function () {
    var _noMoreTips = "#noMoreTips";
    var $filtersCoachMarkNode = $("#filters-coachmark");
    var _detailsCoachMark = "#detailsCoachmark";
    var _coachMarkCookie = "mUsedCarsCoachmark1";
    var $searchListFirst = $('.search-list ul li:first a[id=callBtn]');
    function checkCoachmarkCookie() {
        if (typeof cookie != 'undefined') {
            if (cookie.isCookiePresent(_coachMarkCookie)) {
                var cookieValue = cookie.getCookie(_coachMarkCookie);
                if (cookieValue.indexOf('search') == -1) {
                    cookieValue = cookieValue + "search";                    
                    SetCookieInDays(_coachMarkCookie, cookieValue, 30);
                    showCoachMark('sortFilter');
                }
            } else {
                SetCookieInDays(_coachMarkCookie, "search|", 30);
                showCoachMark('sortFilter');
            }
        }
    };
    function showCoachMark(element) {
        switch (element) {
            case 'sortFilter':
                _showCoachMarkSortFilter();
                break;
            case 'getSellerDetails':
                _showCoachMarkOnGetSellerDetails();
                break;
            case 'CoachMarkCallBtn':
                _hideCoachmarkOnGetSellerDetails();
                break;
            case 'noMoreTips':
                _closeCoachMark();
                break;
        };
    };
    function _showCoachMarkSortFilter() {
        if ($("#listing").length) {
            var coachmarkHtml = $("#detailsCoackmarkWrapper").html();
            $("#detailsCoackmarkWrapper").html("");
            $("#searchListingMobile-0 .seller-btn-detail").prepend(coachmarkHtml);
        }
        else {
            $("#filterCoachmark").text("Got it");
        }
        $filtersCoachMarkNode.css('display', 'block');
    };

    function _showCoachMarkOnGetSellerDetails() {
        var $detailsCoachMark = $("#details-coachmark");
        $filtersCoachMarkNode.css('display', 'none');
        if ($("#listing").length) {
            if ($searchListFirst.is(":visible") == false) {
                $(_detailsCoachMark).text('Got it');
                $detailsCoachMark.css('display', 'block');
                $detailsCoachMark.find(_noMoreTips).css('display', 'none');
            }
            else {
                $detailsCoachMark.css('display', 'block');
            }
        }
    };
    function _hideCoachmarkOnGetSellerDetails() {
        $("#details-coachmark").css('display', 'none');
        if ($searchListFirst.is(':visible')) {
            $('.search-list ul li').first().find('.call-btn-pos').css('display', 'block');
        }
    };
    function _closeCoachMark() {
        $('#filters-coachmark').css('display', 'none');
    };
    return { checkCoachmarkCookie: checkCoachmarkCookie, showCoachMark: showCoachMark };
})();
$(document).ready(function () {
    $(document).on("click", "#filterCoachmark", function () { coachMark.showCoachMark("getSellerDetails") });
    $(document).on("click", "#noMoreTips", function (event) {
        event.stopPropagation();
        event.preventDefault();
        coachMark.showCoachMark("noMoreTips");
    });
    $(document).on("click", "#detailsCoachmark", function () { coachMark.showCoachMark('CoachMarkCallBtn'); });
});