<%@ Control Language="C#" Inherits="Carwale.UI.Controls.DateControl" AutoEventWireUp="false" %>
<asp:CheckBox ID="chkDate" Visible="false" runat="server" />
<asp:DropDownList id="cmbDay" runat="server"></asp:DropDownList>
<asp:DropDownList id="cmbMonth" runat="server"></asp:DropDownList>
<asp:TextBox ID="txtYear" MaxLength="4" Columns="4" runat="server" CssClass="text" Style="width:42px;"></asp:TextBox>
<script  language="javascript"  src="/static/src/calender.js" ></script>
<script language="javascript">
	var <%=this.ID%> = new Calender('<%=this.ID%>', <%=DateTime.Today.Year%>, <%=this.FutureTolerance%>);
	<%=this.ID%>.init();
</script>