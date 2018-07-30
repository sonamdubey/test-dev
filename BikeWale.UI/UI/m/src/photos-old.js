var cacheData,
    modelColorImageCount = 0,
    pageNo = 1,
    modelImages = [],
    pageSize = 4,
    modelColorImages = []
    videoList = null;

(function () {
    try {
    var imageList = JSON.parse(Base64.decode(encodedImageList));
    videoList = JSON.parse(Base64.decode(encodedVideoList));

    modelImages = imageList;
    modelColorImages = filterColorImagesArray(imageList);

    if(modelColorImages)
    modelColorImageCount = modelColorImages.length;   
    } catch (e) {
        console.warn(e);
    }
})();


$(document).ready(function () {
    var photosLength = $('.photos-grid-list').first().find('li').length,
        photosLimit = 30,
        lastPhotoIndex = photosLimit - 1;

    // add 'more photos count' if photo grid contains 30 images
    if (photosLength == photosLimit) {
        var lastPhoto = $('.photos-grid-list li').eq(lastPhotoIndex),
            morePhotoCount = $('<span class="black-overlay"><span class="font14 text-white">+' + (photoCount - lastPhotoIndex) + '<br />images</span></span>');
        lastPhoto.append(morePhotoCount);

    }
});


$('.photos-grid-list').on('click', 'li', function () {
    var photoCount = $('.photos-grid-list li').length;
    if (photoCount > 1) {
        bindGallery($(this));
    }
});

var bindGallery = function (clickedImg) {
    triggerGA('Gallery_Page', 'Gallery_Loaded', modelName);
    vmPhotosPage.photoGalleryContainerActive(true);
    var imgIndex = clickedImg.index(),
        parentGridType = clickedImg.closest('.photos-grid-list');

    if (parentGridType.hasClass('remainder-grid-list')) {
        var gridOneLength = $('.photos-grid-list').first().find('li').length;

        imgIndex = gridOneLength + imgIndex; // (grid type 1's length + grid type remainder's index)
    }

    vmPhotosPage.imageIndex(imgIndex);
    showGallery();

    if (!isModelPage) {
        window.location.hash = 'photosGallery';
    }
    pageNo = 1;
};

$(document).on('click', '#gallery-close-btn', function () {
    if (isModelPage) {
        gallery.gotoModelPage();
    }
    else {
        gallery.close();
    }
});

function toggleFullScreen(goFullScreen) {
    var doc = window.document;
    var docElement = doc.documentElement;

    var requestFullScreen = docElement.requestFullscreen || docElement.mozRequestFullScreen || docElement.webkitRequestFullScreen || docElement.msRequestFullscreen;
    var cancelFullScreen = doc.exitFullscreen || doc.mozCancelFullScreen || doc.webkitExitFullscreen || doc.msExitFullscreen || doc.webkitCancelFullScreen;

    if (goFullScreen && requestFullScreen != undefined) {
        docElement.style.backgroundColor = '#000';
        requestFullScreen.call(docElement);
    }
    else if (cancelFullScreen != undefined) {
        cancelFullScreen.call(doc);
        docElement.style.backgroundColor = '';
    }
}

function filterColorImagesArray(responseArray) {
    return ko.utils.arrayFilter(responseArray, function (response) {
        return response.ImageType == 3;
    });
}

function bindPhotoGallery() {
    // remove the binding and then re-apply
    vmPhotosPage.photoGalleryContainerActive(false);
    vmPhotosPage.activateGallery(true);
    ko.cleanNode(document.getElementById('gallery-root'))
    ko.applyBindings(vmPhotosPage, document.getElementById('gallery-root'))
    $('body').addClass('lock-browser-scroll');
}

function showGallery() {
    try {
        bindPhotoGallery();
    }
    catch (e) {
        console.warn(e);
    }
};

function hideGallery() {
    vmPhotosPage.activateGallery(false);
    vmPhotosPage.photoGalleryContainerActive(false);
    $('body').removeClass('lock-browser-scroll');
    toggleFullScreen(false);
};

var photosPage = function () {
    var self = this;
    self.photoGalleryContainerActive = ko.observable(false);
    self.activateGallery = ko.observable(false);
    self.imageIndex = ko.observable(0);
};

var vmPhotosPage = new photosPage();

