// JavaScript Document
$(document).ready(function() {
	var availableTags = [
		"Bajaj 1",
		"Bajaj 2",
		"Bajaj 3",
		"Bajaj 4",
		"Bajaj 5"
	];
	
	$("#finalPriceBikeSelect").autocomplete({
		source: function(request, response) {
			dataListDisplay(availableTags,request, response);
		},minLength: 1
	}).css({'width':'290px'});
	$("#locateDealerBikeSelect").autocomplete({
		source: function(request, response) {
			dataListDisplay(availableTags,request, response);
		},minLength: 1
	}).css({'width':'290px'});
	
	function dataListDisplay(availableTags,request,response){
		var results = $.ui.autocomplete.filter(availableTags, request.term);
		response(results.slice(0, 5));
	}		
	
	$("a.view-more-btn").click(function(e){
		e.preventDefault();
		var a,b,c;
		a = $(this).parent().parent().find("ul.brand-style-moreBtn");
		b = $(this).find("span");
		c = $(".brand-bottom-border");
		a.slideToggle();
		b.text(b.text() === "More" ? "Less" : "More");
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

});