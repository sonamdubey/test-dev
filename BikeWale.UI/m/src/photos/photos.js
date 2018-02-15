function morePhotosOverlay(limitCount) {
    var photosCnt = (photoCount - limitCount - 1);
    if (photosCnt && photosCnt > 0) {
        var lastPhoto = $('.photos-grid-list li').last(),
                   morePhotoCount = $('<span class="black-overlay"><span class="font14 text-white">+' + photosCnt + '<br />images</span></span>');
        lastPhoto.append(morePhotoCount);
    }
}

function bindPhotos(photosLimit) {
    var photosLength = $('.photos-grid-list').first().find('li').length;
          
             lastPhotoIndex = photosLimit - 1;

    // add 'more photos count' if photo grid contains 30 images
    if (photosLength == photosLimit) {
        morePhotosOverlay(photosLength-1);
    }
}
var vmLoadPhotos = function () {
    var self = this;
    self.Loadedphotos = ko.observableArray();
}


docReady(function () {
    var photosLimit = 30;
    bindPhotos(photosLimit);
    var vmPhotosMore = new vmLoadPhotos();
    ko.applyBindings(vmPhotosMore, $("#photoTemplateWrapper")[0]);
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
                   

                    var imageIndex = $(this).index(),
                        parentGridType = $(this).closest('.photos-grid-list');

                    if (parentGridType.hasClass('remainder-grid-list')) {
                        var gridOneLength = $('.photos-grid-list').first().find('li').length;

                        imageIndex = gridOneLength + imageIndex; // (grid type 1's length + grid type remainder's index)
                    }

                    if (typeof (logBhrighuForImage) != "undefined" && imageIndex <= 0) {
                        //included in gallery js
                        logBhrighuForImage($(this));
                    }
                    if (photosLimit == imageIndex+1) {
                        $('.photos-grid-list').find("span").remove();
                        if (vmModelGallery.photoList().length >= imageIndex + 12) {
                            vmPhotosMore.Loadedphotos(vmModelGallery.photoList().slice(imageIndex + 1, imageIndex + 13));
                            $("#grid-images-noremainder").append($("#photoTemplateWrapper ul li"));
                            photosLimit = photosLimit + 12;

                            if ($('.photos-grid-list li').length != vmModelGallery.photoList().length)
                            morePhotosOverlay($('.photos-grid-list li').length);
                        }
                        else {
                            var nonGirdIndex = (vmModelGallery.photoList().length - imageIndex) % 6;
                            vmPhotosMore.Loadedphotos(vmModelGallery.photoList().slice(imageIndex + 1, vmModelGallery.photoList().length - nonGirdIndex));
                            $("#grid-images-noremainder").append($("#photoTemplateWrapper ul li"));
                            vmPhotosMore.Loadedphotos('');
                            vmPhotosMore.Loadedphotos(vmModelGallery.photoList().slice(vmModelGallery.photoList().length - nonGirdIndex+1, vmModelGallery.photoList().length));
                            $(".remainder-grid-list").append($("#photoTemplateWrapper ul li"));
                            photosLimit = vmModelGallery.photoList().length - imageIndex+1;
                        }



                    }
                    else {
                        galleryRoot.find('.gallery-loader-placeholder').show();
                        popupGallery.bindGallery(imageIndex);
                        galleryRoot.find('.gallery-loader-placeholder').hide();
                    }
               
                }
            });

            $('.photos-grid-list img.lazy').on('load', function () {
                resizePortraitImage($(this));
            });
        } catch (e) {
            console.warn(e.message);
        }
    }

    $('.model-gallery img').on('click', function (e) {
        e.stopPropagation();
        $(this).closest('.model-gallery-section').addClass('model-gallery--fixed');
        //$('.photos-grid-list> li:first-child').trigger('click');
    });
    $(document).on('click', '.model-gallery--fixed .swiper-slide', function (e) {
        e.stopPropagation();
        $(this).closest('.model-gallery-section').removeClass('model-gallery--fixed');
    });

});