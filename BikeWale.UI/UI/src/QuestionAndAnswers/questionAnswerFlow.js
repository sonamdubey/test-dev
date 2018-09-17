var AnswerForm = function () {
    var form, submitAnswer, answerField;

    function _setSelectors() {
        form = $('#answerForm');
        submitAnswer = $('#submitAnswer');
        answerField = $('#submittedAnswer');
    }

    function _onInputFocused() {
        $(form).on('focusin', '.form-control__input', function () {
            var parent = $(this).closest('.form-control-box');

            if (parent.hasClass('invalid')) {
                parent.removeClass('invalid');
            }
        });
    };

    function _isAnswerValid() {
        var isValid = false;
        if (answerField.val().trim().length === 0) {
            answerField.closest('.form-control-box').addClass('invalid');
        }
        else {
            answerField.closest('.form-control-box').removeClass('invalid');
            isValid = true;
        }

        return isValid;
    }

    function _handleFormSubmit() {
        form.on('submit', function () {
            if (_isAnswerValid()) {
                var answer = answerField.val().trim();
                $("#submittedAnswerText").val(answer);
                var pageSrc = $('#sourceId').val();
                var modelName = $('#modelName').val();
                var makeName = $('#makeName').val();
                if (parseInt(pageSrc) === 7)
                    triggerGA('Write_Review', 'Submit_Answer_Button_Clicked', makeName + "_" + modelName);
                else if (parseInt(pageSrc) === 8)
                    triggerGA('Rate_Bike', 'Submit_Answer_Button_Clicked', makeName + "_" + modelName);
            }
            else
            {
                return false;
            }
        });
    }

    function registerEvents() {
        _setSelectors();
        _onInputFocused();
        _handleFormSubmit();
    }

    return {
        registerEvents: registerEvents
    }
}();

$(document).ready(function () {
    TextArea.registerEvents();
    AnswerForm.registerEvents();
});
