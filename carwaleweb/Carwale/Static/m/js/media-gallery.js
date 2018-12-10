var isAutoPlay = !($.cookie("YoutubeAutoplay") == "false");
var videoGallery = {
    showVideoLBAd: Number($.cookie("_abtest")) < 91
};

var toggleAutoPlay = function () {
    Common.utils.trackAction("CWInteractive", "video_details", isAutoPlay ? "autoplay_off" : "autoplay_on", GALLERY_DATA.modelDetails.MakeName + ' ' + GALLERY_DATA.modelDetails.MaskingName);
    isAutoPlay = !isAutoPlay;
    Common.utils.setEachCookie("YoutubeAutoplay", isAutoPlay);
}

var hideShowLayout = function (activeSection) {
    if (activeSection == 2) {
        $('#header').hide();
        $('.bg-footer').hide();
    }
    else {
        $('#header').show();
        $('.bg-footer').show();
    }
}
function getUserModelHistory() {
    if (isCookieExists('_userModelHistory')) {
        var userHistoryString = $.cookie('_userModelHistory');
        var userHistory = userHistoryString.split('~').join(',');
        return userHistory;
    } else {
        return "";
    }
}
function updateViews(videoId, basicId) {
    $.ajax({
        type: "GET",
        url: "https://www.googleapis.com/youtube/v3/videos?part=statistics&id=" + videoId + "&key=AIzaSyDQH7Jl_wa5N7Dvh4j1wQGlDC8Sa56H-aM",
        async: true,
        dataType: 'json'
    }).done(function (a) {
        $.ajax({
            type: "POST",
            url: "/ajaxpro/Carwale.UI.Editorial.VideoDetails,Carwale.ashx",
            async: true,
            data: '{"basicId":"' + basicId + '","views":"' + a.items[0].statistics.viewCount + '","likes":"' + a.items[0].statistics.likeCount + '"}',
            beforeSend: function (xhr) {
                xhr.setRequestHeader("X-AjaxPro-Method", "UpdateParameters");
            },
            error: function () {
                console.log("could not update views from youtube");
            }
        });
    });
}
function ytiframeReady(videoId, basicId) {
    onYouTubeIframeReady(videoId);
    updateViews(videoId, basicId);
}
function setSrcAttr(node) {
    var img = node.find('img');
    if (img.attr('data-src')) {
        img.attr('src', img.data('src'));
        img.removeAttr('data-src');
    }
}

function lazyLoadImages(node, prevCount, nextCount) {
    var nodeItr = node.prev();
    for (var i = 1; nodeItr && i <= prevCount; i++) {
        setSrcAttr(nodeItr);
        nodeItr = nodeItr.prev();
    }
    nodeItr = node.next();
    for (var i = 1; nodeItr && i <= nextCount; i++) {
        setSrcAttr(nodeItr);
        nodeItr = nodeItr.next();
    }
}

function onVideoIframeReady(videoId) {
    onYouTubeIframeReady(videoId);
    $('.video-prev-btn').hide();

}

function showNavIcons() {
    $('.next-prev-btns').show();
    setTimeout(function () {
        $('.next-prev-btns').hide();
    }, 4000);
}

function autoPlayNextVideo() {
    if (isAutoPlay) {
        var nextVideo = $('.news-video-options.active').next();
        if (nextVideo.length > 0)
            Common.utils.trackAction("CWNonInteractive", "msite_video_details", "next_video", GALLERY_DATA.modelDetails.MakeName + ' ' + GALLERY_DATA.modelDetails.MaskingName);
        goToAdjVideo(nextVideo);
    }
}

function callDfp() {
    try {
        $.dfp({
            dfpID: '1017752',
            enableSingleRequest: false,
            collapseEmptyDivs: true,
            refreshExisting: true
        });
    }
    catch (err) {
        console.log("dfp:", err.message)
    }
}

function goToAdjVideo(activeVideo) {
    if (activeVideo.length > 0) {
        if (videoGallery.showVideoLBAd)
        {
            callDfp();
        }

        var prevVideo = $('.news-video-options.active');
        prevVideo.removeClass('active');
        prevVideo.find('.watching-container').addClass('hideImportant');
        activeVideo.addClass('active');
        activeVideo.find('.watching-container').removeClass('hideImportant');
        player.loadVideoById(activeVideo.attr('data-videoid'));
        $('.model-video-title').text(activeVideo.attr('data-videotitle'));
        $('.video-viewsCount').text(activeVideo.attr('data-views'));
        $('#videoIframe').attr('data-id', activeVideo.attr('data-videoid'));
        handleNavIcons(activeVideo);
        handleTracking(activeVideo);
    }
}

