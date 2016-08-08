<div id="sybh-list">
    <div>
		<a class="feedback-tab"></a>
    </div>
</div>
<div class="feedback-popup content-block hide">
    <div class="close-btn-fback"><a href="#">&times;</a></div>
    <div class="feedback-form">
        <span>Please share your feedback:</span>
        <textarea id="txtFeedbackComment" placeholder="Type here..."></textarea>
        <div id="errFeedback" class="left-float margin-top15 error"></div>
        <input type="button" value="Send Feedback" class="action-btn right-float margin-top10" id="sendFeedback" />
        <div class="clear"></div>
    </div>
    <div class="thankyou-msg">
        <span>Wow, you're awesome!</span>
        <div class="margin-top5">Thanks for your time and insights. We look forward to building great products for you.</div>
    </div>
    <span class="popup-point"></span>
</div>
<script type="text/javascript">
    $(document).ready(function () {
        var feedbackId = 16;
        var feedbackComments = null;
        var pageUrl = window.location.href;

		$(".feedback-popup").hide();
		$(".thankyou-msg").hide();
		$(".feedback-popup").on('click',function(e){
			e.stopPropagation();
		});
		$(".feedback-tab").click(function(e){
			e.preventDefault();
			e.stopPropagation();
			$(".feedback-popup").fadeIn("fast");
			$("#txtFeedbackComment").focus();
		});
		$(document).on('click', function(){
			$(".feedback-popup").fadeOut("fast");
		});
		$(".close-btn-fback").click(function(){
			$(".feedback-popup").fadeOut("fast");
		});
		$("#sendFeedback").click(function () {
		    if (!ValidateFeedback()) {
		        $("#sendFeedback").val("Processing..").attr('disabled', 'disabled');;
		        $.ajax({
		            type: "POST",
		            url: "/ajaxpro/Bikewale.Ajax.AjaxCommon,Bikewale.ashx",
		            data: '{"feedbackType":"' + feedbackId + '", "feedbackComment":"' + feedbackComments + '", "platformId":"1", "pageUrl" : "' + pageUrl + '"}',
		            beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "SaveCustomerFeedback"); },
		            success: function (response) {
		                var responseJSON = eval('(' + response + ')');
		                var resObj = eval('(' + responseJSON.value + ')');

		                if (resObj == true) {
		                    $(".thankyou-msg").show();
		                    $(".feedback-form").hide();
		                    $(".feedback-popup").css({ 'top': '35%' });
		                }
		                else {
		                    $(".feedback-form").show();
		                }
		            }
		        });

		        
		    }
		});
		//setTimeout(function () { $(".feedback-popup").fadeOut() }, 5000);

		function ValidateFeedback()
		{
		    var isError = false;
		    $("#errFeedback").html("");

		    feedbackComments = $("#txtFeedbackComment").val().trim();

		    var arrFeedbackLineBreak = feedbackComments.match(/\n/g);
		    var arrFeedbackDoubleQuote = feedbackComments.match(/"/g);
		    var feedbackCommentsLength = feedbackComments.length;
		   
		    if (feedbackComments != "") {
		        if (arrFeedbackLineBreak != "" && arrFeedbackLineBreak != null)
		            feedbackCommentsLength += arrFeedbackLineBreak.length;
		        
		        if (feedbackCommentsLength > 500) {
		            $("#errFeedback").text("Exceeded maximum 500 character limit. Please remove some characters.");
		            $("#txtFeedbackComment").focus();
		            isError = true;
		        }
		    }
		    else {
		        $("#errFeedback").text("Required");
		        isError = true;
		    }
		    if (arrFeedbackDoubleQuote != "" && arrFeedbackDoubleQuote != null) {
		        if (arrFeedbackDoubleQuote.length > 0) {
		            $("#errFeedback").text("Invalid input.");
		            isError = true;
		        }
		    }

		    return isError;
		}
	});
</script>

