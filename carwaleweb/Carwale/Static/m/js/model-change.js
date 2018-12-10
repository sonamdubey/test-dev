$(document).ready(function () {

    var editModelPopup = $('.edit-model-popup'),
        blackOutWindow = $('#globalPopupBlackOut');
    var suggestedCarsSwiper = $('#divModelSwiper');
        suggestedCarInput = $('#suggestedCarList'),
        clearButton = $('.model-input__clrBtn');
        var closeButton = $('.edit-model-popup__close-btn');
    var pqPageId = 136;

    $(document).on('transitionend', '#editModelPopup', function (event) {
    	if (event.target.getAttribute('id') === 'editModelPopup') {
    		if (typeof suggestedCarsSwiper.data('swiper') !== 'undefined') {
    			suggestedCarsSwiper.data('swiper').update();
    		}
    	}
    })

    function initialize() {
        blackOutWindow.show();
        Common.utils.lockPopup();
        editModelPopup.addClass('edit-model-popup--active');
        editModelPopup.addClass('edit-model-popup--full-height');
        priceQuote.appendState("ModelChange");
        initializeSwiper();
    }

    $(document).on('click', "#globalPopupBlackOut", function () {
        hidePopup();
        priceQuote.removeState();
    });

    $(window).on('popstate', function (event) {
        if ($("#editModelPopup").is(":visible")) {
            hidePopup();
        }
    });

    $(document).on('focus', ".model-name-input", function () {
        editModelPopup.addClass('edit-model-popup--full-height');
        $('.model-name-input__placeholder').hide();
    });

    $(document).on("blur", ".model-name-input", function () {
        if ($(".model-name-input").val().length == 0) {
            $('.model-name-input__placeholder').show();
        }
    });

    function initializeSwiper() {
    	suggestedCarsSwiper.swiper({
    		slidesPerView: 'auto',
    		spaceBetween: 10,
    		preloadImages: false,
    		lazyLoading: true,
    		lazyLoadingInPrevNext: true
    	});
    }

    function clearSuggestedInput() {
        if (suggestedCarInput.length > 0) {
            suggestedCarInput[0].value = '';
        }
    }

    function hidePopup() {
        editModelPopup.removeClass('edit-model-popup--full-height');
        editModelPopup.removeClass('edit-model-popup--active');
        blackOutWindow.hide();
        Common.utils.unlockPopup();
        clearSuggestedInput();
    }

    function showHideClearButton(isShown) {
        if (isShown) {
            clearButton.removeClass('hide');
        }
        else {
            clearButton.addClass('hide');
        }
    }

    $(document).on('click', ".model-input__clrBtn", function () {
        clearSuggestedInput();
        showHideClearButton(false);
        suggestedCarInput.focus();
    });

    closeButton.unbind().on('click', function () {
        hidePopup();
        priceQuote.removeState();
    });

    suggestedCarInput.cw_easyAutocomplete({
        inputField: suggestedCarInput,
        resultCount: 5,
        isPriceExists: 1,
        source: ac_Source.generic,
        textType: ac_textTypeEnum.model,
        click: function (e) {
            var selectedModel = suggestedCarInput.getSelectedItemData().value;
            var selectedModelId = selectedModel.split('|')[1].split(':')[1];
            var selectedModelName = selectedModel.split('|')[1].split(':')[0];
            dataLayer.push({ event: 'CWInteractive', cat: "MSite_PQ_ModelChange", act: "PQ_ModelChange_ModelSelected_Search", lab: selectedModelName });
            Common.PQ.redirectToNewPqPage(selectedModelId, 0, modelChangeData.cityId, modelChangeData.areaId, false, pqPageId, null, null, false);
        },
        keyup: function (e) {
            if (this.inputField[0].value.length > 0) {
                showHideClearButton(true);
            }
            else {
                showHideClearButton(false);
            }
        }
    });

    initialize();
});