// JavaScript Document
$(document).ready(function() {
	
	$("a.view-more-btn").click(function(e){
		e.preventDefault();
		var a,b,c;
		a = $(this).parent().parent().find("ul.brand-style-moreBtn");
		b = $(this).find("span");
		c = $(".brand-bottom-border");
		a.slideToggle();
		b.text(b.text() === "more" ? "less" : "more");
		c.slideToggle();
	});
	$("ul.brand-budget-mileage-style-UL li").click(function(){
		var a,b;
		a = $(".view-more-btn").find("span");
		b = $(".brand-bottom-border");
		a.text("More");
		$("ul.brand-style-moreBtn").slideUp();
		b.slideUp();
	});
	changeHeaderColor();

});

$(window).on("scroll", function () {
    changeHeaderColor();
});

var changeHeaderColor = function () {
    if ($(window).scrollTop() > 40)
        $('#header').removeClass("header-landing").addClass("header-fixed");
    else
        $('#header').removeClass("header-fixed").addClass("header-landing");
}