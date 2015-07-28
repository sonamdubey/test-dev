<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.FeaturedBike" %>
<asp:Repeater ID="rptFeaturedBike" runat="server">
    <ItemTemplate>
        <div <%#ControlWidth== "grid_2" ? "class='grid_2 alpha margin-top15'" :"class='grid_3'" %> >
            <div style='height:270px;' <%#ControlWidth== "grid_2" ? "class='content-block-inner'" :"class='border-light border-radius5 content-block'" %> >
                <a href="/<%#DataBinder.Eval( Container.DataItem, "MakeMaskingName" ).ToString()+"-bikes/"+DataBinder.Eval( Container.DataItem, "MaskingName" ).ToString() %>/">
                    <img alt="<%# DataBinder.Eval( Container.DataItem, "BikeName" ) %>" title="<%# DataBinder.Eval( Container.DataItem, "BikeName" ) %>" src="http://<%# DataBinder.Eval( Container.DataItem, "HostUrl").ToString() %><%# DataBinder.Eval( Container.DataItem, "ImagePath").ToString() %>" width="<%= ImageWidth %>" hspace="0" vspace="0" border="0" />
                </a>                
                <h3 class="margin-top5">
                    <a href="/<%# DataBinder.Eval( Container.DataItem, "MakeMaskingName" ).ToString()+"-bikes/"+DataBinder.Eval( Container.DataItem, "MaskingName" ).ToString() %>/"><%# DataBinder.Eval( Container.DataItem, "BikeName" ) %></a>
                </h3>
                <div class="margin-top5" style="<%= ControlWidth== "grid_2" ? "height:80px;" : "height:60px; "%>"><%# FormatedTopic( DataBinder.Eval( Container.DataItem, "Description" ).ToString()) %></div>
                <div class="margin-top5 readmore">
                    <a href="/<%# DataBinder.Eval( Container.DataItem, "MakeMaskingName" ).ToString()+"-bikes/"+DataBinder.Eval( Container.DataItem, "MaskingName" ).ToString() %>/">Read More</a>
                </div>                
            </div>
        </div>
    </ItemTemplate>
</asp:Repeater>