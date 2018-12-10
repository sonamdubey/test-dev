var StartIndex = 0;
var pageLoad = true;

$(document).ready(function () {
    // Initial setting when the page loads
    var LiIndex = 0;
    $('#galleryList li').each(function () {
        if ($(this).children('a').attr('href').indexOf(ImageName) > 0)
            StartIndex = LiIndex;
        LiIndex++;
    });

    var galleries = $('.pg-gallery').adGallery({
        start_at_index: StartIndex,
        loader_image: "https://imgd.aeplcdn.com/0x0/adgallery/loader.gif",
        callbacks: { // CallBack function after the image is loaded and is visible
            afterImageVisible: function () {
                $('#artTitle').html(this.images[this.current_index].artTitle);
                $('#artTitle').attr('href', this.images[this.current_index].artUrl);
                var gallery_info = this.gallery_info.html();
                if (this.images[this.current_index].imgCnt == 0) {
                    this.gallery_info.hide();
                    $('div.pg-forward').add('div.pg-back').add('div.pg-next').add('div.pg-prev').hide();
                }
                //pageTracker._trackPageview("/modelphotos/" + this.images[this.current_index].image);
                var downloadImagePath = $('#galleryHolder').find('img').attr('src').replace('600x337', '0x0');
                $('#btnDownload').attr('href', downloadImagePath.substring(0, downloadImagePath.indexOf('?')) + '?wm=1');
                if (!pageLoad) {
                    window.history.replaceState(null, null, $('#galleryList a.pg-active').attr('href'));
                    Common.utils.firePageView(window.location.pathname);
                }
                pageLoad = false;
                fireComscorePageView();
            }
        }
    });
    InitGallery(galleries); // Initialize the gallery
    LoadGallery(MakeId, ModelId); // Load the Similar and other models Gallery
    //LazyLoad();

    // Information about an album for the Similar and Other models gallery
    $("div.img-placer").live('mouseenter', function () {
        $(this).find('div.rollover-container').stop(true, true).slideDown();
    }).live('mouseleave', function () {
        $(this).find('div.rollover-container').stop(true, true).slideUp();
    });
});

function LazyLoad() {
    $("img.lazy").lazyload({
        effect: "fadeIn",
        container: $("div.pg-thumbs")
    });
}

// Get the values required to load the Similar and Other Models Gallery
function LoadGallery(makeId, modelId) {
    var mId = makeId;
    var moId = modelId;
    var makehashParams = "mId=" + mId;
    var modelhashParams = "moId=" + moId;
}

// Gallery Initialization function.
function InitGallery(galleries) {
    galleries[0].settings.description_wrapper = $('#descriptions');
    galleries[0].slideshow.disable();
    // Add custom Effects to the Gallery
    galleries[0].addAnimation('fadeNew', function (img_container, direction, desc) {
        img_container.css('opacity', 0);
        if (desc) {
            desc.animate({ opacity: 1 }, 5);
        };
        if (this.current_description) {
            this.current_description.animate({ opacity: 0 }, 5);
        };
        return {
            old_image: { opacity: 0 },
            new_image: { opacity: 1 }
        };
    });
    galleries[0].settings.effect = "fadeNew";
    //alert(galleries[0].settings.start_at_index);
}