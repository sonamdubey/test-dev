$(document).ready(function () {
    var photosLength = $('.photos-grid-list').first().find('li').length;

    // add 'more photos count' if photo grid contains 30 images
    if (photosLength == 30) {
        var lastPhoto = $('.photos-grid-list li').eq(29),
            morePhotoCount = $('<span class="black-overlay"><span class="font14 text-white">+'+ photoCount +'<br />photos</span></span>');

        lastPhoto.append(morePhotoCount);
    }
});