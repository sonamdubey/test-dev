<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.MyBikeWale.BuyerDetails" Trace="false" Debug="false" %>
<%
    AdId = "1395996606542";
    AdPath = "/1017752/BikeWale_MyBikeWale_";
   
    //Modified By :Sajal Gupta on 03 August 2016
    isAd300x250BtfShown = false;
    
%>
<!-- #include file="/includes/headmybikewale.aspx" -->

<div class="container_12">
        <div class="grid_12">
            <ul class="breadcrumb">
                <li>You are here: </li>
                <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                    <a href="/" itemprop="url">
                        <span itemprop="title">Home</span>
                    </a>
                </li>
                <li class="fwd-arrow">&rsaquo;</li>
                <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                    <a href="/mybikewale/" itemprop="url">
                        <span itemprop="title">My BikeWale</span>
                    </a>
                </li>
                <li class="fwd-arrow">&rsaquo;</li>                
                <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                    <a href="/mybikewale/myinquiries/" itemprop="url">
                        <span itemprop="title">My Inquiries</span>
                    </a>
                </li>
                <li class="fwd-arrow">&rsaquo;</li>
                <li class="current"><strong>Inquiry details for <%=_bikeName %></strong></li>
            </ul><div class="clear"></div>
        </div>
        <div class="grid_8 margin-top10">
            <ul style="list-style:none;padding:0;" class="margin-bottom15">
                <li class="left-float"><h3>Inquiry details for <%=_bikeName %></h3></li>
                <li class="right-float"><h3>Profile No : S<%=inquiryId %></h3></li>
                <li class="clear"></li>
            </ul>            
            <asp:Repeater id="rptBuyersList" runat="server">
                <HeaderTemplate>
                    <table class="tbl-std">
                        <tr>
                            <th>Buyer's Name</th>
                            <th>Arrived On</th>
                            <th>Mobile</th>
                            <th>Email</th>
                        </tr>                
                </HeaderTemplate>
                <ItemTemplate>
                        <tr>
                            <%# GetBikeDetails(Convert.ToString(DataBinder.Eval(Container.DataItem,"Bike"))) %>
                            <td><%# DataBinder.Eval(Container.DataItem,"Name") %></td>
                            <td><%# Convert.ToDateTime(DataBinder.Eval(Container.DataItem,"ArrivedOn")).ToString("dd MMM, yyyy hh:mm tt") %></td>
                            <td><%# DataBinder.Eval(Container.DataItem,"Mobile") %></td>
                            <td><%# DataBinder.Eval(Container.DataItem,"Email") %></td>
                        </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>                   
        </div>
        <div class="grid_4">
            <div class="margin-top15">
                <!-- BikeWale_NewBike/BikeWale_NewBike_HP_300x250 -->
                <!-- #include file="/ads/Ad300x250.aspx" -->
            </div>
        </div>
</div>
<!-- #include file="/includes/footerinner.aspx" -->