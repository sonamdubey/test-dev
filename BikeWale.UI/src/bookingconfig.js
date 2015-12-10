// JavaScript Document
var validateTabB, validateTabC;

$("#configBtnWrapper").on('click', 'span.viewBreakupText', function () {
    $("div#breakupPopUpContainer").show();
    $(".blackOut-window").show();
});

$(".breakupCloseBtn,.blackOut-window").on('mouseup click', function (e) {
    $("div#breakupPopUpContainer").hide();
    $(".blackOut-window").hide();
});

$(document).on('keydown', function (e) {
    if (e.keyCode === 27) {
        $("div.breakupCloseBtn").click();
    }
});

$("#customizeBike ul.select-versionUL li").click(function () {
	if(!$(this).hasClass("selected-version")) {
		$("#customizeBike ul.select-versionUL li").removeClass("selected-version text-bold border-dark-grey").addClass("text-light-grey border-light-grey").find("span.radio-btn").removeClass("radio-sm-checked").addClass("radio-sm-unchecked");
		$(this).removeClass("text-light-grey border-light-grey").addClass("selected-version text-bold border-dark-grey").find("span.radio-btn").removeClass("radio-sm-unchecked").addClass("radio-sm-checked");
		$("#customizeBike").find("h4.select-versionh4").removeClass("text-red");
		validateTabB = 0;
		$('#financeDetailsTab').addClass('disabled-tab').removeClass('active-tab text-bold');
		$('#dealerDetailsTab').addClass('disabled-tab').removeClass('active-tab text-bold');
		
		$("#selectedVersionId").val($(this).attr('versionId'));
		//x.value = $(this).attr('versionId');
	    //$(this).find("input[type='submit']").click();
		viewModel.Bike().selectedVersionId($(this).attr('versionId'));
		viewModel.ActualSteps(1);
		
	}
});

