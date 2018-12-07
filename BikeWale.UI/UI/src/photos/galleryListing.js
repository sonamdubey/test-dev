var galleryColorThumbnailSwiper = null;
var refreshAdSlot = "div-gpt-ad-1516082384256-19";

var modelColorImageCount = 0, lastSlide = 0, MODEL_IMAGES = [], MODEL_COLOR_IMAGES = [], MODEL_VIDEO_LIST = null;
var eleGallery, vmModelGallery, vmPhotosMore;

var PHOTO_COUNT, VIDEO_COUNT, MODEL_NAME, MAKE_NAME, BIKE_MODEL_ID, IMAGE_INDEX, COLOR_IMAGE_ID, COLOR_INDEX, COLOR_IMAGE_SELECTED = false, RETURN_URL, MODEL_IMAGE_SELECTED = false, MAIN_SWIPER_SWIPED = false;

var imageTypes = ["Other", "ModelImage", "ModelGallaryImage", "ModelColorImage"];

var triggerGalleryImageChangeGA, logBhrighu;

var buttonClicked = false, currentSlide = 0, lastColorSlide = 0, currentColorSlide = 0, currentPage;

var setPageVariables = function () {
    eleGallery = $("#pageGallery");

    try {
        PHOTO_COUNT = eleGallery.data("photoscount");
        VIDEO_COUNT = eleGallery.data("videoscount");
        IMAGE_INDEX = eleGallery.data("selectedimageid");
        COLOR_IMAGE_ID = eleGallery.data("selectedcolorimageid");
        RETURN_URL = eleGallery.data("returnurl");
        MAKE_NAME = eleGallery.data("makename");
        MODEL_NAME = eleGallery.data("modelname");
        BIKE_MODEL_ID = eleGallery.data("modelid");
        MAKE_NAME = eleGallery.data("makename");
        triggerGalleryImageChangeGA = true;
        logBhrighu = true;
        currentPage = 'Model_Images_Page';
        
        if (eleGallery.length > 0 && eleGallery.data("images") != '') {
            var imageList = JSON.parse(Base64.decode(eleGallery.data("images")));
            MODEL_IMAGES = imageList;
            MODEL_COLOR_IMAGES = filterColorImagesArray(imageList);

            if (MODEL_COLOR_IMAGES) {
                modelColorImageCount = MODEL_COLOR_IMAGES.length;
            }

            if (RETURN_URL) {
                if (COLOR_IMAGE_ID > 0) {
                    ko.utils.arrayForEach(MODEL_COLOR_IMAGES, function (item, index) {
                        if (item.ColorId === COLOR_IMAGE_ID) {
                            COLOR_IMAGE_SELECTED = true;
                            COLOR_INDEX = index;
                        }
                    });
                }
                else {
                    MODEL_IMAGE_SELECTED = true;
                }  
            }

        }

        if (eleGallery.length > 0 && eleGallery.data("videos") != '') {
            MODEL_VIDEO_LIST = JSON.parse(Base64.decode(eleGallery.data("videos")));
        }

    } catch (e) {
        console.warn(e);
    }
}

function filterColorImagesArray(responseArray) {
    return ko.utils.arrayFilter(responseArray, function (response) {
        return response.ImageType == 3;
    });
}

var LoadPhotosViewModel = function () {
    var self = this;
    self.Loadedphotos = ko.observableArray();
}

