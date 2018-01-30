

docReady(function () {
    $("body").floatingSocialShare();
    
    $('#back-to-top').click(function (event) {
        $('html, body').stop().animate({ scrollTop: 0 }, 600);
        event.preventDefault();
    });
    $('#image-gallery').on('click', 'li', function () {
        var itemIndex = $(this).index();
        gallery.lock();
        $('.blackOut-window-model').show();
        $('.modelgallery-close-btn, .bike-gallery-popup').removeClass('hide').addClass('show');
        $('.carousel-stage-photos li').find('img.lazy').trigger('imgLazyLoad');
        $('.carousel-navigation-photos li').find('img.lazy').trigger('imgLazyLoad');
        setGalleryImage(itemIndex);
        window.history.pushState('addGallery', "", "");
    });
    $(".modelgallery-close-btn, .blackOut-window-model").click(function () {
        window.history.back();

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
    var $blackModel = $(".blackOut-window-model");
    var $bikegallerypopup = $(".bike-gallery-popup");
    if ($bikegallerypopup.hasClass("show") && e.keyCode == 27) {
        $(".modelgallery-close-btn").click();
    }
    if ($bikegallerypopup.hasClass("show") && e.keyCode == 39) {
        $(".photos-next-stage").click();
    }
    if ($bikegallerypopup.hasClass("show") && e.keyCode == 37) {
        $(".photos-prev-stage").click();
    }
});

(function ($) {
    
    var connector = function (itemNavigation, carouselStage) {
        return carouselStage.jcarousel('items').eq(itemNavigation.index());
    };

    $(function () {
        var carouselStage = $('.carousel-stage-media').jcarousel();
        var carouselNavigation = $('.carousel-navigation-media').jcarousel();

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
        $('.photos-prev-navigation')
            .on('jcarouselcontrol:inactive', function () {
                $(this).addClass('inactive');
            })
            .on('jcarouselcontrol:active', function () {
                $(this).removeClass('inactive');
            })
            .jcarouselControl({
                target: '-=4'
            });

        $('.photos-next-navigation')
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
})(jQuery);

var mainImgIndexA;



var setGalleryImage = function (currentImgIndex) {
    $(".carousel-stage-photos").jcarousel('scroll', currentImgIndex);
    getImageDetails();
};

var getImageDetails = function () {
    imgTotalCount = $(".carousel-stage-photos ul li").length;
    var imgIndexA = $(".carousel-navigation-photos ul li.active");
    var imgIndex = imgIndexA.index() + 1;
    var imgTitle = imgIndexA.find("img").attr("title");
    setImageDetails(imgTitle, imgIndex);
};

var getImageNextIndex = function () {
    var imgIndexA = $(".carousel-navigation-photos ul li.active").next();
    var imgIndex = imgIndexA.index() + 1;
    var imgTitle = imgIndexA.find("img").attr("title");
    setImageDetails(imgTitle, imgIndex);
}

var getImagePrevIndex = function () {
    var imgIndexA = $(".carousel-navigation-photos ul li.active").prev();
    var imgIndex = imgIndexA.index() + 1;
    var imgTitle = imgIndexA.find("img").attr("title");
    setImageDetails(imgTitle, imgIndex);
}

var getImageIndex = function () {
    var imgIndexA = $(".carousel-navigation-photos ul li.active");
    var imgIndex = imgIndexA.index() + 1;
    var imgTitle = imgIndexA.find("img").attr("title");
    setImageDetails(imgTitle, imgIndex);
}

var setImageDetails = function (imgTitle, imgIndex) {
    $(".leftfloatbike-gallery-details").text(imgTitle);
    if (imgIndex > 0) {
        $(".bike-gallery-count").text(imgIndex.toString() + "/" + imgTotalCount.toString());
    }
}


var gallery = {
    lock: function () {
        var htmlElement = $('html'), bodyElement = $('body');
        if ($(document).height() > $(window).height()) {
            var windowScrollTop = htmlElement.scrollTop() ? htmlElement.scrollTop() : bodyElement.scrollTop();
            if (windowScrollTop < 0) {
                windowScrollTop = 0;
            }
            htmlElement.addClass('lock-browser-scroll').css('top', -windowScrollTop);
        }
    },

    unlock: function () {
        var htmlElement = $('html'),
            windowScrollTop = parseInt(htmlElement.css('top'));
        
        htmlElement.removeClass('lock-browser-scroll');
        $('html, body').scrollTop(-windowScrollTop);
    }
};

// light box logic

var lightBoxContainer = $('#lightBoxContainer')
$('.article-content').on('click', '.image__zoom-btn', function () {
    if (!$(this).closest('#divPhotos').length) {
			var image = lightBoxContainer.find('img.lazy');
			var imageSrc = $(this).siblings('img').attr('src');
			var fullWidthImageSrc = imageSrc.replace(/\/[0-9]+x[0-9]+\/+/g, '/0x0/')

			image.attr('data-original', fullWidthImageSrc);
			lightBoxContainer.fadeIn(200);
			image.lazyload();
			popup.lock();
			$(".blackOut-window").hide();
			window.history.pushState('addImage', "", "");
    }
});

$('#lightBoxContainer .light-box__close-icon').on('click', function () {
    window.history.back();
});
$(window).on("popstate", function (e) {
    if ($('.bike-gallery-popup').is(":visible")) {
        gallery.unlock();
        $(".blackOut-window-model").hide();
        $(".modelgallery-close-btn, .bike-gallery-popup").removeClass("show").addClass("hide");
        var galleryThumbIndex = $(".carousel-navigation-photos ul li.active").index();
        $(".article-jcarousel").jcarousel('scroll', galleryThumbIndex);
    }
    if ($('#lightBoxContainer').is(":visible")) {
        var image = lightBoxContainer.find('img');
        lightBoxContainer.hide();
        image.attr('src', '');
        popup.unlock();
    }
});
$(document).keyup(function (e) {
    if ($('#lightBoxContainer').is(":visible")) {
        window.history.back();
    }
    if ($('.bike-gallery-popup').is(":visible")) {
        window.history.back();
    }
});