function handleTracking(activeVideo) {
    Common.utils.trackAction("CWInteractive", "gallery-videos", "Video-Clicked-" + activeVideo.attr('data-index'), window.location.href);
    var state = window.history.state ? window.history.state : GLOBAL_STATE;
    state.activeVideoIndex = activeVideo.attr('data-index');
    updateUrl(state, activeVideo.attr('data-videoTitle'), '/m' + activeVideo.attr('data-videoTitleUrl'), false, false);
    window.scroll(0, 1);
}

function handleNavIcons(activeVideo) {
    if (activeVideo) {
        if (activeVideo.next().length <= 0)
            $('.video-next-btn').hide();
        else {
            $('.video-next-btn').show();
            Common.utils.trackAction("CWNonInteractive", "msite_video_details", "next_button_impression", GALLERY_DATA.modelDetails.MakeName + " " + GALLERY_DATA.modelDetails.MaskingName);
        }
        if (activeVideo.prev().length <= 0)
            $('.video-prev-btn').hide();
        else {
            $('.video-prev-btn').show();
            Common.utils.trackAction("CWNonInteractive", "msite_video_details", "prev_button_impression", GALLERY_DATA.modelDetails.MakeName + " " + GALLERY_DATA.modelDetails.MaskingName);
        }
    }
}


handleNavIcons($('.news-video-options.active'));


var srcValue = Common.utils.getValueFromQS('src');

var GLOBAL_STATE = {};

