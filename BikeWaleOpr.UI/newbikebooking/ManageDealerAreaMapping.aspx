<%@ Page Language="C#" AutoEventWireup="false" Inherits="BikeWaleOpr.NewBikeBooking.ManageDealerAreaMapping" Trace="false" Async="true" EnableEventValidation="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Dealer Area Mapping</title>
    <script type="text/javascript" src="/src/jquery-1.6.min.js"></script>
    <script type="text/javascript" src="/src/AjaxFunctions.js"></script>
    <script type="text/javascript" src="/src/graybox.js"></script>
    <link href="/css/Common.css" rel="stylesheet" />
    <style>
        .inner-content{ border:1px solid #808080; padding:5px; margin-bottom:5px; float:left;margin-left:5px; width:300px;}
        .bw-popup, .bw-contact-popup { background:#fff; width:454px; position:fixed; left:50%; top:50%; z-index:999; margin-left:-220px; margin-top:-150px; }
        .popup-inner-container { padding:10px 10px 20px 10px;}
        .popup-inner-container h2{ padding-bottom:10px; border-bottom:2px solid #c62000;}
        .gery { background-color : #FFE4C4;}
    </style>
</head>
<body>
    <form runat="server">
        <h1>Manage Dealer Area Mapping</h1>
    <div style="padding:10px;">
        
        <div>
            <div> Select City - 
                <asp:DropDownList id="ddlCity" runat="server" AutoPostBack="true">
                </asp:DropDownList>
            </div>
        </div>
        <div>
              <h3 id="mapedArea" class="hide">Mapped Areas  <span><input type="button" id="chkMap" onclick="checkAll(this);" value="Check All" /></span>&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button id="unmapDealer" Text="Unmap Dealers" runat="server"  /></h3> 
            <asp:Repeater ID="rptMappedArea" runat="server">
                <ItemTemplate>
                    <div class="inner-content">
                        <input type="checkbox" id="chkMappedCity" class="mapdealers" areaId="<%#DataBinder.Eval(Container.DataItem,"AreaId") %>"/>
                        <%# DataBinder.Eval(Container.DataItem,"AreaName")%> (<%# DataBinder.Eval(Container.DataItem,"PinCode") %>)
                    </div>
                </ItemTemplate>
            </asp:Repeater>
            <div style="clear:both;"></div>
        </div>

        <div>
            <h3 id="unmapedArea" class="hide">Unmapped Areas  <span><input type="button" id="chkUnMap" onclick="checkAll(this);" value="Check All"/></span>&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button id="MapDealer" Text="Map Dealers" runat="server" /></h3> 
                    <div style="display:inline-block;padding:5px;">
            <span  style="width:20px;height:20px; background-color:#FFE4C4;display:block; float:left;">&nbsp;</span>&nbsp;&nbsp;&nbsp;<span style="float:left;margin-left:5px;"><b> Areas which are already mapped to current dealer</b></span>
        </div>
        <div style="clear:both;"></div>
            <asp:Repeater ID="rptUnmappedCity" runat="server">
                <ItemTemplate>
                    <%# showArea(DataBinder.Eval(Container.DataItem,"AreaName").ToString(),DataBinder.Eval(Container.DataItem,"AreaId").ToString(),DataBinder.Eval(Container.DataItem,"PinCode").ToString(),Convert.ToInt32(DataBinder.Eval(Container.DataItem,"DealerCount")),Convert.ToInt32(DataBinder.Eval(Container.DataItem,"DealerRank"))) %>
                    <div class="hide dlr_<%# DataBinder.Eval(Container.DataItem,"AreaId") %> dealer_row">
                        <div class="dealerArea">
                            <div style="float:left;"><%# DataBinder.Eval(Container.DataItem,"DealerOrganization") %> (<%#DataBinder.Eval(Container.DataItem,"MakeName") %>)</div>
                            <div style="float:right"><a areaId="<%#DataBinder.Eval(Container.DataItem,"AreaId") %>" dealerId="<%#DataBinder.Eval(Container.DataItem,"DealerId") %>" class="remove_<%#DataBinder.Eval(Container.DataItem,"DealerId") %>">Remove</a></div>
                            <div style="clear:both;"></div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
            </div>
            <div style="clear:both;"></div>
            <input type="hidden" id="hdnMapArea" runat="server"/>
            <input type="hidden" id="hdnUnmapArea" runat="server" />
        <div class="bw-popup contact-details hide">
            <div class="popup-inner-container">
            </div>
        </div>
           <script type="text/javascript">
        $(document).ready(function () {
            $("a[id^='edit_']").click(function () {
                var html = $(".popup-inner-container").html("");
                var areaId = $(this).attr('areaId');
                var dealerClass = $(".dlr_" + areaId).removeClass("hide");
                $(".popup-inner-container").html(dealerClass);
                html = $(".popup-inner-container").html();

                var comment = "";
                var caption = "Mapped Dealer List";
                var url = "";
                var applyIframe = true;
                var GB_Html = html;
                GB_show(caption, url, 220, 340, applyIframe, GB_Html);


                $("#gb-window").find('[class^="remove"]').click(function () {
                    var areaId = $(this).attr('areaid');
                    var dealerId = $(this).attr('dealerid');
                    var bwOprHostUrl = '<%= ConfigurationManager.AppSettings["BwOprHostUrlForJs"] %>';

                    $.ajax({
                        type: "POST",
                        url: bwOprHostUrl + "/api/dealerpricequote/UnmapDealerWithArea/?dealerid=" + dealerId + "&areaidlist=" + areaId,
                        success: function (response) {
                            $(this).parent().parent().addClass("hide");
                            $("#edit_" + areaId).addClass("hide");
                            alert("Dealer Mapped with selected Area removed Successfully.")
                        }
                    });                    
                });
            });

            var countArea = 0;
            $(".mapdealers").each(function () {
                countArea += 1;
            });

            if (countArea > 0) {
                $("#mapedArea").removeClass("hide");
            }

            var countArea1 = 0;
            $(".unmapdealers").each(function () {
                countArea1 += 1;
            });

            if (countArea1 > 0) {
                $("#unmapedArea").removeClass("hide");
            }

            $(".mapdealers").each(function () {
                var input1 = $(this).attr("areaid");
                cityid = $(this).attr("areaid");
                $(".unmapdealers").each(function () {
                    if (cityid == $(this).attr("areaid")) {
                        $(input1).parent().addClass("gery");
                        $(this).parent().addClass("gery");
                    }
                });
            });
        });

        function checkAll(chk) {
            var id = $(chk).attr("id");
            var value = $(chk).attr("value");

            if (value == "Check All") {
                if (id == "chkMap") {
                    $('[class^="mapdealers"]').each(function () {
                        //alert($(this).is(":checked"));
                        $(this).prop('checked', true);
                    });
                }
                else if (id == "chkUnMap") {
                    $('[class^="unmapdealers"]').each(function () {
                        $(this).prop('checked', true);
                    });
                }
                
                $(chk).attr("value", "Uncheck All");
            }
            else {
                if (id == "chkMap") {
                    $('[class^="mapdealers"]').each(function () {
                        //alert($(this).is(":checked"));
                        $(this).prop('checked', false);
                    });
                }
                else if (id == "chkUnMap") {
                    $('[class^="unmapdealers"]').each(function () {
                        $(this).prop('checked', false);
                    });
                }
                
                $(chk).attr("value", "Check All");
            }
        }

        $("#unmapDealer").click(function () {
            var count = 0;
            $(".mapdealers").each(function () {
                if ($(this).prop("checked") == true) {
                    count += 1;
                }
            });

            if (count > 0) {
                UnmapDealer();
            }
            else {
                alert("Please select atleast one area to unmap.");
            }
        });

        function UnmapDealer() {
            var arealist = "";
            $(".mapdealers").each(function () {
                if ($(this).prop("checked") == true) {
                    arealist += $(this).attr("areaid") + ",";
                }
            });
            arealist = arealist.substring(0, arealist.length - 1);
            $("#hdnMapArea").val(arealist);
        }

        function validate() {
            var retVal = false;
            var areaList = "Mapped Cities with other dealer are : ";
            $(".unmapdealers").each(function () {
                if ($(this).prop("checked") == true) {
                    if ($(this).attr("dealerCount") > 0) {
                        areaList += $(this).attr("areaName") + ", ";
                        retVal = true;
                    }
                }
            });

            if (retVal)
                alert(areaList);

            return retVal;
        }

        $("#MapDealer").click(function () {
            var count = 0;
            var isSuccess = false;
            $(".unmapdealers").each(function () {
                if ($(this).prop("checked") == true) {
                    count += 1;
                }
            });

            if (count > 0) {
                isSuccess = MapDealer();
            }
            else {
                alert("Please select atleast one area to map.");

            }
            return isSuccess;
        });

        function MapDealer() {
            var arealist = "";
            $(".unmapdealers").each(function () {
                if ($(this).prop("checked") == true) {
                    arealist += $(this).attr("areaid") + ",";
                }
            });
            arealist = arealist.substring(0, arealist.length - 1);
            $("#hdnUnmapArea").val(arealist);
        }
    </script>
     
     </form>
</body>
</html>
