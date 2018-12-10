var cookie = (function () {
    if (typeof window.jQuery != 'undefined') {

        function isCookiePresent(cookieName) {
            var cookieValue = $.cookie(cookieName);
            return cookieValue && cookieValue != "-1";
        };

        function getCookie(cookieName) {
            return $.cookie(cookieName);
        };

        function setCookie(cookieName, cookieValue, cookieOptions) {
            if (cookieName && cookieValue) {
                try {
                    if (cookieOptions) {
                        if (typeof cookieOptions == 'object')
                            $.cookie(cookieName, cookieValue, cookieOptions);
                        else
                            return false;
                    } else {
                        $.cookie(cookieName, cookieValue);
                    }
                } catch (err) {
                    return false;
                }
                return true;
            }
            return false;
        };

        function deleteCookie(cookieName, cookieOptions) { 
            if (isCookiePresent(cookieName)) {
                return $.cookie(cookieName, null, cookieOptions);
            }
            return false;
        };
        return {
            isCookiePresent: isCookiePresent,
            getCookie: getCookie,
            setCookie: setCookie,
            deleteCookie: deleteCookie
        };
    }
    return null;
})();