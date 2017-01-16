$(document).ready(function () {
    var photosLength = $('.photos-grid-list').first().find('li').length;

    // add 'more photos count' if photo grid contains 30 images
    if (photosLength == 30) {
        var lastPhoto = $('.photos-grid-list li').eq(29),
            morePhotoCount = $('<span class="black-overlay"><span class="font14 text-white">+'+ (photoCount - 29) +'<br />photos</span></span>');

        lastPhoto.append(morePhotoCount);
    }
});

$('.photos-grid-list').on('click', 'li', function () {
    var clickedImg = $(this),
        imgIndex = clickedImg.index(),
        parentGridType = clickedImg.closest('.photos-grid-list');

    if (parentGridType.hasClass('remainder-grid-list')) {
        var gridOneLength = $('.photos-grid-list').first().find('li').length;

        imgIndex = gridOneLength + imgIndex; // (grid type 1's length + grid type remainder's index)
    }

    vmPhotosPage.imageIndex(imgIndex);
    vmPhotosPage.activateGallery(true);
    appendState('modelGallery');
    $('body').addClass('lock-browser-scroll');
});

var modelImages = [
    {
        'hostUrl': 'https://imgd1.aeplcdn.com/',
        'imagePathLarge': '/bw/models/honda-cb-shine-electric-start/drum/alloy-112.jpg',
        'imagePathThumbnail': '/bw/models/honda-cb-shine-electric-start/drum/alloy-112.jpg',
        'imageTitle': 'Model image 1'
    },
    {
        'hostUrl': 'https://imgd2.aeplcdn.com/',
        'imagePathLarge': '/bikewaleimg/ec/15504/img/l/Bajaj-Pulsar-RS200-Front-three-quarter-50255.jpg',
        'imagePathThumbnail': '/bikewaleimg/ec/15504/img/l/Bajaj-Pulsar-RS200-Front-three-quarter-50255.jpg',
        'imageTitle': 'Rear three-quarter 2'
    },
    {
        'hostUrl': 'https://imgd3.aeplcdn.com/',
        'imagePathLarge': '/bw/ec/22012/Honda-CB-Shine-Side-66793.jpg',
        'imagePathThumbnail': '/bw/ec/22012/Honda-CB-Shine-Side-66793.jpg',
        'imageTitle': 'Side 3'
    },
    {
        'hostUrl': 'https://imgd4.aeplcdn.com/',
        'imagePathLarge': '/bw/ec/22012/Honda-CB-Shine-Front-threequarter-66794.jpg',
        'imagePathThumbnail': '/bw/ec/22012/Honda-CB-Shine-Front-threequarter-66794.jpg',
        'imageTitle': 'Rear three-quarter 4'
    },
    {
        'hostUrl': 'https://imgd5.aeplcdn.com/',
        'imagePathLarge': '/bw/ec/22012/Honda-CB-Shine-Front-66796.jpg',
        'imagePathThumbnail': '/bw/ec/22012/Honda-CB-Shine-Front-66796.jpg',
        'imageTitle': 'Front 5'
    },
    {
        'hostUrl': 'https://imgd6.aeplcdn.com/',
        'imagePathLarge': '/bw/ec/22012/Honda-CB-Shine-Rear-threequarter-66799.jpg',
        'imagePathThumbnail': '/bw/ec/22012/Honda-CB-Shine-Rear-threequarter-66799.jpg',
        'imageTitle': 'Rear three-quarter 6'
    }
];

window.ACTIONS = {
    ACTIVATE_PHOTO_TAB: 1,
    DEACTIVATE_PHOTO_TAB: 2,
    TOGGLE_PHOTO_THUMBNAIL_SCREEN: 3,
    TOGGLE_MODEL_INFO_SCREEN: 4,
    HIDE_GALLERY_TABS: 5,
    SHOW_GALLERY_TABS: 6,
    TOGGLE_GALLERY_TABS: 7,
    HIDE_GALLERY_FOOTER: 8,
    SHOW_GALLERY_FOOTER: 9,
    TOGGLE_GALLERY_FOOTER: 10,
    TOGGLE_COLORS_THUMBNAIL_SCREEN: 11,
    HIDE_GALLERY: 12,
    SHOW_PHOTO_HEADING: 13,
    HIDE_PHOTO_HEADING: 14,
    TOGGLE_PHOTO_HEADING: 15
}

var ACTION_HANDLER = (function () {
    var self = {};

    self.actions = {};

    // action: action's assigned number from ACTIONS window variable
    // data: data to pass while invoking the function
    function executeAction(action, data) {
        if (self.actions[action]) {
            self.actions[action](data);
        }
    }

    // action: action number from ACTIONS window variable
    // handler: function to register on action number
    function registerAction(action, handler) {
        self.actions[action] = handler;
    };

    return {
        executeAction: executeAction,
        registerAction: registerAction
    }

})();

