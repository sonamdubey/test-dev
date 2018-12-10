<%@ Page Language="C#" AutoEventWireup="false" Inherits="MobileWeb.Research.DealerMicrosite" %>
<%@ Import Namespace="Carwale.BL.Experiments" %>
<%@ Import Namespace="Carwale.Entity" %>
<%
	Title = PageTitle;
    Description = PageDescription;
    Canonical = "https://www.carwale.com/new/" + Carwale.Utility.Format.FormatURL(MakeName) + "-dealers/" + DealerCityId + "-" + Carwale.Utility.Format.FormatText(DealerCityName) + "/" + Carwale.Utility.Format.FormatText(DealerName) + "-" + DealerId + "/";
    bool showExperimentalColor = ProductExperiments.IsShowExperimentalColor(CookiesCustomers.AbTest);
%>
<!DOCTYPE html>
<html>
<head>
    <!-- #include file="/m/includes/global/head-script.aspx" -->
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="stylesheet" href="/static/m/css/dealer-locater.css" type="text/css" >   
    <script  type="text/javascript"  src="/static/m/js/v2/app/new-car-common.js" ></script>
    <style>
        fieldset { border: 0; }
        #slide2 { border-bottom: 1px solid #ccc; }
        #slide2 fieldset { margin-top:5px; margin-bottom:5px; padding:5px 10px; }
        #divLargeImgContainer .closePopIcon{position:absolute; right:10px; top:4px; color:#fff; font-size:24px;}
        #divLargeImgContainer .ui-header {padding:8px 10px;}
        .cw-m-dealer-Gallery .prev {background-position: 0 -20px;}
    </style>
</head>

