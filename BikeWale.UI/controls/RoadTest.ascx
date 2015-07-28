<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.RoadTest" %>
<div id="divControl" runat="server" class="hide">
    <h2><%= HeaderText%></h2>
    <asp:Repeater ID="rptRoadTest" runat="server">        
        <ItemTemplate>
            <li>
                <div class="<%= ControlWidth.ToLower() == "grid_2" ? "grid_2 alpha margin-top15" :"grid_3" %>">
                    <div style='height:270px;' class="<%= ControlWidth.ToLower() == "grid_2" ?  "content-block-inner" : "border-light border-radius5 content-block" %>">
                        <a href="<%# GetLink(DataBinder.Eval( Container.DataItem, "BasicId" ).ToString(), DataBinder.Eval( Container.DataItem, "ArticleUrl" ).ToString()) %>">
                            <img alt="<%# DataBinder.Eval( Container.DataItem, "Title" ) %>" title="<%# DataBinder.Eval( Container.DataItem, "Title" ) %>" src="http://<%# DataBinder.Eval( Container.DataItem, "HostUrl").ToString() %><%# DataBinder.Eval( Container.DataItem, "SmallPicUrl").ToString() %>" width="<%= ImageWidth %>" hspace="0" vspace="0" border="0" />
                        </a>
                        <h3 class="margin-top5">
                            <a href="<%# GetLink(DataBinder.Eval( Container.DataItem, "BasicId" ).ToString(), DataBinder.Eval( Container.DataItem, "ArticleUrl" ).ToString()) %>"><%# DataBinder.Eval( Container.DataItem, "Title" ) %></a>                            
                        </h3>                        
                        <div class="margin-top5"><%# FormatedTopic( DataBinder.Eval( Container.DataItem, "Description" ).ToString()) %></div>
                        <div class="margin-top15 readmore">
                            <a href="<%# GetLink(DataBinder.Eval( Container.DataItem, "BasicId" ).ToString(), DataBinder.Eval( Container.DataItem, "ArticleUrl" ).ToString()) %>">Read More</a>
                        </div>                
                    </div>
                </div>
            </li>
        </ItemTemplate>       
    </asp:Repeater>
</div>