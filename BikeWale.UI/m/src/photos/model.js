﻿var ImageGrid = (function () {
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

	function registerEvents() {
		$(document).on('click', '#viewMoreImageBtn', function () {
			var listImageCount = $('.image-grid__list .image-grid-list__item').length;

			vmPhotosMore.Loadedphotos(vmModelGallery.photoList().slice(listImageCount + 1, listImageCount + 13));
			$("#imageGridBottom").append($("#photoTemplateWrapper ul li"));
			ImageGrid.alignRemainderImage();

			var newImageCount = vmModelGallery.photoList().length - $('.image-grid__list .image-grid-list__item').length - 1;

			if (newImageCount) {
				$('#moreImageCount').html(newImageCount);
			}
			else {
				$('#moreImageCount').closest('.image-grid__list-more').hide();
			}
		});

		$('.image-grid__list').on('click', '.image-grid-list__item', function () {
			if ($(this).attr('data-slug-size')) {
				return;
			}

			var imageIndex = $(this).index();

			if(!$(this).closest('#imageGridTop').length) {
				imageIndex += $('#imageGridTop .image-grid-list__item').length;
			}

			if (typeof (logBhrighuForImage) != "undefined" && imageIndex <= 0) {
				//included in gallery js
				logBhrighuForImage($(this));
			}

			$('#galleryLoader').show();
			vmModelGallery.openGalleryPopup();
			$('#galleryLoader').hide();
			mainGallerySwiper.slideTo(imageIndex);
		});
	}

	return {
		alignRemainderImage: alignRemainderImage,
		registerEvents: registerEvents
	}
})();

var ImageGridVideoSlug = (function () {
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

var LoadPhotosViewModel = function () {
	var self = this;
	self.Loadedphotos = ko.observableArray();
}

var vmPhotosMore;
var colorTabSwiper, colorTabThumbnailSwiper;

docReady(function () {
	vmPhotosMore = new LoadPhotosViewModel();
	ko.applyBindings(vmPhotosMore, $("#photoTemplateWrapper")[0]);

	if (popupGallery) {

		try {
			if (RETURN_URL && RETURN_URL.length > 0) {
				popupGallery.bindGallery();
				if (typeof (logBhrighuForImage) != "undefined" && IMAGE_INDEX <= 0) {
					logBhrighuForImage($('#mainPhotoSwiper .swiper-slide-active'));
				}
			}
		} catch (e) {
			console.warn(e.message);
		}
	}

	$('#galleryLoader').hide();
});

docReady(function () {
	
	var colorsTab = $('#colorTab');

	if (colorsTab.length) {
		vmModelColorSwiper = new ModelColorSwiperViewModel();
		ko.applyBindings(vmModelColorSwiper, colorsTab[0]);
	}

	var videoTab = $('#videoTab');

	if (videoTab.length) {
		vmModelVideo = new ModelVideoViewModel();
		vmModelVideo.getVideos(); // get initial videos
		ko.applyBindings(vmModelVideo, videoTab[0]);
	}

	ImageGrid.alignRemainderImage();
	ImageGrid.registerEvents();

	if (vmModelVideo.videoList()) {
		ImageGridVideoSlug.setSlug();
	}

	// set overalltabs event
	NavigationTabs.registerEvents();
	NavigationTabs.setTab();

	colorTabSwiper = new Swiper('#colorTabSwiper', {
		spaceBetween: 0,
		preloadImages: false,
		lazyLoading: true,
		lazyLoadingInPrevNext: true,
		nextButton: '#colorTabSwiper .color-type-next',
		prevButton: '#colorTabSwiper .color-type-prev',
		onInit: function (swiper) {
			vmModelColorSwiper.activeIndex(1);
			SwiperEvents.setDetails(swiper, vmModelColorSwiper);
		},
		onSlideChangeStart: function (swiper) {
			SwiperEvents.setDetails(swiper, vmModelColorSwiper);
			SwiperEvents.focusThumbnail(colorThumbnailSwiper, vmModelColorSwiper.activeIndex(), true);
		}
	});

	var colorTabThumbnailSwiper = new Swiper('#colorTabThumbnailSwiper', {
		spaceBetween: 0,
		slidesPerView: 'auto',
		onInit: function (swiper) {
			SwiperEvents.focusThumbnail(swiper, vmModelColorSwiper.activeIndex(), true);
		},
		onTap: function (swiper, event) {
			colorSwiper.slideTo(swiper.clickedIndex);
		}
	});

});
