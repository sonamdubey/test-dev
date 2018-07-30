<%@ Control Language="C#" Inherits="Bikewale.Mobile.Controls.LinkPagerControl" %>
<%if(TotalPages > 1 ) { %>

    <div id="pagination-list-content" class="pagination-list-content grid-7 alpha omega position-rel rightfloat">
    <ul id="pagination-list" class="pagination-list">
        <asp:Repeater ID="rptPager" runat="server">
            <ItemTemplate>
                <li  class="<%# Convert.ToInt16(DataBinder.Eval(Container.DataItem,"PageNo")) == CurrentPageNo ? "active" : "" %>">
                    <%# Convert.ToInt16(DataBinder.Eval(Container.DataItem,"PageNo")) == CurrentPageNo ? 
                    DataBinder.Eval(Container.DataItem,"PageNo") : 
                    "<a data-hash="+ string.Format("city={0}&make={1}&model={2}", CityId, MakeId,ModelId) + " data-bind='click: function (d,e) { ChangePageNumber(e) }' href=" + DataBinder.Eval(Container.DataItem,"PageUrl").ToString() + " data-pagenum="+ DataBinder.Eval(Container.DataItem,"PageNo") + ">" + DataBinder.Eval(Container.DataItem,"PageNo").ToString() + "</a>" %></li>
            </ItemTemplate>
        </asp:Repeater>
    </ul>
    <span class="pagination-control-prev <%= String.IsNullOrEmpty(prevPageUrl) ? "inactive" : "" %>">
        <a href="<%=prevPageUrl %>" class="bwmsprite bwsprite prev-page-icon" data-hash="<%= string.Format("city={0}&make={1}&model={2}", CityId, MakeId, ModelId) %>" data-pagenum="<%= CurrentPageNo >1 ? CurrentPageNo - 1: 1 %>"></a>
    </span>
    <span class="pagination-control-next <%= String.IsNullOrEmpty(nextPageUrl) ? "inactive" : "" %>">
        <a href="<%=nextPageUrl %>" class="bwmsprite bwsprite next-page-icon" data-hash="<%= string.Format("city={0}&make={1}&model={2}", CityId, MakeId,ModelId) %>" data-pagenum="<%= CurrentPageNo < TotalPages? CurrentPageNo + 1: TotalPages %>"></a>
    </span>
</div>
<div class="clear"></div>
<% } %>
