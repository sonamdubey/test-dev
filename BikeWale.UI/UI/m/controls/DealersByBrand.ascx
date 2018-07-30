<%@ Control Language="C#" AutoEventWireup="false"  Inherits="Bikewale.Mobile.Controls.DealersByBrand" %>
<%if (AllDealers!=null){ %>
<div class="container bg-white box-shadow padding-top15 padding-bottom20">
    <h2 class="padding-right20 padding-bottom15 padding-left20"><%=WidgetTitle%></h2>

    <div class="swiper-container padding-top5 padding-bottom5 brand-type-carousel">
        <div class="swiper-wrapper">
            <%foreach(var DealerDetails in AllDealers){ %>
                <div class="swiper-slide">
                    <div class="swiper-card">
                        <a href="/m<%= Bikewale.Utility.UrlFormatter.GetDealerShowroomUrl(DealerDetails.MaskingName)%>" title="<%=String.Format("{0} showrooms in India",DealerDetails.MakeName)%>">
                            <div class="brand-logo-image">
                                <span class="brand-type">
                                    <span class="brandlogosprite brand-<%=DealerDetails.MakeId%>"></span>
                                </span>
                            </div>
                            <div class="swiper-details-block">
                                <p class="text-bold text-black font12 margin-bottom5 border-solid-top padding-top10"><%=DealerDetails.MakeName%></p>
                                <h3 class="text-unbold text-light-grey font11"><%=String.Format("{0} {1}",DealerDetails.DealerCount,DealerDetails.MakeName)%> showroom<%=(DealerDetails.DealerCount)>1?"s":"" %></h3>
                            </div>
                        </a>
                    </div>
                </div>
            <%} %>
        </div>
    </div>
</div>
<%} %>
