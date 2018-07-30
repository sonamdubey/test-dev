$(document).ready(function () {
    var StartIndex = 0;
    var LiIndex = 0;
    $('#galleryList li').each(function () {
        if ($(this).children('img').attr('id') > 0)
            StartIndex = LiIndex;
        LiIndex++
    });
    var galleries = $('.ad-gallery').adGallery({
        start_at_index: StartIndex,
        loader_image: "https://imgd.aeplcdn.com/0x0/bw/static/design15/old-images/d/circleloader.gif",
        callbacks: { // CallBack function after the image is loaded and is visible
            afterImageVisible: function () {
                $('#artTitle').html(this.images[this.current_index].artTitle);
                $('#artTitle').attr('href', this.images[this.current_index].artUrl);
                var gallery_info = this.gallery_info.html();
                if (this.images[this.current_index].imgCnt == 0) {
                    this.gallery_info.hide();
                    $('div.ad-forward').add('div.ad-back').add('div.ad-next').add('div.ad-prev').hide();
                }
            }
        }
    });
});
