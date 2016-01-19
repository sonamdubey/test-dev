<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.BikeSeries" Trace="false" %>
<%@ Import Namespace="Bikewale.Common" %>
<%
    title = makeName + " " + seriesName + " Price in India, Review, Mileage,Photos & More - BikeWale";
    keywords = makeName + " bikes, " + makeName + " " + seriesName + " India, " + makeName + " " + seriesName + " bike prices, buy " + makeName + " " + seriesName + " Bikes, " + makeName + " " + seriesName + " reviews, bike reviews, bike news";
    description = makeName + " " + seriesName + " price in India" + price + ". Check out " + makeName + " " + seriesName + " on road price, reviews, mileage, versions, news & photos at Bikewale";
    canonical = "http://www.bikewale.com/" + makeMaskingName + "-bikes/" + seriesMaskingName + "-series/";
    AdPath = "/1017752/Bikewale_Mobile_Series";
    AdId = "1398766939004";
    menu = "2";
    ShowTargeting = "1";
    TargetedSeries = seriesName.Trim();
    AdSeries_300x250 = "1";
%>
<!-- #include file="/includes/headermobile.aspx" -->
    <div class="padding5">
        <div id="br-cr" itemtype="http://data-vocabulary.org/Breadcrumb">
            <a itemprop='url' href='/m/' class="normal"><span itemprop="title">Home</span></a> &rsaquo; 
            <a itemprop="url" href="/m/new/" class="normal"><span itemprop="title">New Bikes</span></a>
            <a itemprop="url" href="/m/<%= makeMaskingName %>-bikes/" class="normal"> <span itemprop="title">&rsaquo; <%= makeName %> Bikes</span></a>
            <span itemprop="title" class="lightgray"> &rsaquo; <%= seriesName %></span></div>

        <h1><%= makeName + " " + seriesName %> Bikes</h1>
        <div id="divModels" class="box1 new-line5" style="padding:0px 10px;">
            <asp:Repeater ID="rptModels" runat="server">
                <ItemTemplate>
                    <a href="/m/<%# DataBinder.Eval(Container.DataItem,"MakeBase.MaskingName") %>-bikes/<%# DataBinder.Eval(Container.DataItem,"MaskingName")%>/" class="link-decoration normal" title="<%# DataBinder.Eval(Container.DataItem,"MakeBase.MakeName").ToString() + DataBinder.Eval(Container.DataItem,"ModelName").ToString() %>">
                        <div class="container">
                            <table style="width:100%;" cellpadding="0" cellspacing="0">
                                <tr>
                                    <%--<td style="width:100px;"><img src='<%# MakeModelVersion.GetModelImage(DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),"/bikewaleimg/models/" + DataBinder.Eval(Container.DataItem,"SmallPicUrl")) %>' title="<%# DataBinder.Eval(Container.DataItem,"MakeBase.MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem,"ModelName").ToString() %>" alt="<%# DataBinder.Eval(Container.DataItem,"MakeBase.MakeName").ToString() + DataBinder.Eval(Container.DataItem,"ModelName").ToString() %>" width="90"/></td>--%>
                                    <td style="width:100px;"><img src='<%# MakeModelVersion.GetModelImage(DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),DataBinder.Eval(Container.DataItem,"OriginalImagePath").ToString(),Bikewale.Utility.ImageSize._110x61) %>' title="<%# DataBinder.Eval(Container.DataItem,"MakeBase.MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem,"ModelName").ToString() %>" alt="<%# DataBinder.Eval(Container.DataItem,"MakeBase.MakeName").ToString() + DataBinder.Eval(Container.DataItem,"ModelName").ToString() %>" width="90"/></td>
                                    <td class="margin-left-10">
                                        <div class="margin-top-5"><b><%# DataBinder.Eval(Container.DataItem,"ModelName")%></b><span class="arr-small margin-left-10">&raquo;</span></div>
                                        <div class="margin-top-5">Starts at Rs. <%# CommonOpn.FormatPrice(DataBinder.Eval(Container.DataItem,"MinPrice").ToString())%></div>
                                        <div class="margin-top-5">
                                            <%# CommonOpn.GetRateImage( Convert.ToDouble(DataBinder.Eval(Container.DataItem,"ReviewRate")))%>                                            
                                            <span><%# DataBinder.Eval(Container.DataItem,"ReviewCount")%> reviews</span>
                                        </div>                                
                                    </td>
                                    </tr>
                            </table>
                        </div>
                    </a>
                </ItemTemplate>
            </asp:Repeater>
        </div>
        <div class="box1 new-line5">
            <!-- Bikewale_Mobile_Model_300x250 -->
            <div id='div-gpt-ad-1398766939004-2' style='width:300px; height:250px;'>
            <script type='text/javascript'>
                googletag.cmd.push(function () { googletag.display('div-gpt-ad-1398766939004-2'); });
            </script>
            </div>
        </div>
    </div>
<!-- #include file="/includes/footermobile.aspx" -->
