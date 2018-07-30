// JavaScript Document
$(document).ready(function(){
	/*
	$(document).mouseup(function (e)
	{
		var container = $(".filter-selection-div");
		
		if (container.hasClass('open')) {
			if (!container.is(e.target) && container.has(e.target).length === 0) {
			{
				//var a = container.prev("div").attr("id");
				var a = $('#' + e.target.id).prev("div").attr('class');
				if(a != "filter-div"){
					$('.filter-div').trigger('click');
				}
			}
		}
		}
	});
	*/
	
	
	function stickyFilter(){
		var windowTop = $(window).scrollTop();
		var stickyTop = $('#filter-container').offset().top;
		
		$(window).scroll(function(){
			if (stickyTop < windowTop) {
			  $('#filter-container').addClass("stick");
			}
			else {
			  $('#filter-container').removeClass("stick");
			}
		});
	};
	
	$(window).scroll(stickyFilter);
    stickyFilter();
	
});

$(".filter-div").click(function(){
	var allDiv = $(".filter-div");
	var clickedDiv = $(this);
	if(!clickedDiv.hasClass("open")) {
		stateChangeDown(allDiv,clickedDiv);
		$(".more-filters-container").slideUp();
	}
	else {
		stateChangeUp(allDiv,clickedDiv);	
	}
	
});

function stateChangeDown(allDiv,clickedDiv){
	allDiv.removeClass("open");
	allDiv.next(".filter-selection-div").slideUp();
	$(clickedDiv).addClass("open");
	$(clickedDiv).next(".filter-selection-div").slideDown();
	$(clickedDiv).next(".filter-selection-div").addClass("open");
};

function stateChangeUp(allDiv,clickedDiv){
	clickedDiv.removeClass("open");
	allDiv.removeClass("open");
	clickedDiv.next(".filter-selection-div").slideUp();
	$(clickedDiv).next(".filter-selection-div").removeClass("open");	
};


var appendText = $(".filter-select-title");
var currentList = $(".filter-selection-div");
var liList = $(".filter-selection-div ul li");
var defaultText = $(".default-text");
liList.click(function(){
	
	var clickedLI = $(this);
	var clickedLIText = clickedLI.text();
	var fid = clickedLI.attr("filterId");
	var a = clickedLI.parent().parent().prev(".filter-div");
	if(clickedLI.hasClass("uncheck")){
		clickedLI.removeClass("uncheck").addClass("active");
		a.find(defaultText).hide();
		a.find(appendText).append("<span class='selected' filterId="+ fid +">" + clickedLIText + "</span>");
	}
	else {
		$(".selected").each(function(){
			if(fid == $(this).attr("filterId")){
				clickedLI.removeClass("active").addClass("uncheck");	
				$(this).remove();
			}
		});
		var p = appendText.children().length;
		if(p == 12){
			a.find(defaultText).show();
			currentList.slideUp();
			$(".filter-div").removeClass("open");
		}
	}
	
});


$(".more-filters-btn").click(function(){
	$(".more-filters-container").slideToggle();
	var a = $(".filter-div");
	a.removeClass("open");
	a.next(".filter-selection-div").slideUp();
});

$(".filter-done-btn").click(function(){
	$(".more-filters-container").slideUp();
	stateChangeUp($('.filter-div'),$('.filter-div'));
});

$(".filter-reset-btn").click(function(){
	$(".selected").remove();
	$(".filter-selection-div li").each(function(){
		$(this).removeClass("active").addClass("uncheck");
	});
	defaultText.show();
	$(".more-filters-container").slideUp();
});

/*
$("body").click(function(e) {
    if($(this).attr("class") != "filter-div") {
        //var a = document.getElementsByClassName('filter-div');
		alert("outside");
    }
	else {
		alert("inside");	
	}
});
*/


