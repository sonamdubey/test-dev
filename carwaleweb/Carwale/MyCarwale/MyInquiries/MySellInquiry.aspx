<%@ Page trace="false" Inherits="Carwale.UI.MyCarwale.MyInquiries.MySellInquiry" AutoEventWireUp="false" Language="C#" Debug="false" EnableEventValidation="false" %>
<%@ Import NameSpace="Carwale.UI.Common" %>
<%@ Import NameSpace="Carwale.UI.Used" %>
<%@ Import NameSpace="Carwale.Utility" %>
<%
	// Define all the necessary meta-tags info here.
	// To know what are the available parameters,
	// check page, headerCommon.aspx in common folder.
	
	PageId 			= 72;
	Title 			= "My Car Sell Inquiry Details";
	Description 	= "";
	Keywords		= "";
	Revisit 		= "15";
	DocumentState 	= "Dynamic";
    AdId            = "1337162297840";
    AdPath          = "/7590/CarWale_MyCarWale/CarWale_MyCarWale_Misc/CarWale_MyCarWale_Misc_";
%>
<!doctype html>
<html>
<head>
    <!-- #include file="/includes/global/head-script.aspx" -->
    <script  type="text/javascript"  src="/static/src/graybox.js" ></script>
    <style type="text/css">
	    .inq {padding:4px 2px 4px 2px; border-bottom:2px solid #888888;border-right:2px solid #999999;border-left:1px solid #AAAAAA;border-top:1px solid #AAAAAA;}
	    .pkgHdr	{background-color:#008080;color:#FFFFC6;font-weight:bold;font-size:18px;text-align:left;padding:5px;line-height:22px;}
        ul.listings { list-style:none; }
        ul.listings li { float:left; min-height:150px; }
        .view-link { cursor:pointer; color:#034FB6;  }
        .modal-box{
                width: 345px;
                min-height: 170px;
                padding: 10px;
                text-align: center;
                top: 30%;
                left: 0;
                right: 0;
                margin: 0 auto;
                background: #fff;
                border-radius: 4px;
                z-index: 11;
                display: block;
                position: absolute;
        }
        #modalBg {
            width: 100%;
            height: 100%;
            background: rgba(0,0,0,.6);
            top: 0;
            z-index: 10;
            display: block;
            position: fixed;
            left: 0;
        }
        .premium__popup{
            padding: 30px 8px;
        }
        .modal-box .cross-default-15x16 {
            position: absolute;
            top: 10px;
            right: 10px;
        }

        .cross-default-15x16 {
            width: 25px;
            height: 25px;
            background: url(https://imgd.aeplcdn.com/0x0/cw/static/icons/565a5c/cross-15x16.png) center no-repeat;
            display: inline-block;
            cursor: pointer;
        }
        .modal__header {
            font-size: 14px;
            font-weight: 600;
            color: #1a1a1a;
        }
        .modal__description {
            margin-top: 15px;
        }

     </style>
</head>
<body class="bg-light-grey">
<form runat="server">
<!-- #include file="/includes/header.aspx" -->
    <section class="margin-top70">
        <div class="container">
            <div class="grid-12">
            <ul class="breadcrumb margin-top10">
                <li><a href="/">Home</a></li>
                <li><span class="fa fa-angle-right margin-right10"></span></li>
                <li><a href="/MyCarwale/default.aspx">My CarWale</a></li>
                <li><span class="fa fa-angle-right margin-right10"></span></li>
                <li class="current"><strong> Car(s) Listed For Sale</strong></li>
            </ul>
            <div class="clear"></div>
            <h1 class="font30 text-black special-skin-text">My Car(s) Listed For Sale</h1>
            <div class="border-solid-bottom margin-top10 margin-bottom15"></div>
            </div>
            <div class="clear"></div>
        </div>
    </section>
    <div class="clear"></div>

    <section>
    
    <% if (!isCustomerEditable)
        { %>
    <div id="modalPopUp" data-current="" class="modal-box">
        <div class="premium__popup">
            <span class="modal__close cross-default-15x16"></span>
            <p class="modal__header">Editing disabled</p>
            <p class="modal__description">Since this car has been inspected, editing is disabled. To edit please call on +91-8530482263</p>
        </div>
    </div>
    <div class="clear"></div>
    <div id="modalBg"></div>
    <% } %>

    <div class="container">
    <div class="grid-12">
        <div class="content-box-shadow content-inner-block-10 rounded-corner2 margin-bottom20">
            <div id="left_container_onethird" class="margin-top10">
		        <span id="spnError" class="error" runat="server"></span>		        
		        <div class="infoTop" style="min-height:130px;">
			        <asp:Repeater ID="rptSellInq" runat="server">
                        <headertemplate>
                            <ul>
                        </headertemplate>
				        <itemtemplate>
					        <li id="inquiry_<%# DataBinder.Eval(Container.DataItem, "Id") %>" class="margin-bottom10 border-solid content-inner-block-10 ul-params-div">
                                <ul class="listings">
                                    <li class="grid-2 alpha omega">
                                        <div class="text-center">
										    <img src='<%# (DataBinder.Eval(Container.DataItem, "HostURL") != null && DataBinder.Eval(Container.DataItem, "OriginalImgPath") != null) ? ImageSizes.CreateImageUrl(DataBinder.Eval(Container.DataItem, "HostURL").ToString(), ImageSizes._110X61, DataBinder.Eval(Container.DataItem, "OriginalImgPath").ToString()) : "https://img.carwale.com/used/no-car.jpg"%>' />
                                        </div>
                                        <div class="margin-top10 text-center"><%# DataBinder.Eval(Container.DataItem, "PhotoCount") %> Photos Available</div>
                                        <div class="margin-top5 text-center <%# Convert.ToInt32(DataBinder.Eval(Container.DataItem, "LLInquiryId")) > 0 ? "" : "hide" %>"><a href='/used/cardetails.aspx?car=s<%# DataBinder.Eval(Container.DataItem, "Id") %>' target="_blank">View Car Details</a></div>
                                    </li>
                                    <li class="grid-4 alpha omega">
                                        <asp:Label ID="lblId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "Id")%>' />
									    <strong><%# DataBinder.Eval(Container.DataItem, "CarName")%></strong>
                                        <div class="margin-top5">Profile #: S<%# DataBinder.Eval(Container.DataItem, "Id") %></div>
                                        <div class="margin-top5">Price : <%# CommonOpn.FormatNumeric(DataBinder.Eval(Container.DataItem, "Price").ToString())%></div>
                                        <div class="margin-top5">Kilometers : <%# CommonOpn.FormatNumeric(DataBinder.Eval(Container.DataItem, "Kilometers").ToString())%></div>
                                        <div class="margin-top5">Model Year : <%# Convert.ToDateTime(DataBinder.Eval(Container.DataItem, "MakeYear")).ToString("dd-MMM, yyyy")%></div>
                                        <div class="margin-top5">Car Listed on : <%# Convert.ToDateTime(DataBinder.Eval(Container.DataItem, "EntryDate")).ToString("dd-MMM, yyyy")%></div>
                                        <div class="margin-top5" style="color:#FF0000">
										    <%# SellInquiryStatus( DataBinder.Eval(Container.DataItem, "Id").ToString(), DataBinder.Eval(Container.DataItem, "IsPremium").ToString(), DataBinder.Eval(Container.DataItem, "ClassifiedExpiryDate").ToString(), DataBinder.Eval(Container.DataItem, "PackageExpiryDate").ToString(), DataBinder.Eval(Container.DataItem, "Status").ToString(), DataBinder.Eval(Container.DataItem, "CurrentStep").ToString(), DataBinder.Eval(Container.DataItem, "IsListingCompleted").ToString(), DataBinder.Eval(Container.DataItem, "PackageId").ToString()) %>
									    </div>
                                    </li>
                                    <li class="grid-2 alpha omega">
                                        <div class="<%# Convert.ToInt32(DataBinder.Eval(Container.DataItem, "LLInquiryId")) > 0 ? "" : "hide" %>">
                                            <div><a href='EditSellCar.aspx?id=<%# DataBinder.Eval(Container.DataItem, "Id")%>'>Edit car details</a></div>									    
									        <div class="margin-top5"><a href='/MyCarwale/MyInquiries/SellCarPhotos.aspx?car=S<%# DataBinder.Eval(Container.DataItem, "Id") %>'>Upload car photos</a></div>
                                            <div class="margin-top5 <%# DataBinder.Eval(Container.DataItem, "IsPremium").ToString() == "True" ? "" : "hide" %>"><a href='/MyCarwale/MyInquiries/managevideo.aspx?id=<%# DataBinder.Eval(Container.DataItem, "Id") %>'>Upload car videos</a></div>                                        
                                            <div class="margin-top5 <%# DataBinder.Eval(Container.DataItem, "IsCustomerEditable").ToString() == "True" ? "" : "hide" %>""><a style='cursor:pointer;' onclick="removeCar('<%# DataBinder.Eval(Container.DataItem, "Id") %>')">Stop showing my ad</a></div>
                                            <div class="margin-top20"><strong>Total Buyers : <%# DataBinder.Eval(Container.DataItem, "TotalInq")%></strong> <span class="view-link" id="viewInq_<%# DataBinder.Eval(Container.DataItem, "Id") %>">[View]</span></div>
                                        </div>
                                        <div class="<%# Convert.ToInt32(DataBinder.Eval(Container.DataItem, "LLInquiryId")) > 0 ? "hide" : "" %>">
                                            <span class="view-link" id="delListing_<%# DataBinder.Eval(Container.DataItem, "Id") %>">Delete this listing</span>
                                        </div>
                                    </li>
                                    <li class="grid-4 alpha omega">
                                        <div align="center">                                            
                                            <div style='line-height:15px;'>                                                
											    <div class="pkgHdr"><asp:Literal ID="ltrInquiryStatus" runat="server" /></div>
											    <p align="justify" style="margin:0px; padding:5px;">
												    <asp:Literal ID="ltrInquiryText" runat="server" />
											    </p>
                                                <asp:Button ID="btnListing" runat="server" class="btn btn-white margin-top10"></asp:Button>
										    </div>										    
									    </div>
                                    </li>
                                </ul>
                                <div class="clear"></div>
                                <asp:HiddenField ID="hdnInquiryId" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "Id").ToString()%>'></asp:HiddenField>
                                <asp:HiddenField ID="hdnPackageType" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "PackageId").ToString()%>'></asp:HiddenField>
                                <asp:HiddenField ID="hdnStepId" runat="server" Value='<%# String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "CurrentStep").ToString()) ? "-1" : DataBinder.Eval(Container.DataItem, "CurrentStep").ToString()%>'></asp:HiddenField>
                                <div class="clear"></div>
					        </li><div class="clear"></div>
				        </itemtemplate>
                        <footertemplate>
                            </ul>
                        </footertemplate>
			        </asp:Repeater>
		        </div>
                <div>
                    <asp:LinkButton runat="server" Text="List Car for Free" class="hide"/><!-- Necessory to run below code -->                    
                    <% if(showListingNotification && notificationCode == 3) { %>
                        <div id="notifyListingDetails" class="hide">
                            <p>
                                Looks like you already have one free listing on CarWale. 
                                We're sorry, but as per our Terms & Conditions, there cannot be anymore live 
                                listings on CarWale.
                            </p>
                            <br/>
                            <p>If you wish, you can delete an existing listing from My CarWale and then create a new one.</p>
                        </div>
                    <% } %>
                </div>
	        </div>
        </div>
    </div>
    <div class="clear"></div>
    </div>
    </section>
    <div class="clear"></div>
<!-- #include file="/includes/footer.aspx" -->
<!-- #include file="/includes/global/footer-script.aspx" -->
</form>
<script language="javascript">
    $(document).ready(function(){
        var notify = <%= showListingNotification.ToString().ToLower() %>;
        inquiryId = "", totalPurchaseRequests = 0;
        $("span[id^='viewInq_']").click(function () {
            inquiryId = $(this).attr("id").split('_')[1];
            var caption = "My Used Car Purchase Inquiries <span id=\"spnInquiryCount\"></span>";
            var url = "/mycarwale/myinquiries/mypurchaserequests.aspx?id=" + inquiryId;
            var applyIframe = true;
            var GB_Html = "";
            GB_show(caption, url, 250, 600, applyIframe, GB_Html);        
        });
    
        if (notify == true) {            
            var caption = 'Listing Notification';
            var url = "";
            var applyIframe = false;
            var GB_Html = $('#notifyListingDetails').html();
            GB_show(caption, url, 300, 520, applyIframe, GB_Html);
        }

        $("span[id^='delListing_']").click(function(){
            var objListing = $(this);
            var inquiryId = objListing.attr("id").split('_')[1];
            objListing.text("Processing please wait...");
            $.ajax({
                type: "POST",
                url: "/ajaxpro/CarwaleAjax.AjaxMyCarwale,Carwale.ashx",
                data: '{"inquiryId":"' + inquiryId + '"}',
                beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "UpdateCarIsArchived"); },
                success: function (response) {
                    $("#inquiry_" + inquiryId ).html("<span class=margin-left10><strong>This listing has been deleted successfully.</strong><span>");
                    $("#inquiry_" + inquiryId ).css("background-color", "#ecf4e8");                    
                },
                error: function(response){
                    objListing.text("Delete this listing");                    
                }                
            });
        });
    });
    
    $("#ddlRequestTime").live("change", function () {
        var caption = "My Used Car Purchase Inquiries <span id=\"spnInquiryCount\"></span>";
        var url = "/mycarwale/myinquiries/mypurchaserequests.aspx?id=" + inquiryId + "&requestdate=" + $(this).val();
        var applyIframe = true;
        var GB_Html = "";
        GB_show(caption, url, 250, 600, applyIframe, GB_Html);        
    });

    $(".modal__close,#modalBg").on('click',function(){
        window.location.replace(window.location.href.replace("?ispremium=true",""));
    });

	function removeCar( carId )
	{
		var left 	= ( screen.width - 300 )/2;
		var top		= ( screen.height - 250 )/2;
		var type = "1" ;	//for sell inquiry
		window.open( "removeFromListing.aspx?type=" + type + "&id=" + carId, "remove", "menu=no,address=no,scrollbars=no,width=300,height=250,left=" + left + ",top=" + top );
	}
	
	function renewCar( carId )
	{
	    var left 	= ( screen.width - 425 )/2;
	    var top		= ( screen.height - 230 )/2;
		window.open( "RenewCar.aspx?id=" + carId, "renew", "menu=no,address=no,scrollbars=no,width=425,height=230,left=" + left + ",top=" + top );
	}
</script>
</body>
</html>
