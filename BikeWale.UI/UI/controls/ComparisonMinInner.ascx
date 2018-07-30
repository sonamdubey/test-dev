<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.ComparisonMin" %>
<h2>Popular Comparisons</h2>
<p class="black-text">Compare on price, features & performance</p>
<div class="margin-top10 margin-bottom15">
    <ul class="ul-hrz-col2">
        <asp:Repeater ID="rptComparison" runat="server">      
            <ItemTemplate>
                <li><a href="/new/compare/compareSpecs.aspx?bike1=<%# DataBinder.Eval( Container.DataItem, "VersionId1" ) %>&bike2=<%# DataBinder.Eval( Container.DataItem, "VersionId2" ) %>"><%# DataBinder.Eval( Container.DataItem, "Bike1" ) %>&nbsp;<span class="red-text">vs</span>&nbsp;<%# DataBinder.Eval( Container.DataItem, "Bike2" ) %></a></li>
            </ItemTemplate>     
        </asp:Repeater>
     </ul>                  
</div>
  