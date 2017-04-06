var ga_pg_id = "16";
var pageUrl = window.location.href;
var modelId;
function showTab(version) {
    $('.model-versions-tabs-wrapper a').removeClass('active');
    $('.model-versions-tabs-wrapper a[id="' + version + '"]').addClass('active');
    $('.priceTable').hide();
    $('.priceTable[id="' + version + '"]').show();
};

docReady(function () {
    try {
        var $dvPgVar = $("#dvPgVar");
        modelId = $dvPgVar.data("modelid");
        var floatButton = $('.float-button'),
            footer = $('footer');

        var tabsLength = $('.model-versions-tabs-wrapper li').length - 1;
        if (tabsLength < 1) {
            $('.model-versions-tabs-wrapper li').css({ 'display': 'inline-block', 'width': 'auto' });
        }

        $("#dealerDetails").click(function (e) {
            try {
                ele = $(this);
                checkCookies();
                $('#priceQuoteWidget,#popupContent,.blackOut-window').show();
                $('#popupWrapper').addClass('loader-active');
                $('#popupWrapper,#popupContent').show();
                var options = {
                    "modelId": modelId,
                    "cityId": onCookieObj.PQCitySelectedId,
                    "areaId": onCookieObj.PQAreaSelectedId,
                    "city": (onCookieObj.PQCitySelectedId > 0) ? { 'id': onCookieObj.PQCitySelectedId, 'name': onCookieObj.PQCitySelectedName } : null,
                    "area": (onCookieObj.PQAreaSelectedId > 0) ? { 'id': onCookieObj.PQAreaSelectedId, 'name': onCookieObj.PQAreaSelectedName } : null,
                };
                vmquotation.setOptions(options);
            } catch (e) {
                console.warn(e.message);
            }
        });

        $(window).scroll(function () {
            try {
                if (floatButton != null) {
                    if (floatButton.offset().top < footer.offset().top - 50)
                        floatButton.addClass('float-fixed').show();
                    if (floatButton.offset().top > footer.offset().top - 50)
                        floatButton.removeClass('float-fixed').hide();
                }
            }
            catch (e) {

            }
        });

        $('.model-versions-tabs-wrapper li').on('click', function () {
            $('.model-versions-tabs-wrapper li').removeClass('active');
            $(this).addClass('active');
            showTab($(this).attr('id'));
        });

        $('.model-versions-tabs-wrapper li').first().trigger("click");

    } catch (e) {
        console.warn(e.message);
    }
});