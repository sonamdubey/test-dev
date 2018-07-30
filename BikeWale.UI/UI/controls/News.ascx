<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.News_Widget" %>
<%@ Import Namespace="Bikewale.Utility" %>
<div id="ctrlNews">
    <div id="modelNewsContent" class="bw-model-tabs-data margin-right10 margin-left10 content-inner-block-2010 border-solid-bottom font14">
        <% if(ShowWidgetTitle) { %>
        <h2><%=WidgetTitle %> News</h2>
        <% } %>
        <!-- main content -->
        <asp:Repeater ID="rptNews" runat="server">
            <ItemTemplate>
                <div class="margin-bottom20">
                    <div class="grid-4 alpha">
                        <div class="model-preview-image-container">
                            <a href="<%#Bikewale.Utility.UrlFormatter.GetArticleUrl(DataBinder.Eval(Container.DataItem,"BasicId").ToString(),DataBinder.Eval(Container.DataItem,"ArticleUrl").ToString(),Bikewale.Entities.CMS.EnumCMSContentType.News.ToString())  %>">
                                <img class="lazy" data-original="<%#Bikewale.Utility.Image.GetPathToShowImages( DataBinder.Eval(Container.DataItem,"OriginalImgUrl").ToString(), DataBinder.Eval(Container.DataItem,"HostUrl").ToString() ,Bikewale.Utility.ImageSize._310x174) %>" title="<%#DataBinder.Eval(Container.DataItem,"Title").ToString() %>" alt="<%#DataBinder.Eval(Container.DataItem,"Title").ToString() %>" src="">
                            </a>
                        </div>
                    </div>
                    <div class="grid-8">
                        <a href="<%#Bikewale.Utility.UrlFormatter.GetArticleUrl(DataBinder.Eval(Container.DataItem,"BasicId").ToString(),DataBinder.Eval(Container.DataItem,"ArticleUrl").ToString(),Bikewale.Entities.CMS.EnumCMSContentType.News.ToString())  %>"  title="<%#DataBinder.Eval(Container.DataItem,"Title").ToString() %>" class="article-target-link line-height">
                            <%#DataBinder.Eval(Container.DataItem,"Title").ToString()%>
                        </a>
                        <div class="article-stats-left-grid">
                            <span class="bwsprite calender-grey-sm-icon"></span>
                            <span class="article-stats-content"><%#Bikewale.Utility.FormatDate.GetFormatDate(DataBinder.Eval(Container.DataItem,"DisplayDate").ToString(),"dd MMMM yyyy") %></span>
                        </div>
                        <div class="article-stats-right-grid">
                            <span class="bwsprite author-grey-sm-icon"></span>
                            <span class="article-stats-content"><%#DataBinder.Eval(Container.DataItem,"AuthorName").ToString()%></span>
                        </div>
                        <p class="line-height17 margin-top10">
                            <%#Bikewale.Utility.FormatDescription.TruncateDescription(DataBinder.Eval(Container.DataItem,"Description").ToString() ,280) %>
                        </p>
                    </div>
                    <div class="clear"></div>
                </div>

            </ItemTemplate>
        </asp:Repeater>
        <!-- main content -->

        <div class="more-article-target view-all-btn-container">
            <a href="<%= UrlFormatter.FormatNewsUrl(MakeMaskingName,ModelMaskingName) %>" title="<%= !String.IsNullOrEmpty(ModelMaskingName) ? String.Format("{0} {1} News", MakeName, ModelName) : (!String.IsNullOrEmpty(MakeMaskingName) ? String.Format("{0} News",MakeName) : "Bikes News") %>" class="btn view-all-target-btn">Read all news<span class="bwsprite teal-right"></span></a>
        </div>
    </div>
</div>
<!-- Ends here-->
