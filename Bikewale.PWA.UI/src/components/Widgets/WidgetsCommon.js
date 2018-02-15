function moreImagesUrl() {
    return "/m/images/";
}

function createImageUrl(hostUrl, originalImagePath) {
    return (!hostUrl || !originalImagePath) ? 'https://imgd.aeplcdn.com/160x89/bikewaleimg/images/noimage.png?q=70':hostUrl + '174x98' + originalImagePath;
}

module.exports = {
    moreImagesUrl,
    createImageUrl
}