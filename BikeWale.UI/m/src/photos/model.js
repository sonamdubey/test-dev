docReady(function () {
	imageGrid.alignRemainderImage();

	if (vmModelGallery.videoList().length) {
		videoSlug.setSlug();
	}
});

var imageGrid = (function () {
	function alignRemainderImage() {
		var imageList = $('.image-grid__list');

		imageList.each(function () {
			var gridSize = Number($(this).attr('data-grid'));
			var imageListItem = $(this).find('.image-grid-list__item');
			var imageListItemCount = imageListItem.length;

			var remainder = imageListItemCount % gridSize;

			switch (remainder) {
				case 1:
					if (gridSize === 7) {
						var remainderImage = imageListItem.slice(imageListItemCount - 1);

						_gridOne(remainderImage);
					}
					break;

				case 2:
					var remainderImage = imageListItem.slice(imageListItemCount - 2);

					_gridTwo(remainderImage);
					break;

				case 3:
					if (gridSize === 6) {
						var remainderImage = imageListItem.slice(imageListItemCount - 3);

						_gridThree(remainderImage);
						remainderImage.first().css({
							'float': 'right'
						})
					}
					break;

				case 5:
					if (gridSize === 6) {
						var remainderImage = imageListItem.slice(imageListItemCount - 1);

						_gridOne(remainderImage);
					}
					else if (gridSize === 7) {
						var remainderImage = imageListItem.slice(imageListItemCount - 2);

						_gridTwo(remainderImage);
					}
					break;

				default:
					break;
			}

		});
	}

	function _gridOne(image) {
		image.css({
			'width': '100%'
		});
	}

	function _gridTwo(images) {
		images.css({
			'width': '50%'
		});
	}

	function _gridThree(images) {
		images.each(function (index) {
			if (!index) {
				$(this).css({
					'width': '66.67%'
				})
			}
			else {
				$(this).css({
					'width': '33.33%'
				})
			}
		});
	}

	return {
		alignRemainderImage: alignRemainderImage
	}

})();

var videoSlug = (function () {
	function setSlug() {
		var imageListItems = $('#imageGridBottom .image-grid-list__item');
		var targetImageListItem;

		switch (imageListItems.length) {
			case 0:
				targetImageListItem = $('#imageGridTop .image-grid-list__item').last();
				break;

			case 1:
			case 3:
				targetImageListItem = imageListItems[0];
				break;

			default:
				targetImageListItem = imageListItems[1];
				break;
		}

		$(targetImageListItem).append($('#videoSlug').html());

		_setDimension($(targetImageListItem));
	}

	function _setDimension(targetElement) {
		var targetElementWidth = targetElement.width();
		var parentElementWidth = targetElement.parent().width();
		var size;

		if (targetElementWidth >= parentElementWidth) {
			size = 'large';
		}
		else if (targetElementWidth >= Math.floor(parentElementWidth / 1.5)) {
			size = 'medium';
		}
		else if (targetElementWidth >= Math.floor(parentElementWidth / 2)) {
			size = 'small';
		}
		else if (targetElementWidth >= Math.floor(parentElementWidth / 3)) {
			size = 'extra-small';
		}

		targetElement.attr('data-slug-size', size);
	}

	return {
		setSlug: setSlug
	}

})();

function morePhotosOverlay(limitCount) {
	var photosCnt = (photoCount - limitCount - 1);
	if (photosCnt && photosCnt > 0) {
		var lastPhoto = $('.photos-grid-list li').last(),
                   morePhotoCount = $('<span class="black-overlay"><span class="font14 text-white">+' + photosCnt + '<br />images</span></span>');
		lastPhoto.append(morePhotoCount);
	}
}

function bindPhotos(photosLimit) {
	var photosLength = $('.photos-grid-list').first().find('li').length;

	lastPhotoIndex = photosLimit - 1;

	// add 'more photos count' if photo grid contains 30 images
	if (photosLength == photosLimit) {
		morePhotosOverlay(photosLength - 1);
	}
}
var vmLoadPhotos = function () {
	var self = this;
	self.Loadedphotos = ko.observableArray();
}


