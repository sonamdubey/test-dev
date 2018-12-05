var HandleReadMore = function () {
    function _readMoreEvent() {
        $(".read-more-wrapper__expand-link").click(function (event) {
            $(this).parent('.read-more-wrapper').addClass("read-more-wrapper--expand-active");
        });
    }
    function registerEvents() {
        _readMoreEvent();
    }
    return {
        registerEvents: registerEvents
    }
}();