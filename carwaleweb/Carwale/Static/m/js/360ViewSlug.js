ThreeSixtyView = {
    closedDoorRoothPath: cdnHostUrl + (isMsite ? '664x374/cw/360/' : '860x484/cw/360/') + modelDetails.urlMakeName + '/' + modelDetails.modelId + '/closed-door/',
    openDoorRoothPath: cdnHostUrl + (isMsite ? '664x374/cw/360/' : '860x484/cw/360/') + modelDetails.urlMakeName + '/' + modelDetails.modelId + '/open-door/',
    interiorRootPath: cdnHostUrl + '0x0/cw/360/' + modelDetails.urlMakeName + '/' + modelDetails.modelId + '/interior/' + (isMsite ? 'm/' : 'd/'),
    xmlPath: '/api/xml/360/' + modelDetails.modelId + '/',
    closedXMLName: 'closed/?isMsite=' + (isMsite ? 'true&imageCount=18' : 'false&imageCount=36') + '&qualityFactor=50&v=20180104112826',
    openXMLName: 'open/?isMsite=' + (isMsite ? 'true&imageCount=18' : 'false&imageCount=36') + '&qualityFactor=50&v=20180104112826',
    interiorXMLName: 'interior/',
    activeState: 0,
    hasLoaded: false,
    startLoadTime: 0,
    loadPosition: 0,
    rotatePosition: 0,
    isRotationComplete: false,
    isPageLoad: true,
    states: {
        exterior: 0,
        open: 1,
        interior: 2
    },
    rotator: WR360.ImageRotator.Create('wr360PlayerId'),
    registerEvents: function () {
        ThreeSixtyView.activeState = (category == 'Closed') ? ThreeSixtyView.states.exterior : (category == 'Open') ? ThreeSixtyView.states.open : ThreeSixtyView.states.interior;
        ThreeSixtyView.loadPosition = $('.360LoadElement').position().top;
        ThreeSixtyView.rotatePosition = $('#threeSixtySlug').position().top - ($(window).innerHeight() / 2);
        ThreeSixtyView.plugin.orientationChange();

        $(window).scroll(function () {
            if (ThreeSixtyView.loadPosition <= $(window).scrollTop() && !ThreeSixtyView.hasLoaded) {
                if (ThreeSixtyView.activeState == ThreeSixtyView.states.interior)
                    ThreeSixtyView.plugin.initialiseInterior();
                else if (ThreeSixtyView.activeState == ThreeSixtyView.states.exterior)
                    ThreeSixtyView.plugin.initialiseExterior();
                else if(ThreeSixtyView.activeState == ThreeSixtyView.states.open)
                    ThreeSixtyView.plugin.initialiseOpen();
            }
            
            if (ThreeSixtyView.rotatePosition <= $(window).scrollTop() && ThreeSixtyView.hasLoaded && !ThreeSixtyView.isRotationComplete && ThreeSixtyView.activeState != ThreeSixtyView.states.interior)
                ThreeSixtyView.rotator.getAPI().toolbar.rotateOnce(4, function () { ThreeSixtyView.isRotationComplete = true; });
        });
    },
    plugin: {
        setRotatorSettings: function (xmlName, rootPath) {
            ThreeSixtyView.rotator.licenseFileURL = '/license.lic';
            ThreeSixtyView.rotator.settings.configFileURL = ThreeSixtyView.xmlPath + xmlName;
            ThreeSixtyView.rotator.settings.googleEventTracking = false;
            ThreeSixtyView.rotator.settings.responsiveBaseWidth = window.innerWidth;
            ThreeSixtyView.rotator.settings.responsiveMinHeight = window.innerWidth * (9 / 16);
            ThreeSixtyView.rotator.settings.rootPath = rootPath;
            ThreeSixtyView.rotator.settings.progressCallback = function (isFullScreen, percentLoaded, isZoomLoading) {
                ThreeSixtyView.plugin.progressCallback(isFullScreen, percentLoaded, isZoomLoading);
            };
            ThreeSixtyView.rotator.settings.apiReadyCallback = function (api) {
                Common.utils.trackUserTimings((isMsite ? 'm' :'d') + '_360 model_page_time', ThreeSixtyView.activeState == ThreeSixtyView.states.exterior ? 'exterior' : 'open', ($.now() - ThreeSixtyView.startLoadTime).toString(), carName);
                ThreeSixtyView.plugin.rotationEvent();
                $('.loadingView').hide();
                $('#wr360PlayerId').show();
                ThreeSixtyView.hasLoaded = true;
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
            ThreeSixtyView.pano.uh = false;
            ThreeSixtyView.skin = new pano2vrSkin(ThreeSixtyView.pano);
            ThreeSixtyView.startLoadTime = $.now();
            ThreeSixtyView.pano.readConfigUrl(ThreeSixtyView.xmlPath + ThreeSixtyView.interiorXMLName + '?qualityFactor=50&isMsite=true&getHotspots=false&v=20182201120825', ThreeSixtyView.interiorRootPath);
            $('#pano2vr360Player').show();
            ThreeSixtyView.hasLoaded = true;
        },

        progressCallback: function (isFullScreen, percentLoaded, isZoomLoading) {
            if (ThreeSixtyView.isPageLoad) {
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
        rotationEvent: function () {
            $('#wr360image_wr360PlayerId').on('load', function (event) {
                var currentImage = ThreeSixtyView.rotator.getAPI().images.getCurrentImageIndex() + 1;
                if (currentImage > 0 && currentImage % 9 == 0)
                    Common.utils.trackAction('CWInteractive', (isMsite ?'msite' : 'desktop') + '_360_rotation', 'image_' + currentImage, 'image_' + currentImage);
            });
        },
        handlingForBrowsers: function () {

        },
        orientationChange: function () {
            $(window).on('orientationchange', function () {
                $('.mainDiv').removeAttr('style');
                if (window.innerWidth < window.innerHeight)
                    $('.mainDiv').css({ 'height': (window.innerWidth * 9) / 16 + 'px !important' });
                else
                    $('.mainDiv').css({ 'height': window.innerHeight + 'px !important' });
            });
        }
    },
    common: {
        showPlayerOptions: function () {

        }
    }
};
var Hotspot = {
    checkHotspotVisibility: function () { }
};
$(document).ready(function () {
    ThreeSixtyView.registerEvents();
});