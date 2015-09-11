﻿<%@ Page Language="C#" AutoEventWireup="false" Trace="false" Inherits="Bikewale.PriceQuote.RSAOfferClaim" %>
<%@ Register TagPrefix="BikeWale" TagName="Calender" Src="~/controls/DateControl.ascx" %>
<%
    title = "Bike Purchase Offer Claim";
    AdId = "1395986297721";
    AdPath = "/1017752/BikeWale_New_";
%>
<!-- #include file="/includes/headNew.aspx" -->
<link rel="stylesheet"  href="<%= !String.IsNullOrEmpty(staticUrl) ? "http://st2.aeplcdn.com" + staticUrl : "" %>/css/bw-pq-new.css?23july2015" />
<style>
    .inner-content {
    border:1px solid #eaeaea;
    padding:10px;
    margin-bottom:20px;
    }
    .grey-bullets li{ 
        background:url(http://img1.carwale.com/bikewaleimg/images/bikebooking/images/bw-grey-bullet.png) no-repeat 0px 9px;
        display: block;
        list-style: square outside none;
        padding: 3px 0 3px 10px;
    }

</style>
<div class="main-container">
    <div class="container_12">
        <div class="grid_12">
            <ul class="breadcrumb">
                <li>You are here: </li>
                <li><a class="blue" href="/">Home</a></li>
                <li class="fwd-arrow">&rsaquo;</li>
                <li><a class="blue" href="/new/">New</a></li>
                <li class="fwd-arrow">&rsaquo;</li>
                <li class="current">Bike Purchase Offer Claim</li>
            </ul><div class="clear"></div>
        </div>
        <div class="grid_8 margin-top10">
            <h1 class="margin-bottom5">Bike Purchase Offer Claim</h1>
            <div id="div_GetPQ" class="<%= isOfferClaimed ? "hide" : "" %>">
		        <!--make payment div starts here-->
                <div class="inner-content relative">
                    <h2 class=" margin-bottom10 payment-pg-heading">Offer 1: Select Your Free Helmet</h2>
                    <p class="margin-top10">Select one helmet from the below three options by clicking on the image:</p>
                    <div class="offer-1 margin-top10">
                        <ul>
                            <li class="offer-box-1" offerId="1">
                                <div data-id="offer1" class="offer-1-title">
                                    <span class="bw-sprite unchecked-radio"></span>
                                    <label for="offer">Vega / Studds Open Face Helmet (Size: M) </label>
                                    <div class="clear"></div>
                                </div>
                                <div class="center-align offer-pic"><img src="http://img.aeplcdn.com/bikewaleimg/images/bikebooking/images/offer-list-pic1.jpg"></div>                                
                                <div class="clear"></div>
                            </li>
                            <li class="offer-box-2" offerId="2" style="display:none;">
                                <div data-id="offer1" class="offer-1-title">
                                    <span class="bw-sprite unchecked-radio"></span>
                                    <label for="offer">Replay Plain Flip-up Helmet (Size: M) </label>
                                    <div class="clear"></div>
                                </div>
                                <div class="center-align offer-pic"><img src="http://img.aeplcdn.com/bikewaleimg/images/bikebooking/images/offer-list-pic2.jpg"></div>                                
                                <div class="clear"></div>
                            </li>
                            <li class="offer-box-3" offerId="3">
                                <div data-id="offer1" class="offer-1-title">
                                    <span class="bw-sprite unchecked-radio"></span>
                                    <label for="offer">Vega / Studds Full Face Helmet (Size: M)</label>
                                    <div class="clear"></div>
                                </div>
                                <div class="center-align offer-pic"><img src="http://img.aeplcdn.com/bikewaleimg/images/bikebooking/images/offer-list-pic3.jpg"></div>                                
                                <div class="clear"></div>
                            </li>
                            <div class="clear"></div>
                        </ul>
                        <div class="clear"></div>
                        <span id="errHelmetOffer" class="error"></span>
                    </div>
                    <!-- offer popup starts here -->
                    <div class="specification-popup-1">
                        <div class="grey-bullets">
                            <ul>
                                <li>Scratch & Crack Resistant</li>
                                <li>Texture finish</li>
                                <li>UV protection visor</li>
                                <li>Color: Red</li>
                            </ul>
                        </div>
                    </div>
                    <div class="specification-popup-2">
                        <div class="grey-bullets">
                            <ul>
                                <li>Dual Full-cum-open face</li>
                                <li>Hard coated visor</li>
                                <li>Superior paint finish</li>
                                <li>Color: Matt Cherry Red</li>
                            </ul>
                        </div>
                    </div>
                    <div class="specification-popup-3">
                        <div class="grey-bullets">
                            <ul>
                                <li> ABS shell</li>
                                <li>Scratch resistant</li>
                                <li>Lightweight & compact</li>
                                <li>Color: Black</li>
                            </ul>
                        </div>
                    </div>
                    <!-- offer popup ends here -->
                    <asp:HiddenField id="hdnSelHelmet" runat="server" />
                </div>
                <!--make payment div ends here-->
                <!-- plus circle starts here -->
                <div class="plus-text">
                    <div><span>+</span></div>
                </div>
                <!-- plus circle ends here -->
                <!--steps starts here-->
                <div class="inner-content">
                    <h2 class="payment-pg-heading">Offer 2: Your Free Roadside Assistance</h2>
                        <div class="margin-top10">
                            <p class="black-text"><strong>Incident Inclusive - Two Wheeler Assistance (Validity: 12 Months)</strong></p>
                            <div class="grey-bullets margin-top10">
                                <ul>
                                    <li>Roadside Repair (On site Minor Repair, Lost keys/ replacement, Flat tyre Support, etc.)</li>
                                    <li>Towing for mechanical, electrical & accidental breakdown (one way).</li>
                                    <li>Customer Service (Toll free hotline, Customer satisfaction survey and Medical Assitance).</li>
                                </ul>                                
                            </div>
                        </div>
                <!--steps end here-->
                </div>
                <!-- share information starts here -->
                <div class="inner-content">
                    <h2 class="payment-pg-heading">Share Bike Purchase Details to avail both offers</h2>
                    <div>                        
                        <!-- table starts here -->
                        <div class="steps">
                            <p class="desc-para">Please provide following information about your bike purchase to claim your free gifts. Upon verification of your purchase, we will ship the gift directly to your address.</p>
                            <p class="desc-para">All <span class="required">*</span> marked fields are required</p>
                            <div id="get-pq-new" class="inner-content">
                                <div class="mid-box" id="pq_car">			       
                                    <table cellspacing="0" cellpadding="3" width="100%" border="0">	
                                        <tr>
                                            <td style="width:230px;">
                                                <b>Full Name as per Vehicle Registration<span class="error">*</span></b>
                                            </td>
                                            <td>
                                                <asp:TextBox id="txtName" runat="server"></asp:TextBox>
                                                <span id="spnName" class="error"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <b>Mobile Number<span class="error">*</span></b>
                                            </td>
                                            <td>
                                                <asp:TextBox id="txtMobile" runat="server"></asp:TextBox>
                                                <span id="spnMobile" class="error"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <b>Email Id<span class="error">*</span></b>
                                            </td>
                                            <td>
                                                <asp:TextBox id="txtEmail" runat="server"></asp:TextBox>
                                                <span id="spnEmail" class="error"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td><b>Purchased Bike<span class="error">*</span></b></td>
                                            <td><select id="ddlMake" runat="server"></select><input type="hidden" id="hdnMake" runat="server" /></td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td><select id="ddlModel"><option value="0">--Select Model--</option></select><input type="hidden" id="hdnModel" runat="server" /></td>
                                            
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td>
                                                <select id="ddlVersion"><option value="0">--Select Version--</option></select>
                                                <span id="spnVersion" class="error"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <b>Vehicle Registration Number<span class="error">*</span></b>
                                            </td>
                                            <td>
                                                <asp:TextBox id="txtVehicle" runat="server"></asp:TextBox>
                                                <span id="spnVehicle" class="error"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <b>Address as per Vehicle Registration<span class="error">*</span></b>
                                            </td>
                                            <td>
                                                <asp:TextBox id="txtAddress" rows="2" columns="60" textmode="MultiLine"  runat="server"></asp:TextBox>
                                                <span id="spnAddress" class="error"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <b>Date of vehicle Delivery<span class="error">*</span></b>
                                            </td>
                                            <td>
                                                <BikeWale:Calender DateId="calMakeYear" ID="calDeliveryDate" runat="server" />
                                                <span id="msgMakeYear" class="error"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td><b>Full Name of the Dealer<span class="error">*</span></b></td>
                                            <td>
                                                <asp:TextBox id="txtdealerName" runat="server"></asp:TextBox>
                                                <span id="spnDealerName" class="error"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <b>Address of the Dealer<span class="error">*</span></b>
                                            </td>
                                            <td>
                                                <asp:TextBox id="txtDealerAddress" rows="2" columns="60" textmode="MultiLine"  runat="server"></asp:TextBox>
                                                <span id="spnDealerAddress" class="error"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td><b>Any Comments (Optional)</b></td>
                                            <td>
                                                <asp:TextBox id="txtComments" rows="2" columns="60" textmode="MultiLine"  runat="server"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                            <td><asp:Button class="action-btn text_white" id="btnSubmit" Text="Claim Offer" runat="server" /></td>
                                        </tr>
                                        <input type="hidden" id="hdnVersion" runat="server" />
                                        <input type="hidden" id="hdnBikeName" runat="server"/>
                                    </table>
                                </div>
                            </div>                            
                        </div>
                        <!-- table ends here -->
                    </div>
                    <!--steps end here-->
                </div>
            </div>
            <!-- share information ends here -->
            <div id="RSAMessage" runat="server" class="grey-bg border-light content-block text-highlight margin-top15"></div>
        </div>
        <div class="grid_4">
            <div class="margin-top15">
                <!-- BikeWale_NewBike/BikeWale_NewBike_HP_300x250 -->
                <!-- #include file="/ads/Ad300x250.aspx" -->
            </div>
        </div><!-- Right Container ends here  -->
    </div>
</div>
<script type="text/javascript">
    $("#btnSubmit").click(function () {
        var isError = false;
        var re = /^[0-9]*$/;
        var reEmail = /^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,6}$/;

        $("#spnName").text("");
        $("#spnMobile").text("");
        $("#spnEmail").text("");
        $("#spnVehicle").text("");
        $("#spnAddress").text("");
        $("#msgMakeYear").text("");
        $("#spnDealerName").text("");
        $("#spnDealerAddress").text("");
        $("#spnVersion").text("");
        $("#errHelmetOffer").text("");

        var customerName = $("#txtName").val();
        var customerEmail = $("#txtEmail").val().toLowerCase();
        var CustomerMobile = $("#txtMobile").val();
        var bikeRegistrationNo = $("#txtVehicle").val();
        var customerAddress = $("#txtAddress").val();
        var deliveryDate = $("#calMakeYear").val();
        var dealerName = $("#txtdealerName").val();
        var dealerAddress = $("#txtDealerAddress").val();
        var selHelmet = $("#hdnSelHelmet").val();

        if (customerName == "") {
            $("#spnName").text("Required");
            isError = true;
        } else if (name.length == 1) {
            $("#spnName").text("Please enter your complete name");
            isError = true;
        } else {
            $("#spnName").text("");
        }

        if (CustomerMobile == "") {
            $("#spnMobile").text("Required");
            isError = true;
        } else if (CustomerMobile != "" && re.test(CustomerMobile) == false) {
            $("#spnMobile").text("Please provide numeric data only in your mobile number.");
            isError = true;
        } else if (CustomerMobile.length != 10) {
            $("#spnMobile").text("Your mobile number should be of 10 digits.");
            isError = true;
        } else {
            $("#spnMobile").text("");
        }

        if (customerEmail == "") {
            $("#spnEmail").text("Required");
            isError = true;
        } else if (customerEmail != "" && reEmail.test(customerEmail) == false) {
            $("#spnEmail").text("Invalid Email");
            isError = true;
        } else {
            $("#spnEmail").text("");
        }

        if (bikeRegistrationNo == "") {
            $("#spnVehicle").text("Required");
            isError = true;
        } else {
            $("#spnVehicle").text("");
        }

        if (dealerName == "") {
            $("#spnDealerName").text("Required");
            isError = true;
        } else {
            $("#spnDealerName").text("");
        }

        if (dealerAddress == "") {
            $("#spnDealerAddress").text("Required");
            isError = true;
        } else {
            $("#spnDealerAddress").text("");
        }

        if ($("#ddlVersion").val() <= 0) {
            $("#spnVersion").text("Please select version.");
            isError = true;
        }
        else {
            $("#spnVersion").text("");
        }

        if (selHelmet == "" || selHelmet <= 0) {            
            $("#errHelmetOffer").text("Please select helmet.");
            isError = true;
        } else {
            $("#errHelmetOffer").text("");
        }

        return !isError;
    });

    var make = "";
    var model = "";
    var version = "";
    var bikeName = "";

    $("#ddlMake").change(function () {
        drpMake_Change($(this));              
    });

    $("#ddlModel").change(function () {
        drpModel_Change($(this));        
    });

    $("#ddlVersion").change(function () {
        $("#hdnVersion").val($(this).val());
        version = $(this).find(":selected").text();
        bikeName = make + model + version;
        $("#hdnBikeName").val(bikeName);
    });

    function drpMake_Change(objMake) {
        var bikeMakeId = objMake.val();
        $("#ddlVersion").val("0").attr("disabled", true);
        if (bikeMakeId != 0) {
            make = objMake.find(":selected").text();
            $.ajax({
                type: "POST",
                url: "/ajaxpro/Bikewale.Ajax.AjaxCommon,Bikewale.ashx",
                data: '{"requestType":"NEW", "makeId":"' + bikeMakeId + '"}',
                beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetModels"); },
                success: function (response) {
                    //alert(response);
                    var responseJSON = eval('(' + response + ')');
                    var resObj = eval('(' + responseJSON.value + ')');

                    var dependentCmbs = new Array();

                    bindDropDownList(resObj, $("#ddlModel"), "", dependentCmbs, "--Select Model--");
                }
            });
        } else {
            $("#ddlModel").val("0").attr("disabled", true);
        }
    }

    function drpModel_Change(objModel) {
        var bikeModelId = objModel.val();
        if (bikeModelId != 0) {
            model = objModel.find(":selected").text();
            $.ajax({
                type: "POST",
                url: "/ajaxpro/Bikewale.Ajax.AjaxCommon,Bikewale.ashx",
                data: '{"requestType":"NEW", "modelId":"' + bikeModelId + '"}',
                beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetVersions"); },
                success: function (response) {
                    var responseJSON = eval('(' + response + ')');
                    var resObj = eval('(' + responseJSON.value + ')');

                    var dependentCmbs = new Array();

                    bindDropDownList(resObj, $("#ddlVersion"), "", dependentCmbs, "--Select Version--");
                }
            });
        } else {
            $("#ddlVersion").val("0").attr("disabled", true);
        }
    }
    $(".offer-1 li").click(function () {
        var panel = $(this).closest(".offer-1");
        panel.find("span.bw-sprite").removeClass("checked-radio");
        $(this).find("span.bw-sprite").toggleClass("checked-radio", "unchecked-radio");
        var panelId = $(this).attr("data-id");
        $("#" + panelId).show();
        $(".offer-1 li").removeClass("selected-offer");
        $(this).addClass("selected-offer");
        $("#hdnSelHelmet").val($(this).attr("offerId"));
    });
    $('.offer-box-1').hover(function () {
        $('.specification-popup-1').toggle(200);
    });
    $('.offer-box-2').hover(function () {
        $('.specification-popup-2').toggle(200);
    });
    $('.offer-box-3').hover(function () {
        $('.specification-popup-3').toggle(200);
    });


</script>
<!-- #include file="/includes/footerInner.aspx" -->