<body class="bg-light-grey  <%= (showExperimentalColor ? "btn-abtest" : "")%>">
    <!-- #include file="/m/includes/header.aspx" -->
    <form name="MyForm">
        <section class="container">
            <div id="divOverlay" class="hide-div"></div>
            <!-- dealer locate Code starts here-->
            <div class="cw-m-dealer-locater-page">
                <div class="grid-12">
                    <div class="margin-top10 margin-bottom10">
                        <div class="glp-pop-hide">
                            <h1><%= DealerName%></h1>
                        </div>
                    </div>
                    <div class="content-box-shadow margin-bottom20" id="tabs">
                        <div class="panel-group">
                            <div class="cw-m-dlp-tabs glp-pop-hide">
                                <ul><li id="buying_assistance" data-id="buying-assistance" class="nav_tab active" tabname="buying-assistance">Buying Assistance</li>
                            
                                    <li data-id="contact" class="nav_tab" tabname="contact">Contact</li>
                            
                            
                                    <li data-id="gallery" class="nav_tab" tabname="gallery">Gallery</li>
                                </ul>
                                <div class="clear"></div>
                            </div>
                        </div>
                        <div class="cw-m-dlp-data2 content-inner-block-10 hide" id="contact" data-bind="foreach:dealer">
                            <div class="cw-m-dlp-info">
                                <strong class="font16">Address :</strong><br>
                                <span><%= DealerAddress %>, <%= DealerCityName%><%=string.IsNullOrWhiteSpace(details.StateName+" "+details.Pincode) ? "" : ", " + details.StateName + " " + details.Pincode %></span>
                                <br>
                                <div>
                                    <%= !String.IsNullOrWhiteSpace(Phone) ? "<div><div class='cw-m-dlp-address-l'>Phone :</div><div id='phoneNo' class='cw-m-dlp-address-r'>" + Phone + "</div><div class='clear'></div></div>" : "" %>
                                    <div>
                                        <div class="cw-m-dlp-address-l">Hours :</div>
                                        <div id="time" class="cw-m-dlp-address-r"><strong><%= details.ShowroomStartTime+" - "+details.ShowroomEndTime%></strong></div>
                                        <div class="clear"></div>
                                    </div>
                                    <div>
                                        <div class="cw-m-dlp-address-l">Email :</div>
                                        <div class="cw-m-dlp-address-r"><span><%= DealerEmailId %></span></div>
                                        <div class="clear"></div>
                                    </div>
                                    <div class="clear"></div>
                                </div>
                            </div>
                            <div class="padding5 map-hide">
                                <div class="border-gray border-radius3">
                                    <div id="map" style="height: 300px;"></div>
                                    <%--<iframe src="https://www.google.com/maps/embed?pb=!1m12!1m8!1m3!1d30172.94341435622!2d73.08057256823733!3d19.03655054974405!3m2!1i1024!2i768!4f13.1!2m1!1smaruti+suzuki+showroom!5e0!3m2!1sen!2sin!4v1423809122615" width="100%" height="200" frameborder="0" style="border:0; vertical-align:middle"></iframe>--%>
                                </div>
                            </div>
                            
                         </div>

                        <div class="cw-m-dlp-data2 content-inner-block-10" id="buying-assistance">
                            <div class="dlp-tabs-div">
                                <!--add class hide-div here while displaying Thank u sms -->
                                <div class="font14 margin-top10">Please help us with these details for the dealer to help you in your car buying.</div>
                                <div>
                                    <div id="cw-m-dlp-accordian">
                                         <ul id="cw-m-dlp-accordian-ul" class="dl-microsite-tab">
                                	        <li id="liBuyingAssistance" class="margin-bottom10">
                                    	        <div id="divSelectCar" class="cw-m-dlp-ac-press relt-position form-control">
                                        	        <span class="fa fa-car margin-right10 floatleft car-icon"></span>
                                                    <span class="fa fa-sort-desc arrow-icon"></span>
                                                    <span class="cw-m-sprite filled-tick-blue abt-position-right hide" style="display:none;"></span>
                                                    <span id="tab1" class="dl-title floatleft">Select Car</span>                                            
                                                    <span class="dlp-icon-sprite dlp-error-icon abt-position-right hide" style="display:none;"></span>
                                                    <div class="cw-m-blackbg-tooltip hide">Please select car</div>
                                                    <div class="clear"></div>
                                                </div>
                                                <div class="cw-m-dlp-ac-info select-car hide">
                                                    <ul id="model" data-bind="template: { name: 'car-model-template', foreach: pqCarModels }">
                                                    </ul>
                                                    <script type="text/html" id="car-model-template">
                                                        <li onclick="modelChanged(this);" data-bind='attr:{value:modelId,text:modelName}'>
                                                            <a href="#"><span data-bind="text:modelName"></span></a>
                                                        </li>
                                                    </script>
                                                </div>
                                            </li>
                                            <li class="margin-bottom10">
                                                 <div id="divCustName" class="field-box">
                                                    <span class="fa fa-user"></span>
                                                    <span class="dlp-icon-sprite dlp-error-icon abt-position-right" style="display:none;"></span>
                                                    <div class="cw-m-blackbg-tooltip hide"></div>
                                                    <input type="text" id="custName" placeholder="Enter your name" data-role="none">
                                                </div>
                                            </li>
                                             <li class="margin-bottom10">
                                                 <div id="divCustMob" class="field-box">
                                                    <span class="fa fa-mobile abt-position-left"></span>
                                                    <span class="dlp-icon-sprite dlp-error-icon abt-position-right" style="display:none;"></span>
                                                    <div class="cw-m-blackbg-tooltip hide"></div>
                                                    <span class="plusNum">+91</span>
                                                    <input class="field-box-mobile" type="text" id="custMobile" placeholder="Mobile number" data-role="none">
                                                </div> 
                                            </li>
                                          
                                            <li id="liServiceDetails" class="margin-bottom10">
                                    	        <div class="cw-m-dlp-ac-press relt-position">
                                        	        <span class="fa fa-wrench"></span>
                                                    <span id="divServiceDetails" class="dl-title floatleft padding-left30">Service Details <span class="font11 lightgray normal-text">&nbsp;&nbsp;(optional)</span></span>
                                                    <span class="fa fa-plus"></span>
                                                    <span class="fa fa-minus hide"></span>
                                                    <div class="clear"></div>                                        	
                                                </div>
                                                <div class="cw-m-dlp-ac-info info-check-box margin-top10" id="slide2" style="display:none;">
                                                    <fieldset data-role="controlgroup">
                                                        <input name="CompleteProductBrochure" id="CompleteProductBrochure" class="checkboxExample1" type="checkbox" />
                                                        <label for="CompleteProductBrochure" class="check1 ">Complete Product Brochure </label>
                                                    </fieldset>
                                                    <fieldset data-role="controlgroup">
                                                        <input name="AvailabilityEnquiry" id="AvailabilityEnquiry" class="checkboxExample1" type="checkbox" />
                                                        <label for="AvailabilityEnquiry" class="check1">Availability Enquiry</label>
                                                    </fieldset>
                                                    <fieldset data-role="controlgroup">
                                                        <input name="DoorstepTestDrive" id="DoorstepTestDrive" class="checkboxExample1" type="checkbox" />
                                                        <label for="DoorstepTestDrive" class="check1">Door-step Test Drive</label>
                                                    </fieldset>
                                                    <fieldset data-role="controlgroup">
                                                        <input name="Offer&DiscountInformation" id="offerDiscountInformation" class="checkboxExample1" type="checkbox" />
                                                        <label for="offerDiscountInformation" class="check1">Offer & Discount Information</label>
                                                    </fieldset>
                                                    <fieldset data-role="controlgroup">
                                                        <input name="OtherAssistance" id="OtherAssistance" class="checkboxExample1" type="checkbox" />
                                                        <label for="OtherAssistance" class="border-bottom-width check1">Other Assistance</label>
                                                    </fieldset>
                                                </div>
                                            </li>
                                        </ul>
                                    </div>
                                    <div class="margin-top10">
                                        <input type="button" id="btnSubmit" value="Get Buying Assistance" class="btn btn-orange btn-full-width text-uppercase">
                                        <div class="clear"></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!--Gallery tab starts here -->
				        <div class="cw-m-dlp-data2 content-inner-block-10 hide" id="gallery">
							        <div class="dlp-gal-div">
								        <div class="cw-m-dealer-Gallery margin-top10">
									        <div style="position:relative;">
                                        <%if(ImagesAvailable){ %>
                                            <asp:Repeater ID="rptImages" runat="server">
                                                <HeaderTemplate>
                                                    <ul id="divPhotos">  
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <li onclick="showLargePhotos(this)" class="thumbDiv">
									                <img src="<%# ((Carwale.Entity.Dealers.AboutUsImageEntity)Container.DataItem).ImgLargeUrl%>">
									                </li>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    </ul>
                                                </FooterTemplate>
                                            </asp:Repeater>
                                        <%} else { %>
									        <div class="font14" style="padding-bottom:50px">There are currently no images available for this dealership.</div>
                                        <%} %>
									        <div class="clear"></div>

									        <!-- Large image code starts here-->
									        <div id="divPhotosOverlay"></div>
									        <div class="hide-div" id="divLargeImgContainer" style="margin-top:-15px;">
									        <div data-icon="delete" class="ui-corner-top ui-header ui-bar-b position-rel" data-theme="b" data-role="header" role="banner">
									        <a class="ui-btn-right ui-link ui-btn ui-btn-b ui-icon-delete ui-btn-icon-notext ui-shadow ui-corner-all closePopIcon" data-iconpos="notext" data-icon="delete" data-theme="b" id="popupPhoto" data-role="button" onClick="showThumbnails();" href="#"><span class="fa fa-times-circle"></span></a>
									        <h2 class="ui-title" role="heading" aria-level="1" style="color:#fff;"><%= DealerName%></h2>
									        </div>
									        <div id="divLargeImg"></div>
									        <div>
									        <div onclick="prevClicked();" class="prev cw-m-sprite"></div>
									        <div onclick="nextClicked();" class="next cw-m-sprite"></div>
									        <div class="clear"></div>
									        </div> 
										   
									        </div>
									        <!-- Large image code ends here-->

									        </div>
								        </div>
                            </div>
                        </div>
				        <!--Gallery tab ends here -->
                
                        <!-- thank u page code starts here -->
                        <div class="dlp-thanks hide" id="thankYou">
                            <!-- add class show-div here while displaying Thank u sms -->
                            <div class="box-bot">
                    	        <div class="dlp-thanks-head">
                        	        <span class="right-tick-bg-gree">
                            	        <span class="cw-m-sprite right-tick"></span>
                                    </span>
                        	        <strong>Thank You!</strong>
                                </div>
                                <div class="font14 margin-top5">
                        	        <p>Thank you for your request. <br>
                        	        <span class="text-black"><%= DealerName %></span> would get in touch with you shortly with an appointment confirmation.</p>
                                </div>    
                                <div id="divCustEmail" class="field-box">
                                                            <span class="dlp-icon-sprite dlp-mail-icon abt-position-left"></span>
                                                            <span class="dlp-icon-sprite dlp-error-icon abt-position-right" style="display:none;"></span>
                                                            <div class="cw-m-blackbg-tooltip hide"></div>
                                                            <input type="text" id="custEmail" placeholder="Enter your email" data-role="none">
                                </div>                    
                            </div>
                            <div class="margin-top10 padding-left10 padding-right10 margin-bottom10">
                                <div class="cw-m-dlp-button-group">
                                    <input type="button" value="Done" id="btnDone" class="btn btn-orange btn-full-width text-uppercase">
                                </div>
                            </div>
                        </div>
                        <div class="clear"></div>
                        <!-- thank u page code ends here-->
                    </div>
                 
                </div>
                <div class="clear"></div>
            <!-- dealer locate Code ends here-->
            </div>
            <div class="extraDivHt"></div>
            <%= !String.IsNullOrWhiteSpace(Phone) ? ("<section id='contact-fixed' class='container bg-dark-grey fix-call-section text-center padding-top10 padding-bottom10'><a href=tel:" + Carwale.Utility.Format.getNumberFromString(Phone)
            + " class='btn btn-green btn-full-width text-bold text-uppercase' data-role='click impression' data-event='CWNonInteractive' data-action='Dealer-Locator-Premium-Microite-Slug' data-cat='Call-Slug-Behaviour' data-label='" + 
            MakeName + ',' + DealerCityName + ',' + DealerName + ',' + DealerId + ',' + Phone + 
            "' data-cwtccat='DealerMicroSitePage' data-cwtcact='MaskingNumberClick' data-cwtclbl='" +
            string.Format("cityid={0}|makieid={1}|dealerid={2}|campaignid={3}|campaignshown=1" ,DealerCityId ,MakeId ,DealerId ,CampaignId ) +
            "' ><span class='cw-m-sprite cw-m-dlp-call-iconbtn'></span> Call Dealer </a><div class='clear'></div></section>") : ""  %>
        </section>
        <div class="clear"></div>
