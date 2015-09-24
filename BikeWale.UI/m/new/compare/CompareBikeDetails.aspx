<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.New.CompareBikeDetails" Trace="false" %>
<%
    if (count == 2)
    {
        title = "Compare " + bikeDetails.Rows[0]["Make"] + " " + bikeDetails.Rows[0]["Model"] + " vs " + bikeDetails.Rows[1]["Make"] + " " + bikeDetails.Rows[1]["Model"] + " - BikeWale";
        keywords = "bike compare, compare bike, compare bikes, bike comparison, bike comparison india";
        description = "Compare " + bikeDetails.Rows[0]["Make"] + " " + bikeDetails.Rows[0]["Model"] + " and " + bikeDetails.Rows[1]["Make"] + " " + bikeDetails.Rows[1]["Model"] + " at Bikewale. Compare Price, Mileage, Engine Power, Space, Features, Specifications, Colours and much more.";
        canonical = "http://www.bikewale.com/comparebikes/" + bikeDetails.Rows[0]["MakeMaskingName"] + "-" + bikeDetails.Rows[0]["ModelMaskingName"] + "-vs-" + bikeDetails.Rows[1]["MakeMaskingName"] + "-" + bikeDetails.Rows[1]["ModelMaskingName"];
        AdPath = "/1017752/Bikewale_Mobile_NewBikes";
        AdId = "1398766302464";
        menu = "11";
        ShowTargeting = "1";
        TargetedModels = targetedModels;
    }
%>


<!-- #include file="/includes/headermobile.aspx" -->
<style>
    table td { text-align:center; height:25px;}
