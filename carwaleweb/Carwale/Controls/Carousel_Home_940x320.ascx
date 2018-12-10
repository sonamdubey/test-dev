<%@ Control Language="C#" AutoEventWireup="false" Inherits="Carwale.UI.Controls.Carousel_Home_940x320" %>
<%@ Import Namespace="Carwale.Utility" %>

<div id="banner">
    <div id="bannerfull" runat="server">
        <asp:Repeater ID="rptImges" runat="server">
            <HeaderTemplate>
                <ul id="mainSlider" class="jcarousel-skin-tango">
            </HeaderTemplate>
            <ItemTemplate>
                <li>
                     <%--<img src='http://<%# DataBinder.Eval(Container.DataItem, "URL").ToString()%>' alt="" id="imgBanner" <%# IsRoundedCorner()%> /></li>--%>
                    <img src='<%#ImageSizes.CreateImageUrl(DataBinder.Eval(Container.DataItem, "HostURL").ToString(),ImageSizes._891X501,DataBinder.Eval(Container.DataItem, "OriginalImgPath").ToString())%>' alt="" id="imgBanner" <%# IsRoundedCorner()%> />
                </li>
            </ItemTemplate>
            <FooterTemplate>
                </ul>
            </FooterTemplate>
        </asp:Repeater>
    </div>
</div>
<script type="text/javascript">
    $(document).ready(function () {
        $('#mainSlider').jcarousel({
            scroll: 1, auto: 2, animation: 1000, wrap: "circular"//, initCallback: carousel_initCallback                
        });
    });
</script>
