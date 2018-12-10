<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MyContactDetails.aspx.cs" Inherits="Carwale.UI.MyCarwale.MyContactDetails" %>
<%@ Import Namespace="Carwale.UI.Common" %>
<%@ Import Namespace="Carwale.UI.Users" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="Carwale.Utility" %>
<!doctype html>
<html>
<head>
<!-- #include file="/includes/global/head-script.aspx" -->
<%
	// Define all the necessary meta-tags info here.
	// To know what are the available parameters,
	// check page, headerCommon.aspx in common folder.
	
	PageId 			= 71;
	Title 			= "MyContact Details";
	Description 	= "";
	Keywords		= "";
	Revisit 		= "15";
	DocumentState 	= "Dynamic";
    AdId            = "1337162297840";
    AdPath          = "/7590/CarWale_MyCarWale/CarWale_MyCarWale_Misc/CarWale_MyCarWale_Misc_";
%>
<script  type="text/javascript"  src="/static/src/ajaxfunctionsrqforums.js" ></script>
<style type="text/css">
	.dashborder{border-bottom:dotted thin #DCDCDC;}
	.insetborder{border-right:inset thin;}
	.textDeco{font-weight: bold;}
    .show {display:block;}
    .hide {display:none;}
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
.ui-tabs-panel { border: 1px solid #6598CC; padding: 10px; background: #fff; min-height:50px; display:block;margin-top:-5px;} /* declare background color for container to avoid distorted fonts in IE while fading */  
.ui-tabs-nav {display: inline-block;}/* auto clear @ IE 6 & IE 7 Quirks Mode */
</style>
</head>
<body class="bg-light-grey header-fixed-inner">
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
                    <div class="breadcrumb margin-bottom15">
                        <!-- breadcrumb code starts here -->
                        <ul class="special-skin-text">
                            <li><a href="/">Home</a></li>
                            <li><span class="fa fa-angle-right margin-right10"></span><a href="default.aspx">My CarWale</a></li>
                            <li><span class="fa fa-angle-right margin-right10"></span>My Profile</li>
                        </ul>
                        <div class="clear"></div>
                    </div>
                </div>
                <div class="clear"></div>
                <div class="grid-12">
	                <div id="content" style="width:100%;">
		                <div id="uiTabs">
				            <ul class="ui-tabs-nav">
					            <li class="ui-tabs-selected"><a href="#tb1"><span>My Profile</span></a></li>				
					            <li><a href="/mycarwale/EditCustomerDetails.aspx"><span>Update Contact Details</span></a></li>				
					            <li><a href="/users/EditUserProfile.aspx"><span>Update Community Profile</span></a></li>				
					            <li><a href="/users/changePassword.aspx"><span>Change Password</span></a></li>								
				            </ul>
				            <!--My Community Profile Content-->	
				            <div  id="tb1" class="ui-tabs-panel" >	
					            <table width="100%" class="RedBarMid" border="0" cellpadding="5" cellspacing="0">
						            <tr>
							            <td colspan="3" class="dashborder">
								         <br />
							            </td>
						            </tr>
						            <tr style="vertical-align:top;">
							            <td style="background:#E7F8FF;" class="insetborder" align="right"><strong>Contact Details</strong></td>
							            <td></td>
						            </tr>						
						            <tr>
							            <td align="right" class="insetborder" width="200px"><span class="textDeco">Name :</span></td>
							            <td><%=cd.Name%> </td>
						            </tr>
						            <tr>
							            <td align="right" class="insetborder"><span class="textDeco">e-Mail :</span></td>
							            <td><%=cd.Email%></td>
						            </tr>
						            <tr>
							            <td align="right" class="insetborder"><span class="textDeco">Phone :</span></td>
							            <td><%=cd.Phone1%></td>
						            </tr>
						            <tr>
							            <td align="right" class="insetborder"><span class="textDeco">Mobile :</span></td>
							            <td><%=cd.Mobile %></td>
						            </tr>
						            <tr>
							            <td align="right" class="insetborder"><span class="textDeco">City :</span></td>
							            <td><%=cd.City%></td>
						            </tr>
						            <tr>
							            <td align="right" class="insetborder"><span class="textDeco">State :</span></td>
							            <td><%=cd.State%></td>
						            </tr>
						            <tr style="padding:0px;">
							            <td class="dashborder" style="border-right:inset thin;">&nbsp;</td>
							            <td colspan="2" class="dashborder" style="text-align:right;">[<a href="/mycarwale/EditCustomerDetails.aspx">Update Contact Details</a>]</td>
						            </tr>
						            <tr style="vertical-align:top;">
							            <td style="background:#E7F8FF;" class="insetborder" align="right"><strong>Community Profile Details</strong></td>
							            <td colspan="2"></td>
						            </tr>	
						            <tr>
							            <td align="right" class="insetborder"><span class="textDeco">Community User Name :</span></td>
							            <td><%=result.HandleName%></td>
						            </tr>
						            <tr>
							            <td align="right" valign="top" class="insetborder"><span class="textDeco">About You :</span></td>
							            <td><%=result.AboutMe%></td>
						            </tr>
						            <tr>
							            <td align="right" valign="top" class="insetborder"><span class="textDeco">Signature :</span></td>
							            <td><%=result.Signature%></td>
						            </tr>
						            <tr>
							            <td align="right" valign="top" class="insetborder"><span class="textDeco">Avtar :</span></td>
							            <td>
                                                <div id='dtlstPhotosA_<%= userId%>' class='<%= result.StatusId=="1" ? "hide" : "show" %>'>                                         
                                                <img  src='<%= ImageSizes.CreateImageUrl(result.HostURL,ImageSizes._110X61,result.AvtOriginalImgPath) %>' width="100px" height="100px" />
                                            </div>
                                            <div id='dtlstPhotosPending_<%= userId%>' class='pending <%= result.StatusId=="1" ? "show" : "hide" %>' pending="<%= result.StatusId =="1" ? "true" : "false" %>">
                                                <p style="color:#555555;font-weight:bold;">
                                                Processing...
                                                <img  align="center" src='https://imgd.aeplcdn.com/0x0/statics/loader.gif'/>
                                                </p>
                                            </div>
                                        </td>
						            </tr>
						            <tr>
							            <td align="right" valign="top" class="insetborder"><span class="textDeco">Your Picture :</span></td>
							            <td>
                                                <div id='dtlstPhotosR_<%= userIdReal%>' class='<%= result.StatusId=="1" ? "hide" : "show" %>'>                                           
                                                <img  src='<%= ImageSizes.CreateImageUrl(result.HostURL,ImageSizes._160X89,result.RealOriginalImgPath) %>' width="100px" height="100px"/>
                                                </div>
                                            <div id='dtlstPhotosPending_<%= userIdReal%>' 
                                                class='pending <%= result.StatusId=="1" ? "show" : "hide" %>' 
                                                pending="<%= result.StatusId =="1" ? "true" : "false" %>">
                                    
                                                <p style="color:#555555;font-weight:bold;">
                                                Processing...
                                                <img  align="center" src='https://imgd.aeplcdn.com/0x0/statics/loader.gif'/>
                                                </p>
                                            </div>
							            </td>
						            </tr>
						            <tr>
							            <td class="insetborder">&nbsp;</td>
							            <td colspan="2" style="text-align:right;">[<a href="/users/EditUserProfile.aspx">Update Community Profile</a>]</td>
						            </tr>
					            </table>
				            </div>
			            </div>	               
		            </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
         <!-- #include file="/includes/footer.aspx" -->
        <!-- all other js plugins -->
        <!-- #include file="/includes/global/footer-script.aspx" -->
        <script type="text/javascript">
            var imgCategory = '<%= imgCategory%>';
        </script>
     </form>
</body>
</html>


