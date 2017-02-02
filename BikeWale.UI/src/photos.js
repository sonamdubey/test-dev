$(document).ready(function () {
    var photosLength = $('.model-grid-images li').length,
        photoLimitCount = 24; // ignore 'more photos' image item

    // add 'more photos count' if photo grid contains 24 images
    if (photosLength == photoLimitCount) {
        var lastPhoto = $('.model-grid-images li').eq(photoLimitCount - 1),
            morePhotoCount = $('<span class="black-overlay"><span class="font18 text-bold">+' + (photoCount - photoLimitCount) + '</span><br /><span class="font16">photos</span></span>');

        lastPhoto.append(morePhotoCount);
    }

});