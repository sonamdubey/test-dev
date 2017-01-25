//local storage Library
(function (window) {
    'use strict';
    //main function to define ls functions within
    function _bwcachelib(settings) {
        var _bwcache = {};
        var webStorageSupported = false;
        var options = {
            StoragePrefix: 'bwc_',
            StorageScope: '',
            IsLocalStorageSupported: true,   //since latest browser support local storage
            IsSessionStorageSupported: true,
            StorageTime: 30, //time in minutes (new Date().getTime() + 30 * 60000)
            IsSessionStorage: false,
            //IsSafariBrowser: navigator.userAgent.toLowerCase().indexOf('safari') != -1,  // handle for safari browser (/Safari/.test(navigator.userAgent) && /Apple Computer/.test(navigator.vendor))
            FallBack: false
        };

        _bwcache.setOptions = function (settings) {
            if (typeCheck(settings, 'object')) {
                options.StorageScope = settings.StorageScope || '';
                options.StorageTime = settings.StorageTime || 30;
                options.FallBack = settings.FallBack || false;
            }
            else console.log("Options should be object ith settings parameters to tweak");
        };

        //check support for local storage in browsers used to handle fall back methods
        var checkStorageSupport = function (type) {
            try {
                var storage = window[type], x = '_tmpls';
                localStorage.setItem(x, x);
                localStorage.removeItem(x);
                return true;
            } catch (e) {
                return false;
            }
        };

        //init function require to set options and other related work to local storage library
        //we can add other default related functions
        _bwcache.init = function () {
            //check for local storage support and save the status
            options.IsSessionStorageSupported = checkStorageSupport("sessionStorage");
            options.IsLocalStorageSupported = checkStorageSupport("localStorage");
            webStorageSupported = options.IsSessionStorageSupported || options.IsLocalStorageSupported;
        };

        var typeCheck = function (item, type) { return (typeof (item) === type); }

        //function to set the scope for the availablity of the local storage values 
        _bwcache.setScope = function (scope) {
            if (typeCheck(scope, 'string')) {
                options.storageScope = scope;
            }
            else {
                console.log("Scope name should be string type");
            }
        };

        var createKey = function (key, scope) {
            if (typeCheck(key, 'string') && (!scope || typeCheck(scope, 'string')))
                return (options.StoragePrefix + (scope ? scope + '_' : (options.StorageScope != '' ? options.StorageScope + '_' : '')) + key);
            return key;
        };

        //function to retrieve localstorage item value by key
        var getbyKey = function (key, storage) {
            if (typeCheck(key, 'string')) {
                var _item_ = JSON.parse(storage.getItem(key)) || {};
                if (!_item_.expiryTime || _item_.expiryTime == 0 || ((new Date).getTime() <= _item_.expiryTime))
                    return _item_.value || null; //if exists returns value else return null
                else return null;
            }
            else console.log("Check for expiry time should be string first argument");
        };

        //function to retrieve localstorage item value by key and check fot expiry
        var getbyExpiry = function (key, expiry, storage) {
            if (typeCheck(key, 'string')) {
                var currentTime = (new Date).getTime();
                if (typeCheck(expiry, 'number')) currentTime -= (expiry * 60000);
                if (typeCheck(expiry, 'boolean') || typeCheck(expiry, 'number')) {
                    var _item_ = JSON.parse(storage.getItem(key)) || {};
                    var time = _item_.expiryTime || 1;
                    //if exists returns value else return null for timeout object
                    if (time < currentTime) {
                        return _item_.value || null;
                    }
                    else console.log("Storage item expired");
                }
                else return getbyKey(key, storage);
                //else console.log("Storage expiry type should be in number or boolean");

            }
            else console.log("Storage key should be in string");
        };

        _bwcache.get = function (key, isSession) {
            key = createKey(key);
            if (webStorageSupported) {
                var storage = ((isSession && typeCheck(isSession, 'boolean')) || options.IsSessionStorage) ? window.sessionStorage : window.localStorage;
                if (typeCheck(key, 'object')) {
                    return getbyExpiry(createKey(key.key), key.expiry, (!key.isSession) ? storage : window.sessionStorage);
                }
                else return getbyKey(key, storage);
            }
            else {
                console.warn("local storage not supported");
                if (options.FallBack)
                    return _bwcache.fallback.getCookie(key);
            }
        };

        //function to retrieve localstorage item value by key
        _bwcache.getByScope = function (key, scope, isSession) {
            var arr = [];
            if (webStorageSupported) {
                if (typeCheck(key, 'string') && typeCheck(scope, 'string')) {
                    key = createKey(key, scope);
                    var storage = ((isSession && typeCheck(isSession, 'boolean')) || options.IsSessionStorage) ? window.sessionStorage : window.localStorage;
                    for (var i in storage)
                        if (storage[i].indexOf(scope) > -1) arr[i] = storage[i];
                }
                else {
                    console.log("Check for expiry time should be string first argument");
                }
            }
            else {
                console.warn("local storage not supported");
                if (options.FallBack) arr = _bwcache.fallback.getCookiebyScope(key);
            }

            return arr;
        };

        _bwcache.remove = function (key, isSession) {
            key = key.indexOf(options.StoragePrefix) > -1 ? key : createKey(key);
            if (webStorageSupported) {

                if (typeCheck(key, 'string')) {
                    var storage = ((isSession && typeCheck(isSession, 'boolean')) || options.IsSessionStorage) ? window.sessionStorage : window.localStorage;
                    storage.removeItem(key);
                }
            }
            else {
                console.warn("local storage not supported");
                if (options.FallBack) _bwcache.fallback.removeCookie(key);
            }
        };

        _bwcache.removeAll = function (isExpired) {
            if (webStorageSupported) {
                var storage = ((isSession && typeCheck(isSession, 'boolean')) || options.IsSessionStorage) ? window.sessionStorage : window.localStorage;

                if (typeCheck(isExpired, 'boolean')) {
                    var currentTime = (new Date).getTime();
                    for (var i in storage) {
                        if (i.indexOf(options.StoragePrefix) > -1) {
                            var _item_ = JSON.parse(storage[i]) || {};
                            var time = _item_.expiryTime || 1;
                            if (time < currentTime) storage.removeItem(i)(i);

                        }
                    }
                }
                else if (typeCheck(isExpired, 'undefined')) {
                    for (var i in bwc) {
                        if (i.indexOf(options.StoragePrefix) > -1)
                            storage.removeItem(i);
                    }
                }

            }
            else {
                console.warn("local storage not supported");
                if (options.FallBack) return _bwcache.fallback.removeAllCookie(isExpired);
            }
        }

        //here item is an object with key-value and some other related properties
        _bwcache.setKeyValue = function (item, storage) {
            if (typeCheck(item, 'object')) {
                if (typeCheck(item.expiryTime, 'number') && item.expiryTime != 0) //set specified expiry along with data
                {
                    item.expiryTime = (new Date).getTime() + (item.expiryTime * 60000) //in minutes	
                }
                else if (typeCheck(item.expiryTime, 'boolean')) {
                    item.expiryTime = 0;
                }
                storage.setItem(item.key, JSON.stringify(item));
            }
            else console.log("Check for expiry time should be string first argument");
        };

        _bwcache.set = function (key, value, isSession) {
            key = createKey(key);
            if (webStorageSupported) {
                var _item_ = {
                    'key': key,
                    'value': value
                };
                var storage = ((isSession && typeCheck(isSession, 'boolean')) || (typeCheck(isSession, 'undefined') && typeCheck(value, 'boolean')) || options.IsSessionStorage) ? window.sessionStorage : window.localStorage;
                if (typeCheck(key, 'object')) {
                    _item_ = {
                        'key': createKey(key.key, key.scope ? key.scope : ''),
                        'value': key.value
                    };
                    if (key.expiryTime) _item_["expiryTime"] = key.expiryTime || 0;

                    _bwcache.setKeyValue(_item_, (!_item_.isSession) ? storage : window.sessionStorage);
                }
                else return _bwcache.setKeyValue(_item_, storage);
            }
            else {
                console.warn("Web Storage not supported");
                if (options.FallBack) return _bwcache.fallback.setCookie(key, value, (isSession && typeCheck(isSession, 'boolean')));
            }
        };


        //fallback methods using cookie
        _bwcache.fallback = {
            options: {
                IsSession: false,
                Scope: "",
                CookiePath: "/",
                CookieInitials: "bwc_",
                WebsiteDomain: document.domain|| ''
            },
            setOptions: function (options) {
                this.IsSession = options.IsSession || false;
                this.Scope = options.Scope || "";
            },
            setCookie: function (key,value,isSession) {
                if (typeCheck(key, 'string')) {
                    if (typeCheck(value, 'object'))
                        value = JSON.stringify(value);
                    value = value.replace(/\s+/g, '-');

                    if (typeCheck(isSession, 'undefined')) //create normal cookie with 1 day expiry
                    {
                        var expire = new Date();
                        expire.setTime(expire.getTime() + 3600000 * 24 * 1);
                        document.cookie = key + "=" + value + ";expires=" + expire.toGMTString() + ';domain=' + this.options.WebsiteDomain + '; path =/';

                    }
                    else (typeCheck(isSession, 'boolean')) //create a session cookie without expiry
                    {
                        document.cookie = key + "=" + value + ';domain=' + this.options.WebsiteDomain + '; path =/';
                    }
                }
            },
            getCookie: function (key) {
                if (typeCheck(key, 'string')) {
                    return ((document.cookie.match('(^|; )' + key + '=([^;]*)') || 0)[2] || null);
                }
                return null;
            },
            getCookiebyScope: function (key, scope) {
                var arr = [];
                if (typeCheck(key, 'string') && typeCheck(scope, 'string')) {
                    var bwc = document.cookie.split(';').map(function (x) { return x.trim().split('='); }).reduce(function (a, b) { a[b[0]] = b[1]; return a; }, {});
                    for (var i in bwc) {
                        if (i.indexOf(options.CookieInitials) > -1 && i.indexOf(scope) > -1)
                            arr[i] = bwc[i];
                    }
                }
                return arr;
            },
            removeCookie: function (key) {
                if (typeCheck(key, 'string')) {
                    document.cookie = key + '=;expires=Thu, 01 Jan 1970 00:00:01 GMT;';
                }
                return false;
            },
            removeAllCookie: function (isExpired) // check for isExpired if true then remove only expired cookie
            {
                var bwc = document.cookie.split(';').map(function (x) { return x.trim().split('='); }).reduce(function (a, b) { a[b[0]] = b[1]; return a; }, {});
                if (typeCheck(isExpired, 'boolean')) {
                    var currentTime = (new Date).getTime();
                    for (var i in bwc) {
                        if (i.indexOf(options.CookieInitials) > -1) {
                            var _item_ = JSON.parse(bwc[i]) || {};
                            var time = _item_.expiryTime || 1;
                            if (time < currentTime) removeCookie(i);

                        }
                    }
                }
                else if (typeCheck(isExpired, 'undefined')) {
                    for (var i in bwc) {
                        if (i.indexOf(options.CookieInitials) > -1)
                            removeCookie(i);
                    }
                }
            }
        };

        return _bwcache;
    }

    // to make globally accessible
    if (typeof (window.bwcache) === 'undefined') {
        window.bwcache = _bwcachelib();
        window.bwcache.init();
    } else {
        console.log("Localstorage library already exists.");
    }
})(window); //passing window variable to function 