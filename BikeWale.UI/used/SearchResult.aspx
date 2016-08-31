<%@ Page Language="C#" Trace="false" Inherits="Bikewale.Used.SearchResult" AutoEventWireup="false" %>
<%@ Register TagPrefix="RP" TagName="RepeaterPagerUsed" Src="~/Controls/RepeaterPagerUsed.ascx" %>
<%@ Import Namespace="Bikewale.Common" %>
<div>    
    <div class="margin-top10">
        <div id="res_msg" runat="server" visible="false" class="grey-bg content-block">
	        <h3>Oops! No bikes found.</h3>
	        <p>Try broadening your search criteria</p>
        </div>
        <RP:RepeaterPagerUsed Id="rpgListings" runat="server" ResultName="Bikes" ShowHeadersVisible="true">
            <asp:Repeater ID="rptStockListings" runat="server">
		        <%--<headertemplate>
		         <table border="0" class="tbl-search" id="rptListings" width="100%" cellspacing="0" cellpadding="0">
			        <tr class="dt_header">	
                        <td width="10px;">&nbsp;</td>
				        <td><a class="sortLink" rel="nofollow" href='<%# SortColumnBy("0", "1") %>'>Bike</a><%# GetSortImage("1") %></td>
				        <td><a class="sortLink" rel="nofollow" href='<%# SortColumnBy("0", "2") %>'>Price(Rs.)</a><%# GetSortImage("2") %></td>
				        <td><a class="sortLink" rel="nofollow" href='<%# SortColumnBy("0", "0") %>'>Year</a><%# GetSortImage("0") %></td>
				        <td><a class="sortLink" rel="nofollow" href='<%# SortColumnBy("0", "3") %>'>Kms</a><%# GetSortImage("3") %></td>
                        <td><a class="sortLink" rel="nofollow" href='<%# SortColumnBy("0", "5") %>'>City</a><%# GetSortImage("5") %></td>
				        <td><a class="sortLink" rel="nofollow" href='<%# SortColumnBy("0", "6") %>'>Updated</a> <%# GetSortImage("6") %></td>				    
			        </tr>
		        </headertemplate>--%>
                 <AlternatingItemTemplate>
                    <div class="grey-bg expended_row padding5">
                        <div class="padding-top10"><span class="right-float">Last updated : <%#  Convert.ToDateTime(DataBinder.Eval(Container.DataItem, "LastUpdated")).ToString("dd-MM-yyyy") %></span> <a href="/used/bikes-in-<%# DataBinder.Eval( Container.DataItem, "CityMaskingName" ).ToString() %>/<%# DataBinder.Eval( Container.DataItem, "MakeMaskingName" ).ToString() %>-<%# DataBinder.Eval( Container.DataItem, "ModelMaskingName" ).ToString() %>-<%# DataBinder.Eval( Container.DataItem, "ProfileId" ) %>/"><%# DataBinder.Eval(Container.DataItem, "MakeName")%>&nbsp;<%# DataBinder.Eval(Container.DataItem, "ModelName")%>&nbsp;<%# DataBinder.Eval(Container.DataItem, "VersionName")%></a><span> (Profile ID : <%# DataBinder.Eval(Container.DataItem, "ProfileId") %>)</span></div>    
                        <div style="width:10%;" class='thumb_img left-float margin-top10' align="center">
                            <div class='<%#Convert.ToInt32(DataBinder.Eval(Container.DataItem, "PhotoCount")) > 0 ? "" : "hide"%>'>
                                  <a class="href-grey" href='<%# String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "OriginalImagePath").ToString()) ? "" : "/used/bikes-in-" + DataBinder.Eval( Container.DataItem, "CityMaskingName" ).ToString() + "/" + DataBinder.Eval( Container.DataItem, "MakeMaskingName" ).ToString() + "-" + DataBinder.Eval( Container.DataItem, "ModelMaskingName" ).ToString()+ "-" + DataBinder.Eval( Container.DataItem, "ProfileId" ) + "/" %>' target="_blank">
                                <%--<img photoCount='<%#DataBinder.Eval(Container.DataItem, "PhotoCount") %>' profileId='<%# DataBinder.Eval(Container.DataItem, "ProfileId") %>' class="front_image" alt="Loading..." src="<%#String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "FrontImagePath").ToString())? "http://imgd1.aeplcdn.com/0x0/bw/static/design15/old-images/d/nobike.jpg"  : Bikewale.Common.ImagingFunctions.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "FrontImagePath").ToString(),DataBinder.Eval(Container.DataItem, "HostUrl").ToString()) %>" border="0" />--%>
                                      <img photoCount='<%#DataBinder.Eval(Container.DataItem, "PhotoCount") %>' profileId='<%# DataBinder.Eval(Container.DataItem, "ProfileId") %>' class="front_image" alt="Loading..." src="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImagePath").ToString(),DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._110x61) %>" border="0" />
                                <br /><%#Convert.ToInt32(DataBinder.Eval(Container.DataItem, "PhotoCount")) > 0 ?DataBinder.Eval(Container.DataItem, "PhotoCount") + " Photos" : ""%></a>
                            </div>
                            <div class='<%#Convert.ToInt32(DataBinder.Eval(Container.DataItem, "PhotoCount")) <= 0 ? "" : "hide"%>'>
                                <img photoCount='<%#DataBinder.Eval(Container.DataItem, "PhotoCount") %>' profileId='<%# DataBinder.Eval(Container.DataItem, "ProfileId") %>' class="front_image" alt="Loading..." src="http://imgd3.aeplcdn.com/110x61/bikewaleimg/images/noimage.png" border="0" />
                            </div>
                            <div><%# DataBinder.Eval(Container.DataItem, "Seller")%></div>
                        </div>
                        <div style="width:80%; display:block;" class="right-float margin-top10">
                            <table class="tbl_row" width="100%" cellspacing="0" border="0" cellpadding="0">
                                <tr>
                                    <th width="100px">Price</th>
                                    <td width="100px" class="price2" ><%# CommonOpn.FormatNumeric(Convert.ToString(DataBinder.Eval(Container.DataItem, "Price")))%></td>			
                                    <th width="100px">City</th>
                                    <td><%# DataBinder.Eval(Container.DataItem, "City")%></td>
                                </tr>
                                <tr>
                                    <th>Km</th>
                                    <td><%# CommonOpn.FormatNumeric( Convert.ToString(DataBinder.Eval(Container.DataItem, "Kilometers")) ) %></td>
                                    <th>Transmission</th>
                                    <td><%# GetTransmissionText(DataBinder.Eval(Container.DataItem, "BikeTransmission").ToString())%>, <%# GetFuelType(DataBinder.Eval(Container.DataItem, "BikeFuelType").ToString())%></td>
                                </tr>
                                <tr>
                                    <th>Model Year </th>
                                    <td><%# Convert.ToDateTime(DataBinder.Eval(Container.DataItem, "BikeYear")).ToString("yyyy") %></td>
                                    <th>Color</th>
                                    <td><%# DataBinder.Eval(Container.DataItem, "Color")%></td>
                                </tr>
                                <tr>
                                    <td ProfileId ='<%# DataBinder.Eval(Container.DataItem, "ProfileId") %>' ModelYear='<%# Convert.ToDateTime(DataBinder.Eval(Container.DataItem, "BikeYear")).ToString("yyyy") %>' sellerType='<%# DataBinder.Eval(Container.DataItem,"SellerType") %>' ModelName='<%# DataBinder.Eval(Container.DataItem, "ModelName")%>'><input id="btnShowinterst" value="Show Interest" type="button" class="buttons btnShowinterst" /></td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td><a target="_blank" href="/used/bikes-in-<%# DataBinder.Eval( Container.DataItem, "CityMaskingName" ).ToString() %>/<%# DataBinder.Eval( Container.DataItem, "MakeMaskingName" ).ToString() %>-<%# DataBinder.Eval( Container.DataItem, "ModelMaskingName" ).ToString() %>-<%# DataBinder.Eval( Container.DataItem, "ProfileId" ) %>/">&raquo; View complete details</a></td>
                                </tr>	          
                            </table>		
                        </div><div class="clear"></div>
                    </div>
                 </AlternatingItemTemplate>
		        <itemtemplate>
                 <%--  <tr id='<%# DataBinder.Eval(Container.DataItem, "ProfileId") %>' title="Click to view details" class='dt_body' style="height:58px;">
			           <td valign="top" style="padding:0; padding-top:5px;"><img align="right" class="hide margin-top5" alt="loading..." id="expandables" src="http://imgd1.aeplcdn.com/0x0/bw/static/design15/old-images/d/expandable.png" border="0" />&nbsp;<input type="hidden" id="imgUrl" value='<%# DataBinder.Eval(Container.DataItem, "FrontImagePath") %>' /><input type="hidden" id="host_url" value='<%# DataBinder.Eval(Container.DataItem, "HostUrl") %>' /></td>
			            <td>                            
                            <a id="bike" class="go_bike_profile" href="/used-bikes-in-<%# DataBinder.Eval( Container.DataItem, "CityMaskingName" ).ToString() %>/<%# DataBinder.Eval( Container.DataItem, "MakeMaskingName" ).ToString() %>-<%# DataBinder.Eval( Container.DataItem, "ModelMaskingName" ).ToString() %>-<%# DataBinder.Eval( Container.DataItem, "ProfileId" ) %>/"><%# DataBinder.Eval(Container.DataItem, "MakeName")%>&nbsp;<%# DataBinder.Eval(Container.DataItem, "ModelName")%>&nbsp;<%# DataBinder.Eval(Container.DataItem, "VersionName")%></a>                            
                            <p class="margin-top5">
                                <span>Seller : <%# DataBinder.Eval(Container.DataItem, "Seller")%></span>
                                <span class="margin-left5"><%# IsPhotosAvailable(DataBinder.Eval(Container.DataItem, "PhotoCount").ToString())%></span>
                            </p>
                            <span id="color" class="hide"><%# DataBinder.Eval(Container.DataItem, "Color")%></span>
                            <input type="hidden" id="photo_count" value="<%# DataBinder.Eval(Container.DataItem, "PhotoCount")%>" />
			            </td>
			            <td><%# CommonOpn.FormatNumeric(Convert.ToString(DataBinder.Eval(Container.DataItem, "Price")))%></td>
			            <td><span id="bike_year"><%# Convert.ToDateTime(DataBinder.Eval(Container.DataItem, "BikeYear")).ToString("yyyy") %></span><input type="hidden" id="make_year" value="<%# Convert.ToDateTime(DataBinder.Eval(Container.DataItem, "BikeYear")).ToString("MMM-yyyy") %>" /> <input type="hidden" id="model_name" value="<%# DataBinder.Eval(Container.DataItem, "ModelName")%>" /><input type="hidden" id="_fueltype" value='<%# DataBinder.Eval(Container.DataItem, "BikeFuelType")%>' /><input type="hidden" id="_transm" value='<%# DataBinder.Eval(Container.DataItem, "BikeTransmission")%>' /></td>
			            <td><%# CommonOpn.FormatNumeric( Convert.ToString(DataBinder.Eval(Container.DataItem, "Kilometers")) ) %></td>
                        <td><%# DataBinder.Eval(Container.DataItem, "City")%></td>
                        <td><%# Convert.ToDateTime(DataBinder.Eval(Container.DataItem, "LastUpdated")).ToString("dd-MM-yyyy") %></td>			                       
		            </tr>--%>
                    <div class="expended_row padding5">
                    <div class="padding-top10"><span id="last_update_row" class="right-float">Last updated : <%# Convert.ToDateTime(DataBinder.Eval(Container.DataItem, "LastUpdated")).ToString("dd-MM-yyyy") %></span> <a href="/used/bikes-in-<%# DataBinder.Eval( Container.DataItem, "CityMaskingName" ).ToString() %>/<%# DataBinder.Eval( Container.DataItem, "MakeMaskingName" ).ToString() %>-<%# DataBinder.Eval( Container.DataItem, "ModelMaskingName" ).ToString() %>-<%# DataBinder.Eval( Container.DataItem, "ProfileId" ) %>/"><%# DataBinder.Eval(Container.DataItem, "MakeName")%>&nbsp;<%# DataBinder.Eval(Container.DataItem, "ModelName")%>&nbsp;<%# DataBinder.Eval(Container.DataItem, "VersionName")%></a><span id="bike_row" class="price2" style="zoom:1;"></span>&nbsp;&nbsp;<span id="profileId_row">(Profile ID : <%# DataBinder.Eval(Container.DataItem, "ProfileId") %>)</span></div>    
                    <div style="width:10%;" class="thumb_img left-float margin-top10" align="center">
                        <div class='<%#Convert.ToInt32(DataBinder.Eval(Container.DataItem, "PhotoCount")) > 0 ? "" : "hide"%>'>
                            <a class="href-grey" href='<%# String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "OriginalImagePath").ToString()) ? "" : "/used/bikes-in-" + DataBinder.Eval( Container.DataItem, "CityMaskingName" ).ToString() + "/" + DataBinder.Eval( Container.DataItem, "MakeMaskingName" ).ToString() + "-" + DataBinder.Eval( Container.DataItem, "ModelMaskingName" ).ToString()+ "-" + DataBinder.Eval( Container.DataItem, "ProfileId" ) + "/" %>' target="_blank">
                            <%--<img photoCount='<%#DataBinder.Eval(Container.DataItem, "PhotoCount") %>' profileId='<%# DataBinder.Eval(Container.DataItem, "ProfileId") %>' class="front_image" alt="Loading..." src="<%#String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "FrontImagePath").ToString())? "http://imgd1.aeplcdn.com/0x0/bw/static/design15/old-images/d/nobike.jpg"  : Bikewale.Common.ImagingFunctions.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "FrontImagePath").ToString(),DataBinder.Eval(Container.DataItem, "HostUrl").ToString()) %>" border="0" />--%>
                                <img photoCount='<%#DataBinder.Eval(Container.DataItem, "PhotoCount") %>' profileId='<%# DataBinder.Eval(Container.DataItem, "ProfileId") %>' class="front_image" alt="Loading..." src="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImagePath").ToString(),DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._110x61) %>" border="0" />
                            <br /><%#Convert.ToInt32(DataBinder.Eval(Container.DataItem, "PhotoCount")) > 0 ?DataBinder.Eval(Container.DataItem, "PhotoCount") + " Photos" : ""%></a>
                        </div>
                        <div class='<%#Convert.ToInt32(DataBinder.Eval(Container.DataItem, "PhotoCount")) <= 0 ? "" : "hide"%>'>
                            <img photoCount='<%#DataBinder.Eval(Container.DataItem, "PhotoCount") %>' profileId='<%# DataBinder.Eval(Container.DataItem, "ProfileId") %>' class="front_image" alt="Loading..." src="http://imgd3.aeplcdn.com/110x61/bikewaleimg/images/noimage.png" border="0" />
                        </div>
                        <div><%# DataBinder.Eval(Container.DataItem, "Seller")%></div>
                    </div>
                        <div style="width:80%; display:block;" class="right-float margin-top10">
                            <table class="tbl_row" width="100%" cellspacing="0" border="0" cellpadding="0" id='<%# DataBinder.Eval(Container.DataItem, "ProfileId") %>'>
                                <tr>
                                    <th width="100px">Price</th>
                                    <td width="100px" class="price2"><%# CommonOpn.FormatNumeric(Convert.ToString(DataBinder.Eval(Container.DataItem, "Price")))%></td>			
                                    <th width="100px">City</th>
                                    <td><%# DataBinder.Eval(Container.DataItem, "City")%></td>
                                </tr>
                                <tr>
                                    <th>Km</th>
                                    <td><%# CommonOpn.FormatNumeric( Convert.ToString(DataBinder.Eval(Container.DataItem, "Kilometers")) ) %></td>
                                    <th>Transmission</th>
                                    <td><%# GetTransmissionText(DataBinder.Eval(Container.DataItem, "BikeTransmission").ToString())%>, <%# GetFuelType(DataBinder.Eval(Container.DataItem, "BikeFuelType").ToString())%></td>
                                </tr>
                                <tr>
                                    <th>Model Year </th>
                                    <td><%# Convert.ToDateTime(DataBinder.Eval(Container.DataItem, "BikeYear")).ToString("yyyy") %></td>
                                    <th>Color</th>
                                    <td><%# DataBinder.Eval(Container.DataItem, "Color")%></td>
                                </tr>
                                <tr>
                                    <td ProfileId ='<%# DataBinder.Eval(Container.DataItem, "ProfileId") %>' sellerType='<%# DataBinder.Eval(Container.DataItem,"SellerType") %>' ModelYear='<%# Convert.ToDateTime(DataBinder.Eval(Container.DataItem, "BikeYear")).ToString("yyyy") %>' ModelName='<%# DataBinder.Eval(Container.DataItem, "ModelName")%>' ><input id="btnShowinterst" value="Show Interest" type="button" class="buttons btnShowinterst" /></td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td><a href="/used/bikes-in-<%# DataBinder.Eval( Container.DataItem, "CityMaskingName" ).ToString() %>/<%# DataBinder.Eval( Container.DataItem, "MakeMaskingName" ).ToString() %>-<%# DataBinder.Eval( Container.DataItem, "ModelMaskingName" ).ToString() %>-<%# DataBinder.Eval( Container.DataItem, "ProfileId" ) %>/" target="_blank">&raquo; View complete details</a></td>
                                </tr>	          
                            </table>		
                        </div><div class="clear"></div>
                    </div>
		        </itemtemplate>        
		   <%--     <footertemplate>
		        </table>
		        </footertemplate>--%>
	        </asp:Repeater>
        </RP:RepeaterPagerUsed>
    </div>  
</div>
<script type="text/javascript">
 
    $(document).ready(function () {
        $(".thumb_img").click(function () {

        });
        initGetSellerDetails();
        onSortOrderChange();
        //$("#drpCity").change(function () {
        //    $("#drpCityDist").attr("disabled", false);
        //    $("#drpCityDist").val("50").attr("selected", true);
        //});
        var sortValue = '<%= sortValue%>';

        $("#ddlSort").val(sortValue);
    });
    
</script>
    