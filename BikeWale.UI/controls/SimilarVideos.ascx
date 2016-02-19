<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="SimilarVideos.ascx.cs" Inherits="Bikewale.controls.SimilarVideos" %>
<asp:Repeater ID="rptSimilarVideos" runat="server">
    <ItemTemplate>
            <li class="front">
                <div class="videocarousel-image-wrapper rounded-corner2">
                    <a href="">
                        <img class="lazy" data-original="<%#String.Format("http://img.youtube.com/vi/{0}/mqdefault.jpg",DataBinder.Eval(Container.DataItem,"VideoId")) %>" 
                            alt="<%#DataBinder.Eval(Container.DataItem,"VideoTitle") %>" title="<%#DataBinder.Eval(Container.DataItem,"VideoTitle") %>" src="" border="0" />
                    </a>
                </div>
                <div class="videocarousel-desc-wrapper">
                    <a href="" class="font14 text-bold text-default"><%# DataBinder.Eval(Container.DataItem,"VideoTitle") %></a>
                    <p class="font12 text-light-grey margin-top10 margin-bottom10"><%# Bikewale.Utility.FormatDate.GetFormatDate(DataBinder.Eval(Container.DataItem,"DisplayDate").ToString(),"MMMM dd, yyyy")  %></p>
                    <div class="grid-6 alpha omega border-light-right font14">
                        <span class="bwsprite video-views-icon margin-right5"></span><span class="text-light-grey margin-right5">Views:</span><span class="text-default"><%# DataBinder.Eval(Container.DataItem,"Views") %></span></div>
                    <div class="grid-6 omega padding-left20 font14">
                        <span class="bwsprite video-likes-icon margin-right5"></span><span class="text-light-grey margin-right5">Likes:</span><span class="text-default"><%# DataBinder.Eval(Container.DataItem,"Views") %></span></div>
                    <div class="clear"></div>
                </div>
            </li>
    </ItemTemplate>
</asp:Repeater>