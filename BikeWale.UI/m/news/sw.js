(global => {

	const version = '1' // version
	var SIZE_0_0 = '0x0';
	var SIZE_110_61 = '110x61';
	var SIZE_160_89 = '160x89';
	var SIZE_600_337 = '600x337';
	var SIZE_640_348 = '640x348';
	var SIZE_174_98 = '174x98';

	var IMAGE_EXPIRATION_TIME = 86400;

	var OFFLINE_PAGE = 'offline.html';
	var VENDOR_JS = 'https://stg1.bikewale.com/Scripts/vendor.bundle.js';
	var APP_JS = 'https://stg1.bikewale.com/Scripts/app.bundle.js';
	var SW_TOOLBOX_JS = 'https://st.aeplcdn.com/staging/bikewale/pwa/build/sw-toolbox.js';
	var IMAGE_CDN_REGEX_PATTERN = /^https:\/\/imgd(\d)?.aeplcdn.com/
	var ST_CDN_REGEX_PATTERN = /^https:\/\/st(\d)?.aeplcdn.com/
	importScripts(SW_TOOLBOX_JS);

	toolbox.options.cache = {
		name: "bw-offline-precache"
	}
	global.toolbox.options.debug = true;

	global.toolbox.precache (
		[
			OFFLINE_PAGE,
			VENDOR_JS,
			APP_JS
		]
	)

	global.toolbox.router.get('/api/pwa/*',global.toolbox.networkFirst,
								{
									cache : {
										name : 'api',
										maxAgeSeconds : 86400,
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
	
   
	global.toolbox.router.get('/'+SIZE_0_0+'/*',global.toolbox.networkFirst,		
								{
									cache : {
												name : 'cdn-images-'+SIZE_0_0,
												maxAgeSeconds : IMAGE_EXPIRATION_TIME,
												maxEntries : 60
											},
									origin : IMAGE_CDN_REGEX_PATTERN
									
								});


	global.toolbox.router.get('/'+SIZE_110_61+'/*',global.toolbox.cacheFirst,
								{
									cache : {
												name : 'cdn-images-'+SIZE_110_61,
												maxAgeSeconds : IMAGE_EXPIRATION_TIME,
												maxEntries : 40
											},
									origin : IMAGE_CDN_REGEX_PATTERN
								});


	global.toolbox.router.get('/'+SIZE_160_89+'/*',global.toolbox.cacheFirst,
								{
									cache : {
												name : 'cdn-images-'+SIZE_160_89,
												maxAgeSeconds : IMAGE_EXPIRATION_TIME,
												maxEntries : 40
											},
									origin : IMAGE_CDN_REGEX_PATTERN
								});


	global.toolbox.router.get('/'+SIZE_174_98+'/*',global.toolbox.cacheFirst,
								{
									cache : {
												name : 'cdn-images-'+SIZE_174_98,
												maxAgeSeconds : IMAGE_EXPIRATION_TIME,
												maxEntries : 40
											},
									origin : IMAGE_CDN_REGEX_PATTERN
								});

	global.toolbox.router.get('/'+SIZE_600_337+'/*',global.toolbox.cacheFirst,
								{
									cache : {
												name : 'cdn-images-'+SIZE_600_337,
												maxAgeSeconds : IMAGE_EXPIRATION_TIME,
												maxEntries : 25
											},
									origin : IMAGE_CDN_REGEX_PATTERN
								});

	global.toolbox.router.get('/'+SIZE_640_348+'/*',global.toolbox.cacheFirst,
								{
									cache : {
												name : 'cdn-images-'+SIZE_640_348,
												maxAgeSeconds : IMAGE_EXPIRATION_TIME,
												maxEntries : 25
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
			maxAgeSeconds : 86400,
			maxEntries : 10
		},
		origin : ST_CDN_REGEX_PATTERN
	})


	global.toolbox.router.get('/*',global.toolbox.cacheFirst,{
		cache : {
			name : 'google-resources',
			maxAgeSeconds : 86400,
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

