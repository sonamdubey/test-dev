var accordion = (function () {
    var accordionhead, accordionlistitem, accordionbody, accordionbodyshow;
    function _setSelectors() {
        accordionhead = '.accordion__head';
        accordionlistitem = '.accordion-list__item';
        accordionbody = '.accordion__body';
    }

    function listScroll(currentelement) {
        var lastelement = currentelement.closest('.accordion-list__item').is(':last-child');
        if (lastelement) {
            var accordionlistelement = currentelement.closest('.accordion__list');
            var scrollheight = accordionlistelement[0].scrollHeight;
            accordionlistelement.animate({ scrollTop: scrollheight }, 400);
        }
    }

    function handleClick(currentelement) {
        accordionbodyshow = currentelement.next(accordionbody);
        if (accordionbodyshow.is(':visible')) {

            currentelement.closest(accordionlistitem).removeClass('accordion-list--active');
            accordionbodyshow.slideUp(400);
        }
        else {
            accordionbodyshow.slideDown(400, function () {
                listScroll(currentelement)
            });
            currentelement.closest(accordionlistitem).addClass('accordion-list--active');
        }
    }
    function registerEvents() {
        _setSelectors();
        $(document).on('click', accordionhead, function () {
            var currentelement = $(this);
            handleClick(currentelement);
        });
    }
    return {
        registerEvents: registerEvents
    }
})();