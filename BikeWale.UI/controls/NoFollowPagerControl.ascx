<%@ Control Language="C#"  AutoEventWireup="false" Inherits="Bikewale.Controls.NoFollowPagerControl" %>
 <%if(TotalPages > 1 ) { %>
<div class="margin-top10">
    <span>
        <% if(HideFirstLastUrl == false){ %>
        <span class="<%= !String.IsNullOrEmpty(firstPageUrl) ? "pg pointer" : "pgEnd" %>" <%= !String.IsNullOrEmpty(firstPageUrl) ? "url='" + firstPageUrl +"'": "" %>>First</span>
        <% } %>
        <span class="<%= !String.IsNullOrEmpty(prevPageUrl) ? "pg pointer" : "pgEnd" %>" <%= !String.IsNullOrEmpty(prevPageUrl) ? "url='" + prevPageUrl + "'" : "" %>">Prev</span>
    </span>
    <span class="listPager">
        <asp:Repeater ID="rptPager" runat="server">
            <ItemTemplate>
                <span class="<%# Convert.ToInt16(DataBinder.Eval(Container.DataItem,"PageNo")) == CurrentPageNo ? "pgSel" : "pg pointer" %>" <%# Convert.ToInt16(DataBinder.Eval(Container.DataItem,"PageNo")) == CurrentPageNo ? "" : "url='" + DataBinder.Eval(Container.DataItem,"PageUrl").ToString() + "'" %>>  <%# DataBinder.Eval(Container.DataItem,"PageNo").ToString() %></span>
            </ItemTemplate>
        </asp:Repeater>
    </span>
    <span>
        <span class="<%= !String.IsNullOrEmpty(nextPageUrl) ? "pg pointer" : "pgEnd" %>" <%= !String.IsNullOrEmpty(nextPageUrl) ? "url='" + nextPageUrl + "'" : "" %>>Next</span>
        <% if(HideFirstLastUrl == false){ %>
        <span class="<%= !String.IsNullOrEmpty(lastPageUrl) ? "pg pointer" : "pgEnd" %>" <%= !String.IsNullOrEmpty(lastPageUrl) ? "url='" + lastPageUrl + "'" : "" %>>Last</span>
        <%} %>
    </span>
</div>
<% } %>
