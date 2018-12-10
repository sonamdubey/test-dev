var ImagePopup = {
    
    ImagePopupDocReady: function () {
        ImagePopup.setSelectors();
        ImagePopup.registerEvent();
    },
    setSelectors: function () {
        blackOutWindow = '.blackOut-window';
        popupBoxContainer = '.image-popup-outer-container';
        totalNumber = '.total-number';
        activeNumber = '.active-number';
    },

    registerEvent: function () {
        $('.view-images__link').on('click', function () {
            ImagePopup.showImagePopup();
            ImagePopup.swipeFunc();
            
        });
        $(blackOutWindow).on('click', function () {
            ImagePopup.hideImagePopup();
        });      
        
    },

    showImagePopup: function () {       
        $(popupBoxContainer).addClass('popup--active');
        $(blackOutWindow).show();
        scrollLockFunc.lockScroll();
        ImagePopup.imageSwiper();
        ImagePopup.activeImageCount();
        ImagePopup.totalImageCount();
    },
    hideImagePopup: function () {
        $(blackOutWindow).hide();
        $(popupBoxContainer).removeClass('popup--active');
        setTimeout(function () {
            scrollLockFunc.unlockScroll();
            $(totalNumber).empty();
        },100)   
    },
    activeImageCount: function () {
        $(activeNumber).empty();
        var activeImageNumber = $('.image-popup-carousel .swiper-slide-active').index() + 1;
        $(activeNumber).append(activeImageNumber);
    },
    totalImageCount: function () {
        var totalImageNumber = $('.image-popup-carousel .swiper-slide').length;
        $(totalNumber).append(totalImageNumber);
    },

    swipeFunc: function () {
        var x = document.getElementsByClassName("image-popup-slide-on")[0];
        x.addEventListener('swiped-down', function (e) {
            ImagePopup.hideImagePopup();
        });
    },

    slideChangeStart: function () {
        ImagePopup.activeImageCount();
    },

    imageSwiper: function () {
        $('.image-popup-carousel').each(function (index, element) {
            var currentSwiper = $(this);
            currentSwiper.addClass('sw-' + index).swiper({
                centeredSlides: true,
                onSlideChangeStart: ImagePopup.slideChangeStart,
                observer: true,
                observeParents: true,
                onInit: ImagePopup.initSwiper,
                slidesPerView: 'auto',
                cssWidthAndHeight: true,
                loop: false,
            });
        });
    },
    initSwiper: function (swiper) {
        $(window).resize(function () { swiper.update(true); })
    },
}

$(document).ready(function () {
    ImagePopup.ImagePopupDocReady();
});