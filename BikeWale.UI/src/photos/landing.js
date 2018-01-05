docReady(function() {
	// explore bikes search
	$("#exploreBikesField").bw_autocomplete({
		recordCount: 5,
		source: 1,
		click: function (event, ui, orgTxt) {
		},
		open: function (result) {
			$("ul.ui-menu").width($('#exploreBikesField').innerWidth());
		},
		focus: function() {
			/*$('html, body').animate({
				scrollTop: $('#exploreBikesField').offset().top - 20
			})*/
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
				/*$('html, body').animate({
					scrollTop: collapsibleContent.offset().top
				}, 500);*/
				break;

			default:
				break;
		}
	});
})
