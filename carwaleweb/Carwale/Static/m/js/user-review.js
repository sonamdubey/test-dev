$(document).on('ready', function () {
    var scrollSpy = new ScrollSpy('#userReviewContainer');
    var scrollPos = $('.terms-and-conditions-container').offset().top;
    $('.banner__tnc-text').on('click', function () {
        $('html, body').animate({
            scrollTop: scrollPos - 50
        }, 800);
    })
})