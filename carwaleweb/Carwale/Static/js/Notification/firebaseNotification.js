var initialiseFCM = (function(){
    
    var eventCategory = "WebNotification";

    var _addToLabel = function (label, key, value) {
        if (label.length > 0) {
            label += '|' + key + '=' + value;
        }
        else {
            label = key + '=' + value;
        }
        return label;
    };

    var getEventLabel = function () {
        var label = '';
        label = _addToLabel(label, 'platform', ( isMobile ? 'Mobile' : 'Desktop'));
        label = label + (((typeof variantCustomPopup != 'undefined') && (isNotificationShow.showCustomPopup())) ? '|' + variantCustomPopup : '');
        return label;
    };

    var _getRegistrationUrl = function (isRegister, fcmToken) {
        var os = isMobile ? '&os=43' : '&os=3';
        var cwc_cookie = '?imei=' + $.cookie('CWC');
        var gcmId = '&gcmid=' + fcmToken;
        var subsMasterId = isRegister ? '&subsmasterid=1' : '';
        return '/api/MobileAppAlert/GetManageMobileAppAlerts/' + cwc_cookie + os + gcmId + subsMasterId;
    };

    var saveFcmTokenId = function (isRegister) {
        var isNotificationAllowed = $.cookie("_notificationAllowed") || '0';
        if (typeof FCMmessaging !== "undefined") {
            FCMmessaging.getToken()
                  .then(function (token) {
                      if (isNotificationAllowed === "0" && isRegister) {
                          _sendRequest(token, isRegister);
                      }
                  })
                  .catch(function (err) {
                      //to unsubscribe user
                      if (isNotificationAllowed === "1" && !isRegister) {
                          _sendRequest('NA', isRegister);
                      }

                      if (isRegister) {
                          cwTracking.trackCustomData(initialiseFCM.eventCategory, "FCMPopupError", initialiseFCM.getEventLabel() + '|errorlog=failed to generate fcm token', false);
                      }
                  });
        }
    };

    var _sendRequest = function (fcmToken, isRegister) {
        var request;
        if (window.XMLHttpRequest) {
            // code for modern browsers
            request = new XMLHttpRequest();
        }
        else {
            // code for old IE browsers
            request = new ActiveXObject("Microsoft.XMLHTTP");
        }
        request.onreadystatechange = function () {
            if (this.readyState == 4 && this.status != 200) {
                cwTracking.trackCustomData(initialiseFCM.eventCategory, "FCMPopupError", initialiseFCM.getEventLabel() + '|errorlog=failed to save user', false);
            }
        };
        request.open('GET', _getRegistrationUrl(isRegister, fcmToken), true);
        request.send();
        SetCookieInDays('_notificationAllowed', isRegister ? 1 : 0, 365);
    };

    var showBrowserNotificationPopup = function () {
        isNotificationShow.isCustomPopup = false;
        isNotificationShow.registerBrowserClose();
        //tracking confirmation pop up impression
        cwTracking.trackCustomData(initialiseFCM.eventCategory, "ConfirmationPopupImpression", initialiseFCM.getEventLabel(), false);
        //ga tracking
        cwTracking.trackAction('CWNonInteractive', initialiseFCM.eventCategory, "ConfirmationPopupImpression", initialiseFCM.getEventLabel());
       
        if (typeof FCMmessaging !== "undefined") {
            FCMmessaging.requestPermission()
              .then(function () {
                  //tracking allow click on confirmation popup
                  cwTracking.trackCustomData(initialiseFCM.eventCategory, "ConfirmationPopupAllowClick", initialiseFCM.getEventLabel(), false);
                  //ga tracking
                  cwTracking.trackAction('CWInteractive', initialiseFCM.eventCategory, "ConfirmationPopupAllowClick", initialiseFCM.getEventLabel());
                  SetCookieInDays('_notificationAllowed', 0, 365);
                  initialiseFCM.saveFcmTokenId(true);
                  //unregister pagechanged event
                  isNotificationShow.unregisterBrowserClose();
              })
              .catch(function (err) {
                  var isNotificationAllowed = $.cookie("_notificationAllowed") || 0;
                  //unregister pagechanged event
                  isNotificationShow.unregisterBrowserClose();
                  if (Notification.permission == "denied") {
                      if (isNotificationAllowed === "1") {
                          initialiseFCM.saveFcmTokenId(false);
                          SetCookieInDays('_notificationAllowed', 0, 365);
                      }
                      //tracking block click on confirmation popup
                      cwTracking.trackCustomData(initialiseFCM.eventCategory, "ConfirmationPopupDenyClick", initialiseFCM.getEventLabel(), false);
                      //ga tracking
                      cwTracking.trackAction('CWInteractive', initialiseFCM.eventCategory, "ConfirmationPopupDenyClick", initialiseFCM.getEventLabel());
                  }
                  else {
                      //tracking close click on confirmation popup
                      cwTracking.trackCustomData(initialiseFCM.eventCategory, "ConfirmationPopupCloseClick", initialiseFCM.getEventLabel(), false);
                      //ga tracking
                      cwTracking.trackAction('CWInteractive', initialiseFCM.eventCategory, "ConfirmationPopupCloseClick", initialiseFCM.getEventLabel());
                      //store in session
                      clientCache.set('_notificationClosed', 1, true);
                  }
              });
        }
    };

    return {
        showBrowserNotificationPopup: showBrowserNotificationPopup,
        saveFcmTokenId: saveFcmTokenId,
        getEventLabel: getEventLabel,
        eventCategory: eventCategory,
    };

})();



