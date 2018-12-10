var MGallery = {
    suggestedGalleryShown: false,
    filters: {
        all: 1,
        interior: 2,
        exterior: 3,
        colors: 4
    },
    $tabNavEleTopPos:0,
    initTimerForNextImg: function (nextImageId) {
        if ((Number(MGallery.currentImage) > 0 ? MGallery.currentImage : null) !== nextImageId) {
            var time = $.now() - MGallery.timer;
            var label = "modelid=" + MGallery.modelId + "|timespent=" + time;
            label += "|imageid=" + MGallery.currentImage + "|makename=" + MGallery.makeName + "|modelname=" + MGallery.modelName;
            if (MGallery.currentImage) {
                cwTracking.trackCustomData("Images", "ImageView", label, false);
            }
            MGallery.timer = $.now();
            MGallery.currentImage = nextImageId;
        }
    },
    swiper: {
        carousel: undefined,
        zoomTracked: true,
        requestFullScreen: undefined,
        isRegisterCarousel: false,
        registerEvents: function () {
            $(document).on("fullscreenchange mozfullscreenchange webkitfullscreenchange msfullscreenchange", function (e) {
                var isFullScreen = MGallery.swiper.isFullScreen();
                $('a.image-gallery-close').toggleClass('hide', !isFullScreen);
                $('a.mgallery-fullscreen-link').toggleClass('hide', isFullScreen);
                $('a.btnDownload').toggleClass('hide', isFullScreen);
                MGallery.gallery.zoomToggle();
                MGallery.gallery.zoomBtnToggle();
                if (isFullScreen) {
                    lockPopup();
                    $(document).off('click', 'li.swiper-slide');
                }
                else {
                    $('body').removeClass('mgallery-fullscreen');
                    setTimeout(function () { unlockPopup() }, 1000);
                    MGallery.swiper.registerCarouselClick();
                    if (activeFilter == MGallery.filters.colors) {
                        var colorsDownloadButton = $('#colors-section-btnDownload');
                        $('#colors-swiper .swiper-slide-active img').attr('src') == noImagePath || MGallery.swiper.isFullScreen() ? colorsDownloadButton.addClass('hide') : colorsDownloadButton.removeClass('hide');
                    }
                }

            });
            MGallery.swiper.registerCarouselClick();

            $('#mainImage').click(function () {
                MGallery.swiper.openFullScreen();
                MGallery.swiper.carousel.slideTo(0);
                MGallery.swiper.onImageChange('carmodel-image-swipper', MGallery.swiper.carousel.activeIndex);
            });

            window.addEventListener("popstate", function () {
                var galleryPopup = $('#galleryPopup');
                if (!galleryPopup.hasClass('hide') && !showCarousel) {
                    galleryPopup.addClass('hide');
                    $('#topContainer').removeClass('hide');
                }
            });

            $(window).resize(function () {
                MGallery.gallery.zoomToggle();
            });

            $(document).on('webkitTransitionEnd otransitionend oTransitionEnd msTransitionEnd transitionend', '#imageGalleryContainer li.swiper-slide-active .swiper-zoom-container', function () {
                MGallery.swiper.imagesZoomEnd();
            });
        },
        registerCarouselClick: function () {
            $(document).on('click', 'li.swiper-slide.new', function () {
                if ((showCarousel || activeFilter != MGallery.filters.colors) && !MGallery.swiper.isFullScreen())
                    MGallery.swiper.openFullScreen();
            });
        },
        isFullScreen: function () {
            return document.fullscreen || document.mozFullScreen || document.webkitIsFullScreen || document.msFullscreenElement;
        },
        openFullScreen: function () {
            var isFullScreen;
            if (showCarousel) {

                $('.mgallery-fullscreen-top-data').show();
                var documentElement = window.document.documentElement;
                MGallery.swiper.requestFullScreen = documentElement.requestFullscreen || documentElement.mozRequestFullScreen || documentElement.webkitRequestFullScreen || documentElement.msRequestFullscreen;
                isFullScreen = document.webkitIsFullScreen;
                if (!isFullScreen && MGallery.swiper.requestFullScreen) {

                    $('body').addClass('mgallery-fullscreen');
                    var element = (activeFilter == MGallery.filters.colors) ? $('#tabColourSelection')[0] : $('div.swiper-container')[0];
                    MGallery.swiper.requestFullScreen.call(element);
                    screen.orientation.unlock();
                    screen.orientation.lock('landscape-primary');
                    $('.btnDownload img').attr('data-action', 'full_screen_download');
                }
                else if (isFullScreen) {

                    document.webkitCancelFullScreen();
                    screen.orientation.unlock();
                    screen.orientation.lock('portrait-primary');
                    $('.btnDownload img').attr('data-action', 'download');
                }
            }
            else {
                var galleryPopup = $('#galleryPopup');
                if (galleryPopup.hasClass('hide')) {
                    galleryPopup.removeClass('hide');
                    if (!MGallery.swiper.carousel || MGallery.swiper.isRegisterCarousel) {
                        MGallery.swiper.registerSwiperCarousal(false);
                        MGallery.swiper.isRegisterCarousel = false;
                    }
                    $('#topContainer').addClass('hide');
                    window.history.pushState("full-screen", "full-screen", "");
                }
                else {
                    galleryPopup.addClass('hide');
                    $('#topContainer').removeClass('hide');
                    window.history.back();
                }
            }
        },
        registerSwiperCarousal: function (zoom) {
            if (MGallery.swiper.carousel)
                MGallery.swiper.carousel.destroy(false, true);
            MGallery.swiper.carousel = $('.carmodel-image-swipper-container').swiper({
                nextButton: $(document).find('.model-swiper-next-btn'),
                prevButton: $(document).find('.model-swiper-prev-btn'),
                lazyLoadingInPrevNext: true,
                lazyLoadingInPrevNextAmount: 2,
                slidesPerView: 1,
                lazyLoading: true,
                zoom: zoom,
                zoomMax: 2.5,
                zoomMin: 1,
                zoomToggle: zoom,
                loop: false,
                spaceBetween: 0,
                onTouchEnd: function (carousal, transition) {
                    MGallery.swiper.imagesZoomEnd();
                }
            });
            MGallery.swiper.carousel.on('onSlideNextEnd', function () {
                MGallery.swiper.onSlide("image_next", 'carmodel-image-swipper', MGallery.swiper.carousel.activeIndex);
            });
            MGallery.swiper.carousel.on('onSlidePrevEnd', function () {
                MGallery.swiper.onSlide("image_prev", 'carmodel-image-swipper', MGallery.swiper.carousel.activeIndex);
            });
        },
        onSlide: function (label, carousal, activeIndex) {
            MGallery.swiper.onImageChange(carousal, activeIndex);
            $('.' + carousal + ' .mgallery-zoom .text-grey').text('Zoom In');
            MGallery.gallery.zoomBtnToggle();
            if (MGallery.swiper.isFullScreen())
                label += "_fullscreen";
            else {
                GalleryAds.swipeCounter++;
                GalleryAds.responsive.refreshAd(carousal);
            }
            Common.utils.trackAction('CWInteractive', 'msite_image_gallery', label, label);
        },
        onImageChange: function (carousal, activeIndex) {
            $('.' + carousal + ' div.mgallery-count span').eq(0).text(activeIndex + 1);
            var element = $('div.active-container ul.swiperUl li img[data-index]').eq(activeIndex);
            var url = element.closest('a.galleryAnchor').attr('href');
            window.history.replaceState('image-url', 'image-url', url);
            Common.utils.firePageView(window.location.pathname);
            MGallery.initTimerForNextImg(element.data("image-id"));
            fireComscorePageView();            
        },
        bindCarousel: function () {
            $('#imageGalleryContainer').children().remove();
            var elements = $('div.active-container ul.gallery-img-list li img:[data-index]');
            elements.each(function (index, element) {
                element = $(element);
                var img = $('#imageGalleryTemplate img');
                var arr = element.data('original').split('211x211');
                if (arr.length >= 2) {
                    img.attr('data-src', arr[0] + '1056x594' + arr[1]);
                }
                else {
                    arr = element.data('original').split('424x424');
                    img.attr('data-src', arr[0] + '1056x594' + arr[1]);
                }
                img.attr('data-image-id', element.data('image-id'));
                img.attr('data-image-name', element.data('image-name'));
                $('#imageGalleryContainer').append($('#imageGalleryTemplate').html());
            });
            var imageIndexElement = $('.carmodel-image-swipper div.mgallery-count span');
            imageIndexElement.eq(0).text(1);
            imageIndexElement.eq(1).text(elements.length);
            if (showCarousel) {
                $('#modelImage').parent().addClass('hide');
                $('#imageGalleryContainer').parent().removeClass('hide-important');
                MGallery.swiper.registerSwiperCarousal(true);
            }
            else {
                $('#modelImage').attr('src', $('#imageGalleryContainer img').eq(0).attr('data-src'));
            }
        },
        zoom: function (toggle, carousal, galleryElement) {
            if (toggle) {
                carousal.clickedSlide = undefined;
                carousal.zoom.toggleZoom(carousal);
            }
            if (carousal.zoom.currentScale > 1) {
                $('.' + galleryElement + ' .mgallery-zoom .text-grey').text('Zoom Out');
                //$('.' + galleryElement + ' .mgallery-zoom .fa').removeClass('fa-search-plus').addClass('fa-search-minus');
            }
            else {
                $('.' + galleryElement + ' .mgallery-zoom .text-grey').text('Zoom In');
                //$('.' + galleryElement + ' .mgallery-zoom .fa').removeClass('fa-search-minus').addClass('fa-search-plus');
            }
        },
        imagesZoomEnd: function () {
            MGallery.swiper.zoom(false, MGallery.swiper.carousel, 'carmodel-image-swipper');
            if (MGallery.swiper.carousel.zoom.currentScale > 1 && MGallery.swiper.zoomTracked) {
                Common.utils.trackAction('CWInteractive', 'msite_image_gallery', "zoom_in", "zoom-in");
                MGallery.swiper.zoomTracked = false;
            }
        }
    },
    gallery: {
        registerEvents: function () {
            $('ul [data-tabs]').click(function (event) {
                var element = $(this);
                var activeElement = $('div.cw-tabs-data.active-container');
                activeElement.removeClass('active-container').addClass('hide');
                var selector = $('#' + element.data('tabs'));
                selector.addClass('active-container').removeClass('hide');
                activeFilter = parseInt(selector.attr('enumId'));
                if (element.data('tabs') != 'tabColourSelection') {
                    event.preventDefault();
                    $('html').animate({ scrollTop: '0px' }, 300);
                    $("#showColours").addClass("hide")
                }
                else {
                    $("#showColours").removeClass("hide")
                }
                GalleryAds.swipeCounter = 0;
                GalleryAds.responsive.refreshAd("tab");
                if (selector.hasClass('loaded')) {
                    if (element.data('tabs') == 'tabColourSelection') {
                        $('#mainCarousal').slideUp('slow', function () {
                            MGallery.$tabNavEleTopPos = MGallery.floatingTabNav.calculateTabHeight();
                        });
                        $('.titleSpan').text('Colours');
                        MGallery.initTimerForNextImg(null);
                    }
                    else {
                        $('#mainCarousal').slideDown('slow', function () {
                            MGallery.$tabNavEleTopPos = MGallery.floatingTabNav.calculateTabHeight();
                        });
                        $('.titleSpan').text('Images');
                        MGallery.swiper.bindCarousel();
                        MGallery.swiper.isRegisterCarousel = true;
                        MGallery.initTimerForNextImg(MGallery.gallery.getActiveImg().data("image-id"));
                    }
                    MGallery.gallery.updateShowMore();
                    $("img.lazy").lazyload();
                    if (MGallery.suggestedGalleryShown)
                        $('.active-container').attr('style', 'min-height:0');
                }
                else {
                    selector.addClass('loaded');
                    var url = element.find('a').attr('href');
                    if (element.data('tabs') == 'tabColourSelection') {
                        $('.titleSpan').text('Colours');
                        $('#mainCarousal').slideUp('slow', function () {
                            MGallery.$tabNavEleTopPos = MGallery.floatingTabNav.calculateTabHeight();
                        });
                    }
                    else {
                        $('#mainCarousal').slideDown('slow', function () {
                            MGallery.$tabNavEleTopPos = MGallery.floatingTabNav.calculateTabHeight();
                        });
                        $('.titleSpan').text('Images');
                    }
                    selector.load(url + (url.indexOf('?category=') > 0 ? "&" : "?") + "isPartial=true", function (response, status, xhr) {
                        if (status == "success") {
                            if (element.data('tabs') == 'tabColourSelection') {
                                MGallery.colorTabSwiper.registerColorSwiper();
                                $('.color-loader-img').hide();
                                MGallery.initTimerForNextImg(null);
                            }
                            else {
                                MGallery.swiper.bindCarousel();
                                if (!showCarousel) {
                                    MGallery.swiper.isRegisterCarousel = true;
                                }
                                MGallery.initTimerForNextImg(MGallery.gallery.getActiveImg().data("image-id"));
                            }
                            MGallery.gallery.updateShowMore();
                            $("img.lazy").lazyload();
                            if (MGallery.suggestedGalleryShown)
                                $('.active-container').attr('style', 'min-height:0');
                        }
                    });                    
                }
                
                window.history.replaceState("image-url", 'image-url', element.find('a').attr('href'));
                window.scrollTo(0, 0);
            });
            $(document).on('click', 'ul.gallery-img-list img[data-index]', function (event) {
                event.preventDefault();
                MGallery.swiper.openFullScreen();
                var index = $(this).data('index');
                if (showCarousel) {
                    MGallery.swiper.carousel.slideTo(index);
                    MGallery.swiper.onImageChange('carmodel-image-swipper', MGallery.swiper.carousel.activeIndex);
                }
                else {
                    setTimeout(function () {
                        MGallery.swiper.carousel.slideTo(index);
                        MGallery.swiper.onImageChange('carmodel-image-swipper', MGallery.swiper.carousel.activeIndex);
                    }, 100);
                }
            });
            $(document).on('click', '.btnDownload', function (event) {
                $(this).attr('href', MGallery.gallery.getActiveImageUrl().replace(/\d+x\d+/, '0x0').split('&q')[0].split('?q')[0]);
            });
            $("#showmore span").text("View More (" + $(".active-container ul.second-list").find("li.hide").length + ")");
            $("#showmore").click(function (event) {
                $(".active-container ul.second-list").find("li.hide:lt(42)").removeClass("hide").filter(":lt(3)").trigger("scroll");
                MGallery.gallery.updateShowMore();
            });
            MGallery.gallery.updateShowMore();
            MGallery.timer = $.now();
        },
        updateShowMore: function () {
            var list = $(".active-container ul.second-list");
            var showmore = $("#showmore");
            var remaining = list.find("li.hide").length;
            if (remaining < 1)
                showmore.hide();
            else
                showmore.show();
            showmore.find("span").text("View More (" + remaining + ")");
        },
        getActiveImg: function () {
            if (showCarousel) {
                return (activeFilter == MGallery.filters.colors) ? $('div.active-container .swiper-slide-active img') : $('#imageGalleryContainer .swiper-slide-active img');
            }
            else {
                return (activeFilter == MGallery.filters.colors) ? $('#tabColourSelection .swiper-slide-active img') : $('#modelImage');
            }
        },
        getActiveImageUrl: function () {
            var element = MGallery.gallery.getActiveImg();
            if (element.attr('data-src'))
                return element.attr('data-src');
            else
                return element.attr('src');
        },
        bindSuggestedGallery: function () {
            $('#userHistoryModelGallery').bind('inview', function (event, visible) {
                if (visible) {
                    var self = $(this);
                    self.load('/m/suggestedmodelgallery?modelId=' + MGallery.modelId + '&count=5', function (response) {
                        if (response.trim().length > 0) {
                            MGallery.suggestedGalleryShown = true;
                            $('.active-container').attr('style', 'min-height:0');
                        }
                        self.removeClass('swiper-imgLoader');
                        self.css('height', 'auto');
                    });
                    self.unbind('inview');
                }
            });
        },
        zoomToggle: function () {
            if (activeFilter != MGallery.filters.colors) {
                if (MGallery.swiper.carousel.zoom.currentScale > 1)
                    MGallery.swiper.carousel.zoom.toggleZoom(MGallery.swiper.carousel);
            }
            else {
                if (MGallery.colorTabSwiper.carColorStage.zoom.currentScale > 1)
                    MGallery.colorTabSwiper.carColorStage.zoom.toggleZoom(MGallery.colorTabSwiper.carColorStage);
            }
        },
        zoomBtnToggle: function () {
            if ((activeFilter != MGallery.filters.colors || (activeFilter == MGallery.filters.colors && !$('#colors-swiper li.swiper-slide-active img').hasClass('no-image'))) && MGallery.swiper.isFullScreen())
                $('.mgallery-zoom').show();
            else
                $('.mgallery-zoom').hide();
        }
    },
    floatingTabNav: {
        tabNavFloat: function () {
            var $tabNavElement = $('.gallery-grid').find('.mgallery-tab');
            MGallery.$tabNavEleTopPos = MGallery.floatingTabNav.calculateTabHeight();
            if (showCarousel) {
                $('.carmodel-image-swipper').find('img').first().load(function () {
                    MGallery.$tabNavEleTopPos = MGallery.floatingTabNav.calculateTabHeight();
                });
            }
            else {
                $('#mainImage').find('img').load(function () {
                    MGallery.$tabNavEleTopPos = MGallery.floatingTabNav.calculateTabHeight();
                });
            }
            var dummyTabsSelector = $('#dummy-cw-tabs');
            dummyTabsSelector.css({ 'height': $tabNavElement.outerHeight() });

            $(window).scroll(function () {
                var $currentScrollPos = $(window).scrollTop();
                if ($currentScrollPos >= MGallery.$tabNavEleTopPos && $currentScrollPos <= $('.imageEnd').position().top) {
                    dummyTabsSelector.show();
                    $tabNavElement
                        .css({
                            'position': 'fixed',
                            'top': '0',
                            'z-index': '99'
                        });
                } else {
                    dummyTabsSelector.hide();
                    $tabNavElement.css('position', 'static');
                }
            });
        },
        calculateTabHeight: function () {
            return $('.mgallery-tab').position().top;
        }
    },
    colorTabSwiper: {
        carColorStage: undefined,
        zoomTracked: true,
        registerColorSwiper: function () {
            MGallery.colorTabSwiper.carColorStage = $('.carColorStage').swiper({
                preloadImages: false,
                lazyLoading: true,
                nextButton: '.swiper-button-next',
                prevButton: '.swiper-button-prev',
                lazyLoadingInPrevNext: true,
                lazyLoadingInPrevNextAmount: 2,
                zoom: true,
                zoomMax: 2.5,
                zoomMin: 1,
                zoomToggle: true,
                onSlideChangeStart: function (swiper) {
                    var activeIndex = swiper.activeIndex;
                    carColorNav.slideTo(activeIndex);
                    MGallery.colorTabSwiper.getColorLabelValue(swiper, activeIndex);

                    var realIndex = carColorNav.slides.eq(activeIndex);
                    var removeSlideClass = carColorNav.slides;

                    MGallery.colorTabSwiper.setActiveColor(realIndex, removeSlideClass);
                },
                onTransitionEnd: function () {
                    var colorsDownloadButton = $('#colors-section-btnDownload');
                    $('#colors-swiper .swiper-slide-active img').attr('src') == noImagePath || MGallery.swiper.isFullScreen() ? colorsDownloadButton.addClass('hide') : colorsDownloadButton.removeClass('hide');
                    $('#tabColourSelection div.mgallery-count span').eq(0).text(MGallery.colorTabSwiper.carColorStage.activeIndex + 1);
                },
                onSlideNextEnd: function () {
                    MGallery.swiper.onSlide("color_image_next", 'image-gallery__colourselection', MGallery.colorTabSwiper.carColorStage.activeIndex);
                },
                onSlidePrevEnd: function () {
                    MGallery.swiper.onSlide("color_image_prev", 'image-gallery__colourselection', MGallery.colorTabSwiper.carColorStage.activeIndex);
                },
                onTouchEnd: function (carousal, event) {
                    MGallery.colorTabSwiper.colorZoomEnd();
                }
            });

            var carColorNav = new $('.carColorNav').swiper({
                pagination: false,
                nextButton: ' .swiper-button-next',
                prevButton: ' .swiper-button-prev',
                passiveListeners: false,
                preventClicksPropagation: false,
                preventClicks: false,
                spaceBetween: 0,
                slidesPerView: "auto",

                onClick: function (swiper, event) { // This block of code will handle the color button click
                    var clickedIndex = swiper.clickedIndex;

                    if (clickedIndex === undefined || clickedIndex === null) {
                        var trackAction;
                        if (MGallery.swiper.isFullScreen()) {
                            trackAction = 'thumb_' + ((event.srcElement.className == carColorNav.nextButton[0].className) ? 'next_fullscreen' : 'prev_fullscreen');
                            Common.utils.trackAction('CWInteractive', 'msite_image_gallery', trackAction, trackAction);
                        }
                        else {
                            trackAction = 'thumb_' + ((event.srcElement.className == carColorNav.nextButton[0].className) ? 'next' : 'prev');
                            Common.utils.trackAction('CWInteractive', 'msite_image_gallery', trackAction, trackAction);
                        }

                        return;
                    }
                    else {
                        MGallery.colorTabSwiper.carColorStage.slideTo(clickedIndex);

                        var realIndex = swiper.slides.eq(clickedIndex);
                        var removeSlideClass = swiper.slides;

                        MGallery.colorTabSwiper.setActiveColor(realIndex, removeSlideClass);
                        if (MGallery.swiper.isFullScreen())
                            Common.utils.trackAction('CWInteractive', 'msite_image_gallery', 'color_thumb_fullscreen', 'color_thumb_fullscreen');
                        else
                            Common.utils.trackAction('CWInteractive', 'msite_image_gallery', 'color_thumb', 'color_thumb');
                    }
                }
            });

            $(document).on('webkitTransitionEnd otransitionend oTransitionEnd msTransitionEnd transitionend', '#tabColourSelection li.swiper-slide-active .swiper-zoom-container', function () {
                MGallery.colorTabSwiper.colorZoomEnd();
            });
        },
        setActiveColor: function (realIndex, removeSlideClass) {
            removeSlideClass.removeClass('colorBtnActive');
            realIndex.addClass('colorBtnActive');
        },
        getColorLabelValue: function (swiper, activeSlide) {
            var $colorSpan = $('.image-gallery__colour-name');
            var labelColor = swiper.slides[activeSlide].dataset.color;
            $colorSpan.text(labelColor);
        },
        colorZoomEnd: function () {
            MGallery.swiper.zoom(false, MGallery.colorTabSwiper.carColorStage, 'image-gallery__colourselection');
            if (MGallery.colorTabSwiper.carColorStage.zoom.currentScale > 1 && MGallery.colorTabSwiper.zoomTracked) {
                Common.utils.trackAction('CWInteractive', 'msite_image_gallery', "zoom_in_colors", "zoom-in_colors");
                MGallery.colorTabSwiper.zoomTracked = false;
            }
        }
    }
}

