docReady(function() {
	$('.model-card__pros-cons').on('click', '.pros-cons__more-btn', function(event) {
		$(this).hide();
		$(this).closest('.pros-cons__content').find('li').show();
	});
});
