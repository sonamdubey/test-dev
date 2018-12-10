<%@ Page Language="C#" Inherits="Carwale.UI.Users.EditUserProfile" AutoEventWireup="false" Trace="false" %>
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
	Title 			= "Edit Community Profile Details";
	Description 	= "";
	Keywords		= "";
	Revisit 		= "15";
	DocumentState 	= "Dynamic";
    AdId = "1337162297840";
    AdPath = "/7590/CarWale_MyCarWale/CarWale_MyCarWale_Misc/CarWale_MyCarWale_Misc_";
%>
<!-- #include file="/includes/global/head-script.aspx" -->
<script  type="text/javascript"  src="/static/src/ajaxfunctionsrqforums.js" ></script>
<style type="text/css">
	.dashborder{border-bottom:dotted thin #DCDCDC;}
	.subTitle{color:#767676;}
	.insetborder{border-right:inset thin;}
	.imgBorder{border-right:1px dotted #666666;border-color:#C3C3C3;}
	.mandatory{color:#ff0000;}
	.errMes{font-weight:bold;color:red;}	
	.comMessage{font:bold;color:green;}
    .hide{display:none;}
	.show{display:block;}
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
.ui-tabs-panel { border: 1px solid #6598CC; padding: 10px; background: #fff; min-height:50px; display:block; margin-top:-5px; } /* declare background color for container to avoid distorted fonts in IE while fading */  
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
     <div class="container">
           <div class="grid-12">
	       <div id="content" style="width:100%;" class=" margin-bottom20">
             <div class="breadcrumb margin-bottom15">
                            <!-- breadcrumb code starts here -->
                            <ul class="special-skin-text">
                                <li><a href="/">Home</a></li>
                                <li><span class="fa fa-angle-right margin-right10"></span><a href="/mycarwale/default.aspx">My CarWale</a></li>
                                <li><span class="fa fa-angle-right margin-right10"></span><a href="/mycarwale/MyContactDetails.aspx">My Profile</a></li>
                                <li><span class="fa fa-angle-right margin-right10"></span>Update Community Profile Details </li>
                            </ul>
                            <div class="clear"></div>
                        </div>
			    <div id="uiTabs">
				    <ul class="ui-tabs-nav">
					    <li><a href="/mycarwale/MyContactDetails.aspx"><span>My Profile</span></a></li>
					    <li><a href="/mycarwale/EditCustomerDetails.aspx"><span>Update Contact Details</span></a></li>				
					    <li class="ui-tabs-selected"><a href="#tb2"><span>Update Community Profile</span></a></li>				
					    <li><a href="/users/changePassword.aspx"><span>Change Password</span></a></li>								
				    </ul>
				    <!--My Community Profile Content-->	
				    <div  id="tb2" class="ui-tabs-panel" >	
					    <table cellpadding="5" cellspacing="0" border="0" width="100%" align="center">
						    <tr>
							    <td colspan="3" align="left">
								    <!--<span>CarWale Community requires you to complete this one-time registration process. Not all fields are mandatory.<br /></span>-->
							    </td>
						    </tr>
						    <tr>
							    <td colspan="3"  align="center" class="dashborder" height="5px">
								    <span id="spnDescription" class="error"></span>
								    <asp:Label ID="lblMessage" CssClass="error" runat="server"/>
							    </td>
						    </tr>
						    <tr>
							    <td align="right" style="background:#E7F8FF;" class="insetborder"><strong>About You</strong></td>
							    <td colspan="2" class="subTitle">Help community members know you better. A brief about you.</td>
						    </tr>
						    <tr>
							    <td class="insetborder">&nbsp;</td>
							    <td>
								    <asp:TextBox ID="txtAboutMe" runat="server" Columns="45" Rows="10"  CssClass="text" TextMode="MultiLine"></asp:TextBox>
								    <span style="vertical-align:top;" class="subTitle">Max 1000 characters</span>
								    <br>
								    <span id="spnDesc"></span>
							    </td>
						    </tr>
						    <tr>
							    <td class="dashborder" style="border-right:inset thin;">&nbsp;</td>
							    <td class="dashborder">&nbsp;</td>
						    </tr>
						    <tr>
							    <td align="right" style="background:#E7F8FF;" class="insetborder"><strong>Signature</strong></td>
							    <td colspan="2" class="subTitle">Will be posted along with every forum post you make.</td>
						    </tr>
						    <tr>
							    <td class="insetborder">&nbsp;</td>
							    <td>
								    <asp:TextBox ID="txtSignature" runat="server" Columns="45" Rows="5" MaxLength="500"  CssClass="text" TextMode="MultiLine"></asp:TextBox>
								    <span style="vertical-align:top;" class="subTitle">Max 500 characters</span>
								    <br>
								    <span id="spnSignature"></span>
							    </td>
						    </tr>
						    <tr>
							    <td class="dashborder" style="border-right:inset thin;">&nbsp;</td>
							    <td class="dashborder">&nbsp;</td>
						    </tr>
						    <tr>
							    <td align="right" style="background:#E7F8FF;" class="insetborder"><strong>Your Car(s)</strong></td>
							    <td colspan="2" class="subTitle">Show off your car(s) in the community!</td>
						    </tr>
							    <td class="dashborder" style="border-right:inset thin;">&nbsp;</td>
							    <td class="dashborder">&nbsp;</td>
						    </tr>
						    <tr>
							    <td align="right" style="background:#E7F8FF;" class="insetborder"><strong>Change Picture</strong></td>
							    <td colspan="2" class="subTitle">A picture is worth a thousand words!</td>
						    </tr>
						    <tr>
							    <td align="right" valign="top" class="insetborder">Avtar:</td>
							    <td>
								    <table style="border-collapse:collapse;" >
									    <tr>
										    <td valign="top">
                                                <div id='dtlstPhotosA_<%= userId%>' class='<%= statusId =="1" ? "hide" : "show" %>'>
											        <img id="imAv" runat="server" title="Avtar Preview" style="height:100px;width:100px;"/>
                                                </div>
                                                <div id='dtlstPhotosPending_<%= userId%>'  
                                                    class='pending <%= statusId =="1" ? "show" : "hide" %>' 
                                                    pending="<%= statusId =="1" ? "true" : "false" %>">
                                                   <p style="color:#555555;font-weight:bold;">
                                                    Processing...
                                                    <img  align="center" src='https://imgd.aeplcdn.com/0x0/statics/loader.gif'/>
                                                   </p>
                                                </div>
										    </td>
										    <td valign="top" style="padding-left:10px;">	
											    <input class="imgBorder" type="file" id="flAvtar" runat="server" />
											    <span class="subTitle">
											    <br>Maximum file-size: 20kb. 
											    <br>Avtar would be automatically reduced to 100x100 pixels.<br/><br />
											    </span>
										    </td>
									    </tr>
								    </table>
							    </td>	
						    </tr>
						
						    <tr>
							    <td class="insetborder">&nbsp;</td>
							    <td>&nbsp;</td>	
						    </tr>
						
						    <tr>
							    <td align="right" valign="top" class="insetborder">Your Picture:</td>
							    <td>
								    <table style="border-collapse:collapse;">
									    <tr>
										    <td valign="top">
                                                <div id='dtlstPhotosR_<%= userIdReal%>' class='<%= statusId=="1" ? "hide" : "show" %>'>
											        <img id="imRp" runat="server" title="Your Picture" style="height:100px;width:100px;"/>
                                                </div>
                                                <div id='dtlstPhotosPending_<%= userIdReal%>' class='pending <%= statusId=="1" ? "show" : "hide" %>' pending="<%= statusId =="1" ? "true" : "false" %>">
                                               
                                                </div>
										    </td>
										    <td  valign="top" style="padding-left:10px;">	
											    <input class="imgBorder" type="file" id="flReal" runat="server" />
											    <span class="subTitle">
											    <br>Maximum file-size: 100kb. 
											    <br>Photo would be automatically reduced to 520x390 pixels.
											    </span>
										    </td>
									    </tr>
								    </table>
							    </td>	
						    </tr>
						    <tr>
							    <td class="dashborder" style="border-right:inset thin;">&nbsp;</td>
							    <td class="dashborder">&nbsp;</td>
						    </tr>
						    <tr>
							    <td class="insetborder">&nbsp;</td>
                                <asp:HiddenField id="setToken" runat="server"/>
							    <td align="left" ><asp:Button ID="btnSave" CssClass="btn btn-orange btn-xs" runat="server" Text="Update Profile"></asp:Button></td>	
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
<script type="text/javascript">
   
    var imgCategory = '<%= imgCategory%>';
	document.getElementById("btnSave").onclick = verifyForm;
	document.getElementById("txtAboutMe").onkeydown = ShowCharactersLeft;
	document.getElementById("txtSignature").onkeydown = ShowCharactersLeftForSignature;

	function verifyForm(e){
		var isError = false;
		
		var aboutMe = document.getElementById("txtAboutMe").value;
		var avtar = document.getElementById("flAvtar").value;
		var real = document.getElementById("flReal").value;
		var sign = document.getElementById("txtSignature").value;
			
		if( aboutMe == "" && avtar == "" && real == "" && sign == ""){
			isError = true;
			document.getElementById("spnDescription").innerHTML = "&nbsp; Sorry! There is nothing to save.";
		}
		else
			document.getElementById("spnDescription").innerHTML = "";

		
		if(isError == true)
			return false;
	}	

	
	function ShowCharactersLeft(){
		var maxSize = 1000;
		var size = document.getElementById("txtAboutMe").value.length;
		
		if(size >= maxSize)	{
			ftb.SetHtml( ftb.GetHtml().substring(0, maxSize -1) );
			size = maxSize;
		}
		
		document.getElementById("spnDesc").innerHTML = "Characters Left : " + (maxSize - size);
	}
	
	function ShowCharactersLeftForSignature(){
		var maxSize = 500;
		var size = document.getElementById("txtSignature").value.length;
		
		if(size >= maxSize){
			ftb.SetHtml( ftb.GetHtml().substring(0, maxSize -1) );
			size = maxSize;
		}
		
		document.getElementById("spnSignature").innerHTML = "Characters Left : " + (maxSize - size);
	}
	
	function AllowAlphaNumeric(){
		var val =  $("#txtHandle").val().toLowerCase();
		var err = false;
		var op  = false;
				
		document.getElementById("spnHandle").innerHTML = "";
		document.getElementById("hdnChk").Value = "False";

		imgDone  = document.getElementById("imDone");
		imgDone.visible = false;
		
		op = AjaxForum.GetHandleName(val).value;
		
		if(op != false){
			document.getElementById("spnHandle").innerHTML = "<span class='errMes'>Oops! The user name belongs to someone else. Try another please!</span>";
			imgDone.src = "<%=ImagingFunctions.GetRootImagePath()%>/images/crossicon.png";
			err = true;
		}
		
		//To check whether it is null or not
		if(val == '' && val == null){
			document.getElementById("spnHandle").innerHTML = "<span class='errMes'>Enter handle name</span>";
			err = true;			
		}

		//To Check to range of value
		if(val.length < 3 || val.length > 20){
			document.getElementById("spnHandle").innerHTML = "<span class='errMes'>Enters characters between 3 to 20 of length</span>";
			err = true;			
		}
		
		//To test Alphanumeric Characters
		var chk = /^[a-z_0-9_.]*$/;		
		
		if (chk.test(val) == false){
			document.getElementById("spnHandle").innerHTML = "<span class='errMes'>invalid</span>";
			err = true;			
		}	

		if(val.substr(0,1) == '.'){
			document.getElementById("spnHandle").innerHTML = "<span class='errMes'>Should not Start with a Dot</span>";
			err = true;			
		}
		
		if(err == true){
			document.getElementById("hdnChk").Value = "True";
		}else{
			document.getElementById("spnHandle").innerHTML = "<span class='comMessage'>Congrats! This user name is available</span>";
			imgDone.src = "<%=ImagingFunctions.GetRootImagePath()%>/images/arrowicon.png";
		}
	}
	
</script>
</body>
   </html>