// Common CW tabs code
$(document).on('click', ".cw-tabs li", function () {
    if (!$(this).hasClass("disabled")) {
        var panel = $(this).closest(".cw-tabs-panel");
        panel.find(".cw-tabs li").removeClass("active");
        $(this).addClass("active");
        var panelId = $(this).attr("data-tabs");
        panel.find(".cw-tabs-data").hide();
        $("#" + panelId).show();
    }
}); // ends
// Common CW select box tabs code
$(".cw-tabs select").change(function () {
    var panel = $(this).closest(".cw-tabs-panel");
    var panelId = $(this).val();
    panel.find(".cw-tabs-data").hide();
    //Pause YouTube video on select change
    if (Swiper.YouTubeApi.playerState == 'playing' || Swiper.YouTubeApi.playerState == 'buffering') {
        try {
            Swiper.YouTubeApi.videoPause();
        } catch (e) {
            console.log(e);
        }
    }
    $('#' + panelId).show();

    var swiperContainer = $('#' + panelId).find(".swiper-container");
    if (swiperContainer.length > 0) {
        var sIndex = swiperContainer.attr('class');
        var regEx = /sw-([0-9]+)/i;
        try {
            var index = regEx.exec(sIndex)[1];
            $('.sw-' + index).data('swiper').update(true);
        } catch (e) { console.log(e) }
    }

}); // ends