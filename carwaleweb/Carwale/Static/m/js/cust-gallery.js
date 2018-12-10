function permanentCookieTime() {
    var now = new Date();
    var time = now.getTime();
    time += 1000 * 60 * 60 * 4320;
    now.setTime(time);
    return (now.toGMTString());
}

//CW Track Code
var url = unescape(window.location);
var landingURL = url;
var imgCreation = new Image();
var hashIndex = url.indexOf("#");

url = url.substr(url.indexOf("?") + 1, hashIndex == -1 ? url.length : url.indexOf("#") - (url.indexOf("?") + 1));
landingURL = landingURL.substr(0, landingURL.indexOf("?"));

var searchAttributes = url.split('&');
for (var no = 0; no < searchAttributes.length; no++) {
    var cutSrc = searchAttributes[no].substr(searchAttributes[no].indexOf("ltsrc"), searchAttributes[no].indexOf("="))
    if (cutSrc == 'ltsrc') {
        var qryString = searchAttributes[no].substr(searchAttributes[no].indexOf("ltsrc") + 6, searchAttributes[no].length)
        imgCreation.src = "/lts/ts.aspx?c=" + qryString + "&refUrl=" + landingURL;
    }
}
//End of CW Track code

//gallery script
var videoCarouselContainer = $("#videoCarouselContainer");
var imgCarouselContainer = $("#imgCarouselContainer");
var imageCarouselHtml = imgCarouselContainer.html();
var videoCarouselHtml = videoCarouselContainer.html();
var viewModel;
var currentImgIndex = 0;
var galleryTabId = 1; //exterior default tab
var panelId = 'exterior';
var modelId = Common.utils.getValueFromQS("modelId"), carName = 'hyundai' + '-' + 'i10';
var cityId = '1';
var dealerId = '1';
var isImgClicked = false, isWindowResize = false;
var PhotoCategory = {
    INTERIOR: 1,
    EXTERIOR: 2,
    VIDEO: 3
};

function applyLazyLoad() {
    $("img.lazy").lazyload({
        event: "imgLazyLoad"
    });
}

function getModelPhotos(modelId) {
    return $.ajax({
        type: 'GET',
        url: '/webapi/CarModeldata/Gallery/?applicationid=1&modelid=' + modelId + '&categoryidlist=8,10',
        dataType: 'json',
        success: function (json) {
            
                
            galleryDataObject = json;
            var defaultObj = { "ImageId": 0, "HostUrl": "https://imgd.aeplcdn.com/", "ImagePathThumbnail": "160x89/adgallery/no-img-big.png", "ImagePathLarge": "600x337/adgallery/no-img-big.png", "OriginalImgPath": "/adgallery/no-img-big.png", "MainImgCategoryId": 2, "ImageCategory": "Driving", "Caption": "", "ImageName": "no-img-big.png", "AltImageName": "", "ImageTitle": "", "ImageDescription": "", "MakeBase": { "makeId": 0, "makeName": "" }, "ModelBase": { "ModelId": 0, "ModelName": "", "MaskingName": "" } }

                //if no images and videos available redirect to model page
            if (galleryDataObject.modelImages.length <= 0 && galleryDataObject.modelVideos <= 0) {
                galleryDataObject.modelImages[0] = defaultObj;
            }
            else {
                $("#main-container .gallery-heading").text(json.modelImages.length > 0 ? json.modelImages[0].MakeBase.makeName + " " + Common.utils.filterModelName(json.modelImages[0].ModelBase.ModelName) : json.modelVideos[0].MakeName + " " + Common.utils.filterModelName(json.modelVideos[0].ModelName));
            }
                filterImageGallery(galleryDataObject.modelImages);
            
                    viewModel = new AppViewModel();
                    ko.applyBindings(viewModel, document.getElementById("videoList"));
                    ko.cleanNode(document.getElementById('imgList'));
                    ko.applyBindings(viewModel, document.getElementById('imgList'));
                    ko.applyBindings(viewModel, document.getElementById('imgGalleryCarousel'));

                    //bind carousel
                    $('div.m-carousel').carousel();

                    videoCarouselContainer.html("");
                    applyLazyLoad();
                
            }
        
    });
}

function redirectToModelPage() {
    var currentUrl = location.href;
    var lastIndex = currentUrl.indexOf("/photo");
    var modelPageUrl = currentUrl.substring(0, lastIndex + 1);
    window.location.href = modelPageUrl;
}

// callActionBox hide and show
function callActionBox() {
    var imgHeight = $('.m-active img').height();
    var imgWidth = $('.m-active img').width();
    var winHeight = $(window).height();
    var winWidth = $(window).width();

    if (winHeight > winWidth && winWidth > 250) {
        $(".call-action-box").show();
        dataLayer.push({ event: 'Photo-Gallery-Mobile', cat: 'Photo-Gallery-Mobile', act: 'Call-Button-Shown', lab: carName });
    } else {
        $(".call-action-box").hide();
    }
}

