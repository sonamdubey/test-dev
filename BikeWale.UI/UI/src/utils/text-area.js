var TextArea = function () {
    var input
    function _setSelector() {
        input = $('.form-control__text-area')
    }
    function _changeHeight(element) {
        var elementHeight = element[0].scrollHeight;
        var elementMinHeight = element.css('min-height');
        if (!element.val()) {
            element.css({
                'height': elementMinHeight
            })
        }
        else {
            element.css({
                'height': elementHeight + 'px'
            });
        }

        element.scrollTop(elementHeight)
    }

    function _handleCharCount(element, event) {
        var parent = element.closest('.area-control-box');

        if (typeof parent.attr('data-char-limit') === 'undefined') {
            return;
        }

        var characterCount = parent.find('.text-area__character-count');
        var maxCharCount = parseInt(element.attr('data-maxlength'));
        var currentCharCount = element.val().length
        if (!currentCharCount) {
            characterCount.text('Max ' + maxCharCount + ' Characters')
        }
        else {
            characterCount.text(maxCharCount - currentCharCount + ' Characters left');
            if (currentCharCount >= maxCharCount) {
                characterCount.text('Max character limit reached')
                parent.removeClass('invalid')
            }
            if (currentCharCount >= maxCharCount + 1) {
                characterCount.text('Max character limit exceeded')
                parent.addClass('invalid')
            }
            else if (currentCharCount < maxCharCount && parent.hasClass('invalid')) {
                parent.removeClass('invalid')
                characterCount.text('Max ' + maxCharCount + ' Characters')
            }
        }
    }

    function _handleInputClick() {
        input.on('keyup keydown keypress', function (event) {
            var element = $(this);
            _handleCharCount(element, event);
            _changeHeight(element);
        })
        input.on('paste', function (event) {
            var element = $(this);
            setTimeout(function () {
                _handleCharCount(element, event);
                _changeHeight(element);
            })

        })
    }
    function registerEvents() {
        _setSelector();
        _handleInputClick();
    }

    return {
        registerEvents: registerEvents
    }
}();
