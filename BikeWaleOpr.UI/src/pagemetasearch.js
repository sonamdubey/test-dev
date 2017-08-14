var preSelectFilter = function () {
    var queryString = location.search.substring(1);
    if (queryString) {
        var keyValues = queryString.split('&');
        var key = keyValues[0].split('=');
        if (key[0] == 'bannerStatus')
            $("#chkFilter" + key[1]).prop("checked", true);
    }
    else
        $("#chkFilter1").prop("checked", true);
}();