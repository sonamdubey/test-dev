<%@ Page Language="C#" AutoEventWireup="false" Trace="false" Inherits="Bikewale.PriceQuote.RSAOfferClaim" %>

<%@ Register TagPrefix="BikeWale" TagName="Calender" Src="~/controls/DateControl.ascx" %>
<%
    title = "Bike Purchase Offer Claim";
    AdId = "1395986297721";
    AdPath = "/1017752/BikeWale_PQ_";
    //modified by SajalGupta for unfilled impression of ads on 04 Aug 2016.
    isAd300x250_BTFShown = false;
%>

<!-- #include file="/includes/headNew.aspx" -->
<link rel="stylesheet" href="/css/datepicker.css" />
<link rel="stylesheet" href="<%=  staticUrl  %>/css/bw-pq-new.css?<%= staticFileVersion %>" />
<script src="/src/picker.js"></script>
<script src="/src/picker.date.js"></script>
<style>
    .inner-content {
        border: 1px solid #eaeaea;
        padding: 10px;
        margin-bottom: 20px;
    }

    .grey-bullets li {
        background: url(https://imgd.aeplcdn.com/0x0/bw/static/design15/old-images/d/bw-grey-bullet.png) no-repeat 0px 9px;
        display: block;
        list-style: square outside none;
        padding: 3px 0 3px 10px;
    }

    #get-pq-new select, #get-pq-new textarea {
        width: 170px;
    }

    #div_GetPQ ul li {
        cursor: pointer;
    }
</style>
<div class="main-container">
    <div class="container_12">
        <div class="grid_12">
            <ul class="breadcrumb">
                <li>You are here: </li>
                <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                    <a class="blue" href="/" itemprop="url">
                        <span itemprop="title">Home</span>
                    </a>
                </li>
                <li class="fwd-arrow">&rsaquo;</li>
                <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                    <a class="blue" href="/new-bikes-in-india/" itemprop="url">
                        <span itemprop="title">New</span>
                    </a>
                </li>
                <li class="fwd-arrow">&rsaquo;</li>
                <li class="current">Bike Purchase Offer Claim</li>
            </ul>
            <div class="clear"></div>
        </div>
        <div class="grid_8 margin-top10">
            <h1 class="margin-bottom5">Bike Purchase Offer Claim</h1>
            <div id="div_GetPQ" class="<%= isOfferClaimed ? "hide" : "" %>">
                <div class="inner-content">
                    <h2 class="payment-pg-heading">Share Bike Purchase Details to avail offers</h2>
                    <div>
                        <!-- table starts here -->
                        <div class="steps">
                            <p class="desc-para">Please provide following information about your bike purchase to claim your free gifts. Upon verification of your purchase, we will ship the gift directly to your address.</p>
                            <p class="desc-para">All <span class="required">*</span> marked fields are required</p>
                            <div id="get-pq-new" class="inner-content">
                                <div class="mid-box" id="pq_car">
                                    <table cellspacing="0" cellpadding="3" width="100%" border="0">
                                        <tr>
                                            <td style="width: 280px;">
                                                <b>Booking Ref. No. (e.g. BW201234)</b>
                                            </td>
                                            <td>
                                                <asp:textbox id="txtBookingNum" runat="server"></asp:textbox>
                                                <span id="spnBookingNum" class="error"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 230px;">
                                                <b>Full Name as per Vehicle Registration<span class="error">*</span></b>
                                            </td>
                                            <td>
                                                <asp:textbox id="txtName" runat="server"></asp:textbox>
                                                <span id="spnName" class="error"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <b>Mobile Number<span class="error">*</span></b>
                                            </td>
                                            <td>
                                                <asp:textbox id="txtMobile" maxlength="10" runat="server"></asp:textbox>
                                                <span id="spnMobile" class="error"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <b>Email Id<span class="error">*</span></b>
                                            </td>
                                            <td>
                                                <asp:textbox id="txtEmail" runat="server"></asp:textbox>
                                                <span id="spnEmail" class="error"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td><b>Purchased Bike<span class="error">*</span></b></td>
                                            <td>
                                                <select id="ddlMake" runat="server"></select><input type="hidden" id="hdnMake" runat="server" /></td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td>
                                                <select id="ddlModel">
                                                    <option value="0">--Select Model--</option>
                                                </select><input type="hidden" id="hdnModel" runat="server" /></td>

                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td>
                                                <select id="ddlVersion">
                                                    <option value="0">--Select Version--</option>
                                                </select>
                                                <span id="spnVersion" class="error"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <b>Vehicle Registration Number<span class="error">*</span></b>
                                            </td>
                                            <td>
                                                <asp:textbox id="txtVehicle" runat="server"></asp:textbox>
                                                <span id="spnVehicle" class="error"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <b>Address as per Vehicle Registration<span class="error">*</span></b>
                                            </td>
                                            <td>
                                                <asp:textbox id="txtAddress" rows="2" textmode="MultiLine" runat="server"></asp:textbox>
                                                <span id="spnAddress" class="error"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 230px;">
                                                <b>Pincode<span class="error">*</span></b>
                                            </td>
                                            <td>
                                                <asp:textbox id="txtPincode" runat="server" maxlength="6"></asp:textbox>
                                                <span id="spnPincode" class="error"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <b>Date of vehicle Delivery<span class="error">*</span></b>
                                            </td>
                                            <td>
                                                <%--<BikeWale:Calender DateId="calMakeYear" ID="calDeliveryDate" runat="server" />--%>
                                                <asp:textbox type="text" id="txtPreferredDate" placeholder="Select Date" class="calender" runat="server"></asp:textbox>
                                                <span id="msgMakeYear" class="error"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td><b>Full Name of the Dealer<span class="error">*</span></b></td>
                                            <td>
                                                <asp:textbox id="txtdealerName" runat="server"></asp:textbox>
                                                <span id="spnDealerName" class="error"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <b>Address of the Dealer<span class="error">*</span></b>
                                            </td>
                                            <td>
                                                <asp:textbox id="txtDealerAddress" rows="2" textmode="MultiLine" runat="server"></asp:textbox>
                                                <span id="spnDealerAddress" class="error"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td><b>Any Comments (Optional)</b></td>
                                            <td>
                                                <asp:textbox id="txtComments" rows="2" textmode="MultiLine" runat="server"></asp:textbox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">To complete your offer claim, please send a scanned copy / photograph of the dealer invoice to <a href="mailto:contact@bikewale.com">contact@bikewale.com</a> to verify that the bike has been delivered to you.</td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                            <td>
                                                <asp:button class="action-btn text_white" id="btnSubmit" text="Claim Offer" runat="server" />
                                            </td>
                                        </tr>
                                        <input type="hidden" id="hdnVersion" runat="server" />
                                        <input type="hidden" id="hdnBikeName" runat="server" />
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
        </div>
        <!-- Right Container ends here  -->
    </div>
