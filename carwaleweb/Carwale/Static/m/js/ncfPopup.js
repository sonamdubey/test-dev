(function () {
	function ncfPopUp() {
		var _ncfPopUp = {};
		function getNcfOriginPageName() {
		    var element = $("#nfcPopUpWidget");
			if (typeof element !== 'undefined') {
				return element.data('pagename');
			}
			return '';
		}
		function getNcfSlugPageName() {
		    var element = $("#nfcWidget");
		    if (typeof element !== 'undefined') {
		        return element.data('pagename');
		    }
		    return '';
		}
		function showPopUp() {
		    $('.ncf-secondary-blue-slug').animate({ bottom: '100px' }, 1000);
			cwTracking.trackAction('CWNonInteractive', 'ncfPopUp', 'ncfPopUp_Impression', getNcfOriginPageName());
			clientCache.set("ncfPopUp", 1, true);
		}
		function getNcfUrl() {
			if (userCityId > 0 && ncfBudget > 0) {
				return '/find-car/results/?cityId='+userCityId+'&budget='+ncfBudget;
			}
			else {
				return '/find-car/';
			}
		}
		_ncfPopUp.hidePopUp = function(){
			$('.ncf-slug-wrapper').fadeOut(200);
			cwTracking.trackAction('CWInteractive', 'ncfPopUp', 'ncfPopUp_close_btn_click', getNcfOriginPageName());
		}
		
		_ncfPopUp.goToNcf = function () {
			$('.ncf-slug-wrapper').fadeOut(200);
			window.location.href = getNcfUrl();
			cwTracking.trackAction('CWInteractive', 'ncfPopUp', 'ncfPopUp_slug_click', getNcfSlugPageName());
		}
		_ncfPopUp.message = function () {
			var key = "ncfPopUp";
			var ncfPopUpKey = clientCache.get(key, true);
			if (!ncfPopUpKey) //for the session only
			{
				setTimeout(showPopUp, ncfPopUpDefaultTimeOutExp ? ncfPopUpDefaultTimeOutExp : 30000);
			}
		}
		return _ncfPopUp;
	}
	if (typeof (window.ncfPopUp) === 'undefined') {
		window.ncfPopUp = ncfPopUp();
	}
})();

$(document).ready(function () {
	if (typeof ncfPopUp === 'object' && Object.keys(ncfPopUp).length > 0) {
        ncfPopUp.message();
		var ncfSection = $(".ncf-secondary-blue-slug");
		ncfSection.find(".ncf-white-car-img").click(ncfPopUp.goToNcf);
		ncfSection.find(".ncf-white-car-content").click(ncfPopUp.goToNcf);
		ncfSection.find(".slug-close-btn").click(ncfPopUp.hidePopUp);
	}
});