<%@ Page Language="C#" Inherits="Bikewale.New.comparebikes" AutoEventWireup="false" Trace="false" %>

<%@ Register TagPrefix="AddBike" TagName="AddBike" Src="~/controls/AddBikeToCompare.ascx" %>
<%@ Import Namespace="Bikewale.Utility.StringExtention" %>
<%@ Register Src="~/controls/SimilarCompareBikes.ascx" TagPrefix="BW" TagName="SimilarBikes" %>

<%
    title = "Compare " + pageTitle + "- BikeWale";
    description = "BikeWale&reg; - Compare " + keyword + ". Compare Price, Mileage, Engine Power, Specifications, features and colours of bikes on BikeWale.";
    keywords = "Compare bikes,Compare bike prices,Compare bike specifications,Compare bike mileage,Compare features,Compare colours,Compare " + pageTitle;
    canonical = "https://www.bikewale.com/comparebikes/" + canonicalUrl + "/";
    if (count == 2)
    {
        alternate = "https://www.bikewale.com/m/comparebikes/" + canonicalUrl + "/";
    }
    AdId = "1395986297721";
    AdPath = "/1017752/BikeWale_CompareBikes_";
    ShowTargeting = "1";
    TargetedModels = targetedModels;
    isAd300x250Shown = false;
    isAd300x250_BTFShown = false;
%>

