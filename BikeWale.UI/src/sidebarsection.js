docReady(function () {
    var windowScrollTop,
    sidebarBikeSection = $('#sidebarBikeSection'),
    sidebarHeight;

    if(sidebarBikeSection.offset()) {
        sidebarHeight = sidebarBikeSection.offset().top;
    }
    
    var overallContainer = $('#overallContainer'),
    overallContainerHeight = overallContainer.outerHeight(),
    sidebarPosition,
    topPosition = 65;   // 65 is top position when sidebar bike section is fixed
    $(window).scroll(function () {
        sidebarPosition = overallContainer.outerHeight() + overallContainer.offset().top
        windowScrollTop = $(this).scrollTop();
        if (windowScrollTop > sidebarHeight - topPosition) {
            sidebarBikeSection.addClass('fixed-sidebar');
        }
        else {
            sidebarBikeSection.removeClass('fixed-sidebar');
        }
        if (sidebarPosition < (windowScrollTop + sidebarBikeSection.outerHeight() + topPosition)) {
            if ((sidebarPosition) > windowScrollTop) {
                var chagePosition = (windowScrollTop - (sidebarPosition - sidebarBikeSection.outerHeight() - topPosition));
                sidebarBikeSection.css('top', (-chagePosition + topPosition))
            }
        }
        else {
            sidebarBikeSection.css('top', topPosition + 'px');
        }
    });

});