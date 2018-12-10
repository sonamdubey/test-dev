<%@ Page Language="C#" validateRequest="false" Inherits="Carwale.UI.Forums.StickyThread" trace="false" AutoEventWireup="false" %>
<%@Import namespace="Carwale.UI.Common"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" itemscope itemtype="http://schema.org/WebPage">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
<link rel="stylesheet" href="/static/css/common.css" type="text/css" >
<link rel="stylesheet" href="/static/css/forums.css" type="text/css" >
<title itemprop="name">Car Forums | Sticky Thread</title>
</head>
<form runat="server">
<h3>Sticky Thread</h3>
<asp:Label ID="lblMessage" ForeColor="#FF0000" runat="server" EnableViewState="false" CssClass="error" />
<table cellpadding="2" border="0" cellspacing="0" width="100%">
	<tr>
		<td valign="top">
			<div id="divCreate" runat="server">
				<table width="100%" border="0" cellpadding="2" cellspacing="0">
						<tr>
							<td colspan="2">
								Do you want to make this Thread Sticky?
							</td>
						</tr>
						<tr>
							<td colspan="2">
								<asp:RadioButton ID="rdbStickyCat" Checked="true" GroupName="threads" Text="For this Category ?" runat="server"></asp:RadioButton>
								<asp:RadioButton ID="rdbStickyForum" runat="server" Text="For the complete Forum ?" GroupName="threads"></asp:RadioButton>
							</td>
						</tr>
						<tr>
							<td>&nbsp;</td>
							<td>&nbsp;</td>
						</tr>
						<tr>
							<td colspan="2" align="center">
								<table width="100%" border="0" cellpadding="2" cellspacing="0">
									<tr>
										<td colspan="2" align="left">
											<asp:Button ID="butSave" runat="server" Text="Save" />
											<input type="button" value="Close" onClick="CloseAndRefresh();" />
										</td>
									</tr>
								</table>
							</td>
						</tr>
					</table>
				</div>
				<div id="divRemove" runat="server">
					<table width="100%" border="0" cellpadding="2" cellspacing="0">
						<tr>
							<td colspan="2">
								Do you want to remove this from Sticky Thread?
							</td>
						</tr>
						<tr>
							<td>&nbsp;</td>
							<td>&nbsp;</td>
						</tr>
						<tr>
							<td colspan="2" align="center">
								<table width="100%" border="0" cellpadding="2" cellspacing="0">
									<tr>
										<td colspan="2" align="left">
											<asp:Button ID="btnDelete" runat="server" Text="Remove" CssClass="buttons" />
											<input type="button" value="Close" onClick="CloseAndRefresh();" class="buttons" />
										</td>
									</tr>
								</table>
							</td>
						</tr>
					</table>
				</div>
		</td>
	</tr>
</table>
</form>
<script type="text/javascript">
    Common.showCityPopup = false;
	function CloseAndRefresh()   
	{     
		//opener.location.reload(true); 
		window.opener.location.href = window.opener.location.href;
		self.close();
	}
</script>

<!-- Footer ends here -->
