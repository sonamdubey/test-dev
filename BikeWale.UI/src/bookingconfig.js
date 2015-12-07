// JavaScript Document
var validateTabB, validateTabC;

$("#customizeBike ul.select-versionUL li").click(function () {
	if(!$(this).hasClass("selected-version")) {
		$("#customizeBike ul.select-versionUL li").removeClass("selected-version text-bold border-dark-grey").addClass("text-light-grey border-light-grey").find("span.radio-btn").removeClass("radio-sm-checked").addClass("radio-sm-unchecked");
		$(this).removeClass("text-light-grey border-light-grey").addClass("selected-version text-bold border-dark-grey").find("span.radio-btn").removeClass("radio-sm-unchecked").addClass("radio-sm-checked");
		$("#customizeBike").find("h4.select-versionh4").removeClass("text-red");
		validateTabB = 0;
		$('#financeDetailsTab').addClass('disabled-tab').removeClass('active-tab text-bold');
	}
});

$("#customizeBike ul.select-colorUL li").click(function () {
	if(!$(this).hasClass("selected-color")) {
		$("#customizeBike ul.select-colorUL li").removeClass("selected-color text-bold text-white border-dark-grey").addClass("text-light-grey border-light-grey");
		$(this).removeClass("text-light-grey border-light-grey").addClass("selected-color text-bold text-white border-dark-grey");
		$("#customizeBike").find("h4.select-colorh4").removeClass("text-red");
		validateTabB = 0;
		$('#financeDetailsTab').addClass('disabled-tab').removeClass('active-tab text-bold');
	}
});

$("#financeDetails ul.select-financeUL li").click(function () {
	if(!$(this).hasClass("selected-finance")) {
		$("#financeDetails ul.select-financeUL li").removeClass("selected-finance text-bold border-dark-grey").addClass("text-light-grey border-light-grey").find("span.radio-btn").removeClass("radio-sm-checked").addClass("radio-sm-unchecked");
		$(this).removeClass("text-light-grey border-light-grey").addClass("selected-finance text-bold border-dark-grey").find("span.radio-btn").removeClass("radio-sm-unchecked").addClass("radio-sm-checked");
		$("#financeDetails").find("h4.select-financeh4").removeClass("text-red");
		validateTabC = 0;
		$('#dealerDetailsTab').addClass('disabled-tab').removeClass('active-tab text-bold');
	}
});

$("#financeDetails ul.select-financeUL li").click(function(){
	if($(this).hasClass("finance-required"))
		$(".finance-emi-container").show();
	else $(".finance-emi-container").hide();
});

$("#customizeBikeNextBtn").on("click", function () {
	if($("#customizeBike ul.select-versionUL li").hasClass("selected-version")) {
		$(".select-versionh4").removeClass("text-red");
		if($("#customizeBike ul.select-colorUL li").hasClass("selected-color")) {
			$(".select-colorh4").removeClass("text-red");
			$.financeDetailsState();
            $("#customizeBike").hide();
			$("#financeDetails").show();
            $("#customizeBikeTab").removeClass('text-bold');
            $('#financeDetailsTab').removeClass('disabled-tab').addClass('text-bold active-tab');
            $('#dealerDetailsTab').removeClass('active-tab text-bold').addClass('disabled-tab');
			validateTabB = validateTab(1);
		}	
		else $(".select-colorh4").addClass("text-red");
	}
	else $(".select-versionh4").addClass("text-red");
});

function validateTab(a) {
	if (a == 1)
		return true;
	else
		return false;
};

$("#customizeBikeTab").click(function () {
    if (!$(this).hasClass('disabled-tab')) {
        $.customizeBikeState();
        $.showCurrentTab('customizeBike');
        $('#customizeBikeTab').addClass('active-tab text-bold');
		if(validateTabB)
        	$('#financeDetailsTab').removeClass('disabled-tab text-bold').addClass('active-tab');
		else
			$('#financeDetailsTab').removeClass('active-tab text-bold').addClass('disabled-tab');
		if(validateTabC)
			$('#dealerDetailsTab').removeClass('disabled-tab text-bold').addClass('active-tab');
		else
        	$('#dealerDetailsTab').removeClass('active-tab text-bold').addClass('disabled-tab');
    }
});

$("#financeDetailsTab").click(function () {
    if (!$(this).hasClass('disabled-tab')) {
        $.financeDetailsState();
        $.showCurrentTab('financeDetails');
		$('#customizeBikeTab').removeClass('disabled-tab text-bold').addClass('active-tab');
        if(validateTabB)
			$('#financeDetailsTab').removeClass('disabled-tab').addClass('active-tab text-bold');
		else
			$('#financeDetailsTab').removeClass('active-tab text-bold').addClass('disabled-tab');
        if(validateTabC)
        	$('#dealerDetailsTab').removeClass('disabled-tab text-bold').addClass('active-tab');
		else
			$('#dealerDetailsTab').removeClass('active-tab text-bold').addClass('disabled-tab');
    }
});

