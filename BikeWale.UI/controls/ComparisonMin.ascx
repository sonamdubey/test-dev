<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.ComparisonMin" %>

<div class="grid-6">
    <a href="" title="Compare Bike 1 vs Bike 2" id="main-compare-bikes-target">
        <div class="grid-6 padding-left20 border-light-right">
            <div class="imageWrapper margin-bottom30">
                <div class="comparison-image">
                    <img class="lazy" data-original="http://imgd1.aeplcdn.com//210x118//bw/models/honda-cb-shine-electric-start/drum/alloy-112.jpg" />
                </div>
            </div>
            <h3 class="font18 text-black margin-bottom5">Hero Honda Hero Honda Hero Honda</h3>
            <div class="text-default text-bold">
                <span class="bwsprite inr-md-lg"></span>&nbsp;<span class="font22">92,000</span><span class="font16">&nbsp;onwards</span>
            </div>
            <p class="text-light-grey margin-bottom5 font14">Ex-showroom, Mumbai</p>
        </div>
        <div class="grid-6 padding-right20">
            <div class="imageWrapper margin-bottom30">
                <div class="comparison-image">
                    <img class="lazy" data-original="http://imgd1.aeplcdn.com//210x118//bw/models/honda-cb-shine-electric-start/drum/alloy-112.jpg" />
                </div>
            </div>
            <h3 class="font18 text-black margin-bottom5">Hero Honda</h3>
            <div class="text-default text-bold">
                <span class="bwsprite inr-md-lg"></span>&nbsp;<span class="font22">92,000</span><span class="font16">&nbsp;onwards</span>
            </div>
            <p class="text-light-grey margin-bottom5 font14">Ex-showroom, Mumbai</p>
        </div>
        <div class="clear"></div>
        <div class="margin-top20 text-center">
            <span class="btn btn-orange">Compare now</span>
        </div>
    </a>
</div>
<div class="grid-6 font14">
    <ul id="compare-sidebar-list">
        <li>
            <a href="" title="Compare Bike 1 vs Bike 2" class="compare-bikes-target">
                <div class="grid-6 compare-bike-box">
                    <div class="sidebar-image inline-block">
                        <img src="http://imgd1.aeplcdn.com//110x61//bw/models/bajaj-pulsar-rs200.jpg" />
                    </div>
                    <div class="sidebar-image-label inline-block text-default">
                        <h3 class="text-black text-bold margin-bottom5">Harley Davidson Heritage</h3>
                        <span class="bwsprite inr-sm-dark"></span>&nbsp;92,000 onwards
                    </div>
                </div>
                <div class="grid-6 compare-bike-box">
                    <div class="sidebar-image inline-block">
                        <img src="http://imgd1.aeplcdn.com//110x61//bw/models/bajaj-pulsar-rs200.jpg" />
                    </div>
                    <div class="sidebar-image-label inline-block text-default">
                        <h3 class="text-black text-bold margin-bottom5">Harley</h3>
                        <span class="bwsprite inr-sm-dark"></span>&nbsp;92,000 onwards
                    </div>
                </div>
                <div class="clear"></div>
            </a>
        </li>
        <li>
            <a href="" title="Bike 1 v Bike 2" class="compare-bikes-target">
                <div class="grid-6 compare-bike-box">
                    <div class="sidebar-image inline-block">
                        <img src="http://imgd1.aeplcdn.com//110x61//bw/models/bajaj-pulsar-rs200.jpg" />
                    </div>
                    <div class="sidebar-image-label inline-block text-default">
                        <h3 class="text-black text-bold margin-bottom5">Harley Davidson Heritage</h3>
                        <span class="bwsprite inr-sm-dark"></span>&nbsp;92,000 onwards
                    </div>
                </div>
                <div class="grid-6 compare-bike-box">
                    <div class="sidebar-image inline-block">
                        <img src="http://imgd1.aeplcdn.com//110x61//bw/models/bajaj-pulsar-rs200.jpg" />
                    </div>
                    <div class="sidebar-image-label inline-block text-default">
                        <h3 class="text-black text-bold margin-bottom5">Harley</h3>
                        <span class="bwsprite inr-sm-dark"></span>&nbsp;92,000 onwards
                    </div>
                </div>
                <div class="clear"></div>
            </a>
        </li>
        <li>
            <a href="" title="Bike 1 v Bike 2" class="compare-bikes-target">
                <div class="grid-6 compare-bike-box">
                    <div class="sidebar-image inline-block">
                        <img src="http://imgd1.aeplcdn.com//110x61//bw/models/bajaj-pulsar-rs200.jpg" />
                    </div>
                    <div class="sidebar-image-label inline-block text-default">
                        <h3 class="text-black text-bold margin-bottom5">Harley Davidson Heritage</h3>
                        <span class="bwsprite inr-sm-dark"></span>&nbsp;92,000 onwards
                    </div>
                </div>
                <div class="grid-6 compare-bike-box">
                    <div class="sidebar-image inline-block">
                        <img src="http://imgd1.aeplcdn.com//110x61//bw/models/bajaj-pulsar-rs200.jpg" />
                    </div>
                    <div class="sidebar-image-label inline-block text-default">
                        <h3 class="text-black text-bold margin-bottom5">Harley</h3>
                        <span class="bwsprite inr-sm-dark"></span>&nbsp;92,000 onwards
                    </div>
                </div>
                <div class="clear"></div>
            </a>
        </li>
    </ul>
    <div class="text-center">
        <a href="">View more comparisons</a>
    </div>