var MainGallery = (function () {
    var isTitleHeightSet, galleryPopup, colorGalleryPopup;
    var descriptionBase;
    var title;
    var date;
    var views;
    var likecount;
    var trimmedDesc;
    var fullDesc;
    var readMore;

    function _setSelector() {
        isTitleHeightSet = false;
        galleryPopup = $("#galleryRoot");
        colorGalleryPopup = $('#colorGalleryPopup');
    };

    function _setVideoRelatedSelectors() {
        if (VIDEO_COUNT > 1) {
            descriptionBase = $('#video-details');
            title = descriptionBase.find(".video-container__title");
            date = descriptionBase.find(".display-date");
            views = descriptionBase.find(".display-views");
            likecount = descriptionBase.find(".like-count");
            trimmedDesc = descriptionBase.find(".read-more-wrapper__trim-content");
            fullDesc = descriptionBase.find(".read-more-wrapper__full-content");
            readMore = descriptionBase.find(".read-more-wrapper");
        }
    }

    function openPopup(activeIndex) {
        toggleFullScreen(true);
        galleryPopup.addClass('popup--active');
        modelSwiper.enableMousewheelControl();
        modelSwiper.update();

        modelSwiper.slideTo(activeIndex, 0, false);
        thumbnailSwiper.update();
        thumbnailSwiper.lazy.load();
        popup.lock();
        $('.blackOut-window').hide();

        vmModelGallery.updateIndexandTitle(activeIndex);

        // set head width in gallery popup
        _setPopupHeaderWidth();
        currentPage = "Gallery_Page";
        triggerGA(currentPage, 'Gallery_Loaded', MAKE_NAME + "_" + MODEL_NAME);
        if (logBhrighu) {
            logBhrighuForImage($("#main-photo-swiper .swiper-slide-active"));
        }
        logBhrighu = true;
    };

    function closePopup() {

        if (RETURN_URL) {
            return window.location.href = RETURN_URL;
        }

        if (VIDEO_COUNT > 0) {
            YoutubeAPI.pauseYoutubeVideo();
        }
        toggleFullScreen(false);
        galleryPopup.removeClass('popup--active video-swiper--active gallery--visible gallery-popup__color--active');
        modelSwiper.container.data('swiper').disableMousewheelControl();
        modelSwiper.update();
        modelSwiper.slideTo(modelSwiper.activeIndex, 0, false)
        popup.unlock();
        colorGalleryPopup.removeClass('color-popup--active');

        // change active tab to image tab
        $('.model-gallery__tab-container > .tab--active').removeClass('tab--active');
        $('.model-gallery__tab-container > li:first-child').addClass('tab--active');
        currentPage = "Model_Images_Page";
    };

    function _setPopupHeaderWidth() {
        if (!isTitleHeightSet) {
            var titleWidth = $('.js-gallery-popup-title').outerWidth();
            if (titleWidth > 370) { // compare with Tab's width
                $('.js-gallery-popup-head').css('width', titleWidth);
            }
            isTitleHeightSet = true;
        }
    };

    function _calculateSidebarVideoSwiperHeight() {
        var header = $(".model-gallery__popup-head") ? $(".model-gallery__popup-head").outerHeight(true) : 0;
        var headPanel = $(".video-container__head-panel") ? $(".video-container__head-panel").outerHeight(true) : 0;
        var swiperAbsoluteBtnHight = $(".video-swiper--next-btn") ? $(".video-swiper__arrow-btn").outerHeight() : 0;
        var marginBottom = 40;
        var viewportHeight = window.innerHeight;
        var contentHeight = viewportHeight - (header + headPanel + swiperAbsoluteBtnHight + marginBottom);
        $(".swiper-card").css("height", contentHeight);
    };

    function _handleTabClick(element) {
        var tabId = element.attr('data-tabs');
        var modelGalleryContainer = element.closest(".model-gallery__container")

        modelGalleryContainer.find(".tab--active").removeClass('tab--active');
        element.addClass("tab--active");

        if (tabId === "imageTab") {
            if (VIDEO_COUNT > 0) {
                YoutubeAPI.pauseYoutubeVideo();
            }
            modelGalleryContainer.removeClass("video-swiper--active");
        }
        else if (tabId === "videoTab") {
            if (modelGalleryContainer.hasClass("popup--active")) {
                modelGalleryContainer.addClass("video-swiper--active");
                colorGalleryPopup.removeClass("color-popup--active");
            }
            // for vertical swiper in video popup
            if (VIDEO_COUNT > 1) {
                verticalVideoSwiper.init();
            }

            _calculateSidebarVideoSwiperHeight();
        }
        $('#galleryRoot').removeClass('gallery--visible');
    };

    function _imageGallery() {
        $('#main-photo-swiper').on('click', '.swiper-slide', function () {
          if(!$("#galleryRoot").hasClass('popup--active')) {
            var activeIndex = $(this).index();
            openPopup(activeIndex);
          }
        });

        $(".model-gallery__container").on('click', '.gallery-close-btn__icon', function () {
            closePopup();
        });
      
        $('.model-gallery__tab-container').on('click', 'li', function () {
            _handleTabClick($(this));
        });
      
        $('.gallery__zoom-icon').on('click', function (event) {
            if (!screenfull.isFullscreen) {
                openPopup(modelSwiper.activeIndex);
            }
            else {
                toggleFullScreen(false);
            }
        })
    };

    function _colorPopup() {
        if (modelColorImageCount > 0) {
            $('.gallery__color-btn').on('click', function () {
                galleryPopup.addClass('gallery-popup__color--active');
                colorGalleryPopup.addClass('color-popup--active');

                triggerGA(currentPage, 'Colours_Tab_Clicked_Opened', MAKE_NAME + '_' + MODEL_NAME);
            });

            $('.color-popup__close-btn').on('click', function () {
                galleryPopup.removeClass('gallery-popup__color--active');
                colorGalleryPopup.removeClass('color-popup--active');
                triggerGA(currentPage, 'Colours_Tab_Clicked_Closed', MAKE_NAME + '_' + MODEL_NAME);
            });
            $(document).on('click', '#galleryColorThumbnailSwiper .swiper-slide', function () {
                ThumbnailSwiper.handleThumbnailScroll($(this).index(), galleryColorThumbnailSwiper);
            })
        }
    };

    function _videoPopup() {

        if (VIDEO_COUNT == 1) {
            YoutubeAPI.onVideoPlaying = function () {
                if (!galleryPopup.hasClass("single-video--active")) {
                    galleryPopup.addClass("single-video--active");
                }
            };

            YoutubeAPI.onVideoPlaybackSuspended = function () {
                if (galleryPopup.hasClass("single-video--active")) {
                    galleryPopup.removeClass("single-video--active");
                }
            };
        }

        var vidSwiper = document.getElementById("galleryVerticalVideoSwiper");

        if (vidSwiper != undefined) {

            $("#galleryVerticalVideoSwiper .swiper-card__list-item").on('click', function () {

                var nowPlaying = '<div class="swiper-card__thumbnail-active">Now Playing</div>';

                clickedSlide = $(this);
                previousSlide = $('#galleryVerticalVideoSwiper').find('.swiper-card--active-card');

                var selectedVid = clickedSlide.attr('data-vidId');

                previousSlide.removeClass('swiper-card--active-card');
                clickedSlide.addClass('swiper-card--active-card');

                previousSlide.find(".swiper-card__thumbnail-active").remove();
                $(nowPlaying).insertAfter(clickedSlide.find(".swiper-card__thumbnail-img"));

                YoutubeAPI.generateIFrame(selectedVid);
                _updateVideoDescription(selectedVid);

            });
        }
    };

    function _setAnimationListeners() {
        if (modelColorImageCount > 0) {
            colorGalleryPopup[0].addEventListener('webkitAnimationEnd', function (event) {
                _handleColorSwiperInit(event);
            });
            colorGalleryPopup[0].addEventListener('msAnimationEnd', function (event) {
                _handleColorSwiperInit(event);
            });
            colorGalleryPopup[0].addEventListener('animationend', function (event) {
                _handleColorSwiperInit(event);
            });
        }
    };

    function _formatDisplayDate(dateObj) {

        var month_names = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];

        return month_names[dateObj.getMonth()] + " " + dateObj.getDate() + ", " + dateObj.getFullYear();

    };

    function _updateVideoDescription(videoId) {

        var readMoreClass = "read-more-wrapper--expand-active";
        
        ko.utils.arrayForEach(MODEL_VIDEO_LIST, function (item, index) {
            if (item.VideoId === videoId) {
                title.text(item.VideoTitle);

                var dateObj = new Date(item.DisplayDate);
                date.text(_formatDisplayDate(dateObj));

                views.text(item.Views);
                likecount.text(item.Likes);

                var desc = document.createElement("div");
                desc.innerHTML = item.Description;
                desc = desc.textContent;

                trimmedDesc.text(desc.substring(0, 200));
                fullDesc.text(desc);

                if (readMore.hasClass(readMoreClass)) {
                    readMore.removeClass(readMoreClass);
                }

                if (desc.length < 200) {
                    readMore.addClass(readMoreClass);
                }

            }
        });
        
    };

    function openColourPopup() {
        openPopup(0);
        galleryPopup.addClass('gallery-popup__color--active');
        colorGalleryPopup.addClass('color-popup--active');
    };

    function _handleColorSwiperInit(event) {
        if (event && event.target.classList.contains('color-popup--active')) {
            $('#preview-image').lazyload();
            galleryColorThumbnailSwiper.update();
        }
    }

    function _handleModelPageLinkage() {

        if (COLOR_IMAGE_SELECTED) {
            openColourPopup();
        }

        if (MODEL_IMAGE_SELECTED) {
            if (IMAGE_INDEX > 19) {
                ko.utils.arrayPushAll(vmModelGallery.limitedPhotoList(), MODEL_IMAGES.slice(20, IMAGE_INDEX + 10));
                vmModelGallery.slideLimit = IMAGE_INDEX + 10;
                vmModelGallery.limitedPhotoList.valueHasMutated();
            }
            openPopup(IMAGE_INDEX);
        }
    };

    function registerEvents() {
        _setSelector();
        _imageGallery();
        _colorPopup();
        _videoPopup();
        _setAnimationListeners();
        _handleModelPageLinkage();
        _setVideoRelatedSelectors();
    };

    return {
        registerEvents: registerEvents,
        openPopup: openPopup,
        closePopup: closePopup
    }
})();

