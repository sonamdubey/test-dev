var tyreDetailPage = {

    registerEvents: function () {
        $('#News div a:last').hide();
        $('#tyre-info').on('click', function () {
            $('#infoPopup h3').text('Tyre Size');
            $('.tyre-info-img').attr('src', 'https://imgd.aeplcdn.com/0x0/cw/es/tyres/tyre-info2.jpg').show();
            tyreDetailPage.openTyrePopup();
        });
        $('#speed-rating-info').on('click', function () {
            $('#infoPopup h3').text('Speed Rating');
            $('.tyre-info-img').attr('src', 'https://imgd.aeplcdn.com/0x0/cw/es/tyres/speed-rating-msite.png').show();
            tyreDetailPage.openTyrePopup();
        });

        $('.tyre-info-close, .blackOut-window').on('click', function () {
            tyreDetailPage.closeTyrePopup();
        });

        $(document).keydown(function (e) {
            if (e.keyCode == 27) {
                tyreDetailPage.closeTyrePopup();
            }
        });

        $("a.view-more-btn").click(function (e) {
            e.preventDefault();
            var hiddenElem = $(this).closest('.tyre-home-carousel').find('.news-body-list')
            if (hiddenElem.hasClass("hide")) {
                hiddenElem.fadeIn(1000,function () {
                    hiddenElem.removeClass("hide").find('img.lazy').lazyload();
                });
            }
            else {
                $("html, body").animate({
                    scrollTop: ($(".news-type-container").closest("section").offset().top - 60)
                }, 1000);
                hiddenElem.fadeOut(800, function () {
                    hiddenElem.addClass("hide");
                });
            }
            var moreContent = $(this).find("span");
            moreContent.text(moreContent.text() === "more" ? "less" : "more");
        });
    },

    openTyrePopup: function () {
        $('#tyre-info-popup, .blackOut-window').show();
        Common.utils.lockPopup();
    },
    closeTyrePopup: function () {
        $('#tyre-info-popup, .blackOut-window').hide();
    },
}


$(document).ready(function () {
    tyreDetailPage.registerEvents();
});

$(document).keydown(function (e) {
    if (e.keyCode == 27) {
        Common.utils.unlockPopup();
        tyreDetailPage.closeTyrePopup();
    }
});