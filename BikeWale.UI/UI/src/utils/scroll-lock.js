var handleBodyScroll = (function () {

    var doc, scrollTopPosition;

    function _setSelector() {
        doc = document.documentElement;
        scrollTopPosition = doc.scrollTop || window.pageYOffset;
    }

    function lockScroll() {
        _setSelector();
        doc.classList.add('lock-browser-scroll');
        doc.style.top = -scrollTopPosition + "px";
    };

    function unlockScroll() {
        _setSelector();
        var scrollPos = parseInt(doc.style.top);
        doc.classList.remove('lock-browser-scroll');
        window.scrollTo(0, -scrollPos)
    };

    return {
        lockScroll: lockScroll,
        unlockScroll: unlockScroll
    }
})();