</div>
<script type="text/javascript">
    $(document).ready(function () {
        $("#txtPreferredDate").datepicker({
            showOn: "both",
            buttonImage: "../image/date-icon.png",
            buttonImageOnly: true,
            dateFormat: 'dd/mm/yy',
            numberOfMonths: 1,
            minDate: '-1y', //days after which dates should be enabled
            maxDate: '+1Y', //max limit months/years to be shown
            firstDay: 1
        });
    });

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
        //$("#errHelmetOffer").text("");

        var customerName = $("#txtName").val();
        var customerEmail = $("#txtEmail").val().toLowerCase();
        var CustomerMobile = $("#txtMobile").val();
        var bikeRegistrationNo = $("#txtVehicle").val();
        var customerAddress = $("#txtAddress").val();
        var customerPincode = $("#txtPincode").val();
        //var deliveryDate = $("#calMakeYear").val();
        var deliveryDate = $("#txtPreferredDate").val();
        var dealerName = $("#txtdealerName").val();
        var dealerAddress = $("#txtDealerAddress").val();
        // var selHelmet = $("#hdnSelHelmet").val();

        var pincodeReg = /^\d{6}$/;

        if (customerName == "") {
            $("#spnName").text("Required");
            isError = true;
        } else if (name.length == 1) {
            $("#spnName").text("Please enter your complete name");
            isError = true;
        } else {
            $("#spnName").text("");
        }

        if (pincodeReg.test(customerPincode) && customerPincode[0] != '0') {
            $("#spnPincode").text("");
        }
        else {
            $("#spnPincode").text("Please Enter six digit Pincode");
            isError = true;
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

        //if (selHelmet == "" || selHelmet <= 0) {
        //    $("#errHelmetOffer").text("Please select helmet.");
        //    isError = true;
        //    $("html, body").animate({ scrollTop: $("#div_GetPQ").offset().top }, 300);
        //} else {
        //    $("#errHelmetOffer").text("");
        //}

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
    //$(".offer-1 li").click(function () {
    //    var panel = $(this).closest(".offer-1");
    //    panel.find("span.bw-sprite").removeClass("checked-radio");
    //    $(this).find("span.bw-sprite").toggleClass("checked-radio", "unchecked-radio");
    //    var panelId = $(this).attr("data-id");
    //    $("#" + panelId).show();
    //    $(".offer-1 li").removeClass("selected-offer");
    //    $(this).addClass("selected-offer");
    //    $("#hdnSelHelmet").val($(this).attr("offerId"));
    //});
    //$('.offer-box-1').hover(function () {
    //    $('.specification-popup-1').toggle(200);
    //});
    ////$('.offer-box-2').hover(function () {
    ////    $('.specification-popup-2').toggle(200);
    ////});
    //$('.offer-box-3').hover(function () {
    //    $('.specification-popup-3').toggle(200);
    //});


</script>
<style type="text/css">
    .ui-datepicker-trigger {
        position: relative;
        top: 6px;
        left: 4px;
    }
</style>
<!-- #include file="/includes/footerInner.aspx" -->
