﻿<%@ Control Language="C#" AutoEventWireup="false"  Inherits="Bikewale.Controls.DealersByBrand" %>
<%if(AllServiceCenters!=null){ %>
<style>
    .brand-logo-image{
    height: 80px;
    width : 260px;
    line-height: 0;
    display: table;
    text-align: center;
    }
    .brand-type {
    width: 180px;
    height: 50px;
    display: table-cell;
    vertical-align: middle;
    margin: 0 auto;
}
    .brandlogosprite { background:url('https://imgd2.aeplcdn.com/0x0/bw/static/sprites/d/brand-type-sprite.png?v=04Oct2016') no-repeat; display: inline-block; }
.brand-2, .brand-1, .brand-40, .brand-3, .brand-4, .brand-24, .brand-5, .brand-6, .brand-39, .brand-37, .brand-7, .brand-23, .brand-8, .brand-34, .brand-17, .brand-18, .brand-9, .brand-19, .brand-10, .brand-20, .brand-41, .brand-38, .brand-11, .brand-12, .brand-22, .brand-15, .brand-42, .brand-16, .brand-16, .brand-13, .brand-14, .brand-81, .brand-71 { height:50px; }
.brand-2 { width: 87px; background-position: 0 0; }.brand-7 { width: 56px; background-position: -96px 0; }.brand-1 { width: 88px; background-position: -162px 0; }.brand-8 { width: 100px; background-position: -260px 0; }.brand-12 { width: 67px; background-position: -370px 0; }.brand-40 { width: 125px; background-position: -447px 0; }.brand-34 { width: 122px; background-position: -582px 0; }.brand-22 { width: 121px; background-position: -714px 0; }.brand-3 { width: 44px; background-position: -845px 0; }.brand-17 { width: 86px; background-position: -899px 0; }.brand-15 { width: 118px; background-position: -995px 0; }.brand-4 { width: 43px; background-position: -1123px 0; }.brand-9 { width: 99px; background-position: -1176px 0; }.brand-16 { width: 117px; background-position: -1285px 0; }.brand-5 { width: 59px; background-position: -1412px 0; }.brand-19 { width: 122px; background-position: -1481px 0; }.brand-13 { width: 122px; background-position: -1613px 0; }.brand-6 { width: 63px; background-position: -1745px 0; }.brand-10 { width: 102px; background-position: -1818px 0; }.brand-14 { width: 127px; background-position: -1930px 0; }.brand-39 { width: 89px; background-position: -2067px 0; }.brand-20 { width: 82px; background-position: -2166px 0; }.brand-11 { width: 121px; background-position: -2258px 0; }.brand-41 { width:63px; background-position: -2389px 0; }.brand-42 { width:64px; background-position:-2461px 0; }.brand-81 { width:71px; background-position:-2535px 0; }

.brand-71 {
    width: 39px;
    background-position: -2616px 0;
}
#carouselServiceCenter span.jcarousel-control-right,#carouselServiceCenter span.jcarousel-control-left {top: 31%;}

</style>

<div class="container section-container ">
<div class="grid-12 margin-bottom20">
    <div class="content-box-shadow padding-top20 padding-bottom20">
    <h2 class="font18 padding-bottom20 padding-left20"><%=WidgetTitle%></h2>

      <div class="jcarousel-wrapper inner-content-carousel" id="carouselServiceCenter">

     
                <div class="jcarousel" data-jcarousel="true">

                    <ul>
                        <%foreach (var centers in AllServiceCenters)
                          { %>
                                <li>
                                    <a href="<%= Bikewale.Utility.UrlFormatter.GetServiceCenterUrl(centers.MakeMaskingName)%>" title="<%=String.Format("{0} Service Centers in India",centers.MakeName)%>" class="jcarousel-card">
                                      
                                        <div class="brand-logo-image">
                                            <span class="brand-type">
                                                <span class="brandlogosprite brand-<%=centers.MakeId%>"></span>
                                            </span>
                                        </div>
                                        <div class="card-desc-block">
                                            <h3 class="bikeTitle border-solid-top padding-top15"><%= centers.MakeName%></h3>
                                          
                                            <p class="font14 text-light-grey margin-bottom5"><%=centers.DealerCount %> Service Center<%=(centers.DealerCount)>1?"s":"" %></p>
                                           
                                        </div>
                                    </a>
                                        </li>
                           <%} %>
                    </ul>

                </div>

          <span class="jcarousel-control-left"><a href="#" class="bwsprite jcarousel-control-prev inactive" rel="nofollow"></a></span>
            <span class="jcarousel-control-right"><a href="#" class="bwsprite jcarousel-control-next" rel="nofollow"></a></span>
            </div>
    </div>
    </div>
</div>
<%} %>