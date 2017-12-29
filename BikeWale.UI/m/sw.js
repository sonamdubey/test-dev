var baseUrl = '/';
var APPSHELL = baseUrl + 'pwa/appshell.html';
var WORKBOX_JS = baseUrl + 'pwa/workbox-sw.prod.v2.1.2.js';
importScripts(WORKBOX_JS);
const workboxSW = new WorkboxSW({
    cacheId: "bw",
    skipWaiting: true,
    clientsClaim : true
});
workboxSW.precache([APPSHELL]); 

workboxSW.router.registerNavigationRoute(APPSHELL, {
    whitelist: [
        /.*\/m\/(news|bike-videos).*/
    ],
    blacklist: [
        /.*\/m\/news\/.*-\d+\/amp\/?/
    ]
});
workboxSW.router.registerRoute(/.*\/api\/pwa\/.*$/,
    workboxSW.strategies.cacheFirst({
        cacheName: 'api',
        cacheExpiration: {
            maxEntries: 100,
            maxAgeSeconds:300
        },
        cacheableResponse : {statuses : [200]}
    }) ,'GET'
);

workboxSW.router.registerRoute(/.*imgd\d?.aeplcdn.*/,
    workboxSW.strategies.cacheFirst({
        cacheName: 'cdn-images',
        cacheExpiration: {
            maxEntries: 150,
            maxAgeSeconds: 864000
        },
        cacheableResponse: { statuses: [0,200] }
    })
);

workboxSW.router.registerRoute(/.*stb.aeplcdn.*/,
    workboxSW.strategies.cacheFirst({
        cacheName: 'cdn-js-css',
        cacheExpiration: {
            maxEntries: 10,
            maxAgeSeconds: 864000
        },
        cacheableResponse: { statuses: [0,200] }
    })
);

workboxSW.router.registerRoute(/.*fonts\.(googleapis|gstatic).*/,
    workboxSW.strategies.cacheFirst({
        cacheName: 'google-resources',
        cacheExpiration: {
            maxEntries: 10,
            maxAgeSeconds: 864000
        },
        cacheableResponce: { statuses: [200] }
}));








//var version = '6Nov2017v1';
//var baseUrl = '/';
//var APPSHELL = baseUrl + 'pwa/appshell.html';
//var SW_TOOLBOX_JS = baseUrl + 'pwa/sw-toolbox.js';
//var IMAGE_EXPIRATION_TIME = 864000;
//var IMAGE_CDN_REGEX_PATTERN = /^https:\/\/imgd(\d)?.aeplcdn.com/;
//var ST_CDN_REGEX_PATTERN = /^https:\/\/stb.aeplcdn.com/;
//var precachedFiles = [APPSHELL];
//var precacheName = "bw-precache";
//importScripts(SW_TOOLBOX_JS);
//toolbox.options.cache = { name: precacheName };
//toolbox.options.debug = false;
//toolbox.precache(precachedFiles);
//function fetchFileFromCache(request, cacheName) {
//    return caches.open(cacheName).then(function (cache) {
//        return cache.match(request).then(function (response) {
//            if (response) {
//                return response;
//            }
//            else {
//                fetch(request).then(function (response) {
//                    if (response.status == 200) {
//                        cache.add(request);
//                    }
//                })
//                return null;
//            }
//        })
//    })
//};

//toolbox.router.get('/api/pwa/*', toolbox.cacheFirst,
//                                {
//                                    cache: {
//                                        name: 'api',
//                                        maxAgeSeconds: 300,
//                                        maxEntries: 100
//                                    }
//                                });

//toolbox.router.get('/m/news/*-(\\d+)/amp/', toolbox.networkOnly, null);

//toolbox.router.get('/m/(news|bike-videos)/*', function (request, values, options) {
//    if ('GET' === request.method && request.headers.get('accept').includes('text/html')) {
//        return fetchFileFromCache(APPSHELL, precacheName).then(function (response) {
//            if (response)
//                return response;
//            return toolbox.networkOnly(request, values, options);
//        }).catch(function () {
//            return toolbox.networkOnly(request, values, options);
//        });
//    }
//    else
//        return toolbox.networkOnly(request, values, options);
//},
//    {
//        cache: {
//            name: 'html-resource',
//            maxEntries: 25,
//            maxAgeSeconds: 86400
//        }
//    });

    

//toolbox.router.get('/*', toolbox.cacheFirst,
//        {
//            cache: {
//                name: 'cdn-images',
//                maxAgeSeconds: IMAGE_EXPIRATION_TIME,
//                maxEntries: 150
//            },
//            origin: IMAGE_CDN_REGEX_PATTERN
//        });
//toolbox.router.get('/*', toolbox.cacheFirst,
//    {
//        mode: "cors",
//        cache: {
//            name: 'cdn-JS',
//            maxAgeSeconds: 864000,
//            maxEntries: 10
//        },
//        origin: ST_CDN_REGEX_PATTERN
//    }
//);

//toolbox.router.get('/*', toolbox.cacheFirst, {
//    cache: {
//        name: 'google-resources',
//        maxAgeSeconds: 864000,
//        maxEntries: 10
//    },
//    origin: /^(https:\/\/fonts.googleapis.com)|(https:\/\/fonts.gstatic.com)/
//});
//self.addEventListener('install', function (event) {
//    event.waitUntil(
//        self.skipWaiting()
//    );
//});
//self.addEventListener('activate', function (event) {
//    event.waitUntil(
//        self.clients.claim()
//    );
//});