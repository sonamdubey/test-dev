var ThumbnailSwiperEvents = (function () {
	function attachColorEvents(swiper) {
		var elements = swiper.container[0].querySelectorAll('.swiper-slide');

		for (var i = 0; i < elements.length; i++) {
			elements[i].addEventListener('click', function (event) {
				var clickedIndex = $(event.currentTarget).index();

				colorGallerySwiper.slideTo(clickedIndex);
			});
		}
	}

	function focusThumbnail(swiper, vmActiveIndex, slideToFlag) {
		var activeIndex = vmActiveIndex - 1; // decrement by 1, since it was incremented by 1
		var thumbnailIndex = swiper.slides[activeIndex];

		if (slideToFlag) {
			swiper.slideTo(activeIndex);
		}

		$(swiper.slides).removeClass('swiper-slide-active slide--focus');
		$(thumbnailIndex).addClass('swiper-slide-active slide--focus');
	}

	function setColorPhotoDetails(swiper, viewModel) {
		var activeSlide = swiper.slides[swiper.activeIndex];
		var activeSlideTitle = $(activeSlide).find('img').attr('alt');

		viewModel.activeIndex(swiper.activeIndex + 1);
		viewModel.activeTitle(activeSlideTitle);
	}

	return {
		focusThumbnail: focusThumbnail,
		setColorPhotoDetails: setColorPhotoDetails,
		attachColorEvents: attachColorEvents
	}
})();

var ImageGrid = (function () {
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


var vmLoadPhotos = function () {
	var self = this;
	self.Loadedphotos = ko.observableArray();
}

docReady(function () {
	var vmPhotosMore = new vmLoadPhotos();
	ko.applyBindings(vmPhotosMore, $("#photoTemplateWrapper")[0]);

	if (popupGallery) {

		try {
			if (returnUrl && returnUrl.length > 0) {
				popupGallery.bindGallery(imageIndex);
				if (typeof (logBhrighuForImage) != "undefined" && imageIndex <= 0) {
					logBhrighuForImage($('#mainPhotoSwiper .swiper-slide-active'));
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

			// TODO: handle portrait images
			$('.photos-grid-list img.lazy').on('load', function () {
				resizePortraitImage($(this));
			});

			$('#viewMoreImageBtn').on('click', function () {
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

		} catch (e) {
			console.warn(e.message);
		}
	}
});

docReady(function () {
	
	var colorsTab = $('#colorTab');

	vmModelColorSwiper = new ModelColorSwiper();

	if (colorsTab.length) {
		ko.applyBindings(vmModelColorSwiper, colorsTab[0]);
	}

	ImageGrid.alignRemainderImage();

	if (vmModelVideo.videoList()) {
		videoSlug.setSlug();
	}

	// set overalltabs event
	NavigationTabs.registerEvents();
	NavigationTabs.setTab();

	var colorSwiper = new Swiper('#colorSwiper', {
		spaceBetween: 0,
		preloadImages: false,
		lazyLoading: true,
		lazyLoadingInPrevNext: true,
		nextButton: '#colorSwiper .color-type-next',
		prevButton: '#colorSwiper .color-type-prev',
		onInit: function (swiper) {
			vmModelColorSwiper.activeIndex(1);
			ThumbnailSwiperEvents.setColorPhotoDetails(swiper, vmModelColorSwiper);
		},
		onSlideChangeStart: function (swiper) {
			ThumbnailSwiperEvents.setColorPhotoDetails(swiper, vmModelColorSwiper);
			ThumbnailSwiperEvents.focusThumbnail(colorThumbnailSwiper, vmModelColorSwiper.activeIndex(), true);
		}
	});

	var colorThumbnailSwiper = new Swiper('#colorTSwiper', {
		spaceBetween: 0,
		slidesPerView: 'auto',
		onInit: function (swiper) {
			ThumbnailSwiperEvents.attachColorEvents(swiper);
			ThumbnailSwiperEvents.focusThumbnail(swiper, vmModelColorSwiper.activeIndex(), true);
		},
		onTap: function (swiper, event) {
			colorSwiper.slideTo(swiper.clickedIndex);
		}
	});

});
