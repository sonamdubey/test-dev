var apiRequest = function () {
    var get = function (options) {
        var xhttp = new XMLHttpRequest();
        if (typeof (options.callback) != "undefined") {
            xhttp.onreadystatechange = function () {
                if (this.readyState == 4) {
                    if (this.status == 200) {
                        options.callback(this.response, this.status);
                    }
                    else {
                        options.callback(this.response, this.status);
                        console.log("API request failed:", this);
                    }   
                }
            };
        }
        xhttp.timeout = 2000;
        xhttp.open("GET", options.url, true);
        xhttp.send();
    }
    return {
        get: get
    }
}();