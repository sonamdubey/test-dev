<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.BikeBooking.RSAOfferClaim" %>
<%@ Register TagPrefix="BikeWale" TagName="Calender" Src="~/UI/controls/DateControl.ascx" %>
<%
    title = "Offer claim form";
    keywords = "";
    description = "";
    canonical = "";
    AdPath = "/1017752/Bikewale_Mobile_PriceQuote";
    AdId = "1398766000399";
    menu = "12";
%>
<!-- #include file="/UI/includes/headermobile_home.aspx" -->
<link rel="stylesheet"  href="/UI/m/css/bw-new-style.css?<%= staticFileVersion %>" />
<script type="text/javascript" src="<%= staticUrl  %>/UI/src/BikeWaleCommon.js?v=3.2"></script>
    <!-- offer claim starts here -->
<div class="margin-bottom10 grid-12">
    <h1>Bike Purchase Offer Claim</h1>
    <div class="<%= isOfferClaimed ? "hide" : "" %>" id="div_GetPQ">
        <!-- new box starts here -->
        <div class="box1 new-line10">
            <h2>Share Bike Purchase Details to avail offers</h2>
            <div class="inner-box">
                <p>Please provide following information about your bike purchase to claim your free gifts. Upon verification of your purchase, we will ship the gift directly to your address.</p>
                <p class="font12 lightgray new-line10">All * marked fields are required.</p>
                <div class="new-line10">
                    <div class="avail-offer-form">
                        <p><b>Booking Ref. No.</b></p>
                        <asp:textbox id="txtBookingNum" runat="server" placeholder="e.g. BW201234"></asp:textbox>
                        <span id="spnBookingNum" class="error"></span>
                    </div>
                    <div class="avail-offer-form">
                        <p><b>Full Name<span class="red-text">*</span></b></p>
                        <asp:textbox id="txtName" runat="server" placeholder="As Per Registration"></asp:textbox>
                        <span id="spnName" class="error"></span>
                    </div>
                    <div class="avail-offer-form">
                        <p><b>Mobile Number<span class="red-text">*</span></b></p>
                        <asp:textbox id="txtMobile" runat="server" maxlength="10" placeholder="As Per Registration"></asp:textbox>
                        <span id="spnMobile" class="error"></span>
                    </div>
                    <div class="avail-offer-form">
                        <p><b>Email Id<span class="red-text">*</span></b></p>
                        <asp:textbox id="txtEmail" runat="server" placeholder="As Per Registration"></asp:textbox>
                        <span id="spnEmail" class="error"></span>
                    </div>
                    <div class="avail-offer-form">
                        <p><b>Purchased Bike<span class="red-text">*</span></b></p>
                        <div class="margin-bottom10">
                            <select id="ddlMake" runat="server"></select>
                        </div>
                        <div class="margin-bottom10">
                            <select id="ddlModel">
                                <option value="0">--Select Model--</option>
                            </select>
                        </div>
                        <div class="margin-bottom10">
                            <select id="ddlVersion">
                                <option value="0">--Select Version--</option>
                            </select>
                            <span id="spnVersion" class="error"></span>
                            <input type="hidden" id="hdnVersion" runat="server" />
                            <input type="hidden" id="hdnBikeName" runat="server" />
                        </div>
                    </div>
                    <div class="avail-offer-form">
                        <p><b>Vehical Registration Number<span class="red-text">*</span></b></p>
                        <asp:textbox id="txtVehicle" runat="server" placeholder="As Per Registration"></asp:textbox>
                        <span id="spnVehicle" class="error"></span>
                    </div>
                    <div class="avail-offer-form">
                        <p><b>Address<span class="red-text">*</span></b></p>
                        <asp:textbox id="txtAddress" rows="2" columns="60" textmode="MultiLine" runat="server" placeholder="As Per Registration"></asp:textbox>
                        <span id="spnAddress" class="error"></span>
                    </div>
                    <div class="avail-offer-form">
                        <p><b>Pincode<span class="red-text">*</span></b></p>
                        <asp:textbox id="txtPincode" runat="server" placeholder="As Per Registration" maxlength="6"></asp:textbox>
                        <span id="spnPincode" class="error"></span>
                    </div>
                    <div class="avail-offer-form">
                        <p><b>Date of Delivery<span class="red-text">*</span></b></p>
                        <BikeWale:Calender DateId="calMakeYear" ID="calDeliveryDate" runat="server" />
                    </div>
                    <div class="avail-offer-form">
                        <p><b>Full Name of the Dealer<span class="red-text">*</span></b></p>
                        <asp:textbox id="txtdealerName" runat="server" placeholder="As Per Registration"></asp:textbox>
                        <span id="spnDealerName" class="error"></span>
                    </div>
                    <div class="avail-offer-form">
                        <p><b>Address of the Dealer<span class="red-text">*</span></b></p>
                        <asp:textbox id="txtDealerAddress" rows="2" columns="60" textmode="MultiLine" runat="server" placeholder="As Per Registration"></asp:textbox>
                        <span id="spnDealerAddress" class="error"></span>
                    </div>
                    <div class="avail-offer-form">
                        <p><b>Any Comments (optional)</b></p>
                        <asp:textbox id="txtComments" rows="2" columns="60" textmode="MultiLine" runat="server" placeholder="As Per Registration"></asp:textbox>
                    </div>
                    <div>
                        <p>To complete your offer claim, please send a scanned copy / photograph of the dealer invoice to <a href="mailto:contact@bikewale.com">contact@bikewale.com</a> to verify that the bike has been delivered to you.</p>
                    </div>
                    <div class="avail-offer-form margin-top20" data-role="none">
                        <asp:button class="rounded-corner5" id="btnSubmit" text="Claim Offer" runat="server" data-role="none" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="RSAMessage" runat="server" class="box1 new-line10 box-min-height"></div>
</div>
    <!-- offer claim starts here -->
    <script type="text/javascript">
        $(document).ready(function () {                        
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

                var customerName = $("#txtName").val();
                var customerEmail = $("#txtEmail").val().toLowerCase();
                var CustomerMobile = $("#txtMobile").val();
                var bikeRegistrationNo = $("#txtVehicle").val();
                var customerAddress = $("#txtAddress").val();
                var customerPincode = $("#txtPincode").val();
                var deliveryDate = $("#calMakeYear").val();
                var dealerName = $("#txtdealerName").val();
                var dealerAddress = $("#txtDealerAddress").val();
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

                if (pincodeReg.test(customerPincode) && customerPincode[0] != '0') {
                    $("#spnPincode").text("");
                }
                else {
                    $("#spnPincode").text("Please Enter six digit Pincode");
                    isError = true;
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
        });
    </script>
<!-- #include file="/UI/includes/footermobile_home.aspx" -->
