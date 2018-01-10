docReady(function() {
	var isMobile = true

	if(window.innerWidth > 768) {
		isMobile = false;
	}

	function MakeModelPhotosRedirection(items) {
	    var make = new Object();
	    make.maskingName = items.payload.makeMaskingName;
	    make.id = items.payload.makeId;
	    var model = null;
	    if (items.payload.modelId > 0) {
	        model = new Object();
	        model.maskingName = items.payload.modelMaskingName;
	        model.id = items.payload.modelId;
	        model.futuristic = items.payload.futuristic;
	    }

	    if (model != null && model != undefined) {
	        window.location.href = "/" + make.maskingName + "-bikes/" + model.maskingName + "/"; // To be replaced for photos page
	        return true;
	    } else if (make != null && make != undefined) {
	        window.location.href = "/" + make.maskingName + "-bikes/"; // To be replaced for photos page
	        return true;
	    }
	}

	// explore bikes search
	$("#exploreBikesField").bw_autocomplete({
		recordCount: 5,
		source: 8,
		click: function (event, ui, orgTxt) {
		    MakeModelPhotosRedirection(ui.item);
		    isMakeModelPhotosRedirected = true;
		},
		open: function (result) {
			$("ul.ui-menu").width($('#exploreBikesField').innerWidth());
		},
		focus: function() {
			if (isMobile) {
				$('html, body').animate({
					scrollTop: $('#exploreBikesField').offset().top - 20
				})
			}
		},
		focusout: function () {
			if ($('li.ui-state-focus a:visible').text() != "") {
			}
			else {
				$('#errExploreBikes').hide();
			}
		},
		afterfetch: function (result, searchtext) {
			if (result != undefined && result.length > 0 && searchtext.trim()) {
				$('#errExploreBikes').hide();
			}
			else {
				if (searchtext.trim() != '') {
					$('#errExploreBikes').show();
				}
			}
		}
	});

	// body type filter
	$('#filterBodyType').on('click', '.body-type__item', function() {
		if(!$(this).hasClass('active')) {
			$(this).siblings('.body-type__item').removeClass('active');
			$(this).addClass('active');
		}
		else {
			$(this).removeClass('active');
		}
	});

	//collapsible content
	$('.foldable-content .read-more-button').on('click', function () {
		var readMoreButton = $(this);
		var collapsibleContent = readMoreButton.closest('.foldable-content');
		var isDataToggle = collapsibleContent.attr('data-toggle');
		var dataTruncate = collapsibleContent.find('.truncatable-content');
		var dataLessText;
		var readLessText;

		switch (isDataToggle) {
			case 'no':
				dataTruncate.attr('data-readtextflag', '0');
				readMoreButton.hide();
				break;

			case 'yes':
				dataLessText = readMoreButton.attr('data-text');
				readLessText = !dataLessText || dataLessText.length === 0 ? 'Collapse' : dataLessText;
				dataTruncate.attr('data-readtextflag', '0');
				readMoreButton.attr('data-text', readMoreButton.text()).text(readLessText);
				collapsibleContent.attr('data-toggle', 'hide');
				break;

			case 'hide':
				dataTruncate.attr('data-readtextflag', '1');
				dataLessText = readMoreButton.attr('data-text');
				readMoreButton.attr('data-text', readMoreButton.text()).text(dataLessText);
				collapsibleContent.attr('data-toggle', 'yes');
				if (isMobile) {
					$('html, body').animate({
						scrollTop: collapsibleContent.offset().top
					}, 500);
				}
				break;

			default:
				break;
		}
	});
})
