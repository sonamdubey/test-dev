<%@ Control Language="C#" AutoEventWireup="false" Inherits="Carwale.UI.Controls.AddPageListToLink"%>

<asp:HyperLink ID="prevSlot" runat="Server" Text="<<" ></asp:HyperLink>
<asp:Repeater id="rptLinkPageList" runat="server">
    <ItemTemplate>
          <a href='<%= pageUrl%>&page=<%# Container.DataItem %>'><%# Container.DataItem %> </a>   
    </ItemTemplate>
</asp:Repeater>
<asp:HyperLink ID="nextSlot" runat="Server" Text=">>" ></asp:HyperLink>

