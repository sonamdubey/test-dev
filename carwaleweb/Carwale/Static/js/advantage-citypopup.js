var Common = Common || {};
Common.advantage = {
    promise:undefined,
    popup: {
		init: function () {
			var adv_city_popup = $($("#advantage_popup").text()).attr('src');
			if (adv_city_popup) {
                $.get(adv_city_popup).done(function (response) {
                    $("body").append(response);
                    if (typeof AdvantageSearch != "undefined" && !AdvantageSearch.isAdvantageCity)
                    {
                        $('#advantage-city-select').attr('data-advantage-url', location.href + "?cityId=");
                        Common.advantage.popup.showCityPopup();
                        Common.advantage.popup.getPopupCities();
                    }
					Common.advantage.popup.registerEvents();
				});
			}
		},
		registerEvents: function () {

			$(document).on('click', "[data-adv-popup='showAdvantageCityPopup']", function () {
				var modelId = 0;
				var versionId = 0;
				if ($(this).attr('data-model') != undefined)
					modelId = $(this).attr('data-model');
				if ($(this).attr('data-versionid') != undefined)
					versionId = $(this).attr('data-versionid');
				Common.advantage.popup.getPopupCities(modelId, versionId);
				var url = $(this).attr('data-advantage-link');
				$('#advantage-city-select').attr('data-advantage-url', url);
				Common.advantage.popup.showCityPopup();
			});

			$(document).on('change', '#advantage-city-select', function () {
				cwTracking.trackAction('CWInteractive', 'deals_mobile', 'city_popup_city_selected');
				var url = $('#advantage-city-select').attr('data-advantage-url');
				var citySelected = $("#advantage-city-select").val();
				if (citySelected != "-1") {
					location.href = url + citySelected;
				}
			});

			$('div.blackOut-window, .advantage-city-popup .globalcity-close-btn').on('click', function () {
				Common.advantage.popup.hideCityPopup();
			});

			$(document).on('keyup', function (e) {
				if (e.keyCode == 27) {
					Common.advantage.popup.hideCityPopup();
				}
			});

			$('.advantagelinks').click(function () {
				location.href = $(this).find('.contentWrapper').attr("data-url");
			});
		},

		getAdvantageCities: function (modelId, versionId) {
			var config = {};
			config.contentType = "application/json";
			config.headers = { "sourceid": "43" };
			if (versionId > 0)
				return Common.utils.ajaxCall('/api/advantage/cities/?versionId=' + versionId, config);
			else if (modelId > 0)
				return Common.utils.ajaxCall('/api/advantage/cities/?modelId=' + modelId, config);
			else
				return Common.utils.ajaxCall('/api/advantage/cities/', config);
		},
		getPopupCities: function (modelId, versionId) {
			try {
				$.when(Common.advantage.popup.getAdvantageCities(modelId, versionId)).done(function (data) {
					var obj = {};
					obj = typeof (data) == "object" ? data : obj;
					$('#advantage-city-select').find('option:gt(0)').remove();
					$.each(obj, function (i, opt) { $('#advantage-city-select').append("<option value='" + opt.cityId + "'>" + opt.cityName + "</option>") });
				});
			} catch (e) { console.log(e) };
		},

		showCityPopup: function () {
			cwTracking.trackAction('CWNonInteractive', 'deals_mobile', 'city_popup_shown');
			$('.advantage-city-popup,.blackOut-window').show();
			
		},

		hideCityPopup: function () {
			$('.advantage-city-popup,.blackOut-window').hide();
		}
	},

	ABTesting: function (element, modulusNumber, text1, text2) {
		if (isCookieExists('_abtest')) {
			if (parseInt($.cookie("_abtest")) % modulusNumber === 0)
				element.text(text1);
			else
				element.text(text2);
		}
	}
};
$(document).ready(function () {
	Common.advantage.popup.init();
});
