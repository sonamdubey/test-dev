var Common = {
    utils: {
        formatSpecial: function (url) {
            reg = /[^/\-0-9a-zA-Z\s]*/g;
            url = url.replace(reg, '');
            var formattedUrl = url.toLowerCase().replace(/ /g, "").replace(/-/g, "").replace("/", "");
            return formattedUrl;
        },

        trackAction: function (actionEvent, actionCat, actionAct, actionLabel) {
            var pushObject;
            if (actionLabel != undefined && actionLabel.length > 0)
                pushObject = { event: actionEvent, cat: actionCat, act: actionAct, lab: actionLabel };
            else
                pushObject = { event: actionEvent, cat: actionCat, act: actionAct };
            dataLayer.push(pushObject);
        }
    },

    isReferrerCarWale: function () {
        return (document.referrer != '' && document.referrer.indexOf('carwale.com') != -1);
    },

    Tracking: {
        eventTracking: function () {
            $(document).on("click", "[data-role='click-tracking']", function () {
                Common.Tracking.callTracking($(this));
            });
        },

        trackImpressions: function () {
            $("[data-role*='impression']").each(function (count, element) {
                var action = '_shown';
                Common.Tracking.callTracking($(this), action);
            });
        },

        callTracking: function (node, action) {
            if (action == undefined) action = '';
            try {
                var evCat = node.data('cat') ? node.data('cat') : '',
                evAct = node.data('action') ? node.data('action') + action : '',
                    evLab = node.data('label') ? node.data('label') : '',
                    evEvent = node.data('event') ? node.data('event') : '';
                Common.utils.trackAction(evEvent, evCat, evAct, evLab);
            } catch (e) {
                console.log(e);
            }
        },

        socialShare: function (label) {
            Common.utils.trackAction('CWInteractive', 'video_details_page', 'social_media_share', label + '-' + window.location.href);
        },

        firePageView: function (url) {
            try {
                ga('create', 'UA-337359-1', 'auto', { 'useAmpClientId': true });
                ga('send', 'pageview', url);
            }
            catch (e) {
                console.log(e);
            }
        }
    }
}

var isAutoPlayed = false;

function autoPlayNextVideo() {
    mediaGallery.gotoNextVideo();
    if (isAutoPlayed)
        Common.utils.trackAction('CWNonInteractive', 'desktop_video_details', 'next_video', modelInfo.makeName + ' ' + modelInfo.modelName);
    isAutoPlayed = false;
};