</style>
    <div class="padding5">
        <div id="br-cr">
            <a href="/m/new/" class="normal">New Bikes</a> &rsaquo; 
            <a href="/m/comparebikes/" class="normal">Compare Bike</a> &rsaquo; 
            <span class="lightgray">Compare Bikes</span>
        </div>
        <div style="margin-bottom:10px;">
	        <div style="float:left;top:3px;" class="position-rel"><h1>Compare Bikes</h1></div>
            <div style="float:right;"><a style="text-decoration:none !important; font-weight:normal; font-size:15px;" href="/m/comparebikes/" ><span class="btn btn-orange rounded-corner2 btn-sm">Edit Compare</span></a></div>
            <div class="clear"></div>
        </div>
        <% if(count == 2) { %>

        <div style="position: fixed; right: 9px; top: -5px; left: 9px; z-index: 10; display: block;" id="divFloat">
            <div style="padding:0px 0px;position:static;" class="box2 new-line5">
             <div id="divFloatBikeHeader" type="divBikeImage" style="border-bottom:1px solid #b3b4c6;">
                <div style="position:relative;" class="compareBikeContainer">
                    <div class="compareBikeBox compareBikeBorder-Rt">
                         <div class="new-line5"><a href="/m/<%=bikeDetails.Rows[0]["MakeMaskingName"] %>-bikes/<%=bikeDetails.Rows[0]["ModelMaskingName"] %>" style="color:black;text-decoration:none;font-weight:bold;" class="ui-link"><%=bikeDetails.Rows[0]["Make"] + " " + bikeDetails.Rows[0]["Model"] + " " + bikeDetails.Rows[0]["Version"]%></a></div>
                        <div class="new-line5 <%=bikeDetails.Rows[0]["Price"].ToString() == "0" ? "hide" : "" %>">Price : Rs. <%=Bikewale.Common.CommonOpn.FormatPrice(bikeDetails.Rows[0]["Price"].ToString()) %></div>
                        <div class="new-line5 <%=bikeDetails.Rows[0]["Price"].ToString() != "0" ? "hide" : "" %>"><a class="fillPopupData" href="/m/pricequote/default.aspx?version=<%=bikeDetails.Rows[0]["BikeVersionId"] %>" modelId="<%=bikeDetails.Rows[0]["Model"]%>">Check On-Road Price</a></div>
                    </div>
                <div style="width:23px;height:24px;position:absolute;top:17px;z-index:1;right:-10px;background: url('http://img.aeplcdn.com/bikewaleimg/images/icons-sheet.png?v=5.2') no-repeat scroll 0 0 transparent;background-position: 0 -1148px;"></div></div>
            
                <div style="position:relative;" class="compareBikeContainer">
                    <div class="compareBikeBox compareBikeBorder-Rt">
                        <div class="new-line5"><a href="/m/<%=bikeDetails.Rows[1]["MakeMaskingName"] %>-bikes/<%=bikeDetails.Rows[1]["ModelMaskingName"] %>" style="color:black;text-decoration:none;font-weight:bold;" class="ui-link"><%=bikeDetails.Rows[1]["Make"] + " " + bikeDetails.Rows[1]["Model"] + " " + bikeDetails.Rows[1]["Version"]%></a></div>
                        <div class="new-line5 <%=bikeDetails.Rows[1]["Price"].ToString() == "0" ? "hide" : "" %>">Price : Rs. <%=Bikewale.Common.CommonOpn.FormatPrice(bikeDetails.Rows[1]["Price"].ToString()) %></div>
                        <div class="new-line5 <%=bikeDetails.Rows[1]["Price"].ToString() != "0" ? "hide" : "" %>"><a href="/m/pricequote/default.aspx?version=<%=bikeDetails.Rows[1]["BikeVersionId"] %>" class="fillPopupData" modelId="<%=bikeDetails.Rows[1]["Model"]%>">Check On-Road Price</a></div>
                    </div>
                </div>
            
                <div style="clear:both;"></div>
            </div>
            <div style="text-align:center;" class="new-line10">
                Prices are Ex-showroom, <%= System.Configuration.ConfigurationManager.AppSettings["defaultName"] %>
            </div>
            <div id="divCompareBikeFloatMenu" class="new-line10 divCompareBikeMenu">
            <ul>
                <li contenttype="CD0" style="width:33%;" class="listActive"><div style="padding:10px 0px;">Specs</div></li>
                <li contenttype="CD2" style="width:33%;" class="list"><div class="compareBikeBorder-Rt compareBikeBorder-Lt" style="padding:10px 0px;">Features</div></li>
                <li style="width:34%;" contenttype="CD1" class="list"><div style="padding:10px 0px;">Colors</div></li>
                <li style="clear:both;"></li>
            </li></ul>
        </div>
        </div>
    </div>

        <div class="box2">
        <div id="divBikeHeader" type="divBikeImage" style="border-bottom:1px solid #b3b4c6;position:relative;">
            <div style="position:relative;" class="compareBikeContainer">
                <div class="compareBikeBox">
                    <%--<div id="divImgContainer" class="divImage"><a href="/m/<%=bikeDetails.Rows[0]["MakeMaskingName"] %>-bikes/<%=bikeDetails.Rows[0]["ModelMaskingName"] %>/" style="color:black;text-decoration:none;font-weight:bold;" class="ui-link"><img src="<%= !String.IsNullOrEmpty(bikeDetails.Rows[0]["HostURL"].ToString()) ? Bikewale.Common.ImagingFunctions.GetPathToShowImages("/bikewaleimg/models/" + bikeDetails.Rows[0]["largePic"].ToString(),bikeDetails.Rows[0]["HostURL"].ToString()) :  "http://img.bikewale.com/bikewaleimg/common/nobike.jpg" %>"></a></div>--%>
                    <div id="divImgContainer" class="divImage"><a href="/m/<%=bikeDetails.Rows[0]["MakeMaskingName"] %>-bikes/<%=bikeDetails.Rows[0]["ModelMaskingName"] %>/" style="color:black;text-decoration:none;font-weight:bold;" class="ui-link"><img src="<%= !String.IsNullOrEmpty(bikeDetails.Rows[0]["HostURL"].ToString()) ? Bikewale.Utility.Image.GetPathToShowImages(bikeDetails.Rows[0]["OriginalImagePath"].ToString(),bikeDetails.Rows[0]["HostURL"].ToString(),Bikewale.Utility.ImageSize._310x174) :  "http://img.bikewale.com/bikewaleimg/common/nobike.jpg" %>"></a></div>
                    <div id="divBikeName" class="new-line5"><a href="/m/<%=bikeDetails.Rows[0]["MakeMaskingName"] %>-bikes/<%=bikeDetails.Rows[0]["ModelMaskingName"] %>/" style="color:black;text-decoration:none;font-weight:bold;" class="ui-link"><%=bikeDetails.Rows[0]["Make"] + " " + bikeDetails.Rows[0]["Model"] + " " + bikeDetails.Rows[0]["Version"]%></a></div>
                    <div class="new-line5 <%=bikeDetails.Rows[0]["Price"].ToString() == "0" ? "hide" : "" %>">Price : Rs. <%=Bikewale.Common.CommonOpn.FormatPrice(bikeDetails.Rows[0]["Price"].ToString()) %></div>
                    <div class="new-line5 <%=bikeDetails.Rows[0]["Price"].ToString() != "0" ? "hide" : "" %>"><a href="/m/pricequote/default.aspx?version=<%=bikeDetails.Rows[0]["BikeVersionId"] %>" class="fillPopupData" modelId="<%=bikeDetails.Rows[0]["Model"]%>" >Check On-Road Price</a></div>
                </div>
            <div style="width:23px;height:24px;position:absolute;top:51.5px;z-index:1;right:-10px;background: url('http://img.aeplcdn.com/bikewaleimg/images/icons-sheet.png') no-repeat scroll 0 0 transparent;background-position: 0 -1148px;"></div></div>
                
            <div style="position:relative;" class="compareBikeContainer">
                <div class="compareBikeBox colorborderLt">
                    <%--<div id="divImgContainer1" class="divImage"><a href="/m/<%=bikeDetails.Rows[1]["MakeMaskingName"] %>-bikes/<%=bikeDetails.Rows[1]["ModelMaskingName"] %>/" style="color:black;text-decoration:none;font-weight:bold;" class="ui-link"><img src="<%= !String.IsNullOrEmpty(bikeDetails.Rows[1]["HostURL"].ToString()) ? Bikewale.Common.ImagingFunctions.GetPathToShowImages("/bikewaleimg/models/",bikeDetails.Rows[1]["HostURL"].ToString()) + bikeDetails.Rows[1]["largePic"] :  "http://img.bikewale.com/bikewaleimg/common/nobike.jpg" %>"></a></div>--%>
                    <div id="divImgContainer1" class="divImage"><a href="/m/<%=bikeDetails.Rows[1]["MakeMaskingName"] %>-bikes/<%=bikeDetails.Rows[1]["ModelMaskingName"] %>/" style="color:black;text-decoration:none;font-weight:bold;" class="ui-link"><img src="<%= !String.IsNullOrEmpty(bikeDetails.Rows[1]["HostURL"].ToString()) ? Bikewale.Utility.Image.GetPathToShowImages(bikeDetails.Rows[1]["OriginalImagePath"].ToString() ,bikeDetails.Rows[1]["HostURL"].ToString(),Bikewale.Utility.ImageSize._310x174) :  "http://img.bikewale.com/bikewaleimg/common/nobike.jpg" %>"></a></div>
                    <div id="divBikeName1" class="new-line5"><a href="/m/<%=bikeDetails.Rows[1]["MakeMaskingName"] %>-bikes/<%=bikeDetails.Rows[1]["ModelMaskingName"] %>/" style="color:black;text-decoration:none;font-weight:bold;" class="ui-link"><%=bikeDetails.Rows[1]["Make"] + " " + bikeDetails.Rows[1]["Model"] + " " + bikeDetails.Rows[1]["Version"]%></a></div>
                    <div class="new-line5 <%=bikeDetails.Rows[1]["Price"].ToString() == "0" ? "hide" : "" %>">Price : Rs. <%=Bikewale.Common.CommonOpn.FormatPrice(bikeDetails.Rows[1]["Price"].ToString()) %></div>
                    <div class="new-line5  <%=bikeDetails.Rows[1]["Price"].ToString() != "0" ? "hide" : "" %>"><a href="/m/pricequote/default.aspx?version=<%=bikeDetails.Rows[1]["BikeVersionId"] %>" class="fillPopupData" modelId="<%=bikeDetails.Rows[1]["Model"]%>">Check On-Road Price</a></div>
                </div>
            </div>
                
            <div class="clear"></div>
        </div>
        <div class="new-line10 showroom">
            Prices are Ex-showroom, <%=System.Configuration.ConfigurationManager.AppSettings["defaultName"] %><br>
            <input type="text" style="display:none;" data-role="none" id="txtTest" name="txtTest">
        </div>
        <div id="divCompareBikeMenu" class="new-line10 divCompareBikeMenu">
            <ul>
                <li contenttype="CD0" style="width:33%;" class="listActive"><div style="padding:10px 0px;">Specs</div></li>
                <li contenttype="CD2" style="width:33%;" class="list"><div class="compareBikeBorder-Rt compareBikeBorder-Lt" style="padding:10px 0px;">Features</div></li>
                <li style="width:34%;" contenttype="CD1" class="list"><div style="padding:10px 0px;">Colors</div></li>
                <li style="clear:both;"></li>
            </ul>
        </div>
        <div id="CD0" style="padding: 0px 5px;">
            <table cellspacing="0" cellpadding="0" class="table">
                <tbody>
                    <tr style="font-weight:bold;">
                        <td class="subCategoryBorder" style="text-align:left;font-size:14px;">Engine</td>
                        <td class="subCategoryBorder"><div onclick="BoxClicked(this);" class="rightMinus"></div></td>
                    </tr>
                    <tr style="display: table-row;">
                        <td colspan="2">
                        <table cellspacing="0" cellpadding="0" class="table tblItem">
                            <tbody>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Displacement (cc) </td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[0]["Displacement"].ToString())%></td>
                                    <td class="" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[1]["Displacement"].ToString())%></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Cylinders</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[0]["Cylinders"].ToString())%></td>
                                    <td class="" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[1]["Cylinders"].ToString())%></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Max Power</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[0]["MaxPower"].ToString())%></td>
                                    <td class="" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[1]["MaxPower"].ToString())%></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Maximum Torque</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[0]["MaximumTorque"].ToString())%></td>
                                    <td class="" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[1]["MaximumTorque"].ToString())%></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Bore (mm)</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[0]["Bore"].ToString())%></td>
                                    <td class="" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[1]["Bore"].ToString())%></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Stroke (mm)</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[0]["Stroke"].ToString())%></td>
                                    <td class="" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[1]["Stroke"].ToString())%></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Valves Per Cylinder</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[0]["ValvesPerCylinder"].ToString())%></td>
                                    <td class="" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[1]["ValvesPerCylinder"].ToString())%></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Fuel Delivery System</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[0]["FuelDeliverySystem"].ToString())%></td>
                                    <td class="" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[1]["FuelDeliverySystem"].ToString())%></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Fuel Type</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[0]["FuelType"].ToString())%></td>
                                    <td class="" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[1]["FuelType"].ToString())%></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Ignition</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[0]["Ignition"].ToString())%></td>
                                    <td class="" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[1]["Ignition"].ToString())%></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Spark Plugs (Per Cylinder)</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[0]["SparkPlugsPerCylinder"].ToString())%></td>
                                    <td class="" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[1]["SparkPlugsPerCylinder"].ToString())%></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Cooling System</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%=ShowFormatedData(bikeSpecs.Rows[0]["CoolingSystem"].ToString()) %></td>
                                    <td class="" style="width:50%;"><%=ShowFormatedData(bikeSpecs.Rows[1]["CoolingSystem"].ToString()) %></td>
                                </tr>
                            </tbody>
                        </table>
                        </td>
                    </tr>
                </tbody>
            </table>
            <table cellspacing="0" cellpadding="0" class="table">
                <tbody>
                    <tr style="font-weight:bold;">
                        <td class="subCategoryBorder" style="text-align:left;font-size:14px;">Transmission</td>
                        <td class="subCategoryBorder"><div onclick="BoxClicked(this);" class="rightMinus"></div></td>
                    </tr>
                    <tr style="display: table-row;">
                        <td colspan="2">
                        <table cellspacing="0" cellpadding="0" class="table tblItem">
                            <tbody>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Gearbox Type</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[0]["GearboxType"].ToString())%></td>
                                    <td class="" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[1]["GearboxType"].ToString())%></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">No. of Gears</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[0]["NoOfGears"].ToString())%></td>
                                    <td class="" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[1]["NoOfGears"].ToString())%></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Transmission Type</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[0]["TransmissionType"].ToString())%></td>
                                    <td class="" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[1]["TransmissionType"].ToString())%></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Clutch</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[0]["Clutch"].ToString())%></td>
                                    <td class="" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[1]["Clutch"].ToString())%></td>
                                </tr>
                            </tbody>
                        </table>
                        </td>
                    </tr>
                </tbody>
            </table>
            <table cellspacing="0" cellpadding="0" class="table">
                <tbody>
                    <tr style="font-weight:bold;">
                        <td class="subCategoryBorder" style="text-align:left;font-size:14px;">Performance</td>
                        <td class="subCategoryBorder"><div onclick="BoxClicked(this);" class="rightMinus"></div></td>
                    </tr>
                    <tr style="display: table-row;">
                        <td colspan="2">
                        <table cellspacing="0" cellpadding="0" class="table tblItem">
                            <tbody>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">0 to 60 kmph (Seconds)</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[0]["Performance_0_60_kmph"].ToString())%></td>
                                    <td class="" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[1]["Performance_0_60_kmph"].ToString())%></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">0 to 80 kmph (Seconds)</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[0]["Performance_0_80_kmph"].ToString())%></td>
                                    <td class="" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[1]["Performance_0_80_kmph"].ToString())%></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">0 to 40 m (Seconds)</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[0]["Performance_0_40_m"].ToString())%></td>
                                    <td class="" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[1]["Performance_0_40_m"].ToString())%></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Top Speed (Kmph)</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[0]["TopSpeed"].ToString())%></td>
                                    <td class="" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[1]["TopSpeed"].ToString())%></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">60 to 0 Kmph (Seconds, metres)</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[0]["Performance_60_0_kmph"].ToString())%></td>
                                    <td class="" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[1]["Performance_60_0_kmph"].ToString())%></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">80 to 0 kmph (Seconds, metres)</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[0]["Performance_80_0_kmph"].ToString())%></td>
                                    <td class="" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[1]["Performance_80_0_kmph"].ToString())%></td>
                                </tr>
                            </tbody>
                        </table>
                        </td>
                    </tr>
                </tbody>
            </table>
            <table cellspacing="0" cellpadding="0" class="table">
                <tbody>
                    <tr style="font-weight:bold;">
                        <td class="subCategoryBorder" style="text-align:left;font-size:14px;">Dimensions & Weight</td>
                        <td class="subCategoryBorder"><div onclick="BoxClicked(this);" class="rightMinus"></div></td>
                    </tr>
                    <tr style="display: table-row;">
                        <td colspan="2">
                        <table cellspacing="0" cellpadding="0" class="table tblItem">
                            <tbody>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Kerb Weight (Kg)</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[0]["KerbWeight"].ToString())%></td>
                                    <td class="" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[1]["KerbWeight"].ToString())%></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Overall Length (mm)</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[0]["OverallLength"].ToString())%></td>
                                    <td class="" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[1]["OverallLength"].ToString())%></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Overall Width (mm)</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[0]["OverallWidth"].ToString())%></td>
                                    <td class="" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[1]["OverallWidth"].ToString())%></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Overall Height (mm)</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[0]["OverallHeight"].ToString())%></td>
                                    <td class="" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[1]["OverallHeight"].ToString())%></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Wheelbase (mm)</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[0]["Wheelbase"].ToString())%></td>
                                    <td class="" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[1]["Wheelbase"].ToString())%></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Ground Clearance (mm)</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[0]["GroundClearance"].ToString())%></td>
                                    <td class="" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[1]["GroundClearance"].ToString())%></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Seat Height (mm)</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[0]["SeatHeight"].ToString())%></td>
                                    <td class="" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[1]["SeatHeight"].ToString())%></td>
                                </tr>
                            </tbody>
                        </table>
                        </td>
                    </tr>
                </tbody>
            </table>
            <table cellspacing="0" cellpadding="0" class="table">
                <tbody>
                    <tr style="font-weight:bold;">
                        <td class="subCategoryBorder" style="text-align:left;font-size:14px;">Fuel Efficiency & Range</td>
                        <td class="subCategoryBorder"><div onclick="BoxClicked(this);" class="rightMinus"></div></td>
                    </tr>
                    <tr style="display: table-row;">
                        <td colspan="2">
                        <table cellspacing="0" cellpadding="0" class="table tblItem">
                            <tbody>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Fuel Tank Capacity (Litres)</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[0]["FuelTankCapacity"].ToString())%></td>
                                    <td class="" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[1]["FuelTankCapacity"].ToString())%></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Reserve Fuel Capacity (Litres)</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[0]["ReserveFuelCapacity"].ToString())%></td>
                                    <td class="" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[1]["ReserveFuelCapacity"].ToString())%></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Fuel Efficiency Overall (Kmpl)</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[0]["FuelEfficiencyOverall"].ToString())%></td>
                                    <td class="" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[1]["FuelEfficiencyOverall"].ToString())%></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Fuel Efficiency Range (Km)</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[0]["FuelEfficiencyRange"].ToString())%></td>
                                    <td class="" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[1]["FuelEfficiencyRange"].ToString())%></td>
                                </tr>
                            </tbody>
                        </table>
                        </td>
                    </tr>
                </tbody>
            </table>
            <table cellspacing="0" cellpadding="0" class="table">
                <tbody>
                    <tr style="font-weight:bold;">
                        <td class="subCategoryBorder" style="text-align:left;font-size:14px;">Chassis & Suspension</td>
                        <td class="subCategoryBorder"><div onclick="BoxClicked(this);" class="rightMinus"></div></td>
                    </tr>
                    <tr style="display: table-row;">
                        <td colspan="2">
                        <table cellspacing="0" cellpadding="0" class="table tblItem">
                            <tbody>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Chassis Type</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[0]["ChassisType"].ToString())%></td>
                                    <td class="" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[1]["ChassisType"].ToString())%></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Front Suspension</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[0]["FrontSuspension"].ToString())%></td>
                                    <td class="" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[1]["FrontSuspension"].ToString())%></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Rear Suspension</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[0]["RearSuspension"].ToString())%></td>
                                    <td class="" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[1]["RearSuspension"].ToString())%></td>
                                </tr>
                            </tbody>
                        </table>
                        </td>
                    </tr>
                </tbody>
            </table>
            <table cellspacing="0" cellpadding="0" class="table">
                <tbody>
                    <tr style="font-weight:bold;">
                        <td class="subCategoryBorder" style="text-align:left;font-size:14px;">Braking</td>
                        <td class="subCategoryBorder"><div onclick="BoxClicked(this);" class="rightMinus"></div></td>
                    </tr>
                    <tr style="display: table-row;">
                        <td colspan="2">
                        <table cellspacing="0" cellpadding="0" class="table tblItem">
                            <tbody>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Brake Type</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[0]["BrakeType"].ToString())%></td>
                                    <td class="" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[1]["BrakeType"].ToString())%></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Front Disc</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFeature(bikeSpecs.Rows[0]["FrontDisc"].ToString())%></td>
                                    <td class="" style="width:50%;"><%= ShowFeature(bikeSpecs.Rows[1]["FrontDisc"].ToString())%></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Front Disc/Drum Size (mm)</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[0]["FrontDisc_DrumSize"].ToString())%></td>
                                    <td class="" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[1]["FrontDisc_DrumSize"].ToString())%></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Rear Disc</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFeature(bikeSpecs.Rows[0]["RearDisc"].ToString())%></td>
                                    <td class="" style="width:50%;"><%= ShowFeature(bikeSpecs.Rows[1]["RearDisc"].ToString())%></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Rear Disc/Drum Size (mm)</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[0]["RearDisc_DrumSize"].ToString())%></td>
                                    <td class="" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[1]["RearDisc_DrumSize"].ToString())%></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Calliper Type</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[0]["CalliperType"].ToString())%></td>
                                    <td class="" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[1]["CalliperType"].ToString())%></td>
                                </tr>
                            </tbody>
                        </table>
                        </td>
                    </tr>
                </tbody>
            </table>
            <table cellspacing="0" cellpadding="0" class="table">
                <tbody>
                    <tr style="font-weight:bold;">
                        <td class="subCategoryBorder" style="text-align:left;font-size:14px;">Wheels & Tyres</td>
                        <td class="subCategoryBorder"><div onclick="BoxClicked(this);" class="rightMinus"></div></td>
                    </tr>
                    <tr style="display: table-row;">
                        <td colspan="2">
                        <table cellspacing="0" cellpadding="0" class="table tblItem">
                            <tbody>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Wheel Size (inches)</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[0]["WheelSize"].ToString())%></td>
                                    <td class="" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[1]["WheelSize"].ToString())%></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Front Tyre</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[0]["FrontTyre"].ToString())%></td>
                                    <td class="" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[1]["FrontTyre"].ToString())%></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Rear Tyre</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[0]["RearTyre"].ToString())%></td>
                                    <td class="" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[1]["RearTyre"].ToString())%></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Tubeless Tyres</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFeature(bikeSpecs.Rows[0]["TubelessTyres"].ToString())%></td>
                                    <td class="" style="width:50%;"><%= ShowFeature(bikeSpecs.Rows[1]["TubelessTyres"].ToString())%></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Radial Tyres</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFeature(bikeSpecs.Rows[0]["RadialTyres"].ToString())%></td>
                                    <td class="" style="width:50%;"><%= ShowFeature(bikeSpecs.Rows[1]["RadialTyres"].ToString())%></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Alloy Wheels</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFeature(bikeSpecs.Rows[0]["AlloyWheels"].ToString())%></td>
                                    <td class="" style="width:50%;"><%= ShowFeature(bikeSpecs.Rows[1]["AlloyWheels"].ToString())%></td>
                                </tr>
                            </tbody>
                        </table>
                        </td>
                    </tr>
                </tbody>
            </table>
            <table cellspacing="0" cellpadding="0" class="table">
                <tbody>
                    <tr style="font-weight:bold;">
                        <td class="subCategoryBorder" style="text-align:left;font-size:14px;">Electricals</td>
                        <td class="subCategoryBorder"><div onclick="BoxClicked(this);" class="rightMinus"></div></td>
                    </tr>
                    <tr style="display: table-row;">
                        <td colspan="2">
                        <table cellspacing="0" cellpadding="0" class="table tblItem">
                            <tbody>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Electric System</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[0]["ElectricSystem"].ToString())%></td>
                                    <td class="" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[1]["ElectricSystem"].ToString())%></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Battery</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[0]["Battery"].ToString())%></td>
                                    <td class="" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[1]["Battery"].ToString())%></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Headlight Type</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[0]["HeadlightType"].ToString())%></td>
                                    <td class="" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[1]["HeadlightType"].ToString())%></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Headlight Bulb Type</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[0]["HeadlightBulbType"].ToString())%></td>
                                    <td class="" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[1]["HeadlightBulbType"].ToString())%></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Brake/Tail Light</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[0]["Brake_Tail_Light"].ToString())%></td>
                                    <td class="" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[1]["Brake_Tail_Light"].ToString())%></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Turn Signal</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[0]["TurnSignal"].ToString())%></td>
                                    <td class="" style="width:50%;"><%= ShowFormatedData(bikeSpecs.Rows[1]["TurnSignal"].ToString())%></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Pass Light</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFeature(bikeSpecs.Rows[0]["PassLight"].ToString())%></td>
                                    <td class="" style="width:50%;"><%= ShowFeature(bikeSpecs.Rows[1]["PassLight"].ToString())%></td>
                                </tr>
                            </tbody>
                        </table>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div id="CD2" class="hide" style="padding: 0px 5px;">
            <table cellspacing="0" cellpadding="0" class="table">
                <tbody>
                    <tr style="font-weight:bold;">
                        <td class="subCategoryBorder" style="text-align:left;font-size:14px;">Features</td>
                        <td class="subCategoryBorder"><div onclick="BoxClicked(this);" class="rightMinus"></div></td>
                    </tr>
                    <tr style="display: table-row;">
                        <td colspan="2">
                        <table cellspacing="0" cellpadding="0" class="table tblItem">
                            <tbody>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Speedometer</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFormatedData(bikeFeatures.Rows[0]["Speedometer"].ToString()) %></td>
                                    <td class="" style="width:50%;"><%= ShowFormatedData(bikeFeatures.Rows[1]["Speedometer"].ToString()) %></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Tachometer</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFeature(bikeFeatures.Rows[0]["Tachometer"].ToString()) %></td>
                                    <td class="" style="width:50%;"><%= ShowFeature(bikeFeatures.Rows[1]["Tachometer"].ToString()) %></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Tachometer Type</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFormatedData(bikeFeatures.Rows[0]["TachometerType"].ToString()) %></td>
                                    <td class="" style="width:50%;"><%= ShowFormatedData(bikeFeatures.Rows[1]["TachometerType"].ToString()) %></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Shift Light</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFeature(bikeFeatures.Rows[0]["ShiftLight"].ToString()) %></td>
                                    <td class="" style="width:50%;"><%= ShowFeature(bikeFeatures.Rows[1]["ShiftLight"].ToString()) %></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Electric Start</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFeature(bikeFeatures.Rows[0]["ElectricStart"].ToString()) %></td>
                                    <td class="" style="width:50%;"><%= ShowFeature(bikeFeatures.Rows[1]["ElectricStart"].ToString()) %></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Tripmeter</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFeature(bikeFeatures.Rows[0]["Tripmeter"].ToString()) %></td>
                                    <td class="" style="width:50%;"><%= ShowFeature(bikeFeatures.Rows[1]["Tripmeter"].ToString()) %></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">No. of Tripmeters</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFormatedData(bikeFeatures.Rows[0]["NoOfTripmeters"].ToString()) %></td>
                                    <td class="" style="width:50%;"><%= ShowFormatedData(bikeFeatures.Rows[1]["NoOfTripmeters"].ToString()) %></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Tripmeter Type</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFormatedData(bikeFeatures.Rows[0]["TripmeterType"].ToString()) %></td>
                                    <td class="" style="width:50%;"><%= ShowFormatedData(bikeFeatures.Rows[1]["TripmeterType"].ToString()) %></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Low Fuel Indicator</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFeature(bikeFeatures.Rows[0]["LowFuelIndicator"].ToString()) %></td>
                                    <td class="" style="width:50%;"><%= ShowFeature(bikeFeatures.Rows[1]["LowFuelIndicator"].ToString()) %></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Low Oil Indicator</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFeature(bikeFeatures.Rows[0]["LowOilIndicator"].ToString()) %></td>
                                    <td class="" style="width:50%;"><%= ShowFeature(bikeFeatures.Rows[1]["LowOilIndicator"].ToString()) %></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Low Battery Indicator</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFeature(bikeFeatures.Rows[0]["LowBatteryIndicator"].ToString()) %></td>
                                    <td class="" style="width:50%;"><%= ShowFeature(bikeFeatures.Rows[1]["LowBatteryIndicator"].ToString()) %></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Fuel Gauge</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFeature(bikeFeatures.Rows[0]["FuelGauge"].ToString()) %></td>
                                    <td class="" style="width:50%;"><%= ShowFeature(bikeFeatures.Rows[1]["FuelGauge"].ToString()) %></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Digital Fuel Gauges</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFeature(bikeFeatures.Rows[0]["DigitalFuelGauge"].ToString()) %></td>
                                    <td class="" style="width:50%;"><%= ShowFeature(bikeFeatures.Rows[1]["DigitalFuelGauge"].ToString()) %></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Pillion Seat</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFeature(bikeFeatures.Rows[0]["PillionSeat"].ToString()) %></td>
                                    <td class="" style="width:50%;"><%= ShowFeature(bikeFeatures.Rows[1]["PillionSeat"].ToString()) %></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Pillion Footrest</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFeature(bikeFeatures.Rows[0]["PillionFootrest"].ToString()) %></td>
                                    <td class="" style="width:50%;"><%= ShowFeature(bikeFeatures.Rows[1]["PillionFootrest"].ToString()) %></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Pillion Backrest</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFeature(bikeFeatures.Rows[0]["PillionBackrest"].ToString()) %></td>
                                    <td class="" style="width:50%;"><%= ShowFeature(bikeFeatures.Rows[1]["PillionBackrest"].ToString()) %></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Pillion Grabrail</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFeature(bikeFeatures.Rows[0]["PillionGrabrail"].ToString()) %></td>
                                    <td class="" style="width:50%;"><%= ShowFeature(bikeFeatures.Rows[1]["PillionGrabrail"].ToString()) %></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Stand Alarm</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFeature(bikeFeatures.Rows[0]["StandAlarm"].ToString()) %></td>
                                    <td class="" style="width:50%;"><%= ShowFeature(bikeFeatures.Rows[1]["StandAlarm"].ToString()) %></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Stepped Seat</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFeature(bikeFeatures.Rows[0]["SteppedSeat"].ToString()) %></td>
                                    <td class="" style="width:50%;"><%= ShowFeature(bikeFeatures.Rows[1]["SteppedSeat"].ToString()) %></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Antilock Braking System</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFeature(bikeFeatures.Rows[0]["AntilockBrakingSystem"].ToString()) %></td>
                                    <td class="" style="width:50%;"><%= ShowFeature(bikeFeatures.Rows[1]["AntilockBrakingSystem"].ToString()) %></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Killswitch</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFeature(bikeFeatures.Rows[0]["Killswitch"].ToString()) %></td>
                                    <td class="" style="width:50%;"><%= ShowFeature(bikeFeatures.Rows[1]["Killswitch"].ToString()) %></td>
                                </tr>
                                <tr class="compareBikeItemContainer">
                                    <td colspan="2">Clock</td>
                                </tr>
                                <tr>
                                    <td class="compareBikeItemBorder-Rt" style="width:50%;"><%= ShowFeature(bikeFeatures.Rows[0]["Clock"].ToString()) %></td>
                                    <td class="" style="width:50%;"><%= ShowFeature(bikeFeatures.Rows[1]["Clock"].ToString()) %></td>
                                </tr>
                            </tbody>
                        </table>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div id="CD1" class="hide" style="padding: 0px 5px;">
            <table cellspacing="0" cellpadding="0" width="100%" border="0">
	            <tbody>
		            <tr class="compareBikeItemContainer">
			            <td colspan="2">Colors</td>
		            </tr>
		            <tr class="colorDiv">
			            <td class="compareBikeItemBorder-Rt" style="width:50%;padding-top:5px;"><%= GetModelColors(bikeDetails.Rows[0]["BikeVersionId"].ToString())%></td>
			            <td class="" style="width:50%;padding-top:5px;"><%= GetModelColors(bikeDetails.Rows[1]["BikeVersionId"].ToString())%></td>
		            </tr>
	            </tbody>
            </table>
        </div>
    </div>
    <% } %>
    </div>
