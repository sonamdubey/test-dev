var versionByDiv = $(".version-div"),
    versionListDiv = $(".version-selection-div"),
    versionListLI = $(".version-selection-div ul li");

versionByDiv.click(function () {
    if (!versionByDiv.hasClass("open"))
        $.versionChangeDown(versionByDiv);
    else
        $.versionChangeUp(versionByDiv);
});

$.versionChangeDown = function (versionByDiv) {
    versionByDiv.addClass("open");
    versionListDiv.show();
};

$.versionChangeUp = function (sortByDiv) {
    versionByDiv.removeClass("open");
    versionListDiv.slideUp();
};

$(document).mouseup(function (e) {
    if (!$(".variantDropDown, .version-div, .version-div #upDownArrow, .version-by-title").is(e.target)) {
        $.versionChangeUp($(".version-div"));
    }
});
//TODO handle version select event

$(document).ready(function () {
    var pqDealerHeader = $('#pqDealerHeader'),
        pqDealerBody = $('#pqDealerBody'),
        pqRemoveHeader = $('#pqRemoveHeader'),
        pqDealerHeaderWrapper = $('#pqDealerDetails'),
        $window = $(window),
        floatButton = $('.float-button'),
        bodHt, footerHt, scrollPosition;
    $window.scroll(function () {
        if (!pqDealerHeader.hasClass('pq-fixed')) {
            if ($window.scrollTop() > pqDealerHeader.offset().top && $window.scrollTop() < pqRemoveHeader.offset().top - 40) { //subtract 40px (pq header height)
                pqDealerHeader.addClass('pq-fixed').find('.dealership-name').addClass('text-truncate padding-bottom5 border-light-bottom');
                pqDealerBody.addClass('padding-top40');
            }
        }
        else if (pqDealerHeader.hasClass('pq-fixed')) {
            if ($window.scrollTop() < pqDealerHeaderWrapper.offset().top || $window.scrollTop() > pqRemoveHeader.offset().top - 40) { //subtract 40px (pq header height)
                pqDealerHeader.removeClass('pq-fixed').find('.dealership-name').removeClass('text-truncate padding-bottom5 border-light-bottom');
                pqDealerBody.removeClass('padding-top40');
            }
        }
        bodHt = $('body').height();
        footerHt = $('footer').height();
        scrollPosition = $(this).scrollTop();
        if (floatButton.offset().top < $('footer').offset().top - 50)
            floatButton.addClass('float-fixed');
        if (floatButton.offset().top > $('footer').offset().top - 50)
                floatButton.removeClass('float-fixed');
    });
    
});