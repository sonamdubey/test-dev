var ToggleReadMore = function () {

    function _readMoreEvent() {
        $(".question-answer-wrapper__read-more").click(function (event) {
            $(this).parent('.question-answer-wrapper__answer-box').addClass("question-answer-wrapper--active");
        });
    }

    function registerEvents() {
        _readMoreEvent();
    }

    return {
        registerEvents: registerEvents
    }

}();