var lazyLoadViewport = function () {
    $("img.lazy:in-viewport").trigger("imgLazyLoad");
};

function filterImageGallery(modelImages) {
    exteriorPhotos = new Array();
    interiorPhotos = new Array();

    if (modelImages.length != 0) {
        $.each(modelImages, function (key, value) {
            value.MainImgCategoryId == PhotoCategory.INTERIOR ? interiorPhotos.push(value) : exteriorPhotos.push(value);
        });
    }
}

//defined view model for gallery
function AppViewModel() {
    var self = this;
    self.filteredGallery = galleryTabId == 1 ? ko.observableArray(exteriorPhotos) : ko.observableArray(interiorPhotos);
    self.videos = ko.observableArray(galleryDataObject.modelVideos);
}

//after window resize set carousel height & width
function resizeCarousel() {
    var width, height, sliderCss, pageCss;
    width = window.innerWidth ? window.innerWidth : $(window).width();
    height = window.innerHeight ? window.innerHeight : $(window).height();
    sliderCss = {
        width: width,
        height: height
    };
    $(".m-uc-gallery").css(sliderCss);
}

function bindAfterSlide() {
    $('div.m-carousel').on('afterSlide', function (event, prevIndex, currentIndex) {
        currentImgIndex = currentIndex;

        if (panelId != "videos") {
            //lazy load
            var carouselSelector = "imgCarouselContainer";
            var currentItem = $("#" + carouselSelector + " img")[currentIndex - 1];
            $(currentItem).trigger("imgLazyLoad");
        }
        else {
            if (!isWindowResize && panelId == 'videos' && galleryDataObject.modelVideos.length > 0)
                stopVideo(prevIndex, currentIndex);
            isWindowResize = false;
        }

        //track click
        trackPrevNextClick(prevIndex, currentIndex);

        isImgClicked = false;
    });
}

function trackPrevNextClick(prevIndex, currentIndex) {
    if (prevIndex > currentIndex)//carousel prev click
        dataLayer.push({ event: 'Photo-Gallery-Mobile', cat: 'Photo-Gallery-Mobile', act: 'Prev-Click', lab: carName });

    if (currentIndex > prevIndex && !isImgClicked)//next click
        dataLayer.push({ event: 'Photo-Gallery-Mobile', cat: 'Photo-Gallery-Mobile', act: 'Next-Click', lab: carName });
}

// function to unbind the video and rebind again
function stopVideo(PrevVideoIndex, currentVideoIndex) {
    var div = document.getElementById("videoGalleryCarousel");
    var src = div.getElementsByTagName("iframe")[PrevVideoIndex - 1].getAttribute("src");
    div.getElementsByTagName("iframe")[PrevVideoIndex - 1].setAttribute("src", "");
    div.getElementsByTagName("iframe")[PrevVideoIndex - 1].setAttribute("src", src);
}

// function to Pause video
function pauseVideo() {
    if (panelId == 'videos' && galleryDataObject.modelVideos.length > 0)
        document.getElementById("videoGalleryCarousel").getElementsByTagName("iframe")[currentImgIndex - 1].contentWindow.postMessage('{"event":"command","func":"pauseVideo","args":""}', '*');
}

function windowResize() {
    var resizeTimer;
    $(window).resize(function () {
        isWindowResize = true;
        clearTimeout(resizeTimer);
        resizeTimer = setTimeout(function () {
            callActionBox();
            resizeCarousel();
            $('div.m-carousel').carousel('move', currentImgIndex);
        }, 200);
    });
}

