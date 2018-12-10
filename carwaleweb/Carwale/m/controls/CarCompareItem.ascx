<%@ Control Language="C#" AutoEventWireup="false" Inherits="MobileWeb.Controls.CarCompareItem" %>
<%--<div class="box new-line5">
        <div style="margin:auto;width:316px;"><img style="height:101px;width:316px;"  src="https://img2.carwale.com/research/carcomparison/hyundai_grandi10_vs_marutisuzuki_swift.jpg"/></div>
        <div style="text-align:center;"> Hundai grand i10 Vs Maruti Suzukki Swift </div>
</div>--%>
<style>
    @media screen and (min-width:330px) {
        .compareImageContainer {width:316px;margin:auto;text-align:center;}
        .compareImageContainer img { width:272px;height:auto;}
    
    }

    @media screen and (max-width:329px) {
        .compareImageContainer {}
            .compareImageContainer img {max-width:100%;height:auto;}
    
    }
</style>
<a href="/m/comparecars/<%=ComapreUrl %>" class="normal" data-role="click-tracking" data-event="CWInteractive" data-cat="Comparison-widget" data-action="CompareCarsList_m_click" data-label="<%=CompareText %>" onclick="window.location.href = this.href +'?c1=<%=VersionId1%>&c2=<%=VersionId2 %>';return false;">
    <div class="content-box-shadow content-inner-block-10 rounded-corner2 margin-bottom10 text-black">
            <span class="font11 text-medium-grey position-abt pos-right10" data-role="impression" data-event="CWNonInteractive" data-cat="Comparison-widget" data-action="CompareCarsList_m" data-label="<%=CompareText %>"><%=SponsoredText %></span>
            <div class="compareImageContainer" ><img  src="<%=ImageUrl %>" alt="<%=CompareText %>" /></div>
            
            <div style="text-align:center;"> <%=CompareText %> </div>
    </div>
</a>