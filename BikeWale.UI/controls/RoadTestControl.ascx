<%@ Control Language="C#" AutoEventWireup="true" Inherits="Bikewale.Controls.RoadTest" %>
<div id="divControl" runat="server" class="hide grid_8">
    <h2><a class="link-decoration" href="road-tests/" title="<%= HeaderText %>"><%= HeaderText %> </a></h2>
    <asp:Repeater ID="rptRoadTest" runat="server"> 
        <ItemTemplate>
            <div class="margin-top10">
                <div class="grid_2 alpha">
                    <a href='<%# GetLink(DataBinder.Eval( Container.DataItem, "BasicId" ).ToString(), DataBinder.Eval( Container.DataItem, "ArticleUrl" ).ToString()) %>'>
                        <img alt="<%# DataBinder.Eval( Container.DataItem, "Title")%>" title="<%# DataBinder.Eval( Container.DataItem, "Title" ) %>" src="http://<%# DataBinder.Eval( Container.DataItem, "HostUrl").ToString() %><%# DataBinder.Eval( Container.DataItem, "SmallPicUrl").ToString() %>" width="<%= ImageWidth %>" style="border:1px solid #E5E4E4;" hspace="0" vspace="0" border="0" />
                    </a>
                </div>
                <div class="grid_6 omega">
                    <h3>
                        <a title="<%# DataBinder.Eval(Container.DataItem, "Title").ToString() %>" href="<%# GetLink(DataBinder.Eval(Container.DataItem, "BasicId").ToString(),DataBinder.Eval(Container.DataItem, "ArticleUrl").ToString()) %>">
                            <b><%# DataBinder.Eval(Container.DataItem, "Title").ToString() %></b>
                        </a>
                    </h3>
                    <div class="margin-bottom5"><abbr><%# Convert.ToDateTime(DataBinder.Eval(Container.DataItem, "DisplayDate")).ToString("MMM dd, yyyy")%></abbr> by <%# DataBinder.Eval(Container.DataItem, "AuthorName")%></div>
                    <div class="margin-top10 readmore">
				        <%# TruncateDesc(DataBinder.Eval(Container.DataItem, "Description").ToString()) %> <a href="<%# GetLink(DataBinder.Eval( Container.DataItem, "BasicId" ).ToString(), DataBinder.Eval( Container.DataItem, "ArticleUrl" ).ToString()) %>">Read More</a>
			        </div>
                </div>
                <div class="clear"></div>
            </div>
        </ItemTemplate>
    </asp:Repeater>
</div>