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
                var makeModelName = $('#makeName').val() + "_" + $('#modelName').val();
                var gaEventAction = "Submit_Answer_Button_Clicked";

                switch(parseInt(pageSrc))
                {
                    case 7: triggerGA('Write_Review', gaEventAction, makeModelName);
                            break;
                    case 8: triggerGA('Rate_Bike', gaEventAction, makeModelName);
                            break;
                    case 9: triggerGA("List_Used_Bike", gaEventAction, makeModelName);
                            break;
                }
                    
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
