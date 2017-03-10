// JavaScript Document
$(document).ready(function(){
	$(window).scroll(function () {
		if ($(this).scrollTop() > 40) {
			$('.header-fixed').addClass('header-fixed-with-bg');
		} else {
			$('.header-fixed').removeClass('header-fixed-with-bg');
		}
	});
});