<!-- #include file="/m/includes/footer.aspx" -->
<!-- #include file="/m/includes/global/footer-script.aspx" -->
<script  type="text/javascript"  src="/static/m/js/dealershowroom.js" ></script>
<script language="javascript" type="text/javascript">
        var count ;
        var viewcount=0 ;
        function showLargePhotos(_thumbDiv)
        {
            fullUrl = $(_thumbDiv).find("img").attr("src").toString().replace("80x52.jpg", "940x300.jpg");	
                    
            var thumbDivs = $("#divPhotos .thumbDiv");
            totalThumbDivs = thumbDivs.length;
            
            currentIndex = 0;
            var i = 0;
                    
            thumbDivs.each(function(){
                if ($(this).find("img").attr("src").toString().replace("80x52.jpg", "940x300.jpg") == fullUrl)
                    currentIndex = i;
                else
                    i++;		
            });
            
            $("#divPhotosOverlay").attr("style", "height:"+ (parseInt($("#divPhotos").height())+10) +"px;").addClass("overlay");
            loadLargePhoto();
            dataLayer.push({ event: 'locate_dealer_section', cat: 'dealer_microsite_mobile', act: 'image_popup', lab: '<%=DealerName.ToLower() %>'});
                viewcount++;
            }
            
            function loadLargePhoto()
            {   
                var imgLarge = $(document.createElement("img"));
                imgLarge.attr("src", fullUrl)
                imgLarge.attr("style", "height: auto !important; max-width: 100% !important; width: 100%;");
                imgLarge.bind("load", function(){
                    setTimeout(function () { 	
                        $("#divLargeImg").html("");
                        imgLarge.appendTo("#divLargeImg");	
                        $("#divPhotosOverlay").hide();
                        $("#divPhotos").hide();
                        $('.banner-div,.header,#footerwrapper').hide();
                        $('.pgsubhead').parent().hide();
                        $("#divLargeImgContainer").show();
                        //code for hide and show Tab and heading while popup
                        $(".glp-pop-hide").hide();
                        $( "#popupPhoto" ).click(function() {
                            $(".glp-pop-hide").show();
                        });
                        //code for hide and show Tab and heading while popup
                        $("#divLargeImgContainer .prev").show();
                        $("#divLargeImgContainer .next").show();
                    }, 2000);
                });	
                imgLarge.bind("error", function(){
                    imgLarge.attr("src", "https://img.carwale.com/adgallery/no-img-big.png")
                    setTimeout(function () { 	
                        $("#divLargeImg").html("");
                        imgLarge.appendTo("#divLargeImg");	
                        $("#divPhotosOverlay").hide();
                        $("#divPhotos").hide();
                        $('.banner-div,.header,#footerwrapper').hide();
                        $('.pgsubhead').parent().hide();
                        $("#divLargeImgContainer").show();
                        //code for hide and show Tab and heading while popup
                        $(".glp-pop-hide").hide();
                        $( "#popupPhoto" ).click(function() {
                            $(".glp-pop-hide").show();
                        });
                        //code for hide and show Tab and heading while popup
                        $("#divLargeImgContainer .prev").show();
                        $("#divLargeImgContainer .next").show();
                    }, 2000);
                });
            }
            
            function nextClicked()
            {
                currentIndex++;
                var lastPossibleIndex=$("#divPhotos .thumbDiv").length-1;
                if(currentIndex<=lastPossibleIndex)
                {
                    fullUrl = $("#divPhotos .thumbDiv").eq(currentIndex).find("img").attr("src").toString().replace("80x52.jpg", "940x300.jpg");
                    $("#divPhotosOverlay").attr("style", "height:"+ (parseInt($("#divLargeImgContainer").height())+20) +"px;").addClass("overlay");	
                    loadLargePhoto();
                    dataLayer.push({ event: 'locate_dealer_section', cat: 'dealer_microsite_mobile', act: 'image_popup_next', lab: '<%=DealerName.ToLower() %>'});
                    viewcount++;
                }else
                {
                    loadNextPageInBackground=true;
                    currentIndex=0;
                    fullUrl = $("#divPhotos .thumbDiv").eq(currentIndex).find("img").attr("src").toString().replace("80x52.jpg", "940x300.jpg");
                    $("#divPhotosOverlay").attr("style", "height:"+ (parseInt($("#divLargeImgContainer").height())+20) +"px;").addClass("overlay");	
                    loadLargePhoto();
                    dataLayer.push({ event: 'locate_dealer_section', cat: 'dealer_microsite_mobile', act: 'image_popup_next', lab: '<%=DealerName.ToLower() %>'});
                    viewcount++;
                }
            }
            
            function prevClicked()
            {
                currentIndex--; //alert(currentIndex)
                fullUrl = $("#divPhotos .thumbDiv").eq(currentIndex).find("img").attr("src").toString().replace("80x52.jpg", "940x300.jpg");
                $("#divPhotosOverlay").attr("style", "height:"+ (parseInt($("#divLargeImgContainer").height())+20) +"px;").addClass("overlay");	
                loadLargePhoto();
                dataLayer.push({ event: 'locate_dealer_section', cat: 'dealer_microsite_mobile', act: 'image_popup_previous', lab: '<%=DealerName.ToLower() %>'});
                viewcount++;
            }
            
            function showThumbnails()
            {
                $("#divPhotosOverlay").hide();
                $("#divLargeImgContainer").hide();
                $("#divPhotos").show();	
                $('.banner-div,.header,#footerwrapper').show();
                $('.pgsubhead').parent().show();
                viewcount=0;
                dataLayer.push({ event: 'locate_dealer_section', cat: 'dealer_microsite_mobile_image_views', act: '<%=DealerName.ToLower() %>', lab: viewcount});
            }
            
            </script>