function toggleFullScreen(goFullScreen) {
    var doc = window.document;
    var docElement = doc.documentElement;

    var requestFullScreen = docElement.requestFullscreen || docElement.mozRequestFullScreen || docElement.webkitRequestFullScreen || docElement.msRequestFullscreen;
    var cancelFullScreen = doc.exitFullscreen || doc.mozCancelFullScreen || doc.webkitExitFullscreen || doc.msExitFullscreen;

    if (goFullScreen && requestFullScreen != undefined) {
        requestFullScreen.call(docElement);
    }
    else if (cancelFullScreen != undefined) {
        cancelFullScreen.call(doc);
    }
}

function resizeHandler() {
    if (window.innerWidth > window.innerHeight) {
        ACTION_HANDLER.executeAction(ACTIONS.HIDE_GALLERY_TABS);
        ACTION_HANDLER.executeAction(ACTIONS.HIDE_GALLERY_FOOTER);
        ACTION_HANDLER.executeAction(ACTIONS.HIDE_PHOTO_HEADING);
    }
    else {
        ACTION_HANDLER.executeAction(ACTIONS.SHOW_GALLERY_TABS);
        ACTION_HANDLER.executeAction(ACTIONS.SHOW_GALLERY_FOOTER);
        ACTION_HANDLER.executeAction(ACTIONS.SHOW_PHOTO_HEADING);
    }
};

function hideGallery() {
    vmPhotosPage.activateGallery(false);
    toggleFullScreen(false);
    $('body').removeClass('lock-browser-scroll');
    // remove the binding and then re-apply
    ko.cleanNode(document.getElementById('gallery-root'))
    ko.applyBindings(vmPhotosPage, document.getElementById('gallery-root'))
};

var photosPage = function () {
    var self = this;

    self.activateGallery = ko.observable(true);
    self.imageIndex = ko.observable(0);
};

var vmPhotosPage = new photosPage();

