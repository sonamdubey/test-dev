var ZoneProperty = {
	zonePropertyDocReady: function () {
		ZoneProperty.setSelectors();
		ZoneProperty.registerEvents();
	},
	//Variables declared for selectors	
	setSelectors: function () {
		ZoneProperty.getPaginationContainer = '.zone-property_pagination-container',
		ZoneProperty.getZonePropertyTab = 'ul.bg-white li',
		ZoneProperty.getZonePropertyContainer = '.featured-car_tabs-container',
		ZoneProperty.getZonePropertyBg = '.zone-property-background',
		ZoneProperty.getActiveBlock = '.zone-property__pagination-container',
		ZoneProperty.getCurrentScreen;
	},
	//All events for the selectors	
	registerEvents: function () {
		$(ZoneProperty.getZonePropertyTab).on('click', function () {
			if ($(this).attr('data-tabs') === 'toyotaYarisZone') {
				$(ZoneProperty.getZonePropertyContainer).addClass('zone-property-background');
				Common.utils.trackAction('CWInteractive', 'HPSlug_d', 'HPSlug_d_shown', 'Toyota Yaris Zone - Screen 1');
				cwTracking.trackCustomData('ESProperties', 'Impression', 'adtype=Toyota Yaris Zone-d|screen-shown=Toyota Yaris Zone - Screen 1', true);
			}
			else {
				$(ZoneProperty.getZonePropertyContainer).removeClass('zone-property-background');
			}
		});
		$(ZoneProperty.getPaginationContainer).jcarouselPagination('reload', {
			'perPage': 1
		});
		$(ZoneProperty.getActiveBlock).on('jcarouselpagination:active', 'a', function () {
			ZoneProperty.getCurrentScreen = $(this).text() - 1;
			Common.utils.trackAction('CWInteractive', 'HPSlug_d', 'HPSlug_d_shown', 'Toyota Yaris Zone - Screen ' + ZoneProperty.getCurrentScreen);
			cwTracking.trackCustomData('ESProperties', 'Impression', 'adtype=Toyota Yaris Zone-d|screen-shown=Toyota Yaris Zone - Screen ' + ZoneProperty.getCurrentScreen, true);
		});
	},
};
$(document).ready(function () {
	ZoneProperty.zonePropertyDocReady();
});