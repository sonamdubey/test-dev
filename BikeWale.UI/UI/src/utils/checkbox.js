var Checkbox = (function () {
    function _handleSelectAllClick() {
        $(".checkbox-toggle").on('change', function (event) {
            var element = $(this);
            var elementInput = element.find('.checkbox-container__input');
            var isChecked = elementInput.is(':checked');
            var checkboxContainer = element.closest('.selection-container').find('.selection-checkbox-container .checkbox-container__input');
            _setLabelText(element);
            elementInput.attr('checked', isChecked);
            checkboxContainer.attr('checked', isChecked);
        })
    }
    function _setLabelText(element) {
        var elementLabel = element.find('.checkbox-container__label');
        var elementIsChecked = element.find('.checkbox-container__input').is(':checked')
        var text = elementIsChecked ? 'Remove All' : 'Select All';
        elementLabel.text(text);
    }

    function _handleCheckboxClick() {
        $(".selection-checkbox-container .checkbox-container").on('change', function () {
            var selectionContainer = $(this).closest('.selection-container');
            var parentElement = selectionContainer.find('.selection-checkbox-container .checkbox-container');
            var elements = parentElement.find('.checkbox-container__input');
            var checkedElements = parentElement.find('.checkbox-container__input:checked');

            var toggleCheckBox = selectionContainer.find('.checkbox-toggle');
            var toggleCheckBoxInput = toggleCheckBox.find('.checkbox-container__input');
            
            if (elements.length === checkedElements.length) {
                toggleCheckBoxInput.attr('checked', true)
                _setLabelText(toggleCheckBox);
            }
            else if (toggleCheckBoxInput.is(':checked')) {
                toggleCheckBoxInput.attr('checked', false)
                _setLabelText(toggleCheckBox);
            }
        })
    }
    function registerEvents() {
        _handleSelectAllClick();
        _handleCheckboxClick();
    }

    return {
        registerEvents: registerEvents
    }
})();

