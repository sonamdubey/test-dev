<%@ Page Language="C#" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <div id="requester_form">
        <p class="font11 light-grey-text">Request the seller to upload photos of this car</p>           
        <table width="100%" cellpadding="5" cellspacing="0" border="0" class="margin-top10">
            <tr>
                <td>Name</td>
                <td class="txtBNameInput"><input class="form-control" type="text" id="txtBName" runat="server" style="width:180px" name="name" />
                    <p class="text-red font10 margin-top-20" id="txtBNameError"></p>
                </td>
            </tr>
            <tr>
                <td>Email</td>
                <td class="txtBEmailInput"><input class="form-control" type="text" id="txtBEmail" runat="server" style="width:180px" name="email" />
                    <p class="text-red font10 margin-top-20" id="txtBEmailError"></p>
                </td>
            </tr>
            <tr>
                <td>Mobile</td>
                <td class="txtBMobileInput"><input class="form-control" type="text" id="txtBMobile" runat="server" style="width:180px" name="mobile" />
                    <p class="text-red font10  margin-top-20" id="txtBMobileError"></p>
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td><a id="request_photos"  class="btn btn-orange action-button">Request</a></div></td>                
            </tr>
        </table>     
    </div>
    <div id="photos_req_msg" class="hide">
	    <%--<h2 class="hd2">It's Done!</h2>--%>
	    <div id="photos_req_confirm" >
		    Your request to the seller submitted successfully. We will inform you as seller updates photos of this car.
	    </div>
    </div> 
     <div id="process_img1" class="process-inline" style="margin-top:-100px; margin-left:150px;display:none;"></div> 
</body>
</html>
<script type="text/javascript">
 
    $(document).ready(function () {
        $('#gb-window, #gb-head').css('border-bottom', 'none');
       

        var Name = $("#txtName").val();
        var Email = $("#txtEmail").val();
        var Mobile = $("#txtMobile").val();

        if ((Name != "") && (Email != "") && (Mobile != "")) {
            $("#txtBName").val(Name);
            $("#txtBEmail").val(Email);
            $("#txtBMobile").val(Mobile);
        }

        $("#request_photos").click(function () {
            buyersName = $("#txtBName").val();
            buyersEmail = $("#txtBEmail").val();
            buyersMobile = $("#txtBMobile").val();

            if (validateControls()) {
                $("#requester_form").addClass("opacity4");
                $("#process_img1").show();
                submitPhotoRequest();
            }
        });
    });
   
    // Added by Aditi Dhaybar on 11/10/2014 for Request Photos validation
    function validateControls() {
        var reEmail = /^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,6}$/;
        $("#txtBNameError").html("");
        $("#txtBEmailError").html("");
        $("#txtBMobileError").html("");
        $("#txtBEmail").removeClass("red-border");
        $("#txtBMobile").removeClass("red-border");
        $("#txtBName").removeClass("red-border");

        buyersName = buyersName.trim();
        if (buyersName == "") {
            ShakeFormView($(".txtBNameInput"));
            $("#txtBName").addClass("red-border");
            $("#txtBNameError").html("Please enter your name");
            return false;
        } else {
            $("#txtBName").removeClass("red-border");
        }
        var regex = new RegExp("^[a-zA-Z0-9 ]+$");
        if (!regex.test(buyersName)) {
            ShakeFormView($(".txtBNameInput"));
            $("#txtBName").addClass("red-border");
            $("#txtBNameError").html("Invalid Name");
            return false;
        }
        else {
            $("#txtBName").removeClass("red-border");
        }

        if (buyersEmail == "") {
            ShakeFormView($(".txtBEmailInput"));
                $("#txtBEmail").addClass("red-border");
                $("#txtBEmailError").html("Please enter your email address");
                return false;
        } else if (!reEmail.test(buyersEmail.toLowerCase())) {
            ShakeFormView($(".txtBEmailInput"));
            $("#txtBEmail").addClass("red-border");
            $("#txtBEmailError").html("Invalid email address");
            return false;
        } else {
            $("#txtBEmail").removeClass("red-border");
        }


        if (buyersMobile == "") {
            ShakeFormView($(".txtBMobileInput"));
            $("#txtBMobileError").html("Please enter your mobile number");
            $("#txtBMobile").addClass("red-border");
            return false;
        } else if (buyersMobile != "" && re.test(buyersMobile) == false) {
            ShakeFormView($(".txtBMobileInput"));
            $("#txtBMobile").addClass("red-border");
            $("#txtBMobileError").html("Invalid mobile number");
            return false;
        } else if (buyersMobile != "" && (!re.test(buyersMobile) || buyersMobile.length < 10 || buyersMobile.length > 10)) {
            $("#txtBMobile").addClass("red-border");
            $("#txtBMobileError").html("Your mobile number should be of 10 digits only");
            return false;
        } else {
            $("#txtBMobile").removeClass("red-border");
        }

        return true;
    }
</script>