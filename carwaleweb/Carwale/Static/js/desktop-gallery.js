if ($('.carousel-target--one')) {
    _target = 1
}

var Gallery = {
    initTimerForNextImg: function (nextImageId) {
        if ((Number(Gallery.currentImage) > 0 ? Gallery.currentImage : null) !== nextImageId) {
            var time = $.now() - Gallery.timer;
            var label = "modelid=" + Gallery.modelId + "|timespent=" + time;
            label += "|imageid=" + Gallery.currentImage + "|makename=" + Gallery.makeName + "|modelname=" + Gallery.modelName;
            if (Gallery.currentImage) {
                cwTracking.trackCustomData("Images", "ImageView", label, false);
                console.log(label);
            }
            Gallery.timer = $.now();
            Gallery.currentImage = nextImageId;
        }
    },
    swiper: {
        carouselStage: undefined,
        colourCarouselStage: undefined,
        carouselNavigation: undefined,
        colourCarouselNavigation: undefined,
        carouselStageFullScreen: undefined,
        carouselNavigationFullScreen: undefined,
        isFullScreenCarouselRegistered: false,
        isPageLoad: true,
        previousIndex: 0,
        previousIndexFullscreen: 0,
        bindCarousal: function () {
            var imageContainer = $('#carousel-template ul.swiper-image-container');
            imageContainer.children().remove();
            var iconContainer = $('#carousel-template ul.swiper-icon-container');
            iconContainer.children().remove();
            $('div.cw-tabs-data.active-container ul li[data-image-size]').each(function (index, element) {
                element = $(element);
                var imageSize = element.data('image-size');
                var image = element.find('img');
                var imageArr = image.data('original').split(imageSize);
                var imageSrc = imageArr[0] + galleryImageSize + imageArr[1];
                var iconSrc = imageArr[0] + galleryIconSize + imageArr[1];
                var imageTitle = image.attr('title');
                var imageAlt = image.attr('alt');
                var imageId = image.data('image-id');
                var url = element.find('a').attr('href');
                Gallery.swiper.setImageAttributes($('#image-gallery-template img'), imageSrc, imageTitle, imageAlt, url, imageId);
                Gallery.swiper.setImageAttributes($('#image-icon-template img'), iconSrc, imageTitle, url, imageAlt, imageId);
                imageContainer.append($('#image-gallery-template').html());
                iconContainer.append($('#image-icon-template').html());
            });
            var html = $('#carousel-template').html();
            $('div.gallery-popup-container .gallery__white-div').html(html);
            $('#image-carousel div.grid-12').html(html)
            GalleryAds.desktopBtfLeaderBoard.insertAd();
        },

        setImageAttributes: function (element, src, title, alt, url, imageId) {
            element.attr({ 'data-original': src, 'alt': alt, 'title': title, 'data-url': url, 'data-image-id': imageId.toString() });
        },

        updateImageAttributes: function (index, imageName, isFullScreen, imgUrl) {
            var isColorCarouselActive = $('div.cw-tabs-data.active-container').attr('id') == 'tabColors';
            var selector = $(isColorCarouselActive ? '#color-carousel' : isFullScreen ? 'div.gallery-popup-container' : '#image-carousel');
            selector.find('span.imageCount span').eq(0).text(index);
            if (!isColorCarouselActive)
                selector.find('p.imageName').text(imageName);

            var downloadBtn = selector.find('.downloadBtn');
            if (imgUrl == noImagePath)
                downloadBtn.hide()
            else {
                downloadBtn.show();
                downloadBtn.attr('href', imgUrl.replace(/\d+x\d+/, '0x0').split('&q')[0].split('?q')[0]);
            }
        },

        connector: function (itemNavigation, carouselStage) {
            return carouselStage.jcarousel('items').eq(itemNavigation.index());
        },

        initialize: function (isFullScreen) {
            Gallery.swiper.carouselStage = $('#image-carousel .carousel-stage').jcarousel();
            Gallery.swiper.carouselNavigation = $('#image-carousel .carousel-navigation').jcarousel();
            Gallery.swiper.carouselNavigation.jcarousel('items').each(function () {
                var item = $(this);

                var target = Gallery.swiper.connector(item, Gallery.swiper.carouselStage);

                item
                    .on('#image-carousel jcarouselcontrol:active', function () {
                        Gallery.swiper.carouselNavigation.jcarousel('scrollIntoView', this);
                        Gallery.swiper.carouselNavigation.jcarousel('items').removeClass('active')
                        item.addClass('active');
                        Gallery.swiper.onImageChange(target);
                        Gallery.initTimerForNextImg(item.find("img").data("image-id"));
                        GalleryAds.swipeCounter++;
                        GalleryAds.desktopBtfLeaderBoard.refreshAd();
                    })
                    .jcarouselControl({
                        target: target,
                        carousel: Gallery.swiper.carouselStage
                    });
            });
        },

        initializeColour: function () {
            // second connected carousel code starts here
            Gallery.swiper.colourCarouselStage = $('#tabColors .carousel-stage').jcarousel();
            Gallery.swiper.colourCarouselNavigation = $('#tabColors .carousel-navigation').jcarousel();
            Gallery.swiper.colourCarouselNavigation.jcarousel('items').each(function () {
                var item = $(this);

                var target = Gallery.swiper.connector(item, Gallery.swiper.colourCarouselStage);

                item.on('#tabColors jcarouselcontrol:active', function () {
                    Gallery.swiper.colourCarouselNavigation.jcarousel('scrollIntoView', this);
                    Gallery.swiper.colourCarouselNavigation.jcarousel('items').removeClass('active');
                    item.addClass('active');
                    // change colour name code starts
                    var colourName = item.attr('data-label');
                    $('#colourName').text(colourName);
                    // change colour name code ends
                    Gallery.swiper.onImageChange(target, false, true);
                }).jcarouselControl({
                    target: target,
                    carousel: Gallery.swiper.colourCarouselStage,
                    event: 'mouseover'
                });
            });
        },

        initializeFullScreen: function () {
            Gallery.swiper.carouselStageFullScreen = $('div.gallery-popup-container .carousel-stage').jcarousel();
            Gallery.swiper.carouselNavigationFullScreen = $('div.gallery-popup-container .carousel-navigation').jcarousel();
            Gallery.swiper.carouselNavigationFullScreen.jcarousel('items').each(function () {
                var item = $(this);

                var target = Gallery.swiper.connector(item, Gallery.swiper.carouselStageFullScreen);

                item
                    .on('jcarouselcontrol:active', function () {
                        Gallery.swiper.carouselNavigationFullScreen.jcarousel('scrollIntoView', this);
                        Gallery.swiper.carouselNavigationFullScreen.jcarousel('items').removeClass('active')
                        item.addClass('active');
                        Gallery.swiper.onImageChange(target, true);
                        var imageId = item.find("img").data("image-id");
                        if (Gallery.currentImage === imageId) Gallery.closeFullScreenTrackId = imageId;
                        else Gallery.closeFullScreenTrackId = null;
                        if (Gallery.swiper.isFullScreenCarouselRegistered) {
                            Gallery.initTimerForNextImg(item.find("img").data("image-id"));
                        }
                    })
                    .jcarouselControl({
                        target: target,
                            carousel: Gallery.swiper.carouselStageFullScreen
                    });
            });
        },

        registerEvents: function () {
            Gallery.swiper.registerCarouselEvents('#image-carousel');
            
            Gallery.swiper.carouselStage.on('jcarousel:visiblein', 'li', function (event, carousel) {
                Gallery.swiper.advanceLaod($(this), 2);
            });
            Gallery.swiper.carouselNavigation.on('jcarousel:lastin', 'li', function (event, carousel) {
                Gallery.swiper.advanceLaod($(this), 7);
            });
            
            if (Gallery.swiper.isFullScreenCarouselRegistered) {
                Gallery.swiper.registerCarouselEvents('div.gallery-popup-container');
                Gallery.swiper.carouselNavigationFullScreen.on('jcarousel:lastin', 'li', function (event, carousel) {
                    Gallery.swiper.advanceLaod($(this), 7);
                });
                Gallery.swiper.carouselNavigationFullScreen.on('jcarousel:visiblein', 'li', function (event, carousel) {
                    Gallery.swiper.advanceLaod($(this), 0);
                });
                Gallery.swiper.carouselStageFullScreen.on('jcarousel:visiblein', 'li', function (event, carousel) {
                    Gallery.swiper.advanceLaod($(this), 2);
                });
            }
            $('#image-carousel .icon-gallery-maximize').click(function () {
                var index = Gallery.swiper.getActiveImage().index();
                Gallery.swiper.openFullScreen(index);
            });
            $(document).on('mouseenter', '#colorThumbnail li', function () {
                Common.utils.trackAction('CWInteractive', 'desktop_image_gallery', 'color_thumb', 'color_thumb');
            });
        },

        registerColourEvents: function () {
            Gallery.swiper.registerCarouselEvents('#tabColors');

            Gallery.swiper.colourCarouselStage.on('jcarousel:visiblein', 'li', function (event, carousel) {
                Gallery.swiper.advanceLaod($(this), 2);
            });
            Gallery.swiper.carouselNavigation.on('jcarousel:visiblein', 'li', function (event, carousel) {
                Gallery.swiper.advanceLaod($(this), 0);
            });
        },

        registerCarouselEvents: function (parent) {
            $(parent + ' .gallery-control-prev').on('jcarouselcontrol:active', function () {
                $(this).removeClass('inactive');
            }).on('jcarouselcontrol:inactive', function () {
                $(this).addClass('inactive');
            }).jcarouselControl({
                target: '-=' + _target
            });

            $(parent + ' .gallery-control-next').on('jcarouselcontrol:active', function () {
                $(this).removeClass('inactive');
            }).on('jcarouselcontrol:inactive', function () {
                $(this).addClass('inactive');
            }).jcarouselControl({
                target: '+=' + _target
            });

            $(parent + ' .left-target5').on('jcarouselcontrol:active', function () {
                $(this).removeClass('inactive');
            }).on('jcarouselcontrol:inactive', function () {
                $(this).addClass('inactive');
            }).jcarouselControl({
                    target: '-=5'
            });

            $(parent + ' .right-target5').on('jcarouselcontrol:active', function () {
                $(this).removeClass('inactive');
            }).on('jcarouselcontrol:inactive', function () {
                $(this).addClass('inactive');
            }).jcarouselControl({
                target: '+=5'
            });
        },

        getActiveImage: function (isFullScreen) {
            return $((isFullScreen ? 'div.gallery-popup-container' : '#image-carousel') + ' .swiper-icon-container li.active');
        },

        enableSwipe: function (parent) {
            var carouselSwipe = $(parent + ' .carousel-swipe');


            if (typeof (carouselSwipe) !== 'undefined') {
                $(carouselSwipe).jcarouselSwipe();
            }
        },

        onImageChange: function (element, isFullScreen, isColorsActive) {
            var label;
            var img = element.find('img');
            var image = img;
            var url = img.attr('data-url');
            var imgUrl = img.attr('data-original');
            var index = element.index();

            if (!Gallery.swiper.isPageLoad) {
                window.history.replaceState("image-url", 'image-url', url);
                Common.utils.firePageView(url);
                fireComscorePageView();
                if (isFullScreen) {
                    if (index >= Gallery.swiper.previousIndexFullscreen)
                        label = "image_next";
                    else
                        label = "image_prev";
                    label += "_fullscreen";
                    Gallery.swiper.previousIndexFullscreen = index;
                }
                else {
                    label = isColorsActive ? "color_" : "";
                    if (index >= Gallery.swiper.previousIndex)
                        label += "image_next";
                    else
                        label += "image_prev";
                    Gallery.swiper.previousIndex = index;
                }
                Common.utils.trackAction('CWInteractive', 'desktop_image_gallery', label, label);
            }
            var imageName = image.attr('title');
            var imageId = image.data('image-id');
            var title = Gallery.common.filterImageName(imageName, makeName, modelName, imageId);
            Gallery.swiper.updateImageAttributes(index + 1, title, isFullScreen, imgUrl);
        },

        lazyLoadElement: function (elements, count) {
            elements.each(function (index, element) {
                if (index < count) {
                    element = $(element);
                    element.attr('src', element.attr('data-original'));
                }
            });
        },

        advanceLaod: function (element, count) {
            for (var i = 0; i <= count; i++) {
                var img = element.find('img');
                if (element)
                    img.attr('src', img.attr('data-original'));
                element = element.next();
            }
        },

        lazyLoadAll: function () {
            Gallery.swiper.lazyLoadElement($('ul.swiper-image-container li img'), 3);
            Gallery.swiper.lazyLoadElement($('ul.swiper-icon-container li img'), 15);
            Gallery.swiper.lazyLoadElement($('div.gallery-popup-container ul.swiper-image-container li img'), 3);
            Gallery.swiper.lazyLoadElement($('div.gallery-popup-container ul.swiper-icon-container li img'), 15);
        },

        openFullScreen: function (index) {
            Common.utils.lockPopup();
            $('.gallery-popup-container').show();
            if (!Gallery.swiper.isFullScreenCarouselRegistered) {
                Gallery.swiper.initializeFullScreen();
                Gallery.swiper.isFullScreenCarouselRegistered = true;
                Gallery.swiper.registerEvents();
                Gallery.swiper.enableSwipe('div.gallery-popup-container');
            }
            Gallery.swiper.carouselStageFullScreen.jcarousel('scroll', index.toString());
        },

        closeFullScreen: function () {
            Common.utils.unlockPopup();
            $('.gallery-popup-container').hide();
            Gallery.initTimerForNextImg(Gallery.closeFullScreenTrackId);
        },

        setTotalImageCount: function () {
            $('span.imageCount span:odd').text($('div.cw-tabs-data.active-container ul:first li[data-index]').length);
        },

        refreshCarousel: function () {
            Gallery.swiper.isPageLoad = true;
            Gallery.swiper.isFullScreenCarouselRegistered = false;

            var isColorActive = ($('.cw-tabs-data.active-container').attr('id') == 'tabColors');
            if (isColorActive)
                Gallery.swiper.initializeColour();
            else
                Gallery.swiper.bindCarousal();

            Gallery.swiper.initialize();

            if (isColorActive)
                Gallery.swiper.registerColourEvents();
            else
                Gallery.swiper.registerEvents();

            $("div.active-container img.lazy").lazyload();
            Gallery.swiper.lazyLoadAll();
            Gallery.swiper.enableSwipe('#image-carousel');
            Gallery.swiper.enableSwipe('#tabColors');
            Gallery.swiper.setTotalImageCount();
            $('.gallery-popup-container .icon-gallery-maximize').hide();
            Gallery.swiper.isPageLoad = false;
        }
    },

    images: {
        registerEvents: function () {
            $('li[data-tabs]').click(function (event) {
                event.preventDefault();
                var element = $(this);
                var activeElement = $('div.cw-tabs-data.active-container');
                activeElement.removeClass('active-container');
                var selector = $('#' + element.data('tabs'));
                var url = element.find('a').attr('href');
                selector.addClass('active-container');
                if ($('#showColours').length && (selector.attr('id') !== 'tabColors'))
                    $('#showColours').remove();
                if (selector.hasClass('loaded')) {
                    if (selector.attr('id') === 'tabColors') {
                        window.location.replace = "url";
                        location.reload(true);
                    }
                    else {
                        $('#image-carousel').slideDown(1000);
                        $('#tabColors').hide();
                        $('#spnHeading').text('Images');
                        $('ul.breadcrumb span').last().text('Images');
                    }
                    Gallery.common.updateShowMore(element);
                    Gallery.swiper.refreshCarousel();
                }
                else {
                    selector.addClass('loaded');
                    selector.load(url + (url.indexOf('?category=') > 0 ? "&" : "?") + "isPartial=true", function (response, status, xhr) {
                        if (status == "success") {
                            if (selector.attr('id') == 'tabColors') {
                                window.location.replace = "url";
                                location.reload(true);
                            }
                            else {
                                $('#image-carousel').slideDown(1000);
                                $('#tabColors').hide();
                                $('#spnHeading').text('Images');
                                $('ul.breadcrumb span').last().text('Images');
                            }
                            Gallery.common.updateShowMore(element);
                            Gallery.swiper.refreshCarousel();
                        }
                    });
                }
                window.history.replaceState("image-url", 'image-url', url);
                Common.utils.firePageView(url);
                fireComscorePageView();
            });

            $(document).on("click", ".photos-grid__list-item[data-index]", function () {
                Gallery.swiper.openFullScreen($(this).data('index'));
            });

            $(document).on("click", "#galleryCloseButton", function () {
                Gallery.swiper.closeFullScreen();
            });

            $(document).on("click", "#globalPopupBlackOut", function () {
                Gallery.swiper.closeFullScreen();
            });

            $("#showmore").click(function () {
                $(".active-container li.hide:lt(40)").removeClass('hide').filter(":lt(16)").trigger('scroll');
                Gallery.common.updateShowMore();
            });

            $(document).keydown(function (e) {
                if (e.keyCode === 27) {
                    Gallery.swiper.closeFullScreen();
                }
            });

            $(document).on("click", "#video .jcarousel-control-right", function () {
                $("#video img.lazy:in-viewport").trigger('imgLazyLoad');
            });
            Gallery.common.updateShowMore();
        }
    },

    common: {
        updateShowMore: function (activeLi) {
            if (!activeLi) activeLi = $(".cw-tabs li.active");
            var showmore = $("#showmore");
            if ($(activeLi).data("tabs") == "tabColors") {
                showmore.hide();
                return;
            }
            var remaining = $(".active-container li.hide").length;
            showmore.find("span").text("View More (" + $(".active-container li.hide").length + ")");
            if (remaining < 1)
                showmore.hide();
            else
                showmore.show();
        },
        initializeSubNavigation: function () {
            $('#dropdown-nav .dropdown-menu').dropdown_menu({
                sub_indicators: true,
                drop_shadows: true,
                close_delay: 300
            });
        },

        filterImageName: function (imageName, makeName, modelName, imageId) {

            if (imageName) {
                imageName = imageName.toLowerCase();
                var arr = imageName.split(".jpg");
                if (arr.length > 0) {
                    imageName = arr[0];
                }
                imageName = imageName.split('-').join(' ');
                imageName = imageName.replace(makeName.toLowerCase() + ' ' + modelName.toLowerCase(), "");
                imageName = imageName.replace(imageId.toString(), "").trim();
                imageName = imageName.replace(/\b[a-z]/g, function (letter) {
                    return letter.toUpperCase();
                });
                return imageName;
            }
        }
    }
};

$(document).ready(function () {
    Gallery.common.initializeSubNavigation();
    Gallery.images.registerEvents();
    $('.main-image').parent().remove();
    $('div.image-gallery-carousel-container').removeClass('hide');
    Gallery.swiper.refreshCarousel();
    $("#video img.lazy").lazyload({		
        event: "imgLazyLoad"
    });
    $('#color-carousel ul:first li:first').css({ 'background-image': '' });
    Gallery.timer = $.now();
});