docReady(function () {
	var photosLimit = 30;
	bindPhotos(photosLimit);
	var vmPhotosMore = new vmLoadPhotos();
	ko.applyBindings(vmPhotosMore, $("#photoTemplateWrapper")[0]);
	if (popupGallery) {

		try {
			if (returnUrl && returnUrl.length > 0) {
				popupGallery.bindGallery(imageIndex);
				if (typeof (logBhrighuForImage) != "undefined" && imageIndex <= 0) {
					logBhrighuForImage($('#main-photo-swiper .swiper-slide-active'));
				}
			}


			$('.image-grid__list').on('click', 'li', function () {
				if ($(this).attr('data-slug-size')) {
					return;
				}
				if (photoCount > 1) {

					var imageIndex = $(this).index(),
						parentGridType = $(this).closest('.image-grid__list');

					if (parentGridType.hasClass('image-grid--bottom')) {
						var gridOneLength = $('.image-grid__list').first().find('li').length;

						imageIndex = gridOneLength + imageIndex; // (grid type 1's length + grid type remainder's index)
					}

					if (typeof (logBhrighuForImage) != "undefined" && imageIndex <= 0) {
						//included in gallery js
						logBhrighuForImage($(this));
					}
					galleryRoot.find('.gallery-loader-placeholder').show();
					popupGallery.bindGallery(imageIndex);
					galleryRoot.find('.gallery-loader-placeholder').hide();
				}
			});

			$('.photos-grid-list img.lazy').on('load', function () {
				resizePortraitImage($(this));
			});

			$('#viewMoreImageBtn').on('click', function () {
				var listImageCount = $('.image-grid__list .image-grid-list__item').length;

				vmPhotosMore.Loadedphotos(vmModelGallery.photoList().slice(listImageCount + 1, listImageCount + 13));
				$("#imageGridBottom").append($("#photoTemplateWrapper ul li"));
				imageGrid.alignRemainderImage();

				var newImageCount = vmModelGallery.photoList().length - $('.image-grid__list .image-grid-list__item').length - 1;

				if (newImageCount) {
					$('#moreImageCount').html(newImageCount);
				}
				else {
					$('#moreImageCount').closest('.image-grid__list-more').hide();
				}

			});

		} catch (e) {
			console.warn(e.message);
		}
	}
	mainGallerySwiper.registerEvents();

});

var mainGallerySwiper = (function () {
	function registerEvents() {
		//$('#mainPhotoSwiper').on('click', 'img', function () {
		//	var imageNumber = $(this).closest('.swiper-slide').index();
		//	$('.photos-grid-list > li:nth-of-type(' + (imageNumber + 1) + ')').trigger('click');
		//});

		$('.model-gallery img').on('click', function (e) {
			e.stopPropagation();
			var container = $(this).closest('.model-gallery__container')
			if (!container.hasClass('model-gallery--relative')) {
				$('.model-gallery__image-title').hide();
				container.addClass('model-gallery--relative');
				calculateCenter();
				$('.black-window').fadeIn(300);
				$('.model-gallery__image-title').css('display', 'inline');
				bodyBackground.lock();
				//setTimeout(function () {
				//    $('.model-gallery__container').css('top', 0);
				//});
				//$('.photos-grid-list> li:first-child').trigger('click');
				toggleGalleryFooter();

               // $('.model-gallery__screen--rotate-slug').show();
			}

		});

		$(document).on('click', '.black-window', function (e) {
			e.stopPropagation();
			var auto = 'auto';
			$('.model-gallery__container').animate({
				top: 0
			}, 300, "swing");
			$('.black-window').fadeOut(300);
			//setTimeout(function () {
			$('.model-gallery__container').removeClass('model-gallery--relative');
			//}, 500);
			bodyBackground.unlock();
            //$('.model-gallery__screen--rotate-slug').hide();
			toggleGalleryFooter();
			gallerySlug.removeSlug($('#mainPhotoSwiper').find('.swiper-slide__slug'));
		});

		$(window).on('resize', function () {
			if ($('.model-gallery--relative').is(':visible')) {
				calculateCenter();
				toggleGalleryFooter();
			}


		});
		function toggleGalleryFooter() {
			if ($(window).height() < $(window).width()) {
				$('#mainPhotoSwiper .swiper-wrapper, #mainPhotoSwiper img').on('click', function () {
					$('.gallery-image__footer').toggle();
				});
            }
            else {
                $('.gallery-image__footer').show();
			}

		}

	};


	function calculateCenter() {
		var CenterPosition,
			element = $('.model-gallery__container'),
			ElementScrollTop = element.closest('.model-gallery-section').offset().top,
			$window = $(window),
			WidowScrollTop = $window.scrollTop(),
			WindowHeight = $window.height() / 2;

		if (WidowScrollTop > ElementScrollTop) {
			CenterPosition = ((WidowScrollTop - ElementScrollTop) + (WindowHeight) - (element.height() / 2));
		}
		else {
			CenterPosition = (((WindowHeight) - element.height() / 2) - (ElementScrollTop - WidowScrollTop));
		}
		$('.model-gallery__container').animate({ top: CenterPosition }, 300, "swing");
	};

	return {
		registerEvents: registerEvents
	}
})();

var bodyBackground = {
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
