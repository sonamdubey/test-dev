var videosPageSize = 40;
var videoiFrame = document.getElementById("video-iframe");
var mainImgIndexA;

function applyLazyLoad() {
    $("img.lazy").lazyload({
        event: "imgLazyLoad",
        effect: "fadeIn"
    });
}

function applyCarousel() {
    var connector = function (itemNavigation, carouselStage) {
        return carouselStage.jcarousel('items').eq(itemNavigation.index());
    };

    var connector2 = function (itemNavigation2, carouselStage2) {
        return carouselStage2.jcarousel('items').eq(itemNavigation2.index());
    };

    $(function () {
        var carouselStage = $('.carousel-stage-media').jcarousel();
        var carouselNavigation = $('.carousel-navigation-media').jcarousel();

        var carouselStage2 = $('.carousel-stage-videos').jcarousel();
        var carouselNavigation2 = $('.carousel-navigation-videos').jcarousel();

        carouselNavigation.jcarousel('items').each(function () {
            var item = $(this);

            var target = connector(item, carouselStage);

            item
                .on('jcarouselcontrol:active', function () {
                    carouselNavigation.jcarousel('scrollIntoView', this);
                    item.addClass('active');
                })
                .on('jcarouselcontrol:inactive', function () {
                    item.removeClass('active');
                })
                .jcarouselControl({
                    target: target,
                    carousel: carouselStage
                });
        });

        carouselNavigation2.jcarousel('items').each(function () {
            var item2 = $(this);
            var target = connector2(item2, carouselStage2);
            item2
				.on('jcarouselcontrol:active', function () {
				    carouselNavigation2.jcarousel('scrollIntoView', this);
				    item2.addClass('active');
				})
				.on('jcarouselcontrol:inactive', function () {
				    item2.removeClass('active');
				})
				.jcarouselControl({
				    target: target,
				    carousel: carouselStage2
				});
        });

        // Setup controls for the stage carousel
        $('.photos-prev-stage')
            .on('jcarouselcontrol:inactive', function () {
                $(this).addClass('inactive');
            })
            .on('jcarouselcontrol:active', function () {
                $(this).removeClass('inactive');
            })
            .jcarouselControl({
                target: '-=1'
            });

        $('.photos-next-stage')
            .on('jcarouselcontrol:inactive', function () {
                $(this).addClass('inactive');
            })
            .on('jcarouselcontrol:active', function () {
                $(this).removeClass('inactive');
            })
            .jcarouselControl({
                target: '+=1'
            });

        // Setup controls for the navigation carousel
        $('.photos-prev-navigation, .videos-prev-navigation')
            .on('jcarouselcontrol:inactive', function () {
                $(this).addClass('inactive');
            })
            .on('jcarouselcontrol:active', function () {
                $(this).removeClass('inactive');
            })
            .jcarouselControl({
                target: '-=4'
            });

        $('.photos-next-navigation, .videos-next-navigation')
            .on('jcarouselcontrol:inactive', function () {
                $(this).addClass('inactive');
            })
            .on('jcarouselcontrol:active', function () {
                $(this).removeClass('inactive');
            })
            .jcarouselControl({
                target: '+=4'
            });
    });
    $(".carousel-stage-photos, .carousel-navigation-photos").on('jcarousel:visiblein', 'li', function (event, carousel) {
        $(this).find("img.lazy").trigger("imgLazyLoad");
    });
    applyLazyLoad();
};

var fallbackModelGallery = function () {
    var self = this;

    self.photoList = ko.observableArray(modelImages || []);
    self.videoList = ko.observableArray([]);

    self.getAllVideos = function () {
        try {
            $.ajax({
                type: 'GET',
                url: '/api/videos/pn/1/ps/' + videosPageSize + '/model/' + ModelId + '/',
                dataType: 'json',
                success: function (response) {
                    if (response) {
                        videosLoaded = true;
                        self.videoList(response.videos);
                        firstVideo();
                    }
                }
            });
        } catch (e) {
            console.warn("Unable to fetch Videos model gallery " + e.message);
        }
    }

    applyCarousel();
};