var mediaGallery = (function (ko) {
    //Scope variables 

    var isIE = !!navigator.userAgent.match(/Trident/g) || !!navigator.userAgent.match(/MSIE/g) || !!navigator.userAgent.match(/Edge/g);
    var HISTORY_SUPPORTED = typeof (window.history.replaceState) == "function";
    var isPageLoad = true;
    var ACTIONS = {
        SHOW_GALLERY: 1,
        CLOSE_GALLERY: 2,
        GOTO_VIDEO_INDEX: 3,
        GOTO_PREV_VIDEO: 4,
        GOTO_NEXT_VIDEO: 5,
        TOGGLE_SHARE: 6,
    }

    //Public functions

    function showVideoGallery(data) {
        data.section = 2; // Video gallery
        ACTION_HANDLER.executeAction(ACTIONS.SHOW_GALLERY, data);
    }
    function closeGallery(source) {ACTION_HANDLER.executeAction(ACTIONS.CLOSE_GALLERY,source);}

    function gotoVideoIndex(index) { ACTION_HANDLER.executeAction(ACTIONS.GOTO_VIDEO_INDEX, index); }

    function gotoNextVideo() { ACTION_HANDLER.executeAction(ACTIONS.GOTO_NEXT_VIDEO); }

    function gotoPrevVideo() { ACTION_HANDLER.executeAction(ACTIONS.GOTO_PREV_VIDEO); }

    function toggleShare() { ACTION_HANDLER.executeAction(ACTIONS.TOGGLE_SHARE); }

    //Private functions

    var ACTION_HANDLER = (function () {
        var self = this;
        self.actions = {};

        function executeAction(action, data) {if (self.actions[action]) self.actions[action](data);}

        function registerAction(action, handler) { self.actions[action] = handler; }

        return {
            registerAction: registerAction,
            executeAction: executeAction
        }
    })();

    function formatGalleryData(modelVideos) {
        var data = {};
        data["videos"] = modelVideos;
        return data;
    }

    function UpdateViews(videoId, basicId) {
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

    function keyListener(e){
        if (e.keyCode == 27)
            closeGallery("EscKey");
    }

    ko.components.register("media-gallery", {
        viewModel: function () {
            var self = this;

            self.defaultState = function(){
                return {
                    activeVideoIndex: ko.observable(0)
                }
            }

            self.isLoading = ko.observable(true);
            self.carData = ko.observable();
            self.galleryData = [];
            self.state = self.defaultState();
            
            ko.computed(function () {
                if (self.carData() != undefined) {
                    var url;

                    var activeVideo = self.galleryData["videos"][self.state.activeVideoIndex()];
                    var url = activeVideo.VideoTitleUrl;

                    if (url) {
                        if (HISTORY_SUPPORTED && !isPageLoad) window.history.replaceState(null, "", url);
                        isPageLoad = false;
                        Common.Tracking.firePageView(url);
                        UpdateViews(activeVideo.VideoId, activeVideo.BasicId);
                    }
                }
            }, self);

            ACTION_HANDLER.registerAction(ACTIONS.SHOW_GALLERY, function (data) {
                self.PAGE_URL = window.location.href;
                if (data.section == 2)
                    self.state.activeVideoIndex(data.activeVideo);
                self.isLoading(true);
                self.galleryData = formatGalleryData(videosData);
                self.baseUrl = '/' + Common.utils.formatSpecial(data.makeName) + '-cars/' + data.modelMaskingName + '/';
                self.carData(data);
                self.isLoading(false);

                window.addEventListener("keydown", keyListener);
            });

            ACTION_HANDLER.registerAction(ACTIONS.CLOSE_GALLERY, function (source) {
                if (Common.isReferrerCarWale() && window.history != undefined && window.history.length > 1) {
                    window.history.back();
                }
                else
                    window.location.href = modelPageUrl;
                Common.utils.trackAction('CWInteractive', 'desktop_video_details', 'close', modelInfo.makeName + ' ' + modelInfo.modelName);
            });
        },
        template: { element: "media-gallery--wrapper" }
    });

    ko.components.register("media-gallery--loader", {
        viewModel: function () {
            var self = this;
        },
        template: { element: "media-gallery--loader-wrapper" }
    });

    ko.components.register("media-gallery--detail-frame", {
        viewModel: function (params) {
            var self = this;

            self.carData = params.carData;
            self.galleryData = params.galleryData;
            self.state = params.state;            
            self.shareIconsVisible = ko.observable(false);
            self.videoTitle = ko.observable();
            
            ACTION_HANDLER.registerAction(ACTIONS.TOGGLE_SHARE, function () {
                self.shareIconsVisible(!self.shareIconsVisible());
                if (!self.shareIconsVisible())
                Common.utils.trackAction('CWInteractive', 'desktop_video_details', 'share', modelInfo.makeName + ' ' + modelInfo.modelName);
            });

            ko.computed(function () {
                if (self.state.activeVideoIndex()) self.shareIconsVisible(false);  //To hide share list on change in focussed content
                self.videoTitle(self.galleryData.videos[self.state.activeVideoIndex()].VideoTitle);
            }, self);
        },
        template: { element: "media-gallery--detail-frame-wrapper" }
    });

    ko.components.register("media-gallery--detail-videos", {

        viewModel: function (params) {
            var self = this;

            self.videos = params.videos;
            self.activeVideoIndex = params.activeVideoIndex;
            self.showVideo = ko.observable(true); // flag used to re-render iframe to prevent history maipulation by iframe

            self.bindClickTracking = function () {
                Common.Tracking.eventTracking();
                Common.Tracking.trackImpressions();
            };

            ACTION_HANDLER.registerAction(ACTIONS.GOTO_VIDEO_INDEX, function (data) {
                self.showVideo(false)
                self.activeVideoIndex(parseInt(data));
                self.showVideo(true);
                if (self.activeVideoIndex() == self.videos.length - 1)
                    $('.gallery-control-next').hide();
                else
                {
                    $('.gallery-control-next').show();
                    Common.utils.trackAction('CWNonInteractive', 'desktop_video_details', 'next_button_impression_shown', modelInfo.makeName + ' ' + modelInfo.modelName);
                }
                if (self.activeVideoIndex() == 0)
                    $('.gallery-control-prev').hide();
                else
                {
                    $('.gallery-control-prev').show();
                    Common.utils.trackAction('CWNonInteractive', 'desktop_video_details', 'prev_button_impression_shown', modelInfo.makeName + ' ' + modelInfo.modelName);
                }
                Common.utils.trackAction('CWInteractive', 'desktop_video_details', 'video_card', modelInfo.makeName + ' ' + modelInfo.modelName);
            });

            ACTION_HANDLER.registerAction(ACTIONS.GOTO_PREV_VIDEO, function () {
                var currentIndex = self.activeVideoIndex();
                if (currentIndex != 0)
                {
                    self.showVideo(false)
                    self.activeVideoIndex(currentIndex - 1);
                    self.showVideo(true);
                    if (self.activeVideoIndex() == 0)
                        $('.gallery-control-prev').hide();
                    else
                        Common.utils.trackAction('CWNonInteractive', 'desktop_video_details', 'prev_button_impression_shown', modelInfo.makeName + ' ' + modelInfo.modelName);
                    $('.gallery-control-next').show();
                    Common.utils.trackAction('CWNonInteractive', 'desktop_video_details', 'next_button_impression_shown', modelInfo.makeName + ' ' + modelInfo.modelName);
                }
            });

            ACTION_HANDLER.registerAction(ACTIONS.GOTO_NEXT_VIDEO, function () {
                var currentIndex = self.activeVideoIndex();
                if (currentIndex <= self.videos.length - 2) {
                    self.showVideo(false)
                    self.activeVideoIndex(currentIndex + 1);
                    self.showVideo(true);
                    if (self.activeVideoIndex() == self.videos.length - 1)
                        $('.gallery-control-next').hide();
                    else
                        Common.utils.trackAction('CWNonInteractive', 'desktop_video_details', 'next_button_impression_shown', modelInfo.makeName + ' ' + modelInfo.modelName);
                    $('.gallery-control-prev').show();
                    Common.utils.trackAction('CWNonInteractive', 'desktop_video_details', 'prev_button_impression_shown', modelInfo.makeName + ' ' + modelInfo.modelName);
                    isAutoPlayed = true;
                }
            });            
        },
        template: {
            element: "media-gallery--detail-videos-wrapper",
        }
    });

    ko.applyBindings({}, document.getElementById("media-gallery--root"));

    return {
        showVideoGallery: showVideoGallery,
        closeGallery: closeGallery,
        gotoVideoIndex: gotoVideoIndex,
        gotoNextVideo: gotoNextVideo,
        gotoPrevVideo: gotoPrevVideo,
        toggleShare: toggleShare
    }
})(ko, $);