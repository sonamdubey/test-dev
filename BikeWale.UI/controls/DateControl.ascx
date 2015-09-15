<%@ Control Language="C#" Inherits="Bikewale.Controls.DateControl" AutoEventWireUp="false" %>
<asp:CheckBox ID="chkDate" Visible="false" runat="server" data-role="none" />
<asp:DropDownList id="cmbDay" runat="server" CssClass="dayField"></asp:DropDownList>
<asp:DropDownList id="cmbMonth" runat="server" CssClass="monthField"></asp:DropDownList>
<asp:TextBox ID="txtYear" MaxLength="4" Columns="3" runat="server" CssClass="text yearField"></asp:TextBox>
<script language="javascript" src="/src/calender.js?15sept2015"></script>
<script language="javascript">
    var <%=this.ID%> = new Calender('<%=this.ID%>', <%=DateTime.Today.Year%>,<%=DateTime.Today.Month%>, <%=this.FutureTolerance%>);
	<%=this.ID%>.init();
</script>