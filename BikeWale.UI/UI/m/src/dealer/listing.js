var clientIP, pageUrl;
docReady(function () {
    clientIP = document.getElementById("changeOptions").getAttribute("data-clientip");
    pageUrl=window.location.href;
    $(".leadcapturebtn").click(function (e) {
        ele = $(this);
        var leadOptions = {
            "dealerid": ele.attr('data-item-id'),
            "dealername": ele.attr('data-item-name'),
            "dealerarea": ele.attr('data-item-area'),
            "campid": ele.attr('data-campid'),
            "leadsourceid": ele.attr('data-leadsourceid'),
            "pqsourceid": ele.attr('data-pqsourceid'),
            "isdealerbikes": true,
            "pageurl": window.location.href,
            "isregisterpq": true,
            "clientip": clientIP,
            "dealercityname": ele.attr('data-cityname'),
            "dealerareaname": ele.attr('data-areaname'),
            "eventcategory" : ele.attr('data-eventcategory')
        };
        dleadvm.setOptions(leadOptions);
    });
    initializeCityMap();
    function initializeCityMap() {
        $(".map_canvas").each(function (index) {
            var lat = $(this).attr("data-lat");
            var lng = $(this).attr("data-long");
            var latlng = new google.maps.LatLng(lat, lng);

            var myOptions = {
                scrollwheel: false,
                zoom: 10,
                center: latlng,
                mapTypeId: google.maps.MapTypeId.ROADMAP
            };
            var map = new google.maps.Map($(".map_canvas")[index], myOptions);
        });
    }
    
});