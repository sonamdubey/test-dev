<%@ Page trace="false" Inherits="Carwale.UI.Users.Login" AutoEventWireUp="false" Language="C#" %>
<%@ Register TagPrefix="Carwale" TagName="Login" src="/Controls/LoginControl.ascx" %>

<!doctype html>
<html>
<head>
<%
	// Define all the necessary meta-tags info here.
	// To know what are the available parameters,
	// check page, headerCommon.aspx in common folder.
	
	PageId 			= 2;
	Title 			= "Member Login";
	Description 	= "CarWale.com Member Login";
	Keywords		= "";
	Revisit 		= "15";
	DocumentState 	= "Static";
    AdId            = "1396439626285";
    AdPath          = "/1017752/Homepage_";
%>
<!-- #include file="/includes/global/head-script.aspx" -->

<style type="text/css">
    .newblue-block{background-color:#f4f3f3; padding:0 10px 10px; border:1px solid #e5e5e5; }

   .cw-sprite { background:url(https://imgd.aeplcdn.com/0x0/cw/static/sprites/cw-sprite_v1.3.png?28052015) no-repeat; display:inline-block; }
    .login-stage, .sign-up-stage, .logged-stage-stage { width:550px; transition: width 1.5s; -moz-transition: width 1.5s; -webkit-transition: width 1.5s; -o-transition: width 1.5s; -ms-transition: width 1.5s; }
    .login-with-fb-gplus { float:left; display:inline-block; width:220px; }
     a.fb-login-btn, a.gplus-login-btn { margin-bottom:20px; height:30px; width:180px; display:inline-block; color:#fff; font-weight:bold; text-decoration:none; border-radius:4px; -moz-border-radius:4px; -webkit-border-radius:4px; -o-border-radius:4px; -ms-border-radius:4px; }
    a.fb-login-btn { background:#4b72cc; }
    a.fb-login-btn:hover { background:#3b589e; }
    a.gplus-login-btn { background:#e63939; }
    a.gplus-login-btn:hover { background:#cc3333; }
    .fb-icon-bg, .gplus-icon-bg { display:inline-block; height:30px; width:30px; margin-right:5px; border-radius:3px 0 0 3px; }
    .fb-icon-bg { background:#3b589e; }
    .gplus-icon-bg { background:#cc3333; }
    .fb-login-icon, .gplus-login-icon { position:relative; }
    .fb-login-icon { background-position:-190px -730px; width:11px; height:19px; top:5px; left:9px; }
    .gplus-login-icon { background-position:-189px -758px; width:19px; height:19px; top:7px; left:6px; }
    .txt-with-fb-gplus { display:inline-block; position:relative; *top:-7px; }
    .lgn-or-box { background:#fff; border-radius:50%; display:inline-block; color:#999999; font-size:12px; font-weight:bold; position:absolute; left:-12px; top:30%; padding:3px 5px; border:1px solid #e2e2e2; }
    .logged-stage-with-cw ul { display:block; height:auto; }
    .logged-stage-with-cw li { display:block; border-right:0; border-bottom:1px solid #e6e6e6; padding:8px 10px 3px 10px; float:none; box-shadow:none; }
    .logged-stage-with-cw li:hover { background:#f2f2f2; color:#333; }
    .logged-stage-with-cw li a { color:#666666; padding:0; display:block; text-decoration:none; }
    .logged-stage-with-cw li a:hover { background:none; }
    .logged-stage-with-cw li a span { display:block; float:left; }
    .logged-stage-with-cw li .prs-logged-pic img { width:25px; height:25px; position:relative; top:-2px; left:-2px; margin-right:9px; }
    #showicon img { width:25px; height:25px; position:relative; top:-5px; left:-3px; margin-right:9px; }
    .prs-logged-text { position:relative; top:2px; font-weight:bold; white-space:nowrap; text-overflow:ellipsis; overflow:hidden; width:135px; }
    .red-border, input[type="text"].red-border, input[type="password"].red-border, input[type="button"].red-border, input[type="tel"].red-border, textarea.red-border, button.red-border { border:1px solid #F00; }
    .login-with-cw { float:left; position:relative; border-left:1px solid #e2e2e2; padding-left:40px; width:285px; padding-bottom:15px; }


</style>
<script type="text/javascript">
    <%--var CurrentId = "<%=CurrentUserId%>";--%>
</script>
</head>
    <body class="bg-white header-fixed-inner special-page special-skin-body no-bg-color">
        <form runat="server">
            <!-- #include file="/includes/header.aspx" -->
            <section class="container">
            <div class="grid-12">
                <div class="padding-bottom15 text-center">
                    <%= Carwale.UI.HtmlHelpers.Helpers.GetAdBarForAspx(AdId, 0, 90, 0, 0, true, 2) %>
                </div>
            </div>
        </section>
        <div class="clear"></div>
        <section class="bg-light-grey padding-top10 padding-bottom20 no-bg-color">
            <div class="container">
                <div class="grid-12">
                    <div class="breadcrumb margin-bottom15">
                        <!-- breadcrumb code starts here -->
                        <ul class="special-skin-text">
                            <li><a href="/">Home</a></li>
                            <li><span class="fa fa-angle-right margin-right10"></span>Login</li>
                        </ul>
                        <div class="clear"></div>
                    </div>
                    <h1 class="font30 text-black special-skin-text">Login</h1>
                    <div class="border-solid-bottom margin-top10 margin-bottom15"></div>
                </div>
                <div class="clear"></div>
                <div class="grid-8 margin-top10">
		            <div class="content-box-shadow content-inner-block-10" id="tb1">
                        <div class="">
                            <!-- login-stage code starts here -->
                            <div class="login-stage">
                                <div class="grid-6">
                        	        <div class="login-with-fb-gplus margin-top10">
                                        <a href="#"  class="fb-login-btn" onclick="return fb_login_main();">
                                	        <span class="fb-icon-bg">
                                    	        <span class="cw-sprite fb-login-icon"></span>
                                            </span>
                                            <span class="txt-with-fb-gplus  pos-top5">Login with Facebook</span>
                                        </a>
                                        <a class="gplus-login-btn" onClick='login_main();' >
                                	        <span class="gplus-icon-bg">
                                    	        <span class="cw-sprite gplus-login-icon"></span>
                                            </span>
                                            <span class="txt-with-fb-gplus  pos-top5">Login with Google</span>
                                        </a>
                                    </div>
                                </div>
                                <div class="grid-6">
                                    <div class="login-with-cw">
                            	        <span class="lgn-or-box">or</span>
                            	        <p class="font12 margin-top5 margin-bottom15">Login using Carwale ID</p>
                                        <div id="invalidlogin_main" class="alert margin-bottom15 hide">
                                            Invalid Credentials
                                        </div>
                                        <div class="clear"></div>
                                    <!----html code for login in carwale using ajax-->
                                        <div class="margin-top5 margin-bottom10">
                                        <input type="text" name="email id" id="txtloginemail_main" class="form-control input-xs" placeholder="Email Id" /> 
                                        </div>
                                        <div class="margin-top5 margin-bottom10">
                                        <input type="password" id="txtpasswordlogin_main" class="form-control input-xs"  placeholder="Password" onkeydown="javascript:if(event.keyCode==13) doLoginCustomer_main();"/>
                                        </div>
                                        <div class="margin-top10 margin-bottom10">
                                        <input type="button" id="btnLogin_main" class="btn btn-orange text-uppercase" value="Login" onclick="doLoginCustomer_main()"/>
                                        </div>
                                        <div class="margin-top5 margin-bottom15">
                                        <input type="checkbox" id="chkRemMe_main" class="margin-left5" name="chkRemMe_main" />
                                        <label for="chkRemMe_main">Remember Me</label>
                                        </div>                                     
                                    </div>
                                </div>
                                <div class="clear"></div>
                            </div> <!-- login-stage code ends here --> 
                        </div>
		            </div>									
	            </div>
                <div class="grid-4 margin-top10">
                    <div class="content-box-shadow content-inner-block-10">
                        <div>
		                    <h2 class="hd2">Dealers Affiliation Program</h2>
                            <div class="clear"></div>
                            <div class="margin-top5 grey-text">Want to join the growing CarWale dealership network.</div>
                            <div class="margin-top5 grey-text">It is easy and fast!</div>
                            <br />
                            <a href="/dealer/dealerregister.aspx" class="btn btn-orange text-uppercase" title="Register Now">Register Now</a>
                        </div>
                    </div>
                </div>
                <div class="clear"></div>
        </section>
        <div class="clear"></div>
        <!-- #include file="/includes/footer.aspx" -->
        <!-- all other js plugins -->
        <!-- #include file="/includes/global/footer-script.aspx" -->
        </form>
    </body>
</html>

 
	
    

<script type="text/javascript">
    function fb_login_main() {
        window.fbAsyncInit = function () {
            FB.init({
                appId: FACEBOOKAPPID,
                cookie: true,  // enable cookies to allow the server to access 
                // the session
                xfbml: true,  // parse social plugins on this page
                version: 'v2.2' // use version 2.2
            });
        }
        window.fbAsyncInit();
        FB.login(function (response) {
            var loginresponse = response;
            if (response.authResponse) {
                FB.api('/me', function (response) {
                    if (response.id > 0) {
                        $.ajax({
                            type: "POST",
                            url: "/ajaxpro/CarwaleAjax.AjaxCommonPro,Carwale.ashx",
                            data: '{"fbId":"' + response.id + '","accessToken":"' + loginresponse.authResponse.accessToken + '"}',
                            beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "FbRegistration"); },
                            success: function (verificationresp) {
                                var x = eval('(' + verificationresp + ')');
                                if (x.value == null || x.value == "") alert("Failed to login");
                                else if ($.isNumeric(x.value.CustomerId) && x.value.CustomerId != "0") {
                                    var now = new Date();
                                    var Time = now.getTime();
                                    Time += 1000 * 60 * 60 * 5040;
                                    now.setTime(Time);
                                    document.cookie = '_Fblogin=' + x.value.Id + '; expires = ' + now.toGMTString() + ';path =/';
                                    window.location.href = '<%=_returnUrl%>';
                                }
                                else alert("Sorry, we don't have permission to login.");
                            }
                        });
                    }
                });
            }
        }, { scope: 'public_profile,email' });
    }
    function login_main() {

        var win = window.open(_url, "windowname1", 'width=800, height=600');

        var pollTimer = window.setInterval(function () {
            try {
                if (win.document.URL.indexOf(REDIRECT) != -1) {
                    window.clearInterval(pollTimer);
                    var url = win.document.URL;
                    acToken = gup(url, 'access_token');
                    tokenType = gup(url, 'token_type');
                    expiresIn = gup(url, 'expires_in');
                    win.close();

                    validateToken_main(acToken);
                }
            } catch (e) { }
        }, 500);
    }
    function validateToken_main(token) {
        $.ajax({
            type: "POST",
            url: "/ajaxpro/CarwaleAjax.AjaxCommonPro,Carwale.ashx",
            data: '{"accessToken":"' + token + '"}',
            beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GplusRegistration"); },
            success: function (verificationresp) {
                var x = eval('(' + verificationresp + ')');
                if (x.value.CustomerId == null || x.value.CustomerId == "-1") alert("Failed to login");
                else if ($.isNumeric(x.value.CustomerId) && x.value.CustomerId != "0") {
                    var now = new Date();
                    var Time = now.getTime();
                    Time += 1000 * 60 * 60 * 5040;
                    now.setTime(Time);
                    document.cookie = '_GoogleCookie=' + x.value.Id + '|' + x.value.Picture + '; expires = ' + now.toGMTString() + ';path =/';
                    window.location.href = '<%=_returnUrl%>';
                }
                else alert("Sorry, we don't have permission to login.");
            }
        });
    }
    function doLoginCustomer_main() {
        var loginId = $("#txtloginemail_main").val();
        var passwd = $("#txtpasswordlogin_main").val();
        var rememberMe = document.getElementById("chkRemMe_main").checked == true ? true : false;
        if (loginId != "" && passwd != "") {
            $('#txtloginemail_main, #txtpasswordlogin_main').removeClass('red-border');
            requestLogin_main(loginId, passwd, rememberMe);
        } else {
            if (loginId == "") $('#txtloginemail_main').addClass('red-border'); else $('#txtloginemail_main').removeClass('red-border');
            if (passwd == "") $('#txtpasswordlogin_main').addClass('red-border'); else $('#txtpasswordlogin_main').removeClass('red-border');
        }
    }

    function requestLogin_main(loginId, passwd, rememberMe) {
        $.ajax({
            type: "POST",
            url: "/ajaxpro/CarwaleAjax.AjaxCommonPro,Carwale.ashx",
            data: '{"loginId":"' + loginId + '","pwd":"' + passwd + '","rememberMe":"' + rememberMe + '","isDealer":' + false + '}',
            beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "UserLogin"); },
            success: function (response) {
                var loginStatus = eval('(' + response + ')');
                if (loginStatus.value == true) {
                    window.location.href = '<%=_returnUrl%>';
                } else {
                    $("#invalidlogin_main").removeClass('hide');
                }
            }
        });
    }
    function uiAfterLogin() {
        window.location.href = '<%=_returnUrl%>';
    }
</script>   



