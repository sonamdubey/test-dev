<%@ Control Language="C#" AutoEventWireup="false"  Inherits="Bikewale.Controls.DealersByBrand" %>
<%if(AllDealers!=null){ %>
<div class="container section-container">
    <div class="grid-12 margin-bottom20">
        <div class="content-box-shadow padding-top20 padding-bottom20">
            <h2 class="font18 padding-bottom20 padding-left20"><%=WidgetTitle%></h2>
            <div class="jcarousel-wrapper inner-content-carousel brand-type-carousel">
                <div class="jcarousel">
                    <ul>
                        <%foreach (var DealerDetails in AllDealers)
                           { %>
                            <li>
                                <a href="<%= Bikewale.Utility.UrlFormatter.GetDealerShowroomUrl(DealerDetails.MaskingName)%>" title="<%=String.Format("{0} showrooms in India",DealerDetails.MakeName)%>" class="jcarousel-card">
                                    <div class="brand-logo-image">
                                        <span class="brand-type">
                                            <span class="brandlogosprite brand-<%=DealerDetails.MakeId%>"></span>
                                        </span>
                                    </div>
                                    <div class="card-desc-block">
                                        <p class="card-heading font14 text-bold text-black padding-top10 border-solid-top"><%= DealerDetails.MakeName%></p>
                                        <h3 class="text-unbold text-light-grey"><%=String.Format("{0} {1}",DealerDetails.DealerCount ,DealerDetails.MakeName) %> showroom<%=(DealerDetails.DealerCount)>1?"s":"" %></h3>
                                    </div>
                                </a>
                            </li>
                        <%} %>
                    </ul>
                </div>
                <span class="jcarousel-control-left"><a href="#" class="bwsprite jcarousel-control-prev" rel="nofollow"></a></span>
                <span class="jcarousel-control-right"><a href="#" class="bwsprite jcarousel-control-next" rel="nofollow"></a></span>
            </div>
        </div>
    </div>
</div>
<%} %>