<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PopularModelComparison.ascx.cs" Inherits="Bikewale.m.controls.PopularModelComparison" %>
<div id="ctrlCompareBikes">
    <div id="makeComparisonContent" class="bw-model-tabs-data padding-top15 padding-bottom20 font14">
        <h2 class="padding-left20 padding-right20 margin-bottom20">Popular Comparisons for <%=versionName %> </h2>
        <div class="swiper-container padding-top5 padding-bottom5">
            <div class="swiper-wrapper model-comparison-list">
                <asp:Repeater ID="rptPopularBikesComparison" runat="server">
                    <ItemTemplate>
                        <div class="swiper-slide">
                            <a href="/m/<%# Bikewale.Utility.UrlFormatter.CreateCompareUrl(DataBinder.Eval(Container.DataItem,"MakeMasking1").ToString(),DataBinder.Eval(Container.DataItem,"ModelMasking1").ToString(),DataBinder.Eval(Container.DataItem,"MakeMasking2").ToString(),DataBinder.Eval(Container.DataItem,"ModelMasking2").ToString(),DataBinder.Eval(Container.DataItem,"VersionId1").ToString(),DataBinder.Eval(Container.DataItem,"VersionId2").ToString()) %>">
                            <h3 class="font12 text-black text-center"><%# Bikewale.Utility.UrlFormatter.CreateCompareTitle(DataBinder.Eval(Container.DataItem, "Model1").ToString(),DataBinder.Eval(Container.DataItem, "Model2").ToString()) %></h3>
                            <div class="grid-6">
                                <div class="model-img-content">
                                    <img class="swiper-lazy" src="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImagePath1").ToString(),DataBinder.Eval(Container.DataItem, "HostUrl1").ToString(),Bikewale.Utility.ImageSize._310x174) %>" alt="" />
                                    <span class="swiper-lazy-preloader"></span>
                                </div>
                                <p class="font11 text-light-grey margin-bottom5">Ex-showroom ,<%=Bikewale.Utility.BWConfiguration.Instance.DefaultName %></p>
                                <span class="bwmsprite inr-dark-md-icon"></span>&nbsp;<span class="font16 text-default text-bold"><%# DataBinder.Eval(Container.DataItem, "Price1").ToString() %></span>
                            </div>
                            <div class="grid-6">
                                <div class="model-img-content">
                                    <img class="swiper-lazy" src="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImagePath2").ToString(),DataBinder.Eval(Container.DataItem, "HostUrl2").ToString(),Bikewale.Utility.ImageSize._310x174) %>" alt="" />
                                    <span class="swiper-lazy-preloader"></span>
                                </div>
                                <p class="font11 text-light-grey margin-bottom5">Ex-showroom ,<%=Bikewale.Utility.BWConfiguration.Instance.DefaultName %></p>
                                <span class="bwmsprite inr-dark-md-icon"></span>&nbsp;<span class="font16 text-default text-bold"><%# DataBinder.Eval(Container.DataItem, "Price2").ToString() %></span>
                            </div>
                            <div class="clear"></div>
                            <div class="margin-top10 text-center">
                                <span class="btn btn-white btn-size-1">Compare now</span>
                            </div>
                            </a>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
        <div class="margin-top15 margin-left20">
            <a href="/comparebikes/">View more comparisons<span class="bwmsprite blue-right-arrow-icon"></span></a>
        </div>
    </div>
    <div class="margin-right20 margin-left20 border-solid-bottom"></div>
</div>
