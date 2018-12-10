<%@ Page Language="C#" Inherits="Carwale.UI.Community.Mods.Default" trace="false" AutoEventWireup="false" %>
<!doctype html>
<html>
<head>
    <!-- #include file="/includes/global/head-script.aspx" -->
    <link rel="stylesheet" href="/static/css/forums.css" type="text/css" >
    <style>
	    .panelStyle {border:1px solid #777777;width:400px;}
	    .panelStyle .panelHeader { background-color:#777777; font-size:12px; font-family:Verdana, Arial, Helvetica, sans-serif; font-weight:bold; color:#ffffff; }
	    .errorHandle {color:red;}
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
                            <li><span class="fa fa-angle-right margin-right10"></span><a href="/community/">Community</a></li>
                            <li><span class="fa fa-angle-right margin-right10"></span>Moderator's Home</li>
                        </ul>
                        <div class="clear"></div>
                    </div>                      
                    <h1 class="font30 text-black special-skin-text">Moderator's Home</h1>
                    <div class="border-solid-bottom margin-top10 margin-bottom15"></div>
                </div>
                <div class="clear"></div>
                <div class="grid-10">
                    <div class="content-box-shadow content-inner-block-10">
						 <div>
	                        <div style="margin-top:12px;">
		                        <table cellpadding="5" cellspacing="0" class="panelStyle">
			                        <tr>
				                        <td class="panelHeader">Manage a member</td>
			                        </tr>
			                        <tr>
				                        <td>
					                        <asp:TextBox ID="txtHandleToBan" runat="server" Columns="35" Text="Enter handle name" CssClass="text form-control inline-block" style="width:155px;" /> 
					                        <asp:Button ID="btnContinue" runat="server" Text="Continue" CssClass="btn btn-orange" style="width:100px; font-size:14px; padding:8px 5px;"/>
				                        </td>
			                        </tr>
			                        <tr>
				                        <td>
					                        <asp:Label ID="lblHandleToBanError" runat="server" Text="Handle name does not exists" Visible="false" CssClass="errorHandle" />	
				                        </td>
			                        </tr>
			                        <tr>
				                        <td>
					                        <asp:Label ID="lblCustomerIdToBan" runat="server" Text="" Visible="false" />
					                        <asp:Label ID="lblCustomerToBan" runat="server" Text="" /><br/><br/>
					                        <asp:Button ID="btnRemoveBan" runat="server" Text="Remove from banned list" Visible="false" CssClass="buttons" />
					                        <asp:Button ID="btnCustomerToBan" runat="server" Text="Ban this participant" Visible="false" CssClass="buttons" />&nbsp;
					                        <asp:Button ID="btnCancelCustomerToBan" runat="server" Text="Cancel" Visible="false" CssClass="buttons" />&nbsp;
				                        </td>
			                        </tr>
		                        </table>
	                        </div>
	
	                        <div style="margin-top:21px;">
		                        <table cellpadding="5" cellspacing="0" class="panelStyle">
			                        <tr>
				                        <td class="panelHeader">Restore/Reopen Thread</td>
			                        </tr>
			                        <tr>
				                        <td>
					                        <asp:TextBox ID="txtForumId" runat="server" Columns="12" Text="Enter thread id" CssClass="text form-control inline-block" style="width:155px;"  /> 
					                        <asp:Button ID="btnReactivateForum" runat="server" Text="Restore Thread" CssClass="btn btn-orange" style="width:110px; font-size:14px; padding:8px 5px;" />
					                        <asp:Button ID="btnReactivateForNewPosts" runat="server" Text="Reopen Thread" CssClass="btn btn-orange" style="width:110px; font-size:14px; padding:8px 5px;" />
				                        </td>
			                        </tr>
			                        <tr>
				                        <td>
					                        <asp:Label ID="lblForumMessage" runat="server" Text="" CssClass="errorHandle" /> 
				                        </td>
			                        </tr>
		                        </table>
	                        </div>		
	
	                        <div style="margin-top:21px;">
		                        <table cellpadding="5" cellspacing="0" class="panelStyle">
			                        <tr>
				                        <td class="panelHeader">Restore Post</td>
			                        </tr>
			                        <tr>
				                        <td>
					                        <asp:TextBox ID="txtForumThreadId" runat="server" Columns="12" Text="Enter post id" CssClass="text form-control inline-block" style="width:155px;"  /> 
					                        <asp:Button ID="btnReactivateForumThread" runat="server" Text="Restore Post" CssClass="btn btn-orange" style="width:110px; font-size:14px; padding:8px 5px;" />
				                        </td>
			                        </tr>
			                        <tr>
				                        <td>
					                        <asp:Label ID="lblForumThreadMessage" runat="server" Text="" CssClass="errorHandle" /> 
				                        </td>
			                        </tr>
		                        </table>
	                        </div>	
                        </div>	
					</div>
				</div>
                <div class="grid-2">
                    <div class="content-box-shadow content-inner-block-10">
						<div> 
                        <h3>Important Links</h3>
                        <ul style="list-style:none;">
	                        <li style="margin-top:7px;"><a href="ModerateReviews.aspx">Unverified reviews</a></li>
	                        <li style="margin-top:7px;"><a href="ModerateUpdatedReviews.aspx">Unverified updated reviews</a></li>
	                        <li style="margin-top:7px;"><a href="ModerateAbusedReviews.aspx">Abused reviews</a></li>
	                        <li style="margin-top:7px;"><a href="modReportAbuse.aspx">Reported Posts</a></li>
                            <li style="margin-top:7px;"><a href="NewsComments.aspx">News Comments</a></li>
                            <li style="margin-top:7px;"><a href="PostsInModeration.aspx">Posts In Moderation</a></li>
                        </ul>
                        </div>	
					</div>
				</div>  
	

				<div class="clear"></div>
            </div>
        </section>
        <div class="clear"></div>
        <!-- #include file="/includes/footer.aspx" -->
        <!-- all other js plugins -->
        <!-- #include file="/includes/global/footer-script.aspx" -->
        <script language="javascript" type="text/javascript">
            function CanceCustomerToBan() {
                $("#txtHandleToBan").removeAttr("disabled");
                $("#btnContinue").removeAttr("disabled");
                $("#lblCustomerToBan").hide();
                $("#btnCustomerToBan").hide();
                $("#btnCancelCustomerToBan").hide();
                $("#btnRemoveBan").hide();
            }

            function ForumReactivateConfirmation() {
                var resForumConfirm = confirm("Are you sure want to restore thread?");
                return resForumConfirm;
            }

            function ForumReactivateForNewPostsConfirmation() {
                var resForumConfirm = confirm("Are you sure want to reopen thread?");
                return resForumConfirm;
            }

            function ForumThreadReactivateConfirmation() {
                var resForumConfirm = confirm("Are you sure want to restore post?");
                return resForumConfirm;
            }
	</script>

</form>
</body>
</html>


