var ModelGalleryViewModel = function () {
    var self = this;
    var QUALITY_FACTOR = 80;

    self.activeTitle = ko.observable('');
    self.activeIndex = ko.observable(0);
    self.downloadImageSize = '1280x720';

    self.modelName = MODEL_NAME;
    self.photoList = ko.observableArray(MODEL_IMAGES);
    self.imageListLength = MODEL_IMAGES.length;
    self.slideLimit = 20 < self.imageListLength ? 20 : self.imageListLength;
    self.limitedPhotoList = ko.observableArray(MODEL_IMAGES.slice(0, self.slideLimit));

    self.downloadUrl = ko.computed(function () {
        var indexId = self.activeIndex() - 1;
        if(indexId < 0) {
            indexId = 0;
        }
        var selectedPhoto = self.photoList()[indexId];
        return selectedPhoto.HostUrl + self.downloadImageSize + selectedPhoto.OriginalImgPath;
    }, self);

    self.renderImage = function (hostUrl, originalImagePath, imageSize) {
        if (originalImagePath && originalImagePath != null) {
            return hostUrl + imageSize + (originalImagePath.indexOf("?") > -1 ? (originalImagePath + "&q=" + QUALITY_FACTOR) : (originalImagePath + "?q=" + QUALITY_FACTOR));
        }
        else {
            return ('https://imgd.aeplcdn.com/' + imageSize + '/bikewaleimg/images/noimage.png?q=' + QUALITY_FACTOR);
        }
    }

    self.updateIndexandTitle = function (activeIndex) {
        self.activeIndex(activeIndex + 1);
        var title = self.modelName + ' ' +  self.photoList()[activeIndex].ImageCategory;
        
        self.activeTitle(title);
    }
}