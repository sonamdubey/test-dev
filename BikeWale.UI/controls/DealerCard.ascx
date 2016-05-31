<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.DealerCard" %>
<% if(showWidget) { %>  
 <h2 class="font14 text-bold text-x-black padding-right20 padding-left20"><%=makeName %> dealers in <%=cityName %></h2>
    <div class="grid-12 padding-top15">
            <ul>
                <asp:Repeater ID="rptDealers" runat="server">
                    <ItemTemplate>           
                            <li class="dealer-details-item grid-4 margin-bottom25">
                                <h3 class="font14"><a href="<%# Bikewale.Utility.UrlFormatter.DealerLocatorUrl(makeMaskingName,cityMaskingName, Convert.ToString(CityId)) %>" class="text-default"><%# DataBinder.Eval(Container.DataItem,"Name") %></a></h3>
                                <div class="margin-top10">
                                    <p class="text-light-grey margin-bottom5">
                                        <span class="bwsprite dealership-loc-icon vertical-top margin-right5"></span>
                                        <span class="vertical-top dealership-address"><%# DataBinder.Eval(Container.DataItem,"Address") %></span>
                                    </p>
                                    <p class="margin-bottom5"><span class="text-bold"><span class="bwsprite phone-black-icon"></span><span><%# DataBinder.Eval(Container.DataItem,"MaskingNumber") %></span></span></p>
                                    <p class="margin-bottom15"><a href="mailto:<%# DataBinder.Eval(Container.DataItem,"Email") %>" class="text-light-grey"><span class="bwsprite mail-grey-icon"></span><span><%# DataBinder.Eval(Container.DataItem,"Email") %></span></a></p>
                                    <a href="" class="btn btn-grey btn-md font14">Get offers from dealer</a>
                                </div>
                                <div class="clear"></div>
                            </li>                                             
                    </ItemTemplate>
                 </asp:Repeater>
             </ul>
      </div>
      <div class="clear"></div>
      <a href="<%# Bikewale.Utility.UrlFormatter.DealerLocatorUrl(makeMaskingName,cityMaskingName, Convert.ToString(CityId)) %>" class="margin-left20">View all dealers<span class="bwsprite blue-right-arrow-icon"></span></a>
<% } %>