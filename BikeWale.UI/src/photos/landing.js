var objBikes = new Object(), focusedMakeModel = null, isMakeModelRedirected = false;
docReady(function () {
	var isMobile = true

	if(window.innerWidth > 768) {
		isMobile = false;
	}
	$("#exploreBikesField").val("");
	
	function MakeModelPhotosRedirection(items) {
	    var makeMaskingName = items.payload.makeMaskingName;
	    var modelMaskingName = items.payload.modelMaskingName;
	    if (items.payload.modelId > 0 && makeMaskingName != null && makeMaskingName != "" && modelMaskingName != null && modelMaskingName != "") {
	        if (isMobile) {
	            window.location.href = "/m/" + makeMaskingName + "-bikes/" + modelMaskingName + "/images/";
	        }
	        else {
	            window.location.href = "/" + makeMaskingName + "-bikes/" + modelMaskingName + "/images/";
	        }
	        return true;
	    } else if (makeMaskingName != null && makeMaskingName !="") {
	        if (isMobile) {
	            window.location.href = "/m/photos/" + makeMaskingName + "-bikes"; // To be replaced for photos page
	        }
	        else {
	            window.location.href = "/photos/" + makeMaskingName + "-bikes"; // To be replaced for photos page
	        }
	        return true;
	    }
	}
	$("#btnexplorebikesfield").on("click", function () {
	    if (focusedMakeModel == undefined || focusedMakeModel == null) {
	        return false;
	    }
	    return MakeModelPhotosRedirection(focusedMakeModel);
	});
	// explore bikes search
	$("#exploreBikesField").bw_autocomplete({
		recordCount: 5,
		source: 8,
		click: function (event, ui, orgTxt) {
		    $("#exploreBikesField").val("");
		    MakeModelPhotosRedirection(ui.item);
		    isMakeModelRedirected = true;
		},
		open: function (result) {
		    $("ul.ui-menu").width($('#exploreBikesField').innerWidth());
		    objBikes.result = result;
		},
		focus: function() {
			if (isMobile) {
				$('html, body').animate({
					scrollTop: $('#exploreBikesField').offset().top - 20
				})
			}
			focusedMakeModel = new Object();
			focusedMakeModel = objBikes.result ? objBikes.result[$('li.ui-state-focus').index()] : null;
		},
		focusout: function () {
		    if ($('#exploreBikesField').find('li.ui-state-focus a:visible').text() != "") {
		        focusedMakeModel = new Object();
		        focusedMakeModel = objBikes.result ? objBikes.result[$('li.ui-state-focus').index()] : null;
			}
			else {
				$('#errExploreBikes').hide();
			}
		},
		afterfetch: function (result, searchtext) {
		    if (result != undefined && result != null && result.length > 0 && searchtext.trim()) {
				$('#errExploreBikes').hide();
			}
		    else {
		        focusedMakeModel = null;
				if (searchtext.trim() != '') {
					$('#errExploreBikes').show();
				}
			}
		},
		keyup: function () {
		    if ($('#exploreBikesField').val().trim() != '' && $('li.ui-state-focus a:visible').text() != "") {
		        focusedMakeModel = new Object();
		        focusedMakeModel = objBikes.result ? objBikes.result[$('li.ui-state-focus').index()] : null;
		        $('#errExploreBikes').hide();
		    } else {
		        if ($('#exploreBikesField').val().trim() == '') {
		            $('#errExploreBikes').hide();
		        }
		    }

		    if ($('#exploreBikesField').val().trim() == '' || e.keyCode == 27 || e.keyCode == 13) {
		        if (focusedMakeModel == null || focusedMakeModel == undefined) {
		            if ($('#exploreBikesField').val().trim() != '') {
		                $('#errExploreBikes').show();
		            }
		        }
		        else {
		            $('#errExploreBikes').hide();
		        }

		    }
		}
	}).keydown(function (e) {
	    if (e.keyCode == 13) {
	        if (!isMakeModelRedirected)
	            $('#btnExploreBikesField').click();
	        else
	            isMakeModelRedirected = false;
	    }

	}).autocomplete("widget").addClass("bike-images-autocomplete")

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
