<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Forums.Reply" ValidateRequest="false" Trace="false" %>
<%@ Register TagPrefix="RTE" TagName="RichTextEditor" src="/Controls/RichTextEditor.ascx" %>
<!-- #include file="/includes/headForums.aspx" -->
<script language="javascript">
    <!--
        function showCharactersLeft(ftb)
        {
            var maxSize = 4000;
            var size = ftb.GetHtml().length;
		
            if(size >= maxSize)
            {
                ftb.SetHtml( ftb.GetHtml().substring(0, maxSize -1) );
                size = maxSize;
            }
		
            document.getElementById("spnDesc").innerHTML = "Characters Left : " + (maxSize - size);
        }
    -->
</script>
<div class="container_12">
    <div class="grid_12">
        <ul class="breadcrumb">
            <li>You are here: </li>
            <li><a href="/">Home</a></li>
            <li class="fwd-arrow">&rsaquo;</li>
            <li><a href="/forums/">Forums</a></li>
            <li class="fwd-arrow">&rsaquo;</li>
            <li><a href="Viewforum-<%= ForumId%>.html"><%= ForumName%></a></li>
            <li class="fwd-arrow">&rsaquo;</li>
            <li><a href="viewthread-<%= threadId%>.html"><%= ForumName%> Bikes</a></li>
            <li class="fwd-arrow">&rsaquo;</li>
            <li class="current"><strong><%= ForumName %> Reply Thread</strong></li>
        </ul><div class="clear"></div>
    </div>
	<div class="grid_12 margin-top10">		
		<form id="Form1" runat="server">
			<asp:Label ID="lblMessage" runat="server" EnableViewState="false" CssClass="error" Font-Bold="true" />
			<table class="writePost" width="100%" border="0" cellpadding="3" cellspacing="0">
				<tr>
					<td colspan="2"><h1>Reply to Thread: <%= GetTitle(ThreadName) %></h1></td>					
				</tr>
				<tr>
					<td width="150" valign="top">Reply:</td>
					<td>
						<RTE:RichTextEditor id="rteRT" Rows="15" Cols="60" runat="server" />	
						<span id="spnDesc"></span><span id="spnDescription" class="error"></span>
					</td>
				</tr>
				<tr id="trCustomer" runat="server">
					<td>Email</td>
					<td><asp:TextBox ID="txtEmail" runat="server" MaxLength="50" Columns="50" /><span id="spnEmail" class="error"></span></td>						
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
					<td>&nbsp;</td>
					<td class="dtHeader" align="center">
						<asp:Button ID="butSave" CssClass="buttons" runat="server" Text="Save" />
						<input type="button" class="buttons" value="Cancel" onClick="javascript:location.href='Viewforum-<%= threadId%>.html'" />
					</td>
				</tr>
			</table>
			<h1><span>Previous</span> Posts in this Thread (Latest 10 are shown)</h1>
			<div id="divThread" runat="server">
				<asp:Repeater ID="rptThreads" runat="server">
					<headertemplate>
						<table border="0" width="100%" class="bdr" cellpadding="5" cellspacing="0">
					</headertemplate>
					<itemtemplate>
							<tr>
								<td width="20%" valign="top">
									<%# DataBinder.Eval(Container.DataItem, "PostedBy") %>
								</td>
								<td>
									<%# GetMessage( DataBinder.Eval(Container.DataItem, "Message").ToString()) %>
								</td>
							</tr>
					</itemtemplate>
					<alternatingitemtemplate>
							<tr bgcolor="#f0f0f0">
								<td valign="top">
									<%# DataBinder.Eval(Container.DataItem, "PostedBy") %>
								</td>
								<td>
									<%# GetMessage( DataBinder.Eval(Container.DataItem, "Message").ToString()) %>
								</td>
							</tr>
					</alternatingitemtemplate>
					<footertemplate>
						</table>
					</footertemplate>
				</asp:Repeater>
			</div>
		
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
		
        var desc = tinyMCE.get('rteRT_txtContent').getContent();
			
        if( desc == "" )
        {
            isError = true;
            document.getElementById("spnDescription").innerHTML = "&nbsp;Why blank reply? Please do write something!";
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