<script type="text/javascript">
    $(document).ready(function () {
        $("#divFloat").hide();

        $(window).bind("scroll", function () {
            var offset = $(this).scrollTop();

            if (offset > parseInt($("#divBikeName").position().top) + 180) {
                $("#divFloat").show();
            }
            else {
                $("#divFloat").hide();
            }
        });
    });

    function BoxClicked(div) {
        if ($(div).attr("class") == "rightMinus") {
            $(div).removeClass("rightMinus").addClass("rightPlus");
            $(div).parent("td").parent("tr").next().hide();
        }
        else if ($(div).attr("class") == "rightPlus") {
            $(div).removeClass("rightPlus").addClass("rightMinus");
            $(div).parent("td").parent("tr").next().show();
        }
    }


    $(".divCompareBikeMenu li").click(function () {
        var contentType = $(this).attr("contentType");
        $(".divCompareBikeMenu li").each(function () {
            if ($(this).attr("contentType") == contentType) {
                if ($(this).hasClass("list")) {
                    $(this).removeClass("list").addClass("listActive");
                    $("#" + contentType).show();
                    window.scrollTo(0,1);
                }
            }
            else {
                if ($(this).hasClass("listActive")) {
                    $(this).removeClass("listActive").addClass("list");
                    $("#" + $(this).attr("contentType")).hide();
                }
            }
        });
    });
</script>
<!-- #include file="/includes/footermobile.aspx" -->