</div>
<div class="clear"></div>

    <div class="grid-6 margin-top20 margin-bottom20">
        <div class="border-solid-right">
            <h3 class="font16 text-center padding-bottom15">
                <a href="<%= FormatComparisonUrl(TopRecord.MakeMaskingName1,TopRecord.ModelMaskingName1,TopRecord.MakeMaskingName2,TopRecord.ModelMaskingName2, TopRecord.VersionId1.ToString(), TopRecord.VersionId2.ToString())%>">
                    <%= FormatBikeCompareAnchorText(TopRecord.Bike1,TopRecord.Bike2) %>
                </a>
            </h3>
            <div class="bike-preview compare-now-image-preview margin-bottom10">
                <a href="<%= FormatComparisonUrl(TopRecord.MakeMaskingName1,TopRecord.ModelMaskingName1,TopRecord.MakeMaskingName2,TopRecord.ModelMaskingName2, TopRecord.VersionId1.ToString(), TopRecord.VersionId2.ToString())%>">
                    <img class="lazy" src="" data-original="<%= TopCompareImage %>" title="<%= FormatBikeCompareAnchorText(TopRecord.Bike1,TopRecord.Bike2) %>" alt="<%= FormatBikeCompareAnchorText(TopRecord.Bike1,TopRecord.Bike2) %>" />
                </a>
            </div>
            <div>
                <div class="grid-6 alpha border-solid-right">
                    <div class="content-inner-block-5 text-center">
                        <div class="font18 margin-bottom5">
                            <span class="bwsprite inr-lg-thin"></span> <%= Bikewale.Utility.Format.FormatPrice(Convert.ToString(TopRecord.Price1)) %>
                        </div>
                    </div>
                </div>
                <div class="grid-6 omega">
                    <div class="content-inner-block-5 text-center">
                        <div class="font18 margin-bottom5">
                            <span class="bwsprite inr-lg-thin"></span> <%= Bikewale.Utility.Format.FormatPrice(TopRecord.Price2.ToString()) %>
                        </div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </div>
    </div>
    <div class="grid-6 margin-top20 margin-bottom20">
        <div class="compare-list-home">
            <ul>
                <asp:Repeater runat="server" ID="rptCompareBike">
                    <ItemTemplate>
                        <li>
                            <p class="font16 text-center padding-bottom15">
                                <a href="<%# FormatComparisonUrl(DataBinder.Eval(Container.DataItem,"MakeMaskingName1").ToString(),DataBinder.Eval(Container.DataItem,"ModelMaskingName1").ToString(),DataBinder.Eval(Container.DataItem,"MakeMaskingName2").ToString(),DataBinder.Eval(Container.DataItem,"ModelMaskingName2").ToString(), DataBinder.Eval(Container.DataItem,"VersionId1").ToString(), DataBinder.Eval(Container.DataItem,"VersionId2").ToString()) %>">
                                    <%# FormatBikeCompareAnchorText(DataBinder.Eval(Container.DataItem,"Bike1").ToString(),DataBinder.Eval(Container.DataItem,"Bike2").ToString()) %>
                                </a>
                            </p>
                            <div>
                                <span class="margin-right50">
                                    <span class="bwsprite inr-md-light"></span> <span class="font16 text-xt-light-grey"><%# Bikewale.Utility.Format.FormatPrice(DataBinder.Eval(Container.DataItem,"Price1").ToString()) %></span>
                                </span>
                                <span class="bwsprite inr-md-light"></span> <span class="font16 text-xt-light-grey"><%# Bikewale.Utility.Format.FormatPrice(DataBinder.Eval(Container.DataItem,"Price2").ToString()) %></span>
                            </div>
                        </li>
                    </ItemTemplate>
                </asp:Repeater>
            </ul>
            <% if (ShowCompButton) {%>
            <div class="text-center margin-top20">
                <a href="/comparebikes/" class="btn btn-orange">View more comparisons</a>
            </div>
            <%} %>
        </div>
    </div>
    <div class="clear"></div>
