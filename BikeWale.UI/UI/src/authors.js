(function ($) {
    $(function () {
        var jcarousel = $('#author__caraousel .jcarousel');

        $('#author__carousel .jcarousel-control-prev')
            .jcarouselControl({
                target: '-=2'
            });

        $('#author__carousel .jcarousel-control-next')
            .jcarouselControl({
                target: '+=2'
            });

        $('#author__carousel .jcarousel-pagination')
            .on('jcarouselpagination:active', 'a', function () {
                $(this).addClass('active');
            })
            .on('jcarouselpagination:inactive', 'a', function () {
                $(this).removeClass('active');
            })
            .on('click', function (e) {
                e.preventDefault();
            })
            .jcarouselPagination({
                perPage: 1,
                item: function (page) {
                    return '<a href="#' + page + '">' + page + '</a>';
                }
            });
    });
})(jQuery);