$(document).ready(function () {
    getModelPhotos(modelId);
    callActionBox();
    windowResize();
    resizeCarousel();
    //trackUserHistory([modelId]);
    $('#reqCallBtn').attr("dealerId", "1");
    Common.utils.trackClicks();
    Common.utils.trackImpressions();

    // below code for opening gallery data on click on gallery text
    $("div.nc-pg-text").click(function () {
        $("div.m-defaultAlert-window-new").show();
        pauseVideo();
        $("div.nc-pg-thumb").slideDown(400, lazyLoadViewport).removeClass("hide");
        $("div.nc-pg-text").hide();
    });

    // below code for hiding gallery data on click on img
    $("div.m-defaultAlert-window-new").hide();

    $("div.m-defaultAlert-window-new").click(function () {
        $("div.nc-pg-thumb").slideUp().removeClass("hide");
        $(this).hide();
        $("div.nc-pg-text").show();
        $('div.m-carousel').carousel();
    });

    $(document).on('click', 'div.nc-pg-tabs-data li', function (e) {
        //Show selected image in carousel
        isImgClicked = true;
        var currentIndex = $(this).index();

        currentImgIndex = currentIndex + 1;

        if (panelId == 'videos' && galleryDataObject.modelVideos.length > 0) {
            var previousIndex = 0;
            if (previousIndex != currentIndex) {
                stopVideo(previousIndex + 1, currentIndex);
                previousIndex = currentIndex;
            }
        }
        else {
            //lazy load carousel image
            var currentItem = $("#imgCarouselContainer img")[currentIndex];
            $(currentItem).trigger("imgLazyLoad");
        }

        $('div.m-carousel').carousel('move', currentImgIndex);
        $("div.nc-pg-thumb").slideUp().removeClass("hide");
        $("html, body").animate({ scrollTop: $("div.nc-pg-thumb").offset().top }, 1000);
        $("div.nc-pg-text").show();

        $("div.m-defaultAlert-window-new").hide();

        dataLayer.push({ event: 'Photo-Gallery-Mobile', cat: 'Photo-Gallery-Mobile', act: 'Image-Click', lab: carName });
    });

    bindAfterSlide();

    // below code for nc-pg-tabs
    $("div.nc-pg-tabs li").click(function () {
        $("div.nc-pg-tabs li").removeClass('active');
        $(this).addClass('active');
        galleryTabId = $(this).index() + 1; //category tab
        var panel = $(this).closest('.tab-panel');

        panelId = $(this).attr('data-id');

        var videoListContainer = $("div#videoList");
        var imgListContainer = $("div#imgList");

        if (panelId == 'videos') {
            bindVideos(videoListContainer, imgListContainer);

            dataLayer.push({ event: 'Photo-Gallery-Mobile', cat: 'Photo-Gallery-Mobile', act: 'Video-Tab-Click', lab: carName });
        }
        else {
            showHideImageContainer();
            bindImages(imgListContainer, videoListContainer);
            applyLazyLoad();

            var tabClick = panelId + '-Tab-Click';
            dataLayer.push({ event: 'Photo-Gallery-Mobile', cat: 'Photo-Gallery-Mobile', act: tabClick, lab: carName });

        }

        bindAfterSlide();
        resizeCarousel();
    });

    function showHideImageContainer() {
        var noImageContainer = $("div#noImages");
        noImageContainer.addClass('hide');

        if (panelId == 'exterior') {
            if (exteriorPhotos.length <= 0)
                noImageContainer.removeClass('hide');
        }
        else {
            if (interiorPhotos <= 0)
                noImageContainer.removeClass('hide');
        }
    }

    function bindVideos(videoListContainer, imgListContainer) {
        videoListContainer.removeClass("hide");
        imgListContainer.addClass("hide");
        imgCarouselContainer.html("");
        videoCarouselContainer.html(videoCarouselHtml);
        ko.applyBindings(viewModel, document.getElementById('videoGalleryCarousel'));

        if (galleryDataObject.modelVideos.length <= 0)
            $("div#noVideos").removeClass('hide');
        else
            $("div#noVideos").addClass('hide');
    }

    function bindImages(imgListContainer, videoListContainer) {
        imgCarouselContainer.html(imageCarouselHtml);

        videoListContainer.addClass("hide");
        imgListContainer.removeClass("hide");

        videoCarouselContainer.html("");

        viewModel.filteredGallery(galleryTabId == 1 ? exteriorPhotos : interiorPhotos);

        ko.cleanNode(document.getElementById('imgGalleryCarousel'));
        ko.applyBindings(viewModel, document.getElementById('imgGalleryCarousel'));
    }

    // deals emi popup close function
    function closeEmiPopup() {
        $('#contact-det-popup').css({ 'overflow': 'hidden', 'top' : 'auto' }).slideUp();
        $("div.blackOut-window").hide();
    }

    // deals emi popup open function
    function openEmiPopup() {
        $("div.blackOut-window").show();
        $('#whatWeGet,#btnAssistance').show();
        $('#contact-det-popup').slideDown().css({ 'overflow-y': 'auto', 'top': '' });
        $('.contact-det-popup-content,.form-helper-content,.form-heading').show();
    }

    // popup cross button click function
    $('div.globalcity-close-btn, div.blackOut-window').on('click', function () {
        closeEmiPopup();
    });

    $('#contact-det-popup').removeClass('rounded-corner2');

    // below code for closing 'm-defaultAlert-window'
    $(".nc-pg-close-icon,#formDone").click(function () {
        $("#reqCallTitle").show();
        $("div.m-defaultAlert-window").hide();
    });

    $('div.m-carousel').on('swipe', function (e) {
        dataLayer.push({ event: 'Photo-Gallery-Mobile', cat: 'Photo-Gallery-Mobile', act: 'Swipe', lab: carName });
    });

    
    function getDealerObject() {
        var data = new Object();
        data.makeId = "1";
        data.makeName = "Hyundai";
        data.modelId = "586";
        data.modelName = "i10";
        data.versionId = "";
        data.versionName = "";
        data.cityId = $.cookie("_CustCityIdMaster");
        data.zoneId = isCookieExists('_CustZoneIdMaster') ? $.cookie('_CustZoneIdMaster') : "";
        data.ShowEmail = 'akansha@gmail.com';
        data.DealerLeadBusinessType = "A";
        data.dealerName = "Alpha";
        data.DealerID = "1";
        data.dealerAutoAssignPanel = "1";
        data.AdType = "";
        data.PQId = "0";
        data.AdType = "photopopup";
        data.GACat = "Photo-Gallery-Msite";
        data.inquirySourceId = "120";
        return data;
    }

    //hide form on blackout window click
    $('div.m-defaultAlert-window').click(function () {
        $("div.m-defaultAlert-window").hide();
    });

    $("div.nc-pg-tabs li,.nc-pg-text").click(function () {
        lazyLoadViewport();
    });

    function prefillUserDetails() {
        $("input[prefill]").each(function (count, element) {
            if ($(element).attr('prefill') == 'name') {
                $(element).val(getUserName());
            }
            else if ($(element).attr('prefill') == 'email') {
                $(element).val(getUserEmail());
            }
            else if ($(element).attr('prefill') == 'mobile') {
                $(element).val(getUserMobile());
            }
        });
    }

    function getUserName() {
        var name = "";
        if ($.cookie('_CustomerName') != null)
            name = $.cookie('_CustomerName');
        else if ($.cookie('TempCurrentUser') != null) {
            name = $.cookie('TempCurrentUser').split(':')[0];
        }
        return name;
    }
    function getUserMobile() {
        var mobile;
        if ($.cookie('_CustMobile') != null && $.cookie('_CustMobile') != "")
            mobile = $.cookie('_CustMobile');
        else if ($.cookie('TempCurrentUser') != null) {
            mobile = $.cookie('TempCurrentUser').split(':')[1];
        }
        return mobile;
    }
    function getUserEmail() {
        var email;
        if ($.cookie('_CustEmail') != null)
            email = $.cookie('_CustEmail');
        else if ($.cookie('TempCurrentUser') != null) {
            email = $.cookie('TempCurrentUser').split(':')[2];
        }
        return email;
    }

    prefillUserDetails();//prefill user details

    Deals.doc.on('click', "#mBtnNoThanks", function () {
        Deals.dealInquiries.m_closeInquiryPopup();
    });
});

