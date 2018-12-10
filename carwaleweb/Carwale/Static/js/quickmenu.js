function initNavPrice() {
    var div = $("#liNavPrice");
    var location = new LocationSearch(div, {
        showCityPopup: true,
        callback: function (locationObj) {
            var carModelId = div.attr('data-modelId');
            var pageId = div.attr('data-pageId');

            var pqObj = { 'modelId': carModelId, 'location': locationObj, 'pageId': pageId };
            if (typeof VersionId !== "undefined") {
                pqObj.versionId = VersionId;
            }

            PriceBreakUp.Quotation.RedirectToPQ(pqObj);
        },
        isDirectCallback: true,
        validationFunction: function () {
            if(typeof CityId == "undefined")
            {
                return PriceBreakUp.Quotation.validLocation(location.selector());
            }
            else {
                return PriceBreakUp.Quotation.validLocation(location.selector(), CityId, CityName);
            }
        }
    });
}

$(document).ready(function () {
    initNavPrice();
});