<script>
    var submitBtnClicked = 0;
    var isMapInitialized = false;
    var checkedboxes = 0;
    var thankYouShown = 0;
    var latitude = '<%= Latitude%>';
    var longitude = '<%= Longitude%>';
    var cityId = '<%=DealerCityId%>';
    var response, imagesResponse, images;
    var items;
    var make = '<%=Request.QueryString["make"]%>';
    var makeName='<%=MakeName%>';
    var city = '<%=DealerCityId%>';
    var dealer = '<%=DealerId%>';
    var comments = "";
    var errMsgName="",errMsgEmail="",errMsgMobile="";
    var selectedCityName = '<%= DealerCityName%>';
    if (($.cookie('_CustomerName') == null || $.cookie('_CustEmail') == null || $.cookie('_CustMobile') == null)) {
    } else {
        $('#custName').val($.cookie('_CustomerName'));
        $('#custEmail').val($.cookie('_CustEmail'));
        $('#custMobile').val($.cookie('_CustMobile'));
    }
    $(document).ready(function (e) {
        Common.utils.loadGoogleApi(null,null);
        count = $("#divPhotos li img").length;
        $(".cw-m-dlp-tabs li").click(function () {
            var panel = $(this).closest(".panel-group");
            panel.find(".cw-m-dlp-tabs li").removeClass("active");
            $(this).addClass("active");
            $('#tabs').find('.cw-m-dlp-data2').hide();
            var panelId = $(this).attr("data-id");
            if(panelId!="contact") {$('#footerwrapper').attr('style','');}
            else {$('#footerwrapper').attr('style','padding-bottom: 80px;');}
            $("#" + panelId).show();
            if(panelId=="buying-assistance"){
                Dealers.utils.hideCallDealer();
                closeThankYouPopup();
                closeSelectCarSection();
            }
            $("#btnSubmit").val("Get Buying Assistance");
            
            if (panelId == "contact" || panelId == "gallery") {
                Dealers.utils.showCallDealer();
                $('#thankYou').hide();
            }else {
                if (panelId == "buying-assistance" && thankYouShown == 1) {
                    $('#thankYou').show();
                    $('#buying-assistance').hide();
                }
            }
            //initialize google map when contact details tab is clicked for the first time
            if (!isMapInitialized && panelId == 'contact') {
                if(isLatLongValid(latitude, longitude)){
                    $(".map-hide").show();
                    window.setTimeout(function () { initialize() }, 1000);
                    isMapInitialized = true;
                }
                else{
                    $(".map-hide").hide();
                }
            }
        });
        $('.cw-m-dlp-ac-press').removeClass("hide-div");
        $("#slide1").removeClass("hide-div");
        $("#cw-m-dlp-accordian-ul li span.cw-m-dlp-ac-press").click(function () {
            $("#tab1").show();
            var tabVal = $("#tab1").text();
            if (tabVal == "Select Car") {
            }else {
                $("#cw-m-dlp-accordian-ul li span.cw-m-dlp-ac-press").removeClass("hide-div");
                $("#cw-m-dlp-accordian li div").removeClass("show-div");
                $(this).siblings().addClass("show-div").parent();
                $(this).removeClass("cw-m-dlp-ac-inactive");
                $("#slide1,#slide2").addClass("hide-div");
                $(current).removeClass("hide-div");
                return false;
            }
        });
    });
    $('#btnSubmit').click(function () {
        dealerObject.EncryptedPQDealerAdLeadId = "";
        submitBtnClicked += 1;
        if(validateAndSendInquiry())
        $('#btnSubmit').val("Processing...");
    });
    
    $('#btnDone').click(function () {
        submitBtnClicked += 1;
       if(validateAndSendInquiry())
        $('#btnSubmit').val("Get Buying Assistance");
    });
    function closeBuyingAssistancePopup(){
        $('#buying-assistance').hide();
        $('#thankYou').show();
    }
    function closeThankYouPopup(){
        $('#buying-assistance').show();
        thankYouShown = 0;
        $('#thankYou').hide();
        uncheckCheckboxes();
        $("#tab1").show();
        $("#cw-m-dlp-accordian-ul li span.cw-m-dlp-ac-press").removeClass("hide-div").addClass("cw-m-dlp-ac-inactive");
        $("#slide1").removeClass('hide-div').addClass("show-div");
        $("#tab1").removeClass("cw-m-dlp-ac-inactive").text("Select Car").removeClass('blue-text-dark');
        $("#slide2").removeClass('show-div').addClass("hide-div");
        openSelectCarSection();
        var buyingAssistTab = $('#liBuyingAssistance');
        buyingAssistTab.find('.filled-tick-blue').hide();
        selectedModelName = "";
        buyingAssistTab.find('.select-car-icon').removeClass('select-car-icon-selected');
    }
    function validateAndSendInquiry(){
        var custName = $.trim($('#custName').val());
        var custEmail = $.trim($('#custEmail').val());
        var custMobile = $.trim($('#custMobile').val());
        checkedboxes = $(".checkboxExample1:checked").length;
        var comments = checkCheckboxes();
        var valid = checkAllSections();
        if (valid == true) {
            document.cookie = '_CustEmail=' + custEmail + '; expires = ' + permanentCookieTime() + "; domain=" + defaultCookieDomain + '; path =/';
            document.cookie = '_CustomerName=' + custName + '; expires = ' + permanentCookieTime() + "; domain=" + defaultCookieDomain + '; path =/';
            document.cookie = '_CustMobile=' + custMobile + '; expires = ' + permanentCookieTime() + "; domain=" + defaultCookieDomain + '; path =/';
            sendNewCarRequest(custName, custMobile, selectedModelId, "", custEmail, "", "", comments);
            submitBtnClicked = 0;
            return true;
        }
        return false;
    }
    
    function initialize(){
        try{
            var myCenter = new google.maps.LatLng(latitude, longitude);
            var mapProp = {
                center: myCenter,
                zoom: 16,
                mapTypeId: google.maps.MapTypeId.ROADMAP
            };
            var map = new google.maps.Map(document.getElementById("map"), mapProp);
            var marker = new google.maps.Marker({
                position: myCenter,
            });
            marker.setMap(map);
            google.maps.event.addDomListener(window, 'load', initialize);
        }
        catch (e) {
            console.debug(e);
        }
    }
    function checkCheckboxes() {
        var comment = "";
        var comments = "";
        $('#slide2 label').each(function () {
            if ($(this).hasClass("ui-checkbox-on")) {
                comment = $.trim($(this).text());
                comments += comment + ",";
            }
        });
        return comments.slice(0, -1);
    }
    function uncheckCheckboxes() {
        $('input:checkbox').removeAttr('checked');
        $(".check1").removeClass('ui-checkbox-on').addClass('ui-checkbox-off');
    }
    var dealerObject = new Object();
    function sendNewCarRequest(name, mobile, modelId, txtDate, email, version, regNo, comments) {
        dealerObject.DealerId = '<%=CampaignId%>';
        dealerObject.ModelId = modelId;
        dealerObject.CityId = cityId;
        dealerObject.Name = name;
        dealerObject.Email = email;
        dealerObject.Mobile = mobile;
        dealerObject.ModelName = selectedModelName.trim();
        dealerObject.Comments = comments.replace("Assistance", "");
        dealerObject.BuyTimeText = "1 week";
        dealerObject.BuyTimeValue = 7;
        dealerObject.RequestType = 1;
        dealerObject.UtmaCookie = isCookieExists('__utma') ? $.cookie('__utma') : '';
        dealerObject.UtmzCookie = isCookieExists('_cwutmz') ? $.cookie('_cwutmz') : '';
        dealerObject.InquirySourceId = "95";
        dealerObject.PQId = 0;
        dealerObject.IsAutoApproved = false;
        dealerObject.AssignedDealerId = -1;
        dealerObject.LeadClickSource = "107";
        dealerObject.PlatformSourceId = "43";
        dealerObject.ModelsHistory = getUserModelHistory();
        dealerObject.SponsoredBannerCookie = isCookieExists('_sb' + dealerObject.ModelId) ? $.cookie('_sb' + dealerObject.ModelId) : '';
        
        $.ajax({
            type: "POST", 
            url: "/webapi/DealerSponsoredAd/PostDealerInquiry/",
            data: dealerObject,
            success: function (response) {
                if($('#liBuyingAssistance').is(':visible')){
                    closeBuyingAssistancePopup();
                }
                if (typeof dealerObject.EncryptedPQDealerAdLeadId == "undefined" || dealerObject.EncryptedPQDealerAdLeadId == "") {
                    leadConversionTracking.track(dealerObject.LeadClickSource, dealerObject.DealerId, dealerObject.CityId, response, 43);
                }
                dataLayer.push({ event: 'locate_dealer_section', cat: 'dealer_microsite_mobile', act: 'submit_success_checkboxes', lab: checkedboxes + "" }); 
                cwTracking.trackCustomData('DealerMicroSitePage', 'DealerMicroSiteLeadSubmit','make:'+makeName+'|model:'+selectedModelName.trim()+'|city:'+selectedCityName, false);
                dealerObject.EncryptedPQDealerAdLeadId = response;
            }
        });
    }
    $('.nav_tab').click(function () {
        action = $(this).attr('tabname') + "_tab";
        dataLayer.push({ event: 'locate_dealer_section', cat: 'dealer_microsite_mobile', act: action, lab: '<%=DealerName.ToLower() %>' });
    });
    $('.navigator').click(function () {
        dataLayer.push({ event: 'locate_dealer_section', cat: 'dealer_microsite_mobile', act: 'get_directions', lab: '<%=DealerName.ToLower() %>' });
    });
    function navigate(lng, lat) {
            window.open('http://maps.google.com?daddr=' + lat + ',' + lng + '&amp;ll=');
    }
    var makeId = <%=Request.QueryString["make"]%>;
    //To bind models 
    bindModels(makeId,dealer);
       
    //When select car section is clicked
    $('#divSelectCar').click(function(){
        if($('#liBuyingAssistance').find('.select-car').hasClass('hide'))
        {
            openSelectCarSection();
        } else {
            closeSelectCarSection();
        }
        });
    //When service details section is clicked
    $('#divServiceDetails').parent().click(function(){
        if($('#liServiceDetails').find('.info-check-box').is(':visible'))
        {
            closeSelectServicesSection();
        } else {
            openSelectServicesSection();
        }
    });
</script>
</form>
</body>
</html>