var fallbackGallery = {
    open: function (imageIndex) {
        $('.blackOut-window-model').show();
        $('.modelgallery-close-btn, .bike-gallery-popup').show();
        $('body').addClass('lock-browser-scroll');
    },

    close: function () {
        if (isModelPage)
        {
            window.location.href = window.location.pathname.split("images/")[0];
        }
        else {
            $('.blackOut-window-model').hide();
            $('.modelgallery-close-btn, .bike-gallery-popup').hide();
            $('body').removeClass('lock-browser-scroll');
        }
    }
}

var setGalleryImage = function (currentImgIndex) {
    $(".carousel-stage-photos").jcarousel('scroll', currentImgIndex);
    getImageDetails();
};

var getImageDetails = function () {
    imgTotalCount = $(".carousel-stage-photos ul li").length;
    var imgIndexA = $(".carousel-navigation-photos ul li.active");
    var imgIndex = imgIndexA.index() + 1;
    var imgTitle = imgIndexA.find("img").attr("alt");
    setImageDetails(imgTitle, imgIndex);
};

var getImageNextIndex = function () {
    var imgIndexA = $(".carousel-navigation-photos ul li.active").next();
    var imgIndex = imgIndexA.index() + 1;
    var imgTitle = imgIndexA.find("img").attr("alt");
    setImageDetails(imgTitle, imgIndex);
}

var getImagePrevIndex = function () {
    var imgIndexA = $(".carousel-navigation-photos ul li.active").prev();
    var imgIndex = imgIndexA.index() + 1;
    var imgTitle = imgIndexA.find("img").attr("alt");
    setImageDetails(imgTitle, imgIndex);
}

var getImageIndex = function () {
    var imgIndexA = $(".carousel-navigation-photos ul li.active");
    var imgIndex = imgIndexA.index() + 1;
    var imgTitle = imgIndexA.find("img").attr("alt");
    setImageDetails(imgTitle, imgIndex);
}

var setImageDetails = function (imgTitle, imgIndex) {
    $(".bike-gallery-title").text(imgTitle);
    if (imgIndex > 0) {
        $(".bike-gallery-count").text(imgIndex.toString() + "/" + imgTotalCount.toString());
    }
}

var firstVideo = function () {
    var a = $(".carousel-navigation-videos ul").first("li");
    var newSrc = a.find("img").attr("iframe-data");
    videoiFrame.setAttribute("src", newSrc);
};


docReady(function () {

    var vmFallbackGallery = new fallbackModelGallery();
    ko.applyBindings(vmFallbackGallery, document.getElementById('fallback-model-gallery'));


    $(".carousel-stage ul li").click(function () {
        mainImgIndexA = $(".carousel-navigation ul li.active").index();
        setGalleryImage(mainImgIndexA);
    });


    $(".photos-next-stage").click(function () {
        getImageNextIndex();
    });

    $(".photos-prev-stage").click(function () {
        getImagePrevIndex();
    });

    $(".carousel-navigation-photos").click(function () {
        getImageIndex();
    });

    $(".modelgallery-close-btn").click(function () {
        fallbackGallery.close();
        if (videoiFrame != undefined) {
            videoiFrame.setAttribute("src", "");
        }
    });

    $('#videos-tab').click(function () {
        vmFallbackGallery.getAllVideos();
    });

    $(".carousel-navigation-videos ul").on('click', 'li', function () {
        $(this).siblings().removeClass("active");
        $(this).addClass("active");
        var newSrc = $(this).find("img").attr("iframe-data");
        videoiFrame.setAttribute("src", newSrc);
    });

    $(document).on("keydown", function (event) {
        var blackModel = $(".blackOut-window-model");
        var bikegallerypopup = $(".bike-gallery-popup");
        if (bikegallerypopup.is(":visible")) {
            if (event.keyCode === 27) {
                $(".modelgallery-close-btn").click();
            }
            if (event.keyCode === 39 && $("#photos-tab").hasClass("active")) {
                $(".photos-next-stage").click();
            }
            if (event.keyCode === 37 && $("#photos-tab").hasClass("active")) {
                $(".photos-prev-stage").click();
            }
        }
    });


});