<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="VideoByCategory.ascx.cs" Inherits="Bikewale.Controls.VideoByCategory" %>


<section class="<%= SectionBackgroundClass %>">
    <div class="container">
        <div class="grid-12">
            <h2 class="text-bold text-center margin-top40 margin-bottom20 font28"><%= SectionTitle %></h2>
            <div class="jcarousel-wrapper firstride-jcarousel">
                <div class="jcarousel">
                    <ul> 
                        <asp:Repeater ID="rptVideosByCat" runat="server">
                            <ItemTemplate>
                                <li class="front">
                                    <div class="videocarousel-image-wrapper rounded-corner2">
                                        <a href="<%# Bikewale.Utility.UrlFormatter.VideoDetailPageUrl(DataBinder.Eval(Container.DataItem,"VideoTitleUrl").ToString(),DataBinder.Eval(Container.DataItem,"BasicId").ToString()) %>">
                                            <img class="lazy" data-original="<%#String.Format("https://img.youtube.com/vi/{0}/mqdefault.jpg",DataBinder.Eval(Container.DataItem,"VideoId")) %>"
                                                alt="<%#DataBinder.Eval(Container.DataItem,"VideoTitle") %>" title="<%#DataBinder.Eval(Container.DataItem,"VideoTitle") %>" src="" border="0" />
                                        </a>
                                    </div>
                                    <div class="videocarousel-desc-wrapper">
                                        <a href="<%# Bikewale.Utility.UrlFormatter.VideoDetailPageUrl(DataBinder.Eval(Container.DataItem,"VideoTitleUrl").ToString(),DataBinder.Eval(Container.DataItem,"BasicId").ToString()) %>" class="font14 text-bold text-default"><%# DataBinder.Eval(Container.DataItem,"VideoTitle") %></a>
                                        <p class="font12 text-light-grey margin-top10 margin-bottom10"><%# Bikewale.Utility.FormatDate.GetFormatDate(DataBinder.Eval(Container.DataItem,"DisplayDate").ToString(),"dd MMMM yyyy")  %></p>
                                        <div class="grid-6 alpha omega border-light-right font14">
                                            <span class="bwsprite video-views-icon margin-right5"></span><span class="text-light-grey margin-right5">Views:</span><span class="text-default"><%# Bikewale.Utility.Format.FormatPrice(DataBinder.Eval(Container.DataItem,"Views").ToString()) %></span>
                                        </div>
                                        <div class="grid-6 omega padding-left20 font14">
                                            <span class="bwsprite video-likes-icon margin-right5"></span><span class="text-light-grey margin-right5">Likes:</span><span class="text-default"><%# Bikewale.Utility.Format.FormatPrice(DataBinder.Eval(Container.DataItem,"Likes").ToString()) %></span>
                                        </div>
                                        <div class="clear"></div>
                                    </div>
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>
                </div>
                <span class="jcarousel-control-left"><a href="#" class="bwsprite jcarousel-control-prev"></a></span>
                <span class="jcarousel-control-right"><a href="#" class="bwsprite jcarousel-control-next"></a></span>
            </div>
            <div class="view-all-btn-container">
            <a title="<%= SectionTitle %> Bike Videos" href="<%= Bikewale.Utility.UrlFormatter.VideoByCategoryPageUrl(SectionTitle,(String.IsNullOrEmpty(CategoryIdList))?((int)CategoryId).ToString():CategoryIdList) %>" class="btn view-all-target-btn">View more videos<span class="bwsprite teal-right"></span></a>
                </div>
        </div>
        <div class="clear"></div>
    </div>
</section>

