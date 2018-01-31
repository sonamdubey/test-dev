/*
Author : Sushil Kumar Kanojia
Created On : 2nd Jan 2016
Description : WebStorage Library with cookie as a fallback.
             Maintain session and persistent based storage of information and at the same time check for the fallback option with the help of cookie (session and peristant).
*/
(function () {
    'use strict';
    //main function to define ls functions within
    function _bwcachelib() {
        var _bwcache = {}, _webStorageSupported = false,
			_options = {
			    StoragePrefix: 'bwc_',
			    StorageScope: '', //define page scope for the webstorage
			    IsLocalStorageSupported: true,   //since latest browser support local storage
			    IsSessionStorageSupported: true,
			    StorageTime: 30, //time in minutes (new Date().getTime() + 30 * 60000)
			    IsSessionStorage: false,
			    //IsSafariBrowser: navigator.userAgent.toLowerCase().indexOf('safari') != -1,
			    // handle for safari browser (/Safari/.test(navigator.userAgent) && /Apple Computer/.test(navigator.vendor))
			    FallBack: false, // change to true to enable cookie fallback
			    AllowNullSave: false, //avoids saving null values to the web storage
			    EnableEncryption: false
			},

		/*
			Function : Log the errors related to library based on exceptions id
			Parameters : errorId is a number and exception is an exception object
			Example : errorLog(string errorId,Exception exception);
		*/
			errorLog = function (errId, ex) {
			    try {
			        var err = "";
			        if (!isNaN(errId)) {
			            switch (errId) {
			                case 0:
			                    err = "Unexpected error occured";
			                    break;
			                case 1:
			                    err = "Web Storage not supported";
			                    break;
			                case 2:
			                    err = "Incorrect argument type to the method";
			                    break;
			                case 3:
			                    err = "Incorrect number of arguments to the method";
			                    break;
			                case 4:
			                    err = "Web Storage limit exceeded";
			                    break;
			                case 5:
			                    err = "Incorrect key not found";
			                    break;
			                case 6:
			                    err = "Reserved for other error";
			                    break;
			                default:
			                    err = "Somethings went wrong";
			                    break;

			            }
			            console.log(err + " : \n" + (ex ? ex.message : ''));
			        }
			        else errorLog(0);
			    }
			    catch (e) {
			        console.log("Unspecified error occurred.");
			    }
			};
        /*
			Function :  To set default options for the web storage which can extend the usage
			Parameters : settings : contains object for differnt options
			Example :   setOptions(object);
		*/
        _bwcache.setOptions = function (settings) {
            if (typeCheck(settings, 'object')) {
                _options.StorageScope = settings.StorageScope || '';
                _options.StorageTime = settings.StorageTime || 30;
                _options.FallBack = settings.FallBack || false;
                _options.EnableEncryption = settings.EnableEncryption || false;
                _options.AllowNullSave = settings.AllowNullSave || false;
            }
            else errorLog(2);
        };

        /*
			Function : Check support for local storage in browsers used to handle fall back methods
			Parameters : Type specifies local or session storage
			Example : _checkStorageSupport(window.sessionStorage/window.localStorage);
		*/
        var _checkStorageSupport = function (type) {
            try {
                var storage = window[type], x = 'tmp';
                storage.setItem(x, x);
                storage.removeItem(x);
                return true;
            } catch (e) {
                return false;
            }
        };

        /*
			Function : Init function require to set options and other related work to web storage library
					   check support and set library state
		*/
        _bwcache.init = function () {
            _options.IsSessionStorageSupported = _checkStorageSupport("sessionStorage");
            _options.IsLocalStorageSupported = _checkStorageSupport("localStorage");
            _webStorageSupported = _options.IsSessionStorageSupported || _options.IsLocalStorageSupported;
        };

        //Simple function to check type is of paased item type
        var typeCheck = function (item, type) { return (typeof (item) === type); };

        /*
			Function :  To set web storage scope at any point of time
			Parameters : scope : should be a string
			Example :   setScope(string scope);
		*/
        _bwcache.setScope = function (scope) {
            if (typeCheck(scope, 'string')) {
                _options.storageScope = scope;
            }
        };

        var Base64 = {
            _keyStr: "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=",
            encode: function (input) {
                var output = "";
                var chr1, chr2, chr3, enc1, enc2, enc3, enc4;
                var i = 0;

                input = Base64._utf8_encode(input);

                while (i < input.length) {

                    chr1 = input.charCodeAt(i++);
                    chr2 = input.charCodeAt(i++);
                    chr3 = input.charCodeAt(i++);

                    enc1 = chr1 >> 2;
                    enc2 = ((chr1 & 3) << 4) | (chr2 >> 4);
                    enc3 = ((chr2 & 15) << 2) | (chr3 >> 6);
                    enc4 = chr3 & 63;

                    if (isNaN(chr2)) {
                        enc3 = enc4 = 64;
                    } else if (isNaN(chr3)) {
                        enc4 = 64;
                    }

                    output = output + this._keyStr.charAt(enc1) + this._keyStr.charAt(enc2) + this._keyStr.charAt(enc3) + this._keyStr.charAt(enc4);

                }

                return output;
            },
            decode: function (input) {
                var output = "";
                var chr1, chr2, chr3;
                var enc1, enc2, enc3, enc4;
                var i = 0;

                input = input.replace(/[^A-Za-z0-9\+\/\=]/g, "");

                while (i < input.length) {

                    enc1 = this._keyStr.indexOf(input.charAt(i++));
                    enc2 = this._keyStr.indexOf(input.charAt(i++));
                    enc3 = this._keyStr.indexOf(input.charAt(i++));
                    enc4 = this._keyStr.indexOf(input.charAt(i++));

                    chr1 = (enc1 << 2) | (enc2 >> 4);
                    chr2 = ((enc2 & 15) << 4) | (enc3 >> 2);
                    chr3 = ((enc3 & 3) << 6) | enc4;

                    output = output + String.fromCharCode(chr1);

                    if (enc3 != 64) {
                        output = output + String.fromCharCode(chr2);
                    }
                    if (enc4 != 64) {
                        output = output + String.fromCharCode(chr3);
                    }

                }

                output = Base64._utf8_decode(output);

                return output;

            },
            _utf8_encode: function (string) {
                string = string.replace(/\r\n/g, "\n");
                var utftext = "";

                for (var n = 0; n < string.length; n++) {

                    var c = string.charCodeAt(n);

                    if (c < 128) {
                        utftext += String.fromCharCode(c);
                    }
                    else if ((c > 127) && (c < 2048)) {
                        utftext += String.fromCharCode((c >> 6) | 192);
                        utftext += String.fromCharCode((c & 63) | 128);
                    }
                    else {
                        utftext += String.fromCharCode((c >> 12) | 224);
                        utftext += String.fromCharCode(((c >> 6) & 63) | 128);
                        utftext += String.fromCharCode((c & 63) | 128);
                    }

                }

                return utftext;
            },
            _utf8_decode: function (utftext) {
                var string = "";
                var i = 0;
                var c = 0, c1 = 0, c2 = 0, c3 = 0;

                while (i < utftext.length) {

                    c = utftext.charCodeAt(i);

                    if (c < 128) {
                        string += String.fromCharCode(c);
                        i++;
                    }
                    else if ((c > 191) && (c < 224)) {
                        c2 = utftext.charCodeAt(i + 1);
                        string += String.fromCharCode(((c & 31) << 6) | (c2 & 63));
                        i += 2;
                    }
                    else {
                        c2 = utftext.charCodeAt(i + 1);
                        c3 = utftext.charCodeAt(i + 2);
                        string += String.fromCharCode(((c & 15) << 12) | ((c2 & 63) << 6) | (c3 & 63));
                        i += 3;
                    }

                }

                return string;
            }

        };

        /*
			Function : To create key based on web local storage support or session storage  
			Parameters : key and scope are string inputs 
			Example : createKey(string,string);
		*/
        var createKey = function (key, scope) {
            if (typeCheck(key, 'string') && (!scope || typeCheck(scope, 'string')))
                return (_options.StoragePrefix + (scope ? scope + '_' : (_options.StorageScope !== '' ? _options.StorageScope + '_' : '')) + key);
            return key;

        };

        /*
			Function : To create SPs  and update the same in case of same id
			Parameters : Key and storage is compulsory depended
			Example : getbyKey(string,string);
		*/
        var getbyKey = function (key, storage) {
            try {
                if (typeCheck(key, 'string')) {
                    var data = storage.getItem(key);

                    if (_options.EnableEncryption && data) data = Base64.decode(data);
                    var _item_ = JSON.parse(data) || {};
                    if (Object.keys(_item_).length !== 0 && !_item_.expiryTime || _item_.expiryTime === 0 || ((new Date()).getTime() <= _item_.expiryTime))
                        return _item_.value ? _item_.value : _item_; //if exists returns value else return null
                }
                else { errorLog(2); }
            }
            catch (e) {
                errorLog(0, e);
            }
            return null;
        };

        /*
			Function : Get all web storage entites from the document.
			Parameters : Key and expiring is compulsion and storage is dynamic
			Example : getbyExpiry(object,string);
		*/



	var getbyExpiry = function (key, expiry, storage) {
            try {
                if (typeCheck(key, 'string')) {
                    var currentTime = (new Date()).getTime();
                    if (typeCheck(expiry, 'number')) currentTime -= (expiry * 60000);
                    if (typeCheck(expiry, 'boolean') || typeCheck(expiry, 'number')) {
                        var _item_  = JSON.parse(storage.getItem(key)) || {};
                        var time = _item_.expiryTime || 1;
                        //if exists returns value else return null for timeout object
                        if (time < currentTime) {
                            return Object.keys(_item_).length !== 0 ? _item_  : null;
                        }
                        else errorLog(2);
                    }
                    else return getbyKey(key, storage);
                }
                else errorLog(2);
            } catch (e) {
                errorLog(0, e);
            }
            return null;
        };


		/*
			Function : Get all web storage entites from the document.
			Parameters : Key and session is boolean for  to maintain 
			Example : get(object/string,boolean isSession(session/localStorage));
		*/
        _bwcache.get = function (key, isSession) {

            try {
                key = createKey(key);
                if (_webStorageSupported) {
                    var storage = ((isSession && typeCheck(isSession, 'boolean')) || _options.IsSessionStorage) ? window.sessionStorage : window.localStorage;
                    if (typeCheck(key, 'object')) {
                        return getbyExpiry(createKey(key.key), key.expiryTime, (!key.isSession) ? storage : window.sessionStorage);
                    }
                    else return getbyKey(key, storage);
                }
                else {
                    errorLog(1);
                    if (_options.FallBack)
                        return _fallback.getCookie(key);
                }
            } catch (e) {
                errorLog(0);
            }
        };

        /*
			Function : To remove web storage key and value pair
			Parameters : item and storage is compulsory
			Example : setKeyValue(object,string);
		*/
        _bwcache.getByScope = function (key, scope, isSession) {
            try {
                var arr = [];
                if (_webStorageSupported) {
                    if (typeCheck(key, 'string') && typeCheck(scope, 'string')) {
                        key = createKey(key, scope);
                        var storage = ((isSession && typeCheck(isSession, 'boolean')) || _options.IsSessionStorage) ? window.sessionStorage : window.localStorage;
                        for (var i in storage)
                            if (storage[i].indexOf(scope) > -1) arr[i] = storage[i];
                    }
                    else {
                        errorLog(2);
                    }
                }
                else {
                    errorLog(1);
                    if (_options.FallBack) arr = _fallback.getCookiebyScope(key);
                }

                return arr;
            } catch (e) {
                errorLog(0, e);
            }
            return null;
        };

        /*
			Function : To remove web storage key and value pair
			Parameters : item and storage is compulsory
			Example : setKeyValue(object,string);
		*/
        _bwcache.remove = function (key, isSession) {
            try {
                key = key.indexOf(_options.StoragePrefix) > -1 ? key : createKey(key);
                if (_webStorageSupported) {

                    if (typeCheck(key, 'string')) {
                        var storage = ((isSession && typeCheck(isSession, 'boolean')) || _options.IsSessionStorage) ? window.sessionStorage : window.localStorage;
                        storage.removeItem(key);
                        return true;
                    }
                }
                else {
                    errorLog(1);
                    if (_options.FallBack) _fallback.removeCookie(key);
                }
            } catch (e) {
                errorLog(0, e);
            }
            return false;
        };


        /*
			Function : To remove web storage key and value pair
			Parameters : item and storage is compulsory
			Example : setKeyValue(object,string);
		*/
        _bwcache.removeAll = function (isExpired) {
            try {
                if (_webStorageSupported) {
                    var storage = _options.IsSessionStorage ? window.sessionStorage : window.localStorage;

                    if (typeCheck(isExpired, 'boolean')) {
                        var currentTime = (new Date()).getTime();
                        for (var i in storage) {
                            try
                            {
                            if (i.indexOf(_options.StoragePrefix) > -1) {
                                var data = storage[i];
                                if (_options.EnableEncryption && data) data = Base64.decode(data);
                                var _item_ = JSON.parse(data) || {};

                                 var time = _item_.expiryTime || 1;

                                if (_item_.expiryTime)
                                {

                                    if (time < currentTime) storage.removeItem(i);
                                }
                                 
            
                                }
                            }
                            catch(e)
                            {
                                storage.removeItem(i);
                                continue;
                            }
                        }
                        return true;
                    }
                    else if (typeCheck(isExpired, 'undefined')) {
                        for (var k in storage) {
                            if (k.indexOf(_options.StoragePrefix) > -1)
                                storage.removeItem(k);
                        }
                        return true;
                    }

                }
                else {
                    errorLog(1);
                    if (_options.FallBack) {
                        return _fallback.removeAllCookie(isExpired);
                    }
                }
            }
            catch (e) {
                errorLog(0, e);
            }
            return false;
        };

        /*
			Function : Set web storage value in json format for retrieval of string
			Parameters : item and storage is compulsory
			Example : setKeyValue(object,string);
		*/

        
		var _setKeyValue = function (item, storage) {
		    try {
		        if (typeCheck(item, 'object')) {
		            if (typeCheck(item.expiryTime, 'number') && item.expiryTime !== 0) //set specified expiry along with data
		            {
		                item.expiryTime = (new Date()).getTime() + (item.expiryTime * 60000); //in minutes    
		            }
		            else if (typeCheck(item.expiryTime, 'boolean')) {
		                item.expiryTime = 0;
		            }
		            if (item.value != null || _options.AllowNullSave) {
		                var data = JSON.stringify(item);
		                if (_options.EnableEncryption && data) data = Base64.encode(data);
		                storage.setItem(item.key, data);
		                return true;
		            }

		        }
		        else errorLog(1);
		    } catch (e) {
		        errorLog(0, e);
		    }
		    return false;
		};

		/*
			Function : Set web storage data to webstorage based on key value pair 
					   If webstorage is not supported then cookie is implemented as a fall back option for the same
			Parameters : Key,Value are required parameters,isSession is optionall parameter
			IF only key object is provided the no need to specify
			1. set(key,value); set(key,value,boolean); set(key as object); set(key,val)
		*/
        _bwcache.set = function (key, value, isSession) {

            try {
                key = createKey(key);
                if (_webStorageSupported) {
                    var _item_ = { 'key': key, 'value': value };
                    var storage = ((isSession && typeCheck(isSession, 'boolean')) || (typeCheck(isSession, 'undefined') && typeCheck(value, 'boolean')) || _options.IsSessionStorage) ? window.sessionStorage : window.localStorage;
                    if (typeCheck(key, 'object')) {
                        _item_ = {
                            'key': createKey(key.key, key.scope ? key.scope : ''),
                            'value': key.value,
                            
                        };
                        if (key.expiryTime) _item_.expiryTime = key.expiryTime || 0;

                        return _setKeyValue(_item_, (!_item_.isSession) ? storage : window.sessionStorage);
                    }
                    else return _setKeyValue(_item_, storage);
                }
                else {
                    errorLog(1);
                    if (_options.FallBack) return _fallback.setCookie(key, value, (isSession && typeCheck(isSession, 'boolean')));
                }
            }
            catch (e) {
                errorLog(0, e);
            }
            return false
        };

        /*
			Function : Fallback methods to implement cookie based storage in case of web storage is not supported.
		*/
        var _fallback = {
            _options: {
                IsSession: false,
                Scope: "",
                CookiePath: "/",
                CookieInitials: "bwc_",
                WebsiteDomain: document.domain || ''
            },
            /*
			Function : To set default default option for cookie based web storage
			*/
            setOptions: function (options) {
                this._options.IsSession = options.IsSession || false;
                this._options.Scope = options.Scope || "";
            },
            /*
			Function : To set cookie with default expiration of one day and 
					   if session is true then create a seesion based cookie
			Parameters : key and value are compulsory,IsSession is an optional Parameters
			Example : setCookie(string,string,boolean);
			*/
            setCookie: function (key, value, isSession) {
                try {
                    if (typeCheck(key, 'string')) {
                        if (typeCheck(value, 'object'))
                            value = JSON.stringify(value);
                        value = value.replace(/\s+/g, '-');

                        if (typeCheck(isSession, 'undefined')) //create normal cookie with 1 day expiry
                        {
                            var expire = new Date();
                            expire.setTime(expire.getTime() + 3600000 * 24 * 1);
                            document.cookie = key + "=" + value + ";expires=" + expire.toGMTString() + ';domain=' + this._options.WebsiteDomain + '; path =/';
                            return true;

                        }
                        else if (typeCheck(isSession, 'boolean') && isSession) //create a session cookie without expiry
                        {
                            document.cookie = key + "=" + value + ';domain=' + this._options.WebsiteDomain + '; path =/';
                            return true;
                        }
                    }
                } catch (e) {
                    errorLog(0, e);
                }
                return false;
            },
            /*
			Function : To retrieve cookie value based on key
			Parameters : key is compulsory
			*/
            getCookie: function (key) {
                if (typeCheck(key, 'string')) {
                    return ((document.cookie.match('(^|; )' + key + '=([^;]*)') || 0)[2] || null);
                }
                return null;
            },
            /*
			Function : To retrieve cookie value based on key  and scope
			Parameters : key and scope are compulsory
			Example : getCookiebyScope(string key,string scope); 
			*/
            getCookiebyScope: function (key, scope) {
                try {
                    var arr = [];
                    if (typeCheck(key, 'string') && typeCheck(scope, 'string')) {
                        var bwc = document.cookie.split(';').map(function (x) { return x.trim().split('='); }).reduce(function (a, b) { a[b[0]] = b[1]; return a; }, {});
                        for (var i in bwc) {
                            if (i.indexOf(this._options.CookieInitials) > -1 && i.indexOf(scope) > -1)
                                arr[i] = bwc[i];
                        }
                    }
                    return arr;
                } catch (e) {
                    errorLog(0, e);
                }
                return null;
            },
            /*
			Function : To clear cookie by key name
			Parameters : key is compulsory
			*/
            removeCookie: function (key) {
                if (typeCheck(key, 'string')) {
                    document.cookie = key + '=;expires=Thu, 01 Jan 1970 00:00:01 GMT;';
                    return true;
                }
                return false;
            },
            /*
			Function : To remove all expired or non expired cookie values
			Parameters : isExpired is an optional arguments
			Example : removeAllCookie(boolean); true resembles isexpired
			*/
            removeAllCookie: function (isExpired) // check for isExpired if true then remove only expired cookie
            {
                try {

                    var bwc = document.cookie.split(';').map(function (x) { return x.trim().split('='); }).reduce(function (a, b) { a[b[0]] = b[1]; return a; }, {});
                    if (typeCheck(isExpired, 'boolean')) {
                        var currentTime = (new Date()).getTime();
                        for (var i in bwc) {
                            if (i.indexOf(this._options.CookieInitials) > -1) {
                                var _item_ = JSON.parse(bwc[i]) || {};
                                var time = _item_.expiryTime || 1;
                                if (time < currentTime) this.removeCookie(i);

                            }
                        }
                        return true;
                    }
                    else if (typeCheck(isExpired, 'undefined')) {
                        for (var j in bwc) {
                            if (j.indexOf(this._options.CookieInitials) > -1)
                                this.removeCookie(j);
                        }
                        return true;
                    }
                } catch (e) {
                    errorLog(0, e);
                }
                return false;
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
})(); //passing window variable to function 