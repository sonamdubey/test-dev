<%@ page language="C#" autoeventwireup="false" inherits="Carwale.UI.Dealer.DealerStockDetails" trace="false" debug="false" %>
<%@ register tagprefix="Vspl" tagname="RepeaterPager" src="/Controls/RepeaterPagerDealerStock.ascx" %>
<%@ import namespace="Carwale.UI.Common" %>
<%@ Import Namespace="Carwale.Utility" %>
<%@ import namespace="System.Data" %>
<vspl:repeaterpager id="rpgListings" resultname="Cars" showheadersvisible="true" pagerposition="TopBottom" runat="server">
	<asp:Repeater ID="rptListings" runat="server" EnableViewState="false">
	<headertemplate>
	    <ul id="ul-stock">
	</headertemplate>
	<itemtemplate>
		<li class="stock-item">
            <div class="item-image">
                <a title="Click here to View details" target="_blank" style="text-decoration:none;" href="/used/cars-in-<%# UrlRewrite.FormatSpecial(DataBinder.Eval(Container.DataItem, "CityName").ToString()) %>/<%# UrlRewrite.FormatSpecial(DataBinder.Eval(Container.DataItem, "MakeName").ToString()) %>-<%# DataBinder.Eval(Container.DataItem, "MaskingName").ToString() %>-<%# DataBinder.Eval(Container.DataItem, "ProfileId").ToString().ToLower() %>/">
               <%-- <div class="img"><%#GetCarImages(((DataRowView)Container.DataItem)["HostURL"].ToString(), ((DataRowView)Container.DataItem)["DirectoryPath"].ToString(),((DataRowView)Container.DataItem)["ImageUrlThumbSmall"].ToString())%></div>--%>
                     <div class="img"><%#GetCarImages(((DataRowView)Container.DataItem)["HostURL"].ToString(), ImageSizes._110X61,((DataRowView)Container.DataItem)["OriginalImgPath"].ToString())%></div>
                     </a>
                <%#((DataRowView)Container.DataItem)["CertificationId"].ToString() != "" ?"<span class='certified-label'></span>":"" %>                
                <div class='<%# ((DataRowView)Container.DataItem)["PhotoCount"].ToString() != "0" ? "photo-count" : "hide" %>'>
                    <span><%#((DataRowView)Container.DataItem)["PhotoCount"].ToString() %> Photos</span>
                </div>
            </div>
            <div class="item-details">
                <ul>
                    <li><a href="/used/cars-in-<%# UrlRewrite.FormatSpecial(DataBinder.Eval(Container.DataItem, "CityName").ToString()) %>/<%# UrlRewrite.FormatSpecial(DataBinder.Eval(Container.DataItem, "MakeName").ToString()) %>-<%# DataBinder.Eval(Container.DataItem, "MaskingName").ToString() %>-<%# DataBinder.Eval(Container.DataItem, "ProfileId").ToString().ToLower() %>/" target="_blank"><%# ((DataRowView)Container.DataItem)["Car"] %></a></li>
                    <li>₹ <%# CommonOpn.FormatNumeric(((DataRowView)Container.DataItem)["Price"].ToString() ) %></li>
                    <li>Kms.<%# ((DataRowView)Container.DataItem)["Kilometers"].ToString() == "0" ? "--" : CommonOpn.FormatNumeric( ((DataRowView)Container.DataItem)["Kilometers"].ToString() ) %></li>
                    <li><%# Convert.ToDateTime(((DataRowView)Container.DataItem)["MakeYr"]).ToString("MMM-yyyy")%></li>                    
                </ul>
            </div> 
            <div class="clear"></div>          
		</li>
	</itemtemplate>
	<footertemplate>
	    </ul>
	</footertemplate>
	</asp:Repeater>
</vspl:repeaterpager>
