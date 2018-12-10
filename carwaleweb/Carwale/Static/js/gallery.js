var MediaGallery = {
    baseUrl: (location.toString().split('/images/')[0] + '/images/'),
    url: {
        getImgUrl: function (node) {
            if (node)
                return MediaGallery.baseUrl + Editorial.utils.getImageUrl(node.attr('alt'), node.data('imgid'));
            return "";
        },
        updateUrl: function (url) {
            window.history.replaceState(null, null, url);
            fireComscorePageView();
        }
    },
    track: {
        firePageView: function (url) {
            try {
                ga('create', 'UA-337359-1', 'auto', { 'useAmpClientId': true });
                ga('send', 'pageview', url);
            }
            catch (e) {
                console.log(e);
            }
        },
        fireInteractiveEvent: function (event, category, action, label) {
            dataLayer.push({ event: event, cat: category, act: action, lab: label });
        }
    },
    download: {
        updateDownloadUrl: function (url) {
            $('#btnDownload').attr('href', url.split('&q')[0].split('?q')[0]);
        }
    },
    share: {
        updateShareUrl: function (url) {
            var currentUrl = window.location.href;
            url += currentUrl.indexOf('?') > 0 ? "&" : "?";

            $("#share-list a").each(function () {
                var node = $(this);
                currentUrl = node.data('href') + url + "utm_source=share";
                node.attr('href', currentUrl);
            });
        },

        toggleShare: function () {
            var self = MediaGallery.share;
            if (!self.viewModel.shareIconsVisible())
                MediaGallery.track.fireInteractiveEvent('CWInteractive', 'contentcons', 'MediaGallery', 'ToggleShare-' + window.location.href);
            self.viewModel.shareIconsVisible(!self.viewModel.shareIconsVisible());
        },

        onMouselLeave: function () {
            if (MediaGallery.share.viewModel.shareIconsVisible())
                MediaGallery.share.toggleShare();
        },

        viewModel: {
            shareIconsVisible: ko.observable(false)
        }
    },

    closeGallery: function () {
        var referrer = document.referrer;
        if (referrer.indexOf('carwale.com') > 0)
            window.history.back();
        else
            window.location = location.toString().split('/images/')[0];
    },

    registerEvents: function () {
        $('.carousel-stage').on('jcarousel:createend', function () {
            $(this).jcarousel('scroll', currImgIndex, false);
        }).jcarousel();
        $('.carousel-navigation').on('jcarousel:createend', function () {
            $(this).jcarousel('scroll', currImgIndex, false);
        }).on('jcarouselcontrol:active', function () {
            var currentUrl = '', self = MediaGallery.url, node = $('#thumbImages li.active img');
            currentUrl = self.getImgUrl(node);
            self.updateUrl(currentUrl);
            MediaGallery.track.firePageView(currentUrl);
            MediaGallery.download.updateDownloadUrl(node.attr('src').replace('160x89', '0x0'));
            MediaGallery.share.updateShareUrl(currentUrl);
        }).jcarousel();
        MediaGallery.share.updateShareUrl(window.location.href);
        MediaGallery.images.setInteriorExteriorImages();
        ko.applyBindings(MediaGallery.share.viewModel, document.getElementById('media-gallery--header'));

        //to handle keyboard navigation for image gallery
        $(document).on('keyup', function (e) {
            var key = e.which || e.keyChar || e.keyCode;
            var instance = $('.carousel-stage').data('jcarousel');
            if (key == 37) {
                instance.scroll('-=1');
            } else if (key == 39) {
                instance.scroll('+=1');
            }
        });

        MediaGallery.images.lazyLoadImages($('.carousel-navigation'));
        MediaGallery.images.lazyLoadImages($('.carousel-stage'));

        if (is360Available)
            MediaGallery.track.fireInteractiveEvent('CWNonInteractive', 'desktop_360_linkages', 'image_gallery_imp', carName);
    },

    images: {
        type: {
            all: 0,
            exterior: 1,
            interior: 2
        },
        interiorImages: [],
        exteriorImages: [],
        viewModel: {
            imagesData: ko.observableArray([]),
            title: ko.observable()
        },
        setInteriorExteriorImages: function () {
            allImages.forEach(function (element) { element.MainImgCategoryId == '2' ? MediaGallery.images.exteriorImages.push(element) : MediaGallery.images.interiorImages.push(element) });
        },
        filterImages: function (type, element) {
            if (!element.hasClass('active')) {
                var self = MediaGallery.images;
                if (allImages.length > 0) {
                    if (type == self.type.all) {
                        self.viewModel.imagesData = allImages;

                    }
                    else if (type == self.type.exterior) {
                        self.viewModel.imagesData = self.exteriorImages;
                    }
                    else if (type == self.type.interior) {
                        self.viewModel.imagesData = self.interiorImages;
                    }

                }
                else
                    $('#media-gallery--empty-view').show();
                element.parent().find('li.active').removeClass('active');
                element.addClass('active');
                $('#titleCategory').text('Images');
                $('#video-wrapper').hide();
                $('#image-wrapper,#btnDownload').show();
                if (allImages.length > 0) {
                    self.bindImagesWithCarousel();
                    self.lazyLoadImages($('.carousel-navigation'));
                    self.lazyLoadImages($('.carousel-stage'));
                }
            }
        },
        bindImagesWithCarousel: function () {
            ko.cleanNode(document.getElementById('mainImages'));
            ko.cleanNode(document.getElementById('thumbImages'));
            ko.applyBindings(MediaGallery.images.viewModel, document.getElementById('mainImages'));
            ko.applyBindings(MediaGallery.images.viewModel, document.getElementById('thumbImages'));
            $('.carousel-stage').jcarousel('destroy', {});
            $('.carousel-navigation').jcarousel('destroy', {});
            currImgIndex = 0;
            ConnectedCarousel.initializeCarousels();
        },
        lazyLoadImages: function (node) {
            node.jcarousel().on('jcarousel:visiblein', 'li', function () {
                    var imgs = [], node = $(this);
                    imgs.push(node.find('img'));
                    imgs.push(node.prev().find('img'));
                    imgs.push(node.next().find('img'));
                    $.each(imgs, function () {
                        var img = $(this);
                        if (img.data('original') && !img.data('loaded')) {
                            img.attr('original', img.data('src')).data('loaded', true);
                            img.attr('src', img.data('original'));
                        }
                    });
            })
        .jcarousel('visible').each(function () {
            $(this).trigger('jcarousel:visiblein');
        });
        },
        createImageUrl: function (hostUrl, size, originalImagePath, quality) {
            return (hostUrl + size + originalImagePath + (originalImagePath.indexOf('?') > -1 ? '&q=' : '?q=') + (quality || 85));
        }
    },
    videos: {
        changeVideoUrl: function (node) {
            $('#mainVideoFrame').attr('src', 'https://www.youtube.com/embed/' + node.data('videoid') + '?rel=0&fs=1&enablejsapi=1&modestbranding=0');
            MediaGallery.url.updateUrl(location.origin + node.data('videourl'));
        },
        setFirstVideo: function (element) {
            $('#media-gallery--empty-view').hide();
            var mainFrame = $('#mainVideoFrame');
            if (!mainFrame.attr('src')) {
                var firstVideo = $('#media-gallery--videos-thumbnail-list li:first');
                mainFrame.attr('src', "https://www.youtube.com/embed/" + firstVideo.data('videoid') + "?rel=0&fs=1&enablejsapi=1&modestbranding=0");
                mainFrame.data('videourl', firstVideo.data('videourl'));
            }
            MediaGallery.url.updateUrl(location.origin + mainFrame.data('videourl'));
            $('#image-wrapper,#btnDownload').hide();
            $('#video-wrapper').show();
            element.parent().find('li.active').removeClass('active');
            element.addClass('active');
            $('#titleCategory').text('Videos');
            $('img.lazy').lazyload({
                container: $("#media-gallery--videos-thumbnail-list"),
                effect: "fadeIn"
            });
        }
    }
};

MediaGallery.registerEvents();

