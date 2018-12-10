/* Photo gallery start */
var galleryTemplate = $('div.pg-gallery').html();
var photoUlTemplate = $('#galleryList').html();
var videoUlTemplate = $('#videosList').html();
var carouselListWidth = 10;
var currentURI = window.location.pathname.substr(1);
var intervalLazy;
var galleryMakeName = 'Tata';
var ModelName = 'Tiago';
var isImagePopupOpen = false;
var videoUrl;

//openImagePopup
function openImagePopup(modelId) {
    $(".left-enquiry-form-2").hide();
    $('.gallery-enq-section').show();
    getModelPhotos(modelId);
    resetTabStateToDefault();
}

//closeImagePopup
function closeImagePopup() {
    isVideoTab = 0;
    $('.pd-blackwindow, div.nc-gallery-container').hide();
    $('.pd-blackwindow').removeClass('opacity85');
    Common.utils.unlockPopup();
    $('div.divCustForm, p.askDealerhide').show();
    isImagePopupOpen = false; // For pushState
    $('div.pg-gallery').html(galleryTemplate);
}

//getModelPhotos
function getModelPhotos(modelId) {
    $('.pd-blackwindow').addClass('opacity85');
    $('.pd-blackwindow, div.nc-gallery-container').show();
    Common.utils.lockPopup();
    $.ajax({
        type: 'GET',
        url: '/webapi/CarModeldata/Gallery/?applicationid=1&modelid='+ modelId +'&categoryidlist=8,10&totalrecords=500',
        dataType: 'json',
        success: function (json) {
            galleryDataObject = json;
            var defaultObj = { "ImageId": 0, "HostUrl": "http://imgd.aeplcdn.com/", "ImagePathThumbnail": "160x89/adgallery/no-img-big.png", "ImagePathLarge": "600x337/adgallery/no-img-big.png", "OriginalImgPath": "/adgallery/no-img-big.png", "MainImgCategoryId": 2, "ImageCategory": "Driving", "Caption": "", "ImageName": "no-img-big.png", "AltImageName": "", "ImageTitle": "", "ImageDescription": "", "MakeBase": { "makeId": 0, "makeName": "" }, "ModelBase": { "ModelId": 0, "ModelName": "", "MaskingName": "" } }
            if (typeof (galleryDataObject.modelImages) == "undefined" || galleryDataObject.modelImages.length < 1) galleryDataObject.modelImages[0] = defaultObj;
            manageGallery(galleryDataObject);
            manageVideoTab(galleryDataObject);
        }
    });
}

//manageGallery
function manageGallery(galleryDataObject) {
    if (galleryDataObject.modelImages.length != 0) {
        bindToKo(0);
    }
}

//manageVideoTab
function manageVideoTab(galleryDataObject) {
    if (galleryDataObject.modelVideos[0] != undefined)
        firstVideo = galleryDataObject.modelVideos[0].VideoUrl;
    else
        $('#galleryTabs #video').hide();
}

//resetTabStateToDefault
function resetTabStateToDefault() {
    $("#galleryTabs li").each(function () {
        $(this).removeClass("nc-gallery-active");
    });

    $('#All').addClass("nc-gallery-active");
}

//bindToKo
function bindToKo(filter) {
    if (filter == undefined) filter = 0;//0 for all
    _viewModel = new viewModelPhotos(filter);
    $('div.pg-gallery').html(galleryTemplate);
    bindKOTemplate(_viewModel, filter);
    popupHeadingKOBinding(_viewModel);
    galleryTabsKOBinding(_viewModel);
    bindGallery();
    clickVideoBinding();
    calculateCorouselWidth(); // calculate width at the time of binding
    recalculateCorouselWidth(); // calculate width at the time of image rendred using Lazy load
    bindLazyLoadEvents();
}

//bindKOTemplate
function bindKOTemplate(viewModel, filter) {
    var ulid = "";
    if (filter > 2) {
        $("div.pg-thumbs").html('<ul id="videosList" data-bind= "foreach: videos">' + videoUlTemplate + '</ul>');
        ulid = "videosList";
    }
    else {
        $("div.pg-thumbs").html('<ul id="galleryList" data-bind="template: { foreach: filteredGallery } ">' + photoUlTemplate + '</ul>');
        ulid = "galleryList";
    }
    ko.cleanNode(document.getElementById(ulid));
    ko.applyBindings(viewModel, document.getElementById(ulid));
}

//viewModelPhotos
var viewModelPhotos = function (filterIndex) {
    var self = this;
    self.gallery = ko.observableArray(galleryDataObject.modelImages);
    self.videos = ko.observableArray(galleryDataObject.modelVideos);

    self.filters = [
            {
                title: 'All', filter: null
            },
            {
                title: 'Interior', filter: function (item) {
                    return item.MainImgCategoryId == 1;
                }
            },
            {
                title: 'Exterior', filter: function (item) {
                    return item.MainImgCategoryId == 2;
                }
            },
            { title: 'Video', filter: function (item) { return false; } }
    ];

    self.activeFilter = ko.observable(self.filters[filterIndex].filter);//set a default filter
    self.setActiveFilter = function (model, event) {
        self.activeFilter(model.filter);
    };


    self.filteredGallery = ko.computed(function () {
        var result
        if (self.activeFilter()) {
            result = ko.utils.arrayFilter(self.gallery(), self.activeFilter());
        } else {
            result = self.gallery();
        }
        return result;
    });
}

