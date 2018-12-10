var modelPopup = function () {
	var trackingData = {
		position: 0,
		count: 0,
		inputText: "",
		isInputClicked: false
	};
	
	var registerEvents = function () {
	  $('.model-selection-feild').on('click', function (e) {
            $('.select-model-popup').addClass('popup--active');
			Common.utils.lockPopup();
            if ($('#modelSelectionInput') != undefined) {
				$('#modelSelectionInput').focus();
				window.location.hash = '#model';
            }
        });

	  $('.js-popup__close').on('click', function (event) {
		  event.stopPropagation();
			$('.select-model-popup').removeClass('popup--active');
			Common.utils.unlockPopup();
			trackingData.isInputClicked = false;
			window.history.back();
	  });

	  $(".js-select-model-popup").on('click', function () {
		  if (trackingData.isInputClicked) {
			  cwTracking.trackCustomData('MobileReviewsLandingPage', 'SearchOutsideClick', "inputText=" + trackingData.inputText + "|source=43", false);
			  trackingData.isInputClicked = false;
		  }
	  });
    };
    var modelAutoComplete = function () {
		registerEvents();
        $('#modelSelectionInput').cw_easyAutocomplete({
            inputField: $('#modelSelectionInput'),
            isPriceExists: 1,
            resultCount: 8,
            isNew: 1,
            showFeaturedCar: false,
            additionalTypes: ac_textTypeEnum.model + ',' + ac_textTypeEnum.discontinuedModel,
            source: ac_Source.generic,
            click: function (event) {
                event.stopPropagation();
                var selectionValue = this.inputField.getSelectedItemData().value;
				var modelId = selectionValue.split(':')[2];
				var carName = this.inputField.getSelectedItemData().label;
				trackingData.position = this.inputField.getSelectedItemIndex() + 1;
				cwTracking.trackCustomData('MobileReviewsLandingPage', 'SearchAutoSuggestValueClick', "results=" + trackingData.count + "|position=" + trackingData.position + "|searchedTerm=" + trackingData.inputText + "|carName=" + carName + "|modelId=" + modelId  + "|source=43", false);
                location.href = "/userreviews/rate-car/?modelId=" + modelId;
			},
			beforefetch: function () {
				trackingData.inputText = $('#modelSelectionInput').val() || "";
				trackingData.isInputClicked = true;
			},
			afterFetch: function (suggestionResult, requestTerm) {
				trackingData.count = suggestionResult ? suggestionResult.length : 0;
				if (trackingData.count == 0)
				{
					cwTracking.trackCustomData('MobileReviewsLandingPage', 'SearchNotFound', "searchedTerm=" + trackingData.inputText + "|source=43", false);
				}
			}
        });
	};
	var handleHashParam = function () {
		var hashValue = location.hash;
		if (!hashValue) {
			$('.popupContainer').removeClass('popup--active');
			$('.select-model-popup').removeClass('popup--active');
			Common.utils.unlockPopup();
		}
		else if (hashValue === '#model') {
			$('.select-model-popup').addClass('popup--active');
			Common.utils.lockPopup();
			trackingData.isInputClicked = false;
		}
		else if (hashValue === '#howtowin') {
			$('.js-how-to-win-popup').addClass('popup--active'); // _HowToWin.cshtml
			Common.utils.lockPopup();
		}
		else if (hashValue === '#termandcondition') {
			$('.terms-and-conditions-popup').addClass('popup--active'); // _TermsAnsCondition.cshtml
			Common.utils.lockPopup();
		}
	};
    var wrapper = {};
	wrapper.modelAutoComplete = modelAutoComplete;
	wrapper.handleHashParam = handleHashParam;
    return wrapper;
};
$(document).ready(function () {
    var autoPopup = new modelPopup();
	autoPopup.modelAutoComplete();
	window.onpopstate = autoPopup.handleHashParam;
	autoPopup.handleHashParam();
});