$("#customizeBike ul.select-colorUL").on('click', "li", function () {
	if(!$(this).hasClass("selected-color")) {
		$("#customizeBike ul.select-colorUL li").removeClass("selected-color text-bold text-white border-dark-grey").addClass("text-light-grey border-light-grey");
		$("#customizeBike ul.select-colorUL li").find('span.color-title-box').removeClass().addClass('color-title-box');
		$(this).removeClass("text-light-grey border-light-grey").addClass("selected-color text-bold  border-dark-grey");
		$("#customizeBike").find("h4.select-colorh4").removeClass("text-red");
		validateTabB = 0;
		bgcolor = $(this).find('span.color-box').css('background-color');
		$(this).find('span.color-title-box').addClass(getContrastYIQ(bgcolor));
		$('#financeDetailsTab').addClass('disabled-tab').removeClass('active-tab text-bold');
		$('#dealerDetailsTab').addClass('disabled-tab').removeClass('active-tab text-bold');

		viewModel.Bike().selectedColor(9);
		viewModel.ActualSteps(1);
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

//$("#customizeBikeNextBtn").on("click", function () {
//	if($("#customizeBike ul.select-versionUL li").hasClass("selected-version")) {
//		$(".select-versionh4").removeClass("text-red");
//		if($("#customizeBike ul.select-colorUL li").hasClass("selected-color")) {
//			$(".select-colorh4").removeClass("text-red");
//			$.financeDetailsState();
//            $("#customizeBike").hide();
//			$("#financeDetails").show();
//            $("#customizeBikeTab").removeClass('text-bold');
//            $('#financeDetailsTab').removeClass('disabled-tab').addClass('text-bold active-tab');
//            $('#dealerDetailsTab').removeClass('active-tab text-bold').addClass('disabled-tab');
//			validateTabB = validateTab(1);
//		}	
//		else $(".select-colorh4").addClass("text-red");
//	}
//	else $(".select-versionh4").addClass("text-red");
//});

//$("#bookingConfigNextBtn").on("click", function () {

//    //if(viewModel.Bike().selectedVersionId()!=undefined && viewModel.Bike().selectedVersionId() > 0 )
//    //var state = $(this).attr('state'); //new
//    //if (state == 'customize') { //new
//    //    if ($("#customizeBike ul.select-versionUL li").hasClass("selected-version")) {
//    //       // $(".select-versionh4").removeClass("text-red");
//    //        if ($("#customizeBike ul.select-colorUL li").hasClass("selected-color")) {
//    //           // $(".select-colorh4").removeClass("text-red");
//    //           // $(this).attr('state', 'finance'); //new
//    //           // $.financeDetailsState();
//    //           // $("#customizeBike").hide();
//    //           // $("#financeDetails").show();
//    //           // $("#customizeBikeTab").removeClass('text-bold');
//    //            //$('#financeDetailsTab').removeClass('disabled-tab').addClass('text-bold active-tab');
//    //           // $('#dealerDetailsTab').removeClass('active-tab text-bold').addClass('disabled-tab');
//    //            //validateTabB = validateTab(1);
//    //           // $("#configBtnWrapper").find(".query-number-container").hide(); //new
//    //            //$("#configBtnWrapper .disclaimer-container").show(); //new
//    //        }
//    //        else $(".select-colorh4").addClass("text-red");
//    //    }
//    //    else $(".select-versionh4").addClass("text-red");
//    //}
//    //else if (state == 'finance') { //new
//    //    $(this).attr('state', 'dealer');
//    //    $.dealerDetailsState();
//    //    $.showCurrentTab('dealerDetails');
//    //    $('#customizeBikeTab').removeClass('disabled-tab text-bold').addClass('active-tab');
//    //    $('#financeDetailsTab').removeClass('disabled-tab text-bold').addClass('active-tab');
//    //    $('#dealerDetailsTab').removeClass('disabled-tab text-bold').addClass('active-tab text-bold');
//    //    validateTabC = validateTab(1);
//    //    $("#configBtnWrapper").find(".query-number-container").show();
//    //    $("#configBtnWrapper .disclaimer-container").hide();
//    //}
//});

//function validateTab(a) {
//	if (a == 1)
//		return true;
//	else
//		return false;
//};

//$("#customizeBikeTab").click(function () {
//    //if(viewModel.CurrentStep() > 1)
//    //{
//    //    $('#financeDetailsTab').hide()
//    //    $('#dealerDetailsTab').hide();
//    //    $(this).show();
//    //}

//    //if (!$(this).hasClass('disabled-tab')) {
//       // viewModel.CurrentStep(1);
//        //$('#bookingConfigNextBtn').attr('state', 'customize'); //new
//       // $.customizeBikeState();
//       // $.showCurrentTab('customizeBike');
//        //$('#customizeBikeTab').addClass('active-tab text-bold');
//        //if (validateTabB)
//        //    $('#financeDetailsTab').removeClass('disabled-tab text-bold').addClass('active-tab');
//        //else
//        //    $('#financeDetailsTab').removeClass('active-tab text-bold').addClass('disabled-tab');
//        //if (validateTabC)
//        //    $('#dealerDetailsTab').removeClass('disabled-tab text-bold').addClass('active-tab');
//        //else
//        //    $('#dealerDetailsTab').removeClass('active-tab text-bold').addClass('disabled-tab');
//       // $("#configBtnWrapper").find(".query-number-container").show(); //new
//        //$("#configBtnWrapper .disclaimer-container").hide(); //new
//});

//$("#financeDetailsTab").click(function () {
//    //if(viewModel.CurrentStep() > 2)
//    //{
//    //    $('#customizeBikeTab').hide()
//    //    $('#dealerDetailsTab').hide();
//    //    $(this).show();
//    //}
//    //viewModel.CurrentStep(2);
//    //if (!$(this).hasClass('disabled-tab')) {
//    //    $('#bookingConfigNextBtn').attr('state', 'finance'); //new
//    //    $.financeDetailsState();
//    //    $.showCurrentTab('financeDetails');
//    //    $('#customizeBikeTab').removeClass('disabled-tab text-bold').addClass('active-tab');
//    //    if (validateTabB)
//    //        $('#financeDetailsTab').removeClass('disabled-tab').addClass('active-tab text-bold');
//    //    else
//    //        $('#financeDetailsTab').removeClass('active-tab text-bold').addClass('disabled-tab');
//    //    if (validateTabC)
//    //        $('#dealerDetailsTab').removeClass('disabled-tab text-bold').addClass('active-tab');
//    //    else
//    //        $('#dealerDetailsTab').removeClass('active-tab text-bold').addClass('disabled-tab');
//    //    $("#configBtnWrapper").find(".query-number-container").hide(); //new
//    //    $("#configBtnWrapper .disclaimer-container").show(); //new
//    //}
//});

//$('#dealerDetailsTab').click(function () {
//    //if(viewModel.CurrentStep() >= 3)
//    //{
//    //    $('#financeDetailsTab').hide()
//    //    $('#customizeBikeTab').hide();
//    //    $(this).show();
//    //}
//   // viewModel.CurrentStep(3);
//    //if (!$(this).hasClass('disabled-tab')) {
//    //    $('#bookingConfigNextBtn').attr('state', 'dealer'); //new
//    //    $.dealerDetailsState();
//    //    $.showCurrentTab('dealerDetails');
//    //    $('#customizeBikeTab').removeClass('disabled-tab text-bold').addClass('active-tab');
//    //    $('#financeDetailsTab').removeClass('disabled-tab text-bold').addClass('active-tab');
//    //    $('#dealerDetailsTab').removeClass('disabled-tab text-bold').addClass('active-tab text-bold');
//    //    $("#configBtnWrapper").find(".query-number-container").show(); //new
//    //    $("#configBtnWrapper .disclaimer-container").hide(); //new
//    //}
//});

//$.showCurrentTab = function (tabType) {
//    $('#customizeBike,#financeDetails,#dealerDetails').hide();
//    $('#' + tabType).show();
//};

//$.customizeBikeState = function () {
//    var container = $('#configTabsContainer ul');
//    container.find('li:eq(0)').find('span.booking-config-icon').attr('class', '').attr('class', 'booking-sprite booking-config-icon customize-icon-selected');
//    container.find('li:eq(1)').find('span.booking-config-icon').attr('class', '').attr('class', 'booking-sprite booking-config-icon finance-icon-grey');
//    container.find('li:eq(2)').find('span.booking-config-icon').attr('class', '').attr('class', 'booking-sprite booking-config-icon confirmation-icon-grey');
//};

//$.financeDetailsState = function () {
//    var container = $('#configTabsContainer ul');
//    container.find('li:eq(0)').find('span.booking-config-icon').attr('class', '').attr('class', 'booking-sprite booking-config-icon booking-tick-blue');
//    container.find('li:eq(1)').find('span.booking-config-icon').attr('class', '').attr('class', 'booking-sprite booking-config-icon finance-icon-selected');
//    container.find('li:eq(2)').find('span.booking-config-icon').attr('class', '').attr('class', 'booking-sprite booking-config-icon confirmation-icon-grey');
//};

//$.dealerDetailsState = function () {
//    var container = $('#configTabsContainer ul');
//    container.find('li:eq(0)').find('span.booking-config-icon').attr('class', '').attr('class', 'booking-sprite booking-config-icon booking-tick-blue');
//    container.find('li:eq(1)').find('span.booking-config-icon').attr('class', '').attr('class', 'booking-sprite booking-config-icon booking-tick-blue');
//    container.find('li:eq(2)').find('span.booking-config-icon').attr('class', '').attr('class', 'booking-sprite booking-config-icon confirmation-icon-selected');
//};

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

function getContrastYIQ(colorCode) {

    if (/rgb/i.test(colorCode))
    {
        l = colorCode.length;
        colorCode = colorCode.substr(4, l - 1);
        b = colorCode.split(",");
        R = parseInt(b[0], 16);
        G = parseInt(b[1], 16);
        B = parseInt(b[2], 16);

        yiq = ((R * 299) + (G * 587) + (B * 114)) / 1000;
        return (yiq >= 300) ? 'text-black' : 'text-white';
        //return Brightness(Math.sqrt(R * R * .241 + G * G * .691 + B * B * .068)) < 130 ? '#FFFFFF' : '#000000';

    }
    else if (/#/i.test(colorCode))
    {
        vl = colorCode.length;
        colorCode = colorCode.substr(1, l - 1);
        r = parseInt(colorCode.substr(0, 2), 16);
        g = parseInt(colorCode.substr(2, 2), 16);
        b = parseInt(colorCode.substr(4, 2), 16);
         yiq = ((r * 299) + (g * 587) + (b * 114)) / 1000;
         return (yiq >= 300) ? 'text-black' : 'text-white';

    }
    else {
        return "text-light-grey";
    }
    
}

