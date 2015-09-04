// JavaScript Document
$(document).ready(function(){
	$(window).scroll(function () {
		if ($(this).scrollTop() > 40) {
			$('.header-fixed').addClass('header-fixed-with-bg');
		} else {
			$('.header-fixed').removeClass('header-fixed-with-bg');
		}
	});
	
	$("#more-brand-tab").click(function(e) {
		e.preventDefault();
		var a = $(this).parent().parent().find("#more-brand-nav");
		a.slideToggle();
		$("html, body").animate({ scrollTop: $("#more-brand-nav").offset().top }, 1000);
		var b = $(this).find("span");
		b.text(b.text() === "more" ? "less" : "more");
	});
});
