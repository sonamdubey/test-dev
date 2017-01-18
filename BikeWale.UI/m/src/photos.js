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

var modelColorImages = [
    {
        'hostUrl': 'https://imgd6.aeplcdn.com/',
        'imagePathLarge': '/bw/models/honda-cb-hornet-160r.jpg',
        'imagePathThumbnail': '/bw/models/honda-cb-hornet-160r.jpg',
        'imageTitle': 'Dual Tone Green',
        'colors': [
            'b3ca20',
            '040004'
        ]
    },
    {
        'hostUrl': 'https://imgd7.aeplcdn.com/',
        'imagePathLarge': '/bw/ec/21058/Honda-CB-Hornet-160R-Side-61749.jpg',
        'imagePathThumbnail': '/bw/ec/21058/Honda-CB-Hornet-160R-Side-61749.jpg',
        'imageTitle': 'Dual Tone Orange',
        'colors': [
            'b83419',
            '040004'
        ]
    },
    {
        'hostUrl': 'https://imgd8.aeplcdn.com/',
        'imagePathLarge': '/bw/ec/21058/Honda-CB-Hornet-160R-Side-61758.jpg',
        'imagePathThumbnail': '/bw/ec/21058/Honda-CB-Hornet-160R-Side-61758.jpg',
        'imageTitle': 'Dual Tone White',
        'colors': [
            'cdcac3',
            '040004'
        ]
    }
];

