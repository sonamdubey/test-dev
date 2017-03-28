var bikeName;
docReady(function () {    
    var $dvPgVar = $("#dvPgVar");
    var cityArea = $dvPgVar.data("cityarea");
    bikeName = $dvPgVar.data("bikename");
    var modelId = $dvPgVar.data("modelid");
    $("#btnDealerPricePopup").click(function () {
        var selArea = '';
        if ($('#ddlAreaPopup option:selected').index() > 0) {
            selArea = '_' + $('#ddlAreaPopup option:selected').text();
        }
        triggerGA('Price_in_City_Page', 'Show_On_Road_Price_Clicked', bikeName + "_" + $('#versions .active').text() + '_' + $('#ddlCitiesPopup option:selected').text() + selArea);

    });

    $(".leadcapturebtn").click(function (e) {
        ele = $(this);
        var leadOptions = {
            "dealerid": ele.attr('data-item-id'),
            "dealername": ele.attr('data-item-name'),
            "dealerarea": ele.attr('data-item-area'),
            "versionid": $("#versions a.active").attr("id"),
            "leadsourceid": ele.attr('data-leadsourceid'),
            "pqsourceid": ele.attr('data-pqsourceid'),
            "pageurl": window.location.href,
            "clientip": '',
            "isregisterpq": true,
            "gaobject": {
                cat: 'Price_in_City_Page',
                act: 'Lead_Submitted',
                lab: bikeName + "_" + cityArea
            }
        };
        dleadvm.setOptions(leadOptions);
    });
    $("#dealerDetails").click(function (e) {
        ele = $(this);
        vmquotation.CheckCookies();
        vmquotation.IsLoading(true);
        $('#priceQuoteWidget,#popupContent,.blackOut-window').show();
        var options = {
            "modelId": modelId,
            "cityId": onCookieObj.PQCitySelectedId,
            "areaId": onCookieObj.PQAreaSelectedId,
            "city": (onCookieObj.PQCitySelectedId > 0) ? { 'id': onCookieObj.PQCitySelectedId, 'name': onCookieObj.PQCitySelectedName.replace(/-/g, ' ') } : null,
            "area": (onCookieObj.PQAreaSelectedId > 0) ? { 'id': onCookieObj.PQAreaSelectedId, 'name': onCookieObj.PQAreaSelectedName.replace(/-/g, ' ') } : null,
        };
        vmquotation.IsOnRoadPriceClicked(false);
        vmquotation.setOptions(options);
    });
    $('.model-versions-tabs-wrapper a').on('click', function () {
        var verid = $(this).attr('id');
        showTab(verid);
    });
});
function showTab(version) {
    $('.model-versions-tabs-wrapper a').removeClass('active');
    $('.model-versions-tabs-wrapper a[id="' + version + '"]').addClass('active');
    $('.priceTable').hide();
    $('.priceTable[id="' + version + '"]').show();
}
function getBikeVersionName() {
    var bikeVersion = $('#versions .active').text();
    var bikeNameVersion = bikeName + '_' + bikeVersion;
    return bikeNameVersion;
}