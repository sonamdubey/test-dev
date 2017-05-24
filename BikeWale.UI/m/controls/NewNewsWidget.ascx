<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Controls.NewNewsWidget" %>
<%@ Import Namespace="Bikewale.Utility" %>
<% if(ShowWidgetTitle) { %>
<h2 class="text-bold"><%= WidgetTitle %> News</h2>
<% } %>
<asp:Repeater ID="rptNews" runat="server">
    <ItemTemplate>
        <div class="margin-bottom20">
            <div class="review-image-wrapper">
                <a href="/m/news/<%# DataBinder.Eval(Container.DataItem,"BasicId").ToString() + "-" + DataBinder.Eval(Container.DataItem,"ArticleUrl").ToString() %>.html" >
                    <img class="lazy" data-original="<%#Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem,"OriginalImgUrl").ToString(), DataBinder.Eval(Container.DataItem,"HostUrl").ToString() ,Bikewale.Utility.ImageSize._144x81) %>" title="<%#DataBinder.Eval(Container.DataItem,"Title").ToString() %>" alt="<%#DataBinder.Eval(Container.DataItem,"Title").ToString() %>"  />
                </a>
            </div>
            <div class="review-heading-wrapper">
                <a href="/m/news/<%#String.Format("{0}-{1}.html", DataBinder.Eval(Container.DataItem,"BasicId").ToString(),DataBinder.Eval(Container.DataItem,"ArticleUrl").ToString()) %>" title="<%#DataBinder.Eval(Container.DataItem,"Title").ToString() %>"  class="target-link"><%# Bikewale.Utility.FormatDescription.TruncateDescription(DataBinder.Eval(Container.DataItem, "Title").ToString(), 44) %></a>
                <div class="grid-7 alpha padding-right5">
                    <span class="bwmsprite calender-grey-sm-icon"></span>
                    <span class="article-stats-content"><%#Bikewale.Utility.FormatDate.GetFormatDate(DataBinder.Eval(Container.DataItem,"DisplayDate").ToString(), "dd MMMM yyyy") %></span>
                </div>
                <div class="grid-5 alpha omega">
                    <span class="bwmsprite author-grey-sm-icon"></span>
                    <span class="article-stats-content"><%# DataBinder.Eval(Container.DataItem,"AuthorName").ToString()%></span>
                </div>
                <div class="clear"></div>
            </div>
            <p class="margin-top10">
                <%# Bikewale.Utility.FormatDescription.TruncateDescription(DataBinder.Eval(Container.DataItem,"Description").ToString(),180) %>
            </p>
        </div>
    </ItemTemplate>
</asp:Repeater>

<div class="view-all-btn-container">
    <a href="/m<%= UrlFormatter.FormatNewsUrl(MakeMaskingName,ModelMaskingName) %>" title="<%= !String.IsNullOrEmpty(ModelMaskingName) ? String.Format("{0} {1} News", MakeName, ModelName) : (!String.IsNullOrEmpty(MakeMaskingName) ? String.Format("{0} News",MakeName) : "Bikes News") %>" class="btn view-all-target-btn">Read all news<span class="bwmsprite teal-right"></span></a>
</div>

<div class="bw-tabs-data" id="ctrlNews">  
</div>
