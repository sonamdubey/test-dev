// JavaScript Document
$("a.view-more-btn").click(function(e){
	e.preventDefault();
	var a = $(this).parent().parent().find("ul.brand-style-moreBtn");
	a.slideToggle();
});
$("ul.brand-budget-mileage-style-UL li").click(function(){
	$("ul.brand-style-moreBtn").slideUp();
});