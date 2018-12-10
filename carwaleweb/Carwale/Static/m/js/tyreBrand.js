var tyresListPage = {
    tyresListNextPageNo: 1,
    pageSize: 10,
    makeId: brandId,
    makeName: "",
    tyreListSelector: $('#tyresList ul'),
    lodingPopupSelector: $('.m-loading-popup'),
    processedTyres: [],
    registerEvents: function () {
        tyresListPage.lodingPopupSelector.hide();
        window.addEventListener('scroll', tyresListPage.scrollHandler, false);
    },
    bindTyresData: function () {
        tyresListPage.tyresListNextPageNo += 1;
        var url = '/tyresList/make/?platformId=' + 43 + '&pageno=' + tyresListPage.tyresListNextPageNo + '&pagesize=' + tyresListPage.pageSize + '&brandId=' + tyresListPage.makeId;

        $.when(Common.utils.ajaxCall(url)).done(function (data) {

            tyresListPage.lodingPopupSelector.hide();
            if (data != null && $.trim(data) != "") {
                tyresListPage.tyreListSelector.find('li:last').after(data);
                $('img.lazy').lazyload();
            }
        });
    },
    scrollHandler: function () {
        var visibleNextPageTrigger = $(".next-page-trigger:in-viewport");
        if (visibleNextPageTrigger.length > 0) {
            var visibleId = visibleNextPageTrigger[0].getAttribute('id');
            if (tyresListPage.processedTyres.indexOf(visibleId) < 0 && tyresListPage.tyreListSelector.find('li').length < $("#hdnTyreCount").val()) {
                tyresListPage.processedTyres.push(visibleId);
                tyresListPage.lodingPopupSelector.show();
                tyresListPage.bindTyresData();
            }
        }
    },
}
$(document).ready(function () {
    tyresListPage.registerEvents();
});