$(document).ready(function () {
    var suggestedCarInput = $('#js-suggestedCarInput');
    var pageId = { PQPAGEMODELCHANGE: 136 };
    var clearButton = $('#js-input__clrBtn');

    function initializePopUp() {
        clearButton.addClass('hide');
    }
    
    function showHideClearButton(isShown) {
        if (isShown) {
            clearButton.removeClass('hide');
        }
        else {
            clearButton.addClass('hide');
        }
    };

    suggestedCarInput.cw_autocomplete({
        resultCount: 5,
        isPriceExists: 1,
        source: ac_Source.generic,
        textType: ac_textTypeEnum.model,
        click: function (e, ui) {
            var selectedModel = ui.item.id;
            var selectedModelId = selectedModel.split('|')[1].split(':')[1];
            var selectedModelName = selectedModel.split('|')[1].split(':')[0];
            // todo (GA tracking) : dataLayer.push({ event: 'CWInteractive', cat: "Desktop_PQ_ModelChange", act: "PQ_ModelChange_ModelSelected_Search", lab: selectedModelName });
            location.href = "/quotation/?m=" + selectedModelId + "&v=" + 0 + "&c=" + pageParams.cityId + "&a=" + pageParams.areaId + "&p=" + pageId.PQPAGEMODELCHANGE;
        },
        keyup: function (e) {
            if (e.target.value.length > 0) {
                showHideClearButton(true);
            }
            else {
                showHideClearButton(false);
            }
        },
        focusout: function () {
            showHideClearButton(false);
        }
        
    }).autocomplete("widget").addClass('autocomplete__list');

    initializePopUp();
});