var modelGallery = function () {
    var self = this;
    var activeVideo = 0;
    self.photosTabActive = ko.observable(true);

    self.galleryTabsActive = ko.observable(true);
    self.galleryFooterActive = ko.observable(true);
    self.photoSwiperActive = ko.observable(true);
    self.fullScreenModeActive = ko.observable(false);
    self.colorTabActive = ko.observable(true);

    // footer screens
    self.screenActive = ko.observable(false);
    self.photoThumbnailScreen = ko.observable(false);
    self.colorsThumbnailScreen = ko.observable(false);
    self.modelInfoScreen = ko.observable(false);
    self.isScreenRotated = ko.observable(false);

    // swiper
    self.photoHeadingActive = ko.observable(true);

    self.activePhotoTitle = ko.observable('');
    self.activePhotoIndex = ko.observable(vmPhotosPage.imageIndex());

    self.activeColorTitle = ko.observable('');

    self.activeColorIndex = ko.observable(0);
    self.colorTabActive(modelColorImageCount == 0 ? false : true);

    // video
    self.activeVideoTitle = ko.observable('');
    self.activeVideoIndex = ko.observable(0);
    self.activeVideoId = ko.observable();

    self.videoListScreen = ko.observable(false);

    self.photoList = ko.observableArray(modelImages);
    self.colorPhotoList = ko.observableArray(modelColorImages);

    self.videoList = ko.observableArray([]);

    self.afterRender = function () {
        if (!self.mainSwiper) {
            self.mainSwiper = $('#main-photo-swiper').swiper({
                spaceBetween: 0,
                preloadImages: false,
                lazyLoading: true,
                nextButton: '.swiper-button-next',
                prevButton: '.swiper-button-prev',
                onInit: function (swiper) {
                    swiper.slideTo(self.activePhotoIndex());
                    setPhotoDetails(swiper);
                },
                onClick: function () {
                    if (window.innerWidth > window.innerHeight) {
                        self.toggleFooterTabs();
                        self.togglePhotoHeading();
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
                preloadImages: false,
                lazyLoading: true,
                nextButton: '.swiper-button-next',
                prevButton: '.swiper-button-prev',
                onInit: function (swiper) {
                    setColorPhotoDetails(swiper);
                },
                onClick: function () {
                    if (window.innerWidth > window.innerHeight) {
                        self.toggleFooterTabs();
                        self.togglePhotoHeading();
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
                    triggerGA('Gallery_Page', 'Colour_Changed', modelName);
                    setColorPhotoDetails(swiper);
                }
            });
        };
    };

    self.togglePhotoTab = function () {
        if (!self.photosTabActive()) {
            self.photosTabActive(true);
            activeVideo = self.activeVideoIndex();
            self.activeVideoId('');
        }
        else {
            self.photosTabActive(false);
            toggleFullScreen(false);
            self.getVideos();
            setVideoDetails(activeVideo);
            triggerGA('Gallery_Page', 'Videos_Clicked', modelName);
        }
        if (self.screenActive()) {
            deactivateAllScreens();
        }
    };

    self.isPhotoThumbnailInitialized = ko.observable(false);

    // all photos tab
    self.togglePhotoThumbnailScreen = function () {
        if (!self.photoThumbnailScreen()) {
            // deactivate all other screens
            deactivateAllScreens();
            self.mainSwiper.update(true);
            if (!self.isPhotoThumbnailInitialized()) {
                self.initiatePhotoThumbnailSwiper();
                self.isPhotoThumbnailInitialized(true);
            }
            // activate clicked tab screen
            self.photoThumbnailScreen(true);
            self.photoSwiperActive(true);
            triggerGA('Gallery_Page', 'All_Photos_Tab_Clicked_Opened', modelName);
        }
        else {
            self.photoThumbnailScreen(false);
            triggerGA('Gallery_Page', 'All_Photos_Tab_Clicked_Closed', modelName);
        }
        self.screenActive(self.photoThumbnailScreen());
    };

    self.toggleColorThumbnailScreen = function () {
        if (!self.colorsThumbnailScreen()) {
            deactivateAllScreens();
            self.colorsThumbnailScreen(true);
            self.photoSwiperActive(false);
            self.mainColorSwiper.update(true);
            self.initiateColorThumbnailSwiper();
            triggerGA('Gallery_Page', 'Colours_Tab_Clicked_Opened', modelName);
        }
        else {
            self.colorsThumbnailScreen(false);
            triggerGA('Gallery_Page', 'Colours_Tab_Clicked_Closed', modelName);
        }
        self.screenActive(self.colorsThumbnailScreen());
    };

    self.toggleModelInfoScreen = function () {
        if (!self.modelInfoScreen()) {
            deactivateAllScreens();
            if (navigator.userAgent.indexOf('UCBrowser/') >= 0) {
                if (!$('#gallery-root').hasClass('uc-iframe-position')) {
                    $('#gallery-root').addClass('uc-iframe-position');
                }
            }
            self.modelInfoScreen(true);
            triggerGA('Gallery_Page', 'Info_Tab_Clicked', modelName);
        }
        else {
            self.modelInfoScreen(false);
        }

        self.screenActive(self.modelInfoScreen());
    };

    self.initiatePhotoThumbnailSwiper = function () {
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

    self.initiateColorThumbnailSwiper = function () {
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

    self.hideGalleryTabs = function () {
        self.galleryTabsActive(false);
    };

    self.showGalleryTabs = function () {
        self.galleryTabsActive(true);
    };

    self.hideFooterTabs = function () {
        self.galleryFooterActive(false);
    };

    self.showFooterTabs = function () {
        self.galleryFooterActive(true);
    };

    self.toggleFooterTabs = function () {
        self.galleryFooterActive() ? self.hideFooterTabs() : self.showFooterTabs();
    };

    self.hidePhotoHeading = function () {
        self.photoHeadingActive(false);
    };

    self.showPhotoHeading = function () {
        self.photoHeadingActive(true);
    };

    self.togglePhotoHeading = function () {
        self.photoHeadingActive() ? self.hidePhotoHeading() : self.showPhotoHeading();
    };

    self.toggleFullScreen = function () {
        fadeOutFooterTabs();
        deactivateAllScreens();
        if (!self.fullScreenModeActive()) {
            toggleFullScreen(true);
            self.fullScreenModeActive(true);
        }
        else {
            toggleFullScreen(false);
            self.fullScreenModeActive(false);
        }
    };

    // video
    self.toggleVideoListScreen = function () {
        if (!self.videoListScreen()) {
            deactivateAllScreens();
            self.videoListScreen(true);
            self.getVideos();
            triggerGA('Gallery_Page', 'All_Videos_Tab_Clicked_Opened', modelName);
        }
        else {
            self.videoListScreen(false);
            triggerGA('Gallery_Page', 'All_Videos_Tab_Clicked_Closed', modelName);
        }
        self.screenActive(self.videoListScreen());
    };

    self.videoSelection = function (data, event) {
        var element = $(event.currentTarget);

        if (!element.hasClass('active')) {
            var elementIndex = element.index();

            setVideoDetails(elementIndex);
        }
    };

    function setVideoDetails(elementIndex) {
        var element = $('.video-tab-list li')[elementIndex],
            elementId = self.videoList()[elementIndex].VideoId,
            elementTitle = self.videoList()[elementIndex].VideoTitle;       

        self.activeVideoId(elementId);
        self.activeVideoIndex(elementIndex);
        self.activeVideoTitle(elementTitle);       

        (document.getElementById("iframe-video").contentWindow || document.getElementById("iframe-video").documentWindow).location.replace('https://www.youtube.com/embed/' + self.activeVideoId() + '?showinfo=0');

        $(element).siblings().removeClass('active');
        $(element).addClass('active');

    };

    window.addEventListener('resize', resizeHandler, true);

    resizeHandler();
    window.addEventListener('scroll', videoScroll, true);

    function resizeHandler() {
        if (window.innerWidth > window.innerHeight) {
            self.hideGalleryTabs();
            self.hideFooterTabs();
            self.hidePhotoHeading();

            if (!self.photosTabActive()) {
                $('.main-video-iframe-content').css({ 'padding-bottom': '44%' });
            }
        }
        else {
            self.showGalleryTabs();
            self.showFooterTabs();
            self.showPhotoHeading();

            if (self.photosTabActive()) {
                $('.main-video-iframe-content').css({ 'padding-bottom': '56.25%' });
            }
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

        swiper.slideTo(activeIndex);
        $(swiper.slides).removeClass('swiper-slide-active');
        $(thumbnailIndex).addClass('swiper-slide-active');
    };

    function focusColorThumbnail(swiper) {
        var activeIndex = self.activeColorIndex() - 1,
            thumbnailIndex = swiper.slides[activeIndex];

        swiper.slideTo(activeIndex);
        $(swiper.slides).removeClass('swiper-slide-active');
        $(thumbnailIndex).addClass('swiper-slide-active');
    };

    function fadeOutFooterTabs() {
        $('.gallery-footer').hide();
        setTimeout(function () {
            $('.gallery-footer').show();
        }, 500);
    };

    function deactivateAllScreens() {
        self.photoThumbnailScreen(false);
        self.colorsThumbnailScreen(false);
        self.modelInfoScreen(false);
        self.videoListScreen(false);
    };    

    self.getVideos = function () {
        try {
            pushVideoList(videoList);
        } catch (e) {
            console.warn("Unable to fetch Videos model gallery " + e.message);
        }
    }

    function pushVideoList(response) {
        ko.utils.arrayPushAll(self.videoList(), ko.toJS(response));
        if (!self.activeVideoId()) {
            setVideoDetails(0);
        }
        pageNo = pageNo + 1;
        self.videoList.notifySubscribers();
    }
    
    function videoScroll() {
        if ($("#main-video-content").is(":visible")) {
            var winScroll = $('#video-tab-screen').scrollTop(),
                pageHeight = $('#video-tab-screen').height(),
                windowHeight = $('#video-tab-screen').height();
            var position = pageHeight - (windowHeight);
            if (winScroll >= position && videoCount > pageNo * pageSize && isNextPage) {
                isNextPage = false;
                pageNo = pageNo + 1;
                self.getVideos();
            }
        }
    }
}

ko.components.register("gallery-component", {
    viewModel: modelGallery,
    template: { element: "gallery-template-wrapper" }
});

ko.applyBindings(vmPhotosPage, document.getElementById('gallery-root'));

var gallery = {
    close: function () {
        hideGallery();

    },

    gotoModelPage: function () {
        window.location.href = window.location.pathname.split("images/")[0];
    }
};
