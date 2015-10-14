<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.ExpertReviews" %>
<div class="bw-tabs-data" id="ctrlExpertReviews"><!-- Reviews data code starts here-->
    <asp:Repeater ID="rptExpertReviews" runat="server">
        <ItemTemplate>
            <div class="padding-bottom30">
                <div class="grid-4 alpha">
                    <div class="img-preview">
                        <a href="/road-tests/<%# DataBinder.Eval(Container.DataItem,"ArticleUrl").ToString() + "-" + DataBinder.Eval(Container.DataItem,"BasicId").ToString() %>.html"><img class="lazy" data-original="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImgUrl").ToString(),DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._310x174) %>" title="<%# DataBinder.Eval(Container.DataItem, "Title").ToString()%>" alt="<%# DataBinder.Eval(Container.DataItem, "Title").ToString()%>" src="http://img.aeplcdn.com/bikewaleimg/images/circleloader.gif" width="310" height="174"></a>
                    </div>
                </div>
                <div class="grid-8 omega">
                    <h2 class="margin-bottom10 font20"><a href="/road-tests/<%# DataBinder.Eval(Container.DataItem,"ArticleUrl").ToString() + "-" + DataBinder.Eval(Container.DataItem,"BasicId").ToString() %>.html" class="text-black"><%# DataBinder.Eval(Container.DataItem, "Title").ToString()%></a></h2>
                    <p class="margin-bottom10 text-xt-light-grey font14"><%# Bikewale.Utility.FormatDate.GetDaysAgo(DataBinder.Eval(Container.DataItem, "DisplayDate").ToString()) %>, by <span class="text-light-grey"><%# DataBinder.Eval(Container.DataItem, "AuthorName").ToString()%></span></p>
                    <p class="margin-bottom15 font14 line-height"><%# Bikewale.Utility.FormatDescription.TruncateDescription(DataBinder.Eval(Container.DataItem, "Description").ToString()) %></p>
                    <div class="margin-bottom15">
                        <a href="/road-tests/<%# DataBinder.Eval(Container.DataItem,"ArticleUrl").ToString() + "-" + DataBinder.Eval(Container.DataItem,"BasicId").ToString() %>.html" class="margin-right25 font14">Read full review</a>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </ItemTemplate>
    </asp:Repeater>    
    <div class="padding-bottom30 text-center">
        <a href="/road-tests/" class="font16">View more reviews</a>
    </div>
    <script type="text/javascript">
        $(document).ready(function () { $("img.lazy").lazyload(); });
    </script>
</div><!-- Ends here-->