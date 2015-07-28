<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Forums.CreateThreads" ValidateRequest="false" Trace="false" %>
<%@ Register TagPrefix="RTE" TagName="RichTextEditor" src="/Controls/RichTextEditor.ascx" %>
<!-- #include file="/includes/headForums.aspx" -->
<div class="container_12">
    <div class="grid_12">
        <ul class="breadcrumb">
            <li>You are here: </li>
            <li><a href="/">Home</a></li>
            <li class="fwd-arrow">&rsaquo;</li>
            <li><a href="/forums/">Forums</a></li>
            <li class="fwd-arrow">&rsaquo;</li>
            <li><a href="Viewforum-<%= forumId%>.html"><%= ForumName%></a></li>
            <li class="fwd-arrow">&rsaquo;</li>           
            <li class="current"><strong><%= ForumName %> Create Thread</strong></li>
        </ul><div class="clear"></div>
    </div>
	<div class="grid_12 margin-top10">		
		<form id="Form1" runat="server">
			<asp:Label ID="lblMessage" runat="server" EnableViewState="false" CssClass="error" Font-Bold="true" />
			<table class="writePost" border="0" cellpadding="2" cellspacing="0" width="100%">
				<tr>
					<td colspan="2"><p><%= ForumDescription %></p></td>
				</tr>
				<tr>
					<td colspan="2"><h1>Start New Discussion</h1></td>
				</tr>
				<tr>
					<td>Title:</td>
					<td>
						<asp:TextBox ID="txtTopic" runat="server" MaxLength="75" Columns="75" />
						<span id="spnName" class="error"></span>
					</td>
				</tr>
				<tr>
					<td width="120" valign="top">Description:</td>
					<td class="noPadding noBorder">
						<RTE:RichTextEditor id="rteNT" Rows="15" Cols="60" runat="server" ></RTE:RichTextEditor>
						<span id="spnDesc"></span><br>
						<span id="spnDescription" class="error"></span>
					</td>
				</tr>
				<tr id="trCustomer" runat="server">
					<td>Your Email:*</td>
					<td><asp:TextBox ID="txtEmail" runat="server" MaxLength="50" Columns="50" />
						<span id="spnEmail" class="error"></span>
					</td>
				</tr>
				<tr id="trCaptcha" runat="server">
					<td>&nbsp;</td>
					<td>
						<img src="/Common/CaptchaImage/JpegImage.aspx"><br>
						<div>
							<strong>Enter the code shown above</strong> (If you can't read it: <input type="submit" value="Regenerate Code" />)<br>
							<asp:TextBox id="txtCaptcha" Columns="6" MaxLength="6" runat="server"></asp:TextBox>
							<asp:Label id="lblCaptcha" CssClass="error" runat="server"></asp:Label>
						</div>
					</td>
				</tr>
				<tr id="trAlert" Visible="false" runat="server">
					<td>Email Alert</td>
					<td>
						<asp:CheckBox ID="chkEmailAlert" Text="Whenever someone replies to this thread, notify me by sending an email" Checked="true" runat="server" />
					</td>
				</tr>
				<tr>
					<td colspan="2" align="center">
						<asp:Button ID="butSave" runat="server" Text="Save" CssClass="buttons" />
						<input type="button" value="Cancel" onClick="javascript:location.href='Viewforum-<%= forumId%>.html'" class="buttons" />
					</td>
				</tr>
			</table>
		</form>
	</div>
</div>
<iframe id="ifrKeepAlive" src="/keepalive.html" frameBorder="no" width="0" height="0" runat="server"></iframe>
<script language="javascript">
    <!--
        document.getElementById("butSave").onclick = verifyForm;
	
    function verifyForm(e)
    {
        var isError = false;
		
        if(document.getElementById("txtTopic").value == "")
        {
            isError = true;
            document.getElementById("spnName").innerHTML = "Required";
        }
        else
            document.getElementById("spnName").innerHTML = "";
			
        var desc = tinyMCE.get('rteNT_txtContent').getContent();

        if( desc == "" )
        {
            isError = true;
            document.getElementById("spnDescription").innerHTML = "&nbsp;Why blank post? Please do write something!";
        }
        else if(desc.length > 4000)
        {
            isError = true;
            document.getElementById("spnDescription").innerHTML = "&nbsp;Posting can not be more than 4000 characters. Currently it is of " + desc.length + " characters. Please correct!";
        }
        else
            document.getElementById("spnDescription").innerHTML = "";
		
		
        if ( document.getElementById('txtEmail') && document.getElementById('txtEmail').value.length == 0 )
        {
            isError = true;
            document.getElementById("spnEmail").innerHTML = "&nbsp;Email needed!";
        }	
		
        if ( document.getElementById('txtCaptcha'))
        {
            if ( document.getElementById('txtCaptcha').value.length == 0 )
            {
                document.getElementById('lblCaptcha').innerHTML = "<br>Code Required";	
                return false;
            }
            else
            {
                document.getElementById('lblCaptcha').innerHTML = "";	
            }
        }
		
        if(isError == true)
            return false;
    }
    -->
</script>
<!-- #include file="/Includes/footerInner.aspx" -->
