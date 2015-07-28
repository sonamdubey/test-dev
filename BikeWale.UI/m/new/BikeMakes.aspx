<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.BikeMakes" Trace="false" %>
<%@ Import Namespace="Bikewale.Common" %>
<%
    title = makeName + " Price in India, Review, Mileage & Photos - Bikewale";
    description = makeName + " Price in India - " + price + ". Check out " + makeName + " on road price, reviews, mileage, variants, news & photos at Bikewale.";
    canonical = "http://www.bikewale.com/" + makeMaskingName + "-bikes/";
    AdPath = "/1017752/Bikewale_Mobile_Make";
    AdId = "1398766517811";
    menu = "2";
    ShowTargeting = "1";
    TargetedMakes = makeName.Trim();
%>
<!-- #include file="/includes/headermobile.aspx" -->
<form id="form1" runat="server">
    <div class="padding5">
        <div id="br-cr" itemtype="http://data-vocabulary.org/Breadcrumb">
            <a itemprop='url' href='/m/' class="normal"><span itemprop="title">Home</span></a> &rsaquo; 
            <a itemprop='url'  href="/m/new/" class="normal"><span itemprop="title">New Bikes</span></a>
            <span itemprop="title" class="lightgray"> &rsaquo; <%= makeName %> Bikes</span>
        </div>
        <h1><%= makeName %> Bikes</h1>
        <%if(count > 0) { %>
        <div id="divModels" class="box1 new-line5" style="padding:0px 10px;">
            <asp:Repeater ID="rptSeries" runat="server">
                <ItemTemplate>
                    <a href="/m/<%# DataBinder.Eval(Container.DataItem,"MakeBase.MaskingName") %>-bikes/<%# Convert.ToInt32(DataBinder.Eval(Container.DataItem,"ModelCount")) == 1 ? DataBinder.Eval(Container.DataItem,"MaskingName") : DataBinder.Eval(Container.DataItem,"ModelSeries.MaskingName") + "-series" %>/" class="link-decoration normal" title="<%# DataBinder.Eval(Container.DataItem,"MakeBase.MakeName") + " " + DataBinder.Eval(Container.DataItem,"ModelSeries.SeriesName") %>">
                        <div class="container">
                            <table style="width:100%;" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="width:100px;"><img src="<%# MakeModelVersion.GetModelImage(DataBinder.Eval(Container.DataItem,"SeriesHostUrl").ToString(), DataBinder.Eval(Container.DataItem,"SeriesSmallPicUrl").ToString()) %>" title='<%# DataBinder.Eval(Container.DataItem,"MakeBase.MakeName") + " " + DataBinder.Eval(Container.DataItem,"ModelSeries.SeriesName") %>' alt="<%# DataBinder.Eval(Container.DataItem,"MakeBase.MakeName") + " " + DataBinder.Eval(Container.DataItem,"ModelSeries.SeriesName") %>" width="90" /></td>
                                    <td class="margin-left-10">
                                        <div class="margin-top-5"><b><%# DataBinder.Eval(Container.DataItem,"ModelSeries.SeriesName") %></b><span class="arr-small margin-left-10">&raquo;</span></div>
                                        <div class="margin-top-5">Starts at Rs. <%# CommonOpn.FormatPrice(DataBinder.Eval(Container.DataItem,"MinPrice").ToString()) %></div>
                                    </td>
                                    </tr>
                            </table>
                        </div>
                    </a>
                </ItemTemplate>
            </asp:Repeater>
        </div>
        <% } %>
    </div>
</form>
<!-- #include file="/includes/footermobile.aspx" -->
