<%@ Page Language="C#" Inherits="Carwale.UI.Users.EditUserHandle" AutoEventWireup="false" Trace="false" %>
<%@ Register TagPrefix="FTB" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>
<%@ Import NameSpace="Carwale.UI.Common" %>
<!doctype html>
<html>
<head>

<%
	// Define all the necessary meta-tags info here.
	// To know what are the available parameters,
	// check page, headerCommon.aspx in common folder.
	PageId 			= 71;
	Title 			= "Choose Community User Name";
	Description 	= "";
	Keywords		= "";
	Revisit 		= "15";
	DocumentState 	= "Dynamic";
    AdId            = "1337162297840";
    AdPath          = "/7590/CarWale_MyCarWale/CarWale_MyCarWale_Misc/CarWale_MyCarWale_Misc_";
%>
<!-- #include file="/includes/global/head-script.aspx" -->
<style type="text/css">
	.dashborder{border-bottom:dotted thin #DCDCDC;}
	.subTitle{color:#767676;}
	.insetborder{border-right:inset thin;}
	.imgBorder{border-right:1px dotted #666666;border-color:#C3C3C3;}
	.mandatory{color:#ff0000;}
	.errMes{font-weight:bold;color:red;}	
	.comMessage{font-weight:bold;color:green;}		
	.imDisplay{width:18px;height:15px;background-color:#FFFFFF;}
	.imNone{display:none;}
	.mainMsg { width:650px; margin:10px; padding:5px; border:1px solid #FF5E5E; background-color:#FFD2D2; color:#CE0000; font-weight:bold; font-size:15px; font-family:Arial, Helvetica, sans-serif}
    /* Css for content tabs */
.ui-tabs .ui-tabs-hide { display: none !important; }/* Caution! Ensure accessibility in print and other media types... */	
.ui-tabs-nav { list-style: none;  margin: 0; padding: 0 0 0 3px;}/* Skin */	
/* clearing without presentational markup, IE gets extra treatment */
.ui-tabs-nav:after {display: block; clear: both; content: " "; }
.ui-tabs-nav li {margin:0 0 0 .5em; float: left;  font-weight: bold; list-style:none;}
/* fixes dir=ltr problem and other quirks IE */
.ui-tabs-nav a, .ui-tabs-nav a span {float: left; padding: 0 7px 0 10px; background: url(https://img.carwale.com/cw-common/tab2.png) no-repeat;}
/* position: relative makes opacity fail for disabled tab in IE */
.ui-tabs-nav a { margin: 5px 0 0;  padding-left: 0; background-position: 100% 0; text-decoration: none; white-space: nowrap; /* @ IE 6 */outline: 0; /* @ Firefox, prevent dotted border after click */ }
.ui-tabs-nav a:link, .ui-tabs-nav a:visited {color: #FFF; font-weight:bold;} 
.ui-tabs-nav .ui-tabs-selected a { position: relative; top: 5px; z-index: 2; margin-top: 0; background-position: 100% -25px;}
.ui-tabs-nav a span { padding-top: 0px; padding-right: 0;height: 25px; background-position: 0 0; line-height:25px;} 
.ui-tabs-nav .ui-tabs-selected a span { padding-top: 1px; height: 25px; background-position: 0 -25px; line-height:25px; color:#000;}
.ui-tabs-nav .ui-tabs-selected a:link, .ui-tabs-nav .ui-tabs-selected a:visited,
.ui-tabs-nav .ui-tabs-disabled a:link, .ui-tabs-nav .ui-tabs-disabled a:visited { cursor: text;/* @ Opera, use pseudo classes otherwise it confuses cursor... */}
.ui-tabs-nav a:hover, .ui-tabs-nav a:focus, .ui-tabs-nav a:active,
.ui-tabs-nav .ui-tabs-unselect a:hover, .ui-tabs-nav .ui-tabs-unselect a:focus, .ui-tabs-nav .ui-tabs-unselect a:active { cursor: pointer;}/* @ Opera, we need to be explicit again here now... */
.ui-tabs-disabled {opacity: .4;filter: alpha(opacity=40);}
.ui-tabs-nav .ui-tabs-disabled a:link, .ui-tabs-nav .ui-tabs-disabled a:visited { color: #000;}
.ui-tabs-panel { border: 1px solid #6598CC; padding: 10px; background: #fff; min-height:50px; display:block; margin-top:-5px;} /* declare background color for container to avoid distorted fonts in IE while fading */  
.ui-tabs-nav {display: inline-block;}/* auto clear @ IE 6 & IE 7 Quirks Mode */
</style>
</head>
<body class="bg-white header-fixed-inner special-page special-skin-body no-bg-color">
    <form runat="server">
        <!-- #include file="/includes/header.aspx" -->
        <input type="hidden" id="hdnIsPageFromCache" runat="server" />
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
                            <li><span class="fa fa-angle-right margin-right10"><a href="/mycarwale/MyContactDetails.aspx"></span>My Profile</a></li>
                            <li><span class="fa fa-angle-right margin-right10"></span>Update Community User Name</li>
                        </ul>
                        <div class="clear"></div>
                    </div>
                    <h1 class="font30 text-black special-skin-text">Manage Car Photos</h1>
                    <div class="border-solid-bottom margin-top10 margin-bottom15"></div>
                </div>
                <div class="clear"></div>
                </div>
            <div class="container">    
                <div class="grid-12">
                   
                    <div class="left_container_top" style="width:100%;">
	                    <div id="content" style="width:100%;">
			                    <div id="uiTabs">
				                    <ul class="ui-tabs-nav">
					                    <li><a href="/mycarwale/MyContactDetails.aspx"><span>My Profile</span></a></li>
					                    <li><a href="/mycarwale/EditCustomerDetails.aspx"><span>Update Contact Details</span></a></li>				
					                    <li><a href="/users/EditUserProfile.aspx"><span>Update Community Profile</span></a></li>				
					                    <li><a href="/users/changePassword.aspx"><span>Change Password</span></a></li>								
					                    <li class="ui-tabs-selected"><a href="#tb2"><span>Update Community User Name</span></a></li>													
				                    </ul>
				                    <!--My Community Profile Content-->	
				                    <div  id="tb2" class="ui-tabs-panel" >	
				                    <div align="center"><div align="center" class="mainMsg">CarWale Community requires you to complete this one-time, one-step registration process. Participation in the community is possible only if you complete it. To know more <a href="https://www.carwale.com/forums/ViewThread-11459.html" target="_blank">read this</a>.</div></div>
					                    <table cellpadding="5" cellspacing="0" border="0" width="100%" align="center">
						                    <tr>
							                    <td colspan="3"  align="center" class="dashborder" height="5px">
								                    <span id="spnDescription" class="error"></span>
								                    <asp:Label ID="lblMessage" CssClass="error" runat="server"/>
							                    </td>
						                    </tr>
						                    <tr>
							                    <td align="right"  style="background:#E7F8FF;width:200px;" class="insetborder"><strong>Community User Name</strong></td>
							                    <td colspan="2">Once chosen, the user name cannot be changed, please be careful while choosing.
							                    <br />User name should be 3-20 characters in length. Use A-Z, 0-9 and dot (.) to form a name.
							                    <br />Example: raj.kumar, raghav, anirudh.007, roshan72 etc</td>
						                    </tr>
						                    <tr>	
							                    <td align="right" class="insetborder">&nbsp;</td>
							                    <td>
								                    <asp:TextBox ID="txtHandle" runat="server" Columns="20" MaxLength="20" Rows="10"  CssClass="text form-control" Width="300"></asp:TextBox>
								                    <img id="imDone" runat="server" class="imNone"/>
								                    &nbsp;<span id="spnHandle" runat="server"></span>
								                    <input type="hidden" id="hdnChk" runat="server" />
							                    </td>
						                    </tr>
						                    <tr>
							                    <td class="dashborder" style="border-right:inset thin;">&nbsp;</td>
							                    <td class="dashborder"><%=sysUserName%></td>
						                    </tr>						
						                    <tr>
							                    <td class="insetborder">&nbsp;</td>
							                    <td align="left" ><asp:Button ID="btnSave" CssClass="buttons btn btn-orange" runat="server" Text="Get me this user name!"></asp:Button></td>	
						                    </tr>
					                    </table>
				                    </div>
			                    </div>		
		
	                    </div>
                    </div>
                </div>
            </div>
                <div class="clear"></div>
        </section>
        <!-- #include file="/includes/footer.aspx" -->
        <!-- all other js plugins -->
        <!-- #include file="/includes/global/footer-script.aspx" -->
        <script language="javascript">
            if (document.getElementById("btnSave"))
                document.getElementById("btnSave").onclick = verifyForm;
            if (document.getElementById("txtHandle"))
                document.getElementById("txtHandle").onchange = AllowAlphaNumeric;

            function verifyForm(e) {
                var isError = false;

                if (AllowAlphaNumeric())
                    isError = true;

                if (isError == true)
                    return false;
            }

            function AllowAlphaNumeric() {
                var val = $("#txtHandle").val().toLowerCase();
                var err = false;
                var op = false;

                document.getElementById("spnHandle").innerHTML = "";
                document.getElementById("hdnChk").Value = "False";

                imgDone = document.getElementById("imDone");
                $("#imDone").removeClass("imNone").addClass("imDisplay").show();

                op = AjaxForum.GetHandleName(val).value;

                if (op != false) {
                    document.getElementById("spnHandle").innerHTML = "<span class='errMes'>Sorry! The user name is already used. Try another please! :-(</span>";
                    imgDone.src = "https://img.carwale.com/forum/crossicon.png";
                    err = true;
                }

                //To check whether it is null or not
                if (val == '' && val == null) {
                    document.getElementById("spnHandle").innerHTML = "<span class='errMes'>Enter user name</span>";
                    imgDone.src = "https://img.carwale.com/forum/crossicon.png";
                    err = true;
                }

                //To Check to range of value
                if (val.length < 3 || val.length > 20) {
                    document.getElementById("spnHandle").innerHTML = "<span class='errMes'>Length should be 3-20 characters.</span>";
                    imgDone.src = "https://img.carwale.com/forum/crossicon.png";
                    err = true;
                }

                //To test Alphanumeric Characters
                var chk = /^[a-z_0-9_.]*$/;

                if (chk.test(val) == false) {
                    document.getElementById("spnHandle").innerHTML = "<span class='errMes'>Choose combination of A-Z, 0-9 and dot (.) to form a name.</span>";
                    imgDone.src = "https://img.carwale.com/forum/crossicon.png";
                    err = true;
                }

                if (val.substr(0, 1) == '.') {
                    document.getElementById("spnHandle").innerHTML = "<span class='errMes'>First character can only be a number or alphabet!</span>";
                    imgDone.src = "https://img.carwale.com/forum/crossicon.png";
                    err = true;
                }

                if (err == true) {
                    document.getElementById("hdnChk").Value = "True";
                    return true;
                } else {
                    document.getElementById("spnHandle").innerHTML = "<span class='comMessage'>Congratulations! User name is available! :-)</span>";
                    imgDone.src = "https://img.carwale.com/forum/arrowicon.png";
                }
            }
</script>
        </form>
     </body>
    </html>
		