function isCookieExists(cookiename) {
    var coockieVal = $.cookie(cookiename);
    if (coockieVal == undefined || coockieVal == null)
        return false;
    return true;
}

// tracking user history
function trackUserHistory(modelIdArray) {
    try {
        if (isCookieExists('_userModelHistory')) {
            var userHistory = getUserModelHistory();
            var userHistoryArray = userHistory.split(",");

            userHistoryArray = insertIntocookie(userHistoryArray, modelIdArray);

            document.cookie = '_userModelHistory=' + userHistoryArray.join("~") + '; expires = ' + permanentCookieTime() + "; domain=" + defaultCookieDomain + '; path =/';
        } else {

            document.cookie = '_userModelHistory=' + modelIdArray.join("~") + '; expires = ' + permanentCookieTime() + "; domain=" + defaultCookieDomain + '; path =/';

            var userHistory = getUserModelHistory();
            var userHistoryArray = userHistory.split("~");

            if (userHistoryArray.length > 20) {
                userHistoryArray.splice(0, modelArrayLen - 20);
                document.cookie = '_userModelHistory=' + userHistoryArray.join("~") + '; expires = ' + permanentCookieTime() + "; domain=" + defaultCookieDomain + '; path =/';
            }
        }
    }
    catch (e) {
        console.log("exception in track user history");
    }
}

function insertIntocookie(userHistoryArray, modelIdArray) {
    try {
        var modelArrayLen = modelIdArray.length;
        var isModelInserted;

        for (var modelIndex = 0; modelIndex < modelArrayLen; modelIndex++) {
            isModelInserted = false;
            for (var userHisIndex = 0; userHisIndex < userHistoryArray.length; userHisIndex++) {
                if (modelIdArray[modelIndex] == userHistoryArray[userHisIndex]) {
                    userHistoryArray.splice(userHisIndex, 1);
                    userHistoryArray.push(modelIdArray[modelIndex]);
                    isModelInserted = true;
                    break;
                }
            }
            if (!isModelInserted) {
                if (userHistoryArray.length == 20) {
                    userHistoryArray.splice(0, 1);
                }
                userHistoryArray.push(modelIdArray[modelIndex]);
            }
        }
        return userHistoryArray;
    }
    catch (e) {
        console.log("exceptionin insert into cookie");
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