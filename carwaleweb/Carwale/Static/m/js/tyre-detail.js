var tyreDetailPage = {

    registerEvents: function () {
        
        $('#tyre-info').on('click', function () {
            $('#infoPopup h3').text('Tyre Size');
            $('.tyre-info-img').attr('src', 'https://imgd.aeplcdn.com/0x0/cw/es/tyres/tyre-info2.jpg?q=85').show();
            $('#tyre-info-popup, .blackOut-window').show();
            window.history.pushState('addPopup', '', '');
        });

        $('#speed-rating-info').on('click', function () {
            $('#infoPopup h3').text('Speed Rating');
            $('.tyre-info-img').attr('src', 'https://imgd.aeplcdn.com/0x0/cw/es/tyres/speed-rating-msite.png').show();
            $('#tyre-info-popup, .blackOut-window').show();
            window.history.pushState('addPopup', '', '');
        });

        $(window).on("popstate", function (e) {
            if ($('#tyre-info-popup').is(":visible")) {
                $('#tyre-info-popup, .blackOut-window').hide();
            }
        });

        $('.tyre-info-close, .blackOut-window').on('click', function () {
            $('#tyre-info-popup, .blackOut-window').hide();
        });

        $(document).keydown(function (e) {
            if (e.keyCode == 27) {
                $('#tyre-info-popup, .blackOut-window').hide();
                window.history.back();
            }
        });
    },
}


$(document).ready(function () {
    $('#news-reviews-videos-container .swiper-pagination').hide();
    tyreDetailPage.registerEvents();
});

