<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Controls.CompareBikesMin" %>

<div class="container">
    <div class="grid-12 bg-white padding-bottom20">
        <h2 class="font18 text-center margin-top20 margin-bottom20">Compare bikes</h2>
        <a href="" title="Compare Bike 1 vs Bike 2" id="main-compare-bikes-target">
            <div class="grid-6">
                <div class="comparison-image">
                    <img class="lazy" data-original="http://imgd1.aeplcdn.com//210x118//bw/models/honda-cb-shine-electric-start/drum/alloy-112.jpg" src="http://imgd3.aeplcdn.com/0x0/bw/static/sprites/m/circleloader.gif" />
                </div>
                <h3 class="font14 text-black margin-bottom5">Hero Honda Hero Honda Hero Honda</h3>
                <div class="text-default text-bold">
                    <span class="bwmsprite inr-xsm-icon"></span>&nbsp;<span class="font16">92,000</span><span class="font12">&nbsp;onwards</span>
                </div>
                <p class="text-light-grey margin-bottom5 font12">Ex-showroom, Mumbai</p>
            </div>
            <div class="grid-6">
                <div class="comparison-image">
                    <img class="lazy" data-original="http://imgd1.aeplcdn.com//210x118//bw/models/honda-cb-shine-electric-start/drum/alloy-112.jpg" src="http://imgd3.aeplcdn.com/0x0/bw/static/sprites/m/circleloader.gif" />
                </div>
                <h3 class="font14 text-black margin-bottom5">Hero Honda Hero Honda Hero Honda</h3>
                <div class="text-default text-bold">
                    <span class="bwmsprite inr-xsm-icon"></span>&nbsp;<span class="font16">92,000</span><span class="font12">&nbsp;onwards</span>
                </div>
                <p class="text-light-grey margin-bottom5 font12">Ex-showroom, Mumbai</p>
            </div>
            <div class="clear"></div>
            <div class="margin-top10 text-center">
                <span class="btn btn-orange">Compare now</span>
            </div>
        </a>
        <div class="text-center margin-bottom10">
            <a href="" class="font14">View more comparisons</a>
        </div>
    </div>
    <div class="clear">
</div>

<div class="container">
    
    <div class="grid-12">
        <h2 class="font18 text-center margin-top20 margin-bottom20">Compare bikes</h2>
        <div class="compare-bikes-container">
        <div class="bike-preview margin-bottom10">
            <a href="<%= FormatComparisonUrl(TopRecord.MakeMaskingName1,TopRecord.ModelMaskingName1,TopRecord.MakeMaskingName2,TopRecord.ModelMaskingName2, TopRecord.VersionId1, TopRecord.VersionId2)%>">
                <img class="lazy" src="http://imgd3.aeplcdn.com/0x0/bw/static/sprites/d/loader.gif" data-original="<%= TopCompareImage %>" title="<%= FormatBikeCompareAnchorText(TopRecord.Bike1,TopRecord.Bike2) %>" alt="<%= FormatBikeCompareAnchorText(TopRecord.Bike1,TopRecord.Bike2) %>">
            </a>
        </div>
        <h3 class="font16 text-center padding-top10 padding-bottom15">
            <a href="<%= FormatComparisonUrl(TopRecord.MakeMaskingName1,TopRecord.ModelMaskingName1,TopRecord.MakeMaskingName2,TopRecord.ModelMaskingName2, TopRecord.VersionId1, TopRecord.VersionId2)%>" class="text-grey">
                <%= FormatBikeCompareAnchorText(TopRecord.Bike1,TopRecord.Bike2) %>
            </a>
        </h3>
       
            <div class="font16 text-center padding-bottom15">
            <a href="/m/comparebikes/">View more comparisons</a>
        </div>
            
        </div>
            </div>
       

        
    </div>
    <div class="clear"></div>
</div>
