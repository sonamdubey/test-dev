
var stopBanner = function  (event) {   
    var bannerId = event.currentTarget.getAttribute("data-bannerId")
    $.ajax({
        type: "POST",
        url: "/api/banner/stop/" + bannerId + '/',
        contentType: "application/json",       
        success: function (response) {
            $("#btnAbort_" + bannerId).closest("tr").fadeOut();
            Materialize.toast('Banner aborted', 4000);
        }
    });
};

var filterData = function (event) {
    var status = $("input[name='group1']:checked").val();

    if (status)
        window.location.href = "/banner/BannersList/?bannerStatus=" + status;
};