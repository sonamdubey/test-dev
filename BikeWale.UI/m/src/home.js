// JavaScript Document
$("a.view-more-btn").click(function(e){
	e.preventDefault();
	var a = $(this).parent().parent().find("ul.brand-style-moreBtn");
	a.slideToggle();
	$("html, body").animate({ scrollTop: $("#discoverBikesContainer").offset().top }, 1000);
	$(this).text($(this).text() == 'View more Brands' ? 'View less Brands' : 'View more Brands');
});
$("ul.brand-budget-mileage-style-UL li").click(function(){
	$("ul.brand-style-moreBtn").slideUp();
});
$("#newBikeList").on("click", function () {
    $("html, body").animate({ scrollTop: $(".new-bikes-search").offset().top - 10}, 1000);
});