var baseUrl = '/';
var APPSHELL = baseUrl + 'pwa/appshell.html';
var WORKBOX_JS = baseUrl + 'pwa/workbox-sw.prod.v2.1.2.js';
importScripts(WORKBOX_JS);
const workboxSW = new WorkboxSW({
    cacheId: "bw",
    skipWaiting: true,
    clientsClaim: true
});
workboxSW.precache([APPSHELL]);
PRECACHE_NAME = workboxSW._revisionedCacheManager.getCacheName();

function fetchRequest(url) {
    return fetch(url).then(function (response) {
        if (!response.ok) {
            throw new TypeError('bad response status');
        }
        return response;
    }).catch(function (err) { })
}

workboxSW.router.registerRoute(/.*\/m\/(news|bike-videos).*/, function (input) {
    if (input.event.request.method === 'GET' && input.event.request.headers.get('accept').includes('text/html')) {
        return caches.match(APPSHELL).then(function (response) {
            if (response) {
                return response;
            }
            else {
                caches.open(PRECACHE_NAME).then(function (cache) {
                    fetch(APPSHELL).then(function (response) {
                        cache.put(APPSHELL, response);
                    })
                })
                return fetchRequest(input.url.href);

            }
        }).catch(function (error) {
            fetchRequest(input.url.href);
        })
    }
    else {
        fetchRequest(input.url.href);
    }
});


workboxSW.router.registerRoute(/.*\/m\/news\/.*-\d+\/amp\/?/, workboxSW.strategies.networkOnly(), 'GET');

workboxSW.router.registerRoute(/.*\/api\/pwa\/.*$/,
    workboxSW.strategies.cacheFirst({
        cacheName: 'api',
        cacheExpiration: {
            maxEntries: 100,
            maxAgeSeconds: 300
        },
        cacheableResponse: { statuses: [200] }
    }), 'GET'
);

workboxSW.router.registerRoute(/.*imgd\d?.aeplcdn.*/,
    workboxSW.strategies.cacheFirst({
        cacheName: 'cdn-images',
        cacheExpiration: {
            maxEntries: 150,
            maxAgeSeconds: 864000
        },
        cacheableResponse: { statuses: [0, 200] }
    })
);

workboxSW.router.registerRoute(/.*stb.aeplcdn.*/,
    workboxSW.strategies.cacheFirst({
        cacheName: 'cdn-js-css',
        cacheExpiration: {
            maxEntries: 10,
            maxAgeSeconds: 864000
        },
        cacheableResponse: { statuses: [0, 200] }
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