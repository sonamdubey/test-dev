docReady(function() {
	$('.model-card__pros-cons').on('click', '.pros-cons__more-btn', function(event) {
		$(this).hide();
		$(this).closest('.pros-cons__content').find('li').show();
	});

	// popular bikes carousel
	$('.carousel__popular-bikes').on('click', '.view-pros-cons__target', function() {
		$(this).closest('.model__card').addClass('card--active');
	});

});
