<%@ Page trace="false" Inherits="Carwale.UI.Users.ChangePassword" AutoEventWireUp="false" Language="C#" %>
<!doctype html>
<html>
<head>

<%
	// Define all the necessary meta-tags info here.
	// To know what are the available parameters,
	// check page, headerCommon.aspx in common folder.
	
	PageId 			= 71;
	Title 			= "Change Password";
	Description 	= "";
	Keywords		= "";
	Revisit 		= "15";
	DocumentState 	= "Dynamic";
    AdId = "1337162297840";
    AdPath = "/7590/CarWale_MyCarWale/CarWale_MyCarWale_Misc/CarWale_MyCarWale_Misc_";
%>
 <!-- #include file="/includes/global/head-script.aspx" -->

<style type="text/css">
	.dashborder{border-bottom:dotted thin #DCDCDC;}
	.insetborder{border-right:inset thin;}
	.mandatory{color:#ff0000;}	
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
    <section>
        <div class="container margin-bottom20">
            <div class="grid-12">
	        <div id="content" style="width:100%;" class=" margin-bottom20">
		            
                 <div class="breadcrumb margin-bottom15">
                        <!-- breadcrumb code starts here -->
                        <ul class="special-skin-text">
                            <li><a href="/">Home</a></li>
                            <li><span class="fa fa-angle-right margin-right10"></span><a href="/mycarwale/default.aspx">My CarWale</a></li>
                            <li><span class="fa fa-angle-right margin-right10"></span><a href="/mycarwale/MyContactDetails.aspx">My Profile</a></li>
                            <li><span class="fa fa-angle-right margin-right10"></span>Change Password</li>
                        </ul>
                        <div class="clear"></div>
                    </div>
                 
		
			<div id="uiTabs">
				<ul class="ui-tabs-nav">
					<li><a href="/mycarwale/MyContactDetails.aspx"><span>My Profile</span></a></li>
					<li><a href="/mycarwale/EditCustomerDetails.aspx"><span>Update Contact Details</span></a></li>				
					<li><a href="/users/EditUserProfile.aspx"><span>Update Community Profile</span></a></li>				
					<li class="ui-tabs-selected"><a href="#tb3"><span>Change Password</span></a></li>								
				</ul>
				<!--Change Password Content-->
				<div  id="tb3" class="ui-tabs-panel" >	
					<span id="spnError" class="error" runat="server"></span>
					<table border="0" cellpadding="5" width="100%"  cellspacing="0">
						<tr>
							<td class="dashborder" colspan="2">
								<p>All fields are mandatory.</p><br />
							</td>
						</tr>
						<tr>
							<td align="right" style="background:#E7F8FF;width:200px;" class="insetborder"><strong>Change Password</strong></td>
							<td colspan="2" ></td>
						</tr>
						<tr>
							<td align="right" class="insetborder">Current Password :</td>
							<td>
                                <div class="classform-control-box inline-block" >
								    <asp:TextBox ID="txtCurPassword" TextMode="Password" runat="server"  CssClass="text form-control ui-autocomplete-input blur"></asp:TextBox>
                                </div>
							</td>
						</tr>
						<tr>
							<td align="right" class="insetborder">New Password :</td>
							<td>
                                <div class="classform-control-box inline-block" >
								    <asp:TextBox ID="txtNewPassword" TextMode="Password" runat="server"  CssClass="text form-control ui-autocomplete-input blur"></asp:TextBox>
                                </div>
							</td>
						</tr>
						<tr>
							<td align="right" class="insetborder" valign="baseline">Confirm New Password :</td>
							<td>
                                <div class="classform-control-box inline-block" >
								    <asp:TextBox ID="txtConfirmNewPassword" TextMode="Password" runat="server"  CssClass="text form-control ui-autocomplete-input blur"></asp:TextBox>
                                </div>
							</td>
						</tr>
						<tr>
							<td class="insetborder">&nbsp;</td>
							<td>				
								<asp:Button ID="butChange" Text="Change Password" CssClass="btn btn-orange btn-xs" runat="server"></asp:Button>
								&nbsp;&nbsp;
								<asp:Button ID="butCancel" CausesValidation="false" Text="Cancel" CssClass="btn btn-orange btn-xs" runat="server"></asp:Button>				
							</td>
						</tr>
					</table>
				</div>
			</div>	
	     </div>
        </div>  
      </div>
    
 </section>
    <div class="clear"></div>

     <!-- #include file="/includes/footer.aspx" -->
        <!-- all other js plugins -->
        <!-- #include file="/includes/global/footer-script.aspx" -->
    </form>
  </body>
    </html>			