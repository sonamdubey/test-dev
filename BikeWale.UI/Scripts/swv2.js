(global => {

/*
https://github.com/GoogleChrome/sw-toolbox/issues/198 :
Hey Mark, that would be an awesome pull-request. I did a quick look through the codebase, and it looks like the place where the query strings are being ignored is in /lib/router.js on line 98. Where it says var path = urlObject.pathname. For example, to always treat query strings as their own unique requests you would modify this line to read: var path = urlObject.pathname + urlObject.search. Hopefully this helps get you started.

*/
	// working regex for cdn -->  /^https:\/\/imgd(\d)?.aeplcdn.com/

	//NOTE
	/*
	http://stackoverflow.com/questions/38835273/when-does-code-in-a-service-worker-outside-of-an-event-handler-run
	*/
	var ROUTE_PREFIX = ''; // TODO change route prefix to '/m/'
	var SIZE_0_0 = '0x0';
	var SIZE_110_61 = '110x61';
	var SIZE_160_89 = '160x89';
	var SIZE_600_337 = '600x337';
	var SIZE_640_348 = '640x348';
	var SIZE_174_98 = '174x98';

	var IMAGE_EXPIRATION_TIME = 86400;

	var OFFLINE_PAGE = 'offline.html';
	var OFFLINE_CSS = 'app.css';
	var VENDOR_JS = 'https://st.aeplcdn.com/staging/bikewale/pwa/build/vendorv2.bundle.js';
	var APP_JS = 'https://st.aeplcdn.com/staging/bikewale/pwa/build/appv2.bundle.js'
	 // TODO specific size of pictures
	
	var IMAGE_CDN_REGEX_PATTERN = /^https:\/\/imgd(\d)?.aeplcdn.com/
	var ST_CDN_REGEX_PATTERN = /^https:\/\/st(\d)?.aeplcdn.com/
	// version 2

	//importScripts('node_modules/sw-toolbox/sw-toolbox.js');
	importScripts('https://st.aeplcdn.com/staging/bikewale/pwa/build/sw-toolbox.js');

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

	/*caching strategy for api
	eg. www.bikewale.com/api/pwa/..	*/
	//global.toolbox.router.get(ROUTE_PREFIX+'/api/pwa/*',global.toolbox.networkFirst,
	//TODO change api url for caching
	global.toolbox.router.get('/api/pwa/*',global.toolbox.networkFirst,
								{
									cache : {
										name : 'api',
										maxAgeSeconds : 86400,
										maxEntries : 50
									}
								})
	
//origin : 'https://imgd.aeplcdn.com/'

	/*caching strategy for css*/
	//TODO remove strategy to put on cdn
	/*global.toolbox.router.get('/news/style/articles.css',function(request,values,options) {
		return global.toolbox.cacheOnly(new Request(OFFLINE_CSS))
	})*/

	/*caching strategy for html within /news/*/
	//TODO add a timeout to the network fetch
	global.toolbox.router.get(ROUTE_PREFIX+'/m/news/*',function(request,values,options) {
			console.log('here')
			return global.toolbox.networkFirst(request,values,options).then(function(response) {
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
	
   
	//caching strategy for dev phase
	//global.toolbox.router.get('/*/*.bundle.js',global.toolbox.networkFirst,
	/*{
		cache : {
			name : 'bundle-cache',
			maxAgeSeconds : 86400,
			maxEntries : 5
		}
	})
*/

	/*Caching strategy for CDN images with different caches (to handle different capacities) for different dimension*/
	//TODO specify correct url for cdn urls
	//global.toolbox.router.get(SIZE_1+'/*',global.toolbox.cacheFirst,
	global.toolbox.router.get('/'+SIZE_0_0+'/*',global.toolbox.networkFirst,		
								{
									cache : {
												name : 'cdn-images-'+SIZE_0_0,
												maxAgeSeconds : IMAGE_EXPIRATION_TIME,
												maxEntries : 60
											},
									origin : IMAGE_CDN_REGEX_PATTERN
									
								});

//  /\/(174x98|160x89)\/(.*)$/
//  /\/(/+SIZE_2+/|/+SIZE_5+/)\/(.*)$/
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


	global.toolbox.router.get('//'+SIZE_174_98+'/*',global.toolbox.cacheFirst,
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



	/*caching strategy for JS on CDN*/
	// cdn CSS and JS is cacheFirst as different version are differentiated by different names.
	global.toolbox.router.get('/*',function(request,values,options) {
			return global.toolbox.cacheFirst(request,values,options).then(function(response) {
				console.log('here in js')
				return response;
			}).catch(function(err) {
				console.log(request.url+' failed')
				if(request.url === 'https://st.aeplcdn.com/staging/bikewale/pwa/build/vendor.bundle.js' ||
					request.url === 'https://st.aeplcdn.com/staging/bikewale/pwa/build/app.bundle.js') {
					console.log(request.url+' requested')
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


	/*
	google domain specific resources:
	https://fonts.gstatic.com/s/opensans/v13/k3k702ZOKiLJc3WVjuplzKRDOzjiPcYnFooOUGCOsRk.woff
	https://www.googletagmanager.com/gtm.js?id=GTM-5CSHD6 // CALLED
	https://www.google-analytics.com/analytics.js
	https://www.google-analytics.com/collect?v=1&_v=j49&a=744710600&t=pageview&_s=1&dl=https%3A%2F%2Fwww.bikewale.com%2Fm%2Fnews%2F28084-2017-tvs-jupiter-spied.html&ul=en-gb&de=UTF-8&dt=2017%20TVS%20Jupiter%20spied%20-%20BikeWale%20News&sd=24-bit&sr=320x568&vp=320x568&je=0&_u=QCCAgAAB~&jid=&cid=172250403.1483700496&tid=UA-34771801-1&gtm=GTM-5CSHD6&cd1=utmcsr%3D127.0.0.1%7Cutmgclid%3D%7Cutmccn%3D(referral)%7Cutmcmd%3Dreferral&cd2=2_Bangalore&z=1756596878
	https://tpc.googlesyndication.com/safeframe/1-0-6/html/container.html
	https://www.googletagservices.com/tag/js/gpt.js
	https://fonts.googleapis.com/css?family=Open+Sans:400,700 // CALLED
	*/

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


/////////////// WORKING EXAMPLE OF CROSS DOMAIN CACHING
/*
global.toolbox.router.get('/*',function(request,values,options) {
		console.log(options)
		console.log('here')
		return global.toolbox.networkFirst(request,values,options).then(function(response) {
			return response;
		}).catch(function(err) {
			console.log(err);
		})
	},
	{	
		cache :{
		
			name : 'images'
		},
		origin: /^https:\/\/imgd.aeplcdn.com/
	}
	)

*/

/* WORKING
	global.toolbox.router.get('/'+SIZE_2+'/*',function(request,values,options) {
		
				console.log('in router get function of imgd')
			return global.toolbox.networkFirst(request,values,options).then(function(response) {
				console.log('success in small news pic')
				return response;
			}).catch(function(err) {
				console.log('error in small news pic ');
				console.log(err)

			})
	}, {
		cache :{
									name : 'cdn-images-large',
									maxAgeSeconds : 86400,
									maxEntries : 20
								},
		origin: IMAGE_CDN_REGEX_PATTERN
	})*/