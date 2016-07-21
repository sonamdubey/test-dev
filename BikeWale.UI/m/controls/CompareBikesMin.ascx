<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Controls.CompareBikesMin" %>

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
