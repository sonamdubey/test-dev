(global => {

    var jsVersion = '26May2017v1';
	var baseUrl = 'https://st.aeplcdn.com/bikewale/pwa/build/';
	var OFFLINE_PAGE = 'offline.html?' + jsVersion;
	var VENDOR_JS = baseUrl + 'vendor.bundle.js?' + jsVersion;
	var APP_JS = baseUrl + 'app.bundle.js?' + jsVersion;
	var APP_BTF_CSS = baseUrl + 'app-btf.css?' + jsVersion;
	var SW_TOOLBOX_JS = baseUrl + 'sw-toolbox.js?' + jsVersion;
	var IMAGE_EXPIRATION_TIME = 864000;
	var IMAGE_CDN_REGEX_PATTERN = /^https:\/\/imgd(\d)?.aeplcdn.com/;
	var ST_CDN_REGEX_PATTERN = /^https:\/\/st(\d)?.aeplcdn.com/;
	
	importScripts(SW_TOOLBOX_JS);

	toolbox.options.cache = {
		name: "bw-offline-precache"
	};
	global.toolbox.options.debug = false;

	global.toolbox.precache (
		[
			OFFLINE_PAGE,
			VENDOR_JS,
			APP_JS,
            APP_BTF_CSS
		]
	);

	global.toolbox.router.get('/api/pwa/*',global.toolbox.cacheFirst,
								{
									cache : {	
										name : 'api',
										maxAgeSeconds : 300,
										maxEntries : 100
									}
								});
	

	global.toolbox.router.get('/m/news/*',function(request,values,options) {
			return global.toolbox.networkOnly(request,values,options).then(function(response) {
				return response;
			}).catch(function(err) {
				if('GET' === request.method && request.headers.get('accept').includes('text/html')) {
					return global.toolbox.cacheOnly(new Request(OFFLINE_PAGE));
				}
				throw err;
			});
		},
		{
			cache : {
				name : 'html-resource',
				maxEntries : 25 , 
				maxAgeSeconds : 86400
			}
		});
	
	global.toolbox.router.get('/*',global.toolbox.cacheFirst,		
								{
									cache : {
												name : 'cdn-images',
												maxAgeSeconds : IMAGE_EXPIRATION_TIME,
												maxEntries : 150
											},
									origin : IMAGE_CDN_REGEX_PATTERN
									
								});
   

	global.toolbox.router.get('/*',function(request,values,options) {
			return global.toolbox.cacheFirst(request,values,options).then(function(response) {
				return response;
			}).catch(function(err) {
				if(request.url === VENDOR_JS ||
					request.url === APP_JS ||
                    request.url === APP_BTF_CSS) {
					return global.toolbox.cacheOnly(new Request(request.url));
				}
				throw err;
			});
		}, {
		mode: "cors",
		cache : {
			name : 'cdn-JS',
			maxAgeSeconds : 864000,
			maxEntries : 10
		},
		origin : ST_CDN_REGEX_PATTERN
	});


	global.toolbox.router.get('/*',global.toolbox.cacheFirst,{
		cache : {
			name : 'google-resources',
			maxAgeSeconds : 864000,
			maxEntries : 10
		},
		origin : /^(https:\/\/fonts.googleapis.com)|(https:\/\/fonts.gstatic.com)/
	});


	global.addEventListener('install',function(event) {
		event.waitUntil(
			global.skipWaiting()
		)
	});

	global.addEventListener('activate',function(event) {
		event.waitUntil(
			global.clients.claim()
		)
	});

}

)(self);

