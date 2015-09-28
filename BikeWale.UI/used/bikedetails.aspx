<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Used.BikeDetails" Trace="false" Debug="false" %>
<%@ Import Namespace="Bikewale.Common" %>
<%
    title = "Used " + objInquiry.ModelYearOnly + " " + objInquiry.BikeName + " (" + profileId.ToUpper() + ") for sale in " + objInquiry.CityName;
    description = "BikeWale - Used " + objInquiry.BikeName + " for sale in " + objInquiry.CityName + ". The bikes is of " + objInquiry.ModelYearOnly + " model year and its profile id is #" + profileId.ToUpper() + ". Get phone number of the seller and call directly to inspect and test drive the bike.";
    keywords = "used " + objInquiry.BikeName + ", used " + objInquiry.BikeName + " for sale, used " + objInquiry.BikeName + " in " + objInquiry.CityName;
    canonical = bikeCanonical;
    AdId = "1395992162974";
    AdPath = "/1017752/BikeWale_UsedBikes_HomePage_";
%>
<!-- #include file="/includes/headUsed.aspx" -->
<script type="text/javascript" src="/src/common/bt.js?v1.1"></script>
<link rel="stylesheet" type="text/css" href="/css/used-cd.css" />
<script type="text/javascript" src="/src/classified/bikedetails.js?<%= staticFileVersion %>"></script>
<style type="text/css">
    .feature-list li { float: left; width: 170px; }
    .cd-tbl th { font-weight:bold; }
    #reqPhotos.buttons { background: #f5f5f5; color: #82888b; border: 1px solid #ccc; font-size:14px;}
    #reqPhotos.buttons:hover { background: #82888b; color: #fff; text-decoration: none; border:1px solid #82888b; }
    #buyer_form input, #verifiy_mobile input { border:1px solid #ccc; padding:5px; }
    #colorbox { width:720px !important; height:570px !important; }
</style>
<script type="text/javascript">
    var bikeName = '<%= objInquiry.BikeName %>';
    var re = /^[0-9]*$/;
    var inquiryId = '<%= sellInqId %>';
    var profileId = '<%= profileId %>';
    var _isDealer = '<%= isDealer ? "1" : "0" %>';
    var consumerType = _isDealer == "1" ? "1" : "2";
    var actualPhoto = "0";
    var sellerContact = "";
    var sellerName = "";
    var bikeModel = '<%= objInquiry.ModelName %>';
    var makeYear = "";
    var isDetailsViewed = "0";
    var custId = '<%= CurrentUser.Id %>';

    var price = '<%=objInquiry.AskingPrice%>';
    var year = '<%=Convert.ToDateTime(objInquiry.ModelYear).Year.ToString()%>';
    var kms = '<%=objInquiry.Kms%>';

    var buyersName = "";
    var buyersEmail = ""
    var buyersMobile = "";
    var isBuyingReq = true, isPhotoReqDone = false;

    var compareCaption = "";