var ThumbnailSwiper = (function () {
    function _focusIn(element) {
        $('.gallery-footer').addClass('footer--fadeout');
        thumbnailSwiper.update();
        thumbnailSwiper.slideTo(thumbnailSwiper.activeIndex, 0, false);
        element.find('.swiper-slide--active').removeClass('swiper-slide--active');
        element.find('.swiper-slide:nth-of-type(' + (modelSwiper.activeIndex + 1) + ')').addClass('swiper-slide--active');
    };

    function _focusOut() {
        $('.gallery-footer').removeClass('footer--fadeout');
        thumbnailSwiper.update();
        thumbnailSwiper.slideTo(thumbnailSwiper.activeIndex, 0, false);
    };

    function _setActiveSlide(element) {
        var elementIndex = element.index();
        $('#thumbnailSwiper .swiper-slide--active').removeClass('swiper-slide--active');
        element.addClass('swiper-slide--active');
        modelSwiper.slideTo(elementIndex);
    }
    function handleThumbnailScroll(activeIndex, childSwiper) {
        var nextThumbnailActive = childSwiper.slides[activeIndex + 1];
        var prevThumbnailActive = childSwiper.slides[activeIndex - 1];
        if (nextThumbnailActive && !nextThumbnailActive.classList.contains('swiper-slide-visible')) {
            childSwiper.slideNext();
        }
        else if (prevThumbnailActive && !(prevThumbnailActive.classList.contains('swiper-slide-visible'))) {
            childSwiper.slidePrev();
        }
    }
    function registerEvents() {
        $(document).on('click', '#thumbnailSwiper .swiper-slide', function () {
            buttonClicked = true;
            _setActiveSlide($(this));
        })
        $(document).on('mouseenter', '#thumbnailSwiper', function () {
            _focusIn($(this));
        })
        $(document).on('mouseleave', '#thumbnailSwiper', function () {
            _focusOut();
        })
    }

    return {
        registerEvents: registerEvents,
        handleThumbnailScroll: handleThumbnailScroll
    }

})();

