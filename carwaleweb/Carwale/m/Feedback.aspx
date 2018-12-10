<%@ Page Language="C#" ContentType="text/html" Inherits="MobileWeb.Feedback"  AutoEventWireup="false" trace="false" %>
<% IsOldJquery = "false"; %>
<% Title = "Feedback - CarWale"; %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" itemscope itemtype="http://schema.org/WebPage">
<head>
<!-- #include file="includes/global-scripts.aspx" -->
<link rel="stylesheet" href="/static/m/css/design.css" type="text/css" >
<style>
    .rdoText {color:#252628;font-weight:normal;}
</style>
</head>

<body>
	<!--Outer div starts here-->
	<div data-role="page" >
    	<!--Main container starts here-->
    	<div id="main-container">
			<!-- #include file="includes/global-header.aspx" -->
            <form runat="server">
            <asp:HiddenField id="hdnRdoFeedback" value="" runat="server"></asp:HiddenField>
            <div class="pgsubhead">Feedback</div>
            <div class="box new-line5">
                <div class="new-line5"><asp:TextBox ID="txtName" runat="server" placeholder="Your Name*" /></div>
                <div class="new-line15"><asp:TextBox ID="txtEmail" runat="server" type="email" placeholder="Your Email*" /></div>
                <div class="new-line15">
                    <input id="numMobile" type="Tel" placeholder="Mobile No." /> 
                    <asp:TextBox ID="txtMobile" runat="server" style="display:none;" data-role="none" />
                </div>
                <div class="new-line15">
                    <fieldset data-role="controlgroup">
                        <input type="radio" name="rdoFeedBackType" class="rdoFeedBackType" id="radio-choice-v-2a" onchange="ChangeRdoText(this)" value="Feedback">
                        <label for="radio-choice-v-2a" style="font-weight:normal;">Feedback</label>
                        <input type="radio" name="rdoFeedBackType" class="rdoFeedBackType" id="radio-choice-v-2b" onchange="ChangeRdoText(this)" value="Complaint">
                        <label for="radio-choice-v-2b" style="font-weight:normal;">Complaint</label>
                    </fieldset>
                </div>
                <div class="new-line15"><asp:TextBox id="txtDesc" placeholder="Description*" TextMode="multiline" runat="server"></asp:TextBox></div>
                <div class="new-line15">
                    <asp:LinkButton  id="btnSubmit" runat="server" data-theme="b" Text="Submit" data-rel="popup" data-role="button" data-transition="pop" data-position-to="window"/>
                </div>
            </div>
            <div data-role="popup" id="popupDialog" data-overlay-theme="a" data-theme="c" data-dismissible="false"  class="ui-corner-all">
                <div data-role="header" data-theme="a" class="ui-corner-top">
                    <h1>Error !!</h1>
                </div>
                <div data-role="content" data-theme="d" class="ui-corner-bottom ui-content">
                    <span id="spnError" style="font-size:14px;line-height:20px;" class="error"></span>
                    <a href="#" data-role="button" data-rel="back" data-theme="c" data-mini="true">OK</a>
                </div>
            </div>
            </form>
            <script language="javascript" type="text/javascript">
                $(document).ready(function () {
                   
                });
            
                function ChangeRdoText(rdoFeedback)
                {
                    $("#hdnRdoFeedback").val($(rdoFeedback).val());
                    //$("label span span").removeClass("rdoText");       
                    //$("label span span").addClass("rdoText");
                    //alert($("label span span").attr("class"));
                }
            
                function IsValid() {
                    var retVal = true;
                    var errorMsg = "";
            
                    var reEmail = /^[a-z]+(([a-z_0-9]*)|([a-z_0-9]*\.[a-z_0-9]+))*@([a-z_0-9\-]+)((\.[a-z]{3})|((\.[a-z]{2})+)|(\.[a-z]{3}(\.[a-z]{2})+))$/;
                    var reMobile = /^[0-9]*$/;
            
                    $("#txtMobile").val($("#numMobile").val());
            
                    if ($("#txtName").val().trim() == "") {
                        retVal = false;
                        errorMsg += "Please enter your name<br>";
                    }
            
                    var _email = $("#txtEmail").val().trim().toString().toLowerCase();
                    if (_email == "") {
                        retVal = false;
                        errorMsg += "Please enter your email<br>";
                    }
                    else if (!reEmail.test(_email)) {
                        retVal = false;
                        errorMsg += "Invalid Email<br>";
                    }
            
                    var _custMobile = $("#txtMobile").val().trim();
                    //if (_custMobile == "") {
                    //    retVal = false;
                    //    errorMsg += "Please enter your mobile number<br>";
                    //}
                    if (!reMobile.test(_custMobile)) {
                        retVal = false;
                        errorMsg += "Mobile number should be numeric<br>";
                    }
                    else if (_custMobile.length != 0 && _custMobile.length != 10) {
                        retVal = false;
                        errorMsg += "Mobile number required 10 digits<br>";
                    }
            
                    if ($(".rdoFeedBackType:checked").length == 0) {
                        retVal = false;
                        errorMsg += "Please select feedback type<br>";
                    }
            
                    if ($("#txtDesc").val().trim() == "") {
                        retVal = false;
                        errorMsg += "Please enter description<br>";
                    }
                    if (retVal == false) {
                        $("#spnError").html(errorMsg);
                        $("#popupDialog").popup("open");
                    }
                    
                    return retVal;
                 }
            </script>
            <!-- #include file="includes/Footer-New.aspx" -->
        </div>
        <!--Main container ends here-->
    </div>
    <!--Outer div ends here-->
</body>
</html>