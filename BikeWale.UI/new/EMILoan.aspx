<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.New.EMILoan" trace="false" EnableEventValidation="false"%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        .red { color:red;}
    </style>
    <link href="/css/style.css?15sep2015" rel="stylesheet" />
    <script type="text/javascript" src="https://st.carwale.com/jquery-1.7.2.min.js?v=1.0" ></script>
    <script type="text/javascript" src="https://stb.aeplcdn.com/bikewale/src/BikeWaleCommon.js?v=3.2"></script>
</head>
<body>
    <div id="divMain">
        <div class="right-float"><span><span class="red">*</span>All fields are mandatory</span></div>
        <div class="clear"></div>
        <table class="tbl-default" cellpadding="0" cellspacing="0" width="100%">
            <tbody>
                <tr>
                    <td style="width:125px;">Name<span class="red">*</span></td>
                    <td><input type="text" id="txtName" runat="server" />&nbsp;<span class="error" id="spnName"></span></td>
                </tr>
                <tr>
                    <td>Email<span class="red">*</span></td>
                    <td><input type="text" id="txtEmail" runat="server" />&nbsp;<span class="error" id="spnEmail"></span></td>
                </tr>
                <tr>
                    <td>
                        <span class="left-float">Mobile No. <span class="red">*</span></span>
                        <span class="right-float">+91-&nbsp;</span>
                        <div class="clear"></div>
                    </td>
                    <td><input type="text" id="txtMobile" maxlength="10" runat="server" />&nbsp;<span class="error" id="spnMobile"></span></td>
                </tr>
                <tr>
                    <td>City<span class="red">*</span></td>
                    <td>
                        <span><asp:DropDownList id="ddlState" runat="server"></asp:DropDownList></span>
                        <span>
                            <asp:DropDownList id="ddlCity" runat="server"><asp:ListItem value="0">--Select City--</asp:ListItem></asp:DropDownList>
                            <asp:HiddenField ID="hdnCityId" runat="server" />
                            &nbsp;<span class="error" id="spnStateCity"></span>
                        </span>
                    </td>
                </tr>
                <tr>
                    <td> </td>
                    <td><input class="action-btn" type="button" ID="btnSubmit" value="Submit"/></td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div id="terms">
                        By clicking on the button above, I agree with <a target="_blank" rel="noopener" href="/visitoragreement.aspx">BikeWale visitor agreement</a> and <a target="_blank" rel="noopener" href="/privacypolicy.aspx">privacy policy</a>.
                        By providing your contact details you agree to be contacted for assistance in your bike buying by us and/or any of our 
                        partners including dealers, bike manufacturers, banks like HDFC bank etc.
                      </div>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
        <div id="divMsg" class="hide"><span>Thank you. Your request has been submitted successfully.</span></div>
        <script type="text/javascript">
            $(document).ready(function () {

                $("#ddlState").change(function () {
                    var stateId = $("#ddlState").val();
                    var requestType = 'ALL';
                    if (stateId > 0) {
                        $.ajax({
                            type: "POST",
                            url: "/ajaxpro/Bikewale.Ajax.AjaxCommon,Bikewale.ashx",
                            data: '{"requestType":"' + requestType + '", "stateId":"' + stateId + '"}',
                            beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetCities"); },
                            success: function (response) {
                                var responseJSON = eval('(' + response + ')');
                                var resObj = eval('(' + responseJSON.value + ')');

                                var dependentCmbs = new Array();
                                bindDropDownList(resObj, $("#ddlCity"), "", dependentCmbs, "--Select City--");
                            }

                        });
                    }
                    else {
                        $("#ddlCity").val("0").prop("disabled", true);
                    }
                });

                $("#btnSubmit").click(function () {
                    if (validateDetails())
                    {
                        var CustName= $("#txtName").val();
                        var custEmail = $("#txtEmail").val();
                        var custMobile = $("#txtMobile").val();
                        var cityId = $("#ddlCity").val();
                        var modelId = '<%=modelId%>';
                        var leadType = "";
                        $.ajax({
                            type: "POST",
                            url: "/ajaxpro/Bikewale.Ajax.AjaxCommon,Bikewale.ashx",
                            data: '{"custName":"' + CustName + '", "email":"' + custEmail + '","mobile":"' + custMobile + '", "modelId":"' + modelId + '","selectedCityId":"' + cityId + '", "leadtype":"' + leadType + '"}',
                            beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "SaveEMIAssistaneRequest"); },
                            success: function (response) {
                                if (response)
                                {
                                    $("#divMain").addClass("hide");
                                    $("#divMsg").removeClass("hide");
                                }
                            }

                        });
                    }
                });
            });

            function validateDetails() {

                $("#spnStateCity").text("");
                $("#spnName").text("");
                $("#spnEmail").text("");
                $("#spnMobile").text("");

                var retVal = true;
                var errorMsg = "";

                var reEmail = /^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,6}$/;
                var reMobile = /^[0-9]*$/;
                var name = /^[a-zA-Z& ]+$/;

                $("#hdnCityId").val($("#ddlCity").val());

                if ($("#ddlCity").val() <= 0) {
                    retVal = false;
                    $("#spnStateCity").text("Please Select City");
                }


                if ($("#ddlState").val() <= 0) {
                    retVal = false;
                    $("#spnStateCity").text("Please Select State");
                }

                if ($("#txtName").val() == "") {
                    retVal = false;
                    $("#spnName").text("Please Enter Name");
                }
                else if (!name.test($("#txtName").val())) {
                    retVal = false;
                    $("#spnName").text("Please Enter valid Name");
                }


                var _email = $("#txtEmail").val().toString().toLowerCase();
                if (_email == "") {
                    retVal = false;
                    $("#spnEmail").text("Please Enter Email");
                }
                else if (!reEmail.test(_email)) {
                    retVal = false;
                    $("#spnEmail").text("Invalid Email");
                }

                var _custMobile = $("#txtMobile").val();
                if (_custMobile == "") {
                    retVal = false;
                    $("#spnMobile").text("Please Enter Mobile Number");
                }
                else if (!reMobile.test(_custMobile)) {
                    retVal = false;
                    $("#spnMobile").text("Mobile No. should be Numeric");
                }
                else if (_custMobile.length != 10) {
                    retVal = false;
                    $("#spnMobile").text("Mobile No. should be of 10 digits");
                }

                return retVal;
            }
    </script>
</body>
</html>
