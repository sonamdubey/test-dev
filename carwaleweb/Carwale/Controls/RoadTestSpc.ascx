<%@ Control Language="C#" AutoEventWireup="false" Inherits="Carwale.UI.Controls.RoadTestSpc" %>
<%@ Import Namespace="Carwale.UI.Common" %>
<div class="margin-bottom20">
    <ul id="roadTest">
        <asp:Repeater ID="rptRoadTestSpc" runat="server">
            <ItemTemplate>
                <li class="list-seperator">
                    <div class="grid-5 alpha">
                        <a href='<%# Eval("ArticleUrl")%>' >
                            <img class="lazy" width="227" height="128"
                                alt="<%#((Carwale.Entity.CMS.Articles.ArticleSummary)Container.DataItem).Title.ToString()%>"
                                title="<%#((Carwale.Entity.CMS.Articles.ArticleSummary)Container.DataItem).Title.ToString()%>"
                                src="https://imgd.aeplcdn.com/0x0/statics/grey.gif"
                                data-original='<%# Carwale.Utility.ImageSizes.CreateImageUrl(((Carwale.Entity.CMS.Articles.ArticleSummary)Container.DataItem).HostUrl.ToString(),
                                Carwale.Utility.ImageSizes._227X128,((Carwale.Entity.CMS.Articles.ArticleSummary)Container.DataItem).OriginalImgUrl.ToString()) %>' />
                        </a>
                    </div>
                    <div class="grid-7 omega <%# String.IsNullOrEmpty(((Carwale.Entity.CMS.Articles.ArticleSummary)Container.DataItem).HostUrl.ToString()+((Carwale.Entity.CMS.Articles.ArticleSummary)Container.DataItem).SmallPicUrl.ToString()) ? "" : "" %>">
                        <a class="font20 margin-top10" title="<%# ((Carwale.Entity.CMS.Articles.ArticleSummary)Container.DataItem).Title.ToString()%>" href='<%# Eval("ArticleUrl")%>'>
                            <%#((Carwale.Entity.CMS.Articles.ArticleSummary)Container.DataItem).Title.ToString()%>
                        </a>
                        <div class="font14 margin-top10">
                            <%# GetPubDate(((Carwale.Entity.CMS.Articles.ArticleSummary)Container.DataItem).DisplayDate.ToLongDateString()) %> by <%# ((Carwale.Entity.CMS.Articles.ArticleSummary)Container.DataItem).AuthorName.ToString() %>
                        </div>                       
                        <div class="margin-top10"><%# GetDesc(((Carwale.Entity.CMS.Articles.ArticleSummary)Container.DataItem).Description.ToString()) %></div>
                         <%--<div class="margin-top20 hide">
                            <a href="/<%# UrlRewrite.FormatSpecial(((Carwale.Entity.CMS.Articles.ArticleSummary)Container.DataItem).MakeName) + "-cars/" +((Carwale.Entity.CMS.Articles.ArticleSummary)Container.DataItem).MaskingName + "/expert-reviews-" + ((Carwale.Entity.CMS.Articles.ArticleSummary)Container.DataItem).BasicId %>/" class="btn btn-grey btn-md text-uppercase" title="<%#((Carwale.Entity.CMS.Articles.ArticleSummary)Container.DataItem).Title.ToString()%>">Read more</a>
                        </div>--%>
                    </div>
                    <div class="clear"></div>
                </li>
            </ItemTemplate>
        </asp:Repeater>
    </ul>
    <asp:Label ID="lblNotFound" runat="server" Visible="false" />
</div>


