var customNotificationPopup = $("#customNotificationPopup");

var isNotificationShow = (function () {

    var _FCMconfig = {
        messagingSenderId: "491935823116"
    };

    var iscloseClicked = true;

    var isCustomPopup = false;

    var showCustomPopup = function () {        
        var abtest = $.cookie("_abtest") || '0';
        return (!isMobile && abtest >= 1 && abtest <= 10);     
    };

    var _showCustomNotificationPopup = function () {
        PrivateMode().then(function (isPrivateMode) {
            if (!isPrivateMode) {
                var isNotificationAllowed = $.cookie("_notificationAllowed") || '0';
                var isNotificationClosed = clientCache.get("_notificationClosed", true) || '0';
                if (window.Notification && Notification.permission === "default" && isNotificationAllowed !== '2' && firebase.messaging.isSupported()) {
                    if (isNotificationClosed === '0')
                    {
                        isNotificationShow.isCustomPopup = true;
                        registerBrowserClose();
                        if (isMobile) {
                            window.history.pushState('customNotificationPopup', '', '');
                            //lock scroll
                            if (typeof HandelBodyScroll !== "undefined") {
                                HandelBodyScroll.lockScroll();
                            }
                        }

                        customNotificationPopup.removeClass('hide');
                        //tracking bhrighu
                        cwTracking.trackCustomData(initialiseFCM.eventCategory, "CustomPopupImpression", initialiseFCM.getEventLabel(), false);
                        //ga tracking
                        cwTracking.trackAction('CWNonInteractive', initialiseFCM.eventCategory, "CustomPopupImpression", initialiseFCM.getEventLabel());
                    }
                    else if(isNotificationClosed == '2')
                    {
                        isNotificationShow.isCustomPopup = false;
                        registerBrowserClose();
                        initialiseFCM.showBrowserNotificationPopup();
                    } 
                }
                else if (window.Notification && Notification.permission === "denied") {
                    initialiseFCM.saveFcmTokenId(false);
                }
                else if (window.Notification && Notification.permission === "granted") {
                    initialiseFCM.saveFcmTokenId(true);
                }
            }
        });
    };

    var _showBrowserNotification = function () {
        var isNotificationAllowed = $.cookie("_notificationAllowed") || '0';
        var isNotificationClosed = clientCache.get("_notificationClosed", true) || '0';
        PrivateMode().then(function (isPrivateMode) {
            if (!isPrivateMode) {
                if (window.Notification && Notification.permission === "default" && isNotificationClosed === '0' && isNotificationAllowed !== '2' && firebase.messaging.isSupported()) {
                    isNotificationShow.isCustomPopup = false;
                    registerBrowserClose();

                    initialiseFCM.showBrowserNotificationPopup();
                }
                else if (window.Notification && Notification.permission === "denied") {
                    initialiseFCM.saveFcmTokenId(false);
                }
                else if (window.Notification && Notification.permission === "granted") {
                    initialiseFCM.saveFcmTokenId(true);
                }
            }
        });
    };



    var registerBrowserClose = function () {
        window.addEventListener('beforeunload', _trackBrowserClose)
    };

    var unregisterBrowserClose = function () {
        window.removeEventListener('beforeunload', _trackBrowserClose)
    };

    var _trackBrowserClose = function () {
        console.log((isNotificationShow.isCustomPopup ? "CustomPopupPageChanged" : "PageChanged"));
        //tracking bhrighu
        cwTracking.trackCustomData(initialiseFCM.eventCategory, (isNotificationShow.isCustomPopup ? "CustomPopupPageChanged" : "PageChanged"), initialiseFCM.getEventLabel(), false);
        //ga tracking
        cwTracking.trackAction('CWInteractive', initialiseFCM.eventCategory, (isNotificationShow.isCustomPopup ? "CustomPopupPageChanged" : "PageChanged"), initialiseFCM.getEventLabel());
    };

    var showNotificationPermission = function () {
        if ('serviceWorker' in navigator) {
            navigator.serviceWorker.register('/root-sw.js')
            .then(function (registration) {
                firebase.initializeApp(_FCMconfig);
                FCMmessaging = firebase.messaging();
                FCMmessaging.useServiceWorker(registration);
                (showCustomPopup()) ? _showCustomNotificationPopup() : _showBrowserNotification();
            })
        .catch(function (err) {
            cwTracking.trackCustomData(initialiseFCM.eventCategory, "FCMPopupError", initialiseFCM.getEventLabel() + '|errorlog=failed to register service worker', false);
        });
        }
    };

    var $window = $(window);
    
    var togglePopup = function () {
        var windowScrollTop = $window.scrollTop();
        var isNotificationAllowed = $.cookie("_notificationAllowed") || '0';
        var isNotificationClosed = clientCache.get("_notificationClosed", true) || '0';
        if (!isMobile && isNotificationClosed === '0' && isNotificationAllowed !== '2') {
            if (windowScrollTop > $window.height() / 4 + 130) {
                $('.notification-popup-container').addClass('hide');
            }
            else {
                $('.notification-popup-container').removeClass('hide');
            }
        }
    };

    return {
        iscloseClicked: iscloseClicked,
        showNotificationPermission: showNotificationPermission,
        registerBrowserClose: registerBrowserClose,
        unregisterBrowserClose: unregisterBrowserClose,
        showCustomPopup: showCustomPopup,
        togglePopup: togglePopup
    };
})();

