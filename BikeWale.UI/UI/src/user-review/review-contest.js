// bike review form
var reviewSelectMake = $('#reviewSelectMake'),
	reviewSelectModel = $('#reviewSelectModel'),
	reviewBikeSubmitBtn = $('#reviewBikeSubmitBtn');

// floating tabs
var $window, overallSpecsTabsContainer, specsTabsContentWrapper, specsFooter, specsTabs;
var topNavBarHeight = 45;

// Make Model info
var makeId = $('#makeId').val(),
    modelId = $('#modelId').val();

docReady(function () {
	var bikeSelectField = $('.bike-select-field');

    bikeSelectField.each(function () {
		$(this).val('').chosen({width: "100%"});

        var text = $(this).attr('data-placeholder');
        $(this).siblings('.chosen-container').find('input[type=text]').attr('placeholder', text);
    });

	//$('#bannerTargetBtn').on('click', function () {
	//	$('html, body').animate({ scrollTop: $('#selectBikeForm').offset().top - topNavBarHeight }, 1000);
	//});

	$('.bike-select-field').on('change', function () {
		if(parseInt($(this).val()) > 0) {
			$(this).closest('.select-box').addClass('done');
			validateSelect.hideError($(this));
		}
	});

	
	reviewSelectMake.on('change', function () {
		var makeId = parseInt(reviewSelectMake.val());

		reviewSelectModel.empty();
		reviewSelectModel.closest('.select-box').removeClass('done');

		if(!isNaN(makeId) && makeId > 0) {
			$.ajax({
                type: "Get",
                async: false,
                url: "/api/modellist/?requestType=3&makeId=" + makeId,
                contentType: "application/json",
                dataType: 'json',
                success: function (response) {
                    if (response) {
                        setModelOptions(reviewSelectModel, response.modelList);
                    }
                },
				complete: function (xhr, ajaxOptions, thrownError) {
					validateSelect.hideError($(reviewSelectModel));
				}
            });
		}
	});

	function setModelOptions(selectField, optionList) {
		if (optionList) {
			var selectOptions = '<option></option>';

			var i = 0,
				listLength = optionList.length;

			for(i; i < listLength; i++) {
				selectOptions += '<option value="'+ optionList[i].modelId +'">'+ optionList[i].modelName +'</option>';
			}

			selectField.append(selectOptions);
			selectField.prop('disabled', false).trigger("chosen:updated");
		}
	}

	reviewBikeSubmitBtn.on('click', function () {
		var isValid = validateBikeSelection();
		if(isValid) {
		    var returnUrl = $('#querystring').data('query');
			window.location = "/rate-your-bike/" + Number(reviewSelectModel.val()) + "/?q=" + returnUrl;
		}
	});

	function validateBikeSelection() {
		var isValid = false;
		isValid = validateBike.make(reviewSelectMake);
		isValid &= validateBike.model(reviewSelectModel);

		return isValid;
	}

	function prefillMakeModel() {
	    reviewSelectMake.val(makeId);
	    reviewSelectMake.parent().addClass("done");
	    reviewSelectMake.trigger("chosen:updated").change();

	    reviewSelectModel.val(modelId).trigger("chosen:updated");
	    reviewSelectModel.parent().addClass("done");

	}
	var validateBike = {
		make: function (selectField) {
			var isValid = false;

			if(!selectField.val()) {
				validateSelect.setError($(selectField), 'Please select make');
                return false;
			}
			else {
				validateSelect.hideError($(selectField));
                return true;
			}
		},

		model: function (selectField) {
			var isValid = false;

			if(!selectField.val()) {
				validateSelect.setError($(selectField), 'Please select model');
                return false;
			}
			else {
				validateSelect.hideError($(selectField));
                return true;
			}
		}
	}

	/* form validation */
    var validateSelect = {
		setError: function (element, message) {
			element.closest('.select-box').addClass('invalid');
			element.siblings('span.error-text').text(message);
		},

		hideError: function (element) {
			element.closest('.select-box').removeClass('invalid');
			element.siblings('span.error-text').text('');
		}
	};

	/* floating tabs */
	$window = $(window);
	overallSpecsTabsContainer = $('#overallSpecsTab');
	specsTabsContentWrapper = $('#specsTabsContentWrapper');
	specsFooter = $('#specsFooter');
	specsTabs = specsTabsContentWrapper.find('.bw-model-tabs-data');

	$(window).scroll(function () {
        var windowScrollTop = $window.scrollTop(),
            specsTabsOffsetTop = specsTabsContentWrapper.offset().top,
            specsFooterOffsetTop = specsFooter.offset().top;

        if (windowScrollTop > specsTabsOffsetTop) {
            overallSpecsTabsContainer.addClass('fixed-tab-nav');
        }

        else if (windowScrollTop < specsTabsOffsetTop) {
            overallSpecsTabsContainer.removeClass('fixed-tab-nav');
        }

        if (overallSpecsTabsContainer.hasClass('fixed-tab-nav')) {
            if (windowScrollTop > specsFooterOffsetTop - topNavBarHeight) {
                overallSpecsTabsContainer.removeClass('fixed-tab-nav');
            }
        }

        specsTabs.each(function () {
            var top = $(this).offset().top - topNavBarHeight,
                bottom = top + $(this).outerHeight();

            if (windowScrollTop >= top && windowScrollTop <= bottom) {
				if(!$(this).hasClass('active')) {
					overallSpecsTabsContainer.find('a').removeClass('active');
					specsTabs.removeClass('active');

					$(this).addClass('active');
					overallSpecsTabsContainer.find('a[href="#' + $(this).attr('id') + '"]').addClass('active');
				}
			}
        });   

    });

	$('.overall-specs-tabs-wrapper a[href^="#"]').click(function () {
        var target = $(this.hash);

        if (target.length == 0) target = $('a[name="' + this.hash.substr(1) + '"]');
        if (target.length == 0) target = $('html');
        $('html, body').animate({ scrollTop: target.offset().top - topNavBarHeight }, 1000);
        return false;
    });

	var winnerSwiper = new Swiper('.winner-review-swiper', {
		pagination: '.winner-review-swiper .swiper-pagination',
		paginationClickable: 'true',
		nextButton: '.swiper-button-next',
		prevButton: '.swiper-button-prev',
		effect: 'coverflow',
		grabCursor: false,
		centeredSlides: true,
		slidesPerView: 'auto',
		initialSlide: 1,
		coverflow: {
			rotate: 50,
			stretch: 0,
			depth: 100,
			modifier: 1,
			slideShadows : true
		}
	});

	// redirect user to review page, since swiper prevents click event
	$('.winner-review-swiper').on('click', '.swiper-card-target', function() {
		window.location = $(this).attr('href'); 
	});

	// read more list - collapse
	$('.collapsible-list').on('click','.read-more-list', function () {
		var element = $(this),
			parentElemtent = element.closest('.collapsible-list');

		if (!parentElemtent.hasClass('active')) {
			parentElemtent.addClass('active');
			element.text('Collapse [-]');
		}
		else {
			parentElemtent.removeClass('active');
			element.text('Read more [+]');
			$('body').scrollTop(parentElemtent.offset().top - 20);
		}
	});

    // Make Model prefill when clicked userreview from Model page
	if(makeId > 0 &&  modelId > 0 )
	{
	    prefillMakeModel();
	 
	}

});