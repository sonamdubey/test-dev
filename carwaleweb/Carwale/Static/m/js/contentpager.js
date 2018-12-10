$(function () {
    if (typeof (currentPageNo) != "undefined")
        $("#ddlPages").val(currentPageNo);
});

if (typeof (DfpAdslot) != "undefined" && DfpAdslot == "Car News")
    insertAd();

function pageChanged(pageNo) {
    var pageUrl = location.href.split('page')[0];
    if (pageNo == -1)
        location.href = pageUrl + "page/" + $("#ddlPages").val() + "";
    else
        location.href = pageUrl + "page/" + pageNo + "";
}

function insertAd() {
    var ad1 = "<div class=\"no-padding border-none\" style='width:100%;'><div style='text-align: center;' class=\"adunit sponsored\" data-adunit=\"CarWale_Mobile_News_340x89\" data-dimensions=\"340x89\"></div></div>";
    $("div.content-box-shadow").eq(3).after(ad1);

    $.dfp({
        dfpID: '1017752',
        enableSingleRequest: false
    });
};

