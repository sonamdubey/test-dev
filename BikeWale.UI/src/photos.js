﻿$(document).ready(function () {
    var photosLength = $('.model-grid-images li').length,
        photoLimitCount = 24;

    // add 'more photos count' if photo grid contains 24 images
    if (photosLength == photoLimitCount) {
        morePhotosOverlay(photoLimitCount);
    }
    else if ((photoCount - 1) % 8 == 1) { // remainder 1 image
        morePhotosOverlay(photosLength);
    }

});

function morePhotosOverlay(limitCount) {
    var lastPhoto = $('.model-grid-images li').last(),
        countOverlay = '<span class="black-overlay"><span class="black-overlay-content"><span class="font18 text-bold">+' + (photoCount - limitCount) + '</span><br /><span class="font16">photos</span></span></span>';   
    lastPhoto.append(countOverlay);
};