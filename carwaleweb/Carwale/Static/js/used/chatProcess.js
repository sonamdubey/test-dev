var chatProcess = (function () {
    /*
     this must run before applozic's js excute, so that the references
     $, jQuery and $original are set to the jQuery version that cw is using.
     This is done so that their is no conflict in case cw and applozic are 
     using different versions of jQuery.
     */
    var $original;
    var oModal = "";
    if (typeof jQuery !== 'undefined') {
        $original = jQuery.noConflict(true);
        $ = $original;
        jQuery = $original;
    }

    var isChatLead = false;
    var _isChatRegistered = false;
    var _chatWindowProperties = {};
    var $_applozicWtLauncherBtn;
    var _defaultMessage;
    var _chatLoginInfo = 'chatLoginInfo';
    var _totalUnreadMsgCount = 0;
    var _setChatWindowProperties = function (chatProperties) {
        _chatWindowProperties.title = chatProperties.title;
        _chatWindowProperties.imageUrl = chatProperties.imageUrl;
        _chatWindowProperties.subtitle = chatProperties.subtitle;
    };

    var getChatHtml = function (callback, chatIconVisibilityCallback, source, hideLoader) {
        $.ajax({
            type: "GET",
            url: "/used/chat/",
            success: function (data) {
                var chatLoginInfo = clientCache.get(_chatLoginInfo, false);
                callback(_isMyChatsVisible(), data);
                if (chatLoginInfo) {
                    var loginInfo = JSON.parse(chatLoginInfo);
                    _chatRegistration(loginInfo.appId, loginInfo.userId, loginInfo.accessToken, chatIconVisibilityCallback, source, hideLoader);
                }
            }
        });
    }

    var _isMyChatsVisible = function () {
        var chatLoginInfo = clientCache.get(_chatLoginInfo, false);
        var userCookie = $.cookie('TempCurrentUser');
        return chatLoginInfo && userCookie;
    }

    var getUnreadMsgCount = function (callback) {
        window.Applozic.ALApiService.getMessages({
            data: {},
            success: function (response) {
                if (response && response.data) {
                    var unreadMsgCount = 0;
                    var userDetails = response.data.userDetails;
                    unreadMsgCount = userDetails.reduce(function (count, currValue) {
                        return count += currValue.unreadCount > 0 ? currValue.unreadCount : 0;
                    }, unreadMsgCount);
                    _totalUnreadMsgCount = unreadMsgCount;
                    if (typeof callback === "function") {
                        callback(unreadMsgCount);
                    }
                }
            }
        });
    }

    var _saveChatLoginInfo = function (appId, userId, accessToken) {
        if (appId && userId && accessToken) {
            var loginInfo = {
                appId: appId,
                userId: userId,
                accessToken: accessToken
            };
            clientCache.set(_chatLoginInfo, JSON.stringify(loginInfo), false);
        }
    };

    var chatRegistrationResponse = {
        Success: 0,
        Invalid_Password: 1
    }

    var source = {
        mobileBrowser: 6,
        desktopBrowser: 5,
        web: 1
    }

    var processChatRegistration = function (appId, buyerInfo, callback, source, hideLoader) {
        if (buyerInfo.isChatLeadGiven !== false) {      //if isChatLeadgiven is false then only remove loginInfo, for all other case register chat
            var loginInfo = JSON.parse(clientCache.get(_chatLoginInfo, false));
            //if loginInfo in localStorage does not exist or loginInfo parameter not match with userId and accesstoken or chat is not registered then register chat 
            if (!(loginInfo && loginInfo.userId === buyerInfo.chatUserId && loginInfo.accessToken === buyerInfo.chatAccessToken && _isChatRegistered)) {
                clientCache.remove(_chatLoginInfo, false);
                _chatRegistration(appId, buyerInfo.chatUserId, buyerInfo.chatAccessToken, callback, source, hideLoader);
            }
        }
        else {
            clientCache.remove(_chatLoginInfo, false);
        }
    }

    var startChat = function (appId, $applozicWtLauncherBtn, seller, buyer, stock, callback) {
        if (stock) {
            _defaultMessage = "Hi, I would like to enquire about the " + stock.color + " " + stock.make + " " + stock.carName + " - " + stock.year + " which you have listed on CarWale";
        }
        // using applozic's jQuery version to create the object.
        $_applozicWtLauncherBtn = $applozic($applozicWtLauncherBtn[0]); // 

        //setting data attibutes using data method as they are retrieved using data method in applozic.sidebox.js.
        $_applozicWtLauncherBtn.data('mck-name', seller.name);
        $_applozicWtLauncherBtn.data('mck-id', seller.chatUserId);

        if (_isChatRegistered) {
            $_applozicWtLauncherBtn.trigger("click");
        }
    };

    var _resolveJQueryConflict = function () {
        //this sets $, jQuery to the $original(cw's jquery version, set at the starting of the chatProcess),
        //if $original exists, else $ and jQuery are set to $applozic(which are set to the jQuery version that applozic is using)
        if (typeof $original !== 'undefined') {
            $ = $original;
            jQuery = $original;
            if (typeof $.fn.modal === 'function') {
                oModal = $.fn.modal.noConflict();
            }
        } else {
            $ = $applozic;
            jQuery = $applozic;
            if (typeof $applozic.fn.modal === 'function') {
                oModal = $applozic.fn.modal.noConflict();
            }
        }
    };

    var _chatRegistration = function (appId, userId, accessToken, setChatIconVisibilty, source, hideLoader) {
        _resolveJQueryConflict();
        var userCookie = $.cookie('TempCurrentUser');
        if (userCookie) {
            var user = userCookie.split(':');
            $applozic.fn.applozic({
                baseUrl: _getApplozicBaseUrl(),
                appId: appId,
                userId: userId,
                userName: user[0],
                desktopNotification: false,
                source: source,
                notificationIconLink: 'https://www.applozic.com/resources/images/applozic_icon.png',
                authenticationTypeId: 1,
                accessToken: accessToken,
                locShare: true,
                autoTypeSearchEnabled: false,
                loadOwnContacts: false,
                ojq: $original,
                obsm: oModal,
                onInit: function (response, apiResult, userPxy) { // this callback gets called once, when the applozic login/initialization process is completed.
                    if (response === "success") {
                        _isChatRegistered = true;
                        getUnreadMsgCount(function (count) {
                            if (typeof setChatIconVisibilty === "function") {
                                setChatIconVisibilty(chatRegistrationResponse.Success, count);
                            }
                        });

                        if (!clientCache.get(_chatLoginInfo, false)) {
                            _saveChatLoginInfo(appId, userId, accessToken);
                        }
                        if (chatProcess.isChatLead && $_applozicWtLauncherBtn) {               //this click should only be triggered when the login is done after giving the fist chat lead
                            $_applozicWtLauncherBtn.trigger('click');//and not on subsequent chat leads or page reload.
                        }
                    }
                    else if (response.errorMessage === 'INVALID PASSWORD') {
                        if (typeof setChatIconVisibilty === "function") {
                            setChatIconVisibilty(chatRegistrationResponse.Invalid_Password);
                        }
                        clientCache.remove(_chatLoginInfo, false);
                        _logError(response.errorMessage, appId, userId, accessToken, apiResult, userPxy);
                    }
                    else {
                        _logError(response.errorMessage, appId, userId, accessToken, apiResult, userPxy);
                    }
                    if (hideLoader && typeof hideLoader === 'function') {
                        hideLoader();
                    }
                },
                onTabClicked: function (response) {
                    if (response && response.hasNoMessage) {
                        $("#mck-text-box").html(_defaultMessage);
                    }
                },
                processUnreadCount: function (count) {
                    //count contains the total unread count returned by applozic api. 
                    //_totalUnreadMsgCount is set to the total unread messsage count on page load.
                    //if count passed is greater than 0, _totalUnreadMsgCount is incremented by 1,
                    //else it is set to zero.
                    _totalUnreadMsgCount = count > 0 ? _totalUnreadMsgCount + 1 : count;
                    setChatIconVisibilty(chatRegistrationResponse.Success, _totalUnreadMsgCount);
                },
                hideLoader: hideLoader
            });
        }
    };

    var _logError = function (errorMessage, appId, userId, accessToken, apiResult, userPxy) {
        var chatError = {
            "errorMessage": errorMessage, "appId": appId, "userId": userId, "accessToken": accessToken, "apiResult": apiResult, "userPxy": userPxy
        };
        $.ajax({
            type: "POST",
            url: "/api/chat/errors/",
            data: JSON.stringify(chatError),
            contentType: 'application/json',
            dataType: 'json'
        });
    }

    var _getApplozicBaseUrl = function () {
        return commonUtilities.getBaseUrl() === 'https://www.carwale.com' ? 'https://chat.applozic.com' : undefined;
    }

    var _getApplozicBaseUrl = function () {
        return commonUtilities.getBaseUrl() === 'https://www.carwale.com' ? 'https://chat.applozic.com' : undefined;
    }

    return {
        isChatLead: isChatLead,
        startChat: startChat,
        getUnreadMsgCount: getUnreadMsgCount,
        chatRegistrationResponse: chatRegistrationResponse,
        getChatHtml: getChatHtml,
        processChatRegistration: processChatRegistration,
        source: source
    };
})();