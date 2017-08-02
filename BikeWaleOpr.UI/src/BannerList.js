
var changeBannerStatus = function  (event, status) {   
    var bannerId = event.currentTarget.getAttribute("data-bannerId")
    $.ajax({
        type: "POST",
        url: "/api/banner/changeStatus/" + bannerId + '/' + status + '/',
        contentType: "application/json",       
        success: function (response) {
            if (status == 0) {
                $("#btnAbort_" + bannerId).closest("tr").fadeOut();
                Materialize.toast('Banner aborted', 4000);
            }
            else
                {
                $("#btnStart_" + bannerId).closest("tr").fadeOut();
                Materialize.toast('Banner started', 4000);
            }
        }
    });
};

var filterData = function (event) {
    var status = $("input[name='group1']:checked").val();

    if (status)
        window.location.href = "/banner/BannersList/?bannerStatus=" + status;
};

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