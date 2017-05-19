(global => {

	var IMAGE_EXPIRATION_TIME = 864000;

	var OFFLINE_PAGE = 'offline.html?19May2017v2';
	var VENDOR_JS = 'https://st.aeplcdn.com/staging/bikewale/pwa/build/vendor.bundle.js?19May2017v2';
	var APP_JS = 'https://st.aeplcdn.com/staging/bikewale/pwa/build/app.bundle.js?19May2017v2';
	var SW_TOOLBOX_JS = 'https://st.aeplcdn.com/staging/bikewale/pwa/build/sw-toolbox.js?19May2017v2';
	var IMAGE_CDN_REGEX_PATTERN = /^https:\/\/imgd(\d)?.aeplcdn.com/;
	var ST_CDN_REGEX_PATTERN = /^https:\/\/st(\d)?.aeplcdn.com/;
	var GOOGLE_FONTS = 'https://fonts.googleapis.com/css?family=Open+Sans:400,600,700'
	importScripts(SW_TOOLBOX_JS);

	toolbox.options.cache = {
		name: "bw-offline-precache"
	}
	global.toolbox.options.debug = false;

	global.toolbox.precache (
		[
			OFFLINE_PAGE,
			VENDOR_JS,
			APP_JS,
			GOOGLE_FONTS
		]
	)

	global.toolbox.router.get('/api/pwa/*',global.toolbox.networkFirst,
								{
									cache : {	
										name : 'api',
										maxAgeSeconds : 864000,
										maxEntries : 100
									}
								})
	

	global.toolbox.router.get('/m/news/*',function(request,values,options) {
			return global.toolbox.networkOnly(request,values,options).then(function(response) {
				return response;
			}).catch(function(err) {
				if('GET' === request.method && request.headers.get('accept').includes('text/html')) {
					return global.toolbox.cacheOnly(new Request(OFFLINE_PAGE))	
				}
				throw err;
			})
		},
		{
			cache : {
				name : 'html-resource',
				maxEntries : 25 , 
				maxAgeSeconds : 86400
			}
		})
	
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
					request.url === APP_JS) {
					return global.toolbox.cacheOnly(new Request(request.url))	
				}
				throw err;
			})
		}, {
		mode: "cors",
		cache : {
			name : 'cdn-JS',
			maxAgeSeconds : 864000,
			maxEntries : 10
		},
		origin : ST_CDN_REGEX_PATTERN
	})


	global.toolbox.router.get('/*',global.toolbox.cacheFirst,{
		cache : {
			name : 'google-resources',
			maxAgeSeconds : 864000,
			maxEntries : 10
		},
		origin : /^(http:\/\/www.googletagmanager.com)|(https:\/\/fonts.googleapis.com)|(https:\/\/fonts.gstatic.com)|(https:\/\/www.google-analytics.com)|(https:\/\/tpc.googlesyndication.com)|(https:\/\/www.googletagservices.com)/
	})


	global.addEventListener('install',function(event) {
		event.waitUntil(
			global.skipWaiting()
		)
	})

	global.addEventListener('activate',function(event) {
		event.waitUntil(
			global.clients.claim()
		)
	})

}

)(self);

