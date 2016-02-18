<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="videosbycategory.ascx.cs" Inherits="Bikewale.controls.videosbycategory" %>


<div class="container">
    <asp:Repeater ID="rptCategoryVideos" runat="server">
    <HeaderTemplate>
        <div class="grid-12">
        <h2 class="text-bold text-center margin-top40 margin-bottom20 font28">Expert reviews</h2>
    </HeaderTemplate>
    <ItemTemplate>
            <div class="grid-6 padding-left20">
                <div class="reviews-image-wrapper rounded-corner2">
                    <a href="<%# String.Format("/bike-videos/{0}/",DataBinder.Eval(Container.DataItem,"SubCatName")) %>">
                        <img class="lazy" data-original="<%# String.Format("{0}/144x81/{1}",DataBinder.Eval(Container.DataItem,"ImgHost"),DataBinder.Eval(Container.DataItem,"ImagePath"))  %>" alt="<%# DataBinder.Eval(Container.DataItem,"VideoTitle")%>" title="<%# DataBinder.Eval(Container.DataItem,"VideoTitle") %>" src="<%# String.Format("{0}/144x81/{1}",DataBinder.Eval(Container.DataItem,"ImgHost"),DataBinder.Eval(Container.DataItem,"ImagePath"))  %>" border="0"/>
                    </a>
                </div>
                <div class="reviews-desc-wrapper">
                    <a href="<%# String.Format("/bike-videos/{0}/",DataBinder.Eval(Container.DataItem,"SubCatName")) %>" class="text-default font14 text-bold"><%# DataBinder.Eval(Container.DataItem,"VideoTitle")%></a>
                    <p class="font12 text-light-grey margin-top10 margin-bottom10"><%# DataBinder.Eval(Container.DataItem,"DisplayDate")%></p>
                    <p class="font14 text-light-grey margin-bottom15"><%# DataBinder.Eval(Container.DataItem,"Description")%></p>
                    <div class="grid-4 alpha omega border-light-right font14">
                        <span class="bwsprite video-views-icon margin-right5"></span><span class="text-light-grey margin-right5">Views:</span><span class="text-default"><%# DataBinder.Eval(Container.DataItem,"Views")%></span>
                    </div>
                    <div class="grid-8 omega padding-left20 font14">
                        <span class="bwsprite video-likes-icon margin-right5"></span><span class="text-light-grey margin-right5">Likes:</span><span class="text-default"><%# DataBinder.Eval(Container.DataItem,"Likes")%></span>
                    </div>
                    <div class="clear"></div>
                </div>
            </div>
    </ItemTemplate>
    <FooterTemplate>
        <div class="clear"></div>
        <a href="" class="font16 text-center padding-top15 more-videos-link">View more videos</a>
        </div>
        <div class="clear"></div>
    </FooterTemplate>
</asp:Repeater>
</div>