</script>
<div class="container_12">
    <div class="grid_12">
        <ul class="breadcrumb">
            <li>You are here: </li>
            <li><a href="/">Home</a></li>
            <li class="fwd-arrow">&rsaquo;</li>
            <li><a href="/used/">Used Bikes</a></li>
            <li class="fwd-arrow">&rsaquo;</li>
            <li><a href="/used/bikes-in-<%=objInquiry.CityMaskingName.ToLower().Replace(" ","") %>/#<%= GetBackToSearch() %>">Search Result</a></li>            
            <li class="fwd-arrow">&rsaquo;</li>
            <li class="current"><strong><%= objInquiry.BikeName %></strong></li>
        </ul><div class="clear"></div>
    </div>
    <div class="grid_8 margin-top10">       
        <h1>Used, <%= objInquiry.BikeName %></h1>
        <div class="grey-bg content-block margin-top15 border-light">
            <div class="text-highlight">For Sale at <%= objInquiry.AreaName == "" ? "" : objInquiry.AreaName + ", " %><%= objInquiry.CityName %>, <span title="<%= objInquiry.StateName %>"><%= objInquiry.StateCode %></span></div>
            <div class="margin-top10">
                <table class="cd-tbl" width="100%" cellspacing="0" cellpadding="0" border="0">
                    <tr>
                        <th width="140">Asking Price (Rs.)</th>
                        <td width="180" class="price2" style="color: #cc0000;"><%= objInquiry.AskingPrice %></td>
                        <th width="160">Kms Done</th>
                        <td><%= objInquiry.Kms %></td>
                    </tr>
                    <tr>
                        <th>Model Year</th>
                        <td><%= objInquiry.ModelYear %></td>
                        <th>Color</th>
                        <td style="padding: 0;"><%= GetColorCode() %></td>
                    </tr>
                    <tr>
                        <th>Registration</th>
                        <td><%= objInquiry.Registration %></td>
                        <th>Profile Id</th>
                        <td><%= profileId %></td>
                    </tr>
                    <tr>
                        <th>Owner</th>
                        <td><%= objInquiry.Owner %></td>
                        <th>Seller</th>
                        <td><%= objInquiry.Seller %></td>
                    </tr>
                    <tr>
                        <th>Insurance</th>
                        <td><%= objInquiry.Insurance %>
                            <br />
                            <span class="text-grey"><%= objInquiry.InsuranceExpiry %></span></td>
                        <th>Lifetime-Tax</th>
                        <td><%= objInquiry.LifetimeTax %></td>
                    </tr>
                    <tr>
                        <th valign="top">Engine</th>
                        <td><%= objInquiry.Engine %></td>
                        <th valign="top">Fuel Type</th>
                        <td valign="top">Petrol</td>
                    </tr>
                    <tr>
                        <td colspan="4" align="right" style="padding:0; padding-right:30px"><div class="margin-top10"><a id="contact-seller" class="action-btn">Get Seller Details</a></div></td>
                    </tr>
                </table>
            </div>
        </div>
        <div id="scrollable_imgs" runat="server" class="scrollable-img margin-top15">
            <div class="thumb_preview">
                <%--<a id="profile_link" rel="slide" title="<%= objPhotos.FrontImageDescription %>" href="<%= GetImagePath( objPhotos.FrontImageLarge, objPhotos.DirectoryPath, objPhotos.HostUrl ) %>">--%>
                <a id="profile_link" rel="slide" title="<%= objPhotos.FrontImageDescription %>" href="<%= GetOriginalImagePath( objPhotos.OriginalImagePath, objPhotos.HostUrl, Bikewale.Utility.ImageSize._642x361 ) %>">
                    <img id="inlarge" style="position: absolute; border: 0; z-index: 1001; margin-top: 1px; margin-left: 1px;" class="hide" alt="inlarge" src="http://img.aeplcdn.com/used/inlarge.gif" />
                    <%--<img id="front_img" border="0" alt="<%= objInquiry.ModelYearOnly + " " + objInquiry.BikeName %>" src="<%= GetImagePath( objPhotos.FrontImageMidThumb, objPhotos.DirectoryPath, objPhotos.HostUrl ) %>" /></a>--%>
                    <img id="front_img" border="0" alt="<%= objInquiry.ModelYearOnly + " " + objInquiry.BikeName %>" src="<%= GetOriginalImagePath( objPhotos.OriginalImagePath, objPhotos.HostUrl,Bikewale.Utility.ImageSize._310x174) %>" width="300" height="222" /></a>
            </div>
            <div class="img-desc hide">
                <img alt="loading..." title="click to inlarge this image" style="margin-right: 5px;" src="http://img.aeplcdn.com/sell/m01.gif" align="left" border="0" /><span id="img_description"><%= objPhotos.FrontImageDescription %></span><div class="clear"></div>
            </div>
            <div class="thumb_navi">
                <div id="navi" class="scrollable">
                    <div class="items">
                        <div>
                            <asp:repeater id="rptPhotos" runat="server">
							    <itemtemplate>
								    <%= GetPageItemContainer() %>
								    <%--<a class="img_thumb" href="<%# GetImagePath( DataBinder.Eval( Container.DataItem, "ImageUrlThumb" ).ToString(), DataBinder.Eval( Container.DataItem, "DirectoryPath" ).ToString(), DataBinder.Eval( Container.DataItem, "HostUrl" ).ToString() )%>"><img alt="loading.." title="click to view" src="<%# GetImagePath( DataBinder.Eval( Container.DataItem, "ImageUrlThumbSmall" ).ToString(), DataBinder.Eval( Container.DataItem, "DirectoryPath" ).ToString(), DataBinder.Eval( Container.DataItem, "HostUrl" ).ToString() )%>" border="0" /></a>--%>
                                    <a class="img_thumb" href="<%# GetOriginalImagePath( DataBinder.Eval( Container.DataItem, "OriginalImagePath" ).ToString(), DataBinder.Eval( Container.DataItem, "HostUrl" ).ToString(),Bikewale.Utility.ImageSize._310x174 )%>"><img alt="loading.." title="click to view" src="<%# GetOriginalImagePath( DataBinder.Eval( Container.DataItem, "OriginalImagePath" ).ToString(), DataBinder.Eval( Container.DataItem, "HostUrl" ).ToString(),Bikewale.Utility.ImageSize._110x61 )%>" border="0" width="80" height="59"/></a>
								    <%--<a rel="slide" name="front<%# DataBinder.Eval( Container.DataItem, "IsMain" ).ToString() == "True" ? "1" : "0"%>" class="aslide" title="<%# DataBinder.Eval( Container.DataItem, "Description" ) %>" href="<%# GetImagePath( DataBinder.Eval( Container.DataItem, "ImageUrlFull" ).ToString(), DataBinder.Eval( Container.DataItem, "DirectoryPath" ).ToString(), DataBinder.Eval( Container.DataItem, "HostUrl" ).ToString() )%>"></a>--%>
                                    <a rel="slide" name="front<%# DataBinder.Eval( Container.DataItem, "IsMain" ).ToString() == "True" ? "1" : "0"%>" class="aslide" title="<%# DataBinder.Eval( Container.DataItem, "Description" ) %>" href="<%# GetOriginalImagePath( DataBinder.Eval( Container.DataItem, "OriginalImagePath" ).ToString(), DataBinder.Eval( Container.DataItem, "HostUrl" ).ToString(),Bikewale.Utility.ImageSize._642x361 )%>"></a>
							    </itemtemplate>
						    </asp:repeater>
                        </div>
                    </div>
                </div>
                <div class="clear"></div>
                <div id="navigate_img" runat="server" class="scrollable-navi">
                    <div class="navi"></div>
                    <a class="prev browse left"></a><a class="next browse right"></a>
                    <div class="clear"></div>
                </div>
                <script type="text/javascript">
                    $("#navi").scrollable().navigator();
                    $("a[rel='slide']").colorbox({ width: "700px", height: "550px" });
                    $("a[name='front1']").attr("rel", "nofollow");
                    $("a.img_thumb").click(function (e) {
                        e.preventDefault();
                        var imgDesc = $(this).next().attr("title");

                        $("#profile_link").attr("href", $(this).next().attr("href")).attr("title", imgDesc != "" ? imgDesc : "Actual bike photos");
                        $("#front_img").hide().attr("src", $(this).attr("href")).fadeIn(700);
                        $("#img_description").text(imgDesc != "" ? imgDesc : "Actual bike photos");

                        $("a.aslide").attr("rel", "slide");
                        $(this).next().attr("rel", "nofollow");
                        $("a[rel='slide']").colorbox({ width: "700px", height: "550px" });
                    });
                </script>
            </div>
            <div class="clear"></div>
        </div>
        
        <div id="requestPhotos" runat="server" visible="false" class="grey-bg content-block margin-top15">Photos of this bike not uploaded by the seller. <a id="reqPhotos" class="buttons margin-left10">Request seller to upload photos</a></div>
        
        <div class="margin-top15">
            <h2 id="std_features" class="<%=  objInquiry.Parse_Features() == "<ul class='ul-tick-chk'></ul>" ? "hide" : ""  %>">Standard Features</h2>
            <div class="margin-top10">
                <div class="feature-list">
                    <%= objInquiry.Parse_Features()%>
                </div>
                <div class="clear"></div>
                <div class="hr-dotted"></div>
            </div>            
        </div>

        <div id="salersNote" runat="server" class="margin-top15">
            <h2>Seller's Note</h2>
            <div class="margin-top10"><%= objInquiry.CustomersNote %></div>            
        </div>
        <div  id="addDetails" class="margin-top15" runat="server">
            <h2>Additional Details</h2>
            <p class='<%= objInquiry.Warranties == "--" ? "hide" : "margin-top10" %>' ><span class="price2">Warranties:</span> <%= objInquiry.Warranties %></p>
            <p class='<%= objInquiry.Modifications == "--" ? "hide" : "margin-top10" %>' ><span class="price2">Modifications:</span> <%= objInquiry.Modifications %></p>
        </div>
        <div class="grey-bg content-block margin-top15 <%=  objInquiry.Parse_Features() == "<ul class='ul-tick-chk'></ul>" ? "hide" : "" %>">
            View <%= objInquiry.BikeName %> <a title="<%= objInquiry.BikeName %> Standard Specifications" href="<%= researchBaseUrl%>">Specifications</a>
        </div>
        <div id="contact" class="hide">
            <a id="closeBox" class="gb-close right-float"></a>
            <div id="buyer_form" style="height: 260px;">
                <h2 id="form_title">Interested? Get Seller Details</h2>
                <p id="byline_text" class="margin-top10">For privacy concerns, We hide owner details. Please fill this form to get owner's details.</p>
                <table class="tbl-default margin-top5" width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td>Your Name</td>
                        <td>
                            <input type="text" id="txtName" size="27" class="text" /></td>
                    </tr>
                    <tr>
                        <td>Email</td>
                        <td>
                            <input type="text" id="txtEmail" size="27" class="text" /></td>
                    </tr>
                    <tr>
                        <td>Mobile Number</td>
                        <td>
                            <input type="text" id="txtMobile" maxlength="10" class="text" /></td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td><a id="get_details" class="buttons">Get Details</a><div id="process_img" class="process-inline"></div></td>                        
                    </tr>
                </table>
            </div>
            <div id="verifiy_mobile" class="hide">
                <h2>One-time Mobile Verification</h2>
                <p class="margin-top10">We have just sent you an SMS with a 5-digit verification code on your mobile number. Please enter the verification code below to proceed.</p>
                <div class="margin-top10">
                    <img align="absmiddle" class="redirect-lt" src="http://img.aeplcdn.com/sell/mobi-verif.gif" border="0" />
                    <input type="text" id="txtCwiCode" maxlength="5" style="margin-left: 10px;" value="Enter your code here" onfocus="javascript:if(this.value == 'Enter your code here') { this.value=''; }" onblur="javascript:if(this.value == '') { this.value='Enter your code here'; }" />
                    <a id="btnVerifyCode" class="buttons redirect-lt">Verify</a><div id="processCode" class="process-inline"></div>
                </div>
            </div>
            <div id="seller_details" class="hide">
                <h2>Seller Details</h2>
                <p class="margin-top10">We have sent these details through SMS</p>               
                <table class="tbl-default margin-top10" width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <th width="120">Seller Name</th>
                        <td><span id="seller_name"></span></td>
                    </tr>
                    <tr>
                        <th>Contact Person</th>
                        <td><span id="contact_person"></span></td>
                    </tr>
                    <tr>
                        <th>Email</th>
                        <td><span id="seller_email"></span></td>
                    </tr>
                    <tr>
                        <th>Mobile Number</th>
                        <td><span id="seller_mobile"></span></td>
                    </tr>
                    <tr>
                        <th valign="top">Address</th>
                        <td><span id="seller_address"></span></td>
                    </tr>
                </table>              
            </div>
            <div id="photos_req_msg" class="hide">
                <h2>It's Done!</h2>
                <div id="photos_req_confirm" class="margin-top15">
                    Your request to the seller submitted successfully. We will inform you as seller updates photos of this bike.
                </div>
            </div>
            <div id="not_auth" class="alert margin-top20 hide">Oops! You have reached the maximum limit for viewing inquiry details in a day.</div>
            <div id="initWait" class="process-inline" style="padding-left: 20px;">Loading...</div>
        </div>
        <script language="javascript">
            $("#contact-seller").bt({           
                contentSelector: "$('#contact').html()", fill: '#ffffff', strokeWidth: 1, strokeStyle: '#D3D3D3', trigger: ['click', 'none'], width: '400px', spikeLength: 7, shadow: true, positions: ['left', 'right', 'bottom'],
                preShow: function (box) {
                    $("div.bt-wrapper").hide();
                }, showTip: function (box) {
                    boxObj = $(box);
                    boxObj.show();
                    isBuyingReq = $(this).attr("id") == "reqPhotos" ? false : true;
                    initBT_Box(boxObj);
                }
            });
            $("#reqPhotos").bt({
                contentSelector: "$('#contact').html()", fill: '#ffffff', strokeWidth: 1, strokeStyle: '#D3D3D3', trigger: ['click', 'none'], width: '330px', spikeLength: 7, shadow: true, positions: ['top', 'bottom'],
                preShow: function (box) {
                    $("div.bt-wrapper").hide();
                }, showTip: function (box) {
                    boxObj = $(box);
                    boxObj.show();
                    isBuyingReq = $(this).attr("id") == "reqPhotos" ? false : true;
                    initBT_Box(boxObj);
                }
            });
        </script>
    </div>
    <!--    Left Container ends here -->
    <div class="grid_4">
        <div class="margin-top15">
            <!-- BikeWale_UsedBike/BikeWale_UsedBike_300x250 -->
            <!-- #include file="/ads/Ad300x250.aspx" -->
        </div>               
        <div id="other_models" runat="server" class="margin-top15">
            <h2>More <%= objInquiry.MakeName +" "+ objInquiry.ModelName %></h2>
            <p class="margin-top5">Located in <%= objInquiry.CityName +", "+ objInquiry.StateCode %></p>
            <ul class="ul-normal">
                <asp:repeater id="rptBikeDetails" runat="server">
				    <%--<itemtemplate><li><a href="/used/bikedetails.aspx?bike=<%# DataBinder.Eval(Container.DataItem, "ProfileId") %>"><%# DataBinder.Eval(Container.DataItem, "MakeYear") %>, <%# DataBinder.Eval(Container.DataItem, "BikeName") %></a><br><span class="text-grey">@ Rs.<%# DataBinder.Eval(Container.DataItem, "Price") %>; Kms:<%# DataBinder.Eval(Container.DataItem, "Kilometers") %>;</span></li></itemtemplate>--%>
                    <itemtemplate><li><a href="/used/bikes-in-<%# DataBinder.Eval(Container.DataItem, "CityMaskingName").ToString() %>/<%# DataBinder.Eval(Container.DataItem, "MakeMaskingName").ToString() %>-<%# DataBinder.Eval(Container.DataItem, "ModelMaskingName").ToString() %>-<%# DataBinder.Eval(Container.DataItem, "ProfileId") %>/"><%# DataBinder.Eval(Container.DataItem, "MakeYear") %>, <%# DataBinder.Eval(Container.DataItem, "BikeName") %></a><br><span class="text-grey">@ Rs.<%# DataBinder.Eval(Container.DataItem, "Price") %>; Kms:<%# DataBinder.Eval(Container.DataItem, "Kilometers") %>;</span></li></itemtemplate>
			    </asp:repeater>
            </ul>
        </div>
    </div>
    <!--    Right Container ends here -->
</div>
<!-- #include file="/includes/footerInner.aspx" -->