var modelGallery = function () {
    var self = this;

    self.photosTabActive = ko.observable(true);

    self.galleryTabsActive = ko.observable(true);
    self.galleryFooterActive = ko.observable(true);
    self.photoHeadingActive = ko.observable(true);

    self.screenActive = ko.observable(false);
    self.photoThumbnailScreen = ko.observable(false);
    self.colorsThumbnailScreen = ko.observable(false);
    self.modelInfoScreen = ko.observable(false);

    self.activePhotoTitle = ko.observable('');
    self.activePhotoIndex = ko.observable(0);
        
    self.postAction = function (action, data) {
        ACTION_HANDLER.executeAction(action, data);
    };

    // set images
    self.photoList = modelImages;

    self.afterRender = function () {
        // if swiper is not defined
        if (!self.mainSwiper) {
            self.mainSwiper = $('#main-photo-swiper').swiper({
                spaceBetween: 5,
                pagination: '.swiper-pagination',
                paginationType: 'fraction',
                onInit: function (swiper) {
                    //toggleFullScreen(true);
                    setPhotoDetails(swiper);
                },
                onClick: function () {
                    if (window.innerWidth > window.innerHeight) {
                        ACTION_HANDLER.executeAction(ACTIONS.TOGGLE_GALLERY_FOOTER);
                        ACTION_HANDLER.executeAction(ACTIONS.TOGGLE_PHOTO_HEADING);
                    }
                    if (self.screenActive()) {
                        deactivateAllScreens();
                    }
                },
                onTouchStart: function () {
                    if (self.screenActive()) {
                        deactivateAllScreens();
                    }
                },
                onSlideChangeStart: function (swiper) {
                    setPhotoDetails(swiper);
                }
            });
        };

        if (!self.thumbnailSwiper) {
            self.thumbnailSwiper = $('#thumbnail-photo-swiper').swiper({
                slidesPerView: 'auto',
                spaceBetween: 0,
                slideActiveClass: '',
                slideToClickedSlide: true,
                onClick: function (swiper) {
                    focusGalleryPhoto();
                }
            });
        }

        if (!self.colorsThumbnailSwiper) {
            self.colorsThumbnailSwiper = $('#thumbnail-colors-swiper').swiper({
                slidesPerView: 'auto',
                spaceBetween: 0
            });
        }

        // set gallery image to clicked image index from listing
        self.mainSwiper.slideTo(vmPhotosPage.imageIndex());
    },

    ACTION_HANDLER.registerAction(ACTIONS.ACTIVATE_PHOTO_TAB, function () {
        self.photosTabActive(true);
    });

    ACTION_HANDLER.registerAction(ACTIONS.DEACTIVATE_PHOTO_TAB, function () {
        self.photosTabActive(false);
        if (self.screenActive()) {
            deactivateAllScreens();
        }
    });

    ACTION_HANDLER.registerAction(ACTIONS.HIDE_GALLERY_TABS, function () {
        self.galleryTabsActive(false);
    });

    ACTION_HANDLER.registerAction(ACTIONS.SHOW_GALLERY_TABS, function () {
        self.galleryTabsActive(true);
    });

    ACTION_HANDLER.registerAction(ACTIONS.TOGGLE_GALLERY_TABS, function () {
        self.galleryTabsActive() ? self.postAction(ACTIONS.HIDE_GALLERY_TABS) : self.postAction(ACTIONS.SHOW_GALLERY_TABS);
    });

    ACTION_HANDLER.registerAction(ACTIONS.HIDE_GALLERY_FOOTER, function () {
        self.galleryFooterActive(false);
    });

    ACTION_HANDLER.registerAction(ACTIONS.SHOW_GALLERY_FOOTER, function () {
        self.galleryFooterActive(true);
    });

    ACTION_HANDLER.registerAction(ACTIONS.TOGGLE_GALLERY_FOOTER, function () {
        self.galleryFooterActive() ? self.postAction(ACTIONS.HIDE_GALLERY_FOOTER) : self.postAction(ACTIONS.SHOW_GALLERY_FOOTER);
    });

    ACTION_HANDLER.registerAction(ACTIONS.SHOW_PHOTO_HEADING, function () {
        self.photoHeadingActive(true);
    });

    ACTION_HANDLER.registerAction(ACTIONS.HIDE_PHOTO_HEADING, function () {
        self.photoHeadingActive(false);
    });

    ACTION_HANDLER.registerAction(ACTIONS.TOGGLE_PHOTO_HEADING, function () {
        self.photoHeadingActive() ? self.postAction(ACTIONS.HIDE_PHOTO_HEADING) : self.postAction(ACTIONS.SHOW_PHOTO_HEADING);
    });

    ACTION_HANDLER.registerAction(ACTIONS.TOGGLE_PHOTO_THUMBNAIL_SCREEN, function () {
        if (!self.photoThumbnailScreen()) {
            // deactivate all other screens
            deactivateAllScreens();
            // activate current screen
            self.photoThumbnailScreen(true);
        }
        else {
            self.photoThumbnailScreen(false);
        }
            
        self.screenActive(self.photoThumbnailScreen());

        if (self.thumbnailSwiper) {
            self.thumbnailSwiper.update(true);
            focusThumbnail();
        }
    });

    ACTION_HANDLER.registerAction(ACTIONS.TOGGLE_MODEL_INFO_SCREEN, function () {
        if (!self.modelInfoScreen()) {
            deactivateAllScreens();                
            self.modelInfoScreen(true);
        }
        else {
            self.modelInfoScreen(false);
        }

        self.screenActive(self.modelInfoScreen());
    });

    ACTION_HANDLER.registerAction(ACTIONS.TOGGLE_COLORS_THUMBNAIL_SCREEN, function () {
        if (!self.colorsThumbnailScreen()) {
            deactivateAllScreens();
            self.colorsThumbnailScreen(true);
        }
        else {
            self.colorsThumbnailScreen(false);
        }

        self.screenActive(self.colorsThumbnailScreen());

        if (self.colorsThumbnailSwiper) {
            self.colorsThumbnailSwiper.update(true);
        }
    });

    ACTION_HANDLER.registerAction(ACTIONS.HIDE_GALLERY, function () {
        hideGallery();
    });

    window.addEventListener('resize', resizeHandler, false);
    resizeHandler();

    // set active image title and index
    function setPhotoDetails(swiper) {
        var activeSlide = swiper.slides[swiper.activeIndex],
            activeSlideTitle = $(activeSlide).find('img').attr('title');

        self.activePhotoIndex(swiper.activeIndex +1); // increment by 1, since swiper starts from index 0
        self.activePhotoTitle(activeSlideTitle);
    };

    // focus gallery to clicked thumbnail index
    function focusGalleryPhoto() {
        var thumbnailIndex = self.thumbnailSwiper.clickedIndex;

        self.mainSwiper.slideTo(thumbnailIndex);
        focusThumbnail();
    };

    // focus thumbnail to main photo index
    function focusThumbnail() {
        var activeIndex = self.activePhotoIndex() -1, // decrement by 1, since it was incremented by 1
            thumbnailIndex = self.thumbnailSwiper.slides[activeIndex];

        $(self.thumbnailSwiper.slides).removeClass('swiper-slide-active');
        $(thumbnailIndex).addClass('swiper-slide-active');
        self.thumbnailSwiper.slideTo(activeIndex);
    };

    function deactivateAllScreens() {
        self.photoThumbnailScreen(false);
        self.colorsThumbnailScreen(false);
        self.modelInfoScreen(false);
    };
}

ko.components.register("gallery-component", {
    viewModel: modelGallery,
    template: { element: "gallery-template-wrapper" }
});

ko.applyBindings(vmPhotosPage, document.getElementById('gallery-root'));

$(window).on('popstate', function (event) {
    if ($('#gallery-container').is(':visible')) {
        hideGallery();
    }
});