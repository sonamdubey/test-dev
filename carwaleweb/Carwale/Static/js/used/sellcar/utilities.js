// This file will contain utilities that are common for both msite and desktop site

var cardetailsUtil = (function () {
    function removeModelYear(arrModelResp) {
        if (arrModelResp && arrModelResp.length) {
            arrModelResp.sort(function (a, b) {
                return a.text.split('[')[0].trim().toLowerCase() < b.text.split('[')[0].trim().toLowerCase() ? -1
                    : (a.text.split('[')[0].trim().toLowerCase() > b.text.split('[')[0].trim().toLowerCase() ? 1 : 0)
            });
            var length = arrModelResp.length;
            for (var i = 0; i < length; i++) {
                if (i === length - 1) {
                    arrModelResp[i].text = arrModelResp[i].text.split('[')[0].trim();
                    break;
                }
                var first = arrModelResp[i].text.split('[')[0].trim();
                var second = arrModelResp[i + 1].text.split('[')[0].trim();
                if (first.toLowerCase() === second.toLowerCase()) {
                    i++;
                }
                else {
                    arrModelResp[i].text = first;
                }
            }
            return arrModelResp;
        }
    };

    var numberToWords = (function () {
        var unitsPlace = ["", "one ", "two ", "three ", "four ", "five ", "six ", "seven ", "eight ", "nine ", "ten ", "eleven ", "twelve ", "thirteen ",
                          "fourteen ", "fifteen ", "sixteen ", "seventeen ", "eighteen ", "nineteen "];
        var tensPlace = ["", "", "twenty ", "thirty ", "forty ", "fifty ", "sixty ", "seventy ", "eighty ", "ninety "];
        var convert = function (n) {
            if (n < 0) return;
            if (n < 20) {
                return unitsPlace[n];
            }
            else if (n < 100) {
                return (tensPlace[Math.floor(n / 10)] + unitsPlace[n % 10]);
            }
            else if (n < 1000) {
                return unitsPlace[Math.floor(n / 100)] + "hundred " + (n % 100 > 0 ? "and " + convert(n % 100) : "");
            }
            else if (n < 100000) {
                return convert(Math.floor(n / 1000)) + "thousand " + convert(n % 1000);
            }
            else if (n < 10000000) {
                return convert(Math.floor(n / 100000)) + "lakh " + convert(n % 100000);
            }
            else if (n < 1000000000) {
                return convert(Math.floor(n / 10000000)) + "crore " + convert(n % 10000000);
            }
            return "";
        };
        return convert;
    })();

    function capitalizeFirstLetter(s) {
        if (s) {
            var str = s.toString();
            return str.charAt(0).toUpperCase() + str.slice(1);
        }
    }
    
    return {
        removeModelYear: removeModelYear,
        numberToWords: numberToWords,
        capitalizeFirstLetter: capitalizeFirstLetter
    

    }
})();

var geoLocation = (function () {
    var city = { name: "", id: "" };
    
    function isSupported() {
        return ('geolocation' in navigator) && navigator.geolocation != 'undefined';
    }

    function validateRequest(geoOptions) {
        return (typeof geoOptions == 'undefined' || typeof geoOptions == 'object');
    }

    function getCurrentCity(geoOptions) {
        return new Promise(function (resolve, reject) {
            if (!isSupported() || !validateRequest(geoOptions)) {
                reject("geolocation is not supported or invalid parmeter geoOptions[it should be object]");
            }

            var successHandler = function (position) {
                var url = "/webapi/geocity/getcityfromlatlong/?latitude=" + position.coords.latitude + "&longitude=" + position.coords.longitude;
                try {
                    $.get(url).done(function (response) {
                        city.id = response.cityId;
                        city.name = response.cityName;
                        resolve(city);
                    }).fail(function (xhr) {
                        reject(xhr.responseText);
                    });
                } catch (err) {
                    reject(err);
                }
            };

            var errorHandler = function (error) {
                var errorMessage;
                switch (error.code) {
                    case error.PERMISSION_DENIED:
                        errorMessage = "User denied the request for Geolocation."
                        break;
                    case error.POSITION_UNAVAILABLE:
                        errorMessage = "Location information is unavailable."
                        break;
                    case error.TIMEOUT:
                        errorMessage = "The request to get user location timed out."
                        break;
                    case error.UNKNOWN_ERROR:
                        errorMessage = "An unknown error occurred."
                        break;
                    default:
                        errorMessage = "";
                }
                reject(errorMessage);
            };
            navigator.geolocation.getCurrentPosition(successHandler, errorHandler, geoOptions);
        });
    }
    return { getCurrentCity: getCurrentCity }
})();