$("#financeDetailsNextBtn").click(function () {
	validateTabC = 1;	
	$.dealerDetailsState();
	$.showCurrentTab('dealerDetails');
	$('#customizeBikeTab').removeClass('disabled-tab text-bold').addClass('active-tab');
	$('#financeDetailsTab').removeClass('disabled-tab text-bold').addClass('active-tab');
	$('#dealerDetailsTab').removeClass('disabled-tab text-bold').addClass('active-tab text-bold');
});

$('#dealerDetailsTab').click(function () {
    if (!$(this).hasClass('disabled-tab')) {
        $.dealerDetailsState();
        $.showCurrentTab('dealerDetails');
        $('#customizeBikeTab').removeClass('disabled-tab text-bold').addClass('active-tab');
		$('#financeDetailsTab').removeClass('disabled-tab text-bold').addClass('active-tab');
		$('#dealerDetailsTab').addClass('active-tab text-bold');
    }
});

$.showCurrentTab = function (tabType) {
    $('#customizeBike,#financeDetails,#dealerDetails').hide();
    $('#' + tabType).show();
};

$.customizeBikeState = function () {
    var container = $('#configTabsContainer ul');
    container.find('li:eq(0)').find('span.booking-config-icon').attr('class', '').attr('class', 'booking-sprite booking-config-icon customize-icon-selected');
    container.find('li:eq(1)').find('span.booking-config-icon').attr('class', '').attr('class', 'booking-sprite booking-config-icon finance-icon-grey');
    container.find('li:eq(2)').find('span.booking-config-icon').attr('class', '').attr('class', 'booking-sprite booking-config-icon confirmation-icon-grey');
};

$.financeDetailsState = function () {
    var container = $('#configTabsContainer ul');
    container.find('li:eq(0)').find('span.booking-config-icon').attr('class', '').attr('class', 'booking-sprite booking-config-icon booking-tick-blue');
    container.find('li:eq(1)').find('span.booking-config-icon').attr('class', '').attr('class', 'booking-sprite booking-config-icon finance-icon-selected');
    container.find('li:eq(2)').find('span.booking-config-icon').attr('class', '').attr('class', 'booking-sprite booking-config-icon confirmation-icon-grey');
};

$.dealerDetailsState = function () {
    var container = $('#configTabsContainer ul');
    container.find('li:eq(0)').find('span.booking-config-icon').attr('class', '').attr('class', 'booking-sprite booking-config-icon booking-tick-blue');
    container.find('li:eq(1)').find('span.booking-config-icon').attr('class', '').attr('class', 'booking-sprite booking-config-icon booking-tick-blue');
    container.find('li:eq(2)').find('span.booking-config-icon').attr('class', '').attr('class', 'booking-sprite booking-config-icon confirmation-icon-selected');
};

var sliderComponentA, sliderComponentB;

$(document).ready(function(e) {
	
	sliderComponentA = $("#downPaymentSlider").slider({
		range: "min",
		min: 0,
		max: 1000000,
		step: 50000,
		value: 50000,
		slide: function( e, ui ) {
			changeComponentBSlider(e,ui);
		},
		change: function(e, ui) {
			changeComponentBSlider(e,ui);
		}
	})
	
	sliderComponentB = $("#loanAmountSlider").slider({
		range: "min",
		min: 0,
		max: 1000000,
		step: 50000,
		value: 1000000 - $('#downPaymentSlider').slider("option", "value"),
		slide: function( e, ui ) {
			changeComponentASlider(e,ui);
		},
		change: function(e, ui) {
			changeComponentASlider(e,ui);
		}
	});
	
	$("#tenureSlider").slider({
		range: "min",
		min: 12,
		max: 84,
		step: 6,
		value: 36,
		slide: function(e,ui) {
			$("#tenurePeriod").text(ui.value);
		}
	});
	
	$("#rateOfInterestSlider").slider({
		range: "min",
		min: 0,
		max: 20,
		step: 0.25,
		value: 5,
		slide: function(e,ui) {
			$("#rateOfInterestPercentage").text(ui.value);
		}
	});
	
	$("#downPaymentAmount").text($("#downPaymentSlider").slider("value"));
	$("#loanAmount").text($("#loanAmountSlider").slider("value"));
	$("#tenurePeriod").text($("#tenureSlider").slider("value"));
	$("#rateOfInterestPercentage").text($("#rateOfInterestSlider").slider("value"));

});

function changeComponentBSlider(e,ui) {
	if (!e.originalEvent) return;
	var totalAmount = 1000000;
	var amountRemaining = totalAmount - ui.value;
	$('#loanAmountSlider').slider("option", "value", amountRemaining);
	$("#loanAmount").text(amountRemaining);
	$("#downPaymentAmount").text(ui.value);
};

function changeComponentASlider(e,ui) {
	if (!e.originalEvent) return;
	var totalAmount = 1000000;
	var amountRemaining = totalAmount - ui.value;
	$('#downPaymentSlider').slider("option", "value", amountRemaining);
	$("#downPaymentAmount").text(amountRemaining);
	$("#loanAmount").text(ui.value);
};
