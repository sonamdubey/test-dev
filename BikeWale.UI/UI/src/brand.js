// JavaScript Document
$("a.read-more-btn").click(function(){
	$(".brand-about-more-desc").toggle();
	var a = $(this).find("span");
	a.text(a.text() === "more" ? "less" : "more");
});