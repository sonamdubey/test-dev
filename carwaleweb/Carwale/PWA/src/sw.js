workbox.skipWaiting();
workbox.clientsClaim();
PRECACHE_NAME = workbox.core.cacheNames.precache;
// This is a placeholder keyword. At build-time, Workbox injects the list of files to cache into the array.
workbox.precaching.precacheAndRoute(self.__precacheManifest || []);

workbox.routing.registerRoute(
  /.*(?:googleapis)\.com.*$/,
  workbox.strategies.cacheFirst({
    cacheName: 'googleapis',
    plugins:[
      new workbox.expiration.Plugin({
          maxEntries: 30,
          maxAgeSeconds: 24 * 60 * 60, // 1 day
      })
    ],
    cacheableResponse: { statuses: [0, 200] },
  }),
  'GET'
);

/**
 * Because the image responses are cross-domain and don't use CORS, they will
 * be "opaque", and have a status code of 0. When using a cache-first strategy,
 * we need to explicitly opt-in to caching responses with a status of 0.
 * https://stackoverflow.com/questions/36292537/what-is-an-opaque-request-and-what-it-serves-for
 */
workbox.routing.registerRoute(/.*(?:aeplcdn).*\.(?:png|gif|jpg|jpeg).*$/, workbox.strategies.cacheFirst({
  cacheName: "image-cache",
  plugins:[
    new workbox.expiration.Plugin({
        maxEntries: 60,
        maxAgeSeconds: 30 * 24 * 60 * 60, // 30 Days
    })
  ],
  cacheableResponse: { statuses: [0, 200] },
}), 'GET');

workbox.routing.registerRoute(/.*(?:aeplcdn).*\.(?:svg).*$/, workbox.strategies.cacheFirst({
  cacheName: "icon-cache",
  plugins: [
    new workbox.expiration.Plugin({
        maxEntries: 60,
        maxAgeSeconds: 30 * 24 * 60 * 60, // 30 Days
    })
  ],
  cacheableResponse: { statuses: [0, 200] },
}), 'GET');

workbox.routing.registerRoute(
  /.*(?:aeplcdn).*(?:js|css)/,
  workbox.strategies.cacheFirst({
    cacheName: 'static-resources',
    plugins: [
      new workbox.expiration.Plugin({
          maxEntries: 30,
          maxAgeSeconds: 30 * 24 * 60 * 60, // 30 Days
      })
    ],
    cacheableResponse: { statuses: [0, 200] },
  }), 'GET'
);

workbox.routing.registerRoute(
  /.*(?:carwale).*(?:api).*([^.*api/newcarfinder/screen])/,
  workbox.strategies.cacheFirst({
    cacheName: 'cw-apis',
    plugins: [
      new workbox.expiration.Plugin({
          maxEntries: 60,
          maxAgeSeconds: 300, // 5 minutes
      })
    ],
    cacheableResponse: { statuses: [0, 200] },
  }), 'GET'
);

