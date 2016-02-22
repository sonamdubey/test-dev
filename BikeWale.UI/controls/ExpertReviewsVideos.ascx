<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExpertReviewsVideos.ascx.cs" Inherits="Bikewale.Controls.ExpertReviewVideos" %>


<section class="bg-white">
    <div class="container">
        <div class="grid-12">
            <h2 class="text-bold text-center margin-top40 margin-bottom20 font28"><%= SectionTitle %></h2>
            <asp:Repeater ID="rptCategoryVideos" runat="server">
                <ItemTemplate>
                    <div class="grid-6 ">
                        <div class="reviews-image-wrapper rounded-corner2">
                            <a href="<%# Bikewale.Utility.UrlFormatter.VideoDetailPageUrl(DataBinder.Eval(Container.DataItem,"VideoTitleUrl").ToString(),DataBinder.Eval(Container.DataItem,"BasicId").ToString()) %>">
                                <img class="lazy" data-original="<%# String.Format("https://img.youtube.com/vi/{0}/sddefault.jpg",DataBinder.Eval(Container.DataItem,"VideoId"))  %>" alt="<%# DataBinder.Eval(Container.DataItem,"VideoTitle") %>" title="<%# DataBinder.Eval(Container.DataItem,"VideoTitle") %>" src="<%# String.Format("https://img.youtube.com/vi/{0}/sddefault.jpg",DataBinder.Eval(Container.DataItem,"VideoId"))  %>" border="0" />
                            </a>
                        </div>
                        <div class="reviews-desc-wrapper">
                            <a href="<%# Bikewale.Utility.UrlFormatter.VideoDetailPageUrl(DataBinder.Eval(Container.DataItem,"VideoTitleUrl").ToString(),DataBinder.Eval(Container.DataItem,"BasicId").ToString()) %>" class="text-default font14 text-bold"><%# DataBinder.Eval(Container.DataItem,"VideoTitle")%></a>
                            <div class="font12 text-light-grey margin-top10 margin-bottom10"><%# Bikewale.Utility.FormatDate.GetFormatDate(DataBinder.Eval(Container.DataItem,"DisplayDate").ToString(),"MMMM dd, yyyy") %></div>
                            <div class="font14 text-light-grey margin-bottom15"><%# DataBinder.Eval(Container.DataItem,"Description")%></div>
                            <div class="grid-4 alpha omega border-light-right font14">
                                <span class="bwsprite video-views-icon margin-right5"></span><span class="text-light-grey margin-right5">Views:</span><span class="text-default"><%# Bikewale.Utility.Format.FormatPrice(DataBinder.Eval(Container.DataItem,"Views").ToString())%></span>
                            </div>
                            <div class="grid-8 omega padding-left20 font14">
                                <span class="bwsprite video-likes-icon margin-right5"></span><span class="text-light-grey margin-right5">Likes:</span><span class="text-default"><%# Bikewale.Utility.Format.FormatPrice(DataBinder.Eval(Container.DataItem,"Likes").ToString()) %></span>
                            </div>
                            <div class="clear"></div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
            <div class="clear"></div>
            <a href="<%= Bikewale.Utility.UrlFormatter.VideoByCategoryPageUrl(FirstVideoRecord.SubCatName,((int)CategoryId).ToString()) %>" class="font16 text-center padding-top15 more-videos-link">View more videos</a>
        </div>
        <div class="clear"></div>
    </div>
</section>







