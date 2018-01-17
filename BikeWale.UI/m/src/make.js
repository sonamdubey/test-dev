docReady(function() {
	$('.model-card__pros-cons').on('click', '.pros-cons__more-btn', function(event) {
		$(this).hide();
		$(this).closest('.pros-cons__content').find('li').show();
	});

	// popular bikes carousel
	$('.carousel__popular-bikes').on('click', '.view-pros-cons__target', function() {
		var modelCard = $(this).closest('.model__card');

		$(this).hide();

		if (!modelCard.hasClass('card--active')) {
			modelCard.addClass('card--active');
		}
		else {
			modelCard.removeClass('card--active');
		}
	});

	$('.carousel__popular-bikes').on('webkitTransitionEnd transitionend', '.model-card__detail', function() {
		var modelCard = $(this).closest('.model__card');		
		var collpaseTargetElement = modelCard.find('.view-pros-cons__target');

		var collapseCurrentText = collpaseTargetElement.html(),
			collapseNextText = collpaseTargetElement.attr('data-text');

		if (!collpaseTargetElement.is(':visible')) {
			if (!modelCard.hasClass('card--active')) {
				modelCard.removeClass('collapse-btn--active');
			}
			else {
				modelCard.addClass('collapse-btn--active');
			}
			
			collpaseTargetElement.attr('data-text', collapseCurrentText);
			collpaseTargetElement.html(collapseNextText).fadeIn();
		}
	});

});
