<%@ Import Namespace="Carwale.UI.Common" %>
<%@ Import Namespace="Carwale.UI.Users" %>

<!-------------------------------------------------------------------------------------------------------------------------->
<!-- Top Navbar code start here -->
<div class="top-navbar">
<script language="c#" runat="server">
    public string CurrentUserId = CurrentUser.Id;
    //public CustomerDetails cd2 = new CustomerDetails(CurrentUser.Id);
</script>
    <script>
        var CHECKCHECK = "<%=CurrentUser.Id%>";
    </script>
    <div class="row">
        <div class="column grid_5">
            <div id="top-navbar" class="top-navbar-list">
                <ul>
                    <li class="first"><a href="https://www.bikewale.com/?utm_source=CW&utm_medium=HeaderLink&utm_campaign=Self&utm_term=nonpaid" target="_blank" class="first">BikeWale</a></li>
                    <%--Below Lang changer code for next release--%>
                	<li class="lang-changer-li">
                    	<ul class="lang-changer">
                        	<li><a class="" id="English"><span class="cw-sprite english-text"></span></a></li>
                            <li><a class="" id="Hindi"><span class="cw-sprite hindi-text"></span></a></li>
                        </ul>
                    </li>
               </ul>
            </div>
        </div>
        <div class="column grid_7">
            <div id="right-login" class="top-navbar-list-rgt">
            	<ul>
					<%--<li class="red-btn margin-right10"><a href="/playeroftheday/day/30/?utm_source=carwalehp&utm_medium=banner&utm_content=winprizeslink&utm_campaign=playeroftheday" title="Car Offers"  class="white-text">Win Exciting Prizes</a></li>--%>
                    <li class="cwCityBox" onclick="popUphtml()">
                        <span id="cwTopCityBox">Select City</span>
                        <span class="cw-sprite cityEditIcon"></span>
                    </li>
                    <li class="login-bx " id="firstLogin">
                    	<span class="login-txt<%=CurrentUserId=="-1"?"":" hide" %>">Login</span>
                        <span class="cw-sprite logged-prs-icon<%=CurrentUserId!="-1"?"":" hide" %>" id="showicon"></span>
                    </li>
                    <li><a href="http://www.facebook.com/CarWale"><span class="cw-sprite tnb-fb-sm-icon"></span></a></li>
                    <li><a href="https://twitter.com/carwale"><span class="cw-sprite tnb-tw-sm-icon"></span></a></li>
                    <li><a href="https://plus.google.com/+CarWale"><span class="cw-sprite tnb-gPlus-sm-icon"></span></a></li>
                </ul>
                <script>
                    if($.cookie("_CustCityMaster")!=null && $.trim($.cookie("_CustCityMaster"))!="") 
                        $("#cwTopCityBox").text($.cookie("_CustCityMaster"));
                </script>
                <div class="login-pop hide">
                    
                	<!-- login-stage and sign-up-stage code starts here -->
                	<div class="login-box<%=CurrentUserId=="-1"?"":" hide" %>" id="Testlogin-box">
                    	<!-- login-stage code starts here -->
                        <div class="login-stage <%=CurrentUserId=="-1"?"":"hide" %>">
                        	<div class="login-with-fb-gplus">
                            	<p class="font16" style="margin-bottom: 15px;"><strong>Login</strong></p>
                                <a   class="fb-login-btn" onclick="return fb_login();">
                                	<span class="fb-icon-bg">
                                    	<span class="cw-sprite fb-login-icon"></span>
                                    </span>
                                    <span class="txt-with-fb-gplus">Login with Facebook</span>
                                </a>
                                <a class="gplus-login-btn" onClick='login();' >
                                	<span class="gplus-icon-bg">
                                    	<span class="cw-sprite gplus-login-icon"></span>
                                    </span>
                                    <span class="txt-with-fb-gplus">Login with Google</span>
                                </a>
                            </div>
                            <div class="login-with-cw">
                            	<span class="lgn-or-box">or</span>
                            	<p class="font12 margin-top5 margin-bottom15">Login using Carwale ID</p>
                                <div id="invalidlogin" class="alert margin-bottom15 hide">
                                    Invalid Credentials
                                </div>
                                <div class="clear"></div>
                            <!----html code for login in carwale using ajax-->
                               <input type="text" name="email id" id="txtloginemail" placeholder="Email Id" /> 
                               <input type="password" id="txtpasswordlogin" class="text"  placeholder="Password" onkeydown="javascript:if(event.keyCode==13) doLoginCustomer();"/>
                               <input type="button" id="btnLogin" class="buttons-gray" value="Login" onclick="doLoginCustomer()"/>
                               <div class="margin-top5 margin-bottom15">
                                <input type="checkbox" id="chkRemMe" class="margin-left5" name="rem" />
                               <label for="rem">Remember Me</label>
                                   </div>
                              <div id="divForgot" class="moz-round"></div>
                                <div class="margin-top5 margin-bottom15">
                                    <a  class="margin-left5 font12" id="forgotpass">Forgot Password?</a>
                                </div>
                                 
                                <!-- Forgot password code starts here -->
                                <div id="forgotpassdiv">
                                <div class="sign-up-stage hide " id="forgotpassbox">
                                <div class="sign-up-with-cw">
                            	<p class="margin-bottom10" >Please enter your Email Id</p>
                                <p><input type="text" name="emailId"id="txtForgotPass" placeholder="Email Id" /></p>
                                <input type="submit" id="frgPwd"  onclick="sendPwd()" value="Send" class="grey-btn" />
                                </div>                                    
                                <div class="clear"></div>
                                </div>
                                 </div>
                                 <!-- Forgot password code ends here -->  

                                <p class="font12">Don't have an account? <a  id="registerHere">Register Here</a></p>
                            </div>
                            <div class="clear"></div>
                        </div>
                        <!-- login-stage code ends here -->
                        <!-- sign-up-stage code starts here -->
                        <div class="sign-up-stage hide">
                            <div class="sign-up-with-cw">
                            	<p class="font16" style="margin-bottom: 15px;"><strong>Sign Up</strong></p>
                                <input type="text" name="name" placeholder="Enter Name"  id="txtnamelogin"/> <span id="spnName" class="error"></span>
                                <input type="text" name="emailId" id="txtemailsignup" placeholder="Email Id" /><span id="spnEmail" class="error"></span>
                                <input type="text" name="mobile" placeholder="Mobile" id="txtmobilelogin" onkeydown="javascript:if(event.keyCode==13) registerCust();"/><span id="spnMobile" class="error"></span>
                                <input type="password" name="password" placeholder="Password" id="txtRegPasswd"/><span id="spnRegPassword" class="error"></span>
                                <input type="password" name="confirmPassword" placeholder="Confirm Password" id="txtConfPasswdlogin" /> <span id="spnConfPasswd" class="error"></span>
                                <div class="margin-bottom15 font12 min-line-height">
                                	<input id="agreecheck" type="checkbox" checked="checked" /> I have read and agree with the <a href="/privacypolicy.aspx" class="margin-left5 font12">User Agreement and Privacy Policy</a>
                                </div>
                                <p class="font12">
                                    Already have an <a  class="margin-right20 font12" id="alreadyAccount">account?</a>
                                    <a id="regpopupbtn" class="buttons-gray" onclick="initRegisterCust()">Sign Up</a>
                                </p>
                            </div>
                            <div class="clear"></div>
                        </div>

                        <!-- sign-up-stage code ends here -->                   
                    </div>
                    <!-- login-stage and sign-up-stage code ends here -->
                    
                    <!-- logged-stage code starts here -->
                    <div class="logged-stage-stage <%=CurrentUserId=="-1"?"hide":"" %>">
                        <div class="logged-stage-with-cw">
                            <ul>
                            	<li>
                                   <a href="/mycarwale/" rel="nofollow" id="carwale" class="hide">                                         
                                        <span id="profilepic" class="prs-logged-pic"></span>   
                                        <span id="status" class="prs-logged-text" title="<%=CurrentUser.Name%>"  ><%=CurrentUser.Name%></span>                                                           
                                        <div class="clear"></div>                                                                                
                                    </a>
                                    
                               	</li>
                                <li>
                                	<a href="/mycarwale/myinquiries/mysellinquiry.aspx" rel="nofollow">
                                    	<span class="cw-sprite manage-listing-icon"></span>
                                    	<span>Manage my car listing</span>
                                        <div class="clear"></div>
                                    </a>
                               	</li>
                                <%--<li>
                                	<a href="/mycarwale/myinquiries/myusedpurchaseinquiry.aspx">
                                    	<span class="cw-sprite short-listing-icon"></span>
                                    	<span>Shortlisted used cars</span>
                                        <div class="clear"></div>
                                    </a>
                               	</li>--%>
                                <li>
                                	<a href="/mygarage/" >
                                    	<span class="cw-sprite my-garage-icon"></span>
                                    	<span>My Garage</span>
                                        <div class="clear"></div>
                                    </a>
                               	</li>
                                <div class="changepass">
                                 <li>
                                	<a href="/users/changePassword.aspx" rel="nofollow">
                                    	<span class="cw-sprite setting-icon "></span>
                                    	<span>Change Password</span>
                                        <div class="clear"></div>
                                    </a>
                               	</li>
                                </div>
                                <li>
                                	<a onclick="clicklogout()">
                                        <span class="cw-sprite logout-icon" ></span>
                                        <span > Logout</span>
                                        <div class="clear"></div>
                                    </a>
                               	</li>
                            </ul>
                        	<div class="clear"></div>
                        </div>                        
                    </div>
                    <!-- logged-stage code ends here -->
                </div>
            </div>          
        </div>
    </div>
