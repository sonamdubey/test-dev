var ThreeSixtyView = {
    closedDoorRoothPath: cdnHostUrl + '1280x720/cw/360/' + modelDetails.urlMakeName + '/' + modelDetails.modelId + '/closed-door/',
    openDoorRoothPath: cdnHostUrl + '1280x720/cw/360/' + modelDetails.urlMakeName + '/' + modelDetails.modelId + '/open-door/',
    interiorRootPath: cdnHostUrl + '0x0/cw/360/' + modelDetails.urlMakeName + '/' + modelDetails.modelId + '/interior/d/',
    xmlPath: '/api/xml/360/' + modelDetails.modelId + '/',
    closedXMLName: 'closed/?v=' + xmlVersion.closed,
    openXMLName: 'open/?v=' + xmlVersion.open,
    interiorXMLName: 'interior/?v=' + xmlVersion.interior,
    isRotationComplete: false,
    isClosedViewLoaded: false,
    isOpenViewLoaded: false,
    isInteriorLoaded: false,
    isPageLoad: true,
    isViewLoading: false,
    rotator: WR360.ImageRotator.Create('wr360PlayerId'),
    pano: null,
    skin: null,
    states: {
        exterior: 0,
        open: 1,
        interior: 2
    },
    activeState: 0,
    isFullScreen: false,
    startLoadTime: null,

    pageLoad: {
        registerEvents: function () {
            if (ThreeSixtyView.activeState == ThreeSixtyView.states.exterior)
                ThreeSixtyView.plugin.initialiseExterior();
            else if (ThreeSixtyView.activeState == ThreeSixtyView.states.open)
                ThreeSixtyView.plugin.initialiseOpen();
            else
                ThreeSixtyView.plugin.initialiseInterior();

            ThreeSixtyView.bindEvents.windowResize();
            ThreeSixtyView.bindEvents.exterior360Click();
            ThreeSixtyView.bindEvents.open360Click();
            ThreeSixtyView.bindEvents.interior360Click();
            ThreeSixtyView.plugin.removePluginCss();
            ThreeSixtyView.isViewLoading = false;
        }
    },

    plugin: {
        setRotatorSettings: function (xmlName, rootPath) {
            ThreeSixtyView.rotator.licenseFileURL = '/license.lic';
            ThreeSixtyView.rotator.settings.configFileURL = ThreeSixtyView.xmlPath + xmlName;
            ThreeSixtyView.rotator.settings.googleEventTracking = false;
            ThreeSixtyView.rotator.settings.responsiveBaseWidth = window.innerWidth;
            ThreeSixtyView.rotator.settings.responsiveMinHeight = window.innerHeight;
            ThreeSixtyView.rotator.settings.rootPath = rootPath;
            ThreeSixtyView.rotator.settings.progressCallback = function (isFullScreen, percentLoaded, isZoomLoading) {
                ThreeSixtyView.plugin.progressCallback(isFullScreen, percentLoaded, isZoomLoading);
            };
            ThreeSixtyView.rotator.settings.apiReadyCallback = function (api) {
                Common.utils.trackUserTimings('d_360_time', ThreeSixtyView.activeState == ThreeSixtyView.states.exterior ? 'exterior' : 'open', ($.now() - ThreeSixtyView.startLoadTime).toString(), carName);
                ThreeSixtyView.common.toggleLoadingView(false, false);
                ThreeSixtyView.rotator.getAPI().toolbar.rotateOnce(4, function () { ThreeSixtyView.isRotationComplete = true; });
                ThreeSixtyView.common.showPlayerOptions();
                ThreeSixtyView.bindEvents.rotation();
                if (ThreeSixtyView.activeState == ThreeSixtyView.states.exterior)
                    ThreeSixtyView.isClosedViewLoaded = true;
                else
                    ThreeSixtyView.isOpenViewLoaded = true;
            };

            ThreeSixtyView.startLoadTime = $.now();
            ThreeSixtyView.rotator.runImageRotator();
        },

        initialiseExterior: function () {
            ThreeSixtyView.plugin.setRotatorSettings(ThreeSixtyView.closedXMLName, ThreeSixtyView.closedDoorRoothPath);
        },

        initialiseOpen: function () {
            ThreeSixtyView.plugin.setRotatorSettings(ThreeSixtyView.openXMLName, ThreeSixtyView.openDoorRoothPath);
        },

        initialiseInterior: function () {
            ThreeSixtyView.pano = new pano2vrPlayer('pano2vr360Player');
            ThreeSixtyView.pano.En = function () { }
            ThreeSixtyView.pano.uh = false;
            ThreeSixtyView.skin = new pano2vrSkin(ThreeSixtyView.pano);
            ThreeSixtyView.startLoadTime = $.now();
            ThreeSixtyView.pano.readConfigUrl(ThreeSixtyView.xmlPath + ThreeSixtyView.interiorXMLName, ThreeSixtyView.interiorRootPath);
            ThreeSixtyView.bindEvents.lockWindowScroll();
            $('#pano2vr-container').show();
            $('.loading').hide();
            ThreeSixtyView.plugin.handlingForBrowsers();
            ThreeSixtyView.pano.$n = function () { }
            $('.option-buttons').show();
            ThreeSixtyView.common.showPlayerOptions();
        },

        progressCallback: function (isFullScreen, percentLoaded, isZoomLoading) {
            ThreeSixtyView.plugin.removePluginCss();
            if (ThreeSixtyView.isPageLoad) {
                $('.loading').hide();

                if (ThreeSixtyView.activeState == ThreeSixtyView.states.exterior)
                    $('#loadingClosedDoor').show();
                else if (ThreeSixtyView.activeState == ThreeSixtyView.states.open)
                    $('#loadingOpenDoor').show();

                $('#tempLoadingImage').show();
                $('#wr360image_wr360PlayerId').addClass('remove-top-margin');
                ThreeSixtyView.isPageLoad = false;
            }
            $('#loadingProgress').text(percentLoaded + '%');
        },

        removePluginCss: function () {
            $('.view-img-container').css('height', '');
            $('#wr360PlayerId').css('height', '');
            $('#wr360image_wr360PlayerId').css({ 'height': '', 'margin-left': '', 'width': '100%' });
            $('#wr360container_wr360PlayerId').css({ 'width': '100% !important', 'height': '' });
            $('#pano2vr360Player').css({ 'height': '', 'margin-left': '', 'width': '100% !important' });
            if (ThreeSixtyView.isFullScreen) {
                $('#player').attr('style', 'height:' + window.innerHeight + 'px !important');
                $('#pano2vr360Player').css({ 'height': window.innerHeight, 'width': '100%' });
            }
            else
                $('#player').attr('style', 'height: auto px !important');
        },

        handlingForBrowsers: function () {
            if (/UCBrowser|iPad|iPhone|iPod/.test(navigator.userAgent))
                $('.three-sixty-full-screen').hide();
        },

        toggleFullScreen: function () {
                var doc = window.document;
                var docEl = doc.documentElement;

                var requestFullScreen = docEl.requestFullscreen || docEl.mozRequestFullScreen || docEl.webkitRequestFullScreen || docEl.msRequestFullscreen;
                if (!ThreeSixtyView.common.fullscreen() && !ThreeSixtyView.isFullScreen && requestFullScreen) {
                    ThreeSixtyView.isFullScreen = true;
                    $('.three-sixty-full-screen').hide();
                    $('.three-sixty-full-screen-close').show();
                    requestFullScreen.call($('#player')[0]);
                    $('#player').attr('style', 'height: auto px !important');
                    $('#pano2vr360Player div').css({ 'height': window.innerHeight });
                    $('#pano2vr360Player div div').css('height', window.innerHeight);
                    $('#pano2vr360Player div canvas').css('height', window.innerHeight);
                }
                else{
                    ThreeSixtyView.isFullScreen = false;
                    if (document.webkitCancelFullScreen)
                        document.webkitCancelFullScreen();
                    else if (document.mozCancelFullScreen)
                        document.mozCancelFullScreen();
                    else if (document.msExitFullscreen)
                        document.msExitFullscreen();
                    $('.three-sixty-full-screen').show();
                }
        },

        panLeft: function () {
            if (ThreeSixtyView.activeState == ThreeSixtyView.states.interior)
                ThreeSixtyView.pano.changePan(1, 1);
            else {
                ThreeSixtyView.rotator.getAPI().toolbar.startLeftArrowRotate();
                ThreeSixtyView.rotator.getAPI().toolbar.stopArrowRotate();
            }
        },

        panRight: function () {
            if (ThreeSixtyView.activeState == ThreeSixtyView.states.interior)
                ThreeSixtyView.pano.changePan(-1, 1);
            else {
                ThreeSixtyView.rotator.getAPI().toolbar.startRightArrowRotate();
                ThreeSixtyView.rotator.getAPI().toolbar.stopArrowRotate();
            }
        },

        panVertical: function (level) {
            ThreeSixtyView.pano.changeTilt(level, 1);
        },

        zoom: function (direction) {
            ThreeSixtyView.pano.changeFov(direction, 1);
        },

        autoPlay: function () {
            if (ThreeSixtyView.activeState == ThreeSixtyView.states.interior)
                ThreeSixtyView.pano.toggleAutorotate();
            else
                ThreeSixtyView.rotator.getAPI().toolbar.playbackToggle();
        }
    },

    common: {
        toggleLoadingView: function (showLoading, isClosedLoading) {
            if (showLoading) {
                if (!ThreeSixtyView.isFullScreen) {
                    $('#wr360PlayerId').hide();
                }
                
                $('.three-sixty-full-screen, .option-buttons').hide();
                $('#tempLoadingImage').show();
                if (isClosedLoading) {
                    $('#loadingClosedDoor').show();
                    $('#loadingOpenDoor').hide();
                }
                else {
                    $('#loadingClosedDoor').hide();
                    $('#loadingOpenDoor').show();
                    $('#loadingProgress').text('0%');
                }
            }
            else {
                $('#wr360PlayerId').show();
                $('#tempLoadingImage, #loadingClosedDoor').hide();
                $('#loadingOpenDoor').hide();
                if (!ThreeSixtyView.isFullScreen)
                    $('.three-sixty-full-screen').show();
                $('.option-buttons').show();
                   // In case of ios and uc we hide the full screen icon
            }
            ThreeSixtyView.plugin.handlingForBrowsers();
        },

        showPlayerOptions: function () {
            if (ThreeSixtyView.activeState == ThreeSixtyView.states.interior) {
                $('#exterior-player').hide();
                $('#interior-player').show();
            }
            else {
                $('#exterior-player').show();
                $('#interior-player').hide();
            }
        },

        keyDownScrollPrevent: function (event) {
            if ([32, 37, 38, 39, 40].indexOf(event.keyCode) > -1)
                event.preventDefault();
        },

        toggleDataAttr: function (selector) {
            if (ThreeSixtyView.isFullScreen) {
                $(selector).each(function () {
                    var element = $(this);
                    element.attr('data-label', element.data('label') + '_full_screen');
                    element.attr('data-action', element.data('action') + '_full_screen');
                });
            }
            else {
                $(selector).each(function () {
                    var element = $(this);
                    element.attr('data-label', element.data('label').split('_full_screen')[0]);
                    element.attr('data-action', element.data('action').split('_full_screen')[0]);
                });
            }
        },

        fullscreen: function () {
            return (document.fullscreen || document.mozFullScreen || document.webkitIsFullScreen || document.msFullscreenElement);
        }
    },

    bindEvents: {
        rotation: function () {
            $('#wr360image_wr360PlayerId').on('load', function (event) {
                var currentImage = ThreeSixtyView.rotator.getAPI().images.getCurrentImageIndex() + 1;
                if (ThreeSixtyView.isRotationComplete && currentImage > 0 && currentImage % 18 == 0)
                    Common.utils.trackAction('CWInteractive', 'desktop_360_rotation', 'image_' + currentImage, 'image_' + currentImage);
            });

            $('#wr360PlayerId').on('click', function () {
                ThreeSixtyView.isRotationComplete = true;
            });
        },

        windowResize: function () {
            $(window).on('resize', function () {
                if (ThreeSixtyView.common.fullscreen())
                {
                    ThreeSixtyView.isFullScreen = true;
                    $('.player-nav').css({ 'z-index': '2147483647', 'left': '0', 'bottom': '5%' });
                    $('.option-buttons').css({ 'z-index': '2147483647', 'top': '115%' });
                    $('body').addClass('full-screen-three-sixty');
                }
                else {
                    ThreeSixtyView.isFullScreen = false;
                    if (ThreeSixtyView.isViewLoading)
                        $('#wr360PlayerId').hide();
                    $('.option-buttons').css({ 'z-index': '2005', 'top': '' });
                    $('.player-nav').css({ 'z-index': '2005', 'left': '', 'bottom': '1%' });
                    $('body').removeClass('full-screen-three-sixty');
                    $('#wr360image_wr360PlayerId').css({ 'height': '', 'margin-left': '', 'width': '100%' });
                    $('#wr360container_wr360PlayerId').css({ 'height': '', 'margin-left': '', 'width': '100%' });
                }
                ThreeSixtyView.plugin.handlingForBrowsers();
                ThreeSixtyView.plugin.removePluginCss();
            });
            $(document).on("fullscreenchange mozfullscreenchange webkitfullscreenchange msfullscreenchange", function (e) {
                if (ThreeSixtyView.common.fullscreen()) {
                    ThreeSixtyView.isFullScreen = true;
                    $('.three-sixty-full-screen').hide();
                    $('.three-sixty-full-screen-close').show();
                }
                else{
                    ThreeSixtyView.isFullScreen = false;
                    $('.three-sixty-full-screen').show();
                    $('.three-sixty-full-screen-close').hide();
                }
                ThreeSixtyView.common.toggleDataAttr('.option-buttons li');
                ThreeSixtyView.plugin.removePluginCss();
            });
        },

        exterior360Click: function () {
            $('.three-sixty-exterior').on('click', function () {
                ThreeSixtyView.activeState = ThreeSixtyView.states.exterior;
                $(document).unbind('keydown');
                window.history.replaceState(null, null, exteriorPageUrl);
                Common.utils.firePageView(exteriorPageUrl);
                $('.three-sixty-open').removeClass('three-sixty-active');
                $('.three-sixty-interior').removeClass('three-sixty-active');
                $('.three-sixty-exterior').addClass('three-sixty-active');
                $('.option-buttons').hide();
                $('.player-nav').hide();
                $('.option-buttons li').css('color', 'rgb(130, 136, 139)');
                $('#wr360PlayerId').show();
                $('#pano2vr-container').removeClass('showInteriorView');
                $('#pano2vr-container').addClass('hideInteriorView');
                ThreeSixtyView.plugin.removePluginCss();
                if (!ThreeSixtyView.isClosedViewLoaded) {
                    ThreeSixtyView.common.toggleLoadingView(true, true);
                }

                ThreeSixtyView.startLoadTime = $.now();
                if (ThreeSixtyView.isClosedViewLoaded || ThreeSixtyView.isOpenViewLoaded) {
                    ThreeSixtyView.rotator.getAPI().reload(
                        ThreeSixtyView.xmlPath + ThreeSixtyView.closedXMLName,
                        ThreeSixtyView.closedDoorRoothPath, function (api) {
                            Common.utils.trackUserTimings('d_360_time', 'exterior', ($.now() - ThreeSixtyView.startLoadTime).toString(), carName);
                            if (!ThreeSixtyView.isClosedViewLoaded)
                                ThreeSixtyView.common.toggleLoadingView(false, false);
                            ThreeSixtyView.isClosedViewLoaded = true;
                            ThreeSixtyView.plugin.removePluginCss();
                            ThreeSixtyView.common.showPlayerOptions();
                            $('.option-buttons').show();
                        }, 0);
                }
                else
                    ThreeSixtyView.plugin.initialiseExterior();
            });
        },

        open360Click: function () {
            $('.three-sixty-open').on('click', function () {
                ThreeSixtyView.activeState = ThreeSixtyView.states.open;
                $(document).unbind('keydown');
                window.history.replaceState(null, null, openPageUrl);
                Common.utils.firePageView(openPageUrl);
                $('.three-sixty-exterior').removeClass('three-sixty-active');
                $('.three-sixty-interior').removeClass('three-sixty-active');
                $('.three-sixty-open').addClass('three-sixty-active');
                $('.option-buttons').hide();
                $('.player-nav').hide();
                $('.option-buttons li').css('color', 'rgb(130, 136, 139)');
                $('#wr360PlayerId').show();
                $('#pano2vr-container').removeClass('showInteriorView');
                $('#pano2vr-container').addClass('hideInteriorView');
                if (!ThreeSixtyView.isOpenViewLoaded) {
                    ThreeSixtyView.plugin.removePluginCss();
                    ThreeSixtyView.common.toggleLoadingView(true, false);
                }
                if (ThreeSixtyView.isClosedViewLoaded || ThreeSixtyView.isOpenViewLoaded) {
                    ThreeSixtyView.startLoadTime = $.now();
                    ThreeSixtyView.rotator.getAPI().reload(
                        ThreeSixtyView.xmlPath + ThreeSixtyView.openXMLName,
                        ThreeSixtyView.openDoorRoothPath, function (api) {
                            Common.utils.trackUserTimings('d_360_time', 'open', ($.now() - ThreeSixtyView.startLoadTime).toString(), carName);
                            if (!ThreeSixtyView.isOpenViewLoaded) {
                                ThreeSixtyView.common.toggleLoadingView(false, false);
                            }
                            ThreeSixtyView.isOpenViewLoaded = true;
                            $('.option-buttons').show();
                            ThreeSixtyView.plugin.removePluginCss();
                            ThreeSixtyView.common.showPlayerOptions();
                        }, 0);
                }
                else
                    ThreeSixtyView.plugin.initialiseOpen();
            });
        },

        interior360Click: function () {
            $('.three-sixty-interior').on('click', function () {
                ThreeSixtyView.activeState = ThreeSixtyView.states.interior;
                window.history.replaceState(null, null, interiorPageUrl);
                Common.utils.firePageView(interiorPageUrl);
                ThreeSixtyView.bindEvents.lockWindowScroll();
                $('.three-sixty-full-screen, .option-buttons, #exterior-player').hide();
                $('.three-sixty-open').removeClass('three-sixty-active');
                $('.three-sixty-exterior').removeClass('three-sixty-active');
                $('.three-sixty-interior').addClass('three-sixty-active');
                $('.option-buttons li').css('color', 'rgb(255, 255, 255)');
                $('#wr360PlayerId').hide();
                $('#pano2vr-container').addClass('showInteriorView');
                $('#pano2vr-container').removeClass('hideInteriorView');
                if (!ThreeSixtyView.isInteriorLoaded) {
                    ThreeSixtyView.plugin.initialiseInterior();
                    ThreeSixtyView.isInteriorLoaded = true;
                }
                else {
                    if (!ThreeSixtyView.isFullScreen) {
                        $('.three-sixty-full-screen').show();
                    }
                    $('.option-buttons').show();
                    ThreeSixtyView.plugin.handlingForBrowsers();
                    ThreeSixtyView.common.showPlayerOptions();
                }
            });
        },

        lockWindowScroll: function () {
            $(document).unbind('keydown');
            $(document).keydown(function (event) { ThreeSixtyView.common.keyDownScrollPrevent(event) });
        }
    }
};

$(window).load(function () {
    ThreeSixtyView.activeState = (category == 'closed') ? ThreeSixtyView.states.exterior : (category == 'open') ? ThreeSixtyView.states.open : ThreeSixtyView.states.interior;

    ThreeSixtyView.pageLoad.registerEvents();
    
});
