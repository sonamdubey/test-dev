<%@ Control Language="C#" AutoEventWireup="false" Inherits="Carwale.UI.Controls.UpcomingCars" %>
<%@ Import Namespace="Carwale.UI.Common" %>
<style type="text/css">
    .uc-ver {
        width: 100%;
    }

        .uc-ver .lt {
            width: 100%;
            height: 69px;
            float: left;
            padding: 8px 0px;
            margin-bottom : 10px;
        }

            .uc-ver .lt img {
                float: left;
                margin-right: 5px;
                background-color: #ffffff;
            }

    .uc-hor {
        width: 100%;
    }

        .uc-hor .lt {
            width: 158px;
            float: left;
            padding: 8px 0px;
        }

            .uc-hor .lt img {
                float: left;
                margin-right: 46px;
                background-color: #ffffff;
            }
            .icon-sheet {
  background: url(https://imgd.aeplcdn.com/0x0/cw-common/icon-sheet2.gif) no-repeat;
}
            .more-link {
  background-position: -8px -23px;
  display: block;
  float: right;
  width: 10px;
  height: 10px;
  margin: 3px 0 0 3px;
}
</style>

<div id="divUpcomingCar" runat="server" class="uc-ver margin-top10">
    <asp:Repeater ID="rptData" runat="server">
        <ItemTemplate>
            <div class="lt margin-bottom10">
                <a href="/<%# UrlRewrite.FormatSpecial((( Carwale.Entity.CarData.UpcomingCarModel )Container.DataItem).MakeName)%>-cars/<%#(( Carwale.Entity.CarData.UpcomingCarModel )Container.DataItem).MaskingName %>/">
                    <img class="lazy" src="<%# Carwale.Utility.ImageSizes.CreateImageUrl(((Carwale.Entity.CarData.UpcomingCarModel)Container.DataItem).Image.HostUrl, Carwale.Utility.ImageSizes._227X128, ((Carwale.Entity.CarData.UpcomingCarModel)Container.DataItem).Image.ImagePath) == "" ? "https://imgd.aeplcdn.com/0x0/statics/grey.gif" : Carwale.Utility.ImageSizes.CreateImageUrl(((Carwale.Entity.CarData.UpcomingCarModel)Container.DataItem).Image.HostUrl, Carwale.Utility.ImageSizes._227X128, ((Carwale.Entity.CarData.UpcomingCarModel)Container.DataItem).Image.ImagePath)  %>" data-original="<%# Carwale.Utility.ImageSizes.CreateImageUrl(((Carwale.Entity.CarData.UpcomingCarModel)Container.DataItem).Image.HostUrl, Carwale.Utility.ImageSizes._227X128, ((Carwale.Entity.CarData.UpcomingCarModel)Container.DataItem).Image.ImagePath)%>" width="100px" height="57px" title="<%#(( Carwale.Entity.CarData.UpcomingCarModel )Container.DataItem).MakeName + " " + (( Carwale.Entity.CarData.UpcomingCarModel )Container.DataItem).ModelName %>" alt="<%# (( Carwale.Entity.CarData.UpcomingCarModel )Container.DataItem).MakeName + " " + (( Carwale.Entity.CarData.UpcomingCarModel )Container.DataItem).ModelName %>" />
                </a>
                <a href="/<%# UrlRewrite.FormatSpecial((( Carwale.Entity.CarData.UpcomingCarModel )Container.DataItem).MakeName)%>-cars/<%#(( Carwale.Entity.CarData.UpcomingCarModel )Container.DataItem).MaskingName%>/"><%# (( Carwale.Entity.CarData.UpcomingCarModel )Container.DataItem).MakeName + " " + (( Carwale.Entity.CarData.UpcomingCarModel )Container.DataItem).ModelName %></a>
                <p class="font12"><%#(( Carwale.Entity.CarData.UpcomingCarModel )Container.DataItem).ExpectedLaunch%></p>
                <p class="font12"> ₹ <%#Carwale.UI.Common.FormatPrice.GetFormattedPriceV2(((( Carwale.Entity.CarData.UpcomingCarModel )Container.DataItem).Price.MinPrice).ToString(),(( Carwale.Entity.CarData.UpcomingCarModel )Container.DataItem).Price.MaxPrice.ToString())%></p>
            </div>
        </ItemTemplate>
    </asp:Repeater>
    <asp:Label ID="lblNotFound" runat="server" Visible="false"></asp:Label>
</div>
<a class="redirect-rt" style="float:right" href="/upcoming-cars/">All Upcoming Cars</a><span class="icon-sheet more-link"></span>
<div class="clear"></div>

<script type="text/javascript">
    $(document).ready(function(){
        $("img.lazy-uc").lazyload();
    });
</script>