var modelVideos = [
    {
        'imagePathLarge': 'https://img.youtube.com/vi/HhOik7KWJwc/sddefault.jpg',
        'imagePathThumbnail': 'https://img.youtube.com/vi/HhOik7KWJwc/default.jpg',
        'videoPath': 'https://www.youtube.com/embed/HhOik7KWJwc?rel=0&showinfo=0',
        'videoTitle': 'Dominar 400 or the Classic 350, CBR 250R, Mahindra Mojo, KTM Duke 390, Pulsar RS 200'
    },
    {
        'imagePathLarge': 'https://img.youtube.com/vi/WubzCZFId1o/sddefault.jpg',
        'imagePathThumbnail': 'https://img.youtube.com/vi/WubzCZFId1o/default.jpg',
        'videoPath': 'https://www.youtube.com/embed/WubzCZFId1o?rel=0&showinfo=0',
        'videoTitle': 'All you need to know about the Bajaj Dominar 400 : PowerDrift'
    },
    {
        'imagePathLarge': 'https://img.youtube.com/vi/h399XRm-OcA/sddefault.jpg',
        'imagePathThumbnail': 'https://img.youtube.com/vi/h399XRm-OcA/default.jpg',
        'videoPath': 'https://www.youtube.com/embed/h399XRm-OcA?rel=0&showinfo=0',
        'videoTitle': 'Honda Navi : First Impression : PowerDrift'
    },
    {
        'imagePathLarge': 'https://img.youtube.com/vi/jOdAplDI2FI/sddefault.jpg',
        'imagePathThumbnail': 'https://img.youtube.com/vi/jOdAplDI2FI/default.jpg',
        'videoPath': 'https://www.youtube.com/embed/jOdAplDI2FI?rel=0&showinfo=0',
        'videoTitle': 'Launch Alert : Bajaj Dominar 400 : PowerDrift'
    },
    {
        'imagePathLarge': 'https://img.youtube.com/vi/W1KOvK9_gAc/sddefault.jpg',
        'imagePathThumbnail': 'https://img.youtube.com/vi/W1KOvK9_gAc/default.jpg',
        'videoPath': 'https://www.youtube.com/embed/W1KOvK9_gAc?rel=0&showinfo=0',
        'videoTitle': 'Indian Scout Sixty : Launch Alert : PowerDrift'
    }
];

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

    self.photoSwiperActive = ko.observable(true);

    // footer screens
    self.screenActive = ko.observable(false);
    self.photoThumbnailScreen = ko.observable(false);
    self.colorsThumbnailScreen = ko.observable(false);
    self.modelInfoScreen = ko.observable(false);

    // swiper
    self.photoHeadingActive = ko.observable(true);

    self.activePhotoTitle = ko.observable('');
    self.activePhotoIndex = ko.observable(0);

    self.activeColorTitle = ko.observable('');
    self.activeColorIndex = ko.observable(0);

    self.photoList = ko.observableArray(modelImages);
    self.colorPhotoList = ko.observableArray(modelColorImages);

    self.afterRender = function(){
        if (!self.mainSwiper) {
            self.mainSwiper = $('#main-photo-swiper').swiper({
                spaceBetween: 0,
                pagination: '.swiper-pagination',
                paginationType: 'fraction',
                onInit: function (swiper) {
                    setPhotoDetails(swiper);
                },
                onClick: function () {
                    if (window.innerWidth > window.innerHeight) {
                        self.TOGGLE_FOOTER_TABS();
                        self.TOGGLE_PHOTO_HEADING();
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

        if (!self.mainColorSwiper) {
            self.mainColorSwiper = $('#main-color-swiper').swiper({
                spaceBetween: 0,
                pagination: '.swiper-pagination',
                paginationType: 'fraction',
                onInit: function (swiper) {
                    setColorPhotoDetails(swiper);
                },
                onClick: function () {
                    if (window.innerWidth > window.innerHeight) {
                        self.TOGGLE_FOOTER_TABS();
                        self.TOGGLE_PHOTO_HEADING();
                    }
                    if (self.modelInfoScreen()) {
                        self.modelInfoScreen(false);
                    }
                },
                onTouchStart: function () {
                    if (self.modelInfoScreen()) {
                        self.modelInfoScreen(false);
                    }
                },
                onSlideChangeStart: function (swiper) {
                    setColorPhotoDetails(swiper);
                }
            });
        };
    };

    self.ACTIVATE_PHOTO_TAB = function () {
        self.photosTabActive(true);
    };

    self.DEACTIVATE_PHOTO_TAB = function () {
        self.photosTabActive(false);
        if (self.screenActive()) {
            deactivateAllScreens();
        }
    };

    // all photos tab
    self.TOGGLE_PHOTO_THUMBNAIL_SCREEN = function () {
        if (!self.photoThumbnailScreen()) {
            // deactivate all other screens
            deactivateAllScreens();
            self.photoThumbnailScreen(true);
            self.photoSwiperActive(true);
            self.INITIATE_PHOTO_THUMBNAIL_SWIPER();
        }
        else {
            self.photoThumbnailScreen(false);
        }

        self.screenActive(self.photoThumbnailScreen());
    };

    self.TOGGLE_COLORS_THUMBNAIL_SCREEN = function () {
        if (!self.colorsThumbnailScreen()) {
            deactivateAllScreens();
            self.colorsThumbnailScreen(true);
            self.photoSwiperActive(false);
            self.mainColorSwiper.update(true);
            self.INITIATE_COLOR_THUMBNAIL_SWIPER();
        }
        else {
            self.colorsThumbnailScreen(false);
        }

        self.screenActive(self.colorsThumbnailScreen());

    };

    self.TOGGLE_MODEL_INFO_SCREEN = function () {
        if (!self.modelInfoScreen()) {
            deactivateAllScreens();
            self.modelInfoScreen(true);
        }
        else {
            self.modelInfoScreen(false);
        }

        self.screenActive(self.modelInfoScreen());
    };

    self.INITIATE_PHOTO_THUMBNAIL_SWIPER = function () {
        self.thumbnailSwiper = $('#thumbnail-photo-swiper').swiper({
            slidesPerView: 'auto',
            spaceBetween: 0,
            slideActiveClass: '',
            slideToClickedSlide: true,
            onClick: function (swiper) {
                focusGalleryPhoto(swiper);
            }
        });

        focusThumbnail(self.thumbnailSwiper);
    };

    self.INITIATE_COLOR_THUMBNAIL_SWIPER = function () {
        self.colorThumbnailSwiper = $('#thumbnail-colors-swiper').swiper({
            slidesPerView: 'auto',
            spaceBetween: 0,
            slideActiveClass: '',
            slideToClickedSlide: true,
            onClick: function (swiper) {
                focusColorGalleryPhoto(swiper);
            }
        });

        focusColorThumbnail(self.colorThumbnailSwiper);
    };

    self.HIDE_GALLERY_TABS = function () {
        self.galleryTabsActive(false);
    };

    self.SHOW_GALLERY_TABS = function () {
        self.galleryTabsActive(true);
    };

    self.HIDE_FOOTER_TABS = function () {
        self.galleryFooterActive(false);
    };

    self.SHOW_FOOTER_TABS = function () {
        self.galleryFooterActive(true);
    };

    self.TOGGLE_FOOTER_TABS = function () {
        self.galleryFooterActive() ? self.HIDE_FOOTER_TABS() : self.SHOW_FOOTER_TABS();
    };

    self.HIDE_PHOTO_HEADING = function () {
        self.photoHeadingActive(false);
    };

    self.SHOW_PHOTO_HEADING = function () {
        self.photoHeadingActive(true);
    };

    self.TOGGLE_PHOTO_HEADING = function () {
        self.photoHeadingActive() ? self.HIDE_PHOTO_HEADING() : self.SHOW_PHOTO_HEADING();
    };

    window.addEventListener('resize', resizeHandler, true);

    function resizeHandler() {
        if (window.innerWidth > window.innerHeight) {
            self.HIDE_GALLERY_TABS();
            self.HIDE_FOOTER_TABS();
            self.HIDE_PHOTO_HEADING();
        }
        else {
            self.SHOW_GALLERY_TABS();
            self.SHOW_FOOTER_TABS();
            self.SHOW_PHOTO_HEADING();
        }
    };

    // swiper
    function setPhotoDetails(swiper) {
        var activeSlide = swiper.slides[swiper.activeIndex],
            activeSlideTitle = $(activeSlide).find('img').attr('title');

        self.activePhotoIndex(swiper.activeIndex + 1);
        self.activePhotoTitle(activeSlideTitle);
    };

    function setColorPhotoDetails(swiper) {
        var activeSlide = swiper.slides[swiper.activeIndex],
            activeSlideTitle = $(activeSlide).find('img').attr('title');

        self.activeColorIndex(swiper.activeIndex + 1);
        self.activeColorTitle(activeSlideTitle);
        
        if (self.colorThumbnailSwiper) {
            focusColorThumbnail(self.colorThumbnailSwiper);
        }
    };

    // focus gallery to clicked thumbnail index
    function focusGalleryPhoto(swiper) {
        var thumbnailIndex = swiper.clickedIndex;

        self.mainSwiper.slideTo(thumbnailIndex);
        focusThumbnail(swiper);
    };

    function focusColorGalleryPhoto(swiper) {
        var thumbnailIndex = swiper.clickedIndex;

        self.mainColorSwiper.slideTo(thumbnailIndex);
        focusColorThumbnail(swiper);
    };

    // focus thumbnail to main photo index
    function focusThumbnail(swiper) {
        var activeIndex = self.activePhotoIndex() - 1, // decrement by 1, since it was incremented by 1
            thumbnailIndex = swiper.slides[activeIndex];

        $(swiper.slides).removeClass('swiper-slide-active');
        $(thumbnailIndex).addClass('swiper-slide-active');
        swiper.slideTo(activeIndex);
    };

    function focusColorThumbnail(swiper) {
        var activeIndex = self.activeColorIndex() - 1, // decrement by 1, since it was incremented by 1
            thumbnailIndex = swiper.slides[activeIndex];

        $(swiper.slides).removeClass('swiper-slide-active');
        $(thumbnailIndex).addClass('swiper-slide-active');
        swiper.slideTo(activeIndex);
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