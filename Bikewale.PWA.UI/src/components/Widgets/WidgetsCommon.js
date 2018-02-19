function moreImagesUrl() {
    return "/m/images/";
}

function createImageUrl(hostUrl, originalImagePath, resolution) {
    return (!hostUrl || !originalImagePath) ? 'https://imgd.aeplcdn.com/160x89/bikewaleimg/images/noimage.png?q=70':hostUrl + resolution + originalImagePath;
}

module.exports = {
    moreImagesUrl,
    createImageUrl
}