var ImageGridGallery = (function () {
    function registerEvents() {

        $('#imageGridTop').on('click', '.image-grid-list__item', function (event) {
            if ($(event.target).closest('.js-video-slug').length) {
                return;
            }

            var element = $(this);
            var activeIndex = element.index();
            MainGallery.openPopup(activeIndex);
        })

        $(document).on('click', '#viewMoreImageBtn', function () {

            var listImageCount = $('.image-grid__list .image-grid-list__item').length;

            vmPhotosMore.Loadedphotos(vmModelGallery.photoList().slice(listImageCount, listImageCount + 16));
            $("#imageGridTop").append($("#photoTemplateWrapper ul li"));
            
            ImageGrid.alignRemainderImage(); //To handle image grid

            var currGridLength = $('.image-grid__list .image-grid-list__item').length;
            var currCarouselLength = vmModelGallery.limitedPhotoList().length;
            var newImageCount = vmModelGallery.photoList().length - currGridLength;

            if (newImageCount > 0) {
                $('#moreImageCount').html(newImageCount);
            }
            else {
                $('#moreImageCount').closest('.image-grid__list-more').hide();
            }

            // resize appended images
            var appendedImages = $('.image-grid__list .image-grid-list__item').splice(listImageCount, currGridLength);
            $(appendedImages).find('img').on('load', function () {
                ImageGrid.resizePortraitImageContainer($(this));
            });


            if (currGridLength > currCarouselLength) {
                ko.utils.arrayPushAll(vmModelGallery.limitedPhotoList(), MODEL_IMAGES.slice(currCarouselLength, currCarouselLength + 20));
                vmModelGallery.slideLimit = currCarouselLength + 20;
                vmModelGallery.limitedPhotoList.valueHasMutated();
            }
        });
    }
    return {
        registerEvents: registerEvents
    }
})();
var ColorTabSwiper = (function () {
    var imageArray;
    function _setSelector() {
        imageArray = MODEL_COLOR_IMAGES;
    };
    function registerEvents() {
        _setSelector();
        $('.overall-tabs__list').on('click', 'li', function () {
            var tabId = $(this).attr('data-tabs');
            if (tabId === "colours") {
                if (tabsColorThumbnailSwiper != undefined) {
                    tabsColorThumbnailSwiper.update();
                }
                _setColorWrapperHeight();
            }
        });

        $(document).on('click', '#tabsColorThumbnailSwiper .swiper-slide', function () {
            ThumbnailSwiper.handleThumbnailScroll($(this).index(), tabsColorThumbnailSwiper);
        })

        $(".model-color-list__item").on('click', function () {
            var element = $(this);
            var swiperWrapper = element.closest(".swiper-wrapper");
            var selectedColorName = element.find(".color-box__title").text();
            var colorIndex = element.attr('data-colorid');
            var result = '', color = '';
            swiperWrapper.find(".slide--focus").removeClass('slide--focus');
            element.addClass("slide--focus");
            $.each(imageArray, function (index, val) {
                if (val.ColorId == colorIndex) {
                    result = val.HostUrl + "0x0" + val.OriginalImgPath;
                    color = val.ImageTitle;
                }
                return result;
            });
            element.closest(".model-color-wrapper__footer").find(".model-color-wrapper__main-title").text(selectedColorName);
            element.closest(".model-color-wrapper").find(".model-color-wrapper__preview-img").attr('src', result);
            triggerGA(currentPage, currentPage === "Gallery_Page" ?  "Color_Changed" : "Color_Image_Carousel_Clicked", MAKE_NAME + "_" + MODEL_NAME + "_" + color);
            logBhrighuForImage($(this));
            _scrollToColorWrapper();
        });

        $('#thumbnailSwiper').on('hover', function () {
            thumbnailSwiper.lazy.load();
        });

        // Handle fullscreen exit
        if (screenfull.enabled) {
            screenfull.on('change', function () {
                if (!screenfull.isFullscreen) {
                    MainGallery.closePopup();
                }
            });
        }
    }

    function _setColorWrapperHeight() {
        var colorWrapper = $('#modelColorWrapper');
        var imageWrapper = colorWrapper.find('.model-color-wrapper__preview');

        if (!imageWrapper.attr('data-height')) {
            var colorFooterHeight = colorWrapper.find('.model-color-wrapper__footer').outerHeight(true);
            var floatingTabHeight = $('#overallTabsContent').height();

            var imageWrapperHeight = window.innerHeight - floatingTabHeight - colorFooterHeight;
            var maxHeight = imageWrapper.outerWidth(true) * 9 / 16;
            imageWrapperHeight = imageWrapperHeight > maxHeight ? maxHeight : imageWrapperHeight;

            imageWrapper.css('height', imageWrapperHeight).attr('data-height', true);
        }
    }

    function _scrollToColorWrapper() {
        var scrollPositionTop = $('#modelColorWrapper').offset().top - $('#overallTabsContent').height()

        if ($(window).scrollTop() > scrollPositionTop) {
            $('html, body').animate({
                scrollTop: scrollPositionTop
            })
        }
    }
    return {
        registerEvents: registerEvents,
        _scrollToColorWrapper: _scrollToColorWrapper
    }
})();


