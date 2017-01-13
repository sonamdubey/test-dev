$(document).ready(function () {
    var photosLength = $('.photos-grid-list').first().find('li').length;

    // add 'more photos count' if photo grid contains 30 images
    if (photosLength == 30) {
        var lastPhoto = $('.photos-grid-list li').eq(29),
            morePhotoCount = $('<span class="black-overlay"><span class="font14 text-white">+'+ (photoCount - 29) +'<br />photos</span></span>');

        lastPhoto.append(morePhotoCount);
    }
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
        'imagePathLarge': '/bw/ec/22012/Honda-CB-Shine-Rear-threequarter-66792.jpg',
        'imagePathThumbnail': '/bw/ec/22012/Honda-CB-Shine-Rear-threequarter-66792.jpg',
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
    TOGGLE_PHOTO_THUMBNAIL_SCREEN: 3
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

ko.components.register("gallery-component", {
    viewModel: function () {
        var self = this;

        self.photosTabActive = ko.observable(true);
        self.photoList = modelImages;

        self.screenActive = ko.observable(false);
        self.photoThumbnailScreen = ko.observable(false);
        self.activePhotoIndex = ko.observable(0);
        
        self.postAction = function (action, data) {
            ACTION_HANDLER.executeAction(action, data);
        };

        self.afterRender = function () {
            // if swiper is not defined
            if (!self.mainSwiper) {
                self.mainSwiper = $('#main-photo-swiper').swiper({
                    onTap: function (swiper) {
                        if (self.photoThumbnailScreen) {
                            self.photoThumbnailScreen(false);
                        }
                    },
                    onTouchStart: function () {
                        if (self.photoThumbnailScreen) {
                            self.photoThumbnailScreen(false);
                        }
                    }
                });
            };

            if (!self.thumbnailSwiper) {
                self.thumbnailSwiper = $('#thumbnail-photo-swiper').swiper({
                    slidesPerView: 'auto',
                    spaceBetween: 0,
                    slideActiveClass: '',
                    slideToClickedSlide: true,
                    onTap: function (swiper) {
                        focusGalleryPhoto();
                    }
                });
            }

        },

        ACTION_HANDLER.registerAction(ACTIONS.ACTIVATE_PHOTO_TAB, function () {
            self.photosTabActive(true);
        });

        ACTION_HANDLER.registerAction(ACTIONS.DEACTIVATE_PHOTO_TAB, function () {
            self.photosTabActive(false);
        });

        ACTION_HANDLER.registerAction(ACTIONS.TOGGLE_PHOTO_THUMBNAIL_SCREEN, function () {
            self.photoThumbnailScreen(!self.photoThumbnailScreen());
            self.screenActive(self.photoThumbnailScreen());
            if (self.thumbnailSwiper) {
                self.activePhotoIndex(self.mainSwiper.activeIndex);
                self.thumbnailSwiper.update(true);
                focusThumbnail();
            }
        });

        // focus gallery to clicked thumbnail index
        function focusGalleryPhoto() {
            var thumbnailIndex = self.thumbnailSwiper.clickedIndex;
            self.mainSwiper.slideTo(thumbnailIndex);
            self.photoThumbnailScreen(false);
        };

        // focus thumbnail to main photo index
        function focusThumbnail() {
            var thumbnailIndex = self.thumbnailSwiper.slides[self.activePhotoIndex()];

            $(self.thumbnailSwiper.slides).removeClass('swiper-slide-active');
            $(thumbnailIndex).addClass('swiper-slide-active');
            self.thumbnailSwiper.slideTo(self.activePhotoIndex());
        };
    },
    template: { element: "gallery-template-wrapper" }
});

ko.applyBindings({}, document.getElementById('gallery-root'));