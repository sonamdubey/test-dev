function morePhotosOverlay(limitCount) {
    var lastPhoto = $('.model-grid-images li').last(),
        countOverlay = '<span class="black-overlay"><span class="font18 text-bold">+' + (photoCount - limitCount-1) + '</span><br /><span class="font16">images</span></span>';
    lastPhoto.append(countOverlay);
};

var vmLoadPhotos = function()
{
    var self = this;
    self.Loadedphotos = ko.observableArray();
    self.init = function () {

    };
}
function bindPhotos(photoLimitCount)
{
    var photosLength = $('.model-grid-images li').length;
       
    if (photosLength == photoLimitCount) {
        morePhotosOverlay(photoLimitCount);
    }
    else if ((photoCount - 1) % 8 == 1) { // remainder 1 image
        $('.model-grid-images li').eq(11).css('float', 'left');
        morePhotosOverlay(photosLength);
    }
}
docReady(function () {


    
   var photoLimitCount = 24;
    var vmPhotosMore = new vmLoadPhotos();
    ko.applyBindings(vmPhotosMore, $("#photoTemplateWrapper")[0]);

    try {
        // add 'more photos count' if photo grid contains 24 images
       
        bindPhotos(photoLimitCount);

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
                if (typeof (logBhrighuForImage) != "undefined" && imageIndex <= 0) {
                    if (colorIndex > 0)
                    {
                        logBhrighuForImage($('.gallery-color-type-swiper .swiper-slide-active').first());
                    }
                    else {
                        logBhrighuForImage($('.gallery-type-swiper .swiper-slide-active').first());
                    }
                    
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
                        if (typeof (logBhrighuForImage) != "undefined" ) {
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

                    if (!parentGridType.hasClass('remainder-grid-list')) {
                        imageIndex = imageIndex + 1;
                    }
                    else {
                        imageIndex = imageIndex + $('.model-grid-images').first().find('li').length + 1;
                    }
                    if (photoLimitCount == imageIndex)
                    {
                        $('.model-grid-images li').find("span").remove();
                        if (vmModelGallery.photoList().length > imageIndex + 17) {
                            vmPhotosMore.Loadedphotos(vmModelGallery.photoList().slice(imageIndex + 1, imageIndex+17));
                            $("#grid-images-noremainder").append($("#photoTemplateWrapper ul li"));
                            photoLimitCount = photoLimitCount + 16;
                            morePhotosOverlay($('.model-grid-images li').length);
                        }
                        else {
                            var nonGirdIndex = (vmModelGallery.photoList().length - imageIndex-1) % 8;
                            vmPhotosMore.Loadedphotos(vmModelGallery.photoList().slice(imageIndex+1, vmModelGallery.photoList().length - nonGirdIndex));
                            $("#grid-images-noremainder").append($("#photoTemplateWrapper ul li"));
                            vmPhotosMore.Loadedphotos('');
                            vmPhotosMore.Loadedphotos(vmModelGallery.photoList().slice(vmModelGallery.photoList().length - nonGirdIndex, vmModelGallery.photoList().length));
                            $(".remainder-grid-list").append($("#photoTemplateWrapper ul li"));
                            photoLimitCount = photoLimitCount + vmModelGallery.photoList().length - imageIndex;
                        }
                       
                      
                       
                    }
                    else {
                        if (!isIEBrowser) {
                            popupGallery.bindGallery(imageIndex);
                        }
                        else {
                            fallbackGallery.open(imageIndex);
                        }
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