$("#notification-no-btn").click(function () {
    isNotificationShow.isCustomPopup = true;
    isNotificationShow.unregisterBrowserClose();
    SetCookieInDays('_notificationAllowed', 2, 365);
    //tracking bhrighu
    cwTracking.trackCustomData(initialiseFCM.eventCategory, "CustomPopupDenyClick", initialiseFCM.getEventLabel(), false);
    //ga tracking
    cwTracking.trackAction('CWInteractive', initialiseFCM.eventCategory, "CustomPopupDenyClick", initialiseFCM.getEventLabel());
    isNotificationShow.iscloseClicked = false;
    if (isMobile) {
        history.back();
    }
    else {
        customNotificationPopup.addClass('hide');
    }
});

$(".notification-close-btn").click(function () {
    isNotificationShow.isCustomPopup = true;
    isNotificationShow.unregisterBrowserClose();
    //store in session
    clientCache.set('_notificationClosed', 1, true);
    //tracking bhrighu
    cwTracking.trackCustomData(initialiseFCM.eventCategory, "CustomPopupCloseClick", initialiseFCM.getEventLabel(), false);
    //ga tracking
    cwTracking.trackAction('CWInteractive', initialiseFCM.eventCategory, "CustomPopupCloseClick", initialiseFCM.getEventLabel());
    isNotificationShow.iscloseClicked = false;
    if (isMobile) {
        history.back();
    }
    else {
        customNotificationPopup.addClass('hide');
    }
});

$("#notification-notify-btn").click(function () {
    isNotificationShow.isCustomPopup = false;
    clientCache.set('_notificationClosed', 2, true);
    //tracking bhrighu
    cwTracking.trackCustomData(initialiseFCM.eventCategory, "CustomPopupAllowClick", initialiseFCM.getEventLabel(), false);
    //ga tracking
    cwTracking.trackAction('CWInteractive', initialiseFCM.eventCategory, "CustomPopupAllowClick", initialiseFCM.getEventLabel());
    isNotificationShow.iscloseClicked = false;
    if (isMobile) {
        history.back();
    }
    else {
        customNotificationPopup.addClass('hide');
    }
    initialiseFCM.showBrowserNotificationPopup();
});

window.addEventListener("popstate", function () {
    if (isMobile && customNotificationPopup.length && !customNotificationPopup.hasClass("hide")) {
        customNotificationPopup.addClass('hide');

        //unlock scroll
        if (typeof HandelBodyScroll !== "undefined") {
            HandelBodyScroll.unlockScroll();
        }

        if (isNotificationShow.iscloseClicked) {
            isNotificationShow.isCustomPopup = true;
            isNotificationShow.unregisterBrowserClose();
            //store in session
            clientCache.set('_notificationClosed', 1, true);
            //tracking bhrighu
            cwTracking.trackCustomData(initialiseFCM.eventCategory, "CustomPopupBackClicked", initialiseFCM.getEventLabel(), false);
            //ga tracking
            cwTracking.trackAction('CWInteractive', initialiseFCM.eventCategory, "CustomPopupBackClicked", initialiseFCM.getEventLabel());
        }
    }
});

$(window).scroll(function () {
    isNotificationShow.togglePopup();
});

window.addEventListener('load', isNotificationShow.showNotificationPermission);