//filterClick
function filterClick(filter, e) {
    isVideoTab = 0;
    $('#descriptions,div.pg-prev,div.pg-next').show();
    $("#galleryTabs li").removeClass("nc-gallery-active");
    $(e).addClass("nc-gallery-active");
    bindToKo(filter);
    showNoImage();
}

//galleryTabsKOBinding
function galleryTabsKOBinding(_viewModel) {
    ko.cleanNode(document.getElementById("galleryTabs"));
    ko.applyBindings(_viewModel, document.getElementById("galleryTabs"));
}

//popupHeadingKOBinding
function popupHeadingKOBinding(_viewModel) {
    ko.cleanNode(document.getElementById("galleryTitle"));
    ko.applyBindings(_viewModel, document.getElementById("galleryTitle"));
}

//bindGallery
function bindGallery() {
    $("div.pg-controls").empty();// empty the image gallery pager
    objGallery = $('div.pg-gallery').adGallery({
        start_at_index: 0,
        loader_image: "http://img.carwale.com/adgallery/loader.gif",
        callbacks: { // CallBack function after the image is loaded and is visible
            afterImageVisible: function () {
                getActiveImageUrl(this);
                manualLazyLoad();
                calculateCorouselWidth();
                isImagePopupOpen = true;
                showNoImage();
                if (videoUrl != undefined)
                    selectFirstVideo(videoUrl);
            }
        }
    })[0];
}

//clickVideoBinding
function clickVideoBinding() {
    $("#videosList li a").click(function (e) {
        videoUrl = this.href;
    });

}

//recalculateCorouselWidth
function recalculateCorouselWidth() {
    $("div.pg-thumbs img").load(function (e) {
        calculateCorouselWidth(e)
    });
    $("div.pg-thumbs img").error(function (e) {
        calculateCorouselWidth(e)
    });
}

//calculateCorouselWidth
function calculateCorouselWidth(img) {
    $("div.pg-thumbs ul").width(15000);

    var liArray = $("div.pg-thumbs li");
    for (i = 0; i < liArray.length; i++) {
        carouselListWidth += liArray[i].clientWidth < 100 ? 160 : liArray[i].clientWidth;
    }
    $("div.pg-thumbs ul").width(carouselListWidth);
    carouselListWidth = 10;
}

//bindLazyLoadEvents
function bindLazyLoadEvents() {
    $('div.pg-forward,div.pg-back').click(manualLazyLoad);
    $('div.pg-forward,div.pg-back').hover(intervalLazyLoad);
}

//showNoImage
function showNoImage() {
    if ($("div.pg-thumbs ul").is(':empty')) {
        $("div.pg-image-wrapper").html("<img src=\"http://imgd.aeplcdn.com/0x0/adgallery/no-img-big.png\" height='320px'/>");
    }
}

//getActiveImageUrl
function getActiveImageUrl(adGallery) {
    var imgUrl, url;

    if ($("#videosList").is(":visible")) {
        var video = _viewModel.videos()[adGallery.current_index];
        url = $.trim(video.SubCatName).toLowerCase().split(' ').join('-') + "-" + video.BasicId + "/";
        sectionURI = 'videos/';
    }
    else {
        imgUrl = adGallery.images[adGallery.current_index].image;
        var arrayurl = imgUrl.split('/');
        url = arrayurl[arrayurl.length - 1].toLowerCase().replace('jpg', 'html');
        sectionURI = 'photos/';
    }

}

//manualLazyLoad
function manualLazyLoad() {
    $("div.pg-thumbs li img:in-viewport").each(function (index) {
        $(this).attr('src', $(this).attr('data-original'))
        $(this).removeClass("lazy");
    })
}

//selectFirstVideo
function selectFirstVideo(url) {
    if (isVideoTab == 1) {
        $('div.pg-image').css('width', '589').css('height', '326').css('left', '0').css('top', '0');
        var videoContent = "<iframe width=\"589\" height=\"326\" src=" + url + " frameborder='0' allowfullscreen></iframe>";
        $("#gallery div.pg-image").html(videoContent);
    }
}

//intervalLazyLoad
function intervalLazyLoad() {
    if (intervalLazy == undefined) intervalLazy = window.setInterval(manualLazyLoad, 1000);
    else {
        window.clearInterval(intervalLazy); intervalLazy = undefined;
    }
}

//videoGallery
function videoGallery(e) {
    isVideoTab = 1;
    videoUrl = firstVideo;
    bindToKo(3);
    $('#descriptions,div.pg-prev,div.pg-next').hide();
    if ($('div.pg-image').length < 1) {
        $('div.pg-image-wrapper').prepend("<div class='pg-image'></div>");
    }
    $('div.pg-image').empty();
    $("#galleryTabs li").removeClass("nc-gallery-active");
    $(e).addClass("nc-gallery-active");
    selectFirstVideo(firstVideo);
}

function removeYear(name)
{
    if (name.indexOf('[') > 0)
        return name.split('[')[0];
    else
        return name;
}

//close btn and backwindow click
$('span.cw-nc-close-btn, .pd-blackwindow').on('click', function () { closeImagePopup() });
/* Photo gallery end */