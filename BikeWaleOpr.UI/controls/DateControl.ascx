<%@ Control Language="C#" Inherits="BikeWaleOpr.Controls.DateControl" AutoEventWireUp="false" %>
<asp:CheckBox ID="chkDate" Visible="false" runat="server" />
<asp:DropDownList id="cmbDay" runat="server"></asp:DropDownList>
<asp:DropDownList id="cmbMonth" runat="server"></asp:DropDownList>
<asp:TextBox ID="txtYear" MaxLength="4" Columns="3" runat="server" CssClass="text"></asp:TextBox>
<script language="javascript" src="/src/calender.js"></script>
<script language="javascript">
    var <%=this.ID%> = new Calender('<%=this.ID%>', <%=DateTime.Today.Year%>, <%=this.FutureTolerance%>);
	<%=this.ID%>.init();
</script>