<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.TipsAdvicesMin" %>
<div id="divControl" runat="server" class="hide">
    <h2>Tips, Advices & Guides</h2>
    <p class="black-text">Bike tips, advices & guides from our experts</p>
    <div class="dotted-line margin-top5"></div>
    <div class="margin-top5 margin-bottom15">
        <asp:Repeater ID="rptTipsAdvices" runat="server">
            <HeaderTemplate>
                <ul class="std-ul-list">
            </HeaderTemplate>
            <ItemTemplate>
                <li><a href="/tipsadvices/<%# DataBinder.Eval(Container.DataItem,"Url") %>-<%# DataBinder.Eval(Container.DataItem,"BasicId") %>/"><%# DataBinder.Eval(Container.DataItem,"Title") %></a></li> 
            </ItemTemplate>
            <FooterTemplate>
                </ul>
            </FooterTemplate>
        </asp:Repeater>       
    </div>
    <div class="dotted-line margin-top5 margin-bottom15"></div>
    <div class="margin-bottom15 readmore">
        <a href="/tipsadvices/">More</a>
    </div>
</div>