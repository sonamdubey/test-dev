$(document).ready(function () {
    var photosLength = $('.photos-grid-list').first().find('li').length;

    // add 'more photos count' if photo grid contains 30 images
    if (photosLength == 30) {
        var lastPhoto = $('.photos-grid-list li').eq(29),
            morePhotoCount = $('<span class="black-overlay"><span class="font14 text-white">+'+ (photoCount - 29) +'<br />photos</span></span>');

        lastPhoto.append(morePhotoCount);
    }
});

var gallery = function() {
    var self = this;

    self.galleryHeader = ko.observable(new galleryHeader);
    self.galleryFooter = ko.observable(new galleryFooter);
};

var galleryHeader = function () {
    var self = this;

    self.photosTabActive = ko.observable(true);

    self.activateTab = function () {
        self.photosTabActive(!self.photosTabActive());
    };

};

var galleryFooter = function () {
    var self = this;

    self.screenActive = ko.observable(false);

    self.shareScreen = ko.observable(false);
    self.modelScreen = ko.observable(false);

    self.activateShareScreen = function () {
        // reset other screens
        self.modelScreen(false);

        // toggle screen
        self.shareScreen(!self.shareScreen());
        self.screenActive(self.shareScreen());

        // append state to maintain history
        appendState('shareScreen');
    };

    self.activateModelScreen = function () {
        self.shareScreen(false);

        self.modelScreen(!self.modelScreen());
        self.screenActive(self.modelScreen());

        appendState('modelScreen');
    };
};

var vmGallery = new gallery();

ko.applyBindings(vmGallery, document.getElementById('gallery-container'));

$('.photos-grid-list').on('click', 'li', function () {

    galleryPopup.open();
    window.dispatchEvent(new Event('resize'));
    appendState('galleryPopup');

});

$(".gallery-close-btn").on('click', function () {
    galleryPopup.close();
    history.back();
});

var appendState = function (state) {
    window.history.pushState(state, '', '');
};

var galleryContainer = $('#gallery-container'),
    shareTabScreen = $('#share-tab-screen'),
    modelInfoTabScreen = $('#info-tab-screen');

$(window).on('popstate', function (event) {
    if (galleryContainer.is(':visible')) {
        if (!shareTabScreen.is(':visible') && !modelInfoTabScreen.is(':visible')) {
            galleryPopup.close();
        }
        else {
            if (shareTabScreen.is(':visible')) {
                vmGallery.galleryFooter().shareScreen(false);
            }

            if (modelInfoTabScreen.is(':visible')) {
                vmGallery.galleryFooter().modelScreen(false);
            }
        }
    }
});

var galleryPopup = {
    open: function () {
        lockPopup();
        galleryContainer.show();
        $('body').addClass('gallery-popup-active');
    },

    close: function () {
        unlockPopup();
        galleryContainer.hide();
        $('body').removeClass('gallery-popup-active');
    }
};