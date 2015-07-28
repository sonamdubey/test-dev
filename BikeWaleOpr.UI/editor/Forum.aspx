<%@ Page Language="C#" ContentType="text/html" ValidateRequest="false" ResponseEncoding="iso-8859-1" %>
<%@ Register TagPrefix="Vspl" TagName="RTE" src="/Controls/RichTextEditor.ascx" %>

<script language="c#" runat="server">
	void butSaveClick(object sender, EventArgs e)
	{
		//Response.Write(rteRT.Content);
	}
</script>
<html>
<head></head>

<body>
<form runat="server">
	<Vspl:RTE id="rteRT" Rows="15" Cols="80" runat="server" />
	<asp:Button ID="but" Text="Save" runat="server" OnClick="butSaveClick"></asp:Button>
</form>
</body>
</html>