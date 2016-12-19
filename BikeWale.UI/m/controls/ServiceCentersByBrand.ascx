<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Controls.ServiceCentersByBrand" %>
<%if (AllServiceCenters!=null){ %>
<style>
    .bwm-brands-swiper .swiper-slide, .bwm-brands-swiper .swiper-card {
        width: 200px;
        min-height: 127px;
    }
    .brand-logo-image{
    height: 60px;
    width : 100%;
    line-height: 0;
    display: table;
    text-align: center;
    }
    .brand-type {
    width: 100%;
    display: table-cell;
    vertical-align: middle;
    margin: 0 auto;
}
 
.brandlogosprite { background:url('https://imgd2.aeplcdn.com/0x0/bw/static/sprites/m/brand-type-sprite.png?v=04Oct2016') no-repeat; display: inline-block; }
.brand-2, .brand-1, .brand-40, .brand-3, .brand-4, .brand-11, .brand-20, .brand-5, .brand-39, .brand-6, .brand-7, .brand-8, .brand-34, .brand-17, .brand-9, .brand-19, .brand-10, .brand-12, .brand-22, .brand-15, .brand-16, .brand-13, .brand-14, .brand-41, .brand-42, .brand-81, .brand-71 { height:30px; }
.brand-2 { width: 52px; background-position: 0 0; }.brand-7 { width: 34px; background-position: -58px 0; }
.brand-1 { width: 53px; background-position: -97px 0; }.brand-8 { width: 60px; background-position: -156px 0; }
.brand-12 { width: 40px; background-position: -222px 0; }.brand-40 { width: 75px; background-position: -268px 0; }
.brand-34 { width: 73px; background-position: -349px 0; }.brand-22 { width: 73px; background-position: -428px 0; }
.brand-3 { width: 26px; background-position: -507px 0; }.brand-17 { width: 52px; background-position: -539px 0; }
.brand-15 { width: 71px; background-position: -597px 0; }.brand-4 { width: 26px; background-position: -674px 0; }
.brand-9 { width: 59px; background-position: -706px 0; }.brand-16 { width: 70px; background-position: -771px 0; }
.brand-5 { width: 35px; background-position: -847px 0; }.brand-19 { width: 73px; background-position: -889px 0; }
.brand-13 { width: 73px; background-position: -968px 0; }.brand-6 { width: 38px; background-position: -1047px 0; }
.brand-10 { width: 61px; background-position: -1091px 0; }.brand-14 { width: 76px; background-position: -1159px 0; }
.brand-39 { width: 53px; background-position: -1242px 0; }.brand-20 { width: 49px; background-position: -1300px 0; }
.brand-11 { width: 74px; background-position: -1354px 0; }.brand-41 { width: 40px; background-position: -1432px 0; }
.brand-42 { width: 38px; background-position: -1481px 0; }.brand-81 { width:38px; background-position:-1529px 0; }
.brand-71 {
    width: 23px;
    background-position: -1577px 0;
} 

</style>
<div class="container bg-white box-shadow padding-top15 padding-bottom20 font14 active">
    <h2 class="font18 padding-bottom20 padding-left20"><%=WidgetTitle%></h2>

    <div class="swiper-container padding-top5 padding-bottom5 sw-0 swiper-container-horizontal bwm-brands-swiper">
        <div class="swiper-wrapper">
            <%foreach(var centers in AllServiceCenters){ %>
                <div class="swiper-slide swiper-slide-visible swiper-slide-active">
                    <div class="swiper-card">
                        <a href="/m<%= Bikewale.Utility.UrlFormatter.GetServiceCenterUrl(centers.MakeMaskingName)%>" title="<%=String.Format("{0} Service Centers in India",centers.MakeName)%>">
                            <div class="brand-logo-image">
                                <span class="brand-type">
                                    <span class="brandlogosprite brand-<%=centers.MakeId%>"></span>
                                </span>
                            </div>
                            <div class="swiper-details-block">
                                <h3 class="target-link font12 text-truncate margin-bottom5 border-solid-top padding-top10"><%=centers.MakeName%></h3>
                                <p class="text-truncate text-light-grey font11"><%=centers.ServiceCenterCount%> Service Center<%=(centers.ServiceCenterCount)>1?"s":"" %></p>
                            </div>
                        </a>
                    </div>
                </div>
            <%} %>
        </div>
    </div>
</div>
<%} %>
