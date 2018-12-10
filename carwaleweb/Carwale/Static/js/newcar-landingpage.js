$(document).ready(function (e) {
    $('.test-gallery-nav li').click(function () {
        $('.test-gallery-nav li').removeClass("nc-gallery-active");
        $(this).addClass("nc-gallery-active");
        var tabId = $(this).attr('data-id');
        var galleryList = $("#testGalleryList");
        if (tabId == "interior") {
            $(galleryList.find("li[data-pro!='interior']")).hide();
            $(galleryList.find("li[data-pro='interior']")).show();
        }
        else if (tabId == "exterior") {
            $(galleryList.find("li[data-pro!='exterior']")).hide();
            $(galleryList.find("li[data-pro='exterior']")).show();
        }
        else if (tabId == "all") {
            $(galleryList.find("li")).show();
        }
    });
});
