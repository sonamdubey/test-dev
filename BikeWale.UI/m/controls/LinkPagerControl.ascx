<%@ Control Language="C#" Inherits="Bikewale.Mobile.Controls.LinkPagerControl" %>
<%if(TotalPages > 1 ) { %>

    <div class="grid-7 alpha omega position-rel">
    <ul id="pagination-list">
        <%--<% if(HideFirstLastUrl == false){ %>
        <li class="<%= !String.IsNullOrEmpty(firstPageUrl) ? "pg" : "pgEnd" %>"><%= !String.IsNullOrEmpty(firstPageUrl) ? "<a href=" + firstPageUrl + ">First</a>" : "First" %></li>
        <% } %>--%>
        <%--<li><%= !String.IsNullOrEmpty(prevPageUrl) ? "<a href=" + prevPageUrl + ">Prev</a>" : "Prev" %></li>--%>
        <%--<span class="listPager">--%>
        <asp:Repeater ID="rptPager" runat="server">
            <ItemTemplate>
                <li class="<%# Convert.ToInt16(DataBinder.Eval(Container.DataItem,"PageNo")) == CurrentPageNo ? "active" : "" %>">
                    <%# Convert.ToInt16(DataBinder.Eval(Container.DataItem,"PageNo")) == CurrentPageNo ? DataBinder.Eval(Container.DataItem,"PageNo") : "<a href=" + DataBinder.Eval(Container.DataItem,"PageUrl").ToString() + ">" + DataBinder.Eval(Container.DataItem,"PageNo").ToString() + "</a>" %></li>
            </ItemTemplate>
        </asp:Repeater>

        <%--</span>--%>
        <%--<li class="<%= !String.IsNullOrEmpty(nextPageUrl) ? "pg" : "pgEnd" %>"><%= !String.IsNullOrEmpty(nextPageUrl) ? "<a href=" + nextPageUrl + ">Next</a>" : "Next" %></li>--%>
        <%--<% if(HideFirstLastUrl == false){ %>
        <span class="<%= !String.IsNullOrEmpty(lastPageUrl) ? "pg" : "pgEnd" %>"><%= !String.IsNullOrEmpty(lastPageUrl) ? "<a href=" + lastPageUrl + ">Last</a>" : "Last" %></span>
        <%} %>--%>
    </ul>
    <span class="pagination-control-prev <%= String.IsNullOrEmpty(prevPageUrl) ? "inactive" : "" %>">
        <a href="<%=prevPageUrl %>" class="bwmsprite prev-page-icon"></a>
    </span>
    <span class="pagination-control-next <%= String.IsNullOrEmpty(nextPageUrl) ? "inactive" : "" %>">
        <a href="<%=nextPageUrl %>" class="bwmsprite next-page-icon"></a>
    </span>
</div>
<div class="clear"></div>
<% } %>