function updateUrl(state, title, url, useBase, push) {
    hideShowLayout(GALLERY_DATA.galleryState.activeSection);
    url = useBase ? baseUrl + url : url;
    $.extend(GLOBAL_STATE, state);
    var pageview = url != window.location.pathname;
    push ? window.history.pushState(state, title, url) : window.history.replaceState(state, title, url);
    if (title) document.title = title;
    if (pageview) {
        Common.utils.firePageView(url);
        fireComscorePageView();
    }
    currentUrl = window.location.href;
}
/*
* @author : Meet Shah
* @email : meet.shah@carwale.com
* @createdOn : 7/12/16
*/
(function (ko, Swiper, $) {
    'use strict';
    var regX = /\/m\/\w+\-cars\/([^\/]*)\//g;

    var imgUrlRegex = /\-?\d+.jpg\??(\w+)?/g;
    if (window.location.pathname.match(regX) != null)
        var baseUrl = window.location.pathname.match(regX)[0];
    else
        var baseUrl = window.location.pathname;

    var hasLandedOnListing = false;
    var photoClicked = false;
    var check = true;
    var prevCount = 1;
    var nextCount = 4;

    window.ACTIONS = {
        SHOW_FILTER_MENU: 1,
        PHOTO_CLICK: 2,
        FILTER_APPLY: 3,
        CLOSE_PHOTO_DETAILS: 4,
        VIDEO_SECTION_CLICK: 5,
        HIDE_FILTER_MENU: 7,
        SHOW_SHARE_MENU: 8,
        HIDE_SHARE_MENU: 9,
        EXPAND_IMAGE: 10,
        CONTRACT_IMAGE: 11,
        SHOW_FILTER_ICON: 12,
        HIDE_FILTER_ICON: 13,
        TOGGLE_FILTER_ICON: 14,
        TOGGLE_ACTION_BAR: 15,
        SHOW_FULLSCREEN_ICON: 16,
        HIDE_FULLSCREEN_ICON: 17,
        SHOW_ACTION_BAR: 18,
        HIDE_ACTION_BAR: 19,
        TOGGLE_PHOTO_HEADING: 20,
        SHOW_PHOTO_HEADING: 21,
        HIDE_PHOTO_HEADING: 22,
        VIDEO_CLICK: 26,
        CLOSE_LISTING: 27,
        CONTACT_DEALER_CLICK: 28,
        SHOW_BLACKOUT: 29,
        HIDE_BLACKOUT: 30,
        BLACKOUT_CLICK: 31,
        GOTO_PHOTO_LISTING: 32,
        DOWNLOAD_IMAGE: 33,
        SHARE_IMAGE: 34,
        UPDATE_SWIPER: 35
    }

    var ACTION_HANDLER = (function () {
        hideShowLayout(GALLERY_DATA.galleryState.activeSection);
        var self = {};
        self.actions = {};

        function executeAction(action, data) {
            if (self.actions[action]) self.actions[action](data);
        }

        function registerAction(action, handler) {

            self.actions[action] = handler;
        }

        return {
            registerAction: registerAction,
            executeAction: executeAction
        }

    })();

    function toggleFullScreen(goFullScreen) {
        var doc = window.document;
        var docEl = doc.documentElement;

        var requestFullScreen = docEl.requestFullscreen || docEl.mozRequestFullScreen || docEl.webkitRequestFullScreen || docEl.msRequestFullscreen;
        var cancelFullScreen = doc.exitFullscreen || doc.mozCancelFullScreen || doc.webkitExitFullscreen || doc.msExitFullscreen || doc.webkitCancelFullScreen;

        if (goFullScreen && requestFullScreen != undefined) {
            requestFullScreen.call(docEl);
        }
        else if (cancelFullScreen != undefined) {
            cancelFullScreen.call(doc);
        }
    }

    function fireInteractiveEvent(category, action) {
        dataLayer.push({ event: "CWInteractive", cat: category, act: action, lab: window.location.href });
    }

    function updateUrl(state, title, url, useBase, push) {
        hideShowLayout(GALLERY_DATA.galleryState.activeSection);
        url = useBase ? baseUrl + url : url;
        $.extend(GLOBAL_STATE, state);
        var pageview = url != window.location.pathname;
        push ? window.history.pushState(state, title, url) : window.history.replaceState(state, title, url);
        if (title) document.title = title;
        if (pageview) {
            Common.utils.firePageView(url);
            fireComscorePageView();
        }
        currentUrl = window.location.href;
    }

    function isIOS() {
        return ((navigator.userAgent.match(/iPhone/i)) || (navigator.userAgent.match(/iPod/i)));
    }

    function resizeHandler() {

        if (window.innerHeight > window.innerWidth) {
            ACTION_HANDLER.executeAction(ACTIONS.SHOW_ACTION_BAR);
            ACTION_HANDLER.executeAction(ACTIONS.SHOW_FULLSCREEN_ICON);
            ACTION_HANDLER.executeAction(ACTIONS.SHOW_PHOTO_HEADING);
            ACTION_HANDLER.executeAction(ACTIONS.SHOW_FILTER_ICON);
            fireInteractiveEvent("gallery-detail", "portrait");
        }
        else {
            ACTION_HANDLER.executeAction(ACTIONS.HIDE_ACTION_BAR);
            ACTION_HANDLER.executeAction(ACTIONS.HIDE_FULLSCREEN_ICON);
            ACTION_HANDLER.executeAction(ACTIONS.HIDE_PHOTO_HEADING);
            ACTION_HANDLER.executeAction(ACTIONS.HIDE_FILTER_ICON);
            fireInteractiveEvent("gallery-detail", "landscape");
            if (isIOS()) setTimeout(function () { ACTION_HANDLER.executeAction(ACTIONS.UPDATE_SWIPER); }, 1000);
        }
    }

    function restoreBackground() {
        $("body").css({ 'background-color': "#fff" });
        $("html").css({ 'background-color': "#fff", 'overflow': "auto" });
        $("news-photo-filter").css({ 'bottom': "20px" });
        window.removeEventListener('resize', resizeHandler, false);
    }

    function showSnackBar() {
        if (window.innerHeight > window.innerWidth) {
            if (Common.utils.storageAvailable("localStorage") && window.localStorage.getItem("landscape_snackbar_shown") == null) {
                $(function () {
                    $(".snackbar-single").animate({ 'margin-bottom': '0px' }, { duration: 2000, queue: false });
                    $(".news-photo-filter").animate({ 'bottom': '68px' }, { duration: 2000, queue: false });
                });
                setTimeout(hideSnackbar, 5000);
                window.localStorage.setItem("landscape_snackbar_shown", "true");
            }
            else {
                $(".snackbar-single").hide();
            }
        }
    }

    function hideSnackbar() {
        $(function () {
            $(".snackbar-single").animate({ 'margin-bottom': '-48px' }, { duration: 1000, queue: false });
            $(".news-photo-filter").animate({ 'bottom': '20px' }, { duration: 1000, queue: false });
        });
    }

    function formatGalleryData(galleryData, filters) {
        var data = {};
        var interiorPhotos = [];
        var exteriorPhotos = [];
        if(galleryData.modelImages != undefined && galleryData.modelImages != null){
            for (var i = 0; i < galleryData.modelImages.length; i++) {
                galleryData.modelImages[i].MainImgCategoryId == 1 ? interiorPhotos.push(galleryData.modelImages[i]) : exteriorPhotos.push(galleryData.modelImages[i]);
            }
        }
        data[filters.allPhotos] = galleryData.modelImages == null ? [] : galleryData.modelImages;
        data[filters.interiorPhotos] = interiorPhotos == null ? [] : interiorPhotos;
        data[filters.exteriorPhotos] = exteriorPhotos == null ? [] : exteriorPhotos;
        data["videos"] = galleryData.modelVideos;
        data["modelDetails"] = galleryData.modelDetails;
        if (galleryData.modelDetails.MakeName != undefined && galleryData.modelDetails.MakeName != null)
            baseUrl = "/m/" + Common.utils.formatSpecial(galleryData.modelDetails.MakeName) + "-cars/" + galleryData.modelDetails.MaskingName + "/";
        else
            baseUrl = "/m/"
        return data;
    }

    function insertAd() {
        if (videoGallery.showVideoLBAd) {
            $('#lbAds').html("<div class=\"adunit padding-bottom10 padding-top10\" data-adunit=\"Carwale_Mobile_Videos_320x50\" data-dimensions=\"320x50\"></div>");
            callDfp();
        }
    }

    ko.components.register("gallery", {
        viewModel: function (params) {
            var gallery = this;
            this.SECTIONS = {
                photosListing: 1,
                photosDetail: 2,
                videosSection: 3
            }

            this.FILTERS = {
                allPhotos: 1,
                interiorPhotos: 2,
                exteriorPhotos: 3
            }

            this.FILTER_NAMES = {
                1: " ",
                2: " Interior ",
                3: " Exterior "
            }

            this.galleryData = formatGalleryData(params.galleryData, this.FILTERS);

            this.state = ko.observable(
                params.galleryData.galleryState
             );
            if (params.galleryData.modelDetails.MakeName != undefined && params.galleryData.modelDetails.MakeName != '' && params.galleryData.modelDetails.MakeName != null)
                this.showFilterIcon = ko.observable(true);
            else
                this.showFilterIcon = ko.observable(false);
            this.showFilterMenu = ko.observable(false);
            this.showBlackout = ko.observable(false);
            this.postAction = function (action, data, isWindowback) {
                ACTION_HANDLER.executeAction(action, data);
                if (isWindowback && photoClicked && params.galleryData.galleryState.activeSection == this.SECTIONS.photosDetail)
                    window.history.back();
            }
            this.applyFilterUrl = function () {
                var state = gallery.state();
                switch (state.activeFilter) {
                    case gallery.FILTERS.interiorPhotos:
                        updateUrl(state, "", "images/?category=interior", true, false);
                        break;
                    case gallery.FILTERS.exteriorPhotos:
                        updateUrl(state, "", "images/?category=exterior", true, false);
                        break;
                    default:
                        updateUrl(state, "", "images/", true, false);
                        break;
                }
            };

            this.highlightedTile = ko.computed(function () {
                if (gallery.state().activeSection == gallery.SECTIONS.photosDetail || gallery.state().activeSection == gallery.SECTIONS.photosListing) {
                    return gallery.state().activeFilter;
                } else if (gallery.state().activeSection == gallery.SECTIONS.videosSection) {
                    return 4;
                }
            }, gallery);


            ACTION_HANDLER.registerAction(ACTIONS.SHOW_FILTER_MENU, function () {
                gallery.showFilterMenu(true);
                gallery.postAction(ACTIONS.SHOW_BLACKOUT);
                fireInteractiveEvent("gallery-fab", "click");
                if (is360Available)
                    track360Impression();
                Common.utils.trackAction("CWInteractive", getTrackingDataCat(), "more_button", gallery.galleryData.modelDetails.MakeName + ' ' + gallery.galleryData.modelDetails.ModelName);
            });

            ACTION_HANDLER.registerAction(ACTIONS.HIDE_FILTER_MENU, function () {
                gallery.showFilterMenu(false);
                gallery.postAction(ACTIONS.HIDE_BLACKOUT);
            });

            ACTION_HANDLER.registerAction(ACTIONS.TOGGLE_FILTER_ICON, function () {
                gallery.showFilterIcon() ? gallery.postAction(ACTIONS.HIDE_FILTER_ICON) : gallery.postAction(ACTIONS.SHOW_FILTER_ICON);

            });

            ACTION_HANDLER.registerAction(ACTIONS.SHOW_FILTER_ICON, function () {
                gallery.showFilterIcon(true);
            });

            ACTION_HANDLER.registerAction(ACTIONS.HIDE_FILTER_ICON, function () {
                gallery.showFilterIcon(false);
            });

            ACTION_HANDLER.registerAction(ACTIONS.SHOW_BLACKOUT, function () {
                gallery.showBlackout(true);
            });

            ACTION_HANDLER.registerAction(ACTIONS.HIDE_BLACKOUT, function () {
                gallery.showBlackout(false);
            });

            ACTION_HANDLER.registerAction(ACTIONS.BLACKOUT_CLICK, function () {
                gallery.postAction(ACTIONS.HIDE_BLACKOUT);
                gallery.postAction(ACTIONS.HIDE_FILTER_MENU);
                gallery.postAction(ACTIONS.HIDE_SHARE_MENU);
            });

            ACTION_HANDLER.registerAction(ACTIONS.FILTER_APPLY, function (data) {
                if (gallery.galleryData[data].length > 0) {
                    fireInteractiveEvent("gallery-filter-options", (gallery.FILTER_NAMES[data] != " " ? gallery.FILTER_NAMES[data].trim() : "All") + "-Clicked");
                    var state = gallery.state();
                    if (state.activeFilter != data) {
                        state.activeFilter = data;
                        state.activeSlideIndex = 0;
                    }

                    state.activeSection = gallery.SECTIONS.photosListing;

                    gallery.state(state);

                    restoreBackground();
                    gallery.applyFilterUrl();
                    document.title = gallery.galleryData.modelDetails.MakeName + ' ' + gallery.galleryData.modelDetails.ModelName + ' ' + "Gallery";
                    gallery.postAction(ACTIONS.HIDE_FILTER_MENU);
                }
            });

            ACTION_HANDLER.registerAction(ACTIONS.PHOTO_CLICK, function (data) {
                fireInteractiveEvent("gallery-listing", "Photo-Clicked-" + data);
                var state = gallery.state();
                state.activeSection = gallery.SECTIONS.photosDetail;
                state.activeSlideIndex = data;
                gallery.state(state);
                gallery.postAction(ACTIONS.HIDE_FILTER_MENU);
                updateUrl(state, null, 'images/' + Editorial.utils.getImageUrl(gallery.galleryData[state.activeFilter][data].ImageName, gallery.galleryData[state.activeFilter][data].ImageId), true, true);
                hasLandedOnListing = false;
                photoClicked = true;
            });


            ACTION_HANDLER.registerAction(ACTIONS.CLOSE_PHOTO_DETAILS, function () {
                fireInteractiveEvent("gallery-detail", "Close-Clicked");
                toggleFullScreen(false);
                if (srcValue == 'nd') {
                    window.close();
                }
                else {
                    if (document.referrer != "" && !photoClicked) {
                        window.history.back();
                    }
                    else {
                        ACTION_HANDLER.executeAction(ACTIONS.GOTO_PHOTO_LISTING);
                    }
                }
            });

            ACTION_HANDLER.registerAction(ACTIONS.GOTO_PHOTO_LISTING, function () {
                fireInteractiveEvent("gallery-detail", "List-Icon-Clicked");
                toggleFullScreen(false);
                gallery.postAction(ACTIONS.HIDE_FILTER_MENU);
                gallery.postAction(ACTIONS.SHOW_FILTER_ICON);
                gallery.state().activeSection = gallery.SECTIONS.photosListing;
                gallery.state().activeSlideIndex = GLOBAL_STATE.activeSlideIndex;
                gallery.state(gallery.state());
                gallery.applyFilterUrl();
                restoreBackground();
                if (photoClicked)
                    window.history.back();
            });

            ACTION_HANDLER.registerAction(ACTIONS.VIDEO_SECTION_CLICK, function () {
                if (gallery.galleryData.videos.length > 0) {
                    fireInteractiveEvent("gallery-filter-options", "Videos-Clicked");
                    toggleFullScreen(false);
                    var state = gallery.state();
                    state.activeSection = gallery.SECTIONS.videosSection;
                    updateUrl(state, gallery.galleryData.videos[state.activeVideoIndex].VideoTitle, '/m' + gallery.galleryData.videos[state.activeVideoIndex].VideoTitleUrl, false, check);
                    if (check)
                        check = false;
                    gallery.state(state);
                    gallery.postAction(ACTIONS.HIDE_FILTER_MENU);
                    restoreBackground();
                    window.scrollTo(0, 1);
                    hasLandedOnListing = false;
                }
            });

            ACTION_HANDLER.registerAction(ACTIONS.CONTACT_DEALER_CLICK, function () {
                toggleFullScreen(false);
                gallery.postAction(ACTIONS.HIDE_FILTER_MENU);
                gallery.postAction(ACTIONS.HIDE_BLACKOUT);
                gallery.postAction(ACTIONS.HIDE_SHARE_MENU);
            });

            if (params.galleryData.galleryState.activeSection == 1) {
                hasLandedOnListing = true;
                $.extend(GLOBAL_STATE, params.galleryData.galleryState);
            }


            if (isIOS()) {
                document.getElementsByTagName('html')[0].className += " iphone";
            }

            window.onscroll = function () {
                ACTION_HANDLER.executeAction(ACTIONS.HIDE_FILTER_MENU);
            }

            window.addEventListener('popstate', function (e) {
                var state = e.state ? e.state : GLOBAL_STATE;
                if (!$.isEmptyObject(state) && (currentUrl != window.location.href)) {
                    currentUrl = window.location.href;
                    check = true;
                    var activeFilter = state.activeFilter ? state.activeFilter : gallery.FILTERS.allPhotos;
                    gallery.postAction(ACTIONS.FILTER_APPLY, activeFilter);
                }
            });

            var track360Impression = function () {
                var trackAction = '';
                switch (gallery.state().activeSection) {
                    case gallery.SECTIONS.photosDetail:
                        trackAction = 'gallery_page_menu_impression';
                        break;
                    case gallery.SECTIONS.photosListing:
                        trackAction = 'images_page_menu_impression';
                        break;
                    default:
                        trackAction = 'videos_page_menu_impression';
                }
                Common.utils.trackAction('CWNonInteractive', 'msite_360_linkages', trackAction, gallery.galleryData.modelDetails.MakeName + ' ' + gallery.galleryData.modelDetails.ModelName);
            }

            var getTrackingDataCat = function () {
                if (gallery.state().activeSection == gallery.SECTIONS.photosListing)
                    return "msite_image_landing";
                else if (gallery.state().activeSection == gallery.SECTIONS.photosDetail)
                    return "msite_image_gallery";
                else
                    return "msite_video_details";
            }
        },
        template: { element: 'gallery-template' }
    });

    ko.components.register("photos-listing", {
        viewModel: function (params) {

            this.photoList = params.photoList;
            this.modelDetails = params.modelDetails;
            this.filterName = params.filterName;
            this.afterRender = function () {
                $("img.lazy").lazyload();
            };

            this.postAction = function (action, data) {
                ACTION_HANDLER.executeAction(action, data);
            }
        },
        template: { element: 'photos-listing-wrapper' }
    });

    ko.components.register("photos-detail", {
        viewModel: function (params) {
            var galleryDetail = this;
            this.photoList = params.photoList;
            this.currentSlideIndex = ko.observable(params.selectedSlideIndex);
            this.modelDetails = params.modelDetails;
            this.filterName = params.filterName;
            this.showPhotoHeading = ko.observable(true);
            this.showArrows = ko.observable(true);
            this.showActionBar = ko.observable(true);
            this.showFullScreenIcon = ko.observable((true && "orientation" in screen));
            this.showSmallScreenIcon = ko.observable(false);
            this.showShareMenu = ko.observable(false);
            this.postAction = function (action, data) {
                ACTION_HANDLER.executeAction(action, data);
            }
            this.afterRender = function () {
                if (!galleryDetail.swiper) {
                    try {
                        galleryDetail.swiper = $('.swiper-container').swiper({
                            nextButton: '.swiper-button-next',
                            prevButton: '.swiper-button-prev',
                            pagination: false,
                            paginationClickable: false,
                            // Disable preloading of all images
                            preloadImages: false,
                            // Enable lazy loading
                            lazyLoading: true,
                            initialSlide: galleryDetail.currentSlideIndex(),
                            onSlideChangeEnd: function (swiper) {
                                galleryDetail.currentSlideIndex(swiper.activeIndex);
                                galleryDetail.postAction(ACTIONS.HIDE_SHARE_MENU);
                                galleryDetail.postAction(ACTIONS.HIDE_FILTER_MENU);
                                var state = window.history.state ? window.history.state : GLOBAL_STATE;
                                state.activeSlideIndex = swiper.activeIndex;
                                updateUrl(state, null, 'images/' + Editorial.utils.getImageUrl(galleryDetail.photoList[swiper.activeIndex].ImageName, galleryDetail.photoList[swiper.activeIndex].ImageId), true, false);
                                lazyLoadImages($('.swiper-slide-active'), prevCount, nextCount);
                            },
                            onInit: function (swiper) {
                                toggleFullScreen(true);
                                showSnackBar();
                            },
                            onClick: function () {
                                if (window.innerHeight < window.innerWidth) {
                                    ACTION_HANDLER.executeAction(ACTIONS.TOGGLE_PHOTO_HEADING);
                                    ACTION_HANDLER.executeAction(ACTIONS.TOGGLE_ACTION_BAR);
                                    ACTION_HANDLER.executeAction(ACTIONS.TOGGLE_FILTER_ICON);
                                    ACTION_HANDLER.executeAction(ACTIONS.HIDE_FULLSCREEN_ICON);
                                }
                                galleryDetail.postAction(ACTIONS.HIDE_SHARE_MENU);
                                galleryDetail.postAction(ACTIONS.HIDE_FILTER_MENU);
                            },
                            onSlidePrevStart: function (swiper) {
                                fireInteractiveEvent("gallery-detail", "Previous-Swiped");
                            },
                            onSlideNextStart: function (swiper) {
                                fireInteractiveEvent("gallery-detail", "Next-Swiped");
                            }

                        });
                    }
                    catch (e) {
                        console.log(e);
                    }
                }
                lazyLoadImages($('.swiper-slide-active'), prevCount, nextCount);
                $("body").css({ 'background-color': "#2a2a2a" });
                $("html").css({ 'background-color': "#2a2a2a", 'overflow': "hidden" })
                if (is360Available)
                    Common.utils.trackAction("CWNonInteractive", "msite_360_linkages", "gallery_page_toolbar_impression", GALLERY_DATA.modelDetails.MakeName + " " + GALLERY_DATA.modelDetails.ModelName);
            };

            ACTION_HANDLER.registerAction(ACTIONS.SHOW_SHARE_MENU, function () {
                fireInteractiveEvent("gallery-detail", "Share-Icon-Clicked");
                galleryDetail.showShareMenu(true);
                var fabIcon = document.getElementsByClassName("news-photo-filter")[0];
                if (fabIcon) { fabIcon.style.zIndex = "-1"; }
                galleryDetail.postAction(ACTIONS.SHOW_BLACKOUT);
            });

            ACTION_HANDLER.registerAction(ACTIONS.HIDE_SHARE_MENU, function () {
                galleryDetail.showShareMenu(false);
                var fabIcon = document.getElementsByClassName("news-photo-filter")[0];
                if (fabIcon) { fabIcon.style.zIndex = "4"; }
                galleryDetail.postAction(ACTIONS.HIDE_BLACKOUT);
            });

            ACTION_HANDLER.registerAction(ACTIONS.TOGGLE_PHOTO_HEADING, function () {
                galleryDetail.showPhotoHeading() ? galleryDetail.postAction(ACTIONS.HIDE_PHOTO_HEADING) : galleryDetail.postAction(ACTIONS.SHOW_PHOTO_HEADING);
            });

            ACTION_HANDLER.registerAction(ACTIONS.SHOW_PHOTO_HEADING, function () {
                galleryDetail.showPhotoHeading(true);
            });

            ACTION_HANDLER.registerAction(ACTIONS.HIDE_PHOTO_HEADING, function () {
                galleryDetail.showPhotoHeading(false);
            });

            ACTION_HANDLER.registerAction(ACTIONS.TOGGLE_ACTION_BAR, function () {
                galleryDetail.showActionBar() ? galleryDetail.postAction(ACTIONS.HIDE_ACTION_BAR) : galleryDetail.postAction(ACTIONS.SHOW_ACTION_BAR);
            });

            ACTION_HANDLER.registerAction(ACTIONS.SHOW_ACTION_BAR, function () {
                galleryDetail.showActionBar(true);
            });

            ACTION_HANDLER.registerAction(ACTIONS.HIDE_ACTION_BAR, function () {
                galleryDetail.showActionBar(false);
            });

            ACTION_HANDLER.registerAction(ACTIONS.SHOW_FULLSCREEN_ICON, function () {
                galleryDetail.showFullScreenIcon(true && "orientation" in screen);
                galleryDetail.showSmallScreenIcon(false);
            });

            ACTION_HANDLER.registerAction(ACTIONS.HIDE_FULLSCREEN_ICON, function () {
                galleryDetail.showFullScreenIcon(false);
                galleryDetail.showSmallScreenIcon(true && "orientation" in screen);
            });

            ACTION_HANDLER.registerAction(ACTIONS.EXPAND_IMAGE, function () {
                fireInteractiveEvent("gallery-detail", "Fullscreen-Icon-Clicked");
                toggleFullScreen(true);
                if ("orientation" in screen && screen.orientation.type == 'portrait-primary') {
                    screen.orientation.unlock();
                    screen.orientation.lock('landscape-primary');
                }
            });

            ACTION_HANDLER.registerAction(ACTIONS.CONTRACT_IMAGE, function () {
                fireInteractiveEvent("gallery-detail", "Smallscreen-Icon-Clicked");
                toggleFullScreen(true);
                if ("orientation" in screen && screen.orientation.type == 'landscape-primary') {
                    screen.orientation.unlock();
                    screen.orientation.lock('portrait-primary');
                }
            });

            ACTION_HANDLER.registerAction(ACTIONS.DOWNLOAD_IMAGE, function () {
                fireInteractiveEvent("gallery-detail", "Download-Icon-Clicked");
            });

            ACTION_HANDLER.registerAction(ACTIONS.SHARE_IMAGE, function (data) {
                fireInteractiveEvent("gallery-detail", "Image-Shared-" + data);
                galleryDetail.postAction(ACTIONS.HIDE_SHARE_MENU);
            });

            ACTION_HANDLER.registerAction(ACTIONS.UPDATE_SWIPER, function () {
                if (galleryDetail.swiper) {
                    galleryDetail.swiper.update(true);
                }
            });

            window.addEventListener('resize', resizeHandler, false);
            resizeHandler();
        },
        template: { element: 'photos-detail-wrapper' }
    });

    ko.components.register("videos-section", {
        viewModel: function (params) {
            var videoSection = this;
            this.videoList = params.videoList;
            this.selectedVideoIndex = ko.observable(params.selectedVideoIndex);
            this.selectedVideo = ko.computed(function () { return this.videoList[this.selectedVideoIndex()] }, this);
            this.showVideo = ko.observable(true);
            this.postAction = function (action, data) {
                ACTION_HANDLER.executeAction(action, data);
            };
            this.isAutoPlay = ko.observable(isAutoPlay);
            this.afterRender = function () { insertAd(); };

            ACTION_HANDLER.registerAction(ACTIONS.VIDEO_CLICK, function (data) {
                fireInteractiveEvent("gallery-videos", "Video-Clicked-" + data);
                videoSection.showVideo(false);
                videoSection.selectedVideoIndex(data);
                videoSection.postAction(ACTIONS.HIDE_FILTER_MENU);
                var state = window.history.state ? window.history.state : GLOBAL_STATE;
                state.activeVideoIndex = data;
                updateUrl(state, videoSection.videoList[data].VideoTitle, '/m' + videoSection.videoList[data].VideoTitleUrl, false, false);
                videoSection.showVideo(true);

                window.scrollTo(0, 1);
            });

            videoSection.similarVideos = ko.observableArray([]);
            videoSection.showSimilarVideos = ko.observable(false);
            videoSection.videoListCount = ko.observable(this.videoList.length);

            videoSection.getSimilarVideos = function () {
                $.ajax({
                    type: 'GET',
                    url: '/api/videos/similar/?applicationid=1&makeid=' + GALLERY_DATA.modelDetails.MakeId + '&modelid=' + GALLERY_DATA.modelDetails.ModelId,
                    success: function (response) {
                        if (response.length > 0) {
                            videoSection.showSimilarVideos(true);
                            videoSection.similarVideos(response);
                        }
                    }
                });
            }

            this.trackRecommendedVideos = function (videoUrl) {
                Common.utils.trackAction("CWInteractive", "contentcons", "Other-Recommended-Video-Click", "https://www.carwale.com/m" + videoUrl);
            }

            videoSection.getSimilarVideos();
        },
        template: { element: 'videos-section-wrapper'}
    });
    ko.applyBindings({}, document.getElementById("root"));

})(ko, Swiper, $);
var currentUrl = window.location.href;