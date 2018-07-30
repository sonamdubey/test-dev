<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.UpcomingBikeHome" %>
<asp:Repeater ID="rptUpcomingBikes" runat="server">
    <ItemTemplate>
        <div class="grid_3">
            <div class="border-light border-radius5 content-block">
                <%--<img alt="<%# DataBinder.Eval( Container.DataItem, "BikeName" ) %>" title="<%# DataBinder.Eval( Container.DataItem, "BikeName" ) %>" src="https://<%# DataBinder.Eval( Container.DataItem, "HostUrl").ToString() %><%# DataBinder.Eval( Container.DataItem, "ImagePath").ToString() %>" hspace="0" vspace="0" border="0" style="width:200px;height:125px;" />--%>
                <img alt="<%# DataBinder.Eval( Container.DataItem, "BikeName" ) %>" title="<%# DataBinder.Eval( Container.DataItem, "BikeName" ) %>" src="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval( Container.DataItem, "OriginalImagePath").ToString(),DataBinder.Eval( Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._144x81) %>" hspace="0" vspace="0" border="0" style="width:200px;height:125px;" />
                <h3 class="margin-top5"><%# DataBinder.Eval( Container.DataItem, "BikeName" ) %></h3>
                <p><%# FormatedTopic( DataBinder.Eval( Container.DataItem, "Description" ).ToString()) %></p>
                <div class="margin-top15 readmore">
                    <a href="/<%# Bikewale.Common.UrlRewrite.FormatURL(DataBinder.Eval( Container.DataItem, "MakeName" ).ToString())+"-bikes/"+Bikewale.Common.UrlRewrite.FormatURL(DataBinder.Eval( Container.DataItem, "ModelName" ).ToString()) %>/">Read More</a>
                </div>                
            </div>
        </div>
    </ItemTemplate>
</asp:Repeater>