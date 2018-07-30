<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Controls.ServiceCentersByBrand" %>
<%if (AllServiceCenters!=null){ %>
<div class="container bg-white box-shadow padding-top15">
    <h2 class="padding-right20 padding-bottom15 padding-left20">Find service centers for other brands</h2>

    <div class="swiper-container card-container brand-type-carousel">
        <div class="swiper-wrapper">
            <%foreach(var centers in AllServiceCenters){ %>
                <div class="swiper-slide">
                    <div class="swiper-card">
                        <a href="/m<%= Bikewale.Utility.UrlFormatter.GetServiceCenterUrl(centers.MaskingName)%>" title="<%=String.Format("{0} Service Centers in India",centers.MakeName)%>">
                            <div class="brand-logo-image">
                                <span class="brand-type">
                                    <span class="brandlogosprite brand-<%=centers.MakeId%>"></span>
                                </span>
                            </div>
                            <div class="swiper-details-block">
                                <p class="text-bold text-black font12 margin-bottom5 border-solid-top padding-top10"><%=centers.MakeName%></p>
                                <h3 class="text-unbold text-light-grey font11"><%= String.Format("{0} {1}", centers.ServiceCenterCount,centers.MakeName) %> service center<%=(centers.ServiceCenterCount)>1?"s":"" %></h3>
                            </div>
                        </a>
                    </div>
                </div>
            <%} %>
        </div>
    </div>
</div>
<%} %>