<!-- #include file="/includes/headNew.aspx" -->
<style>.Features .repeater-1 td.info-td,.Specs .repeater-1 td.info-td,.blue{cursor:pointer}.container-border{border-top:1px solid #F3F2F2}.tab_inner_container .container-border th.mainth{border-right:1px solid #F3F2F2}.tab_inner_container .container-1{border-left:1px solid #F3F2F2;border-right:1px solid #F3F2F2}.tab_inner_container .container-1 .repeater-1{border-right:1px solid #F3F2F2;vertical-align:top}.tab_inner_container .container-1 .specs-title th{border-right:0 solid #F3F2F2}.tab_inner_container .container-1 .repeater-2{border-right:1px solid #F3F2F2}.container-1 table table th,.container-border td.maintd td div,.tab_inner_container .maintd{border-right:0}.tab_inner_container .mainth{background:0 0}.tab_inner_container th{width:170px;border-right:0}.tbl-compare td.headerSpecs{border-bottom:1px solid #FFF}.tab_inner_container .tbl-compare td{height:60px;text-align:center;vertical-align:middle;padding:0}.tab_inner_container .tbl-compare th{height:30px;text-align:center;vertical-align:middle;padding:0}.tab_inner_container .tbl-compare .maintd td{vertical-align:top}.container-border td.maintd td{border-bottom:0}.container-border th.mainth{border-right:1px solid #F3F2F2;padding:5px;margin:0 0 5px}.container-border td.maintd{border-right:1px solid #F3F2F2;border-bottom:0 solid #F3F2F2;vertical-align:top}.container-border td.maintd:last-child{border-right:0 solid #F3F2F2}.container-border td.maintd table{padding:5px;margin:0 0 5px}.tab_inner_container .container-1 .repeater-1 .tblColor td{vertical-align:top;border-bottom:0}.tab_inner_container .container-1 .Colors td:last-child.repeater-1,.tab_inner_container .container-1 .repeater-1 .tblColor th{border-right:0}.tab_inner_container .tbl-compare .specs-title td,.tab_inner_container .tbl-compare .specs-title th{text-align:left;padding-left:5px}.maintd .bikemain{border:0 solid red;overflow:hidden}.maintd .bikename{margin-bottom:10px;text-align:center;font-size:12px;min-height:34px}.maintd .bikeclose{overflow:hidden;display:inline-block;text-align:right;float:right;font-size:10.5px}.sixcolum{width:123px}.fivecolum{width:149px}.fourcolum{width:186px}.threecolum{width:248px}.sixcolum img.second-img{width:103px;height:auto}.fivecolum img.second-img{width:128px;height:auto}.fourcolum img.second-img{width:166px;height:auto}.threecolum img.second-img{width:228px;height:auto}.featuredBike{background-color:#fffae8!important}.blue{color:#0056cc;text-decoration:none}.info-td{position:relative}.info-popup{display:none;background-color:#fff;position:absolute;top:45px;left:100px;box-shadow:0 0 15px #ccc;width:225px;min-height:40px;border:1px solid #e0e0e0;padding:10px;z-index:2}.Features .repeater-1 td.info-td:hover .info-popup,.Specs .repeater-1 td.info-td:hover .info-popup{display:block}div.color-box{width:50px;height:50px;margin:10px auto auto;background:#ccc;border:1px solid #e2e2e2;-moz-border-radius:2px;-webkit-border-radius:2px;-o-border-radius:2px;-ms-border-radius:2px;border-radius:2px}.color-box.color-count-one span{width:100%;height:100%;display:block!important;background:#eee}.color-box.color-count-two span{width:100%;height:50%;display:block!important;background:#eee}.color-box.color-count-three span{width:100%;height:33.33%;display:block!important}.similarbikes{background-color:#efeeee!important;margin-top:15px;padding:10px}.usedbikes{background-color:#efeeee!important;margin-top:30px;padding:10px}.related-comparison-wrapper{display:inline-block;vertical-align:top;width:220px}.bikeclose .cross-md-dark-grey{background-position:-62px -223px}.bikeclose .cross-md-dark-grey:hover{background-position:-62px -246px}.compare-tick{width:18px;height:14px;background-position:-153px -515px}.compare-cross{width:14px;height:14px;background-position:-181px -515px}.position-abt{font-size:8px;}.pos-right35 { right: 35px; }.fivecolum .pos-right35 { right: 20px }.btn-secondary-small { padding: 6px 0; width: 180px; font-size: 14px; } .tab_inner_container .tbl-compare .similarbikes .mainth { text-align: left; padding-left: 5px; }
    .tab_inner_container .tbl-compare .usedbikes .mainth{ text-align: left; padding-left: 5px}
    .usedbikes .threecolum, .usedbikes .fourcolum, .usedbikes .fivecolum {border-left: 1px solid #fff;}
</style>

<div class="container_12">
    <div class="grid_12">
        <ul class="breadcrumb">
            <li>You are here: </li>
            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                <a itemprop="url" href="/"><span itemprop="title">Home</span></a>
            </li>
            <li class="fwd-arrow">&rsaquo;</li>
            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                <a itemprop="url" href="/comparebikes/"><span itemprop="title">Compare Bikes in India</span></a>
            </li>
            <li class="fwd-arrow">&rsaquo;</li>
            <li class="current"><strong>Bike Comparison</strong></li>
        </ul>
        <div class="clear"></div>
    </div>

    <div class="grid_12 margin-top10">
        <!--    Left Container starts here -->
        <span id="spnError" class="error" runat="server"></span>
        <h1 class="red margin-bottom15">Bike Comparison -
            <%= pgTitle %>
        </h1>
        <div class="padding-top20 featured-bike-tabs">
            <ul class="featured-bike-tabs-inner padding-top20">
                <li id="liSpecs" class="fbike-active-tab first_tab">Specs</li>
                <li id="liFeatures">Features</li>
                <li id="liColors">Colours</li>
            </ul>
        </div>
        <div class="tab_inner_container">
            <table width="100%" class="tbl-compare margin-bottom15" cellpadding="0" border="0" cellspacing="0">
                <tr>
                    <td class="container-1">
                        <table width="100%" class="container-border" cellpadding="0" border="0" cellspacing="0">
                            <tr>
                                <th class="mainth">&nbsp;</th>
                                <asp:repeater runat="server" id="rptCommon">
                                         <ItemTemplate>
                                           <td class="maintd <%# Container.ItemIndex == featuredBikeIndex ? "featuredBike" : ""  %>">
                                               <table class="<%=!isFeatured ? ((count==2) ? "threecolum": (count==3 ? "fourcolum" : (count==4 ? "fourcolum" : ""))) : ((count==2) ? "threecolum": (count==3 ? "fourcolum" : (count==4 ? "fivecolum" : (count==5 ? "fivecolum" : ""))))%>" cellpadding="0" border="0" cellspacing="0">
                                                   <tr><td>
                                                       <div class="bikeclose"><a class='delBike pointer font16 right-float <%= !isFeatured ? ((count==2) ? "hide" : "") : ((count==3) ? "hide" : "") %>' versionId='<%# DataBinder.Eval(Container.DataItem,"BikeVersionId") %>' ><span class="bwsprite cross-md-dark-grey"></span></a></div>
                                                       <div class="clear"></div>
                                                       <div class="bikemain">
                                                           <div class="bikename"><strong><a class="blue" href='/<%# DataBinder.Eval(Container.DataItem,"MakeMaskingName") +"-bikes/"+ DataBinder.Eval(Container.DataItem,"ModelMaskingName") %>/' title='<%# DataBinder.Eval(Container.DataItem,"Make") + " " + DataBinder.Eval(Container.DataItem,"Model") + " " + DataBinder.Eval(Container.DataItem,"Version") + " Details"%>'><%#DataBinder.Eval(Container.DataItem,"Make") + " " + DataBinder.Eval(Container.DataItem,"Model")%></a></strong></div>
                                                           <div class="margin-bottom10" style="margin-top:-10px;"><%# DataBinder.Eval(Container.DataItem,"Version") %></div>
                                                        </div>
                                                       </td>
                                                   </tr>
                                                   <%--<tr><td><a title='View complete details of <%# DataBinder.Eval(Container.DataItem,"Bike")%>' href='/<%# DataBinder.Eval(Container.DataItem,"MakeMaskingName")%>-bikes/<%#DataBinder.Eval(Container.DataItem,"ModelMaskingName") %>/'><img class="second-img" alt="<%#DataBinder.Eval(Container.DataItem,"Bike")%>" src="<%# !String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem,"HostURL").ToString()) ? Bikewale.Common.ImagingFunctions.GetPathToShowImages("/bikewaleimg/models/", DataBinder.Eval(Container.DataItem,"HostURL").ToString()) + DataBinder.Eval(Container.DataItem,"largePic") :  "http://imgd1.aeplcdn.com/0x0/bw/static/design15/old-images/d/nobike.jpg" %>" border="0"/></a></td></tr>--%>
                                                   <tr>
                                                       <td>
                                                           <a title='View complete details of <%# DataBinder.Eval(Container.DataItem,"Bike")%>' href='/<%# DataBinder.Eval(Container.DataItem,"MakeMaskingName")%>-bikes/<%#DataBinder.Eval(Container.DataItem,"ModelMaskingName") %>/'><img class="second-img" alt="<%#DataBinder.Eval(Container.DataItem,"Bike")%>" src="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem,"OriginalImagePath").ToString(), DataBinder.Eval(Container.DataItem,"HostURL").ToString(),Bikewale.Utility.ImageSize._310x174) %>" border="0"/></a>
                                                       </td>
                                                   </tr>                                                   
                                                   <tr class="<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"Futuristic")) ? "hide" : "" %>" >
                                                       <td>
                                                        <strong>Price Rs. <%# Bikewale.Common.CommonOpn.FormatPrice(DataBinder.Eval(Container.DataItem,"Price").ToString()) %></strong><br />
                                                        <span class="<%#String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem,"Price").ToString())?"hide":"" %>">Ex-Showroom, <%= ConfigurationManager.AppSettings["defaultName"].ToString() %></span>
                                                        <%# DataBinder.Eval(Container.DataItem,"Price").ToString() == "" ? "" : "<div class='la' style='margin-top:5px;'><a data-pqSourceId='"+ (int)Bikewale.Entities.PriceQuote.PQSourceEnum.Desktop_CompareBike +"' class='getquotation blue' data-modelId='"+ DataBinder.Eval(Container.DataItem,"ModelId") +"' href=\"/pricequote/default.aspx?version=" + DataBinder.Eval(Container.DataItem,"BikeVersionId") + "\">Check on-road price</a></div>"%>
                                                       <% if(isSponsored) { %>
                                                           <span class="<%# Container.ItemIndex != featuredBikeIndex ? "hide" : ""  %>"><%= string.Format("<p class='position-rel margin-top10'><span class='position-abt pos-right35 pos-top0'>Ad</span><a target='_blank' rel='nofollow' class='blue padding-top5 bw-ga' href='{0}' c='Comparison_Page' a='Sponsored_Comparison' l='{1}'>Know more</a></p>", knowMoreHref,featuredBikeName ) %></span>
                                                           <% } %>
                                                       </td>
                                                   </tr>
                                                   <tr class="<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"Futuristic")) ? "" : "hide" %>" >
                                                       <td>
                                                           <p class="margin-top30"><strong>Price Rs. <%= estimatePrice %> onwards </strong>(Expected)</p>
                                                           <p class="margin-top5"><strong><%= estimateLaunchDate %></strong></p><p>(Expected Launch)</p>
                                                       </td>
                                                   </tr>
                                               </table>
                                           </td>                                       
                                        </ItemTemplate>
                                    </asp:repeater>
                                <% if (isFeatured ? (count < 5) : (count < 4))
                                   { %>
                                <td class="maintd">
                                    <table cellpadding="0" border="0" cellspacing="0" id="addAnotherBike" class="<%=!isFeatured ? ((count==2) ? "threecolum": (count==3 ? "fourcolum" : (count==4 ? "fourcolum" : ""))) : ((count==2) ? "threecolum": (count==3 ? "fourcolum" : (count==4 ? "fivecolum" : (count==5 ? "fivecolum" : ""))))%>">
                                        <tr>
                                            <td>
                                                <AddBike:AddBike ID="addBike" runat="server"></AddBike:AddBike>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <% } %>
                            </tr>
                            <tr class="Specs">
                                <td align="left" class="specs-title">
                                    <table width="165" cellpadding="0" border="0" cellspacing="0">
                                        <tr>
                                            <th>Engine</th>
                                        </tr>
                                        <tr>
                                            <td class="headerSpecs">Displacement (cc)</td>
                                        </tr>
                                        <tr>
                                            <td class="headerSpecs">Cylinders</td>
                                        </tr>
                                        <tr>
                                            <td class="headerSpecs">Max Power</td>
                                        </tr>
                                        <tr>
                                            <td class="headerSpecs">Maximum Torque</td>
                                        </tr>
                                        <tr>
                                            <td class="headerSpecs">Bore (mm)</td>
                                        </tr>
                                        <tr>
                                            <td class="headerSpecs">Stroke (mm)</td>
                                        </tr>
                                        <tr>
                                            <td class="headerSpecs">Valves Per Cylinder</td>
                                        </tr>
                                        <tr>
                                            <td class="headerSpecs">Fuel Delivery System</td>
                                        </tr>
                                        <tr>
                                            <td class="headerSpecs">Fuel Type</td>
                                        </tr>
                                        <tr>
                                            <td class="headerSpecs">Ignition</td>
                                        </tr>
                                        <tr>
                                            <td class="headerSpecs">Spark Plugs (Per Cylinder)</td>
                                        </tr>
                                        <tr>
                                            <td class="headerSpecs">Cooling System</td>
                                        </tr>
                                        <tr>
                                            <th class="headerSpecs">Transmission</th>
                                        </tr>
                                        <tr>
                                            <td class="headerSpecs">Gearbox Type</td>
                                        </tr>
                                        <tr>
                                            <td class="headerSpecs">No Of Gears</td>
                                        </tr>
                                        <tr>
                                            <td class="headerSpecs">Transmission Type</td>
                                        </tr>
                                        <tr>
                                            <td class="headerSpecs">Clutch</td>
                                        </tr>
                                        <tr>
                                            <th class="headerSpecs">Performance</th>
                                        </tr>
                                        <tr>
                                            <td class="headerSpecs">0 to 60 kmph (Seconds)</td>
                                        </tr>
                                        <tr>
                                            <td class="headerSpecs">0 to 80 kmph (Seconds)</td>
                                        </tr>
                                        <tr>
                                            <td class="headerSpecs">0 to 40 m (Seconds)</td>
                                        </tr>
                                        <tr>
                                            <td class="headerSpecs">Top Speed (Kmph)</td>
                                        </tr>
                                        <tr>
                                            <td class="headerSpecs">60 to 0 Kmph (Seconds, metres)</td>
                                        </tr>
                                        <tr>
                                            <td class="headerSpecs">80 to 0 kmph (Seconds, metres)</td>
                                        </tr>
                                        <tr>
                                            <th class="headerSpecs">Dimensions & Weight</th>
                                        </tr>
                                        <tr>
                                            <td class="headerSpecs">Kerb Weight (Kg)</td>
                                        </tr>
                                        <tr>
                                            <td class="headerSpecs">Overall Length (mm)</td>
                                        </tr>
                                        <tr>
                                            <td class="headerSpecs">Overall Width (mm)</td>
                                        </tr>
                                        <tr>
                                            <td class="headerSpecs">Overall Height (mm)</td>
                                        </tr>
                                        <tr>
                                            <td class="headerSpecs">Wheelbase (mm)</td>
                                        </tr>
                                        <tr>
                                            <td class="headerSpecs">Ground Clearance (mm)</td>
                                        </tr>
                                        <tr>
                                            <td class="headerSpecs">Seat Height (mm)</td>
                                        </tr>
                                        <tr>
                                            <th class="headerSpecs">Fuel Efficiency & Range</th>
                                        </tr>
                                        <tr>
                                            <td class="headerSpecs">Fuel Tank Capacity (Litres)</td>
                                        </tr>
                                        <tr>
                                            <td class="headerSpecs">Reserve Fuel Capacity (Litres)</td>
                                        </tr>
                                        <tr>
                                            <td class="headerSpecs">FuelEfficiency Overall (Kmpl)</td>
                                        </tr>
                                        <tr>
                                            <td class="headerSpecs">Fuel Efficiency Range (Km)</td>
                                        </tr>
                                        <tr>
                                            <th class="headerSpecs">Chassis & Suspension</th>
                                        </tr>
                                        <tr>
                                            <td class="headerSpecs">Chassis Type</td>
                                        </tr>
                                        <tr>
                                            <td class="headerSpecs">Front Suspension</td>
                                        </tr>
                                        <tr>
                                            <td class="headerSpecs">Rear Suspension</td>
                                        </tr>
                                        <tr>
                                            <th class="headerSpecs">Braking</th>
                                        </tr>
                                        <tr>
                                            <td class="headerSpecs">Brake Type</td>
                                        </tr>
                                        <tr>
                                            <td class="headerSpecs">Front Disc</td>
                                        </tr>
                                        <tr>
                                            <td class="headerSpecs">Front Disc/Drum Size (mm)</td>
                                        </tr>
                                        <tr>
                                            <td class="headerSpecs">Rear Disc</td>
                                        </tr>
                                        <tr>
                                            <td class="headerSpecs">Rear Disc/Drum Size (mm)</td>
                                        </tr>
                                        <tr>
                                            <td class="headerSpecs">Calliper Type</td>
                                        </tr>
                                        <tr>
                                            <th class="headerSpecs">Wheels & Tyres</th>
                                        </tr>
                                        <tr>
                                            <td class="headerSpecs">Wheel Size (inches)</td>
                                        </tr>
                                        <tr>
                                            <td class="headerSpecs">Front Tyre</td>
                                        </tr>
                                        <tr>
                                            <td class="headerSpecs">Rear Tyre</td>
                                        </tr>
                                        <tr>
                                            <td class="headerSpecs">Tubeless Tyres</td>
                                        </tr>
                                        <tr>
                                            <td class="headerSpecs">Radial Tyres</td>
                                        </tr>
                                        <tr>
                                            <td class="headerSpecs">Alloy Wheels</td>
                                        </tr>
                                        <tr>
                                            <th class="headerSpecs">Electricals</th>
                                        </tr>
                                        <tr>
                                            <td class="headerSpecs">Electric System</td>
                                        </tr>
                                        <tr>
                                            <td class="headerSpecs">Battery</td>
                                        </tr>
                                        <tr>
                                            <td class="headerSpecs">Headlight Type</td>
                                        </tr>
                                        <tr>
                                            <td class="headerSpecs">Headlight Bulb Type</td>
                                        </tr>
                                        <tr>
                                            <td class="headerSpecs">Brake/Tail Light</td>
                                            <tr>
                                                <td class="headerSpecs">Turn Signal</td>
                                            </tr>
                                        <tr>
                                            <td class="headerSpecs">Pass Light</td>
                                        </tr>
                                    </table>
                                </td>
                                <asp:repeater runat="server" id="rptSpecs">
                                            <ItemTemplate>
                                            <td class="repeater-1 <%# Container.ItemIndex == featuredBikeIndex ? "featuredBike" : ""  %>">
                                                <table cellpadding="0" border="0" cellspacing="0" class="<%=!isFeatured ? ((count==2) ? "threecolum": (count==3 ? "fourcolum" : (count==4 ? "fourcolum" : ""))) : ((count==2) ? "threecolum": (count==3 ? "fourcolum" : (count==4 ? "fivecolum" : (count==5 ? "fivecolum" : ""))))%>">
                                                    <tr><th>&nbsp;</th></tr>
                                                    <tr><td><%# ShowFormatedData(DataBinder.Eval(Container.DataItem,"Displacement").ToString())%></td></tr>
                                                    <tr><td><%# ShowFormatedData(DataBinder.Eval(Container.DataItem,"Cylinders").ToString())%></td></tr>
                                                    <tr><td><%# ShowFormatedData(DataBinder.Eval(Container.DataItem,"MaxPower").ToString()) != "--" ? DataBinder.Eval(Container.DataItem,"MaxPower").ToString() + " bhp " + ( ShowFormatedData(DataBinder.Eval(Container.DataItem,"MaxPowerRpm").ToString()) != "--" ? " @ " + DataBinder.Eval(Container.DataItem,"MaxPowerRpm").ToString()  + " rpm " : "" ): "--" %></td></tr>
                                                    <tr><td><%# ShowFormatedData(DataBinder.Eval(Container.DataItem,"MaximumTorque").ToString()) != "--" ? DataBinder.Eval(Container.DataItem,"MaximumTorque").ToString() + " Nm  " + (ShowFormatedData(DataBinder.Eval(Container.DataItem,"MaximumTorqueRpm").ToString()) != "--" ? " @ " + DataBinder.Eval(Container.DataItem,"MaximumTorqueRpm").ToString()  + " rpm " : "") : "--" %></td></tr>
                                                    <tr><td><%# ShowFormatedData(DataBinder.Eval(Container.DataItem,"Bore").ToString())%></td></tr>
                                                    <tr><td><%# ShowFormatedData(DataBinder.Eval(Container.DataItem,"Stroke").ToString())%></td></tr>
                                                    <tr><td><%# ShowFormatedData(DataBinder.Eval(Container.DataItem,"ValvesPerCylinder").ToString())%></td></tr>
                                                    <tr><%--<td class="info-td"><span class="info-shrt-data"><%# ShowFormatedData(DataBinder.Eval(Container.DataItem,"FuelDeliverySystem").ToString())%></span><span class="info-popup"><%# ShowFormatedData(DataBinder.Eval(Container.DataItem,"FuelDeliverySystem").ToString())%></span></td>--%>
                                                         <%# (!String.IsNullOrEmpty(Convert.ToString(DataBinder.Eval(Container.DataItem, "FuelDeliverySystem"))) && (Convert.ToString(DataBinder.Eval(Container.DataItem, "FuelDeliverySystem")).Length > trSize) )
                                                        ?  String.Format("<td class='info-td'><span class='info-shrt-data'>{0}...</span><span class='info-popup'>{1}</span></td>",ShowFormatedData(Convert.ToString(DataBinder.Eval(Container.DataItem, "FuelDeliverySystem")).Truncate(trSize)),ShowFormatedData(Convert.ToString(DataBinder.Eval(Container.DataItem, "FuelDeliverySystem"))))                                                            
                                                        : String.Format("<td>{0}</td>",ShowFormatedData(Convert.ToString(DataBinder.Eval(Container.DataItem, "FuelDeliverySystem"))))
                                                        %>
                                                    </tr>
                                                    <tr><td><%# ShowFormatedData(DataBinder.Eval(Container.DataItem,"FuelType").ToString())%></td></tr>
                                                    <tr><td><%# ShowFormatedData(DataBinder.Eval(Container.DataItem,"Ignition").ToString())%></td></tr>
                                                    <tr><td><%# ShowFormatedData(DataBinder.Eval(Container.DataItem,"SparkPlugsPerCylinder").ToString())%></td></tr>
                                                    <tr><td><%# ShowFormatedData(DataBinder.Eval(Container.DataItem,"CoolingSystem").ToString())%></td></tr>
                                                    <tr><th>&nbsp;</th></tr>
                                                    <tr><td><%# ShowFormatedData(DataBinder.Eval(Container.DataItem,"GearboxType").ToString())%></td></tr>
                                                    <tr><td><%# ShowFormatedData(DataBinder.Eval(Container.DataItem,"NoOfGears").ToString())%></td></tr>
                                                    <tr><td><%# ShowFormatedData(DataBinder.Eval(Container.DataItem,"TransmissionType").ToString())%></td></tr>
                                                    <tr>
                                                         <%# (!String.IsNullOrEmpty(Convert.ToString(DataBinder.Eval(Container.DataItem, "Clutch"))) && (Convert.ToString(DataBinder.Eval(Container.DataItem, "Clutch")).Length > trSize) )
                                                        ?  String.Format("<td class='info-td'><span class='info-shrt-data'>{0}...</span><span class='info-popup'>{1}</span></td>",ShowFormatedData(Convert.ToString(DataBinder.Eval(Container.DataItem, "Clutch")).Truncate(trSize)),ShowFormatedData(Convert.ToString(DataBinder.Eval(Container.DataItem, "Clutch"))))                                                            
                                                        : String.Format("<td>{0}</td>",ShowFormatedData(Convert.ToString(DataBinder.Eval(Container.DataItem, "Clutch"))))
                                                        %>
                                                    </tr>
                                                    <tr><th>&nbsp;</th></tr>
                                                    <tr><td><%# ShowFormatedData(DataBinder.Eval(Container.DataItem,"Performance_0_60_kmph").ToString())%></td></tr>
                                                    <tr><td><%# ShowFormatedData(DataBinder.Eval(Container.DataItem,"Performance_0_80_kmph").ToString())%></td></tr>
                                                    <tr><td><%# ShowFormatedData(DataBinder.Eval(Container.DataItem,"Performance_0_40_m").ToString())%></td></tr>
                                                    <tr><td><%# ShowFormatedData(DataBinder.Eval(Container.DataItem,"TopSpeed").ToString())%></td></tr>
                                                    <tr><td><%# ShowFormatedData(DataBinder.Eval(Container.DataItem,"Performance_60_0_kmph").ToString())%></td></tr>
                                                    <tr><td><%# ShowFormatedData(DataBinder.Eval(Container.DataItem,"Performance_80_0_kmph").ToString())%></td></tr>
                                                    <tr><th>&nbsp;</th></tr>
                                                    <tr><td><%# ShowFormatedData(DataBinder.Eval(Container.DataItem,"KerbWeight").ToString())%></td></tr>
                                                    <tr><td><%# ShowFormatedData(DataBinder.Eval(Container.DataItem,"OverallLength").ToString())%></td></tr>
                                                    <tr><td><%# ShowFormatedData(DataBinder.Eval(Container.DataItem,"OverallWidth").ToString())%></td></tr>
                                                    <tr><td><%# ShowFormatedData(DataBinder.Eval(Container.DataItem,"OverallHeight").ToString())%></td></tr>
                                                    <tr><td><%# ShowFormatedData(DataBinder.Eval(Container.DataItem,"Wheelbase").ToString())%></td></tr>
                                                    <tr><td><%# ShowFormatedData(DataBinder.Eval(Container.DataItem,"GroundClearance").ToString())%></td></tr>
                                                    <tr><td><%# ShowFormatedData(DataBinder.Eval(Container.DataItem,"SeatHeight").ToString())%></td></tr>
                                                    <tr><th>&nbsp;</th></tr>
                                                    <tr><td><%# ShowFormatedData(DataBinder.Eval(Container.DataItem,"FuelTankCapacity").ToString())%></td></tr>
                                                    <tr><td><%# ShowFormatedData(DataBinder.Eval(Container.DataItem,"ReserveFuelCapacity").ToString())%></td></tr>
                                                    <tr><td><%# ShowFormatedData(DataBinder.Eval(Container.DataItem,"FuelEfficiencyOverall").ToString())%></td></tr>
                                                    <tr><td><%# ShowFormatedData(DataBinder.Eval(Container.DataItem,"FuelEfficiencyRange").ToString())%></td></tr>
                                                    <tr><th>&nbsp;</th></tr>
                                                    <tr><%--<td class="info-td"><span class="info-shrt-data"><%# ShowFormatedData(DataBinder.Eval(Container.DataItem,"ChassisType").ToString())%></span><span class="info-popup"><%# ShowFormatedData(DataBinder.Eval(Container.DataItem,"ChassisType").ToString())%></span></td>--%>
                                                        
                                                        <%# (!String.IsNullOrEmpty(Convert.ToString(DataBinder.Eval(Container.DataItem, "ChassisType"))) && (Convert.ToString(DataBinder.Eval(Container.DataItem, "ChassisType")).Length > trSize) )
                                                        ?  String.Format("<td class='info-td'><span class='info-shrt-data'>{0}...</span><span class='info-popup'>{1}</span></td>",ShowFormatedData(Convert.ToString(DataBinder.Eval(Container.DataItem, "ChassisType")).Truncate(trSize)),ShowFormatedData(Convert.ToString(DataBinder.Eval(Container.DataItem, "ChassisType"))))                                                            
                                                        : String.Format("<td>{0}</td>",ShowFormatedData(Convert.ToString(DataBinder.Eval(Container.DataItem, "ChassisType"))))
                                                        %>
                                                    </tr>
                                                    <tr>
                                                        <%# (!String.IsNullOrEmpty(Convert.ToString(DataBinder.Eval(Container.DataItem, "FrontSuspension"))) && (Convert.ToString(DataBinder.Eval(Container.DataItem, "FrontSuspension")).Length > trSize) )
                                                        ?  String.Format("<td class='info-td'><span class='info-shrt-data'>{0}...</span><span class='info-popup'>{1}</span></td>",ShowFormatedData(Convert.ToString(DataBinder.Eval(Container.DataItem, "FrontSuspension")).Truncate(trSize)),ShowFormatedData(Convert.ToString(DataBinder.Eval(Container.DataItem, "FrontSuspension"))))                                                            
                                                        : String.Format("<td>{0}</td>",ShowFormatedData(Convert.ToString(DataBinder.Eval(Container.DataItem, "FrontSuspension"))))
                                                        %>
                                                    </tr>
                                                    <tr><%--<td class="info-td"><span class="info-shrt-data"><%# ShowFormatedData(DataBinder.Eval(Container.DataItem,"RearSuspension").ToString())%></span><span class="info-popup"><%# ShowFormatedData(DataBinder.Eval(Container.DataItem,"RearSuspension").ToString())%></span></td>--%>
                                                         
                                                        <%# (!String.IsNullOrEmpty(Convert.ToString(DataBinder.Eval(Container.DataItem, "RearSuspension"))) && (Convert.ToString(DataBinder.Eval(Container.DataItem, "RearSuspension")).Length > trSize) )
                                                        ?  String.Format("<td class='info-td'><span class='info-shrt-data'>{0}...</span><span class='info-popup'>{1}</span></td>",ShowFormatedData(Convert.ToString(DataBinder.Eval(Container.DataItem, "RearSuspension")).Truncate(trSize)),ShowFormatedData(Convert.ToString(DataBinder.Eval(Container.DataItem, "RearSuspension"))))                                                            
                                                        : String.Format("<td>{0}</td>",ShowFormatedData(Convert.ToString(DataBinder.Eval(Container.DataItem, "RearSuspension"))))
                                                        %>
                                                    </tr>
                                                    <tr><th>&nbsp;</th></tr>
                                                    <tr><td><%# ShowFormatedData(DataBinder.Eval(Container.DataItem,"BrakeType").ToString())%></td></tr>
                                                    <tr><td><%# ShowFeature(DataBinder.Eval(Container.DataItem,"FrontDisc").ToString())%></td></tr>
                                                    <tr><td><%# ShowFormatedData(DataBinder.Eval(Container.DataItem,"FrontDisc_DrumSize").ToString())%></td></tr>
                                                    <tr><td><%# ShowFeature(DataBinder.Eval(Container.DataItem,"RearDisc").ToString())%></td></tr>
                                                    <tr><td><%# ShowFormatedData(DataBinder.Eval(Container.DataItem,"RearDisc_DrumSize").ToString())%></td></tr>
                                                    <tr><%--<td><%# ShowFormatedData(DataBinder.Eval(Container.DataItem,"CalliperType").ToString())%></td>--%>
                                                        <%# (!String.IsNullOrEmpty(Convert.ToString(DataBinder.Eval(Container.DataItem, "CalliperType"))) && (Convert.ToString(DataBinder.Eval(Container.DataItem, "CalliperType")).Length > trSize) )
                                                        ?  String.Format("<td class='info-td'><span class='info-shrt-data'>{0}...</span><span class='info-popup'>{1}</span></td>",ShowFormatedData(Convert.ToString(DataBinder.Eval(Container.DataItem, "CalliperType")).Truncate(trSize)),ShowFormatedData(Convert.ToString(DataBinder.Eval(Container.DataItem, "CalliperType"))))                                                            
                                                        : String.Format("<td>{0}</td>",ShowFormatedData(Convert.ToString(DataBinder.Eval(Container.DataItem, "CalliperType"))))
                                                        %>
                                                    </tr>
                                                    <tr><th>&nbsp;</th></tr>
                                                    <tr><td><%# ShowFormatedData(DataBinder.Eval(Container.DataItem,"WheelSize").ToString())%></td></tr>
                                                    <tr><td><%# ShowFormatedData(DataBinder.Eval(Container.DataItem,"FrontTyre").ToString())%></td></tr>
                                                    <tr><td><%# ShowFormatedData(DataBinder.Eval(Container.DataItem,"RearTyre").ToString())%></td></tr>
                                                    <tr><td><%# ShowFeature(DataBinder.Eval(Container.DataItem,"TubelessTyres").ToString())%></td></tr>
                                                    <tr><td><%# ShowFeature(DataBinder.Eval(Container.DataItem,"RadialTyres").ToString())%></td></tr>
                                                    <tr><td><%# ShowFeature(DataBinder.Eval(Container.DataItem,"AlloyWheels").ToString())%></td></tr>
                                                    <tr><th>&nbsp;</th></tr>
                                                    <tr><td><%# ShowFormatedData(DataBinder.Eval(Container.DataItem,"ElectricSystem").ToString())%></td></tr>
                                                    <tr><td><%# ShowFormatedData(DataBinder.Eval(Container.DataItem,"Battery").ToString())%></td></tr>
                                                    <tr><td><%# ShowFormatedData(DataBinder.Eval(Container.DataItem,"HeadlightType").ToString())%></td></tr>
                                                    <tr><td><%# ShowFormatedData(DataBinder.Eval(Container.DataItem,"HeadlightBulbType").ToString())%></td></tr>
                                                    <tr><td><%# ShowFormatedData(DataBinder.Eval(Container.DataItem,"Brake_Tail_Light").ToString())%></td></tr>
                                                    <tr><td><%# ShowFormatedData(DataBinder.Eval(Container.DataItem,"TurnSignal").ToString())%></td></tr>
                                                    <tr><td><%# ShowFeature(DataBinder.Eval(Container.DataItem,"PassLight").ToString())%></td></tr>
                                                </table>
                                            </td>                                       
                                        </ItemTemplate>
                                    </asp:repeater>
                                <% if (isFeatured ? (count < 5) : (count < 4))
                                   { %>
                                <td>
                                    <table cellpadding="0" border="0" cellspacing="0" class="<%= !isFeatured ? ((count==2) ? "threecolum": (count==3 ? "fourcolum" : (count==4 ? "fourcolum" : ""))) : ((count==2) ? "threecolum": (count==3 ? "fourcolum" : (count==4 ? "fivecolum" : (count==5 ? "fivecolum" : ""))))%>">
                                        <tr>
                                            <th>&nbsp;</th>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <th>&nbsp;</th>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <th>&nbsp;</th>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <th>&nbsp;</th>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <th>&nbsp;</th>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <th>&nbsp;</th>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <th>&nbsp;</th>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <th>&nbsp;</th>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <th>&nbsp;</th>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                    </table>
                                </td>
                                <% } %>
                            </tr>
                            <tr class="Features hide">
                                <td class="specs-title">
                                    <table width="165" cellpadding="0" border="0" cellspacing="0">
                                        <tr>
                                            <th>Features</th>
                                        </tr>
                                        <tr>
                                            <td class="headerSpecs">Speedometer</td>
                                        </tr>
                                        <tr>
                                            <td class="headerSpecs">Tachometer</td>
                                        </tr>
                                        <tr>
                                            <td class="headerSpecs">Tachometer Type</td>
                                        </tr>
                                        <tr>
                                            <td class="headerSpecs">Shift Light</td>
                                        </tr>
                                        <tr>
                                            <td class="headerSpecs">Electric Start</td>
                                        </tr>
                                        <tr>
                                            <td class="headerSpecs">Tripmeter</td>
                                        </tr>
                                        <tr>
                                            <td class="headerSpecs">No Of Tripmeters</td>
                                        </tr>
                                        <tr>
                                            <td class="headerSpecs">Tripmeter Type</td>
                                        </tr>
                                        <tr>
                                            <td class="headerSpecs">Low Fuel Indicator</td>
                                        </tr>
                                        <tr>
                                            <td class="headerSpecs">Low Oil Indicator</td>
                                        </tr>
                                        <tr>
                                            <td class="headerSpecs">Low Battery Indicator</td>
                                        </tr>
                                        <tr>
                                            <td class="headerSpecs">Fuel Gauge</td>
                                        </tr>
                                        <tr>
                                            <td class="headerSpecs">Digital Fuel Gauges</td>
                                        </tr>
                                        <tr>
                                            <td class="headerSpecs">Pillion Seat</td>
                                        </tr>
                                        <tr>
                                            <td class="headerSpecs">Pillion Footrest</td>
                                        </tr>
                                        <tr>
                                            <td class="headerSpecs">Pillion Backrest</td>
                                        </tr>
                                        <tr>
                                            <td class="headerSpecs">Pillion Grabrail</td>
                                        </tr>
                                        <tr>
                                            <td class="headerSpecs">Stand Alarm</td>
                                        </tr>
                                        <tr>
                                            <td class="headerSpecs">Stepped Seat</td>
                                        </tr>
                                        <tr>
                                            <td class="headerSpecs">Antilock Braking System</td>
                                        </tr>
                                        <tr>
                                            <td class="headerSpecs">Killswitch</td>
                                        </tr>
                                        <tr>
                                            <td class="headerSpecs">Clock</td>
                                        </tr>
                                        <tr>
                                            <td class="headerSpecs">Colours</td>
                                        </tr>
                                    </table>
                                </td>
                                <asp:repeater runat="server" id="rptFeatures">
                                        <itemtemplate>
                                            <td class="repeater-1 <%# Container.ItemIndex == featuredBikeIndex ? "featuredBike" : ""  %>">
                                                <table cellpadding="0" border="0" cellspacing="0" class="<%=!isFeatured ? ((count==2) ? "threecolum": (count==3 ? "fourcolum" : (count==4 ? "fourcolum" : ""))) : ((count==2) ? "threecolum": (count==3 ? "fourcolum" : (count==4 ? "fivecolum" : (count==5 ? "fivecolum" : ""))))%>">
                                                    <tr><th>&nbsp;</th></tr>
                                                    <tr><td><%# ShowFormatedData(DataBinder.Eval(Container.DataItem,"Speedometer").ToString())%></td></tr>
                                                    <tr><td><%# ShowFeature(DataBinder.Eval(Container.DataItem,"Tachometer").ToString())%></td></tr>
                                                    <tr><td><%# ShowFormatedData(DataBinder.Eval(Container.DataItem,"TachometerType").ToString())%></td></tr>
                                                    <tr><td><%# ShowFeature(DataBinder.Eval(Container.DataItem,"ShiftLight").ToString())%></td></tr>
                                                    <tr><td><%# ShowFeature(DataBinder.Eval(Container.DataItem,"ElectricStart").ToString())%></td></tr>
                                                    <tr><td><%# ShowFeature(DataBinder.Eval(Container.DataItem,"Tripmeter").ToString())%></td></tr>
                                                    <tr><td><%# ShowFormatedData(DataBinder.Eval(Container.DataItem,"NoOfTripmeters").ToString())%></td></tr>
                                                    <tr><td><%# ShowFormatedData(DataBinder.Eval(Container.DataItem,"TripmeterType").ToString())%></td></tr>
                                                    <tr><td><%# ShowFeature(DataBinder.Eval(Container.DataItem,"LowFuelIndicator").ToString())%></td></tr>
                                                    <tr><td><%# ShowFeature(DataBinder.Eval(Container.DataItem,"LowOilIndicator").ToString())%></td></tr>
                                                    <tr><td><%# ShowFeature(DataBinder.Eval(Container.DataItem,"LowBatteryIndicator").ToString())%></td></tr>
                                                    <tr><td><%# ShowFeature(DataBinder.Eval(Container.DataItem,"FuelGauge").ToString())%></td></tr>
                                                    <tr><td><%# ShowFeature(DataBinder.Eval(Container.DataItem,"DigitalFuelGauge").ToString())%></td></tr>
                                                    <tr><td><%# ShowFeature(DataBinder.Eval(Container.DataItem,"PillionSeat").ToString())%></td></tr>
                                                    <tr><td><%# ShowFeature(DataBinder.Eval(Container.DataItem,"PillionFootrest").ToString())%></td></tr>
                                                    <tr><td><%# ShowFeature(DataBinder.Eval(Container.DataItem,"PillionBackrest").ToString())%></td></tr>
                                                    <tr><td><%# ShowFeature(DataBinder.Eval(Container.DataItem,"PillionGrabrail").ToString())%></td></tr>
                                                    <tr><td><%# ShowFeature(DataBinder.Eval(Container.DataItem,"StandAlarm").ToString())%></td></tr>
                                                    <tr><td><%# ShowFeature(DataBinder.Eval(Container.DataItem,"SteppedSeat").ToString())%></td></tr>
                                                    <tr><td><%# ShowFeature(DataBinder.Eval(Container.DataItem,"AntilockBrakingSystem").ToString())%></td></tr>
                                                    <tr><td><%# ShowFeature(DataBinder.Eval(Container.DataItem,"Killswitch").ToString())%></td></tr>
                                                    <tr><td><%# ShowFeature(DataBinder.Eval(Container.DataItem,"Clock").ToString())%></td></tr>
                                                    <tr>
                                                         <%# (!String.IsNullOrEmpty(Convert.ToString(DataBinder.Eval(Container.DataItem, "Colors"))) && (Convert.ToString(DataBinder.Eval(Container.DataItem, "Colors")).Length > trSize) )
                                                        ?  String.Format("<td class='info-td'><span class='info-shrt-data'>{0}...</span><span class='info-popup'>{1}</span></td>",ShowFormatedData(Convert.ToString(DataBinder.Eval(Container.DataItem, "Colors")).Truncate(trSize)),ShowFormatedData(Convert.ToString(DataBinder.Eval(Container.DataItem, "Colors"))))                                                            
                                                        : String.Format("<td>{0}</td>",ShowFormatedData(Convert.ToString(DataBinder.Eval(Container.DataItem, "Colors"))))
                                                        %>
                                                    </tr>
                                                    
                                                </table>
                                            </td>
                                        </itemtemplate>
                                    </asp:repeater>
                                <% if (isFeatured ? (count < 5) : (count < 4))
                                   { %>
                                <td>
                                    <table cellpadding="0" border="0" cellspacing="0" class="<%=!isFeatured ? ((count==2) ? "threecolum": (count==3 ? "fourcolum" : (count==4 ? "fourcolum" : ""))) : ((count==2) ? "threecolum": (count==3 ? "fourcolum" : (count==4 ? "fivecolum" : (count==5 ? "fivecolum" : ""))))%>">
                                        <tr>
                                            <th>&nbsp;</th>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                    </table>
                                </td>
                                <% } %>
                            </tr>
                            <tr class="Colors hide">
                                <td class="specs-title repeater-1">
                                    <table class="tblColor" width="165" cellpadding="0" border="0" cellspacing="0">
                                        <tr>
                                            <th>Available Colours</th>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                    </table>
                                </td>
                                <asp:repeater runat="server" id="rptColors">
                                        <itemtemplate>
                                            <td class="repeater-1 <%# Container.ItemIndex == featuredBikeIndex ? "featuredBike" : ""  %>">
                                                <table cellpadding="0" border="0" cellspacing="0" class="tblColor <%=!isFeatured ? ((count==2) ? "threecolum": (count==3 ? "fourcolum" : (count==4 ? "fourcolum" : ""))) : ((count==2) ? "threecolum": (count==3 ? "fourcolum" : (count==4 ? "fivecolum" : (count==5 ? "fivecolum" : ""))))%>">
                                                    <tr><th>&nbsp;</th></tr>
                                                    <tr><td><%# GetModelColors((DataBinder.Eval(Container.DataItem,"BikeVersionId").ToString()),Container.ItemIndex) %></td></tr>
                                                </table>
                                            </td>
                                        </itemtemplate>
                                    </asp:repeater>
                                <% if (isFeatured ? (count < 5) : (count < 4))
                                   { %>
                                <td class="repeater-1">
                                    <table cellpadding="0" border="0" cellspacing="0" class="tblColor <%=!isFeatured ? ((count==2) ? "threecolum": (count==3 ? "fourcolum" : (count==4 ? "fourcolum" : ""))) : ((count==2) ? "threecolum": (count==3 ? "fourcolum" : (count==4 ? "fivecolum" : (count==5 ? "fivecolum" : ""))))%>">
                                        <tr>
                                            <th>&nbsp;</th>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                    </table>
                                </td>
                                <% } %>
                            </tr>
                        
                     
                        </table>
                    </td>
                   
                       
                      <% if(isUsedBikePresent){ %>
                    
                     <table width="100%" class="tbl-compare" cellpadding="0" border="0" cellspacing="0">
                         <tr class="usedbikes">
                             <td width="165" align="left" class="mainth">You may also like</td>
                             <asp:repeater runat="server" id="rptUsedBikes">
                                 <ItemTemplate>
                                     <td class="<%# Container.ItemIndex == featuredBikeIndex ? "featuredBike" : ""  %>">
                                         <table class="<%=!isFeatured ? ((count==2) ? "threecolum": (count==3 ? "fourcolum" : (count==4 ? "fourcolum" : ""))) : ((count==2) ? "threecolum": (count==3 ? "fourcolum" : (count==4 ? "fivecolum" : (count==5 ? "fivecolum" : ""))))%>" cellpadding="0" border="0" cellspacing="0">
                                             <tr>
                                                 <td>
                                                     <p><%# CreateUsedBikeLink(Convert.ToUInt32(DataBinder.Eval(Container.DataItem,"bikeCount")),Convert.ToString(DataBinder.Eval(Container.DataItem,"make")),Convert.ToString(DataBinder.Eval(Container.DataItem,"MakeMaskingName")),Convert.ToString(DataBinder.Eval(Container.DataItem,"model")),Convert.ToString(DataBinder.Eval(Container.DataItem,"ModelMaskingName")),Bikewale.Common.CommonOpn.FormatPrice(Convert.ToString(DataBinder.Eval(Container.DataItem,"minPrice"))),Convert.ToString(DataBinder.Eval(Container.DataItem,"citymaskingname"))) %></p>
                                                 </td>
                                             </tr>
                                         </table>
                                     </td>
                                 </ItemTemplate>
                                 <FooterTemplate>
                                     <% if (isFeatured ? (count < 5) : (count < 4))
                                        {%>
                                     <td>
                                         <table class="<%=!isFeatured ? ((count==2) ? "threecolum": (count==3 ? "fourcolum" : (count==4 ? "fourcolum" : ""))) : ((count==2) ? "threecolum": (count==3 ? "fourcolum" : (count==4 ? "fivecolum" : (count==5 ? "fivecolum" : ""))))%>" cellpadding="0" border="0" cellspacing="0">
                                             <tbody>
                                                 <tr>
                                                     <td>
                                                         <p></p>
                                                     </td>
                                                 </tr>
                                             </tbody>
                                         </table>
                                     </td>
                                     <% } %>
                                 </FooterTemplate>
                             </asp:repeater>
                         </tr>
                     </table>
                    <% } %>
                </tr>
            </table>
            <div>
                <p class="text-bold padding-top10"><%= templateSummaryTitle %></p>
                <p class="padding-top10"><%= compareBikeText %></p>
            </div>
            <div class="margin-bottom30 similarbikes <%= (ctrlSimilarBikes.fetchedCount > 0) ? string.Empty : "hide" %>">
                <BW:SimilarBikes ID="ctrlSimilarBikes" runat="server" />
            <div class="clear"></div>
            </div>
        </div>
    </div>
    <!--    Left Container ends here -->
</div>
<div id="back-to-top" class="back-to-top"><a><span></span></a></div>

<script type="text/javascript">
    var versions = '<%= hashModels %>';
    var currentversions = '<%= hashVersions %>';
    $(document).ready(function () {
        var speed = 300;
        //input parameter : id of element, scroll up speed
        ScrollToTop("back-to-top", speed);

        if ($("#tblCompare").length > 0) {
            var featuredBikeIndex = '<%= featuredBikeIndex + 2 %>';
            $("#tblCompare tr td:nth-child(" + featuredBikeIndex + ")").css({ "background-color": "#FCF8D0" });
            $("#tblCompare tr:first td:nth-child(" + featuredBikeIndex + ")").html("Sponsored Bike").addClass("td-featured");
        }

        $("#liSpecs,#liFeatures, #liColors").click(function () {
            $("#liSpecs,#liFeatures, #liColors").removeClass("fbike-active-tab");
            $(this).addClass("fbike-active-tab");
            $(".Specs,.Features,.Colors").addClass("hide");
            $("." + $(this).attr('id').substring(2, $(this).attr('id').length)).removeClass("hide");
        });

        var featuredBike = $(".featuredBike").find('.bikeclose');
        $(featuredBike).html('<span>sponsored</span>');

        $("a.delBike").click(function () {
            var verId = $(this).attr("versionId");
            var basicUrl = (window.location.pathname).split('/')[2];
            var makeModelList = new Array();
            var VersionIdList = new Array();
            var newBasicUrl = "";
            var newQueryStr = "";
            var Count = 0;
            makeModelList = basicUrl.split("-vs-");
            VersionIdList = currentversions.split(',');

            for (var i = 0; i < makeModelList.length; i++) {
                if (verId != VersionIdList[i]) {
                    newBasicUrl += makeModelList[i] + "-vs-";
                    newQueryStr += "bike" + (Count + 1) + "=" + VersionIdList[i] + "&";
                    Count++;
                }
            }

            newBasicUrl = newBasicUrl.substring(0, newBasicUrl.lastIndexOf("-vs-"));
            newQueryStr = newQueryStr.substring(0, newQueryStr.length - 1);
            window.location = "/comparebikes/" + newBasicUrl + "/?" + newQueryStr+ "&source=" + <%= (int)Bikewale.Entities.Compare.CompareSources.Desktop_CompareBike_UserSelection %>;

        });
    });
</script>
<!-- #include file="/includes/footerInner.aspx" -->