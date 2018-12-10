importScripts("/static/js/notification/customTracking.js");

var landingUrl = "https://www.carwale.com/",    
    defaultIcon = "https://imgd.aeplcdn.com/0x0/cw/static/icons/cw-logo-crest.jpg",
    defaulTitle = "CarWale";


self.addEventListener('install', function (event) {    
    event.waitUntil(self.skipWaiting());
});

self.addEventListener('activate', function (event) {    
    event.waitUntil(self.clients.claim());
});

self.addEventListener('push', function (event) {
    var notificationData = event.data.json();
        var options = {
            body: notificationData.data.title,
            icon: defaultIcon,
            image: notificationData.data.largePicUrl ? notificationData.data.largePicUrl.replace("http:", "https:") : "",
            vibrate: [300, 100, 400], // Vibrate 300ms, pause 100ms, then vibrate 400ms
            data: notificationData.data
        }
        event.waitUntil(
            Promise.all([
                self.registration.showNotification(defaulTitle, options),
                fetch(customTracking.getTrackingUrl("WebNotification", "NotificationImpression", customTracking.getEventLabel(notificationData.data.title, notificationData.data.alertId, notificationData.data.alertTypeId)))
            ])
        );
});


self.addEventListener('notificationclick', function (event) {
    event.notification.close();

    var notificationData = event.notification.data;    
    let clickResponsePromise = Promise.resolve();
    if (notificationData && notificationData.detailUrl) {
        clickResponsePromise = clients.openWindow(notificationData.detailUrl);
    }

    event.waitUntil(
      Promise.all([
        clickResponsePromise,
        fetch(customTracking.getTrackingUrl("WebNotification", "NotificationClick", customTracking.getEventLabel(notificationData.title, notificationData.alertId, notificationData.alertTypeId)))
      ])
    );
});

self.addEventListener('notificationclose', function (event) {
    var notificationData = event.notification.data;
    event.waitUntil(
       Promise.all([
         fetch(customTracking.getTrackingUrl("WebNotification", "NotificationClose", customTracking.getEventLabel(notificationData.title, notificationData.alertId, notificationData.alertTypeId)))
       ])
     );

});
