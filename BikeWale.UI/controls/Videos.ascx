<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Videos.ascx.cs" Inherits="Bikewale.Controls.Videos" %>

<asp:Repeater ID="rptLandingVideos" runat="server">
    <HeaderTemplate>
        <div class="content-box-shadow">
            <div class="grid-8">
                <a href="<%= String.Format("/bike-videos/{0}/",FirstVideoRecord.SubCatName) %>" class="main-video-container">
                    <img class="lazy" data-original="<%= String.Format("{0}/640x348/{1}",FirstVideoRecord.ImgHost,FirstVideoRecord.ImagePath)  %>" alt="<%= FirstVideoRecord.VideoTitle  %>" title="<%= FirstVideoRecord.VideoTitle  %>" src="<%= String.Format("{0}/640x348/{1}",FirstVideoRecord.ImgHost,FirstVideoRecord.ImagePath)  %>" border="0" />
                    <span><%= FirstVideoRecord.VideoTitle  %></span>
                </a>
            </div>
            <div class="grid-4">
                <ul>
    </HeaderTemplate>
    <ItemTemplate>

        <li>
            <a href="<%# String.Format("/bike-videos/{0}/",DataBinder.Eval(Container.DataItem,"SubCatName")) %>" class="sidebar-video-image">
                <img class="lazy" data-original="<%# String.Format("{0}/144x81/{1}",DataBinder.Eval(Container.DataItem,"ImgHost"),DataBinder.Eval(Container.DataItem,"ImagePath"))  %>" alt="<%# DataBinder.Eval(Container.DataItem,"VideoTitle") %>" title="<%# DataBinder.Eval(Container.DataItem,"VideoTitle") %>" src="<%# String.Format("{0}/144x81/{1}",DataBinder.Eval(Container.DataItem,"ImgHost"),DataBinder.Eval(Container.DataItem,"ImagePath"))  %>" border="0" /></a>
            <a href="<%# String.Format("/bike-videos/{0}/",DataBinder.Eval(Container.DataItem,"SubCatName")) %>" class="sidebar-video-title font14 text-light-grey"><%# DataBinder.Eval(Container.DataItem,"VideoTitle").ToString() %></a>
        </li>

    </ItemTemplate>
    <FooterTemplate>
        </ul>
        </div>
        <div class="clear"></div>
        </div>
    </FooterTemplate>
</asp:Repeater>
