<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.UpcomingBikesMin" %>
<div id="divControl" runat="server" class="hide">
    <h2><%= HeaderText %></h2>
    <asp:Repeater ID="rptUpcomingBikes" runat="server">
        <ItemTemplate>
            <li>
                <div class="<%= ControlWidth.ToLower() == "grid_2" ? "grid_2 alpha margin-top15" :"grid_3" %>">
                    <div style='min-height: 270px;' class="<%= ControlWidth.ToLower() == "grid_2" ?  "content-block-inner" : "border-light border-radius5 content-block" %>">
                        <a href="<%# GetLink(DataBinder.Eval( Container.DataItem, "MakeMaskingName" ).ToString(), DataBinder.Eval( Container.DataItem, "MaskingName" ).ToString()) %>">
                            <%--<img alt="<%# DataBinder.Eval( Container.DataItem, "BikeName" ) %>" title="<%# DataBinder.Eval( Container.DataItem, "BikeName" ) %>" src="http://<%# DataBinder.Eval( Container.DataItem, "HostUrl").ToString() %><%# DataBinder.Eval( Container.DataItem, "ImagePath").ToString() %>" width="<%= ImageWidth %>" hspace="0" vspace="0" border="0" />--%>
                            <img alt="<%# DataBinder.Eval( Container.DataItem, "BikeName" ) %>" title="<%# DataBinder.Eval( Container.DataItem, "BikeName" ) %>" src="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval( Container.DataItem, "OriginalImagePath").ToString(),DataBinder.Eval( Container.DataItem, "HostUrl").ToString() ,Bikewale.Utility.ImageSize._210x118) %>" width="<%= ImageWidth %>" hspace="0" vspace="0" border="0" />
                        </a>
                        <h3 class="margin-top5">
                            <a href="<%# GetLink(DataBinder.Eval( Container.DataItem, "MakeMaskingName" ).ToString(), DataBinder.Eval( Container.DataItem, "MaskingName" ).ToString()) %>"><%# DataBinder.Eval( Container.DataItem, "BikeName" ) %></a>
                        </h3>
                        <div class="margin-top5"><%# FormatedTopic( DataBinder.Eval( Container.DataItem, "Description" ).ToString()) %></div>
                        <div class="margin-top15 readmore">
                            <a href="<%# GetLink(DataBinder.Eval( Container.DataItem, "MakeMaskingName" ).ToString(), DataBinder.Eval( Container.DataItem, "MaskingName" ).ToString()) %>">Read More</a>
                        </div>
                    </div>
                </div>
            </li>
        </ItemTemplate>
    </asp:Repeater>
</div>
