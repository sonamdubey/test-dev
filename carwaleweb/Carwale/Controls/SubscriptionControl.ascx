<%@ Control Language="C#" Inherits="Carwale.UI.Controls.SubscriptionControl" AutoEventWireUp="false" %>
<style type="text/css">
   .ul-alerts{ list-style:none; }
   .ul-alerts li{ float:left; padding:5px; padding-left: 0px;}
   #txtUpcomingAlertEmail{font-size:11px; font-style:italic;}
   
</style>
<div class="content-box-shadow content-inner-block-10">
   <div id="divSubscription">
      <h3 class="font16"><asp:Literal ID="subscriptionHeading" runat="server"></asp:Literal></h3>
      <ul class="ul-alerts margin-top10">
         <li class="heading grid-4" ><span style="line-height: 1.9;"><asp:Literal ID="subscriptionLabel" runat="server" ></asp:Literal></span></li>
         <li class="grid-5"><input type="text" class="form-control" id="txtUpcomingAlertEmail" value="Enter your email address" placeholder="Enter your email address" /></li>
         <li class="grid-3"><input type="button" class="btn btn-orange btn-xs" id="btnSubscribe" value="Subscribe" /></li>
      </ul>
      <div class="clear"></div>
   </div>
</div>
<script type="text/javascript">
   $("#txtUpcomingAlertEmail").click(function () {
       if ($("#txtUpcomingAlertEmail").val() == "Enter your email address") {
           $("#txtUpcomingAlertEmail").val("");
       }            
   }).focus(function () {
       if ($("#txtUpcomingAlertEmail").val() == "Enter your email address") {
           $("#txtUpcomingAlertEmail").val("");
       }
   }).blur(function () {
       $("#txtUpcomingAlertEmail").val($("#txtUpcomingAlertEmail").val().trim());
       if ($("#txtUpcomingAlertEmail").val() == "") {
           $("#txtUpcomingAlertEmail").val("Enter your email address");
       }
   });
   $(document).ready(function(){
       $("#btnSubscribe").click(function (e) {
           e.preventDefault();
           if (validateEmailAddress()) {
               Subscribe();
           }
           return false;
       });
       $(window).keydown(function(event){
           if(event.keyCode == 13) {
               event.preventDefault();
               return false;
           }
       });
       $('#txtUpcomingAlertEmail').on('keyup', function(e) {          
           e.preventDefault();
           if (e.keyCode == 13) {            
               if (validateEmailAddress()) {
                   return Subscribe();
               }               
           }
           return false;
       });
   });
  
   $("#btnSubscribe").ajaxStart(function () {
       $('#ajaxBusy').show();
   });
   function validateEmailAddress() {
       var emailPattern = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;
       var email = $("#txtUpcomingAlertEmail").val();
       var error = false;
       if (email != null && email != "") {
           if (email != $("#txtUpcomingAlertEmail").attr("placeholder")) {
               if (!emailPattern.test(email)) {
                   alert("Please enter a valid email");
                   error = true;
               }
           } else {
               alert("Please enter your email");
               error = true;
           }
       } else {
           alert("Please enter your email");
           error = true;
       }
       if (error == false)
       return true
       else
       return false
   }
   
   function Subscribe() {
       $('#divSubscription').append('<label id="ajaxBusy" style="display:none;margin:0px;font-size:10px">processing...</label>');
       var subscriptionCategory = <%= SubscriptionCategory%>;
       var subscriptionType = <%= SubscriptionType %>;
       $.ajax({
           type: "POST", url: "/ajaxpro/CarwaleAjax.AjaxResearch,Carwale.ashx",
           data: '{"emailAddress":"' + $("#txtUpcomingAlertEmail").val() + '", "subscriptionCategory":"' + subscriptionCategory + '", "subscriptionType":"' + subscriptionType + '"}',
           beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "Subscribe"); },
           success: function (response) {
               $('#ajaxBusy').hide();
               var responseJSON = eval('(' + response + ')');
               if (responseJSON.value == false) {
                   alert("You are already subscribed");
               }
               else {
                   alert("You are successfully subscribed");
               }
           }
       });

       return false;
   }
</script>