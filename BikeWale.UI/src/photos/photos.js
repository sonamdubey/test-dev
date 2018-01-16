function morePhotosOverlay(limitCount) {
    var lastPhoto = $('.model-grid-images li').last(),
        countOverlay = '<span class="black-overlay"><span class="font18 text-bold">+' + (photoCount - limitCount) + '</span><br /><span class="font16">images</span></span>';
    lastPhoto.append(countOverlay);
};

docReady(function () {


    var photosLength = $('.model-grid-images li').length,
        photoLimitCount = 24;
    try {
        // add 'more photos count' if photo grid contains 24 images
        if (photosLength == photoLimitCount) {
            morePhotosOverlay(photoLimitCount);
        }
        else if ((photoCount - 1) % 8 == 1) { // remainder 1 image
            $('.model-grid-images li').eq(11).css('float', 'left');
            morePhotosOverlay(photosLength);
        }


        var windowHeight = window.innerHeight;
        if (windowHeight < 700) {
            $('body').addClass('gallery-700');
        }
        else if (windowHeight > 700 && windowHeight < 880) {
            $('body').addClass('gallery-768');
        }
        else if (windowHeight > 890 && windowHeight < 910) {
            $('body').addClass('gallery-900');
        }
        else if (windowHeight > 920 && windowHeight < 1100) {
            $('body').addClass('gallery-1024');
        }

        if (popupGallery) {
            if (returnUrl && returnUrl.length > 0) {
                if (!isIEBrowser) {
                    popupGallery.bindGallery(imageIndex);
                }
                else {
                    fallbackGallery.open();
                }
            }

            $(document).on('click', '.model-main-image li', function () {
                try {
                    if (photoCount > 1) {
                        if (!isIEBrowser) {
                            popupGallery.bindGallery(0);
                        }
                        else {
                            fallbackGallery.open(0);
                        }
                        if (typeof (logBhrighuForImage) != "undefined") {
                            //included in gallery js
                            logBhrighuForImage($(this));
                        } 
                    }
                } catch (e) {
                    console.warn(e.message);
                }
            });

            $(document).on('click', '.model-grid-images li', function () {
                try {
                    var imageIndex = $(this).index(),
                        parentGridType = $(this).closest('.model-grid-images');

                    if (typeof (logBhrighuForImage) != "undefined") {
                        //included in gallery js
                        logBhrighuForImage($(this));
                    } 

                    if (!parentGridType.hasClass('remainder-grid-list')) {
                        imageIndex = imageIndex + 1;
                    }
                    else {
                        imageIndex = imageIndex + $('.model-grid-images').first().find('li').length + 1;
                    }

                    if (!isIEBrowser) {
                        popupGallery.bindGallery(imageIndex);
                    }
                    else {
                        fallbackGallery.open(imageIndex);
                    }
                } catch (e) {
                    console.warn(e.message);
                }

            });
        }


    } catch (e) {
        console.warn(e.message);
    }
});