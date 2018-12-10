<%@ Page Language="C#" Inherits="Carwale.UI.Forums.SplitPost" validateRequest="false" trace="false" AutoEventWireup="false" %>
<%@Import namespace="Carwale.UI.Common"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" itemscope itemtype="http://schema.org/WebPage">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
<link rel="stylesheet" href="/static/css/common.css" type="text/css" >
<link rel="stylesheet" href="/static/css/forums.css" type="text/css" >
<title itemprop="name">Car Forums | Split Posts</title>
</head>
<form runat="server">
<h3>Split Thread</h3>
<asp:Label ID="lblMessage" ForeColor="#FF0000" runat="server" EnableViewState="false" CssClass="error" />
<table cellpadding="2" border="0" cellspacing="0" width="100%">
	<tr>
		<td valign="top">
			<table width="100%" border="0" cellpadding="2" cellspacing="0">
				<tr>
					<td colspan="2">
						<asp:RadioButton ID="rdbNewThread" Checked="true" GroupName="threads" Text="New Thread" runat="server"></asp:RadioButton>
						<asp:RadioButton ID="rdbExistingThread" runat="server" Text="Existing Thread" GroupName="threads"></asp:RadioButton>
					</td>
				</tr>
				<tr>
					<td>&nbsp;</td>
					<td>&nbsp;</td>
				</tr>
				<tr>
				<td colspan="2" align="center">
					<div id="div1" style="display:none;" align="left">
						<table width="100%" border="0" cellpadding="2" cellspacing="0">
							<tr>
								<td width="80">Title : </td>
								<td><asp:TextBox ID="txtTitle" Columns="30" runat="server" /> 
										&nbsp;<span id="spnTitle" class="error"></span>
								</td>
							</tr>
							<tr>
								<td>Category : </td>
								<td><asp:DropDownList ID="cmbCategories" runat="server" />
										&nbsp;<span id="spnCategories" class="error"></span>
								</td>
							</tr>
							<tr>
							<td colspan="2" align="left">
								<asp:Button ID="butSave" runat="server" Text="Update" />
								<input type="button" value="Close" onClick="CloseAndRefresh();" />
							</td>
							</tr>
						</table>
					</div>
				</td>
			</tr>
			<tr>
				<td colspan="2" align="center">
					<div id="div2" style="display:none;" align="left">
					<table width="100%" border="0" cellpadding="2" cellspacing="0">
						<tr>
							<td width="80">Thread Id : </td>
							<td><asp:TextBox ID="txtThreadID" Columns="10" runat="server" />
									&nbsp;<span id="spnThreadID" class="error"></span>
							</td>
						</tr>
						<tr>
							<td colspan="2" align="left">
								<asp:Button ID="butSaveExisting" runat="server" Text="Update" />
								<input type="button" value="Close" onClick="CloseAndRefresh();" />
							</td>
						</tr>
					</table>
					</div>
				</td>
				</tr>
			</table>
		</td>
	</tr>
</table>
</form>
<script type="text/javascript">
    Common.showCityPopup = false;
	document.getElementById('rdbNewThread').onclick = rdbNewThread_Click;
	document.getElementById('rdbExistingThread').onclick = rdbExistingThread_Click;
	document.getElementById("butSave").onclick = butSave_Click;
	document.getElementById("butSaveExisting").onclick = butSaveExisting_Click;
	
	document.getElementById("div1").style.display = 'block';
	
	function rdbNewThread_Click()
	{
		document.getElementById("div1").style.display = 'block';
		document.getElementById("div2").style.display = 'none';
	}
	
	function rdbExistingThread_Click()
	{
		document.getElementById("div1").style.display = 'none';
		document.getElementById("div2").style.display = 'block';
	}
	
	function butSave_Click(e)
	{
		var isError = false;
		
		if ( document.getElementById('txtTitle') && document.getElementById('txtTitle').value.length == 0 )
		{
			isError = true;
			document.getElementById("spnTitle").innerHTML = "Title Required";
		}	
		else
			document.getElementById("spnTitle").innerHTML = "";
		
		if ( document.getElementById('cmbCategories').selectedIndex == 0 ) 
		{
			isError = true;
			document.getElementById('spnCategories').innerHTML = "Select Category";
		}
		else
			document.getElementById("spnCategories").innerHTML = "";
			
		if(isError == true)
			return false;
	}	
	
	function butSaveExisting_Click(e)
	{
		 var re = /^[0-9]*$/;
		var isError = false;
		
		if ( document.getElementById('txtThreadID') && document.getElementById('txtThreadID').value.length == 0 )
		{
			isError = true;
			document.getElementById("spnThreadID").innerHTML = "Thread ID Required";
		}	
		else
		{
			document.getElementById("spnThreadID").innerHTML = "";
			var threadId = document.getElementById("txtThreadID").value;
			
			if (threadId != "" && re.test(threadId) == false) 
			{
            	document.getElementById("spnThreadID").innerHTML = "Thread ID should be numeric";
           		return false;
			}
			
			var range = AjaxForum.GetTitle(threadId);
			if(confirm("Topic : " + range.value))
			{}
			else{return false;}
		}
		
		if(isError == true)
			return false;
	}
	
	function CloseAndRefresh()   
	{     
		//opener.location.reload(true); 
		window.opener.location.href = window.opener.location.href;
		self.close();
	}
	
</script>   
<!-- Footer ends here -->
