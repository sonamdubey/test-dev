<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.ComparisonMin_old" %>
<div class="grid_8 alpha omega margin-bottom15">
    <h2 class="grid_4 omega margin-top10" title="Compare on road price, features & performance">Popular Comparisons</h2>
    <span class="grid_4 alpha omega black-text margin-top15 margin-right10 right-align">Compare on road price, features & performance</span>
</div>
<div class="grid_4 alpha omega margin-bottom15">
    <div class="margin-bottom15 center-align">
        <a class="link-decoration" href="/comparebikes/<%=makeMaskingName1%>-<%=modelMaskingName1%>-vs-<%= makeMaskingName2%>-<%= modelMaskingName2%>/" >
            <%=bike1%>&nbsp;<span class="red-text">vs</span>&nbsp;<%=bike2 %>
        </a>
    </div>
    <a href="/comparebikes/<%=makeMaskingName1%>-<%=modelMaskingName1%>-vs-<%= makeMaskingName2%>-<%= modelMaskingName2%>/">
        <img id="imgCompBike" src="<%=imageUrl %>" border="0" width="300" />
    </a>        
    <div class="grid_2 alpha omega margin-top10 right-align" style="border-right:1px solid #E2E2E2;">
        <div class="margin-right10"><%= Bikewale.Common.CommonOpn.GetRateImage(review1) %></div>
        <div class="margin-top10 margin-right10">Rs. <%= Bikewale.Common.CommonOpn.FormatPrice(price1) %></div>
        <div class="margin-top10 margin-right10">
            <a class="link-decoration <%=reviewCount1 == "0" ? "hide" : "" %>" href="/<%= makeMaskingName1%>-bikes/<%=modelMaskingName1 %>/user-reviews/" ><%= reviewCount1 %> reviews</a>
            <a class="link-decoration <%=reviewCount1 == "0" ? "show" : "hide" %>" href="/content/userreviews/writereviews.aspx?bikem=<%= modelId1%>">Write a review</a>
        </div>
    </div>
    <div class="grid_2 omega margin-top10">
        <div><%= Bikewale.Common.CommonOpn.GetRateImage(review2) %></div>
        <div class="margin-top10">Rs. <%= Bikewale.Common.CommonOpn.FormatPrice(price2) %></div>
        <div class="margin-top10">
            <a class='link-decoration <%=reviewCount2 == "0" ? "hide" : "" %>' href="/<%= makeMaskingName2%>-bikes/<%=modelMaskingName2 %>/user-reviews/" ><%= reviewCount2 %> reviews</a>
            <a class="link-decoration <%=reviewCount2 == "0" ? "show" : "hide" %>" href="/content/userreviews/writereviews.aspx?bikem=<%= modelId2%>">Write a review</a>
        </div>
    </div>
</div>
<div class="grid_4 margin-bottom15 alpha">
    <asp:Repeater ID="rptComparison" runat="server">
        <HeaderTemplate>
            <ul class="no-std-ul-list">
        </HeaderTemplate>
        <ItemTemplate>
            <li>
                <div class="center-align">
                    <div class="margin-top10"><a href="/comparebikes/<%#DataBinder.Eval(Container.DataItem,"MakeMaskingName1")%>-<%#DataBinder.Eval(Container.DataItem,"ModelMaskingName1")%>-vs-<%#DataBinder.Eval(Container.DataItem,"MakeMakingName2")%>-<%#DataBinder.Eval(Container.DataItem,"ModelMaskingName2")%>/"><%# DataBinder.Eval( Container.DataItem, "Bike1" ) %>&nbsp;<span class="red-text">vs</span>&nbsp;<%# DataBinder.Eval( Container.DataItem, "Bike2" ) %></a></div>
                    <div class="margin-top5 margin-bottom10">
                        <span>Rs. <%# Bikewale.Common.CommonOpn.FormatPrice(DataBinder.Eval(Container.DataItem,"Price1").ToString()) %></span>
                        <span class="margin-left20">Rs. <%# Bikewale.Common.CommonOpn.FormatPrice(DataBinder.Eval(Container.DataItem,"Price2").ToString()) %></span>
                    </div>
                </div>
            </li>
            <div class="dotted-line"></div>
        </ItemTemplate>
        <FooterTemplate>
            </ul>
        </FooterTemplate>
    </asp:Repeater>
    <div id="compButton" class="center-align margin-top10 margin-bottom10 action-btn" runat="server">            
        <a href="/comparebikes/">More Comparisons</a>
    </div>
</div>




<%--<div class="dotted-line margin-top5"></div>--%>
<%--<div class="grid_4 column alpha omega" style="background-color:red;">
    <div class="margin-top10 margin-bottom15">
        <div><img id="imgCompBike" src="<%=imageUrl %>"/></div>
    </div>
</div>--%>
<%--<div class="grid_4 column alpha omega" >
    <div class="margin-top10 margin-bottom15">
    <asp:Repeater ID="rptComparison" runat="server">
        <HeaderTemplate>
            <ul class="no-std-ul-list">
        </HeaderTemplate>
        <ItemTemplate>
            <li><a href="/new/compare/compareSpecs.aspx?bike1=<%# DataBinder.Eval( Container.DataItem, "VersionId1" ) %>&bike2=<%# DataBinder.Eval( Container.DataItem, "VersionId2" ) %>"><%# DataBinder.Eval( Container.DataItem, "Bike1" ) %>&nbsp;<span class="red-text">vs</span>&nbsp;<%# DataBinder.Eval( Container.DataItem, "Bike2" ) %></a></li>
        </ItemTemplate>
        <FooterTemplate>
            </ul>
        </FooterTemplate>
    </asp:Repeater>                  
    </div>
</div>--%>
<%--<div class="dotted-line margin-bottom15"></div>
<div class="margin-top15 margin-bottom15 readmore">
    <a href="/new/compare/">More</a>
</div>--%>

  