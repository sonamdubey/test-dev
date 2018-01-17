docReady(function () {
    try {
        var photosLength = $('.photos-grid-list').first().find('li').length,
           photosLimit = 30,
           lastPhotoIndex = photosLimit - 1;

        // add 'more photos count' if photo grid contains 30 images
        if (photosLength == photosLimit) {
            var lastPhoto = $('.photos-grid-list li').eq(lastPhotoIndex),
                morePhotoCount = $('<span class="black-overlay"><span class="font14 text-white">+' + (photoCount - lastPhotoIndex) + '<br />images</span></span>');
            lastPhoto.append(morePhotoCount);
        }

    } catch (e) {
        console.log(e.message);
    }
    if (popupGallery) {

        try {
            if (returnUrl && returnUrl.length > 0) {
                popupGallery.bindGallery(imageIndex);
                if (typeof (logBhrighuForImage) != "undefined" && imageIndex <= 0) {
                    logBhrighuForImage($('#main-photo-swiper .swiper-slide-active'));
                }
            }


            $('.photos-grid-list').on('click', 'li', function () {
                if (photoCount > 1) {
                    galleryRoot.find('.gallery-loader-placeholder').show();

                    if (typeof (logBhrighuForImage) != "undefined" && imageIndex <= 0)
                    {
                        //included in gallery js
                        logBhrighuForImage($(this));
                    }                    

                    var imageIndex = $(this).index(),
                        parentGridType = $(this).closest('.photos-grid-list');

                    if (parentGridType.hasClass('remainder-grid-list')) {
                        var gridOneLength = $('.photos-grid-list').first().find('li').length;

                        imageIndex = gridOneLength + imageIndex; // (grid type 1's length + grid type remainder's index)
                    }

                    popupGallery.bindGallery(imageIndex);
                    galleryRoot.find('.gallery-loader-placeholder').hide();
                }
            });

            $('.photos-grid-list img.lazy').on('load', function () {
                resizePortraitImage($(this));
            });
        } catch (e) {
            console.warn(e.message);
        }
    }
});