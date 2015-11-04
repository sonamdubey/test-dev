<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.News_new" %>
<div class="bw-tabs-data" id="ctrlNews"><!-- News data code starts here-->
    <asp:Repeater ID="rptNews" runat="server">
        <ItemTemplate>
            <div class="padding-bottom30">
                <div class="grid-4 alpha">
                    <div class="img-preview">
                        <a href="/news/<%# DataBinder.Eval(Container.DataItem,"BasicId").ToString() + "-" + DataBinder.Eval(Container.DataItem,"ArticleUrl").ToString() %>.html"><img class="lazy" data-original="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImgUrl").ToString(),DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._310x174) %>" title="<%# DataBinder.Eval(Container.DataItem, "Title").ToString()%>" alt="<%# DataBinder.Eval(Container.DataItem, "Title").ToString()%>" src></a>
                    </div>
                </div>
                <div class="grid-8 omega">
                    <h2 class="margin-bottom10 font20"><a href="/news/<%# DataBinder.Eval(Container.DataItem,"BasicId").ToString() + "-" + DataBinder.Eval(Container.DataItem,"ArticleUrl").ToString() %>.html" class="text-black"><%# DataBinder.Eval(Container.DataItem, "Title").ToString()%></a></h2>
                    <p class="margin-bottom10 text-xt-light-grey font14"><%# Bikewale.Utility.FormatDate.GetDaysAgo(DataBinder.Eval(Container.DataItem, "DisplayDate").ToString()) %>, by <span class="text-light-grey"><%# DataBinder.Eval(Container.DataItem, "AuthorName").ToString()%></span></p>
                    <p class="margin-bottom15 font14 line-height"><%# Bikewale.Utility.FormatDescription.TruncateDescription(DataBinder.Eval(Container.DataItem, "Description").ToString()) %></p>
                    <div class="margin-bottom15">
                        <a href="/news/<%# DataBinder.Eval(Container.DataItem,"BasicId").ToString() + "-" + DataBinder.Eval(Container.DataItem,"ArticleUrl").ToString() %>.html" class="margin-right25 font14">Read full story</a>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </ItemTemplate>    
    </asp:Repeater>    
    <div class="padding-bottom30 text-center">
        <a href="/news/" class="font16">View more news</a>
    </div>
    <script type="text/javascript">
        $(document).ready(function () { $("img.lazy").lazyload(); });
    </script>
</div><!-- Ends here-->