<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.News_Widget" %>

<!-- #include file="/ads/Ad976x204.aspx" -->
<div id="ctrlNews"><!-- News data code starts here-->    

    <div class="margin-top20 margin-right10 margin-left10 border-solid-top"></div>
    <div id="modelNewsContent" class="bw-model-tabs-data padding-top20 font14">
       <h2 class="padding-left20 padding-right20"><%=WidgetTitle %> News</h2>
       <div class="margin-bottom10">
                            <div class="grid-8 padding-left20 border-light-right">
                                <div class="padding-bottom5">
                                    <div class="model-preview-image-container leftfloat">
                                        <a href="javascript:void(0)">
                                            <img class="lazy" data-original="http://imgd1.aeplcdn.com//310x174//bw/ec/21352/TVS-Wego-Front-threequarter-63408.jpg?wm=0&t=193955533&t=193955533" title="" alt="" />
                                        </a>
                                    </div>
                                    <div class="model-news-title-container leftfloat">
                                        <h3 class="margin-top5"><a href="<%=firstPost.ArticleUrl %>" class="font16 text-black line-height"><%=firstPost.Title %></a></h3>
                                        <p class="text-light-grey margin-bottom15"><%= firstPost.DisplayDate.ToString("") %>, by <span class="text-light-grey"><%=firstPost.AuthorName%></span></p>
                                    </div>
                                    <div class="clear"></div>
                                    <p class="margin-top20 line-height17"><%= Bikewale.Utility.FormatDescription.TruncateDescription(firstPost.Description,150) %>
                                        <a href="<%=firstPost.ArticleUrl %>">Read full story</a>
                                    </p>
                                </div>
                            </div>
                            <div class="grid-4">
                                <ul>
                                    <asp:Repeater ID="rptNews" runat="server">
                                        <HeaderTemplate>
                                            <!-- #include file="/ads/Ad976x204.aspx" -->
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <li>
                                                <h3 class="red-bullet-point">
                                                    <a href="/news/<%# DataBinder.Eval(Container.DataItem,"BasicId").ToString() + "-" + DataBinder.Eval(Container.DataItem,"ArticleUrl").ToString() %>.html" class="text-black"><%# DataBinder.Eval(Container.DataItem, "Title").ToString()%></a>
                                                </h3>
                                                <p class="text-light-grey margin-left15"><%# Bikewale.Utility.FormatDate.GetFormatDate(DataBinder.Eval(Container.DataItem, "DisplayDate").ToString(), "MMMM dd, yyyy") %>, by <span class="text-light-grey"><%# DataBinder.Eval(Container.DataItem, "AuthorName").ToString()%></span></p>
                                            </li>
                                        </ItemTemplate>

                                    </asp:Repeater>
                                </ul>
                            </div>
                            <div class="clear"></div>
                        </div>
       
       <div class="grid-12 model-single-news margin-bottom20 omega padding-left20 hide"><!-- when one news -->
                            <div class="model-preview-image-container leftfloat">
                                <a href="javascript:void(0)">
                                    <img class="lazy" data-original="http://imgd1.aeplcdn.com//310x174//bw/ec/21352/TVS-Wego-Front-threequarter-63408.jpg?wm=0&t=193955533&t=193955533" title="" alt="" />
                                </a>
                            </div>
                            <div class="model-news-title-container leftfloat">
                                <h3 class="margin-top5"><a href="" class="font16 text-black line-height">Bajaj Avenger 220 Cruise vs Royal Enfield Thunderbird 350 : Comparison Test</a></h3>
                                <p class="text-light-grey margin-bottom15">April 15, 2016, by Sagar Bhanushali</p>
                                <p class="margin-top20 line-height17">I was excited when I got an email from Bajaj Motorcycles to test their new motorcycle, the Pulsar RS200, at their Chakan test track. And there were two reasons...
                                    <a href="">Read full story</a>
                                </p>
                            </div>
                            <div class="clear"></div>
                        </div>

       <div class="padding-left20">
           <a href="/news/" >Read all news<span class="bwsprite blue-right-arrow-icon"></span></a>
       </div>
     </div>


    <script type="text/javascript">
        $(document).ready(function () { $("img.lazy").lazyload(); });
    </script>
</div><!-- Ends here-->