<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Controls.NewsWidget" %>
<div class="bw-tabs-data" id="ctrlNews">
    <div class="jcarousel-wrapper">
        <div class="jcarousel">
            <ul>
                <asp:Repeater ID="rptNews" runat="server">
                    <ItemTemplate>
                        <li>
                            <div class="front">
                                <div class="contentWrapper">
                                    <div class="imageWrapper">
                                        <a href="/m/news/<%# DataBinder.Eval(Container.DataItem,"BasicId").ToString() + "-" + DataBinder.Eval(Container.DataItem,"ArticleUrl").ToString() %>.html">
                                            <img class="lazy" alt="<%# DataBinder.Eval(Container.DataItem, "Title").ToString()%>" title="<%# DataBinder.Eval(Container.DataItem, "Title").ToString()%>" data-original="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImgUrl").ToString(),DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._370x208) %>" src="http://img.aeplcdn.com/bikewaleimg/images/circleloader.gif" width="370" height="208">
                                        </a>
                                    </div>
                                    <div class="bikeDescWrapper">
                                        <div class="bikeTitle margin-bottom20">
                                            <h3>
                                                <a href="/m/news/<%# DataBinder.Eval(Container.DataItem,"BasicId").ToString() + "-" + DataBinder.Eval(Container.DataItem,"ArticleUrl").ToString() %>.html">
                                                    <%# DataBinder.Eval(Container.DataItem, "Title").ToString()%>
                                                </a>
                                            </h3>
                                        </div>
                                        <div class="margin-bottom10 text-light-grey"><%# Bikewale.Utility.FormatDate.GetDaysAgo(DataBinder.Eval(Container.DataItem, "DisplayDate").ToString()) %>, by <span class="text-light-grey"><%# DataBinder.Eval(Container.DataItem, "AuthorName").ToString()%></span></div>
                                    </div>
                                </div>
                            </div>
                        </li>
                    </ItemTemplate>
                </asp:Repeater>
            </ul>
        </div>
        <span class="jcarousel-control-left"><a href="javascript:void(0)" class="bwmsprite jcarousel-control-prev"></a></span>
        <span class="jcarousel-control-right"><a href="javascript:void(0)" class="bwmsprite jcarousel-control-next"></a></span>
        <p class="text-center jcarousel-pagination margin-bottom30"></p>
    </div>
    <div class="text-center margin-bottom30">
        <a class="font16" href="/m/news/">View More News</a>
    </div>
    <script type="text/javascript">
        $(document).ready(function () { $("img.lazy").lazyload(); });
    </script>
</div>
