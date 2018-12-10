var buyingIndex = (function () {
    var queryParam = {};
    var url;

    if (typeof events !== 'undefined') {
        events.subscribe("versionChanged", addQueryParam);
        events.subscribe("yearChanged", addQueryParam);
        events.subscribe("cityChanged", addQueryParam);
        events.subscribe("ownersChanged", addQueryParam);
        events.subscribe("kmsChanged", addQueryParam);
        events.subscribe("cardetailsViewLoaded",getBuyingIndexUrl);
    }

    function getBuyingIndexUrl() {
        url = $('#bodyExpectedPrice').data("url");
    }

    function addQueryParam(eventObj) {
        queryParam = appState.setSelectedData(queryParam, eventObj);
        fetchBuyingIndex();
    }

    function fetchBuyingIndex() {
        if(validateParam()) {
            $.when(isEligibleCity(queryParam.cityId)).done(function (response) {
                if (response) {
                    if (parseInt(queryParam['owners']) >= 4) {
                        queryParam['owners'] = "4+";
                    }
                    $.when(get().done(function (response) {
                        if (typeof events != 'undefined') {
                            events.publish('buyingIndexSuccess',response);
                        }
                    }).fail(function (response) {
                        if (typeof events != 'undefined') {
                            events.publish('buyingIndexFailed',response);
                        }
                    }));
                }
            });
        }
    }

    function get() {
        if (url) {
            return $.ajax({
                type: "GET",
                url: url,
                data: queryParam,
                async: true
            });
        }
    }

    function isEligibleCity(cityId) {
        return $.ajax({
            type: "GET",
            url: "/api/used/sell/c2bcity/?cityId=" + cityId,
            async: true
        });
    }

    function validateParam() {
        return queryParam.makeId > "0" &&
            queryParam.modelId > "0" &&
            queryParam.versionId > "0" &&
            queryParam.year > "0" &&
            queryParam.owners > "0" &&
            queryParam.cityId > "0" &&
            queryParam.kms_driven > "0";
    }
})();