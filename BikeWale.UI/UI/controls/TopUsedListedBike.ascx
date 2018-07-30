<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.TopUsedListedBike" %>
<%@ Import Namespace="Bikewale.Common" %>
<h<%= DisplayTwoColumn == false ? "3" : "2" %> class="margin-top5">Top Deals in your City</h<%= DisplayTwoColumn == false ? "3" : "2" %>>
<div id="noCarsMessage" runat="server" visible="false" class="margin-top10">BikeWale just got launched and this section will soon thrive with listings. Regret to keep you waiting. Come back soon!</div>
<div id="topUsedCarItems" runat="server" class="margin-top10">
    <ul class="<%= DisplayTwoColumn == false ? "std-ul-list" : "ul-hrz-col2" %>">
        <asp:Repeater ID="rptListings" runat="server">        
            <ItemTemplate>
                <li><a href="/used/bikes-in-<%# Convert.ToString(DataBinder.Eval( Container.DataItem, "CityMaskingName" )).Trim() %>/<%# DataBinder.Eval( Container.DataItem, "MakeMaskingName" ).ToString() %>-<%# DataBinder.Eval( Container.DataItem, "ModelMaskingName" ).ToString() %>-<%# DataBinder.Eval( Container.DataItem, "ProfileId" ) %>/"><%# String.Format("{0} {1} {2} (Year {3})", DataBinder.Eval( Container.DataItem, "MakeName" ),DataBinder.Eval( Container.DataItem, "ModelName" ), DataBinder.Eval( Container.DataItem, "VersionName" ) ,DataBinder.Eval( Container.DataItem, "MakeYear" )) %></a></li>
            </ItemTemplate>        
        </asp:Repeater> 
    </ul>          
</div>