<script type="text/javascript">  
    if ((!(($.cookie("_Fblogin") == "" || $.cookie("_Fblogin") == null)))) {
        document.getElementById('profilepic').innerHTML = '<img src="http://graph.facebook.com/' + $.cookie('_Fblogin') + '/picture" />';
        document.getElementById('showicon').innerHTML = '<img src="http://graph.facebook.com/' + $.cookie('_Fblogin') + '/picture" />';
    }
    else if ((!(($.cookie("_GoogleCookie") == "" || $.cookie("_GoogleCookie") == null)))) {
        document.getElementById('profilepic').innerHTML = '<img src="' + $.cookie('_GoogleCookie').split('|')[1] + '" />';
        document.getElementById('showicon').innerHTML = '<img src="' + $.cookie('_GoogleCookie').split('|')[1] + '" />';
    }
    else document.getElementById('profilepic').innerHTML = '<span class="cw-sprite logged-prs-icon"></span>';

    setLangChangerHref(pagePath());

    if (window.location.hostname == "www"+"."+"carwale"+"."+"com")
        $("#English").addClass('active-lang').children("span").addClass('active');
    else
        $("#Hindi").addClass('active-lang').children("span").addClass('active');
</script>
<div id="fb-root"></div>
</div>
<!-- Top Navbar code end here -->


