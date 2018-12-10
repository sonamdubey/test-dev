var ViewPortDetect = (function () {
    function isInViewport(elem) {
        bounding = elem.getBoundingClientRect();
        return (
            bounding.top >= 0 &&
            bounding.bottom <= (window.innerHeight || document.documentElement.clientHeight)
        );
    }
    function TopInViewport(elem, bottomExtraHeight) {
        bottomExtraHeight = bottomExtraHeight || 0;
        bounding = elem.getBoundingClientRect();
        return (
            bounding.top >= 0 &&
            bounding.top <= ((window.innerHeight - bottomExtraHeight) || (document.documentElement.clientHeight - bottomExtraHeight)) &&
            bounding.bottom >= ((window.innerHeight) || (document.documentElement.clientHeight))
        );
    }
    function BottomInViewport(elem, bottomExtraHeight) {
        bottomExtraHeight = bottomExtraHeight || 0;
        bounding = elem.getBoundingClientRect();
        return (
          bounding.top <= 0 &&
          (bounding.bottom) >= 0 &&
          bounding.bottom <= ((window.innerHeight - bottomExtraHeight) || (document.documentElement.clientHeight - bottomExtraHeight))
       );
    }
    function HeightOutOfViewport(elem, bottomExtraHeight) {
        bottomExtraHeight = bottomExtraHeight || 0;
        bounding = elem.getBoundingClientRect();
        return (
            bounding.top <= 0 &&
            bounding.bottom >= ((window.innerHeight - bottomExtraHeight) || (document.documentElement.clientHeight - bottomExtraHeight))
        );
    }
    return {
        isInViewport: isInViewport,
        TopInViewport: TopInViewport,
        BottomInViewport: BottomInViewport,
        HeightOutOfViewport: HeightOutOfViewport
    }
})();
