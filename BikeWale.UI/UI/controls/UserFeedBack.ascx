<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.UserFeedBack" %>
<%@ Import Namespace="Bikewale.Common" %>
<script type="text/javascript" src="/ajax/common.ashx"></script>
<script type="text/javascript" src="/ajax/Vspl.Common.AjaxFunctions,Carwale.Common.ashx"></script>
<script type="text/javascript" language="javascript" src="/UI/src/AjaxFunctions.js?v=1.0"></script>
<div id="divFeedback">
	<h2 class="greyboxtitle">Feedback</h2>
	<p>What is your opinion about this  page?</p>
	<div style="float:left;">
		<p><input id="rdoHelpful" type="radio" value="Helpful" name="feedback"/>Helpful</p>
		<p><input id="rdoJustOk" type="radio" value="Just Ok" name="feedback" />Average</p>
		<p><input id="rdoPoor" type="radio" value="Poor" name="feedback"/>Poor</p>
		<p><label><textarea name="textarea" id="txtComments" cols="14" rows="5"></textarea></label></p>
		<div style="padding-right:10px">
			<div class="buttons1">
				<input id="btnSubmit" type="button" class="buttons" onclick="SaveUserFeedback()" value="Submit"/>
			</div>
		</div>
	</div>
</div>
<div id="divFeedbackMsg" style="font-weight:bold;"></div>

<script language="javascript">
    function SaveUserFeedback(){
        var helpFull = document.getElementById("rdoHelpful");
        var justOk = document.getElementById("rdoJustOk");
        var poor = document.getElementById("rdoPoor");
		
		
        if( helpFull.checked || justOk.checked || poor.checked ){
            var comments = document.getElementById("txtComments").value;
            var sourceURL = "<%=Request.ServerVariables["URL"]%>";
            var userAgent = "<%=Request.ServerVariables["HTTP_USER_AGENT"]%>";
            var clientIP = "<%=Request.ServerVariables["REMOTE_ADDR"]%>";
            var customerId = <%=CurrentUser.Id%>
			
            var feedback;
			
            if( helpFull.checked )
                feedback = helpFull.value;
				
            if( justOk.checked )
                feedback = justOk.value;
				
            if( poor.checked )
                feedback = poor.value;
			
            var response = AjaxFunctions.SaveUserFeedback(customerId, feedback, comments, sourceURL, clientIP, userAgent);
			
            if(response = 1){
                document.getElementById("divFeedback").style.display = "none";
                document.getElementById("divFeedbackMsg").innerHTML = "Thank you for your valuable feedback. It will help us improve";
            }
        }else
            alert("Please select atleast one feedback.");
    }
</script>