$(document).ready(function () {
    $("img.lazy").lazyload();
    if ($('#tabColourSelection').hasClass('active-container'))
        MGallery.colorTabSwiper.registerColorSwiper();
    else
        MGallery.swiper.bindCarousel();
    MGallery.swiper.registerEvents();
    MGallery.gallery.registerEvents();
    MoreOptionPopup.registerEvents();
    MGallery.floatingTabNav.tabNavFloat();
    MGallery.gallery.bindSuggestedGallery();

    $('div.tab-container.active-container .loader-img').addClass('hide');
    if (MGallery.modelId > 0)
        $("#expertreviews-div").load("/html-api/expertreview-widget/?modelid=" + MGallery.modelId,
    function(){
        $(this).find("img.lazy").lazyload();
    });
});

var MoreOptionPopup = {
    optionscreen: $(".more-option"),
    blackscreenselector: $(".blackscreen"),
    moreButtonSelector: $(".more-button"),
    registerEvents: function () {
        $(".more-option").click(function () { MoreOptionPopup.open(); });
        MoreOptionPopup.moreButtonSelector.click(MoreOptionPopup.open);
        MoreOptionPopup.blackscreenselector.click(MoreOptionPopup.close);
    },
    open: function () {
        MoreOptionPopup.optionscreen.show();
        MoreOptionPopup.blackscreenselector.show();
    },
    close: function () {
        MoreOptionPopup.blackscreenselector.hide();
        MoreOptionPopup.optionscreen.hide();
    },
}