var SwiperEvents = (function () {
    
    function setDetails(swiper, viewModel) {
        var activeSlideIndex = swiper.activeIndex;
        var activeSlideTitle = $(swiper.slides[activeSlideIndex]).find('img').attr('alt');

        viewModel.activeIndex(activeSlideIndex + 1);
        viewModel.activeTitle(activeSlideTitle);
    }

    function setColorDetails(swiper, viewModel) {
        var activeSlideIndex = swiper.activeIndex;
        var activeSlideTitle = $(swiper.slides[activeSlideIndex]).attr('data-imgcat');

        viewModel.activeIndex(activeSlideIndex + 1);
        viewModel.activeTitle(activeSlideTitle);
    }

    return {
        setDetails: setDetails,
        setColorDetails: setColorDetails
    }
})();

$(document).ready(function () {
    setPageVariables();

    var galleryRoot = $('#galleryRoot');
    vmModelGallery = new ModelGalleryViewModel();
    if (galleryRoot.length) {
        ko.applyBindings(vmModelGallery, galleryRoot[0]);
    }

    var photoTemplate = $("#photoTemplateWrapper");
    if (photoTemplate.length) {
        vmPhotosMore = new LoadPhotosViewModel();
        ko.applyBindings(vmPhotosMore, photoTemplate[0]);
    }

    modelSwiper = new Swiper('#main-photo-swiper', {
        nextButton: '.gallery__next',
        prevButton: '.gallery__prev',
        setWrapperSize: true,
        slidesPerView: 1,
        preloadImages: false,
        lazyLoading: true,
        centeredSlides: true,
        spaceBetween: 0,
        observer: true,
        keyboardControl: true,
        lazyLoadingInPrevNext: true,
        mousewheelControl: true,
        watchSlidesVisibility: true,
        simulateTouch: false,
        onInit: function (swiper) {
            SwiperEvents.setDetails(swiper, vmModelGallery);
            swiper.disableMousewheelControl();
            $(this.nextButton).on('click', function () {
                buttonClicked = true;
            });
            $(this.prevButton).on('click', function () {
                buttonClicked = true;
            });
        },
        onSlideChangeStart: function (swiper) {
            if($('#galleryRoot').hasClass('popup--active')) {
                $('#galleryRoot').addClass('gallery--visible');
            }

            MAIN_SWIPER_SWIPED = true;

            ThumbnailSwiper.handleThumbnailScroll(swiper.activeIndex, thumbnailSwiper);

            lastSlide = vmModelGallery.activeIndex();
        },
        onSlideChangeEnd: function (swiper) {
            SwiperEvents.setDetails(swiper, vmModelGallery);

            if (!$('#galleryRoot').hasClass('popup--active') && swiper.activeIndex % 3 == 0) {
                refreshAd(refreshAdSlot);
            }

            if (vmModelGallery.slideLimit - lastSlide <= 5) {
                
                var startIndex = vmModelGallery.slideLimit < PHOTO_COUNT ? vmModelGallery.slideLimit : PHOTO_COUNT;
                var lastIndex = vmModelGallery.slideLimit + 10 < PHOTO_COUNT ? vmModelGallery.slideLimit + 10 : PHOTO_COUNT;
                ko.utils.arrayPushAll(vmModelGallery.limitedPhotoList(), MODEL_IMAGES.slice(startIndex, lastIndex));
                vmModelGallery.slideLimit = lastIndex;
                vmModelGallery.limitedPhotoList.valueHasMutated();
            }
            currentSlide = vmModelGallery.activeIndex();
            if (triggerGalleryImageChangeGA) {
                if (!buttonClicked) {
                    if (currentSlide != lastSlide) {
                        currentSlide > lastSlide ? triggerGA(currentPage, 'Swipe_Right', MAKE_NAME + "_" + MODEL_NAME) : triggerGA(currentPage, 'Swipe_Left', MAKE_NAME + "_" + MODEL_NAME);
                    }
                }
                else {
                    triggerGA(currentPage, 'Image_Carousel_Clicked', MAKE_NAME + "_" + MODEL_NAME);
                }
            }
            triggerGalleryImageChangeGA = true;
            logBhrighuForImage($("#main-photo-swiper .swiper-slide-active"));
            buttonClicked = false;
        }
    });

    NavigationTabs.registerEvents();
    NavigationTabs.setTab();
    thumbnailSwiper = new Swiper('#thumbnailSwiper', {
        nextButton: '.gallery-thumbnail__next',
        prevButton: '.gallery-thumbnail__prev',
        setWrapperSize: false,
        watchSlidesVisibility: true,
        slidesPerView: 'auto',
        slidesPerGroup: 3,
        preloadImages: false,
        observer: true,
        lazyLoading: true,
        centeredSlides: false,
        watchSlidesProgress: true,
        spaceBetween: 0,
        lazyLoadingInPrevNext: true,
        mousewheelControl: false,

        onSlideChangeStart: function (swiper) {
            lastThumb = thumbnailSwiper.activeIndex;
        },

        onSlideChangeEnd: function (swiper) {
            
            if (!MAIN_SWIPER_SWIPED && vmModelGallery.slideLimit - (lastThumb * 3) <= 5) {

                var startIndex = vmModelGallery.slideLimit < PHOTO_COUNT ? vmModelGallery.slideLimit : PHOTO_COUNT;
                var lastIndex = vmModelGallery.slideLimit + 10 < PHOTO_COUNT ? vmModelGallery.slideLimit + 10 : PHOTO_COUNT;
                ko.utils.arrayPushAll(vmModelGallery.limitedPhotoList(), MODEL_IMAGES.slice(startIndex, lastIndex));
                vmModelGallery.slideLimit = lastIndex;
                vmModelGallery.limitedPhotoList.valueHasMutated();
            }

            MAIN_SWIPER_SWIPED = false;
        }
        
    });

    verticalVideoSwiper = new Swiper('#galleryVerticalVideoSwiper', {
        direction: 'vertical',
        nextButton: '.video-swiper--next-btn',
        prevButton: '.video-swiper--prev-btn',
        slidesPerView: 'auto',
        slidesPerGroup: 3,
        setWrapperSize: false,
        preloadImages: false,
        lazyLoading: true,
        lazyLoadingInPrevNext: true,
        mousewheelControl: true,
        autoHeight: true
    });
    function initColorPopupThumbnailSwiper() {
        if (galleryColorThumbnailSwiper) {
            galleryColorThumbnailSwiper.update();
        }
        else {
            galleryColorThumbnailSwiper = new Swiper('#galleryColorThumbnailSwiper', {
                nextButton: '#galleryColorThumbnailSwiper .model-color-swiper__next-btn',
                prevButton: '#galleryColorThumbnailSwiper .model-color-swiper__prev-btn',
                slidesPerView: 'auto',
                slidesPerGroup: 3,
                setWrapperSize: false,
                watchSlidesVisibility: true
            });
        }
    }

    tabsColorThumbnailSwiper = new Swiper('#tabsColorThumbnailSwiper', {
        nextButton: '#tabsColorThumbnailSwiper .model-color-swiper__next-btn',
        prevButton: '#tabsColorThumbnailSwiper .model-color-swiper__prev-btn',
        slidesPerView: 'auto',
        slidesPerGroup: 3,
        setWrapperSize: false,
        watchSlidesVisibility: true
    });

    // image grid
    ImageGrid.alignRemainderImage();
    ImageGridGallery.registerEvents();

    // initialize color popup thumbnail swiper
    initColorPopupThumbnailSwiper();

    // MainGallery
    MainGallery.registerEvents();
    ThumbnailSwiper.registerEvents();

    // Color Tab
    ColorTabSwiper.registerEvents();

    //Read More in video
    HandleReadMore.registerEvents();

    // handle portrait images
    var topGridImages = $('#imageGridTop img');
    if (topGridImages.length) {
        topGridImages.each(function () {
            ImageGrid.resizePortraitImageContainer($(this));
        })
    }

    $('.image-grid-list__item img').on('load', function () {
        ImageGrid.resizePortraitImageContainer($(this));
    });
})
function logBhrighuForImage(item) {
    if (item) {
        var imageid = item.attr("data-imgid"), imgcat = item.attr("data-imgcat"), imgtype = item.attr("data-imgtype");
        if (imageid) {
            var lb = "";
            if (imgcat) {
                lb += "|category=" + imgcat;
            }

            if (imgtype) {
                lb += "|type=" + imageTypes[imgtype];
            }

            label = 'modelId=' + BIKE_MODEL_ID + '|imageid=' + imageid + lb + '|pageid=' + (gaObj ? gaObj.id : 0);
            cwTracking.trackImagesInteraction("BWImages", "ImageViewed", label);
            triggerVirtualPageView(window.location.host + window.location.pathname, lb);
        }
    }

}
