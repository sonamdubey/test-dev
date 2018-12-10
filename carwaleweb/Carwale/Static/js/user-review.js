var makeModelSearch;
var eventCategory = "DesktopReviewsLandingPage";
var trackingData = {
	position: 0,
	count: 0,
	inputText: ""
};

var readMoreClicked = function () {
    cwTracking.trackCustomData(eventCategory, "TAndCReadMore", "NA", false);
}
var collapseClicked = function () {
    cwTracking.trackCustomData(eventCategory, "TAndCReadLess", "NA", false);
}
var modelAutoCompleteOptions = {
    isPriceExists: 1,
    resultCount: 6,
    isNew: 1,
    showFeaturedCar: false,
    additionalTypes: ac_textTypeEnum.model + ',' + ac_textTypeEnum.discontinuedModel,
    source: ac_Source.generic,
    click: function (event, ui, orgTxt) {
		event.stopPropagation();
		var carName = ui.item.label;
		var ul = event.originalEvent.target;
		trackingData.position = $(ul).find('li.ui-state-focus').index() + 1;
		var label = ui.item.id;
		var modelData = label.split('|')[1];
		var modelId = modelData.split(':')[1];
		cwTracking.trackCustomData(eventCategory, 'SearchAutoSuggestValueClick', "results=" + trackingData.count + "|position=" + trackingData.position + "|searchedTerm=" + trackingData.inputText + "|carName=" + carName + "|modelId=" + modelId + "|source=1", false);
		window.location.href = "/userreviews/rate-car/?modelId=" + modelId;
	},
	beforefetch: function () {
		trackingData.inputText = $('#makeModelSearch').val() || "";
	},
	afterfetch: function (result, searchtext) {
		if (!result || result.length == 0) {
			cwTracking.trackCustomData(eventCategory, 'SearchNotFound', "searchedTerm=" + searchtext + "|source=1", false);
		}
		if (result != undefined && result.length > 0) {
			trackingData.count = result.length;
		}
	},
	focusout: function () {
		if (trackingData.count > 0) {
			cwTracking.trackCustomData('DesktopReviewsLandingPage', 'SearchOutsideClick', "inputText=" + trackingData.inputText + "|source=1", false);
		}
	}
}

$(document).on('ready', function () {
    var lastScrollTop = 0;
    makeModelSearch = $('#makeModelSearch');
	makeModelSearch.val('');
	makeModelSearch.cw_autocomplete(modelAutoCompleteOptions);
	makeModelSearch.hint(); // this line should always be below cw_autocomplete plugin initialization
    var scrollSpy = new ScrollSpy('#user-review-container', {
        onScrollContainer: function () {
            var container = document.getElementById('userReviewMenu');
            var windowScrollTop = window.pageYOffset || document.documentElement.scrollTop;
            if (windowScrollTop < lastScrollTop) {
                container.classList.remove('overall-tabs-container--visible');
            } else {
                container.classList.add('overall-tabs-container--visible');
            }
            lastScrollTop = windowScrollTop <= 0 ? 0 : windowScrollTop;
        }
    });
    var modelReviewReadMoreCollapse = new ReadMoreCollapse('#termsAndCondition', {
        expandText: 'Read More',
        collapseText: 'Collapse',
        ellipsisText: '&nbsp;',
        onExpandClick: readMoreClicked,
        onCollapseClick: collapseClicked
    });
    var scrollPos = $('.terms-and-conditions-container').offset().top;
    $('.banner__tnc-text').on('click', function () {
        $('html, body').animate({
            scrollTop: scrollPos - 50
        }, 500);
    })
});
