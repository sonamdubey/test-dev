<%@ Control Language="C#" Inherits="Bikewale.Controls.LinkPagerControl" %>
 <%if(TotalPages > 1 ) { %>
<div class="margin-top15">
    <span>
        <% if(HideFirstLastUrl == false){ %>
        <span class="<%= !String.IsNullOrEmpty(firstPageUrl) ? "pg" : "pgEnd" %>"><%= !String.IsNullOrEmpty(firstPageUrl) ? "<a href=" + firstPageUrl + ">First</a>" : "First" %></span>
        <% } %>
        <span class="<%= !String.IsNullOrEmpty(prevPageUrl) ? "pg" : "pgEnd" %>"><%= !String.IsNullOrEmpty(prevPageUrl) ? "<a href=" + prevPageUrl + ">Prev</a>" : "Prev" %></span>
    </span>
    <span class="listPager">
        <asp:Repeater ID="rptPager" runat="server">
            <ItemTemplate>
                <span class="<%# Convert.ToInt16(DataBinder.Eval(Container.DataItem,"PageNo")) == CurrentPageNo ? "pgSel" : "pg" %>"><%# Convert.ToInt16(DataBinder.Eval(Container.DataItem,"PageNo")) == CurrentPageNo ? DataBinder.Eval(Container.DataItem,"PageNo") : "<a href=" + DataBinder.Eval(Container.DataItem,"PageUrl").ToString() + ">" + DataBinder.Eval(Container.DataItem,"PageNo").ToString() + "</a>" %></span>
            </ItemTemplate>
        </asp:Repeater>
    </span>
    <span>
        <span class="<%= !String.IsNullOrEmpty(nextPageUrl) ? "pg" : "pgEnd" %>"><%= !String.IsNullOrEmpty(nextPageUrl) ? "<a href=" + nextPageUrl + ">Next</a>" : "Next" %></span>
        <% if(HideFirstLastUrl == false){ %>
        <span class="<%= !String.IsNullOrEmpty(lastPageUrl) ? "pg" : "pgEnd" %>"><%= !String.IsNullOrEmpty(lastPageUrl) ? "<a href=" + lastPageUrl + ">Last</a>" : "Last" %></span>
        <%} %>
    </span>
</div>
<% } %>
